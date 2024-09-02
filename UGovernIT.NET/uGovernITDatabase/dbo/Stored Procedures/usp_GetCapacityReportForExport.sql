ALTER Procedure [dbo].[usp_GetCapacityReportForExport]
 @TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069' ,
 @Fromdate datetime='2022-01-01 00:00:00.000',
 @Todate datetime='2023-01-01 00:00:00.000',
 @mode varchar(10) ='Monthly',
 @Closed bit= 'False',
 @type varchar(15) = 'COUNT', --'FTE', --'PERCENT', --'FTE', --, --'COUNT',  --'AVAILABILITY'
 @Department varchar(2000)='',
 @IncludeAllResources bit='False',
 @url varchar(500)='',
@Category varchar(2000)='JobTitle',
--@Category varchar(2000)='Role',
--@Category varchar(2000)='UnfilledRoles',
 @AllocationType varchar(50)='Estimated',
 @LevelName VARCHAR(100)='',
 @Filter VARCHAR(100)='',
 @Studio VARCHAR(100)='',
 @Division INT=0,
 @Sector VARCHAR(100)='',
 @SoftAllocation bit=0,
 @OnlyNCO bit=0
as
BEGIN
DECLARE @cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = N'' ,@sumcols   AS NVARCHAR(MAX)
SET @cols = STUFF((SELECT ',' + QUOTENAME(months) 
            FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode) 
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 

        ,1,1,'')
SET @sumcols = STUFF((SELECT ',' + 'ISNULL(SUM(' + QUOTENAME(months) + '),0)' + QUOTENAME(months) 
    FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode) 
    FOR XML PATH(''), TYPE
    ).value('.', 'NVARCHAR(MAX)') 
,1,1,'')

Create table #tblJobProfile
(
JobProfile nvarchar(500),
[Count] int,
UserList nvarchar(max)
)

If @IncludeAllResources = 0
Begin
	Insert into #tblJobProfile 
	SELECT  JobProfile, count(JobProfile), dbo.GetUsersByJobProfile(JobProfile, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate)) from AspNetUsers where TenantID = @TenantID
	and Id in (select distinct ResourceUser from ResourceAllocationMonthly  where TenantID = @TenantID and Year(MonthStartDate) = Year(@Fromdate))
	and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	and LEN(JobProfile) > 0  and Enabled = 1 group by JobProfile;
End
Else
Begin
	Insert into #tblJobProfile 
	SELECT  JobProfile, count(JobProfile), dbo.GetUsersByJobProfile(JobProfile, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate)) from AspNetUsers where TenantID = @TenantID
	and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	 group by JobProfile;
End
--update #tblJobProfile set UserList = REPLACE(UserList,',',CHAR(10));

	--select * from #tblJobProfile;
--print @sumcols;

Create table #tblRole
(
RoleId nvarchar(500),
Role nvarchar(500),
[Count] int,
UserList nvarchar(max)
)


If @IncludeAllResources = 0
Begin
	 Insert into #tblRole
     SELECT  r.Id,r.Name, count(r.Name), dbo.GetUsersByRoleId(r.Id, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate))
	 from AspNetUsers u join Roles r on r.Id = u.GlobalRoleID
	 where u.TenantID = @TenantID
	 and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	 and LEN(u.GlobalRoleID) > 0 and u.Enabled = 1 group by r.Id, r.Name order by r.Name
End
Else
Begin
	Insert into #tblRole
     SELECT  r.Id,r.Name, count(r.Name), dbo.GetUsersByRoleId(r.Id, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate))
	 from AspNetUsers u join Roles r on r.Id = u.GlobalRoleID
	 where u.TenantID = @TenantID
	 and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	  group by r.Id, r.Name order by r.Name
End



	-- select * from #tblRole;

	Create table #tblProjectCapacity
	(
	JobProfile nvarchar(500),
	ProjectCapacity int
	)

	If @Closed = 1 and @IncludeAllResources = 0
	Begin
		Insert into #tblProjectCapacity
		SELECT  u.JobProfile, sum(s.AllCount) from AspNetUsers u
		left join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		where u.TenantID = @TenantID
		and LEN(u.JobProfile) > 0 and u.Enabled = 1 group by u.JobProfile
	 End
	Else If @Closed = 1 and @IncludeAllResources = 1
	Begin
		Insert into #tblProjectCapacity
		SELECT  u.JobProfile, sum(s.AllCount) from AspNetUsers u
		left join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		where u.TenantID = @TenantID
		and u.Enabled = 1 group by u.JobProfile
	 End
	 Else If @Closed = 0 and @IncludeAllResources = 0
	 Begin
	 	Insert into #tblProjectCapacity
		SELECT  u.JobProfile, sum(s.Count) from AspNetUsers u
		left join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		where u.TenantID = @TenantID
		and LEN(u.JobProfile) > 0 and u.Enabled = 1 group by u.JobProfile
	 End
	 Else If @Closed = 0 and @IncludeAllResources = 1
	 Begin
	 	Insert into #tblProjectCapacity
		SELECT  u.JobProfile, sum(s.Count) from AspNetUsers u
		left join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		where u.TenantID = @TenantID
		and u.Enabled = 1 group by u.JobProfile
	 End
	--select * from #tblProjectCapacity;

	----------------------
	Create table #tblProjectCapacityByRole
	(
	RoleId nvarchar(500),
	Role nvarchar(500),
	ProjectCapacity int
	)

	If @Closed = 1 and @IncludeAllResources = 0
	Begin
		Insert into #tblProjectCapacityByRole
		SELECT r.Id, r.Name, sum(s.AllCount) from AspNetUsers u
		join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		join Roles r on r.Id = u.GlobalRoleID
		where u.TenantID = @TenantID
		and LEN(u.GlobalRoleID) > 0 and u.Enabled = 1 group by r.Id, r.Name
	 End
	Else If @Closed = 1 and @IncludeAllResources = 1
	Begin
		Insert into #tblProjectCapacityByRole
		SELECT r.Id, r.Name, sum(s.AllCount) from AspNetUsers u
		join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		join Roles r on r.Id = u.GlobalRoleID
		where u.TenantID = @TenantID
		and u.Enabled = 1 group by r.Id, r.Name
	 End
	 Else If @Closed = 0 and @IncludeAllResources = 0
	 Begin
		Insert into #tblProjectCapacityByRole
		SELECT  r.Id, r.Name, sum(s.Count) from AspNetUsers u
		join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		join Roles r on r.Id = u.GlobalRoleID
		where u.TenantID = @TenantID
		and LEN(u.GlobalRoleID) > 0 and u.Enabled = 1 group by r.Id, r.Name
	 End
	 Else If @Closed = 0 and @IncludeAllResources = 1
	 Begin
		Insert into #tblProjectCapacityByRole
		SELECT  r.Id, r.Name, sum(s.Count) from AspNetUsers u
		join Summary_ResourceProjectComplexity s on s.UserId = u.Id
		join Roles r on r.Id = u.GlobalRoleID
		where u.TenantID = @TenantID
		and u.Enabled = 1 group by r.Id, r.Name
	 End
	--select * from #tblProjectCapacityByRole;
	----------------------

IF(@Category='JobTitle')
Begin
	IF(@type='COUNT')
	Begin

		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  JobProfile asc) ItemOrder, 
		JobProfile  JobTitle,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	u.JobProfile, jp.[Count], jp.UserList, pc.ProjectCapacity,		  
		
		Count(a.ResourceUser) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacity pc on pc.JobProfile = u.JobProfile	
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,u.JobProfile, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
			
	End
	Else If(@type='FTE' or @type='ALLOCATION')
	Begin
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  JobProfile asc) ItemOrder,
		JobProfile JobTitle,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	u.JobProfile, jp.[Count], jp.UserList, pc.ProjectCapacity,		  
		
		Round((sum(a.PctAllocation)/100),2)  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacity pc on pc.JobProfile = u.JobProfile	
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,u.JobProfile, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
				
	End
	Else IF(@type='PERCENT')
	Begin
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  JobProfile asc) ItemOrder, 
		JobProfile JobTitle,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	u.JobProfile, jp.[Count], jp.UserList, pc.ProjectCapacity,		  
		
		Convert(varchar,Round((sum(a.PctAllocation)*100/100)/jp.[Count],0)) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacity pc on pc.JobProfile = u.JobProfile	
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,u.JobProfile, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
				
	End
	Else If(@type='AVAILABILITY')
	Begin
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  JobProfile asc) ItemOrder,
		JobProfile JobTitle,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	u.JobProfile, jp.[Count], jp.UserList, pc.ProjectCapacity,	  
		
		Round(jp.[Count]-(sum(a.PctAllocation)/100),2)  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacity pc on pc.JobProfile = u.JobProfile	
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,u.JobProfile, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
				
	End
	   	 	
End
Else If(@Category='Role')
Begin
	IF(@type='COUNT')
	BEGIN
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  Role asc) ItemOrder ,
	--	Role as Role,
		Role Role,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	jp.[Role],jp.RoleId, jp.[Count], jp.UserList, pc.ProjectCapacity,	
		Count(a.ResourceUser) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacityByRole pc on pc.RoleId = u.GlobalRoleID	
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,jp.[Role], jp.RoleId,pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
		
	END
	ELSE IF(@type='PERCENT')
	BEGIN
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  Role asc) ItemOrder,
	--	Role as Role,
		Role  Role,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	jp.[Role],jp.RoleId, jp.[Count], jp.UserList, pc.ProjectCapacity,		  
		
		Convert(varchar,Round((sum(a.PctAllocation)*100/100)/jp.[Count],0)) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacityByRole pc on pc.RoleId = u.GlobalRoleID	
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,jp.[Role],jp.RoleId, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
				
	END
	ELSE IF(@type='FTE' OR @type='ALLOCATION')
	BEGIN
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  Role asc) ItemOrder,
	--	Role as Role,
		Role Role,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	jp.[Role],jp.RoleId, jp.[Count], jp.UserList, pc.ProjectCapacity,	  
		
		Round((sum(a.PctAllocation)/100),2) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacityByRole pc on pc.RoleId = u.GlobalRoleID	
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,jp.[Role],jp.RoleId, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
	
	END
	ELSE IF(@type='AVAILABILITY')
	BEGIN
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  Role asc) ItemOrder, 
	--	Role as Role,
		Role Role,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	jp.[Role],jp.RoleId, jp.[Count], jp.UserList, pc.ProjectCapacity,		  
		
		Round(jp.[Count]-(sum(a.PctAllocation)/100),2)  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacityByRole pc on pc.RoleId = u.GlobalRoleID	
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,jp.[Role],jp.RoleId, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
	END
End
Else If(@Category='UnfilledRoles')
Begin
	IF(@type='COUNT')
	BEGIN			
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 

		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		
		ResourceQuantity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem,

		Count(a.ResourceSubWorkItem)  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
	
	END
	ELSE IF(@type='PERCENT')
	BEGIN
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 
		
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		
		ResourceQuantity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 

		Convert(varchar,Round(sum(a.PctAllocation),0)) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';

	END
	ELSE IF(@type='FTE' OR @type='ALLOCATION')
	BEGIN
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 
		
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		ResourceQuantity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 
		
		Cast(Sum(a.PctAllocation)/100 as decimal(8,2))  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';

	END
	ELSE IF(@type='AVAILABILITY')
	BEGIN
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 

		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		ResourceQuantity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 
		
		Cast(1-Sum(a.PctAllocation)/100 as decimal(8,2))  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
		
	END
End

--If(@IncludeAllResources='False')
--begin
--Set @SQLStatement =@SQLStatement +' and (Select Sum(count) from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+''' and userid =u.Id) is not null
--and t.ResourceUserAllocated is not null'
--End

Print(@SQLStatement)
EXEC(@SQLStatement)

------------
--  Below code for Footer
SET @SQLStatement = '';
IF(@Category='JobTitle')
	BEGIN
		SET @SQLStatement = 'Select  1 ItemOrder , '''' JobTitle,
		SUM([Count]) ResourceQuantity,
		1 ProjectCapacity,
		'+@sumcols+ '
		from (
		Select 	u.JobProfile, jp.[Count],	
		Round(sum(a.PctAllocation)/100,2)  PctAllocation,
		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,u.JobProfile, jp.[Count]
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
	END
	ELSE IF(@Category='Role')
	BEGIN
		SET @SQLStatement = 'Select  1 ItemOrder , '''' as Role,
		SUM([Count]) ResourceQuantity,
		1 ProjectCapacity,
		'+@sumcols+ '
		from (
		Select 	jp.[Role], jp.[Count],
		Round(sum(a.PctAllocation)/100,2)  PctAllocation,
		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,jp.[Role], jp.[Count]
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
	END
	ELSE IF(@Category='UnfilledRoles')
	BEGIN
		SET @SQLStatement ='Select 1 ItemOrder ,
		'''' as Role, 
		count(ResourceSubWorkItem) ResourceQuantity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@sumcols+ 'from (
		Select a.ResourceSubWorkItem, 		
		Round(Sum(a.PctAllocation)/100,2) PctAllocation,
		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
	END

Print(@SQLStatement)
EXEC(@SQLStatement)

---Below code to return Percentages only
set @SQLStatement = '';
IF(@Category='JobTitle')
Begin

		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  JobProfile asc) ItemOrder,
		JobProfile JobTitle,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	u.JobProfile, jp.[Count], jp.UserList, pc.ProjectCapacity,		  
		
		Round((sum(a.PctAllocation)*100/100)/jp.[Count],0)  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacity pc on pc.JobProfile = u.JobProfile	
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,u.JobProfile, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
			
End
Else If(@Category='Role')
Begin
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  Role asc) ItemOrder,
	--	Role as Role,
		Role Role,
		[Count] ResourceQuantity,
		ProjectCapacity ProjectCapacity,
		'+@cols+ '
		from (
		Select 	jp.[Role],jp.RoleId, jp.[Count], jp.UserList, pc.ProjectCapacity,	  
		
		Round((sum(a.PctAllocation)*100/100)/jp.[Count],0) PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblProjectCapacityByRole pc on pc.RoleId = u.GlobalRoleID	
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,jp.[Role],jp.RoleId, pc.ProjectCapacity, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
	
End
Else If(@Category='UnfilledRoles')
Begin	
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 
		
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		ResourceQuantity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 
		
		Round(sum(a.PctAllocation),0)  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';

End

Print(@SQLStatement)
EXEC(@SQLStatement)

Drop table #tblJobProfile;
Drop table #tblProjectCapacity;
Drop table #tblRole;
Drop table #tblProjectCapacityByRole;

END

--  usp_GetCapacityReport



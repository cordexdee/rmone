CREATE Procedure [dbo].[usp_GetCapacityReport]
 @TenantID varchar(128), --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069' ,
 @Fromdate datetime,
 @Todate datetime,
 @mode varchar(10) ='Monthly',
 @Closed bit,
 @type varchar(15), --'FTE', --'PERCENT', --'FTE', --, --'COUNT',  --'AVAILABILITY'
 @Department varchar(2000),
 @IncludeAllResources bit,
 @url varchar(500),
 @LevelName varchar(50),  
 @Category varchar(2000),
--@Category varchar(2000)='UnfilledRoles',
 @AllocationType varchar(50)='Estimated',
 @Filter nvarchar(250),
 @Studio nvarchar(250),
 @Division bigint,
 @Sector nvarchar(250),
 @SoftAllocation bit,
 @OnlyNCO bit
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

SELECT * INTO #tmpTickets
FROM 
	(
	select TicketId, ApproxContractValue, Closed, Status from CRMProject  where   TenantID = @TenantID and Status != 'Cancelled'  
	Union all 
	select TicketId, ApproxContractValue, Closed, Status from  Opportunity where  TenantID = @TenantID and Status != 'Cancelled' and Closed = 0
	Union all 
	select TicketId, ApproxContractValue, Closed, Status from  CRMServices  where  TenantID = @TenantID and Status != 'Cancelled'
	) t1;

CREATE NONCLUSTERED INDEX [IX_tmpTickets_TicketId]
ON #tmpTickets(TicketId);

Create table #tblJobProfile
(
JobProfile nvarchar(500),
[Count] int,
UserList nvarchar(max),
UserIdList nvarchar(max)
)

If @IncludeAllResources = 0
Begin
	Insert into #tblJobProfile 
	SELECT  JobProfile, count(JobProfile), STRING_AGG(u.Name, ', '),--dbo.GetUsersByJobProfile(JobProfile, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed),
	STRING_AGG(u.Id, ',')--dbo.GetUsersIdByJobProfile(JobProfile, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed) from AspNetUsers where TenantID = @TenantID
	from AspNetUsers u where TenantID = @TenantID
	and Id in (select distinct ResourceUser from ResourceAllocationMonthly  where TenantID = @TenantID and Year(MonthStartDate) = Year(@Fromdate) 
		and ResourceWorkitem in (SELECT case when  CONVERT(varchar, @Closed) = '1' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets))
	and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	and LEN(JobProfile) > 0  and Enabled = 1 group by JobProfile;
End
Else
Begin
	Insert into #tblJobProfile 
	SELECT  JobProfile, count(JobProfile),STRING_AGG(u.Name, ', '), --dbo.GetUsersByJobProfile(JobProfile, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed), 
	STRING_AGG(u.Id, ',')--dbo.GetUsersIdByJobProfile(JobProfile, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed)
	from AspNetUsers u where TenantID = @TenantID
	and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	and Id in (select distinct ResourceUser from ResourceAllocationMonthly  where TenantID = @TenantID and Year(MonthStartDate) = Year(@Fromdate) 
		and ResourceWorkitem in (SELECT case when  CONVERT(varchar, @Closed) = '1' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets))
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
UserList nvarchar(max),
UserIdList nvarchar(max)
)


If @IncludeAllResources = 0
Begin
	 Insert into #tblRole
     SELECT  r.Id,r.Name, count(r.Name),STRING_AGG(u.Name, ', '), --dbo.GetUsersByRoleId(r.Id, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed),
	 STRING_AGG(u.Id, ',') --dbo.GetUsersIdByRoleId(r.Id, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed)
	 from AspNetUsers u join Roles r on r.Id = u.GlobalRoleID
	 where u.TenantID = @TenantID
	 and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	 and LEN(u.GlobalRoleID) > 0 and u.Enabled = 1 
	 and u.Id in (select distinct ResourceUser from ResourceAllocationMonthly  where TenantID = @TenantID and Year(MonthStartDate) = Year(@Fromdate) 
		and ResourceWorkitem in (SELECT case when  CONVERT(varchar, @Closed) = '1' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets))
		and u.Enabled = 1
	 group by r.Id, r.Name order by r.Name
End
Else
Begin
	Insert into #tblRole
     SELECT  r.Id,r.Name, count(r.Name), STRING_AGG(u.Name, ', '), --dbo.GetUsersByRoleId(r.Id, @TenantID, ISNULL(@Department,'0'),@IncludeAllResources, Year(@Fromdate), @Closed),
		STRING_AGG(u.Id, ',')
	 from AspNetUsers u join Roles r on r.Id = u.GlobalRoleID
	 where u.TenantID = @TenantID
	 and ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
	  and u.Id in (select distinct ResourceUser from ResourceAllocationMonthly  where TenantID = @TenantID and Year(MonthStartDate) = Year(@Fromdate) 
		and ResourceWorkitem in (SELECT case when  CONVERT(varchar, @Closed) = '1' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets))
	  group by r.Id, r.Name order by r.Name
End

IF(@Category='JobTitle')
Begin
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  JobProfile asc) ItemOrder, 
		''<div style=cursor:pointer; onclick=window.parent.UgitOpenPopupDialog('''''+@url+'?control=resourceavailability&AllocationViewType=CapacityReport&RAdisplayMode='+CONVERT(varchar,@mode)+'&pStartDate='+Format(@Fromdate,'dd/MM/yyyy')+'&pEndDate='+Format(@Todate,'dd/MM/yyyy')+'&pDepartmentName='+CONVERT(varchar,@Department)+'&JobProfile=''+ Replace(JobProfile, '' '', ''%20'') +''&isdlg=1&isudlg=1'''',''''Module=RMM&Status=resourceutilization&showalldetail=false;showglobalfilter=true'''',''''Resource&nbsp;Utilization'''',96,90,0,'''''''') >''
			 + JobProfile + ''</div>'' JobTitle,
		''<div title="''+ UserList +''">'' + convert(varchar, [Count]) +  ''</div>'' ResourceQuantity,
		ProjectCapacity ProjectCapacity,RevenueCapacity RevenueCapacity,
		'+@cols+ '
		from (
		Select 	u.JobProfile, jp.[Count], jp.UserList, 
		(Select Count(distinct(x.Workitem)) from ResourceUsageSummaryMonthWise x Left join ResourceAllocation ra
			on ra.ResourceWorkItemLookup = x.WorkItemID where x.TenantID=''' + @Tenantid + '''
			and x.Workitem != ''PTO HR'' and x.ActualStartDate is not null 
			and x.resourceUser in (select * from  STRING_SPLIT(jp.UserIdList, '',''))
			and ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else ra.SoftAllocation  END
			and ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then ra.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
			and x.Workitem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		if (@Closed = 0)
		Begin	
			SET @SQLStatement = @SQLStatement + ' and x.monthstartdate >=''' + CONVERT(varchar, @Fromdate, 121)+ ''' and x.monthstartdate  <=''' + CONVERT(varchar, @Todate, 121)+ ''''
		end

		SET @SQLStatement = @SQLStatement + '
		)''ProjectCapacity'',
		(Select (CONVERT (VARCHAR(10), (CAST(ROUND(ISNULL(SUM(rc.ApproxContractValue),0) / 1000000.0, 2) AS NUMERIC(6, 1))))) from (
			Select x.Workitem, y.ApproxContractValue from ResourceUsageSummaryMonthWise x Left join #tmpTickets y 
			on x.Workitem = y.TicketId 
			left join ResourceAllocation ra
			on ra.ResourceWorkItemLookup = x.WorkItemID 
			where x.TenantID=''' + @Tenantid + '''
			and x.Workitem != ''PTO HR'' and x.ActualStartDate is not null 
			and x.resourceUser in (select * from  STRING_SPLIT(jp.UserIdList, '',''))
			and ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else ra.SoftAllocation  END
			and ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then ra.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
			'
			if (@Closed = 0)
			Begin	
				SET @SQLStatement = @SQLStatement + ' and y.Closed = 0 and x.monthstartdate >=''' + CONVERT(varchar, @Fromdate, 121)+ ''' and x.monthstartdate  <=''' + CONVERT(varchar, @Todate, 121)+ ''''
			end

			SET @SQLStatement = @SQLStatement + ' group by x.Workitem, y.ApproxContractValue)rc )''RevenueCapacity'',
		
		''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&capacityreport=true&jobTitleLookup=''+ Replace(u.JobProfile, '' '', ''%20'') +''&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirectFromCardView=true&startDate=''+ convert(varchar(10),a.MonthStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+''&IncludeClosed=False'''',''''Resource&nbsp;Utilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>
		<div style=color:#fff;''
		+Case when (Round((sum(a.PctAllocation))/jp.[Count],0)) >= 120 then ''background-color:#F9AA33;>'' 
		When ((Round((sum(a.PctAllocation))/jp.[Count],0)) >=75 and (Round((sum(a.PctAllocation))/jp.[Count],0)) <120) then ''background-color:#6BA538;>'' 
		When ((Round((sum(a.PctAllocation))/jp.[Count],0)) >=40 and (Round((sum(a.PctAllocation))/jp.[Count],0)) <75) then ''background-color:#ACB8C0;>'' 
		When ((Round((sum(a.PctAllocation))/jp.[Count],0)) >=0 and (Round((sum(a.PctAllocation))/jp.[Count],0)) <40) then ''background-color:#FF5757;>'' 
		else ''display:none;>'' end'
		IF(@type='COUNT')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,Count(a.ResourceUser)) +''</div></a>''  PctAllocation,'
		END
		ELSE IF(@type='PERCENT')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,Round((sum(a.PctAllocation))/jp.[Count],0)) + ''%</div></a>''  PctAllocation,'
		END
		ELSE IF(@type='HOURS')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar, Round(sum(a.AllocationHour),0)) +''</div></a>''  PctAllocation,'
		END
		IF(@type='FTE' OR @type='ALLOCATION')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,Round((sum(a.PctAllocation)/100),2)) +''</div></a>''  PctAllocation,'
		END
		ELSE IF(@type='AVAILABILITY')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,(Case when ((100*jp.[Count])-(sum(a.PctAllocation)))<0  then 0 else Round((((100*jp.[Count])-(sum(a.PctAllocation)))/jp.[Count]),2) end)) + ''%</div></a>''  PctAllocation,'
		END

		SET @SQLStatement = @SQLStatement + 'Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceUsageSummaryMonthWise a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblJobProfile jp on jp.JobProfile = u.JobProfile
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.WorkItemID
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.JobProfile)>0
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate'
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and WorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and WorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
		AND u.Enabled = CASE WHEN '''+CONVERT(varchar,@IncludeAllResources)+'''= 0 THEN 1 ELSE u.Enabled  END
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		and ((not(year(u.UGITStartDate) < year(''' + CONVERT(varchar, @Fromdate, 121)+ ''')  and year(u.UGITEndDate) < year(''' + CONVERT(varchar, @Fromdate, 121)+ '''))
		or (year(u.UGITStartDate) > year(''' + CONVERT(varchar, @Fromdate, 121)+ ''') and year(u.UGITEndDate) > year(''' + CONVERT(varchar, @Fromdate, 121)+ '''))))
		group by a.MonthStartDate,u.JobProfile, jp.UserIdList, jp.[Count], jp.UserList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
End
Else If(@Category='Role')
Begin
		SET @SQLStatement = 'Select  ROW_NUMBER() OVER(order by  Role asc) ItemOrder ,
		--Role as Role,
		''<div style=cursor:pointer; onclick=window.parent.UgitOpenPopupDialog('''''+@url+'?control=resourceavailability&AllocationViewType=CapacityReport&RAdisplayMode='+CONVERT(varchar,@mode)+'&pStartDate='+Format(@Fromdate,'dd/MM/yyyy')+'&pEndDate='+Format(@Todate,'dd/MM/yyyy')+'&pDepartmentName='+CONVERT(varchar,@Department)+'&GlobalRoleId=''+ Replace(RoleId, '' '', ''%20'') +''&isdlg=1&isudlg=1'''',''''Module=RMM&Status=resourceutilization&showalldetail=false;showglobalfilter=true'''',''''Resource&nbsp;Utilization'''',96,90,0,'''''''') >''
		+ Role + ''</div>'' Role,
		''<div title="''+ UserList +''">'' + convert(varchar, [Count]) + ''</div>'' ResourceQuantity,
		ProjectCapacity ProjectCapacity,RevenueCapacity RevenueCapacity,
		'+@cols+ '
		from (
		Select 	jp.[Role],jp.RoleId, jp.[Count], jp.UserList, 
		(Select Count(distinct(x.Workitem)) from ResourceUsageSummaryMonthWise x Left join ResourceAllocation ra
			on ra.ResourceWorkItemLookup = x.WorkItemID where x.TenantID=''' + @Tenantid + '''
			and x.Globalroleid =jp.RoleId and x.Workitem != ''PTO HR'' and x.ActualStartDate is not null 
			and x.resourceUser in (select * from  STRING_SPLIT(jp.UserIdList, '',''))
			and ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else ra.SoftAllocation  END
			and ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then ra.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
			and x.Workitem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		if (@Closed = 0)
		Begin	
			SET @SQLStatement = @SQLStatement + ' and x.monthstartdate >=''' + CONVERT(varchar, @Fromdate, 121)+ ''' and x.monthstartdate  <=''' + CONVERT(varchar, @Todate, 121)+ ''''
		end

		SET @SQLStatement = @SQLStatement + '
		)''ProjectCapacity'',
		(Select (CONVERT (VARCHAR(10), (CAST(ROUND(ISNULL(SUM(rc.ApproxContractValue),0) / 1000000.0, 2) AS NUMERIC(6, 1))))) from (
			Select x.Workitem, y.ApproxContractValue from ResourceUsageSummaryMonthWise x Left join #tmpTickets y 
			on x.Workitem = y.TicketId
			left join ResourceAllocation ra
			on ra.ResourceWorkItemLookup = x.WorkItemID 
			where x.TenantID=''' + @Tenantid + '''
			and x.Workitem != ''PTO HR'' and x.ActualStartDate is not null 
			and x.resourceUser in (select * from  STRING_SPLIT(jp.UserIdList, '',''))
			and ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else ra.SoftAllocation  END
			and ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then ra.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
			'
		if (@Closed = 0)
			Begin	
				SET @SQLStatement = @SQLStatement + ' and y.Closed = 0 and x.monthstartdate >=''' + CONVERT(varchar, @Fromdate, 121)+ ''' and x.monthstartdate  <=''' + CONVERT(varchar, @Todate, 121)+ ''''
			end

		SET @SQLStatement = @SQLStatement + ' group by x.Workitem, y.ApproxContractValue)rc )''RevenueCapacity'',
		
		''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&capacityreport=true&roleLookup=''+ jp.RoleId +''&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirectFromCardView=true&startDate=''+ convert(varchar(10),a.MonthStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+''&IncludeClosed=False'''',''''Resource&nbsp;Utilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>
		<div style=color:#fff;''
		+Case when (Round((sum(a.PctAllocation))/jp.[Count],0)) >= 120 then ''background-color:#F9AA33;>'' 
		When ((Round((sum(a.PctAllocation))/jp.[Count],0)) >=75 and (Round((sum(a.PctAllocation))/jp.[Count],0)) <120) then ''background-color:#6BA538;>'' 
		When ((Round((sum(a.PctAllocation))/jp.[Count],0)) >=40 and (Round((sum(a.PctAllocation))/jp.[Count],0)) <75) then ''background-color:#ACB8C0;>'' 
		When ((Round((sum(a.PctAllocation))/jp.[Count],0)) >=0 and (Round((sum(a.PctAllocation))/jp.[Count],0)) <40) then ''background-color:#FF5757;>'' 
		else ''display:none;>'' end'
		IF(@type='COUNT')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,Count(a.ResourceUser)) +''</div></a>''  PctAllocation,'
		END
		ELSE IF(@type='PERCENT')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,Round((sum(a.PctAllocation))/jp.[Count],0)) + ''%</div></a>''  PctAllocation,'
		END
		ELSE IF(@type='HOURS')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar, Round(sum(a.AllocationHour),0)) +''</div></a>''  PctAllocation,'
		END
		IF(@type='FTE' OR @type='ALLOCATION')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,Round((sum(a.PctAllocation)/100),2)) +''</div></a>''  PctAllocation,'
		END
		ELSE IF(@type='AVAILABILITY')
		BEGIN
			SET @SQLStatement = @SQLStatement + '+ Convert(varchar,(Case when ((100*jp.[Count])-(sum(a.PctAllocation)))<0  then 0 else Round((((100*jp.[Count])-(sum(a.PctAllocation)))/jp.[Count]),2) end)) + ''%</div></a>''  PctAllocation,'
		END

		SET @SQLStatement = @SQLStatement + 'Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceUsageSummaryMonthWise a
		left join AspNetUsers u on u.id=a.ResourceUser and u.TenantID='''+@TenantID+'''
		left join #tblRole jp on jp.RoleId = u.GlobalRoleID
		inner JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.WorkItemID
		where a.TenantID='''+@TenantID+'''
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <= '''+ CONVERT(varchar, @Todate,121)+ '''
		and len(u.GlobalRoleID)>0
		AND u.Enabled = CASE WHEN '''+CONVERT(varchar,@IncludeAllResources)+'''= 0 THEN 1 ELSE u.Enabled  END
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate
		and jp.[Role] !='''''
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and WorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and WorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
		and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		and ((not(year(u.UGITStartDate) < year(''' + CONVERT(varchar, @Fromdate, 121)+ ''')  and year(u.UGITEndDate) < year(''' + CONVERT(varchar, @Fromdate, 121)+ '''))
		or (year(u.UGITStartDate) > year(''' + CONVERT(varchar, @Fromdate, 121)+ ''') and year(u.UGITEndDate) > year(''' + CONVERT(varchar, @Fromdate, 121)+ '''))))
		group by a.MonthStartDate,jp.[Role], jp.RoleId, jp.[Count], jp.UserList, jp.UserIdList
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p'
End
Else If(@Category='UnfilledRoles')
Begin
	IF(@type='COUNT')
	BEGIN			
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 
		''<div title="Unfilled Roles">'' +
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		+ ''</div>''
		ResourceQuantity,'''' RevenueCapacity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem,

		''<a style=cursor:pointer; onclick=FindUnfilledAllocationsByRole(''''''+r.Id+'''''',''''''+convert(varchar(10),a.MonthStartDate,121)+'''''',''''''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+'''''','''''+CONVERT(varchar,@SoftAllocation)+'''''''+'');>''
		+Case when (Round(sum(a.PctAllocation),0)) >= 120 then ''<div style=background-color:#F9AA33;color:#fff>''+Convert(varchar,Count(a.ResourceSubWorkItem))+''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=75 and (Round(sum(a.PctAllocation),0)) <120) then ''<div style=background-color:#6BA538;color:#fff>''+ Convert(varchar,Count(a.ResourceSubWorkItem))+''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=40 and (Round(sum(a.PctAllocation),0)) <75) then ''<div style=background-color:#ACB8C0;color:#fff>''+ Convert(varchar,Count(a.ResourceSubWorkItem))+''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=0 and (Round(sum(a.PctAllocation),0)) <40) then ''<div style=background-color:#FF5757;color:#fff>''+ Convert(varchar,Count(a.ResourceSubWorkItem))+''</div>'' 
		else ''<div>''+Convert(varchar,Count(a.ResourceSubWorkItem))+''</div>'' end + ''</a>''  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		left join Roles r on r.Name = a.ResourceSubWorkItem		
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate
		and a.ResourceSubWorkItem != '''''
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
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
		''<div title="Unfilled Roles">'' +
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		+ ''</div>''
		ResourceQuantity,'''' RevenueCapacity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 

		''<a style=cursor:pointer; onclick=FindUnfilledAllocationsByRole(''''''+r.Id+'''''',''''''+convert(varchar(10),a.MonthStartDate,121)+'''''',''''''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+'''''','''''+CONVERT(varchar,@SoftAllocation)+'''''''+'');>''
		+Case when (Round(sum(a.PctAllocation),0)) >= 120 then ''<div style=background-color:#F9AA33;color:#fff>''+Convert(varchar,Round(sum(a.PctAllocation),0))+''%</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=75 and (Round(sum(a.PctAllocation),0)) <120) then ''<div style=background-color:#6BA538;color:#fff>''+ Convert(varchar,Round(sum(a.PctAllocation),0))+''%</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=40 and (Round(sum(a.PctAllocation),0)) <75) then ''<div style=background-color:#ACB8C0;color:#fff>''+ Convert(varchar,Round(sum(a.PctAllocation),0))+''%</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=0 and (Round(sum(a.PctAllocation),0)) <40) then ''<div style=background-color:#FF5757;color:#fff>''+ Convert(varchar,Round(sum(a.PctAllocation),0))+''%</div>'' 
		else ''<div>''+Convert(varchar,Round(sum(a.PctAllocation),0))+''%</div>'' end + ''</a>''  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 		
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate
		and a.ResourceSubWorkItem != '''''
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';

	END
	ELSE IF(@type='HOURS')
	BEGIN
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  SubWorkItem asc) ItemOrder ,
		SubWorkItem as Role, 
		''<div title="Unfilled Roles">'' +
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		+ ''</div>''
		ResourceQuantity,'''' RevenueCapacity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.SubWorkItem, 

		''<a style=cursor:pointer; onclick=FindUnfilledAllocationsByRole(''''''+r.Id+'''''',''''''+convert(varchar(10),a.MonthStartDate,121)+'''''',''''''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+'''''','''''+CONVERT(varchar,@SoftAllocation)+'''''''+'');>''
		+Case when (Round(sum(a.PctAllocation),0)) >= 120 then ''<div style=background-color:#F9AA33;color:#fff>''+ Convert(varchar, Round(sum(a.AllocationHour),0))+''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=75 and (Round(sum(a.PctAllocation),0)) <120) then ''<div style=background-color:#6BA538;color:#fff>''+ Convert(varchar, Round(sum(a.AllocationHour),0))+''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=40 and (Round(sum(a.PctAllocation),0)) <75) then ''<div style=background-color:#ACB8C0;color:#fff>''+ Convert(varchar, Round(sum(a.AllocationHour),0))+''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=0 and (Round(sum(a.PctAllocation),0)) <40) then ''<div style=background-color:#FF5757;color:#fff>''+ Convert(varchar, Round(sum(a.AllocationHour),0))+''</div>'' 
		else ''<div>''+Convert(varchar, Round(sum(a.AllocationHour),0))+''</div>'' end + ''</a>''  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceUsageSummaryMonthWise a 		
		left join Roles r on r.Name = a.SubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.WorkItemID
		where a.TenantID='''+@TenantID+''' and  (a.ResourceUser = ''00000000-0000-0000-0000-000000000000'' or a.ResourceUser is null)
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate
		and a.SubWorkItem !='''''
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and WorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and WorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.SubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';

	END
	ELSE IF(@type='FTE' OR @type='ALLOCATION')
	BEGIN
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 
		''<div title="Unfilled Roles">'' +
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		+ ''</div>'' ResourceQuantity,'''' RevenueCapacity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 
		
		''<a style=cursor:pointer; onclick=FindUnfilledAllocationsByRole(''''''+r.Id+'''''',''''''+convert(varchar(10),a.MonthStartDate,121)+'''''',''''''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+'''''','''''+CONVERT(varchar,@SoftAllocation)+'''''''+'');>''
		+Case when (Round(sum(a.PctAllocation),0)) >= 120 then ''<div style=background-color:#F9AA33;color:#fff>''+ Convert(varchar,Cast(Sum(a.PctAllocation)/100 as decimal(8,2))) +''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=75 and (Round(sum(a.PctAllocation),0)) <120) then ''<div style=background-color:#6BA538;color:#fff>''+ Convert(varchar,Cast(Sum(a.PctAllocation)/100 as decimal(8,2))) +''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=40 and (Round(sum(a.PctAllocation),0)) <75) then ''<div style=background-color:#ACB8C0;color:#fff>''+ Convert(varchar,Cast(Sum(a.PctAllocation)/100 as decimal(8,2))) +''</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=0 and (Round(sum(a.PctAllocation),0)) <40) then ''<div style=background-color:#FF5757;color:#fff>''+ Convert(varchar,Cast(Sum(a.PctAllocation)/100 as decimal(8,2))) +''</div>'' 
		else ''<div>''+Convert(varchar,Cast(Sum(a.PctAllocation)/100 as decimal(8,2))) +''</div>'' end + ''</a>''  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a 
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate
		and a.ResourceSubWorkItem != '''''
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';

	END
	ELSE IF(@type='AVAILABILITY')
	BEGIN
		SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  ResourceSubWorkItem asc) ItemOrder ,
		ResourceSubWorkItem as Role, 
				''<div title="Unfilled Roles">'' +
		(SELECT  Convert(varchar,count(distinct UserId)) from Summary_ResourceProjectComplexity where TenantID = '''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId)
		+ ''</div>'' ResourceQuantity,'''' RevenueCapacity,
		(SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then sum(Count) else sum(AllCount) End from Summary_ResourceProjectComplexity where TenantID ='''+@TenantID+''' and UserId = ''00000000-0000-0000-0000-000000000000'' group by UserId) ProjectCapacity,
		 '+@cols+ 'from (
		Select a.ResourceSubWorkItem, 
		
		''<a style=cursor:pointer; onclick=FindUnfilledAllocationsByRole(''''''+r.Id+'''''',''''''+convert(varchar(10),a.MonthStartDate,121)+'''''',''''''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+'''''','''''+CONVERT(varchar,@SoftAllocation)+'''''''+'');>''
		+Case when (Round(sum(a.PctAllocation),0)) >= 120 then ''<div style=background-color:#F9AA33;color:#fff>''+ Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round(((100 -(sum(a.PctAllocation)))),2) end)) +''%</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=75 and (Round(sum(a.PctAllocation),0)) <120) then ''<div style=background-color:#6BA538;color:#fff>''+ Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round(((100 -(sum(a.PctAllocation)))),2) end)) +''%</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=40 and (Round(sum(a.PctAllocation),0)) <75) then ''<div style=background-color:#ACB8C0;color:#fff>''+ Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round(((100 -(sum(a.PctAllocation)))),2) end)) +''%</div>'' 
		When ((Round(sum(a.PctAllocation),0)) >=0 and (Round(sum(a.PctAllocation),0)) <40) then ''<div style=background-color:#FF5757;color:#fff>''+ Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round(((100 -(sum(a.PctAllocation)))),2) end)) +''%</div>'' 
		else ''<div>''+Convert(varchar,(Case when (100-(sum(a.PctAllocation)))<0  then 0 else Round(((100 -(sum(a.PctAllocation)))),2) end)) +''%</div>'' end + ''</a>''  PctAllocation,

		Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
		from ResourceAllocationMonthly a
		left join Roles r on r.Name = a.ResourceSubWorkItem
		LEFT JOIN ResourceAllocation ra ON ra.ResourceWorkItemLookup = a.ResourceWorkItemLookup
		where a.TenantID='''+@TenantID+''' and  a.ResourceUser = ''00000000-0000-0000-0000-000000000000''
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate
		and a.ResourceSubWorkItem != '''''
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem, r.Id
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
		
	END
End



Print(@SQLStatement)
EXEC(@SQLStatement)

--  Below code for Footer
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
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate'
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
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
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate'
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
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
		and ra.AllocationStartDate = a.ActualStartDate 
		and ra.AllocationEndDate = a.ActualEndDate'
		IF(@Studio !='' OR @Sector!='')
		begin 
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ))'
		end
		else
		begin
			set @SQLStatement = @SQLStatement + ' and ResourceWorkItem in (SELECT case when ''' + CONVERT(varchar, @Closed)+ ''' = ''1'' then  TicketId else case when Closed = 0 then TicketId else '''' end end from #tmpTickets)'
		end
		set @SQLStatement = @SQLStatement + ' and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
		and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ '''and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+ '''
		AND ra.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 THEN '''+CONVERT(varchar,@SoftAllocation)+''' ELSE ra.SoftAllocation  END
		AND ra.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 THEN ra.NonChargeable ELSE '''+CONVERT(varchar,@OnlyNCO)+'''  END
		group by a.MonthStartDate,a.ResourceSubWorkItem
		)s pivot(max(PctAllocation) for MName  in ('+@cols+')) p';
	END

Print(@SQLStatement)
EXEC(@SQLStatement)

Drop table #tblJobProfile;
Drop table #tblRole;
Drop Table #tmpTickets;
END

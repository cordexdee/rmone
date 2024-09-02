
ALTER procedure [dbo].[usp_GetResourceUtilzation]
 @TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069' ,
 @Fromdate datetime='2023-01-01 00:00:00.000',
 @Todate datetime='2023-12-31 00:00:00.000', 
 @mode varchar(10) ='Monthly',
 @Closed bit= '0',
 @type varchar(15) = 'PERCENT', --FTE', PERCENT , COUNT, AVAILABILITY
 @Department varchar(2000)='', --'570',
 @IncludeAllResources bit='True',
 @IsAssignedallocation bit='False',
 @ResourceManager varchar(250)='0',
 @url varchar(500)='',
 /*https://localhost:44300/layouts/ugovernit/DelegateControl.aspx*/
 @AllocationType varchar(50)='Estimated',
 @LevelName varchar(50)='',
 --@JobProfile varchar(500)='Assistant Project Manager',
 @JobProfile varchar(500)='',
 @GlobalRoleId nvarchar(128)='',
--@UserList nvarchar(max) = 'a9a6495c-fe3d-4689-94c9-6b18442ac591',
@UserList nvarchar(max) = '',
 @IsManager  bit='False',
 --@Filter nvarchar(250) = 'Pipeline',
 --@Studio nvarchar(250) = '',
 --@Division bigint = 0,
 --@Sector nvarchar(250) = 'Corporate Interiors'
 @Filter nvarchar(250) = '',
 @Studio nvarchar(250) = '',
 @Division bigint = 0,
 @Sector nvarchar(250) = ''
As
begin
--'35525396-E5FE-4692-9239-4DF9305B915B'
--'BCD2D0C9-9947-4A0B-9FBF-73EA61035069'
DECLARE @cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = N'' ,@IsnullCols   AS NVARCHAR(MAX) ,  @UserCols   AS NVARCHAR(MAX)
SET @cols = STUFF((SELECT ',' + QUOTENAME(months) 
            FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode) 
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

SET @UserCols =  REPLACE(@UserList,  ',', ''',''')       

Declare @IncludeClosed nvarchar(10);

--print @UserCols;
--DROP TABLE IF EXISTS ##tmpClosedTickets;
IF OBJECT_ID('tempdb..##tmpClosedTickets') IS NOT NULL DROP TABLE ##tmpClosedTickets;
IF OBJECT_ID('tempdb..##tmpResourceAllocationMonthly') IS NOT NULL DROP TABLE ##tmpResourceAllocationMonthly;

Create table ##tmpClosedTickets
(
	TicketId nvarchar(30)
)

Insert into ##tmpClosedTickets (TicketId) 
select TicketId from CRMProject where TenantID = @Tenantid and Closed = 1;
Insert into ##tmpClosedTickets (TicketId) 
select TicketId from Opportunity where TenantID = @Tenantid and Closed = 1;
Insert into ##tmpClosedTickets (TicketId) 
select TicketId from CRMServices where TenantID = @Tenantid and Closed = 1;

--select * from ##tmpClosedTickets;

If(@Closed = 0)
Begin
	Set @IncludeClosed = 'False';
End
Else
Begin
	Set @IncludeClosed = 'True';
End

 If(@type='AVAILABILITY')
 begin
 SET @IsnullCols = STUFF((SELECT ',' + 'Isnull(' + QUOTENAME(months) + ',''100%'')' + QUOTENAME(months)
					FROM  dbo.GetMonthListForResourceAllocation(@Fromdate,@Todate,@Mode) 
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')
End
If(@type='FTE')
Begin
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, 
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then u.Name else 
''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png'''' onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')></div>'' End ResourceUser,
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''ProjectCapacity'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''RevenueCapacity'',
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''#LifeTime'',
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''$LifeTime'',
t.* from AspNetUsers u left join
(Select  ResourceUser ResourceUserAllocated ,' + @cols+ ' from (
Select a.ResourceUser,
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then '''' else 
''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirectFromCardView=true&startDate=''+ convert(varchar(10),a.MonthStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+''&ID=''+a.ResourceUser+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>'' End
+Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#E62F2F;color:#fff;padding:7px>''+Convert(varchar,Round((sum(a.PctAllocation)/100),2))+''</div>'' 
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:#D9D9D9;padding:7px>''+ Convert(varchar,Round((sum(a.PctAllocation)/100),2))+''</div>'' 
else ''<div style=background-color:#6BA538;color:white;padding:7px>''+Convert(varchar,Round((sum(a.PctAllocation)/100),2))+''</div>'' end + ''</a>''   PctAllocation,

Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
from ResourceAllocationMonthly a where a.TenantID='''+@tenantid+''' 
and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''
and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''') ) 
and ResourceWorkItem not in (SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then  TicketId else '''' end from ##tmpClosedTickets )
and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
group by a.MonthStartDate, a.ResourceUser
)s pivot(max(PctAllocation) for MName  in (' + @cols + ' )) p) t on u.id=t.ResourceUserAllocated 
where u.TenantID='''+@tenantid+''' and u.isRole=0 
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
and ISNULL(u.ManagerIDUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(u.ManagerIDUser,'''') else '''+@ResourceManager +''' end)
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END
'
End
Else If(@type='COUNT')
Begin
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, 
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then u.Name else 
 ''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png'''' onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')></div>'' End ResourceUser,
(Select  Sum(count) from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''ProjectCapacity'',
 (Select ROUND(SUM(HighProjectCapacity) / 1000000.0, 2)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''RevenueCapacity'',
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''#LifeTime'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))    from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''$LifeTime'',
t.* from AspNetUsers u left join
(Select  ResourceUser ResourceUserAllocated ,' + @cols+ ' from (
Select a.ResourceUser,
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then '''' else 
''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirectFromCardView=true&startDate=''+ convert(varchar(10),a.MonthStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+''&ID=''+a.ResourceUser+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>'' End
+Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#E62F2F;color:#fff;padding:7px>''+Convert(varchar,Count(ResourceUser))+''</div>'' 
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:#D9D9D9;padding:7px>''+ Convert(varchar,Count(ResourceUser))+''</div>'' 
else ''<div style=background-color:#6BA538;color:white;padding:7px>''+Convert(varchar,Count(ResourceUser))+''</div>'' end + ''</a>''  PctAllocation,

Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
from ResourceAllocationMonthly a where a.TenantID='''+@tenantid+''' 
and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''
--and a.ResourceUser=''5344cb75-a739-44b7-8bd6-cb51665d97d9'' 
and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ) )
and ResourceWorkItem not in (SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then  TicketId else '''' end from ##tmpClosedTickets )
and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))
group by a.MonthStartDate, a.ResourceUser
)s pivot(max(PctAllocation) for MName  in (' + @cols + ' )) p) t on u.id=t.ResourceUserAllocated 
where u.TenantID='''+@tenantid+''' and u.isRole=0
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
and ISNULL(u.ManagerIDUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(u.ManagerIDUser,'''') else '''+@ResourceManager +''' end)
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END
'
end
Else IF(@type='PERCENT')
begin
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, 
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then u.Name else 
''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png'''' onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')></div>'' End ResourceUser,
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''ProjectCapacity'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''RevenueCapacity'',
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''#LifeTime'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''$LifeTime'',
t.* from AspNetUsers u left join
(Select  ResourceUser ResourceUserAllocated ,' + @cols+ ' from (
Select a.ResourceUser, 
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then '''' else 
''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirectFromCardView=true&startDate=''+ convert(varchar(10),a.MonthStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+''&ID=''+a.ResourceUser+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>'' End
+Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#E62F2F;color:#fff;padding:7px>''+Convert(varchar,(sum(a.PctAllocation)*100/100))+''%''+''</div>'' 
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:#D9D9D9;padding:7px>''+ Convert(varchar,(sum(a.PctAllocation)*100/100))+''%''+''</div>'' else ''<div style=background-color:#6BA538;color:white;padding:7px>''+Convert(varchar,(sum(a.PctAllocation)*100/100))+''%''+''</div>'' end + ''</a>''   PctAllocation,
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
from ResourceAllocationMonthly a where a.TenantID='''+@tenantid+''' 
and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''
--and a.ResourceUser=''5344cb75-a739-44b7-8bd6-cb51665d97d9'' 
and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ) )
and ResourceWorkItem not in (SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then  TicketId else '''' end from ##tmpClosedTickets )
and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
group by a.MonthStartDate, a.ResourceUser
)s pivot(max(PctAllocation) for MName  in (' + @cols + ' )) p) t on u.id=t.ResourceUserAllocated 
where u.TenantID='''+@tenantid+''' and u.isRole=0
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
and ISNULL(u.ManagerIDUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(u.ManagerIDUser,'''') else '''+@ResourceManager +''' end)
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END
'
end
Else IF(@type='AVAILABILITY')
begin
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, 
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then u.Name else 
''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png'''' onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')></div>'' End ResourceUser,
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''ProjectCapacity'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1))))) from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''RevenueCapacity'',
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''#LifeTime'',
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''$LifeTime'',
t.* from AspNetUsers u left join
(Select  ResourceUser ResourceUserAllocated ,' + @IsnullCols+ ' from (
Select a.ResourceUser, 
--Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end))+''%'' PctAllocation,
case when '''+ CONVERT(varchar, @IsManager)+ ''' = ''0'' then '''' else 
''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirectFromCardView=true&startDate=''+ convert(varchar(10),a.MonthStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.MonthStartDate),121)+''&ID=''+a.ResourceUser+''&IncludeClosed='+@IncludeClosed+''''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>'' End
+Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#E62F2F;color:#fff;padding:7px>''+Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end))+''%''+''</div>'' 
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:#D9D9D9;padding:7px>''+ Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end))+''%''+''</div>'' else ''<div style=background-color:#6BA538;color:white;padding:7px>''+Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end))+''%''+''</div>'' end + ''</a>''  PctAllocation,

Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
from ResourceAllocationMonthly a where a.TenantID='''+@tenantid+''' 
and a.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and a.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''
--and a.ResourceUser=''5344cb75-a739-44b7-8bd6-cb51665d97d9'' 
and ResourceWorkItem in (select TicketId from [fnGetDivisonStudioTicketWise]('''+@TenantID+''','''+@Filter+''','''+@Studio+''','''+CAST(@Division as varchar(10))+''','''+@Sector+''' ) )
and ResourceWorkItem not in (SELECT case when '''+ CONVERT(varchar, @Closed)+ ''' = ''0'' then  TicketId else '''' end from ##tmpClosedTickets )
and ISNULL(a.ResourceWorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.ResourceWorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
group by a.MonthStartDate, a.ResourceUser
ALLRESOURCES

)s pivot(max(PctAllocation) for MName  in (' + @cols + ' )) p) t on u.id=t.ResourceUserAllocated 
where u.TenantID='''+@tenantid+''' and u.isRole=0
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
and ISNULL(u.ManagerIDUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(u.ManagerIDUser,'''') else '''+@ResourceManager +''' end)
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END
'


	If @IncludeAllResources = 'True'
	Begin
			select * into ##tmpResourceAllocationMonthly
			from ResourceAllocationMonthly where id = -999

			Declare @queryPart nvarchar(max);
			Declare @Resource nvarchar(500);
			Declare @num int;
			Declare @count int;
			DECLARE @CurrentDate AS DATETIME;
			DECLARE @UGITStartDate AS DATETIME;
			DECLARE @UGITEndDate AS DATETIME;
			Declare @PctAllocation as int;
			Declare @IsEnabled as bit;
			Declare @tblResourceAllocMonthly TABLE
			(
			Id int identity,
			UserId nvarchar(500),
			StartDate date,
			EndDate date,
			IsEnabled bit
			)

			set @queryPart = '';

			
			Insert into @tblResourceAllocMonthly (UserId, StartDate, EndDate, IsEnabled) 
			select Id, UGITStartDate, UGITEndDate, Enabled from AspNetUsers where TenantID = @TenantID and isRole = 0
			and id not in (
			select ResourceUser from ResourceAllocationMonthly where TenantID = @TenantID and MonthStartDate between @Fromdate and @Todate
			)
			select @count = count(*) from @tblResourceAllocMonthly;			
			set @num = 1;
				
			WHILE @num <= @count
			BEGIN
			SET @CurrentDate = @Fromdate;
			select @Resource = UserId, @UGITStartDate = StartDate,@UGITEndDate=EndDate, @IsEnabled = IsEnabled  from @tblResourceAllocMonthly where Id = @num;
				WHILE (@CurrentDate < @Todate)
					BEGIN
					IF(year(@CurrentDate) * 100 + month(@CurrentDate)   between year(@UGITStartDate) * 100 + month(@UGITStartDate) and year(@UGITEndDate) * 100 + month(@UGITEndDate))
					BEGIN
						SET @PctAllocation = 0
					END
					ELSE
					BEGIN
					if(@IsEnabled = 1)
						BEGIN
						SET @PctAllocation = 0
						END
					ELSE
						BEGIN
						SET @PctAllocation = 100
						END
					END
						INSERT INTO ##tmpResourceAllocationMonthly
							   ([MonthStartDate]
							   ,[PctAllocation]
							   ,[PctPlannedAllocation]
							   ,[ResourceUser]
							   ,[ResourceSubWorkItem]
							   ,[ResourceWorkItem]
							   ,[ResourceWorkItemLookup]
							   ,[ResourceWorkItemType]
							   ,[Title]
							   ,[TenantID]
							   ,[Created]
							   ,[Modified]
							   ,[CreatedByUser]
							   ,[ModifiedByUser]
							   ,[Deleted]
							   ,[Attachments]
          
							   ,[GlobalRoleID])
						 VALUES
							   (@CurrentDate
							   ,@PctAllocation
							   ,0
							   ,@Resource
							   ,''
							   ,''
							   ,0
							   ,''
							   ,''
							   ,@TenantID
							   ,getdate()
							   ,getdate()
							   ,'00000000-0000-0000-0000-000000000000'
							   ,'00000000-0000-0000-0000-000000000000'
							   ,0
							   ,''
          
							   ,'00000000-0000-0000-0000-000000000000')
							SET @CurrentDate = DATEADD(MONTH, 1, @CurrentDate)
						end

						SET @num = @num + 1;
						end

					--select * from ##tmpResourceAllocationMonthly;

--@SQLStatement
			Set @queryPart = 'union
						Select bk.ResourceUser, 
						Case when (sum(bk.PctAllocation)*100/100) >= 100 then ''''+Convert( varchar,( Case when Round((100-(sum(bk.PctAllocation)*100/100)),2)<=0  then '''' else ''''  end))+''''
						When ((sum(bk.PctAllocation)*100/100) >=80 and (sum(bk.PctAllocation)*100/100) <=99) then ''<div>''+ Convert( varchar,( Case when (100-(sum(bk.PctAllocation)*100/100))<0  then '''' else Round((100-(sum(bk.PctAllocation)*100/100)),2) end))+''%''+''</div>'' else ''<div>''+Convert( varchar,( Case when (100-(sum(bk.PctAllocation)*100/100))<0  then '''' else Round((100-(sum(bk.PctAllocation)*100/100)),2) end))+''%''+''</div>'' end  PctAllocation,

						Left(DATENAME(MONTH,DATEADD(MONTH, 0, bk.MonthStartDate)),3)+''-''+right(convert(date,bk.MonthStartDate,121),2)+''-'' + right(YEAR(bk.MonthStartDate),2) MName
						from ##tmpResourceAllocationMonthly bk where bk.TenantID='''+@tenantid+''' 
						and bk.monthstartdate >= '''+ CONVERT(varchar, @Fromdate,121)+ ''' and bk.monthstartdate  <='''+ CONVERT(varchar, @Todate,121)+'''
						group by bk.MonthStartDate, bk.ResourceUser';
						
			Set @SQLStatement = REPLACE(@SQLStatement, 'ALLRESOURCES', @queryPart);

	End
	Else
	Begin
		Set @SQLStatement = REPLACE(@SQLStatement, 'ALLRESOURCES', '');
	End

End

IF(@UserList != '')
Begin
	Set @SQLStatement =@SQLStatement + ' and u.Id in ('''+@UserCols+''') '
End

If(@IncludeAllResources='False')
begin
Set @SQLStatement =@SQLStatement +' and (Select Sum(count) from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+''' and userid =u.Id) is not null
and t.ResourceUserAllocated is not null and u.Enabled=1'
End
Else If(@IncludeAllResources='True')
Begin
Set @SQLStatement =@SQLStatement +' and ( (not(year(u.UGITStartDate) < year('''+CONVERT(varchar, @Fromdate,121)+''') and year(u.UGITEndDate) < year('''+CONVERT(varchar, @Fromdate,121)+''')) or (year(u.UGITStartDate) > year('''+CONVERT(varchar, @Fromdate,121)+''') and year(u.UGITEndDate) > year('''+CONVERT(varchar, @Fromdate,121)+'''))))'
End

--drop table ##tmpClosedTickets;

Set @SQLStatement= @SQLStatement+ '  order by u.Name '
Print (@SQLStatement)  
EXEC (@SQLStatement)
EXEC usp_GetResourceUtlizationFooter @TenantID, @Fromdate,@Todate ,@mode ,@Closed ,@type ,	@Department,  @IncludeAllResources ,@IsAssignedallocation, @ResourceManager,@AllocationType,@LevelName,@JobProfile,@GlobalRoleId,@UserList;
END

  
Alter procedure [dbo].[usp_GetResourceUtilzationweekly]  
 @Tenantid varchar(128)='35525396-E5FE-4692-9239-4DF9305B915B' ,  
 @Fromdate datetime='2022-01-03 00:00:00.000',  
 @Todate datetime='2022-02-06 00:00:00.000',  
 @Mode varchar(10) ='Weekly',  
 @Closed bit= 'False',  
 @type varchar(15) ='COUNT',  
 @Department varchar(2000)='',  
 @IncludeAllResources bit='False',  
 @IsAssignedallocation bit='False',  
 @ResourceManager varchar(250)='',  
 @url varchar(500)='',  
 @AllocationType varchar(50)='Estimated',  
 @LevelName varchar(50)='',  
  @JobProfile varchar(500)='',  
 @GlobalRoleId nvarchar(128)='',  
 @UserList nvarchar(max) = '',  
 @IsManager  bit='False',  
 @Filter nvarchar(250) = 'Pipeline',  
 @Studio nvarchar(250) = '',  
 @Division bigint = 0,  
 @Sector nvarchar(250) = 'Corporate Interiors'  
As  
begin  
  
DECLARE @cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = N''     
Declare @tmpTenants table  
(ID int identity,  
WeekDayName varchar(50),  
WeekStartDate DateTime,  
WeekEndDate DateTime  
)  
If(@mode='Weekly')  
begin  
;WITH CTE(dt)  
AS  
(  
 SELECT @Fromdate  
 UNION ALL  
 SELECT DATEADD(d, 1, dt) FROM CTE  
 WHERE dt < @Todate  
)insert into @tmpTenants (WeekDayName,WeekStartDate,WeekEndDate) (  
SELECT  DATENAME(dw, @Fromdate) WeekDayName, @Fromdate WeekStartDate ,Case when DATENAME(dw, @Fromdate) = 'Monday' then DATEADD(d,6, @Fromdate)  
When DATENAME(dw, @Fromdate) = 'Tuesday' then DATEADD(d,5, @Fromdate)   
When DATENAME(dw, @Fromdate) = 'Wednesday'  then DATEADD(d,4, @Fromdate)   
When DATENAME(dw, @Fromdate) = 'Thrusday'  then DATEADD(d,3, @Fromdate)  
When DATENAME(dw, @Fromdate) = 'Friday'  then DATEADD(d,2, @Fromdate)  
When DATENAME(dw, @Fromdate) = 'Saturday'  then DATEADD(d,1, @Fromdate)  
else  DATEADD(d,0, @Fromdate) end  WeekEndDate  
union   
SELECT DATENAME(dw, dt) WeekDayName, dt WeekStartDate,   
Case when DATENAME(dw, dt) = 'Monday' then DATEADD(d,6, dt)  
When DATENAME(dw, dt) = 'Tuesday' then DATEADD(d,5, dt)   
When DATENAME(dw, dt) = 'Wednesday'  then DATEADD(d,4, dt)   
When DATENAME(dw, dt) = 'Thrusday'  then DATEADD(d,3, dt)  
When DATENAME(dw, dt) = 'Friday'  then DATEADD(d,2, dt)  
When DATENAME(dw, dt) = 'Saturday'  then DATEADD(d,1, dt)  
else  DATEADD(d,0, dt)  
end WeekEndDate  FROM CTE  
WHERE DATENAME(dw, dt) In ('Monday')  
  
)  
end  
Else Begin  
;WITH CTE(dt)  
AS  
(  
 SELECT @Fromdate  
 UNION ALL  
 SELECT DATEADD(d, 1, dt) FROM CTE  
 WHERE dt < @Todate   
)  
--select * from CTE;  
insert into @tmpTenants (WeekDayName,WeekStartDate,WeekEndDate) (  
SELECT DATENAME(dw, dt),dt,dt   FROM CTE  
--WHERE DATENAME(dw, dt) In ('Monday')  
)  
End  
/*Step1- get all weekstartdate bwteen given @Fromdate and @Todate*/  
Select top 0 *  
into ##ArchiveTable  
from(  
Select a.AllocationEndDate ,  
a.AllocationStartDate ,  
a.PctAllocation ,  
a.PctPlannedAllocation ,  
a.ResourceUser ,  
a.ResourceWorkItemLookup ,  
a.Title ,  
a.PlannedStartDate ,  
a.PlannedEndDate ,  
a.PctEstimatedAllocation ,  
a.EstStartDate ,  
a.EstEndDate ,  
a.TenantID ,  
a.Created ,  
a.Modified ,  
a.CreatedByUser ,  
a.ModifiedByUser ,  
a.Deleted ,  
a.Attachments ,  
a.DataMigrationID ,  
a.ResolutionDate ,  
a.RoleId ,  
a.ProjectEstimatedAllocationId,  
a.TicketID ,CAST(NULL as DATETIME) as  WeekStartDate   
from ResourceAllocation a   
  
) t  
Declare @i as integer,@totalrecords int=0,@WeekStartDate DateTime, @WeekEndDate DateTime  
Set @i=1  
SET @cols = STUFF((SELECT ',' + QUOTENAME(Left(DATENAME(MONTH,DATEADD(MONTH, 0, WeekStartDate)),3)+'-'+right(convert(date,WeekStartDate,121),2)+'-' + right(YEAR(WeekStartDate),2))   
            FROM  @tmpTenants   
            FOR XML PATH(''), TYPE  
            ).value('.', 'NVARCHAR(MAX)')   
        ,1,1,'')  
Set @totalrecords= (Select count(1) from @tmpTenants)  
--select * from @tmpTenants
while @i <= @totalrecords  
begin  
Select @WeekStartDate=WeekStartDate, @WeekEndDate=WeekEndDate from @tmpTenants where Id = @i   
  
INSERT INTO ##ArchiveTable     
Select  a.AllocationEndDate ,  
a.AllocationStartDate ,  
a.PctAllocation ,  
a.PctPlannedAllocation ,  
a.ResourceUser ,  
a.ResourceWorkItemLookup ,  
a.Title ,  
a.PlannedStartDate ,  
a.PlannedEndDate ,  
a.PctEstimatedAllocation ,  
a.EstStartDate ,  
a.EstEndDate ,  
a.TenantID ,  
a.Created ,  
a.Modified ,  
a.CreatedByUser ,  
a.ModifiedByUser ,  
a.Deleted ,  
a.Attachments ,  
a.DataMigrationID ,  
a.ResolutionDate ,  
a.RoleId ,  
a.ProjectEstimatedAllocationId,  
a.TicketID ,(select WeekStartDate from @tmpTenants where Id = @i ) WeekStartDate  
from ResourceAllocation a   
where a.TenantID=@TenantID   
and   
(  
(a.AllocationEndDate <= @WeekStartDate and a.AllocationStartDate  >=@WeekEndDate OR  a.AllocationEndDate >=@WeekStartDate and a.AllocationStartDate  <=@WeekEndDate)  
OR   
(a.PlannedEndDate <= @WeekStartDate and a.PlannedStartDate  >=@WeekEndDate OR  a.PlannedEndDate>= @WeekStartDate and a.PlannedStartDate <=@WeekEndDate)  
)  
  and  
(  
(@WeekStartDate >= a.EstStartDate and @WeekStartDate <= a.EstEndDate and  @WeekEndDate <= a.EstEndDate)  
          --startDate >= allocatedstdt && endDate >= allocatedenddt && startDate <= allocatedenddt                         
    OR (@WeekStartDate >= a.EstStartDate and @WeekEndDate >= a.EstEndDate and @WeekStartDate <= a.EstEndDate)  
                            
    OR (@WeekStartDate <= a.EstStartDate and @WeekEndDate >= a.EstEndDate)  
                                   
    OR (@WeekStartDate <= a.EstStartDate and  @WeekEndDate <= a.EstEndDate and  @WeekEndDate <= a.EstEndDate)  
  
)  
and a.PctEstimatedAllocation>0  
Set @i = @i + 1  
END  
If(@type='COUNT')  
Begin  
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id,  
''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png''''  
onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed=False'''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2
fdelegatecontrol'''')></div>''  ResourceUser,  
  
  
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''  
and userid =u.Id)''ProjectCapacity'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''RevenueCapacity'',  
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''#LifeTime'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''$LifeTime'',  
t.* from AspNetUsers u inner join  
(Select  ResourceUser ResourceUserAllocated , ' + @cols   +' from (  
Select ResourceUser , ''<a style=cursor:pointer; onclick=openResourceAllocationDialog('''''+@url+'?control=CustomResourceAllocation&pageTitle=Resource%20Utilization&isdlg=1&isudlg=1&allocationType='+@AllocationType+'&monthlyAllocationEdit=false&isRedirect
FromCardView=true&startDate=''+ convert(varchar(10),a.WeekStartDate,121)+''&endDate=''+convert(varchar(10),EOMONTH(a.WeekStartDate),121)+''&ID=''+a.ResourceUser+''&IncludeClosed=False'''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2fdelegatecontrol'''')>''  
+Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#FF765E;color:#fff>''+Convert(varchar,Count(ResourceUser))+''</div>''   
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:Yellow>''+ Convert(varchar,Count(ResourceUser))+''</div>''   
else ''<div style=background-color:#90ee90>''+Convert(varchar,Count(ResourceUser))+''</div>'' end + ''</a>'' PctAllocation,Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.WeekStartDate)),3)+''-''+right(convert(date,a.WeekStartDate,121),2)+''-'' + right(YEAR(a.WeekStartDate),2) MName  
from ##ArchiveTable a where a.TenantID='''+@tenantid+'''   
group by a.WeekStartDate,a.ResourceUser  
)s pivot(max(PctAllocation) for MName  in (' + @cols + '  )) p) t on u.id=t.ResourceUserAllocated   
where u.TenantID='''+@tenantid+''' and u.isRole=0 and Enabled=1   
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))   and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END  and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END  '  
End  
Else If(@type='FTE')  
Begin  
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, ''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png''''  
onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed=False'''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2
fdelegatecontrol'''')></div>''  ResourceUser,  
  
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''  
and userid =u.Id)''ProjectCapacity'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''RevenueCapacity'',  
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''#LifeTime'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''$LifeTime'',  
t.* from AspNetUsers u inner join  
(Select  ResourceUser ResourceUserAllocated , ' + @cols   +' from (  
Select ResourceUser ,    
Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#FF765E;color:#fff>''+Convert(varchar,Round((sum(a.PctAllocation)/100),2))+''</div>''   
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:Yellow>''+ Convert(varchar,Round((sum(a.PctAllocation)/100),2))+''</div>''   
else ''<div style=background-color:#90ee90>''+Convert(varchar,Round((sum(a.PctAllocation)/100),2))+''</div>'' end  PctAllocation,  
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.WeekStartDate)),3)+''-''+right(convert(date,a.WeekStartDate,121),2)+''-'' + right(YEAR(a.WeekStartDate),2) MName  
from ##ArchiveTable a where a.TenantID='''+@tenantid+'''   
--and a.ResourceUser=''f78868bb-198d-4ada-ada8-46d4c91afe31''   
group by a.WeekStartDate,a.ResourceUser  
)s pivot(max(PctAllocation) for MName  in (' + @cols + '  )) p) t on u.id=t.ResourceUserAllocated   
where u.TenantID='''+@tenantid+''' and u.isRole=0 and Enabled=1   
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))   
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END  
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END  
'  
End  
Else If(@type='PERCENT')  
Begin  
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, ''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png''''  
onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed=False'''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2
fdelegatecontrol'''')></div>''  ResourceUser,  
  
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''  
and userid =u.Id)''ProjectCapacity'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''RevenueCapacity'',  
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''#LifeTime'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''$LifeTime'',  
t.* from AspNetUsers u inner join  
(Select  ResourceUser ResourceUserAllocated , ' + @cols   +' from (  
Select ResourceUser ,    
Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#FF765E;color:#fff>''+Convert(varchar,(sum(a.PctAllocation)*100/100))+''%''+''</div>''   
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:Yellow>''+ Convert(varchar,(sum(a.PctAllocation)*100/100))+''%''+''</div>''   
else ''<div style=background-color:#90ee90>''+Convert(varchar,(sum(a.PctAllocation)*100/100))+''%''+''</div>'' end  PctAllocation,  
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.WeekStartDate)),3)+''-''+right(convert(date,a.WeekStartDate,121),2)+''-'' + right(YEAR(a.WeekStartDate),2) MName  
from ##ArchiveTable a where a.TenantID='''+@tenantid+'''   
--and a.ResourceUser=''f78868bb-198d-4ada-ada8-46d4c91afe31''   
group by a.WeekStartDate,a.ResourceUser  
)s pivot(max(PctAllocation) for MName  in (' + @cols + '  )) p) t on u.id=t.ResourceUserAllocated   
where u.TenantID='''+@tenantid+''' and u.isRole=0 and Enabled=1   
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))  
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END  
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END  
'  
End  
Else If(@type='AVAILABILITY')  
Begin  
SET @SQLStatement ='Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder ,'''' FullAllocation, u.Id, ''<div onmouseover=ShowEditImage(this) onmouseout=HideEditImage(this)>''+u.Name+'' <image style=''''width:20px;padding-right:5px;visibility:hidden;'''' src=''''/content/images/plus-blue.png''''  
onclick=javascript:openResourceAllocationDialog('''''+@url+'?control=addworkitem&pageTitle=AddAllocation&isdlg=1&isudlg=1&Type=ResourceAllocation&SelectedUsersList=''+u.id+''&IncludeClosed=False'''',''''ResourceUtilization'''',''''%2flayouts%2fugovernit%2
fdelegatecontrol'''')></div>''  ResourceUser,  
  
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''  
and userid =u.Id)''ProjectCapacity'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''RevenueCapacity'',  
 (Select Sum(allcount)  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''#LifeTime'',  
 (Select ''$''+(CONVERT (VARCHAR(10), (CAST(ROUND(SUM(AllHighProjectCapacity)  / 1000000.0, 2) AS NUMERIC(6, 1)))))+''M''   from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''  
and userid =u.Id)''$LifeTime'',  
t.* from AspNetUsers u inner join  
(Select  ResourceUser ResourceUserAllocated , ' + @cols   +' from (  
Select ResourceUser ,    
Case when (sum(a.PctAllocation)*100/100) >= 100 then ''<div style=background-color:#FF765E;color:#fff>''+Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end))+''%''+''</div>'' 
  
When ((sum(a.PctAllocation)*100/100) >=80 and (sum(a.PctAllocation)*100/100) <=99) then ''<div style=background-color:Yellow>''+ Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2)
 end))+''%''+''</div>'' else ''<div style=background-color:#90ee90>''+Convert( varchar,( Case when (100-(sum(a.PctAllocation)*100/100))<0  then 0 else Round((100-(sum(a.PctAllocation)*100/100)),2) end))+''%''+''</div>'' end  PctAllocation,  
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.WeekStartDate)),3)+''-''+right(convert(date,a.WeekStartDate,121),2)+''-'' + right(YEAR(a.WeekStartDate),2) MName  
from ##ArchiveTable a where a.TenantID='''+@tenantid+'''   
--and a.ResourceUser=''f78868bb-198d-4ada-ada8-46d4c91afe31''   
group by a.WeekStartDate,a.ResourceUser  
)s pivot(max(PctAllocation) for MName  in (' + @cols + '  )) p) t on u.id=t.ResourceUserAllocated   
where u.TenantID='''+@tenantid+''' and u.isRole=0 and Enabled=1   
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))   
and isnull(u.JobProfile,'''') =CASE WHEN '''+@JobProfile +'''='''' then isnull(u.JobProfile,'''')  else '''+@JobProfile +''' END  
and ISNULL(u.GlobalRoleID,'''') =CASE WHEN '''+@GlobalRoleId +'''='''' then isnull(u.GlobalRoleID,'''')  else '''+@GlobalRoleId +''' END  
'  
End  
  
If(@IncludeAllResources='False')  
begin  
Set @SQLStatement =@SQLStatement +' and (Select Sum(count) from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+''' and userid =u.Id) is not null'  
End  
  
Print (@SQLStatement)    
EXEC (@SQLStatement)    
Drop table [##ArchiveTable]  
EXEC usp_GetResourceUtlizationFooter @TenantID, @Fromdate,@Todate ,@mode ,@Closed ,@type , @Department,  @IncludeAllResources ,@IsAssignedallocation, @ResourceManager;  
End  
  
Go
/****** Object:  StoredProcedure [dbo].[usp_GetResourceUtlizationFooter]    Script Date: 5/10/2024 5:37:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER procedure [dbo].[usp_GetResourceUtlizationFooter]
--[usp_GetResourceUtlizationFooter] '35525396-E5FE-4692-9239-4DF9305B915B',0,0,'FTE','574','2023-01-01 00:00:00.000','2023-12-31 00:00:00.000','0','','','','Monthly'
--[usp_GetResourceUtlizationFooter] '35525396-E5FE-4692-9239-4DF9305B915B',0,0,'FTE','','2023-01-01 00:00:00.000','2023-01-31 00:00:00.000','0','','','','Weekly'
	@TenantID varchar(128), 
	@IncludeAllResources bit, 
	@IncludeClosedProject bit, 
	@DisplayMode varchar(15), --'FTE', PERCENT , COUNT, AVAILABILITY
	@Department varchar(500), --'574,570',
	@StartDate datetime, 
	@EndDate datetime, 
	@ResourceManager varchar(max), -- 0 means all users
	@AllocationType varchar(50), 
	@LevelName varchar(50), 
	@GlobalRoleId nvarchar(128), -- RoleId
	@Mode varchar(10),
	@SoftAllocation bit=0,
	@OnlyNCO bit=0
 As
begin
DECLARE @cols AS NVARCHAR(MAX), @SQLStatement NVARCHAR(MAX) = N'' ,@sumCols AS NVARCHAR(MAX),@i as integer,
@totalrecords int=0,@IsNullCols NVARCHAR(MAX)=''
SELECT * INTO #tmpClosedTickets
FROM (
select TicketId, Status from CRMProject where TenantID = @TenantID and Closed = 1
Union all																				
select TicketId, Status from Opportunity where TenantID = @TenantID and Closed = 1
Union all																				
select TicketId, Status from CRMServices where TenantID = @TenantID and Closed = 1
)t;
CREATE NONCLUSTERED INDEX [IX_tmpClosedTickets_TicketId]
ON #tmpClosedTickets (TicketId);
CREATE TABLE #TempTable (
    MName NVARCHAR(15),
    Total decimal(9,2)
);
IF(@mode='Monthly')
Begin
	SET @cols = STUFF((SELECT ',' + QUOTENAME(months) 
				FROM  dbo.GetMonthListForResourceAllocation(@StartDate,@EndDate,@Mode) 
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')
	SET @IsNullCols = STUFF((SELECT ',' + 'Isnull(' + QUOTENAME(months) + ',''0'')' + QUOTENAME(months)
					FROM  dbo.GetMonthListForResourceAllocation(@StartDate,@EndDate,@Mode) 
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')
	SET @sumCols = STUFF((SELECT ',' + 'SUM(' + QUOTENAME(months) + ')' + QUOTENAME(months)
					FROM  dbo.GetMonthListForResourceAllocation(@StartDate,@EndDate,@Mode) 
				FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)') 
			,1,1,'')
	SET @SQLStatement ='SELECT '''',''FTE'',''Allocated Demand (FTE)'' ,'+@sumCols+'  FROM 
(
Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder,
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID='''+@Tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''ProjectCapacity'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))  from Summary_ResourceProjectComplexity where TenantID='''+@tenantid+'''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName +''','';#'','',''), '',''))  
)''RevenueCapacity'',
u.Id,u.Name  ResourceUser,t.ResourceUserAllocated , ' + @IsNullCols+ '  
from AspNetUsers u 
left join
(
Select ResourceUser ResourceUserAllocated , '+@cols+' from (
Select  a.ResourceUser,Round((sum(a.PctAllocation)/100),2)
PctAllocation,
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.MonthStartDate)),3)+''-''+right(convert(date,a.MonthStartDate,121),2)+''-'' + right(YEAR(a.MonthStartDate),2) MName
from ResourceUsageSummaryMonthWise a inner join ResourceAllocation res on res.ResourceWorkItemLookup = a.WorkItemID where a.TenantID='''+@TenantID+'''
and res.AllocationStartDate = a.ActualStartDate 
and res.AllocationEndDate = a.ActualEndDate
and a.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else a.SoftAllocation  END
and res.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then res.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
and a.monthstartdate >='''+ CONVERT(varchar, @StartDate,121)+''' and a.monthstartdate  <='''+ CONVERT(varchar, @EndDate,121)+'''
and WorkItem not in (SELECT case when '''+CONVERT(varchar,@IncludeClosedProject)+''' = ''0'' then  TicketId else case when Status = ''Cancelled'' or TicketId like ''OPM%'' then TicketId else '''' end end from #tmpClosedTickets)
and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName+''','';#'','',''), '',''))
group by a.MonthStartDate, a.ResourceUser
)s 
pivot(max(PctAllocation) for MName  in (' + @cols + ')) p
) t 
on u.id=t.ResourceUserAllocated 
where u.TenantID='''+@tenantid+''' and u.isRole=0   
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace('''+@Department +''','';#'','',''), '',''))
and ISNULL(u.ManagerUser,'''') in (Case when '''+@ResourceManager +''' =''0'' then Isnull(u.ManagerUser,'''') else '''+@ResourceManager +''' end)
and ISNULL(u.GlobalRoleID,'''') in (SELECT Case when len(Item)=0 then Isnull(u.GlobalRoleID,'''') else Item end  FROM  DBO.SPLITSTRING('''+@GlobalRoleId +''', '',''))  
AND (
        ('''+CONVERT(varchar,@IncludeAllResources)+''' = 0 AND ISNULL(t.ResourceUserAllocated, ''0'') != ''0'')
        OR
        ('''+CONVERT(varchar,@IncludeAllResources)+''' = 1)
    )
  AND (
        -- Apply the u.Enabled condition only when @IncludeAllResources = 1
        ('''+CONVERT(varchar,@IncludeAllResources)+''' = 0 AND u.Enabled = 1)
        OR
        -- Include all records when @IncludeAllResources = 0
        ('''+CONVERT(varchar,@IncludeAllResources)+''' = 1)
    )
and ((not(year(u.UGITStartDate) < year('''+ CONVERT(varchar, @StartDate,121)+''')  and year(u.UGITEndDate) < year('''+ CONVERT(varchar, @StartDate,121)+'''))
or (year(u.UGITStartDate) > year('''+ CONVERT(varchar, @StartDate,121)+''') and year(u.UGITEndDate) > year('''+ CONVERT(varchar, @StartDate,121)+'''))))
)tt'
 
Print (@SQLStatement)  
EXEC (@SQLStatement) 


;WITH Months AS (
    SELECT 
        CAST(DATEADD(MONTH, number, @StartDate) AS datetime) AS MonthStartDate,
        CAST(EOMONTH(DATEADD(MONTH, number, @StartDate)) AS DATETIME) AS MonthEndDate
    FROM master.dbo.spt_values
    WHERE type = 'P'
    AND DATEADD(MONTH, number, @StartDate) <= @EndDate
) 
INSERT INTO #TempTable (MName, Total)
SELECT
    Left(DATENAME(MONTH, DATEADD(MONTH, 0, MonthStartDate)), 3) + '-' + RIGHT(CONVERT(DATE, MonthStartDate, 121), 2) + '-' + RIGHT(YEAR(MonthStartDate), 2) AS MName,
	Sum(CASE 
        WHEN A.UGITStartDate >= MonthStartDate AND A.UGITStartDate <= EOMONTH(MonthStartDate) 
            THEN CAST(CAST(((DATEDIFF(DAY, UGITStartDate, EOMONTH(DATEADD(MONTH, 0, UGITStartDate)))+ 1)*100)/Day(EOMONTH(DATEADD(MONTH, 0, UGITStartDate))) as decimal(5,2))/100 as decimal(4,2)) 
        WHEN A.UGITEndDate >= MonthStartDate AND A.UGITEndDate <= EOMONTH(MonthStartDate) 
            THEN CAST(CAST((DAY(UGITEndDate)*100)/Day(EOMONTH(DATEADD(MONTH, 0, UGITEndDate))) as decimal(5,2))/100 as decimal(5,2))
        ELSE 1.00
    END) AS Total
FROM
    Months M
LEFT JOIN
    AspNetUsers A
ON
    A.TenantID = @TenantID
    AND A.isRole = 0
    AND DATEADD(month, DATEDIFF(month, 0, A.UGITStartDate), 0) < M.MonthStartDate
    AND EOMONTH(DATEADD(MONTH, 1, A.UGITEndDate)) > M.MonthEndDate
	AND ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department ,';#',','), ',')) 
	 AND ISNULL(GlobalRoleID,'') in (SELECT Case when len(Item)=0 then Isnull(GlobalRoleID,'') else Item end  FROM  DBO.SPLITSTRING(@GlobalRoleId, ','))
	AND ISNULL(ManagerUser,'') =CASE WHEN @ResourceManager ='0' then isnull(ManagerUser,'')  else @ResourceManager  END
	GROUP BY
    M.MonthStartDate
ORDER BY
    M.MonthStartDate;
--Select * from #TempTable
Set @SQLStatement= 'Select '''',''TCFTE'',''Total Capacity (FTE)'' , '+@cols+' from 
(
Select  Sum(Total)Total, MName
from  #TempTable
group by MName
)s pivot(sum(Total) for MName  in ('+@cols+')) 
p'
Print(@SQLStatement)
EXEC (@SQLStatement)  

END
Else Begin
DECLARE @tblWeeks TABLE  
(
    ID INT IDENTITY,  
    WeekDayName VARCHAR(50),  
    WeekStartDate DATETIME,  
    WeekEndDate DATETIME  
)
INSERT INTO @tblWeeks (WeekDayName, WeekStartDate, WeekEndDate)
SELECT DATENAME(dw, DateColumn) AS WeekDayName,
       CASE WHEN DATENAME(dw, DateColumn) = 'Sunday' THEN DATEADD(DAY, 1, DateColumn)
            ELSE DATEADD(DAY, -DATEPART(WEEKDAY, DateColumn) + 2, DateColumn) END AS WeekStartDate,
       CASE WHEN DATENAME(dw, DateColumn) = 'Sunday' THEN DATEADD(DAY, 7, DateColumn)
            ELSE DATEADD(DAY, 8 - DATEPART(WEEKDAY, DateColumn), DateColumn) END AS WeekEndDate
FROM (
    SELECT TOP (DATEDIFF(DAY, @StartDate, @EndDate) + 1)
           DATEADD(DAY, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1, @StartDate) AS DateColumn
    FROM master.dbo.spt_values
) AS Dates
WHERE DATENAME(dw, DateColumn) = 'Monday'
--SELECT * FROM @tblWeeks  
SET @cols = STUFF((SELECT ',' + QUOTENAME(Left(DATENAME(MONTH,DATEADD(MONTH, 0, WeekStartDate)),3)+'-'+right(convert(date,WeekStartDate,121),2)+'-' + right(YEAR(WeekStartDate),2))     
            FROM  @tblWeeks     
            FOR XML PATH(''), TYPE    
            ).value('.', 'NVARCHAR(MAX)')     
        ,1,1,'') 

SET @sumCols = STUFF((SELECT ',' + 'SUM('+ QUOTENAME(Left(DATENAME(MONTH,DATEADD(MONTH, 0, WeekStartDate)),3)+'-'+right(convert(date,WeekStartDate,121),2)+'-' + right(YEAR(WeekStartDate),2)) + ')' +
		QUOTENAME(Left(DATENAME(MONTH,DATEADD(MONTH, 0, WeekStartDate)),3)+'-'+right(convert(date,WeekStartDate,121),2)+'-' + right(YEAR(WeekStartDate),2))
            FROM  @tblWeeks 
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')
		
SET @SQLStatement='SELECT '''',''FTE'',''Allocated Demand (FTE)'' ,'+@sumCols+'  FROM 
(

Select ROW_NUMBER() OVER(order by  u.Name asc) ItemOrder,
(Select  Sum(count)  from Summary_ResourceProjectComplexity where TenantID=''' + @Tenantid + '''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace(''' + @LevelName + ''','';#'','',''), '',''))  
)''ProjectCapacity'',
 (Select (CONVERT (VARCHAR(10), (CAST(ROUND(SUM(HighProjectCapacity) / 1000000.0, 2) AS NUMERIC(6, 1)))))  from Summary_ResourceProjectComplexity where TenantID=''' + @tenantid + '''
and userid =u.Id
and ISNULL(ModuleNameLookup,'''') in (SELECT Case when len(Item)=0 then Isnull(ModuleNameLookup,'''') else Item end  FROM  DBO.SPLITSTRING(replace(''' + @LevelName + ''','';#'','',''), '',''))  
)''RevenueCapacity'',
u.Id,
u.Name  ResourceUser,
t.ResourceUserAllocated , ' + @cols + '  
from AspNetUsers u 
left join
(
Select ResourceUser ResourceUserAllocated , ' + @cols + ' from (
Select  a.ResourceUser,Round((sum(a.PctAllocation)/100),2) PctAllocation,
Left(DATENAME(MONTH,DATEADD(MONTH, 0, a.WeekStartDate)),3)+''-''+right(convert(date,a.WeekStartDate,121),2)+''-'' + right(YEAR(a.WeekStartDate),2) MName
from ResourceUsageSummaryWeekWise a inner join ResourceAllocation res on res.ResourceWorkItemLookup = a.WorkItemID where a.TenantID=''' + @TenantID + '''
and res.AllocationStartDate = a.ActualStartDate 
and res.AllocationEndDate = a.ActualEndDate
and a.SoftAllocation = CASE WHEN '''+CONVERT(varchar,@SoftAllocation)+'''= 0 then '''+CONVERT(varchar,@SoftAllocation)+''' else a.SoftAllocation  END
and res.NonChargeable = CASE WHEN '''+CONVERT(varchar,@OnlyNCO)+'''= 0 then res.NonChargeable else '''+CONVERT(varchar,@OnlyNCO)+'''  END
and a.WeekStartDate >=''' + CONVERT(varchar, @StartDate, 121)+ ''' and a.WeekStartDate  <=''' + CONVERT(varchar, @EndDate, 121)+ '''
and WorkItem not in (SELECT case when '''+CONVERT(varchar,@IncludeClosedProject)+''' = ''0'' then  TicketId else case when Status = ''Cancelled'' or TicketId like ''OPM%'' then TicketId else '''' end end from #tmpClosedTickets)
and ISNULL(a.WorkItemType,'''') in (SELECT Case when len(Item)=0 then Isnull(a.WorkItemType,'''') else Item end  FROM  DBO.SPLITSTRING(replace('''+@LevelName+''','';#'','',''), '',''))
group by a.WeekStartDate, a.ResourceUser
)s 
pivot(max(PctAllocation) for MName  in (' + @cols + ')) p
) t 
on u.id=t.ResourceUserAllocated 
where u.TenantID=''' + @tenantid + ''' and u.isRole=0   
and ISNULL(u.DepartmentLookup,''0'') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,''0'') else Item end  FROM  DBO.SPLITSTRING(replace(''' + @Department + ''','';#'','',''), '',''))
and ISNULL(u.ManagerUser,'''') in (Case when ''' + @ResourceManager + ''' =''0'' then Isnull(u.ManagerUser,'''') else ''' + @ResourceManager + ''' end)
and ISNULL(u.GlobalRoleID,'''') in (SELECT Case when len(Item)=0 then Isnull(u.GlobalRoleID,'''') else Item end  FROM  DBO.SPLITSTRING('''+@GlobalRoleId +''', '',''))  

AND (
        (''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 0 AND ISNULL(t.ResourceUserAllocated, ''0'') != ''0'')
        OR
        (''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 1)
    )
  AND (
        -- Apply the u.Enabled condition only when @IncludeAllResources = 1
        (''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 0 AND u.Enabled = 1)
        OR
        -- Include all records when @IncludeAllResources = 0
        (''' + CONVERT(varchar, @IncludeAllResources)+ ''' = 1)
    )
and ((not(year(u.UGITStartDate) < year(''' + CONVERT(varchar, @StartDate, 121)+ ''')  and year(u.UGITEndDate) < year(''' + CONVERT(varchar, @StartDate, 121)+ '''))
or (year(u.UGITStartDate) > year(''' + CONVERT(varchar, @StartDate, 121)+ ''') and year(u.UGITEndDate) > year(''' + CONVERT(varchar, @StartDate, 121)+ ''')))) 
) tt'
Print (@SQLStatement)  
EXEC (@SQLStatement)  

 
INSERT INTO #TempTable (MName, Total)
SELECT
    Left(DATENAME(MONTH, DATEADD(MONTH, 0, WeekStartDate)), 3) + '-' + RIGHT(CONVERT(DATE, WeekStartDate, 121), 2) + '-' + RIGHT(YEAR(WeekStartDate), 2) AS MName,
    COUNT(1) AS Total
FROM
    @tblWeeks M
LEFT JOIN
    AspNetUsers A
ON
    A.TenantID = @TenantID
    AND A.isRole = 0
    AND A.UGITStartDate < M.WeekStartDate
    AND A.UGITEndDate > M.WeekEndDate
	AND ISNULL(DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department ,';#',','), ',')) 
	 AND ISNULL(GlobalRoleID,'') in (SELECT Case when len(Item)=0 then Isnull(GlobalRoleID,'') else Item end  FROM  DBO.SPLITSTRING(@GlobalRoleId, ','))
	AND ISNULL(ManagerUser,'') =CASE WHEN @ResourceManager ='0' then isnull(ManagerUser,'')  else @ResourceManager  END
	--and Enabled = case when  @IncludeAllResources = 0 then 1 else Enabled end
GROUP BY
    M.WeekStartDate
ORDER BY
    M.WeekStartDate;

Set @SQLStatement= 'Select '''',''TCFTE'',''Total Capacity (FTE)'' , '+@cols+' from 
(
Select  Sum(Total)Total, MName
from  #TempTable
group by MName
)s pivot(sum(Total) for MName  in ('+@cols+')) 
p'
Print (@SQLStatement)  
EXEC (@SQLStatement)  
END
Drop table #TempTable
Drop table #tmpClosedTickets
END

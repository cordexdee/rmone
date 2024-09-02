CREATE PROCEDURE [dbo].[usp_GetResourceSummaryReportData]  
(@TenantID VARCHAR(MAX),  
@DateFrom varchar(25),  
@DateTo varchar(25),  
@WorkItemType varchar(250) = '',  
@FunctionalArea varchar(250) = '',  
@ReportType varchar(30)= '',  
@ViewType varchar(15),  
@Department varchar(250) = '',  
@ManagerID varchar(250) = '',  
@unitAllocAct VARCHAR(10))  
/*  
EXEC usp_GetResourceSummaryReportData '35525396-e5fe-4692-9239-4df9305b915b', '04/22/2024 00:00:00', '04/28/2024 00:00:00','CPR,Time Off','','allocation', 'Weekly','','','hours'  
EXEC usp_GetResourceSummaryReportData '35525396-e5fe-4692-9239-4df9305b915b', '01/01/2022 00:00:00', '01/31/2022 00:00:00', 'CPR', '', 'allocation', 'Weekly', '570,571',null,''  
EXEC usp_GetResourceSummaryReportData '35525396-e5fe-4692-9239-4df9305b915b', '01/01/2022 00:00:00', '01/31/2022 00:00:00', 'CPR', '', 'allocation', 'Monthly', '576,577,578,579','','ftes'  
EXEC usp_GetResourceSummaryReportData '35525396-e5fe-4692-9239-4df9305b915b', '01/01/2023 00:00:00', '02/23/2023 00:00:00', 'CNS,CPR,OPM', NULL, 'Allocation', 'Monthly', '576,577,578,579','','ftes'  
*/  
AS  
BEGIN  
DECLARE @SqlQuery varchar(max)  
DECLARE @DateColName varchar(50)  
DECLARE @TableName varchar(100)  
DECLARE @EnableDivision bit  
  
SELECT * INTO #tmpTickets
FROM 
	(
	select TicketId from CRMProject  where   TenantID = @TenantID and Status != 'Cancelled'
	Union all 
	select TicketId from  Opportunity where  TenantID = @TenantID and Closed = 0 and Status != 'Cancelled'
	Union all 
	select TicketId from  CRMServices  where  TenantID = @TenantID and Status != 'Cancelled'
	) t1;

CREATE NONCLUSTERED INDEX [IX_tmpTickets_TicketId]
ON #tmpTickets(TicketId);


IF @ViewType = 'Weekly'  
BEGIN  
 SET @DateColName= 'WeekStartDate'  
 SET @TableName = 'ResourceUsageSummaryWeekWise'  
END  
ELSE IF @ViewType = 'Monthly'  
BEGIN  
 SET @DateColName= 'MonthStartDate'  
 SET @TableName = 'ResourceUsageSummaryMonthWise'  
END  
  
select @EnableDivision = KeyValue from Config_ConfigurationVariable where TenantID = @TenantID and KeyName = 'EnableDivision';  
  
SET @SqlQuery = 'select R.ID, R.ActualHour, R.AllocationHour, R.FunctionalArea [FunctionalAreaTitleLookup], R.FunctionalAreaTitleLookup as FunctionalArea , R.ManagerName [ManagerLookup], '  
SET @SqlQuery = @SqlQuery + ' R.PctActual, R.PctAllocation, R.PctPlannedAllocation, R.PlannedAllocationHour, R.[ResourceNameUser],r.ResourceUser, '  
  
If (@EnableDivision = 1)  
 SET @SqlQuery = @SqlQuery + ' R.SubWorkItem[SubWorkItem], R.WorkItem, R.WorkItemID, R.WorkItemType, R.Title, R.ERPJobID, u.DepartmentLookup [DepartmentID], case when ' + Cast(@EnableDivision as char(1))  + ' = 1 then cd.Title + '' > '' +  d.Title else d.
Title end [DepartmentNameLookup], R.' + @DateColName  
Else  
 SET @SqlQuery = @SqlQuery + ' R.SubWorkItem[SubWorkItem], R.WorkItem, R.WorkItemID, R.WorkItemType, R.Title, R.ERPJobID, u.DepartmentLookup [DepartmentID], d.Title [DepartmentNameLookup], R.' + @DateColName  
  
SET @SqlQuery = @SqlQuery + ' from ' + @TableName + ' R '   
--SET @SqlQuery = @SqlQuery + ' LEFT JOIN FunctionalAreas f '  
--SET @SqlQuery = @SqlQuery + ' on f.ID= ISNULL(r.FunctionalAreaTitleLookup,0) and f.TenantID= ''' + @TenantID + ''''  
SET @SqlQuery = @SqlQuery + ' INNER JOIN AspNetUsers u '  
 SET @SqlQuery = @SqlQuery + ' ON r.ResourceUser = u.id '  
--SET @SqlQuery = @SqlQuery + ' INNER JOIN #tmpTickets t '  
-- SET @SqlQuery = @SqlQuery + ' ON r.WorkItem = t.TicketId '  
SET @SqlQuery = @SqlQuery + ' LEFT JOIN ResourceAllocation ra '  
 SET @SqlQuery = @SqlQuery + ' ON r.WorkItemID = ra.ResourceWorkItemLookup '  
SET @SqlQuery = @SqlQuery + ' LEFT JOIN Department d '  
 SET @SqlQuery = @SqlQuery + 'ON u.DepartmentLookup = d.ID '  
  
If (@EnableDivision = 1)  
 SET @SqlQuery = @SqlQuery + ' LEFT JOIN CompanyDivisions cd ON cd.ID = d.DivisionIdLookup '  
  
SET @SqlQuery = @SqlQuery + ' where r.TenantID = ''' + @TenantID + ''''  
--15Feb2023: As discussed with Prasad, Fetch only Hard Allocations   
--SET @SqlQuery = @SqlQuery + ' AND ra.SoftAllocation = 0 '  
SET @SqlQuery = @SqlQuery + ' AND r.SoftAllocation = 0 and (R.WorkItem = ''PTO HR'' OR R.WorkItem in (select TicketId from #tmpTickets))' --AND t.Status != ''Cancelled''   
SET @SqlQuery = @SqlQuery + ' AND r.' + @DateColName + ' >= ''' + @DateFrom + ''''  
SET @SqlQuery = @SqlQuery + ' AND r.' + @DateColName + ' <= ''' + @DateTo + ''''  
if (ISNULL(@ManagerID,'') <> '')  
 SET @SqlQuery = @SqlQuery + ' And (R.ManagerLookup =''' + @ManagerID + '''  OR r.ResourceUser = ''' + @ManagerID + ''')'    
-- SET @SqlQuery = @SqlQuery + ' And R.ManagerLookup =''' + @ManagerID + ''''    
if (ISNULL(@WorkItemType,'') <> '')  
 SET @SqlQuery = @SqlQuery + ' And R.WorkItemType IN (SELECT value FROM string_split(''' + @WorkItemType + ''', '',''))'  
if (ISNULL(@Department,'') <> '')   
 SET @SqlQuery = @SqlQuery + ' AND u.DepartmentLookup IN (SELECT value FROM string_split(''' + @Department + ''', '','')) '  
if (ISNULL(@FunctionalArea,'') <> '' )  
 --SET @SqlQuery = @SqlQuery + ' AND f.Title IN (SELECT value FROM string_split(''' + @FunctionalArea + ''', '',''))'  
 SET @SqlQuery = @SqlQuery + ' AND R.FunctionalArea IN (SELECT value FROM string_split(''' + @FunctionalArea + ''', '',''))'  
SET @SqlQuery = @SqlQuery + ' AND R.TenantID = ''' + @TenantID  + ''''  
  
PRINT @SqlQuery  
EXEC (@SqlQuery)  
   
END  

Go
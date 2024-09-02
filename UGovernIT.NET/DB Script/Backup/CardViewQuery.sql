Create Procedure usp_GetCardKpis
 @TenantId varchar(123)= '35525396-E5FE-4692-9239-4DF9305B915B',--'BCD2D0C9-9947-4A0B-9FBF-73EA61035069'
 @Startdate datetime ='2022-01-01 00:00:00.000',
 @Endate datetime ='2022-07-31 00:00:00.000',
 @filter nvarchar(250) = 'Pipeline',
 @division int = 0,
 @studio nvarchar(250) = '',
 @sector nvarchar(250) = '',
 @base nvarchar(250) = 'Sector'
as
Begin
	Declare @tmptable table
	(ID int identity,
	HeadName varchar(256),
	HeadCount bigint
	)

	Declare @OverheadResourceCount int=0,@BillableResourceCount int =0;
	Set @BillableResourceCount=(Select Count(Id) from dbo.[fnGetBillableResources](@TenantId)
	Where JobType='Overhead'  and GlobalRoleID is not null and JobProfile is not null)

	Select  Ceiling((Sum(Ra.PctAllocation)/100) - (100 - Sum(Ra.PctAllocation)/100)) as HeadCount, 'UtilizationBillable' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' )
	union all

	Select  Ceiling((Sum(Ra.PctAllocation)/100) - (100 - Sum(Ra.PctAllocation)/100)) as HeadCount, 'UtilizationOverhead' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Overhead' )
	union all

	Select  Ceiling((Sum(Ra.PctAllocation)/100) - (100 - Sum(Ra.PctAllocation)/100)) as HeadCount, 'Top 5 Billable' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select top 5 Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' )
	union all
	Select  round((Sum(RA.PctAllocation)/100),2) as HeadCount, 'Billed Work Month' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and month(RA.MonthStartDate) = month(GETDATE()) and Year(RA.MonthStartDate) = YEAR(getdate()) 
			--and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' )
	union all
	Select  Abs(round((Sum(RA.PctAllocation)/100),2)) as HeadCount, 'UnBilled Work Month' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and month(RA.MonthStartDate) = month(GETDATE()) and Year(RA.MonthStartDate) = YEAR(getdate()) 
			--and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Overhead' )
	union all
	Select  Abs(round((Sum(RA.PctAllocation)/100),2)) as HeadCount, 'Billed Work Month YTD' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' )
	union all
	Select  Abs(round((Sum(RA.PctAllocation)/100),2)) as HeadCount, 'UnBilled Work Month YTD' as HeadName,
			 'FinancialView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('CPR,OPM,CNS', ','))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Overhead' )
	union all

	Select  Sum(mk.TotalBillingLaborRate) as HeadCount, 'Total Billing Labor Rate' as HeadName, 'FinancialView' as HeadType
			from (
			Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
			((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate,
			((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate) as TotalEmployeeCostRate,
			(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)) as GrossMargin
			from ResourceUsageSummaryMonthWise RA
			left join AspNetUsers NU on RA.ResourceUser = NU.Id
			join JobTitle JT on JT.ID = NU.JobTitleLookup
			where RA.TenantID=@tenantID
			and NU.Enabled = 1
			and (JT.JobType = case when '1'='1' then 'Billable' else '' end
			or JT.JobType = case when '0'='1' then 'Overhead' else '' end)
			and JT.Deleted = 0
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			) mk join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on mk.WorkItem = b.TicketId
					where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			

	union all

	Select  Sum(mk.TotalEmployeeCostRate) as HeadCount, 'Total Employee Cost Rate' as HeadName, 'FinancialView' as HeadType
			from (
			Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
			((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate,
			((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate) as TotalEmployeeCostRate,
			(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)) as GrossMargin
			from ResourceUsageSummaryMonthWise RA
			left join AspNetUsers NU on RA.ResourceUser = NU.Id
			join JobTitle JT on JT.ID = NU.JobTitleLookup
			where RA.TenantID=@tenantID
			and NU.Enabled = 1
			and (JT.JobType = case when '1'='1' then 'Billable' else '' end
			or JT.JobType = case when '0'='1' then 'Overhead' else '' end)
			and JT.Deleted = 0
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			) mk join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on mk.WorkItem = b.TicketId
					where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
				

	union all

	Select  Sum(GrossMargin) as HeadCount, 'Gross Margin' as HeadName, 'FinancialView' as HeadType
			from (
			Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
			((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate,
			((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate) as TotalEmployeeCostRate,
			(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)) as GrossMargin
			from ResourceUsageSummaryMonthWise RA
			left join AspNetUsers NU on RA.ResourceUser = NU.Id
			join JobTitle JT on JT.ID = NU.JobTitleLookup
			where RA.TenantID=@tenantID
			and NU.Enabled = 1
			and (JT.JobType = case when '1'='1' then 'Billable' else '' end
			or JT.JobType = case when '0'='1' then 'Overhead' else '' end)
			and JT.Deleted = 0
			and RA.MonthStartDate >= DATEFROMPARTS(YEAR(GETDATE()), 1, 1) and RA.MonthStartDate <= DATEFROMPARTS(YEAR(GETDATE()), 12, 31)
			) mk join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on mk.WorkItem = b.TicketId
					where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
				
		
End;


--(RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate
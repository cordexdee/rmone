
CREATE Procedure [dbo].[usp_GetTotalMissedRevenue]
-- [usp_GetTotalMissedRevenue] '35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31'
@TenantID varchar(250),
@StartDate datetime,
@EndDate datetime
as
Begin
    Create Table #ResultOfNonAllocResource(
	 MonthName nvarchar(20),
	 id nvarchar(250),
	 Name nvarchar(250),
	 JobProfile nvarchar(250),
	 Allocation float,
	 EmpLaborRate float,
	 EmpCostRate float,
	 TotalEmpCost float,
	 TotalEmpBillingCost float,
	 ResourceAvailability float
	)

	Insert into #ResultOfNonAllocResource
	exec usp_GetMissedRevenue @TenantID


	select MonthName, Count(*) as ResourceNotBilled, Format(Sum(TotalEmpBillingCost),'C0') as TotalMissedBilling, 
	Format(Sum(TotalEmpCost),'C0')  as TotalMissedCost,
	Format((Sum(TotalEmpBillingCost) - Sum(TotalEmpCost)),'C0') as GrossMargin
	from #ResultOfNonAllocResource Group by MonthName;
	drop table #ResultOfNonAllocResource;

End;



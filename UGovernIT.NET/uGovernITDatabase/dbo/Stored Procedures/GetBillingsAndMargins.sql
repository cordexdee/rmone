-- [GetBillingsAndMargins] '35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','CPR,OPM,CNS','PC'
CREATE PROCEDURE [dbo].[GetBillingsAndMargins]
@TenantID varchar(250),
@StartDate nvarchar(250),
@EndDate nvarchar(250),
@modulenames varchar(max)='CPR,CNS,OPM',
@Mode varchar(max)='C',
@Billable bit = 'True',
@Overhead bit = 'False'
AS
BEGIN
	Declare @year varchar(5);
	Set @year = YEAR(@StartDate);
	
	create table #Months(
		Number int,
		Name varchar(10)
	)

	Insert into #Months values
	(1, 'Jan'),
	(2, 'Feb'),
	(3, 'Mar'),
	(4, 'Apr'),
	(5, 'May'),
	(6, 'Jun'),
	(7, 'Jul'),
	(8, 'Aug'),
	(9, 'Sep'),
	(10, 'Oct'),
	(11, 'Nov'),
	(12, 'Dec')

    Create Table #Billings(
		StartMonth nvarchar(20),
		TotalBillingLaborRate nvarchar(100),
		TotalEmployeeCostRate nvarchar(100),
		GrossMargin nvarchar(100),
		TotalProjects nvarchar(100),
		BilledResources nvarchar(100),
		BilledWorkMonth nvarchar(100),
		UnBilledWorkMonth nvarchar(100),
		Utilization nvarchar(100)
	)

	Insert into #Billings
	exec GetBillings @TenantID,@StartDate,@EndDate,@modulenames,@Mode,@Billable,@Overhead

	--select * from #Billings;

	Create Table #Margins(
		MonthName nvarchar(20),
		UnbilledResources nvarchar(100),
		TotalMissedBilling nvarchar(100)
	)

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
	exec usp_GetMissedRevenue @TenantID,@year,@Billable,@Overhead

	Insert into #Margins
	select MonthName, Count(*) as UnbilledResources, Format(Sum(TotalEmpBillingCost),'C0') as TotalMissedBilling
	from #ResultOfNonAllocResource Group by MonthName;
	
	--select * from #Margins;


	select b.*, m.UnbilledResources, m.TotalMissedBilling, Format((cast(REPLACE(REPLACE(m.TotalMissedBilling,'$',''),',','') as decimal) - cast(REPLACE(REPLACE(b.TotalBillingLaborRate,'$',''),',','') as decimal)),'C0') as MissedRevenues  from #Billings b join #Margins m
	on b.StartMonth = m.MonthName
	join #Months mt on mt.[Name] = b.StartMonth
	order by mt.Number;

	drop table #Billings;
	drop table #Margins;
	drop table #ResultOfNonAllocResource;
	drop table #Months;
END
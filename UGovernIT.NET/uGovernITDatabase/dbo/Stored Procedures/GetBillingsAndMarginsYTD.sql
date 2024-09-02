CREATE PROCEDURE [dbo].[GetBillingsAndMarginsYTD]
@TenantID varchar(250),
@StartDate nvarchar(250),
@EndDate nvarchar(250),
@modulenames varchar(max)='CPR,CNS,OPM',
@Mode varchar(max)='C'
AS
BEGIN
	Declare @year varchar(5);
	Set @year = YEAR(@StartDate);

    Create Table #Billings(
		StartMonth nvarchar(20),
		TotalBillingLaborRate nvarchar(100),
		TotalEmployeeCostRate nvarchar(100),
		GrossMargin nvarchar(100),
		TotalProjects nvarchar(100),
		BilledResources nvarchar(100)
	)

	Insert into #Billings
	exec GetBillings @TenantID,@StartDate,@EndDate,@modulenames,@Mode

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
	exec usp_GetMissedRevenue @TenantID,@year

	Insert into #Margins
	select MonthName, Count(*) as UnbilledResources, Format(Sum(TotalEmpBillingCost),'C0') as TotalMissedBilling
	from #ResultOfNonAllocResource Group by MonthName;
	
	--select * from #Margins;
	
	--select b.*, m.UnbilledResources, m.TotalMissedBilling, Format((cast(REPLACE(REPLACE(m.TotalMissedBilling,'$',''),',','') as decimal) - cast(REPLACE(REPLACE(b.TotalBillingLaborRate,'$',''),',','') as decimal)),'C0') as MissedRevenues  from #Billings b join #Margins m
	--on b.StartMonth = m.MonthName
	
	select 
	Format(sum((cast(REPLACE(REPLACE(b.TotalBillingLaborRate,'$',''),',','') as decimal))),'C0') as TotalBillingLaborRate,
	Format(sum((cast(REPLACE(REPLACE(b.TotalEmployeeCostRate,'$',''),',','') as decimal))),'C0') as TotalEmployeeCostRate,
	Format(sum((cast(REPLACE(REPLACE(b.GrossMargin,'$',''),',','') as decimal))),'C0') as GrossMargin,
	sum(cast(b.TotalProjects as int)) as TotalProjects,
	sum(cast(b.BilledResources as int)) as BilledResources,
	sum(cast(m.UnbilledResources as int)) as UnbilledResources,
	Format(sum((cast(REPLACE(REPLACE(m.TotalMissedBilling,'$',''),',','') as decimal))),'C0') as TotalMissedBilling,
	Format(Sum((cast(REPLACE(REPLACE(m.TotalMissedBilling,'$',''),',','') as decimal) - cast(REPLACE(REPLACE(b.TotalBillingLaborRate,'$',''),',','') as decimal))),'C0') as MissedRevenues
	from #Billings b join #Margins m
	on b.StartMonth = m.MonthName 
		
	drop table #Billings;
	drop table #Margins;
	drop table #ResultOfNonAllocResource;
END

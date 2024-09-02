CREATE Procedure [dbo].[usp_GetMissedRevenue]
@TenantID varchar(250),
@year varchar(5),
@Billable bit = 'True',
@Overhead bit = 'False'
as
Begin
	Select 'Jan' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='01/01/'+ @year and ra.MonthStartDate<= EOMONTH('01/01/'+ @year) --'01/31/2021'
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
			and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
				and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Feb' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='02/01/'+ @year and ra.MonthStartDate<= EOMONTH('02/01/'+ @year) --'02/28/2021'
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
	and isnull(r.Allocation,0) = 0
					and j.Deleted = 0
	and u.Enabled = 1
	Union All

	Select 'Mar' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='03/01/'+ @year and ra.MonthStartDate<= EOMONTH('03/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Apr' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='04/01/'+ @year and ra.MonthStartDate<= EOMONTH('04/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'May' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='05/01/'+ @year and ra.MonthStartDate<= EOMONTH('05/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Jun' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='06/01/'+ @year and ra.MonthStartDate<= EOMONTH('06/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Jul' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='07/01/'+ @year and ra.MonthStartDate<= EOMONTH('07/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Aug' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='08/01/'+ @year and ra.MonthStartDate<= EOMONTH('08/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Sep' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='09/01/'+ @year and ra.MonthStartDate<= EOMONTH('09/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Oct' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='10/01/'+ @year and ra.MonthStartDate<= EOMONTH('10/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Nov' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='11/01/'+ @year and ra.MonthStartDate<= EOMONTH('11/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
	Union All

	Select 'Dec' as MonthName, u. id, u.Name, u.JobProfile,isnull(r.Allocation,0)Allocation,isnull(j.BillingLaborRate,0)EmpLaborRate,
	isnull(j.EmployeeCostRate,0)EmpCostRate,
	(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
	(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost
	, (100-isnull(r.Allocation,0)) ResourceAavailablity
	from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
	from ResourceAllocationMonthly RA
	where RA.TenantID=@TenantID and ra.MonthStartDate>='12/01/'+ @year and ra.MonthStartDate<= EOMONTH('12/01/'+ @year)
	group by RA.ResourceUser
	having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
	 join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
	where u.TenantID=@TenantID
	--and j.JobType = 'Billable'
				and (j.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or j.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
						and j.Deleted = 0
	and isnull(r.Allocation,0) = 0
	and u.Enabled = 1
End;




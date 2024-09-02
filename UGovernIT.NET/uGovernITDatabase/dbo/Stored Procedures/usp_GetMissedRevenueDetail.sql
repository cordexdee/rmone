
Create Procedure [dbo].[usp_GetMissedRevenueDetail]
-- [usp_GetMissedRevenueDetail] '35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31'
@TenantID varchar(250),
@StartDate datetime,
@EndDate datetime
as
Begin

	Select u.Name, u.JobProfile, isnull(r.Allocation,0)Allocation, isnull(j.BillingLaborRate,0)EmpLaborRate,
		isnull(j.EmployeeCostRate,0)EmpCostRate,
		(((isnull(j.EmployeeCostRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpCost,
		(((isnull(j.BillingLaborRate,0)*8)*22)*(100-isnull(r.Allocation,0))/100)TotalEmpBillingCost,
		(100-isnull(r.Allocation,0)) ResourceAavailablity
		from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
		from ResourceAllocationMonthly RA
		where RA.TenantID=@TenantID 
		and ra.MonthStartDate>=@StartDate and ra.MonthStartDate<=@EndDate
		group by RA.ResourceUser
		having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
		left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
		where u.TenantID=@TenantID
		and u.Enabled = 1
		and u.isRole = 0
		and j.JobType = 'Billable'
		order by u.Name
End;



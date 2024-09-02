


ALTER Procedure [dbo].[usp_GetCardKpis]
 @TenantId varchar(123)= 'BCD2D0C9-9947-4A0B-9FBF-73EA61035069', -- '35525396-E5FE-4692-9239-4DF9305B915B',
 @Startdate datetime ='2022-08-01 00:00:00.000',
 @Endate datetime ='2022-08-31 00:00:00.000',
 @filter nvarchar(250) = 'Pipeline',
 @division int = 0,
 @studio nvarchar(250) = '',
 @sector nvarchar(250) = '',
 @base nvarchar(250) = 'Sector',
 @headtype nvarchar(100) = 'Financial'
as
Begin
	
	Declare @OverheadResourceCount int=0,@BillableResourceCount int =0;
	Set @BillableResourceCount=(Select Count(Id) from dbo.[fnGetBillableResources](@TenantId)
	Where JobType='Billable' and Enabled=1 and GlobalRoleID is not null and JobProfile is not null)
	
	Declare @ytdHours int;
	If MONTH(@Startdate)=MONTH(@Endate)
	Begin
		Set @ytdHours= 22*8;
	End
	Else 
	Begin
		Set @ytdHours =22*8*(DateDiff(Month,@Startdate,@Endate)+1);
	End
	Declare @totalWorkHours int = @BillableResourceCount * @ytdHours;


	Select * into #MonthAllocationJoinModuleTable
	from (
		select * from (
				Select JT.Name, JT.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingRate) as TotalBillingLaborRate,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.EmpCostRate) as TotalEmployeeCostRate,
				(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmpCostRate)) as GrossMargin
				from ResourceUsageSummaryMonthWise RA
				 join fnGetBillableResources(@TenantId) JT on RA.ResourceUser = JT.Id
				where RA.TenantID=@tenantID and JT.Enabled = 1
				and JT.JobType = 'Billable'	and JT.Deleted = 0
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
				) mk join [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector) b on mk.WorkItem = b.TicketId
						where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
						)jointemp

	If(@headtype='Financial')
	Begin
		Select '# Billed Work Months' as HeadName, 
		CAST( 
		Round((Sum(ra.AllocationHour))/(22*8),2)
		as nvarchar) as HeadCount, 
					'FinancialView' as HeadType
				from ResourceUsageSummaryMonthWise RA
				where RA.TenantID=@tenantID
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate 
				and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
				and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
		union all
		Select   '# Unbilled Work Months' as HeadName, 
		CAST( 
		Round((@totalWorkHours - Sum(ra.AllocationHour))/(22*8),2)
		as nvarchar) as HeadCount,
					'FinancialView' as HeadType
				from ResourceUsageSummaryMonthWise RA
				where RA.TenantID=@tenantID
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate 
				and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
				and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
		--union all
		--Select  'Old Utilization' as HeadName, CAST( round(Avg(Ra.PctAllocation),2) as nvarchar) + ' %' as HeadCount,
		--			'FinancialView' as HeadType
		--		from ResourceUsageSummaryMonthWise RA
		--		where RA.TenantID=@tenantID
		--		and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
		--		and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
		--		and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
		union all
		Select  'Utilization' as HeadName, 
		CAST( 
		ceiling(((Sum(ra.AllocationHour)/@ytdHours)/@BillableResourceCount)*100)
		as nvarchar) + ' %' as HeadCount,
					'FinancialView' as HeadType
				from ResourceUsageSummaryMonthWise RA
				where RA.TenantID=@tenantID
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
				and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
				and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
		union all
		Select  'Resource Billing' as HeadName, FORMAT(ROUND(Sum(TotalBillingLaborRate) / 1000000.0, 2), '#0.00 M$') as HeadCount, 'FinancialView' as HeadType from #MonthAllocationJoinModuleTable

		union all
		Select  'Resource Cost' as HeadName, FORMAT(ROUND(Sum(TotalEmployeeCostRate) / 1000000.0, 2), '#0.00 M$') as HeadCount, 'FinancialView' as HeadType from #MonthAllocationJoinModuleTable
		union all
		Select  'Gross Margin' as HeadName, FORMAT(ROUND(Sum(GrossMargin) / 1000000.0, 2), '#0.00 M$') as HeadCount, 'FinancialView' as HeadType from #MonthAllocationJoinModuleTable
	End
	Else If(@headtype='Resource')
	Begin
		Select  'Billable Resource' as HeadName, count(distinct RA.ResourceUser) as HeadCount, 
			 'ResourceView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
			and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
		union all
		Select  'Overhead Resource' as HeadName, count(distinct RA.ResourceUser) as HeadCount, 
			 'ResourceView' as HeadType
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
			and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Overhead' and Enabled=1)
		union all
		select * from(
		Select top 5 RA.SubWorkItem as HeadName, count(distinct RA.ResourceUser) as HeadCount, 
			 'ResourceView' as Head5Type
			from ResourceUsageSummaryMonthWise RA
			where RA.TenantID=@tenantID
			and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
			and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
			and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
			group by RA.SubWorkItem order by HeadCount desc )temp
	End
	Else If(@headtype='Recruitment')
	Begin
		select 'Billable Resource Needed' as HeadName, sum(HeadCount) as HeadCount, 'RecruitmentView' as HeadType from (
			select  temp.SubWorkItem as HeadName,  (ceiling(sum(pct)/100) - count(temp.SubWorkItem)) as HeadCount, 'RecruitmentView' as HeadType from (
					select a.SubWorkItem, a.ResourceUser, sum(a.PctAllocation)/count(distinct a.WeekStartDate) as pct, count(distinct a.WeekStartDate) as tcount from ResourceUsageSummaryWeekWise a
					where @TenantId=TenantID 
					and a.WeekStartDate > @Startdate and a.WeekStartDate <= @Endate
					and a.ResourceUser in ( Select Id from dbo.[fnGetBillableResources](@TenantId)
					Where JobType='Billable' and Enabled=1 and GlobalRoleID is not null and JobProfile is not null
					)
					and a.WorkItem in (
							Select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector)
							)
					group by a.SubWorkItem, a.ResourceUser
					)temp
					group by temp.SubWorkItem )sumofroles
		union all
		select 'Overhead Resource Needed' as HeadName, sum(HeadCount) as HeadCount, 'RecruitmentView' as HeadType from (
			select  temp.SubWorkItem as HeadName,  (ceiling(sum(pct)/100) - count(temp.SubWorkItem)) as HeadCount, 'RecruitmentView' as HeadType from (
					select a.SubWorkItem, a.ResourceUser, sum(a.PctAllocation)/count(distinct a.WeekStartDate) as pct, count(distinct a.WeekStartDate) as tcount from ResourceUsageSummaryWeekWise a
					where @TenantId=TenantID 
					and a.WeekStartDate > @Startdate and a.WeekStartDate <= @Endate
					and a.ResourceUser in ( Select Id from dbo.[fnGetBillableResources](@TenantId)
					Where JobType='Overhead' and Enabled=1 and GlobalRoleID is not null and JobProfile is not null
					)
					and a.WorkItem in (
							Select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector)
							)
					group by a.SubWorkItem, a.ResourceUser
					)temp
					group by temp.SubWorkItem )sumofroles
		union all
		select * from (
			select top 5 temp.SubWorkItem as HeadName,  (ceiling(sum(pct)/100) - count(temp.SubWorkItem)) as HeadCount, 'RecruitmentView' as HeadType from (
			select a.SubWorkItem, a.ResourceUser, sum(a.PctAllocation)/count(distinct a.WeekStartDate) as pct, count(distinct a.WeekStartDate) as tcount from ResourceUsageSummaryWeekWise a
			where @TenantId=TenantID 
			and a.WeekStartDate > @Startdate and a.WeekStartDate <= @Endate
			and a.ResourceUser in ( Select Id from dbo.[fnGetBillableResources](@TenantId)
			Where JobType='Billable' and Enabled=1 and GlobalRoleID is not null and JobProfile is not null
			)
			and a.WorkItem in (
					Select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector)
					)
			group by a.SubWorkItem, a.ResourceUser
			)temp
			group by temp.SubWorkItem order by HeadCount desc)ordering
	End
	Else If(@headtype='Project')
	Begin
		Select '# of Billable Resources' as HeadName, CAST( count(distinct RA.ResourceUser) as nvarchar) as HeadCount, 
					'ProjectView' as HeadType
				from ResourceUsageSummaryMonthWise RA
				where RA.TenantID=@tenantID
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
				and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
				and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1 )
		union all
		Select   '# of Overhead Resources' as HeadName, CAST(count(distinct RA.ResourceUser) as nvarchar) as HeadCount,
					'ProjectView' as HeadType
				from ResourceUsageSummaryMonthWise RA
				where RA.TenantID=@tenantID
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate 
				and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
				and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Overhead' and Enabled=1 )
		union all
		Select  'Utilization' as HeadName, CAST( 
		ceiling(((Sum(ra.AllocationHour)/@ytdHours)/@BillableResourceCount)*100)
		as nvarchar) + ' %' as HeadCount,
					'ProjectView' as HeadType
				from ResourceUsageSummaryMonthWise RA
				where RA.TenantID=@tenantID
				and RA.MonthStartDate >= @Startdate and RA.MonthStartDate <= @Endate
				and RA.WorkItem in (select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector))
				and RA.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId) Where JobType='Billable' and Enabled=1)
		union all
		Select  'Resource Billing' as HeadName, FORMAT(ROUND(Sum(TotalBillingLaborRate) / 1000000.0, 2), '#0.00 M$') as HeadCount, 'ProjectView' as HeadType from #MonthAllocationJoinModuleTable
		union all
		Select  'Resource Cost' as HeadName, FORMAT(ROUND(Sum(TotalEmployeeCostRate) / 1000000.0, 2), '#0.00 M$') as HeadCount, 'ProjectView' as HeadType from #MonthAllocationJoinModuleTable
		union all
		Select  'Gross Margin' as HeadName, FORMAT(ROUND(Sum(GrossMargin) / 1000000.0, 2), '#0.00 M$') as HeadCount, 'ProjectView' as HeadType
				from #MonthAllocationJoinModuleTable
	End

	Drop table #MonthAllocationJoinModuleTable
				
		
End;


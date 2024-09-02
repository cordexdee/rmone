ALTER Procedure [dbo].[usp_GetResourceRequired]
@TenantID nvarchar(max) = '',
@Startdate datetime ='',
@Endate datetime ='',
@filter nvarchar(250) = '',
@division int = 0,
@studio nvarchar(250) = '',
@sector nvarchar(250) = '',
@base nvarchar(250) = ''
as
Begin
	select temp.SubWorkItem as RoleId, sum(pct) as pct, sum(pct)/100 as PctAllocation, count(temp.SubWorkItem) as TotalCount, 
		(ceiling(sum(pct)/100) - count(temp.SubWorkItem)) as ResourceRequired from (
		select a.SubWorkItem, a.ResourceUser, sum(a.PctAllocation)/count(distinct a.WeekStartDate) as pct, count(distinct a.WeekStartDate) as tcount from ResourceUsageSummaryWeekWise a
		where @TenantId=TenantID 
		and a.WeekStartDate > @Startdate and a.WeekStartDate <= @Endate
		and a.ResourceUser in ( Select Id from dbo.[fnGetBillableResources](@TenantId)
		Where JobType='Billable'  and GlobalRoleID is not null and JobProfile is not null
		)
		and a.WorkItem in (
				Select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector)
				)
		group by a.SubWorkItem, a.ResourceUser
		)temp
		group by temp.SubWorkItem
End;
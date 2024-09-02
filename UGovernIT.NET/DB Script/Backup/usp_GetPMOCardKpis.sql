

ALTER Procedure [dbo].[usp_GetPMOCardKpis]
 @TenantId varchar(123)= 'C345E784-AA08-420F-B11F-2753BBEBFDD5', -- '35525396-E5FE-4692-9239-4DF9305B915B',
 @Startdate datetime ='2021-01-01 00:00:00.000',
 @Endate datetime ='2022-08-31 00:00:00.000',
 @filter nvarchar(250) = 'Project Requests',
 @requesttype bigint = 0,
 @priority bigint = 0,
 @projectclass bigint = 0,
 @base nvarchar(250) = 'Priority',
 @headtype nvarchar(100) = 'Project'
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


	If(@headtype='Project')
	Begin
		Select  '# of Staff' as HeadName, Cast(count(distinct a.ResourceUser) as nvarchar) as HeadCount, 'ResourceView' as HeadType
				from ResourceAllocationMonthly a join AspNetUsers users on a.ResourceUser = users.Id
			left join EmployeeTypes et on users.EmployeeType = et.Id where et.Title like 'Employee%' and
			a.ResourceWorkItem in (select Ticketid from [fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b) 
			and a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate 
		union all
		Select  '# of Consultants' as HeadName, Cast(count(distinct a.ResourceUser) as nvarchar) as HeadCount,  'ResourceView' as HeadType
			from ResourceAllocationMonthly a join AspNetUsers users on a.ResourceUser = users.Id
			left join EmployeeTypes et on users.EmployeeType = et.Id where et.Title like 'Consultant%' and
			a.ResourceWorkItem in (select Ticketid from [fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b) 
			and a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate 
		union all
		-- total cost stores sum of project budgets
		 select 'Project Budget' as HeadName, Format(sum(TotalCost),'C') as HeadCount,  'ResourceView' as HeadType from (
			Select distinct a.ResourceWorkItem, b.TotalCost as TotalCost
			from ResourceAllocationMonthly a join [fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b on a.ResourceWorkItem = b.TicketID where
			 a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate )temp
		union all
		-- Project Cost stores sum of budget actuals
		select 'Project Expenditures' as HeadName, Format(sum(ProjectCost),'C') as HeadCount,  'ResourceView' as HeadType from (
			Select distinct a.ResourceWorkItem, b.ProjectCost as ProjectCost
			from ResourceAllocationMonthly a join [fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b on a.ResourceWorkItem = b.TicketID where
			 a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate )temp
		
		union all
		Select  'Funds Available' as HeadName, Format(sum(ProjectCost),'C') as HeadCount,  'ResourceView' as HeadType from (
			Select distinct a.ResourceWorkItem, (b.TotalCost - b.ProjectCost) as ProjectCost
			from ResourceAllocationMonthly a join [fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b on a.ResourceWorkItem = b.TicketID where
			 a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate )temp
	End
	
End;


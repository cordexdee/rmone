

Alter Procedure [dbo].[usp_GetPMMChartData]
@TenantId varchar(123)= 'C345E784-AA08-420F-B11F-2753BBEBFDD5',
@Startdate datetime = '2021-01-01 00:00:00.000',
@Endate datetime = '2021-07-31 00:00:00.000',
@filter nvarchar(max) = 'Project Requests',
@requesttype bigint = 0,
@priority bigint = 0,
@projectclass bigint = 0,
@base nvarchar(250) = 'Priority'
as
Begin

	IF @base = 'Project Type'
	begin
		select count(resourceworkItem) as Projects, RequestTypeLookup as Name, rt.RequestType as ArgumentField  from(
		select distinct ResourceWorkItem, RequestTypeLookup, PriorityLookup
			from ResourceAllocationMonthly a join [dbo].[fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b on a.ResourceWorkItem = b.TicketId
					where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp join Config_Module_RequestType rt on temp.RequestTypeLookup = rt.ID where rt.TenantID=@TenantId
					group by RequestTypeLookup, rt.RequestType
	End;
	Else IF @base = 'Priority'
	begin
		select count(resourceworkItem) as Projects, PriorityLookup as Name, pt.Title as ArgumentField  from(
		select distinct ResourceWorkItem, RequestTypeLookup, PriorityLookup
		from ResourceAllocationMonthly a join [dbo].[fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority, @projectclass) b on a.ResourceWorkItem = b.TicketId
				where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp join Config_Module_Priority pt on temp.PriorityLookup = pt.ID where pt.TenantID=@TenantId
				group by PriorityLookup, pt.Title
	End;
	Else if @base = 'Project Class'
	begin
		select count(resourceworkItem) as Projects, ProjectClassLookup as Name, pc.Title as ArgumentField  from(
		select distinct ResourceWorkItem, ProjectClassLookup, PriorityLookup
			from ResourceAllocationMonthly a join [dbo].[fnGetProjectTickets](@TenantId,@filter,@requesttype,@priority,@projectclass) b on a.ResourceWorkItem = b.TicketId
					where a.TenantID=@TenantId and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate)temp join Config_ProjectClass pc on temp.ProjectClassLookup = pc.ID where pc.TenantID=@TenantId
					group by ProjectClassLookup, pc.Title
	End;

End;


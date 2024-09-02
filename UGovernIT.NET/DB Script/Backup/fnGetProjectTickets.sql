
ALTER FUNCTION [dbo].[fnGetProjectTickets]  
(	@TenantID varchar(128) ,
@filter nvarchar(max) = '',
@requestType bigint = 0,
@priority bigint = 0,
@projectclass bigint = 0
)      
RETURNS @Result TABLE
(
ActualStartDate datetime, ActualCompletionDate datetime, BusinessManagerUser nvarchar(max), DesiredCompletionDate datetime, FunctionalAreaLookup bigint, PriorityLookup bigint, ProjectCost int
,ProjectInitiativeLookup bigint, RequestTypeLookup bigint, Status nvarchar(250), TargetCompletionDate datetime, TicketId nvarchar(250), ProjectClassLookup bigint, TotalCost float, Title nvarchar(max) 
)
AS 
begin
		declare @rewardedStage int;
	set @rewardedStage = 0;
	select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

	IF @filter = 'Project Requests'
	begin
			insert into @Result select * from (
		select ActualStartDate, ActualCompletionDate, BusinessManagerUser, DesiredCompletionDate, FunctionalAreaLookup, PriorityLookup, ProjectCost,ProjectInitiativeLookup, RequestTypeLookup, Status, TargetCompletionDate, TicketId, ProjectClassLookup, TotalCost, Title
		from NPR where TenantID = @TenantID and (Closed <> 1 or Closed is null) and (
		( RequestTypeLookup = (case when @requestType > 0 then @requestType else RequestTypeLookup end)  ) or
		( PriorityLookup = (case when @priority > 0 then @priority else PriorityLookup end) ) or
		( ProjectClassLookup = (case when @projectclass > 0 then @projectclass else ProjectClassLookup end) )
		)
		)temp
	end;
	IF @filter = 'Open'
	begin
		insert into @Result select * from (
		select ActualStartDate, ActualCompletionDate, BusinessManagerUser, DesiredCompletionDate, FunctionalAreaLookup, PriorityLookup, ProjectCost, ProjectInitiativeLookup, RequestTypeLookup, Status, TargetCompletionDate, TicketId, ProjectClassLookup, TotalCost, Title 
		from PMM where TenantID = @TenantID and (Closed <> 1 or Closed is null) and (
		( RequestTypeLookup = (case when @requestType > 0 then @requestType else RequestTypeLookup end)  ) or
		( PriorityLookup = (case when @priority > 0 then @priority else PriorityLookup end) ) or
		( ProjectClassLookup = (case when @projectclass > 0 then @projectclass else ProjectClassLookup end) )
		)
		)temp
	end;
	IF @filter = 'Closed'
	begin
		insert into @Result select * from (
		select ActualStartDate, ActualCompletionDate, BusinessManagerUser, DesiredCompletionDate, FunctionalAreaLookup, PriorityLookup, ProjectCost, ProjectInitiativeLookup, RequestTypeLookup, Status, TargetCompletionDate, TicketId, ProjectClassLookup, TotalCost, Title 
		from PMM where TenantID = @TenantID and Closed = 1 and (
		( RequestTypeLookup = (case when @requestType > 0 then @requestType else RequestTypeLookup end)  ) or
		( PriorityLookup = (case when @priority > 0 then @priority else PriorityLookup end) ) or
		( ProjectClassLookup = (case when @projectclass > 0 then @projectclass else ProjectClassLookup end) )
		)
		)temp
	End;
	Return;
End;
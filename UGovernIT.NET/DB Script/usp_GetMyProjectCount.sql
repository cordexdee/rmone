CREATE PROCEDURE [dbo].[usp_GetMyProjectCount]
(
@TenantId nvarchar(128),
@UserId nvarchar(128),
@IsManager bit = 0
)
AS
BEGIN

declare @rewardedStage int;
declare @trackedWork int;
declare @ongoingWork int;
declare @Opportunities int;
set @rewardedStage = 0;
select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID
if(@IsManager = 0)
begin
	select @trackedWork = count(*) from CRMProject where TenantID = @TenantId and TicketId in (
	select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser=@UserId) and Status <> 'Closed' and Closed<>1 and StageStep < @rewardedStage
	
	select @ongoingWork = count(*) from CRMProject where TenantID = @TenantId and TicketId in (
	select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser=@UserId) and Status <> 'Closed' and Closed<>1 and StageStep >= @rewardedStage
	
	select @Opportunities = count(*) from Opportunity where TenantID = @TenantId and TicketId in(
	select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser = @UserId) and Status <> 'Closed' and Closed<>1
end
else
begin
	select @trackedWork = count(*) from CRMProject where TenantID = @TenantId and TicketId in (
	select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser in (select id from AspNetUsers where ManagerUser = @UserId)) and Status <> 'Closed' and Closed<>1 and StageStep < @rewardedStage

	select @ongoingWork = count(*) from CRMProject where TenantID = @TenantId and TicketId in (
	select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser in (select id from AspNetUsers where ManagerUser = @UserId)) and Status <> 'Closed' and Closed<>1 and StageStep >= @rewardedStage
	
	select @Opportunities = count(*) from Opportunity where TenantID = @TenantId and TicketId in(
	select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser in (select id from AspNetUsers where ManagerUser = @UserId)) and Status <> 'Closed' and Closed<>1

end

select @trackedWork TrackedWork, @ongoingWork OngoingWork, @Opportunities Opportunities
END


CREATE PROCEDURE [dbo].[usp_GetUserProjectDetails]
(
@TenantId nvarchar(128),
@UserId nvarchar(128),
@ProjectType nvarchar(128) = 'trackedwork',
@IsManager bit = 0
)
AS
BEGIN
declare @rewardedStage int;
set @rewardedStage = 0;
select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID
if(@IsManager = 0)
begin
	if @ProjectType = 'trackedwork'
	begin
		select * from CRMProject where TenantID = @TenantId and TicketId in (
		select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser=@UserId) and Status <> 'Closed' and Closed<>1 and StageStep < @rewardedStage
	end 

	if @ProjectType = 'ongoingwork'
	begin
		select * from CRMProject where TenantID = @TenantId and TicketId in (
		select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser=@UserId) and Status <> 'Closed' and Closed<>1 and StageStep >= @rewardedStage
	end

	if @ProjectType = 'opportunities'
	begin
		select * from Opportunity where TenantID = @TenantId and TicketId in(
		select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser = @UserId) and Status <> 'Closed' and Closed<>1
	end
end
else
begin
if @ProjectType = 'trackedwork'
	begin
		select * from CRMProject where TenantID = @TenantId and TicketId in (
		select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser in (select id from AspNetUsers where ManagerUser = @UserId)) and Status <> 'Closed' and Closed<>1 and StageStep < @rewardedStage
	end 

	if @ProjectType = 'ongoingwork'
	begin
		select * from CRMProject where TenantID = @TenantId and TicketId in (
		select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser in (select id from AspNetUsers where ManagerUser = @UserId)) and Status <> 'Closed' and Closed<>1 and StageStep >= @rewardedStage
	end

	if @ProjectType = 'opportunities'
	begin
		select * from Opportunity where TenantID = @TenantId and TicketId in(
		select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser in (select id from AspNetUsers where ManagerUser = @UserId)) and Status <> 'Closed' and Closed<>1
	end

end

END

-- =============================================
-- Description:exec GetMyProjectCount '2e4d74d8-b163-4de1-8154-63cc2b8b87ef','4bb3bcb1-8da5-4a97-b6c3-266785905956'
-- =============================================
CREATE PROCEDURE [dbo].[GetMyProjectCount]
(
@TenantId nvarchar(128),
@UserId nvarchar(128)
)
AS
BEGIN



select TicketId, Title from CRMProject where TenantID = @TenantId and TicketId in (
select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser=@UserId) and Status <> 'Closed' and Closed<>1
union
select TicketId, Title from CRMServices where TenantID = @TenantId and TicketId in(
select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser = @UserId) and Status <> 'Closed' and Closed<>1
union
select TicketId, Title from Opportunity where TenantID = @TenantId and TicketId in(
select distinct TicketID from ResourceAllocation ra where ra.TenantID = @TenantId and ra.ResourceUser = @UserId) and Status <> 'Closed' and Closed<>1
--union
--select TicketId, Title from ModuleUserStatistics where TenantID = @TenantId and UserName = @UserId and ModuleNameLookup in ('CPR','CNS','OPM')



END

create procedure [dbo].[usp_getProjectDetails]
@TenantId nvarchar(128)='',
@TicketId varchar(100)=''
as
begin
	select PreconStartDate, PreconEndDate, EstimatedConstructionStart, EstimatedConstructionEnd, CloseoutDate from CRMProject with (nolock) where TenantID = @TenantId and TicketId=@TicketId
end
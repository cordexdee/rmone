create proc ProjectPendingAllocation
(
@TenantId nvarchar(128)   
)
As
Begin
select * from CRMProject where TicketId not in (
select distinct  TicketId from ResourceAllocation where TicketId like 'CPR%'
)
and (Closed=0 or Closed=null) and TenantID=@TenantId
End
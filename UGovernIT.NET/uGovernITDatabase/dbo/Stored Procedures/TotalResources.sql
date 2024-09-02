create proc TotalResources 
(
@TenantId nvarchar(128)   
)
As
Begin
/*
select * from AspNetUsers where Id not in (
select distinct  ResourceUser from ResourceAllocation ra inner join CRMProject cp  on cp.TicketId=ra.TicketID 
where (Closed=0 or Closed=null) 
)
and Enabled=1 and isRole=0 and TenantID=@TenantId
*/

select count(*) from AspNetUsers where Enabled = 1 and isRole = 0 and TenantID = @TenantId;
END
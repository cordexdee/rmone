CREATE Procedure [dbo].[usp_GetBilledAndAcquisionHours]      
@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B',
@IncludeClosed bit= '0',
@IncludeOpportunity bit = '0',
@Fromdate datetime='2022-08-11 00:00:00.000',
@Todate datetime='2023-08-10 00:00:00.000' ,
@Division int = 0
AS
BEGIN
Declare @AcquisionHours float;
Declare @BilledHours float;
Declare @tblAcquisitionCost TABLE
			(
			ticketId nvarchar(500),
			UserId nvarchar(500),
			PreconStartDate date,
			PreconEndDate date,
			ConstructionStartDate date,
			ConstructionEndDate date,
			AllocationStartDate date,
			AllocationEndDate date,
			PctAllocation float,
			Closed bit
			)

insert into @tblAcquisitionCost(ticketId, UserId, PreconStartDate, PreconEndDate,ConstructionStartDate,ConstructionEndDate, AllocationStartDate, AllocationEndDate, PctAllocation,Closed)
		select 
			c.TicketId,
			p.AssignedToUser,
			(case when c.PreconStartDate is null then c.CreationDate else c.PreconStartDate end)  as PreconStartDate, 
			(case when c.PreconEndDate is null then (case when c.EstimatedConstructionStart is null then GETDATE() else c.EstimatedConstructionStart end) else c.PreconEndDate end)  as PreconEndDate, 		
			c.EstimatedConstructionStart, c.EstimatedConstructionEnd, p.AllocationStartDate, p.AllocationEndDate, p.PctAllocation, c.Closed
		from CRMProject c 
			inner join ProjectEstimatedAllocation p 
			on c.TicketId = p.TicketID
			inner join AspNetUsers a
			on a.Id = p.AssignedToUser
			inner join Department d on
			a.DepartmentLookup = d.ID
			inner join CompanyDivisions cd 
			on d.DivisionIdLookup = cd.ID
		where c.TenantID = @TenantID and ( cd.ID = (case when @division > 0 then @division else cd.ID end)  )
	union all
		select 
			c.TicketId,
			p.AssignedToUser,
			(case when c.PreconStartDate is null then c.CreationDate else c.PreconStartDate end)  as PreconStartDate, 
			(case when c.PreconEndDate is null then (case when c.EstimatedConstructionStart is null then GETDATE() else c.EstimatedConstructionStart end) else c.PreconEndDate end)  as PreconEndDate, 		
			c.EstimatedConstructionStart, c.EstimatedConstructionEnd, p.AllocationStartDate, p.AllocationEndDate, p.PctAllocation, c.Closed
		from CRMServices c 
			inner join ProjectEstimatedAllocation p 
			on c.TicketId = p.TicketID
			inner join AspNetUsers a
			on a.Id = p.AssignedToUser
			inner join Department d on
			a.DepartmentLookup = d.ID
			inner join CompanyDivisions cd 
			on d.DivisionIdLookup = cd.ID
		where c.TenantID = @TenantID and ( cd.ID = (case when @division > 0 then @division else cd.ID end)  )
		
if(@IncludeOpportunity = '1')
begin
insert into @tblAcquisitionCost(ticketId, UserId, PreconStartDate, PreconEndDate,ConstructionStartDate,ConstructionEndDate, AllocationStartDate, AllocationEndDate, PctAllocation,Closed)
	select 
		c.TicketId,
		p.AssignedToUser,
		(case when c.PreconStartDate is null then c.CreationDate else c.PreconStartDate end)  as PreconStartDate, 
		(case when c.PreconEndDate is null then (case when c.EstimatedConstructionStart is null then GETDATE() else c.EstimatedConstructionStart end) else c.PreconEndDate end)  as PreconEndDate, 		
		c.EstimatedConstructionStart, c.EstimatedConstructionEnd, p.AllocationStartDate, p.AllocationEndDate, p.PctAllocation, c.Closed
	from Opportunity c 
		inner join ProjectEstimatedAllocation p 
		on c.TicketId = p.TicketID
		inner join AspNetUsers a
			on a.Id = p.AssignedToUser
			inner join Department d on
			a.DepartmentLookup = d.ID
			inner join CompanyDivisions cd 
			on d.DivisionIdLookup = cd.ID
	where c.TenantID = @TenantID and ( cd.ID = (case when @division > 0 then @division else cd.ID end)  )
end

if(@IncludeClosed = '0')
begin
	delete from @tblAcquisitionCost where Closed = '0'
end

select 
	@AcquisionHours = sum(((case when AllocationStartDate < PreconStartDate and AllocationEndDate > PreconEndDate then DATEDIFF(day, PreconStartDate, PreconEndDate) else 
	(case when AllocationStartDate< PreconStartDate and AllocationEndDate < PreconEndDate then DATEDIFF(day, PreconStartDate, AllocationEndDate) else 
	(case when AllocationStartDate > PreconStartDate and AllocationEndDate > PreconEndDate then DATEDIFF(day, AllocationStartDate, PreconEndDate) else
	(DATEDIFF(day, AllocationStartDate, AllocationEndDate))
	end)
	end)
	end)) * PctAllocation * 8)
from @tblAcquisitionCost  where PreconEndDate >= AllocationStartDate and PreconStartDate <= AllocationEndDate and @Todate >= AllocationStartDate and @FromDate <= AllocationEndDate

select 
	@BilledHours =
	sum(((case when AllocationStartDate < ConstructionStartDate and AllocationEndDate > ConstructionEndDate then DATEDIFF(day, ConstructionStartDate, ConstructionEndDate) else 
	(case when AllocationStartDate< ConstructionStartDate and AllocationEndDate < ConstructionEndDate then DATEDIFF(day, ConstructionStartDate, AllocationEndDate) else 
	(case when AllocationStartDate > ConstructionStartDate and AllocationEndDate > ConstructionEndDate then DATEDIFF(day, AllocationStartDate, ConstructionEndDate) else
	(DATEDIFF(day, AllocationStartDate, AllocationEndDate))
	end)
	end)
	end)) * PctAllocation * 8)
from @tblAcquisitionCost   where ConstructionEndDate >= AllocationStartDate and ConstructionStartDate <= AllocationEndDate and @Todate >= AllocationStartDate and @FromDate <= AllocationEndDate

print('Billed -' +CONVERT(varchar,@BilledHours));
print( 'Acquision -' + CONVERT(varchar,@AcquisionHours));
print(@BilledHours/@AcquisionHours);
select FORMAT(@BilledHours, 'N2') as BilledHours ,FORMAT(@AcquisionHours, 'N2') as AcquisionHours, FORMAT(@BilledHours/@AcquisionHours, 'N2') as AcquisionRatio
END

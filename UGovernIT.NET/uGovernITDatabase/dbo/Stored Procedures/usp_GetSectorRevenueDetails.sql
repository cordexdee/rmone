
ALTER procedure [dbo].[usp_GetSectorRevenueDetails]
--exec usp_GetSectorRevenueDetails '35525396-E5FE-4692-9239-4DF9305B915B', 0
@tenantID nvarchar(max),
@division bigint=0
as
begin

	if len(@division) > 0
	begin
		Select * from (
			Select SectorChoice as Name ,count(distinct t.TicketId)ResourceCount,
			(
				Select count(TicketId) from CRMProject where Closed != 1 and TenantID=@tenantID and SectorChoice=t.SectorChoice and DivisionLookup=@division group by SectorChoice
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed != 1 and TenantID=@tenantID
					and SectorChoice=t.SectorChoice and DivisionLookup=@division group by SectorChoice
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  SectorChoice=t.SectorChoice and DivisionLookup=@division group by SectorChoice
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  SectorChoice=t.SectorChoice and DivisionLookup=@division group by SectorChoice
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and SectorChoice=t.SectorChoice and DivisionLookup=@division group by SectorChoice
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, SectorChoice from CRMProject where Closed != 1 and TenantID=@tenantID
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			group by t.SectorChoice
			) anu where 1=1 
	end;
	else
	begin
			Select * from (
			Select SectorChoice as Name ,count(distinct t.TicketId)ResourceCount,
			(
				Select count(TicketId) from CRMProject where Closed != 1 and TenantID=@tenantID and SectorChoice=t.SectorChoice group by SectorChoice
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed != 1 and TenantID=@tenantID
					and SectorChoice=t.SectorChoice group by SectorChoice
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  SectorChoice=t.SectorChoice group by SectorChoice
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  SectorChoice=t.SectorChoice group by SectorChoice
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and SectorChoice=t.SectorChoice group by SectorChoice
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, SectorChoice from CRMProject where Closed != 1 and TenantID=@tenantID
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			group by t.SectorChoice
			) anu where 1=1 
	end;
End;

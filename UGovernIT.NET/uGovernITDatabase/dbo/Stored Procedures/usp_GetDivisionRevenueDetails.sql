
ALTER procedure [dbo].[usp_GetDivisionRevenueDetails]
--exec usp_GetDivisionRevenueDetails '35525396-E5FE-4692-9239-4DF9305B915B'
@tenantID nvarchar(max),
@sector nvarchar(max) = ''
as
begin

	if len(@sector) > 0
	begin
		Select * from (
			Select DivisionLookup as Name ,count(distinct t.TicketId)ResourceCount,
			(
			Select count(TicketId) from CRMProject where Closed != 1 and TenantID=@tenantID and DivisionLookup=t.DivisionLookup and SectorChoice=@sector group by DivisionLookup
			) 'hotproject',

			(
			Select Sum(ApproxContractValue) ApproxContractValue  from(
			Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed != 1 and TenantID=@tenantID
			and DivisionLookup=t.DivisionLookup and SectorChoice=@sector group by DivisionLookup
			) t where 1=1) 'hotrevenue',

			(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup and SectorChoice=@sector group by DivisionLookup) 'committedprojects',
			(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup and SectorChoice=@sector group by DivisionLookup) 'committedrevenue',

			(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup and SectorChoice=@sector group by DivisionLookup) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
			Select TicketId, DivisionLookup from CRMProject where TenantID=@tenantID
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			group by t.DivisionLookup
			) anu where 1=1 and len(name) > 0
	end;
	else
	begin
		Select * from (
			Select DivisionLookup as Name ,count(distinct t.TicketId)ResourceCount,
			(
			Select count(TicketId) from CRMProject where Closed != 1 and TenantID=@tenantID and DivisionLookup=t.DivisionLookup group by DivisionLookup
			) 'hotproject',

			(
			Select Sum(ApproxContractValue) ApproxContractValue  from(
			Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed != 1 and TenantID=@tenantID
			and DivisionLookup=t.DivisionLookup group by DivisionLookup
			) t where 1=1) 'hotrevenue',

			(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup group by DivisionLookup) 'committedprojects',
			(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup group by DivisionLookup) 'committedrevenue',

			(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup group by DivisionLookup) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
			Select TicketId, DivisionLookup from CRMProject where TenantID=@tenantID
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			group by t.DivisionLookup
			) anu where 1=1 and len(name) > 0
	end;
End;

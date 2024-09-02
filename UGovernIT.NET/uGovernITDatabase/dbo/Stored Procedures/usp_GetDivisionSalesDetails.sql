
ALTER procedure [dbo].[usp_GetDivisionSalesDetails]
--exec usp_GetDivisionSalesDetails '35525396-E5FE-4692-9239-4DF9305B915B'
@tenantID nvarchar(max),
@Sector nvarchar(max) = ''
as
begin

declare @rewardedStage int;
set @rewardedStage = 0;
select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTitle = 'Contract Award' and ModuleNameLookup='CPR' and TenantID=@tenantID

if LEN(@Sector) = 0
begin
	Select * from (
	Select DivisionLookup as Name ,count(distinct t.TicketId)ResourceCount,
	(
	Select count(TicketId) from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and DivisionLookup=t.DivisionLookup group by DivisionLookup
	union all
	Select count(TicketId) from Opportunity where  TenantID=@tenantID and DivisionLookup=t.DivisionLookup group by DivisionLookup
	) 'hotproject',

	(
	Select Sum(ApproxContractValue) ApproxContractValue  from(
	Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
	and DivisionLookup=t.DivisionLookup group by DivisionLookup
	union all
	Select round(sum(isnull(ApproxContractValue,0)),2) ApproxContractValue from Opportunity where  TenantID=@tenantID
	and DivisionLookup=t.DivisionLookup group by DivisionLookup
	) t where 1=1) 'hotrevenue',

	(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup group by DivisionLookup) 'committedprojects',

	(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup group by DivisionLookup) 'committedrevenue',

	(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup group by DivisionLookup) 'pastrevenue',
	Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
	from (
	Select TicketId, DivisionLookup from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
	union all
	Select TicketId ,DivisionLookup from Opportunity where TenantID=@tenantID
	) t inner join ResourceAllocation r on r.TicketID=t.TicketId
	group by t.DivisionLookup
	) anu where 1=1 
End
Else
Begin
	Select * from (
	Select DivisionLookup as Name ,count(distinct t.TicketId)ResourceCount,
	(
	Select count(TicketId) from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup
	union all
	Select count(TicketId) from Opportunity where  TenantID=@tenantID and DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup
	) 'hotproject',

	(
	Select Sum(ApproxContractValue) ApproxContractValue  from(
	Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
	and DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup
	union all
	Select round(sum(isnull(ApproxContractValue,0)),2) ApproxContractValue from Opportunity where  TenantID=@tenantID
	and DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup
	) t where 1=1) 'hotrevenue',

	(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup) 'committedprojects',
	(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup) 'committedrevenue',

	(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup and SectorChoice = @Sector group by DivisionLookup) 'pastrevenue',
	Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
	from (
	Select TicketId, DivisionLookup from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and SectorChoice = @Sector
	union all
	Select TicketId ,DivisionLookup from Opportunity where TenantID=@tenantID and SectorChoice = @Sector
	) t inner join ResourceAllocation r on r.TicketID=t.TicketId
	group by t.DivisionLookup
	) anu where 1=1 
End;

		

End;

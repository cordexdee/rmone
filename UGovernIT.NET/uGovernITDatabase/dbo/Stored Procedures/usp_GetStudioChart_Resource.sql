

ALTER procedure [dbo].[usp_GetStudioChart_Resource]
--exec usp_GetCommonStudioChartData '35525396-E5FE-4692-9239-4DF9305B915B', 'Contracted',0
@tenantID nvarchar(max),
@filter nvarchar(max) = '',
@studio bigint
as
begin

	declare @rewardedStage int;
		set @rewardedStage = 0;
		select top 1 @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

	
	if @filter = 'Contracted'
	begin
			Select * from (
			Select t.StudioLookup as Name ,count(distinct r.ResourceUser)ResourceCount,
			(
				Select count(TicketId) from CRMProject where StageStep > @rewardedStage - 1 and  Closed != 1 and TenantID=@tenantID and StudioLookup=t.StudioLookup
				group by StudioLookup
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep > @rewardedStage - 1 and  Closed != 1 and TenantID=@tenantID
					and StudioLookup=t.StudioLookup 
					group by StudioLookup
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed != 1 and  StudioLookup=t.StudioLookup 
				group by StudioLookup
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed != 1 and  StudioLookup=t.StudioLookup 
				group by StudioLookup
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed = 1 and StudioLookup=t.StudioLookup 
				group by StudioLookup
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, StudioLookup from CRMProject where StageStep > @rewardedStage - 1 and Closed != 1 and TenantID=@tenantID
				
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.StudioLookup
			) anu where 1=1 and LEN(Name) > 0
	
	End;
	if @filter = 'Pipeline'
	begin

			Select * from (
			Select t.StudioLookup as Name ,count(distinct r.ResourceUser)ResourceCount,
			(
				Select sum(tt.cnt) from ( 
				Select count(TicketId) cnt from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and StudioLookup=t.StudioLookup
				group by StudioLookup
				union all
				Select count(TicketId) cnt from Opportunity where  TenantID=@tenantID and StudioLookup=t.StudioLookup
				group by StudioLookup) tt 
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
				Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
				and StudioLookup=t.StudioLookup group by StudioLookup
				union all
				Select round(sum(isnull(ApproxContractValue,0)),2) ApproxContractValue from Opportunity where  TenantID=@tenantID
				and StudioLookup=t.StudioLookup group by StudioLookup
			) t where 1=1) 'hotrevenue',

			(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  StudioLookup=t.StudioLookup
			group by StudioLookup) 'committedprojects',

			(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  StudioLookup=t.StudioLookup
			group by StudioLookup) 'committedrevenue',

			(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and StudioLookup=t.StudioLookup  group by StudioLookup) 'pastrevenue',

			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
			Select TicketId, StudioLookup from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID 
			union all
			Select TicketId ,StudioLookup from Opportunity where TenantID=@tenantID 
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.StudioLookup
			) anu where 1=1 
	End;
	if @filter = 'Closed'
	begin
			Select * from (
			Select t.StudioLookup as Name ,count(distinct r.ResourceUser)ResourceCount,
			(
				Select count(TicketId) from CRMProject where Closed = 1 and TenantID=@tenantID and StudioLookup=t.StudioLookup
				group by StudioLookup
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed = 1 and TenantID=@tenantID
					and StudioLookup=t.StudioLookup
					group by StudioLookup
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed = 1 and  StudioLookup=t.StudioLookup
				group by StudioLookup
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and  StudioLookup=t.StudioLookup
				group by StudioLookup
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and StudioLookup=t.StudioLookup
				group by StudioLookup
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, StudioLookup from CRMProject where Closed = 1 and TenantID=@tenantID
				and CloseDate > DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.StudioLookup
			) anu where 1=1 and LEN(Name) > 0
	End;
	
End;

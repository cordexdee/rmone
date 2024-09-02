
ALTER procedure [dbo].[usp_GetStudioChart_Resource]
--exec usp_GetCommonStudioChartData 'bcd2d0c9-9947-4a0b-9fbf-73ea61035069', 'Contracted'
@tenantID nvarchar(max),
@filter nvarchar(max) = '',
@studio nvarchar(max) = ''
as
begin

	declare @rewardedStage int;		set @rewardedStage = 0;		select top 1 @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

		if @filter = 'Contracted'	begin			Select * from (			Select StudioChoice as Name ,count(distinct r.ResourceUser)ResourceCount,			(				Select count(TicketId) from CRMProject where StageStep > @rewardedStage - 1 and  Closed != 1 and TenantID=@tenantID and StudioChoice=t.StudioChoice				group by StudioChoice			) 'hotproject',			(				Select Sum(ApproxContractValue) ApproxContractValue  from(					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep > @rewardedStage - 1 and  Closed != 1 and TenantID=@tenantID					and StudioChoice=t.StudioChoice 					group by StudioChoice			) t where 1=1) 'hotrevenue',			(				select count(1) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed != 1 and  StudioChoice=t.StudioChoice 				group by StudioChoice			) 'committedprojects',			(				select round(SUM(ApproxContractValue),2) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed != 1 and  StudioChoice=t.StudioChoice 				group by StudioChoice			) 'committedrevenue',			(				select round(sum(ApproxContractValue),2) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed = 1 and StudioChoice=t.StudioChoice 				group by StudioChoice			) 'pastrevenue',			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'			from (				Select TicketId, StudioChoice from CRMProject where StageStep > @rewardedStage - 1 and Closed != 1 and TenantID=@tenantID							) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'			group by t.StudioChoice			) anu where 1=1 and LEN(Name) > 0		End;	if @filter = 'Pipeline'	begin			Select * from (			Select StudioChoice as Name ,count(distinct r.ResourceUser)ResourceCount,			(				Select sum(tt.cnt) from ( 				Select count(TicketId) cnt from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and StudioChoice=t.StudioChoice				group by StudioChoice				union all				Select count(TicketId) cnt from Opportunity where  TenantID=@tenantID and StudioChoice=t.StudioChoice				group by StudioChoice) tt 			) 'hotproject',			(				Select Sum(ApproxContractValue) ApproxContractValue  from(				Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID				and StudioChoice=t.StudioChoice group by StudioChoice				union all				Select round(sum(isnull(ApproxContractValue,0)),2) ApproxContractValue from Opportunity where  TenantID=@tenantID				and StudioChoice=t.StudioChoice group by StudioChoice			) t where 1=1) 'hotrevenue',			(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  StudioChoice=t.StudioChoice			group by StudioChoice) 'committedprojects',			(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  StudioChoice=t.StudioChoice			group by StudioChoice) 'committedrevenue',			(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and StudioChoice=t.StudioChoice  group by StudioChoice) 'pastrevenue',			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'			from (			Select TicketId, StudioChoice from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID 			union all			Select TicketId ,StudioChoice from Opportunity where TenantID=@tenantID 			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'			group by t.StudioChoice			) anu where 1=1 	End;	if @filter = 'Closed'	begin			Select * from (			Select StudioChoice as Name ,count(distinct r.ResourceUser)ResourceCount,			(				Select count(TicketId) from CRMProject where Closed = 1 and TenantID=@tenantID and StudioChoice=t.StudioChoice				group by StudioChoice			) 'hotproject',			(				Select Sum(ApproxContractValue) ApproxContractValue  from(					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed = 1 and TenantID=@tenantID					and StudioChoice=t.StudioChoice					group by StudioChoice			) t where 1=1) 'hotrevenue',			(				select count(1) from CRMProject where TenantID=@tenantID and Closed = 1 and  StudioChoice=t.StudioChoice				group by StudioChoice			) 'committedprojects',			(				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and  StudioChoice=t.StudioChoice				group by StudioChoice			) 'committedrevenue',			(				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and StudioChoice=t.StudioChoice				group by StudioChoice			) 'pastrevenue',			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'			from (				Select TicketId, StudioChoice from CRMProject where Closed = 1 and TenantID=@tenantID				and CloseDate > DATEFROMPARTS(YEAR(GETDATE()), 1, 1)			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'			group by t.StudioChoice			) anu where 1=1 and LEN(Name) > 0	End;	
End;



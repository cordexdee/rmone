
ALTER procedure [dbo].[usp_GetCommonSectorChartData]
--exec usp_GetCommonSectorChartData '35525396-E5FE-4692-9239-4DF9305B915B', '', 'Contracted'
@tenantID nvarchar(max),
@division bigint = 0,
@filter nvarchar(max) = '',
@studio bigint = 0
as
begin
	
	declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

	if @filter = 'Contracted'
	begin
		Select * from (
			Select SectorChoice as Name , count(distinct t.TicketId)ResourceCount,
			(
				Select count(TicketId) from CRMProject where Closed != 1 and TenantID=@tenantID and SectorChoice=t.SectorChoice and StageStep > @rewardedStage - 1 and 
				( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
					and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
				group by SectorChoice
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where  Closed != 1 and TenantID=@tenantID and SectorChoice=t.SectorChoice and StageStep > @rewardedStage - 1
					and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
						and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
					group by SectorChoice
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and StageStep > @rewardedStage - 1 and  SectorChoice=t.SectorChoice 
				and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
					and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
				group by SectorChoice
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and StageStep > @rewardedStage - 1 and Closed != 1 and  SectorChoice=t.SectorChoice 
				and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
					and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
				group by SectorChoice
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and StageStep > @rewardedStage - 1 and Closed = 1 and SectorChoice=t.SectorChoice 
				and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
					and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
				group by SectorChoice
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, SectorChoice from CRMProject where Closed != 1 and TenantID=@tenantID and StageStep > @rewardedStage - 1
				and ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
					and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.SectorChoice
			) anu  where 1=1 and LEN(Name) > 0 order by anu.Name
	End;
	if @filter = 'Pipeline'
	begin
		
		Select * from (
			Select SectorChoice as Name ,count(distinct t.TicketId)ResourceCount,
			(
				Select sum(tt.cnt) from ( 
				Select count(TicketId) cnt from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and SectorChoice=t.SectorChoice and
				( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  )
				and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
				union all
				Select count(TicketId) cnt from Opportunity where  TenantID=@tenantID and SectorChoice=t.SectorChoice  
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup) tt 
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and SectorChoice=t.SectorChoice 
					and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
					group by SectorChoice
					union all
					Select round(sum(isnull(ApproxContractValue,0)),2) ApproxContractValue from Opportunity where  TenantID=@tenantID
					and SectorChoice=t.SectorChoice and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
					group by SectorChoice
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  SectorChoice=t.SectorChoice 
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  SectorChoice=t.SectorChoice
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and SectorChoice=t.SectorChoice
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, SectorChoice from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				union all
				Select TicketId ,SectorChoice from Opportunity where TenantID=@tenantID
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.SectorChoice
			) anu where 1=1 and LEN(Name) > 0 order by anu.Name
	End;
	if @filter = 'Closed'
	begin
		Select * from (
			Select SectorChoice as Name ,count(distinct t.TicketId)ResourceCount,
			(
				Select count(TicketId) from CRMProject where Closed = 1 and TenantID=@tenantID and SectorChoice=t.SectorChoice and
				( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  )
				and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed = 1 and TenantID=@tenantID
					and SectorChoice=t.SectorChoice 
					and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
					group by SectorChoice
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed = 1 and  SectorChoice=t.SectorChoice 
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and  SectorChoice=t.SectorChoice 
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  )  and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and SectorChoice=t.SectorChoice 
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by SectorChoice
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, SectorChoice from CRMProject where Closed = 1 and TenantID=@tenantID
				and ( (@division > 0  AND DivisionLookup=@division )  OR (@division = 0  AND DivisionLookup is not null)  ) and ( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  ) and
				--(CloseDate > GETDATE() or CloseDate is null)
			 CloseDate > DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.SectorChoice
			) anu where 1=1 and LEN(Name) > 0 order by anu.Name
	End;
End;



ALTER procedure [dbo].[usp_GetDivisionChart_Resource]
--exec usp_GetCommonDivisionChartData '35525396-E5FE-4692-9239-4DF9305B915B', '', 0, 'Pipeline'
@tenantID nvarchar(max),
@sector nvarchar(max) = '',
@studio bigint = 0,
@filter nvarchar(max) = ''
as
begin

	declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

	
	if @filter = 'Contracted'
	begin
			Select * from (
			Select DivisionLookup as Name ,count(distinct r.ResourceUser)ResourceCount,
			(
				Select count(TicketId) from CRMProject where StageStep > @rewardedStage - 1 and  Closed != 1 and TenantID=@tenantID and DivisionLookup=t.DivisionLookup and
				( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep > @rewardedStage - 1 and  Closed != 1 and TenantID=@tenantID and DivisionLookup=t.DivisionLookup and 
					( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
					( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
					group by DivisionLookup
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup 
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  )  and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where StageStep > @rewardedStage - 1 and  TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, DivisionLookup from CRMProject where StageStep > @rewardedStage - 1 and Closed != 1 and TenantID=@tenantID
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.DivisionLookup
			) anu where 1=1 and LEN(Name) > 0
	
	End;
	if @filter = 'Pipeline'
	begin

			Select * from (
			Select DivisionLookup as Name ,count(distinct r.ResourceUser)ResourceCount,
			(
				Select sum(tt.cnt) from ( 
				Select count(TicketId) cnt from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID and DivisionLookup=t.DivisionLookup and 
				( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
				union all
				Select count(TicketId) cnt from Opportunity where  TenantID=@tenantID and DivisionLookup=t.DivisionLookup 
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup) tt 
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
				Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
				and DivisionLookup=t.DivisionLookup 
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
				union all
				Select round(sum(isnull(ApproxContractValue,0)),2) ApproxContractValue from Opportunity where  TenantID=@tenantID
				and DivisionLookup=t.DivisionLookup 
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) t where 1=1) 'hotrevenue',

			(select count(1) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup 
			and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and
			( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			group by DivisionLookup) 'committedprojects',

			(select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and  DivisionLookup=t.DivisionLookup 
			and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
			( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			group by DivisionLookup) 'committedrevenue',

			(select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup 
			and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and
			( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			group by DivisionLookup) 'pastrevenue',

			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
			Select TicketId, DivisionLookup from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID 
			and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
			( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			union all
			Select TicketId ,DivisionLookup from Opportunity where TenantID=@tenantID 
			and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
			( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.DivisionLookup
			) anu where 1=1 and LEN(Name) > 0
	End;
	if @filter = 'Closed'
	begin
			Select * from (
			Select DivisionLookup as Name ,count(distinct r.ResourceUser)ResourceCount,
			(
				Select count(TicketId) from CRMProject where Closed = 1 and TenantID=@tenantID and DivisionLookup=t.DivisionLookup and
				( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'hotproject',

			(
				Select Sum(ApproxContractValue) ApproxContractValue  from(
					Select round(ISNULL(sum(ApproxContractValue),0),2) ApproxContractValue from CRMProject where Closed = 1 and TenantID=@tenantID
					and DivisionLookup=t.DivisionLookup
					and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
					( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
					group by DivisionLookup
			) t where 1=1) 'hotrevenue',

			(
				select count(1) from CRMProject where TenantID=@tenantID and Closed = 1 and  DivisionLookup=t.DivisionLookup 
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'committedprojects',

			(
				select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and  DivisionLookup=t.DivisionLookup
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  )  and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'committedrevenue',

			(
				select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=t.DivisionLookup
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  )
				group by DivisionLookup
			) 'pastrevenue',
			Round((sum(r.PctAllocation)/count(r.PctAllocation)),2) 'Utilization'
			from (
				Select TicketId, DivisionLookup from CRMProject where Closed = 1 and TenantID=@tenantID
				and ( (LEN(@sector) > 0  AND SectorChoice=@sector )  OR (LEN(@sector) = 0  AND SectorChoice is not null)  ) and 
				( (@studio > 0  AND StudioLookup=@studio )  OR (@studio = 0  AND StudioLookup is not null)  ) and
				 CloseDate > DATEFROMPARTS(YEAR(GETDATE()), 1, 1)
				--(CloseDate > GETDATE() or CloseDate is null)
			) t inner join ResourceAllocation r on r.TicketID=t.TicketId
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			group by t.DivisionLookup
			) anu where 1=1 and LEN(Name) > 0
	End;
	
End;

--exec usp_GetCommonSectorChartData 'bcd2d0c9-9947-4a0b-9fbf-73ea61035069', '', 'Contracted'
declare @tenantID nvarchar(max) = 'bcd2d0c9-9947-4a0b-9fbf-73ea61035069'
declare @division bigint = 69
declare @filter nvarchar(max) = ''
declare @studio bigint = 333

declare @rewardedStage int;
		set @rewardedStage = 0;
		select @rewardedStage=stagestep from Config_Module_ModuleStages where StageTypeChoice = 'Resolved' and ModuleNameLookup='CPR' and TenantID=@tenantID

select  divisionlookup, StudioLookup, SectorChoice, ApproxContractValue from
(Select divisionlookup, StudioLookup, SectorChoice, ApproxContractValue from CRMProject where StageStep <= @rewardedStage and TenantID=@tenantID
union all
Select divisionlookup, StudioLookup, SectorChoice, ApproxContractValue from Opportunity where TenantID=@tenantID) temp
where ( divisionlookup = (case when @division > 0 then @division else divisionlookup end)  )
and ( StudioLookup = (case when @studio > 0 then @studio else StudioLookup end)  )
--group by SectorChoice






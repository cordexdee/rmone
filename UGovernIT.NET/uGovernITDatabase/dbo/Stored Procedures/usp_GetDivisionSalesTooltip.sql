
ALTER procedure [dbo].[usp_GetDivisionSalesTooltip]
--exec usp_GetDivisionSalesTooltip '35525396-E5FE-4692-9239-4DF9305B915B',12
@tenantID nvarchar(max),
@Division bigint
as
begin
		
		declare @nofresource int;
		declare @totalUtalization int;
		declare @committedprojects int;
		declare @committedRevenue nvarchar(max);
		declare @hotreveneue nvarchar(max);
		declare @hotproject int;
		declare @pastrevenue nvarchar(max);
		DECLARE @returnSet TABLE (NAME VARCHAR(max), Value nvarchar(max))

		set @nofresource = ( select count(ra.TicketID)
					from   (Select TicketId, ApproxContractValue, Title, StageStep from CRMProject where StageStep <= 7 and TenantID=@tenantID and DivisionLookup=@Division
					union all Select TicketId, ApproxContractValue, Title, StageStep  from Opportunity where TenantID=@tenantID and DivisionLookup=@Division) projects
						inner join ResourceAllocation ra on projects.TicketId=ra.TicketID ) 
		insert into @returnSet values('resources', @nofresource);

		set @hotproject = ( select count(1)
					from (Select TicketId, ApproxContractValue, Title, StageStep from CRMProject where StageStep <= 7 and TenantID=@tenantID and DivisionLookup=@Division
					union all Select TicketId, ApproxContractValue, Title, StageStep  from Opportunity where TenantID=@tenantID and DivisionLookup=@Division) projects )
		insert into @returnSet values('hotproject', @hotproject);

		set @hotreveneue = ( select round(sum(ApproxContractValue),2)
					from   (Select TicketId, ApproxContractValue, Title, StageStep from CRMProject where StageStep <= 7 and TenantID=@tenantID 
					union all Select TicketId, ApproxContractValue, Title, StageStep  from Opportunity where TenantID=@tenantID) projects )
		insert into @returnSet values('hotrevenue', @hotreveneue);

		set @committedprojects = (select count(*) from CRMProject where TenantID=@tenantID and Closed != 1 and DivisionLookup=@Division)
		insert into @returnSet values('committedprojects', @committedprojects);

		set @committedRevenue = (select round(SUM(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed != 1 and DivisionLookup=@Division)
		insert into @returnSet values('committedrevenue', @committedRevenue);
		set @pastrevenue = ( select round(sum(ApproxContractValue),2) from CRMProject where TenantID=@tenantID and Closed = 1 and DivisionLookup=@Division)
		insert into @returnSet values('pastrevenue', @pastrevenue);

		select * from @returnSet

End;

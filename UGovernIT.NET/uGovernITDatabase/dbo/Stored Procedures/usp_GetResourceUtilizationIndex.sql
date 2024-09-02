

-----

-- usp_GetResourceUtilizationIndex '35525396-e5fe-4692-9239-4df9305b915b','2021','sector','pct'
-- usp_GetResourceUtilizationIndex '35525396-e5fe-4692-9239-4df9305b915b','2021','division','fte'
-- usp_GetResourceUtilizationIndex '35525396-e5fe-4692-9239-4df9305b915b','2021','studio','pct'
ALTER PROCEDURE [dbo].[usp_GetResourceUtilizationIndex]
	@TenantId nvarchar(256),
	@Year varchar(5),
	@Category nvarchar(50),
	@Type nvarchar(50)
AS
BEGIN
	Declare @i int;
	Declare @count int;
	Declare @workingDays int;

	Declare @StartDate date;
	Declare @EndDate date;

	Declare @CategoryName nvarchar(1000);

	Declare @Jan decimal(18,2);
	Declare @Feb decimal(18,2);
	Declare @Mar decimal(18,2);
	Declare @Apr decimal(18,2);
	Declare @May decimal(18,2);
	Declare @Jun decimal(18,2);
	Declare @Jul decimal(18,2);
	Declare @Aug decimal(18,2);
	Declare @Sep decimal(18,2);
	Declare @Oct decimal(18,2);
	Declare @Nov decimal(18,2);
	Declare @Dec decimal(18,2);

	Declare @Jan_RC decimal(18,2);
	Declare @Feb_RC decimal(18,2);
	Declare @Mar_RC decimal(18,2);
	Declare @Apr_RC decimal(18,2);
	Declare @May_RC decimal(18,2);
	Declare @Jun_RC decimal(18,2);
	Declare @Jul_RC decimal(18,2);
	Declare @Aug_RC decimal(18,2);
	Declare @Sep_RC decimal(18,2);
	Declare @Oct_RC decimal(18,2);
	Declare @Nov_RC decimal(18,2);
	Declare @Dec_RC decimal(18,2);

	set @StartDate = @Year + '-' + '01' + '-' + '01';
	set @EndDate = @Year + '-' + '12' + '-' + '31';

	CREATE TABLE #tmpOpenTickets(	
		TicketId nvarchar(100),	
	)
	insert into #tmpOpenTickets
	select O.ticketid from Opportunity O where O.TenantID = @TenantId and (O.closed = 0 or O.closed is null)
	union
	select P.ticketid from CRMProject P where P.TenantID = @TenantId and (P.closed = 0 or P.closed is null)
	union
	select S.ticketid from CRMServices S where S.TenantID = @TenantId and (S.closed = 0 or S.closed is null)


	CREATE TABLE #tmpResourceAllocationMonthly(	
		MonthStartDate datetime,
		ResourceUser nvarchar(256),
		PctAllocation float,
		ResourceWorkItem nvarchar(250),		
		TenantID nvarchar(128),		
		Sector nvarchar(250),
		Division bigint,
		Studio bigint
	)

	insert into #tmpResourceAllocationMonthly (
		MonthStartDate,
		ResourceUser,
		PctAllocation,
		ResourceWorkItem,		
		TenantID	
	) 
	select 
	MonthStartDate,
	ResourceUser,
	PctAllocation,
	ResourceWorkItem,		
	TenantID
	from ResourceAllocationMonthly where TenantID = @TenantId and MonthStartDate between @StartDate and @EndDate;

	update tmp
	set tmp.Sector = cpr.SectorChoice, tmp.Division = cpr.DivisionLookup, tmp.Studio = cpr.StudioLookup
	from #tmpResourceAllocationMonthly tmp
	join CRMProject cpr
	on tmp.ResourceWorkItem = cpr.TicketId
	where cpr.TenantID = @TenantId;

	update tmp
	set tmp.Sector = opm.SectorChoice, tmp.Division = opm.DivisionLookup, tmp.Studio = opm.StudioLookup
	from #tmpResourceAllocationMonthly tmp
	join Opportunity opm
	on tmp.ResourceWorkItem = opm.TicketId
	where opm.TenantID = @TenantId;

	update tmp
	set tmp.Division = cns.DivisionLookup, tmp.Studio = cns.StudioLookup
	from #tmpResourceAllocationMonthly tmp
	join CRMServices cns
	on tmp.ResourceWorkItem = cns.TicketId
	where cns.TenantID = @TenantId;
	

	CREATE TABLE #tmpKPItable(
		Category nvarchar(256),
		KPI nvarchar(256),
		Jan decimal(18,2),
		Feb decimal(18,2),
		Mar decimal(18,2),
		Apr decimal(18,2),
		May decimal(18,2),
		Jun decimal(18,2),
		Jul decimal(18,2),
		Aug decimal(18,2),
		Sep decimal(18,2),
		Oct decimal(18,2),
		Nov decimal(18,2),
		[Dec] decimal(18,2),
		DataType nvarchar(50)
	)

	create table #tmpKPI
	(
		MonthName varchar(5),
		ItemValue decimal(18,2)
	)

	DECLARE @PipelinedTickets TABLE
	(
		TicketId varchar(30),
		ApproxContractValue float,
		Sector nvarchar(250),
		Division bigint,
		Studio bigint,
		PreconStartDt datetime,
		ConstructionEndDt datetime
	)	
	Insert into @PipelinedTickets select TicketId, ApproxContractValue, SectorChoice, Division, Studio, PreconStartDt, ConstructionEndDt 
	from dbo.GetPipelineProjects(@TenantID,'0','','','',0,0);

	DECLARE @ContractedTickets TABLE
	(
		TicketId varchar(30),
		ApproxContractValue float,
		Sector nvarchar(250),
		Division bigint,
		Studio bigint,
		PreconStartDt datetime,
		ConstructionEndDt datetime
	)	
	Insert into @ContractedTickets select TicketId, ApproxContractValue, SectorChoice, Division, Studio, PreconStartDt, ConstructionEndDt 
	from dbo.GetContractedProjects(@TenantID,'0','','','',0,0);
	
	DECLARE @tblCategory TABLE
	(
		Id int Identity,
		Category nvarchar(250)
	)	

	If @Category = 'sector'
		Begin
			Insert into @tblCategory (Category)
			select distinct Sector from @PipelinedTickets where Len(Sector) > 0
			Union
			select distinct Sector from @ContractedTickets where Len(Sector) > 0
		End
	Else If  @Category = 'division'
		Begin
			Insert into @tblCategory (Category)
			select distinct Division from @PipelinedTickets where Division > 0
			Union
			select distinct Division from @ContractedTickets where Division > 0
		End
	Else If  @Category = 'studio'
		Begin
			Insert into @tblCategory (Category)
			select distinct Studio from @PipelinedTickets where Studio > 0
			Union
			select distinct Studio from @ContractedTickets where Studio > 0
		End

	set @i = 1;
	select @count = count(*) from @tblCategory;
	While @i <= @count
	Begin
		select @Jan = 0, @Feb = 0, @Mar = 0, @Apr = 0, @May = 0, @Jun = 0, @Jul = 0, @Aug = 0, @Sep = 0, @Oct = 0, @Nov = 0, @Dec = 0;
		select @Jan_RC = 0, @Feb_RC = 0, @Mar_RC = 0, @Apr_RC = 0, @May_RC = 0, @Jun_RC = 0, @Jul_RC = 0, @Aug_RC = 0, @Sep_RC = 0, @Oct_RC = 0, @Nov_RC = 0, @Dec_RC = 0;
		select @CategoryName = Category from @tblCategory where Id = @i;

		If @Category = 'sector'
		Begin
				If @Type = 'fte'
					Begin

								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
											join aspnetusers u on u.id = RA.ResourceUser
											where RA.TenantID = @TenantId
											and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Sector = @CategoryName
											and u.Enabled = 1
											and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
											group by RA.MonthStartDate, RA.Sector;

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;


								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'),(sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.MonthStartDate, RA.Sector

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------								

					End
				Else If @Type = 'pct'
					Begin
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'),(sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.MonthStartDate, RA.Sector
								--select * from #tmpKPI

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')
								

								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.MonthStartDate, RA.Sector
								
								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;


								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------	
					End

				----------------------------------------------------
				select @Jan = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Feb = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Mar = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Apr = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @May = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Jun = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Jul = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Aug = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Sep = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Oct = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Nov = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Dec = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Committed Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')

		
				select @Jan = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Feb = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Mar = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Apr = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @May = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Jun = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Jul = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Aug = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Sep = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Oct = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Nov = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 
				select @Dec = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Pipeline Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
		
				-----------------------------------------------------------------------
		
				select @Jan = count(distinct TicketId) from @ContractedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Feb = count(distinct TicketId) from @ContractedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Mar = count(distinct TicketId) from @ContractedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Apr = count(distinct TicketId) from @ContractedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @May = count(distinct TicketId) from @ContractedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Jun = count(distinct TicketId) from @ContractedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Jul = count(distinct TicketId) from @ContractedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Aug = count(distinct TicketId) from @ContractedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Sep = count(distinct TicketId) from @ContractedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Oct = count(distinct TicketId) from @ContractedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Nov = count(distinct TicketId) from @ContractedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Dec = count(distinct TicketId) from @ContractedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;

				Insert into #tmpKPItable values (@CategoryName, 'Commited project counts',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'numberwithoutdecimal')
				
				select @Jan = count(distinct TicketId) from @PipelinedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Feb = count(distinct TicketId) from @PipelinedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Mar = count(distinct TicketId) from @PipelinedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Apr = count(distinct TicketId) from @PipelinedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @May = count(distinct TicketId) from @PipelinedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Jun = count(distinct TicketId) from @PipelinedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Jul = count(distinct TicketId) from @PipelinedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Aug = count(distinct TicketId) from @PipelinedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Sep = count(distinct TicketId) from @PipelinedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Oct = count(distinct TicketId) from @PipelinedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Nov = count(distinct TicketId) from @PipelinedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;
				select @Dec = count(distinct TicketId) from @PipelinedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Sector = @CategoryName;

				Insert into #tmpKPItable values (@CategoryName, 'Pipeline project counts',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'numberwithoutdecimal')

				------------------------------

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-01-01',EOMONTH(@Year + '-01-01'), @TenantID);				
				Select 
				@Jan = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jan_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-01-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-02-01',EOMONTH(@Year + '-02-01'), @TenantID);
				Select 
				@Feb = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Feb_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-02-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-03-01',EOMONTH(@Year + '-03-01'), @TenantID);
				Select 
				@Mar = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Mar_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-03-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-04-01',EOMONTH(@Year + '-04-01'), @TenantID);
				Select 
				@Apr = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Apr_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-04-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-05-01',EOMONTH(@Year + '-05-01'), @TenantID);
				Select 
				@May = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@May_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-05-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-06-01',EOMONTH(@Year + '-06-01'), @TenantID);
				Select 
				@Jun = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jun_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-06-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-07-01',EOMONTH(@Year + '-07-01'), @TenantID);
				Select 
				@Jul = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jul_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-07-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-08-01',EOMONTH(@Year + '-08-01'), @TenantID);
				Select 
				@Aug = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Aug_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-08-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-09-01',EOMONTH(@Year + '-09-01'), @TenantID);
				Select 
				@Sep = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Sep_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-09-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-10-01',EOMONTH(@Year + '-10-01'), @TenantID);
				Select 
				@Oct = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Oct_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-10-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-11-01',EOMONTH(@Year + '-11-01'), @TenantID);
				Select 
				@Nov = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Nov_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-11-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-12-01',EOMONTH(@Year + '-12-01'), @TenantID);
				Select 
				@Dec = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Dec_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Sector = @CategoryName  and (ra.MonthStartDate = @Year + '-12-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				Insert into #tmpKPItable values (@CategoryName, 'Potential Resource Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				Insert into #tmpKPItable values (@CategoryName, 'Resource Costs',@Jan_RC,@Feb_RC,@Mar_RC,@Apr_RC,@May_RC,@Jun_RC,@Jul_RC,@Aug_RC,@Sep_RC,@Oct_RC,@Nov_RC,@Dec_RC,'currency')

				delete from #tmpKPI;
				insert into #tmpKPI  
				Select LEFT(DATENAME(m, mk.MonthStartDate), 3), Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)
								
				select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
				from
				(
					select MonthName, ItemValue
					from #tmpKPI
				) d
				pivot
				(
					max(ItemValue)
					for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
				) piv;

				Insert into #tmpKPItable values (@CategoryName, 'Actual Billings',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				-----------------------------
				Insert into #tmpKPItable --values (@CategoryName, 'Missed Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				SELECT @CategoryName, 'Missed Revenues', (t1.Jan - t2.Jan) AS Jan, (t1.Feb - t2.Feb) AS Feb, (t1.Mar - t2.Mar) AS Mar, (t1.Apr - t2.Apr) AS Apr, (t1.May - t2.May) AS May, (t1.Jun - t2.Jun) AS Jun, (t1.Jul - t2.Jul) AS Jul, (t1.Aug - t2.Aug) AS Aug, (t1.Sep - t2.Sep) AS Sep, (t1.Oct - t2.Oct) AS Oct, (t1.Nov - t2.Nov) AS Nov, (t1.Dec - t2.Dec) AS Dec, 'currency'
				FROM #tmpKPItable t1 CROSS JOIN
					 #tmpKPItable t2
				WHERE t1.KPI = 'Potential Resource Revenues' AND t2.kpi = 'Actual Billings' and t1.Category = @CategoryName and t2.Category = @CategoryName;
				-------------------------------
				Insert into #tmpKPItable --values (@CategoryName, 'Actual Margins',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				SELECT @CategoryName, 'Actual Margins', (t1.Jan - t2.Jan) AS Jan, (t1.Feb - t2.Feb) AS Feb, (t1.Mar - t2.Mar) AS Mar, (t1.Apr - t2.Apr) AS Apr, (t1.May - t2.May) AS May, (t1.Jun - t2.Jun) AS Jun, (t1.Jul - t2.Jul) AS Jul, (t1.Aug - t2.Aug) AS Aug, (t1.Sep - t2.Sep) AS Sep, (t1.Oct - t2.Oct) AS Oct, (t1.Nov - t2.Nov) AS Nov, (t1.Dec - t2.Dec) AS Dec, 'currency'
				FROM #tmpKPItable t1 CROSS JOIN
					 #tmpKPItable t2
				WHERE t1.KPI = 'Actual Billings' AND t2.kpi = 'Resource Costs' and t1.Category = @CategoryName and t2.Category = @CategoryName;				
		End			
		Else If @Category = 'division'
		Begin
				If @Type = 'fte'
					Begin

								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
											join aspnetusers u on u.id = RA.ResourceUser
											where RA.TenantID = @TenantId
											and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Division = @CategoryName
											and u.Enabled = 1
											and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
											group by RA.MonthStartDate, RA.Division;
								
								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.MonthStartDate, RA.Division;

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;
								

								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------								

					End
				Else If @Type = 'pct'
					Begin
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31' and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.MonthStartDate, RA.Division;

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31' and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.MonthStartDate, RA.Division;

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------	
					End

				----------------------------------------
				select @Jan = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Feb = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Mar = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Apr = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @May = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jun = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jul = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Aug = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Sep = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Oct = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Nov = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Dec = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Committed Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')

		
				select @Jan = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Feb = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Mar = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Apr = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @May = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jun = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jul = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Aug = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Sep = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Oct = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Nov = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Dec = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Pipeline Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
		
				-----------------------------------------------------------------------
		
				select @Jan = count(distinct TicketId) from @ContractedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Feb = count(distinct TicketId) from @ContractedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Mar = count(distinct TicketId) from @ContractedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Apr = count(distinct TicketId) from @ContractedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @May = count(distinct TicketId) from @ContractedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jun = count(distinct TicketId) from @ContractedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jul = count(distinct TicketId) from @ContractedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Aug = count(distinct TicketId) from @ContractedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Sep = count(distinct TicketId) from @ContractedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Oct = count(distinct TicketId) from @ContractedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Nov = count(distinct TicketId) from @ContractedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Dec = count(distinct TicketId) from @ContractedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Commited project counts',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'numberwithoutdecimal')


				select @Jan = count(distinct TicketId) from @PipelinedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Feb = count(distinct TicketId) from @PipelinedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Mar = count(distinct TicketId) from @PipelinedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Apr = count(distinct TicketId) from @PipelinedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @May = count(distinct TicketId) from @PipelinedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jun = count(distinct TicketId) from @PipelinedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Jul = count(distinct TicketId) from @PipelinedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Aug = count(distinct TicketId) from @PipelinedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Sep = count(distinct TicketId) from @PipelinedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Oct = count(distinct TicketId) from @PipelinedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Nov = count(distinct TicketId) from @PipelinedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 
				select @Dec = count(distinct TicketId) from @PipelinedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Division = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Pipeline project counts',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'numberwithoutdecimal')

				------------------------------

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-01-01',EOMONTH(@Year + '-01-01'), @TenantID);
				Select 
				@Jan = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jan_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-01-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-02-01',EOMONTH(@Year + '-02-01'), @TenantID);
				Select 
				@Feb = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Feb_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-02-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-03-01',EOMONTH(@Year + '-03-01'), @TenantID);
				Select 
				@Mar = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Mar_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-03-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-04-01',EOMONTH(@Year + '-04-01'), @TenantID);
				Select 
				@Apr = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Apr_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-04-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-05-01',EOMONTH(@Year + '-05-01'), @TenantID);
				Select 
				@May = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@May_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-05-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-06-01',EOMONTH(@Year + '-06-01'), @TenantID);
				Select 
				@Jun = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jun_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-06-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-07-01',EOMONTH(@Year + '-07-01'), @TenantID);
				Select 
				@Jul = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jul_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-07-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-08-01',EOMONTH(@Year + '-08-01'), @TenantID);
				Select 
				@Aug = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Aug_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-08-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-09-01',EOMONTH(@Year + '-09-01'), @TenantID);
				Select 
				@Sep = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Sep_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-09-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-10-01',EOMONTH(@Year + '-10-01'), @TenantID);
				Select 
				@Oct = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Oct_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-10-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-11-01',EOMONTH(@Year + '-11-01'), @TenantID);
				Select 
				@Nov = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Nov_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-11-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-12-01',EOMONTH(@Year + '-12-01'), @TenantID);
				Select 
				@Dec = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Dec_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Division = @CategoryName  and (ra.MonthStartDate = @Year + '-12-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				Insert into #tmpKPItable values (@CategoryName, 'Potential Resource Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				Insert into #tmpKPItable values (@CategoryName, 'Resource Costs',@Jan_RC,@Feb_RC,@Mar_RC,@Apr_RC,@May_RC,@Jun_RC,@Jul_RC,@Aug_RC,@Sep_RC,@Oct_RC,@Nov_RC,@Dec_RC,'currency')
					
				
				delete from #tmpKPI;
				insert into #tmpKPI  
				Select LEFT(DATENAME(m, mk.MonthStartDate), 3), Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.DivisionLookup = @CategoryName or OPM.DivisionLookup = @CategoryName or CNS.DivisionLookup = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)
				
				select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
				from
				(
					select MonthName, ItemValue
					from #tmpKPI
				) d
				pivot
				(
					max(ItemValue)
					for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
				) piv;

				Insert into #tmpKPItable values (@CategoryName, 'Actual Billings',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				-----------------------------
				Insert into #tmpKPItable --values (@CategoryName, 'Missed Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				SELECT @CategoryName, 'Missed Revenues', (t1.Jan - t2.Jan) AS Jan, (t1.Feb - t2.Feb) AS Feb, (t1.Mar - t2.Mar) AS Mar, (t1.Apr - t2.Apr) AS Apr, (t1.May - t2.May) AS May, (t1.Jun - t2.Jun) AS Jun, (t1.Jul - t2.Jul) AS Jul, (t1.Aug - t2.Aug) AS Aug, (t1.Sep - t2.Sep) AS Sep, (t1.Oct - t2.Oct) AS Oct, (t1.Nov - t2.Nov) AS Nov, (t1.Dec - t2.Dec) AS Dec, 'currency'
				FROM #tmpKPItable t1 CROSS JOIN
					 #tmpKPItable t2
				WHERE t1.KPI = 'Potential Resource Revenues' AND t2.kpi = 'Actual Billings' and t1.Category = @CategoryName and t2.Category = @CategoryName;
				-------------------------------
				Insert into #tmpKPItable --values (@CategoryName, 'Actual Margins',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				SELECT @CategoryName, 'Actual Margins', (t1.Jan - t2.Jan) AS Jan, (t1.Feb - t2.Feb) AS Feb, (t1.Mar - t2.Mar) AS Mar, (t1.Apr - t2.Apr) AS Apr, (t1.May - t2.May) AS May, (t1.Jun - t2.Jun) AS Jun, (t1.Jul - t2.Jul) AS Jul, (t1.Aug - t2.Aug) AS Aug, (t1.Sep - t2.Sep) AS Sep, (t1.Oct - t2.Oct) AS Oct, (t1.Nov - t2.Nov) AS Nov, (t1.Dec - t2.Dec) AS Dec, 'currency'
				FROM #tmpKPItable t1 CROSS JOIN
					 #tmpKPItable t2
				WHERE t1.KPI = 'Actual Billings' AND t2.kpi = 'Resource Costs' and t1.Category = @CategoryName and t2.Category = @CategoryName;	


		End
		Else If @Category = 'studio'
		Begin
				If @Type = 'fte'
					Begin
					
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.MonthStartDate, RA.Studio

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31' and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.MonthStartDate, RA.Studio

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------								

					End
				Else If @Type = 'pct'
					Begin
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31' and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.MonthStartDate, RA.Studio
								
								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')
								
								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31' and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.MonthStartDate, RA.Studio

								select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
								from
								(
								  select MonthName, ItemValue
								  from #tmpKPI
								) d
								pivot
								(
								  max(ItemValue)
								  for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
								) piv;

								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'percentage')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------	
					End

				---------------------------------
				select @Jan = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Feb = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Mar = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Apr = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @May = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Jun = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Jul = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Aug = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Sep = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Oct = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Nov = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Dec = sum(IsNULL(ApproxContractValue, 0)) from @ContractedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Committed Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')

		
				select @Jan = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Feb = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Mar = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Apr = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @May = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Jun = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Jul = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Aug = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Sep = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Oct = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Nov = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 
				select @Dec = sum(IsNULL(ApproxContractValue, 0)) from @PipelinedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName; 

				Insert into #tmpKPItable values (@CategoryName, 'Pipeline Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
		
				-----------------------------------------------------------------------
		
				select @Jan = count(distinct TicketId) from @ContractedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Feb = count(distinct TicketId) from @ContractedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Mar = count(distinct TicketId) from @ContractedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Apr = count(distinct TicketId) from @ContractedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @May = count(distinct TicketId) from @ContractedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Jun = count(distinct TicketId) from @ContractedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Jul = count(distinct TicketId) from @ContractedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Aug = count(distinct TicketId) from @ContractedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Sep = count(distinct TicketId) from @ContractedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Oct = count(distinct TicketId) from @ContractedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Nov = count(distinct TicketId) from @ContractedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Dec = count(distinct TicketId) from @ContractedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;

				Insert into #tmpKPItable values (@CategoryName, 'Commited project counts',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'numberwithoutdecimal')


				select @Jan = count(distinct TicketId) from @PipelinedTickets where @Year + '-01-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Feb = count(distinct TicketId) from @PipelinedTickets where @Year + '-02-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Mar = count(distinct TicketId) from @PipelinedTickets where @Year + '-03-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Apr = count(distinct TicketId) from @PipelinedTickets where @Year + '-04-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @May = count(distinct TicketId) from @PipelinedTickets where @Year + '-05-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Jun = count(distinct TicketId) from @PipelinedTickets where @Year + '-06-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Jul = count(distinct TicketId) from @PipelinedTickets where @Year + '-07-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Aug = count(distinct TicketId) from @PipelinedTickets where @Year + '-08-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Sep = count(distinct TicketId) from @PipelinedTickets where @Year + '-09-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Oct = count(distinct TicketId) from @PipelinedTickets where @Year + '-10-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Nov = count(distinct TicketId) from @PipelinedTickets where @Year + '-11-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;
				select @Dec = count(distinct TicketId) from @PipelinedTickets where @Year + '-12-01' between PreconStartDt and ConstructionEndDt and Studio = @CategoryName;

				Insert into #tmpKPItable values (@CategoryName, 'Pipeline project counts',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'numberwithoutdecimal')

				------------------------------

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-01-01',EOMONTH(@Year + '-01-01'), @TenantId);

				Select 
				@Jan = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jan_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-01-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-02-01',EOMONTH(@Year + '-02-01'), @TenantId);
				Select 
				@Feb = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Feb_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-02-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-03-01',EOMONTH(@Year + '-03-01'), @TenantId);
				Select 
				@Mar = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Mar_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-03-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-04-01',EOMONTH(@Year + '-04-01'), @TenantId);
				Select 
				@Apr = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Apr_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-04-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-05-01',EOMONTH(@Year + '-05-01'), @TenantId);
				Select 
				@May = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@May_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-05-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-06-01',EOMONTH(@Year + '-06-01'), @TenantId);
				Select 
				@Jun = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jun_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-06-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-07-01',EOMONTH(@Year + '-07-01'), @TenantId);
				Select 
				@Jul = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Jul_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-07-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-08-01',EOMONTH(@Year + '-08-01'), @TenantId);
				Select 
				@Aug = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Aug_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-08-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-09-01',EOMONTH(@Year + '-09-01'), @TenantId);
				Select 
				@Sep = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Sep_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-09-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-10-01',EOMONTH(@Year + '-10-01'), @TenantId);
				Select 
				@Oct = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Oct_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-10-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-11-01',EOMONTH(@Year + '-11-01'), @TenantId);
				Select 
				@Nov = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Nov_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-11-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				set @workingDays = dbo.fnGetWorkingDays(@Year + '-12-01',EOMONTH(@Year + '-12-01'), @TenantId);
				Select 
				@Dec = Sum((((isnull(j.BillingLaborRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)), --TotalEmpBillingCost,
				@Dec_RC = Sum((((isnull(j.EmployeeCostRate,0)*8)* @workingDays)*(100-isnull(r.Allocation,0))/100)) --TotalEmpCost
				from AspNetUsers u left join (select RA.ResourceUser as ResourceUser, SUM(RA.PctAllocation) as Allocation
				from #tmpResourceAllocationMonthly RA
				where RA.TenantID=@TenantID and RA.Studio = @CategoryName  and (ra.MonthStartDate = @Year + '-12-01')
				group by RA.ResourceUser
				having SUM(RA.PctAllocation) < 100) r on r.ResourceUser=u.id
				left join JobTitle j on j.ID=isnull(u.JobTitleLookup,'')
				where u.TenantID=@TenantID
				and j.JobType = 'Billable'
				and isnull(r.Allocation,0) = 0
				and u.Enabled = 1

				Insert into #tmpKPItable values (@CategoryName, 'Potential Resource Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				Insert into #tmpKPItable values (@CategoryName, 'Resource Costs',@Jan_RC,@Feb_RC,@Mar_RC,@Apr_RC,@May_RC,@Jun_RC,@Jul_RC,@Aug_RC,@Sep_RC,@Oct_RC,@Nov_RC,@Dec_RC,'currency')
					
				
				delete from #tmpKPI;
				insert into #tmpKPI  
				Select LEFT(DATENAME(m, mk.MonthStartDate), 3), Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioLookup = @CategoryName or OPM.StudioLookup = @CategoryName or CNS.StudioLookup = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)
				
				select @Jan = Jan, @Feb = Feb, @Mar = Mar, @Apr = Apr, @May = May, @Jun = Jun, @Jul = Jul, @Aug = Aug, @Sep = Sep, @Oct = Oct, @Nov = Nov , @Dec = Dec
				from
				(
					select MonthName, ItemValue
					from #tmpKPI
				) d
				pivot
				(
					max(ItemValue)
					for MonthName in (Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov , Dec)
				) piv;

				Insert into #tmpKPItable values (@CategoryName, 'Actual Billings',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				-----------------------------
				Insert into #tmpKPItable --values (@CategoryName, 'Missed Revenues',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				SELECT @CategoryName, 'Missed Revenues', (t1.Jan - t2.Jan) AS Jan, (t1.Feb - t2.Feb) AS Feb, (t1.Mar - t2.Mar) AS Mar, (t1.Apr - t2.Apr) AS Apr, (t1.May - t2.May) AS May, (t1.Jun - t2.Jun) AS Jun, (t1.Jul - t2.Jul) AS Jul, (t1.Aug - t2.Aug) AS Aug, (t1.Sep - t2.Sep) AS Sep, (t1.Oct - t2.Oct) AS Oct, (t1.Nov - t2.Nov) AS Nov, (t1.Dec - t2.Dec) AS Dec, 'currency'
				FROM #tmpKPItable t1 CROSS JOIN
					 #tmpKPItable t2
				WHERE t1.KPI = 'Potential Resource Revenues' AND t2.kpi = 'Actual Billings' and t1.Category = @CategoryName and t2.Category = @CategoryName;
				-------------------------------
				Insert into #tmpKPItable --values (@CategoryName, 'Actual Margins',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'currency')
				SELECT @CategoryName, 'Actual Margins', (t1.Jan - t2.Jan) AS Jan, (t1.Feb - t2.Feb) AS Feb, (t1.Mar - t2.Mar) AS Mar, (t1.Apr - t2.Apr) AS Apr, (t1.May - t2.May) AS May, (t1.Jun - t2.Jun) AS Jun, (t1.Jul - t2.Jul) AS Jul, (t1.Aug - t2.Aug) AS Aug, (t1.Sep - t2.Sep) AS Sep, (t1.Oct - t2.Oct) AS Oct, (t1.Nov - t2.Nov) AS Nov, (t1.Dec - t2.Dec) AS Dec, 'currency'
				FROM #tmpKPItable t1 CROSS JOIN
					 #tmpKPItable t2
				WHERE t1.KPI = 'Actual Billings' AND t2.kpi = 'Resource Costs' and t1.Category = @CategoryName and t2.Category = @CategoryName;	

		End


		set @i = @i + 1;
	End

	--Declare @sql nvarchar(max) = ''
	--IF @Category = 'sector'
	--Begin
	--	set @sql  = 'select * from #tmpKPItable'
	--End;
	--Else If @Category = 'division'
	--Begin
	--	set @sql  = 'select cd.Title as [Category], KPI, Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec, DataType
	--	from #tempKPItable temp left join CompanyDivisions cd on CAST(temp.Category as bigint) = cd.ID'
	--End;
	--Else If @Category = 'studio'
	--Begin
	--	set @sql  = 'select s.FieldDisplayName as [Category], KPI, Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec, DataType
	--	from #tempKPItable temp left join Studio s on CAST(temp.Category as bigint) = s.ID'
	--End;

	
	--EXEC sp_executesql @sql;
	select * from #tmpKPItable
	Drop table #tmpOpenTickets;
	Drop table #tmpResourceAllocationMonthly;
	Drop table #tmpKPItable;
	Drop table #tmpKPI;
END




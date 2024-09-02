
ALTER FUNCTION [dbo].[fnGetWorkingDays]
(
	@StartDate DATETIME,
	@EndDate DATETIME,
	@TenantID varchar(128)
)
RETURNS INT
AS
BEGIN
	Declare @WorkingDays int;
	Declare @Holidays int;
	Set @Holidays = 0;

	  Set @WorkingDays = (DATEDIFF(dd, @StartDate, @EndDate) + 1)				-- Returning No of days in the Date range
		  -(DATEDIFF(wk, @StartDate, @EndDate) * 2)								-- Returning No of Weeks in the Date range and multiplying with 2 (counting for Saturday & Sunday)
		  -(CASE WHEN DATENAME(dw, @StartDate) = 'Sunday' THEN 1 ELSE 0 END)	-- Returning No of days if Start Date is Sunday (Need to exclude this day, if any)
		  -(CASE WHEN DATENAME(dw, @EndDate) = 'Saturday' THEN 1 ELSE 0 END);	-- Returning No of days if Start Date is Saturday (Need to exclude this day, if any)

	  select @Holidays = count(*) from Appointment where TenantID = @TenantID 
		and DATENAME(dw, StartTime) NOT IN ('Saturday','Sunday')
		and StartTime between @StartDate and @EndDate	

	  RETURN (@WorkingDays - @Holidays);	  
END

-- select dbo.fnGetWorkingDays('2021-01-01','2021-12-31','35525396-e5fe-4692-9239-4df9305b915b')
-- select dbo.fnGetWorkingDays('2023-06-03','2023-06-10','35525396-e5fe-4692-9239-4df9305b915b')
/*
select (DATEDIFF(dd, '2023-06-03','2023-06-10') + 1) 
select DATEDIFF(wk, '2023-06-03','2023-06-10') * 2
select (CASE WHEN DATENAME(dw, '2023-06-03') = 'Sunday' THEN 1 ELSE 0 END)
select (CASE WHEN DATENAME(dw, '2023-06-10') = 'Saturday' THEN 1 ELSE 0 END);
*/


Go
---------------

ALTER FUNCTION [dbo].[fnGetWorkingDaysTillDate]  
(  
  @month int  
)  
RETURNS INT  
AS  
BEGIN  
 Declare @StartDate Datetime=''  ,@EndDate Datetime='', @Days int=0;  
 Select @StartDate = dateadd(month, @month - 1, dateadd(year, Year(getDate()) - 1900, 0))  
    Select @EndDate  = dateadd(month, @month,dateadd(year, Year(getDate()) - 1900, -1))   
 --If(Month(@EndDate)=MONTH(GETDATE()))  
 --begin  
 --SET @EndDate=Getdate()  
 --end  
 Set @Days= (select dbo.fnGetWorkingDays(@StartDate,@EndDate,'35525396-E5FE-4692-9239-4DF9305B915B'))  
 RETURN @Days  
END  

Go

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
		Division nvarchar(250),
		Studio nvarchar(250)
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
	set tmp.Sector = cpr.SectorChoice, tmp.Division = cpr.CRMBusinessUnitChoice, tmp.Studio = cpr.StudioChoice
	from #tmpResourceAllocationMonthly tmp
	join CRMProject cpr
	on tmp.ResourceWorkItem = cpr.TicketId
	where cpr.TenantID = @TenantId;

	update tmp
	set tmp.Sector = opm.SectorChoice, tmp.Division = opm.CRMBusinessUnitChoice, tmp.Studio = opm.StudioChoice
	from #tmpResourceAllocationMonthly tmp
	join Opportunity opm
	on tmp.ResourceWorkItem = opm.TicketId
	where opm.TenantID = @TenantId;

	update tmp
	set tmp.Division = cns.CRMBusinessUnitChoice, tmp.Studio = cns.StudioChoice
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
		Division nvarchar(250),
		Studio nvarchar(250),
		PreconStartDt datetime,
		ConstructionEndDt datetime
	)	
	Insert into @PipelinedTickets select TicketId, ApproxContractValue, SectorChoice, Division, Studio, PreconStartDt, ConstructionEndDt from dbo.GetPipelineProjects(@TenantID,'0','','','','','');

	DECLARE @ContractedTickets TABLE
	(
		TicketId varchar(30),
		ApproxContractValue float,
		Sector nvarchar(250),
		Division nvarchar(250),
		Studio nvarchar(250),
		PreconStartDt datetime,
		ConstructionEndDt datetime
	)	
	Insert into @ContractedTickets select TicketId, ApproxContractValue, SectorChoice, Division, Studio, PreconStartDt, ConstructionEndDt from dbo.GetContractedProjects(@TenantID,'0','','','','','');
	
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
			select distinct Division from @PipelinedTickets where Len(Division) > 0
			Union
			select distinct Division from @ContractedTickets where Len(Division) > 0
		End
	Else If  @Category = 'studio'
		Begin
			Insert into @tblCategory (Category)
			select distinct Studio from @PipelinedTickets where Len(Studio) > 0
			Union
			select distinct Studio from @ContractedTickets where Len(Studio) > 0
		End

	--select * from @tblCategory;

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
					/*
					select @Jan = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Feb = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Mar = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Apr = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @May = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Jun = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Jul = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Aug = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Sep = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Oct = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Nov = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Dec = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector
								*/

								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'), (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
											join aspnetusers u on u.id = RA.ResourceUser
											where RA.TenantID = @TenantId
											and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Sector = @CategoryName
											and u.Enabled = 1
											and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
											group by RA.MonthStartDate, RA.Sector;

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


								Insert into #tmpKPItable values (@CategoryName, 'Resources billed on Current Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								
								Insert into #tmpKPItable values (@CategoryName, 'Resources used on Precon/Potential Projects',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')
								------------------------------

								/*
								select @Jan = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Feb = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Mar = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Apr = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @May = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Jun = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Jul = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Aug = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Sep = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Oct = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Nov = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Dec = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector
								*/

								delete from #tmpKPI;
								insert into #tmpKPI  
								select FORMAT(RA.MonthStartDate, 'MMM'),(sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate between @Year + '-01-01' and @Year + '-12-31'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
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

								Insert into #tmpKPItable values (@CategoryName, 'Total Resources Used',@Jan,@Feb,@Mar,@Apr,@May,@Jun,@Jul,@Aug,@Sep,@Oct,@Nov,@Dec,'number')

								update #tmpKPItable set Jan = (@Jan - Jan), Feb = (@Feb - Feb), Mar = (@Mar - Mar),   Apr = (@Apr - Apr), May = (@May - May), Jun = (@Jun - Jun), Jul = (@Jul - Jul), Aug = (@Aug - Aug), Sep = (@Sep - Sep), Oct = (@Oct - Oct), Nov = (@Nov - Nov), Dec = (@Dec - Dec)
								where KPI = 'Resources used on Precon/Potential Projects' and Category = @CategoryName;
								----------------------------								

					End
				Else If @Type = 'pct'
					Begin
								/*
								select @Jan = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector
								
								select @Feb = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Mar = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Apr = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @May = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Jun = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Jul = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Aug = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Sep = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Oct = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Nov = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector

								select @Dec = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Sector
								*/

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
								------------------------------
								/*
								select @Jan = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Feb = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Mar = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Apr = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @May = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Jun = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Jul = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Aug = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Sep = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Oct = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Nov = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector

								select @Dec = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Sector = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Sector
								*/

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
				--------------------------------------------------------
				/*
				Select @Jan = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-01-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Feb = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-02-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Mar = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-03-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Apr = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-04-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @May = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-05-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Jun = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-06-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Jul = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-07-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Aug = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-08-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Sep = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-09-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Oct = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-10-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Nov = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-11-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Dec = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-12-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.Sectorchoice = @CategoryName or OPM.Sectorchoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)
				*/
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
					/*
					select @Jan = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Feb = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Mar = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Apr = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @May = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Jun = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Jul = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Aug = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Sep = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Oct = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Nov = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Dec = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division
								*/

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
								------------------------------
								/*
								select @Jan = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Feb = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Mar = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Apr = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @May = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Jun = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Jul = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Aug = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Sep = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Oct = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Nov = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Dec = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division
								*/

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
								/*
								select @Jan = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division
								
								select @Feb = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Mar = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Apr = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @May = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Jun = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Jul = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Aug = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Sep = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Oct = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Nov = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division

								select @Dec = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Division
								*/

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
								------------------------------
								/*
								select @Jan = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Feb = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Mar = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Apr = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @May = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Jun = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Jul = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Aug = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Sep = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Oct = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Nov = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division

								select @Dec = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Division = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Division
								*/

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
					
				--------------------------------------------------------
				/*
				Select @Jan = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-01-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Feb = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-02-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Mar = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-03-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Apr = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-04-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @May = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-05-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Jun = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-06-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Jul = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-07-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Aug = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-08-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Sep = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-09-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Oct = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-10-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Nov = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-11-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Dec = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-12-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)
				*/

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
						and (cpr.CRMBusinessUnitChoice = @CategoryName or OPM.CRMBusinessUnitChoice = @CategoryName or CNS.CRMBusinessUnitChoice = @CategoryName)
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
					/*
					select @Jan = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Feb = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Mar = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Apr = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @May = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Jun = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Jul = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Aug = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Sep = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Oct = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Nov = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Dec = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio
								*/

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
								------------------------------
								/*
								select @Jan = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Feb = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Mar = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Apr = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @May = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Jun = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Jul = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Aug = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Sep = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Oct = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Nov = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Dec = (sum(RA.pctallocation)/100) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio
								*/

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
								/*
								select @Jan = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio
								
								select @Feb = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Mar = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Apr = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @May = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Jun = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Jul = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Aug = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Sep = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Oct = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Nov = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio

								select @Dec = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from @ContractedTickets)
								group by RA.Studio
								*/

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
								------------------------------
								/*
								select @Jan = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-01-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Feb = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-02-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Mar = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-03-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Apr = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-04-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @May = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-05-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Jun = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-06-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Jul = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-07-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Aug = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-08-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Sep = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-09-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Oct = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-10-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Nov = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-11-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio

								select @Dec = (sum(RA.pctallocation)/count(distinct RA.ResourceUser)) from #tmpResourceAllocationMonthly RA 
								join aspnetusers u on u.id = RA.ResourceUser
								where RA.TenantID = @TenantId
								and RA.MonthStartDate = @Year + '-12-01'  and RA.Studio = @CategoryName
								and u.Enabled = 1
								and  RA.ResourceWorkItem in (select Ticketid from #tmpOpenTickets)
								group by RA.Studio
								*/

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
					
				--------------------------------------------------------
				/*
				Select @Jan = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-01-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Feb = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-02-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Mar = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-03-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Apr = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-04-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @May = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-05-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Jun = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-06-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Jul = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-07-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Aug = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-08-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Sep = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-09-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Oct = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-10-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Nov = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-11-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)

				Select @Dec = Sum(mk.TotalBillingLaborRate)	
				from (
				Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
				((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate
				from ResourceUsageSummaryMonthWise RA
				left join AspNetUsers NU on RA.ResourceUser = NU.Id
				left join JobTitle JT on JT.ID = NU.JobTitleLookup
				where RA.TenantID= @TenantID 
				and RA.MonthStartDate >= @year + '-12-01'
				and RA.WorkItemType in ('OPM','CPR','CNS')) mk
				left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID= @TenantID
						left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID= @TenantID 
						left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID= @TenantID 
						where 1=1 and mk.TotalBillingLaborRate is not null
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
						and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
				group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
				Order by Month(mk.MonthStartDate)
				*/

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
						and (cpr.StudioChoice = @CategoryName or OPM.StudioChoice = @CategoryName or CNS.StudioChoice = @CategoryName)
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


	select * from #tmpKPItable;

	Drop table #tmpOpenTickets;
	Drop table #tmpResourceAllocationMonthly;
	Drop table #tmpKPItable;
	Drop table #tmpKPI;
END


Go

--------------------

ALTER PROCEDURE [dbo].[GetProjectCosts] 
	@TicketId nvarchar(100), 
	@TenantId nvarchar(128)
AS
BEGIN
	
	select usr.[Name], 
	CONVERT(NVARCHAR, pe.AllocationStartDate, 106) as 'StartDate', 
	CONVERT(NVARCHAR, pe.AllocationEndDate, 106) as 'EndDate', /* pe.AssignedToUser, */ 
	pe.PctAllocation, 
	dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate,@TenantId) as 'days',
	jt.EmpCostRate, 
	dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED') as 'AllocatedAcquisitionCost',
	dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ACTUAL') as 'ActualACquisitionCost',
	(pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate,@TenantId) * 8) * jt.EmpCostRate) as 'AllocatedResourceCost',
	dbo.fnGetActualProjectCostByResource(@TenantId,pe.AssignedToUser,@TicketId, pe.AllocationStartDate, pe.AllocationEndDate) as 'ActualResourceCost',
	
	case when (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED')) > 0 and (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ACTUAL')) > 0 then 
	cast(round(((dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED')) - (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ACTUAL'))) / (dbo.fnGetAcquisitionCostByResource(@TenantId,pe.AssignedToUser,@TicketId,left(@TicketId, charindex('-', @TicketId) - 1), 'ALLOCATED')), 1, 1) as numeric(38,1))
	else 0 end as 'Acquisition Cost Variance',
	
	case when (pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate,@TenantId) * 8) * jt.EmpCostRate) > 0 and (dbo.fnGetActualProjectCostByResource(@TenantId,pe.AssignedToUser,@TicketId, pe.AllocationStartDate, pe.AllocationEndDate)) > 0 then 
	cast(round(((pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate,@TenantId) * 8) * jt.EmpCostRate) - (dbo.fnGetActualProjectCostByResource(@TenantId,pe.AssignedToUser,@TicketId, pe.AllocationStartDate, pe.AllocationEndDate))) / (pe.PctAllocation * (dbo.fnGetWorkingDays(AllocationStartDate,AllocationEndDate,@TenantId) * 8) * jt.EmpCostRate), 1, 1) as numeric(38,1))
	else 0 end as 'Resource Cost Variance' 
	into #temp
	from ProjectEstimatedAllocation pe join AspNetUsers usr on pe.AssignedToUser = usr.Id
	join JobTitleCostRateByDept jt on usr.JobTitleLookup = jt.JobTitleLookup and usr.DepartmentLookup = jt.DeptLookup
	where pe.TenantID = @TenantId and pe.TicketID = @TicketId;

	--select * from #temp

	select SUM(AllocatedAcquisitionCost) as AllocatedAcquisitionCost, SUM(ActualACquisitionCost) as ActualACquisitionCost, 
		   SUM(AllocatedResourceCost) as AllocatedResourceCost, SUM(ActualResourceCost) as ActualResourceCost 
		   into #tempCalculation
		   from #temp

	 select @TicketId as TicketId, @TenantId as TenantId, AllocatedAcquisitionCost, ActualACquisitionCost, AllocatedResourceCost, ActualResourceCost, 
			case when ((AllocatedAcquisitionCost > 0) and (ActualACquisitionCost > 0)) then
			cast(round(((AllocatedAcquisitionCost - ActualACquisitionCost) / AllocatedAcquisitionCost),1,1) as numeric(38,1)) else 0 end as AcquisitionCostVariance,
			case when ((AllocatedResourceCost > 0) and (ActualResourceCost > 0)) then
			cast(round(((AllocatedResourceCost - ActualResourceCost) / AllocatedResourceCost), 1, 1) as numeric(38,1)) else 0 end as ResourceCostVariance
	from #tempCalculation

END

Go
--------------------

-- select dbo.fnGetUtilizationRate('35525396-e5fe-4692-9239-4df9305b915b', 'CPR-19-000104','2021-07-01','2021-07-29')
-- select dbo.fnGetUtilizationRate('35525396-e5fe-4692-9239-4df9305b915b','CPR-19-000104','2021-01-01',NULL)
ALTER FUNCTION [dbo].[fnGetUtilizationRate] 
(
	@TenantID nvarchar(256),
	@TicketId nvarchar(100),
	@PreconStartDate datetime,
	@ConstructionEndDate datetime
)
RETURNS decimal(18,2)
AS
BEGIN
	DECLARE @Result decimal(18,2);

	DECLARE @i int;
	DECLARE @count int;
	DECLARE @workingDaysinWeek int;
	DECLARE @TotalworkingDays int;
	DECLARE @AvgPctAllocation decimal(18,2);
	DECLARE @PctAllocation decimal(18,2);

	DECLARE @StartDate datetime;
	DECLARE @EndDate datetime;
	DECLARE @firstWkStartDt datetime;

	Set @PctAllocation = 0;
	Set @TotalworkingDays = 0;
	Set @Result = 0;

	set @StartDate = @PreconStartDate;
	set @EndDate = @ConstructionEndDate;

	If @PreconStartDate IS NULL OR @ConstructionEndDate IS NULL	
	RETURN @Result;
	
	DECLARE @Dates TABLE
	(
		Id int identity,
		WeekStartDate datetime
	)
	
	set @StartDate =  DATEADD(day, DATEDIFF(day, 0, @PreconStartDate) /7*7, 0);
	--set @StartDate = DATEADD(DAY, 1 - 4, @PreconStartDate); --start from first day of week of @PreconStartDate
	WHILE (@StartDate <= @EndDate)
	BEGIN
		--print @StartDate;
		Insert into @Dates(WeekStartDate) values (@StartDate)
		set @StartDate = DATEADD(day, 7, @StartDate);
	END;
			
	--select * from @Dates
	set @firstWkStartDt =  DATEADD(day, DATEDIFF(day, 0, @PreconStartDate) /7*7, 0);
	set @workingDaysinWeek = dbo.fnGetWorkingDays(@PreconStartDate, DATEADD(DAY, 4, @firstWkStartDt), @TenantID);

	select @AvgPctAllocation = ISNULL(avg(PctAllocation), 0) from ResourceUsageSummaryWeekWise where  TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
	and WorkItem = @TicketId
	and WeekStartDate = @firstWkStartDt
	set @PctAllocation = (@AvgPctAllocation * @workingDaysinWeek);
	set @TotalworkingDays = @workingDaysinWeek;

	--select @PctAllocation as PctAllocation, @workingDaysinWeek as workingDays;
	--From Second Week calculation
	set @i = 2;
	select @count = count(*) from @Dates;
	While @i < @count
	Begin
		select @StartDate = WeekStartDate from @Dates where Id = @i;

		select @AvgPctAllocation = ISNULL(avg(PctAllocation), 0) from ResourceUsageSummaryWeekWise where  TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
		and WorkItem = @TicketId
		and WeekStartDate = @StartDate
		set @workingDaysinWeek = dbo.fnGetWorkingDays(@StartDate, DATEADD(DAY, 4, @StartDate), @TenantID);

		set @PctAllocation = @PctAllocation + (@AvgPctAllocation * @workingDaysinWeek);

		If @AvgPctAllocation > 0
		set @TotalworkingDays =  @TotalworkingDays + @workingDaysinWeek;
		--select (@AvgPctAllocation * @workingDaysinWeek) as PctAllocation, @workingDaysinWeek as workingDays;
		set @i = @i + 1;
	End;

	-- Last Week calculations
	select @StartDate = WeekStartDate from @Dates where Id = @count;
	select @AvgPctAllocation = ISNULL(avg(PctAllocation), 0) from ResourceUsageSummaryWeekWise where  TenantID = '35525396-e5fe-4692-9239-4df9305b915b'
	and WorkItem = @TicketId
	and WeekStartDate = @StartDate;
	set @workingDaysinWeek = dbo.fnGetWorkingDays(@StartDate, @ConstructionEndDate, @TenantID);
	set @PctAllocation = @PctAllocation + (@AvgPctAllocation * @workingDaysinWeek);
	set @TotalworkingDays = @TotalworkingDays + @workingDaysinWeek;


	--RETURN @Result
	--select @PctAllocation as 'FinalPctAlloc', @TotalworkingDays as 'totalDays'
	--select (@AvgPctAllocation) as PctAllocation, @workingDaysinWeek as TotalworkingDays;

	select @Result = (@PctAllocation / @TotalworkingDays);
	RETURN @Result;
END


Go

-------------------

-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','role','Architect')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','jobtitle','Assistant Project Manager')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','sector','Aviation')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','division','1-21 Silicon Valley')
-- select dbo.fnGetPipelineUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-12-31','studio','SF Studio 2')
ALTER FUNCTION [dbo].[fnGetPipelineUtilization] 
(
	@TenantID nvarchar(256),
	@StartDate datetime,
	@EndDate datetime,
	@Category nvarchar(50),
	@value nvarchar(1000)
)
RETURNS int
AS
BEGIN
	DECLARE @Result decimal;
	DECLARE @MonthlyResult decimal;
	--Declare @Query nvarchar(500);
	Declare @i int;
	Declare @NoofDays int;
	Declare @NoofResources int;

	Declare @tmpDate date;
	Declare @StartMonth int;
	Declare @EndMonth int;

	Set @StartMonth = MONTH(@StartDate);
	Set @EndMonth = MONTH(@EndDate);

	Set @Result = 0;
	Set @NoofDays = 0;

	DECLARE @Tickets TABLE
	(
		TicketId varchar(30)
	)

	IF @Category = 'sector'
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','',@value,'','')
			--select TicketId,SectorChoice,Division from dbo.GetPipelineProjects(@TenantID,'0','','',@value,'','')
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','','',@value,'')
			--select TicketId,SectorChoice,Division from dbo.GetPipelineProjects(@TenantID,'0','','','',@value,'')
		End
	ELSE IF @Category = 'studio'
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','','','',@value)
			--select TicketId,SectorChoice,Division from dbo.GetPipelineProjects(@TenantID,'0','','','','',@value,'')
		End
	ELSE
		Begin
			Insert into @Tickets select TicketId from dbo.GetPipelineProjects(@TenantID,'0','','','','','')
		End
			
	--select * from @Tickets

	DECLARE @tmpMonthDays TABLE (
		Month varchar(20),
		Days int
	);

	--INSERT INTO #tmpMonthDays Values (0,0,0,0,0,0,0,0,0,0,0,0);	
	Set @i = @StartMonth;
	While @i <= @EndMonth
	Begin
		set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@i as varchar(4)) + '-' + '01';

		INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @i,-1), 'MMM'), dbo.fnGetWorkingDays(@tmpDate, EOMONTH(@tmpDate),@TenantID))
		Set @i = @i + 1;
	End

	--select * from @tmpMonthDays;

	DECLARE @tmpRAMonthly TABLE(	
		Category nvarchar(1000),
		MonthStartDate datetime,
		PctAllocation float
		)

	IF @Category = 'sector' OR @Category = 'studio'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'jobtitle'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select usr.JobProfile, r.MonthStartDate, r.PctAllocation
			 from ResourceAllocationMonthly as r
			 join AspNetUsers as usr on r.ResourceUser = usr.Id
			 where r.TenantID = @TenantID and usr.TenantID = @TenantID
			 and r.MonthStartDate >= @StartDate and r.MonthStartDate <= @EndDate
			and usr.JobProfile = @value
			and r.ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1 and JobProfile = @value;
		End
	ELSE IF @Category = 'role'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select ResourceSubWorkItem, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and ResourceSubWorkItem = @value
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers as usr join Roles as r on usr.GlobalRoleID = r.Id where usr.TenantID = @TenantID
			and r.Name = @value and r.TenantID = @TenantID and usr.Enabled = 1;
		End


	--select * from @tmpRAMonthly order by MonthStartDate

	Set @Result = 0;
	Set @i = @StartMonth;
	While @i <= @EndMonth
	Begin
		Set @MonthlyResult = 0;
		select @NoofDays = Days from @tmpMonthDays where Month = FORMAT(DATEADD(mm, @i,-1), 'MMM');
		select @MonthlyResult = SUM(PctAllocation) * @NoofDays  from @tmpRAMonthly  where MONTH(MonthStartDate) = @i  group by MonthStartDate

		--print @NoofDays
		--print @MonthlyResult
		--print '-----------'

		Set @Result = @Result + @MonthlyResult;
		Set @i = @i + 1;
	End
	
	select @NoofDays = Sum(Days) from @tmpMonthDays;
	--select @NoofDays as 'No of days';
	Set @Result = (@Result / @NoofDays)/@NoofResources;

	RETURN @Result
END


Go

--------------------

-- select dbo.fnGetEffectiveUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-11','role','Architect')
-- select dbo.fnGetEffectiveUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-11','jobtitle','Assistant Project Manager')
-- select dbo.fnGetEffectiveUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-11','sector','Corporate Interiors')
-- select dbo.fnGetEffectiveUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-11','division','1-21 Silicon Valley')
-- select dbo.fnGetEffectiveUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-01-01','2021-06-11','studio','SF Studio 2')
ALTER FUNCTION [dbo].[fnGetEffectiveUtilization] 
(
	@TenantID nvarchar(256),
	@StartDate datetime,
	@EndDate datetime,
	@Category nvarchar(50),
	@value nvarchar(1000)
)
RETURNS int
AS
BEGIN
	DECLARE @Result decimal;
	DECLARE @MonthlyResult decimal;
	--Declare @Query nvarchar(500);
	Declare @i int;
	Declare @NoofDays int;
	Declare @NoofResources int;

	Declare @tmpDate date;
	Declare @StartMonth int;
	Declare @EndMonth int;
	
	Set @StartMonth = MONTH(@StartDate);
	Set @EndMonth = MONTH(@EndDate);
	
	Set @Result = 0;
	Set @NoofDays = 0;

	DECLARE @Tickets TABLE
	(
		TicketId varchar(30)
	)

	IF @Category = 'sector'
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'','','',@value,'','')
			--select TicketId,SectorChoice,Division from dbo.GetContractedProjects(@TenantID,'','','',@value,'','')
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'','','','',@value,'')
			--select TicketId,SectorChoice,Division from dbo.GetContractedProjects(@TenantID,'','','','',@value,'')
		End
	ELSE IF @Category = 'studio'
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'','','','','',@value)
			--select TicketId,SectorChoice,Division from dbo.GetContractedProjects(@TenantID,'','','','','',@value)
		End
	ELSE
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'','','','','','')
		End
			
	--select * from @Tickets

	DECLARE @tmpMonthDays TABLE(
		Month varchar(20),
		Days int
	);


	Set @i = @StartMonth;
	-- If End date is in January
	If @i = @EndMonth
		Begin
			set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@i as varchar(4)) + '-' + '01';
			INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @i,-1), 'MMM'), dbo.fnGetWorkingDays(@tmpDate, @EndDate, @TenantID))
		End
	Else
		Begin
			While @i < @EndMonth
			Begin
				set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@i as varchar(4)) + '-' + '01';

				INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @i,-1), 'MMM'), dbo.fnGetWorkingDays(@tmpDate, EOMONTH(@tmpDate), @TenantID))
				Set @i = @i + 1;
			End

			set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@EndMonth as varchar(4)) + '-' + '01';
			INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @EndMonth,-1), 'MMM'), dbo.fnGetWorkingDays(@tmpDate, @EndDate, @TenantID))
		End



	--select * from #tmpMonthDays;

	DECLARE @tmpRAMonthly TABLE(	
		Category nvarchar(1000),
		MonthStartDate datetime,
		PctAllocation float
		)

	IF @Category = 'sector' OR @Category = 'studio'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1
			--Set @NoofResources = 1;
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
			--Set @NoofResources = 1;
		End
	ELSE IF @Category = 'jobtitle'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select usr.JobProfile, r.MonthStartDate, r.PctAllocation
			 from ResourceAllocationMonthly as r
			 join AspNetUsers as usr on r.ResourceUser = usr.Id
			 where r.TenantID = @TenantID and usr.TenantID = @TenantID
			 and r.MonthStartDate >= @StartDate and r.MonthStartDate <= @EndDate
			and usr.JobProfile = @value
			and r.ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1 and JobProfile = @value;
		End
	ELSE IF @Category = 'role'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select ResourceSubWorkItem, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and ResourceSubWorkItem = @value
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers as usr join Roles as r on usr.GlobalRoleID = r.Id where usr.TenantID = @TenantID
			and r.Name = @value and r.TenantID = @TenantID and usr.Enabled = 1;
		End


	--select * from #tmpRAMonthly order by MonthStartDate

	Set @Result = 0;
	Set @i = @StartMonth;
	While @i <= @EndMonth
	Begin
		Set @MonthlyResult = 0;
		select @NoofDays = Days from @tmpMonthDays where Month = FORMAT(DATEADD(mm, @i,-1), 'MMM');
		select @MonthlyResult = SUM(PctAllocation) * @NoofDays  from @tmpRAMonthly  where MONTH(MonthStartDate) = @i  group by MonthStartDate

		--print @NoofDays
		--print @MonthlyResult
		--print '-----------'

		Set @Result = @Result + @MonthlyResult;
		Set @i = @i + 1;
	End
	
	select @NoofDays = Sum(Days) from @tmpMonthDays;
	--select @NoofDays as 'No of days';
	Set @Result = (@Result / @NoofDays)/@NoofResources;

	RETURN @Result
END

Go
------------

-- select dbo.fnGetCommittedUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-06-11','2021-12-31','role','Architect')
-- select dbo.fnGetCommittedUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-06-11','2021-12-31','jobtitle','Assistant Project Manager')
-- select dbo.fnGetCommittedUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-06-11','2021-12-31','sector','Corporate Interiors')
-- select dbo.fnGetCommittedUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-06-11','2021-12-31','division','1-21 Silicon Valley')
-- select dbo.fnGetCommittedUtilization('35525396-e5fe-4692-9239-4df9305b915b','2021-06-11','2021-12-31','studio','SF Studio 1')
ALTER FUNCTION [dbo].[fnGetCommittedUtilization] 
(
	@TenantID nvarchar(256),
	@StartDate datetime,
	@EndDate datetime,
	@Category nvarchar(50),
	@value nvarchar(1000)
)
RETURNS int
AS
BEGIN
	DECLARE @Result decimal;
	DECLARE @MonthlyResult decimal;
	--Declare @Query nvarchar(500);
	Declare @i int;
	Declare @NoofDays int;
	Declare @NoofResources int;

	Declare @tmpDate date;
	Declare @StartMonth int;
	Declare @EndMonth int;
	
	Set @StartMonth = MONTH(@StartDate);
	Set @EndMonth = MONTH(@EndDate);
	
	Set @Result = 0;
	Set @NoofDays = 0;

	DECLARE @Tickets TABLE
	(
		TicketId varchar(30)
	)

	IF @Category = 'sector'
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'0','','',@value,'','')
			--select TicketId,SectorChoice,Division from dbo.GetContractedProjects(@TenantID,'0','','',@value,'')
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'0','','','',@value,'')
			--select TicketId,SectorChoice,Division from dbo.GetContractedProjects(@TenantID,'0','','','',@value)
		End
	ELSE IF @Category = 'studio'
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'0','','','','',@value)
			--select TicketId,SectorChoice,Division from dbo.GetContractedProjects(@TenantID,'0','','','',@value)
		End
	ELSE
		Begin
			Insert into @Tickets select TicketId from dbo.GetContractedProjects(@TenantID,'0','','','','','')
		End
			
	--select * from @Tickets

	DECLARE @tmpMonthDays TABLE(
		Month varchar(20),
		Days int
	);


	Set @i = @StartMonth;
	-- If End date is in January
	If @i = @EndMonth
		Begin
			INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @i,-1), 'MMM'), dbo.fnGetWorkingDays(@StartDate, EOMONTH(@StartDate), @TenantID))
		End
	Else
		Begin
			--set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@EndMonth as varchar(4)) + '-' + '01';
			INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @StartMonth,-1), 'MMM'), dbo.fnGetWorkingDays(@StartDate, EOMONTH(@StartDate), @TenantID))
			Set @i = @i + 1;
			While @i <= @EndMonth
			Begin
				set @tmpDate = CAST(YEAR(@StartDate) as varchar(4)) + '-' + CAST(@i as varchar(4)) + '-' + '01';

				INSERT INTO @tmpMonthDays (Month, Days) Values (FORMAT(DATEADD(mm, @i,-1), 'MMM'), dbo.fnGetWorkingDays(@tmpDate, EOMONTH(@tmpDate), @TenantID))
				Set @i = @i + 1;
			End

			
		End

	--select * from @tmpMonthDays;

	DECLARE @tmpRAMonthly TABLE(	
		Category nvarchar(1000),
		MonthStartDate datetime,
		PctAllocation float
		)

	IF @Category = 'sector' OR @Category = 'studio'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'division'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select @value, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1;
		End
	ELSE IF @Category = 'jobtitle'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select usr.JobProfile, r.MonthStartDate, r.PctAllocation
			 from ResourceAllocationMonthly as r
			 join AspNetUsers as usr on r.ResourceUser = usr.Id
			 where r.TenantID = @TenantID and usr.TenantID = @TenantID
			 and r.MonthStartDate >= @StartDate and r.MonthStartDate <= @EndDate
			and usr.JobProfile = @value
			and r.ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers where TenantID = @TenantID and Enabled = 1 and JobProfile = @value;
		End
	ELSE IF @Category = 'role'
		Begin
			Insert into @tmpRAMonthly (Category, MonthStartDate, PctAllocation) select ResourceSubWorkItem, MonthStartDate, PctAllocation from ResourceAllocationMonthly where TenantID = @TenantID
			and ResourceSubWorkItem = @value
			and MonthStartDate >= @StartDate and MonthStartDate <= @EndDate
			and ResourceWorkItem in (select TicketId from @Tickets)

			Select @NoofResources = COUNT(*) from AspNetUsers as usr join Roles as r on usr.GlobalRoleID = r.Id where usr.TenantID = @TenantID
			and r.Name = @value and r.TenantID = @TenantID and usr.Enabled = 1;
		End


	--select * from @tmpRAMonthly order by MonthStartDate

	Set @Result = 0;
	Set @i = @StartMonth;
	While @i <= @EndMonth
	Begin
		Set @MonthlyResult = 0;
		select @NoofDays = Days from @tmpMonthDays where Month = FORMAT(DATEADD(mm, @i,-1), 'MMM');
		select @MonthlyResult = SUM(PctAllocation) * @NoofDays  from @tmpRAMonthly  where MONTH(MonthStartDate) = @i  group by MonthStartDate

		--print @NoofDays
		--print @MonthlyResult
		--print '-----------'

		Set @Result = @Result + @MonthlyResult;
		Set @i = @i + 1;
	End
	
	select @NoofDays = Sum(Days) from @tmpMonthDays;
	--select @NoofDays as 'No of days';
	Set @Result = (@Result / @NoofDays)/@NoofResources;

	RETURN @Result
END

Go
----------------------
ALTER procedure [dbo].[usp_GetMonthWiseUtilizationChart]
--exec [usp_GetMonthWiseUtilization] 'bcd2d0c9-9947-4a0b-9fbf-73ea61035069', 'Aviation', '', 'Los Angeles', 'Pipeline'
@Startdate datetime ='',
@Endate datetime ='',
@tenantID nvarchar(max) = '',
@sector nvarchar(max) = '',
@studio nvarchar(max) = '',
@division nvarchar(max) = '',
@filter nvarchar(max) = '',
@billable nvarchar(100) = 'Billable'
as
begin

	Declare @Days int=0,@BillableResourceCount int =0;
	Set @Days=(Select dbo.fnGetWorkingDays(@Startdate,@Endate,@tenantID))
	Set @BillableResourceCount=(Select Count(Id) from dbo.[fnGetBillableResources](@TenantId)
	Where JobType=@billable  and GlobalRoleID is not null and JobProfile is not null)

	Select LEFT(DATENAME(m,a.MonthStartDate), 3) Months,
	Ceiling((Sum(a.PctAllocation)/100)) FTE,
	[dbo].[fnGetWorkingDaysTillDate](Month(a.MonthStartDate))WorkingDays,
	Round(((Sum(a.PctAllocation)/100) *[dbo].[fnGetWorkingDaysTillDate](Month(a.MonthStartDate))),2) FinalCount
	from ResourceAllocationMonthly a 
	Where a.TenantID=@TenantId
	and a.monthstartdate >= @Startdate and a.monthstartdate  <= @Endate
	and a.ResourceUser in (Select Id from dbo.[fnGetBillableResources](@TenantId)
	Where JobType=@billable  and GlobalRoleID is not null and JobProfile is not null 
	)
	and a.ResourceWorkItem in (
		Select TicketId from [dbo].[fnGetDivisonStudioTicketWise](@TenantId,@filter,@studio,@division,@sector)
	)
	group by  DATENAME(m,a.MonthStartDate), Month(a.MonthStartDate) order by Month(a.MonthStartDate)
	
End;


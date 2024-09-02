Create Procedure [dbo].[GetBillings]
@TenantID varchar(250),
@StartDate nvarchar(250),
@EndDate nvarchar(250),
@modulenames varchar(max)='CPR,CNS,OPM',
@Mode varchar(max)='C',
@Billable bit = 'True',
@Overhead bit = 'False'
AS
Begin
	Declare @SQL nvarchar(max)=''

	SET @SQL= 'Select LEFT(DATENAME(m, mk.MonthStartDate), 3) as StartMonth,
	          Format(Sum(mk.TotalBillingLaborRate),''C0'') as TotalBillingLaborRate,
	Format(Sum(TotalEmployeeCostRate),''C0'') as TotalEmployeeCostRate,
	Format(Sum(mk.GrossMargin),''C0'') as GrossMargin, count(mk.workitem) TotalProjects,
	count(distinct mk.Name) BilledResources,
	round((Sum(mk.PctAllocation)/100),2) BilledWorkMonth,
	Abs(round((Sum(mk.PctAllocation)/100),2) - 100) UnBilledWorkMonth,
	round((Sum(mk.PctAllocation)/count(distinct mk.Name)),2) Utilization
	from (
    Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem, RA.MonthStartDate, RA.PctAllocation,
	((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) as TotalBillingLaborRate,
    ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate) as TotalEmployeeCostRate,
    (((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)) as GrossMargin
    from ResourceUsageSummaryMonthWise RA
    left join AspNetUsers NU on RA.ResourceUser = NU.Id
    join JobTitle JT on JT.ID = NU.JobTitleLookup
    where RA.TenantID='''+@TenantID+'''
	and NU.Enabled = 1
	and (JT.JobType = case when '''+ CONVERT(varchar, @Billable)+ '''=''1'' then ''Billable'' else '''' end
	or JT.JobType = case when '''+ CONVERT(varchar, @Overhead)+ '''=''1'' then ''Overhead'' else '''' end)
	and JT.Deleted = 0
	and RA.MonthStartDate >= '''+@StartDate+''' and RA.MonthStartDate <= '''+@EndDate+'''
    and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('''+ @modulenames +''', '',''))) mk'

	--	and (JT.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
	--or JT.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )

	IF(@Mode ='P')
    BEGIN
		-- FOR PIPLINE
		SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+'''
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+'''
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+'''
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			and (CPR.StageStep < 8 or OPM.StageStep <> 6 or CNS.StageStep < 8)
			group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
			Order by Month(mk.MonthStartDate)'
		PRINT(@SQL)
		EXEC(@SQL)
    END
    ELSE IF(@Mode='C')
    BEGIN
		--FOR CONSTRUCTION
		SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+''' 
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+''' 
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+''' 
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			and (CPR.StageStep = 8 or OPM.StageStep > 6 or CNS.StageStep = 8)
			group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
			Order by Month(mk.MonthStartDate)'
		PRINT(@SQL)
		EXEC(@SQL)
    END
	ELSE IF(@Mode='PC')
	BEGIN
	   --For Both Const and pipline
	   SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+''' 
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+''' 
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+''' 
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)
			group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
			Order by Month(mk.MonthStartDate)'
		PRINT(@SQL)
		EXEC(@SQL)
	END
	ELSE IF(@Mode='R')
	BEGIN
	   --For Both Const and pipline
	   SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+''' 
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+''' 
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+''' 
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			and (CPR.StageStep = 9 or OPM.StageStep = 6 or CNS.StageStep = 9)
			group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
			Order by Month(mk.MonthStartDate)'
		PRINT(@SQL)
		EXEC(@SQL)
	END
	ELSE IF(@Mode='CR')
	BEGIN
	   --For Both Const and pipline
	   SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+''' 
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+''' 
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+''' 
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			and (CPR.StageStep >= 8 or OPM.StageStep = 6 or CNS.StageStep >= 8)
			group by LEFT(DATENAME(m, mk.MonthStartDate), 3), Month(mk.MonthStartDate)
			Order by Month(mk.MonthStartDate)'
		PRINT(@SQL)
		EXEC(@SQL)
	END
	ELSE IF(@Mode='PCR')
	BEGIN
	   select LEFT(DATENAME(m, RA.MonthStartDate), 3) as StartMonth, 
		FORMAT(Sum((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate),'C0') as TotalBillingLaborRate,
		FORMAT(Sum((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate),'C0') as TotalEmployeeCostRate,
		FORMAT(Sum(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)),'C0') as GrossMargin,
		count(RA.workitem) TotalProjects,
		count(distinct NU.Name) BilledResources,
		round((Sum(RA.PctAllocation)/100),2) BilledWorkMonth,
		Abs(round((Sum(RA.PctAllocation)/100),2) - 100) UnBilledWorkMonth,
		round((Sum(RA.PctAllocation)/count(distinct NU.Name)),2) Utilization
		from ResourceUsageSummaryMonthWise RA
		left join AspNetUsers NU on RA.ResourceUser = NU.Id
		join JobTitle JT on JT.ID = NU.JobTitleLookup
		and (JT.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or JT.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
		where RA.TenantID=@TenantID
		and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING(@modulenames, ',')) 
		and RA.MonthStartDate >= @StartDate and RA.MonthStartDate <= @EndDate
		group by LEFT(DATENAME(m, RA.MonthStartDate), 3), Month(RA.MonthStartDate)
		Order by Month(RA.MonthStartDate)
	END
    ELSE
    BEGIN
		select LEFT(DATENAME(m, RA.MonthStartDate), 3) as StartMonth,
		FORMAT(Sum((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate),'C0') as TotalBillingLaborRate,
		FORMAT(Sum((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate),'C0') as TotalEmployeeCostRate,
		FORMAT(Sum(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)),'C0') as GrossMargin,
		count(RA.workitem) TotalProjects,
		count(distinct NU.Name) BilledResources,
		round((Sum(RA.PctAllocation)/100),2) BilledWorkMonth,
		Abs(round((Sum(RA.PctAllocation)/100),2) - 100) UnBilledWorkMonth,
		round((Sum(RA.PctAllocation)/count(distinct NU.Name)),2) Utilization
		from ResourceUsageSummaryMonthWise RA
		left join AspNetUsers NU on RA.ResourceUser = NU.Id
		 join JobTitle JT on JT.ID = NU.JobTitleLookup
		where RA.TenantID=@TenantID
		and (JT.JobType = case when CONVERT(varchar, @Billable)='1' then 'Billable' else '' end
		or JT.JobType = case when CONVERT(varchar, @Overhead)='1' then 'Overhead' else '' end )
		and JT.Deleted = 0
		and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING(@modulenames, ',')) 
		and RA.MonthStartDate >= @StartDate and RA.MonthStartDate <= @EndDate
		group by LEFT(DATENAME(m, RA.MonthStartDate), 3), Month(RA.MonthStartDate)
		Order by Month(RA.MonthStartDate)
    END

End;



CREATE Procedure [dbo].[usp_GetBillingDetails]
--[usp_GetBillingDetails] 'BCD2D0C9-9947-4A0B-9FBF-73EA61035069','2021-01-01','2021-01-31','CPR,OPM,CNS','PCR'
@TenantID varchar(250),
@StartDate nvarchar(250),
@EndDate nvarchar(250),
@modulenames nvarchar(max)='CPR,CNS,OPM',
@Mode nvarchar(max)='C'
AS
Begin
	Declare @SQL nvarchar(max)=''

	SET @SQL= 'Select mk.*, CPR.StageStep CPR, OPM.StageStep OPM, CNS.StageStep CNS from (
    Select NU.Name, NU.JobProfile, RA.AllocationHour, RA.WorkItem,
    FORMAT(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate),''C0'') as TotalBillingLaborRate,
    FORMAT(((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate),''C0'') as TotalEmployeeCostRate,
    FORMAT((((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)),''C0'') as GrossMargin
    from ResourceUsageSummaryMonthWise RA
    left join AspNetUsers NU on RA.ResourceUser = NU.Id
    left join JobTitle JT on JT.ID = NU.JobTitleLookup
    where RA.TenantID='''+@TenantID+'''
	and RA.MonthStartDate >= '''+@StartDate+''' and RA.MonthStartDate <= '''+@EndDate+'''
    and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING('''+ @modulenames +''', '',''))) mk'
	print(@sql)
	IF(@Mode ='P')
    BEGIN
		-- FOR PIPLINE)
		SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+'''
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+'''
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+'''
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null
			and (CPR.StageStep < 8 or OPM.StageStep <> 6 or CNS.StageStep < 8)'
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
			and (CPR.StageStep = 8 or OPM.StageStep > 6 or CNS.StageStep = 8)'
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
			and (CPR.StageStep <=8 or OPM.StageStep < 6 or CNS.StageStep <= 8)'
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
			and (CPR.StageStep = 9 or OPM.StageStep = 6 or CNS.StageStep = 9)'
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
			and (CPR.StageStep >= 8 or OPM.StageStep = 6 or CNS.StageStep >= 8)'
		PRINT(@SQL)
		EXEC(@SQL)
	END
	ELSE IF(@Mode = 'PCR')
	Begin
			SET @SQL= @SQL + ' left join CRMProject CPR on mk.WorkItem = CPR.TicketId and CPR.TenantID='''+@TenantID+''' 
			left join Opportunity OPM on mk.WorkItem = OPM.TicketId and OPM.TenantID='''+@TenantID+''' 
			left join CRMServices CNS on mk.WorkItem = CNS.TicketId and CNS.TenantID='''+@TenantID+''' 
			where 1=1 and mk.TotalBillingLaborRate is not null and mk.TotalEmployeeCostRate is not null'
		PRINT(@SQL)
		EXEC(@SQL)
	End
    ELSE
    BEGIN
		
		select LEFT(DATENAME(m, RA.MonthStartDate), 3) as StartMonth, 
		FORMAT(Sum((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate),'C0') as TotalBillingLaborRate,
		FORMAT(Sum((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate),'C0') as TotalEmployeeCostRate,
		FORMAT(Sum(((RA.AllocationHour * RA.PctAllocation/100) * JT.BillingLaborRate) - ((RA.AllocationHour * RA.PctAllocation/100) * JT.EmployeeCostRate)),'C0') as GrossMargin
		from ResourceUsageSummaryMonthWise RA
		left join AspNetUsers NU on RA.ResourceUser = NU.Id
		left join JobTitle JT on JT.ID = NU.JobTitleLookup
		where RA.TenantID=''
		and RA.WorkItemType in (SELECT item FROM DBO.SPLITSTRING(@modulenames, ',')) 
		and RA.MonthStartDate >= @StartDate and RA.MonthStartDate <= @EndDate
		group by LEFT(DATENAME(m, RA.MonthStartDate), 3), Month(RA.MonthStartDate)
		Order by Month(RA.MonthStartDate)
    END

End;





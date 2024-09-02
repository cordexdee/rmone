
Create Procedure Usp_GetRPTModulebudgetData
@ModuleNamelookup varchar(10),
@TenantID varchar(128)
as
begin
Select a.ActualCost,a.AllocationStartDate,a.BudgetAmount,c.Title BudgetCategoryLookup,c.BudgetType,
a.NonProjectActualTotal,a.NonProjectPlanedTotal,a.ProjectActualTotal,a.ProjectPlanedTotal,a.EstimatedCost,
a.ResourceCost, a.TicketId,a.Title from ModuleMonthlyBudget a
left join Config_BudgetCategories c on c.id=a.BudgetCategoryLookup and c.TenantID=@TenantID
Where a.TenantID = @TenantID and ModuleNameLookup=@ModuleNamelookup
End

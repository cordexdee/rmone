Create Procedure Usp_GetModuleBudgetActuals
@TenantID nvarchar(max)='',
@ModuleNameLookup nvarchar(10)='',
@BudgetCategoryLookup nvarchar(max)=''
as
begin
if(@BudgetCategoryLookup='')
begin
Select * from ModuleBudgetActuals a where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup
end
else begin
Select * from ModuleBudgetActuals a where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup
and ModuleBudgetLookup in (Select id from ModuleBudget where ModuleNameLookup=@ModuleNameLookup and BudgetCategoryLookup in ((SELECT CAST(Item AS INTEGER)
FROM dbo.SplitString(@BudgetCategoryLookup, ','))) and TenantID=@TenantID)
end
end
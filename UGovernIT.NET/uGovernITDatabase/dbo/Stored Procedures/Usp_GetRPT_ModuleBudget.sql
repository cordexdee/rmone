 
Create Procedure [dbo].[Usp_GetRPT_ModuleBudget]
@TenantID varchar(128)='',
@ModuleNameLookup varchar(50)='ITG',
@BudgetStartDate varchar(20)='',
@BudgetEndDate varchar(20)='',
@itemid int =0

as
begin

if(@itemid>0)
begin
Select Replace(convert(varchar(15),AllocationStartDate,106),' ','-') BudgetStartDate, Replace(convert(varchar(15),AllocationEndDate,106),' ','-')  BudgetEndDate ,(Select a.Title from ModuleBudget a where a.TenantID=@TenantID and a.ID=@itemid)BudgetItem,
ActualCost Actual  from ModuleBudgetActuals where ModuleBudgetLookup=@itemid and TenantID=@TenantID
end
else begin
Select c.BudgetCategoryName BudgetCategory,  c.BudgetSubCategory,a.Title BudgetItem,c.BudgetType, d. Title 'DepartmentLookup',a.ID,
(Select isnull(sum(actualcost),0)  from ModuleBudgetActuals where ModuleNameLookup=@ModuleNameLookup and TenantID=@TenantID 
and ModuleBudgetLookup  in (a.ID))'Actuals', 
a.BudgetAmount -(Select isnull(sum(actualcost),0)  from ModuleBudgetActuals where ModuleNameLookup=@ModuleNameLookup and TenantID=@TenantID 
and ModuleBudgetLookup  in (a.ID))'Variance', a.AllocationStartDate BudgetStartDate, a.AllocationEndDate BudgetEndDate,
a.BudgetAmount,a.GLCode from ModuleBudget a 
left join Config_BudgetCategories c on c.id=a.BudgetCategoryLookup
left join Department d on d.ID=a.DepartmentLookup
where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup 
and (CONVERT(datetime, a.AllocationStartDate,121) >= CONVERT(datetime, @BudgetStartDate,121) and CONVERT(datetime, AllocationStartDate,121)  <=CONVERT(datetime, @BudgetEndDate,121) 
OR CONVERT(datetime, AllocationEndDate,121) >= CONVERT(datetime, @BudgetStartDate,121) and (CONVERT(datetime, AllocationEndDate,121) <=CONVERT(datetime,@BudgetEndDate,121)))
order by c.BudgetCategoryName
end


end



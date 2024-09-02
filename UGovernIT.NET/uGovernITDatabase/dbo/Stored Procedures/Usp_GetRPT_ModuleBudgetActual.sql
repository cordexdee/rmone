 
Create Procedure [dbo].[Usp_GetRPT_ModuleBudgetActual]
@TenantID varchar(128)='',
@ModuleNameLookup varchar(50)='ITG',
@BudgetStartDate varchar(20)='',
@BudgetEndDate varchar(20)=''


as
begin
Select b.ID, c.BudgetCategoryName BudgetCategory, c.BudgetSubCategory,b.Title BudgetItem,c.BudgetType , a. Title, b.GLCode, d. Title 'DepartmentLookup',a.ActualCost Actuals, 
Replace(convert(varchar(15),a.AllocationStartDate,106),' ','-') BudgetStartDate, Replace(convert(varchar(15),a.AllocationEndDate,106),' ','-')  BudgetEndDate,
 a.BudgetDescription, v.Title VendorLookup,a.ActualCost, a.InvoiceNumber from ModuleBudgetActuals a  left join ModuleBudget b on b.ID=a.ModuleBudgetLookup
 left join Config_BudgetCategories c on c.id=b.BudgetCategoryLookup  
 left join Department d on d.ID=b.DepartmentLookup
 left join AssetVendors v on v.id=a.VendorLookup
 where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup
 and (CONVERT(datetime, a.AllocationStartDate,121) >= CONVERT(datetime, @BudgetStartDate,121) and CONVERT(datetime, a.AllocationStartDate,121)  <=CONVERT(datetime, @BudgetEndDate,121) 
OR CONVERT(datetime, a.AllocationEndDate,121) >= CONVERT(datetime, @BudgetStartDate,121) and (CONVERT(datetime, a.AllocationEndDate,121) <=CONVERT(datetime,@BudgetEndDate,121)))
 order by c.BudgetCategoryName



end


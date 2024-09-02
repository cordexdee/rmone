CREATE procedure [dbo].[Usp_GetModuleBudget]      
@TenantID nvarchar(max)='',      
@ModuleNameLookup nvarchar(10)='',      
@BudgetCategoryLookup nvarchar(max)='',      
@AllocationStartDate datetime='',      
@AllocationEndDate datetime=''      
      
as      
begin      
if(@BudgetCategoryLookup='')      
begin      
Select c.BudgetCategoryName BudgetCategory, d.Title DepartmentTitle, a.* from ModuleBudget a       
left join Config_BudgetCategories c on c.id=a.BudgetCategoryLookup      
left join Department d on d.id=a.DepartmentLookup      
where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup order by c.BudgetCategoryName      
end      
else if(@AllocationStartDate!='')      
begin      
	if(@ModuleNameLookup='')      
		begin      
		Select c.BudgetCategoryName BudgetCategory,d.Title DepartmentTitle, a.* from ModuleBudget a       
		left join Config_BudgetCategories c on c.id=a.BudgetCategoryLookup      
		left join Department d on d.id=a.DepartmentLookup      
		where a.TenantID=@TenantID and a.ModuleNameLookup not in ('ITG')
		and Convert(datetime, a.AllocationEndDate,121)>= @AllocationStartDate and Convert(datetime, a.AllocationStartDate,121)<=@AllocationEndDate      
		and a.BudgetCategoryLookup  in (SELECT CAST(Item AS INTEGER)      
				FROM dbo.SplitString(@BudgetCategoryLookup, ','))       
		  order by c.BudgetCategoryName      
		end      
	else
		begin
		Select c.BudgetCategoryName BudgetCategory,d.Title DepartmentTitle, a.* from ModuleBudget a       
		left join Config_BudgetCategories c on c.id=a.BudgetCategoryLookup      
		left join Department d on d.id=a.DepartmentLookup      
		where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup
		and Convert(datetime, a.AllocationEndDate,121)>= @AllocationStartDate and Convert(datetime, a.AllocationStartDate,121)<=@AllocationEndDate      
		and a.BudgetCategoryLookup  in (SELECT CAST(Item AS INTEGER)      
				FROM dbo.SplitString(@BudgetCategoryLookup, ','))       
		  order by c.BudgetCategoryName      
		end      
end
else begin      
Select c.BudgetCategoryName BudgetCategory,d.Title DepartmentTitle, a.* from ModuleBudget a       
left join Config_BudgetCategories c on c.id=a.BudgetCategoryLookup      
left join Department d on d.id=a.DepartmentLookup      
where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleNameLookup and a.BudgetCategoryLookup  in (SELECT CAST(Item AS INTEGER)      
        FROM dbo.SplitString(@BudgetCategoryLookup, ','))       
  order by c.BudgetCategoryName      
end      
end      


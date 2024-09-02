CREATE PROCEDURE [dbo].[usp_GetProjectStandardWorkItems]
--[usp_GetProjectStandardWorkItems] 'c345e784-aa08-420f-b11f-2753bbebfdd5'
@TenantID varchar(max)  
as  
Begin  
select a.*, c.BudgetCategoryName, c.BudgetSubCategory from ProjectStandardWorkItems a
left join Config_BudgetCategories c on  c.ID = a.BudgetCategoryLookup
where  a.TenantID=@TenantID
order by a.ItemOrder
END

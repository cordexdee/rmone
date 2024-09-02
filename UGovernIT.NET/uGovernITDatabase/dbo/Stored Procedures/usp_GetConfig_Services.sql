CREATE  procedure [dbo].[usp_GetConfig_Services]  
--[usp_GetConfig_Services] 'c345e784-aa08-420f-b11f-2753bbebfdd5','services'  
@TenantID varchar(max)  
as  
Begin  
select a.*, c.ItemOrder as 'ServiceCategoryItemOrder', [dbo].[fnGetusername](a.OwnerUser,@TenantID)[OwnerUser$] from Config_Services a
left join Config_Service_ServiceCategories c on  c.ID = a.CategoryId
where  a.TenantID=@TenantID
order by a.ItemOrder, a.Title, c.ItemOrder
END
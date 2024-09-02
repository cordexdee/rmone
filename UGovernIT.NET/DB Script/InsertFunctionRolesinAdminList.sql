
insert into Config_ClientAdminConfigurationLists(ClientAdminCategoryLookup, Description, ListName, TabSequence, Title, TenantID)
values((select top 1 ID from Config_ClientAdminCategory where CategoryName='User Management' and TenantID='35525396-E5FE-4692-9239-4DF9305B915B'
), 'Function Roles','functionrolemapping', 58, 'Function Roles', '35525396-E5FE-4692-9239-4DF9305B915B')

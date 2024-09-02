declare @i as integer
declare @total as integer

declare @TenantId nvarchar(150)
declare @ListName nvarchar(150)
declare @tabSequence as integer
declare @ClientAdminCategoryLookup as bigint

declare @tmpTenants table
(ID int identity,
TblID uniqueidentifier,
TenantId nvarchar(150)
)

insert into @tmpTenants (TblID, TenantId) select TenantID, TenantID from [core2_common].dbo.Tenant where TenantID = '35525396-E5FE-4692-9239-4DF9305B915B';

select @total = COUNT(*) from @tmpTenants
set @i = 1
while @i <= @total
begin
 select @TenantId = TenantId from @tmpTenants where Id = @i

set @ListName = 'UserCertificates';
IF NOT EXISTS (select Id from Config_ClientAdminConfigurationLists where ListName = @ListName and TenantID = @TenantID)
BEGIN
select @ClientAdminCategoryLookup = Id from Config_ClientAdminCategory where TenantID = @TenantID and CategoryName = 'User Management'

	select @tabSequence = TabSequence from Config_ClientAdminConfigurationLists where TenantID = @TenantID
		and ClientAdminCategoryLookup = @ClientAdminCategoryLookup

	set @tabSequence = @tabSequence + 1

	INSERT INTO [dbo].[Config_ClientAdminConfigurationLists]
			   ([ClientAdminCategoryLookup]
			   ,[Description]
			   ,[ListName]
			   ,[TabSequence]
			   ,[Title]
			   ,[TenantID]
			   ,[Created]
			   ,[Modified]
			   ,[Deleted]
			   ,[Attachments])
		 VALUES
			   (@ClientAdminCategoryLookup
			   ,'User Certification'
			   ,@ListName
			   ,@tabSequence
			   ,'User Certification'
			   , @TenantID
			   ,GETDATE()
			   ,GETDATE()
			   ,0
			   ,'')
END



set @i = @i + 1
end

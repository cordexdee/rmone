
CREATE PROCEDURE [dbo].[CreateDatabaseBackUp]
(
	@Path varchar(250),
	@AccountId nvarchar(64)
)
as
BEGIN

	DECLARE 
		@Date varchar(10),
		@Disk varchar(250)
	
	SET @Date = CONVERT(VARCHAR(10), GETDATE(), 120);	
	SET @Disk = @Path + '\\ugovernit_itsm_before_tenant_deletion_' + @AccountId + '_' + @Date + '.bak';
	
	BACKUP DATABASE [ugovernit] TO  DISK = @Disk 
	WITH  COPY_ONLY, NOFORMAT, INIT,  
	NAME = N'ugovernit-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10;

	-------
	SET @Date = CONVERT(VARCHAR(10), GETDATE(), 120);	
	SET @Disk = @Path + '\\ugovernit_itsm_common_before_tenant_deletion_' + @AccountId + '_' + @Date + '.bak';
	
	BACKUP DATABASE [ugovernit_common] TO  DISK = @Disk 
	WITH  COPY_ONLY, NOFORMAT, INIT,  
	NAME = N'ugovernit-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10;

END

CREATE PROC [dbo].[GetDatabaseBackUp]
@Path varchar(250)
as
BEGIN
DECLARE 
	@Date varchar(10),
	@Disk varchar(250)
	

SET @Date = CONVERT(VARCHAR(10), GETDATE(), 120);
print @Date
SET @Disk = @Path +  @Date + '.bak';
print @Disk

--SET @Disk = N'C:\UGIT\database\backup\ugovernit_itsm_' +  @Date + '.bak';

BACKUP DATABASE ugovernit_itsm TO  DISK = @Disk 
WITH  COPY_ONLY, NOFORMAT, INIT,  
NAME = N'ugovernit-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
END
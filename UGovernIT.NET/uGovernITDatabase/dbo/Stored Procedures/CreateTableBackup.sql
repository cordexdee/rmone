CREATE PROCEDURE [dbo].[CreateTableBackup]
	@TableName nvarchar(256)
AS
BEGIN
	DECLARE @query nvarchar(1000);
	DECLARE @dateFormat nvarchar(50);
	select @dateFormat = FORMAT(getdate(), 'ddMMMyyyy_HHmmss');
	
	set @query = 'SELECT * INTO ' + @TableName + '_bak_' + @dateFormat + ' FROM ' + @TableName;
	
	EXEC (@query)
END
GO

Declare @totaltenants int=0 ,@TenantID nvarchar(128), @i as integer
DECLARE @TenantTable TABLE (Id int Identity, TenantId NVARCHAR(MAX))  
INSERT INTO @TenantTable (TenantId)  
Select TenantID from core_staging_common..tenant where Deleted=0
Set @totaltenants= (Select Count(1) from @TenantTable)  
Set @i=1  
while @i <= @totaltenants  
begin  
Select @TenantID= TenantID from @TenantTable where Id = @i   
IF NOT EXISTS (SELECT 1 from Config_ConfigurationVariable WHERE KeyName = 'CloseoutPeriod' and TenantID = @TenantID)
	BEGIN
	INSERT [dbo].[Config_ConfigurationVariable] ([CategoryName], [Description], [KeyName], [KeyValue], [Title], [Type], [Internal], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments])
	VALUES (N'RMM', N'', N'CloseoutPeriod', N'14', N'CloseoutPeriod', N'Text', 0, N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2023-08-01T13:39:36.813' AS DateTime), CAST(N'2023-08-01T13:39:36.813' AS DateTime), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'')
	PRINT('Record added for Tenant Id: ' + @TenantID)
	END
	ELSE
	PRINT('Record not added for Tenant Id: ' + @TenantID)
Set @i = @i + 1  
End
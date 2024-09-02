
ALTER procedure [dbo].[usp_GetResourceUtlizationFooter_New]      
 @TenantID varchar(128) = '35525396-E5FE-4692-9239-4DF9305B915B', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069' ,   
 @Fromdate datetime ='2023-01-01 00:00:00.000',      
 @Todate datetime ='2023-12-01 00:00:00.000',       
 @Mode varchar(10) = 'Monthly', --'Daily',
 @Closed bit = 'False',      
 @type varchar(15) ='FTE',      
 @Department varchar(2000) = '', -- '570',     
 @IncludeAllResources bit ='True',      
 @IsAssignedallocation bit ='False',      
 @ResourceManager varchar(250) = 'f66c66d3-55d4-47dc-8094-33804dfad0a8',
 @AllocationType varchar(250) = 'Estimated',
 @LevelName varchar(50) ='',
 @JobProfile varchar(500) ='', --'Assistant Project Manager',
 @GlobalRoleId nvarchar(128) ='',
 @UserList nvarchar(max) = '',
 @IsManager  bit =1, 
 @Filter nvarchar(250) = '',  
 @url nvarchar(max)=''
As      
begin     
	
	DECLARE @cols AS NVARCHAR(MAX), @isnullcols AS NVARCHAR(MAX), @issumcols AS NVARCHAR(MAX), @SQLStatement AS NVARCHAR(MAX)

	if @Department = '0'
	begin
		SET @Department = ''
	end

	SELECT * INTO #tmpClosedTickets

	SET @cols = STUFF((SELECT ',' + QUOTENAME(months) 

	SET @isnullcols = STUFF((SELECT ',' + 'Isnull(' + QUOTENAME(months) + ',''0'')' + QUOTENAME(months)

	SET @issumcols = STUFF((SELECT ',' + 'Sum(' + QUOTENAME(months) + ')' + QUOTENAME(months)

	SET @SQLStatement = 'select ''Total FTE'' ResourceUser,'''','''','''','''','''' , '+@issumcols+' FROM (
		)temp'
	
	Print(@SQLStatement)
	exec (@SQLStatement)
	
End;
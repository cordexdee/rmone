Alter Procedure usp_UpdateProjectRoles
@TenantID nvarchar(128) = '35525396-E5FE-4692-9239-4DF9305B915B'
as
begin
	-- Create a temporary table based on a select query
SELECT AssignedToUser, Type, TicketId, r.name, LEFT(TicketId, 3) as ModuleName, pa.Created
INTO #TempTable
from ProjectEstimatedAllocation pa
join Roles r on pa.Type = r.Id
where pa.TenantID = @TenantID and r.TenantID = @TenantID
order by pa.Created
-- Declare variables for cursor
DECLARE @AssignedTo nvarchar(max)
DECLARE @Name varchar(max)
DECLARE @Type nvarchar(128)
DECLARE @TicketId nvarchar(20)
DECLARE @ModuleName nvarchar(3)
-- Declare cursor to loop through temporary table
DECLARE myCursor CURSOR forward_only static FOR SELECT AssignedToUser, Type, TicketId, name, ModuleName FROM #TempTable ORDER BY Created

-- Open cursor
OPEN myCursor

-- Fetch first row
FETCH NEXT FROM myCursor INTO @AssignedTo, @Type, @TicketId, @Name, @ModuleName

-- Loop through cursor
WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @ColumnName varchar(MAX)
     Declare @sqlstr nvarchar(max) = '';
	SELECT TOP 1 @ColumnName = ColumnName FROM Config_Module_ModuleUserTypes WHERE UserTypes = @Name and ModuleNameLookup=@ModuleName and TenantID = @TenantID
    DECLARE @ColumnValue nvarchar(max) = '';
    DECLARE @StageActionUsers nvarchar(max) = '';
	IF @ModuleName = 'CPR'
	begin
        
        SET @sqlstr = 'SELECT @StageActionUsers = StageActionUsersUser, @ColumnValue = ' + QUOTENAME(@ColumnName) + ' FROM CRMProject WHERE TicketId = ''' + @TicketId + ''' AND TenantID = ''' + @TenantID + '''';
        EXEC sp_executesql @sqlstr, N'@StageActionUsers nvarchar(max) OUTPUT, @ColumnValue nvarchar(max) OUTPUT', @StageActionUsers OUTPUT, @ColumnValue OUTPUT;
        --print @sqlstr;
        IF CHARINDEX(CONVERT(VARCHAR(MAX), @AssignedTo), @ColumnValue) > 0
        begin
            SET @ColumnValue = dbo.AddStringWithComma(@ColumnValue, @AssignedTo, ',');
            SET @StageActionUsers = dbo.AddStringWithComma(@StageActionUsers, @AssignedTo, ';#');

		    Set @sqlstr = 'Update CRMProject set ' + @ColumnName + ' = ''' + @ColumnValue + ''', StageActionUsersUser = ''' + @StageActionUsers + ''' where TicketId = ''' + @TicketId + ''' and TenantID = ''' + @TenantID + '''';
            exec(@sqlstr);
        End;
	end;
	IF @ModuleName = 'OPM'
	begin
        SET @sqlstr = 'SELECT @StageActionUsers = StageActionUsersUser, @ColumnValue = ' + QUOTENAME(@ColumnName) + ' FROM Opportunity WHERE TicketId = ''' + @TicketId + ''' AND TenantID = ''' + @TenantID + '''';
        EXEC sp_executesql @sqlstr, N'@StageActionUsers nvarchar(max) OUTPUT, @ColumnValue nvarchar(max) OUTPUT', @StageActionUsers OUTPUT, @ColumnValue OUTPUT;
        
        IF CHARINDEX(CONVERT(VARCHAR(MAX), @AssignedTo), @ColumnValue) > 0
        begin
            SET @ColumnValue = dbo.AddStringWithComma(@ColumnValue, @AssignedTo, ',');
            SET @StageActionUsers = dbo.AddStringWithComma(@StageActionUsers, @AssignedTo, ';#');

		    Set @sqlstr = 'Update Opportunity set ' + @ColumnName + ' = ''' + @ColumnValue + ''', StageActionUsersUser = ''' + @StageActionUsers + ''' where TicketId = ''' + @TicketId + ''' and TenantID = ''' + @TenantID + '''';
            exec(@sqlstr);
        End;
	end;
	IF @ModuleName = 'CNS'
	begin
        SET @sqlstr = 'SELECT @StageActionUsers = StageActionUsersUser, @ColumnValue = ' + QUOTENAME(@ColumnName) + ' FROM CRMServices WHERE TicketId = ''' + @TicketId + ''' AND TenantID = ''' + @TenantID + '''';
        EXEC sp_executesql @sqlstr, N'@StageActionUsers nvarchar(max) OUTPUT, @ColumnValue nvarchar(max) OUTPUT', @StageActionUsers OUTPUT, @ColumnValue OUTPUT;
        
        IF CHARINDEX(CONVERT(VARCHAR(MAX), @AssignedTo), @ColumnValue) > 0
        begin
            SET @ColumnValue = dbo.AddStringWithComma(@ColumnValue, @AssignedTo, ',');
            SET @StageActionUsers = dbo.AddStringWithComma(@StageActionUsers, @AssignedTo, ';#');

		    Set @sqlstr = 'Update CRMServices set ' + @ColumnName + ' = ''' + @ColumnValue + ''', StageActionUsersUser = ''' + @StageActionUsers + ''' where TicketId = ''' + @TicketId + ''' and TenantID = ''' + @TenantID + '''';
            exec(@sqlstr);
        End;
	end;
  -- Fetch next row
  FETCH NEXT FROM myCursor INTO @AssignedTo, @Type, @TicketId, @Name, @ModuleName
END

-- Close cursor
CLOSE myCursor

-- Deallocate cursor
DEALLOCATE myCursor

-- Drop temporary table
DROP TABLE #TempTable

end
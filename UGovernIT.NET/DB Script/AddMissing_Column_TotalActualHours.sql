DECLARE @ColumnName NVARCHAR(255) = 'TotalActualHours'
DECLARE @DataType NVARCHAR(255) = 'float'
DECLARE @TableName NVARCHAR(255)
DECLARE @SqlStatement NVARCHAR(MAX)

DECLARE tableCursor CURSOR FOR


SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' and TABLE_NAME in (Select ModuleTable from Config_Modules 
where TenantID='35525396-e5fe-4692-9239-4df9305b915b' and ModuleTable not in ('HelpCard',
'WikiArticles'))

OPEN tableCursor
FETCH NEXT FROM tableCursor INTO @TableName

WHILE @@FETCH_STATUS = 0
BEGIN
    IF NOT EXISTS (
        SELECT *
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = @TableName
        AND COLUMN_NAME = @ColumnName
    )
    BEGIN
        SET @SqlStatement = 'ALTER TABLE ' + @TableName + ' ADD ' + @ColumnName + ' ' + @DataType
        EXEC sp_executesql @SqlStatement
    END

    FETCH NEXT FROM tableCursor INTO @TableName
END

CLOSE tableCursor
DEALLOCATE tableCursor


 



DECLARE @table_name NVARCHAR(max) = 'Opportunity';
DECLARE @column_name_pattern NVARCHAR(max) = '%User';

-- Create a temporary table to store the updated column values
CREATE TABLE #UpdatedColumns (
    TABLE_NAME NVARCHAR(max),
    COLUMN_NAME NVARCHAR(max)
);

-- Generate dynamic SQL to update the columns
DECLARE @sql NVARCHAR(max) = '';

SELECT @sql += 'UPDATE Opportunity
                SET ' + COLUMN_NAME + ' = (
                    SELECT STRING_AGG(SplitVal, '','')
                    FROM (
                        SELECT value AS SplitVal,
                               ROW_NUMBER() OVER (PARTITION BY value ORDER BY (SELECT NULL)) AS RowNum
                        FROM STRING_SPLIT(' + COLUMN_NAME + ', '','')
                    ) AS sub
                    WHERE RowNum = 1
                );'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = @table_name
    AND COLUMN_NAME LIKE @column_name_pattern;

-- Execute the dynamic SQL
EXEC(@sql);
--Print(@sql);

-- Clean up the temporary table
DROP TABLE #UpdatedColumns;

CREATE Procedure [dbo].[usp_GetClosedTickets] 
--usp_GetClosedTickets '35525396-e5fe-4692-9239-4df9305b915b' , 'CPR,OPM,CNS,PMM,NPR'
@tenantid varchar(max),
@ModuleNames varchar(max)
as
begin

DECLARE @Counter INT,
@sql as nvarchar(max) = N'',
@tblname varchar(max)

SET @Counter=1
CREATE TABLE #TempTable(
ID int,
ModuleTable varchar(100))

INSERT INTO #TempTable (ID,  ModuleTable) 
SELECT ROW_NUMBER() OVER(ORDER BY ModuleTable) AS RowNum,   ModuleTable
from Config_Modules where TenantID= @tenantid and ModuleName in (SELECT * FROM DBO.SPLITSTRING(@ModuleNames, ','))

WHILE (@Counter <= (Select count(1) from #TempTable))
BEGIN
Set @tblname= (Select ModuleTable from #TempTable where id=@Counter);
    set @sql = @sql + N'SELECT TicketId FROM ' +@tblname+ ' Where TenantId='''+@tenantid+''' AND Closed =1'   
	+ Char(10) 
	+N'  UNION ALL '
    SET @Counter  = @Counter  + 1
END
set @sql = left(@sql, len(@sql) - 10)
 
EXEC (@sql)
--Print(@SQL)
Drop table #TempTable
END


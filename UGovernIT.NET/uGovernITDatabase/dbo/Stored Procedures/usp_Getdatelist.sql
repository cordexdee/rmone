
CREATE Procedure [dbo].[usp_Getdatelist] 
@_Fromdate datetime,
@_Todate datetime,
@Mode Char(10)
as

BEGIN
DECLARE @SQLStatement NVARCHAR(MAX) = N''
DECLARE @UniqueToPivot NVARCHAR(MAX) = N''
DECLARE @PivotColumnsToSelect NVARCHAR(MAX) = N''
Declare @Fromdate datetime, @Todate datetime
Set @Fromdate=@_Fromdate 
Set @Todate=@_Todate
--Exec [usp_Getdatelist] '2024-01-01 00:00:00.000' ,'2024-03-31 00:00:00.000','Weekly'
--select months as MName, id from dbo.GetMonthList('2024-01-01 00:00:00.000' ,'2024-03-31 00:00:00.000','Weekly') group by months, id
SELECT @UniqueToPivot = @UniqueToPivot + ', [' + COALESCE(MName, '') + ']'
FROM (
select months as MName,id from dbo.GetMonthList(@_Fromdate,@_Todate,@Mode) group by months, id
)DT
SELECT @UniqueToPivot = LTRIM(STUFF(@UniqueToPivot, 1, 1, ''))

SELECT  @PivotColumnsToSelect =   @UniqueToPivot 
--@PivotColumnsToSelect + ', ISNULL([' + COALESCE(MName, '') + '], 0) AS [' + MName + ']'
FROM (
select months as MName, id from dbo.GetMonthList(@_Fromdate,@_Todate,@Mode) group by months, id
)DT 
--Generate dynamic PIVOT query here

SET @SQLStatement =
N'SELECT '
+ @PivotColumnsToSelect +
'
FROM (
select months as MName, id from dbo.GetMonthList('+ Convert(varchar(10), @Fromdate, 121)+ ',' + Convert(varchar(10), @Fromdate, 121)+ ','''+@Mode+''') group by months, id
) s
PIVOT
(max(id)for MName in
(' + @UniqueToPivot + ')
) AS p 
'
--Execute the dynamic t-sql PIVOT query below
Print (@SQLStatement)
EXEC (@SQLStatement)
END
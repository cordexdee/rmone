
  
CReate Procedure [dbo].[usp_GetdatelistAllocation]   
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
--Exec [usp_GetdatelistAllocation] '2021-01-01 00:00:00.000' ,'2021-12-01 00:00:00.000','Weekly'  
--select months as MName, id from dbo.GetMonthListForResourceAllocation('2021-01-01 00:00:00.000' ,'2021-12-01 00:00:00.000','Weekly')  
SELECT @UniqueToPivot = @UniqueToPivot + ', [' + COALESCE(MName, '') + ']'  
FROM (  
select months as MName,id from dbo.GetMonthListForResourceAllocation(@_Fromdate,@_Todate,@Mode)  
)DT  
SELECT @UniqueToPivot = LTRIM(STUFF(@UniqueToPivot, 1, 1, ''))  
  
SELECT  @PivotColumnsToSelect =   @UniqueToPivot   
--@PivotColumnsToSelect + ', ISNULL([' + COALESCE(MName, '') + '], 0) AS [' + MName + ']'  
FROM (  
select months as MName, id from dbo.GetMonthListForResourceAllocation(@_Fromdate,@_Todate,@Mode)  
)DT   
--Generate dynamic PIVOT query here  
  
SET @SQLStatement =  
N'SELECT '  
+ @PivotColumnsToSelect +  
'  
FROM (  
select months as MName, id from dbo.GetMonthListForResourceAllocation('+ Convert(varchar(10), @Fromdate, 121)+ ',' + Convert(varchar(10), @Fromdate, 121)+ ','''+@Mode+''')  
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
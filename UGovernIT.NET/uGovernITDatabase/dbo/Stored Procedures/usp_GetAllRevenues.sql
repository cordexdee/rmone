/*
exec usp_GetAllRevenues '35525396-e5fe-4692-9239-4df9305b915b'
*/
alter proc [dbo].[usp_GetAllRevenues]  
(  
@TenantId nvarchar(256)
)  
AS  
BEGIN  
  declare @CurrentYear varchar(4)
   set @CurrentYear=year(getdate())

 CREATE TABLE #tmpKPItable(  
  Category nvarchar(256),  
  KPI nvarchar(256),  
  Jan decimal(18,2),  
  Feb decimal(18,2),  
  Mar decimal(18,2),  
  Apr decimal(18,2),  
  May decimal(18,2),  
  Jun decimal(18,2),  
  Jul decimal(18,2),  
  Aug decimal(18,2),  
  Sep decimal(18,2),  
  Oct decimal(18,2),  
  Nov decimal(18,2),  
  [Dec] decimal(18,2),  
  DataType nvarchar(50)  
 )  
 insert into #tmpKPItable exec usp_GetResourceUtilizationIndex @TenantId,@CurrentYear,'sector','pct'  
  
 --select KPI,sum(isnull(Jan,0)) as Jan,sum(isnull(Feb,0)) as Feb,sum(isnull(Mar,0)) as Mar,sum(isnull(Apr,0)) as Apr,  
 --sum(isnull(May,0)) as May,sum(isnull(Jun,0)) as Jun,sum(isnull(Jul,0)) as Jul,sum(isnull(Aug,0)) as Aug,sum(isnull(Sep,0)) as Sep,sum(isnull(Oct,0)) as Oct,sum(isnull(Nov,0)) as Nov,  
 --sum(isnull([Dec],0)) as [Dec],DataType ,@TenantId As TenantId  from #tmpKPItable where KPI in('Pipeline Revenues','Committed Revenues')  
 --group by KPI,DataType  
 SELECT KPI, [Month], [Value] into #temp1  
FROM   
 ( select KPI,sum(isnull(Jan,0)) as Jan,sum(isnull(Feb,0)) as Feb,sum(isnull(Mar,0)) as Mar,sum(isnull(Apr,0)) as Apr,  
 sum(isnull(May,0)) as May,sum(isnull(Jun,0)) as Jun,sum(isnull(Jul,0)) as Jul,sum(isnull(Aug,0)) as Aug,sum(isnull(Sep,0)) as Sep,sum(isnull(Oct,0)) as Oct,sum(isnull(Nov,0)) as Nov,  
 sum(isnull([Dec],0)) as [Dec],DataType ,@TenantId As TenantId  from #tmpKPItable where KPI in('Pipeline Revenues','Committed Revenues','Missed Revenues')  
 group by KPI,DataType) p  
UNPIVOT  
   (  
   [Value]   
   FOR [Month] IN (Jan, Feb, Mar, Apr, May,Jun,Jul,Aug,Sep,Oct,Nov,[Dec])  
   ) AS unpvt;  
   SELECT [Month],[Pipeline Revenues] as PipelineRevenues,[Committed Revenues] as CommittedRevenues,[Missed Revenues] as MissedRevenues  
FROM  
(  
    SELECT *  
    FROM #temp1  
) AS SourceTable PIVOT(sum([Value]) FOR [KPI] IN([Pipeline Revenues],  
                                                         [Committed Revenues],[Missed Revenues])) AS PivotTable;  
END  
  
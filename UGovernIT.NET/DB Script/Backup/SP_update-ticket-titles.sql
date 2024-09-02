--UpdateTicketTitles '35525396-e5fe-4692-9239-4df9305b915b'
CREATE proc UpdateTicketTitles
(@TenantID as nvarchar(128) )
AS
BEGIN
--declare @TenantID as nvarchar(128) = ''

 CREATE TABLE #tmpProjects  
 (  
  TicketId varchar(50),  
  Title nvarchar(1000)  
 )  
  
 insert into #tmpProjects  
 (  
  TicketId,  
  Title  
 )  
 select TicketId, Title  
 from CRMProject where TenantID = @TenantID  
  
 insert into #tmpProjects  
 (  
  TicketId,  
  Title  
 )  
 select TicketId, Title  
 from Opportunity where TenantID = @TenantID  
  
 insert into #tmpProjects  
 (  
  TicketId,  
  Title  
 )  
 select TicketId, Title  
 from CRMServices where TenantID = @TenantID  
   
 insert into #tmpProjects  
 (  
  TicketId,  
  Title  
 )  
 select TicketId, Title  
 from PMM where TenantID = @TenantID  
   
 insert into #tmpProjects  
 (  
  TicketId,  
  Title  
 )  
 select TicketId, Title  
 from NPR where TenantID = @TenantID  
   
 --select * from #tmpProjects;  
BEGIN TRY
PRINT 'Begin: Updating ResourceUsageSummaryWeekWise'
 update ResourceUsageSummaryWeekWise 
 set ResourceUsageSummaryWeekWise.title = #tmpProjects.title
 from #tmpProjects 
 where 
	ResourceUsageSummaryWeekWise.WorkItem = #tmpProjects.TicketId
	AND TenantID = @TenantID  


select * from ResourceUsageSummaryWeekWise
PRINT 'End: Updating ResourceUsageSummaryWeekWise'

PRINT 'Begin: Updating ResourceUsageSummaryMonthWise'
 update ResourceUsageSummaryMonthWise
 set ResourceUsageSummaryMonthWise.title = #tmpProjects.title
 from #tmpProjects 
 where 
	ResourceUsageSummaryMonthWise.WorkItem = #tmpProjects.TicketId
	AND TenantID = @TenantID  
PRINT 'End: Updating ResourceUsageSummaryMonthWise'

select * from ResourceUsageSummaryMonthWise
END TRY
BEGIN CATCH
	SELECT  
    ERROR_NUMBER() AS ErrorNumber  
    ,ERROR_SEVERITY() AS ErrorSeverity  
    ,ERROR_STATE() AS ErrorState  
    ,ERROR_PROCEDURE() AS ErrorProcedure  
    ,ERROR_LINE() AS ErrorLine  
    ,ERROR_MESSAGE() AS ErrorMessage
END CATCH

DROP TABLE #tmpProjects

END
GO
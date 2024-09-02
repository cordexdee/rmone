 
Create view [dbo].[vw_SprintSummary] as
--select ID,Title,PMMIdLookup,Description,ItemOrder,StartDate,EndDate,RemainingHours,TaskEstimatedHours,SprintLookup,PercentComplete,TenantID,Created

select * from SprintSummary readonly WITH (NOLOCK)   



CREATE VIEW [dbo].[vw_TicketwithSatus]
AS

SELECT DISTINCT TicketId, Closed, C.TenantID, ERPJobID, PreconStartDate, PreconEndDate, EstimatedConstructionStart,EstimatedConstructionEnd, CloseoutStartDate, ISNULL(OnHold, 0 ) as OnHold, Status,ChanceOfSuccessChoice,
case when CloseoutDate is null and CloseoutStartDate is not null then DateAdd(day, (case when (select count(*) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') > 0 then (select top 1 cast(KeyValue as int) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') else 14 end),CloseoutStartDate) else CloseoutDate end as CloseoutDate
,A.Id ProjectLeadUser$Id ,A.Name ProjectLeadUser,B.Id LeadEstimatorUser$Id,B.Name LeadEstimatorUser,D.Id ProjectManagerUser$Id,D.Name ProjectManagerUser  
FROM CRMProject C LEFT JOIN AspNetUsers A ON C.ProjectLeadUser=A.Id 
LEFT JOIN AspNetUsers B ON C.LeadEstimatorUser=B.Id 
LEFT JOIN AspNetUsers D ON C.ProjectManagerUser=D.Id
/*where TenantID='35525396-e5fe-4692-9239-4df9305b915b'*/ UNION ALL
SELECT DISTINCT TicketId, Closed, C.TenantID, ERPJobIDNC AS ERPJobID, PreconStartDate, PreconEndDate, EstimatedConstructionStart,EstimatedConstructionEnd, CloseoutStartDate, ISNULL(OnHold, 0 ) as OnHold, Status,ChanceOfSuccessChoice,
case when CloseoutDate is null and CloseoutStartDate is not null then DateAdd(day, (case when (select count(*) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') > 0 then (select top 1 cast(KeyValue as int) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') else 14 end),CloseoutStartDate) else CloseoutDate end as CloseoutDate
,A.Id ProjectLeadUser$Id ,A.Name ProjectLeadUser,B.Id LeadEstimatorUser$Id,B.Name LeadEstimatorUser,D.Id ProjectManagerUser$Id,D.Name ProjectManagerUser
FROM Opportunity C LEFT JOIN AspNetUsers A ON C.ProjectLeadUser=A.Id 
LEFT JOIN AspNetUsers B ON C.LeadEstimatorUser=B.Id 
LEFT JOIN AspNetUsers D ON C.ProjectManagerUser=D.Id
union all
SELECT DISTINCT TicketId, Closed, C.TenantID, ERPJobIDNC AS ERPJobID, PreconStartDate, PreconEndDate, EstimatedConstructionStart,EstimatedConstructionEnd, CloseoutStartDate, ISNULL(OnHold, 0 ) as OnHold, Status,'' as ChanceOfSuccessChoice,
case when CloseoutDate is null and CloseoutStartDate is not null then DateAdd(day, (case when (select count(*) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') > 0 then (select top 1 cast(KeyValue as int) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') else 14 end),CloseoutStartDate) else CloseoutDate end as CloseoutDate
,A.Id ProjectLeadUser$Id ,A.Name ProjectLeadUser,B.Id LeadEstimatorUser$Id,B.Name LeadEstimatorUser,D.Id ProjectManagerUser$Id,D.Name ProjectManagerUser
FROM CRMServices C LEFT JOIN AspNetUsers A ON C.ProjectLeadUser=A.Id 
LEFT JOIN AspNetUsers B ON C.LeadEstimatorUser=B.Id 
LEFT JOIN AspNetUsers D ON C.ProjectManagerUser=D.Id
GO



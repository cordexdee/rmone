
CREATE VIEW [dbo].[vw_TicketwithSatus]
AS

SELECT DISTINCT TicketId, Closed, TenantID, ERPJobID, PreconStartDate, PreconEndDate, EstimatedConstructionStart,EstimatedConstructionEnd, CloseoutStartDate, ISNULL(OnHold, 0 ) as OnHold, Status,ChanceOfSuccessChoice,
case when CloseoutDate is null and CloseoutStartDate is not null then DateAdd(day, (case when (select count(*) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') > 0 then (select top 1 cast(KeyValue as int) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') else 14 end),CloseoutStartDate) else CloseoutDate end as CloseoutDate
FROM            CRMProject
/*where TenantID='35525396-e5fe-4692-9239-4df9305b915b'*/ UNION ALL
SELECT DISTINCT TicketId, Closed, TenantID, ERPJobIDNC AS ERPJobID, PreconStartDate, PreconEndDate, EstimatedConstructionStart,EstimatedConstructionEnd, CloseoutStartDate, ISNULL(OnHold, 0 ) as OnHold, Status,ChanceOfSuccessChoice,
case when CloseoutDate is null and CloseoutStartDate is not null then DateAdd(day, (case when (select count(*) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') > 0 then (select top 1 cast(KeyValue as int) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') else 14 end),CloseoutStartDate) else CloseoutDate end as CloseoutDate
FROM            Opportunity
union all
SELECT DISTINCT TicketId, Closed, TenantID, ERPJobIDNC AS ERPJobID, PreconStartDate, PreconEndDate, EstimatedConstructionStart,EstimatedConstructionEnd, CloseoutStartDate, ISNULL(OnHold, 0 ) as OnHold, Status,'' as ChanceOfSuccessChoice,
case when CloseoutDate is null and CloseoutStartDate is not null then DateAdd(day, (case when (select count(*) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') > 0 then (select top 1 cast(KeyValue as int) from Config_ConfigurationVariable where KeyName = 'CloseoutPeriod') else 14 end),CloseoutStartDate) else CloseoutDate end as CloseoutDate
FROM            CRMServices
GO



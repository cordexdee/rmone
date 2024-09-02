
ALTER TABLE CRMServices ADD PreconDuration INT, EstimatedConstructionDuration INT
ALTER TABLE CRMProject ADD PreconDuration INT, EstimatedConstructionDuration INT
ALTER TABLE Opportunity ADD PreconDuration INT, EstimatedConstructionDuration INT
GO


---------------========================

UPDATE CRMProject SET PreconDuration=(Select dbo.fnGetWorkingDays(PreconStartDate,PreconEndDate,TenantID)) 
WHERE PreconStartDate IS NOT NULL AND PreconEndDate IS NOT NULL AND PreconEndDate>=PreconStartDate
GO

UPDATE CRMProject SET EstimatedConstructionDuration=(Select dbo.fnGetWorkingDays(EstimatedConstructionStart,EstimatedConstructionEnd,TenantID)) 
WHERE EstimatedConstructionStart IS NOT NULL AND EstimatedConstructionEnd IS NOT NULL AND EstimatedConstructionEnd>=EstimatedConstructionStart 
GO

UPDATE CRMServices SET PreconDuration=(Select dbo.fnGetWorkingDays(PreconStartDate,PreconEndDate,TenantID)) 
WHERE PreconStartDate IS NOT NULL AND PreconEndDate IS NOT NULL AND PreconEndDate>=PreconStartDate
GO

UPDATE CRMServices SET EstimatedConstructionDuration=(Select dbo.fnGetWorkingDays(EstimatedConstructionStart,EstimatedConstructionEnd,TenantID)) 
WHERE EstimatedConstructionStart IS NOT NULL AND EstimatedConstructionEnd IS NOT NULL AND EstimatedConstructionEnd>=EstimatedConstructionStart
GO

UPDATE Opportunity SET PreconDuration=(Select dbo.fnGetWorkingDays(PreconStartDate,PreconEndDate,TenantID)) 
WHERE PreconStartDate IS NOT NULL AND PreconEndDate IS NOT NULL AND PreconEndDate>=PreconStartDate 
GO

UPDATE Opportunity SET EstimatedConstructionDuration=(Select dbo.fnGetWorkingDays(EstimatedConstructionStart,EstimatedConstructionEnd,TenantID)) 
WHERE EstimatedConstructionStart IS NOT NULL AND EstimatedConstructionEnd IS NOT NULL AND EstimatedConstructionEnd>=EstimatedConstructionStart
GO
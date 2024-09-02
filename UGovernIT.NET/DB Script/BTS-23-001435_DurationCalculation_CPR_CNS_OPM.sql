UPDATE CRMProject SET Duration=(Select dbo.fnGetWorkingDays(EstimatedConstructionStart,EstimatedConstructionEnd,TenantID)/5) 
WHERE EstimatedConstructionStart IS NOT NULL AND EstimatedConstructionEnd IS NOT NULL AND EstimatedConstructionEnd>=EstimatedConstructionStart
GO

UPDATE CRMServices SET Duration=(Select dbo.fnGetWorkingDays(EstimatedConstructionStart,EstimatedConstructionEnd,TenantID)/5) 
WHERE EstimatedConstructionStart IS NOT NULL AND EstimatedConstructionEnd IS NOT NULL AND EstimatedConstructionEnd>=EstimatedConstructionStart
GO

UPDATE Opportunity SET CRMDuration=(Select dbo.fnGetWorkingDays(EstimatedConstructionStart,EstimatedConstructionEnd,TenantID)/5) 
WHERE EstimatedConstructionStart IS NOT NULL AND EstimatedConstructionEnd IS NOT NULL AND EstimatedConstructionEnd>=EstimatedConstructionStart
GO
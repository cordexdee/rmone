
DROP INDEX IX_CRMServices_Volatility ON CRMServices
GO
ALTER TABLE CRMServices
ALTER COLUMN Volatility FLOAT
GO
CREATE INDEX IX_CRMServices_Volatility
ON Opportunity (Volatility)

GO
DROP INDEX IX_SVCRequests_ModuleStepLookup ON SVCRequests
GO
ALTER TABLE SVCRequests
ALTER COLUMN ModuleStepLookup BIGINT
GO
CREATE INDEX IX_SVCRequests_ModuleStepLookup
ON SVCRequests (ModuleStepLookup)

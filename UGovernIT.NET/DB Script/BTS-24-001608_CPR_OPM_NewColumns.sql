ALTER TABLE CRMProject ADD GLInsurance FLOAT, SDI FLOAT, Bond FLOAT
ALTER TABLE Opportunity ADD GLInsurance FLOAT, SDI FLOAT, Bond FLOAT

ALTER TABLE CRMProject ADD AuditRights NVARCHAR(MAX)
ALTER TABLE Opportunity ADD AuditRights NVARCHAR(MAX)
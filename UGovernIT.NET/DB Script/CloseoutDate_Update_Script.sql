
UPDATE CRMProject 
SET CloseoutDate = DATEADD(day, 14, CloseoutStartDate) 
WHERE CloseoutStartDate IS NOT NULL and CloseoutDate IS NULL
and TenantID='35525396-E5FE-4692-9239-4DF9305B915B' 

UPDATE Opportunity 
SET CloseoutDate = DATEADD(day, 14, CloseoutStartDate) 
WHERE CloseoutStartDate IS NOT NULL and CloseoutDate IS NULL
and TenantID='35525396-E5FE-4692-9239-4DF9305B915B' 

UPDATE CRMServices 
SET CloseoutDate = DATEADD(day, 14, CloseoutStartDate) 
WHERE CloseoutStartDate IS NOT NULL and CloseoutDate IS NULL
and TenantID='35525396-E5FE-4692-9239-4DF9305B915B' 
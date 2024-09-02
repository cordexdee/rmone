-- Save this Data
select Convert(nvarchar,PreconStartDate, 101) as PreconStart, 
 convert(nvarchar, PreconEndDate, 101) as PreconEnd, 
 Convert(nvarchar, EstimatedConstructionStart, 101) as ConstStart, 
 Convert(nvarchar, EstimatedConstructionEnd, 101) as ConstEnd,
 Convert(nvarchar, CloseoutDate, 101) as CloseoutEnd, TicketId, Title
 from Opportunity where TenantID='35525396-E5FE-4692-9239-4DF9305B915B'


 -- Add new CloseoutStartDate column to CRMProject table
ALTER TABLE Opportunity 
ADD CloseoutStartDate DATETIME NULL

-- Update CloseoutStartDate column based on EstimatedConstructionEnd
UPDATE Opportunity 
SET CloseoutStartDate = DATEADD(day, 1, EstimatedConstructionEnd) 
WHERE EstimatedConstructionEnd IS NOT NULL
and TenantID='35525396-E5FE-4692-9239-4DF9305B915B'


-- Save this Data
select Convert(nvarchar,PreconStartDate, 101) as PreconStart, 
 convert(nvarchar, PreconEndDate, 101) as PreconEnd, 
 Convert(nvarchar, EstimatedConstructionStart, 101) as ConstStart, 
 Convert(nvarchar, EstimatedConstructionEnd, 101) as ConstEnd,
 Convert(nvarchar, CloseoutStartDate, 101) as CloseoutStart,
 Convert(nvarchar, CloseoutDate, 101) as CloseoutEnd, TicketId, Title
 from Opportunity where TenantID='35525396-E5FE-4692-9239-4DF9305B915B'
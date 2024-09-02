ALTER TABLE ProjectEstimatedAllocation 
ADD IsLocked bit 

update ProjectEstimatedAllocation set islocked = 0
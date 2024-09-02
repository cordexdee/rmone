

--CPR
IF EXISTS (
  SELECT 1
  FROM ProjectEstimatedAllocation ra 
  JOIN CRMProject cpr ON ra.TicketId = cpr.TicketId
  JOIN AspNetUsers u ON ra.AssignedToUser = u.Id
  WHERE ra.AllocationStartDate = cpr.EstimatedConstructionEnd
)
BEGIN
  UPDATE ProjectEstimatedAllocation
  SET AllocationStartDate = DATEADD(day, 1, AllocationStartDate)
  WHERE ID IN (
    SELECT ra.ID
    FROM ProjectEstimatedAllocation ra 
    JOIN CRMProject cpr ON ra.TicketId = cpr.TicketId
    JOIN AspNetUsers u ON ra.AssignedToUser = u.Id
    WHERE ra.AllocationStartDate = cpr.EstimatedConstructionEnd
  )
END

--OPM
IF EXISTS (
  SELECT 1
  FROM ProjectEstimatedAllocation ra 
    JOIN Opportunity opm ON ra.TicketId = opm.TicketId
    JOIN AspNetUsers u ON ra.AssignedToUser = u.Id
    WHERE ra.AllocationStartDate = opm.EstimatedConstructionEnd
)
BEGIN
  update ProjectEstimatedAllocation set AllocationStartDate = Dateadd(day, 1, AllocationStartDate)
    where ID in (
    SELECT ra.ID
    FROM ProjectEstimatedAllocation ra 
    JOIN Opportunity opm ON ra.TicketId = opm.TicketId
    JOIN AspNetUsers u ON ra.AssignedToUser = u.Id
    WHERE ra.AllocationStartDate = opm.EstimatedConstructionEnd
    )
END

--CNS
IF EXISTS (
  SELECT 1
  FROM ProjectEstimatedAllocation ra 
  JOIN CRMServices cns ON ra.TicketId = cns.TicketId
  JOIN AspNetUsers u ON ra.AssignedToUser = u.Id
  WHERE ra.AllocationStartDate = cns.EstimatedConstructionEnd
)
BEGIN
  UPDATE ProjectEstimatedAllocation
  SET AllocationStartDate = DATEADD(day, 1, AllocationStartDate)
  WHERE ID IN (
    SELECT ra.ID
    FROM ProjectEstimatedAllocation ra 
      JOIN CRMServices cns ON ra.TicketId = cns.TicketId
      JOIN AspNetUsers u ON ra.AssignedToUser = u.Id
      WHERE ra.AllocationStartDate = cns.EstimatedConstructionEnd
  )
END
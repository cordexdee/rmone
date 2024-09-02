 
Create view [dbo].[vw_DashboardSummary] as
--select ID,Title,ModuleNameLookup,GenericStatusLookup,Status,TicketId,InitiatorUser,RequestorUser,OwnerUser,PRPGroupUser,PRPUser,ORPUser,ActualHours,PriorityLookup,RequestTypeLookup,InitiatorResolvedChoice,OnHold,CreationDate,RequestSourceChoice,SLAMet,Category,WorkflowType,StageActionUsersUser,FunctionalAreaLookup,LocationLookup,State,Country,Region,RequestorCompany,RequestorDivision,RequestorDepartment,Closed,ServiceName,ServiceCategoryName,SubCategory,AssignmentSLAMet,RequestorContactSLAMet,ResolutionSLAMet,CloseSLAMet,OtherSLAMet,ALLSLAsMet,ResolutionTypeChoice,Rejected,TotalHoldDuration,OnHoldTillDate,ClosedByUser,ResolvedByUser,SLADisabled,TenantID,Age
select * from DashboardSummary readonly WITH (NOLOCK)   

  


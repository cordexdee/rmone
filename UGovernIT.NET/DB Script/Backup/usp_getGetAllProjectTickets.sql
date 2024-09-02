 CREATE Procedure usp_getGetAllProjectTickets 
 --usp_getGetAllProjectTickets '35525396-E5FE-4692-9239-4DF9305B915B'
 @TenantID varchar(128)   
 as
 begin
 Select TicketId,Title,ERPJobID,CRMCompanyLookup,dbo.fnGetCompanyTitle(CRMCompanyLookup,@TenantID) as CRMCompanyTitleLookup ,Closed,PreconStartDate,PreconEndDate,EstimatedConstructionStart,EstimatedConstructionEnd,CloseoutDate
 from CRMProject a 
 where TenantID=@TenantID
 Union all
  Select TicketId,Title,ERPJobID,CRMCompanyLookup,dbo.fnGetCompanyTitle(CRMCompanyLookup,@TenantID) as CRMCompanyTitleLookup,Closed,PreconStartDate,PreconEndDate,EstimatedConstructionStart,EstimatedConstructionEnd,CloseoutDate
 from Opportunity where TenantID=@TenantID
 Union all
  Select TicketId,Title,ERPJobID,CRMCompanyLookup,dbo.fnGetCompanyTitle(CRMCompanyLookup,@TenantID) as CRMCompanyTitleLookup,Closed,PreconStartDate,PreconEndDate,EstimatedConstructionStart,EstimatedConstructionEnd,CloseoutDate
 from CRMServices where TenantID=@TenantID
 End
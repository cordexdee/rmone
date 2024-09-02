CREATE Procedure [dbo].[usp_getGetAllProjectTickets]       
 --usp_getGetAllProjectTickets '35525396-E5FE-4692-9239-4DF9305B915B' ,0     
 @TenantID varchar(128),
 @includeClosed bit
 as      
 begin      
 If(@includeClosed=1)
 Begin
	 Select a.TicketId,a.Title,a.ERPJobID,a.ERPJobIDNC,a.CRMCompanyLookup,com.Title as CRMCompanyTitleLookup ,a.Closed,    
	 a.PreconStartDate,a.PreconEndDate,a.EstimatedConstructionStart,a.EstimatedConstructionEnd,a.CloseoutDate,a.CloseoutStartDate,    
	 a.Description,a.RequestTypeLookup,a.ApproxContractValue,a.CRMProjectComplexityChoice, a.Status    
	 from CRMProject a left join CRMCompany com on com.TicketId=a.CRMCompanyLookup  and com.TenantID=@TenantID
	 where a.TenantID=@TenantID
	 Union all      
	  Select o.TicketId,o.Title,o.ERPJobID,o.ERPJobIDNC,o.CRMCompanyLookup,com.Title as CRMCompanyTitleLookup ,o.Closed,    
	  o.PreconStartDate,o.PreconEndDate,o.EstimatedConstructionStart,o.EstimatedConstructionEnd,o.CloseoutDate,o.CloseoutStartDate,    
	  o.Description,o.RequestTypeLookup,o.ApproxContractValue,o.CRMProjectComplexityChoice, o.Status    
	 from Opportunity o left join CRMCompany com on com.TicketId=o.CRMCompanyLookup and com.TenantID=@TenantID    
	 where o.TenantID=@TenantID    
	 Union all      
	  Select s.TicketId,s.Title,s.ERPJobID,s.ERPJobIDNC,s.CRMCompanyLookup,com.Title as CRMCompanyTitleLookup ,s.Closed,    
	  s.PreconStartDate,s.PreconEndDate,s.EstimatedConstructionStart,s.EstimatedConstructionEnd,s.CloseoutDate,s.CloseoutStartDate,    
	  s.Description,s.RequestTypeLookup,s.ApproxContractValue,s.CRMProjectComplexityChoice, s.Status    
	 from CRMServices s left join CRMCompany com on com.TicketId=s.CRMCompanyLookup  and com.TenantID=@TenantID      
	 where s.TenantID=@TenantID     
 End
 Else Begin
 
 Select a.TicketId,a.Title,a.ERPJobID,a.ERPJobIDNC,a.CRMCompanyLookup,com.Title as CRMCompanyTitleLookup ,a.Closed,    
	 a.PreconStartDate,a.PreconEndDate,a.EstimatedConstructionStart,a.EstimatedConstructionEnd,a.CloseoutDate,a.CloseoutStartDate,    
	 a.Description,a.RequestTypeLookup,a.ApproxContractValue,a.CRMProjectComplexityChoice, a.Status    
	 from CRMProject a left join CRMCompany com on com.TicketId=a.CRMCompanyLookup
	 where a.TenantID=@TenantID and a.Closed=@includeClosed 
	  Union all      
	  Select o.TicketId,o.Title,o.ERPJobID,o.ERPJobIDNC,o.CRMCompanyLookup,com.Title as CRMCompanyTitleLookup ,o.Closed,    
	  o.PreconStartDate,o.PreconEndDate,o.EstimatedConstructionStart,o.EstimatedConstructionEnd,o.CloseoutDate,o.CloseoutStartDate,    
	  o.Description,o.RequestTypeLookup,o.ApproxContractValue,o.CRMProjectComplexityChoice, o.Status    
	 from Opportunity o left join CRMCompany com on com.TicketId=o.CRMCompanyLookup 
	 where o.TenantID=@TenantID  and o.Closed=@includeClosed
	 Union all      
	  Select s.TicketId,s.Title,s.ERPJobID,s.ERPJobIDNC,s.CRMCompanyLookup,com.Title as CRMCompanyTitleLookup ,s.Closed,    
	  s.PreconStartDate,s.PreconEndDate,s.EstimatedConstructionStart,s.EstimatedConstructionEnd,s.CloseoutDate,s.CloseoutStartDate,    
	  s.Description,s.RequestTypeLookup,s.ApproxContractValue,s.CRMProjectComplexityChoice, s.Status    
	 from CRMServices s left join CRMCompany com on com.TicketId=s.CRMCompanyLookup     
	 where s.TenantID=@TenantID and s.Closed=@includeClosed   
 
 End

 End


ALTER Procedure [dbo].[Usp_GetRptCombinedReport]    
--Usp_GetRptCombinedReport '35525396-e5fe-4692-9239-4df9305b915b','3/1/2021 00:00:00','3/28/2022 23:59:00','true'    
@TenantID nvarchar(256),--='35525396-e5fe-4692-9239-4df9305b915b',    
@StartDate  datetime,--='3/1/2021 00:00:00',    
@EndDate  datetime,--='3/28/2022 23:59:00'    
@JobSummary bit    
as    
    
begin    
    
DECLARE @SQLStatement NVARCHAR(MAX) = N''    
Declare @SQLOPM VARCHAR(MAX)  
Declare @SQLCPR VARCHAR(MAX)  
Declare @SQLCNS VARCHAR(MAX)  
  
IF(@JobSummary='True')  
BEGIN  
  
	SET @SQLOPM='Select     
		(Case when ProjectId is null or ProjectId =''''  then TicketId else ProjectId End )ProjectId,ERPJobID,ERPJobIDNC,TicketId,Title,
		ApproxContractValue,dbo.fnGetusername(OwnerUser,'''+@TenantID+''')OwnerUser,AwardedorLossDate,  
		CloseDate,Reason,DivisionLookup,CreationDate,dbo.fnGetusername(EstimatorUser,'''+@TenantID+''')EstimatorUser,  
		CRMOpportunityStatusChoice CRMProjectStatusChoice ,StudioLookup,Comment    
		from Opportunity where  TenantID = '''+@TenantID+''' '  
  
	SET @SQLCPR= 'Select (Case when ProjectId is null or ProjectId =''''  then TicketId else ProjectId End )ProjectId,ERPJobID,ERPJobIDNC,
	TicketId,Title,ApproxContractValue,dbo.fnGetusername(OwnerUser,'''+@TenantID+''')OwnerUser,AwardedorLossDate,  
		CloseDate,Reason,DivisionLookup,CreationDate,dbo.fnGetusername(EstimatorUser,'''+@TenantID+''')EstimatorUser,  
		CRMProjectStatusChoice ,StudioLookup ,Comment    
		from CRMProject where  TenantID = '''+@TenantID+''' '  
    
	SET @SQLCNS= 'Select (Case when ProjectId is null or ProjectId =''''  then TicketId else ProjectId End )ProjectId,ERPJobID,ERPJobIDNC,
	TicketId,Title,ApproxContractValue,dbo.fnGetusername(OwnerUser,'''+@TenantID+''')OwnerUser,AwardedorLossDate,  
		CloseDate,Reason,DivisionLookup,CreationDate,  
		dbo.fnGetusername(EstimatorUser,'''+@TenantID+''')EstimatorUser,CRMProjectStatusChoice ,StudioLookup ,Comment    
		from CRMServices where TenantID = '''+@TenantID+''' '  
  
	SET @SQLStatement=  @SQLOPM +' Union all '+ @SQLCPR +' Union all '+ @SQLCNS  
	SET @SQLStatement = 'Select a.* from ( ' +  @SQLStatement  + ' ) a '   
	If(@StartDate<>'' and @EndDate<>'')  
	begin  
		SET @SQLStatement= @SQLStatement + ' Where a.AwardedorLossDate >= ''' + Convert(varchar(19), @StartDate, 121) + ''' and a.AwardedorLossDate <=''' + Convert(varchar(19), @EndDate, 121)+''''  
	end  
  
	If(@StartDate<>'' and @EndDate ='' )  
	begin  
		SET @SQLStatement= @SQLStatement + ' Where a.AwardedorLossDate >= ''' + Convert(varchar(19), @StartDate, 121) + ''''  
	end  
	If(@StartDate='' and @EndDate <>'')  
	begin  
		SET @SQLStatement= @SQLStatement + ' Where a.AwardedorLossDate <= ''' + Convert(varchar(19), @EndDate, 121) + ''''  
	end  
  
	PRINT(@SQLStatement)  
	EXEC(@SQLStatement)   
  
END  
ELSE BEGIN  
	SET @SQLOPM='Select     
		(Case when ProjectId is null or ProjectId =''''  then TicketId else ProjectId End )ProjectId,ERPJobID,ERPJobIDNC,TicketId,Title,
		ApproxContractValue,dbo.fnGetusername(OwnerUser,'''+@TenantID+''')OwnerUser,AwardedorLossDate,  
		CloseDate,Reason,DivisionLookup,CreationDate,dbo.fnGetusername(EstimatorUser,'''+@TenantID+''')EstimatorUser,  
		CRMOpportunityStatusChoice CRMProjectStatusChoice ,StudioLookup,Comment    
		from Opportunity where  TenantID = '''+@TenantID+''' and CRMOpportunityStatusChoice=''Lost'' and Closed=1'  
  
	SET @SQLCPR= 'Select (Case when ProjectId is null or ProjectId =''''  then TicketId else ProjectId End )ProjectId,ERPJobID,ERPJobIDNC,
	TicketId,Title,ApproxContractValue,dbo.fnGetusername(OwnerUser,'''+@TenantID+''')OwnerUser,AwardedorLossDate,  
		CloseDate,Reason,DivisionLookup,CreationDate,dbo.fnGetusername(EstimatorUser,'''+@TenantID+''')EstimatorUser,  
		CRMProjectStatusChoice ,StudioLookup ,Comment    
		from CRMProject where  TenantID = '''+@TenantID+''' and CRMProjectStatusChoice=''Lost''  '  
    
	SET @SQLCNS= 'Select (Case when ProjectId is null or ProjectId =''''  then TicketId else ProjectId End )ProjectId,ERPJobID,ERPJobIDNC,
	TicketId,Title,ApproxContractValue,dbo.fnGetusername(OwnerUser,'''+@TenantID+''')OwnerUser,AwardedorLossDate,  
		CloseDate,Reason,DivisionLookup,CreationDate,  
		dbo.fnGetusername(EstimatorUser,'''+@TenantID+''')EstimatorUser,CRMProjectStatusChoice ,StudioLookup ,Comment    
		from CRMServices where TenantID = '''+@TenantID+''' and CRMProjectStatusChoice=''Lost''   '  
  
	SET @SQLStatement=  @SQLOPM +' Union all '+ @SQLCPR +' Union all '+ @SQLCNS  
	SET @SQLStatement = 'Select a.* from ( ' +  @SQLStatement  + ' ) a '   
	If(@StartDate<>'' and @EndDate<>'')  
	begin  
		SET @SQLStatement= @SQLStatement + ' Where a.AwardedorLossDate >= ''' + Convert(varchar(19), @StartDate, 121) + ''' and a.AwardedorLossDate <=''' + Convert(varchar(19), @EndDate, 121)+''''  
	end  
  
	If(@StartDate<>'' and @EndDate ='' )  
	begin  
		SET @SQLStatement= @SQLStatement + ' Where a.AwardedorLossDate >= ''' + Convert(varchar(19), @StartDate, 121) + ''''  
	end  
	If(@StartDate='' and @EndDate <>'')  
	begin  
		SET @SQLStatement= @SQLStatement + ' Where a.AwardedorLossDate <= ''' + Convert(varchar(19), @EndDate, 121) + ''''  
	end  
  
	PRINT(@SQLStatement)  
	EXEC(@SQLStatement)   
END  
    
end
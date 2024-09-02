  
ALTER procedure [dbo].[usp_GetSectorStudioDivisionData]      
-- [usp_GetSectorStudioDivisionData] 'f6846222-04eb-445f-bf95-e1636e089e76', ''    
(      
@TenantId varchar(128)= '', --'BCD2D0C9-9947-4A0B-9FBF-73EA61035069',        
@DataRequiredFor varchar(100) = ''  
)    
AS    
BEGIN    
 if OBJECT_ID('tempdb..#tmpProjects') is not null drop table #tmpProjects      
 create table #tmpProjects      
 (      
  TicketId varchar(100),      
  SectorChoice varchar(250),    
  DivisionLookup bigint,    
  StudioLookup bigint,    
 )      
    
 Insert into #tmpProjects    
	 Select TicketId, SectorChoice, DivisionLookup, StudioLookup    
		 FROM CRMProject     
		 WHERE TenantId = @TenantId  
	 UNION all
	 Select TicketId, SectorChoice, DivisionLookup, StudioLookup    
		 FROM Opportunity    
		 WHERE TenantId = @TenantId     
  
   
 if(@DataRequiredFor = 'sector' OR @DataRequiredFor = '')    
 BEGIN    
  SELECT SectorChoice Title, COUNT(TicketId) Counts    
	  FROM #tmpProjects    
	  WHERE LEN(ISNULL(SectorChoice,'')) > 0    
	  GROUP BY SectorChoice    HAVING COUNT(TicketId) > 0   ORDER BY Title    
 END    
    
 if(@DataRequiredFor = 'division' OR @DataRequiredFor = '')    
 BEGIN    
  SELECT     
  DivisionLookup ID ,    
  (SELECT TITLE FROM CompanyDivisions WHERE TenantID=@TenantId and ID = DivisionLookup ) Title,   COUNT(TicketId) Counts   
  FROM CRMPROJECT    
  WHERE TenantID=@TenantId and ISNULL(DivisionLookup,0) >0    
  GROUP BY DivisionLookup HAVING COUNT(TicketId) > 0  ORDER BY Title    
 END  
   
 if(@DataRequiredFor = 'alldivisions' OR @DataRequiredFor = '')    
 BEGIN    
  SELECT ID,  Title, ShortName from CompanyDivisions where TenantID=@TenantId and Deleted=0  
  order by Title  
 END    
   
 if(@DataRequiredFor = 'studio' OR @DataRequiredFor = '')    
 BEGIN    
  SELECT     
  StudioLookup ID,    
  (SELECT TITLE FROM Studio WHERE TenantID=@TenantId and ID = StudioLookup ) Title,    
  (SELECT DivisionLookup FROM Studio WHERE TenantID=@TenantId and ID = StudioLookup) DivisionLookup, COUNT(TicketId) Counts   
  FROM CRMPROJECT   WHERE TenantID=@TenantId and ISNULL(StudioLookup,0) >0    
  GROUP BY StudioLookup   HAVING COUNT(TicketId) > 0    ORDER BY Title    
 END    
    
  if OBJECT_ID('tempdb..#tmpProjects') is not null drop table #tmpProjects      
    
END  
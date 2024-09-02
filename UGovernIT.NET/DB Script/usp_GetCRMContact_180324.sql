/****** Object:  StoredProcedure [dbo].[usp_GetCRMContact]    Script Date: 3/18/2024 9:01:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER Procedure [dbo].[usp_GetCRMContact]       
--[usp_GetCRMContact] '35525396-e5fe-4692-9239-4df9305b915b'        
@TenantID VARCHAR(128),        
@IsClosed char(1)='',        
@TicketId VARCHAR(50)=''        
as       
Begin    
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'CON')

SET @strsql='Select (SELECT COUNT(*) FROM CRMProject WHERE ContactLookup=a.TicketID     
  AND Closed=0) OpenProjectCount,
  (SELECT COUNT(*) FROM CRMProject WHERE ContactLookup=a.TicketID AND Closed=1) CloseProjectCount,'+ @AllColumnsList+',(SELECT Title FROM CRMCompany WHERE TicketID=CRMCompanyLookup) AS CRMCompanyTitle from CRMContact (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''
and (a.Deleted<>1 or a.Deleted is null)'
IF(LEN(@IsClosed)>0)
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'
IF(len(@TicketId)>0)
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'
EXEC(@strsql)
--Print(@strsql)
END


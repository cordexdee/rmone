USE [core_staging]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetCRMProject]    Script Date: 2/8/2024 6:30:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER Procedure [dbo].[usp_GetCRMProject]         
--[usp_GetCRMProject] '35525396-e5fe-4692-9239-4df9305b915b'          
@TenantID VARCHAR(128),          
@IsClosed char(1)='',          
@TicketId VARCHAR(50)=''          
as         
Begin      
  
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''  
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'CPR')  
  
SET @strsql='Select [dbo].fnGetResourceAllocationCount(a.TicketId,'''+@TenantID+''') as ResourceAllocationCount, 
'+ @AllColumnsList+' from CRMProject (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''  
and (a.Deleted<>1 or a.Deleted is null)'  
IF(LEN(@IsClosed)>0)  
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'  
IF(len(@TicketId)>0)  
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'  
  
EXEC(@strsql)  
--Print(@strsql)  
END


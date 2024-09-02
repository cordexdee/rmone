ALTER Procedure [dbo].[usp_GetCRMServices]       
--[usp_GetCRMServices] '35525396-e5fe-4692-9239-4df9305b915b'        
@TenantID VARCHAR(128),        
@IsClosed char(1)='',        
@TicketId VARCHAR(50)=''        
as       
Begin    
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'CNS')
SET @strsql='Select [dbo].fnGetResourceAllocationCount(a.TicketId,'''+@TenantID+''') as ResourceAllocationCount, '+ @AllColumnsList+' from CRMServices (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''
and (a.Deleted<>1 or a.Deleted is null)'
IF(LEN(@IsClosed)>0)
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'
IF(len(@TicketId)>0)
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'

EXEC(@strsql)
--Print(@strsql)
END

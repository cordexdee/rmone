   
CREATE Procedure [dbo].[usp_GetNPR]    
--[usp_GetNPR] '32f5ad5b-fa7c-446f-9ce5-64c586ca13a9'
@TenantID VARCHAR(128),    
@IsClosed char(1)='',    
@TicketId VARCHAR(50)=''      
as    
Begin    
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'NPR')
SET @strsql='Select  '+ @AllColumnsList+' from NPR (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''
and (a.Deleted<>1 or a.Deleted is null)'
IF(LEN(@IsClosed)>0)
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'
IF(len(@TicketId)>0)
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'

EXEC(@strsql)
--Print(@strsql)
END
 
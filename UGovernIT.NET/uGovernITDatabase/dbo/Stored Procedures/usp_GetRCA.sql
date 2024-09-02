CREATE Procedure [dbo].[usp_GetRCA] 
--[usp_GetRCA] 'f442cdac-a01d-4400-9c03-8c77203f3ef7'
@TenantID VARCHAR(MAX),  
@IsClosed char(1)='',  
@TicketId VARCHAR(MAX)=''   
as  
Begin  
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''  
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'RCA')  
SET @strsql='Select  '+ @AllColumnsList+' from RCA (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''  
and (a.Deleted<>1 or a.Deleted is null)'  
IF(LEN(@IsClosed)>0)  
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'  
IF(len(@TicketId)>0)  
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'  
  
EXEC(@strsql)  
--Print(@strsql)  
End  
  
  
  
  
  
  
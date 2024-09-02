CREATE Procedure [dbo].[usp_GetINC]
--[usp_GetINC] 'c345e784-aa08-420f-b11f-2753bbebfdd5' 
@TenantID VARCHAR(MAX),
@IsClosed char(1)='',
@TicketId VARCHAR(MAX)=''
as
Begin
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'INC')
SET @strsql='Select  '+ @AllColumnsList+' from INC (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''
and (a.Deleted<>1 or a.Deleted is null)'
IF(LEN(@IsClosed)>0)
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'
IF(len(@TicketId)>0)
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'
EXEC(@strsql)
--Print(@strsql)
END
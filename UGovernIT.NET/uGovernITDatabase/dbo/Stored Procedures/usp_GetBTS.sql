CREATE Procedure [dbo].[usp_GetBTS]
--[usp_GetBTS] 'C345E784-AA08-420F-B11F-2753BBEBFDD5'
@TenantID VARCHAR(MAX),
@IsClosed char(1)='',
@TicketId VARCHAR(MAX)=''
as
Begin
Declare @strsql VARCHAR(MAX), @AllColumnsList VARCHAR(MAX) = ''
SET @AllColumnsList= [dbo].[fnGetResolvedColumnsList](@TenantID,'BTS')
SET @strsql='Select  '+ @AllColumnsList+' from BTS (READCOMMITTED) a where a.TenantID='+ ''''+@TenantID+'''
and (a.Deleted<>1 or a.Deleted is null)'
IF(LEN(@IsClosed)>0)
SET @strsql=@strsql+' AND a.closed = CASE WHEN LEN('''+@IsClosed+''')=0 then a.closed else '''+@IsClosed+'''   END'
IF(len(@TicketId)>0)
SET @strsql=@strsql+ ' AND  isnull(a.TicketId,'''') =CASE WHEN LEN('''+@TicketId+''')=0 then isnull(a.TicketId,'''')  else '''+@TicketId+''' END'

EXEC(@strsql)
--Print(@strsql)
END

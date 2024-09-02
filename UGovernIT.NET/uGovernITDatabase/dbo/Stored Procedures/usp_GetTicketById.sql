CREATE Procedure [dbo].[usp_GetTicketById]   
--[usp_GetTicketById]  '12686', '','35525396-e5fe-4692-9239-4df9305b915b','CRMProject','*'  
@ID varchar(max)='0',  
@TicketId varchar(max)='',  
@TenantId varchar(max),  
@ModuleTable varchar(max),  
@ViewFields varchar(max)  
as begin  
Declare @SQL varchar(max)=''  
IF(LEN(@TicketId)>0)  
SET @SQL='Select TOP 1 '+@ViewFields+' from '+ @ModuleTable+ ' where tenantid='''+@TenantId+''' AND TicketId='''+ @TicketId+''''   
ELSE IF(@ID<>'0')  
SET @SQL='Select TOP 1 '+@ViewFields+' from '+ @ModuleTable+ ' where tenantid='''+@TenantId+''' AND ID='+ @ID   
PRINT(@SQL)  
EXEC(@SQL)  
End
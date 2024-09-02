CREATE Procedure Usp_GetTicketHours    
@TenantID varchar(256),    
@ModuleName varchar(256)='',    
@TicketID varchar(256)='',
@TaskID varchar(256)= ''
as    
begin    

Declare @SQLStatement NVARCHAR(MAX) = N''

SET @SQLStatement ='select ID, WorkDate, dbo.fnGetusername(ResourceUser,''' + @TenantID + ''')ResourceUser, HoursTaken, Comment 
 from TicketHours where TenantID=''' + @TenantID + ''''

if(Len(@ModuleName)>0 and Len(@TicketID)>0)
BEGIN
	SET @SQLStatement = @SQLStatement + ' and ModuleNameLookup=''' + @ModuleName +''' and TicketID= ''' + @TicketID + ''''
END

IF(Len(@TaskID) > 0)
BEGIN
	SET @SQLStatement = @SQLStatement + ' and TaskID = ''' + @TaskID + ''''
END

Print (@SQLStatement)  
EXEC (@SQLStatement)

end

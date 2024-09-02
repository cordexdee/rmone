Create Procedure [dbo].[USP_GetModuleTableData]   
--[USP_GetModuleTableData] '96b79a73-9339-4506-a12c-89a11474c82e','TSR','0'
@TenantID VARCHAR(MAX),  
@ModuleName VARCHAR(MAX),  
@IsClosed char(1)=''  
as  
BEGIN  
DECLARE @SQLStatement NVARCHAR(MAX) = N''    
 Declare @ProcName varchar(max)  
Declare @counter  int =1  
Declare @TBLNAME varchar(max)  
  
SET TRANSACTION ISOLATION LEVEL READ COMMITTED  
Create Table #tempModules  
(  
ID int, ModuleTable varchar(max)  
);  
Insert into #tempModules(ID, ModuleTable) Select ROW_NUMBER() OVER(ORDER BY ModuleTable), ModuleTable from Config_Modules   
where TenantID=@TenantID and EnableModule=1 and ModuleName = CASE WHEN LEN(@ModuleName)=0 then ModuleName else @ModuleName END  
WHILE @counter <= (Select count(1) from #tempModules)      
BEGIN    
SET @TBLNAME= (Select  ModuleTable from #tempModules where ID=@counter)  
SET @ProcName = 'usp_Get'+@TBLNAME  
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = @ProcName)  
BEGIN  
Exec @ProcName @TenantID,@IsClosed  
END  
ELSE  
begin
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME=@TBLNAME)
	Begin
		   
	SET @SQLStatement=' Select * from  ' + @TBLNAME +' Where TenantId='+ ''''+@TenantID+''''  
		IF(@IsClosed=1)  
		BEGIN  
		SET @SQLStatement=@SQLStatement + 'AND Closed=1';  
		END
	END

end
  
PRINT(@SQLStatement)  
EXEC(@SQLStatement)  
SET @counter = @counter + 1;    
END  
END  
DROP TABLE #tempModules;
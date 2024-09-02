Create FUNCTION [dbo].[split_string]
(
    @in_string VARCHAR(MAX),
    @delimeter VARCHAR(1)
)
RETURNS @list TABLE(tuple VARCHAR(100))
AS
BEGIN
        WHILE LEN(@in_string) > 0
        BEGIN
            INSERT INTO @list(tuple)
            SELECT Replace(Replace(left(Trim(@in_string), charindex(@delimeter, @in_string+',') -1),CHAR(13),''),CHAR(10),'') as tuple
    
            SET @in_string = Replace(stuff(@in_string, 1, charindex(@delimeter, @in_string + @delimeter), ''),' ','')
        end
    RETURN 
END

GO

Create Procedure [dbo].[GetAllTableList]
as begin
SELECT
TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
EXCEPT
SELECT * FROM split_string('__MigrationHistory,
__RefactorLog,ACR_Archive,Assets_Archive,BTS_Archive,CRMCompany_Archive,CRMContact_Archive,CRMProject_Archive,
CRMServices_Archive,DRQ_Archive,INC_Archive,Lead_Archive,ModuleWorkflowHistory_Archive,NPR_Archive,Opportunity_Archive,
PMM_Archive,PRS_Archive,RCA_Archive,SchedulerActionArchives,SVCRequests_Archive,TSK_Archive,TSR_Archive,UPR_Archive,TenantRegistration,TenantScheduler,__RefactorLog,sysdiagrams,ITGovernance,Company,
ETFactTable_c345e784-aa08-420f-b11f-2753bbebfdd5_Test_Data,ETFactTable_c345e784-aa08-420f-b11f-2753bbebfdd5_Test456
Config_TabView,Config_UserRoles,Contacts
Contracts,
EscalationLog,
ETFactTable_c345e784-aa08-420f-b11f-2753bbebfdd5_Test456,
FieldConfiguration_temp,
Log,
qa_FieldConfiguration,
Resources_1,
Roles,
 Schema,
 Server,
 ServiceUpdates_Master,
 Set,
 Sprint,ADUserMapping,' ,',')
end
  
  
      
CREATE Procedure [dbo].[Usp_setdefaultcofiguration]       
@TenantId uniqueidentifier, @AccountId nvarchar(100)      
as      
begin      
Declare @cntpc int=0 ,@cntvrb int=0, @cntmc int=0,@cntlp int=0;      
--Select @cntpc=(Select count(1) from Config_PageConfiguration where TenantID=@TenantId)      
Select @cntvrb=(Select count(1) from Config_ConfigurationVariable where TenantID=@TenantId)      
--Select @cntmc=(Select count(1) from Config_Module_ModuleColumns where TenantID=@TenantId)      
--Select @cntlp=(Select count(1) from LandingPages where TenantID=@TenantId)      
--Select @cntlp=(Select count(1) from LandingPages where TenantID=@TenantId)      
    
--if(@cntpc=0)      
--begin      
--INSERT INTO Config_PageConfiguration (Title ,      
--Name ,      
--LeftMenuName ,      
--LeftMenuType ,      
--HideLeftMenu ,      
--HideTopMenu ,      
--HideHeader ,      
--TopMenuName ,      
--TopMenuType ,      
--HideSearch ,      
--HideFooter ,      
--ControlInfo ,      
--LayoutInfo ,      
--TenantID ,      
--RootFolder ,      
--Created ,      
--Modified ,      
--CreatedByUser ,      
--ModifiedByUser ,      
--Deleted ,      
--Attachments       
--)      
--SELECT Title ,      
--Name ,      
--LeftMenuName ,      
--LeftMenuType ,      
--HideLeftMenu ,      
--HideTopMenu ,      
--HideHeader ,      
--TopMenuName ,      
--TopMenuType ,      
--HideSearch ,      
--HideFooter ,      
--ControlInfo ,      
--LayoutInfo ,      
--@TenantId,      
--RootFolder ,      
--Created ,      
--Modified ,      
--CreatedByUser ,      
--ModifiedByUser ,      
--Deleted ,      
--Attachments       
--FROM Config_PageConfiguration      
--WHERE TenantID= 'C345E784-AA08-420F-B11F-2753BBEBFDD5'      
--end      
if(@cntvrb=0)      
begin      
INSERT INTO Config_ConfigurationVariable (CategoryName,      
Description ,      
KeyName ,      
KeyValue ,      
Title ,      
Type ,      
Internal ,      
TenantID ,      
Created ,      
Modified ,      
CreatedByUser ,      
ModifiedByUser ,      
Deleted ,      
Attachments)       
      
Select CategoryName, Description ,      
KeyName ,      
KeyValue ,      
Title ,      
Type ,      
Internal ,      
@TenantId ,      
Created ,      
Modified ,      
CreatedByUser ,      
ModifiedByUser ,      
Deleted ,      
Attachments from Config_ConfigurationVariable   where TenantID= 'C345E784-AA08-420F-B11F-2753BBEBFDD5' and KeyName not in ('ModelSite','SmtpCredentials') and KeyName not in (      
select KeyName from Config_ConfigurationVariable where TenantID=@TenantId)      
end      
--if(@cntmc=0)      
--begin     

----INSERT INTO Config_Module_ModuleColumns (      
----ColumnType ,      
----CustomProperties ,      
----DisplayForClosed ,      
----DisplayForReport ,      
----FieldDisplayName ,      
----FieldName ,      
----FieldSequence ,      
----IsAscending ,      
----IsDisplay ,      
----IsUseInWildCard ,      
----ShowInMobile ,      
----SortOrder ,      
----TruncateTextTo ,      
----TextAlignmentChoice ,      
----Title ,      
----CategoryName,      
----TenantID,      
----Created ,      
----Modified ,      
----CreatedByUser ,      
----ModifiedByUser ,      
----Deleted ,      
----Attachments ,      
----SelectedTabs ,      
----ShowInCardView       
----)       
      
----Select ColumnType ,      
----CustomProperties ,      
----DisplayForClosed ,      
----DisplayForReport ,      
----FieldDisplayName ,      
----FieldName ,      
----FieldSequence ,      
----IsAscending ,      
----IsDisplay ,      
----IsUseInWildCard ,      
----ShowInMobile ,      
----SortOrder ,      
----TruncateTextTo ,      
----TextAlignmentChoice ,      
----Title ,      
----CategoryName ,      
----@TenantID ,      
----Created ,      
----Modified ,      
----CreatedByUser ,      
----ModifiedByUser ,      
----Deleted ,      
----Attachments ,      
----SelectedTabs ,      
----ShowInCardView       
----from Config_Module_ModuleColumns   where TenantID= 'C345E784-AA08-420F-B11F-2753BBEBFDD5'      
--end      
--Exec Update_default_tenant_KeyValue @TenantId, @AccountId      
      
Update Config_ConfigurationVariable set  KeyValue=(Select KeyValue from Config_ConfigurationVariable where TenantID='C345E784-AA08-420F-B11F-2753BBEBFDD5' and KeyName='CompanyLogo') ,      
Title=(Select KeyValue from Config_ConfigurationVariable where TenantID='C345E784-AA08-420F-B11F-2753BBEBFDD5' and KeyName='CompanyLogo')      
where TenantID=@TenantId and KeyName='CompanyLogo'      
      
Update Config_ConfigurationVariable set  KeyValue=(Select KeyValue from Config_ConfigurationVariable where TenantID='C345E784-AA08-420F-B11F-2753BBEBFDD5' and KeyName='UgitTheme') ,      
Title=(Select KeyValue from Config_ConfigurationVariable where TenantID='C345E784-AA08-420F-B11F-2753BBEBFDD5' and KeyName='UgitTheme')      
where TenantID=@TenantId and KeyName='UgitTheme'      
exec usp_migration_CreateSuperAdmin @TenantId      
--if(@cntlp=0)      
--begin      
--INSERT INTO LandingPages(       
--Name,      
--Description,      
--LandingPage,      
--TenantID,      
--Created,      
--Modified,      
--CreatedByUser,      
--ModifiedByUser,      
--Deleted,      
--Attachments      
--)      
--SELECT Name,      
--Description,      
--LandingPage,      
--@TenantId,      
--Created,      
--Modified,      
--CreatedByUser,      
--ModifiedByUser,      
--Deleted,      
--Attachments      
--FROM LandingPages      
--WHERE TenantID= 'C345E784-AA08-420F-B11F-2753BBEBFDD5'      
--end      
      
 update ModuleTasks set AssignToPct=[dbo].[ReturnAssignedToFormat](REPLACE(AssignToPct,'i:0#.w|ugovernit\',''),@TenantId)  
   where AssignToPct is not null and TenantID=@TenantId and AssignToPct like '%i:0#.w|ugovernit\%'  
  
end
--changes made by Munna on 06/02/2017
use uGovernIT
alter table AssetVendors
add Vendortype Int

alter table AssetVendors
add ProductServiceDesc varchar(500) 


alter table AssetVendors
add VendorTimeZone varchar(3)


alter table AssetVendors
add VendorSupportHours varchar(100)
alter table AssetVendors
add VendorSupportCredentials varchar(100)

alter table AssetVendors
add VendorAccountRepPhone varchar(100)
alter table AssetVendors
add VendorAccountRepEmail varchar(100)
alter table AssetVendors
add VendorAccountRepMobile varchar(100)


--MK:2/23/2017
sp_rename 'ModuleUserStatistics.ModuleId','ModuleName'
alter table ModuleUserStatistics alter column ModuleName nvarchar(10)
alter table ModuleUserStatistics add UserName nvarchar(250)

--MS:03/02/2017
ALTER TABLE Config_ConfigurationVariable
ADD [Type][nvarchar](100) NULL




--MS:03/02/2017
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Config_ConfigurationVariable ADD
	Internal bit NOT NULL CONSTRAINT DF_Config_ConfigurationVariable_Internal DEFAULT (0)
GO
ALTER TABLE dbo.Config_ConfigurationVariable SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



--MK: 03/06/2017
alter table config_tabview drop column TabId
alter table config_tabview add TabId bigint identity(1,1)

--ms :03/14/2017
ALTER TABLE Config_Module_RequestType
ADD [EmailToTicketSender][nvarchar](max) NULL



--MK: 03/16/2017
alter table Config_Module_ModuleStages alter column stageapprovedStatus int 
alter table config_module_modulestages alter column stagereturnstatus int
alter table config_module_modulestages alter column stagerejectedstatus int

--MK: 03/17/2013
alter table config_module_modulestages drop constraint FK__Config_Mo__Stage__3F9B6DFF
sp_rename 'config_module_modulestages.StageTypeLookup','StageType'
alter table config_module_modulestages  alter column StageType nvarchar(250)
ALTER TABLE dbo.Config_Modules ADD AutoRefreshListFrequency int NULL;


--MK: 02/11/2020
ALTER TABLE [dbo].[Config_Modules] DROP COLUMN [AllowActualHoursByUser];


--MK: 02/13/2020
--Below db queries  for Document Management
Alter Table DMSDocument
ADD [Description] [nvarchar](255) NULL,
    [DocumentControlID] [nvarchar](255) NULL,
    [DocumentType] [nvarchar](255) NULL,
    [ReviewRequired] [bit] NULL,
    [ReviewStep] [float] NULL,
    [Tags] [nvarchar](255) NULL,
    [Title] [nvarchar](255) NULL,
    [UserDocumentVersion] [nvarchar](255) NULL,
    [DataMigrationID] [int] NULL


    

Alter Table [DMSDirectory]
    ADD [Deleted] [bit] NOT NULL DEFAULT(0)

ALTER TABLE ASPNETUSERS
ADD  IsDefaultAdmin int Default(0);

--MK: 03/05/2020
Alter table Config_Module_ModuleColumns add TruncateTextTo int default(0) null
update Config_Module_ModuleColumns set TruncateTextTo=0


--MK: 05/27/2021
alter table PMM alter column ProjectComplexityChoice nvarchar(max) null


--MK: 07/15/2021
alter table modulebudgetactuals add BudgetItem  NVARCHAR (250)  NULL
alter table modulebudgetactuals add DepartmentLookup bigint null
alter table modulebudgetactuals add foreign key (DepartmentLookup) REFERENCES [Department](ID)



--MK:7/1/2022
Alter table ResourceUsageSummaryMonthWise add GlobalRoleID nvarchar(128) NULL
Alter table ResourceUsageSummaryWeekWise add GlobalRoleID nvarchar(128) NULL


--MK:5/12/2022
Alter table ResourceAllocation add SoftAllocation bit default((0))
Alter table projectestimatedallocation add SoftAllocation bit default((0))
update ProjectEstimatedAllocation set SoftAllocation = 0


--MK:12/23/2022
Alter table CRMServices add PreconEndDate Datetime null

--MK:09/03/2023
Alter table ResourceAllocation add IsNCO bit default((0))
Alter table projectestimatedallocation add IsNCO bit default((0))
update ProjectEstimatedAllocation set IsNCO = 0
update ResourceAllocation set IsNCO = 0
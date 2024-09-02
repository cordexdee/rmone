/*
1. CloseoutStartDate (OPM, CPR, CNS)
2. ForecastedProjectCost(OPM, CPR, CNS)
3. ActualProjectCost(OPM, CPR, CNS)
4. ActualAcquisitionCost(OPM, CPR, CNS)
5. ERPJobID, ERPJobIDNC, ProjectId, ProjectName -(OPM, CPR, CNS)
6. LeadStatus(LEM)
7. AwardedorLossDate(OPM)
8.SectorChoice(LEM)
9.CRMProjectStatusChoice(CPR)
*/

/*
1. PRPGroupUser (RCA)
2. ServiceLookUp(Column Update for SVC)
3.ActualStartDate,ActualCompletionDate,ProjectSummaryNote,ProjectScore,ProjectManagerUser (NPR)
4. RequestTypeCategory(TSR)

*/
/*Added by Inderjeet Kaur
BusinessManagerUser added for ACR 
SecurityManagerUser,DRQRapidTypeLookup  added for DRQ
ResolvedByUser in TSR
EstProjectSpendComment, EstProjectSpend,ProjectMonitors in PMM
TotalCost,ProjectCost in NPR
*/
GO

/*
CREATE TABLE [dbo].[Temp_ModuleColumns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FieldName] [nvarchar](200) NOT NULL,
	[ModuleName] [nvarchar](200) NOT NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[Temp_ModuleColumns]
           ([FieldName],[ModuleName])
     VALUES
			('EstProjectSpendComment','PMM'),
			('EstProjectSpend','PMM'),
			('ProjectMonitors','PMM'),
			('TotalCost','NPR'),
			('ProjectCost','NPR'),
			('BusinessManagerUser','ACR'),
			('SecurityManagerUser','DRQ'),
			('ResolvedByUser','TSR'),
			('ResolvedByUser','BTS'),
			('PriorityLookup','CMDB'),
			('DRQRapidTypeLookup','DRQ'),
			('CloseoutStartDate', 'OPM'),
			('CloseoutStartDate', 'CPR'),
			('CloseoutStartDate', 'CNS'),
			('ForecastedProjectCost', 'OPM'),
			('ForecastedProjectCost', 'CPR'),
			('ForecastedProjectCost', 'CNS'),
			('ActualProjectCost', 'OPM'),
			('ActualProjectCost', 'CPR'),
			('ActualProjectCost', 'CNS'),
			('ActualAcquisitionCost', 'OPM'),
			('ActualAcquisitionCost', 'CPR'),
			('ActualAcquisitionCost', 'CNS'),
			('ERPJobID','OPM'),
			('ERPJobIDNC','OPM'),
			('ProjectId','OPM'),
			('ProjectName','OPM'),
			('ERPJobID','CPR'),
			('ERPJobIDNC','CPR'),
			('ProjectId','CPR'),
			('ProjectName','CPR'),
			('ERPJobID','CNS'),
			('ERPJobIDNC','CNS'),
			('ProjectId','CNS'),
			('ProjectName','CNS'),
			('LeadStatus','LEM'),
			('AwardedorLossDate','OPM'),
			('SectorChoice','LEM'),
			('CRMProjectStatusChoice','CPR'),
			('PRPGroupUser','RCA'),
			('ServiceLookUp','SVC'),
			('ActualStartDate','NPR'),
			('ActualCompletionDate','NPR'),
			('ProjectSummaryNote','NPR'),
			('ProjectScore','NPR'),
			('ProjectManagerUser','NPR'),
			('ProjectSummaryNote','TSK'),
			('IconBlob','PMM'),
			('IconBlob','TSR'), -- 17 aug 2023
			('RiskScore','PMM'), -- 18 aug 2023 
			('ProjectScore','PMM'), -- 18 aug 2023 
			('RequestTypeCategory','TSR')

GO
Select * from Temp_ModuleColumns
GO
--DROP TABLE [dbo].[Temp_ModuleColumns]
*/

Declare @totaltenants int=0  
Declare @totalFields int=0  
Declare @ModuleName nvarchar(100) --='PMM' 
--Declare @FieldNames varchar(1000)='EstProjectSpendComment,EstProjectSpend,ProjectMonitors'
Declare @TenantID nvarchar(100)
DEclare @FieldSequence int=0
Declare @i as integer
Declare @j as integer
DECLARE @TenantTable TABLE (Id int Identity, TenantId NVARCHAR(MAX))  
DECLARE @FieldName varchar(100)


INSERT INTO @TenantTable (TenantId)  
Select TenantID from qa_ugovernit_common_16Aug..tenant where Deleted=0  -------------------- Change the DB name
Set @totaltenants= (Select Count(1) from @TenantTable)  
Set @i=1 

Set @totalFields= (Select Count(1) from Temp_ModuleColumns)  

while @i <= @totaltenants  
begin  
	Set @j=1 
	Select @TenantID= TenantID from @TenantTable where Id = @i  

	WHILE @j <= @totalFields  
	BEGIN 

		Select @ModuleName = ModuleName from Temp_ModuleColumns where Id = @j  
		Select @FieldName = FieldName from Temp_ModuleColumns where Id = @j  
	
		IF NOT EXISTS (SELECT 1 from Config_Module_ModuleColumns WHERE FieldName = @FieldName and CategoryName=@ModuleName AND TenantID = @TenantID)
			BEGIN
			(Select @FieldSequence= ISNULL(Max(FieldSequence), 0)+1 from Config_Module_ModuleColumns where CategoryName=@ModuleName AND TenantID = @TenantID)
				INSERT INTO [dbo].[Config_Module_ModuleColumns]
					([ColumnType]
					,[CustomProperties]
					,[DisplayForClosed]
					,[DisplayForReport]
					,[FieldDisplayName]
					,[FieldName]
					,[FieldSequence]
					,[IsAscending]
					,[IsDisplay]
					,[IsUseInWildCard]
					,[ShowInMobile]
					,[SortOrder]
					,[TruncateTextTo]
					,[TextAlignmentChoice]
					,[Title]
					,[CategoryName]
					,[TenantID]
					,[Created]
					,[Modified]
					,[CreatedByUser]
					,[ModifiedByUser]
					,[Deleted]
					,[Attachments]
					,[SelectedTabs]
					,[ShowInCardView])
				VALUES
					(''
					,''
					,NULL
					,0
					,@FieldName
					,@FieldName
					,@FieldSequence
					,NULL
					,0
					,0
					,0
					,NULL
					,0
					,'Center'
					,@ModuleName +' - '+ @FieldName
					,@ModuleName
					,@TenantID
					,GETDATE()
					,GETDATE()
					,'00000000-0000-0000-0000-000000000000'
					,'00000000-0000-0000-0000-000000000000' 
					,0
					,NULL
					,'all'
					,0)
				PRINT(@ModuleName +' - '+ @FieldName +' added for Tenant Id: ' + @TenantID)
			END
			ELSE
			PRINT(@ModuleName +' - '+ @FieldName +' NOT added for Tenant Id: ' + @TenantID) 

		Set @j = @j + 1 -- Go for the next field record in the modulecolumn table  

	END

	Set @i = @i + 1 -- Go for the next tenant in the table  
End

GO

-- DROP TABLE [dbo].[Temp_ModuleColumns]
--select * from Config_Module_ModuleColumns order by Created desc

ALTER FUNCTION [dbo].[fnGetModule_Columns]
(
    @TenantID varchar(128),
	@ModuleNameLookup varchar(50)
)
RETURNS @ColumnsTable TABLE (Value NVARCHAR(MAX))
AS
BEGIN
DECLARE @TabelName NVARCHAR(MAX)='',
@DefaultColumns NVARCHAR(MAX) ='ID,TicketId, StageStep, Title,Closed,Status, ModuleStepLookup, WorkflowSkipStages, StageActionUsersUser, StageActionUserTypes,CreationDate, Created, Modified,CreatedByUser, ModifiedByUser, CloseDate,CloseoutDate,CloseoutStartDate, OnHold, TenantID, TagMultiLookup'
Select @TabelName=ModuleTable  from Config_Modules where TenantID=@TenantID and ModuleName=@ModuleNameLookup

If(@ModuleNameLookup='DashboardSummary')
Begin
	INSERT INTO @ColumnsTable (Value)
		SELECT COLUMN_NAME
		FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'DashboardSummary'
End
Else Begin
INSERT INTO @ColumnsTable (Value)
SELECT COLUMN_NAME
		FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME =@TabelName
		and COLUMN_NAME in (
SELECT DISTINCT FieldName
    FROM Config_Module_ModuleColumns
    WHERE TenantID = @TenantID
	and CategoryName=@ModuleNameLookup
Union all
SELECT Trim(Value)Value
        FROM STRING_SPLIT(@DefaultColumns, ',')
	)


    --INSERT INTO @ColumnsTable (Value)
    --SELECT DISTINCT FieldName
    --FROM Config_Module_FormLayout
    --WHERE TenantID = @TenantID
    --    AND FieldName NOT IN ('Owner1','#GroupEnd#Hide','#PlaceHolder#', '#GroupStart#Hide', '#GroupEnd1#Hide', '#GroupStart#', '#GroupEnd#', '#Control#')
    --    AND ModuleNameLookup = @ModuleNameLookup
		
--;WITH CTE_Values AS (
--        SELECT Trim(Value)Value
--        FROM STRING_SPLIT(@DefaultColumns, ',')
--    )
--	INSERT INTO @ColumnsTable (Value)
--	SELECT Value FROM CTE_Values WHERE Value NOT IN (SELECT Value FROM @ColumnsTable);
End
	
    RETURN
END
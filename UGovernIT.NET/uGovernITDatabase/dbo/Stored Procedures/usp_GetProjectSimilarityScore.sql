Alter procedure usp_GetProjectSimilarityScore
@ModuleName nvarchar(5) = 'CPR',
@TenantID nvarchar(128) = '35525396-E5FE-4692-9239-4DF9305B915B'
as
begin
	DECLARE @ScoreSum DECIMAL(18, 2);
	DECLARE @WeightSum DECIMAL(18, 2);

	CREATE TABLE #TempResult (
    ModuleNameLookup NVARCHAR(MAX),
    ScoreSum DECIMAL(18, 2),
    WeightSum DECIMAL(18, 2));

	INSERT INTO #TempResult (ModuleNameLookup, ScoreSum, WeightSum)
	SELECT ModuleNameLookup, SUM(StageWeight) AS ScoreSum, SUM(Weight) AS WeightSum
	FROM ProjectSimilarityConfig where TenantID=@TenantID
	GROUP BY ModuleNameLookup;

	SELECT @ScoreSum = ScoreSum, @WeightSum = WeightSum
	FROM #TempResult
	WHERE ModuleNameLookup = @ModuleName;

	select ModuleNameLookup, StageWeight, Weight, ColumnName, Title, ColumnType,
	FORMAT((CAST(StageWeight AS DECIMAL(18, 2))/@ScoreSum),'N2') NormalizeWeight,  
	FORMAT((StageWeight * (CAST(StageWeight AS DECIMAL(18, 2))/@ScoreSum) * 5),'N2') WeightedScore,
	FORMAT((CAST(Weight AS DECIMAL(18, 2))/@WeightSum)*100,'N2') as NormalizedScore from ProjectSimilarityConfig
	where ModuleNameLookup = @ModuleName and TenantID = @TenantID

	Drop table #TempResult;
End

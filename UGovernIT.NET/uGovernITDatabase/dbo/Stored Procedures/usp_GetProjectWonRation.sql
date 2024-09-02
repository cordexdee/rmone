CREATE PROCEDURE [dbo].[usp_GetProjectWonRation]   
	@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B',
	@Fromdate datetime='2021-01-01 00:00:00.000',
	@Todate datetime='2023-02-01 00:00:00.000' 
AS
BEGIN
	
	SET NOCOUNT ON;
	declare @ProjectWon float;
	declare @TotalProject float;
	declare @LastMonthProjectCount int;
	declare @AverageMonthlyProjectCount int;

	SELECT @ProjectWon = COUNT(*) FROM CRMProject where Created between @Fromdate and @Todate and Closed = 0
	SELECT @TotalProject = (@ProjectWon + (SELECT COUNT(*) from Opportunity where Created between @Fromdate and @Todate and Closed = 0))
	print(@ProjectWon)
	print(@TotalProject)
	print(@ProjectWon/@TotalProject)
	SELECT FORMAT(@ProjectWon/@TotalProject * 100, 'N2') as ProjectWonRation
END

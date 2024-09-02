CREATE PROCEDURE [dbo].[usp_GetOpportunityAndProjectCount]   
	@TenantID varchar(128)= '35525396-E5FE-4692-9239-4DF9305B915B',
	@Division int = 0
AS
BEGIN

	SET NOCOUNT ON;
	declare @LastMonthOpportunityCount int;
	declare @AverageMonthlyOpportunityCount int;
	declare @LastMonthProjectCount int;
	declare @AverageMonthlyProjectCount int;

	SELECT @LastMonthOpportunityCount = COUNT(*) from Opportunity where Created between DATEADD(month,-1, GETDATE()) and GETDATE() and TenantID = @TenantID and Closed = 0
	SELECT @AverageMonthlyOpportunityCount = COUNT(*)/11 from Opportunity where Created between DATEADD(month,-12, GETDATE()) and DATEADD(month,-1, GETDATE()) and TenantID = @TenantID and Closed = 0
	
	Select  @LastMonthProjectCount =  (select count(*) from CRMServices where Created between DATEADD(month,-1, GETDATE()) and DATEADD(day,1,GETDATE()) and TenantID = @TenantID and Closed = 0) +
	(select count(*) from CRMProject where Created between DATEADD(month,-1, GETDATE()) and DATEADD(day,1,GETDATE())  and TenantID = @TenantID and Closed = 0)

	Select  @AverageMonthlyProjectCount =  (select count(*) from CRMServices where Created between DATEADD(month,-12, GETDATE()) and DATEADD(month,-1, GETDATE()) and TenantID = @TenantID and Closed = 0) +
	(select count(*) from CRMProject where Created between DATEADD(month,-12, GETDATE()) and DATEADD(month,-1, GETDATE()) and TenantID = @TenantID and Closed = 0)
	

	print(@LastMonthOpportunityCount)
	print(@AverageMonthlyOpportunityCount)
	print(@LastMonthProjectCount)
	print(@AverageMonthlyProjectCount)
	select @LastMonthOpportunityCount LastMonthOpportunityCount, @AverageMonthlyOpportunityCount AverageMonthlyOpportunityCount, @LastMonthProjectCount LastMonthProjectCount, @AverageMonthlyProjectCount AverageMonthlyProjectCount
END

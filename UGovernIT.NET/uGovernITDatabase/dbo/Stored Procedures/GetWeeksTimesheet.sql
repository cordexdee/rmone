CREATE PROCEDURE [dbo].[GetWeeksTimesheet]
	@TenantId nvarchar(128),
	@UserId nvarchar(200),
	@Date DateTime,
	@WorkItem nvarchar(100) = '',
	@SubWorkItem nvarchar(100) = ''
AS
BEGIN
	Declare @StartDate DateTime;
	Declare @EndDate DateTime;
	set @Date = DATEADD(DAY, -1, @Date);
	set @StartDate = DATEADD( DAY , 2 - DATEPART(WEEKDAY, @Date), CAST (@Date AS DATE ));
	set @EndDate = DATEADD( DAY , 8 - DATEPART(WEEKDAY, @Date), CAST (@Date AS DATE ));

	exec GetTimesheet @TenantId,@UserId,@StartDate,@EndDate,@WorkItem,@SubWorkItem
END
GO
CREATE FUNCTION [dbo].[fnGetWorkingDays]
(
	@StartDate DATETIME,
	@EndDate DATETIME
)
RETURNS INT
AS
BEGIN
	Declare @WorkingDays int;
	Declare @Holidays int;
	Set @Holidays = 0;

	  Set @WorkingDays = (DATEDIFF(dd, @StartDate, @EndDate) + 1)
		  -(DATEDIFF(wk, @StartDate, @EndDate) * 2)
		  -(CASE WHEN DATENAME(dw, @StartDate) = 'Sunday' THEN 1 ELSE 0 END)
		  -(CASE WHEN DATENAME(dw, @EndDate) = 'Saturday' THEN 1 ELSE 0 END);

	  select @Holidays = count(*) from Appointment where TenantID = '35525396-e5fe-4692-9239-4df9305b915b' 
		and DATENAME(dw, StartTime) NOT IN ('Saturday','Sunday')
		and StartTime between @StartDate and @EndDate	

	  RETURN (@WorkingDays - @Holidays);	  
END

-- select dbo.fnGetWorkingDays('2021-01-01','2021-12-31')
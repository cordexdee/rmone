
CREATE FUNCTION [dbo].[CheckDateRangeOverlap]
(
	@StartDate1 datetime = '',
	@EndDate1 datetime = '',
	@StartDate2 datetime = '',
	@EndDate2 datetime = ''
)
RETURNS bit
AS
BEGIN
	if(@StartDate1 != '' and @EndDate1 != '' and @StartDate2 != '' and @EndDate2 != '' )
	begin
		if(@StartDate1 <= @EndDate2 and @EndDate1 >= @StartDate2)
		begin
			return 1;
		end
	End
	return 0;
END
GO


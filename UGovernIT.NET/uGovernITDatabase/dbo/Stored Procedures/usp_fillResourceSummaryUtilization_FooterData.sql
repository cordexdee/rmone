CREATE PROCEDURE [dbo].[usp_fillResourceSummaryUtilization_FooterData]
@TenantId varchar(128)=''
as
begin

	declare @FromDate datetime, @ToDate datetime, @startType int = 1, @start int = 1, @currentDate datetime = getdate(), 
			@startYear int, @totalYears int = 6, @mode varchar(10) = 'Monthly', @IsAssignedallocation bit='False',
			@AllocationType varchar(50)='Estimated'

	set @startYear = YEAR(@currentDate)
	while(@start <= @totalYears)
	begin
		if(@start = 1)
		begin
			set @FromDate = convert(varchar,@startYear) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear) + '-12-31 00:00:00.000')
		end
		if(@start = 2)
		begin
			set @FromDate = convert(varchar,@startYear + 1) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear + 1) + '-12-31 00:00:00.000')
		end
		if(@start = 3)
		begin
			set @FromDate = convert(varchar,@startYear + 2) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear + 2) + '-12-31 00:00:00.000')
		end
		if(@start = 4)
		begin
			set @FromDate = convert(varchar,@startYear - 1) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear - 1) + '-12-31 00:00:00.000')
		end
		if(@start = 5)
		begin
			set @FromDate = convert(varchar,@startYear - 2) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear - 2) + '-12-31 00:00:00.000')
		end
		if(@start = 6)
		begin
			set @FromDate = convert(varchar,@startYear -3) + '-01-01 00:00:00.000'
			set @ToDate = DATEADD(MONTH,0, convert(varchar,@startYear -3) + '-12-31 00:00:00.000')
		end

		EXEC [usp_fillResourceSummaryUtilizationFooter] @TenantId, @FromDate, @ToDate, @mode,@AllocationType,@IsAssignedallocation

		set @start = @start + 1

	end

end
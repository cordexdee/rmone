create procedure usp_GetSummaryPageData
@TicketId varchar(100),
@TenantId varchar(max)
as
begin
		select TicketId, 
			   Title, 
			   TenantId, 
			   isnull(ResouceHoursBilled,0) ResouceHoursBilled, 
			   isnull(Complexity,0) Complexity,
			   isnull(Volatility,0) Volatility, 
			   isnull(ResourceHoursPrecon,0) ResourceHoursPrecon, 
			   isnull(ResourceHoursBilledtoDate,0) ResourceHoursBilledtoDate, 
			   isnull(ResourceHoursActual,0) ResourceHoursActual, 
			   isnull(ResourceHoursRemaining,0) ResourceHoursRemaining, 
			   isnull(TotalResourceHours,0) TotalResourceHours, 
			   isnull(TotalResourceCost,0) TotalResourceCost
		   from ProjectSummaryPage 
		   where TicketId = @TicketId and TenantId = @TenantId
end
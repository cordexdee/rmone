/****** Object:  StoredProcedure [dbo].[usp_getUtilizationForecastData]    Script Date: 1/12/2024 3:33:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 ALTER procedure [dbo].[usp_getUtilizationForecastData]  
(  
 @TenantId nvarchar(128),  
 @Year varchar(4),
 @division int = 0
)  
as  
begin  
   
 if object_id('tempdb..#tmpData') is not null drop table #tmpData  
  
 select PA.AllocationStartDate, PA.AllocationEndDate, PA.AssignedToUser
 , SUM(PA.PctAllocation/100) as PctAllocation, PA.TicketId,CP.DivisionLookup into #tmpCheck  
 from ProjectEstimatedAllocation PA with (nolock) JOIN CRMProject CP WITH(NOLOCK) ON PA.TicketID = CP.TicketId  
 where PA.TenantID=@TenantId and PA.AssignedToUser <> '00000000-0000-0000-0000-000000000000' --and PA.TicketId like 'CPR-%'  
 group by PA.TicketId, PA.AssignedToUser, PA.AllocationStartDate, PA.AllocationEndDate,CP.DivisionLookup order by TicketId desc  
   
 declare @start int = 1, @monthCount int = 12  
  
 create table #tmpData  
 (  
  [Year] varchar(4),  
  [Month] int,  
  [Project] varchar(50),  
  [Pct] float,  
  [Resources] int  
 )  
  
 while(@start <= @monthCount)  
 begin  
  
  insert into #tmpData  
  select @Year as [Year],  
     @start [Month],  
     TicketId,  
     sum(PctAllocation) as Pct,  
     count(AssignedToUser) as Resources  
  from #tmpCheck   
  where year(AllocationStartDate) = @Year and MONTH(AllocationStartDate) = @start  
  AND (DivisionLookup = @division OR @division = 0 OR @division IS NULL)
  group by TicketId  
  
  set @start = @start + 1;  
 end  
  
 select MAX(year) [Year], DateName( month , DateAdd( month , [Month] , 0 ) - 1 ) as [Month], CEILING(SUM(Pct)) Pct, count(Project) as Projects, SUM(Resources) as Resources   
 from #tmpData  
 group by Month  
  
end  

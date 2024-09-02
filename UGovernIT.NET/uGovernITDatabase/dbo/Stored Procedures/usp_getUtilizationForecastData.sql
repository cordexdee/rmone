 create procedure [dbo].[usp_getUtilizationForecastData]  
(  
 @TenantId nvarchar(128),  
 @Year varchar(4)  
)  
as  
begin  
   
 if object_id('tempdb..#tmpData') is not null drop table #tmpData  
  
 select AllocationStartDate, AllocationEndDate, AssignedToUser, SUM(PctAllocation/100) as PctAllocation, TicketId into #tmpCheck  
 from ProjectEstimatedAllocation with (nolock)   
 where TenantID=@TenantId and AssignedToUser <> '00000000-0000-0000-0000-000000000000' and TicketId like 'CPR-%'  
 group by TicketId, AssignedToUser, AllocationStartDate, AllocationEndDate order by TicketId desc  
   
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
  group by TicketId  
  
  set @start = @start + 1;  
 end  
  
 select MAX(year) [Year], DateName( month , DateAdd( month , [Month] , 0 ) - 1 ) as [Month], CEILING(SUM(Pct)) Pct, count(Project) as Projects, SUM(Resources) as Resources   
 from #tmpData  
 group by Month  
  
end  

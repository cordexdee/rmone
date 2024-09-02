
CREATE PROCEDURE [dbo].[GetForecastAndAcquisitionCosts]  
 @TicketId nvarchar(100),   
 @TenantId nvarchar(128)  
AS  
BEGIN  
-- GetForecastAndAcquisitionCosts 'CPR-23-000666','35525396-e5fe-4692-9239-4df9305b915b' 
 Declare @PreconStartDate Datetime;  
 Declare @PreconEndDate Datetime;  
 Declare @EstimatedConstructionStart Datetime;  
 Declare @EstimatedConstructionEnd Datetime;  
 Declare @CloseoutStartDate Datetime;  
  
 Declare @StartDate Datetime;  
 Declare @EndDate Datetime;  
 Declare @AllocStartDt Datetime;  
 Declare @AllocEndDate Datetime;  
   
 Declare @Resource nvarchar(128);  
 Declare @JobTitleLookup bigint;  
  
 Declare @Cost float;  
 Declare @ForecastedAcquisitionCost float;  
 Declare @ActualAcquisitionCost float;  
 Declare @ForecastedProjectCost float;  
 Declare @ActualProjectCost float;  
 Declare @Hours float;  
   
 Declare @Module nvarchar(10);  
  
 Declare @EnablePrjStdWorkItem bit;  
  
 --print @TicketId;  

 IF CHARINDEX('-', @TicketId) > 0
BEGIN
    SELECT @Module = LEFT(@TicketId, CHARINDEX('-', @TicketId) - 1);
	   	    
		 If @Module = 'CPR'  
		 Begin  
		  Select @PreconStartDate = PreconStartDate, @PreconEndDate = PreconEndDate, 
		  @EstimatedConstructionStart = EstimatedConstructionStart, 
		  @EstimatedConstructionEnd = EstimatedConstructionEnd, @CloseoutStartDate = CloseoutStartDate
		  from CRMProject where TenantID = @TenantID and TicketId = @TicketID;  
  
		 End  
		 Else If @Module = 'OPM'  
		 Begin  
		  Select @PreconStartDate = PreconStartDate, @PreconEndDate = PreconEndDate,
		  @EstimatedConstructionStart = EstimatedConstructionStart,
		  @EstimatedConstructionEnd = EstimatedConstructionEnd, @CloseoutStartDate = CloseoutStartDate
		  from Opportunity where TenantID = @TenantID and TicketId = @TicketID;    
		 End  
  
		 --select @PreconStartDate as PreconStartDate, @PreconEndDate as PreconEndDate, @EstimatedConstructionStart as EstimatedConstructionStart, @EstimatedConstructionEnd as EstimatedConstructionEnd, @CloseoutStartDate as CloseoutStartDate;  
  
		 If @PreconStartDate IS NULL or @PreconEndDate IS NULL  
		 Begin   
		 print 'start'  
			If @PreconEndDate IS  NULL  
		   Begin  
			Set @EndDate = @EstimatedConstructionStart - 1;  
			Set @PreconEndDate = @EstimatedConstructionStart - 1;  
		   End  
  
		   If @EstimatedConstructionEnd IS  NULL  
		   Begin  
			Set @EstimatedConstructionEnd = @CloseoutStartDate - 1;  
		   End  
  
		   -- SUPERINTENDENT RULE  
		   If @EstimatedConstructionEnd IS  NULL and @CloseoutStartDate IS NULL  
		   Begin  
			 Set @EnablePrjStdWorkItem = 0;  
			 select @EnablePrjStdWorkItem = KeyValue from Config_ConfigurationVariable where TenantID = @TenantId and KeyName = 'EnableProjStdWorkItems';  
   
			 If @EnablePrjStdWorkItem = 0   
			  Begin  
			   Select @EstimatedConstructionStart = MIN(r.WorkDate), @EstimatedConstructionEnd = MAX(r.WorkDate) from ResourceTimeSheet r join ResourceWorkItems wi on r.ResourceWorkItemLookup = wi.ID  
			   where r.TenantID = @TenantID and wi.WorkItem = @TicketID and wi.SubWorkItem = 'Superintendent';     
			  End  
			 Else If @EnablePrjStdWorkItem = 1  
			  Begin  
			   Select @EstimatedConstructionStart = MIN(WorkDate), @EstimatedConstructionEnd = MAX(WorkDate) from TicketHours  where TenantID = @TenantID and WorkItem = @TicketID and SubWorkItem = 'Superintendent';  
			  End  
		   End  
  
		 End  
  
		-- select @PreconStartDate as PreconStartDate, @PreconEndDate as PreconEndDate  
   
		 If @PreconStartDate IS NULL  
		  Begin  
		   Set @ForecastedAcquisitionCost = NULL;  
		   Set @ActualAcquisitionCost = NULL;  
		  End  
		 Else  
		  Begin  
		   select @ForecastedAcquisitionCost = dbo.fnGetAcquisitionCost(@TenantId,@TicketId,@Module, @PreconStartDate, @PreconEndDate);  
		   select @ActualAcquisitionCost = dbo.fnGetActualAcquisitionCost(@TenantId,@TicketId,@Module, @PreconStartDate, @PreconEndDate);  
		  end  
  
		 select @ForecastedProjectCost = dbo.fnGetForecastedProjectCost(@TenantId,@TicketId,@Module, @PreconStartDate, @PreconEndDate);  
		 select @ActualProjectCost = dbo.fnGetActualProjectCost(@TenantID, @TicketId, @PreconStartDate, @PreconEndDate);   
  
		 select Round(@ForecastedAcquisitionCost, 2) as ForecastedAcquisitionCost, 
		 Round(@ActualAcquisitionCost, 2) as ActualAcquisitionCost, Round(@ForecastedProjectCost, 2) as ForecastedProjectCost,
		 Round(@ActualProjectCost, 2) as ActualProjectCost, 
		 @TicketId as TicketId;  
  END 
END  
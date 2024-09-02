CREATE PROCEDURE [dbo].[usp_insertActualHours]
	@ResourceUser nvarchar(128),
	@ProjectId nvarchar(100),
	@ERPJobId nvarchar(100),
	@ERPJobIdNC nvarchar(100),
	@Dept nvarchar(100),
	@Date DateTime,
	@Hours float,
	@TenantId nvarchar(128),
	@CreatedBy nvarchar(128)
AS
BEGIN
	Declare @TicketId nvarchar(100);
	Declare @Title nvarchar(500);
	Declare @Module nvarchar(25);
	Declare @RoleName nvarchar(200);
	Declare @PrjStdTitle nvarchar(500);
	Declare @PrjStdCode nvarchar(100);
	Declare @Result nvarchar(3);
	
	Declare @StartDate DateTime;
	Declare @EndDate DateTime;
	Declare @Dt Datetime;
	Declare @FromDate DateTime;
	Declare @ToDate DateTime;
	Declare @MonthStartDt DateTime;

	Declare @WorkItemLookup bigint;

	Declare @PrjStdId int;

	Declare @EnablePrjStdWorkItem bit;

	Set @EnablePrjStdWorkItem = 0;
	Set @TicketId = '';

	select @EnablePrjStdWorkItem = KeyValue from Config_ConfigurationVariable where TenantID = @TenantId and KeyName = 'EnableProjStdWorkItems';

	If @Dept = 'JOB'
		Begin
				If Exists (select TicketId from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId) and Closed <> 1)
					Begin
						select @TicketId = TicketId, @Title = Title from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId) and Closed <> 1;
					End
				Else If Exists (select TicketId from CRMProject where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId))
					Begin
						select @TicketId = TicketId, @Title = Title from CRMProject where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId);
					End
				Else If Exists (select TicketId from CRMServices where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId))
					Begin
						select @TicketId = TicketId, @Title = Title from CRMServices where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId);
					End
				Else If Exists (select TicketId from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId) and Closed = 1)
					Begin
						select @TicketId = TicketId, @Title = Title from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobID = @ERPJobId) and Closed = 1;
					End

				Select @PrjStdId = ID ,@PrjStdTitle = Title, @PrjStdCode = Code from ProjectStandardWorkItems where TenantID = @TenantId and Code = 'Billable' and Deleted = 0;
		End
	Else If @Dept = 'NCO'
		Begin
				If Exists (select TicketId from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC) and Closed <> 1)
					Begin
						select @TicketId = TicketId, @Title = Title from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC) and Closed <> 1;
					End
				Else If Exists (select TicketId from CRMProject where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC))
					Begin
						select @TicketId = TicketId, @Title = Title from CRMProject where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC);
					End
				Else If Exists (select TicketId from CRMServices where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC))
					Begin
						select @TicketId = TicketId, @Title = Title from CRMServices where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC);
					End
				Else If Exists (select TicketId from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = ERPJobIDNC) and Closed = 1)
					Begin
						select @TicketId = TicketId, @Title = Title from Opportunity where TenantID = @TenantId and (ProjectId = @ProjectId or ERPJobIDNC = @ERPJobIdNC) and Closed = 1;
					End

				Select @PrjStdId = ID, @PrjStdTitle = Title, @PrjStdCode = Code from ProjectStandardWorkItems where TenantID = @TenantId and Code = 'NCO' and Deleted = 0;
		End
		

	print 'TicketId: ' + @TicketId;

	If Len(@TicketId) <= 0
	Begin
		Select 0;
		Return;
	End

	Select @Module = left(@TicketId, charindex('-', @TicketId) - 1)
	print 'Module: ' + @Module;

	Select @RoleName = [Name] from Roles where TenantId = @TenantId and Id = (select GlobalRoleID from AspNetUsers where TenantID = @TenantId and Id = @ResourceUser);

	set @Dt = DATEADD(DAY, -1, @Date);
	set @StartDate = DATEADD( DAY , 2 - DATEPART(WEEKDAY, @Dt), CAST (@Dt AS DATE ));
	set @EndDate = DATEADD( DAY , 8 - DATEPART(WEEKDAY, @Dt), CAST (@Dt AS DATE ));
	--select @StartDate,@EndDate;
	Set @FromDate = @StartDate;
	Set @ToDate = @EndDate;
	Set @MonthStartDt = DATEADD(MONTH, DATEDIFF(MONTH, 0, @StartDate) , 0); 

		BEGIN TRY  
		IF @EnablePrjStdWorkItem = 0
			Begin
				If Exists (Select ID from ResourceWorkItems where TenantID =  @TenantId and ResourceUser = @ResourceUser and WorkItem = @TicketId and (SubWorkItem = @RoleName or SubWorkItem is NULL) and StartDate = @StartDate and EndDate = @EndDate and Deleted = 0)
					Begin
						Select @WorkItemLookup = ID from ResourceWorkItems where TenantID =  @TenantId and ResourceUser = @ResourceUser and WorkItem = @TicketId and (SubWorkItem = @RoleName or SubWorkItem is NULL) and StartDate = @StartDate and EndDate = @EndDate and Deleted = 0;
					End
				Else
					Begin			
						INSERT INTO [dbo].[ResourceWorkItems]
							   ([ResourceUser]
							   ,[SubWorkItem]
							   ,[WorkItem]
							   ,[WorkItemType]
							   ,[Title]
							   ,[TenantID]
							   ,[Created]
							   ,[Modified]
							   ,[CreatedByUser]
							   ,[ModifiedByUser]
							   ,[Deleted]
							   ,[Attachments]
							   ,[StartDate]
							   ,[EndDate]
							   )
						 VALUES
							   (@ResourceUser
							   ,@RoleName
							   ,@TicketId
							   ,@Module
							   ,NULL
							   ,@TenantId
							   ,GETDATE()
							   ,GETDATE()
							   ,@CreatedBy
							   ,@CreatedBy
							   ,0
							   ,''
							   ,@StartDate
							   ,@EndDate
							   )

						SET @WorkItemLookup = SCOPE_IDENTITY()
					End

				If Not Exists (Select ID from ResourceTimeSheet where TenantID =  @TenantId and ResourceUser = @ResourceUser and ResourceWorkItemLookup = @WorkItemLookup and WorkDate = @Date and Deleted = 0)
				 Begin
					INSERT INTO [dbo].[ResourceTimeSheet]
						   ([HoursTaken]
						   ,[ResourceUser]
						   ,[ResourceWorkItemLookup]
						   ,[WorkDate]
						   ,[WorkDescription]
						   ,[Title]
						   ,[TenantID]
						   ,[Created]
						   ,[Modified]
						   ,[CreatedByUser]
						   ,[ModifiedByUser]
						   ,[Deleted]
						   ,[Attachments])
					 VALUES
						   (@Hours
						   ,@ResourceUser
						   ,@WorkItemLookup
						   ,@Date
						   ,NULL
						   ,@Title + ';#' + @TicketId + ';#'+ISNULL(@RoleName,'')
						   ,@TenantId
						   ,GETDATE()
						   ,GETDATE()
						   ,@CreatedBy
						   ,@CreatedBy
						   ,0
						   ,'')

						   --SELECT '1';
						   Set @Result = '1';
					End
				 Else
				 Begin
					Update ResourceTimeSheet set HoursTaken = @Hours, Modified = GETDATE(), ModifiedByUser = @CreatedBy  
					where TenantID =  @TenantId and ResourceUser = @ResourceUser and ResourceWorkItemLookup = @WorkItemLookup and WorkDate = @Date and Deleted = 0;

					--SELECT '3';
					Set @Result = '3';
				 End			
			End
		Else If @EnablePrjStdWorkItem = 1
			Begin
				If Exists (Select ID from ResourceWorkItems where TenantID =  @TenantId and ResourceUser = @ResourceUser and WorkItem = @TicketId and (SubWorkItem = @RoleName) and (SubSubWorkItem = @PrjStdTitle + ';#' + ISNULL(@PrjStdCode,'') ) and StartDate = @StartDate and EndDate = @EndDate and Deleted = 0)
					Begin
						print 'exist';
						Select @WorkItemLookup = ID from ResourceWorkItems where TenantID =  @TenantId and ResourceUser = @ResourceUser and WorkItem = @TicketId and (SubWorkItem = @RoleName) and (SubSubWorkItem = @PrjStdTitle + ';#' + ISNULL(@PrjStdCode,'')) and StartDate = @StartDate and EndDate = @EndDate and Deleted = 0;
						print @WorkItemLookup;

					End
				Else
					Begin			
						print 'not exist';
						INSERT INTO [dbo].[ResourceWorkItems]
							   ([ResourceUser]
							   ,[SubWorkItem]
							   ,[WorkItem]
							   ,[WorkItemType]
							   ,[Title]
							   ,[TenantID]
							   ,[Created]
							   ,[Modified]
							   ,[CreatedByUser]
							   ,[ModifiedByUser]
							   ,[Deleted]
							   ,[Attachments]
							   ,[StartDate]
							   ,[EndDate]
							   ,[SubSubWorkItem]
							   )
						 VALUES
							   (@ResourceUser
							   ,@RoleName
							   ,@TicketId
							   ,@Module
							   ,NULL
							   ,@TenantId
							   ,GETDATE()
							   ,GETDATE()
							   ,@CreatedBy
							   ,@CreatedBy
							   ,0
							   ,''
							   ,@StartDate
							   ,@EndDate
							   ,@PrjStdTitle + ';#' + ISNULL(@PrjStdCode,'')
							   )

						SET @WorkItemLookup = SCOPE_IDENTITY();
						print @WorkItemLookup;

					End
									

				 If Not Exists (Select ID from ResourceTimeSheet where TenantID =  @TenantId and ResourceUser = @ResourceUser and ResourceWorkItemLookup = @WorkItemLookup and WorkDate = @Date and Deleted = 0)
				 Begin
					INSERT INTO [dbo].[ResourceTimeSheet]
						   ([HoursTaken]
						   ,[ResourceUser]
						   ,[ResourceWorkItemLookup]
						   ,[WorkDate]
						   ,[WorkDescription]
						   ,[Title]
						   ,[TenantID]
						   ,[Created]
						   ,[Modified]
						   ,[CreatedByUser]
						   ,[ModifiedByUser]
						   ,[Deleted]
						   ,[Attachments])
					 VALUES
						   (0
						   ,@ResourceUser
						   ,@WorkItemLookup
						   ,@Date
						   ,NULL
						   ,@Title + ';#' + @TicketId + ';#'+ISNULL(@RoleName,'')
						   ,@TenantId
						   ,GETDATE()
						   ,GETDATE()
						   ,@CreatedBy
						   ,@CreatedBy
						   ,0
						   ,'')

						   --SELECT '1';
							Set @Result = '1';
					End
				 --Else
				 --Begin
					Update ResourceTimeSheet set HoursTaken = @Hours, Modified = GETDATE(), ModifiedByUser = @CreatedBy  
					where TenantID =  @TenantId and ResourceUser = @ResourceUser and ResourceWorkItemLookup = @WorkItemLookup and WorkDate = @Date and Deleted = 0;

					--SELECT '3';
				 --End	


						IF Not EXISTS (SELECT ID from TicketHours  where TenantID = @TenantId and TicketID = @TicketId and ResourceUser = @ResourceUser and WorkDate = @Date and MonthStartDate = @MonthStartDt and WeekStartDate = @StartDate and TaskID = @PrjStdId and WorkItem = @TicketId and (SubWorkItem = @RoleName or SubWorkItem is NULL) and Deleted = 0)
							BEGIN

									INSERT INTO [dbo].[TicketHours]
											   ([TicketID]
											   ,[StageStep]
											   ,[ResourceUser]
											   ,[WorkDate]
											   ,[HoursTaken]
											   ,[ModuleNameLookup]
											   ,[MonthStartDate]
											   ,[WeekStartDate]
											   ,[StandardWorkItem]
											   ,[TaskID]
											   ,[WorkItem]
											   ,[Comment]
											   ,[TenantID]
											   ,[Created]
											   ,[Modified]
											   ,[CreatedByUser]
											   ,[ModifiedByUser]
											   ,[Deleted]
											   ,[Attachments]
											   ,[SubWorkItem])
										 VALUES
											   (@TicketId
											   ,1
											   ,@ResourceUser
											   ,@Date
											   ,@Hours
											   ,@Module
											   ,@MonthStartDt
											   ,@StartDate
											   ,1
											   ,@PrjStdId
											   ,@TicketId
											   ,NULL
											   ,@TenantId
											   ,GETDATE()
											   ,GETDATE()
											   ,@CreatedBy
											   ,@CreatedBy
											   ,0
											   ,''
											   ,ISNULL(@RoleName,''))
							END
					Else
					Begin
						Update TicketHours set HoursTaken = @Hours where TenantID = @TenantId and TicketID = @TicketId and ResourceUser = @ResourceUser and WorkDate = @Date and MonthStartDate = @MonthStartDt and WeekStartDate = @StartDate and TaskID = @PrjStdId and WorkItem = @TicketId and (SubWorkItem = @RoleName or SubWorkItem is NULL) and Deleted = 0;

						Set @Result = '3';
					End
			End

		END TRY  
		BEGIN CATCH  
			 SELECT @ProjectId;  
		END CATCH; 	
									
		select @Result as 'Result', @ResourceUser as 'Resource', @WorkItemLookup as 'WorkItemId', @StartDate as 'WeekStartDate';
END
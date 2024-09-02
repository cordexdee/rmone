CREATE PROCEDURE GetTimesheet  
@TenantId nvarchar(128),
	@UserId nvarchar(200),
	@StartDate DateTime,
	@EndDate DateTime,
	@WorkItem nvarchar(100) = '',
	@SubWorkItem nvarchar(100) = ''
AS
BEGIN
		Declare @UserName nvarchar(500);
		
		Declare @WorkItem1 varchar(50);
		Declare @WorkItemType1 varchar(50);		
		Declare @SubWorkItem1 nvarchar(500);
		Declare @ProjectTitle nvarchar(500);
		Declare @Code1 nvarchar(100);

		Declare @i int;
		Declare @count int;

		Declare @ActualStartDate DateTime;
		Declare @ActualEndDate DateTime;

		Declare @EnableProjStdWorkItems bit;
		select @EnableProjStdWorkItems = KeyValue from Config_ConfigurationVariable where TenantID = @TenantID
		and keyname = 'EnableProjStdWorkItems'

	create table #tmpRecords
	(
		[Name] nvarchar(200),
		UserID nvarchar(128),
		WorkDate Datetime,
		[Day] varchar(20),
		WorkItemType varchar(50),
		WorkItem varchar(50),
		HoursTaken float,
		Title nvarchar(500),
		SubWorkItem nvarchar(500),
		Comment nvarchar(1000),
		Code nvarchar(100)
	)
	
	create table #tmpWorkItems
	(		
		Id int identity,
		WorkItem varchar(50),
		WorkItemType varchar(50),		
		SubWorkItem nvarchar(500),
		Code nvarchar(100)
	)

	create table #tmpProjects
	(
		TicketId varchar(50),
		Title nvarchar(1000)
	)

	insert into #tmpProjects
	(
		TicketId,
		Title
	)
	select TicketId, Title
	from CRMProject where TenantID = @TenantID
	and  isnull(TicketId,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(TicketId,'')  else @WorkItem END;

	insert into #tmpProjects
	(
		TicketId,
		Title
	)
	select TicketId, Title
	from Opportunity where TenantID = @TenantID
	and  isnull(TicketId,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(TicketId,'')  else @WorkItem END;

	insert into #tmpProjects
	(
		TicketId,
		Title
	)
	select TicketId, Title
	from CRMServices where TenantID = @TenantID
	and  isnull(TicketId,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(TicketId,'')  else @WorkItem END;
	
	insert into #tmpProjects
	(
		TicketId,
		Title
	)
	select TicketId, Title
	from PMM where TenantID = @TenantID
	and  isnull(TicketId,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(TicketId,'')  else @WorkItem END;
	
	insert into #tmpProjects
	(
		TicketId,
		Title
	)
	select TicketId, Title
	from NPR where TenantID = @TenantID
	and  isnull(TicketId,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(TicketId,'')  else @WorkItem END;
	
	--select * from #tmpProjects;
	If @EnableProjStdWorkItems = 0
		Begin
				Insert into #tmpRecords
				select u.Name, u.Id as UserID, th.WorkDate, DATENAME(WEEKDAY, th.WorkDate) as [Day], th.ModuleNameLookup as 'WorkItemType', th.WorkItem, th.HoursTaken, tmp.Title, th.SubWorkItem, th.Comment, ps.Code
				from TicketHours th join AspNetUsers u on u.Id = th.ResourceUser
				left join ProjectStandardWorkItems ps on ps.ID = th.TaskID
				left join #tmpProjects tmp on tmp.TicketId = th.WorkItem
				where th.TenantID = @TenantId and th.ResourceUser = @UserId
				and th.WorkDate >= @StartDate and th.WorkDate <= @EndDate and th.Deleted = 0
				and  isnull(th.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(th.WorkItem,'')  else @WorkItem END
				and  isnull(th.SubWorkItem,'') =CASE WHEN LEN(@SubWorkItem)=0 then isnull(th.SubWorkItem,'')  else @SubWorkItem END
				union 
				select u.Name, rt.ResourceUser as UserID, rt.WorkDate, DATENAME(WEEKDAY, rt.WorkDate) as [Day], rw.WorkItemType, rw.WorkItem, rt.HoursTaken,tmp.Title as Title, rw.SubWorkItem, rt.WorkDescription as Comment, '' as Code from ResourceTimeSheet rt
				join ResourceWorkItems rw on rw.ID = rt.ResourceWorkItemLookup
				left join AspNetUsers u on u.Id = rw.ResourceUser
				left join #tmpProjects tmp on tmp.TicketId = rw.WorkItem
				where rt.tenantid = @TenantId
				and rt.WorkDate >= @StartDate and rt.WorkDate <= @EndDate 
				--and rw.WorkItem not in (select TicketId from #tmpProjects)
				and rw.ResourceUser = @UserId and rt.Deleted = 0
				and  isnull(rw.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(rw.WorkItem,'')  else @WorkItem END
		End
	Else
		Begin
				Insert into #tmpRecords
				select u.Name, u.Id as UserID, th.WorkDate, DATENAME(WEEKDAY, th.WorkDate) as [Day], th.ModuleNameLookup as 'WorkItemType', th.WorkItem, th.HoursTaken, tmp.Title, th.SubWorkItem, th.Comment, ps.Code
				from TicketHours th join AspNetUsers u on u.Id = th.ResourceUser
				left join ProjectStandardWorkItems ps on ps.ID = th.TaskID
				left join #tmpProjects tmp on tmp.TicketId = th.WorkItem
				where th.TenantID = @TenantId and th.ResourceUser = @UserId
				and th.WorkDate >= @StartDate and th.WorkDate <= @EndDate and th.Deleted = 0
				and  isnull(th.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(th.WorkItem,'')  else @WorkItem END
				and  isnull(th.SubWorkItem,'') =CASE WHEN LEN(@SubWorkItem)=0 then isnull(th.SubWorkItem,'')  else @SubWorkItem END
				union 
				select u.Name, rt.ResourceUser as UserID, rt.WorkDate, DATENAME(WEEKDAY, rt.WorkDate) as [Day], rw.WorkItemType, rw.WorkItem, rt.HoursTaken,'' as Title, rw.SubWorkItem, rt.WorkDescription as Comment, '' as Code from ResourceTimeSheet rt
				join ResourceWorkItems rw on rw.ID = rt.ResourceWorkItemLookup
				left join AspNetUsers u on u.Id = rw.ResourceUser
				where rt.tenantid = @TenantId
				and rt.WorkDate >= @StartDate and rt.WorkDate <= @EndDate 
				and rw.WorkItem not in (select TicketId from #tmpProjects)
				and rw.ResourceUser = @UserId and rt.Deleted = 0
				and  isnull(rw.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(rw.WorkItem,'')  else @WorkItem END
		End


	

	--select * from #tmpRecords;	
	Select @count = count(*) from ResourceWorkItems where TenantID =  @TenantId
	and ResourceUser =  @UserId and StartDate >= @StartDate and EndDate <= @EndDate and WorkItem not in (select WorkItem from #tmpRecords);
	--select  @count
	select @UserName = [Name] from AspNetUsers where Id = @UserId;
	if @count > 0
	Begin
		print 'No records';
		Insert into #tmpRecords
		(
			[Name],UserID, WorkDate, [Day], WorkItemType, WorkItem, HoursTaken, Title, SubWorkItem, Comment, Code
		)
		select
		@UserName,
		@UserId,
		@StartDate,
		'Monday',
		SUBSTRING(ra.TicketID,CHARINDEX('-',ra.TicketID)-3,3),
		ra.TicketID,
		0,
		tmp.Title,
		r.Name,
		'',
		''
		from 
		ResourceAllocation ra
		left join #tmpProjects tmp on tmp.TicketId = ra.TicketId
		left join Roles r on r.id = ra.RoleId
		where ra.TenantID = @TenantId and ra.ResourceUser = @UserId
		and (@EndDate <= ra.AllocationEndDate AND @StartDate >= ra.AllocationStartDate)
		and len(ra.RoleId) > 0
		and  isnull(ra.TicketID,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(ra.TicketID,'')  else @WorkItem END

		Insert into #tmpRecords
		(
			[Name],UserID, WorkDate, [Day], WorkItemType, WorkItem, HoursTaken, Title, SubWorkItem, Comment, Code
		)
		select
		@UserName,
		@UserId,
		@StartDate,
		'Monday',
		rw.WorkItemType,
		rw.WorkItem,
		0,
		tmp.Title,
		rw.SubWorkItem,
		'',
		''
		from 
		 ResourceWorkItems rw
		left join #tmpProjects tmp on tmp.TicketId =rw.WorkItem
		 where rw.TenantID = @TenantId and rw.ResourceUser = @UserId
		and Deleted = 0
		and id in (
		select ResourceWorkItemLookup from ResourceTimeSheet where TenantID = @TenantId and ResourceUser = @UserId
		and WorkDate = @StartDate and rw.WorkItem not in (select WorkItem from #tmpRecords)
		)
		and  isnull(rw.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(rw.WorkItem,'')  else @WorkItem END
	End
	
	--select * from #tmpRecords;

	insert into #tmpWorkItems select WorkItem, WorkItemType, SubWorkItem,Code from #tmpRecords group by WorkItem, WorkItemType, SubWorkItem,Code having count(*) < 7;

	if Not Exists(select * from #tmpWorkItems)
		Begin
		 select Name, UserID, Convert(varchar(10), WorkDate,120) WorkDate, DATENAME(WEEKDAY, WorkDate) as [Day], WorkItemType, WorkItem, HoursTaken, Title, SubWorkItem, Comment, Code from #tmpRecords order by WorkItem, SubWorkItem,WorkDate;
		End
	Else 
		Begin
			--select * from #tmpWorkItems;
			select @UserName = [Name] from AspNetUsers where Id = @UserId;
			set @i = 1
			Select @count = count(*) from #tmpRecords;
			While @i <= @count
			Begin
				Select @WorkItem1 = WorkItem, @WorkItemType1= WorkItemType, @SubWorkItem1 = SubWorkItem, @Code1 = Code from #tmpWorkItems where Id = @i;

					set @ActualStartDate = @StartDate;
					set @ActualEndDate = @EndDate;
					WHILE (@ActualStartDate <= @ActualEndDate)
					BEGIN
						If Not Exists(select * from #tmpRecords where  @WorkItem1 = WorkItem and @WorkItemType1 = WorkItemType and @SubWorkItem1 = SubWorkItem and @Code1 = Code and WorkDate = @ActualStartDate)
						Begin
								Select @ProjectTitle = Title from #tmpProjects where TicketId = @WorkItem1;

								Insert into #tmpRecords (
									[Name] ,
									UserID ,
									WorkDate ,
									[Day] ,
									WorkItemType ,
									WorkItem ,
									HoursTaken,
									Title,
									SubWorkItem,
									Comment,
									Code)
									values
									(
									@UserName,
									@UserId,
									@ActualStartDate,
									DATENAME(WEEKDAY, @ActualStartDate),
									@WorkItemType1,
									@WorkItem1,
									0,
									@ProjectTitle,
									@SubWorkItem1,
									'',
									''
									)

						End			   					 
					
						set @ActualStartDate = DATEADD(day, 1, @ActualStartDate);
						set @ProjectTitle = '';
					END;

				set @i = @i +1;
			End

			select Name, UserID, Convert(varchar(10), WorkDate,120) WorkDate, DATENAME(WEEKDAY, WorkDate) as [Day], WorkItemType, WorkItem, HoursTaken, Title, SubWorkItem, Comment, Code from #tmpRecords order by WorkItem, SubWorkItem,WorkDate;
		End

	--select WorkItem, WorkItemType, SubWorkItem,Code, count(*) from #tmpRecords group by WorkItem, WorkItemType, SubWorkItem,Code having count(*) < 7;

	drop table #tmpProjects;
	drop table #tmpRecords;
	drop table #tmpWorkItems;
END



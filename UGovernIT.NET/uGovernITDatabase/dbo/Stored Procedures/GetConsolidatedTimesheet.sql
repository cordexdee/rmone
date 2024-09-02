CREATE PROCEDURE [dbo].[GetConsolidatedTimesheet]
@TenantId nvarchar(128),
	@UserId nvarchar(200),
	@StartDate DateTime,
	@EndDate DateTime,
	@WorkItem nvarchar(100) = ''
AS
BEGIN
	Declare @EnableProjStdWorkItems bit;
	select @EnableProjStdWorkItems = KeyValue from Config_ConfigurationVariable where TenantID = @TenantID
	and keyname = 'EnableProjStdWorkItems'

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
			select u.Name, rt.ResourceUser as UserID, Convert(varchar(10), @StartDate,120) WorkDate, rw.WorkItemType, rw.WorkItem, sum(rt.HoursTaken) TotalHours, tmp.Title as Title, rw.SubWorkItem from ResourceTimeSheet rt
			join ResourceWorkItems rw on rw.ID = rt.ResourceWorkItemLookup
			left join AspNetUsers u on u.Id = rw.ResourceUser
			left join #tmpProjects tmp on tmp.TicketId = rw.WorkItem
			where rt.tenantid = @TenantId
			and rt.WorkDate >= @StartDate and rt.WorkDate <= @EndDate and rt.Deleted = 0
			--and rw.WorkItem not in (select TicketId from #tmpProjects)
			and rw.ResourceUser = @UserId
			and  isnull(rw.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(rw.WorkItem,'')  else @WorkItem END
			group by u.Name, rt.ResourceUser, rw.WorkItemType, rw.WorkItem, rw.SubWorkItem, tmp.Title	
			order by WorkItem
		End
	Else
		Begin
			select u.Name, u.Id as UserID, Convert(varchar(10), @StartDate,120) WorkDate, th.ModuleNameLookup as 'WorkItemType', th.WorkItem, sum(th.HoursTaken) TotalHours, tmp.Title, th.SubWorkItem, ps.Code + ' ' + ps.Title as Code
			from TicketHours th join AspNetUsers u on u.Id = th.ResourceUser
			left join ProjectStandardWorkItems ps on ps.ID = th.TaskID
			left join #tmpProjects tmp on tmp.TicketId = th.WorkItem
			where th.TenantID = @TenantId and th.ResourceUser = @UserId
			and th.WorkDate >= @StartDate and th.WorkDate <= @EndDate and th.Deleted = 0
			and  isnull(th.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(th.WorkItem,'')  else @WorkItem END
			group by u.Name, u.Id, th.ModuleNameLookup, th.WorkItem, tmp.Title, th.SubWorkItem, ps.Code, ps.Title
			union 
			select u.Name, rt.ResourceUser as UserID, Convert(varchar(10), @StartDate,120) WorkDate, rw.WorkItemType, rw.WorkItem, sum(rt.HoursTaken) TotalHours,'' as Title, rw.SubWorkItem, '' as Code from ResourceTimeSheet rt
			join ResourceWorkItems rw on rw.ID = rt.ResourceWorkItemLookup
			left join AspNetUsers u on u.Id = rw.ResourceUser
			where rt.tenantid = @TenantId
			and rt.WorkDate >= @StartDate and rt.WorkDate <= @EndDate and rt.Deleted = 0
			and rw.WorkItem not in (select TicketId from #tmpProjects)
			and rw.ResourceUser = @UserId
			and  isnull(rw.WorkItem,'') =CASE WHEN LEN(@WorkItem)=0 then isnull(rw.WorkItem,'')  else @WorkItem END
			group by u.Name, rt.ResourceUser, rw.WorkItemType, rw.WorkItem, rw.SubWorkItem	
			union
	
			select u.Name, rw.ResourceUser as UserID, Convert(varchar(10), @StartDate,120) WorkDate, rw.WorkItemType, rw.WorkItem, 0 as TotalHours, tmp.Title, rw.SubWorkItem,  '' as Code from ResourceWorkItems rw
			left join AspNetUsers u on u.Id = rw.ResourceUser
			left join #tmpProjects tmp on tmp.TicketId = rw.WorkItem
			where rw.TenantID =@TenantId  and rw.ResourceUser = @UserId
			and rw.Deleted = 0
			and rw.ID in (
			select ResourceWorkItemLookup from ResourceTimeSheet where TenantID = @TenantId and ResourceUser = @UserId
			and WorkDate = @StartDate)
			and rw.WorkItem not in (select WorkItem from TicketHours  where TenantID = @TenantId
			and ResourceUser = @UserId
			and WorkDate between @StartDate and @EndDate)
			order by WorkItem
		End

		
	drop table #tmpProjects;
END
CREATE procedure [dbo].[usp_deleteUserProjectExperience]
(
	@TenantId nvarchar(128),
	@UserId varchar(max),
	@TicketId varchar(100),
	@Tags varchar(max)
)
as
begin

	if(@Tags like '%,%')
	begin
		select @TicketId ProjectID, @UserId UserId, Item as TagLookup, @TenantID TenantID into #tmpNewUserMulti from [dbo].[SplitString](@Tags, ',')
		select ROW_NUMBER() over (order by UserId) rn, * into #tmpNewUserRNMulti from #tmpNewUserMulti
		declare @startMulti int = 1, @countMulti int, @TenantIDcheckMulti nvarchar(128), @UserIdcheckMulti varchar(max), @ProjectIDcheckMulti varchar(200), @TagLookupcheckMulti varchar(max)
		set @countMulti = (select count(*) from #tmpNewUserRNMulti)

		while(@startMulti <= @countMulti)
		begin
			set @TenantIDcheckMulti = (select TenantID from #tmpNewUserRNMulti where rn=@startMulti)
			set @UserIdcheckMulti = (select UserId from #tmpNewUserRNMulti where rn=@startMulti)
			set @ProjectIDcheckMulti = (select ProjectID from #tmpNewUserRNMulti where rn=@startMulti)
			set @TagLookupcheckMulti = (select TagLookup from #tmpNewUserRNMulti where rn=@startMulti)

			if exists(select top 1 * from UserProjectExperience where TenantID=@TenantIDcheckMulti and ProjectID=@ProjectIDcheckMulti and UserId=@UserIdcheckMulti and TagLookup = @TagLookupcheckMulti)
			begin
				delete from UserProjectExperience where TenantID=@TenantIDcheckMulti and ProjectID=@ProjectIDcheckMulti and UserId=@UserIdcheckMulti and TagLookup = @TagLookupcheckMulti
			end
			set @startMulti = @startMulti + 1;
		end
	end
	else
	begin
		if exists(select 1 from UserProjectExperience where TenantID=@TenantID and ProjectID=@TicketId and UserId=@UserId and TagLookup = @Tags)
		begin
			delete from UserProjectExperience where TenantID=@TenantID and ProjectID=@TicketId and UserId=@UserId and TagLookup = @Tags
		end
	end
end
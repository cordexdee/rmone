create procedure [dbo].[usp_insertUserProjectExperience]
(
	@TenantID nvarchar(128),
	@UserId varchar(max),
	@ProjectID varchar(200),
	@CreatedBy varchar(max),
	@TagLookup varchar(max) = '',
	@From varchar(50) = ''
)
as
begin

	declare @ExperienceTag varchar(200), @ResourceName varchar(200)
	if(@TagLookup != '')
	begin
		declare @name varchar(20)
		set @name = (select [Name] from AspNetUsers with (nolock) where Id=@UserId)
		if(@From = 'UserInfo')
		begin
			if exists(select 1 from UserProjectExperience where TenantID=@TenantID and UserId=@UserId and ProjectID = '')
			begin
				delete from UserProjectExperience where TenantID=@TenantID and UserId=@UserId and ProjectID = ''
			end

			insert into UserProjectExperience(ProjectID, UserId, ResourceUser, TagLookup, TenantID, Created,Modified,CreatedByUser,ModifiedByUser)
			select @ProjectID, @UserId, @name, Item as TagLookup, @TenantID, getdate(), getdate(), @CreatedBy, @CreatedBy from [dbo].[SplitString](@TagLookup, ',')
		end
		else
		begin
			select @ProjectID ProjectID, @UserId UserId, @name [name], Item as TagLookup, @TenantID TenantID, getdate() Created, getdate() Modified, @CreatedBy CreatedBy, @CreatedBy ModifiedBy into #tmpNewUser from [dbo].[SplitString](@TagLookup, ',')
			select ROW_NUMBER() over (order by UserId) rn, * into #tmpNewUserRN from #tmpNewUser

			declare @start int = 1, @count int, @TenantIDcheck nvarchar(128), @UserIdcheck varchar(max), @ProjectIDcheck varchar(200), @TagLookupcheck varchar(max)
			set @count = (select count(*) from #tmpNewUser)

			while(@start <= @count)
			begin
				set @TenantIDcheck = (select TenantID from #tmpNewUserRN where rn=@start)
				set @UserIdcheck = (select UserId from #tmpNewUserRN where rn=@start)
				set @ProjectIDcheck = (select ProjectID from #tmpNewUserRN where rn=@start)
				set @TagLookupcheck = (select TagLookup from #tmpNewUserRN where rn=@start)

				if not exists(select top 1 * from UserProjectExperience where TenantID=@TenantIDcheck and ProjectID=@ProjectIDcheck and UserId=@UserIdcheck and TagLookup = @TagLookupcheck)
				begin
					insert into UserProjectExperience(ProjectID, UserId, ResourceUser, TagLookup, TenantID, Created,Modified,CreatedByUser,ModifiedByUser)
					select ProjectID, UserId, name, TagLookup, TenantID, Created, Modified, CreatedBy, ModifiedBy from #tmpNewUserRN where rn=@start
				end
				set @start = @start + 1;
			end
		end
	end
	else
	begin
		
		if(@From = 'UserInfo')
		begin
			if exists(select 1 from UserProjectExperience where TenantID=@TenantID and UserId=@UserId and ProjectID = '')
			begin
				delete from UserProjectExperience where TenantID=@TenantID and UserId=@UserId and ProjectID = ''
			end
			return;
		end
		
		set @ExperienceTag = (select TagMultiLookup from CRMProject with (nolock) where TenantID=@TenantID and TicketId=@ProjectID)
		--select @ExperienceTag
		set @ResourceName = (select Name from AspNetUsers with (nolock) where TenantID=@TenantID and Id=@UserId)
		--select @ResourceName
		if(@ExperienceTag != '')
		begin
			if(@ExperienceTag like '%,%')
			begin
				select @ProjectID ProjectID, @UserId UserId, @ResourceName [name], Item as TagLookup, @TenantID TenantID, getdate() Created, getdate() Modified, @CreatedBy CreatedBy, @CreatedBy ModifiedBy into #tmpNewUserMulti from [dbo].[SplitString](@ExperienceTag, ',')
				select ROW_NUMBER() over (order by UserId) rn, * into #tmpNewUserRNMulti from #tmpNewUserMulti
				declare @startMulti int = 1, @countMulti int, @TenantIDcheckMulti nvarchar(128), @UserIdcheckMulti varchar(max), @ProjectIDcheckMulti varchar(200), @TagLookupcheckMulti varchar(max)
				set @countMulti = (select count(*) from #tmpNewUserRNMulti)
				--select * from #tmpNewUserRNMulti
				while(@startMulti <= @countMulti)
				begin
					set @TenantIDcheckMulti = (select TenantID from #tmpNewUserRNMulti where rn=@startMulti)
					set @UserIdcheckMulti = (select UserId from #tmpNewUserRNMulti where rn=@startMulti)
					set @ProjectIDcheckMulti = (select ProjectID from #tmpNewUserRNMulti where rn=@startMulti)
					set @TagLookupcheckMulti = (select TagLookup from #tmpNewUserRNMulti where rn=@startMulti)

					if not exists(select top 1 * from UserProjectExperience where TenantID=@TenantIDcheckMulti and ProjectID=@ProjectIDcheckMulti and UserId=@UserIdcheckMulti and TagLookup = @TagLookupcheckMulti)
					begin
						print 'insert'
						insert into UserProjectExperience(ProjectID, UserId, ResourceUser, TagLookup, TenantID, Created,Modified,CreatedByUser,ModifiedByUser)
						select @ProjectIDcheckMulti, @UserIdcheckMulti, [name], TagLookup, @TenantIDcheckMulti, getdate(), getdate(), @CreatedBy, @CreatedBy from #tmpNewUserRNMulti where rn=@startMulti

						--select @ProjectIDcheckMulti, @UserIdcheckMulti, [name], TagLookup, @TenantIDcheckMulti, getdate(), getdate(), @CreatedBy, @CreatedBy from #tmpNewUserRNMulti where rn=@startMulti
					end
					set @startMulti = @startMulti + 1;
				end
			end
			else	
			begin
				if exists(select top 1 * from UserProjectExperience where TenantID=@TenantID and ProjectID=@ProjectID and UserId=@UserId and TagLookup = @ExperienceTag)
				begin
					select 'Mapping already present'
				end
				else
				begin
					insert into UserProjectExperience values(@ProjectID, @UserId, @ResourceName, @ExperienceTag, @TenantID, getdate(), getdate(), @CreatedBy, @CreatedBy)
					select 'Mapping Added'
				end
			end
		 end
		
	end
	

	
end
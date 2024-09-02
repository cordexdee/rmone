create procedure usp_getUserExperienceTag
(
	@TenantID nvarchar(128),
	@UserId varchar(max),
	@ProjectID varchar(max) = ''
)
as
begin
	if(@ProjectID != '')
	begin
	select STRING_AGG(TagLookup, ',') TagLookup from UserProjectExperience with (nolock) 
	where TenantID=@TenantID and UserId=@UserId and ProjectID=@ProjectID
	end
	else
	begin
	select STRING_AGG(TagLookup, ',') TagLookup from UserProjectExperience with (nolock) 
	where TenantID=@TenantID and UserId=@UserId
	end
end
create procedure usp_getUserProjectExperienceTag
(
	@TenantID nvarchar(128),
	@UserId varchar(max) = '',
	@ProjectTag varchar(100) = ''
)
as
begin
	--select distinct STRING_AGG(UserId, ',') as UserId from UserProjectExperience with (nolock) where TenantID=@TenantID --and TagLookup like '%' + @ProjectTag + '%'
	--and ISNULL(UserId,'0') in (SELECT Case when len(Item)=0 then Isnull(UserId,'0') else Item end FROM DBO.SPLITSTRING(replace(@UserId,';#',','), ','))
	--and ISNULL(TagLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(TagLookup,'0') else Item end FROM DBO.SPLITSTRING(replace(@ProjectTag,';#',','), ','))


	select distinct UserId into #tmpUserId from UserProjectExperience with (nolock) where TenantID=@TenantID --and TagLookup like '%' + @ProjectTag + '%'
	and ISNULL(UserId,'0') in (SELECT Case when len(Item)=0 then Isnull(UserId,'0') else Item end FROM DBO.SPLITSTRING(replace(@UserId,';#',','), ','))
	and ISNULL(TagLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(TagLookup,'0') else Item end FROM DBO.SPLITSTRING(replace(@ProjectTag,';#',','), ','))

	select STRING_AGG(UserId, ',') UserId from #tmpUserId
end
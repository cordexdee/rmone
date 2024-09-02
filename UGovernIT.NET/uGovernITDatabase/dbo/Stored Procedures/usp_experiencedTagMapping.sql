CREATE procedure [dbo].[usp_experiencedTagMapping]
(
	@TagId varchar(max)='',
	@Type varchar(20)='',
	@TenantID nvarchar(128)
)
as
begin
	declare @start int = 1, @count int, @ExperiencedTag varchar(max), @id varchar(max)
	set @count = (select count(*) from ExperiencedTags with (nolock) where TenantID=@TenantID)

	if OBJECT_ID('tempdb..#tmpETRN') is not null drop table #tmpETRN

	update CRMProject set TagMultiLookup = NULL

	select ROW_NUMBER() Over (Order by Title) rn, ID, Title into #tmpETRN from ExperiencedTags with (nolock)

	while(@start <= @count)
	begin
		
		
		if OBJECT_ID('tempdb..#tmpDescription') is not null drop table #tmpDescription
		if OBJECT_ID('tempdb..#tmpTagCount') is not null drop table #tmpTagCount

		select TicketId, Title, Description, ERPJobID, TagMultiLookup into #tmpDescription from CRMProject with (nolock) where TenantID=@TenantID

		set @id = (select ID from #tmpETRN where rn = @start)
		set @ExperiencedTag = (select Title from #tmpETRN where rn = @start)

		select dbo.fn_GetPhraseCount(Description, @ExperiencedTag) as [count], TicketId, Title, TagMultiLookup, ERPJobID into #tmpTagCount from #tmpDescription

		update #tmpTagCount set TagMultiLookup = @id where [count] > 0

		update CP
		set CP.TagMultiLookup = case when ((CP.TagMultiLookup is not null or CP.TagMultiLookup <> '') and (TC.TagMultiLookup is not null)) then (CP.TagMultiLookup + ',' + TC.TagMultiLookup)
								else TC.TagMultiLookup end
		from CRMProject CP inner join #tmpTagCount TC on CP.TicketId = TC.TicketId and CP.Title=TC.Title and TC.ERPJobID=CP.ERPJobID 
		where TC.[count] > 0

		set @start = @start + 1;
	end

	if(@Type = 'Delete')
	begin
		delete from UserProjectExperience where TagLookup=@TagId
	end
end
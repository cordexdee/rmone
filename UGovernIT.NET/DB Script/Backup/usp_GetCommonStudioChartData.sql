Alter procedure usp_GetCommonStudioChartData
--exec usp_GetCommonStudioChartData 'bcd2d0c9-9947-4a0b-9fbf-73ea61035069', 'Contracted'
@tenantID nvarchar(max),
@filter nvarchar(max) = '',
@studio nvarchar(max) = ''
as
begin

	declare @rewardedStage int;

	
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
			inner join AspNetUsers u on r.ResourceUser = u.Id
			inner join JobTitle jt on jt.ID = u.JobTitleLookup where JobType = 'Billable'
End;


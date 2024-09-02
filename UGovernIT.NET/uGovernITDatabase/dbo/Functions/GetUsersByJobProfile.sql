CREATE FUNCTION [dbo].[GetUsersByJobProfile]
(
	-- Add the parameters for the function here
	@JobProfile nvarchar(1000),
	@TenantId nvarchar(128),
	@Department varchar(2000),
	@IncludeAllResources bit = 'False',
	@Year varchar(5)
)
RETURNS nvarchar(max)
AS
BEGIN
	-- Return the result of the function
	Declare @query nvarchar(2000);

	If @IncludeAllResources = 0
		Begin
			Set @query =  STUFF((SELECT CHAR(10) + u.[Name]
					FROM  AspNetUsers u where u.JobProfile =  @JobProfile and len(JobProfile) > 0 and TenantID = @TenantId and Enabled = 1
					and u.Id in (select distinct ResourceUser from ResourceAllocationMonthly where TenantID = @TenantId  and Year(MonthStartDate) = @Year)
					and ISNULL(u.DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
					order by u.[Name]
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'');
		End
	Else
		Begin
			Set @query =  STUFF((SELECT CHAR(10) + u.[Name]
			FROM  AspNetUsers u where u.JobProfile =  @JobProfile and len(JobProfile) > 0 and TenantID = @TenantId
			and ISNULL(u.DepartmentLookup,'0') in (SELECT Case when len(Item)=0 then Isnull(u.DepartmentLookup,'0') else Item end  FROM  DBO.SPLITSTRING(replace(@Department,';#',','), ','))
			order by u.[Name]
			FOR XML PATH(''), TYPE
			).value('.', 'NVARCHAR(MAX)') 
		,1,1,'');
		End
	RETURN @query;
END

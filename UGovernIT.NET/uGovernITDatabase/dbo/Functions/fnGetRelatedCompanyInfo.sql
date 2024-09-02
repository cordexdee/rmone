
CREATE FUNCTION [dbo].[fnGetRelatedCompanyInfo]        
(     
@TicketId varchar(20),
@TenantID varchar(128),
@RequiredColumn varchar(128)
)        
RETURNS NVARCHAR(MAX)        
AS        
BEGIN        
 Declare @outputStr nvarchar(max)
 Declare @name nvarchar(max)
 Declare @tmpAllocations Table(
	Id int identity,
	Name nvarchar(max)
)
	Insert into @tmpAllocations 
	(Name) select (case when @RequiredColumn = 'title' then dbo.fnGetCompanyTitle(CRMCompanyLookup,@TenantID) else dbo.fnGetCompanyType(CRMCompanyLookup,@TenantID) end)  from RelatedCompanies where TenantID=@TenantID and TicketId=@TicketId

	Declare @i int, @Count int;
	Set @i = 1;
	Select @Count = count(*) from @tmpAllocations;
	While @i <= @Count
	Begin
		Select @name = Name from @tmpAllocations where Id = @i;
		Set @outputStr = concat(@name,', ', @outputStr);
		Set @i = @i + 1;
	end
 RETURN @outputStr        
        
END 
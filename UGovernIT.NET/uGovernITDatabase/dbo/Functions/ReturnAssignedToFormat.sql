Create FUNCTION [dbo].[ReturnAssignedToFormat] (@str nvarchar(max), @TenantID nvarchar(max))
RETURNS varchar(max)
AS
BEGIN
		
	Declare @totalrecords int=0
	declare @username varchar(max) = dbo.firstword(@SPVal);
	 declare @pct varchar(max) = SUBSTRING(@SPVal, CHARINDEX('*', replace(@SPVal,';~','*')) + 2, LEN(@SPVal));
	 select @outputStr = coalesce(@outputStr + ';#', '') + con.Id + ';~' + @pct + ';~' + con.Name  from  string_split(replace(@username,';#',','), ',') as SP         
	 join AspNetUsers con on con.UserName = SP.value  and con.TenantID=@TenantID

 END




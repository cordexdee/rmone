Create FUNCTION [dbo].[ReturnAssignedToFormat] (@str nvarchar(max), @TenantID nvarchar(max))
RETURNS varchar(max)
AS
BEGIN
		
	Declare @totalrecords int=0Declare @SPVal nvarchar(100)Declare @DTVal nvarchar(100)Declare @i as integer--Declare @TenantID nvarchar(128)='886705e8-089c-4aa1-b9e0-07a588cd471f' --declare @str nvarchar(max) = 'smurthy;~10;#prasad;~15;#vkotrappa;~5'declare @outputStr varchar(max);--select value from string_split(replace(@str,';#','*'),'*')declare @tmpTenants table(ID int identity,SPC varchar(100))Set @totalrecords= (select count(value) from string_split(replace(@str,';#','*'),'*'))insert into @tmpTenants (SPC) select value from string_split(replace(@str,';#','*'),'*');--select * from @tmpTenantsSet @i=1while @i <= @totalrecordsbeginselect @SPVal = SPC from @tmpTenants where Id = @i 		
	declare @username varchar(max) = dbo.firstword(@SPVal);
	 declare @pct varchar(max) = SUBSTRING(@SPVal, CHARINDEX('*', replace(@SPVal,';~','*')) + 2, LEN(@SPVal));
	 select @outputStr = coalesce(@outputStr + ';#', '') + con.Id + ';~' + @pct + ';~' + con.Name  from  string_split(replace(@username,';#',','), ',') as SP         
	 join AspNetUsers con on con.UserName = SP.value  and con.TenantID=@TenantIDset @i = @i + 1Endreturn @outputStr

 END





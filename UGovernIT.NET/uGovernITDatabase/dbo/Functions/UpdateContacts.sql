CREATE FUNCTION [dbo].[UpdateContacts]
(
	@str NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	declare @outputStr nvarchar(2000)
	select @outputStr= coalesce(@outputStr + ',', '') + con.TicketId   from dbo.split_string(@str, ',') as SP	
	join CRMContact con on CAST(con.ID AS VARCHAR) = SP.tuple

	--select @outputStr 

	RETURN @outputStr

END
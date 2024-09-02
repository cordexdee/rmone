

CREATE FUNCTION [dbo].[FirstWord] (@value varchar(max))
RETURNS varchar(max)
AS
BEGIN
    RETURN CASE CHARINDEX('*', replace(@value,';~','*'), 1)
        WHEN 0 THEN @value
        ELSE SUBSTRING(@value, 1, CHARINDEX('*', replace(@value,';~','*'), 1) - 1)  end
END




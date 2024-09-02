ALTER FUNCTION AddStringWithComma
(
    @oldValue NVARCHAR(MAX),
    @newValue NVARCHAR(MAX),
    @delimeter NVARCHAR(2)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
    IF LEN(@oldValue) > 0 
    BEGIN
        SET @oldValue = @oldValue + @delimeter + @newValue;
    END
    ELSE
    BEGIN
        SET @oldValue = @newValue;
    END

    RETURN @oldValue;
END

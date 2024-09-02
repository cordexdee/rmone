Create procedure UpdateStudioTable
@TenantID varchar(250)
as
Begin
		
		Declare @DivisionLookup bigint;
		Declare @CompanyLookup bigint;
		--DECLARE THE VARIABLES FOR HOLDING DATA.
      DECLARE @DivisionName VARCHAR(100)
             ,@StudioName VARCHAR(100)
 
      --DECLARE AND SET COUNTER.
      DECLARE @Counter INT
      SET @Counter = 1

	DECLARE Cprojectrow CURSOR FOR select CRMBusinessUnitChoice, StudioChoice from CRMProject 
	where (CRMBusinessUnitChoice is not null OR studiochoice is not null) ANd TenantID=@TenantID;

	OPEN Cprojectrow;

	--FETCH THE RECORD INTO THE VARIABLES.
      FETCH NEXT FROM Cprojectrow INTO @DivisionName, @StudioName

	  WHILE @@FETCH_STATUS = 0
      BEGIN
				IF @Counter = 1
				Begin
					Select @DivisionLookup = ID, @CompanyLookup = CompanyIdLookup from CompanyDivisions where Title = @DivisionName
					if @DivisionName <> ''
					Begin
						If ISNULL(@DivisionLookup, 0) > 0
						begin
							Insert Into Studio(Title, Description, DivisionLookup, CompanyLookup, tenantID, created, modified)
							values(@StudioName, @DivisionName + ' > ' + @StudioName, @DivisionLookup, ISnull(@CompanyLookup,7), @TenantID, GETDATE(), GETDATE())
						End;
						Else
						Begin
							Insert Into CompanyDivisions(CompanyIdLookup, Description, GLCode, Title, TenantID, Created, Modified)
							values(ISNULL(@CompanyLookup,7), @DivisionName, '', @DivisionName, @TenantID, GETDATE(), GETDATE())

							Select @DivisionLookup = ID, @CompanyLookup = CompanyIdLookup from CompanyDivisions where Title = @DivisionName

							Insert Into Studio(Title, Description, DivisionLookup, CompanyLookup, tenantID, created, modified)
							values(@StudioName, @DivisionName + ' > ' + @StudioName, @DivisionLookup, ISnull(@CompanyLookup,7), @TenantID, GETDATE(), GETDATE())
						End;
					End;
				End;

				SET @Counter = @Counter + 1
				FETCH NEXT FROM Cprojectrow INTO @DivisionName, @StudioName
	  End;
End;



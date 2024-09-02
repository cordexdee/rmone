create  proc MapStudioTextColToLookup 
@TenantID varchar(250)
as
Begin
		Declare @StudioText varchar(250)
		Declare @DivsionText varchar(250)
		Declare @StudioLookup bigint
		Declare @UpdatedRecordID bigint

		Declare c cursor For select ID, StudioChoice, RIGHT(CRMBusinessUnitChoice, Case When LEN(CRMBusinessUnitChoice) = 0 then null else LEN(CRMBusinessUnitChoice) End - 5) as CRMBusinessUnitChoice from CRMProject where TenantID=@TenantID;

		OPEN C;

		FETCH NEXT FROM C INTO @UpdatedRecordID, @StudioText, @DivsionText
		WHILE @@FETCH_STATUS = 0
        BEGIN
			if DATALENGTH(@StudioText) > 0
			Begin
				select @StudioLookup = ID  from Studio where Title = @StudioText and TenantID=@TenantID;				
			End;
			Else
			Begin
				IF DATALENGTH(@DivsionText) > 0
				Begin 
					select @StudioLookup = ID from Studio where Title = @DivsionText and TenantID=@TenantID;
				End;
			End;

			--Update 
				if @StudioLookup > 0
				Begin
					Update CRMProject set StudioLookup = @StudioLookup where ID=@UpdatedRecordID;
				End;

			Set @StudioLookup = 0;
			FETCH NEXT FROM C INTO @UpdatedRecordID, @StudioText, @DivsionText
		End;
End;
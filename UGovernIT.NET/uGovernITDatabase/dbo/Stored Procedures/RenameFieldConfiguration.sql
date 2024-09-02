-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RenameFieldConfiguration]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;



SELECT IDENTITY(BIGINT, 1, 1) AS ID,* INTO #temptbl FROM  
(SELECT DISTINCT FieldName,ParentTableName,DataType,TableName FROM FieldConfiguration WHERE (DataType ='Lookup' OR DataType='Choices' OR DataType='UserField') AND FieldName NOT LIKE '%Lookup' AND FieldName NOT LIKE '%Choice' AND FieldName NOT LIKE '%User') x
DECLARE @rowCount BIGINT;
SET @rowCount=(SELECT count(*) FROM #temptbl);
PRINT @rowCount;
DECLARE @counter BIGINT =1;
	IF @rowCount>0
	BEGIN
		PRINT @rowCount
		WHILE(@counter<=@rowCount)
		BEGIN
			declare @CurrentColumnName sysname = ''
			, @NewColumnName sysname = ''
			,@datatype nvarchar (100)=''
			,@actualCol sysname=''
			,@mappedtbl nvarchar(max)
			,@parenttbl nvarchar(max);

			SELECT @actualCol=FieldName, @CurrentColumnName= FieldName,@datatype=DataType,@parenttbl=ParentTableName,@mappedtbl=TableName FROM #temptbl WHERE ID=@counter;
			Declare @colDiffer BIT=0;
			Declare @suffix nvarchar(30)='';
			if(@datatype='Lookup')
			Begin 
				Set @suffix='Lookup';
				Set @colDiffer=case when @CurrentColumnName like '%'+@suffix then 1 else 0 end;
				--case when  CHARINDEX(@suffix,@CurrentColumnName)>0 then 1 else 0 end;
			End;
			else if(@datatype='Choices')
			Begin
				Set @suffix='Choice';
				Set @colDiffer=case when  @CurrentColumnName like '%'+@suffix then 1 else 0 end;
				--case when  CHARINDEX(@suffix,@CurrentColumnName)>0 then 1 else 0 end;
			End;
			else if(@datatype='UserField')
			Begin
				Set @suffix='User';
				Set @colDiffer=case when  @CurrentColumnName like '%'+@suffix then 1 else 0 end;
				--case when  CHARINDEX(@suffix,@CurrentColumnName)>0 then 1 else 0 end;
			End;

			if @colDiffer=1 OR (@datatype='Choices' AND (@mappedtbl IS NULL OR @mappedtbl=''))
				begin
				 Set @counter=@counter+1;
				 continue;
				end;

			if @colDiffer=0
			begin
				Set @NewColumnName=@CurrentColumnName+@suffix;
				--drop table #ColRelatedTables
				IF @datatype='Lookup'
				BEGIN
					SELECT IDENTITY(BIGINT, 1, 1) AS colID,* INTO #ColRelatedTables FROM (
					select distinct o.name tableName,c.name colName from sys.columns c
					join sys.objects o on o.object_id = c.object_id
					where c.name = @CurrentColumnName) y
					Declare @colRelaCount BIGINT=(SELECT count(*) FROM #ColRelatedTables),@innerCounter BIGINT=1;
					Declare @paramval nvarchar(max)='';
					Declare @tableName nvarchar(100)='',@colN nvarchar(100)='';

						if @colRelaCount>0
						begin
							while @innerCounter<=@colRelaCount
							begin
								Select @tableName=cr.tableName,@colN=cr.colName from #ColRelatedTables cr where cr.colID=@innerCounter;
								
								if @colN=@NewColumnName OR (@datatype='Lookup' AND @parenttbl=@tableName) OR ((@CurrentColumnName='ModuleType' OR @CurrentColumnName='CategoryName') AND @tableName='Config_Modules' ) 
								begin
									Set @innerCounter=@innerCounter+1;
									continue;
								end;
								Set @paramval ='';
								Set @paramval= @tableName + '.' + @colN ;
								If COL_LENGTH(@tableName,@colN) IS NULL
								Begin
									Set @innerCounter=@innerCounter+1;
									continue;
								End;
								-- Rename columns
								Exec sp_rename @paramval,@NewColumnName,'COLUMN';

								Insert into FieldConfigurationChanges(OldColumn,NewColumn,TableName) values(@colN,@NewColumnName,@tableName)
								Set @innerCounter=@innerCounter+1;
							end;
						end;

					drop table #ColRelatedTables;
				END;
				ELSE IF @datatype='Choices'
				BEGIN
					Set @colN=@CurrentColumnName;
					Set @paramval ='';
					Set @paramval= @mappedtbl + '.' + @colN ;
					If COL_LENGTH(@mappedtbl,@colN) IS NULL
					Begin
						Set @counter=@counter+1;
						continue;
					End;

					-- Rename columns
					Exec sp_rename @paramval,@NewColumnName,'COLUMN';

					Insert into FieldConfigurationChanges(OldColumn,NewColumn,TableName) values(@colN,@NewColumnName,@mappedtbl)

				END;
				ELSE IF @datatype='UserField'
				BEGIN
					SELECT IDENTITY(BIGINT, 1, 1) AS colID,* INTO #ColRelatedTablesU FROM (
					select distinct o.name tableName,c.name colName from sys.columns c
					join sys.objects o on o.object_id = c.object_id
					where c.name = @CurrentColumnName) y;
				    Declare @colURelaCount BIGINT=(SELECT count(*) FROM #ColRelatedTablesU),@innerCounterU BIGINT=1;
					Declare @paramvalU nvarchar(max)='';
					Declare @tableNameU nvarchar(100)='',@colNU nvarchar(100)='';

						if @colURelaCount>0
						begin
							while @innerCounterU<=@colURelaCount
							begin
								Select @tableNameU=cr.tableName,@colNU=cr.colName from #ColRelatedTablesU cr where cr.colID=@innerCounterU;
								
								if @colNU=@NewColumnName 
								begin
									Set @innerCounterU=@innerCounterU+1;
									continue;
								end;
								Set @paramvalU ='';
								Set @paramvalU= @tableNameU + '.' + @colNU ;
								If COL_LENGTH(@tableNameU,@colNU) IS NULL
								Begin
									Set @innerCounterU=@innerCounterU+1;
									continue;
								End;
								-- Rename columns
								Exec sp_rename @paramvalU,@NewColumnName,'COLUMN';

								Insert into FieldConfigurationChanges(OldColumn,NewColumn,TableName) values(@colNU,@NewColumnName,@tableNameU)
								Set @innerCounterU=@innerCounterU+1;
							end;
						end;

					drop table #ColRelatedTablesU;
				END;
			end;

		SET @counter=@counter+1;
	END;
END;

END
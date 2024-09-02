

--exec Usp_InsertFieldConfiguration 'c345e784-aa08-420f-b11f-2753bbebfdd5'
CREATE PROCEDURE Usp_InsertFieldConfiguration
@TenantId varchar(max)
AS
Begin
--create table #FieldConfiguration123
--(
--	[FieldName] [nvarchar](250) NULL,
--	[ParentTableName] [varchar](250) NULL,
--	[ParentFieldName] [varchar](250) NULL,
--	[DataType] [varchar](250) NULL,
--	[Data] [nvarchar](250) NULL,
--	[DisplayChoicesControl] [nvarchar](250) NULL,
--	[Multi] [bit] NOT NULL,
--	[SelectionSet] [nvarchar](500) NULL,
--	[Notation] [varchar](500) NULL,
--	[TenantID] [nvarchar](128) NULL,
--	[TemplateType] [varchar](500) NULL,
--	[Width] [nvarchar](10) NULL
--	)
	




if object_id('tempdb..#TempFieldConf') is not null
    drop table #TempFieldConf;

 
--Get all table with foreign key columns
select * into #TempFieldConf from 
 (select distinct t.primary_table, t.fk_columns from (select fk_tab.name as foreign_table,
     pk_tab.name as primary_table,
    substring(column_names, 1, len(column_names)-1) as [fk_columns]
from sys.foreign_keys fk
    inner join sys.tables fk_tab
        on fk_tab.object_id = fk.parent_object_id
    inner join sys.tables pk_tab
        on pk_tab.object_id = fk.referenced_object_id
    cross apply (select col.[name] + ', '
                    from sys.foreign_key_columns fk_c
                        inner join sys.columns col
                            on fk_c.parent_object_id = col.object_id
                            and fk_c.parent_column_id = col.column_id
                    where fk_c.parent_object_id = fk_tab.object_id and fk_c.constraint_object_id = fk.object_id
                            order by col.column_id
                            for xml path ('') ) D (column_names)) as t left join FieldConfiguration as f on t.fk_columns = f.FieldName where f.FieldName is null) as stable;

 

select * from #TempFieldConf;


DECLARE 
    @primary_table NVARCHAR(MAX), 
    @fk_columns   NVARCHAR(MAX);
 
DECLARE cursor_ConfigTenant CURSOR
FOR SELECT 
        primary_table, 
        fk_columns
    FROM 
        #TempFieldConf;

		OPEN cursor_ConfigTenant;

		FETCH NEXT FROM cursor_ConfigTenant INTO 
    @primary_table, 
    @fk_columns;
 
WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT @primary_table + '_' + @fk_columns;
        FETCH NEXT FROM cursor_ConfigTenant INTO 
            @primary_table, 
            @fk_columns;

			DECLARE @sql varchar(4000)
			DECLARE @count int
			
	select @count= (select count(*)
          FROM   INFORMATION_SCHEMA.COLUMNS
          WHERE  TABLE_NAME =  @primary_table 
                 AND COLUMN_NAME ='Title')
Declare @parentFieldName varchar(max)='Title';
Declare @dataType varchar(max)='Lookup';
if(@primary_table='AspNetUsers')
begin
set @parentFieldName='Id'
set @dataType='UserField'
end
else if(@primary_table='Config_ModuleLifeCycles')
begin
set @parentFieldName='Name'
set @dataType='Lookup'
end
else if(@primary_table='DMSAccess')
begin
set @parentFieldName='AccessType'
set @dataType='Lookup'
end
else if(@primary_table='DMSCustomer')
begin
set @parentFieldName='CustomerName'
set @dataType='Lookup'
end
else if(@primary_table='DMSDirectory')
begin
set @parentFieldName='DirectoryName'
set @dataType='Lookup'
end
else if(@primary_table='DMSUsersFilesAuthorization')
begin
set @parentFieldName='UsersFilesAuthorizationId'
set @dataType='Lookup'
end
else if(@primary_table='LandingPages')
begin
set @parentFieldName='Name'
set @dataType='Lookup'
end
else if(@primary_table='Assets')
begin
set @parentFieldName='AssetTagNum'
set @dataType='Lookup'
end

	if (@count>0)
	BEGIN
	if exists (select * from FieldConfiguration where FieldName=@fk_columns AND ParentTableName=@primary_table)
	
	BEGIN
	PRINT 'Field  Exist'

	END
	ELSE
	BEGIN
	PRINT 'Field not Exist'
	INSERT INTO FieldConfiguration
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           (@fk_columns, @primary_table, @parentFieldName, @dataType, NULL, NULL, 0, NULL, NULL, @TenantId, NULL,NULL)
	END
	

	END
	else
	begin
	if exists (select * from FieldConfiguration where FieldName=@fk_columns AND ParentTableName=@primary_table)
	
	BEGIN
	PRINT 'Field  Exist'

	END
	ELSE
	BEGIN
	PRINT 'Field not Exist'
	INSERT INTO FieldConfiguration
           ([FieldName], [ParentTableName], [ParentFieldName], [DataType], [Data], [DisplayChoicesControl], [Multi], [SelectionSet], [Notation], [TenantID], [TemplateType], [Width])
     VALUES
           (@fk_columns, @primary_table, @parentFieldName, @dataType, NULL, NULL, 0, NULL, NULL, @TenantId, NULL,NULL)
	END
	--Insert into #TableTitleNotFound values(@primary_table)
	
	End
    END;
		
CLOSE cursor_ConfigTenant;
DEALLOCATE cursor_ConfigTenant;

 drop table #TempFieldConf;

end
 


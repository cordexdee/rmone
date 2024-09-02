/****** Object:  UserDefinedFunction [dbo].[fnGetResolvedColumnsList]    Script Date: 3/21/2024 1:38:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[fnGetResolvedColumnsList]  
(        
@TenantID NVARCHAR(128),  
@ModuleNameLookup NVARCHAR(50)
)        
RETURNS NVARCHAR(MAX)        
AS        
BEGIN        
Declare @outputStr nvarchar(max) ,
--@dateFormat NVARCHAR(20) = 'MMM-dd-yyyy', --'MMM dd, yyyy'      
@UserColumnsList VARCHAR(MAX) = '', @LookupColumnsList VARCHAR(MAX) = '',
--@DateColumnsList VARCHAR(MAX) = '',
@GeneralColumnsList VARCHAR(MAX) = ''
DECLARE @ColumnsTable TABLE (Value NVARCHAR(MAX))
INSERT INTO @ColumnsTable (Value)
Select distinct value from [dbo].[fnGetModule_Columns](@TenantID,@ModuleNameLookup)

SELECT @GeneralColumnsList = STUFF((SELECT ', ' + 'a.' + Value
                                   FROM @ColumnsTable where 
								   value not like '%Lookup%' and value not like '%User%' FOR XML PATH('')),1,1,'');
								   --value not like '%Lookup%' and value not like '%User%' and value not like '%Date%'FOR XML PATH('')),1,1,'');
                         
SELECT @UserColumnsList = STUFF((SELECT ', ' + 'a.' + Value + ' [' + Value + '$Id]' + ' ,' + '[dbo].[fnGetusername] (a.' + Value + ',''' + @TenantID + ''')[' + Value + ']'
                                FROM @ColumnsTable
                                WHERE Value LIKE '%User%'
                                FOR XML PATH('')), 1, 2, '')

--SELECT @DateColumnsList = STUFF((SELECT ', ' + 'FORMAT(a.' + Value + ',''' + @dateFormat + ''')' + ' ' + '[' + Value + ']'
--                                FROM @ColumnsTable
--                                WHERE Value LIKE '%Date%'
--                                FOR XML PATH('')), 1, 2, '')

SELECT @LookupColumnsList = STUFF((SELECT ', ' + 'a.' + Value + ' [' + Value + '$Id]' + ' ,' + '[dbo].[fnGetResolveLookups] (''' + Value + ''', a.' + Value + ',''' + @TenantID + ''')[' + Value + ']'
                                   FROM @ColumnsTable
                                   WHERE Value LIKE '%Lookup%'
                                   FOR XML PATH('')), 1, 2, '')
						SET @LookupColumnsList= REPLACE(@LookupColumnsList,'[CRMCompanyLookup]','[CRMCompanyTitle]')
						SET @LookupColumnsList= REPLACE(@LookupColumnsList,'[CRMCompanyLookup$Id]','[CRMCompanyLookup]')
select @outputStr= @GeneralColumnsList +','+ @UserColumnsList +','+ @LookupColumnsList
--select @outputStr= @GeneralColumnsList +','+ @UserColumnsList+ CASE WHEN NULLIF(@DateColumnsList ,'') IS NOT NULL 
--                           THEN ','+ @DateColumnsList ELSE '' END  +','+ @LookupColumnsList

RETURN @outputStr 
END

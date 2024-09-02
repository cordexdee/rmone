/****** Object:  UserDefinedFunction [dbo].[fnGetResolveLookups]    Script Date: 3/21/2024 1:39:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER FUNCTION [dbo].[fnGetResolveLookups]        
(        
@ColumnName NVARCHAR(MAX),  
@ColumnValue NVARCHAR(MAX), 
@TenantID NVARCHAR(MAX)  
)        
RETURNS NVARCHAR(MAX)        
AS        
BEGIN        
Declare @outputStr nvarchar(max)        
If(@ColumnName='RequestTypeLookup' OR @ColumnName='SubCategoryLookup')
	BEGIN
	IF(@ColumnName='SubCategoryLookup')
		Begin
		select @outputStr= coalesce(@outputStr + '; ', '') + con.SubCategory   from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
			join Config_Module_RequestType con on con.ID = SP.value  and con.TenantID=@TenantID    
		End
	ELSE
		select @outputStr= coalesce(@outputStr + '; ', '') + con.RequestType   from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Config_Module_RequestType con on con.ID = SP.value  and con.TenantID=@TenantID    
	END  
Else If(@ColumnName='AssetLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title   from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Assets con on con.ID = SP.value  and con.TenantID=@TenantID    
	END  
Else If(@ColumnName='DepartmentLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title   from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Department con on con.ID = SP.value  and con.TenantID=@TenantID    
	END  
Else If(@ColumnName='FunctionalAreaLookup')
	BEGIN
		select @outputStr= Title   from  FunctionalAreas where ID = @ColumnValue and TenantID=@TenantID
	END  
Else If(@ColumnName='LocationLookup')
	BEGIN
		select @outputStr= Title   from  Location where ID = @ColumnValue and TenantID=@TenantID
	END  
Else If(@ColumnName='ImpactLookup')
	BEGIN
		select @outputStr= Title   from  Config_Module_Impact where ID = @ColumnValue and TenantID=@TenantID    
	END  
Else If(@ColumnName='PriorityLookup')
	BEGIN
		select @outputStr= Title   from  Config_Module_Priority where ID = @ColumnValue and TenantID=@TenantID    
	END  

Else If(@ColumnName='SeverityLookup')
	BEGIN
		select @outputStr= Title   from  Config_Module_Severity where ID = @ColumnValue and TenantID=@TenantID
	END
Else If(@ColumnName='StateLookup')
	BEGIN
		select @outputStr= Title   from  State where ID = @ColumnValue and TenantID=@TenantID 
	END
Else If(@ColumnName='ModuleStepLookup')
	BEGIN
		select @outputStr= StageTitle   from Config_Module_ModuleStages where ID = @ColumnValue and TenantID=@TenantID    
	END

Else If(@ColumnName='ServiceLookUp')
	BEGIN
		select @outputStr= Title   from  Config_Services where ID = @ColumnValue and TenantID=@TenantID
	END
Else If(@ColumnName='DivisionLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title   from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join CompanyDivisions con on con.ID = SP.value  and con.TenantID=@TenantID    
	END
Else If(@ColumnName='BusinessUnitLookup')
	BEGIN
		select @outputStr= Title from BusinessUnits where ID = @ColumnValue and TenantID=@TenantID 
	END
Else If(@ColumnName='LEMIdLookup')
	BEGIN
		 select @outputStr= Title from Lead where TicketId = @ColumnValue and   TenantID=@TenantID  
	END
Else If(@ColumnName='StageActionUsersUser')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Name  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join AspNetUsers con on con.ID = SP.value  and con.TenantID=@TenantID    
	END
Else If(@ColumnName='StageActionUsersUser')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Name  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join AspNetUsers con on con.ID = SP.value  and con.TenantID=@TenantID    
	END

Else If(@ColumnName='BeneficiariesLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Department con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID    
	END
Else If(@ColumnName='LocationMultLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Location con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID    
	END
Else If(@ColumnName='ACRTypeTitleLookup')
	BEGIN
		select @outputStr= Title from ACRTypes where id= @ColumnValue and   TenantID=@TenantID   
	END
Else If(@ColumnName='APPTitleLookup')
	BEGIN
		select @outputStr= Title from Applications where id= @ColumnValue and   TenantID=@TenantID   
	END

Else If(@ColumnName='ProjectClassLookup')
	BEGIN
		select @outputStr= Title from Config_ProjectClass where id= @ColumnValue and   TenantID=@TenantID   
	END
Else If(@ColumnName='ProjectLifeCycleLookup')
	BEGIN
		select @outputStr= Name from Config_ModuleLifeCycles where id= @ColumnValue and   TenantID=@TenantID   
	END

Else If(@ColumnName='ProjectInitiativeLookup')
BEGIN
	select @outputStr= Title from Config_ProjectInitiative where id= @ColumnValue and   TenantID=@TenantID   
END
Else If(@ColumnName='CompanyMultiLookup')
BEGIN
	select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Company con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID   
END
Else If(@ColumnName='DivisionMultiLookup')
BEGIN
	select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join CompanyDivisions con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID   
END
Else If(@ColumnName='NPRIdLookup')
BEGIN
	select @outputStr= coalesce(@outputStr + '; ', '') + con.TicketId  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join NPR con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID   
END
Else If(@ColumnName='ApplicationMultiLookup')
BEGIN
	select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join Applications con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID   
END
Else If(@ColumnName='RelationshipTypeLookup')
BEGIN
	select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join CRMRelationshipType con on con.ID = CONVERT(bigint,SP.value)  and con.TenantID=@TenantID   
END
Else If(@ColumnName='StudioLookup')
	BEGIN
		select @outputStr= Description   from  Studio where ID = @ColumnValue and TenantID=@TenantID
	END
Else If(@ColumnName='CRMVerticalMultiLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join CRMVerticals con on con.ID = SP.value  and con.TenantID=@TenantID    
	END
Else If(@ColumnName='CRMGeographiesMultiLookup')
	BEGIN
		select @outputStr= coalesce(@outputStr + '; ', '') + con.Title  from  string_split(replace(@ColumnValue,';#',','), ',') as SP         
		join CRMGeographies con on con.ID = SP.value  and con.TenantID=@TenantID    
	END
ELSE If(@ColumnName='CRMCompanyLookup')
	BEGIN
		select @outputStr= Title from CRMCompany where TicketId = @ColumnValue and TenantID=@TenantID
	END
Else
Select @outputStr=@ColumnValue
RETURN @outputStr        
  
END



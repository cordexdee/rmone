
Declare @tenantId nvarchar(128) = '';
Delete from Config_ConfigurationVariable where KeyName='CompanyLogo' and TenantID=@tenantId
Delete from Config_ConfigurationVariable where KeyName='CompanyLogoSidebar' and TenantID=@tenantId

IF NOT EXISTS (SELECT 1 FROM Config_ConfigurationVariable WHERE KeyName = 'HeaderLogo' AND TenantID = @tenantId)
BEGIN
    INSERT INTO Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, Type, TenantID)
    VALUES ('General', 'Company Logo used in Site menu and header, footer, etc. - add here as attachment', 'HeaderLogo', '/content/images/RMONE/rm-one-logo.png', 'HeaderLogo', 'Text', @tenantId)
END

IF NOT EXISTS (SELECT 1 FROM Config_ConfigurationVariable WHERE KeyName = 'ReportLogo' AND TenantID = @tenantId)
BEGIN
    INSERT INTO Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, Type, TenantID)
    VALUES ('General', 'Company Logo used in reports, etc. - add here as attachment', 'ReportLogo', '/content/images/RMONE/rm-one-logo.png', 'ReportLogo', 'Text', @tenantId)
END
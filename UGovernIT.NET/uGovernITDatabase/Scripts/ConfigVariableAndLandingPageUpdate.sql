update Config_ConfigurationVariable set KeyValue = '/content/images/RMONE/rm-one-logo.png' where KeyName = 'CompanyLogo' and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'

update LandingPages set LandingPage = '/Pages/UserEntryPage' where Name = 'admin' and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'

--MK

insert into Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, TenantID) 
values('General','Set company logo at left bottom of page','FooterLogo','/content/images/RMONE/rm-one-logo.png','Footer Logo', '35525396-e5fe-4692-9239-4df9305b915b')

update Config_ConfigurationVariable set KeyValue = '<a href="http://rmone.com" target="_blank">rmone.com</a> 2022-2023 v1.0' 
where KeyName = 'FooterText' and TenantID = '35525396-e5fe-4692-9239-4df9305b915b'

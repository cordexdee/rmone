ALTER TABLE Opportunity
ADD ShortName varchar(20);
ALTER TABLE CRMProject
ADD ShortName varchar(20);
ALTER TABLE CRMServices
ADD ShortName varchar(20);

GO

Update CRMProject set ShortName=  left(Title,20) 
Update Opportunity set ShortName=  left(Title,20) 
Update CRMServices set ShortName=  left(Title,20) 

select ShortName  from CRMProject
select ShortName  from Opportunity
select ShortName  from CRMServices

GO

insert into Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, TenantID)
values('General','This is used set a limit that how many chars are allowed', 'ShortNameCharacters','20','ShortNameCharacters','35525396-e5fe-4692-9239-4df9305b915b')
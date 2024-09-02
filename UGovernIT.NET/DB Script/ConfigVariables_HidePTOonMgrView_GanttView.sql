Delete from Config_ConfigurationVariable where KeyName= 'ShowPTOinGantt'

insert into Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, Type, TenantID) 
values('General','Show/Hide PTO data on Manager Screen Gantt View','HidePTOonGantt','False','HidePTOonGantt','Bool', '35525396-e5fe-4692-9239-4df9305b915b')

insert into Config_ConfigurationVariable(CategoryName, Description, KeyName, KeyValue, Title, Type, TenantID) 
values('General','Show/Hide PTO data on Manager Screen Grid View','HidePTOonMgrView','False','HidePTOonMgrView','Bool', '35525396-e5fe-4692-9239-4df9305b915b')
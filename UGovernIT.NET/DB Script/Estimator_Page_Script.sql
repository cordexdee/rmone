INSERT INTO Config_PageConfiguration(Title,Name, HideLeftMenu, HideTopMenu,HideHeader,HideSearch,HideFooter,ControlInfo,LayoutInfo,TenantID,RootFolder)
VALUES
(
'EstimatorView',
'EstimatorView',
0,
0,
0,
0,
0,
'<?xml version="1.0" encoding="utf-16"?><ArrayOfDockPanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><DockPanelSetting><AssemblyName>uGovernIT.Web.ControlTemplates.DockPanels.EstimatorDockPanel</AssemblyName><ControlID>3bc57069-1dad-4bbd-9d05-2aecdcff9e76_DockPanel_</ControlID><ShowTitle>false</ShowTitle><PanelOrder>0</PanelOrder><ShowCompactRows>false</ShowCompactRows><ShowBandedRows>false</ShowBandedRows></DockPanelSetting></ArrayOfDockPanelSetting>',
'{''testPanel'':[true,''DockedOnly'',''LeftZone'','''','''',0,0,0],''3bc57069-1dad-4bbd-9d05-2aecdcff9e76_DockPanel_'':[true,''All'',''LeftZone'',''275px'','''',0,0,0]}',
'35525396-e5fe-4692-9239-4df9305b915b',
'Pages'
)

UPDATE AspNetRoles
SET LandingPage = '/Pages/EstimatorView'
WHERE Name = 'Estmtr'
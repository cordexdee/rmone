update Config_PageConfiguration 
set 
ControlInfo = '<?xml version="1.0" encoding="utf-16"?><ArrayOfDockPanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><DockPanelSetting xsi:type="HomeDockPanelSetting"><AssemblyName>uGovernIT.Web.ControlTemplates.DockPanels.HomeDockPanel</AssemblyName><ControlID>2775eea1-1c05-4946-8c16-ba9dcbb186c6_DockPanel_</ControlID><Title>CORE Home Page</Title><ShowTitle>true</ShowTitle><PanelOrder>0</PanelOrder><ShowCompactRows>false</ShowCompactRows><ShowBandedRows>false</ShowBandedRows><WelcomeHeading>Welcome to BCCI CORE!</WelcomeHeading><WelcomeDesc /><HideWaitingOnMeTab>false</HideWaitingOnMeTab><HideMyDepartmentTickets>false</HideMyDepartmentTickets><HideMyDivisionTickets>false</HideMyDivisionTickets><HideMyPendingApprovals>false</HideMyPendingApprovals><HideMyProject>false</HideMyProject><HideMyTasks>false</HideMyTasks><HideMyTickets>false</HideMyTickets><HideMyClosedTickets>false</HideMyClosedTickets><HideSMSModules>false</HideSMSModules><HideGovernanceModules>false</HideGovernanceModules><ShowServiceCatalog>false</ShowServiceCatalog><ShowIcons>false</ShowIcons><ServiceViewType /><EnableNewButton>false</EnableNewButton><NoOfPreviewTickets>5</NoOfPreviewTickets><ModulePanelOrder>1</ModulePanelOrder><MyTicketPanelOrder>3</MyTicketPanelOrder><ServiceCatalogOrder>2</ServiceCatalogOrder><IconSize>0</IconSize></DockPanelSetting><DockPanelSetting><AssemblyName>uGovernIT.Web.ControlTemplates.DockPanels.NewTaskDockPanel</AssemblyName><ControlID>30bae5be-8d64-4190-8414-098359837f63_DockPanel_</ControlID><ShowTitle>false</ShowTitle><PanelOrder>0</PanelOrder><ShowCompactRows>false</ShowCompactRows><ShowBandedRows>false</ShowBandedRows></DockPanelSetting></ArrayOfDockPanelSetting>'
where id = 6101

update Config_TabView 
set 
TabDisplayName = 'My Open Projects', ShowTab = '1' where TabName = 'myproject' and ViewName = 'Home'

update Config_TabView 
set 
TabDisplayName = 'My Opportunities', ShowTab = '1' where TabName = 'myopentickets' and ViewName = 'Home'
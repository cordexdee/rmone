UPDATE Config_MenuNavigation SET IsDisabled = 1 WHERE id = 2805

INSERT INTO Config_MenuNavigation 
(CustomizeFormat
,CustomProperties
,MenuHeight
,IconUrl
,IsDisabled
,ItemOrder
,MenuBackground
,MenuDisplayType
,MenuFontColor
,MenuName
,MenuParentLookup
,NavigationType
,NavigationUrl
,MenuWidth
,Title
,TenantID)
VALUES
(0
,'<?xml version="1.0" encoding="utf-16"?><MenuNavigationProperties xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><MenuFontSize /><MenuFontFontFamily>Default</MenuFontFontFamily><MenuIconAlignment /></MenuNavigationProperties>'
,0
,'/Content/images/resourceMng.png'
,0
,3
,'Black'
,'Both'
,'White'
,'Default'
,'2855'
, 'Navigation'
,'/Pages/RMM'
,0
,'Resources'
,'35525396-e5fe-4692-9239-4df9305b915b')


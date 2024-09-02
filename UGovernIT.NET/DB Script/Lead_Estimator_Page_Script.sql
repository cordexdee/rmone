INSERT INTO Config_PageConfiguration(Title,Name, HideLeftMenu, HideTopMenu,HideHeader,HideSearch,HideFooter,ControlInfo,LayoutInfo,TenantID,RootFolder)
VALUES
(
'SeniorEstimatorView',
'SeniorEstimatorView',
0,
0,
0,
0,
0,
'<?xml version="1.0" encoding="utf-16"?><ArrayOfDockPanelSetting xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"><DockPanelSetting><AssemblyName>uGovernIT.Web.ControlTemplates.DockPanels.SeniorEstimatorDockPanel</AssemblyName><ControlID>a8654799-e345-4bf8-80b3-747f3717553f_DockPanel_</ControlID><ShowTitle>false</ShowTitle><PanelOrder>0</PanelOrder><ShowCompactRows>false</ShowCompactRows><ShowBandedRows>false</ShowBandedRows></DockPanelSetting></ArrayOfDockPanelSetting>',
'{''testPanel'':[true,''DockedOnly'',''LeftZone'','''','''',0,0,0],''a8654799-e345-4bf8-80b3-747f3717553f_DockPanel_'':[true,''All'',''LeftZone'',''275px'','''',0,0,0]}',
'35525396-e5fe-4692-9239-4df9305b915b',
'Pages'
)


INSERT INTO AspNetRoles (id,Name,Title,Discriminator,IsSystem,RoleType,TenantID,Created,Modified,CreatedByUser,ModifiedByUser,Deleted,LandingPage)
VALUES('33216e80-b3ad-4b7e-8bea-c0dc6eb02de5','Dir.Est','Dir.Est','Dir.Est',0,0,'35525396-e5fe-4692-9239-4df9305b915b',GETDATE(),GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7',0,'/Pages/SeniorEstimatorView')

INSERT INTO LandingPages(id,Name,LandingPage,TenantID,Created,Modified,CreatedByUser,ModifiedByUser,Deleted)
VALUES('06a45ef7-65ae-4f8b-95fc-4d70f9a8b58c', 'Dir.Est','/Pages/UserEntryPage','35525396-e5fe-4692-9239-4df9305b915b',GETDATE(),GETDATE(),'44380d17-c887-488c-856b-31753e4197b7','44380d17-c887-488c-856b-31753e4197b7',0)

INSERT INTO Config_Module_ModuleColumns ( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) 
VALUES ( N'String', N'', 0, 0, N'', N'LeadEstimatorUser', 3, NULL, 0, 0, 0, NULL, N'Left', N'OPM - ', N'OPM', N'35525396-e5fe-4692-9239-4df9305b915b', GETDATE(), GETDATE(), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'', 0, N'', 0, 0)

INSERT INTO Config_Module_ModuleColumns ( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) 
VALUES ( N'String', N'', 0, 0, N'', N'PriorityLookup', 3, NULL, 0, 0, 0, NULL, N'Left', N'OPM - ', N'OPM', N'35525396-e5fe-4692-9239-4df9305b915b', GETDATE(), GETDATE(), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'', 0, N'', 0, 0)

INSERT INTO Config_Module_ModuleColumns ( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) 
VALUES ( N'String', N'', 0, 0, N'', N'StudioLookup', 3, NULL, 0, 0, 0, NULL, N'Left', N'OPM - ', N'OPM', N'35525396-e5fe-4692-9239-4df9305b915b', GETDATE(), GETDATE(), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'', 0, N'', 0, 0)

INSERT INTO Config_Module_ModuleColumns ( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) 
VALUES ( N'String', N'', 0, 0, N'', N'DivisionLookup', 3, NULL, 0, 0, 0, NULL, N'Left', N'OPM - ', N'OPM', N'35525396-e5fe-4692-9239-4df9305b915b', GETDATE(), GETDATE(), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'', 0, N'', 0, 0)

INSERT INTO Config_Module_ModuleColumns ( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) 
VALUES ( N'String', N'', 0, 0, N'', N'BidDueDate', 3, NULL, 0, 0, 0, NULL, N'Left', N'OPM - ', N'OPM', N'35525396-e5fe-4692-9239-4df9305b915b', GETDATE(), GETDATE(), N'44380d17-c887-488c-856b-31753e4197b7', N'44380d17-c887-488c-856b-31753e4197b7', 0, N'', 0, N'', 0, 0)

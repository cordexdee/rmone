
Declare @TenantID nvarchar(128) = '35525396-e5fe-4692-9239-4df9305b915b'


INSERT Config_Module_ModuleColumns( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified],  [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) VALUES ( N'MultiUser', N'', 0, 0, N'Project Manager', N'ProjectManagerUser', 18, NULL, 1, 1, 1, NULL, N'Center', N'MyHomeTab - Project Manager', N'MyHomeTab', @TenantID, GETDATE(), GETDATE(),  0, NULL, 0, NULL, 0, 0)



INSERT Config_Module_ModuleColumns( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified], [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) VALUES ( N'', N'', 0, 0, N'Studio', N'StudioChoice', 20, NULL, 1, 0, 0, NULL, N'Center', N'MyHomeTab - Studio', N'MyHomeTab', @TenantID, GETDATE(), GETDATE(),  0, N'', 0, NULL, 0, 0)

INSERT Config_Module_ModuleColumns( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified],  [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) VALUES ( N'', N'', 0, 0, N'Start Date', N'EstimatedConstructionStart', 21, NULL, 1, 0, 0, NULL, N'Center', N'MyHomeTab - Start Date', N'MyHomeTab', @TenantID, GETDATE(), GETDATE(),  0, N'', 0, NULL, 0, 0)

INSERT Config_Module_ModuleColumns( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified],  [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) VALUES ( N'', N'', 0, 0, N'Sector', N'SectorChoice', 19, NULL, 1, 0, 0, NULL, N'Center', N'MyHomeTab - Sector', N'MyHomeTab', @TenantID, GETDATE(), GETDATE(),  0, N'', 0, NULL, 0, 0)

INSERT Config_Module_ModuleColumns( [ColumnType], [CustomProperties], [DisplayForClosed], [DisplayForReport], [FieldDisplayName], [FieldName], [FieldSequence], [IsAscending], [IsDisplay], [IsUseInWildCard], [ShowInMobile], [SortOrder], [TextAlignmentChoice], [Title], [CategoryName], [TenantID], [Created], [Modified],  [Deleted], [Attachments], [AllowInlineEdit], [SelectedTabs], [TruncateTextTo], [ShowInCardView]) VALUES ( N'', N'', 0, 0, N'End Date', N'EstimatedConstructionEnd', 22, NULL, 1, 0, 0, NULL, N'Center', N'MyHomeTab - End Date', N'MyHomeTab', @TenantID, GETDATE(), GETDATE(),  0, N'', 0, NULL, 0, 0)



 



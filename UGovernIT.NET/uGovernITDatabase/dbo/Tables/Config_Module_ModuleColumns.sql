CREATE TABLE [dbo].[Config_Module_ModuleColumns] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [ColumnType]          NVARCHAR (250)  NULL,
    [CustomProperties]    NVARCHAR (MAX)  NULL,
    [DisplayForClosed]    BIT             NULL,
    [DisplayForReport]    BIT             NULL,
    [FieldDisplayName]    NVARCHAR (250)  NULL,
    [FieldName]           NVARCHAR (250)  NULL,
    [FieldSequence]       INT             NULL,
    [IsAscending]         BIT             NULL,
    [IsDisplay]           BIT             NULL,
    [IsUseInWildCard]     BIT             NULL,
    [ShowInMobile]        BIT             NULL,
    [SortOrder]           INT             NULL,
    [TruncateTextTo]      INT             DEFAULT ((0)) NULL,
    [TextAlignmentChoice] NVARCHAR (MAX)  NULL,
    [Title]               VARCHAR (250)   NULL,
    [CategoryName]        VARCHAR (250)   NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    [SelectedTabs]        NVARCHAR (MAX)  NULL,
    [ShowInCardView]	  BIT NULL DEFAULT 0
);








GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_ModuleColumns_CategoryName]
    ON [dbo].[Config_Module_ModuleColumns]([CategoryName] ASC);


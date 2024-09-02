
CREATE TABLE [dbo].[DMSDocument] (
    [FileId]              INT              IDENTITY (1, 1) NOT NULL,
    [FileName]            NVARCHAR (250)   NOT NULL,
    [FullPath]            NVARCHAR (1000)  NOT NULL,
    [Size]                INT              NOT NULL,
    [Version]             NVARCHAR (150)   NOT NULL,
    [MainVersionFileId]   INT              NOT NULL,
    [IsCheckedOut]        BIT              NOT NULL,
    [DirectoryId]         INT              NOT NULL,
    [AuthorId]            NVARCHAR (128)   NOT NULL,
    [CustomerId]          INT              NOT NULL,
    [FileParentId]        INT              NULL,
    [Deleted]             BIT              NOT NULL,
    [CreatedByUser]       NVARCHAR (128)   NOT NULL,
    [UpdatedBy]           NVARCHAR (128)   NOT NULL,
    [CreatedOn]           DATETIME         NULL,
    [UpdatedOn]           DATETIME         NULL,
    [FileUid]             UNIQUEIDENTIFIER NULL,
    [CheckOutBy]          NVARCHAR (128)   NULL,
    [StoredFileName]      NVARCHAR (256)   NULL,
    [Description]         NVARCHAR (255)   NULL,
    [DocumentControlID]   NVARCHAR (255)   NULL,
    [DocumentType]        NVARCHAR (255)   NULL,
    [ReviewRequired]      BIT              NULL,
    [ReviewStep]          FLOAT (53)       NULL,
    [Tags]                NVARCHAR (255)   NULL,
    [Title]               NVARCHAR (255)   NULL,
    [UserDocumentVersion] NVARCHAR (255)   NULL,
    [DataMigrationID]     INT              NULL,
	[TenantID]			  NVARCHAR (128) NULL,
    CONSTRAINT [PK_Files_DMS] PRIMARY KEY CLUSTERED ([FileId] ASC),
    CONSTRAINT [FK_File_aspnet_Users] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_File_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[DMSCustomer] ([CustomerId]),
    CONSTRAINT [FK_File_Directory] FOREIGN KEY ([DirectoryId]) REFERENCES [dbo].[DMSDirectory] ([DirectoryId])
);



GO


GO


GO


GO


GO


GO


GO
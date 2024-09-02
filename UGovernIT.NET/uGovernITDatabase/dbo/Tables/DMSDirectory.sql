
CREATE TABLE [dbo].[DMSDirectory] (
    [DirectoryId]       INT            IDENTITY (1, 1) NOT NULL,
    [DirectoryName]     NVARCHAR (250) NOT NULL,
    [DirectoryParentId] INT            NULL,
    [AuthorId]          NVARCHAR (128) NOT NULL,
    [CreatedByUser]     NVARCHAR (128) NOT NULL,
    [UpdatedBy]         NVARCHAR (128) NOT NULL,
    [CreatedOn]         DATETIME       NULL,
    [UpdatedOn]         DATETIME       NULL,
    [Deleted]           BIT            DEFAULT ((0)) NOT NULL,
	[TenantID]			NVARCHAR (128) NULL,
    CONSTRAINT [PK_Directories] PRIMARY KEY CLUSTERED ([DirectoryId] ASC),
    CONSTRAINT [FK_Directory_aspnet_Users] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);



GO


GO


GO



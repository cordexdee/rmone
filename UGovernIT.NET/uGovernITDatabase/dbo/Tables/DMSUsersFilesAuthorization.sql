CREATE TABLE [dbo].[DMSUsersFilesAuthorization] (
    [UsersFilesAuthorizationId] INT            IDENTITY (1, 1) NOT NULL,
    [AccessId]                  INT            NOT NULL,
    [FileId]                    INT            NULL,
    [CustomerId]                INT            NOT NULL,
    [DirectoryId]               INT            NULL,
    [UserId]                    NVARCHAR (128) NOT NULL,
    [CreatedByUser]             NVARCHAR (128) NOT NULL,
    [UpdatedBy]                 NVARCHAR (128) NOT NULL,
    [CreatedOn]                 DATETIME       NULL,
    [UpdatedOn]                 DATETIME       NULL,
	[TenantID]					NVARCHAR (128) NULL,
    CONSTRAINT [PK_UsersFilesAuthorization_DMS] PRIMARY KEY CLUSTERED ([UsersFilesAuthorizationId] ASC),
    CONSTRAINT [FK_DMSUsersFilesAuthorization_DMSUsersFilesAuthorization] FOREIGN KEY ([UsersFilesAuthorizationId]) REFERENCES [dbo].[DMSUsersFilesAuthorization] ([UsersFilesAuthorizationId]),
    CONSTRAINT [FK_UsersFilesAuthorization_Access] FOREIGN KEY ([AccessId]) REFERENCES [dbo].[DMSAccess] ([AccessId]),
    CONSTRAINT [FK_UsersFilesAuthorization_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[DMSCustomer] ([CustomerId])
);



GO


GO


GO


GO


GO


GO


GO
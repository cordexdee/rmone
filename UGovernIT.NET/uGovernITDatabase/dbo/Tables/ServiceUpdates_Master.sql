CREATE TABLE [dbo].[ServiceUpdates_Master] (
    [Id]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [Version]            NVARCHAR (50)   NULL,
    [ServiceId]          BIGINT          NULL,
    [ServiceType]        NVARCHAR (50)   NULL,
    [Title]              NVARCHAR (50)   NULL,
    [Description]        NVARCHAR (MAX)  NULL,
    [ServiceInfo]        NVARCHAR (MAX)  NULL,
    [AvailableForUpdate] BIT             DEFAULT ((0)) NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL
);



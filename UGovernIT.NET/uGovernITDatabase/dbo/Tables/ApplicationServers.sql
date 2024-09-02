CREATE TABLE [dbo].[ApplicationServers] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [APPTitleLookup]        BIGINT          NOT NULL,
    [AssetsTitleLookup]     BIGINT          NOT NULL,
    [EnvironmentLookup]     BIGINT          NOT NULL,
    [Title]                 VARCHAR (250)   NULL,
    [ServerFunctionsChoice] VARCHAR (500)   NULL,
    [TenantID]              NVARCHAR (128)  NULL,
    [Created]               DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]              DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]         NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]        NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]               BIT             DEFAULT ((0)) NULL,
    [Attachments]           NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([APPTitleLookup]) REFERENCES [dbo].[Applications] ([ID]),
    FOREIGN KEY ([AssetsTitleLookup]) REFERENCES [dbo].[Assets] ([ID]),
    FOREIGN KEY ([EnvironmentLookup]) REFERENCES [dbo].[Environment] ([ID])
);








GO

GO

GO



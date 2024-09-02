CREATE TABLE [dbo].[WikiArticles] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [AuthorizedToView]        NVARCHAR (MAX)  NULL,
    [ModuleNameLookup]        NVARCHAR (250)  NULL,
    [RequestTypeLookup]       BIGINT          NOT NULL,
    [WikiContentID]           BIGINT          NULL,
    [TicketId]                NVARCHAR (250)  NULL,
    [WikiSnapshot]            NVARCHAR (600)  NULL,
    [WikiAverageScore]        FLOAT (53)      NULL,
    [WikiDescription]         NVARCHAR (MAX)  NULL,
    [WikiDiscussionCount]     BIGINT          NULL,
    [WikiDislikesCount]       BIGINT          NULL,
    [WikiFavorites]           BIT             NULL,
    [WikiLikesCount]          BIGINT          NULL,
    [WikiLinksCount]          BIGINT          NULL,
    [WikiServiceRequestCount] BIGINT          NULL,
    [WikiViews]               BIGINT          NULL,
    [Title]                   VARCHAR (250)   NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    [ResolutionDate]          DATETIME        NULL,
    [WikiHistory] NVARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID])
);










GO

CREATE TABLE [dbo].[SurveyFeedback] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [Description]      NVARCHAR (MAX)  NULL,
    [ModuleNameLookup] NVARCHAR (250)  NULL,
    [Rating1]          INT             NULL,
    [Rating2]          INT             NULL,
    [Rating3]          INT             NULL,
    [Rating4]          INT             NULL,
    [Rating5]          INT             NULL,
    [Rating6]          INT             NULL,
    [Rating7]          INT             NULL,
    [Rating8]          INT             NULL,
    [ServiceLookUp]    BIGINT          NOT NULL,
    [CategoryName]     NVARCHAR (250)  NULL,
    [SubCategory]      NVARCHAR (250)  NULL,
    [RequestType]      NVARCHAR (250)  NULL,
    [SLADisabled]      BIT             DEFAULT ((0)) NULL,
    [TicketId]         NVARCHAR (250)  NULL,
    [Title]            NVARCHAR (250)  NULL,
    [TotalRating]      INT             NULL,
    [UserDepartment]   NVARCHAR (250)  NULL,
    [UserLocation]     NVARCHAR (250)  NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ServiceLookUp]) REFERENCES [dbo].[Config_Services] ([ID])
);










GO

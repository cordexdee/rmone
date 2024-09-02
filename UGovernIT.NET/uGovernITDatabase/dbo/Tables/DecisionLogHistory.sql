CREATE TABLE [dbo].[DecisionLogHistory] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [ReleaseDate]        DATETIME        NULL,
    [ItemOrder]          INT             NULL,
    [ReleaseID]          NVARCHAR (50)   NULL,
    [Description]        NVARCHAR (500)  NULL,
    [AssignedToUser]     NVARCHAR (MAX)  NULL,
    [AdditionalComments] NVARCHAR (MAX)  NULL,
    [ModuleNameLookup]   NVARCHAR (50)   NULL,
    [TicketId]           NVARCHAR (50)   NULL,
    [Title]              NVARCHAR (250)  NULL,
    [DateIdentified]     DATETIME        NULL,
    [DecisionStatus]     NVARCHAR (MAX)  NULL,
    [DecisionMakerUser]      NVARCHAR (250)  NULL,
    [DateAssigned]       DATETIME        NULL,
    [Decision]           NVARCHAR (MAX)  NULL,
    [DecisionSource]     NVARCHAR (250)  NULL,
    [DecisionDate]       DATETIME        NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    [BaseLineId]         INT             NULL,
    [BaselineDate]       DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);





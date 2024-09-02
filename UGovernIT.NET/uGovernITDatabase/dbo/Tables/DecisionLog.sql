CREATE TABLE [dbo].[DecisionLog] (
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
    [Created]            DATETIME        NOT NULL,
    [Modified]           DATETIME        NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  NOT NULL,
    [Deleted]            BIT             NULL,
    [Attachments]        NVARCHAR (2000) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);





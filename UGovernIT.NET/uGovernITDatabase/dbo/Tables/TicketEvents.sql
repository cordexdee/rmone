CREATE TABLE [dbo].[TicketEvents] (
    [Title]             NVARCHAR (MAX) NULL,
    [Ticketid]          NVARCHAR (200) NULL,
    [SubTaskTitle]      NVARCHAR (200) NULL,
    [SubTaskId]         NVARCHAR (200) NULL,
    [AffectedUsersUser] NVARCHAR (MAX) NULL,
    [Automatic]         BIT            NULL,
    [Comment]           NVARCHAR (MAX) NULL,
    [Created]           DATETIME2 (7)  NULL,
    [EventReason]       NVARCHAR (MAX) NULL,
    [EventTime]         DATETIME2 (7)  NULL,
    [Modified]          DATETIME2 (7)  NULL,
    [ModuleNameLookup]  NVARCHAR (200) NULL,
    [PlannedEndDate]    DATETIME2 (7)  NULL,
    [StageStep]         BIGINT         NULL,
    [Status]            NVARCHAR (100) NULL,
    [TicketEventBy]     NVARCHAR (500) NULL,
    [TicketEventType]   NVARCHAR (100) NULL,
    [CreatedByUser]     NVARCHAR (200) NULL,
    [ModifiedByUser]    NVARCHAR (200) NULL,
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID] VARCHAR(MAX) NULL, 
    CONSTRAINT [PK_TicketEvents] PRIMARY KEY CLUSTERED ([Id] ASC)
);





CREATE TABLE [dbo].[CRMActivities] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ActivityStatus] NVARCHAR (50)  NULL,
    [AssignedToUser] NVARCHAR (MAX) NULL,
    [ContactLookup]  BIGINT         NULL,
    [CreatedByUser]  NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Created]        DATETIME       DEFAULT (getdate()) NOT NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [DueDate]        DATETIME       NULL,
    [EndDate]        DATETIME       NULL,
    [StageStep]      INT            NULL,
    [StartDate]      DATETIME       NULL,
    [TaskID]         NVARCHAR (MAX) NULL,
    [TicketId]       NVARCHAR (128) NULL,
    [Title]          NVARCHAR (128) NULL,
    [TenantID]       NVARCHAR (128) NULL,
    [ModifiedByUser] NVARCHAR (128) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [Deleted]        BIT            NULL,
    [Attachments]    NVARCHAR (500) NULL,
    CONSTRAINT [PK_CRMActivities] PRIMARY KEY CLUSTERED ([Id] ASC)
);



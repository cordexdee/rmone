CREATE TABLE [dbo].[TicketHours] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [TicketID]         NVARCHAR (50)   NULL,
    [StageStep]        INT             NULL,
    [ResourceUser]     NVARCHAR (128)  NULL,
    [WorkDate]         DATETIME        NULL,
    [HoursTaken]       FLOAT (53)      NULL,
    [ModuleNameLookup] VARCHAR (128)   NULL,
    [MonthStartDate]   DATETIME        NULL,
    [WeekStartDate]    DATETIME        NULL,
    [StandardWorkItem] BIT             DEFAULT ((0)) NULL,
    [TaskID]           BIGINT          NULL,
    [WorkItem]         NVARCHAR (250)  NULL,
    [Comment]          NVARCHAR (1000) NULL,
    [TenantID]         NVARCHAR (128)  NULL,
    [Created]          DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]    NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]          BIT             DEFAULT ((0)) NULL,
    [Attachments]      NVARCHAR (2000) DEFAULT ('') NULL,
    [SubWorkItem] NVARCHAR(250) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);









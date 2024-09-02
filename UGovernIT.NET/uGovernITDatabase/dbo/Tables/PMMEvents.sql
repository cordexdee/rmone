CREATE TABLE [dbo].[PMMEvents] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [CategoryChoice]     NVARCHAR (MAX)  NULL,
    [Comments]           NVARCHAR (MAX)  NULL,
    [Duration]           NVARCHAR (250)  NULL,
    [EndDate]            DATETIME        NULL,
    [EventCanceled]      BIT             NULL,
    [EventType]          NVARCHAR (250)  NULL,
    [fAllDayEvent]       NVARCHAR (250)  NULL,
    [fRecurrence]        NVARCHAR (250)  NULL,
    [Location]           NVARCHAR (250)  NULL,
    [MasterSeriesItemID] NVARCHAR (250)  NULL,
    [PMMIdLookup]        NVARCHAR (50)   NULL,
    [RecurrenceData]     NVARCHAR (MAX)  NULL,
    [RecurrenceID]       DATETIME        NULL,
    [RecurrenceInfo]     NVARCHAR (MAX)  NULL,
    [StartDate]          DATETIME        NULL,
    [Status]             NVARCHAR (250)  NULL,
    [TimeZone]           NVARCHAR (250)  NULL,
    [UID]                NVARCHAR (250)  NULL,
    [Workspace]          NVARCHAR (250)  NULL,
    [WorkspaceLink]      NVARCHAR (250)  NULL,
    [XMLTZone]           NVARCHAR (MAX)  NULL,
    [Title]              VARCHAR (250)   NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        CONSTRAINT [DF__PMMEvents__Creat__3B16B004] DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        CONSTRAINT [DF__PMMEvents__Modif__3C0AD43D] DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  CONSTRAINT [DF__PMMEvents__Creat__3CFEF876] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  CONSTRAINT [DF__PMMEvents__Modif__3DF31CAF] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             CONSTRAINT [DF__PMMEvents__Delet__3EE740E8] DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) CONSTRAINT [DF__PMMEvents__Attac__3FDB6521] DEFAULT ('') NULL,
    CONSTRAINT [PK__PMMEvent__3214EC27E2D226A6] PRIMARY KEY CLUSTERED ([ID] ASC)
);












GO

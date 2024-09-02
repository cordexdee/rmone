CREATE TABLE [dbo].[HolidaysAndWorkDaysCalendar] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [CategoryChoice]     NVARCHAR (MAX)  NULL,
    [Description]        NVARCHAR (MAX)  NULL,
    [Duration]           NVARCHAR (250)  NULL,
    [EndDate]            DATETIME        NULL,
    [EventCanceled]      BIT             NULL,
    [EventDate]          DATETIME        NULL,
    [EventType]          NVARCHAR (250)  NULL,
    [Facilities]         NVARCHAR (250)  NULL,
    [fAllDayEvent]       NVARCHAR (250)  NULL,
    [fRecurrence]        NVARCHAR (250)  NULL,
    [FreeBusy]           NVARCHAR (250)  NULL,
    [Location]           NVARCHAR (250)  NULL,
    [MasterSeriesItemID] NVARCHAR (250)  NULL,
    [Overbook]           NVARCHAR (250)  NULL,
    [Participants]       NVARCHAR (MAX)  NULL,
    [ParticipantsPicker] NVARCHAR (250)  NULL,
    [RecurrenceData]     NVARCHAR (MAX)  NULL,
    [RecurrenceID]       DATETIME        NULL,
    [RecurrenceInfo]     NVARCHAR (250)  NULL,
    [Status]             NVARCHAR (250)  NULL,
    [TimeZone]           NVARCHAR (250)  NULL,
    [Title]              NVARCHAR (250)  NULL,
    [UID]                NVARCHAR (250)  NULL,
    [Workspace]          NVARCHAR (250)  NULL,
    [WorkspaceLink]      NVARCHAR (250)  NULL,
    [XMLTZone]           NVARCHAR (MAX)  NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        CONSTRAINT [DF__HolidaysA__Creat__2F1AED73] DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        CONSTRAINT [DF__HolidaysA__Modif__300F11AC] DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  CONSTRAINT [DF__HolidaysA__Creat__310335E5] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  CONSTRAINT [DF__HolidaysA__Modif__31F75A1E] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             CONSTRAINT [DF__HolidaysA__Delet__32EB7E57] DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) CONSTRAINT [DF__HolidaysA__Attac__33DFA290] DEFAULT ('') NULL
);












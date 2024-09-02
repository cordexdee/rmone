CREATE TABLE [dbo].[Appointment] (
    [ID]             INT             IDENTITY (1, 1) NOT NULL,
    [EventType]      INT             NULL,
    [StartTime]      SMALLDATETIME   NULL,
    [EndTime]        SMALLDATETIME   NULL,
    [AllDay]         BIT             NULL,
    [Subject]        NVARCHAR (50)   NULL,
    [Location]       NVARCHAR (50)   NULL,
    [Description]    NVARCHAR (MAX)  NULL,
    [Status]         INT             NULL,
    [Label]          INT             NULL,
    [ResourceID]     INT             NULL,
    [ResourceIDs]    NVARCHAR (MAX)  NULL,
    [ReminderInfo]   NVARCHAR (MAX)  NULL,
    [RecurrenceInfo] NVARCHAR (MAX)  NULL,
    [CustomField1]   NVARCHAR (MAX)  NULL,
    [TenantID]       NVARCHAR (128)  NULL,
    [Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Appointment] PRIMARY KEY CLUSTERED ([ID] ASC)
);









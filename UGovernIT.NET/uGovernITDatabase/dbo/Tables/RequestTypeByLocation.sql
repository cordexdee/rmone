CREATE TABLE [dbo].[RequestTypeByLocation] (
    [ID]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssignmentSLA]               FLOAT (53)      NULL,
    [CloseSLA]                    FLOAT (53)      NULL,
    [LocationLookup]              BIGINT          NOT NULL,
    [ModuleNameLookup]            NVARCHAR (250)  NULL,
    [ORPUser]                     NVARCHAR (MAX)  NULL,
    [PRPGroupUser]                NVARCHAR (MAX)  NULL,
    [RequestorContactSLA]         FLOAT (53)      NULL,
    [BackupEscalationManagerUser] NVARCHAR (MAX)  NULL,
    [EscalationManagerUser]       NVARCHAR (MAX)  NULL,
    [RequestTypeLookup]           BIGINT          NOT NULL,
    [OwnerUser]                   NVARCHAR (250)  NULL,
    [ResolutionSLA]               FLOAT (53)      NULL,
    [Title]                       VARCHAR (250)   NULL,
    [TenantID]                    NVARCHAR (128)  NULL,
    [Created]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                    DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]              NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                     BIT             DEFAULT ((0)) NULL,
    [Attachments]                 NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([LocationLookup]) REFERENCES [dbo].[Location] ([ID]),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID])
);








GO

GO

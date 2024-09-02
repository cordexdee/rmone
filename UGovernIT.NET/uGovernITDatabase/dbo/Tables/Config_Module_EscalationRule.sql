CREATE TABLE [dbo].[Config_Module_EscalationRule] (
    [ID]                       BIGINT          IDENTITY (1, 1) NOT NULL,
    [EscalationDescription]    NVARCHAR (MAX)  NULL,
    [EscalationEmailBody]      NVARCHAR (MAX)  NULL,
    [EscalationFrequency]      FLOAT (53)      NULL,
    [EscalationMailSubject]    NVARCHAR (250)  NULL,
    [EscalationMinutes]        FLOAT (53)      NULL,
    [EscalationToEmails]       NVARCHAR (250)  NULL,
    [EscalationToRoles]        NVARCHAR (250)  NULL,
    [IncludeActionUsers]       BIT             NULL,
    [SLARuleIdLookup]          BIGINT          NOT NULL,
    [UseDesiredCompletionDate] BIT             NULL,
    [NotifyInPlainText]        BIT             NULL,
    [Title]                    VARCHAR (250)   NULL,
    [TenantID]                 NVARCHAR (128)  NULL,
    [Created]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                  BIT             DEFAULT ((0)) NULL,
    [Attachments]              NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([SLARuleIdLookup]) REFERENCES [dbo].[Config_Module_SLARule] ([ID])
);






GO
CREATE NONCLUSTERED INDEX [IX_Config_Module_EscalationRule_UseDesiredCompletionDate] ON [dbo].[Config_Module_EscalationRule] ([UseDesiredCompletionDate])
CREATE TABLE [dbo].[VendorReport] (
    [ID]                       BIGINT          IDENTITY (1, 1) NOT NULL,
    [AcceptanceCriteria]       NVARCHAR (MAX)  NULL,
    [ClientObligations]        NVARCHAR (MAX)  NULL,
    [DeliverableAttributes]    NVARCHAR (MAX)  NULL,
    [DeliverableMode]          NVARCHAR (MAX)  NULL,
    [DocumentLibraryName]      NVARCHAR (250)  NULL,
    [FolderUrl]                NVARCHAR (250)  NULL,
    [NoDocumentNeeded]         BIT             NULL,
    [ReminderDays]             INT             NULL,
    [ReminderType]             NVARCHAR (MAX)  NULL,
    [ReportFrequencyType]      NVARCHAR (MAX)  NULL,
    [ReportingFrequency]       INT             NULL,
    [ReportingFrequencyUnit]   NVARCHAR (MAX)  NULL,
    [ReportingRecepients]      NVARCHAR (250)  NULL,
    [ReportingSLA]             INT             NULL,
    [ReportingStartDate]       DATETIME        NULL,
    [ReportMonthFrequencyType] NVARCHAR (MAX)  NULL,
    [Responsible]              NVARCHAR (250)  NULL,
    [SLAMissedPenalty]         INT             NULL,
    [VendorMSALookup]          BIGINT          NOT NULL,
    [VendorMSANameLookup]      BIGINT          NOT NULL,
    [VendorReportingType]      NVARCHAR (250)  NULL,
    [VendorSOWLookup]          BIGINT          NOT NULL,
    [VendorSOWNameLookup]      BIGINT          NOT NULL,
    [Title]                    VARCHAR (250)   NULL,
    [TenantID]                 NVARCHAR (128)  NULL,
    [Created]                  DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]            NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                  BIT             DEFAULT ((0)) NULL,
    [Attachments]              NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([VendorMSALookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorMSANameLookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorSOWLookup]) REFERENCES [dbo].[VendorSOW] ([ID]),
    FOREIGN KEY ([VendorSOWNameLookup]) REFERENCES [dbo].[VendorSOW] ([ID])
);






GO

GO

GO

GO

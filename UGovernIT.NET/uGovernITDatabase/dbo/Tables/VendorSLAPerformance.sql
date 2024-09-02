CREATE TABLE [dbo].[VendorSLAPerformance] (
    [ID]                         BIGINT          IDENTITY (1, 1) NOT NULL,
    [HigherIsBetter]             BIT             NULL,
    [ITMSReported]               INT             NULL,
    [MaxThreshold]               INT             NULL,
    [MinThreshold]               INT             NULL,
    [Penalty]                    INT             NULL,
    [RequestTypeCategory]        NVARCHAR (250)  NULL,
    [RequestTypeLookup]          BIGINT          NOT NULL,
    [RequestTypeWorkflow]        NVARCHAR (250)  NULL,
    [Reward]                     INT             NULL,
    [SLANumber]                  NVARCHAR (250)  NULL,
    [SLATarget]                  INT             NULL,
    [SLAUnit]                    NVARCHAR (MAX)  NULL,
    [SPReported]                 INT             NULL,
    [VendorMSALookup]            BIGINT          NOT NULL,
    [VendorMSANameLookup]        BIGINT          NOT NULL,
    [VendorPerformanceWaiver]    NVARCHAR (250)  NULL,
    [VendorSLALookup]            BIGINT          NOT NULL,
    [VendorSLAMet]               NVARCHAR (MAX)  NULL,
    [VendorSLANameLookup]        BIGINT          NOT NULL,
    [VendorSLAPerformanceNumber] INT             NULL,
    [VendorSLAReportingDate]     DATETIME        NULL,
    [VendorVPMLookup]            BIGINT          NOT NULL,
    [Weightage]                  INT             NULL,
    [Title]                      VARCHAR (250)   NULL,
    [TenantID]                   NVARCHAR (128)  NULL,
    [Created]                    DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                   DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]              NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]             NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                    BIT             DEFAULT ((0)) NULL,
    [Attachments]                NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([RequestTypeLookup]) REFERENCES [dbo].[Config_Module_RequestType] ([ID]),
    FOREIGN KEY ([VendorMSALookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorMSANameLookup]) REFERENCES [dbo].[VendorMSA] ([ID]),
    FOREIGN KEY ([VendorSLALookup]) REFERENCES [dbo].[VendorSLA] ([ID]),
    FOREIGN KEY ([VendorSLANameLookup]) REFERENCES [dbo].[VendorSLA] ([ID]),
    FOREIGN KEY ([VendorVPMLookup]) REFERENCES [dbo].[VendorVPM] ([ID])
);






GO

GO

GO

GO

GO

GO

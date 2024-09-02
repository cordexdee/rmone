CREATE TABLE [dbo].[Investments] (
    [ID]                      BIGINT          IDENTITY (1, 1) NOT NULL,
    [AcquireDate]             DATETIME        NULL,
    [EmailAddress]            NVARCHAR (250)  NULL,
    [ExpectedExit]            DATETIME        NULL,
    [Investment]              NVARCHAR (250)  NULL,
    [InvestmentManagers]      NVARCHAR (250)  NULL,
    [InvestorShortNameLookup] BIGINT          NOT NULL,
    [INVType]                 NVARCHAR (250)  NULL,
    [ReturnYield]             NVARCHAR (250)  NULL,
    [State]                   NVARCHAR (250)  NULL,
    [StreetAddress]           NVARCHAR (MAX)  NULL,
    [Title]                   NVARCHAR (250)  NULL,
    [VendorPhone]             NVARCHAR (250)  NULL,
    [WorkCity]                NVARCHAR (250)  NULL,
    [WorkZip]                 NVARCHAR (250)  NULL,
    [TenantID]                NVARCHAR (128)  NULL,
    [Created]                 DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]           NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]          NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                 BIT             DEFAULT ((0)) NULL,
    [Attachments]             NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([InvestorShortNameLookup]) REFERENCES [dbo].[Investors] ([ID])
);






GO

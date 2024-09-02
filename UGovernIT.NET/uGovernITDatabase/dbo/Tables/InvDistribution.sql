CREATE TABLE [dbo].[InvDistribution] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [DistributionAmount]  NVARCHAR (250)  NULL,
    [DistributionDate]    DATETIME        NULL,
    [DistributionQuarter] NVARCHAR (MAX)  NULL,
    [DistributionType]    NVARCHAR (250)  NULL,
    [InvestmentIDLookup]  BIGINT          NOT NULL,
    [InvestorIDLookup]    BIGINT          NOT NULL,
    [Title]               NVARCHAR (250)  NULL,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([InvestmentIDLookup]) REFERENCES [dbo].[Investments] ([ID]),
    FOREIGN KEY ([InvestorIDLookup]) REFERENCES [dbo].[Investments] ([ID])
);






GO

GO

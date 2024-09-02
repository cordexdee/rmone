CREATE TABLE [dbo].[Contacts] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AddressedAs]        NVARCHAR (250)  NULL,
    [City]               NVARCHAR (250)  NULL,
    [Country]            NVARCHAR (250)  NULL,
    [EmailAddress]       NVARCHAR (250)  NULL,
    [Fax]                NVARCHAR (250)  NULL,
    [FirstName]          NVARCHAR (250)  NULL,
    [LastName]           NVARCHAR (250)  NULL,
    [MiddleName]         NVARCHAR (250)  NULL,
    [Mobile]             NVARCHAR (250)  NULL,
    [OrganizationLookup] BIGINT          NOT NULL,
    [SecondaryEmail]     NVARCHAR (250)  NULL,
    [State]              NVARCHAR (250)  NULL,
    [StreetAddress1]     NVARCHAR (250)  NULL,
    [StreetAddress2]     NVARCHAR (250)  NULL,
    [Telephone]          NVARCHAR (250)  NULL,
    [Title]              NVARCHAR (250)  NULL,
    [Zip]                NVARCHAR (250)  NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    [ResolutionDate]     DATETIME        NULL,
    [ReopenCount] INT NULL, 
    [ApprovalDate] DATETIME NULL, 
    [ApprovedByUser] VARCHAR(128) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([OrganizationLookup]) REFERENCES [dbo].[Organization] ([ID])
);






GO

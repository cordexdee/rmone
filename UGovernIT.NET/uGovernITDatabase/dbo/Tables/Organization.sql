CREATE TABLE [dbo].[Organization] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [BusinessType]       NVARCHAR (250)  NULL,
    [Certifications]     NVARCHAR (MAX)  NULL,
    [City]               NVARCHAR (250)  NULL,
    [CompanyName]        NVARCHAR (250)  NULL,
    [ContractorLicense]  NVARCHAR (250)  NULL,
    [Country]            NVARCHAR (250)  NULL,
    [CRMStatus]          NVARCHAR (MAX)  NULL,
    [Division]           NVARCHAR (MAX)  NULL,
    [EmailAddress]       NVARCHAR (250)  NULL,
    [Fax]                NVARCHAR (250)  NULL,
    [FederalID]          NVARCHAR (250)  NULL,
    [LegalName]          NVARCHAR (250)  NULL,
    [MasterAgreement]    NVARCHAR (MAX)  NULL,
    [OrganizationNote]   NVARCHAR (250)  NULL,
    [OrganizationStatus] NVARCHAR (MAX)  NULL,
    [OrganizationType]   NVARCHAR (MAX)  NULL,
    [ShortName]          NVARCHAR (250)  NULL,
    [State]              NVARCHAR (250)  NULL,
    [StreetAddress1]     NVARCHAR (250)  NULL,
    [StreetAddress2]     NVARCHAR (250)  NULL,
    [Telephone]          NVARCHAR (250)  NULL,
    [Title]              NVARCHAR (250)  NULL,
    [Trade]              NVARCHAR (MAX)  NULL,
    [WebsiteUrl]         NVARCHAR (250)  NULL,
    [WorkRegion]         NVARCHAR (250)  NULL,
    [WorkType]           NVARCHAR (250)  NULL,
    [Zip]                NVARCHAR (250)  NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL,
    CONSTRAINT [PK_Organization] PRIMARY KEY CLUSTERED ([ID] ASC)
);






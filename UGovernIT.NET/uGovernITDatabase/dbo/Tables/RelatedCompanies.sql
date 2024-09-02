CREATE TABLE [dbo].[RelatedCompanies] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [ContactLookup]          NVARCHAR (1000)  NULL,
    [CostCodeLookup]         BIGINT          NULL,
    [Created]                DATETIME        NOT NULL,   
    [CustomProperties]       NVARCHAR (MAX)  NULL,
    [ItemOrder]              INT             NULL,
    [Modified]               DATETIME        NOT NULL,
    [RelationshipTypeLookup] BIGINT          NULL,
    [TicketID]               NVARCHAR (128)  NULL,
    [Title]                  VARCHAR (250)   NULL,
    [CreatedByUser]          NVARCHAR (128)  NULL,
    [ModifiedByUser]         NVARCHAR (128)  NULL,
    [TenantID]               NVARCHAR (128)  NULL,
    [Deleted]                BIT             NULL,
    [Attachments]            NVARCHAR (2000) NULL,
    [DataMigrationID]        INT             NULL,
	[CRMCompanyLookup]		 NVARCHAR (250)  NULL,
    CONSTRAINT [PK__RelatedC__3214EC07CFB4765E] PRIMARY KEY CLUSTERED ([ID] ASC)
);



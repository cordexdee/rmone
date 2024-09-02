CREATE TABLE [dbo].[Studio]
(
	[ID] BIGINT  IDENTITY (1, 1) NOT NULL,
	[Title]          VARCHAR (250)   NULL,
	[Description] NVARCHAR (MAX)  NULL, 
	[DivisionLookup] BIGINT NULL,
	[FieldDisplayName] NVARCHAR(250) NULL, 
	[CompanyLookup] BIGINT NULL,
	[TenantID]       NVARCHAR (128)  NULL,
	[Created]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]        BIT             DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (2000) DEFAULT ('') NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CompanyLookup]) REFERENCES [dbo].[Company] ([ID]),
    FOREIGN KEY ([DivisionLookup]) REFERENCES [dbo].[CompanyDivisions] ([ID])
)

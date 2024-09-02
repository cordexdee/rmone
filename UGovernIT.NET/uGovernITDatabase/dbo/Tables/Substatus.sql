CREATE TABLE [dbo].[Substatus] (
    [ID]                BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]             NVARCHAR (250)  NULL,
    [DefaultValue]      NVARCHAR (250)  NULL,
    [ModuleLookup]      NVARCHAR (250)  NULL,
    [ModuleStageLookup] NVARCHAR (250)  NULL,
    [CreatedByUser]     NVARCHAR (128)  NULL,
    [CreatedDate]       DATETIME        CONSTRAINT [DF_Substatus_CreatedDate] DEFAULT (getdate()) NULL,
    [ModifiedByUser]    NVARCHAR (128)  NULL,
    [ModifiedDate]      DATETIME        CONSTRAINT [DF_Substatus_ModifiedDate] DEFAULT (getdate()) NULL,
    [TenantID]          NVARCHAR (128)  NULL,
    [Deleted]           BIT             CONSTRAINT [DF_Substatus_Deleted] DEFAULT ((0)) NULL,
    [Attachments]       NVARCHAR (2000) NULL,
    CONSTRAINT [PK_Substatus] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO


GO


GO


GO

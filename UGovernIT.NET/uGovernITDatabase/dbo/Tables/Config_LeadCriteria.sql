CREATE TABLE [dbo].[Config_LeadCriteria] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Priority]       NVARCHAR (200) NULL,
    [MinValue]       DECIMAL (5, 2) NULL,
    [MaxValue]       DECIMAL (5, 2) NULL,
    [Created]        DATETIME       CONSTRAINT [DF__Config_Le__Creat__18CD504F] DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME       CONSTRAINT [DF__Config_Le__Modif__19C17488] DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]  NVARCHAR (128) CONSTRAINT [DF__Config_Le__Creat__1AB598C1] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser] NVARCHAR (128) CONSTRAINT [DF__Config_Le__Modif__1BA9BCFA] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [TenantID]       NVARCHAR (128) NULL,
    [Deleted]        BIT            CONSTRAINT [DF__Config_Le__Delet__1C9DE133] DEFAULT ((0)) NULL,
    [Attachments]    NVARCHAR (100) CONSTRAINT [DF__Config_Le__Attac__1D92056C] DEFAULT ('') NULL,
    CONSTRAINT [PK_LeadCriteria] PRIMARY KEY CLUSTERED ([ID] ASC)
);



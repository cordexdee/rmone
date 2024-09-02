CREATE TABLE [dbo].[PMMBaselineDetail] (
    [ID]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [BaselineComment] NVARCHAR (MAX)  NULL,
    [BaselineDate]    DATETIME        NULL,
    [BaselineNum]     INT             NULL,
    [PMMIdLookup]     BIGINT          NOT NULL,
    [Title]           VARCHAR (250)   NULL,
    [TenantID]        NVARCHAR (128)  NULL,
    [Created]         DATETIME        CONSTRAINT [DF__PMMBaseli__Creat__24334AAC] DEFAULT (getdate()) NOT NULL,
    [Modified]        DATETIME        CONSTRAINT [DF__PMMBaseli__Modif__25276EE5] DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]   NVARCHAR (128)  CONSTRAINT [DF__PMMBaseli__Creat__261B931E] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]  NVARCHAR (128)  CONSTRAINT [DF__PMMBaseli__Modif__270FB757] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]         BIT             CONSTRAINT [DF__PMMBaseli__Delet__2803DB90] DEFAULT ((0)) NULL,
    [Attachments]     NVARCHAR (2000) CONSTRAINT [DF__PMMBaseli__Attac__28F7FFC9] DEFAULT ('') NULL,
    CONSTRAINT [PK__PMMBasel__3214EC2700344126] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK__PMMBaseli__PMMId__14A6EE59] FOREIGN KEY ([PMMIdLookup]) REFERENCES [dbo].[PMM] ([ID])
);


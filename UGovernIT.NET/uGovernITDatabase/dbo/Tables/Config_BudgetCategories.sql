CREATE TABLE [dbo].[Config_BudgetCategories] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [AuthorizedToEdit]   NVARCHAR (250)  NULL,
    [AuthorizedToView]   NVARCHAR (250)  NULL,
    [BudgetAcronym]      NVARCHAR (250)  NULL,
    [BudgetCategoryName] NVARCHAR (250)  NULL,
    [BudgetCOA]          NVARCHAR (250)  NULL,
    [BudgetDescription]  NVARCHAR (MAX)  NULL,
    [BudgetSubCategory]  NVARCHAR (250)  NULL,
    [BudgetType]         NVARCHAR (250)  NULL,
    [BudgetTypeCOA]      NVARCHAR (250)  NULL,
    [CapitalExpenditure] BIT             NULL,
    [IncludesStaffing]   BIT             NULL,
    [Title]              VARCHAR (250)   NULL,
    [TenantID]           NVARCHAR (128)  NULL,
    [Created]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]           DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]     NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]            BIT             DEFAULT ((0)) NULL,
    [Attachments]        NVARCHAR (2000) DEFAULT ('') NULL
);










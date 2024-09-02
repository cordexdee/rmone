﻿CREATE TABLE [dbo].[Analytic_ModelOutputs]
(
	[ModelOutputID] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [ModelVersionID] BIGINT NULL, 
    [ModelInputID] BIGINT NULL, 
    [ModelInternalID] NVARCHAR(50) NULL, 
    [ModelName] NVARCHAR(250) NULL, 
    [RowScore] FLOAT NOT NULL, 
    [WeightedScore] FLOAT NOT NULL, 
    [CummulativeScore] FLOAT NOT NULL, 
    [Weight] FLOAT NOT NULL, 
    [CummulativeWeight] FLOAT NOT NULL,
    [TenantID]                        NVARCHAR (128)  NOT NULL,
    [Created]                         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                         BIT             DEFAULT (0) NULL,
    [Attachments]                     NVARCHAR (2000) DEFAULT ('') NULL
)
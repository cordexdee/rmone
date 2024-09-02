﻿CREATE TABLE [dbo].[ProcoreMapping](
	[Id] [bigint]  IDENTITY(1,1)  ,
	[Created] [datetime] NOT NULL DEFAULT getdate(),
	[Default] [bit] NULL,
	[FieldFrequency] [nvarchar](max) NULL,
	[FieldSetName] [nvarchar](max) NULL,
	[FieldSetType] [nvarchar](max) NULL,
	[InternalColumnName] [nvarchar](max) NULL,
	[Modified] [datetime] NOT NULL DEFAULT getdate(),
	[ModuleNameLookup] NVARCHAR(128) NULL,
	[ProcoreColumnName] [nvarchar](max) NULL,
	[ProcoreUtilityLookup] NVARCHAR(128) NULL,
	[Title] [nvarchar](128) NULL,
	[ModifiedBy] [nvarchar](128) NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
	[Attachments] NVARCHAR(500) NULL, 
	[Deleted] BIT NULL, 
[TenantID] [nvarchar](128) NULL,
);

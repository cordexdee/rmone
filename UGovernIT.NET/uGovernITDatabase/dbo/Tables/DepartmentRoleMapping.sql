
 
 
CREATE TABLE [dbo].[DepartmentRoleMapping](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DepartmentLookup] [bigint] NULL,
	[RoleLookup] [varchar](128) NULL,
	[BillingRate] [float] NULL,
	[TenantID] [varchar](128) NULL,
	[Deleted] [bit] NULL,
 CONSTRAINT [PK_DepartmentRoleMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

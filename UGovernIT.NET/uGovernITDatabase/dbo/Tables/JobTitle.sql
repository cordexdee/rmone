CREATE TABLE [dbo].[JobTitle] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Title]               NVARCHAR (250)  NULL,
    [DepartmentId]        BIGINT          NULL,
    [RoleId]              NVARCHAR (128)  NULL,
    [LowRevenueCapacity]  FLOAT (53)      DEFAULT ((0)) NULL,
    [HighRevenueCapacity] FLOAT (53)      DEFAULT ((0)) NULL,
    [LowProjectCapacity]  INT             DEFAULT ((0)) NULL,
    [HighProjectCapacity] INT             DEFAULT ((0)) NULL,
	[BillingLaborRate] float   null,
	[EmployeeCostRate] float null,
	[ResourceLevelTolerance] INT null,
    [TenantID]            NVARCHAR (128)  NULL,
    [Created]             DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]            DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]       NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]      NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]             BIT             DEFAULT ((0)) NULL,
    [Attachments]         NVARCHAR (2000) DEFAULT ('') NULL,
	[JobType]			  NVARCHAR (100) NULL
);



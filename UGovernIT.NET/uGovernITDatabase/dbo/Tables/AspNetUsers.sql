CREATE TABLE [dbo].[AspNetUsers] (
    [Id]                           NVARCHAR (128)  NOT NULL,
    [Email]                        NVARCHAR (256)  NULL,
    [EmailConfirmed]               BIT             NOT NULL,
    [PasswordHash]                 NVARCHAR (MAX)  NULL,
    [SecurityStamp]                NVARCHAR (MAX)  NULL,
    [PhoneNumber]                  NVARCHAR (MAX)  NULL,
    [PhoneNumberConfirmed]         BIT             NOT NULL,
    [TwoFactorEnabled]             BIT             NOT NULL,
    [LockoutEndDateUtc]            DATETIME        NULL,
    [LockoutEnabled]               BIT             NOT NULL,
    [AccessFailedCount]            INT             NOT NULL,
    [UserName]                     NVARCHAR (256)  NOT NULL,
    [Name]                         VARCHAR (100)   NULL,
    [EmployeeId]                   VARCHAR (100)   NULL,
    [DepartmentLookup]			   VARCHAR (100)   NULL,
    [HourlyRate]                   INT             NULL,
    [LocationLookup]                     VARCHAR (100)   NULL,
    [ManagerUser]                  VARCHAR (100)   NULL,
    [MobilePhone]                  VARCHAR (100)   NULL,
    [JobProfile]                   VARCHAR (100)   NULL,
    [IsIT]                         BIT             NULL,
    [IsConsultant]                 BIT             NULL,
    [IsManager]                    BIT             NULL,
    [LocationId]                   INT             NULL,
    [DepartmentId]                 INT             NULL,
    [Enabled]                      BIT             DEFAULT ((1)) NOT NULL,
    [FunctionalAreaLookup]         INT             NULL,
    [BudgetIdLookup]               INT             NULL,
    [DeskLocation]                 VARCHAR (100)   NULL,
    [UGITStartDate]                DATETIME        NULL,
    [UGITEndDate]                  DATETIME        NULL,
    [EnablePasswordExpiration]     BIT             NULL,
    [PasswordExpiryDate]           DATETIME        NULL,
    [DisableWorkflowNotifications] BIT             NULL,
    [Picture]                      VARCHAR (500)   NULL,
    [NotificationEmail]            VARCHAR (256)   NULL,
    [ApproveLevelAmount]           FLOAT (53)      NULL,
    [LeaveFromDate]                DATETIME        NULL,
    [LeaveToDate]                  DATETIME        NULL,
    [EnableOutofOffice]            BIT             NULL,
    [ManagerIDUser]                NVARCHAR (100)  NULL,
    [DelegateUserOnLeave]          VARCHAR (500)   NULL,
    [DelegateUserFor]              VARCHAR (500)   NULL,
    [isRole]                       BIT             DEFAULT ((1)) NOT NULL,
    [UserSkillLookup]              NVARCHAR (MAX)  NULL,
    [WorkingHoursStart]            DATETIME        NULL,
    [WorkingHoursEnd]              DATETIME        NULL,
    [GlobalRoleID]                 NVARCHAR (256)  NULL,
    [UserRoleIdLookup]			   NVARCHAR (128)  NULL,
    [JobTitleLookup]               BIGINT          DEFAULT ((0)) NULL,
    [TenantID]                     NVARCHAR (128)  NULL,
    [Created]                      DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                      BIT             DEFAULT ((0)) NULL,
    [Attachments]                  NVARCHAR (2000) DEFAULT ('') NULL,
    [IsShowDefaultAdminPage]       BIT             DEFAULT ((1)) NULL,
    [IsDefaultAdmin]               INT             DEFAULT ((0)) NULL,
	[History]                NVARCHAR (MAX)  NULL,
    [EmployeeType] VARCHAR(MAX) NULL, 
    [Resume] nvarchar(500) null,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUsers_dbo.UserRoles_Id] FOREIGN KEY ([UserRoleIdLookup]) REFERENCES [dbo].[LandingPages] ([Id]) ON DELETE CASCADE
);






GO

GO

GO
/****** Object:  Index [UserNameIndex]    Script Date: 3/13/2017 5:14:58 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[UserName] ASC,
	[TenantID]
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
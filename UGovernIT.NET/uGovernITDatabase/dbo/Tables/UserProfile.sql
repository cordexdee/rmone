CREATE TABLE [dbo].[UserProfile] (
    [ID]                           VARCHAR (128)   NULL,
    [LoginName]                    VARCHAR (100)   NULL,
    [Name]                         VARCHAR (100)   NULL,
    [Email]                        VARCHAR (256)   NULL,
    [Department]                   VARCHAR (100)   NULL,
    [HourlyRate]                   INT             NULL,
    [Location]                     VARCHAR (100)   NULL,
    [ManagerUser]                  VARCHAR (100)   NULL,
    [MobilePhone]                  VARCHAR (100)   NULL,
    [JobProfile]                   VARCHAR (100)   NULL,
    [IsIT]                         BIT             NULL,
    [IsConsultant]                 BIT             NULL,
    [IsManager]                    BIT             NULL,
    [LocationId]                   INT             NULL,
    [DepartmentId]                 INT             NULL,
    [RoleId]                       INT             NULL,
    [RoleNameChoice]               VARCHAR (100)   NULL,
    [Enabled]                      BIT             NULL,
    [FunctionalArea]               INT             NULL,
    [BudgetCategory]               INT             NULL,
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
    [TenantID]                     NVARCHAR (128)  NULL,
    [Created]                      DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                     DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]               NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                      BIT             DEFAULT ((0)) NULL,
    [Attachments]                  NVARCHAR (2000) DEFAULT ('') NULL,
    [UserRoleId]                   NVARCHAR (128)  NULL,
    [IsDefaultAdmin]               INT             DEFAULT ((0)) NULL
);








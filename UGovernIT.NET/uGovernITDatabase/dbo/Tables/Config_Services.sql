CREATE TABLE [dbo].[Config_Services] (
    [ID]                            BIGINT          IDENTITY (1, 1) NOT NULL,
    [AllowServiceTasksInBackground] BIT             NULL,
    [AttachmentRequired]            NVARCHAR (MAX)  NULL,
    [AttachmentsInChildTickets]     BIT             DEFAULT ((0)) NULL,
    [AuthorizedToView]              NVARCHAR (250)  NULL,
    [ConditionalLogic]              NVARCHAR (MAX)  NULL,
    [CreateParentServiceRequest]    BIT             NULL,
    [IncludeInDefaultData]          BIT             DEFAULT ((0)) NULL,
    [CustomProperties]              NVARCHAR (MAX)  NULL,
    [HideSummary]                   BIT             NULL,
    [HideThankYouScreen]            BIT             NULL,
    [ImageUrl]                      NVARCHAR (250)  NULL,
    [IsActivated]                   BIT             NULL,
    [ItemOrder]                     INT             NULL,
    [LoadDefaultValue]              BIT             NULL,
    [ModuleNameLookup]              NVARCHAR (250)  NULL,
    [ModuleStage]                   NVARCHAR (250)  NULL,
    [NavigationUrl]                 NVARCHAR (MAX)  NULL,
    [OwnerUser]                     NVARCHAR (250)  NULL,
    [OwnerApprovalRequired]         BIT             NULL,
    [QuestionMapVariables]          NVARCHAR (MAX)  NULL,
    [SectionConditionalLogic]       NVARCHAR (MAX)  NULL,
    [CategoryId]                    BIGINT          NULL,
    [ServiceCategoryType]           NVARCHAR (250)  NULL,
    [ServiceDescription]            NVARCHAR (MAX)  NULL,
    [ShowStageTransitionButtons]    BIT             NULL,
    [StagingId]                     INT             NULL,
    [Title]                         VARCHAR (250)   NULL,
    [ServiceType]                   VARCHAR (250)   NULL,
    [TenantID]                      NVARCHAR (128)  NULL,
    [Created]                       DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]                      DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]                 NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]                NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]                       BIT             DEFAULT ((0)) NULL,
    [Attachments]                   NVARCHAR (2000) DEFAULT ('') NULL,
    [SLADisabled]                   BIT             DEFAULT ((0)) NOT NULL,
    [ResolutionSLA]                 FLOAT (53)      NULL,
    [CompletionMessage]             NVARCHAR (2000) NULL,
    [Use24x7Calendar]               BIT             DEFAULT ((0)) NOT NULL,
    [EnableTaskReminder]            BIT             DEFAULT ((0)) NOT NULL,
    [Reminders]                     NVARCHAR (MAX)  NULL,
    [NavigationType]    	    NVARCHAR (250)  NULL,
    [StartResolutionSLAFromAssigned] BIT NOT NULL DEFAULT ((0)), 
    [SLAConfiguration] NVARCHAR(MAX) NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Config_Service_ServiceCategories] ([ID])
);








GO
CREATE NONCLUSTERED INDEX [IX_Config_Services_ModuleName]
    ON [dbo].[Config_Services]([ModuleNameLookup] ASC);



GO
CREATE NONCLUSTERED INDEX [IX_Config_Services_Deleted] ON [dbo].[Config_Services] ([Deleted])
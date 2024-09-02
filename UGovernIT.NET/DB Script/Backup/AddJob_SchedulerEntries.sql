
INSERT [dbo].[TenantScheduler] ([JobType], [JobInterVal], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [CronExpression]) 
VALUES (N'RefreshResoureSummarySchedular', 0, N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2020-07-01T00:07:09.483' AS DateTime), CAST(N'2020-07-01T00:07:09.483' AS DateTime), N'e92254f5-a490-46c2-b65e-f4011ede51c4', N'e92254f5-a490-46c2-b65e-f4011ede51c4', 0, N'0', N'0 * * * *')
GO
INSERT [dbo].[TenantScheduler] ([JobType], [JobInterVal], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [CronExpression]) 
VALUES (N'UpdateERPJobIDSchedular', 0, N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2020-07-01T00:07:09.483' AS DateTime), CAST(N'2020-07-01T00:07:09.483' AS DateTime), N'e92254f5-a490-46c2-b65e-f4011ede51c4', N'e92254f5-a490-46c2-b65e-f4011ede51c4', 0, N'0', N'*/5 * * * *')
GO
INSERT [dbo].[Config_ClientAdminConfigurationLists] ([ClientAdminCategoryLookup], [Description], [ListName], [TabSequence], [Title], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [AuthorizedToViewUser]) 
VALUES (752, N'Redirects to job scheduler dashboard page', N'SchedulerJob', 38, N'Schedule Jobs', N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2021-02-23T22:11:09.940' AS DateTime), CAST(N'2021-02-23T22:11:09.940' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0, NULL, NULL)



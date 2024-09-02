
 


SET IDENTITY_INSERT [dbo].[TenantScheduler] ON 
GO
INSERT [dbo].[TenantScheduler] ([Id], [JobType], [JobInterVal], [TenantID], [Created], [Modified], [CreatedByUser], [ModifiedByUser], [Deleted], [Attachments], [CronExpression]) VALUES (1, N'CleanupJobScheduler', 0, N'35525396-e5fe-4692-9239-4df9305b915b', CAST(N'2024-03-28T18:00:32.480' AS DateTime), CAST(N'2024-03-28T18:00:32.480' AS DateTime), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000', 0, N'', N'0 4 * * *')
GO
SET IDENTITY_INSERT [dbo].[TenantScheduler] OFF
GO

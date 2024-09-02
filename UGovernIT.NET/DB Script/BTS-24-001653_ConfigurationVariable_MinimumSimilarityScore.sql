
INSERT INTO [dbo].[Config_ConfigurationVariable]
           ([CategoryName]
           ,[Description]
           ,[KeyName]
           ,[KeyValue]
           ,[Title]
           ,[Type]
           ,[Internal]
           ,[TenantID]
           ,[Created]
           ,[Modified]
           ,[CreatedByUser]
           ,[ModifiedByUser]
           ,[Deleted]
           ,[Attachments])
     VALUES
           ('Project Mgt'
           ,'Filter the projects by equal to OR greater than Minimum score.'
           ,'MinimumSimilarityScore'
           ,'30'
           ,'MinimumSimilarityScore'
           ,'Text'
           ,0
           ,'35525396-E5FE-4692-9239-4DF9305B915B'
           ,GETDATE()
           ,GETDATE()
           ,'00000000-0000-0000-0000-000000000000'
           ,'00000000-0000-0000-0000-000000000000'
           ,0
           ,null)
GO
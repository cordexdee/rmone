INSERT INTO [dbo].[Config_ClientAdminConfigurationLists]
           ([ClientAdminCategoryLookup]
           ,[Description]
           ,[ListName]
           ,[TabSequence]
           ,[Title]
           ,[TenantID]
           ,[Created]
           ,[Modified]
           ,[CreatedByUser]
           ,[ModifiedByUser]
           ,[Deleted]
           ,[Attachments]
           )
     VALUES
           (10
           ,'List of phrases for similar Ticket search'
           ,'phrasesview'
           ,11
           ,'Phrases'
           ,'c345e784-aa08-420f-b11f-2753bbebfdd5'
           ,getdate()
           ,getdate()
           ,'00000000-0000-0000-0000-000000000000'
           ,'00000000-0000-0000-0000-000000000000'
           ,0
           ,''
           )




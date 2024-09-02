
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
           ,'Each item is separated by `;` It is the combimation of Hex Color and Score Max Value (Min value can be 0 OR the previous item Max Value).'
           ,'ProjectComparisonMatrixColor'
           ,'#FF5757=20;#FFEF5F=40;#96DF56=80;#6BA538=100'
           ,'ProjectComparisonMatrixColor'
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
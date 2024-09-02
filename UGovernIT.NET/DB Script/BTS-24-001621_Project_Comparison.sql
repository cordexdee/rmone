ALTER table [dbo].[ProjectSimilarityConfig]
Add MetricType nvarchar(100)

GO

UPDATE [dbo].[ProjectSimilarityConfig] SET MetricType = 'Similarity'

GO

UPDATE [core2].[dbo].[Config_ClientAdminConfigurationLists]
  SET Title =  'Project Comparison', Description = 'Project Comparison' WHERE ListName = 'ProjectSimilarityConfig' AND Description = 'Project Similarity'
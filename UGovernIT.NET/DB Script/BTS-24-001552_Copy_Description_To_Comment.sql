
UPDATE [dbo].[CRMProject]
SET COMMENT = CASE
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')='' THEN COMMENT
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')<>''
                       AND ISNULL(LTRIM(RTRIM(COMMENT)), '')='' THEN '44380d17-c887-488c-856b-31753e4197b7;#UTC:'+FORMAT (GETUTCDATE(), 'G')+';#'+ Description +';#False'
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')<>''
                       AND ISNULL(LTRIM(RTRIM(COMMENT)), '')<>'' THEN COMMENT + '<;#>44380d17-c887-488c-856b-31753e4197b7;#UTC:'+FORMAT (GETUTCDATE(), 'G')+';#'+ Description +';#False'
              END

UPDATE [dbo].[CRMServices]
SET COMMENT = CASE
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')='' THEN COMMENT
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')<>''
                       AND ISNULL(LTRIM(RTRIM(COMMENT)), '')='' THEN '44380d17-c887-488c-856b-31753e4197b7;#UTC:'+FORMAT (GETUTCDATE(), 'G')+';#'+ Description +';#False'
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')<>''
                       AND ISNULL(LTRIM(RTRIM(COMMENT)), '')<>'' THEN COMMENT + '<;#>44380d17-c887-488c-856b-31753e4197b7;#UTC:'+FORMAT (GETUTCDATE(), 'G')+';#'+ Description +';#False'
              END
              
UPDATE [dbo].[Opportunity]
SET COMMENT = CASE
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')='' THEN COMMENT
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')<>''
                       AND ISNULL(LTRIM(RTRIM(COMMENT)), '')='' THEN '44380d17-c887-488c-856b-31753e4197b7;#UTC:'+FORMAT (GETUTCDATE(), 'G')+';#'+ Description +';#False'
                  WHEN ISNULL(LTRIM(RTRIM(Description)), '')<>''
                       AND ISNULL(LTRIM(RTRIM(COMMENT)), '')<>'' THEN COMMENT + '<;#>44380d17-c887-488c-856b-31753e4197b7;#UTC:'+FORMAT (GETUTCDATE(), 'G')+';#'+ Description +';#False'
              END
CREATE TABLE [dbo].[DMSTenant_Documents_Details]
(
	[ID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID] NVARCHAR(128) NOT NULL, 
    [AppID] NVARCHAR(128) NULL, 
    [AppSecret] NVARCHAR(128) NULL, 
    [AWSBucket] NVARCHAR(128) NULL, 
    [AWSProfileName] NVARCHAR(128) NULL, 
    [AWSAccessKey] NVARCHAR(128) NULL, 
    [AWSSecretKey] NVARCHAR(128) NULL, 
    [Is_Deleted] BIT NULL, 
    [CreatedDate] DATETIME NULL, 
    [ModifiedDate] DATETIME NULL,
	CONSTRAINT [PK_DMSTenant_Documents_Details] PRIMARY KEY ([ID])
)

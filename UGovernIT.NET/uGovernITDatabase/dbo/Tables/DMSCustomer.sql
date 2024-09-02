
CREATE TABLE [dbo].[DMSCustomer] (
    [CustomerId]    INT            IDENTITY (1, 1) NOT NULL,
    [CustomerName]  NVARCHAR (150) NOT NULL,
    [CreatedByUser] NVARCHAR (128) NOT NULL,
    [UpdatedBy]     NVARCHAR (128) NOT NULL,
    [CreatedOn]     DATETIME       NULL,
    [UpdatedOn]     DATETIME       NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerId] ASC)
);



GO
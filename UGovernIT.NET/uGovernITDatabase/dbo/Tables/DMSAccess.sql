

CREATE TABLE [dbo].[DMSAccess] (
    [AccessId]      INT            IDENTITY (1, 1) NOT NULL,
    [AccessType]    NCHAR (125)    NOT NULL,
    [CreatedByUser] NVARCHAR (128) NOT NULL,
    [UpdatedBy]     NVARCHAR (128) NOT NULL,
    [CreatedOn]     DATETIME       NULL,
    [UpdatedOn]     DATETIME       NULL,
    CONSTRAINT [PK_Access] PRIMARY KEY CLUSTERED ([AccessId] ASC)
);



GO
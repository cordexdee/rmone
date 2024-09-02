CREATE TABLE [dbo].[WikiLinks] (
    [ID]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [Comments]        NVARCHAR (MAX)  NULL,
    [DocIcon]         NVARCHAR (250)  NULL,
    [Edit]            NVARCHAR (250)  NULL,
    [LinkTitle]       NVARCHAR (250)  NULL,
    [LinkTitleNoMenu] NVARCHAR (250)  NULL,
    [TicketId]        NVARCHAR (250)  NULL,
    [URL]             NVARCHAR (250)  NULL,
    [URLNoMenu]       NVARCHAR (250)  NULL,
    [URLwMenu]        NVARCHAR (250)  NULL,
    [URLwMenu2]       NVARCHAR (250)  NULL,
    [WikiID]          NVARCHAR (250)  NULL,
    [WikiLinkTitle]   NVARCHAR (250)  NULL,
    [Title]           VARCHAR (250)   NULL,
    [TenantID]        NVARCHAR (128)  NULL,
    [Created]         DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified]        DATETIME        DEFAULT (getdate()) NOT NULL,
    [CreatedByUser]   NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [ModifiedByUser]  NVARCHAR (128)  DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Deleted]         BIT             DEFAULT ((0)) NULL,
    [Attachments]     NVARCHAR (2000) DEFAULT ('') NULL
);






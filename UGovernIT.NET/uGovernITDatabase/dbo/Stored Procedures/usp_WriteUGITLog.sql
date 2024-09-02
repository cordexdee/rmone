CREATE PROCEDURE [dbo].[usp_WriteUGITLog]
	@UserId nvarchar(250),
	@Description nvarchar(MAX),
	@CategoryName nvarchar(250),
	@ModuleNameLookup nvarchar(250),
	@Severity nvarchar(250),
	@TicketId nvarchar(250),
	@TenantID nvarchar(250)
AS
BEGIN
	DECLARE @Name nvarchar(250);
	select @Name = [Name] from AspNetUsers where Id = @UserId

	INSERT INTO [Log]
           ([CategoryName]
           ,[Description]
           ,[ItemUser]
           ,[ModuleNameLookup]
           ,[Severity]
           ,[TicketId]
           ,[Title]
           ,[TenantID]
           ,[Created]
           ,[Modified]
           ,[CreatedByUser]
           ,[ModifiedByUser]
           ,[Deleted]
           ,[Attachments])
     VALUES
           (@CategoryName
           ,@Description
           ,@Name
           ,@ModuleNameLookup
           ,@Severity
           ,@TicketId
           ,CONVERT(VARCHAR, GETDATE(), 0)
           ,@TenantID
           ,GETDATE()
           ,GETDATE()
           ,@UserId
           ,@UserId
           ,0
           ,'')

END

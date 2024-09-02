---exec usp_CreateSuperAdmin  '3f69dd83-7b92-4215-94c3-9fbff24cb759'
create proc usp_CreateSuperAdmin
(
@TenantId nvarchar(128)
)
AS
BEGIN

declare @i as integer
declare @total as integer
--declare @TenantId nvarchar(150)
declare @UserName nvarchar(150)
declare @NewUserId nvarchar(128)
declare @AdminUserId nvarchar(128)

declare @tmpTenants table
(ID int identity,
TblID uniqueidentifier,
TenantId nvarchar(150)
)

insert into @tmpTenants (TblID, TenantId) select TenantID, TenantID from [ugovernit_common].dbo.Tenant where TenantID =@TenantId;

select @total = COUNT(*) from @tmpTenants
set @i = 1
while @i <= @total
begin
 select @TenantId = TenantId from @tmpTenants where Id = @i

set @UserName = 'SuperAdmin';
IF NOT EXISTS (select ID from AspNetUsers where UserName = @UserName and TenantID = @TenantID)
BEGIN
	Set @NewUserId = NEWID();

		INSERT INTO [dbo].[AspNetUsers]
				   ([Id]
				   ,[Email]
				   ,[EmailConfirmed]
				   ,[PasswordHash]
				   ,[SecurityStamp]
				   ,[PhoneNumberConfirmed]
				   ,[TwoFactorEnabled]
				   ,[LockoutEnabled]
				   ,[AccessFailedCount]
				   ,[UserName]
				   ,[Name]
				   ,[HourlyRate]
				   ,[IsIT]
				   ,[IsConsultant]
				   ,[IsManager]
				   ,[LocationId]
				   ,[DepartmentId]
				   ,[Enabled]
				   ,[UGITStartDate]
				   ,[UGITEndDate]
				   ,[EnablePasswordExpiration]
				   ,[PasswordExpiryDate]
				   ,[DisableWorkflowNotifications]
				   ,[Picture]
				   ,[ApproveLevelAmount]
				   ,[LeaveFromDate]
				   ,[LeaveToDate]
				   ,[EnableOutofOffice]
				   ,[isRole]
				   ,[WorkingHoursStart]
				   ,[WorkingHoursEnd]
				   ,[TenantID]
				   ,[Created]
				   ,[Modified]
				   ,[CreatedByUser]
				   ,[ModifiedByUser]
				   ,[Deleted]
				   ,[Attachments]
				   ,[JobTitleLookup]
				   ,[IsDefaultAdmin]
				   )
			 VALUES
				   (@NewUserId
				   ,'support@ugovernit.com'
				   ,0
				   ,'AJnqGbx9wiCvNuQzodccHt9JnHvSnLD27LM3pFUeslcC6LbJB7pzrQyyw4H/T6fSLg=='
				   ,'d3cd47d8-3fc0-44e3-8e87-63ad030525bf'
				   ,0
				   ,0
				   ,0
				   ,0
				   ,@UserName
				   ,'Super Admin'
				   ,0
				   ,0
				   ,0
				   ,0
				   ,0
				   ,0
				   ,1
				   ,'1754-01-01 00:00:00.000'
				   ,'1754-01-01 00:00:00.000'
				   ,0
				   ,'2020-04-07 02:03:05.567'
				   ,0
				   ,'/Content/Images/userNew.png'
				   ,0
				   ,'1754-01-01 00:00:00.000'
				   ,'1754-01-01 00:00:00.000'
				   ,0
				   ,0
				   ,'1900-01-01 09:00:00.000'
				   ,'1900-01-01 17:00:00.000'
				   ,@TenantID
				   ,GETDATE()
				   ,GETDATE()
				   ,'00000000-0000-0000-0000-000000000000'
				   ,'00000000-0000-0000-0000-000000000000'
				   ,0
				   ,''
				   ,0
				   ,0
				   );

				  Select @AdminUserId = Id from AspNetRoles where [Name] = 'Admin' and TenantID = @TenantID;
				  IF NOT EXISTS (select * from AspNetUserRoles where RoleId = @AdminUserId and UserId = @NewUserId and TenantID = @TenantID)
					BEGIN
						INSERT INTO [dbo].[AspNetUserRoles]
							   ([UserId]
							   ,[RoleId]
							   ,[TenantID]
							   ,[Created]
							   ,[Modified]
							   ,[CreatedByUser]
							   ,[ModifiedByUser]
							   ,[Deleted]
							   ,[Attachments])
						 VALUES
							   (@NewUserId
							   ,@AdminUserId
							   ,@TenantID
							   ,GETDATE()
							   ,GETDATE()
							   ,'00000000-0000-0000-0000-000000000000'
							   ,'00000000-0000-0000-0000-000000000000'
							   ,0
							   ,'')
					END

END


set @i = @i + 1
end

END
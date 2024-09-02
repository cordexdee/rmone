    
ALTER PROCEDURE [dbo].[usp_TenantDefaultConfiguration]  --'bd3de8cd-e224-43c4-ac0f-1d80baa58fc8','LiRO' ,1  
 @TenantId uniqueidentifier,    
 @AccountId nvarchar(100),    
 @ResetSequence bit    
AS    
BEGIN    
 If @ResetSequence = '1'    
 Begin    
  update Config_Modules set LastSequence = 0, LastSequenceDate= getdate() where tenantid = @TenantId;    
  update Config_ConfigurationVariable set KeyValue = '0' where KeyName = 'LastSequenceProjectID' and TenantID = @TenantId;    
 End;    
    
 update Config_ConfigurationVariable set KeyValue = 'True' where KeyName = 'SendEmail' and TenantID = @TenantId;    
 --update Config_ConfigurationVariable set KeyValue = 'False' where KeyName = 'EnableDivision' and TenantID = @TenantId;    
    
 update Config_ConfigurationVariable set KeyValue = 'RMOne' where KeyName = 'DisplayName' and TenantID = @TenantId;    
 update Config_ConfigurationVariable set KeyValue = 'Thank You, <br /><b>RMOne</b><br />' where KeyName = 'Signature' and TenantID = @TenantId;    
    
 update Config_ConfigurationVariable set KeyValue = @AccountId where KeyName = 'CompanyEmailKey' and TenantID = @TenantId;    
 update Config_ConfigurationVariable 
 set KeyValue = '<html>
	<head></head>
	<body>
		<p> Dear [$name$], <br />You have been added as a user of '+@AccountId+'. <br /><br />You will be provided with introductory information. <br><br> This email contains your credentials: <strong> [$userCredentials$] </strong>
			<br /><br />
			Please <a href=''[$SiteUrl$]''>click here </a> to activate your account. Once logged in you can change your password by clicking on your name at the upper right and click "Change Password".
		</p>
		<p>
			<br />Thank you,<br> [$CurrentUserName$]<br />'+@AccountId+' Information Technology.
		</p>
	</body>
</html>' 
 where KeyName = 'CreateUserMailBody' and TenantID = @TenantId;   
 
 update Config_ConfigurationVariable set KeyValue = 'Welcome to '+@AccountId+'!' where KeyName = 'CreateUserMailSubject' and TenantID = @TenantId;    

 update Config_ConfigurationVariable set KeyValue = (select Email from AspNetUsers where UserName = 'Administrator_' + @AccountId and TenantID = @TenantID) where KeyName = 'SupportEmail' and TenantID = @TenantId;    

 update Config_ConfigurationVariable set KeyValue = '@test.com' where KeyName = 'AdfsDomainName' and TenantID = @TenantId; 
 
 update Config_ConfigurationVariable set KeyValue = '/content/images/RMONE/rm-one-logo.png' where KeyName = 'ReportLogo' and TenantID = @TenantId;    

 update Config_ConfigurationVariable    
 set KeyValue =    
  (    
   select id    
   from Config_Dashboard_DashboardPanelView    
   where viewName = 'ticketsummary'    
      and TenantID = @TenantId    
  )    
 where TenantID = @TenantId    
    and KeyName in ( 'OpenTicketsChart', 'ClosedTicketsChart' );    
    
 update Config_ConfigurationVariable    
 set KeyValue =    
 (    
  select id    
  from Config_Dashboard_DashboardPanelView    
  where viewName = 'ProjectSummary'    
    and TenantID = @TenantId    
 )    
 where TenantID = @TenantId    
  and KeyName in ( 'OpenProjectsChart', 'ClosedProjectsChart' );    
    
 Declare @ClientType nvarchar(100);    
 Declare @AdminId nvarchar(128);    
    
 select @AdminId = Id from AspNetUsers where UserName = 'Administrator_' + @AccountId and TenantID = @TenantID; 
 
 update AspNetUsers set UserRoleIdLookup=(select id from LandingPages where Name='Admin' and TenantID=@TenantId) where Id=@AdminId and TenantID=@TenantId


    
 
END  
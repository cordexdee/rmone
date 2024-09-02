CREATE PROCEDURE [dbo].[GetTimeSheetSignOffList]  
@TenantID varchar(128)= '2e4d74d8-b163-4de1-8154-63cc2b8b87ef',  
@ManagerId varchar(128)= '5c37e450-64b6-4cb2-895f-a2019531919d',  
@Resource nvarchar(128)= '',  
@StartDate varchar(10)= '', --'2022-10-03',  
@SignOffStatus nvarchar(100) = ''   -- 'Approved'  
AS  
BEGIN  
 Declare @EnableProjStdWorkItems bit;  
 select @EnableProjStdWorkItems = KeyValue from Config_ConfigurationVariable where TenantID = @TenantID  
 and keyname = 'EnableProjStdWorkItems'  
  
   
 create table #tmpSignOffList  
 (  
  UserId nvarchar(128),  
  Name nvarchar(1000),  
  UserName nvarchar(1000),  
  Picture nvarchar(1000),  
  StartDate nvarchar(30),  
  EndDate nvarchar(30),  
  SignOffStatus nvarchar(50),  
  Modified nvarchar(30),  
  TotalHours int,  
  NoofWorkItems int  
 )  
  
 Insert into #tmpSignOffList (UserId, Name, UserName, Picture, StartDate, EndDate, SignOffStatus, Modified)  
 select u.Id, u.Name, u.UserName, u.Picture, Convert(varchar(10), rt.StartDate,120) StartDate, Convert(varchar(10), rt.EndDate,120) EndDate, rt.SignOffStatus, Convert(varchar(10), rt.Modified,120) Modified  from ResourceTimeSheetSignOff rt  
 join AspNetUsers u on u.Id = rt.ResourceUser  
 where rt.TenantID = @TenantID  
 and u.ManagerUser = @ManagerId  
 and  isnull(rt.ResourceUser,'') =CASE WHEN LEN(@Resource)=0 then isnull(rt.ResourceUser,'')  else @Resource END  
 and  isnull(rt.SignOffStatus,'') =CASE WHEN LEN(@SignOffStatus)=0 then isnull(rt.SignOffStatus,'')  else @SignOffStatus END  
 and  convert(date, rt.StartDate,121) =CASE WHEN LEN(@StartDate)=0 then convert(date, rt.StartDate,121)  else @StartDate END;  
  
 If @EnableProjStdWorkItems = 0  
  Begin  
   update #tmpSignOffList  set TotalHours = (select sum(HoursTaken) from ResourceTimeSheet  where TenantID = @TenantID  
   and WorkDate between StartDate and EndDate and ResourceUser = UserId),  
   NoofWorkItems = (select count(distinct ResourceWorkItemLookup) from ResourceTimeSheet  where TenantID = @TenantID  
   and WorkDate between StartDate and EndDate and ResourceUser = UserId);  
  End   
 Else  
  Begin  
   update #tmpSignOffList  set TotalHours = (select sum(HoursTaken) from TicketHours  where TenantID = @TenantID  
   and WorkDate between StartDate and EndDate and ResourceUser = UserId),  
   NoofWorkItems = (select count(distinct WorkItem) from TicketHours  where TenantID = @TenantID  
   and WorkDate between StartDate and EndDate and ResourceUser = UserId);  
  End  
  
 select Name, UserName,Picture, StartDate, EndDate, SignOffStatus, Modified, TotalHours, NoofWorkItems from #tmpSignOffList;  
  
 drop table #tmpSignOffList;  
END  
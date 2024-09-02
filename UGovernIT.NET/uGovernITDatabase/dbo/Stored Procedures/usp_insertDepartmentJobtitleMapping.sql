 ALTER Procedure usp_insertDepartmentJobtitleMapping      
 @RoleId varchar(128)='',@DepartmentId int=0,@JobTitleId int=0,      
 @EmpCostRate float=0,@TenantID varchar(128) ,    
 @Deleted bit ,
 @id int=0
 As      
 Begin      
 Declare @Flag int=0      
 SET @Flag= (Select count(1) from JobTitleCostRateByDept where JobtitleLookup= @JobTitleId      
  and DeptLookup=@DepartmentId and TenantID=@TenantID)      
      
 If(@id=0)      
 Begin      
 INSERT INTO [dbo].[JobTitleCostRateByDept]        
    ([DeptLookup],[JobtitleLookup],[EmpCostRate],[TenantID],[Deleted])        
 values (@DepartmentId,@JobTitleId,@EmpCostRate,@TenantID,@Deleted)        
      
 --Update AspNetUsers Set GlobalRoleId=@RoleId where TenantID=@TenantID and JobTitleLookup=@JobTitleId        
 End      
 Else Begin      
 
 update [JobTitleCostRateByDept] set DeptLookup= @DepartmentId,        
 EmpCostRate= @EmpCostRate, Deleted=@Deleted where TenantID=@TenantID        
and JobtitleLookup= @JobTitleId and Id=@id;        
--Update AspNetUsers Set GlobalRoleId=@RoleId where TenantID=@TenantID and JobTitleLookup=@JobTitleId;        
      
 End      
 END      
      
       
      
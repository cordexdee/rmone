CREATE Procedure [dbo].[usp_GetJobtitlebyDept]                
@TenantID varchar(128),                
@Dept varchar(100) ='0',            
@RoleId varchar(128)=''            
as
Begin              
If(@RoleId!='' and @Dept='0')            
begin            
	Select j.ID ,j.Title,m.DeptLookup DepartmentId  from JobTitle j            
	left join            
	(Select DeptLookup,JobtitleLookup from JobTitleCostRateByDept where            
	TenantID=@TenantID) m on m.JobtitleLookup=j.ID            
	where j.TenantID=@TenantID            
End            
Else IF (@RoleId!='' and @Dept!='0')            
Begin
	Select j.ID ,j.Title,m.DeptLookup DepartmentId  from JobTitle j            
	inner join            
	(Select DeptLookup,JobtitleLookup from JobTitleCostRateByDept             
	where TenantID=@TenantID and DeptLookup IN (SELECT value FROM STRING_SPLIT( @Dept , ',')))  m on m.JobtitleLookup=j.ID            
	where j.TenantID=@TenantID            
End            
Else Begin            
	Select ID ,Title from JobTitle where TenantID=@TenantID                
	and Id in (Select JobtitleLookup from JobTitleCostRateByDept where DeptLookup IN (SELECT value FROM STRING_SPLIT( @Dept , ','))
	and TenantID=@TenantID)                
End            
End  
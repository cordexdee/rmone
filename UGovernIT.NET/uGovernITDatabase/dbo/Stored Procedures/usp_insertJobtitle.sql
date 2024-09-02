      
ALTER PROCEDURE [dbo].[usp_insertJobtitle]      
 @Id int =0,      
 @Title [varchar](500) NULL,
 @Shortname [varchar](500) NULL,
 @JobType [nvarchar](20) NULL,      
 @LowProjectCapacity int NULL,      
 @HighProjectCapacity int NULL,      
 @LowRevenueCapacity int NULL,      
 @HighRevenueCapacity int NULL,      
 @RoleId varchar(128) null,  
      
 @ResourceLevelTolerance int NULL,      
 @Deleted bit=0,      
 @TenantID [nvarchar](128) NULL      
AS      
BEGIN      
if(@Id=0)      
Begin      
Declare @JobTitleId int =0;       
 INSERT INTO [dbo].[JobTitle]      
           ([Title],[ShortName],[JobType],[RoleId],[LowProjectCapacity],[HighProjectCapacity],[LowRevenueCapacity],[HighRevenueCapacity]      
           ,[ResourceLevelTolerance],[Deleted],[TenantID])      
     VALUES      
           (@Title,@Shortname,@JobType,@RoleId,@LowProjectCapacity,@HighProjectCapacity,@LowRevenueCapacity,@HighRevenueCapacity,      
     @ResourceLevelTolerance,@Deleted,@TenantID)      
     
End      
Else Begin      
Update [JobTitle]      
SET  [Title]=@Title,[JobType]=@JobType,[ShortName]=@Shortname,[LowProjectCapacity]= @LowProjectCapacity,[HighProjectCapacity]=@HighProjectCapacity,      
[LowRevenueCapacity]=@LowRevenueCapacity,[HighRevenueCapacity]=@HighRevenueCapacity,      
[ResourceLevelTolerance]=@ResourceLevelTolerance,[Deleted]=@Deleted ,  
RoleId=@RoleId  
Where [TenantID]=@TenantID      
and ID=@Id;      
  
 Update AspNetUsers Set GlobalRoleId=(   
 Select RoleId from JobTitle where TenantID=@TenantID  
 and id =@Id)  
 where TenantID=@TenantID and JobTitleLookup=@Id   
End      
END  
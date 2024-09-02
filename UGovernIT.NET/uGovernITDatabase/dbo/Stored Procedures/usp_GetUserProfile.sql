CREATE Procedure [dbo].[usp_GetUserProfile]  
-- usp_GetUserProfile 'c345e784-aa08-420f-b11f-2753bbebfdd5'  
@TenantId varchar(max)  
as  
Begin  
exec usp_GetAspNetUsers @TenantId
End

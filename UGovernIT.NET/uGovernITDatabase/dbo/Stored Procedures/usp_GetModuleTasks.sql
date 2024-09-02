CREATE PROCEDURE [dbo].[usp_GetModuleTasks]
@TenantID VARCHAR(MAX),  
@TicketId VARCHAR(MAX)='', 
@ModuleName VARCHAR(100)
AS
BEGIN
	--SET NOCOUNT ON;
	SELECT a.*
	,[dbo].[fnGetusername](a.ApproverUser,@TenantID)[ApproverUser$]
	,[dbo].[fnGetusername](a.AssignedToUser,@TenantID)[AssignedToUser$]
	,[dbo].[fnGetusername](a.GroupUser,@TenantID)[GroupUser$]
	,[dbo].[fnGetusername](a.CreatedByUser,@TenantID)[CreatedByUser$]
	,[dbo].[fnGetusername](a.ModifiedByUser,@TenantID)[ModifiedByUser$]
	,h.requesttype[RequestTypeLookup$]
	,umsk.Title[UserSkillMultiLookup$]
	FROM ModuleTasks (READCOMMITTED) a   
	left join Config_Module_RequestType (READCOMMITTED) h on isnull(h.ID,'')= isnull(a.RequestTypeLookup,'') and h.TenantID=@TenantID   
	left join UserSkills (READCOMMITTED) umsk on isnull(umsk.ID,'')=ISNULL(a.UserSkillMultiLookup,'') and umsk.TenantID=@TenantID
	where a.TenantID=@TenantID and a.ModuleNameLookup=@ModuleName
and (a.Deleted<>1 or a.Deleted is null) 
and  isnull(a.TicketId,'') =CASE WHEN LEN(@TicketId)=0 then isnull(a.TicketId,'')  else @TicketId END   order by a.ItemOrder 
END

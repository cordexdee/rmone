
CREATE procedure [dbo].[usp_updateparenttask]
@TenantID varchar(250),
@ModuleId varchar(50)
as
begin
update ModuleTasks set ParentTaskID= Isnull((Select did from MyTempTable where SPId=Isnull(ParentTaskID,0) and TenantID=@TenantID and ModuleId=@ModuleId),0)
where TenantID=@TenantID and ModuleNameLookup=@ModuleId;

truncate table MyTempTable;
end



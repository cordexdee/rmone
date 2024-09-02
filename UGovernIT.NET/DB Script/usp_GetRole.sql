ALTER Procedure [dbo].[usp_GetRole]  
@TenantID varchar(128),  
@deptid int=0,  
@roleid varchar(128) ='',
@Id bigint=0,
@Deleted bit =0,
@BillingRate Float=0
as  
Begin  
If(@Id=0)
Begin
Select * from Roles where TenantID=@TenantID  and name !='' order by Name  
Select a.*, case when isnull(d.DivisionIdLookup,0)>0 then cd.Title+' > '+d.Title else d.Title end    DepartmentName from RoleBillingRateByDept a left join  
Department d on d.ID=a.departmentlookup 
left join CompanyDivisions cd on cd.ID=DivisionIdLookup
where a.TenantID=@TenantID  
and d.TenantID=@TenantID  
and departmentlookup  is not null  
END
Else Begin
Select a.*,case when isnull(d.DivisionIdLookup,0)>0 then cd.Title+' > '+d.Title else d.Title end DepartmentName,cd.Title from RoleBillingRateByDept a left join  
Department d on d.ID=a.departmentlookup 
left join CompanyDivisions cd on cd.ID=DivisionIdLookup
where a.TenantID=@TenantID  
and d.TenantID=@TenantID  
and departmentlookup =@deptid
and a.RoleLookup=@roleid
--and a.BillingRate=@BillingRate
and a.deleted=@Deleted
END
End


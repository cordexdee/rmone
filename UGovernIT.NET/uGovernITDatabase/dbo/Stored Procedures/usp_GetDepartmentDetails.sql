CREATE Procedure usp_GetDepartmentDetails   
@TenantId varchar(max),  
@DeptId varchar(max)  
as  
begin  
Select  Case when len(c.Title)>0 then c.Title +' > '+ isnull(d.Title,'') else d.Title  end 'CombinedTitle' , d.*,c.Title   
from Department d left join CompanyDivisions c on c.ID=isnull(d.DivisionIdLookup,0)  
where d. TenantID=@TenantId and cast(d.ID as varchar) in (Select trim(value) from string_split(@DeptId,','))  
end


----exec usp_ConfigurationByTenant
CREATE procedure usp_ConfigurationByTenant
 as
 begin
 DECLARE 
    @Tenantid VARCHAR(MAX)
   
 
DECLARE cursor_tenant CURSOR
FOR SELECT 
        distinct TenantID from ugovernit_common.dbo.Tenant;
 
OPEN cursor_tenant;
 
FETCH NEXT FROM cursor_tenant INTO 
    @Tenantid
    
 
WHILE @@FETCH_STATUS = 0
    BEGIN
        
        FETCH NEXT FROM cursor_tenant INTO 
            @Tenantid 
			exec Usp_InsertFieldConfiguration @Tenantid
           
    END;
 
CLOSE cursor_tenant;
 
DEALLOCATE cursor_tenant;
 end



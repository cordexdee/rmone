

/****************************************************************************************************
Author:Sanjeev Kumar																				*
Date : 20 Aug 2021																					*
exec GetUserChartData '35525396-E5FE-4692-9239-4DF9305B915B','studio',0,0								*
****************************************************************************************************/
ALTER proc [dbo].[GetUserChartData]
(
@TenantId nvarchar(50),
@Mode nvarchar(10),
@division bigint,
@studio bigint
)
As 
Begin
declare @TableName nvarchar(50)
set @TableName='#UserChartTable'
if(@Mode='studio')
begin
		select CASE WHEN StudioLookup = 0 THEN 'None' ELSE StudioLookup END as 'Studio', 
				isnull(sum(ApproxContractValue),0) as 'Value'  from CRMProject where 
				TenantID = @TenantId and (@division=0 or  DivisionLookup=@division) and (@studio=0 or StudioLookup=@studio) 
				group by CASE WHEN StudioLookup = 0 THEN 'None' ELSE StudioLookup END;
end
else if(@Mode='division')
begin
	select CASE WHEN DivisionLookup = 0 THEN 'None' ELSE DivisionLookup END as 'Division',  
			isnull(sum(ApproxContractValue),0) as 'Value' from CRMProject where 
			TenantID =@TenantId and (@division=0 or  DivisionLookup=@division) and (@studio=0 or StudioLookup=@studio) 
			group by CASE WHEN DivisionLookup = 0 THEN 'None' ELSE DivisionLookup END;
end
else
begin
		select CASE WHEN SectorChoice = 0 THEN 'None' ELSE SectorChoice END as 'Sector',
		isnull(sum(ApproxContractValue),0) as 'Value'  from CRMProject where 
		TenantID = @TenantId and (@division=0  or DivisionLookup=@division) and (@studio=0 or StudioLookup=@studio) 
		 group by CASE WHEN SectorChoice = 0 THEN 'None' ELSE SectorChoice END;
end
End

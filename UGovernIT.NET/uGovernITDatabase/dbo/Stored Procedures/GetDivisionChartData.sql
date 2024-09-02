/****************************************************************************************************
Author:Manish Kumar																				*
Date : 5 Oct 2021																					*
exec [GetDivisionChartData] '35525396-E5FE-4692-9239-4DF9305B915B',''							*
****************************************************************************************************/
create proc [dbo].[GetDivisionChartData]
(
@TenantId nvarchar(50),
@Mode nvarchar(10),
@division nvarchar(50)=null,
@studio nvarchar(50)=null
)
As 
Begin
	declare @TableName nvarchar(50)
	if(len(@division) > 0 and LEN(@studio) > 0)
	begin
			select isnull(sum(ApproxContractValue),0) as 'Value', isnull(st.Title, 'None') as Name
		from CRMProject CRP 
		left join Studio St on CRP.StudioLookup = st.ID 
		left join CompanyDivisions cd on st.DivisionLookup = cd.ID
		where CRP.TenantID = @TenantId and cd.ID=@division and st.ID=@studio
		group by st.Title
	end
	else if(len(@division) > 0 and LEN(@studio) < 1 )
	begin
		select isnull(sum(ApproxContractValue),0) as 'Value', isnull(st.Title, 'None') as Name
		from CRMProject CRP 
		left join Studio St on CRP.StudioLookup = st.ID 
		left join CompanyDivisions cd on st.DivisionLookup = cd.ID
		where CRP.TenantID = @TenantId and cd.ID=@division
		group by st.Title
	end
	else
	begin
		select isnull(sum(ApproxContractValue),0) as 'Value', isnull(cd.Title, 'None') as Name
		from CRMProject CRP 
		left join Studio St on CRP.StudioLookup = st.ID 
		left join CompanyDivisions cd on st.DivisionLookup = cd.ID
		where CRP.TenantID = @TenantId
		group by cd.Title
	end

End

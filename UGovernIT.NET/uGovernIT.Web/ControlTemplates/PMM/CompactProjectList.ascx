<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompactProjectList.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.PMM.CompactProjectList" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style>

</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {

        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }
</script>

<div>
    <div>
    <dx:ASPxGridView ID="grdProjects" runat="server" ClientInstanceName="grdProjects">
        <Columns>
            <dx:GridViewDataColumn FieldName="Title" Caption="Name"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="ProjectManagerUser" Caption="Project Manager"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="Description" Caption="Description"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="ActualStartDate" Caption="Start Date"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="DesiredCompletionDate" Caption="End Date"></dx:GridViewDataColumn>
        </Columns>
    </dx:ASPxGridView>
    </div>
    <div style="float:right; height:50px;">
        btNewbutton
        <dx:ASPxButton ID="btNewButton" runat="server" AutoPostBack="false" Text="New Ticket"></dx:ASPxButton>
    </div>
</div>
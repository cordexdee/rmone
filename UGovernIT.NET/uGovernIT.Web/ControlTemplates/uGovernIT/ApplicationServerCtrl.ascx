<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationServerCtrl.ascx.cs" Inherits="uGovernIT.Web.ApplicationServerCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteRoles(s, e) {

        if (confirm('Are you sure you want to unlink this asset?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
</script>
<div>
    <table style="width: 100%">
        <tr>
            <td>
                <dx:ASPxLoadingPanel ID="roleLoadingPanel" ClientInstanceName="roleLoadingPanel" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
                <ugit:ASPxGridView id="grdAppServer" runat="server" AutoGenerateColumns="false" 
                    KeyFieldName="ID" border-bordercolor="#ced8d9" border-borderwidth="2px" width="100%" 
                    Border-BorderStyle="Solid" OnHtmlDataCellPrepared="grdAppServer_HtmlDataCellPrepared" onhtmlrowcreated="grdAppServer_HtmlRowCreated"
                    Styles-AlternatingRow-BackColor="WhiteSmoke" SettingsBehavior-AllowGroup="true" SettingsBehavior-AutoExpandAllGroups="true">
                    <Columns>
                        <dx:GridViewDataColumn FieldName="EnvTitle" Caption="Environment" GroupIndex="0" Visible="false" CellStyle-Font-Bold="true" ></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="ServerFunctionsChoice" Caption="Server Function(s)" CellStyle-Font-Bold="true"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="TicketId" Caption="Asset ID" CellStyle-Font-Bold="true"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="AssetTagNum" Caption="Asset Tag #" CellStyle-Font-Bold="true"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="AssetTitle" Caption="Server Name" CellStyle-Font-Bold="true"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="HostName" Caption="Host Name" CellStyle-Font-Bold="true" ></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn FieldName="AssetDescription" Caption="Description" CellStyle-Font-Bold="true"></dx:GridViewDataColumn>
                        <dx:GridViewDataColumn>
                            <DataItemTemplate>
                                <dx:ASPxButton ID="lnkDelete" ClientInstanceName="lnkDelete" RenderMode="Link" runat="server" OnClick="lnkDelete_Click"
                                     Image-Url="/Content/Images/delete-icon-new.png">
                                    <ClientSideEvents Click="DeleteRoles" />
                                </dx:ASPxButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                    </Columns>
                    
                    <Settings GroupFormat="{1}" ShowGroupedColumns="true" />
                </ugit:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <span style="float: right;padding-top: 5px;">
                    <dx:ASPxButton ID="aAddNew" runat="server" Text="New" ToolTip="Add New" ValidationGroup="Save" AutoPostBack="false" CssClass="primary-blueBtn"></dx:ASPxButton>
                </span>
            </td>
        </tr>
    </table>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationRoleCtrl.ascx.cs" Inherits="uGovernIT.Web.ApplicationRoleCtrl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function adjustControlSize() {
        setTimeout(function () {
            try {
                $("#s4-workspace").width("100%");
                if (grdApplRoles) {
                    grdApplRoles.AdjustControl();
                    grdApplRoles.SetWidth($(window).width() - 25);
                    grdApplRoles.SetHeight($(window).height() - 35);
                }
            } catch (ex) { }
        }, 10);
    }

    function SetApplicationGridWidth(s, e) {
        s.SetWidth($(window).width() - 25);
        s.SetHeight($(window).height() - 35);
    }

    function DeleteRoles(s, e) {
        
        if (confirm('This action will remove all dependencies.\n\n Are u sure want to delete this role?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
</script>

<div>
    <table style="width: 100%;margin-left: 7px;height:200px">
        <tr>
            <td>
                <span style="float: right;margin-right: 7px;">
                      <dx:ASPxButton ID="aAddNew" runat="server" Text="New" ToolTip="Add New" ValidationGroup="Save" AutoPostBack="false" CssClass="primary-blueBtn"></dx:ASPxButton>
                </span>
            </td>
        </tr>
        <tr>
            <td>
                <dx:ASPxLoadingPanel ID="roleLoadingPanel" ClientInstanceName="roleLoadingPanel" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
                <ugit:ASPxGridView ID="grdApplRoles" runat="server" AutoGenerateColumns="false" EnableViewState="false" ClientInstanceName="grdApplRoles"
                    OnHtmlRowCreated="grdApplRoles_HtmlRowCreated"
                    KeyFieldName="ID" Border-BorderColor="#ced8d9" Border-BorderWidth="2px" Width="100%"
                    Border-BorderStyle="Solid">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="#" CellStyle-HorizontalAlign="Center"  FieldName="ItemOrder" Width="30px" />
                        <dx:GridViewDataTextColumn Caption="Application Role" FieldName="Title" />
                        <dx:GridViewDataTextColumn Caption="Modules" FieldName="ApplicationRoleModuleLookupName" />
                        <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" />
                        <dx:GridViewDataColumn CellStyle-HorizontalAlign="Center" Width="100px">
                            <DataItemTemplate>
                                <a id="aEdit" runat="server" href="">
                                    <img id="Imgedit" runat="server" src="/Content/images/edit-icon.png" style="margin-top:5px;" />
                                </a>
                                <dx:ASPxButton ID="lnkDelete" ClientInstanceName="lnkDelete" RenderMode="Link" runat="server" OnClick="lnkDelete_Click"
                                     Image-Url="/Content/Images/delete-icon-new.png">
                                    <ClientSideEvents Click="DeleteRoles" />
                                </dx:ASPxButton>
                            </DataItemTemplate>
                        </dx:GridViewDataColumn>
                    </Columns>
                    <Styles>
                        <Header Font-Bold="true"></Header>
                        <AlternatingRow Enabled="True" CssClass="ms-alternatingstrong"></AlternatingRow>
                    </Styles>
                    <ClientSideEvents Init="function(s, e) {SetApplicationGridWidth(s,e);}" />
                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                    <Settings VerticalScrollBarMode="Auto" />
                </ugit:ASPxGridView>
            </td>
        </tr>
    </table>
</div>
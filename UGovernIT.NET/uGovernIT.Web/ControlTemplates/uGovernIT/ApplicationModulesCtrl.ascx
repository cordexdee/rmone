<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationModulesCtrl.ascx.cs" Inherits="uGovernIT.Web.ApplicationModulesCtrl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function adjustControlSize() {
        setTimeout(function () {
            try {
                $("#s4-workspace").width("100%");
                if (grdApplModules) {
                    grdApplModules.AdjustControl();
                    grdApplModules.SetWidth($(window).width() - 25);
                    grdApplModules.SetHeight($(window).height() - 35);
                }
            } catch (ex) { }
        }, 10);
    }

    function SetApplicationGridWidth(s, e) {
        s.SetWidth($(window).width() - 25);
        s.SetHeight($(window).height() - 35);
    }
    function DeleteModules() {
        if (confirm('This action will remove all dependencies.\n\n Are u sure want to delete this module?')) {
            moduleLoadingPanel.Show();
        }
        else {
            moduleLoadingPanel.Hide();
        }
    }
</script>

<div>
    <table style="width: 100%;margin-left:7px;height:200px">
        <tr>
            <td>
                <span style="float: right;margin-right: 7px;">
                    <dx:ASPxButton ID="aAddNew" runat="server" Text="New" ToolTip="Add Modules" ValidationGroup="Save" AutoPostBack="false" CssClass="primary-blueBtn"></dx:ASPxButton>
                </span>
            </td>
        </tr>

        <tr>
            <td>
                <dx:ASPxLoadingPanel ID="moduleLoadingPanel" ClientInstanceName="moduleLoadingPanel" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
                <ugit:ASPxGridView ID="grdApplModules" runat="server" AutoGenerateColumns="false" EnableViewState="false" ClientInstanceName="grdApplModules"
                    OnHtmlRowCreated="grdApplModules_HtmlRowCreated" OnLoad="grdApplModules_Load"
                    KeyFieldName="ID" Border-BorderColor="#ced8d9" Border-BorderWidth="2px" Width="100%" 
                    Border-BorderStyle="Solid">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="#" CellStyle-HorizontalAlign="Center" SortOrder="Ascending"  FieldName="ItemOrder" Width="30px"  />
                        <dx:GridViewDataTextColumn Caption="Application Module"   FieldName="Title"  />
                        <dx:GridViewDataTextColumn Caption="Owner"   FieldName="Owner" />
                        <dx:GridViewDataTextColumn Caption="Supported By"   FieldName="SupportedBy" />
                        <dx:GridViewDataTextColumn Caption="Access Admin"   FieldName="AccessAdmin" />
                        <dx:GridViewDataTextColumn Caption="Approver"   FieldName="Approver" />
                        <dx:GridViewDataTextColumn Caption="Approval Type"   FieldName="ApprovalTypeChoice" />
                        <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" />
                        <dx:GridViewDataColumn CellStyle-HorizontalAlign="Center" Width="100px">
                            <DataItemTemplate>
                                <a id="aEdit" runat="server" href="">
                                    <img id="Imgedit" runat="server" src="/Content/images/edit-icon.png" />
                                </a>
                                <asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="javascript:DeleteModules();"
                                    OnClick="lnkDelete_Click">
                                    <img id="Imgdelete" runat="server" src="/Content/Images/delete-icon-new.png"/>
                                </asp:LinkButton>
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

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationReport_Filter.ascx.cs" Inherits="uGovernIT.DxReport.ApplicationReport.ApplicationReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<style>
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 150px;
        vertical-align: middle;
        font-weight: normal;
    }

    .full_width {
        width: 100%;
    }

    .main_heading {
        background: none repeat scroll 0 0 #BED0E5;
        float: left;
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 5px;
        padding: 0;
        text-align: center;
        width: 100%;
    }

    .table_lst {
        width: 100%;
    }

    .button_lst {
        width: 70px;
        margin: 5px;
    }

    .listbox_lst {
        width: 150px;
        height: 100px;
    }

    .td_lst {
        /*width: 33%;*/
        padding: 5px 5px;
        vertical-align: middle;
        text-align: center;
    }

    .error {
        color: red;
    }

    .rbutton {
        float: left;
        width: 100px;
    }

        .rbutton label {
            vertical-align: middle;
            position: relative;
            top: 1px;
        }
</style>
<script type="text/javascript">
    function UpdateButtonStateCategories() {
        btnMoveAllItemsToRightCategories.SetEnabled(lstCategoriesLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftCategories.SetEnabled(lstCategoriesRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightCategories.SetEnabled(lstCategoriesLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftCategories.SetEnabled(lstCategoriesRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateApplications() {
        btnMoveAllItemsToRightApplication.SetEnabled(lstApplicationsLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftApplication.SetEnabled(lstApplicationRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightApplication.SetEnabled(lstApplicationsLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftApplication.SetEnabled(lstApplicationRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateUsers() {
        btnMoveAllItemsToRightUsers.SetEnabled(lstUsersLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftUsers.SetEnabled(lstUsersRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightUsers.SetEnabled(lstUsersLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftUsers.SetEnabled(lstUsersRHS.GetSelectedItems().length > 0);
    }
    function ShowAppAccessReportPopUp(url) {
        window.parent.UgitOpenPopupDialog(url, "", 'Application Access Report', '1300', '800', 0, escape("<%= Request.Url.AbsolutePath %>"), 'true');
        return false;
    }

</script>
<asp:Panel runat="server" ID="pnlFilter">

    <div>
        <asp:Label ID="lblMsg" Text="" runat="server" CssClass="error" />
    </div>
    <table style="width: 100%">
        <tr>
            <td style="vertical-align: top;">
                <div style="width: auto;">

                    <fieldset>
                        <legend style="font-weight: bold;">Select Application(s):</legend>
                        <table style="width: 100%;" id="tblApplicationFilters" runat="server">
                            <tr>
                                <td class="ms-formlabel">Selection Type</td>
                                <td class="ms-formbody">
                                    <span class="rbutton">
                                        <asp:RadioButton ID="rbApplication" runat="server" GroupName="ApplicationSelection" RbText="application"
                                            AutoPostBack="true" Text="Application" OnCheckedChanged="rbSelectionType_CheckedChanged" Checked="true" />
                                    </span>
                                    <span class="rbutton">
                                        <asp:RadioButton ID="rbUser" Text="User" runat="server" GroupName="ApplicationSelection" RbText="user"
                                            AutoPostBack="true" OnCheckedChanged="rbSelectionType_CheckedChanged" />
                                    </span>

                                    <span>
                                        <asp:CheckBox ID="chkShowDetail" Text="Show Details" TextAlign="Right" runat="server" Visible="false" />
                                    </span>
                                </td>
                            </tr>

                            <tr id="trCategory" runat="server" style="height: 50px;">
                                <td class="ms-formlabel">Categories</td>
                                <td class="ms-formbody">
                                    <div>
                                        <table class="table_lst">
                                            <tr>
                                                <td>
                                                    <dx:ASPxListBox ID="lstCategoriesLHS" runat="server" CssClass="list_box"
                                                        Width="170px" Height="140px" SelectionMode="CheckColumn" ClientInstanceName="lstCategoriesLHS" Font-Names="Verdana" Font-Size="8pt">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateCategories(); }" />
                                                    </dx:ASPxListBox>
                                                </td>
                                                <td class="td_lst">
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToRightCategories" runat="server" AutoPostBack="True" Text="Add >"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightCategories_Click"
                                                            ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightCategories" CssClass="button_list" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveAllItemsToRightCategories" runat="server" AutoPostBack="True" Text="Add All >>"
                                                            Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightCategories_Click"
                                                            ClientInstanceName="btnMoveAllItemsToRightCategories" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div style="height: 10px"></div>
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToLeftCategories" runat="server" AutoPostBack="True" Text="< Remove"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftCategories_Click"
                                                            ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftCategories" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div class="TopPadding">
                                                        <dx:ASPxButton ID="btnMoveAllItemsToLeftCategories" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftCategories_Click"
                                                            ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftCategories" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                </td>
                                                <td>
                                                    <dx:ASPxListBox ID="lstCategoriesRHS" runat="server" ClientInstanceName="lstCategoriesRHS" Width="170px"
                                                        Height="140px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateCategories(); }" />
                                                    </dx:ASPxListBox>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </td>
                            </tr>
                            <tr id="trUsers" runat="server" style="height: 50px;">
                                <td class="ms-formlabel">Users</td>
                                <td class="ms-formbody">
                                    <div>
                                        <table class="table_lst">
                                            <tr>
                                                <td>
                                                    <dx:ASPxListBox ID="lstUsersLHS" runat="server" ClientInstanceName="lstUsersLHS" Width="170px"
                                                        Height="140px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateUsers(); }" />
                                                    </dx:ASPxListBox>
                                                </td>
                                                <td class="td_lst">
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToRightUsers" runat="server" AutoPostBack="True" Text="Add >"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightUsers_Click"
                                                            ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightUsers" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveAllItemsToRightUsers" runat="server" AutoPostBack="True" Text="Add All >>"
                                                            Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightUsers_Click"
                                                            ClientInstanceName="btnMoveAllItemsToRightUsers" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div style="height: 10px"></div>
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToLeftUsers" runat="server" AutoPostBack="True" Text="< Remove"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftUsers_Click"
                                                            ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftUsers" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div class="TopPadding">
                                                        <dx:ASPxButton ID="btnMoveAllItemsToLeftUsers" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftUsers_Click"
                                                            ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftUsers" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                </td>
                                                <td>
                                                    <dx:ASPxListBox ID="lstUsersRHS" runat="server" Width="170px"
                                                        Height="140px" SelectionMode="CheckColumn" ClientInstanceName="lstUsersRHS" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateUsers(); }" />
                                                    </dx:ASPxListBox>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trApplications" runat="server" style="height: 50px;">
                                <td class="ms-formlabel">Applications</td>
                                <td class="ms-formbody">
                                    <div>
                                        <table class="table_lst">
                                            <tr>
                                                <td>
                                                    <dx:ASPxListBox ID="lstApplicationsLHS" runat="server" ClientInstanceName="lstApplicationsLHS" Width="170px"
                                                        Height="140px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateApplications(); }" />
                                                    </dx:ASPxListBox>
                                                </td>
                                                <td class="td_lst">
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToRightApplication" runat="server" AutoPostBack="True" Text="Add >"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightApplication_Click"
                                                            ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightApplication" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveAllItemsToRightApplication" runat="server" AutoPostBack="True" Text="Add All >>"
                                                            Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightApplication_Click"
                                                            ClientInstanceName="btnMoveAllItemsToRightApplication" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div style="height: 10px"></div>
                                                    <div>
                                                        <dx:ASPxButton ID="btnMoveSelectedItemsToLeftApplication" runat="server" AutoPostBack="True" Text="< Remove"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftApplication_Click"
                                                            ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftApplication" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                    <div class="TopPadding">
                                                        <dx:ASPxButton ID="btnMoveAllItemsToLeftApplication" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                            Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftApplication_Click"
                                                            ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftApplication" Font-Names="Verdana" Font-Size="8pt">
                                                        </dx:ASPxButton>
                                                    </div>
                                                </td>
                                                <td>
                                                    <dx:ASPxListBox ID="lstApplicationRHS" runat="server" ClientInstanceName="lstApplicationRHS" Width="170px"
                                                        Height="140px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateApplications(); }" />
                                                    </dx:ASPxListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>

                                </td>
                            </tr>

                        </table>
                    </fieldset>
                </div>
            </td>

        </tr>
        <tr>
            <td colspan="2" style="float: right">
                <div class="first_tier_nav">
                    <dx:ASPxButton ID="lnkSubmit" Text="Build Report" runat="server" OnClick="lnkBuild_Click" CssClass="primary-blueBtn">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" OnClick="lnkCancel_Click" CssClass="primary-blueBtn">
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>





</asp:Panel>

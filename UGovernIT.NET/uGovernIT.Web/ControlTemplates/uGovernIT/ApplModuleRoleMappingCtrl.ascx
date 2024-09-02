<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplModuleRoleMappingCtrl.ascx.cs" Inherits="uGovernIT.Web.ApplModuleRoleMappingCtrl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>


<script data-v="<%=UGITUtility.AssemblyVersion %>">


    function adjustControlSize() {

        setTimeout(function () {
            try {
                $("#s4-workspace").width("100%");
                if (grdApplModuleRoleMap) {
                    grdApplModuleRoleMap.AdjustControl();
                    // grdApplModuleRoleMap.SetWidth($(window).width() - 20);
                    grdApplModuleRoleMap.SetHeight($(window).height() - 60);
                }
                else {
                    gvApplModuleRoleMapByUser.AdjustControl();
                    //  gvApplModuleRoleMapByUser.SetWidth($(window).width() - 20);
                    gvApplModuleRoleMapByUser.SetHeight($(window).height() - 60);
                }

            } catch (ex) { }
        }, 10);
    }

    function SetApplicationGridWidth(s, e) {
        s.AdjustControl();
        //  s.SetWidth($(window).width() - 20);
        // s.SetHeight($(window).height() - 60)
    }

    //For Excel Import
    function OpenImportExcel() {
        var title = 'Import Access';
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', '', title, '400px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function grdvUserList_click(s, e) {

        var selectedIDs = s.GetSelectedKeysOnPage();
        if (selectedIDs.length > 0) {
            gvApplModuleRoleMapByUser.PerformCallback("SelectedUserId|" + selectedIDs[0]);
        }

    }
    function grdvModuleList_click(s, e) {

        var selectedIDs = s.GetSelectedKeysOnPage();
        if (selectedIDs.length > 0) {
            grdApplModuleRoleMap.PerformCallback("SelectedModuleId|" + selectedIDs[0]);
        }

    }
    function AppAccessCtrExport() {
        $("#<%=btnExportAppAccessCtr.ClientID%>").click();
    }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .pointerCss {
        cursor: pointer;
    }
</style>
<dx:ASPxButton ID="btnExportAppAccessCtr" ClientInstanceName="btnExportAppAccessCtr" ClientVisible="false" runat="server" OnClick="btnExportAppAccessCtr_Click"></dx:ASPxButton>
<div>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlViewType_SelectedIndexChanged">
                    <asp:ListItem Text="By Module" Value="bymodule"></asp:ListItem>
                    <asp:ListItem Text="By User" Value="byuser" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="float: right; margin-right: 7px;">
                <span style="float: left; padding-bottom: 5px;">
                    <dx:ASPxButton ID="aAddNew" runat="server" Text="Add Access" ToolTip="Add Access" ValidationGroup="Save" AutoPostBack="false" CssClass="primary-blueBtn"></dx:ASPxButton>
                    <%-- <a href="javascript:" title="New" id="aAddNew" runat="server">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">Add Access</b>
                            <i style="float: left; position: relative; top: 1px; left: 2px">
                                <img alt="" title="" style="border: none;" src="/_layouts/15/images/uGovernIT/add_icon.png"></i>
                        </span>
                    </a>--%>
                </span>
                <span id="exportAction1" style="padding-left: 3px; float: right; padding-top: 5px;" class="fright">
                    <a href="javascript:" title="Report" id="lnkReport" runat="server">
                        <img id="imgReport" runat="server" src="/Content/Images/Reports_16x16.png" alt="Reports" title="Reports" style="cursor: pointer;" class="imgReport" />
                    </a>
                </span>
                <span id="onlyExcelImport" style="padding-left: 2px" class="fright" runat="server">
                    <img src="/Content/images/import.png" style="cursor: pointer;" title="Import Excel" alt="Import Excel" onclick="return OpenImportExcel();" />
                </span>
                <span id="onlyExportExcel" style="padding-left: 2px;" class="fright" runat="server">
                    <img src="/Content/images/export.png" style="cursor: pointer;" title="Export Excel" alt="Export Excel" onclick="return AppAccessCtrExport();" />
                </span>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
        </tr>
    </table>
</div>

<dx:ASPxSplitter runat="server" Width="99%" SeparatorVisible="false" ID="splitterAppRoleAccess" ClientInstanceName="splitterAppRoleAccess" ResizingMode="Live">
    <ClientSideEvents />
    <Panes>
        <dx:SplitterPane AutoHeight="true" PaneStyle-CssClass="leftSplitterPane" ScrollBars="None" AutoWidth="false" Size="200" Name="leftPane">
            <ContentCollection>
                <dx:SplitterContentControl CssClass="spltrLeftContentControl" ID="spltrLeftContentControl" runat="server" SupportsDisabledAttribute="True">
                    <div class="page-left-Nav">
                        <div class="page-list">
                            <ugit:ASPxGridView EnableCallBacks="false" AutoGenerateColumns="False" KeyFieldName="UserId" ID="grdvUserList" ClientInstanceName="grdvUserList" CssClass="customgridview"
                                OnHtmlRowCreated="grdvUserList_HtmlRowCreated" runat="server" AllowSelectNode="true" AutoPostBack="false" OnSelectionChanged="grdvUserList_SelectionChanged">
                                <Columns>
                                    <dx:GridViewDataColumn Width="5%" CellStyle-HorizontalAlign="Center">
                                        <DataItemTemplate>
                                            <dx:ASPxImage runat="server" ID="ImgEditUser" ImageUrl="/Content/buttonImages/edit-icon.png" Style="cursor: pointer;"></dx:ASPxImage>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn Width="170px"  Settings-HeaderFilterMode="CheckedList" Settings-AllowHeaderFilter="True" Caption="User Name" FieldName="ApplicationRoleAssignUser">
                                        <DataItemTemplate>
                                            <dx:ASPxButton RenderMode="Link" runat="server" ID="btnApplicationRoleAssignUser" Style="cursor: pointer;"></dx:ASPxButton>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>

                                    <%--<dx:GridViewDataColumn Width="170px" Caption="User Name" Settings-AllowHeaderFilter="True" Settings-HeaderFilterMode="CheckedList"
                                        FieldName="ApplicationRoleAssignUser">
                                    </dx:GridViewDataColumn>--%>
                                </Columns>
                                <SettingsBehavior AllowSelectSingleRowOnly="true" AllowSelectByRowClick="true" AllowSort="true" />
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                <%--<Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />--%>
                                <ClientSideEvents SelectionChanged="function(s,e){grdvUserList_click(s,e);}" />
                                <SettingsContextMenu EnableRowMenu="True">
                                    <RowMenuItemVisibility NewRow="false" EditRow="false" Refresh="false" />
                                </SettingsContextMenu>
                                <Styles>
                                    <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                                    <Header Font-Bold="true"></Header>
                                </Styles>
                                <ClientSideEvents />
                            </ugit:ASPxGridView>

                            <ugit:ASPxGridView EnableCallBacks="false" AutoGenerateColumns="False" KeyFieldName="ModuleId" ID="grdvModuleList" ClientInstanceName="grdvModuleList" CssClass="customgridview"
                                runat="server" AllowSelectNode="true" AutoPostBack="false" OnSelectionChanged="grdvModuleList_SelectionChanged">
                                <Columns>
                                    <dx:GridViewDataTextColumn Width="170px" Caption="Module" Settings-AllowHeaderFilter="True" Settings-HeaderFilterMode="CheckedList" FieldName="ApplicationModulesLookup"></dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowSelectSingleRowOnly="true" AllowSelectByRowClick="true" AllowSort="true" />
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                <Settings />
                                <ClientSideEvents SelectionChanged="function(s,e){grdvModuleList_click(s,e);}" />
                                <SettingsContextMenu EnableRowMenu="True">
                                    <RowMenuItemVisibility NewRow="false" EditRow="false" Refresh="false" />
                                </SettingsContextMenu>
                                <Styles>
                                    <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                                </Styles>
                                <ClientSideEvents />
                            </ugit:ASPxGridView>
                        </div>
                    </div>
                </dx:SplitterContentControl>
            </ContentCollection>
        </dx:SplitterPane>

        <dx:SplitterPane AutoHeight="true" PaneStyle-CssClass="rightSplitterPane" Size="85%" MinSize="280px" Name="rightPane">
            <ContentCollection>
                <dx:SplitterContentControl ID="spltrRightContentControl" runat="server" SupportsDisabledAttribute="True">
                    <asp:Panel ID="editContentPanel" runat="server" CssClass="page-right-Nav" Visible="false"></asp:Panel>
                    <ugit:ASPxGridView ID="gvApplModuleRoleMapByUser" ClientInstanceName="gvApplModuleRoleMapByUser" runat="server" AutoGenerateColumns="false" EnableViewState="false"
                        OnHtmlRowCreated="gvApplModuleRoleMapByUser_HtmlDataCellPrepared"
                        KeyFieldName="UserId" Border-BorderColor="#ced8d9" Border-BorderWidth="2px" Width="99%"
                        Border-BorderStyle="Solid" SettingsBehavior-AllowGroup="true">
                        <Columns>
                            <dx:GridViewDataColumn Width="30%" Caption="Module" Settings-AllowHeaderFilter="True" Settings-HeaderFilterMode="CheckedList" FieldName="ApplicationModulesLookup">
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataTextColumn Width="65%" Caption="Application Role(s)" FieldName="ApplicationRoleLookup" />

                        </Columns>
                        <Templates>
                            <GroupRowContent>
                                <dx:ASPxHyperLink ID="aRoleAssignee" runat="server" Text="<%#Eval(DatabaseObjects.Columns.ApplicationRoleAssign) %>" Style="text-decoration: underline; color: black; font-weight: bold;">
                                </dx:ASPxHyperLink>
                            </GroupRowContent>
                        </Templates>
                        <Styles>
                            <GroupRow Font-Bold="true"></GroupRow>
                            <Header Font-Bold="true"></Header>
                            <AlternatingRow Enabled="True" CssClass="ms-alternatingstrong"></AlternatingRow>
                        </Styles>
                        <ClientSideEvents Init="function(s, e) {SetApplicationGridWidth(s,e);}" />
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                        <Settings VerticalScrollBarMode="Auto" />
                    </ugit:ASPxGridView>

                    <ugit:ASPxGridView ID="grdApplModuleRoleMap" ClientInstanceName="grdApplModuleRoleMap" runat="server" AutoGenerateColumns="false" EnableViewState="false"
                        OnHtmlRowCreated="grdApplModuleRoleMap_HtmlDataCellPrepared"
                        KeyFieldName="UserId" Border-BorderColor="#ced8d9" Border-BorderWidth="2px" Width="99%"
                        Border-BorderStyle="Solid" SettingsBehavior-AllowGroup="true">
                        <Columns>
                            <dx:GridViewDataColumn Width="5%" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <dx:ASPxImage runat="server" ID="Imgedit" ImageUrl="/Content/ButtonImages/edit-icon.png" Style="cursor: pointer;"></dx:ASPxImage>
                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn Width="30%" Caption="User" FieldName="ApplicationRoleAssignUser" Settings-AllowHeaderFilter="True" Settings-HeaderFilterMode="CheckedList">
                                <DataItemTemplate>
                                    <dx:ASPxHyperLink ID="aRoleAssignee" runat="server" Text="<%#Eval(DatabaseObjects.Columns.ApplicationRoleAssign) %>" Style="text-decoration: underline;"></dx:ASPxHyperLink>

                                </DataItemTemplate>
                            </dx:GridViewDataColumn>
                            <dx:GridViewDataColumn Width="65%" Caption="Application Role(s)" FieldName="ApplicationRoleLookup" />

                        </Columns>
                        <Styles>
                            <GroupRow Font-Bold="true"></GroupRow>
                            <Header Font-Bold="true"></Header>
                            <AlternatingRow Enabled="True" CssClass="ms-alternatingstrong"></AlternatingRow>
                        </Styles>
                        <ClientSideEvents Init="function(s, e) {SetApplicationGridWidth(s,e);}" />
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                        <Settings VerticalScrollBarMode="Auto" />
                    </ugit:ASPxGridView>
                </dx:SplitterContentControl>
            </ContentCollection>
        </dx:SplitterPane>
    </Panes>
</dx:ASPxSplitter>
<dx:ASPxSpreadsheet ID="aspxSpreadSheetCtr" runat="server" Visible="false">
</dx:ASPxSpreadsheet>



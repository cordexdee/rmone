<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageEditor.ascx.cs" Inherits="uGovernIT.Web.PageEditor" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .divMain {
        width: 100%;
        height: 100%;
        position: relative;
    }

        .divMain .page-heading {
            float: right;
            border: 1px solid;
            padding: 5px;
            position: absolute;
            right: 17px;
            top: 8px;
        }

    .button-bg.add-bt {
        border: 0px;
        width: 100%;
        float: left;
    }

    .button-bg.add {
        border: 0px;
        position: relative;
        top: -1px;
        left: -2px;
    }

    .page-left-Nav {
        position: relative;
        float: left;
        width: 230px;
    }

    .page-footer-nav {
        float: left;
        width: 99%;
        border-top: 1px solid;
    }

    .customgridview {
        width: 100% !important;
    }

    .addnew-main {
        float: left;
        width: 100%;
        padding:10px 0;

    }

    .page-list {
        float: left;
        margin-top: 4px;
        width: 100%;
    }

    .page-right-Nav {
        position: relative;
        float: left;
        width: 100%;
    }

        .page-right-Nav .left-nav {
            float: left;
            width: 20%;
            border-right: 1px solid;
        }

            .page-right-Nav .left-nav .title,
            .page-right-Nav .right-block .header .title,
            .page-right-Nav .right-block .webpart .title,
            .page-footer-nav .title {
                background: #9da0aa;
                color: #FFF;
                float: left;
                padding: 4px;
                margin-left: 1px;
                font-size: 13px;
                font-weight: bold;
            }

        .page-right-Nav .right-block {
            float: left;
            width: 79%;
        }

            .page-right-Nav .right-block .header {
                position: absolute;
                top: 2px;
                float: left;
                height: 100px;
                border-bottom: 1px solid;
                width: 100%;
            }

                .page-right-Nav .right-block .header .content {
                    float: left;
                    width: 90%;
                    margin-left: 100px;
                }

                .page-right-Nav .right-block .header .menu-option {
                    margin-left: 5px;
                    margin-top: 5px;
                }

            .page-right-Nav .right-block .webpart {
                position: relative;
                top: 40px;
                border-top: solid 1px;
            }

            .page-right-Nav .right-block .content {
                width: 100%;
                height: inherit;
            }

        .page-right-Nav .left-nav .config-block {
            width: 100%;
            height: inherit;
            padding-top: 40px;
        }

            .page-right-Nav .left-nav .config-block > div {
                margin-bottom: 5px;
            }

    .page-footer-nav .content {
        float: left;
        margin-left: 10px;
    }


    .page-right-Nav .right-block .webpart .content {
        float: left;
        width: 100%;
        margin: 10px;
        /*height: 257px;*/
        overflow-y: auto;
        overflow-x: hidden;
    }

    .page-right-Nav .right-block .webpart .add-bt {
        float: left;
        margin-left: 5px;
    }

    .margin-top5 {
        margin-top: 5px;
    }

    .page-action {
        float: left;
        width: 100%;
        text-align: right;
    }

    .webpartlist .card {
        display: block;
        position: relative;
        /*height: 80px !important;*/
        width: 250px !important;
    }

    .infomessageboard {
        min-height: 24px;
        margin-right: 0px;
        text-align: center;
        margin-top: 80px;
        top: 50px;
        color: red;
        font-size: 12px;
    }

    .ui-state-highlight {
        /*height: 50px;*/
        line-height: 1.2em;
    }

    .save-webpartprops {
        float: right;
        margin-bottom: 6px;
    }

    /*.FooterCss {
        background-color: #c9cad1;
    }*/

    .spltrLeftContentControl {
        padding: 0 0 0 3px !important;
    }

    .leftSplitterPane, .rightSplitterPane {
        border: none;
    }

    .header-container .title {
        background: #9da0aa;
        color: #FFF;
        float: left;
        padding: 4px;
        margin-left: 1px;
        font-size: 13px;
        font-weight: bold;
    }

    .search-container {
        padding-top: 0;
        padding-left: 61px;
    }

    .page-title {
        padding-left: 70px;
    }

    .page-title .all-input {
        padding: 5px !important;
        font-size: 12px !important;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function initalizeScript() {
        var sourceKey;
        var targetKey;
        var sourceIndex;
        $(".sortable .dxcvTable_DevEx > tbody").sortable({
            containment: "parent",
            placeholder: "ui-state-highlight",
            items: " > tr:even",
            start: function (event, ui) {
                sourceKey = $.trim($(ui.item[0]).find(".UniqueID").text());
            },
            stop: function (event, ui) {

                sourceKey = $(ui.item[0]).find(".UniqueID").text();
                targetKey = $.trim($(ui.item[0]).next().find(".UniqueID").text());

                if (sourceKey != "") {
                    hdnSetting.Set("sortSourceWebpartKey", sourceKey);
                    hdnSetting.Set("sortTargetWebpartKey", targetKey);
                    $(".btSortWebpart").trigger("click");
                }

            }
        });
    }

    function splitterPageEditor_init(s, e) {

        var frameHeight = $(window).height();
        frameHeight = frameHeight - 30;
        splitterPageEditor.SetHeight(frameHeight);
        grdvPageList.SetHeight(frameHeight - 20);
        var leftPaneWidth = splitterPageEditor.GetPaneByName("leftPane").lastWidth;
        $(".left-nav").height(frameHeight - 50 + "px");
        $(".divMain").css("visibility", "visible");
        loadingPanel.Hide();
    }
    function openPopup(panelid, controlid) {
        if (panelid != undefined && controlid != undefined) {
            loadingPanel.Hide();
            hdnSetting.Set("EditWebpartIndex", panelid + '|' + controlid)
            editWebpartPropertiesPopup.PerformCallback(panelid + '|' + controlid);
            editWebpartPropertiesPopup.Show();

        }
        //loadingPanel.Hide();
    }
    function editWebpartPropertiesPopup_shown(s, e) {

        //var frameHeight =  $(document).height();
        var frameHeight = $(window).height();
        var popupContentHeight = webpartPropsHolder.GetHeight();
        var popupHeight = popupContentHeight + 90;
        if (frameHeight < popupHeight) {
            popupHeight = popupHeight - 90;
        }
        editWebpartPropertiesPopup.SetHeight(popupHeight);
        editWebpartPropertiesPopup.UpdatePosition();
    }

    function grdvPageList_click(s, e) {

        var selectedIDs = s.GetSelectedKeysOnPage();
        if (selectedIDs.length > 0) {
            hdnSetting.Set("SelectedPage", selectedIDs[0]);
            $(".btEditPage").trigger("click");
        }

    }

    function btAddWebpart_Click(s, e) {
        
        addWebpartPopup.ShowAtElementByID(s.name);
    }

    function addSelectedWebpart_Click(webpartName) {
        addWebpartPopup.Hide();
        webpartName = $.trim(webpartName)
        hdnSetting.Set("SelectedWebpartName", webpartName);
        $(".btAddWebpartOnPage").trigger("click");
    }

    function btPreviewPage_Click(s, e) {
        var url = "<%= selectedPageUrl%>";
        if (url != "") {
            window.open(url);
        }
    }
    var startDeletePage = false;
    function btDeletePage_click(s, e) {
        e.processOnServer = false;
        if (startDeletePage) {
            startDeletePage = false;
            e.processOnServer = true;
            confirmDeletePopup.Hide();
            loadingPanel.Show();
        }
        else {
            confirmDeletePopup.Show();
        }
    }

    function btConfirmDeleteButton_Click(s, e) {
        startDeletePage = true;
        btDeletePage.DoClick();
    }

    function confirmDeletePopup_Closing(s, e) {
        startDeletePage = false;
    }

    function ShowLeftNavigation(s, e) {
        $('.leftnav-dashboardoption').hide();
        $('.clsEnableLeftNavigation').hide();
        $('.leftmenuoption').hide();

        if (s.GetChecked()) {
            $('.clsEnableLeftNavigation').show();
            if (rdbenableleftnavigation != null && rdbenableleftnavigation.GetSelectedItem().value == "0") {
                $('.leftmenuoption').show();
            }
            else if (rdbenableleftnavigation != null && rdbenableleftnavigation.GetSelectedItem().value == "1") {
                $('.leftnav-dashboardoption').show();
            }
        }
    }

    function ConfirmDeletePanel(s, e) {
        var confirm_delete_value = document.createElement("INPUT");
        confirm_delete_value.type = "hidden";
        confirm_delete_value.name = "confirm_delete_value";
        if (confirm("Are you sure want to delete?")) {
            confirm_delete_value.value = "YES";
        } else {
            confirm_delete_value.value = "NO";
            e.processOnServer = false;
        }
        document.forms[0].appendChild(confirm_delete_value);
    }
    $(document).ready(function () {
        var rowindex = '<%=RowIndex%>';
        var GridId = $('#<%=grdvPageList.ClientID%>').attr('id');
        $(".dxgvCSD").animate({
            scrollTop: $("#" + GridId + '_DXDataRow' + rowindex).offset().top + (-150)
        }, 1000);
        //$(".dxgvCSD").animate({ 
        //            scrollTop: $("#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_splitterPageEditor_grdvPageList_DXDataRow"+rowindex+"").offset().top + (-150)
        //}, 1000);

    });

    function UpdatePageTitle(titlebox) {
        var pageTitle = "<%=pageTitle %>";
        var title = $("#<%= txtPageTitle.ClientID%>").val();

        if (pageTitle != title)
            btnSaveConfiguration.DoClick();
    }
</script>

<dx:ASPxHiddenField ID="hdnSetting" ClientInstanceName="hdnSetting" runat="server"></dx:ASPxHiddenField>
<asp:Button ID="btEditPage" runat="server" CssClass="hide btEditPage" OnClick="btEditPage_Click" />
<asp:Button ID="btSortWebpart" runat="server" CssClass="hide btSortWebpart" OnClick="btSortWebpart_Click" />
<asp:Button ID="btAddWebpartOnPage" runat="server" CssClass="hide btAddWebpartOnPage" OnClick="btAddWebpartOnPage_Click" />
<dx:ASPxLoadingPanel ID="loadingPanel" ClientInstanceName="loadingPanel" runat="server" Modal="true" Text="Please Wait..."></dx:ASPxLoadingPanel>

<div class="divMain" style="visibility: hidden">
    <%--<div style="width: 100%; text-align: center;">
        <dx:ASPxButton ID="btnSwitch" runat="server" AutoPostBack="true" Text="Reverse" Visible="false" OnClick="btnSwitch_Click"></dx:ASPxButton>
    </div>--%>
    <div id="pageHeaderContainer" runat="server" class="page-heading">
        <dx:ASPxLabel ID="txtPageHeading" runat="server" Text="home.aspx"></dx:ASPxLabel>
    </div>
    <dx:ASPxSplitter runat="server" SeparatorVisible="true" ID="splitterPageEditor" ClientInstanceName="splitterPageEditor" ResizingMode="Live">
        <ClientSideEvents Init="splitterPageEditor_init" />
        <Panes>
            <dx:SplitterPane AutoHeight="true" PaneStyle-CssClass="leftSplitterPane" ScrollBars="None" AutoWidth="true" Size="230" Name="leftPane">
                <ContentCollection>
                    <dx:SplitterContentControl CssClass="spltrLeftContentControl" ID="spltrLeftContentControl" runat="server" SupportsDisabledAttribute="True">
                        <div class="page-left-Nav">
                            <div class="addnew-main">
                                <div class="addnew-container">
                                    <div class="fleft">
                                        <dx:ASPxTextBox Width="174px" NullText="New Page Name" CssClass="asptextBox-input" Height="25px" ID="txtNewPage" runat="server"></dx:ASPxTextBox>
                                    </div>
                                    <div class="fleft" style="margin-left:1px;">
                                        <dx:ASPxButton ID="btAddNewPage" Text="Add" OnClick="btAddNewPage_Click" runat="server" AutoPostBack="true" Height="15px" CssClass="primary-blueBtn">
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                            <div class="page-list">
                                <ugit:ASPxGridView EnableCallBacks="false" SettingsEditing-Mode="Inline" AutoGenerateColumns="False" KeyFieldName="ID" ID="grdvPageList" ClientInstanceName="grdvPageList" CssClass="customgridview homeGrid"
                                    runat="server" AllowSelectNode="true" AutoPostBack="false" OnFillContextMenuItems="grdvPageList_FillContextMenuItems"
                                    OnContextMenuItemClick="grdvPageList_ContextMenuItemClick" OnRowDeleting="grdvPageList_RowDeleting">
                                    <Columns>
                                        <dx:GridViewDataTextColumn Width="200px" Caption="Page Name" FieldName="Name" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior AllowSelectSingleRowOnly="true" AllowFocusedRow="false" AllowSelectByRowClick="true" AllowSort="true" />
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="0" />
                                    <SettingsSearchPanel Visible="true" />
                                    <ClientSideEvents SelectionChanged="function(s,e){grdvPageList_click(s,e);}"  />
                                    <SettingsContextMenu EnableRowMenu="true">
                                        <RowMenuItemVisibility NewRow="false" EditRow="false" Refresh="false" />
                                    </SettingsContextMenu>

                                    <Styles>
                                        <SelectedRow BackColor="#c6efef"></SelectedRow>
                                        <Row CssClass="homeGrid_dataRow"></Row>
                                        <Header CssClass="homeGrid_headerColumn"></Header>
                                    </Styles>


                                </ugit:ASPxGridView>
                            </div>
                        </div>
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
            <dx:SplitterPane AutoHeight="true" PaneStyle-CssClass="rightSplitterPane" Size="80%" MinSize="280px" Name="rightPane">
                <ContentCollection>
                    <dx:SplitterContentControl ID="spltrRightContentControl" runat="server" SupportsDisabledAttribute="True">
                        <asp:Panel ID="editContentPanel" runat="server" CssClass="page-right-Nav" Visible="false">
                            <div class="left-nav">
                                <div class="title">Left Navigation</div>
                                <dx:ASPxCallbackPanel CssClass="content" ID="AjaxPanelLeftPanel" runat="server">
                                    <PanelCollection>
                                        <dx:PanelContent>

                                            <div class="config-block">
                                                <div>
                                                    <%--//Show Left Navigation--%>
                                                    <dx:ASPxCheckBox ID="chkShowLeftNav" runat="server" Text="Enable Left Navigation" AutoPostBack="true" TextAlign="Right" OnCheckedChanged="chkShowLeftNav_CheckedChanged">
                                                        <ClientSideEvents
                                                            Init="function(s,e){ShowLeftNavigation(s,e); }"
                                                            CheckedChanged="function(s,e){ ShowLeftNavigation(s,e);}" />
                                                    </dx:ASPxCheckBox>
                                                </div>
                                                <div class="clsEnableLeftNavigation">
                                                    <dx:ASPxRadioButtonList ID="rdbenableleftnavigation" RepeatDirection="Vertical" TextWrap="true" ClientInstanceName="rdbenableleftnavigation" Border-BorderStyle="None" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdbenableleftnavigation_SelectedIndexChanged">
                                                        <Items>
                                                            <dx:ListEditItem Text="Show Menu" Selected="true" Value="0" />
                                                            <%--<dx:ListEditItem Text="Show DashBoard" Value="1"  />--%>
                                                        </Items>
                                                    </dx:ASPxRadioButtonList>
                                                </div>
                                                <div class="fleft leftmenuoption">
                                                    <dx:ASPxComboBox ID="ddlleftmenuoptions" NullText="Menu Name" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuList_SelectedIndexChanged" runat="server" Width="150px">
                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){loadingPanel.Show();}" />
                                                    </dx:ASPxComboBox>
                                                </div>
                                                <div class="leftnav-dashboardoption">
                                                    <div>
                                                        <dx:ASPxComboBox ID="ddlDashboardGroup" NullText="Dashboard Group" runat="server" Width="90%" AutoPostBack="true" OnSelectedIndexChanged="ddlDashboardGroup_SelectedIndexChanged">
                                                            <Items>
                                                                <dx:ListEditItem Text="Indivisible Dashboards" Value="Indivisible Dashboards" />
                                                                <dx:ListEditItem Text="Super Dashboards" Value="Super Dashboards" />
                                                                <dx:ListEditItem Text="Side Dashboards" Value="Side Dashboards" />
                                                                <dx:ListEditItem Text="Common Dashboards" Value="Common Dashboards" />
                                                            </Items>
                                                        </dx:ASPxComboBox>
                                                    </div>
                                                    <div>
                                                        <dx:ASPxComboBox AutoPostBack="true" OnSelectedIndexChanged="ddlDashboardView_SelectedIndexChanged" ID="ddlDashboardView" runat="server" Width="90%"></dx:ASPxComboBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>

                                </dx:ASPxCallbackPanel>
                            </div>
                            <div class="right-block">
                                <div class="header-container">
                                    <div class="title">Header</div>
                                    <div class="page-title">
                                        <asp:TextBox ID="txtPageTitle" runat="server" CssClass="all-input bg-light-blue" Width="250px" onblur="UpdatePageTitle(this);">
                                        </asp:TextBox>
                                    </div>
                                    <div style="padding-left:62px">
                                        <dx:ASPxCheckBox ID="chkHeader" runat="server" ClientInstanceName="chheader" AutoPostBack="true" OnCheckedChanged="chkHeaderSetting_CheckedChanged" Text="Enable Header">
                                        <ClientSideEvents CheckedChanged="function(s,e){loadingPanel.Show();}" />
                                    </dx:ASPxCheckBox>
                                    </div>
                                    
                                    <div class="fleft" style="display: none;">
                                        <dx:ASPxCheckBox ID="chkenablemenu" runat="server" ClientInstanceName="chkenablemenu" AutoPostBack="true" OnCheckedChanged="chkenablemenu_CheckedChanged" Text="Enable Menu">
                                            <ClientSideEvents CheckedChanged="function(s,e){loadingPanel.Show();}" />
                                        </dx:ASPxCheckBox>
                                    </div>
                                    <dx:ASPxCallbackPanel ID="ajaxPanelHeaderPane" CssClass="content search-container" runat="server">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <dx:ASPxPanel ID="panelHeaderOptions" CssClass="fullwidth" runat="server" Visible="false" ClientInstanceName="panelHeaderOptions">
                                                    <PanelCollection>
                                                        <dx:PanelContent>
                                                            <div class="fleft" style="display: none;">
                                                                <div class="fleft">
                                                                    <dx:ASPxCheckBox ID="chkShowHeader" CssClass="fleft" OnCheckedChanged="chkShowHeader_CheckedChanged" AutoPostBack="true" runat="server" Text="Show Menu">
                                                                        <ClientSideEvents CheckedChanged="function(s,e){loadingPanel.Show();}" />
                                                                    </dx:ASPxCheckBox>
                                                                </div>
                                                                <div class="fleft menu-option">
                                                                    <dx:ASPxComboBox ID="ddlMenuList" NullText="Menu Name" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuList_SelectedIndexChanged" runat="server" Width="150px" Visible="false">
                                                                        <ClientSideEvents SelectedIndexChanged="function(s,e){loadingPanel.Show();}" />
                                                                    </dx:ASPxComboBox>
                                                                </div>
                                                            </div>
                                                            <div class="fullwidth" style="margin-left: 1px;">
                                                                <dx:ASPxCheckBox ID="chkShowSearch" AutoPostBack="true" OnCheckedChanged="chkShowSearch_CheckedChanged" runat="server" Text="Show Search">
                                                                    <ClientSideEvents CheckedChanged="function(s,e){loadingPanel.Show();}" />
                                                                </dx:ASPxCheckBox>
                                                            </div>
                                                            <div class="fleft">
                                                                <dx:ASPxCheckBox ID="chkShowSetting" runat="server" ClientInstanceName="chkShowSetting" Visible="false" Text="Show Setting" AutoPostBack="true" OnCheckedChanged="chkShowSetting_CheckedChanged"></dx:ASPxCheckBox>
                                                            </div>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxPanel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                    <div>
                                    </div>
                                </div>
                                <div class="webpart">
                                    <div class="title">Content</div>
                                    <div class="add-bt" style="margin-top: -1px">
                                        <dx:ASPxButton ID="btAddWebpart" Height="16px" CssClass="primary-blueBtn" runat="server" AutoPostBack="false" Text="Add">
                                            <ClientSideEvents Click="function(s,e){btAddWebpart_Click(s,e);}" />
                                        </dx:ASPxButton>
                                    </div>
                                    <div class="content">
                                        <asp:Panel ID="panelContainer" runat="server" Height="600px">
                                            <ugit:DockPanelManager ID="abb" runat="server" ClientInstanceName="dockManager" OnClientLayout="abb_ClientLayout" OnAfterDock="abb_AfterDock">
                                            </ugit:DockPanelManager>
                                            
                                            <dx:ASPxDockPanel ID="testPanel" runat="server" AllowedDockState="DockedOnly" OwnerZoneUID="LeftZone" >

                                            </dx:ASPxDockPanel>
                                            
                                            <dx:ASPxDockZone runat="server" ID="ASPxDockZone1" ZoneUID="LeftZone" CssClass="leftzone" ClientInstanceName="LeftZone" Height="100%" Width="100%"
                                                PanelSpacing="3" AllowGrowing="false" Orientation="Vertical"  >
                                            </dx:ASPxDockZone>
                                            
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                            <div class="page-footer-nav">
                                <div class="title">Footer</div>
                                <div class="content">
                                    <dx:ASPxCheckBox ID="chkShowFooter" AutoPostBack="true" OnCheckedChanged="chkShowFooter_CheckedChanged" runat="server" Text="Enable Footer">
                                        <ClientSideEvents CheckedChanged="function(s,e){loadingPanel.Show();}" />
                                    </dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="actionBtn-wrap" style="bottom:4px !important;">
                                <dx:ASPxButton ID="btDeletePage" ClientInstanceName="btDeletePage" runat="server" OnClick="btDeletePage_Click" Text="Delete Page" CssClass="primary-blueBtn">
                                    <ClientSideEvents Click="function(s,e){btDeletePage_click(s,e);}" />
                                </dx:ASPxButton>

                                <dx:ASPxButton ID="btPreviewPage" ClientInstanceName="btPreviewPage" runat="server" AutoPostBack="false" Text="Preview" CssClass="primary-blueBtn">
                                    <ClientSideEvents Click="function(s,e){btPreviewPage_Click(s,e);}" />
                                </dx:ASPxButton>

                                <dx:ASPxButton ID="btnSaveConfiguration" ClientInstanceName="btnSaveConfiguration" runat="server" OnClick="btnSaveConfiguration_Click" Text="Save" CssClass="primary-blueBtn" ClientVisible="false">
                                </dx:ASPxButton>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="infoMessageBoard" runat="server" CssClass="infomessageboard">
                            Please select or create page to edit its properties.
                        </asp:Panel>
                    </dx:SplitterContentControl>

                </ContentCollection>
            </dx:SplitterPane>

        </Panes>
    </dx:ASPxSplitter>
</div>

<dx:ASPxPopupControl ID="addWebpartPopup" runat="server" Modal="True" Width="900px" Height="800px" ScrollBars="Vertical" PopupElementID="btAddWebpart"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="addWebpartPopup" EncodeHtml="false"
    HeaderText="Add Content" ShowHeader="false" AllowDragging="false" PopupAnimationType="Slide" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl13" runat="server">
            <dx:ASPxCardView KeyFieldName="Title" CssClass="webpartlist" ClientInstanceName="acvWebparts" ID="acvWebparts" runat="server" Settings-ShowCustomizationPanel="false">
                <Columns>
                    <dx:CardViewTextColumn FieldName="Name"></dx:CardViewTextColumn>
                    <dx:CardViewTextColumn FieldName="Title" Visible="false"></dx:CardViewTextColumn>
                </Columns>
                <SettingsPager Mode="ShowAllRecords">
                    <SettingsTableLayout ColumnCount="3" />
                </SettingsPager>
                <SettingsBehavior AllowSelectByCardClick="true" AllowSelectSingleCardOnly="true" />

                <Templates>
                    <Card>
                        <div style="text-align: center; padding: 5px;" class="cardtitle">
                            <%# Eval("Name") %>
                        </div>
                        <div style="text-align: center; padding: 5px;">
                            <input type="button" value="Add" onclick="addSelectedWebpart_Click('<%# Eval("Title") %>')" />
                        </div>
                    </Card>
                </Templates>
                <Styles>
                    <Card CssClass="card"></Card>
                </Styles>
            </dx:ASPxCardView>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ID="editWebpartPropertiesPopup" CssClass="aspxPopup" OnWindowCallback="editWebpartPropertiesPopup_WindowCallback" runat="server" Modal="True" Width="530px" AutoUpdatePosition="true"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="editWebpartPropertiesPopup" EncodeHtml="false"
    HeaderText="Edit Content" SettingsAdaptivity-Mode="Always" ShowFooter="true" ShowHeader="true" AllowDragging="true" PopupAnimationType="Fade" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="editWebpartPropertiesPopupContent" runat="server">
            <div>
                <dx:ASPxPanel ClientInstanceName="webpartPropsHolder" ID="webpartPropsHolder" runat="server">
                </dx:ASPxPanel>
                <asp:HiddenField ID="hdnEditWebpartIndex" runat="server" />
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <FooterStyle HorizontalAlign="Center" CssClass="aspxPopup-footer" />
    <FooterContentTemplate>
        <dx:ASPxButton CssClass="primary-blueBtn fright" ID="btSaveWebpartProps" runat="server" Text="Save" OnClick="btSaveWebpartProps_Click" style="margin-top:-3px;margin-bottom:2px;"></dx:ASPxButton>
    </FooterContentTemplate>
    <ClientSideEvents Shown="function(s,e){editWebpartPropertiesPopup_shown(s,e);}" />
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ClientInstanceName="confirmDeletePopup" Modal="true"
    ID="confirmDeletePopup"
    ShowFooter="false" ShowHeader="true" HeaderText="Confirm Delete Page"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">

    <ContentCollection>
        <dx:PopupControlContentControl ID="conformToClosePopup6" runat="server">

            <div style="float: left; height: 120px; width: 400px;" class="first_tier_nav">
                <table style="width: 100%;">
                    <tr>
                        <td style="text-align: center; height: 80px;">
                            <asp:Label ID="Label1" Text="Do you really want to delete" runat="server" OnLoad="Label1_Load"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="buttoncell" style="text-align: right;">
                            <dx:ASPxButton ID="btConfirmDeleteButton" runat="server" AutoPostBack="false" Text="Delete Page">
                                <ClientSideEvents Click="function(s,e){btConfirmDeleteButton_Click(s,e);}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btClose" runat="server" AutoPostBack="false" Text="Cancel">
                                <ClientSideEvents Click="function(s,e){confirmDeletePopup.Hide();}" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ClientSideEvents Closing="function(s,e){confirmDeletePopup_Closing(s,e);}" />
</dx:ASPxPopupControl>

<script type="text/javascript">
    loadingPanel.Show();
</script>




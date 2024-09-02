<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectResourceDetail.ascx.cs" Inherits="uGovernIT.Web.ProjectResourceDetail" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/RMM/CRMProjectAllocationViewNew.ascx" TagPrefix="ugit" TagName="CRMProjectAllocationViewNew" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript"  id="dxss_inlineCtrScriptProjectResource" data-v="<%=UGITUtility.AssemblyVersion %>">
  
    $(function () {
        
        var tabMode = $.cookie("TabName");
        if (tabMode == "TaskAllocation") {
            $("#divGridAllocation").show();
            $("#divProjectTeam").hide();
            allocationViewOptions.SetSelectedIndex(1);
        }
        else {
            $("#divGridAllocation").hide();
            $("#divProjectTeam").show();
            allocationViewOptions.SetSelectedIndex(0);
        }

        try {
            $(".jqtooltip").tooltip({
                hide: { duration: 4000, effect: "fadeOut", easing: "easeInExpo" },
                content: function () {
                    var title = $(this).attr("title");
                    if (title)
                        return title.replace(/\n/g, "<br/>");
                }
            });
        }
        catch (ex) {
        }
    });

    function addNewResource() {

        var moduleName = hdnInformation.Get("Module");
        var projectID = hdnInformation.Get("PublicID");
        var params = "projectID=" + projectID + "&module=" + moduleName;
        window.parent.UgitOpenPopupDialog(hdnInformation.Get("UpdateUrl"), params, 'New Resource for project: ' + projectID, '500px', '400px', 0, escape(hdnInformation.Get("RequestUrl")));
    }

    function editResource(s, e) {
        if (e.command == "STARTEDIT") {
        }
    }

    function gridAllocation_CustomButtonClick(s, e) {
        if (e.buttonID != 'editButton') return;

        var ctrl = getUrlParam("ctrl", '');
        var key = s.GetRowKey(e.visibleIndex);
        var url = hdnInformation.Get("UpdateUrl") + "&module=pmm&projectID=" + hdnInformation.Get("PublicID") + "&ItemID=" + key;
        var requestUrl = hdnInformation.Get("RequestUrl");
        if (ctrl == "PMM.ProjectCompactView")
            UgitOpenPopupDialog(url, '', 'Edit Allocation', '600', '300', 0, requestUrl, true);
        else
            window.parent.UgitOpenPopupDialog(url, '', 'Edit Allocation', '600', '440', 0, requestUrl, true);
    }

    function SyncAllocations(s, e) {
        if (confirm('This will DELETE all existing allocations for this project and recreate them based on the current task assignments. \n\nAre you sure you want to proceed?')) {
            LoadingPanel.SetText('Syncing, Please Wait ...');
            LoadingPanel.Show();
        }
        else {
            e.processOnServer = false;
        }
    }

    function allocationViewOptions_ValueChanged(s, e) {
 
        if (allocationViewOptions.GetSelectedItem() != null) {
            if (allocationViewOptions.GetValue() == 1 || allocationViewOptions.GetValue() == 2) {
                $("#divGridAllocation").show();
                $("#divProjectTeam").hide();
                $.cookie("TabName", "TaskAllocation");
                gridAllocation.Refresh();
            } else {
                $("#divGridAllocation").hide();
                $("#divProjectTeam").show();
                $.cookie("TabName","ProjectAllocation");
            }
        }
    }
</script>
<script type="text/javascript" id="dxss_inlineCtrScriptProjectResource1" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        try {
            gridAllocation.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            gridAllocation.SetHeight(containerHeight);
        } catch (e) {

        }
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<dx:ASPxHiddenField ID="hdnInformation" runat="server" ClientInstanceName="hdnInformation"></dx:ASPxHiddenField>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="min-height:350px !important">
    
    <div class="row">
         <dx:ASPxRadioButtonList Border-BorderWidth="0" CssClass="custom-radiobuttonlist" ID="allocationViewOptions" ClientInstanceName="allocationViewOptions" runat="server" AutoPostBack="false" RepeatDirection="Horizontal">
            <Items>
                <dx:ListEditItem Text="Project Allocation" Value="0" Selected="true" />
               <%-- <dx:ListEditItem Text="Consolidated" Value="1" />--%>
                <dx:ListEditItem Text="Task Assignments" Value="2" />
            </Items>
             <ClientSideEvents ValueChanged="allocationViewOptions_ValueChanged" />
        </dx:ASPxRadioButtonList>
    </div>
    
    <div id="divGridAllocation" style="display:none" class="row">
        <div class="row">
        <ugit:ASPxGridView ID="gridAllocation" runat="server" AutoGenerateColumns="False"
            OnDataBinding="gridAllocation_DataBinding" ClientInstanceName="gridAllocation"
            Width="100%" KeyFieldName="Id"
            OnCellEditorInitialize="gridAllocation_CellEditorInitialize"
            OnRowDeleting="gridAllocation_RowDeleting"
            OnRowUpdating="gridAllocation_RowUpdating" OnHtmlRowPrepared="gridAllocation_HtmlRowPrepared"
            OnRowValidating="gridAllocation_RowValidating" OnStartRowEditing="gridAllocation_StartRowEditing" 
            SettingsText-EmptyDataRow="No Resources Allocated" CssClass="customgridview homeGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <ClientSideEvents CustomButtonClick="gridAllocation_CustomButtonClick" />
            <Columns>
            </Columns>
             <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Header Font-Bold="true" CssClass=" homeGrid_headerColumn"></Header>
                <Row CssClass="homeGrid_dataRow"></Row>
            </Styles>
            <Settings GroupFormat="{1}" />
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
            <SettingsEditing Mode="Inline" />
            <SettingsBehavior ConfirmDelete="true" AllowDragDrop="false" AutoExpandAllGroups="true" ColumnResizeMode="Disabled" />
            <SettingsText ConfirmDelete="Are you sure you want to delete this allocation?" />
            <SettingsPopup HeaderFilter-Height="200"></SettingsPopup>
        </ugit:ASPxGridView>
        <script type="text/javascript">
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
        </script>
        </div>
        <div class="row addEditPopup-btnWrap">
        <dx:ASPxButton ID="lnkSync" runat="server" Text="Re-Sync" ToolTip="Re-Sync" OnClick="lnkSync_Click" CssClass="primary-blueBtn">
            <ClientSideEvents Click="function(s, e){ SyncAllocations(s, e);}" />
        </dx:ASPxButton>
    </div>
    </div>
        
        
    <div id="divProjectTeam" >    
        <ugit:CRMProjectAllocationViewNew runat="server" id="CRMProjectAllocationViewNew" />
        </div>
</div>





<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True">
</dx:ASPxLoadingPanel>

<%--<div class="nprResouceTab_btnWrap">
    <asp:LinkButton ID="lnkSync1" runat="server" Text="Sync" CssClass="" OnClientClick="return SyncAllocations()" OnClick="lnkSync_Click">
        <span class="nprResouceTab_btn">
            <b>Re-Sync</b>
            <i style="position: relative; top: -2px;left:2px">
        <img src="/_layouts/15/images/uGovernIT/refresh-icon.png"  style="border:none;" title="" alt=""/></i> 
            </span>
    </asp:LinkButton>
</div>--%>

<div style="float: right; padding-top: 5px;">
    <asp:HyperLink ID="newTask" Visible="false" runat="server" Text="Add New Resource" CssClass="fright" NavigateUrl="javascript:addNewResource()">
        <span class="button-bg">
            <b style="float: left; font-weight: normal;">
            Add Resource</b>
            <i
        style="float: left; position: relative; top: 1px;left:2px">
        <img src="/images/uGovernIT/add_icon.png"  style="border:none;" title="" alt=""/></i> 
            </span>
    </asp:HyperLink>
</div>

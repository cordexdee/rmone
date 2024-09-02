<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BenchTab.ascx.cs" Inherits="uGovernIT.Web.BenchTab" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-sweetalert/1.0.1/sweetalert.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css" />

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .reportPanelImage {
    /* display: inline-block; */
    float: right;
}
    .rmmExport-optionBtnWrap_ {
    padding-top: 5px !important;
    float: right;
}
    /*body input.dxeEditArea_UGITNavyBlueDevEx*/ /*Bootstrap correction*/ /*{
        color: inherit;
    }*/
    body {
        color: black;
    }

    .ModuleBlock {
        float: left;
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        position: absolute;
        z-index: 1;
    }

    .radiobutton label {
        margin-left: 5px;
        margin-right: 5px;
    }

    .checkboxbutton label {
        padding: 3px 0 0 3px;
    }
    .valueViewMode {
        display:flex;
    }
    /*.valueViewMode label {
        position:relative;
        top:2px;
    }*/
    .valueTypeMode {
        float: left;
        /*padding-left: 30px;*/
    }
    /*.valueTypeMode label {
        position: relative;
        top: 2px;
    }*/
    /*.viewProjectMode {
        float: left; padding-top: 5px;padding-left:5px;
    }*/
    .viewProjectMode label {
        position: relative;
        top: 2px;
        padding-right: 2px;
    }

    .valueComplexityMode {
        float: left;
        padding-left: 30px;
    }

    .rmmLookup-valueBoxEdit table.department tr td.dxic input[type="text"] {
        height: 20px !important;
        background: #fff;
    }

    .dxgvTable_Moderno, .dxgvFooter_Moderno td.dxgv {
        border: none !important;
    }

    .RMM-checkWrap input:checked + label::after {
        top: 6px;
        left: 8px;
    }

    .reportPanelImage {
        display: inline-block;
        padding-left: 5px;
    }

    .displayHide {
        display: none;
    }

    .ganttdataRow td {
        border-bottom: 0px solid #f6f7fb !important;
    }
    /*a{
        font-weight:800;
    }*/
    .preconbgcolor-constbgcolor{
        color:#FFFFFF !important;
        background: linear-gradient(to right, rgba(82, 190, 217, 1), rgba(0, 92, 155, 1));
    }

    .constbgcolor-closeoutbgcolor{
        color:#FFFFFF !important;
        background: linear-gradient(to right, rgba(0, 92, 155, 1), rgba(53, 27, 130, 1));
    }
    
    .preconbgcolor{
        background-color:#52BED9 !important;
        color:#ffffff !important;
    }
    .constbgcolor{
        background-color:#005C9B !important;
        color:#ffffff !important;
    }
    .closeoutbgcolor{
        background-color:#351B82 !important;
        color:#ffffff !important;
    }
    .ptobgcolor{
        background-color:#909090 !important;
    }
    .nobgcolor{
        background-color:#D6DAD9 !important;
        /*color:#ffffff !important;*/
    }
    .softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border: 1.5px dashed !important;
        border-left: hidden !important;
        border-right: hidden !important;
        color:#000000 !important
    }

    .RoundLeftSideCorner.softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        opacity:0.8;
        border-right:hidden !important;
    }
    .RoundRightSideCorner.softallocationbgcolor{
        background-color:#ecf1f9 !important;
        border:1.5px dashed !important;
        opacity:0.8;
        border-left:hidden !important;
    }
    .cmicnoLabel{
        font-weight:600;
        color:black;
    }
    .dxgvGroupRow_UGITNavyBlueDevEx td.dxgv{
        padding:0px 0px;
    }
    .RoundLeftSideCorner{
        border-top-left-radius: 12px;
        border-bottom-left-radius: 12px;
    }
    .RoundRightSideCorner{
        border-top-right-radius:12px;
        border-bottom-right-radius:12px;
    }

    .ptoAlignmentClass {
    vertical-align:initial;
    }

    .Checkbox {
        font: 13px 'Roboto', sans-serif;
        font-weight: 500;
    }

    .AllCheckBox{
        width: 100%;
    padding: 3px 5px 5px 5px;
    }

    #ddlDepartment .nprDropDown_actionBtnWrap{
        float:right;
    }

    .dxichTextCellSys {
        padding: 2px 0px 1px;
        font-size: 13px;
        font-weight: 400;
    }

    .square {
        height: 12px;
        width: 12px;
        margin-top:-12px;
    }
    .square1 {
        height: 12px;
        width: 12px;
        margin-top: -12px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    let assignUser = [];
    {
        function onGridInit(s, e) {
            displayGantt();
            var selectedKeys = gvResourceAvailablity.GetSelectedKeysOnPage(); // Get array of selected row keys (IDs)
            if (selectedKeys.length > 0) {
                for (var i = 0; i < selectedKeys.length; i++) {
                    assignUser.push(selectedKeys[i]);
                }
                displayGantt();
                //console.log("Selected Row IDs: ", selectedKeys);
            } else {
                //console.log("No rows selected");
            }
        }
        $(function () {
            
            $(".reportPanelImage").click(function () {
                
                if ($(".ExportOption-btns").is(":visible") == true)
                    $(".ExportOption-btns").hide();
                else
                    $(".ExportOption-btns").show();
                
            });

            $(".checkboxbutton").click(function () {
                ShowLoader();
            });
        });

        function adjustControlSize() {
            setTimeout(function () {
                $("#s4-workspace").width("100%");
                var height = $(window).height();
                $("#s4-workspace").height(height);
            }, 10);
        }

        function OnSelectionChanged(s, e) {
            var rowIndex = e.visibleIndex;
            var keyValue = gvResourceAvailablity.GetRowKey(rowIndex);
            if (gvResourceAvailablity._isRowSelected(rowIndex)) {
                if (!assignUser.includes(keyValue)) {
                    assignUser.push(keyValue);
                }
            } else {
                const index = assignUser.indexOf(keyValue);
                if (index > -1) {
                    assignUser.splice(index, 1);
                }
            }
            displayGantt();
            //console.log('Assign User:', assignUser);
        }
        function displayGantt() {
            if (assignUser.length > 0)
                btnOpenGantt.SetVisible(true);
            else
                btnOpenGantt.SetVisible(false);
        }

        function OnEndCallback(s, e) {
            ResourceAvailabilityloadingPanel.Hide();
            if (typeof (s.cpShowClearFilter) != 'undefined') {
                if (s.cpShowClearFilter == true) {

                    $('#' + s.cpClientID).show();
                }
                else {
                    $('#' + s.cpClientID).hide();
                }
            }
        }

        function ClickOnDrillDown(obj, fieldname, caption) {
            $('#<%= hdnSelectedDate.ClientID%>').val(fieldname);
            if (typeof $("[id$='btnDrilDown']").get(0) != 'undefined') {
                showLoader();
                $("[id$='btnDrilDown']").get(0).click();
            }
            else {
                return;
            }
        }

        function ClickOnDrillUP(obj, caption) {
            $('#<%= hdnSelectedDate.ClientID%>').val(caption);
            if (typeof $("[id$='btnDrilUp']").get(0) != 'undefined') {
                showLoader();
                $("[id$='btnDrilUp']").get(0).click();
            }
            else {
                return;
            }
        }

        function ClickOnPrevious(obj, caption) {
            $('#<%= previousYear.ClientID%>').click();
        }

        function ClickOnNext(obj, caption) {
            $('#<%= nextYear.ClientID%>').click();
        }
        function OpenAddAllocationPopup(obj, userName) {
            window.parent.UgitOpenPopupDialog('<%= absoluteUrlView %>' + '&SelectedUsersList=' + obj + '&groupId=' + $('#<%= hdnSelectedGroup.ClientID%>').val(), "", 'Add Allocation for ' + userName, '600px', '410px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
        function OpenCombinedMultiAllocationPopup(obj) {
            var userName = "";
            $.get("/api/RMOne/GetUserName?userId=" + obj, function (data, status) {
                userName = data;
                window.parent.parent.UgitOpenPopupDialog('<%= AddCombineMultiAllocationUrl %>' + '&SelectedUsersList=' + obj, "", 'Add Allocation for ' + userName, '880px', '700px', 0, escape("<%= Request.Url.AbsolutePath %>"));
            });
        }

        function openResourceAllocationDialog(path, titleVal, returnUrl) {
            <%if (IsCPRModuleEnabled)
    { %>

            let selectedUser = path.match(/(?<=\SelectedUsersList=).+?(?=\&)/g);
            //let elem = $("#" + selectedUser);
            //let userName = elem.parent().text();
            if (selectedUser != null) {
                OpenCombinedMultiAllocationPopup(selectedUser);
            }
            else {
                var title = titleVal.replace(/%20/g, ' ');
                window.parent.parent.UgitOpenPopupDialog(path, '', title, 80, 70, false, returnUrl);
            }
            <%}
    else
    { %>
            var title = titleVal.replace(/%20/g, ' ');
            window.parent.parent.UgitOpenPopupDialog(path, '', title, 80, 70, false, returnUrl);
            <%}%>
        }

        function showTaskActions(ID) {
            $("[id$='actionButtons" + ID + "']").css("display", "block");
        }

        function hideTaskActions(ID) {
            $("[id$='actionButtons" + ID + "']").css("display", "none");
        }

        function ShowLoader() {
            ResourceAvailabilityloadingPanel.SetText('Loading...');
            var width = window.innerWidth;
            var height = window.outerHeight;
            ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
        }

        function TypeCloseUp(s, e) {
            if ($('#<%= hdnTypeLoader.ClientID%>').val() != s.GetText()) {
                ShowLoader();
            }
        }


    }

    function showLoader() {
        ResourceAvailabilityloadingPanel.SetText('Loading...');
        var width = window.innerWidth;
        var height = window.outerHeight;
        ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
    }

    function ShowEditImage(objthis) {
        $(objthis).find('img').css('visibility', 'visible');
    }
    function HideEditImage(objthis) {
        $(objthis).find('img').css('visibility', 'hidden');
    }

    function onDepartmentChanged(ccID) {
        showLoader();
        var cmbDepartment = $("#" + ccID + " span");
        var selectedDepartments = cmbDepartment.attr("id");
        
        var selectedDepts = "";
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepts = selectedDepts + cmbDepartment[i].id + ",";
        document.getElementById('<%= hdnaspDepartment.ClientID %>').value = selectedDepts;
        cbpResourceAvailability.PerformCallback("LoadRoles~" + selectedDepts.substring(0, selectedDepts.length - 1));

        //cbpManagers.PerformCallback(selectedDepts);
        //gvResourceAvailablity.PerformCallback();
    }

    function cbpResourceAvailability_EndCallback(s, e) {
        cbpManagers.PerformCallback();
    }

    function InitiateGridCallback(s, e) {
        refreshGridDataUsingCallback();
    }

    function GetRoleSelectedValue(s,e) {
        let roleids = s.GetValue();

        let value = roleids != null ? roleids.join(';#') : "";
       
        
        $("#hdnRoleId").val(value);
        var path = "";
        $.cookie("roleid", value, { path: "/" });

    }

    function OnCmbManager_IndexChanged(s, e) {
            cbpManagers.PerformCallback();
    }
    function cbpManagers_EndCallback(s, e) {
            showLoader();
            gvResourceAvailablity.PerformCallback();
    }

    function OnColumnSorting(s, e) {
        <%generateColumns = true;%>
    }
    
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        showLoader();
        var rows = gvResourceAvailablity.GetVisibleRowsOnPage()
        
        if (rows > 22)
            gvResourceAvailablity.SetHeight(650);
        else if (rows == 0)
            gvResourceAvailablity.SetHeight(150);
        else
            gvResourceAvailablity.SetHeight((rows * 26) + 150);

        ResourceAvailabilityloadingPanel.Hide();
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });

    function LoaderOnExport() {
        ResourceAvailabilityloadingPanel.SetText('Exporting ...');
        ResourceAvailabilityloadingPanel.Show();
        setTimeout(function () { ResourceAvailabilityloadingPanel.Hide(); }, 5000);
    }

    function OpenNewAllocationGantt(s, e) {
        
        
        var rows = gvResourceAvailablity.GetVisibleRowsOnPage()

        if (rows > 150) {
            ResourceAvailabilityloadingPanel.SetText('Exporting' + '<br />' + 'Gantt Chart...');
            ResourceAvailabilityloadingPanel.Show();
            setTimeout(function () { ResourceAvailabilityloadingPanel.Hide(); }, 10000);
        }
        else {
            showLoader();
            setTimeout(function () { ResourceAvailabilityloadingPanel.Hide(); }, 7000);
        }
    }
    function onExportWithCallbackClick(exportType) {
        ResourceAvailabilityloadingPanel.SetText('Exporting' + '<br />' + 'Gantt Chart...');
        ResourceAvailabilityloadingPanel.Show();
        ExportCallback.PerformCallback(exportType);
    }
    function ExportCallbackComplete(s, e) {
        ResourceAvailabilityloadingPanel.Hide();
        response_btn.DoClick();
    }

    function chkAll_ValueChanged(s, e) {
        refreshGridDataUsingCallback();
    }

    function chkIncludeClosed_ValueChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function chkIncludeSoftAllocations_ValueChanged(s, e) {
        refreshGridDataUsingCallback();
    }

    function rbtnCount_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnPercentage_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnHrs_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnFTE_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnAvailability_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }

    function refreshGridDataUsingCallback() {
        showLoader();
        gvResourceAvailablity.PerformCallback();
    }

    function rbtnEstimate_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnAssigned_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }

    function cmbFunctionRole_Changed(s, e) {
        var functionid = s.GetValue();
        document.getElementById('<%= hdnaspFunction.ClientID %>').value = (functionid != null ? functionid.join(';#') : "");
        cbpResourceAvailability.PerformCallback("FunctionalRole | " + (functionid != null ? functionid.join(';#') : ""));
        //refreshGridDataUsingCallback();
    }

    
    function openResourceTimeSheet(s, e) {
        //debugger;
        let AssignedUsers = [];
        AssignedUsers = assignUser.join(','); //$.cookie("lstAssignUser").split(',');
        showTimeSheet = true;
        if (assignUser.length > 0) {
            var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&SelectedUsers=" + AssignedUsers.toString();
            window.parent.UgitOpenPopupDialog(url, "", "Timeline", "95", "95", "", false);
        } else {
            alert("Please select at least a few resources.");
        }
    }
    function OnGridFocusedRowChanged() {
        grid.GetRowValues(grid.GetFocusedRowIndex(), 'Id', OnGetRowValues);
    }
    function OnGetRowValues(values) {
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&SelectedUsers=" + values[0].toString();
        window.parent.UgitOpenPopupDialog(url, "", "Timeline", "95", "95", "", false);
    }

    function customFilterGridViewCallBack(s, e) {
        if (typeof (s.cpShowClearFilter) != 'undefined') {
            if (s.cpShowClearFilter == true) {

                $('#' + s.cpClientID).show();
            }
            else {
                $('#' + s.cpClientID).hide();
            }
        }
        if (typeof (s.cpAssignToMe) != 'undefined') {
            if (s.cpAssignToMe.length > 0) {
                alert(s.cpAssignToMe);
                s.cpAssignToMe = '';
            }
        }
        //$("html, body").animate({ scrollTop: $(".customgridview").scrollTop() }, 1200);

        //
        //console.log('Grid End');
        if (moduleName !== '') {
            var cols = moduleName + ';#';
            for (var i = 0; i < gridClientInstance.columns.length; i++) {
                if (gridClientInstance.columns[i]["visible"] === false)
                    cols = cols + gridClientInstance.columns[i]["fieldName"] + ',';
            }
            columnMoved.Set(moduleName + 'Columns', cols);
            $.cookie(moduleName + 'Columns', cols, { path: "/" });
        }
    }

    function clearFilterFromList() {
        gvResourceAvailablity.ClearFilter();
    }


</script>

<asp:HiddenField ID="hndClearFilter" runat="server" EnableViewState="true" />
<asp:HiddenField ID="hndFilterStatus" runat="server" />

<dx:ASPxLoadingPanel ID="ResourceAvailabilityloadingPanel" runat="server" ClientInstanceName="ResourceAvailabilityloadingPanel" Text="Loading..." ImagePosition="Top" CssClass="customeLoader" Modal="True">
    <Image Url="~/Content/Images/ajaxloader.gif"></Image>
</dx:ASPxLoadingPanel>

<asp:HiddenField ID="hdnTypeLoader" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12 noSidePadding reourceUti-container">
    <div class="row">
        <div id="divFilter" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                <div class="row resourceUti-filterWarp">
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Department:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit rmmDepartment-drpDown" 
                                FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />

                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Function:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <dx:ASPxGridLookup Visible="true" TextFormatString="{0}" SelectionMode="Multiple" ID="functionGridLookup"
                                runat="server" MultiTextSeparator="; " CssClass="rmmGridLookup" AllowUserInput="false"
                                GridViewProperties-Settings-VerticalScrollableHeight="175"
                                DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow"
                                Style="width: 100%">
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" SelectAllCheckboxMode="Page" ShowClearFilterButton="true" ToolTip="Select All" />
                                    <dx:GridViewDataTextColumn FieldName="Title" Width="200px" Caption="Choose Function:">
                                    </dx:GridViewDataTextColumn>
                                    
                                </Columns>
                                <GridViewProperties>
                                    <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                    <SettingsBehavior AllowSort="false" />
                                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    <SettingsText SelectAllCheckBoxInPageMode="Select All"></SettingsText>
                                </GridViewProperties>
                                <ClientSideEvents BeginCallback="function(s,e){cmbFunctionRole_Changed(s,e); }"  />
                            </dx:ASPxGridLookup>
                            <dx:ASPxComboBox visible="false" ID="cmbFunctionRole" runat="server" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="cmbFunctionRole_Changed" />
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Roles:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <dx:ASPxCallbackPanel ID="cbpResourceAvailability" ClientInstanceName="cbpResourceAvailability" runat="server" OnCallback="cbpResourceAvailability_Callback" SettingsLoadingPanel-Enabled="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxGridLookup SelectInputTextOnClick="false" KeyFieldName="ID" OnValueChanged="ddlUserGroup_SelectedIndexChanged" Visible="true" TextFormatString="{0}" SelectionMode="Multiple" ID="userGroupGridLookup"
                                            runat="server" MultiTextSeparator="; " CssClass="rmmGridLookup" AllowUserInput="false"
                                            GridViewProperties-Settings-VerticalScrollableHeight="175" AutoPostBack="true"
                                            DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow"
                                            Style="width: 100%">
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" SelectAllCheckboxMode="Page" ShowClearFilterButton="true" ToolTip="Select All" />
                                                <dx:GridViewDataTextColumn FieldName="Name" Width="200px" Caption="Choose Role:">
                                                </dx:GridViewDataTextColumn>
                                            </Columns>
                                            <GridViewProperties>
                                                <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior AllowSort="false" />
                                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                <SettingsText SelectAllCheckBoxInPageMode="Select All"></SettingsText>
                                            </GridViewProperties>
                                            <ClientSideEvents BeginCallback="function(s,e){ShowLoader();GetRoleSelectedValue(s,e); }" />
                                        </dx:ASPxGridLookup>
                                        <asp:DropDownList Visible="false" ID="ddlUserGroup" runat="server" AutoPostBack="true" onchange="ShowLoader();GetRoleSelectedValue();"
                                            OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged" CssClass="resourceUti-dropDownList aspxDropDownList rmm-dropDownList">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnaspDepartment" runat="server" />
                                        <asp:HiddenField ID="hdnaspFunction" runat="server" />
                                    </dx:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="cbpResourceAvailability_EndCallback" />
                            </dx:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView clearBoth-forSM">
                        <div class="resourceUti-label" id="tdManager" runat="server">
                            <label>Manager:</label>
                        </div>
                        <div class="resourceUti-inputField" id="tdManagerDropDown" runat="server">
                            <dx:ASPxCallbackPanel ID="cbpManagers" ClientInstanceName="cbpManagers" runat="server" SettingsLoadingPanel-Enabled="false" OnCallback="cbpManagers_Callback">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <dx:ASPxComboBox ClientInstanceName="resourceManager" runat="server" ID="cmbResourceManager" OnCallback="cmbResourceManager_Callback"
                                            DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" EnableSynchronization="true" CssClass="aspxComboBox-dropdown"
                                            ListBoxStyle-CssClass="aspxComboBox-listBox" AutoPostBack="false">
                                            <ClientSideEvents SelectedIndexChanged="OnCmbManager_IndexChanged" />
                                        </dx:ASPxComboBox>

                                    </dx:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents  EndCallback="cbpManagers_EndCallback" />
                            </dx:ASPxCallbackPanel>
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
        <div style="float: right; margin-left: 2px; padding-left: 5px; padding-right: 10px; display: none">
            <span style="padding-right: 5px;">
                <asp:ImageButton ImageUrl="/Content/images/Previous16x16.png" ID="previousYear" ToolTip="Prevoius" runat="server" OnClientClick="ShowLoader()"
                    OnClick="previousYear_Click" CssClass="resource-img" />
            </span>
            <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
            <span style="padding-left: 5px;">
                <asp:ImageButton ImageUrl="/Content/images/next-arrowBlue.png" ID="nextYear" ToolTip="Next" runat="server" OnClientClick="ShowLoader()" OnClick="nextYear_Click"
                    CssClass="resource-img" />
            </span>
        </div>
        <asp:HiddenField ID="hdnSelectedGroup" runat="server" />
    </div>

    <div class="row" style="padding-top: 5px; padding-bottom: 10px;">
        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12 colForTabView">
                        <div class="resourceUti-label" id="tdType" runat="server">
                            <label>Type:</label>
                        </div>
                            <div id="tdTypeGridLookup" runat="server" class="resourceUti-inputField">
                                <dx:ASPxGridLookup Visible="true" TextFormatString="{0}" SelectionMode="Multiple" ID="glType"
                                    runat="server" KeyFieldName="LevelTitle" MultiTextSeparator="; " CssClass="rmmGridLookup"
                                    GridViewProperties-Settings-VerticalScrollableHeight="175" AllowUserInput="false"
                                    DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow"
                                    Style="width: 100%">
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" SelectAllCheckboxMode="Page" ShowClearFilterButton="true" ToolTip="Select All" />
                                        <dx:GridViewDataTextColumn FieldName="LevelTitle" Width="200px" Caption="Choose Type:">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="LevelName" Visible="false"></dx:GridViewDataTextColumn>
                                    </Columns>
                                    <GridViewProperties>
                                        <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                        <SettingsBehavior AllowSort="false" />
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                        <SettingsText SelectAllCheckBoxInPageMode="Select All"></SettingsText>
                                    </GridViewProperties>
                                    <ClientSideEvents EndCallback="function(s,e){ InitiateGridCallback(s,e);}" />
                                </dx:ASPxGridLookup>
                            </div>
        </div>
        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12 pt-3">
            
                    <div class="rmmChkBox-container AllCheckBox" id="divCheckbox" runat="server">
                        <dx:ASPxCheckBox ID="chkAll" runat="server" AutoPostBack="false" Checked="true" Text="All Resources" Visible="false" CssClass="Checkbox">
                            <ClientSideEvents ValueChanged="chkAll_ValueChanged" />
                        </dx:ASPxCheckBox>
                        <dx:ASPxCheckBox ID="chkIncludeClosed" runat="server" AutoPostBack="false" Text="Closed Projects" CssClass="Checkbox">
                            <ClientSideEvents ValueChanged="chkIncludeClosed_ValueChanged" />
                        </dx:ASPxCheckBox>
                        <dx:ASPxCheckBox ID="chkIncludeSoftAllocations" runat="server" AutoPostBack="false" Text="Include Soft Allocations" CssClass="Checkbox">
                            <ClientSideEvents ValueChanged="chkIncludeSoftAllocations_ValueChanged" />
                        </dx:ASPxCheckBox>
                        <dx:ASPxCheckBox ID="chkOnlyNCO" runat="server" AutoPostBack="false" Text="Only NCO" CssClass="Checkbox">
                            <ClientSideEvents ValueChanged="chkIncludeSoftAllocations_ValueChanged" />
                        </dx:ASPxCheckBox>
                    </div>

        </div>
        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12 pt-3">
            <div class="valueViewMode bC-radioBtnWrap">
                 <dx:ASPxRadioButton ID="rbtnFTE" runat="server" AutoPostBack="false" Text="FTE" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                    <ClientSideEvents CheckedChanged="rbtnFTE_CheckedChanged" />
                </dx:ASPxRadioButton>
                <dx:ASPxRadioButton ID="rbtnHrs" runat="server" AutoPostBack="false" Text="Hrs" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                    <ClientSideEvents CheckedChanged="rbtnHrs_CheckedChanged" />
                </dx:ASPxRadioButton>
                 <dx:ASPxRadioButton ID="rbtnPercentage" runat="server" AutoPostBack="false" Text="%" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                    <ClientSideEvents CheckedChanged="rbtnPercentage_CheckedChanged" />
                </dx:ASPxRadioButton>
                <dx:ASPxRadioButton ID="rbtnItemCount" runat="server" AutoPostBack="false" Text="#" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                    <ClientSideEvents CheckedChanged="rbtnCount_CheckedChanged" />
                </dx:ASPxRadioButton>
                <dx:ASPxRadioButton ID="rbtnAvailability" runat="server" AutoPostBack="false" Text="Availability" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                    <ClientSideEvents CheckedChanged="rbtnAvailability_CheckedChanged" />
                </dx:ASPxRadioButton>
            </div>
        </div>

        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12 pt-3">
            <div class="allocationView bC-radioBtnWrap">
                <dx:ASPxRadioButton ID="rbtnBenchonly" runat="server" Text="Bench Only" AutoPostBack="true" Checked="true" OnValueChanged="rbtnBenchonly_ValueChanged"></dx:ASPxRadioButton>
                <dx:ASPxRadioButton ID="rbtnOverstaffedonly" runat="server" Text="Overstaffed Only" AutoPostBack="true" OnValueChanged="rbtnOverstaffedonly_ValueChanged"></dx:ASPxRadioButton>
                <%--<dx:ASPxCheckBox ID="chkBenchonly" runat="server" Text="Bench Only" AutoPostBack="true" Checked="true" OnValueChanged="chkBenchonly_ValueChanged" ></dx:ASPxCheckBox>
                <dx:ASPxCheckBox ID="chkOverstaffedonly" runat="server" Text="Overstaffed Only" AutoPostBack="true" OnValueChanged="chkOverstaffedonly_ValueChanged" ></dx:ASPxCheckBox>--%>
            </div>
            <div id="exportOption" runat="server" class="rmmExport-optionBtnWrap_">
                <div class="" style="float: left;margin-right: 10px;">
                    <dx:ASPxButton ID="btnOpengant" runat="server" Style="font-size: 0pt; display: none;" AutoPostBack="false" EnableTheming="false" RenderMode="Link" ClientInstanceName="btnOpenGantt"
                        Image-Url="/Content/Images/ganttBlackNew.png" ToolTip="View Gantt" Image-Height="20" Image-Width="20">
                        <ClientSideEvents Click="function(s,e){openResourceTimeSheet()}" />
                    </dx:ASPxButton>
                </div>
                    <div class="ExportOption-btns displayHide" style="padding-right: 5px;">
                        <dx:ASPxCallback ID="ExportCallback" ClientInstanceName="ExportCallback"  runat="server" OnCallback="ExportCallback_Callback">
                        <ClientSideEvents CallbackComplete="ExportCallbackComplete" />
                    </dx:ASPxCallback>
                    <div style="display: flex; flex-direction: row; justify-content: flex-start">
                        <dx:ASPxButton ID="btnExportPDF" runat="server" AutoPostBack="false" Visible="false" EnableTheming="false" RenderMode="Link"
                             ToolTip="Allocation Gantt" Image-Height="20" Image-Width="20">
                            <ClientSideEvents Click="function(s,e){onExportWithCallbackClick('pdf')}" />
                            <Image>
                            <SpriteProperties CssClass="pdf-icon" />
                        </Image>
                        </dx:ASPxButton>
                    </div>
                    <dx:ASPxButton ID="response_btn" runat="server" ClientInstanceName="response_btn" ClientVisible="false"
                        OnClick="response_btn_Click" />
                        <%--<dx:ASPxButton ID="imgNewGanttAllocation" runat="server" AutoPostBack="false" EnableTheming="false" ToolTip="Allocation Gantt"
                            Image-Url="/Content/Images/ganttBlackNew.png" RenderMode="Link" Image-Height="20" Image-Width="20" OnClick="imgNewGanttAllocation_Click">
                            <ClientSideEvents Click="OpenNewAllocationGantt" />
                        </dx:ASPxButton>--%>
                    </div>
                    <div class="ExportOption-btns displayHide">
                        <dx:ASPxButton ID="btnExcelExport" ClientInstanceName="btnExcelExport" runat="server" EnableTheming="false" UseSubmitBehavior="False"
                            OnClick="btnExcelExport_Click" RenderMode="Link" ToolTip="Export to Excel">
                            <Image>
                                <SpriteProperties CssClass="excelicon" />
                            </Image>
                            <ClientSideEvents Click="function(s, e) { LoaderOnExport(); _spFormOnSubmitCalled=false; }" />
                        </dx:ASPxButton>
                    </div>
                    <div class="ExportOption-btns displayHide" style="display:none !important">
                        <dx:ASPxButton ID="btnPdfExport" ClientInstanceName="btnPdfExport" runat="server" Visible="false" CssClass="export-buttons" EnableTheming="false" UseSubmitBehavior="False"
                            RenderMode="Link" OnClick="btnPdfExport_Click" ToolTip="Export to Pdf">
                            <Image>
                                <SpriteProperties CssClass="pdf-icon" />
                            </Image>
                            <ClientSideEvents Click="function(s, e) { LoaderOnExport(); _spFormOnSubmitCalled=false; }" />
                        </dx:ASPxButton>
                    </div>
                    <div class="reportPanelImage">                        
                        <dx:ASPxButton ID="imgReport" runat="server" RenderMode="Link" AutoPostBack="false" Image-Width="25" Image-Height="25"
                            Image-Url="~/Content/Images/reports-Black.png">
                        </dx:ASPxButton>
                    </div>
                 
               </div>
        </div>
        
        
    </div>
    <div class="col-xl-2 col-md-2 col-sm-6 col-xs-12 noSidePadding allocationTime-exportOptionWrap" style="float: right;display:none">
         </div>
        <div id="customMessageContainer" runat="server" style="display: none;" class="row">
            <div class="ugitlight1lighter" style="padding: 2px 0px;" align="center">
                <span id="customMessage" class="customfitler-message">Displaying Filtered View, <a href="javascript:void(0)" onclick="clearFilterFromList();">Click Here to Reset Filter</a></span>
            </div>
        </div>
    <div class="row">
        <div id="divProject" runat="server" class="viewProjectMode bC-radioBtnWrap" visible="true">
                <asp:RadioButton ID="rbtnProject" runat="server" AutoPostBack="true" Text="Project Allocation" onchange="showLoader()" Checked="true"
                    GroupName="ProjectAllocation" OnCheckedChanged="rbtnProject_CheckedChanged" CssClass="importChk-radioBtn" />
                <asp:RadioButton ID="rbtnAll" runat="server" AutoPostBack="true" Text="All Allocation" onchange="showLoader()"
                    GroupName="ProjectAllocation" OnCheckedChanged="rbtnAll_CheckedChanged" CssClass="importChk-radioBtn" />
         </div>
        <div class="respurceUti-gridContainer" style="margin-bottom: 250px;" id="grid">
            
            <dx:ASPxGridView ID="gvResourceAvailablity" runat="server" AutoGenerateColumns="false" KeyFieldName="Id"
                CssClass="homeGrid" ClientInstanceName="gvResourceAvailablity"
                OnDataBinding="gvResourceAvailablity_DataBinding" OnCustomSummaryCalculate="gvResourceAvailablity_CustomSummaryCalculate" 
                OnCustomCallback="gvResourceAvailablity_CustomCallback" OnBeforeHeaderFilterFillItems="gvResourceAvailablity_BeforeHeaderFilterFillItems"
                OnHeaderFilterFillItems="gvResourceAvailablity_HeaderFilterFillItems" 
                OnAfterPerformCallback="gvResourceAvailablity_AfterPerformCallback"
                
                SettingsText-EmptyDataRow="No Resource allocated." Width="100%" OnHtmlRowPrepared="gvResourceAvailablity_HtmlRowPrepared"  OnHtmlDataCellPrepared="gvResourceAvailablity_HtmlDataCellPrepared">
                <Columns>
                </Columns>

                <SettingsBehavior AllowGroup="false" AutoExpandAllGroups="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="false" />
                <Settings ShowGroupedColumns="false" ShowGroupFooter="VisibleIfExpanded" ShowFooter="true" VerticalScrollableHeight="550" VerticalScrollBarMode="Auto" />
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <SettingsLoadingPanel Mode="Disabled" />
                <ClientSideEvents SelectionChanged="OnSelectionChanged" EndCallback="OnEndCallback" FocusedRowChanged="function(s, e) { OnGridFocusedRowChanged(); }"/>
                <SettingsCommandButton>
                    <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </SettingsCommandButton>
                <Styles>
                    <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                    <Row CssClass="RMM-resourceUti-gridDataRow"></Row>
                    <Header CssClass="RMM-resourceUti-gridHeaderRow" Wrap="True"></Header>
                   <%-- <AlternatingRow BackColor="#f6f7fc"></AlternatingRow>--%>
                    <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px"
                        BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow">
                    </Footer>
                </Styles>
                <ClientSideEvents Init="onGridInit" />
            </dx:ASPxGridView>
            <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvResourceAvailablity" OnRenderBrick="gridExporter_RenderBrick"></dx:ASPxGridViewExporter>
            <script type="text/javascript">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
        </div>

        <div id="gantt" style="display: none;">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                    <dx:ASPxGridView ID="gvPreview" runat="server" AutoGenerateColumns="false" CssClass="gvPreview homeGrid" KeyFieldName="WorkItemID"
                        Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" EnableCallBacks="true" SettingsBehavior-SortMode="Custom"
                        OnCustomSummaryCalculate="gvPreview_CustomSummaryCalculate" ClientVisible="true"
                        OnHtmlDataCellPrepared="gvPreview_HtmlDataCellPrepared" >
                        <Columns>
                        </Columns>
                        <Templates>

                            <StatusBar>
                                <div id="editControlBtnContainer" style="display: none;">
                                    <asp:HyperLink ID="updateTask" runat="server" Text="Save Task Changes" CssClass="fleft updateTask savepaddingleft" OnClick="grid_BatchUpdate();">
                                                <span class="alloTimeSave-gridBtn">
                                                    <b style="font-weight: normal;">Save Changes</b>
                                                </span>
                                    </asp:HyperLink>
                                    <asp:HyperLink ID="cancelTask" runat="server" Style="padding: 10px 5px; float: right;" Text="Cancel Changes" CssClass="fleft" OnClick="grid_CancelBatchUpdate();">
                                                <span class="alloTimeCancel-gridBtn">
                                                    <b style="font-weight: 600;">Cancel Changes</b>
                                                </span>
                                    </asp:HyperLink>
                                </div>
                            </StatusBar>
                        </Templates>

                        
                        <Styles>
                            <Row CssClass="ganttdataRow"></Row>
                            <Header CssClass="gridHeader RMM-resourceUti-gridHeaderRow" />
                            <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                            <%--<GroupRow CssClass="gridGroupRow estReport-gridGroupRow" />--%>
                            <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                            <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px"
                                BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow">
                            </Footer>
                            <Table CssClass="timeline_gridview"></Table>
                            <Cell Wrap="True"></Cell>
                        </Styles>
                        <SettingsDataSecurity AllowDelete="false" AllowEdit="false" />
                        <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" />
                        <SettingsLoadingPanel Mode="Disabled" />
                        <SettingsPager PageSizeItemSettings-ShowAllItem="true">
                        </SettingsPager>
                        <Settings GroupFormat="{1}" ShowFooter="true" ShowStatusBar="Visible" UseFixedTableLayout="true" HorizontalScrollBarMode="Visible" />
                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" GridViewID="gvPreview" OnRenderBrick="ASPxGridViewExporter1_RenderBrick"></dx:ASPxGridViewExporter>
                </div>
            </div>
        </div>
    </div>
</div>

<div style="padding-left: 15px;">
    <div style="display: none;">
        <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" OnClientClick="ShowLoader()" />
        <asp:Button ID="btnDrilDown" runat="server" OnClick="btnDrilDown_Click" />
        <asp:Button ID="btnDrilUp" runat="server" OnClick="btnDrilUp_Click" />
    </div>
</div>

<asp:HiddenField ID="hdndtfrom" runat="server" />
<asp:HiddenField ID="hdndtto" runat="server" />
<asp:HiddenField ID="hdndisplayMode" runat="server" />
<asp:HiddenField ID="hdnSelectedDate" runat="server" />
<asp:HiddenField ID="hdnGenerateColumns" runat="server" Value="0" />



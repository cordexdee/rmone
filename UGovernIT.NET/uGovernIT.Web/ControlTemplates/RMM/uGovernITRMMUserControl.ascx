<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITRMMUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITRMMUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/CONTROLTEMPLATES/RMM/CustomResourceAllocation.ascx" TagPrefix="uGovernIT"  TagName="CustomResourceAllocation" %>
<%@ Register Src="~/CONTROLTEMPLATES/RMM/CustomResourceTimeSheet.ascx" TagPrefix="uGovernIT"  TagName="CustomResourceTimeSheet" %>
<%@ Register Src="~/CONTROLTEMPLATES/RMM/CustomGroupsAndUsersInfo.ascx" TagPrefix="uGovernIT"  TagName="CustomGroupsAndUsersInfo" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<asp:PlaceHolder runat="server">
<%= UGITUtility.LoadScript("../../Scripts/uGITCommon.js") %>
<%= UGITUtility.LoadStyleSheet("/Content/UGITCommonTheme.css") %>
<%= UGITUtility.LoadStyleSheet("/Content/UGITNewTheme.css") %>
</asp:PlaceHolder>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    body {
        color: black !important;
    }

    .tabmaindiv {
        float: left;
        width: 100%;
    }

    .tabmaindivinner {
        float: left;
    }

    .tabspan {
        float: left;
        padding: 6px;
        margin-right: 2px;
    }

    .primary-blueBtn  {
        background: #4A6EE2;
        color: #FFF;
        border: none;
        border-radius: 4px;
        padding: 3px 13px 2px 13px !important;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .containermain {
        float: left;
        width: 100%;
    }

    .containerblockmain {
        float: left;
        width: 100%;
        /*border: 1px solid #000;*/
    }

    /*.moduleinfo-m {
        float: left;
        width: 100%;
        padding-top: 9px;
        padding-bottom: 15px;
    }*/

    .reportDiv {
        float: right;
    }

    .ModuleBlock {
        background-color: #fff;
        border: 4px solid #ccc;
        padding: 10px; 
    }

    .filterpopup-table {
        border-collapse: collapse;
    }

    .filterpopup-table > tbody > tr > td {
        padding-bottom: 10px;
    }

    .filterpopup-table .filter-label {
        padding-right: 5px;
        font-weight: bold;
    }

    .button-red {
        float: none;
        padding: 0px;
    }

    .clsleft td {
        padding-left:0px !important;
    }

    .left-dev {
        text-align: right;
    }
    .cusWidth {
        width: 170px;
    }
    .cusWidth2 {
        width: 140px;
    }
    .form-group.department {
        min-width: auto !important;
        max-width: auto !important;
        margin-bottom: 0;
    }
    label {
        margin-left: 5px !important;
        margin-right: 0;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function adjustControlSize() {
        setTimeout(function () {
            $("#s4-workspace").width("100%");
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }

    function loadTabOnLoad(tabName) {
        var url = location.href;
        var previousurl = document.referrer;
        var arr = url.split('?');
        if (url.length > 1 && arr[1] != 'undefined') {
            if (localStorage.getItem("myindex") != -1) {
                actionOnTabClick('change', 0, 0);
            }
            else {
                var tab = tbcDetailTabs.GetTabByName(tabName);
                if (tab) {
                    tbcDetailTabs.SetActiveTabIndex(tab.index);
                    if (tab.index === 0)
                        actionOnTabClick('change', tab.index, tab.index);
                }
            }
        }
        else {
            var tab = tbcDetailTabs.GetTabByName(tabName);
            if (tab) {
                tbcDetailTabs.SetActiveTabIndex(tab.index);
                if (tab.index === 0)
                    actionOnTabClick('change', tab.index, tab.index);
            }
        }
    }

    function actionOnTabClick(actionType, tabIndex, activeTabIndex) {
        //debugger;
        var url = location.href;
        var previousurl = document.referrer;
        var arr = url.split('?');
        if (url.length > 1 && arr[1] != 'undefined') {
            localStorage.setItem("myindex", -1);
        }

        var clickTab = tbcDetailTabs.GetTab(tabIndex).name;
        var activeTab = tbcDetailTabs.GetTab(activeTabIndex).name;
       
        var activeTabContainer = $(".containermain[id='" + "tabPanel_" + activeTab + "']");
        if (actionType == "change") {

            $(".containermain").each(function (i, item) {
                $(item).hide();
            });
             $.cookie('RMMTabId', activeTab);
            $("#<%=hTabNumber.ClientID%>").val(activeTab);
        activeTabContainer.show();
        RefreshIframeData(activeTabContainer, true);

    }
    else if (actionType == "click") {

        if (tabIndex == activeTabIndex)
            RefreshIframeData(activeTabContainer, false);
    }

}

function RefreshIframeData(selectedContainer, ifNotLoaded) {
    var currentDate = new Date();
    if (selectedContainer != null) {
        var frames = $(selectedContainer).find("iframe");

        if (frames.length > 0) {
            for (var i = 0; i < frames.length; i++) {
                var frame = $(frames[i]);
                if (frame.attr("frameurl") != null) {
                    var url = frame.attr("frameurl");
                    var frameSrcUrl = frame.attr("src");
                    if (frameSrcUrl != undefined && frameSrcUrl != "" && ifNotLoaded) {
                        return;
                    }
                    frame.attr("src", url + "&timespan=" + currentDate.getTime());
                    var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Loading...</div></div></span>";
                    frame.parent().append(loadingElt);
                    frame.parent().css("position", "relative");
                }
            }
        }
    }
}

    function OpenReport(obj) {
        //debugger;
        if (selectedValue == "0") {
            alert('Please select the report type.');
            return false;
        }
        $("#pnlResourceReportForm").css({ 'display': 'none' });
        $("#pnlEstimatedRemaningHoursReportForm").css({ 'display': 'none' });


    var selectedValue = $("#<%=ddlReportView.ClientID%>").val();
        var sIndex = $("#<%=ddlReportView.ClientID%>").get(0).selectedIndex;
        var selectedOption = $("#<%=ddlReportView.ClientID%>").find("option").get(sIndex);
        var selectedText = $(selectedOption).text();

        if (selectedValue == "1") {
            $("#trResource").css({ 'display': 'none' });
            $("#pnlResourceReportForm").css({ 'top': ($(obj).position().top - 10) + 'px', 'display': 'block', 'left': ($(obj).position().left - $("#pnlResourceReportForm").width()) + $(obj).width() + 'px' });
            return false;
        }
        else if (selectedValue == "2") {
            $("#trResource").css({ 'display': 'block' });
            $("#pnlResourceReportForm").css({ 'top': ($(obj).position().top - 10) + 'px', 'display': 'block', 'left': ($(obj).position().left - $("#pnlResourceReportForm").width()) + $(obj).width() + 'px' });
            return false;
        }
        else if (selectedValue == "3") {
            if ($('#<%=ddlFunctionalArea.ClientID%>')[0].length > 0)
                $("#trFuncArea").show();
            else
                $("#trFuncArea").hide();

            $("#trResource").css({ 'display': 'none' });
            $("#pnlEstimatedRemaningHoursReportForm").css({ 'display': 'none' });
            $("#pnlResourceReportForm").css({ 'top': ($(obj).position().top - 10) + 80 + 'px', 'display': 'block', 'left': ($(obj).offsetParent()[0].offsetLeft - $(obj).position().left) - $(obj).width() - 140 + 'px' });
            $("#trStartWith").show();

            //$("#trManager").show();
            //$("#trFuncArea").show();
            return false;
        }
        else if (selectedValue == "4") {
            $("#pnlResourceReportForm").css({ 'display': 'none' });
            $("#pnlEstimatedRemaningHoursReportForm").css({ 'top': ($(obj).position().top - 10) + 80 + 'px', 'display': 'block', 'left': ($(obj).offsetParent()[0].offsetLeft - $(obj).position().left) - $(obj).width() - 140 + 'px' });
            //$("#pnlEstimatedRemaningHoursReportForm").css({ 'top': ($(obj).position().top - 10) + 'px', 'display': 'block', 'left': ($(obj).position().left - $("#pnlEstimatedRemaningHoursReportForm").width()) + $(obj).width() + 'px' });
            return false;
        }
        else if (selectedValue == "5") {
            var url = '<%= RUIurl%>';
            window.parent.parent.UgitOpenPopupDialog(url, '', 'Resource Utilization Summary', 95, 95);
            return false;
        }
        else if (selectedValue.indexOf("query_") != -1) {
            var query = selectedValue.replace("query_", "");
            selectedValue = query;
            var url = "<%= customQueryUrl%>";
            var title = "Report: " + selectedText;
            var params = "reportQueryID=" + selectedValue;

            window.parent.UgitOpenPopupDialog(url, params, title, 90, 90, true, "");
            return false;
        }
        else if (selectedValue == "6") {
            var benchReportUrl = '<%=BenchReportUrl%>';
            var title = "Bench Report";
            window.parent.UgitOpenPopupDialog(benchReportUrl, "", title, 90, 90, false, "");
            return false;
        }
}

function Cancel(obj) {
    $("#pnlResourceReportForm").css({ 'display': 'none' });
    return false;
}

function ERHCancel(obj) {
    $("#pnlEstimatedRemaningHoursReportForm").css({ 'display': 'none' });
    return false;
}

    function RunReport(obj) {
       // debugger;
        var approvedHoursOnly = chkApprovedOnly.GetChecked();
        var ddl = '<%=ddlDepartmentControl.ClientID%>';
        var cmbDepartment = $("#" + ddl +" span[code]");
       // var selectedDepartments = cmbDepartment.attr("id");

        var selectedDepartments = "";
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepartments = selectedDepartments + cmbDepartment[i].id + ",";

        var dateFrom = $("input[name*=dtDateFrom]").val();
        var dateTo = $("input[name*=dtDateTo]").val();
        var viewType = $("[id *=ddlView]").val();
        var reportType = $("[id *=ddlReportType]").val();
        var unittype = rdnUnit.GetValue();
        // var resourceManager = $("[id *=ddlResourceManager]").val();
        var ddlStartWith = $('#<%=ddlStartWith.ClientID%>').val();
        var url = "";
        var title = "";
        var selectedReport = $("#<%=ddlReportView.ClientID%>").val();
        var params = "";
        var paramsSkill = "";
        var groupbydepartment = chkgroupbydepartment.GetValue();
        var groupbyfunctionalarea = chkgroupbyfunctionalarea.GetValue();

        if (groupbydepartment == true) {
            params = params + "&GroupByDepartment=true";
        }
        if (groupbyfunctionalarea == true) {
            params = params + "&GroupByFunctionalArea=true";
        }

        params = params + "&ViewType=" + viewType;
        paramsSkill = paramsSkill + "&ViewType=" + viewType;
        params = params + "&ReportType=" + reportType;
        params = params + '&ApprovedHoursOnly=' + approvedHoursOnly;

        if (unittype != undefined && unittype != 'undefined' && unittype != null && unittype != '') {
            params = params + "&unitAllocAct=" + unittype;
            paramsSkill = paramsSkill + "&unitAllocAct=" + unittype;
        }
        if (selectedReport == "1") {
            url = "<%=RmmResourceReportUrl%>";
            title = "Resource Summary by Category";

        }
        else if (selectedReport == "2") {
            url = "<%=resourceByPersonReportUrl%>";
            title = "Resource Summary By Manager";
            params = params + "&control=ResourceWiseReport";
            // params = params + "&ResourceManagerID=" + resourceManager;
        }
        else if (selectedReport == "3") {
            if (ddlStartWith == "Skill") {
                url = "<%=resourceSkillUrl%>";
            }
            else {
                url = "<%=resourceUrl%>";
            }
            title = "Resource Summary";
        }

        var catLength = $(".chkBoxCategoryList :checked").length;
        var category = "";
        for (x = 0; x < catLength; x++) {
            var catName = $($($(".chkBoxCategoryList :checked")[x]).parent()[0]).find("label")[0].innerHTML;

            if (catName == '<%= ObjConfigurationVariableManager.GetValue("RMMLevel1NPRProjects")%>') {
                catName = '<%= uGovernIT.Utility.Constants.RMMLevel1NPRProjectsType%>';
            }
            else if (catName == '<%=ObjConfigurationVariableManager.GetValue("RMMLevel1PMMProjects")%>') {
                catName = '<%=uGovernIT.Utility.Constants.RMMLevel1PMMProjectsType%>';
            }
            else if (catName == '<%=ObjConfigurationVariableManager.GetValue("RMMLevel1TSKProjects")%>') {
                catName = '<%=uGovernIT.Utility.Constants.RMMLevel1TSKProjectsType%>';
            }

            if (category == "")
                category = catName;
            else
                category = category + "#" + catName;
        }

        category = escape(category);

        if (typeof selectedDepartments !== "undefined" && selectedDepartments !== "")
            params = params + "&Department=" + selectedDepartments;

        params = params + "&SelectedCategory=" + category;

        if (dateFrom != "") {
            params = params + "&DateFrom=" + dateFrom;
            params = params + "&DateTo=" + dateTo;

            paramsSkill = paramsSkill + "&DateFrom=" + dateFrom;
            paramsSkill = paramsSkill + "&DateTo=" + dateTo;
        }
        
        if (DrillDownToCategory.GetChecked() == true) {
            params = params + "&DrillDownTo=Category";
        }
        else if (DrillDownToPeople.GetChecked() == true) {
            params = params + "&DrillDownTo=People";
            paramsSkill = paramsSkill + "&DrillDownTo=People";
        }


        if (ddlStartWith.trim() != '') {
            params = params + "&StartWith=" + escape(ddlStartWith);
            paramsSkill = paramsSkill + "&StartWith=" + escape(ddlStartWith);
        }

        var ddlResourceManager = $('#<%=ddlResourceManager.ClientID%>');
        if (ddlResourceManager.val().trim() != '' && ddlResourceManager.val() != 'All') {
            params = params + "&Manager=" + escape(ddlResourceManager.val());
        }

        var ddlFunctionalArea = $('#<%=ddlFunctionalArea.ClientID%>');
        if (ddlFunctionalArea[0].length > 0) {
            if (ddlFunctionalArea.val().trim() != '' && ddlFunctionalArea.val() != 'All') {
                params = params + "&FuncArea=" + escape(ddlFunctionalArea.val());
            }
        }
        else
            params = params + "&FuncArea=hide";

        if (selectedReport == "3" && ddlStartWith == "Skill")
            url = url + paramsSkill;
        else
            url = url + params;
        window.parent.UgitOpenPopupDialog(url, "", title, 80, 80, false, "");
        //Cancel(obj);

        if (ddlStartWith == "Skill") {
            $("#trManager").hide();
            $("#trDepartment").hide();
            $("#trFuncArea").hide();
            $("#trReportType").hide();
            $("#trUnit").hide();
            $("#trSelectCategory").hide();
            //chkDrillDownToCategory.hide();
            //chkDrillDownToPeople.show();
            DrillDownToCategory.SetVisible(false);
            DrillDownToPeople.SetVisible(true);
            chkApprovedOnly.SetVisible(false);
        }
        else {
            $("#trManager").show();
            $("#trDepartment").show();
            if ($('#<%=ddlFunctionalArea.ClientID%>')[0].length > 0)
                $("#trFuncArea").show();
            else
                $("#trFuncArea").hide();
            $("#trReportType").show();
            $("#trUnit").show();
            $("#trSelectCategory").show();
            
            ShowApprovedHoursOnly();
        }
        return false;
    }

function RunERHReport(obj) {

    var dateFrom = $("input[name*=dtERHDateFrom]").val();
    var dateTo = $("input[name*=dtERHDateTo]").val();
    // var resourceManager = $("[id *=ddlResourceManager]").val();
    var urlERH = "";
    var titleERH = "";
    var paramsERH = "";

    url = "<%=resourceEstimatedRemainingHoursUrl%>";
    title = "Estimated Remaining Hours";

    if (dateFrom != "") {
        paramsERH = paramsERH + "&DateFrom=" + dateFrom;
        paramsERH = paramsERH + "&DateTo=" + dateTo;
    }

    // $("#ctl00_m_g_7bf137e2_10c3_4c67_a68f_37324f932d02_ctl00_ddlUserResource option:selected").val()

    var ddlUser = $('#<%=ddlUserResource.ClientID%> option:selected');
    if (ddlUser.val().trim() != '' && ddlUser.val() != 'All') {
        paramsERH = paramsERH + "&Manager=" + escape(ddlUser.val()) + "&ManagerName=" + ddlUser.text();
    }

    url = url + paramsERH;
    window.parent.UgitOpenPopupDialog(url, "", title, 80, 80, false, "");
    ERHCancel(obj);
    return false;
}

function selectAll(obj) {
    $(".chkBoxCategoryList input[type='checkbox']").each(function () {
        $(this).attr("checked", $("#<%= chkSelectAll.ClientID%>").get(0).checked);
    })
}

    function ShowDrillDownOption(obj) {
    var chkDrillDownToPeople = $('#dvDrillDownToPeople');
    var chkDrillDownToCategory = $('#dvDrillDownToCategory');
    //var chkDrillDownToPeople_ = $('#<%=chkDrillDownToPeople.ClientID%>');
    var chkDrillDownToCategory_ = $('#<%=chkDrillDownToCategory.ClientID%>');

    var startwith = $('#<%=ddlStartWith.ClientID%>').val();
    if ($('#<%=ddlFunctionalArea.ClientID%>')[0].length > 0)
        $("#trFuncArea").show();
    else
        $("#trFuncArea").hide();

    if (startwith != "") {
        if (startwith == "Manager/People") {
            chkDrillDownToCategory.show();
            chkDrillDownToCategory_.show();
            //chkDrillDownToPeople.hide();
            DrillDownToCategory.SetVisible(true);
            DrillDownToPeople.SetVisible(false);
            $("#trDepartment").show();
            $("#trManager").show();
            $("#trReportType").show();
            $("#trUnit").show();
            $("#trSelectCategory").show();
        }
        else if (startwith == "Skill") {
            chkDrillDownToCategory.hide();
            //chkDrillDownToPeople.show();
            DrillDownToCategory.SetVisible(false);
            DrillDownToPeople.SetVisible(true);
            $("#trDepartment").hide();
            $("#trManager").hide();
            $("#trFuncArea").hide();
            $("#trReportType").hide();
            $("#trUnit").hide();
            $("#trSelectCategory").hide();
            chkApprovedOnly.SetVisible(false);
        }
        else {
            chkDrillDownToCategory.hide();
            //chkDrillDownToPeople.show();
            DrillDownToCategory.SetVisible(false);
            DrillDownToPeople.SetVisible(true);
            $("#trDepartment").show();
            $("#trManager").show();
            $("#trReportType").show();
            $("#trUnit").show();
            $("#trSelectCategory").show();
            ShowApprovedHoursOnly();
        }
    }
}

$(function () {
    var startwith = $('#<%=ddlStartWith.ClientID%>').val();
    if (startwith != 'Skill')
        ShowApprovedHoursOnly();
    else
        chkApprovedOnly.SetVisible(false);
});

    function ShowApprovedHoursOnly() {
        var viewType = $("[id *=ddlView]").val();
        var reportType = $("[id *=ddlReportType]").val();
        if (viewType == "Weekly" && (reportType == 'Actuals' || reportType == 'AllocationsActuals')) {
            chkApprovedOnly.SetVisible(true);
        }
        else {
            chkApprovedOnly.SetVisible(false);
            chkApprovedOnly.SetChecked(false)
        }
    }

    function btnFindResource_click(s, e) {
        var url = "<%=  UGITUtility.GetDelegateUrl("RMM.PickWorkItem", true, "Find Resource")%>";
        window.parent.UgitOpenPopupDialog(url, "", "Find Resource", "500px", "400px", false, "");
    }

    // disabling Enter Key press event, when search word entered in Global Search & pressing Enter key is causing entire Page Reload.
    //PreventEnterKeyPressOnButtons();
    $(document).ready(function () {
        document.onkeypress = function (evt) {
            return ((evt) ? evt : ((event) ? event : null)).keyCode != 13;
        };
    });

    $(document).ready(function () {
        $('#<%=ddlReportView.ClientID%>').change(function (e) {
            if (this.value != '0')
                return OpenReport(this);
        });
    });

</script>

<asp:HiddenField ID="hdnCategory" runat="server" />

<div id="pnlResourceReportForm" style="display: none;" class="ModuleBlock">
    <div>
        <h3 class="mt-0 text-center mb-4">Resource Summary Report</h3>
        <table cellspacing="10px" cellpadding="5px">
            <tr>
                <td class="titleTD" width="100%" align="center" style="text-align: center">
                    <asp:Panel ID="componentsForm" runat="server" Style="float: left; padding-left: 3px; width: 100%; text-align: center;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <table width="500px" class="filterpopup-table" frame="box" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td class="labelClass">
                                                    <asp:Label ID="lblDateFrom" CssClass="filter-label" Text="Date From:" runat="server"></asp:Label>
                                                </td>
                                                <td class="DateControlDateFrom">
                                                    <dx:ASPxDateEdit ID="dtDateFrom" runat="server" DisplayFormatString="MM/d/yyyy"></dx:ASPxDateEdit>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="labelClass">
                                                    <asp:Label ID="lblDateTo" CssClass="filter-label" Text="Date To:" runat="server"></asp:Label>
                                                </td>
                                                <td class="DateControlDateTo">
                                                    <dx:ASPxDateEdit ID="dtDateTo" runat="server" DateOnly="true" DisplayFormatString="MM/d/yyyy"></dx:ASPxDateEdit>
                                                </td>
                                            </tr>

                                            <tr id="trStartWith" style="display: none;">
                                                <td class="labelClass">
                                                    <asp:Label ID="lblStartWith" CssClass="filter-label" Text="Start With:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <div class="d-flex align-items-center justify-content-between">
                                                        <asp:DropDownList ID="ddlStartWith" CssClass="rmm-dropDownList cusWidth" runat="server" onchange="ShowDrillDownOption(this);">
                                                            <asp:ListItem Text="Work Category" Value="Work Category"></asp:ListItem>
                                                            <asp:ListItem Text="Manager/People" Value="Manager/People"></asp:ListItem>
                                                            <asp:ListItem Text="Skill" Value="Skill"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <div class="cusWidth2">
                                                            <div id="dvDrillDownToPeople">
                                                                <dx:ASPxCheckBox ID="chkDrillDownToPeople" runat="server" ClientInstanceName="DrillDownToPeople"
                                                                    ToggleSwitchDisplayMode="Always" Text="Drill-down to people?" Theme="iOS" Checked="false" >
                                                                </dx:ASPxCheckBox>
                                                                <%--<asp:CheckBox ID="chkDrillDownToPeople" runat="server" Text="Drill-down to people?" />--%>
                                                            </div>
                                                            <div id="dvDrillDownToCategory" style="display: none;">
                                                                <dx:ASPxCheckBox ID="chkDrillDownToCategory" runat="server" Text="Drill-down to category?"
                                                                    ClientInstanceName="DrillDownToCategory" ToggleSwitchDisplayMode="Always" Theme="iOS" Checked="false" >
                                                                </dx:ASPxCheckBox>
                                                                <%--<asp:CheckBox ID="chkDrillDownToCategory" runat="server" Text="Drill-down to category?" />--%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr id="trManager">
                                                <td class="labelClass">
                                                    <asp:Label ID="lblManager" CssClass="filter-label" Text="Manager:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlResourceManager" runat="server" CssClass="rmm-dropDownList cusWidth"></asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr id="trDepartment">
                                                <td class="labelClass">
                                                    <asp:Label ID="lbldapartment" CssClass="filter-label" Text="Department(s):" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <div class="d-flex align-items-center justify-content-between">
                                                        <ugit:LookupValueBoxEdit ID="ddlDepartmentControl" IsMulti="true"  runat="server" FieldName="DepartmentLookup" CssClass="cusWidth"></ugit:LookupValueBoxEdit>
                                                        <div class="cusWidth2">
                                                            <dx:ASPxCheckBox ID="chkgroupbydepartment" ToolTip="Group By Deparment" Text="Group By" runat="server" 
                                                                ClientInstanceName="chkgroupbydepartment" Theme="iOS" ToggleSwitchDisplayMode="Always" Checked="false"></dx:ASPxCheckBox></div>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr id="trFuncArea">
                                                <td class="labelClass">
                                                    <asp:Label ID="lblFunctionalArea" CssClass="filter-label" Text="Functional Area:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <div class="d-flex align-items-center justify-content-between">
                                                        <asp:DropDownList CssClass="rmm-dropDownList cusWidth" ID="ddlFunctionalArea" runat="server"></asp:DropDownList>
                                                        <div class="cusWidth2">
                                                            <dx:ASPxCheckBox ID="chkgroupbyfunctionalarea" ToolTip="Group By Functional Area" Text="Group By" 
                                                                runat="server" ClientInstanceName="chkgroupbyfunctionalarea" Theme="iOS" ToggleSwitchDisplayMode="Always" Checked="false"></dx:ASPxCheckBox></div>
                                                    </div>
                                                </td>
                                            </tr>


                                            <tr id="trReportType">
                                                <td class="labelClass">
                                                    <asp:Label ID="lbl" CssClass="filter-label" Text="Report Type:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlReportType" runat="server" onchange="ShowApprovedHoursOnly();" CssClass="rmm-dropDownList cusWidth">
                                                        <asp:ListItem Text="Allocation Only" Value="Allocation"></asp:ListItem>
                                                        <asp:ListItem Text="Actuals Only" Value="Actuals"></asp:ListItem>
                                                        <asp:ListItem Text="Allocations & Actuals" Value="AllocationsActuals"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="trUnit">
                                                <td class="labelClass">
                                                    <asp:Label ID="Label1" CssClass="filter-label" Text="Unit:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <div class=" d-flex align-items-center flex-wrap">
                                                        <div >
                                                        <dx:ASPxRadioButtonList ID="rdnUnit" runat="server" ClientInstanceName="rdnUnit" CssClass=" clsleft" RepeatDirection="Horizontal" >
                                                            <Border BorderStyle="None" />
                                                            <Items>
                                                                <dx:ListEditItem Text="FTEs" Value="ftes" Selected="true"/>
                                                                <dx:ListEditItem Text="Hours" Value="hours" />
                                                            </Items>
                                                        </dx:ASPxRadioButtonList>
                                                        </div>
                                                        <div>
                                                            <dx:ASPxCheckBox ID="chkApprovedOnly" ToolTip="Approved Hours Only" Text="Approved Hours Only" runat="server" 
                                                                ClientInstanceName="chkApprovedOnly" Theme="iOS" ToggleSwitchDisplayMode="Always" Checked="false"></dx:ASPxCheckBox>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>

                                            <tr id="trSelectCategory">
                                                <td class="labelClass">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="labelClass" style="text-align: right;">
                                                                <asp:Label ID="lblSelectCategory" CssClass="filter-label" Text="Select Category:"
                                                                    runat="server"></asp:Label>
                                                            </td>
                                                            <td class="text-right pr-2 pt-1">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" Checked="true" Text="All" onclick="selectAll()" />
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </td>
                                                <td class="chkBoxCategoryList">
                                                    <asp:CheckBoxList ID="chkBoxCategoryList" runat="server"></asp:CheckBoxList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="labelClass">
                                                    <asp:Label ID="lblViewType" CssClass="filter-label" Text="Select View:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlView" runat="server" CssClass="rmm-dropDownList cusWidth">
                                                        <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                                        <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelClass" style="text-align: right; padding-top: 5px;">
                                        <asp:Button CssClass="action-btn" ID="btnRun" runat="server" Text="Run" OnClientClick="return RunReport(this)" />
                                        <asp:Button CssClass="btnBlue-Secondary" ID="btnCancel" runat="server" Text="Cancel" OnClientClick="return Cancel(this);" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="lblMessage" runat="server" Visible="false" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</div>

<div id="pnlEstimatedRemaningHoursReportForm" style="display: none;" class="ModuleBlock">
    <fieldset>
        <legend>Estimated Remaining Hours Report</legend>
        <table cellspacing="10px" cellpadding="5px">
            <tr>
                <td class="titleTD" width="100%" align="center" style="text-align: center">
                    <asp:Panel ID="Panel1" runat="server" Style="float: left; padding-left: 3px; width: 100%; text-align: center;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <table width="400px" class="filterpopup-table" frame="box" cellpadding="0" cellspacing="0" border="0">

                                            <tr id="tr2">
                                                <td class="labelClass" style="width: 30%; text-align: left;">
                                                    <asp:Label ID="lblUserName" CssClass="filter-label" Text="Resource:" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlUserResource" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="labelClass">

                                                    <asp:Label ID="lblERHDateFrom" CssClass="filter-label" Text="Date From:" runat="server"></asp:Label>

                                                </td>
                                                <td class="DateControlDateFrom">
                                                    <dx:ASPxDateEdit ID="dtERHDateFrom" runat="server" DateOnly="true" DisplayFormatString="MMM d, yyyy"></dx:ASPxDateEdit>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td class="labelClass" style="width: 30%; text-align: left;">

                                                    <asp:Label ID="lblERHDateTo" CssClass="filter-label" Text="Date To:" runat="server"></asp:Label>

                                                </td>
                                                <td class="DateControlDateTo">
                                                    <dx:ASPxDateEdit ID="dtERHDateTo" runat="server" DateOnly="true" DisplayFormatString="MMM d, yyyy"></dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="labelClass" style="text-align: right; padding-top: 5px;">
                                        <asp:Button ID="btnERHRun" runat="server" Text="Run" OnClientClick="return RunERHReport(this)" />
                                        <asp:Button ID="btnERHCancel" runat="server" Text="Cancel" OnClientClick="return ERHCancel(this);" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="Label9" runat="server" Visible="false" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>



<div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
    <div class="row" id="helpTextRow" runat="server">
        <div class="paddingleft8" align="right">
            <asp:Panel ID="helpTextContainer" runat="server">
            </asp:Panel>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-s -12 col-xs-12 noSidePadding">
            <div class="moduleinfo-m rmmHome-title">
                <div>
                    <div class="moduleimgtd" id="cModuleImgPanel" runat="server" width="40">
                        <asp:Image runat="server" ID="moduleLogo" />
                    </div>
                    <div class="moduledesciptiontd rmmModule-discription">
                        <asp:Literal runat="server" ID="moduleDescription" Text=""></asp:Literal>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hTabNumber" runat="server" />
            <div class="tabmaindiv row bottomBorder">
                <div class="col-md-10 noSidePadding">
                    <asp:Panel ID="pFilteredData" runat="server" Visible="true" CssClass="dxtcLite_UGITNavyBlueDevEx">
                    <dx:ASPxTabControl ID="tbcDetailTabs" ClientInstanceName="tbcDetailTabs"  runat="server" Visible="true" Theme="Default" >
                        <ClientSideEvents ActiveTabChanged="function(s,e){ actionOnTabClick('change', e.tab.index, s.activeTabIndex);}" TabClick="function(s,e){ actionOnTabClick('click', e.tab.index, s.activeTabIndex);}" />
                        <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px" CssClass="customFilteredTabStyle">
                            <Paddings PaddingLeft="10px" PaddingRight="10px"></Paddings>
                        </TabStyle>
                    </dx:ASPxTabControl>
                        </asp:Panel>
                </div>

                <div class="col-md-2 next-cancel-but">
                    <dx:ASPxButton ID="btnFindResource" runat="server" Text="Find Resource" AutoPostBack="false" CssClass="next" Visible="false">
                        <Image SpriteProperties-CssClass="dx-vam" Url="/Content/images/search-white.png" Width="14px"></Image>
                        <ClientSideEvents Click="function(s,e){btnFindResource_click(s,e);}" />
                    </dx:ASPxButton>
                    <div id="dvResourceReport" runat="server" class="reportDiv">
                        <span>
                            <asp:DropDownList ID="ddlReportView" CssClass="aspxDropDownList rmm-dropDownList" runat="server" Width="200px" Height="24px" OnLoad="ddlReportView_Load">
                                <asp:ListItem Text="--Select Report--" Value="0"></asp:ListItem>
                                <%-- <asp:ListItem Text="Resource Summary by Category" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Resource Summary by Manager" Value="2"></asp:ListItem>--%>
                                <asp:ListItem Text="Resource Summary" Value="3"></asp:ListItem>
                               <%-- <asp:ListItem Text="Estimated Remaining Hours" Value="4"></asp:ListItem>--%>
                                <asp:ListItem Text="Resource Utilization Summary" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Bench Report" Value="6"></asp:ListItem>
                            </asp:DropDownList>
                        </span>
                         
                        <span style="display:none">
                            <asp:Button ID="btnShowReport" runat="server" Text="Run" CssClass="primary-blueBtn" Visible="false"
                                OnClientClick="return OpenReport(this)" />
                        </span>
                    </div>
                </div>
            </div>

            <div class="containerblockmain">
                <div style="display: none;" class="containermain" id="tabPanel_ResourceAllocation">
                    <asp:Panel ID="tabPanelconainerAllocation" runat="server">
                    </asp:Panel>
                </div>
                <div style="display: none;" class="containermain" id="tabPanel_Actuals">
                    <asp:Panel ID="tabPanelconainerActual" runat="server">
                    </asp:Panel>
                </div>

                <div style="display: none;" class="containermain" id="tabPanel_Resources">
                    <asp:Panel ID="tabPanelconainerUsers" runat="server">
                    </asp:Panel>
                </div>

                <div style="display: none;" class="containermain" id="tabPanel_ResourcePlanning">
                    <asp:Panel ID="tabPanelresourcePlanning" runat="server">
                    </asp:Panel>
                </div>

                <div style="display:none;" class="containermain" id="tabPanel_ResourceAvailability">
                    <asp:Panel ID="tabPanelresourceavailability" runat="server">
                    </asp:Panel>
                </div>
                <div style="display: none;" class="containermain" id="tabPanel_AllocationTimeline">
                    <asp:Panel ID="tabPanelAllocationTimeline" runat="server">
                    </asp:Panel>
                </div>
                <div style="display:none;" class="containermain" id="tabPanel_ProjectComplexity">
                    <asp:Panel ID="tabPanelProjectComplexity" runat="server">
                    </asp:Panel>
                </div>

                 <div class="containermain" id="tabPanel_CapacityReport">
                    <asp:Panel ID="tabPanelCapacityReport" runat="server">
                    </asp:Panel>
                </div>

                <div style="display:none;" class="containermain" id="tabPanel_BillingAndMargins">
                    <asp:Panel ID="tabPanelBillingAndMargins" runat="server">

                    </asp:Panel>
                </div>

                <div style="display:none;" class="containermain" id="tabPanel_ExecutiveKpi">
                    <asp:Panel ID="tabPanelExecutiveKpi" runat="server">
                    </asp:Panel>
                </div>

                <div style="display:none;" class="containermain" id="tabPanel_ResourceUtilizationIndex">
                    <asp:Panel ID="tabPanelResourceUtilizationIndex" runat="server">
                    </asp:Panel>
                </div>

                <div style="display:none;" class="containermain" id="tabPanel_ManageAllocationTemplates">
                    <asp:Panel ID="tabPanelManageAllocationTemplates" runat="server">
                    </asp:Panel>
                </div>

                <div style="display:none;" class="containermain" id="tabPanel_Bench">
                    <asp:Panel ID="tabPanelBench" runat="server">

                    </asp:Panel>
                </div>
                <%--<div style="display:none;" class="containermain" id="tabPanel_FinancialView">
                    <asp:Panel ID="tabPanelFinancialView" runat="server">
                </asp:Panel>
                </div>--%>
            </div>


            <%--<div class="containerblockmain" style="min-height:600px;">
                <div style="display: none;" class="containermain" id="tabPanel_1">
                    <asp:Panel ID="tabPanelconainerAllocation" runat="server">
                    </asp:Panel>
                </div>
                <div style="display: none;" class="containermain" id="tabPanel_2">
                    <asp:Panel ID="tabPanelconainerActual" runat="server">
                    </asp:Panel>
                </div>

                <div style="display: none;" class="containermain" id="tabPanel_3">
                    <asp:Panel ID="tabPanelconainerUsers" runat="server">
                    </asp:Panel>
                </div>

                <div style="display: none;" class="containermain" id="tabPanel_4">
                    <asp:Panel ID="tabPanelresourcePlanning" runat="server">
                    </asp:Panel>
                </div>
            </div>--%>
        </div>
    </div>
</div>
<script type="text/javascript">
    loadTabOnLoad("<%=hTabNumber.Value%>");
</script>




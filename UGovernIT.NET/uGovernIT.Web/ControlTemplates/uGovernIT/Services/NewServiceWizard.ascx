<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewServiceWizard.ascx.cs" Inherits="uGovernIT.Web.NewServiceWizard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>

<script src="/Scripts/jquery.tablednd.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function ddlTargetType_SelectedIndexChanged(ddl) {
       
        var e = document.getElementById("ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_tabservicePanel_ddlTargetType");
        var strval = e.options[e.selectedIndex].value;

        switch (strval) {
            case "File":
                $(".trLink").hide();
                $(".trFileUpload").show();
                $(".trWiki").hide();
                $(".trHelpCard").hide();
                break;
            case "Link":
                $(".trLink").show();
                $(".trFileUpload").hide();
                $(".trWiki").hide();
                $(".trHelpCard").hide();
                break;
            case "Wiki":
                $(".trLink").hide();
                $(".trFileUpload").hide();
                $(".trWiki").show();
                $(".trHelpCard").hide();
                break;
            case "HelpCard":
                $(".trLink").hide();
                $(".trFileUpload").hide();
                $(".trWiki").hide();
                $(".trHelpCard").show();
                break;
            default:
                break;

        }
    }

</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ratingContainer {
        float: left;
        width: 100%;
        padding: 0 15px;
    }

    .ratingUrl {
        font-size: 13px;
    }

    .txtDateMapping {
        float: left;
        margin-right: 5px;
        height: 21px;
        /* font-weight: bold; */
        /* padding-top: 5px; */
    }

    .heading {
        font-weight: bold;
    }

    .topborder {
        border-top: 1px solid black;
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

    linkbutton {
        FONT-WEIGHT: bold;
        FONT-SIZE: 7pt;
        TEXT-TRANSFORM: capitalize;
        COLOR: white;
        FONT-FAMILY: Verdana;
    }

    .doubleWidth {
        width: 99%;
    }

    .s4-toplinks .s4-tn a.selected {
        padding-left: 10px;
        padding-right: 10px;
    }

    .leftBox {
        width: 1%;
        height: 54px;
        text-align: right;
        background: url(/Content/images/box_left.gif) no-repeat;
    }

    .rightBox {
        width: 1%;
        height: 54px;
        background: url(/Content/images/box_right.gif) no-repeat;
    }

    .middleBox {
        width: 100%;
        height: 44px;
        padding-top: 10px;
        text-align: left;
        float: left;
        margin-top: 1px;
        margin-left: -1px;
        background: url(/Content/images/box_mid.gif) repeat-x;
    }

    .width25 {
        width: 25%;
    }

    .wizardwidth {
        width: 100%;
    }

    .hideblock {
        display: none;
    }

    .peopleeditor-box {
        width: 300px;
    }

    .ms-inputuserfield {
        border: 1px solid gray;
    }

    .formtable {
        width: 100%;
        border-collapse: separate;
        border-spacing: 10px;
    }

        .formtable tr {
            width: 100%;
        }

    .txtcategory {
        float: left;
        padding-left: 3px;
    }

    .canceladdcategory {
        float: left;
        padding-left: 5px;
    }

    .addcategory {
        float: left;
        padding-left: 5px;
    }

    .ms-listviewtable {
        border-collapse: collapse !important;
        /*background: #F8F8F8 !important;*/
        box-shadow: 0px 0px 1px #aaaaaa;
        box-shadow: 0px 0px 1px #aaaaaa;
        border: 1px solid #d0d0d1;
    }



    /*.ms-listviewtable > tbody > tr > td {
            border-bottom: 2px solid #fff !important;
        }*/

    .paddingfirstcell {
        padding-left: 6px !important;
    }

    .widhtfirstcell {
        padding-left: 5px;
    }

    /*.ms-viewheadertr th {
        text-align: left;
        font-weight: bold;
    }*/

    .detailviewitem td {
        text-align: left;
        cursor: pointer;
        height: 25px;
    }

    .ms-listviewtable tr.ticketgrouptr {
        background: white;
        font-weight: bold;
    }

    .ticketgrouppanel {
        float: left;
        width: 99%;
        border-bottom: 2px solid black;
        margin-bottom: 4px;
        padding: 3px 0px;
    }

    /*.tab {
        border: 1px solid black;
        width: 100%;
        float: left;
        padding: 0px 1px;
    }*/

    /*.header-height {
        height: 26px !important;
    }*/

    .section-item td {
        background: white !important;
    }

    .topaction-button {
        float: right;
    }

    /*.topaction-button .activate {
            background-color: red !important;
            /* height:23px;
            font-weight: bold;
        }*/

    /*.activate div {
        /*background: #FF0000 !important;
        background-color: red !important;
        background: red;
        /* height:23px;
        font-weight: bold;
    }*/

    /*.lightText {
        color: #9B9B9B;
    }*/

    .showDrag {
        background-image: url(/Content/images/updown.gif);
        background-repeat: no-repeat;
        background-position: right;
        cursor: move;
        z-index: 1000000;
    }

    .dragHandle {
        width: 18px;
    }

    .HideMe {
        display: none;
    }

    .ms-listviewtable > tbody > tr > th {
        height: 25px;
        padding-left: 3px;
        background: #f2f3f4;
        font-size: 13px;
        font-weight: 500;
    }

    .ms-listviewtable > tbody > tr > td {
        border-bottom: 5px solid #f6f7fb !important;
        background: #FFF;
    }

    /*.ms-listviewtable > tbody > tr > td {
        border: 1px solid #fff !important;
        height: 24px;
    }*/

    .mappedquestion-defaultval {
        padding-right: 10px;
    }

    /*.addtaskheader {
        font-weight: 900;
        background-color: #e0e0e0;
    }*/

    .addsubtaskheader {
        font-weight: 300 !important;
    }

    .section-item-bgcolor {
        background-color: #e6e8f1;
    }

    /*.addboarder {
        border: 1px solid #9da0aa;
    }*/



    .addtaskheaderboarder {
        border-right: 1px solid #9da0aa;
        border-bottom: 1px solid #9da0aa;
    }

    .addtaskboarder {
        border-right: 1px solid #9da0aa;
        border-bottom: 1px solid #9da0aa;
        width: 21px;
    }

    .textpadding {
        padding-left: 10px !important;
    }

    .hover-indent {
        position: relative;
        top: -5px;
    }

    .alternatebanding {
        background-color: rgba(239, 239, 239, 0.78) !important;
    }

    .ms-listviewtable .ms-vb2 {
        text-align: left;
        background: #FFF;
        border-bottom: 5px solid #f6f7fb !important;
        color: #4b4b4b;
        font-size: 12px;
    }

    /*.tblParentService {
        width: 58%;
    }

        .tblParentService td:nth-child(1) {
            width: 35%;
        }*/

    .paddingLR5 {
        padding: 0px 5px;
    }

    /*.ServiceName {
        width: 190px;
    }*/

    .task-quickaction {
        position: relative;
        top: -5px;
    }

    .Add {
        margin-top: 2px;
    }
    /*.taskReminders-checkWrap {
        display: inline-block;
    }

    .taskReminders-checkWrap input {
        padding: 0;
        height: initial;
        width:  initial;
        margin-bottom: 0;
        cursor: pointer;
    }

    .taskReminders-checkWrap label {
        position: relative;
        cursor: pointer;
        font: 13px 'Poppins', sans-serif;
        color: #4A90E2;
    }

    .taskReminders-checkWrap label::before {
        content: '';
        -webkit-appearance: none;
        background-color: transparent;
        border: 2px solid #4A90E2;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05), inset 0px -15px 10px -12px rgba(0, 0, 0, 0.05);
        padding: 7px;
        display: inline-block;
        position: relative;
        vertical-align: middle;
        cursor: pointer;
        margin-right: 5px;
    }

    .taskReminders-checkWrap input:checked + label::after {
        content: '';
        display: block;
        position: absolute;
        top: 4px;
        left: 7px;
        width: 4px;
        height: 10px;
        border: solid #4A90E2;
        border-width: 0 2px 2px 0;
        transform: rotate(45deg);
    }
    .taskReminders-checkWrap input[type="checkbox"] {
        display: none;
    }
   .taskReminders-checkWrap input[type="checkbox"] {
        display: none;
    }
    .taskReminders-checkWrap input:nth-child(2n)[type="text"],input[type="text"] {
        width: 50px !important;
    }
    .taskReminders-checkWrap select, .taskReminders-checkWrap-textbox input[type="text"] {
        width: 70px !important;
    }*/

    /*.escalation {
        padding-top:5px;
    }*/

    /*.disableSLA, .enableescalationbySLA, .resolutionSLA, .taskReminders {
        padding-top:10px;
    }*/
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    function RelatedAssetsOnLoad(s, e) {

        if (typeof (s) != 'undefined' && !s.InCallback()) {
            s.PerformCallback();
        }
    }
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);

    function InitializeRequest(sender, args) {

        var prm = Sys.WebForms.PageRequestManager.getInstance();
    }
    var notifyId = null;
    var statusId = null;
    function BeginRequestHandler(sender, args) {
        RemoveAllStatus();
        notifyId = AddNotification("Processing ..");
    }

    function EndRequest(sender, args) {

        var s = sender;
        var a = args;
        var msg = null;
        if (a._error != null) {
            switch (args._error.name) {
                case "Sys.WebForms.PageRequestManagerServerErrorException":
                    msg = "PageRequestManagerServerErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerParserErrorException":
                    msg = "PageRequestManagerParserErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerTimeoutException":
                    msg = "PageRequestManagerTimeoutException";
                    break;
            }
            args._error.message = "My Custom Error Message " + msg;
            args.set_errorHandled(true);

        }
        AddStatus("Success:", "Data has been updated", true);
        //RemoveNotification(notifyId);

    }



    function addNewCategory() {
        var title = "New Category";
        var params = "categoryID=0";
        UgitOpenPopupDialog('<%= newServiceCategoryUrl %>', params, title, '50', '60', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function cancelServicePopup(obj) {

        var sourceURL = "<%= Request["source"] %>";
        window.parent.commitPopup(sourceURL);
    }

    function activeTabChanged(s, e) {
        var preTab = $("#<%= hfCurrentTab.ClientID%>").val();
        var selectedTab = e.tab.index.toString();
        var isValid = true;

        $("#<%=hfCurrentTab.ClientID%>").val(selectedTab);
        $("#<%=hfPreTab.ClientID%>").val(preTab);

        if (ASPxClientEdit.ValidateGroup('serviceIntitial')) {
            LoadingPanel.Show();
            if (preTab == "0") {

                btSaveNewServiceInitials.DoClick();

            }
            else if (preTab == "1") {
                LoadingPanel.Hide();
                $(".btnHdnButton").trigger("click");
            }
            else if (preTab == "2") {
                btSaveSectionQuestionOrder.DoClick();
            }
            else if (preTab == "3") {

                $(".btnHdnButton").trigger("click");
                //$(".btnHdnButton").trigger("click");
            }
            else if (preTab == "4") {

                btSaveQuestionMapping1.DoClick();
                ///tabMenu.SetActiveTabIndex(selectedTab);
            }
        }

        if (typeof Page_IsValid != "undefined" && !Page_IsValid) {
            LoadingPanel.Hide();
            $("#<%=hfCurrentTab.ClientID%>").val(preTab);
            $("#<%=hfPreTab.ClientID%>").val(preTab);
        }
    }



    function nextTab(tabNumber, stopSubmit) {
        if ('<%= Category %>' == '<%= uGovernIT.Utility.Constants.ModuleService%>') {
            if (tabNumber == 0)
                tabNumber = tabNumber + 2;
            else if (tabNumber == 2 || tabNumber == 3)
                tabNumber = tabNumber + 1;
        }
        if ('<%= Category %>' == '<%= uGovernIT.Utility.Constants.ModuleAgent%>') {
            tabNumber = tabNumber + 2;
        }
        if ('<%= Category %>' == '<%= uGovernIT.Utility.Constants.ModuleFeedback%>') {
            tabNumber = tabNumber + 1;
        }
        tabMenu.SetActiveTabIndex(tabNumber);
    }

    function previousTab(tabNumber, stopSubmit) {
        if ('<%= Category %>' == '<%= uGovernIT.Utility.Constants.ModuleAgent%>') {
            tabNumber = tabNumber - 2;
        }

        if ('<%= Category %>' == '<%= uGovernIT.Utility.Constants.ModuleFeedback%>') {
            tabNumber = tabNumber - 1;
        }

        if ('<%= Category %>' == '<%= uGovernIT.Utility.Constants.ModuleService%>') {
            if (tabNumber == 4 || tabNumber == 3)
                tabNumber = tabNumber - 1;
            else if (tabNumber == 2)
                tabNumber = tabNumber - 2;
        }

        tabMenu.SetActiveTabIndex(tabNumber);
    }

    function HideUploadLabel() {
        $("#<%=lblUploadedFile.ClientID %>").hide();
          $("#<%=fileUploadControl.ClientID %>").show();
          $("#<%=ImgEditFile.ClientID%>").hide();
        return false;
    }

    function editQuestion(isNew, questionID) {
        var projectID = $("#<%= hfServiceID.ClientID%>").val();
        var title = "Edit Question";
        if (isNew) {
            title = "New Question";
        }
        var params = "svcconfigid=" + projectID + "&questionid=" + questionID;
        window.parent.UgitOpenPopupDialog('<%= editServiceQuestionUrl %>', params, title, '50', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function validateAndSaveBasicDetail(s, e) {

        var serviceId = $("#<%=hfServiceID.ClientID %>").val();
        $("#<%=hfSaveAndClose.ClientID %>").val("");

        if (serviceId == "" || serviceId == "0") {

            e.ProcessOnServer = true;
            btSaveNewServiceInitials.DoClick();
            //if (Page_IsValid && ASPxClientEdit.ValidateGroup('serviceIntitial')) {
            if (typeof Page_IsValid != "undefined" && Page_IsValid && ASPxClientEdit.ValidateGroup('serviceIntitial')) {
                {
                    LoadingPanel.Show();
                    return true;
                }
            }
            else {
                return false;
            }

        }
        else {
            //LoadingPanel.Show();
            e.ProcessOnServer = true;
            btSaveServiceInitials.DoClick();
            if (ASPxClientEdit.ValidateGroup('serviceIntitial')) {
                return true;
            }
            else {
                LoadingPanel.Hide();
                return false;
            }

        }


    }
    function onbtnTaskClick() {
        window.parent
    }





    function validateAndSaveQuestionMapping() {
        // LoadingPanel.Show();
        btSaveQuestionMapping.DoClick();
    }


    function reOrderSection(obj, currentIndex, total) {
        var sectionArray = new Array();
        var sectionOrderBoxs = $(".section-order");
        $.each(sectionOrderBoxs, function (i, item) {
            sectionArray.push($(item).val());
        });

        var currentObj = sectionOrderBoxs.get(currentIndex);
        var newValue = Number($(currentObj).val());

        var oldValue = 0;
        for (var i = 1; i <= total; i++) {
            if ($.inArray(i.toString(), sectionArray) == -1) {
                oldValue = i;
                break;
            }
        }

        if (newValue < oldValue) {
            sectionOrderBoxs.each(function (j, item) {
                var val = Number($(item).val());
                if (val >= newValue && val < oldValue && val < total && item != currentObj) {
                    $(item).val((val + 1).toString());
                }
            });
        }
        else {
            sectionOrderBoxs.each(function (j, item) {
                var val = Number($(item).val());
                if (val <= newValue && val > oldValue && item != currentObj) {
                    $(item).val((val - 1).toString());
                }
            });
        }
    }

    function reOrderQuestion(obj, currentIndex, total, sectionID) {
        var sectionArray = new Array();
        var sectionOrderBoxs = $(".question-order-" + sectionID);
        $.each(sectionOrderBoxs, function (i, item) {
            sectionArray.push($(item).val());
        });

        var currentObj = sectionOrderBoxs.get(currentIndex);
        var newValue = Number($(currentObj).val());

        var oldValue = 0;
        for (var i = 1; i <= total; i++) {
            if ($.inArray(i.toString(), sectionArray) == -1) {
                oldValue = i;
                break;
            }
        }

        if (newValue < oldValue) {
            sectionOrderBoxs.each(function (j, item) {
                var val = Number($(item).val());
                if (val >= newValue && val < oldValue && val < total && item != currentObj) {
                    $(item).val((val + 1).toString());
                }
            });
        }
        else {
            sectionOrderBoxs.each(function (j, item) {
                var val = Number($(item).val());
                if (val <= newValue && val > oldValue && item != currentObj) {
                    $(item).val((val - 1).toString());
                }
            });
        }
    }

    function editSection(isNew, sectionID) {

        var projectID = $("#<%= hfServiceID.ClientID%>").val();
        var title = "Edit Section";
        if (isNew) {
            title = "New Section";
        }
        var params = "svcconfigid=" + projectID + "&sectionID=" + sectionID;
        UgitOpenPopupDialog('<%= editServiceSectionUrl %>', params, title, '40', '80', 0, escape("<%= Request.Url.AbsoluteUri %>"));
    }

    function saveSectionQuestOrder() {
        LoadingPanel.Show();
        btSaveSectionQuestionOrder.DoClick();
    }


    function editQuestionBranch() {
        var projectID = $("#<%= hfServiceID.ClientID%>").val();
        var moduleName = "SVCConfig";
        var title = "Skip Section";
        var params = "svcConfigID=" + projectID;
        window.parent.UgitOpenPopupDialog('<%= editservicequestionbranchUrl %>', params, title, '67.5', '80', 0, escape("<%= Request.Url.AbsoluteUri %>"));
    }

    function showAttachmentCondition(obj) {
        var projectID = $("#<%= hfServiceID.ClientID%>").val();
        var moduleName = "SVCConfig";
        var title = "Condition to Make Attachment Mandatory";
        var params = "svcConfigID=" + projectID + "&conditionType=attachment";
        UgitOpenPopupDialog('<%= editservicequestionbranchUrl %>', params, title, '600px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function editTaskCondition() {

        var projectID = $("#<%= hfServiceID.ClientID%>").val();
        var moduleName = "SVCConfig";
        var title = "Skip Tasks/Tickets";
        var params = "svcConfigID=" + projectID + "&conditionType=skiptask";
        window.parent.UgitOpenPopupDialog('<%= editservicetaskBranchUrl %>', params, title, '60', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


    function closeAndSaveTab1() {
        if (ASPxClientEdit.ValidateGroup('serviceIntitial')) {
            LoadingPanel.Show();
            $("#<%=hfSaveAndClose.ClientID %>").val("true");
            btSaveNewServiceInitials.DoClick();
        }
    }
    // Added to prevent closing of popup & Refresh page.
    function SaveOrder() {
        LoadingPanel.Show();
        $("#<%=hfSaveAndClose.ClientID %>").val("false");
        btSaveSectionQuestionOrder1.DoClick();
    }

    function closeAndSaveTab2() {

        LoadingPanel.Show();
        $("#<%=hfSaveAndClose.ClientID %>").val("true");
        btSaveSectionQuestionOrder1.DoClick();
    }

    function closeAndSaveTab4() {

        LoadingPanel.Show();
        $("#<%=hfSaveAndClose.ClientID %>").val("true");
        btSaveQuestionMapping.DoClick();
    }


    function onSelectedDefaultPicker(obj) {
        var selectedPicker = $(obj).val();
        var isDefaultSelected = selectedPicker == "[$Initiator$]" || selectedPicker == "[$InitiatorManager$]" || selectedPicker == "[$Today$]" ||
            selectedPicker == "[$InitiatorLocation$]" || selectedPicker == "[$InitiatorDepartment$]" || selectedPicker == "[$InitiatorDepartmentManager$]" ||
            selectedPicker == "[$InitiatorDivisionManager$]";
        var dropdownCtr = $(obj).parent().parent().find(".mappedquestion-defaultval select");
        var dropdownExist = dropdownCtr.length > 0 ? true : false;
        if (isDefaultSelected && dropdownExist) {
            dropdownCtr.css("display", "none");
        }
        else {
            dropdownCtr.css("display", "block");
        }
    }



    function deleteServcie() {
        if (confirm("Are you sure you want to archive this service?")) {
            $("#<%= btDeleteService.ClientID %>").click();
            return true;
        }
        return false;
    }

    function unArchiveService() {
        if (confirm("Are you sure you want to unarchive this service?")) {
            $("#<%= btDeleteService.ClientID %>").click();
            return true;
        }
        return false;
    }

    function fieldMapToQuestion(obj) {
        var defaultValCtr = $(obj).parent().parent().find(".mapfield-defaultvalcontrol");
        var udefalutValCtr = $(obj).parent().parent().find(".mapfield-usercollection");
        if ($(obj).hasClass("donthide")) {
            return false;
        }

        if ($(obj).val() == "") {
            defaultValCtr.css("display", "block");
            udefalutValCtr.css("display", "none");
        }
        else {

            defaultValCtr.css("display", "none");
            udefalutValCtr.css("display", "block");
        }
    }
    function showWiki() {
        $("#<%=txtWiki.ClientID %>").show();
        <%--$("#<%=fileupload.ClientID %>").hide();--%>

    }
    function showUploadControl1() {

        $("#<%=txtWiki.ClientID %>").hide();
        <%--$("#<%=fileupload.ClientID %>").show()--%>
    }

    function ddlAttachmentRequired_Change(obj) {
        var linkObj = $(".lnkattachmentconditionspan");
        if ($(obj).val() == "Conditional") {
            linkObj.show();
        }
        else {
            linkObj.hide();
        }
    }

    function deleteQuestionConfirm(obj) {
        var question = $.trim($(obj).parent().parent().find(".questiontitle").text());
        if (confirm("Do you want to delete question \"" + question + "\" and related question mapping?")) {
            return true;
        }
        return false;
    }
    function deleteSectionConfirm(obj) {
        var section = $.trim($(obj).parent().text());
        if (confirm("This will also delete the questions in the section, Do you want to delete section \"" + section + "\"?")) {
            return true;
        }
        return false;
    }


    function showTaskActions(trObj, taskID) {
        //show description icon
        $("#actionButtons" + taskID).css("display", "block");
    }

    function hideTaskActions(trObj, taskID) {
        //show description icon
        $("#actionButtons" + taskID).css("display", "none");
    }



    function loadingScreen() {
        SP.UI.ModalDialog.showWaitScreenWithNoClose("Processing...", "Please wait while data is being loaded...", 60, 280);
        ExecuteOrDelayUntilScriptLoaded(CloseWaitScreen, "sp.ui.dialog.js");
        return true;
    }

    function CloseWaitScreen() {
        window.parent.waitDialog.close(SP.UI.DialogResult.OK);
    }

    function setCookies(taskID) {

        set_cookie("projectTask", taskID);
        //loadingScreen();
        return true;
    }

    function defaultTaskUI() {
        //collapseAllTask(true);


        var taskID = get_cookie("projectTask");
        var task = $("#ServiceTaskRowLevel_" + taskID);
        if (task.length > 0 && task.attr("ParentTaskID") != "0") {
            task.css("display", "");
            collapseParents(task);
        }
        // }

        delete_cookie("taskProjectID");
        delete_cookie("projectTask");
    }

    function collapseAllTask(doDefault) {
        var trs = $(".ro-table .taskitem[ParentTaskID='0']")

        var minLength = 2;
        if (!doDefault) {
            minLength = 0;
        }
        if (trs.length > minLength) {
            $.each(trs, function (i, item) {
                var maxMinIcon = $(item).find(".task-title img[colexpand]");
                var parentID = $(item).attr("ParentTaskID");
                var taskID = $(item).attr("task");
                var level = $(item).attr("level");
                if (maxMinIcon.length > 0) {
                    CloseChildren(level, taskID, maxMinIcon.get(0))
                }
            });
        }
    }


    function CloseChildren(level, id, imageObj) {

        imageObj.setAttribute("src", "/Content/images/maximise.gif");
        imageObj.setAttribute("onclick", "OpenChildren('" + level + "', '" + id + "', this)");

        var currentTask = $(".ms-listviewtable .detailviewitem[task='" + id + "']");
        currentTask.attr("mode", "collapse");

        var childTasks = $(".ms-listviewtable .detailviewitem[ParentTaskID='" + id + "']");
        for (var i = 0; i < childTasks.length; i++) {
            var task = childTasks.get(i);
            $(task).css("display", "none");

            if ($(task).find(".task-title img[colexpand]").length > 0) {
                CloseChildren($(task).attr("level"), $(task).attr("task"), $(task).find(".task-title img[colexpand]").get(0))
            }
        }
    }

    function OpenChildren(level, id, imageObj) {
        imageObj.setAttribute("src", "/Content/images/minimise.gif");
        imageObj.setAttribute("onclick", "CloseChildren('" + level + "', '" + id + "', this)");
        var childTasks = $(".ro-table .taskitem[ParentTaskID='" + id + "']");
        for (var i = 0; i < childTasks.length; i++) {
            var task = childTasks.get(i);
            $(task).css("display", "");
            if ($(task).find(".task-title img[colexpand]").length > 0) {
                OpenChildren($(task).attr("level"), $(task).attr("task"), $(task).find(".task-title img[colexpand]").get(0))
            }
        }


    }

    function expandAllTask(doDefault) {
        var trs = $(".ro-table .taskitem[ParentTaskID='0']")
        var minLength = 2;
        if (!doDefault) {
            minLength = 0;
        }

        if (trs.length > minLength) {
            $.each(trs, function (i, item) {
                var maxMinIcon = $(item).find(".task-title img[colexpand]");
                var parentID = $(item).attr("ParentTaskID");
                var taskID = $(item).attr("task");
                var level = $(item).attr("level");
                if (maxMinIcon.length > 0) {
                    OpenChildren(level, taskID, maxMinIcon.get(0))
                }
            });
        }
    }



    function BtCopyServiceLink_Click() {
        copyLinkPopup.Show();
        try {
            $(".serviceLinkBox textarea").select();
            document.execCommand('copy');
        } catch (ex) { }
        return false;
    }

    $(function () {

        try {
            defaultTaskUI();
            ddlTargetType_SelectedIndexChanged('');

            if ('<%=Category%>' == '<%=uGovernIT.Utility.Constants.ModuleAgent%>') {
                $(".ServiceName").html('Agent Name<b style="color: Red">*</b>');
                $(".ServiceDescription").html('Agent Description');

            }
        }
        catch (ex) {
        }
    });

    function requestTypeSelectionChanged(s, e) {
    }

    function UserValueChanged(s, e) {
    }

    function rowClick(s, e) {
    }

    function editRatingQuestion(isNew, questionID) {
        var projectID = $("#<%= hfServiceID.ClientID%>").val();
        var title = "Edit Rating";
        if (isNew) {
            title = "New Rating";
        }
        var params = "svcconfigid=" + projectID + "&questionid=" + questionID;
        UgitOpenPopupDialog('<%= editServiceRatingQuestionUrl %>', params, title, '50', '75', 0, escape("<%= Request.Url.AbsoluteUri %>"));
    }

    function OnEndCallback(s, e) {
        LoadingPanel.Hide();
    }
    function change_chkTaskReminders() {
        if ($(".changeParentService :checked").length > 0 && $(".chkTaskReminders :checked").length > 0) {
            $("#<%=reminder1.ClientID %>").show();
            $("#<%=reminder2.ClientID %>").show();
        }
        else {
            $("#<%=reminder1.ClientID %>").hide();
            $("#<%=reminder2.ClientID %>").hide();
        }
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var prevId;
    function OnLoadFunction() {

        $("#tasks").tableDnD({
            onDragClass: "myDragClass",
            onDrop: function (table, row) {
                var orderStr = "";
                var changeStr = "";
                prevId = "0";
                var taskID = "0";

                var rows = $(table).find(".taskitem");
                for (var i = 0; i < rows.length; i++) {
                    orderStr += $(rows[i]).attr("task") + ";#";
                }

                taskID = $(row).attr("task");
                var visibleTaskRows = $(table).find(".taskitem:visible");
                var droppedRowIndex = visibleTaskRows.index(row);

                if (droppedRowIndex > 0) {
                    var dropAfterRow = visibleTaskRows[droppedRowIndex - 1];
                    if ($(dropAfterRow).find('#grpImage').length > 0 && $(visibleTaskRows[droppedRowIndex + 1]).find('#grpImage').length == 0) {
                        dropAfterRow = visibleTaskRows[droppedRowIndex + 1];
                    }
                    changeStr = $(dropAfterRow).attr("task") + ";#" + taskID;
                }
                else {
                    changeStr = 0 + ";#" + taskID;
                }

                set_cookie("projectTask", taskID);

                $("#<%=changeOrder.ClientID %>").val(changeStr);
                $("#<%=order.ClientID %>").val(orderStr);
                $(".reorderButton").get(0).click();
            },

            onDragStart: function (table, row) {
                prevId = row.id;
            }, dragHandle: "dragHandle"
        });

        $("#tasks tr:eq(0)").removeAttr('style');
        $($("#tasks tr").get($("#tasks tr").length - 1)).removeAttr('style');
        $("#tasks .taskitem").hover(function () {
            $(this.cells[parseInt("<%=reorderActionColumn %>")]).addClass('showDrag');
        }, function () {
            $(this.cells[parseInt("<%=reorderActionColumn%>")]).removeClass('showDrag');
        });
    }

    $(function () {
        OnLoadFunction();
        DisableTicketToBeChild();
        change_ParentService();
        var IschkTaskReminders = '<%= chkTaskReminders.Checked%>';
        if (IschkTaskReminders.toLowerCase() == "true") {
            $("#<%=reminder1.ClientID %>").show();
            $("#<%=reminder2.ClientID %>").show();
        }
        else {
            $("#<%=reminder1.ClientID %>").hide();
            $("#<%=reminder2.ClientID %>").hide();
        }
    });

    function Callback_EndCallback(s, e) {
        // OnLoadFunction();
    }
    function DisableTicketToBeChild() {

        for (var i = 0; i < $("#tasks .taskitem").length; i++) {
            if (i > 0 && ($($("#tasks .taskitem").get(i - 1)).attr('type') != "")) {
                $($("#tasks .taskitem").get(i)).find("[id$='lnkIncIndent']").css('display', 'none');
            }
        }
    }



    function imgQuestionMapArrow_Click(obj) {
        var trObj = $($(obj).parents("tr").get(0));
        var questionSelectObj = trObj.find(".ddlquestionmapped");
        var qTextObj = trObj.find(".mapfield-defaultvalcontrol input:text")
        if (qTextObj.length == 0)
            qTextObj = trObj.find(".mapfield-defaultvalcontrol textarea")

        try {
            var sOption = questionSelectObj.find("option:eq(" + questionSelectObj.get(0).selectedIndex + ")");
            var sValue = $.trim(sOption.text());
            var token = "";
            if (sValue.indexOf("]") != -1) {
                token = $.trim(sValue.split("]")[0] + "]");
            }
            if (token == "")
                return;

            var startPos = qTextObj.get(0).selectionStart;
            var endPos = qTextObj.get(0).selectionEnd;

            var textVal = qTextObj.val();
            var startStr = textVal.substring(0, startPos);
            var endStr = textVal.substring(endPos, textVal.length);
            var finalText = startStr + token + endStr;
            qTextObj.val(finalText);
        } catch (ex) {
        }
    }
    function change_ParentService() {

        if ($(".changeParentService :checked").length > 0) {
            $('.ApprovalRequired').show();
            $('.tasksInbackground').show();
            $(".disableSLA").show();
            //$(".resolutionSLA").show();
            //$('.tblParentService_td').css('border', "1px solid black");
            $(".spanAttachInChild").show();
            $(".taskReminders-checkWrap").show();
            $(".taskReminders-container").show();
            $("#parentSerWrap").show();
            $('#trParentService').addClass('parent-service-container');
        }
        else {
            $("#<%=cbOwnerApprovalRequired.ClientID%>").prop("checked", false);
            $("#<%=cbOwnerTasksInBackground.ClientID%>").prop("checked", false);
            $("#<%=chkbxAttachInChild.ClientID%>").prop("checked", false);
            $(".spanAttachInChild").hide();
            $('.ApprovalRequired').hide();
            $('.tasksInbackground').hide();
            $(".disableSLA").hide();
            $(".taskReminders-checkWrap").hide();
            $(".taskReminders-container").hide();
            $("#parentSerWrap").hide();
            $('#trParentService').removeClass('parent-service-container');
            //$(".resolutionSLA").hide();
            //$('.tblParentService_td').css('border', "none");

        }
        change_chkDisableSLA();
        change_chkEscalation();
        change_chkTaskReminders();
    }
    function change_chkDisableSLA() {
        if ($(".changeParentService :checked").length > 0 && $(".chkDisableSLA :checked").length > 0) {
            $(".resolutionSLA").show();
            $(".enableescalationbySLA").show();
        }
        else {
            $(".resolutionSLA").hide();
            $(".enableescalationbySLA").hide();
        }
    }

    function change_chkEscalation() {
        if ($(".changeParentService :checked").length > 0 && $(".chkEnableEscalation :checked").length > 0) {
            $(".escalation").show();
        }
        else {
            $(".escalation").hide();
        }
    }

    function OnItemValidation(s, e) {
        var item = e.value;
        if (item == null || item == "")
            e.isValid = false;
    }

    function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
    }

    function OpenSendSurvey() {
        var Param = "SelectedModule=" + '<%=selectedModule%>' + "&surveyName=" + $('#<%=txtServiceName.ClientID%>').val();
        UgitOpenPopupDialog('<%= sendSurveyURL %>', Param, "Send Survey", '80', '730px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function rdbSurveyType_SelectedIndexChanged(s, e) {
        tabservicePanel.PerformCallback();
    }

    function BindModuleStageGridLookup(s, e) {

        pnlModuleStages.PerformCallback(s.lastSuccessText);  //use lastSuccessText property only in valuechanged event
    }



    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });
</script>


<div style="display: none;">
    <dx:ASPxDateEdit ID="tempDate" runat="server" />
    <ugit:UserValueBox principalsource="UserInfoList" ID="tempPicker" runat="server" augmententitiesfromuserinfo="true" />
</div>
<asp:HiddenField ID="hfCurrentTab" runat="server" />
<asp:HiddenField ID="hfPreTab" runat="server" />
<asp:HiddenField ID="hfSaveAndClose" runat="server" />
<asp:HiddenField ID="order" runat="server" Value="" />
<asp:HiddenField ID="changeOrder" runat="server" />
<asp:HiddenField ID="hfMapPreviousTaskID" runat="server" />
<asp:Button ID="btnReOrder" class="reorderButton HideMe" runat="server" CommandName="ReOrder" OnClick="ReOrder_Click" />
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
</dx:ASPxLoadingPanel>
<asp:Button ID="btnHdnButton" runat="server" class="btnHdnButton HideMe" />

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row" id="helpTextRow" runat="server">
        <div class="paddingleft8">
            <asp:Panel ID="helpTextContainer" runat="server">
            </asp:Panel>
        </div>
    </div>

    <div class="row serviceTab-container formLayout-tabControlPannel">
        <div class="col-md-7 col-sm-7 col-xs-12 noPadding">
            <dx:ASPxTabControl ID="tabMenu" ClientInstanceName="tabMenu" runat="server" Width="100%" CssClass="formLayout-tabControl">
                <Tabs>
                    <dx:Tab Text="Service" Name="service" />
                    <dx:Tab Text="Ratings" Name="ratings" />
                    <dx:Tab Text="Questions" Name="questions" />
                    <dx:Tab Text="Tickets/Tasks" Name="tasks" />
                    <dx:Tab Text="Mapping" Name="mapping" />
                </Tabs>
                <ClientSideEvents ActiveTabChanged="function(s,e){activeTabChanged(s,e);}" />
                <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
            </dx:ASPxTabControl>
        </div>
        <div class="col-md-5 col-sm-5 col-xs-12 rightSide-btnWrap">
            <dx:ASPxButton ID="btSendSurvey" runat="server" Visible="true" AutoPostBack="false" Text="Send Survey" CssClass="primary-blueBtn">
                <ClientSideEvents Click="OpenSendSurvey" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btRunService" runat="server" AutoPostBack="false" Text="Run Service" CssClass="primary-blueBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btActivateService" CssClass="primary-blueBtn" runat="server" Text="Activate" OnClick="BtActivateService_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btDeactivateService" CssClass="primary-blueBtn" runat="server" Text="Deactivate" OnClick="BtDeactivateService_Click"></dx:ASPxButton>
        </div>
    </div>

    <div class="row" style="padding: 0 5px;">
        <asp:HiddenField ID="hfServiceID" runat="server" />
        <asp:Panel runat="server" ID="tabServiceDetails" CssClass="tab tab1 services-field-wrap" Visible="false">
            <dx:ASPxCallbackPanel ID="tabservicePanel" ClientInstanceName="tabservicePanel" runat="server" OnCallback="tabservicePanel_Callback">
                <PanelCollection>
                    <dx:PanelContent>
                        <div class="formtable accomp-popup">
                            <div class="col-md-12 col-sm-12 col-xs-12" id="trServiceName" runat="server">
                                <div class="ServiceName ms-formlabel">
                                    <dx:ASPxLabel CssClass="budget_fieldLabel ms-standardheader" ID="lblServiceName" runat="server" Text="Service Name"></dx:ASPxLabel>
                                    <b style="color: Red">*</b>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxTextBox ID="txtServiceName" runat="server" Width="100%" CssClass="aspxTextBox-inputBox">
                                        <ValidationSettings ErrorDisplayMode="Text" ValidationGroup="serviceIntitial">
                                            <RequiredField IsRequired="true" ErrorText="Please enter service name" />
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12" id="trServiceDescription" runat="server">
                                <div class="ServiceDescription ms-formlabel">
                                    <dx:ASPxLabel CssClass="budget_fieldLabel ms-standardheader" ID="lblSeviceDescription" runat="server" Text="Service Description"></dx:ASPxLabel>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox runat="server" ID="txtServiceDescription" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12 " id="trCategory" runat="server" visible="false">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Category</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:UpdatePanel runat="server" ID="addCategoryPanel" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div style="width: 90%; float: left; display: inline-block;" class="">
                                                <ugit:LookUpValueBox ID="categoryDD" runat="server" FieldName="CategoryLookup" AllowNull="true" CssClass="lookupValueBox-dropown"
                                                    FilterExpression="Title is not null" />
                                            </div>

                                            <div style="float: right; display: inline-block; padding: 4px 4px 0px;">
                                                <img alt="Add Category" src="/Content/images/plus-square.png" style="cursor: pointer; width: 16px; margin-top: 4px;" onclick="addNewCategory()" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12 fieldClear" id="trOwner" runat="server" visible="false">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Owner</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <ugit:UserValueBox ID="peServiceOwner" CssClass="peAssignedTo userValueBox-dropDown" runat="server" isMulti="true" />
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12" id="trAuthorizedToRun" runat="server" visible="false">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Authorized To Run</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <ugit:UserValueBox ID="peAuthorizedToRun" runat="server" CssClass="peAssignedTo userValueBox-dropDown" />
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding parent-service-container" id="trParentService" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12 addMore-leftPadding parentServiceRequest">
                                        <div class="chkBoxWrap-forOnchange parentService-chkBox-wrap">
                                            <asp:CheckBox ID="cbCreateParentServiceRequest" runat="server" Text="Create Parent Service Request "
                                                CssClass="changeParentService" onchange="change_ParentService()" />
                                        </div>
                                    </div>

                                    <div class="fleft" id="parentSerWrap">
                                        <div class="row serviceCat-grayBg">
                                            <div class="col-md-4 col-sm-4 col-xs-12 addMore-leftPadding parentServiceRequest" runat="server" id="divIncludeInDefaultData"
                                                visible="false">
                                                <div class="chkBoxWrap-forOnchange">
                                                    <asp:CheckBox ID="chkIncludeInDefaultData" runat="server" Text="Include in default data " CssClass="changeParentService" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-12 addMore-leftPadding ApprovalRequired" style="display: none">
                                                <div class="crm-checkWrap">
                                                    <asp:CheckBox ID="cbOwnerApprovalRequired" Text="Owner Approval Required to Close" runat="server" Checked="false" />
                                                </div>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-12 addMore-leftPadding tasksInbackground" style="display: none">
                                                <div class="crm-checkWrap">
                                                    <asp:CheckBox ID="cbOwnerTasksInBackground" runat="server" Checked="false" Text="Create Tasks/Tickets in Background" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row taskReminders-container">
                                            <div class="col-md-12 col-sm-12 col-xs-12 taskReminders taskReminder-container" id="taskReminders" runat="server">
                                                <div class="taskReminders-checkWrap crm-checkWrap" style="padding-left: 8px;">
                                                    <asp:CheckBox ID="chkTaskReminders" runat="server" CssClass="chkTaskReminders" Checked="false" onchange="change_chkTaskReminders()"
                                                        Text="Enable Task Reminders" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="reminder1" runat="server" style="padding-top: 5px;">
                                                <div class="taskReminders-checkWrap-textbox">
                                                    <div class="crm-checkWrap-secondary reminder-wrap">
                                                        <asp:CheckBox ID="chkReminder1" runat="server" Checked="false" Text="Reminder 1" />
                                                        <span>Due Date</span>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                                        <asp:TextBox ID="txtReminder1Duration" runat="server" CssClass="asptextbox-asp" Text="0" Style="width: 94% !important;" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12" style="padding-left: 5px; padding-right: 28px;">
                                                        <asp:DropDownList ID="ddlReminder1Unit" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                            <asp:ListItem Value="Days">Days</asp:ListItem>
                                                            <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                            <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                                        <asp:DropDownList ID="ddlReminder1Frequency" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                            <asp:ListItem Value="After">After</asp:ListItem>
                                                            <asp:ListItem Value="Before">Before</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="reminder2" runat="server" style="padding-top: 5px;">
                                                <div class="taskReminders-checkWrap taskReminders-checkWrap-textbox">
                                                    <div class="crm-checkWrap-secondary reminder-wrap">
                                                        <asp:CheckBox ID="chkReminder2" runat="server" Checked="false" Text="Reminder 2" />
                                                        <span>Due Date</span>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                                        <asp:TextBox ID="txtReminder2Duration" runat="server" CssClass="asptextbox-asp" Text="0" Style="width: 94% !important;" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12" style="padding-left: 5px; padding-right: 28px;">
                                                        <asp:DropDownList ID="ddlReminder2Unit" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                            <asp:ListItem Value="Days">Days</asp:ListItem>
                                                            <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                            <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12">
                                                        <asp:DropDownList ID="ddlReminder2Frequency" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                            <asp:ListItem Value="After">After</asp:ListItem>
                                                            <asp:ListItem Value="Before">Before</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row serviceCat-grayBg">
                                            <div class="col-md-12 col-sm-12 col-xs-12 disableSLA disableSLA-container" style="display: none;">
                                                <div class="chkBoxWrap-forOnchange" style="padding-left: 1px;">
                                                    <asp:CheckBox ID="chkDisableSLA" runat="server" CssClass="chkDisableSLA" Checked="false" Text="Enable SLA"
                                                        onchange="change_chkDisableSLA()" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding resolutionSLA">
                                                <div class="col-md-4 col-sm-4 col-xs-12 noRightPadding">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader resolutionSLA budget_fieldLabel">Resolution SLA</h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField">
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="txtResolutionSLA" runat="server" Text="0" CssClass="asptextbox-asp" />
                                                        </div>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:DropDownList ID="ddlResolutionSLAType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                                <asp:ListItem Value="Days">Days</asp:ListItem>
                                                                <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                                <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-12 crm-checkWrap-secondary" style="padding-left: 20px; padding-top: 30px;">
                                                    <asp:CheckBox ID="cbStartResolutionSLAFromStart" runat="server" Text="Measure from Assigned" />
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-12 addMore-leftPadding crm-checkWrap-secondary" style="padding-top: 30px;">
                                                    <asp:CheckBox ID="chkUse24x7" runat="server" Text="Use 24x7 Calendar" />
                                                </div>
                                            </div>
                                            <div class="enableescalationbySLA">
                                                <div class="col-md-12 col-sm-12 col-xs-12">
                                                    <div class="taskReminders-checkWrap enable-escalation-container crm-checkWrap">
                                                        <asp:CheckBox ID="chkEnableEscalation" runat="server" CssClass="chkEnableEscalation" Checked="false"
                                                            onchange="change_chkEscalation()" Text="Enable Escalations" />
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12 noRightPadding  escalation" style="display: none;">
                                                        <div class="ms-formlabel">
                                                            <h3 class="ms-standardheader secandary-feildLable budget_fieldLabel" style="padding-left: 15px;">Escalation</h3>
                                                        </div>
                                                        <div class="ms-formbody accomp_inputField">
                                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                                <asp:TextBox ID="txtEscalationAfter" runat="server" CssClass="asptextbox-asp" Text="0" />
                                                            </div>
                                                            <div class="col-md-6 col-sm-6 col-xs-12">
                                                                <asp:DropDownList ID="ddlEscalationAfter" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                                    <asp:ListItem Value="Days">Days</asp:ListItem>
                                                                    <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                                    <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-12 escalation" style="padding-top: 21px;">
                                                        <div class="ms-formbody accomp_inputField">
                                                            <asp:DropDownList ID="ddlEscaltionUnit" runat="server" CssClass="itsmDropDownList aspxDropDownList fleft"
                                                                Style="width: 70%!important;">
                                                                <asp:ListItem Text="After" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Before" Value="1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="ms-formlabel fright">
                                                                <span class="budget_fieldLabel secandary-feildLable" style="color: #4A90E2">SLA Expiration</span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-12 noRightPadding escalation" style="display: none; padding-left: 30px;">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel secandary-feildLable" style="padding-left: 15px;">Escalation Frequency</h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField">
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:TextBox ID="txtEsclationFrequency" runat="server" CssClass="asptextbox-asp" Text="0" />
                                                        </div>
                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                            <asp:DropDownList ID="ddlEsclationFrequency" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                                <asp:ListItem Value="Days">Days</asp:ListItem>
                                                                <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                                <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-4 col-sm-4 col-xs-12 escalation" style="display: none;">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel secandary-feildLable">Escalation To</h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField all-drop-down">
                                                        <ugit:UserValueBox ID="pplEscalationTo" runat="server" CssClass="userValueBox-dropDown" isMulti="true" />
                                                    </div>
                                                </div>

                                                <div class="col-md-4 col-sm-4 col-xs-12 escalation" style="display: none;">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel secandary-feildLable">Also Send To 
                                                        <span style="font-size: 10px; color: #4A90E2; padding-left: 5px;">*Comma-Separated Emails*</span>
                                                        </h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField">
                                                        <asp:TextBox ID="txtEmail" Style="width: 90%;" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-12 accomp-popup" id="trOrder" runat="server" visible="false">
                            <div class="ms-formlabel Order">
                                <h3 class="ms-standardheader budget_fieldLabel">Order</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtOrder" runat="server" Width="40%" CssClass="asptextbox-asp" />
                            </div>
                        </div>
                        <div class="col-md-8 col-sm-8 col-xs-12" id="trAttachmentReqd" runat="server" visible="false">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Attachments</h3>
                            </div>
                            <div class="row feildRow">
                                <div class="col-md-6 col-sm-6 col-xs-12 noLeftPadding">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlAttachmentRequired" runat="server" CssClass="itsmDropDownList  aspxDropDownList"
                                            onchange="ddlAttachmentRequired_Change(this)">
                                            <asp:ListItem Text="Optional" Value="Optional"></asp:ListItem>
                                            <asp:ListItem Text="Always" Value="Always"></asp:ListItem>
                                            <asp:ListItem Text="Conditional" Value="Conditional"></asp:ListItem>
                                            <asp:ListItem Text="Disabled" Value="Disabled"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12">
                                    <div class="spanAttachInChild ms-formbody accomp_inputField">
                                        <div class="crm-checkWrap">
                                            <asp:CheckBox ID="chkbxAttachInChild" runat="server" Text="Attach to All Child Tickets" />
                                        </div>
                                    </div>
                                    <span style="float: left; padding-left: 4px; display: none;" id="lnkAttachmentConditionSpan" class="lnkattachmentconditionspan"
                                        runat="server">
                                        <asp:HyperLink ID="lnkAttachmentCondition" runat="server" NavigateUrl="javascript:showAttachmentCondition(this)">
                                            <img id="Img1" src="/Content/images/editNewIcon.png" style="border: none;" runat="server" />
                                        </asp:HyperLink>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <%--<div class="col-md-4 col-sm-4 col-xs-12" id="trServiceHelp" runat="server" visible="false">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel lightText"> Service Help (Upload document or pick wiki)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtWiki" runat="server" Width="100%" />
                                    <asp:FileUpload ID="fileupload" class="fileupload" Style="display: none" runat="server" /><br />
                                    <a id="aAddItem" runat="server" onclick="showWiki()" style="cursor: pointer;">Add Wiki</a> |
                                    <a onclick="showUploadControl1()" style="cursor: pointer;">Upload Document</a>
                                </div>
                            </div>--%>

                        <div id="trNavigationType" class="col-md-6 col-sm-6 col-xs-12 accomp-popup" runat="server">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Service Help</h3>
                            </div>

                            <div class="ms-formbody accomp_inputField">
                                <asp:DropDownList ID="ddlTargetType" onchange="ddlTargetType_SelectedIndexChanged(this);" CssClass="target_section itsmDropDownList aspxDropDownList"
                                    runat="server">
                                </asp:DropDownList>
                            </div>

                        </div>

                        <div id="trFileUpload" runat="server" class="col-md-3 col-sm-3 col-xs-12 trFileUpload">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">File</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:Label ID="lblUploadedFile" runat="server"></asp:Label>
                                <asp:FileUpload ID="fileUploadControl" CssClass="fileUploader" Width="200px" ToolTip="Browse and upload file" runat="server" Style="display: none;" />
                                <img alt="Edit File" title="Edit File" runat="server" id="ImgEditFile" src="/content/Images/editNewIcon.png" style="cursor: pointer; width: 16px;" onclick="HideUploadLabel();" />
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControl" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div id="trLink" runat="server" class="col-md-6 col-sm-6 col-xs-12 accomp-popup trLink" style="display: none">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Link URL</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtFileLink" CssClass="fileUploaderLink asptextbox-asp" runat="server" />
                                <div>
                                </div>
                            </div>

                        </div>
                        <div id="trWiki" runat="server" class="col-md-6 col-sm-6 col-xs-12 accomp-popup trWiki" style="display: none">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Select Wiki</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtWiki" runat="server" CssClass="asptextbox-asp" Style="width: 95%!important;" />
                                <a id="aAddItem" runat="server" style="cursor: pointer;">
                                    <img alt="Add Wiki" title="Add Wiki" runat="server" id="imgWiki" src="/content/Images/editNewIcon.png" width="16" style="cursor: pointer;" />
                                </a>
                            </div>
                        </div>
                        <div id="trHelpCard" runat="server" class="col-md-6 col-sm-6 col-xs-12 accomp-popup trHelpCard" style="display: none">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Select Help Card</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox ID="txtHelpCard" runat="server" CssClass="asptextbox-asp" Style="width: 95%!important;" />
                                <a id="aAddHelpCard" runat="server" style="cursor: pointer;">
                                    <img alt="Add Help Card" title="Add Help Card" runat="server" width="16" id="img" src="/content/Images/editNewIcon.png" style="cursor: pointer;" />
                                </a>
                            </div>
                        </div>

                        <div class="col-md-12 col-sm-12 col-xs-12" id="trSurveyType" runat="server">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Survey Type</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxRadioButtonList ID="rdbSurveyType" runat="server" AutoPostBack="false"
                                    RepeatDirection="Horizontal" CssClass="dxAspx-radioBtnList" Width="100%">
                                    <Items>
                                        <dx:ListEditItem Text="Generic" Value="Generic" Selected="true" />
                                        <dx:ListEditItem Text="Module" Value="Module" />
                                    </Items>
                                    <ClientSideEvents SelectedIndexChanged="rdbSurveyType_SelectedIndexChanged" />
                                </dx:ASPxRadioButtonList>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12" id="trModule" runat="server" visible="false">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel lightText">Module<b style="color: Red">*</b></h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <div class="col-md-4 col-sm-4 col-xs-4 pl-0">
                                    <ugit:LookUpValueBox ID="ddlModule" runat="server" ClientInstanceName="ddlModule" FieldName="ModuleNameLookup" IsMandatory="true"
                                        ValidationGroup="serviceIntitial" CssClass="lookupValueBox-dropown"/>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12" id="trAgentStages" runat="server" visible="false">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Workflow Stage(s)<b style="color: Red">*</b></h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <div>
                                    <dx:ASPxCallbackPanel ID="pnlModuleStages" ClientInstanceName="pnlModuleStages" runat="server" OnCallback="pnlModuleStages_Callback">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <%--<ugit:LookupValueBoxEdit ID="glModuleStages" FieldName="ModuleStages" runat="server" />--%>
                                                <dx:ASPxGridLookup Theme="DevEx" AutoGenerateColumns="true" CssClass="stagesctionusers" Visible="true" Width="355px" SelectionMode="Multiple"
                                                    ID="glModuleStages" runat="server" MultiTextSeparator="; " ViewStateMode="Disabled" ValidationSettings-ValidationGroup="serviceIntitial">
                                                    <Columns>

                                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" />
                                                        <dx:GridViewDataTextColumn FieldName="Name" Width="300px" Caption="Choose Module Stages">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <ValidationSettings CausesValidation="true" ErrorDisplayMode="Text" Display="Dynamic" ValidationGroup="serviceIntitial">
                                                        <RequiredField IsRequired="true" ErrorText="Please select the workflow stage" />
                                                    </ValidationSettings>
                                                    <GridViewProperties>
                                                        <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                                        <SettingsBehavior AllowSort="false" AllowGroup="true" AllowAutoFilter="true" AutoExpandAllGroups="true" />
                                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                    </GridViewProperties>
                                                    <ClientSideEvents EndCallback="OnEndCallback" />
                                                </dx:ASPxGridLookup>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-12" id="trShowStageTransitionButtons" runat="server" visible="false">
                            <div class="ms-formbody accomp_inputField">
                                <div class="crm-checkWrap">
                                    <asp:CheckBox ID="chkbxShowStageTranBtns" runat="server" Text="Show Stage Transition Button(s)" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-xs-12" id="trShowDefaultvalChkAgent" runat="server" visible="false">
                            <div class="ms-formbody accomp_inputField">
                                <div class="crm-checkWrap">
                                    <asp:CheckBox ID="chkbxShowDefaultvalAgent" runat="server" Text="Load Current Field Values" Checked="false"></asp:CheckBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 col-sm-4 col-xs-12" id="trHideSummary" runat="server" visible="false">
                            <div class="ms-formbody accomp_inputField">
                                <div class="crm-checkWrap">
                                    <asp:CheckBox ID="chkhidesummary" runat="server" Text="Hide Summary" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3 col-sm-4 col-xs-12" id="trHideThankuScreen" runat="server" visible="false">
                            <div class="ms-formbody accomp_inputField">
                                <div class="crm-checkWrap">
                                    <asp:CheckBox ID="chkHideThankYouScreen" runat="server" Text="Hide Thank You Screen" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12 col-sm-12 col-xs-12" id="trIcon" runat="server" visible="false">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Icon</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <ugit:UGITFileUploadManager ID="fileUploadIcon" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                                <%--<asp:TextBox runat="server" ID="txtServiceIcon" Width="500px" /><br />
                                    <asp:LinkButton ID="lnkbtnPickAssets" runat="server" Font-Size="10px" Text="PickFromAsset">Pick From Library</asp:LinkButton>--%>
                            </div>
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-12" id="trCompletionMessage" runat="server" visible="false">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Completion Message</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField" style="padding-top: 5px;">
                                <asp:TextBox ID="txtCompletionMessage" Width="100%" CssClass="asptextbox-asp" Height="70" TextMode="MultiLine" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12 addMore-padding pb-2">
                            <div class="row" style="padding-top: 10px;">
                                <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                    <div style="float: left">
                                        <dx:ASPxButton ID="lnkFullDeleteService" runat="server" Visible="false" Text="Delete" CssClass="btn-danger1" ToolTip="Delete" OnClick="lnkFullDeleteService_Click">
                                            <ClientSideEvents Click="function(s, e) { return confirm('Are you sure you want to delete?'); }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="lbBtDeleteService" CssClass="primary-blueBtn" ClientInstanceName="lbBtDeleteService" AutoPostBack="false" runat="server" ClientVisible="false" Text="Archive">
                                            <ClientSideEvents Click="deleteServcie" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btDeleteService" runat="server" Text="Archive" OnClick="BtDeleteService_Click" ClientVisible="false" CssClass="primary-blueBtn hideblock">
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btCopyServiceLink" runat="server" AutoPostBack="false" Text="Copy Link" CssClass="primary-blueBtn">
                                            <ClientSideEvents Click="BtCopyServiceLink_Click" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                    <div style="float: right">
                                        <dx:ASPxButton ID="btNext1stTab" ClientInstanceName="btNext1stTab" AutoPostBack="false" CssClass="primary-blueBtn" runat="server" Text="Next >>">
                                            <ClientSideEvents Click="function(){ 
                                                nextTab(0, false);
                                            }" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="btSaveLabel" ClientInstanceName="btSaveLabel" AutoPostBack="false" runat="server" Text="Save" CssClass="primary-blueBtn">
                                            <ClientSideEvents Click="validateAndSaveBasicDetail" />
                                        </dx:ASPxButton>
                                        <div style="display: none;">
                                            <dx:ASPxButton ID="btSaveServiceInitials" ClientInstanceName="btSaveServiceInitials" Text="create" runat="server" CssClass="primary-blueBtn"
                                                ValidationGroup="serviceIntitial" OnClick="BtSaveServiceInitials_Click" AutoPostBack="true">
                                                <ClientSideEvents Click="validateAndSaveBasicDetail" />
                                            </dx:ASPxButton>
                                        </div>
                                        <div style="display: none;">
                                            <dx:ASPxButton ID="btSaveNewServiceInitials" CssClass="primary-blueBtn" ClientInstanceName="btSaveNewServiceInitials" AutoPostBack="true"
                                                ValidationGroup="serviceIntitial" runat="server" OnClick="BtSaveServiceInitials_Click">
                                            </dx:ASPxButton>
                                        </div>
                                        <dx:ASPxButton ID="btSaveAndClose1stTab" CssClass="primary-blueBtn" runat="server" ClientInstanceName="btSaveAndClose1stTab" AutoPostBack="false"
                                            Text="Save & Close" ValidationGroup="serviceIntitial">
                                            <ClientSideEvents Click="closeAndSaveTab1" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </div>
                        </div>
    </div>
    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </asp:panel>
</div>
<div class="row">
    <asp:Panel ID="tabQuestions" runat="server" CssClass="tab tab3 services-field-wrap" Style="width: 88.5%; float: left; width: 100%;" Visible="false">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="row">
                <asp:Panel ID="lWorkFlow" runat="server" Style="width: 100%; float: left; margin: 0px 10px 20px;">
                </asp:Panel>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <%--<h3 class="ms-standardheader budget_fieldLabel">Un-Categorized:</h3>--%>
                    <label id="lblunCategorized" runat="server" visible="false" class="ms-standardheader budget_fieldLabel" title="Un-Categorized">Un-Categorized:</label>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <label id="noUnCategorizedDataMsg" runat="server" visible="false" class="unCategorized-queLabel" title="No Un-categorized Questions">No Un-categorized Questions</label>
                </div>
                <div class="">
                    <table id="unCategoriezedQuestion" class="formtable" runat="server" visible="false">
                        <tr>
                            <td>
                                <table class="ms-listviewtable addboarder" cellpadding="0" cellspacing="0" width="99%">
                                    <asp:Repeater ID="rNonSectionQuestions" runat="server" OnItemDataBound="rNonSectionQuestions_ItemDataBound">
                                        <HeaderTemplate>
                                            <tr class="ms-viewheadertr">
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Question</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Question Type</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Token</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Help Text</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height"></th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="ms-vb2 questiontitle">
                                                    <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                        <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                    </a>
                                                </td>
                                                <td class="ms-vb2">
                                                    <%# UGITUtility.AddSpaceBeforeWord(Convert.ToString(Eval("QuestionType")))%>
                                                </td>
                                                <td class="ms-vb2 questiontoken">
                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                </td>
                                                <td class="ms-vb2">
                                                    <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                </td>
                                                <td class="ms-vb2">
                                                    <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" class="fleft" alt="Edit" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton ID="btDeleteQuestion" OnClientClick="return deleteQuestionConfirm(this);" CssClass="service-catlogdel-icon fleft" runat="server" ImageUrl="/Content/images/grayDelete.png"
                                                        CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ms-alternating">
                                                <td class="ms-vb2 questiontitle">
                                                    <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                        <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                    </a>
                                                </td>
                                                <td class="ms-vb2">
                                                    <%# UGITUtility.AddSpaceBeforeWord(Convert.ToString(Eval("QuestionType")))%>
                                                </td>
                                                <td class="ms-vb2 questiontoken"><%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %></td>

                                                <td class="ms-vb2">
                                                    <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                </td>
                                                <td class="ms-vb2">
                                                    <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" class="fleft" alt="Edit" title="Edit"
                                                        onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" runat="server"
                                                        class="service-catlogdel-icon fleft" ImageUrl="/Content/images/grayDelete.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Sections:</h3>
                </div>
                <div class="">
                    <table class="formtable">
                        <tr>
                            <td>
                                <table class="serviceAgent-queTabSecTable" style="border-collapse: unset" cellpadding="0" cellspacing="0" width="100%">
                                    <asp:Repeater ID="rSections" runat="server" OnItemDataBound="RSections_ItemDataBound">
                                        <HeaderTemplate>
                                            <tr class="ms-viewheadertr queTabSecTable-headerTr">
                                                <th class="ms-vh2-nofilter ms-vh2-gridview queTabSecTable-headerTd" width="80">Order</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview queTabSecTable-headerTd">Section</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview queTabSecTable-headerTd"></th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="section-item queSection-tableItem">
                                                <td class="ms-vb2">
                                                    <asp:DropDownList ID="ddlSectionOrderBy" runat="server" Width="60%">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hfServiceSectionID" runat="server" />
                                                </td>
                                                <td class="ms-vb2 qsectionname">
                                                    <span class="queSection-tableItemLabel"><%# Eval("SectionName") %></span>
                                                    <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px" alt="Edit" class="fleft" title="Edit" onclick='editSection(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton ID="btDeleteSection" OnClientClick="return deleteSectionConfirm(this);" CssClass="fleft service-catlogdel-icon" runat="server"
                                                        ImageUrl="/Content/images/grayDelete.png" CommandArgument='<%#Eval("ID") %>' OnClick="btDeleteSection_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <table class="ms-listviewtable queTabSecTable-listviewtable" cellpadding="0" cellspacing="0" width="100%">
                                                        <asp:Repeater ID="rSectionQuestions" runat="server" OnItemDataBound="RSectionQuestions_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <tr class="queTabSecTable-headerTr">
                                                                    <th class="queTabSecTable-headerTd" width="80">Order</th>
                                                                    <th class="queTabSecTable-headerTd" align="left">Question</th>
                                                                    <th class="queTabSecTable-headerTd" align="left">Question Type</th>
                                                                    <th class="queTabSecTable-headerTd" align="left">Token</th>
                                                                    <th class="queTabSecTable-headerTd" align="left">Help Text</th>
                                                                    <th class="queTabSecTable-headerTd" align="left" width="70"></th>
                                                                </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="ms-alternating queSection-tableItem">
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server" Width="60%" CssClass=" itsmDropDownList aspxDropDownList">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>

                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2"><%# Eval("QuestionType")%></td>
                                                                    <td class="ms-vb2 questiontoken">
                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                                    </td>

                                                                    <td class="ms-vb2"><%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%></td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" alt="Edit" class="fleft" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="fleft" runat="server"
                                                                            ImageUrl="/Content/images/grayDelete.png" Width="16px" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr class="queSection-tableItem">
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server" Width="60%"
                                                                            CssClass="itsmDropDownList aspxDropDownList">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>

                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <%# Eval("QuestionType")%>
                                                                    </td>
                                                                    <td class="ms-vb2 questiontoken">
                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                        
                                                                    </td>

                                                                    <td class="ms-vb2">
                                                                        <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" class="fleft" alt="Edit" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="service-catlogdel-icon fleft" runat="server"
                                                                            ImageUrl="/Content/images/grayDelete.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ms-alternating queSection-tableItem">
                                                <td class="ms-vb2">
                                                    <asp:DropDownList ID="ddlSectionOrderBy" runat="server" Width="60%" CssClass="itsmDropDownList aspxDropDownList">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hfServiceSectionID" runat="server" />
                                                </td>
                                                <td class="ms-vb2 qsectionname" style="font-weight: bold;">
                                                    <%# Eval("SectionName") %>
                                                    <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" alt="Edit" class="fleft" title="Edit" onclick='editSection(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton ID="btDeleteSection" OnClientClick="return deleteSectionConfirm(this);" CssClass="service-catlogdel-icon fleft" runat="server"
                                                        ImageUrl="/Content/images/grayDelete.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteSection_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <table class="ms-listviewtable queTabSecTable-listviewtable" cellpadding="0" cellspacing="0" width="100%">
                                                        <asp:Repeater ID="rSectionQuestions" runat="server" OnItemDataBound="RSectionQuestions_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <tr class="queTabSecTable-headerTr">
                                                                    <th class="queTabSecTable-headerTd" width="80">Order</th>
                                                                    <th class="queTabSecTable-headerTd" width="110" align="left">Question</th>
                                                                    <th class="queTabSecTable-headerTd" width="80" align="left">Question Type</th>
                                                                    <th class="queTabSecTable-headerTd" width="80" align="left">Token</th>
                                                                    <th class="queTabSecTable-headerTd" align="left">Help Text</th>
                                                                    <th class="queTabSecTable-headerTd" align="left" width="70"></th>
                                                                </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="queSection-tableItem">
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server" Width="60%" CssClass="itsmDropDownList aspxDropDownList">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <%# Eval("QuestionType")%>
                                                                    </td>
                                                                    <td class="ms-vb2 questiontoken">

                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                       
                                                                    </td>

                                                                    <td class="ms-vb2">
                                                                        <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px" class="fleft" alt="Edit" title="Edit"
                                                                            onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="service-catlogdel-icon fleft"
                                                                            runat="server" ImageUrl="/Content/images/grayDelete.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr class="ms-alternating queSection-tableItem ">
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server" Width="60%" CssClass="itsmDropDownList aspxDropDownList">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <%# Eval("QuestionType")%>
                                                                    </td>
                                                                    <td class="ms-vb2 questiontoken">

                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                       
                                                                    </td>

                                                                    <td class="ms-vb2">
                                                                        <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" alt="Edit" class="fleft" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="service-catlogdel-icon fleft"
                                                                            runat="server" ImageUrl="/Content/images/grayDelete.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div style="float: left;">
                        <dx:ASPxButton ID="Span7" runat="server" Text="New Section" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(){ editSection(true, 0); }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="Span1" runat="server" Text="New Question" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(){ editQuestion(true, 0); }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="Span5" runat="server" Text="Skip Logic" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(){ editQuestionBranch(); }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btSaveSectionQuestionOrder" ClientInstanceName="btSaveSectionQuestionOrder" ClientVisible="false" runat="server"
                            OnClick="BtSaveSectionQuestionOrder_Click" CssClass="primary-blueBtn">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btSaveSectionQuestionOrder1" ClientInstanceName="btSaveSectionQuestionOrder1" ClientVisible="false" runat="server"
                            OnClick="BtSaveSectionQuestionOrder_Click" CssClass="primary-blueBtn">
                        </dx:ASPxButton>

                    </div>
                    <div style="float: right;">
                        <dx:ASPxButton ID="btPreviousTab" runat="server" Text="<< Previous" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(){ 
                                        previousTab(2, false); 
                                        }" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btNextTab" runat="server" Text="Next >>" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(){ 
                                        nextTab(2, false); 
                                        }" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btSaveQuestionOrder" runat="server" Text="Save Order" AutoPostBack="false" CssClass="primary-blueBtn">
                            <%--<ClientSideEvents Click="closeAndSaveTab2" />--%>
                            <ClientSideEvents Click="SaveOrder" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btCloseAndSaveTab2" runat="server" Text="Save & Close" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="closeAndSaveTab2" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
<div class="row services-field-wrap">
    <asp:Panel ID="tabMapping" runat="server" CssClass="tab tab4" Visible="false">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <div class="col-md-4 col-sm-4 col-xs-12 noPadding" id="divMappingTickets" visible="false" runat="server" >
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Ticket/Task:</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlTaskTickets" runat="server" AutoPostBack="true" onchange="LoadingPanel.Show();"
                            OnSelectedIndexChanged="DDLTaskTickets_SelectedIndexChanged" CssClass=" itsmDropDownList aspxDropDownList">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="divMappingTickets1" visible="false" runat="server" style="float: left; width: 100%; padding: 10px 0px 8px 5px;">
                </div>
            </div>
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding formtable">
                    <table class="ms-listviewtable serviceAgent-queTabSecTable" cellpadding="0" cellspacing="0" width="100%">
                        <asp:Repeater ID="rItemMapping" runat="server" OnItemDataBound="RItemMapping_ItemDataBound">
                            <HeaderTemplate>
                                <tr class="ms-viewheadertr queTabSecTable-headerTr">
                                    <%--<th class=" header-height addtaskheaderboarder" width="5%"></th>--%>
                                    <th id="thEnableMapping" runat="server" class=" queTabSecTable-headerTd" width="5%">Enable</th>
                                    <th class=" queTabSecTable-headerTd" width="30%">Field Name</th>
                                    <th class=" queTabSecTable-headerTd" width="35%">Pick Value From <b style="font-weight: 900;">(Current Selection=[$Current$])</b></th>
                                    <th colspan="2" class=" queTabSecTable-headerTd" width="30%">Default Value</th>
                                </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr id="ticketGroupTr" runat="server" class="ticketgrouptr queSection-tableItem">
                                    <td colspan="4">
                                        <asp:Panel ID="ticketGroupPanel" runat="server" CssClass="ticketgrouppanel"></asp:Panel>
                                    </td>
                                </tr>
                                <tr class="queSection-tableItem">
                                    <%--<td class="ms-vb2">&nbsp;</td>--%>
                                    <td id="tdEnableMapping" runat="server" class="textpadding">
                                        <asp:CheckBox ID="chkbxMapping" runat="server" />
                                    </td>
                                    <td id="ticketFieldName" runat="server" class="textpadding"></td>
                                    <td valign="top" class="ms-vb2 ">
                                        <asp:DropDownList runat="server" CssClass="ddlquestionmapped itsmDropDownList aspxDropDownList" ID="ddlQuestionMapped" Width="60%"></asp:DropDownList>
                                        <asp:HiddenField ID="hfQuestionMapID" runat="server" />
                                        <asp:HiddenField ID="hfFieldInternalName" runat="server" />
                                        <asp:HiddenField ID="hfServieTaskID" runat="server" />
                                    </td>
                                    <td valign="middle" align="center" class="paddingLR5">
                                        <asp:Image ID="imgQuestionMapArrow" onclick="imgQuestionMapArrow_Click(this)" CssClass="imgquestionmaparrow" runat="server"
                                            Visible="false" ImageUrl="/Content/images/arrow-right.png" AlternateText="->" Style="cursor: pointer;" />
                                    </td>
                                    <td class="ms-vb2 mappedquestion-defaultval">
                                        <asp:Panel runat="server" ID="controlPanel" Width="100%" CssClass="mapfield-defaultvalcontrol"></asp:Panel>
                                        <asp:Panel runat="server" ID="ctpUserCollection" Width="96%" CssClass="mapfield-usercollection">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="ticketGroupTr" runat="server" class="ticketgrouptr queSection-tableItem">
                                    <td colspan="4">
                                        <asp:Panel ID="ticketGroupPanel" runat="server" CssClass="ticketgrouppanel"></asp:Panel>
                                    </td>
                                </tr>
                                <tr class="ms-alternating queSection-tableItem">
                                    <%--<td class="ms-vb2">&nbsp;</td>--%>
                                    <td id="tdEnableMapping" runat="server" class="textpadding">
                                        <asp:CheckBox ID="chkbxMapping" runat="server" />
                                    </td>
                                    <td id="ticketFieldName" runat="server" class="textpadding"></td>
                                    <td valign="top" class="ms-vb2">
                                        <asp:DropDownList runat="server" ID="ddlQuestionMapped" Width="60%" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                        <asp:HiddenField ID="hfQuestionMapID" runat="server" />
                                        <asp:HiddenField ID="hfFieldInternalName" runat="server" />
                                        <asp:HiddenField ID="hfServieTaskID" runat="server" />

                                    </td>
                                    <td valign="middle" align="center" class="paddingLR5">
                                        <asp:Image ID="imgQuestionMapArrow" onclick="imgQuestionMapArrow_Click(this)" CssClass="imgquestionmaparrow" runat="server"
                                            Visible="false" ImageUrl="/Content/images/arrow-right.png" AlternateText="->" Style="cursor: pointer;" />
                                    </td>
                                    <td class="ms-vb2 mappedquestion-defaultval">
                                        <asp:Panel runat="server" CssClass="mapfield-defaultvalcontrol" Width="100%" ID="controlPanel"></asp:Panel>
                                        <asp:Panel runat="server" ID="ctpUserCollection" Width="96%" CssClass="mapfield-usercollection">
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
            <div class="row mappingTab-btnWrap">
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                    <div style="float: left;">
                        <dx:ASPxButton ID="btVariables" CssClass="primary-blueBtn" ClientInstanceName="btVariables" runat="server" Text="Variables"
                            AutoPostBack="false">
                        </dx:ASPxButton>
                    </div>
                    <div style="float: right;">
                        <dx:ASPxButton ID="Span4" runat="server" Text="<< Previous" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(){ previousTab(4, false); }" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="Span3" runat="server" Text="Save" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="validateAndSaveQuestionMapping" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btSaveQuestionMapping" CssClass="primary-blueBtn" ClientInstanceName="btSaveQuestionMapping" runat="server" ClientVisible="false"
                            Text="Save" AutoPostBack="true" OnClick="BtSaveQuestionMapping_Click">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btSaveQuestionMapping1" ClientInstanceName="btSaveQuestionMapping1" CssClass="primary-blueBtn" runat="server" ClientVisible="false"
                            AutoPostBack="true" Text="Save" OnClick="BtSaveQuestionMapping_Click">
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnCloseAndSaveTab4" CssClass="primary-blueBtn" runat="server" Text="Save & Close" AutoPostBack="false">
                            <ClientSideEvents Click="closeAndSaveTab4" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>
</div>

<asp:Panel runat="server" ID="tabRating" CssClass="tab tab5 ratingContainer">
    <table class="ms-listviewtable" cellpadding="0" cellspacing="0" width="100%">
        <asp:Repeater ID="rRatingQuestions" EnableViewState="false" runat="server" OnItemDataBound="RRatingQuestions_ItemDataBound">
            <HeaderTemplate>
                <tr class="ms-viewheadertr">
                    <th class="ms-vh2-nofilter ms-vh2-gridview  header-height"></th>
                    <th class="ms-vh2-nofilter ms-vh2-gridview  header-height">Name
                    </th>
                    <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Description
                    </th>
                    <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Rating
                    </th>
                    <th class="ms-vh2-nofilter ms-vh2-gridview header-height"></th>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td class="ms-vb2">
                        <asp:DropDownList ID="ddlRatingQuestionOrderBy" Width="50%" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                    </td>
                    <td class="ms-vb2">
                        <a onclick='editRatingQuestion(false, <%# Eval("ID")%>)' class="ratingUrl" href="javascript:void(0);">
                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                        </a>

                    </td>
                    <td class="ms-vb2">
                        <%# Eval("HelpText")%>
                    </td>
                    <td class="ms-vb2">
                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : Convert.ToString(Eval("TokenName")) %>
                    </td>

                    <td class="ms-vb2">
                        <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" alt="Edit" title="Edit" onclick='editRatingQuestion(false, <%# Eval("ID")%>)' />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="ms-alternating">
                    <td class="ms-vb2">
                        <asp:DropDownList ID="ddlRatingQuestionOrderBy" runat="server" Width="50%"
                            CssClass="itsmDropDownList aspxDropDownList">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                    </td>
                    <td class="ms-vb2">
                        <a onclick='editRatingQuestion(false, <%# Eval("ID")%>)' class="ratingUrl" href="javascript:void(0);">
                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                        </a>
                    </td>
                    <td class="ms-vb2">
                        <%# Eval("HelpText")%>
                    </td>
                    <td class="ms-vb2"><%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : Convert.ToString(Eval("TokenName")) %></td>


                    <td class="ms-vb2">
                        <img src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" alt="Edit" title="Edit" onclick='editRatingQuestion(false, <%# Eval("ID")%>)' />

                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>

    </table>

    <div style="float: left; width: 100%; padding-top: 10px; padding-bottom: 1px;">
        <div style="float: left;">
            <dx:ASPxButton ID="Span9" runat="server" Text="Add Rating" AutoPostBack="false" CssClass="primary-blueBtn">
                <ClientSideEvents Click="function(){ editRatingQuestion(true, 0); }" />
            </dx:ASPxButton>
            <%--<span class="button-bg" id="Span9" onclick="editRatingQuestion(true, 0)">
                        <b style="float: left; font-weight: normal;">Add Rating</b>
                    </span>--%>
        </div>
        <div style="float: right;">
            <dx:ASPxButton ID="btnRatingPrevious" runat="server" Text="<< Previous" CssClass="primary-blueBtn" AutoPostBack="false">
                <ClientSideEvents Click="function(){ previousTab(1, false); }" />
            </dx:ASPxButton>

            <dx:ASPxButton ID="btnRatingNext" runat="server" CssClass="primary-blueBtn" AutoPostBack="false" Text="Next >>">
                <ClientSideEvents Click="function(){ 
                                    nextTab(1, false); 
                                    }" />
            </dx:ASPxButton>

            <%--<dx:ASPxButton ID="btnRatingSave" runat="server" Text="Save Order">
                            <ClientSideEvents Click="saveSectionQuestOrder" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnRatingSaveAndClose" runat="server" Text="Save & Close" AutoPostBack="false">
                            <ClientSideEvents Click="closeAndSaveTab2" />
                        </dx:ASPxButton>--%>
        </div>
    </div>
</asp:Panel>

<%--<asp:panel id="tabQuestions" runat="server" cssclass="tab tab3" style="float: left;" visible="false">
    <table style="width: 100%; border-collapse: collapse; border: none;">
        <tr>
            <td>
                <asp:Panel ID="lWorkFlow" runat="server" Style="width: 88.5%; float: left; margin: 0px 10px 20px;">
                </asp:Panel>
                <div style="float: left; width: 100%; padding-top: 10px;"><span style="font-weight: bold;">Un-Categorized:</span></div>
                <div style="float: left; width: 100%; max-height: 200px; overflow: auto;">
                    <div style="float: left; width: 100%; padding-top: 10px; padding-bottom: 1px;">
                        <label id="noUnCategorizedDataMsg" runat="server" visible="false" style="padding-left: 11px" title="No Un-categorized Questions">No Un-categorized Questions</label>
                    </div>
                    <table id="unCategoriezedQuestion" class="formtable" runat="server" visible="false">
                        <tr>
                            <td>
                                <table class="ms-listviewtable addboarder" cellpadding="0" cellspacing="0" width="99%">
                                    <asp:Repeater ID="rNonSectionQuestions" runat="server" OnItemDataBound="rNonSectionQuestions_ItemDataBound">
                                        <HeaderTemplate>
                                            <tr class="ms-viewheadertr">
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Question</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Question Type</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Token</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Help Text</th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height"></th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="ms-vb2 questiontitle">
                                                    <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                        <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                    </a>
                                                </td>
                                                <td class="ms-vb2">
                                                    <%# UGITUtility.AddSpaceBeforeWord(Convert.ToString(Eval("QuestionType")))%>
                                                </td>
                                                <td class="ms-vb2 questiontoken">
                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                </td>
                                                <td class="ms-vb2">
                                                    <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                </td>
                                                <td class="ms-vb2">
                                                    <img src="/Content/images/edit-icon.png" style="cursor: pointer;" class="fleft" alt="Edit" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton ID="btDeleteQuestion" OnClientClick="return deleteQuestionConfirm(this);" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ms-alternating">
                                                <td class="ms-vb2 questiontitle">
                                                    <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                        <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                    </a>
                                                </td>
                                                <td class="ms-vb2">
                                                    <%# UGITUtility.AddSpaceBeforeWord(Convert.ToString(Eval("QuestionType")))%>
                                                </td>
                                                <td class="ms-vb2 questiontoken"><%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %></td>

                                                <td class="ms-vb2">
                                                    <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                </td>
                                                <td class="ms-vb2">
                                                    <img src="/Content/images/edit-icon.png" style="cursor: pointer;" class="fleft" alt="Edit" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" runat="server" class="fleft" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="float: left; width: 100%; padding-top: 10px;"><span style="font-weight: bold;">Sections:</span></div>
                <div style="float: left; width: 88.5%; overflow: auto;">
                    <table class="formtable">
                        <tr>
                            <td>
                                <table class="addboarder margin" style="border-collapse:unset" cellpadding="0" cellspacing="0" width="100%">
                                    <asp:Repeater ID="rSections" runat="server" OnItemDataBound="RSections_ItemDataBound">
                                        <HeaderTemplate>
                                            <tr class="ms-viewheadertr">
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height" width="50">Order
                                                </th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height">Section
                                                </th>
                                                <th class="ms-vh2-nofilter ms-vh2-gridview header-height"></th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="section-item">
                                                <td class="ms-vb2">
                                                    <asp:DropDownList ID="ddlSectionOrderBy" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hfServiceSectionID" runat="server" />
                                                </td>
                                                <td class="ms-vb2 qsectionname" style="font-weight: bold;">
                                                    <%# Eval("SectionName") %>
                                                    <img src="/Content/images/edit-icon.png" style="cursor: pointer;" alt="Edit" class="fleft" title="Edit" onclick='editSection(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton ID="btDeleteSection" OnClientClick="return deleteSectionConfirm(this);" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#Eval("ID") %>' OnClick="btDeleteSection_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <table class="ms-listviewtable" cellpadding="0" cellspacing="0" width="100%">
                                                        <asp:Repeater ID="rSectionQuestions" runat="server" OnItemDataBound="RSectionQuestions_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <tr>
                                                                    <th width="50">Order
                                                                    </th>
                                                                    <th align="left">Question
                                                                    </th>
                                                                    <th align="left">Question Type
                                                                    </th>
                                                                    <th align="left">Token
                                                                    </th>

                                                                    <th align="left">Help Text
                                                                    </th>
                                                                    <th align="left" width="50"></th>
                                                                </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="ms-alternating">
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>

                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2"><%# Eval("QuestionType")%></td>
                                                                    <td class="ms-vb2 questiontoken">
                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                                    </td>

                                                                    <td class="ms-vb2"><%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%></td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/edit-icon.png" style="cursor: pointer;" alt="Edit" class="fleft" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr>
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>

                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <%# Eval("QuestionType")%>
                                                                    </td>
                                                                    <td class="ms-vb2 questiontoken">
                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                        
                                                                    </td>

                                                                    <td class="ms-vb2">
                                                                        <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/edit-icon.png" style="cursor: pointer;" class="fleft" alt="Edit" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ms-alternating section-item">
                                                <td class="ms-vb2">
                                                    <asp:DropDownList ID="ddlSectionOrderBy" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hfServiceSectionID" runat="server" />
                                                </td>
                                                <td class="ms-vb2 qsectionname" style="font-weight: bold;">
                                                    <%# Eval("SectionName") %>
                                                    <img src="/Content/images/edit-icon.png" style="cursor: pointer;" alt="Edit" class="fleft" title="Edit" onclick='editSection(false, <%# Eval("ID")%>)' />
                                                    <asp:ImageButton ID="btDeleteSection" OnClientClick="return deleteSectionConfirm(this);" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteSection_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <table class="ms-listviewtable" cellpadding="0" cellspacing="0" width="100%">
                                                        <asp:Repeater ID="rSectionQuestions" runat="server" OnItemDataBound="RSectionQuestions_ItemDataBound">
                                                            <HeaderTemplate>
                                                                <tr>
                                                                    <th width="50">Order</th>
                                                                    <th align="left">Question</th>
                                                                    <th align="left">Question Type</th>
                                                                    <th align="left">Token</th>
                                                                    <th align="left">Help Text</th>
                                                                    <th align="left" width="50"></th>
                                                                </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <%# Eval("QuestionType")%>
                                                                    </td>
                                                                    <td class="ms-vb2 questiontoken">

                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                       
                                                                    </td>

                                                                    <td class="ms-vb2">
                                                                        <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/edit-icon.png" style="cursor: pointer;" class="fleft" alt="Edit" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <AlternatingItemTemplate>
                                                                <tr class="ms-alternating">
                                                                    <td class="ms-vb2">
                                                                        <asp:DropDownList ID="ddlSectionOrderBy" runat="server">
                                                                        </asp:DropDownList>
                                                                        <asp:HiddenField ID="hfServiceQuestionID" runat="server" />
                                                                    </td>
                                                                    <td class="ms-vb2 questiontitle">
                                                                        <a onclick='editQuestion(false, <%# Eval("ID")%>)' href="javascript:void(0);">
                                                                            <asp:Label ID="lbQuestionTitle" runat="server"></asp:Label>
                                                                        </a>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <%# Eval("QuestionType")%>
                                                                    </td>
                                                                    <td class="ms-vb2 questiontoken">

                                                                        <%# string.IsNullOrEmpty(Convert.ToString(Eval("TokenName"))) ? string.Empty : "[$"+Convert.ToString(Eval("TokenName"))+"$]" %>
                                                       
                                                                    </td>

                                                                    <td class="ms-vb2">
                                                                        <%# Eval("HelpText").ToString().Replace("\n\r", "<br/>").Replace("\n", "<br/>")%>
                                                                    </td>
                                                                    <td class="ms-vb2">
                                                                        <img src="/Content/images/edit-icon.png" style="cursor: pointer;" alt="Edit" class="fleft" title="Edit" onclick='editQuestion(false, <%# Eval("ID")%>)' />
                                                                        <asp:ImageButton OnClientClick="return deleteQuestionConfirm(this);" ID="btDeleteQuestion" CssClass="fleft" runat="server" ImageUrl="/Content/images/delete-icon.png" CommandArgument='<%#  Eval("ID") %>' OnClick="btDeleteQuestion_Click" />
                                                                    </td>
                                                                </tr>
                                                            </AlternatingItemTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>

                <div style="float: left; width: 88.5%; padding-top: 10px; padding-bottom: 1px;">
                    <div style="float: left;">
                        <dx:ASPxButton ID="Span7" runat="server" Text="New Section" AutoPostBack="false">
                            <ClientSideEvents Click="function(){ editSection(true, 0); }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="Span1" runat="server" Text="New Question" AutoPostBack="false">
                            <ClientSideEvents Click="function(){ editQuestion(true, 0); }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="Span5" runat="server" Text="Skip Logic" AutoPostBack="false">
                            <ClientSideEvents Click="function(){ editQuestionBranch(); }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btSaveSectionQuestionOrder" ClientInstanceName="btSaveSectionQuestionOrder" ClientVisible="false" runat="server" OnClick="BtSaveSectionQuestionOrder_Click"></dx:ASPxButton>
                        <dx:ASPxButton ID="btSaveSectionQuestionOrder1" ClientInstanceName="btSaveSectionQuestionOrder1" ClientVisible="false" runat="server" OnClick="BtSaveSectionQuestionOrder_Click"></dx:ASPxButton>

                    </div>
                    <div style="float: right;">
                        <dx:ASPxButton ID="btPreviousTab" runat="server" Text="<< Previous" AutoPostBack="false">
                            <ClientSideEvents Click="function(){ 
                                    previousTab(2, false); 
                                    }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btNextTab" runat="server" Text="Next >>" AutoPostBack="false">
                            <ClientSideEvents Click="function(){ 
                                    nextTab(2, false); 
                                    }" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btSaveQuestionOrder" runat="server" Text="Save Order" AutoPostBack="false">
                            <%--<ClientSideEvents Click="closeAndSaveTab2" />--   
                            <ClientSideEvents Click="SaveOrder" />                            
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btCloseAndSaveTab2" runat="server" Text="Save & Close" AutoPostBack="false">
                            <ClientSideEvents Click="closeAndSaveTab2" />
                        </dx:ASPxButton>

                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:panel>--%>

<asp:Panel ID="tabTasks" runat="server" CssClass="tab tab3 services-field-wrap" Style="float: left; width: 100%; margin-top: -12px;" Visible="false">
    <div class="formtable col-md-12 col-sm-12 col-xs-12">
        <div style="margin-top:2px" id="dvTaskDisclaimer" runat="server" visible="false">
            <asp:Label ID="lbTaskDisclaimer" Visible="false" runat="server" ForeColor="Red" Text="Service will not create any task if &ldquo;Create Parent Service Request&rdquo; option is unchecked."></asp:Label>
        </div>
        
            <dx:ASPxPanel ID="pnlSubTickets" ClientInstanceName="pnlSubTickets" runat="server">
            </dx:ASPxPanel>
        
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <div style="float: left; padding-top: 10px;">
                    <dx:ASPxButton ID="btNewTask" runat="server" Text="New Task" AutoPostBack="false" CssClass="primary-blueBtn">
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btNewTicket" runat="server" Text="New Ticket" AutoPostBack="false" CssClass="primary-blueBtn">
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btTaskSkipLogic" runat="server" Text="Skip Logic" AutoPostBack="false" CssClass="primary-blueBtn">
                        <ClientSideEvents Click="editTaskCondition" />
                    </dx:ASPxButton>
                </div>

                <div style="float: right;">
                    <dx:ASPxButton ID="btPreviousTab1" runat="server" Text="<< Previous" AutoPostBack="false" CssClass="primary-blueBtn">
                        <ClientSideEvents Click="function(){ previousTab(3, false); }" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="btnexttab1" runat="server" Text="Next >>" AutoPostBack="false" CssClass="primary-blueBtn">
                        <ClientSideEvents Click="function(){ nextTab(3, false); }" />
                    </dx:ASPxButton>

                    <dx:ASPxButton ID="cancelServicePopup" runat="server" Text="Save & Close" OnClick="cancelServicePopup_Click" CssClass="primary-blueBtn">
                        <%--<ClientSideEvents Click="cancelServicePopup" />--%>
                    </dx:ASPxButton>

                </div>
            </div>
        </div>
    </div>
</asp:Panel>

<%--<asp:panel id="tabMapping" runat="server" cssclass="tab tab4" visible="false">
    <div id="divMappingTickets" visible="false" runat="server" style="float: left; width: 100%; padding: 10px 0px 8px 5px;">
        <span>Ticket/Task: </span>
        <span>

            <asp:DropDownList ID="ddlTaskTickets" runat="server" AutoPostBack="true" onchange="LoadingPanel.Show();" OnSelectedIndexChanged="DDLTaskTickets_SelectedIndexChanged">
            </asp:DropDownList>
        </span>
    </div>

    <table class="formtable">
        <tr>
            <td>
                <table class="ms-listviewtable addboarder" cellpadding="0" cellspacing="0" width="100%">
                    <asp:Repeater ID="rItemMapping" runat="server" OnItemDataBound="RItemMapping_ItemDataBound">
                        <HeaderTemplate>
                            <tr class="ms-viewheadertr addtaskheader">
                                <%--<th class=" header-height addtaskheaderboarder" width="5%"></th>-
                                <th id="thEnableMapping" runat="server" class=" header-height addtaskheaderboarder textpadding" width="5%">Enable</th>
                                <th class=" header-height addtaskheaderboarder textpadding" width="30%">Field Name</th>
                                <th class=" header-height addtaskheaderboarder" width="35%">Pick Value From <b style="font-weight: 900;">(Current Selection=[$Current$])</b></th>
                                <th colspan="2" class=" header-height addtaskheaderboarder" width="30%">Default Value</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="ticketGroupTr" runat="server" class="ticketgrouptr">
                                <td colspan="4">
                                    <asp:Panel ID="ticketGroupPanel" runat="server" CssClass="ticketgrouppanel"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <%--<td class="ms-vb2">&nbsp;</td>-
                                <td id="tdEnableMapping" runat="server" class="textpadding">
                                    <asp:CheckBox ID="chkbxMapping" runat="server" />
                                </td>
                                <td id="ticketFieldName" runat="server" class="textpadding"></td>
                                <td valign="top" class="ms-vb2 ">
                                    <asp:DropDownList runat="server" CssClass="ddlquestionmapped" ID="ddlQuestionMapped" Width="98%"></asp:DropDownList>
                                    <asp:HiddenField ID="hfQuestionMapID" runat="server" />
                                    <asp:HiddenField ID="hfFieldInternalName" runat="server" />
                                    <asp:HiddenField ID="hfServieTaskID" runat="server" />
                                </td>
                                <td valign="middle" align="center" class="paddingLR5">
                                    <asp:Image ID="imgQuestionMapArrow" onclick="imgQuestionMapArrow_Click(this)" CssClass="imgquestionmaparrow" runat="server" 
                                        Visible="false" ImageUrl="/Content/images/arrow-right.png" AlternateText="->" Style="cursor: pointer;" />
                                </td>
                                <td class="ms-vb2 mappedquestion-defaultval">
                                    <asp:Panel runat="server" ID="controlPanel" Width="96%" CssClass="mapfield-defaultvalcontrol"></asp:Panel>
                                    <asp:Panel runat="server" ID="ctpUserCollection" Width="96%" CssClass="mapfield-usercollection">
                                    </asp:Panel>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="ticketGroupTr" runat="server" class="ticketgrouptr ">
                                <td colspan="4">
                                    <asp:Panel ID="ticketGroupPanel" runat="server" CssClass="ticketgrouppanel"></asp:Panel>
                                </td>
                            </tr>
                            <tr class="ms-alternating alternatebanding">
                                <%--<td class="ms-vb2">&nbsp;</td>-
                                <td id="tdEnableMapping" runat="server" class="textpadding">
                                    <asp:CheckBox ID="chkbxMapping" runat="server" />
                                </td>
                                <td id="ticketFieldName" runat="server" class="textpadding"></td>
                                <td valign="top" class="ms-vb2">
                                    <asp:DropDownList runat="server" ID="ddlQuestionMapped" Width="98%"></asp:DropDownList>
                                    <asp:HiddenField ID="hfQuestionMapID" runat="server" />
                                    <asp:HiddenField ID="hfFieldInternalName" runat="server" />
                                    <asp:HiddenField ID="hfServieTaskID" runat="server" />

                                </td>
                                <td valign="middle" align="center" class="paddingLR5">
                                    <asp:Image ID="imgQuestionMapArrow" onclick="imgQuestionMapArrow_Click(this)" CssClass="imgquestionmaparrow" runat="server" 
                                        Visible="false" ImageUrl="/Content/images/arrow-right.png" AlternateText="->" Style="cursor: pointer;" />
                                </td>
                                <td class="ms-vb2 mappedquestion-defaultval">
                                    <asp:Panel runat="server" CssClass="mapfield-defaultvalcontrol" Width="96%" ID="controlPanel"></asp:Panel>
                                    <asp:Panel runat="server" ID="ctpUserCollection" Width="96%" CssClass="mapfield-usercollection">
                                    </asp:Panel>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
    </table>

    <div style="float: left; width: 100%; padding-top: 10px; padding-bottom: 1px;">
        <div style="float: left;">
            <span style="float: right; padding-right: 5px;">
                <dx:ASPxButton ID="btVariables" ClientInstanceName="btVariables" runat="server" Text="Variables" AutoPostBack="false"></dx:ASPxButton>
            </span>

        </div>
        <div style="float: right;">
            <dx:ASPxButton ID="Span4" runat="server" Text="<< Previous" AutoPostBack="false">
                <ClientSideEvents Click="function(){ previousTab(4, false); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="Span3" runat="server" Text="Save" AutoPostBack="false">
                <ClientSideEvents Click="validateAndSaveQuestionMapping" />
            </dx:ASPxButton>

            <dx:ASPxButton ID="btSaveQuestionMapping" ClientInstanceName="btSaveQuestionMapping" runat="server" ClientVisible="false"
                Text="Save" AutoPostBack="true" OnClick="BtSaveQuestionMapping_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btSaveQuestionMapping1" ClientInstanceName="btSaveQuestionMapping1" runat="server" ClientVisible="false"
                AutoPostBack="true" Text="Save" OnClick="BtSaveQuestionMapping_Click">
            </dx:ASPxButton>

            <dx:ASPxButton ID="btnCloseAndSaveTab4" runat="server" Text="Save & Close" AutoPostBack="false">
                <ClientSideEvents Click="closeAndSaveTab4" />
            </dx:ASPxButton>

        </div>
    </div>
</asp:panel>--%>

<dx:ASPxPopupControl ID="copyLinkPopup" runat="server" Modal="True" Width="100%" CssClass="aspxPopup" SettingsAdaptivity-Mode="Always"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="copyLinkPopup"
    HeaderText="Copy Service Link" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row">
                    <div>
                        <dx:ASPxMemo CssClass="aspxMemo-linkBox serviceLinkBox" ReadOnly="true" ClientInstanceName="serviceLinkBox" ID="memoServiceLinkBox" Width="100%"
                            runat="server">
                        </dx:ASPxMemo>
                    </div>
                </div>
                <div class="row adminCopyLink-btnWrap">
                    <ul class="adminCopyLink-btnUl">
                        <li runat="server" id="liCancel" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <a id="aCancel" onclick="copyLinkPopup.Hide();" class="cancel-linkBtn" href="javascript:void(0);">Cancel</a>
                        </li>
                    </ul>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>




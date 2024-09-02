<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditTask.ascx.cs" Inherits="uGovernIT.Web.EditTask" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script src="../../../Scripts/jquery.contextMenu.js"></script>
<script src="../../../Scripts/DMS/jquery.dynatree.js"></script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    fieldset {
        border: 1px solid #c0c0c0;
        padding: 0.35em 0.6em 0.75em;
    }

    legend {
        width: auto;
        border: none;
        font-size: 13px;
        font-weight: bold !important;
        margin-bottom: 0px !important;
        padding: 0px 4px;
    }

    .form-group {
        margin-bottom: 5px !important;
    }

    .tskbehaviour-opt {
        margin-top: 0;
    }

    .tskbehaviour-opt td {
        padding-right: 20px;
        padding-top: 3px;
    }

    .tskbehaviour-opt tr {
        display: flex;
        flex-wrap: wrap;
    }

    .tskbehaviour-opt input {
        float: left;
    }

    .tskbehaviour-opt label > b {
        padding: 2px 0 0 2px;
    }

    .tskbehaviour-opt label {
        margin-top: -2px;
        margin-right: 0;
        margin-left: 5px;
    }

    .actionBtn-wrap {
        position: unset !important;
    }
    .crm-checkWrap label {
        margin-left: 0;
    }
    /*body {
        overflow-y: auto !important;
    }

    #s4-leftpanel {
        display: none;
    }

    .s4-ca {
        margin-left: 0px !important;
    }

    #s4-ribbonrow {
        height: auto !important;
        min-height: 0px !important;
    }

    #s4-ribboncont {
        display: none;
    }

    #s4-titlerow {
        display: none;
    }

    .s4-ba {
        width: 100%;
        min-height: 0px !important;
    }

    #s4-workspace {
        float: left;
        width: 100%;
        overflow: auto !important;
    }

    body #MSO_ContentTable {
        min-height: 0px !important;
        position: inherit;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }*/

    /*.pctcomplete {
        text-align: right;
    }*/

    /*.estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fleft {
        float: left;
    }

    .proposeddatelb {
        padding-top: 5px;
        padding-right: 4px;
        float: left;
    }



    

    .attachment-container {
        float: left;
        width: 100%;
        padding-top: 7px;
    }

        .attachment-container .label {
            float: left;
            width: 24%;
            padding-left: 23px;
        }

        .attachment-container .attachmentform {
            float: left;
            width: 100%;
        }

    .attachmentform .oldattachment, .attachmentform .newattachment {
        float: left;
        width: 100%;
    }

    .attachmentform .fileupload {
        float: left;
        width: 100%;
    }

    .fileupload span, .fileread span {
        float: left;
    }

    .fileupload label, .fileread label {
        float: left;
        padding-left: 5px;
        font-weight: bold;
        padding-top: 3px;
        cursor: pointer;
    }

    .attachmentform .fileread {
        float: left;
        width: 100%;
    }

    .attachment-container .addattachment {
        float: left;
    }

        .attachment-container .addattachment img {
            border: 1px outset;
            padding: 3px;
        }

    .overlay {
        display: none;
        position: absolute;
        left: 0%;
        top: 0%;
        padding: 25px;
        background-color: black;
        width: 93%;
        height: 740px;
        -moz-opacity: 0.3;
        opacity: .30;
        filter: alpha(opacity=30);
        z-index: 100;
    }*/
    /*recurring Task Style
    .recurringtaskdiv {
        display: inline-flex;
        width: 100%;
    }*/

    /*  .recurringtaskdiv span,
        .recurringtaskdiv input,
        .recurringtaskdiv select {
            display: inline;
        }*/

    /*.lblmessage {
        color: red;
    }

    .recurringcount {
        width: 50px;
    }

    table#tbAssignTo td.assignToPct input {*/
    /*height: 23px;*/
    /*width: 85px;
        text-align: right;
    }

    .aspNetDisabled {
        color: #A5A5A5 !important;
    }

    .assigntopct {
        height: 23px;
        float: left;*/
    /*margin-top: 4px;*/
    /*}

    .createuser-picker .ms-inputuserfield {
        height: 21px !important;
    }

    .lbusername {
        padding: 4px 0px;
        float: left;
        width: 100%;
    }

    .lbDescription {
        white-space: pre-wrap;
    }

    .bottom {
        margin-bottom: 4px;
    }

    .content-bottom {
        padding-bottom: 15px;
    }

    .bg-light-blueDrodown {
        background: #f8fafc;
        border: 1px solid #ccd4e1 !important;
        border-radius: 4px !important;
        margin-top: 1px;
    }

    .ms-formbodyModule {
        background-color: #f8fafc;
        border: 1px solid #A5A5A5;
        padding: 5px 5px 5px;
        vertical-align: top;
    }

    .divPredecessors {
        display: none !important;
    }

    .show {
        display: block !important;
    }

    .actionBtn-wrap {
        position: unset !important;
    }*/
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        //var list = document.getElementsByTagName("a");
        $('.uploadedFileContainer').find('a').addClass('hyperLinkIcon');
        $('.uploadedFileContainer').find('img').addClass('cancelUploadedFiles');
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
        $(".imgtoggle").click(function () {
            $(".divPredecessors").toggleClass("show");
        });
    });




    function modifyStatusFromPctComplete() {
        var statusObj = $("#<%= ddlStatus.ClientID %>");
        var pctCompleteObj = $("#<%= txtPctComplete.ClientID%>");
        var pctComplete = Number($.trim(pctCompleteObj.val()));

        if (pctComplete <= 0) {
            statusObj.val("Not Started");
            pctCompleteObj.val("0");
        }
        else if (pctComplete >= 100) {
            statusObj.val("Completed");
            pctCompleteObj.val("100");
        }
        else {
            statusObj.val("In Progress");
        }

        AutoCalculateRemainingEstimateHours();
    }

    function modifyPctCompleteFromStatus() {
        var statusObj = $("#<%= ddlStatus.ClientID %>");
        var pctCompleteObj = $("#<%= txtPctComplete.ClientID%>");
        var pctComplete = Number($.trim(pctCompleteObj.val()));

        if (statusObj.val() == "Not Started") {
            pctCompleteObj.val("0");
        }
        else if (statusObj.val() == "Completed") {
            pctCompleteObj.val("100");
        }
        else if (statusObj.val() != "Completed" && pctComplete >= 100) {
            pctCompleteObj.val("90");
        }
    }

    function OnAllCheckedChanged(s, e) {
        if (s.GetChecked()) {
            gridAllocation.SelectRows();
        }
        else {
            gridAllocation.UnselectRows();
        }
    }

    function key_Pressed() {
        var focused = document.activeElement;
        if (focused.tagName != "TEXTAREA") {
            if (window.event.keyCode == 13)
                return false;
        }

    }

    function checkBeforSave() {
        //new line of code for add skill into hidden field.
        var skillData = [];
        var strSkillId = new Array();

        var IsRepeatableTask = "<%= Request["RepeatableTask"]%>";
        if (IsRepeatableTask) {
            var recCount = $(".recurringcount").val();
            if ($.isNumeric(recCount) && recCount > 0 && recCount <= 100) {
                (document.getElementById('<%=recurringcount.ClientID %>')).value = recCount;
            }
        }
        if ($("#demo-input").length > 0) {
            strSkillId = $("#demo-input").val().split(',')
        }
        var tempcount = 0;
        $(".token-input-token-facebook p").each(function (index) {
            skillData.push({ id: strSkillId[tempcount], name: $(this).text() });
            tempcount++;
        });

        $("#<%= skillJson.ClientID%>").val(JSON.stringify(skillData));


        var status = $.trim($("#<%=ddlStatus.ClientID %>").val());
        var pctComplete = Number($.trim($("#<%=txtPctComplete.ClientID %>").val()));
        var returnBit = true;
        if (status == "Completed" || pctComplete >= 100) {

            var baselinePrompt = false;
            var confirmChildTaskComplete = false;
            var confirmChildTaskCompletePromt = false;
            var baselineTaskThreshold = Number("<%=baselineTaskThreshold %>");
            var childTaskCount = Number("<%=childTaskCount%>");
            if (baselineTaskThreshold && baselineTaskThreshold > 0 && baselineTaskThreshold <= childTaskCount) {
                baselinePrompt = true;
            }

            if (childTaskCount > 0) {
                confirmChildTaskComplete = true;
            }

            if (confirmChildTaskComplete) {
                confirmChildTaskCompletePromt = true;
            }

            if (confirmChildTaskCompletePromt) {
                if (!confirm("This action will also mark all child tasks as Completed. Would you like to proceed?")) {
                    returnBit = false;
                }
            }

            if (baselinePrompt && returnBit) {
                if (confirm("You are changing the status of " + childTaskCount + " tasks to Completed. Would you like to create a baseline first?")) {
                    $("#<%= hdnActionType.ClientID %>").val("BaselineAndSave");
                }
            }
        }

        if ($("#<%=pnlRelatedDocuments.ClientID%> label").length > 0) {
            var ids = "";
            $.each($("#<%=pnlRelatedDocuments.ClientID%> label"), function () {
                ids += $(this).attr('docId') + ";~";
            });
            if (ids != "") {
                ids = ids.substring(0, ids.length - 2);
                $("#<%=hdnDocIds.ClientID%>").val(ids);
            }
        }
        if (!Page_IsValid)
            aspxAssignToLoading.Show();
    }

    function ShowWatingPopup(obj) {

        if ($(obj).attr("class") == "aspNetDisabled") {
            return false;
        }

        <%--var count = parseInt("<%= rAssignToPct.Items.Count %>");
        $("#<%=hdnReptRowcount.ClientID%>").val(count + 1);
        aspxAssignToLoading.Show();--%> // Postback hides automatically, so no need to call Hide() later

        <%--var counnt = $("#<%=rAssignToPct.ClientID%>").find('tr:gt(0)').length;
        var count = parseInt("<%= rAssignToPct.Items.Count %>");--%>
        <%--//    $("#<%=hdnReptRowcount.ClientID%>").val(count + 1);--%>
        //aspxAssignToLoading.Show(); // Postback hides automatically, so no need to call Hide() later
        pnlAssignToPct.PerformCallback();

    }

    function confirmBeforeDelete(s, e) {

        var type = "<%= taskType %>";
        var message = "";

        if (type.toLowerCase() == "ticket") {

            message = "Are you sure you want to delete this ticket and its mapping?";
        }
        else {
            message = "Are you sure you want to delete this task?";

        }

        if (confirm(message)) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }

    }

    function confirmBeforeCancel(s, e) {
        var type = "<%= taskType %>";
        var message = "";
        if (type.toLowerCase() == "task") {
            message = "Are you sure you want to cancel this task?";
        }
        if (!confirm(message)) {
            e.processOnServer = false;
        }
    }

    function AdjustStartByPredecessorDueDate(dueDate) {
        var originalStartDateRaw = dtcStartDate.date;               //$(".startDateEdit").val(); 
        var originalEndDateRaw = dtcDueDate.date;               //$(".endDateEdit").val()

        var originalStartDate = new Date(originalStartDateRaw);
        if (originalStartDate > dueDate)
            return; // Only change dates if we need to move start date forward!

        var newStartDate = (dueDate.getMonth() + 1) + "/" + dueDate.getDate() + "/" + dueDate.getFullYear();
        var newStartDateRaw = new Date(newStartDate);
        var ticketId = '<%=projectPublicID%>';
        var taskid = parseInt('<%=taskID%>');
        var moduleName = '<%=ModuleName%>';

        var jsonObj = [];
        var item = {}
        item["startDateRaw"] = originalStartDateRaw;
        item["endDateRaw"] = originalEndDateRaw;
        item["newStartDateRaw"] = newStartDateRaw;
        item["addOneMoreDay"] = "true";
        item["taskid"] = taskid;
        item["ticketId"] = ticketId;
        item["moduleName"] = moduleName;

        jsonObj.push(item);

        var paramsInJson = JSON.stringify(jsonObj[0]);
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperURL %>/GetNextWorkingDateForTasks",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    if ($(".endDateEdit")) {
                        dtcDueDate.SetDate(new Date(resultJson.enddate));     //$(".endDateEdit").val(resultJson.enddate);
                        $("#<%=hdnDueDate.ClientID%>").val(resultJson.enddate);
                    }
                    if ($(".startDateEdit")) {
                        dtcStartDate.SetDate(new Date(resultJson.startdate));     //$(".startDateEdit").val(resultJson.startdate);
                        $("#<%=hdnStartDate.ClientID%>").val(resultJson.startdate);
                    }
                    $(".estimatedhours").val(resultJson.workinghours);
                    lastStartDate = dtcStartDate.date;        //$(".startDateEdit").val();
                }
                else {

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function SetStartDateAndDuration(node) {
        var chksprint = $('.cbSprints').attr('checked') ? true : false;

        if (chksprint == false) {
            var originalStartDateRaw = dtcStartDate.date;           //$(".startDateEdit").val();
            var originalEndDateRaw = dtcDueDate.date;               //$(".endDateEdit").val()

            var newStartDate = null;
            var ctstartdate = node.treeView["cp" + node.name].split('/');
            var selectedStartDate = new Date(ctstartdate);

            if (newStartDate == null || newStartDate < selectedStartDate) {
                newStartDate = selectedStartDate;
            }
            //originalStartDateRaw.
            newStartDateRaw = (newStartDate.getMonth() + 1) + "/" + newStartDate.getDate() + "/" + newStartDate.getFullYear();
            var paramsInJson = '{' + '"startDateRaw":"' + originalStartDateRaw + '","endDateRaw":"' + originalEndDateRaw + '","newStartDateRaw":"' + newStartDateRaw + '","addOneMoreDay":"true"}';

            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperURL %>/GetNextWorkingDate",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        if ($(".endDateEdit")) {
                            dtcDueDate.SetDate(new Date(resultJson.enddate));     //$(".endDateEdit").val(resultJson.enddate);
                        }
                        if ($(".startDateEdit")) {
                            dtcStartDate.SetDate(new Date(resultJson.startdate));     //$(".startDateEdit").val(resultJson.startdate);
                        }

                        $(".estimatedhours").val(resultJson.workinghours);
                        lastStartDate = dtcStartDate.date;
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }

    var prevId;
    var lastStartDate = "";
    $(function () {
        $(".predecessorEdit input").bind("click", function () { SetStartDateAndDuration(); })

        if (typeof (dtcStartDate) != "undefined")
            lastStartDate = dtcStartDate.date;

        showAssignRows();

        try {
            var mileStoneObj = $(".cbMilestone").find('input')
            if (mileStoneObj != undefined)
                hideShowMilestoneStages(mileStoneObj);

        }
        catch { }

    });

    function showAssignRows() {
        $(".dataAssignToPct").each(function (i) {

            var trUserAllocation = $(this);
            if ($(this).index() != 1) {

                var tdUserAllocation = $(this).find("td:first");
                var spans = $(tdUserAllocation).find("span table table span:last");
                $(spans).each(function () {

                    if ($(this).html() != "") {

                        trUserAllocation.css('display', '');
                    }
                });
            }
            else
                trUserAllocation.css('display', '');

        });

        if ($('.dataAssignToPct:hidden').length == 0)
            $('.assigntopct').prop("disabled", true);
        else
            $('.assigntopct').prop("disabled", false);
    }

    function OpenTicketWindow(PrmThis) {
        var ticketurl = $(PrmThis).attr("TicketUrl");
        var ticketId = $("#<%=hdnTicketId.ClientID%>").val();
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = '';
        window.parent.UgitOpenPopupDialog(ticketurl, param, ticketId, '90', '90', 0, escape(requestUrl));
    }

    function OpenTicketPicker() {
        var url = '<%=ticketPickerUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', 'Select Module to Pick Any Ticket', '95', '95', 0);
    }

    function hideShowMilestoneStages(object) {
        var milestoneStageDiv = $(".milestoneStageDiv");
        if ($(object).prop("checked")) {
            milestoneStageDiv.show();
        }
        else {
            milestoneStageDiv.hide();
        }
    }

   <%-- function showDescriptionToEdit(obj) {
        //$(".txtDescription").show();
        $("#<%=txtDescription.ClientID%>").show();
        $(obj).hide();
        $(".lbDescription").hide();
    }--%>

    function showPredecessorsToEdit(obj) {

        //$(".lbPredecessor").hide();
        $('#<%=lbPredecessor.ClientID%>').hide();
        $(obj).hide();
        $(".divPredecessors").show();

    }

    function AutoCalculateDueDate() {
        var totalworkingHours = 0;
        var totalAssignToPct = 0;
        $(".dataAssignToPct").each(function (i) {

            var trUserAllocation = $(this);
            var tdUserAllocation = $(this).find("td:first");
            var spans = $(tdUserAllocation).find("span table table span:last");
            var percentageValue = $(this).find(".edit-task-allocation").val();
            if (percentageValue != "")
                totalAssignToPct += parseInt(percentageValue);
            else
                totalAssignToPct += 100;
        });

        var originalStartDateRaw = new Date(dtcStartDate.date);
        var noofworkingHours = $(".estimatedhours").val();
        if (noofworkingHours > 0 && totalAssignToPct > 0) {
            var actualworkingHours = (noofworkingHours / totalAssignToPct) * 100;
            var noofWorkingDays = parseInt(actualworkingHours / 8);
        }

        if (originalStartDateRaw != "" && noofworkingHours != "" && totalAssignToPct > 0 && noofWorkingDays > 0) {
            var paramsInJson = '{' + '"startDate":"' + originalStartDateRaw.toLocaleDateString() + '","noOfWorkingDays":"' + noofWorkingDays + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperURL %>/GetEndDateByWorkingDays",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        dtcDueDate.SetValue(new Date(resultJson.enddate))
                        //$(".endDateEdit").val(resultJson.enddate);
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);
                }
            });
        }
    }

    function ShowUserAllocationDetailsBySkills() {

        var skilldata = "";
        $(".token-input-token-facebook p").each(function (index) {
            if (skilldata != "")
                skilldata = skilldata + ","
            skilldata = skilldata + $(this).text();
        });

        $("#<%= hdnSkillText.ClientID%>").val(skilldata);
    }

    function resolveFractionHour(workHour) {

        var workHourNumber = Number(workHour);

        var newTotal = 0;
        if (Number(workHour)) {
            var hours = Number(workHourNumber.toString().split(".")[0]);
            var mins = (workHourNumber * 60) % 60;

            if (mins > 45) {
                mins = 60;
            }
            else if (mins > 30) {
                mins = 45;
            }
            else if (mins > 15) {
                mins = 30;
            }
            else if (mins > 0) {
                mins = 15;
            }

            newTotal = ((hours * 60) + mins) / 60;
        }

        return newTotal;
    }

    function AutoCalculateRemainingEstimateHours() {
        var estimatedHours = 0;
        if ($(".estimatedhours").length > 0)
            estimatedHours = $(".estimatedhours").val();

        var actualHours = 0;
        if ($(".actualhours").length > 0)
            actualHours = $(".actualhours").val();
        else if ($(".lbActualHours").length > 0)
            actualHours = $(".lbActualHours").text();

        var perComplete = 0;
        if ($(".pctcomplete").length > 0)
            perComplete = $(".pctcomplete").val();

        var estRemainingHrs = $(".estimatedRemaininghours");
        var estRemaining = 0;

        if (perComplete != 0 && actualHours != 0)
            estRemaining = (parseFloat((actualHours / perComplete) * 100).toFixed(1) - actualHours).toFixed(1);
        else if (estimatedHours > 0)
            estRemaining = parseFloat(estimatedHours - actualHours).toFixed(1);

        if (estRemaining > 0)
            estRemainingHrs.val(estRemaining);
        else
            estRemainingHrs.val(0);
    }


    function AutoCalculateAssignTo() {

        var totalworkingHours = 0;
        var totalAssignToPct = 0;

        var originalStartDateRaw = dtcStartDate.GetValue();  //$(".startDateEdit").val();
        var originalEndDateRaw = dtcDueDate.GetValue(); // $(".endDateEdit").val()

        if (originalStartDateRaw != "" && originalEndDateRaw != "") {
            var paramsInJson = '{' + '"startDateRaw":"' + originalStartDateRaw.toLocaleDateString() + '","endDateRaw":"' + originalEndDateRaw.toLocaleDateString() + '","newStartDateRaw":"' + originalStartDateRaw.toLocaleDateString() + '","addOneMoreDay":"false"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperURL %>/GetNextWorkingDate",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        totalworkingHours = resultJson.workinghours;
                        var noofEstiamtedHours = $(".estimatedhours").val();
                        if (totalworkingHours > 0 && noofEstiamtedHours > 0)
                            var percentageAllocationFTE = noofEstiamtedHours / totalworkingHours;
                        var totaluserAllocation = 0;
                        if (percentageAllocationFTE > 0) {
                            $(".dataAssignToPct").each(function (i) {
                                var trUserAllocation = $(this);
                                var percentageValue = $(this).find(".edit-task-allocation").val();
                                if (percentageValue != "")
                                    totaluserAllocation += parseInt(percentageValue);
                                else
                                    totaluserAllocation += 100;
                            });

                            if (totaluserAllocation > 0) {
                                $(".dataAssignToPct").each(function (i) {
                                    var trUserAllocation = $(this);
                                    var userAllocationBox = $(this).find(".edit-task-allocation");
                                    var userAllocation;
                                    userAllocation = $(this).find(".edit-task-allocation").val();
                                    if (userAllocation == "")
                                        userAllocation = "100";
                                    var newUserAllocation = ((percentageAllocationFTE * parseInt(userAllocation) * 100) / totaluserAllocation).toFixed(0);
                                    if (!isNaN(newUserAllocation))
                                        userAllocationBox.val(newUserAllocation);
                                });
                            }
                        }
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }

    function AutoCalculateEstimateHours() {

        var totalworkingHours = 0;
        var totalAssignToPct = 0;

        $(".dataAssignToPct").each(function (i) {
            var trUserAllocation = $(this);
            var tdUserAllocation = $(this).find("td:first");
            var spans = $(tdUserAllocation).find("span table table span:last");
            var percentageValue = $(this).find(".edit-task-allocation").val();
            if (percentageValue != "")
                totalAssignToPct += parseInt(percentageValue);
            else
                totalAssignToPct += 100;
        });

        var originalStartDateRaw = dtcStartDate.GetValue(); // $(".startDateEdit").val();
        var originalEndDateRaw = dtcDueDate.GetValue(); // $(".endDateEdit").val()

        if (originalStartDateRaw != "" && originalEndDateRaw != "") {
            var paramsInJson = '{' + '"startDateRaw":"' + originalStartDateRaw.toLocaleDateString() + '","endDateRaw":"' + originalEndDateRaw.toLocaleDateString() + '","newStartDateRaw":"' + originalStartDateRaw.toLocaleDateString() + '","addOneMoreDay":"false"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperURL %>/GetNextWorkingDate",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        totalworkingHours = resultJson.workinghours;

                        if (totalworkingHours > 0) {
                            if (totalAssignToPct > 0)
                                $(".estimatedhours").val(((totalAssignToPct * totalworkingHours) / 100).toFixed(1));
                            else
                                $(".estimatedhours").val(totalworkingHours);
                        }
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }

    function changeRequestType() {
        var moduleid = ddlModuleDetail.GetValue();
        pnlRequestTypeCustom.PerformCallback('ValueChanged~' + moduleid);
    }

    function callRejectRequest() {
        commentsRejectPopup.Show();
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, false, returnUrl);
    }

    function deleteEmptyRecord(item, index) {
        //debugger;
        if (confirm('Are you sure you want to remove this Resource?')) {
            var userclientObject = $('peAssignedToPct_' + index + 'LookupSearchValue');
            if (typeof userclientObject !== "undefined")
                pnlAssignToPct.PerformCallback('Delete~' + index);
        }
    }

    function validatePctAllocation(item) {
        var val = $("#" + item).val();
        if (val.trim() != '') {
            if (val < 0 || val > 100 || !Number(val)) {
                alert('Allocation % must be between 1 and 100');
                $("#" + item).focus();
                return false;
            }
        }
    }

    $(function () {
        $("#<%=rblTaskBehaviour.ClientID%>").change(function () {

            if ($("#<%=rblTaskBehaviour.ClientID%> input:checked").val() == "Ticket") {
                if ($("#<%=hdnTicketId.ClientID%>").val() != "") {
                    $("#<%=trTicketReadOnly.ClientID%>").show();
                    $("#<%=trTicket.ClientID%>").hide();
                    $("#<%=trTitle.ClientID%>").hide();
                }
                else {
                    $("#<%=trTicket.ClientID%>").show();
                    $("#<%=trTicketReadOnly.ClientID%>").hide();
                    $("#<%=trTitle.ClientID%>").show();
                }
            }
            else {
                $("#<%=trTicket.ClientID%>").hide();
                $("#<%=trTicketReadOnly.ClientID%>").hide();
                $("#<%=trTitle.ClientID%>").show();
            }
        })
    })

    function hideShowSprints(object) {
        //debugger
        if ($(object).prop("checked")) {
            if (confirm("NOTE: Linking this task to a sprint will remove any predecessors and tie all the date and hour fields to those from the sprint.")) {
                return true;
            }
            else {
                $(object).prop('checked', false);;
                return false;
            }
        }
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function holdTask_click(s, e) {
        commentsHoldPopup.Show();
    }

    function unholdTask_click(s, e) {
        commentsUnHoldPopup.Show();
    }

    function popupMenuActionMenuItemClick(s, e) {
        if (e.item.name == "DeleteTask") {
            lnkDelete.DoClick();
        }
        else if (e.item.name == "CancelTask") {
            lnkCancel.DoClick();
        }
        else if (e.item.name == "DeleteMappingTask") {
            btDeleteMappingTask.DoClick();
        }
        else if (e.item.name == "Hold") {
            aspxHoldTask.DoClick();
        }
        else if (e.item.name == "EditHold") {
            commentsHoldPopup.Show();
        }
        else if (e.item.name == "UnHold") {
            aspxUnholdTask.DoClick();
        }
        else if (e.item.name == "UncancelTask") {
            lnkUncancelTask.DoClick();
        }
    }

    function DateChanged() {
        //if (dtcStartDate.date == null || dtcDueDate.date == null) {
        //    dtcStartDate.SetDate(null);
        //    dtcDueDate.SetDate(null);
        //}
    }
</script>
<dx:ASPxLoadingPanel ID="aspxAssignToLoading" ClientInstanceName="aspxAssignToLoading" Modal="True" runat="server" Text="Please Wait .."></dx:ASPxLoadingPanel>
<div id="divOverlay" class="overlay"></div>
<dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
<asp:HiddenField ID="fileAttchmentId" runat="server" />

<div class="fetch-popupParent pt-3">
    <div class="row" runat="server" id="taskForm">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <fieldset id="Fieldset1" class="mb-2">
                <legend>General</legend>
                <div>
                    <div class="row" id="trParent" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <label for="usr">Parent</label>
                                <a id="anchrParentTicketId" style="text-decoration: underline;" runat="server"></a>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trTitle" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group newTask_info_title">
                                <label for="usr">Title <span class="red-star">*</span></label>
                                <asp:TextBox ID="txtTitle" CssClass="all-input form-control bg-light-blue text-left" runat="server" ValidationGroup="Task_new"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task_new" ControlToValidate="txtTitle"
                                    Display="Dynamic" CssClass="errormsg-container" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>

                                <asp:Label ID="lbTitle" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trMilestone" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="d-flex align-items-center flex-wrap">
                                <div class="pr-3">
                                    <label for="usr" class="ms-standardheader" style="display: inline-block">Link to Stage?</label>
                                    <asp:CheckBox ID="cbMilestone" runat="server" CssClass="cbMilestone" onclick="hideShowMilestoneStages(this)" />
                                </div>
                                <div id="milestoneStageDiv" runat="server" style="display: none;" class="milestoneStageDiv col-md-4 col-sm-4 col-xs-12 px-0">
                                    <asp:DropDownList ID="ddlProjectStages" runat="server" CssClass="aspxDropDownList itsmDropDownList" Width="100%"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trType" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <label for="usr" style="margin-top: 7px;">Task Type</label>
                                <asp:DropDownList ID="ddlTypes" runat="server" AutoPostBack="true" CssClass=" itsmDropDownList aspxDropDownList"
                                    OnSelectedIndexChanged="ddlTypes_SelectedIndexChanged">
                                    <asp:ListItem Text="Normal Task" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Account Task" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Access Task" Value="2"></asp:ListItem>
                                </asp:DropDownList>

                                <div id="tdAccountTask" class="row" style="display: none; padding-top: 15px; padding-bottom: 15px;" runat="server">
                                    <div class="col-md-3 col-sm-3 col-xs-12 noPadding crm-checkWrap">
                                        <asp:CheckBox ID="chkAutoFillRequestor" Visible="false" runat="server"
                                            Text="Auto Fill Requestor" TextAlign="Right" />
                                    </div>
                                    <div class="col-md-3 col-sm-3 col-xs-12 noPadding crm-checkWrap">
                                        <asp:CheckBox ID="chkAutoUserCreation" Visible="false" TextAlign="Right" runat="server" Text="Automatic User Creation" />
                                    </div>
                                </div>
                            </div>
                            <div id="tdApplicationAccess" style="display: none; padding-bottom: 15px;" runat="server">
                                <label for="usr" style="margin-top: 7px;">Question:</label>
                                <asp:DropDownList ID="ddlApplicationAccess" AutoPostBack="true" runat="server" CssClass=" itsmDropDownList aspxDropDownList"
                                    OnSelectedIndexChanged="ddlApplicationAccess_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                        </div>
                    </div>
                    <div class="row" id="trApplicationAccess" runat="server" visible="false" style="padding-bottom: 15px;">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <label for="usr" style="margin-top: 7px;">Application</label>
                                <dx:ASPxGridLookup ID="GridLookupApplicationQuestion" OnLoad="GridLookupApplicationQuestion_Load" runat="server" Width="100%"
                                    SelectionMode="Single" ClientInstanceName="GridLookupApplicationQuestion" AutoPostBack="false" CssClass="aspxGridLookUp-dropDown"
                                    KeyFieldName="ID" TextFormatString="{0}" MultiTextSeparator=", ">
                                    <Columns>
                                        <dx:GridViewDataColumn FieldName="Title" Settings-AllowAutoFilter="False" />
                                        <dx:GridViewDataColumn FieldName="ID" Visible="false"></dx:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties>
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    </GridViewProperties>
                                </dx:ASPxGridLookup>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trTicketReadOnly" runat="server" style="display: none;">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down" style="margin-bottom: 15px;">
                                <label for="usr" class="ms-standardheader" style="display: inline-block">Linked Ticket</label>
                                <asp:Label ID="lblTicketReadOnly" runat="server" Style="font-weight: bold; text-decoration: underline; cursor: pointer;" onclick="javascript:OpenTicketWindow(this);"></asp:Label>
                                <img src="/Content/Images/edit-icon.png" runat="server" alt="edit" id="img1" style="cursor: pointer; vertical-align: bottom;" onclick="OpenTicketPicker()" />
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trTaskBehaviour" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <label for="usr" style="margin-top: 5px;">Task Type</label>
                                <asp:RadioButtonList ID="rblTaskBehaviour" CssClass="tskbehaviour-opt" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Text="<i><img src='/Content/images/ittask.png'/></i><b>Action</b>" Value="Action"></asp:ListItem>
                                    <asp:ListItem Text="<i><img src='/Content/images/milestone_icon.png'/></i><b>Milestone</b>" Value="Milestone"></asp:ListItem>
                                    <asp:ListItem Text="<i><img src='/Content/images/document_down.png'/></i><b>Deliverable</b>" Value="Deliverable"></asp:ListItem>
                                    <asp:ListItem Text="<i><img src='/Content/images/document_up.png'/></i><b>Receivable</b>" Value="Receivable"></asp:ListItem>
                                    <asp:ListItem Text="<i><img src='/Content/images/document_up.png'/></i><b>Ticket</b>" Value="Ticket"></asp:ListItem>
                                </asp:RadioButtonList>

                            </div>
                        </div>
                    </div>

                    <div class="row" id="trTicket" runat="server" style="display: none;">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down" style="margin-bottom: 15px;">
                                <label for="usr" class="ms-standardheader" style="display: inline-block">Linked Ticket</label>
                                <asp:Label ID="lblTicket" runat="server" Style="font-weight: bold; text-decoration: underline; cursor: pointer;" onclick="javascript:OpenTicketWindow(this);"></asp:Label>
                                <img src="/Content/Images/edit-icon.png" runat="server" alt="edit" id="imgEdit" style="cursor: pointer; vertical-align: bottom;" onclick="OpenTicketPicker()" />
                                <input type="hidden" id="hdnTicketId" runat="server" />
                            </div>
                        </div>
                    </div>

                    <div class="row pt-1">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                        <div id="trCritical" runat="server" visible="false">
                            <div class="form-group">
                                <div class="form-group all-drop-down all-select content-bottom">
                                <label for="usr" class="ms-standardheader" style="display: inline-block">Critical</label>
                                <asp:CheckBox ID="chkCritical" runat="server" CssClass="cbMilestone" />
                            </div>
                            </div>
                            </div>
                        </div>
                    </div>
                   
                    <div class="row" id="trSprints" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="d-flex align-items-center flex-wrap">
                                <div class="pr-3">
                                    <label for="usr" class="ms-standardheader" style="display: inline-block">Link to Sprint?</label>
                                    <asp:CheckBox ID="cbSprints" runat="server" onclick="hideShowSprints(this)" AutoPostBack="true" CssClass="cbSprints" />
                                </div>
                                <div id="sprintsDiv" runat="server" class="col-md-4 col-sm-4 col-xs-12 px-0">
                                    <asp:DropDownList ID="ddlSprints" runat="server" CssClass="aspxDropDownList itsmDropDownList" Width="100%"></asp:DropDownList>
                                </div>
                            </div>
                            </div>
                    </div>

                  
                    <div class="row" id="trNote" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <label>Task Description</label>
                                <%--<img src="/Content/images/editNewIcon.png" id="editDescription" class="edit_task_img imageStyle" runat="server" visible="false" onclick="showDescriptionToEdit(this);" />--%>
                                <asp:TextBox CssClass="all-input form-control bg-light-blue text-left" Rows="1" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <asp:Label ID="lbDescription" CssClass="lbDescription svcEditTask_fieldLable" runat="server" Visible="false" Style="display: none"></asp:Label>
                            </div>
                        </div>
                    </div>
                      <div class="row pt-1" id="trPredecessors" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group" style="margin-bottom: 15px;">
                                <label for="usr" class="ms-standardheader" style="display: inline-block">Task Predecessors</label>
                                <img src="/Content/images/editNewIcon.png" id="editPredecessor" class="edit_task_img imageStyle imgtoggle" runat="server" visible="false" onclick="showPredecessorsToEdit(this);" />
                                <asp:Label ID="lblPredecessorError" CssClass="createTicket_errorMsg" runat="server"></asp:Label>
                                <asp:Label ID="lbPredecessor" CssClass="svcEditTask_fieldLable" runat="server" Visible="false"></asp:Label>
                                  <div id="dvpheditcontrol" runat="server" class="divPredecessors" style="width: 100%;">
                                    <div class="checkbox chkbox-1">
                                        <asp:PlaceHolder runat="server" ID="pheditcontrol" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="crm-checkWrap" id="trApprovalRequired" runat="server" visible="false" style="padding-bottom: 20px;">
                                <asp:CheckBox ID="chkApprovalRequired" runat="server" AutoPostBack="true" Text="Approval Required" TextAlign="Right"
                                    OnCheckedChanged="chkApprovalRequired_CheckedChanged" />
                                <asp:Label ID="lbApprovalRequired" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trModuleStep" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <div class="usr-label-wrap">
                                    <label for="usr">Module Step</label>
                                </div>
                                <div class="edit-btn-wrap">
                                    <asp:DropDownList runat="server" ID="ddlModuleStep"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </fieldset>
            <div>
                <div class="row content-wrap">
                    <asp:Panel runat="server" ID="taskForm1">
                        <fieldset id="fsAccountTask" runat="server" visible="false">
                            <legend>User Account:</legend>
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                <tr id="trUserName" runat="server" visible="true">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Login Name<b style="color: red;">*</b>
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <div>
                                            <asp:Label ID="lblErrorMsg" runat="server" Style="color: red; font-weight: bold; padding: 5px 0px; float: left; width: 100%;"></asp:Label>
                                        </div>
                                        <div>
                                            <asp:Label ID="lbUserName" runat="server" CssClass="lbusername"></asp:Label>
                                        </div>
                                        <%--<ugit:UserValueBox ID="pplUserName" maximumheight="30" runat="server" Width="250px"  />--%>
                                        <asp:TextBox ID="txtUsername" runat="server" Width="250px"></asp:TextBox>
                                        <div>
                                            <asp:CustomValidator ID="cvPPLUserName" runat="server" CssClass="errormsg-container" ErrorMessage="Please enter user name." ValidationGroup="Task" Display="Dynamic" ControlToValidate="txtTitle"
                                                OnServerValidate="cvPPLUserName_ServerValidate"></asp:CustomValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset id="fsApproval1" runat="server" visible="false">
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">

                                <tr id="trApproval1" runat="server">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Pending Approver(s):
                                        </h3>
                                    </td>
                                    <td class="ms-formbody" colspan="2">
                                        <div style="float: left">
                                            <ugit:UserValueBox ID="peApprover1" runat="server" />
                                        </div>
                                        <div style="float: left">
                                            <dx:ASPxButton Visible="false" ID="lnkbtnAssignApprover1" CssClass="ugit-button quick-ticketbt" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover"
                                                OnClick="lnkbtnAssignApprover_Click" runat="server" Text="Assign Approver" ToolTip="Assign Approver">
                                            </dx:ASPxButton>
                                        </div>
                                        <div>
                                            <asp:Label ID="lbApprover1" runat="server" Visible="false"></asp:Label>
                                        </div>

                                    </td>
                                </tr>



                                <tr id="trbtnApproveReject1" runat="server" visible="false">
                                    <td class="ms-formlabel">&nbsp;
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:LinkButton ID="btnApproveApp1" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Accept&nbsp;&nbsp;" OnClick="BtApproveTask_Click"
                                            ToolTip="Approve" Style="float: left;">
                                <span class="button-bg">
                                     <b style="float: left; font-weight: normal;">
                                     Approve</b>
                                     <i style="float: left; position: relative; top: -2px;left:2px">
                                    <img src="/Content/ButtonImages/approve.png"  style="border:none;" title="" alt=""/></i> 
                                </span>
                                        </asp:LinkButton>
                                        &nbsp; &nbsp; &nbsp; &nbsp;

                          <asp:LinkButton ID="btnRejectApp1" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Reject&nbsp;&nbsp;"
                              ToolTip="Reject" Style="float: left;">
                              <span class="button-bg">
                                  <i style="float: left; position: relative; top: -2px;left:2px">
                                <img src="/Content/ButtonImages/reject.png"  style="border:none;" title="" alt=""/></i>
                                <b style="float: left; font-weight: normal;">
                                Reject</b>
                              </span>
                          </asp:LinkButton>
                                    </td>
                                </tr>


                            </table>
                        </fieldset>
                        <fieldset id="fsScheduling1" runat="server" visible="false">
                            <legend>Add/Update User access:</legend>
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                width="100%">
                                <tr>
                                    <td id="tdDates1" runat="server" colspan="2">
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="ms-formlabel" id="tdlblDueDate1" runat="server" visible="false">
                                                    <h3 class="ms-standardheader">Due Date
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody" id="tdDueDate1" runat="server" visible="false">

                                                    <asp:HyperLink ID="btProposeNewDate" Visible="false" NavigateUrl="javascript:void(0);" runat="server" Style="float: left;">
                            <span class="button-bg">
                             <b style="float: left; font-weight: normal;">
                             Propose New Date</b>
                             <i style="float: left; position: relative; top: -1px;left:3px">
                            <img src="/Content/images/calendar.gif"  style="border:none;" title="" alt=""/></i> 
                             </span>
                                                    </asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr id="trProposedDueDate" runat="server" visible="false">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Proposed Due Date
                                        </h3>
                                    </td>
                                    <td class="ms-formbody" colspan="5">
                                        <asp:Label ID="lbProposedDate" runat="server" CssClass="proposeddatelb" Visible="false"></asp:Label>

                                        <asp:HyperLink ID="btApprove" Visible="false" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Accept&nbsp;&nbsp;"
                                            ToolTip="Accept" Style="float: left;">
                        <span class="button-bg">
                         <b style="float: left; font-weight: normal;">
                         Accept</b>
                         <i style="float: left; position: relative; top: -2px;left:2px">
                        <img src="/Content/ButtonImages/approve.png"  style="border:none;" title="" alt=""/></i> 
                           </span>
                                        </asp:HyperLink>
                                        <asp:HyperLink ID="btReject" Visible="false" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Reject&nbsp;&nbsp;"
                                            ToolTip="Reject" Style="float: left;">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                            Reject</b>
                            <i style="float: left; position: relative; top: -2px;left:2px">
                        <img src="/Content/ButtonImages/reject.png"  style="border:none;" title="" alt=""/></i> 
                            </span>
                                        </asp:HyperLink>

                                    </td>
                                </tr>

                            </table>
                            <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                width="100%">

                                <tr id="trRemoveUser" runat="server" visible="false">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Add/Update User access
                                        </h3>
                                    </td>
                                    <td class="ms-formbody">
                                        <asp:Panel ID="pnlRemoveUserAccess" runat="server"></asp:Panel>
                                    </td>
                                </tr>


                                <tr id="trAttachment1" runat="server" visible="false">
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader">Attachments</h3>
                                    </td>
                                    <td class="ms-formbody">

                                        <ugit:FileUploadControl ID="ugitupAttachments1" runat="server" />
                                        <asp:Panel ID="pAttachmentContainer1" runat="server" CssClass="attachment-container">
                                            <div class="attachmentform">
                                                <span style="display: none;">
                                                    <asp:DropDownList ID="ddlExistingAttc1" runat="server"></asp:DropDownList>
                                                    <asp:TextBox ID="txtDeleteFiles11" runat="server"></asp:TextBox>
                                                </span>
                                                <asp:Panel ID="pAttachment1" runat="server" CssClass="oldattachment">
                                                </asp:Panel>
                                                <asp:Panel ID="pNewAttachment1" runat="server" CssClass="newattachment">
                                                </asp:Panel>
                                                <asp:Panel ID="pAddattachment1" runat="server" CssClass="addattachment">
                                                    <%--<a href="javascript:void(0);" onclick="addAttachment()">Add More Files</a>--%>
                                                </asp:Panel>

                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pAddDocuments" runat="server" Style="display: none;">
                                            <label runat="server" id="lblPortalError" style="display: none; cursor: pointer;"></label>
                                            <div style="padding-top: 10px;" id="divPortal" runat="server">
                                                <%--<label style="font-weight: bold;" id="lblPortalMessage">Click on the button below to create the document portal:</label><br /><br />--%>
                                                <a id="btnCreatePortal" style="padding-top: 10px;" text="&nbsp;&nbsp;Create Document Portal&nbsp;&nbsp;" tooltip="Create Document Portal">
                                                    <span class="button-bg">
                                                        <b style="float: left; font-weight: normal;">Create Document Portal</b>
                                                        <i style="float: left; position: relative; left: 2px">
                                                            <img src="/Content/images/add_icon.png" style="border: none;" title="" alt="" />
                                                        </i>
                                                    </span>
                                                </a>
                                                <span style="margin-left: 10px;">
                                                    <input type="checkbox" id="chkFolders" checked="checked" />Create Default Folders
                                                </span>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="pDocuments" runat="server" Style="display: none;">
                                            <a id="aDocuments" onclick="OpenDocumentPicker()" style="padding-top: 10px;" runat="server">
                                                <span class="button-bg">
                                                    <b style="float: left; font-weight: normal;">Link To Existing Document(s)</b>
                                                    <i style="float: left; position: relative; left: 2px">
                                                        <img src="/Content/images/add_icon.png" style="border: none;" title="" alt="" />
                                                    </i>
                                                </span>
                                            </a>
                                            <a id="aUploadDocuments" onclick="UploadDocPage()" style="padding-top: 10px;" runat="server">
                                                <span class="button-bg">
                                                    <b style="float: left; font-weight: normal;">Upload & Link To Document(s)</b>
                                                    <i style="float: left; position: relative; left: 2px">
                                                        <img src="/Content/images/add_icon.png" style="border: none;" title="" alt="" />
                                                    </i>
                                                </span>
                                            </a>
                                            <%--<a href="javascript:void(0)" id="aDocuments" runat="server">Link to Document(s)</a>--%>
                                            <%--   <a href="javascript:void(0)" id="aUploadDocuments" runat="server">Upload Document(s)</a>--%>
                                            <br />
                                            <asp:HiddenField ID="hdnDocIds" runat="server" />
                                            <asp:HiddenField ID="hdnSelectedFolderID" runat="server" />
                                            <asp:HiddenField ID="pathValue" runat="server" />
                                            <asp:HiddenField ID="hdnResponse" runat="server" />
                                            <asp:HiddenField ID="hdnData" runat="server" />

                                            <input type="text" value="" style="display: none;" id="txtWorkFlowIcon" onchange="txtWorkFlowIcon_Change()" />
                                            <asp:Panel ID="pnlRelatedDocuments" runat="server" Style="width: 100%; float: left;">
                                            </asp:Panel>
                                            <div id="ConfirmPopup" style="display: none;">
                                                <div id="messageDiv" style="height: 100px; padding-top: 10px; padding-left: 10px; padding-right: 10px; font-weight: bold;">
                                                    <label id="lblConfirmationMessage"></label>
                                                </div>

                                                <div id="buttonDiv" style="float: right; padding-right: 5px;">
                                                    <%--<input type="button" id="btnYes" style="width: 75px;" value="Yes" index="" onclick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.Yes, this.getAttribute('index') + ';#Yes'); return false;" />
                                        <%--<input type="button" style="width:75px;" value="No" onclick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.No, 'No'); return false;"/>--%>
                                                    <%--<input type="button" style="width: 75px;" value="Cancel" onclick="SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.cancel, 'Cancel'); return false;" />--%>
                                                    <input type="submit" value="" id="btnSubmit" style="display: none;" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

                                            function txtWorkFlowIcon_Change() {

                                                if ($("#txtWorkFlowIcon").val() != "") {
                                                    var arr = $("#txtWorkFlowIcon").val().split(";~");
                                                    $("img[AltText]", $("label[docid='" + arr[0] + "']")).attr('src', '/Content/images/DocumentLibraryManagement/StopWorkflow.png');
                                                    $("img[AltText]", $("label[docid='" + arr[0] + "']")).attr('title', 'Stop Review');
                                                    $("img[AltText]", $("label[docid='" + arr[0] + "']")).attr('AltText', arr[1]);
                                                    $("a[status]", $("label[docid='" + arr[0] + "']")).text("(" + arr[1] + ")");
                                                }
                                            }
                                            function ConfirmDeleteDocs(docId, fileName, PrmThis) {
                                                if (confirm("Are you sure you want to delete the " + fileName + " document relationship with the task?")) {
                                                    $(PrmThis).parent().remove();
                                                }
                                                else {
                                                    e.stopImmediatePropagation();
                                                    return false;
                                                }
                                                return false;
                                            }
                                            function StartStopWorkFlow(action, version, PrmThis) {


                                                if ($(PrmThis).attr('title') == "Start Review") {
                                                       // var url = "<%=updateReviewersUrl %>" + "&DocumentID=" + $(PrmThis).parent().attr('docid') + "&Version=" + version + "&IsPMM=true&controlId=txtWorkFlowIcon";
                                                    UpdateReviwersPopUp(url);
                                                }
                                                else {
                                                    if ((action != "Approved" && action != "Draft") || ($(PrmThis).attr("AltText") != "Approved" && $(PrmThis).attr("AltText") != "Draft")) {
                                                        var qData = { "documentID": $(PrmThis).parent().attr('docid'), "version": version };
                                                        // var qData = '{' + '"documentID":"' + $(PrmThis).parent().attr('docid') + '" ,"version":"' + version + '"}';
                                                        $("#<%=hdnData.ClientID%>").data("jsonData", qData);
                                                        ConfirmationMessage = "Do you really want to cancel the review of this document?";
                                                        openConfirmationDialog(ConfirmationMessage, 1);
                                                        return false;
                                                    }
                                                    else {

                                                        return false;
                                                    }
                                                }
                                                e.preventDefault();
                                                //return true;
                                            }
                                            function openConfirmationDialog(ConfirmationMessage, index) {
                                                var cloneModalContent = document.createElement('div');
                                                $("#lblConfirmationMessage").html(ConfirmationMessage);
                                                $("#btnYes").attr("index", index);


                                                cloneModalContent.innerHTML = document.getElementById('ConfirmPopup').innerHTML;

                                                var options = {
                                                    html: cloneModalContent,  // ID of the HTML tag
                                                    // or HTML content to be displayed in modal dialog
                                                    width: 375,
                                                    height: 150,
                                                    title: "Please Confirm",
                                                    dialogReturnValueCallback: onDialogClose,  // custom callback function
                                                    allowMaximize: false,
                                                    showClose: true
                                                };
                                                //SP.UI.ModalDialog.showModalDialog(options);
                                            }
                                            //Results displayed if 'OK' or 'Cancel' button is clicked if the html content has 'OK' and 'Cancel' buttons
                                            function onDialogClose(dialogResult, returnValue) {
                                                if (returnValue.indexOf('Yes') > 0) {
                                                    var index = returnValue.split(";#")[0];
                                                    $("#<%=hdnResponse.ClientID%>").val('Yes');
                                                    UpdateWorkFlow();
                                                    return true;
                                                }
                                                if (returnValue == 'No') {
                                                    $("#<%=hdnResponse.ClientID%>").val('No');
                                                    return false;
                                                }
                                                if (returnValue == 'Cancel') {
                                                    $("#<%=hdnResponse.ClientID%>").val('');
                                                    return false;
                                                }
                                            }

                                            function UpdateWorkFlow() {
                                                var jsonData = $("#<%=hdnData.ClientID%>").data("jsonData");
                                                var docId = jsonData.documentID;
                                                //  var qData = '{' + '"documentID":"' + docId + '" ,"version":"' + jsonData.version + '"}';

                                                $.ajax({
                                                    type: "POST",
                                                    url: "<%=ajaxHelperURL %>/StopWorkFlow",
                                                    data: JSON.stringify(jsonData),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (message) {
                                                        $("img[AltText]", $("label[docid='" + docId + "']")).attr('src', '/Content/images/DocumentLibraryManagement/StartWorkflow.png');
                                                        $("img[AltText]", $("label[docid='" + docId + "']")).attr('title', 'Start Review');
                                                        $("a[status]", $("label[docid='" + docId + "']")).text("(" + message.d + ")");
                                                    },
                                                    error: function (xhr, ajaxOptions, thrownError) {
                                                    }
                                                });
                                            }

                                            function UploadDocPage() {
                                                var docID = '<%=projectPublicID %>';
                                                var find = '-';
                                                var reg = new RegExp(find, 'g');
                                                docID = docID.replace(reg, '_');
                                                var selectedfolderGuid = $("#<%=hdnSelectedFolderID.ClientID%>").val();
                                                var pathValue = (document.getElementById('<%=pathValue.ClientID %>')).value;
                                                var selectedFolder = '<%=selectedFolder%>';
                                                if (docID != "") {
                                                    var sourceUrl = document.location.href;
                                                    //if (sourceUrl != "" && sourceUrl.indexOf("&source") != -1) {
                                                    //    sourceUrl = sourceUrl.substring(sourceUrl.indexOf("&source"), sourceUrl.length - 1);

                                                    var url = "<%=UploadPageUrl%>&" + "docID=" + docID + "&pathValue=" + pathValue + "&selectedfolderGuid=" + selectedfolderGuid + "&IsSelectFolder=true&ControlId=" + '<%= pnlRelatedDocuments.ClientID%>' + "&source=" + sourceUrl + "&type=projectreport";
                                                    window.parent.UgitOpenPopupDialog(url, 'selectedFolder=' + selectedFolder, docID + ' - ' + 'Upload Document', '790px', '800px', 0);
                                                    // }
                                                }

                                            }
                                            function OpenDocumentPicker() {
                                                var url = '<%=documnetPickerUrl%>';
                                                var selectedfolderGuid = $("#<%=hdnSelectedFolderID.ClientID%>").val();
                                                // set_cookie("SelectedFolderID", selectedfolderGuid, null, " ServerRelativeUrl");
                                                window.parent.UgitOpenPopupDialog(url, '', 'Pick Document', '95', '95', 0);
                                            }
                                            function OpenTicketPicker() {
                                                var url = '<%=ticketPickerUrl%>';
                                                window.parent.UgitOpenPopupDialog(url, '', 'Select Module to Pick Any Ticket', '95', '95', 0);
                                            }
                                            function addAttachment() {
                                                var uploadFiles = $(".attachment-container .fileitem");
                                                var uploadContainer = $(".attachment-container .newattachment");
                                                var addIcon = $(".attachment-container .addattachment");

                                                if (uploadFiles.length <= 5) {
                                                    if (uploadFiles.length == 4) {
                                                        addIcon.css("visibility", "hidden");
                                                    }

                                                    uploadContainer.append('<div class="fileitem fileupload"><span><input type="file" name="pAttachment1" /></span><label onclick="removeAttachment(this)"><img src="/Content/images/delete-icon.png" alt="Delete"/></label></div>');
                                                }
                                            }
                                            function CreatePortal() {
                                                ShowOverlay();
                                                var ticketID = '<%=projectPublicID %>';
                                                var find = '-';
                                                var reg = new RegExp(find, 'g');
                                                docID = ticketID.replace(reg, '_');
                                                var jsonData = { "IsChecked": chkFolders.checked, "moduleName": "<%=ModuleName%>", "TicketId": ticketID };
                                                $.ajax({
                                                    type: "POST",
                                                    url: "<%=ajaxHelperURL %>/CreatePortal",
                                                    data: JSON.stringify(jsonData),
                                                    contentType: "application/json; charset=utf-8",
                                                    dataType: "json",
                                                    success: function (message) {

                                                        if ($.trim(message.d) == "") {
                                                            if (chkFolders.checked) {
                                                                $("#<%=pAddDocuments.ClientID%>").hide();
                                                                $("#<%=pDocuments.ClientID%>").show();
                                                            }
                                                            else {
                                                                $("#<%=lblPortalError.ClientID%>").text("Go to document(s) tab to create Folder first for linking/uploading document(s).")
                                                                $("#<%=lblPortalError.ClientID%>").css("text-decoration", "underline");
                                                                $("#<%=pAddDocuments.ClientID%>").show();
                                                                $("#<%=lblPortalError.ClientID%>").show();
                                                                $("#<%=divPortal.ClientID%>").hide();
                                                                $("#<%=pDocuments.ClientID%>").hide();
                                                            }
                                                        }
                                                        else {
                                                            $("#<%=lblPortalError.ClientID%>").text(message.d);
                                                            $("#<%=lblPortalError.ClientID%>").show();
                                                            $("#<%=pAddDocuments.ClientID%>").show();
                                                            $("#<%=divPortal.ClientID%>").hide();
                                                            $("#<%=pDocuments.ClientID%>").hide();
                                                        }
                                                        HideOverlay();
                                                    },
                                                    error: function (xhr, ajaxOptions, thrownError) {
                                                        HideOverlay();
                                                    }
                                                });
                                            }
                                            function RemoveNotification() {
                                                $("#divOverlay").css("display", "block");
                                                return true;
                                            }
                                            var notifyId = "";
                                            function ShowOverlay() {
                                                if (notifyId == "") {
                                                    //notifyId = SP.UI.Notify.addNotification('Creating Portal...', true);
                                                    $("#divOverlay").css("display", "block");
                                                    return true;
                                                }
                                            }
                                            function HideOverlay() {
                                                //SP.UI.Notify.removeNotification(notifyId);
                                                notifyId = '';
                                                $("#divOverlay").css("display", "none");
                                            }
                                            function removeAttachment(obj) {

                                                var fileName = $(obj).find("span").text()
                                                var deleteCtr = $(".attachment-container :text");
                                                deleteCtr.val(deleteCtr.val() + "~" + fileName);

                                                var addIcon = $(".attachment-container .addattachment");
                                                $(obj).parent().remove();
                                                addIcon.css("visibility", "visible");
                                            }
                                            function openDialog(html, titleVal, source, stopRefesh) {
                                                var divContanier = document.createElement("div");
                                                divContanier.innerHTML = unescape(html).replace(/\+/g, " ").replace(/\~/g, "'");
                                                var htmlObj = $("body").append(divContanier);
                                                $(divContanier).width(1000);
                                                $(divContanier).height(700);
                                                $(divContanier).css("float", "left");
                                                var refreshParent = stopRefesh ? 0 : 1;
                                                //  $(divContanier).append("<div style='float:right;width:96%;' ><span style='float:right;margin-right:6px;padding:5px 14px;border:1px solid;' class='ugitsellinkbg '><a href='javascript:void(0);' onclick='SP.UI.ModalDialog.commonModalDialogClose(" + refreshParent + ", \"" + source + "\")'>Close</a></span></div>");
                                                var options = {
                                                    html: divContanier,
                                                    width: $(divContanier).width() + 5,
                                                    height: $(divContanier).height() + 20,
                                                    title: titleVal,
                                                    allowMaximize: false,
                                                    showClose: true,
                                                    dialogReturnValueCallback: UgitOpenHTMLPopupDialogClose
                                                };

                                                if (closePopupNotificationId != null) {
                                                    if (navigator.appName != "Microsoft Internet Explorer") {
                                                        window.stop();
                                                    }
                                                }
                                                //SP.UI.ModalDialog.showModalDialog(options);

                                                if (closePopupNotificationId != null) {
                                                    if (navigator.appName == "Microsoft Internet Explorer") {
                                                        window.document.execCommand('Stop');
                                                    }

                                                    //SP.UI.Notify.removeNotification(closePopupNotificationId);
                                                    closePopupNotificationId = null;
                                                }
                                            }

                                            function addDocuments() {
                                                var html = $("#divPopUpDocuments").html();
                                                openDialog(html, "Link to Document(s)", "", false);
                                            }



                                        </script>


                                    </td>
                                </tr>

                            </table>
                        </fieldset>

                        <asp:HiddenField ID="hdnControl" runat="server" />
                    </asp:Panel>
                </div>
            </div>
            <div class="row" id="fsApproval" runat="server" visible="false">
                <div class="col-md-12 col-sm-12 col-xs-12 section-heading">
                    <div class="section-title">
                        <span class="newTask_title">Approval</span>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="row" id="trApprovedBy" runat="server" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader"></h3>
                        </div>
                        <div class="ms-formbody">
                            <asp:Label ID="lblApproverName" runat="server" Font-Bold="true"></asp:Label>
                        </div>
                    </div>
                    <div class="row" id="trApproval" runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12">
                            <div class="form-group all-drop-down">
                                <label for="usr">Pending Approver(s)</label>
                                <div class="btn-group btn-block svcEditask_aprroverBtnWrap" id="req-by">
                                    <div class="edit_task_approveDropdown col-md-4 col-sm-4 col-xs-12 noPadding">
                                        <ugit:UserValueBox ID="peApprover" runat="server" CssClass="userValueBox-dropDown" />
                                    </div>
                                    <div class="editTask_assignApprover_btn" id="assignApprover" visible="false" runat="server">
                                        <div class="approver-btn" id="approver">
                                            <dx:ASPxButton Visible="false" ID="lnkbtnAssignApprover" CssClass="ugit-button quick-ticketbt primary-blueBtn" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover"
                                                OnClick="lnkbtnAssignApprover_Click" runat="server" Text="Assign Approver" ToolTip="Assign Approver">
                                            </dx:ASPxButton>
                                            <asp:Label ID="lbApprover" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-xs-12 approve_reject_container">
                            <div class="approve_reject_wrap" id="trbtnApproveReject" runat="server" visible="false">
                                <div class="approve-btn">
                                    <asp:LinkButton ID="btnApproveApp" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Accept&nbsp;&nbsp;" OnClick="BtApproveTask_Click"
                                        ToolTip="Approve">
                                                <span class="assignAprrover_btnName">
                                                     <b class="assignApprover_name">
                                                     Approve</b>
                                                     <i class="approve_btnWrap">
                                                    <img src="/Content/ButtonImages/approve.png"  style="border:none;" title="" alt=""/></i> 
                                                </span>
                                    </asp:LinkButton>
                                </div>
                                <div class="reject-btn">
                                    <asp:LinkButton ID="btnRejectApp" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Reject&nbsp;&nbsp;"
                                        ToolTip="Reject">
                                          <span class="assignAprrover_btnName">
                                                  <i class="approve_btnWrap">
                                                  <img src="/Content/ButtonImages/reject.png"  class="editTask_img" title="" alt=""/></i>
                                                  <b class="assignApprover_name reject_label">Reject</b>
                                          </span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div>
                            <asp:Label ID="lblTaskStatus" runat="server" CssClass="createTicket_errorMsg"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row mb-2" id="fsScheduling" runat="server">
                <fieldset id="fsScheduling_">
                    <legend>Scheduling</legend>
                    <div>
                        <div class="row" id="tdDates" runat="server">
                            <div id="trSkill" runat="server" visible="false" class="form-group col-md-4 col-sm-4 col-xs-12" >
                                <div class="all-drop-down">
                                    <label for="usr">Skills</label>
                                    <asp:Label ID="lblSkills" runat="server"></asp:Label>
                                    <div class="d-flex align-items-center">
                                        <div id="divUserSkill" class="flex-fill" >
                                            <ugit:LookUpValueBox ID="cbSkill" runat="server" FieldName="UserSkillMultiLookup" AllowNull="true" CssClass="lookupValueBox-dropown d-block" Width="100%"></ugit:LookUpValueBox>
                                        </div>
                                        <div class="ml-1">
                                            <asp:ImageButton ID="btnSkillAutoCalculate" runat="server" Style="border: none; float: right; padding-top: 4px; width: 16px;"
                                                ImageUrl="/Content/images/Autocalculator-new.png" ToolTip="Auto Calculate" OnClick="btnSkillAutoCalculate_Click"
                                                OnClientClick="return ShowUserAllocationDetailsBySkills()" />
                                        </div>
                                    </div>
                                    <dx:ASPxPopupControl ID="grdSkill" runat="server" CloseAction="CloseButton" Modal="true"
                                        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
                                        ShowFooter="false" Width="550px" Height="400px" HeaderText="Resource(s) with Skills:" ClientInstanceName="grdSkill">
                                        <ContentCollection>
                                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                                <div style="vertical-align: middle">

                                                    <ugit:ASPxGridView ID="gridAllocation" runat="server" AutoGenerateColumns="False"
                                                        OnDataBinding="gridAllocation_DataBinding"
                                                        ClientInstanceName="gridAllocation"
                                                        Width="100%" KeyFieldName="ResourceId"
                                                        SettingsText-EmptyDataRow="No Resources Available">
                                                        <%--   <ClientSideEvents CustomButtonClick="gridAllocation_CustomButtonClick" />--%>
                                                        <Columns>
                                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                    <dx:ASPxCheckBox ID="cbAllResourceCheck" runat="server" ClientInstanceName="cbAllResourceCheck" ToolTip="Select all rows">
                                                                        <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                                                                    </dx:ASPxCheckBox>
                                                                </HeaderTemplate>
                                                            </dx:GridViewCommandColumn>
                                                        </Columns>
                                                        <Styles AlternatingRow-CssClass="ugitlight1lightest">
                                                            <GroupRow Font-Bold="true"></GroupRow>
                                                            <Header Font-Bold="true"></Header>
                                                        </Styles>
                                                        <Settings GroupFormat="{1}" />
                                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                        <SettingsEditing Mode="Inline" />
                                                        <SettingsBehavior ConfirmDelete="true" AllowDragDrop="false" AutoExpandAllGroups="true" ColumnResizeMode="Disabled" />
                                                        <SettingsPopup HeaderFilter-Height="200"></SettingsPopup>
                                                    </ugit:ASPxGridView>
                                                    <br />
                                                    <asp:Label ID="lblSkillErrorMessage" runat="server" Text="Please select at least one resource." ForeColor="Red" Visible="false"></asp:Label>
                                                    <br />
                                                    <div>
                                                        <span style="float: right; padding-right: 1px;">
                                                            <dx:ASPxButton ID="btCancelResourceSkill" runat="server" ClientInstanceName="btCancelResourceSkill" Text="Cancel" Width="50px" ToolTip="Cancel"
                                                                CausesValidation="false" Style="float: right; margin-right: 1px;" CssClass="bt ugitbutton">
                                                                <ClientSideEvents Click="function(s, e) { grdSkill.Hide(); }" />
                                                            </dx:ASPxButton>
                                                        </span>
                                                        <span style="float: right; padding-right: 1px;">
                                                            <dx:ASPxButton ID="dxUpdateSkill" Text="Add" runat="server" ClientInstanceName="dxUpdateSkill" Visible="false" OnClick="dxUpdateSkill_Click"
                                                                Style="float: right; margin-right: 1px;" CssClass="bt ugitbutton">
                                                            </dx:ASPxButton>
                                                        </span>
                                                    </div>
                                                </div>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>
                                </div>
                            </div>
                            <div class="form-group newTask_info_title col-md-4 col-sm-4 col-xs-12" id="tdlblStartDate" runat="server" visible="false">
                                <label for="usr">Start Date<span class="red-star">*</span></label>
                                <div id="tdStartDate" runat="server" visible="false">
                                    <dx:ASPxDateEdit ID="dtcStartDate" runat="server" ClientInstanceName="dtcStartDate" UseMaskBehavior="false" EditFormat="Date" CssClass="CRMDueDate_inputField startDateEdit"
                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18px" ClientSideEvents-DateChanged="DateChanged">
                                    </dx:ASPxDateEdit>
                                    <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ValidationGroup="Task_new" ControlToValidate="dtcStartDate"
                                            Display="Dynamic" CssClass="errormsg-container" ErrorMessage="Please Enter Start Date."></asp:RequiredFieldValidator>                                    
                                    <asp:HiddenField ID="hdnStartDate" runat="server"></asp:HiddenField>
                                    <asp:Label ID="lbStartDate" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group all-drop-down col-md-4 col-sm-4 col-xs-12" id="tdlblDueDate" runat="server" visible="false">
                                <label for="usr">Due Date<span class="red-star">*</span></label>
                                <div class="align-items-center">
                                    <div id="tdDueDate" runat="server" visible="false">
                                        <dx:ASPxDateEdit ID="dtcDueDate" runat="server" ClientInstanceName="dtcDueDate" EditFormat="Date" UseMaskBehavior="false" CssClass="CRMDueDate_inputField endDateEdit"
                                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16px" Width="50%" ClientSideEvents-DateChanged="DateChanged">
                                        </dx:ASPxDateEdit>
                                        <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ValidationGroup="Task_new" ControlToValidate="dtcDueDate"
                                            Display="Dynamic" CssClass="errormsg-container" ErrorMessage="Please Enter Due Date."></asp:RequiredFieldValidator>
                                        <asp:Label ID="dtcDateError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                        <asp:Label ID="lbDueDate" CssClass="proposeddatelb" runat="server" Visible="false"></asp:Label>
                                        <asp:HiddenField ID="hdnDueDate" runat="server"></asp:HiddenField>

                                    </div>
                                    <div id="dueDateAutoCal" runat="server" class="ml-1">
                                        <img src="/Content/images/Autocalculator-new.png" id="imgDueDateAutoCalculater" runat="server" style="border: none; float: right; padding-top: 4px; width: 16px;"
                                            title="Auto Calculate" alt="" onclick="return AutoCalculateDueDate()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="trHours" runat="server" visible="false">
                            <div id="tdlblEstimatedHours" runat="server" visible="false" class="form-group newTask_info_title col-sm-4 col-md-4 col-xs-12">
                                <label for="usr">Estimated Hours</label>
                                <div id="tdEstimatedHours" runat="server" visible="false" class="d-flex align-items-center">
                                    <div id="txtEstimatedHourswrap" runat="server">
                                        <asp:TextBox ID="txtEstimatedHours" ValidationGroup="Task" CssClass="form-control estimatedhours" onblur="resolveFractionHour(this);"
                                            runat="server">
                                        </asp:TextBox>
                                        <asp:Label ID="lbEstimatedHours" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="ml-1">
                                        <img id="imgEstimatedHrs" runat="server" src="/Content/images/Autocalculator-new.png" style="border: none; float: right; padding-top: 4px; width: 16px;"
                                            title="Auto Calculate" alt="" onclick="return AutoCalculateEstimateHours()" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4 col-md-4 col-xs-12" id="tdlblActualHours" runat="server" visible="false">
                                <div class="form-group newTask_info_title ">
                                    <label for="usr">Actual Hours <b style="color: Red;" visible="false" id="mlblActualHours" runat="server">*</b> </label>
                                    <div id="tdActualHours" runat="server" visible="false" class="d-flex align-items-center">
                                        <div class="flex-fill pr-1">
                                            <asp:TextBox ID="txtActualHours" ValidationGroup="Task" CssClass="form-control form-control actualhours"
                                                runat="server" Visible="false">
                                            </asp:TextBox>
                                        </div>
                                        <asp:Label ID="lbActualHours" runat="server" Visible="false" CssClass="lbActualHours"></asp:Label>
                                        <div>
                                            <span style="vertical-align: middle" class="">hrs</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="fleft">
                                    <asp:RegularExpressionValidator ID="revEstimatedHours" CssClass="error" runat="server" ControlToValidate="txtEstimatedHours"
                                        ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter estimated hour in correct format"
                                        ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="revActualHours" CssClass="error" runat="server" ControlToValidate="txtActualHours"
                                        ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter actual hours in correct format"
                                        ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                                    <asp:CustomValidator ID="rfvActualHours" runat="server" CssClass="error" OnServerValidate="rfvActualHours_ServerValidate" ControlToValidate="txtActualHours" ValidationGroup="Task"
                                        Display="Dynamic" ErrorMessage="Please enter actual hours" ValidateEmptyText="true" Visible="false"></asp:CustomValidator>
                                </div>
                            </div>
                            <div id="tdlblERH" runat="server" visible="false" class="col-sm-4 col-md-4 col-xs-12">
                                <div class="form-group newTask_info_title ">
                                    <label for="usr">Estimated Remaining Hours</label>
                                    <div id="tdERH" runat="server" visible="false" class="d-flex align-items-center">
                                        <div class="flex-fill pr-1">
                                            <asp:TextBox ID="txtEstimatedRemainingHours" ValidationGroup="Task" CssClass="form-control estimatedRemaininghours"
                                                runat="server">
                                            </asp:TextBox>
                                            <asp:Label ID="lblEstimatedRemainingHours" runat="server" Visible="false"></asp:Label>
                                        </div>
                                        <div class="d-flex align-items-center">
                                            <span style="vertical-align: central" class="mr-1">hrs</span>
                                            <img id="imgRemainingHRS" runat="server" src="/Content/images/Autocalculator-new.png" style="border: none; width: 16px;"
                                                title="Auto Calculate" alt="" onclick="return AutoCalculateRemainingEstimateHours()" />
                                        </div>
                                    </div>
                                </div>
                                <div class="fleft">
                                    <asp:RegularExpressionValidator ID="revERHHours" CssClass="error" runat="server" ControlToValidate="txtEstimatedRemainingHours"
                                        ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter estimated remaining hours in correct format"
                                        ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="trAssignedTo" runat="server" visible="false" class="form-group col-md-12 col-sm-12 col-xs-12 noPadding">
                                <div class="all-drop-down">
                                    <%-- <label style="padding-left: 15px; padding-bottom: 10px;" for="usr">Assigned To</label>--%>
                                    <div id="Td1">
                                        <dx:ASPxCallbackPanel ID="pnlAssignToPct" ClientInstanceName="pnlAssignToPct" runat="server" OnCallback="pnlAssignToPct_Callback">
                                            <PanelCollection>
                                                <dx:PanelContent>
                                                    <div class="row">
                                                        <div class="col-md-8 col-sm-12 col-xs-12 noPadding">
                                                            <asp:Repeater ID="rAssignToPct" runat="server" OnItemDataBound="rAssignToPct_ItemDataBound"
                                                                OnItemCommand="rAssignToPct_ItemCommand">
                                                                <ItemTemplate>
                                                                    <div class="row dataAssignToPct">
                                                                        <div class="col-md-6 col-sm-6 col-xs-12">
                                                                            <label>Assigned To</label>
                                                                            <div class="d-flex align-items-center">
                                                                                <div class="flex-fill">
                                                                                    <asp:HiddenField ID="hdUserBoxId" runat="server" />
                                                                                    <div id="divAssignedToPct" runat="server"></div>
                                                                                    <asp:Label ID="lblPEAssignTo" runat="server" Text='<%# Eval("LoginName") %>' Visible="false"></asp:Label>
                                                                                </div>
                                                                                <img id="imgUserAvailability" cursor: pointer;' class='assigneeToImg ml-1' title='Find user based on availability' src='/Content/Images/add-groupBlue.png' />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6 col-sm-6 col-xs-12" id="itemPctComplete" runat="server">
                                                                            <label>Allocation %</label>
                                                                            <asp:TextBox ID="txtPercentage" runat="server" Text='<%# Eval("Percentage") %>' CssClass="edit-task-allocation all-input form-control bg-light-blue text-left">
                                                                            </asp:TextBox>
                                                                            <dx:ASPxButton ID="imgbtnDelete" runat="server" AutoPostBack="false" CssClass="edit-task-deleteBtn" RenderMode="Link" Image-Width="16px" ToolTip="Remove"
                                                                                Image-Url="/Content/images/grayDelete.png"
                                                                                CommandName="Delete" CommandArgument='<%# Eval("UserName") %>'>
                                                                            </dx:ASPxButton>
                                                                            <asp:Image ID="imgFlag" Style="padding-bottom: 7px; padding-left: 5px;" runat="server" Visible="false"
                                                                                ImageUrl="/Content/images/exclamation-red16X16.png" ToolTip="Over Allocated" />
                                                                            <asp:RegularExpressionValidator Display="Dynamic" ID="revPercentage" runat="server" ControlToValidate="txtPercentage"
                                                                                ValidationExpression="^(?:\d{1,3})(\.[0-9]{1,2})?$" ErrorMessage="Enter in correct format 00.00!"
                                                                                ValidationGroup="AssigntoPct"></asp:RegularExpressionValidator>
                                                                            <asp:RangeValidator Type="Double" Display="Dynamic" ValidationGroup="AssigntoPct" ErrorMessage="range 0-100!" ID="rngPercentageValidator"
                                                                                runat="server" ControlToValidate="txtPercentage" MinimumValue="0" MaximumValue="100"></asp:RangeValidator>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <div class="col-md-4 col-sm-4 col-xs-12">
                                                            <div style="padding-top: 18px;">
                                                                <asp:CustomValidator ID="cvUser" ValidateEmptyText="true" Enabled="true" runat="server" ErrorMessage="" Display="Dynamic" ValidationGroup="Task"></asp:CustomValidator>
                                                                <dx:ASPxButton ID="btnAssignToPct" runat="server" Text="Add Assignee" ValidationGroup="AssigntoPct" CssClass="primary-blueBtn" AutoPostBack="false">
                                                                    <ClientSideEvents Click="ShowWatingPopup" />
                                                                </dx:ASPxButton>
                                                                <img id="imgAssignee" runat="server" src="/Content/images/Autocalculator-new.png" style="border: none; padding-top: 4px; width: 16px;"
                                                                    title="Auto Calculate" alt="" onclick="return AutoCalculateAssignTo()" />
                                                                &nbsp;</img><asp:HiddenField ID="hdnReptRowcount" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>


                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dx:ASPxCallbackPanel>
                                    </div>
                                    <div id="tdlblAutoAdjustAllocation" runat="server" visible="false" style="vertical-align: top; width: 320px !important;">
                                        <label for="usr">Adjust Due Date when Allocation changed</label>
                                    </div>
                                    <div id="tdAutoAdjustAllocation" runat="server" visible="false">
                                        <asp:CheckBox ID="chAutoAdjustAllocation" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-xs-12 col-sm-4" id="trWeight" runat="server" visible="false">
                                <div class="form-group newTask_info_title all-drop-down">
                                    <label for="usr">Weight</label>
                                    <asp:TextBox ID="txtWeight" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtWeight"
                                        ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter weight in digit"
                                        ValidationExpression="^[0-9]+"></asp:RegularExpressionValidator>

                                    <asp:Label ID="lbWeight" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-xs-12" id="trDisableSLA" runat="server" visible="false">
                                <div class="form-group all-drop-down crm-checkWrap" style="padding-top: 15px;">
                                    <asp:CheckBox ID="chkDisableSLA" runat="server" Text="Disable SLA" />
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12" id="trAssignedToNew" runat="server" visible="false">
                                <div class="form-group all-drop-down">
                                    <label for="usr">Assigned To</label>
                                    <ugit:UserValueBox ID="peAssignedTo" runat="server" CssClass="userValueBox-dropDown" />
                                    <asp:Label ID="lbAssignedTo" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lbMessageForPendingApproval" Text="" runat="server" Style="color: red;" />
                                    <div id="assignToContainer" runat="server" visible="false">
                                        <asp:LinkButton ID="btnApprove" runat="server" Text="&nbsp;Approve&nbsp;" ToolTip="Approve" Visible="false">
                                        <span class="button-bg">
                                            <b style="float: left; font-weight: normal;">Approve</b>
                                            <i style="float: left; position: relative; top: -4px;left:4px">
                                                <img src="/Content/ButtonImages/approve.png"  style="border:none;" title="" alt=""/>
                                            </i> 
                                        </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btAssignToClear" runat="server" Text="&nbsp;Clear&nbsp;" ToolTip="Clear" Visible="false">
                                        <span class="button-bg">
                                            <b style="float: left; font-weight: normal;">Clear</b>
                                            <i style="float: left; position: relative; top: -4px;left:4px">
                                                <img src="/Content/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/>
                                            </i> 
                                        </span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12" id="startDateNew" runat="server" visible="false">
                                <asp:Label ID="lblStartDateNew" runat="server" CssClass="taskLabel"></asp:Label>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12" id="trStatus" runat="server" visible="false" style="clear: both;">
                                <div class="form-group all-select all-drop-down">
                                    <label for="usr">Status</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="aspxDropDownList" Height="35px">
                                        <asp:ListItem Text="Not Started"></asp:ListItem>
                                        <asp:ListItem Text="In Progress"></asp:ListItem>
                                        <asp:ListItem Text="Completed"></asp:ListItem>
                                        <asp:ListItem Text="Waiting"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lbStatus" runat="server" Visible="false"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12">
                                <div class="form-group newTask_info_title">
                                    <div id="tdPctCompletelbl" runat="server" visible="false">
                                        <label for="usr">% Complete</label>
                                    </div>
                                    <div id="tdPctComplete" runat="server" visible="false">
                                        <asp:TextBox ID="txtPctComplete" Width="50" ValidationGroup="Task" CssClass="pctcomplete" runat="server">
                                        </asp:TextBox>
                                        <asp:Label ID="lbPctComplete" runat="server" Visible="false"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-12" id="tdCompletedOn" runat="server">
                                <asp:Label ID="lblCompletedBy" runat="server" CssClass="taskLabel"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-xs-12" id="trDisableNotification" runat="server" visible="false">
                                <div class="form-group all-drop-down crm-checkWrap">
                                    <%--<label for="usr">Disable Notification</label>--%>
                                    <asp:CheckBox ID="chkDisableNotification" runat="server" Text="Disable Notification" TextAlign="Right" />
                                </div>
                            </div>
                            <div class="col-md-4 col-xs-12" id="trHoldDuration" runat="server" visible="false">
                                <div class="form-group newTask_info_title">
                                    <label for="usr">Hold Duration:</label>
                                    <asp:Label ID="lbHoldDuration" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-md-4 col-xs-12" id="taskHoldBlock" runat="server" visible="false">
                                <div class="form-group all-drop-down">
                                    <label for="usr">Hold Detail</label>
                                    <asp:Label ID="lbHoldTill" CssClass="padding-right10" runat="server"></asp:Label>
                                    <asp:Label ID="lbHoldReason" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="row" runat="server" id="fsMiscellaneous">
                <fieldset runat="server" id="fsMiscellaneous1">
                    <legend>Miscellaneous:</legend>
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="col-md-12 col-sm-12 col-xs-12" id="trServiceMatrix" runat="server" visible="false">
                            <div class="form-group newTask_info_title all-drop-down">
                                <label for="usr">
                                    Application Access Details
                                </label>
                            </div>
                            <div class="ms-formbody">
                                <asp:Panel ID="pnlServiceMatrix" runat="server"></asp:Panel>
                            </div>
                        </div>
                        <div class="newTask_info_title" id="trComment" runat="server" visible="false">
                            <label for="usr">Comment</label>
                            <asp:TextBox CssClass="all-input form-control bg-light-blue text-left" Rows="1" ID="txtComment" runat="server"
                                TextMode="MultiLine"></asp:TextBox>
                            <div style="float: left; width: 100%; display: block; max-height: 150px; overflow-x: auto;">
                                <asp:Repeater ID="rComments" runat="server" OnItemDataBound="RComments_ItemDataBound">
                                    <ItemTemplate>
                                        <div style="float: left; width: 100%;">
                                            <span style="font-weight: bold;"><a href="javascript:void(0);">
                                                <asp:Literal ID="lCommentOwner" runat="server"></asp:Literal></a>
                                                (<a href="javascript:void(0);"><asp:Literal ID="lCommentDate" runat="server"></asp:Literal></a>):</span>
                                            <span style="white-space: pre-wrap;">
                                                <asp:Literal ID="lCommentDesc" runat="server"></asp:Literal>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="row bs" id="trTaskReminderDays" runat="server" visible="false">
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 d-flex align-items-center flex-wrap">
                                <div class="pt-2">
                                    <asp:CheckBox ID="chkTaskReminderEnable" CssClass="crm-checkWrap" runat="server" AutoPostBack="true"
                                        OnCheckedChanged="chkTaskReminderEnable_CheckedChanged" Text="Reminder"/>
                                </div>
                                <div class="all-select pt-2 pr-3">
                                    <asp:Label ID="lblCount" runat="server" CssClass="popup-fieldLabel"></asp:Label>
                                    <asp:DropDownList ID="ddlCount" runat="server" Width="50px"></asp:DropDownList>

                                    <asp:Label ID="lblTimeInterval" runat="server" CssClass="popup-fieldLabel"></asp:Label>
                                    <asp:DropDownList ID="ddlTimeInterval" runat="server" Width="73px">
                                        <asp:ListItem Text="Days" Value="Days"></asp:ListItem>
                                        <asp:ListItem Text="Weeks" Value="Weeks"></asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:Label ID="lblIntervalType" runat="server" CssClass="popup-fieldLabel"></asp:Label>
                                    <asp:DropDownList ID="ddlIntervalType" runat="server" Width="73px">
                                        <asp:ListItem Text="Before" Value="-"></asp:ListItem>
                                        <asp:ListItem Text="After" Value="+"></asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:Label ID="lblDueDateText" runat="server" Text="Due Date"></asp:Label>
                                </div>
                                <div id="dvRepeatInterval" runat="server" class="all-select pt-2" visible="false">
                                    <asp:Label ID="lnlRepeatInterval" runat="server" Text="Repeat Interval" style="font: 13px 'Roboto', sans-serif;"></asp:Label>
                                    <asp:Label ID="lblRepeatCount" runat="server" CssClass="popup-fieldLabel"></asp:Label>
                                    <asp:DropDownList ID="ddlRepeatCount" runat="server" Width="50px"></asp:DropDownList>
                                    <asp:DropDownList ID="ddlInetrvalInWeeknDays" runat="server" Width="73px">
                                        <asp:ListItem Text="Days" Value="Days"></asp:ListItem>
                                        <asp:ListItem Text="Weeks" Value="Weeks"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Label ID="lblInetrvalInWeeknDays" runat="server" CssClass="popup-fieldLabel"></asp:Label>
                                </div>
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 mt-2">
                                <label class="ms-standardheader mr-1">Attachments</label>
                                <ugit:UploadAndLinkDocuments runat="server" ID="UploadAndLinkDocuments" />
                                <ugit:FileUploadControl ID="taskFileUploadControl" runat="server" Visible="false" />
                            </div>
                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" id="trShowonProjectCalendar" runat="server" visible="false">
                                <asp:CheckBox ID="chkShowonProjectCalendar" runat="server" CssClass="crm-checkWrap" Text="Show On Project Calendar" TextAlign="Right" />
                            </div>
                        </div>
                        <div id="trRepeatableTask" class="col-lg-12 col-md-12 col-sm-12 col-xs-12" runat="server" visible="false">
                            <div class="form-group tasksubitem all-select content-bottom">
                                <div class="recurringtaskdiv all-select ">
                                    <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2">
                                        <asp:CheckBox ID="chkRepeatEnable" CssClass="crm-checkWrap" Text="Recurring" AutoPostBack="true" runat="server" Checked="true" OnCheckedChanged="chkRepeatEnable_CheckedChanged" />
                                    </div>
                                    <div id="divRecurringTask" runat="server" class="col-lg-10 col-md-10 col-sm-10 col-xs-10">
                                        <div style="float: left; padding: 0px;" class="col-lg-5 col-md-5 col-sm-5 col-xs-5">
                                            <input type="number" min="1" max="100" value="1" class="taskTextbox recurringcount" />
                                            <asp:HiddenField ID="recurringcount" runat="server" />
                                            <label class="lblmessage"></label>
                                            <asp:DropDownList ID="ddlRecurringInterval" runat="server" Style="width: 50%">
                                                <asp:ListItem Value="Days" Text="Day(s)"></asp:ListItem>
                                                <asp:ListItem Value="Weeks" Text="Week(s)"></asp:ListItem>
                                                <asp:ListItem Value="Months" Text="Month(s)" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="Years" Text="Year(s)"></asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                        <div class="col-lg-2 col-md-2 col-sm-2 col-xs-2" style="padding: 8px 3px 0px 0px;">
                                            <asp:Label ID="lblRecurrEndDate" runat="server" CssClass="popup-fieldLabel">Recurring End Date</asp:Label>
                                        </div>
                                        <div class="col-lg-3 col-md-3 col-sm-3 col-xs-3 " style="float: left; padding: 3px 3px 0px 0px;">
                                            <dx:ASPxDateEdit ID="dtcRecurrEndDate" runat="server" ClientInstanceName="dtcRecurrEndDate" UseMaskBehavior="false" EditFormat="Date" CssClass="CRMDueDate_inputField"
                                                DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18px" ClientSideEvents-DateChanged="DateChanged">
                                            </dx:ASPxDateEdit>
                                            <asp:Label ID="dtcRecurrEndDateError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lblRecEndDateError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                        </div>

                                    </div>

                                </div>
                            </div>
                        </div>
                        <div id="trSvcConfigSelectModules" runat="server" visible="false" class="col-md-4 col-sm-4 col-xs-12">
                            <div class="form-group newTask_info_title all-drop-down">
                                <label for="usr">Module Name</label>
                                <asp:DropDownList Height="22px" ID="ddlModuleDetail" AutoPostBack="true" OnSelectedIndexChanged="DdlModuleDetail_SelectedIndexChanged"
                                    runat="server">
                                </asp:DropDownList>
                                <%--<ugit:LookUpValueBox FieldName="ModuleNameLookup" ID="ddlModuleDetail" ClientInstanceName="ddlModuleDetail"
                                     runat="server" CssClass="lookupValueBox-dropown" />--%>
                            </div>
                            <div class="form-group newTask_info_title all-drop-down noPadding">
                                <label for="usr">Request Type</label>
                                <div class="fleft" style="display: inline-flex;">
                                    <div>
                                        <asp:DropDownList Height="22px" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeSubCategory_SelectedIndexChanged" Width="200" ID="ddlRequestTypeSubCategory" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:DropDownList Height="22px" Width="200" ID="ddlTicketRequestType" AutoPostBack="true" OnSelectedIndexChanged="ddlTicketRequestType_SelectedIndexChanged" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <%--<div id="tdRequestType" runat="server"></div>--%>
                            </div>
                        </div>
                        <div id="trSvcSelectModules" runat="server" visible="false">
                            <div class="form-group newTask_info_title all-drop-down">
                                <label for="usr">Module Name</label>
                                <ugit:LookUpValueBox FieldName="ModuleName" CssClass="lookupValueBox-dropown" ID="ddlModules" autopostback="true" runat="server" />
                            </div>
                            <div class="form-group newTask_info_title all-drop-down">
                                <label for="usr">Map Ticket<b style="color: Red">*</b></label>
                                <asp:DropDownList Width="100%" ID="ddlTickets" runat="server" CssClas="itsmDropDownList aspxDropDownList">
                                </asp:DropDownList>
                                <asp:Label ID="lbTicketMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-12 col-sm-12 col-xs-12" style="clear: both" id="trAttachment" runat="server" visible="false">

                            <div id="bindMultipleLink" runat="server"></div>
                            <div class="editTicket_addDoc_wrap task_addDoc_wrap">
                                <div class="row">
                                    <asp:Panel ID="pAttachmentContainer" runat="server" CssClass="attachment-container">
                                        <div class="doc-btn-name">
                                            <span style="display: none;">
                                                <asp:DropDownList ID="ddlExistingAttc" CssClass="itsmDropDownList aspxDropDownList" runat="server"></asp:DropDownList>
                                                <asp:TextBox ID="txtDeleteFiles" runat="server"></asp:TextBox>
                                            </span>
                                            <asp:Panel ID="pAttachment" runat="server" CssClass="documents-file">
                                            </asp:Panel>
                                            <asp:Panel ID="pNewAttachment" runat="server" CssClass="documents-file">
                                            </asp:Panel>
                                            <asp:Panel ID="pAddattachment" runat="server" CssClass="documents-file">
                                                <%--<a href="javascript:void(0);" onclick="addAttachment()">Add More Files</a>--%>
                                            </asp:Panel>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>

                        </div>
                    </div>

                </fieldset>
            </div>
        </div>
        <div class="">
            <fieldset id="accounttask" style="display: none">
                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%" style="display: none;">
                    <tr id="trTaskType" runat="server" visible="false">
                        <td class="ms-formlabel">
                            <h3 class="ms-standardheader">Account Task
                            </h3>
                        </td>
                        <td class="ms-formbody">
                            <asp:CheckBox ID="chkAccountTask" AutoPostBack="true" OnCheckedChanged="chkAccountTask_CheckedChanged" runat="server" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <div class="row content-wrap" id="fsActualHours" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="row">
                    <div id="trActualHours" runat="server" class="col-md-12 col-sm-12 col-xs-12">
                        <div class="form-group newTask_info_title col-md-6" visible="false">
                        </div>

                        <div class="form-group all-drop-down col-md-6" visible="false">
                        </div>
                        <asp:Panel ID="panelActualHours" runat="server">
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row bs pt-3">
        <div id="btActions" runat="server" class="col-md-3 col-sm-3 col-xs-12 pb-2">
            <dx:ASPxButton CssClass="primary-blueBtn editTask-actionbtn" Visible="false" ClientInstanceName="aspxMoreActions" ToolTip="Actions" AutoPostBack="false" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover"
                ID="aspxMoreActions" runat="server" Text="Actions">
                <%--<HoverStyle CssClass="ugit-buttonhover"></HoverStyle>--%>
                <Image Url="/Content/images/caret-down.png" Width="10px"></Image>
            </dx:ASPxButton>
            <dx:ASPxPopupMenu ID="ASPxPopupActionMenu" runat="server" PopupElementID="aspxMoreActions" class="editTask_actionBtn_listWrap" CloseAction="MouseOut" ItemSpacing="0" SubMenuStyle-ItemSpacing="0" ClientInstanceName="ASPxPopupActionMenu" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" PopupAction="LeftMouseClick">
                <Items>
                    <dx:MenuItem Name="DeleteTask" Text="Delete Task" Visible="false" Image-Url="/Content/images/close.png" Image-Width="10px"></dx:MenuItem>
                    <dx:MenuItem Name="CancelTask" Text="Cancel Task" Visible="false" Image-Url="/Content/images/close.png" Image-Width="10px"></dx:MenuItem>
                    <dx:MenuItem Name="DeleteMappingTask" Text="Delete" Visible="false" Image-Url="/Content/images/close.png" Image-Width="10px"></dx:MenuItem>
                    <dx:MenuItem Name="Hold" Text="Put on Hold" Visible="false" Image-Url="/Content/images/lock_close.png" Image-Width="10px" ItemStyle-BackColor="Red"></dx:MenuItem>
                    <dx:MenuItem Name="EditHold" Text="Edit Hold" Visible="false" Image-Url="/Content/images/lock_close.png" ItemStyle-ForeColor="#E46E94" ItemStyle-Font-Bold="true" Image-Width="10px"></dx:MenuItem>
                    <dx:MenuItem Name="UnHold" Text="Remove Hold" Visible="false" Image-Url="/Content/images/unlock.png" Image-Width="10px" ItemStyle-BackColor="Green"></dx:MenuItem>
                    <dx:MenuItem Name="UncancelTask" Text="Uncancel Task" Visible="false" Image-Url="/Content/images/close.png" Image-Width="10px"></dx:MenuItem>
                </Items>
                <ClientSideEvents ItemClick="function(s, e) { popupMenuActionMenuItemClick(s,e);}" />
                <ItemStyle CssClass="dxb" BackColor="#ebedf2" HoverStyle-BackColor="Black"></ItemStyle>
                <%-- <ItemStyle BackgroundImage-ImageUrl="/ontent/images/firstnavbg1X28.gif" BackgroundImage-Repeat="RepeatX" Height="28px" ForeColor="White" VerticalAlign="Middle"
                            HoverStyle-BackgroundImage-ImageUrl="/ontent/images/firstnavbg_hover1X28.gif"
                            HoverStyle-BackgroundImage-Repeat="RepeatX" DropDownButtonStyle-CssClass="dropdownicon"></ItemStyle>--%>
                <%-- <SubMenuItemStyle BackgroundImage-ImageUrl="/Content/images/firstnavbg1X28.gif" BackgroundImage-Repeat="RepeatX" Height="28px" ForeColor="White" VerticalAlign="Middle"
                            HoverStyle-BackgroundImage-ImageUrl="/Content/images/firstnavbg_hover1X28.gif" HoverStyle-BackgroundImage-Repeat="RepeatX">
                        </SubMenuItemStyle>--%>
            </dx:ASPxPopupMenu>
            <dx:ASPxButton ClientVisible="false" CssClass="ugit-button" ClientInstanceName="lnkDelete" ToolTip="Delete Task" AutoPostBack="true" ImagePosition="Right" Visible="false" HoverStyle-CssClass="ugit-buttonhover"
                OnClick="lnkDelete_Click" ID="lnkDelete" runat="server" Text="Delete Task">
                <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                <Image Url="/Content/images/delete-icon.png"></Image>
                <ClientSideEvents Click="function(s, e) { confirmBeforeDelete(s,e); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ClientVisible="false" CssClass="ugit-button hide" ClientInstanceName="lnkCancel" AutoPostBack="true" ToolTip="Cancel Task" ImagePosition="Right" Visible="false" HoverStyle-CssClass="ugit-buttonhover"
                OnClick="lnkCancel_Click" ID="lnkCancel" runat="server" Text="Cancel Task">
                <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                <Image Url="/Content/images/cancel.png"></Image>
                <ClientSideEvents Click="function(s, e) { confirmBeforeCancel(s,e); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ClientVisible="false" CssClass="ugit-button hide" ClientInstanceName="btDeleteMappingTask" AutoPostBack="true" ToolTip="Delete" ImagePosition="Right" Visible="false" HoverStyle-CssClass="ugit-buttonhover"
                OnClick="btDeleteMappingTask_Click" ID="btDeleteMappingTask" runat="server" Text="Delete">
                <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                <Image Url="/Content/images/cancel.png"></Image>
                <ClientSideEvents Click="function(s, e) { confirmBeforeDelete(s,e); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ClientVisible="false" CssClass="ugit-button hide" ClientInstanceName="aspxHoldTask" ToolTip="Hold Task" AutoPostBack="false" ImagePosition="Right" Visible="false" HoverStyle-CssClass="ugit-buttonhover"
                ID="aspxHoldTask" runat="server" Text="Hold">
                <HoverStyle CssClass="ugit-button hover"></HoverStyle>
                <Image Url="/Content/images/lock.png"></Image>
                <ClientSideEvents Click="function(s,e){holdTask_click(s,e);}" />
            </dx:ASPxButton>
            <dx:ASPxButton CssClass="ugit-button" ClientVisible="false" ClientInstanceName="aspxUnholdTask" ToolTip="Unhold Task" AutoPostBack="false" ImagePosition="Right" Visible="false" HoverStyle-CssClass="ugit-buttonhover"
                ID="aspxUnholdTask" runat="server" Text="Unhold">
                <HoverStyle CssClass="ugit-button hover"></HoverStyle>
                <Image Url="/Content/images/unlock.png"></Image>
                <ClientSideEvents Click="function(s,e){unholdTask_click(s,e);}" />
            </dx:ASPxButton>
            <dx:ASPxButton ClientVisible="false" CssClass="ugit-button hide" ClientInstanceName="lnkUncancelTask" AutoPostBack="true" ToolTip="Cancel Task" ImagePosition="Right" Visible="false" HoverStyle-CssClass="ugit-buttonhover"
                OnClick="lnkUncancelTask_Click" ID="lnkUncancelTask" runat="server" Text="Uncancel Task">
                <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                <Image Url="/_layouts/15/images/uGovernIT/ButtonImages/cancel.png"></Image>
                <ClientSideEvents Click="function(s, e) { confirmBeforeUncancel(s,e); }" />
            </dx:ASPxButton>
        </div>
        <div id="pAuditInformataion" runat="server" visible="false" class="col-md-5 col-sm-5 col-xs-12 pb-2" style="text-align: center">
            <asp:Label ID="lbCreatedInfo" runat="server" CssClass="fullwidth"></asp:Label>
            <asp:Label ID="lbModifiedInfo" runat="server" CssClass="fullwidth"></asp:Label>
        </div>
        <div id="btActionContainer" runat="server" class="col-md-4 col-sm-4 col-xs-12 pb-3">
            <div class="actionBtn-wrap">
                <dx:ASPxButton ID="BtCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" Visible="true" OnClick="BtCancel_Click">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btSaveAndNotify" runat="server" Text="Save & Notify" CssClass="primary-blueBtn" Visible="false" OnClick="BtSaveTask_Click" ValidationGroup="Task_new">
                    <ClientSideEvents Click="checkBeforSave" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btSaveTask" runat="server" Text="Save" CssClass="primary-blueBtn" AutoPostBack="false" OnClick="BtSaveTask_Click" ValidationGroup="Task_new">
                    <ClientSideEvents Click="checkBeforSave" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btSaveMyTask" runat="server" Text="Save" Visible="false" OnClick="BtSaveMyTask_Click" CssClass="primary-blueBtn">
                    <ClientSideEvents Click="checkBeforSave" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btSaveAndNotifyMyTask" runat="server" Text="Save & Notify" CssClass="primary-blueBtn" Visible="false" OnClick="BtSaveMyTask_Click" ValidationGroup="Task_new">
                    <ClientSideEvents Click="checkBeforSave" />
                </dx:ASPxButton>
            </div>
        </div>
        <div id="btActionscancel" runat="server" visible="false" class="col-md-2 col-sm-2 col-xs-2 px-0">
            <div class="actionBtn-wrap">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" Visible="true" OnClick="BtCancel_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>


<asp:HiddenField ID="hdnActionType" runat="server" />

<dx:ASPxPopupControl ID="pcAssignTo" runat="server" ClientInstanceName="pcAssignTo"
    HeaderText="Summary Task Resource Assignment" CloseAction="CloseButton" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxPanel ID="pnlcontrol" runat="server" Width="300px" DefaultButton="btnOK">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Table ID="tParameter" runat="server" Width="100%">
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                    <asp:Label ID="lblMsg" Text="Also apply resource assignments to child task(s)?" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                                     &nbsp;
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell ID="TableCell2" runat="server">
                                    <%--<dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false"
                                        UseSubmitBehavior="false" Style="text-align: left; margin-right: 8px">
                                        <ClientSideEvents Click="function(s, e) { pcAssignTo.Hide();}" />
                                    </dx:ASPxButton>--%>
                                </asp:TableCell><asp:TableCell ID="TableCell1" runat="server">
                                    <dx:ASPxButton ID="btnNo" runat="server" Text="  No  " AutoPostBack="true"
                                        Style="float: right; margin-right: 8px; display: inline">
                                        <ClientSideEvents Click="function(s,e){ pcAssignTo.Hide();}" />
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnOK" runat="server" Text="  Yes  " AutoPostBack="true"
                                        Style="float: right; margin-right: 8px; display: inline">
                                        <ClientSideEvents Click="function(s,e){pcAssignTo.Hide();}" />
                                    </dx:ASPxButton>

                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<asp:HiddenField ID="hdnStrAssignToPct" runat="server" />

<asp:HiddenField ID="skillJson" runat="server" />
<asp:HiddenField ID="hdnEditSkill" runat="server" />
<asp:HiddenField ID="hdnSkillText" runat="server" />

<dx:ASPxPopupControl ClientInstanceName="commentsHoldPopup" Modal="true"
    PopupElementID="holdWithCommentsButton" ID="commentsHoldPopup"
    ShowFooter="false" ShowHeader="true" CssClass="departmentPopup putOnHold_popUp" HeaderText="Put on Hold"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="putOnHold_popup_wrap">
                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div>
                                <asp:Label ID="lblHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label>
                            </div>
                            <div class="putOnHold_input_label">Hold Till<b style='color: red'> * </b></div>
                            <div class="aspxDateEdit-dropDownWrap">
                                <dx:ASPxDateEdit ID="aspxdtOnHoldDate" runat="server" ClientInstanceName="aspxdtOnHoldDate"
                                    CssClass="CRMDueDate_inputField" DropDownButton-Image-Url="/Content/Images/calendarNew.png" DropDownButton-Image-Width="18px">
                                    <TimeSectionProperties>
                                        <TimeEditProperties EditFormatString="hh:mm:tt"></TimeEditProperties>
                                    </TimeSectionProperties>
                                    <ValidationSettings ValidationGroup="holdtask" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField IsRequired="true" ErrorText="Please select date." />
                                    </ValidationSettings>
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="putOnHold_input_label">Reason</div>
                            <div class="reason_dropDown_holder">
                                <dx:ASPxComboBox ID="aspxOnHoldReason" runat="server" CssClass="aspxComBox-dropDown itsmDropDownList" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                    <Items>
                                        <dx:ListEditItem Value="Waiting on User" Selected="true" Text="Waiting on User" />
                                        <dx:ListEditItem Value="Waiting on Purchase" Text="Waiting on Purchase" />
                                        <dx:ListEditItem Value="Waiting on Vendor" Text="Waiting on Vendor" />
                                        <dx:ListEditItem Value="Waiting on Resources" Text="Waiting on Resources" />
                                        <dx:ListEditItem Value="Other" Text="Other" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                    <div class="row putOnHold_comment_wrap">
                        <div class="putOnHold_input_label">Comment</div>
                        <div style="padding-top: 5px;">
                            <asp:TextBox runat="server" ID="popedHoldComments" Width="500px" Columns="30" Rows="3" TextMode="MultiLine" Text="" CssClass="form-control bg-light-blue"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row putOnHold-btnContainer">
                        <dx:ASPxButton CssClass="primary-blueBtn" ValidationGroup="holdtask" ToolTip="Hold Task" AutoPostBack="true" ImagePosition="Right" OnClick="aspxTriggerTaskHold_Click" HoverStyle-CssClass="ugit-buttonhover"
                            ID="NotifyHold" runat="server" Text="Hold & Notify">
                            <%--<Image Url="/_layouts/15/images/uGovernIT/ButtonImages/lock.png"></Image>--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton CssClass="primary-blueBtn" ValidationGroup="holdtask" ToolTip="Hold Task" AutoPostBack="true" ImagePosition="Right" OnClick="aspxTriggerTaskHold_Click" HoverStyle-CssClass="ugit-buttonhover"
                            ID="aspxTriggerTaskHold" runat="server" Text="Put on Hold">
                            <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                            <%--<Image Url="/Content/images/lock.png"></Image>--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton CssClass="secondary-cancelBtn" ToolTip="Cancel" AutoPostBack="false" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover"
                            ID="ASPxButton1" runat="server" Text="Cancel">
                            <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                            <%--<Image Url="/Content/images/cancel.png"></Image>--%>
                            <ClientSideEvents Click="function(s,e){commentsHoldPopup.Hide();}" />
                        </dx:ASPxButton>

                    </div>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ClientInstanceName="commentsUnHoldPopup" Modal="true"
    PopupElementID="unHoldWithCommentsButton" ID="commentsUnHoldPopup"
    ShowFooter="false" ShowHeader="true" CssClass="departmentPopup removeOnHold_popUp" HeaderText="Remove Hold Feedback"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <div style="float: left; height: 220px; width: 400px;" class="first_tier_nav">
                <div style="width: 100%;">
                    <div>
                        <asp:Label ID="lblUnHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                    </div>
                    <div>
                        <asp:TextBox runat="server" ID="popedUnHoldComments" Width="400px" Columns="52" Rows="9" TextMode="MultiLine" Text="" CssClass="form-control bg-light-blue"></asp:TextBox>
                    </div>
                    <div class="fright" style="padding-top: 10px">
                        <dx:ASPxButton CssClass="primary-blueBtn" ToolTip="Unhold Task" AutoPostBack="true" ImagePosition="Right" OnClick="aspxTriggerUnholdTask_Click" HoverStyle-CssClass="ugit-buttonhover"
                            ID="aspxTriggerUnholdTask" runat="server" Text="Remove Hold">
                            <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                            <%--<Image Url="/Content/images/unlock.png"></Image>--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton CssClass="secondary-cancelBtn" ToolTip="Cancel" AutoPostBack="false" ImagePosition="Right" HoverStyle-CssClass="ugit-buttonhover"
                            ID="ASPxButton2" runat="server" Text="Cancel">
                            <HoverStyle CssClass="ugit-buttonhover"></HoverStyle>
                            <%--<Image Url="/Content/images/cancel.png"></Image>--%>
                            <ClientSideEvents Click="function(s,e){commentsUnHoldPopup.Hide();}" />
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<dx:ASPxPopupControl ClientInstanceName="commentsRejectPopup" Modal="true"
    ID="commentsRejectPopup"
    ShowFooter="false" ShowHeader="true" HeaderText="Reject Reason"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <div style="float: left; height: 200px; width: 420px;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lblRejectMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>
                            <h3 class="ms-standardheader" style="text-align: left !important;">Reason<b style="color: red;">*</b> </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="popedRejectComments" Width="400px" Columns="50" Rows="9" TextMode="MultiLine" Text="" ValidationGroup="Task"></asp:TextBox><asp:RequiredFieldValidator ID="rfvpopedRejectComments" runat="server" ValidationGroup="Task" ControlToValidate="popedRejectComments"
                                Display="Dynamic" CssClass="errormsg-container" ErrorMessage="Field required!"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td class="buttoncell">
                            <div class="first_tier_nav" style="width: 100%">
                                <ul style="float: right">
                                    <li runat="server" id="Li1" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="color: red">
                                        <asp:LinkButton runat="server" ID="rejectButton" Style="color: white" Name="Reject" CssClass="reject" ValidationGroup="Task"
                                            Text="Reject" OnClick="BtRejectReason_Click" />
                                    </li>
                                    <li runat="server" id="Li2" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                        <a style="color: white" class="cancelwhite" onclick="commentsRejectPopup.Hide();" href="javascript:void(0);">Cancel</a> </li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>

<div id="popupContainer"></div>

<script id="dxss_inlineCtrScript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];

    var popupFilters = {};
    var minDate = "0001-01-01T00:00:00";
    //var filename = null;
    var ticketID = '<%= projectPublicID %>';
    var moduleName = '<%= ModuleName %>';
    var projectID = "<%= Request["TicketId"]%>";
    var selectedUserControlId = '';
<%--    var currentTasKIndex = <%= tasKIndex %>;--%>
    var strResourceSelectFilter = '<%=ResourceSelectFilter%>';

    function bindDatapopup(popupFilters) {
        var titleView = null;
        if ($("#tileViewContainer").length > 0) {

            var titleViewObj = $('#tileViewContainer').dxTileView('instance');
            if (titleViewObj) {
                titleViewObj.option("dataSource", "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters));
                titleViewObj._refresh();
            }
        }
        else {

            titleView = $("<div id='tileViewContainer' style='clear:both;padding-top: 30px' />").dxTileView({
                height: window.innerHeight - 210,
                width: window.innerWidth - 250,
                showScrollbar: true,
                baseItemHeight: 65,
                baseItemWidth: 150,
                itemMargin: 15,
                direction: "vertical",
                // showScrollbar: true,
                elementAttr: { "class": "tileViewContainer" },
                noDataText: "No resource available",
                dataSource: "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters),
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var html = new Array();
                    html.push("<div class='UserDetails'>");
                    html.push("<div id='" + itemData.AssignedTo + "'>");
                    //   html.push("<div id='" + itemData.AssignedTo + "'>");
                    html.push("<div class='AssignedToName'>");
                    html.push(itemData.AssignedToName);
                    html.push("</div>");
                    if (itemData.PctAllocation >= 100) {
                        html.push("<div>");
                        html.push("(Over Allocated: " + itemData.PctAllocation + "%)");
                        html.push("</div>");
                    }
                    else if (itemData.PctAllocation > 0) {
                        html.push("<div style='padding-bottom:3px;'>");
                        html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                        html.push("</div>");
                    }
                    if (popupFilters.projectCount || popupFilters.projectVolume) {
                        if (itemData.PctAllocation > 0) {
                            html.push("<div class='capacitymain'>");
                            html.push("<div class='capacityblock allocation-v" + itemData.TotalVolumeRange + "'>");
                            html.push(itemData.TotalVolume);
                            html.push("</div>");

                            html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                            html.push("#" + itemData.ProjectCount);
                            html.push("</div>");
                            html.push("</div>");
                        }
                    }
                    html.push("</div>");
                    html.push("</div>");

                    itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                    itemElement.append(html.join(""));

                },
                onItemClick: function (e) {

                    try {
                        var data = e.itemData;
                        if (ASPxClientControl.GetControlCollection().GetByName(selectedUserControlId) != null)
                            ASPxClientControl.GetControlCollection().GetByName(selectedUserControlId).SetValue(data.AssignedTo);
                    } catch (e) {

                    }
                    finally {
                        $('#popupContainer').dxPopup('instance').hide();
                    }

                }

            });
        }

        return titleView;
    };

    $(document).on("click", "img.assigneeToImg", function (e) {

        var groupid = $(this).attr("id");
        if ($(this).siblings('input[type=hidden]').length > 0) {
            if ($(this).siblings('input[type=hidden]')[0] != undefined) {
                selectedUserControlId = $(this).siblings('input[type=hidden]')[0].value + 'LookupSearchValue';
            }
        }
        popupFilters.projectID = ticketID;
        popupFilters.resourceAvailability = 2;
        popupFilters.complexity = false;
        popupFilters.projectVolume = false;
        popupFilters.projectCount = false;
        popupFilters.RequestTypes = false;
        popupFilters.groupID = $(this).attr("group");

        if (dtcStartDate.date != null)
            popupFilters.allocationStartDate = dtcStartDate.date.format('yyyy-MM-ddTHH:mm:ss'); //$(this).attr("startDate");

        if (dtcDueDate.date != null)
            popupFilters.allocationEndDate = dtcDueDate.date.format('yyyy-MM-ddTHH:mm:ss'); //$(this).attr("endDate");

        if (ticketID.startsWith("OPM") || ticketID.startsWith("PMM")) {
            popupFilters.ModuleIncludes = true;
        }
        else {
            popupFilters.ModuleIncludes = false;
        }
        popupFilters.JobTitles = "";
        popupFilters.departments = "";
        popupFilters.SelectedUserID = "";
        popupFilters.ID = $(this).attr("ID");
        RowId = $(this).attr("ID");
        var popupTitle = "Available Resource";

        $("#popupContainer").dxPopup({
            title: popupTitle,
            width: "85%",
            height: "90%",
            visible: true,
            scrolling: true,
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<div class='col-md-6 colsm-6 col-xs-12 devExtRadioGroup' />").dxRadioGroup({
                        dataSource: radioGroupItems,
                        displayExpr: "text",
                        value: _.findWhere(radioGroupItems, { value: popupFilters.resourceAvailability }),
                        layout: "horizontal",
                        onValueChanged: function (e) {
                            popupFilters.resourceAvailability = e.value.value;
                            bindDatapopup(popupFilters);
                        }
                    }),
                    $("<div class='col-md-6 col-sm-6 col-xs-12 devExtChkBox-wrap'>").append(
                        $("<div id='projecttype' class='chkFilterCheck col-md-3 noPadding'  />").dxCheckBox({
                            text: "Project Type",
                            onValueChanged: function (e) {
                                popupFilters.RequestTypes = e.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                        $("<div id='capacity' class='chkFilterCheck col-md-3 noPadding' />").dxCheckBox({
                            text: "Capacity",
                            value: popupFilters.projectVolume,
                            onValueChanged: function (e) {
                                popupFilters.projectVolume = e.value;
                                popupFilters.projectCount = e.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                        $("<div id='complexity' class='chkFilterCheck col-md-3 noPadding' />").dxCheckBox({
                            text: "Complexity",
                            value: popupFilters.complexity,
                            onValueChanged: function (e) {

                                popupFilters.complexity = e.value;
                                bindDatapopup(popupFilters);
                            },
                        }),

                        $("<div id='opportunities' class='chkFilterCheck col-md-3 noPadding'  />").dxCheckBox({
                            text: "Opportunities",
                            value: popupFilters.ModuleIncludes,
                            onValueChanged: function (e) {
                                popupFilters.ModuleIncludes = e.value;
                                bindDatapopup(popupFilters);
                            }
                        }),

                    ),


                    $("<div class='filterctrl-jobDepartment col-md-4 col-sm-6 col-xs-12' />").dxSelectBox({
                        placeholder: "Limit By Department",
                        valueExpr: "ID",
                        displayExpr: "Title",
                        dataSource: "/api/rmmapi/GetDepartments?GroupID=" + popupFilters.groupID,
                        onSelectionChanged: function (selectedItems) {
                            var items = selectedItems.selectedItem.ID;
                            var tagBox = $("#tagboxTitles").dxTagBox("instance");
                            popupFilters.departments = items;
                            $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=" + items, function (data, status) {
                                JobTitleData = data;
                                tagBox.option("dataSource", JobTitleData);
                                tagBox.reset();
                            });

                            bindDatapopup(popupFilters);
                        },
                        onContentReady: function (e) {

                        }
                    }),
                    $("<div id='tagboxTitles'  class='filterctrl-jobtitle col-md-4 col-sm-6 col-xs-12 noPadding'/>").dxTagBox({
                        text: "Job Titles",
                        placeholder: "Limit By Job Title",
                        //valueExpr: "Title",
                        //displayExpr: "Title",

                        dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=0",
                        maxDisplayedTags: 2,
                        onSelectionChanged: function (selectedItems) {
                            var items = selectedItems.component._selectedItems;
                            if (items.length > 0) {
                                var lstItems = items.map(function (i) {
                                    return i;
                                });
                                popupFilters.JobTitles = lstItems.join(';#');
                            }
                            else {
                                popupFilters.JobTitles = '';
                            }
                            bindDatapopup(popupFilters);
                        }
                    }),

                    $("<div class='filterctrl-userpicker col-md-4 col-sm-6 col-xs-12 noPadding' />").dxSelectBox({
                        placeholder: "Choose Any User",
                        valueExpr: "Id",
                        displayExpr: "Name",
                        searchEnabled: true,
                        showClearButton: true,
                        dataSource: "/api/rmmapi/GetUserList",
                        onSelectionChanged: function (selectedItems) {
                            if (selectedItems.selectedItem == null) {
                                popupFilters.SelectedUserID = "";
                                popupFilters.complexity = false;
                                //var checkcomplexity = $("#complexity").dxCheckBox("instance");
                                //checkcomplexity.option("value", true);
                                bindDatapopup(popupFilters);
                            } else {
                                var items = selectedItems.selectedItem.Id;
                                popupFilters.SelectedUserID = items;
                                popupFilters.complexity = false;
                                bindDatapopup(popupFilters);
                            }
                        }
                    }),
                    bindDatapopup(popupFilters)
                )

            },
            itemClick: function (e) {

            },
            onContentReady: function (e) {
                strResourceSelectFilter = '<%=ResourceSelectFilter%>';
                var arrResourceSelectFilter = [];
                if (strResourceSelectFilter)
                    arrResourceSelectFilter = strResourceSelectFilter.split(';#');

                arrResourceSelectFilter.forEach(function (x, i) {
                    var arrfilter = x.split('=');
                    if (arrfilter && arrfilter.length == 2 && arrfilter[1] == 'true') {
                        var chck = $('#' + arrfilter[0].toLowerCase());
                        chck.css({ 'display': 'block' });
                    }
                    else {
                        var chck = $('#' + arrfilter[0].toLowerCase());
                        chck.css({ 'display': 'none' });
                    }
                });
            }
        });
        var popupInstance = $('#popupContainer').dxPopup('instance');
        popupInstance.option('title', popupTitle);
    });

</script>

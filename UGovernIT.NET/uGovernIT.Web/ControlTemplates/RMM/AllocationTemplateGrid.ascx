<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllocationTemplateGrid.ascx.cs" Inherits="uGovernIT.Web.AllocationTemplateGrid" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
     .dx-datagrid-revert-tooltip {
        display: none;
    }
    .dx-calendar-body thead th {
        width:0px;
    }
    .isa_info, .isa_success, .isa_warning, .isa_error {
        margin: 10px 0px;
        padding: 12px;
    }

    .isa_info {
        color: #00529B;
        background-color: #BDE5F8;
    }

    .isa_success {
        color: #4F8A10;
        background-color: #DFF2BF;
    }

    .isa_warning {
        color: #9F6000;
        background-color: #FEEFB3;
    }

    .isa_error {
        color: #D8000C;
        background-color: #FFD2D2;
    }

        .isa_info i, .isa_success i, .isa_warning i, .isa_error i {
            margin: 10px 30px;
            font-size: 2em;
            vertical-align: middle;
        }

    .chkFilterCheck {
        /*padding-top:5px;*/
        padding-left: 2px;
        padding-right: 2px;
    }

    .tileViewContainer .dx-tile {
        text-align: center;
    }

    .ClassNoLevel {
        background-color: #82e186;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .ClassLevel1 {
        background-color: #d0ff3f;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .ClassLevel2 {
        background-color: #82e186;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .ClassLevel3 {
        background-color: #f68d67;
        padding-bottom: 5px;
        padding-top: 5px;
        height: inherit;
    }

    .btnProceed, .btnProceed:hover {
        background-color: #4fa1d6;
        margin-top: 20px;
        color: #fff;
    }

    .btnAddNew, .btnAddNew:hover {
        background-color: #4A6EE2 !important;
        margin-top: 20px;
        color: #fff;
    }

    #tileViewContainer .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    /*#tileViewContainer .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        height: 20px;
    }

        #tileViewContainer .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }*/

    #tileViewContainer .allocation-v0 {
        background: #ffffff;
    }

    #tileViewContainer .allocation-v1 {
        background: #fcf7b5;
    }

    #tileViewContainer .allocation-v2 {
        background: #f8ac4a;
    }

    #tileViewContainer .allocation-r0 {
    }

    #tileViewContainer .allocation-r1 {
        background: #baf0d7;
    }

    #tileViewContainer .allocation-r2 {
        background: #fcf7b5;
    }

    #tileViewContainer .allocation-r3 {
        background: #f8ac4a;
    }

    #tileViewContainer .allocation-c0 {
        background: #ffffff;
    }

    #tileViewContainer .allocation-c1 {
        background: #fcf7b5;
    }

    #tileViewContainer .allocation-c2 {
        background: #f8ac4a;
    }

    #tileViewContainer .allocation-block {
        height: 63px;
        display: flex;
        justify-content: center;
        align-items: center;
    }


    #tileViewContainer .dx-tile {
        border: 1px solid #c3c3c3;
    }

    #tileViewContainer .capacitymain {
        border-top: 1px solid #c3c3c3;
    }

    .filterlb-jobtitle {
        float:left;
        padding-left:15px;
        float: left;
        padding-top: 7px;
        margin-top: 5px;
        width:80px;
    }

    .filterctrl-jobtitle {
        width: 30%;
        float: left;
        margin-left: 10px;
        margin-top:6px;
    }

    .filterlb-jobDepartment {
        float:left;
        clear:both;
        padding-left:15px;
        float: left;
        padding-top: 7px;
        padding-right: 7px;
        margin-top: 5px;
        width:100px;
    }
     .filterctrl-jobDepartment {
       clear: both;
       float: left;
       width: 30%;
       margin-top:6px;
       margin-left:12px;
    }
       .filterctrl-userpicker{
        width: 30%;
        float: left;
        margin-left: 10px;
        margin-top:6px;
       }

    .cls .dx-datagrid-revert-tooltip {
        display: none;
    }
    .marcal{
       float: left;
       clear: both;
       width: 100%;
       text-align:center;
       margin-bottom: 240px;             
    }
   #tileViewContainer .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    #tileViewContainer .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        /*height: 20px;*/
        padding-top:2px;
        padding-bottom: 2px;
    }

        #tileViewContainer .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }
    #tileViewContainer .capacityblock-1 {
        float: left;
        width: 148px;
        text-align: center;
        /*height: 20px;*/
        padding-top: 2px;
        padding-bottom: 2px;
    }

    #tileViewContainer .capacityblock-1:first-child {
        border-right: 1px solid #c3c3c3;
    }
    #tileViewContainer .allocation-v0 {
        background: #ffffff;
        color: #000;
    }

    #tileViewContainer .allocation-v1 {
        background: #fcf7b5;
        color: #000;
    }

    #tileViewContainer .allocation-v2 {
        background: #f8ac4a;
        color: #000;
    }

    #tileViewContainer .allocation-r0 {
        /*background: #57A71D;*/
        background:#6BA538;
        color: #fff;
    }

    #tileViewContainer .allocation-r1 {
        /*background: #A9C23F;*/
        background: #6BA538;
        color: #fff;
    }

    #tileViewContainer .allocation-r2 {
        background: #FFC100;
        color: #000;
    }

/*    #tileViewContainer .allocation-r3 {
        background: #f8ac4a;
    }
*/
    #tileViewContainer .allocation-r3 {
        background: #FF3535;
        color: #ffffff;
    }

    #tileViewContainer .allocation-c0 {
        background: #ffffff;
        color: #000;
    }

    #tileViewContainer .allocation-c1 {
        background: #fcf7b5;
        color: #fff;
    }

    #tileViewContainer .allocation-c2 {
        background: #f8ac4a;
        color: #000;
    }

    #tileViewContainer .allocation-block {
        height: 59px;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius:7px;
    }

        #tileViewContainer .allocation-block .timesheet {
            position: absolute;
            top: 1px;
            left: 85%;
            cursor: pointer;
        }

    #tileViewContainer .dx-tile {
        border: none; 
    }

    #tileViewContainer .capacitymain {
        border-top: 1px solid #c3c3c3;
        overflow: hidden;
        border-bottom-left-radius: 5px;
        border-bottom-right-radius: 5px;
    }

    .preconCellStyle {
    background-color: #52BED9;
    border: 5px solid #fff !important;
    font-weight: 500;
}

.constCellStyle {
    color: #fff;
    background-color: #005C9B;
    border: 5px solid #fff !important;
}

.closeoutCellStyle {
    color: #fff;
    background-color: #351B82;
    border: 5px solid #fff !important;
}

.noDateCellStyle {
    color: #000000;
    background-color: #D6DAD9;
    border: 5px solid #fff !important;
}
.v-align {
    vertical-align: middle !important;
}
.schedule-label {
    font-size: 15px;
    font-weight: 500;
    width: 170px;
}

.preconborderbox {
    border-style: dashed;
    border-width: 1.5px;
    border-radius: 5px;
    border-color: #52BED9;
}

.constborderbox {
    border-style: dashed;
    border-width: 1.5px;
    border-radius: 5px;
    border-color: #005C9B;
}

.closeoutborderbox {
    border-style: dashed;
    border-width: 1.5px;
    border-radius: 5px;
    border-color: #351B82;
}
.d-flex-modified {
    display: flex;
    justify-content: space-between;
    align-items: center;
}
 .Precon_Btn {
     color: white;
     background-color: #52BED9 !important;
 }

 .Const_Btn {
     color: white;
     background-color: #005C9B !important;
 }

 .Closeout_Btn {
     color: white;
     background-color: #351B82 !important;
 }
 /*.dx-button-mode-contained.dx-state-focused {
    background-color: #4fa1d6 !important;
    border-color: #ddd;
    opacity:0.7;
}*/


</style>

<div>

    <div id="msgSuccess" class="isa_success" style="display: none;">
        <i class="fa fa-check"></i>
        All Allocation Saved Successfully.
    </div>
    <div id="divCalendar">
    </div>
    <div class="d-flex-modified">
        <div style="display: flex; margin-top: 27px;" class="text-center allocdatesbtn">
            <div class="col-md-4 px-1">
                <div id="btnPrecondate" style="display: none;"></div>
                <div class="schedule-label preconborderbox" style="color: #52BED9;">Precon Dates</div>
                <p id="pPrecon"></p>
            </div>
            <div class="col-md-4 px-1">
                <div id="btnConstructionDate" style="display: none;"></div>
                <div class="schedule-label constborderbox" style="color: #005C9B;">Const. Dates</div>
                <p id="pConst"></p>
            </div>
            <div class="col-md-4 px-1">
                <div id="btnCloseoutDate" style="max-width: initial !important; display: none;"></div>
                <div class="schedule-label closeoutborderbox" style="color: #351B82;">Closeout Dates</div>
                <p id="pCloseout"></p>
            </div>
        </div>
        <div id="btnAddNew" class="btnAddNew"></div>
    </div>
    <div id="gridTemplateContainer" class="grid-template-container" style="width: 100%; float: left; display: none;">
    </div>
    <div id="compactGridTemplateContainer" class="grid-template-container pt-3" style="float: left; clear: both;">
    </div>
    <div class="marcal">
        <%--        <div id="btnAddNew" class="btnProceed mr-1" style="display:inline-block;float:left;"></div>--%>
        <div id="btnDetailedSummary" class="btnProceed mr-1" style="display: inline-block; float: left;"></div>
        <div id="btnShowProjectTeam" class="btnProceed mr-1" style="display: inline-block; float: left;"></div>
        <div id="btnAutofillAllocations" class="btnProceed mr-1" style="display: inline-block; float: left;"></div>
        <div id="btnCancel" class="btnProceed ml-1" style="display: inline-block; float: right;"></div>
        <div id="btnProceed" class="btnProceed ml-1" style="display: inline-block; float: right;"></div>
    </div>
</div>
<div id="toastwarning"></div>
<div id="loadPanel"></div>
<div id="popupContainer">
</div>
<div id="buttonContainer"></div>
<div id="InternalAllocationGridDialog"></div>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var radioGroupItems = [
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 },
        { text: "All Resources", value: 2 }
    ];
    var checkboxGroupItems = [

        { text: "1", value: "Complexity", checked: true },
        { text: "2", value: "Count" },
        { text: "3", value: "Voulme" }
    ];
    
    var GroupsData = [];
    var popupFilters = {};  
    var IsFirstRequest = false;
    var JobTitleData = [];
    var UserProfileData = [];
    var TemplateAllocationData = [];
    var CompactTempData = [];
    var DatesModel = [];
    var globaldata = [];
    var PreconStartDate = null;
    var PreconEndDate = null;
    var ConstStartDate = null;
    var ConstEndDate = null;
    var CloseoutStartDate = null;
    var CloseoutEndDate = null;
    var strSelectedAllocationIDs = "";
    var selectionObject;
    var projectID = "<%= Request["projectID"]%>";
    $(function () {
        var RowId = 0;
        var AssignedToName = "";
        var resultData = [];
        var typename;
        var rowValidation = true;
        $("#toastwarning").dxToast({
            message: "Please select at least one record. ",
            type: "warning",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        $("#btnAddNew").dxButton({
            text: "Add New Allocation",
            focusStateEnabled: false,
            onClick: function (s, e) {
                var projectStartdate = PreconStartDate;
                var projectEnddate = CloseoutEndDate;
                var sum = 0;
                if ($("#gridTemplateContainer").is(":visible")) {
                    projectStartdate = undefined;
                    projectEnddate = undefined;
                }
                sum = globaldata.length + 1;
                globaldata.push({ "ID": -Math.abs(sum), ProjectID: projectID, "AssignedTo": "User" + sum, "AssignedToName": "", "AllocationStartDate": projectStartdate, "AllocationEndDate": projectEnddate, "PctAllocation": 100, "SoftAllocation": false, "NonChargeable": 0, "Type": 'TYPE-' + sum, "TypeName": '', "Tags": '' });
                CheckPhaseConstraints(false);
                var grid = $("#gridTemplateContainer").dxDataGrid("instance");
                grid.option("dataSource", globaldata);

                var compactgrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                compactgrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel, true));
            }
        });

        $("#btnShowProjectTeam").dxButton({
            text: "Show Team Template",
            visible: true,
            onClick: function (e) {
                if (globaldata == null || globaldata.length == 0) {
                    DevExpress.ui.dialog.alert("Please add an allocation to view the team template.", "Error!");
                }
                else {
                    $("#loadPanel").dxLoadPanel({
                        hideOnOutsideClick: true,
                        visible: true,
                        message: 'Loading...',
                        hideOnOutsideClick: false
                    });
                    $.post("/api/rmmapi/ShowProjectTeam", { Allocations: globaldata, TemplateID: <%=TemplateID%> }).then(function (response) {
                        globaldata.forEach(function (item, index) {
                            var responseItem = response.filter(x => x.ID == item.ID);
                            if (responseItem.length > 0) {
                                item.AssignedTo = responseItem[0].AssignedTo;
                                item.AssignedToName = responseItem[0].AssignedToName;
                            }
                        });
                        var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel));
                        dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", globaldata);
                    }, function (error) { }).done(function () { $("#loadPanel").dxLoadPanel("hide"); }).fail(function () { $("#loadPanel").dxLoadPanel("hide"); });
                }
            }
        });

        $("#btnAutofillAllocations").dxButton({
            text: "Autofill Allocations",
            visible: true,
            onClick: function (e) {
                IsFirstRequest = false;
                if (globaldata == null || globaldata.length == 0) {
                    DevExpress.ui.dialog.alert("Please add an allocation to autofill the team template.", "Error!");
                }
                else {
                    autofillAllocations();
                }
            }
        });

        $("#btnProceed").dxButton({
            text: "Apply Template",
            onClick: function (e) {
                var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                //rowValidation = true;
                $.each(globaldata, function (i, s) {
                    if (s.TypeName == '') {
                        isEmptyField = true;
                        DevExpress.ui.dialog.alert("Role is Required", "Error!");
                        rowValidation = false;
                        return false;
                    }

                    if (typeof (s.AllocationStartDate) == "object") {
                        if (s.AllocationStartDate != null) {
                            s.AllocationStartDate = (s.AllocationStartDate).toISOString();
                        }
                        else {
                            isEmptyField = true;
                            DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                            rowValidation = false;
                            return false;
                        }
                    } else if (s.AllocationStartDate == '') {
                        isEmptyField = true;
                        DevExpress.ui.dialog.alert("Start Date should not be blank.", "Error!");
                        rowValidation = false;
                        return false;
                    }

                    if (typeof (s.AllocationEndDate) == "object") {
                        if (s.AllocationEndDate != null) {
                            s.AllocationEndDate = (s.AllocationEndDate).toISOString();
                        }
                        else {
                            isEmptyField = true;
                            DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                            rowValidation = false;
                            return false;
                        }
                    } else if (s.AllocationEndDate == '') {
                        isEmptyField = true;
                        DevExpress.ui.dialog.alert("End Date should not be blank.", "Error!");
                        rowValidation = false;
                        return false;
                    }

                    if (new Date(s.AllocationEndDate) < new Date(s.AllocationStartDate)) {
                        isEmptyField = true;
                        DevExpress.ui.dialog.alert("Start Date should be less than End Date.", "Error!");
                        rowValidation = false;
                        return false;
                    }
                    if (s.AssignedTo.startsWith("User") || s.AssignedToName == '') {
                        s.AssignedTo = '';
                    }
                });

                if (!rowValidation) {
                    //DevExpress.ui.dialog.alert("Please fill all the required values.", "Error!");
                    return false;
                }


                var projectStartdate = DatesModel.PreconStart;
                var projectEnddate = DatesModel.ConstEnd;
                if (globaldata == null || globaldata.length == 0) {
                    DevExpress.ui.dialog.alert("No allocation to save.", "Error!");
                }
                else {
                    $.get("/api/rmmapi/GetProjectAllocations?projectID=<%= ProjectID %>", function (response, status) {
                        if (response.Allocations.length > 0) {

                            var myDialog = DevExpress.ui.dialog.custom({
                                title: "Confirm",
                                messageHtml: "To override the existing allocations, click on the <b>'Replace'</b> button.<br/> To add allocations to the existing allocations, click on the <b>'Add'</b> button.",
                                buttons: [{
                                    text: "Add",
                                    onClick: function (e) {
                                        return { buttonText: e.component.option("text") }
                                    }
                                },
                                {
                                    text: "Replace",
                                    onClick: function (e) {
                                        return { buttonText: e.component.option("text") }
                                    }
                                },
                                {
                                    text: "Close",
                                    onClick: function (e) {
                                        return { buttonText: e.component.option("text") }
                                    }
                                }
                                ]
                            });
                            myDialog.show().done(function (dialogResult) {
                                console.log(dialogResult.buttonText);
                                if (dialogResult.buttonText == "Replace") {
                                    UpdateFromTemplateAllocation(projectStartdate, projectEnddate, true);
                                } else if (dialogResult.buttonText == "Add") {
                                    UpdateFromTemplateAllocation(projectStartdate, projectEnddate, false);
                                }
                            });
                        } else {
                            UpdateFromTemplateAllocation(projectStartdate, projectEnddate, true);
                        }
                    });
                }

            }
        });

        $("#btnCancel").dxButton({
            text: "Cancel",
            onClick: function (e) {
                window.parent.CloseWindowCallback(0, document.location.href);
            }
        });

        $("#btnPrecondate").dxButton({
            text: "Select Precon Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Precon_Btn'
            },
            onClick: function (e) {
                var startDate = PreconStartDate;
                var EndDate = PreconEndDate;
                updateDatesInGrid(startDate, EndDate, 'black');
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });
        $("#btnConstructionDate").dxButton({
            text: "Select Const. Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Const_Btn'
            },
            onClick: function (e) {
                var startDate = ConstStartDate;
                var EndDate = ConstEndDate;
                updateDatesInGrid(startDate, EndDate, '#fff');
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });

        $("#btnCloseoutDate").dxButton({
            text: "Select Closeout Dates",
            icon: "/content/Images/RMONE/calender_activeWhite.png",
            focusStateEnabled: false,
            elementAttr: {
                class: 'Closeout_Btn'
            },
            onClick: function (e) {
                var startDate = CloseoutStartDate;
                var EndDate = CloseoutEndDate;
                updateDatesInGrid(startDate, EndDate, '#fff');
                $('#btnCancelChanges').dxButton({ visible: true });
            }
        });

        $("#btnDetailedSummary").dxButton({
            text: "Detailed View",
            focusStateEnabled: false,
            visible: true,
            onClick: function (e) {
                CheckPhaseConstraints(false);
                CompactPhaseConstraintsTemplate(globaldata, DatesModel);
                if ($("#gridTemplateContainer").is(":visible")) {
                    $("#gridTemplateContainer").hide();
                    $("#compactGridTemplateContainer").show();
                    $("#btnDetailedSummary span").text("Detailed View");
                    $("#btnPrecondate, #btnConstructionDate, #btnCloseoutDate").hide();
                    $(".schedule-label").show();
                    $("#btnSaveAsTemplate").hide();
                    //$("#btnAddNew").show();
                    var compactgrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                    compactgrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel));
                }
                else {
                    $("#compactGridTemplateContainer").hide();
                    $("#gridTemplateContainer").show();
                    $("#btnDetailedSummary span").text("Summary View");
                    //$("#btnAddNew").hide();
                    $("#btnPrecondate, #btnConstructionDate, #btnCloseoutDate").show();
                    $(".schedule-label").hide();
                    var compactgrid = $("#gridTemplateContainer").dxDataGrid("instance");
                    compactgrid.option("dataSource", globaldata);
                }
            }
        });

        $.get("/api/rmmapi/GetTemplateAllocations?id=" + <%=TemplateID%>+"&projectID=<%= ProjectID %>" + "&StartDate=" + '<%= StartDate%>' + "&EndDate=" + '<%= EndDate%>', function (data, status) {

            globaldata = data;
            globaldata.forEach(function (data, index) {
                if (data.AssignedTo == '') {
                    let internaldata = globaldata.filter(x => x.Type == data.Type && x.AssignedTo != '')[0];
                    if (internaldata != null && internaldata.AssignedTo != '') {
                        data.AssignedTo = internaldata.AssignedTo;
                    }
                    else {
                        data.AssignedTo = "User" + index;
                    }
                }
                if (data.ID < 0) {
                    data.ID = -2000 + index;
                }
                data.AssignedToName = "";
            });
            $.get("/api/rmone/GetProjectDates?TicketId=<%= ProjectID %>", function (datesData, status) {
                DatesModel = datesData;
                PreconStartDate = new Date(DatesModel.PreconStart);
                PreconEndDate = new Date(DatesModel.PreconEnd);
                ConstStartDate = new Date(DatesModel.ConstStart);
                ConstEndDate = new Date(DatesModel.ConstEnd);
                CloseoutStartDate = new Date(DatesModel.CloseoutStart);
                CloseoutEndDate = new Date(DatesModel.Closeout);
                $("#pPrecon").text(PreconStartDate.format("MMM d, yyyy") + " To " + PreconEndDate.format("MMM d, yyyy"));
                $("#pConst").text(ConstStartDate.format("MMM d, yyyy") + " To " + ConstEndDate.format("MMM d, yyyy"));
                $("#pCloseout").text(CloseoutStartDate.format("MMM d, yyyy") + " To " + CloseoutEndDate.format("MMM d, yyyy"));
                CheckPhaseConstraints(false);
                TemplateAllocationData = CompactPhaseConstraintsTemplate(globaldata, DatesModel, true);
                CompactTempData = TemplateAllocationData;
                $("#compactGridTemplateContainer").dxDataGrid({
                    dataSource: CompactTempData,
                    ID: "grdTemplate",
                    editing: {
                        mode: "cell",
                        allowEditing: true,
                        allowUpdating: true
                    },
                    columns: [
                        {
                            dataField: "AssignedToName",
                            dataType: "text",
                            caption: "Assigned To",
                            allowEditing: false,
                            sortIndex: "0",
                            sortOrder: "asc",
                            cellTemplate: function (container, options) {
                                if (options.key.ID) {
                                    options.data.TemplateID = '<%=TemplateID%>';
                                    options.data.ProjectID = '<%=ProjectID%>';
                                    var element = _.findWhere(globaldata, { ID: options.data.ID });
                                    if (!element) {

                                        globaldata.push(options.data);
                                    }
                                    var str = options.data.AssignedTo + "','" + options.data.AssignedToName;
                                    var strwithspace = str.replace(" ", "&nbsp;");

                                    var resource = options.value;

                                    if (options.data.IsResourceOverAllocated == true)
                                        resource = "<span style='font-weight:500'>" + options.value + "</span>";
                                    else if (options.data.IsResourceDisabled == true)
                                        resource = "<span style='color:#FF0000'>" + options.value + "</span>";


                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='#' onclick=openResourceTimeSheet('" + strwithspace + "');>" + resource + "</a></span>")
                                        .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            caption: "Role",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "24%",
                            cssClass: "cls"
                        },

                        {
                            dataField: "Type",
                            dataType: "text",
                            visible: false
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            dataType: "date",
                            width: "10%",
                            alignment: 'center',
                            cssClass: "v-align",
                            allowEditing: false,
                            validationRules: [{ type: "required", message: "" }],
                            format: 'MMM d, yyyy',
                            editorOptions: {
                                onFocusOut: function (e) {
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                }
                            }
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            dataType: "date",
                            width: "10%",
                            alignment: 'center',
                            cssClass: "v-align",
                            allowEditing: false,
                            validationRules: [{ type: "required", message: "" }],
                            format: 'MMM d,yyyy',
                            editorOptions: {
                                onFocusOut: function (e) {
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                }
                            }
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Precon",
                            dataType: "text",
                            width: "6%",
                            cellTemplate: function (container, options) {
                                if (options.data.preconRefIds != null) {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.preconRefIds + "');>" + options.data.PctAllocation + "</a></span>")
                                        .appendTo(container);
                                }
                                else {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocation + "</span>")
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "PctAllocationConst",
                            caption: "% Const.",
                            dataType: "text",
                            cssClass: "v-align",
                            width: "6%",
                            cellTemplate: function (container, options) {

                                if (options.data.constRefIds != null) {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.constRefIds + "');>" + options.data.PctAllocationConst + "</a></span>")
                                        .appendTo(container);
                                }
                                else {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationConst + "</span>")
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "PctAllocationCloseOut",
                            caption: "% Closeout",
                            dataType: "text",
                            cssClass: "v-align",
                            width: "6%",
                            cellTemplate: function (container, options) {

                                if (options.data.closeOutRefIds != null) {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=OpenInternalAllocation('" + options.data.closeOutRefIds + "');>" + options.data.PctAllocationCloseOut + "</a></span>")
                                        .appendTo(container);
                                }
                                else {
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'>" + options.data.PctAllocationCloseOut + "</span>")
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            width: "8%",
                            fieldName: "SoftAllocation",
                            name: 'SoftAllocation',
                            caption: "",
                            dataType: 'text',
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                if (isAllAllocationsSame(options)) {
                                    $("<div id='divSoftAllocation'>").append(
                                        $("<div id='divSwitch' />").dxSwitch({
                                            switchedOffText: 'Hard',
                                            switchedOnText: 'Soft',
                                            width: 60,
                                            value: options.data.SoftAllocation,
                                            onValueChanged(data) {

                                            },
                                        })
                                    ).appendTo(container);
                                } else {
                                    $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;margin-top:4px;' ><span style='cursor:pointer;' onclick=$('#btnDetailedSummary').click()>Mixed</span></div>").appendTo(container);
                                }
                            },

                        },
                        {
                            width: "6%",
                            fieldName: "NonChargeable",
                            caption: 'NCO',
                            dataType: 'text',
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                if (isAllNCOAllocationsSame(options)) {
                                    $("<div id='divNonChargeable'>").append(
                                        $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                            width: 30,
                                            value: options.data.NonChargeable,
                                            onValueChanged(data) {

                                            },
                                        })
                                    ).appendTo(container);
                                } else {
                                    $("<div id='divSoftAllocation' style='padding-left:10px;padding-right:10px;margin-top:4px;' ><span style='cursor:pointer;' onclick=$('#btnDetailedSummary').click()>Mixed</span></div>").appendTo(container);
                                }
                            },
                        },
                        {
                            width: "5%",
                            cellTemplate: function (container, options) {
                                $("<div id='rowDelete' style='text-align:center;'>")
                                    .append($("<img>", { "src": "/content/images/deleteIcon-new.png", "ID": options.data.IdsForDelete, "TemplateID": options.data.TemplateID, "DeleteTemplate": options.data.Name, "style": "overflow: auto;cursor: pointer;", "class": "imgDelete", "width": "22px" }))
                                    .appendTo(container);
                            }
                        }

                    ],
                    showBorders: true,
                    showRowLines: true,
                    onRowUpdating: function (e) {
                        SaveToGlobalData(e);
                    },
                    onCellClick: function (e) {
                        let compactElement = _.findWhere(CompactTempData, { ID: parseInt(e.data.ID) });
                        let uniqueIds = [...new Set(compactElement.IdsForDelete.split(';'))];
                        if (e.column.fieldName == "SoftAllocation") {
                            if (isAllAllocationsSame(e)) {
                                CompactTempData.forEach(function (part, index, theArray) {
                                    if (part.ID == e.data.ID) {
                                        part.SoftAllocation = !part.SoftAllocation;
                                    }
                                });
                                globaldata.forEach(function (part, index, theArray) {
                                    if (uniqueIds.includes(String(part.ID))) {
                                        part.SoftAllocation = !part.SoftAllocation;
                                    }
                                });
                            }
                        }
                        if (e.column.fieldName == "NonChargeable") {
                            if (isAllNCOAllocationsSame(e)) {
                                CompactTempData.forEach(function (part, index, theArray) {
                                    if (part.ID == e.data.ID) {
                                        part.NonChargeable = !part.NonChargeable;
                                    }
                                });
                                globaldata.forEach(function (part, index, theArray) {
                                    if (uniqueIds.includes(String(part.ID))) {
                                        part.NonChargeable = !part.NonChargeable;
                                    }
                                });
                            }
                        }
                    },
                    onContentReady: function (e) {
                        if ('<%= Option%>' == 'AutofillAllocations') {
                            e.component.columnOption("command:edit", "visibleIndex", -1);
                            autofillAllocations();
                        }
                        else {
                            if (!IsFirstRequest) {
                                for (var i in globaldata) {
                                    if (!globaldata[i]['AssignedTo'].startsWith("User")) {
                                        //globaldata[i]['AssignedTo'] = '';
                                        globaldata[i]['AssignedToName'] = '';
                                    }
                                }
                                IsFirstRequest = true;
                            }
                        }
                    },
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = new Date(DatesModel.PreconStart);
                            var conststartDate = new Date(DatesModel.ConstStart);
                            var constEndDate = new Date(DatesModel.ConstEnd);
                            var closeoutstartDate = new Date(DatesModel.CloseoutStart);

                            if (e.column.dataField == 'AllocationStartDate') {
                                let cellValue = new Date(e.data.AllocationStartDate)
                                let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                e.cellElement.addClass(className);
                            }
                            if (e.column.dataField == 'AllocationEndDate') {
                                let cellValue = new Date(e.data.AllocationEndDate)
                                let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                e.cellElement.addClass(className);
                            }
                        }
                    },
                    onEditorPreparing: function (e) {
                        if (e.parentType === 'dataRow' && e.dataField === 'TypeName') {

                            e.editorElement.dxSelectBox({
                                searchEnabled: true,
                                dataSource: GroupsData,
                                valueExpr: "Id",
                                displayExpr: "Name",
                                value: e.row.data.Type,
                                onValueChanged: function (ea) {
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    $.each(ea.component._dataSource._items, function (i, v) {
                                        if (v.Id === ea.value) {
                                            e.setValue(v.Name);
                                            dataGrid.getDataSource()._items[e.row.rowIndex].Type = v.Id;
                                            dataGrid.getDataSource()._items[e.row.rowIndex].TypeName = v.Name;
                                            let allocData = globaldata.filter(x => x.AssignedTo == e.row.data.AssignedTo && x.Type == e.row.data.Type);
                                            $.each(allocData, function (key, value) {
                                                value.Type = v.Id;
                                                value.TypeName = v.Name;
                                            });
                                        }
                                    });
                                    dataGrid.saveEditData();
                                }
                            });
                            e.cancel = true;

                        }
                        if (e.dataField == "AssignedToName" && e.parentType == "dataRow") {
                            e.editorOptions.disabled = true;
                        }
                        if (e.parentType == "dataRow" && e.dataField == "PctAllocation") {
                            if (e.row.key.preconRefIds != null) {
                                e.editorOptions.disabled = true;
                            }
                        }
                        if (e.parentType == "dataRow" && e.dataField == "PctAllocationConst") {
                            if (e.row.key.constRefIds != null) {
                                e.editorOptions.disabled = true;
                            }
                        }
                        if (e.parentType == "dataRow" && e.dataField == "PctAllocationCloseOut") {
                            if (e.row.key.closeOutRefIds != null) {
                                e.editorOptions.disabled = true;
                            }
                        }
                    },
                    onRowValidating: function (e) {

                        if (typeof e.newData.AllocationEndDate !== "undefined") {
                            if (e.newData.AllocationStartDate) {
                                if (new Date(e.newData.AllocationEndDate) < new Date(e.newData.AllocationStartDate)) {
                                    e.isValid = false;
                                    e.errorText = "StartDate should be less then EndDate";
                                }
                            } else if (e.oldData.AllocationStartDate) {
                                if (new Date(e.newData.AllocationEndDate) < new Date(e.oldData.AllocationStartDate)) {
                                    e.isValid = false;
                                    e.errorText = "StartDate should be less then EndDate";
                                }
                            }
                        }

                        if (typeof e.newData.AllocationStartDate !== "undefined") {
                            if (e.newData.AllocationEndDate) {
                                if (new Date(e.newData.AllocationStartDate) > new Date(e.newData.AllocationEndDate)) {
                                    e.isValid = false;
                                    e.errorText = "StartDate should be less then EndDate";
                                }
                            } else if (e.oldData.AllocationEndDate) {
                                if (new Date(e.newData.AllocationStartDate) > new Date(e.oldData.AllocationEndDate)) {
                                    e.isValid = false;
                                    e.errorText = "StartDate should be less then EndDate";
                                }
                            }
                        }

                        rowValidation = e.isValid;
                    }
                });

                $("#gridTemplateContainer").dxDataGrid({
                    //columnHidingEnabled: true,
                    dataSource: globaldata,
                    ID: "grdTemplate",
                    editing: {
                        mode: "cell",
                        allowEditing: true,
                        allowUpdating: true
                    },
                    sorting: {
                        mode: "multiple" // or "multiple" | "none"
                    },
                    scrolling: {
                        mode: 'Standard',
                    },
                    paging: { enabled: false },
                    columns: [
                        {
                            dataField: "AssignedToName",
                            dataType: "text",
                            caption: "Assigned To",
                            //allowEditing: false,
                            sortIndex: "0",
                            sortOrder: "asc",
                            cellTemplate: function (container, options) {
                                $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                                $('.dx-data-row').addClass('devExtDataGrid-DataRow');
                                if (options.key.ID > 0) {

                                    var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                                    var strwithspace = str.replace(/ /g, "&nbsp;")
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                            + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                        .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                        .appendTo(container);
                                }

                                if (options.key.ID <= 0) {
                                    var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                                    var strwithspace = str.replace(" ", "&nbsp;")
                                    $("<div id='dataId'>")
                                        .append("<span style='float: left;overflow: auto;'><a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                            + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                        .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                        .appendTo(container);
                                }
                            }
                        },
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            caption: "Role",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "30%",
                            cssClass: "cls",
                        },
                        {
                            dataField: "Type",
                            dataType: "text",
                            visible: false
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            dataType: "date",
                            width: "10%",
                            alignment: 'center',
                            cssClass: "v-align",
                            validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            sortIndex: "2",
                            sortOrder: "asc",
                            editorOptions: {
                                onFocusOut: function (e, options) {
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                            },
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            dataType: "date",
                            alignment: 'center',
                            cssClass: "v-align",
                            width: "10%",
                            validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            editorOptions: {
                                onFocusOut: function (e) {
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                            }
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Alloc",
                            dataType: "text",
                            width: "5%",
                            setCellValue: function (newData, value, currentRowData) {
                                if (parseInt(value) <= 0) {
                                    globaldata = globaldata.filter(x => x.ID != currentRowData.ID);
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.option("dataSource", globaldata);
                                }
                                else {
                                    newData.PctAllocation = value;
                                }
                            }
                        },
                        {
                            width: "8%",
                            fieldName: "SoftAllocation",
                            name: 'SoftAllocation',
                            caption: "",
                            dataType: "text",
                            alignment: 'center', 
                            cellTemplate: function (container, options) {

                                $("<div id='divSoftAllocation' >").append(
                                    $("<div id='divSwitch' />").dxSwitch({
                                        switchedOffText: 'Hard',
                                        switchedOnText: 'Soft',
                                        width: 60,
                                        value: options.data.SoftAllocation,
                                        onValueChanged(data) {

                                        },
                                    })
                                ).appendTo(container);
                            },

                        },
                        {
                            width: "6%",
                            fieldName: "NonChargeable",
                            caption: 'NCO',
                            dataType: 'text',
                            alignment: 'center',
                            cellTemplate: function (container, options) {
                                $("<div id='divNonChargeable' >").append(
                                    $("<div id='divSwitchNonChargeable' />").dxCheckBox({
                                        //switchedOffText: 'BillHr',
                                        //switchedOnText: 'NCO',
                                        width: 30,
                                        value: options.data.NonChargeable,
                                        onValueChanged(data) {

                                        },
                                    })
                                ).appendTo(container);
                            },
                        },
                        {
                            width: "5%",
                            cellTemplate: function (container, options) {
                                var preconStartdate = new Date(DatesModel.PreconStart);
                                var preconEnddate = new Date(DatesModel.PreconEnd);
                                if (preconEnddate == 'Jan 1, 0001' || preconStartdate == 'Jan 1, 0001') {
                                    $("<div id='rowDelete' style='text-align:center;'>")
                                        .append($("<img>", {
                                            "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                            "UserID": options.data.AssignedTo, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                            "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                            "style": "overflow: auto;cursor: pointer;", "class": "imgDelete", "title": "Delete", "width": "23px"
                                        }))
                                        .appendTo(container);
                                } else {
                                    $("<div id='rowDelete' style='text-align:center;'>")
                                        //.append($("<img>", { "src": "/Content/images/PreconCal2.png", "Index": options.rowIndex, "style": "overflow: auto;cursor: pointer;height:20px;width:20px;", "class": "imgPreconDate", "title":"Set Precon Date" }))
                                        .append($("<img>", {
                                            "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, "TemplateID": options.data.TemplateID,
                                            "UserID": options.data.AssignedTo, "TypeName": options.data.TypeName, "Tags": options.data.Tags,
                                            "StartDate": options.data.AllocationStartDate, "EndDate": options.data.AllocationEndDate,
                                            "style": "overflow: auto;cursor: pointer;", "class": "imgDelete", "title": "Delete", "width": "23px"
                                        }))
                                        .appendTo(container);
                                }
                            }
                        },
                    ],
                    showBorders: true,
                    showRowLines: true,
                    selection: {
                        mode: 'multiple',
                        showCheckBoxesMode: 'always',
                    },
                    onSelectionChanged(e) {
                        selectionObject = e;
                        const data = e.selectedRowsData;
                        strSelectedAllocationIDs = "";

                        $.each(data, function (i, item) {
                            if (item.ID)
                                strSelectedAllocationIDs += item.ID + ",";
                        });
                    },
                    onRowUpdating: function (e) {
                        //lastEditedRow = e.key.ID;
                        //openDateConfirmationDialog();
                    },
                    onCellClick: function (e) {
                        if (e.column.fieldName == "SoftAllocation") {
                            globaldata.forEach(function (part, index, theArray) {
                                if (part.ID == e.data.ID) {
                                    part.SoftAllocation = !part.SoftAllocation;
                                }
                            });
                        }
                        if (e.column.fieldName == "NonChargeable") {
                            globaldata.forEach(function (part, index, theArray) {
                                if (part.ID == e.data.ID) {
                                    part.NonChargeable = !part.NonChargeable;
                                }
                            });
                        }

                    },
                    onContentReady: function (e) {
                        var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        if (dataGrid.getDataSource() !== null)
                            globaldata = dataGrid.getDataSource()._items;

                        //clickUpdateSize();
                    },
                    toolbar: function (e) {
                        e.toolbarOptions.visible = false;
                    },
                    onEditorPreparing: function (e) {
                        if (e.parentType === 'dataRow' && e.dataField === 'TypeName') {

                            e.editorElement.dxSelectBox({
                                dataSource: GroupsData,
                                valueExpr: "Id",
                                displayExpr: "Name",
                                value: e.row.data.Type,
                                searchEnabled: true,
                                onValueChanged: function (ea) {
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = '';
                                    $.each(ea.component._dataSource._items, function (i, v) {
                                        if (v.Id === ea.value) {
                                            e.setValue(v.Name);
                                            dataGrid.getDataSource()._items[e.row.rowIndex].Type = v.Id;
                                            dataGrid.getDataSource()._items[e.row.rowIndex].TypeName = v.Name;
                                        }
                                    });
                                    dataGrid.saveEditData();
                                }
                            });
                            e.cancel = true;

                        }
                    },
                    onRowValidating: function (e) {
                        if (typeof e.newData.AllocationEndDate !== "undefined") {
                            let value = new Date(e.newData.AllocationEndDate);
                            if (value.format() == 'Invalid Date' || String(value.getFullYear()).length > 4) {
                                let newdate = convertToValidDate(e.newData.AllocationEndDate);
                                if (newdate == 'Invalid year') {
                                    e.isValid = false;
                                    e.errorText = "Please enter a valid year in format YYYY.";
                                }
                                else {
                                    e.isValid = false;
                                    e.errorText = "Please enter a valid End date.";
                                }
                            }
                            if (typeof value != undefined) {
                                let yearpart = removeLeadingZeros(value.getFullYear());
                                let monthpart = value.getMonth();
                                let daypart = value.getDate();
                                if (isTwoDigitNumber(yearpart)) {
                                    let firsttwodigits = getFirstTwoDigitsOfYear(new Date());
                                    let newyearpart = parseInt(firsttwodigits.toString() + yearpart.toString())
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    var rowIndex = e.component.getRowIndexByKey(e.key);
                                    e.component.cellValue(rowIndex, "AllocationEndDate", new Date(newyearpart, monthpart, daypart));
                                    dataGrid.saveEditData();
                                }
                                else {
                                    if (yearpart < 1000) {
                                        e.isValid = false;
                                        e.errorText = "Please enter a valid year in format YYYY.";
                                    }
                                }

                            }

                        }
                        if (typeof e.newData.AllocationStartDate !== "undefined") {
                            let value = new Date(e.newData.AllocationStartDate);
                            if (value.format() == 'Invalid Date' || String(value.getFullYear()).length > 4) {
                                let newdate = convertToValidDate(e.newData.AllocationStartDate);
                                if (newdate == 'Invalid year') {
                                    e.isValid = false;
                                    e.errorText = "Please enter a valid year in format YYYY.";
                                }
                                else {
                                    e.isValid = false;
                                    e.errorText = "Please enter a valid Start date.";
                                }
                            }
                            if (typeof value != undefined) {
                                let yearpart = removeLeadingZeros(value.getFullYear());
                                let monthpart = value.getMonth();
                                let daypart = value.getDate();
                                if (isTwoDigitNumber(yearpart)) {

                                    let firsttwodigits = getFirstTwoDigitsOfYear(new Date());
                                    let newyearpart = parseInt(firsttwodigits.toString() + yearpart.toString())
                                    var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                                    var rowIndex = e.component.getRowIndexByKey(e.key);
                                    e.component.cellValue(rowIndex, "AllocationStartDate", new Date(newyearpart, monthpart, daypart));
                                    dataGrid.saveEditData();
                                }
                                else {
                                    if (yearpart < 1000) {
                                        e.isValid = false;
                                        e.errorText = "Please enter a valid year in format YYYY.";
                                    }
                                }
                            }
                        }
                        $.cookie("dataChanged", 1, { path: "/" });
                        $.cookie("projTeamAllocSaved", 0, { path: "/" });
                        $('#btnCancelChanges').dxButton({ visible: true });
                        isGridInValidState = e.isValid;
                    },
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = new Date(DatesModel.PreconStart);
                            var conststartDate = new Date(DatesModel.ConstStart);
                            var constEndDate = new Date(DatesModel.ConstEnd);
                            var closeoutstartDate = new Date(DatesModel.CloseoutStart);

                            if (e.column.dataField == 'AllocationStartDate') {

                                let cellValue = new Date(e.data.AllocationStartDate)
                                let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                e.cellElement.addClass(className);

                            }
                            if (e.column.dataField == 'AllocationEndDate') {
                                let cellValue = new Date(e.data.AllocationEndDate)
                                let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                e.cellElement.addClass(className);
                            }
                        }
                    }
                });
            });
        });



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
                titleView = $("<div id='tileViewContainer' style='clear:both;padding-top: 10px' />").dxTileView({
                    height: window.innerHeight - 200,
                    // width: window.innerWidth - 100,
                    showScrollbar: true,
                    baseItemHeight: 65,
                    baseItemWidth: 150,
                    itemMargin: 15,
                    direction: "vertical",
                    elementAttr: { "class": "tileViewContainer" },
                    noDataText: "No resource available",
                    dataSource: "/api/rmmapi/FindResourceBasedOnGroup?" + $.param(popupFilters),
                    itemTemplate: function (itemData, itemIndex, itemElement) {
                        itemData.PctAllocation = Math.round(itemData.PctAllocation);
                        itemData.SoftPctAllocation = Math.round(itemData.SoftPctAllocation);
                        itemData.TotalPctAllocation = Math.round(itemData.TotalPctAllocation);
                        var html = new Array();
                        var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                        var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                        html.push("<div class='timesheet'><img src='/content/images/icon_three_black_dots.png' height='5px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
                        html.push("</div>");
                        html.push("<div class='UserDetails'>");
                        html.push("<div id='" + itemData.AssignedTo + "'>");
                        //   html.push("<div id='" + itemData.AssignedTo + "'>");
                        html.push("<div class='AssignedToName'>");
                        html.push(itemData.AssignedToName);
                        html.push("</div>");
                        if (itemData.PctAllocation >= 100) {
                            html.push("<div>");
                            html.push("(" + itemData.PctAllocation + "%)");
                            html.push("</div>");
                        }
                        else if (itemData.PctAllocation > 0) {
                            html.push("<div style='padding-bottom:3px;'>");
                            html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                            html.push("</div>");
                        }
                        else {
                            html.push("<div style='padding-bottom:3px;'>");
                            html.push("(0%)");
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
                        else {
                            if (itemData.SoftPctAllocation > 0) {
                                if (popupFilters.isAllocationView) {
                                    html.push("<div class='capacitymain'>");
                                <%if (ShowTotalAllocationsInSearch) { %>
                                    html.push("<div class='capacityblock cc allocation-v" + itemData.TotalVolumeRange + "'>");
                                    html.push("T: " + Number(itemData.TotalPctAllocation) + "%");
                                    html.push("</div>");
                                    html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                                <%} else { %>
                                    html.push("<div class='capacityblock-1 allocation-c" + itemData.projectCountRange + "''>");
                                <%}%>
                                    html.push("S: " + Number(itemData.SoftPctAllocation) + "%");
                                    html.push("</div>");
                                    html.push("</div>");

                                }
                                else {
                                    html.push("<div class='capacitymain'>");
                                 <%if (ShowTotalAllocationsInSearch) { %>
                                    html.push("<div class='capacityblock cc allocation-v" + itemData.TotalVolumeRange + "'>");
                                    html.push("T: " + (100 - Number(itemData.TotalPctAllocation)) + "%");
                                    html.push("</div>");
                                    html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                                <%} else { %>
                                    html.push("<div class='capacityblock-1 allocation-c" + itemData.projectCountRange + "''>");
                                <%}%>
                                    html.push("S: " + (100 - Number(itemData.SoftPctAllocation)) + "%");
                                    html.push("</div>");
                                    html.push("</div>");

                                }
                            }

                        }
                        html.push("</div>");
                        html.push("</div>");

                        itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                        itemElement.append(html.join(""));

                    },
                    onItemClick: function (e) {
                        var data = e.itemData;
                        if (!$("#gridTemplateContainer").is(":visible")) {
                            var element = _.findWhere(CompactTempData, { ID: parseInt(popupFilters.ID) });
                            let uniqueIds = [...new Set(element.IdsForDelete.split(';'))];
                            globaldata.forEach(function (part, index, theArray) {
                                if (uniqueIds.includes(String(part.ID))) {
                                    part.AssignedTo = data.AssignedTo;
                                    part.AssignedToName = data.AssignedToName;
                                    part.IsResourceDisabled = false;
                                    part.IsResourceOverAllocated = false;
                                }
                            });
                            var grid = $('#compactGridTemplateContainer').dxDataGrid('instance');
                            grid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel, true));
                        }
                        else {
                            var element = _.findWhere(globaldata, { ID: parseInt(popupFilters.ID) });
                            element.AssignedTo = data.AssignedTo;
                            element.AssignedToName = data.AssignedToName;
                            element.IsResourceDisabled = false;
                            element.IsResourceOverAllocated = false;
                            globaldata = globaldata.filter(x => x.ID != element.ID);
                            globaldata.push(element);
                            var grid = $('#gridTemplateContainer').dxDataGrid('instance');
                            grid.option("dataSource", globaldata);
                        }

                        $('#popupContainer').dxPopup('instance').hide();
                    }

                });
            }

            return titleView;
        };
        $(document).on("click", "img.assigneeToImg", function (e) {
            var groupid = $(this).attr("id");
            var dataid = e.target.id;
            var data = _.find(globaldata, function (s) { return s.ID.toString() === dataid; });
            if ($(this).attr("group") == '' || ($(this).attr("group") != '' && $(this).attr("group").startsWith("TYPE-"))) {
                DevExpress.ui.dialog.alert(`Please select the role.`, 'Error');
            }
            else {
                popupFilters.projectID = projectID;
                popupFilters.resourceAvailability = 2;
                popupFilters.complexity = true;
                popupFilters.projectVolume = false;
                popupFilters.projectCount = false;
                popupFilters.RequestTypes = false;
                popupFilters.groupID = $(this).attr("group");
                popupFilters.allocationStartDate = $(this).attr("startDate");
                popupFilters.allocationEndDate = $(this).attr("endDate");
                if (projectID.startsWith("OPM")) {
                    popupFilters.ModuleIncludes = true;
                }
                else {
                    popupFilters.ModuleIncludes = false;
                }
                popupFilters.JobTitles = "";
                popupFilters.SelectedUserID = "";
                popupFilters.ID = $(this).attr("ID");
                RowId = $(this).attr("ID");
                var popupTitle = "Available Resource";
                if (data && data.TypeName.length > 0)
                    popupTitle = "Available " + data.TypeName + "s";

                $("#popupContainer").dxPopup({
                    title: popupTitle,
                    width: "95%",
                    height: "95%",
                    visible: true,
                    scrolling: true,
                    contentTemplate: function (contentElement) {
                        contentElement.append(
                            $("<div style='float:left;padding-left:15px; padding-bottom: 10px;' />").dxRadioGroup({
                                dataSource: radioGroupItems,
                                displayExpr: "text",
                                value: _.findWhere(radioGroupItems, { value: popupFilters.resourceAvailability }),
                                layout: "horizontal",
                                onValueChanged: function (e) {
                                    popupFilters.resourceAvailability = e.value.value;
                                    bindDatapopup(popupFilters);
                                }
                            }),

                            $("<div class='chkFilterCheck' style='float:left;padding-left:15px;' />").dxCheckBox({
                                text: "Project Type",
                                onValueChanged: function (e) {
                                    popupFilters.RequestTypes = e.value;
                                    bindDatapopup(popupFilters);
                                }
                            }),

                            $("<div class='chkFilterCheck' style='float:left;padding-left:5px;' />").dxCheckBox({
                                text: "Capacity",
                                value: popupFilters.projectVolume,
                                onValueChanged: function (e) {
                                    popupFilters.projectVolume = e.value;
                                    popupFilters.projectCount = e.value;
                                    bindDatapopup(popupFilters);
                                }
                            }),

                            $("<div  id='chkComplexity' class='chkFilterCheck' style='float:left;padding-left:5px;' />").dxCheckBox({
                                text: "Complexity",
                                value: popupFilters.complexity,
                                onValueChanged: function (e) {

                                    popupFilters.complexity = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),
                            $("<div class='chkFilterCheck' style='float:left;padding-left:5px;' />").dxCheckBox({
                                text: "Opportunities",
                                value: popupFilters.ModuleIncludes,
                                onValueChanged: function (e) {
                                    popupFilters.moduleIncludes = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),
                            $("<div class='filterctrl-jobDepartment' />").dxSelectBox({
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
                            $("<div  id='tagboxTitles'  class='filterctrl-jobtitle'/>").dxTagBox({
                                text: "Job Titles",
                                placeholder: "Limit By Job Title",
                                dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DepartmentID=0",
                                maxDisplayedTags: 4,
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
                            $("<div class='filterctrl-userpicker' />").dxSelectBox({
                                placeholder: "Choose Any User",
                                valueExpr: "Id",
                                displayExpr: "Name",
                                searchEnabled: true,
                                showClearButton: true,
                                dataSource: "/api/rmmapi/GetUserList",
                                onSelectionChanged: function (selectedItems) {

                                    if (selectedItems.selectedItem == null) {
                                        popupFilters.SelectedUserID = "";
                                        popupFilters.complexity = true;
                                        var checkcomplexity = $("#chkComplexity").dxCheckBox("instance");
                                        checkcomplexity.option("value", true);
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

                    }
                });
            }
        });

        $(document).on("click", "img.imgDelete", function (e) {
            var refIds = $(this).attr("id");
            let ids = refIds.split(";");
            globaldata = globaldata.filter(x => !ids.includes(String(x.ID)));
            let dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", globaldata);
            dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
            dataGrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel));
        });

        $.get("/api/rmmapi/GetGroupsOrResource", function (data, status) {
            GroupsData = data;
        });

        function autofillAllocations() {

            if (!IsFirstRequest) {
                if (CompactTempData != null && CompactTempData.length > 0 && CompactTempData.filter(x => x.Type != null && x.Type != '').length > 0) {
                    $("#loadPanel").dxLoadPanel({
                        hideOnOutsideClick: true,
                        visible: true,
                        message: 'Finding the best resource first ...',
                        hideOnOutsideClick: false
                    });
                    IsFirstRequest = true;
                    let filteredData = CompactTempData.filter(x => x.Type != null && x.Type != '' && !x.Type.startsWith("TYPE-"));
                    $.post("/api/rmmapi/SelectMostAvailableResource", { TemplateAllocations: filteredData }).then(function (response) {
                        CompactTempData.forEach(function (item, index) {
                            var responseItem = response.filter(x => x.ID == item.ID);
                            if (responseItem.length > 0) {
                                item.AssignedTo = responseItem[0].AssignedTo;
                                item.AssignedToName = responseItem[0].AssignedToName;
                                let uniqueIds = [...new Set(item.IdsForDelete.split(';'))];
                                globaldata.forEach(function (part, index, theArray) {
                                    if (uniqueIds.includes(String(part.ID))) {
                                        part.AssignedTo = responseItem[0].AssignedTo;
                                        part.AssignedToName = responseItem[0].AssignedToName;
                                        part.IsResourceDisabled = false;
                                        part.IsResourceOverAllocated = false;
                                    }
                                });
                            }
                        });

                        var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", CompactTempData);
                        dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", globaldata);
                    }, function (error) { }).done(function () { $("#loadPanel").dxLoadPanel("hide"); }).fail(function () { $("#loadPanel").dxLoadPanel("hide"); });
                }
            }
        }
    });

    function UpdateFromTemplateAllocation(projectStartdate, projectEnddate, deleteExistingAllocations) {
        Object.keys(globaldata).forEach((key) => (globaldata[key] == null) && delete globaldata[key]);
        $("#loadPanel").dxLoadPanel({
            hideOnOutsideClick: true,
            visible: true,
            message: 'Saving Allocations...',
            hideOnOutsideClick: false
        });
        $.post("/api/rmmapi/UpdateFromTemplateAllocation", { Allocations: globaldata, ProjectStartDate: projectStartdate, ProjectEndDate: projectEnddate, DeleteExistingAllocations: deleteExistingAllocations }).then(function (response) {
            if (response.includes("OverlappingAllocation")) {
                var resultvalues = response.split(":");
                HighlightSummaryGridOnerror(resultvalues);
                DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                let datakey = _.findWhere(globaldata, { ID: parseInt(resultvalues[1]) });
                let dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
                let rowIndex = dataGrid.getRowIndexByKey(datakey);
                dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
            }
            else {
                $.cookie("opensummaryview", true);
                window.parent.CloseWindowCallback(1, document.location.href);
            }
        }, function (error) { }).done(function () { $("#loadPanel").dxLoadPanel("hide"); }).fail(function () { $("#loadPanel").dxLoadPanel("hide"); });
    }

    function openResourceTimeSheet(assignedTo, assignedToName) {
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + assignedToName, "95", "95", false, "");
    }
    function HighlightSummaryGridOnerror(resultvalues) {
        var compactDataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
        var datakey = _.findWhere(CompactTempData, { ID: parseInt(resultvalues[1]) });
        if (datakey == null || datakey == '') {
            datakey = _.findWhere(CompactTempData, { ConstId: parseInt(resultvalues[1]) });
        }
        if (datakey == null || datakey == '') {
            datakey = _.findWhere(CompactTempData, { CloseOutId: parseInt(resultvalues[1]) });
        }
        var rowIndex = compactDataGrid.getRowIndexByKey(datakey);
        compactDataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
    }
    function getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate) {

        let preconEndDate = new Date(DatesModel.PreconEnd);
        let closeoutEndDate = new Date(DatesModel.Closeout);

        if (isDateValid(closeoutstartDate) && isDateValid(closeoutEndDate)
            && cellValue >= closeoutstartDate && cellValue <= closeoutEndDate) {
            return 'closeoutCellStyle';
        }
        else if (isDateValid(conststartDate) && isDateValid(constEndDate) &&
            cellValue <= constEndDate && cellValue >= conststartDate) {
            return 'constCellStyle';
        }
        else if (isDateValid(preconstartDate) && isDateValid(preconEndDate)
            && cellValue >= preconstartDate && cellValue <= preconEndDate) {
            return 'preconCellStyle';
        }
        else
            return 'noDateCellStyle';
    }
    function CalculatePctAllocation(dataRow) {
        let totalPercentage = 0;
        let minDate = new Date(dataRow[0].AllocationStartDate);
        let maxDate = new Date(dataRow[dataRow.length - 1].AllocationEndDate);
        $.each(dataRow, function (index, e) {
            let allocStartdate = new Date(e.AllocationStartDate);
            let allocEnddate = new Date(e.AllocationEndDate);
            if (allocStartdate < minDate) {
                minDate = allocStartdate;
            }
            if (allocEnddate > maxDate) {
                maxDate = allocEnddate;
            }
        });
        let milli_secs_total = maxDate.getTime() - minDate.getTime();

        // Convert the milli seconds to Days 
        var totalDays = (milli_secs_total / (1000 * 3600 * 24)) + 1;

        $.each(dataRow, function (index, e) {
            let allocStartdate = new Date(e.AllocationStartDate);
            let allocEnddate = new Date(e.AllocationEndDate);
            var milli_secs = allocEnddate.getTime() - allocStartdate.getTime();

            // Convert the milli seconds to Days 
            var daysDiff = (milli_secs / (1000 * 3600 * 24)) + 1;

            let pctAlloc = parseInt(e.PctAllocation);
            totalPercentage += pctAlloc * daysDiff;
        });

        return totalDays > 0 ? Math.ceil(totalPercentage / totalDays) : 0;
    }
    function OpenInternalAllocation(refIds) {
        let ids = refIds.split(";");
        if (ids != null && ids.length > 0) {
            let gdata = globaldata.filter(x => ids.includes(String(x.ID)));
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let datagrid = $("<div id='InternalAllocationGrid'>").dxDataGrid({
                    dataSource: gdata,
                    ID: "grdTemplate",
                    editing: {
                        mode: "cell",
                        allowEditing: true,
                        allowUpdating: true
                    },
                    sorting: {
                        mode: "multiple" // or "multiple" | "none"
                    },
                    scrolling: {
                        mode: 'Standard',
                    },
                    columns: [
                        {
                            dataField: "AssignedToName",
                            dataType: "text",
                            caption: "Assigned To",
                            allowEditing: false,
                        },
                        {
                            dataField: "TypeName",
                            dataType: "text",
                            allowEditing: false,
                            caption: "Role",
                            sortIndex: "1",
                            sortOrder: "asc",
                            width: "30%",
                            cssClass: "cls",
                        },
                        {
                            dataField: "Type",
                            dataType: "text",
                            visible: false
                        },
                        {
                            dataField: "AllocationStartDate",
                            caption: "Start Date",
                            dataType: "date",
                            width: "15%",
                            alignment: 'center',
                            cssClass: "v-align",
                            allowEditing: false,
                            //validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            sortIndex: "2",
                            sortOrder: "asc",
                            editorOptions: {
                                onFocusOut: function (e, options) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                }
                            }
                        },
                        {
                            dataField: "AllocationEndDate",
                            caption: "End Date",
                            dataType: "date",
                            alignment: 'center',
                            cssClass: "v-align",
                            width: "15%",
                            allowEditing: false,
                            //validationRules: [{ type: "required", message: '', }],
                            format: 'MMM d, yyyy',
                            editorOptions: {
                                onFocusOut: function (e) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                                onClosed: function (e) {
                                    var dataGrid = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGrid.saveEditData();
                                },
                            }
                        },
                        {
                            dataField: "PctAllocation",
                            caption: "% Alloc",
                            dataType: "text",
                            width: "10%",
                            setCellValue: function (newData, value, currentRowData) {
                                if (parseInt(value) <= 0) {
                                    globaldata = globaldata.filter(x => x.ID != currentRowData.ID);
                                    let gdata = globaldata.filter(x => refIds.includes(String(x.ID)));
                                    let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                                    dataGridChild.option("dataSource", gdata);
                                    var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                                    dataGrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel));
                                }
                                else {
                                    newData.PctAllocation = value;
                                }
                            }
                        },
                    ],
                    showBorders: true,
                    showRowLines: true,
                    onCellPrepared: function (e) {
                        if (e.rowType === 'data') {
                            var preconstartDate = new Date(DatesModel.PreconStart);
                            var conststartDate = new Date(DatesModel.ConstStart);
                            var constEndDate = new Date(DatesModel.ConstEnd);
                            var closeoutstartDate = new Date(DatesModel.CloseoutStart);

                            if (e.column.dataField == 'AllocationStartDate') {
                                let cellValue = new Date(e.data.AllocationStartDate)
                                let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                e.cellElement.addClass(className);
                            }
                            if (e.column.dataField == 'AllocationEndDate') {
                                let cellValue = new Date(e.data.AllocationEndDate)
                                let className = getDateBackgroundColor(cellValue, preconstartDate, conststartDate, constEndDate, closeoutstartDate);
                                e.cellElement.addClass(className);
                            }
                        }
                    },
                });
                let confirmBtn = $(`<div class="mt-4 mb-2 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Save",
                    hint: 'Save Allocations',
                    visible: true,
                    onClick: function (e) {
                        let dataGridChild = $("#InternalAllocationGrid").dxDataGrid("instance");
                        let rowschild = dataGridChild.getVisibleRows();
                        globaldata = globaldata.filter(x => !ids.includes(String(x.ID)));
                        $.each(rowschild, function (index, e) {
                            if (parseInt(e.data.PctAllocation) > 0) {
                                globaldata.push(e.data);
                            }
                        });
                        var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                        dataGrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel));
                        //var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
                        //dataGrid.option("dataSource", globaldata);
                        //CompactPhaseConstraints();
                        popup.hide();
                    }
                })
                let cancelBtn = $(`<div class="mt-4 mb-2 btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                    text: "Cancel",
                    visible: true,
                    onClick: function (e) {
                        popup.hide();
                    }
                })
                container.append(datagrid);
                container.append(confirmBtn);
                container.append(cancelBtn);
                return container;
            };

            const popup = $("#InternalAllocationGridDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "900",
                height: "auto",
                showTitle: true,
                title: "Edit Allocations",
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                },
                onHiding: function () {

                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupContentTemplate1()

            });
            popup.show();
        }
    }
    function SaveToGlobalData(row) {
        let ConstStartDate = DatesModel.ConstStart;
        let ConstEndDate = DatesModel.ConstEnd;

        let PreconStartDate = DatesModel.PreconStart;
        let PreconEndDate = DatesModel.PreconEnd;

        let CloseoutStartDate = DatesModel.CloseoutStart;
        let CloseoutEndDate = DatesModel.Closeout;
        let data = row.key;
        if (parseInt(row.oldData.PctAllocation) == 0 && parseInt(row.newData.PctAllocation) > 0) {
            var sum = 0;
            sum = globaldata.length + 100 + Math.floor((Math.random() * 100) + 1);
            globaldata.push({ "ID": -Math.abs(sum), ProjectID: projectID, "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": PreconStartDate, "AllocationEndDate": PreconEndDate, "PctAllocation": parseInt(row.newData.PctAllocation), "SoftAllocation": false, "NonChargeable": 0, "Type": data.Type, "TypeName": data.TypeName, "Tags": '' });
        }
        if (parseInt(row.oldData.PctAllocationConst) == 0 && parseInt(row.newData.PctAllocationConst) > 0) {
            var sum = 0;
            sum = globaldata.length + 200 + Math.floor((Math.random() * 100) + 1);
            globaldata.push({ "ID": -Math.abs(sum), ProjectID: projectID, "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": ConstStartDate, "AllocationEndDate": ConstEndDate, "PctAllocation": parseInt(row.newData.PctAllocationConst), "SoftAllocation": false, "NonChargeable": 0, "Type": data.Type, "TypeName": data.TypeName, "Tags": '' });
        }
        if (parseInt(row.oldData.PctAllocationCloseOut) == 0 && parseInt(row.newData.PctAllocationCloseOut) > 0) {
            var sum = 0;
            sum = globaldata.length + 300 + Math.floor((Math.random() * 100) + 1);
            globaldata.push({ "ID": -Math.abs(sum), ProjectID: projectID, "AssignedTo": data.AssignedTo, "AssignedToName": data.AssignedToName, "AllocationStartDate": CloseoutStartDate, "AllocationEndDate": CloseoutEndDate, "PctAllocation": parseInt(row.newData.PctAllocationCloseOut), "SoftAllocation": false, "NonChargeable": 0, "Type": data.Type, "TypeName": data.TypeName, "Tags": '' });
        }
        let gdata = globaldata.filter(x => x.ID == data.PreconId || x.ID == data.ConstId || x.ID == data.CloseOutId);
        $.each(gdata, function (index, e) {
            if (e.ID == data.PreconId && row.newData.PctAllocation != null && row.newData.PctAllocation != "") {
                if (parseInt(row.newData.PctAllocation) > 0) {
                    e.PctAllocation = row.newData.PctAllocation;
                    data.PctAllocation = e.PctAllocation;
                }
                else {
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                }
            }
            if (e.ID == data.ConstId && row.newData.PctAllocationConst != null && row.newData.PctAllocationConst != "") {
                if (parseInt(row.newData.PctAllocationConst) > 0) {
                    e.PctAllocation = row.newData.PctAllocationConst;
                    data.PctAllocation = e.PctAllocation;
                }
                else {
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                }
            }
            if (e.ID == data.CloseOutId && row.newData.PctAllocationCloseOut != null && row.newData.PctAllocationCloseOut != "") {
                if (parseInt(row.newData.PctAllocationCloseOut) > 0) {
                    e.PctAllocation = row.newData.PctAllocationCloseOut;
                    data.PctAllocation = e.PctAllocation;
                }
                else {
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                }
            }
            if (row.newData.TypeName != null) {
                e.TypeName = row.newData.TypeName;
            }
        });
        if (row.newData.AllocationEndDate != null) {
            data.AllocationEndDate = row.newData.AllocationEndDate;
            data.AllocationStartDate = row.newData.AllocationStartDate != "" && row.newData.AllocationStartDate != undefined
                ? row.newData.AllocationStartDate : data.AllocationStartDate;
            globaldata = globaldata.filter(x => x.ID != data.PreconId && x.ID != data.ConstId && x.ID != data.CloseOutId);
            globaldata.push(data);
            CheckPhaseConstraints(false);
        }
        else if (row.newData.AllocationStartDate != null) {
            data.AllocationStartDate = row.newData.AllocationStartDate;
            globaldata = globaldata.filter(x => x.ID != data.PreconId && x.ID != data.ConstId && x.ID != data.CloseOutId);
            globaldata.push(data);
            CheckPhaseConstraints(false);
        }

        var dataGrid = $("#compactGridTemplateContainer").dxDataGrid("instance");
        dataGrid.option("dataSource", CompactPhaseConstraintsTemplate(globaldata, DatesModel));
    }
    function CompactPhaseConstraintsTemplate(data, datesData, calculatePct) {
        CheckPhaseConstraints(false);
        CompactTempData = [];
        let tempData = JSON.parse(JSON.stringify(data));
        //compactTempData = Object.create(globaldata);
        let isDateInMultiPhase = false;
        $.each(tempData, function (index, e) {
            let data1 = JSON.parse(JSON.stringify(tempData.filter(x => x.Type == e.Type && x.AssignedTo == e.AssignedTo)));
            let internalPhaseData = [];
            let constStartDate = new Date(datesData.ConstStart);
            let constEndDate = new Date(datesData.ConstEnd);

            let preconStartDate = new Date(datesData.PreconStart);
            let preconEndDate = new Date(datesData.PreconEnd);

            let closeoutStartDate = new Date(datesData.CloseoutStart);
            let closeoutEndDate = new Date(datesData.Closeout);

            if (!isDateValid(constStartDate) && !isDateValid(constEndDate) && isDateValid(preconStartDate) && isDateValid(preconEndDate)) {
                constStartDate = preconStartDate;
                constEndDate = preconEndDate;
            }

            let internalPrecon = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) < constStartDate)));
            let internalConst = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) >= constStartDate && new Date(x.AllocationEndDate) <= constEndDate)));
            let internalCloseOut = JSON.parse(JSON.stringify(data1.filter(x => new Date(x.AllocationStartDate) > constEndDate)));
            if (internalPrecon.length > 1) {
                let internalPreconTemp = JSON.parse(JSON.stringify(internalPrecon[0]));
                let ids = [];
                let endDateForPctCal = new Date(Math.max.apply(null, internalPrecon.filter(x => new Date(x.AllocationEndDate) >= preconStartDate).map(x => new Date(x.AllocationEndDate))));
                let startDateForPctCal = new Date(Math.min.apply(null, internalPrecon.filter(x => new Date(x.AllocationEndDate) >= preconStartDate).map(x => new Date(x.AllocationStartDate))));
                internalPreconTemp.AllocationEndDate = new Date(Math.max.apply(null, internalPrecon.map(x => new Date(x.AllocationEndDate))));
                internalPreconTemp.AllocationStartDate = new Date(Math.min.apply(null, internalPrecon.map(x => new Date(x.AllocationStartDate))));
                let percentage = CalculatePctAllocation(internalPrecon, startDateForPctCal, endDateForPctCal);
                internalPreconTemp.PctAllocation = percentage <= 0 ? Math.max.apply(null, internalPrecon.map(x => parseInt(x.PctAllocation))) : percentage;

                $.each(internalPrecon, function (index, e) {
                    ids.push(e.ID);
                });
                internalPreconTemp.preconRefIds = ids.join(';');
                internalPhaseData.push(internalPreconTemp);
            }
            else if (internalPrecon.length) {
                internalPhaseData.push(internalPrecon[0]);
            }

            if (internalConst.length > 1) {
                let internalConstTemp = JSON.parse(JSON.stringify(internalConst[0]));
                let ids = [];
                internalConstTemp.AllocationEndDate = new Date(Math.max.apply(null, internalConst.map(x => new Date(x.AllocationEndDate))));
                internalConstTemp.AllocationStartDate = new Date(Math.min.apply(null, internalConst.map(x => new Date(x.AllocationStartDate))));
                internalConstTemp.PctAllocation = CalculatePctAllocation(internalConst, internalConstTemp.AllocationStartDate, internalConstTemp.AllocationEndDate);
                $.each(internalConst, function (index, e) {
                    ids.push(e.ID);
                });
                internalConstTemp.constRefIds = ids.join(';');
                internalPhaseData.push(internalConstTemp);
            }
            else if (internalConst.length) {
                internalPhaseData.push(internalConst[0]);
            }

            if (internalCloseOut.length > 1) {
                let internalCloseOutTemp = JSON.parse(JSON.stringify(internalCloseOut[0]));
                let ids = [];
                let endDateForPctCal = new Date(Math.max.apply(null, internalCloseOut.filter(x => new Date(x.AllocationStartDate) <= closeoutEndDate).map(x => new Date(x.AllocationEndDate))));
                let startDateForPctCal = new Date(Math.min.apply(null, internalCloseOut.filter(x => new Date(x.AllocationStartDate) <= closeoutEndDate).map(x => new Date(x.AllocationStartDate))));
                internalCloseOutTemp.AllocationEndDate = new Date(Math.max.apply(null, internalCloseOut.map(x => new Date(x.AllocationEndDate))));
                internalCloseOutTemp.AllocationStartDate = new Date(Math.min.apply(null, internalCloseOut.map(x => new Date(x.AllocationStartDate))));

                let percentage = CalculatePctAllocation(internalCloseOut, startDateForPctCal, endDateForPctCal);
                internalCloseOutTemp.PctAllocation = percentage <= 0 ? Math.max.apply(null, internalCloseOut.map(x => parseInt(x.PctAllocation))) : percentage;

                $.each(internalCloseOut, function (index, e) {
                    ids.push(e.ID);
                });
                internalCloseOutTemp.closeOutRefIds = ids.join(';');
                internalPhaseData.push(internalCloseOutTemp);
            }
            else if (internalCloseOut.length) {
                internalPhaseData.push(internalCloseOut[0]);
            }
            let remainingData = data1.filter(x => x.AllocationStartDate == "" && x.AllocationEndDate == "");
            if (remainingData.length > 0) {
                internalPhaseData.push(remainingData[0]);
            }
            let data = JSON.parse(JSON.stringify(internalPhaseData));
            let odata = JSON.parse(JSON.stringify(internalPhaseData));
            if (data.length > 0) {
                if (data.length > 1) {
                    data[0].AllocationEndDate = data[data.length - 1].AllocationEndDate
                    if (data.length == 2) {
                        let startDate = new Date(data[0].AllocationStartDate);
                        let endDate = new Date(data[0].AllocationEndDate);
                        let preconStartDate = new Date(datesData.PreconStart);
                        let preconEndDate = new Date(datesData.PreconEnd);

                        let constStartDate = new Date(datesData.ConstStart);
                        let constEndDate = new Date(datesData.ConstEnd);

                        if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                            preconStartDate = startDate;
                        }
                        if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                            preconEndDate = constStartDate.addDays(-1);
                        }
                        if (startDate < constStartDate && endDate > constEndDate) {
                            data[0].PctAllocation = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate) {
                                //let percentage = CalculatePctAllocation([odata[1]], closeoutStartDate, closeoutEndDate);
                                //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].closeOutRefIds = data[1].closeOutRefIds == null || data[1].closeOutRefIds == '' ? data[1].ID : data[1].closeOutRefIds;//data[1].ID;
                            }

                            data[0].PctAllocationConst = 0;
                            data[0].PreconId = data[0].ID;
                            data[0].ConstId = 0;
                            data[0].CloseOutId = data[1].ID;
                            if (data[1].closeOutRefIds != null)
                                data[0].closeOutRefIds = data[1].closeOutRefIds;
                        }
                        else if (startDate < constStartDate) {

                            data[0].PctAllocation = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                                //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                                //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                            }

                            data[0].PctAllocationConst = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate) {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                                data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                            }

                            data[0].PctAllocationCloseOut = 0;
                            data[0].PreconId = data[0].ID;
                            data[0].ConstId = data[1].ID;
                            data[0].CloseOutId = 0;
                            if (data[1].constRefIds != null)
                                data[0].constRefIds = data[1].constRefIds;
                        }
                        else {

                            data[0].PctAllocationConst = data[0].PctAllocation;
                            if (new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate) {
                                //data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                                data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds == '' ? data[0].ID : data[0].constRefIds;
                            }

                            data[0].PctAllocationCloseOut = data[1].PctAllocation;
                            if (new Date(odata[1].AllocationEndDate) < closeoutEndDate || new Date(odata[1].AllocationStartDate) > closeoutStartDate) {
                                //let percentage = CalculatePctAllocation([odata[1]], closeoutStartDate, closeoutEndDate);
                                //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                                data[0].closeOutRefIds = data[1].closeOutRefIds == null || data[1].closeOutRefIds == '' ? data[1].ID : data[1].closeOutRefIds; //data[1].ID;
                            }

                            data[0].PctAllocation = 0;
                            data[0].PreconId = 0;
                            data[0].ConstId = data[0].ID;
                            data[0].CloseOutId = data[1].ID;
                            if (data[1].closeOutRefIds != null)
                                data[0].closeOutRefIds = data[1].closeOutRefIds;
                        }
                    }
                    else {
                        data[0].PctAllocation = data[0].PctAllocation;
                        if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                            //let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                            //data[0].PctAllocation = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds;
                        }

                        data[0].PctAllocationConst = data[1].PctAllocation;
                        if (new Date(odata[1].AllocationEndDate) < constEndDate || new Date(odata[1].AllocationStartDate) > constStartDate) {
                            //data[0].PctAllocationConst = CalculatePctAllocation([odata[1]], constStartDate, constEndDate);
                            data[0].constRefIds = data[1].constRefIds == null || data[1].constRefIds == '' ? data[1].ID : data[1].constRefIds; //data[1].ID;
                        }

                        data[0].PctAllocationCloseOut = data[2].PctAllocation;
                        if (new Date(odata[2].AllocationEndDate) < closeoutEndDate || new Date(odata[2].AllocationStartDate) > closeoutStartDate) {
                            //let percentage = CalculatePctAllocation([odata[2]], closeoutStartDate, closeoutEndDate);
                            //data[0].PctAllocationCloseOut = percentage <= 0 ? Math.max.apply(null, data.map(x => parseInt(x.PctAllocation))) : percentage;
                            data[0].closeOutRefIds = data[2].closeOutRefIds == null || data[2].closeOutRefIds == '' ? data[2].ID : data[2].closeOutRefIds; //data[2].ID;
                        }

                        data[0].PreconId = data[0].ID;
                        data[0].ConstId = data[1].ID;
                        data[0].CloseOutId = data[2].ID;
                        if (data[1].constRefIds != null)
                            data[0].constRefIds = data[1].constRefIds;
                        if (data[2].closeOutRefIds != null)
                            data[0].closeOutRefIds = data[2].closeOutRefIds;
                    }
                }
                else {
                    let startDate = new Date(data[0].AllocationStartDate);
                    let endDate = new Date(data[0].AllocationEndDate);
                    let preconStartDate = new Date(datesData.PreconStart);
                    let preconEndDate = new Date(datesData.PreconEnd);

                    let constStartDate = new Date(datesData.ConstStart);
                    let constEndDate = new Date(datesData.ConstEnd);

                    if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                        preconStartDate = startDate;
                    }
                    if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                        preconEndDate = constStartDate.addDays(-1);
                    }
                    if (startDate >= constStartDate && endDate <= constEndDate) {

                        data[0].PctAllocationConst = data[0].PctAllocation;
                        //if (new Date(odata[0].AllocationEndDate) < constEndDate || new Date(odata[0].AllocationStartDate) > constStartDate) {
                        //    data[0].PctAllocationConst = CalculatePctAllocation([odata[0]], constStartDate, constEndDate);
                        //    data[0].constRefIds = data[0].constRefIds == null || data[0].constRefIds == '' ? data[0].ID : data[0].constRefIds; //data[0].ID;
                        //}

                        data[0].PctAllocation = 0;
                        data[0].PctAllocationCloseOut = 0;
                        data[0].PreconId = 0;
                        data[0].ConstId = data[0].ID;
                        data[0].CloseOutId = 0;
                    }
                    else if (constEndDate < startDate) {
                        data[0].PctAllocationCloseOut = data[0].PctAllocation;
                        //if (new Date(odata[0].AllocationEndDate) < closeoutEndDate || new Date(odata[0].AllocationStartDate) > closeoutStartDate) {
                        //    let percentage = CalculatePctAllocation([odata[0]], closeoutStartDate, closeoutEndDate);
                        //    data[0].PctAllocationCloseOut = percentage <= 0 ? data[0].PctAllocationCloseOut : percentage;
                        //    data[0].closeOutRefIds = data[0].closeOutRefIds == null || data[0].closeOutRefIds == '' ? data[0].ID : data[0].closeOutRefIds; //data[0].ID;
                        //}
                        data[0].PctAllocation = 0;
                        data[0].PctAllocationConst = 0;
                        data[0].PreconId = 0;
                        data[0].ConstId = 0;
                        data[0].CloseOutId = data[0].ID;
                    }
                    else {
                        //if (new Date(odata[0].AllocationEndDate) < preconEndDate || new Date(odata[0].AllocationStartDate) > preconStartDate) {
                        //    let percentage = CalculatePctAllocation([odata[0]], preconStartDate, preconEndDate);
                        //    data[0].PctAllocation = percentage <= 0 ? data[0].PctAllocation : percentage;
                        //    data[0].preconRefIds = data[0].preconRefIds == null || data[0].preconRefIds == '' ? data[0].ID : data[0].preconRefIds; //data[0].ID;
                        //}
                        data[0].PctAllocationConst = 0;
                        data[0].PctAllocationCloseOut = 0;
                        data[0].PreconId = data[0].ID;
                        data[0].ConstId = 0;
                        data[0].CloseOutId = 0;
                    }
                }
                if (CompactTempData.length == 0) {
                    CompactTempData.push(JSON.parse(JSON.stringify(data[0])));
                }
                else if (CompactTempData.filter(x => x.Type == e.Type && x.AssignedTo == e.AssignedTo).length == 0) {
                    CompactTempData.push(JSON.parse(JSON.stringify(data[0])));
                }
            }
        });


        CompactTempData.forEach(function (data) {
            let ids = [];
            if (parseInt(data.PreconId) != 0) {
                ids.push(data.PreconId);
            }
            if (parseInt(data.ConstId) != 0) {
                ids.push(data.ConstId);
            }
            if (parseInt(data.CloseOutId) != 0) {
                ids.push(data.CloseOutId);
            }
            if (data.closeOutRefIds != undefined && data.closeOutRefIds != '') {
                let rids = String(data.closeOutRefIds).split(";");
                if (rids != null && rids.length > 0) {
                    rids.forEach(function (id) {
                        ids.push(id);
                    });
                }
            }
            if (data.preconRefIds != undefined && data.preconRefIds != '') {
                let rids = String(data.preconRefIds).split(";");
                if (rids != null && rids.length > 0) {
                    rids.forEach(function (id) {
                        ids.push(id);
                    });
                }
            }
            if (data.constRefIds != undefined && data.constRefIds != '') {
                let rids = String(data.constRefIds).split(";");
                if (rids != null && rids.length > 0) {
                    rids.forEach(function (id) {
                        ids.push(id);
                    });
                }
            }
            data.IdsForDelete = ids.join(";");
        });

        return CompactTempData;

    }

    function CheckPhaseConstraints(checkDateInMultiPhase) {
        var datesData = DatesModel;
        OverlappingAllocationInPhases = [];
        let tempData = Object.create(globaldata);
        //compactTempData = Object.create(globaldata);
        let isDateInMultiPhase = false;
        $.each(tempData, function (index, e) {
            let preconStartDate = new Date(datesData.PreconStart);
            let preconEndDate = new Date(datesData.PreconEnd);

            let constStartDate = new Date(datesData.ConstStart);
            let constEndDate = new Date(datesData.ConstEnd);

            let closeoutStartDate = new Date(datesData.CloseoutStart);
            let closeoutEndDate = new Date(datesData.Closeout);

            let startDate = new Date(e.AllocationStartDate);
            let endDate = new Date(e.AllocationEndDate);

            if (!isDateValid(startDate) || !isDateValid(endDate)) {
                return;
            }
            let overlapDates = {
                precon: false,
                const: false,
                closeout: false
            };
            if (checkDateInMultiPhase) {
                if (!isDateValid(preconStartDate) && startDate <= constStartDate) {
                    preconStartDate = startDate;
                }
            }
            else {
                if (!isDateValid(preconStartDate) && startDate < constStartDate) {
                    preconStartDate = startDate;
                }
            }
            if (!isDateValid(preconEndDate) && isDateValid(constStartDate)) {
                preconEndDate = constStartDate.addDays(-1);
            }
            if (checkDateInMultiPhase) {
                if (!isDateValid(closeoutEndDate) && endDate >= constEndDate) {
                    closeoutEndDate = endDate;
                }
            }
            else {
                if (!isDateValid(closeoutEndDate) && endDate > constEndDate) {
                    closeoutEndDate = endDate;
                }
            }
            if (!isDateValid(closeoutStartDate) && isDateValid(constEndDate)) {
                closeoutStartDate = constEndDate.addDays(1);
            }

            if (startDate <= preconEndDate && endDate >= preconStartDate) {
                overlapDates.precon = true;
            }
            if (startDate <= constEndDate && endDate >= constStartDate) {
                overlapDates.const = true;
            }
            if (startDate <= closeoutEndDate && endDate >= closeoutStartDate) {
                overlapDates.closeout = true;
            }
            if (checkDateInMultiPhase) {
                if (overlapDates.precon && overlapDates.const && overlapDates.closeout) {
                    isDateInMultiPhase = true;
                }
                else {
                    if (overlapDates.precon && overlapDates.const) {
                        isDateInMultiPhase = true;
                    }
                    if (overlapDates.const && overlapDates.closeout) {
                        isDateInMultiPhase = true;
                    }
                    if (overlapDates.precon && !overlapDates.const && !overlapDates.closeout) {
                        if (endDate > preconEndDate) {
                            isDateInMultiPhase = true;
                        }
                    }
                    if (!overlapDates.precon && overlapDates.const && !overlapDates.closeout) {
                        if (startDate < constStartDate || endDate > constEndDate) {
                            isDateInMultiPhase = true;
                        }
                    }
                    if (!overlapDates.precon && !overlapDates.const && overlapDates.closeout) {
                        if (startDate < closeoutStartDate) {
                            isDateInMultiPhase = true;
                        }
                    }
                }
            }
            if (overlapDates.precon && overlapDates.const && overlapDates.closeout) {
                OverlappingAllocationInPhases.push(e);
                let alloc1 = JSON.parse(JSON.stringify(e));
                let alloc2 = JSON.parse(JSON.stringify(e));
                let alloc3 = JSON.parse(JSON.stringify(e));
                globaldata = globaldata.filter(x => x.ID != e.ID);
                alloc1.AllocationEndDate = preconEndDate.toLocaleString("en-US");
                alloc2.AllocationStartDate = constStartDate.toLocaleString("en-US");
                alloc2.AllocationEndDate = constEndDate.toLocaleString("en-US");
                alloc3.AllocationStartDate = closeoutStartDate.toLocaleString("en-US");
                if (alloc1.PctAllocationConst != undefined && parseInt(alloc1.PctAllocationConst) > 0) {
                    alloc2.PctAllocation = alloc1.PctAllocationConst;
                    alloc3.PctAllocation = alloc1.PctAllocationCloseOut;
                }
                globaldata.push(alloc1);
                alloc2.ID = -Math.abs(globaldata.length + 1);
                globaldata.push(alloc2);
                alloc3.ID = -Math.abs(globaldata.length + 1);
                globaldata.push(alloc3);
            }
            else {
                if (overlapDates.precon && overlapDates.const) {
                    OverlappingAllocationInPhases.push(e);
                    let alloc1 = JSON.parse(JSON.stringify(e));
                    let alloc2 = JSON.parse(JSON.stringify(e));
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                    if (alloc1.PctAllocationConst != undefined && parseInt(alloc1.PctAllocationConst) > 0) {
                        alloc2.PctAllocation = alloc1.PctAllocationConst;
                    }
                    alloc1.AllocationEndDate = preconEndDate.toLocaleString("en-US");
                    alloc2.AllocationStartDate = constStartDate.toLocaleString("en-US");
                    globaldata.push(alloc1);
                    alloc2.ID = -Math.abs(globaldata.length + 1);
                    globaldata.push(alloc2);
                }
                if (overlapDates.const && overlapDates.closeout) {
                    OverlappingAllocationInPhases.push(e);
                    let alloc1 = JSON.parse(JSON.stringify(e));
                    let alloc2 = JSON.parse(JSON.stringify(e));
                    globaldata = globaldata.filter(x => x.ID != e.ID);
                    if (alloc1.PctAllocationCloseOut != undefined && parseInt(alloc1.PctAllocationCloseOut) > 0) {
                        alloc2.PctAllocation = alloc1.PctAllocationCloseOut;
                    }

                    alloc1.AllocationStartDate = startDate < constStartDate
                        ? constStartDate.toLocaleString("en-US") : alloc1.AllocationStartDate;

                    alloc1.AllocationEndDate = constEndDate.toLocaleString("en-US");
                    alloc2.AllocationStartDate = closeoutStartDate.toLocaleString("en-US");
                    globaldata.push(alloc1);
                    alloc2.ID = -Math.abs(globaldata.length + 1);
                    globaldata.push(alloc2);
                }
                if (overlapDates.precon && !overlapDates.const && !overlapDates.closeout) {
                    if (endDate > preconEndDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationEndDate = preconEndDate.toLocaleString("en-US");
                        globaldata.push(alloc1);
                    }
                }
                if (!overlapDates.precon && overlapDates.const && !overlapDates.closeout) {
                    if (startDate < constStartDate || endDate > constEndDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationStartDate = startDate < constStartDate
                            ? constStartDate.toLocaleString("en-US") : alloc1.AllocationStartDate;
                        alloc1.AllocationEndDate = endDate > constEndDate
                            ? constEndDate.toLocaleString("en-US") : alloc1.AllocationEndDate;
                        globaldata.push(alloc1);
                    }
                }
                if (!overlapDates.precon && !overlapDates.const && overlapDates.closeout) {
                    if (startDate < closeoutStartDate) {
                        OverlappingAllocationInPhases.push(e);
                        let alloc1 = JSON.parse(JSON.stringify(e));
                        globaldata = globaldata.filter(x => x.ID != e.ID);
                        alloc1.AllocationStartDate = closeoutStartDate.toLocaleString("en-US");
                        globaldata.push(alloc1);
                    }
                }
            }
        });

    }

    function updateDatesInGrid(startDate, EndDate, color) {
        if (startDate == "Invalid Date") {
            startDate = '';
        }
        if (EndDate == "Invalid Date") {
            EndDate = '';
        }
        flag = true;
        if (strSelectedAllocationIDs) {
            var Ids = strSelectedAllocationIDs.split(",");
            var dataGrid = $("#gridTemplateContainer").dxDataGrid("instance");
            var rows = dataGrid.getVisibleRows();
            rows.forEach(function (item, index) {
                if (Ids.indexOf(item.data.ID.toString()) != -1) {
                    dataGrid.cellValue(index, "AllocationStartDate", startDate);
                    dataGrid.cellValue(index, "AllocationEndDate", EndDate);

                    var cellElementStartDate = dataGrid.getCellElement(index, "AllocationStartDate");
                    cellElementStartDate.css('color', color);
                    var cellElementEndDate = dataGrid.getCellElement(index, "AllocationEndDate");
                    cellElementEndDate.css('color', color);
                }
            });

            selectionObject.component.clearSelection();
            dataGrid.saveEditData();

        } else {
            $("#toastwarning").dxToast("show");
        }
    }

    function isAllAllocationsSame(options) {
        let allSoftAllocationValues = []
        if (options.data.ID != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ID) });
            if (idData)
                allSoftAllocationValues.push(idData.SoftAllocation);
        }
        if (options.data.ConstId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ConstId) });
            if (idData)
                allSoftAllocationValues.push(idData.SoftAllocation);
        }
        if (options.data.CloseOutId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.CloseOutId) });
            if (idData)
                allSoftAllocationValues.push(idData.SoftAllocation);
        }
        if (options.data.preconRefIds != null) {
            let rids = String(options.data.preconRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allSoftAllocationValues.push(idData.SoftAllocation);
                });
            }
        }
        if (options.data.constRefIds != null) {
            let rids = String(options.data.constRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allSoftAllocationValues.push(idData.SoftAllocation);
                });
            }
        }
        if (options.data.closeOutRefIds != null) {
            let rids = String(options.data.closeOutRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allSoftAllocationValues.push(idData.SoftAllocation);
                });
            }
        }

        const areAllSame = allSoftAllocationValues.every(value => value === allSoftAllocationValues[0]);

        return areAllSame;
    }


    function isAllNCOAllocationsSame(options) {
        let allNCOAllocationValues = []
        if (options.data.ID != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ID) });
            if (idData)
                allNCOAllocationValues.push(idData.NonChargeable);
        }
        if (options.data.ConstId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.ConstId) });
            if (idData)
                allNCOAllocationValues.push(idData.NonChargeable);
        }
        if (options.data.CloseOutId != null) {
            var idData = _.findWhere(globaldata, { ID: parseInt(options.data.CloseOutId) });
            if (idData)
                allNCOAllocationValues.push(idData.NonChargeable);
        }
        if (options.data.preconRefIds != null) {
            let rids = String(options.data.preconRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allNCOAllocationValues.push(idData.NonChargeable);
                });
            }
        }
        if (options.data.constRefIds != null) {
            let rids = String(options.data.constRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allNCOAllocationValues.push(idData.NonChargeable);
                });
            }
        }
        if (options.data.closeOutRefIds != null) {
            let rids = String(options.data.closeOutRefIds).split(";");
            if (rids != null && rids.length > 0) {
                rids.forEach(function (id) {
                    var idData = _.findWhere(globaldata, { ID: parseInt(id) });
                    if (idData)
                        allNCOAllocationValues.push(idData.NonChargeable);
                });
            }
        }

        const areAllSame = allNCOAllocationValues.every(value => value === allNCOAllocationValues[0]);

        return areAllSame;
    }
</script>
<script>
    //utility functions

    
</script>

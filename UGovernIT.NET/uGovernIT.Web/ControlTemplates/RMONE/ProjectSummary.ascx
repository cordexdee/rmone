<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectSummary.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.ProjectSummary" %>

<%@ Register Src="~/ControlTemplates/RMONE/ModuleConstraintsListDx.ascx" TagPrefix="ugit" TagName="ModuleConstraintsListDx" %>
<%@ Register Src="~/ControlTemplates/RMONE/AddProjectExperienceTags.ascx" TagPrefix="ugit" TagName="AddProjectExperienceTags" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-sort {
        visibility: hidden !important;
    }

    .rcorners2 {
        border-radius: 7px;
        border: 1px solid #D3D3D3;
        padding: 7px;
        width: 200px;
        height: 150px;
    }

    .center {
        text-align: center;
    }
    #allcationGrid .dx-datagrid{  
        font-size:12px;
        font-family: 'Roboto', sans-serif !important;
    }
    .dx-datagrid .dx-row-alt.dx-row:not(.dx-row-removed) {
        background-color:#F5F5F5;
    }
    #form .dx-layout-manager .dx-label-h-align.dx-flex-layout {
    display: -webkit-box;
    display: -ms-flexbox;
    display: flex;
    flex-direction: column;
}
    #form .dx-form-group-with-caption > .dx-form-group-content {
    padding-top: 0px !important;
    margin-top: 0px !important;
    border-top: 0px solid #ddd !important;
    padding-bottom: 15px !important;
}

    .userbox{
        width:100%;
        padding:10px;
    }

    .btnAddNew .dx-icon {
        margin-right: 10px !important;
        filter: brightness(4);
        transform: scale(1);
    }

    .ProjectLead, .LeadEstimator, .LeadSuperintendent {
        border-radius: 7px;
        border: 1px solid #D3D3D3;
        padding: 7px;
        height: 36px;
    }
        #updateAllocationDates .dx-checkbox-container {
    display: flex;
    flex-direction: row-reverse;
    font-weight:500;

  }
    #updateAllocationDates .dx-checkbox-icon {
    width: 22px;
    height: 22px;
    margin-top: 2px;
    border-radius: 2px;
    border: 1px solid #ddd;
    background-color: #fff;
    margin-left: 5px;
}
    #saveButton.dx-button-has-icon .dx-icon {
    width: 18px !important;
    height: 18px !important;
    background-position: 0 0;
    background-size: 18px 18px;
    padding: 0;
    font-size: 18px;
    text-align: center;
    line-height: 18px;
    margin-right: 9px;
    margin-left: 0;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var title = "";
    var baseUrl = ugitConfig.apiBaseUrl;
    var ajaxHelperPage = "<%=ajaxHelperPage%>";
    var hasAnyPastAllocation = "<%=this.HasAnyPastAllocation%>" == "True" ? true : false;
    var dataModel = {
        preconStartDate: "",
        preconEndDate: "",
        constStartDate: "",
        constEndDate: "",
        closeoutStartDate: "",
        closeoutEndDate: "",
        onHold: false,
        preconDuration: "",
        constDuration: "",
        closeOutDuration: ""
    };
    var Model = {
        RecordId: "",
        Fields: [{
            FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconStartDate%>",
                Value: dataModel.preconStartDate
            }, {
                    FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.PreconEndDate%>",
                    Value: dataModel.preconEndDate
                }, {
                    FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionStart%>",
                    Value: dataModel.constStartDate
                }, {
                    FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.EstimatedConstructionEnd%>",
                    Value: dataModel.constEndDate
                }, {
                    FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.CloseoutDate%>",
                    Value: dataModel.closeoutEndDate
                }, {
                    FieldName: "<%=uGovernIT.Utility.DatabaseObjects.Columns.TicketOnHold%>",
                Value: dataModel.onHold
            }]
    };
    var ProjectLead = '<%=ProjectLeadUserID%>';
    var LeadEstimator = '<%=LeadEstimatorUserID%>';
    var LeadSuperintendent = '<%=LeadSuprintendentUserID%>';

    function showTooltip(element, str) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.option({
            target: "#" + $(element).attr("id"),
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<span />").attr("id", "name").html($(element).attr("tooltip"))
                );
            }
        });
        tooltip.show();

    }
    function hideTooltip(element) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.hide();
    }

    $(function () {

        $.get("/api/rmmapi/GetProjectAllocations?projectID=" + '<%=TicketId%>', function (data, status) {
            // First, group the data by AssignedToName
            const groupedData = data.Allocations.reduce((acc, cur) => {
                if (!acc[cur.AssignedToName + cur.TypeName]) {
                    acc[cur.AssignedToName + cur.TypeName] = [];
                }
                acc[cur.AssignedToName + cur.TypeName].push(cur);
                return acc;
            }, {});

            // Then, loop through each group and merge the data
            const mergedData = Object.entries(groupedData).map(([key, group]) => {
                
                const mergedObj = {
                    AssignedToName: group[0].AssignedToName,
                    AllocationEndDate: null,
                    AllocationStartDate: null,
                    PctAllocation: null,
                    AssignedTo: group[0].AssignedTo,
                    IsInCloseoutStage: group.some(item => item.IsInCloseoutStage),
                    IsInConstStage: group.some(item => item.IsInConstStage),
                    IsInPreconStage: group.some(item => item.IsInPreconStage),
                    NonChargeable: group[0].NonChargeable,
                    ProjectID: group[0].ProjectID,
                    UserImageUrl: group[0].UserImageUrl,
                    TypeName: group[0].TypeName,
                    PreconTooltip: "",
                    ConstTooltip: "",
                    CloseOutTooltip: "",
                };
                group.forEach(obj => {
                    mergedObj.AllocationEndDate = mergedObj.AllocationEndDate
                        ? new Date(obj.AllocationEndDate) > new Date(mergedObj.AllocationEndDate)
                            ? obj.AllocationEndDate
                            : mergedObj.AllocationEndDate
                        : obj.AllocationEndDate;
                    mergedObj.AllocationStartDate = mergedObj.AllocationStartDate
                        ? new Date(obj.AllocationStartDate) < new Date(mergedObj.AllocationStartDate)
                            ? obj.AllocationStartDate
                            : mergedObj.AllocationStartDate
                        : obj.AllocationStartDate;
                    mergedObj.PctAllocation = mergedObj.PctAllocation
                        ? Math.max(obj.PctAllocation, mergedObj.PctAllocation)
                        : obj.PctAllocation;
                    let startDate = new Date(obj.AllocationStartDate).toLocaleDateString('en-us', { year: "numeric", month: "short", day: 'numeric' });
                    let endDate = new Date(obj.AllocationEndDate).toLocaleDateString('en-us', { year: "numeric", month: "short", day: 'numeric' });
                    if (obj.IsInPreconStage) {
                        mergedObj.PreconTooltip += `Allocation: ${obj.PctAllocation}% ${startDate} to ${endDate} <br>`;
                    }
                    if (obj.IsInConstStage) {
                        mergedObj.ConstTooltip += `Allocation: ${obj.PctAllocation}% ${startDate} to ${endDate} <br>`;
                    }
                    if (obj.IsInCloseoutStage) {
                        mergedObj.CloseOutTooltip += `Allocation: ${obj.PctAllocation}% ${startDate} to ${endDate} <br>`;
                    }
                });
                return mergedObj;
            });

            var moreAllocItems = mergedData.length - 10;
            var globaldata = mergedData.slice(0, 10);

            $('#allcationGrid').dxDataGrid({
                dataSource: globaldata,
                remoteOperations: false,
                searchPanel: {
                    visible: false,
                },
                rowAlternationEnabled: true,
                wordWrapEnabled: true,
                showBorders: false,
                showColumnHeaders: true,
                showColumnLines: false,
                showRowLines: false,
                TextEncoder: true,
                noDataText: "No Data Found",
                columns: [
                    {
                        dataField: "AssignedToName",
                        dataType: "text",
                        caption: "Team Member",
                        allowEditing: false,
                        sortIndex: "0",
                        sortOrder: "asc",
                        cellTemplate: function (container, options) {
                            container.css('display', 'flex');
                            let imageUrl = "";
                            if (options.data.UserImageUrl && options.data.UserImageUrl.indexOf("userNew.png") != -1) {
                                imageUrl = "/Content/Images/RMONE/blankImg.png";
                            }
                            else if (options.data.UserImageUrl != null) {
                                imageUrl = options.data.UserImageUrl;
                            }
                            $(`<img src="${imageUrl}" style="width:28px; border-radius: 50%;"><div style='margin-left:5px;'>${options.data.AssignedToName}</div>`).appendTo(container);
                        },
                        headerCellTemplate: function (header, info) {
                            $(`<div style="color: black;font-size:14px;font-weight:600;margin-left:28px;">${info.column.caption}</div>`).appendTo(header);
                        }
                    },
                    {
                        dataField: "TypeName",
                        dataType: "text",
                        caption: "Role",
                        sortIndex: "1",
                        sortOrder: "asc",
                        width: "30%",
                        headerCellTemplate: function (header, info) {
                            $(`<div style="color: black;font-size:14px;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                        }
                    },
                    {
                        caption: 'Phases',
                        columns: [
                            {
                                dataField: "IsInPreconStage",
                                dataType: "text",
                                caption: "P",
                                cellTemplate: stageIconTemplate,
                                width: '10%',
                                allowFiltering: false,
                                allowSorting: false,
                                headerCellTemplate: function (header, info) {
                                    $(`<div style="color: black;font-size:14px;font-weight:600;margin-left: 9px;">${info.column.caption}</div>`).appendTo(header);
                                }
                            },
                            {
                                dataField: "IsInConstStage",
                                dataType: "text",
                                caption: "C",
                                cellTemplate: stageIconTemplate,
                                width: '10%',
                                allowFiltering: false,
                                allowSorting: false,
                                headerCellTemplate: function (header, info) {
                                    $(`<div style="color: black;font-size:14px;font-weight:600;margin-left: 6px;">${info.column.caption}</div>`).appendTo(header);
                                }
                            },
                            {
                                dataField: "IsInCloseoutStage",
                                dataType: "text",
                                caption: "CO",
                                cellTemplate: stageIconTemplate,
                                width: '10%',
                                allowFiltering: false,
                                allowSorting: false,
                                headerCellTemplate: function (header, info) {
                                    $(`<div style="color: black;font-size:14px;font-weight:600;margin-left: 3px;">${info.column.caption}</div>`).appendTo(header);
                                }
                            },
                        ],
                        headerCellTemplate: function (header, info) {
                            $(`<div style="color: black;font-size:14px;font-weight:600;margin-left: 32px;">${info.column.caption}</div>`).appendTo(header);
                        }
                    },

                ]
            });

            if (moreAllocItems > 0) {
                $("#btnMoreAllocation").dxButton({
                    text: 'More: ' + moreAllocItems,
                    stylingMode: 'text',
                    icon: 'chevrondown',
                    focusStateEnabled: false,
                    onClick: function (e) {
                        var allocationGrid = $('#allcationGrid').dxDataGrid("instance");
                        allocationGrid.option('dataSource', data.Allocations);
                        e.component.option('visible', false);
                    }
                });
            }
        });

        var tooltip = $("#tooltip").dxTooltip({
            encodeHtml: false,
            contentTemplate: function (contentElement) {
                contentElement.append(

                )
            }
        });
        $("#divPrecon").click(function (e) {

            openDateAgent(this);
        });
        $("#divConst").click(function (e) {
            openDateAgent(this);
        });
        $("#divCloseout").click(function (e) {
            openDateAgent(this);
        });

        $('#popup').dxPopup({
            visible: false,
            hideOnOutsideClick: true,
            showTitle: true,
            showCloseButton: true,
            title: "Update Dates",
            width: "auto",
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
            contentTemplate: () => {
                const content = $("<div />");
                content.append(
                    $("<div id='form' />").dxForm({
                        formData: dataModel,
                        title: 'Update Dates',
                        items: [{
                            itemType: 'group',
                            name: 'group',
                            caption: '',
                            colCount: 3,
                            items: [{
                                dataField: 'preconStartDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: undefined,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredPreconStartDate = e.value;
                                        let newdate = new Date(enteredPreconStartDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }

                                        if (enteredPreconStartDate != null) {
                                            if (String(enteredPreconStartDate).startsWith("00")) {
                                                enteredPreconStartDate = enteredPreconStartDate.replace(/^.{2}/g, '20');
                                                e.value = enteredPreconStartDate;
                                                dataModel.preconStartDate = enteredPreconStartDate;
                                            }
                                        }
                                        if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                            $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                //validationRules: [{
                                //    type: 'required',
                                //    message: 'Precon Start date is required',
                                //}],
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                            {
                                dataField: 'preconDuration',
                                editorType: 'dxNumberBox',
                                editorOptions: {
                                    value: undefined,
                                    onFocusOut(e) {
                                        if (e.component.option('value') != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.preconStartDate, e.component.option('value'));
                                            $("#form").dxForm("instance").updateData({ 'preconEndDate': new Date(dataModel.preconEndDate).toLocaleDateString('en-US') });
                                        }
                                    },
                                },
                            },
                            {
                                dataField: 'preconEndDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: undefined,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredPreconEndDate = e.value;
                                        let newdate = new Date(enteredPreconEndDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredPreconEndDate != null) {
                                            if (String(enteredPreconEndDate).startsWith("00")) {
                                                enteredPreconEndDate = enteredPreconEndDate.replace(/^.{2}/g, '20');
                                                e.value = enteredPreconEndDate;
                                                dataModel.preconEndDate = enteredPreconEndDate;
                                            }
                                        }
                                        if (dataModel.preconEndDate != '' && dataModel.preconStartDate != '') {
                                            dataModel.preconDuration = GetDurationInWeek(ajaxHelperPage, dataModel.preconStartDate, dataModel.preconEndDate);
                                            $("#form").dxForm("instance").updateData({ 'preconDuration': dataModel.preconDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                //validationRules: [{
                                //    type: 'required',
                                //    message: 'Precon End date is required',
                                //}],
                                label: {
                                    template: labelTemplate('PreCon End'),
                                },
                            },
                            {
                                dataField: 'constStartDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: dataModel.constStartDate,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredConstStartDate = e.value;
                                        let newdate = new Date(enteredConstStartDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredConstStartDate != null) {
                                            if (String(enteredConstStartDate).startsWith("00")) {
                                                enteredConstStartDate = enteredConstStartDate.replace(/^.{2}/g, '20');
                                                e.value = enteredConstStartDate;
                                                dataModel.constStartDate = enteredConstStartDate;
                                            }
                                        }
                                        if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                            dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                            $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                //validationRules: [{
                                //    type: 'required',
                                //    message: 'Const Start date is required',
                                //}],
                                label: {
                                    template: labelTemplate('Const Start'),
                                },
                            },
                            {
                                dataField: 'constDuration',
                                editorType: 'dxNumberBox',
                                editorOptions: {
                                    value: undefined,
                                    onFocusOut(e) {
                                        if (e.component.option('value') != '' && dataModel.constStartDate != '') {
                                            dataModel.constEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.constStartDate, e.component.option('value'));
                                            $("#form").dxForm("instance").updateData({ 'constEndDate': new Date(dataModel.constEndDate).toLocaleDateString('en-US') });
                                        }
                                    },
                                },
                                label: {
                                    template: labelTemplate('PreCon Start'),
                                },
                            },
                            {
                                dataField: 'constEndDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: dataModel.constEndDate,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredConstEndDate = e.value;
                                        let newdate = new Date(enteredConstEndDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredConstEndDate != null) {
                                            if (String(enteredConstEndDate).startsWith("00")) {
                                                enteredConstEndDate = enteredConstEndDate.replace(/^.{2}/g, '20');
                                                e.value = enteredConstEndDate;
                                            }
                                        }
                                        if (dataModel.constEndDate != '' && dataModel.constStartDate != '') {
                                            dataModel.constDuration = GetDurationInWeek(ajaxHelperPage, dataModel.constStartDate, dataModel.constEndDate);
                                            $("#form").dxForm("instance").updateData({ 'constDuration': dataModel.constDuration });
                                        }
                                        if (e.value != null) {
                                            $.ajax({
                                                type: "GET",
                                                url: "<%= ajaxPageURL %>GetNextWorkingDateAndTime?dateString=" + new Date(e.value).toLocaleDateString('en-US'),
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                async: false,
                                                success: function (message) {
                                                    dataModel.closeoutStartDate = new Date(message).toLocaleDateString('en-US');
                                                    dataModel.closeoutEndDate = new Date(GetEndDateByWorkingDays(ajaxHelperPage, message, "<%=closeoutperiod%>")).toISOString();
                                                    dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                                    $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US'), 'closeoutStartDate': dataModel.closeoutStartDate, 'closeOutDuration': dataModel.closeOutDuration });
                                                },
                                                error: function (xhr, ajaxOptions, thrownError) {

                                                }
                                            });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                label: {
                                    template: labelTemplate('Const End'),
                                },
                                },
                                {
                                    dataField: 'closeoutStartDate',
                                    /*editorType: 'dxDateBox',*/
                                    editorOptions: {
                                        value: dataModel.closeoutStartDate,
                                        format: 'MMM d, yyyy',
                                        readOnly: true,
                                    },
                                    //validationRules: [{
                                    //    type: 'required',
                                    //    message: 'Closeout date is required',
                                    //}],
                                    label: {
                                        template: labelTemplate('Close Out'),
                                    },
                                },
                                {
                                    dataField: 'closeOutDuration',
                                    editorType: 'dxNumberBox',
                                    editorOptions: {
                                        value: undefined,
                                        onFocusOut(e) {
                                            if (e.component.option('value') != '' && dataModel.closeoutStartDate != '') {
                                                dataModel.closeoutEndDate = GetEndDateByWeeks(ajaxHelperPage, dataModel.closeoutStartDate, e.component.option('value'));
                                                $("#form").dxForm("instance").updateData({ 'closeoutEndDate': new Date(dataModel.closeoutEndDate).toLocaleDateString('en-US') });
                                            }
                                        },
                                    },
                                    label: {
                                        template: labelTemplate('PreCon Start'),
                                    },
                                },
                            {
                                dataField: 'closeoutEndDate',
                                editorType: 'dxDateBox',
                                editorOptions: {
                                    value: dataModel.closeoutEndDate,
                                    type: "date",
                                    displayFormat: "MM/dd/yyyy", 
                                    onValueChanged(e) {
                                        var enteredCloseoutEndDate = e.value;
                                        let newdate = new Date(enteredCloseoutEndDate);
                                        if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                                            DevExpress.ui.dialog.alert("Please enter a valid date.");
                                            return;
                                        }
                                        if (enteredCloseoutEndDate != null) {
                                            if (String(enteredCloseoutEndDate).startsWith("00")) {
                                                enteredCloseoutEndDate = enteredCloseoutEndDate.replace(/^.{2}/g, '20');
                                                e.value = enteredCloseoutEndDate;
                                                dataModel.closeoutEndDate = enteredCloseoutEndDate;
                                            }
                                        }
                                        if (dataModel.closeoutEndDate != '' && dataModel.closeoutStartDate != '') {
                                            dataModel.closeOutDuration = GetDurationInWeek(ajaxHelperPage, dataModel.closeoutStartDate, dataModel.closeoutEndDate);
                                            $("#form").dxForm("instance").updateData({ 'closeOutDuration': dataModel.closeOutDuration });
                                        }
                                    },
                                    displayFormat: {
                                        parser: function (value) {
                                            let parts = value.split('/');
                                            if (3 !== parts.length) {
                                                return;
                                            }
                                            return new Date(parts[2].length < 3 ? Number('20' + parts[2]) : Number(parts[2]), Number(parts[0]) - 1, Number(parts[1]))
                                        },
                                        formatter: function (value) {
                                            return DevExpress.localization.date.format(value, 'shortdate');
                                        }

                                    }
                                },
                                //validationRules: [{
                                //    type: 'required',
                                //    message: 'Closeout date is required',
                                //}],
                                label: {
                                    template: labelTemplate('Close Out End'),
                                },
                            },
                            {
                                dataField: 'onHold',
                                editorType: 'dxSwitch',
                                visible: false,
                                editorOptions: {
                                    width: 100,
                                    value: dataModel.onHold,
                                    switchedOffText: "OFF HOLD",
                                    switchedOnText: "ON HOLD",
                                },
                                label: {
                                    template: labelTemplate(''),
                                },
                            }
                            ]
                        }],
                        onContentReady: function (data) {
                            data.element.find("label[for$='preconDuration']").text("Precon Duration(Weeks)");
                            data.element.find("label[for$='constDuration']").text("Const Duration(Weeks)");
                            data.element.find("label[for$='closeOutDuration']").text("Closeout Duration(Weeks)");
                        }
                    }),
                    $("<div class='ml-2' id='updateAllocationDates'>").dxCheckBox({
                        value: false,
                        text: "Change Allocations:"
                    }),
                    $("#saveButton").dxButton({
                        text: 'Save',
                        icon: '/content/Images/save-open-new-wind.png',
                        onClick: function (e) {
                            if (hasAnyPastAllocation && $("#updateAllocationDates").dxCheckBox('option', 'value') == true) {
                                var conflictDialog = DevExpress.ui.dialog.custom({
                                    title: "Alert",
                                    message: `Do you want to shift the past dates?`,
                                    buttons: [
                                        { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                                        { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                                    ]
                                });
                                conflictDialog.show().done(function (dialogResult) {
                                    if (dialogResult == "Ok") {
                                        UpdateRecord($("#updateAllocationDates").dxCheckBox('option', 'value'), true);
                                    }
                                    else if (dialogResult == "Cancel") {
                                        UpdateRecord($("#updateAllocationDates").dxCheckBox('option', 'value'), false);
                                    }
                                });
                            }
                            else {
                                UpdateRecord($("#updateAllocationDates").dxCheckBox('option', 'value'), false);
                            }
                        }
                    })
                );
                return content;
            },
            onDisposing: function (e) {
                dataModel.preconStartDate = "";
                dataModel.preconEndDate = "";
                dataModel.constStartDate = "";
                dataModel.constEndDate = "";
                dataModel.closeoutEndDate = "";
                dataModel.onHold = false;
            }
        });

        $("#toast").dxToast({
            message: "Record Saved Successfully. ",
            type: "info",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });

       

        $("#divLeadUserBox").click(function (e) {
            openUserEditPopup(this);
        });

        $('#userFieldPopup').dxPopup({
            visible: false,
            hideOnOutsideClick: true,
            showTitle: true,
            showCloseButton: true,
            title: "Update Resources",
            width: "300",
            height: "auto",
            resizeEnabled: true,
            dragEnabled: true,
            contentTemplate: () => {
                const content = $("<div />");
                content.append(
                    $(`<div style='padding-bottom: 10px' />`).append(
                        $(`<div><%=divProjectLead.InnerText%></div>`),
                        $("<div id='divProjectLead' />").dxSelectBox({
                            placeholder: '<%=ProjectLeadUser%>',
                            valueExpr: "Id",
                            displayExpr: "Name",
                            searchEnabled: true,
                            showClearButton: true,
                            dataSource: "/api/rmmapi/GetUserList?skipDisabled=true",
                            value: '<%=ProjectLeadUserID%>',
                            width: '100%',
                            onValueChanged: function (e) {
                                //if (typeof e.value !== "undefined") {
                                    
                                //    const Id = e.value;
                                //    ProjectLead = Id;

                                //}
                            }
                    }),
                    ),
                    $(`<div style='padding-bottom: 10px' />`).append(
                        $(`<div><%=divLeadEstimator.InnerText%></div>`),
                        $("<div id='divLeadEstimator' />").dxSelectBox({
                            placeholder: '<%=LeadEstimatorUser%>',
                            valueExpr: "Id",
                            displayExpr: "Name",
                            searchEnabled: true,
                            showClearButton: true,
                            dataSource: "/api/rmmapi/GetUserList?skipDisabled=true",
                            value: '<%=LeadEstimatorUserID%>',
                            width:'100%',
                            onValueChanged: function (e) {
                                //if (typeof e.value !== "undefined") {
                                //    const Id = e.value;

                                //    LeadEstimator = Id;
                                    
                                //}
                            }
                    }),
                    ),
                    $(`<div style='padding-bottom: 10px' />`).append(
                        $(`<div><%=divLeadSuperintendent.InnerText%></div>`),
                        $("<div id='divLeadSuprintendent' />").dxSelectBox({
                            placeholder: '<%=LeadSuperintendentUser%>', 
                            valueExpr: "Id",
                            displayExpr: "Name",
                            searchEnabled: true,
                            showClearButton: true,
                            dataSource: "/api/rmmapi/GetUserList?skipDisabled=true",
                            value: '<%=LeadSuprintendentUserID%>',
                            width: '100%',
                            onValueChanged: function (e) {
                                //if (typeof e.value !== "undefined") {
                                //    const Id = e.value;

                                //    LeadSuperintendent = Id;
                                    
                                //}
                        }
                    }),
                    ),
                    $(`<div style='padding-bottom:10px;float:right;' />`).append(
                        $(`<div id='divSaveUserFields' class='btnAddNew' />`).dxButton({
                            text: 'Save',
                            icon: '/content/Images/save-open-new-wind.png',
                            width:100,
                            onClick: function (e) {
                                loadingPanel.Show();
                                ProjectLead = $("#divProjectLead").dxSelectBox("instance").option('value');
                                LeadEstimator = $("#divLeadEstimator").dxSelectBox("instance").option('value');
                                LeadSuperintendent = $("#divLeadSuprintendent").dxSelectBox("instance").option('value');
                                $.post(baseUrl + `/api/OPMWizard/UpdateLeadUserFields?TicketId=${'<%=TicketId%>'}&ProjectLead=${ProjectLead}&LeadEstimator=${LeadEstimator}&Superintendent=${LeadSuperintendent}`, function (result) {
                                    loadingPanel.Hide();
                                    if (result.IsSuccess && result.ErrorMessages.length > 0) {
                                        var errorMessage = "Resources are assigned. Below are the errors occurred while creating allocations for them.<br/><br/>";
                                        result.ErrorMessages.forEach(function (item, index) {
                                            errorMessage = errorMessage + item + "<br/>";
                                        });
                                        refreshUserFields(result.Data);
                                        var res = DevExpress.ui.dialog.alert(errorMessage, "Assign Resources & Create Allocation.");
                                        res.done(function () {
                                            const popup = $("#userFieldPopup").dxPopup("instance");
                                            popup.hide();
                                            return;
                                        });
                                        return;
                                    } else if (result.IsSuccess && result.ErrorMessages.length == 0) {
                                        refreshUserFields(result.Data);
                                        const popup = $("#userFieldPopup").dxPopup("instance");
                                        popup.hide();
                                    } else {
                                        DevExpress.ui.dialog.alert("Failed to Assign Resources or Create Allocations.", "Error");
                                        return;
                                    }
                                });
                            }
                        }),
                    ),
                );
                return content;
            },
            onHidden: function (e) {
                //setTimeout(function () {
                //    location.reload();
                //}, 500);
            }
        });
    });

    function openUserEditPopup(obj) {
        const popup = $("#userFieldPopup").dxPopup("instance");
        popup.show();
    }

    function openDateAgent(obj) {
        Model.RecordId = '<%=TicketId%>';
        title = $(obj).attr("ticketTitle");
        $.get("/api/rmone/GetProjectDates?TicketId=" + Model.RecordId, function (data, status) {
            dataModel.preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : data.PreconStart;
            dataModel.preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : data.PreconEnd;
            dataModel.constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : data.ConstStart;
            dataModel.constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : data.ConstEnd;
            dataModel.closeoutStartDate = data.CloseoutStart == '0001-01-01T00:00:00' ? '' : new Date(data.CloseoutStart).toLocaleDateString('en-US');
            dataModel.closeoutEndDate = data.Closeout == '0001-01-01T00:00:00' ? '' : data.Closeout;
            dataModel.onHold = data.OnHold;
            dataModel.preconDuration = parseInt(data.PreconDuration) > 0 ? data.PreconDuration : "";
            dataModel.constDuration = parseInt(data.ConstDuration) > 0 ? data.ConstDuration : "";
            dataModel.closeOutDuration = parseInt(data.CloseOutDuration) > 0 ? data.CloseOutDuration : ""; 

            const popup = $("#popup").dxPopup("instance");
            popup.show();
            let form = $("#form").dxForm("instance");
            form.option("formData", dataModel);
            //form.itemOption("group", "caption", title);
        });
    }

    function labelTemplate(iconName) {
        return (data) => $(`<div><i class='dx-icon dx-icon-${iconName}'></i>${data.text}</div>`);
    }
   
    function UpdateRecord(updateAllocations, updatePastAllocations) {
        //[+][30-10-2023][SANKET][Added validation condition]
        if ((dataModel.preconEndDate != null && dataModel.preconEndDate != "") && (dataModel.preconStartDate == null || dataModel.preconStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Precon Start Date is required.", "Error!");
            return;
        }
        if (dataModel.preconEndDate != null) {
            if (new Date(dataModel.preconStartDate) > new Date(dataModel.preconEndDate)) {
                DevExpress.ui.dialog.alert("Precon End Date should be after the Precon Start Date.", "Error!");
                return;
            }
        }
        //[+][30-10-2023][SANKET][Added validation condition]
        if ((dataModel.constEndDate != null && dataModel.constEndDate != "") && (dataModel.constStartDate == null || dataModel.constStartDate == "")) {
            DevExpress.ui.dialog.alert("Entry of Construction Start Date is required.", "Error!");
            return;
        }
        if (new Date(dataModel.constStartDate) > new Date(dataModel.constEndDate)) {
            //[+][30-10-2023][SANKET][Added validation condition]
            if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
                DevExpress.ui.dialog.alert("Entry of Construction End Date is required.", "Error!");
                return;
            }
            DevExpress.ui.dialog.alert("Construction End Date should be after the Construction Start Date.", "Error!");
            return;
        }
        
        if (new Date(dataModel.constEndDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout Date should be after the Construction End Date.", "Error!");
            return;
        }
        if (new Date(dataModel.closeoutStartDate) > new Date(dataModel.closeoutEndDate)) {
            DevExpress.ui.dialog.alert("Closeout End Date should be after the Closeout Start Date.", "Error!");
            return;
        }
        if (dataModel.constEndDate == null || dataModel.constEndDate == "") {
            dataModel.closeoutStartDate = null;
            dataModel.closeoutEndDate = null;
        }

        var arrDates = [
            ['Precon Start Date', dataModel.preconStartDate == null ? "" : dataModel.preconStartDate],
            ['Precon End Date', dataModel.preconEndDate == null ? "" : dataModel.preconEndDate],
            ['Construction Start Date', dataModel.constStartDate == null ? "" : dataModel.constStartDate],
            ['Construction End Date', dataModel.constEndDate == null ? "" : dataModel.constEndDate],
            ['Closeout End Date', dataModel.closeoutEndDate == null ? "" : dataModel.closeoutEndDate]
        ];
        for (let i = 0; i < arrDates.length; i++) {
            let newdate = new Date(arrDates[i][1]);
            if (arrDates[i][1] != "") {
                if (newdate == 'Invalid Date' || String(newdate.getFullYear()).length > 4) {
                    DevExpress.ui.dialog.alert("Please enter a valid " + arrDates[i][0]);
                    return;
                }
                Model.Fields[i].Value = newdate.toLocaleDateString('en-US');
            }
        }
        //Model.Fields[0].Value = dataModel.preconStartDate;
        //Model.Fields[1].Value = dataModel.preconEndDate;
        //Model.Fields[2].Value = dataModel.constStartDate;
        //Model.Fields[3].Value = dataModel.constEndDate;
        //Model.Fields[4].Value = dataModel.closeoutEndDate;
        Model.Fields[5].Value = dataModel.onHold == true ? '1' : '0';
        Model.UpdateAllocations = updateAllocations;
        Model.UpdatePastAllocations = updatePastAllocations;
        $.ajax({
            type: "POST",
            url: "<%= ajaxPageURL %>UpdateRecord",
                data: JSON.stringify(Model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (message) {
                    if (message.Status == true) {
                        dataModel.preconStartDate = "";
                        dataModel.preconEndDate = "";
                        dataModel.constStartDate = "";
                        dataModel.constEndDate = "";
                        dataModel.closeoutEndDate = "";
                        dataModel.closeoutStartDate = "";
                        dataModel.onHold = false;
                        //var resultJson = $.parseJSON(message);


                        $("#toast").dxToast("show");
                        const popup = $("#popup").dxPopup("instance");
                        popup.hide();
                        let IsParent = <%=IsParentModuleWebPart%>;
                        //console.log(IsParent);
                        if (<%=IsParentModuleWebPart%> == 1) {
                            $.cookie("TicketSelectedTab", 1, { path: "/" });
                            window.parent.location.reload();
                        }
                        else
                        {
                            setTimeout(function ()
                            {
                                location.reload();
                            }, 1000);
                        }
                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {

                    //   alert(ajaxOptions);
                }
            });
    }

    function refreshUserFields(data) {
        
        data.forEach(function (item, index) {
            if (item.Field == "<%=DatabaseObjects.Columns.ProjectLeadUser%>") {
                ProjectLead = item.Name;
                $(".ProjectLead").text(ProjectLead);
            }
            else if (item.Field == "<%=DatabaseObjects.Columns.LeadEstimatorUser%>") {
                LeadEstimator = item.Name;
                $(".LeadEstimator").text(LeadEstimator);
            }
            else if (item.Field == "<%=DatabaseObjects.Columns.LeadSuperintendentUser%>") {
                LeadSuperintendent = item.Name;
                $(".LeadSuperintendent").text(LeadSuperintendent);
            }
        })
    }

    const stageIconTemplate = function (container, options) {
        if (options.column.dataField == 'IsInPreconStage') {
            if (options.data.IsInPreconStage)
                $("<i id='" + options.data.AssignedTo + "precon" + Math.floor((Math.random() * 2000) + 1) + "' tooltip ='" + options.data.PreconTooltip + "' onmouseover='showTooltip(this)' onmouseout='hideTooltip(this)' class='fa fa-circle' style='font-size: 22px; color:#52BED9'></i>").appendTo(container);
            else
                $("<i class='far fa-circle' style='font-size: 22px; color:#52BED9'></i>").appendTo(container);
        }
        else if (options.column.dataField == 'IsInConstStage') {
            if (options.data.IsInConstStage)
                $("<i id='" + options.data.AssignedTo + "const" + Math.floor((Math.random() * 2000) + 1) + "' class='fa fa-circle' tooltip ='" + options.data.ConstTooltip + "' onmouseover='showTooltip(this)' onmouseout='hideTooltip(this)' style='font-size: 22px; color:#005C9B'></i>").appendTo(container);
            else
                $("<i class='far fa-circle' style='font-size: 22px; color:#005C9B'></i>").appendTo(container);
        }
        else if (options.column.dataField == 'IsInCloseoutStage') {
            if (options.data.IsInCloseoutStage)
                $("<i id='" + options.data.AssignedTo + "closeout" + Math.floor((Math.random() * 2000) + 1) + "' class='fa fa-circle' tooltip ='" + options.data.CloseOutTooltip + "' onmouseover='showTooltip(this)' onmouseout='hideTooltip(this)' style='font-size: 22px; color:#351B82'></i>").appendTo(container);
            else
                $("<i class='far fa-circle' style='font-size: 22px; color:#351B82'></i>").appendTo(container);
        }
    }
    $(document).ready(function () {
        let ticketStage = "<%=this.CurrentTicketStage%>";
            if (ticketStage == "Precon") {
                $(".project-phase-inner").css("background-color", "#52bed9");
            }
            else if (ticketStage == "Const") {
                $(".project-phase-inner").css("background-color", "#005c9b");
            }
            else if (ticketStage == "Closeout") {
                $(".project-phase-inner").css("background-color", "#351b82");
            }       
            else if(ticketStage == "OnHold")
            {
                $(".project-phase-inner").css("background-color", "red");
            }
        });
    


</script>
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text=" Please Wait ..." ClientInstanceName="loadingPanel" Modal="True">
    <Image Url="~/Content/Images/ajax-loader.gif"></Image>
</dx:ASPxLoadingPanel>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12 p-0">
<div class="dashboard-panel-new">
    <div class="dashboard-panel-main">
        <div class="project-title">
            <%=TitleLink%>
        </div>
        <div class="project-id">
            <%=this.TicketId%>
        </div>
        <%--<div class="project-phase">
            <span class="project-phase-inner"><%=this.CurrentTicketStage%></span>
        </div>--%>
        <div class="row" style="text-align: center;padding-bottom:8px;">
            <div class="col-md-2 col-xs-6 col-sm-4 data-value">
                <div id="divCRMCompanyLookup" runat="server" class="dataTitle"> </div>
                <div class="whiteSpace"><%=this.CRMCompanyLookup%></div>
            </div>
            <div class="col-md-2 col-xs-6 col-sm-4 data-value">
                <div id="divSector" runat="server" class="dataTitle">Sector  </div>
                <div class="whiteSpace"><%=this.Sector%></div>
            </div>
            <div class="col-md-1 col-xs-6 col-sm-4 data-value">
                <div id="divProjectType" runat="server" class="dataTitle">Project Type  </div>
                <div class="whiteSpace"><%=ProjectType%></div>
            </div>
            <div class="col-md-2 col-xs-6 col-sm-4 data-value project-phase">
                <span class="project-phase-inner"><%=this.TicketStageTitle%></span>
            </div>
            <div class="col-md-1 col-xs-6 col-sm-4 data-value">
                <div id="divERPJobID" runat="server" class="dataTitle">CMiC #</div>
                <div class="whiteSpace"><%=ERPJobID%></div>
            </div>
            <div class="col-md-2 col-xs-6 col-sm-4 data-value">
                <div id="divERPJobIDNC" runat="server" class="dataTitle">CMiC NCO #</div>
                <div class="whiteSpace"><%=ERPJobIDNC%></div>
            </div>
            <div class="col-md-2 col-xs-6 col-sm-4 data-value">
                <div id="divSalesForceId" runat="server" class="dataTitle">Salesforce ID</div>
                <div class="whiteSpace"><%=this.SalesForceId%></div>
            </div>
        </div>
        <div class="row rowStyle">
            <div id="divPrecon" class="col-md-3 col-xs-6 col-sm-4 data-value boxAlignPrecon">
                <div class="ticket-stage" style="color: #52bed9;">Precon</div>
                <div class="whiteSpace-1"><span id="PreconStartDateCss" runat="server">
                    <%=string.IsNullOrWhiteSpace(this.PreconStartDate) ? "<span class='rcorners2 whitecolor'>12, feb 2022</span>" : this.PreconStartDate %></span>
                    <span class="vl" style="border-left: 2px solid #52bed9;"></span>
                    <span id="PreconEndDateCss" runat="server"><%=string.IsNullOrWhiteSpace(this.PreconEndDate) ? "<span class='rcorners2 whitecolor'>12, feb 2022</span>" : this.PreconEndDate %></span>

                </div>
            </div>
            <div id="divConst" class="col-md-3 col-xs-6 col-sm-4 data-value">
                <div class="ticket-stage" style="color: #005c9b">Const.</div>
                <div class="whiteSpace-1"><span id="ConstStartDateCss" runat="server"><%=string.IsNullOrWhiteSpace(this.ConstStartDate) ? "<span class='rcorners2 whitecolor'>12, feb 2022</span>" : this.ConstStartDate %></span>
                    <span class="vl" style="border-left: 2px solid #005c9b;"></span>
                    <span id="ConstEndDateCss" runat="server"><%=string.IsNullOrWhiteSpace(this.ConstEndDate) ? "<span class='rcorners2 whitecolor'>12, feb 2022</span>" : this.ConstEndDate %></span>

                </div>
            </div>
            <div id="divCloseout" class="col-md-3 col-xs-6 col-sm-4 data-value boxAlignClosedOut">
                <div class="ticket-stage" style="color: #351b82;">Closeout</div>
                <div class="whiteSpace-1">
                    <span id="CloseoutStartDateCss" runat="server"><%=string.IsNullOrWhiteSpace(this.CloseoutStartDate) ? "</span><span class='rcorners2 whitecolor'>12, feb 2022</span>" : this.CloseoutStartDate %></span>
                    <span class="vl" style="border-left: 2px solid #351b82;"></span><span id="CloseoutEndDateCss" runat="server"><%=string.IsNullOrWhiteSpace(this.CloseoutEndDate) ? "<span class='rcorners2 whitecolor'>12, feb 2022</span>" : this.CloseoutEndDate %></span>

                </div>
            </div>
        </div>
    </div>
</div>
        </div>
    </div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12 p-0">
<div class="dashboard-panel-new">
    <div class="dashboard-panel-main mt-1">
        <div class="discription-title">Description</div>
        <div class="discription-value">
            <%=ProjectDescription%>
        </div>
    </div>
</div>
        </div>
    </div>
<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12 p-0">
<div class="dashboard-panel-new">
    <div class="dashboard-panel-main mt-1">
        <div class="discription-title">Tags</div>
        <div class="discription-value">
            <ugit:AddProjectExperienceTags runat="server" ID="AddProjectExperienceTags" />
        </div>
    </div>
</div>
        </div>
    </div>
<div class="row">
    <div class="col-md-4 col-sm-12 col-xs-12 p-0 mt-1">
        <div class="dashboard-panel-new pr-0">
            <div class="dashboard-panel-main" style="height:80px;">
                <div id="divLeadUserBox" class="d-flex">
                    <div class="userbox">
                        <div id="divProjectLead" runat="server" class="dataTitle">Project Lead</div>
                        <div class="whiteSpace ProjectLead" ><%=ProjectLeadUser%></div>
                    </div>
                    <div class="userbox">
                        <div id="divLeadEstimator" runat="server" class="dataTitle">Lead Estimator</div>
                        <div class="whiteSpace LeadEstimator" ><%=LeadEstimatorUser%></div>
                    </div>
                    <div class="userbox">
                        <div id="divLeadSuperintendent" runat="server" class="dataTitle">Lead Superintendent</div>
                        <div class="whiteSpace LeadSuperintendent" ><%=LeadSuperintendentUser%></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="dashboard-panel-new pr-0">
            <div class="dashboard-panel-main" style="height:612px;">

                <div id="allcationGrid"></div>
                <div id="btnMoreAllocation">
                </div>
            </div>

        </div>
        
    </div>
    <div class="col-md-4 col-sm-12 col-xs-12 p-0 mt-1">
        <div class="dashboard-panel-new pr-0">
            <div class="dashboard-panel-main d-flex-data" style="height: 290px;">
                <div class="discription-title">General</div>
                <div class="display-data row">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divAddress" runat="server" class="dataTitle">Address</div>
                        <div class="whiteSpace"><%=Address%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divCity" runat="server" class="dataTitle">City</div>
                        <div class="whiteSpace"><%=City%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divZip" runat="server" class="dataTitle">Zip</div>
                        <div class="whiteSpace"><%=ZipCode%></div>
                    </div>
                </div>
                <div class="display-data row">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divNetRentableSqFt" runat="server" class="dataTitle">Net Rentable Sq Ft</div>
                        <div class="whiteSpace"><%=NetRentableSqFt%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divContractType" runat="server" class="dataTitle">Contract Type</div>
                        <div class="whiteSpace"><%=this.ContractType %></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divRetailSqftNum" runat="server" class="dataTitle">Gross Sq FT</div>
                        <div class="whiteSpace"><%=RetailSqftNum%></div>
                    </div>
                </div>
                <div class="display-data row">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divApproxContractValue" runat="server" class="dataTitle">Contract Cost</div>
                        <div class="whiteSpace">$<%=ApproxContractValue%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <%--<div id="divResourceHoursBilled" runat="server" class="dataTitle">Resource Hours Billed</div>--%>
                        <div id="divAwardedLossDate" runat="server" class="dataTitle">Award/Loss Date</div>
                        <%--<div class="whiteSpace">$<%=ResouceHoursBilled%></div>--%>
                        <div class="whiteSpace"><%=AwardedLossDate%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div class="dataTitle">Proposal Type</div>
                        <div class="whiteSpace"><%=OpportunityTypeChoice %></div>
                        <%--<div class="dataTitle">Estimating Docs</div>--%>
                        <%--<div class="whiteSpace doc-style">14</div>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="dashboard-panel-new pr-0">
            <div class="dashboard-panel-main d-flex-data" style="height: 250px;">
                <div class="discription-title">Analytics</div>
                <div class="display-data">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div class="dataTitle">Forecasted Acquisition Cost</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : "$" + AcquisitionCost%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divComplexity" runat="server" class="dataTitle">Complexity</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : ProjectComplexityChoice %></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div class="dataTitle">Volatility</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : Convert.ToString(Volatility) %></div>
                        <%--<div class="progress-bar-1">
                                    <progress value="75" min="0" max="100" style="visibility: hidden; height: 0; width: 0;">75%</progress>
                                    75
                                </div>--%>
                    </div>
                    </div>
                    <div class="display-data">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divActualAcquisitionCost" runat="server" class="dataTitle">Actual Acquisition Cost</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : "$" + ActualAcquisitionCost%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divForecastedProjectCost" runat="server" class="dataTitle">Forecasted Project Cost</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : "$" + ForecastedProjectCost%></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divActualProjectCost" runat="server" class="dataTitle">Actual Project Cost</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : "$" + ActualProjectCost%></div>
                    </div>
                </div>
                <div class="display-data">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divResourceHoursPrecon" runat="server" class="dataTitle">Resource 'Hours' Precon</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : Convert.ToString(ResourceHoursPrecon) %></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divResourceHoursBilledtoDate" runat="server" class="dataTitle">Resource Hours Billed to Date</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : Convert.ToString(ResourceHoursBilledtoDate) %></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divResourceHoursActual" runat="server" class="dataTitle">Resource 'Hours' Actual</div>
                        <div class="whiteSpace"><%=HideAnalyticsValue == true ? "N/A" : Convert.ToString(ResourceHoursActual) %></div>
                    </div>
                </div>
            </div>
        </div>
        <div class="dashboard-panel-new pr-0">
            <div class="dashboard-panel-main d-flex-data" style="height: 140px;">
                <div class="discription-title">Projection</div>
                <div class="display-data">
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divResourceHoursRemaining" runat="server" class="dataTitle">Resource 'Hours' Remaining</div>
                        <div class="whiteSpace"><%=ResourceHoursRemaining %></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divTotalResourceHours" runat="server" class="dataTitle">Total Resource Hours</div>
                        <div class="whiteSpace"><%=TotalResourceHours %></div>
                    </div>
                    <div class="col-md-4 col-sm-12 col-xs-12 data-value inline-grid-data">
                        <div id="divTotalResourceCost" runat="server" class="dataTitle">Total Resource Cost</div>
                        <div class="whiteSpace">$<%=TotalResourceCost %></div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="col-md-4 col-sm-12 col-xs-12 p-0 mt-1">
        <div class="dashboard-panel-new pr-3">
            <div class="col-height dashboard-panel-main">
                <ugit:ModuleConstraintsListDx runat="server" ID="ModuleConstraintsListDx" />
            </div>
        </div>
    </div>
</div>

<div id="saveButton" class="btnAddNew mb-3" style="float:right;font-size:14px;margin-right:-5px;">
</div>
<div id="popup"></div>
<div id="toast"></div>
<div id="tooltip" class="tooltip">
</div>
<div id="userFieldPopup">

</div>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .whitecolor {
    color:#fff;
    }
    .dx-datagrid-nodata {
        text-align: center;
        font-weight: 500;
        color: black;
        font-family: 'Roboto', sans-serif !important;
    }
    .px-3 {
        padding-left: 0rem !important; 
        padding-right: 0rem !important; 
    }
    .glyphicon {
        color: black;
        font-size: 12px;
    }
    .boxAlignClosedOut {
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: flex-start;
    }
    .boxAlignPrecon{
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: flex-end;
    }
    .rowStyle {
        text-align: center;
        padding-bottom: 8px;
        display: flex;
        justify-content: center;
        flex-wrap:wrap;
    }
    .d-flex-data {
        display: flex !important;
        flex-direction: column !important;
        justify-content: space-evenly !important;
    }
    .doc-style {
        background: orange;
        padding: 5px;
        margin-left: 50px !important;
        margin-right: 50px !important;
        color: white;
        border-radius: 30px;
    }

    .col-height {
        height: 704px;
    }

    .project-title {
        font-size: 28px;
        text-align: center;
        font-weight: 600;
        color:black;
    }

    .project-id {
        font-size: 24px;
        text-align: center;
        font-weight: 500;
        color:black;
    }

    .project-phase {
        font-size: 17px;
        text-align: center;
        font-weight: 500;
        padding-bottom: 10px;
        margin-top: 10px;
    }

    .project-phase-inner {
        border: 1px solid;
        padding: 7px 10px;
        border-radius: 22px;
        color: white;
    }

    .inline-grid-data {
        display: inline-table;
    }

    .dataTitle {
        color: gray;
        font-size: 12px;
    }

    .whiteSpace {
        white-space: pre-line;
        font-weight: 500;
        font-size: 13px;
        margin: 6px 0px;
        color:black;
    }
    .whiteSpace-1 {
        font-weight: 500;
        font-size: 13px;
        margin: 6px 0px;
        color:black;
    }
    .display-data {
        text-align: center;
        display: flex;
        justify-content: space-around;
    }

    .discription-title {
        text-align: center;
        font-size: 24px;
        font-weight: 600;
        padding: 10px;
        color:black;
    }

    .discription-value {
        text-align: center;
        font-size: 14px;
        padding:5px 15px 10px 15px;
        color:black;
    }

    .vl {
        border-left: 2px solid gray;
        margin-left: 12px;
        padding-left: 10px;
        padding-top: 5px;
        padding-bottom: 5px;
    }

    .dashboard-panel-new {
        border: none !important;
        padding-top: 6px;
        padding-bottom: 6px
    }

    .dashboard-panel-main, .dashboard-panel-main-mini, .dashboard-panel-main-notmove:not(.panelDashboard) {
        width: 100%;
        background-color: #FFFFFF;
        border-radius: 0px;
        box-shadow: 10px 12px 20px 9px #ddd;
        /* cursor: pointer; */
    }

    .ticket-stage {
        font-size: 20px;
        font-weight: 600;

    }

    .progress-bar-1 {
        width: 100px;
        height: 100px;
        border-radius: 50%;
        background: radial-gradient(closest-side, white 79%, transparent 80% 100%), conic-gradient(hotpink 75%, pink 0);
    }

    #btnMoreAllocation {
        margin-left: 35%;
    }
</style>

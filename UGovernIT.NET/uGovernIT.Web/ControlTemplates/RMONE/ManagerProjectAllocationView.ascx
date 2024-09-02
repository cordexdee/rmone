<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManagerProjectAllocationView.ascx.cs" Inherits="uGovernIT.Web.ManagerProjectAllocationView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">  
    var dataModel = {
        preconStartDate: "",
        preconEndDate: "",
        constStartDate: "",
        constEndDate: "",
        closeoutStartDate: "",
        closeoutEndDate: "",
        onHold: false,
    };
    var ajaxHelperPage = "<%=ajaxHelperPage%>";
    function showTooltip(element, str)
    {
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
    //$(document).ready(function () {
    //    setTimeout(function () {
    //        $(".jqtooltip").tooltip({
    //            //hide: { duration: 4000, effect: "fadeOut", easing: "easeInExpo" },
    //            position: {
    //                my: "left",
    //                at: "bottom",
    //            },
    //            content: function () {
    //                var title = $(this).attr("title");
    //                if (title)
    //                    return title.replace(/\n/g, "<br/>");
    //            }
    //        });
    //    }, 3000)
    //});
    
    var Model = {
        RecordId: "",
        Fields: [
            {
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
            }
        ]
    };
    function labelTemplate(iconName) {
        return (data) => console.log(data.text); $(`<div><i class='dx-icon dx-icon-${iconName}'></i>${data.text}</div>`);
    }
    function openDateAgent(ticketId) {
        Model.RecordId = ticketId;
        $.get("/api/rmone/GetProjectDates?TicketId=" + ticketId, function (data, status) {
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
            dataModel.hasAnyPastAllocation = data.HasAnyPastAllocation; 
            const popup = $("#popup").dxPopup("instance");
            popup.option("title", "Update Dates for " + ticketId);
            popup.show();
            let form = $("#form").dxForm("instance");
            form.option("formData", dataModel);
            //form.itemOption("group", "caption", title);
        });
    }
    function OpenProjectTeam(ticketId) {
        var path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + ticketId + "&module=CPR";
        window.parent.UgitOpenPopupDialog(path, "moduleName=CPR", "Project Allocation: " + ticketId, '85', '85', false, escape(window.location.href));

    }
    $(function () {
        var cHeight = "<%= Height != null && Height.Value > 0 ? Convert.ToInt32(Height.Value) - 40 : 500%>";
        var projectType = "opportunities";
        const tabs = [
            { text: 'Opportunities: <%=this.UserOpportunitiesCount%>' },
                { text: 'Tracked Work: <%=this.UserTrackedWorkCount%>' },
                { text: 'Projects: <%=this.UserOnGoingWorkCount%>' }
        ];
        UserProjectDetailes();
        const showLoadPanel = function () {
            loadPanel.show();
            UserProjectDetailes();
        };
        var tooltip = $("#tooltip").dxTooltip({
            encodeHtml: false,
            contentTemplate: function (contentElement) {
                contentElement.append(
                    
                )
            }
        });
        const loadPanel = $('.loadpanel').dxLoadPanel({
            shadingColor: 'rgba(0,0,0,0.4)',
            position: { of: '#employee' },
            visible: false,
            showIndicator: true,
            showPane: true,
            shading: true,
            hideOnOutsideClick: false
        }).dxLoadPanel('instance');

        $('.tabs-container').dxTabs({
            dataSource: tabs,
            selectedIndex: 0,
            onItemClick: function (e) {
                if (e.itemIndex == 0) {
                    projectType = "opportunities";
                }
                if (e.itemIndex == 1) {
                    projectType = "trackedwork";
                }
                if (e.itemIndex == 2) {
                    projectType = "ongoingwork";
                }
                showLoadPanel();
            }
        });

        $(".tabs-container").parent().parent().removeAttr("align");
            
        function UserProjectDetailes() {
            $.get("/api/rmmapi/GetUserProjectDetails?projectType=" + projectType, function (data, status) {
                $("#divManagerProjectList").dxList({
                    dataSource: data,
                    height: cHeight,
                    grouped: false,
                    showScrollbar: 'always',
                    itemTemplate: function (data, index) {
                        var container = $(`<div class="dashboard-panel-new"></div>`);
                        if (data.FilledAllocationCount == data.TotalAllocationCount) {
                            container.append(`<div onclick="OpenProjectTeam('${data.ProjectID}')" class="cardallocation blueCircle">${data.FilledAllocationCount}/${data.TotalAllocationCount}</div>`);
                        }
                        else {
                            container.append(`<div onclick="OpenProjectTeam('${data.ProjectID}')" class="cardallocation redCircle">${data.FilledAllocationCount}/${data.TotalAllocationCount}</div>`);
                        }

                        var selectedUsers = "";
                        $.each(data.Allocations, function (index, data) {
                            if (data.AssignedToName != '') {
                                selectedUsers += data.AssignedTo + ";";
                            }
                        });
                        var compareResumeBtn = $(`<div id='${data.ProjectID}compareResume' class='compare-resume-position' />`).append(
                            $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                icon: '/content/Images/RMONE/compareresume.png',
                                hint: 'Compare Resume',
                                width: 0,
                                onClick() {
                                    window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=buildprofile&ProjectID=' + data.ProjectID, '', 'Build Profile', '95', '95', "", false);
                                },
                            })
                        );
                        container.append(compareResumeBtn);
                        var ganttViewBtn = $(`<div id='${data.ProjectID}openGanttView' class='gantt-view-position' />`).append(
                            $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                icon: '/content/Images/ganttBlackNew.png',
                                hint: 'Open Gantt View',
                                width: 0,
                                onClick() {
                                    var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&RequestFromProjectAllocation=true&SelectedUsers=" + selectedUsers.replaceAll(';', ',');
                                    window.parent.UgitOpenPopupDialog(url, "", "Timeline for Users Assigned to " + data.ProjectID, "95", "95", "", false);
                                },
                            })
                        );
                        container.append(ganttViewBtn);

                        var topPart = $(`<div>
                                        <div class="project-title" title="${data.ProjectTitle.split(';')[1]}">
                                            ${data.TitleLink}
                                        </div>
                                        <div class="project-id">
                                            ${data.ProjectID}
                                        </div>
                                            
                                        <div class="row rowStyle" onclick="openDateAgent('${data.ProjectID}')">
                                            <div id="${data.ProjectID}divPrecon" class="col-md-4 col-xs-6 col-sm-4 data-value boxAlignPrecon">
                                                <div class="ticket-stage" style="color: #52bed9;">Precon</div>
                                                <div class="whiteSpace-1"><span id="${data.ProjectID}PreconStartDateCss">
                                                    ${data.PreconStartDate}</span><span class="vl" style="border-left: 2px solid #52bed9;"></span><span id="${data.ProjectID}PreconEndDateCss" > ${data.PreconEndDate}</span></div>
                                            </div>
                                            <div id="${data.ProjectID}divConst" class="col-md-4 col-xs-6 col-sm-4 data-value">
                                                <div class="ticket-stage" style="color: #005c9b">Const.</div>
                                                <div class="whiteSpace-1"><span id="${data.ProjectID}ConstStartDateCss" >
                                                    ${data.ConstStartDate}</span><span class="vl" style="border-left: 2px solid #005C9B;"></span><span id="${data.ProjectID}PreconEndDateCss" > ${data.ConstEndDate}</span></div>
                                            </div>
                                            <div id="${data.ProjectID}divCloseout" class="col-md-4 col-xs-6 col-sm-4 data-value boxAlignClosedOut">
                                                <div class="ticket-stage" style="color: #351b82;">Closeout</div>
                                                <div class="whiteSpace-1">
                                                    <span id="${data.ProjectID}CloseoutStartDateCss">
                                                    ${data.CloseOutStartDate}</span><span class="vl" style="border-left: 2px solid #351B82;"></span><span id="${data.ProjectID}PreconEndDateCss" > ${data.CloseOutEndDate}</span></div>
                                            </div>
                                        </div>
                                    </div>
                                `);
                        container.append(topPart);

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


                        var allocation = $(`<div id="${data.ProjectID}allocations">`).dxDataGrid({
                            dataSource: mergedData,
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
                                        let imageUrl = "";
                                        if (options.data.UserImageUrl && options.data.UserImageUrl.indexOf("userNew.png") != -1) {
                                            imageUrl = "/Content/Images/RMONE/blankImg.png";
                                        }
                                        else {
                                            imageUrl = options.data.UserImageUrl;
                                        }
                                        $(`<img src="${imageUrl}" class='teamMember_image'><div style='margin-left:5px;display:inline;'>${options.data.AssignedToName}</div>`).appendTo(container);
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
                                    width: "35%",
                                    headerCellTemplate: function (header, info) {
                                        $(`<div style="color: black;font-size:14px;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                //{
                                //    dataField: "PctAllocation",
                                //    dataType: "text",
                                //    caption: "Alloc",
                                //    sortIndex: "1",
                                //    sortOrder: "asc",
                                //    width: "15%",
                                //    cellTemplate: function (container, options) {
                                //        $(`<div>${options.data.PctAllocation}%</div>`).appendTo(container);
                                //    },
                                //    headerCellTemplate: function (header, info) {
                                //        $(`<div style="color: black;font-size:14px;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                                //    }
                                //},
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

                            ]
                        });
                        container.append(allocation);
                        return container;
                    },
                    onItemRendered: function (e) {
                        var itemContent = e.itemElement.find('.dx-list-item-content');
                        if (itemContent != null && itemContent.html().length == 0) {
                            itemContent.hide();
                            itemContent.parent().parent().prev().addClass('data-content-hide');
                        }
                    },
                }).dxList('instance');;
                loadPanel.hide();
            });
        }
        const stageIconTemplate = function (container, options) {
            if (options.column.dataField == 'IsInPreconStage') {
                if (options.data.IsInPreconStage)
                    $("<i id='"+options.data.AssignedTo + "precon" + Math.floor((Math.random() * 2000) + 1) +"' tooltip ='" + options.data.PreconTooltip + "' onmouseover='showTooltip(this)' onmouseout='hideTooltip(this)' class='fa fa-circle' style='font-size: 22px; color:#52BED9'></i>").appendTo(container);
                else
                    $("<i class='far fa-circle' style='font-size: 22px; color:#52BED9'></i>").appendTo(container);
            }
            else if (options.column.dataField == 'IsInConstStage') {
                if (options.data.IsInConstStage)
                    $("<i id='"+options.data.AssignedTo + "const" + Math.floor((Math.random() * 2000) + 1) +"' class='fa fa-circle' tooltip ='" + options.data.ConstTooltip + "' onmouseover='showTooltip(this)' onmouseout='hideTooltip(this)' style='font-size: 22px; color:#005C9B'></i>").appendTo(container);
                else
                    $("<i class='far fa-circle' style='font-size: 22px; color:#005C9B'></i>").appendTo(container);
            }
            else if (options.column.dataField == 'IsInCloseoutStage') {
                if (options.data.IsInCloseoutStage)
                    $("<i id='"+options.data.AssignedTo + "closeout" + Math.floor((Math.random() * 2000) + 1) +"' class='fa fa-circle' tooltip ='" + options.data.CloseOutTooltip + "' onmouseover='showTooltip(this)' onmouseout='hideTooltip(this)' style='font-size: 22px; color:#351B82'></i>").appendTo(container);
                else
                    $("<i class='far fa-circle' style='font-size: 22px; color:#351B82'></i>").appendTo(container);
            }
        }
        
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
                                                    url: "<%=UGITUtility.GetAbsoluteURL("/api/RMOne/")%>GetNextWorkingDateAndTime?dateString=" + new Date(e.value).toLocaleDateString('en-US'),
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
                                    editorOptions: {
                                        value: dataModel.closeoutStartDate,
                                        format: 'MMM d, yyyy',
                                        readOnly: true,
                                    },
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
                            ],
                            onContentReady: function (data) {
                                data.element.find("label[for$='preconDuration']").text("Precon Duration(Weeks)");
                                data.element.find("label[for$='constDuration']").text("Const Duration(Weeks)");
                                data.element.find("label[for$='closeOutDuration']").text("Closeout Duration(Weeks)");
                            }
                        }],
                    }),
                    $("<div class='ml-2' id='updateAllocationDates'>").dxCheckBox({
                        value: false,
                        text: "Change Allocations:"
                    }),
                    $("#saveButton").dxButton({
                        text: 'Save',
                        icon: '/content/Images/saveFile_icon.png',
                        onClick: function (e) {
                            if (dataModel.hasAnyPastAllocation && $("#updateAllocationDates").dxCheckBox('option', 'value') == true) {
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
        function UpdateRecord(updateAllocations, updatePastAllocations) {
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
            Model.Fields[5].Value = dataModel.onHold == true ? '1' : '0';
            Model.UpdateAllocations = updateAllocations;
            Model.UpdatePastAllocations = updatePastAllocations;
            $.ajax({
                type: "POST",
                url: "<%=UGITUtility.GetAbsoluteURL("/api/RMOne/")%>UpdateRecord",
                data: JSON.stringify(Model),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (message) {
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
                    setTimeout(UserProjectDetailes, 2000);

                },
                error: function (xhr, ajaxOptions, thrownError) {

                    //   alert(ajaxOptions);
                }
            });
        }
    });
</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    #divManagerProjectList .dx-datagrid-rowsview::-webkit-scrollbar {
        width: 0.5em;
        height: 0.5em;
    }

    #divManagerProjectList .dx-datagrid-rowsview::-webkit-scrollbar-thumb {
        background-color: #555;
    }

    .dx-button-has-icon .dx-icon {
        width: 22px;
        height: 22px;
    }

    .redCircle {
        border-color: #ff3535;
        color: #ff3535;
    }

    .blueCircle {
        border-color: #005C9B;
        color: #005C9B;
    }

    .cardallocation {
        display: flex;
        margin: -14px -10px 0px 0px;
        font-size: 15px;
        border: 2px solid;
        font-weight: 600;
        padding-left: 7px;
        justify-content: space-around;
        align-items: center;
        float: right;
        background: white;
        padding: 0px 0px;
        border-radius: 26px;
        height: 50px;
        width: 50px;
    }

    .compare-resume-position {
        display: flex;
        margin: 5px 0px 0px 5px;
        justify-content: space-around;
        align-items: center;
        float: left;
        min-height:54px;
    }

    .gantt-view-position {
        display: flex;
        margin: 5px 0px 0px 35px;
        justify-content: space-around;
        align-items: center;
        float: left;
        min-height:54px;
    }

    #divManagerProjectList .dx-datagrid-rowsview {
        height: 300px !important;
        overflow-y: scroll;
        direction: ltr;
    }

    #divManagerProjectList .dx-datagrid-content {
        direction: ltr;
    }

    #divManagerProjectList .dx-scrollview-content {
        display: flex;
        flex-wrap: wrap;
    }

    #divManagerProjectList.dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-hover {
        background-color: white;
        color: #333;
    }

    #divManagerProjectList .dx-list-item {
        height: 400px;
        width: 600px;
    }

    #divManagerProjectList .dx-datagrid-nodata {
        text-align: center;
        font-weight: 500;
        color: black;
        font-family: 'Roboto', sans-serif !important;
    }

    #divManagerProjectList .px-3 {
        padding-left: 0rem !important;
        padding-right: 0rem !important;
    }

    #divManagerProjectList .glyphicon {
        color: black;
        font-size: 12px;
    }

    #divManagerProjectList .boxAlignClosedOut {
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: flex-start;
    }

    #divManagerProjectList .boxAlignPrecon {
        display: flex;
        flex-direction: column;
        flex-wrap: wrap;
        align-items: center;
        align-content: flex-end;
    }

    #divManagerProjectList .rowStyle {
        text-align: center;
        padding-bottom: 8px;
        padding-top: 5px;
        display: flex;
        justify-content: center;
        flex-wrap: wrap;
    }
    #divManagerProjectList .dx-list .dx-empty-message, #divManagerProjectList .dx-list-item {
    border-top: 0px solid #ddd;
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
        height: 665px;
    }

    .project-title {
         font-size: 20px;
        margin-top: 5px;
        text-align: center;
        font-weight: 600;
        width: 75%;
        color: black;
        white-space:normal;
        /*text-wrap: wrap; property not avaiable in all browsers*/
        margin-left: 13%;
        min-height: 54px;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor:default;
    }

    .project-id {
        font-size: 14px;
        text-align: center;
        font-weight: 500;
        color: black;
        cursor:default;
    }

    .project-phase {
        font-size: 18px;
        text-align: center;
        font-weight: 500;
        padding-bottom: 10px;
        margin-top: 10px;
    }

    .project-phase-inner {
        border: 1px solid;
        padding: 7px 20px;
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
        color: black;
    }

    .whiteSpace-1 {
        font-weight: 500;
        font-size: 12px;
        margin: 6px 0px;
        color: black;
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
        color: black;
    }

    .discription-value {
        text-align: center;
        font-size: 14px;
        padding: 5px 15px 10px 15px;
        color: black;
    }

    .vl {
        border-left: 2px solid gray;
        margin-left: 6px;
        padding-left: 3px;
        padding-top: 5px;
        padding-bottom: 5px;
    }

    #divManagerProjectList .dashboard-panel-new {
        border: 2px solid #ddd !important;
        padding: 0px !important;
        margin-right: 10px;
        margin-top: 10px;
    }

    #divManagerProjectList .dashboard-panel-main, #divManagerProjectList .dashboard-panel-main-mini, #divManagerProjectList .dashboard-panel-main-notmove:not(.panelDashboard) {
        width: 100%;
        background-color: #FFFFFF;
        border-radius: 0px;
        box-shadow: 10px 12px 20px 9px #ddd;
        /* cursor: pointer; */
    }

    .ticket-stage {
        font-size: 18px;
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

    .tabs-container.dx-tabs-expanded {
        width: 410px;
    }

    .tabs-container .dx-tab.dx-tab-selected {
        background-color: #fff;
        color: #333;
        font-size: 13px;
        padding: -2px;
        border: 3px solid #4fa1d6 !important;
        border-left: 1px solid #4fa1d6;
        border-bottom: 0px solid #4fa1d6 !important;
        border-top-left-radius: 10px;
        border-top-right-radius: 10px;
        display:grid;
    }

    .hline {
        border-top: 3px solid #4fa1d6;
    }

    .tabs-container .dx-tab {
        padding: 4px;
        background-color: #fff;
        color: #333;
        border: 0px solid !important;
    }

    .tabs-container {
        border: 0px;
        font-weight: 600;
        font-family: 'Roboto', sans-serif;
    }

    #divManagerProjectList .dx-list .dx-empty-message, #divManagerProjectList .dx-list-item-content {
        padding: 10px;
        font-size: 16px;
        font-weight: 600;
        justify-content: space-between;
    }

    .rcorners2 {
        border-radius: 7px;
        border: 1px solid #D3D3D3;
        padding: 7px 0px;
        width: 200px;
        color: #fff;
        height: 150px;
    }
    .dashboard-panel-new {
    border: none !important;
    padding: 9px;
}
    .dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-active {
    background-color: #fff;
    color: #fff;
    }
    .dx-datagrid-content .dx-datagrid-table .dx-row > td, .dx-datagrid-content .dx-datagrid-table .dx-row > tr > td {
        vertical-align: middle;
    }
   .teamMember_image{
       width:28px;
       height:28px;
       border-radius: 50%;
       display:inline;
   }
</style>
<div class="mt-3 mb-2">
    <div class="loadpanel"></div>
    <div class="tabs-container mt-1 ml-1"></div>
    <div class="ml-1 hline"></div>
    <div id="divManagerProjectList" class="divManagerProjectList"></div>
</div>
<div id="saveButton" class="btnAddNew mb-3" style="float:right;font-size:14px;margin-right:-5px;">
</div>
<div id="popup"></div>
<div id="toast"></div>
<div id="tooltip" class="tooltip">
</div>
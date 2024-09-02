<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddNewProject.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.PMM.AddNewProject" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .SelectedNPR {
        display: none;
        float: left;
        padding-left: 3px;
        padding-top: 10px;
    }

    .TemplateDropdown {
        display: none;
    }

    #DivSelectedProject {
        padding-top: 10px;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var modulename = null;
    var _startdate = null;
    var similarprojectdiv = null;
    var similarProjectList = null;
    var importeditems = ["Task", "Budget", "Complete"];
    var nprGridDatasource = [];
    var templateGridDatasource = [];
    var templateTaskGridDS = [];

    var now = new Date();
    var startDatePicker = null;
    var selectedStartDate = null;
    var title = "";
    var CreateObj = {
        "Title": "",
        "StartDate": now.toISOString(),
        "Mode": 1,
        "SelectedItem": "",
        "ImportDataOption": ["Complete"],
        "IscreateDocumentPortal": false,
        "IsDefaultFolder": false,
        "LifeCycle": ""
    };


    $(function () {
        nprGridDatasource = new DevExpress.data.DataSource({
            key: "ID",
            paginate: true,
            pageSize: 10,
            totalCountRequired: true,
            load: function (loadOptions) {

                return $.getJSON(ugitConfig.apiBaseUrl + "/api/pmmapi/GetNPR?title=" + title + "&Start=" + loadOptions.skip + "&Paging=" + loadOptions.take);
            },
            totalCount: function (loadOptions) {

                return $.getJSON(ugitConfig.apiBaseUrl + "/api/pmmapi/GetNPR?title=" + title + "&Start=" + loadOptions.skip + "&Paging=0").done(function (data) {
                    return data.length;
                });
            }
        });

        similarProjectList = new DevExpress.data.DataSource({
            key: "ID",
            load: function (loadOptions) {
                return $.getJSON(ugitConfig.apiBaseUrl + "/api/pmmapi/GetSuggestedPMM?title=" + title);
            }
        });

        templateGridDatasource = new DevExpress.data.DataSource({
            key: "ID",
            load: function (loadOptions) {
                return $.getJSON(ugitConfig.apiBaseUrl + "/api/pmmapi/GetTemplates?title=" + title + "&Start=" + loadOptions.skip + "&Paging=" + loadOptions.take);
            },
            totalCount: function (loadOptions) {
                return $.getJSON(ugitConfig.apiBaseUrl + "/api/pmmapi/GetTemplates?title=" + title + "&Start=" + loadOptions.skip + "&Paging=0").done(function (data) {
                    return data.length;
                });
            }
        });

        $("#toast").dxToast({
            message: "Ticket Saved Successfully. ",
            type: "info",
            displayTime: 5000,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        var txtTitle = $("#DivTitle").dxTextBox({
            showClearButton: true,
            placeholder: "Enter Title",
            valueChangeEvent: "keyup",
            onValueChanged: function (data) {
                title = data.value;
                CreateObj.Title = data.value;
                if (CreateObj.Mode != 4) {
                    $.get(ugitConfig.apiBaseUrl + "/api/pmmapi/GetSuggestedPMM?title=" + data.value, function (returndata, status) {
                        $("#lblCountLabel").show();
                        var count = returndata.length;
                        var intcount = parseInt(count, 0);

                        if (intcount > 0) {
                            $("#lblCountLabel").show();
                            $("#lblMPPSimilarCount").hide();
                            $("#lblCountLabel").empty();
                            $("#lblCountLabel").append(
                                $("<a href='javascript:void(0);' onclick='showSimilarProjects()'><p class='pmm-similarProject-link'>Similar Project found based on Title : <span> " + intcount + "</span></p>")
                            );
                        }
                        else {
                            $("#lblCountLabel").empty();
                            $("#lblCountLabel").hide();
                        }
                    });
                }
            }
        }).dxValidator({
            validationRules: [{
                type: "custom",
                message: "Required",
                reevaluate: true,
                validationCallback: function (e) {
                    //var mmpstate = chkmpp.option('value');
                    //var nprstate = chknpr.option('value');
                    //var templatestate = chktemplate.option('value');
                    //if (!mmpstate && !nprstate && !templatestate) {
                    //    return !!e.value;
                    //}
                    if (txtTitle.option('value').trim() == "") {
                        return !!e.value;
                    }
                    return true;
                }
            }]
        }).dxTextBox("instance");

        var ddLifeCycle = $("#DivLifeCycle").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Name",
            searchEnabled: true,
            showClearButton: true,
            value: '<%=lifecyleid%>',
            dataSource: "/api/rmmapi/GetPMMLifeCycles",
            onSelectionChanged: function (selectedItem) {
                if (selectedItem.selectedItem != null && typeof (selectedItem.selectedItem) != "undefined") {
                    CreateObj.LifeCycle = selectedItem.selectedItem.ID;
                }
            }
        }).dxValidator({
            validationRules: [{
                type: "required",
                message: "Life Cycle is required"
            }]
        }).dxSelectBox("instance");


        startDatePicker = $("#DivStartDate").dxDateBox({
            type: "date",
            pickerType: "calendar",
            value: _startdate != null ? _startdate : now,
            displayFormat: "MMM d, yyyy",
            onValueChanged: function (data) {
                const dateStr = data.value;
                const date = new Date(dateStr);
                const iso = date.toISOString();
                if (typeof data != "undefined") {
                    if (modulename == "NPR") {
                        CreateObj.StartDate = iso;
                        selectedStartDate = iso
                    }
                    else {
                        CreateObj.StartDate = data.value.toISOString();
                        selectedStartDate = data.value.toISOString()
                    }
                }
            }
        }).dxDateBox('instance');

        var ddlTemplateDropdown = $("#DivTemplateDropdown").dxSelectBox({
            valueExpr: "ID",
            displayExpr: "Title",
            searchEnabled: true,
            showClearButton: true,
            dataSource: templateGridDatasource,
            onSelectionChanged: function (selectedItem) {
                if (selectedItem !== null && typeof (selectedItem.selectedItem) !== "undefined") {
                    if (selectedItem.selectedItem === null)
                        CreateObj.NPRTemplateID = null;
                    else
                        CreateObj.NPRTemplateID = selectedItem.selectedItem.ID;
                }
            }
        }).dxSelectBox("instance");

        var chkmpp = $("#chkMPP").dxCheckBox({
            value: false,
            text: "MPP",
            onValueChanged: function (e) {
                var previousValue = e.previousValue;
                var newValue = e.value;
                CreateObj.Mode = 3;
                if (newValue == true) {
                    chknpr.option('value', false);
                    chktemplate.option('value', false);
                    $("#DivFileUploader").show();
                    $("#DivTemplateList").hide();
                    $("#DivNprProjects").hide();
                    $("#DivImportedItems").hide();
                    $("#lblMPPSimilarCount").hide();
                    $("#DivSelectedNPR").hide();
                    $("#DivSelectedProject").hide();
                    $("#DivSelectedProject").hide();

                    var mppfileuploader = $("#DivFileUploader").dxFileUploader({
                        selectButtonText: "Upload Project File",
                        labelText: "",
                        uploadMethod: "POST",
                        chunkSize: 200000,
                        uploadMode: "instantly",
                        uploadUrl: ugitConfig.apiBaseUrl + "api/pmmapi/UploadFile",
                        allowedFileExtensions: [".mpp", ".xml"],
                        onUploaded: function (e) {
                            if (typeof e.request != "undefined") {
                                if (typeof e.file.name != "undefined") {
                                    var filename = e.file.name.split(".")[0];
                                    $.get(ugitConfig.apiBaseUrl + "/api/pmmapi/GetSuggestedPMM?title=" + filename, function (returndata, status) {
                                        //title = filename;
                                        $("#lblMPPSimilarCount").show();
                                        $("#lblCountLabel").hide();
                                        var count = returndata.length;
                                        var intcount = parseInt(count, 0);
                                        if (intcount > 0) {
                                            $("#lblMPPSimilarCount").empty();
                                            $("#lblMPPSimilarCount").append(
                                                $("<a onclick='showSimilarProjects()'><p class='pmm-similarProject-link'>Similar Project found based on Title : <span> " + intcount + "</span></p>")
                                            );
                                        }
                                        else {
                                            $("#lblMPPSimilarCount").hide();
                                            $("#lblCountLabel").show();
                                        }
                                    });
                                    SetSelectedItemAndMode(e.request.response, "MPP", filename);
                                }
                            }
                        },
                        onValueChanged: function (e) {

                        }
                    }).dxValidator({
                        validationRules: [{
                            type: "custom",
                            message: "Please upload MPP file !!",
                            reevaluate: true,
                            validationCallback: function (e) {
                                //debugger;
                                if (mppfileuploader.option('value').length == 0) {
                                    return false;
                                }
                                return true;
                            }
                        }]
                    }).dxFileUploader("instance");
                }
                else {
                    $("#DivFileUploader").hide();
                    ClearSelectedItemAndMode("", "1", title);
                }
            }
        }).dxCheckBox("instance");

        var chknpr = $("#chkNPR").dxCheckBox({
            value: false,
            text: "NPR",
            onValueChanged: function (e) {
                var previousValue = e.previousValue;
                var newValue = e.value;
                if (newValue == true) {
                    chkmpp.option('value', false);
                    chktemplate.option('value', false);
                    $("#DivNprProjects").show();
                    $("#DivFileUploader").hide();
                    $("#DivTemplateList").hide();
                    $("#DivSelectedProject").hide();
                    $("#lblMPPSimilarCount").hide();
                    if (CreateObj.SelectedItem && CreateObj.Mode == 4)
                        $("#DivImportedItems").show();
                    else
                        $("#DivImportedItems").hide();
                    $("#nprgrid").dxDataGrid({
                        dataSource: nprGridDatasource,
                        showBorders: true,
                        paging: {
                            pageSize: 10
                        },
                        pager: {
                            showPageSizeSelector: true,
                            allowedPageSizes: [10, 20],
                            showInfo: true
                        },
                        selection: {
                            mode: "single"
                        },
                        //paging: {
                        //    visible: true,
                        //    pageSize: 10
                        //},
                        //pager: {
                        //   // showPageSizeSelector: true,
                        //    allowedPageSizes: [10, 20],
                        //    showInfo: true,
                        //    visible: true
                        //},
                        //remoteOperations: {
                        //    paging: true
                        //},
                        //scrolling: {
                        //    mode: "virtual",
                        //    rowRenderingMode: "virtual"
                        //},
                        columns: [
                            {
                                dataField: "TicketId",
                                caption: "Project ID"
                            },
                            {
                                dataField: "Title",
                                caption: "Title"
                            },
                            {
                                caption: "",
                                cellTemplate: function (container, options) {
                                    if (typeof options.data != "undefined")
                                        $("<a class='preview-link'>").attr("onclick", "previewLink('" + options.data.TicketId + "', '" + options.data.Title + "')").text("Preview").appendTo(container);
                                }
                            }
                        ],
                        onRowPrepared: function (e) {
                            var dataGrid = e.component;
                            var keys = dataGrid.getSelectedRowKeys();
                            dataGrid.deselectRows(keys);
                        },
                        onSelectionChanged: function (selectedItems) {
                            var data = selectedItems.selectedRowsData[0];
                            if (typeof data != "undefined") {
                                $("#DivSelectedNPR").show();
                                $("#DivSelectedNPR").children().show();
                                $("#DivSelectednprid").text(data.TicketId + "(" + data.Title + ")");
                                $("#DivImportedItems").show();
                                $("#imgNPRRemove").show();
                                $("#lblMPPSimilarCount").hide();
                                SetSelectedItemAndMode(data.TicketId, "NPR", data.Title);
                            }
                        }
                    });
                }
                else {
                    RemoveSelectedNPRID();
                    $("#DivNprProjects").hide();
                    ClearSelectedItemAndMode("", "1", title);
                }
            }
        }).dxCheckBox("instance");

        var chktemplate = $("#chkTemplate").dxCheckBox({
            value: false,
            text: "Template",
            onValueChanged: function (e) {
                var previousValue = e.previousValue;
                var newValue = e.value;
                if (newValue == true) {
                    $("#DivTemplateList").show();
                    $("#DivNprProjects").hide();
                    $("#DivImportedItems").hide();
                    $("#DivFileUploader").hide();
                    chknpr.option('value', false);
                    chkmpp.option('value', false);
                    $("#DivSelectedNPR").hide();
                    $("#DivSelectedProject").hide();
                    $("#lblMPPSimilarCount").hide();

                    $("#templategrid").dxDataGrid({
                        dataSource: templateGridDatasource,
                        showBorders: true,

                        //filterRow: { visible: true },
                        searchPanel: { visible: true },
                        paging: {
                            pageSize: 10
                        },
                        pager: {
                            showPageSizeSelector: true,
                            allowedPageSizes: [10, 20],
                            showInfo: true
                        },
                        selection: {
                            mode: "single"
                        },
                        sorting: {
                            mode: "multiple" // or "multiple" | "none"
                        },
                        //headerFilter: {
                        //    visible: true
                        //},

                        columns: [{
                            // ...
                            //allowFiltering: false
                            //allowSorting: true
                            dataField: "Title", sortIndex: 0, sortOrder: "desc"
                        }],
                        //paging: {
                        //    visible: true,
                        //    pageSize: 10
                        //},
                        //remoteOperations: {
                        //    paging: true
                        //},
                        //pager: {
                        //    //showPageSizeSelector: true,
                        //    allowedPageSizes: [10, 20],
                        //    showInfo: true,
                        //    visible: true
                        //},
                        onSelectionChanged: function (selectedItems) {
                            if (typeof selectedItems.selectedRowsData != "undefined")
                                SetSelectedItemAndMode(selectedItems.selectedRowsData[0].ID, "Template", selectedItems.selectedRowsData[0].Title);
                        },
                        columns: [
                            {
                                dataField: "ID",
                                caption: "ID",
                                visible: false
                            },

                            {
                                dataField: "Title",
                                caption: "Title"
                            },
                            {
                                caption: "",
                                cellTemplate: function (container, options) {
                                    if (typeof options.data != "undefined")
                                        $("<a class='preview-link'>").attr("onclick", "previewTemplateLink('" + options.data.ID + "')").text("Preview").appendTo(container);
                                }
                            }
                        ]
                    });
                }
                else {
                    $("#DivTemplateList").hide();
                    ClearSelectedItemAndMode("", "1", title);
                }
            }
        }).dxCheckBox("instance");

        var chkCreateDocumentPortal = $("#chkCreateDocumentPortal").dxCheckBox({
            value: false,
            text: "Create Document Portal",
            onValueChanged: function (e) {
                var IsCreatedocumentPortal = e.value;

                if (IsCreatedocumentPortal) {
                    var isDefaultFolder = $("#chkDefaultFolder").dxCheckBox({
                        visible: true,
                        value: true,
                        text: "Default Folder",

                        onValueChanged: function (e) {
                            var isDefaultFolder = e.value;
                            if (isDefaultFolder) {
                                value: true;
                                CreateObj.IsDefaultFolder = true;
                            }
                            else {
                                CreateObj.IsDefaultFolder = false;
                            }

                        }

                    }).dxCheckBox("instance");

                    if (isDefaultFolder.option('value')) {
                        CreateObj.IsDefaultFolder = true;
                    }

                    CreateObj.IscreateDocumentPortal = true;
                }
                else {
                    $("#chkDefaultFolder").dxCheckBox({
                        visible: false,
                    });

                }

            }
        }).dxCheckBox("instance");

        var chkTask = $("#chkTask").dxCheckBox({
            value: false,
            text: "Tasks",
            onValueChanged: function (e) {
                var newValue = e.value;
                if (newValue == true) {
                    chkComplete.option('value', false);
                    pushpopimportitems("Task", true)
                }
                else {
                    pushpopimportitems("Task", false)
                }
            }
        }).dxCheckBox("instance");

        var chkBudget = $("#chkBudget").dxCheckBox({
            value: false,
            text: "Budgets",
            onValueChanged: function (e) {
                var newValue = e.value;
                if (newValue == true) {
                    chkComplete.option('value', false);
                    pushpopimportitems("Budget", true);
                }
                else { pushpopimportitems("Budget", false); }
            }
        }).dxCheckBox("instance");

        var chkComplete = $("#chkComplete").dxCheckBox({
            value: true,
            text: "Complete",
            onValueChanged: function (e) {
                var newValue = e.value;
                if (newValue == true) {
                    pushpopimportitems("Complete", true);
                }
                else { pushpopimportitems("Complete", false); }
            }
        }).dxCheckBox("instance");

        similarprojectdiv = $("#divSimilarProject").dxPopup({
            title: "Similar Projects",
            visible: false,
            //onShowing: function (e) {
            //    var list = $("#DivProjectList").dxList("instance");
            //    list.unselectAll();
            //},
            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("#DivProjectList").dxList({
                        dataSource: similarProjectList,
                        height: 400,
                        //allowItemDeleting: false,
                        //itemDeleteMode: "toggle",
                        showSelectionControls: true,
                        selectionMode: "single",
                        itemTemplate: function (data, index) {
                            var result = $("<div>");
                            $("<div class='importWizard-ticketId'>").text(data.TicketId + ": " + data.Title).appendTo(result);
                            $("<a class='importWizard-previewlink'>").attr("onclick", "previewLink('" + data.TicketId + "', '" + data.Title + "')").text("Preview").appendTo(result);
                            return result;
                        },
                        onContentReady: function (e) {
                            $('.dx-item-content').on('dxclick', function (e) {
                                e.stopPropagation()
                            });
                        },

                        onSelectionChanged: function (data) {
                            if (typeof data != "undefined") {
                                var dataitem = data.addedItems[0];
                                if (typeof dataitem != "undefined") {
                                    $("#DivSelectedProject").show();
                                    $("#DivSelectedprojectid").text(dataitem.Title + "(" + dataitem.TicketId + ")");
                                    similarprojectdiv.hide();
                                    $("imgProjectRemove").show();
                                    $("#DivSelectedNPR").hide();
                                    $("#DivFileUploader").hide();
                                    $("#DivTemplateList").hide();
                                    $("#DivNprProjects").hide();
                                    $("#DivImportedItems").show();

                                    chkmpp.option('value', false);
                                    chknpr.option('value', false);
                                    chktemplate.option('value', false);
                                    SetSelectedItemAndMode(dataitem.TicketId, "PMM", dataitem.Title);
                                }
                            }

                        }

                    })
                )
            }
        }).dxPopup("instance");


        var btncreate = $("#btnCreate").dxButton({
            stylingMode: "contained",
            text: "Create",
            type: "default",
            onClick: function (params) {
                //debugger;
                var validationobj = params.validationGroup.validate();
                if (validationobj.isValid == true) {
                    loadPanel.show();

                    $.post("/api/pmmapi/CreateProject", CreateObj).then(function (response) {
                        window.parent.DevExpress.ui.notify("New Poject Created: " + response, "success", 2000);

                        loadPanel.hide();
                        if (modulename == "NPR") {
                            window.parent.parent.UgitOpenPopupDialog("<%=ProjectSummaryLink%>", "ModuleName=PMM&TicketId=" + response, response + ": " + title, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
                        CloseWindowCallback(0, document.location.href);
                    } else {
                        window.parent.parent.UgitOpenPopupDialog("<%=ProjectSummaryLink%>", "ModuleName=PMM&TicketId=" + response, response + ": " + title, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
                        window.parent.parent.CloseWindowCallback(0, document.location.href);
                    }
                }, function (error) {
                    /*console.log(error.message);*/
                });
            }
        }
    });

        var btncancel = $("#btnCancel").dxButton({
            stylingMode: "contained",
            text: "Cancel",
            type: "default",
            onClick: function () {
                window.parent.CloseWindowCallback(0, document.location.href);
            }
        });

        var loadPanel = $("#loadpanel").dxLoadPanel({
            shadingColor: "rgba(0,0,0,0.4)",
            visible: false,
            showIndicator: true,
            showPane: true,
            shading: true,
            hideOnOutsideClick: false,
            onShown: function () {
                //setTimeout(function () {
                //    loadPanel.hide();
                //}, 3000);
            },
            onHidden: function () {

            }
        }).dxLoadPanel("instance");

        //method call when user creates pmm from npr window using send to pmm button, 
        //npr mode will be default selected
        SetNPRValuesFromURL();
    });

    function showSimilarProjects() {
        similarprojectdiv.show();
        similarProjectList.reload();
    }

    function RemoveSelectedNPRID() {
        $("#DivSelectednprid").empty();
        $("#DivSelectedNPR").hide();
        $("#DivSelectedNPR").children().hide();
        $("#DivImportedItems").hide();
        ClearSelectedItemAndMode("", "", title);
        var nprGridInstance = $("#nprgrid").dxDataGrid("instance");
        var selectedKeys = nprGridInstance.getSelectedRowKeys();
        nprGridInstance.deselectRows(selectedKeys);
    }

    function RemoveSelectedProjectID() {
        $("#DivSelectedprojectid").empty();
        $("#DivSelectedProject").hide();
        $("#DivImportedItems").hide();
        ClearSelectedItemAndMode("", "", title);
    }

    function previewLink(ticketid, title) {
        if (typeof ticketid != "undefined") {
            var modulename = ticketid.split('-')[0];
            if (modulename == "PMM") {
                window.parent.UgitOpenPopupDialog("<%=PMMURL%>", 'TicketId=' + ticketid, ticketid + ": " + title, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            }
            if (modulename == "NPR") {
                window.parent.UgitOpenPopupDialog("<%=NPRURL%>", 'TicketId=' + ticketid, ticketid + ": " + title, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            }
        }
    }

    function previewTemplateLink(ID) {
        var divTemplateTasks = $("#DivTemplateTasks").dxPopup({
            title: "Template Tasks",
            visible: false,
            scrolling: {
                mode: "standard" // or "virtual"
            },

            contentTemplate: function (contentElement) {
                contentElement.append(
                    $("<Div class='template-preview-grid' />").dxTreeList({
                        dataSource: new DevExpress.data.DataSource({
                            key: "ID",
                            load: function (loadOptions) {
                                return $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/pmmapi/GetTemplateTasks?title=" + ID + "&Start=" + loadOptions.skip + "&Paging=" + loadOptions.take
                                });
                            }
                        }),
                        keyExpr: "ID",
                        columnAutoWidth: true,
                        parentIdExpr: "ParentTaskID",
                        hasItemsExpr: "Has_Items",
                        expandedRowKeys: [1, 2],
                        headerFilter: {
                            visible: true
                        },
                        scrolling: {
                            mode: "standard" // or "virtual"
                        },
                        height: 500,
                        //paging: {
                        //    pageSize:2
                        //},
                        //remoteOperations: {
                        //    paging: true
                        //},
                        //pager: {
                        //    showPageSizeSelector: true,
                        //    //allowedPageSizes: [2, 4, 6, 8, 10],
                        //    showInfo: true
                        //},
                        columns: [

                            {
                                dataField: "Title",
                                caption: "Title",
                                cssClass: 'template-preview-grid-col',

                            },
                            {
                                dataField: "StartDate",
                                caption: "Start Date",
                                dataType: "date",
                                format: 'MMM d, yyyy',
                                cssClass: 'template-preview-grid-col-date',

                            },
                            {
                                dataField: "DueDate",
                                caption: "Due Date",
                                dataType: "date",
                                cssClass: 'template-preview-grid-col-date',
                                format: 'MMM d, yyyy',

                            }
                        ]
                    })
                )
            }
        }).dxPopup("instance");
        divTemplateTasks.show();
    }

    function pushpopimportitems(value, flag) {
        if (flag)
            CreateObj.ImportDataOption.push(value);
        else {
            var n = CreateObj.ImportDataOption.indexOf(value);
            if (n != -1) { CreateObj.ImportDataOption.splice(n, 1); }
        }
    }

    function SetSelectedItemAndMode(item, mode, selectedTitle) {
        //alert(1);
        var modetype = 1;
        switch (mode) {
            case "0":
                modetype = 1;
                break;
            case "":
                modetype = 1;
                break;
            case "PMM":
                modetype = 2;
                break;
            case "MPP":
                modetype = 3;
                break;
            case "NPR":
                modetype = 4;
                break;
            case "Template":
                modetype = 5;
                break;
            default:
            // code block
        }
        CreateObj.SelectedItem = item;
        CreateObj.Mode = modetype;
        CreateObj.Title = title;
        if (modetype == 4) {
            CreateObj.StartDate = _startdate;
        }
        if (selectedStartDate != null && selectedStartDate != undefined && selectedStartDate != now.toISOString())
            CreateObj.StartDate = selectedStartDate;
        else
            CreateObj.StartDate = now.toISOString();
    }

    function ClearSelectedItemAndMode(item, mode, selectedTitle) {
        CreateObj.SelectedItem = item;
        CreateObj.Mode = mode;
        CreateObj.Title = title;
        CreateObj.StartDate = now.toISOString();
    }

    function SetNPRValuesFromURL() {
        var url = new URL(window.location.href);
        modulename = url.searchParams.get("module");
        var ticketId = url.searchParams.get("ticketid");
        var title = url.searchParams.get("title");
        _startdate = url.searchParams.get("startdate");

        if (modulename == "NPR") {
            $("#DivNprProjects").show();
            $(".TemplateDropdown").show();
            $("#DivSelectedNPR").show();
            $("#DivSelectedNPR").children().show();
            $("#DivSelectednprid").text(ticketId + "(" + title + ")");
            $("#DivImportedItems").show();
            $("#imgNPRRemove").show();
            $("#lblMPPSimilarCount").hide();
            SetSelectedItemAndMode(ticketId, modulename, title);
            var _startDatePicker = $("#DivStartDate").dxDateBox("instance");
            _startDatePicker.option("value", _startdate);
            var ckmpp = $("#chkMPP").dxCheckBox("instance");
            ckmpp.option("disabled", true);
            var cktemplate = $("#chkTemplate").dxCheckBox("instance");
            cktemplate.option("disabled", true);
            var cknpr = $("#chkNPR").dxCheckBox("instance");
            cknpr.option("value", true);
            cknpr.option("disabled", true);
            var txtitle = $("#DivTitle").dxTextBox("instance");
            txtitle.option("value", title);
            $("#lblCountLabel").hide();
            $("#DivNprProjects").hide();

            var cktask = $("#chkTask").dxCheckBox("instance");
            cktask.option("disabled", true);
            var ckbudget = $("#chkBudget").dxCheckBox("instance");
            ckbudget.option("disabled", true);
            var ckcomplete = $("#chkComplete").dxCheckBox("instance");
            ckcomplete.option("disabled", true);

        }

    }
</script>
<div>

    <div id="DivAddNewProject" class="col-md-12 col-sm-12 col-xs-12">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div id="DivTitle" class="pmmAdd-titleWrap"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 BasicDivStyle">
                <div id="lblCountLabel" style="display: none"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <div class="devExtLabel">Life Cycle:</div>
                <div id="DivLifeCycle" class="NewProjectScreenRow"></div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <div class="devExtLabel">Start Date: </div>
                <div id="DivStartDate" class="NewProjectScreenRow"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 devExtChkBoxWrap">
                <div class="devExtLabel">Import Data From: </div>
                <div id="DivNewProjectOptions">
                    <div id="chkMPP" class="noPadding pmmAdd-chkBox"></div>
                    <div id="chkNPR" class="noPadding pmmAdd-chkBox"></div>
                    <div id="chkTemplate" class="noPadding pmmAdd-chkBox"></div>

                </div>
            </div>
        </div>
        <div class="row TemplateDropdown">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <div class="devExtLabel">Templates: </div>
                <div id="DivTemplateDropdown" class="NewProjectScreenRow"></div>
            </div>
        </div>

        <div class="row">
            <div id="DivFileUploader" style="display: none" class="col-md-12 col-sm-12 col-xs-12 devExt-fileUploadCtrl"></div>
        </div>
        <div class="row">
            <div id="DivNprProjects" class="col-md-12 col-sm-12 col-xs-12  nprProject-gridWrap">
                <div id="nprgrid" class="nprProject-grid"></div>
            </div>
        </div>
        <div class="row">
            <div id="DivTemplateList" class="col-md-12 col-sm-12 col-xs-12  nprTemplate-gridWrap">
                <div id="templategrid" class="nprTemplate-grid"></div>
            </div>
        </div>


        <div class="BasicDivStyle">
            <div style="float: left; padding-top: 10px;">
                <span style="font-size: 14px;"></span>
            </div>

        </div>

        <div id="DivSelectedProject" class="row BasicDivStyle" style="display: none;">
            <div class="col-md-12 col-sm-12 col-xs-12 devExtLabel">Selected Project:</div>
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div id="DivSelectedprojectid" class="selected-projectID" style="float: left;"></div>
                <div id="imgProjectRemove" class="" style="float: left;">
                    <img class="project-remove-img" onclick="RemoveSelectedProjectID()" src="/Content/Images/close-red.png" />
                </div>
            </div>
        </div>


        <div id="DivSelectedNPR" class="row SelectedNPR">
            <div class="col-md-12 col-sm-12 col-xs-12 devExtLabel">Selected Project:</div>
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div id="DivSelectednprid" style="float: left;"></div>
                <div id="imgNPRRemove" style="float: left;">
                    <img onclick="RemoveSelectedNPRID()" src="/Content/Images/close-red.png" width="16" />
                </div>
            </div>
        </div>

        <div id="DivImportedItems" style="display: none;" class="BasicDivStyle">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12 devExtLabel">Import Options:</div>
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div id="chkTask" class="pmmAdd-chkBox" style="float: left; padding: 5px; padding-left: 0px;">
                    </div>
                    <div id="chkBudget" class="pmmAdd-chkBox" style="float: left; padding: 5px;">
                    </div>
                    <div id="chkComplete" class="pmmAdd-chkBox" style="float: left; padding: 5px;">
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div id="chkCreateDocumentPortal" style="width: 100%; padding-left: 3px;" class="docPortal-chkBox pmmAdd-chkBox"></div>
                <div id="chkDefaultFolder" style="width: 100%; clear: both;" class="defaultFolder-chkBox pmmAdd-chkBox"></div>
            </div>
        </div>
        <div class="row">
            <div>
                <div id="DivButtons" class="col-md-12 col-sm-12 col-xs-12 btn-group importBtnWrap" style="text-align: right">
                    <div id="btnCreate" class="primary-dvExtBtn"></div>
                    <div id="btnCancel" class="secondary-devExtBtn"></div>
                </div>
            </div>
        </div>
        <div class="BasicDivStyle">
            <div id="lblMPPSimilarCount" style="clear: both; display: none">
            </div>
        </div>
    </div>
</div>
<div id="DivProjectList" class="projectList-radioBtnList">
</div>
<div id="divSimilarProject" class="similarProject-popup">
</div>
<div id="DivTemplateTasks" class="templateTask-grid">
</div>
<div id="DivTemplateTaskGrid">
</div>
<div id="toast"></div>
<div id="loadpanel"></div>

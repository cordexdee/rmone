<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/Main.Master" CodeBehind="NewLoginWizard.aspx.cs" Inherits="uGovernIT.Web.NewLoginWizard" %>
<%@ MasterType VirtualPath="~/master/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
<script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.8/FileSaver.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/3.3.1/exceljs.js"></script>
    <style>
        .dx-datagrid .dx-row > td[id="dx-col-14"] {
            text-align: left !important;
        }
        .dx-datagrid .dx-row > td[aria-describedby="dx-col-14"] {
            text-align: center !important;
        }
        .activeText {
            color: #2AB7C9;
            background-color: #fff;
            padding-left: 6px;
            padding-right: 6px;
            border-radius: 3px;
        }
        .importButton {
            background-color: #4A6EE2 !important;
            color: #FFF;
            border: 0;
            border-radius: 4px;
            font-size: 12px;
            font-family: 'Poppins', sans-serif;
            font-weight: 500;
        }
        .importButton .dx-button-content {
            padding: 5px 8px;
        }

        .dx-datagrid-header-panel .dx-toolbar {
            padding-left: 4px;
            padding-right: 10px;
        }

        .dx-button-has-text .dx-icon {
            margin-right: 5px;
        }

    </style>
    <script>
        var baseUrl = ugitConfig.apiBaseUrl;
        const dataSource = [{
            ID:'1', Name:'Departments'
        }, { ID: '2', Name: 'Roles' }, { ID: '3', Name: 'Job Titles' }, { ID: '4', Name: 'Users' }, { ID: '5', Name: 'Projects' }
        ];
        
        var viewIndex = 0;


        $(document).ready(function () {
            const APIPath = ugitConfig.apiBaseUrl + "/api/NewWizard";

            viewIndex = $.cookie("viewIndex_" + '<%=TenantAccountID%>');
            if (viewIndex == null)
                viewIndex = 0;
            else
                viewIndex = parseInt(viewIndex);

            setInterval(function () { refreshAllData(); }, 60000);
            //All Data Sources and lookup stores

            var DepartmentStore = new DevExpress.data.CustomStore({
                reshapeOnPush: true,
                load: function (loadOptions) {
                    var deferred = $.Deferred();
                    $.get(`${APIPath}/GetDepartments`).done(function (response) {
                        deferred.resolve({ data: response, totalCount: response.length });
                    });
                    return deferred.promise();
                },
                requireTotalCount: true,
                key:"ID"
            });

            var RoleStore = new DevExpress.data.CustomStore({
                reshapeOnPush: true,
                load: function (loadOptions) {
                    var deferred = $.Deferred();
                    $.get(`${APIPath}/GetRoles`).done(function (response) {
                        deferred.resolve({ data: response, totalCount: response.length });
                    });
                    return deferred.promise();
                },
                requireTotalCount: true,
                key:"Id"
            });

            var lookupRoleDataSource = {
                store: new DevExpress.data.CustomStore({
                    key: "Id",
                    loadMode: "raw",
                    cacheRawData: false,
                    load: function () {
                        return $.getJSON(`${APIPath}/GetRoles`);
                        var deferred = $.Deferred();
                        $.get(`${APIPath}/GetRoles`).done(function (response) {
                            deferred.resolve({ data: response, totalCount: response.length });
                        });
                        return deferred.promise();
                    }
                }),
                sort: "Name"
            }

            var lookupDepartmentDataSource = {
                store: new DevExpress.data.CustomStore({
                    key: "ID",
                    loadMode: "raw",
                    cacheRawData: false,
                    load: function () {
                        return $.getJSON(`${APIPath}/GetDepartments`);
                    }
                }),
                sort: "Title"
            }

            var JobTitleStore = new DevExpress.data.CustomStore({
                reshapeOnPush: true,
                load: function (loadOptions) {
                    var deferred = $.Deferred();
                    $.get(`${APIPath}/GetJobTitles`).done(function (response) {
                        deferred.resolve({ data: response, totalCount: response.length });
                    });
                    return deferred.promise();
                },
                requireTotalCount: true,
                key:"ID"
            });

            var lookupJobTitleStore = {
                store: new DevExpress.data.CustomStore({
                    key: "ID",
                    loadMode: "raw",
                    cacheRawData: false,
                    load: function () {
                        return $.getJSON(`${APIPath}/GetJobTitles`);
                    }
                }),
                sort: "Title"
            };

            var UserStore = new DevExpress.data.CustomStore({
                load: function (loadOptions) {
                    var deferred = $.Deferred();
                    $.get(`${APIPath}/GetUserProfiles`).done(function (response) {
                        deferred.resolve({ data: response, totalCount: response.length });
                    });
                    return deferred.promise();
                },
                requireTotalCount: true,
                key:"Id"
            });

            var lookupUserStore = {
                store: new DevExpress.data.CustomStore({
                    key: "Id",
                    loadMode: "raw",
                    cacheRawData: false,
                    load: function () {
                        return $.getJSON(`${APIPath}/GetUserProfiles`);
                    }
                }),
                sort: "Name"
            }

            var lookupManagerUserStore = {
                store: new DevExpress.data.CustomStore({
                    key: "Id",
                    loadMode: "raw",
                    cacheRawData: false,
                    load: function () {
                        return $.getJSON(`${APIPath}/GetManagerUserProfiles`);
                    }
                }),
                sort: "Name"
            }

            var ProjectStore = new DevExpress.data.CustomStore({
                load: function (loadOptions) {
                    var deferred = $.Deferred();
                    $.get(`${APIPath}/GetProjects`).done(function (response) {
                        deferred.resolve({ data: response, totalCount: response.length });
                    });
                    return deferred.promise();
                },
                requireTotalCount: true,
                key: "TicketId"
            });
            
            var multiView = $('#multiview-container').dxMultiView({
                dataSource: dataSource,
                selectedIndex: viewIndex,
                activeStateEnabled: false,
                loop: false,
                animationEnabled: false,
                itemTemplate: $("#boxContainerDiv"),
                onSelectionChanged(e) {
                    
                    $('#workflow')
                        .html("Please enter or import <span class='activeText'> " + e.addedItems[0].Name + "</span> ");
                    let index = e.component.option('selectedIndex');
                    
                    if (index == 0) {
                        $("#divDepartments").show();
                        $(".divDepartmentBox").removeClass("is-done").addClass("is-active");
                        $(".divRoleBox").removeClass("is-done").removeClass("is-active");
                        $(".divJobTitleBox").removeClass("is-done").removeClass("is-active");
                        $(".divUserBox").removeClass("is-done").removeClass("is-active");
                        $(".divProjectBox").removeClass("is-done").removeClass("is-active");
                        $("#divRoles").hide();
                        $("#divJobTitle").hide();
                        $("#divUsers").hide();
                        $("#divAllocations").hide();
                        $("#divProjects").hide();

                    } else if (index == 1) {
                        var dataGrid = $("#divDepartments").dxDataGrid("instance");                    
                        if (dataGrid.getDataSource()._totalCount > 0) {
                            $("#divDepartments").hide();
                            $("#divRoles").show();
                            $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                            $(".divRoleBox").removeClass("is-done").addClass("is-active");
                            $(".divJobTitleBox").removeClass("is-done").removeClass("is-active");
                            $(".divUserBox").removeClass("is-done").removeClass("is-active");
                            $(".divProjectBox").removeClass("is-done").removeClass("is-active");
                            $("#divJobTitle").hide();
                            $("#divUsers").hide();
                            $("#divAllocations").hide();
                            $("#divProjects").hide();
                            $.cookie("viewIndex_" + '<%=TenantAccountID%>', e.component.option('selectedIndex'), { expires: 2 });
                        } else {
                            e.component.option('selectedIndex', index - 1);
                            DevExpress.ui.notify('Please Add Departments!', 'info', 10000);
                            return;
                        }
                    } else if (index == 2) {
                        var dataGrid = $("#divRoles").dxDataGrid("instance");
                        
                        if (dataGrid.getDataSource()._totalCount > 0) {
                            $("#divDepartments").hide();
                            $("#divRoles").hide();
                            $("#divJobTitle").show();
                            $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                            $(".divRoleBox").removeClass("is-active").addClass("is-done");
                            $(".divJobTitleBox").removeClass("is-done").addClass("is-active");
                            $(".divUserBox").removeClass("is-done").removeClass("is-active");
                            $(".divProjectBox").removeClass("is-done").removeClass("is-active");
                            $("#divUsers").hide();
                            $("#divAllocations").hide();
                            $("#divProjects").hide();
                        } else {
                            e.component.option('selectedIndex', index - 1);
                            DevExpress.ui.notify('Please Add Roles!', 'info', 10000);
                            return;
                        }
                    } else if (index == 3) {
                        var dataGrid = $("#divJobTitle").dxDataGrid("instance");
                        if (dataGrid.getDataSource()._totalCount > 0) {
                            $("#divDepartments").hide();
                            $("#divRoles").hide();
                            $("#divJobTitle").hide();
                            $("#divUsers").show();
                            $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                            $(".divRoleBox").removeClass("is-active").addClass("is-done");
                            $(".divJobTitleBox").removeClass("is-active").addClass("is-done");
                            $(".divUserBox").removeClass("is-done").addClass("is-active");
                            $(".divProjectBox").removeClass("is-done").removeClass("is-active");
                            $("#divAllocations").hide();
                            $("#divProjects").hide();
                        } else {
                            e.component.option('selectedIndex', index - 1);
                            DevExpress.ui.notify('Please Add Job Titles!', 'info', 10000);
                            return;
                        }
                    } else if (index == 4) {
                        var dataGrid = $("#divUsers").dxDataGrid("instance");
                        if (dataGrid.getDataSource()._totalCount > 0) {
                            $("#divDepartments").hide();
                            $("#divRoles").hide();
                            $("#divJobTitle").hide();
                            $("#divUsers").hide();
                            $("#divAllocations").hide();
                            $("#divProjects").show();
                            $(".divUserBox").removeClass("is-active").addClass("is-done");
                            $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                            $(".divRoleBox").removeClass("is-active").addClass("is-done");
                            $(".divJobTitleBox").removeClass("is-active").addClass("is-done");
                            $(".divUserBox").removeClass("is-active").addClass("is-done");
                            $(".divProjectBox").removeClass("is-done").addClass("is-active");
                        } else {
                            e.component.option('selectedIndex', index - 1);
                            DevExpress.ui.notify('Please Add Users!', 'info', 10000);
                            return;
                        }
                    }
                    
                    $.cookie("viewIndex_" + '<%=TenantAccountID%>', e.component.option('selectedIndex'), { expires: 2 });
                    
                },
                onContentReady: function (e) {
                   // e.component.option('selectedIndex', screenIndex);
                }
            }).dxMultiView('instance');

            //All View or Grids

            const dataGridDepartment = $('#divDepartments').dxDataGrid({
                dataSource: DepartmentStore,
                keyExpr: 'ID',
                showBorders: true,
                paging: {
                    pageSize: 10,
                    enabled: false
                },
                height: 500,
                pager: {
                    visible: false,
                    allowedPageSizes: [5, 10, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
                toolbar: {
                    items: ["addRowButton", "revertButton", "saveButton", {
                        name: "exportButton",
                        location: "before",
                        showText: "always",
                        options: { text: "Template/Export" }
                    }]
                },
                editing: {
                    mode: 'cell',
                    allowAdding: true,
                    allowUpdating: true,
                    allowDeleting: true,
                    useIcons: true,
                    newRowPosition: 'pageBottom',
                },
                sorting: {
                    mode: "multiple"
                },
                onRowInserted(e) {
                    //
                    e.component.navigateToRow(e.key);
                },
                remoteOperations: false,
                repaintChangesOnly: true,
                onSaving(e) {
                    e.cancel = true;
                    if (e.changes.length) {
                        e.promise = sendBatchRequest(`${APIPath}/BatchUpdateDepartments`, e.changes).done(() => {
                            e.component.refresh(true).done(() => {
                                e.component.cancelEditData();

                            });

                        });
                    }

                },
                columns: [
                    {
                        dataField: 'Title',
                        caption: 'Department',
                        //width: 240,
                        allowSorting: true,
                        sortIndex: 0,
                        sortOrder: "asc",
                        validationRules: [{ type: 'required' },
                            {
                                type: 'async',
                                message:'Deparment already exists.',
                                validationCallback(params) {
                                    
                                    var id = (typeof params.data.ID === 'undefined') ? 0 : params.data.ID;
                                    return $.ajax({
                                        url: `${APIPath}/IsDepartmentDuplicate?departmentName=${params.value}&ID=${id}`,
                                        type: 'GET',
                                    });
                                }

                            }
                        ]
                    },
                    {
                        dataField: 'DepartmentDescription',
                        caption: 'Description',
                        //width:400
                    },

                ],
                export: {
                    enabled: true
                },
                onExporting: function (e) {
                    const workbook = new ExcelJS.Workbook();
                    const worksheet = workbook.addWorksheet('Departments');

                    DevExpress.excelExporter.exportDataGrid({
                        worksheet: worksheet,
                        component: e.component
                    }).then(function () {
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Departments.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContentReady: function (e) {
                    var toolbar = e.element.find('.dx-datagrid-header-panel .dx-toolbar').dxToolbar('instance');
                    toolbar.on('optionChanged', function (arg) {
                        addCustomItem(toolbar, 'Department');
                    });
                    addCustomItem(toolbar, 'Department');
                }
            }).dxDataGrid('instance');

            const dataGridRole = $('#divRoles').dxDataGrid({
                dataSource: RoleStore,
                keyExpr: 'ID',
                showBorders: true,
                paging: {
                    pageSize: 10,
                    enabled: false,
                },
                height: 500,
                pager: {
                    visible: false,
                    allowedPageSizes: [5, 10, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
                toolbar: {
                    items: ["addRowButton", "revertButton","saveButton", {
                        name: "exportButton",
                        location: "before",
                        showText: "always",
                        options: {text:"Template/Export"}
                    }]
                },
                editing: {
                    mode: 'cell',
                    allowAdding: true,
                    allowUpdating: true,
                    allowDeleting: true,
                    useIcons: true,
                    newRowPosition: 'pageBottom',
                },
                remoteOperations: false,
                repaintChangesOnly: true,
                onSaving(e) {
                    e.cancel = true;
                    //
                    if (e.changes.length) {
                        e.promise = sendBatchRequest(`${APIPath}/BatchUpdateRoles`, e.changes).done(() => {
                            e.component.refresh(true).done(() => {
                                e.component.cancelEditData();

                            });
                        });
                    }
                },
                columns: [
                    {
                        dataField: 'Name',
                        caption: 'Role Name',
                        //width: 240,
                        validationRules: [{ type: 'required' }]
                    },
                    {
                        dataField: 'BillingRate',
                        caption: 'Billing Rate',
                        width: 150,
                        dataType: 'number',
                        editorOptions: {
                            //max: 100
                        }
                    }
                ],
                export: {
                    enabled: true,
                    texts: {
                        exportAll: "Export all data",
                        exportSelectedRows: "Export selected rows",
                        exportTo: "Export"
                    },
                },
                onExporting: function (e) {
                    const workbook = new ExcelJS.Workbook();
                    const worksheet = workbook.addWorksheet('Roles');

                    DevExpress.excelExporter.exportDataGrid({
                        worksheet: worksheet,
                        component: e.component
                    }).then(function () {
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Roles.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContentReady: function (e) {
                    var toolbar = e.element.find('.dx-datagrid-header-panel .dx-toolbar').dxToolbar('instance');
                    toolbar.on('optionChanged', function (arg) {
                        addCustomItem(toolbar, 'Roles');
                    });
                    addCustomItem(toolbar, 'Roles');
                }
            }).dxDataGrid('instance');

            const dataGridJobTitle = $('#divJobTitle').dxDataGrid({
                dataSource: JobTitleStore,
                keyExpr: 'ID',
                showBorders: true,
                paging: {
                    pageSize: 10,
                    enabled: false,
                },
                height: 500,
                pager: {
                    visible: false,
                    allowedPageSizes: [5, 10, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
                editing: {
                    mode: 'cell',
                    allowAdding: true,
                    allowUpdating: true,
                    allowDeleting: true,
                    useIcons: true,
                    newRowPosition: 'pageBottom',
                },
                toolbar: {
                    items: ["addRowButton", "revertButton", "saveButton", {
                        name: "exportButton",
                        location: "before",
                        showText: "always",
                        options: { text: "Template/Export" }
                    }]
                },
                remoteOperations: false,
                repaintChangesOnly: true,
                onSaving(e) {
                    e.cancel = true;
                    if (e.changes.length) {
                        e.promise = sendBatchRequest(`${APIPath}/BatchUpdateJobTitles`, e.changes).done(() => {
                            e.component.refresh(true).done(() => {
                                e.component.cancelEditData();
                            });
                        });
                    }
                },
                columns: [
                    {
                        dataField: 'Title',
                        caption: 'Job Title',
                        width: 240,
                        validationRules: [{ type: 'required' }]
                    },
                    {
                        dataField: 'DepartmentId',
                        caption: 'Department',
                        width: 400,
                        calculateDisplayValue: "DepartmentName",
                        lookup: {
                            dataSource: lookupDepartmentDataSource,
                            valueExpr: 'ID', // contains the same values as the "statusId" field provides
                            displayExpr: 'Title' // provides display va
                        },
                        editorOptions: {
                            showClearButton: true,
                            useItemTextAsTitle: true
                        },
                        validationRules: [{ type: 'required' }]
                    },
                    {
                        dataField: 'RoleId',
                        caption: 'Role',
                        width: 400,
                        calculateDisplayValue: "RoleName",
                        lookup: {
                            dataSource: lookupRoleDataSource,
                            valueExpr: 'Id', // contains the same values as the "statusId" field provides
                            displayExpr: 'Name' // provides display va
                        },
                        editorOptions: {
                            showClearButton: true,
                            useItemTextAsTitle: true
                        },
                        validationRules: [{ type: 'required' }]
                    }
                ],
                export: {
                    enabled: true
                },
                onExporting: function (e) {
                    const workbook = new ExcelJS.Workbook();
                    const worksheet = workbook.addWorksheet('JobTitles');

                    DevExpress.excelExporter.exportDataGrid({
                        worksheet: worksheet,
                        component: e.component
                    }).then(function () {
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'JobTitles.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContentReady: function (e) {
                    var toolbar = e.element.find('.dx-datagrid-header-panel .dx-toolbar').dxToolbar('instance');
                    toolbar.on('optionChanged', function (arg) {
                        addCustomItem(toolbar, 'JobTitle');
                    });
                    addCustomItem(toolbar, 'JobTitle');
                    
                }
            }).dxDataGrid('instance');

            const dataGridUsers = $("#divUsers").dxDataGrid({
                dataSource: UserStore,
                keyExpr: 'Id',
                showBorders: true,
                paging: {
                    pageSize: 10,
                    enabled: false,
                },
                height: 500,
                pager: {
                    visible: false,
                    allowedPageSizes: [5, 10, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
                toolbar: {
                    items: ["addRowButton", "revertButton", "saveButton", {
                        name: "exportButton",
                        location: "before",
                        showText: "always",
                        options: { text: "Template/Export" }
                    }]
                },
                editing: {
                    mode: 'cell',
                    allowAdding: true,
                    allowUpdating: true,
                    allowDeleting: false,
                    useIcons: true,
                    newRowPosition: 'pageBottom',
                },
                remoteOperations: false,
                repaintChangesOnly: false,
                onSaving(e) {
                    e.cancel = true;
                    if (e.changes.length) {
                        e.promise = sendBatchRequest(`${APIPath}/BatchUpdateUsers`, e.changes).done(() => {
                            e.component.refresh(true).done(() => {
                                e.component.cancelEditData();
                            });
                            //lookupManagerUserStore.reload();
                        });
                    }
                },
                columns: [
                    {
                        dataField: 'Name',
                        caption: 'Name',
                        width: 160,
                        validationRules: [{ type: 'required' }]
                    },
                    {
                        dataField: 'UserName',
                        caption: 'User Name',
                        width: 160,
                        validationRules: [{ type: 'required' }, { type: 'pattern', pattern: /^\S*$/, message: 'Enter User Name without Space' } ]
                        
                    },
                    {
                        dataField: 'Email',
                        caption: 'Email',
                        width: 240,
                        validationRules: [{ type: 'required' }, { type: 'email' }]
                    },
                    {
                        dataField: 'DepartmentLookup',
                        caption: 'Department',
                        width: 160,
                        calculateDisplayValue: "DepartmentName",
                        setCellValue(rowData, value) {
                            rowData.DepartmentLookup = value;
                            rowData.JobTitleLookup = 0;
                        },
                        lookup: {
                            dataSource: lookupDepartmentDataSource,
                            valueExpr: 'ID', // contains the same values as the "statusId" field provides
                            displayExpr: 'Title' // provides display va
                        },
                        editorOptions: {
                            showClearButton: true,
                            useItemTextAsTitle: true
                        },
                    },
                    {
                        dataField: 'ManagerUser',
                        caption: 'Manager',
                        width: 160,
                        lookup: {
                            dataSource: lookupManagerUserStore,
                            valueExpr: "Id",
                            displayExpr: 'Name'
                        }
                    },
                    {
                        dataField: 'PhoneNumber',
                        caption: 'Phone Number',
                        width: 100,
                        validationRules: [
                            {
                                type: 'pattern',
                                message: 'Your phone must have "(555)555-5555" format!',
                                pattern: /^\(\d{3}\)\d{3}-\d{4}$/i,
                            }
                        ]
                    },
                    {
                        dataField: 'JobTitleLookup',
                        caption: 'Job Title',
                        width: 160,
                        calculateDisplayValue: "JobProfile",
                        editorOptions: {
                            showClearButton: true,
                            useItemTextAsTitle: true
                        },
                        lookup: {
                            dataSource(options) {

                                return {
                                    store: new DevExpress.data.CustomStore({
                                        key: "ID",
                                        loadMode: "raw",
                                        cacheRawData: false,
                                        load: function () {
                                            return $.getJSON(`${APIPath}/GetJobTitles`);
                                        }
                                    }),
                                    sort: "Title",
                                    filter: options.data ? ['DepartmentId', '=', options.data.DepartmentLookup] : null,
                                }
                            },
                            valueExpr: 'ID', // contains the same values as the "statusId" field provides
                            displayExpr: 'Title' // provides display va
                        }
                    },
                    {
                        dataField: 'HourlyRate',
                        caption: 'Billing Rate',
                        width: 80
                    }
                ],
                export: {
                    enabled: true
                },
                onEditorPreparing(e) {
                    if (e.parentType === 'dataRow' && (e.dataField === 'JobTitleLookup' || e.dataField == 'DepartmentLookup')) {
                        e.editorOptions.text = '';
                    }
                },
                onExporting: function (e) {
                    const workbook = new ExcelJS.Workbook();
                    const worksheet = workbook.addWorksheet('Users');

                    DevExpress.excelExporter.exportDataGrid({
                        worksheet: worksheet,
                        component: e.component
                    }).then(function () {
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Users.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContentReady: function (e) {
                    var toolbar = e.element.find('.dx-datagrid-header-panel .dx-toolbar').dxToolbar('instance');
                    toolbar.on('optionChanged', function (arg) {
                        addCustomItem(toolbar, 'Users');
                    });
                    addCustomItem(toolbar, 'Users');
                }
            }).dxDataGrid("instance");

            const dataGridProjects = $("#divProjects").dxDataGrid({
                dataSource: ProjectStore,
                keyExpr: 'ID',
                showBorders: true,
                paging: {
                    pageSize: 10,
                    enabled: false,
                },
                height: 500,
                pager: {
                    visible: false,
                    allowedPageSizes: [5, 10, 'all'],
                    showPageSizeSelector: true,
                    showInfo: true,
                    showNavigationButtons: true,
                },
                toolbar: {
                    items: ["addRowButton", "revertButton", "saveButton", {
                        name: "exportButton",
                        location: "before",
                        showText: "always",
                        options: { text: "Template/Export" }
                    }]
                },
                editing: {
                    mode: 'cell',
                    allowAdding: true,
                    allowUpdating: true,
                    allowDeleting: false,
                    useIcons: true,
                    newRowPosition: 'pageBottom',
                },
                remoteOperations: false,
                repaintChangesOnly: true,
                columns: [
                    {
                        dataField: 'ModuleType',
                        caption: 'Type',
                        width: 100,
                        lookup: {
                            dataSource: JSON.parse('<%= moduletypejson%>'),
                            valueExpr: "id",
                            displayExpr: "name"
                        },
                        validationRules: [{ type: 'required' }]
                    },
                    {
                        dataField: 'ProjectName',
                        caption: 'Project',
                        width: 250,
                        //cellTemplate(container, options) {
                        //    $('<div>')
                        //        .append($('<a />', { text: options.value, onclick: "openTicketLink('" + options.data.TicketId + "');" }))
                        //        .appendTo(container);
                        //},
                        validationRules: [{ type: 'required' }]
                    },
                    {
                        dataField: 'ProjectManagerUser',
                        caption: 'Project Manager',
                        width: 140,
                        lookup: {
                            dataSource: lookupManagerUserStore,
                            valueExpr: "Id",
                            displayExpr: 'Name'
                        },
                        validationRules: [{ type: 'required' }],
                    },
                    {
                        dataField: 'StartDate',
                        caption: 'Start Date',
                        width: 120,
                        dataType: "date",
                        validationRules: [{ type: 'required' }],
                        alignment: "center",
                        format: 'MMM d, yyyy',
                        editorOptions: {
                            showClearButton: true,
                            onOpened: function (e) {

                                e.component._popup.option("position", {
                                    my: "center",
                                    at: "center",
                                });

                            }
                        }
                    },
                    {
                        dataField: 'EndDate',
                        caption: 'End Date',
                        width: 120,
                        dataType: "date",
                        validationRules: [{ type: 'required' }],
                        alignment: "center",
                        format: 'MMM d, yyyy',
                        editorOptions: {
                            showClearButton: true,
                            onOpened: function (e) {

                                e.component._popup.option("position", {
                                    my: "center",
                                    at: "center",
                                });

                            }
                        }
                    },
                    {
                        dataField: 'Description',
                        caption: 'Description',
                        //width: 80
                    },
                    {
                        caption: 'Resource',
                        width: 80,
                        cellTemplate(container, options) {
                            var param1 = options.data.TicketId;
                            var param2 = options.data.ModuleType;
                            var param3 = options.data.ProjectName;
                            $('<div>')
                                .append($(`<span><img class="quickedit" src="/Content/images/resourcemanagementBlue.png" title="Allocate Resource" width="20px" onclick="openResourceAllocation(\'${param1}\',\' ${param2}\',\' ${param3}\')"  /></span>`))
                                .appendTo(container);
                        },

                    }
                ],
                onSaving(e) {
                    e.cancel = true;
                    if (e.changes.length) {
                        e.promise = sendBatchRequest(`${APIPath}/BatchUpdateProjects`, e.changes).done(() => {
                            e.component.refresh(true).done(() => {
                                e.component.cancelEditData();
                            });
                        });
                    }
                },
                onEditorPreparing: function (e) {

                    if (e.dataField == "StartDate" || e.dataField == "EndDate") {
                        if (e.editorOptions.value == "" || e.editorOptions.value == "")
                            e.editorOptions.value = new Date();
                    }
                },
                onRowValidating: function (e) {
                    if (typeof e.newData.StartDate !== "undefined" && typeof e.newData.EndDate !== "undefined") {
                        if (new Date(e.newData.EndDate) < new Date(e.newData.StartDate)) {
                            e.isValid = false;
                            e.errorText = "StartDate should be less then EndDate";
                        }
                    }
                    else {
                        if (typeof e.newData.StartDate == "undefined") {
                            if (new Date(e.newData.EndDate) < new Date(e.oldData.StartDate)) {
                                e.isValid = false;
                                e.errorText = "StartDate should be less then EndDate";

                            }
                        }
                        if (typeof e.newData.EndDate == "undefined") {
                            if (new Date(e.oldData.EndDate) < new Date(e.newData.StartDate)) {
                                e.isValid = false;
                                e.errorText = "StartDate should be less then EndDate";
                            }
                        }
                    }
                },
                export: {
                    enabled: true
                },
                onExporting: function (e) {
                    const workbook = new ExcelJS.Workbook();
                    const worksheet = workbook.addWorksheet('Projects');

                    DevExpress.excelExporter.exportDataGrid({
                        worksheet: worksheet,
                        component: e.component
                    }).then(function () {
                        workbook.xlsx.writeBuffer().then(function (buffer) {
                            saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'Projects.xlsx');
                        });
                    });
                    e.cancel = true;
                },
                onContentReady: function (e) {
                    var toolbar = e.element.find('.dx-datagrid-header-panel .dx-toolbar').dxToolbar('instance');
                    toolbar.on('optionChanged', function (arg) {
                        addCustomItem(toolbar, 'Projects');
                    });
                    addCustomItem(toolbar, 'Projects');
                }
            }).dxDataGrid("instance");

            setSelectedView(viewIndex);
        });
    </script>
    <script name="scriptDocument">
        

        //this method use to send batch request call for all datasources
        function sendBatchRequest(url, changes) {
            const d = $.Deferred();

            $.ajax(url, {
                method: 'POST',
                data: JSON.stringify(changes),
                cache: false,
                contentType: 'application/json',
                xhrFields: { withCredentials: true },
            }).done(d.resolve).fail((xhr) => {
                d.reject(xhr.responseJSON ? xhr.responseJSON.Message : xhr.statusText);
            });

            return d.promise();
        }

        function openTicketLink(ticketid) {
        }
        function openResourceAllocation(ticketid, modulename, title) {
            
            let path = "/Layouts/uGovernIT/DelegateControl.aspx?isdlg=1&isudlg=1&control=CRMProjectAllocationNew&isreadonly=true&ticketId=" + ticketid + "&module=" + modulename;
            UgitOpenPopupDialog(path, "moduleName=" + modulename + " & ConfirmBeforeClose=true", title, '90', '90', false, document.URL);
        }

        function setPrevious() {
            
            var multiviewobj = $('#multiview-container').dxMultiView("instance");
            var selectedindex = multiviewobj.option("selectedIndex");
            $('#multiview-container').dxMultiView("option", "selectedIndex", selectedindex - 1);
            //$.cookie("screenIndex", selectedindex - 1, { expires : 2 });
        }
        function setNext() {
            
            var multiviewobj = $('#multiview-container').dxMultiView("instance");
            var selectedindex = multiviewobj.option("selectedIndex");
            $('#multiview-container').dxMultiView("option", "selectedIndex", selectedindex + 1);
            //$.cookie("screenIndex", selectedindex + 1, { expires : 2});
        }

        function addCustomItem(toolbar, importItem) {
           // 
            var items = toolbar.option('items');
            var myItem = DevExpress.data.query(items).filter(function (item) {
                return item.name == 'Import';
            }).toArray();
            if (!myItem.length) {
                items.push({
                    location: 'before',
                    widget: 'dxButton',
                    name: 'Import',
                    options: {
                        text: 'Import',
                        elementAttr: {
                            class: "importButton"
                        },
                        onClick: function (e) {
                            
                            DevExpress.ui.notify('Upload Excel To Import ' + importItem);
                            let control = "importexcelfile";
                            let listname = importItem;
                            let tablename;
                            if (listname == "Department")
                                tablename = "Department";
                            else if (listname == "Roles")
                                tablename = "Roles"
                            else if (listname == "JobTitle")
                                tablename = "JobTitle";
                            else if (listname == "Users")
                                tablename = "UserInformationList";
                            else if (listname == "Projects")
                                tablename = "Projects";

                            let absoluteUrlImport = `/layouts/ugovernit/DelegateControl.aspx?control=${control}&listName=${tablename}`
                            UgitOpenPopupDialog(absoluteUrlImport, "", 'Import ' + listname, '450px', '450px', 0, escape("<%= Request.Url.AbsoluteUri %>"));
                        }
                    }
                });

                items.push({
                    location: 'before',
                    widget: 'dxButton',
                    name: 'Reload',
                    options: {
                        text: 'Load Import Data',
                        icon:'refresh',
                        elementAttr: {
                            class: "refreshButton"
                        },
                        onClick: function (e) {
                            
                            if (importItem == "Department")
                            {
                                var dxdepartment = $("#divDepartments").dxDataGrid("instance");
                                dxdepartment.refresh();
                            }
                            else if (importItem == "Roles")
                            {
                                var dxRole = $("#divRoles").dxDataGrid("instance");
                                dxRole.refresh();
                            }
                            else if (importItem == "JobTitle")
                            {
                                var dxjobtitle = $("#divJobTitle").dxDataGrid("instance");
                                dxjobtitle.refresh();
                            }
                            else if (importItem == "Users")
                            {
                                var dxUser = $("#divUsers").dxDataGrid("instance");
                                dxUser.refresh();
                            }
                            else if (importItem == "Projects")
                            {
                                var dxProject = $("#divProjects").dxDataGrid("instance");
                                dxProject.refresh();
                            }
                        }
                    }
                });
                toolbar.option('items', items);
            }
        }

        function setSelectedView(index) {
            
            $('#multiview-container').dxMultiView("option", "selectedIndex", parseInt(index));

            if (index == 0) {
                $("#divDepartments").show();
                $(".divDepartmentBox").removeClass("is-done").addClass("is-active");
                $("#divRoles").hide();
                $("#divJobTitle").hide();
                $("#divUsers").hide();
                $("#divAllocations").hide();
                $("#divProjects").hide();
                setWorkflowLabel("Departments");
            } else if (index == 1) {
                    $("#divDepartments").hide();
                    $("#divRoles").show();
                    $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                    $(".divRoleBox").removeClass("is-done").addClass("is-active");
                    $("#divJobTitle").hide();
                    $("#divUsers").hide();
                    $("#divAllocations").hide();
                    $("#divProjects").hide();
                setWorkflowLabel("Roles");
            } else if (index == 2) {
                    $("#divDepartments").hide();
                    $("#divRoles").hide();
                    $("#divJobTitle").show();
                    $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                    $(".divRoleBox").removeClass("is-active").addClass("is-done");
                    $(".divJobTitleBox").removeClass("is-done").addClass("is-active");
                    $("#divUsers").hide();
                    $("#divAllocations").hide();
                $("#divProjects").hide();
                setWorkflowLabel("Job Titles");
            } else if (index == 3) {
                    $("#divDepartments").hide();
                    $("#divRoles").hide();
                    $("#divJobTitle").hide();
                    $("#divUsers").show();
                    $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                    $(".divRoleBox").removeClass("is-active").addClass("is-done");
                    $(".divJobTitleBox").removeClass("is-active").addClass("is-done");
                    $(".divUserBox").removeClass("is-done").addClass("is-active");
                    //$(".divUserBox").addClass("is-done");
                    //$("#divAllocations").hide();
                $("#divProjects").hide();
                setWorkflowLabel("Users");
            } else if (index == 4) {
                    $("#divDepartments").hide();
                    $("#divRoles").hide();
                    $("#divJobTitle").hide();
                    $("#divUsers").hide();
                    $("#divAllocations").hide();
                    $("#divProjects").show();
                    $(".divUserBox").removeClass("is-active").addClass("is-done");
                    $(".divDepartmentBox").removeClass("is-active").addClass("is-done");
                    $(".divRoleBox").removeClass("is-active").addClass("is-done");
                    $(".divJobTitleBox").removeClass("is-active").addClass("is-done");
                    $(".divUserBox").removeClass("is-active").addClass("is-done");
                $(".divProjectBox").removeClass("is-done").addClass("is-active");
                setWorkflowLabel("Projects");
            }

        }

        function setWorkflowLabel(text) {
            $('#workflow')
                .html("Please enter or import <span class='activeText'> " + text + "</span> ")
        }

        function showClickedView(index) {
            
            var clickedView = $(event.currentTarget);
            if (clickedView == null || typeof clickedView == "undefined")
                return;
            var previousIndex = parseInt($.cookie("viewIndex_" + '<%=TenantAccountID%>'));
            if (isNaN(previousIndex))
                setSelectedView(1);
            else {
                if (clickedView.hasClass('is-done') || clickedView.hasClass('is-active'))
                    $('#multiview-container').dxMultiView("option", "selectedIndex", parseInt(index));
                else
                    $('#multiview-container').dxMultiView("option", "selectedIndex", parseInt(previousIndex + 1));
            }
        }

        function refreshAllData() {
            
            var sindex = parseInt($.cookie("viewIndex_" + '<%=TenantAccountID%>'));
            if (sindex == 0) {
                var dxdepartment = $("#divDepartments").dxDataGrid("instance");
                dxdepartment.refresh();
            }
            else if (sindex == 1) {
                var dxRole = $("#divRoles").dxDataGrid("instance");
                dxRole.refresh();
            }
            else if (sindex == 2) {
                var dxjobtitle = $("#divJobTitle").dxDataGrid("instance");
                dxjobtitle.refresh();
            }
            else if (sindex == 3) {
                var dxUser = $("#divUsers").dxDataGrid("instance");
                dxUser.refresh();
            }
            else if (sindex == 4) {
                var dxProject = $("#divProjects").dxDataGrid("instance");
                dxProject.refresh();
            }
                
            
        }
    </script>

    <script type="text/html" id="boxContainerDiv">
        <div class="d-flex align-items-center pt-2 pb-3 pr-1">
            <div id="boxContainer" class="breadcrumbs curGrab">
                <a href="#" class="breadcrumbs__item divDepartmentBox" onclick="showClickedView(0)" > Departments </a>
                <a href="#" class="breadcrumbs__item divRoleBox" onclick="showClickedView(1)"> Roles </a>
                <a href="#" class="breadcrumbs__item divJobTitleBox" onclick="showClickedView(2)"> Job Titles </a>
                <a href="#" class="breadcrumbs__item divUserBox" onclick="showClickedView(3)"> Users </a>
                <a href="#" class="breadcrumbs__item divProjectBox" onclick="showClickedView(4)"> Projects </a>
                <%--<a href="#" class="breadcrumbs__item divAllocationBox" > Allocations </a>--%>
            </div>
            <div class="tabsSlideArrow ml-3">
                <div class="arrow-box" title="Prev" onclick="setPrevious()">
                    <i class="fas fa-chevron-left"></i>
                </div>
                <div class="arrow-box" title="Next" onclick="setNext()">
                    <i class="fas fa-chevron-right"></i>
                </div>
            </div>
        </div>        
    </script>

    <div style="padding-left: 2px; padding-right: 2px;">
        <div id="multiview-container" style="width:100%;"></div>
         <div id="workflow" class="alert-attr">
            Please enter or import <span class="activeText">Departments</span> 
        </div>
        <%--<div id="divImport" class="mt-2 fw-bold"></div>--%>
    </div>
    
    <div style="padding-left: 2px; padding-right: 2px;padding-top:10px;">
        
        <div id="divDepartments">
        </div>
        <div style="display:none" id="divRoles">
        </div>
        <div style="display:none" id="divJobTitle">
        </div>
        <div style="display:none" id="divUsers">
        </div>
         <div style="display:none" id="divProjects">
        </div>
        <%--<div style="display:none" id="divAllocations">
            
        </div>--%>
    </div>
    
</asp:Content>
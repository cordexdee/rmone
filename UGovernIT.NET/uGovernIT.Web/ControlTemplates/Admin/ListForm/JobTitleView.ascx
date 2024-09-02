<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JobTitleView.ascx.cs" Inherits="uGovernIT.Web.JobTitleView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<%@ Register assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpreadsheet" tagprefix="dx" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var DS = [];
    var Jobtitleid = "";
    $(() => {
        $('#checked').dxCheckBox({
            value: false,
            text: 'Show Deleted',
            onValueChanged(e) {
                var grd = $('#gridContainer').dxDataGrid("instance");
                if (e.value) {
                    grd.option("dataSource", DS.Table);
                }
                else {
                    grd.option("dataSource", DS.Table.filter(item => item.Deleted == false));
                }
            },
        });
        

    $('#btnAdd').dxButton({
        elementAttr: {
            id: "btnAdd",
            class: "primary-btn-link phrasesAdd-label"
        },
        stylingMode: 'contained',
        text: 'Add New Item',
        type: 'default',
        width: 130,
        onClick() {
            AddJobTitle();
            //DevExpress.ui.notify('The Contained button was clicked');
        },
    });
    $('#btnImport').dxButton({
        elementAttr: {
            id: "btnImport",
            class: "primary-btn-link phrasesAdd-label"
        },
        stylingMode: 'contained',
        text: 'Import',
        type: 'default',
        width: 110,
        onClick() {
            Import();
            //DevExpress.ui.notify('The Contained button was clicked');
        },
    });

    $.get(baseUrl + "api/common/GetJobTitle", function (data) {
        DS = data;
        var dt = data.Table.filter(item => item.Deleted == false);
        $('#gridContainer').dxDataGrid({
            dataSource: dt,
            keyExpr: 'ID',
            showBorders: true,
            paging: false,
            remoteOperations: false,
            cellHintEnabled: true,
            rowAlternationEnabled: true,
            columns: [
                {
                    dataField: 'Title',
                    width: 220,
                    cellTemplate: function (container, options) {
                        //debugger;
                        //<span><a href='javascript:void(0);' title='" + options.data.Title + "' onclick=openTask('" + params + "');>" + options.data.Title + "</a>
                        //openJobTitleDialog(path, params, titleVal, width, height, stopRefresh) 
                        $(`<span><a style ='cursor:pointer'onclick=openJobTitleDialog('<%= editUrl%>','JobTitleId=${options.data.ID}','JobTitle','680px','380px',false)>${options.data.Title}</a></span >`)
                                //$(`<span><a href='javascript: void (0);' onclick='AddJobTitle()'>${ options.data.Title}</a></span >`)
                                //.attr('href', someUrl + "?cucode=" + options.value + "&startDate=" + myStartValue + "&endDate=" + myEndValue)
                                //.attr('onclick', AddJobTitle())
                                //.attr('target', '_blank')
                                .appendTo(container);
                        },
                    },
                    {
                        dataField: 'JobType',
                        minWidth: 60,
                        alignment: 'center'
                },
                {
                    caption: 'Role',
                    dataField: 'RoleName',
                    minWidth: 110,
                    alignment: 'center'
                },
                    {
                        caption: 'LR',
                        dataField: 'LowRevenueCapacity',
                        minWidth: 5,
                        alignment: 'center',
                        hint: 'Low Revenue Capacity'

                    },
                    {
                        caption: 'HR',
                        dataField: "HighRevenueCapacity",
                        minWidth: 5,
                        alignment: 'center',
                        hint: 'High Revenue Capacity'
                    },
                    {
                        caption: 'LPC',
                        dataField: "LowProjectCapacity",
                        minWidth: 5,
                        alignment: 'center',
                        hint: 'Low Project Capacity'
                    },
                    {
                        caption: 'HPC',
                        dataField: "HighProjectCapacity",
                        minWidth: 5,
                        alignment: 'center',
                        hint: 'Low Project Capacity'

                    },
                    {
                        caption: 'RLT',
                        dataField: "ResourceLevelTolerance",
                        minWidth: 5,
                        alignment: 'center',
                        hint: 'Resource Level Tolerance'
                    },

                ],

                onCellPrepared: function (e) {
                    if (e.rowType === "header") {
                        e.cellElement.attr("title", e.column.hint);
                    }
                },
                onRowPrepared: function (e) {
                    if (e.rowType == "data") {
                        if (e.data.Deleted != false) {
                            e.rowElement.addClass('clssdeletedrow');
                        }
                    }
                },

                masterDetail: {
                    enabled: true,
                    template(container, options) {
                        data = DS.Table1.filter(item => item.JobTitleLookup == options.data.ID && item.Deleted == false);
                        var title = 'Departments Using Title: ' + options.data.Title;
                        Jobtitleid = options.data.ID;
                        var url = '<%= departmentJobtitleMappingUrl%>';
                        $('<div>')
                            .addClass('master-detail-caption')
                            .text(`Departments Using Title: ` + options.data.Title)
                            .append($(`<img type='button' style='cursor:pointer;margin-left:5px' class='orgGridBtn' height='18' width='18' src='/content/images/plus-blue.png' onclick="openJobTitleDialog('${url}','JobTitleId=${options.data.ID}','${title}','600px','400px',false)" />`))
                            .append($(`<input type="checkbox" id='checkedchild_${options.data.ID}' onclick="showdeletedchilddata(this,'${options.data.ID}')" ><label for='checkedchild_${options.data.ID}' > Show Deleted</label>`))

                            .appendTo(container);
                        $(`<div class="child1" id='div_${options.data.ID}'>`)
                            .dxDataGrid({
                                columnAutoWidth: true,
                                showBorders: true,
                                paging: false,
                                rowAlternationEnabled: true,
                                remoteOperations: false,
                                columns: [{
                                    caption: 'Department',
                                    dataField: 'DepartmentName',
                                    width: 300,
                                    cellTemplate: function (container, options) {
                                        $(`<span><a style ='cursor:pointer'onclick="openJobTitleDialog('<%= departmentJobtitleMappingUrl%>','JobTitleId=${Jobtitleid}&deptid=${options.data.DeptLookup}&roleid=${options.data.RoleLookup}&ecr=${options.data.EmpCostRate}&deleted=${options.data.Deleted}&mode=${'E'}&Id=${options.data.Id}','Edit ${options.data.DepartmentName}','680px','380px',false)">${options.data.DepartmentName}</a></span >`)
                                            .appendTo(container);
                                    },
                                },

                                    

                                    {
                                        caption: 'Employee Cost Rate ($)',
                                        dataField: 'EmpCostRate',
                                        minWidth: 50,
                                        alignment: 'center'
                                    },
                                    //columns: [
                                    //    'DepartmentName',
                                    //    'EmpCostRate'
                                ],
                                dataSource: new DevExpress.data.DataSource({
                                    store: new DevExpress.data.ArrayStore({
                                        key: 'Id',
                                        data: data,
                                    }),

                                    filter: ['JobTitleLookup', '=', options.key],
                                }),
                                onRowPrepared: function (e) {
                                    if (e.rowType == "data") {
                                        if (e.data.Deleted != false) {
                                            e.rowElement.addClass('clssdeletedrow');
                                        }
                                    }
                                },
                                //masterDetail: {
                                //    enabled: true,
                                //    template(container, options) {
                                //        debugger;
                                //        data = DS.Table2.filter(item => item.DepartmentLookup == options.data.DepartmentLookup);
                                //        const currentEmployeeData = options.data;

                                //        $('<div>')
                                //            .addClass('master-detail-caption')
                                //            .text(`Department v/s Role`)
                                //            .appendTo(container);

                                //        $('<div class="child2">')
                                //            .dxDataGrid({
                                //                columnAutoWidth: true,
                                //                showBorders: true,
                                //                rowAlternationEnabled: true,
                                //                paging: false,
                                //                remoteOperations: false,
                                //                columns: [
                                //                    'RoleName',
                                //                    'BillingRate'
                                //                ],
                                //                dataSource: new DevExpress.data.DataSource({
                                //                    store: new DevExpress.data.ArrayStore({
                                //                        key: 'Id',
                                //                        data: data,
                                //                    }),

                                //                    filter: ['DepartmentLookup', '=', options.data.DepartmentLookup],
                                //                }),
                                //            }).appendTo(container);
                                //    }
                                //}
                            }).appendTo(container);
                    },

                },
            });
        });

    });
    function Import() {
        var param = '&listName=JobTitle';
        window.parent.UgitOpenPopupDialog('<%= ImportJobTitleUrl%>', param, 'Import Job Titles', '550px', '260px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function AddJobTitle() {
        window.UgitOpenPopupDialog('<%= editUrl%>&JobTitleID=0', "", "Add User Job Title", '680px', '380px', 0);
    }
    function openJobTitleDialog(path, params, titleVal, width, height, stopRefresh) {

        window.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }
    function showdeletedchilddata(e, Id) {

        var grd = $('#div_' + Id).dxDataGrid("instance");
        if (e.checked) {
            grd.option("dataSource", DS.Table1.filter(item => item.JobTitleLookup == Jobtitleid));
        }
        else {
            grd.option("dataSource", DS.Table1.filter(item => item.JobTitleLookup == Jobtitleid && item.Deleted == false));

        }
    }
</script>
<%--<script>
    var baseUrl = ugitConfig.apiBaseUrl;
    var DS = [];
    $(() => {

        $.get(baseUrl + "api/RMMAPI/GetJobTitle", function (data) {
            debugger;
            DS = data;
            $('#gridContainer').dxDataGrid({
                showBorders: true,
                paging: false,
                dataSource: data.Table,
                remoteOperations: false,
                //columnMinWidth: 20,
                /* columnAutoWidth: true,*/
                cellHintEnabled: true,
                rowAlternationEnabled: true,
                columns: [{
                    dataField: 'Title',
                    width: 400
                },
                {
                    dataField: 'JobType',
                    minWidth: 50,
                    alignment: 'center'
                },
                {
                    caption: 'LR',
                    dataField: 'LowRevenueCapacity',
                    minWidth: 5,
                    alignment: 'center',
                    hint: 'Low Revenue Capacity'

                },
                {
                    caption: 'HR',
                    dataField: "HighRevenueCapacity",
                    minWidth: 5,
                    alignment: 'center',
                    hint: 'High Revenue Capacity'
                },
                {
                    caption: 'LPC',
                    dataField: "LowProjectCapacity",
                    minWidth: 5,
                    alignment: 'center',
                    hint: 'Low Project Capacity'
                },
                {
                    caption: 'HPC',
                    dataField: "HighProjectCapacity",
                    minWidth: 5,
                    alignment: 'center',
                    hint: 'Low Project Capacity'

                },
                {
                    caption: 'RLT',
                    dataField: "ResourceLevelTolerance",
                    minWidth: 5,
                    alignment: 'center',
                    hint: 'Resource Level Tolerance'
                }
                ],
                //columns: [
                //    'Title',
                //    'JobType',
                //    'LowRevenueCapacity',
                //    'HighRevenueCapacity',
                //    'LowProjectCapacity',
                //    'HighProjectCapacity',
                //    'ResourceLevelTolerance'
                //],
                masterDetail: {
                    enabled: true,
                    template: masterDetailTemplate,
                },
                onCellPrepared: function (e) {
                    if (e.rowType === "header") {
                        //debugger;
                        //console.log(e);
                        e.cellElement.attr("title", e.column.hint);
                    }
                }
            });

        });
    });

    function masterDetailTemplate(_, masterDetailOptions) {
        debugger;
        return $('<div>').dxTabPanel({
            items: [{
                title: 'Department',
                template: createOrdersTabTemplate(masterDetailOptions.data),
            }
                //, {
                //title: 'Address',
                //template: createAddressTabTemplate(masterDetailOptions.data),
                //}
            ],
        });
    }

    function createOrdersTabTemplate(masterDetailData) {
        debugger;
        return function () {
            let orderHistoryDataGrid;
            function onProductChanged(productID) {
                debugger;
                orderHistoryDataGrid.option('dataSource', createOrderHistoryStore(productID));
            }
            function onDataGridInitialized(e) {
                orderHistoryDataGrid = e.component;
            }
            return $('<div>').addClass('form-container').dxForm({
                labelLocation: 'top',
                items: [
                    //    {
                    //    label: { text: 'Product' },
                    //    template: createProductSelectBoxTemplate(masterDetailData, onProductChanged),
                    //},
                    {
                        /*label: { text: 'Order History' },*/
                        template: createOrderHistoryTemplate(masterDetailData),
                    }],
            });
        };
    }

    //function createProductSelectBoxTemplate(masterDetailData, onProductChanged) {
    //    return function () {
    //        return $('<div>').dxSelectBox({
    //            dataSource: DevExpress.data.AspNet.createStore({
    //                key: 'ProductID',
    //                loadParams: { SupplierID: masterDetailData.SupplierID },
    //                loadUrl: `${url}/GetProductsBySupplier`,
    //            }),
    //            valueExpr: 'ProductID',
    //            displayExpr: 'ProductName',
    //            deferRendering: false,
    //            onContentReady(e) {
    //                const firstItem = e.component.option('items[0]');
    //                if (firstItem) {
    //                    e.component.option('value', firstItem.ProductID);
    //                }
    //            },
    //            onValueChanged(e) {
    //                onProductChanged(e.value);
    //            },
    //        });
    //    };
    //}

    function createOrderHistoryTemplate(masterDetailData, onDataGridInitialized) {
        debugger;
        data = DS.Table1.filter(item => item.JobTitleLookup == masterDetailData.ID);
        return function () {
            return $('<div>').dxDataGrid({
                onInitialized: onDataGridInitialized,
                dataSource: data,//DS.Table1, //DS.Table1.find(item => item.JobTitleLookup == masterDetailData.ID),
                paging: false,
                remoteOperations: false,
                showBorders: true,
                columns: [
                    'DepartmentName',
                    'EmpCostRate'
                ]
                 
                 
            });
        };
    }

    function createAddressTabTemplate(data) {
        return function () {
            return $('<div>').addClass('address-form form-container').dxForm({
                formData: data,
                colCount: 2,
                customizeItem(item) {
                    item.template = formItemTemplate;
                },
                items: ['Address', 'City', 'Region', 'PostalCode', 'Country', 'Phone'],
            });
        };
    }

    function formItemTemplate(item) {
        return $('<span>').text(item.editorOptions.value);
    }

    function createOrderHistoryStore(productID) {
        return DevExpress.data.AspNet.createStore({
            //key: 'OrderID',
            //loadParams: { ProductID: productID },
            //loadUrl: `${url}/GetOrdersByProduct`,
        });
    }
</script>--%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .clssdeletedrow {
        color: red;
    }

    .dx-button .dx-button-content {
        padding: unset !important;
    }

    .child1 {
        border: 1px solid #000000;
    }

    .child2 {
        border: 1px solid #1b13dc;
    }

    .contentPane {
        float: left;
        width: 100%;
        height: auto !important;
    }

    .dx-datagrid-headers {
        background-color: #f7f7f7 !important;
        color: black;
    }

    #gridContainer {
        max-height: 370px
    }

    .master-detail-caption {
        padding: 0 0 5px 10px;
        font-size: 14px;
        font-weight: bold;
    }

    .dx-datagrid-headers .dx-datagrid-table .dx-row {
        border-bottom: 1px solid #ddd;
        max-width: 100%;
        font-weight: bold;
        color: #4b4b4b;
    }
    .div_actions {
        margin: 7px 15px 7px 15px;
    }
    .primary-blueBtn_Export {
        background: none;
        border: none;
        padding:0px !important;
        width:110px !important; 
        background-color:#337AB7 !important;
    }
    .primary-blueBtn_Export .dxb {
        background: #337AB7;
        color: #FFF;
        border: 1px solid #4FA1D6 !important;
        border-radius: 4px;
        padding: 5px 13px 5px 13px !important;
        font-size: 12px;
        font-family: 'Roboto', sans-serif;
        font-weight: 500;
    }
</style>
<div class="row div_actions">
        <div id="checked"></div>
        <div id="btnAdd"></div>
    <div id="btnImport"></div>
    <dx:ASPxButton runat="server" Text="Export" OnClick="btnExport_Click" CssClass="primary-blueBtn_Export primary-btn-link" ID="btnExport"></dx:ASPxButton>
</div>
<div class="row">
    <div class="demo-container col-md-12 col-sm-12 col-xs-12">
        <div id="gridContainer"></div>
    </div>
</div>

<dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>


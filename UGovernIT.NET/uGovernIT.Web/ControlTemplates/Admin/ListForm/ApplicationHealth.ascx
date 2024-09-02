<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationHealth.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.ApplicationHealth" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.8/FileSaver.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/3.3.1/exceljs.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>--%>



<script src="https://cdnjs.cloudflare.com/ajax/libs/babel-polyfill/7.4.0/polyfill.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/4.0.1/exceljs.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/2.0.2/FileSaver.min.js"></script>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .tabs {
        display: flex;
        cursor: pointer;
        margin-bottom: 0px !IMPORTANT;
    }

    .dx-datagrid {
        font: 10px verdana;
    }

    .tab {
        padding: 10px 20px;
        background-color: lightgray;
        border: 1px solid gray;
    }

        .tab.active {
            background-color: white;
        }

    .content {
        display: none;
        /* padding: 20px;
        border: 1px solid gray;*/
    }

        .content.active {
            display: block;
        }

    .dx-datagrid-borders > .dx-datagrid-headers {
        border-top: 1px solid #ddd;
        font-weight: bold;
        color: #4b4b4b;
    }
</style>
<div id="loadpanel"></div>
<div class="row">
    <div class="col-md-11">
        <div class="tabs">
            <div class="tab" id="RAllocation" onclick="changeTab(0)">Resource Allocation</div>
            <div class="tab" id="ProjEst" onclick="changeTab(1)">Project Est. Allocation</div>
            <div class="tab" id="RMonthly" onclick="changeTab(2)">Monthly Allocation</div>
            <div class="tab" id="RMonthWise" onclick="changeTab(3)">Summary Month Wise</div>
            <div class="tab" id="RWeekWise" onclick="changeTab(4)">Summary Week Wise</div>
        </div>
    </div>
    <div class="col-md-1" id="utitlity">
        <div style="float: left;" id="btnExport" class="btnAddNew" cssclass="pdf-icon"></div>
        <div style="float: left; margin-left: 2px;" id="btnPdfExport" class="btnAddNew"></div>
        <div style="float: left; margin-left: 2px;" id="btnSendPDF" class="btnAddNew"></div>
    </div>
</div>
<%--<hr style="margin-top: 1px;
    margin-bottom: 5px;
    border: 0;
    border-top: 1px solid #eee;"/>--%>
<div class="row">
    <div class="col-md-12">
        <div class="content" id="content1">
            <div id="gridContainer0"></div>
        </div>
        <div class="content" id="content2">
            <div id="gridContainer1"></div>
        </div>
        <div class="content" id="content3">
            <div id="gridContainer2"></div>
        </div>
        <div class="content" id="content4">
            <div id="gridContainer3"></div>
        </div>
        <div class="content" id="content5">
            <div id="gridContainer4"></div>
        </div>
    </div>
</div>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    window.jsPDF = window.jspdf.jsPDF;
    var datagrid = '#gridContainer';
    var baseUrl = ugitConfig.apiBaseUrl;
    var DS = [];
    var newdt = "";
    var boolval = false;
    function changeTab(tabIndex, boolval = false) {
        const tabs = document.querySelectorAll(".tab");
        tabs.forEach((tab, index) => {
            if (index === tabIndex) {
                tab.classList.add("active");
            } else {
                tab.classList.remove("active");
            }
        });
        const contents = document.querySelectorAll(".content");
        contents.forEach((content, index) => {
            if (index === tabIndex) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: true
                });
                datagrid = '#gridContainer';
                var tablename = "";
                if (tabIndex == 0) { tablename = "RAllocation"; datagrid = datagrid + tabIndex; }
                else if (tabIndex == 1) { tablename = "ProjEst"; datagrid = datagrid + tabIndex; }
                else if (tabIndex == 2) { tablename = "RMonthly"; datagrid = datagrid + tabIndex; }
                else if (tabIndex == 3) { tablename = "RMonthWise"; datagrid = datagrid + tabIndex; }
                else if (tabIndex == 4) { tablename = "RWeekWise"; datagrid = datagrid + tabIndex; }
                content.classList.add("active");
                if (!content.dataset.loaded) {
                    $(() => {
                        $.get(baseUrl + "api/rmmapi/GetCorruptedAllocations?tabname=" + tablename + "&IncludedClosed=" + boolval, function (data) {
                            DS = data;
                            dt = DS.Table;

                            $(datagrid).dxDataGrid({
                                dataSource: dt,
                                keyExpr: 'ID',
                                showBorders: true,
                                paging: false,
                                remoteOperations: false,
                                cellHintEnabled: true,
                                rowAlternationEnabled: true,
                                allowColumnResizing: false,
                                columnAutoWidth: true,
                                columnFixing: {
                                    enabled: true
                                },
                                allowColumnReordering: true,
                               /* columnChooser: { enabled: true },*/
                                filterRow: { visible: true },
                                searchPanel: { visible: true },
                                groupPanel: { visible: true },
                                selection: { mode: "single" },
                                grouping: {
                                    contextMenuEnabled: true
                                },
                                summary: {
                                    groupItems: [{
                                        summaryType: "count",
                                    }]
                                },
                                //export: {
                                //    enabled: true,
                                //    formats: ['xlsx', 'pdf']
                                //},
                                toolbar: {
                                    items: [
                                        "groupPanel",
                                        {
                                            location: "after",
                                            widget: "dxCheckBox",
                                            options: {
                                                text: "Closed Projects",
                                                width: 136,
                                                activeStateEnabled: true,
                                                onInitialized: function (e) {
                                                    e.component.option("value", boolval);
                                                },
                                                onValueChanged: function (e) {
                                                    boolval = e.value;
                                                    $(datagrid).data("preserveCheckbox", boolval);
                                                    changeTab(tabIndex, boolval);
                                                },
                                            },
                                        },
                                       
                                        {
                                            location: "after",
                                            widget: "dxButton",
                                            options: {
                                                tooltip: "Export to Excel",
                                                stylingMode: 'text',
                                                icon: "/content/images/excel-icon.png",
                                                focusStateEnabled: false,
                                                onClick(e) {
                                                    var workbook = new ExcelJS.Workbook();
                                                    var worksheet = workbook.addWorksheet("CorruptData");
                                                    DevExpress.excelExporter.exportDataGrid({
                                                        component: $(datagrid).dxDataGrid("instance"),
                                                        worksheet: worksheet
                                                    }).then(function () {
                                                        workbook.xlsx.writeBuffer().then(function (buffer) {
                                                            saveAs(new Blob([buffer], { type: "application/octet-stream" }), "CorruptData.xlsx");
                                                        });
                                                    });
                                            },
                                        },
                                        },
                                        {
                                            location: "after",
                                            widget: "dxButton",
                                            options: {
                                                icon: "collapse",
                                                /*text: "Collapse All",*/
                                                width: 30,
                                                height: 27,
                                                onClick(e) {
                                                    // Handle button click
                                                    //const expanding = e.component.option("text") === "Expand All";
                                                    //$(datagrid).dxDataGrid("instance").option("grouping.autoExpandAll", expanding);
                                                    //e.component.option("text", expanding ? "Collapse All" : "Expand All");
                                                    const expanding = e.component.option("icon") === "collapse" ? "expand" : "collapse";
                                                    e.component.option("icon", expanding);
                                                    $(datagrid).dxDataGrid("instance").option("grouping.autoExpandAll", expanding === "expand");
                                                },
                                            },
                                        },
                               /* "exportButton",*/
                                /*"columnChooserButton",*/
                                "searchPanel",
                                    ],
                            },
                                onInitialized: function (e) {
                                    var dataGrid = e.component;
                                    var preserveCheckbox = $(datagrid).data("preserveCheckbox");
                                    dataGrid.option("toolbar.items[0].options.value", preserveCheckbox);
                                },
                                //onExporting(e) {
                                //    if (e.format === 'xlsx')
                                //    {
                                //        const workbook = new ExcelJS.Workbook();
                                //        const worksheet = workbook.addWorksheet("Main sheet");
                                //        DevExpress.excelExporter.exportDataGrid({
                                //            worksheet: worksheet,
                                //            component: e.component,
                                //        }).then(function () {
                                //            workbook.xlsx.writeBuffer().then(function (buffer) {
                                //                saveAs(new Blob([buffer], { type: "application/octet-stream" }), "DataGrid.xlsx");
                                //            });
                                //        });
                                //        e.cancel = true;
                                //    }
                                //    else if (e.format === 'pdf')
                                //    {
                                //        e.component.beginUpdate();
                                //        e.component.repaint();
                                //        const doc = new jsPDF('landscape', 'mm', 'a4');

                                //        // Access the columns from e.component.getVisibleColumns()
                                //        const columns = e.component.getVisibleColumns();

                                //        // Map column widths
                                //        const columnWidths = columns.map((col) => {
                                //            // Handle columns with 'auto' width and set a default width
                                //            return col.width === 'auto' ? 100 : col.width;
                                //        });

                                //        const pdfExportOptions = {
                                //            component: e.component,
                                //            jsPDFDocument: doc,
                                //            customizeCell: function (options) {
                                //                const columnIndex = options.gridCell.columnIndex;
                                //                options.pdfCell.width = columnWidths[columnIndex];
                                //            },
                                //        };

                                //        DevExpress.pdfExporter.exportDataGrid(pdfExportOptions).then(() => {
                                //            doc.save('DataGrid.pdf');
                                //        }).catch(function (error) {
                                //            console.error("Error exporting to PDF:", error);
                                //        }).finally(function () {
                                //            e.component.endUpdate();
                                //        });

                                //        e.cancel = true;
                                //    }
                                //}
                            });
                        if (dt==null || dt.length == 0) {
                            $("#loadpanel").dxLoadPanel("hide");
                            return;
                        }
                        const columns = Object.keys(dt[0]);
                        // Choose a column to use for grouping (for example, the first column)
                        const groupingColumn = columns[0];
                        // Create the column configuration dynamically, setting groupIndex for the first column
                        const dynamicColumns = columns.map((column, index) => ({
                            dataField: column,
                            caption: column,
                            groupIndex: index === 0 ? 0 : undefined,
                        }));
                        // Set the dynamically created columns in the grid
                        $(datagrid).dxDataGrid("instance").option("columns", dynamicColumns);
                        $("#loadpanel").dxLoadPanel("hide");
                    });
                });
    }

            } else {
        content.classList.remove("active");
    }
        });
    }
    //$("#btnExport").dxButton({
    //    tooltip: "Export to Excel",
    //    stylingMode: 'text',
    //    icon: "/content/images/excel-icon.png",
    //    focusStateEnabled: false,
    //    onClick: function (e) {
    //        var workbook = new ExcelJS.Workbook();
    //        var worksheet = workbook.addWorksheet("CorruptData");
    //        DevExpress.excelExporter.exportDataGrid({
    //            component: $(datagrid).dxDataGrid("instance"),
    //            worksheet: worksheet
    //        }).then(function () {
    //            workbook.xlsx.writeBuffer().then(function (buffer) {
    //                saveAs(new Blob([buffer], { type: "application/octet-stream" }), "CorruptData.xlsx");
    //            });
    //        });

    //    }
    //});
</script>

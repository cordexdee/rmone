<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceUtilizationIndex.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.ResourceUtilizationIndex" %>
<script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.8/FileSaver.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/3.3.1/exceljs.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.0.0/jspdf.umd.min.js"></script>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .reportPanelImage {
        display: inline-block;
        padding-left:5px;            
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var category = [
        "Division",
        "Studio",
        "Sector"
    ];

    var CurrentYear = new Date().getFullYear();
    var lstYear = [];
    for (var i = CurrentYear; i >= 2018; i--) {
        lstYear.push(i);
    }

    var rbOptions = ["FTE", "%"];

    var reportParam =
    {
        CurrentYear: CurrentYear,
        Category: "Division",
        Type: "FTE"
    };

    $(function () {
        $(document).ready(function () {
            BindGridData();
            window.jsPDF = window.jspdf.jsPDF;
        });


        $("#ddlCategory").dxSelectBox({
            items: category,
            value: category[0],
            onValueChanged: function (e) {
                reportParam.Category = e.value;
            }
        });


        $("#ddlYear").dxSelectBox({
            items: lstYear,
            value: lstYear[0],
            onValueChanged: function (e) {
                reportParam.CurrentYear = e.value;
            }
        });


        var radioGroup = $("#rbType").dxRadioGroup({
            items: rbOptions,
            layout: "horizontal",
            onValueChanged: function (e) {
                if (e.value == '%')
                    reportParam.Type = 'pct';
                else
                    reportParam.Type = e.value;     
            }
        }).dxRadioGroup("instance");

        radioGroup.option("value", rbOptions[0]);

        $("#btnViewReport").dxButton({
            text: "View Report",
            //icon: "/content/Images/saveAsTemplate.png",
            focusStateEnabled: false,
            onClick: function (e) {
                BindGridData();
            }
        });

        $("#btnExport").dxButton({
            tooltip: "Export to Excel",
            stylingMode: 'text',
           icon: "/content/images/excel-icon.png",
            focusStateEnabled: false,
            onClick: function (e) {
                //var workbook = new ExcelJS.Workbook();
                //var worksheet = workbook.addWorksheet(reportParam.Category);
                //DevExpress.excelExporter.exportDataGrid({
                //    worksheet: worksheet,
                //    component: e.component,
                //    customizeCell: function (options) {
                //        var excelCell = options;
                //        excelCell.font = { name: 'Arial', size: 12 };
                //        excelCell.alignment = { horizontal: 'left' };
                //    }
                //}).then(function () {
                //    workbook.xlsx.writeBuffer().then(function (buffer) {
                //        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'ResourceUtilizationIndex.xlsx');
                //    });
                //});
                //e.cancel = true;

                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet(reportParam.Category);

                DevExpress.excelExporter.exportDataGrid({
                    component: $("#DivKPI").dxDataGrid("instance"),
                    worksheet: worksheet
                }).then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: "application/octet-stream" }), "ResourceUtilizationIndex_" + reportParam.CurrentYear + ".xlsx");
                    });
                });

            }
        });


      $('#btnPdfExport').dxButton({
                tooltip: "Export to PDF",
                stylingMode: 'text',
             icon: "/content/images/pdf-icon.png",
            onClick: function() {
                const doc = new jsPDF();
                DevExpress.pdfExporter.exportDataGrid({
                    jsPDFDocument: doc,
                    component: kpidxDataGrid
                }).then(function() {
                    doc.save("ResourceUtilizationIndex_" + reportParam.CurrentYear + ".pdf");
                 });
              }
        });


      $('#btnSendPDF').dxButton({
                tooltip: "Send PDF To Email",
                icon: "/content/images/MailTo.png",
                stylingMode: 'text',
            onClick: function() {
                const doc = new jsPDF();
                DevExpress.pdfExporter.exportDataGrid({
                    jsPDFDocument: doc,
                    component: kpidxDataGrid
                }).then(function() {
                     var binary = doc.output();
                     var reqData = binary ? btoa(binary) : "";
                     var url="/api/rmmapi/GetPDFData";
                    $.ajax({
                            url:url,
                            data: JSON.stringify(reqData),
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            success:function(response){
                            alert(response.message);
                            }
                     });
                 });
              }
        });

        var kpidxDataGrid = $("#DivKPI").dxDataGrid({
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },
            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15,
                enabled: false
            },
            allowColumnResizing: true,
            //export: {
            //    enabled: true
            //},
            //columnChooser: {
            //    enabled: true
            //},         
            loadPanel: {
                enabled: false
            },
            columns: [
                {
                    dataField: "Category",
                    caption: "Category",
                    groupIndex: 0
                },
                {
                    dataField: "KPI",
                    caption: "KPI",
                    width: "15%"
                },
                {
                    dataField: "Jan",
                    caption: "Jan",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Feb",
                    caption: "Feb",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Mar",
                    caption: "Mar",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Apr",
                    caption: "Apr",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "May",
                    caption: "May",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Jun",
                    caption: "Jun",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Jul",
                    caption: "Jul",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Aug",
                    caption: "Aug",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Sep",
                    caption: "Sep",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Oct",
                    caption: "Oct",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Nov",
                    caption: "Nov",
                    alignment: "right",
                    dataType: "string"
                },
                {
                    dataField: "Dec",
                    caption: "Dec",
                    alignment: "right",
                    dataType: "string"
                }
            ]
        }).dxDataGrid('instance');

        $("#autoExpand").dxCheckBox({
            value: true,
            text: "Expand",
            onValueChanged: function (data) {
                kpidxDataGrid.option("grouping.autoExpandAll", data.value);
            }
        });


        function BindGridData() {
            $("#loadpanel").dxLoadPanel({
                message: "Loading...",
                visible: true
            });
            $.get("/api/rmmapi/GetResourceUtilizationIndex?Year=" + reportParam.CurrentYear + "&Category=" + reportParam.Category + "&Type=" + reportParam.Type, function (data, status) {
                if (data) {
                    kpidxDataGrid.option('dataSource', data);
                }
                else {
                    data = [];
                    kpidxDataGrid.option('dataSource', data);
                }
                $("#loadpanel").dxLoadPanel("hide");
            });

            $("#DivKPI").show();

        }

        var reportBtn = $("#reportPanelImage").dxButton({
            stylingMode: "text",
            icon: "/Content/Images/reports-Black.png",
            height: 25,
            width: 25,
            hoverStateEnabled: false,
            focusStateEnabled: false,
            hint:"Report Options",
            onClick: function (e) {
                if ($(".ExportOption-btns").is(":visible") == true)
                    $(".ExportOption-btns").hide();
                else
                    $(".ExportOption-btns").show();
            }
        });
        
    });
</script>
<div id="loadpanel"></div>
<div style="width: 99%;">
    <div style="float: left; padding-top: 20px; display:inline-block">

        <div style="float: left;">
            <div id="autoExpand"></div>
        </div>
        <div style="float: left; padding-left: 20px;">
            <div id="ddlCategory"></div>
        </div>

        <div style="float: left; padding-left: 20px;">
            <div id="ddlYear"></div>
        </div>

        <div style="float: left; padding-left: 20px;">
            <div style="float: left;" id="rbType"></div>
        </div>

        <div style="float: left; padding-left: 40px;">
            <div style="float: left;" id="btnViewReport" class="btnAddNew"></div>
        </div>

        <div id="exportOptions" class="ExportOption-btns" style="float: right; padding-left: 300px;display:none">
            <div style="float: left;" id="btnExport" class="btnAddNew" CssClass="pdf-icon"></div>
            <div style="float: left; margin-left: 2px;" id="btnPdfExport" class="btnAddNew"></div>
            <div style="float: left; margin-left: 2px;" id="btnSendPDF" class="btnAddNew"></div>
        </div>
        <div id="reportPanelImage" style="float:right;" >
            
        </div>
    </div>

    <div style="float: left; clear: both; height: 10px;"></div>
    <div style="float: left; clear: both; width: 100%; display: none;" id="DivKPI">
    </div>
</div>


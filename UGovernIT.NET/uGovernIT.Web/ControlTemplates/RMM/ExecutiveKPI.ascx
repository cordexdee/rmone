<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecutiveKPI.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.ExecutiveKPI" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var category = [
        "Job Title",
        "Role",
        "Division",
        "Sector",
        "Studio",
        "Project View"
    ];

    var CurrentYear = new Date().getFullYear();
       
    $(function () {
        $(document).ready(function () {
            $("#loadpanel").dxLoadPanel({
                message: "Loading...",
                visible: true
            });
            $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=jobtitle", function (data, status) {
                if (data) {
                    jobtitledxDataGrid.option('dataSource', data);
                }
                else {
                    data = [];
                    jobtitledxDataGrid.option('dataSource', data);
                }
            });
        });


        $("#ddlCategory").dxSelectBox({
            items: category,
            value: category[0],
            onValueChanged: function (e) {
                $("#DivJobTitle").hide();
                $("#DivRole").hide();
                $("#DivDivision").hide();
                $("#DivSector").hide();
                $("#DivStudio").hide();
                $("#DivProjectView").hide();
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: true
                });
                if (e.value == "Job Title") {
                    $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=jobtitle", function (data, status) {
                        if (data) {
                            jobtitledxDataGrid.option('dataSource', data);
                        }
                        else {
                            data = [];
                            jobtitledxDataGrid.option('dataSource', data);
                        }
                    });

                    $("#DivJobTitle").show();
                }
                else if (e.value == "Role") {
                    $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=" + e.value, function (data, status) {
                        if (data) {
                            roledxDataGrid.option('dataSource', data);
                        }
                        else {
                            data = [];
                            roledxDataGrid.option('dataSource', data);
                        }
                    });

                    $("#DivRole").show();
                }
                else if (e.value == "Division") {
                    $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=" + e.value, function (data, status) {
                        if (data) {
                            divisiondxDataGrid.option('dataSource', data);
                        }
                        else {
                            data = [];
                            divisiondxDataGrid.option('dataSource', data);
                        }
                    });

                    $("#DivDivision").show();
                }
                else if (e.value == "Sector") {
                    $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=" + e.value, function (data, status) {
                        if (data) {
                            sectordxDataGrid.option('dataSource', data);
                        }
                        else {
                            data = [];
                            sectordxDataGrid.option('dataSource', data);
                        }
                    });

                    $("#DivSector").show();
                }
                else if (e.value == "Studio") {
                    $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=" + e.value, function (data, status) {
                        if (data) {
                            studiodxDataGrid.option('dataSource', data);
                        }
                        else {
                            data = [];
                            studiodxDataGrid.option('dataSource', data);
                        }
                    });

                    $("#DivStudio").show();
                }
                else if (e.value == "Project View") {
                    $.get("/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=projectview", function (data, status) {
                        if (data) {
                            projectdxDataGrid.option('dataSource', data);
                        }
                        else {
                            data = [];
                            projectdxDataGrid.option('dataSource', data);
                        }
                    });

                    $("#DivProjectView").show();
                }
            }
        });

        var jobtitledxDataGrid  = $("#DivJobTitle").dxDataGrid({
            //dataSource: "/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=jobtitle",
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },

            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "Category",
                    caption: "Job Title",
                    width: "10%"                  
                },
                {
                    dataField: "Margins",
                    caption: "Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectedMargins",
                    caption: "Projected Margins",                    
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectMargins",
                    caption: "Project Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "EffectiveUtilization",
                    caption: "Effective Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "CommittedUtilization",
                    caption: "Committed Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "PipelineUtilization",
                    caption: "Pipeline Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "RevenuesRealized",
                    caption: "Revenues Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "RevenuesLost",
                    caption: "Revenues Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedRevenues",
                    caption: "Committed Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "PipelineRevenues",
                    caption: "Pipeline Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                }
            ]
        }).dxDataGrid('instance');


        var roledxDataGrid  = $("#DivRole").dxDataGrid({
            //dataSource: "/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=role",
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },
            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "Category",
                    caption: "Role",
                    width: "10%"                  
                },
                {
                    dataField: "Margins",
                    caption: "Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectedMargins",
                    caption: "Projected Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectMargins",
                    caption: "Project Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "EffectiveUtilization",
                    caption: "Effective Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "CommittedUtilization",
                    caption: "Committed Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "PipelineUtilization",
                    caption: "Pipeline Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "RevenuesRealized",
                    caption: "Revenues Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "RevenuesLost",
                    caption: "Revenues Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedRevenues",
                    caption: "Committed Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "PipelineRevenues",
                    caption: "Pipeline Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                }
            ]
        }).dxDataGrid('instance');

        var divisiondxDataGrid  = $("#DivDivision").dxDataGrid({
            //dataSource: "/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=division",
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },
            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "Category",
                    caption: "Division",
                    width: "10%"                  
                },
                {
                    dataField: "Margins",
                    caption: "Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectedMargins",
                    caption: "Projected Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectMargins",
                    caption: "Project Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "EffectiveUtilization",
                    caption: "Effective Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "CommittedUtilization",
                    caption: "Committed Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "PipelineUtilization",
                    caption: "Pipeline Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "RevenuesRealized",
                    caption: "Revenues Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "RevenuesLost",
                    caption: "Revenues Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedRevenues",
                    caption: "Committed Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "PipelineRevenues",
                    caption: "Pipeline Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "MarginsRealized",
                    caption: "Margins Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "MarginsLost",
                    caption: "Margins Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedMargins",
                    caption: "Committed Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                }
            ]
        }).dxDataGrid('instance');

        var sectordxDataGrid  = $("#DivSector").dxDataGrid({
            //dataSource: "/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=sector",
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },
            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "Category",
                    caption: "Sector",
                    width: "10%"
                },
                {
                    dataField: "Margins",
                    caption: "Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectedMargins",
                    caption: "Projected Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectMargins",
                    caption: "Project Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "EffectiveUtilization",
                    caption: "Effective Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "CommittedUtilization",
                    caption: "Committed Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "PipelineUtilization",
                    caption: "Pipeline Utilization %",
                    alignment: "right",
                    dataType: "number"                    
                },
                {
                    dataField: "RevenuesRealized",
                    caption: "Revenues Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "RevenuesLost",
                    caption: "Revenues Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedRevenues",
                    caption: "Committed Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "PipelineRevenues",
                    caption: "Pipeline Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "MarginsRealized",
                    caption: "Margins Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "MarginsLost",
                    caption: "Margins Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedMargins",
                    caption: "Committed Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                }
            ]
        }).dxDataGrid('instance');

        var studiodxDataGrid = $("#DivStudio").dxDataGrid({
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },
            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "Category",
                    caption: "Studio",
                    width: "10%"
                },
                {
                    dataField: "Margins",
                    caption: "Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectedMargins",
                    caption: "Projected Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ProjectMargins",
                    caption: "Project Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "EffectiveUtilization",
                    caption: "Effective Utilization %",
                    alignment: "right",
                    dataType: "number"
                },
                {
                    dataField: "CommittedUtilization",
                    caption: "Committed Utilization %",
                    alignment: "right",
                    dataType: "number"
                },
                {
                    dataField: "PipelineUtilization",
                    caption: "Pipeline Utilization %",
                    alignment: "right",
                    dataType: "number"
                },
                {
                    dataField: "RevenuesRealized",
                    caption: "Revenues Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "RevenuesLost",
                    caption: "Revenues Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedRevenues",
                    caption: "Committed Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "PipelineRevenues",
                    caption: "Pipeline Revenues",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "MarginsRealized",
                    caption: "Margins Realized",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "MarginsLost",
                    caption: "Margins Lost",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "CommittedMargins",
                    caption: "Committed Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                }
            ]
        }).dxDataGrid('instance');

        var projectdxDataGrid  = $("#DivProjectView").dxDataGrid({
            //dataSource: "/api/rmmapi/GetExecutiveKpi?Year=" + CurrentYear + "&Category=projectview",
            showBorders: true,
            rowAlternationEnabled: true,
            selection: {
                mode: "single"
            },
            groupPanel: {
                visible: false
            },
            paging: {
                pageSize: 15
            },
            allowColumnResizing: true,
            onContentReady: function (e) {
                $("#loadpanel").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            },
            columns: [
                {
                    dataField: "ProjectId",
                    caption: "Project Id",
                    alignment: "center"
                },
                {
                    dataField: "Category",
                    caption: "Ticket Id",
                    alignment: "center"                  
                },
                {
                    dataField: "TicketTitle",
                    caption: "Title"             
                },
                {
                    dataField: "ResourceHours",
                    caption: "Resource Hours",
                    alignment: "right",
                    format: "###,###,###,###", 
                    dataType: "number"
                },
                {
                    dataField: "ResourceBillings",
                    caption: "Billings",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ResourceCosts",
                    caption: "Costs",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"
                },
                {
                    dataField: "ResourceMargins",
                    caption: "Resource Margins",
                    alignment: "right",
                    dataType: "number",
                    format: "currency"                 
                },
                {
                    dataField: "UtilizationRate",
                    caption: "Utilization Rate %",
                    alignment: "right",
                    dataType: "number"               
                }
            ]
        }).dxDataGrid('instance');
                
    });
</script>

<div id="loadpanel">
</div>
<div style="padding-left: 15px; padding-right: 15px;">
    <div style="float:left; padding-top:20px; ">
        <div style="float:left;" id="ddlCategory">
        </div>
    </div>

    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%;" id="DivJobTitle">
    </div>
    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; display:none;" id="DivRole">
    </div>

    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; display:none;" id="DivDivision">
    </div>
    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; display:none;" id="DivSector">
    </div>
    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; display:none;" id="DivStudio">
    </div>
    <div style="float:left; clear:both;height:10px;"></div>
    <div style="float:left; clear:both; width:100%; display:none;" id="DivProjectView">
    </div>    
</div>
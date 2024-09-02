<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FinancialDetails.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.FinancialDetails" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var baseUrl = ugitConfig.apiBaseUrl;
    
    $(document).ready(function () {
        const filter = encodeURIComponent('<%=Filter%>');
        const title = '<%=Filter%>';
        $.ajax({
            type: "GET",
            url: baseUrl + `/api/CoreRMM/GetFinancialChartDetails?filter=${filter}&type=${'<%=DataType%>'}`,
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (dataSource) {
                sortByMonth(dataSource);
                $('#divpiechart').dxChart({
                    palette: 'Soft Pastel',
                    dataSource,
                    title: 'Revenue Chart: ' + title,
                    commonSeriesSettings: {
                        argumentField: 'Month',
                        type: 'stackedbar',
                    },
                    series: [{
                        name: 'PipelineRevenues',
                        caption: 'Pipeline',
                        valueField: 'PipelineRevenues',
                        label: {
                            indentFromAxis: 10, staggeringSpacing: 20, visible: true,
                            customizeText(point) {
                                return convertToMillion(point.value);
                            }
                        },
                        
                    },
                        {
                        name: 'CommittedRevenues',
                        caption: 'Committed',
                            valueField: 'CommittedRevenues',
                            label: {
                                indentFromAxis: 10, staggeringSpacing: 20, visible: true,
                                customizeText(point) {
                                    return convertToMillion(point.value);
                                }
                            },
                            
                        },
                        {
                            axis: 'MissedRevenues',
                            type: 'spline',
                            valueField: 'MissedRevenues',
                            name: 'Missed Revenues',
                            color: '#FA8072',
                        },
                    ],
                    scrollBar: {
                        visible: false
                    },
                    zoomAndPan: {
                        argumentAxis: "both"
                    },
                    valueAxis: [{
                        grid: {
                            visible: true,
                        },
                    }, {
                        name: 'MissedRevenues',
                        position: 'right',
                        grid: {
                            visible: true,
                        },
                        title: {
                            text: 'Missed Revenues, millions',
                        },
                    }],
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr);
            }
        });
    });
   
</script>
<div id="divChartDetailPanel" runat="server" class="ChartDetails">
    <div class="row">
        <div class="col-md-12 py-3">

            <div id="divpiechart" class="ChartDetailsChildElement"></div>
            
        </div>

    </div>
</div>
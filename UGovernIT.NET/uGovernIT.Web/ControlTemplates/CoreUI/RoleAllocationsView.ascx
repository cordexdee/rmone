<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleAllocationsView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.RoleAllocationsView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-data-row td {
        text-align: center !important;
        vertical-align:middle !important;
    }
    .dx-datagrid-headers td {
        text-align: center !important;
        vertical-align:middle !important;
        font: bold !important;
        color:#4A6EE2;
    }

    .dx-header-row > td[role="columnheader"] > div.dx-datagrid-text-content {
        font-size: 17px;
        font-weight: bold;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;

    $(document).ready(function () {

        var roleAllocations = $("#divRoleAllocations").dxDataGrid({
            dataSource: baseUrl + "/api/CoreRMM/GetRoleWiseAllocations",
            width: '<%=Width%>',
            height: '<%=Height%>',
            showRowLines: true,
            ShowBorders: true,
            columns: [
                {
                    dataField: 'RoleName',
                    caption: 'Role'
                },
                {
                    dataField: 'Capacity',
                    caption: 'Current Capacity'
                },
                {
                    dataField: 'Utilization',
                    caption: 'YTD Utilization',
                    format: "#0.#",
                    customizeText: (options) => {
                        return options.valueText + '%';
                    }
                }
            ],
            onRowPrepared: function (e) {
                //debugger;
                if (e.rowType == 'header') {
                    e.rowElement.css({ font: 'bold !important' })
                }
                if (e.rowType == 'data') {
                    e.rowElement.css({ height: 50 });
                    if (e.data.RoleName == 'Overall')
                        e.rowElement.css({ color: '#4A6EE2'})
                }
            }
        });

        $("#divRoleAllocations").click(function () {
            window.parent.UgitOpenPopupDialog('/layouts/ugovernit/delegatecontrol.aspx?control=capcityreport', '', 'Resource Utilization', '90', '90', '', true);

        });
    });
</script>
<div>
    <div id="divRoleAllocations">

    </div>
    <div id="divBillings">

    </div>
</div>
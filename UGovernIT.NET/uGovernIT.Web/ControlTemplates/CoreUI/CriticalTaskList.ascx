<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CriticalTaskList.ascx.cs" Inherits="uGovernIT.Web.CriticalTaskList" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        $("#DivTaskList").dxDataGrid({
            dataSource: "/api/CoreRMM/GetCriticalTasks",
            keyExpr: "ID",
            selection: {
                mode: "single"
            },
            columnAutoWidth: true,
            columns: [
                {
                    dataField: "Title",
                    dataType: "string"
                }
            ],
            onSelectionChanged: function (selectedItems) {
                
                var item = selectedItems.selectedRowsData[0];
                var params = "moduleName=" + item.ModuleNameLookup + "&TaskId=" + item.ID + "&projectID=" + item.TicketId + "&viewType=1";
                window.parent.parent.UgitOpenPopupDialog('<%=editTaskFormUrl %>', params, item.Title, '90', '90', false, "");
            },
            rowTemplate: function (container, item) {
                var data = item.data,
                    markup = "<tbody class='dx-row " + ((item.rowIndex % 2) ? 'dx-row-alt' : '') + "'>" +
                        "<tr class='main-row'>" +
                        "<td rowspan='2'><b>" + data.Title + "</b></td>" +
                             
                        "</tr>" +
                        "<tr class='notes-row'>" +
                        "<td><div>" + data.ProjectTitle + "</div></td>" +
                        "</tr>" +
                        "</tbody>";

                container.append(markup);
            }
        });
    });

   
</script>

<div>
    <div id="DivTaskList">

    </div>
</div>
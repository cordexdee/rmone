<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EstimatedRemainingHoursReport_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.EstimatedRemainingHoursReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

<style type="text/css">
    body {
        overflow: auto !important;
    }

    #s4-leftpanel {
        display: none;
    }

    .s4-ca {
        margin-left: 0px !important;
    }

    #s4-ribbonrow {
        height: auto !important;
        min-height: 0px !important;
    }

    #s4-ribboncont {
        display: none;
    }

    #s4-titlerow {
        display: none;
    }

    .s4-ba {
        width: 100%;
        min-height: 0px !important;
    }

    #s4-workspace {
        float: left;
        width: 100%;
        overflow: auto !important;
    }

    body #MSO_ContentTable {
        min-height: 0px !important;
        position: inherit;
    }

    .dataCellStyle {
        text-align: center !important;
        padding: 0px 1px 0px 0 !important;
    }

    .footerCell {
        font-weight: bold;
        margin-top: 5px !important;
        margin-bottom: 5px !important;
        text-align: center;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
    }

    .totalrowstyle {
        font-weight: bold;
        border-color: #000 !important;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
    }

    .headerCellStyle {
        font-weight: bold;
        margin-top: 5px !important;
        margin-bottom: 5px !important;
        text-align: center;
        padding-right: 7px !important;
    }

    .subtotalrowstyle {
        font-weight: bold;
        border-color: #000 !important;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
        text-align: center !important;
    }

    .subtotalrowstyletitle {
        font-weight: bold;
        border-color: #000 !important;
        border-top: 2px solid !important;
        border-bottom: 1px solid !important;
        text-align: right !important;
    }

    .htmlFooterCell {
        width: 67px;
        font-weight: bold;
        margin-top: 5px !important;
        margin-bottom: 5px !important;
        text-align: center;
        border-bottom: 1px solid !important;
    }

    .htmlHeaderCell {
        width: 140px;
        font-weight: bold;
        margin-top: 5px !important;
        margin-bottom: 5px !important;
        text-align: center;
        border-bottom: 1px solid !important;
    }

    .dxgvIndentCell, .dxgvIndentCell dxgv, .gvEstimatedRemaningHours-row td.dxgvIndentCell .Hide {
        border: none;
        border-right: none;
    }
    
    .gvEstimatedRemaningHours-row td.dxgvIndentCell{
        border: none;
        border-right: none;
    }
</style>

<script type="text/javascript">

    function downloadExcel(obj) {
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function startPrint(obj) {
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=print";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    $(function () {
        collapseAllTask(true);
        var print = '<%=printReport%>';
        if (print == "True") {
            $("#s4-titlerow").remove();
            window.print();
        }
    });

    function CloseChildren(level, id, imageObj) {
        imageObj.setAttribute("src", "/_layouts/15/images/uGovernIT/maximise.gif");
        imageObj.setAttribute("onclick", "event.cancelBubble=true; OpenChildren('" + level + "', '" + id + "', this)");

        var currentTask = $(".gvEstimatedRemaningHours-table .gvEstimatedRemaningHours-row[task='" + id + "']");
        currentTask.attr("mode", "collapse");

        var childTasks = $(".gvEstimatedRemaningHours-table .gvEstimatedRemaningHours-row[parenttask='" + id + "']");
        for (var i = 0; i < childTasks.length; i++) {
            var task = childTasks.get(i);
            $(task).css("display", "none");

            if ($(task).find(".task-title img[colexpand]").length > 0) {
                CloseChildren($(task).attr("level"), $(task).attr("task"), $(task).find(".task-title img[colexpand]").get(0))
            }
        }
    }

    function OpenChildren(level, id, imageObj) {
        imageObj.setAttribute("src", "/_layouts/15/images/uGovernIT/minimise.gif");
        imageObj.setAttribute("onclick", "event.cancelBubble=true; CloseChildren('" + level + "', '" + id + "', this)");

        var currentTask = $(".gvEstimatedRemaningHours-table .gvEstimatedRemaningHours-row[task='" + id + "']");
        currentTask.attr("mode", "expand");

        var childTasks = $(".gvEstimatedRemaningHours-table .gvEstimatedRemaningHours-row[parenttask='" + id + "']");
        for (var i = 0; i < childTasks.length; i++) {
            var task = childTasks.get(i);
            $(task).css("display", "");
            if ($(task).find(".task-title img[colexpand]").length > 0) {
                OpenChildren($(task).attr("level"), $(task).attr("task"), $(task).find(".task-title img").get(0))
            }
        }

    }


    function collapseParents(jqTask) {
        var parentID = jqTask.attr("parenttask");
        var parentTask = $("#PMMTaskRowLevel_" + parentID);
        if (!parentTask) {
            return false;
        }

        var maxMinIcon = parentTask.find(".task-title img[colexpand]");
        var parentID = parentTask.attr("parenttask");
        var taskID = parentTask.attr("task");
        var level = parentTask.attr("level");
        if (maxMinIcon.length > 0) {
            OpenChildren(level, taskID, maxMinIcon.get(0))
        }

        if (Number(parentID) != 0) {
            collapseParents(parentTask);
        }
    }

    function expandAllTask(doDefault) {
        var trs = $(".gvEstimatedRemaningHours-table .gvEstimatedRemaningHours-row[parentTask='0']")
        var minLength = 2;
        if (!doDefault) {
            minLength = 0;
        }

        if (trs.length > minLength) {
            $.each(trs, function (i, item) {
                var maxMinIcon = $(item).find(".task-title img[colexpand]");
                var parentID = $(item).attr("parenttask");
                var taskID = $(item).attr("task");
                var level = $(item).attr("level");
                if (maxMinIcon.length > 0) {
                    OpenChildren(level, taskID, maxMinIcon.get(0))
                }
            });
        }
    }

    function collapseAllTask(doDefault) {
        var trs = $(".gvEstimatedRemaningHours-table .gvEstimatedRemaningHours-row[parenttask='0']")
        var minLength = 2;
        if (!doDefault) {
            minLength = 0;
        }
        if (trs.length > minLength) {
            $.each(trs, function (i, item) {
                var maxMinIcon = $(item).find(".task-title img[colexpand]");
                var parentID = $(item).attr("parenttask");
                var taskID = $(item).attr("task");
                var level = $(item).attr("level");
                if (maxMinIcon.length > 0) {
                    CloseChildren(level, taskID, maxMinIcon.get(0))
                }
            });
        }
    }

    function openSprintTask(sprintTitle) {
        var params = "PublicTicketID=" + '<%= TicketPublicId %>' + "&viewType=" + 1 + "&sprintTitle=" + sprintTitle;
         window.parent.UgitOpenPopupDialog('<%= sprintTaskUrl %>', params, 'Sprint', '90', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
     }

     function editTask(taskID, order) {
         var projectID = "<%= TicketId %>";
        var params = "projectID=" + projectID + "&taskID=" + taskID + "&moduleName=PMM";
        window.parent.UgitOpenPopupDialog('<%= editTaskFormUrl %>', params, 'Edit Task #' + order, '800px', '100', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>


<table width="100%">
    <tr>
        <td style="text-align: center; font-weight: bold; font-size: medium;">
            <asp:Label ID="lblReportTitle" runat="server" Text="Estimated Remaining Hours"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>

            <span class="fright" style="padding-right: 2px;">
                <asp:Label ID="lblRightHeading" runat="server" Text=""></asp:Label>
            </span>
            <div style="float: left">
                <img src="/Content/IMAGES/expand-all.png" title="Expand All" onclick="expandAllTask(false)" style="cursor: pointer;" />
                <img onclick="collapseAllTask(false)" style="cursor: pointer;" src="/Content/IMAGES/collapse-all.png" title="Collapse All" />
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div id="exportPanel" runat="server" style="background: #fff; display: block; float: right; border: 0px inset;">
                <span class="fleft">
                    <dx:ASPxButton ID="btnExcelExport" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                        OnClick="btnExcelExport_Click">
                        <Image Url="/Content/IMAGES/excel-icon.png" />
                        <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                    </dx:ASPxButton>
                </span>
                <span class="fleft" style="padding-left: 3px;">
                    <dx:ASPxButton ID="btnPdfExport" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                        OnClick="btnPdfExport_Click">
                        <Image Url="/Content/IMAGES/pdf-icon.png" />
                        <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                    </dx:ASPxButton>
                </span>

                <span class="fleft" style="padding-left: 3px;">
                    <dx:ASPxButton ID="btnPrint" runat="server" RenderMode="Link" EnableTheming="false" UseSubmitBehavior="False"
                        OnClick="btnPrint_Click">
                        <Image Url="/Content/IMAGES/print-icon.png" />
                        <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                    </dx:ASPxButton>
                </span>
            </div>
        </td>
    </tr>


    <tr style="height: 90%">
        <td>
            <div>

                <dx:ASPxGridView ID="gvEstimatedRemaningHoursReport" runat="server" AutoGenerateColumns="false" OnHtmlRowPrepared="gvEstimatedRemaningHoursReport_HtmlRowPrepared"
                    Width="100%" KeyFieldName="ID" ClientInstanceName="gvEstimatedRemaningHoursReport"
                    SettingsText-EmptyDataRow="no record found." Theme="Aqua">
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="#" Width="10px" ></dx:GridViewDataTextColumn>
                        <%--<dx:GridViewDataTextColumn FieldName="Title" Caption="Tilte" PropertiesTextEdit-EncodeHtml="false" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>--%>
                        <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" Width="200px" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left">
                            <DataItemTemplate>
                                <div style="float: left; width: 100%; position: relative;">
                                    <%# GetTitleHtmlData()%>
                                </div>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="PercentComplete" Caption="% Comp" Width="50px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="AssignedToUser" Caption="Assigned To" Width="120px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="StartDate" Caption="Start Date" Width="80px" PropertiesTextEdit-DisplayFormatString="MMM-d-yyyy" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DueDate" Caption="Due Date" Width="80px" PropertiesTextEdit-DisplayFormatString="MMM-d-yyyy" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="EstimatedHours" Caption="Est. Hours" Width="50px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ActualHours" Caption="Act Hours" Width="50px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ActualVariance" Caption="Actual vs Estimate" Width="120px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="EstimatedRemainingHours" Caption="ERH" Width="40" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="Projected" Caption="Projected" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                         <dx:GridViewDataTextColumn FieldName="ProjectedEstimate" Caption="Projected vs Estimate" Width="120px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="PMMComment" Caption="Comment" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                    </Columns>
                    <Styles>
                        <Table CssClass="gvEstimatedRemaningHours-table"> </Table>
                        <Row CssClass="gvEstimatedRemaningHours-row"></Row>
                    </Styles>
                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                </dx:ASPxGridView>
            </div>
        </td>
    </tr>

    <tr style="height: 10%">
        <td style="text-align: right;">
            <div style="float: right">
                <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
            </div>
        </td>
    </tr>
</table>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskUserControlNew.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.TaskUserControlNew" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="System.Data" %>
<div class="row">
    <div class="col-md-3 noPadding">
        <div class="dashboard-panel-new">
            <div class="dashboard-panel-main mt-1 noPadding" style="width: 100%">
                <div class="task-title">My Projects</div>
                <div id="pie"></div>
            </div>
        </div>
    </div>
    <div class="col-md-9 noPadding">
        <div class="dashboard-panel-new pl-0">
            <div class="dashboard-panel-main mt-1 noPadding" style="width: 100%">
                <div class="task-title">Pending Tasks &nbsp;<img style="width:19px;cursor:pointer;" src="/content/images/plus-blue-new.png" title="New Task" onclick="javascript:event.cancelBubble=true; AddNewTask();"></div> 
                <div class="row scrollelement">
                    <div class="col-md-4">
                        <div class="task-title-sub">Not Started</div>
                        <%if (NotStartedTask != null) { 
                                foreach (var data in NotStartedTask)
                                {
                        %>

                        <div class="dashboard-panel-new pt-0 pr-0" title="<%=data.ToolTip%>">
                            <div class="dashboard-panel-main bg-color mt-1 border-left p-2" <%="style=border-color:" + data.Color%>>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2"><%=data.TitleText%></div>
                                    <div class="statustext disp-inline"><a onclick="<%=data.EditLink%>">
                                        <img src="/content/images/editIcon-new.png" class="icon-style" style="width: 16px;" /></a></div>
                                    <div class="statustext disp-inline" style="float: right;"><a onclick="<%=data.DeleteLink %>">
                                        <img src="/content/images/deleteIcon-new.png" class="icon-style-1" style="width: 22px;" /></a></div>
                                </div>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2 pr-1"><%=data.DueDate%></div>
                                    <div class="statustext disp-inline pl-3"><%=data.AgeText %></div>
                                    <div class="statustext disp-inline pl-3 checkbox-style" title="Mark as Complete"><input type="checkbox" onclick="<%=data.MarkAsCompleteLink%>" /></div>
                                </div>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2 text-decoration"><%=data.TicketId %></div>
                                </div>
                            </div>
                        </div>
                        <%} }%>
                    </div>
                    <div class="col-md-4">
                        <div class="task-title-sub">OnGoing</div>
                        <%if (OnGoingTask != null)
                            {
                                foreach (var data in OnGoingTask)
                                { %>
                        <div class="dashboard-panel-new pt-0 pr-0" title="<%=data.ToolTip%>">
                            <div class="dashboard-panel-main bg-color mt-1 border-left p-2" <%="style=border-color:" + data.Color%>>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2"><%=data.TitleText%></div>
                                    <div class="statustext disp-inline"><a onclick="<%=data.EditLink%>">
                                        <img src="/content/images/editIcon-new.png" class="icon-style" style="width: 16px;" /></a></div>
                                    <div class="statustext disp-inline" style="float: right;"><a onclick="<%=data.DeleteLink %>">
                                        <img src="/content/images/deleteIcon-new.png" class="icon-style-1" style="width: 22px;" /></a></div>
                                </div>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2 pr-1"><%=data.DueDate%></div>
                                    <div class="statustext disp-inline pl-3"><%=data.AgeText %></div>
                                    <div class="statustext disp-inline pl-3 checkbox-style"><input type="checkbox" onclick="<%=data.MarkAsCompleteLink%>" /></div>
                                </div>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2 text-decoration"><%=data.TicketId %></div>
                                </div>
                            </div>
                        </div>
                        <%}
                            } %>
                    </div>
                    <div class="col-md-4">
                        <div class="task-title-sub">Done</div>
                        <%if (CompletedTask != null)
                            {
                                foreach (var data in CompletedTask)
                                { %>
                        <div class="dashboard-panel-new pt-0 pr-0" title="<%=data.ToolTip%>">
                            <div class="dashboard-panel-main bg-color mt-1 border-left p-2" <%="style=border-color:green"%>>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2"><%=data.TitleText%></div>
                                    <div class="statustext disp-inline"><a onclick="<%=data.EditLink%>">
                                        <img src="/content/images/editIcon-new.png" class="icon-style" style="width: 16px;" /></a></div>
                                    <div class="statustext disp-inline" style="float: right;"><a onclick="<%=data.DeleteLink %>">
                                        <img src="/content/images/deleteIcon-new.png" class="icon-style-1" style="width: 22px;" /></a></div>
                                </div>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2 pr-1"><%=data.DueDate %></div>
                                    <div class="statustext disp-inline pl-3"></div>
                                </div>
                                <div class="pt-1">
                                    <div class="statustext disp-inline pl-2 text-decoration"><%=data.TicketId%></div>
                                </div>
                            </div>
                        </div>
                        <%}
                            } %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #pie {
        height: 340px;
    }

    .disp-inline {
        display: inline;
    }

    .task-title {
        font-size: 18px;
        font-weight: 500;
        text-align: center;
        padding-top: 15px;
    }

    .task-title-sub {
        font-size: 14px;
        font-weight: 500;
        text-align: center;
    }

    .dashboard-panel-main {
        border-radius: 0px;
        width: 100%;
    }

    .bg-color {
        background: #f5f5f5;
    }

    .border-left {
        border-left: 5px solid;
    }

    .statustext {
        font-size: 14px;
    }

    .text-decoration {
        text-decoration: underline;
    }

    .dashboard-panel-new {
        border: none !important;
        padding-bottom: 15px;
    }

    .noPadding {
        padding: 0px;
    }

    .page-container {
        background: #ddd;
    }

    .icon-style {
        position: absolute;
        margin-left: 7px;
    }

    .icon-style-1 {
        position: absolute;
        margin-left: -22px;
    }

    div.scrollelement {
        margin: 4px, 4px;
        padding: 4px;
        height: 340px;
        overflow-x: hidden;
        overflow-y: hidden;
        text-align: justify;
    }

    div.scrollelement:hover {
        overflow-y: scroll;
    }

    .scrollelement::-webkit-scrollbar-track {
        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
        background-color: #F5F5F5;
    }

    .scrollelement::-webkit-scrollbar {
        width: 6px;
        background-color: #F5F5F5;
    }

    .scrollelement::-webkit-scrollbar-thumb {
        background-color: #000000;
    }
    .checkbox-style {
    float: right;
    margin-right: 5px;
    margin-top: 24px;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    let onGoingWorkCount = <%=this.UserOnGoingWorkCount%>;
    let opportunitiesCount = <%= this.UserOpportunitiesCount%>;
    let trackedWorkCount = <%=this.UserTrackedWorkCount%>;
    <%--let data = "<%=AllTasks%>";
    console.log(data);--%>
    const dataSource = [{
        region: 'On Going',
        val: onGoingWorkCount,
    }, {
        region: 'Opportunities',
        val: opportunitiesCount,
    }, {
        region: 'Tracked Work',
        val: trackedWorkCount,
    }];

    $(() => {
        $('#pie').dxPieChart({
            type: 'doughnut',
            palette: 'Soft Pastel',
            dataSource,
            legend: {
                horizontalAlignment: 'right',
                verticalAlignment: 'top',
                margin: 0,
                visible: false,
            },
            series: [{
                argumentField: 'region',
                label: {
                    visible: true,
                    backgroundColor: "#fff",
                    position: "outside",
                    connector: {
                        visible: false,
                    },
                    customizeText(arg) {
                        return `${arg.valueText} - ${arg.argument}`;
                    },
                    font: {
                        color: "black",
                        size: "13",
                        family: "'Roboto', sans-serif",
                        weight: "500"
                    }
                },
            }],
        });
    });

    function deleteTask(TicketId, TaskId, mode) {
        var postData = {};
        postData.TicketId = TicketId;
        postData.TaskId = TaskId;
        postData.mode = mode;

        if (confirm("Are you sure you want to delete task?")) {
            $.ajax({
                url: ugitConfig.apiBaseUrl + "/api/module/DeleteHomePageTask",
                method: "DELETE",
                data: { TaskKeys: postData, TicketPublicId: TicketId },
                success: function (data) {
                    location.reload();
                },
                error: function (error) { }
            });
        }
    }

    function doTaskComplete(ticketid, taskid, tasktype) {
        if (confirm("Are you sure you want to mark task as completed?")) {
            $.ajax({
                url: ugitConfig.apiBaseUrl + "/api/module/MarkTaskAsComplete",
                method: "POST",
                data: { TaskKeys: taskid, TicketPublicId: ticketid, TaskType: tasktype },
                success: function (data) {
                    location.reload();
                },
                error: function (error) { }
            });
        }
        else {
            $(".checkbox-style input[type=checkbox]").prop("checked", false);
        }
    }


    function AddNewTask() {
        window.parent.UgitOpenPopupDialog("/Layouts/uGovernIT/DelegateControl.aspx?control=AddNewTask", "", "Create New Task", 40, 40, false, escape(window.location.href));;
    }

    //$(".reportListTiles").dxTileView
    //    ({
    //        noDataText: "No cards available",
    //        dataSource: data,
    //        direction: "horizontal",
    //        baseItemHeight: 100,

    //        itemTemplate: function (itemData, itemIndex, itemElement) {
    //            var html = new Array();
    //            html.push("<div class='dashboard-panel-new pt-0 pr-0'>");
    //            html.push(`<div class="dashboard-panel-main mt-1 border-left p-2" style="width: 100%;background:#f5f5f5;">`);
    //            html.push(`<div class="pt-1">`);
    //            html.push(`<div class="statustext disp-inline pl-2">Plenty of Time</div>`);
    //            html.push(`<div class="statustext disp-inline"><img src="/content/images/editIcon-new.png" class="icon-style" style="width:40px;" /></div>`);
    //            html.push(`<div class="statustext disp-inline" style="float: right;"><img src="/content/images/deleteIcon-new.png" class="icon-style-1" style="width:30px;" /></div>`);
    //            html.push(`</div>`);
    //            html.push(`<div class="pt-1">`);
    //            html.push(`<div class="statustext disp-inline pl-2 pr-1">`);
    //            html.push(itemData.DueDate);
    //            html.push('</div>');
    //            html.push(`<div class="statustext disp-inline pl-3">`);
    //            html.push(itemData.Duration);
    //            html.push(`</div>`);
    //            html.push(`</div>`);
    //            html.push(`<div class="pt-1">`);
    //            html.push(`<div class="statustext disp-inline pl-2 text-decoration">`);
    //            html.push(itemData.TicketId);
    //            html.push(`</div>`);
    //            html.push(`</div >`);
    //            html.push(`</div>`);
    //            html.push(`</div>`);


    //            //html.push("<div class='d-flex'>");
    //            //html.push(itemData.AgeText);
    //            //html.push(itemData.DueDate);


    //            //html.push("</div>");

    //            itemElement.append(html.join(""));
    //        },
    //        onContentReady: function (obj) {
    //        },
    //        onItemClick: function (obj) {
    //            //if (obj != null && obj.itemData != null) {
    //            //    if (obj.itemData.LongTitle != null)
    //            //        ShowReportViewer(obj.itemData.ModuleNameLookup, obj.itemData.Name, obj.itemData.LongTitle, obj.itemData.RouteUrl);
    //            //    else
    //            //        ShowReportViewer(obj.itemData.ModuleNameLookup, obj.itemData.Name, obj.itemData.Title, obj.itemData.RouteUrl);
    //            //}
    //        }
    //    }).dxTileView('instance');

</script>


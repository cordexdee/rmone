<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskWorkflow.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.PMM.TaskWorkflow" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxdi-canvas .shape ellipse, .dxdi-canvas .shape text {
        cursor: pointer !important;
    }
</style>
<script type="text/javascript" id="dxss_TaskWorkflowScripts" data-v="<%=UGITUtility.AssemblyVersion %>">
    var dataItems = [];
    var dataLinks = [];
    var defaultNode = [{
        "id": "1",
        "text": "No root tasks are found to draw the task workflow.",
        "type": "defaultnode",
        "status": ""
    }];

    $(function () {
        RepaintTaskWorkflow();
    });

    function RepaintTaskWorkflow() {

        $.getJSON(ugitConfig.apiBaseUrl + "/api/pmmapi/GetTaskWorkflowData?ticketId=" + "<%=TicketID %>" + "&moduleName=" + "<%= ModuleName %>").done(function (data) {
            if (data != null) 
                DrawWorkflow(data);
            else 
                DrawNoDataDiagram();

        }).fail(function (error) {
            DrawNoDataDiagram();
        });
    }

    function DrawWorkflow(data) {
        dataItems = data.Nodes;
        var index;
        for (index = 0; index < data.Links.length; index++) {
            data.Links[index].from = data.Links[index]['fromId'];
            delete data.Links[index].fromId;
        }
        dataLinks = data.Links;

        if (dataItems != undefined && dataItems != null && dataItems.length > 0 && dataLinks != undefined && dataLinks != null) {

            $("#taskDiagram").dxDiagram({
                customShapes: [{
                    category: "task",
                    type: "roottask",
                    title: "Root Task",
                    defaultWidth: 0.5,
                    defaultHeight: 0.5,
                    baseType: "ellipse",
                    defaultText: "task",
                    allowEditText: true,
                    textLeft: -0.2,
                    textTop: 0.9,
                    textWidth: 1.6,
                    textHeight: 1,

                    connectionPoints: [
                        { x: 0.5, y: 0 },
                        { x: 1, y: 0.5 },
                        { x: 0.5, y: 1 },
                        { x: 0, y: 0.5 }
                    ]
                }],
                nodes: {
                    dataSource: new DevExpress.data.ArrayStore({
                        key: "this",
                        data: dataItems
                    }),
                    autoLayout: {
                        orientation: "horizontal"
                    },
                    styleExpr: itemStyleExpr
                },
                edges: {
                    dataSource: new DevExpress.data.ArrayStore({
                        key: "this",
                        data: dataLinks
                    })
                },
                toolbox: {
                    visible: false,
                    groups: ["general"]
                },
                toolbar: {
                    visible: false
                },
                zoomLevel: { value: 1 },
                contextMenu: { enabled: false },
                height: 200,
                simpleView: true,
                width: function () { return $(window.document).width() - 17; },
                showGrid: false,
                readOnly: true,
                onItemClick: nodeItemClick
            });
        }
        else {
            DrawNoDataDiagram();
        }
    }

    function DrawNoDataDiagram() {

        $("#taskDiagram").dxDiagram({
            customShapes: [{
                category: "task",
                type: "defaultnode",
                title: "Default Node",
                defaultWidth: 3,
                defaultHeight: 1,
                baseType: "rectangle",
                defaultText: "No task found.",
                allowEditText: false,
            }],
            nodes: {
                dataSource: new DevExpress.data.ArrayStore({
                    key: "this",
                    data: defaultNode
                }),
                autoLayout: {
                    orientation: "horizontal"
                },
                styleExpr: itemStyleExpr
            },
            toolbox: {
                visible: false,
                groups: ["general"]
            },
            toolbar: {
                visible: false
            },
            zoomLevel: { value: 1 },
            contextMenu: { enabled: false },
            height: 200,
            simpleView: true,
            width: function () { return $(window.document).width() - 30; },
            showGrid: false,
            readOnly: true
        });
    }

    function itemStyleExpr(obj) {
        let style = { "stroke": "#444444" };

        if (obj.status.toLowerCase() == "in progress")
            style["fill"] = "#ffff00";
        else if (obj.status.toLowerCase() == "completed")
            style["fill"] = "#00c600";
        else if (obj.type == "defaultnode")
            style["fill"] = "#f6f7fb";

        return style;
    }

    function nodeItemClick(obj) {

        if (obj != null && obj.item != null && obj.item.dataItem != null && obj.item.dataItem.title != undefined) {
            var nodeTooltip = '<div class=divText><span><b>Title:</b> ' + obj.item.dataItem.title + '</span><br /><span><b>% Complete:</b> '
                + obj.item.dataItem.pctComplete + '%</span><br /><span><b>Status:</b> ' + obj.item.dataItem.status + '</span><br /><span><b>Due Date:</b> '
                + new Date(obj.item.dataItem.dueDate).format("MMM d, yyyy") + '</span></div>';

            var targateElement = $(event.srcElement).parent();

            if (targateElement[0].tagName == "text")
                targateElement = $(event.srcElement).parent().parent();

            ShowPopoverTooltip(targateElement, nodeTooltip);
        }
    }

    function ShowPopoverTooltip(targateElement, nodeTooltip) {
        $("#popoverTooltip").dxPopover({
            target: targateElement,
            contentTemplate: nodeTooltip,
            showEvent: "dxclick",
            hideEvent: "mouseleave",
            width: 300,
            position: "top",
            animation: {
                show: {
                    type: "pop",
                    from: { scale: 0 },
                    to: { scale: 1 }
                },
                hide: {
                    type: "fade",
                    from: 1,
                    to: 0
                }
            }
        });
    }
</script>

<div id="taskDiagram"></div>
<div id="popoverTooltip"></div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceTaskWorkFlow.ascx.cs" Inherits="uGovernIT.Web.ServiceTaskWorkFlow" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .shape-expand-btn {
        display: none !important;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">



    RepaintTaskWorkflow();
    function RepaintTaskWorkflow() {

        $.ajax({

            url: "/api/svctaskapi/GetServiceTaskFlowData?TicketID=" + "<%=TicketId %>" + "&ModuleName=" + "<%= ModuleName %>",
            type: "GET",
            contentType: "application/json",
            success: function (data) {

                console.log(data)
                if (data != null)
                    DrawWorkflow(data);
                else
                    DrawNoDataDiagram();
            }

        });


    }

    function DrawWorkflow(data) {


        dataItems = data.Nodes;
        dataLinks = data.Edges;
        //if (dataItems != undefined && dataItems != null && dataItems.length > 0 && dataLinks != undefined && dataLinks != null) {

        $("#ServiceTaskDiagram").dxDiagram({

            zoomLevel: 0.9,
            customShapes: [{
                category: "task",
                type: "roottask",
                title: "Root Task",
                defaultWidth: 0.4,
                defaultHeight: 0.4,
                //baseType: "ellipse",
                defaultText: "task",
                allowEditText: true,
                textLeft: -1,
                textTop: -3,
                textWidth: 2,
                textHeight: 4,
                //picture: "/Content/Images/new_task.png",
                backgroundImageUrl: "/Content/Images/workflow-support.png",
                //backgroundImageUrl: "~/Content/Images/new_task.png",
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
                    data: dataItems,


                }),
                widthExpr: 'width',
                heightExpr: 'height',
                leftExpr: 'leftX',
                topExpr: 'topY',
                childrenExpr: null,
                containerKeyExpr: "ContainerId",
                styleExpr: itemStyleExpr,
                textStyleExpr: itemTextStyleExpr,
                autoLayout: {
                    type: 'off',
                    orientation: 'horizontal'
                }


            },
            edges: {
                dataSource: new DevExpress.data.ArrayStore({
                    key: "this",
                    data: dataLinks,

                }),
                fromLineEndExpr: linkFromLineEndExpr,
                toLineEndExpr: linkToLineEndExpr,
                styleExpr: linkStyleExpr,
                //fromPointIndexExpr: 'fromShapePoint',
                lineTypeExpr: 'lineTypeExpr'

            },
            toolbox: {
                visible: false
            },
            toolbar: {
                visible: false
            },
            showGrid: false,
            readOnly: true,
            simpleView: true,
            width: function () { return $(window.document).width() - 5; },
            height: function () { return $(window.document).height() - 5; },
            onItemClick: nodeItemClick
        });

    }


    function lineTypeExpr() {
        //console.log("jjjj");
        //return "Orthogonal";
    }

    function linkFromLineEndExpr(obj) { return "none"; }
    function linkToLineEndExpr(obj) { return "none"; }

    function linkStyleExpr(obj) {
        return { "fill": "#a0a3a2", "stroke": "#a0a3a2", "stroke-width": "1px" };
    }
    function itemTextStyleExpr(obj) {
        if (obj.status != null) {
            return { "fill": "#000" };
        } else {
            return { "fill": "#000" };
        }
    }



    function itemStyleExpr(obj) {

        let style = { "stroke": "" };
        if (obj.type === "verticalContainer") {
            return { "stroke-dasharray": "-1px", "stroke": "#fff" };

        }
        if (obj.status != null) {

            //if (obj.status.toLowerCase() == "in progress") {
            //    return { "fill": "rgba(63, 38, 146, 0.82)", "stroke": "rgb(73, 31, 209)" };   
            //}
            //else if (obj.status.toLowerCase() == "completed") {
            //    return { "fill": "#11916a", "stroke": "#11916a" };
            //}
            //else if (obj.status.toLowerCase() == "waiting" || obj.status.toLowerCase() == "manager approval"  || obj.status.toLowerCase() == "on hold" )
            //    return { "fill": "#f69d17" ,"stroke": "#f69d17" };

            if (obj.status.toLowerCase() == "in progress") {
                return { "fill": "#A7BEFF", "stroke": "#fff" };
            }
            else if (obj.status.toLowerCase() == "completed" || obj.status.toLowerCase() == "close") {
                return { "fill": "#C5FFD0", "stroke": "#fff" };
            }
            else if (obj.status.toLowerCase() == "waiting" || obj.status.toLowerCase() == 'initiated' || obj.status.toLowerCase() == "not started") {
                return { "fill": "#E3E3E3", "stroke": "#fff" };
            }
            else {
                return { "fill": "#A7BEFF", "stroke": "#fff" };
            }

        }
        return style;
    }

    function nodeItemClick(obj) {
        if (obj != null && obj.item != null && obj.item.dataItem != null) {
            if (obj.item.dataItem.taskType === "task") {
                var width = "90";
                var height = "90";
                var stopRefresh = "0";
                var returnUrl = "Edit Ticket";
                var editTaskUrl = '<%=editTaskUrl %>';
                var moduleName = '<%=ModuleName  %>';
                var ticketId = '<%=TicketId %>';
                var relatedTitle = "Edit Task";
                var params = "ID=" + obj.item.dataItem.id + "&ItemOrder=" + moduleName + "&control=taskedit&moduleName=" + moduleName + "&ticketId=" + ticketId + "&taskID=" + obj.item.dataItem.id + "&taskType=" + obj.item.dataItem.taskType;
                window.parent.UgitOpenPopupDialog(editTaskUrl, params, relatedTitle, width, height, stopRefresh, returnUrl);
            }
            else if (obj.item.dataItem.taskType === "Ticket") {
                var relatedTitle = obj.item.dataItem.text;
                window.parent.UgitOpenPopupDialog(obj.item.dataItem.onClickString, '', (relatedTitle), "90", "90", "0")
            }
        }
    }
</script>



<div class="col-md-12 col-sm-12 col-xs-12 workFlowLegend-container statusLegend-Container">
    <div class="legendNormal-wrap">
        <div class="legend-complete legendIcon"></div>
        <div class="legendLabel">Completed</div>
    </div>
    <div class="legendHeigh-wrap">
        <div class="legend-waiting legendIcon"></div>
        <div class="legendLabel">Waiting</div>
    </div>
    <div class="legendCritical-wrap">
        <div class="legend-inprogess legendIcon"></div>
        <div class="legendLabel">In Progress</div>
    </div>
</div>
<div id="ServiceTaskDiagram"></div>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        /*$('#ServiceTaskDiagram g.shape rect').attr('rx', '10');*/
        setTimeout(function () {
            $('#ServiceTaskDiagram g.shape rect').attr('rx', '10');
        }, 100);
    });
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleStagesView.ascx.cs" Inherits="uGovernIT.Web.ModuleStagesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .deleteStep {
        position: absolute;
        top: 0px;
        left: 8px;
    }
</style>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .StaticMenuStyle a {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }
    /*.StaticMenuStyle a:hover {
    color:black;       
    background-color:white;
    }*/
    .aa {
        height: 20px;
    }

    a, img {
        border: 0px;
    }

    .fullwidth {
        width: 100%;
        float: left;
    }

    .lifecycle-pane {
        float: left;
        width: 200px;
        margin-left:2px;
    }

    .lifecyclestage-pane {
        /*float: left;
        width: 100%;*/
        margin-left:2px;
        position:relative;
    }

    .lcsgraphics {
        float: left;
        width: 100%;
        height: 150px;
        text-align: center;
        padding-top: 40px;
        margin-bottom:10px;
    }

    .lcsDetail {
        padding-top: 21px;
        padding-bottom:5px;
        float: left;
        width: 100%;
    }

    .btlifecycle-header {
        background: none repeat scroll 0 0 #E8E6E5;
        float: left;
        font-weight: bold;
        padding: 6px 6px 6px 2px;
        width: 96%;
    }

    .btlifecycle-link {
        background: #F7FAFA;
        float: left;
        padding: 6px 6px 6px 2px;
        width: 96%;
    }

    .btlc-div {
        float: left;
        width: 100%;
        border-top: 1px solid;
    }

    .lc-td {
        padding-right: 2px;
        border-right: 2px solid #BFBCBC;
    }

    .ms-viewheadertr .ms-vh2-gridview {
        width: 25px !important;
    }

    .action-align {
        float: right;
        padding: 3px 3px 0px 0px;
    }

    .action-align-del {
        float: left;
        padding: 3px 0px 0px 0px;
    }

    .message-box {
        font-weight: bold;
        text-align: center;
    }

    .arrow-down-red {
        position: absolute;
        top: -6px;
        left: 100px;
        width: 0;
        height: 0;
        border-top: 10px solid red;
        border-left: 5px solid transparent;
        border-right: 5px solid transparent;
    }

    .arrow-down-green {
        position: absolute;
        top: -6px;
        left: 100px;
        width: 0;
        height: 0;
        border-top: 10px solid green;
        border-left: 5px solid transparent;
        border-right: 5px solid transparent;
    }

    .arrow-down-yellow {
        position: absolute;
        top: -6px;
        left: 100px;
        width: 0;
        height: 0;
        border-bottom: 10px solid blue;
        border-left: 5px solid transparent;
        border-right: 5px solid transparent;
    }


    .arrow-down-connector-red {
        width: 0;
        height: 0;
        border-top: 10px solid red;
        border-left: 5px solid transparent;
        border-right: 5px solid transparent;
        /*float:left;*/
    }

    .arrow-down-connector-green {
        width: 0;
        height: 0;
        border-top: 10px solid green;
        border-left: 5px solid transparent;
        border-right: 5px solid transparent;
        /*float:left;*/
    }

    .arrow-down-connector-yellow {
        width: 0;
        height: 0;
        border-top: 10px solid blue;
        border-left: 5px solid transparent;
        border-right: 5px solid transparent;
        /*float:left;*/
    }

    .arrow-right-red {
        width: 0;
        height: 0;
        border-left: 10px solid red;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }

    .arrow-right-green {
        width: 0;
        height: 0;
        border-left: 10px solid green;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }

    .arrow-right-yellow {
        width: 0;
        height: 0;
        border-left: 10px solid blue;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }


    .arrow-left-red {
        width: 0;
        height: 0;
        border-right: 10px solid red;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }

    .arrow-left-green {
        width: 0;
        height: 0;
        border-right: 10px solid green;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }

    .arrow-left-yellow {
        width: 0;
        height: 0;
        border-right: 10px solid blue;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }
    .btUpdateTickets {
        margin-left:5px;
    }
    .btnApplyChanges {
        margin-left: 5px;
    }

    .contract_steps_module {
        width: 94%;
        float: left;
        margin: 0 0 10px 0;
        padding: 20px 40px 0px !important;
    }
        .contract_steps_module tt {
            width: 100%;
            float: left;
            font-style: normal;
            text-align: center;
            padding: 0 0 10px;
        }
    .contract_steps_container,
    .contract_steptop_content {
        width: 100%;
        float: left;
        height: 73px;
        margin-top: 5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var keyTracker;
    function InitalizejQuery() {
        $('.draggable').draggable({
            helper: 'clone',
            opacity: 1,
            revert: "invalid",
            scroll: true,
            cursorAt: { top: 7, left: 40 },
            drag: function (event, ui) {
                if (ui.originalPosition.top != window.FindPosition(this).top) {
                    ui.position.top = event.clientY - 7 + ui.originalPosition.top - window.FindPosition(this).top;
                }
            }
        });


        $('.droppable').droppable({
            activeClass: "hover",
            drop: function (event, ui) {
                if ($(ui.draggable).attr('id').indexOf('_tdStage') > -1) {
                    var draggingRowKey = $(ui.draggable).find("span").text().trim();
                    var droppedRowKey;
                    if (draggingRowKey != undefined) {

                        if ($(this).attr('id').indexOf('tdStepLine') > -1) {
                            droppedRowKey = $(this).prev().find("span").text().trim();
                            if (draggingRowKey > droppedRowKey) {
                                droppedRowKey = $(this).next().find("span").text().trim();
                            }
                        }
                        else {
                            droppedRowKey = $(ui.draggable).find("span").text().trim();
                        }
                        if (droppedRowKey != undefined) {
                            CallbackPanel.PerformCallback("DRAGROW|" + draggingRowKey + '|' + droppedRowKey);
                        }
                    }
                }
                else {

                    var droppedRowKey = $(this).find("span").text().trim();
                    if (droppedRowKey != undefined) {
                        CallbackPanel.PerformCallback("CONNECT|" + $(ui.draggable).attr('type') + '|' + droppedRowKey + '|' + $(ui.draggable).attr('stage'));
                    }
                }
            },
            accept: function (el) {

                if ($(this).attr('id').indexOf('_tdStepLine') > -1 && $(el).attr('id').indexOf('_tdStage') > -1) {
                    return true;
                }
                else if ($(this).attr('id').indexOf('_tdStage') > -1 && $(el).attr('id').indexOf('_connection') > -1) {
                    return true;
                }
                else if ($(this).attr('id').indexOf('_tdStepLine') > -1 && $(el).attr('id').indexOf('_connection') > -1) {
                    if (!keyTracker) {
                        $(el).parent().find('.arrow-right-connector-green').hide();
                        $(el).parent().find('.arrow-right-connector-red').hide();
                        $(el).parent().find('.arrow-right-connector-yellow').hide();
                    }
                    return false;
                }
                return false;
            }
        });
    }

    function OnEndCallback(s, e) {
        drawGraphicArrow();
        showConnectionCreator();
    }
    function checkBeforeDelete() {
        <% if (lifeCycleInUse)
    {%>
        alert("You cannot delete this Lifecycle because it is being used in some projects.");
        return false;
        <% }
    else
    {%>
        return confirm('Are you sure you want to delete selected lifecycle?');
        <% } %>
        return false;
    }
    $(document).ready(function () {
        drawGraphicArrow();
        showConnectionCreator();
    });

    function showConnectionCreator() {
        $(".lcsgraphics").find('[id$="_tdStage"]').each(function (i, item) {
            $(item).hover(function () {
                var deleteImage = '<div style="position:relative; width:100%; top:-22px;">';
                if ($(item).find('.deleteStep').length == 0) {
                    deleteImage += '<div id="dv_deleteStep" type="delete"  stage=' + $(item).find("span").text().trim() +
                        ' class="deleteStep" onclick="deleteStepItem(' + $(item).find("span").text().trim() + ');">' +
                        '<img src="/Content/ButtonImages/cancel.png" style="border:none;" title="Delete Step" alt=""></div>';
                }
                else {
                    $(item).find('[id$="_deleteStep"]').show();
                }
                deleteImage += '</div>';

                if ($(item).find('.deleteStep').length == 0) {
                    $(item).append(deleteImage)
                }


                var left = 0;
                var connectors = '<div style="position:relative; height:10px; width:100%;top:38px;">';

                //Approve arrow
                if (i + 1 != $(".lcsgraphics").find('[id$="_tdStage"]').length) {
                    if ($(item).find('.arrow-down-connector-green').length == 0) {
                        connectors += '<div id="dv_green_connection" type="approve"  stage=' + $(item).find("span").text().trim() +
                            ' class="arrow-down-connector-green draggable" style="position: absolute;top:0px;left:' + left + 'px;"></div>';
                        left += 13;
                    }
                    else {
                        $(item).find('[id$="_green_connection"]').show();
                    }
                }

                //Return arrow
                if (i != 0) {
                    if ($(item).find('.arrow-down-connector-yellow').length == 0) {
                        connectors += '<div id="dv_yellow_connection" type="return"  stage=' + $(item).find("span").text().trim() +
                            ' class="arrow-down-connector-yellow draggable" style="position: absolute;top:0px;left:' + left + 'px;"></div>';
                        left += 13;
                    }
                    else {
                        $(item).find('[id$="_yellow_connection"]').show();
                    }
                }

                //Reject arrow
                if ($(item).find('.arrow-down-connector-red').length == 0) {
                    connectors += '<div id="dv_red_connection" type="reject" stage=' + $(item).find("span").text().trim() +
                        ' class="arrow-down-connector-red draggable" style="position: absolute;top:0px;left:' + left + 'px;"></div>';
                    left += 13;
                }
                else {
                    $(item).find('[id$="_red_connection"]').show();
                }

                connectors += '</div>';

                if ($(item).find('.arrow-down-connector-red').length == 0) {
                    $(item).append(connectors)
                }

                $(item).find('.draggable').draggable({
                    helper: 'clone',
                    opacity: 1,
                    revert: "invalid",
                    scroll: true,
                    cursorAt: { top: 0, left: 0 },
                    start: function (event, ui) {

                    },
                    drag: function (event, ui) {

                        if ($(".lcsgraphics").find('#dvDrag').length == 0) {
                            var color = 'color:green';
                            if ($(ui.helper).attr('type') == 'reject') {
                                color = 'color:red';
                            }
                            else if ($(ui.helper).attr('type') == 'return') {
                                color = 'color:blue';
                            }
                            $(".lcsgraphics").append('<div id="dvDrag" h=' + event.clientY + ' w=' + event.clientX +
                                ' style="position:absolute;top:' + event.clientY + 'px;left:' + event.clientX +
                                'px;height:20px;width:60px;border:2px solid;' + color + '"></div>'); //border-radius: 35px/35px;
                        }
                        if ($(".lcsgraphics").find('#dvDrag').length > 0) {

                            var left = $(".lcsgraphics").find('#dvDrag').attr('w');
                            var top = $(".lcsgraphics").find('#dvDrag').attr('h');

                            var vDiff = (event.clientY - top);
                            var hDiff = (event.clientX - left);

                            var vOffset = top + parseInt(vDiff);
                            if (vDiff < 0 && hDiff < 0) {
                                $(".lcsgraphics").find('#dvDrag').css('top', (parseInt(top) + parseInt(vDiff)) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('left', (parseInt(left) + parseInt(hDiff)) + 'px');

                                $(".lcsgraphics").find('#dvDrag').css('height', (parseInt(top) - event.clientY) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('width', (parseInt(left) - event.clientX) + 'px');

                                $(".lcsgraphics").find('#dvDrag').css('border-top', '1px solid');
                                $(".lcsgraphics").find('#dvDrag').css('border-bottom', 'none');
                                $(".lcsgraphics").find('#dvDrag').css('border-left', 'none');
                                $(".lcsgraphics").find('#dvDrag').css('border-right', '1px solid');
                            }
                            else if (vDiff < 0) {
                                $(".lcsgraphics").find('#dvDrag').css('top', (parseInt(top) + parseInt(vDiff)) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('height', (parseInt(top) - event.clientY) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('width', (event.clientX - left) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('left', left + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('border-top', '1px solid');
                                $(".lcsgraphics").find('#dvDrag').css('border-bottom', 'none');
                                $(".lcsgraphics").find('#dvDrag').css('border-left', '1px solid');
                                $(".lcsgraphics").find('#dvDrag').css('border-right', 'none');
                            }
                            else if (hDiff < 0) {
                                $(".lcsgraphics").find('#dvDrag').css('left', (parseInt(left) + parseInt(hDiff)) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('height', (event.clientY - parseInt(top)) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('width', (parseInt(left) - event.clientX) + 'px');

                                $(".lcsgraphics").find('#dvDrag').css('border-top', 'none');
                                $(".lcsgraphics").find('#dvDrag').css('border-bottom', '1px solid');
                                $(".lcsgraphics").find('#dvDrag').css('border-left', 'none');
                                $(".lcsgraphics").find('#dvDrag').css('border-right', '1px solid');
                            }
                            else {
                                $(".lcsgraphics").find('#dvDrag').css('top', top + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('left', left + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('height', (event.clientY - top) + 'px');
                                $(".lcsgraphics").find('#dvDrag').css('width', (event.clientX - left) + 'px');

                                $(".lcsgraphics").find('#dvDrag').css('border-top', 'none');
                                $(".lcsgraphics").find('#dvDrag').css('border-bottom', '1px solid');
                                $(".lcsgraphics").find('#dvDrag').css('border-left', '1px solid');
                                $(".lcsgraphics").find('#dvDrag').css('border-right', 'none');
                            }

                            // This is to test calculations
                            //$(".lcsgraphics").find('#dvDrag').html('left:' + $(".lcsgraphics").find('#dvDrag').css('left') +
                            //                                       ' o left:' + left +
                            //                                       ' X:' + event.clientX +
                            //                                       ' width:' + $(".lcsgraphics").find('#dvDrag').width()
                            //    );
                        }

                        if (!keyTracker) {
                            keyTracker = true;
                        }

                        if (ui.originalPosition.top != window.FindPosition(this).top) {
                            ui.position.top = event.clientY - 7 + ui.originalPosition.top - window.FindPosition(this).top;
                        }
                    },
                    stop: function (event, ui) {
                        // $(ui.helper).css('display', 'none');
                        $(".lcsgraphics").find('#dvDrag').remove();
                        keyTracker = false;
                    }
                });


            }, function () {
                if (!keyTracker) {
                    $(item).find('.arrow-down-connector-green').hide();
                    $(item).find('.arrow-down-connector-red').hide();
                    $(item).find('.arrow-down-connector-yellow').hide();
                    $(item).find('.deleteStep').hide();
                }
            });
        });
    }

    function drawGraphicArrow() {
        var returnOffset = 0;
        var rejectOffset =0;
        var approveOffset = 0;
        var leftMargin = 40;
        var stageStdWidth = 37.5;

        var returnTopOffset =2;
        var rejectTopOffset = 14;
        var approveTopOffset = 2;

        var approveDLineOffset = 0;
        var returnDLineOffset =0;
        var margindiff = 0;
        $(".lcsgraphics").find('[id$="_tdStage"]').each(function (i, item) {
            if (margindiff == 0) {
                margindiff = $(item).closest('table').parent().width() - $(item).closest('table').width();
            }

            var left = $(item).position().left + $(item).width() + leftMargin +margindiff/2 ;
            
         
            var aStage = $(item).attr('aStage');
            var rStage = $(item).attr('rStage');
            var rjStage = $(item).attr('rjStage');

            if (aStage > 0) {
                var stage = getStageByNo(aStage);
                var stepDiff = Math.abs($(stage).find("span").text().trim() - $(item).find("span").text().trim());

                if (parseInt($(stage).find("span").text().trim()) > parseInt($(item).find("span").text().trim())) {
                    var width = ($(stage).position().left + $(stage).width()) - left;

                    var arrowHtml = '<div style="margin-top: ' + approveTopOffset + 'px; margin-left: ' + approveOffset +
                        'px; border:1px solid; height:30px; width: ' + (width + approveOffset + 40) + 'px;border-bottom: None;position: absolute;top: 2px;left: ' +
                        (left - 20) + 'px;color: green;"><div class="arrow-down-green" style="top:30px;left:' + (width + approveOffset - 4 + 40) + 'px !important;"></div></div>';

                    if (stepDiff == 1) {
                        arrowHtml = '<div style="margin-top: 10px; margin-left: ' + approveOffset + 'px; border:1px solid; height:0px; width: ' +
                            (width + approveOffset - approveDLineOffset) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 73px;left: ' +
                            (left + approveDLineOffset) + 'px;color: green;"><div class="arrow-right-green" style="top:2px;left:' + (width + approveOffset - 4) +
                            'px !important;margin-top: -5px;"></div></div>';
                    }

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                else {
                    left = $(stage).position().left + $(stage).width() + leftMargin;
                    var width = ($(item).position().left + $(item).width()) - left;

                    var arrowHtml = '<div style="margin-top: ' + approveTopOffset + 'px;margin-left: ' + approveOffset + 'px;border:1px solid;height:30px;width: ' +
                        (width + approveOffset) + 'px;border-bottom: None;position: absolute;top: 2px;left: ' + left +
                        'px;color: green;"><div class="arrow-down-green" style="top:30px;left:-6px !important;"></div></div>';

                    if (stepDiff == 1) {
                        arrowHtml = '<div style="margin-top: 10px;margin-left: ' + approveOffset + 'px;border:1px solid;height:0px;width: ' +
                            (width + approveOffset - approveDLineOffset) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 64px;left: ' +
                            (left + approveDLineOffset) +
                            'px;color: green;"><div class="arrow-left-green" style="top:2px;left:-6px !important;margin-top: -5px;float:left;"></div></div>';
                    }

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }

                approveTopOffset += 4;
            }

            if (rStage > 0) {
                var stage = getStageByNo(rStage);
                var stepDiff = Math.abs($(stage).find("span").text().trim() - $(item).find("span").text().trim());
                if (parseInt($(stage).find("span").text().trim()) > parseInt($(item).find("span").text().trim())) {

                    left = $(item).position().left + $(stage).width()+ leftMargin + margindiff/2 - returnOffset;
                    var width = (($(stage).position().left + $(stage).width()) - left - returnOffset);
                    var arrowHtml = '<div style="margin-top: ' + returnTopOffset + 'px;margin-left: ' + (returnOffset) +
                        'px;border:1px solid;height:30px;width: ' + (width - (returnOffset)) + 'px;border-top: None;position: absolute;top: 116px;left: ' +
                        left + 'px;color: blue;"><div class="arrow-down-yellow" style="top:-4px;left:' + (width - 5 - (returnOffset)) + 'px !important;"></div></div>';

                    if (stepDiff == 1) {
                        arrowHtml = '<div style="margin-top: 14px;margin-left: ' + (returnOffset) + 'px;border:1px solid;height:0px;width: ' +
                            (width - (returnOffset) - returnDLineOffset) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 93px;left: ' +
                            (left + (returnDLineOffset - 5)) + 'px;color: blue;"><div class="arrow-right-yellow" style="top:0px;left:' + (width - 5 - (returnOffset)) +
                            'px !important;margin-top: -6px;"></div></div>';
                    }

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                else {
                    left = $(stage).position().left + $(item).width() +leftMargin + margindiff/2 - returnOffset - 20;
                    var width = (($(item).position().left + $(item).width()) - left - returnOffset + 20);

                    var arrowHtml = '<div style="margin-top: ' + returnTopOffset + 'px;margin-left: ' + (returnOffset) +
                        'px;border:1px solid;height:30px;width: ' + (width - (returnOffset)) + 'px;border-top: None;position: absolute;top: 116px;left: ' + left +
                        'px;color: blue;"><div class="arrow-down-yellow" style="top:-4px;left:-6px !important;"></div></div>';

                    if (stepDiff == 1) {
                        var arrowHtml = '<div style="margin-top: 14px;margin-left: ' + (returnOffset) + 'px;border:1px solid;height:0px;width: ' +
                            (width - (returnOffset) - returnDLineOffset - 40) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 85px;left: ' +
                            (left + (returnDLineOffset - 5 + 20)) +
                            'px;color: blue;"><div class="arrow-left-yellow" style="top:-4px;left:-6px !important;margin-top: -6px;float:left;"></div></div>';
                    }

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                returnTopOffset += 4;
            }

            if (rjStage > 0) {
                var stage = getStageByNo(rjStage);
                if (parseInt($(stage).find("span").text().trim()) > parseInt($(item).find("span").text().trim())) {
                    left = $(item).position().left + leftMargin + stageStdWidth + margindiff/2 - $(item).width() / 2 - 2;
                    var width = ($(stage).position().left + $(stage).width() / 2 +leftMargin+margindiff/2) - left - 2;

                    var arrowHtml = '<div style="margin-top: ' + rejectTopOffset + 'px;margin-left: ' + rejectOffset +
                        'px;border:1px solid;height:30px;width: ' + (width - rejectOffset) + 'px;border-bottom: None;position: absolute;top: 3px;left: ' + left +
                        'px;color: red;"><div class="arrow-down-red" style="top:30px;left:' + (width - rejectOffset - 7) + 'px !important;"></div></div>';

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                else {
                    left = $(stage).position().left+leftMargin + stageStdWidth + margindiff/2 - $(stage).width() / 2 - 2;
                    var width = ($(item).position().left + $(item).width() / 2+leftMargin+margindiff/2) - left - 2;

                    var arrowHtml = '<div style="margin-top: ' + rejectTopOffset + 'px;margin-left: ' + rejectOffset +
                        'px;border:1px solid;height:30px;width: ' + (width - rejectOffset) + 'px;border-bottom: None;position: absolute;top: 3px;left: ' +
                        left + 'px;color: red;"><div class="arrow-down-red" style="top:30px;left:-6px !important;"></div></div>';

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                rejectTopOffset += 4;
            }

        })
    }

    function getStageByNo(stageNo) {
        for (var i = 0; i < $(".lcsgraphics").find('[id$="_tdStage"]').length; i++) {
            if (stageNo == $($(".lcsgraphics").find('[id$="_tdStage"]').get(i)).find("span").text().trim()) {
                return $(".lcsgraphics").find('[id$="_tdStage"]').get(i);
            }
        }
    }

    function deleteStepItem(stage) {
        if (confirm('Are you sure you want to delete Step ' + stage + ' from workflow?')) {
            //alert("Yes");
            CallbackPanel.PerformCallback("DELETE|" + stage);
        }
    }

    function ShowLifeCyclePopup(s, e) {
        popupLifeCycle.Show();
    }

    function AddLifeCycle(s, e) {
        if (ASPxClientEdit.ValidateGroup('popupsave')) {
            panelLifeCycle.PerformCallback();
        }
    }

    function panelLifeCycle_EndCallback(s, e) {
        popupLifeCycle.Hide();
    }
</script>
<dx:aspxloadingpanel id="LoadingPanel" runat="server" text="Please Wait ..." clientinstancename="LoadingPanel" modal="True">
</dx:aspxloadingpanel>
<dx:ASPxGlobalEvents ID="ge" runat="server">
    <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
</dx:ASPxGlobalEvents>
<table style="width: 97%;" cellspacing="0" cellpadding="0">
    <tr>
        <td valign="top" style="width: 200px;  class="lc-td">
            <asp:Panel ID="pLifeCycle" runat="server" CssClass="lifecycle-pane">
                <div class="fullwidth" style="border: 1px inset gray;">
                    <div class="fullwidth btlc-div">
                        <asp:LinkButton ID="btLifeCycle" runat="server" Text='Modules' CssClass="btlifecycle-header"></asp:LinkButton>
                    </div>
                    <asp:Repeater ID="rModules" runat="server" OnItemDataBound="rModules_ItemDataBound">
                        <ItemTemplate>
                            <div class="fullwidth btlc-div">
                                <asp:LinkButton ID="btModuleStages" runat="server" Text='<%# Eval("Title") %>' CssClass="btlifecycle-link" OnClick="btModuleStages_Click"></asp:LinkButton>
                                <asp:HiddenField ID="hndModuleName" runat="server" Value='<%# Eval("ModuleName") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

            </asp:Panel>
        </td>
        <td valign="top">
            <asp:Panel ID="pLifeCycleStage" runat="server" CssClass="lifecyclestage-pane">

                <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                    Width="100%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">


                            <asp:Panel ID="lcsgraphics" runat="server" CssClass="lcsgraphics">
                            </asp:Panel>
                            <div id="divAddLifecycle" runat="server" visible="false">
                                <dx:ASPxLabel ID="lbllifecycles" runat="server" ClientInstanceName="lbllifecycles" Text="LifeCycles:&nbsp;&nbsp;"
                                    Style="padding-top: 5px; float: left;" Font-Bold="true">
                                </dx:ASPxLabel>
                                <dx:ASPxComboBox ID="moduleLifeCycles" ClientInstanceName="moduleLifeCycles" runat="server" Style="float: left;"
                                    SelectedIndex="0" OnSelectedIndexChanged="moduleLifeCycles_SelectedIndexChanged" AutoPostBack="true">
                                    <Items>
                                    </Items>
                                </dx:ASPxComboBox>
                                <dx:ASPxHyperLink ID="lnkNewLifeCycle" runat="server" Style="padding-top: 5px; padding-left: 10px; float: left; cursor: pointer;"
                                    ImageUrl="/Content/Images/add_icon.png">
                                    <ClientSideEvents Click="ShowLifeCyclePopup" />
                                </dx:ASPxHyperLink>
                            </div>
                            <asp:Panel ID="lcsDetail" runat="server" CssClass="lcsDetail">
                                <div style="width: 100%; float: left; color: red;">
                                    <asp:Label ID="lbMsg" runat="server"></asp:Label>
                                </div>
                                <div style="width: 100%; float: right;">
                                    <div>
                                        <ugit:ASPxGridView ID="_grid" runat="server" Width="100%" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke" SettingsBehavior-AllowSort="false"
                                            HeaderStyle-Height="10px" HeaderStyle-CssClass="aa" DataKeyNames="ID" AutoGenerateColumns="false" OnHtmlRowPrepared="_grid_HtmlRowPrepared" OnPageIndexChanged="_grid_PageIndexChanged">

                                            <Columns>
                                                <dx:GridViewDataColumn Name="Edit" HeaderStyle-Height="2" HeaderStyle-Font-Bold="true">
                                                    <DataItemTemplate>
                                                        <a id="aEdit" runat="server" src="">
                                                            <img id="Imgedit" runat="server" style="width:16px" src="/Content/Images/editNewIcon.png" />
                                                        </a>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="StageStep" Caption="Stage #" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center">
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Name="Title" FieldName="Title" Caption="Stage Title" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                                    <DataItemTemplate>
                                                        <a id="lnkStage" runat="server" src=""><%# Eval("StageTitle") %></a>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn Caption="Type" FieldName="StageTypeChoice" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                                    <DataItemTemplate>
                                                        <%--<asp:Label ID="lblStageType" runat="server"><%# Convert.ToString(Eval("StageTypeChoice")).Equals("None", StringComparison.InvariantCultureIgnoreCase) ? "" : Eval("StageTypeChoice") %></asp:Label>--%>
                                                        <asp:Label ID="lblStageType" runat="server"><%# Eval("StageTypeChoice")%></asp:Label>
                                                     
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="ActionUser" Caption="Action User(s)" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                                    <DataItemTemplate>
                                                        <asp:Label ID="lblActionUser" runat="server"><%# Eval("ActionUser") %></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="DataEditors" Caption="Data Editor(s)" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                                    <DataItemTemplate>
                                                        <asp:Label ID="lblDataEditor" runat="server"><%# Eval("DataEditors") %></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataTextColumn FieldName="StageWeight" Caption="Weight" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center">
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataColumn FieldName="StageApprovedStatus" Caption="Approved Stage" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center">
                                                    <DataItemTemplate>
                                                        <asp:Label ID="lblApprove" runat="server"><%# Eval("StageApprovedStatus") %></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>

                                                <dx:GridViewDataColumn FieldName="StageReturnStatus" Caption="Return Stage" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center">
                                                    <DataItemTemplate>
                                                        <asp:Label ID="lblReturn" runat="server"><%# Eval("StageReturnStatus") %></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                                <dx:GridViewDataColumn FieldName="StageRejectedStatus" Caption="Reject Stage" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center">
                                                    <DataItemTemplate>
                                                        <asp:Label ID="lblReject" runat="server"><%# Eval("StageRejectedStatus") %></asp:Label>
                                                    </DataItemTemplate>
                                                </dx:GridViewDataColumn>
                                            </Columns>
                                        </ugit:ASPxGridView>

                                        <%--<a id="aAddItem" runat="server" style="padding-top: 5px; float: right;" src="">
                                        <img id="Img1" runat="server" style="border: none" src="/Content/Images/add_icon.png" />
                                        <asp:Label ID="LblAddStage" runat="server" Text="Add New Stage"></asp:Label>
                                         </a>--%>
                                    </div>
                                    
                                    
                                </div>
                            </asp:Panel>
                            <div>
                                <div>
                                        <a id="addNewItem" Style="float: right;cursor:pointer" runat="server" href="" class="primary-btn-link">
                                            <img id="Img11" runat="server" src="/Content/Images/plus-symbol.png" />
                                            <asp:Label ID="LblAddItem1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                                        </a>
                                        <%--<dx:ASPxButton ID="aAddItem" runat="server" Style="float: right;" Text="Add New Stage" AutoPostBack="false">
                                            <Image Url="/Content/Images/add_icon.png"></Image>
                                        </dx:ASPxButton>--%>
                                    </div>
                                <div style="float: left; padding-right: 4px; padding-left: 3px;">
                                    <dx:ASPxButton ID="btnApplyChanges" ClientInstanceName="btnApplyChanges" CssClass="primary-blueBtn" OnClick="btnRefreshCache_Click" AutoPostBack="true" runat="server" Text="Apply Changes">
                                        <ClientSideEvents Click="function(s,e){ LoadingPanel.Show();}" />
                                    </dx:ASPxButton>
                                </div>
                                <div style="float: left;">
                                            <asp:Panel ID="lcsActions" runat="server">
                                                <dx:ASPxButton ID="btUpdateTickets" CssClass="primary-blueBtn" runat="server" OnClick="btUpdateTickets_Click" Text="Update Tickets"></dx:ASPxButton>
                                            </asp:Panel>
                                </div>
                            </div>
                           
                            <dx:ASPxCallbackPanel ID="panelLifeCycle" ClientInstanceName="panelLifeCycle" runat="server" OnCallback="panelLifeCycle_Callback">
                                    <ClientSideEvents EndCallback="function(s, e){ popupLifeCycle.Hide(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent runat="server">
                                            <dx:ASPxPopupControl ID="popupLifeCycle" ClientInstanceName="popupLifeCycle" runat="server" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                                                Width="400px" Height="100px" HeaderText="Add New Life Cycle" HeaderStyle-Font-Bold="true">

                                                <%--<ClientSideEvents EndCallback="function(s,e){ popupLifeCycle.Hide(); }" />--%>
                                                <ContentCollection>
                                                    <dx:PopupControlContentControl runat="server">

                                                        <div>
                                                            <table>
                                                                <tr>
                                                                    <td colspan="2"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td><b>LifeCycle Name:&nbsp;&nbsp;&nbsp; </b></td>
                                                                    <td>
                                                                        <div style="text-align: center;">
                                                                            <dx:ASPxTextBox ID="txtName" ClientInstanceName="txtName" runat="server">
                                                                                <ValidationSettings ValidationGroup="popupsave" ErrorImage-ToolTip="Required" ErrorDisplayMode="ImageWithTooltip" ErrorText="Required">
                                                                                    <RequiredField ErrorText="Required" IsRequired="true" />
                                                                                </ValidationSettings>
                                                                            </dx:ASPxTextBox>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;&nbsp;  </td>
                                                                    <td>
                                                                        <dx:ASPxComboBox ID="cmbModuleName" ClientInstanceName="cmbModuleName" runat="server" Visible="false">
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <div style="width: 100%; text-align: center;">
                                                                            <dx:ASPxButton ID="btnAddLifeCycle" ClientInstanceName="btnAddLifeCycle" runat="server" Text="Submitt"
                                                                                HorizontalAlign="Center" AutoPostBack="false">
                                                                                <ClientSideEvents Click="function(s,e){popupLifeCycle.Hide(); AddLifeCycle();}" />
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>

                                                    </dx:PopupControlContentControl>
                                                </ContentCollection>
                                            </dx:ASPxPopupControl>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                        </dx:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
                </dx:ASPxCallbackPanel>

            </asp:Panel>


        </td>
    </tr>

</table>


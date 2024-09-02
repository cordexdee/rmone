<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorkflowBottleneck.ascx.cs" Inherits="uGovernIT.Web.WorkflowBottleneck" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .StaticMenuStyle a {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }

    .btlifecycleSummary-header {
        background: none repeat scroll 0 0 #E8E6E5;
        /* float: left; */
        font-weight: bold;
        /* padding: 6px 6px 6px 2px; */
        width: 150px;
        margin-top: 1px;
        /* margin-left: 59px; */
        margin-right: -12px;
    }

    .bottleneckLabel-container {
        width: 100%;
        padding: 0px 0px 0px 0px;
        font-size: 10px;
        margin-top: 20px;
        margin-bottom: 0px;
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
    }

    .lifecyclestage-pane {
        float: left;
        width: 100%;
    }

    .lcsgraphics {
        float: left;
        width: 100%;
        height: 135px;
        text-align: center;
        padding-top: 40px;
        position: relative;
    }

    .lcsDetail {
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
        /*border-top: 10px solid red;*/
        border-top: 10px solid rgba(255, 0, 0, 0.12);
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

    .arrow-down-blue {
        position: absolute;
        top: -6px;
        left: 100px;
        width: 0;
        height: 0;
        /*border-bottom: 10px solid blue;*/
        border-bottom: 10px solid #0000ff2b;
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

    .arrow-down-connector-blue {
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
        border-left: 10px solid #4fca66;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }

    .arrow-right-blue {
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

    .arrow-left-blue {
        width: 0;
        height: 0;
        border-right: 10px solid blue;
        border-top: 5px solid transparent;
        border-bottom: 5px solid transparent;
        float: right;
    }

    /*.outer-circle {
        background: blue;
        border-radius: 50%;
        height: 38px;
        width: 38px;
        position: relative;
        /* 
        Child elements with absolute positioning will be 
        positioned relative to this div 
       
    }*

    /*.inner-circle {
        position: absolute;
        background: #fff;
        border-radius: 50%;
        height: 26px;
        width: 26px;
        /*
        Put top edge and left edge in the center
       */
    /*top: 50%;
        left: 50%;
        margin: -13px 0px 0px -13px;*/
    /* 
        Offset the position correctly with
        minus half of the width and minus half of the height 
       
    }*/

    /*.stageCount {
        top: -2px;
    }*/

    /*.digit-1 {
        left: -2px;
    }*/

    /*.digit-2 {
        left: -6px;
    }*/

    /*.digit-3 {
        left: -10px;
    }*/

    .stageCount,
    .stageCount.active,
    .stageCount.visited {
        width: 33px;
        height: 33px;
        position: relative;
    }

    .stageCount b,
    .stageCount.active b,
    .stageCount.visited b {
        font-weight: normal;
        font-size: 12px;
        font-style: normal;
        position: absolute;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
    }

    .parallelogram {
        border: 1px solid #e7e7e7;
        height: 19px;
        border-radius: 40px 40px 40px;
        /*-webkit-transform: skew(-20deg);
        -moz-transform: skew(-20deg);
        -ms-transform: skew(-20deg);
        -o-transform: skew(-20deg);*/
    }

        .parallelogram > span {
            display: block;
            height: 100%;
            background-color: red;
            position: relative;
            overflow: hidden;
            width: 50%;
            border-radius: 40px 0px 0px 40px;
        }
    /*.parallelogram:after {
            content: "";
            position: absolute;
            z-index: 10;
            top: 0;
            bottom: 0;
            left: 20%;
            border-left: 1px dashed black;
        }
        .parallelogram:before {
            content: "";
            position: absolute;
            z-index: 10;
            top: 0;
            bottom: 0;
            left: 60%;
            border-left: 1px dotted black;
        }*/
    .container {
        width: 800px;
        margin-left: auto;
        margin-right: auto;
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

    .chart-toggleBtn {
        padding: 10px;
    }

    .chart-toggleBtn i {
        border: 1px solid #e1e1e1;
        padding: 7px;
        border-radius: 4px;
        color: #4A6EE2;
        cursor: pointer;
    }

    .chart-toggleBtnText .dxeBase_UGITNavyBlueDevEx {
        font-size: 14px;
        text-transform: capitalize;
        color: #333;
    }

    .bottlneck-chart-wrap {
        padding-left: 0px;
        padding-bottom: 79px;
        border: 1px solid lightgrey;
        height: 100%;
        float: left;
        border-radius: 4px;
        width: 100%;
    }

    .chart-legent-containers {
        float: right;
        display: inline-block;
        padding: 7px;
    }

    .sub-chart {
        box-shadow: 1px 3px 10px rgb(0 0 0 / 33%);
        display: none;
        z-index: 9999;
        border-radius: 8px;
        background: #fff;
        padding: 10px;
    }

    
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var keyTracker;

    $(document).ready(function () {
        drawGraphicArrow();
    });

    function drawGraphicArrow() {
        var returnOffset = 0;
        var rejectOffset = 0;
        var approveOffset = 0;
        var leftMargin = 40;
        var stageStdWidth = 40;

        //var returnTopOffset = 2;
        var returnTopOffset = 10;
        //var rejectTopOffset = 14;
        var rejectTopOffset = -10;
        var approveTopOffset = 2;
        var subChartWidth = 350;
        var approveDLineOffset = 0;
        var returnDLineOffset = 0;

        $(".lcsgraphics").find('[id$="_tdStage"]').each(function (i, item) {
            var left = $(item).position().left + $(item).width() + leftMargin;
            var aStage = $(item).attr('aStage');
            var rStage = $(item).attr('rStage');
            var rjStage = $(item).attr('rjStage');
            var sbChart = $('#' + item.id + ' .sub-chart');
            if (sbChart) {
                var sbChartRight = parseInt($(".lcsgraphics").width()) - left;
                sbChart.css({ 'left': left + 'px', 'margin-top': `${$(item).width()}px` });
                if (parseInt(sbChartRight) < parseInt($(sbChart).width()))
                    sbChart.css({ 'left': left - ($(sbChart).width() + $(item).width() + $(item).width() / 2) + 'px', 'margin-top': `${$(item).width()}px` });
            }
            if (aStage > 0) {
                var stage = getStageByNo(aStage);
                var stepDiff = Math.abs($(stage).find("span").text().trim() - $(item).find("span").text().trim());

                if (parseInt($(stage).find("span").text().trim()) > parseInt($(item).find("span").text().trim())) {
                    var width = ($(stage).position().left + $(stage).width()) - left;

                    var arrowHtml = '<div style="margin-top: ' + approveTopOffset + 'px; margin-left: ' + approveOffset +
                        'px; border:1px solid; height:30px; width: ' + (width + approveOffset) + 'px;border-bottom: None;position: absolute;top: 2px;left: ' +
                        left + 'px;color: green;"><div class="arrow-down-green" style="top:30px;left:' + (width + approveOffset - 4) + 'px !important;"></div></div>';

                    if (stepDiff == 1) {
                        arrowHtml = '<div style="margin-top: 10px; margin-left: ' + approveOffset + 'px; border:1px solid; height:0px; width: ' +
                            (width + approveOffset - approveDLineOffset) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 73px;left: ' +
                            (left + approveDLineOffset) + 'px;color: #4fca66;"><div class="arrow-right-green" style="top:2px;left:' + (width + approveOffset - 4) +
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

                    left = $(item).position().left + $(stage).width() + leftMargin - returnOffset;
                    var width = (($(stage).position().left + $(stage).width()) - left - returnOffset);
                    //var arrowHtml = '<div style="margin-top: ' + returnTopOffset + 'px;margin-left: ' + (returnOffset) +
                    //    'px;border:1px solid;height:30px;width: ' + (width - (returnOffset)) + 'px;border-top: None;position: absolute;top: 116px;left: ' +
                    //    left + 'px;color: blue;"><div class="arrow-down-blue" style="top:-4px;left:' + (width - 5 - (returnOffset)) + 'px !important;"></div></div>';

                    var arrowHtml = '<div style="margin-top: ' + returnTopOffset + 'px;margin-left: ' + (returnOffset) +
                        'px;border:1px solid;height:30px;width: ' + (width - (returnOffset)) + 'px;border-top: None;position: absolute;top: 116px;left: ' +
                        left + 'px;color: #0000ff2b;"><div class="arrow-down-blue" style="top:-4px;left:' + (width - 5 - (returnOffset)) + 'px !important;"></div></div>';

                    if (stepDiff == 1) {
                        arrowHtml = '<div style="margin-top: 14px;margin-left: ' + (returnOffset) + 'px;border:1px solid;height:0px;width: ' +
                            (width - (returnOffset) - returnDLineOffset) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 93px;left: ' +
                            (left + (returnDLineOffset - 5)) + 'px;color: blue;"><div class="arrow-right-blue" style="top:0px;left:' + (width - 5 - (returnOffset)) +
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
                    left = $(stage).position().left + $(item).width() + leftMargin - returnOffset;
                    // left = $(stage).position().left + $(item).width()  - returnOffset;
                    var width = (($(item).position().left + $(item).width()) - left - returnOffset);

                    //var arrowHtml = '<div style="margin-top: ' + returnTopOffset + 'px;margin-left: ' + (returnOffset) +
                    //    'px;border:1px solid;height:30px;width: ' + (width - (returnOffset)) + 'px;border-top: None;position: absolute;top: 116px;left: ' + left +
                    //    'px;color: blue;"><div class="arrow-down-blue" style="top:-4px;left:-6px !important;"></div></div>';
                    var returnleft = $(stage).position().left + leftMargin + stageStdWidth - $(stage).width() / 2 - returnOffset;
                    var returnwidth = (($(item).position().left + $(item).width() + leftMargin) - left - returnOffset);
                    var arrowHtml = '<div style="margin-top: ' + returnTopOffset + 'px;margin-left: ' + (returnOffset) +
                        'px;border:1px solid;height:30px;width: ' + (returnwidth - (returnOffset)) + 'px;border-top: None;position: absolute;top: 116px;left: ' + returnleft +
                        'px;color:#0000ff2b;" class="rLine-down-blue"><div class="arrow-down-blue" style="top:-4px;left:-6px !important;"></div></div>';

                    if (stepDiff == 1) {
                        var arrowHtml = '<div style="margin-top: 14px;margin-left: ' + (returnOffset) + 'px;border:1px solid;height:0px;width: ' +
                            (width - (returnOffset) - returnDLineOffset) + 'px;border-bottom: None;border-right: none;border-left: none;position: absolute;top: 85px;left: ' +
                            (left + (returnDLineOffset - 5)) +
                            'px;color: blue;"><div class="arrow-left-blue" style="top:-4px;left:-6px !important;margin-top: -6px;float:left;"></div></div>';
                    }

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                //returnTopOffset += 4;
                returnTopOffset += 6;
            }

            if (rjStage > 0) {
                var stage = getStageByNo(rjStage);
                if (parseInt($(stage).find("span").text().trim()) > parseInt($(item).find("span").text().trim())) {
                    left = $(item).position().left + leftMargin + stageStdWidth - $(item).width() / 2 - 2;
                    var width = ($(stage).position().left + $(stage).width() / 2 + leftMargin) - left - 2;

                    //var arrowHtml = '<div style="margin-top: ' + rejectTopOffset + 'px;margin-left: ' + rejectOffset +
                    //    'px;border:1px solid;height:30px;width: ' + (width - rejectOffset) + 'px;border-bottom: None;position: absolute;top: 3px;left: ' + left +
                    //    'px;color: red;"><div class="arrow-down-red" style="top:30px;left:' + (width - rejectOffset - 7) + 'px !important;"></div></div>';

                    var arrowHtml = '<div style="margin-top: ' + rejectTopOffset + 'px;margin-left: ' + rejectOffset +
                        'px;border:1px solid;height:30px;width: ' + (width - rejectOffset) + 'px;border-bottom: None;position: absolute;top: 3px;left: ' + left +
                        'px;color: #ff00001f;" class="rjLine-down-red"><div class="arrow-down-red" style="top:30px;left:' + (width - rejectOffset - 7) + 'px !important;"></div></div>';

                    if ($(item).next().length > 0) {
                        $(item).next().append(arrowHtml);
                    }
                    else {
                        $(item).prev().append(arrowHtml);
                    }
                }
                else {
                    left = $(stage).position().left + leftMargin + stageStdWidth - $(stage).width() / 2 - 2;
                    var width = ($(item).position().left + $(item).width() / 2 + leftMargin) - left - 2;

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
                rejectTopOffset += 6;
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



    function highlightAllLines() {
        var ListRejectDownLine = $('.rjLine-down-red');
        var listArrowDownRed = $('.arrow-down-red');
        var listReturnDownLine = $('.rLine-down-blue');
        var listArrowDownBlue = $('.arrow-down-blue');
        ListRejectDownLine.each(function (i, rejectDownLine) { $(rejectDownLine).css('color', 'red') })
        listArrowDownRed.each(function (i, arrowDownRed) { $(arrowDownRed).css('border-top', '10px solid red') })
        listReturnDownLine.each(function (i, returnDownLine) { $(returnDownLine).css('color', 'blue') })
        listArrowDownBlue.each(function (i, arrowDownBlue) { $(arrowDownBlue).css('border-bottom', '10px solid blue') })
        $('#btnFadeLine').css('display', 'block');
        $('#btnShowLine').css('display', 'none');
    }
    function fadeAllLines(btnFadeLine) {
        var ListRejectDownLine = $('.rjLine-down-red');
        var listArrowDownRed = $('.arrow-down-red');
        var listReturnDownLine = $('.rLine-down-blue');
        var listArrowDownBlue = $('.arrow-down-blue');
        ListRejectDownLine.each(function (i, rejectDownLine) { $(rejectDownLine).css('color', 'rgba(255, 0, 0, 0.12)') })
        listArrowDownRed.each(function (i, arrowDownRed) { $(arrowDownRed).css('border-top', '10px solid rgba(255, 0, 0, 0.12)') })
        listReturnDownLine.each(function (i, returnDownLine) { $(returnDownLine).css('color', 'rgba(0, 0, 255, 0.17)') })
        listArrowDownBlue.each(function (i, arrowDownBlue) { $(arrowDownBlue).css('border-bottom', '10px solid rgba(0, 0, 255, 0.17)') })
        $(btnFadeLine).css('display', 'none');
        $('#btnShowLine').css('display', 'block');
    }

    function highlightLine(td) {
        var listLines = $(td).next().children();
        var subChart = $('#' + td.id + ' .sub-chart')
        subChart.css('display', 'block');

        listLines.each(function (i, line) {
            if ($(line).hasClass('rjLine-down-red')) {
                $(line).css('color', 'red');
                $(line).css('border', '2px solid');
                $(line).css('border-bottom', 'none');
            }
            if ($($(line).children()).hasClass('arrow-down-red')) {
                $($(line).children()).css('border-top', '10px solid red');
                // $(line).css('border', '1px solid');
            }
            if ($(line).hasClass('rLine-down-blue')) {
                $(line).css('color', 'blue');
                $(line).css('border', '2px solid');
                $(line).css('border-top', 'none');
            }
            if ($($(line).children()).hasClass('arrow-down-blue')) {
                $($(line).children()).css('border-bottom', '10px solid blue');
                // $(line).css('border', '1px solid');
            }

        })

    }


    function fadeLines(td) {

        var listLines = $(td).next().children();
        var subChart = $('#' + td.id + ' .sub-chart')
        subChart.css('display', 'none');
        listLines.each(function (i, line) {
            if ($(line).hasClass('rjLine-down-red')) {
                $(line).css('color', 'rgba(255, 0, 0, 0.12)');
                $(line).css('border', '1px solid');
                $(line).css('border-bottom', 'none');
            }
            if ($($(line).children()).hasClass('arrow-down-red')) {
                $($(line).children()).css('border-top', '10px solid rgba(255, 0, 0, 0.12)');
                // $(line).css('border', '1px solid');
            }
            if ($(line).hasClass('rLine-down-blue')) {
                $(line).css('color', 'rgba(0, 0, 255, 0.17)');
                $(line).css('border', '1px solid');
                $(line).css('border-top', 'none');
            }
            if ($($(line).children()).hasClass('arrow-down-blue')) {
                $($(line).children()).css('border-bottom', '10px solid rgba(0, 0, 255, 0.17)');
                // $(line).css('border', '1px solid');
            }

        })
    }

    <%--$(document).ready(function () {
        var chartOnDiolog = "<%=IsDialog%>";
        $('.chart-container').removeClass('bottleneckChart-container');
        $('.chart-container').removeClass('container');
        if (chartOnDiolog == 'False') {
            $('.chart-container').addClass('bottleneckChart-container');
        } else {
            $('.chart-container').addClass('container');
        }
    });--%>
    //$(function () {
    //    try {
    //        //If stage lable height is less then 20then change top position of label
    //        $(".alternategraphiclabel").each(function (i, item) {
    //            var label = $.trim($(item).find("b").html());
    //            if ($(item).find("b").height() < 20) {
    //                $(item).css("top", "-18px");
    //            }
    //        });
    //    } catch (ex) {
    //    }
    //});
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.bottleneckChart-containerWrap').parents().eq(3).addClass('bottleneckChartBody');
    });

    function UpdateTitleInCount() {
        debugger;
        var label = $('#<%=lblTotalCount.ClientID%>');
        label.text(hdnkeepcount.Get("ticketCountTitle"));
    }
</script>

<div>
    <div style="text-align: center; width: 100%; float: left;">
        <h1>
            <asp:Label ID="lblTotalCount" runat="server"></asp:Label></h1>
    </div>

    <%if (ModuleName == "PMM")
        {%>
    <div style="width: 100%; text-align: center;">
        <div style="display: inline-block; margin-top: 10px; width: 40%;">
            <dx:ASPxComboBox ID="cmbLifeCycle" Caption="Life Cycle" CaptionStyle-Font-Bold="true" runat="server" ClientInstanceName="cmbLifeCycle" AutoPostBack="false" Width="100%">
                <ClientSideEvents ValueChanged="function(s,e){callbackpanel.PerformCallback();}" />
            </dx:ASPxComboBox>
        </div>
    </div>
    <% }%>
</div>
<dx:ASPxCallbackPanel ID="callbackpanel" runat="server" ClientInstanceName="callbackpanel" RenderMode="Div">
    <PanelCollection>
        <dx:PanelContent>
            <dx:ASPxHiddenField ID="hdnkeepcount" ClientInstanceName="hdnkeepcount" runat="server"></dx:ASPxHiddenField>
            <div id="dvContainer" runat="server" class="bottleneckChart-containerWrap">
                <asp:Repeater ID="ParentRepeater" runat="server" OnItemDataBound="Parentrepeater_ItemDataBound">
                    <ItemTemplate>
                       

                        <div class="bottleneckChart-container">
                            <%--<dx:ASPxHiddenField ID="hdnkeepcount" ClientInstanceName="hdnkeepcount" runat="server"></dx:ASPxHiddenField>--%>
                            <div class="bottlneck-chart-wrap">
                                <div class="d-flex justify-content-between align-items-center mb-4">
                                    <div>
                                        <div onclick="highlightAllLines();" id="btnShowLine" title="show arrows" class="chart-toggleBtn">
                                            <i class="far fa-eye"></i>
                                        </div>
                                        <div id="btnFadeLine" style="width: 50px; display: none" class="chart-toggleBtn" onclick="fadeAllLines(this);" title="show arrows">
                                            <i class="far fa-eye-slash"></i>
                                        </div>
                                    </div>
                                    <div class="chart-toggleBtnText">
                                        <dx:ASPxLabel ID="lblTotalText" runat="server" ></dx:ASPxLabel>
                                    </div>
                                    <div class="chart-legent-containers">
                                        <div class="legendNormal-wrap">
                                            <div class="legend-close legendIcon"></div>
                                            <div class="legendLabel">Closed</div>
                                        </div>
                                        <div class="legendHeigh-wrap">
                                            <div class="legend-next legendIcon"></div>
                                            <div class="legendLabel">Next Stage</div>
                                        </div>
                                        <div class="legendCritical-wrap">
                                            <div class="legend-ruturn legendIcon"></div>
                                            <div class="legendLabel">Reject / Return</div>
                                        </div>
                                    </div>
                                </div>
                                <div style="padding-left: 60px; width: 100%; float: left;">
                                    <asp:Panel ID="lcsgraphics" runat="server" CssClass="lcsgraphics">
                                        <div class="contract_steps_module workflowGraphicContainer111">
                                            <div id="stepContainer" runat="server" class="contract_steps_container">
                                                <div id="steptopContainer" runat="server" class="contract_steptop_content">
                                                    <table style="text-align: center; border-collapse: collapse; width: auto; height: 30px;">
                                                        <tr>
                                                            <div id="divraz" style="text-align: center; border-collapse: collapse; width: 900px;">
                                                                <%--<td style="height: 38px; width: 98px;">--%>
                                                                <td style="height: 38px; width: auto;">
                                                                    <table style="text-align: center; border-collapse: collapse;">
                                                                        <tr>
                                                                            <asp:Repeater ID="stageRepeater" runat="server" OnItemDataBound="StageRptr_ItemDataBound">
                                                                                <ItemTemplate>
                                                                                    <td id="tdStage" runat="server" style="height: 38px; width: 38px;" class="tdStage" onmouseover="highlightLine(this)" onmouseout="fadeLines(this)">
                                                                                        <div style="position: absolute; width: 350px;" id="divBar" runat="server" class="sub-chart">
                                                                                            <asp:Literal runat="server" ID="lbLine" />
                                                                                        </div>
                                                                                        <div class="outer-circle" id="divOuterCircle" runat="server" style="cursor: pointer">
                                                                                            <div class="inner-circle">
                                                                                                <div>
                                                                                                    <span style="display: none"><i>
                                                                                                        <asp:Literal ID="lbStageNumber" runat="server"></asp:Literal>
                                                                                                    </i>
                                                                                                    </span>

                                                                                                    <div id="spdigit" runat="server">
                                                                                                        <b>
                                                                                                            <asp:Literal ID="lbStageCount" runat="server"></asp:Literal>
                                                                                                        </b>
                                                                                                    </div>

                                                                                                    <i class="stage-titlecontainer" id="stageTitleContainer" runat="server">
                                                                                                        <b class="pos_rel " style="">
                                                                                                            <asp:Literal ID="stageTitle" runat="server"></asp:Literal>
                                                                                                        </b>
                                                                                                    </i>

                                                                                                    <i id="activeStageArrow" runat="server"></i>
                                                                                                </div>

                                                                                            </div>
                                                                                        </div>
                                                                                    </td>
                                                                                    <td id="tdStepLine" runat="server" class="droppable tdStepLine" style="height: 38px; background-repeat: repeat-x;">&nbsp;
                                                                                    </td>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <td>
                                                                                        <asp:Label ID="lblEmptyData" Text="Lifecycle Not Found." runat="server" Visible="false"></asp:Label></td>
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </div>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <div id="summaryContainer" runat="server" class="summaryContainer" style="display: none;">
                    <div style="text-align: left;">
                        <asp:Repeater ID="rptSummary" runat="server" OnItemDataBound="rptSummary_ItemDataBound">
                            <ItemTemplate>
                                <div style="margin-bottom: 20px;">
                                    <div style="font-weight: bold; font-size: 14px; color: gray;">
                                        <asp:Literal ID="lbStageTitle" runat="server"></asp:Literal>
                                        <asp:Literal ID="lbStageCount" runat="server" Visible="false"></asp:Literal>
                                    </div>
                                    <div>
                                        <div style="position: relative; width: 350px;" id="divBar" runat="server">
                                            <asp:Literal runat="server" ID="lbLine" />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="function(s,e){UpdateTitleInCount();}" />
</dx:ASPxCallbackPanel>


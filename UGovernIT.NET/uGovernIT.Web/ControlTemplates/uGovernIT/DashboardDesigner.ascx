<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardDesigner.ascx.cs" Inherits="uGovernIT.Web.DashboardDesigner" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('#<%=chkbxThemable.ClientID%>').bind("click", function () {
            if ($('#<%=chkbxThemable.ClientID%>').is(':checked')) {
                $('#<%=ceViewBGColor.ClientID%>').hide();
            }
            else {
                $('#<%=ceViewBGColor.ClientID%>').show();
            }
        });

    });

    function InitalizejQuery() {

        $(".dashboardpicker-row").draggable({
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

        $('.dragresize').draggable({
            //containment: "parent",
            opacity: 1,
            scroll: true,
            cursorAt: { top: 7, left: 40 }
        });


        $(".dragresize").resizable({
            minHeight: 100,
            minWidth: 150,
            grid: 20,
            start: function (e, ui) {
                if (ui.element.find('#divID').length == 0) {
                    ui.element.append('<div id="divID" style="width:150px;position:absolute;"></div>');
                }
            },
            resize: function (e, ui) {
                if (ui.element.find('#divID').length > 0) {
                    //ui.element.find('#divID').css('left', ui.element.find('.ui-icon').position().left + 20);
                    ui.element.find('#divID').css('top', ui.element.find('.ui-icon').position().top);
                    ui.element.find('#divID')[0].innerText = Math.floor(ui.size.width) + ' X ' + Math.floor(ui.size.height);
                }

            },
            stop: function (e, ui) {
            }
        });

        $('.droppable').droppable({
            activeClass: "hover",
            drop: function (event, ui) {
                var draggingRowKey = ui.draggable.find("[id$='title']").val(); //ui.draggable.find("input[type='hidden']").val();
                var theme = ui.draggable.find("[id$='SPTheme']").val();
                var panelType = ui.draggable.find("[id$='panelType']").val();
                var fontstyle = ui.draggable.find("[id$='fontstyle']").val();
                var headerfontstyle = ui.draggable.find("[id$='headerfontstyle']").val();
                var dashboardSubType = ui.draggable.find("[id$='dashboardSubType']").val();

                var offSetTop = ui.position.top < 0 ? ui.position.top * -1 : ui.position.top;
                var offSetLeft = ui.position.left < 0 ? ui.position.left * -1 : ui.position.left;

                var dashboardListTop = dashboardList.GetPosition(0, false);
                var dashboardListLeft = dashboardList.GetPosition(0, true);

                offSetTop = ui.position.top < 0 ? dashboardListTop - offSetTop : dashboardListTop + offSetTop;
                offSetLeft = ui.position.left < 0 ? dashboardListLeft - offSetLeft : dashboardListLeft + offSetLeft;

                if (theme == undefined) {
                    theme = 'None';
                }

                //alert(dashboardList.GetPosition(0, true) + ', ' + dashboardList.GetPosition(0, false));
                if (draggingRowKey != undefined) {
                    $(".droppable").append(' <div id="' + draggingRowKey + '" dashboardSubType=' + dashboardSubType + '  Theme="' + theme + '" panelType="' + panelType + '" fontstyle="' + fontstyle + '" headerfontstyle="' + headerfontstyle + '"  style="z-index:100;width: 150px;height:120px;background-color:#fff;text-align: center;position:absolute;border: 1px solid;top:' + offSetTop + 'px;left:' + offSetLeft + 'px;" class="dragresize"><img style="float: right;padding-right: 2px;padding-top: 2px;" onclick="CloseDashboard(this);" src="/content/images/delete.png"/> <img style="float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px" onclick="ShowEditPanel(this);" src="/content/images/edit-icon.png"/> <img style="float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px" onclick="Sendtoback(this);" src="/content/images/backward-16.png"  title="Sent to Back"/> <img style="float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px" onclick="Bringtofront(this);" src="/content/images/forward-16.png" title="Bring to front"/> <img style="float: right;padding-right: 2px;padding-top: 2px;width:15px;height:15px" onclick="ShowClonePanel(this);" src="/content/images/duplicate.png" title="duplicate"/><br/><b><p>' + ui.draggable.find('td').text().trim() + '</p></b></div>');
                    $('.dragresize').draggable({
                        //containment: "parent",
                        opacity: 1,
                        scroll: true,
                        cursorAt: { top: 7, left: 130 }
                    });

                    $(".dragresize").resizable({
                        minHeight: 100,
                        minWidth: 150,
                        grid: 20,
                        start: function (e, ui) {
                            if (ui.element.find('#divID').length == 0) {
                                ui.element.append('<div id="divID" style="width:150px;position:absolute;">150 X 120</div>');
                            }
                        },
                        resize: function (e, ui) {
                            if (ui.element.find('#divID').length > 0) {
                                //ui.element.find('#divID').css('left', ui.element.find('.ui-icon').position().left + 20);
                                ui.element.find('#divID').css('top', ui.element.find('.ui-icon').position().top);
                                ui.element.find('#divID')[0].innerText = Math.floor(ui.size.width) + ' X ' + Math.floor(ui.size.height);
                            }
                        },
                        stop: function (e, ui) {
                        }
                    });
                }
            }
        });
    }



    $(function () {

        lnkType_Change(true);
        if ($('div .dragresize').length > 0) {

            for (var i = 0; i < $('div .dragresize').length; i++) {

                if ($('div .dragresize').get(i).id == $('[id$="hndDashboardName"]').val()) {
                    var obj = $('div .dragresize').get(i);

                    $('[id$="trLinkControl"]').hide();
                    $('[id$="trLinkLabel"]').hide();
                    $('[id$="trImage"]').hide();
                    $('[id$="trIconShape"]').hide();

                    $('[id$="trBackground"]').hide();
                    $('[id$="trTitlePosition"]').hide();
                    $('[id$="hndIsLink"]').val(0);
                    $('[id$="panalType"]').val('');
                    $('[id$="txtIconImage"]').val('');
                    $('[id$="ddlIconShape"]').val('');

                    $('[id$="trPanelPostion"]').hide();
                    $('[id$="trIconDimension"]').hide();
                    $('[id$="trIconPosition"]').hide();
                    $('[id$="trNavigationType"]').hide();

                    $('[id$="trHeaderFontStyle"]').hide();
                    $('[id$="trFontStyle"]').hide();
                    ceBackGround.SetVisible(false);
                    $('[id$="trBorderColor"]').hide();
                    break;

                }
            }
        }

        checkUser();
        $("#<%=rdoUser.ClientID%>").change(function () {
            checkUser();
        });

        $(".dxpc-mainDiv.dxpc-shadow").css("float", "left");
    });

    function UpdateHiddenFields() {
        hndDashboards.Clear();
        hndDashboardsList.Clear();
        var prefix;
        var vOffSet = 0;
        for (var i = 0; i < $(".dragresize").length; i++) {
            if ($($(".dragresize")[i]).position().top < 0) {
                var offSet = (-1 * $($(".dragresize")[i]).position().top);
                if (offSet > vOffSet) {
                    vOffSet = offSet;
                }
            }
        }

        if (vOffSet != 0) {
            vOffSet += 50;
        }
        for (var i = 0; i < $(".dragresize").length; i++) {
            prefix = $($(".dragresize")[i]).attr('id');
            if ($($(".dragresize")[i]).attr('widthUnitType') == '%') {
                let width = $(".dragresize")[i].offsetWidth;
                let outerWidth = $(".dragresize").parent().parent().outerWidth() - 32;
                let widthInPer = Math.floor(width * 100 / outerWidth);
                let actualWidth = $($(".dragresize")[i]).attr('widthPercentage');
                widthInPer = actualWidth - widthInPer > -3 || actualWidth - widthInPer < 3 ? actualWidth : widthInPer;
                hndDashboards.Set(prefix + '_width', widthInPer);
                hndDashboards.Set(prefix + '_widthunit', "Percentage");
            }
            else {
                hndDashboards.Set(prefix + '_width', $(".dragresize")[i].offsetWidth);
                hndDashboards.Set(prefix + '_widthunit', "Pixel");
            }
            if ($($(".dragresize")[i]).attr('leftUnitType') == '%') {
                let left = $($(".dragresize")[i]).position().left;
                let outerWidth = $(".dragresize").parent().parent().outerWidth() - 32;
                let leftInPer = Math.floor(left * 100 / outerWidth);
                let actualLeft = $($(".dragresize")[i]).attr('leftPercentage');
                leftInPer = actualLeft - leftInPer > -3 || actualLeft - leftInPer < 3 ? actualLeft : leftInPer;
                hndDashboards.Set(prefix + '_left', leftInPer);
                hndDashboards.Set(prefix + '_leftunit', "Percentage");
            }
            else {
                hndDashboards.Set(prefix + '_left', $($(".dragresize")[i]).position().left);
                hndDashboards.Set(prefix + '_leftunit', "Pixel");
            }
            hndDashboards.Set(prefix + '_height', $(".dragresize")[i].offsetHeight);
            hndDashboards.Set(prefix + '_top', vOffSet + $($(".dragresize")[i]).position().top);
            hndDashboards.Set(prefix + '_zindex', $($(".dragresize")[i]).css("z-index"));
            hndDashboards.Set(prefix + '_paneltype', $($(".dragresize")[i]).attr('paneltype'));
            hndDashboards.Set(prefix + '_DashboardSubType', $($(".dragresize")[i]).attr('dashboardSubType'));

            hndDashboards.Set(prefix + '_iconwidth', $($(".dragresize")[i]).attr('iconwidth'));
            hndDashboards.Set(prefix + '_iconheight', $($(".dragresize")[i]).attr('iconheight'));
            hndDashboards.Set(prefix + '_iconleft', $($(".dragresize")[i]).attr('iconleft'));
            hndDashboards.Set(prefix + '_icontop', $($(".dragresize")[i]).attr('icontop'));

            hndDashboards.Set(prefix + '_linktype', $($(".dragresize")[i]).attr('linktype'));
            hndDashboards.Set(prefix + '_linkdetails', $($(".dragresize")[i]).attr('linkdetails'));
            hndDashboards.Set(prefix + '_navigationtype', $($(".dragresize")[i]).attr('navigationtype'));
            hndDashboards.Set(prefix + '_theme', $($(".dragresize")[i]).attr('theme'));
            hndDashboards.Set(prefix + '_queryparameter', $($(".dragresize")[i]).attr('queryparameter'));
            hndDashboards.Set(prefix + '_iconurl', $($(".dragresize")[i]).attr('iconurl'));
            hndDashboards.Set(prefix + '_iconshape', $($(".dragresize")[i]).attr('iconshape'));
            hndDashboards.Set(prefix + '_islink', $($(".dragresize")[i]).attr('islink'));
            hndDashboards.Set(prefix + '_panelleft', $($(".dragresize")[i]).attr('panelleft'));
            hndDashboards.Set(prefix + '_paneltop', $($(".dragresize")[i]).attr('paneltop'));
            hndDashboards.Set(prefix + '_background', unescape($($(".dragresize")[i]).attr('background')));
            hndDashboards.Set(prefix + '_fontstyle', $($(".dragresize")[i]).attr('fontstyle'));
            hndDashboards.Set(prefix + '_headerfontstyle', $($(".dragresize")[i]).attr('headerfontstyle'));
            hndDashboards.Set(prefix + '_titletop', $($(".dragresize")[i]).attr('titletop'));
            hndDashboards.Set(prefix + '_titleleft', $($(".dragresize")[i]).attr('titleleft'));
            hndDashboards.Set(prefix + '_bordercolor', $($(".dragresize")[i]).attr('bordercolor'));
            hndDashboards.Set(prefix + '_module', $($(".dragresize")[i]).attr('module'));
            hndDashboards.Set(prefix + '_psize', $($(".dragresize")[i]).attr('psize'));
            hndDashboards.Set(prefix + '_hidetitle', $($(".dragresize")[i]).attr('hidetitle'));
            hndDashboards.Set(prefix + '_priority', $($(".dragresize")[i]).attr('priority'));
            hndDashboards.Set(prefix + '_duedate', $($(".dragresize")[i]).attr('duedate'));
            hndDashboards.Set(prefix + '_iscritical', $($(".dragresize")[i]).attr('iscritical'));
            hndDashboards.Set(prefix + '_iscategory', $($(".dragresize")[i]).attr('iscategory'));
            hndDashboards.Set(prefix + '_issubcategory', $($(".dragresize")[i]).attr('issubcategory'));
            hndDashboards.Set(prefix + '_status', $($(".dragresize")[i]).attr('status'));
            hndDashboards.Set(prefix + '_enablefilter', $($(".dragresize")[i]).attr('enablefilter'));
            hndDashboards.Set(prefix + '_weeklyaverage', $($(".dragresize")[i]).attr('weeklyaverage'));
            hndDashboards.Set(prefix + '_enablefilterforpredictbacklog', $($(".dragresize")[i]).attr('enablefilterforpredictbacklog'));
            hndDashboards.Set(prefix + '_enablefilterforticketcreatedbyweek', $($(".dragresize")[i]).attr('enablefilterforticketcreatedbyweek'));
            hndDashboards.Set(prefix + '_enablefilterscorecard', $($(".dragresize")[i]).attr('enablefilterscorecard'));
            hndDashboards.Set(prefix + '_scorecardstartdate', $($(".dragresize")[i]).attr('scorecardstartdate'));
            hndDashboards.Set(prefix + '_scorecardenddate', $($(".dragresize")[i]).attr('scorecardenddate'));
            hndDashboards.Set(prefix + '_enablefilterticketflow', $($(".dragresize")[i]).attr('enablefilterticketflow'));
            hndDashboards.Set(prefix + '_ticketflowstartdate', $($(".dragresize")[i]).attr('ticketflowstartdate'));
            hndDashboards.Set(prefix + '_ticketflowenddate', $($(".dragresize")[i]).attr('ticketflowenddate'));
            hndDashboards.Set('screenwidth', $(window).width());
            //console.log("prefix : ");
            hndDashboardsList.Set(prefix, prefix);
        }
    }

    function CloseDashboard(obj) {

        $(obj).parent().remove();
    }

    function ShowGlobalFilterEditPanel(obj) {

        $('[id$="hndGlobalFilterID"]').val(obj);
        globalFilterEditPanel.PerformCallback('GetFilterDetails');
        AddEditGloblalFilter();
    }

    function ShowEditPanel(obj) {
      
        if ($(obj).parent().length > 0) {
            if ($(obj).parent().find('p').length > 0) {
                $('[id$="txtTitle"]').val($(obj).parent().find('p').text().trim()); // $(obj).parent().find('p').get(0).textContent
            }

            $('[id$="trPanelPostion"]').hide();
            $('[id$="trLinkControl"]').hide();
            $('[id$="trLinkLabel"]').hide();
            $('[id$="trImage"]').hide();
            $('[id$="trIconShape"]').hide();

            $('[id$="trBackground"]').hide();
            $('[id$="trIconDimension"]').hide();
            $('[id$="trIconPosition"]').hide();
            $('[id$="trNavigationType"]').hide();
            $('[id$="trTitlePosition"]').hide();
            $('[id$="hndIsLink"]').val(0);
            $('[id$="panalType"]').val('');
            $('[id$="txtIconImage"]').val('');
            $('[id$="ddlIconShape"]').val('');
            $('[id$="txtIconWidth"]').val('');
            $('[id$="txtIconHeight"]').val('');
            $('[id$="txtIconTop"]').val('');
            $('[id$="txtIconLeft"]').val('');
            $('[id$="txtBackgroundImage"]').val('');
            $('[id$="txtPanelTop"]').val('');
            $('[id$="trPanelLeft"]').val('');
            $('[id$="txtzindex"]').val('');
            $('[id$="trHeaderFontStyle"]').hide();
            $('[id$="trFontStyle"]').hide();
            ceBackGround.SetVisible(false);
            ceBackGround.SetColor('');
            $('[id$="trBorderColor"]').hide();
            $('[id$="trModule"]').hide();
            $('[id$="trHideSLAPerformanceTabular"]').hide();
            $('[id$="trPageSize"]').hide();
            $('[id$="trPriority"]').hide();
            $('[id$="trIsCritical"]').hide();
            $('[id$="trDueDate"]').hide();
            $('[id$="trUser"]').hide();
            $('[id$="trStatus"]').hide();
            $('[id$="trEnableFilter"]').hide();
            $('[id$="trWeeklyAverage"]').hide();
            $('[id$="trEnableFilterForPredictBacklog"]').hide();
            $('[id$="trEnableFilterForTicketCreatedByWeek"]').hide();
            $('[id$="trEnableFilterScoreCard"]').hide();
            $('[id$="trStartDateEndDateFilterScoreCardStartDate"]').hide();
            $('[id$="trStartDateEndDateFilterScoreCardEndDate"]').hide();
            $('[id$="trEnableFilterTicketFlow"]').hide();
            $('[id$="trStartEndDateTicketFlowStartDate"]').hide();
            $('[id$="trStartEndDateTicketFlowEndDate"]').hide();
            $('[id$="trCategoryorSubCategory"]').hide();
            $('[id$="trShowTabDetails"]').hide();
            $('[id$="countmyproject"]').hide();
            $('[id$="countmyopenopportunities"]').hide();
            $('[id$="countallopenproject"]').hide();
            $('[id$="countallcloseproject"]').hide();
            $('[id$="countcurrentopencpr"]').hide();
            $('[id$="countfutureopencpr"]').hide();
            $('[id$="counttotalresource"]').hide();
            $('[id$="countallopenopportunities"]').hide();
            $('[id$="countallopenservices"]').hide();
            $('[id$="countrecentwonopportunity"]').hide();
            $('[id$="countrecentlostopportunity"]').hide();
            $('[id$="helpCards"]').hide();
            $('[id$="countwaitingonme"]').hide();
            $('[id$="countopenticketstoday"]').hide();
            $('[id$="countclosedticketstoday"]').hide();
            $('[id$="countcloseticketstoday"]').hide();
            $('[id$="countnprtickets"]').hide();
            $('[id$="countresolvedtickets"]').hide();
            $('[id$="welcomemsg"]').hide();
            $('[id$="divResourceAllocation"]').hide();
            $('[id$="divAllocationConflicts"]').hide();
            $('[id$="divUnfilledProjectAllocations"]').hide();
            $('[id$="divUnfilledPipelineAllocations"]').hide();

            if (deStartDateScoreCard.GetDate() == null) {
                var d = new Date();
                d.setMonth(d.getMonth() - 1);
                deStartDateScoreCard.SetDate(d);

            }
            if (deEndDateScoreCard.GetDate() == null) { deEndDateScoreCard.SetDate(new Date()); }

            if (deStartDateTicketFlow.GetDate() == null) {
                var d = new Date();
                d.setDate(d.getDate() - 8);
                deStartDateTicketFlow.SetDate(d);
            }
            if (deEndDateTicketFlow.GetDate() == null) { deEndDateTicketFlow.SetDate(new Date); }


            $('#spHideTitle').show();
            //if ($('[id$="ddlBorder"] option[value="Transparent"]').length > 0) {
            //    $('[id$="ddlBorder"] option[value="Transparent"]').remove();
            //}

            if ($(obj).parent().attr('hideTitle') == 'True') {
                $('[id$="chkHideTitle"]').get(0).checked = true;
            }
            else {
                $('[id$="chkHideTitle"]').get(0).checked = false;
            }

            if (($(obj).parent().attr('paneltype') == 'Link' || $(obj).parent().attr('IsLink') == 'True')) {

                if ($(obj).parent().attr('linkType') != undefined && $(obj).parent().attr('linkType') != '') {
                    $('input[name$="lnkType"][value="' + $(obj).parent().attr('linkType') + '"]').prop('checked', true);
                    lnkType_Change(true);
                    $('[id$="lblLinkDetail"]').html(unescape($(obj).parent().attr('linkDetails')));
                    if ($(obj).parent().attr('linkType') != 'None') {
                        $('#dvLinkDetail').show();

                        var navigationType = $(obj).parent().attr('navigationtype');
                        if (navigationType == "1") {

                            $('input[id$="rbNewWindow"]').attr("checked", "true");
                        }
                        else if (navigationType == "2") {
                            $('input[id$="rbPopup"]').attr("checked", "true");
                        }
                        else {
                            $('input[id$="rbSameWindow"]').attr("checked", "true");
                        }
                    }
                    else {
                        $('#dvLinkDetail').hide();
                    }
                }
                else {
                    $('#dvLinkDetail').hide();
                }
                $('[id$="trTitlePosition"]').show();
                switch ($(obj).parent().attr('linkType')) {
                    case 'Analytic':
                        $('[id$="txtHelp"]').hide();
                        $("#<%=ddlModel.ClientID %>").hide();
                        $("#<%=ddlDashbaord.ClientID %>").hide();
                        break;
                    case 'Document':
                        $('[id$="txtHelp"]').hide();
                        $("#<%=fileupload.ClientID %>").hide()
                        break;
                    case 'Service':
                        $('[id$="txtHelp"]').hide();
                        $("#<%=ddlService.ClientID %>").val($(obj).parent().attr('linkDetails'));
                        $("#<%=ddlService.ClientID %>").show();
                        $('#dvLinkDetail').hide();
                        break;
                    case 'Url':
                        $('#dvLinkDetail').hide();
                        break;
                    case 'Wiki':
                        $('[id$="txtHelp"]').hide();
                        break;
                }

                if ($('[id$="ddlBorder"] option[value="Elliptical"]').length == 0) {
                    $('[id$="ddlBorder"]').append("<option value='Elliptical'>Elliptical</option>");
                }

                if ($('[id$="ddlBorder"] option[value="Transparent"]').length == 0) {
                    $('[id$="ddlBorder"]').append("<option value='Transparent'>Transparent</option>");
                }


                $('[id$="trLinkControl"]').show();
                $('[id$="trLinkLabel"]').show();
                $('[id$="hndIsLink"]').val(1);

                $('[id$="trImage"]').show();
                $('[id$="trIconShape"]').show();
                $('[id$="trBackground"]').show();
                $('[id$="trIconDimension"]').show();
                $('[id$="trIconPosition"]').show();
                $('[id$="trNavigationType"]').show();
                $('[id$="trHeaderFontStyle"]').show();
                if ($(obj).parent().attr('Theme') == 'Rectangle' || $(obj).parent().attr('Theme') == 'Elliptical') {
                    ceBackGround.SetVisible(true);
                    $('[id$="trBorderColor"]').show();
                }
                $('[id$="ddlIconShape"]').val($(obj).parent().attr('IconShape'));
                $('[id$="txtIconImage"]').val($(obj).parent().attr('IconUrl'));
                $('[id$="txtIconWidth"]').val($(obj).parent().attr('iconWidth'));
                $('[id$="txtIconHeight"]').val($(obj).parent().attr('iconHeight'));
                $('[id$="txtIconTop"]').val($(obj).parent().attr('iconTop'));
                $('[id$="txtIconLeft"]').val($(obj).parent().attr('iconLeft'));

                $('[id$="txtTitleTop"]').val($(obj).parent().attr('titleTop'));
                $('[id$="txtTitleLeft"]').val($(obj).parent().attr('titleLeft'));
                if ($(obj).parent().attr('background') != undefined) {
                    var value = unescape($(obj).parent().attr('background'));
                    if (value.indexOf(';#') > -1) {
                        if (value.split(';#')[0] == 'Url') {
                            $('[id$="txtBackgroundImage"]').val(value.split(';#')[1]);
                        }
                        else {
                            ceBackGround.SetColor(value.split(';#')[1]);
                        }
                    }
                    else {
                        $('[id$="txtBackgroundImage"]').val(unescape($(obj).parent().attr('background')));
                    }
                }


                if ($(obj).parent().attr('headerfontStyle') != undefined && $(obj).parent().attr('headerfontStyle') != '') {
                    var items = $(obj).parent().attr('headerfontStyle').split(';#');
                    $('[id$="ddlHeaderFontStyle"]').val(items[0]);
                    $('[id$="ddlHeaderFontSize"]').val(items[1]);
                    ceHeaderFont.SetColor(items[2]);
                    $('[id$="ddlHeaderFontName"]').val(items[3]);
                }

                if ($(obj).parent().attr('borderColor') != '') {
                    ceBorder.SetColor($(obj).parent().attr('borderColor'));
                }

            }
            else if ($(obj).parent().attr('paneltype') == 'Panel') {
                if ($('[id$="ddlBorder"] option[value="Elliptical"]').length == 0) {
                    $('[id$="ddlBorder"]').append("<option value='Elliptical'>Elliptical</option>");
                }
                if ($('[id$="ddlBorder"] option[value="Transparent"]').length == 0) {
                    $('[id$="ddlBorder"]').append("<option value='Transparent'>Transparent</option>");
                }
                $('[id$="trImage"]').show();
                $('[id$="trIconShape"]').show();
                $('[id$="trBackground"]').show();
                $('[id$="trIconDimension"]').show();
                $('[id$="trIconPosition"]').show();
                $('[id$="trPanelPostion"]').show();
                $('[id$="trHeaderFontStyle"]').show();
                $('[id$="trFontStyle"]').show();
                $('[id$="trTitlePosition"]').show();
                if ($(obj).parent().attr('Theme') == 'Rectangle' || $(obj).parent().attr('Theme') == 'Elliptical') {
                    ceBackGround.SetVisible(true);
                    $('[id$="trBorderColor"]').show();
                }

                $('[id$="txtIconImage"]').val($(obj).parent().attr('IconUrl'));
                $('[id$="ddlIconShape"]').val($(obj).parent().attr('IconShape'));
                $('[id$="txtIconWidth"]').val($(obj).parent().attr('iconWidth'));
                $('[id$="txtIconHeight"]').val($(obj).parent().attr('iconHeight'));
                $('[id$="txtIconTop"]').val($(obj).parent().attr('iconTop'));
                $('[id$="txtIconLeft"]').val($(obj).parent().attr('iconLeft'));
                if ($(obj).parent().attr('background') != undefined) {
                    var value = unescape($(obj).parent().attr('background'));
                    if (value.indexOf(';#') > -1) {
                        if (value.split(';#')[0] == 'Url') {
                            $('[id$="txtBackgroundImage"]').val(value.split(';#')[1]);
                        }
                        else {
                            ceBackGround.SetColor(value.split(';#')[1]);
                        }
                    }
                    else {
                        $('[id$="txtBackgroundImage"]').val(unescape($(obj).parent().attr('background')));
                    }
                }
                $('[id$="txtPanelTop"]').val($(obj).parent().attr('panelTop'));
                $('[id$="txtPanelLeft"]').val($(obj).parent().attr('panelLeft'));
                $('[id$="txtzindex"]').val($(obj).parent().css('z-index'));
                $('[id$="txtTitleTop"]').val($(obj).parent().attr('titleTop'));
                $('[id$="txtTitleLeft"]').val($(obj).parent().attr('titleLeft'));

                if ($(obj).parent().attr('fontstyle') != undefined && $(obj).parent().attr('fontstyle') != '') {
                    var items = $(obj).parent().attr('fontstyle').split(';#');
                    $('[id$="ddlFontStyle"]').val(items[0]);
                    $('[id$="ddlFontSize"]').val(items[1]);
                    ceFont.SetColor(items[2]);
                    $('[id$="ddlFontName"]').val(items[3]);
                }
                else {
                    $('[id$="ddlFontStyle"]').val("Bold");
                    $('[id$="ddlFontSize"]').val("10pt");
                    ceFont.SetColor("#000000");
                }

                if ($(obj).parent().attr('headerfontStyle') != undefined && $(obj).parent().attr('headerfontStyle') != '') {
                    var items = $(obj).parent().attr('headerfontStyle').split(';#');
                    $('[id$="ddlHeaderFontStyle"]').val(items[0]);
                    $('[id$="ddlHeaderFontSize"]').val(items[1]);
                    ceHeaderFont.SetColor(items[2]);
                    $('[id$="ddlHeaderFontName"]').val(items[3]);
                }
                else {
                    $('[id$="ddlHeaderFontStyle"]').val("Bold");
                    $('[id$="ddlHeaderFontSize"]').val("10pt");
                    ceHeaderFont.SetColor("#000000");
                }


            }
            else {

                if ($('[id$="ddlBorder"] option[value="Elliptical"]').length > 0) {
                    $('[id$="ddlBorder"] option[value="Elliptical"]').remove();
                }
                if ($(obj).parent().attr('paneltype') == "Control") {

                    if ($(obj).parent().attr('dashboardSubType') == 'MessageBoard') {
                        $('#spHideTitle').hide();
                        if ($('[id$="ddlBorder"] option[value="Transparent"]').length == 0) {
                            $('[id$="ddlBorder"]').append("<option value='Transparent'>Transparent</option>");
                        }

                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'CustomFilter') {
                        $('[id$="trModule"]').show();
                        $('[id$="trPageSize"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'SLAPerformanceTimeLine') {
                        $('[id$="trModule"]').show();

                        $('[id$="trHideSLAPerformanceTabular"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'SLAPerformanceTabular') {
                        $('[id$="trModule"]').show();



                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'UserHotList') {
                        $('[id$="trPriority"]').show();
                        $('[id$="trIsCritical"]').show();
                        $('[id$="trDueDate"]').show();
                        $('[id$="trUser"]').show();

                        $('[id$="trPageSize"]').show();
                        if ($(obj).parent().attr('priority') != undefined && $(obj).parent().attr('priority') != '')
                            $("#<%=ddlPriority.ClientID %>").val($(obj).parent().attr('priority'));
                        else
                            $("#<%=ddlPriority.ClientID %>").val('high');

                        if ($(obj).parent().attr('iscritical') != undefined && $(obj).parent().attr('iscritical') != '') {
                            if ($(obj).parent().attr('iscritical') == 'True')
                                $("#<%=chkIsCritical.ClientID %>").attr("checked", true);
                            else
                                $("#<%=chkIsCritical.ClientID %>").removeAttr("checked");
                        }
                        else
                            $("#<%=chkIsCritical.ClientID %>").attr("checked", true);

                        if ($(obj).parent().attr('duedate') != undefined && $(obj).parent().attr('duedate') != '')
                            $("#<%=ddlDueDate.ClientID %>").val($(obj).parent().attr('duedate'));
                        else
                            $("#<%=ddlDueDate.ClientID %>").val('both');


                        var value = "all";
                        if ($(obj).parent().attr('userid') != undefined && $(obj).parent().attr('userid') != '') {
                            if ($(obj).parent().attr('userid') == -1) {
                                value = "current";
                            }
                            else if ($(obj).parent().attr('userid') > 0) {
                                value = "specific"
                                cmbUser.SetSelectedItem(cmbUser.FindItemByValue($(obj).parent().attr('userid')));
                            }
                        }
                        else {
                            $("#<%=ddlDueDate.ClientID %>").val('both');
                        }
                        var radio = $("[id*=rdoUser] input[value=" + value + "]");
                        radio.attr("checked", "checked");
                        checkUser();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'WorkflowBottleneck') {
                        $('[id$="trModule"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'TicketByCategoryReport') {

                        $('[id$="trStatus"]').show();
                        $('[id$="trModule"]').show();
                        $('[id$="trCategoryorSubCategory"]').show();
                        $('[id$="hdncategoryid"]').val($(obj).parent().attr('iscategory'))

                        var arrofcategoryids = $('[id$="hdncategoryid"]').val().split(',');
                        $('[id$="ddlModule"]').val($(obj).parent().attr('module'));
                        requestTypeTreeList.PerformCallback("byshowedit :" + arrofcategoryids.join(","));

                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'WeeklyRollingAverage') {
                        $('[id$="trEnableFilter"]').show();
                        $('[id$="trWeeklyAverage"]').show();

                        if ($(obj).parent().attr('enablefilter') == 'True') {
                            $('[id$="chkEnableFilter"]').get(0).checked = true;
                        }
                        else {
                            $('[id$="chkEnableFilter"]').get(0).checked = false;
                        }

                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'PredictBacklog') {

                        $('[id$="trEnableFilterForPredictBacklog"]').show();
                        $('[id$="trWeeklyAverage"]').show();
                        if ($(obj).parent().attr('EnableFilterForPredictBacklog') == 'True') {
                            $('[id$="chkPredictBacklog"]').get(0).checked = true;
                        }
                        else {
                            $('[id$="chkPredictBacklog"]').get(0).checked = false;
                        }

                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'TicketCreatedByWeek') {
                        $('[id$="trEnableFilterForTicketCreatedByWeek"]').show();
                        $('[id$="trWeeklyAverage"]').show();

                        if ($(obj).parent().attr('EnableFilterForTicketCreatedByWeek') == 'True') {
                            $('[id$="chkTicketCreatedByWeek"]').get(0).checked = true;
                        }
                        else {
                            $('[id$="chkTicketCreatedByWeek"]').get(0).checked = false;
                        }

                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'ScoreCard') {
                        $('[id$="trEnableFilterScoreCard"]').show();
                        $('[id$="trStartDateEndDateFilterScoreCardStartDate"]').show();
                        $('[id$="trStartDateEndDateFilterScoreCardEndDate"]').show();

                        if ($(obj).parent().attr('EnableFilterScoreCard') == 'True') {
                            $('[id$="chkScoreCard"]').get(0).checked = true;
                        }
                        else {
                            $('[id$="chkScoreCard"]').get(0).checked = false;
                        }
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'TicketFlow') {
                        $('[id$="trEnableFilterTicketFlow"]').show();
                        $('[id$="trStartEndDateTicketFlowStartDate"]').show();
                        $('[id$="trStartEndDateTicketFlowEndDate"]').show();
                        if ($(obj).parent().attr('EnableFilterTicketFlow') == 'True') {
                            $('[id$="chkTicketFlow"]').get(0).checked = true;
                        }
                        else {
                            $('[id$="chkTicketFlow"]').get(0).checked = false;
                        }
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'LeftTicketCountBar') {
                        $('[id$="trShowTabDetails"]').show();
                        $('[id$="myproject"]').show();
                        $('[id$="myopenopportunities"]').show();
                        $('[id$="allopenproject"]').show();
                        $('[id$="allcloseproject"]').show();
                        $('[id$="currentopencpr"]').show();
                        $('[id$="futureopencpr"]').show();
                        $('[id$="totalresource"]').show();
                        $('[id$="allopenopportunities"]').show();
                        $('[id$="allopenservices"]').show();
                        $('[id$="recentwonopportunity"]').show();
                        $('[id$="recentlostopportunity"]').show();
                        $('[id$="waitingonme"]').show();
                        $('[id$="openticketstoday"]').show();
                        $('[id$="closeticketstoday"]').show();
                        $('[id$="countnprtickets"]').show();
                        $('[id$="countresolvedtickets"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'HelpCardsPanel') {
                        $('[id$="helpCards"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'UserWelcomePanel') {
                        $('[id$="welcomemsg"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'AllocationTimeline') {
                        $('[id$="divResourceAllocation"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'AllocationConflicts') {
                        $('[id$="divAllocationConflicts"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'UnfilledProjectAllocations') {
                        $('[id$="divUnfilledProjectAllocations"]').show();
                    }
                    else if ($(obj).parent().attr('dashboardSubType') == 'UnfilledPipelineAllocations') {
                        $('[id$="divUnfilledPipelineAllocations"]').show();
                    }
                    else {
                        $('[id$="trPageSize"]').show();
                    }
                }
            }

            if ($(obj).parent().attr('paneltype') == "Query") {
                $('#trParameters').show();
                $('#spHideTitle').hide();
                if ($('[id$="ddlBorder"] option[value="Transparent"]').length == 0) {
                    $('[id$="ddlBorder"]').append("<option value='Transparent'>Transparent</option>");
                }
                UpdateHiddenFields();
                CallbackPanel.PerformCallback("QueryParamenter");
            }
            else {
                $('#trParameters').hide();
            }

            $('[id$="txtPageSize"]').val($(obj).parent().attr('pSize'));
            $('[id$="ddlModule"]').val($(obj).parent().attr('module'));
            if ($(obj).parent().attr('hideSLATabular') == 'True') {
                $('[id$="chkbxHideTabular"]').get(0).checked = true;
            }
            else {
                $('[id$="chkbxHideTabular"]').get(0).checked = false;
            }
            $('[id$="panalType"]').val($(obj).parent().attr('paneltype'));
            $('[id$="hndDashboardName"]').val($(obj).parent().attr('id'));
            if ($(obj).parent().attr('widthUnitType') == '%') {
                let width = $(obj).parent().outerWidth();
                let outerWidth = $(obj).parent().parent().parent().outerWidth() - 32;
                let widthInPer = Math.floor(width * 100 / outerWidth);
                if ($(obj).parent().attr('widthpercentage') - widthInPer > -3 && $(obj).parent().attr('widthpercentage') - widthInPer < 3) {
                    $('[id$="txtWidth"]').val($(obj).parent().attr('widthpercentage'));
                }
                else {
                    $('[id$="txtWidth"]').val(widthInPer);
                }
                
                $("#<%=ddlWidthUnit.ClientID%>").val("Percentage");
            }
            else {
                $('[id$="txtWidth"]').val(Math.round($(obj).parent().outerWidth()));
                $("#<%=ddlWidthUnit.ClientID%>").val("Pixel");
            }

            $('[id$="txtHeight"]').val(Math.round($(obj).parent().outerHeight()));

            if ($(obj).parent().attr('leftUnitType') == '%') {

                let left = $(obj).parent().position().left;
                let outerWidth = $(obj).parent().parent().parent().outerWidth() - 32;
                let leftInPer = Math.floor(left * 100 / outerWidth);
                if ($(obj).parent().attr('leftpercentage') - leftInPer > -3 && $(obj).parent().attr('leftpercentage') - leftInPer < 3) {
                    $('[id$="txtLeft"]').val($(obj).parent().attr('leftpercentage'));
                }
                else {
                    $('[id$="txtLeft"]').val(leftInPer);
                }
                $("#<%=ddlLeftUnit.ClientID%>").val("Percentage");
            }
            else {
                $('[id$="txtLeft"]').val($(obj).parent().position().left);
                $("#<%=ddlLeftUnit.ClientID%>").val("Pixel");
            }
            $('[id$="txtTop"]').val($(obj).parent().position().top);
            $('[id$="txtzindex"]').val($(obj).parent().css('z-index'));
            $('[id$="ddlBorder"]').val($(obj).parent().attr('Theme'));
            $('[id$="txtHelp"]').val($(obj).parent().attr('LinkUrl'));
            $('[id$="ddlStatus"]').val($(obj).parent().attr('status'));

            //$('[id$="chkEnableFilter"]').val($(obj).parent().attr('enablefilter'));
            $('[id$="txtWeeklyAverage"]').val($(obj).parent().attr('WeeklyAverage'));
            $('[id$="chkPredictBacklog"]').val($(obj).parent().attr('EnableFilterForPredictBacklog'));
            $('[id$="chkTicketCreatedByWeek"]').val($(obj).parent().attr('EnableFilterForTicketCreatedByWeek'));
            $('[id$="chkScoreCard"]').val($(obj).parent().attr('EnableFilterScoreCard'));
            $('[id$="deStartDateScoreCard"]').val($(obj).parent().attr('ScoreCardStartDate'));
            $('[id$="deEndDateScoreCard"]').val($(obj).parent().attr('ScoreCardEndDate'));
            $('[id$="chkTicketFlow"]').val($(obj).parent().attr('EnableFilterTicketFlow'));
            $('[id$="deStartDateTicketFlow"]').val($(obj).parent().attr('TicketFlowStartDate'));
            $('[id$="deEndDateTicketFlow"]').val($(obj).parent().attr('TicketFlowEndDate'));


        }
        editPanel.Show();
    }



    function ShowDashboardPreview() {

        UpdateHiddenFields();
        cbPreview.PerformCallback('SaveDashboards');
        $(cbPreview.loadingDivElement).hide();
    }



    function cbPreview_EndCallback(s, e) {
        var fullUrl = "<%= delegateUrl %>" + "<%=viewID%>";
    //window.parent.UgitOpenPopupDialog(fullUrl, "", "", '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
        window.open(fullUrl, '_blank');
    }

    function ShowGlobalFilter() {
        globalFilterPanel.Show();
    }

    function SaveDashboard(e) {

        if ($('[id$="txtTitle"]').val().trim() == '') {
            alert('Enter valid title');
            e.processOnServer = false;
        }
        else {
            UpdateHiddenFields();
            editPanel.Hide();
            e.processOnServer = true;
        }

    }
    function EditLinkDetail(obj) {

        var list = document.getElementById("<%=lnkType.ClientID%>");
    var inputs = list.getElementsByTagName("input");
    $('#dvLinkDetail').hide();
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            switch (inputs[i].value) {
                case 'Document':
                    $("#<%=fileupload.ClientID %>").show();
                    $("#<%=trNavigationType.ClientID%>").show();

                    break;
                case 'Analytic':
                    $("#<%=ddlModel.ClientID %>").show();
                    $("#<%=ddlDashbaord.ClientID %>").show();
                    break;
                case 'Service':
                    $("#<%=ddlService.ClientID %>").show();

                    break;
                case 'Url':
                    $("#<%=txtHelp.ClientID %>").show();
                    $("#<%=trNavigationType.ClientID%>").show();
                    break;
                case 'Wiki':
                    $("#<%=txtHelp.ClientID %>").show();
                    $("#<%=trNavigationType.ClientID%>").show();
                    window.location.href = "<%=WikiPickerUrl%>";

                        break;
                }
                break;
            }
        }
    }

    function lnkType_Change(isOnLoad) {

        var list = document.getElementById("<%=lnkType.ClientID%>"); //Client ID of the radiolist
    var inputs = list.getElementsByTagName("input");
    var selected;
    $('#dvLinkDetail').hide();
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            selected = inputs[i];
            switch (inputs[i].value) {
                case 'Document':
                    $("#<%=txtHelp.ClientID %>").hide();
                    $("#<%=fileupload.ClientID %>").show();
                    $("#<%=ddlService.ClientID %>").hide();
                    $("#<%=ddlService.ClientID %>").val('Select Service');
                    $("#<%=trNavigationType.ClientID%>").show();
                    $("#<%=ddlModel.ClientID %>").hide();
                    $("#<%=ddlDashbaord.ClientID %>").hide();

                    break;
                case 'Analytic':
                    $("#<%=txtHelp.ClientID %>").hide();
                    $("#<%=fileupload.ClientID %>").hide();
                    $("#<%=ddlService.ClientID %>").hide();
                    $("#<%=ddlModel.ClientID %>").show();
                    $("#<%=ddlDashbaord.ClientID %>").show();
                    $("#<%=trNavigationType.ClientID%>").hide();
                    break;
                case 'Service':
                    $("#<%=txtHelp.ClientID %>").hide();
                    $("#<%=fileupload.ClientID %>").hide();
                    $("#<%=ddlService.ClientID %>").show();
                    $("#<%=trNavigationType.ClientID%>").hide();
                    $("#<%=ddlModel.ClientID %>").hide();
                    $("#<%=ddlDashbaord.ClientID %>").hide();
                    break;
                case 'Url':
                    $("#<%=txtHelp.ClientID %>").show();
                    $("#<%=fileupload.ClientID %>").hide()
                    $("#<%=ddlService.ClientID %>").hide();
                    $("#<%=trNavigationType.ClientID%>").show();
                    $("#<%=ddlModel.ClientID %>").hide();
                    $("#<%=ddlDashbaord.ClientID %>").hide();
                    break;
                case 'Wiki':
                    $("#<%=txtHelp.ClientID %>").show();
                    $("#<%=fileupload.ClientID %>").hide();
                    $("#<%=ddlService.ClientID %>").hide();
                    $("#<%=ddlService.ClientID %>").val('Select Service');
                    $("#<%=trNavigationType.ClientID%>").show();
                    $("#<%=ddlModel.ClientID %>").hide();
                    $("#<%=ddlDashbaord.ClientID %>").hide();
                    if (!isOnLoad) {
                        window.location.href = "<%=WikiPickerUrl%>";
                    }
                    break;
                case 'None':
                    $("#<%=txtHelp.ClientID %>").hide();
                    $("#<%=fileupload.ClientID %>").hide()
                    $("#<%=ddlService.ClientID %>").hide();
                    $("#<%=trNavigationType.ClientID%>").hide();
                    $("#<%=ddlModel.ClientID %>").hide();
                    $("#<%=ddlDashbaord.ClientID %>").hide();
                        break;
                }
                break;
            }
        }
    }

    var lastModel = null;
    function OnModelChanged(ddlModel) {
        if (ddlModel.GetValue() == null)
            return;

        if (ddlDashbaord.InCallback())
            lastModel = ddlModel.GetValue().toString();
        else
            ddlDashbaord.PerformCallback(ddlModel.GetValue().toString());
    }
    function OnEndCallback(s, e) {
        if (lastModel) {
            ddlDashbaord.PerformCallback(lastModel);
            lastModel = null;
        }
    }

    function AddEditGloblalFilter() {
        globalFilterEditPanel.Show();
    }

    var lastFactTable = null;
    function OnFactTableChanged(ddlFactTable) {
        if (ddlFactTable.GetValue() == null)
            return;

        if (ddlDashbaord.InCallback())
            lastFactTable = ddlFactTable.GetValue().toString();
        else
            ddlFilterField.PerformCallback(ddlFactTable.GetValue().toString());
    }

    function FilterField_OnEndCallback(s, e) {
        if (lastFactTable) {
            FilterField.PerformCallback(lastModel);
            lastFactTable = null;
        }
    }

    function ShowSizePanel() {
        rblLayoutType_SelectedIndexChanged();
        sizePanel.Show();
    }
    function ShowAuthorizedToView() {
        authorizedToViewPanel.Show();
    }
    function rblLayoutType_SelectedIndexChanged(s, e) {

        if (rblLayoutType.GetSelectedItem() != null && rblLayoutType.GetSelectedItem().value == "FixedSize") {

            $("#<%=txtRPadding.ClientID %>").val('0');
           $("#<%=txtLPadding.ClientID %>").val('0');
           $("#<%=txtTPadding.ClientID %>").val('0');
           $("#<%=txtBPadding.ClientID %>").val('0');

           $("#<%=trViewWidth.ClientID %>").show();
           $("#<%=trViewHeight.ClientID %>").show();
           $("#<%=trRPadding.ClientID %>").hide();
           $("#<%=trLPadding.ClientID %>").hide();
           $("#<%=trTPadding.ClientID %>").hide();
           $("#<%=trBPadding.ClientID %>").hide();
       }
       else {

           $("#<%=txtViewHeight.ClientID %>").val('0');
           $("#<%=txtViewWidth.ClientID %>").val('0');

           $("#<%=trViewWidth.ClientID %>").hide();
           $("#<%=trViewHeight.ClientID %>").hide();
           $("#<%=trRPadding.ClientID %>").show();
           $("#<%=trLPadding.ClientID %>").show();
           $("#<%=trTPadding.ClientID %>").show();
           $("#<%=trBPadding.ClientID %>").show();
        }

    }

    function AddDashboards() {
        dashboardList.Show();
    }

    function ddlBorder_Change(obj) {
        var isLinkorPanel = false;
        if ($('div .dragresize').length > 0) {

            for (var i = 0; i < $('div .dragresize').length; i++) {

                if ($('div .dragresize').get(i).id == $('[id$="hndDashboardName"]').val()) {
                    var objDashboard = $('div .dragresize').get(i);

                    if ($(objDashboard).attr('panelType') == "Link" || $(objDashboard).attr('panelType') == 'Panel') {
                        isLinkorPanel = true;
                        break;
                    }
                }
            }
        }

        if (isLinkorPanel) {
            if ($(obj).val() == 'Rectangle' || $(obj).val() == 'Elliptical') {
                ceBorder.SetColor('#000000');
                ceBackGround.SetVisible(true);
                $('[id$="trBorderColor"]').show();
            }
            else {
                ceBackGround.SetVisible(false);
                ceBackGround.SetColor('');
                $('[id$="trBorderColor"]').hide();
            }
        }
    }

    function checkUser() {
        var rbvalue = $("input[name='<%=rdoUser.UniqueID%>']:radio:checked").val();
       if (rbvalue == "specific") {
           $("#<%=cmbUser.ClientID%>").show();
       }
       else
           $("#<%=cmbUser.ClientID%>").hide();
    }

    function increasePixel(pixel, offset) {
        if (pixel != undefined) {
            var x = parseInt(pixel.replace('px', ''));
            return (x + offset) + 'px';
        }
    }

    function CloneDashboard(obj) {
        var rows = $("[id$='_txtRows']").val();
        var columns = $("[id$='_txtColumns']").val();
        var selectedDB = $("[id^='" + hndDdForCloning.Get('name') + "']");
        if (rows == '' || rows == 0) {
            rows = 1;
        }

        if (columns == '' || columns == 0) {
            columns = 1;
        }

        var ctr = 1;
        for (var i = 0; i < rows; i++) {
            for (var j = 0; j < columns; j++) {
                if (i == 0 && j == 0)
                    continue;
                var obj2 = selectedDB.clone();
                ctr = (i * columns) + (j) + 1;
                var prevCtr
                if ($(obj2).attr('id').indexOf('_') > -1) {
                    var vals = $(obj2).attr('id').split('_');
                    prevCtr = parseInt(vals[vals.length - 1]);
                    ctr = prevCtr + 1;
                }

                $(obj2).attr('id', $(obj2).attr('id').replace('_' + prevCtr, '') + '_' + ctr);
                $(obj2).css({ top: increasePixel($(obj2).css('top'), (i) * (selectedDB.height() + parseInt($("[id$='_txtVSpacing']").val()))), left: increasePixel($(obj2).css('left'), (j) * (selectedDB.width() + parseInt($("[id$='_txtHSpacing']").val()))) });
                $(obj2).find('p').html($(obj2).find('p').html().replace('_' + prevCtr, '') + '_' + ctr)
                $(".droppable").append(obj2);

                $('.dragresize').draggable({
                    //containment: "parent",
                    opacity: 1,
                    scroll: true,
                    cursorAt: { top: 7, left: 130 }
                });

                $(".dragresize").resizable({
                    minHeight: 100,
                    minWidth: 150,
                    grid: 20,
                    start: function (e, ui) {
                        if (ui.element.find('#divID').length == 0) {
                            ui.element.append('<div id="divID" style="width:150px;position:absolute;">150 X 120</div>');
                        }
                    },
                    resize: function (e, ui) {
                        if (ui.element.find('#divID').length > 0) {
                            //ui.element.find('#divID').css('left', ui.element.find('.ui-icon').position().left + 20);
                            ui.element.find('#divID').css('top', ui.element.find('.ui-icon').position().top);
                            ui.element.find('#divID')[0].innerText = Math.floor(ui.size.width) + ' X ' + Math.floor(ui.size.height);
                        }
                    },
                    stop: function (e, ui) {
                    }
                });

            }
        }
        clonePanel.Hide();
    }

    function ShowClonePanel(obj) {
        hndDdForCloning.Set('name', $(obj).parent().attr('id'));
        clonePanel.Show();
    }

    function Duplicate(obj) {
        var obj2 = $(obj).parent().clone();
        var ctr = 1;
        var prevCtr
        if ($(obj2).attr('id').indexOf('_') > -1) {
            var vals = $(obj2).attr('id').split('_');
            prevCtr = parseInt(vals[vals.length - 1]);
            ctr = prevCtr + 1;
        }

        $(obj2).attr('id', $(obj2).attr('id').replace('_' + prevCtr, '') + '_' + ctr);
        $(obj2).css({ top: increasePixel($(obj2).css('top'), 20), left: increasePixel($(obj2).css('left'), 20) });
        $(obj2).find('p').html($(obj2).find('p').html().replace('_' + prevCtr, '') + '_' + ctr)
        $(".droppable").append(obj2);

        $('.dragresize').draggable({
            //containment: "parent",
            opacity: 1,
            scroll: true,
            cursorAt: { top: 7, left: 130 }
        });

        $(".dragresize").resizable({
            minHeight: 100,
            minWidth: 150,
            grid: 20,
            start: function (e, ui) {
                if (ui.element.find('#divID').length == 0) {
                    ui.element.append('<div id="divID" style="width:150px;position:absolute;">150 X 120</div>');
                }
            },
            resize: function (e, ui) {
                if (ui.element.find('#divID').length > 0) {
                    //ui.element.find('#divID').css('left', ui.element.find('.ui-icon').position().left + 20);
                    ui.element.find('#divID').css('top', ui.element.find('.ui-icon').position().top);
                    ui.element.find('#divID')[0].innerText = Math.floor(ui.size.width) + ' X ' + Math.floor(ui.size.height);
                }
            },
            stop: function (e, ui) {
            }
        });
    }

    function Bringtofront(obj) {
        var divobj = $(obj).parent("div");
        var left = $(divobj).position().left;
        var top = $(divobj).position().top;
        var width = $(divobj).width();
        var height = $(divobj).height();

        if ($("div.dragresize") != null && $("div.dragresize").length > 1) {
            var i = 0;
            $("div.dragresize").each(function () {
                if ($(this).attr("id") != $(divobj).attr("id")) {
                    var sleft = $(this).position().left;
                    var stop = $(this).position().top;
                    var swidth = $(this).width();
                    var sheight = $(this).height();
                    if (sleft < left + width && stop < top + height) {
                        i = i + 1;
                        var zindex = 1 + (100 * i);
                        $(this).attr("Zindex", "100");
                        $(this).css("z-index", "100");
                        $(divobj).attr("Zindex", zindex);
                        $(divobj).css("z-index", zindex);
                    }
                }

            });
        }

    }

    function Sendtoback(obj) {
        var divobj = $(obj).parent("div");
        var left = $(divobj).position().left;
        var top = $(divobj).position().top;
        var width = $(divobj).width();
        var height = $(divobj).height();

        if ($("div.dragresize") != null && $("div.dragresize").length > 1) {
            var i = 0;
            $("div.dragresize").each(function () {
                if ($(this).attr("id") != $(divobj).attr("id")) {
                    var sleft = $(this).position().left;
                    var stop = $(this).position().top;
                    var swidth = $(this).width();
                    var sheight = $(this).height();
                    if (sleft < left + width && stop < top + height) {
                        i = i + 1;
                        var zindex = 1 + (100 * i);
                        $(this).css("Zindex", zindex);
                        $(this).css("z-index", zindex);
                        $(divobj).attr("Zindex", "100");
                        $(divobj).css("z-index", "100");
                    }
                }

            });
        }

    }

    $(document).ready(function () {
        $('#<%=ddlModule.ClientID%>').bind('change', function () {
           requestTypeTreeList.PerformCallback();
       })
   });

    function pickSiteAsset(Url) {
        var siteAsset = unescape(Url);
        //$('#<=txtIconImage.ClientID%>').val(siteAsset);
    }
    function pickBackgroundFromLibrary(Url) {
        var siteAsset = unescape(Url);
        $('#<%=txtBackgroundImage.ClientID%>').val(siteAsset);
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .droppable {
        border: 1px solid;
        background-color: #F7F7F7;
        width: 100%;
        height: 1800px;
        float: left;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        width: 500px;
    }

    .ms-formlabel {
        text-align: right;
        width: 150px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
        font-size: 8pt;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    tr.alternet {
        background-color: whitesmoke;
    }

    .services {
        display: none;
    }

    input[type=number] {
        width: 60px;
    }

    .ms-formbody span {
        margin-right: 5px;
    }

        .ms-formbody span select {
            padding: 1px;
        }

    .tabRow {
        width: 100%;
        display: flex;
    }
    .border-right-radius-0 {
        border-top-right-radius: 0px;
        border-bottom-right-radius: 0px;
    }
    .border-left-radius-0 {
        border-top-left-radius: 0px;
        border-bottom-left-radius: 0px;
    }
</style>
<dx:ASPxHiddenField runat="server" ClientInstanceName="hndDashboards" ID="hndDashboards"></dx:ASPxHiddenField>
<dx:ASPxHiddenField runat="server" ClientInstanceName="hndDashboardsList" ID="hndDashboardsList"></dx:ASPxHiddenField>
<dx:ASPxHiddenField runat="server" ClientInstanceName="hndDdForCloning" ID="hndDdForCloning"></dx:ASPxHiddenField>
<div class="d-flex flex-wrap justify-content-between" style="margin-top: 15px; margin-bottom: 15px">
    <div>
        <dx:ASPxButton ID="btnAddDashboards" runat="server" Text="Add Dashboards" CssClass="primary-blueBtn" ImagePosition="Right" AutoPostBack="false">
            <ClientSideEvents Click="function(s,e){AddDashboards();}" />
            <Image Url="/content/Images/add_icon.png"></Image>
        </dx:ASPxButton>

        <dx:ASPxPopupControl ID="dashboardList" runat="server" CloseAction="OuterMouseClick" AllowDragging="true"
            PopupVerticalAlign="Below" PopupHorizontalAlign="RightSides"
            ShowFooter="false" Width="250px" Height="85px" HeaderText="Dashboard List" ClientInstanceName="dashboardList">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssClass="customgridview homeGrid"
                            ClientInstanceName="grid" EnableCallBacks="true" EnableViewState="false"
                            Width="100%" KeyFieldName="ID" OnDataBinding="grid_DataBinding">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Type" CellStyle-HorizontalAlign="Left" GroupIndex="0" HeaderStyle-HorizontalAlign="Center" FieldName="DashboardType" />
                                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                    <DataItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                        <input id="title" type="hidden" value='<%# Eval("Title") %>' />
                                        <input id="panelType" type="hidden" value='<%# Eval("DashboardType") %>' />
                                        <input id="theme" type="hidden" value='<%# Eval("ThemeColor") %>' />
                                        <input id="fontstyle" type="hidden" value='<%# Eval("FontStyle") %>' />
                                        <input id="headerfontstyle" type="hidden" value='<%# Eval("HeaderFontStyle") %>' />
                                        <input id="dashboardSubType" type="hidden" value='<%# Eval("DashboardSubType") %>' />
                                    </DataItemTemplate>
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <SettingsBehavior AutoExpandAllGroups="false" AllowGroup="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                            <Settings GridLines="Horizontal" VerticalScrollBarMode="Auto" VerticalScrollableHeight="480" />
                            <SettingsCookies Enabled="false" />
                            <SettingsPopup>
                                <HeaderFilter Height="200" />
                            </SettingsPopup>
                            <SettingsPager Mode="ShowAllRecords" Position="TopAndBottom">
                            </SettingsPager>
                            <Styles>
                                <Row CssClass="homeGrid_dataRow dashboardpicker-row"></Row>
                                <Header CssClass="homeGrid_headerColumn"></Header>
                            </Styles>
                        </ugit:ASPxGridView>
                        <dx:ASPxGlobalEvents ID="ge" runat="server">
                            <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
                        </dx:ASPxGlobalEvents>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="editPanel" runat="server" CloseAction="CloseButton"
            PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup" MinWidth="500px"
            ShowFooter="false" HeaderText="Edit Dashboard" ClientInstanceName="editPanel">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="ms-formtable">
                            <div class="row" id="trTitle" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtTitle" runat="server" Width="100%" CssClass="asptextbox-asp" />
                                    <div class="crm-checkWrap mt-1" id="spHideTitle">
                                        <asp:CheckBox ID="chkHideTitle" CssClass="ml-n1" runat="server" Text="Hide" />
                                    </div>
                                    <asp:HiddenField ID="hndIsLink" runat="server" Value="0" />
                                    <asp:HiddenField ID="hndDashboardName" runat="server" />
                                    <asp:HiddenField ID="panalType" runat="server" Value="" />
                                </div>
                            </div>
                            <div class="row" id="trTitlePosition" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Title (Top x left) 
                                    </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtTitleTop" runat="server" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">x </span>
                                    <asp:TextBox ID="txtTitleLeft" runat="server" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">px </span>
                                </div>
                            </div>
                            <div class="row" id="tr1" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Border</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <div>
                                        <asp:DropDownList ID="ddlBorder" CssClass=" itsmDropDownList aspxDropDownList" runat="server" onchange="ddlBorder_Change(this)">
                                            <asp:ListItem Value="None">None</asp:ListItem>
                                            <asp:ListItem Value="Rectangle">Rectangle</asp:ListItem>
                                            <asp:ListItem Value="Rounded Rectangle">Rounded Rectangle</asp:ListItem>
                                            <asp:ListItem Value="Elliptical">Elliptical</asp:ListItem>
                                            <asp:ListItem Value="Transparent">Transparent</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <dx:ASPxColorEdit runat="server" Width="100%" AllowUserInput="true" ClientEnabled="true"
                                            ClientInstanceName="ceBackGround" ID="ceBackGround" CssClass="aspxColorEdit-dropDwon"
                                            Color="#ffffff">
                                        </dx:ASPxColorEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="trBorderColor" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Border Color</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <div style="float: left">
                                        <dx:ASPxColorEdit runat="server" Width="100%" AllowUserInput="true" ClientEnabled="true"
                                            ClientInstanceName="ceBorder" ID="ceBorder" CssClass="aspxColorEdit-dropDwon"
                                            Color="#ffffff">
                                        </dx:ASPxColorEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="trFontStyle" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Font Name & Style</h3>
                                </div>
                                <div class="ms-formbody">
                                    <div style="float: left">
                                        <asp:DropDownList ID="ddlFontName" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <asp:DropDownList ID="ddlFontStyle" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                                            <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                            <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                            <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                            <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <asp:DropDownList ID="ddlFontSize" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                                            <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                            <asp:ListItem Value="8pt" Selected="True">8pt</asp:ListItem>
                                            <asp:ListItem Value="10pt">10pt</asp:ListItem>
                                            <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                            <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                            <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                            <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                            <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                            <asp:ListItem Value="36pt">36pt</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <dx:ASPxColorEdit runat="server" AllowUserInput="true" Width="100%" ClientEnabled="true"
                                            ClientInstanceName="ceFont" ID="ceFont" CssClass="aspxColorEdit-dropDwon"
                                            Color="#000000">
                                        </dx:ASPxColorEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="trHeaderFontStyle" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Header Font Name & Style</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <div style="float: left">
                                        <asp:DropDownList ID="ddlHeaderFontName" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <asp:DropDownList ID="ddlHeaderFontStyle" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                            <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                            <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                            <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <asp:DropDownList ID="ddlHeaderFontSize" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                            <asp:ListItem Value="8pt">8pt</asp:ListItem>
                                            <asp:ListItem Value="10pt" Selected="True">10pt</asp:ListItem>
                                            <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                            <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                            <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                            <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                            <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                            <asp:ListItem Value="36pt">36pt</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float: left">
                                        <dx:ASPxColorEdit runat="server" Width="100%" AllowUserInput="true" ClientEnabled="true"
                                            ClientInstanceName="ceHeaderFont" ID="ceHeaderFont" CssClass="aspxColorEdit-dropDwon"
                                            Color="#000000">
                                        </dx:ASPxColorEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6" style="padding:0px;">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Dashboard (Width x Height)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <div class="input-group input-group-append">
                                        
                                    <asp:TextBox ID="txtWidth" runat="server" TextMode="Number" CssClass="asptextbox-asp border-right-radius-0" step="any" />
                                    <asp:DropDownList style="border-left:0px;background-color:#F7F7F7;" ID="ddlWidthUnit" Height="35px" CssClass="asptextbox-asp input-group-text border-left-radius-0" runat="server">
                                        <asp:ListItem Value ="Pixel">px</asp:ListItem>
                                        <asp:ListItem Value="Percentage">%</asp:ListItem>
                                    </asp:DropDownList>
                                        
                                    <span style="padding-left: 2px;">   X </span>
                                    <asp:TextBox ID="txtHeight" runat="server" TextMode="Number" CssClass="asptextbox-asp" step="any" />
                                    <span style="padding-left: 2px;">px </span>
                                    </div>
                                </div>
                                </div>
                             </div>
                            <div class="row">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Dashboard (Top x left) 
                                    </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <div class="input-group input-group-append">
                                    <asp:TextBox ID="txtTop" runat="server" TextMode="Number" CssClass="asptextbox-asp" step="any" />
                                    <span style="padding-left: 2px;">px X </span>
                                    <asp:TextBox ID="txtLeft" runat="server" TextMode="Number" CssClass="asptextbox-asp border-right-radius-0" step="any" />
                                     <asp:DropDownList ID="ddlLeftUnit" Height="35px" CssClass="asptextbox-asp border-left-radius-0" style="border-left:0px;background-color:#F7F7F7;" runat="server">
                                        <asp:ListItem Value ="Pixel">px</asp:ListItem>
                                        <asp:ListItem Value="Percentage">%</asp:ListItem>
                                    </asp:DropDownList>
                                        </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Dashboard (Z-Index) </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtzindex" runat="server" TextMode="Number" CssClass="asptextbox-asp" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trLinkLabel" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Link Type</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:RadioButtonList ID="lnkType" runat="server" RepeatDirection="Horizontal"
                                        onchange="lnkType_Change();" CssClass="custom-radiobuttonlist">
                                        <asp:ListItem Value="Analytic">Analytic</asp:ListItem>
                                        <asp:ListItem Value="Document">Document</asp:ListItem>
                                        <asp:ListItem Value="Service">Service</asp:ListItem>
                                        <asp:ListItem Value="Url" Selected="True">Url</asp:ListItem>
                                        <asp:ListItem Value="Wiki">Wiki</asp:ListItem>
                                        <asp:ListItem Value="None">None</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="row" id="trLinkControl">
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtHelp" runat="server" Width="100%" />
                                    <div id="dvLinkDetail">
                                        <asp:Label ID="lblLinkDetail" runat="server" Visible="true"></asp:Label>
                                        <img style="padding-right: 2px; padding-top: 4px; margin-bottom: -4px;" onclick="EditLinkDetail(this);" src="/content//images/edit-icon.png">
                                    </div>
                                    <asp:DropDownList ID="ddlService" runat="server" CssClass="services" Width="286px"></asp:DropDownList>
                                    <asp:FileUpload ID="fileupload" class="fileupload" Style="display: none" runat="server" />
                                    <div>
                                        <span style="float: left">
                                            <dx:ASPxComboBox runat="server" ID="ddlModel" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith" Width="140px"
                                                EnableSynchronization="False" ClientInstanceName="ddlModel">
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnModelChanged(s); }" />
                                            </dx:ASPxComboBox>
                                        </span>
                                        <span style="float: right">
                                            <dx:ASPxComboBox runat="server" ID="ddlDashbaord" ClientInstanceName="ddlDashbaord" OnCallback="ddlDashbaord_Callback" Width="140px"
                                                DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" EnableSynchronization="False">
                                                <ClientSideEvents EndCallback="OnEndCallback" />
                                            </dx:ASPxComboBox>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="trNavigationType" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Navigation Type</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:RadioButton ID="rbSameWindow" GroupName="NavigationType" Checked="true" runat="server" Text="Navigate" />
                                    <asp:RadioButton ID="rbPopup" runat="server" GroupName="NavigationType" Text="Popup" />
                                    <asp:RadioButton ID="rbNewWindow" runat="server" GroupName="NavigationType" Text="New Window" />
                                </div>
                            </div>
                            <div class="row" id="trImage">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Icon</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                <ugit:UGITFileUploadManager ID="fileUploadIcon" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                                
                                </div>
                            </div>
                            <div class="row" id="trIconShape" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Icon Shape</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList ID="ddlIconShape" runat="server" CssClass=" itsmDropDownList aspxDropDownList">
                                        <asp:ListItem Value="Rectangle">Rectangle</asp:ListItem>
                                        <asp:ListItem Value="Elliptical">Elliptical</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" id="trIconDimension" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Icon (Width x Height)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtIconWidth" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">x </span>
                                    <asp:TextBox ID="txtIconHeight" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">px </span>
                                </div>
                            </div>
                            <div class="row" id="trIconPosition" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Icon (Top x left)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtIconTop" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">x </span>
                                    <asp:TextBox ID="txtIconLeft" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">px </span>
                                </div>
                            </div>
                            <div class="row" id="trPanelPostion" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Panel (Top x left)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtPanelTop" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">x </span>
                                    <asp:TextBox ID="txtPanelLeft" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                    <span style="padding-left: 2px;">px </span>
                                </div>
                            </div>
                            <div class="row" id="trBackground">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Background</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtBackgroundImage" runat="server" CssClass="asptextbox-asp" Width="100%" />
                                    <br />
                                    <asp:LinkButton ID="lnkbackground" runat="server" Font-Size="10px" class="fileupload" Text="PickFromAsset">Pick From Library</asp:LinkButton>
                                    <%--<asp:FileUpload ID="fileuploadBackGround" class="fileupload" runat="server" />--%>
                                </div>
                            </div>
                            <div class="row" id="trParameters">
                                <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="CallbackPanel"
                                    Width="100%">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <div class="ms-formtable accomp-popup">
                                                <div runat="server" id="trParameter" visible="true">
                                                    <div class="ms-formlabel" style="width: 153px;">
                                                        <h3 class="ms-standardheader budget_fieldLabel">Parameters</h3>
                                                    </div>
                                                    <div id="tdParameter" runat="server" class="ms-formbody accomp_inputField"></div>
                                                </div>
                                            </div>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxCallbackPanel>
                            </div>
                            <div class="row" id="trPageSize" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Page Size</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtPageSize" runat="server" CssClass="asptextbox-asp" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trEnableFilter" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Show Filter</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox ID="chkEnableFilter" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trEnableFilterForPredictBacklog" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Show Filter</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox ID="chkPredictBacklog" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trEnableFilterForTicketCreatedByWeek" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Show Filter</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox ID="chkTicketCreatedByWeek" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trWeeklyAverage" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Weekly Average</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtWeeklyAverage" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trEnableFilterScoreCard" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Show Filter</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox ID="chkScoreCard" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trEnableFilterTicketFlow" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Show Filter</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox ID="chkTicketFlow" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trStartDateEndDateFilterScoreCardStartDate">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Start Date:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxDateEdit ID="deStartDateScoreCard" ClientInstanceName="deStartDateScoreCard"
                                        Width="100%" runat="server" DropDownButton-Image-Width="16px" CssClass="CRMDueDate_inputField"
                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                        <DropDownButton>
                                            <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                        </DropDownButton>
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="row" id="trStartDateEndDateFilterScoreCardEndDate" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">End Date:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxDateEdit ID="deEndDateScoreCard" ClientInstanceName="deEndDateScoreCard"
                                        Width="100%" runat="server" DropDownButton-Image-Width="16px" CssClass="CRMDueDate_inputField"
                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                        <DropDownButton>
                                            <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                        </DropDownButton>
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="row" id="trStartEndDateTicketFlowStartDate" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Start Date:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxDateEdit ID="deStartDateTicketFlow" ClientInstanceName="deStartDateTicketFlow"
                                        Width="100%" runat="server" DropDownButton-Image-Width="16px" CssClass="CRMDueDate_inputField"
                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                        <DropDownButton>
                                            <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                        </DropDownButton>
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="row" id="trStartEndDateTicketFlowEndDate" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">End Date:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxDateEdit ID="deEndDateTicketFlow" ClientInstanceName="deEndDateTicketFlow"
                                        Width="100%" runat="server" DropDownButton-Image-Width="16px" CssClass="CRMDueDate_inputField"
                                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                        <DropDownButton>
                                            <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                        </DropDownButton>
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="row" id="trModule" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Module</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList ID="ddlModule" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList" />
                                </div>
                            </div>
                            <div class="row" id="trHideSLAPerformanceTabular" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Hide SLA Table</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox ID="chkbxHideTabular" runat="server" />
                                </div>
                            </div>
                            <div class="row" id="trStatus" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Status</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList">
                                        <asp:ListItem Value="All" Text="All"></asp:ListItem>
                                        <asp:ListItem Value="Open" Text="Open" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Closed" Text="Closed"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" id="trCategoryorSubCategory" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Category</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:HiddenField ID="hdncategoryid" runat="server" />
                                    <asp:HiddenField ID="hdnRequestTypeModule" runat="server" />
                                    <dx:ASPxTreeList ID="requestTypeTreeList" runat="server" Width="100%" CssClass="aspxTreeList"
                                        ClientInstanceName="requestTypeTreeList" AutoGenerateColumns="false"
                                        AutoGenerateServiceColumns="true" KeyFieldName="Id" ParentFieldName="ParentID"
                                        OnCustomCallback="requestTypeTreeList_CustomCallback">
                                        <Columns>
                                            <dx:TreeListDataColumn VisibleIndex="0" FieldName="Title" Caption="Request Type">
                                            </dx:TreeListDataColumn>
                                        </Columns>

                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>

                                        <Styles>
                                            <AlternatingNode CssClass="homeGrid_dataRow treeList-dataRow" Enabled="True"></AlternatingNode>
                                            <Header CssClass="homeGrid_headerColumn"></Header>
                                            <Node CssClass="homeGrid_dataRow treeList-dataRow"></Node>
                                        </Styles>
                                        <SettingsSelection Enabled="True" AllowSelectAll="true" Recursive="true" />
                                    </dx:ASPxTreeList>
                                </div>
                            </div>
                            <%--User--%>
                            <div class="row" id="trUser" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">User</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:RadioButtonList runat="server" ID="rdoUser" CssClass="custom-radiobuttonlist" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="all">All</asp:ListItem>
                                        <asp:ListItem Value="current" Selected="True">Current</asp:ListItem>
                                        <asp:ListItem Value="specific" Enabled="true">Specific</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <dx:ASPxComboBox ID="cmbUser" runat="server" ClientInstanceName="cmbUser"
                                        DropDownStyle="DropDown" IncrementalFilteringMode="Contains" CssClass="aspxComBox-dropDown"
                                        TextField="Name" ValueField="ID" ListBoxStyle-CssClass="aspxComboBox-listBox"
                                        Width="100%">
                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <%--Priority--%>
                            <div class="row" id="trPriority" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Ticket Priority
                                    </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList ID="ddlPriority" runat="server" Width="100%" CssClass="itsmDropDownList aspxDropDownList">
                                        <asp:ListItem Value="all" Text="All"></asp:ListItem>
                                        <asp:ListItem Value="high" Text="High" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="medium" Text="Medium"></asp:ListItem>
                                        <asp:ListItem Value="low" Text="Low"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--Critical--%>
                            <div class="row" id="trIsCritical" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Crucial Tasks Only</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:CheckBox runat="server" ID="chkIsCritical"></asp:CheckBox>
                                </div>
                            </div>

                            <%-- Tabs --%>
                            <div class="row" id="trShowTabDetails">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Show Tab Details</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxCheckBox ID="chkShowTabDetails" runat="server"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countmyproject">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="myproject" runat="server" Text="My Project" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtmyproject" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbmyproject" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkmyprojecticon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countmyopenopportunities">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="myopenopportunities" runat="server" Text="My Open Opportunities" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtmyopenopportunities" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbmyopenopportunities" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkmyopenopportunitiesicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countallopenproject">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="allopenproject" runat="server" Text="All Open Project" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtallopenproject" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmballopenproject" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkallopenprojecticon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countallcloseproject">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="allcloseproject" runat="server" Text="All Close Project" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtallcloseproject" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmballcloseproject" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkallcloseprojecticon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countcurrentopencpr">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="currentopencpr" runat="server" Text="All Open CPR" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtcurrentopencpr" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbcurretopencpr" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkcurrentopencpr" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                             <div class="row" id="countfutureopencpr">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="futureopencpr" runat="server" Text="All future CPR" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtfutureopencpr" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbfutureopencpr" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkfutureopencpr" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="counttotalresource">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="totalresource" runat="server" Text="Total Resource" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txttotalresource" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbtotalresource" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chktotalresource" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countallopenopportunities">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="allopenopportunities" runat="server" Text="All Open Opportunities" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtallopenopportunities" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmballopenopportunities" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkallopenopportunitiesicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countallopenservices">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="allopenservices" runat="server" Text="All Open Services" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtallopenservices" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmballopenservices" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkallopenservicesicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countrecentwonopportunity">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="recentwonopportunity" runat="server" Text="Recent Won Opportunities" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtrecentwonopportunity" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbrecentwonopportunity" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkrecentwonopportunityicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>

                            <div class="row" id="countrecentlostopportunity">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="recentlostopportunity" runat="server" Text="Recent Lost Opportunities" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtrecentlostopportunity" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbrecentlostopportunity" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkrecentlostopportunityicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>

                            <div class="row" id="countwaitingonme">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="waitingonme" runat="server" Text="Waiting on Me" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtwaitingonme" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbwaitingonme" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkwaitingonmeicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>

                            <div class="row" id="countopenticketstoday">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="openticketstoday" runat="server" Text="Tickets Opened Today" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtopenticketstoday" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbopenticketstoday" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkopenticketstodayicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countcloseticketstoday">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="closeticketstoday" runat="server" Text="Tickets Closed Today" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtcloseticketstoday" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbcloseticketstoday" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkcloseticketstodayicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countnprtickets">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="nprtickets" runat="server" Text="New Project Requests" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtnprtickets" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbnprtickets" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chknprticketsicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="countresolvedtickets">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxCheckBox ID="resolvedtickets" runat="server" Text="Resolved Tickets" Width="30%"></dx:ASPxCheckBox>
                                    <dx:ASPxTextBox ID="txtresolvedtickets" runat="server" CssClass="aspxTextBox-inputBox mx-2" Width="60%"></dx:ASPxTextBox>
                                    <dx:ASPxComboBox ID="cmbresolvedtickets" runat="server" Width="10%" ListBoxStyle-CssClass="aspxComboBox-listBox" CssClass="aspxComBox-dropDown">
                                        <Items>
                                            <dx:ListEditItem Text="1" Value="1" />
                                            <dx:ListEditItem Text="2" Value="2" />
                                            <dx:ListEditItem Text="3" Value="3" />
                                            <dx:ListEditItem Text="4" Value="4" />
                                            <dx:ListEditItem Text="5" Value="5" />
                                            <dx:ListEditItem Text="6" Value="6" />
                                            <dx:ListEditItem Text="7" Value="7" />
                                        </Items>

                                        <ListBoxStyle CssClass="aspxComboBox-listBox"></ListBoxStyle>
                                    </dx:ASPxComboBox>
                                    <dx:ASPxCheckBox ID="chkresolvedticketsicon" runat="server" Text="Icon"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <div class="row" id="welcomemsg">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Welcome Screen Message</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxTextBox ID="txtWelcomemsg" runat="server" CssClass="asptextbox-asp w-100"></dx:ASPxTextBox>
                                </div>
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel"></h3>
                                </div>
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkprojectcount" runat="server" Checked="false" Text="Hide Resource Message and show project count" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                            </div>
                              <div class="row" id="divResourceAllocation">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel"></h3>
                                </div>
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkHideResouceAllocationFilter" runat="server" Checked="false" Text="Hide Filter" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkShowCurrentUserDetailsOnly" runat="server" Checked="false" Text="Show Current User Details" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkHideAllocationType" runat="server" Checked="false" Text="Hide Allocation Type" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                            </div>
                            <%--Due Date--%>
                            <div class="row" id="trDueDate" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Due Date</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList runat="server" ID="ddlDueDate" Width="100%" CssClass="itsmDropDownList aspxDropDownList">
                                        <asp:ListItem Value="all">All</asp:ListItem>
                                        <asp:ListItem Value="pastdue">Past Due</asp:ListItem>
                                        <asp:ListItem Value="currentweek">Current Week</asp:ListItem>
                                        <asp:ListItem Value="both" Selected="True">Both (Current Week and Past Due)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" id="helpCards">
                                <div class="ms-formbody accomp_inputField tabRow">
                                    <dx:ASPxGridLookup ID="gvHelpCards" runat="server" SelectionMode="Multiple" TextFormatString="{0}" MultiTextSeparator="," KeyFieldName="TicketId" AutoPostBack="false" Width="100%">
                                        <GridViewProperties>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True"></SettingsBehavior>

                                            <SettingsPopup>
                                                <FilterControl AutoUpdatePosition="False"></FilterControl>
                                            </SettingsPopup>
                                        </GridViewProperties>
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                                            <dx:GridViewDataColumn FieldName="TicketId" />
                                            <dx:GridViewDataColumn FieldName="Title" />
                                            <dx:GridViewDataColumn FieldName="Category" />
                                        </Columns>
                                    </dx:ASPxGridLookup>
                                </div>
                            </div>

                            <div class="row" id="divAllocationConflicts">
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkShowByUsersDivisionAC" runat="server" Checked="false" Text="Show Records by Users Division" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                            </div>

                            <div class="row" id="divUnfilledProjectAllocations">
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkShowByUsersDivisionPrj" runat="server" Checked="false" Text="Show Records by Users Division" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                            </div>

                            <div class="row" id="divUnfilledPipelineAllocations">
                                <div class="ms-formbody">
                                    <dx:ASPxCheckBox ID="chkShowByUsersDivisionPpl" runat="server" Checked="false" Text="Show Records by Users Division" CssClass="asptextbox-asp w-100"></dx:ASPxCheckBox>
                                </div>
                            </div>

                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="UpdateButton" runat="server" Text="Save" AutoPostBack="true"
                                    CssClass="primary-blueBtn" OnClick="UpdateButton_Click"
                                    ClientSideEvents-Click="function(s, e) { SaveDashboard(e); }">
                                    <ClientSideEvents Click="function(s, e) { SaveDashboard(e); }"></ClientSideEvents>
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <LoadingPanelStyle VerticalAlign="Middle" HorizontalAlign="Center"></LoadingPanelStyle>
        </dx:ASPxPopupControl>


        <dx:ASPxPopupControl ID="clonePanel" runat="server" CloseAction="OuterMouseClick"
            PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup"
            ShowFooter="false" Width="300px" Height="85px" HeaderText="Clone Dashboard" ClientInstanceName="clonePanel">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentclonePanel" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="ms-formtable accomp-popup row">
                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">No of Rows</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtRows" runat="server" Width="100%" CssClass="asptextbox-asp" TextMode="Number" step="any" Text="1" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">No of Columns</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtColumns" runat="server" Width="100%" CssClass="asptextbox-asp" TextMode="Number" step="any" Text="2" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Vertical Spacing</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtVSpacing" runat="server" Width="100%" CssClass="asptextbox-asp" TextMode="Number" Text="10" step="any" />
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Horizontal Spacing</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtHSpacing" runat="server" Width="100%" CssClass="asptextbox-asp" TextMode="Number" Text="10" step="any" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12 col-sm-12 col-xs-12 noPadding addEditPopup-btnWrap">
                                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Clone" CssClass="primary-blueBtn" AutoPostBack="false"
                                        ClientSideEvents-Click="function(s, e) { CloneDashboard(e); }" />
                                </div>
                            </div>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <LoadingPanelStyle VerticalAlign="Middle" HorizontalAlign="Center"></LoadingPanelStyle>
        </dx:ASPxPopupControl>
    </div>
    <div class="d-flex flex-wrap">
        <dx:ASPxPopupControl ID="globalFilterPanel" runat="server" CloseAction="CloseButton" CssClass="aspxPopup"
            PopupElementID="lnkGlobalFilter" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
            ShowFooter="false" Width="450px" Height="385px" HeaderText="Global Filter List" ClientInstanceName="globalFilterPanel">
            <ContentCollection>
                <dx:PopupControlContentControl ID="globalFilterPopupContentControl" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="row">
                            <ugit:ASPxGridView ID="gridGlobalFilter" runat="server" AutoGenerateColumns="False"
                                CssClass="customgridview homeGrid" ClientInstanceName="gridGlobalFilter" EnableCallBacks="true"
                                EnableViewState="false"
                                Width="100%" KeyFieldName="ID" OnDataBinding="gridGlobalFilter_DataBinding">
                                <Columns>
                                    <dx:GridViewDataTextColumn Caption=" " FieldName="Change" Width="40px">
                                        <DataItemTemplate>
                                            <div>
                                                <a id="aEdit" runat="server" href="" onload="aEdit_Load">
                                                    <img id="Imgedit" width="16" runat="server" src="/content/images/editNewIcon.png" />
                                                </a>
                                                <input type="hidden" value='<%# Container.KeyValue %>' />
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Title" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" FieldName="Title">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Column" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" FieldName="ColumnName">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ListName" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" FieldName="ListName">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <CellStyle HorizontalAlign="Left"></CellStyle>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowDragDrop="false" AllowGroup="false" AllowSort="false" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                <Settings GridLines="Horizontal" />
                                <SettingsCookies Enabled="false" />
                                <SettingsPopup>
                                    <HeaderFilter Height="200" />
                                </SettingsPopup>
                                <Styles>
                                    <Row CssClass="homeGrid_dataRow"></Row>
                                    <Header CssClass="homeGrid_headerColumn"></Header>
                                </Styles>
                                <SettingsPager Mode="ShowAllRecords" Position="TopAndBottom"></SettingsPager>
                            </ugit:ASPxGridView>
                        </div>
                        <div class="row addEditPopup-btnWrap">
                            <dx:ASPxButton ID="btnNewGlobalFilter" runat="server" Text="New" CssClass="primary-blueBtn"
                                UseSubmitBehavior="false" AutoPostBack="false"
                                ClientSideEvents-Click="function(s, e) { AddEditGloblalFilter(); }" />
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="globalFilterEditPanel" runat="server" CloseAction="OuterMouseClick"
            PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup"
            ShowFooter="false" Width="600px" Height="85px" HeaderText="Edit Dashboard" ClientInstanceName="globalFilterEditPanel">
            <ContentCollection>
                <dx:PopupControlContentControl ID="globalFilterEditPanelContentControl" runat="server">
                    <div class="col-md-12col-sm-12 col-xs-12 noPadding">
                        <div class="ms-formtable accomp-popup">
                            <div class="row" id="trGFTitle" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtGFTitle" runat="server" CssClass="asptextbox-asp" />
                                    <asp:HiddenField ID="hndGlobalFilterID" runat="server" Value="0" />
                                </div>
                            </div>
                            <div class="row" id="tr2" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Item Order<b style="color: Red;">*</b></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtItemOrder" Width="100%" CssClass="asptextbox-asp" runat="server" TextMode="Number"
                                        step="any" />
                                </div>
                            </div>
                            <div class="row" id="tr3" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Fact Table<b style="color: Red;">*</b></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxComboBox runat="server" ID="ddlFactTable" DropDownStyle="DropDownList"
                                        IncrementalFilteringMode="StartsWith" Width="100%" ListBoxStyle-CssClass="aspxComboBox-listBox"
                                        CssClass="aspxComBox-dropDown" EnableSynchronization="False">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnFactTableChanged(s); }" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row" id="tr5" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Filter Field<b style="color: Red;">*</b></h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxComboBox runat="server" ID="ddlFilterField" ClientInstanceName="ddlFilterField"
                                        OnCallback="ddlFilterField_Callback" Width="100%" CssClass="aspxComBox-dropDown"
                                        ListBoxStyle-CssClass="aspxComboBox-listBox" DropDownStyle="DropDown"
                                        IncrementalFilteringMode="StartsWith" EnableSynchronization="False">
                                        <ClientSideEvents EndCallback="FilterField_OnEndCallback" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="btnDeleteGlobalFilter" runat="server" Text="Delete" CssClass="secondary-cancelBtn"
                                    sAutoPostBack="true" OnClick="btnDeleteGlobalFilter_Click"
                                    ClientSideEvents-Click="function(s, e) {if(confirm('Are you sure you want to delete selected filter?')){globalFilterEditPanel.Hide();}else { e.processOnServer =false;return false;}}" />
                                <dx:ASPxButton ID="btnSaveGlobalFilter" runat="server" Text="Save" CssClass="primary-blueBtn"
                                    AutoPostBack="true" OnClick="btnSaveGlobalFilter_Click"
                                    ClientSideEvents-Click="function(s, e) {	globalFilterEditPanel.Hide();}" />
                            </div>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <LoadingPanelStyle VerticalAlign="Middle" HorizontalAlign="Center"></LoadingPanelStyle>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="authorizedToViewPanel" runat="server" CloseAction="CloseButton"
            PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup"
            ShowFooter="false" Width="450px" Height="85px" HeaderText="Authorized To View"
            ClientInstanceName="authorizedToViewPanel">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="ms-formtable accomp-popup">
                            <div class="row" id="tr6" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Authorized To View</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <ugit:UserValueBox ID="peAuthorizedToView" runat="server" CssClass="userValueBox-dropDown"
                                        Width="100%" isMulti="true" />
                                </div>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="btnSaveAuthorizedToView" runat="server" Text="Save" CssClass="primary-blueBtn" OnClick="btnSaveAuthorizedToView_Click"
                                    ClientSideEvents-Click="function(s, e) {authorizedToViewPanel.Hide();}" />
                            </div>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <LoadingPanelStyle VerticalAlign="Middle" HorizontalAlign="Center"></LoadingPanelStyle>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="sizePanel" runat="server" CloseAction="OuterMouseClick"
            PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup"
            ShowFooter="false" Width="450px" Height="85px" HeaderText="Dashboard View Size" ClientInstanceName="sizePanel">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="ms-formtable accomp-popup">
                            <div class="row" id="tr8">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Layout Type</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxRadioButtonList runat="server" Border-BorderWidth="0" ID="rblLayoutType" ClientInstanceName="rblLayoutType"
                                        RepeatColumns="2" CssClass="custom-radiobuttonlist" RepeatDirection="Horizontal" RepeatLayout="Table">
                                        <Items>
                                            <dx:ListEditItem Value="Autosize" Selected="true" Text="Autosize" />
                                            <dx:ListEditItem Value="FixedSize" Text="Fixed Size" />
                                        </Items>
                                        <ClientSideEvents SelectedIndexChanged="rblLayoutType_SelectedIndexChanged" />
                                    </dx:ASPxRadioButtonList>
                                </div>
                            </div>
                            <div class="row" id="trRPadding" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Padding Right (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtRPadding" runat="server" CssClass="asptextbox-asp" Width="100%" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trLPadding" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Padding Left (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtLPadding" CssClass="asptextbox-asp" Width="100%" runat="server" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trTPadding" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Padding Top (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtTPadding" runat="server" CssClass="asptextbox-asp" Width="100%" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trBPadding" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Padding Bottom (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtBPadding" CssClass="asptextbox-asp" Width="100%" runat="server" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trViewHeight" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">View Height (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtViewHeight" runat="server" CssClass="asptextbox-asp" Width="100%" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trViewWidth" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">View Width (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtViewWidth" runat="server" CssClass="asptextbox-asp" Width="100%" TextMode="Number" step="any" />
                                </div>
                            </div>
                            <div class="row" id="trViewBGColor" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Background Color</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxColorEdit ID="ceViewBGColor" Width="100%" ClientInstanceName="ceBGColor" runat="server"
                                        Color="#FFFFFF" CssClass="aspxColorEdit-dropDwon">
                                    </dx:ASPxColorEdit>
                                    <div class="crm-checkWrap" style="padding-top: 15px;">
                                        <asp:CheckBox ID="chkbxThemable" Text="Pick From Theme" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="trViewBackground">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Background Image</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtViewBackGround" CssClass="asptextbox-asp" runat="server" Width="100%" />
                                    <div style="padding-top: 15px;">
                                        <asp:FileUpload ID="fuViewBackground" class="fuViewBackground" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="trOpacity" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Opacity (Min = 0 / Max = 1)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtOpacity" runat="server" CssClass="asptextbox-asp" Width="100%" TextMode="Number" step="any" />

                                </div>
                            </div>
                            <div class="row" id="trBorder" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Border (px)</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtBorderWidth" CssClass="asptextbox-asp" Width="100%" runat="server"
                                        TextMode="Number" step="any" />
                                    <div style="padding-top: 15px">
                                        <dx:ASPxColorEdit ID="ceBorderColor" Width="100%" CssClass="aspxColorEdit-dropDwon" runat="server"
                                            Color="#FFFFFF">
                                        </dx:ASPxColorEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="btnSaveLayout" runat="server" Text="Save" CssClass="primary-blueBtn"
                                    AutoPostBack="true" OnClick="btnSaveLayout_Click"
                                    ClientSideEvents-Click="function(s, e) {sizePanel.Hide();}" />
                            </div>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <LoadingPanelStyle VerticalAlign="Middle" HorizontalAlign="Center"></LoadingPanelStyle>
        </dx:ASPxPopupControl>
        <dx:ASPxButton ID="btnShowAuthorizeView" runat="server" Text="Set Authorized To View" CssClass="primary-blueBtn"
            AutoPostBack="false">
            <ClientSideEvents Click="function(s,e){ShowAuthorizedToView();}" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="btnShowSizePanel" runat="server" Text="View Size" CssClass="primary-blueBtn" AutoPostBack="false">
            <ClientSideEvents Click="function(s,e){ShowSizePanel();}" />
            <%--<Image Url="/content/Images/ruler_16x16.png"></Image>--%>
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnPreview" runat="server" Text="Preview" AutoPostBack="false" CssClass="primary-blueBtn">
            <ClientSideEvents Click="ShowDashboardPreview" />
        </dx:ASPxButton>
        <dx:ASPxCallbackPanel runat="server" ID="cbPreview" ClientInstanceName="cbPreview">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server"></dx:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="cbPreview_EndCallback" />
        </dx:ASPxCallbackPanel>
        <dx:ASPxButton ID="btnGlobalFilter" runat="server" Text="Global Filter" AutoPostBack="false" CssClass="primary-blueBtn">
            <ClientSideEvents Click="function(){ ShowGlobalFilter(); }" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btSaveAs" runat="server" Text="Save As" CssClass="primary-blueBtn">
            <ClientSideEvents Click="function(){ pcSaveAs.Show(); }" />
        </dx:ASPxButton>
        <dx:ASPxPopupControl ID="pcSaveAs" runat="server" CloseAction="OuterMouseClick"
            PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CssClass="aspxPopup"
            ShowFooter="false" Width="450px" Height="85px" HeaderText="Save As" ClientInstanceName="pcSaveAs">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                        <div class="ms-formtable accomp-popup">
                            <div class="row" id="tr4" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Name</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:TextBox ID="txtSaveAsName" runat="server" CssClass="asptextbox-asp" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row addEditPopup-btnWrap">
                                <dx:ASPxButton ID="btnSaveAs" runat="server" Text="Save" AutoPostBack="true" CssClass="primary-blueBtn" OnClick="btnSaveAs_Click"
                                    ClientSideEvents-Click="function(s, e) {pcSaveAs.Hide();}" />
                                <dx:ASPxButton ID="btnSavenClose" runat="server" Text="Save & Close" CssClass="primary-blueBtn" AutoPostBack="true" OnClick="btnSaveAs_Click"
                                    ClientSideEvents-Click="function(s, e) {pcSaveAs.Hide();}" />
                            </div>
                        </div>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <LoadingPanelStyle VerticalAlign="Middle" HorizontalAlign="Center"></LoadingPanelStyle>
        </dx:ASPxPopupControl>
        <dx:ASPxButton ID="lnkSave" runat="server" Text="Save" OnClick="lnkSave_Click" CssClass="primary-blueBtn">
            <ClientSideEvents Click="UpdateHiddenFields" />
        </dx:ASPxButton>

        <dx:ASPxButton ID="lnkDelete" runat="server" Text="Delete" CssClass="primary-blueBtn"
            OnClick="lnkDelete_Click">
            <ClientSideEvents Click="function(s, e){
                if(!confirm('Are you sure you want to delete this view')){ e.processOnServer = false; }
                }" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="primary-blueBtn"
            OnClick="btnCancel_Click">
        </dx:ASPxButton>
    </div>
</div>
<div id="divContainer" runat="server" class="droppable">
</div>


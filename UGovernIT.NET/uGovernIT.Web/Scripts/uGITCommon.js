var colorCodes = ['#00A2C9',
    '#00929E',
    //'#007F6D',
    '#6abcb1',
    //'#006A3A',
    '#60d1cb',
    //'#105401',
    '#45A396',
    '#ECEDED',
    '#BDBDBC',
    '#999A9B',
    //'#737474',
    '#899B9B',
    //'#4A5857',
    '#d0c0c7',
    '#00A2C9',
    '#00929E',
    //'#007F6D',
    '#6abcb1',
    //'#006A3A',
    '#60d1cb',
    //'#105401',
    '#45A396',
    '#ECEDED',
    '#BDBDBC',
    '#999A9B',
    //'#737474',
    '#899B9B',
    //'#4A5857'
    '#d0c0c7'
];

var colorCodesDarkToLight = ['#105401',
    '#006A3A',
    '#007F6D',
    '#00929E',
    '#00A2C9',
    '#4A5857',
    '#737474',
    '#999A9B',
    '#BDBDBC',
    '#ECEDED',
    '#105401',
    '#006A3A',
    '#007F6D',
    '#00929E',
    '#00A2C9',
    '#4A5857',
    '#737474',
    '#999A9B',
    '#BDBDBC',
    '#ECEDED'
];

//var colorCodes = ['#6BA537', '#AAC240', '#BA6047', '#E87726', '#447DA2', '#50A1D6', '#4E5759', '#737373', '#A6A6A6', '#D9D9D9',
//    '#6BA537', '#AAC240', '#BA6047', '#E87726', '#447DA2', '#50A1D6', '#4E5759', '#737373', '#A6A6A6', '#D9D9D9',
//    '#6BA537', '#AAC240', '#BA6047', '#E87726', '#447DA2', '#50A1D6', '#4E5759', '#737373', '#A6A6A6', '#D9D9D9'];

//var colorCodes = ['#D9D9D9', '#A6A6A6', '#737373', '#4E5759', '#E87726', '#E87726', '#50A1D6', '#447DA2', '#AAC240', '#6BA537',
//    '#D9D9D9', '#A6A6A6', '#737373', '#4E5759', '#E87726', '#E87726', '#50A1D6', '#447DA2', '#AAC240', '#6BA537'];

//if (!Array.prototype.filter) {
//    Array.prototype.filter = function (fun /*, thisp*/) {
//        var len = this.length;
//        if (typeof fun != "function")
//            throw new TypeError();
//        var res = new Array();
//        var thisp = arguments[1];
//        for (var i = 0; i < len; i++) {
//            if (i in this) {
//                var val = this[i]; // in case fun mutates this
//                if (fun.call(thisp, val, i, this))
//                    res.push(val);
//            }
//        }
//        return res;
//    };
//}

String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

var closePopupNotificationId = null;
$.cookie("refreshAjaxPanelCallBack", false);
function showTab(liPrefix, tabPrefix, containerPrefix, tabId, tabCount) {
    var selectedContainer = null;
    for (var i = 1; i <= tabCount; i++) {
        var tab = document.getElementById(tabPrefix + i);
        var li = document.getElementById(liPrefix + i);
        var container = document.getElementById(containerPrefix + i);
        if (container && tab) {
            if (i == tabId) {
                tab.className = "tabspan ugitsellinkbg ugitsellinkborder";
                container.style.display = "block";
                selectedContainer = container;

            } else {
                container.style.display = "none";
                tab.className = "tabspan ugitlinkbg";
            }
        }
    }
    RefreshIFrame(selectedContainer);
}
function RefreshIFrame(selectedContainer) {
    var currentDate = new Date();
    if (selectedContainer != null) {
        var frames = $(selectedContainer).find("iframe");
        if (frames.length > 0) {
            for (var i = 0; i < frames.length; i++) {
                var frame = $(frames[i]);
                if (frame.attr("frameurl") != null) {
                    var url = frame.attr("frameurl");
                    if (url.indexOf("PMMSprints") > 0) {
                        _aspxSetCookieInternalDelete("IsPaneExpanded", "", new Date(1970, 1, 1), _spPageContextInfo.webServerRelativeUrl);
                    }
                    frame.attr("src", url + "&timespan=" + currentDate.getTime());
                    var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Loading</div></div></span>";
                    frame.parent().append(loadingElt);
                    frame.parent().css("position", "relative");
                }
            }
        }
    }

}

function callAfterFrameLoad(control) {
    control.height = control.contentWindow.document.documentElement.scrollHeight + 'px'
    $(control).parent().find(".loading111").remove();
    try {
        control.contentWindow.clickUpdateSize();
        control.contentWindow.adjustControlSize();
    } catch (ex) {
    }
}

function adjustIFrameWithHeight(frameId, height) {

    var addHeight = Number($("#" + frameId).attr("addheight"));
    var minheight = Number($("#" + frameId).attr("minheight"));
    if (!isNumber(minheight))
        minheight = 0;

    if (!isNumber(addHeight))
        addHeight = 10;

    if (height && height > 0) {

        var totalHeight = height + addHeight;
        if (minheight > 0 && totalHeight < minheight)
            totalHeight = minheight;

        $("#" + frameId).attr("height", totalHeight + "px");
    }
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function Void(p) {
}

function UgitOpenHTMLPopupDialog(html, titleVal, source, stopRefesh) {
    html = unescape(html).replace(/\+/g, " ").replace(/\~/g, "'");

    var refreshParent = stopRefesh ? 0 : 1;
    var data = new Array();
    data.push("<div id='NewTicketDialog' class='NewTktDialog' title='" + titleVal + "' style='float:left;'>" + html);
    //data.push("<div style='float:right;width:96%;' ><span style='float:right;margin-right:6px;padding:5px 14px;border:1px solid;' class='ugitsellinkbg '><a href='javascript:void(0);' onclick='JqueryDialogClose();'>Close</a></span></div>");
    data.push("<div class='closeDlg'><div class='closeDlg-btnWrap'><a href='javascript:void(0);' onclick='JqueryDialogClose();'>Close</a></div></div>");
    data.push("</div>");

    var htmlObj = $("body").append(data.join(""));
    $("#NewTicketDialog").dialog({
        resizable: false,
        height: 160,
        width: 350,
        modal: true,
        dialogClass: "ugit-dialog",
        close: function (event, ui) {
            $(this).dialog('destroy').remove();
            //Don't refresh page, if New sub Ticket is created, from Edit popup of another Ticket
            if (source != '') {
                window.location.reload();
            }

        }
    });

}


function UgitDialogBox(html, titleVal, source, stopRefesh) {
    html = unescape(html).replace(/\+/g, " ").replace(/\~/g, "'");

    var refreshParent = stopRefesh ? 0 : 1;
    var data = new Array();
    data.push("<div id='NewTicketDialog' class='col-md-12 col-sm-12 col-xs-12 NewTktDialog' title='" + titleVal + "' style='float:left; width:100%;'><div class='row'><p>" + html + "</p></div>");
    //data.push("<div style='float:right;width:96%;' ><span style='float:right;margin-right:6px;padding:5px 14px;border:1px solid;' class='ugitsellinkbg '><a href='javascript:void(0);' onclick='JqueryDialogClose();'>Close</a></span></div>");
    data.push("<div class='row closeDlg'><div class='dialogBox-okBtnWrap'><a class='dialogBox-okBtn' href='javascript:void(0);' onclick='JqueryDialogClose();'>Ok</a></div></div>");
    data.push("</div>");

    var htmlObj = $("body").append(data.join(""));
    $("#NewTicketDialog").dialog({
        resizable: false,
        height: 160,
        width: 350,
        modal: true,
        dialogClass: "ugit-dialog",
        close: function (event, ui) {
            $(this).dialog('destroy').remove();
            //Don't refresh page, if New sub Ticket is created, from Edit popup of another Ticket
            if (source != '') {
                window.location.reload();
            }

        }
    });

}

function htmlPopupDialogClose() {
    popupInsertUpdateMsg.Hide();
}

function JqueryDialogClose() {
    $("#NewTicketDialog").dialog('close');
    return false;
}

function UgitOpenHTMLPopupDialogClose(dialogResult, returnValue) {

    if (dialogResult == 1) {
        var iframes = $("iframe");
        var iframeFound = false;
        for (var i = 0; i < iframes.length; i++) {
            if (iframes[i] != null && $(iframes[i]).attr("src") && $(iframes[i]).attr("src").indexOf(returnValue) >= 0) {
                iframeFound = true;
                var currentDate = new Date();
                var url = $(iframes[i]).attr("src");
                if (url.indexOf('&timespan') >= 0) {
                    var subString = url.substring(url.indexOf('&timespan'), url.indexOf('endtimespan') + 11);
                    url = url.replace(subString, "");
                }
                $(iframes[i]).attr("src", url + "&timespan=" + currentDate.getTime() + "endtimespan");
            }
        }

        if (!iframeFound) {
            document.location.href = document.location.href
            closePopupNotificationId = SP.UI.Notify.addNotification("Loading...", true);
        }
    }
}

function UgitOpenPopupDontRefesh(path, titleVal, width, height) {
    var options = {
        url: path,
        width: width,
        height: height,
        title: titleVal,
        allowMaximize: false,
        showClose: true,
        dialogClass: "ugit-dialog",
        beforeClose: function (event, ui) {

        },
        close: function (event, ui) {

        }
    };
    //SP.UI.ModalDialog.showModalDialog(options);
    // SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options)

}

function _aspxSetCookieInternalDelete(name, value, date, path) {
    document.cookie = escape(name) + "=" + escape(value.toString()) + "; expires=" + date.toGMTString() + "; path= " + path; ///sites/ugovernit";
}

var popupList = [];
function UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefesh, returnUrl) {
    // To clear grid dashboard cookies.
    if (params.indexOf("isDashboard") > 0 || (path.indexOf("kID") > 0 && path.indexOf("dID") > 0)) {
        _aspxSetCookieInternalDelete("Dashboard_Grid_Cookies", "", new Date(1970, 1, 1), window.location.pathname);
        _aspxSetCookieInternalDelete("DashboardWildCardExpression", "", new Date(1970, 1, 1), window.location.pathname);
        _aspxSetCookieInternalDelete("DashboardFromDateExpression", "", new Date(1970, 1, 1), window.location.pathname);
        _aspxSetCookieInternalDelete("DashboardToDateExpression", "", new Date(1970, 1, 1), window.location.pathname);
    }

    var winWidth = 0, winHeight = 0;
    if (parseInt(navigator.appVersion) > 3) {
        if (navigator.appName == "Netscape") {
            winWidth = window.innerWidth;
            winHeight = window.innerHeight;
        }
        if (navigator.appName.indexOf("Microsoft") != -1) {
            winWidth = document.body.offsetWidth;
            winHeight = document.body.offsetHeight;
        }
    }

    var fWidth = winWidth;
    var fHeight = winHeight;

    if (width != "auto") {
        fWidth = (fWidth * width) / 100;
        //fWidth =  fWidth - width;
    }
    if (height != "auto") {
        //fHeight = fHeight - height;
        fHeight = (fHeight * height) / 100;
    }
    width = String(width);
    height = String(height);
    if (width != "auto" && width.indexOf("px") > 0) {
        fWidth = width.substring(0, width.indexOf("px"));
    }
    if (height != "auto" && height.indexOf("px") > 0) {
        fHeight = height.substring(0, height.indexOf("px"));
    }

    //if arguments 7th is true then considers width and height fixed
    if (arguments.length > 7 && arguments[7]) {
        fWidth = width;
        fHeight = height;

        if (fHeight > winHeight)
            fHeight = winHeight;
        if (fWidth > winWidth)
            fWidth = winWidth;
    }


    var startpram = "?";
    if (path.split("?").length > 1) {
        startpram = "&";
    }



    //var path = path + startpram + params + "&Width=" + fWidth + "&Height=" + fHeight;
    path = path + startpram + params + "&Width=" + fWidth + "&Height=" + fHeight;

    if (path.indexOf("isudlg") == -1) {
        path += "&isudlg=1";
        path += "&IsDlg=1";
    }
    path += "&pageTitle=" + titleVal;

    if (returnUrl && returnUrl != "") {
        path += "&source=" + returnUrl;
    }



    if (stopRefesh && (stopRefesh == 1 || stopRefesh == "true")) {
        UgitOpenPopupDontRefesh(path, titleVal, fWidth, fHeight);
    }
    else {

        var frameID = Math.random().toString().replace(".", "");
        //set framepopupid in cookies if popup url contain Ticketid in parameters
        try {
            var ticketID = $.urlParam(path, "TicketId");
            if (ticketID == null)
                ticketID = $.urlParam(path, "PublicTicketId");
            if (ticketID != null && ticketID != "") {
                $.cookie("refreshParent", null, { path: _spPageContextInfo.webServerRelativeUrl });
                $.cookie("framePopupID", frameID, { path: _spPageContextInfo.webServerRelativeUrl });

            }
        } catch (ex) { }

        if (closePopupNotificationId != null) {
            if (navigator.appName != "Microsoft Internet Explorer") {
                window.stop();
            }

        }


        ////var dialogHtml = new Array();
    
        popupList.push(frameID);
    
        var innerFrame = "<div id='" + frameID + "' name='" + frameID + "' style='padding:0px !important;overflow:hidden;' title='" + titleVal + "'><div id='loading_" + frameID + "'></div>" +
            "<iframe  id='iframe_" + frameID + "' class='iframeDialog' width='" + eval(fWidth - 5) + "' height='" + eval(fHeight - 41) + "'  seamless/></div>";

        //$.cookie("hidenext", false);
        //$.cookie("hideprevious", false);


        var dialog = $(innerFrame).dialog({
            resizable: true,
            height: fHeight,
            width: fWidth,
            modal: true,
            position: { my: "center", at: "center", of: window },
            beforeClose: function () {
                //debugger;
                var dataChanged = $.cookie("dataChanged");
                //var projTeamAllocSaved = $.cookie("projTeamAllocSaved");
                if ((params.includes('ConfirmBeforeClose=true') || path.indexOf("ugitmodulewebpart") > 0) && dataChanged == 1) {
                    var customDialog = DevExpress.ui.dialog.custom({
                        title: "Alert",
                        message: "You have some unsaved Changes. Do you want to Close?",
                        buttons: [
                            { text: "Ok", onClick: function () { return "Ok" } },
                            { text: "Cancel", onClick: function () { return "Cancel" } }
                        ]
                    });
                    customDialog.show().done(function (dialogResult) {
                        //debugger;
                        if (dialogResult == "Ok") {
                            $.cookie("dataChanged", 0, { path: "/" });
                            CloseWindowCallback(0, "", path);
                        }
                        else if (dialogResult == "Cancel") {
                            return false;
                        }
                        else
                            return false;
                    });

                    return false;

                }
                //else if (projTeamAllocSaved == 1){
                //    CloseWindowCallback(1, "", path);
                //}
            },
            close: function (event, ui) {
                $.cookie("ticketDivision", null, { path: "/" });
                
                if (typeof returnUrl != "undefined" && returnUrl != "") {
                    var iframes = $("iframe");
                    var returnCtrl = "";
                    try {
                        returnCtrl = new URL(decodeURIComponent(returnUrl));
                    } catch (e) {
                        returnCtrl = "";
                    }
                    if (returnCtrl != "") {
                        var controlname1 = returnCtrl.searchParams.get("control");
                        var controlname2 = returnCtrl.searchParams.get("ctrl");
                        for (var i = 0; i < iframes.length; i++) {
                            var url = $(iframes[i]).attr("src");

                            if (typeof url != "undefined" && (url.includes(controlname1) || url.includes(controlname2))) {
                                var iframeObj = $(iframes[i]);
                                iframeObj.prop('contentWindow').location.reload();
                            }
                        }
                    }
                }
                $(this).dialog("destroy");
                //This will code will excute only CRMProjectAllocationNew popup close
                if (window.parent.location.href.includes("https://localhost:44300/Pages/SeniorEstimatorView")) {
                    GetLeadEstimatorWithProjectDetails();
                }
                if (path.indexOf("ugitmodulewebpart") > 0 || path.indexOf("projectsummary") > 0) {
                    var sourceURL = window.location.href;
                    sourceURL += "**refreshDataOnly";
                    window.parent.CloseWindowCallback(1, sourceURL);
                }
                if (path.indexOf("CRMProjectAllocationNew") > 0 && ASPxCallbackPanel1) {
                    ASPxCallbackPanel1.PerformCallback("");
                }
                if (path.indexOf("newopmwizard") > 0 && ASPxCallbackPanel1) {
                    ASPxCallbackPanel1.PerformCallback("");
                }
                if (path.indexOf("customprojectteamgantt") > 0) {
                   
                    LoadGlobalDataObject();
                    //location.reload(true);  
                }
                if (path.indexOf("crmtemplateallocationview") > 0 && $("li[title=ManageAllocationTemplates]").length > 0) {
                    if ($.cookie('templatenamechanged') != null && $.cookie('templatenamechanged') != "") {
                        $("li[title=ManageAllocationTemplates]").click();
                        set_cookie("templatenamechanged", "");
                    }
                }
                //const string = params;
                //var substring = "configdashboardquery,newdashboardchartui,configdashboardpanel";
                //var splitString = substring.split(',');
               
                //for (var i = 0; i < splitString.length; i++) {
                //    const stringPart = splitString[i];
                //    if (string.includes(stringPart)) {
                //        //var sourceURL = window.location.href;
                //        //sourceURL += "**refreshDataOnly";
                //        //window.CloseWindowCallback(1, sourceURL);
                //        AspxCallbackDashboardPanel.PerformCallback("");
                //        break;
                //    }
                //}
                //console.log(string.includes(substring));
                //if (string.includes(substring))
                //    if (params.split("=")[1].split("&")[0] == "newdashboardchartui") {
                //    CloseWindowCallback(1, "", path);
                //}

                //BTS-22-000934: Reverting to Request List in Error from Timeline. (Entire window is refreshing, after popup close)
                //window.location.reload();
                CloseWindowCallback(0, "", path);
              
            }
        });

        //SPDelta 155(Start:- Survey complete functionality)
        if (typeof (dialog.dialogExtend) != "undefined") {
            //SPDelta 155(End:- Survey complete functionality)
            dialog.dialogExtend({
                "maximizable": true,
                "dblclick": "maximize",
                "icons": { "maximize": "ui-icon-arrow-4-diag" }
            });
            //SPDelta 155(Start:- Survey complete functionality)
        }
        //SPDelta 155(End:- Survey complete functionality)
        iframeObj = $("#" + frameID + " .iframeDialog");
        if (iframeObj.length > 0) {

            if (iframeObj.get(0).contentWindow) {
                //SPDelta 155(Start:- Survey complete functionality)
                if (typeof ($("#loading_" + frameID).dxLoadPanel) != "undefined") {
                    //SPDelta 155(End:- Survey complete functionality)
                    var loadPanel = $("#loading_" + frameID).dxLoadPanel({
                        shadingColor: "",
                        position: { of: "#" + frameID },
                        visible: true,
                        showIndicator: true,
                        showPane: false,
                        shading: true,
                        hideOnOutsideClick: false
                    }).dxLoadPanel("instance");

                    iframeObj.get(0).contentWindow.onunload = function () {
                        setTimeout(function () {
                            loadPanel.hide();
                        }, 500);

                    };
                    //SPDelta 155(Start:-Survey complete functionality)
                }
                //SPDelta 155(End :-Survey complete functionality)
            }
            iframeObj.attr('src', path);
        }

    }
}

function PerformAjaxPanelCallBack() {
    $.cookie("refreshAjaxPanelCallBack", true);
}

function endcallbackevent(s, e) {
    var popupid = String(s.cppopupId);
    eval(popupid).Show();
}

function OnpopupInsertUpdateMsgCloseUp(s, e) {

    var popup = winodw.parent;
    popup.Hide();
}

function CloseWindowCallbackForWiki(dialogResult, returnValue, params) {
    popupList.map(function (x) {
        $("#" + x).dialog('close');
    });
}
function CloseWindowCallback(dialogResult, returnValue, params) {

    var lastPopup = null;
    if (popupList.length > 0) {
        for (var i = 0; i < popupList.length; i++) {
            var returnIframe = document.getElementById("iframe_" + popupList[i]);
            if (returnIframe !== null && returnIframe.src === returnValue)
                lastPopup = popupList[i];
        }
        if (lastPopup === null || lastPopup === "")
            lastPopup = popupList.pop();

        if (lastPopup) {
            $("#" + lastPopup).dialog('destroy');
        }
   
        var refreshAjaxPanelCallBack = $.cookie("refreshAjaxPanelCallBack");
        if (refreshAjaxPanelCallBack) {
            if (refreshAjaxPanelCallBack == "true") {
                AspxCallbackDashboardPanel.PerformCallback("");
                $.cookie("refreshAjaxPanelCallBack", false);
                return;
            }
        }
    }

    //refresh parent page if refreshparent cookie matches with popupid.
    var forceRefreshParent = false;
    var refreshDataOnly = false;
    try {

        var refreshParentCookie = $.cookie("refreshParent");
        var refreshParentID = this.get_args();
        if (refreshParentCookie && refreshParentID && refreshParentCookie != "" && refreshParentID != "" && refreshParentCookie == refreshParentID) {
            forceRefreshParent = true;
            refreshDataOnly = true;
            if ((returnValue === undefined) || returnValue == null || returnValue == "")
                returnValue = unescape($.urlParam(this.$l_0, "source"));

            var pathVal = "";
            if (_spPageContextInfo && _spPageContextInfo != null) {
                pathVal = _spPageContextInfo.webServerRelativeUrl;
            }
            $.cookie("refreshParent", null, { path: pathVal })
            $.cookie("framePopupID", null, { path: pathVal })
        }
    } catch (ex) { }

    if (dialogResult != 0 || forceRefreshParent) {
        var stopRefresh = false;

        //if (returnValue)
        //    returnValue = returnValue.toLowerCase();
        var json;
        var iframes = $("iframe");
        if (!(returnValue === undefined) && returnValue != null) {

            var returnSplit = returnValue.split("**");
            for (var j = 0; j < returnSplit.length; j++) {
                var qVar = $.trim(returnSplit[j]);


                if (qVar.indexOf("stoprefreshpage") != -1) {
                    stopRefresh = true;
                    returnValue = returnValue.replace("**refreshDataOnly", "").replace("**stoprefreshpage", "");
                }
                else if (qVar.indexOf("refreshDataOnly") != -1) {
                    refreshDataOnly = true;
                    returnValue = returnValue.replace("**refreshDataOnly", "");
                  /*  $("iframe").attr('src', returnValue);*/
                }

                // For returning value using jquery.
                if (qVar.indexOf('WIK') > -1 || qVar.indexOf('ReturnValue') > -1 || qVar.indexOf('ScheduleTicketId') > -1 || qVar.indexOf('DocId') > -1 || qVar.indexOf('TicketInfo') > -1 || qVar.indexOf('HLP') > -1) {
                    json = qVar;
                }
            }
        }
        else {
            stopRefresh = true;
        }

        if (!stopRefresh || forceRefreshParent) {

            var iframeFound = false;
            var irelatedticket = false;
            for (var i = 0; i < iframes.length; i++) {

                var url = $(iframes[i]).attr("src");
                try {
                    url = iframes[i].contentWindow.location.href.toLowerCase();
                }
                catch (ex) {
                }

                if (url.indexOf('customticketrelationship') >= 0)
                    irelatedticket = true;
                if (iframes[i] != null && url && (url.indexOf(returnValue) >= 0 || url.indexOf(returnValue.slice(0, -1)) >= 0)) {
                    iframeFound = true;
                    var currentDate = new Date();

                    if (url.indexOf('&timespan') >= 0 && url.indexOf('endtimespan') >= 0) {
                        var subString = url.substring(url.indexOf('&timespan'), url.indexOf('endtimespan') + 11);
                        url = url.replace(subString, "");
                    }

                    if (url.indexOf("#done") != -1) {
                        url = url.replace("TicketId=", "").replace("ticketid=", "").replace("ticketid=", "").replace("#done", "");
                    }

                    $(iframes[i]).attr("src", url + "&timespan=" + currentDate.getTime() + "endtimespan");
                    var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Loading...</div></div></span>";
                    $(iframes[i]).parent().append(loadingElt);
                    $(iframes[i]).parent().css("position", "relative");
                    if ($(iframes[i]).prop('contentWindow').document.readyState === 'complete') {
                        var iframeObj = $(iframes[i]);
                        iframeObj.prop('contentWindow').location.reload();
                        iframeObj.parent().find(".loading111").remove();
                    }
                    else {
                        $(iframes[i]).bind("load", function () {
                            callAfterFrameLoad(this);
                            $(this).parent().find(".loading111").remove();
                        });
                    }
                }
            }

            if (!iframeFound) {
                if (refreshDataOnly) {
                    try {
                        triggerRefresh();
                        return;
                    } catch (ex) {
                        //return;
                    }
                }

                var selectedTab = $(".menu-horizontal a.selected[id^='Tab']");
                var selectTabID = "";
                if (selectedTab.length > 0) {
                    var tabID = selectedTab.get(0).id;
                    selectTabID = $.trim(tabID.replace("Tab_", ""));
                }
                if ($('input[id$=hdnActiveTab]') != null) {
                    if (typeof ($('input[id$=hdnActiveTab]').val()) != "undefined") {
                        selectTabID = $('input[id$=hdnActiveTab]').val();
                    }
                }
                if (irelatedticket)
                    selectTabID = 1;
                var url = document.location.href;
                url = url.replace(/&showTab=[0-9]*/, "");
                if (selectTabID != "") {
                    url = url + "&showTab=" + selectTabID;
                }

                if (url.indexOf("#done") != -1) {
                    url = url.split("?")[0];
                }

                document.location.href = url;
                // closePopupNotificationId = SP.UI.Notify.addNotification("Please wait .. loading page", true);
            }
        }
        else {
            if (json && json.length > 0) {
                json = json.replace(/"/g, "\\\"").replace(/'/g, "\"");
                var obj = $.parseJSON(json.replace(/'/g, "\""));

                try {
                    if (obj.callbackfunction) {
                        eval(obj.callbackfunction);
                    }
                } catch (ex) { }

                if ($("#" + obj.ControlId).length > 0) {
                    $("#" + obj.ControlId).html(unescape(obj.ReturnValue).replace(/@/g, "\'"));

                    try {
                        hdnSkipOnCondition.Set("SkipCondition", unescape(obj.ReturnValue).replace(/@/g, "\'"));
                    } catch (ex) { }
                }
                else {
                    for (var i = 0; i < iframes.length; i++) {
                        var url = $(iframes[i]).attr("src");
                        try {
                            url = iframes[i].contentWindow.location.href;
                        }
                        catch (ex) {
                        }

                        if (iframes[i] != null && url && (url.indexOf(obj.frameUrl) >= 0 || (url.replaceAll('+', ' ')).indexOf(obj.frameUrl) >= 0)) {

                            if ($($(iframes[i]).contents()).find("#" + obj.ControlId).length > 0) {
                                if (obj.WikiId != undefined) {
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val('/Layouts/uGovernIT/DelegateControl.aspx?control=wikiDetails&isHelp=true&ticketId=' + obj.WikiId);
                                }
                                else if (obj.HelpCardId != undefined) {
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val(obj.HelpCardId);
                                }
                                else if (obj.ScheduleTicketId != undefined) {

                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val(obj.ScheduleTicketId);
                                    //Set value on the hyperlink to display it on add/edit ticket popup when user pick any Related Request ID from Picker list
                                    if (obj.ViewControlId != null && obj.ViewControlId != undefined && obj.ViewControlId.length > 0) {

                                        var ViewTicketDetails = obj.ViewTicketDetails.split(";#");
                                        var viewTicketToottip = ViewTicketDetails[0];
                                        var viewTicketText = ViewTicketDetails[1];
                                        var viewTicketURI = decodeURIComponent(ViewTicketDetails[2].replace(/\+/g, '%20'));

                                        if (viewTicketText != null && viewTicketText != undefined && viewTicketText.length > 0)
                                            $($(iframes[i]).contents()).find("#" + obj.ViewControlId).text(obj.ScheduleTicketId + ": " + viewTicketText);
                                        else
                                            $($(iframes[i]).contents()).find("#" + obj.ViewControlId).text(obj.ScheduleTicketId);

                                        $($(iframes[i]).contents()).find("#" + obj.ViewControlId).attr('title', viewTicketToottip);

                                        if (viewTicketURI != null && viewTicketURI != undefined && viewTicketURI.indexOf(obj.ScheduleTicketId) != -1)
                                            $($(iframes[i]).contents()).find("#" + obj.ViewControlId).attr('href', viewTicketURI);
                                        else
                                            $($(iframes[i]).contents()).find("#" + obj.ViewControlId).removeAttr('href');
                                    }
                                    if ($($(iframes[i]).contents()).find("#aticket") != null) {
                                        $($(iframes[i]).contents()).find("#aticket").html(obj.ScheduleTicketId);
                                        $($(iframes[i]).contents()).find("#aticket").attr("href", "javascript:OpenTicketWindow('" + obj.ScheduleTicketUrl + "','" + obj.ScheduleTicketId + "');");
                                    }
                                }
                                else if (obj.DocId != undefined) {

                                    var docHtml = $($(iframes[i]).contents()).find("#" + obj.ControlId).html();
                                    if (obj.IsUpload == "" || obj.IsUpload == "False") {
                                        var arrDocs = obj.DocId.split(";#");
                                        for (var docCount = 0; docCount < arrDocs.length; docCount++) {
                                            if (arrDocs[docCount] != "") {
                                                var arr = arrDocs[docCount].split(";~");
                                                if (arr[0] != "" && arr[1] != "") {
                                                    if (docHtml == "" || (docHtml != "" && docHtml.indexOf(arr[0]) == -1)) {
                                                        var imageUurl = arr[2];
                                                        var popupUrl = (arr[3]);
                                                        var version = (arr[4]);
                                                        var status = arr[5];
                                                        docHtml += "<label runat='server' docId='" + arr[0] + "'>" + arr[1];
                                                        if (obj.WorkFlowHistoryUrl != "" && obj.WorkFlowHistoryUrl != undefined) {
                                                            var workflowHistoryUrlTemp = obj.WorkFlowHistoryUrl + "&DocumentID=" + arr[0] + "&Version=" + version;
                                                            docHtml += "<a status='true' style='color:#3E4F50; cursor:pointer;padding-left:5px;'  onclick='window.parent.UgitOpenPopupDialog(\"" + workflowHistoryUrlTemp + "\",\"TicketId=0\", \"Approval History\", \"60\", \"40\", 0);'>(" + status + ")</a>";
                                                        }
                                                        docHtml += "<img style='padding-left:10px;vertical-align:bottom;cursor:pointer;'  class='deleteimg'  alt='*' src='/Content/images/TrashIcon.png' class='' onclick='event.cancelBubble = true;  ConfirmDeleteDocs(\"" + arr[0] + "\",\"" + arr[1] + "\",this);' />";
                                                        if (imageUurl != "") {
                                                            var tooltipText = "";
                                                            if (imageUurl.indexOf('StartWorkflow') != -1) {
                                                                tooltipText = "Start Review";
                                                                // status="Draft"
                                                                docHtml += "<img title='" + tooltipText + "' AltText='" + status + "' style='padding-left:2px;cursor:pointer;vertical-align:bottom;'  class=''  alt='*' src='" + imageUurl + "' class='' onclick='event.cancelBubble = true; event.stopImmediatePropagation(); StartStopWorkFlow(\"" + status + "\",\"" + version + "\",this);'/> ";
                                                            }
                                                            else if (imageUurl.indexOf('StopWorkflow') != -1) {
                                                                tooltipText = "Stop Review";
                                                                //   status="Pending Approval"
                                                                docHtml += "<img title='" + tooltipText + "' AltText='" + status + "' style='padding-left:2px;cursor:pointer;vertical-align:bottom;'  class=''  alt='*' src='" + imageUurl + "' class='' onclick='event.cancelBubble = true;StartStopWorkFlow(\"" + status + "\",\"" + version + "\",this);'/>";
                                                            }

                                                        }
                                                        docHtml += "<br/></label>";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        var arrFolder = obj.DocId.split(";~");
                                        if (arrFolder[0] != "" && arrFolder[1] != "") {
                                            docHtml = "<label runat='server' FolderId='" + arrFolder[0] + "'>" + arrFolder[1] + "</label>";
                                        }
                                    }

                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).html(docHtml);
                                }
                                else if (obj.ControlId == "txtWorkFlowIcon") {
                                    var retValues = obj.ReturnValue;
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).text(retValues);
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val(retValues);
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).trigger('change');
                                }
                                else if (obj.TicketInfo != undefined) {

                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).text(obj.TicketInfo);
                                    $($(iframes[i]).contents()).find('[id$="_lblTicket"]').text(obj.TicketInfo);
                                    $($(iframes[i]).contents()).find('[id$="_txtTitle"]').val(obj.TicketInfo);
                                    $($(iframes[i]).contents()).find('[id$="hdnTicketId"]').val(obj.TicketInfo);
                                    $($(iframes[i]).contents()).find('[id$="_trTitle"]').hide();
                                    $($(iframes[i]).contents()).find('[id$="_trTicketReadOnly"]').show();
                                    $($(iframes[i]).contents()).find('[id$="_trTicket"]').hide();
                                    if (obj.TicketUrl != undefined) {
                                        $($(iframes[i]).contents()).find("#" + obj.ControlId).attr("TicketUrl", obj.TicketUrl);
                                        $($(iframes[i]).contents()).find('[id$="_lblTicket"]').attr("TicketUrl", obj.TicketUrl);
                                    }
                                }
                                else {
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val(unescape(obj.ReturnValue).replace(/@/g, "\'"));
                                }
                            }

                        }

                    }
                }
            }
        }
    }
}

function getUrlParameter(name) {

    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

function FindPosition(obj) {
    var curleft = curtop = 0;
    if (obj.offsetParent) {
        curleft = obj.offsetLeft
        curtop = obj.offsetTop
        while (obj = obj.offsetParent) {
            curleft += obj.offsetLeft
            curtop += obj.offsetTop
        }
    }
    return { 'Left': curleft, 'Top': curtop };
}


function IsClassExist(elementClass, className) {
    try {
        var elClasses = elementClass.split(" ");
        for (var i = 0; i < elClasses.length; i++) {
            if (elClasses[i].trim().toLowerCase() == className.toLowerCase()) {
                return true;
            }
        }
        return false;
    }
    catch (e) {
        return "";
    }
}

function RemoveClass(elementClass, className) {
    try {
        elementClass = elementClass.replace(" " + className.trim(), "");
        return elementClass;
    }
    catch (e) {
        return "";
    }
}

function AddClass(elementClass, className) {
    try {
        elementClass = elementClass.replace(" " + className.trim(), "");
        elementClass = elementClass + " " + className;
        return elementClass;
    }
    catch (e) {
        return "";
    }
}

function AddStatus(heading, msg, isSuccess) {

    var statusId = SP.UI.Status.addStatus(heading, msg, true);
    if (isSuccess) {
        SP.UI.Status.setStatusPriColor(statusId, 'green');
    }
    else {
        SP.UI.Status.setStatusPriColor(statusId, 'red');
    }

    return statusId;
}

function RemoveLastStatus(statusId) {
    SP.UI.Status.removeStatus(statusId);
}

function RemoveAllStatus() {
    SP.UI.Status.removeAllStatus(true);
}

function AddNotification(msg) {
    //comment by Munna as showing error that sp is not defined
    //return SP.UI.Notify.addNotification(msg, true);

    return;
}

function RemoveNotification(notifyId) {
    SP.UI.Notify.removeNotification(notifyId);
}

function UgitOpenNewDialog(url, Title) {
    window.open(url, Title);
}


function OpenTicket1(path, params, titleVal, width, height) {
    if ("<%=isPopup %>".toLowerCase() == "true") {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, 0);
    }
    else {
        UgitOpenPopupDialog(path, params, titleVal, width, height, 0);
    }
}




function get_cookie(cookie_name) {
    var results = document.cookie.match('(^|;) ?' + cookie_name + '=([^;]*)(;|$)');

    if (results)
        return (unescape(results[2]));
    else
        return null;
}

function delete_cookie(cookie_name) {
    var cookie_date = new Date();  // current date & time
    cookie_date.setTime(cookie_date.getTime() - 1);
    document.cookie = cookie_name += "=; expires=" + cookie_date.toGMTString();
}

function set_cookie(name, value, exps, path, domain, secure) {
    var cookie_string = name + "=" + escape(value);

    if (exps) {
        cookie_string += "; expires=" + (exps * 24 * 1000);
    }

    if (path) {
        cookie_string += "; path=" + escape(path);
    }
    else {
        cookie_string += "; path=/";
    }

    if (domain)
        cookie_string += "; domain=" + escape(domain);

    if (secure)
        cookie_string += "; secure";


    document.cookie = cookie_string;
}

function startWaiting(containerClass) {

    if ($.trim(containerClass) != "") {
        var pos = $("." + containerClass).position();
        var width = $("." + containerClass).width();
        var height = $("." + containerClass).height();
        var innerHeight = eval((height / 2 - 5));
        var innerWidth = eval((width / 2 - 5));
        var loadingText = "<div class='waitdataloading111' style='background:#F4F4F4;opacity:0.6;filter:alpha(opacity=60);position:absolute;width:" + width + "px;height:" + height + "px;top:" + (pos.top + 10) + "px;left:" + (pos.left + 10) + "px'><div style='float:left;padding-left:" + innerWidth + "px;padding-top:" + innerHeight + "px;'><img src='/Content/images/ajax-loader.gif' alt='Please Wait...' title='Please Wait...' /></div></div>";
        $("." + containerClass).append(loadingText);
    }
}

function stopWaiting() {
    $(".waitdataloading111").remove();
}


$.fn.shorten = function (settings) {

    var config = {
        showChars: 100,
        ellipsesText: "...",
        moreText: "more",
        lessText: "less",
        showline: 1,

    };

    if (settings) {
        $.extend(config, settings);
    }

    $(document).off("click", '.morelink');

    $(document).on({
        click: function () {
            var $this = $(this);
            if ($this.hasClass('less')) {
                $this.removeClass('less');
                $this.html(config.moreText);
            } else {
                $this.addClass('less');
                $this.html(config.lessText);
            }
            $this.parent().prev().toggle();
            $this.prev().toggle();
            return false;
        }
    }, '.morelink');

    return this.each(function () {
        var $this = $(this);
        if ($this.hasClass("shortened")) return;

        try {
            var classVal = $this.parent().attr("class");
            var classArgr = classVal.split(" ");
            var length = 0;
            for (var i = 0; i < classArgr.length; i++) {
                if (classArgr[i].indexOf("charlength_") != -1) {
                    length = Number(classArgr[i].replace("charlength_", ""));
                    if (isNaN(length)) {
                        length = 0;
                    }
                    break;
                }
            }

            if (length > 0) {
                showChars = length;
            }
        } catch (ex) {
        }

        $this.addClass("shortened");
        var content = $this.html();
        var contentArry = content.split("<br>");

        var leftContent = "";
        var rightContent = "";

        if (config.showline > 1) {
            for (var i = 1; i <= contentArry.length; i++) {
                if (i < config.showline) {
                    if (i != 1)
                        leftContent += "<br>";
                    leftContent += contentArry[i - 1];
                }
                else if (i == config.showline) {
                    leftContent += "<br>" + contentArry[i - 1].substr(0, config.showChars);
                    rightContent += contentArry[i - 1].substr(config.showChars, content.length - config.showChars);
                }
                else if (i > config.showline) {
                    rightContent += "<br>" + contentArry[i - 1];
                }
            }
        }
        else {
            leftContent = content.substr(0, config.showChars);
            rightContent = content.substr(config.showChars, content.length - config.showChars);
        }

        if (rightContent != "") {
            var html = leftContent + '<span class="moreellipses">' + config.ellipsesText + ' </span><span class="morecontent"><span>' + rightContent + '</span> <a href="#" class="morelink">' + config.moreText + '</a></span>';
            $this.html(html);
            $(".morecontent span").hide();
        }

    });

};

$.urlParam = function (url, name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)', 'gi').exec(url);
    if (results == null) {
        return null;
    }
    else {
        return results[1] || 0;
    }
}

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function deleteDocument(documentid) {
    if (confirm("Are you sure you want to delete attachment?")) {
        if (documentid != '' && documentid != null && documentid != undefined) {
            var dataVar = "{'id':'" + documentid + "', 'name':'" + documentid + "'}";
            var parentdiv = $('#' + documentid).parent();
            var loadingPanel = ASPxClientControl.GetControlCollection().GetByName($('#' + parentdiv[0].id.replace('_imageLinks', '_loadingPanel'))[0].id);
            loadingPanel.Show();
            var msglabel = $('#' + parentdiv[0].id.replace('_imageLinks', '_Label')); $(msglabel)[0].innerHTML = '';
            $.ajax({
                type: "POST",
                url: "/api/Account/DeleteDocument",
                data: dataVar,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response == 'deleted') {
                        var hideField = $('#' + parentdiv[0].id.replace('_imageLinks', '_hiddenField'));
                        var textField = $('#' + parentdiv[0].id.replace('_imageLinks', 'fileUpload_TXTVALUE'));
                        var valueList = $(hideField).val().split(",");
                        //var listofvalue = ""; //valueList.filter(e => e !== documentid);
                        //$(hideField).val(listofvalue.join(","));
                        //$(textField).val(listofvalue.join(","));
                        $('#' + documentid).remove();
                        loadingPanel.Hide();
                    }
                },
                error: function () {
                    loadingPanel.Hide();
                    return false;
                }
            });
        }
    }
}
function ScrollFixedInBottom() {
    if ($('.ugit-stick-bottom').length > 0) {
        var allContainer = $('.ugit-stick-bottom');
        allContainer.each(function (i, item) {
            var container = $(allContainer[i]);
            setTimeout(function () {
                container.scrollToFixed({
                    bottom: 0,
                    limit: container.offset().top
                });
            }, 1000);
        });
    }
}


function getUrlVars() {
    var vars = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
        vars[key] = value;
    });
    return vars;
}

function getUrlParam(parameter, defaultvalue) {
    var urlparameter = defaultvalue;
    if (window.location.href.indexOf(parameter) > -1) {
        urlparameter = getUrlVars()[parameter];
    }
    return urlparameter;
}



function showNextPreviousTicket(navtype) {

    var frameitem;
    var callParentMethod = false;
    var iframelist = window.parent.document.getElementsByTagName("iframe");
    for (let i = 0; i < iframelist.length; i++) {
        frameitem = iframelist[i];
        if (frameitem !== null) {
            var frameNAme = frameitem.getAttribute("frameName");
            if (frameNAme !== null && frameNAme === $.cookie("mytab")) {
                callParentMethod = true;
                break;
            }
        }
    }

    if (callParentMethod)
        callHomePageNextPrevious(navtype, window.frameElement.id, frameitem);
    else
        window.parent.callModulePaggeNextPrevious(navtype, window.frameElement.id);
}
var iframehomePopupId;
function callHomePageNextPrevious(navtype, modelframeid, frameitem) {

    if (frameitem !== null && typeof frameitem !== "undefined") {
        var gridClientInstance = frameitem.contentWindow.ASPxClientControl.GetControlCollection().GetByName('gridClientInstance');
        var length = gridClientInstance.GetVisibleRowsOnPage();
        var index = gridClientInstance.keys.indexOf(gridClientInstance.GetSelectedKeysOnPage()[0]);
        var startindex = length * gridClientInstance.pageIndex;
        var endindex = (length - 1) * (gridClientInstance.pageIndex + 1);
        var hideNext = false;
        var hidePrevious = false;

        var ticketURLPath = "/Pages/";
        if (index >= 0 && index < length) {
            var ticketid = "";
            var nxtIndex = index;
            if (navtype === "next") {
                ticketid = gridClientInstance.keys[index + 1];
                nxtIndex = eval(index + 1);
                if (nxtIndex >= endindex)
                    window.parent.$.cookie("hidenext", true);
                else
                    window.parent.$.cookie("hidenext", false);
                window.parent.$.cookie("hideprevious", false);
            }
            else {
                ticketid = gridClientInstance.keys[index - 1];
                nxtIndex = eval(index - 1);
                if (nxtIndex <= startindex)
                    window.parent.$.cookie("hideprevious", true);
                else
                    window.parent.$.cookie("hideprevious", false);
                window.parent.$.cookie("hidenext", false);
            }

            gridClientInstance.UnselectAllRowsOnPage();
            gridClientInstance.SelectRowsByKey(ticketid);

            iframehomePopupId = modelframeid.replace("iframe_", "");

            $.get(ugitConfig.apiBaseUrl + "api/module/getmoduleshowdetail?TicketId=" + ticketid, function (data) {
                ticketURLPath = ugitConfig.apiBaseUrl + data.ModuleURL + "?TicketId=" + ticketid + "&pageTitle=" + ticketid + "&source=" + "/pages/tsr";

                var returnIframe = window.parent.document.getElementById(modelframeid);
                returnIframe.src = ticketURLPath;

                var parent = $(window.parent.document);
                var dialog = $(parent.find("div [id=" + iframehomePopupId + "]").parent()[0]).find(".ui-dialog-title");
                $(dialog).text(ticketid + ": " + data.TicketTitle);
            });

        }
    }

}


function AlertToSaveChanges() {
    var dataChanged = $.cookie("dataChanged");
    if (dataChanged == 1) {
        alert('You have pending changes. Please Save before proceeding.\nClick OK, then click Save to save changes.');
    }
}

function SingleSelectTokenBox(s, e) {
    if (s.tokens.length > 1)
        s.SetValue(s.tokens[s.tokens.length - 1]);
}
function ContactSingleSelectTokenBox(s, e) {
    if (s.tokens.length > 1) {
        setContactValue(s.values[s.values.length - 1]);
        s.SetValue(s.tokens[s.tokens.length - 1]);
    }
    else {
        setContactValue(s.values);
    }
}

// disabling Enter Key press event, when search word entered in Global Search & pressing Enter key is causing entire Page Reload.
function PreventEnterKeyPressOnButtons() {
    document.onkeypress = function (evt) {
        return ((evt) ? evt : ((event) ? event : null)).keyCode != 13;
    };
}


function getFirstNWords(str, n) {
    return str.split(' ').slice(0, n).join(' ')
}

// Attaching a new function  toUGITDateFormat()  to any instance of Date() class
Date.prototype.toUGITDateFormat = function () {

    const monthNames = ["JAN", "FEB", "MAR", "APR",
        "MAY", "JUN", "JUL", "AUG",
        "SEP", "OCT", "NOV", "DEC"];

    const day = this.getDate();

    const monthIndex = this.getMonth();
    const monthName = monthNames[monthIndex];

    const year = this.getFullYear();

    return `${monthName} ${day}, ${year}`;
}

Date.prototype.toUGITDateFormat = function (dateFormat) {
    const monthNames = ["JAN", "FEB", "MAR", "APR",
        "MAY", "JUN", "JUL", "AUG",
        "SEP", "OCT", "NOV", "DEC"];
    const monthNamesLower = ["Jan", "Feb", "Mar", "Apr",
        "May", "Jun", "Jul", "Aug",
        "Sep", "Oct", "Nov", "Dec"];

    const day = this.getDate();

    const monthIndex = this.getMonth();
    const monthName = monthNames[monthIndex];
    const monthNameLower = monthNamesLower[monthIndex];

    const year = this.getFullYear();

    if (dateFormat === 'dd/mmm/yyyy')
        return `${day}/${monthNameLower}/${year}`;
    else if (dateFormat === 'dd/MMM/yyyy')
        return `${day}/${monthName}/${year}`;
    else
        return `${monthName} ${day}, ${year}`;
}

function SetWidthHeight() {
    set_cookie("screenWidth", $(window).width());
    set_cookie("screenHeight", $(window).height());
}

$(window).resize(function () {
    SetWidthHeight();
});

function addMonths(isoDate, numberMonths) {
    var dateObject = new Date(isoDate),
        day = dateObject.getDate();
    dateObject.setHours(20);
    dateObject.setMonth(dateObject.getMonth() + numberMonths + 1, 0);
    dateObject.setDate(Math.min(day, dateObject.getDate()));
    return dateObject.toISOString().split('T')[0];
};

function getFirstTwoDigitsOfYear(date) {
    let year = date.getFullYear();
    let firstTwoDigits = Math.floor(year / 100);
    return firstTwoDigits;
}

function isTwoDigitNumber(number) {
    // Convert the number to a string
    var numberString = number.toString();

    // Check if the length of the string is 2
    if (numberString.length === 2) {
        return true;
    } else {
        return false;
    }
}


// Test cases
//console.log(removeLeadingZeros(0.00123));  // 0.00123
//console.log(removeLeadingZeros(00123));    // 123
//console.log(removeLeadingZeros(0.000));    // 0
//console.log(removeLeadingZeros(123));      // 123
function removeLeadingZeros(number) {
    // Convert the number to a string
    var numberString = number.toString();

    // Remove leading zeros using regular expression
    var cleanedNumberString = numberString.replace(/^0+/, '');

    // Convert the cleaned string back to a number
    var cleanedNumber = parseFloat(cleanedNumberString);

    return cleanedNumber;
}

function convertToValidDate(dateString) {
    const currentYear = new Date().getFullYear(); // Get the current year
    const currentCentury = Math.floor(currentYear / 100) * 100; // Get the current century

    const dateParts = dateString.split('/');
    const year = parseInt(dateParts[0], 10);

    if (year >= 0 && year <= 99) {
        const fourDigitYear = currentCentury + year;
        return `${fourDigitYear}/${dateParts[1]}/${dateParts[2]}`;
    } else {
        return "Invalid year";
    }
}

//The function takes the input color code, converts it to RGB components, calculates the background delta, and determines the ideal text color based on the delta value.
function idealTextColor(hexColor) {
    const nThreshold = 150;

    // Convert hex color to RGB components
    const hex = hexColor.replace(/^#/, '');
    const bigint = parseInt(hex, 16);
    const r = (bigint >> 16) & 255;
    const g = (bigint >> 8) & 255;
    const b = bigint & 255;

    const bgDelta = Math.round((r * 0.299) + (g * 0.587) + (b * 0.114));
    const colorVal = 255 - bgDelta < nThreshold
    const foreColor = (colorVal) ? "#FFFFFF" : "#000000";
    return foreColor;
}

//the function isColorDark takes a color code and calculates its relative luminance.You can adjust the threshold value to determine your specific criteria for what's considered dark or light.
//A threshold of 0.5 is commonly used, but you can experiment to find the value that works best for your context
//The relative luminance is a value between 0 and 1, where 0 represents black and 1 represents white.
function isColorDark(hexColor) {

    const r = parseInt(hexColor.substr(1, 2), 16);
    const g = parseInt(hexColor.substr(3, 2), 16);
    const b = parseInt(hexColor.substr(5, 2), 16);

    const relativeLuminance = 0.2126 * r / 255 + 0.7152 * g / 255 + 0.0722 * b / 255;

    // Choose a threshold value (e.g., 0.5) to determine if the color is dark or light
    const threshold = 0.25;

    if (relativeLuminance <= threshold)
        return "#777777";
    else
        return "#000000";
}


function isDateValid(dateString) {
    var date = new Date(dateString);
    return dateString != null && date.toString() !== 'Invalid Date' ? true : false;
}

function truncateString(str, maxLength) {
    if (typeof str !== 'string' || typeof maxLength !== 'number') {
        throw new Error('Invalid title.');
    }

    if (str.length <= maxLength) {
        return str;
    } else {
        return str.substring(0, maxLength) + '...';
    }
}

function GetEndDateByWeeks(ajaxHelperPage, startDate, noOfWeeks) {
    let retValue = "";
    if (startDate != null && startDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {
        let paramsInJson = '{' + '"startDate":"' + new Date(startDate).format('MM/dd/yyyy') + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
        $.ajax({
            type: "POST",
            url: ajaxHelperPage + "/GetEndDateByWeeks",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                let resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    retValue = isDateValid(resultJson.enddate) ? new Date(resultJson.enddate) : "";
                }
            },
        });
    }
    return retValue;
}


function GetDurationInWeek(ajaxHelperPage, startDate, endDate) {
    let retValue = "";
    if (startDate != null && startDate != "" && endDate != null && endDate != "") {
        let paramsInJson = '{' + '"startDate":"' + new Date(startDate).format('MM/dd/yyyy') + '","endDate":"' + new Date(endDate).format('MM/dd/yyyy') + '"}';
        $.ajax({
            type: "POST",
            url: ajaxHelperPage + "/GetDurationInWeeks",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                let resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    retValue = resultJson.duration;
                }
            },
        });
    }

    return retValue;
}

function GetTotalWorkingDaysBetween(ajaxHelperPage, startDate, endDate) {
    let retValue = "";
    if (startDate != null && startDate != "" && endDate != null && endDate != "") {
        let paramsInJson = '{' + '"startDate":"' + new Date(startDate).format('MM/dd/yyyy') + '","endDate":"' + new Date(endDate).format('MM/dd/yyyy') + '"}';
        $.ajax({
            type: "POST",
            url: ajaxHelperPage + "/GetTotalWorkingDaysBetween",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                let resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    retValue = resultJson.noOfWorkingDays;
                }
            },
        });
    }
    return retValue;
}
function GetEndDateByWorkingDays(ajaxHelperPage, startDate, noOfWorkingDays) {
    let retValue = "";
    if (startDate != null && startDate != "" && noOfWorkingDays != null && noOfWorkingDays != "") {
        var paramsInJson = '{' + '"startDate":"' + new Date(startDate).format('MM/dd/yyyy') + '","noOfWorkingDays":"' + noOfWorkingDays + '"}';
        $.ajax({
            type: "POST",
            url: ajaxHelperPage + "/GetEndDateByWorkingDays",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                var resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    retValue = resultJson.enddate;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    return retValue;
}

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}
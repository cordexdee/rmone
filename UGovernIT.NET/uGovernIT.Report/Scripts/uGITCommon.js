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
                    var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Please wait .. loading page</div></div></span>";
                    frame.parent().append(loadingElt);
                    frame.parent().css("position", "relative");
                }
            }
        }
    }

}

function callAfterFrameLoad(control) {
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
    data.push("<div id='NewTicketDialog' class='col-md-12 col-sm-12 col-xs-12 NewTktDialog' title='" + titleVal + "' style='float:left; width:100%;'><div class='row'><p>" + html +"</p></div>");
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
            closePopupNotificationId = SP.UI.Notify.addNotification("Please wait .. loading page", true);
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
                ticketID= $.urlParam(path, "PublicTicketId");
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
            "<iframe  id='iframe_" + frameID + "' class='iframeDialog' width='" + eval(fWidth - 5) + "' height='" + eval(fHeight - 31) + "'  seamless/></div>";

      


        var dialog = $(innerFrame).dialog({
            resizable: true,
            height: fHeight,
            width: fWidth,
            modal: true,
            position: { my: "center", at: "center", of: window },
            close: function (event, ui) {
                // window.location.reload(true);
                //var currenUrl = window.top.location.href.split('?');
                // if (currenUrl.length > 1) {
                //    if (currenUrl.length > 1 && currenUrl[1] == "frommail") {
                //        window.top.location.href = "Pages/UserHomePage";
                //    }
                //    if (currenUrl.length > 1 && currenUrl[1] == "fromMail&mnu")
                //    {
                //        window.top.location = currenUrl[0];
                //    }
                //}
                //if (currenUrl.length > 1 && currenUrl[1] == "frommail")
                //{
                //    window.top.location.href = "Pages/UserHomePage";
                //}
                //
                //if (currenUrl.length > 1) {
                //    window.top.location = currenUrl[0];
                //}

                $(this).dialog("destroy");
            }
        });
        
        //SPDelta 155(Start:- Survey complete functionality)
        if (typeof (dialog.dialogExtend) != "undefined") {
            //SPDelta 155(End:- Survey complete functionality)
        dialog.dialogExtend({
            "maximizable": true,
         "dblclick" : "maximize",
         "icons" : { "maximize": "ui-icon-arrow-4-diag" }
            });
            //SPDelta 155(Start:- Survey complete functionality)
        }
        //SPDelta 155(End:- Survey complete functionality)
        iframeObj = $("#" + frameID + " .iframeDialog");
        if (iframeObj.length > 0) {
            
            if (iframeObj.get(0).contentWindow) {
               //SPDelta 155(Start:- Survey complete functionality)
                if (typeof($("#loading_" + frameID).dxLoadPanel) != "undefined") { 
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
            if (returnIframe!==null && returnIframe.src === returnValue)
                lastPopup = popupList[i];
        }
        if (lastPopup === null || lastPopup === "")
            lastPopup = popupList.pop();

        if (lastPopup) {
            $("#" + lastPopup).dialog('destroy');
        }
    }

    //refresh parent page if refreshparent cookie matches with popupid.
    var forceRefreshParent = false;
    try {
       
        var refreshParentCookie = $.cookie("refreshParent");
        var refreshParentID = this.get_args();
        if (refreshParentCookie && refreshParentID && refreshParentCookie != "" && refreshParentID != "" && refreshParentCookie == refreshParentID) {
            forceRefreshParent = true;
            if ((returnValue === undefined) || returnValue == null || returnValue =="")
                returnValue = unescape($.urlParam(this.$l_0, "source"));

            var pathVal = "";
            if (_spPageContextInfo && _spPageContextInfo != null) {
                pathVal = _spPageContextInfo.webServerRelativeUrl;
            }
            $.cookie("refreshParent", null, { path: pathVal })
            $.cookie("framePopupID", null, { path: pathVal })
        }
    } catch (ex)
    { }
   
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
                }

                // For returning value using jquery.
                if (qVar.indexOf('WIK') > -1 || qVar.indexOf('ReturnValue') > -1 || qVar.indexOf('ScheduleTicketId') > -1 || qVar.indexOf('DocId') > -1 || qVar.indexOf('TicketInfo') > -1) {
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
                    var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Please wait .. loading page</div></div></span>";
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

                        if (iframes[i] != null && url && url.indexOf(obj.frameUrl) >= 0) {

                            if ($($(iframes[i]).contents()).find("#" + obj.ControlId).length > 0) {
                                if (obj.WikiId != undefined) {
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val('/Layouts/uGovernIT/DelegateControl.aspx?control=wikiDetails&isHelp=true&ticketId=' + obj.WikiId);
                                }
                                else if (obj.ScheduleTicketId != undefined) {
                                    $($(iframes[i]).contents()).find("#" + obj.ControlId).val(obj.ScheduleTicketId);
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
                    leftContent += contentArry[i-1];
                }
                else if (i == config.showline) {
                    leftContent +="<br>"+ contentArry[i - 1].substr(0, config.showChars);
                    rightContent += contentArry[i - 1].substr(config.showChars, content.length - config.showChars);
                }
                else if( i > config.showline)
                {
                    rightContent +="<br>"+ contentArry[i - 1];
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
        allContainer.each(function (i,item) {
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



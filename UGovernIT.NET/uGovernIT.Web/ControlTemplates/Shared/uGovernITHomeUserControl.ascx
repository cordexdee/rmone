
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITHomeUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITHomeUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>
<script src="../../Scripts/HelpCardDisplayPopup.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
.modulemainpanel{
    width:888px; 
    /*padding-bottom:10px;*/
}
.mpaneldiv{float:left;height:145px;width:303px;}
.mborderdiv{float:left;background:url("/Content/images/bordershadow.png");cursor:pointer;}
.mcontentdiv{float:left;margin-left:6px;margin-top:7px;cursor:pointer;}
.mbottmlinktd{padding:0 10px 5px 10px;}
.mcontentdetailinner{float:left;padding-top:4px;height:96%;width:100%;}

.uborderdiv{float:left;cursor:pointer;}
.ucontentdiv{float:left;height:18px;float:left;padding:1px 6px 0;margin:2px 4px;}
.uborderdicselected{position:relative;top:-1px;left:-1px;}
.upanelbottomshadow{background:url("/Content/images/panel_bottomshadow.png") no-repeat;float:left;width:inherit;}
.myticketpaneldiv{float:left;}
.mytickettext{float:left;font-weight:bold;left:4px;position:relative;top:7px;background-position:0 -5px;height:16px;padding:0px 2px;background:#fff;}
.myticketcontent{/*border:1px solid #eeecea;*/float:left;padding:9px 0;}

<%--.myticketpanel{float:left;display:none;}--%>
.myticketpanel{float:left;}
/*.myticketinner1{float:left;min-width:100px;width:100%;}*/
/*.myticketinner2{float:left;min-width:100px;margin-left:3px;width:99.6%;}*/

.backgroundwhite{background:#fff !important;}
.backgroundtrans{background:transparent !important;}
/*.homecontent{float:left; background-color:#F3F4F5}*/

.tab-div{float:left;position:relative;}
.tab-active{top:1px;}
.tab-span{border:1px solid #cacaca;float:left;margin:0 2px;padding:4px;background:#fff;border-bottom:none;font-weight:bold;}
.tab-span1{border:1px solid #cacaca;}
/*.tab-contentdiv{float:left;width:inherit;}*/
.tab-contentdiv1{border:1px solid #CACACA;float:left;margin:0 0px;padding:2px 2px 2px 5px;width:inherit;}
.tab-footerdiv{background:url("/Content/images/myticket_smallshadow.png"); height:5px;float:left;margin:0 2px;width:98.6%;}
.tab-contentdiv2{float:left;width:99%;}

.mytickettextdiv{float:left;position:relative;}
.mytickettextdiv1{top:0px;}
.mytickettextspan{border:1px solid #cacaca;float:left;margin:0 2px;padding:4px;background:#fff;border-bottom:none;font-weight:bold;}
.mytickettextspan1{border:1px solid #cacaca;}
.myticketContentdiv{float:left;width:inherit;}
.myticketContentdiv1{border:1px solid #CACACA;float:left;margin:0 2px;padding:2px;width:inherit;}
.myticketfooterdiv{background:url("/Content/images/myticket_smallshadow.png"); height:5px;float:left;margin:0 2px;width:98.6%;}
.myticketContentdiv2{float:left;width:99%;}

.paddingleft8{padding-left:8px;}
.ticketpreviewdiv{float:left;width:100%;padding-left:2px;/* max-height:150px;overflow:scroll;overflow-x:hidden;overflow-y:auto;*/}
  .help-container{position:absolute;right:10px;}
  .tabcontent-container{min-width:910px;width:100%;}
  .modulelist-container{float:left;width:910px;}

 
   .hide
    {
       display:none;
    }

    .action-container
        {
         
            background: none repeat scroll 0 0 #FFFFAA;
            border: 1px solid #FFD47F;
            float: left;
            padding: 1px 5px 0;
            position: absolute;
            z-index: 1000;
            margin-top:-4px;
            margin-left:3px;
            right:0px;
            top:0px;
        }

        /*<% if (Request["IsDlg"] != null && Request["IsDlg"].ToString() == "1")
           { %>
  
                 body #s4-ribbonrow {display:none;} 
        <% } %>
        <% else
           { %>
     
                body #s4-titlerow {display:none;} 
                body #s4-leftpanel {display:none;} 
                body #MSO_ContentTable {margin-left:0px !important;} 
        <% } %>*/

    
             /*.dxeListBox_UGITNavyBlueDevEx div.dxlbd {
                height: auto !important;
                font-family: 'Poppins', sans-serif;
                }*/
             .dxeListBoxItemRow_UGITNavyBlueDevEx .dxeListBoxItem_UGITNavyBlueDevEx{
                background: #f1f1f1;
                padding: 5px 10px;
                height: auto;
                font-size: 14px;
             }

             /*.dxeButtonEdit_UGITNavyBlueDevEx{
                 width: 15%;
                padding: 10px;
                height: auto;
                font-size: 14px !important;
                margin-bottom: 20px;
                border:1px solid  #CACACA;
    
             }*/
             .dxeListBoxItemRow_UGITNavyBlueDevEx .dxeListBoxItem_UGITNavyBlueDevEx:hover {
    background: #03A9F4;
    color: #fff;
}
             .dxtc-leftIndent, .dxtc-rightIndent {
        border-bottom: 0px solid #9da0aa !important;
        width:0px !important;
    }
    .aspxComboBox-dbDropDown .dxtc-stripContainer .dxtc-activeTab a {
    color: #4A6EE2 !important;
    background-color: white;
    }
    .dxtcLite.dxtc-top > .dxtc-stripContainer {
    padding-top: 1px;
    }
    .dxtcLite > .dxtc-stripContainer .dxtc-link {
    padding: 6px 12px 7px;
    display: block;
    height: 100%;
    color: #333333;
    background-color: #F9F9F9;
    }
    .dxtcLite > .dxtc-stripContainer .dxtc-tabHover .dxtc-link {
    background-color: #FFFFFF;
    color:blue;
    filter: brightness(120%);
    }
    .svcDashboard_actionBtn {
    background: #4fa1d6;
    color: #fff !important;
}
    .svcDashboard_actionBtn div.dxb img.dx-vam {
    margin-top: 3px;
    max-width: 11px;
}
   .ugit-trcnoti-toast {
    text-align: center;
}
   
    
</style>
<asp:PlaceHolder runat="server">
    <%= UGITUtility.LoadStyleSheet("/Content/UGITCommonTheme.css") %>
    <%= UGITUtility.LoadStyleSheet("/Content/UGITNewTheme.css") %>
</asp:PlaceHolder>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);
    prm.add_pageLoaded(pageHomeLoadedHandler);

    function BeginRequestHandler(sender, args) {
        try{
            if(sender._postBackSettings != null)
            {
                var sourceEle=$(sender._postBackSettings.sourceElement);
                var isShow=true;
                var sourceUpdatePanel ="";
                if(sender._postBackSettings.panelsToUpdate.length>0)
                {
                    sourceUpdatePanel = sender._postBackSettings.panelsToUpdate[0];
                }
                if((sourceEle.length > 0 && sourceEle.attr("DownloadOnly")=="True") || ( sourceUpdatePanel != null && sourceUpdatePanel.indexOf("categoryUpdatePanel") != -1 ))
                {
                    isShow=false;;
                }

                if(isShow)
                {
                    LoadingPanel.Show();
                }
            }
        }catch(ex)
        {
        }
    }

    function EndRequest(sender, args) {
        if(sender._postBackSettings != null)
        {
            var updatePanelId = sender._postBackSettings.panelsToUpdate[0];
            if(updatePanelId == "<%= upHomePanel.UniqueID%>" || updatePanelId.indexOf("subTicketUpdatePanel") != -1)
            {
                LoadingPanel.Hide();
                initializeDefault();
            }
        }
    }

    function pageHomeLoadedHandler(sender, args) {
        if (sender._postBackSettings != null) {
            var updatePanelId = sender._postBackSettings.panelsToUpdate[0];
            if (updatePanelId == "<%= upHomePanel.UniqueID%>") {
                try{
                    var scriptData = $("#myhometabs").find("script").text();
                    scriptData = scriptData.replace(/<!--/g, "").replace(/-->/g, "");
                    $.globalEval(scriptData);
                   
                }
                catch(ex)
                {
                }
            }
        }
    }

    
    function ShowModuleDesc(panelId) {
        $("#mDesc" + panelId).show();
    }

    function HideModuleDesc(panelId) {
        $("#mDesc" + panelId).hide();
    }

    function SetSelectedMyTicketLink(val) {
        var setlink = document.getElementById("<%=selectedMyTicketLink.ClientID %>");
        var tabID = $("#<%= tabID.ClientID %>");
        tabID.val("mytickets");
        set_cookie("mytab", "mytickets");
        if (setlink) {
            setlink.value = val;
            return true;
        }
    }

    


    function changeMyTaskView(viewType) {
        $("#myTaskViewType").val(viewType);
        set_cookie("mytaskviewtype", viewType);
        return true;
    }

    // Ticket Panel
    function HideShowTicketPanel(obj){
        if($(".myticketpanel").is(':hidden') == true)
        {
            ShowTicketPanel();
            $.cookie("ShowTicketPanel", 1,{ expires : 9999 });
        }
        else
        {
            HideTicketPanel();
            $.cookie("ShowTicketPanel", 0,{ expires : 9999 });
        }
    }

    function ShowTicketPanel(){
        if( $("#imgHideShowTicketPanel") != undefined)
        {
            $(".myticketpanel").show()
            $("#spTicketPanelHeader").hide();
            $("#imgHideShowTicketPanel").attr('src', '/Content/Images/Minus_16x16.png');
            $('#<%=cMyTicketPanel.ClientID%>').css('border-bottom', 'none');
            $("#imgHideShowTicketPanel").css('position', 'absolute');
            $("#imgHideShowTicketPanel").css('left', $('.myticketpanel').width() - 30 + 'px');
           
            $("#imgHideShowTicketPanel").hide();           
        }
    }

    function HideTicketPanel(){
        if( $("#imgHideShowTicketPanel") != undefined)
        {
            $(".myticketpanel").hide()
            $("#spTicketPanelHeader").show();
            $("#imgHideShowTicketPanel").show();
            // $('#<%=cMyTicketPanel.ClientID%>').css('border-bottom', '1px solid #D8D8D8');
            $("#imgHideShowTicketPanel").css('position', 'relative');
            $("#imgHideShowTicketPanel").css('left', '0px');          
            $("#imgHideShowTicketPanel").attr('src', '/Content/Images/Plus_16x16.png');
        }
    }

    // Service Catalog
    function HideShowServiceCatalog(obj){
        var ctr = $('#<%=panelServiceCatalog.ClientID%>');
        if(ctr.is(':hidden') == true)
        {
            ShowServiceCatalog();
            $.cookie("ShowServiceCatalog", 1,{ expires : 9999 });
        }
        else
        {
            HideServiceCatalog()
            $.cookie("ShowServiceCatalog", 0,{ expires : 9999 });
        }
    }

    function ShowServiceCatalog(){
        if( $("#imgHideShowServiceCatalog") != undefined)
        {
            $('#<%=panelServiceCatalog.ClientID%>').show()
            $("#imgHideShowServiceCatalog").attr('src', '/Content/Images/Minus_16x16.png');
            $("#imgHideShowServiceCatalog").css('position', 'absolute');
            $('#<%=cServiceCatalog.ClientID%>').css('border-bottom', 'none');
            $("#spServiceCatalog").hide();
            $("#imgHideShowServiceCatalog").hide();  
            $("#imgHideShowServiceCatalog").css('left',  $('#<%=panelServiceCatalog.ClientID%>').width() - 15 + 'px');          
        }
    }

    function HideServiceCatalog(){
        if( $("#imgHideShowServiceCatalog") != undefined)
        {
            $('#<%=panelServiceCatalog.ClientID%>').hide()
            $("#imgHideShowServiceCatalog").show();
            $("#imgHideShowServiceCatalog").attr('src', '/Content/Images/Plus_16x16.png');
            $("#imgHideShowServiceCatalog").css('position', 'relative');
            // $('#<%=cServiceCatalog.ClientID%>').css('border-bottom', '1px solid #D8D8D8');
            $("#spServiceCatalog").show();  
            $("#imgHideShowServiceCatalog").css('left', '0px');          
        }
    }


    // Module Panel
    function HideShowModulePanel(obj){
        var ctr = $('#<%=moduleListViewPanel.ClientID%>');
        if(ctr.is(':hidden') == true)
        {
            ShowModulePanel();
            $.cookie("ShowModulePanel", 1,{ expires : 9999 });
        }
        else
        {
            HideModulePanel();
            $.cookie("ShowModulePanel", 0,{ expires : 9999 });
        }
    }

    function ShowModulePanel(){

        if($('#imgHideShowModulePanel') != undefined)
        {
            $('#<%=moduleListViewPanel.ClientID%>').show()
            $("#imgHideShowModulePanel").attr('src', '/Content/Images/Minus_16x16.png');
            $("#imgHideShowModulePanel").css('position', 'absolute');           
            $('#<%=cModulePanel.ClientID%>').css('border-bottom', 'none');
            $("#spModulePanel").hide();   
            $("#imgHideShowModulePanel").hide(); 
            $("#imgHideShowModulePanel").css('left',  $('#<%=moduleListViewPanel.ClientID%>').width() - 35 + 'px');       
        }
    }

    function HideModulePanel(){
        if($('#imgHideShowModulePanel') != undefined)
        {
            $('#<%=moduleListViewPanel.ClientID%>').hide()
            $("#imgHideShowModulePanel").show();
            $("#imgHideShowModulePanel").attr('src', '/Content/Images/Plus_16x16.png');
            $("#imgHideShowModulePanel").css('position', 'relative');          
            //$('#<%=cModulePanel.ClientID%>').css('border-bottom', '1px solid #D8D8D8');
            $("#spModulePanel").show(); 
            $("#imgHideShowModulePanel").css('left',  '0px');       
      
           
        }
    }  

    function initializeDefault()
    {
        var ctrModulePanel = $('#<%=moduleListViewPanel.ClientID%>');
        var ctrServiceCatalog = $('#<%=panelServiceCatalog.ClientID%>');


        if ($.cookie("ShowModulePanel") == '0') {
            HideModulePanel();    
        }
        else {           
            ShowModulePanel();
        }

        if ($.cookie("ShowServiceCatalog") == '0') {
            HideServiceCatalog();
        }
        else {
            ShowServiceCatalog();
        }

        $('#<%=moduleListViewPanel.ClientID%>').hover(
            function () {
                $("#imgHideShowModulePanel").show();
            }, function () {
                if ($('#<%=moduleListViewPanel.ClientID%>').is(':hidden') == false) {
                    $("#imgHideShowModulePanel").hide();
                }
            });

        $('#imgHideShowModulePanel').hover(
     function () {
         $("#imgHideShowModulePanel").show();
     }, function () {
         if ($('#<%=moduleListViewPanel.ClientID%>').is(':hidden') == false) {
             $("#imgHideShowModulePanel").hide();
         }
     });

        $('#<%=panelServiceCatalog.ClientID%>').hover(
     function () {
         $("#imgHideShowServiceCatalog").show();
     }, function () {
         if ($('#<%=panelServiceCatalog.ClientID%>').is(':hidden') == false) {
             $("#imgHideShowServiceCatalog").hide();
         }
     });

        $('#imgHideShowServiceCatalog').hover(
     function () {
         $("#imgHideShowServiceCatalog").show();
     }, function () {
         if ($('#<%=panelServiceCatalog.ClientID%>').is(':hidden') == false) {
             $("#imgHideShowServiceCatalog").hide();
         }
     });

        if ($.cookie("ShowTicketPanel") == '0') {
            HideTicketPanel();
        }
        else {
            ShowTicketPanel();
        }

        $('.myticketpanel').hover(
      function () {
          $("#imgHideShowTicketPanel").show();
      }, function () {
          if ($('.myticketpanel').is(':hidden') == false) {
              $("#imgHideShowTicketPanel").hide();
          }
      });

        $('#imgHideShowTicketPanel').hover(
     function () {
         $("#imgHideShowTicketPanel").show();
     }, function () {
         if ($('.myticketpanel').is(':hidden') == false) {
             $("#imgHideShowTicketPanel").hide();
         }
     });
    }

    $(function () { 
        initializeDefault();
        var url = window.location.href;
        var ispostback = "<%= IsPostBack%>";

        if (url.indexOf("TicketId") != -1 && '<%=Request["TicketId"] %>' != '0' && ispostback.toLowerCase() == "false")
        {
            OpenTicketOnLoad();
        }
        else if (url.indexOf("projectID") != -1 && '<%=Request["projectID"] %>' != '0' && ispostback.toLowerCase() == "false")
        {
            OpenSVCOnLoad();
        }
        else if (url.indexOf("serviceID") != -1 && '<%=Request["serviceID"] %>' != '0' && ispostback.toLowerCase() == "false") {
            OpenServiceOnLoad();
        }
    });

    function OpenTicketOnLoad() {
        var url = window.location.href;
       // RemoveNotification();
        if (url.indexOf("#done") == -1 && url.indexOf("removeticketidfromurl") == -1) {
            window.location = window.location + "#done";
            var ticketId = '<%=Request["TicketId"] %>';
            var popupTitle = '<%=Request["pageTitle"] %>';
            var showtab = '<%=Request["showtab"] %>';
            var params = "TicketId=" + ticketId;
            if (showtab != "")
                params += "&showtab=" + showtab;

            //  path, params, titleVal, width, height, stopRefresh, returnUrl
            popupTitle = (popupTitle == null || popupTitle == "") ? ("From Email: " + ticketId) : popupTitle;
          
            window.parent.parent.UgitOpenPopupDialog('<%=ticketURL %>', params, popupTitle, '90', '90', false,  "<%=sourceURL %>");
        }
    }

    function OpenSVCOnLoad() {
        var url = window.location.href;

        if (url.indexOf("#done") == -1 && url.indexOf("removeticketidfromurl") == -1) {
           
            window.location = window.location + "#done";
            var taskType = 'task';
            var viewtype = '1';
            var projectID = '<%=Request["projectID"] %>';
            var taskID = '<%=Request["taskID"] %>';
            var moduleName = '<%=Request["moduleName"] %>';
           
            var popupTitle = '<%=Request["pageTitle"] %>';
            var showtab = '<%=Request["showtab"] %>';
            var params = "taskType=" + taskType + "&" + "viewtype=" + viewtype + "&" + "projectID=" + projectID + "&" + "taskID=" + taskID + "&" + "moduleName=" + moduleName;

            if (showtab != "")
              params += "&showtab=" + showtab;

            //path, params, titleVal, width, height, stopRefresh, returnUrl
            popupTitle = (popupTitle == null || popupTitle == "") ? ("From Email: " + projectID) : popupTitle;
           
            window.parent.parent.UgitOpenPopupDialog('<%=editTaskFormUrl %>', params, popupTitle, '90', '90', false,  "<%=sourceURL %>");
         }
    }

    function OpenServiceOnLoad()
    {
        var url = window.location.href;
        
        if (url.indexOf("#done") != -1)
            return;

        window.location = window.location + "#done";
        var title = "Service";
        var searchParams = new URLSearchParams( window.location.search);
        searchParams.delete("Width");
        searchParams.delete("Height");
        searchParams.delete("isudlg");
        searchParams.delete("IsDlg");
        searchParams.delete("pageTitle");
        var serviceUrl = "<%= newServiceURL%>" + "?" + searchParams.toString();
        window.parent.parent.UgitOpenPopupDialog(serviceUrl, "", title, '90', '90', false,  "<%=sourceURL %>");
    }

    function DisplayTaskCalendarView(obj) {
        var calendarURL = '<%=calendarURL %>';
        var url = calendarURL+ "?control=taskcalender";
        window.parent.UgitOpenPopupDialog(url, '', 'Calendar Task View', '850px', '640px', 0, '');
        return false;
    }


    function saveHomeTabStage(val) {
        var tabID = $("#<%= tabID.ClientID%>");
        if (tabID.length > 0) {
            tabID.val(val);
        }

        if( val == "mytickets")
        {
            var setlink = document.getElementById("<%=selectedMyTicketLink.ClientID %>");
            if(setlink)
            setlink.value = "my";
        }
    }

    function loadTabOnLoad(tabName) {
        
        try
        {
            var tab = filterTabHome.GetTabByName(tabName);
            if(tab == null || tab.visible == false)
            {
                tab = filterTabHome.GetActiveTab();
            }

            if(tab != null)
            {
                if(filterTabHome.activeTabIndex == tab.index)
                {
                    actionOnTabClick('change', tab.index, tab.index);
                }
                else
                {
                    filterTabHome.SetActiveTab(tab);
                }
            }
         
        }
        catch(ex)
        {
        
        }
        var tabCounts="";
       
        // 
    }
    function actionOnTabClick1(s)
    {
        var clickTab = s.GetValue().toString();
        
        var tab = filterTabHome.GetTabByName(clickTab);
        var ddlSelectIndex = ddlFilterHome.GetSelectedIndex();
        if (tab != null) {
            var tabText = tab.GetText(tab.index);
        }
        var tabID = $("#<%= tabID.ClientID%>");
        if (tabID.length > 0) {
            tabID.val(clickTab);
        }
        var activeTab = clickTab;
        //var activeTabContainer = $(".containermain_" + activeTab);
        var activeTabContainer = $(".containermain_" + activeTab);

        var actionType = "change";
        if (actionType == "change") {

            $(".containermain").each(function (i, item) {
                $(item).hide();
            });
            $.cookie("mytab", activeTab, { path: "/" });
            $("#<%=tabID.ClientID%>").val(activeTab);
            activeTabContainer.show();
            if (tabText.indexOf('*') == -1)
                RefreshIframeData(activeTabContainer, activeTab, true);
            else
                RefreshIframeData(activeTabContainer, activeTab, false);
        }
        else if (actionType == "click") {

            if (tabIndex == activeTabIndex)
                RefreshIframeData(activeTabContainer, activeTab, false);
        }
        removeClass1(activeTab);
    }
    function actionOnTabClick(actionType, tabIndex, activeTabIndex) {
        
        var clickTab = filterTabHome.GetTab(tabIndex).name;
        var activeTab = filterTabHome.GetTab(activeTabIndex).name;
        var tab = filterTabHome.GetTabByName(activeTab);
        if(tab!=null)
        {
            var tabText=tab.GetText(tab.index);
        }
        var tabID = $("#<%= tabID.ClientID%>");
        if (tabID.length > 0) {
            tabID.val(clickTab);
        }
      
        var activeTabContainer = $(".containermain_" + activeTab);
       
        if (actionType == "change") {
            
            $(".containermain").each(function (i, item) {
                $(item).hide();
            });
            $.cookie("mytab", activeTab, { path:"/" });
            $("#<%=tabID.ClientID%>").val(activeTab);
            activeTabContainer.show();
            if(tabText.indexOf('*') == -1)
                RefreshIframeData(activeTabContainer, activeTab, true);
            else
                RefreshIframeData(activeTabContainer, activeTab, false);
        }
        else if (actionType == "click") {

            if (tabIndex == activeTabIndex)
                RefreshIframeData(activeTabContainer, activeTab, false);
        }
        removeClass1(activeTab);
    }
    function removeClass1(pActiveTab)
    {
        if(pActiveTab == 'myopentickets' || pActiveTab == 'mytask' )
        {
            $('.tab-contentdiv1').each(function(){
                $(this).removeClass('px-3');
            });
        }
        else
        {
            $('.tab-contentdiv1').each(function(){
                $(this).addClass('px-3');
            });
        }
    }
    function RefreshIframeData(selectedContainer, pActiveTab, ifNotLoaded) {
        var currentDate = new Date();
        if (selectedContainer != null) {
            var frames = $(selectedContainer).find("iframe");
           
            if (frames.length > 0) {
                for (var i = 0; i < frames.length; i++) {
                    var frame = $(frames[i]);
                    frame.attr("frameName",$.cookie("mytab"))
                    if (frame.attr("frame-url") != null) {
                        var url = frame.attr("frame-url");
                        var frameSrcUrl = frame.attr("src");
                        if (frameSrcUrl != undefined && frameSrcUrl != "" && ifNotLoaded) {
                            return;
                        }
                        
                        frame.attr("src", url + "&timespan=" + currentDate.getTime());
                        //alert(2);
                        var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Loading...</div></div></span>";
                        frame.parent().append(loadingElt);
                        frame.parent().css("position", "relative");
                    }
                }
            }
        }
    }

    function PopupCloseBeforeOpenUgit(url, tickedId, tickedIdVal, width, height, refresh, urlVal) {
       
        $("#NewTicketDialog").remove();
        window.parent.UgitOpenPopupDialog(url, tickedId, tickedIdVal, width, height, refresh, urlVal);

    }

    //function open() {

    //}
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var search_Help_Popup;
    $(document).ready(function () {
        //var phrase = "New task";
        /***************JS for autocomplete******************/
        $.ajax({
            url: "<%= ajaxPageURL %>GetPhrases",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (autocompleteData) {
                $("#phrase").dxAutocomplete({
                    dataSource: autocompleteData,
                    placeholder: 'How can I help you?',
                    valueExpr: 'Phrase',
                    showClearButton: true,
                    showDropDownButton: false,
                    searchExpr: 'Phrase',
                    onValueChanged: function (e) {                       
                        phrase = e.value;                        
                    },
                    onItemClick: function (e) {                        
                        $.ajax({
                            url: "<%= ajaxPageURL %>GetPhraseWikiAndHelpForAgentBar?WikiLookUp=" + e.itemData.WikiLookUp + "&HelpCardLookUp=" + e.itemData.HelpCardLookUp,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (PhraseData) {
                                var listWikiArticle = PhraseData.dynwikiArticle;
                                var listHelpCard = PhraseData.dynhelpCard;
                                var html = new Array();

                                //html.push("<div class='homeSearch-header'><div class='search-header-Left'>Does this help?</div><div class='search-header-right' id='btncreate' onclick=createAgentTicket()>No, Open ticket</div></div>");                                                                
                                if ((listWikiArticle == '' || listWikiArticle == undefined) && (listHelpCard == '' || listHelpCard == undefined)) {
                                  html.push("<div class='homeSearch-header'><div class='search-header-Left'>No related help found.</div><div class='search-header-right' id='btncreate' onclick=createAgentTicket()>Open a ticket</div></div>");                                                                
                                  //html.push("<div class='noSearch-message'>No related help found.</div>")
                                }
                                else
                                {
                                  html.push("<div class='homeSearch-header'><div class='search-header-Left'>Does this help?</div><div class='search-header-right' id='btncreate' onclick=createAgentTicket()>No, Open a ticket</div></div>");                                                                
                                    html.push("<div class='search-wikilist-wrap'>Wikis");
                                    if (listWikiArticle != '' && listWikiArticle != undefined) {
                                        html.push("<ul>");
                                        listWikiArticle.map(x => {
                                            html.push(`<li><a href=${x.Link}> ${x.Title} </a></li>`);
                                        });
                                        html.push("</ul>");
                                    }
                                    else {
                                        html.push("<div>No related wiki found.</div>");
                                    }
                                    html.push("</div>");
                                    html.push("<div class='search-helpCardlist-wrap'> Help Cards");
                                    if (listHelpCard != '' && listHelpCard != undefined) {
                                        html.push("<ul>");
                                        listHelpCard.map(x => {
                                            html.push(`<li><a href=${x.Link}> ${x.Title} </a></li>`);
                                        });
                                        html.push("</ul>");
                                    }
                                    else {
                                        html.push("<div class='noHelpCard-msg'>No related help found.</div>");

                                    }
                                    
                                    html.push("</div>")
                                
                                }
                                
                                $('.search_Help_Popup').html(html.join(""));                                
                                $(".search_Help_Popup").dialog('open');                                
                            }
                         })                                                                       
                    }

                });
            }
        });

        $("#phrase").unbind().keypress(function (event) {
            if (event && event.keyCode == '13') {
                phraseCreate();
            }
        });
        
        //$("#btncreateTicket").unbind().click(function () {
        //    debugger;
        //    phraseCreate();
        //});
        
        $(".search_Help_Popup").dialog({
            
                 position: {
                    my: "top",
                    at: "bottom",
                    of: "#phrase"
                },
                modal: true,
                autoOpen: false,
                dialogClass:'homeSearchDialog',
                width: $('#phrase').outerWidth(),
                //height: 240,
                 minHeight: 0,
                 maxHeight:240,
                resizable: false,
                visible: false,
                showTitle: true,
                draggable: false,
                show: { effect: 'fade', duration: 500 },
                open: function (event, ui) {
                        $('.ui-widget-overlay').bind('click', function () {
                            $(".search_Help_Popup").dialog('close');
                        });
                }
                
         });
    });

     function phraseCreate() {

            var input = "{ 'tittitlele' : " + phrase + " }";

         JSON.stringify({ "title": phrase })        
            var phraseInput = {
                'title': phrase
            };

            var title = phrase;
         $.ajax({
             type: "POST",
             url: "<%= ajaxPageURL %>CreateTicket",
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             async: false,
             data: JSON.stringify(phraseInput),
             success: function (TicketTypeDetail) {
                 debugger;
                 if (!TicketTypeDetail.IsTicketCreated) {
                     if (TicketTypeDetail.ModuleName == "SVC") {
                         var param = "&serviceID=" + TicketTypeDetail.ServiceId;
                         window.parent.UgitOpenPopupDialog("/Layouts/uGovernIT/uGovernITConfiguration?control=ServicesWizard", param, TicketTypeDetail.Title, 90, 90, false, "/Pages/UserHomePage");
                     }
                     else {
                         var param = "Title=" + TicketTypeDetail.Title + "&Description=" + TicketTypeDetail.Description + "&Agenttype=" + TicketTypeDetail.AgentType + "&PhraseSearch=1";
                         window.parent.UgitOpenPopupDialog(TicketTypeDetail.StaticModulePagePath, param, TicketTypeDetail.ModuleName, '90', '90', 0, '%2fLayouts%2fuGovernIT%2fDelegateControl.aspx');
                     }
                 }
                 else {
                     var html = new Array();
                     html.push("<div class='phraseTickt-create'>")
                     html.push("<div class='createdTickt-title'>New </div>");
                     html.push("<div class='bulkuser-existUser'> " + TicketTypeDetail.Link + " </div>");
                     html.push("<div class='createdTickt-title'> created Succesfully</div>");
                     html.push("</div>");
                     $('#dialog').append(html.join(""));
                     $("#dialog").dialog('option', 'title', 'New ' + TicketTypeDetail.ModuleName + ' Created');
                     $("#dialog").dialog('open');
                 }
             }
         });
     }

    function createAgentTicket() {
        phraseCreate();
        $(".search_Help_Popup").dialog('close');
    }
        
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        var width = $(window).width();
        if (width <= 720) {
            $('.homecontent-fromServices').find('table').addClass('mobile-homeContent');
                <%--$('#<%=itemsTable.clientid%>').addClass('mobile-homeContent');--%>
        }
    });
</script>
	<div id="dialog"></div>
<%--<asp:Button ID="btGetMail" runat="server" OnClick="btGetMail_Click" Text="Mail to ticket" />--%>
<dx:ASPxCallbackPanel ID="upHomePanel" runat="server">
    <PanelCollection>
        <dx:PanelContent>
        <asp:HiddenField ID="myTicketPanel" runat="server" />
        <asp:HiddenField ID="myServiceCatalog" runat="server" />
        <asp:HiddenField ID="myModulePanel" runat="server" />
        <asp:HiddenField ID="hTabNumber" runat="server" />
        <asp:HiddenField ID="tabID" runat="server" />
        <div class="homeContent_container">
           <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;" width="100%">
        <tr id="helpTextRow" runat="server">
            <td class="paddingleft8" align="right">
                <asp:Panel ID="helpTextContainer" runat="server" CssClass="help-container">
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>
                <div class="homecontent" runat="server" id="homecontent" style="width: 100%">
                    <table id="itemsTable" runat="server" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                        width="100%">

                        <tr id="cWelcomeHeadingPanel" runat="server">
                            <td class="paddingleft8 welcomeTitle">
                                <div class="ms-rteElement-H1B home_welcomeTitle">
                                    <asp:Literal ID="welcomeHeading" runat="server"></asp:Literal>
                                </div>
                            </td>
                        </tr>

                        <tr id="cWelcomeDescPanel" runat="server">
                            <td class="paddingleft8">
                                <p>
                                    <asp:Literal ID="welcomeDesciption" runat="server"></asp:Literal>
                                </p>
                            </td>
                        </tr>

                        <tr id="cServiceCatalog" runat="server" visible="true">
                            <td class="homeService-panelContainer">
                                    <div class="col-md-8 col-sm-12 col-xs-12 pb-2">
                                        <div id="searchPanel"  runat="server">
                                            <div class="searchPannelWrap-container">
                                                <div class="prblemStatement-inputWrap">
                                                    <div id="phrase" class="searchContent"></div>
                                                    <%--<input name="phrase" class="phrase" type="text" placeholder="Problem Statment" />--%>
                                                </div>
                                                <div class="prblemStatment-submitBtn">
                                                    <button id="btncreateTicket" class="btn btn-default prblemStatment-addTicketBtn" type="button" style="display:none">
                                                        <img src="../../Content/Images/sendBlue.png" width="16"/>
                                                    </button>
                                                </div>
                                                <div class="search_Help_Popup"></div>
                                            </div>

                                            <div style="position: relative;">
                                                <div style="display:none">
                                                    <img id="imgHideShowServiceCatalog" style="float: left; display: none; z-index: 1; padding-top: 6px; padding-bottom: 9px;" onclick="HideShowServiceCatalog(this)" src="/Content/Images/Minus_16x16.png" />
                                                </div>
                                                <span id="spServiceCatalog" style="float: left; display: none; padding-top: 8px; padding-left: 2px;">Service Catalog</span>
                                            </div>
                                            <asp:Panel ID="panelServiceCatalog" CssClass="ServiceCatalog" runat="server" visible="true">
                                            </asp:Panel>
                                        </div>            
                                    </div>
                                    <div class="col-md-4 col-sm-12 col-xs-12">
                                        <div id="dashobaord" runat="server" class="fromServiceDoc-chartWrap">
                                                <asp:Panel ID="dashboardPreview" runat="server" CssClass="fromServiceDoc" OnPreRender="DashboardPreview_PreRender">
                                            </asp:Panel>
                                        </div>
                                    </div>
                                </td>                                   
                        </tr>

                        <tr id="cMyTicketPanel" runat="server">
                                    <td class="modulemainpanel">
                                <div style="position: relative;">
                                    <div style="display:none">
                                        <img id="imgHideShowTicketPanel" style="float: left; display: block; z-index: 1; padding-top: 4px; padding-bottom: 9px;" onclick="HideShowTicketPanel(this)" src="/Content/Images/Minus_16x16.png" />
                                    </div>
                                            <%--<span id="spTicketPanelHeader" style="float: left; display: block; padding-top: 6px; padding-left: 2px;">My Requests & Tasks</span>--%>
                                </div>

                                <div class="myticketpanel">
                                    <div style="float: left; width:100%;">
                                        <div class="myticketinner1">
                                            <div class="myticketinner2">
                                                <dx:ASPxLoadingPanel ID="LoadingPanel" ContainerElementID="myhometabs" runat="server" Text="Please Wait..." ClientInstanceName="LoadingPanel"
                                                    Modal="True">
                                                </dx:ASPxLoadingPanel>

                                                <table id="myhometabs" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse; table-layout:fixed;background:#fff" width="100%" >
                                                    <tr>
                                                         <td class="aspxComboBox-dbDropDown">
                                                             <div class="row bottomBorder">
                                                                 <div class="col-md-3 col-sm-3 col-xs-12 noPadding">
                                                                     <dx:ASPxComboBox ClientInstanceName="ddlFilterHome"  EncodeHtml="false" ID="ddlFilterHome" runat="server" Width="100%" 
                                                                         CssClass="btn btn-secondary dropdown-toggle aspxComBox-dropDown homeGrid_dropDown" ItemStyle-CssClass="homeGrid-dropDown-option">
                                                                        <%-- <ClientSideEvents ActiveTabChanged="function(s,e){ actionOnTabClick('change', e.tab.index, s.activeTabIndex);}" TabClick="function(s,e){ actionOnTabClick('click', e.tab.index, s.activeTabIndex);}" />--%>
                                                                         <ClientSideEvents SelectedIndexChanged="function(s, e) {actionOnTabClick1(s); }" />

                                                                        <ItemStyle CssClass="homeGrid-dropDown-option"></ItemStyle>
                                                                     </dx:ASPxComboBox>
                                                                     <dx:ASPxTabControl ID="filterTabHome" ClientInstanceName="filterTabHome" runat="server" ActiveTabIndex="1" Theme="Default" Height="31px">
                                                                         <Tabs>
                                                                         </Tabs>
                                                                         <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px">
                                                                             <Paddings PaddingLeft="13px" PaddingRight="13px"></Paddings>
                                                                         </TabStyle>
                                                                         <ClientSideEvents ActiveTabChanged="function(s,e){ actionOnTabClick('change', e.tab.index, s.activeTabIndex);}" TabClick="function(s,e){ actionOnTabClick('click', e.tab.index, s.activeTabIndex);}" />
                                                                     </dx:ASPxTabControl>
                                                                 </div>
                                                                 <div class="col-md-9 col-sm-9 col-xs-12">
<%--                                                                     <div class="prblemStatement-inputWrap">
                                                                         <div id="phrase" class="searchContent"></div>
                                                                         <%--<input name="phrase" class="phrase" type="text" placeholder="Problem Statment" />--%>
<%--                                                                     </div>--%>
<%--                                                                     <div class="prblemStatment-submitBtn">
                                                                         <button id="btncreateTicket" class="btn btn-default prblemStatment-addTicketBtn" type="button">
                                                                                     <img src="../../Content/Images/sendBlue.png" width="16"/>
                                                                         </button>
                                                                     </div>--%>
                                                                 </div>
                                                             </div>
                                                        </td>
														</tr>
                                                    <tr style="display:none">
                                                        <td>
                                                            
                                                                    <%--function(s, e){saveHomeTabStage(e.tab.name)}--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Repeater runat="server" ID="tabRepeater" OnItemDataBound="tabRepeater_ItemDataBound">
                                                                <ItemTemplate>
                                                                    
                                                                    <div class="containermain containermain_<%# Eval("TabName")%>" style="display:none; width: 100%;">
                                                                        <div class="tab-contentdiv myticketshadowpanel">
                                                                            <div class="tab-contentdiv1 ugit-contentcontainer">
                                                                                <asp:Panel runat="server" ID="tabPanel" Width="100%">
                                                                           
                                                                                </asp:Panel>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </td>
                                                    </tr>
                                                            <asp:HiddenField ID="selectedMyTicketLink" runat="server" />
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>                       
                        
                        <tr id="cModulePanel" runat="server">
                                    <td class="modulemainpanel" style="background:#fff;">
                                <div style="position: relative;">
                                    <img id="imgHideShowModulePanel" style="float: left; display: none; z-index: 1; padding-top: 6px; padding-bottom: 9px" onclick="HideShowModulePanel(this)" src="/Content/Images/Minus_16x16.png" />
                                    <span id="spModulePanel" style="float: left; display: none; padding-top: 6px; padding-left: 2px;">Module Panel </span>
                                </div>
                                <asp:Panel ID="moduleListViewPanel" runat="server" CssClass="modulelist-container hide">
                                    <asp:Repeater ID="moduleListRepeater" runat="server">
                                        <ItemTemplate>
                                            <div class="moduleblockamain">
                                                <div class="mpaneldiv">
                                                    <div class=" mborderdiv" id="m<%# Eval(DatabaseObjects.Columns.Id)%>" onmouseout="HideModuleDesc('<%# Eval(DatabaseObjects.Columns.Id)%>');"
                                                        onmouseover="ShowModuleDesc('<%# Eval(DatabaseObjects.Columns.Id)%>');" onclick="UgitOpenPopupDialog('<%# viewTicketsPath%>','Module=<%# Eval(DatabaseObjects.Columns.ModuleName)%>&UserType=All&Status=Open&showalldetail=true&showFilterTabs=false','Open <%# Eval(DatabaseObjects.Columns.ModuleName)%> Tickets', 90, 90, 0);">
                                                        <div id="contentDetailDiv" class="ugitaccent1homembg mcontentdiv" runat="server">
                                                            <div class="mcontentdetailinner">
                                                                <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;"
                                                                    width="96%" height="97%">
                                                                    <tr>
                                                                        <td style="padding: 5px;" valign="top">
                                                                            <span class="imageblock">
                                                                                <%-- Icon block  --%>
                                                                                <img src="<%# Eval(DatabaseObjects.Columns.Attachments) %>" alt="Module" title="<%# Eval(DatabaseObjects.Columns.Title)%>" /></span>
                                                                        </td>
                                                                        <td style="padding: 5px;" valign="top" width="100%">
                                                                            <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <strong>
                                                                                            <%-- Title block  --%>
                                                                                            <%# Eval(DatabaseObjects.Columns.Title)%>
                                                                                        </strong>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" style="padding-top: 10px;">
                                                                                        <%-- Description block  --%>
                                                                                        <div style="display: none;" id="mDesc<%# Eval(DatabaseObjects.Columns.Id)%>"><%# Eval(DatabaseObjects.Columns.ModuleDescription) != null ? UGITUtility.TruncateWithEllipsis(Eval(DatabaseObjects.Columns.ModuleDescription).ToString(), 50, ".") : string.Empty%></div>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" valign="bottom" class="mbottmlinktd">
                                                                            <table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse; width: 100%;">
                                                                                <tr>
                                                                                    <td style="width: 50%">
                                                                                        <%-- Show my ticket count (Link)  --%>
                                                                                        <asp:LinkButton ID="myTicketLink" runat="server"></asp:LinkButton>
                                                                                    </td>
                                                                                    <td rowspan="2" align="right">
                                                                                        <%-- Open new ticket (button)   --%>
                                                                                        <div style="float: right;">
                                                                                            <asp:Panel runat="server" ID="btnNewPanel">
                                                                                                        <input class="pointer" type="button" value="New" onclick='event.cancelBubble = true;UgitOpenPopupDialog("<%# Eval(DatabaseObjects.Columns.ModuleRelativePagePath) != null ? UGITUtility.GetAbsoluteURL(Eval(DatabaseObjects.Columns.ModuleRelativePagePath).ToString()) : string.Empty%>    ","TicketId=0","<%# UGITUtility.newTicketTitle(Eval(DatabaseObjects.Columns.ModuleName).ToString())%>    ","90","90",0)' />
                                                                                            </asp:Panel>
                                                                                        </div>
                                                                                        <div style="float: right; padding-right: 5px">
                                                                                            <asp:Panel runat="server" ID="btnQuickTicket">
                                                                                                <input class="pointer" type="button" value="Quick" onclick='event.cancelBubble = true;return showTicketTemplate("<%# Eval(DatabaseObjects.Columns.ModuleName) != null ? Eval(DatabaseObjects.Columns.ModuleName).ToString() : string.Empty%>");' />
                                                                                            </asp:Panel>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <%-- Show open ticket count (Link)  --%>
                                                                                        <asp:LinkButton ID="openTicketLink" runat="server"></asp:LinkButton>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>
                                                            <div class="upanelbottomshadow">
                                                                &nbsp;
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </td>
                        </tr>

                    </table>
                    
                </div>
                <div style="padding-bottom:12px; width: 100%">
                  <%--   <ugit:uGovernITMessageBoardUserControl ID="MessageBoard" runat="server" />--%>
                </div> 
            </td>
        </tr>
    </table>
        </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>

<%--<asp:UpdatePanel ID="upHomePanel" runat="server" UpdateMode="Conditional">
    
</asp:UpdatePanel>--%>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var ticketUrl;
    function showTicketTemplate(obj) {
        ClientPopupControl.PerformCallback(obj);
        return false;
    }

    function OnSelectionChanged(s, e) {
        if (e.isSelected) {
            var key = s.GetRowKey(e.visibleIndex);
            var params = "TemplateId=" + key;
            var popupTitle = 'New Ticket';
            openTicketDialog(ticketUrl, params, popupTitle, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }
    }

    function OnEndPopupCallback(s, e) {
        if (typeof (s.cpTicketUrl) != 'undefined') {
            ticketUrl = s.cpTicketUrl;
        }       
        ClientPopupControl.Show();
    }
    
     $(document).ready(function () {
        $('#dialog').dialog({
            autoOpen: false,
            width: 550,
            hieght: 200,
            modal: true,
            //title: "New Ticket Created",
             buttons: [{
                text: "Ok",
                "class": 'phrase-okBtn',
                click: function () {
                    
                    window.parent.CloseWindowCallback(1, window.top.location.href);
                 }

            }],
            close: function (event, ui) { window.location.reload(true); }
        });
});


</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    $(document).ready(function () {
        
        loadTabOnLoad("<%=tabID.Value%>");

        var autoRefreshFrequency=parseInt('<%=autoRefreshFrequency%>');
        if(autoRefreshFrequency>0)
        {
            setInterval(calculateTabCounts,(autoRefreshFrequency*60000));
        }
    });
    var qData =[];
   
    function setQData(tabName)
    {
        if(tabName =="waitingonme")
        {
            qData.push("WaitingOnMe");
        }
        else if(tabName =="mytickets")
        {
            qData.push("MyTickets");
        }
        else if(tabName =="mytask")
        {
            qData.push("MyTasks");
        }
        else if(tabName =="myclosedtickets")
        {
            qData.push("ClosedTickets");
        }
        else if(tabName =="mydepartment")
        {
            qData.push("MyDepartment");
        }
        else if(tabName=="myproject")
        {
            qData.push("MyProject");
        }
        else if(tabName =="documentpendingapprove")
        {
            qData.push("MyPendingApprovals");
        }
    }
    function RefreshTabCount(tabName, count)
    {
        try
        {
           //debugger;
            var tab = filterTabHome.GetTabByName(tabName);
            if(tab!=null)
            {
                var tabText=tab.GetText(tab.index);
                var res = tabText.split("(");
                var finalText=res[0]+" ("+count+")";
                /*var oldCount=parseInt(res[1].replace(')',''));*/
                tab.SetText(finalText);
            }
        }
        catch(ex)
        {

        }
    }
    function checkTabCount(tabName,tabCount,newTabCount)
    {
        //debugger;
        var tab = filterTabHome.GetTabByName(tabName);
        if(tab!=null)
        {
            var tabText=tab.GetText(tab.index);
            
            if(tabCount!=newTabCount && tabText.indexOf('*') == -1)
            {
             
                tabText=tabText+"*";
            }
            else if(tabCount==newTabCount && tabText.indexOf('*') > -1)
            {
                tabText= tabText.replace('*',"");
              
            }
            tab.SetText(tabText);
        }
    }
    function calculateTabCounts()
    {
        try
        {
            var tabData=[];
           
            //debugger;
            for(var i=0;i<filterTabHome.tabs.length;i++)
            {
                if(!filterTabHome.tabs[i].GetVisible())
                    continue;
                else
                {
                    var tab = filterTabHome.GetTabByName(filterTabHome.tabs[i].name);
                    if(tab!=null)
                    {
                        var tabText=tab.GetText(tab.index);
                        var res = tabText.split("(");
                        var count=res[1].replace(')','');
                        if(count.indexOf('*')!=-1)
                            count=count.replace('*','');
                        if(count.indexOf(' ')!=-1)
                            count=count.replace(' ','');
                        tabData.push([filterTabHome.tabs[i].name,parseInt(count)]);
                    }
                    setQData(filterTabHome.tabs[i].name);
                }
            }
            var strData=qData.join(',');
            var jsonData = '{' + '"strTabs":"' + strData + '" }';
            var pageURL = "<%=ajaxHelperPage %>/GetTicketsCounts";
           
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: pageURL,
                data: jsonData,
                dataType: 'json',
                success: function (result) {
                    var str=  JSON.parse(result.d);
               
                    if(str!=null)
                    {
                        for(var x=0;x<tabData.length;x++)
                        {
                            if(tabData[x][0]=="waitingonme")
                                checkTabCount(tabData[x][0],tabData[x][1],str.WaitingOnMe);
                            else if(tabData[x][0]=="mytickets")
                                checkTabCount(tabData[x][0],tabData[x][1],str.OpenTicket);
                            else if(tabData[x][0]=="mytask")
                                checkTabCount(tabData[x][0],tabData[x][1],str.OpenTask);
                            else if(tabData[x][0]=="myclosedtickets")
                                checkTabCount(tabData[x][0],tabData[x][1],str.CloseTicket);
                            else if(tabData[x][0]=="mydepartment")
                                checkTabCount(tabData[x][0],tabData[x][1],str.Department);
                            else if(tabData[x][0]=="myproject")
                                checkTabCount(tabData[x][0],tabData[x][1],str.Projects);
                            else if(tabData[x][0]=="documentpendingapprove")
                                checkTabCount(tabData[x][0],tabData[x][1],str.DocumentPendingApproval);
                       
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) { //Add these parameters to display the required response
                   
                }
            });
        }
        catch(ex)
        {
        }
      
    }
   
</script>

<dx:ASPxPopupControl ID="PopupControl" runat="server" CloseAction="CloseButton"
    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" PopupElementID="gridTemplate" Theme="Material" CssClass="customgridview homeGrid"
    ShowFooter="False" Width="250px" Height="85px" HeaderText="Select template to create new ticket" ClientInstanceName="ClientPopupControl" OnWindowCallback="PopupControl_WindowCallback">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl" runat="server">
            <div style="vertical-align: middle">

                <ugit:ASPxGridView ID="gridTemplate" runat="server" AutoGenerateColumns="False"
                    ClientInstanceName="gridTemplate"  Width="100%" KeyFieldName="ID">
                    <Columns>

                        <dx:GridViewDataTextColumn Caption="Template" CellStyle-HorizontalAlign="Left" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left" FieldName="Title" Settings-AllowSort="False" CellStyle-Cursor="pointer" >
                        </dx:GridViewDataTextColumn>
                    </Columns>

                    <SettingsBehavior AllowSelectByRowClick="true" AutoExpandAllGroups="true" />
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <SettingsPager Position="TopAndBottom">
                        <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                    </SettingsPager>
                    <Settings VerticalScrollableHeight="300" VerticalScrollBarStyle="Standard" VerticalScrollBarMode="Auto" />
                    <ClientSideEvents SelectionChanged="OnSelectionChanged" />
                </ugit:ASPxGridView>
            </div>

        </dx:PopupControlContentControl>
    </ContentCollection>

    <ClientSideEvents EndCallback="OnEndPopupCallback" />
</dx:ASPxPopupControl>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
.dxgvControl_UGITNavyBlueDevEx{
   margin-top: 0px !important;
}
</style>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsMenuBar.ascx.cs" Inherits="uGovernIT.Web.SettingsMenuBar" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function refreshDiv(msgtype) {
        var str = null;
        if (msgtype == "Critical") {
            $('#ulale').empty();
            str = $("#ulale");
        }
        else if (msgtype == "Warning") {
            $('#ulwar').empty();
            str = $("#ulwar");
        }
        else if (msgtype == "Ok") {
            $('#uladv').empty();
            str = $("#uladv");
        }
        else {
            msgtype == "";
        }
        $.ajax({
            type: "POST",
            url: "<%= ajaxAPIURL %>GetDashboardAdvisor",
            data: "{'MessageType':'"+msgtype+"'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                for (var i = 0; i < response.length; i++) {
                    str.append("<li id='liMessage' style='text-align:left'>" + response[i].Body + "</li>");
                }
                if (msgtype == "Ok") { $("#successMsgCount").html(response.length); }
                if (msgtype == "Critical") { $("#alertMsgCount").html(response.length); }
                if (msgtype == "Warning") { $("#warningMsgCount").html(response.length); }
                
            },
            failure: function (response) {
                alert(response);
            },
            error: function (response) {
                alert(response);
            }
        });
    }
<%--    function setPageTitleHeader(title) {
      
        var pageTitle = document.getElementById("<%=pageTitle.ClientID%>");
        pageTitle.innerText = title;
    }--%>
    //  var phrase = string.empty;
    function setLandingPage(s, e) {
        var groupname = "<%=defaultrole %>";
       "<%Session["SelectedGroup"] = defaultrole; %>"
        window.location = "<%=UserLandingPage %>";
    }

    function uGitWatermarkFocus(txtElem, strWatermark) {
        var txtwatermark = document.getElementById("<%=ugitIdSearchBox.ClientID%>");
        if (txtElem.value == strWatermark) {
            txtElem.value = '';
            txtElem.className = "uGitsearchTextBox";
        }
    }

    function uGitWatermarkBlur(txtElem, strWatermark) {
        var txtwatermark = document.getElementById("<%=ugitIdSearchBox.ClientID%>");
        if (txtElem.value == '') {
            txtElem.value = strWatermark;
            txtElem.className = "uGitWaterMarkClass";
        }
    }
    function SubmitSearchRedirect() {
        if ($('#Globalsearch').val().trim() == '')
            return;

         <%--var param = "globalSearchString=" + ugitIdSearchBox.GetText() + "&isGlobalSearch=true&showGlobalFilter=true";
        window.parent.UgitOpenPopupDialog("<%=filterPage %>", param, 'Global Search', '90', '70', 0);--%>
        var param = "globalSearchString=" + $('#Globalsearch').val() + "&isGlobalSearch=true&showGlobalFilter=true";
        window.parent.UgitOpenPopupDialog("<%=filterPageNew %>", param, 'Global Search', '90', '90', 0);

        return false;
    }

    function LogInUser_Click(s, e) {
        
        var menuIndex = e.item.index;
        if (menuIndex == 6) {
            adminMenuLoadingPanel.Show();

        }
        event.stopPropagation();

        //cbSettingMenuBar.PerformCallback();
    }

    $(document).ready(function () {

        /*****Global Search function*******/

        $("#btnGlobalsearch").on('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            SubmitSearchRedirect();
        });

        $("#Globalsearch").keydown(function (event) {
            if (event && event.keyCode == '13') {
                if ($('#Globalsearch').val() == '') {
                    return false;
                }
                SubmitSearchRedirect();
            }
        });

        /******End for gloabal Search*******/


        /********Start function for message board******/
        if ($('#alertMsgCount').text() == 0) {
            $('#alertMsgCount').parent().find('img').css("cursor", "not-allowed");
        }
        if ($('#warningMsgCount').text() == 0) {
            $('#warningMsgCount').parent().find('img').css("cursor", "not-allowed");
        }
        if ($('#successMsgCount').text() == 0) {
            $('#successMsgCount').parent().find('img').css("cursor", "not-allowed");
        }
        $("#<%=ugitIdSearchBox.ClientID%>").keypress(function (event) {
            if (event && event.keyCode == '13') {
                SubmitSearchRedirect();
            }
        });

        // Dropdown toggle
        $('.msgBoard-dropdown-toggle').click(function () {
            if ($('#alertMsgCount').text() == 0) {
                $('#alertMsgCount').parent().find('ul').css("display", "none");
            } else {
                $(this).next('.alertDropDown').toggle();
            }
        });
        $('.warningBoard-dropdown-toggle').click(function () {
            if ($('#warningMsgCount').text() == 0) {
                $('#warningMsgCount').parent().find('ul').css("display", "none");
            } else {
                $(this).next('.warningDropdown').toggle();
            }

        });
        $('.sucessBoard-dropdown-toggle').click(function () {
            if ($('#successMsgCount').text() == 0) {
                $('#successMsgCount').parent().find('ul').css("display", "none");
            } else {
                $(this).next('.sucessDropdown').toggle();
            }
        });

        /****************End function for message board***************/
    });
    $(document).click(function (e) {
        var target = e.target;
        if (!$(target).is('.msgBoard-dropdown-toggle') && !$(target).parents().is('.msgBoard-dropdown-toggle')) {
            $('.alertDropDown').hide();
        }
        if (!$(target).is('.warningBoard-dropdown-toggle') && !$(target).parents().is('.warningBoard-dropdown-toggle')) {
            $('.warningDropdown').hide();
        }
        if (!$(target).is('.sucessBoard-dropdown-toggle') && !$(target).parents().is('.sucessBoard-dropdown-toggle')) {
            $('.sucessDropdown').hide();
        }
    });

    <%--$(document).ready(function () {
        // function to check is mobile device 
         $(window).on("resize", function (e) {
                checkScreenSize();
         });

         checkScreenSize();

         function checkScreenSize(){
            var newWindowWidth = $(window).width();
             if (newWindowWidth < 767) {
                 alert('in mobile');
                 $('#userProfileXs').appendTo('#<%=dvUserProfile.ClientID%>');
            }
            else
             {
                // $('#userProfileDesktop').prependTo('#<%=dvUserProfile.ClientID%>');
               //alert('is not mobile')
            }
         }
    });--%>


    $(document).ready(function () {

        var phrase = "Laptop not booting";
        $('.popup-menuLog-inuser-outer').parent().addClass('menuprofile-popUP')
        var valW = $('.prof-percent').text();
        var finalval = parseInt(valW) * 2;
        $('.prof-bar').width(finalval);


        /***********JS for Search panel*******************/
        $("#btnGlobalsearch1").click(function () {
            $("#SearchPanel").slideToggle("slow");
        });

        /***************JS for autocomplete******************/


        $.ajax({
            url: "<%= ajaxPageURL %>GetPhrases",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (autocompleteData) {
                $("#autocompleteContainer").dxAutocomplete({
                    dataSource: autocompleteData,
                    placeholder: 'Search',
                    valueExpr: 'Phrase',
                    showClearButton: true,
                    showDropDownButton: false,
                    searchExpr: 'Phrase',
                    onValueChanged: function (e) {
                        // var data = e.selectedItem;
                        alert(e.value);
                        phrase = e.value;
                        // OpenNewTicketForTicketType();
                    },

                });
            }
        });






        $("#btncreateTicket1").click(function () {
            //alert('hiee');
            alert(phrase);
            var phraseInput = "{ 'title' : " + JSON.stringify(phrase) + " }";

            $.ajax({
                type: "POST",
                url: "<%= ajaxPageURL %>CreateTicket",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: phraseInput,
                success: function (TicketTypeDetail) {
                    // window.parent.UgitOpenPopupDialog('/Pages/acr', 'homepage=1', 'New ACR Ticket', '90', '90', 0, '%2fLayouts%2fuGovernIT%2fDelegateControl.aspx');
                    window.parent.UgitOpenPopupDialog(TicketTypeDetail.StaticModulePagePath, 'homepage=1', TicketTypeDetail.ModuleName, '90', '90', 0, '%2fLayouts%2fuGovernIT%2fDelegateControl.aspx');

                }

            });

        });

    });

    function GetMenuItem() {
        return ASPxMenu1.GetItem(get_cookie('activemenuitem'));
    }

    function InitMenu() {
        if (get_cookie('activemenuitemurl') == '') {
            set_cookie('activemenuitemurl', window.location.href);
        }
        if (get_cookie('activemenuitemurl') != window.location.href) {
            set_cookie('activemenuitem', '');
        }
        var activeMenuItem = get_cookie('activemenuitem');
        if (activeMenuItem != '') {
            ASPxMenu1.SetSelectedItem(GetMenuItem());
        }
    }

    function MenuItemClick(s, e) {
        //delete cookie.
        delete_cookie('activemenuitemurl');
        delete_cookie('activemenuitem');
        
        // set cookie.
        set_cookie('activemenuitem', e.item.index);
        set_cookie('activemenuitemurl', '');
    }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.dx-overlay-shader {
        background-color: white !important;
        opacity: 0.7;
    }*/
    .popup-menuLog-inuser-outer {
        font-family: Poppins;
        font-size: 12px;
    }

    .dxmLite_UGITNavyBlueDevEx .dxm-popup .dxm-hovered,
    .dxmLite_UGITNavyBlueDevEx .dxm-main .dxm-hovered {
        background: none !important;
        border: none !important;
        color: #4A6EE2;
    }

        .dxmLite_UGITNavyBlueDevEx .dxm-main .dxm-hovered a {
            color: #4A6EE2 !important;
        }

    .navbar-inverse {
        background-color: #253746;
        border-color: #253746;
    }
    .tenant-name {
    font-size: 20px;
    color: white;
    font-weight: 900;
    height: 60px;
    align-content: center;
    display: flex;
    flex-wrap: wrap;
}
    .userGroup-menuWrap ul li.dxm-item {
    margin-left:10px;
    }

    .userGroup-tabContainer .userGroup-headerMenu a {
    height: 60px;
    display: flex;
    align-content: center;
    flex-wrap: wrap;
    font-size: 10pt !important;
    font-weight: 600 !important;
    }

    .userGroup-menuWrap ul li.dxm-item.dxm-selected {
    border: none;
    height: 61px;
    margin-top: -2px;
    background: none;
    background-color: #ffffff;
    border: 1px solid #ffffff;
    color: black !important;
    border-top-left-radius: 20px;
    border-top-right-radius: 20px;
}
     .userGroup-menuWrap ul li.dxm-item.dxm-hovered {
    border: none;
    height: 61px;
    margin-top: -2px;
    background: none;
    background-color: #ffffff;
    border: 0px;
    color: black !important;
    border-top-left-radius: 20px;
    border-top-right-radius: 20px;
}
        .userGroup-menuWrap ul li.dxm-item.dxm-hovered .dxm-contentText {
            
        }
        .userGroup-menuWrap ul li.dxm-hovered a {
    color: white !important;
    border: 0px;
}
   .userGroup-menuWrap ul li.dxm-selected {
       background: white !important;
    /*border: none;
    height: 55px;
    margin-top: -2px;
    background: none !important;
    border: 1px solid #ffffff;
    border-top-left-radius: 20px;
    color: #a0db49 !important;
    border-top-right-radius: 20px;*/
}

  .userGroup-menuWrap ul li.dxm-item.dxm-selected a {
    color: black !important;
}
    .userGroup-menuWrap ul.dxm-noImages {
    height:60px !important;
    }

    .popup-menuLog-inuser-outer ul li.dxm-item.dxm-selected {
        background: #d8dde6 !important;
        border: none;
    }

    .popupMenuUI {
        /*font-family: 'Roboto', sans-serif !important;*/
        font-size: 13px;
    }

    .dxmLite_UGITNavyBlueDevEx .dxm-popup {
        background-color: #f1f2f6;
        border: 1px solid #9da0aa;
        padding: 1px;
    }

    #<%=popupLandingPageItems.ClientID%> ul {
	        background: #70747b;
			        border: 1px solid black;
					        
							    }
								
	    #<%=popupLandingPageItems.ClientID%> .dxm-item {
        cursor: pointer;
        border-bottom: 1px solid #70747b;
        padding: 5px;
        font-size: 13px;
    }
	
	    #<%=popupLandingPageItems.ClientID%> .dxm-contentText{
        color: white !important;

    }

    #<%=popupLandingPageItems.ClientID%> .dxmLite_UGITNavyBlueDevEx .dxm-popup {
        background-color: #70747b !important;
        border: 1px solid #9da0aa;
        padding: 1px;
    }
	 

    #ctl00_ctl00_MainContent_HeaderPane_SettingsMenuBar_popupLandingPageItems ul {
        background: #70747b;
        border: none;
    }   

    #imageugovernIilogo {
        /*width:100% !important;*/
    max-width: 80px !important;
    height: 20px;
    margin-top: 18px !important;
    }
</style>
<div>
    <div class="header">

        <nav class="navbar navbar-inverse navbar-default customNavbar" role="navigation">
            <div class="jumbotron homeDashboard_headerMenu_container">
                <div class="row homeDashboard_headerMenu_row">
                    <!-- Brand and toggle get grouped for better mobile display -->
                    <div class="navbar-header">
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                            <span class="sr-only">Toggle navigation</span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                        <div class="">
                            <div class="headerLogo-imgWrap">
                                <dx:ASPxImage ID="imageugovernIilogo" runat="server" ClientIDMode="Static" >
                                    <%--<ClientSideEvents Click="function(s, e){ window.location = '/'; }" />--%>
                                    <ClientSideEvents Click="setLandingPage" />
                                </dx:ASPxImage>
                                
                            </div>
                            
                        </div>
                        <div id="userProfileXs">
                        </div>
                    </div>

                    <!-- Collect the nav links, forms, and other content for toggling -->
                    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                        <div class="col-md-1 col-sm-1 col-xs-12 landing-pageTitle">
                            <%--<asp:Label ID="pageTitle" runat="server"></asp:Label>--%>
                            <asp:Literal ID="pageTitle" runat="server" Visible="false"></asp:Literal>
                            <asp:Label ID="accountName" runat="server" Visible="true" style="color: #c7cfda;font-family: Roboto; font-size: 11pt;font-weight:600;"/>
                        </div>
                        <div class="col-md-10 col-sm-10 col-xs-12 headerContent_profile">
                            <div class="userProfile-headerContainer" id="userProfileDesktop">
                                <div class="post-container col-sm-1 homeDashboard_headerMenu_wrap" runat="server" id="dvUserProfile">
                                    <div class="post-thumb pull-left" style="margin-right: 10px;">
                                        <asp:Image ID="imgUserProfile" runat="server" CssClass="header-profileImg" />
                                    </div>
                                    <div class="profile-right-info pull-left">
                                        <div class="d-block">
                                            <asp:Label ID="lblLogInUser" runat="server" CssClass="hidden-xs" Style="color: #c7cfda; font-size: 11pt;font-weight:600;" />
                                            <span class="glyphicon" style="top: 2px; color: #c7cfda; padding-right: 5px;">&nbsp;&#xe252;</span>
                                        </div>
                                        <%--<div class="post-content d-block" style="font-size:10px !important;">&nbsp;</div>--%>
                                    </div>
                                    <%--<div class="post-title"><asp:Label ID="lblLogInUser" runat="server" /><span class="glyphicon" style="color:#4A6EE2;top:2px;">&nbsp;&#xe252;</span></div>--%>
                                </div>
                                <div class="headerPage-mesage ">
                                    <div class="meassageBord">
                                        <ul class="messageListWrap">
                                            <li class="messageAlert" title="Alert">
                                                <nav class="alertMsgContainer" onclick="refreshDiv('Critical');">
                                                    <div id="alertMsgCount" class="msg-count"><%=Criticalcount%></div>
                                                    <img src="../Content/Images/NewAdmin/alert-gray.png" class="msgBoard-dropdown-toggle" />
                                                    <ul id="ulale" class="msgDropdown alertDropDown">
                                                        <asp:Repeater ID="MyMessageAlert" runat="server">
                                                            <ItemTemplate>
                                                                <li id="liMessage" style="text-align: left" runat="server">
                                                                    <%# Eval("Body") %>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </nav>
                                            </li>
                                            <li class="messageWarning" title="Warnings">
                                                <nav class="warningMsgContainer" onclick="refreshDiv('Warning');">
                                                    <div id="warningMsgCount" class="msg-count"><%= WarningCount %></div>
                                                    <img src="../Content/Images/NewAdmin/warning-gray.png" class="warningBoard-dropdown-toggle" />
                                                    <ul id="ulwar" class="msgDropdown warningDropdown">
                                                        <asp:Repeater ID="MyMessageWarning" runat="server">
                                                            <ItemTemplate>
                                                                <li id="liMessage" style="text-align: left" runat="server">
                                                                    <%# Eval("Body") %>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </ul>
                                                </nav>
                                            </li>
                                            <li class="messageSuccess" title="Advisories">
                                                <nav class="sucessMsgContainer" onclick="refreshDiv('Ok');">
                                                    <div id="successMsgCount" class="msg-count"><%= Sucesscount %></div>
                                                    <img src="../Content/Images/NewAdmin/advisories-gray.png" class="sucessBoard-dropdown-toggle" />
                                                    <ul id="uladv" class="msgDropdown sucessDropdown">
                                                        <asp:Repeater ID="MyMessageSuccess" runat="server">
                                                            <ItemTemplate>
                                                                <li id="liMessage" style="text-align: left" runat="server">
                                                                    <%# Eval("Body") %>
                                                                </li>
                                                            </ItemTemplate>
                                                        </asp:Repeater>

                                                    </ul>
                                                </nav>
                                            </li>
                                            <%if (ShowHelpButton) {  %>
                                            <li class="messageSuccess" title="Advisories">
                                                <a onclick="gotoDefaultAdminPage()" title="Help">
                                                    <img src="../Content/Images/i-icon.png" class="sucessBoard-dropdown-toggle" />
                                                </a>
                                            </li>
                                            <%} %>
                                        </ul>
                                    </div>
                                </div>
                                <div id="globalSearch-container" class="globalSearch-container col-md-2 col-sm-3 col-xs-12 noPadding">
                                    <%--<div class="navbar-form" role="search" id="globalSearchform">--%>
                                        <div class="input-group globalSearch-wrap add-on">
                                            <input class="form-control SearchInput" placeholder="Search" name="srch-term" id="Globalsearch" type="text">
                                            <div class="input-group-btn">
                                                <button id="btnGlobalsearch" class="btn btn-default globalSearch-icon" type="button"><i class="glyphicon glyphicon-search"></i></button>
                                            </div>
                                        </div>
                                    <%--</div>--%>
                                </div>
                                <div>
                                    <div class="userGroup-tabContainer">
                                        <div class="userGroup-headerMenu" style="float: inline-start;">
                                            <dx:ASPxCallback ID="GroupCallback" runat="server" ClientInstanceName="GroupCallback" OnCallback="GroupCallback_Callback"></dx:ASPxCallback>
                                            <dx:ASPxMenu ID="ASPxMenu1" ClientInstanceName="ASPxMenu1" runat="server" SelectParentItem="true" AllowSelectItem="True" ShowPopOutImages="True" Theme="Moderno" Border-BorderStyle="None" CssClass="userGroup-menuWrap">
                                                <ClientSideEvents ItemClick="MenuItemClick" Init="InitMenu" />
                                            </dx:ASPxMenu>
                                        </div>   
<%--                                        <div class="" runat="server" id="divLandingPagesPopup" style="height:1px">--%>
                                            <img id="divLandingPagesPopupimage" runat="server" src="../../Content/Images/more_white.png" style="width: 20px; margin-top: 23px;" />
                                        <%--</div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="SearchPanel">
                            <div id="autocompleteContainer" class="searchContent"></div>
                            <div id="globalCreateSearch-container" class="globalSearch-container">
                                <div class="input-group-btn">
                                    <button id="btncreateTicket1" class="btn btn-default globalSearch-icon" type="button"><i class="glyphicon glyphicon-search"></i></button>
                                </div>
                                <a>
                            </div>
                        </div>
                        <!-- /.navbar-collapse -->
                    </div>
                </div>
                </div>
        </nav>


        <div class="col-md-2 col-sm-3 col-xs-12 logoWrap no-padding-xs xs-flex-menu">
            <div class="col-md-6 col-sm-6 col-xs-6 search" style="display: none;">
                <%--<input type="submit" value="">
                        <input type="text" value="" placeholder="search...">--%>
                <dx:ASPxPanel ID="GlobalSearch" runat="server" EnableTheming="true" CssClass="homeDashboard_search_wrap">
                    <PanelCollection>
                        <dx:PanelContent>
                            <div>
                                <dx:ASPxButtonEdit ID="ugitIdSearchBox" runat="server" AutoPostBack="false" Width="140" CssClass="homeDashboard_search_tableWrap"
                                    NullText="Search..." ClientInstanceName="ugitIdSearchBox" BackColor="Transparent" Border-BorderStyle="None">
                                    <ClientSideEvents ButtonClick="function(s,e){SubmitSearchRedirect();}" />
                                    <ButtonStyle BackColor="Transparent"></ButtonStyle>
                                    <Buttons>
                                        <dx:EditButton Position="Left">
                                            <Image Url="/Content/images/AppIcon/search_icon.png" ToolTip="Search" Width="17px">
                                            </Image>
                                        </dx:EditButton>
                                    </Buttons>
                                </dx:ASPxButtonEdit>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </div>
        </div>

    </div>

    <div class="header-messageBordWrap">
        <%--   <div class="col-md-6 col-sm-6 col-xs-12 headerPage-title">  
            <div class="pageTitle">
                <p>Page Title</p>
            </div>
        </div>--%>
        <%-- <div class="col-md-6 col-sm-6 col-xs-12 headerPage-mesage">
            <div class="meassageBord">
                <ul class="messageListWrap">
                    <li class="messageAlert" title="Alert">
                        <nav class="alertMsgContainer">
                            <div class="msg-count">2</div>
                            <img src="../Content/Images/NewAdmin/alert-error.png"class="msgBoard-dropdown-toggle"/>
                            <ul class="msgDropdown alertDropDown">
                                <li>Message 1</li>
                                <li>Message 2</li>
                                <li>Message 3</li>
                                <li>Message 4</li>
                                <li>Message 5</li>
                            </ul>
                        </nav>
                    </li>
                    <li class="messageWarning" title="Warnings">
                        <nav class="warningMsgContainer">
                            <div class="msg-count">2</div>
                            <img src="../Content/Images/NewAdmin/warning.png"class="warningBoard-dropdown-toggle"/>
                            <ul class="msgDropdown warningDropdown">
                                <li>Message Warning</li>
                                <li>Message Warning2</li>
                                <li>Message Warning3</li>
                                <li>Message Warning4</li>
                                <li>Message Warning5</li>
                            </ul>
                        </nav>
                    </li>
                    <li class="messageSuccess" title="Advisories">
                        <nav class="sucessMsgContainer">
                            <div class="msg-count">2</div>
                            <img src="../Content/Images/NewAdmin/ddvisories.png"class="sucessBoard-dropdown-toggle"/>
                            <ul class="msgDropdown sucessDropdown">
                                <li>Message Sucess1</li>
                                <li>Message Sucess2</li>
                                <li>Message Sucess3</li>
                                <li>Message Sucess4</li>
                                <li>Message Sucess5</li>
                            </ul>
                        </nav>
                    </li>
                </ul>
            </div>
        </div>--%>
    </div>

    <%--<div style="float: left;position:relative;">
        <dx:ASPxPanel ID="GlobalSearch" runat="server" EnableTheming="true">
            <PanelCollection>
                <dx:PanelContent>
                    <div>
                        <dx:ASPxButtonEdit ID="ugitIdSearchBox" runat="server"  Width="140" NullText="Search String" ClientInstanceName="ugitIdSearchBox" Visible="false" >
                            <ClientSideEvents ButtonClick="function(s,e){SubmitSearchRedirect();}" />
                            <Buttons>
                                <dx:EditButton>
                                    <Image Url="/Content/images/newsearch.png" ToolTip="Search">
                                    </Image>
                                                                                           
                                </dx:EditButton>
                            </Buttons>
                        </dx:ASPxButtonEdit>
                    </div>
                </dx:PanelContent>
            </PanelCollection>
        </dx:ASPxPanel>
    </div>
    <div style="float: left; font-weight:bold; height:28px; padding-left:15px;">
        <dx:ASPxLabel ID="lblLogInUser" runat="server" Text="User Name" Height="28px" ForeColor="White" Visible="false">
            <BackgroundImage ImageUrl="Content/Images/firstnavbg1X28.gif" Repeat="RepeatX"></BackgroundImage>
            
        </dx:ASPxLabel>    
    </div>--%>

    <dx:ASPxCallback ID="cbSettingMenuBar" runat="server" ClientInstanceName="cbSettingMenuBar" OnCallback="cbSettingMenuBar_Callback"></dx:ASPxCallback>
    <dx:ASPxLoadingPanel ID="adminMenuLoadingPanel" ClientInstanceName="adminMenuLoadingPanel" runat="server" Modal="true" Text="Please wait..."></dx:ASPxLoadingPanel>
    <dx:ASPxPopupMenu ID="popupMenuLogInUser" CssClass="popup-menuLog-inuser-outer dxmLite_UGITNavyBlueDevEx 
        dxm-main dxm-hovered dxmLite_UGITNavyBlueDevEx dxm-popup "
        runat="server" ClientInstanceName="popupMenuLogInUser"
        PopupElementID="dvUserProfile" ShowPopOutImages="True" AutoPostBack="false" CloseAction="MouseOut" ItemSpacing="0"
        PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Below" PopupAction="LeftMouseClick" OnItemClick="popupMenuLogInUser_ItemClick">
        <ClientSideEvents ItemClick="LogInUser_Click" />
        <Items>
            <dx:MenuItem Name="itemClientProfile" NavigateUrl='<%= redirectToclientProfile %>'>
                <Template>
                    <a href="<%= redirectToclientProfile %>">
                        <div class="prof-progress" runat="server" id="dvclientprofile" visible='<%# isProgressBarActivated %>'>
                            <div class="prof-bar">
                                <p runat="server" id="clientProfile" class="prof-percent"><%= clientProfilePer%><span>%</span></p>
                            </div>
                        </div>
                    </a>
                </Template>
            </dx:MenuItem>
            <dx:MenuItem Text="" Name="LoginUser" ItemStyle-CssClass="userProfile_name"></dx:MenuItem>
            <dx:MenuItem Text="My Profile" Name="MyProfile"></dx:MenuItem>
            <dx:MenuItem Text="Sign Out" Name="SignOut"></dx:MenuItem>
            <dx:MenuItem Text="Change Password" Name="Change Password"></dx:MenuItem>
            <%--<dx:MenuItem Text="Change The Look" Visible="false" Name="ChangeLook"></dx:MenuItem>--%>
            <%--<dx:MenuItem Text="Admin" Name="Admin" Visible="false"></dx:MenuItem>--%>
            <dx:MenuItem Text="Admin" Name="Admin" Visible="false"></dx:MenuItem>
            <dx:MenuItem Text="Back to Super admin Login" Name="SuperAdminLogin">
                <Template>
                    <asp:Button runat="server" ID="LoginToSuperAdmin" OnClick="LoginToSuperAdmin_Click" Text="Back to Super Admin" CssClass="redirect-superAdmin" />
                </Template>
            </dx:MenuItem>
        </Items>

    </dx:ASPxPopupMenu>

    <dx:ASPxPopupMenu ID="popupLandingPageItems" CssClass="popup-menuLog-inuser-outer dxmLite_UGITNavyBlueDevEx 
    dxm-main dxm-hovered dxmLite_UGITNavyBlueDevEx dxm-popup popupMenuUI"
        runat="server"
        PopupElementID="divLandingPagesPopupimage" ShowPopOutImages="True" AutoPostBack="false" CloseAction="MouseOut" ItemSpacing="0"
        PopupHorizontalAlign="NotSet" PopupVerticalAlign="Below" PopupAction="LeftMouseClick">
        <Items></Items>
    </dx:ASPxPopupMenu>
</div>

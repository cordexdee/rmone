<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalSearchPage.ascx.cs" Inherits="uGovernIT.Web.GlobalSearchPage" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" src="https://vadikom.com/demos/poshytip/src/jquery.poshytip.js"></script>
<link rel="stylesheet" href="https://vadikom.com/demos/poshytip/src/tip-skyblue/tip-skyblue.css" type="text/css" />


<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formlabel {
        width: 160px;
        text-align: right;
    }

    .full-width {
        width: 90%;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .text-error {
        color: #a94442;
        font-weight: 500;
        margin-top: 5px;
    }

    div.ms-inputuserfield {
        height: 17px;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/Content/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .required-item:after {
        content: '* ';
        color: red;
        font-weight: bold;
    }

    .hide {
        display: none;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        if (typeof gridClientInstance !== "undefined") {
            gridClientInstance.SetHeight(0);

            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            gridClientInstance.SetHeight(containerHeight);
        }
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>


<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxgvTable_UGITNavyBlueDevEx {
        background-color: #f6f7fb !important;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {


        var ticketid = params.split('=')[1];


       <%--//set_cookie('UseManageStateCookies', 'true', null, "<%= SPContext.Current.Web.ServerRelativeUrl %>");--%>
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function servicecatalog(path, params, titleVal, width, height, stopRefresh, returnUrl) {


        //var Servicesid =  e.item.name;
        var absPath = path + params;

        window.parent.UgitOpenPopupDialog(absPath, '', titleVal, '95', '95', 0, "/default.aspx");
        return false;
        //UgitOpenPopupDialog(absPath, '', title, '90', '90', '', '');
    }

    function checkEmptyString() {
        if ($('#<%=txtWildCard.ClientID%>').val().trim() == '')
            return false;

        LoadingPanel.Show();
        return true;
    }

</script>



<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    // <![CDATA[
    var curTailElement = null;
    var loadingDivText = '<div style="vertical-align: middle; text-align: center;">Loading&hellip;</div>';
    function OnTailClick(newsID, htmlElement) {
        if (!NewsCallback.InCallback() && !IsCurrentNews(htmlElement)) {
            curTailElement = htmlElement;
            ShowPopup(htmlElement, loadingDivText);
            NewsCallback.PerformCallback(newsID);
        }
    }
    function OnCallbackComplete(result) {
        if (GetPopupControl().IsVisible())
            ShowPopup(curTailElement, result);
    }
    function OnNewsControlBeginCallback() {
        GetPopupControl().Hide();
    }
    function IsCurrentNews(htmlElement) {
        return (curTailElement == htmlElement) && GetPopupControl().IsVisible();
    }
    function GetPopupControl() {
        return ASPxPopupClientControl;
    }
    function ShowPopup(element, contentText) {
        GetPopupControl().Hide();
        GetPopupControl().SetContentHTML(contentText);
        GetPopupControl().ShowAtElement(element);
    }
    // ]]>
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $(".nav-tabs li").click(function () {
            $('.nav-tabs li').not(this).removeClass('active');
            $(this).addClass('active');
            var activeTab = $(this).find('a').attr('href').replace("#", "");
            $(".tab-content .tab-pane").each(function () {
                if ($(this).attr('id') == activeTab) {
                    if ($(this).attr('id') == "b") {
                        $("#dvServiceUserRequest").show();
                    }
                    else if ($(this).attr('id') == "a" || $(this).attr('id') == "c") {
                        $("#dvServiceUserRequest").hide();
                    }

                    $('.tab-content .tab-pane').not(this).removeClass('active');
                    $(this).addClass('active');
                }
            });
        });
        $(".nav-tabs li.active").trigger('click');
        $('.global-searchWrap').parent().addClass('global-searchContainer');
    });
</script>

<%--<div class="searchFilter" style="display:none">
    <div class="searchFilter_searchWrap">
        <dx:ASPxButtonEdit ID="txtWildCard" runat="server" CssClass="txtWildCard selectFromTo_field" NullText="Search String" Width="140">
        <ClientSideEvents ButtonClick="function(s,e){startLocalSearch(s,e);}" KeyPress="function(s,e){onSearchKeyPress(s,e)}" />
            <Buttons>
                <dx:EditButton>
                    <Image Url="/Content/images/searchNew.png" ToolTip="Search" Width="15px">
                    </Image>

                </dx:EditButton>
            </Buttons>
        </dx:ASPxButtonEdit>
    </div>
</div> 
<div class="searchFilter" style="display:none">
    <div class="searchFilter_searchWrap" style="display:none">
        <dx:ASPxButtonEdit ID="ASPxButtonEdit1" runat="server" CssClass="txtWildCard selectFromTo_field" NullText="Search String" Width="140">
        <ClientSideEvents ButtonClick="function(s,e){startLocalSearch(s,e);}" KeyPress="function(s,e){onSearchKeyPress(s,e)}" />
            <Buttons>
                <dx:EditButton>
                    <Image Url="/Content/images/searchNew.png" ToolTip="Search" Width="15px"></Image>
                </dx:EditButton>
            </Buttons>
        </dx:ASPxButtonEdit>
    </div>
</div>--%>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." CssClass="customeLoader" ClientInstanceName="LoadingPanel" Image-Url="~/Content/IMAGES/ajaxloader.gif"
    Modal="True" ImagePosition="Top">
</dx:ASPxLoadingPanel>

<div class="col-md-12 col-sm-12 col-xs-12 global-searchWrap">
    <div class="row">
        <h4 id="NodataFound" style="text-align: center;" runat="server" visible="false">Sorry, we couldn't find any matching result for  "<%=searchText%>". Please try another search.</h4>
    </div>
    <div class="row" style="padding-top: 10px;">
        <div class="col-md-4 col-sm-4 col-xs-12"></div>
        <div class="col-md-5 col-sm-4 col-xs-12">
            <div class="searchPopup-inputSearch">
                <%--<input class="form-control SearchInput popupSearch-input" placeholder="Refine Search" name="srch-term" id="Globalsearch" type="text">--%>
                <asp:TextBox ID="txtWildCard" runat="server" CssClass="form-control SearchInputPopup popupSearch-input" placeholder="Refine Search"></asp:TextBox>
            </div>
            <div style="display: inline-block">
                <%--<button id="btnGlobalsearch" class="btn btn-default search-btn" type="submit">Search</button>--%>
                <asp:Button ID="btnGlobalsearch" runat="server" CssClass="btn btn-default search-btn" Text="Search" OnClientClick="return checkEmptyString();" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="tabs-left" style="margin-top: 25px;">
                <ul class="nav nav-tabs">
                    <li class="active" id="serviceCatlogLink" runat="server">
                        <a href="#a" data-toggle="tab">
                            <div class="tab-listIcon serviceCatlog-icon">
                                <span class="">
                                    <i class="fas fa-user-cog"></i>
                                </span>
                            </div>
                            <div class="tab-listName">
                                <span>Service Catalog</span>
                            </div>
                        </a>
                    </li>
                    <li id="userRequestLink" runat="server">
                        <a href="#b" data-toggle="tab">
                            <div class="tab-listIcon">
                                <span class="">
                                    <i class="fas fa-clipboard-list"></i>
                                </span>
                            </div>
                            <div class="tab-listName">
                                <span>User Request</span>
                            </div>
                        </a>
                        <%-- <div>
                                <asp:CheckBox ID="chkIncludeClosed" runat="server" Text="&nbsp;&nbsp;Include Closed"
                                AutoPostBack="true" onchange="LoadingPanel.Show();" />
                        </div>--%>
                        <%--<div>
                                <label class="gbSearch-label">Module:</label>
                                <div class="gbSearch-drpDownWrap">
                                    <asp:DropDownList ID="ddlModuleName" runat="server" OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged" onchange="LoadingPanel.Show();"
                                        AutoPostBack="true" CssClass="aspxDropDownList" Width="100%"></asp:DropDownList>
                                </div>
                        </div>--%>
                        <div>
                        </div>
                        <div>
                        </div>
                        <div>
                        </div>

                    </li>
                    <li id="wikiList" runat="server">
                        <a href="#c" data-toggle="tab">
                            <div class="tab-listIcon">
                                <span class="">
                                    <i class="fab fa-wikipedia-w"></i>
                                </span>
                            </div>
                            <div class="tab-listName">
                                <span>Wiki</span>
                            </div>
                        </a>
                    </li>
                </ul>
                <div class="tab-content">
                    <div id="ServiceCatalogsearch" runat="server">
                        <div class="tab-pane active" id="a">
                            <div class="col-md-12 col-sm-12 col-xs-12 search-header">
                                <p>SEARCH RESULT</p>
                            </div>
                            <dx:ASPxGridView ID="globalServiceCatalog" ClientInstanceName="globalServiceCatalog" runat="server" KeyFieldName="TicketId" AutoGenerateColumns="false"
                                Width="100%" OnHtmlRowPrepared="globalServiceCatalog_HtmlRowPrepared"
                                Settings-ShowColumnHeaders="false">
                                <Columns>
                                    <dx:GridViewDataColumn>
                                        <DataItemTemplate>
                                            <dx:ASPxLabel runat="server" Text='<%# Eval("Title") %>' CssClass="globalSearch-title" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn>
                                        <DataItemTemplate>
                                            <dx:ASPxLabel runat="server" Text='<%# Eval("ServiceDescription") %>' CssClass="globalSearch-desc" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn>
                                        <DataItemTemplate>
                                            <dx:ASPxHeadline runat="server" HeaderText='<%#  Convert.ToDateTime(Eval("Created")).ToString("MMM-d-yyyy") %>' CssClass="globalSearch-created"></dx:ASPxHeadline>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                </Columns>
                                <Styles>
                                    <Row CssClass="customrowheight homeGrid_dataRow"></Row>
                                </Styles>
                            </dx:ASPxGridView>
                        </div>
                    </div>
                    <div id="UserRequestsSearch" runat="server">
                        <div class="tab-pane row" id="b">
                            <div class="col-md-12 col-sm-12 col-xs-12 search-header">
                                <p>SEARCH RESULT</p>
                            </div>
                            <%--<div style="text-align:center;">
                                <img src="/Content/Images/search-userRequest.png" width="30px" />
                                <h4 style="text-align: center; display:inline-block">User Requests</h4>
                            </div>--%>
                            <div class="col-md-12 col-sm-12 col-xs-12" style="padding: 15px; box-shadow: 0px 0px 8px rgba(0, 0, 0, 0.1);">
                                <div class="row">
                                    <div class="col-md-2 col-sm-2 col-xs-12" style="padding: 22px 0 0">
                                        <div class="crm-checkWrap">
                                            <asp:CheckBox ID="chkIncludeClosed" runat="server" Text="&nbsp;&nbsp;Include Closed"
                                                AutoPostBack="true" onchange="LoadingPanel.Show();" TextAlign="Right" />
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-2 col-xs-12">
                                        <label class="gbSearch-label">Module:</label>
                                        <div class="gbSearch-drpDownWrap">
                                            <asp:DropDownList ID="ddlModuleName" runat="server" OnSelectedIndexChanged="ddlModuleName_SelectedIndexChanged" onchange="LoadingPanel.Show();"
                                                AutoPostBack="true" CssClass="aspxDropDownList itsmDropDownList" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-2 col-xs-12">
                                        <label class="gbSearch-label">Field:</label>
                                        <div class="gbSearch-drpDownWrap">
                                            <asp:DropDownList ID="lstFilteredFields" runat="server"
                                                CssClass="aspxDropDownList itsmDropDownList" Width="100%">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-2 col-sm-3 col-xs-12">
                                        <label class="gbSearch-label">From:</label>
                                        <div class="">
                                            <dx:ASPxDateEdit ID="dtFrom" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16" 
                                                CssClass="CRMDueDate_inputField" runat="server" Visible="true">
                                            </dx:ASPxDateEdit>
                                        </div>
                                     </div>
                                     <div class="col-md-2 col-sm-3 col-xs-12">
                                            <label class="gbSearch-label">To:</label>
                                            <div class="">
                                                <dx:ASPxDateEdit ID="dtTo" runat="server" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16" 
                                                    CssClass="CRMDueDate_inputField" Visible="true"></dx:ASPxDateEdit>
                                            </div>
                                     </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="wikisearch" runat="server">
                        <div class="tab-pane" id="c">
                            <div class="col-md-12 col-sm-12 col-xs-12 search-header">
                                <p>SEARCH RESULT</p>
                            </div>

                            <ugit:ASPxGridView ID="globalSearchPage" ClientInstanceName="globalSearchPage" runat="server" KeyFieldName="TicketId" Width="100%"
                                OnHtmlRowPrepared="globalSearchPage_HtmlRowPrepared"
                                Settings-ShowColumnHeaders="false" UseFixedTableLayout="false" CssClass="customgridview homeGrid SVCHomeGrid" OnHtmlDataCellPrepared="globalSearchPage_HtmlDataCellPrepared">

                                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                <SettingsCommandButton>
                                    <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                    <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                </SettingsCommandButton>

                                <Columns>
                                    <dx:GridViewDataColumn Width="25%">
                                        <DataItemTemplate>
                                            <dx:ASPxLabel runat="server" Text='<%# Eval("Title") %>' CssClass="globalSearch-title" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn Width="15%">
                                        <DataItemTemplate>
                                            <dx:ASPxHeadline runat="server" HeaderText='<%# Eval("TicketId") %>' CssClass="globalSearch-link"></dx:ASPxHeadline>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn Width="45%">
                                        <DataItemTemplate>
                                            <dx:ASPxLabel runat="server" Text='<%# Eval("WikiSnapshot") %>' CssClass="globalSearch-desc" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                    <dx:GridViewDataColumn Width="15%">
                                        <DataItemTemplate>
                                            <dx:ASPxHeadline runat="server" HeaderText='<%#  Convert.ToDateTime(Eval("Created")).ToString("MMM-d-yyyy") %>' CssClass="globalSearch-created"></dx:ASPxHeadline>
                                        </DataItemTemplate>
                                    </dx:GridViewDataColumn>
                                </Columns>

                                <Styles>
                                    <Row CssClass="customrowheight homeGrid_dataRow"></Row>
                                </Styles>
                                <%-- <Styles>
                                    <Row CssClass="globalSearch-dataRow"></Row>
                                    <PagerBottomPanel CssClass="gridFooter-pager"></PagerBottomPanel>
                                </Styles>--%>
                            </ugit:ASPxGridView>
                        </div>
                    </div>
                </div>

                <div class="row" id="dvServiceUserRequest">
                    <div class="col-md-12 col-sm-12 col-xs-12" style="padding-top: 15px">
                        <ugit:ASPxGridView ID="globalServiceUserRequest" ClientInstanceName="globalServiceUserRequest" runat="server" KeyFieldName="TicketId" Width="100%"
                            OnHtmlDataCellPrepared="globalServiceUserRequest_HtmlDataCellPrepared" AutoGenerateColumns="false" OnHtmlRowPrepared="globalServiceUserRequest_HtmlRowPrepared1">
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                            <SettingsCommandButton>
                                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                            </SettingsCommandButton>

                        </ugit:ASPxGridView>
                        <styles>
                            <row cssclass="customrowheight homeGrid_dataRow"></row>
                        </styles>
                    </div>

                    <script type="text/javascript">
                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                    </script>

                </div>
            </div>
        </div>
    </div>
    <%-- <script>
      $(document).ready(function () {
          $(".tabs-left ul li:first-child a").trigger("click")
      })
  </script>
    --%>
</div>

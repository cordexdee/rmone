<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryWizardPreview.ascx.cs" Inherits="uGovernIT.Web.QueryWizardPreview" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .excel-button {
        background-image: url('/content/images/excel-icon.png');
    }

    .export-buttons {
        padding-left: 3px;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        border-top: 1px solid #A5A5A5;
        /*color: #000000;*/
        padding-bottom: 6px;
        padding-right: 8px;
        padding-top: 3px;
        width: 150px;
        text-align: right;
    }
    .headerLabel {
        font-size: 14px;
        font-weight: bold;
        padding: 6px;
        display: block;
    }
    .panel-parameter {
        width: 400px;
        border: 1px solid #ccc;
        padding: 15px;
    }

    .table-header {
        background-color: #E8EDED;
    }

    a:hover {
        text-decoration: none !important;
    }
    
    .paddings{
        padding:2px;
    }
    .leftpadding{
        padding-left:10px;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function EmptyWhereClause() {
        $('#<%=hdnwhereFilter.ClientID%>').val('');
        $('#<%=hdnbuttonClicked.ClientID%>').val('');
    }
    function buttonClick() {
        var isvalue = true;
        var numParam = $(".param-value").length;
        var paramValue = new Array(numParam);
        var index = 0;

        $(".param-value").each(function () {
            var paramtype = $(this).attr("paramtype");
            if (paramtype == "user" || paramtype == "group" || paramtype == "usergroup") {
                var spans = $(this).find("span table table span");
                $(spans).each(function () {
                    if ($(this).html() != "") {
                        paramValue[index] = $(this).html();
                        return;
                    }
                });

                if ($(this).parent().find('span').hasClass("mandatory")) {
                    if (paramValue[index] == "") {
                        isvalue = false;
                        //return false;
                    }
                }

            }
            else if (paramtype == "date") {
                var input = $(this).find("input");
                paramValue[index] = $(input).val();
            }
            else {
                if ($(this).find("select").length > 0) {
                    var input = $(this).find("select").val();
                    if (input != null) {
                        paramValue[index] = input;
                    }
                }
                else {
                    var input = $(this).find("input");
                    if (input != null) {
                        paramValue[index] = $(input).val();
                    }
                }
            }

            if ($(this).parent().find('span').hasClass("mandatory")) {
                if (paramValue[index] == "") {
                    isvalue = false;
                    return false;
                }
            }

            index++;

        });
        if (isvalue == true) {
            $('#<%=hdnbuttonClicked.ClientID%>').val('true');
            return true;
        }
        return false;
    }

    function setFilterMode(obj) {
        var filterMode = '<%= FilterMode%>';
        if (filterMode == 'True') {
            __doPostBack('<%=imgAdvanceMode.UniqueID %>', '__HideFilterMode__');

        }
        else {
            __doPostBack('<%=imgAdvanceMode.UniqueID %>', '__ShowFilterMode__');
        }
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        set_cookie('UseManageStateCookies', 'true', null, "<%= serverrelativeurl %>");
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function readCookie(cookieName) {
        var re = new RegExp('[; ]' + cookieName + '=([^\\s;]*)');
        var sMatch = (' ' + document.cookie).match(re);
        if (cookieName && sMatch) return unescape(sMatch[1]);
        return '';
    }

    var isZoomIn;
    function GZoomIn() {

        var zoomLevel = readCookie("ZoomLevel")
        if (zoomLevel == 'Monthly' || zoomLevel == 'Quarterly') {
            $('#<%=btnExcelExport.ClientID%>').hide();
            $('#<%=btnPdfExport.ClientID%>').hide();
        }
        else {
            $('#<%=btnExcelExport.ClientID%>').show();
            $('#<%=btnPdfExport.ClientID%>').show();
        }
        if (typeof (grid) != 'undefined' && !grid.InCallback()) {
            grid.PerformCallback('+');
        }
    }

    function GZoomOut() {

        var zoomLevel = readCookie("ZoomLevel")
        if (zoomLevel == 'Weekly') {
            $('#<%=btnExcelExport.ClientID%>').hide();
            $('#<%=btnPdfExport.ClientID%>').hide();
        }
        else {
            $('#<%=btnExcelExport.ClientID%>').show();
            $('#<%=btnPdfExport.ClientID%>').show();
        }

        if (typeof (grid) != 'undefined' && !grid.InCallback()) {
            grid.PerformCallback('-');
        }
    }

    function OpenSendEmailWindow(url) {
        //var url = hdnConfiguration.Get("SendEmailUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Send Email - Query Report', '800px', '600px', 0, escape(requestUrl))
        return false;
    }


    function SendMailClick() {
        loadingpanel.Show();
        cbMailsend.PerformCallback("SendMail");
    }

    function SaveToDocument() {
        loadingpanel.Show();
        cbMailsend.PerformCallback("SaveToDoc");
    }

    function OpenSaveToDocument(url) {
        //var url = hdnconfiguration.Get("SaveToDocumentUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        var param = "type=tsksummaryreport&IsSelectFolder=true";
        UgitOpenPopupDialog(url, param, 'Save to Folder', '800px', '400px', 0, escape(requestUrl))
        return false;
    }

    function OnCallbackComplete(s, e) {
        loadingpanel.Hide();
        var result = e.result
        if (result != null && result.length > 0) {
            if (e.parameter.toString() == "SendMail") {
                OpenSendEmailWindow(result);
            }
            else if (e.parameter.toString() == "SaveToDoc") {
                OpenSaveToDocument(result);
            }
        }
        else {
            alert("No record found.");
        }
    }
    function OpenEditDialog(s, e) {
        loadingpanel.Show();
        s.GetRowValues(e.visibleIndex, 'EnableEditUrl', OnGetSelectedFieldValues);
    }

    function OnGetSelectedFieldValues(selectedValues) {
        loadingpanel.Hide();
        if (selectedValues == undefined || selectedValues == '') return;

        var splited = selectedValues.split('&');
        var height;
        var width;
        if (splited.length > 0) {
            $.each(splited, function (index, value) {
                if (value.startsWith('Height')) {
                    height = value.split('=')[1];
                }
                else if (value.startsWith('Width')) {
                    width = value.split('=')[1];
                }
            });
        }
        var baseUrl = "<%= HttpContext.Current.Request.Url.AbsolutePath%>";

        UgitOpenPopupDialog(baseUrl + selectedValues, '', '', width + 'px', height + 'px', 0, escape("<%= Request.Url.AbsolutePath %>"))
    }
</script>
<asp:PlaceHolder ID="phStyleGrid" runat="server" Visible="false">
    <style>
        .dxgvHeader_DevEx {
            background-image: none !important;
        }
    </style>
</asp:PlaceHolder>
<asp:Panel ID="pnlWizardPreview" runat="server">
    <dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
    <dx:ASPxLoadingPanel ID="loadingpanel" runat="server" Modal="true" ClientInstanceName="loadingpanel"></dx:ASPxLoadingPanel>
    <dx:ASPxCallback ID="cbMailsend" runat="server" ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback">
        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
    </dx:ASPxCallback>

    <asp:HiddenField runat="server" ID="hdnbuttonClicked" />
    <span style="color: red;">
        <asp:Label ID="lblMsg" Text="" runat="server" />
    </span>
    <asp:Panel ID="pnlParameter" runat="server" CssClass="panel-parameter"></asp:Panel>
    <asp:Panel ID="pnlgrid" runat="server">
        <asp:HiddenField runat="server" ID="hdnwhereFilter" />
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row" id="trToolBar" runat="server">
                <div class="col-md-12 col-sm-12 col-xs-12 noPading">
                     <div id="dvZoomLevel" runat="server" style="padding-top: 5px; float: left;">
                         <!--As discussed with Prasad, zoom icons are not required here.  -->
                        <img src="/content/Images/zoom_plus.png" alt="Zoom In" id="Img1"
                            style="width: 23px; height: 23px; padding-right: 2px; display:none;" onclick="GZoomIn()" title="Zoom In" />
                        <img src="/content/Images/zoom_minus.png" alt="Zoom Out" id="Img2"
                            style="width: 23px; height: 23px; padding-right: 8px; display:none;" onclick="GZoomOut()" title="Zoom Out" />

                        <dx:ASPxHiddenField ID="hdnZoomLevel" runat="server" ClientInstanceName="hdnZoomLevel"></dx:ASPxHiddenField>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12 col-xs-12" style="padding:15px 0 0 0;">
                    <div class="toolbarWrap" style="float: right;">
                        <div id="export-options" style="background: #fff;">
                            <span class="fleft">
                                <img id="imgAdvanceMode" runat="server" style="cursor: pointer; margin-top: -3px; width: 24px;" onclick="setFilterMode(this)" />
                            </span>
                            <span class="fleft">
                                <dx:ASPxButton ID="btnCSVExport" runat="server" CssClass="export-buttons"  ToolTip="CSV Export" EnableTheming="false" UseSubmitBehavior="False"
                                    OnClick="btnCSVExport_Click" RenderMode="Link">
                                    <Paddings PaddingLeft="2px" PaddingRight="2px" />

                                    <Image >
                                        <SpriteProperties CssClass="csvicon"/>
                                    </Image>
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />

                                </dx:ASPxButton>
                            </span>
                            <span class="fleft">
                                <dx:ASPxButton ID="btnExcelExport" runat="server" EnableTheming="false" ToolTip="Excel Export" UseSubmitBehavior="False"
                                    OnClick="btnExcelExport_Click" RenderMode="Link">

                                    <Image>
                                        <SpriteProperties CssClass="excelicon" />
                                    </Image>
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>
                            <span class="fleft">
                                <dx:ASPxButton ID="btnPdfExport" runat="server" CssClass="export-buttons" ToolTip="PDf Export" EnableTheming="false" UseSubmitBehavior="False" RenderMode="Link"
                                    OnClick="btnPdfExport_Click">
                                    <Image>
                                        <SpriteProperties CssClass="pdf-icon" />
                                    </Image>
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>
                            <span class="fleft">
                                <dx:ASPxButton ID="SendEmail" runat="server" CssClass="export-buttons" AutoPostBack="false"
                                    EnableTheming="false" UseSubmitBehavior="False" RenderMode="Link">
                                    <Image>
                                        <SpriteProperties CssClass="sendmail" />
                                    </Image>
                                    <ClientSideEvents Click="function(s,e) { SendMailClick(); }" />
                                </dx:ASPxButton>
                            </span>
                            <span class="fleft">
                                <dx:ASPxButton ID="SaveDocument" runat="server" EnableTheming="false"
                                    UseSubmitBehavior="False" AutoPostBack="false" RenderMode="Link">
                                    <Image>
                                        <SpriteProperties CssClass="savetofolder" />
                                    </Image>
                                    <ClientSideEvents Click="function(s,e) { SaveToDocument(); }" />
                                </dx:ASPxButton>
                            </span>
                            <span class="fleft" style="display: none">
                                <asp:ImageButton AlternateText="Print" ID="imgPrint" ImageUrl="~/content/images/print-icon.png" runat="server" OnClick="btnExport_Click" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div style="text-align: left">
                    <img id="imgLogo" runat="server" style="cursor: pointer; margin-top: -3px; width: 50px;" src="" />
                </div>
                <div style="text-align: center">
                    <asp:Label ID="lblReportTitle" runat="server"></asp:Label>
                </div>
                <div style="text-align: right">
                    <asp:Label ID="lblAdditionalInfo" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                 <div style="display:inline-block;">
                    <dx:ASPxImage runat="server" ID="imgExpand" Text="Expand All Rows" ImageUrl="~/content/images/expand-all.png"
                        AutoPostBack="false">
                        <ClientSideEvents Click="function() { grid.ExpandAll() }" />
                    </dx:ASPxImage>
                </div>
                <div style="display:inline-block;">
                    <dx:ASPxImage runat="server" ID="imgCollapse" Text="Collapse All Rows" ImageUrl="~/content/images/collapse-all.png"
                        AutoPostBack="false">
                        <ClientSideEvents Click="function() { grid.CollapseAll() }" />
                    </dx:ASPxImage>
                </div>
            </div>
            <div class="row">
                <ugit:ASPxGridView ID="gvPreview"   runat="server"  Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" AutoGenerateColumns="true"
                          OnHeaderFilterFillItems="gvPreview_HeaderFilterFillItems" OnHtmlRowCreated="gvPreview_HtmlRowCreated" 
  						 	OnSummaryDisplayText="gvPreview_SummaryDisplayText" CssClass="customgridview homeGrid">
                        <SettingsCookies Enabled="false" />
                        <%-- <Styles>
                            <Header CssClass="gvpreview-header"></Header>
                        </Styles>--%>
                        <Styles>
                            <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                             <Row CssClass="estReport-dataRow"></Row>
                             <GroupRow CssClass="estReport-gridGroupRow"></GroupRow>
                             <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                             <Header HorizontalAlign="Center" CssClass="estReport-gridHeaderRow"></Header>
                             <Footer CssClass="report-footerRow"></Footer>
                        </Styles>
                    </ugit:ASPxGridView>
            </div>
        </div>
        <table style="width: 99%;">
           
           
            <tr>
                <td colspan="2">


                    <div style="float: left;">
                        <table>
                            <tr>
                               
                            </tr>
                        </table>
                    </div>
                    
                    <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvPreview"></dx:ASPxGridViewExporter>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
<asp:Panel runat="server" ID="pnlFormatPreview" Style="text-align: left !important">
</asp:Panel>

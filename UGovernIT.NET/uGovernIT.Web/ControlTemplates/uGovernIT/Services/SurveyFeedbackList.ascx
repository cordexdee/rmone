
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyFeedbackList.ascx.cs" Inherits="uGovernIT.Web.SurveyFeedbackList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .pagerBox td table tr td span {
        /* font-size : larger; */
        border: 1px solid black;
        padding: 0px 3px;
    }

    .grid-footerrow {
        background: #D0F6F2;
        font-weight: bold;
        text-align: center;
    }

    .pncalculatedrow {
        display: none;
    }
</style>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .pagerBox td table tr td span {
        /* font-size : larger; */
        border: 1px solid black;
        padding: 0px 3px;
    }

    .moduleimgtd {
        padding: 0 5px 0 0;
        width: 35px;
    }

    .tickettypetab {
        float: left;
        padding: 4px 4px 4px 4px;
    }

    .tickettypetabsel {
        float: left;
        padding: 4px 4px 4px 4px;
    }

    .linkseprator {
        padding-left: 3px;
    }

    .fixedbutton a:hover {
        text-decoration: none;
    }

    .dnone {
        display: none;
    }

    .search_bg {
        float: right;
        background: url("bgximg-4DDA2070.png?ctag") repeat-x 0 -511px #FFFFFF;
    }

    .moduledesciptiontd {
        padding-bottom: 15px;
    }

    table.ms-listviewtable > tbody > tr > td {
        border: none;
    }

    .ms-viewheadertr .ms-vh2-gridview {
        background: transparent !important;
        height: 22px;
    }

    a.newlinkbutton, a.newlinkbutton:hover {
        text-decoration: none;
    }

    .dnode {
        display: none;
    }

    .top_right_nav {
        margin: 0;
        text-align: right;
        position: relative;
        left: 15px;
        float: right;
    }

        .top_right_nav span {
            float: left;
            width: auto;
            margin-left: 5px;
        }

    .ms-viewheadertr th[align="center"] {
        text-align: center !important;
    }


    .headercenter {
        text-align: center !important;
    }

        .headercenter td {
            text-align: center !important;
        }

    .fleft {
        float: left;
    }

    .img[alt="Open Menu"] {
        border: none;
    }


    .button-bg {
        color: white;
        background: url("/Content/images/firstnavbg.gif") repeat-x scroll 0 0 transparent;
        float: left;
        margin: 1px;
        padding: 6px;
        cursor: pointer;
    }

        .button-bg:hover {
            background: url("/Content/images/firstnavbg_hover.gif") repeat-x scroll 0 0 transparent;
        }

    .ModuleBlock {
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        position: absolute;
        z-index: 100;
    }

    .reportitem {
        border-bottom: 1px solid black;
        cursor: pointer;
        color: black;
    }

    .menuTextCell {
        padding-left: 2px;
        padding-top: 2px;
        height: 18px;
    }

    .menuTable {
        width: 100%;
        border-collapse: collapse;
    }

    .reportItemSelected {
        background-color: aqua;
    }

    .linkcss {
        color: white;
        text-decoration:none !important;
    }

    .settext {
        float: left;
        padding-left: 6px;
        padding-right: 6px;
    }

    .hidebutton {
        display: none;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function opensurveyreportviewer() {
        var params = "";
        var from = "";
        var to = "";
        var filterids = "";
        var selectedsurveyhere = "";
        filterids = '<%=strfilterids%>';
        params = "feedbackids=" + filterids;
        if ('<%=fromdatefilter%>' != "")
            from = '<%=fromdatefilter%>';
        if ('<%=todatefilter%>' != "")
            to = '<%=todatefilter%>';
        selectedsurveyhere = '<%=selectedsurvey%>';
        params = params + "&type=" + '<%=type%>' + "&survey=" + '<%=survey%>';
        params = params + "&selectedsurvey=" + selectedsurveyhere
        var filter = "<%=filterExpression%>";
        if (filter != null && filter != '' && filter != undefined)
            params += "&FilterExp=" +escape(filter);
        if (from != "" && to != "")
            params = params + "&from=" + from + "&to=" + to;
        var url = '<%=reportviewrUrl %>' +"?reportName=SurveyFeedbackReport&isdlg=1";
        if (params != "")
            window.parent.UgitOpenPopupDialog(url, params, 'Survey Feedback Report', '100', '100', 0);
        else {
            alert('Please select survey');
            return false
        }
    }

    function downloadExcel(obj) {
        if ($('#<%=btnInvokeClick.ClientID%>') != null) {
            $('#<%=btnInvokeClick.ClientID%>').trigger('click');
        }
    }

    function setFormSubmitToFalse() {
        setTimeout(function () { _spFormOnSubmitCalled = false; _spSuppressFormOnSubmitWrapper = true; }, 3000);

        return true;
    }
</script>
<%--<asp:UpdatePanel ID="uppanel" runat="server">
    <ContentTemplate>--%>
        <div>
            <dx:ASPxLoadingPanel ID="surveyfeedbackloading" ClientInstanceName="surveyfeedbackloading" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
        </div>
        <div id="scriptPanel" runat="server" visible="false">
            <script type="text/javascript">
                opensurveyreportviewer();
            </script>
        </div>
<div style="padding:10px;">
<div id="header" style="padding-bottom: 10px; float: left;">
    <div class="d-flex">
        <b style="padding-top: 4px; font-weight: normal; width: 101px;">Select Type: </b>
        <asp:DropDownList ID="ddlType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged" Height="27px" Width="150px" CssClass="itsmDropDownList">
            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
            <asp:ListItem Text="Module" Value="Module"></asp:ListItem>
            <asp:ListItem Text="Generic" Value="Generic"></asp:ListItem>
        </asp:DropDownList>
    </div>
    <div class="d-flex mt-1">
        <b style="padding-top: 4px; font-weight: normal; width: 101px;">Select Survey: </b>
        <span style="width: 100%;">
            <asp:DropDownList ID="ddlSurvey" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSurvey_SelectedIndexChanged" Height="27px" Width="150px" CssClass="itsmDropDownList"></asp:DropDownList>
        </span>
    </div>
</div>
<div style="float: right;">
    <div style="float: left;">
        <div style="float: left;">
            <span>
                <b style="padding-top: 4px; float: left; font-weight: normal;padding-right:3px;">From: </b>
                <span style="position: relative; float: left; top: 1px;">
                    <dx:ASPxDateEdit ID="dtFrom" runat="server" TimeSectionProperties-Visible="false" ></dx:ASPxDateEdit>
                    <%--<SharePoint:DateTimeControl IsRequiredField="false" ValidateRequestMode="Disabled" CssClassTextBox="inputTextBox datetimectr111" DateOnly="true" ID="dtFrom"
                        runat="server" ToolTip="" />--%>
                </span>
            </span>
        </div>
        <div style="float: right;padding-left:10px;">
            <span>
                <b style="padding-top: 4px; float: left; font-weight: normal;padding-right:3px;">To: </b>
                <span style="position: relative; float: left; top: 1px;">
                    <dx:ASPxDateEdit ID="dtTo" runat="server" TimeSectionProperties-Visible="false" DateRangeSettings-StartDateEditID="dtFrom"></dx:ASPxDateEdit>
                    <%--<SharePoint:DateTimeControl IsRequiredField="false" ValidateRequestMode="Disabled" CssClassTextBox="inputTextBox datetimectr111" DateOnly="true" ID="dtTo"
                        runat="server" ToolTip="" />--%>
                </span>
            </span>
        </div>
    </div>
    <div style="float: right;padding-left:10px;">
        <div class="settext">
            <dx:ASPxButton ID="lnkfilter" runat="server" Text="Apply Filter&nbsp;&nbsp;" CssClass="primary-blueBtn" OnClick="lnkfilter_Click" ></dx:ASPxButton>
        </div>
        <div class="settext">
            <dx:ASPxButton ID="lnkClear" runat="server" Text="Clear Filter" CssClass="primary-blueBtn" OnClick="lnkClear_Click"></dx:ASPxButton>
        </div>
                <div style="float: left; padding-left: 5px;">
                    <span id="onlyExcelExport" style="padding-left: 2px; padding-top: 6px" class="fright" runat="server" visible="true">
                        <img src="/Content/images/excel-icon.png" alt="Excel Export" title="Excel Export" onclick="downloadExcel(this);" style="cursor: pointer; width: 16px;" />
                    </span>
                    <asp:Button ID="btnInvokeClick" runat="server" OnClick="btnInvokeClick_Click" OnClientClick="javascript:setFormSubmitToFalse()" CssClass="hidebutton" />
                </div>
                <div style="float: left; padding-left: 5px;padding-top: 6px">
                    <span style="padding-left: 5px;">
                        <asp:ImageButton ImageUrl="/Content/images/Reports_16x16.png" ID="rptSurveyFeedback" OnClientClick="surveyfeedbackloading.Show();" ToolTip="Survey Feedback Report" runat="server" OnClick="rptSurveyFeedback_Click" />
                        <%--  <img src="/_layouts/15/images/ugovernit/Reports_16x16.png" id="rptfeedback" onclick="opensurveyreportviewer();" />--%>
                    </span>
                </div>
            </div>

        </div>

        <div style="margin-top: 5px; margin-bottom: 11px;">
    <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowGroup="true" SettingsBehavior-AllowSort="true" SettingsBehavior-EnableRowHotTrack="false" Images-HeaderActiveFilter-Url="/Content/images/Filter_Red_24.png"
                OnDataBinding="grid_DataBinding" Settings-ShowFooter="true" OnSummaryDisplayText="grid_SummaryDisplayText"
                OnHtmlRowPrepared="grid_HtmlRowPrepared"
                ClientInstanceName="grid"
                Theme="DevEx" Width="100%" KeyFieldName="ID" CssClass="customgridview dxgvControl_UGITNavyBlueDevEx">
                <Columns>
                </Columns>
                <Settings ShowHeaderFilterButton="true" ShowGroupPanel="true" />
                <SettingsPager Mode="ShowPager" PageSize="20" >
                    <PageSizeItemSettings Visible="true" ShowAllItem="true"></PageSizeItemSettings>
                </SettingsPager>
                <Styles>
                    <Row CssClass="customrowheight"></Row>
                    <Footer HorizontalAlign="Center"></Footer>
                </Styles>

            </ugit:ASPxGridView>
        </div>
   
    </div>
    <%-- </ContentTemplate>
</asp:UpdatePanel>--%>


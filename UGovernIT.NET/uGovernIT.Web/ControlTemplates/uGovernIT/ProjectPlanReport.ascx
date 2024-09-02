<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectPlanReport.ascx.cs" Inherits="uGovernIT.Web.ProjectPlanReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function next() {
        $("[id$='_hndYear']").val(parseInt($("[id$='_hndYear']").val()) + 1);
        $("#<%= lblSelectedYear.ClientID %>").val($("[id$='_hndYear']").val());       
        onChange();
    }

    function prev() {
        $("[id$='_hndYear']").val(parseInt($("[id$='_hndYear']").val()) - 1);
        $("#<%= lblSelectedYear.ClientID %>").val($("[id$='_hndYear']").val());
        onChange();
    }

    function onChange() {
        LoadingPanel.SetText('Loading ...');
        LoadingPanel.Show();
    }

    function SendMailClick() {
        LoadingPanel.Show();
        cbMailsend.PerformCallback("SendMail");
    }

    function OnCallbackComplete(s, e) {
        LoadingPanel.Hide();
        var result = e.result
        if (result != null && result.length > 0) {
            if (e.parameter.toString() == "SendMail") {
                OpenSendEmailWindow(result);
            }           
        }
        else {
            alert("No record found.");
        }
    }
    $(function () {
        adjustControlSize();
    });

    function OpenSendEmailWindow(url) {
        
        UgitOpenPopupDialog(url, '', 'Send Email - Project Plan Report', '800px', '600px', 0, escape('<%=Request.Url.AbsolutePath%>'))
        return false;
    }

    function adjustControlSize() {

        setTimeout(function () {           
            $("#s4-workspace").width("100%");
            grid.AdjustControl();
            grid.SetWidth($(window).width() - 10);
            var height = $(window).height();
            grid.SetHeight(height - 100);
        }, 10);
    }

</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grid.SetHeight(containerHeight);
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
    .bg-Completed{
        background-color:#C6EFCE;
        padding: 5px 3px 5px 3px;    }
    .bg-InProgress{
        background-color:#FFCC99;
        padding: 5px 3px 5px 3px;    }
    .bg-Planned{
        background-color:#B4C6E7;
        padding: 5px 3px 5px 3px;
    }
     /*.excelicon, .pdf-icon, .sendmail, .savetofolder, .csvicon {
        background-repeat: no-repeat;
        width: 18px;
        /*height: 22px;
    }*/

    /*.excelicon {
        background-image: url(/Content/images/excel_icon.png);
    }
    .pdf-icon {
        background-image: url(/Content/images/pdf_icon.png);
    }*/
    /*.sendmail {
        background-image: url(/Content/Images/MailTo16X16.png);
    }*/
</style>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True"></dx:ASPxLoadingPanel>
 <dx:ASPxCallback ID="cbMailsend" runat="server" ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback">
        <ClientSideEvents CallbackComplete="OnCallbackComplete"></ClientSideEvents>
</dx:ASPxCallback>
 
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <asp:HiddenField ID="hndYear" runat="server" Value="" />
        <div id="dvYearNavigation" runat="server" style="float: left; margin-left: 2px; padding-left: 5px; padding-right: 10px;">
            <span style="padding-right: 5px;">
                <asp:ImageButton ImageUrl="/Content/images/back-arrowBlue.png" ID="previousYear" ToolTip="Previous" runat="server" 
                    OnClientClick="prev()" OnClick="previousYear_Click" Width="18" />
            </span>
            <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
            <span style="padding-left: 5px;">
                <asp:ImageButton ImageUrl="/Content/images/next-arrowBlue.png" ID="nextYear" ToolTip="Next" runat="server" 
                    OnClientClick="next()" OnClick="nextYear_Click" Width="18" />
            </span>
        </div>
    </div>
    <div class="row">
        <dx:ASPxButton ID="btnExcelExport" runat="server" CssClass="export-buttons" EnableTheming="false" ToolTip="Excel Export" UseSubmitBehavior="False"
            OnClick="btnExcelExport_Click" RenderMode="Link">
            <Image><SpriteProperties CssClass="excelicon" /></Image>
            <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnPdfExport" runat="server" CssClass="export-buttons" ToolTip="PDf Export" EnableTheming="false" UseSubmitBehavior="False" 
            RenderMode="Link" OnClick="btnPdfExport_Click">
            <Image><SpriteProperties CssClass="pdf-icon" /></Image>
            <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="SendEmail" runat="server" CssClass="export-buttons" AutoPostBack="false" EnableTheming="false" UseSubmitBehavior="False" 
            RenderMode="Link">
            <Image><SpriteProperties CssClass="sendmail" /></Image>
            <ClientSideEvents Click="function(s,e) { SendMailClick(); }" />
        </dx:ASPxButton>

        <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssClass="customgridview homeGrid"
            OnHtmlRowPrepared="grid_HtmlRowPrepared" Width="100%"
            ClientInstanceName="grid" KeyFieldName="TicketId">
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="TicketId" Caption="Id" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False" Width="120px" 
                        HeaderStyle-HorizontalAlign="Center" FixedStyle="Left">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" Width="150px"
                        FixedStyle="Left">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PriorityLookup" Caption="Priority" CellStyle-HorizontalAlign="Center" CellStyle-Wrap="False" 
                        HeaderStyle-HorizontalAlign="Center" FixedStyle="Left">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Status" Caption="Stage" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" 
                        FixedStyle="Left">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="PctComplete" Caption="Progress" Width="100px"  CellStyle-HorizontalAlign="Center" 
                        HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-DisplayFormatString="{0:N}%" FixedStyle="Left">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Jan" PropertiesTextEdit-EncodeHtml="false" Caption="Jan" CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Feb" PropertiesTextEdit-EncodeHtml="false" Caption="Feb" CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Mar" PropertiesTextEdit-EncodeHtml="false" Caption="Mar"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Apr" PropertiesTextEdit-EncodeHtml="false" Caption="Apr"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="May" Caption="May" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Jun" Caption="Jun" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Jul" Caption="Jul" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Aug" Caption="Aug" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Sep" Caption="Sep" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Oct" Caption="Oct" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Nov" Caption="Nov" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Dec" Caption="Dec" PropertiesTextEdit-EncodeHtml="false"  CellStyle-BackColor="White" 
                        CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <Settings ShowFilterRow="false" VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto"/>
                <SettingsBehavior EnableRowHotTrack="false" AllowSelectByRowClick="false" AllowSelectSingleRowOnly="true" AllowSort="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                    <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                </Styles>
                <SettingsPager Position="TopAndBottom" Mode="ShowAllRecords">
                    <PageSizeItemSettings ShowAllItem="true" Items="10, 15, 20, 25, 50, 75, 100" />
                </SettingsPager>
            </dx:ASPxGridView>
             <script type="text/javascript">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvPreview"></dx:ASPxGridViewExporter>
    </div>
</div>
<table>
    <tr><td>
        <div id="dvCustomFilter" style="float: left; padding-top: 10px;">
    <table>
        <tr>
            <td style="background-color: #C6EFCE; width: 15px; height: 15px;">
               
            </td>
            <td>&nbsp; Completed  &nbsp;</td>

            <td style="background-color: #FFCC99; width: 15px; height: 15px;">
             
            </td>
            <td>&nbsp; In Progress  &nbsp;</td>

            <td style="background-color: #B4C6E7; width: 15px; height: 15px;">
                
            </td>
            <td>&nbsp; Planned  &nbsp;</td>
        </tr>
    </table>

</div>
        </td></tr>
</table>

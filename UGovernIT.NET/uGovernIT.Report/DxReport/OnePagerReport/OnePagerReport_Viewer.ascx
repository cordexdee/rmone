<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnePagerReport_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.OnePagerReport_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<script src="/Scripts/uGITCommon.js"></script>
<style>
    .savetodisk {
        background-position: -112px 0;
        background-image: url("/Content/ButtonImages/DXR.axd.png");
        background-repeat: no-repeat;
    }

    .savetoWindow {
        background-position: -112px 0;
        background-image: url("/Content/ButtonImages/DXR.axd.png");
        background-repeat: no-repeat;
    }

    .export-buttons {
        padding-left: 3px;
    }
    .hidefooter-container {
    display:none;
    }
</style>
<script type="text/javascript">
    function OpenSendEmailWindow(url) {
        //var url = re
        var requestUrl = hdnconfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Send Email - 1-Pager Project Report', '800px', '600px', 0, escape(requestUrl))
        return false;
    }

    function OpenSaveToDocument(url) {
        var docID = '<%=TicketId %>';
        var find = '-';
        var reg = new RegExp(find, 'g');
        docID = docID.replace(reg, '_');
        var selectedfolderGuid = '<%=FolderGuid%>';
        var pathValue = '<%=PathValue %>';
        var selectedFolder = '<%=SelectFolder%>';
        //var url = hdnconfiguration.Get("SaveToDocumentUrl");
        var requestUrl = hdnconfiguration.Get("RequestUrl");
        var param = "docID=" + docID + "&pathValue=" + pathValue + "&selectedfolderGuid=" + selectedfolderGuid + "&IsSelectFolder=true";
        UgitOpenPopupDialog(url, param, 'Save', '400px', '200px', 0, escape(requestUrl))
        return false;
    }

    function SaveToDocument() {
        loadingpanel.Show();
        cbMailsend.PerformCallback("SaveToDoc");
    }
    function SendMailClick() {
        loadingpanel.Show();
        cbMailsend.PerformCallback("SendMail");
    }

    function OnCallbackComplete(s, e) {
        loadingpanel.Hide();
        if (e.result != null && e.result.length > 0) {
            if (e.parameter.toString() == "SendMail") {
                OpenSendEmailWindow(e.result);
            }
            else if (e.parameter.toString() == "SaveToDoc") {
                OpenSaveToDocument(e.result);
            }
        }
    }
</script>


<dx:ASPxLoadingPanel ID="loadingpanel" runat="server" Modal="true" ClientInstanceName="loadingpanel" Theme="DevEx"></dx:ASPxLoadingPanel>
<dx:ASPxCallback ID="cbMailsend" runat="server" ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback">
    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
</dx:ASPxCallback>




<asp:Panel runat="server" ID="pnlReport" Width="100%">
    <dx:ASPxHiddenField ID="hdnconfiguration" runat="server" ClientInstanceName="hdnconfiguration"></dx:ASPxHiddenField>
    <asp:HiddenField runat="server" ID="hdnPDFGenerated" />
    <dx:ASPxSplitter ID="ASPxSplitter1" runat="server" Height="850px" Width="100%" ClientInstanceName="sampleSplitter">
        <%--  <ClientSideEvents PaneResized="OnSplitterPaneResized" />--%>
        <Panes>
            <dx:SplitterPane AutoWidth="False" Size="200" MaxSize="200" Name="ReportDocMapContainer" ShowCollapseBackwardButton="False">
                <ContentCollection>
                    <dx:SplitterContentControl ID="sccReportDocMap" runat="server">
                        <dx:ASPxPanel ID="pnlnavigationUrl" runat="server" Width="200" Height="850px" ClientInstanceName="pnlnavigationUrl">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ReportDocumentMap ID="ReportDocMap" runat="server" ClientInstanceName="ReportDocMap" ReportViewerID="RptVwrProjectReport"></dx:ReportDocumentMap>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
            <dx:SplitterPane Size="1300px"   Name="RptViewerContainer" AutoWidth="false">
                <ContentCollection>
                    <dx:SplitterContentControl ID="sccReportViewer" runat="Server">
                        <dx:ReportToolbar ID="RptToolBar" runat='server' Width="100%" ShowDefaultButtons='False'
                            ReportViewerID="RptVwrProjectReport" Theme="DevEx">
                            <Items>
                                <dx:ReportToolbarButton ItemKind='Search' />
                                <dx:ReportToolbarSeparator />
                                <dx:ReportToolbarButton ItemKind='PrintReport' />
                                <dx:ReportToolbarButton ItemKind='PrintPage' />
                                <dx:ReportToolbarSeparator />
                                <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' />
                                <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' />
                                <dx:ReportToolbarLabel ItemKind='PageLabel' />
                                <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'></dx:ReportToolbarComboBox>
                                <dx:ReportToolbarLabel ItemKind='OfLabel' />
                                <dx:ReportToolbarTextBox ItemKind='PageCount' />
                                <dx:ReportToolbarButton ItemKind='NextPage' />
                                <dx:ReportToolbarButton ItemKind='LastPage' />
                                <dx:ReportToolbarSeparator />
                                <dx:ReportToolbarButton ItemKind='SaveToDisk' />
                                <dx:ReportToolbarButton ItemKind='SaveToWindow' />
                                <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                                    <Elements>
                                        <dx:ListElement Value='pdf' />
                                        <dx:ListElement Value='xls' />
                                        <dx:ListElement Value='xlsx' />
                                        <dx:ListElement Value='png' />
                                    </Elements>
                                </dx:ReportToolbarComboBox>
                                <dx:ReportToolbarSeparator />
                                <dx:ReportToolbarTemplateItem>
                                    <Template>
                                        <dx:ASPxButton ID="SendEmail" RenderMode="Link" runat="server" CssClass="export-buttons" AutoPostBack="false"
                                            EnableTheming="false" UseSubmitBehavior="False">
                                            <Image Url="/Content/Images/MailTo.png" />
                                            <ClientSideEvents Click="function(s,e){ SendMailClick(); }" />
                                        </dx:ASPxButton>
                                    </Template>
                                </dx:ReportToolbarTemplateItem>
                                <dx:ReportToolbarTemplateItem Name="AttachToDocumentTemplate">
                                    <Template>
                                        <dx:ASPxButton ID="AttachToDocument" RenderMode="Link" runat="server" CssClass="export-buttons" AutoPostBack="false"
                                            EnableTheming="false" UseSubmitBehavior="False">
                                            <Image Url="/Content/Images/saveToFolder.png" />
                                            <ClientSideEvents Click="function(s,e){SaveToDocument();}" />
                                        </dx:ASPxButton>
                                    </Template>
                                </dx:ReportToolbarTemplateItem>

                            </Items>

                        </dx:ReportToolbar>
                        <dx:ReportViewer ID="RptVwrProjectReport" runat="server" Width="100%" Height="850px" AutoSize="true">
                            <BookmarkSelectionBorder BorderStyle="None" />
                        </dx:ReportViewer>
                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>
        </Panes>
    </dx:ASPxSplitter>
</asp:Panel>

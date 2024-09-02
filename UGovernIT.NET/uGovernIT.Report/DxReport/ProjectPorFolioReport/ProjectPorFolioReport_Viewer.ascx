<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectPorFolioReport_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.ProjectPorFolioReport.ProjectPorFolioReport_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>


<style>
    .savetodisk {
        background-position: -112px 0;
        background-image: url("/_layouts/15/images/ugovernit/ButtonImages/DXR.axd.png");
        background-repeat: no-repeat;
    }
     .savetoWindow {
        background-position: -112px 0;
        background-image: url("/_layouts/15/images/ugovernit/ButtonImages/DXR.axd.png");
        background-repeat: no-repeat;
    }
     .export-buttons {
        padding-left: 3px;
    }
</style>
<script type="text/javascript">
    function OpenSendEmailWindow(url) {
        //var url = re
        var requestUrl = hdnconfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Send Email - Project Status Report', '800px', '600px', 0, escape(requestUrl))
        return false;
    }

    function OpenSaveToDocument(url) {
        var docID = '<%=TicketId %>';
        var find = '-';
        var reg = new RegExp(find, 'g');
        docID = docID.replace(reg, '_');
        var selectedfolderGuid = '<%=FolderGuid%>';
        var pathValue = '<%=PathValue %>';
        <%--var selectedFolder = '<%=SelectFolder%>';--%>
        //var url = hdnconfiguration.Get("SaveToDocumentUrl");
        var requestUrl = hdnconfiguration.Get("RequestUrl");
        var param = "docID=" + docID + "&pathValue=" + pathValue + "&selectedfolderGuid=" + selectedfolderGuid + "&IsSelectFolder=true";
        UgitOpenPopupDialog(url, param, 'Save to Folder', '800px', '400px', 0, escape(requestUrl))
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
<dx:ASPxCallback ID="cbMailsend" runat="server"  ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback" >
    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
</dx:ASPxCallback>


<asp:Panel runat="server" ID="pnlReport" Width="100%">
    <dx:ASPxHiddenField ID="hdnconfiguration" runat="server" ClientInstanceName="hdnconfiguration"></dx:ASPxHiddenField>
    <asp:HiddenField runat="server" ID="hdnPDFGenerated" />
      <dx:ReportToolbar ID="RptToolBar" runat='server' Width="100%" ShowDefaultButtons='False'
                            ReportViewerID="RptVwrProjectReport" Theme="DevEx" >
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
                                <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px' >
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
                                        <dx:ASPxButton ID="SendEmail" RenderMode="Link"  runat="server" CssClass="export-buttons" AutoPostBack="false"
                                            EnableTheming="false"  UseSubmitBehavior="False" >
                                            <Image Url="~/_Layouts/15/Images/uGovernIT/MailTo.png"/>
                                            <ClientSideEvents  Click="function(s,e){ SendMailClick(); }" />
                                        </dx:ASPxButton>
                                    </Template>
                                </dx:ReportToolbarTemplateItem>
                                <dx:ReportToolbarTemplateItem Name="AttachToDocumentTemplate" >
                                    <Template>
                                        <dx:ASPxButton ID="AttachToDocument" RenderMode="Link" runat="server" CssClass="export-buttons" AutoPostBack="false"
                                            EnableTheming="false"  UseSubmitBehavior="False" >
                                            <Image Url="~/_Layouts/15/Images/uGovernIT/saveToFolder.png"/>
                                            <ClientSideEvents  Click="function(s,e){SaveToDocument();}" />
                                        </dx:ASPxButton>
                                    </Template>
                                </dx:ReportToolbarTemplateItem>
                               
                            </Items>
                            
                        </dx:ReportToolbar>
                        <dx:ReportViewer ID="RptVwrProjectReport" runat="server" Width="100%" Height="850px" AutoSize="false">
                            <BookmarkSelectionBorder BorderStyle="None" />
                        </dx:ReportViewer>
</asp:Panel>
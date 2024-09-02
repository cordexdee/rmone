<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeeklyTeamReport_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.WeeklyTeamReport_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
 
<script type="text/javascript">
    function OpenSendEmailWindow(url) {
        //var url = hdnConfiguration.Get("SendEmailUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Send Email - Weekly Team Performance Report', '800px', '600px', 0, escape(requestUrl))
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
        var param = "type=openticketReport&IsSelectFolder=true";
        UgitOpenPopupDialog(url, param, 'Save', '400px', '200px', 0, escape(requestUrl))
        return false;
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
<dx:ASPxLoadingPanel ID="loadingpanel" runat="server" Modal="true" ClientInstanceName="loadingpanel" ></dx:ASPxLoadingPanel>
<dx:ASPxCallback ID="cbMailsend" runat="server"  ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback" >
    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
</dx:ASPxCallback>
<dx:ASPxHiddenField ID="hdnConfiguration"  runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
<asp:Panel runat="server" ID="pnlReport" Width="100%">
    
    <asp:HiddenField runat="server" ID="hdnPDFGenerated" />
      <dx:ReportToolbar ID="RptToolBar" runat='server' Width="100%" ShowDefaultButtons='False'
                            ReportViewerID="RptWeeklyTeamPerformanceReport" Theme="DevEx" >
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
                                            EnableTheming="false"  UseSubmitBehavior="False" ToolTip="Send Report by Email" >
                                            <Image Url="/Content/Images/MailTo.png"/>
                                            <ClientSideEvents  Click="function(s,e){ SendMailClick(); }" />
                                        </dx:ASPxButton>
                                    </Template>
                                </dx:ReportToolbarTemplateItem>
                                <dx:ReportToolbarTemplateItem Name="AttachToDocumentTemplate" >
                                    <Template>
                                        <dx:ASPxButton ID="AttachToDocument" RenderMode="Link" runat="server" CssClass="export-buttons" AutoPostBack="false"
                                            EnableTheming="false"  UseSubmitBehavior="False" >
                                            <Image Url="/Content/Images/saveToFolder.png"/>
                                            <ClientSideEvents  Click="function(s,e){SaveToDocument();}" />
                                        </dx:ASPxButton>
                                    </Template>
                                </dx:ReportToolbarTemplateItem>
                               
                            </Items>
                            
                        </dx:ReportToolbar>
                        <dx:ReportViewer ID="RptWeeklyTeamPerformanceReport" runat="server" Width="100%" Height="850px" AutoSize="false">
                            <BookmarkSelectionBorder BorderStyle="None" />
                        </dx:ReportViewer>
</asp:Panel>
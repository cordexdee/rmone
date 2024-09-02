
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketSummary_Viewer.ascx.cs" Inherits="uGovernIT.DxReport.TicketSummary_Viewer" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style>
    .projectSummary-reportToolbar ul li.dxm-item div.dxm-content img {
        width: 18px;
    }
    .projectSum-emailIcon{
        background-size:18px;
    }
    .projectSummary-reportToolbar ul li.dxm-item.dxm-hovered{
        background:none !important;
        border-bottom:2px solid #4A6EE2 !important;
        border-left:none !important;
        border-top:none !important;
        border-right:none !important;
    }
</style>
<script type="text/javascript">
    function OpenSendEmailWindow(url) {
        //var url = hdnConfiguration.Get("SendEmailUrl");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Send Email - Open Tickets Summary Report', '800px', '600px', 0, escape(requestUrl))
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
<dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
<asp:Panel runat="server" ID="pnlReport" Width="100%"> 
    <dx:ReportToolbar ID="RptAccessReportToolbar" runat='server' ShowDefaultButtons='False' ReportViewerID="RptOpenTicketsReport" Width="100%" CssClass="projectSummary-reportToolbar">
        <Items>
            <dx:ReportToolbarButton ItemKind='Search' ImageUrl="/Content/Images/SearchNew.png"/>
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarButton ItemKind='PrintReport' ImageUrl="/Content/Images/print.png" />
            <dx:ReportToolbarButton ItemKind='PrintPage' ImageUrl="/Content/Images/print_current.png" />
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarButton Enabled='False' ItemKind='FirstPage' ImageUrl="/Content/Images/prev.png" ImageUrlDisabled="/Content/Images/d-prev.png" />
            <dx:ReportToolbarButton Enabled='False' ItemKind='PreviousPage' ImageUrl="/Content/Images/prev-page.png" ImageUrlDisabled="/Content/Images/d-prev-page.png" />
            <dx:ReportToolbarLabel ItemKind='PageLabel' />
            <dx:ReportToolbarComboBox ItemKind='PageNumber' Width='65px'></dx:ReportToolbarComboBox>
            <dx:ReportToolbarLabel ItemKind='OfLabel' />
            <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
            <dx:ReportToolbarButton ItemKind='NextPage' ImageUrl="/Content/Images/next-page.png" ImageUrlDisabled="/Content/Images/d-next-page.png"/>
            <dx:ReportToolbarButton ItemKind='LastPage' ImageUrl="/Content/Images/next.png" ImageUrlDisabled="/Content/Images/d-next.png" />
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarButton ItemKind='SaveToDisk' ImageUrl="/Content/Images/Exportsave.png" />
            <dx:ReportToolbarButton ItemKind='SaveToWindow' ImageUrl="/Content/Images/save-open-new-wind.png" />
            <dx:ReportToolbarComboBox ItemKind='SaveFormat' Width='70px'>
                <Elements>
                    <dx:ListElement Value='pdf' />
                    <dx:ListElement Value='xls' />
                    <dx:ListElement Value='xlsx' />
                    <dx:ListElement Value='csv' />
                    <dx:ListElement Value='rtf' />
                </Elements>
            </dx:ReportToolbarComboBox>
            <dx:ReportToolbarSeparator />
            <dx:ReportToolbarTemplateItem>
                <Template>
                    <dx:ASPxButton ID="SendEmail" runat="server" Border-BorderStyle="None" Border-BorderWidth="0"
                        AutoPostBack="false" BackColor="Transparent" BackgroundImage-Repeat="NoRepeat" CssClass="projectSum-emailIcon">
                        <backgroundimage imageurl="/Content/Images/email_send.png" horizontalposition="center" verticalposition="center"/>
                        <clientsideevents click="function(s,e){SendMailClick();}" />
                    </dx:ASPxButton>
                </Template>
            </dx:ReportToolbarTemplateItem>
             <dx:ReportToolbarTemplateItem Name="AttachToDocumentTemplate" >
                <Template>
                    <dx:ASPxButton ID="AttachToDocument" runat="server" CssClass="export-buttons" AutoPostBack="false"
                        EnableTheming="false"  UseSubmitBehavior="False" >
                        <Image Url="/Content/images/folder_download.png" Width="18px"/>
                        <ClientSideEvents  Click="function(s,e){SaveToDocument();}" />
                    </dx:ASPxButton>
                </Template>
            </dx:ReportToolbarTemplateItem>
        </Items>
        <Styles>
            <LabelStyle>
                <Margins MarginLeft='3px' MarginRight='3px' />
            </LabelStyle>
        </Styles>
    </dx:ReportToolbar>
    <dx:ReportViewer ID="RptOpenTicketsReport" runat="server" Width="100%" Height="800px" AutoSize="False" ></dx:ReportViewer>
</asp:Panel>
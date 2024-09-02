
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationAccessReportViewer.ascx.cs" Inherits="uGovernIT.Web.ApplicationAccessReportViewer" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Panel runat="server" ID="pnlReport" Width="100%"> 
    
<dx:ReportToolbar ID="RptAccessReportToolbar" runat='server' ShowDefaultButtons='False' ReportViewerID="RptAccessReport" Width="100%">
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
        <dx:ReportToolbarTextBox IsReadOnly='True' ItemKind='PageCount' />
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
               <%-- <dx:ListElement Value='rtf' />
                <dx:ListElement Value='mht' />
                <dx:ListElement Value='html' />
                <dx:ListElement Value='txt' />
                <dx:ListElement Value='csv' />
                <dx:ListElement Value='png' />--%>
            </Elements>
        </dx:ReportToolbarComboBox>
    </Items>
    <Styles>
        <LabelStyle>
            <Margins MarginLeft='3px' MarginRight='3px' />
        </LabelStyle>
    </Styles>
</dx:ReportToolbar>

<dx:ReportViewer ID="RptAccessReport" AutoSize="true" runat="server" Width="100%" Height="100%" PageByPage="false"  >

    
</dx:ReportViewer>
</asp:Panel>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceUsage_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.ResourceUsage.ResourceUsage_Viewer" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<asp:Panel runat="server" ID="pnlReport" Width="100%">
    <dx:ReportToolbar ID="RptAccessReportToolbar" runat='server' ShowDefaultButtons='False' ReportViewerID="RptResourceUsageReport" Width="100%">
        <Items>
            <dx:ReportToolbarButton ItemKind='Search' />
            <dx:ReportToolbarSeparator />            
            <dx:ReportToolbarButton ItemKind='PrintReport' Name="PrintReport" />

            <dx:ReportToolbarButton ItemKind='PrintPage' Name="PrintPage" />
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
            <dx:ReportToolbarButton ItemKind='SaveToDisk' Name="SaveToDisk" />

            <dx:ReportToolbarButton ItemKind='SaveToWindow' />
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
        </Items>
        <Styles>
            <LabelStyle>
                <Margins MarginLeft='3px' MarginRight='3px' />
            </LabelStyle>
        </Styles>
        
    </dx:ReportToolbar>
    <dx:ReportViewer ID="RptResourceUsageReport" runat="server" Width="100%" Height="800px" AutoSize="False"></dx:ReportViewer>
</asp:Panel>
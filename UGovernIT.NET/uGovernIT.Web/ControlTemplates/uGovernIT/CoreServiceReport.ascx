<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoreServiceReport.ascx.cs" Inherits="uGovernIT.Web.CoreServiceReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .excel-button {
        background-image: url('/Content/images/excel-icon.png');
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
        color: #000000;
        padding-bottom: 6px;
        padding-right: 8px;
        padding-top: 3px;
        width: 150px;
        text-align: right;
    }

    .panel-parameter {
        width: 400px;
        border: solid 1px #000000;
    }

    .table-header {
        background-color: #E8EDED;
    }

    a:hover {
        text-decoration: none !important;
    }

    .excelicon, .pdf-icon, .sendmail, .savetofolder {
        background-repeat: no-repeat;
        width: 22px;
        height: 22px;
    }

    .excelicon {
        background-image: url('/Content/images/excel-icon.png');
    }

    .pdf-icon {
        background-image: url('/Content/images/Pdf-icon.png');
    }

    .sendmail {
        background-image: url('/Content/images/MailTo.png');
    }

    .savetofolder {
        background-image: url('/Content/images/saveToFolder.png');
    }
</style>

<div id="exportOption" runat="server" style="background: #fff; /*padding: 2px 2px 0px;*/">
    <span class="fleft">
        <dx:ASPxButton ID="btnExcelExport" ClientInstanceName="btnExcelExport" runat="server" EnableTheming="false" UseSubmitBehavior="False"
            OnClick="btnExcelExport_Click" RenderMode="Link">
            <Image>
                <SpriteProperties CssClass="excelicon" />
            </Image>
            <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
        </dx:ASPxButton>
    </span>
    <span class="fleft">
        <dx:ASPxButton ID="btnPdfExport" ClientInstanceName="btnPdfExport" runat="server" CssClass="export-buttons" EnableTheming="false" UseSubmitBehavior="False" RenderMode="Link"
            OnClick="btnPdfExport_Click">
            <Image>
                <SpriteProperties CssClass="pdf-icon" />
            </Image>
            <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
        </dx:ASPxButton>
    </span>
</div>


<div id="content">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div style="width: 100%;">
            <dx:ASPxGridView ID="grdReport" AutoGenerateColumns="False" runat="server" CssClass="customgridview homeGrid"
                SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="99%" OnHtmlRowPrepared="grdReport_HtmlRowPrepared"
                OnDataBinding="grdReport_DataBinding">
                <Columns>

                    <%-- <dx:GridViewDataColumn FieldName="Type"  Caption="Type" GroupIndex="0">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>                            

                    <dx:GridViewDataColumn FieldName="CRMBusinessUnit"  Caption="CRMBusinessUnit">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="Estimator" Caption="Estimator">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="ApproxContractValue"  Caption="Value" CellStyle-HorizontalAlign="Center" Width="120px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>  
                            
                        <dx:GridViewDataTextColumn FieldName="TicketId"  Caption="# " CellStyle-HorizontalAlign="Center" Width="70px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>                          --%>
                </Columns>

                <Settings ShowFooter="true" ShowHeaderFilterButton="true" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <Row HorizontalAlign="Center" CssClass="estReport-dataRow"></Row>
                    <GroupRow CssClass="estReport-gridGroupRow"></GroupRow>
                    <Header Font-Bold="true" HorizontalAlign="Center" CssClass="reportDataGrid-header"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                    <Footer Font-Bold="true"></Footer>
                </Styles>
            </dx:ASPxGridView>
        </div>
    </div>
</div>

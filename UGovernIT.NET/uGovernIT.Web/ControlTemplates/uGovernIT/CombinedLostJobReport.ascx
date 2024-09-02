<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CombinedLostJobReport.ascx.cs" Inherits="uGovernIT.Web.CombinedLostJobReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %><%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
        background-image: url(/Content/15/images/ugovernit/excel-icon.png);
    }

    .pdf-icon {
        background-image: url(/Content/15/images/ugovernit/Pdf-icon.png);
    }

    .sendmail {
        background-image: url(/Content/15/Images/uGovernIT/MailTo.png);
    }

    .savetofolder {
        background-image: url(/Content/15/images/uGovernIT/saveToFolder.png);
    }
</style>

<div id="exportOption" runat="server" class="col-md-12 col-sm-12 col-xs-12">
    <div class="toolbarWrap">
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
    


</div>

<div id="content">
    <div class="col-md-12 col-sm-12 col-xs-12">
        <div style="width: 100%;>
            <dx:ASPxGridView ID="grdReport" AutoGenerateColumns="False" runat="server" CssClass="customgridview homeGrid" OnLoad="grdReport_Load"
                SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="99%" OnHtmlRowPrepared="grdReport_HtmlRowPrepared" OnDataBinding="grdReport_DataBinding">
                <Columns>

                    <dx:GridViewDataColumn FieldName="CRMBusinessUnit" Caption="Business Unit" GroupIndex="0">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataTextColumn FieldName="EstimateNo" Caption="Item ID" CellStyle-HorizontalAlign="Center" Width="100px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataColumn FieldName="Title" Caption="Name" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Width="250px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="TicketOwner" Caption="Owner" Width="150px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataDateColumn FieldName="AwardedLossDate" Caption="Loss Date" Width="90px" CellStyle-HorizontalAlign="Center">
                        <PropertiesDateEdit DisplayFormatString="{0:MMM-dd-yyyy}"></PropertiesDateEdit>
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataTextColumn FieldName="ApproxContractValue" Caption="Approx Contract Value" CellStyle-HorizontalAlign="Right" Width="100px">
                        <PropertiesTextEdit DisplayFormatString="{0:C0}" />
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataColumn FieldName="TicketComment" Caption="Loss Reason" CellStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataDateColumn FieldName="TicketCreationDate" Caption="Created On" Width="90px" CellStyle-HorizontalAlign="Center">
                        <PropertiesDateEdit DisplayFormatString="{0:MMM-dd-yyyy}"></PropertiesDateEdit>
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataColumn FieldName="CRMProjectStatus" Caption="Status" CellStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="Estimator" Caption="Estimator" CellStyle-HorizontalAlign="Left"  GroupIndex="1">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                </Columns>
                        
                <GroupSummary>
                    <dx:ASPxSummaryItem FieldName="ApproxContractValue" SummaryType="Sum" ShowInGroupFooterColumn="ApproxContractValue" DisplayFormat="<b>{0:C0}<b/>"  />                           
                </GroupSummary>
                <Settings ShowFooter="true" ShowHeaderFilterButton="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <Row HorizontalAlign="Center" CssClass="reportDataGrid_row"></Row>
                    <GroupRow Font-Bold="true"></GroupRow>
                    <Header Font-Bold="true" HorizontalAlign="Center" CssClass="reportDataGrid-header"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                    <Footer Font-Bold="true" HorizontalAlign="Center"></Footer>
                </Styles>
            </dx:ASPxGridView>
        </div>
    </div>
</div>
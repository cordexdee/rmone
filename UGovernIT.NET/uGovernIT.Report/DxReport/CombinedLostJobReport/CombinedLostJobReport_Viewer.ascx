<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CombinedLostJobReport_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.CombinedLostJobReport_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"  Namespace="DevExpress.XtraCharts" TagPrefix="dxcharts" %>

  <style>
      .excel-button {
          background-image: url('/Content/images/excel-icon.png');
      }

    .export-buttons {
        padding-left: 3px;
    }

    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;*/
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

    /*.excelicon {
        background-image: url('/Content/images/excel-icon.png');
    }

    .pdf-icon {
        background-image: url(/Content/images/Pdf-icon.png);
    }

    .sendmail {
        background-image: url(/Content/images/MailTo.png);
    }

    .savetofolder {
        background-image: url(/Content/images/saveToFolder.png);
    }*/
</style>

<script>
    function expandAllTask() {
        grdReport.ExpandAll();
    }
    function collapseAllTask() {
        grdReport.CollapseAll();
    }
</script>

<div id="exportOption" runat="server" class="col-md-12 col-sm-12 col-xs-12">
    <span class="fleft expandCollapseIcons">
        <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
        <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
    </span>
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
        <div style="width: 100%;">
            <dx:ASPxGridView ID="grdReport" ClientInstanceName="grdReport" AutoGenerateColumns="False" runat="server" CssClass="customgridview homeGrid" OnLoad="grdReport_Load"
                SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="100%" OnHtmlRowPrepared="grdReport_HtmlRowPrepared" OnDataBinding="grdReport_DataBinding">
                <Columns>
                    <dx:GridViewDataColumn FieldName="CRMBusinessUnitChoice" Caption="Division" GroupIndex="0">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

<%--                    <dx:GridViewDataTextColumn FieldName="ProjectId" Caption="Item ID" CellStyle-HorizontalAlign="Left" Width="150px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>--%>
                    <dx:GridViewDataTextColumn FieldName="ERPJobID" Caption="CMIC #" CellStyle-HorizontalAlign="Left" Width="150px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataColumn FieldName="Title" Caption="Name" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Width="300px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="CRMownerUser" Caption="Owner" CellStyle-HorizontalAlign="Left" Width="150px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataDateColumn FieldName="AwardedorLossDate" Caption="Loss Date" Width="90px" CellStyle-HorizontalAlign="Center">
                        <PropertiesDateEdit DisplayFormatString="{0:MMM-dd-yyyy}"></PropertiesDateEdit>
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataTextColumn FieldName="ApproxContractValue" Caption="Approx Contract Value" CellStyle-HorizontalAlign="Right" Width="100px" PropertiesTextEdit-DisplayFormatString="{0:c}" >
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataColumn FieldName="Reason" Caption="Loss Reason" CellStyle-HorizontalAlign="Left"  Width="250px">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataDateColumn FieldName="CreationDate" Caption="Created On" Width="90px" CellStyle-HorizontalAlign="Center">
                        <PropertiesDateEdit DisplayFormatString="{0:MMM-dd-yyyy}"></PropertiesDateEdit>
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataColumn FieldName="CRMProjectStatusChoice" Caption="Status" CellStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="EstimatorUser" Caption="Estimator" CellStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="StudioChoice" Caption="Studio" CellStyle-HorizontalAlign="Left"  GroupIndex="1">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                </Columns>
                <GroupSummary>
                    <dx:ASPxSummaryItem FieldName="ApproxContractValue" SummaryType="Sum" ShowInGroupFooterColumn="ApproxContractValue" DisplayFormat="<b>{0:C0}<b/>"  />                           
                </GroupSummary>
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="ApproxContractValue" SummaryType="Sum" ShowInGroupFooterColumn="ApproxContractValue" DisplayFormat="<b>{0:C0}<b/>"  />                           
                </TotalSummary>
                <Settings ShowFooter="true" ShowHeaderFilterButton="true" ShowGroupFooter="VisibleAlways" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <Row HorizontalAlign="Center" CssClass="estReport-dataRow"></Row>
                    <GroupRow  CssClass="estReport-gridGroupRow"></GroupRow>
                    <Header Font-Bold="true" HorizontalAlign="Center" CssClass="estReport-gridHeaderRow"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                    <GroupFooter CssClass="report-footerGroupRow"></GroupFooter>
                    <Footer Font-Size="11"></Footer>
                </Styles>
            </dx:ASPxGridView>
        </div>
    </div>
</div>
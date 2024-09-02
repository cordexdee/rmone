<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimilarityScoreDetail.ascx.cs" Inherits="uGovernIT.Web.SimilarityScoreDetail" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        border-left: 1px solid #A5A5A5;
        text-align: right;
        width: 190px;
        vertical-align: top;
        padding: 3px 6px 4px;
    }

    .ms-standardheader {
        text-align: right !important;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .header {
        padding: 5px 15px 5px;
        border-top: 1px solid #A5A5A5;
        border-left: 1px solid #A5A5A5;
        font-weight: bold;
        text-align: center;
    }

    .footer {
        padding: 3px 6px 4px;
        border: 1px solid #A5A5A5;
        font-weight: bold;
        text-align: center;
    }

    .clssimilarityscore {
        /*table-layout:fixed;*/
        white-space: nowrap;
        width: 100%
    }

    .clssimilarityscore td {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
    }
    .similarityScoreGrid {
        padding: 0px;
        margin-top: 15px;
        margin-bottom: 30px;
    }
</style>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap similarityScoreGrid">
    <div class="row">
        <dx:ASPxGridView ID="dxgridProjectSimilarity" ClientInstanceName="dxgridProjectSimilarity" runat="server"
            Width="100%" AutoGenerateColumns="false" CssClass="customgridview homeGrid mt-3" OnCustomSummaryCalculate="dxgridProjectSimilarity_CustomSummaryCalculate" >
            <Columns>
                <dx:GridViewDataTextColumn FieldName="FieldName" Caption="Factor" SortOrder="Ascending" Width="90px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="pFieldValue" CellStyle-HorizontalAlign="left" Width="80px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="sFieldValue" CellStyle-HorizontalAlign="left" Width="80px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Score" Caption="Score" CellStyle-HorizontalAlign="Center" Width="30px">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataTextColumn>
            </Columns>
            <TotalSummary>
                <dx:ASPxSummaryItem SummaryType="Custom" FieldName="FieldName" />
                <dx:ASPxSummaryItem SummaryType="Custom" FieldName="Score"/>
            </TotalSummary>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                <Footer HorizontalAlign="Center"></Footer>
            </Styles>
            <Settings ShowHeaderFilterButton="true" ShowFooter="True" />
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        </dx:ASPxGridView>
    </div>
</div>
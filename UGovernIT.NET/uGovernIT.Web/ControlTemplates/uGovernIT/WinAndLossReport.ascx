<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WinAndLossReport.ascx.cs" Inherits="uGovernIT.Web.WinAndLossReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<div id="exportOption" runat="server" style="background: #fff;">
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

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grdWinLossReport.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grdWinLossReport.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div id="content">
    <table width="100%" align="left">
        <tr>
            <td align="left">
                <div style="width: 100%; float: right;">

                    <dx:ASPxGridView ID="grdWinLossReport" AutoGenerateColumns="False" runat="server" CssClass="customgridview homeGrid"
                        SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="99%" OnDataBinding="grdWinLossReport_DataBinding">
                         <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                        <Columns>

                            <dx:GridViewDataColumn FieldName="TicketId" Caption="Item Id" Width="120px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>


                            <dx:GridViewDataColumn FieldName="Title" Caption="Name" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Width="250px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                            <dx:GridViewDataColumn FieldName="CRMOpportunityStatusChoice" Caption="Status" Width="150px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                            <dx:GridViewDataDateColumn FieldName="AwardedorLossDate" Caption="Award/Loss Date" Width="90px" CellStyle-HorizontalAlign="Center">
                                <PropertiesDateEdit DisplayFormatString="{0:MMM-dd-yyyy}"></PropertiesDateEdit>
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataDateColumn>

                            <dx:GridViewDataTextColumn FieldName="ApproxContractValue" Caption="Projected Volume" CellStyle-HorizontalAlign="Right" Width="100px">
                                <PropertiesTextEdit DisplayFormatString="{0:C0}" />
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataTextColumn>

                            <dx:GridViewDataColumn FieldName="Reason" Caption="Reason" CellStyle-HorizontalAlign="Left">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>


                            <dx:GridViewDataColumn FieldName="MarketSector" Caption="Market Sectors" CellStyle-HorizontalAlign="Left" Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                               <dx:GridViewDataColumn FieldName="CMFirm" Caption="CM Firm" CellStyle-HorizontalAlign="Left"  Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>


                            <dx:GridViewDataColumn FieldName="CMContact" Caption="CM Contact" CellStyle-HorizontalAlign="Left"  Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                               <dx:GridViewDataColumn FieldName="ArchitectFirm" Caption="Architect Firm" CellStyle-HorizontalAlign="Left" Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                               <dx:GridViewDataColumn FieldName="ArchitectContact" Caption="Architect Contact" CellStyle-HorizontalAlign="Left"  Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                               <dx:GridViewDataColumn FieldName="DeveloperEndUserFirm" Caption="Developer/End User Firm" CellStyle-HorizontalAlign="Left" Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                               <dx:GridViewDataColumn FieldName="DeveloperEndUserContact" Caption="Developer/End User Contact" CellStyle-HorizontalAlign="Left" Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                               <dx:GridViewDataColumn FieldName="BrokerFirm" Caption="Broker Firm" CellStyle-HorizontalAlign="Left" Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                              

                               <dx:GridViewDataColumn FieldName="BrokerContact" Caption="Broker Contact" CellStyle-HorizontalAlign="Left" Width="100px">
                                <Settings HeaderFilterMode="CheckedList" />
                            </dx:GridViewDataColumn>

                        </Columns>
                         <settingscommandbutton >
                            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn" ></ShowAdaptiveDetailButton>
                            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></HideAdaptiveDetailButton>
                        </settingscommandbutton>
                      <%--  <GroupSummary>
                            <dx:ASPxSummaryItem FieldName="ApproxContractValue" SummaryType="Sum" ShowInGroupFooterColumn="ApproxContractValue" DisplayFormat="<b>{0:C0}<b/>"  />                           
                        </GroupSummary>
                        <Settings ShowFooter="true" ShowHeaderFilterButton="true" ShowGroupFooter="VisibleIfExpanded" />
                        <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />--%>
                        <SettingsPopup>
                            <HeaderFilter Height="200" />
                        </SettingsPopup>
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                        <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                            <Row HorizontalAlign="left" CssClass="CRMstatusGrid_row"></Row>
                            <GroupRow Font-Bold="true"></GroupRow>
                            <Header Font-Bold="true" HorizontalAlign="left" CssClass="CRMstatusGrid_headerRow"></Header>
                            <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                            <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                            <Footer Font-Bold="true" HorizontalAlign="Center"></Footer>
                        </Styles>

                    </dx:ASPxGridView>

                </div>
            </td>
        </tr>
    </table>
</div>
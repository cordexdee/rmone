
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceCostView.ascx.cs" Inherits="uGovernIT.Web.ResourceCostView" %>
 <%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<style type="text/css">
    .labour-grid-container{
        padding: 1px 0px 2px 0px;
        width: 100%;
        background: #F8F8F8 !important;
    }
    .grid-style{
        background: #F8F8F8 !important;
    }
    .grid-custom-header{
        border: none !important;
        font-family: "Verdana",verdana,arial,helvetica,sans-serif !important;
        font-size: 0.85em !important;
        color: black !important;
        font-weight: bold !important;
        text-align: left;
        cursor: default !important;
        background: #F8F8F8 !important;
    }
    .clsNthrowcolor {
        border-bottom:1px solid !important;
        border-bottom-color: black !important;
        border-bottom-style: solid !important;
    }

    .context-popup-grid .dxpc-content {
        padding: 0 0 0 0 !important;
    }
</style>--%>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OnTotalHoursClick(obj, resource) {
        if (resource != null && resource != undefined) {
            var resourceInfo = resource.split(";#");
            resourceHoursGridContainer.SetHeaderText("Reported Hours For " + resourceInfo[1]);
            resourceHoursGridContainer.SetPopupElementID(obj.id);
            resourceHoursGridContainer.PerformCallback(resource);
            resourceHoursGridContainer.Show();
        }
    }
</script>
<div class="labour-grid-container">
    <dx:ASPxGridView ID="labourChargesGrid" runat="server"  Settings-GridLines="None" 
        AutoGenerateColumns="false" Width="100%" CssClass="customgridview homeGrid" OnHtmlDataCellPrepared="labourChargesGrid_HtmlDataCellPrepared">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ResourceNameUser" Width="20%" Caption="Resource" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="HourlyRate" Width="16%" Caption="Hourly Rate" PropertiesTextEdit-DisplayFormatString="{0:c}"
                 CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataDateColumn FieldName="StartDate" Width="16%" Caption="Start Date" PropertiesDateEdit-DisplayFormatString="MMM-dd-yyyy"
                CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesDateEdit-DisplayFormatInEditMode="true">
            </dx:GridViewDataDateColumn> 
            <dx:GridViewDataDateColumn FieldName="EndDate" Width="16%" Caption="End Date" 
                CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="HoursTaken" Width="16%" Caption="Total Hours"
                CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <DataItemTemplate>
                    <a id="lnkHoursTaken" runat="server" href="javascript:void(0);" ></a>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="LabourCharges" Width="16%" Caption="Total Cost" PropertiesTextEdit-DisplayFormatString="{0:c}"
                CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
            </dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowHeaderFilterButton="false" EnableFilterControlPopupMenuScrolling="false" ShowFooter="true" />
        <SettingsPopup>
            <HeaderFilter Height="180" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="false" EnableRowHotTrack="false"/>
        <SettingsPager Mode="ShowAllRecords"/>
        <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
        <SettingsText EmptyDataRow="No Timesheet entry is found." />
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="EndDate" SummaryType="Count" DisplayFormat="Total: " />
            <dx:ASPxSummaryItem FieldName="HoursTaken" SummaryType="Sum" DisplayFormat="{0}" />
            <dx:ASPxSummaryItem FieldName="LabourCharges" SummaryType="Sum" DisplayFormat="{0:c}" />
        </TotalSummary>        
        <Styles>
            <Header CssClass="homeGrid_headerColumns"></Header>
            <AlternatingRow Enabled="True" BackColor="#f0f0f0"></AlternatingRow>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Footer Font-Bold="true" HorizontalAlign="Center" BackColor="#F8F8F8"></Footer>
        </Styles>
    </dx:ASPxGridView>
</div>

<%--Popup to show hours taken by a resource on particular dates--%>
<dx:ASPxPopupControl ID="resourceHoursGridContainer" runat="server" AllowDragging="false" ClientInstanceName="resourceHoursGridContainer" CloseAction="OuterMouseClick" ShowCloseButton="true"
    ShowFooter="false" ShowHeader="true" PopupVerticalAlign="Middle" PopupHorizontalAlign="WindowCenter" EnableViewState="false" EnableHierarchyRecreation="True"
    OnWindowCallback="resourceHoursGridContainer_WindowCallback" CssClass="context-popup-grid" Width="400" Height="350">
    <HeaderStyle Font-Bold="true" ForeColor="Black" />
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <dx:ASPxGridView ID="resourceHoursGrid" runat="server" OnDataBinding="resourceHoursGrid_DataBinding" AutoGenerateColumns="false"
                Width="100%" ClientInstanceName="resourceHoursGrid" EnableViewState="false">
                <Columns>
                    <dx:GridViewDataDateColumn FieldName="WorkDate" Caption="Date" Width="20%" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings AllowHeaderFilter="false" AllowAutoFilter="false" AllowSort="false" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="HoursTaken" Width="16%" Caption="Hours" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowHeaderFilterButton="false" ShowFooter="true" EnableFilterControlPopupMenuScrolling="false" VerticalScrollBarMode="Visible"
                    VerticalScrollableHeight="350" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsBehavior AllowSort="false" EnableRowHotTrack="false" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
                <TotalSummary>
                    <dx:ASPxSummaryItem FieldName="WorkDate" SummaryType="Count" DisplayFormat="Total: " />
                    <dx:ASPxSummaryItem FieldName="HoursTaken" SummaryType="Sum" DisplayFormat="{0}" />
                </TotalSummary>   
                <Styles>
                    <AlternatingRow Enabled="True"></AlternatingRow>
                    <Footer Font-Bold="true" HorizontalAlign="Center"></Footer>
                </Styles>
            </dx:ASPxGridView>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceTimeSheetSignOffHistory.ascx.cs" Inherits="uGovernIT.Web.ResourceTimeSheetSignOffHistory" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .gridStyle{
        padding: 7px 0 0 10px;
    }
    .customrowheight {
        height: 35px;
    }
</style>
<div class="fullwidth gridStyle">
    <dx:ASPxGridView ID="historyGrid" runat="server" AutoGenerateColumns="false" Width="100%" CssClass="customgridview">
        <Columns>
            <dx:GridViewDataDateColumn FieldName="Date" Width="20%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
            </dx:GridViewDataDateColumn>
            <dx:GridViewDataTextColumn FieldName="Action By" Width="20%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True"/>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Action" Width="20%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Comment" Width="40%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                <Settings AllowHeaderFilter="False" AllowAutoFilter="false" AllowSort="False" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowHeaderFilterButton="true" EnableFilterControlPopupMenuScrolling="true" />
        <SettingsPopup>
            <HeaderFilter Height="180" />
        </SettingsPopup>
        <SettingsBehavior AllowSort="true" EnableRowHotTrack="false"/>
        <SettingsPager Mode="ShowAllRecords"/>
        <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
        <Styles>
            <AlternatingRow Enabled="True"></AlternatingRow>
            <Row CssClass="customrowheight"></Row>
        </Styles>
    </dx:ASPxGridView>
</div>
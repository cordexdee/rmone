<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="trackprojectstagehistory.ascx.cs" Inherits="uGovernIT.Web.trackprojectstagehistory" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function downloadExcel(obj) {
        var exportUrl = $("#<%= exportURL.ClientID %>").val();
        exportUrl += "&initiateExport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }
</script>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .gvtrack_headerfilter{padding:5px !important;}
   .gvtrack_headerfilter .dxgvHFDRP{margin-bottom:3px !important;}
</style>

<div style="float: left; padding: 5px">
    <dx:ASPxCheckBox ID="showSingleCheckBox" runat="server" Text="Only Show One Change Per Day" Checked="true" AutoPostBack="true" OnCheckedChanged="showSingleCheckBox_CheckedChanged"></dx:ASPxCheckBox>
</div>

<div style="float: right; padding: 5px">
    <asp:HiddenField ID="exportURL" runat="server" />
    <img src="/Content/images/excel-icon.png" alt="Excel" title="Excel" style="cursor: pointer;" onclick="downloadExcel(this);" />
</div>

<div>
    <dx:ASPxGridView ID="gvTrackProjectStageReport" runat="server" AutoGenerateColumns="false"
        Width="99%" ClientInstanceName="gvTrackProjectStageReport">
        <Columns>
        </Columns>
        <SettingsPopup>
            <HeaderFilter Width="250" MinHeight="250" />
        </SettingsPopup>
         <StylesPopup>
            <HeaderFilter  Content-CssClass="gvtrack_headerfilter"></HeaderFilter>
         </StylesPopup>
        <Settings VerticalScrollBarMode="Auto" VerticalScrollableHeight="400" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>  
        <SettingsCookies Enabled="true" StoreGroupingAndSorting="true" StoreColumnsWidth="false" StoreFiltering="true" Version="0" CookiesID="grid_Cookies" />
    </dx:ASPxGridView>
</div>


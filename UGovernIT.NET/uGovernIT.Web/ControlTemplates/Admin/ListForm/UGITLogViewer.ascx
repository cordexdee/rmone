

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UGITLogViewer.ascx.cs" Inherits="uGovernIT.Web.UGITLogViewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="padding-bottom:20px;">
    <ugit:ASPxGridView ID="grid" runat="server" ViewStateMode="Disabled" EnableViewState="false" AutoGenerateColumns="False"
        OnDataBinding="grid_DataBinding" CssClass="customgridview homeGrid" OnHtmlRowPrepared="grid_HtmlRowPrepared"
        OnBeforeHeaderFilterFillItems="grid_BeforeHeaderFilterFillItems" OnHeaderFilterFillItems ="grid_HeaderFilterFillItems" 
        OnCustomColumnDisplayText="grid_CustomColumnDisplayText" ClientInstanceName="gridClientInstance"
        Width="100%" KeyFieldName="ID">
        <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
        <Columns></Columns>
        <settingscommandbutton>
            <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
            <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
        </settingscommandbutton>
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
            <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
        </Styles>
        <SettingsPopup>
                <HeaderFilter Height="200" />
        </SettingsPopup>                          
        <SettingsPager Position="TopAndBottom">
            <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
        </SettingsPager>
        <SettingsBehavior AllowSort ="true" />
        <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" ShowDetailButtons="true" />
         <Templates>
             <DetailRow>
                 <b>Message: </b><%# Eval("Description") %>
             </DetailRow>
         </Templates>
       
</ugit:ASPxGridView>

</div>
 
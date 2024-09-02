<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GanttView_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.GanttView_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"  Namespace="DevExpress.XtraCharts" TagPrefix="dxcharts" %>



  <style type="text/css">
       

        .ms-menutoolbar {
            display: none !important;
        }
        .ms-WPBorder, .ms-WPBorderBorderOnly
        {
             border-style: none !important;
             border-width: 0px !important;
        }
    </style>
  <script type="text/javascript">
      var isZoomIn;
      function GZoomIn() {
          if (typeof (gridClientInstance) != 'undefined' && !gridClientInstance.InCallback()) {
              gridClientInstance.PerformCallback('+');
          }
      }

      function GZoomOut() {
          if (typeof (gridClientInstance) != 'undefined' && !gridClientInstance.InCallback()) {
              gridClientInstance.PerformCallback('-');
          }
      }

      function ClosePopUp() {
          window.frameElement.commitPopup();
          return false;
      }



</script>

<asp:Panel ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

    <div style="padding-top: 2px; float: left">
        <table>
            <tr>
                <td>
                    <dx:ASPxImage runat="server" ID="imgExpand" Text="Expand All Rows" ImageUrl="~/Content/images/expand-all.png"
                        AutoPostBack="false">
                        <ClientSideEvents Click="function() { gridClientInstance.ExpandAll() }" />
                    </dx:ASPxImage>
                </td>
                <td>
                    <dx:ASPxImage runat="server" ID="imgCollapse" Text="Collapse All Rows" ImageUrl="~/Content/images/collapse-all.png"
                        AutoPostBack="false">
                        <ClientSideEvents Click="function() { gridClientInstance.CollapseAll() }" />
                    </dx:ASPxImage>
                </td>
            </tr>
        </table>
        <dx:ASPxHiddenField ID="hdnZoomLevel" runat="server" ClientInstanceName="hdnZoomLevel"></dx:ASPxHiddenField>
    </div>


    <div id="export-options" style="background: #fff; padding: 2px 2px 5px 5px; float: right;">
      <div class="toolbarWrap">
        <%--<span class="fleft">
            <img id="imgAdvanceMode" runat="server" style="cursor: pointer; margin-top: -3px; width: 20px;" onclick="setFilterMode(this)" />
        </span>--%>

        <span class="fleft" style="padding-left: 4px;">
            <img src="/Content/Images/zoom_minus.png" alt="Zoom Out" id="Img2" style="float: right; width: 20px; height: 20px; padding-right: 3px;" onclick="GZoomOut()" title="Zoom Out" />
        </span>
        <span class="fleft" style="padding-left: 4px;">
            <img src="/Content/Images/zoom_plus.png" alt="Zoom In" id="Img1" style="float: right; width: 20px; height: 20px; padding-right: 3px;" onclick="GZoomIn()" title="Zoom In" />
        </span>

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
    <div style="padding-top: 5px">

        <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
            OnDataBinding="grid_DataBinding"
            OnHtmlRowPrepared="grid_HtmlRowPrepared"
            OnHeaderFilterFillItems="grid_HeaderFilterFillItems"
            OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
            ClientInstanceName="gridClientInstance"
            EnableViewState="false"
            EnableRowsCache="false"
             Width="100%" KeyFieldName="TicketId">

            <Columns></Columns>

            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>

            <SettingsPager Position="TopAndBottom">
                <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
            </SettingsPager>
            <SettingsCookies Enabled="false" />
            <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" ColumnResizeMode="Disabled" AllowSort="true" />
            <Settings HorizontalScrollBarMode="Auto" />
           <Styles>
             <GroupRow Font-Bold="true">
           </GroupRow>
</Styles>
        </dx:ASPxGridView>

    </div>
    <div style="float: right; padding-top: 5px;">
        <dx:ASPxButton ID ="btnClose" runat="server" Text="Close" AutoPostBack="false" OnClick="btnClose_Click">

        </dx:ASPxButton>

    </div>


</asp:Panel>
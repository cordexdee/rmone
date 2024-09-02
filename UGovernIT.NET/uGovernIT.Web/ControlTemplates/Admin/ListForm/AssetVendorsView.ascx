<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetVendorsView.ascx.cs" Inherits="uGovernIT.Web.AssetVendorsView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        dx_gridView.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        dx_gridView.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
    function expandAllTask() {
        dx_gridView.ExpandAll();
    }
    function collapseAllTask() {
        dx_gridView.CollapseAll();
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <div class="headerContent-right">
            <div class="headerItem-addItemBtn">
                <a id="aAddItem_Top" runat="server" href="" style="padding-left: 15px" class="primary-btn-link">
                    <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
         
            <div class="headerItem-showChkBox">
                <dx:ASPxButton ID="btnApplyChanges" CssClass="primary-blueBtn" Text="Apply Changes" runat="server" OnClick="btnRefreshCache_Click" CausesValidation="false">
                    <ClientSideEvents click="function(s, e){loadingPanel.Show();}"/>
                </dx:ASPxButton>
             
            </div>

        </div>
    </div>
    <div class="row" style="margin-top:10px;">
        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
        <div class="col-md-2 col-sm-2 col-xs-2 noPadding" style="float: right;">
            <div class="headerItem-showChkBox crm-checkWrap" style="margin-top: 0px;">
                <asp:CheckBox ID="dxShowDeleted" OnCheckedChanged="dxShowDeleted_CheckedChanged" Text="Show Deleted" runat="server" TextAlign="right" AutoPostBack="true" Style="float: right;" />
            </div>
        </div>
    </div>
    <div class="row">

        <div class="row" style="margin-top:15px;">
            <ugit:ASPxGridView ID="dx_gridView" runat="server" SettingsPager-Mode="ShowAllRecords" OnHtmlDataCellPrepared="dx_gridView_HtmlDataCellPrepared"
                Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid" ClientInstanceName="dx_gridView">
                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="VendorType" Caption="Type" GroupIndex="1"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Name="aEdit">
                        <DataItemTemplate>
                            <a id="editLink" runat="server" href="">
                                <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                            </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Vendor Name" FieldName="VendorName" SortOrder="Ascending">
                        <DataItemTemplate>
                            <a id="editLink" runat="server" href=""></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Location" FieldName="VendorLocation" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Phone" FieldName="VendorPhone" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Email" FieldName="VendorEmail" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Address" FieldName="VendorAddress" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Contact Name" FieldName="ContactName" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                </Columns>
                <SettingsCommandButton>
                    <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </SettingsCommandButton>
                <Settings ShowHeaderFilterButton="true" />
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass=" homeGrid_headerColumn" Font-Bold="true"></Header>
                    <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                </Styles>
                <FormatConditions>
                    <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]= 1" Format="Custom" RowStyle-CssClass="formatcolor" ApplyToRow="true" />
                </FormatConditions>
            </ugit:ASPxGridView>
        </div>

        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            try {
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            } catch (ex) { }
        </script>
    </div>
    <div class="row bottom-addBtn">
        <div class="headerItem-addItemBtn">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>

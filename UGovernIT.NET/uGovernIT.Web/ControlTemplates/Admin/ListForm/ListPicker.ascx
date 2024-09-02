<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListPicker.ascx.cs" Inherits="uGovernIT.Web.ListPicker" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var isMultiSelect = '<%=IsMultiSelect%>';
    function OnModuleChange() {
        grid.SetVisible(false);
        LoadingPanel.Show();
    }

    function OnRowClickEvent(s, e) {
        if (s.IsRowSelectedOnPage(e.visibleIndex)) {
            s.UnselectRows(e.visibleIndex);
        }
    }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .chkIncClosedTkts {
        float: right;
        /*margin:24px 124px;*/
    }
</style>


<div class="ms-formtable col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-12 noPadding">
            <div class="relatedTicket_dropDown_select">
                <asp:Label ID="lblModule" runat="server" class="dropDown-fieldLabel">Select Module</asp:Label>
                <asp:DropDownList ID="ddlModuleService" runat="server" CssClass="aspxDropDownList" AutoPostBack="true" onchange="OnModuleChange();" OnSelectedIndexChanged="ddlModuleService_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12 noPadding relatedTicket_msg">
            <asp:Label ID="lblNotificationText" runat="server" Text="Select item(s)" Font-Size="larger" Font-Bold="true" ForeColor="#000066"></asp:Label>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-12 noPadding" style="padding-right: 5px;">
            <asp:CheckBox ID="chkIncClosedTkts" runat="server" CssClass="chkIncClosedTkts existingTicket_checkbox crm-checkWrap" TextAlign="right" Text="Include Closed Items" AutoPostBack="true" OnCheckedChanged="ddlModuleService_SelectedIndexChanged" />
        </div>
    </div>
    <div class="row">
        <div style="text-align: center; width: auto">
            <asp:Label ID="lblCheckMsg" runat="server" Visible="false" Font-Size="larger" Font-Bold="true" ForeColor="#000066"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div  style="text-align: right; padding: 5px 5px 5px 0px;">
            <div>
                    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                        function UpdateGridHeight() {
                            grid.SetHeight(0);
                            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                            if (document.body.scrollHeight > containerHeight)
                                containerHeight = document.body.scrollHeight;
                            grid.SetHeight(containerHeight);
                        }
                        window.addEventListener('resize', function (evt) {
                            if (!ASPxClientUtils.androidPlatform)
                                return;
                            var activeElement = document.activeElement;
                            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                        });
                </script>
                <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" 
                    OnDataBinding="grid_DataBinding" OnCustomColumnDisplayText="grid_CustomColumnDisplayText" 
                    OnHtmlRowPrepared="grid_HtmlRowPrepared" OnHeaderFilterFillItems="grid_HeaderFilterFillItems"
                    OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" CssClass="customgridview homeGrid"
                    ClientInstanceName="grid"  Width="100%" KeyFieldName="TicketId">
                    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                    <Columns></Columns>                        
                    <settingscommandbutton>
                        <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                    </settingscommandbutton>
                    <SettingsBehavior EnableRowHotTrack="false"  AllowSelectByRowClick="false" AllowSelectSingleRowOnly="false" />                        
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <SettingsPager Position="TopAndBottom">
                        <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                    </SettingsPager>
                    <Styles>
                        <Row HorizontalAlign="Center" CssClass="CRMstatusGrid_row"></Row>
                        <Header Font-Bold="true" HorizontalAlign="Center" CssClass="homeGrid_headerColumn"></Header>
                        <PagerTopPanel CssClass="gridPager"></PagerTopPanel>
                        <PagerBottomPanel CssClass="gridPager"></PagerBottomPanel>
                    </Styles>
                    <ClientSideEvents RowClick="OnRowClickEvent" SelectionChanged="function(s, e) {
                        if (e.isSelected) {
                            var key = s.GetRowKey(e.visibleIndex);
                            if($('.BtnSaveLink').length > 0){  
                                    if(isMultiSelect == 'False')
                                {
                                    LoadingPanel.SetText('Saving ...');
                                    LoadingPanel.Show();      
                                var val = $('.BtnSaveLink').attr('id').replace(/_/g,'$');
                                javascript:__doPostBack(val,'');  
                                }
                            }
                        }                             
                    }" />
                </ugit:ASPxGridView>
                <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                    ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                        UpdateGridHeight();
                    });
                    ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                        UpdateGridHeight();
                    });
            </script>
            </div>
            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True" Image-Url="~/Content/IMAGES/AjaxLoader.gif" ImagePosition="Top" ContainerElementID="filterticketlist"  Text="Loading..."></dx:ASPxLoadingPanel>
        </div>
    </div>
</div>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SLARulesView.ascx.cs" Inherits="uGovernIT.Web.SLARulesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        dx_SPGrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        dx_SPGrid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
    function expandAllTask() {
        dx_SPGrid.ExpandAll();
    }
    function collapseAllTask() {
        dx_SPGrid.CollapseAll();
    }
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" id="header">
        <div class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-12 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="dxddlModule" Width="100%" runat="server" AutoPostBack="true" CssClass="aspxComBox-dropDown"
                    OnSelectedIndexChanged="dxddlModule_SelectedIndexChanged" ListBoxStyle-CssClass="aspxComboBox-listBox">
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-12 noPadding">
            <div class="headerContent-right">
                <div class="headerItem-addItemBtn" style="padding-top: 25px">
                    <dx:ASPxButton ID="aAddItem_Top" runat="server" Text="Add New Item" AutoPostBack="false" CssClass="primary-blueBtn" 
                        Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png"></dx:ASPxButton>
                </div>
                
                <div class="menuNav-applyChngBtn">
                    <dx:ASPxButton ID="btnApplyChanges" CausesValidation="false" Text="Apply Changes" runat="server" OnClick="btnApplyChanges_Click"
                        CssClass="primary-blueBtn">
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top">
        <div class="col-md-1 col-sm-1 col-xs-6 noPadding">
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
        <div class="col-md-2 col-sm-2 col-xs-6 noPadding">
            <div class="crm-checkWrap">
                <asp:CheckBox ID="dxShowDeleted" TextAlign="Left" runat="server" Visible="true" Text="Show Deleted" AutoPostBack="true" OnCheckedChanged="dxShowDeleted_CheckedChanged"/>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <ugit:ASPxGridView ID="dx_SPGrid" runat="server" CssClass="customgridview homeGrid" Width="100%" KeyFieldName="ID" AutoGenerateColumns="false"
            EnableRowsCache="False" OnHtmlDataCellPrepared="dx_SPGrid_HtmlDataCellPrepared" ClientInstanceName="dx_SPGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="SLACategoryChoice" Caption="SLA Category" GroupIndex="1"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="1" Name="aEdit">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title" VisibleIndex="2">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Priority" VisibleIndex="3" FieldName="Priority">
                    <%-- <DataItemTemplate>
                              <a id="editLink" runat="server" href="" ></a>
                         </DataItemTemplate>--%>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="StartStage" Caption="Start Stage" VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EndStage" Caption="End Stage" VisibleIndex="5"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SLA" Caption="SLA" VisibleIndex="6"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SLATarget" Caption="SLA Target %" CellStyle-HorizontalAlign="Left" VisibleIndex="7"></dx:GridViewDataTextColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
            <Settings ShowGroupPanel="false" ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowFixedGroups="true" AllowSort="true" />
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </ugit:ASPxGridView>
        </div>
        
        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
        </script>
    </div>
    <div class="row bottom-addBtn">
        <div class="bottom-leftSideBtn">
            <dx:ASPxButton runat="server" ID="dxReGenerateEscalation" CssClass="primary-blueBtn" Text="Refresh Escalations" OnClick="dxReGenerateEscalation_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ID="dxRefreshingEscalation" runat="server" Visible="false" Enabled="false" CssClass="primary-blueBtn"
                Text="Refresh Escalations (In Process)">
            </dx:ASPxButton>
        </div>
        <div class="headerItem-addItemBtn">
            <dx:ASPxButton ID="aAddItem" runat="server" CssClass=" primary-blueBtn" Text="Add New Item" AutoPostBack="false" 
                Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png"></dx:ASPxButton>
        </div>
    </div>
</div>

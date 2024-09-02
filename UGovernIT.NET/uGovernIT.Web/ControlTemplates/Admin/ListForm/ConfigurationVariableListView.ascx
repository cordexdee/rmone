<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigurationVariableListView.ascx.cs" Inherits="uGovernIT.Web.ConfigurationVariableListView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    a, img {
        border: 0px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        dxgridConfigVariable.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        dxgridConfigVariable.SetHeight(containerHeight);
    }

    try {
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
    } catch (e) {
    }
    function expandAllTask() {
        dxgridConfigVariable.ExpandAll();
    }
    function collapseAllTask() {
        dxgridConfigVariable.CollapseAll();
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .wrapEmail {
        word-break: break-all;
    }

    .SearchInput{
        background:none !important;
    }
</style>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" style="float: right; margin-top: 15px">
        <div class="col-md-12" style="margin-bottom:5px">
            <dx:ASPxTextBox ID="txtSearch" AutoCompleteType="Disabled" CssClass="form-control SearchInput primary-btn-link" runat="server" Width="170px" Height="25px" NullText="Search">
            </dx:ASPxTextBox>
            <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                <img id="Img1" runat="server" src="~/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes"
                ToolTip="Apply Changes" OnClick="btnApplyChanges_Click">
            </dx:ASPxButton>

            <dx:ASPxButton ID="btnExport" runat="server" Text="Export" ToolTip="Export" CssClass="primary-blueBtn" OnClick="btnExport_Click">
            </dx:ASPxButton>
        </div>
        
    </div>
    <div class="col-md-2" style="float:left;margin-bottom:5px;margin-top:25px" >
           <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
    <div class="row" style="clear: both;">
     
        
        <ugit:ASPxGridView ID="dxgridConfigVariable" ClientInstanceName="dxgridConfigVariable" runat="server" AutoGenerateColumns="false"
            OnHtmlDataCellPrepared="dxgridConfigVariable_HtmlDataCellPrepared"
            Width="100%" SettingsPager-Mode="ShowAllRecords" KeyFieldName="ID">
            <SettingsSearchPanel CustomEditorID="txtSearch" />
            <%--<SettingsSearchPanel Visible="true" ShowApplyButton="True" ShowClearButton="True" />--%>
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="CategoryName" Caption="Category" GroupIndex="1"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Name="aEdit" Width="50">
                    <DataItemTemplate>
                        <a id="editlink" runat="server" href="">
                            <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" style="width: 16px;" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="KeyName" SortOrder="Ascending">
                    <DataItemTemplate>
                        <a id="editlink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="KeyValue" Caption="Value" SortOrder="Ascending">
                    <CellStyle CssClass="wrapEmail"></CellStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Description" Caption="Description" SortOrder="Ascending"></dx:GridViewDataTextColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Settings ShowGroupPanel="false" ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowFixedGroups="true" AllowSort="true" AutoExpandAllGroups="true" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                <Header CssClass=" homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
        </ugit:ASPxGridView>
    </div>
    <div class="row" style="float: right; margin: 15px 0">
        <a id="aAddItem" runat="server" href="" class="primary-btn-link">
            <img runat="server" src="~/Content/Images/plus-symbol.png" />
            <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
        <div>
            &nbsp;
        </div>
    </div>
</div>
<dx:ASPxSpreadsheet ID="SpreadSheetConfigVar" runat="server" Visible="false"></dx:ASPxSpreadsheet>

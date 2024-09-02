<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetCategoriesView.ascx.cs" Inherits="uGovernIT.Web.BudgetCategoriesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenImportExcel() {
        UgitOpenPopupDialog('<%= importUrl %>', "", 'Import Budget Category', '400px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function expandAllTask() {
        Insdxgridview.ExpandAll();
    }
    function collapseAllTask() {
        Insdxgridview.CollapseAll();
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="margin-top:10px;">
    <div class="row">
        <div id="dxheader" runat="server" class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-12 noLeftPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Budget Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="dxddlBudgetType" runat="server" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox"
                    OnSelectedIndexChanged="dxddlBudgetType_SelectedIndexChanged" AutoPostBack="true" Width="100%">
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-12 noRightPadding" style="padding-top: 25px;">

            <div class="headerContent-right">
                <dx:ASPxButton ID="btnImport" runat="server" Visible="false" Text="Import" ToolTip="Import" AutoPostBack="false" CssClass="primary-blueBtn marginLeft15">
                    <ClientSideEvents Click="function(s, e){return OpenImportExcel();}" />
                </dx:ASPxButton>
                <div class="headerItem-addItemBtn">
                    <a id="aAddItem_Top" runat="server" href="" style="padding-left: 15px" class="primary-btn-link">
                        <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                        <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                    </a>
                </div>
                <div class="headerItem-showChkBox crm-checkWrap" style="margin-top: 0px;">
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-bottom:10px;">
        <div class="col-md-1 colsm-1 col-xs-1 noPadding">
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
        <div class="col-md-2 col-sm-2 col-xs-2  crm-checkWrap noPadding" style="float:right;">
            <asp:CheckBox ID="dxShowDeleted" Text="Show Deleted" runat="server" TextAlign="right" Checked="false" AutoPostBack="true" style="float:right;"
                OnCheckedChanged="dxShowDeleted_CheckedChanged" />
        </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView runat="server" ID="dxgridview" KeyFieldName="ID" SettingsPager-Mode="ShowAllRecords" OnHtmlDataCellPrepared="dxgridview_HtmlDataCellPrepared"
            AutoGenerateColumns="false" Width="100%" CssClass="customgridview homeGrid" ClientInstanceName="Insdxgridview">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="BudgetCategoryName" Caption="Category" GroupIndex="0"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Name="aEdit" VisibleIndex="1">
                    <DataItemTemplate>
                        <a id="editlink" runat="server" href="">
                            <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BudgetSubCategory" VisibleIndex="2" Caption="Budget Sub-Category" SortOrder="Ascending">
                    <DataItemTemplate>
                        <a id="editlink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BudgetAcronym" Caption="Category GL Code" SortOrder="Ascending" VisibleIndex="3"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BudgetCOA" Caption="Sub-Category GL Code" SortOrder="Ascending" VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="IncludesStaffing" Caption="Includes Staffing" SortOrder="Ascending" VisibleIndex="5"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BudgetType" Caption="Budget Type" SortOrder="Ascending" VisibleIndex="6"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BudgetTypeCOA" Caption="Budget Type GL Code" SortOrder="Ascending" VisibleIndex="7"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CapitalExpenditure" Caption="CapEx" SortOrder="Ascending" VisibleIndex="8"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Visible="false"></dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowGroupPanel="false" ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowFixedGroups="true" AllowSort="true" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </ugit:ASPxGridView>
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

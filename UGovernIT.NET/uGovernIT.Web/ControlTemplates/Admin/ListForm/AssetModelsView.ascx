<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetModelsView.ascx.cs" Inherits="uGovernIT.Web.AssetModelsView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function expandAllTask() {
        aspxassetmodels.ExpandAll();
    }
    function collapseAllTask() {
        aspxassetmodels.CollapseAll();
    }
</script>
<%--<div id="header">
   
</div>--%>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" style="margin-bottom:10px;">
        <div class="headerContent-right">
            <div class="headerItem-addItemBtn">
                <a id="aAddItem_Top" runat="server" href="#" style="padding-left: 12px" class="primary-btn-link">
                    <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
           
             <div class="headerItem-showChkBox">
                 <dx:ASPxButton ID="btnApplyChanges" CssClass="primary-blueBtn" runat="server" CausesValidation="false" OnClick="btnRefreshCache_Click" Text="Apply Changes">
                     <ClientSideEvents Click="function(s, e){loadingPanel.Show();}" />
                 </dx:ASPxButton>
            </div>
        </div>
    </div>
    <div class="row" style="margin-bottom:10px;">
        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
            <img src="/Content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/Content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
       <div class="col-md-2 col-sm-2 col-xs-2 noPadding" style="float:right;">
            <div class="headerItem-showChkBox crm-checkWrap" style="margin-top: 0px;">
                <asp:CheckBox ID="dxShowDeleted" Text="Show Deleted" TextAlign="right" runat="server" AutoPostBack="true" style="float:right;"
                    OnCheckedChanged="dxShowDeleted_CheckedChanged"/>
            </div>
       </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="dx_SPGrid" runat="server" Width="100%" KeyFieldName="ID" SettingsPager-Mode="ShowAllRecords"
            OnHtmlDataCellPrepared="dx_SPGrid_HtmlDataCellPrepared" AutoGenerateColumns="false" CssClass="customgridview homeGrid"
            EnableRowsCache="False" ClientInstanceName="aspxassetmodels">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="VendorName" Caption="Vendor Name" GroupIndex="1"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn VisibleIndex="1" Name="aEdit" Width="10px">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Model Name" FieldName="ModelName" VisibleIndex="2">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ModelDescription" Caption="Description" VisibleIndex="4"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="BudgetItem" Caption="Budget Item" VisibleIndex="5"></dx:GridViewDataTextColumn>
            </Columns>
            <Styles>
                <AlternatingRow Enabled="True"></AlternatingRow>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
            <Settings ShowGroupPanel="false" ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowFixedGroups="true" AllowSort="true" />
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="ModelName" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight>
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

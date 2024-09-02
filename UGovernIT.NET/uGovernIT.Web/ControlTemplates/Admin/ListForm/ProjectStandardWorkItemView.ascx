<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectStandardWorkItemView.ascx.cs" Inherits="uGovernIT.Web.ProjectStandardWorkItemView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" style="margin:5px 0;">
        <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
            <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
    </div>
    <div class="row" style="margin-bottom:10px;">
        <div class="col-md-3 col-sm-3 col-xs-6 noPadding">
            <div class="crm-checkWrap">
                <asp:CheckBox ID="chkEnableProjectStandardWorkItems" Text="Enable Standard Work Items" runat="server" TextAlign="Right" AutoPostBack="true" 
                    OnCheckedChanged="chkEnableProjectStandardWorkItems_CheckedChanged" />
            </div>
        </div>
        <div class="col-md-2 col-sm-2 col-xs-6 noPadding" style="float:right;">
            <div class="crm-checkWrap" style="float:right;">
                <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted"  Style="margin: 5px;" runat="server" TextAlign="Right" AutoPostBack="true" 
                    OnCheckedChanged="chkShowDeleted_CheckedChanged" />
            </div>
        </div>
    </div>
    <div class="row">
        <dx:ASPxGridView ID="dx_gridView" runat="server" KeyFieldName="ID" OnHtmlDataCellPrepared="dx_gridView_HtmlDataCellPrepared" CssClass="customgridview homeGrid"
            OnHtmlRowPrepared="dx_gridView_HtmlRowPrepared" Width="100%">            
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="20px">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" style="width:16px" runat="server" src="/Content/images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataSpinEditColumn Caption="Order" FieldName="ItemOrder" Width="50px">
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataSpinEditColumn Caption="Code" FieldName="Code">
                    <CellStyle HorizontalAlign="Center" ></CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataSpinEditColumn Caption="Description" FieldName="Description">
                    <CellStyle HorizontalAlign="Left" ></CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                </dx:GridViewDataSpinEditColumn>
                <dx:GridViewDataTextColumn Caption="Budget Category" FieldName="BudgetCategoryName"></dx:GridViewDataTextColumn>
            </Columns>
             <Settings ShowHeaderFilterButton="true" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
             <SettingsPopup>
                <HeaderFilter MinHeight="250px" />
            </SettingsPopup>
            <SettingsPager Visible="true" AlwaysShowPager="true" Mode="ShowPager" Position="TopAndBottom" PageSize="50">
                <PageSizeItemSettings Items="15, 20, 25, 50, 75, 100" />
            </SettingsPager>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </dx:ASPxGridView>
    </div>

    <div class="row" style="margin:5px 0;">
        <a id="aAddItem" runat="server" href="" class="primary-btn-link">
            <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
    </div>
</div>

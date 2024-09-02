<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiNavigationView.ascx.cs" Inherits="uGovernIT.Web.WikiNavigationView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openWikinavigationDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '40', '65', 0);
    }

    function NewWikiNavigationDialog() {
        window.parent.UgitOpenPopupDialog('<%=viewUrl%>&WikiId=0', "", "Add Wiki Navigation", '40', '65', 0);
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row">
        <div style="float: right; padding: 10px 8px 15px;">
            <div class="headerItem-addItemBtn">
                <a id="LinkButton1" runat="server" href="" class="primary-btn-link">
                    <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
        </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="ASPxGridViewWikiNavigation" AutoGenerateColumns="False" runat="server" 
            SettingsText-EmptyDataRow="No record found." CssClass="customgridview homeGrid"
            KeyFieldName="ID" Width="99%" ClientInstanceName="DXWikiNavigation" 
            OnHtmlRowPrepared="ASPxGridViewWikiNavigation_HtmlRowPrepared">
            <Columns>
                <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0" Width="40px">
                    <DataItemTemplate>
                        <img src="/Content/Images/editNewIcon.png" alt="Edit" style="float: right; padding-right: 10px; width: 25px" />
                    </DataItemTemplate>
                    <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataColumn FieldName="ItemOrder" VisibleIndex="1" Width="40px" Caption="#" CellStyle-HorizontalAlign="Center">
                    <Settings AllowAutoFilter="False" AllowHeaderFilter="False" />
                </dx:GridViewDataColumn>

                <dx:GridViewDataDateColumn FieldName="Title" VisibleIndex="2" Caption="Category">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataDateColumn>

                <dx:GridViewDataColumn FieldName="ColumnType" VisibleIndex="3" Width="150px" Caption="Type">
                    <Settings HeaderFilterMode="CheckedList" />
                </dx:GridViewDataColumn>
            </Columns>

            <Settings ShowFooter="True" ShowHeaderFilterButton="true"  />
            <%--<Settings VerticalScrollableHeight="300" />--%>
            <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
            <Settings VerticalScrollBarMode="Auto" />
            <SettingsPager PageSize="15"></SettingsPager>
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
                <Header CssClass="homeGrid_headerColumn"></Header>
                <Row CssClass="homeGrid_dataRow"></Row>
                <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            </Styles>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div style="float: right; padding:15px 5px">
            <div class="headerItem-addItemBtn">
                <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                    <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
        </div>
    </div>
</div>

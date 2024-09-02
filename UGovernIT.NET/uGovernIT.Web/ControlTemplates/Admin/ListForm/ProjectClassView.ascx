<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectClassView.ascx.cs" Inherits="uGovernIT.Web.ProjectClassView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <div class="fright" style="padding-top:20px;">
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApplyChanges_Click">
            </dx:ASPxButton>
             <div style="float: right;">
                <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                    <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding  crm-checkWrap">
            <asp:CheckBox  ID="dxShowDeleted" Text="Show Deleted" runat="server" TextAlign="right" AutoPostBack="true" OnCheckedChanged="dxShowDeleted_CheckedChanged"/>
        </div>
    </div>
    <div class="row">
         <ugit:ASPxGridView runat="server" ID="dxgridview" KeyFieldName="ID" CssClass="customgridview homeGrid" AutoGenerateColumns="false" Width="100%" OnHtmlDataCellPrepared="dxgridview_HtmlDataCellPrepared">
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit">
                    <DataItemTemplate>
                        <a id="editlink" runat="server" href="">
                            <img id="Imgedit" style="width: 16px" runat="server" src="~/Content/Images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" SortOrder="Ascending">
                    <DataItemTemplate>
                        <a id="editlink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ProjectNote" Caption="Description" SortOrder="Ascending"></dx:GridViewDataTextColumn>
            </Columns>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowSort="true" />
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div class="headerItem-addItemBtn" style="float:right; padding-top:15px; padding-bottom:25px;">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>

    <%--<div style="float: right; border:2px solid red">
    <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted" runat="server" TextAlign="Left" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged"/>


     <a id="aAddItem_Top1" runat="server" href="" style="padding-left:5px">
                    <img id="Img2" runat="server" src="../Content/images/uGovernIT/add_icon.png" />
                    <asp:Label ID="Label1" runat="server" Text="Add New Item"></asp:Label>
                </a>

</div>
<div id="content">
    <table width="100%" align="left">
        <tr>
            <td align="left">
                <div style="width: 100%; float: right;">
                    <div style="border: 2px solid #CED8D9">
                       <asp:GridView ID="_gridView" runat="server" Width="100%" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke"
                            HeaderStyle-Height="20px" HeaderStyle-CssClass="gridheader" HeaderStyle-Font-Bold="false"  AutoGenerateColumns="false" AllowFiltering="true"
                            DataKeyNames="ID" OnRowDataBound="_gridView_RowDataBound" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Height="2">
                                    <ItemTemplate>
                                        <a id="aEdit" runat="server" href="">
                                            <img id="Imgedit" runat="server" src="/Content/images/edit.gif"/>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Title">
                                    <ItemTemplate>
                                        <a id="aTitle" runat="server" href=""></a>
                                        <asp:HiddenField runat="server" ID="hiddenTitle" Value='<%#Bind("Title") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ProjectNote" HeaderText="Description" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                <a id="aAddItem1" runat="server" href="">
                    <img id="Img1" runat="server" src="../Content/images/uGovernIT/add_icon.png" />
                    <asp:Label ID="LblAddItem" runat="server" Text="Add New Item"></asp:Label>
                </a>
            </td>
        </tr>
    </table>

</div>--%>

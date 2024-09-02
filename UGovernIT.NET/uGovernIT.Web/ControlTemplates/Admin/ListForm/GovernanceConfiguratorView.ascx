<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GovernanceConfiguratorView.ascx.cs" Inherits="uGovernIT.Web.GovernanceConfiguratorView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
         <div style=" float:right;margin:10px;">
             <a id="aAddItem_Top" runat="server" href=""  class="primary-btn-link" >
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png"  />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
         </div>
     </div>
    <div class="row">
        <ugit:ASPxGridView ID="dx_gridView" runat="server"  SettingsPager-Mode="ShowAllRecords" OnHtmlDataCellPrepared="dx_gridView_HtmlDataCellPrepared" 
            Width="100%" KeyFieldName="ID" CssClass=" customgridview homeGrids">
            <Columns>
                 <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" width="16" src="~/Content/Images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title">
                   <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Description" FieldName="Description"></dx:GridViewDataTextColumn>
           
            </Columns>
             <Settings ShowHeaderFilterButton="true" />       
            <Styles>
                <Row CssClass=" homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
                <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div style="float:right;margin-top:3px">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png"  />
                <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>




<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WidgetsView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.WidgetsView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 phrases-popupContainer">
    <div class="row">
        <ugit:ASPxGridView ID="gridWidgets" runat="server" Width="100%" KeyFieldName="Id" AutoGenerateColumns="false" EnableCallBacks="true" SettingsPager-PageSize="20" 
             CssClass="customgridview homeGrid" OnHtmlDataCellPrepared="gridWidgets_HtmlDataCellPrepared">
            <Columns>
                <dx:GridViewDataColumn Name="Edit">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" style="width:16px;" runat="server" src="~/Content/Images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>

                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="Title" Caption="Title">
                    <DataItemTemplate>
                        <a id="editLink" runat="server"></a>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="Description" Caption="Description">                  
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="Icon" Caption="Icon">
                </dx:GridViewDataColumn>
                
            </Columns>
            <Styles>
                <Header CssClass="homeGrid_headerColumn"></Header>
                <Row CssClass="phrasesGrid-dataRow homeGrid_dataRow"></Row>
            </Styles>
        </ugit:ASPxGridView>
    </div>
     <div class="row">
         <div class="addPhrases-link">
            <a id="aWidget" runat="server" href="" class="primary-btn-link">
                <img id="imgWidget" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="lblAddWidget" runat="server" Text="Add New Widget" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
     </div>
</div>

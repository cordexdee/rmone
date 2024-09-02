<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhrasesView.ascx.cs" Inherits="uGovernIT.Web.PhrasesView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 phrases-popupContainer">
    <div class="row">
        <ugit:ASPxGridView ID="gridPhrase" runat="server" Width="100%" KeyFieldName="PhraseId" AutoGenerateColumns="false" EnableCallBacks="true" SettingsPager-PageSize="20" 
            OnHtmlDataCellPrepared="gridPhrase_HtmlDataCellPrepared" CssClass="customgridview homeGrid">
            <Columns>
                <dx:GridViewDataColumn Name="Edit">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" style="width:16px;" runat="server" src="~/Content/Images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>

                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="Phrase" Caption="Phrase">
                    <DataItemTemplate>
                        <a id="editLink" runat="server"></a>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="AgentType" Caption="Agent Type">                  
                </dx:GridViewDataColumn>

                 <dx:GridViewDataColumn FieldName="TicketType" Caption="Ticket Type">
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="RequestTypeName" Caption="Request Type">
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="ServiceName" Caption="Services">
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
            <a id="aPhrase" runat="server" href="" class="primary-btn-link">
                <img id="imgAddPhrase" runat="server" src="/Content/Images/plus-symbol.png" style="width:12px;" />
                <asp:Label ID="lblAddPhrase" runat="server" Text="Add New Phrase" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
     </div>
</div>



<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DRQSystemAreaView.ascx.cs" Inherits="uGovernIT.Web.DRQSystemAreaView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" style="padding-top:15px;">
        <div style="float:right;">
            <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                <img id="Img12" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
        <div style="float:right;">
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes" 
                OnClick="btnApplyChanges_Click"></dx:ASPxButton>
        </div>
    </div>
    <div class="row">
        <div class="crm-checkWrap" style="margin-bottom:15px;">
            <asp:CheckBox ID="dxchkShowDeleted" Text="Show Deleted" runat="server" TextAlign="Left" AutoPostBack="true" OnCheckedChanged="dxchkShowDeleted_CheckedChanged"/>
        </div>
        <ugit:ASPxGridView ID="dxGvDSAreas" OnHtmlDataCellPrepared="dxGvDSAreas_HtmlDataCellPrepared" Width="100%" runat="server"  KeyFieldName="ID"  
            AutoGenerateColumns="false"  ClientInstanceName="gridView" EnableCallBacks="true" CssClass="customgridview homeGrid">
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="5%">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""> 
                           <img id="Imgedit" runat="server" style="width:16px" src="~/Content/Images/editNewIcon.png"/>
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" SortOrder="Ascending">
                    <DataItemTemplate>
                      <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
            
                </dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowHeaderFilterButton="true"/>
            <SettingsPager PageSize="10"></SettingsPager>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
             <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]= True" Format="Custom" RowStyle-CssClass="formatcolor" ApplyToRow="true"/>
            </FormatConditions>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div style="float:right;padding-top:5px;">
             <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img11" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="LblAddItem1" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
    </div>
</div>





 



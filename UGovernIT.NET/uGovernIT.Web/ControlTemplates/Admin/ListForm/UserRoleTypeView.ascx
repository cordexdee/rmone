<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleTypeView.ascx.cs" Inherits="uGovernIT.Web.UserRoleTypeView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="margin-top:5px;">
   <div class="row">
       <div class="col-md-4 col-sm-6 col-xs-6 noPadding ms-formtable accomp-popup">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module</h3>
            </div>
           <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="dxcombox" AutoPostBack="true" CssClass="aspxComBox-dropDown" Width="100%" runat="server" ListBoxStyle-CssClass="aspxComboBox-listBox"
                    OnSelectedIndexChanged="combox_SelectedIndexChanged"></dx:ASPxComboBox>
           </div>
        </div>
       <div class="col-md-8 col-sm-6 col-xs-6" style="padding-top:15px; padding-right:0;">
           <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link" style="float:right;">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
           <div style="float:right">
               <dx:ASPxButton ID="btnApplyChanges" runat="server" ToolTip="Apply Changes" CssClass="primary-blueBtn" Text="Apply Changes" OnClick="btnApplyChanges_Click"></dx:ASPxButton>
           </div>
           
       </div>
   </div>
    <div class="row">
        <ugit:ASPxGridView ID="dxgridview" Width="100%" runat="server" AutoGenerateColumns="false" CssClass="customgridview homeGrid"
            OnHtmlDataCellPrepared="dxgridview_HtmlDataCellPrepared" KeyFieldName="ID" EnableCallBacks="false">
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" SortOrder="Ascending">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit1" runat="server" src="~/Content/Images/editNewIcon.png" style="width: 16px;" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ColumnName" Caption=" Column Name" SortOrder="Ascending">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="UserTypes" Caption="User Type" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="DefaultUser" Caption="Default User" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ITOnly" Caption="IT" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ManagerOnly" Caption="Manager" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Groups" Caption="User Groups" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CustomProperties" Caption="Custom Properties" SortOrder="Ascending"></dx:GridViewDataTextColumn>
            </Columns>
            <Settings ShowHeaderFilterButton="true" />
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Format="BoldText" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[ITOnly] = 0"></dx:GridViewFormatConditionHighlight>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Format="BoldText" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[ManagerOnly] = 0"></dx:GridViewFormatConditionHighlight>
            </FormatConditions>
        </ugit:ASPxGridView>
    </div>
    <div class="row" style="padding:15px 0;">
        <a id="aAddItem1" runat="server" href="" class="primary-btn-link">
            <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
    </div>
</div>


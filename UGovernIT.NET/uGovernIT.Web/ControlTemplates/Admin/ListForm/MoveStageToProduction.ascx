
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MoveStageToProduction.ascx.cs" Inherits="uGovernIT.Web.MoveStageToProduction" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<dx:aspxloadingpanel id="LoadingPanelMigrate" runat="server" text="Please Wait ..." clientinstancename="LoadingPanelMigrate" modal="True">
 </dx:aspxloadingpanel>
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row crm-checkWrap">
        <asp:CheckBox ID="chkAllModule" Text="All Module" runat="server" Visible="false" />
        <asp:CheckBox ID="chkAllServices" Text="All Services" runat="server" Visible="false" />
    </div>
    <div class="row" id="dvDifference" runat="server">
        <div class="ms-formlabel">
            <asp:Label CssClass="ms-standardheader budget_fieldLabel" ID="lblDifference" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap migrate-popup-btnWrap">
        <dx:ASPxButton ID="migrateCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" onClick="btnCancel_Click"></dx:ASPxButton>
        <dx:ASPxButton ID="btnMoveToProduction" runat="server" Text="Migrate" ToolTip="Migrate" CssClass="primary-blueBtn" OnClick="btnMoveToProduction_Click">
            <ClientSideEvents Click="function(s, e){return LoadingPanelMigrate.Show();}" />
        </dx:ASPxButton>
        
           <%-- <asp:Button ID="" CssClass="AspPrimary-blueBtn" runat="server" Text="Migrate" OnClientClick="" OnClick="" />
            <input type="button" value="Cancel" class="htmlSecondary-Btn"  />--%>
    </div>
</div>

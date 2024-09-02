<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorDropdownList.ascx.cs" Inherits="uGovernIT.Web.VendorDropdownList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .container {
        width: 100%;
        float: left;
    }

    .input-control {
        width: 75%;
        float: left;
    }

    .link-icon {
        width: 20%;
        float: left;
        text-align: left;
        vertical-align: middle;
        padding-left:5px;
        padding-top:5px;
    }

        .link-icon img {
            border: 0;
            background-color: transparent;
        }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function Test(s, e) {
        VendorAddEdit_PopUp.Show();
        //popup.PerformCallback();
    }
    var vendorid;
    function OnSelectionChange(s) {
        if (VendorAddEdit_PopUp.InCallback()) {
            vendorid = s.GetValue().toString();
        }
        else {
            VendorAddEdit_PopUp.PerformCallback("VENDORID|" + s.GetValue().toString());
            $("#<%=hdnVendorid.ClientID%>").val(s.GetValue().toString());
        }
    }

    function OnEditClick()
    {
        VendorAddEdit_PopUp.PerformCallback("VENDORID|" + ddlVendor.GetValue().toString());
        VendorAddEdit_PopUp.Show();
        return false;
    }
    function OnAddClick()
    {
        VendorAddEdit_PopUp.PerformCallback("ADDVENDOR");
        VendorAddEdit_PopUp.Show();
        return false;
    }
    </script>

<asp:HiddenField ID="hdnVendorid"  runat="server" />   
<asp:Panel ID="pnlOrg" runat="server" class="container">
    <div class="input-control">
        <dx:ASPxComboBox ID="ddlVendor" runat="server" ValueType="System.String" Width="100%"
            DropDownStyle="DropDownList" IncrementalFilteringMode="Contains" ClientInstanceName="ddlVendor">
            <ClientSideEvents SelectedIndexChanged="function(s,e){OnSelectionChange(s);}" />
        </dx:ASPxComboBox>
         
    </div>
    <div class="link-icon">
        <asp:LinkButton ID="lnkSearch" runat="server" OnClientClick="return OnEditClick();">
            <img alt="Add" src="/Content/images/edit-icon.png"  title="Edit Vendor" height="12">
        </asp:LinkButton>
        <asp:LinkButton ID="lnkAdd" runat="server" OnClientClick="return OnAddClick();">
            <img alt="Add" src="/Content/images/add_icon.png" title="Add new Vendor">
        </asp:LinkButton>
    </div>
</asp:Panel>
 <asp:Label ID="lblOrg" runat="server" Visible="false"/>
<dx:ASPxPopupControl ID="VendorAddNewPopUp" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" ScrollBars="Auto" 
    ClientInstanceName="VendorAddEdit_PopUp" runat="server" Width="600" Height="500" HeaderText="Add New Organization" OnWindowCallback="VendorAddNewPopUp_WindowCallback">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <asp:Panel ID="pnlOrgPopup" runat="server"></asp:Panel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
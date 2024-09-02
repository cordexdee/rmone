<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactDropDownList.ascx.cs" Inherits="uGovernIT.Web.ContactDropDownList" %>
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
        ContactAddEdit_PopUp.Show();
        //popup.PerformCallback();
    }
    </script>

<asp:Panel ID="pnlContact" runat="server" class="container">
    <div class="input-control">
        <dx:ASPxComboBox ID="ddlContacts" runat="server" ValueType="System.String" Width="100%"
            DropDownStyle="DropDownList" IncrementalFilteringMode="Contains">
            <%--<Columns>
                <dx:ListBoxColumn FieldName="UGITFirstName" Width="100px" />
                <dx:ListBoxColumn FieldName="Description" Width="300px" />
            </Columns>--%>
        </dx:ASPxComboBox>
       
       
    </div>
    <div class="link-icon">
        <asp:LinkButton ID="lnkSearch" runat="server">
            <img alt="Add" src="/Content/images/search32x32.png" title="Search Contact" height="12">
        </asp:LinkButton>
        <asp:LinkButton ID="lnkAdd" runat="server" OnClientClick="ContactAddEdit_PopUp.Show(); return false;">
            <img alt="Add" src="/Content/images/add_icon.png" title="Add new Contact">
        </asp:LinkButton>
    </div>
</asp:Panel>
  <asp:Label ID="lblContact" runat="server" Visible="false"/>
<dx:ASPxPopupControl ID="ContactAddNewPopup" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" ScrollBars="Auto" 
    ClientInstanceName="ContactAddEdit_PopUp" runat="server" Width="600" Height="500" HeaderText="Add New Contact">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <asp:Panel ID="pnlContactPopup" runat="server"></asp:Panel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
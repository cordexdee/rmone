
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemoveUserAccess.ascx.cs" Inherits="uGovernIT.Web.RemoveUserAccess" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .lblHeading {
        font-weight: bold;
        margin-bottom: 10px;
    }

</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">


    //Results displayed if 'OK' or 'Cancel' button is clicked if the html content has 'OK' and 'Cancel' buttons
    function onDialogClose(dialogResult, returnValue) {

    }

</script>

<div  id="divAssignee">
    <asp:Panel ID="pnlAssignee" runat="server" Visible="true">
        <asp:Label ID="lblUser" runat="server" Text="User:" CssClass="lblHeading" Style="margin-bottom: 10px; margin-right: 5px; vertical-align: top; display: inline">

        </asp:Label>
        <ugit:UserValueBox runat="Server" IsMulti="false" ID="pplUser" Width="300px"></ugit:UserValueBox>
        <asp:Label ID="lblUserReadOnly" runat="server" Style="margin-bottom: 10px;" Visible="false">
        
        </asp:Label>
        <asp:Label ID="lblErrorMessage" runat="server" Text="User is Mandatory" Visible="false" Style="color: red;"></asp:Label>
        <asp:RadioButtonList ID="rdRemoveAccess" runat="server" OnSelectedIndexChanged="rdRemoveAccess_SelectedIndexChanged" AutoPostBack="true">
            <asp:ListItem Text="Remove all access from all applications" Value="removeallaccess" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Add/Remove specific access from specific applications" Value="removespecificaccess"></asp:ListItem>
        </asp:RadioButtonList>
          <asp:Button ID="btnAllApplications" runat="server" Visible="true" Style="display: block; vertical-align:top; margin-top:10px;" Text="Show List of Applications to remove access from" OnClick="btnAllApplication_Click" />
        <asp:Panel ID="pnlSelection" runat="server" Visible="false" Style="padding-top:5px;">
            <asp:Label ID="lblSelection" runat="server" CssClass="lblHeading" Style="margin-bottom: 10px; margin-right: 5px; vertical-align: top; display: inline" Text="Selection Type:"></asp:Label>
            <asp:Label ID="lblselectionType" runat="server" Style="margin-bottom: 10px;">
            </asp:Label>
        </asp:Panel>
        <asp:Panel ID="pnlUserAccess" runat="server" Visible="false"></asp:Panel>
        <asp:Label ID="lblNoApp" runat="server" Text="No Application Exists." Visible="false" CssClass="lblHeading"></asp:Label>
    </asp:Panel>
    <asp:Panel ID="pnlApplications" runat="server" Style="display: none; padding-top:10px;">
     <div style="padding-left:10px;">   <asp:Label ID="lblPopUpMessage" runat="server" style="font-weight:bold;">All access from the below listed applications will be removed</asp:Label></div>
        <asp:GridView ID="gvApplications" runat="server" AutoGenerateColumns="false" HeaderStyle-CssClass="titleHeaderBackground tablerow" Style="margin: 10px; border:1px solid black"
             GridLines="Both" CssClass="gvResourceClass" Width="95%">
            <HeaderStyle BackColor="LightGray" Height="25px" Font-Bold="true" HorizontalAlign="Left"/>
            <AlternatingRowStyle BackColor="white" Height="25px"  HorizontalAlign="Left"/>
            <RowStyle BackColor="WhiteSmoke"  Height="25px" HorizontalAlign="Left"/>
            <Columns>
                <asp:BoundField DataField="APPTitleLookup" HeaderText="Applications" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
</div>

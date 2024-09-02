
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopMenuBarSubItem.ascx.cs" Inherits="uGovernIT.Web.TopMenuBarSubItem" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function showSubMenuPopUp(linkUrl, title) {
        ASPxClientMenuBase.GetMenuCollection().HideAll();
        window.parent.UgitOpenPopupDialog(linkUrl, "", title, 90, 90, false, "");
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .divSubMenuContainer {
    width:100%;
    text-align:left;
    }
</style>
<div id="divSubMenu" runat="server" class="divSubMenu">
    <asp:Repeater ID="subMenuRepeater" runat="server" OnItemDataBound="subMenuRepeater_ItemDataBound">
        <ItemTemplate>
            <div id="divSubMenuContainer" runat="server" class="divSubMenuContainer">
                <asp:Repeater ID="subMenuItemRepeater" runat="server" OnItemDataBound="subMenuItemRepeater_ItemDataBound">
                    <ItemTemplate>
                        <asp:Panel ID="divSubMenuitem" runat="server" class="divSubMenuitem">
                            <asp:HyperLink runat="server" ID="subMenuLink" CssClass="subMenuItem anchorRootMenuItem">
                  
                            </asp:HyperLink>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</div>


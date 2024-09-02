<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UGITCustomLeftMenuBar.ascx.cs" Inherits="uGovernIT.Web.UGITCustomLeftMenuBar" %>
<dx:ASPxPanel ID="dvTopMenu" runat="server" EnableTheming="true">
    <PanelCollection>
        <dx:PanelContent>
            <table style="width: 100%; border-collapse: collapse;">
                <tr>
                    <td id="tdmenu" runat="server">
                        <dx:ASPxMenu ID="menuLeftBar" ClientInstanceName="menuLeftBar" runat="server" Orientation="Vertical" 
                            EnableTheming="false" AutoSeparators="None" Width="200" >
                            <Items>

                            </Items>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>
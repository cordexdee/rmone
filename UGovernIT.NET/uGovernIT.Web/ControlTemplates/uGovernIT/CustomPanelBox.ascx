<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomPanelBox.ascx.cs" Inherits="uGovernIT.Web.CustomPanelBox" %>

   <div id="<%=pageId %>_Box" class="fleft custompanelbox" style="height:<%=BoxHeight%>;width:<%=BoxWidth%>;padding-right:5px;padding-bottom:5px;">
        <div id="<%=pageId %>_Box_Panel" style="border:1px solid black;" >
         <table width="100%" cellpadding="0" cellspacing="0"><tr><td>
           <div id="<%=pageId %>_Box_Panel_Header" class="boxheader" >
             <div class="ugitlinkbg" style="position:relative;float:left; background-color:aliceblue;  width:100%;padding:2px 0px;">
             <span><asp:Label ID="BoxHeaderTitle" CssClass="menu-item-text" style="font-size:1.2em;float:left;padding-left:5px;" runat="server" ></asp:Label></span>
             <span><img src="/content/images/FILTEROFF.gif" class="globalfilterbutton cursor"  style="border:0;display:none;" alt="Clear" id="filterOutIcon" runat="server" /></span>
             </div>
            </div>
            </td></tr>
             <tr>
             <td  align="center"  valign="top" style="display:block;">
               <asp:PlaceHolder ID="ContentPlaceHolder" runat="server" ></asp:PlaceHolder>
            </td></tr>
            </table>
        </div>
    </div>
   

  
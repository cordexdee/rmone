
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectAllStatusSummary.ascx.cs" Inherits="uGovernIT.Web.ProjectAllStatusSummary" %>

<asp:Panel Visible="true" ID="pEmptyMsg" runat="server" style="border: 1px solid;float: left;padding: 4px;text-align: center;width: 98%;" >
    <span>No summary entered</span>
</asp:Panel>
<asp:Panel ID="pSummary" runat="server" Visible="false" style="padding-top:20px;">
<table  border="0" style="width:100%">
    <asp:Repeater ID="rSummaryItems" runat="server">
       <ItemTemplate>
           <tr class=" ms-selectednav whiteborder" style="border-width:1px;border-style:Solid; ">
             <td class=" tableCell labelCell ms-selectednav whiteborder"  style="border-width:0px;border-style:None;height:30px;text-align:right;">
                <div style="width:140px;float:left;"> <%# Eval("createdBy") %></div>
                <div style="font-size:10px;color:gray;font-style:italic"> (<%# Eval("created") %>)</div>
             </td>
             <td class="tableCell ms-alternatingstrong" valign="top" colspan="5" style="color:Black;border-width:0px;border-style:None;height:30px;width:80%;">
                  <%# Eval("entry") %>
             </td>
          </tr>
       </ItemTemplate>
    </asp:Repeater>
</table>
</asp:Panel>
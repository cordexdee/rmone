<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleItemApprovals.ascx.cs" Inherits="uGovernIT.Web.ModuleItemApprovals" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .whiteborder {
        border: 1px solid white;
    }

    .first-column, .second-column {
        height: 42px;
        color: #000;
    }

    .wf-stage.completed {
        background-color: #228B22;
        color: #e8e5e5;
    }
    .wf-stage {
        position: relative;
        height: 24px;
        width: 24px;
        vertical-align: middle;
        text-align: center;
        border-radius: 12px;
    }
    tr.ms-selectednav:nth-child(even) .ms-alternatingstrong{
        background-color: rgba(239, 239, 239, 0.78);
    }
</style>

<table border="0" style="border-collapse: collapse;">
    <asp:Repeater ID="rApprovals" runat="server" OnItemDataBound="rApprovals_ItemDataBound">
        <ItemTemplate>
            <tr class="ms-selectednav">
                <td>
                    <div id="detRun" runat="server"  visible='<%#Convert.ToBoolean(Eval("isParent")) %>'  class="wf-stage completed">
                        <span class="pos_rel" style="padding-top:5px;">
                                <i>
                                    <b><%# Eval("DisplayStageStep") %></b>
                                </i>
                        </span>
                    </div>
                </td>
                <td id="trTitle" runat="server" class="tableCell ms-selectednav first-column" style="width:350px; text-align:left;">
                    <%# Eval("Title")%>
                </td>
                <td class="tableCell  ms-selectednav first-column" style='<%# Convert.ToBoolean(Eval("Latest")) ? "width:200px; text-align:center;font-weight:600" : "width:200px; text-align:center;" %>'>
                    <%# Eval("Action") %>
                </td>
                <td class="tableCell ms-selectednav first-column" style="width:200px; text-align:center;">
                    <%# Eval("UserName")%>
                </td>
                <td class="tableCell ms-selectednav first-column" style="width:200px; text-align:center;">
                    <%# Eval("EndDateTime")%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="ms-selectednav">
                <td>
                    <div id="detRun" runat="server"  visible='<%#Convert.ToBoolean(Eval("isParent")) %>'  class="wf-stage completed">
                        <span class="pos_rel" style="padding-top:5px;">
                                <i>
                                    <b><%# Eval("DisplayStageStep") %></b>
                                </i>
                        </span>
                    </div>
                </td>
                <td id="trTitle" runat="server" class="tableCell ms-alternatingstrong second-column" style="width:350px; text-align: left;">
                    <%# Eval("Title")%>
                </td>
                <td class="tableCell  ms-alternatingstrong second-column" style='<%# Convert.ToBoolean(Eval("Latest")) ? "width:200px; text-align:center;font-weight:600" : "width:200px; text-align:center;" %>'>
                    <%# Eval("Action") %>
                </td>
                <td class="tableCell ms-alternatingstrong second-column" style="width:200px; text-align:center;">
                    <%# Eval("UserName")%>
                </td>
                <td class="tableCell ms-alternatingstrong second-column" style="width:200px; text-align:center;">
                    <%# Eval("EndDateTime")%>
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>



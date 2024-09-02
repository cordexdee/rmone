<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskPredecessorsControl.ascx.cs" Inherits="uGovernIT.Web.TaskPredecessorsControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .leftpad100 {
        padding-left: 26px !important;
    }

    .hightlightcurrentTask {
        background-color: #d6def6;
        font-weight: bold;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function getCheckedNodeDates(parent, dates) {
        for (var i = 0; i < parent.GetNodeCount() ; i++) {
            if (parent.GetNode(i).GetChecked()) {
                var node = parent.GetNode(i);
                dates.push(new Date(node.treeView["cp" + node.name]));
            }
            if (parent.GetNode(i).GetNodeCount() != 0) {
                getCheckedNodeDates(parent.GetNode(i), dates);
            }
        }
    }

    function getMaxDate(dates) {
        var x = dates;
        for (i in x) {
            if (Math.max.apply(null, x) === dates[i].getTime()) return dates[i];
        }
    }

    function OnCheckedChanged(s, e) {
        <% if (PredecessorMode == uGovernIT.Web.PredecessorType.Task)
    {%>
        //if (e.node.GetChecked()) {
        //    setStartDateAndDuration1(e.node);
        //}

        var dates = [];
        getCheckedNodeDates(s, dates);

        if (dates.length > 0) {
            var maxDate = getMaxDate(dates);
            AdjustStartByPredecessorDueDate(maxDate);
        }
        <%}%>
        var tasknode = e.node.GetText();
        var name = e.node.name;
        var textvalue = $('#<%=hdnPred.ClientID%>').val();//lPred.GetText();
        var textids = $('#<%= hdnPredIds.ClientID%>').val();
        var predecessors = textids.split(';');
        if ($.inArray(name, predecessors) > -1 && e.node.GetChecked()) {

            return;
        }
        else if ($.inArray(name, predecessors) > -1 && !e.node.GetChecked()) {
            textvalue = textvalue.replace(tasknode + ";", "");
            $('#<%=hdnPred.ClientID%>').val(textvalue);//lPred.SetText(textvalue);
            textids = textids.replace(name + ";", "");
            $('#<%= hdnPredIds.ClientID%>').val(textids);
            SetCheckedState(e.node, e.node.GetCheckState());
            return;
        }
        textvalue += tasknode + ";";
        $('#<%=hdnPred.ClientID%>').val(textvalue);//lPred.SetText(textvalue);
        textids += name + ";";
        $('#<%= hdnPredIds.ClientID%>').val(textids);
        SetCheckedState(e.node, e.node.GetCheckState());
    }

    $(document).ready(function () {
      
        <% if (tvTasks.Nodes.Count > 0)
           {%>
        for (var i = 0; i < tvTasks.GetNodeCount(); i++) {
            IsNodeChecked(tvTasks.GetNode(i));
        }
        <%}%>
        $(".tvpredecessorstasks").removeAttr("style")
        $(".tvpredecessorstasks").attr("style", "width:100% !important")
    });

    function IsNodeChecked(clientnode) {
        if (clientnode.nodes != null && clientnode.nodes.length > 0) {
            for (var i = 0; i < clientnode.nodes.length; i++) {
                var node = clientnode.nodes[i];
                if (node.GetCheckState() == 'Checked') {
                    SetCheckedState(node, node.GetCheckState());
                }
                IsNodeChecked(node);
            }
        }
    }

    function SetCheckedState(clientnode, state) {
        if (clientnode.parent == null) {
            return;
        }
        var parentState = 'Checked';
        if (state == 'Checked') {
            if (clientnode.parent != null && clientnode.parent.nodes.length > 1) {
                for (var i = 0; i < clientnode.parent.nodes.length; i++) {
                    if (!clientnode.parent.nodes[i].GetChecked()) {
                        parentState = 'Indeterminate';
                    }
                }
            }
        }
        else if (state == 'Unchecked') {
            parentState = 'Unchecked';
            if (clientnode.parent != null && clientnode.parent.nodes.length > 1) {
                for (var i = 0; i < clientnode.parent.nodes.length; i++) {
                    if (clientnode.parent.nodes[i].GetChecked()) {
                        parentState = 'Indeterminate';
                    }
                }
            }
        }
        else {
            parentState = 'Indeterminate';
        }
        clientnode.parent.SetCheckState(parentState);

        if (clientnode.parent != null) {
            SetCheckedState(clientnode.parent, clientnode.parent.GetCheckState());
        }
    }
</script>

<div>
    <asp:Label Text="" runat="server" ID="lblmessage" />
    <table style="width: 100%;" runat="server" id="tbPredecessors">
        <tr>
            <td colspan="2">
                <span>
                    <a href="javascript:tvTasks.ExpandAll();">
                        <img src="/Content/images/expand-all.png" style="cursor: pointer;" class="" alt="expandall">
                    </a>
                </span>
                <span>
                    <a href="javascript:tvTasks.CollapseAll();">
                        <img src="/Content/images/collapse-all.png" style="cursor: pointer;" class="" alt="collapseall">
                    </a>
                </span>
                <div class="btn-lable-wrap">
                    <span class="taskLabel">Use buttons below to add/remove predecessors</span>
                </div>

            </td>
        </tr>
        <tr>
            <td class="predecessors-wrap">
                <div style="display: block; max-height: 150px; overflow-y: auto;">

                    <dx:ASPxTreeView ID="tvTasks" runat="server" TextField="Title" ShowExpandButtons="true"
                        ShowTreeLines="true" AllowSelectNode="false" Width="100%"
                        AllowCheckNodes="true" ClientInstanceName="tvTasks" CssClass="tvpredecessorstasks all-checkbox checkbox chkbox-1">
                        <ClientSideEvents CheckedChanged="OnCheckedChanged" />
                    </dx:ASPxTreeView>
                </div>
            </td>
            <td style="vertical-align: top; width: 0%; display: none">
                <asp:HiddenField ID="hdnPred" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnPredIds" runat="server" />
            </td>
        </tr>
    </table>
</div>




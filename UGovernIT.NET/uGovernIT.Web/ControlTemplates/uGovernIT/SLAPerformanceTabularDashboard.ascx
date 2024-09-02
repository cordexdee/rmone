<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SLAPerformanceTabularDashboard.ascx.cs" Inherits="uGovernIT.Web.SLAPerformanceTabularDashboard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .tablecss {
    width:100%;
    }
    .tablecss td,
    .tablecss th {
        border: 1px solid;
        height: 25px;
    }
    .dropdown {
        width: 235px;
        margin-bottom: 9px;
    }
     .module {
        float: left;
        padding-right: 3px;
    }
</style>

<table style="width:100%">
  <%--  <tr>
        <td><b class="module">Module: </b>
            <asp:DropDownList ID="ddlModule" CssClass="dropdown" runat="server" AppendDataBoundItems="true" onchange="loadingPanel.Show();" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"></asp:DropDownList></td>
    </tr>--%>
    <tr>
        <td>
            <div style="float: left;width:100%">
                <asp:Repeater ID="rptSLAParent" runat="server">
                    <HeaderTemplate>
                        <table id="tblmain" class="tablecss" cellspacing="0" cellpadding="0">
                            <tr class="titleclass">
                                <th scope="col" style="font-weight: bold; text-align: center; padding-left: 11px;">Activity</th>
                                <th scope="col" style="width: 75px !important; font-weight: bold; text-align: center; background-color: #39CB71">Target (Bus&nbsp;Days)</th>
                                <th style="width: 73px !important; text-align: center; font-weight: bold; padding-left: 11px; background-color: #43E9E9">Actual (Bus&nbsp;Days) </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: center;padding-left:3px;padding-right:3px;font-weight:normal!important">
                                <asp:Label ID="lblRuleName" runat="server" Text='<%# Eval("Title") %>' /></td>
                            <td style="text-align: center; background-color: #39CB71;font-weight:normal!important">
                                <asp:Label ID="lblSLATargetX2" runat="server" Text='<%# Eval("SLATargetX2") %>' /></td>
                            <td style="text-align: center; background-color: #43E9E9;font-weight:normal!important">
                                <asp:Label ID="lblSLAActualX2" runat="server" Text='<%# Eval("SLAActualX2") %>' /></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>   
                    </FooterTemplate>
                </asp:Repeater>

            </div>
        </td>
    </tr>
</table>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." ClientInstanceName="loadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>

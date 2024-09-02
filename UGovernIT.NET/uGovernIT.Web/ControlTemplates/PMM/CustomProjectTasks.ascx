
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomProjectTasks.ascx.cs" Inherits="uGovernIT.Web.CustomProjectTasks" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        var tab = projecttaskTab.GetTabByName("tasklist");
        projecttaskTab.SetActiveTab(tab);
        actionOnTabClick("", tab.index, tab.index);

    });

    function FrameLoadOnDemand(control) {
        try {
            control.contentWindow.clickUpdateSize();
            control.contentWindow.adjustControlSize();
        } catch (ex) {
        }
    }

    function actionOnTabClick(actionType, tabIndex, activeTabIndex) {
        var clickTab = projecttaskTab.GetTab(tabIndex).name;

        var activeTab = projecttaskTab.GetTab(activeTabIndex).name;
        var tab = projecttaskTab.GetTabByName(activeTab);

        if (activeTab == "tasklist") {
            $("#<%=trPanelTask.ClientID%>").show();
            $("#<%=trPanelScrum.ClientID%>").hide();

            var frames = $($("#<%=trPanelTask.ClientID%>")).find("iframe");
            FrameLoadOnDemand(frames[0]);

        }
        else {
            $("#<%=trPanelScrum.ClientID%>").show();
            $("#<%=trPanelTask.ClientID%>").hide();

            var frames = $($("#<%=trPanelScrum.ClientID%>")).find("iframe");
            FrameLoadOnDemand(frames[0]);
        }
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function AutoAdjustActiveTabContainer() {
        var frames = $($("#<%=trPanelTask.ClientID%>")).find("iframe");
        var totalHeight = $(window).height();
        var containerHeight = (totalHeight - 225)
        frames[0].height = containerHeight;
    }
</script>

<table cellpadding="0" cellspacing="0" border="0" class="width100 bordercolps" width="100%">
    <tr id="cModuleDetailPanel" runat="server">
        <td style="padding-top: 3px;">
            <table class="width100 bordercolps" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="moduleimgtd" id="cModuleImgPanel" runat="server" valign="top" style="padding-bottom: 8px;">
                        <asp:Image runat="server" ID="moduleLogo" ImageUrl="/Content/images/ProjectPlan.png" Width="32" Height="32" />
                    </td>
                    <td valign="middle">
                        <table cellpadding="0" cellspacing="0" border="0" class="width100 bordercolps" width="100%">
                            <tr id="cModuleDescriptionPanel" runat="server">
                                <td class="moduledesciptiontd" style="font-size: 12px; font-weight: bold;" valign="top">
                                    <asp:Literal runat="server" ID="moduleDescription" Text=""></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<table cellpadding="0" cellspacing="0" border="0" width="100%" height="100%">
    <tr>
        <td style="border: 1px inset lightgray; padding: 10px 5px 8px 5px;">
            <div style="float: left; padding-left: 5px;">
                <asp:Label ID="lblModule" Text="Module: " runat="server" Style="font-weight: bold; vertical-align: middle;"></asp:Label>
            </div>
            <div style="width: 170px; float: left; margin: -2px 0 0 5px; vertical-align: middle;">
                <dx:ASPxComboBox ID="cmbModuleList" runat="server" Width="100%" AutoPostBack="true" ClientInstanceName="cmbModuleList" ValueField="ModuleName"
                    ValueType="System.String" TextField="Title" OnSelectedIndexChanged="cmbModuleList_SelectedIndexChanged">
                </dx:ASPxComboBox>
            </div>
            <div style="float: left; padding-left: 30px;">
                <asp:Label ID="lblProject" runat="server" Text="Project: " Style="font-weight: bold; vertical-align: middle;"></asp:Label>
            </div>
            <div style="width: 120px; float: left; margin: -2px 0 0 5px; vertical-align: middle;">
                <dx:ASPxComboBox ID="cmbProject" runat="server" Width="100%" AutoPostBack="true" ClientInstanceName="cmbProject" ValueField="TicketId"
                    EnableCallbackMode="true" TextFormatString="{0}" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="cmbProject_SelectedIndexChanged"
                    DropDownStyle="DropDownList" EnableSynchronization="True">
                    <Columns>
                        <dx:ListBoxColumn Caption="Project ID" FieldName="TicketId" Width="90px" />
                        <dx:ListBoxColumn Caption="Title" FieldName="Title" Width="320px" />
                    </Columns>
                </dx:ASPxComboBox>
            </div>
            <div style="float: left; padding-left: 30px;">
                <asp:Label ID="lblProjectName" runat="server" Text="Title: " Style="font-weight: bold;"></asp:Label>
            </div>
            <div style="width: 300px; float: left; margin: -2px 0 0 5px;">
                <a id="aProjectTitle" runat="server" href="" style="cursor: pointer; vertical-align: middle;" />
            </div>
            <div style="float: right;">
                <div style="float: left; padding-left: 30px;">
                    <asp:Label ID="lblStatus" runat="server" Text="Status: " Style="font-weight: bold;"></asp:Label>
                </div>
                <div style="width: 100px; float: left; margin: -2px 0 0 5px;">
                    <asp:Label ID="lblProjectStatus" runat="server" Style="padding-left: 4px; padding-right: 4px; vertical-align: middle;"></asp:Label>
                </div>
            </div>
        </td>
    </tr>

    <tr>
        <td>
            <table id="myhometabs" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse; margin-top: 10px; margin-left: 5px; margin-bottom: 10px; width: 100%; height: 100%;">
                <tr>
                    <td>
                        <dx:ASPxTabControl ID="projecttaskTab" ClientInstanceName="projecttaskTab" runat="server" ActiveTabIndex="1" Style="margin-bottom: 4px;">
                            <Tabs>
                                <dx:Tab Text="Task List" Name="tasklist" Visible="false" />
                                <dx:Tab Text="Scrum" Name="scrum" Visible="false" />
                            </Tabs>

                            <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
                            <ClientSideEvents ActiveTabChanged="function(s,e){ actionOnTabClick('change', e.tab.index, s.activeTabIndex);}" TabClick="function(s,e){ actionOnTabClick('click', e.tab.index, s.activeTabIndex);}" />
                        </dx:ASPxTabControl>
                    </td>
                </tr>

                <tr id="trPanelTask" runat="server" style="display: none;">
                    <td>
                        <asp:Panel ID="panelTaskList" runat="server">
                        </asp:Panel>
                    </td>
                </tr>

                <tr id="trPanelScrum" runat="server" style="display: none;">
                    <td>
                        <asp:Panel ID="panelSprint" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>

</table>

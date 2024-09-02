<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITBudgetUserControl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.Budget.uGovernITBudgetUserControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .tabmaindiv {
        float: left;
        width: 100%;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(
        function () {
            actionOnTabClick('click', 0, 0);
        }
    );

    function actionOnTabClick(actionType, tabIndex, activeTabIndex) {
        //debugger;
        var url = location.href;
        var previousurl = document.referrer;
        var arr = url.split('?');
        if (url.length > 1 && arr[1] != 'undefined') {
            localStorage.setItem("myindex", -1);
        }

        var clickTab = tbBudgetTabs.GetTab(tabIndex).name;
        var activeTab = tbBudgetTabs.GetTab(activeTabIndex).name;

        if (clickTab == "ITGPortfolio") {

            var site = "<%=ITGPortfolioURL%>";
            document.getElementsByName('iframeTab')[0].src = site;
        }
        else if (clickTab == "ITGBudgetManagement") {
            var site = "<%=ITGBudgetManagementURL%>";
            document.getElementsByName('iframeTab')[0].src = site;
        }
        else if (clickTab == "ITGBudgetEditor") {
            var site = "<%=ITGBudgetEditorURL%>"
            document.getElementsByName('iframeTab')[0].src = site;
        }
        else if (clickTab == "GovernanceReview") {
            var site = "<%=GovernanceReviewURL%>"
            document.getElementsByName('iframeTab')[0].src = site;
        }
    }
</script>

<div id="pnlBudgetMain" class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
    <div class="row">
        <div class="tabmaindiv row">
            <div class="col-md-10 noSidePadding">
                <dx:ASPxTabControl ID="tbBudgetTabs" ClientInstanceName="tbBudgetTabs" runat="server" Visible="true" CssClass="rmm-tabWrap">
                    <ClientSideEvents ActiveTabChanged="function(s,e){ actionOnTabClick('change', e.tab.index, s.activeTabIndex);}"
                        TabClick="function(s,e){ actionOnTabClick('click', e.tab.index, s.activeTabIndex);}" />
                    <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
                    <Tabs>
                        <dx:Tab Name="ITGPortfolio" Text="Project Portfolio"></dx:Tab>
                        <dx:Tab Name="ITGBudgetManagement" Text="Budget Categories"></dx:Tab>
                        <dx:Tab Name="ITGBudgetEditor" Text="Non-Project Budget"></dx:Tab>
                        <dx:Tab Name="GovernanceReview" Text="Pending Review"></dx:Tab>
                    </Tabs>
                </dx:ASPxTabControl>
            </div>

            <div id="divTabContent" style="padding: 5px 5px 5px 5px">
                <dx:ASPxPanel ID="pnlTabContent" runat="server" Height="650px">
                    <PanelCollection>
                        <dx:PanelContent>

                            <iframe name="iframeTab" onload='callAfterFrameLoad(this)' width='100%' height="650px" frameborder='0' style="display: block; height: 650px;"></iframe>

                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </div>
        </div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdministerCore.ascx.cs" Inherits="uGovernIT.Web.AdministerCore" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .allowPointerEvent {
        pointer-events: auto !important;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var disabledModules = '<%=DisabledAdminModules%>';  
    var moduleArr = disabledModules.split(',');

    $(document).ready(function () {
        $('.outerWrap').find('a').each(function () {
            //console.log($(this).attr('href'));
            for (index = 0; index < moduleArr.length; index++) {
                //console.log(moduleArr[index]);
                if ($(this).attr('Id').indexOf(moduleArr[index]) > 0) {
                    $(this).parent().addClass('disableModule').addClass('allowPointerEvent');
                }
            }
        });

        var url = window.location.href;
        url = url.split('?');
        if (url != null && url.length == 2) {
            var istrailuserdir = url[1];
            if (istrailuserdir == "svc") {
                window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services', '', 'Services & Agents', '90', '90', '', true);
            }
            var newUrl = url[0];
            history.pushState({}, null, newUrl);

        }

        var IsDisableReportControl = $('#<%=a2.ClientID%>').parent().hasClass("disableModule");
        if (IsDisableReportControl) {
            $('#<%=a2.ClientID%>').removeAttr('onclick');
            $('#<%=configureUser.ClientID%>').removeAttr('onclick');
        }
        else {
            $('#<%=a2.ClientID%>').removeAttr('href');
            $('#<%=configureUser.ClientID%>').removeAttr('onclick');
        }

        var IsDisableServiceCatalog = $('#<%=serviceCatelogAndAgent.ClientID%>').parent().hasClass("disableModule");
        if (IsDisableServiceCatalog) {
            $('#<%=serviceCatelogAndAgent.ClientID%>').removeAttr('onclick');
        }
        else {
            $('#<%=serviceCatelogAndAgent.ClientID%>').removeAttr('href');
        }
    });

    $(document).on("click", ".disableModule", function () {
        var answer = window.confirm("If you would like to enable these features, please click OK and one of our CORE RMM administrators will reach out to you.");
        if (answer) {

        }
        else {
            return false;
        }
    });
</script>

<div class="adminPage-container">
    <div class="backAdmin-btnWrap">
        <i class="fas fa-angle-left backtoAdmin" id="backAdminBtn" runat="server" visible="false"></i>
        <asp:Button ID="btnBackButton" Text="Return to Administrator" runat="server" OnClick="btnBackButton_Click" Visible="false" CssClass="backToAdmin-btn" />
    </div>
    <%-- Admin core page --%>
    <div id="divContainer" runat="server">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 adminService-headingWrap">
                <%--<h3 class="adminService-heading">Administer Service Prime</h3>--%>
            </div>
            <%--<div class="adminService-btnWrap col-md-12 col-xs-12 col-sm-12">
                <button ID="adminServiceBtn" Text="On-Board Customer" Class="adminService-btn">On-Board Customer</button>
            </div>--%>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12 workflow-container">
                <div class="outerWrap">
                    <div class="outerImg-wrap img-wrap1" runat="server" onclick="coresetup()">
                        <a id="acoreSetUp" href="javascript:;" runat="server" onserverclick="acoreSetUp_ServerClick">
                            <img class="outer-img rotate-pos" src="../Content/Images/initial-setup.png">
                            <%--<asp:ImageButton ID="coreSetUp" CssClass="outer-img1" AlternateText="Core Set UP" ImageUrl="~/Content/Images/initial-setup.png"  runat="server" />--%>

                            <h4 class="step-tile">Initial Setup</h4>
                            <p class="step-description">Initial Setup Of <%=DisplayName%></p>  
                            <div class="innerDot dot1"></div>
                        </a>
                    </div>
                    <div class="outerImg-wrap img-wrap2">
                        <a id="aworkflow" href="javascript:;" runat="server" onserverclick="aworkflow_ServerClick">
                            <img class="outer-img" src="../Content/Images/workflowAdmin.png">
                            <%--<asp:ImageButton ID="workflow" CssClass="outer-img" AlternateText="WorkFlows"  ImageUrl="~/Content/Images/workflowAdmin.png"  runat="server" />--%>

                            <h4 class="step-tile">Workflows</h4>
                            <p class="step-description">Configure & Manages Services</p>
                        </a>
                        <div class="innerDot dot2"></div>
                    </div>
                    <div class="outerImg-wrap img-wrap3">
                        <a id="aKnowledge" href="javascript:;" runat="server" onserverclick="aKnowledge_ServerClick">
                            <img class="outer-img" src="../Content/Images/Knowledge.png">
                            <%-- <asp:ImageButton ID="knowledge" CssClass="outer-img" AlternateText="Knowledge"  ImageUrl="~/Content/Images/Knowledge.png"  runat="server" />--%>
                            <h4 class="step-tile">Knowledge</h4>
                            <p class="step-description">Manage Knowledge Base (Documents, Wikis)</p>
                        </a>
                        <div class="innerDot dot3"></div>
                    </div>
                    <div class="outerImg-wrap img-wrap4">
                        <a id="asystemResource" href="javascript:;" runat="server" onserverclick="asystemResource_ServerClick">
                            <img class="outer-img" src="../Content/Images/system.png">
                            <%--<asp:ImageButton ID="systemResource" CssClass="outer-img" AlternateText="System Resource"  ImageUrl="~/Content/Images/system.png" OnClick="systemResource_Click" runat="server" />--%>

                            <h4 class="step-tile">System</h4>
                            <p class="step-description">Configure and Manage System Resources</p>
                        </a>
                        <div class="innerDot dot4"></div>
                    </div>
                    <div class="outerImg-wrap img-wrap5">
                        <a id="a2" href="javascript:;" runat="server" onserverclick="a2_ServerClick" onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboard','','Report','95','95');return false;">
                            <asp:ImageButton ID="configureUser" CssClass="outer-img" OnClick="a2_ServerClick" AlternateText="Dashboard And Reports" ImageUrl="~/Content/Images/Reports.png" runat="server" OnClientClick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboard','','Report','95','95');return false;" />

                            <h4 class="step-tile">Reports</h4>
                            <p class="step-description">Create and Update Analytics, Dashboards and Reports</p>
                            <div class="innerDot dot5"></div>
                        </a>
                    </div>
                    <div class="outerImg-wrap img-wrap6">

                        <a id="amanageInterface" href="javascript:;" runat="server" onserverclick="amanageInterface_ServerClick">
                            <img class="outer-img" src="../Content/Images/Interface.png">
                            <%-- <asp:ImageButton ID="manageInterface" CssClass="outer-img" AlternateText="Manage Interface"  ImageUrl="~/Content/Images/Interface.png"  runat="server" />--%>

                            <h4 class="step-tile">Interfaces</h4>
                            <p class="step-description">QuickBooks, Dynamics</p>
                            <div class="innerDot dot6"></div>
                        </a>
                    </div>
                    <div class="outerImg-wrap img-wrap7">
                        <a id="atemplate" href="javascript:;" runat="server" onserverclick="atemplate_ServerClick">
                            <img class="outer-img" src="../Content/Images/Templates.png">
                            <%-- <asp:ImageButton ID="resourceConfig" CssClass="outer-img" AlternateText="System Resource"  ImageUrl="~/Content/Images/ResourceAdmin.png"  runat="server" />--%>

                            <h4 class="step-tile">Templates</h4>
                            <p class="step-description">Build and update agents, checklist, macros, tasks</p>
                        </a>
                        <div class="innerDot dot7"></div>
                    </div>
                    <div class="outerImg-wrap img-wrap8">

                        <a id="agovernance" href="javascript:;" runat="server" onserverclick="agovernance_ServerClick">
                            <img class="outer-img" src="../Content/Images/governanceAdmin.png">
                            <%-- <asp:ImageButton ID="template" CssClass="outer-img" AlternateText="Template"  ImageUrl="~/Content/Images/Templates.png"  runat="server" />--%>

                            <h4 class="step-tile">Governance</h4>
                            <p class="step-description">Create and define the processes</p>
                            <div class="innerDot dot8"></div>
                        </a>
                    </div>
                    <div class="outerImg-wrap img-wrap9">
                        <a id="serviceCatelogAndAgent" href="javascript:;" runat="server" onserverclick="serviceCatelogAndAgent_ServerClick" onclick="UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services','','Services & Agents','90','90',false,'',false);return false;">
                            <img class="outer-img" src="../Content/Images/service-catloge-and-agents.png">
                            <%--<asp:ImageButton ID="governance" CssClass="outer-img" AlternateText="Governance"  ImageUrl="~/Content/Images/governanceAdmin.png"  runat="server" />--%>

                            <h4 class="step-tile">Service Catalog & Agents</h4>
                            <p class="step-description">Configure services and agents.</p>
                            <div class="innerDot dot9"></div>
                        </a>
                    </div>
                    <div class="outerImg-wrap img-wrap10">
                        <a id="aresourceConfig" href="javascript:;" runat="server" onserverclick="aresourceConfig_ServerClick">
                            <img class="outer-img" src="../Content/Images/ResourceAdmin.png">
                            <%--<asp:ImageButton ID="serviceCatelogAndAgent" CssClass="outer-img" runat="server" ImageUrl="~/Content/Images/service-catloge-and-agents.png" OnClientClick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=services','','Services & Agents','90','90',false,'',false);return false;" />--%>
                            <h4 class="step-tile">User Management</h4>
                            <p class="step-description">Configure and manage users.</p>
                            <div class="innerDot dot10"></div>
                        </a>
                    </div>
                    <div class="inner-imgWrap">
                        <img class="innerImg" src="/Content/Images/administer.png">
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Panel ID="admincorePanel" runat="server"></asp:Panel>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrialUser.ascx.cs" Inherits="uGovernIT.Web.TrialUser" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function acoreSetUp(s,e) {
        //alert("This option is not configured for you");
         var message = "This option is not configured for you";
             UgitDialogBox(message, 'Helpdesk', '', 0);
    }

    function aworkflow(s,e) {
        //alert("This option is not configured for you");
         var message = "This option is not configured for you";
             UgitDialogBox(message, 'Projects', '', 0);
    }

    function aKnowledge(s,e) {
        //alert("This option is not configured for you");
         var message = "This option is not configured for you";
             UgitDialogBox(message, 'IT Staff', '', 0);
    }

    function asystemResource(s,e) {
        //alert("This option is not configured for you");
         var message = "This option is not configured for you";
             UgitDialogBox(message, 'Portfolios', '', 0);
    }

    function a2(s,e) {
        //alert("This option is not configured for you");
         var message = "This option is not configured for you";
             UgitDialogBox(message, 'Dashboards', '', 0);
    }

    function amanageInterface(s,e) {
        //alert("This option is not configured for you");
         var message = "This option is not configured for you";
             UgitDialogBox(message, 'Analytics', '', 0);
    }

</script>


<div>
            <div class="col-md-12 col-sm-12 col-xs-12" id="divContainer" runat="server">
        <div class="row">
            <div class="adminService-headingWrap">
<%--                <h3 class="adminService-heading">Trial User</h3>--%>
            </div>
            <%--<div class="adminService-btnWrap col-md-12 col-xs-12 col-sm-12">
                <button ID="adminServiceBtn" Text="On-Board Customer" Class="adminService-btn">On-Board Customer</button>
            </div>--%>
        </div>
	    <div class="row">
		    <div class="workflow-container">
			    <div class="outerWrap">
				    <div class="outerImg-wrap img-wrap1">
				        <a id="acoreSetUp" onclick="acoreSetUp()">
                             <img class="outer-img" src="/Content/Images/NewAdmin/hepldesk.png">
                        <%--<asp:ImageButton ID="coreSetUp" CssClass="outer-img1" AlternateText="Core Set UP" ImageUrl="~/Content/Images/initial-setup.png"  runat="server" />--%>
					
                         <h4 class="step-tile">Helpdesk</h4>
					    <p class="step-description">Trial our ticketing system</p>
					    <div class="innerDot user-dot1"></div>
                        </a>
				    </div>
				    <div class="outerImg-wrap userImg-wrap2">
					     <a id="aworkflow" onclick="aworkflow()">
                             <img class="outer-img" src="/Content/Images/NewAdmin/project.png">
                        <%--<asp:ImageButton ID="workflow" CssClass="outer-img" AlternateText="WorkFlows"  ImageUrl="~/Content/Images/workflowAdmin.png"  runat="server" />--%>

					    <h4 class="step-tile">Projects</h4>
					    <p class="step-description">Trial Our project management system</p>
                        </a>
					    <div class="innerDot user-dot2"></div>
				    </div>
				    <div class="outerImg-wrap userImg-wrap3">	
					    <a id="aKnowledge" onclick="aKnowledge()">
                             <img class="outer-img" src="/Content/Images/NewAdmin/it-staff.png">
                       <%-- <asp:ImageButton ID="knowledge" CssClass="outer-img" AlternateText="Knowledge"  ImageUrl="~/Content/Images/Knowledge.png"  runat="server" />--%>
					    <h4 class="step-tile">IT Staff</h4>
					    <p class="step-description">Manage people and connect them to projects</p>
                        </a>
					    <div class="innerDot user-sdot3"></div>
				    </div>
				    <div class="outerImg-wrap userImg-wrap4">	
					    <a id="asystemResource" onclick="asystemResource()">
                        <img class="outer-img" src="/Content/Images/NewAdmin/portfolios.png">
                            <%--<asp:ImageButton ID="systemResource" CssClass="outer-img" AlternateText="System Resource"  ImageUrl="~/Content/Images/system.png" OnClick="systemResource_Click" runat="server" />--%>

					    <h4 class="step-tile">Portfolios</h4>
					    <p class="step-description">Manage project portfolios</p>
                        </a>
					    <div class="innerDot user-dot4"></div>
				    </div>
				    <div class="outerImg-wrap useIimg-wrap5">	
					    <a id="a2" onclick="a2()">
                        <asp:ImageButton ID="configureUser" CssClass="outer-img" AlternateText="Dashboard And Reports"  ImageUrl="/Content/Images/NewAdmin/dashboard.png"  runat="server"  OnClientClick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configdashboard','','Report','95','95');return false;"/>
                   
					    <h4 class="step-tile">Dashboards</h4>
					    <p class="step-description">Try out our Dashboards and Reports</p>
					    <div class="innerDot user-dot5"></div>
                        </a>
				    </div>
				    <div class="outerImg-wrap userImg-wrap6">	
					
                        <a id="amanageInterface"  onclick="amanageInterface()">
                            <img class="outer-img" src="/Content/Images/NewAdmin/analytics.png">
                       <%-- <asp:ImageButton ID="manageInterface" CssClass="outer-img" AlternateText="Manage Interface"  ImageUrl="~/Content/Images/Interface.png"  runat="server" />--%>

					    <h4 class="step-tile">Analytics</h4>
					    <p class="step-description">Try out our Score carding system</p>
					    <div class="innerDot user-dot6"></div>
                        </a>
				    </div>
				    <div class="inner-imgWrap">
					    <img class="trialUser-innerImg" src="/Content/Images/NewAdmin/user-register-workflow.png">
				    </div> 
			    </div>
		    </div>
	    </div>
    </div>
        </div>
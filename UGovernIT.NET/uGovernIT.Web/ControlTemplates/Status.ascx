<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Status.ascx.cs" Inherits="uGovernIT.Web.Status" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<label runat="server" id="qwa"></label>
<asp:TextBox ID="txtShortName" runat="server" />--%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
.ml166{
	left : 166px 
}
</style>

<div class="col-md-12 col-sm-12 col-xs-12 popupMainContainer-status">
    <ul class="statusModule-list">
			<li class="statusList" id="lstRegistration" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/Registration.png">
					</div>
					<div class="statusVr-line">
						<span runat ="server" id="registration"  class="statusDot"></span>
					</div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Registration</h5>
						<p class="statusModule-discription">registered and ready to start using Service Prime.</p>
					</div>
				<%--</a>--%>
			</li>
			<li class="statusList" id="lstManageIncidents" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/mange.png">
					</div>
                    <div runat ="server" id="dvmanageIncident" class="statusCount-wrap">8524</div>
					<div class="statusVr-line">
						<span runat ="server" id="manageIncident" class="statusDot status-dot1"></span>
					</div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Manage Incidents</h5>
						<p class="statusModule-discription">Used Service Prime to manage incidents.</p>
					</div>
				<%--</a>--%>
			</li>
			<li class="statusList"  id="lstAddUser" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/Added-Users.png">
					</div>
                    <div runat ="server" id="dvAddedUser" class="statusCount-wrap">8524</div>
					<div class="">
						<span runat ="server" id="AddedUser" class="statusDot ml166"></span>
					</div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Added Users</h5>
						<p class="statusModule-discription">Added other users to use Service Prime.</p>
					</div>
				<%--</a>  --%>  
			</li>
        <li class="statusList" id="lstUserCreated" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/User-Created.png">
					</div>
                    <div runat ="server" id="dvusercreatedincident" class="statusCount-wrap">8524</div>
					<div class="statusVr-line">
						<span runat ="server" id="usercreatedincident" class="statusDot status-dot3"></span>
					</div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">User Created Incidents</h5>
						<p class="statusModule-discription">End users created incidents/ Tickets.</p>
					</div>
				<%--</a>   --%> 
			</li>
			<li class="statusList" id="lstUserService" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/Used-Services.png">
					</div>
                    <div runat="server" id="dvUsedSvc" class="statusCount-wrap">8524</div>
					<div class=""><span runat ="server" id="UsedSvc" class="statusDot ml166"></span></div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Used Services</h5>
						<p class="statusModule-discription">Used prebuild services.</p>
					</div>
				<%--</a>--%>
			</li>
		<li class="statusList" id="lstDepartment" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/departmentIcon.png">
					</div>
                    <div runat="server" id="dvDepartment" class="statusCount-wrap">8524</div>
					<div class=""><span runat ="server" id="Department" class="statusDot ml166"></span></div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Department</h5>
						<p class="statusModule-discription">Added department to use Service Prime.</p>
					</div>
				<%--</a>--%>
			</li>
		<li class="statusList" id="lstTitle"  runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/titleIcon.png">
					</div>
                    <div runat="server" id="dvTitle" class="statusCount-wrap">8524</div>
					<div class=""><span runat ="server" id="Title" class="statusDot ml166"></span></div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Titles</h5>
						<p class="statusModule-discription">Added title to use Service Prime.</p>
					</div>
				<%--</a>--%>
			</li>
		<li class="statusList" id="lstRole" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/rolesIcon.png">
					</div>
                    <div runat="server" id="dvRole" class="statusCount-wrap">8524</div>
					<div class=""><span runat ="server" id="Role" class="statusDot ml166"></span></div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Roles</h5>
						<p class="statusModule-discription">Added Roles to use Service Prime.</p>
					</div>
				<%--</a>--%>
			</li>
		<li class="statusList" id="lstProject" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/projectIcon.png">
					</div>
                    <div runat="server" id="dvProject" class="statusCount-wrap">8524</div>
					<div class=""><span runat ="server" id="Project" class="statusDot ml166"></span></div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Projects</h5>
						<p class="statusModule-discription">Created Projects</p>
					</div>
				<%--</a>--%>
			</li>
		<li class="statusList" id="lstAllocation" runat="server">
				<%--<a href="#">--%>
					<div class="status-image">
						<img src="/content/images/NewAdmin/allocationsIcon.png">
					</div>
                    <div runat="server" id="dvAllocation" class="statusCount-wrap">8524</div>
					<div class=""><span runat ="server" id="Allocation" class="statusDot ml166"></span></div>
					<div class="statusModule-content">
						<h5 class="statusModule-name">Allocation</h5>
						<p class="statusModule-discription">Added allocation to use Service Prime.</p>
					</div>
				<%--</a>--%>
			</li>
		</ul>
</div>
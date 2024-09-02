<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="defaultadmin.ascx.cs" Inherits="uGovernIT.Web.defaultadmin" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style>

</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

   function OnItemClick() {
       window.location.href = "<%= newTask %>";
       localStorage.removeItem("myindex");
    }

    function OnItemClickTicket() {
        window.parent.UgitOpenPopupDialog('/Pages/tsr', 'TicketId=0', 'New Service Prime Ticketing System', '90', '90', 0, '%2fPages%2fTSRTickets');
    }

    function OnItemClickTask() {
        window.parent.UgitOpenPopupDialog('/Pages/tsk', 'TicketId=0', 'New Tasklist', '90', '90', 0, '%2fPages%2fTSRTickets');
    }
    
    $(document).ready(function () {
        $('.adminDefault-pageWrap').parents().eq(2).addClass('adminDefault-container');
        $('.adminDefault-container').parents().eq(1).addClass('adminDefault-Maincontainer')
    });

    function redirectToGuideMe() {
        window.location = "/SitePages/GuideMe"
    }
</script>
<%--<asp:Button runat="server" id="dontShowAgain" Text="Dont Show" OnClick="dontShowAgain_Click"/>--%>
<div class="row adminDefault-pageWrap">
    <section class="mainSection">
       <div class="mainSection-container mainSection-bgBanner">
           <div class="mainSection-wrap container">
               <div class="mainSection-textBlock col-md-6 col-sm-6 col-xs-12 noPadding">
                   <h2>Welcome to Service Prime for <%=TenantName %> </h2>
                   <p>You are setup as an Admin User. This site is exclusively setup for use by '<%=TenantName %>'.
                       The panel on the left lets you add users, create and manage IT support tickets and 
                       create services etc.
                   </p>
                   <p class="lastPara">We recommend you add a few users who will be part of trying Service Prime.</p>

                   <div class="admindflt-checkWrap">
                       <asp:CheckBox ID="skiplogin" AutoPostBack="true" runat="server" Text="Skip this message next time I login in" ToolTip="Skip this message next time I login in" OnCheckedChanged="skiplogin_CheckedChanged"/>
                   </div>
                   <div class="continue-btnWrap">
                       <a runat="server" class="btn secondaryHome-btn" onclick="OnItemClick()">
                           <img src="/Content/Images/TickBlack.png" width="18" style="margin:0px 10px 0px 0px;"/>ADD User
                       </a>
                       <a runat="server" class="btn secondaryHome-btn addTicket-homeBTn" onclick="OnItemClickTicket()">
                            <img src="/Content/Images/AddusersBlack.png" width="18" style="margin:0px 10px 0px 0px;"/>Add ticket
                       </a>
                       <a  class="btn secondaryHome-btn addTicket-homeBTn" onclick="OnItemClickTask()">
                            <img src="/Content/Images/homeBlack.png" width="18" style="margin:0px 10px 0px 0px;"/>Add Task
                       </a>
                   </div>
                   
               </div>
               <div class="mainSection-ImageBlock col-md-6 col-sm-6 col-xs-12 noPadding" onclick="redirectToGuideMe()">
                   <img src="/Content/images/NewAdmin/Complete-Solution.svg" class="section-bannerImg" />
                   <a href="#" class="btn continue-btn">                       
                        <img src="/Content/Images/guide_Me.png" width="20" style="margin:0px 10px 0px 0px;" />Guide me
                    </a>
               </div>
           </div>
       </div>
    </section>
</div>
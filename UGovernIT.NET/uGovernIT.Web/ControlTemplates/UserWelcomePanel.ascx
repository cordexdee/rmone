<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserWelcomePanel.ascx.cs" Inherits="uGovernIT.Web.UserWelcomePanel" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style>
   
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var globalheight = '<%=Height%>';
    var width = '<%=Width%>';
    function truncateString(str, num) {
        if (str.length > num) {
            return str.slice(0, num) + "...";
        } else {
            return str;
        }
    }
    $.ajax({
        type: "GET",
        url: "/api/CoreRMM/GetUserWelcomeMessage?ViewID=" + '<%=viewId%>',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {
            $("#userWelcomeMessage").html(message.WelcomeMessage);
            $("#userInfoMessage").html(message.userInfoMessage);
            $("#userDisplayImage").attr("src", message.img);
            if (message.HideMoreIcon)
                $(".moreIconDiv").hide();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
        }
    });

    function openCriticalTasks() {
        window.parent.UgitOpenPopupDialog('<%=MoreLinkUrl%>', '', 'My Items', '900', '900', 0, "", true, true);
    }

    function openUserProfile() {
        window.parent.UgitOpenPopupDialog('<%=userLinkUrl%>', '', 'User Profile', '900', '900', 0, "", true, true);
    }

    function openTasklink(taskid, modulename, ticketid, title) {
        
        var params = "moduleName=" + modulename + "&TaskId=" + taskid + "&projectID=" + ticketid + "&viewType=1";
        window.parent.parent.UgitOpenPopupDialog('<%=editTaskFormUrl %>', params, title, '90', '90', false, "");
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .moreIconDiv{
        z-index: 99;
    color: #4A6EE2;
    font-size: 18px;
    width: 26px;
    text-align: center;
    }
    .rmmHome .userWelc {
    padding-left: 0px !important;
    overflow-wrap: break-word;
}
    p {
        
        font-size: 14px;
        text-align: center;
        color: grey !important;
    }
    .rmmHome .userImgBox {
        margin-left: auto;
        margin-right: auto;
    }
</style>
<div id="divUserWelcomePanel" runat="server" class="rmmHome text-left overflow-hidden">
    <div class="align-items-center justify-content-between">
        <div>
            <div class="userImgBox">
                <img id="userDisplayImage" class="w-100" alt="User Photo" onclick="openUserProfile()" style="transform:scale(1.5);" />
            </div>
            <div class="userWelc">
                <p id="userWelcomeMessage" class="userWelcName mb-0"></p>
                <p class="userWelcMssg mb-0"><%=WelcomeMessage %></p>
            </div>
        </div>
        <div class="divUserInfoMessage" style="display:none">
            <p id="userInfoMessage" class="mb-0"></p>
        </div>
        <div class="moreIconDiv">
            <a class="moreIcon" href="#" onclick="openCriticalTasks()"><i class="fas fa-angle-double-right"></i></a>
        </div>
    </div>
</div>

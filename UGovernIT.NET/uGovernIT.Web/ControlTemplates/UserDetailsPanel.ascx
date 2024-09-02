<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetailsPanel.ascx.cs" Inherits="uGovernIT.Web.UserDetailsPanel" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #UserDetails .rmm-managerCard {
        box-shadow: 3px 1px 5px rgb(0 0 0 / 20%);
        border: 1px solid #CCC;
        padding: 3px 0;
        margin: 5px;
        height: 130px;
        position: relative;
        border-radius: 6px;
        width: 140px;
    }

    #UserDetails .resource-img-container {
        border-radius: 50%;
        position: relative;
        width: 35px;
        height: 35px;
        margin: 0 auto;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    #UserDetails .user-edit-icon {
        position: absolute;
        top: 16%;
        border: 1.5px solid #092da0;
        border-radius: 50%;
        padding: 2px;
        left: 57%;
        background: #092da0;
        cursor: pointer;
    }

    #UserDetails .user-manager-name {
        color: #444649;
        font-size: 12px;
        margin-top: 4px;
    }

    #UserDetails .card-inner-data span {
        font-size: 10px;
    }

    #UserDetails .card-inner-data {
        font-size: 10px;
    }

    #UserDetails .emptyProgressBar {
        background-color: #fff;
        height: 25px;
        /* border: 1px solid gray; */
        background-size: 100% 100%;
        color: white;
        overflow: hidden;
    }

    #scrollContent table {
        font-size: 14px;
        font-weight: 500;
        width: max-content;
        margin: 5px;
    }

        #scrollContent table td {
            padding: 3px;
            vertical-align:baseline;    
        }
        #scrollContent div {
        font-size: 13px;
        font-weight: 600;
        margin-top: 7px;
        margin-left: 5px;
        text-align:left;
        width: max-content;
    }

    #scrollContent span {
    font-size: 13px;
    font-weight: 500;
    }

    .buttonStyle {
        background: #4fa1d6;
        color: white;
        padding: 4px 10px;
        border-radius: 10px;
        font-size: 14px;
        font-weight: 600;
        margin-top: 5px;
        text-align: center !important;
    }
    .buttonStyle1 {
        background: #57a71d;
        color: white;
        padding: 4px 10px;
        border-radius: 10px;
        font-size: 14px;
        font-weight: 600;
        margin-top: 5px;
        text-align: center !important;
    }
    #UserDetails .moreIcon {
        height: 140px;
        display: inline-flex;
        justify-content: space-around;
        align-items: center;
    }

    #scrollContent {
        height: 140px;
        overflow-x: scroll;
        overflow-y: hidden;
        padding-bottom: 10px;
        display: inline-flex;
        float: left;
        padding-top: 20px;
    }
    #UserDetails .userValue {
    font-weight: 400;
    }
    #UserDetails .marginLeft {
    margin-left: 10px;
    text-align: -webkit-center;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var globalheight = '<%=Height%>';
    var globalWidth = '<%=Width%>';
    var scrollWidth = '<%=Width.Value - 190%>';
    $(document).ready(function () {
        $('#UserDetails #scrollContent').width(scrollWidth);
        $("#UserDetails .users-manager").hide();
        var scrollContent = $('#scrollContent');
        var hasHorizontalScrollbar = scrollContent[0].scrollWidth > scrollContent[0].clientWidth;
        if (!hasHorizontalScrollbar) {
            $('#UserDetails .moreIcon').hide();
        }
    
    $.ajax({
        url: "/api/rmmcard/GetManager?hdnChildOf=<%=this.UserId%>&hdnParentOf=&Year=" + new Date().getFullYear(),
        type: "GET",
        contentType: "application/json",
        async:false,
        success: function (data) {
            
            if (data != null) {
                let html = new Array();

                if (data.PendingApprovalCount > 0)
                    html.push("<div class='timesheet-count' title='Timesheet Pending Approvals' onclick=event.cancelBubble=true;PendingTimesheetApprovals('" + data.ID + "','" + data.Title + "'); >" + data.PendingApprovalCount + "</div>");

                html.push("<div class='card-inner-container' onclick=singleAssistantClick('" + data.ID + "')>");
                if (data.ManagerLinkUrl != '')
                    html.push(data.ManagerLinkUrl);
                html.push("<div class='resource-img-container'>");
                html.push("<img src='" + data.imageUrl + "'>");
                html.push("</div>");
                html.push("<div>" + data.UsrEditLinkUrl.replace(/&nbsp;/g, '') + "</div>");
                if (data.JobTitle != null) {
                    html.push("<div class='card-inner-data'><span>" + data.JobTitle + "</span></div>");
                }
                html.push("<div class='card-inner-data'><span># of Resources </span>" + data.AssitantCount + "</div>");
                html.push("<div class='card-inner-data'><span># of Allocations </span>" + data.ProjectCount + "</div>");
                html.push("<div class='card-allocationBar'>" + data.AllocationBar + "</div>");
                html.push("</div>");
                html.join('');
                $("#managerCard<%=UserId%>").append(html.join(''));
                $(".users-manager").hide();

            }
        }
    });
});
    function moveLeftUserDetails() {
        let elem = $('#UserDetails .moreIcon').find('i');
            if (elem.hasClass('glyphicon-chevron-right')) {
                $('#UserDetails #scrollContent').animate({ scrollLeft: '+=500' }, 1000);
                elem.removeClass('glyphicon-chevron-right').addClass('glyphicon-chevron-left');
            }
            else if (elem.hasClass('glyphicon-chevron-left')) {
                $('#UserDetails #scrollContent').animate({ scrollLeft: '-=500' }, 1000);
                elem.removeClass('glyphicon-chevron-left').addClass('glyphicon-chevron-right');
            }
    }
</script>
<div id="UserDetails">
    <div id="managerCard<%=UserId%>" style="float: left;" class="rmm-managerCard"></div>
    <div id="scrollContent">
        <div>
            <div>Office:<span> <%=UserData != null ? UserData.DeskLocation : string.Empty%></span></div>
            <div>Division:<span> <%=Division %></span></div>
            <div>Roles:<span> <%=Roles%></span></div>
        </div>
      
        <%if (!string.IsNullOrWhiteSpace(Skills)) { 
            var skills = Skills.Split(',');
            for(int i = 0; i < skills.Length; i+=2) { %>
                <%if (i == 0) { %>
                <div class="marginLeft">
                    <div style="text-align:center;">Skills</div>
                <%} else { %>
                <div>
                    <div style="visibility:hidden;">Skills</div>
                <%} %>
                <%if (!string.IsNullOrWhiteSpace(skills[i])) { %>
                        <div class="buttonStyle1"><%=skills[i]%></div>
                <%} if(i+1 < skills.Length && !string.IsNullOrWhiteSpace(skills[i+1])) { %>
                        <div class="buttonStyle"><%=skills[i+1] %></div>
                <%} %>
            </div>
            <%}%>
        <%} %>
            
        <%if (!string.IsNullOrWhiteSpace(UserData?.Resume)) {  %>
        <div class="marginLeft">
            <div style="visibility:hidden;">Resume</div>
            <div style="float:right;"><a href='<%=UserData.Resume%>' title="View resume" target='_blank' style="display: block;"><img id='imgPdfResume' src='/Content/Images/ResumeIcon.png' style='width:24px;'></a></div>
        </div>
        <%} %>
    </div>
    <a class="moreIcon" id="myProjectLeftIcon" href="#" onclick="moveLeftUserDetails()"><i class="glyphicon glyphicon-chevron-right" style="color:black;"></i></a>
</div>

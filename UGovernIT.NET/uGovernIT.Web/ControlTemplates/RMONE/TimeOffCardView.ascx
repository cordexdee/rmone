<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeOffCardView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMONE.TimeOffCardView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<% if (ListOfUsers != null)
    {
        foreach (uGovernIT.Web.ControlTemplates.RMONE.UserList userList in ListOfUsers)
        { %>
<div class="row">
    <div class="col-md-12">
        <div class="lineHorizontal__container">
            <div class="groupStyle justify-content">
                <img class="dxGridView_gvExpandedButton_UGITNavyBlueDevEx imgHeightWidth" onclick="togglElement($('.toggle-outer<%=userList.ResourceId%>'), $(this));" src="/content/images/icons/right-angle-arrow.png" alt="Collapse" style="cursor: pointer;">
                <span class="dxeBase_UGITNavyBlueDevEx" style="font-size: 16px; font-weight: 500; margin: 0px 3px; vertical-align: middle;white-space:nowrap;"><%=userList.ResourceUser.Replace("'", "`") %></span>
                <a href="javascript:window.parent.parent.UgitOpenPopupDialog" onclick="<%=userList.AddLink%>">
                    <span>
                        <img title="New Allocation" class="clickEventLink dxeImage_UGITNavyBlueDevEx imgHeightWidth" src="/content/images/plus-blue-new.png" alt="" style="height: 18px; width: 18px;"></span>
                </a>
            </div>
            <div class="lineHorizontal"></div>
        </div>
    </div>
</div>
<%foreach (var workItem in GetWorkItems(userList.ResourceId))
    { %>
<div class="toggle-outer<%=userList.ResourceId%>">
<div class="row">
    <div class="col-md-12">
        <div class="innerGroup col-md-2">
            <span class="dxeBase_UGITNavyBlueDevEx" style="font-size: 16px; font-weight: 500; vertical-align: middle; margin: 0px 3px;"><%=workItem %></span>
            <img class="dxGridView_gvExpandedButton_UGITNavyBlueDevEx imgHeightWidth" src="/content/images/icons/right-angle-arrow.png" onclick="togglElement($('.toggle-inner<%=userList.ResourceId + workItem.Replace(" ", "")%>'), $(this))" alt="Collapse" style="cursor: pointer;">
        </div>
    </div>
</div>
<div class="row toggle-inner<%=userList.ResourceId + workItem.Replace(" ", "")%>">
    <div class="col-md-offset-3 col-md-9 disp-flex">
        <%foreach (var resourceItem in GetResourceItems(userList.ResourceId, workItem))
            { %>
        <a href="#" style="color:black;" onclick="openworkitem('<%=resourceItem.WorkItemID%>', '<%=userList.ResourceUser%>')">
        <div class="dashboard-panel-new pt-0 pr-0" title="" <%= resourceItem.AllocationStartDate == resourceItem.AllocationEndDate ? "style='width: 200px;'" : "style='width:300px;'"%> style="width: 300px;">
            <div class="dashboard-panel-main bg-color mt-1 border-left p-2" <%="style=border-color:" + resourceItem.Color%>>
                <div class="pt-1" style="text-align:center">
                    <div class="statustext disp-inline pl-2"><%= resourceItem.AllocationStartDate == resourceItem.AllocationEndDate ? resourceItem.AllocationStartDate.ToString("dd MMMM, yyyy") :
                                                                                     resourceItem.AllocationStartDate.ToString("dd MMMM, yyyy") + " To " + resourceItem.AllocationEndDate.ToString("dd MMMM, yyyy") %></div>
                </div>
                <div class="pt-1 pb-1 pto-style">
                    <div class="statustext disp-inline pl-2 pr-1"><%=resourceItem.WorkItem%></div>
                </div>

            </div>
        </div>
        </a>
        <%} %>
    </div>
</div>
</div>
<%}
        }
    }%>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function togglElement(elem, elem1) {
        if (elem.is(":visible")) {
            elem.hide();
            elem1.attr("src", "/content/images/icons/down-angle-arrow.png");
            elem1.addClass("heightImage");
        }
        else {
            elem.show();
            elem1.attr("src","/content/images/icons/right-angle-arrow.png")
            elem1.removeClass("heightImage");
        }
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #pie {
        height: 340px;
    }
    .heightImage {
    height:15px !important;
    }
    .justify-content {
        display: flex;
    justify-content: center;
    align-items: center;
    }

    .pto-style {
        background: black;
        color: white;
        font-weight: 600;
        margin-top: 7px;
        text-align:center;
    }

    .disp-inline {
        display: inline;
    }

    .disp-flex {
        display: inline-flex;
        flex-direction: row;
        flex-wrap: wrap;
    }

    .task-title {
        font-size: 18px;
        font-weight: 500;
        text-align: center;
        padding-top: 15px;
    }

    .task-title-sub {
        font-size: 14px;
        font-weight: 500;
        text-align: center;
    }

    .dashboard-panel-main {
        border-radius: 0px;
        width: 100%;
    }

    .bg-color {
        background: #f5f5f5;
    }

    .border-left {
        border-left: 5px solid;
    }

    .statustext {
        font-size: 15px;
    }

    .text-decoration {
        text-decoration: underline;
    }

    .dashboard-panel-new {
        border: none !important;
        padding-bottom: 15px;
    }

    .noPadding {
        padding: 0px;
    }

    .page-container {
        background: #ddd;
    }

    .icon-style {
        position: absolute;
        margin-top: -3px;
        margin-left: -7px;
    }

    .icon-style-1 {
        position: absolute;
        margin-top: -3px;
        margin-left: -27px;
    }

    div.scrollelement {
        margin: 4px, 4px;
        padding: 4px;
        height: 340px;
        overflow-x: hidden;
        overflow-y: hidden;
        text-align: justify;
    }

        div.scrollelement:hover {
            overflow-y: scroll;
        }

    .scrollelement::-webkit-scrollbar-track {
        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
        background-color: #F5F5F5;
    }

    .scrollelement::-webkit-scrollbar {
        width: 6px;
        background-color: #F5F5F5;
    }

    .scrollelement::-webkit-scrollbar-thumb {
        background-color: #000000;
    }
</style>

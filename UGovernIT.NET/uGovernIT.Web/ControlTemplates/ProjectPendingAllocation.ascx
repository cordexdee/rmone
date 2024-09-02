<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectPendingAllocation.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.ProjectPendingAllocation" %>
<%@ Import Namespace="uGovernIT.Utility" %>


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
        url: "/api/CoreRMM/GetProjectPendingAllocation",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {
            var counter = 0;
            $('#allocationMoreIcon').hide();
            $("#divProjectView").dxTileView({
                items: message,
                baseItemHeight: 70,
                baseItemWidth: 100,
                direction: "horizontal",
                height: "93px",
                itemMargin:10,
                /*width: function () {
                    return window.innerWidth / 1.5;
                },*/
                showScrollbar: true,
                hint: message.Name,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    ++counter;
                    var Image = itemData.Image == '' ? '/Content/Images/userNew.png' : itemData.Image;
                    if (typeof Image == "undefined" || Image == null)
                        itemElement.append("<div class='paneldiv tooltipp'><div class='panelImgDiv'><img  src='/Content/Images/mangmnt.jpg'></div><p class='mb-0'>" + truncateString(itemData.Name, 20) + "<span class='tooltiptext'>" + itemData.Name + "</span></p></div>");
                    else
                        itemElement.append("<div class='paneldiv tooltipp'><div class='panelImgDiv'><img  src=" + "data:image/png;base64," + Image + "></div><p class='mb-0'>" + truncateString(itemData.Name, 20) + "<span class='tooltiptext'>" + itemData.Name + "</span></p></div>");
                    if (counter >= 5) {
                        $('#allocationMoreIcon').show();
                    }
                    else {
                        $('#allocationMoreIcon').hide();
                    }
                },
                onItemClick: function (e) {
                    
                    if (typeof e.itemData != "undefined") {
                        params = "&TicketID=" + e.itemData.TicketId;
                        openTicketDialog(e.itemData.TicketURL, params, e.itemData.TicketId + ': ' + e.itemData.Name, '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
                    }
                }
            });

        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr);
        }
    });

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    function openPendingAllocation() {
        
        var params = "Status=ProjectPendingAllocation&ShowResourceAllocationBtn=True";
        window.parent.UgitOpenPopupDialog('<%=viewTicketsPath%>', params, 'Project Pending Alloations', '1250', '1250', 0, "", true, true);
    }
    $(document).ready(function () {
        setTimeout(function () {
            $('.dx-empty-message').css("margin-top", "35px");
            $('.dx-empty-message').css("margin-left", "255px");
        }, 1 * 1000);

    });
</script>

<div id="divUserProjectPanel" runat="server"  class="p-3 overflow-hidden">
    <div class="d-flex justify-content-between align-items-center">
        <div id="divProjectView"></div>
        <a class="moreIcon" id="allocationMoreIcon" href="#" onclick="openPendingAllocation()"><i class="fas fa-angle-double-right"></i></a>
    </div>
</div>



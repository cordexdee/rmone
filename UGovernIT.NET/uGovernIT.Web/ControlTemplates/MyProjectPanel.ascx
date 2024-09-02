<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyProjectPanel.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.MyProjectPanel" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .overflow {
    overflow-x:hidden !important;
    }
    .dx-scrollable-content {
    top:10px !important;
    }
   .moreIcon{
    z-index: 9;
    color: #4A6EE2;
    font-size: 18px;
    width: 26px;
    text-align: center;
}
   a:visited {
    color: #4A6EE2;
    text-decoration: none;
}
 .paneldiv .panelImgDiv1 {
    background: gray;
    width: 50px;
    height: 50px;
    margin-left: auto;
    margin-right: auto;
    border-radius: 50%;
    margin-bottom: 10px;
    overflow: hidden;
}
    .paneldiv p {
    font-size:12px;
    }
 #divProjectView .dx-scrollview-content.dx-tileview-wrapper .dx-item.dx-tile, #divLeftPanel .dx-list-item .detailblock .dx-scrollview-content.dx-tileview-wrapper .dx-item.dx-tile {
    border: none !important;
    position: static;
    height: 92px !important;
    width: 150px !important;
    cursor: pointer;
}
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
        url: "/api/CoreRMM/GetUserOpenProject",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {
            var counter = 0;
            $('#myProjectRightIcon').hide();
            $('#myProjectLeftIcon').hide();
            $("#divProjectView").dxTileView({
                items: message,
                baseItemHeight: 100,
                baseItemWidth: 150,
                //direction: "horizontal",
                height: '<%=Height.Value - 32%>px' ,
                itemMargin: 10,
                /*width: function () {
                    return window.innerWidth / 1.5;
                },*/
                showScrollbar: true,
                hint: message.Name,
                noDataText: "No projects assigned",
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    ++counter;
                    var Image = itemData.Image == '' ? '/Content/Images/userNew.png' : itemData.Image;
                    if (typeof Image == "undefined" || Image == null)
                        itemElement.append("<div class='paneldiv tooltipp'><div class='panelImgDiv1'><img style='transform:scale(2);margin-top:10px;height:30px;'  src='/Content/Images/ProjectNew.png'></div><p class='mb-0'>" + truncateString(itemData.Name, 20) + "<span class='tooltiptext'>" + itemData.Name + "</span></p></div>");
                    else
                        itemElement.append("<div class='paneldiv tooltipp'><div class='panelImgDiv'><img  src=" + "data:image/png;base64," + Image + "></div><p class='mb-0'>" + truncateString(itemData.Name, 20) + "<span class='tooltiptext'>" + itemData.Name + "</span></p></div>");
                    if (counter > 2) {
                        $('#myProjectRightIcon').show();
                        $('#myProjectLeftIcon').show();
                    }
                    else {
                        $('#myProjectRightIcon').hide();
                        $('#myProjectLeftIcon').hide();
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

    function moveLeft() {
        $('#divProjectView').animate({ scrollLeft: '+=<%=Width.Value - 58%>' }, 1000);
    }
    function moveRight() {
        $('#divProjectView').animate({ scrollLeft: '-=<%=Width.Value - 58%>' }, 1000);
    }

    $(document).ready(function () {
        setTimeout(function () {
            $('.dx-empty-message').css("margin-top", "35px");
            $('.dx-empty-message').css("font-size", "16px");
            $('.dx-empty-message').css("font-weight", "500");
        }, 1 * 1000);
    });
</script>

<div id="divUserProjectPanel" runat="server"  class="p-3 overflow-hidden overflow">
    <div class="d-flex justify-content-between align-items-center">
        <a class="moreIcon" id="myProjectRightIcon" href="#" onclick="moveRight()"><i class="glyphicon glyphicon-chevron-left" style="color:black;"></i></a>
        <div id="divProjectView"  class="overflow-hidden overflow"></div>
        <a class="moreIcon" id="myProjectLeftIcon" href="#" onclick="moveLeft()"><i class="glyphicon glyphicon-chevron-right" style="color:black;"></i></a>
        
    </div>
</div>



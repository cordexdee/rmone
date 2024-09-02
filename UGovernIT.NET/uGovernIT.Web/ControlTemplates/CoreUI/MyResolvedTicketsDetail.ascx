<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyResolvedTicketsDetail.ascx.cs" Inherits="uGovernIT.Web.MyResolvedTicketsDetail" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function truncateString(str, num) {
        if (str.length > num) {
            return str.slice(0, num) + "...";
        } else {
            return str;
        }
    }
    $.ajax({
        type: "GET",
        url: "/api/CoreRMM/GetResolvedTickets",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {

            $("#divResolvedTickets").dxTileView({
                items: message,
                baseItemHeight: 100,
                baseItemWidth: 70,
                direction: "horizontal",
                height: "93px",
                itemMargin:10,
                //width: function () {
                //    return window.innerWidth / 1.5;
                //},
                showScrollbar: true,
                hint: message.Name,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    
                    var Image = itemData.Image == '' ? '/Content/Images/userNew.png' : itemData.Image;
                    if (typeof Image == "undefined" || Image == null)
                        itemElement.append("<div class='paneldiv tooltipp'><div class='panelImgDiv'><img  src='/Content/Images/mangmnt.jpg'></div><p class='mb-0'>" + truncateString(itemData.Name, 20) + "<span class='tooltiptext'>" + itemData.Name + "</span></p></div>");
                    else
                        itemElement.append("<div class='paneldiv tooltipp'><div class='panelImgDiv'><img  src=" + "data:image/png;base64,"+ Image + "></div><p class='mb-0'>" + truncateString(itemData.Name, 20) + "<span class='tooltiptext'>" + itemData.Name + "</span></p></div>");
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
</script>

<div class="d-flex align-items-center mt-2">
    <div id="divResolvedTickets"></div>
    <a class="moreIcon" href="#"><i class="fas fa-angle-double-right"></i></a>
</div>
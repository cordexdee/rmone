<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UGITCustomTopMenuBar.ascx.cs" Inherits="uGovernIT.Web.UGITCustomTopMenuBar" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-belltown-authenticated #suiteBar {
        display: none;
    }

    .dxm-item {
        padding-left: 0px !important;
        /*font-weight:bold;*/
    }

    .menusettingcss {
        float: right !important;
    }

    .topmenudiv {
        background: none !important;
    }

    .topmenu {
        border: 0px !important;
        background: none !important;
    }

        .topmenu .item {
            margin: 2px;
            /*border: 1px solid rgb(236, 230, 230);*/
            text-align: center;
        }

    .menutopbar {
        border: none !important;background-color: transparent !important;padding: 0px !important;width: 100%;float: left;
    }

    .menutopbar ul.dx {
    }

    .menutopbar-item {
        text-align: center;
    }

        .menutopbar-item > div {
            display: table-cell !important;
            vertical-align: middle;
            text-align: center;
            height: inherit;
            width: inherit;
            float: none !important;
        }

        .menutopbar-item > a {
            display: table-cell !important;
            vertical-align: middle;
            text-align: center;
            white-space: normal !important;
        }

        .menutopbar-item a:hover {
            text-decoration: none !important;
        }

        .menutopbar-item span {
            white-space: nowrap !important;
            margin-right: 0px !important;
            width: 100%;
        }

        .menutopbar-item img {
            max-width: inherit;
            max-height: 25px;
        }

    .menutopbar ul li.dxm-spacing {
        width: 0px;
        min-width: 0px;
    }

    .dxmLite .dxm-separator b {
        /* background-color: none !important;*/
    }

    .divSubMenuitem {
        /*float:left;*/
        margin: 5px 10px 5px 5px;
        padding: 5px;
        display: inline-block;
        /*border: 1px solid #c0c0c0;*/
    }

        /*.divSubMenu {
        display: inline-table;
    }*/

        .divSubMenuitem > a {
            display: table-cell !important;
            vertical-align: middle;
            text-align: center;
            white-space: normal !important;
        }

        .divSubMenuitem > div {
            display: table-cell !important;
            vertical-align: middle;
            text-align: center;
            height: inherit;
            width: inherit;
            float: none !important;
        }

        .divSubMenuitem > a:hover {
            text-decoration: none;
        }

        .divSubMenuitem > a > div {
            /*display: table-cell !important;
                vertical-align: middle;
            opacity:0;*/
        }

            .divSubMenuitem > a > div img {
                width: inherit;
                height: 20px;
            }

            .divSubMenuitem > a > div span {
                white-space: nowrap !important;
                margin-right: 0px !important;
                width: 100%;
            }

    .menu-item {
        background: #FFFFFF !important;
        color: #000000 !important;
    }

     .menu-item:hover{
         /*background: #000000 !important;*/
         color: #FFFFFF !important;
         
         width: 100%;
     }

    .dxm-pImage {
        display: none
    }

    .divSubMenu{
        left:238px !important;
    }

</style>
<link href="<%= ResolveUrl("~/Content/CSS/Dashboard/style.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">



    function cHideTopMenuClicked(obj) {
        if (obj.checked == true) {
            $(".cssHideGlobalSearch").find(":checkbox")[0].checked = true;
        }
        else {
            $(".cssHideGlobalSearch").find(":checkbox")[0].checked = false;
        }
    }
    function setMenuSelectedMenuHighlight() {
        var str = location.href.toLowerCase();
        $(".anchorRootMenuItem").each(function (i, item) {
            var anchor = $(item);


            if (anchor != null && $(anchor).attr('href') != undefined && str == $(anchor).attr('href').toLowerCase()) {
                var color = '<%=menuHighlightColor%>';

                var parentclass = $(anchor).closest('div').attr('class').split(' ').filter(Boolean);
                if (parentclass != null && parentclass != 'undefined' && parentclass.length > 0) {
                    $.each(parentclass, function (index, value) {
                       
                        if (value.match("^parentmenu-")) {
                            var parentmenuitemclass = [];
                            var rootitemli = $('.anchorRootMenuItem').closest('li')
                            $.each(rootitemli, function (currentlist, currentelement) {
                                var parentmenuitemclass = $(currentelement).attr('class').split(' ').filter(Boolean);
                                if ($.inArray(value, parentmenuitemclass) != -1) {
                                    $(currentelement).css("border", "3px solid " + color + "");
                                    $(currentelement).addClass("selectedparent");
                                }

                            });

                        }
                    });
                }

                var borderProperties = "3px solid " + color + "";// + color + "";

                if ($(anchor).parents('li')[0]) {
                    if ('<%=menuHighlightColor%>' == "" || '<%=menuHighlightColor%>' == "#000000")
                        $($(anchor).parents('li')[0]).addClass(" selected-menuitem");
                    else {
                        $($(anchor).parents('li')[0]).css("border", borderProperties);

                    }
                }
                else if ($(anchor).parents('div')[0]) {
                    var subMenuItem = $(anchor).parents('div')[0];
                    if ('<%=menuHighlightColor%>' == "" || '<%=menuHighlightColor%>' == "#000000")
                        $(subMenuItem).addClass(" selected-menuitem");
                    else {
                        $(subMenuItem).css("border", borderProperties);

                    }
                    var subMenu = $($(subMenuItem).parent()).parent();
                    if (subMenu.length > 0) {
                        var strSubMenuClasses = $($(subMenu).parent()).attr("class");
                        if (strSubMenuClasses != "") {
                            var subMenuClasses = strSubMenuClasses.split(" ");
                            $(subMenuClasses).each(function (i, item) {
                                if (subMenuClasses[i].indexOf("parentmenu") > -1) {
                                    if ('<%=menuHighlightColor%>' == "" || '<%=menuHighlightColor%>' == "#000000")
                                        $("li." + item).addClass("selected-menuitem");
                                    else {
                                        $("li." + item).css("border", borderProperties);

                                    }
                                }

                            });
                        }
                    }
                }
        }
        });


}
$(function () {
    setMenuSelectedMenuHighlight();
    var tdwidth = 06
    //$("#ugitIdSearchBox").keyup(function (event) {
    //    if (event && event.keyCode == '13') {
    //        return SubmitSearchRedirect();
    //    }
    //});


  <%-- function SubmitSearchRedirect() {
        var param = "globalSearchString=" + document.getElementById("<%=ugitIdSearchBox.ClientID%>").value + "&isGlobalSearch=true&showGlobalFilter=true";
        window.parent.UgitOpenPopupDialog("<%=filterPage %>", param, 'Global Search', '90', '70', 0);
        return false;
    }--%>

    <%--$(document).ready(function () {
        $("#<%=ugitIdSearchBox.ClientID%>").keypress(function (event) {
            if (event && event.keyCode == '13') {
                SubmitSearchRedirect();
            }
        });
    });--%>

    $(".topmenu-table td").mouseover(function () {
        var menuItems = $(".topmenu-table td .topmenu-itemcontainer");
        $(".topmenu-table td .level2menu-container").hide();
        $(this).find(".topmenu-itemcontainer").addClass("hoverlevel1tab");
        if ($(this).find(".level2menu-container").length > 0 && $(this).find(".level2menu-container>div").length > 0) {
            $(this).find(".level2menu-container").show();
        }
    });

    $(".topmenu-table td").mouseout(function () {
        $(this).find(".topmenu-itemcontainer").removeClass("hoverlevel1tab");
        $(this).find(".level2menu-container").hide();
    });

    $(".topmenu-table .topsubmenu-item").mouseover(function () {
        $(this).addClass("topsubmenu-itemhover");
    });

    $(".topmenu-table .topsubmenu-item").mouseout(function () {
        $(this).removeClass("topsubmenu-itemhover");
    });

    $(this).find(".topmenu-itemcontainer:last").css("border-right", "1px solid #BFBCBC");
    var str = location.href.toLowerCase();

    $("#tblMenu a").each(function () {
        if (str.indexOf(this.href.toLowerCase()) > -1) {
            $("Div.topmenu-itemcontainersub").removeClass("topmenu-itemcontainersub");
            $(this).parent().addClass("topsubmenu-itemselected");
            var mydiv = $(this).parent();
            $($($(mydiv).parent()).parent().children(0)[0]).addClass("topsubmenu-itemselected");
        }
    });

    $('.dxm-popOut').addClass("fa fa-angle-right")

});
function showSubMenuPopUp(linkUrl, title) {
    ASPxClientMenuBase.GetMenuCollection().HideAll();
    window.parent.UgitOpenPopupDialog(linkUrl, "", title, 90, 90, false, "");
}
function MenuItemClick(e) {

    var i = e.item.name;
    if (i == "Change Password")
        OpenPopUpPageWithTitle('<%=siteurl%>/Layouts/uGovernIT/DelegateControl.aspx?control=changepassword', RefreshOnDialogClose, 600, 300, 'Change Your Password');

}

<%--function showTopBarSubMenuItems(s, e) {
    var tabPosition = $(s.GetItemElement(e.item.index)).position();
    var subMenu = $(".submenu-" + e.item.name);
    var subMenuItems = subMenu.find(".divSubMenuitem");
    var parentclass = $(s.GetItemElement(e.item.index)).attr('class').split(' ').filter(Boolean);
    var subMenuAlignment = "center";
    if (parentclass != null && parentclass != 'undefined' && parentclass.length > 0) {
        $.each(parentclass, function (index, value) {
            if (value.match("^submenuitemalignment-"))
            {
                subMenuAlignment = value.split("-")[1];
               
            }
        });
    }
    if (subMenuItems.find(".verticalSubMenu").length == 0) {
        var totalWidth = 0;
        subMenuItems.each(function (i, item) {
            totalWidth += $(item).width();

        });
        totalWidth = totalWidth + subMenuItems.length * 20;
        var popupWidth = $(window).width();
        popupWidth = popupWidth - 0;
        subMenu.parent().width(popupWidth + "px");
        subMenu.width(popupWidth + "px");
      
        var parentItem = $(s.GetItemElement(e.item.index));
        if ($(parentItem).hasClass("selectedparent"))
            subMenu.css({ "margin-top": "4px", "border": "none", "text-align": "" + subMenuAlignment + "", "background-color": "rgba(0, 0, 0, 0.1)" }); // Black with 10% alpha (90% transparent)
        else
            subMenu.css({ "margin-top": "2px", "border": "none", "text-align": "" + subMenuAlignment + "", "background-color": "rgba(0, 0, 0, 0.1)" }); // Black with 10% alpha (90% transparent)

    }
    else {
        subMenuItems.each(function (i, item) {
            $(item).css("display", "block");
        });
        subMenu.parent().width("auto");
        subMenu.width("auto");
        subMenu.css({ "border": "none", "text-align": "center", "left": "" + tabPosition.left + "px" });
    }
}
--%>
</script>

<dx:ASPxMenu  runat="server" ID="dxNavigationMenuForUsers" Width="100%" Border-BorderStyle="None" Paddings-Padding="0px" AllowSelectItem="true" EnableTheming="false"
 Orientation="Vertical" ItemSpacing="0" OnItemDataBound="menuTopBar_ItemDataBound" Class="cust-sub-menu">
    <ItemStyle Paddings-Padding="0" Paddings-PaddingBottom="10px"  Font-Size="14px" />
            <RootItemTextTemplate>
            <asp:HyperLink ID="anchorRootMenuItem" OnLoad="anchorRootMenuItem_Load" CssClass="anchorRootMenuItem" runat="server">
                                 
            </asp:HyperLink>
        </RootItemTextTemplate>
    
    <Items>
	
    </Items>
</dx:ASPxMenu>
            

<dx:ASPxPanel ID="dvTopMenu" runat="server" CssClass="topmenudiv" EnableTheming="true" Visible="false">
    <PanelCollection>
        <dx:PanelContent>
            <table style="width: 100%; border-collapse: collapse;" cellpadding="0" cellspacing="0">
                <tr>
                    <td id="tdmenu" runat="server">
                        <dx:ASPxMenu ID="menuTopBar" AllowSelectItem="True" SelectParentItem="true" ClientInstanceName="menuTopBar" runat="server" SeparatorCssClass="separator" EnableTheming="false" OnItemDataBound="menuTopBar_ItemDataBound" EnableSubMenuFullWidth="true"
                            EnableHotTrack="true" AutoSeparators="None"
                            EnableClientSideAPI="true" CssClass="menutopbar" ItemWrap="false" Orientation="Vertical">
                            <RootItemTextTemplate>
                                <asp:HyperLink ID="anchorRootMenuItem1" OnLoad="anchorRootMenuItem_Load" CssClass="anchorRootMenuItem" runat="server">
                                 
                                </asp:HyperLink>
                            </RootItemTextTemplate>
                            <ItemStyle CssClass="menutopbar-item" />
                            <ClientSideEvents PopUp="function(s,e){ showTopBarSubMenuItems(s,e);}" />
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>"> 
    
    //Hide top menu with open up in iframe
    var isInIframe = window.frameElement && window.frameElement.nodeName == "IFRAME"

    if (isInIframe) {
        $(".topmenudiv").hide();
        document.write('<style type="text/css">.ugit-dialogHidden{display:none !important;}</style>');
    }
    $(document).ready(function () {
       $("#<%=dxNavigationMenuForUsers.ClientID%>").removeClass("dxm-main");
    });
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomMenuBar.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.CustomMenuBar" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .collapseListItem > div {
        text-indent: 0 !important;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function ExpandGroup(value) {
        $.cookie('ExpandGroupValue', value);
        return true;
    }


    $(".ms-bodyareacell> div > table").attr("cellpadding", "0");

    function cHideTopMenuClicked(obj) {
        if (obj.checked == true) {
            $(".cssHideGlobalSearch").find(":checkbox")[0].checked = true;
        }
        else {
            $(".cssHideGlobalSearch").find(":checkbox")[0].checked = false;
        }
    }
    function setMenuSelectedMenuHighlight() {
        var str = location.href.toLowerCase().split('/').pop();
        $(".anchorRootMenuItem").each(function (i, item) {
            var anchor = $(item);
            if (anchor != null && $(anchor).attr('href') != undefined && str == $(anchor).attr('href').toLowerCase().split('/').pop()) {

                //var color = '<%=menuHighlightColor%>';
                var color = 'blue';
                //var parentclass = $(anchor).closest('div').attr('class').split(' ').filter(Boolean);
                var parentclass = $(anchor).parent().parent().prev().prev()
                if (parentclass != null && parentclass != 'undefined' && parentclass.length > 0) {
                    $.each(parentclass, function (index, value) {
                        $(parentclass).find("span").css("color", "blue");
                        //if (value.match("^parentmenu-")) {
                        //    var parentmenuitemclass = [];
                        //    var rootitemli = $('.anchorRootMenuItem').closest('li')
                        //    $.each(rootitemli, function (currentlist, currentelement) {
                        //        var parentmenuitemclass = $(currentelement).attr('class').split(' ').filter(Boolean);
                        //        if ($.inArray(value, parentmenuitemclass) != -1) {
                        //            $(currentelement).css("border", "3px solid " + color + "");
                        //            $(currentelement).addClass("selectedparent");
                        //        }

                        //    });

                        //}
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

    //$(document).ready(function () {
    //    $(".boxshowding").find('ul li').click(function () {
    //        //$(this).find('span').css("cssText", "color: red !important;");
    //        $(this).find('span').addClass("sayali");
    //    });
    //});
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $(".left_menu_wrap .dxpnl-bar:first-child").addClass("dashboardLeft_menuIcon");
        $('.anchorRootMenuItem.leftMenu_listItem').tooltip();
        $('.collapseListItem').tooltip();
    });


</script>



<div class="left_menu_wrap">
        <dx:ASPxPanel ID="ASPxPanel1" ClientInstanceName="AspxCallBackPanel1" runat="server" FixedPosition="WindowLeft" Collapsible="true" 
            cssClass="homeDashboard_leftSideMenu_contentWrap">  
        <SettingsAdaptivity CollapseAtWindowInnerWidth="900" />
            
        <PanelCollection>
            <dx:PanelContent runat="server"  SupportsDisabledAttribute="True">
                <dx:ASPxNavBar ID="dxNavigationMenuForUsers" OnDataBinding="navbarLeftMenu_DataBinding"  OnGroupDataBound="navbarLeftMenu_GroupDataBound" 
                    runat="server" ClientInstanceName="dxNavigationMenuForUsers" AutoCollapse="true" EnableTheming="true" CssClass="homeDashboard_menuListWrap">
                    <GroupHeaderTemplate>
                            <asp:HyperLink ID="anchorGroopmenuitem" runat="server" OnLoad="anchorGroopmenuitem_Load" CssClass="collapseListItem"></asp:HyperLink>
                    </GroupHeaderTemplate>
                    <ItemTextTemplate>
                        <asp:HyperLink ID="anchorRootMenuItem" runat="server" OnLoad="anchorRootMenuItem_Load" CssClass="navbarcss anchorRootMenuItem leftMenu_listItem"></asp:HyperLink>
                    </ItemTextTemplate>
                    <SettingsLoadingPanel ImagePosition="Right"/>
                    <GroupHeaderStyle HorizontalAlign="Left"></GroupHeaderStyle>
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ClientSideEvents ItemClick="function(s,e){ExpandGroup(e.item.group.name);}" HeaderClick="function(s,e){ExpandGroup(e.group.name);}" />
                </dx:ASPxNavBar>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxPanel>
</div>

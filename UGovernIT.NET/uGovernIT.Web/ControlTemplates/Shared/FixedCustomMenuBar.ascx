<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FixedCustomMenuBar.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.FixedCustomMenuBar" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-loadpanel-content {
    
     user-select: none; 
     border: none; 
     background: transparent; 
     border-radius: 0px; 
     -webkit-box-shadow: none; 
     box-shadow: none; 
}
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    function OnItemClick(s, e) {
        LoadingPanel.Show();
        if (e.item.index == 0) {
        <%--    window.location.href = "<%= myProject%>";--%>
        }
        else if (e.item.index == 1) {

        }
        else if (e.item.index == 2) {

        }
        else if (e.item.index == 3) {

        }
        else if (e.item.index == 4) {
            window.location.href = "<%= newTask %>" + "?tkt=tkt";
        }
       
        else if (e.item.index == 5) {
            window.location.href = "<%= newTask %>";
        }

        else if (e.item.index == 6) {
            window.location.href = "<%= newTask %>" + "?tkt=morit";
        }

         else if (e.item.index == 7) {
            window.location.href = "<%= newTask %>" + "?tkt=newt";
        }

    }

    $(document).ready(function () {
        $('.fixedMenu-subMenuItem').parent().addClass('fixedMenu-subMenuItemUl');
        $('.fixedMenu-subMenuItemUl').parent().addClass('fixedMenu-subMenuItemContainer');
        //$('.fixedMenu-subMenuItemUl a img.dxm-image').tooltip();
        //$('.leftSideBottomMenu-container ul li img.dxm-image.dx-vam').tooltip();
        //$('.fixedMenu-subMenuItemUl li img.dxm-image').tooltip();
        $('.BackgroundTaskcontainer img').tooltip();
        $('.leftMenu_listItem img').tooltip();
    });
    $(document).ready(function () {
        //$('#backgroundTasksActive').hide();
        //$('#backgroundTasksView').hide();
        $(".left_menu_wrap .dxpnl-btn").click(function () { init(); });
    });
    function init() {
        let clickExpand = true;
        $(".dxnb-gr .dxnb-header").each(function () {
            if ($(this).is(":visible")) {
                clickExpand = false;
            }
        });
        if (clickExpand) {
            if ($(".dxWeb_nbCollapse_UGITNavyBlueDevEx").length) {
                $(".dxWeb_nbExpand_UGITNavyBlueDevEx")[0].click();
            }
        }
        $(".left_menu_wrap .dxnb-gr").each(function () {
            if ($(this).find(".dxnb-header span").length == 0) {
                if ($(this).find(".dxWeb_nbCollapse_UGITNavyBlueDevEx").length) {
                    $(this).find(".dxWeb_nbCollapse_UGITNavyBlueDevEx").addClass("homeDashboard_menuListWrap_notitle");
                    $(this).find(".dxWeb_nbCollapse_UGITNavyBlueDevEx").before($(this).find(".dxWeb_nbCollapse_UGITNavyBlueDevEx").next());
                }
            }
            if ($(this).find(".dxnb-headerCollapsed span").length == 0) {
                if ($(this).find(".dxWeb_nbExpand_UGITNavyBlueDevEx").length) {
                    $(this).find(".dxWeb_nbExpand_UGITNavyBlueDevEx").addClass("homeDashboard_menuListWrap_notitle");
                    $(this).find(".dxWeb_nbExpand_UGITNavyBlueDevEx").before($(this).find(".dxWeb_nbExpand_UGITNavyBlueDevEx").next());
                }
            }
        });
        //$(".dxWeb_nbCollapse_UGITNavyBlueDevEx").before($(".dxWeb_nbCollapse_UGITNavyBlueDevEx").next());
        //$(".dxWeb_nbExpand_UGITNavyBlueDevEx").before($(".dxWeb_nbExpand_UGITNavyBlueDevEx").next());
        //$(".dxnb-content").show();
        //$(".dxnb-content").prev().hide();
        //$(".dxnb-content").prev().prev().show();
    }
</script>
   
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Border-BorderStyle="None"
    LoadingDivStyle-BackColor="Transparent" LoadingDivStyle-Border-BorderStyle="None" BackColor="Transparent"  Modal="True">
    <Image Url="~/Content/Images/ajax-loader.gif"></Image>
    
</dx:ASPxLoadingPanel>
<dx:ASPxMenu ID="menuNewPresentation" runat="server" EnableHotTrack="true" OnItemDataBound="menuNewPresentation_ItemDataBound"
    Orientation="Vertical" SeparatorWidth="0" CssClass="leftSideBottomMenu-container">
    <ItemTextTemplate>
          <asp:HyperLink ID="anchorRootMenuItem" runat="server" OnLoad="anchorRootMenuItem_Load" CssClass="navbarcss anchorRootMenuItem leftMenu_listItem"></asp:HyperLink>
    </ItemTextTemplate>
    <ClientSideEvents Init="init"  />
    <%--<ClientSideEvents ItemClick="OnItemClick" />--%>
</dx:ASPxMenu>

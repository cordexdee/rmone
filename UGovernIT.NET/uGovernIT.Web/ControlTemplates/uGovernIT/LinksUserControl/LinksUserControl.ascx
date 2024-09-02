<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LinksUserControl.ascx.cs" Inherits="uGovernIT.Web.LinksUserControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
     .newuititleclass {
        position: relative !important;
        z-index: 1;
        left: -14px;
        top: 3px;
        
    }
</style>
<asp:Panel ID="authorizedPanel" runat="server">
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        $(document).ready(function () {
            if ('<%=HideControlBorder%>' != 'False') {
                removeLinkRCornerClass();
            }
            if ('<%=ControlWidth%>' != "") {
                $($('#<%=dvLinkView.ClientID%>').find(".service-ul")).attr("style", "width:<%=ControlWidth%>px");
            }
            ShowLoadLinkView();

            $('#<%=dvLinkView.ClientID%>').hover(
                function () {
                    $("#<%=imgHideShowLinkView.ClientID%>").show();
                    $('#<%=imgLinkEditMessage.ClientID%>').show();
                }, function () {
                    if ($('#<%=dvLinkView.ClientID%>').is(':hidden') == false) {
                        $('#<%=imgLinkEditMessage.ClientID%>').hide();
                        $("#<%=imgHideShowLinkView.ClientID%>").hide();
                    }
                });

            $("#<%=imgHideShowLinkView.ClientID%>").hover(
                function () {
                    $("#<%=imgHideShowLinkView.ClientID%>").show();
                    $('#<%=imgLinkEditMessage.ClientID%>').hide();
                }, function () {
                    if ($('#<%=dvLinkView.ClientID%>').is(':hidden') == false) {
                        $("#<%=imgHideShowLinkView.ClientID%>").hide();
                        $('#<%=imgLinkEditMessage.ClientID%>').hide();
                    }
                });


            $('#<%=imgLinkEditMessage.ClientID%>').hover(
                function () {
                    $('#<%=imgLinkEditMessage.ClientID%>').show();
                    $("#<%=imgHideShowLinkView.ClientID%>").hide();

                }, function () {
                    if ($('#<%=dvLinkView.ClientID%>').is(':hidden') == false) {
                        $('#<%=imgLinkEditMessage.ClientID%>').hide();
                        $("#<%=imgHideShowLinkView.ClientID%>").hide();
                    }
                });
        });


        function HideShowLinkView(obj) {
            if ($(obj).parent().next().is(':hidden') == true)
                ShowLinkView(obj);
            else
                HideLinkView(obj);
        }


        function ShowLinkView(ctrlObj) {
            var parentDiv = $(ctrlObj).parent();
            $(parentDiv).find(".imgShowHideClass").hide();
            $(parentDiv).find(".imgShowHideClass").attr('src', '/Content/Images/collapse-all.png');
            $(parentDiv).find(".imgShowHideClass").css('position', 'absolute');
            $(parentDiv).find(".imgShowHideClass").css('left', '<%=ControlWidth%>' - 30 + 'px');

            $(parentDiv).css('border-bottom', 'none');
            $(parentDiv).css('padding-bottom', '0px');
            $(parentDiv).css('margin-left', '0px');
            $(parentDiv).find(".linkTitleClass").hide();
            $(parentDiv).find(".imgShowHideClass").show();
            $(ctrlObj).parent().next().show();
        }

        function HideLinkView(ctrlObj) {
            var parentDiv = $(ctrlObj).parent();
            $(parentDiv).find(".imgShowHideClass").hide();
            $(parentDiv).find(".imgShowHideClass").attr('src', '/Content/Images/expand-all.png');
            $(parentDiv).find(".imgShowHideClass").css('position', 'relative');
            $(parentDiv).find(".imgShowHideClass").css('left', '0px');

            $(parentDiv).css('border-bottom', '1px solid #D8D8D8');
            $(parentDiv).css('padding-bottom', '33px');
            $(parentDiv).css('margin-left', '2px');
            $(parentDiv).find(".linkTitleClass").show();
            $(parentDiv).find(".imgLinkClass").hide();
            $(parentDiv).find(".imgShowHideClass").show();
            $(ctrlObj).parent().next().hide();
        }

        function removeLinkRCornerClass() {
            <%--$('#<%=dvLinkView.ClientID%>').find(".cg-topleft-corner").removeClass("cg-topleft-corner");
            $('#<%=dvLinkView.ClientID%>').find(".cg-topmiddle-line").removeClass("cg-topmiddle-line");
            $('#<%=dvLinkView.ClientID%>').find(".cg-topright-corner").removeClass("cg-topright-corner");

            $('#<%=dvLinkView.ClientID%>').find(".cg-middleleft-line").removeClass("cg-middleleft-line");
            $('#<%=dvLinkView.ClientID%>').find(".cg-middleright-line").removeClass("cg-middleright-line");

            $('#<%=dvLinkView.ClientID%>').find(".cg-bottomleft-corner").removeClass("cg-bottomleft-corner");
            $('#<%=dvLinkView.ClientID%>').find(".cg-bottommiddle-line").removeClass("cg-bottommiddle-line");
            $('#<%=dvLinkView.ClientID%>').find(".cg-bottomright-corner").removeClass("cg-bottomright-corner");--%>

            $('#<%=dvLinkView.ClientID%>').css("border-width","0")
        }

        function ShowLoadLinkView() {
            $('#<%=dvLinkView.ClientID%>').show();

            $("#<%=imgHideShowLinkView.ClientID%>").hide();
            $("#<%=imgHideShowLinkView.ClientID%>").attr('src', '/Content/Images/collapse-all.png');
            $("#<%=imgHideShowLinkView.ClientID%>").css('position', 'absolute');
            $("#<%=imgHideShowLinkView.ClientID%>").css('left', $('#<%=dvLinkView.ClientID%>').width() - 30 + 'px');

            $('#<%=dvLinkViewHeader.ClientID%>').css('border-bottom', 'none');
            $('#<%=dvLinkViewHeader.ClientID%>').css('padding-bottom', '0px');
            $('#<%=dvLinkViewHeader.ClientID%>').css('margin-left', '0px');
            $("#<%=spLinkViewTitle.ClientID%>").hide();

            $('.imgShowHideClass').each(function (index) {
                $(this).hide();
                $(this).css('left', '<%=ControlWidth%>' - 30 + 'px');
            });

            $('.imgLinkClass').each(function (index) {
                $(this).hide();
                $(this).css('left', '<%=ControlWidth%>' - 50 + 'px');
            });
        }

        function OpenEditViewLink(ctrl) {
            var linkviewpathurl = $(ctrl).parent().find('input[type="hidden"]').val()
            window.parent.UgitOpenPopupDialog(linkviewpathurl, "", "Link View", "800px", "500px", false);
            return false;
        }
    </script>

    <div>
        <div id="dvLinkViewHeader" runat="server" style="position: relative; width: 99.2%; padding-top: 5px;">
            <img id="imgHideShowLinkView" runat="server" class="imgShowHideClass" src="/Content/Images/collapse-all.png" style="z-index: 1; position: absolute; float: left; display: none; padding-top: 6px; padding-left: 4px;" onclick="HideShowLinkView(this)" />
            <span id="spLinkViewTitle" class="linkTitleClass" runat="server" style="float: left; display: none; padding-top: 8px; padding-left: 2px;">Links</span>
            <img id="imgLinkEditMessage" class="imgLinkClass" runat="server" src="/Content/Images/edit-newIcon_16x16.png" title="Edit Link View" style="z-index: 1; position: absolute; float: left; display: none; padding-top: 6px; padding-left: 4px;" onclick="OpenEditViewLink(this)" />
            <asp:HiddenField ID="hdnEditLinkViewPath" runat="server" />
        </div>

        <div id="dvLinkView" runat="server" style="padding-top: 0px; float: left; padding-left: 0px; z-index: 9999; -webkit-border-radius: 20px; -moz-border-radius: 20px; border-radius: 20px; border: 1px solid #212100;">
            <div class="links-items">
                <table cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                    <tr>
                        <td valign="top">
                            <div style="padding-left: 15px; padding-top: 5px; font-weight: bold;">
                                <asp:Label ID="lblViewHeader" runat="server"></asp:Label>
                            </div>
                            <div class="dashboard-panel" style="margin-top: 20px;">
                                <asp:Panel ID="Panel1" runat="server" class="cg-d-main dashboardpanelcontainer ">
                                    <asp:Panel ID="Panel2" runat="server" CssClass="cg-dashboardtopaction-type1">
                                        <table style="border-collapse: collapse;" cellpadding="0" cellspacing="0">
                                            <tr style="height: 20px"></tr>
                                            <tr>
                                                <td valign="top">
                                                    <div style="float: left; padding-left: 15px;">
                                                        <ul class="service-ul">
                                                            <asp:Repeater ID="RptSubCategory" runat="server" OnItemDataBound="RptSubCategory_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <li>
                                                                        <table cellspacing="0" border="0" style="border-collapse: collapse;">
                                                                            <tr>
                                                                                <td class="servicecategory" valign="top">
                                                                                    <b>
                                                                                        <asp:HiddenField ID="hdnCategroyId" runat="server" Value='<%#Eval("ID")%>' />
                                                                                        <asp:Label ID="LblSubCategroy" runat="server" Text='<%#Eval("Title")%>'></asp:Label></b><br />
                                                                                    <br />
                                                                                    <img id="img2" runat="server" width="50" src='<%#Eval("ImageUrl")%>' alt="" />
                                                                                </td>

                                                                                <td valign="top" class="servicetype">
                                                                                    <asp:Repeater ID="RptItemList" runat="server" OnItemDataBound="RptItemList_ItemDataBound">
                                                                                        <ItemTemplate>
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td style="text-align: left; padding: 2px 0px;" class="serviceitem">
                                                                                                        <asp:LinkButton ID="LnkListName" runat="server" Text='<%#Eval("Title")%>'></asp:LinkButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ItemTemplate>
                                                                                    </asp:Repeater>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ul>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

</asp:Panel>


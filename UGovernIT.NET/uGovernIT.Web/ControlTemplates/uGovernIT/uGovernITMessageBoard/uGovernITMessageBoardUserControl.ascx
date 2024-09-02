<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITMessageBoardUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITMessageBoardUserControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" src="/Scripts/jquery.cookie.js?v=<%=UGITUtility.AssemblyVersion %>"></script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function removeRCornerClass(obj) {
        $(obj).find(".cg-topleft-corner").removeClass("cg-topleft-corner");
        $(obj).find(".cg-topmiddle-line").removeClass("cg-topmiddle-line");
        $(obj).find(".cg-topright-corner").removeClass("cg-topright-corner");

        $(obj).find(".cg-middleleft-line").removeClass("cg-middleleft-line");
        $(obj).find(".cg-middleright-line").removeClass("cg-middleright-line");

        $(obj).find(".cg-bottomleft-corner").removeClass("cg-bottomleft-corner");
        $(obj).find(".cg-bottommiddle-line").removeClass("cg-bottommiddle-line");
        $(obj).find(".cg-bottomright-corner").removeClass("cg-bottomright-corner");
    }

    $(function () {
        var dashboardDisplay = '<%= DisplayOnDashboard%>'
        if (dashboardDisplay != 'True') {
            if ($.cookie("ShowMessageBoard") == '0') {
                HideMessageBoard();
            }
            else {
                ShowMessageBoards(false);
            }
            $('#<%=dvMessageBoard.ClientID%>').hover(
                function () {
                    $("#imgHideShowMessageBoard").show();
                    $('#<%=imgEditMessage.ClientID%>').show();
                }, function () {
                    if ($('#<%=dvMessageBoard.ClientID%>').is(':hidden') == false) {
                        $('#<%=imgEditMessage.ClientID%>').hide();
                        $("#imgHideShowMessageBoard").hide();

                    }
                });

            $('#imgHideShowMessageBoard').hover(
                function () {
                    $("#imgHideShowMessageBoard").show();
                    $('#<%=imgEditMessage.ClientID%>').hide();
                }, function () {
                    if ($('#<%=dvMessageBoard.ClientID%>').is(':hidden') == false) {
                        $("#imgHideShowMessageBoard").hide();
                        $('#<%=imgEditMessage.ClientID%>').hide();
                    }
                });

            $('#<%=imgEditMessage.ClientID%>').hover(
                function () {
                    $("#imgHideShowMessageBoard").hide();
                    $('#<%=imgEditMessage.ClientID%>').show();
                }, function () {
                    if ($('#<%=dvMessageBoard.ClientID%>').is(':hidden') == false) {
                        $("#imgHideShowMessageBoard").hide();
                        $('#<%=imgEditMessage.ClientID%>').hide();
                    }
                });
        }
        else {

            $('[id$="dvMessageBoard"]').each(function (i, item) {
                $(item).show();

                //$(item).css('padding-left', '0px');
                if ($(item).find('[id$="hndborderStyle"]').val() != 'Rounded Rectangle') {
                    removeRCornerClass(item);
                    $(item).find('[id$="ulMessageboard"]').each(function (i, item) {
                        $(item).css('padding', '16px');
                    });

                }
                else {

                }
            });
        }
    });

    function HideShowMessageBoard(obj) {
        var ctr = $('#<%=dvMessageBoard.ClientID%>');
        if (ctr.is(':hidden') == true) {
            ShowMessageBoards(true);
            $.cookie("ShowMessageBoard", 1, { expires: 9999 });
        }
        else {
            HideMessageBoard()
            $.cookie("ShowMessageBoard", 0, { expires: 9999 });
        }
    }

    function ShowMessageBoards(setPosition) {

        $('#<%=dvMessageBoard.ClientID%>').show();
        $("#imgHideShowMessageBoard").hide();
        $("#imgHideShowMessageBoard").attr('src', '/Content/Images/Minus_16x16.png');
        $("#imgHideShowMessageBoard").css('position', 'absolute');
        $("#imgHideShowMessageBoard").css('left', $('#<%=dvMessageBoard.ClientID%>').width() - 30 + 'px');
        $("#spMessageBoardTitle").hide();
        $('div[id$="messagefooterdata"]').hide();
        $('#<%=imgEditMessage.ClientID%>').hide();
        $('#<%=imgEditMessage.ClientID%>').css('left', $('#<%=dvMessageBoard.ClientID%>').width() - 50 + 'px');
        if (setPosition) {
            $("#s4-workspace").scrollTop($('#messageBoardBotton').position().top);
        }
    }

    function HideMessageBoard() {

        $('#<%=dvMessageBoard.ClientID%>').hide();
        $("#imgHideShowMessageBoard").hide();
        $("#imgHideShowMessageBoard").css('position', 'relative');
        $("#imgHideShowMessageBoard").css('right', '0px');
        $('div[id$="messagefooterdata"]').show();
        var strFooterMsg = '<%=hdnDispayFooterMsg.Value%>';
        $('span[id$="spanbody"]').html(strFooterMsg);
        $('img[id$="imgMessage"]').attr('src', '<%=hdnimgFooter.Value%>');

    }

    function openContactDialog(path, ticketId, title) {
        window.parent.parent.UgitOpenPopupDialog(path, "", ticketId + ": " + title, 90, 90, false);
    }

    function OpenEditMessageBoard() {
        window.parent.UgitOpenPopupDialog('<%=editMessagePath%>', "", "Message Board", "800px", "500px", false);
        return false;
    }


</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #tblMessage {
        overflow: hidden;
        margin: 10px 0;
    }

    #tblMessage .error-icon-left {
        float: left;
        padding: 15px;
        margin-left: 20px;
    }

    #tblMessage .error-msg-right {
        float: left;
        padding: 10px 20px !important;
        border-radius: 5px;
        border: 1px solid #ccc;
        font-family: 'Poppins', sans-serif !important;
        font-style: normal !important;
    }

    .dxdpLite.dxpclW {
        height: auto;
    }

    #tblMessage .error-icon-left img {
        width: 65px !important;
        height: auto !important;
    }
</style>


<div style="width: 100%;">

    <div id="dvMessageBoard" runat="server" class="col-md-12 col-xs-12 col-xs-12" style="padding-top: 0px; display: none; float: left; padding-left: 0px;">

        <asp:HiddenField ID="hndborderStyle" runat="server" />
        <asp:HiddenField ID="hndDisplayOnDashboard" runat="server" />
        <asp:HiddenField ID="hdnDispayFooterMsg" runat="server" />
        <asp:HiddenField ID="hdnimgFooter" runat="server" />
        <%--<div style="float: left; width: auto;">
            <img id="imgMessage" runat="server" style="width: 64px; height: 64px; padding-right: 4px;" src="/Content/Images/overallstatus_good.png" />
        </div>--%>

        <div class="homeDashboard_msgBoard_container row">
            <div id="tblMessage col-xs-12 col-md-12 col-sm-12">
                <div class="row">
                    <div class="error-icon-left msgBoard_errorIcon col-md-2">
                        <img id="imgMessage" runat="server" class="homeDashboard_msgBoardIconImg" style="" src="/Content/Images/overallstatus_good.png" />
                    </div>
                    <div id="dvMessageBoardHeader" runat="server" style="float: left;">
                        <div style="display:none">
                            <img id="imgHideShowMessageBoard" src="/content/Images/Minus_16x16.png" style="z-index: 1; position: absolute; float: left; display: none; padding-top: 6px; padding-left: 4px;" onclick="HideShowMessageBoard(this)" />
                        </div>
                        <span id="spMessageBoardTitle" style="float: left; display: none; padding-top: 8px; padding-left: 2px;">Announcements</span>
                        <img id="imgEditMessage" runat="server" src="/content/images/edit-icon.png" title="Edit Message" style="z-index: 1; position: absolute; float: left; display: none; padding-top: 6px; padding-left: 4px;" onclick="OpenEditMessageBoard()" />
                    </div>
                    <div id="ulMessageboard" runat="server" style="padding: 7px" class="messageboard-ul error-msg-right">
                        <asp:Repeater ID="MyMessageRepeater" runat="server" OnItemDataBound="MyMessageRepeater_OnDataBinding">
                            <ItemTemplate>
                                <li id="liMessage" style="text-align: left" runat="server">
                                    <asp:HiddenField ID="MessageType" runat="server" Value='<%#Bind("MessageType") %>' />
                                    <asp:HiddenField ID="hdnTitle" runat="server" Value='<%#Bind("Title") %>' />
                                    <asp:HiddenField ID="hdnTicketId" runat="server" Value='<%#Bind("TicketId") %>' />
                                    <asp:HiddenField ID="hdnNavigationUrl" runat="server" Value='<%#Bind("NavigationUrl") %>' />
                                    <%# Eval("Body") %>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <!-- tblMessage -->

            <%--<table id="tblMessage" cellpadding="0" cellspacing="0" style="border-collapse: collapse;">
                <tbody>
                    <tr>
                        <td colspan="3">
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="cg-topleft-corner" align="left"></td>
                        <td class="cg-topmiddle-line" align="right"></td>
                        <td class="cg-topright-corner" align="right">
                            <div id="dvMessageBoardHeader" runat="server" style="padding-top: 5px;">
                                <img id="imgHideShowMessageBoard" src="/content/Images/Minus_16x16.png" style="z-index: 1; position: absolute; float: right; display: none; padding-top: 6px; padding-left: 4px;" onclick="HideShowMessageBoard(this)" />
                                <span id="spMessageBoardTitle" style="float: left; display: none; padding-top: 8px; padding-left: 2px;">Announcements</span>
                                <img id="imgEditMessage" runat="server" src="/content/images/edit-icon.png" title="Edit Message" style="z-index: 1; position: absolute; float: right; display: none; padding-top: 6px; padding-left: 4px;" onclick="OpenEditMessageBoard()" />
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td class="cg-middleleft-line"></td>
                        <td>
                            <ul id="ulMessageboard" runat="server" style="padding: 7px" class="messageboard-ul">
                                <asp:Repeater ID="MyMessageRepeater" runat="server" OnItemDataBound="MyMessageRepeater_OnDataBinding">
                                    <ItemTemplate>
                                        <li id="liMessage" style="text-align: left" runat="server">
                                            <asp:HiddenField ID="MessageType" runat="server" Value='<%#Bind("MessageType") %>' />
                                            <asp:HiddenField ID="hdnTitle" runat="server" Value='<%#Bind("Title") %>' />
                                            <asp:HiddenField ID="hdnTicketId" runat="server" Value='<%#Bind("TicketId") %>' />
                                            <asp:HiddenField ID="hdnNavigationUrl" runat="server" Value='<%#Bind("NavigationUrl") %>' />
                                            <%# Eval("Body") %>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </td>
                        <td class="cg-middleright-line"></td>
                    </tr>

                    <tr>
                        <td class="cg-bottomleft-corner" align="left" valign="top"></td>
                        <td class="cg-bottommiddle-line" align="right" valign="top"></td>
                        <td align="right" valign="top" class="cg-bottomright-corner"></td>
                    </tr>
                </tbody>
            </table>--%>
        </div>
    </div>
</div>
<a id="messageBoardBotton"></a>


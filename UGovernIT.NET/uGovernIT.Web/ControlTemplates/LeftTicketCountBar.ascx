<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftTicketCountBar.ascx.cs" Inherits="uGovernIT.Web.LeftTicketCountBar" %>
<%@ Register Src="~/ControlTemplates/UserProjectPanel.ascx" TagPrefix="ugit" TagName="UserProjectPanel" %>
<%@ Register Src="~/ControlTemplates/CoreUI/RecentProjectDetail.ascx" TagPrefix="ugit" TagName="RecentProjectDetail" %>
<%@ Register Src="~/ControlTemplates/CoreUI/RecentOPMDetail.ascx" TagPrefix="ugit" TagName="RecentOPMDetail" %>
<%@ Register Src="~/ControlTemplates/CoreUI/AllOpenOPM.ascx" TagPrefix="ugit" TagName="AllOpenOPM" %>
<%@ Register Src="~/ControlTemplates/CoreUI/AllOpenProjectDetail.ascx" TagPrefix="ugit" TagName="AllOpenProjectDetail" %>
<%@ Register Src="~/ControlTemplates/CoreUI/WaitingOnMeDetail.ascx" TagPrefix="ugit" TagName="WaitingOnMeDetail" %>
<%@ Register Src="~/ControlTemplates/CoreUI/NPRTicketsDetail.ascx" TagPrefix="ugit" TagName="NPRTicketsDetail" %>
<%@ Register Src="~/ControlTemplates/CoreUI/MyResolvedTicketsDetail.ascx" TagPrefix="ugit" TagName="MyResolvedTicketsDetail" %>
<%@ Import Namespace="uGovernIT.Utility" %>




<script  type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var globalheight = '<%=Height%>';
    var width = '<%=Width%>';
    $(function () {

        $("#divLeftPanel").dxList({
            dataSource: "/api/CoreRMM/GetRMMPanelCount?ViewID=" + '<%=ViewID%>',
            width: '<%=Width%>',
            height: '<%=Height%>',
            itemTemplate: function (data, index, element) {
                var html = new Array();
                html.push("<div class='itemblock' onclick='openticketimage(\"" + data.Status + "\")'>");
                if (data.HideIcon == true) {
                    if (data.IconUrl == undefined) {
                        html.push("<div id='logo'><img src='/Content/Images/newMoveTo.png' width='20' /></div>");
                    }
                    else
                        html.push("<div id='logo'><img src='/Content/Images/" + data.IconUrl + "' width='35' /></div>");
                }
                html.push("<p>" + data.Text + " </p></div>");
                if ('<%=ShowDetails%>' == 'True') {
                    if (data.Status == "myproject")
                        html.push("<div class='detailblock'>" + $('#recentProject').html() + "</div>")
                    else if (data.Status == "myopenopportunities")
                        html.push("<div class='detailblock'>" + $('#recentOPM').html() + "</div>")
                    else if (data.Status == "allopenproject")
                        html.push("<div class='detailblock'>" + $('#AllOpenProjects').html() + "</div>")
                    else if (data.Status == "allopenopportunities")
                        html.push("<div class='detailblock'>" + $('#AllOpenOPM').html() + "</div>")
                    else if (data.Status == "waitingonme")
                        html.push("<div class='detailblock'>" + $('#WaitingOnMe').html() + "</div>")
                    else if (data.Status == "nprtickets")
                        html.push("<div class='detailblock'>" + $('#NPRTickets').html() + "</div>")
                    else if (data.Status == "resolvedtickets")
                        html.push("<div class='detailblock'>" + $('#ResolvedTickets').html() + "</div>")
                }
                element.attr("class", "CountBlock d-flex align-items-center justify-content-center");
                element.append(html.join(""));
            }
            //onItemClick: function (e) {
            //    if (typeof e.itemData != "undefined") {
            //        openticketimage(e.itemData.Status);
            //    }
            //}
        }).dxList("instance");

    });

    function openticketimage(status) {
        var params = "Status=" + status + "&ShowResourceAllocationBtn=True";
        if (status == 'totalresource') {
            window.parent.UgitOpenPopupDialog('<%=viewResourcesPath%>', params, 'Resources', '90%', '800', 0, "", true, true);
        }
        else
            window.parent.UgitOpenPopupDialog('<%=viewTicketsPath%>', params, 'My Projects', '85%', '900', 0, "", true, true);
    }

    $(document).on("click", "img.openticketimage", function (e) {

    });
</script>


<div id="divLeftPanel" class="px-3 py-2">
</div>
<div style="display: none">
    <div id="recentProject">
        <ugit:RecentProjectDetail runat="server" ID="RecentProjectDetail" />
    </div>
    <div id="recentOPM">
        <ugit:RecentOPMDetail runat="server" ID="RecentOPMDetail" />
    </div>
    <div id="AllOpenOPM">
        <ugit:AllOpenOPM runat="server" ID="AllOpenOPM1" />
    </div>
    <div id="AllOpenProjects">
        <ugit:AllOpenProjectDetail runat="server" ID="AllOpenProjectDetail" />
    </div>
    <div id="WaitingOnMe">
        <ugit:WaitingOnMeDetail runat="server" ID="WaitingOnMeDetail" />
    </div>
    <div id="NPRTickets">
        <ugit:NPRTicketsDetail runat="server" ID="NPRTicketsDetail" />
    </div>
    <div id="ResolvedTickets">
        <ugit:MyResolvedTicketsDetail runat="server" ID="MyResolvedTicketsDetail" />
    </div>
</div>

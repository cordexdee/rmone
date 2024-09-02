<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeftLinkBar.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.LeftLinkBar" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var Projects = [{ 'Status': 'PTO', 'Text': 'PTO', 'iconUrl': 'out_of_office2.png' }, { 'Status': 'Timesheet', 'Text': 'Timesheet', 'iconUrl': 'time_sheet_icon.png' }];
    $(function () {
        $("#divLeftPanel").dxList({
            dataSource: Projects,
            width: '<%=Width%>',
            height: '<%=Height%>',
            itemTemplate: function (data, index, element) {
                var html = new Array();
                html.push("<div class='' onclick='openticketimage(\"" + data.Status + "\")'>");
                //html.push("<p>" + data.Text + " </p>");
                html.push("<img width='50' title=" + data.Text + " class='img-responsive' src='<%=ResolveUrl("~/Content/Images/")%>" + data.iconUrl + "' />");
                html.push("</div>")
                element.attr("class", "CountBlock d-flex align-items-center justify-content-center py-2");
                element.append(html.join(""));
            }
        }).dxList("instance");
    });
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #divLeftPanel .dx-scrollview-content {
    display: inline-flex;
    justify-content: flex-start;
    align-items: center;
    margin-top: 10px;
    width: 100%;
    }
</style>
<div id="divLeftPanel" class="px-2 py-2"></div>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function openticketimage(status) {
        var params = 'control=customresourcetimeSheet';
        if (status == 'Timesheet') {
            params = 'control=customresourcetimeSheet';
            window.parent.UgitOpenPopupDialog('<%=viewTicketsPath%>', params, 'Timesheet', '85%', '900', 0, "", true, true);
        }
        else  {
            params = 'control=projectcalendarview';
            window.parent.UgitOpenPopupDialog('<%=viewTicketsPath%>', params, 'Out-Of-Office Team Calendar', '85%', '900', 0, "", true, true);
        }
    }
</script>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserOrganizationChart.ascx.cs" Inherits="uGovernIT.Web.UserOrganizationChart" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script src="/Scripts/getorgchart.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<link href="<%= ResolveUrl(@"~/Content/getorgchart.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
   html, body {margin: 0px; padding: 0px;width: 100%;height: 100%;overflow: hidden; }
        #people {width: 100%;height: 100%; }   
		#people .get-org-chart .get-oc-c .get-box {fill:url(#level1);  stroke: #201F35;  }
		#people .get-org-chart {background-color: #FFF;}		
		#people .get-org-chart .get-oc-tb{background-image: url("/Content/images/uGovernIT/simple-gradient.png"); background-repeat:repeat-x;}
		#people .get-org-chart .get-oc-tb {border-bottom: 2px solid #000000;}		
		/*#people .get-org-chart .get-oc-c .link{stroke: #FFFFFF;}*/				
		#people .get-org-chart .get-oc-c .get-text {fill: #201F35;}
		#people .get-org-chart .get-oc-c .get-text-0 {font-size: 30px;}
		/*#people .get-org-chart .get-user-logo path  {fill: url(#level2);}*/
</style>

<div id="people"></div>
<dx:ASPxHiddenField ID="hdnfield" runat="server" ClientInstanceName="clienthdnfield"></dx:ASPxHiddenField>
<script type="text/ecmascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    showChart();
    function showChart() {
        
       $('#people').getOrgChart({
            theme: "annabel",
            primaryColumns: ["name", "title", "phone", "mail"],
            imageColumn: "picture",
            color: "neutralgrey",
            zoomable: true,
            movable: true,
            orientation: 7,
            linkType: "M",
            scale: 0.5,
            embededDefinitions:
				'<linearGradient id="level1" gradientUnits="userSpaceOnUse" x1="0px" y1="0px" x2="200px" y2="150px">'
                + '<stop stop-color="#FFFFFF" stop-opacity=".3" offset="0"/>'
                + '<stop stop-color="#CED8D9" stop-opacity=".3" offset="1"/></linearGradient>',
            clickEvent: function (sender, args) { return clickUserId(args.id, args.data.name); },
            editable: false,
            dataSource: <%=jsonString%>
            });
    }
    
    
    function clickUserId(id, title)
    {
        var url = clienthdnfield.Get("UserInfo");
        var requestUrl = clienthdnfield.Get("RequestUrl");
        
        var param = "uID=" + id +"&UpdateUser=1";
        window.parent.UgitOpenPopupDialog(url, param, 'User Details: ' + title, '600px', '90', 0, escape(requestUrl));
        return false;
    }
    </script>	
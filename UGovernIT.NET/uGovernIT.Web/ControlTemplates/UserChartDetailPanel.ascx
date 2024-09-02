<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserChartDetailPanel.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.UserChartDetailPanel" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    var globalheight = '<%=Height%>';
    var width = '<%=Width%>';
    $(document).ready(function () {
         GetChartDetails(<%=ViewID%>,<%=panelId%>);
    });
   
</script>
<div id="divChartDetailPanel" runat="server" class="ChartDetails">
    <div class="row">
        <div class="col-md-12 py-3">

            <div id="divpiechart1_<%=ViewID%>_<%=panelId%>" class="ChartDetailsChildElement"></div>
            
        </div>

    </div>
</div>

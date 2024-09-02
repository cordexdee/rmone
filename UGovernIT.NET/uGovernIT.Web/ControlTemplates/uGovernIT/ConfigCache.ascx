<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigCache.ascx.cs" Inherits="uGovernIT.Web.ConfigCache" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .fleft {
        float: left;
    }

    .cache-container {
        float: left;
        width: 98%;
        padding: 10px;
    }

    .cache-item-label {
        float: left;
        padding-right: 10px;
        padding-left: 5px;
    }

    .cache-module-label {
        float: left;
        padding: 5px 0px;
        font-weight: bold;
        width: 98%;
    }

    .cache-item-container {
        float: left;
        width: 98%;
    }

    .cache-new-lable {
        padding-top: 3px;
    }

    .dx-datagrid-headers{
        color:#4b4b4b;
        font-weight:bold;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function showWait(message) {
        aspxConfigCacheLoading.SetText(message);
        aspxConfigCacheLoading.Show(); // Postback hides automatically, so no neConfigCacheed to call Hide() later
    }

    function NotifyUser(textMsg) {
        if (textMsg == null || textMsg == undefined)
            return false;

        if (textMsg == "")
            textMsg = "Entire cache has cleared.";
        else
            textMsg = textMsg + " cache has refreshed.";

        DevExpress.ui.notify({
            message: textMsg,
            type: "success",
            width: 300,
            position: "{ my: 'center', at: 'center', of: window }",
        });
    }




</script>

<div>
    <div class="cache-container" id="cacheContainer">
        <span>
            <dx:ASPxButton ID="btnClearCache" Text="Clear Cache" runat="server" CssClass="primary-blueBtn" OnClick="BtClearCache_Click">
                <ClientSideEvents Click="function(s,e){ showWait('Please Wait .. Flushing Cache');$('#cacheContainer').css('display','none'); }" />
            </dx:ASPxButton>
            &nbsp;
        </span>
        <span>
            <dx:ASPxButton ID="btnRefreshConfigurationCache" Text="Refresh Configuration Cache" runat="server" CssClass="primary-blueBtn" OnClick="btnRefreshConfigCache_Click">
                <ClientSideEvents Click="function(s,e){ showWait('Please Wait .. Refreshing Configuration Cache');$('#cacheContainer').css('display','none'); }" />
            </dx:ASPxButton>
            &nbsp;
        </span>
        <span>
            <dx:ASPxButton ID="btnRefreshTicketsCache" Text="Refresh Tickets Cache" runat="server" CssClass="primary-blueBtn" OnClick="btnRefreshTicketsCache_Click">
                <ClientSideEvents Click="function(s,e){ showWait('Please Wait .. Refreshing Tickets Cache');$('#cacheContainer').css('display','none'); }" />
            </dx:ASPxButton>
            &nbsp;
        </span>
        <span>
            <dx:ASPxButton ID="btnRefreshProfileCache" Text="Refresh Profile Cache" runat="server" CssClass="primary-blueBtn" OnClick="btnRefreshProfileCache_Click">
                <ClientSideEvents Click="function(s,e){ showWait('Please Wait .. Refreshing Profile Cache');$('#cacheContainer').css('display','none'); }" />
            </dx:ASPxButton>
            &nbsp;
        </span>
        <span>
            <dx:ASPxButton ID="btnRefreshEntireCache" Text="Refresh Entire Cache" runat="server" CssClass="primary-blueBtn" OnClick="btnRefreshEntireCache_Click">
                <ClientSideEvents Click="function(s,e){ showWait('Please Wait .. Refreshing Entire Cache');$('#cacheContainer').css('display','none'); }" />
            </dx:ASPxButton>
            &nbsp;
        </span>

        <div style="padding-top:4px;">
            <span>
                <asp:Button ID="btnRebuildStatistics" runat="server" Text="Rebuild Statistics" OnClick="btnRebuildStatistics_Click" OnClientClick="showWait('Please Wait .. Rebuilding User Statistics');$('#cacheContainer').css('display','none');" />
            </span>
         </div>
        <div style="padding-top: 4px; display: none;">
            <%--<span>
                <asp:Button ID="btnRearrangeTasks" runat="server" Text="Reorganize Tasks" OnClick="BtnRearrangeTasks_Click" OnClientClick="showWait('Please Wait .. Rearranging all tasks Summary');$('#cacheContainer').css('display','none');" /></span>
            <span>--%>
            <span>
                <asp:Button ID="btnRefreshDocumentsCache" runat="server" Text="Refresh Documents Cache" OnClick="btnRefreshDocumentsCache_Click" OnClientClick="showWait('Please Wait .. Refresh Documents Cache');$('#cacheContainer').css('display','none');" />&nbsp;</span>
            <span>
                <asp:Button ID="btnRefreshTasksCache" runat="server" Text="Refresh Tasks Cache" OnClick="btnRefreshTasksCache_Click" OnClientClick="showWait('Please Wait .. Refresh Tasks Cache');$('#cacheContainer').css('display','none');" />&nbsp;</span>
     
            <span>
                <asp:Button ID="btnRebuildDashboardSummary" runat="server" Text="Rebuild Dashboard" OnClick="BtRebuildDashboardSummary_Click" OnClientClick="showWait('Please Wait .. Rebuilding Dashboard Summary');$('#cacheContainer').css('display','none');" /></span>
            <span>
                <asp:Button ID="btnUpdateWorkflowHistory" runat="server" Text="Recate Workflow History" OnClick="btnUpdateWorkflowHistory_Click" OnClientClick="showWait('Please Wait .. Recalculating Workflow History Data');$('#cacheContainer').css('display','none');" /></span>
        </div>
</div>
</div>

<div>
    <dx:ASPxLoadingPanel ID="aspxConfigCacheLoading" ClientInstanceName="aspxConfigCacheLoading" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
    <asp:Panel CssClass="cache-container" ID="cacheDetailPanel" runat="server">
        <div id="accordion-container"></div>
    </asp:Panel>
</div>

<script type="text/template" id="acTitle">
    <h6>{%= Title %}: Last updated on {%= UpdatedOn ?  (new Date(UpdatedOn)).format("MMM dd, yyyy HH:mm tt") : "not mentioned"%} </h6>
</script>

<script type="text/template" id="acDetail">
    <div class="accodion-item">
        <div>
            <span>Total Records:  {%= Detail[0]["Total records"] %}</span>
        </div>
    </div>
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    DevExpress.setTemplateEngine("underscore");
    _.templateSettings = { interpolate: /\{%=(.+?)%\}/g, escape: /\{%-(.+?)%\}/g, evaluate: /\{%(.+?)%\}/g };

    function loadGrid(data) {
        var grid = $("<div/>").dxDataGrid({
            dataSource: data,
            width: 300,
            showBorders: true,
            columns: ['Module', 'Open Items', 'Closed Items']
        });
        return grid;
    }
    $(function () {

        $("#accordion-container").dxAccordion({
            dataSource: "/api/cache/GetCacheDetail",
            animationDuration: 300,
            collapsible: false,
            multiple: true,
            itemTitleTemplate: $("#acTitle"),
            itemTemplate: function (content, index) {
                if (content.Title == "Modules") {
                    return loadGrid(content.Detail);
                }
                else {
                    return _.template($("#acDetail").html())(content);
                }
            },
            onSelectionChanged: function (e) {

            }
        })

    });
</script>


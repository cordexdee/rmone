<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimesheetPendingApprovals.ascx.cs" Inherits="uGovernIT.Web.TimesheetPendingApprovals" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<script data-v="<%=UGITUtility.AssemblyVersion %>">
    //Function to bind timesheet when user click on any row from pendingStatus Grid
    function pendingStatusGrid_RowClickEvent(s, e) {
        s.GetRowValues(e.visibleIndex, 'Resource;StartDate', OnSelectedRowValues);
    }

    function OnSelectedRowValues(selectedValues) {
        var param = 'ResourceId=' + selectedValues[0] + '&WeekStartDt=' + selectedValues[1].toLocaleDateString();
        window.parent.UgitOpenPopupDialog('<%=viewTimesheetPath%>', param, 'Timesheet', '85%', '900', 0, "", true, true);
    }
</script>

<dx:ASPxGridView ID="pendingStatusGrid" runat="server" KeyFieldName="Resource" AutoGenerateColumns="false"
    Width="100%" ClientInstanceName="pendingStatusGrid" EnableViewState="false">
    <Columns>
        <dx:GridViewDataTextColumn FieldName="Resource" Visible="false">
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataDateColumn FieldName="StartDate" Caption="Start Date" Width="40%" HeaderStyle-Font-Bold="true"
            CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortOrder="Ascending">
            <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
        </dx:GridViewDataDateColumn>
        <dx:GridViewDataDateColumn FieldName="EndDate" Caption="End Date" Width="40%" HeaderStyle-Font-Bold="true"
            CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortOrder="Ascending">
            <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
        </dx:GridViewDataDateColumn>
    </Columns>
    <Settings ShowHeaderFilterButton="true" EnableFilterControlPopupMenuScrolling="true" VerticalScrollBarMode="Visible"
        VerticalScrollableHeight="400" />
    <SettingsPopup>
        <HeaderFilter Height="200" />
    </SettingsPopup>
    <SettingsBehavior AllowSelectByRowClick="true" AllowSort="true" />
    <SettingsPager Mode="ShowAllRecords" />
    <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
    <Styles>
        <AlternatingRow Enabled="True"></AlternatingRow>
    </Styles>
    <ClientSideEvents RowClick="function(s,e){ pendingStatusGrid_RowClickEvent(s,e);}" />
</dx:ASPxGridView>
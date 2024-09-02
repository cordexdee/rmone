

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTemplateView.ascx.cs" Inherits="uGovernIT.Web.ProjectTemplateView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function grid_CustomButtonClick(s, e) {
        if (e.buttonID != 'editButton') return;

        var key = s.GetRowKey(e.visibleIndex);
        openEditBox(key);
    }

    function openEditBox(ID)
    {
        var url = hdnInformation.Get("UpdateUrl") + "&ID=" + ID;
        var requestUrl = hdnInformation.Get("RequestUrl");
        window.UgitOpenPopupDialog(url, '', 'Edit Template', '600', '450', 0, requestUrl, true)
    }

    function expandAllTask() {
        grid.ExpandAll();
    }
    function collapseAllTask() {
        grid.CollapseAll();
    }
</script>

<dx:ASPxHiddenField ID="hdnInformation" runat="server" ClientInstanceName="hdnInformation"></dx:ASPxHiddenField>
<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row" style="margin-top: 5px;">
        <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
        <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
    </div>
    <div class="row" style="padding-bottom:25px;">
        <ugit:ASPxGridView EnableCallBacks="false" ID="grid" runat="server" AutoGenerateColumns="False"
           ClientInstanceName="grid" CssClass="customgridview homeGrid"
           Width="100%" KeyFieldName="ID"
             SettingsText-EmptyDataRow="No Allocations" OnRowDeleting="grid_RowDeleting" OnHtmlRowPrepared="grid_HtmlRowPrepared">
             <ClientSideEvents CustomButtonClick="grid_CustomButtonClick" />
            <Columns>
            </Columns>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                <Row CssClass="homeGrid_dataRow"></Row>
                <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
            </Styles>
            <Settings ShowGroupedColumns="false" />
            <SettingsPager Mode="ShowAllRecords" ShowEmptyDataRows="true"></SettingsPager>
            <SettingsEditing Mode="Inline" />
            <SettingsBehavior AllowSort="true" AllowDragDrop="false" ConfirmDelete="true" ColumnResizeMode="Disabled" AutoExpandAllGroups="true" />
            <SettingsPopup HeaderFilter-Height="200"></SettingsPopup>
            <SettingsText EmptyDataRow="No Template Found" ConfirmDelete="Are you sure you want to delete this template?" />
        </ugit:ASPxGridView>
    </div>
</div>
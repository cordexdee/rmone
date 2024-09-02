<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTeam.ascx.cs" Inherits="uGovernIT.Web.ProjectTeam" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .leftCellPadding {
        padding: 5px 1px 5px 7px !important;
        overflow: hidden;
        border-bottom: 1px solid #d9dae0;
        border-right: 1px solid #d9dae0;
    }
</style>

<div class="summaryCPRdb_tableWrap">
    <div class="summaryCPRdb_header">
        <asp:Label ID="lblHeader" runat="server" Text="Project Team" CssClass="summaryCPRdb_title"></asp:Label>
    </div>
    
    <dx:ASPxGridView ID="grdAllocation" AutoGenerateColumns="False" runat="server" Theme="DevEx"
        SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="100%" CssClass="summaryCPRdb_gridView">        
        <Columns>         
            <dx:GridViewDataColumn FieldName="AssignedTo" Caption="User" Width="50%"
                CellStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
            </dx:GridViewDataColumn>
            <dx:GridViewDataColumn FieldName="Type" Caption="User Type"  Width="49%"
                CellStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
            </dx:GridViewDataColumn>
        </Columns>

        <Settings ShowFooter="false" ShowHeaderFilterButton="false" ShowColumnHeaders="false"/>
        <SettingsBehavior AllowSort="false" AllowDragDrop="false" AutoExpandAllGroups="true" />
        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
        <Styles AlternatingRow-CssClass="ms-alternatingstrong">
            <Row HorizontalAlign="Center" CssClass="summaryCPRdb_gridView_dataRow"></Row>
            <GroupRow Font-Bold="true"></GroupRow>
            <Header Font-Bold="true" HorizontalAlign="Center"></Header>
            <%--<AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>--%>
            <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            <Footer Font-Bold="true" HorizontalAlign="Center"></Footer>
        </Styles>
    </dx:ASPxGridView>
</div>

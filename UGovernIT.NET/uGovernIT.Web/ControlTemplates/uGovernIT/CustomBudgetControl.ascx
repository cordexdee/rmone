<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomBudgetControl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.CustomBudgetControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>

<style type="text/css">
    .ms-alternating {
    }

    .borderTop {
        border-top: solid 1px;
    }
    .ugitaccent2lighter {
        background-color: #81E4FF;
        color:black;
    }
</style>
<asp:Panel runat="server" ID="pnlProjectBudget" CssClass='categories-list' style="margin-top: 10px;">
    <%-- Actual Information--%>
    <asp:Label ID="lblProjectMessage" runat="server" Text="There is no item to show in the list." ForeColor="Red" Visible="false"></asp:Label>
    <dx:ASPxGridView ID="gvProjectBudget" runat="server" AutoGenerateColumns="false" ClientInstanceName="gvProjectBudget" 
        SettingsBehavior-AutoExpandAllGroups="true" Width="100%" KeyFieldName="ID"  SettingsBehavior-AllowGroup ="true">
        <Columns>
            <dx:GridViewDataColumn Caption ="Module" FieldName="ModuleNameLookup" Visible="false"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Project ID" FieldName="TicketId"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Budget Item" FieldName="BudgetItem"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Title" FieldName="BudgetDescription"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn Caption ="Start Date" FieldName="AllocationStartDate">
                <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption ="End Date" FieldName="AllocationEndDate">
                <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption ="Amount" FieldName="BudgetAmount">
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>           
        </Columns>

        <%--<GroupSummary>
            <dx:ASPxSummaryItem FieldName="BudgetAmount" Visible="true" SummaryType ="Sum" ShowInGroupFooterColumn="Amount" DisplayFormat="<b>{0:C0}<b/>"/>
        </GroupSummary>--%>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="BudgetAmount" Visible="true" SummaryType ="Sum"/>
        </TotalSummary>
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
        </Styles>
        <Settings ShowFooter="true" />

    </dx:ASPxGridView>
</asp:Panel>

<asp:Panel runat="server" ID="pnlNonProjectBudget" CssClass='categories-list' style="margin-top: 10px;">
    <%-- Actual Information--%>
    <asp:Label ID="lblNonProjectMessage" runat="server" Text="There is no item to show in the list." ForeColor="Red" Visible="false"></asp:Label>

        <dx:ASPxGridView ID="gvNonProjectBudget" runat="server" AutoGenerateColumns="false" ClientInstanceName="gvNonProjectBudget" 
        SettingsBehavior-AutoExpandAllGroups="true" Width="100%" KeyFieldName="ID">
        <Columns>
            <dx:GridViewDataColumn Caption ="Budget Item" FieldName="Title" ></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Department" FieldName="DepartmentTitle"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn Caption ="Start Date" FieldName="AllocationStartDate">
                <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption ="End Date" FieldName="AllocationEndDate">
                <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption ="Amount" FieldName="BudgetAmount">
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>           
        </Columns>

        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="BudgetAmount" Visible="true" SummaryType ="Sum"/>
        </TotalSummary>
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
        </Styles>
        <Settings ShowFooter="true" />

    </dx:ASPxGridView>

</asp:Panel>

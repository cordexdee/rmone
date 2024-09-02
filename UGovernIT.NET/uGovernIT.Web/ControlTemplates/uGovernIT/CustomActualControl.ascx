<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomActualControl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.CustomActualControl" %>
 
<%@ Import Namespace="uGovernIT.Utility" %>
 
<style type="text/css">
    .borderTop {
        border-top: solid 1px;
    }
</style>
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
<asp:Panel runat="server" ID="pnlProjectActual" CssClass='categories-list' style="margin-top: 10px;">
    <%-- Actual Information--%>
    <asp:Label ID="lblProjectHeader" runat="server" Text=""></asp:Label>
        <dx:ASPxGridView ID="gvProjectActual" runat="server" AutoGenerateColumns="false" ClientInstanceName="gvProjectActual" 
        SettingsBehavior-AutoExpandAllGroups="true" Width="100%" KeyFieldName="ID"  SettingsBehavior-AllowGroup ="true">
        <Columns>
            <dx:GridViewDataColumn Caption ="Module" FieldName="ModuleNameLookup" Visible="false"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Project ID" FieldName="TicketId"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Budget Item" FieldName="BudgetItem"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Title" FieldName="Title"></dx:GridViewDataColumn>
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

<asp:Panel runat="server" ID="pnlNonProjectActual" CssClass='categories-list'>
    <%-- Actual Information--%>
    <asp:Label ID="lblnonProjectHeader" runat="server" Text=""></asp:Label>
        <dx:ASPxGridView ID="gvNonProjectActual" runat="server" AutoGenerateColumns="false" ClientInstanceName="gvNonProjectActual" 
        SettingsBehavior-AutoExpandAllGroups="true" Width="100%" KeyFieldName="ID">
        <Columns>
            <dx:GridViewDataColumn Caption ="Budget Item" FieldName="ITGBudgetLookup" ></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Title" FieldName="Title"></dx:GridViewDataColumn>
            <dx:GridViewDataColumn Caption ="Vendor" FieldName="VendorLookup"></dx:GridViewDataColumn>
            <dx:GridViewDataTextColumn Caption ="Start Date" FieldName="BudgetStartDate">
                <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption ="End Date" FieldName="BudgetEndDate">
                <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption ="Amount" FieldName="ActualCost">
                <PropertiesTextEdit DisplayFormatString="c" />
            </dx:GridViewDataTextColumn>           
        </Columns>

        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="ActualCost" Visible="true" SummaryType ="Sum"/>
        </TotalSummary>
        <Styles>
            <Row CssClass="homeGrid_dataRow"></Row>
            <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            <GroupRow CssClass="homeGrid-groupRow"></GroupRow>
        </Styles>
        <Settings ShowFooter="true" />

    </dx:ASPxGridView>

</asp:Panel>
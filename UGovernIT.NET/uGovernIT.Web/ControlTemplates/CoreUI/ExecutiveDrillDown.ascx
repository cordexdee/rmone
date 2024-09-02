<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExecutiveDrillDown.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.CoreUI.ExecutiveDrillDown" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .strip-primary p {
        font-weight: 500;
        font-size: 14px;
    }

    .cardHeading-row {
        background-color: #ff9d66;
        height: 30px !important;
        width: 100% !important;
        text-align: left;
        color: #fff;
        padding-top: 6px;
        padding-left: 30px;
        background-size: 20px;
        background-position-x: 5px !important;
        font-family: 'Poppins', sans-serif !important;
        font-size: 12px;
    }
</style>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxcvFLECW {
        text-align: center !important;
    }

    .pendingTask-heading .cardView-label, .completedTask-heading .cardView-label {
        margin-bottom: 10px;
    }

    .cardView-wrap .dxcvEmptyCard_UGITNavyBlueDevEx {
        width: auto;
        height: 98px;
    }

    .dxtcLite_UGITNavyBlueDevEx.dxtcLite-row .dxtc-leftIndent, .dxtcLite_UGITNavyBlueDevEx.dxtcLite-row .dxtc-rightIndent {
        display: none !important;
    }

    .CRMstatusGrid_headerRow {
        padding-left: 6px;
        padding-right: 6px;
    }

    .cardTask-row {
        height: 70px;
        text-align: left;
        padding-top: 10px;
        font-family: 'Poppins', sans-serif !important;
        font-size: 12px;
        display: block !important;
        width: 100% !important;
        padding-left: 30px;
    }

    .cardrow{
        float: left;
        font-size: 12px;
        font-weight: 500;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function CardClick(s, e) {
        debugger;
        var key = s.GetItemKey(e.visibleIndex)
        const moduleName = key.split("-")[0];
        if (moduleName == "CNS") {
            const path = '<%=CNSPath%>' + "?TicketId=" + key
            window.parent.UgitOpenPopupDialog(path, '', key, 90, 90, false, escape(window.location.href));
        } else if (moduleName == "CPR") {
            const path = '<%=CPRPath%>' + "?TicketId=" + key
            window.parent.UgitOpenPopupDialog(path, '', key, 90, 90, false, escape(window.location.href));
        } else if (moduleName == "OPM") {
            const path = '<%=OPMPath%>' + "?TicketId=" + key
            window.parent.UgitOpenPopupDialog(path, '', key, 90, 90, false, escape(window.location.href));
        }
    }

    
</script>
<div class="row" id="divThingtodo" runat="server">
    <div class="bg-primary p-2 strip-primary">
        <p class="mb-0">Things to Do</p>
    </div>

    <dx:ASPxCardView ID="cardThingToDo" runat="server" Width="100%" KeyFieldName="ID">

        <SettingsPopup>
            <FilterControl AutoUpdatePosition="False"></FilterControl>
        </SettingsPopup>

        <SettingsExport ExportSelectedCardsOnly="False"></SettingsExport>
        <Columns>
            <dx:CardViewColumn FieldName="ItemOrder" Caption="#" SortOrder="Ascending" />
            <dx:CardViewColumn FieldName="Title" Caption="Title" />
            <dx:CardViewColumn FieldName="StartDate" Caption="Start Date" >
            </dx:CardViewColumn>
            <dx:CardViewColumn FieldName="DueDate" Caption="End Date" >
            </dx:CardViewColumn>
        </Columns>
        <Templates>
            <Card>
                <div>
                    <div class="cardHeading-row">
                        <dx:ASPxLabel runat="server" Font-Size="14px" Text='<%# Eval("Title") %>' />
                    </div>
                    <div class="cardTask-row">
                        <div class="cardrow">Start Date: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("StartDate") %>' /></div>
                        <br />
                        <div class="cardrow">Due Date: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("DueDate") %>' /></div>
                        
                    </div>
                </div>
            </Card>
        </Templates>
        <Settings VerticalScrollableHeight="150" />
        <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
        <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="4" EndlessPagingMode="OnClick" AlwaysShowPager="true" SettingsFlowLayout-ItemsPerPage="6" />
        <Styles>
            <Card Width="25%" Height="90%"></Card>

        </Styles>
    </dx:ASPxCardView>
    <%--<ugit:ASPxGridView ID="grdThingToDo" runat="server" KeyFieldName="ID" CssClass="customgridview homeGrid mb-3 mt-0" AutoGenerateColumns="false" Width="100%">
        <Columns>
                <dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="#" SortOrder="Ascending" CellStyle-HorizontalAlign="Center" Width="1px"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="300px"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="StartDate" Caption="Start Date" PropertiesTextEdit-DisplayFormatString="MM/dd/yyyy" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="DueDate" Caption="End Date" PropertiesTextEdit-DisplayFormatString="MM/dd/yyyy" CellStyle-HorizontalAlign="Center">
                </dx:GridViewDataTextColumn>
                
            </Columns>
            <Styles>
                <Row CssClass="CRMstatusGrid_row"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
            <SettingsBehavior AllowSort="true" />
    </ugit:ASPxGridView>--%>
</div>
<div class="row" id="divPipeline" runat="server">
    <div class="bg-primary p-2 strip-primary">
        <p class="mb-0">Pipeline</p>
    </div>
    <dx:ASPxCardView ID="cardPipeline" ClientInstanceName="cardPipeline" runat="server" Width="100%" OnCardLayoutCreated="cardPipeline_CardLayoutCreated1" KeyFieldName="TicketId">
            <SettingsPopup>
            <FilterControl AutoUpdatePosition="False"></FilterControl>
        </SettingsPopup>
        <ClientSideEvents CardClick="CardClick" />
        <SettingsExport ExportSelectedCardsOnly="False"></SettingsExport>
        <Columns>
            <dx:CardViewColumn FieldName="ProjectId" Caption="#" SortOrder="Ascending" />
            <dx:CardViewColumn FieldName="Title" Caption="Title" />
            <dx:CardViewColumn FieldName="ApproxContractValue" Caption="Project Cost" >
            </dx:CardViewColumn>
        </Columns>
        <Templates>
            <Card>
                <div>
                    <div class="cardHeading-row">
                        <dx:ASPxLabel runat="server" Font-Size="14px" Text='<%# Eval("ProjectId") %>' />
                    </div>
                    <div class="cardTask-row">
                        <div class="cardrow">Title: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("Title") %>' /></div>
                        <br />
                        <div class="cardrow">Project Cost: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("ApproxContractValue") %>' /></div>
                        
                    </div>
                </div>
            </Card>
        </Templates>
        <Settings VerticalScrollableHeight="150" />
        <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
        <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="4" EndlessPagingMode="OnClick" AlwaysShowPager="true" SettingsFlowLayout-ItemsPerPage="6" />
        <Styles>
            <Card Width="25%" Height="90%"></Card>

        </Styles>
        
    </dx:ASPxCardView>
    <%--<ugit:ASPxGridView ID="grdPipeline" runat="server" KeyFieldName="ID" CssClass="customgridview homeGrid mb-3 mt-0" AutoGenerateColumns="false" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ProjectId" Caption="#" SortOrder="Ascending" CellStyle-HorizontalAlign="Center" Width="1px"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="300px"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProjectManagerUser" Caption="Project Manager" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ApproxContractValue" Caption="Project Cost" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StudioLookup" Caption="Studio" CellStyle-HorizontalAlign="Center">
            </dx:GridViewDataTextColumn>

        </Columns>
        <Styles>
            <Row CssClass="CRMstatusGrid_row"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
        </Styles>
        <Settings ShowHeaderFilterButton="true" />
        <SettingsBehavior AllowSort="true" />
    </ugit:ASPxGridView>--%>
</div>
<div class="row" id="divClosed" runat="server">
    <div class="bg-primary p-2 strip-primary">
        <p class="mb-0">Closed</p>
    </div>
    <dx:ASPxCardView ID="cardClosed" runat="server"  Width="100%" KeyFieldName="TicketId">
        <ClientSideEvents CardClick="CardClick" />
        <Columns>
            <dx:CardViewColumn FieldName="ProjectId" Caption="Title" />
            <dx:CardViewColumn FieldName="Title" Caption="Title" />
            <dx:CardViewColumn FieldName="ApproxContractValue" Caption="Project Cost" />
        </Columns>
       <Templates>
            <Card>
                <div>
                    <div class="cardHeading-row">
                        <dx:ASPxLabel runat="server" Font-Size="14px" Text='<%# Eval("ProjectId") %>' />
                    </div>
                    <div class="cardTask-row">
                        <div class="cardrow">Title: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("Title") %>' /></div>
                        <br />
                        <div class="cardrow">Project Cost: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("ApproxContractValue") %>' /></div>
                        
                    </div>
                </div>
            </Card>
        </Templates>
        <Settings VerticalScrollableHeight="150" />
        <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
        <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="4" EndlessPagingMode="OnClick" AlwaysShowPager="true" SettingsFlowLayout-ItemsPerPage="6" />
        <Styles>
            <Card Width="25%" Height="90%"></Card>

        </Styles>
    </dx:ASPxCardView>
    <%--<ugit:ASPxGridView ID="grdClosed" runat="server" KeyFieldName="ID" CssClass="customgridview homeGrid mb-3 mt-0" AutoGenerateColumns="false" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ProjectId" Caption="#" SortOrder="Ascending" CellStyle-HorizontalAlign="Center" Width="1px"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="300px"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProjectManagerUser" Caption="Project Manager" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ApproxContractValue" Caption="Project Cost" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StudioLookup" Caption="Studio" CellStyle-HorizontalAlign="Center">
            </dx:GridViewDataTextColumn>

        </Columns>
        <Styles>
            <Row CssClass="CRMstatusGrid_row"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
        </Styles>
        <Settings ShowHeaderFilterButton="true" />
        <SettingsBehavior AllowSort="true" />
    </ugit:ASPxGridView>--%>
</div>
<div class="row" id="divOnGoing" runat="server">
    <div class="bg-primary p-2 strip-primary">
        <p class="mb-0">On Going</p>
    </div>
    <dx:ASPxCardView ID="cardOnGoing"  runat="server" Width="100%" KeyFieldName="TicketId">
        <ClientSideEvents CardClick="CardClick" />
         <Columns>
            <dx:CardViewColumn FieldName="ProjectId" Caption="Title" />
            <dx:CardViewColumn FieldName="Title" Caption="Title" />
            <dx:CardViewColumn FieldName="ApproxContractValue" Caption="Project Cost" />
        </Columns>
        <Templates>
            <Card>
                <div>
                    <div class="cardHeading-row">
                        <dx:ASPxLabel runat="server" Font-Size="14px" Text='<%# Eval("ProjectId") %>' />
                    </div>
                    <div class="cardTask-row">
                        <div class="cardrow">Title: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("Title") %>' /></div>
                        <br />
                        <div class="cardrow">Project Cost: <dx:ASPxLabel runat="server" Font-Bold="true" Font-Size="13px" Text='<%# Eval("ApproxContractValue") %>' /></div>
                        
                    </div>
                </div>
            </Card>
        </Templates>
        <Settings VerticalScrollableHeight="150" />
        <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
        <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="4" EndlessPagingMode="OnClick" AlwaysShowPager="true" SettingsFlowLayout-ItemsPerPage="6" />
        <Styles>
            <Card Width="25%" Height="90%"></Card>
        </Styles>
    </dx:ASPxCardView>
    <%--<ugit:ASPxGridView ID="grdOnGoing" runat="server" KeyFieldName="ID" CssClass="customgridview homeGrid mb-3 mt-0" AutoGenerateColumns="false" Width="100%">
        <Columns>
            <dx:GridViewDataTextColumn FieldName="ProjectId" Caption="#" SortOrder="Ascending" CellStyle-HorizontalAlign="Center" Width="1px"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="300px"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ProjectManagerUser" Caption="Project Manager" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="ApproxContractValue" Caption="Project Cost" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn FieldName="StudioLookup" Caption="Studio" CellStyle-HorizontalAlign="Center">
            </dx:GridViewDataTextColumn>

        </Columns>
        <Styles>
            <Row CssClass="CRMstatusGrid_row"></Row>
            <Header CssClass="homeGrid_headerColumn"></Header>
        </Styles>
        <Settings ShowHeaderFilterButton="true" />
        <SettingsBehavior AllowSort="true" />
    </ugit:ASPxGridView>--%>
</div>

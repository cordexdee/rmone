<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectByDueDate_Viewer.ascx.cs" Inherits="uGovernIT.DxReport.ProjectByDueDate_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<style type="text/css">
    .projectGroup {
        display: table;
        margin: 0 auto;
    }

    .bordercss {
        border-left: 4px solid rgb(12, 197, 219);
        border-right: 4px solid rgb(12, 197, 219);
        border-bottom: 4px solid rgb(12, 197, 219);
    }

    .setascurve {
        border-top-right-radius: 4em;
        background: #ffffff;
        border-color: #216098;
        cursor: pointer;
    }

    .grouped {
        width: 300px;
    }

    .data-cell {
        margin-bottom: 0px;
        text-align: center;
    }

        .data-cell .gray {
            padding: 4px;
            background-color: #f3f3f3;
        }


        .data-cell .red {
            padding: 4px;
            background-color: #FDCACA;
        }

        .data-cell .green {
            padding: 4px;
            background-color: #a2e6a2;
        }

        .data-cell .yellow {
            padding: 4px;
            background-color: yellow;
        }

    .titlecss {
        font-weight: bold;
        padding: 10px 0px 10px 0px;
        text-align: center;
    }

    .totalrowcss {
        text-align: center;
    }

    .circle-cell {
        fill: #847b7b;
    }

    .circle-text {
        fill: #ffffff;
    }

    .layoutgroup {
        padding: 0px 10px 10px 10px;
        width: 300px;
    }

    .layoutgroup-cell {
        padding: 0px 1px;
    }

    .scrum-title {
        font-weight: bold;
    }

    .backward-icon {
        float: left;
        padding-top: 20px;
        cursor: pointer;
    }

    .child-container {
        cursor: pointer;
        border-color: #f3f3f3;
    }

    .pnlparentcss {
        width: 100%;
        height: 435px;
        padding: 0px 0px 0px 10px;
    }

    .flipblue {
        background-color: rgb(12, 197, 219);
        float: left;
    }

    .flipgray {
        background-color: #f3f3f3;
        padding: 0px;
        float: left;
        width: 100%;
    }

    .flipgreen {
        background-color: #a2e6a2;
    }

    .flipred {
        background-color: #FDCACA;
    }

    .flipyellow {
        background-color: yellow;
    }

    .flipgray > div > span {
        padding: 5px;
        width: 100%;
        float: left;
    }

    .flipblue > span {
        padding: 5px;
        width: 100%;
        float: left;
    }

    .flipalign {
        float: right;
        margin-top: -54px;
        margin-right: 26px;
        cursor: pointer;
    }

    .checkbox-Messagecontainer {
        margin-top: 4px;
        text-align: center;
        width: 100%;
    }

    .checkbox-container {
        display: inherit;
        margin: 0 auto;
        border: 1px solid;
        width: 360px;
        height: 28px;
        border-color: #f3f3f3;
        padding: 0px 0px 0px 15px;
    }
</style>

<script type="text/javascript">
    function showprojects(s, e) {
        crdviewgroup1.GetCardValues(e.visibleIndex, 'Title;previousDate;nextDate', getvalues)
    }
    function getvalues(selectedcardValues) {
        if (selectedcardValues.length > 0) {
            var datafilter = 'CurrentProjects';
            if ($('#<%=FilterCheckBox_pa.ClientID%>').is(":checked"))
                datafilter = datafilter + '_PendingApprovalNPRs';
            if ($('#<%=FilterCheckBox_apr.ClientID%>').is(":checked"))
                datafilter = datafilter + '_ApprovedNPRs';

            var url = '<%=filterUrl%>';
            url = url + '&datafilter=' + datafilter;
            if (selectedcardValues[1] == null && selectedcardValues[2] == null)
                url = url + '&dayplan=true';
            else
                url = url + '&dayplan=true' + '&previousDate=' + selectedcardValues[1] + '&nextDate=' + selectedcardValues[2];

            window.parent.UgitOpenPopupDialog(url, "", selectedcardValues[0], 90, 90);
        }
        else
            return;
    }
</script>

<asp:Panel ID="pnlShowBS" runat="server" Width="100%">
    <div class="checkbox-container">
        <div>
            <asp:HiddenField ID="hdnkeepFileters" runat="server" />
            <table>
                <tr>
                    <td style="padding: 5px;">
                        <asp:CheckBox runat="server" ID="FilterCheckBox_pa" CssClass="itg-view" Text="Pending Approval" AutoPostBack="true" GroupName="ITGRadio"
                            Checked="false" OnCheckedChanged="FilterListView" />
                    </td>
                    <td style="padding: 5px;">
                        <asp:CheckBox runat="server" ID="FilterCheckBox_apr" CssClass="itg-view" Text="Ready To Start" GroupName="ITGRadio"
                            Checked="false" OnCheckedChanged="FilterListView" AutoPostBack="true" />
                    </td>
                    <td style="padding: 5px;">
                        <asp:CheckBox runat="server" ID="FilterCheckBox_cp" CssClass="itg-view" Text="In Progress" AutoPostBack="true" GroupName="ITGRadio"
                            Checked="true" OnCheckedChanged="FilterListView" />
                    </td>
            </table>
        </div>
    </div>
    <div style="font-family:inherit">
    <svg width="100%" height="80px" >
      <defs>
        <marker id="arrow" markerWidth="4" viewBox="0 0 10 10" markerHeight="4" refx="0" refy="3" orient="auto" markerUnits="strokeWidth">
          <path d="M0,0 L0,6 L4,3 z" fill="#03AEF3" />
        </marker>
      </defs>
      <line x1="15" y1="50" x2="965" y2="50" stroke="#03AEF3" stroke-width="25" marker-end="url(#arrow)" />
    </svg>
    </div>
    <div style="padding:0px 0px 15px 15px;">
        <div>
            <dx:ASPxCardView ID="crdviewgroup1" runat="server" KeyFieldName="days" CssClass="child-container" ClientInstanceName="crdviewgroup1" OnCustomColumnDisplayText="crdviewgroup1_CustomColumnDisplayText" SettingsDataSecurity-AllowReadUnlistedFieldsFromClientApi="true">
                <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="2" SettingsTableLayout-ColumnCount="3"></SettingsPager>
                <ClientSideEvents CardClick="function(s,e){showprojects(s,e);}" />
                <Styles>
                    <Card CssClass="setascurve grouped">
                    </Card>
                </Styles>
                <SettingsText EmptyCard="No data found" />
                <Columns>
                    <dx:CardViewColumn FieldName="BusinessStrategies">
                                <DataItemTemplate>
                                    <div>
                                        <div>
                                            <div>
                                                <asp:Panel ID="pnlBusinessStrategiesCount" runat="server">
                                                    <svg width="60" height="60">
                                                        <circle cx="30" cy="30" r="20" class="circle-cell"></circle>
                                                        <text id="txtBusinessStrategiesTotal" text-anchor="middle" x="30" y="34" class="circle-text" runat="server"><%# Eval("projcount") %></text>
                                                    </svg>
                                                </asp:Panel>

                                            </div>
                                            <div><span id="spnBusinessStrategies" runat="server">Projects</span></div>
                                        </div>
                                    </div>
                                </DataItemTemplate>
                            </dx:CardViewColumn>
                    <dx:CardViewTextColumn FieldName="Title" Caption="Project Description"></dx:CardViewTextColumn>
                    <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                    <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                    <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                    <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                </Columns>
                <CardLayoutProperties ColCount="2">
                    <Items>
                        <dx:CardViewColumnLayoutItem ColumnName="BusinessStrategies" CssClass=" titlecss totalrowcss" ShowCaption="False" ColSpan="2"></dx:CardViewColumnLayoutItem>
                        <dx:CardViewColumnLayoutItem ColumnName="Title" HorizontalAlign="Center" ShowCaption="False" CssClass=" titlecss" ColSpan="2">
                        </dx:CardViewColumnLayoutItem>
                        <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AmountLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                        <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="MonthLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                        <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="Issues" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                        <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="RiskLevel" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                    </Items>
                    <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                    </Styles>
                </CardLayoutProperties>
            </dx:ASPxCardView>
        </div>
    </div>
</asp:Panel>

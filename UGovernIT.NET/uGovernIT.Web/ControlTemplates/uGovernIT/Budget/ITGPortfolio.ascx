<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITGPortfolio.ascx.cs"
    Inherits="uGovernIT.Web.ITGPortfolio" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    fieldset {
        border: 1px solid silver;
        margin: 0 2px;
        padding: 0.35em 0.625em 0.75em;
    }

    legend {
        display: unset;
        width: unset;
        padding: unset;
        margin-bottom: unset;
        font-size: unset;
        line-height: unset;
        color: black;
        border: unset;
        border-bottom: unset;
    }

    .MaxHeight {
        max-height: inherit;
    }

    .bold {
        font-weight: bold;
    }

    .rdAlign label {
        vertical-align: bottom;
    }

    .itg-view label {
        position: relative;
    }

    .action-container {
        background: none repeat scroll 0 0 #FFFFAA;
        border: 1px solid #FFD47F;
        float: left;
        padding: 1px 5px 0;
        position: absolute;
        z-index: 800;
        margin-top: -16px;
        margin-left: 3px;
        left: 5px;
    }

    .hide {
        display: none;
    }

    .makeMeHidden {
        display: none;
    }

    .cssviews {
        float: left;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">



    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(initializeRequest);
    prm.add_pageLoaded(pageLoaded);

    function initializeRequest(sender, args) {

        itgLoading.Show();
    }

    function pageLoaded(sender, args) {
        try {
            var panels = args.get_panelsUpdated();
            if (panels.length > 0) {
                itgLoading.Hide();
            }
        }
        catch (ex) { }
    }

    //Action button hide show: start
    function showTasksActions(trObj) {

        $(trObj).find(".action-container").show();
    }

    function hideTasksActions(trObj) {
        $(trObj).find(".action-container").hide();
    }
    //Action button hide show: end

    function nprActionBtnsHandle(s, moduleName, action, title, ticketPublicID) {
        var param = "moduleName=" + moduleName + "&command=" + action + "&ticketPublicId=" + ticketPublicID;
        var width = "450px";
        var height = "250px";
        if (action == "approve") {
            width = "390px";
            height = "150px";
        }
        window.parent.UgitOpenPopupDialog('<%=TicketActionUrl%>', param, ticketPublicID + ': ' + title, width, height, 0, escape("<%= Request.Url.AbsolutePath %>"));

    }
    function loadProjectPorFolioReportControl() {
        var ticketids = '<%=strTicketIDs%>';
        var allocationType = '<%=allocationType%>';
        var year = '<%=year%>';
        var isperntge = '<%=isPercentage%>';
        var yeartype = '<%=yearType%>';
        var isApprovedProjectRequests = '<%=isApprovedProjectRequests%>';

        var isPendingApproval = '<%=isPendingApproval%>';
        var params = "ticketids=" + ticketids + "&year=" + year + "&yeartype=" + yeartype + "&allocationtype=" + allocationType + "&isPercentage=" + isperntge;
<%--        var url = '<%= UGITUtility.GetAbsoluteURL("/Layouts/ugovernit/delegatecontrol.aspx?isdlg=1&control=projectporfolioreportviewer") %>';--%>
        var url = '<%=reporturl%>'+"?reportName=projectporfolioreport";
        window.parent.UgitOpenPopupDialog(url, params, 'Project Portfolio Report', '100', '100', 0);

    }


    $(document).ready(function () {
        InitalizejQuery();
    });

    function InitalizejQuery() {
        var sourceKey;
        var targetKey;
        var sourceIndex;
        $(".sortable").find("tbody").sortable({
            start: function (event, ui) {
                sourceKey = $(ui.item[0]).find("input[type='hidden']").val();
                sourceIndex = ui.item[0].rowIndex;
            }
        });

        $(".sortable").find("tbody").sortable({
            stop: function (event, ui) {
                targetKey = $(ui.item[0]).next().find("input[type='hidden']").val();
                if (sourceIndex != 0) {
                    itgGridClientInstance.PerformCallback("DRAGROW|" + sourceKey + '|' + targetKey);
                }
            }
        });


    }

    function RefreshInstance(s, e) {

        if (s.cpIsCustomCallback) {
            s.cpIsCustomCallback = false;
            s.Refresh();
        }
    }
    function selectionChanged(s, e) {

        $("#<%=hdnBtnSelectionChanged.ClientID%>").trigger("click");

    }
</script>

<div id="scriptPanel" runat="server" visible="false">
    <script type="text/javascript">
        loadProjectPorFolioReportControl();
    </script>
</div>
<fieldset id="fldsetNPRResource" runat="server">
    <legend style="font-weight: bold;">Project Summary</legend>
    <div style="min-height: 500px">
        <div style="float: left; width: 100%; padding-top: 5px;">
            <dx:ASPxLoadingPanel ID="itgLoading" ClientInstanceName="itgLoading" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>
            <asp:HiddenField ID="filterId" runat="server" />
            <div style="width: 100%; text-align: center;">

                <div style="float: left; padding-bottom: 10px; padding-top: 4px; padding-right: 4px;">
                    <span style="float: left; padding-right: 2px;">Group by: </span>
                    <asp:DropDownList AutoPostBack="true" CssClass="cssviews" runat="server" ID="ddlviewtype" OnSelectedIndexChanged="ddlviewtype_SelectedIndexChanged">
                        <Items>
                            <asp:ListItem Value="0" Text="Priority" />
                            <asp:ListItem Value="1" Text="Project Type " />
                            <asp:ListItem Value="2" Text="Business Initiative " />
                        </Items>
                    </asp:DropDownList>
                </div>


                <div style="display: inline-block; margin: 0 auto; border: 2px ridge; width: 675px;" class="ms-selectednav">
                    <table class="rdAlign">
                        <tr>
                            <td style="padding: 5px;">
                                <asp:CheckBox runat="server" ID="FilterCheckBox_pa" CssClass="itg-view" Text="Pending Approval" AutoPostBack="true" GroupName="ITGRadio"
                                    Checked="false" OnCheckedChanged="FilterListView" onclick="itgLoading.Show()" />
                            </td>
                            <td style="padding: 5px;">
                                <asp:CheckBox runat="server" ID="FilterCheckBox_rop" CssClass="itg-view" Text="On-Hold" GroupName="ITGRadio"
                                    Checked="false" AutoPostBack="true" OnCheckedChanged="FilterListView" onclick="itgLoading.Show()" />
                            </td>
                            <td style="padding: 5px;">
                                <asp:CheckBox runat="server" ID="FilterCheckBox_apr" CssClass="itg-view" Text="Approved Project Requests" GroupName="ITGRadio"
                                    Checked="false" AutoPostBack="true" OnCheckedChanged="FilterListView" onclick="itgLoading.Show()" />
                            </td>
                            <td style="padding: 5px;">
                                <asp:CheckBox runat="server" ID="FilterCheckBox_cp" CssClass="itg-view" Text="Current Projects" AutoPostBack="true" GroupName="ITGRadio"
                                    Checked="true" OnCheckedChanged="FilterListView" onclick="itgLoading.Show()" />
                            </td>
                            <td style="padding: 5px;">
                                <asp:CheckBox runat="server" ID="FilterCheckBox_cpp" CssClass="itg-view" Text="Completed Projects" GroupName="ITGRadio"
                                    Checked="false" AutoPostBack="true" OnCheckedChanged="FilterListView" onclick="itgLoading.Show()" />
                            </td>
                    </table>
                </div>


                <div style="float: right; width: 365px; border: none; height: 28px; padding-top: 4px;" class="ms-selectednav">
                    <table class="rdAlign">
                        <tr>
                            <td>
                                <div style="padding-bottom: 4px;">
                                    <asp:RadioButtonList ID="rdbAllocationType" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdbAllocationType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="FTE" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Percentage" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" onchange="itgLoading.Show()" OnSelectedIndexChanged=" ddlYear_SelectedIndexChanged">
                                    <asp:ListItem Text="Calendar Year" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Fiscal Year" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <div style="float: left; margin-left: 2px;">
                                    <span style="padding-right: 5px;">
                                        <asp:ImageButton ImageUrl="/Content/images/Previous16x16.png" ID="previousYear" ToolTip="Prevoius Year" runat="server" OnClick="previousYear_Click" OnClientClick="itgLoading.Show()" />
                                    </span>
                                    <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
                                    <span style="padding-left: 5px;">
                                        <asp:ImageButton ImageUrl="/Content/images/Next16x16.png" ID="nextYear" ToolTip="Next Year" runat="server" OnClick="nextYear_Click" OnClientClick="itgLoading.Show()" />
                                    </span>
                                </div>

                            </td>
                            <td>
                                <div>
                                    <span style="padding-left: 5px;">
                                        <asp:ImageButton ImageUrl="/Content/images/Reports_16x16.png" ID="projectPorFolioReport" OnClientClick="itgLoading.Show()" ToolTip="Project Portfolio Report" runat="server" OnClick="projectPorFolioReport_Click" />
                                    </span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>


            </div>
            <%--<asp:Panel runat="server" ID="portfolioContainer" CssClass="MaxHeight" Width="100%">--%>
                <asp:Button ID="hdnBtnSelectionChanged" runat="server" Style="display: none" OnClick="hdnBtnSelectionChanged_Click" />
                <ugit:ASPxGridView ID="itgGrid" CssClass="sortable" OnAfterPerformCallback="itgGrid_AfterPerformCallback" OnCustomColumnDisplayText="itgGrid_CustomColumnDisplayText"
                    OnSummaryDisplayText="itgGrid_SummaryDisplayText" EnableViewState="false" runat="server" AutoGenerateColumns="False" Images-HeaderActiveFilter-Url="/Content/images/Filter_Red_16.png"
                    OnDataBinding="itgGrid_DataBinding" OnHtmlDataCellPrepared="itgGrid_HtmlDataCellPrepared" OnHtmlRowPrepared="itgGrid_HtmlRowPrepared" OnHtmlRowCreated="itgGrid_HtmlRowCreated"
                    ClientInstanceName="itgGridClientInstance" OnCustomColumnSort="itgGrid_CustomColumnSort" OnBeforeGetCallbackResult="itgGrid_BeforeGetCallbackResult"
                    Theme="DevEx" Width="100%" SettingsText-EmptyHeaders="&nbsp;" KeyFieldName="TicketId">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Caption=" " CellStyle-Wrap="Default" CellStyle-Font-Bold="true" SelectAllCheckboxMode="Page" VisibleIndex="0">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Priority" FieldName="PriorityLookup" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-EncodeHtml="false" HeaderStyle-Font-Bold="true" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Rank" FieldName="ProjectRank" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Initiative" FieldName="ProjectInitiativeLookup" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Category" FieldName="RequestTypeCategory" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Project Type" FieldName="RequestTypeLookup" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Sub Category" FieldName="RequestTypeSubCategory" HeaderStyle-Font-Bold="true" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Project ID" FieldName="TicketId" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList" Width="100px" HeaderStyle-Font-Bold="true">
                            <DataItemTemplate>
                                <dx:ASPxLabel ID="lblProjectId" runat="server" Text='<%# Eval("TicketId") %>'></dx:ASPxLabel>
                                <input type="hidden" id="hdnkeyfieldname" value='<%# Container.KeyValue%>' />
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Project" FieldName="TitleLink" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Resources" FieldName="TicketId" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="False" SettingsHeaderFilter-Mode="CheckedList" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Status" FieldName="Status" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Budget" FieldName="BudgetAmountWithLink" CellStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center" FooterCellStyle-HorizontalAlign="Right" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList" HeaderStyle-Font-Bold="true">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Actual" FieldName="Actual" CellStyle-HorizontalAlign="Right" PropertiesTextEdit-DisplayFormatString="C" HeaderStyle-HorizontalAlign="Center" FooterCellStyle-HorizontalAlign="Right" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Module" FieldName="ModuleName" Visible="false"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" Visible="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn Caption="Start Date" FieldName="ActualStartDate" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesDateEdit-DisplayFormatString="{0:MMM-dd-yyyy}" CellStyle-Wrap="False" HeaderStyle-Font-Bold="true" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList"></dx:GridViewDataDateColumn>
                        <dx:GridViewDataDateColumn Caption="End Date" FieldName="ActualCompletionDate" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" PropertiesDateEdit-DisplayFormatString="{0:MMM-dd-yyyy}" CellStyle-Wrap="False" HeaderStyle-Font-Bold="true" Settings-AllowHeaderFilter="Default" SettingsHeaderFilter-Mode="CheckedList"></dx:GridViewDataDateColumn>
                        <dx:GridViewBandColumn Caption="Resource Demand" ExportCellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Q1" FieldName="Q1" Settings-AllowAutoFilter="False" Settings-AllowSort="False" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Q2" FieldName="Q2" Settings-AllowAutoFilter="False" Settings-AllowSort="False" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Q3" FieldName="Q3" Settings-AllowAutoFilter="False" Settings-AllowSort="False" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Q4" FieldName="Q4" Settings-AllowAutoFilter="False" Settings-AllowSort="False" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true"></dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:GridViewBandColumn>

                    </Columns>
                    <Styles AlternatingRow-CssClass="ms-alternatingstrong" GroupRow-Font-Bold="true" AlternatingRow-Enabled="True" Footer-Font-Bold="true"></Styles>
                    <SettingsLoadingPanel Mode="ShowAsPopup" />
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                    <SettingsBehavior AllowSort="true" SortMode="Custom" AllowSelectByRowClick="true" AllowGroup="true" AutoExpandAllGroups="true" ProcessSelectionChangedOnServer="false" />
                    <Settings ShowFooter="true" ShowHeaderFilterButton="true" />
                    <TotalSummary>
                        <dx:ASPxSummaryItem FieldName="BudgetAmount" ShowInColumn="BudgetAmountWithLink" DisplayFormat="{0:C}" SummaryType="Sum" />
                        <dx:ASPxSummaryItem FieldName="Actual" ShowInColumn="Actual" DisplayFormat="{0:C}" SummaryType="Sum" />
                        <dx:ASPxSummaryItem FieldName="Q1" ShowInColumn="Q1" SummaryType="Sum" DisplayFormat="{0}" />
                        <dx:ASPxSummaryItem FieldName="Q2" ShowInColumn="Q2" SummaryType="Sum" DisplayFormat="{0}" />
                        <dx:ASPxSummaryItem FieldName="Q3" ShowInColumn="Q3" SummaryType="Sum" DisplayFormat="{0}" />
                        <dx:ASPxSummaryItem FieldName="Q4" ShowInColumn="Q4" SummaryType="Sum" DisplayFormat="{0}" />

                        <%--<dx:ASPxSummaryItem FieldName="CostToCompletion" ShowInColumn="CostToCompletion" DisplayFormat="{0:C}" SummaryType="Sum" />--%>
                    </TotalSummary>
                    <GroupSummary>
                        <dx:ASPxSummaryItem FieldName="BudgetAmount" SummaryType="Sum" DisplayFormat="Budget={0:C}" />
                        <dx:ASPxSummaryItem FieldName="Actual" SummaryType="Sum" DisplayFormat="Actual={0:C}" />
                    </GroupSummary>
                    <ClientSideEvents EndCallback="RefreshInstance" SelectionChanged="function(s, e) {   selectionChanged(s,e); } " />

                </ugit:ASPxGridView>
                <dx:ASPxGlobalEvents ID="ge" runat="server">
                    <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
                </dx:ASPxGlobalEvents>
                <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="itgGrid"></dx:ASPxGridViewExporter>
            <%--</asp:Panel>--%>
        </div>
        <div style="padding-top: 10px; width: 100%; float: left;">
            <asp:Panel runat="server" ID="constraintsContainer" GroupingText="Total Cost" Width="48%"
                Style="float: left; padding: 5px;">
                <div>
                    <asp:ListView runat="server" ID="lvITGConstraints" ItemPlaceholderID="PlaceHolder2">
                        <LayoutTemplate>
                            <table class="ms-listviewtable allowcationdetail" width="100%" cellpadding="0" cellspacing="0">
                                <tr class="detailviewheader ms-viewheadertr" style="font-weight: bold">
                                    <%-- <th class="ms-vh2 bold">
                                            <asp:LinkButton ID="lblConstraints" runat="server" CommandName="Sort" CommandArgument="Constraints"
                                                Text="Constraint" Width="150px" />
                                        </th>--%>
                                    <th class="ms-vh2 bold">
                                        <asp:Label ID="lblValues" runat="server" CommandName="Sort" CommandArgument="Budget"
                                            Text="Budget Cost" />
                                    </th>
                                    <th class="ms-vh2 bold">
                                        <asp:Label ID="lblNotes" runat="server" CommandName="Sort" CommandArgument="Actual"
                                            Text="Actual Cost" />
                                    </th>
                                </tr>
                                <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class="detailviewitem " ondblclick="Select(<%# Container.DataItemIndex %>)">

                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Budget") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Actual") %>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="detailviewitem ms-alternatingstrong" ondblclick="Select(<%# Container.DataItemIndex %>)">
                                <%--   <td class="ms-vb2 paddingfirstcell">
                                        <%# Eval("Constraint") %>
                                    </td>--%>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Budget") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Actual") %>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="measuresContainrer" GroupingText="Total Resource Demand" Width="48%"
                Style="float: left; padding: 5px;">
                <div>
                    <asp:ListView runat="server" ID="lvITGMeasures" ItemPlaceholderID="PlaceHolder3">
                        <LayoutTemplate>
                            <table class="ms-listviewtable allowcationdetail" width="100%" cellpadding="0" cellspacing="0">
                                <tr class="detailviewheader ms-viewheadertr" style="font-weight: bold">
                                    <th class="ms-vh2 bold">
                                        <asp:Label ID="lblQ1" runat="server" CommandName="Sort" CommandArgument="Q1"
                                            Text="Q1" />
                                    </th>
                                    <th class="ms-vh2 bold">
                                        <asp:Label ID="lblQ2" runat="server" CommandName="Sort" CommandArgument="Q2"
                                            Text="Q2" />
                                    </th>
                                    <th class="ms-vh2 bold">
                                        <asp:Label ID="lblQ3" runat="server" CommandName="Sort" CommandArgument="Q3"
                                            Text="Q3" />
                                    </th>
                                    <th class="ms-vh2 bold">
                                        <asp:Label ID="lblQ4" runat="server" CommandName="Sort" CommandArgument="Q4"
                                            Text="Q4" />
                                    </th>
                                    <asp:PlaceHolder ID="PlaceHolder3" runat="server"></asp:PlaceHolder>
                                </tr>
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class="detailviewitem " ondblclick="Select(<%# Container.DataItemIndex %>)">
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q1") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q2") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q3") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q4") %>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="detailviewitem ms-alternatingstrong" ondblclick="Select(<%# Container.DataItemIndex %>)">
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q1") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q2") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q3") %>
                                </td>
                                <td class="ms-vb2 paddingfirstcell">
                                    <%# Eval("Q4") %>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </div>
            </asp:Panel>
        </div>


    </div>
</fieldset>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openTicketmultiple(url1, title1, id, ticketId) {
        var winWidth = 0, winHeight = 0;
        if (parseInt(navigator.appVersion) > 3) {
            if (navigator.appName == "Netscape") {
                winWidth = window.innerWidth;
                winHeight = window.innerHeight;
            }
            if (navigator.appName.indexOf("Microsoft") != -1) {
                winWidth = document.body.offsetWidth;
                winHeight = document.body.offsetHeight;
            }
        }
        var options = {
            url: url1 + "?TicketId=" + ticketId,
            width: winWidth,
            height: winHeight,
            title: title1 + " Ticket: " + ticketId,
            allowMaximize: false,
            showClose: true,
            dialogReturnValueCallback: SP.UI.ModalDialog.RefreshPage
        };
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options)
    }
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NPRBudget.ascx.cs" Inherits="uGovernIT.Web.NPRBudget" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>--%>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
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
        color: unset;
        border: unset;
        border-bottom: unset;
    }

    .calenderyearnum {
            font-family: 'Poppins', sans-serif;
    font-size: 12px;
    font-weight: 500;
    padding: 0px 2px;
        

    }
</style>
<div class="row" style="margin-right: 15px; margin-left: 15px">
    <asp:Panel ID="editMode" runat="server">
        <style type="text/css">
            .errormessage-block {
                text-align: center;
                display: block;
            }

                .errormessage-block ol, .errormessage-block ol {
                    list-style-type: none;
                    color: Red;
                }

            .mainblock {
            }

            .fullwidth {
                width: 98%;
            }

            .paddingfirstcell {
                padding-left: 6px !important;
            }

            .ms-listviewtable {
                border: 1px solid #DCDCDC !important;
                border-collapse: separate !important;
                background: #F8F8F8 !important;
            }

            .ms-viewheadertr .ms-vh2-gridview {
                height: 25px;
            }

            .detailviewitem td {
                text-align: left;
            }

            .widhtfirstcell {
                width: 99px;
            }

            .editviewtable td, .editviewtable th {
                border: 1px solid #DCDCDC;
                text-align: center;
                padding: 2px;
            }

                .editviewtable td td {
                    border: none;
                }

            .datetimectr {
                height: 20px;
                margin-right: -4px;
            }

            .fleft {
                float: left;
            }

            .padding-button {
                padding-left: 2px;
            }

             

            .alncenter {
                text-align: center;
            }

            .worksheetpanel {
                position: relative;
            }

            .worksheetmessage-m1 {
                padding-right: 6%;
                position: absolute;
                top: 3px;
                left: 2px;
            }

            .totalbudget-container td {
                border-top: 1px solid gray !important;
                border-bottom: 1px solid gray !important;
            }

            #ms-belltown-table {
                width: 100% !important;
            }
        </style>

        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_pageLoading(MyPageLoading);
            prm.add_endRequest(EndRequest);
            var btnId;

            function InitializeRequest(sender, args) {
                var prm = Sys.WebForms.PageRequestManager.getInstance();
            }

            var notifyId = "";
            function AddNotification(msg) {
                if (notifyId == "") {
                    notifyId = SP.UI.Notify.addNotification(msg, true);
                }
            }
            function RemoveNotification() {
                SP.UI.Notify.removeNotification(notifyId);
                notifyId = '';
            }
            function BeginRequestHandler(sender, args) {
                AddNotification("Processing ..");
            }

            function EndRequest(sender, args) {
                window.parent.adjustIFrameWithHeight("<%=FrameId %>", $(".managementcontrol-main").height());
                var s = sender;
                var a = args;
                var msg = null;
                if (a._error != null) {
                    switch (args._error.name) {
                        case "Sys.WebForms.PageRequestManagerServerErrorException":
                            msg = "PageRequestManagerServerErrorException";
                            break;
                        case "Sys.WebForms.PageRequestManagerParserErrorException":
                            msg = "PageRequestManagerParserErrorException";
                            break;
                        case "Sys.WebForms.PageRequestManagerTimeoutException":
                            msg = "PageRequestManagerTimeoutException";
                            break;
                    }
                    args._error.message = "My Custom Error Message " + msg;
                    args.set_errorHandled(true);

                }
                else {
                    RemoveNotification();
                    $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
                        addHeightToCalculateFrameHeight(this, 170);
                    });
                }
            }

            function MyPageLoading(sender, args) {
            }


            function openDialog(path, params, titleVal, width, height, stopRefresh) {
                window.parent.UgitOpenPopupDialog(path, params, titleVal, '800px', '580px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }

        function NewResourceItem() {
            //aspxNPRResourceList.AddNewRow();
            var nprID = "<%= nprId %>";
            var param = "NPRID=" + nprID;
            window.parent.UgitOpenPopupDialog('<%= NPRResourceAddEditUrl %>', param, 'Add New Resource', '800px', '580px', 0, escape("<%= Request.Url.AbsolutePath %>"));
            }

            function newBudget() {
                var nprID = "<%= nprId %>";
            var param = "NPRID=" + nprID;
            window.parent.UgitOpenPopupDialog('<%= NPRBudgetAddEditUrl %>', param, 'Add New Budget', '600px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
            }

            function deletedBudget() {
                if (confirm("Are you sure want to delete budget item?")) {
                    return true;
                }
                return false;
            }

            function deleteResource() {
                if (confirm("Are you sure want to delete this resource?")) {
                    return true;
                }
                return false;
            }

        </script>

        <div style="display: none">
            <%--<SharePoint:DateTimeControl runat="server" ID="dummyCalendar" Visible="true" />--%>
        </div>
        <asp:UpdatePanel runat="server" ID="updateNPR" UpdateMode="Conditional" Visible="true">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <asp:HiddenField runat="server" ID="subCategoryHidden" Value="" />

                <fieldset id="fldsetNPRResource" runat="server">
                    <legend style="font-weight: bold;">Resources</legend>
                    <ugit:aspxgridview id="aspxNPRResourceList" clientinstancename="aspxNPRResourceList" runat="server" autogeneratecolumns="false"
                        width="100%" styles-footer-font-bold="true" styles-footer-horizontalalign="Center" ondatabinding="aspxNPRResourceList_DataBinding"
                        settingstext-emptydatarow="No record found." keyfieldname="ID" enablecallbacks="false" enableviewstate="true"
                        onhtmlrowprepared="aspxNPRResourceList_HtmlRowPrepared" onhtmldatacellprepared="aspxNPRResourceList_HtmlDataCellPrepared">
                        <columns>
                            <dx:gridviewdatatextcolumn caption="Skill" fieldname="UserSkillLookup"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Staff Type" fieldname="BudgetTypeChoice"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Description" fieldname="_ResourceType"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="FTEs" fieldname="NoOfFTEs" width="50px"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Start Date" fieldname="AllocationStartDate" propertiestextedit-displayformatstring="MMM-d-yyyy" width="100px"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="End Date" fieldname="AllocationEndDate" propertiestextedit-displayformatstring="MMM-d-yyyy" width="100px"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Req. Resource(s)" fieldname="RequestedResourcesUser"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Role Name" fieldname="RoleNameChoice"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn fieldname=" " width="55px">
                                <dataitemtemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" ImageUrl="/Content/images/delete-icon.png"
                                        Style="float: right; padding-right: 10px;" OnClick="imgDelete_Click1" CommandArgument='<%# Eval("ID") %>' OnClientClick="return deleteResource();" />
                                    <img runat="server" id="imgEdit" src="/Content/images/edit-icon.png" alt="Edit" style="float: right; padding-right: 10px;" />
                                </dataitemtemplate>
                                <settings allowautofilter="False" allowsort="False" allowheaderfilter="False" />
                            </dx:gridviewdatatextcolumn>
                        </columns>
                        <totalsummary>
                            <dx:aspxsummaryitem fieldname="NoOfFTEs" summarytype="Sum" showincolumn="NoOfFTEs" displayformat="{0}" />
                            <dx:aspxsummaryitem fieldname="_ResourceType" summarytype="Sum" displayformat="Total" showincolumn="_ResourceType" />
                        </totalsummary>
                        <settingsediting mode="Inline" />
                        <settingsbehavior allowsort="false" confirmdelete="true" />
                        <settings showfooter="true" showcolumnheaders="true" showstatusbar="Hidden" />
                        <settingspager visible="false" mode="ShowAllRecords"></settingspager>
                        <settingstext emptydatarow="No record found." confirmdelete=""></settingstext>
                        <styles>
                            <cell horizontalalign="Center"></cell>
                            <header horizontalalign="Center" font-bold="true"></header>
                            <commandcolumnitem paddings-paddingleft="5px"></commandcolumnitem>
                        </styles>
                    </ugit:aspxgridview>

                    <asp:HyperLink ID="addNewResourceItem" runat="server" Text="Add New Resource" CssClass="fleft" NavigateUrl="javascript:NewResourceItem()">
                                    <span class="button-bg">
                                        <b style="float: left; font-weight: normal;">
                                        Add New Resource</b>
                                        <i
                                    style="float: left; position: relative; top: 1px;left:2px">
                                    <img src="/Content/images/add_icon.png"  style="border:none;" title="" alt=""/></i> 
                                        </span>
                    </asp:HyperLink>
                </fieldset>

                <fieldset id="fldsetNPRBudget" runat="server">
                    <legend style="font-weight: bold;">Budget Items</legend>
                    <asp:Label ID="budgetMessage" runat="server" EnableViewState="false" CssClass="errormessage-block ugitlight1lightest" ForeColor="Blue"></asp:Label>
                    <asp:HiddenField ID="sortingExp" runat="server" />

                    <dx:aspxgridview id="aspxNPRBudgetgrid" clientinstancename="aspxNPRBudgetgrid" runat="server" autogeneratecolumns="false"
                        width="100%" styles-footer-font-bold="true" styles-footer-horizontalalign="Center" ondatabinding="aspxNPRBudgetgrid_DataBinding"
                        settingstext-emptydatarow="No record found." keyfieldname="ID" enablecallbacks="false" enableviewstate="true"
                        onhtmlrowprepared="aspxNPRBudgetgrid_HtmlRowPrepared">
                        <columns>
                            <dx:gridviewdatatextcolumn caption="Category" fieldname="BudgetCategory"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Sub Category" fieldname="BudgetSubCategory"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Budget Item" fieldname="BudgetItem"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Amount" fieldname="BudgetAmount" width="50px" propertiestextedit-displayformatstring="{0:c}"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Start Date" fieldname="AllocationStartDate" propertiestextedit-displayformatstring="MMM-d-yyyy" width="100px"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="End Date" fieldname="AllocationEndDate" propertiestextedit-displayformatstring="MMM-d-yyyy" width="100px"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn caption="Notes" fieldname="BudgetDescription"></dx:gridviewdatatextcolumn>
                            <dx:gridviewdatatextcolumn fieldname=" " width="55px">
                                <dataitemtemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" ImageUrl="/Content/images/delete-icon.png"
                                        Style="float: right; padding-right: 10px;" OnClick="imgDelete_Click" CommandArgument='<%# Eval("ID") %>' OnClientClick="return deletedBudget();" />
                                    <img id="imgEdit" runat="server" src="/Content/images/edit-icon.png" alt="Edit" style="float: right; padding-right: 10px;" />
                                </dataitemtemplate>
                            </dx:gridviewdatatextcolumn>
                        </columns>
                        <totalsummary>
                            <dx:aspxsummaryitem fieldname="BudgetAmount" summarytype="Sum" showincolumn="BudgetAmount" displayformat="{0:c}" />
                        </totalsummary>
                        <settingsediting mode="Inline" />
                        <settingsbehavior allowsort="false" confirmdelete="true" />
                        <settings showfooter="true" showcolumnheaders="true" showstatusbar="Hidden" />
                        <settingstext emptydatarow="No record found."></settingstext>
                        <styles>
                            <cell horizontalalign="Center"></cell>
                            <header horizontalalign="Center" font-bold="true"></header>
                            <commandcolumnitem paddings-paddingleft="5px"></commandcolumnitem>
                        </styles>
                    </dx:aspxgridview>

                    <asp:LinkButton ID="newBudget" runat="server" OnClientClick="newBudget(); return false;" Text="Add New Budget Item" CssClass="fleft" CommandName="NewBudgetItem" CommandArgument="0#0">
                    <span class="button-bg">
                        <b style="float: left; font-weight: normal;">
                        Add New Budget Item</b>
                        <i style="float: left; position: relative; top: 1px;left:2px">
                        <img src="/Content/images/add_icon.png"  style="border:none;" title="" alt=""/></i> 
                    </span>
                    </asp:LinkButton>
                </fieldset>

                <fieldset id="fldsetPlanSheet" runat="server">
                    <legend style="font-weight: bold;">Monthly Budget and Resources</legend>
                    <asp:UpdatePanel runat="server" ID="projectPlanUpdatePanel">
                        <Triggers>
                        </Triggers>
                        <ContentTemplate>
                            <asp:Panel ID="projectPlanPanel" CssClass="worksheetpanel" runat="server">
                                <div class="worksheetmessage-m1">
                                    <table cellpadding="2" cellspacing="0" class="bordercolps" width="100%">
                                        <tr>
                                            <td>
                                                <div class="">
                                                    <span class="fleft">
                                                        <asp:ImageButton ImageUrl="/Content/images/Previous16x16.png" ID="previousWeek"
                                                            runat="server" OnClientClick="previousYearBudget();" />
                                                    </span><span class="fleft calenderyearnum">
                                                        <%= currentYear %>
                                                    </span><span class="fleft">
                                                        <asp:ImageButton ImageUrl="/Content/images/Next16x16.png" ID="nextWeek"
                                                            runat="server" OnClientClick="nextYearBudget();" />
                                                    </span>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="worksheetpanel-m">
                                    <asp:ListView ID="projectPlanSheet" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1">
                                        <LayoutTemplate>
                                            <table class="ms-listviewtable worksheettable" style="border-collapse: collapse"
                                                width="100%" cellpadding="0" cellspacing="0">
                                                <tr class="worksheetheader ms-viewheadertr">
                                                    <th class="ms-vh2 paddingfirstcell" width="150">&nbsp;
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Jan</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Feb</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Mar</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>April</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>May</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>June</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>July</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Aug</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Sep</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Oct</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Nov</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                        <b>Dec</b>
                                                    </th>
                                                    <th class="ms-vh2 alncenter totalbordervartical" style="text-align: right; padding-right: 5px;">
                                                        <b>Total</b>
                                                    </th>
                                                </tr>
                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="ms-vb2 paddingfirstcell ">
                                                    <%#  Eval("Title") %>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month1") : String.Format("{0:C}", float.Parse(Eval("Month1").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month2") : String.Format("{0:C}", float.Parse(Eval("Month2").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month3") : String.Format("{0:C}", float.Parse(Eval("Month3").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month4") : String.Format("{0:C}", float.Parse(Eval("Month4").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month5") : String.Format("{0:C}", float.Parse(Eval("Month5").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month6") : String.Format("{0:C}", float.Parse(Eval("Month6").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month7") : String.Format("{0:C}", float.Parse(Eval("Month7").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month8") : String.Format("{0:C}", float.Parse(Eval("Month8").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month9") : String.Format("{0:C}", float.Parse(Eval("Month9").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month10") : String.Format("{0:C}", float.Parse(Eval("Month10").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month11") : String.Format("{0:C}", float.Parse(Eval("Month11").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month12") : String.Format("{0:C}", float.Parse(Eval("Month12").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter totalbordervartical" style="text-align: right; padding-right: 5px;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Total") : String.Format("{0:C}", float.Parse(Eval("Total").ToString()))%>
                                                </td>

                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="ugitlight1lightest">
                                                <td class="ms-vb2 paddingfirstcell ">
                                                    <%#  Eval("Title") %>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month1") : String.Format("{0:C}", float.Parse(Eval("Month1").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month2") : String.Format("{0:C}", float.Parse(Eval("Month2").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month3") : String.Format("{0:C}", float.Parse(Eval("Month3").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month4") : String.Format("{0:C}", float.Parse(Eval("Month4").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month5") : String.Format("{0:C}", float.Parse(Eval("Month5").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month6") : String.Format("{0:C}", float.Parse(Eval("Month6").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month7") : String.Format("{0:C}", float.Parse(Eval("Month7").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month8") : String.Format("{0:C}", float.Parse(Eval("Month8").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month9") : String.Format("{0:C}", float.Parse(Eval("Month9").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month10") : String.Format("{0:C}", float.Parse(Eval("Month10").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month11") : String.Format("{0:C}", float.Parse(Eval("Month11").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter" style="text-align: right;">
                                                    <%#  Eval("Title") != "Budget" ? Eval("Month12") : String.Format("{0:C}", float.Parse(Eval("Month12").ToString()))%>
                                                </td>
                                                <td class="ms-vb2 alncenter totalbordervartical" style="text-align: right; padding-right: 5px;">
                                                    <%# Eval("Total")%>
                                                </td>

                                            </tr>
                                        </AlternatingItemTemplate>

                                    </asp:ListView>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>


        <script type="text/javascript">
            function saveInHidden(obj) {
                document.getElementById('<%= subCategoryHidden.ClientID %>').value = obj.options[obj.selectedIndex].value;
            }

            function previousYearBudget() {
                var year = Number($.trim($(".calenderyearnum").text()));
                var preYear = year - 1;
                var url = window.document.location.href;
                set_cookie("budgetyear", preYear, null, window.location.pathname, null, null);
            }
            function nextYearBudget() {
                var year = Number($.trim($(".calenderyearnum").text()));
                var nextYear = year + 1;
                var url = window.document.location.href;
                set_cookie("budgetyear", nextYear, null, window.location.pathname, null, null);
            }
        </script>

    </asp:Panel>
    <asp:Panel ID="viewMode" runat="server" CssClass="">

        <style type="text/css">
            .heading123 {
                font-weight: bold;
                padding-right: 5px;
                padding-top: 2px;
            }

            .scoreheading {
                font-weight: bold;
                padding-right: 5px;
            }

            .topborder {
                border-top: 1px solid black;
            }

            .fleft {
                float: left;
            }

            .readonlyblock {
                float: left;
                width: 100%;
                margin-top: 10px;
            }


            .padding-button {
                padding-left: 2px;
            }

             

            .alncenter {
                text-align: center;
            }

            .worksheetpanel {
                position: relative;
            }

            .worksheetmessage-m1 {
                padding-right: 6%;
                position: absolute;
                top: 3px;
                left: 2px;
            }

            .totalbudget-container td {
            }
        </style>

        <script type="text/javascript">
            function previousYearBudget() {
                var year = Number($.trim($(".calenderyearnum").text()));
                var preYear = year - 1;
                var url = window.document.location.href;
                set_cookie("budgetyear", preYear, null, window.location.pathname, null, null);
                return false;
            }
            function nextYearBudget() {
                var year = Number($.trim($(".calenderyearnum").text()));
                var nextYear = year + 1;
                var url = window.document.location.href;
                set_cookie("budgetyear", nextYear, null, window.location.pathname, null, null);
            }
        </script>

        <fieldset id="fldSetReadOnlyNPRResource" runat="server">
            <legend style="font-weight: bold;">Resources</legend>
            <dx:aspxgridview id="aspxReadOnlyNPRResource" clientinstancename="aspxReadOnlyNPRResource" onhtmldatacellprepared="aspxNPRResourceList_HtmlDataCellPrepared" runat="server" autogeneratecolumns="false" width="100%" styles-footer-font-bold="true" styles-footer-horizontalalign="Center"
                settingstext-emptydatarow="No record found." keyfieldname="ID" cssclass="ro-table">
                <columns>
                    <dx:gridviewdatatextcolumn caption="Skill" fieldname="UserSkillLookup"></dx:gridviewdatatextcolumn>
                    <dx:gridviewdatatextcolumn caption="Staff Type" fieldname="BudgetTypeChoice"></dx:gridviewdatatextcolumn>
                    <dx:gridviewdatatextcolumn caption="Description" fieldname="_ResourceType"></dx:gridviewdatatextcolumn>
                    <dx:gridviewdatatextcolumn caption="FTEs" fieldname="NoOfFTEs" width="50px"></dx:gridviewdatatextcolumn>
                    <dx:gridviewdatatextcolumn caption="Start Date" fieldname="AllocationStartDate" propertiestextedit-displayformatstring="MMM-d-yyyy" width="100px"></dx:gridviewdatatextcolumn>
                    <dx:gridviewdatatextcolumn caption="End Date" fieldname="AllocationEndDate" propertiestextedit-displayformatstring="MMM-d-yyyy" width="100px"></dx:gridviewdatatextcolumn>

                    <%--<dx:GridViewDataTextColumn Caption="Est. Hrs" FieldName="EstimatedHours" Width="50px"></dx:GridViewDataTextColumn>--%>
                    <dx:gridviewdatatextcolumn caption="Req. Resource(s)s" fieldname="RequestedResourcesUser"></dx:gridviewdatatextcolumn>
                    <dx:gridviewdatatextcolumn caption="Role Name" fieldname="RoleNameChoice"></dx:gridviewdatatextcolumn>
                </columns>
                <totalsummary>
                    <dx:aspxsummaryitem fieldname="NoOfFTEs" summarytype="Sum" showincolumn="TicketNoOfFTEs" displayformat="{0}" />
                    <dx:aspxsummaryitem fieldname="_ResourceType" summarytype="Sum" displayformat="Total" showincolumn="_ResourceType" />
                </totalsummary>
                <settingsbehavior allowsort="false" />
                <settings showfooter="true" showcolumnheaders="true" showstatusbar="Hidden" />
                <settingstext emptydatarow="No record found."></settingstext>

                <styles>
                    <cell horizontalalign="Center"></cell>
                    <header horizontalalign="Center" font-bold="true" cssclass="ro-padding"></header>
                    <row cssclass="ro-header"></row>
                    <commandcolumnitem paddings-paddingleft="5px"></commandcolumnitem>
                </styles>
            </dx:aspxgridview>

            <%--   <asp:ObjectDataSource ID="objReadOnlyDataSourceSkill" runat="server"></asp:ObjectDataSource>
                <asp:ObjectDataSource ID="objReadOnlyDataSourceCategory" runat="server"></asp:ObjectDataSource>--%>
        </fieldset>

        <fieldset id="fldSetReadOnlyBudgetItems" runat="server">
            <legend style="font-weight: bold;">Budget Items</legend>
            <asp:ListView ID="readOnlyNPRBugetList" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1"
                DataKeyNames="Id">
                <LayoutTemplate>
                    <table class="ro-table" frame="box" rules="all" width="100%" cellpadding="0" cellspacing="0">
                        <tr class="ro-header">
                            <th class="ro-padding">
                                <b>Category</b>
                            </th>
                            <th class="ro-padding">
                                <b>Sub Category</b>
                            </th>
                            <th class="ro-padding">
                                <b>Budget Item</b>
                            </th>
                            <th class="ro-padding" style="text-align: right; padding-right: 5px;">
                                <b>Amount</b>
                            </th>
                            <th class="ro-padding">
                                <b>Start Date</b>
                            </th>
                            <th class="ro-padding">
                                <b>End Date</b>
                            </th>
                            <th class="ro-padding">
                                <b>Notes</b>
                            </th>

                        </tr>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                        <tr class="totalbudget-container ro-item">

                            <td colspan="2"></td>
                            <td class="ms-vh2" style="text-align: right; padding-right: 5px;"><b>Total</b></td>
                            <td style="text-align: right;"><b>
                                <asp:Literal ID="lblTotalBudget" runat="server"></asp:Literal></b> </td>
                            <td colspan="3"></td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class="ro-item">
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetCategory) %>
                        </td>
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetSubCategory)%>
                        </td>
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetItem)%>
                        </td>
                        <td class="ro-padding" style="text-align: right; padding-right: 5px;">
                            <%# String.Format("{0:C}",Eval(DatabaseObjects.Columns.BudgetAmount)) %>
                        </td>
                        <td class="ro-padding">
                            <%# string.Format("{0:MMM-dd-yyyy}", Eval(DatabaseObjects.Columns.AllocationStartDate)) %>
                        </td>
                        <td class="ro-padding">
                            <%# string.Format("{0:MMM-dd-yyyy}", Eval(DatabaseObjects.Columns.AllocationEndDate)) %>
                        </td>
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetDescription)%>
                        </td>

                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="ro-alternateitem">
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetCategory) %>
                        </td>
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetSubCategory)%>
                        </td>
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetItem)%>
                        </td>
                        <td class="ro-padding" style="text-align: right; padding-right: 5px;">
                            <%# String.Format("{0:C}",Eval(DatabaseObjects.Columns.BudgetAmount))%>
                        </td>
                        <td class="ro-padding">
                            <%# string.Format("{0:MMM-dd-yyyy}", Eval(DatabaseObjects.Columns.AllocationStartDate)) %>
                        </td>
                        <td class="ro-padding">
                            <%# string.Format("{0:MMM-dd-yyyy}", Eval(DatabaseObjects.Columns.AllocationEndDate)) %>
                        </td>
                        <td class="ro-padding">
                            <%# Eval(DatabaseObjects.Columns.BudgetDescription)%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:ListView>
        </fieldset>



        <fieldset id="fldSetReadOnlyplanSheet" runat="server">
            <legend style="font-weight: bold;">Monthly Budget and Resources</legend>
            <asp:Panel ID="Panel1" CssClass="worksheetpanel" runat="server">
                <div class="worksheetmessage-m1">
                    <table cellpadding="2" cellspacing="0" class="bordercolps" width="100%">
                        <tr>
                            <td>
                                <div class="">
                                    <span class="fleft">
                                        <asp:ImageButton ImageUrl="/Content/images/Previous16x16.png" ID="ImageButton1"
                                            runat="server" OnClientClick="previousYearBudget()" />
                                    </span><span class="fleft calenderyearnum">
                                        <%= currentYear %>
                                    </span><span class="fleft">
                                        <asp:ImageButton ImageUrl="/Content/images/Next16x16.png" ID="ImageButton2"
                                            runat="server" OnClientClick="nextYearBudget()" />
                                    </span>
                                </div>
                            </td>
                        </tr>
                    </table>

                </div>
                <div class="worksheetpanel-m">
                    <asp:ListView ID="readOnlyPlanSheet" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1">
                        <LayoutTemplate>
                            <table class="ro-table" frame="box" rules="all" width="100%" cellpadding="0" cellspacing="0">
                                <tr class="ro-header">
                                    <th class="ro-padding" width="150">&nbsp;
                                    </th>
                                    <th class="ro-padding">
                                        <b>Jan</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Feb</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Mar</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>April</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>May</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>June</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>July</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Aug</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Sep</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Oct</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Nov</b>
                                    </th>
                                    <th class="ro-padding">
                                        <b>Dec</b>
                                    </th>
                                    <th class="ro-padding" style="text-align: right; padding-right: 5px;">
                                        <b>Total</b>
                                    </th>

                                </tr>
                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr class="ro-item">
                                <td class="ro-padding">
                                    <%#  Eval("Title") %>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month1") : String.Format("{0:C}", float.Parse(Eval("Month1").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month2") : String.Format("{0:C}", float.Parse(Eval("Month2").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month3") : String.Format("{0:C}", float.Parse(Eval("Month3").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month4") : String.Format("{0:C}", float.Parse(Eval("Month4").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month5") : String.Format("{0:C}", float.Parse(Eval("Month5").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month6") : String.Format("{0:C}", float.Parse(Eval("Month6").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month7") : String.Format("{0:C}", float.Parse(Eval("Month7").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month8") : String.Format("{0:C}", float.Parse(Eval("Month8").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month9") : String.Format("{0:C}", float.Parse(Eval("Month9").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month10") : String.Format("{0:C}", float.Parse(Eval("Month10").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month11") : String.Format("{0:C}", float.Parse(Eval("Month11").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month12") : String.Format("{0:C}", float.Parse(Eval("Month12").ToString()))%>
                                </td>
                                <td class="ro-padding" style="text-align: right; padding-right: 5px;">
                                    <%#  Eval("Title") != "Budget" ? Eval("Total") : String.Format("{0:C}", float.Parse(Eval("Total").ToString()))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="ro-alternateitem">
                                <td class="ro-padding">
                                    <%#  Eval("Title") %>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month1") : String.Format("{0:C}", float.Parse(Eval("Month1").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month2") : String.Format("{0:C}", float.Parse(Eval("Month2").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month3") : String.Format("{0:C}", float.Parse(Eval("Month3").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month4") : String.Format("{0:C}", float.Parse(Eval("Month4").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month5") : String.Format("{0:C}", float.Parse(Eval("Month5").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month6") : String.Format("{0:C}", float.Parse(Eval("Month6").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month7") : String.Format("{0:C}", float.Parse(Eval("Month7").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month8") : String.Format("{0:C}", float.Parse(Eval("Month8").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month9") : String.Format("{0:C}", float.Parse(Eval("Month9").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month10") : String.Format("{0:C}", float.Parse(Eval("Month10").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month11") : String.Format("{0:C}", float.Parse(Eval("Month11").ToString()))%>
                                </td>
                                <td class="ro-padding">
                                    <%#  Eval("Title") != "Budget" ? Eval("Month12") : String.Format("{0:C}", float.Parse(Eval("Month12").ToString()))%>
                                </td>
                                <td class="ro-padding" style="text-align: right; padding-right: 5px;">
                                    <%#  Eval("Title") != "Budget" ? Eval("Total") : String.Format("{0:C}", float.Parse(Eval("Total").ToString()))%>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </div>
            </asp:Panel>
        </fieldset>
    </asp:Panel>
</div>

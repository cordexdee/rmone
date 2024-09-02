<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleBudgetList.ascx.cs" Inherits="uGovernIT.Web.ModuleBudgetList" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Panel ID="editMode" runat="server">
    <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .errormessage-block {
            text-align: center;
            display: block;
        }

            .errormessage-block ol, .errormessage-block ol {
                list-style-type: none;
                color: Red;
            }

        /*.mainblock {
        }*/

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

        /*.calenderyearnum {
            font-weight: bold;
            padding-top: 1px;
            padding-left: 3px;
            padding-right: 3px;
        }*/

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

        legend {
            padding-top: 0;
            padding-bottom: 0;
            margin-bottom: 5px;
        }

        fieldset, td {
            padding-top: 0;
        }
    </style>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

        function setSelectedDocumentDetails(documentData, documentId) {

        }
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

            }
        }
        function RemoveNotification() {

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
            window.parent.UgitOpenPopupDialog(path, params, titleVal, '800px', '450px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }

        function NewResourceItem() {
            var TicketID = "<%= TicketID %>";
            var ModuleName = "<%=ModuleName%>";
            var param = "TicketID=" + TicketID + "&" + "ModuleName=" + ModuleName;
            window.parent.UgitOpenPopupDialog('<%= ModuleResourceAddEditUrl %>', param, 'Add New Resource', '800px', '550px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
        function ResourceEditItem(key) {
            var TicketID = "<%= TicketID %>";
            var ModuleName = "<%=ModuleName%>";
            var param = "ID=" + key + "&" + "TicketID=" + TicketID + "&" + "ModuleName=" + ModuleName;
            window.parent.UgitOpenPopupDialog('<%= ModuleResourceAddEditUrl %>', param, 'Edit Resource', '800px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));

        }

        function NewBudgetItem() {
            var TicketID = "<%= TicketID %>";
            var ModuleName = "<%=ModuleName%>";
            var param = "TicketID=" + TicketID + "&" + "ModuleName=" + ModuleName;
            window.parent.UgitOpenPopupDialog('<%= BudgetAddEditUrl %>', param, 'Add New Budget', '800px', '500px', 0, escape("<%= Request.Url.AbsoluteUri %>"));
        }
        //var budgetKey;
        //function EditBudgetitem(key, e) {
        //    
        //    budgetKey = key;
        //   // aspxBudgetgrid.GetSelectedFieldValues('BudgetCategory', SelectedFieldValue);
        //    aspxBudgetgrid.GetRowValues(e.visibleIndex, 'BudgetCategory', SelectedFieldValue);

        //}
        function EditBudgetitem(key) {
            var TicketID = "<%= TicketID %>";
            var ModuleName = "<%=ModuleName%>";
            var param = "ID=" + key + "&" + "TicketID=" + TicketID + "&" + "ModuleName=" + ModuleName;
            window.parent.UgitOpenPopupDialog('<%= BudgetAddEditUrl %>', param, 'Edit Budget', '800px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));

        }

        function NewActualsItem(key) {
            var TicketID = "<%= TicketID %>";
            var ModuleName = "<%=ModuleName%>";
            var param = "TicketID=" + TicketID + "&" + "ModuleName=" + ModuleName;
            if (key != '' && key != undefined)
                param = param + "&BudgetId=" + key;
            window.parent.UgitOpenPopupDialog('<%= BudgetActualEditUrl %>', param, 'Add New Actual', '800px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
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
        function ApproveBudget(hvalue) {
            document.getElementById('<%=approveValue.ClientID%>').value = hvalue;
            comntbudget.SetHeaderText("Approve Budget");
            cmntBudgetSave.SetVisible(true);
            cmntBudgetReject.SetVisible(false);
            txtBudgetComment.text = "";
            comntbudget.Show();
        }
        function RejectBudget(rejectValue) {
            document.getElementById('<%=rejectValue.ClientID%>').value = rejectValue;
            comntbudget.SetHeaderText("Reject Budget");
            cmntBudgetSave.SetVisible(false);
            cmntBudgetReject.SetVisible(true);
            txtBudgetComment.text = "";
            comntbudget.Show();
        }
        function showTaskActions(trObj, actualId) {
            $("#actionButtons" + actualId).css("display", "block");
            //var desc = $.trim(unescape($(trObj).find(".taskDesc").html()).replace(/\+/g, " "));
        }

        function hideTaskActions(trObj, actualId) {
            $("#actionButtons" + actualId).css("display", "none");
        }
        function OnContextMenuItemClick(sender, args, type) {
            //debugger;
            sender.allowInsert = false;
            sender.allowEdit = false;
            if (args.objectType == "emptyrow") {
                if (args.item.name == "NewRow") {
                    if (type == 1) {
                        NewBudgetItem();
                    } else if (type == 2) {
                        NewActualsItem();
                    }
                    else if (type == 3) {
                        NewResourceItem();
                    }
                }
            }
            if (args.objectType == "row") {
                var index = args.elementIndex;
                var key = sender.GetRowKey(index);
                switch (args.item.name) {
                    case "NewRow":
                        if (type == 1) {
                            NewBudgetItem();
                        } else if (type == 2) {
                            NewActualsItem();
                        }
                        else if (type == 3) {
                            NewResourceItem();
                        }

                        break;
                    case "EditRow":
                        if (type == 1) {
                            EditBudgetitem(key);
                        } else if (type == 2) {
                            ActualEdit(key);
                        } else if (type == 3) {
                            ResourceEditItem(key);

                        }
                        break;
                    case "DeleteRow":
                        if (type == 1) {
                            document.getElementById('<%=HiddenFieldDeleteBudget.ClientID%>').value = key;
                            $('#<%=BtDeleteBudgetContext.ClientID%>').click();
                        } else if (type == 2) {
                            document.getElementById('<%=HiddenFieldBudgetActual.ClientID%>').value = key;
                            $('#<%=imgDeleteActual.ClientID%>').click();
                        } else if (type == 3) {
                            document.getElementById('<%=HiddenFieldNprResource.ClientID%>').value = key;
                            $('#<%=imgDeleteNprResource.ClientID%>').click();
                        }
                        break;
                    case "Approve":
                        ApproveBudget(key);
                        break;
                    case "Reject":
                        RejectBudget(key);
                        break;
                    case "Actual":
                        NewActualsItem(key);
                }
            }
            else if (args.objectType == undefined) {
                if (args.item.name == "NewRow") {

                    if (type == 1) {
                        NewBudgetItem();
                    } else if (type == 2) {
                        NewActualsItem();
                    }
                    else if (type == 3) {
                        NewResourceItem();
                    }
                }

            }
        }

        function ActualEdit(key) {
            var TicketID = "<%= TicketID %>";
            var ModuleName = "<%=ModuleName%>";
            var ID = key;
            var param = "ID=" + ID + "&" + "TicketID=" + TicketID + "&" + "ModuleName=" + ModuleName + "chetan";
            window.parent.UgitOpenPopupDialog('<%= BudgetActualEditUrl %>', param, 'Edit Actual', '800px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }


        function OnContextMenu(s, e) {
            //debugger;
            if (e.objectType == "row") {
                var menuItemSelectedApprove = e.menu.GetItemByName("Approve");
                var menuItemSelectedReject = e.menu.GetItemByName("Reject");
                var isRowDiscontinued = s.cpBudgetStatus[e.index];
                var flag = isRowDiscontinued == 0 ? true : false;
                menuItemSelectedApprove.SetVisible(flag);
                menuItemSelectedReject.SetVisible(flag);
            }
        }

        function OpenImportExcel() {
            window.parent.UgitOpenPopupDialog('<%= importUrl %>', "", 'Import Budget', '400px', '210px', 0, escape("<%= Request.Url.AbsolutePath %>"));
            return false;
        }

    </script>


    <asp:HiddenField ID="approveValue" runat="server" />
    <asp:HiddenField ID="rejectValue" runat="server" />
    <dx:ASPxCallbackPanel ID="updateNPR" runat="server">
        <PanelCollection>
            <dx:PanelContent>
                <asp:HiddenField runat="server" ID="subCategoryHidden" Value="" />
                <div>
                    <dx:ASPxSpreadsheet ID="spdExporter" runat="server" Visible="false"></dx:ASPxSpreadsheet>
                </div>

                <fieldset id="fldsetNPRResource" runat="server">
                    <legend class="nprPlanning_title">Resources</legend>
                    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                        function UpdateGridHeight() {
                            aspxModuleResourceList.SetHeight(0);
                            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                            if (document.body.scrollHeight > containerHeight)
                                containerHeight = document.body.scrollHeight;
                            aspxModuleResourceList.SetHeight(containerHeight);
                        }
                        window.addEventListener('resize', function (evt) {
                            if (!ASPxClientUtils.androidPlatform)
                                return;
                            var activeElement = document.activeElement;
                            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                        });
                    </script>
                    <ugit:ASPxGridView ID="aspxModuleResourceList" ClientInstanceName="aspxModuleResourceList" runat="server" AutoGenerateColumns="false"
                        Width="100%" Styles-Footer-Font-Bold="true" Styles-Footer-HorizontalAlign="Center" OnDataBinding="aspxModuleResourceList_DataBinding"
                        SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" EnableCallBacks="false" EnableViewState="true" OnCustomColumnDisplayText="aspxModuleResourceList_CustomColumnDisplayText"
                        OnHtmlRowPrepared="aspxModuleResourceList_HtmlRowPrepared" OnHtmlDataCellPrepared="aspxModuleResourceList_HtmlDataCellPrepared"
                        CssClass="customgridview homeGrid">
                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName=" " Width="55px" VisibleIndex="8">
                                <DataItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" ImageUrl="/Content/Images/redNew_delete.png"
                                        Style="float: right; margin-top: 4px; width: 16px;" OnClick="imgDelete_Click1" CommandArgument='<%# Eval("ID") %>' OnClientClick="return deleteResource();" />
                                    <img runat="server" id="imgEdit" src="~/Content/Images/editNewIcon.png" alt="Edit" style="float: right; width: 16px; margin-top: 4px;" />
                                </DataItemTemplate>
                                <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsCommandButton>
                            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                        </SettingsCommandButton>
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="NoOfFTEs" SummaryType="Sum" ShowInColumn="NoOfFTEs" DisplayFormat="{0}" />
                            <dx:ASPxSummaryItem FieldName="_ResourceType" SummaryType="Sum" DisplayFormat="Total" ShowInColumn="_ResourceType" />
                        </TotalSummary>


                        <SettingsEditing Mode="Inline" />
                        <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <ClientSideEvents ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e , '3'); }" />
                        <Settings ShowFooter="true" ShowColumnHeaders="true" ShowStatusBar="Hidden" />
                        <SettingsPager Visible="false" Mode="ShowAllRecords"></SettingsPager>
                        <SettingsText EmptyDataRow="No record found." ConfirmDelete=""></SettingsText>
                        <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                            <SelectedRow BackColor="#D9D5D5" ForeColor="Black"></SelectedRow>
                            <Cell HorizontalAlign="Center"></Cell>
                            <Header HorizontalAlign="Center" Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                            <CommandColumnItem Paddings-PaddingLeft="5px"></CommandColumnItem>
                            <Row CssClass="homeGrid_dataRow"></Row>
                            <Footer CssClass="budgetGrid_footerRow"></Footer>
                        </Styles>
                    </ugit:ASPxGridView>
                    <script type="text/javascript">
                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                    </script>
                    <dx:ASPxButton RenderMode="Link" runat="server" AutoPostBack="false" ClientSideEvents-Click=" function() {NewResourceItem();}" Text="Add New Resource" CssClass="nprPlanning_linkLable">
                        <Image Url="/content/images/sub_Task.png"></Image>
                    </dx:ASPxButton>
                </fieldset>
                <div style="display: none;">
                    <asp:HiddenField ID="HiddenFieldNprResource" runat="server" />
                    <asp:ImageButton ID="imgDeleteNprResource" runat="server" AlternateText="Delete"
                        Style="float: right; padding-right: 10px;" OnClick="imgDelete_Click1" CommandArgument='0' OnClientClick="return deleteResource();" />
                </div>
                <fieldset id="fldsetNPRBudget" runat="server">
                    <legend class="nprPlanning_title">Budget Items
                    <div style="float: right">
                        <dx:ASPxButton ID="btnImport" RenderMode="Link" AutoPostBack="false" runat="server" ToolTip="Import" Image-Width="18" Image-Url="~/Content/Images/importTasks.png">
                            <ClientSideEvents Click="function(s,e){OpenImportExcel();}" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnExport" runat="server" RenderMode="Link" AutoPostBack="false" ToolTip="Export" Image-Width="18" Image-Url="~/Content/Images/exportTasks.png" OnClick="btnExport_Click">
                        </dx:ASPxButton>
                    </div>
                    </legend>
                    <asp:Label ID="budgetMessage" runat="server" EnableViewState="false" CssClass="errormessage-block ugitlight1lightest" ForeColor="Blue"></asp:Label>
                    <asp:HiddenField ID="sortingExp" runat="server" />
                    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                        function UpdateGridHeight() {
                            aspxBudgetgrid.SetHeight(0);
                            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                            if (document.body.scrollHeight > containerHeight)
                                containerHeight = document.body.scrollHeight;
                            aspxBudgetgrid.SetHeight(containerHeight);
                        }
                        window.addEventListener('resize', function (evt) {
                            if (!ASPxClientUtils.androidPlatform)
                                return;
                            var activeElement = document.activeElement;
                            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                        });
                    </script>
                    <ugit:ASPxGridView ID="aspxBudgetgrid" ClientInstanceName="aspxBudgetgrid" runat="server" AutoGenerateColumns="false" Styles-Footer-Font-Bold="true"
                        Styles-Footer-HorizontalAlign="Center" Width="100%" OnDataBinding="aspxBudgetgrid_DataBinding" OnSummaryDisplayText="aspxBudgetgrid_SummaryDisplayText"
                        SettingsText-EmptyDataRow="No record found." KeyFieldName="Id" EnableCallBacks="false" EnableViewState="true" OnHtmlDataCellPrepared="aspxBudgetgrid_OnHtmlDataCellPrepared"
                        OnHtmlRowPrepared="aspxBudgetgrid_HtmlRowPrepared" OnFillContextMenuItems="aspxBudgetgrid_FillContextMenuItems" OnCustomJSProperties="aspxBudgetgrid_CustomJSProperties"
                        CssClass="customgridview homeGrid">
                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName=" " Width="80px" VisibleIndex="8" CellStyle-HorizontalAlign="Center">
                                <DataItemTemplate>
                                    <asp:ImageButton ID="BtDeleteBudget" runat="server" AlternateText="Delete" ImageUrl="/Content/Images/redNew_delete.png"
                                        Style="float: right;" OnClick="BtDeleteBudget_Click1" CommandArgument='<%# Eval("ID") %>' OnClientClick="return deletedBudget();" />
                                    <img id="imgEdit" runat="server" src="/content/images/editNewIcon.png" alt="Edit" style="float: right; width: 16px;" />
                                    <img id="imgApprove" runat="server" src="/Content/Images/Approved16x16.png" alt="Approve" style="width: 16px;" />
                                    <img id="imgReject" runat="server" src="/Content/Images/Rejected16x16.png" alt="Reject" style="width: 16px;" />
                                </DataItemTemplate>
                                <Settings AllowAutoFilter="true" AllowSort="true" AllowHeaderFilter="False" />
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsCommandButton>
                            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="nprPlanningGrid_btn"></ShowAdaptiveDetailButton>
                            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="nprPlanningGrid_btn"></HideAdaptiveDetailButton>
                        </SettingsCommandButton>
                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="BudgetAmount" SummaryType="Sum" ShowInColumn="BudgetAmount" />
                        </TotalSummary>
                        <SettingsEditing Mode="Inline" />
                        <%--<SettingsBehavior AllowSort="false" ConfirmDelete="true" />--%>

                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsBehavior ProcessSelectionChangedOnServer="false" />
                        <ClientSideEvents ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e,'1'); }" ContextMenu="OnContextMenu" />
                        <Settings ShowColumnHeaders="true" ShowStatusBar="Hidden" ShowFilterRowMenu="false" ShowHeaderFilterButton="true" ShowFilterRow="false" ShowFilterBar="Auto" ShowFooter="true" ShowGroupPanel="false" />
                        <SettingsText EmptyDataRow="No record found."></SettingsText>
                        <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                            <%--<SelectedRow BackColor="#D9D5D5" ForeColor="Black"></SelectedRow>--%>
                            <%--<Cell HorizontalAlign="Center"></Cell>--%>
                            <Header HorizontalAlign="Center" Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                            <%--<CommandColumnItem Paddings-PaddingLeft="5px"></CommandColumnItem>--%>
                            <Row CssClass=" homeGrid_dataRow"></Row>
                            <Footer CssClass="budgetGrid_footerRow"></Footer>
                        </Styles>
                        <%--<SettingsExport EnableClientSideExportAPI="true" />
                        <SettingsContextMenu Enabled="true">
                            <RowMenuItemVisibility ExportMenu-Visible="true" />
                        </SettingsContextMenu>--%>
                         
                    </ugit:ASPxGridView>
                    
                    <dx:ASPxSpreadsheet ID="SpreadSheetConfigVar" runat="server" Visible="false"></dx:ASPxSpreadsheet>
                    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                    </script>
                    <dx:ASPxButton RenderMode="Link" runat="server" ID="newBudget" AutoPostBack="false" ClientSideEvents-Click=" function() {NewBudgetItem();}" Text="Add New Budget Item" CssClass="nprPlanning_linkLable">
                        <Image Url="/content/images/sub_Task.png"></Image>
                    </dx:ASPxButton>

                </fieldset>
                <div style="display: none;">
                    <asp:HiddenField ID="HiddenFieldDeleteBudget" runat="server" />
                    <asp:ImageButton ID="BtDeleteBudgetContext" runat="server" AlternateText="Delete" ImageUrl="/Content/Images/delete-icon.png" CommandArgument="0"
                        Style="float: right; padding-right: 10px;" OnClick="BtDeleteBudget_Click1" OnClientClick="return deletedBudget();" />
                </div>
                <fieldset id="PMMBudgetActuals" runat="server">
                    <legend class="nprPlanning_title">Budget Actuals</legend>
                    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                        function UpdateGridHeight() {
                            aspxBudgetActualsgrid.SetHeight(0);
                            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                            if (document.body.scrollHeight > containerHeight)
                                containerHeight = document.body.scrollHeight;
                            aspxBudgetActualsgrid.SetHeight(containerHeight);
                        }
                        window.addEventListener('resize', function (evt) {
                            if (!ASPxClientUtils.androidPlatform)
                                return;
                            var activeElement = document.activeElement;
                            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                        });
                    </script>
                    <ugit:ASPxGridView ID="aspxBudgetActualsgrid" ClientInstanceName="aspxBudgetActualsgrid" runat="server" AutoGenerateColumns="false"
                        OnHtmlDataCellPrepared="aspxBudgetActualsgrid_OnHtmlDataCellPrepared" OnSummaryDisplayText="aspxBudgetActualsgrid_SummaryDisplayText"
                        Width="100%" Styles-Footer-Font-Bold="true" Styles-Footer-HorizontalAlign="Center" EnableCallBacks="false" EnableViewState="true"
                        SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" OnHtmlRowPrepared="aspxBudgetActualsgrid_HtmlRowPrepared"
                        CssClass="customgridview homeGrid" OnCustomColumnDisplayText="aspxModuleResourceList_CustomColumnDisplayText">
                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                        <Columns>
                            <dx:GridViewDataTextColumn Width="55px" VisibleIndex="7">
                                <DataItemTemplate>
                                    <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" ImageUrl="/Content/Images/redNew_delete.png"
                                        Style="float: right; padding-right: 10px;" OnClick="imgDelete_Click" CommandArgument='<%# Eval("ID") %>' OnClientClick="return deletedBudget();" />
                                    <img id="imgEdit" runat="server" src="/Content/Images/editNewIcon.png" alt="Edit" style="float: right; width: 16px;" />
                                </DataItemTemplate>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                        <SettingsCommandButton>
                            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                        </SettingsCommandButton>

                        <TotalSummary>
                            <dx:ASPxSummaryItem FieldName="BudgetAmount" SummaryType="Sum" ShowInColumn="BudgetAmount" />
                        </TotalSummary>
                        <SettingsEditing Mode="Inline" />
                        <%--<SettingsBehavior AllowSort="false" ConfirmDelete="false" />--%>

                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <ClientSideEvents ContextMenuItemClick="function(s,e) { OnContextMenuItemClick(s, e , '2'); }" />
                        <Settings ShowColumnHeaders="true" ShowStatusBar="Hidden" ShowFilterRowMenu="false" ShowHeaderFilterButton="true" ShowFilterRow="false" ShowFilterBar="Auto" ShowFooter="true" ShowGroupPanel="false" />
                        <SettingsText EmptyDataRow="No record found."></SettingsText>
                        <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                            <SelectedRow BackColor="#D9D5D5" ForeColor="Black"></SelectedRow>
                            <Cell HorizontalAlign="Center"></Cell>
                            <Header HorizontalAlign="Center" Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                            <Row CssClass="homeGrid_dataRow"></Row>
                            <Footer CssClass="budgetGrid_footerRow"></Footer>
                            <CommandColumnItem Paddings-PaddingLeft="5px"></CommandColumnItem>
                        </Styles>
                    </ugit:ASPxGridView>
                    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                            UpdateGridHeight();
                        });
                    </script>
                    <dx:ASPxButton RenderMode="Link" runat="server" ID="newActual" AutoPostBack="false" ClientSideEvents-Click=" function() {NewActualsItem();}" Text="Add New Actual Item" CssClass="nprPlanning_linkLable">
                        <Image Url="/content/images/sub_Task.png"></Image>
                    </dx:ASPxButton>
                </fieldset>
                <div style="display: none;">
                    <asp:HiddenField ID="HiddenFieldBudgetActual" runat="server" />
                    <asp:ImageButton ID="imgDeleteActual" runat="server" AlternateText="Delete" ImageUrl="/content/images/delete-icon.png"
                        Style="float: right; padding-right: 10px;" OnClick="imgDelete_Click" CommandArgument='0' OnClientClick="return deletedBudget();" />
                </div>
                <fieldset id="fsLabourCharges">
                    <%-- Labour Charges --%>
                    <asp:Panel ID="pnlLabourCharges" runat="server">
                        <div class="mainblock" style="width: 100%;">
                            <fieldset class="clsLegendStyle">
                                <legend class="nprPlanning_title">Timesheet Cost </legend>
                                <%-- Project Actuals--%>
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                <asp:Panel ID="pnlResourceCostView" runat="server" Width="100%">
                                </asp:Panel>
                            </fieldset>
                        </div>
                    </asp:Panel>
                </fieldset>
                <div class="mainblock">
                    <fieldset>
                        <legend>
                            <span class="fleft">
                                <asp:ImageButton ImageUrl="/content/images/previous-arrowBlue.png" Width="24" ID="previousWeek" runat="server" OnClick="PreviousYearReadOnly_Click" />
                            </span>
                            <span class="fleft calenderyearnum">
                                <%= currentYear %>
                            </span><span class="fleft">
                                <asp:ImageButton ImageUrl="/content/images/Next-arrowBlue.png" Width="24" ID="nextWeek" runat="server" OnClick="NextYearReadOnly_Click" />
                            </span>
                            <asp:HiddenField runat="server" ID="currentYearHidden" Value="" />
                        </legend>
                        <div class="worksheetpanel-m" style="padding-top: 10px;">
                            <fieldset id="MonthlyBudgetSummary" runat="server">
                                <legend class="nprPlanning_title" runat="server">
                                    <dx:ASPxLabel ID="lblBudgetSummary" runat="server" CssClass="budget-summary-title" />
                                </legend>

                                <dx:ASPxPivotGrid ID="ASPxPivotGrid1" runat="server" ClientInstanceName="ASPxPivotGrid1" Width="100%" OptionsView-ShowRowGrandTotals="False" Visible="false">
                                    <Fields>
                                        <dx:PivotGridField FieldName="AllocationStartDate" Area="ColumnArea" Options-AllowSort="False" Options-AllowSortBySummary="False" SortMode="None" GroupInterval="DateMonth" CellFormat-FormatString="MMM"></dx:PivotGridField>
                                        <dx:PivotGridField FieldName="Total" Area="DataArea"></dx:PivotGridField>
                                        <dx:PivotGridField FieldName="Title" Area="RowArea" Options-AllowSort="False" Options-AllowSortBySummary="False" SortMode="None"></dx:PivotGridField>
                                    </Fields>
                                </dx:ASPxPivotGrid>

                                <%--new code--%>
                                <asp:ListView ID="lviewBudget" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1"
                                    DataKeyNames="Category">
                                    <LayoutTemplate>
                                        <table class=" worksheettable" style="border-collapse: collapse"
                                            width="100%" cellpadding="0" cellspacing="0" id="budgetsummary">
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
                                                    <b>Apr</b>
                                                </th>
                                                <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                    <b>May</b>
                                                </th>
                                                <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                    <b>Jun</b>
                                                </th>
                                                <th class="ms-vh2 alncenter" style="text-align: right; padding-right: 5px;">
                                                    <b>Jul</b>
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
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month1")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month2")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month3")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month4")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month5")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month6")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month7")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month8")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month9")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month10")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month11")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month12")%>
                                            </td>
                                            <td class="ms-vb2 alncenter totalbordervartical" style="text-align: right; padding-right: 5px;">
                                                <%#  Eval("Total")%></span>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="ms-alternatingstrong">
                                            <td class="ms-vb2 paddingfirstcell ">
                                                <%#  Eval("Title")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month1")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month2")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month3")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month4")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month5")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month6")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month7")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month8")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month9")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month10")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month11")%>
                                            </td>
                                            <td class="ms-vb2 alncenter" style="text-align: right; padding-right: 5px;">
                                                <%# Eval("Month12")%>
                                            </td>
                                            <td class="ms-vb2 alncenter totalbordervartical" style="text-align: right; padding-right: 5px;">
                                                <%#  Eval("Total")%></span>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:ListView>
                                <%-- new code --%>
                            </fieldset>
                        </div>
                    </fieldset>
                </div>
            </dx:PanelContent>
        </PanelCollection>
    </dx:ASPxCallbackPanel>

    <dx:ASPxPopupControl ID="comntbudget" runat="server" ClientInstanceName="comntbudget" AllowResize="true" MinHeight="250" Width="300"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Above" AllowDragging="true" PopupAnimationType="Fade"
        EnableViewState="false" CloseAction="CloseButton" ShowOnPageLoad="false" background="#000000;">
        <ContentCollection>
            <dx:PopupControlContentControl>

                <dx:ASPxMemo ID="txtBudgetComment" runat="server" ClientInstanceName="txtBudgetComment" Height="100px" Width="100%"></dx:ASPxMemo>
                <div style="margin-top: 10px; padding-left: 4px; float: right;">
                    <div id="div_actuals" class="budgetBtn_wrap row fieldWrap">
                        <div class="rmmNewUserbtn">
                            <div class="RMMBtnWrap">
                                <dx:ASPxButton ID="cmntBudgetSave" ClientInstanceName="cmntBudgetSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="formABudget"
                                    OnClick="cmntBudgetSave_Click" CssClass="primary-blueBtn">
                                    <Image Url="/Content/Images/saveFile_icon.png"></Image>

                                </dx:ASPxButton>
                                <dx:ASPxButton ID="cmntBudgetReject" ClientInstanceName="cmntBudgetReject" runat="server" Text="Save" ToolTip="Save" ValidationGroup="formABudget"
                                    OnClick="cmntBudgetReject_Click" CssClass="primary-blueBtn">
                                    <Image Url="/Content/Images/saveFile_icon.png"></Image>
                                </dx:ASPxButton>
                            </div>
                        </div>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function saveInHidden(obj) {
            document.getElementById('<%= subCategoryHidden.ClientID %>').value = obj.options[obj.selectedIndex].value;
        }

        function previousYearBudget() {
            var year = Number($.trim($(".calenderyearnum").text()));
            var preYear = year - 1;
            var url = window.document.location.href;
            set_cookie("budgetyear", preYear, null, _spPageContextInfo.webServerRelativeUrl, null, null);
        }
        function nextYearBudget() {
            var year = Number($.trim($(".calenderyearnum").text()));
            var nextYear = year + 1;
            var url = window.document.location.href;
            set_cookie("budgetyear", nextYear, null, _spPageContextInfo.webServerRelativeUrl, null, null);
        }
    </script>

</asp:Panel>
<asp:Panel ID="viewMode" runat="server" CssClass="">

    <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .heading123 {
            font-weight: bold;
            padding-right: 5px;
            padding-top: 2px;
        }

        /*.scoreheading {
            font-weight: bold;
            padding-right: 5px;
        }*/

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

        /*.calenderyearnum {
            font-weight: bold;
            padding-top: 1px;
            padding-left: 3px;
            padding-right: 3px;
        }*/

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

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function previousYearBudget() {
            var year = Number($.trim($(".calenderyearnum").text()));
            var preYear = year - 1;
            var url = window.document.location.href;
            set_cookie("budgetyear", preYear, null, _spPageContextInfo.webServerRelativeUrl, null, null);
            return false;
        }
        function nextYearBudget() {
            var year = Number($.trim($(".calenderyearnum").text()));
            var nextYear = year + 1;
            var url = window.document.location.href;
            set_cookie("budgetyear", nextYear, null, _spPageContextInfo.webServerRelativeUrl, null, null);
        }
    </script>
</asp:Panel>

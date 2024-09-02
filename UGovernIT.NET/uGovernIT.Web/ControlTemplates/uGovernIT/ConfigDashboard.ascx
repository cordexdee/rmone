<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigDashboard.ascx.cs" Inherits="uGovernIT.Web.ConfigDashboard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function InitalizejQuery() {

         
        //$(".sortable").find("tbody").sortable({
        //    start: function (event, ui) {
        //        sourceKey = $(ui.item[0]).find("input[type='hidden']").val();
        //        sourceIndex = ui.item[0].rowIndex;
        //    }
        //});

        //$(".sortable").find("tbody").sortable({
        //    stop: function (event, ui) {
        //        targetKey = $(ui.item[0]).next().find("input[type='hidden']").val();
        //        if (sourceIndex != 0) {
        //            grid.PerformCallback("DRAGROW|" + sourceKey + '|' + targetKey);
        //        }
        //    }
        //});
    }

    var url = "<%= delegatePageUrl %>";
    var newdashboarduiurl = "<%=newDashboardUIurl%>";
    $(function () {

        $('#<%=saveTemplate.ClientID%>').on('click', function (e) {
            $('#trtemplateName').show();
            $('#trtemplates').hide();
        });

        $('#<%=overrideTemplate.ClientID%>').on('click', function (e) {

            $('#trtemplateName').hide();
            $('#trtemplates').show();
        });

        $('#<%=btnFactTable.ClientID%>').on('click', function (e) {
            
            UgitOpenPopupDialog('<%=FactTableUrl %>', '', 'Fact Table', '90', '85');

        });



        $("#rdTable").bind("change", function () {
            var tables = $(".queryTables111");
            var cachedData = $(".queryCachedData111");
            if (cachedData.css("display") == 'block') {
                cachedData.css("display", "none");
                tables.css("display", "block");

                $("#tablesSpan").css("display", "block");
                $("#factTableClass").css("display", "none");
            }
            else if (cachedData.css("display") == 'none') {
                cachedData.css("display", "block");
                tables.css("display", "none");
                $("#tablesSpan").css("display", "none");
                $("#factTableClass").css("display", "block");
            }
        });
        $("#rdCachedData").bind("change", function () {
            var tables = $(".queryTables111");
            var cachedData = $(".queryCachedData111");
            if (cachedData.css("display") == 'block') {
                cachedData.css("display", "none");
                tables.css("display", "block");
                $("#tablesSpan").css("display", "block");
                $("#factTableClass").css("display", "none");

            }
            else if (cachedData.css("display") == 'none') {
                cachedData.css("display", "block");
                tables.css("display", "none");
                $("#tablesSpan").css("display", "none");
                $("#factTableClass").css("display", "block");
            }
        });
    });
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);
    var notifyId = null;
    function InitializeRequest(sender, args) {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
    }

    function BeginRequestHandler(sender, args) {
        notifyId = AddNotification("Processing ..");
    }

    function EndRequest(sender, args) {
        //do your stuff
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
            //RemoveNotification(notifyId);
        }
    }

    function editDashboard(obj) {
        var dashboardTitle =$(obj).parent().attr("dashboardTitle");
        var dashboardType = $(obj).parent().attr("dashboardType");
        var dashboardID = $(obj).parent().attr("dashboardID");


        var dUrl = url;

        var params = "";
        var title = "";
        if (dashboardType.toLocaleLowerCase() == "panel") {
            params += "control=configdashboardpanel&DashboardID=" + dashboardID;
            title = "Panel Dashboard";
        }
        else if (dashboardType.toLocaleLowerCase() == "chart") {

            params += "control=newdashboardchartui&DashboardID=" + dashboardID;
            title = "Chart Dashboard";
            dUrl = newdashboarduiurl;
        }
        else if (dashboardType.toLocaleLowerCase() == "query") {
            params += "control=configdashboardquery&DashboardID=" + dashboardID;
            title = "Query Dashboard";
        }
        title += ": " + unescape(dashboardTitle).replace(/\+/g, " ");

        window.UgitOpenPopupDialog(dUrl, params, title, 90, 90, false, escape("<%=Request.Url.AbsolutePath %>"));
    }


    function editThisDashboard(dashboardID, dashboardTitle, dashboardType) {
        var dUrl = url;
        
        var params = "";
        var title = "";
        dashboardTitle = dashboardTitle.replace(" ", "+") || "";
        if (dashboardType.toLocaleLowerCase() == "panel") {
            params += "control=configdashboardpanel&DashboardID=" + dashboardID;
            title = "Panel Dashboard";
        }
        else if (dashboardType.toLocaleLowerCase() == "chart") {

            params += "control=newdashboardchartui&DashboardID=" + dashboardID;
            title = "Chart Dashboard";
            dUrl = newdashboarduiurl;
        }
        else if (dashboardType.toLocaleLowerCase() == "query") {
            params += "control=configdashboardquery&DashboardID=" + dashboardID;
            title = "Query Dashboard";
        }
        title += ": " + unescape(dashboardTitle).replace(/\+/g, " ");

        window.UgitOpenPopupDialog(dUrl, params, title, 90, 90, false, escape("<%=Request.Url.AbsolutePath %>"));
    }


    function showViews() {
        var params = "";
        var title = "";

        params += "control=configdashboardviews";
        title = "Dashboard Views";

        window.parent.UgitOpenPopupDialog(url, params, title, "500px", "520px");
    }
    function setFormSubmitToFalse() {
        setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
        return true;
    }

    function saveTemplate() {
        $("#<%=lblmsg.ClientID%>").hide();
        if (grdDashboard.GetSelectedRowCount() == 1) {
            grdDashboard.GetSelectedFieldValues("Title", getSelectedField);

            window.parent.UgitOpenPopupDialog(url, params, title, "500px", "600px");

        }
        else
            alert('Please select at least one dashboard to save as template');
        return false;
    }
    function getSelectedField(selectedValue) {
        $('#<%=templateName.ClientID%>').val(selectedValue);
        pcTemplate.Show();
    }
    function selectionChanged(s, e) {
        /*
         if (s.GetSelectedRowCount() > 1) {
             s.UnselectRowOnPage(e.visibleIndex);
             alert("You cannot select more than one dashboard");
 
         }
         return false;
        */
    }
    function OpenImportExcel() {
        var title = "";
        var params = "";
        title = "Import Dashboard & Query";
        params += "control=dashboardimport"
          <%--  window.parent.UgitOpenPopupDialog('<%= importUrl %>', "Module=CMDB", 'Import Dashboard & Query', '400px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));--%>
        window.parent.UgitOpenPopupDialog(importUrl, params, title, "500px", "600px");
    }


    function duplicateDashboard() {
        loadingPanel.Show();
        if (grdDashboard.GetSelectedRowCount() > 0) {
            return true;
        }
        else {
            loadingPanel.Hide();
            alert('Please select at least one dashboard to duplicate');
            return false;
        }
    }
    function Validate() {
        if (grdDashboard.GetSelectedRowCount() > 0) {
            setFormSubmitToFalse();
            //return true;
        }
        else {
            alert('Please select at least one');
            return false;
        }
    }

    function ConfirmDelete(s, e, Id) {    
        if (confirm("Are you sure you want to delete?") == true) {                 
            __doPostBack('DeleteQuery', Id);
        }
        else {
            e.processOnServer = false;
        }
    }

    function UpdateGridHeight() {
        grdDashboard.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grdDashboard.SetHeight(containerHeight-100);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });

  
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    
    /*.dxgvDataRowHover_UGITCore {
    background: white !important;
    color: #4a6ee2;
}*/
    
    .dxgvDataRowHover_UGITNavyBlueDevEx,.dxgvFocusedRow_UGITNavyBlueDevEx, .dxgvDataRowHover_UGITNavyBlueDevEx.ms-alternatingstrong {
    background-color: transparent !important;
    background-image:none;

    }
    .dxgvDataRowHover_UGITNavyBlueDevEx td.dxgv, .dxgvFocusedRow_UGITNavyBlueDevEx td.dxgv {
        color:#4a6ee2 !important;
    }
    .tasklistview-focused {
        background-color: #f1f1f1 !important;
    }
    .ms-alternatingstrong {
        width: 40px;
        background: #F2F2F2 !important;
    }
    .tasklistview-row:hover .taskSNo {
        background: url("/Content/Images/Sorting-16.png") no-repeat;
        background-position: left 0px top 9px;
        cursor: pointer;
    }
</style>
<dx:ASPxPopupControl ID="pcTemplate" runat="server" ClientInstanceName="pcTemplate"
    HeaderText="Please select Template" CloseAction="CloseButton" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxPanel ID="pnlcontrol" runat="server" Width="400px" DefaultButton="btnOK" CssClass="popupcontrol">
                <PanelCollection>
                    <dx:PanelContent runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td colspan="2">
                                    <asp:Label Text="" runat="server" ID="lblmsg" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="tdradiobutton">
                                    <asp:RadioButton ID="saveTemplate" Text="Add New Template" runat="server" GroupName="template" Checked="true"
                                        Style="float: left; margin-right: 8px" />
                                    <asp:RadioButton ID="overrideTemplate" Text="Override from Template" runat="server" GroupName="template"
                                        Style="float: left; margin-right: 8px" />
                                </td>
                            </tr>
                            <tr id="trtemplateName">
                                <td class="tdradiobutton" style="width: 100px;">Template Name:</td>
                                <td class="tdradiobutton" style="width: 300px;">
                                    <asp:TextBox ID="templateName" runat="server" Width="200px" />
                                </td>
                            </tr>
                            <tr id="trtemplates" style="display: none">
                                <td class="tdradiobutton" style="width: 100px;">Templates:</td>
                                <td class="tdradiobutton" style="width: 300px;">
                                    <asp:DropDownList ID="ddlTemplateList" runat="server" Width="200px"
                                        OnPreRender="DdlTemplateList_PreRender">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px;"></td>
                                <td style="width: 300px;">

                                    <dx:ASPxButton ID="btnOK" runat="server" Text="Save"
                                        ClientInstanceName="saveTemplateButton" OnClick="btnOK_Click"
                                        Style="float: left; margin-right: 8px">
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false"
                                        UseSubmitBehavior="false" Style="float: left; margin-right: 8px">
                                        <ClientSideEvents Click="function(s, e) { pcTemplate.Hide();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    <ContentTemplate>
        <dx:ASPxLoadingPanel ID="filterTicketLoading" ClientInstanceName="loadingPanel" Modal="True" runat="server" ContainerElementID="loadingPanel"
            Text="Please Wait...">
        </dx:ASPxLoadingPanel>
        <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="padding-top:10px;">
            <div class="row">
                <asp:Label CssClass="errormessage-block ms-alternatingstrong" runat="server" ID="accomplishmentMessage"></asp:Label>
                <dx:ASPxCallbackPanel runat="server" ID="AspxCallbackDashboardPanel" ClientInstanceName ="AspxCallbackDashboardPanel"  Enabled="true" OnCallback="AspxCallbackDashboardPanel_Callback">
                        <PanelCollection>
                    <dx:PanelContent runat="server">
                <ugit:ASPxGridView ID="lvDashbaords"  SettingsBehavior-EnableRowHotTrack="true" SettingsBehavior-AllowFocusedRow="true" runat="server" Width="100%" Settings-VerticalScrollableHeight="500" Settings-VerticalScrollBarMode="Auto"
                    ClientInstanceName="grdDashboard" AutoGenerateColumns ="False"  KeyFieldName="ID" 
                       OnHtmlRowPrepared="lvDashbaords_HtmlRowPrepared">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="5%" VisibleIndex="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="ID" Caption="ID" Width="8%" VisibleIndex="1" HeaderStyle-HorizontalAlign="Center"
                            CellStyle-HorizontalAlign="Center" Settings-AllowSort="True" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList">
                            <DataItemTemplate>
                                <%# Eval(DatabaseObjects.Columns.Id) %>
                                <input type="hidden" id="hdnvalsltedrow" value='<%# Container.KeyValue %>' />
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn  CellStyle-CssClass="taskSNo" FieldName="ItemOrder" Caption="Order" Width="7%" VisibleIndex="2" ReadOnly="True" CellStyle-HorizontalAlign="Center"
                            Settings-AllowHeaderFilter="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="DashboardType$" Caption="Type" Width="8%" VisibleIndex="3" HeaderStyle-HorizontalAlign="Left"
                            Settings-AllowSort="true" Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn FieldName="CategoryName" Caption="Category" Width="12%" VisibleIndex="4" HeaderStyle-HorizontalAlign="Left" Settings-AllowSort="true"
                            Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="SubCategory" Caption="Sub-Category" Width="13%" VisibleIndex="5" HeaderStyle-HorizontalAlign="Left" Settings-AllowSort="true"
                            Settings-AllowHeaderFilter="True" SettingsHeaderFilter-Mode="CheckedList">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" Width="15%" VisibleIndex="6" Settings-AllowSort="True" Settings-AllowHeaderFilter="True"
                            SettingsHeaderFilter-Mode="CheckedList">
                            <%--<DataItemTemplate>
                                <a href="javascript:void(0);" dashboardtitle='<%# Server.UrlEncode(Convert.ToString(Eval(DatabaseObjects.Columns.Title)))%>'
                                    dashboardtype='<%# Eval(DatabaseObjects.Columns.DashboardType+"$")%>' dashboardid='<%# Eval(DatabaseObjects.Columns.Id)%>'>
                                    <span onclick="editDashboard(this);"><%# Eval(DatabaseObjects.Columns.Title)%></span>
                                </a>
                            </DataItemTemplate>--%>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="ModifiedByUser$" Caption="Modified By" ReadOnly="True" Width="12%" VisibleIndex="7" CellStyle-HorizontalAlign="Center" Settings-AllowHeaderFilter="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataDateColumn  CellStyle-HorizontalAlign="Center" PropertiesDateEdit-DisplayFormatString="MMM,d,yyyy" FieldName="Modified" Caption="Modified On" ReadOnly="True" Width="13%" VisibleIndex="8" Settings-AllowHeaderFilter="false">
                        </dx:GridViewDataDateColumn>

                        <dx:GridViewDataColumn VisibleIndex="9" Width="7%">
                            <DataItemTemplate>
                                <a href="javascript:void(0);" dashboardtitle='<%# Server.UrlEncode(Convert.ToString(Eval(DatabaseObjects.Columns.Title)))%>'
                                    dashboardtype='<%# Eval(DatabaseObjects.Columns.DashboardType+"$")%>' dashboardid='<%# Eval(DatabaseObjects.Columns.Id)%>'>

                                    <img src="/Content/Images/editNewIcon.png" style="cursor: pointer;" alt="edit"
                                        title="Edit" onclick="editDashboard(this);" width="16" /></a>
                                <span title="Delete">
                                    <dx:ASPxButton ID="btnDelete" runat="server" RenderMode="Link" CssClass="deleteBtn-report" CommandArgument="<%# Eval(DatabaseObjects.Columns.Id)%>" UseSubmitBehavior="false" ClientSideEvents-Click='<%# "function(s, e){  ConfirmDelete(s, e, " + Eval(DatabaseObjects.Columns.Id)  + ");}" %>' >
                                        <Image Url="/content/images/redNew_delete.png" Width="16"></Image>
                                    </dx:ASPxButton>
                                </span>

                            </DataItemTemplate>
                        </dx:GridViewDataColumn>

                    </Columns>
                    <Settings ShowGroupPanel="true"/>
                    <SettingsBehavior ConfirmDelete="true" AllowSelectByRowClick="true" AutoExpandAllGroups="true" />
                    <SettingsText EmptyDataRow="No record found." />
                    <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                        <SelectedRow BackColor="#D9D5D5" ForeColor="Black"></SelectedRow>
                        <Header HorizontalAlign="Center" Font-Bold="true"></Header>
                        <Table CssClass="tasklistview"></Table>
                        <Row CssClass="tasklistview-row"></Row>
                    </Styles>
                    <SettingsPager Mode="ShowAllRecords">
                    </SettingsPager>
                    <SettingsCookies Enabled="true" />
                </ugit:ASPxGridView>
                          </dx:PanelContent>
                            </PanelCollection>
                    </dx:ASPxCallbackPanel>
                <script type="text/javascript">
                    ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                        UpdateGridHeight();
                    });
                    ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                        UpdateGridHeight();
                    });
                </script>
            </div>
            <div class="row reportPopup-footerBtnWrap">
                <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                    <dx:ASPxGlobalEvents ID="ge" runat="server">
                        <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
                    </dx:ASPxGlobalEvents>
                    <div class="footer-btnLeft">
                        <dx:ASPxButton ID="btnShowsViews" runat="server" Text="Views" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){ showViews();}" />
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnDuplicate" runat="server" Text="Duplicate" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){if(duplicateDashboard()){duplicateBtn.DoClick();}}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="duplicateBtn" ClientInstanceName="duplicateBtn" EnableClientSideAPI="true" ClientVisible="false"
                            runat="server" Text="Duplicate" OnClick="btnDuplicate_Click" CssClass="primary-blueBtn">
                        </dx:ASPxButton>

                        <dx:ASPxButton ID="btnSaveAsTemplate" runat="server" Text="Template" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){ return saveTemplate();}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnFactTable" runat="server" Text="Fact Table" CssClass="primary-blueBtn">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnAnalytics" runat="server" Text="Analytics" AutoPostBack="false" Visible="false" CssClass="primary-blueBtn">
                        </dx:ASPxButton>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12 noPadding reportFooter-rightBtn">
                    <div class="footer-btnRight">
                        <dx:ASPxButton ID="ASPxButtonDashboardPanel" runat="server" Text="New Query" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){ openCreateDashboardPanel(event, 3);}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="ASPxButtonChart" runat="server" Text="New Chart" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){ openCreateDashboardPanel(event, 2);}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="ASPxButtonPanel" runat="server" Text="New Panel" AutoPostBack="false" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){ openCreateDashboardPanel(event, 1);}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnExport" OnClick="btExport_Click" EnableClientSideAPI="true" runat="server" CssClass="primary-blueBtn"
                            ToolTip="Export Services" Text="Export" ClientSideEvents-Click="function(s,e){return Validate();}">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btImport" runat="server" Text="Import" ToolTip="Import Services" CssClass="primary-blueBtn">
                            <%-- <ClientSideEvents Click="function(s,e){OpenImportExcel();}"/>--%>
                            <%--  <ClientSideEvents Click="function(s,e){ openCreateDashboardPanel(s, 3);}" />--%>
                        </dx:ASPxButton>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%-- Panel to create dashboard KPI ,Query or chart--%>
<div id="createDashboardPanel" class="ms-alternatingstrong" style="position: absolute; display: none; float: left; height: 275px; width: 300px; border: 1px solid gray; background-color: #ffffff;">
    <div style="float: left; padding-left: 25px; padding-right: 25px; padding-top: 20px;">
        <input type="hidden" id="dashboardPanelType" />
        <div class="fleft">
            <span><b class="panel-heading111"></b></span>
        </div>
        <div class="fleft">
            <span class="panel-description111"></span>
        </div>
        <div class="fleft" style="padding-top: 20px;">
            <span class="fleft" style="width: 100%">Title<b style="color: Red">*</b>:</span>
            <span class="fleft" style="width: 100%">
                <input style="width: 247px;" type="text" id="dashbaordTitle" /></span>
        </div>
        <span class="fleft" style="width: 100%; display: none; color: Red;" id="titleError">Title is required</span>
        <br />
        <div class="fleft" style="padding-top: 5px;">
            <span class="fleft" style="width: 100%">Template:</span> <span class="fleft" style="width: 100%">
                <asp:DropDownList ID="ddlChartPanelTemplates" Width="247" CssClass="chartpanel-templates111"
                    runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlKPIPanelTemplates" Width="247" CssClass="kpipanel-templates111"
                    runat="server">
                </asp:DropDownList>
                <asp:DropDownList ID="ddlQueryPanelTemplates" Width="247" CssClass="querypanel-templates111"
                    runat="server">
                </asp:DropDownList>
            </span>
        </div>
        <div class="fleft dashboardtables-container111" style="padding-top: 5px;">
            <div id='choiceInputData' style="padding-bottom: 5px;">
                <span style="color: Black;">
                    <input type="radio" value="tables" class="tablesClass" name="inputChoice" id="rdTable" />Tables</span>
                <span style="color: Black;">
                    <input type="radio" name="inputChoice" value="cachedData" class="cachedDataClass"
                        id="rdCachedData" />Fact Tables</span>
            </div>
            
            <div class="tablepickerbox">
                <span id="tablesSpan" class="fleft" style="width: 100%">Tables</span>
                <span class="fleft" style="width: 100%">
                    <asp:DropDownList Width="247" CssClass="dashboardtables111" ID="ddlDashboardTables"
                        runat="server" OnLoad="DdlDashboardTables_Load">
                    </asp:DropDownList>

                    <asp:DropDownList Width="247" Style="display: none" CssClass="queryTables111" ID="ddlQueryTables"
                        runat="server">
                    </asp:DropDownList>
                </span>
            </div>
            <span class="fleft" style="width: 100%; display: none; color: Red;" id="dashboardTableError">Please select dashboard table</span>
            <span id='factTableClass' class="fleft" style="width: 100%">Fact Tables</span>
            <span class="fleft" style="width: 100%">
                <asp:DropDownList Width="247" CssClass="queryCachedData111" ID="ddlCachedData"
                    runat="server" OnLoad="DdlDashboardTables_Load">
                </asp:DropDownList>
            </span>

        </div>
        <div class="actionBtn-wrap" style="padding-right:16px;">
            <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s,e){cancelCreate(); e.processOnServer = false;}" />
            </dx:ASPxButton>

            <dx:ASPxButton ID="btCreateDashboard" runat="server" Text="Create" CssClass="primary-blueBtn">
                <ClientSideEvents Click="function(s,e){createDashboard()}" />
            </dx:ASPxButton>
        </div>
    </div>
</div>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function customChangeOnTemplate(obj) {
        if ($(obj).val() == "0") {
            $("#createDashboardPanel .dashboardtables-container111").show("fast");
        }
        else {
            $("#createDashboardPanel .dashboardtables-container111").hide("fast");
        }
    }

    function openCreateDashboardPanel(evt, dashboardType) {
        
        var top = evt.pageY - 290;
        var left = evt.pageX - 140;

        $("#createDashboardPanel").css({ "left": left + "px", "top": top + "px" });
        $("#createDashboardPanel").show("slide");
        $("#createKPIPanelBtn").removeClass("ms-alternatingstrong");
        $("#createChartPanelBtn").removeClass("ms-alternatingstrong");
        $("#createDashboardPanel .tablepickerbox").css("display", "block");

        $("#createDashboardPanel #dashboardPanelType").val(dashboardType);
        if (dashboardType == 1) {
            $("#createKPIPanelBtn").addClass("ms-alternatingstrong");
            $("#createDashboardPanel .panel-heading111").html("New Panel Dashboard:");
            $("#createDashboardPanel .panel-description111").html("Panel is a performance Indicators which helps to ensure the quality of work.");
            $("#createDashboardPanel .kpipanel-templates111").css("display", "block");
            $("#createDashboardPanel .chartpanel-templates111").css("display", "none");
            $("#createDashboardPanel .querypanel-templates111").css("display", "none");
            $("#createDashboardPanel .dashboardtables111").css("display", "block");
            $("#createDashboardPanel .queryTables111").css("display", "none");
            $("#createDashboardPanel .query-category").css("display", "none");
            $("#choiceInputData").css("display", "none");
            $("#factTableClass").css("display", "none");
            $("#createDashboardPanel .queryCachedData111").css("display", "none");
            $("#tablesSpan").css("display", "block");
            $("#createDashboardPanel .tablepickerbox").css("display", "none");
        }
        else if (dashboardType == 2) {
            $("#createChartPanelBtn").addClass("ms-alternatingstrong");
            $("#createDashboardPanel .panel-heading111").html("New Chart Dashboard:");
            $("#createDashboardPanel .panel-description111").html("You create chart of any kind which help to you to understand the data in graphical way.");
            $("#createDashboardPanel .kpipanel-templates111").css("display", "none");
            $("#createDashboardPanel .chartpanel-templates111").css("display", "block");
            $("#createDashboardPanel .querypanel-templates111").css("display", "none");
            $("#createDashboardPanel .dashboardtables111").css("display", "block");
            $("#createDashboardPanel .queryTables111").css("display", "none");
            $("#createDashboardPanel .query-category").css("display", "none");
            $("#choiceInputData").css("display", "none");
            $("#factTableClass").css("display", "none");
            $("#createDashboardPanel .queryCachedData111").css("display", "none");
            $("#tablesSpan").css("display", "block");


        }

        else if (dashboardType == 3) {

            $("#createChartPanelBtn").addClass("ms-alternatingstrong");
            $("#createDashboardPanel .panel-heading111").html("New Query Dashboard:");
            $("#createDashboardPanel .panel-description111").html("You create query of any kind which help to you to create alerts");
            $("#createDashboardPanel .kpipanel-templates111").css("display", "none");
            $("#createDashboardPanel .chartpanel-templates111").css("display", "none");
            $("#createDashboardPanel .querypanel-templates111").css("display", "block");
            $("#createDashboardPanel .dashboardtables111").css("display", "none");
            $("#createDashboardPanel .queryTables111").css("display", "block");
            $("#createDashboardPanel .queryTables111").css("display", "none");
            $("#createDashboardPanel .query-category").css("display", "block");
            $("#choiceInputData").css("display", "block");
            $("#factTableClass").css("display", "block");
            $("#createDashboardPanel .queryCachedData111").css("display", "block");
            $("#tablesSpan").css("display", "none");
            $('#rdCachedData').attr('checked', true);

        }


        $("#createDashboardPanel #dashbaordTitle").val("");
        $("#createDashboardPanel #dashbaordTitle").get(0).focus();
        $("#createDashboardPanel .kpipanel-templates111").val("0");
        $("#createDashboardPanel .chartpanel-templates111").val("0");
        $("#createDashboardPanel .dashboardtables111").val("");
    }

    function cancelCreate() {
        $("#createDashboardPanel").hide("medium");
        $("#createKPIPanelBtn").removeClass("ms-alternatingstrong");
        $("#createChartPanelBtn").removeClass("ms-alternatingstrong");
        $("#ddlDashboardTables").css("display", true);
    }

    function createDashboard() {

        var isError = false;
        $("#createDashboardPanel #titleError").hide();
        $("#createDashboardPanel #dashboardTableError").hide();

        var type = $("#createDashboardPanel #dashboardPanelType").val();
        var title = $("#createDashboardPanel #dashbaordTitle").val();
        var kpiTemplate = $("#createDashboardPanel .kpipanel-templates111").val();
        var chartTemplate = $("#createDashboardPanel .chartpanel-templates111").val();
        var dashboardTable = $("#createDashboardPanel .dashboardtables111").val();
        var factTable = $("#createDashboardPanel .queryCachedData111").val();
        var queryTemplate = $("#createDashboardPanel .querypanel-templates111").val();
        var category = $("#createDashboardPanel .query-category").val();


        if ($.trim(title) == "") {
            $("#createDashboardPanel #titleError").show();
            isError = true;
        }

        if (kpiTemplate == "0" && dashboardTable == "0" && dashboardTable == "") {
            $("#createDashboardPanel #dashboardTableError").show();
            isError = true;
        }

        if (!isError) {
            var url = "<%=delegatePageUrl %>";
            var params = "";
            var popupTitle = "";
            if (type == "1") {
                params += "control=configdashboardpanel&templateID=" + kpiTemplate + "&dashboardTitle=" + escape(title);
                popupTitle = "New Panel Dashboard";
            }
            else if (type == "2") {
                url = newdashboarduiurl;
                params += "control=newdashboardchartui&templateID=" + chartTemplate + "&factTable=" + dashboardTable + "&dashboardTitle=" + escape(title);
                popupTitle = "New Chart Dashboard";
            }
            else if (type == "3") {
                dashboardTable = $("#createDashboardPanel .queryTables111").val();
                var val = $('input:radio[name=inputChoice]:checked').val();
                if (val == 'tables') {
                    factTable = "";
                }
                else if (val == 'cachedData') {
                    dashboardTable = "";
                }

                params += "control=configdashboardquery&isudlg=1&templateID=" + queryTemplate + "&queryTable=" + dashboardTable + "&dashboardTitle=" + escape(title) + "&categoryName=" + category + "&factTable=" + factTable;
                popupTitle = "New Query Dashboard";

            }
            cancelCreate();
            window.parent.UgitOpenPopupDialog(url, params, popupTitle, 90, 85, false, escape("<%=Request.Url.AbsolutePath %>"));
        }
    }

</script>

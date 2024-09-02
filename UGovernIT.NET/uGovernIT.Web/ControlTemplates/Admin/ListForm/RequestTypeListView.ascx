<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestTypeListView.ascx.cs" Inherits="uGovernIT.Web.RequestTypeListView" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .StaticMenuStyle a {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }

    .aa {
        height: 20px;
    }

    a, img {
        border: 0px;
    }

    .selectednode td, .selectednode td a {
        color: #000 !important;
    }

    /*.ddlModule {
        min-width: 100px;
    }*/

    .chkbox {
        margin-top: -2px;
        display: inline-block;
    }

    input[type="checkbox"] {
        /*margin: 4px 0 0;*/
    }

    table th:first-child {
        width: 10px !important;
    }
    .dxtlHeader_UGITNavyBlueDevEx td.dxtl {
        white-space: nowrap;
        text-align: left;
        overflow: hidden;
        color: #4b4b4b;
    }

    .dxtlControl_UGITNavyBlueDevEx a {
        color: #4b4b4b;
        text-decoration: none;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function editMultipleRequestType() {

        var selectedVals = aspxtreelistRequestType.GetVisibleSelectedNodeKeys();
        if (selectedVals.length > 0) {
            var ids = selectedVals.join(",");
            //console.log(('<%= addNewItem%>').replace('ID=0', 'ID=' + ids));
            javascript: UgitOpenPopupDialog(('<%= addNewItem%>').replace('ID=0', 'ID=' + ids), '', 'Request Type - Edit Item', '653', '900', 0, '<%= Server.UrlEncode(Request.Url.AbsolutePath)%>', 'true')
        }
        else {
            alert("Please select request type to edit.");
        }

        return false;
    }

    function ShowMessage() {
        alert("Please select one or more request types to edit.");
    }

    //function ImportExportItemClick(s, e) {
    //    var actionType = s.popupElementIDList[0];
    //    setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);

    //    if (e.item.name == "Excel") {
    //        var exportUrl = window.location.href;
    //        exportUrl += "&initiateExport=true&exportType=excel";
    //        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    //        return false;
    //    }
    //    else if (e.item.name == "XML") {
    //        var exportUrl = window.location.href;
    //        exportUrl += "&initiateExport=true&exportType=xml";
    //        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    //        return false;
    //    }
    //}
    function ImportExportItemClick(s, e) {
        var actionType = s.popupElementIDList[0];
        setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
        
        if (e.item.name == "Excel") {
            
            var btnExport = document.getElementById('<%=btnExport.ClientID%>');
            btnExport.click(s, e);
            return true;
             
         }
         else if (e.item.name == "XML") {
            var btnExportXML = document.getElementById('<%=btnExportXML.ClientID%>');
            btnExportXML.click(s, e);
            return true;
         }
     }

    function downloadExcel(obj) {
        var exportUrl = window.location.href;
        exportUrl += "&initiateExport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
        return false;

    }
    function downloadxml(obj) {
        var exportUrl = window.location.href;
        exportUrl += "&initiateExport=true&exportType=xml";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
        return false;

    }


    function OpenImportExcel() {
        window.UgitOpenPopupDialog('<%= importUrl %>', "", 'Import Request Type', '400px', '210px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    <%if (!string.IsNullOrWhiteSpace(selectedIDs))
    {%>
    editMultipleRequestType("<%= selectedIDs%>");
    <%}%>

    function deleteMultipleRequestType() {
        var selectedVals = aspxtreelistRequestType.GetVisibleSelectedNodeKeys();
        if (selectedVals.length > 0) {
            if (confirm("Are you sure you want to delete the selected request types?")) {
                var ids = selectedVals.join(",");
                $(".btDeleteItem_Top").trigger("click");
                loadingPanel.SetText("Please Wait...");
                loadingPanel.Show();
            }
        }
        else {
            alert("Please select the request type(s) to delete.");
        }
    }
    function undeleteMultipleRequestType() {
        var selectedVals = aspxtreelistRequestType.GetVisibleSelectedNodeKeys();
        if (selectedVals.length > 0) {
            if (confirm("Are you sure you want to un-delete the selected request types?")) {
                var ids = selectedVals.join(",");
                $(".btUnDeleteItem_Top").trigger("click");
                loadingPanel.Show();
            }
        }
        else {
            alert("Please select the request type(s) to delete.");
        }
    }

    function MoveToProduction(obj) {
        var url = '<%=moveToProductionUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', 'Migrate Change(s)', '300px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function expandAllTask() {
        aspxtreelistRequestType.ExpandAll();
    }
    function collapseAllTask() {
        aspxtreelistRequestType.CollapseAll();
    }

    function duplicateRequestType(s, e) {
        var requestType = '';
        var category = '';
        var subCat = '';
        var focusNode = aspxtreelistRequestType.GetFocusedNodeKey()

        if (focusNode.includes('_')) {
            var arr = focusNode.split('_');
            requestType = arr[arr.length - 1];
            subCat = 'Sub Category: ';
        }
        else if (isNaN(focusNode) == false)
            requestType = 'request type(s)';
        else {
            category = 'Category: ';
            requestType = focusNode;
        }

        if (confirm('Do you want to duplicate selected ' + category + subCat + requestType + ' ?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
</script>
<asp:HiddenField ID="hdnModuleList" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row reqType-headerWrap">
        <div class="col-md-3 col-sm-2 col-xs-12 noPadding">
            <div class="ms-formtable accomp-popup">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlModule" CssClass="itsmDropDownList aspxDropDownList" runat="server" AppendDataBoundItems="true" onchange="loadingPanel.Show();"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="col-md-2 col-sm-2 col-xs-12 noPadding">
            
            <div class="reqType-headerBtnMigrtkWrap">
                <asp:Button ID="btnMigrateRequestType" runat="server" CssClass="AspPrimary-blueBtn" Text="Migrate" OnClientClick="MoveToProduction(this)" Visible="false" />
            </div>
        </div>
        <div class="col-md-7 col-sm-7 col-xs-12 noPadding">
            <div class="reqType-headerBtnWrap">
                <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn margin-right10" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApplyChanges_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btDuplicateItem" runat="server" CssClass="primary-blueBtn" Text="Duplicate" ToolTip="Duplicate" OnClick="btDuplicateItem_Click">
                    <ClientSideEvents Click="duplicateRequestType" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="AddItem_Top" runat="server" Visible="false" CssClass="primary-blueBtn" Text="Add New Item" ToolTip="Add New Item" AutoPostBack="false">
                </dx:ASPxButton>
                <dx:ASPxButton ID="aEditItem_Top" runat="server" CssClass="primary-blueBtn" Text="Edit Item" ToolTip="Edit Item" AutoPostBack="false">
                    <ClientSideEvents Click="function(s,e){editMultipleRequestType();}" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="lnkDeleteItem_Top" runat="server" CssClass="primary-blueBtn" Text="Delete" AutoPostBack="false" ToolTip="Delete">
                    <ClientSideEvents Click="function(s,e){deleteMultipleRequestType();}" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btDeleteItem_Top" CssClass="btDeleteItem_Top hide primary-blueBtn" runat="server" OnClick="btDeleteItem_Top_Click">
                </dx:ASPxButton>

                <dx:ASPxButton ID="lnkUnDeleteItem_Top" runat="server" Text="Un-Delete" CssClass="primary-blueBtn" Visible="false" ToolTip="Un-Delete Deleted Items" AutoPostBack="false">
                    <ClientSideEvents Click="function(s,e){undeleteMultipleRequestType();}" />
                </dx:ASPxButton>

                <dx:ASPxButton ID="btUnDeleteItem_Top" CssClass="btUnDeleteItem_Top hide primary-blueBtn" runat="server" OnClick="btUnDeleteItem_Top_Click">
                </dx:ASPxButton>

                <dx:ASPxButton ID="btnImport" runat="server" Text="Import" CssClass="primary-blueBtn" AutoPostBack="false" Visible="false" ToolTip="Import">
                    <ClientSideEvents Click="function(s,e){OpenImportExcel();}" />
                </dx:ASPxButton>

               <%-- <dx:ASPxButton ID="btnExport1" runat="server" Text="Export" CssClass="primary-blueBtn" ToolTip="Export">
                    <ClientSideEvents Click="function(s,e){downloadExcel(this);}" />

                </dx:ASPxButton>
                <dx:ASPxButton ID="btnExportxml" runat="server" Text="XML" CssClass="primary-blueBtn" ToolTip="Export">
                    <ClientSideEvents Click="function(s,e){downloadxml(this);}" />

                </dx:ASPxButton>--%>
                <asp:Button ID="btnExportXML" runat="server" CssClass="hide" OnClick="btnExportXML_Click" />
                <asp:Button ID="btnExport" runat="server" CssClass="hide" OnClick="btnExport_Click" />
                <div style="float: right; padding: 4px;margin-top:-4px !important" class="cioReportbuttonContainer111 actionBtn_container">
                    <div class="btn-container">
                        <div class="dropdown btn-wrap actionSvcBtn_wrap">
                            <dx:ASPxButton ID="lnkbtnActionMenu" CssClass="btn dropdown-toggle action-btn" data-toggle="dropdown" runat="server" Text="Export" ImagePosition="Right" AutoPostBack="false">
                                <Image Url="/Content/Images/arrow-down.png"></Image>
                            </dx:ASPxButton>

                            <dx:ASPxPopupMenu ID="ASPxPopupActionMenu" runat="server" PopupElementID="lnkbtnActionMenu" CloseAction="MouseOut" ItemSpacing="0" SubMenuStyle-ItemSpacing="0"
                                ClientInstanceName="ASPxPopupActionMenu" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" PopupAction="LeftMouseClick">
                                <Items>
                                    <dx:MenuItem GroupName="ImportExport" Text="Excel" Name="Excel"></dx:MenuItem>
                                    <dx:MenuItem GroupName="ImportExport" Text="XML" Name="XML"></dx:MenuItem>
                                </Items>
                                 <ClientSideEvents ItemClick="function(s, e) { ImportExportItemClick(s, e);}" />

                                <ItemStyle CssClass="dxb editTicket-actionMenu" BackColor="#ebedf2" HoverStyle-BackColor="Blue"></ItemStyle>

                            </dx:ASPxPopupMenu>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="row collapsExpand-btnWrap">
        <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
        <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        <div class="crm-checkWrap reqType-headerchkWrap" style="float:right;">
            <asp:CheckBox ID="chkShowDeleted" runat="server" TextAlign="Right" AutoPostBack="true" Text="Show Deleted" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
        </div>
    </div>
    <div class="row">
        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." ClientInstanceName="loadingPanel" Image-Url="~/Content/IMAGES/AjaxLoader.gif" Modal="True" ImagePosition="Top"></dx:ASPxLoadingPanel>

        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="apsxTreeList-wrap">
                <dx:ASPxTreeList ID="aspxtreelist" runat="server" Width="100%" ClientInstanceName="aspxtreelistRequestType" CssClass="apsxTreeList"
                    OnHtmlRowPrepared="aspxtreelist_HtmlRowPrepared" OnHtmlDataCellPrepared="aspxtreelist_HtmlDataCellPrepared"
                    OnDataBound="aspxtreelist_DataBound" AutoGenerateColumns="false" AutoGenerateServiceColumns="true" KeyFieldName="ItemID"
                    SettingsBehavior-AllowFocusedNode="true"
                    ParentFieldName="ParentID">
                    <Columns>
                        <dx:TreeListDataColumn VisibleIndex="0" Caption="Request Type">
                            <DataCellTemplate>
                                <a id="aRequestType1" runat="server"  href=""><%#Eval("RequestType") %></a>
                            </DataCellTemplate>
                        </dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="OwnerUser" VisibleIndex="2" Caption="Owner"></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="PRPGroupUser" VisibleIndex="3" Caption="PRP Group"></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="PRPUser" VisibleIndex="4" Caption="PRP"></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="EscalationManagerUser" VisibleIndex="5" Caption="Escalation Mgr."></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="EscalationManagerUser" VisibleIndex="6" Caption="Backup Esc. Mgr."></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="RequestCategory" VisibleIndex="7" Caption="RMM Category"></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="FunctionalAreaLookup" VisibleIndex="8" Caption="Functional Area"></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="WorkflowType" VisibleIndex="9" Caption="Workflow Type"></dx:TreeListDataColumn>
                        <dx:TreeListDataColumn FieldName="Use24x7Calendar" VisibleIndex="10" Caption="24x7"></dx:TreeListDataColumn>
                    </Columns>
                    <Styles>
                       <AlternatingNode Enabled="false"></AlternatingNode>
                        <%--<FilterRow CssClass="aspxTreeList-filterRow"></FilterRow>--%>
                        <%--<Node CssClass="aspxTreeList-Node "></Node>--%>
                        <Header CssClass="aspxTreeList-Heder" Font-Bold="true"></Header>
                        <SelectedNode CssClass="aspxTreeList-Node spxTreeList-selectedNode"></SelectedNode>
                       <%-- <FocusedNode CssClass="aspxTreeList-Node"></FocusedNode>--%>

                    </Styles>
                    <SettingsSelection Enabled="True" />
                    <SettingsCookies   StoreExpandedNodes="true" StorePaging="true" StoreSelection="true" StoreSorting="true" />
                </dx:ASPxTreeList>
            </div>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <dx:ASPxButton ID="AddItem" runat="server" Visible="false" Text="Add New Item" ToolTip="Add New Item" AutoPostBack="false" CssClass="primary-blueBtn">
            </dx:ASPxButton>
            <dx:ASPxButton ID="aEditItem" runat="server" Text="Edit Item" ToolTip="Edit Item" AutoPostBack="false" CssClass="primary-blueBtn">
                <ClientSideEvents Click="function(s,e){editMultipleRequestType();}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btDeleteItem_Bottom" runat="server" Text="Delete Item" ToolTip="Delete" AutoPostBack="false" CssClass="primary-blueBtn">
                <ClientSideEvents Click="function(s,e){deleteMultipleRequestType();}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="lnkUnDeleteItem_Bottom" runat="server" Text="Un-Delete" Visible="false" ToolTip="Un-Delete Deleted Items" CssClass="primary-blueBtn"
                AutoPostBack="false">
                <ClientSideEvents Click="function(s,e){undeleteMultipleRequestType();}" />
            </dx:ASPxButton>
        </div>
    </div>
    <div class="row">
        <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>
        <asp:HiddenField ID="bunchids" runat="server" />
    </div>
</div>



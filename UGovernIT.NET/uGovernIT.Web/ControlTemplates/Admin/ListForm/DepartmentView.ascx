<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentView.ascx.cs" Inherits="uGovernIT.Web.DepartmentView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxTreeList" tagprefix="dx" %>  
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">

    .archived-row {
        /*background: #fcdcdb !important;*/
    }

        .archived-row td {
            color: #ed0808 !important;
        }

            .archived-row td a:link {
                color: #ed0808 !important;
            }
    .dxtlNode_UGITNavyBlueDevEx td.dxtl{
        padding: 3px 6px 4px;
        border: 1px solid #DFDFDF;
        white-space: nowrap;
        overflow: hidden;
    }
     
    /*[+][26-10-2023][SANKET][Added code for header color change]*/
    .dxtlHeader_UGITNavyBlueDevEx td.dxtl {
        white-space: nowrap;
        text-align: left;
        overflow: hidden;
        font-weight: bold;
    }

    .crm-checkWrap2 label{
        margin-top: 0px;
    }

</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


    var companyLevel = "<%=companyLabel %>";
    var divisionLevel = "<%= divisionLevel%>";
    var departmentLevel = "<%= departmentLevel%>";
    function addNewCompany(s, e) {
        var url = hdnConfiguration.Get("NewCompanyURL");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Add New ' + companyLevel, '600px', '370px', 0, escape(requestUrl));
        e.processOnServer = false;
    }
    function addNewDivision(s, e) {
        var url = hdnConfiguration.Get("NewDivisionURL");
        //url += '&companyid=' + gvCompany.GetRowKey(gvCompany.focusedRowIndex);
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Add New ' + divisionLevel, '600px', '350px', 0, escape(requestUrl));
        e.processOnServer = false;
    }
    function addNewDepartment(s, e) {
        var url = hdnConfiguration.Get("NewDepartmentURL");
        //url += '&companyid=' + gvCompany.GetRowKey(gvCompany.focusedRowIndex);
        //var enableDivision = hdnConfiguration.Get("enableDivision");
        //if (enableDivision) {
        //    url += '&divisionid=' + gvDivision.GetRowKey(gvDivision.focusedRowIndex);
        //}
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Add New ' + departmentLevel, '600px', '400px', 0, escape(requestUrl));
        e.processOnServer = false;
    }
    function editCompnay(id) {
        var url = hdnConfiguration.Get("EditCompanyURL");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, 'ItemID=' + id, 'Edit ' + companyLevel, '600px', '400px', 0, escape(requestUrl))
    }
    function editDivision(id) {
        var url = hdnConfiguration.Get("EditDivisionURL");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, 'ItemID=' + id, 'Edit ' + divisionLevel, '600px', '400px', 0, escape(requestUrl))
    }
    function editDepartment(id) {
        var url = hdnConfiguration.Get("EditDepartmentURL");
        var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, 'ItemID=' + id, 'Edit ' + departmentLevel, '600px', '450px', 0, escape(requestUrl))
    }


    //function onCompanyChanged(gvCompany) {
     
        
    //    var enableDivision = hdnConfiguration.Get("enableDivision");
    //    if (enableDivision) {
    //        gvDivision.PerformCallback(gvCompany.GetFocusedRowIndex());
    //    }
    //    else {
    //        gvDepartment.PerformCallback(gvCompany.GetFocusedRowIndex());
    //    }

    //}

    //function onDivisionCallEnd(gvDivision) {
      
    //    if (gvDivision.GetFocusedRowIndex() > -1) {
    //        gvDepartment.PerformCallback(gvDivision.GetFocusedRowIndex());
    //    }
    //}

    function downloadExcel(obj) {
        var exportUrl = window.location.href;
        exportUrl += "&initiateExport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
        return false;
    }

    function OpenImportExcel() {
        window.parent.UgitOpenPopupDialog('<%= importUrl %>', "", 'Import ' + departmentLevel, '400px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function MoveToProduction(obj) {
        var url = '<%=moveToProductionUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', 'Migrate Change(s)', '350px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function confirmDelete(Id) {
        if (TreeListDepartments.focusedKey.startsWith('DP')) {
            $('#dialog-confirm').dialog(
                {
                    modal: true,
                    buttons: {
                        'Ok': function () {
                            __doPostBack('DeleteItem', Id);
                            $(this).dialog("close");
                        },
                        'Cancel': function () {
                            $(this).dialog("close");
                        }
                    }
                });
        }
        else
        {
            $('#dialog-confirm').dialog(
            {
                modal: true, 
                buttons: {
                    'Parent & Child': function () {
                        TreeListDepartments.PerformCallback();
                        $(this).dialog("close");
                    },
                    'Parent Only': function () {
                        __doPostBack('DeleteItem', Id);
                        $(this).dialog("close");
                    },
                    'Cancel': function () {
                        $(this).dialog("close");
                    }
                }
            });
        }
    }
    function refresh(s, e) {
        if (e.command == 'CustomCallback')
            CloseWindowCallback(1, document.location.href);
    }
</script>

<dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row adminPopup-hederSec">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
        <div class="col-lg-8 col-md-8 col-sm-8 col-xs-8">
            <dx:ASPxButton ID="btnAddCompany" runat="server" Text="Add Company +" CssClass="primary-blueBtn" ToolTip="Add New Company">
                <ClientSideEvents  Click="addNewCompany"/>
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnAddDivision" runat="server" CssClass="primary-blueBtn" Text="Add Division +" ToolTip="Add New Division">
                <ClientSideEvents  Click="addNewDivision"/> 
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnAddDepartment" runat="server" Text="Add Department +" CssClass="primary-blueBtn" ToolTip="Add Department">
                <ClientSideEvents  Click="addNewDepartment"/> 
            </dx:ASPxButton>
        </div>

            <div class="rightBtnSection">
                <asp:Button ID="btnMigrateCompany" CssClass="AspPrimary-blueBtn" runat="server" Text="Migrate" OnClientClick="MoveToProduction(this)" Visible="false" />
                <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes" Visible="false" OnClick="btnApplyChanges_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnImport" runat="server" Text="Import" ToolTip="Import" Visible="false" CssClass="primary-blueBtn importBtn">
                    <ClientSideEvents  Click="function(s, e){return OpenImportExcel();}"/>
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnExport" runat="server" Text="Export" ToolTip="Export" CssClass="primary-blueBtn exportBtn" AutoPostBack="false">
                    <%--<ClientSideEvents Click="function(s, e){return downloadExcel(this);}" />--%>
                    <ClientSideEvents Click="function(s, e){TreeListDepartments.ExportTo(ASPxClientTreeListExportFormat.Xls);}" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
    <div class="col-lg-12" style="margin-bottom:10px;">
       <span class="crm-checkWrap2">
            <asp:ImageButton ToolTip="Expand All" ImageUrl="~/Content/Images/expand-all-new.png" ID="btExpandAllAllocation" runat="server"
                OnClick="btExpandAllAllocation_Click" CssClass="resouce-expand" />
            <asp:ImageButton ToolTip="Collapse All" ImageUrl="~/content/images/collapse-all-new.png" ID="btCollapseAllAllocation" runat="server" 
                OnClick="btCollapseAllAllocation_Click" CssClass="resouce-collapse" />
       </span>
        <div style="float:right">
            <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted&nbsp;" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
        </div>
    </div>
    <div class="col-lg-12">
        <dx:ASPxTreeList ID="TreeListDepartments" runat="server" AutoGenerateColumns="False" Width="95%" CssClass="apsxTreeList" OnNodeDeleting="TreeListDepartments_NodeDeleting"
            KeyFieldName="CUSTOM_ID" ParentFieldName="CUSTOM_PARENT_ID" ClientInstanceName="TreeListDepartments" OnCustomCallback="TreeListDepartments_CustomCallback"
            Border-BorderStyle="Solid" OnHtmlRowPrepared ="TreeListDepartments_HtmlRowPrepared" ClientSideEvents-EndCallback="refresh" >
            <Settings GridLines="Both" ShowTreeLines="true"/>
            <SettingsExport EnableClientSideExportAPI="true"></SettingsExport>
            <columns>
                <dx:TreeListDataColumn FieldName="Title">
                </dx:TreeListDataColumn>
                <dx:TreeListDataColumn FieldName="GLCode">
                    </dx:TreeListDataColumn>
                <dx:TreeListDataColumn FieldName="Manager" >
                    </dx:TreeListDataColumn>
                <dx:TreeListCheckColumn FieldName="DELETED" Visible="false">
                </dx:TreeListCheckColumn>
                <dx:TreeListDataColumn FieldName="ID" Visible="false">
                    <cellstyle BackColor="Wheat"/>
                </dx:TreeListDataColumn>
                <dx:TreeListDataColumn FieldName="ParentID" Visible="false">
                    <cellstyle BackColor="Wheat"/>
                </dx:TreeListDataColumn>
                <dx:TreeListImageColumn Caption="Action" CellStyle-HorizontalAlign="Right" Width="80px">
                </dx:TreeListImageColumn>
            </columns>
            <settingsbehavior expandcollapseaction="NodeDblClick" AutoExpandAllNodes="true" 
                AllowFocusedNode="true" />
        </dx:ASPxTreeList>

    </div>


    <div class="row"> &nbsp;</div>
</div>

<div id="dialog-confirm" title="Confirm Delete" style="display:none">
  <p><span class="ui-icon ui-icon-alert" style="float:left; margin:12px 12px 20px 0;"></span>Please select the delete action.</p>
</div>
<dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>
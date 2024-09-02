<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckListProjectView.ascx.cs" Inherits="uGovernIT.Web.CheckListProjectView" %>
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


    #content {
        /*width: 100%;*/
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }

    a:hover {
        text-decoration: underline;
    }

    a, img {
        border: 0px;
    }

    .seqNoHeader {
        width: 20px !important;
        text-align: center;
    }

    .seqNoItem {
        width: 20px !important;
        text-align: center;
    }

    .txtBoldBorder {
        font-weight: bold !important;
        color: black;
    }

    .txtNormalBorder {
        font: normal !important;
        color: gray;
    }
</style>

<style type="text/css">
    /*body {
        overflow-y: auto !important;
    }

    #s4-leftpanel {
        display: none;
    }

    .s4-ca {
        margin-left: 0px !important;
    }

    #s4-ribbonrow {
        height: auto !important;
        min-height: 0px !important;
    }

    #s4-ribboncont {
        display: none;
    }

    #s4-titlerow {
        display: none;
    }

    .s4-ba {
        width: 100%;
        min-height: 0px !important;
    }

    #s4-workspace {
        float: left;
        width: 100%;
        overflow: auto !important;
    }

    body #MSO_ContentTable {
        min-height: 0px !important;
        position: inherit;
    }

    .full-width {
        width: 98%;
    }*/

    .ms-formlabel {
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fleft {
        float: left;
    }

    .proposeddatelb {
        padding-top: 5px;
        padding-right: 4px;
        float: left;
    }

    /*.overlay {
        display: none;
        position: absolute;
        left: 0%;
        top: 0%;
        padding: 25px;
        background-color: black;
        width: 93%;
        height: 740px;
        -moz-opacity: 0.3;
        opacity: .30;
        filter: alpha(opacity=30);
        z-index: 100;
    }*/

    /*.divcell_0 {
        float: left;
        display: table;
        width: 97%;
        font-size: 10px;
    }*/

    /*.cell_0_0 {
        width: 150px;
        background-color: white;
        border: none;
        height: 30px;
        vertical-align: top;
        padding: 0px;
        word-wrap: break-word;
        text-align: left;
    }*/

    .header {
        background-color: white;
    }

    .header div {
        display: table;
    }

    .addiconheader {
        border: none;
        background-color: white;
    }

    .tdrowdata {
        text-align: center;
    }

    /*.taskContent {
        width: 80px;
        display: table-cell;
    }*/

    /*.imgAlign {
        width: 10px;
        display: table-cell;
    }*/

    .checkListContent {
        width: 140px;
        display: table-cell;
    }

    .checkListImage {
        width: 10px;
        display: table;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    AlertToSaveChanges();

    function OpenImportCheckList() {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlImportCheckList %>', "", 'Import CheckList Template', '400px', '250px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpenAddCheckList() {

        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&CheckListId=0', "", 'Add CheckList', '500px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpenEditCheckListPopup(objCheckListId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&CheckListId=' + objCheckListId, "", 'Edit CheckList', '500px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function showeditCheckList(obj) {
        $(obj).find("img").css("visibility", "visible");
    }

    function hideeditCheckList(obj) {
        $(obj).find("img").css("visibility", "hidden");
    }

    function showedittask(obj) {
        $(obj).find("img").css("visibility", "visible");
    }

    function hideedittask(obj) {
        $(obj).find("img").css("visibility", "hidden");
    }

    function OpenEditTaskPopup(objCheckListId, objCheckListTaskId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckListTask %>' + '&CheckListTaskId=' + objCheckListTaskId + '&CheckListId=' + objCheckListId, "", 'Edit CheckList Task', '500px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpenAddTaskPopup(objCheckListId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckListTask %>' + '&CheckListTaskId=0&CheckListId=' + objCheckListId, "", 'Add CheckList Task', '500px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function showActionRole(obj) {
        $(obj).find("img").css("visibility", "visible");
    }

    function hideActionRole(obj) {
        $(obj).find("img").css("visibility", "hidden");
    }

    function CheckListRoleEmail(ObjCheckListId, objRoleId) {
        //debugger;
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlEmailNotification %>' + '&CheckListRoleId=' + objRoleId + '&CheckListId=' + ObjCheckListId, "", 'Email Notification', '600px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


    function OpenAddRolePopup(objCheckListId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckListRole %>' + '&CheckListRoleId=0&CheckListId=' + objCheckListId, "", 'Add CheckList Role', '490px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpenEditRolePopup(objCheckListId, objCheckListRoleId) {
        window.parent.UgitOpenPopupDialog('<%= absoluteUrlCheckListRole %>' + '&CheckListRoleId=' + objCheckListRoleId + '&CheckListId=' + objCheckListId, "", 'Edit CheckList Role', '600px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function onCheckboxClick(obj) {
        if ($(obj).parent().parent().find('input[type=text]').hasClass('txtBoldBorder')) {
            $(obj).parent().parent().find('input[type=text]').removeClass('txtBoldBorder');
            $(obj).parent().parent().find('input[type=text]').addClass('txtNormalBorder');
        }

        if ($(obj).parent().find('input[type=checkbox]').prop('checked')) {
            $($(obj).parent().parent().find('input[type=hidden]')[1]).val('C');
        }
        else {
            $($(obj).parent().parent().find('input[type=hidden]')[1]).val('NC');
        }
        $(obj).parent().find('input[type=checkbox]').css('visibility', 'visible');
        $(obj).parent().parent().find('input[type=text]').css('visibility', 'hidden');
    }

    function ontxtboxClick(obj) {
        if ($(obj).parent().find('input[type=text]').hasClass('txtNormalBorder')) {
            $(obj).parent().find('input[type=text]').removeClass('txtNormalBorder');
            $(obj).parent().find('input[type=text]').addClass('txtBoldBorder');
            $($(obj).parent().find('input[type=hidden]')[1]).val('NA');
            $(obj).parent().find('input[type=checkbox]').attr('checked', false);
            $(obj).parent().find('input[type=checkbox]').css('visibility', 'hidden');
            $(obj).parent().find('input[type=text]').css('visibility', 'visible');
        }
        else {
            $(obj).parent().find('input[type=text]').removeClass('txtBoldBorder');
            $(obj).parent().find('input[type=text]').addClass('txtNormalBorder');
            $($(obj).parent().find('input[type=hidden]')[1]).val('NC');
            $(obj).parent().find('input[type=checkbox]').attr('checked', false);
            $(obj).parent().find('input[type=checkbox]').css('visibility', 'visible');
            $(obj).parent().find('input[type=text]').css('visibility', 'hidden');
        }
    }

    function OpenSyncProcoreCheckList() {
        SyncCheckListWithProcorePopup.Show();
        return false;
    }

    function showNA(obj) {
        $(obj).find('input[type=checkbox]').css('visibility', 'visible');
        $(obj).find('input[type=text]').css('visibility', 'visible');
    }

    function hideNA(obj) {
        if ($($(obj).find('input[type=hidden]')[1]).val() == "NA") {
            $(obj).find('input[type=checkbox]').css('visibility', 'hidden');
            $(obj).find('input[type=text]').css('visibility', 'visible');
        }
        else if ($($(obj).find('input[type=hidden]')[1]).val() == "C") {
            $(obj).find('input[type=checkbox]').css('visibility', 'visible');
            $(obj).find('input[type=text]').css('visibility', 'hidden');
        }
        else {
            $(obj).find('input[type=checkbox]').css('visibility', 'visible');
            $(obj).find('input[type=text]').css('visibility', 'hidden');
        }
    }


    $(function () {
        var height = $(window).height();
        $('#tblCheckListProject').css('height', height - 50);
    });
</script>
<div id="content">
   <div>        <%-- <tr>
            <td colspan="2">
                 <img src="/Content/images/ugovernit/importTemplate_20x20.png" title="Load Template" id="btLoadTempate" runat="server" style="float:right;" onclick="ImportTemplatePopup(this)" />
            </td>
        </tr>--%>
      
                <asp:Repeater ID="RptCheckList" runat="server" OnItemDataBound="RptCheckList_ItemDataBound">
                    <ItemTemplate>

                        <div class="row">
                            <div class="col-md-12 col-xs-12 col-sm-12">
                                <div class="table-responsive">
                                    <asp:HiddenField ID="hdnCheckListId" runat="server" Value='<%#Eval("ID")%>' />
                                    <asp:Label ID="lblCheckListName" Style="display: none;" runat="server" Text='<%#Eval("Title")%>'></asp:Label>

                                    <asp:GridView ID="gridCheckList" ClientInstanceName="gridCheckList" runat="server" EnableModelValidation="True" ForeColor="#333333" 
                                        GridLines="Both"  CssClass="table table-bordered table-condensed"
                                        OnRowDataBound="gridCheckList_RowDataBound" CellPadding="4" BorderColor="#3E4F50">
                                        <HeaderStyle BackColor="#F2F2F2" Font-Bold="True" ForeColor="#3E4F50" />
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#3E4F50" Height="20px" />
                                    </asp:GridView>
                                    <br />
                                </div>
                            </div>

                            <%--      <tr>
                                <td colspan="2">
                                    <asp:LinkButton ID="btnSave" CssClass="SaveCheckListClass" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" ValidationGroup="save" OnClick="btnSave_Click">
                                    <span class="button-bg">
                                        <b style="float: left; font-weight: normal;">
                                            Save</b>
                                        <i style="float: left; position: relative; top: -3px;left:2px">
                                            <img src="/Content/images/uGovernIT/ButtonImages/save.png"  style="border:none;" title="" alt=""/>
                                        </i> 
                                    </span>
                                    </asp:LinkButton>
                                </td>

                            </tr>--%>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
       </div>

    <div width="100%" style="padding-bottom: 5px;">
        <div class="row">
            <div>
                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
                <%-- <asp:LinkButton ID="lnktempSave" style="display:none;" runat="server" OnClick="lnktempSave_Click"></asp:LinkButton>--%>
                <asp:Button ID="btntempsave" runat="server" OnClick="btntempsave_Click" Style="display: none;" />
            </div>
        </div>
        <div class="row">
            <div class="checkList_btnWrap">
                <div class="addChk_listBtnWrap">
                    <asp:LinkButton ID="lnkbtnAddSubContractor" runat="server" Text="&nbsp;&nbsp;Add New CheckList&nbsp;&nbsp;" ToolTip="Add New CheckList" OnClientClick="return OpenAddCheckList()">
                            <span class="addchkList_btn">
                                <b style="font-weight: 500;">
                                    Add CheckList</b>
                               <%-- <i style="float: left; position: relative; top: -3px;left:2px">
                                    <img src="/Content/images/plus-blue.png"  style="border:none; width:16px;" title="" alt=""/>
                                </i> --%>
                            </span>
                    </asp:LinkButton>
                </div>
                <div class="importChk_listBtnWrap">
                    <asp:LinkButton ID="lnkbtnImportCheckListTemplate" runat="server" Text="&nbsp;&nbsp;Import CheckList Template&nbsp;&nbsp;" ToolTip="Import CheckList Template" OnClientClick="return OpenImportCheckList()">
                        <span class="addchkList_btn">
                            <b style="font-weight: 500;">
                                Import CheckList Template</b>
                           <%-- <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/Content/images/plus-blue.png"  style="border:none; width:16px;" title="" alt=""/>
                            </i> --%>
                        </span>
                    </asp:LinkButton>
                </div>
                
                

                

                <asp:LinkButton ID="lnkbtnSyncWithProcore" runat="server" Text="&nbsp;&nbsp;Get Subcontractor from Procore&nbsp;&nbsp;" ToolTip="Get Subcontractor from Procore" OnClientClick="return OpenSyncProcoreCheckList()" Visible="false">
                                    <span class="button-bg">
                                        <b style="float: left; font-weight: normal;">
                                            Get Subcontractor from Procore</b>
                                        <i style="float: left; position: relative; top: -3px;left:2px">
                                            <img src="/Content/images/uGovernIT/add_icon.png"  style="border:none;" title="" alt=""/>
                                        </i> 
                                    </span>
                </asp:LinkButton>

            </div>
        </div>
    </div>

    <dx:ASPxPopupControl ClientInstanceName="SyncCheckListWithProcorePopup" Modal="true"
        ID="SyncCheckListWithProcorePopup" ShowFooter="false" ShowHeader="true" HeaderText="Sync With Procore"
        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                <div style="float: left; width: 400px; height: 100px; padding-left: 10px;" id="baselineBox" class="first_tier_nav">


                    <table width="100%" align="left" style="padding-bottom: 5px;">
                        <tr>
                            <td style="width: 105px;">Choose CheckList:
                            </td>
                            <td>
                                <asp:DropDownList Width="200" Height="24" ID="ddlProjectCheckList" AutoPostBack="false" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblInformationMessage" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>

                    </table>


                    <div style="float: left; width: 100%;">
                        <ul style="float: right">

                            <asp:LinkButton ID="lnkbtnSubContractorProcore" runat="server" Text="&nbsp;&nbsp;Get Subcontractor from Procore&nbsp;&nbsp;" OnClick="lnkbtnSubContractorProcore_Click" ToolTip="Get Subcontractor from Procore" OnClientClick="return confirm('Warning! This action will delete all existing sub-contractors in the checklist and import the list of subcontractors from Procore!');">
                                    <span class="button-bg">
                                        <b style="float: left; font-weight: normal;">
                                            Get Subcontractor from Procore</b>
                                        <i style="float: left; position: relative; top: -3px;left:2px">
                                            <img src="/Content/images/uGovernIT/add_icon.png"  style="border:none;" title="" alt=""/>
                                        </i> 
                                    </span>
                            </asp:LinkButton>

                            <li runat="server" id="Li22" class="" style="height: 25px;" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                <a id="A2" style="color: white" onclick="SyncCheckListWithProcorePopup.Hide();"
                                    class="cancelwhite" href="javascript:void(0);">Cancel</a>
                            </li>
                        </ul>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckListTemplatesView.ascx.cs" Inherits="uGovernIT.Web.CheckListTemplatesView" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
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

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">    
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

    .divcell_0 {
        float: left;
        display: table;
        width: 97%;
        font-size: 10px;
        color: #4b4b4b;
        font-family: 'Roboto', sans-serif !important;
    }

    .cell_0_0 {
        width: 150px;
        background-color: white;
        border: none;
        height: 30px;
        vertical-align: top;
        padding: 0px;
        word-wrap: break-word;
        /*text-align: left;*/
        text-align: center;
    }

    .header {
        padding: 0px;
        font-weight: normal;
        text-align: left;
        background-color: white;
        border: none;
        width: 100px;
        /*text-align: left;*/
        text-align: center;
        height: 30px;
        word-wrap: break-word;
        vertical-align: top;
        font-size:10px;
        border: 1px solid black;
    }

    .addiconheader {
        border: none;
        background-color: white;
    }

    .taskContent {
        width: 80px;
        display: table-cell;
    }   

    .gridHeaderBorder{
        border:1px solid black;
    }

    .addiconheader img{
        width:15px;
    }

    .tdrowhead img{
        width:15px;
    }

    .header a {
        color: #4b4b4b;
        font-family: 'Roboto', sans-serif !important;
    }

    .tdrowhead a {
        color: #4b4b4b;
        font-family: 'Roboto', sans-serif !important;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OpenAddCheckList() {        
        UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&CheckListId=0', "", 'Add CheckList', '500px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;
    }

    function OpenEditCheckListPopup(objCheckListId) {
        UgitOpenPopupDialog('<%= absoluteUrlCheckList %>' + '&CheckListId=' + objCheckListId, "", 'Edit CheckList', '500px', '400px', 0, escape("<%= Request.Url.AbsolutePath %>"));
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
        UgitOpenPopupDialog('<%= absoluteUrlCheckListTask %>' + '&CheckListTaskId=' + objCheckListTaskId + '&CheckListId=' + objCheckListId, "", 'Edit CheckList Task', '500px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpenAddTaskPopup(objCheckListId) {
        UgitOpenPopupDialog('<%= absoluteUrlCheckListTask %>' + '&CheckListTaskId=0&CheckListId=' + objCheckListId, "", 'Add CheckList Task', '500px', '200px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function showActionRole(obj) {
        $(obj).find("img").css("visibility", "visible");
    }

    function hideActionRole(obj) {
        $(obj).find("img").css("visibility", "hidden");
    }


    function OpenAddRolePopup(objCheckListId) {
        UgitOpenPopupDialog('<%= absoluteUrlCheckListRole %>' + '&CheckListRoleId=0&CheckListId=' + objCheckListId, "", 'Add CheckList Role', '500px', '320px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpenEditRolePopup(objCheckListId, objCheckListRoleId) {
        UgitOpenPopupDialog('<%= absoluteUrlCheckListRole %>' + '&CheckListRoleId=' + objCheckListRoleId + '&CheckListId=' + objCheckListId, "", 'Edit CheckList Role', '500px', '320px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>

<div id="content" style="padding:10px;">
    <table align="left" style="padding-bottom: 5px;">
        <tr>
            <td colspan="2">
                <asp:Repeater ID="RptCheckList" runat="server" OnItemDataBound="RptCheckList_ItemDataBound">
                    <ItemTemplate>

                        <table cellspacing="0" border="0" style="border-collapse: collapse;">
                            <tr>
                                <%--<td class="servicecategory" valign="top">
                        <b>
                            <asp:HiddenField ID="hdnCheckListId" runat="server" Value='<%#Eval("ID")%>' />
                            <asp:Label ID="lblCheckListName" runat="server" Text='<%#Eval("Title")%>'></asp:Label></b><br />
                    </td>--%>

                                <td colspan="2">
                                    <asp:HiddenField ID="hdnCheckListId" runat="server" Value='<%#Eval("ID")%>' />
                                    <asp:Label ID="lblCheckListName" Style="display: none;" runat="server" Text='<%#Eval("Title")%>'></asp:Label>

                                    <asp:GridView ID="gridCheckList" runat="server" EnableModelValidation="True" ForeColor="#333333" GridLines="Both"
                                        OnRowDataBound="gridCheckList_RowDataBound" CellPadding="4" BorderColor="#3E4F50">
                                        <HeaderStyle BackColor="#F2F2F2" Font-Bold="True" ForeColor="#3E4F50" CssClass="gridHeaderBorder"/>
                                        <RowStyle BackColor="#FFFFFF" ForeColor="#3E4F50" Height="20px" />
                                    </asp:GridView>
                                    <br />

                                </td>
                            </tr>

                        </table>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>

    </table>


    <table width="100%" align="left" style="padding-bottom: 5px;">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton ID="lnkbtnAddSubContractor" runat="server" Text="&nbsp;&nbsp;Add Subcontractor&nbsp;&nbsp;" ToolTip="Add Subcontractor" OnClientClick="return OpenAddCheckList()">
                    <span class="primary-btn-link" style="float:left;margin-bottom:5px;">
                        <i style="float: left; position: relative; top: -3px;left:2px">
                            <%--<img src="../../Content/Images/plus-symbol.png"  style="border:none;width:15px;" title="" alt=""/>--%>
                            <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
                        </i> 
                    <asp:Label ID="Label1" runat="server" Text="Add CheckList" CssClass="phrasesAdd-label"></asp:Label>
                        
                        <%--<b style="float: left; font-weight: normal;">
                            Add CheckList</b>--%>
                    </span>
                </asp:LinkButton>
            </td>
        </tr>
    </table>
</div>

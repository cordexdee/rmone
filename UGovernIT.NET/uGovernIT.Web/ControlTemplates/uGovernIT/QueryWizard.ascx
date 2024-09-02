<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryWizard.ascx.cs" Inherits="uGovernIT.Web.QueryWizard" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #e8eded;
        border-top: 1px solid #a5a5a5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        border-top: 1px solid #a5a5a5;
        color: #000000;
        padding-bottom: 6px;
        padding-right: 8px;
        padding-top: 3px;
        width: 150px;
        text-align: right;
    }

    .panel-parameter {
        width: 400px;
        border: solid 1px #000000;
    }

    .table-footer {
        background-color: white;
    }

    .table-header {
        background-color: #e8eded;
    }

    .ui-widget-header {
        background-color: #d0d1d7;
    }

    .group-span-col {
        width: 250px;
        margin: 2px 5px 2px 0;
        float: left;
    }

    .dxichCellSys {
        padding: 0;
    }

    th.ms-vh2 {
        padding-left: 0;
    }

    .error {
        color: red;
        float: left;
        margin-top: 3px;
        padding-left: 5px;
    }
    /*parameter div style*/
    .close-button {
        float: right;
    }

    .span-submit {
        float: right;
    }

    .param-col1 {
        width: 100px;
        border: 1px solid #c8ccc6;
    }

    .param-col2 {
        width: 200px;
        border: 1px solid #c8ccc6;
    }

    .param-col2 > input[type="text"] {
        width: 200px;
    }

    .param-value {
        width: 200px;
    }

    .span-header {
        font-weight: bold;
        float: left;
    }

    .parameter-div > table {
        padding: 5px;
    }

    .parameter-div {
        background-color: white;
        border: 1px solid #000000;
        float: left;
        margin: 5px;
        padding: 5px 2px;
        position: absolute;
        width: 370px;
        z-index: 1;
    }
    /*parameter div style*/
    .headerdiv,
    /*.categorymenu-container {
        float: left;
        width: 100%;
        margin: 3px;
    }*/

    .headerdiv {
        font-weight: bold;
    }

    .ms-vh2-gridview {
        background-color: #ced8d9;
        background-image: none;
        background-repeat: no-repeat;
        font-weight: bold;
        height: 22px;
    }

    .gridheader {
        height: 20px;
        background-color: #ced8d9;
        text-align: left;
        font-weight: normal;
    }

    .columninfo-grid, .columninfo-drilldown, .query-formatter-div {
        background: none repeat scroll 0 0 #ffffff;
        border: 1px solid #ccc;
        max-height: 274px;
        margin: 0;
        overflow: auto;
        width: 100%;
    }

    .panel-joininfo {
        float: left;
        background-color: #d0d1d7;
        border: 1px solid #9698a4;
    }

    /*.main-table {
        height: 640px;
    }*/

    /*.main-table-td1 {
        border-right: 1px solid #808080;
    }*/

    /*.main-table-td2 {
        width: 30%;
    }*/

    .cancel-cat {
        cursor: pointer;
        float: left;
        height: 10px;
        width: 10px;
        display: none;
        margin-left: 7px;
        margin-top: 3px;
    }

    .add-cat {
        cursor: pointer;
        float: left;
        margin-left: 7px;
        margin-top: 3px;
    }

    .formulafields {
        width: 100px;
    }

    .module-div {
        float: left;
        background: white;
        color: Black;
    }

    .span-label {
	    width: 110px;
	    float: left;
	    font-weight: bold;
    }

    .span-label-header {
        float: left;
        font-weight: bold;
    }

    /*input[type='text'], textarea {
            width: 365px;
            float: left;
        }*/

    textarea {
        height: 80px;
    }

    #tabPanel_1,
    #tabPanel_1 table {
        width: 720px;
    }

    .query-container div {
        float: left;
        width: 100%;
        padding-bottom: 2px;
        padding-left: 5px;
    }

    .query-container b {
        color: Blue;
        /*height: 580px;*/
    }

    .query-container .data {
        padding-left: 8px;
        color: #444;
        word-break: break-all;
    }

    .tabspan {
        float: left;
        padding: 6px;
        margin-right: 2px;
    }

    ul {
        float: left;
    }

    ul li {
        display: inline-block;
        float: left;
    }

    .div-left-button {
        float: left;
    }

    .div-button {
        float: right;
    }

    .panel-main {
        width: 100%;
    }

    th[class*='main-table-'], td[class*='main-table-'] {
	    border: 1px solid #808080;
    }

    .container {
        width: 800px;
        margin-left: 0;
        margin-right: 0;
    }

    .pnljointable,
    .pnljointable img,
    .dropdown-table {
        float: left;
    }

    .button {
        float: left;
        height: 20px;
        font: 8pt Verdana;
    }

    .joinbutton {
        float: right;
        height: 20px;
        font: 8pt Verdana;
    }

    .ddlCategories-dropDown {
        float: left;
        width: 365px;
    }

    .tdradiobutton label {
        float: right;
        margin-bottom: 8px;
        margin-top: 3px;
    }

    .span-inline {
        display: inline;
        margin-right: 20px;
    }

    input[type="number"] {
        width: 40px;
    }

    .imageurlinput {
        width: 300px;
    }

    span.col1,
    span.col2 {
        width: 130px;
        float: left;
    }

    span.col1 > b,
    span.col2 > b {
        margin-top: 2px;
        margin-right: 3px;
        float: left;
    }

    .query-formatter-div select {
        padding: 2px;
    }

    .query-formatter-div span > input[type="text"] {
        padding: 3px 5px !important;
    }

    .ms-formbody > span {
        margin-right: 5px;
    }

    .longtext {
        width: 446px;
    }

    .dxpc-mainDiv,
    .dxpc-shadow {
        float: left;
    }

    .disabledItems {
        font-weight: bold;
        color: #696464;
    }

    .padding-left5 .remove-group {
        position: relative;
        /*width: 14px;*/
        top: 1px;
        cursor: pointer;
    }

    .filtergrouping {
        padding-left: 45px;
    }

    .firstgroupitem {
        padding-left: 28px;
    }

    .userBox .userValueBox-dropDown .dxgvCSD {
        width: 360px !important;
    }
    .userBox .userValueBox-dropDown table {
        max-width: 361px !important;
    }

    .missingColContainer-headerStyle {
        font-weight: bold !important;
        min-width: 265px;
    }

    .column-name label {
        margin-left: 3px !important;
    }

    .dxtc-leftIndent, .dxtc-rightIndent {
        display: none !important;
    }

    .dxtcLite_UGITNavyBlueDevEx > .dxtc-stripContainer .dxtc-spacer {
        border-bottom: none;
    }

    textarea:focus-visible {
        outline: none;
    }

    .altRowsSortBy {
        padding-top: 4px;
        display: block;
    }
    .dxlpLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxcaLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx .dxlp-loadingImage, .dxeImage_UGITNavyBlueDevEx.dxe-loadingImage {
    background-image: url(/Content/Images/ajaxloader.gif);
    height: 32px;
    width: 32px;
}
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ClosePopUp() {
       <%-- var sourceURL = "<%= Request["source"] %>";
        sourceURL += "**refreshDataOnly";
        window.parent.CloseWindowCallback(1, sourceURL);--%>
        var sourceURL = "<%= Request["source"] %>";

        window.frameElement.commitPopup(sourceURL);
    }

    function showCategary(cat) {
        if (cat == 1) {
            $("#<%=ddlCategories.ClientID %>").css("display", 'block');
            $("#<%=queryCategory.ClientID %>").css("display", 'none');
            $("#imgaddc").css("display", 'block');
            $("#imgcancelc").css("display", 'none');
        }
        else {
            $("#<%=ddlSubCategories.ClientID %>").css("display", 'block');
            $("#<%=querySubCategory.ClientID %>").css("display", 'none');
            $("#imgaddn").css("display", 'block');
            $("#imgcanceln").css("display", 'none');
        }
    }

    function hideCategary(cat) {
        if (cat == 1) {
            $("#<%=ddlCategories.ClientID %>").css("display", 'none');
            $("#<%=queryCategory.ClientID %>").css("display", 'block');
            $("#imgaddc").css("display", 'none');
            $("#imgcancelc").css("display", 'block');
         }
        else {
            $("#<%=ddlSubCategories.ClientID %>").css("display", 'none');
            $("#<%=querySubCategory.ClientID %>").css("display", 'block');
            $("#imgaddn").css("display", 'none');
            $("#imgcanceln").css("display", 'block');
        }
    }

    function tabClick(tabName) {

        switch (tabName) {
            case 'GeneralInfo':
                $('#<%=hdnFormState.ClientID%>').val("TablesJoin");
                $('.back-button').click();
                break;
            case 'TablesJoin':
                $('#<%=hdnFormState.ClientID%>').val("GeneralInfo");
                $('.next-button').click();
                break;
            case 'Columns':
                $('#<%=hdnFormState.ClientID%>').val("TablesJoin");
                $('.next-button').click();
                break;
            case 'WhereClause':
                $('#<%=hdnFormState.ClientID%>').val("Columns");
                $('.next-button').click();
                break;
            case 'GroupBy':
                $('#<%=hdnFormState.ClientID%>').val("WhereClause");
                $('.next-button').click();
                break;
            case 'SortBy':
                $('#<%=hdnFormState.ClientID%>').val("GroupBy");
                $('.next-button').click();
                break;
            case 'Totals':
                $('#<%=hdnFormState.ClientID%>').val("SortBy");
                $('.next-button').click();
                break;
            case 'QueryFormat':
                $('#<%=hdnFormState.ClientID%>').val("Totals");
                $('.next-button').click();
                break;
            case 'Drilldown':
                $('#<%=hdnFormState.ClientID%>').val("QueryFormat");
                $('.next-button').click();
                break;
            default:

        }
    }

    function ddljointype_change() {

        if ($(this).val() == 'MERGE') {
            $('.ddlfirstcol').prop('disabled', 'disabled');
            $('.ddlsecondcol').prop('disabled', 'disabled');
            $('.ddloperator').prop('disabled', 'disabled');
        }
        else {
            $('.ddlfirstcol').prop('disabled', false);
            $('.ddlsecondcol').prop('disabled', false);
            $('.ddloperator').prop('disabled', false);
        }

    }
    $(document).ready(function () {

        $('#<%=chkTransparent.ClientID%>').bind("click", function () {

            if ($('#<%=chkTransparent.ClientID%>').is(':checked')) {
                $('#<%=ceBGColor.ClientID%>').hide();

            }
            else {
                $('#<%=ceBGColor.ClientID%>').show();
            }
        });

        var selectedJoinType = $('#<%= DdlJoin.ClientID%> option:selected').val();
        if (selectedJoinType == "UNION") {
            $('#<%=DdlAvlTables.ClientID %>').attr("disabled", true);
            $('#<%=DdlFirstTabCols.ClientID %>').attr("disabled", true);
            $('#<%=DdlSecondTabCols.ClientID %>').attr("disabled", true);
            $('#<%=DdlOperator.ClientID %>').attr("disabled", true);
        }
    });

    function ddlJoin_Change(obj) {
        if (obj.value == "UNION") {
            $('#<%=DdlAvlTables.ClientID %>').attr("disabled", true);
            $('#<%=DdlFirstTabCols.ClientID %>').attr("disabled", true);
            $('#<%=DdlSecondTabCols.ClientID %>').attr("disabled", true);
            $('#<%=DdlOperator.ClientID %>').attr("disabled", true);
        }
        else {
            $('#<%=DdlAvlTables.ClientID %>').attr("disabled", false);
            $('#<%=DdlFirstTabCols.ClientID %>').attr("disabled", false);
            $('#<%=DdlSecondTabCols.ClientID %>').attr("disabled", false);
            $('#<%=DdlOperator.ClientID %>').attr("disabled", false);
        }
    }

</script>
<div class="px-3 py-2">

    <asp:HiddenField ID="hdnFormState" runat="server" />
    <asp:HiddenField ID="hdnPreviewClick" runat="server" />

    <asp:HiddenField ID="hdnTitle" runat="server" />
    <asp:HiddenField ID="hDashboardId" runat="server" />


    <table style="width: 100%; border: none; border-collapse: collapse;" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                <dx:ASPxTabControl ID="tabMenu" ClientInstanceName="tabMenu" runat="server" Width="100%" ActiveTabIndex="3" style="padding-top: 12px;">
                    <Tabs>
                        <dx:Tab Text="General Info" Name="GeneralInfo" />
                        <dx:Tab Text="Table Joins" Name="TablesJoin" />
                        <dx:Tab Text="Columns" Name="Columns" />
                        <dx:Tab Text="Filters" Name="WhereClause" />
                        <dx:Tab Text="Grouping" Name="GroupBy" />
                        <dx:Tab Text="Sorting" Name="SortBy" />
                        <dx:Tab Text="Totals" Name="Totals" />
                        <dx:Tab Text="Query Format" Name="QueryFormat" />
                        <dx:Tab Text="Drill-Down" Name="Drilldown" />
                    </Tabs>
                    <ClientSideEvents TabClick="function(s, e){tabClick(e.tab.name);loadingPanel.Show();}" />
                    <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
                </dx:ASPxTabControl>
            </td>
            <td>
                <div class="first_tier_nav d-flex align-items-center justify-content-end">
                    <dx:ASPxLoadingPanel ID="publishHideLoading" ClientInstanceName="loadingPanel" Modal="True" runat="server" ContainerElementID="loadingPanel" Text="Please Wait..."></dx:ASPxLoadingPanel>
                    <asp:CheckBox Text="Formatted Preview:" runat="server" TextAlign="Left" ID="chkIsFormattedPreview" CssClass="formatted-preview" />
                    <ul class="ml-2 mb-0">
                        <li class="">
                            <dx:ASPxButton ID="lnkPublish" runat="server" Text="Publish" CssClass="primary-blueBtn" OnClick="lnkPublish_Click" ImagePosition="Right">
                                <Image Url="/content/buttonimages/publish.png"></Image>
                                <ClientSideEvents Click="function(s, e) { loadingPanel.Show(); }" />
                            </dx:ASPxButton>
                        </li>
                    </ul>
                </div>
            </td>
        </tr>
    </table>

    <asp:Panel ID="pnlMain" runat="server" CssClass="panel-main">
        <table class="main-table" width="100%">
            <tr style="vertical-align: top">
                <td class="main-table-td1">
                    <asp:Panel ID="tabPanel_1" runat="server" CssClass="container py-3">
                        <table>
                            <tr>
                                <td class="pt-1">
                                    <div>
                                        <span class="span-label">Title<b style="color: Red">*</b>:</span>
                                        <span>
                                            <asp:TextBox MaxLength="200" ValidationGroup="generalInfo" ID="txtTitle" Width="365px" runat="server"></asp:TextBox>
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="pt-1">
                                    <div>
                                        <span class="span-label">Description:</span>
                                        <span>
                                            <asp:TextBox MaxLength="1000" CssClass="widthtxtbox" TextMode="MultiLine" ID="txtDescription"
                                                runat="server" Width="365px"></asp:TextBox>
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="pt-1">
                                    <div>
                                        <span class="span-label">Category:</span>
                                        <span>
                                            <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="false" class="ddlCategories-dropDown" />
                                            <asp:TextBox ID="queryCategory" runat="server" Style="display: none; float:left;" Width="365px" />
                                        </span>
                                        <span>
                                            <img id="imgaddc" src="/content/images/add_icon.png" class="add-cat" alt="Add" onclick="hideCategary(1)" />
                                            <img id="imgcancelc" src="/content/images/cancel-icon.png" class="cancel-cat" alt="Cancel" onclick="showCategary(1)" />
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="pt-1">
                                    <div>
                                        <span class="span-label">Sub-Category:</span>
                                        <span>
                                            <asp:DropDownList ID="ddlSubCategories" runat="server" AutoPostBack="false" class="ddlCategories-dropDown"
                                                AppendDataBoundItems="True" />
                                            <asp:TextBox ID="querySubCategory" runat="server" Style="display: none; float:left;" Width="365px" />
                                        </span>
                                        <span>
                                            <img id="imgaddn" src="/content/images/add_icon.png" class="add-cat" alt="Add" onclick="hideCategary(2)" />
                                            <img id="imgcanceln" src="/content/images//cancel-icon.png" class="cancel-cat" alt="Cancel" onclick="showCategary(2)" />
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="pt-1">
                                    <div class="clearfix">
                                        <span class="span-label">Modules:</span>
                                        <div class="module-div">
                                            <asp:CheckBoxList ID="cbModules" runat="server" RepeatColumns="3" 
                                                Width="450px" CssClass="module-chkList">
                                            </asp:CheckBoxList>
                                        </div>

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="pt-1">
                                    <div class="d-flex align-items-center">
                                        <span class="span-label">Authorize to View:</span>
                                        <span class="d-block">
                                            <ugit:UserValueBox ID="ppeAuthorizeToView" runat="server" Width="363px" cssclass="userValueBox-dropDown" isMulti="true" />
                                        </span>
                                        <span>
                                            <asp:CustomValidator ID="cvAuthorizedToView" runat="server" />
                                        </span>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="pt-1">
                                    <div class="d-flex align-items-center">
                                        <div>
                                            <span class="span-label">Order:</span>
                                        </div>
                                        <div>
                                            <dx:ASPxComboBox ID="cmbOrder" runat="server" DropDownStyle="DropDownList"></dx:ASPxComboBox>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_2" runat="server" CssClass="container py-3">
                        <asp:UpdatePanel ID="upTableJoin" runat="server">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlTable">
                                    <table>
                                        <tr>
                                            <td>
                                                <span class="span-label">Query Table:</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span>
                                                    <asp:DropDownList Width="200" AutoPostBack="True" ID="DdlQueryTables" DataTextField="Text" DataValueField="Value"
                                                        runat="server" OnSelectedIndexChanged="DdlQueryTables_SelectedIndexChanged">
                                                    </asp:DropDownList></span>
                                                <span>
                                                    <asp:ImageButton ID="ibAdd" ImageUrl="/content/images/add_icon.png" runat="server"
                                                        Style="cursor: pointer;" AlternateText="Add" OnClick="ibAdd_Click" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTable2" runat="server" Width="100%" Visible="false" CssClass="pnljointable">
                                                    <asp:DropDownList Width="200" ID="DdlQueryTables2" AutoPostBack="true" DataTextField="Text" DataValueField="Value" 
                                                        OnSelectedIndexChanged="BtJoin_Click" runat="server" CssClass="dropdown-table">
                                                    </asp:DropDownList>

                                                    <asp:ImageButton ID="BtJoin2" ImageUrl="/content/buttonimages/edit-icon.png" runat="server"
                                                        Style="cursor: pointer; padding-bottom: 2px; padding-left: 3px;" AlternateText="edit" OnClick="BtEditJoin_Click" />
                                                    <asp:ImageButton ID="ibRemoveTable2" ImageUrl="/content/ButtonImages/cancel.png" runat="server"
                                                        Style="cursor: pointer;" AlternateText="Remove" OnClick="ibRemoveTable_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTable3" runat="server" Width="100%" Visible="false" CssClass="pnljointable">
                                                    <asp:DropDownList Width="200" ID="DdlQueryTables3" runat="server" DataTextField="Text" DataValueField="Value" 
                                                        AutoPostBack="true" OnSelectedIndexChanged="BtJoin_Click" CssClass="dropdown-table">
                                                    </asp:DropDownList>

                                                    <asp:ImageButton ID="BtJoin3" ImageUrl="/content/buttonimages/edit-icon.png" runat="server"
                                                        Style="cursor: pointer; padding-bottom: 2px; padding-left: 3px;" AlternateText="edit" OnClick="BtEditJoin_Click" />
                                                    <asp:ImageButton ID="ibRemoveTable3" ImageUrl="/content/ButtonImages/cancel.png" runat="server"
                                                        Style="cursor: pointer;" AlternateText="Remove" OnClick="ibRemoveTable_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTable4" runat="server" Width="100%" Visible="false" CssClass="pnljointable">
                                                    <asp:DropDownList Width="200" ID="DdlQueryTables4" runat="server" DataTextField="Text" DataValueField="Value" 
                                                        AutoPostBack="true" OnSelectedIndexChanged="BtJoin_Click" CssClass="dropdown-table">
                                                    </asp:DropDownList>

                                                    <asp:ImageButton ID="BtJoin4" ImageUrl="/content/ButtonImages/edit-icon.png" runat="server"
                                                        Style="cursor: pointer; padding-bottom: 2px; padding-left: 3px;" AlternateText="edit" OnClick="BtEditJoin_Click" />
                                                    <asp:ImageButton ID="ibRemoveTable4" ImageUrl="/content/ButtonImages/cancel.png" runat="server"
                                                        Style="cursor: pointer;" AlternateText="Remove" OnClick="ibRemoveTable_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTable5" runat="server" Width="100%" Visible="false" CssClass="pnljointable">
                                                    <asp:DropDownList Width="200" ID="DdlQueryTables5" runat="server" DataTextField="Text" DataValueField="Value" 
                                                        AutoPostBack="true" OnSelectedIndexChanged="BtJoin_Click" CssClass="dropdown-table">
                                                    </asp:DropDownList>

                                                    <asp:ImageButton ID="BtJoin5" ImageUrl="/content/ButtonImages/edit-icon.png" runat="server"
                                                        Style="cursor: pointer; padding-bottom: 2px; padding-left: 3px;" AlternateText="Remove" OnClick="BtEditJoin_Click" />
                                                    <asp:ImageButton ID="ibRemoveTable5" ImageUrl="/content/ButtonImages/cancel.png" runat="server"
                                                        Style="cursor: pointer;" AlternateText="Remove" OnClick="ibRemoveTable_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlTablesJoin" runat="server" Visible="False" CssClass="panel-joininfo">
                                    <%--<asp:HiddenField ID="hNumberOfTables" runat="server" />--%>
                                    <table class="table-join">
                                        <tr>
                                            <td width="50px">
                                                <span class="span-label-header">Join Type:</span>
                                            </td>
                                            <td width="100px">
                                                <span class="span-label-header">Select Table:</span>
                                            </td>
                                            <td width="150px">
                                                <span class="span-label-header">
                                                    <asp:Label Text="" ID="lbFirstTableName" runat="server" /></span>
                                            </td>
                                            <%--  <td width="100px">
                                                <span class="span-label-header">Select Table:</span>
                                            </td>--%>
                                            <td width="150px">
                                                <span class="span-label-header">
                                                    <asp:Label Text="" ID="lbSecondTableName" runat="server" /></span>
                                            </td>
                                            <td width="80px">
                                                <span class="span-label-header">Operator:</span>
                                            </td>
                                            <td width="50px">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="DdlJoin" runat="server" CssClass="formulafields" onchange="ddlJoin_Change(this);">
                                                    <asp:ListItem Text="Inner Join" Value="INNER" />
                                                    <asp:ListItem Text="Outer Join" Value="OUTER" />
                                                    <asp:ListItem Text="Union" Value="UNION" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlAvlTables" CssClass="formulafields" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdlAvlTables_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlFirstTabCols" Width="150px" CssClass="formulafields ddlfirstcol" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlSecondTabCols" Width="150px" CssClass="formulafields ddlsecondcol" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="DdlOperator" runat="server" Width="50px" CssClass="formulafields ddloperator">
                                                    <asp:ListItem Text="=" Value="=" />
                                                    <asp:ListItem Text="!=" Value="<>" />
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <span>
                                                    <asp:Button ID="btnAddJoin" Text="Add" runat="server" CssClass="joinbutton" OnClick="btnAddJoin_Click" />
                                                </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <asp:TextBox ID="JoinTextArea" Enabled="false" Width="99%" Height="100px" TextMode="MultiLine"
                                                    runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_3" runat="server" CssClass="container py-3">
                        <asp:Panel ID="columnsInfoPanel" runat="server">
                            <div>
                                <table>
                                    <tr>
                                        <td align="left" width="100%">
                                            <span class="span-label">Select Columns:</span>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                                <div class="columninfo-grid">
                                    <ugit:ASPxGridView ID="grdColumns" runat="server" ClientInstanceName="grdColumns" AutoGenerateColumns="false"  SettingsPager-Mode="ShowAllRecords" Width="100%" 
                                        OnHtmlRowPrepared="grdColumns_HtmlRowCreated" Styles-GroupRow-Font-Bold="true" >
                                        <SettingsBehavior AutoExpandAllGroups="true" />
                                        <Settings ShowGroupPanel="false" />
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Sequence">
                                                <DataItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlSequence" SelectedValue='<%# Bind("Sequence") %>'
                                                        Width="46px" CssClass="sequenceddl" OnInit="ddlSequence_Load">
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Column Name">
                                                <DataItemTemplate>
                                                    <span>
                                                        <asp:HiddenField ID="hTableName" runat="server" Value='<%# Bind("TableName") %>' />
                                                        <asp:CheckBox ID="chkSelect" Text='<%# Bind("FieldName") %>' runat="server"
                                                            Checked='<%# Bind("Selected") %>' CssClass="column-name" />
                                                    </span>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Display Name">
                                                <DataItemTemplate>
                                                    <asp:TextBox ID="txtLabel" runat="server" Text='<%# Bind("DisplayName") %>' Width="185px" />
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Format">
                                                <DataItemTemplate>
                                                    <asp:HiddenField ID="hdnDataType" runat="server" Value='<%# Bind("DataType") %>' />
                                                    <asp:DropDownList ID="ddlDataType" runat="server">
                                                        <asp:ListItem Value="none">None</asp:ListItem>
                                                        <asp:ListItem Value="Currency">Currency</asp:ListItem>
                                                        <asp:ListItem Value="Double">Double</asp:ListItem>
                                                        <asp:ListItem Value="Percent">Percent</asp:ListItem>
                                                        <asp:ListItem Text="Percent*100">Percent*100</asp:ListItem>
                                                        <asp:ListItem Value="Integer">Integer</asp:ListItem>
                                                        <asp:ListItem Value="Date">Date</asp:ListItem>
                                                        <asp:ListItem Value="Days">Days</asp:ListItem>
                                                        <asp:ListItem Value="DateTime">DateTime</asp:ListItem>
                                                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                                        <asp:ListItem Value="Time">Time</asp:ListItem>
                                                        <asp:ListItem Value="String">String</asp:ListItem>
                                                        <asp:ListItem Text="Progress">Progress</asp:ListItem>
                                                        <asp:ListItem Value="Boolean">Boolean</asp:ListItem>
                                                        <asp:ListItem Value="User">User</asp:ListItem>
                                                        <asp:ListItem Value="MultiUser">MultiUser</asp:ListItem>
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Alignment">
                                                <DataItemTemplate>
                                                    <asp:HiddenField ID="hdnAlignment" runat="server" Value='<%# Bind("Alignment") %>' />
                                                    <asp:DropDownList ID="ddlAlignment" runat="server">
                                                        <asp:ListItem Text="None" />
                                                        <asp:ListItem Text="Center" />
                                                        <asp:ListItem Text="Left" />
                                                        <asp:ListItem Text="Right" />
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Functions">
                                                <DataItemTemplate>
                                                    <asp:HiddenField ID="hdnFunctions" runat="server" Value='<%# Bind("Function") %>' />
                                                    <asp:DropDownList ID="ddlFunctions" runat="server">
                                                        <asp:ListItem Text="none" />
                                                        <asp:ListItem Text="Count" />
                                                        <asp:ListItem Text="Avg" />
                                                        <asp:ListItem Text="Sum" />
                                                        <asp:ListItem Text="Max" />
                                                        <asp:ListItem Text="Min" />
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Width">
                                                <DataItemTemplate>
                                                    <asp:TextBox ID="txtWidthColumn" runat="server" TextMode="Number" Text='<%# Bind("Width") %>' Width="45px" />
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TableName" Visible="false"/>
                                        </Columns>
                                    </ugit:ASPxGridView>
                                    <script data-v="<%=UGITUtility.AssemblyVersion %>">

                                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                            UpdateGridHeight();
                                        });
                                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                            UpdateGridHeight();
                                        });
                                        function UpdateGridHeight() {
                                            debugger;
                                            grdColumns.SetHeight(0);
                                            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                                            if (document.body.scrollHeight > containerHeight)
                                                containerHeight = document.body.scrollHeight;
                                            grdColumns.SetHeight(containerHeight - 100);
                                        }
                                        window.addEventListener('resize', function (evt) {
                                            if (!ASPxClientUtils.androidPlatform)
                                                return;
                                            var activeElement = document.activeElement;
                                            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                                                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                                        });
                                    </script>
                                </div>
                            </div>
                        </asp:Panel>

                    </asp:Panel>
                    <asp:Panel ID="tabPanel_4" runat="server" CssClass="container py-3">
                        <script type="text/javascript">
                            function getSelectedQueryItems(all) {
                                var queryItems = $(".aspxqueryitem:checked");
                                if (all)
                                    queryItems = $(".aspxqueryitem");

                                if (queryItems.length == 0) {
                                    return [];
                                }

                                var items = [];
                                $.each(queryItems, function (i, s) {
                                    items.push({ itemid: $(s).attr("itemid"), parentid: $(s).attr("parentid") });
                                });
                                return items;
                            }
                            function btCreateGroup_click(s, e) {
                                var items = getSelectedQueryItems();
                                if (items.length <= 1) {
                                    e.processOnServer = false;
                                    return;
                                }

                                var itemIds = [];
                                for (var i = 0; i < items.length; i++) {
                                    itemIds.push(items[i].itemid);
                                }

                                hdnQueryParams.Set("action", 'create');
                                hdnQueryParams.Set("items", itemIds.join(','));
                                LoadingPanel.Show();

                            }

                            function btRemoveGroup_click(id) {
                                hdnQueryParams.Set("items", id);
                                LoadingPanel.Show();
                                btRemoveGroup.DoClick();
                            }

                            function detectQueryGroup() {
                                var items = getSelectedQueryItems();
                                var allItems = getSelectedQueryItems(true);
                                var enableCreate = true;

                                if (items.length == 0)
                                    enableCreate = false;

                                btCreateGroup.SetEnabled(enableCreate);

                                //enable create group
                                for (var i = 0; i < items.length; i++) {
                                    if (items[i].parentid > 0 || items.length <= 1) {
                                        enableCreate = false;
                                        break;
                                    }

                                    for (var j = 0; j < allItems.length; j++) {
                                        if (allItems[j].parentid == items[i].itemid) { enableCreate = false; break; }
                                    }
                                }
                                btCreateGroup.SetEnabled(enableCreate);
                            }
                        </script>
                        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" CssClass="customeLoader" Image-Url="~/Content/IMAGES/ajax-loader.gif" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
                        </dx:ASPxLoadingPanel>
                        <dx:ASPxHiddenField ID="hdnQueryParams" ClientInstanceName="hdnQueryParams" runat="server"></dx:ASPxHiddenField>
                        <asp:ListView ID="WhereClauseList" runat="server" ItemPlaceholderID="phItem"
                            OnItemDeleting="WhereClauseList_ItemDeleting"
                            OnItemDataBound="WhereClauseList_ItemDataBound" DataKeyNames="ID">
                            <LayoutTemplate>
                                <table class="ms-listviewtable " width="100%" cellpadding="2" cellspacing="0" style="padding: 3px;">
                                    <tr class="detailviewheader ms-viewheadertr">
                                        <th class="ms-vh2">
                                            <b>Group</b>
                                        </th>
                                        <th class="ms-vh2">
                                            <b>And/Or</b>
                                        </th>
                                        <th class="ms-vh2">
                                            <b>Column</b>
                                        </th>
                                        <th class="ms-vh2">
                                            <b>Operator</b>
                                        </th>
                                        <th class="ms-vh2">
                                            <b>Value Type</b>
                                        </th>

                                        <th class="ms-vh2">
                                            <b>Parameter Type</b>
                                        </th>
                                        <th class="ms-vh2" style="width: 150px">
                                            <b>Value</b>
                                        </th>
                                        <th class="ms-vh2" style="width: 50px">
                                            <b>Actions</b>
                                        </th>
                                    </tr>
                                    <asp:PlaceHolder ID="phItem" runat="server"></asp:PlaceHolder>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <input type="checkbox" onclick="detectQueryGroup();" class='aspxqueryitem' parentid="<%# Eval("ParentID") %>" itemid="<%# Eval("ID") %>" />
                                     </td>
                                    <td class="padding-left5">
                                        <asp:Label ID="lblRelationOpt" CssClass='<%# Convert.ToInt32(Eval("ParentID")) > 0 ? "filtergrouping padding-right5":"" %>' runat="server" Text='<%# Eval("RelationOpt") %>'></asp:Label>
                                        <img class='remove-group <%#  IsGroupParent(Convert.ToInt32(Eval("ID"))) ? "" :"hide" %> <%# (IsGroupParent(Convert.ToInt32(Eval("ID"))) && Convert.ToInt32(Eval("ID")) == 1) ? "firstgroupitem" :"" %>' src="/Content/Images/indent-dec.png" title="Remove Group" onclick="btRemoveGroup_click('<%# Eval("ID") %>')" />
                                    </td>
                                    <td>
                                        <%# Eval("ColumnName") %>
                                    </td>
                                    <td>
                                        <%# uGovernIT.Manager.QueryHelperManager.GetOperatorDisplayFormatFromType((uGovernIT.Utility.OperatorType)Enum.Parse(typeof(uGovernIT.Utility.OperatorType), Convert.ToString(Eval("Operator")))) %>
                                    </td>
                                    <td>
                                        <%# Eval("Valuetype") %>
                                    </td>
                                    <td>
                                        <%# Eval("ParameterType") %>
                                    </td>
                                    <td>
                                        <%# (Convert.ToString(Eval("Valuetype")) == "Parameter") ? Eval("ParameterName") : Eval("Value") %>
                                    </td>
                                    <td>
                                        <asp:ImageButton runat="server" ID="lnkEdit" ImageUrl="/content/buttonimages/edit-icon.png"
                                            BorderWidth="0" ToolTip="Edit" />
                                        <%-- CommandName="Edit"--%>
                                        <span onclick="return confirm('Are you sure you want to delete?')">
                                            <asp:ImageButton runat="server" ID="lnkDelete" CommandName="Delete" ImageUrl="/content/images/delete-icon-new.png"
                                                BorderWidth="0" ToolTip="Delete" />
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr style="background-color: #F5F5F5;">
                                    <td>
                                        <input type="checkbox" class='aspxqueryitem' onclick="detectQueryGroup();" parentid="<%# Eval("ParentID") %>" itemid="<%# Eval("ID") %>" />
                                    </td>
                                    <td class="padding-left5">
                                        <asp:Label ID="lblRelationOpt" runat="server" CssClass='<%# Convert.ToInt32(Eval("ParentID")) > 0 ? "filtergrouping padding-right5":"" %>' Text='<%# Eval("RelationOpt") %>'></asp:Label>
                                        <img class='remove-group <%#  IsGroupParent(Convert.ToInt32(Eval("ID"))) ? "" :"hide" %> <%# (IsGroupParent(Convert.ToInt32(Eval("ID"))) && Convert.ToInt32(Eval("ID")) == 1) ? "firstgroupitem" :"" %>' src="/Content/Images/indent-dec.png" title="Remove Group" onclick="btRemoveGroup_click('<%# Eval("ID") %>')" />
                                    </td>
                                    <td>
                                        <%# Eval("ColumnName") %>
                                    </td>
                                    <td>
                                        <%# uGovernIT.Manager.QueryHelperManager.GetOperatorDisplayFormatFromType((uGovernIT.Utility.OperatorType)Enum.Parse(typeof(uGovernIT.Utility.OperatorType), Convert.ToString(Eval("Operator")))) %>
                                    </td>
                                    <td>
                                        <%# Eval("Valuetype") %>
                                    </td>
                                    <td>
                                        <%# Eval("ParameterType") %>
                                    </td>
                                    <td>
                                        <%# (Convert.ToString(Eval("Valuetype")) == "Parameter") ? Eval("ParameterName") : Eval("Value")%>
                                    </td>
                                    <td>
                                        <asp:ImageButton runat="server" ID="lnkEdit" ImageUrl="/content/buttonimages/edit-icon.png"
                                            BorderWidth="0" ToolTip="Edit" />
                                        <span onclick="return confirm('Are you sure you want to delete?')">
                                            <asp:ImageButton runat="server" ID="lnkDelete" CommandName="Delete" ImageUrl="/content/images/delete-icon-new.png"
                                                BorderWidth="0" ToolTip="Delete" />
                                        </span>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>

                        </asp:ListView>
                        <div>
                            <dx:ASPxButton ID="aspxAddQuery" RenderMode="Link" Image-Url="/content/images/add_icon.png" ClientInstanceName="aspxAddQuery" runat="server" 
                                Text="Add Filter" AutoPostBack="false">
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="btCreateGroup" ClientEnabled="false" Paddings-PaddingLeft="5px" CssClass="padding-left5" RenderMode="Link" OnClick="btCreateGroup_Click"
                                 Image-Url="/content/images/add_icon.png" ClientInstanceName="btCreateGroup" runat="server" Text="Create Group" AutoPostBack="true">
                                <ClientSideEvents Click="btCreateGroup_click" />
                            </dx:ASPxButton>

                            <dx:ASPxButton ID="btRemoveGroup" ClientVisible="false" Paddings-PaddingLeft="5px" CssClass="padding-left5" RenderMode="Link" OnClick="btRemoveGroup_Click" 
                                Image-Url="/Content/Images/delete-iconOld.png" ClientInstanceName="btRemoveGroup" runat="server" Text="Remove Group" AutoPostBack="true">
                            </dx:ASPxButton>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_5" runat="server" CssClass="container py-3">
                        <asp:UpdatePanel ID="upGroupBy" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="groupByPanel" runat="server">
                                    <div>
                                        <div class="headerdiv">Select upto 3 Columns:</div>
                                        <div class="headerdiv">
                                            <dx:ASPxCheckBox ID="chkbxExpandGrouping" Text="Start Expanded" runat="server"></dx:ASPxCheckBox>
                                        </div>
                                        <div class="categorymenu-container" id="Div3">
                                            <table cellpadding="2" cellspacing="2" width="100%" class='altRowsSortBy'>
                                                <tr>
                                                    <td>
                                                        <span class="group-span-col">First Group by the column</span>
                                                        <span class="group-span-col">Display Text</span>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class='filterSelect'>
                                                        <span class="group-span-col">
                                                            <asp:DropDownList ID="ddlFirstGroupBy" Width='250px' runat="server" OnSelectedIndexChanged="ddlFirstGroupBy_SelectedIndexChanged" AutoPostBack="true" />
                                                        </span>
                                                        <span class="group-span-col">
                                                            <asp:TextBox ID="txtFirstGroupByDisplaytext" runat="server" Width="200px" />
                                                        </span>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td>
                                                        <span class="group-span-col">Second Group by the column</span>
                                                        <span class="group-span-col">Display Text</span>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class='filterSelect'>
                                                        <span class="group-span-col">
                                                            <asp:DropDownList ID="ddlSecondGroupBy" Width='250px' runat="server" OnSelectedIndexChanged="ddlSecondGroupBy_SelectedIndexChanged" AutoPostBack="true" /></span>
                                                        <span class="group-span-col">
                                                            <asp:TextBox ID="txtSecondGroupByDisplayText" runat="server" Width="200px" />
                                                        </span>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <td>
                                                        <span class="group-span-col">Third Group by the column</span>
                                                        <span class="group-span-col">Display Text</span>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td class='filterSelect'>
                                                        <span class="group-span-col">
                                                            <asp:DropDownList ID="ddlThirdGroupBy" Width='250px' runat="server" OnSelectedIndexChanged="ddlThirdGroupBy_SelectedIndexChanged" AutoPostBack="true" /></span>
                                                        <span class="group-span-col">
                                                            <asp:TextBox ID="txtThirdGroupByDisplayText" runat="server" Width="200px" />
                                                        </span>
                                                    </td>

                                                </tr>
                                            </table>
                                        </div>

                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_6" runat="server" CssClass="container py-3">
                        <asp:Panel ID="sortByPanel" runat="server" Style="height: 205px;">
                            <div>
                                <div class="headerdiv">Select columns to determine the order in which the items in the view are displayed</div>
                                <div class="categorymenu-container" id="Div1">
                                    <table cellpadding="2" cellspacing="2" width="100%" class='altRowsSortBy'>
                                        <tr class='altRowsSortBy'>
                                            <td>
                                                <asp:Label ID="Label1" Text="First sort by the column" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='filterSelect'>
                                                <asp:DropDownList ID="ddlFirstOrderBy" Width='250px' runat="server" />
                                            </td>
                                        </tr>
                                        <tr class='altRowsSortBy'>
                                            <td class="radioClass">
                                                <asp:RadioButton ID="rdFirstAscending" runat="server" Text="ASC" GroupName="RadioGroup1"
                                                    Checked="true" />

                                                <asp:RadioButton ID="rdFirstDescending" runat="server" Text="DESC" GroupName="RadioGroup1" />
                                            </td>
                                        </tr>

                                        <tr class='altRowsSortBy'>
                                            <td>
                                                <asp:Label ID="Label2" Text="Second sort by the column" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='filterSelect'>
                                                <asp:DropDownList ID="ddlSecondOrderBy" Width='250px' runat="server" />
                                            </td>
                                        </tr>
                                        <tr class='altRowsSortBy'>
                                            <td class="radioClass">
                                                <asp:RadioButton ID="rdSecondAscending" runat="server" Text="ASC" GroupName="RadioGroup2"
                                                    Checked="true" />

                                                <asp:RadioButton ID="rdSecondDescending" runat="server" Text="DESC" GroupName="RadioGroup2" />
                                            </td>
                                        </tr>

                                        <tr class='altRowsSortBy'>
                                            <td>
                                                <asp:Label ID="Label3" Text="Then sort by the column" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class='filterSelect'>
                                                <asp:DropDownList ID="ddlThirdOrderBy" Width='250px' runat="server" />
                                            </td>
                                        </tr>
                                        <tr class='altRowsSortBy'>
                                            <td class="radioClass">
                                                <asp:RadioButton ID="rdThirdAscending" runat="server" Text="ASC" GroupName="RadioGroup3"
                                                    Checked="true" />

                                                <asp:RadioButton ID="rdThirdDescending" runat="server" Text="DESC" GroupName="RadioGroup3" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_7" runat="server" CssClass="container py-3">
                        <asp:Panel ID="panel_totals" runat="server">
                            <div>
                                <div class="headerdiv">
                                    <span class="span-label">Select Columns:</span>
                                </div>
                                <div class="columninfo-grid">
                                    <ugit:ASPxGridView ID="spgrid_totals" runat="server" Width="100%" KeyFieldName="ID" AutoGenerateColumns="false" 
                                        OnHtmlRowPrepared="spgrid_totals_RowDataBound" Styles-GroupRow-Font-Bold="true">
                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Column Name">
                                                <DataItemTemplate>
                                                    <span>
                                                        <asp:HiddenField ID="hTableName" runat="server" Value='<%# Bind("TableName") %>' />
                                                        <asp:CheckBox ID="chkSelect" Text='<%# Bind("FieldName") %>' runat="server"
                                                            Checked='<%# Bind("Selected") %>' CssClass="column-name" />
                                                    </span>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Functions">
                                                <DataItemTemplate>
                                                    <asp:HiddenField ID="hdnFunctions" runat="server" Value='<%# Bind("Function") %>' />
                                                    <asp:DropDownList ID="ddlFunctions" runat="server">
                                                        <asp:ListItem Text="none" />
                                                        <asp:ListItem Text="Count" />
                                                        <asp:ListItem Text="Avg" />
                                                        <asp:ListItem Text="Sum" />
                                                        <asp:ListItem Text="Max" />
                                                        <asp:ListItem Text="Min" />
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TableName" Visible="false" />
                                        </Columns>
                                        <SettingsBehavior AutoExpandAllGroups="true" />
                                        <Settings ShowGroupButtons="true" ShowGroupPanel="false" />
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    </ugit:ASPxGridView>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_8" runat="server" CssClass="container py-3">
                        <asp:Panel ID="pnl_queryformat" runat="server">
                            <div>
                                <span class="span-label">Query Format:</span>

                                <div class="query-formatter-div">
                                    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                        <tr id="qfquerytype">
                                            <td class="ms-formlabel">Query Type
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlQueryType" runat="server" CssClass="querytype-dropdown">
                                                    <asp:ListItem Text="Simple Number" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Formatted Number" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Column" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Row" Value="4"></asp:ListItem>
                                                    <asp:ListItem Text="Table" Value="5"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="qfnumfontstyle">
                                            <td class="ms-formlabel">Result Font Style
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlFontName" runat="server">
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlNumFontStyle" runat="server">
                                                        <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                                        <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                                        <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                                        <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlNumFontSize" runat="server">
                                                        <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                                        <asp:ListItem Value="8pt" Selected="True">8pt</asp:ListItem>
                                                        <asp:ListItem Value="10pt">10pt</asp:ListItem>
                                                        <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                                        <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                                        <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                                        <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                                        <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                                        <asp:ListItem Value="38pt">36pt</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <dx:ASPxColorEdit runat="server" ID="ceNumFont" Color="#000000">
                                                    </dx:ASPxColorEdit>
                                                </span>
                                            </td>
                                        </tr>

                                        <tr id="qftitle">
                                            <td class="ms-formlabel">Text
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <asp:TextBox ID="txtText" runat="server" class="longtext" />
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlTitlePosition" runat="server">
                                                        <asp:ListItem Text="Top" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Bottom" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: right;">
                                                    <asp:CheckBox Text="Hide" runat="server" ID="chkTextHide" /></span>
                                            </td>
                                        </tr>
                                        <tr id="qftitlefontstyle">
                                            <td class="ms-formlabel">Text Font Name & Style
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlTextFontName" runat="server">
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlFontStyle" runat="server">
                                                        <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                                        <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                                        <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                                        <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlFontSize" runat="server">
                                                        <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                                        <asp:ListItem Value="8pt" Selected="True">8pt</asp:ListItem>
                                                        <asp:ListItem Value="10pt">10pt</asp:ListItem>
                                                        <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                                        <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                                        <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                                        <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                                        <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                                        <asp:ListItem Value="38pt">36pt</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <dx:ASPxColorEdit runat="server" ID="ceFont" Color="#000000">
                                                    </dx:ASPxColorEdit>
                                                </span>

                                            </td>
                                        </tr>
                                        <tr id="qflabel">
                                            <td class="ms-formlabel">Label
                                            </td>
                                            <td class="ms-formbody">

                                                <span style="float: left">
                                                    <asp:TextBox ID="txtLabel" runat="server" class="longtext" />
                                                </span>

                                                <span style="float: right;">
                                                    <asp:CheckBox Text="Hide" runat="server" ID="chkLabelHide" /></span>
                                            </td>
                                        </tr>
                                        <tr id="qflabelfontstyle">
                                            <td class="ms-formlabel">Label Font Name & Style
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlLabelFontName" runat="server">
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlLabelFontStyle" runat="server">
                                                        <asp:ListItem Value="Bold">Bold</asp:ListItem>
                                                        <asp:ListItem Value="Italic">Italic</asp:ListItem>
                                                        <asp:ListItem Value="Regular">Regular</asp:ListItem>
                                                        <asp:ListItem Value="Underline">Underline</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <asp:DropDownList ID="ddlLabelFontSize" runat="server">
                                                        <asp:ListItem Value="6pt">6pt</asp:ListItem>
                                                        <asp:ListItem Value="8pt" Selected="True">8pt</asp:ListItem>
                                                        <asp:ListItem Value="10pt">10pt</asp:ListItem>
                                                        <asp:ListItem Value="12pt">12pt</asp:ListItem>
                                                        <asp:ListItem Value="14pt">14pt</asp:ListItem>
                                                        <asp:ListItem Value="18pt">18pt</asp:ListItem>
                                                        <asp:ListItem Value="24pt">24pt</asp:ListItem>
                                                        <asp:ListItem Value="30pt">30pt</asp:ListItem>
                                                        <asp:ListItem Value="38pt">36pt</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <span style="float: left">
                                                    <dx:ASPxColorEdit runat="server" ID="ceLabelFont" Color="#000000">
                                                    </dx:ASPxColorEdit>
                                                </span>
                                            </td>
                                        </tr>

                                        <tr id="qfbgImage">
                                            <td class="ms-formlabel">Background Image
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <asp:TextBox ID="txtBackgroundimage" runat="server" class="imageurlinput" />
                                                </span>
                                                <span style="float: left">
                                                    <asp:FileUpload ID="uploadBackgroundImage" runat="server" />
                                                </span>

                                            </td>
                                        </tr>
                                        <tr id="qfsize">
                                            <td class="ms-formlabel">Frame Size & Position
                                            </td>
                                            <td class="ms-formbody">
                                                <span class="span-inline col1"><b>Height:</b>
                                                    <asp:TextBox ID="txtHeight" runat="server" type="number" Text="0" />px
                                                </span>
                                                <span class="span-inline col2"><b>Width:</b>
                                                    <asp:TextBox ID="txtWidth" runat="server" type="number" Text="0" />px
                                                </span>
                                                <span class="span-inline col1"><b>Left:</b>
                                                    <asp:TextBox ID="txtLocationLeft" runat="server" type="number" Text="0" />px
                                                </span>
                                                <span class="span-inline col2"><b>Top:</b>
                                                    <asp:TextBox ID="txtLocationTop" runat="server" type="number" Text="0" />px
                                                </span>
                                            </td>
                                        </tr>

                                        <tr id="qfresultPnldesign">
                                            <td class="ms-formlabel">Result Panel Design
                                            </td>
                                            <td class="ms-formbody">

                                                <asp:DropDownList ID="ddlResultPnlDesign" runat="server" onchange="ShowDesignControls(this);">
                                                    <asp:ListItem Text="Without Icon or Border" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="With Icon but no Border" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="With Icon and Border" Value="3"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="qficon">
                                            <td class="ms-formlabel">Icon
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <asp:TextBox ID="txtIconImage" runat="server" class="imageurlinput" />
                                                </span>
                                                <span style="float: left">
                                                    <asp:FileUpload ID="uploadIconImage" runat="server" />
                                                </span>

                                            </td>
                                        </tr>
                                        <tr id="qficonposition">
                                            <td class="ms-formlabel">Icon Size & Position
                                            </td>
                                            <td class="ms-formbody">
                                                <span class="span-inline col1"><b>Height:</b>
                                                    <asp:TextBox ID="txticonHeight" runat="server" type="number" Text="0" />px
                                                </span>
                                                <span class="span-inline col2"><b>Width:</b>
                                                    <asp:TextBox ID="txticonWidth" runat="server" type="number" Text="0" />px
                                                </span>
                                                <span class="span-inline col1"><b>Right:</b>
                                                    <asp:TextBox ID="txticonPositionLeft" runat="server" type="number" Text="0" />px
                                                </span>
                                                <span class="span-inline col2"><b>Top:</b>
                                                    <asp:TextBox ID="txticonPositionTop" runat="server" type="number" Text="0" />px
                                                </span>
                                            </td>
                                        </tr>
                                        <tr id="qfbgcolor">
                                            <td class="ms-formlabel">Background 
                                            </td>
                                            <td class="ms-formbody">
                                                <%--  <span class="span-inline col1">--%>
                                                <dx:ASPxColorEdit ID="ceBGColor" ClientInstanceName="ceBGColor" runat="server" Color="#FFFFFF">
                                                </dx:ASPxColorEdit>
                                                <%--    </span>--%>
                                                <%-- <span class="span-inline col2">--%>
                                                <asp:CheckBox ID="chkTransparent" Text="Transparent" runat="server" />
                                                <%-- </span>--%>
                                            </td>
                                        </tr>
                                        <tr id="qftextalign">
                                            <td class="ms-formlabel">Text-Align
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList ID="ddlTextAlign" runat="server">
                                                    <asp:ListItem Text="Center" Value="center"></asp:ListItem>
                                                    <asp:ListItem Text="Left" Value="left"></asp:ListItem>
                                                    <asp:ListItem Text="Right" Value="right"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="qfborder">
                                            <td class="ms-formlabel">Border
                                            </td>
                                            <td class="ms-formbody">
                                                <span style="float: left">
                                                    <dx:ASPxColorEdit ID="ceBorderColor" runat="server" Color="#FFFFFF"></dx:ASPxColorEdit>
                                                </span>
                                                <span id="spanborderwidth">
                                                    <asp:TextBox ID="txtBorderWidth" runat="server" type="number" Text="0" />px
                                                </span>
                                            </td>
                                        </tr>
                                        <tr id="qfheadercolor">
                                            <td class="ms-formlabel">Header Color
                                            </td>
                                            <td class="ms-formbody">
                                                <dx:ASPxColorEdit ID="ceHeaderColor" runat="server" Color="#FFFFFF"></dx:ASPxColorEdit>

                                            </td>
                                        </tr>
                                        <tr id="qfrowcolor">
                                            <td class="ms-formlabel">Row Color
                                            </td>
                                            <td class="ms-formbody">
                                                <dx:ASPxColorEdit ID="ceRowColor" runat="server" Color="#FFFFFF"></dx:ASPxColorEdit>

                                            </td>
                                        </tr>
                                        <tr id="qfrowaltercolor">
                                            <td class="ms-formlabel">Alternate-Row Color
                                            </td>
                                            <td class="ms-formbody">
                                                <dx:ASPxColorEdit ID="ceRowAlterColor" runat="server" Color="#FFFFFF"></dx:ASPxColorEdit>
                                            </td>
                                        </tr>

                                        <tr id="qfdrilldowntype">
                                            <td class="ms-formlabel">Drill Down Type
                                            </td>
                                            <td class="ms-formbody">
                                                <span class="span-inline col1">
                                                    <asp:DropDownList ID="ddlDrillDown" runat="server" onchange="ShowCustomUrl(this);">
                                                        <asp:ListItem Text="None" Value="0" />
                                                        <asp:ListItem Text="Default Drill down" Value="1" />
                                                        <asp:ListItem Text="Custom Query" Value="2" />
                                                        <asp:ListItem Text="Custom Url" Value="3" />
                                                    </asp:DropDownList>
                                                </span>
                                                <span id="spandrilldown" class="span-inline">
                                                    <a href="javascript:;" onclick="tabMenu.SetActiveTab(tabMenu.tabs[8]); tabClick(tabMenu.tabs[8].name); "
                                                        style="margin-top: 3px; margin-left: 5px; float: left;"><i>Select drill down columns</i></a>
                                                </span>
                                                <span id="spanQuery" class="span-inline col2">
                                                    <asp:DropDownList ID="ddlqueries" runat="server"></asp:DropDownList>
                                                </span>

                                                <span id="spancustomUrl" class="span-inline">
                                                    <asp:TextBox ID="txtCustomUrl" runat="server" Style="width: 200px; margin-left: 5px;" />
                                                    <span class="span-inline">
                                                        <asp:DropDownList ID="ddlNavigationType" runat="server">
                                                            <asp:ListItem Text="Popup" />
                                                            <asp:ListItem Text="Navigation" />
                                                        </asp:DropDownList>
                                                    </span>
                                                </span>
                                            </td>
                                        </tr>
                                        <tr id="trenableediturl" runat="server">
                                            <td class="ms-formlabel">Enable Edit Url
                                            </td>
                                            <td class="ms-formbody">
                                                <dx:ASPxCheckBox ID="chkEnableEditUrl" runat="server" ClientInstanceName="chkEnableEditUrl"></dx:ASPxCheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ms-formlabel" colspan="2"><b style="float: left; padding-left: 5px;">Export Options:</b>
                                            </td>
                                        </tr>

                                        <tr id="trShowHeader">
                                            <td class="ms-formlabel">Header
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:TextBox ID="txtHeader" runat="server" type="Text" Width="280px" />
                                            </td>
                                        </tr>

                                        <tr id="trShowFooter">
                                            <td class="ms-formlabel">Footer
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:TextBox ID="txtFooter" runat="server" type="Text" Width="280px" />
                                            </td>
                                        </tr>

                                        <tr id="trShowCompanyLogo">
                                            <td class="ms-formlabel">Show Company Logo
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:CheckBox ID="chkShowCompanyLogo" runat="server" />
                                            </td>
                                        </tr>

                                        <tr id="trShowDateInFooter">
                                            <td class="ms-formlabel">Show Date In Footer
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:CheckBox ID="chkShowDateInFooter" runat="server" />
                                            </td>
                                        </tr>

                                        <tr id="trAdditionalInfo">
                                            <td class="ms-formlabel">Additional Info
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:TextBox ID="txtAdditionalInfo" runat="server" type="Text" Width="280px" />
                                            </td>
                                        </tr>
                                        <tr id="tr1">
                                            <td class="ms-formlabel">Additional Footer Info
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:TextBox ID="txtAdditionalFooterInfo" runat="server" type="Text" Width="280px" />
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel ID="tabPanel_9" runat="server" CssClass="container py-3">
                        <asp:Panel ID="pnl_drilldown" runat="server">
                            <div>
                                <table>
                                    <tr>
                                        <td align="left" width="100%">
                                            <span class="span-label">Select Columns:</span>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                                <div class="columninfo-grid">
                                    <ugit:ASPxGridView ID="gvDrillDownColumns" runat="server" EnableViewState="false"
                                        AlternatingRowStyle-BackColor="WhiteSmoke" AllowGrouping="true"
                                        HeaderStyle-Height="20px" HeaderStyle-CssClass="aa" AutoGenerateColumns="false" DataKeyNames="ID"
                                        GroupFieldDisplayName=" TableName " AllowFiltering="true" GroupField="TableName" AllowGroupCollapse="true" 
                                        OnRowDataBound="grdColumns_RowDataBound" Styles-GroupRow-Font-Bold="true">

                                        <Columns>
                                            <dx:GridViewDataColumn Caption="Column Name">
                                                <DataItemTemplate>
                                                    <span>
                                                        <asp:HiddenField ID="hTableName" runat="server" Value='<%# Bind("TableName") %>' />
                                                        <asp:CheckBox ID="chkSelect" Text='<%# Bind("FieldName") %>' runat="server"
                                                            Checked='<%# Bind("Selected") %>' />
                                                    </span>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Format">
                                                <DataItemTemplate>
                                                    <asp:HiddenField ID="hdnDataType" runat="server" Value='<%# Bind("DataType") %>' />
                                                    <asp:DropDownList ID="ddlDataType" runat="server">
                                                        <asp:ListItem Text="none" />
                                                        <asp:ListItem Text="Currency" />
                                                        <asp:ListItem Text="Double" />
                                                        <asp:ListItem Text="Percent" />
                                                        <asp:ListItem Text="Percent*100" />
                                                        <asp:ListItem Text="Integer" />
                                                        <asp:ListItem Text="DateTime" />
                                                        <asp:ListItem Text="Date" />
                                                        <asp:ListItem Text="Days" />
                                                        <asp:ListItem Text="Hours" />
                                                        <asp:ListItem Text="Minutes" />
                                                        <asp:ListItem Text="Time" />
                                                        <asp:ListItem Text="String" />
                                                        <asp:ListItem Text="Progress" />
                                                        <asp:ListItem Text="Boolean" />
                                                        <asp:ListItem Text="User" />
                                                        <asp:ListItem Text="MultiUser" />
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Functions">
                                                <DataItemTemplate>
                                                    <asp:HiddenField ID="hdnFunctions" runat="server" Value='<%# Bind("Function") %>' />
                                                    <asp:DropDownList ID="ddlFunctions" runat="server">
                                                        <asp:ListItem Text="none" />
                                                        <asp:ListItem Text="Count" />
                                                        <asp:ListItem Text="Avg" />
                                                        <asp:ListItem Text="Sum" />
                                                        <asp:ListItem Text="Max" />
                                                        <asp:ListItem Text="Min" />
                                                        <asp:ListItem Text="Distinct Count" />
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Display Name">
                                                <DataItemTemplate>
                                                    <asp:TextBox ID="txtLabel" runat="server" Text='<%# Bind("DisplayName") %>' Width="200px" />
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn Caption="Sequence">
                                                <DataItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlSequence"
                                                        Width="40px" CssClass="sequenceddl" OnInit="ddlSequence_Load">
                                                    </asp:DropDownList>
                                                </DataItemTemplate>
                                            </dx:GridViewDataColumn>
                                            <dx:GridViewDataColumn FieldName="TableName" GroupIndex="0" />
                                        </Columns>
                                        <SettingsBehavior AutoExpandAllGroups="true" />
                                        <Settings ShowGroupPanel="false" />
                                    </ugit:ASPxGridView>
                                </div>
                            </div>
                        </asp:Panel>

                    </asp:Panel>
                </td>
                <td class="main-table-td2">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="queryDisplayPanel" runat="server" CssClass='query-container p-2'>
                                <asp:Literal ID="litQuery" runat="server">

                                </asp:Literal>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="pt-2 first_tier_nav">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:Button Text="" ID="Preview" runat="server" Style="display: none;" CssClass="preview-button" OnClick="Preview_Click" />

                                    <ul class="mb-0">
                                        <li class="">
                                            <dx:ASPxButton ID="aPreview" runat="server" Text="Preview" AutoPostBack="false" CssClass="primary-blueBtn" ImagePosition="Right" Style="color: white; float: right;">
                                                <Image Url="/content/buttonimages/execute.png"></Image>
                                            </dx:ASPxButton>
                                        </li>
                                        <li class="">
                                            <dx:ASPxButton ID="aSchedule" runat="server" Text="Schedule" AutoPostBack="false" CssClass="primary-blueBtn" ImagePositio="Right" Style="color: white; float: right;">
                                                <Image Url="/content/buttonimages/schedule.png"></Image>
                                            </dx:ASPxButton>
                                        </li>
                                        <li class="">
                                            <dx:ASPxButton ID="btnTemplate" runat="server" Text="Template" AutoPostBack="false" CssClass="primary-blueBtn" ImagePositio="Right" Style="color: white; float: right;">
                                                <Image Url="/content/buttonimages/save-template.png"></Image>
                                                <ClientSideEvents Click="function(s,e){pcTemplate.Show(); return false;}" />
                                            </dx:ASPxButton>
                                        </li>
                                        <li class="">
                                            <dx:ASPxButton ID="btnExpression" runat="server" Text="Expression" AutoPostBack="false" CssClass="primary-blueBtn">
                                                <Image Url="/content/buttonimages/expression.png"></Image>
                                            </dx:ASPxButton>
                                        </li>
                                    </ul>
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
                                                                    <td style="width: 100px;">Template Name:</td>
                                                                    <td style="width: 300px;">
                                                                        <asp:TextBox ID="templateName" runat="server" Width="260px" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trtemplates" style="display: none">
                                                                    <td style="width: 100px;">Templates:</td>
                                                                    <td style="width: 300px;">
                                                                        <asp:DropDownList ID="ddlTemplateList" runat="server" Width="260px"
                                                                            OnPreRender="DdlTemplateList_PreRender">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px;"></td>
                                                                    <td style="width: 300px;">
                                                                        <div class="addEditPopup-btnWrap">
                                                                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false" ImagePosition="Right"
                                                                                UseSubmitBehavior="false" Style="float: left; margin-right: 8px" CssClass="secondary-cancelBtn">
                                                                                <%--<Image Url="/content/buttonimages/cancel.png"></Image>--%>
                                                                                <ClientSideEvents Click="function(s, e) { pcTemplate.Hide();}" />
                                                                            </dx:ASPxButton>
                                                                            <dx:ASPxButton ID="btnOK" runat="server" Text="Save" CssClass="primary-blueBtn" ClientInstanceName="saveTemplateButton" 
                                                                                OnClick="btnOK_Click" Style="float: left; margin-right: 8px" ImagePosition="Right">
                                                                                <%--<Image Url="/content/buttonimages/save.png"></Image>--%>
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dx:ASPxPanel>
                                            </dx:PopupControlContentControl>
                                        </ContentCollection>
                                    </dx:ASPxPopupControl>

                                    <ul style="float: right; padding-right: 40px; margin: 0px;">
                                        <li class="">
                                            <dx:ASPxButton ID="btnValidate" runat="server" Text="Validate" ToolTip="Validate Query" OnClick="btnValidate_Click" 
                                                CssClass="validate-query primary-blueBtn" Style="color: white; float: right;">
                                            </dx:ASPxButton>
                                        </li>
                                    </ul>

                                </td>
                                <td>
                                    <dx:ASPxButton ID="btnhiddenb" runat="server" OnClick="btnBack_Click" ImagePosition="Right" ClientVisible="false" CssClass="back-button" Text=""></dx:ASPxButton>
                                    <dx:ASPxButton ID="btnhiddenn" runat="server" OnClick="btnNext_Click" ClientVisible="false" CssClass="next-button" Text="" ImagePosition="Right"></dx:ASPxButton>

                                    <ul style="float: right; margin: 0px;">
                                        <li class="">
                                            <dx:ASPxButton ID="btnUpdateGeneralInfo" ImagePosition="Right" CssClass="primary-blueBtn" runat="server" OnClick="btnUpdateGeneralInfo_Click" Text="Save Changes">
                                                <Image Url="/content/buttonimages/save.png"></Image>
                                                <ClientSideEvents Click="function(s, e) { loadingPanel.Show(); }" />
                                            </dx:ASPxButton>
                                        </li>
                                        <li class="">
                                            <dx:ASPxButton ID="btnBack" CssClass="primary-blueBtn" ImagePosition="Right" runat="server" OnClick="btnBack_Click" Text="Back">
                                                <Image Url="/content/buttonimages/return.png"></Image>
                                            </dx:ASPxButton>
                                        </li>
                                        <li class="">
                                            <dx:ASPxButton ID="btnNext" CssClass="primary-blueBtn" ImagePosition="Right" runat="server" OnClick="btnNext_Click" Text="Next">
                                                <Image Url="/content/buttonimages/next.png"></Image>
                                            </dx:ASPxButton>
                                        </li>
                                        <li class="">
                                            <dx:ASPxButton ID="ntmClose" CssClass="primary-blueBtn" runat="server" Text="Close" ImagePosition="Right" OnClick="btnClose_Click">
                                                <Image Url="/content/buttonimages/cancel.png"></Image>
                                                <%--<ClientSideEvents Click="function(s, e) {return ClosePopUp();}" />--%>
                                            </dx:ASPxButton>
                                        </li>
                                    </ul>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>

            </tr>
        </table>
    </asp:Panel>
    <input type="hidden" id="drilldowntype" runat="server" value="0" />
    <input type="hidden" id="isPreview" runat="server" value="false" />
    <input type="hidden" id="paramValue" runat="server" value="false" />
    <div id="parameterDiv" runat="server" class="parameter-div" style="display: none;">
        <asp:Panel ID="parameterPnl" runat="server" class="parameter1-div"></asp:Panel>
    </div>

</div>

<%--Popup to show Missing columns from Imported script for particular Modules--%>
<dx:ASPxPopupControl ID="missingColumnsContainer" runat="server" AllowDragging="false" ClientInstanceName="missingColumnsContainer" CloseAction="CloseButton" ShowCloseButton="true"
    ShowFooter="false" ShowHeader="true" HeaderText="Columns Missing in Request Lists" PopupVerticalAlign="Middle" PopupHorizontalAlign="WindowCenter" EnableViewState="false" EnableHierarchyRecreation="True"
    CssClass="context-popup-grid" Width="400" MinHeight="200" MaxHeight="300" >
    <HeaderStyle ForeColor="Red" CssClass="missingColContainer-headerStyle" />
    <%--<ClientSideEvents CloseUp="function(s, e) { ClosePopup(); }" />--%>
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <div runat="server" id="noColFoundContainer" visible="false" class="main-table">
                <p>No missing columns are found.</p>
            </div>
            <div runat="server" id="missingColGridContainer" class="main-table">
                <p>The following columns from the query script are missing in Request Lists. Please add them to Request Lists or <a href="javascript:;" onclick="RemoveMissingFields();" style="color: #0645ad; cursor: pointer;">click here</a> to save your query again.</p>
            </div>
            <dx:ASPxGridView ID="missingColumnsGrid" runat="server" AutoGenerateColumns="false" OnDataBinding="missingColumnsGrid_DataBinding"
                Width="100%" ClientInstanceName="missingColumnsGrid" EnableViewState="false">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="#" Width="30%" Caption="#" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ColumnName" Width="70%" Caption="Columns" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowHeaderFilterButton="false" ShowFooter="false" EnableFilterControlPopupMenuScrolling="false" VerticalScrollBarMode="Auto"
                    VerticalScrollableHeight="300" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsBehavior AllowSort="false" EnableRowHotTrack="false" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
                <Styles>
                    <Header Font-Bold="true"></Header>
                    <AlternatingRow Enabled="true"></AlternatingRow>
                    <Row Wrap="True"></Row>
                </Styles>
            </dx:ASPxGridView>
             
<%--            <div class="proceed-container">
                <asp:Button ID="btnProceed" runat="server" Text="Proceed" OnClick="btnProceed_Click" CssClass="fright button-bg" />
            </div>--%>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    

    function ShowCustomUrl(drilldown) {
        $("#spancustomUrl").hide();
        $("#spanQuery").hide();
        $("[Id$='tabMenu_T8']").show();
        $("#spandrilldown").show();
        switch ($(drilldown).val()) {
            case "0":
                $("#spandrilldown").hide();
                $("[Id$='tabMenu_T8']").hide();
            case "1":

                break;
            case "2":
                $("#spanQuery").show();
                $("#spandrilldown").hide();
                $("[Id$='tabMenu_T8']").hide();
                break;
            case "3":
                $("#spancustomUrl").show();
                $("#spandrilldown").hide();
                $("[Id$='tabMenu_T8']").hide();
                break;
            default:
                break;
        }



    }
    function ShowDesignControls(objdesign) {
        var qficon = $("#qficon").show();
        var qfborder = $("#qfborder").show();
        var pnldesign = parseInt($(objdesign).val());
        switch (pnldesign) {
            case 1:
                $(qficon).hide();
                $(qfborder).hide();
                break;
            case 2:
                $(qfborder).hide();
                break;
            default:
                break;
        }
    }



    function ShowQueryTypeControls(obj) {
        var queryType = parseInt($(obj).val());
        var qftitle = $("#qftitle").show();
        var qfquerytype = $("#qfquerytype").show();
        var qfbgImage = $("#qfbgImage").show();
        var qfsize = $("#qfsize").show();
        var qfresultPnldesign = $("#qfresultPnldesign").show();
        var qficon = $("#qficon").show();
        var qficonposition = $("#qficonposition").show();
        var qfbgcolor = $("#qfbgcolor").show();
        var qfborder = $("#qfborder").show();
        var qfrowcolor = $("#qfrowcolor").show();
        var qfrowaltercolor = $("#qfrowaltercolor").show();
        var tdformatnumber = $("#tdformatnumber").show();
        var qftitleposition = $("#qftitleposition").show();
        var qflabel = $("#qflabel").show();
        var qflabelfontstyle = $("#qflabelfontstyle").show();
        var qfheadercolor = $("#qfheadercolor").show();
        var qftitlefontstyle = $("#qftitlefontstyle").show();
        var spanborderwidth = $("#spanborderwidth").show();
        var qfdrilldowntype = $("#qfdrilldowntype").show();
        var qftextalign = $("#qftextalign").show();
        //$("[Id$='tabMenu_T8']").show();
        if ($("#<%=drilldowntype.ClientID%>").val() != 1) {
            $("[Id$='tabMenu_T8']").hide();
        }
        switch (queryType) {
            case 1:
                $(qfresultPnldesign).css("display", "none");
                $(qficon).css("display", "none");
                $(qficonposition).css("display", "none");
                $(qfbgcolor).css("display", "none");
                $(qfborder).css("display", "none");
                $(qfrowcolor).css("display", "none");
                $(qfrowaltercolor).css("display", "none");
                $(tdformatnumber).css("display", "none");
                $(qftitleposition).css("display", "none");
                $(qflabel).css("display", "none");
                $(qflabelfontstyle).css("display", "none");
                $(qfbgImage).css("display", "none");
                $(qfheadercolor).css("display", "none");
                var dripdown = $("#<%=ddlDrillDown.ClientID%>");
                ShowCustomUrl(dripdown);
                break;
            case 2:
                $(qfrowcolor).css("display", "none");
                $(qfheadercolor).css("display", "none");
                $(qfrowaltercolor).css("display", "none");
                var resultpnldesign = $("#<%= ddlResultPnlDesign.ClientID%>");
                ShowDesignControls(resultpnldesign);
                var dropdown = $("#<%=ddlDrillDown.ClientID%>");
                ShowCustomUrl(dropdown);
                break;
            case 3:
            case 4:
            case 5:
                $(qftitle).css("display", "none");
                $(qftitlefontstyle).css("display", "none");
                $(qflabel).css("display", "none");
                $(qflabelfontstyle).css("display", "none");
                $(qfsize).css("display", "none");
                $(qfresultPnldesign).css("display", "none");
                $(qficon).css("display", "none");
                $(qficonposition).css("display", "none");
                $(qfbgcolor).css("display", "none");
                $(qfbgImage).css("display", "none");
                $(spanborderwidth).css("display", "none");
                $(qfdrilldowntype).css("display", "none");
                $(qftextalign).css("display", "none");
                break;
            default:

        }
    }

    function Preview(x, y, val) {
        var html = $('.parameter-div').html();
        var height = $('.parameter-div').height() + 5;
        if (val && '<%=whereClauses.Count%>' != '0') {

            $('#<%=hdnPreviewClick.ClientID%>').val(x + ':' + y);
            __doPostBack();
        }
        else {
            $('#<%=hdnPreviewClick.ClientID%>').val('');
            //if (html != '') {
            if ($('.parameter1-div').html().trim() != '') {
                $(".parameter-div").offset({ left: x, top: y - height })
                $(".parameter-div").show();
            }
            else {
                //$("#<%= isPreview.ClientID %>").val('true');
                parameterSubmit('');
            }
        }
    }
    function paramButtonClick() {

        var isvalue = true;
        var numParam = $(".param-value").length;
        var paramValue = new Array(numParam);
        var index = 0;

        $(".param-value").each(function () {
            var paramtype = $(this).attr("paramtype");
            if (paramtype == "user" || paramtype == "group" || paramtype == "usergroup") {

                if ($.trim($(this).find("div.ms-inputuserfield").text()) != "") {
                    var spans = $(this).find("span table table span");
                    $(spans).each(function () {

                        //ms-inputuserfield
                        var peoplePicker = $(this);
                        peoplepickerValue = peoplePicker.html().replace("<br>", "");

                        if (peoplepickerValue && peoplepickerValue != "") {
                            paramValue[index] = peoplepickerValue;
                            return;
                        }
                    });
                }

                if ($(this).parent().find('span').hasClass("mandatory")) {
                    if (paramValue[index] == "") {
                        isvalue = false;
                        return false;
                    }
                }

            }
            else if (paramtype == "date") {
                var input = $(this).find("input");
                paramValue[index] = $(input).val();
            }
            else {
                if ($(this).find("select").length > 0) {
                    var input = $(this).find("select").val();
                    if (input != null) {
                        paramValue[index] = input;
                    }
                }
                else {
                    var input = $(this).find("input");
                    if (input != null && input.length > 0) {
                        paramValue[index] = $(input).val();
                    }
                }
            }

            if ($(this).parent().find('span').hasClass("mandatory")) {
                if (paramValue[index] == "") {
                    isvalue = false;
                    return false;
                }
            }

            index++;

        });

        if (isvalue == true) {
            var whereClause = "";
            for (var i = 0; i < paramValue.length; i++) {
                if (paramValue[i] == null || paramValue[i] == undefined)
                    paramValue[i] = "";
                if (i == 0)
                    whereClause = paramValue[i];
                else
                    whereClause += "," + paramValue[i];
                $("#<%= paramValue.ClientID %>").val(whereClause);
            }

            parameterSubmit($("#<%= paramValue.ClientID %>").val());
        }
        return false;
    }

    function parameterSubmit(whereFilter) {
        //var isPreview = $("#<%= isPreview.ClientID %>").val();
        //if (isPreview == 'true') {
        var control = 'querywizardpreview';
        var formattedview = false;
        if ($("#<%=chkIsFormattedPreview.ClientID %>").prop("checked")) {
            formattedview = true;
        }
        var dashboardId = $("#<%=hDashboardId.ClientID %>").val();

        var params = 'control=' + control + '&ItemId=' + dashboardId + '&whereFilter=' + whereFilter + '&formattedview=' + formattedview;
        if (whereFilter == "false")
            params = 'control=' + control + '&ItemId=' + dashboardId + '&formattedview=' + formattedview;
        var title = 'Query Wizard - ' + $('#<%= hdnTitle.ClientID %>').val();

        var url = '<%= uGovernIT.Utility.UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/uGovernITConfiguration.aspx")%>';
        window.parent.UgitOpenPopupDialog(url, params, title, 90, 80);
        //}
    }

    function validateWhere(button) {

        var prefix = $(button).attr('id').substring(0, $(button).attr('id').indexOf('btnUpdate'));
        var txt = '#' + prefix + 'txtValue';
        var chk = '#' + prefix + 'chkValue';
        var dtc = '#' + prefix + 'dtcValue';
        var user = '#' + prefix + 'ppeValue';
        var opertype = '#' + prefix + 'drpRelOpt';
        var valtype = '#' + prefix + 'drpValuetype';
        var drpcolumn = '#' + prefix + 'drpColumn';

        if (prefix.indexOf('ctrl0') != -1) {
            //return true;
        }
        else {
            if ($(opertype).val() == 'None') {
                $('#lbl_RelOpt').each(function () { $(this).show(); });
                return false;
            }
            else {
                $('#lbl_RelOpt').each(function () { $(this).hide(); });
            }
        }
        var typeval = $(valtype).val();
        var selectedcolumn = $(drpcolumn).val();
        var datatype = selectedcolumn.substring(selectedcolumn.indexOf('(') + 1, selectedcolumn.indexOf(')'));
        datatype = datatype.replace('System.', '');
        switch (datatype) {
            case 'DateTime':
                if (typeval == 'Constant') {
                    var inputdate = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDate';
                    if ($(inputdate).val() == '') {
                        $('#lbl_Value').each(function () { $(this).show(); });
                        return false;
                    }
                    else {
                        $('#lbl_Value').hide();
                    }
                }
                else
                    if ($(txt).val() == '') {
                        $('#lbl_Value').each(function () { $(this).show(); });
                        return false;
                    }
                    else {
                        $('#lbl_Value').hide();
                    }
                break;
            case 'Boolean':
                if (typeval != 'Constant') {
                    if ($(txt).val() == '') {
                        $('#lbl_Value').each(function () { $(this).show(); });
                        return false;
                    }
                    else {
                        $('#lbl_Value').hide();
                    }
                }
                break;
            case 'User':
                if (typeval == 'Constant') {
                    var userval = user + '_upLevelDiv';
                    if ($(userval).html() == '') {
                        return false;
                    }
                    else {
                        $('#lbl_Value').hide();
                    }
                }
                else
                    if ($(txt).val() == '') {
                        $('#lbl_Value').each(function () { $(this).show(); });
                        return false;
                    } else {
                        $('#lbl_Value').hide();
                    }
                break;
            default:
                if ($(txt).val() == '') {
                    $('#lbl_Value').each(function () { $(this).show(); });
                    return false;
                } else {
                    $('#lbl_Value').each(function () { $(this).hide(); });
                }
                break;
        }
        return true;

    }

    $(document).ready(function () {
        var querytypedropdown = $(".querytype-dropdown");
        ShowQueryTypeControls(querytypedropdown);
        $(querytypedropdown).change(function () { ShowQueryTypeControls(this) });

        $('.where-update').click(function () {
            return validateWhere(this);
        });
        $('.close-button').click(function () {
            $(".parameter-div").hide();
        });
        $("#<%=aPreview.ClientID%>").click(function (e) {
            Preview(e.pageX, e.pageY, true);
        });

        $('.sequenceddl').bind('change', function () {
            Reorder(this, $(this).attr('previous'), $('.sequenceddl').length);
        });
        $('.sequenceddl').each(function () {
            $(this).attr('previous', $(this).val());
        });

        $('.drop-column').each(function () {
            $(this).bind('change', function () {
                if (this != null) {
                    var prefix = this.id.substring(0, this.id.indexOf('drpColumn'));
                    var txt = '#' + prefix + 'txtValue';
                    var chk = '#' + prefix + 'chkValue';
                    var dtc = '#span_datetime';
                    var user = '#span_user';
                    var valuetype = '#' + prefix + 'drpValuetype';
                    change_column(this, valuetype, txt, chk, dtc, user);
                }
            });
        });
        $('.drop-valuetype').each(function () {
            $(this).bind('change', function () {
                if (this != null) {
                    var prefix = this.id.substring(0, this.id.indexOf('drpValuetype'));
                    var txt = '#' + prefix + 'txtValue';
                    var chk = '#' + prefix + 'chkValue';
                    var dtc = '#span_datetime';
                    var user = '#span_user';
                    var column = '#' + prefix + 'drpColumn';
                    change_column(column, this, txt, chk, dtc, user);
                }
            });
        });

        $('#<%=saveTemplate.ClientID%>').on('click', function (e) {
            $('#trtemplateName').show();
            $('#trtemplates').hide();
        });

        $('#<%=overrideTemplate.ClientID%>').on('click', function (e) {
            $('#trtemplateName').hide();
            $('#trtemplates').show();
        });



    });

    $(function () {


        if ($('#<%=hdnPreviewClick.ClientID%>').val() != '') {
            var val = $('#<%=hdnPreviewClick.ClientID%>').val();
            Preview(val.split(':')[0], val.split(':')[1], false);
        }

        var drpcolumn = $('.drop-column');
        if (drpcolumn.length > 0) {
            var prefix = $(drpcolumn).attr('id').substring(0, $(drpcolumn).attr('id').indexOf('drpColumn'));
            var txt = '#' + prefix + 'txtValue';
            var chk = '#' + prefix + 'chkValue';
            var dtc = '#span_datetime';
            var user = '#span_user';
            var valuetype = '#' + prefix + 'drpValuetype';
            change_column(drpcolumn, valuetype, txt, chk, dtc, user);
        }
    });

    function change_column(drpcolumn, valuetype, txt, chk, dtc, user) {
        var formstate = $("#<%=hdnFormState.ClientID %>").val();
        if (formstate == 'WhereClause') {
            $(txt).hide();
            $(chk).hide();
            $(dtc).hide();
            $(user).hide();
            var typeval = $(valuetype).val();
            var selectedcolumn = $(drpcolumn).val();
            var datatype = selectedcolumn.substring(selectedcolumn.indexOf('(') + 1, selectedcolumn.indexOf(')'));
            datatype = datatype.replace('System.', '');
            switch (datatype) {
                case 'DateTime':
                    if (typeval == 'Constant') {
                        var inputdate = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDate';
                        var inputmin = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDateMinutes';
                        var inputhours = '#' + $(drpcolumn).attr('id').replace('drpColumn', 'dtcValue') + '_dtcValueDateHours';
                        $(inputmin).hide();
                        $(inputhours).hide();
                        $(inputdate).css('width', '130px');
                        $(dtc).show();
                    }
                    else
                        $(txt).show();
                    break;
                case 'Boolean':
                    if (typeval == 'Constant')
                        $(chk).show();
                    else
                        $(txt).show();
                    break;
                case 'User':
                    if (typeval == 'Constant')
                        $(user).show();
                    else
                        $(txt).show();
                    break;
                default:
                    $(txt).show();
                    break;
            }

            if (typeval == 'Constant') {
                $('.drop-ParameterType').hide();
            }
            else {
                $('.drop-ParameterType').show();
            }
        }
    }

    function Reorder(eSelect, iCurrentField, numSelects) {
        var eForm = eSelect.form;
        var iNewOrder = eSelect.selectedIndex + 1;
        var iPrevOrder;
        var positions = new Array(numSelects);
        var ix;
        for (ix = 0; ix < numSelects; ix++) {
            positions[ix] = 0;
        }
        $('.sequenceddl').each(function () {
            positions[this.selectedIndex] = 1;
        });
        for (ix = 0; ix < numSelects; ix++) {
            if (positions[ix] == 0) {
                iPrevOrder = ix + 1;
                break;
            }
        }
        if (iNewOrder != iPrevOrder) {
            var iInc = iNewOrder > iPrevOrder ? -1 : 1
            var iMin = Math.min(iNewOrder, iPrevOrder);
            var iMax = Math.max(iNewOrder, iPrevOrder);

            $('.sequenceddl').each(function () {
                if (eSelect.id != this.id) {
                    if (this.selectedIndex + 1 >= iMin && this.selectedIndex + 1 <= iMax) {
                        this.selectedIndex += iInc;
                    }
                }
            });
        }
    }

    function RemoveMissingFields() {
        missingColumnsContainer.Hide();
        $(".saveUpdateQuery").get(0).click();
    }

</script>

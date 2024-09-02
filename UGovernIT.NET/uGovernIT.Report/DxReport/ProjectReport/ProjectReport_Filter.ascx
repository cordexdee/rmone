
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectReport_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.ProjectReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<style type="text/css">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
    }

    .ms-formbody label {
        margin-bottom: 0;
    }

    .ms-formbody input {
        margin-left: 3px;
    }

    .ms-formlabel {
        text-align: right;
        width: 100px;
        vertical-align: middle;
        font-weight: normal;
        border-top: 1px solid #A5A5A5;
        padding-right: 6px;
    }
    /*.ms-component-formlabel {
        border-top: 1px solid #A5A5A5;
        padding-top: 2px;
        padding-right: 8px;
        padding-bottom: 2px;
        color: #000;
        text-align: right;
        width: 100px;
        vertical-align: middle;
        font-weight: normal;
    }*/
    .full_width {
        width: 100%;
    }

    .main_heading {
        background: none repeat scroll 0 0 #BED0E5;
        float: left;
        font-size: 20px;
        font-weight: bold;
        margin-bottom: 5px;
        padding: 0;
        text-align: center;
        width: 100%;
    }

    .table_lst {
        width: 100%;
    }

    .button_lst {
        width: 70px;
        margin: 5px;
    }

    .listbox_lst {
        width: 150px;
        height: 100px;
    }

    .td_lst {
        /*width: 33%;*/
        padding: 2px 5px;
        vertical-align: middle;
        text-align: center;
    }

    .error {
        color: red;
    }

   .rbutton {
        float: left;
        width: 100px;
        margin-top: 5px;
    }
    .rbutton input[type="radio"]{
        margin: 0 2px;
        float: left;
    }

    .dxe .dxeTAR .dxichTextCellSys label {
        margin-left: 5px;
    }

    .rbutton label {
        vertical-align: middle;
        margin-top: -1px;
    }

    .button-refresh {
        background: url('/Content/images/refresh-icon.png') no-repeat;
        width: 20px;
        height: 20px;
    }

    .rblistPriority td {
        padding: 0 !important;
    }

    .rblistPriority .dxe > table > tbody > tr > td {
        width: 100px !important;
    }
    .top_container {
        padding:8px;
        overflow:auto;
    }
    legend{
        width: auto;
        border: none;
        font-size: 12px;
        font-weight: 600;
    }
    .first_tier_nav{
        padding-top: 15px;
        padding-bottom: 15px;
    }
    .primary-blueBtn {
        background: none;
        border: none;
    }

    .primary-blueBtn .dxb {
        background: #4A6EE2;
        color: #FFF;
        border: 1px solid #4A6EE2 !important;
        border-radius: 4px;
        padding: 5px 13px 5px 13px !important;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }
</style>

<script type="text/javascript">
    function UpdateButtonStatePriority() {
        btnMoveAllItemsToRightPriority.SetEnabled(lstPriorityLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftPriority.SetEnabled(lstPriorityRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightPriority.SetEnabled(lstPriorityLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftPriority.SetEnabled(lstPriorityRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateProgress() {
        btnMoveAllItemsToRightProgress.SetEnabled(lstProgressLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProgress.SetEnabled(lstProgressRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProgress.SetEnabled(lstProgressLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProgress.SetEnabled(lstProgressRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateProj() {
        btnMoveAllItemsToRightProj.SetEnabled(lstProjectsLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProj.SetEnabled(lstProjectsRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProj.SetEnabled(lstProjectsLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProj.SetEnabled(lstProjectsRHS.GetSelectedItems().length > 0);
    }

    

    var lastCategory = null;
    var lastSubCategory = null;
    function OnCategoryChanged(cmbCategory) {
        if (cbSubCategory.InCallback())
            lastCategory = cmbCategory.GetValue().toString();
        else
            cbSubCategory.PerformCallback(cmbCategory.GetValue().toString());
    }

    function OnSubCategoryChanged(cmbSubCategory) {
        if (cbRequesttype.InCallback())
            lastCategory = cmbSubCategory.GetValue().toString();
        else
            cbRequesttype.PerformCallback(cbCategory.GetValue().toString() + "|" + cmbSubCategory.GetValue().toString());
    }

    function OnDepartmentChanged(cmbDepartment) {
        if (cbFunctionalarea.InCallback())
            lastCategory = cmbDepartment.GetValue().toString();
        else
            cbFunctionalarea.PerformCallback(cmbDepartment.GetValue().toString());
    }

    function onEndCallBackSubCat(s, e) {
        if (lastCategory) {
            cbSubCategory.PerformCallback(lastCategory);
            cbRequesttype.PerformCallback(lastCategory + "|0");
            lastCategory = null;
        }
    }

    function onEndCallBackRequestType(s, e) {
        if (lastCategory && lastSubCategory) {
            cbRequesttype.PerformCallback(lastCategory + "|" + lastSubCategory);
            lastCategory = null;
            lastSubCategory = null;
        }
    }
</script>
<asp:ScriptManager ID="scrpit1" runat="server"></asp:ScriptManager>
<asp:Panel runat="server" ID="pnlFilter">
    <asp:UpdatePanel ID="upPanel" runat="server">
        <ContentTemplate>
            <div>
                <asp:Label ID="lblMsg" Text="" runat="server" CssClass="error" />
            </div>
            <table style="margin: 0px auto;">
                <tr>
                    <td style="vertical-align: top; width: 800px;">
                        <div style="width: 100%; float: left;">

                            <fieldset>
                                <legend>Select Project(s):</legend>
                                <table style="width: 100%; height: 445px">
                                    <tr>
                                        <td class="ms-formlabel">Project Status</td>
                                        <td class="ms-formbody">
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbOpen" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" Checked="true" />
                                                <label>Open</label>
                                            </span>
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbClose" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" />
                                                <label>Closed</label>
                                            </span>
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbAll" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" />
                                                <label>All</label>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td class="ms-formlabel">Priority</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstPriorityLHS" runat="server" ValueType="System.String" CssClass="list_box"
                                                                Width="265px" Height="85px" SelectionMode="CheckColumn" ClientInstanceName="lstPriorityLHS"
                                                                Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStatePriority(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToRightPriority" runat="server" AutoPostBack="True" Text="Add >"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightPriority_Click"
                                                                    ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightPriority" CssClass="button_list" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveAllItemsToRightPriority" runat="server" AutoPostBack="True" Text="Add All >>"
                                                                    Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightPriority_Click"
                                                                    ClientInstanceName="btnMoveAllItemsToRightPriority" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToLeftPriority" runat="server" AutoPostBack="True" Text="< Remove"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftPriority_Click"
                                                                    ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftPriority" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:ASPxButton ID="btnMoveAllItemsToLeftPriority" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftPriority_Click"
                                                                    ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftPriority" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstPriorityRHS" runat="server" ValueType="System.String" ClientInstanceName="lstPriorityRHS"
                                                                Width="265px" Height="85px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStatePriority(); }" />
                                                            </dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td class="ms-formlabel">Progress</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProgressLHS" runat="server" ValueType="System.String" ClientInstanceName="lstProgressLHS"
                                                                Width="265px" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProgress(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToRightProgress" runat="server" AutoPostBack="True" Text="Add >"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightProgress_Click"
                                                                    ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightProgress" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveAllItemsToRightProgress" runat="server" AutoPostBack="True" Text="Add All >>"
                                                                    Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightProgress_Click"
                                                                    ClientInstanceName="btnMoveAllItemsToRightProgress" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToLeftProgress" runat="server" AutoPostBack="True" Text="< Remove"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftProgress_Click"
                                                                    ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftProgress" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:ASPxButton ID="btnMoveAllItemsToLeftProgress" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftProgress_Click"
                                                                    ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftProgress" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProgressRHS" runat="server" ValueType="System.String" ClientInstanceName="lstProgressRHS"
                                                                Width="265px" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProgress(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Priority</td>
                                        <td class="ms-formbody">
                                            <dx:ASPxRadioButtonList CssClass="rblistPriority" ID="rblistPriority" runat="server" ValueType="System.String"
                                                RepeatLayout="Table" RepeatColumns="5" RepeatDirection="Horizontal">
                                                <Border BorderStyle="None" BorderWidth="0" BorderColor="Transparent" />
                                            </dx:ASPxRadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel"><%=departmentLabel %> 
                                            <br />
                                            Functional Area</td>
                                        <td class="ms-formbody">
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbDepartment" runat="server" ValueType="System.Int32">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ OnDepartmentChanged(s);}" />
                                                </dx:ASPxComboBox>
                                            </span>
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbFunctionalarea" runat="server" ValueType="System.Int32"
                                                    OnCallback="cbFunctionalarea_Callback" ClientInstanceName="cbFunctionalarea">
                                                </dx:ASPxComboBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Project Type</td>
                                        <td class="ms-formbody">
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbCategory" runat="server" ValueType="System.String"
                                                    DropDownStyle="DropDownList" ClientInstanceName="cbCategory">
                                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ OnCategoryChanged(s);}" />
                                                </dx:ASPxComboBox>
                                            </span>
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbSubCategory" runat="server" ValueType="System.String"
                                                    DropDownStyle="DropDownList" ClientInstanceName="cbSubCategory" OnCallback="cbSubCategory_Callback">
                                                    <ClientSideEvents EndCallback="onEndCallBackSubCat" SelectedIndexChanged="function(s,e){ OnSubCategoryChanged(s);}" />
                                                </dx:ASPxComboBox>
                                            </span>
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbRequesttype" runat="server" ValueType="System.Int32"
                                                    DropDownStyle="DropDownList" ClientInstanceName="cbRequesttype" OnCallback="cbRequesttype_Callback">
                                                    <ClientSideEvents EndCallback="onEndCallBackRequestType" />
                                                </dx:ASPxComboBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Business Initiative /
                                            <br />
                                            Project Class</td>
                                        <td class="ms-formbody">
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbProjectInitiative" runat="server" ValueType="System.Int32"
                                                    DropDownStyle="DropDownList" ClientInstanceName="cbProjectInitiative">
                                                </dx:ASPxComboBox>
                                            </span>
                                            <span style="display: inline-block">
                                                <dx:ASPxComboBox ID="cbProjectClass" runat="server" ValueType="System.Int32"
                                                    DropDownStyle="DropDownList" ClientInstanceName="cbProjectClass">
                                                </dx:ASPxComboBox>
                                            </span>
                                            <span style="display: inline-block; float: right;">
                                                <dx:ASPxButton ID="btRefresh" runat="server" RenderMode="Link" OnClick="btRefresh_Click"
                                                    UseSubmitBehavior="false" AutoPostBack="false" Text="Apply/Refresh" RightToLeft="True">
                                                    <Image>
                                                        <SpriteProperties CssClass="button-refresh" />
                                                    </Image>
                                                    <ClientSideEvents Click="function(s,e){ lstProjectsLHS.PerformCallback();}" />
                                                </dx:ASPxButton>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Projects</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjectsLHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstProjectsLHS"
                                                                Width="265px" Height="230px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt"
                                                                >
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProj(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToRightProj" runat="server" AutoPostBack="True" Text="Add >"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightProj_Click"
                                                                    ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightProj" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveAllItemsToRightProj" runat="server" AutoPostBack="True" Text="Add All >>"
                                                                    Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightProj_Click"
                                                                    ClientInstanceName="btnMoveAllItemsToRightProj" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToLeftProj" runat="server" AutoPostBack="True" Text="< Remove"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftProj_Click"
                                                                    ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftProj" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:ASPxButton ID="btnMoveAllItemsToLeftProj" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftProj_Click"
                                                                    ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftProj" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjectsRHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstProjectsRHS"
                                                                Width="265px" Height="230px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProj(); }" />
                                                            </dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>

                                </table>
                            </fieldset>
                        </div>
                        <div style="width: 100%; float: left;">

                            <fieldset>
                                <legend style="font-weight:bold;">Sort Order:</legend>
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="ms-formlabel">Project Name</td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlProjectSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProjectSortOrder_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ms-formlabel">Priority</td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlPrioritySortOrder" OnSelectedIndexChanged="ddlPrioritySortOrder_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                         <td class="ms-formlabel">Rank</td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlProjectRank" OnSelectedIndexChanged="ddlProjectRank_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>


                                        <td class="ms-formlabel">Progress</td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlProgressSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlProgressSortOrder_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="ms-formlabel">Target Date</td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlTargetDateSortOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTargetDateSortOrder_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4" ></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Initiative
                                            
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:DropDownList ID="ddlBusinessInitiative" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBusinessInitiative_SelectedIndexChanged">
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                    <td style="vertical-align: top; width: 400px;">
                        <div style="width: 100%; float: left;">
                            <fieldset>
                                <legend>Project Fields</legend>
                                <table style="width: 100%; height: 500px;">
                                    <tr>
                                        <td class="ms-formlabel">Project Name
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" Enabled="false" ID="chkProjectName"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Priority
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPriority"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td class="ms-formlabel">Status
                                        </td>
                                        <td class="ms-formbody">
                                            <div style="float: left;">
                                                <asp:CheckBox runat="server" ID="chkStatus"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Progress
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkProgress"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Description
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkDescription"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Target Date
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkTargetDate"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Project Managers
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkProjectManager"></asp:CheckBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ms-formlabel">Project Type
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkProjectType"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">% Complete
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPercentComp"></asp:CheckBox>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="ms-formlabel">Status
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkProStatus"></asp:CheckBox>
                                            <div style="float: right;">
                                                <asp:CheckBox runat="server" ID="chkPlainText" TextAlign="Left" Text="Plain Text:"></asp:CheckBox>
                                                <asp:CheckBox runat="server" ID="chkLatestOnly" TextAlign="Left" Text="Latest Only:"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Accomplishments
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkAccomplishments"></asp:CheckBox>
                                            <div style="float: right;">
                                                <asp:CheckBox runat="server" ID="chkAccDesc" TextAlign="Left" Text="Show Description:"></asp:CheckBox>

                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Planned Items
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPlan"></asp:CheckBox>
                                            <div style="float: right;">
                                                <asp:CheckBox runat="server" ID="chkPlanDesc" TextAlign="Left" Text="Show Description:"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Risks
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkRisk"></asp:CheckBox>
                                            <div style="float: right;">
                                                <asp:CheckBox runat="server" ID="chkRiskDesc" TextAlign="Left" Text="Show Description:"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Issues
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkIssues"></asp:CheckBox>
                                            <div style="float: right;">
                                                <asp:CheckBox runat="server" ID="chkIssuesDesc" TextAlign="Left" Text="Show Description:"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Monitors
                                        </td>
                                        <td class="ms-formbody">
                                            <div style="float:left;">
                                                <asp:CheckBox runat="server" ID="chkMonitors"></asp:CheckBox>
                                            </div>
                                              <div style="float: right; width:132px;">
                                                <span>Traffic Lights</span>
                                                <asp:CheckBox ID="chkTrafficLight" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table style="width: 1200px; margin: 0px auto">
        <tr>
            <td>
                <div class="first_tier_nav" style="padding-top: 0px !important;">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <ul class="d-flex flex-wrap my-3" style="list-style:none; ">
                                    <li runat="server" id="liBuild" class="mr-3">
                                        <dx:ASPxButton ID="lnkBuild" cssClass="primary-blueBtn" runat="server" Text="Build Report" OnClick="lnkBuild_Click">
                                             <%--<Image Url="/content/images/GanttChart1.png"></Image>
                                            <ClientSideEvents Click="loadingpanel.Show();" />--%>
                                        </dx:ASPxButton>
                                        <%--<asp:LinkButton runat="server" ID="lnkBuild" Style="color: white; float: right;" Text="Build Report" OnClientClick="loadingpanel.Show();" OnClick="lnkBuild_Click"
                                            CssClass="ganttImg" />--%>
                                    </li>
                                    <li runat="server" id="liCancel">
                                        <dx:ASPxButton ID="lnkCancel" CssClass="secondary-cancelBtn" runat="server" Text="Cancel" OnClick="lnkCancel_Click">
                                             <%--<Image Url="/Content/images/cancelwhite.png"></Image>--%>
                                        </dx:ASPxButton>
                                        <%--<asp:LinkButton ID="lnkCancel" runat="server" Style="color: white; float: right;" Visible="true" Text="Cancel" CssClass="cancelwhite" OnClick="lnkCancel_Click" />--%>
                                    </li>
                                </ul>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <dx:ASPxLoadingPanel ID="loadingpanel" runat="server" ClientInstanceName="loadingpanel" Modal="true"></dx:ASPxLoadingPanel>
</asp:Panel>


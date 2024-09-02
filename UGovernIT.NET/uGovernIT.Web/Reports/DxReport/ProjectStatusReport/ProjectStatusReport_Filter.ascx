<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectStatusReport_Filter.ascx.cs" Inherits="uGovernIT.DxReport.ProjectStatusReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<style type="text/css">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        padding: 6px 7px 6px 0px;
        width: 100px;
        vertical-align: middle;
        font-weight: normal;
    }

    .ms-component-formlabel {
        border-top: 1px solid #A5A5A5;
        padding-top: 3px;
        padding-right: 8px;
        padding-bottom: 6px;
        color: #000;
        text-align: right;
        width: 210px;
        vertical-align: middle;
        font-weight: normal;
    }

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
        padding: 5px 5px;
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
        margin: 0 6px;
        float: left;
    }

    .rbutton label {
        vertical-align: middle;
        margin-top: -2px;
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

    fieldset {
        display: block;
        margin-inline-start: 2px;
        margin-inline-end: 2px;
        padding-block-start: 0.35em;
        padding-inline-start: 0.75em;
        padding-inline-end: 0.75em;
        padding-block-end: 0.625em;
        min-inline-size: min-content;
        border-width: 2px;
        border-style: groove;
        border-color: threedface;
        border-image: initial;
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
        padding: 15px 2px;
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
    .secondary-cancelBtn {
        background: none;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        margin-right: 5px;
        margin-top: 1px;
        padding: 3px 3px 3px 4px;
    }

    .secondary-cancelBtn .dxb {
        padding: 4px 10px 3px;
        color: #4A6EE2;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {
        $(".chkShowMilestone").bind("change", function () {
          
            if (!$(".chkShowMilestone").is(":checked")) {
                $(".chkAllMilestone input").attr('checked', false);

            }
        })
    })
    function UpdateButtonStateProjectType() {
        btnMoveAllItemsToRightProjType.SetEnabled(lstProjTypeLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProjType.SetEnabled(lstProjTypeRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProjType.SetEnabled(lstProjTypeLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProjType.SetEnabled(lstProjTypeRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateFuncArea() {
        btnMoveAllItemsToRightFuncArea.SetEnabled(lstFunctionAreaLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftFuncArea.SetEnabled(lstFunctionAreaRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightFuncArea.SetEnabled(lstFunctionAreaLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftFuncArea.SetEnabled(lstFunctionAreaRHS.GetSelectedItems().length > 0);
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
            <table runat ="server" id="tblprostatusreport">
                <tr>
                    <td style="vertical-align: top; width: 600px;"  runat="server" id="projectside">
                        <div style="width: 100%; float: left;">
                            <fieldset>
                                <legend>Select Project(s):</legend>
                                <table style="width: 100%; height: 500px">
                                    <tr style="display: none;">
                                        <td class="ms-formlabel">Current Date Range

                                        </td>
                                        <td class="ms-formbody">
                                            <table>
                                                <tr>
                                                    <td>From
                                                    </td>
                                                    <td>
                                                           <dx:ASPxDateEdit ID="dtcfrom" runat="server"></dx:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>To
                                                    </td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="dtcto" runat="server"></dx:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
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
                                        <td class="ms-formlabel"><%= departmentLabel %> /
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
                                                <dx:ASPxButton ID="btRefresh" runat="server" RenderMode="Link"
                                                    UseSubmitBehavior="false" AutoPostBack="false" Text="Apply/Refresh" RightToLeft="True">
                                                    <Image>
                                                        <SpriteProperties CssClass="button-refresh" />
                                                    </Image>
                                                    <ClientSideEvents Click="function(s,e){ lstProjectsLHS.PerformCallback();}" />
                                                </dx:ASPxButton>
                                            </span>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                        <td class="ms-formlabel">Project Type</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjTypeLHS" runat="server" ValueType="System.Int32" CssClass="list_box"
                                                                Width="265px" Height="135px" SelectionMode="CheckColumn" ClientInstanceName="lstProjTypeLHS"
                                                                 Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True"  />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProjectType(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToRightProjType" runat="server" AutoPostBack="True" Text="Add >"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightProjType_Click"
                                                                    ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightProjType" CssClass="button_list" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveAllItemsToRightProjType" runat="server" AutoPostBack="True" Text="Add All >>"
                                                                    Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightProjType_Click"
                                                                    ClientInstanceName="btnMoveAllItemsToRightProjType" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToLeftProjType" runat="server" AutoPostBack="True" Text="< Remove"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftProjType_Click"
                                                                    ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftProjType" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:ASPxButton ID="btnMoveAllItemsToLeftProjType" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftProjType_Click"
                                                                    ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftProjType" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjTypeRHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstProjTypeRHS"  
                                                                Width="265px" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True"  />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateProjectType(); }" />
                                                            </dx:ASPxListBox>

                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Functional Area</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstFunctionAreaLHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstFunctionAreaLHS"
                                                                 Width="265px" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True"  />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateFuncArea(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                        <td class="td_lst">
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToRightFuncArea" runat="server" AutoPostBack="True" Text="Add >"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToRightFuncArea_Click"
                                                                    ToolTip="Add selected items" ClientInstanceName="btnMoveSelectedItemsToRightFuncArea" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveAllItemsToRightFuncArea" runat="server" AutoPostBack="True" Text="Add All >>"
                                                                    Width="110px" Height="23px" ToolTip="Add all items" OnClick="btnMoveAllItemsToRightFuncArea_Click"
                                                                    ClientInstanceName="btnMoveAllItemsToRightFuncArea" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div style="height: 10px"></div>
                                                            <div>
                                                                <dx:ASPxButton ID="btnMoveSelectedItemsToLeftFuncArea" runat="server" AutoPostBack="True" Text="< Remove"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveSelectedItemsToLeftFuncArea_Click"
                                                                    ToolTip="Remove selected items" ClientInstanceName="btnMoveSelectedItemsToLeftFuncArea" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                            <div class="TopPadding">
                                                                <dx:ASPxButton ID="btnMoveAllItemsToLeftFuncArea" runat="server" AutoPostBack="True" Text="<< Remove All"
                                                                    Width="110px" Height="23px" ClientEnabled="False" OnClick="btnMoveAllItemsToLeftFuncArea_Click"
                                                                    ToolTip="Remove all items" ClientInstanceName="btnMoveAllItemsToLeftFuncArea" Font-Names="Verdana" Font-Size="8pt">
                                                                </dx:ASPxButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstFunctionAreaRHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstFunctionAreaRHS"
                                                                 Width="265px" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True"  />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateFuncArea(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td class="ms-formlabel">Projects</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjectsLHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstProjectsLHS"
                                                                Width="220px" Height="315px" SelectionMode="CheckColumn" OnCallback="lstProjectsLHS_Callback" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
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
                                                                Width="220px" Height="315px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
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
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <div style="float: left; width: 100%;">
                            <fieldset>
                                <legend>Report Components:</legend>
                                <table style="width: 100%; height: 500px;">
                                    <tr>
                                        <td class="ms-component-formlabel">Project Status
                                        </td>
                                        <td class="ms-formbody">
                                            <div style="float: left;">
                                                <asp:CheckBox runat="server" ID="chkStatus"></asp:CheckBox>
                                            </div>
                                            <div style="float: right; width: 132px;">
                                                <span>Traffic Lights</span>
                                                <asp:CheckBox ID="chkTrafficLight" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Project Roles
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkProjectRoles"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Project Description
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkProjectDescription"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Project Budget Summary
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkBudgetSummary"></asp:CheckBox>
                                            <div style="float: right; width: 210px;">
                                                <asp:CheckBox runat="server" ID="chkCalculate" TextAlign="Left"></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Planned vs Actuals By Category
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPlannedvsActualByCategory"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Accomplishments
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkAccomplishments"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Planned Items
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPlan"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Risks
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkRisks" Checked="true"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Issues
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkIssues"></asp:CheckBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="ms-component-formlabel">Decision Log
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkDecisionLog"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Key Deliverables</td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkKeyDeliverables" Text="" GroupName="ProjectTask"></asp:CheckBox></td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Key Receivables</td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkKeyReceivables" Text="" GroupName="ProjectTask"></asp:CheckBox></td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Planned vs Actuals By Budget Item
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPlannedvsActualByBudgetItem"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Planned vs Actuals By Month
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkPlannedvsActualByMonth"></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Key Tasks
                                        </td>
                                       <td class="ms-formbody">
                                            <div style="float: left;">
                                                <asp:CheckBox runat="server" ID="chkShowMilestone" CssClass="chkShowMilestone" Text=""></asp:CheckBox>
                                            </div>
                                            <div style="float: right; width: 132px;">
                                                <span>All Milestones</span>
                                                <asp:CheckBox ID="chkAllMilestone" CssClass="chkAllMilestone" Checked="false" runat="server" />
                                            </div>
                                        </td>

                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">All Tasks
                                        </td>
                                        <td class="ms-formbody">
                                             <div style="float: left;">
                                               <asp:CheckBox runat="server" ID="chkShowAllTasks" Text=""></asp:CheckBox>
                                            </div>
                                            <div style="float: right; width: 132px;">
                                                <span>Open Tasks Only</span>
                                                <asp:CheckBox runat="server" ID="chkShowOpenTaskOnly" Text=""></asp:CheckBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Resource Allocation
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkResourceAllocation" Text=""></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">Summary Gantt Chart
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkMilestone" Text="" GroupName="ProjectGanttChart"></asp:CheckBox>
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
     <table style="width: 100%;">
        <tr>
            <td>
                <div>
                    <div class="first_tier_nav">
                        <dx:ASPxButton ID="lnkSubmit" Text="Build Report" runat="server" OnClick="lnkBuild_Click" CssClass="primary-blueBtn">
                        <%--    <ClientSideEvents Click="loadingpanel.Show();" />--%>
                            <%--<Image Url="/Content/images/GanttChart1.png"></Image>--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" OnClick="lnkCancel_Click" CssClass="primary-blueBtn">
                            <%--<Image Url="/Content/images/cancelwhite.png"></Image>--%>
                        </dx:ASPxButton>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <dx:ASPxLoadingPanel ID="loadingpanel" runat="server" ClientInstanceName="loadingpanel" Modal="true"></dx:ASPxLoadingPanel>
</asp:Panel>


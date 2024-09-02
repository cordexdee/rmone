<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OnePagerReport_SchedulerFilter.ascx.cs" Inherits="uGovernIT.DxReport.OnePagerReport_SchedulerFilter" %>
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
    }

        .rbutton label {
            vertical-align: middle;
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
</style>
<script type="text/javascript">
    function UpdateButtonStateProjectType() {
        btnMoveAllItemsToRightProjType.SetEnabled(lstProjTypeLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProjType.SetEnabled(lstProjTypeRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProjType.SetEnabled(lstProjTypeLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProjType.SetEnabled(lstProjTypeRHS.GetSelectedItems().length > 0);
    }

    function UpdateButtonStateProj() {
        btnMoveAllItemsToRightProj.SetEnabled(lstProjectsLHS.GetItemCount() > 0);
        btnMoveAllItemsToLeftProj.SetEnabled(lstProjectsRHS.GetItemCount() > 0);
        btnMoveSelectedItemsToRightProj.SetEnabled(lstProjectsLHS.GetSelectedItems().length > 0);
        btnMoveSelectedItemsToLeftProj.SetEnabled(lstProjectsRHS.GetSelectedItems().length > 0);
    }
    function OnDepartmentChanged(cmbDepartment) {
        if (cbFunctionalarea.InCallback())
            lastCategory = cmbDepartment.GetValue().toString();
        else
            cbFunctionalarea.PerformCallback(cmbDepartment.GetValue().toString());
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
            <table>
                <tr>
                    <td style="vertical-align: top; width: 500px;">
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
                                                       <%-- <SharePoint:DateTimeControl ID="dtcfrom" runat="server" DateOnly="true" />--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>To
                                                    </td>
                                                    <td>
                                                         <dx:ASPxDateEdit ID="dtcto" runat="server"></dx:ASPxDateEdit>
                                                       <%-- <SharePoint:DateTimeControl ID="dtcto" runat="server" DateOnly="true" />--%>
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
                                                RepeatLayout="Table" RepeatColumns="4" RepeatDirection="Horizontal">
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
                                <dx:ASPxButton ID="lnkSubmit" Text="Submit" runat="server" OnClick="lnkSubmit_Click">
                                    <Image Url="/Content/images/GanttChart1.png"></Image>
                                </dx:ASPxButton>
                                <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" OnClick="lnkCancel_Click">
                                    <Image Url="/Content/images/cancelwhite.png"></Image>
                                </dx:ASPxButton>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>

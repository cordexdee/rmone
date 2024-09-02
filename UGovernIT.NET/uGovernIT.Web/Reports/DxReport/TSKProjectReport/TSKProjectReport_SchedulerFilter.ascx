<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TSKProjectReport_SchedulerFilter.ascx.cs" Inherits="uGovernIT.DxReport.TSKProjectReport_SchedulerFilter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<style>
    body {
        background-color: #fff;
    }
    legend {
        width:auto;
        border:none;
        font-size: 12px;
        font-weight: bold !important;
        margin-bottom: 5px !important;
        padding:0px 4px;
    }
    .ms-formbody {
        padding-left: 10px;
        padding-top: 10px;
    }

    .ms-formlabel {
        text-align: right;
        padding-top: 10px;
    }

    .ms-component-formlabel {
        padding-top: 10px;
        text-align: right;
        vertical-align: middle;
        padding-right: 10px;
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

    .rbutton input[type="radio"] {
        margin-top: 0;
    }

    .rbutton label {
        vertical-align: middle;
        margin-bottom: 0;
    }

    .primary-blueBtn {
        background: none;
        border: none;
    }

    .primary-blueBtn .dxb {
        background: #4A6EE2;
        color: #FFF;
        border: none;
        border-radius: 4px;
        padding: 5px 13px;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .dxbButton_UGITGreenDevEx div.dxb {
	    padding: 6px 14px;
        border: none;
    }

    .secondary-cancelBtn {
        background: none;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        margin-right: 5px;
        padding: 0;
    }

    .dxbButton_UGITNavyBlueDevEx.secondary-cancelBtn .dxb {
        padding: 0px 10px;
        color: #4A6EE2;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .addEditPopup-btnWrap {
        padding-top: 10px;
        text-align: right;
    }

    .filterBlock {
        padding: 10px;
    }
    .pl10 {
        padding-left: 10px;
    }
    .px10 {
        padding-left: 10px;
        padding-right: 10px;
    }
    .pl5 {
        padding-left: 5px;
    }
</style>

<script>
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
</script>
<asp:ScriptManager runat="server" ID="scrptmanager"></asp:ScriptManager>
<asp:Panel runat="server" ID="pnlFilter" CssClass="filterBlock">
    <div>
        <asp:Label ID="lblMsg" Text="" runat="server" CssClass="error" />
    </div>
    <table>
        <tr>
            <td>
                <asp:UpdatePanel ID="upPanelLeft" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%; float: left;">

                            <fieldset>
                                <legend>Select Task List(s):</legend>
                                <table>
                                    <tr>
                                        <td class="ms-formlabel">Task List Status</td>
                                        <td class="ms-formbody">
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbOpen" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" />
                                                <label>Open</label>
                                            </span>
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbClose" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" />
                                                <label>Closed</label>
                                            </span>
                                            <span class="rbutton">
                                                <asp:RadioButton ID="rbAll" runat="server" GroupName="TicketStatus"
                                                    AutoPostBack="true" OnCheckedChanged="rbOpen_CheckedChanged" Checked="true" />
                                                <label>All</label>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Class</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjTypeLHS" runat="server" ValueType="System.Int32" CssClass="list_box"
                                                                Width="255" Height="135px" SelectionMode="CheckColumn" ClientInstanceName="lstProjTypeLHS"
                                                                Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
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
                                                                Width="255" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
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
                                                                Width="255" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
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
                                                                Width="255" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
                                                                <ItemStyle Wrap="True" />
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonStateFuncArea(); }" />
                                                            </dx:ASPxListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel">Task Lists</td>
                                        <td class="ms-formbody">
                                            <div>
                                                <table class="table_lst">
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxListBox ID="lstProjectsLHS" runat="server" ValueType="System.Int32" ClientInstanceName="lstProjectsLHS"
                                                                Width="255" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
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
                                                                Width="255" Height="135px" SelectionMode="CheckColumn" CssClass="list_box" Font-Names="Verdana" Font-Size="8pt">
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td class="px10" style="vertical-align: top;">
                <asp:UpdatePanel ID="upPanelRight" runat="server">
                    <ContentTemplate>
                        <div>
                            <fieldset>
                                <legend>Report Components:</legend>
                                <table>
                                    <tr>
                                        <td class="ms-component-formlabel">Project Status
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkStatus"></asp:CheckBox>
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
                                        <td class="ms-component-formlabel">Summary Tasks
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkShowMilestone" Text=""></asp:CheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-component-formlabel">All Tasks
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkShowAllTasks" Text=""></asp:CheckBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="ms-formlabel">Summary Gantt Chart
                                        </td>
                                        <td class="ms-formbody">
                                            <asp:CheckBox runat="server" ID="chkGanttChart" Text="" GroupName="ProjectGanttChart"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>

                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="addEditPopup-btnWrap">
                    <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="lnkSubmit" runat="server" Text="Schedule Report" CssClass="primary-blueBtn" OnClick="lnkSubmit_Click">
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</asp:Panel>


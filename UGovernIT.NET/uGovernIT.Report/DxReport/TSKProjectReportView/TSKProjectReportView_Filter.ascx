
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TSKProjectReportView_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.TSKProjectReportView_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<style type="text/css">
    body {
        background-color: #fff;
    }
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 250px;
        vertical-align: top;
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
<%--    $(document).ready(function () {
        $(".ganttImg").click(function () {
            var url = '/ugovernit/delegatecontrol.aspx?control=tskprojectreportviewer';
            url = url + '&TSKIds=<%= TSKid %>';
            url = url + '&Status=' + ($('#<%= chkStatus.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&SGC=' + ($('#<%= chkGanttChart.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&SAT=' + ($('#<%= chkShowAllTasks.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&SMS=' + ($('#<%= chkShowMilestone.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&SKD=' + ($('#<%= chkKeyDeliverables.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&SKR=' + ($('#<%= chkKeyReceivables.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&ProjectDesc=' + ($('#<%= chkProjectDescription.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');
            url = url + '&ProjectRoles=' + ($('#<%= chkProjectRoles.ClientID %>').attr("checked") == 'checked' ? 'true' : 'false');

            window.parent.UgitOpenPopupDialog(url, '', 'Project Status Report', '100', '100', 0);

        });
    });--%>
</script>

<%--To Render DevEx Theme--%>
<dx:ASPxPanel runat="server" ID="dx_RenderDevexpressTheme" ClientVisible="false">
    <PanelCollection>
        <dx:PanelContent>
          
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxPanel>

<div style="width: 500px;">
    <fieldset>
        <legend>Report Components</legend>
        <table style="width: 100%;">
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
                <td class="ms-component-formlabel">Summary Gantt Chart
                </td>
                <td class="ms-formbody">
                    <asp:CheckBox runat="server" ID="chkGanttChart" Text="" GroupName="ProjectGanttChart"></asp:CheckBox>
                </td>
            </tr>
        </table>
    </fieldset>
    <div class="first_tier_nav">
        <table style="width: 100%; float: right">
            <tr>
                <td>
                    <ul class="list-unstyled">
                        <li runat="server" id="Li1" class="primary-blueBtn dxbButtonSys dxbTSys ml-2" style="float: right;">
                            <asp:LinkButton ID="lnkCancel" runat="server" Style="color: white; float: right;" Visible="true" Text="Cancel" CssClass="dxb" OnClick="lnkCancel_Click" />
                        </li>
                        <li runat="server" id="Li2" class="primary-blueBtn dxbButtonSys dxbTSys" style="float: right;">
                            <%--<a id="lnkBuild1" class="ganttImg" style="color: white; float: right;">Build Report</a>--%>
                            <asp:LinkButton ID="lnkBuild" runat="server" style="color: white; float: right;"  Text="Build Report" CssClass="dxb" OnClick="lnkBuild_Click" />
                        </li>
                    </ul>
                </td>
            </tr>
        </table>
    </div>
</div>


<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITProjectWizardUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITProjectWizardUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    body #s4-ribbonrow {
        display: none;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function monitorChanged(obj) {
        var obj = $(obj);
        var parentRow= obj.closest('tr');
        if (obj == null || obj == undefined)
            return;

        if (obj.is(":checked") == false) {
            $($(parentRow).find('.autocalc')).find('input[type=checkbox]').prop("checked", false)
            $(parentRow).find('.autocalc').hide();
        }
        else {
            $($(parentRow).find('.autocalc')).find('input[type=checkbox]').prop("checked", true)
            $(parentRow).find('.autocalc').show();
        }

    }
    $(function () {
        var wizardTrs = $(".wizardwidth> tbody > tr");
        if (wizardTrs.length >= 3) {
            var actionTr = $(wizardTrs[2]);
            var finishBt = $(wizardTrs[2]).find("input:submit[value='Finish']");
            finishBt.bind("click", function (e) {
                LoadingPanel.Show();
                actionTr.hide();
            });
        }
    });
    function ChangePortalSelection() {

        if ($("#<%=chkPortal.ClientID%>").attr("checked") == "checked") {
            $("#divShowFolder").show();
        }
        else {
            $("#divShowFolder").hide();
        }
    }
    function CreatePortal() {

        LoadingPanel.Show();
        var ticketID = '<%=TicketId %>';
        var find = '-';
        var reg = new RegExp(find, 'g');
        var docID = ticketID.replace(reg, '_');
        var chkFolder = $("#<%=chkFolder.ClientID%>").attr("checked");
      var isChecked = false;
      if (chkFolder) {
          isChecked = true;
      }
      var jsonData = { "IsChecked": isChecked, "moduleName": "PMM", "TicketId": ticketID };
      $.ajax({
          type: "POST",
          url: "<%=ajaxHelperURL %>/CreatePortal",
            data: JSON.stringify(jsonData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {

                //if ($.trim(message.d) == "") {
                var sourceUrl = escape("<%= Request.Url.AbsolutePath%>");
                window.frameElement.commitPopup(sourceUrl);
                //}
                //else {

                //}
                LoadingPanel.hide();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                LoadingPanel.hide();
            }
        });
        }
</script>
<%--SharePoint:CssRegistration ID="CssRegistration1" Name="/_layouts/15/1033/STYLES/Themable/ugitThemable.css" runat="server" />--%>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .Good {
        background-color: Green;
        cursor: pointer;
    }
    .content {
    padding: 10px;
}

    .OK {
        background-color: Yellow;
        cursor: pointer;
    }

    .Bad {
        background-color: Red;
        cursor: pointer;
    }

    .heading {
        font-weight: bold;
    }

    .topborder {
        border-top: 1px solid black;
    }

    linkbutton {
        FONT-WEIGHT: bold;
        FONT-SIZE: 7pt;
        TEXT-TRANSFORM: capitalize;
        COLOR: white;
        FONT-FAMILY: Verdana;
    }

    .doubleWidth {
        width: 99%;
    }

    .s4-toplinks .s4-tn a.selected {
        padding-left: 10px;
        padding-right: 10px;
    }

    .leftBox {
        width: 1%;
        height: 54px;
        text-align: right;
        background: url(/content/images/uGovernIT/box_left.gif) no-repeat;
    }

    .rightBox {
        width: 1%;
        height: 54px;
        background: url(/content/images/uGovernIT/box_right.gif) no-repeat;
    }

    .middleBox {
        width: 100%;
        height: 44px;
        padding-top: 10px;
        text-align: left;
        float: left;
        margin-top: 1px;
        margin-left: -1px;
        background: url(/content/images/uGovernIT/box_mid.gif) repeat-x;
    }

    .width25 {
        width: 25%;
    }

    .wizardwidth {
        width: 100%;
    }

    .hideblock {
        display: none;
    }

    .pmmitem-container {
        width: 500px;
        float: left;
        padding-bottom: 10px;
    }

    .pmmitem-title {
        float: left;
        padding-right: 5px;
        text-align: right;
        width: 175px;
        font-weight: bold;
    }

    .pmmitem-edit {
        float: left;
    }

    .peopleeditor-box {
        width: 300px;
    }

    .ms-inputuserfield {
        border: 1px solid gray;
    }

    .full-width {
        float: left;
        width: 99%;
    }

    .pmmitem-container2 {
        float: left;
        width: 99%;
        padding-bottom: 10px;
    }

    .importoption {
        float: left;
        font-weight: bold;
        left: -4px;
        position: relative;
    }

        .importoption input {
            position: relative;
            top: -2px;
        }

    .wizard-step {
        padding-top: 10px;
    }

    .summarypanel {
        float: left;
    }

    .closenpr {
        float: left;
        width: 300px;
        border: 1px dotted;
        padding: 5px 0px;
        margin-top: 5px;
    }

    .divPortal {
        float: left;
        width: 300px;
        padding: 5px 0px;
        margin-top: 5px;
    }

    .bordertop {
        border-top: 1px dotted;
    }

    .borderright {
        border-right: 1px dotted;
    }

    .borderleft {
        border-left: 1px dotted;
    }

    .borderbottom {
        border-bottom: 1px dotted;
    }

    .tableProject {
        border: 1px dotted;
        width: 300px;
    }
    /*.tableProject td{
        border-left: 1px dotted;
        border-right: 1px dotted;
    }*/
</style>

<table cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;" width="100%">
    <tr id="helpTextRow" runat="server">
        <td class="paddingleft8" align="right">
            <asp:Panel ID="helpTextContainer" runat="server">
            </asp:Panel>
        </td>
    </tr>
    <tr id="trError" runat="server">
        <td class="paddingleft8" align="center">
            <asp:Label ID="lblError" runat="server" Text="Invalid User, user must be the member of either PMO or PMMCreateGroup" ForeColor="Red">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:HiddenField ID="nprID" runat="server" />
            <asp:Wizard StepStyle-CssClass="wizard-step" runat="server" CssClass="wizardwidth" ID="pmmWizard" HeaderText="New Project" OnNextButtonClick="PMMWizard_NextButtonClick"
                CancelButtonType="Button" DisplayCancelButton="true" OnCancelButtonClick="PMMWizard_CancelButtonClick"
                OnFinishButtonClick="PMMWizard_FinishButtonClick" FinishPreviousButtonText="<< Previous" StartNextButtonText="Next >>" StepPreviousButtonText="<< Previous" StepNextButtonText="Next >>" DisplaySideBar="false" OnPreviousButtonClick="PMMWizard_PreviousButtonClick">

                <HeaderTemplate>
                    <table width="100%" style="border-collapse: collapse; padding: 0px;" cellspacing="0" cellpadding="0">
                        <tr>
                            <td class="arrow_active" runat="server" id="step1Div">
                                <div>
                                    <asp:Label runat="server" ID="Step1Header">1</asp:Label>
                                    <strong></strong>
                                </div>
                            </td>
                            <td class="arrow" runat="server" id="step2Div">
                                <div>
                                    <em></em>
                                    <asp:Label runat="server" ID="Step2Header">2</asp:Label>
                                    <strong></strong>
                                </div>
                            </td>
                            <td class="arrow" runat="server" id="step3Div">
                                <div>
                                    <em></em>
                                    <asp:Label runat="server" ID="Step3Header">3</asp:Label>
                                    <strong></strong>
                                </div>
                            </td>
                            <td class="arrow" runat="server" id="step4Div">
                                <div>
                                    <em></em>
                                    <asp:Label runat="server" ID="Step4Header">4</asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr valign="middle" class="selected_background" style="text-align: center;">
                            <td colspan="4" style="height: 20px">
                                <asp:Label runat="server" ID="stepHeading" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <WizardSteps>
                    <asp:WizardStep ID="wizardStep1" Title="Select NPR ticket" StepType="Start">

                        <ugit:ListPicker id="lstPicker" runat="server" Module="NPR" IsFilteredTableExist="true" ShowModuleDetail="false"></ugit:ListPicker>
                    </asp:WizardStep>

                    <asp:WizardStep ID="wizardStep2" Title="Basic Project Details" StepType="Step" OnActivate="Step2_Load">
                        <table cellpadding="3" cellspacing="2">
                            <tr>
                                <td>
                                    <div class="pmmitem-container">
                                        <span class="pmmitem-title">Project Name: </span>
                                        <span class="pmmitem-edit">
                                            <asp:Label ID="lblPmmProjectName" Visible="false" runat="server"></asp:Label>
                                            <asp:TextBox Width="295" ID="pmmProjectName" EnableViewState="true" runat="server"></asp:TextBox></span>
                                    </div>
                                </td>
                                <td rowspan="3" valign="top">
                                   <%-- <div id="pMonitors" runat="server" visible="false">--%>
                                    <asp:Panel ID="pMonitors" runat="server" Visible="false">
                                        <div><b>Monitors:</b></div>
                                        <div>
                                            <table>
                                                <asp:Repeater ID="monitorsRepeater" runat="server" EnableViewState="true" OnItemDataBound="monitorsRepeater_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox OnClick="monitorChanged(this);" runat="server" ID='checkbox' Text='<%# Eval("MonitorName")%>' Checked='<%# GetChecked(Eval("IsDefault").ToString()) %>' />

                                                                <asp:HiddenField ID="monitorLabel" runat="server" Value='<%# Eval("ID")%>'></asp:HiddenField>
                                                                <%--<asp:HiddenField ID="monitorDefaultLabel" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "MonitorName")%>'></asp:HiddenField>--%>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox CssClass="autocalc" Visible="false" runat="server" ID="chkautocalc" Text="Auto Calculate" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:CheckBox OnClick="monitorChanged(this);" runat="server" ID='checkbox' Text='<%# Eval("MonitorName")%>'  Checked='<%# GetChecked(Eval("IsDefault").ToString()) %>' />

                                                                <asp:HiddenField ID="monitorLabel" runat="server" Value='<%# Eval("ID")%>'></asp:HiddenField>
                                                                <%--<asp:HiddenField ID="monitorDefaultLabel" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "ModuleMonitorNameLookup")%>'></asp:HiddenField>--%>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox CssClass="autocalc" runat="server" Visible="false" ID="chkautocalc" Text="Auto Calculate" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                        <%--</div>--%>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pmmitem-container">
                                        <span class="pmmitem-title">Sponsors: </span>
                                        <span class="pmmitem-edit">
                                            <asp:Label ID="lblSponsors" runat="server"></asp:Label>
                                            <ugit:UserValueBox ID="pmmSponsor" runat="server" SelectionSet="User" isMulti="true"/>
                                            <%--<SharePoint:PeopleEditor PrincipalSource="UserInfoList" ID="pmmSponsor"
                                                AugmentEntitiesFromUserInfo="true"
                                                CssClass="peopleeditor-box" runat="server"
                                                MultiSelect="true"pmmSponsor />--%>
                                        </span>
                                        <asp:HiddenField ID="pmmSponsorHidden" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="pmmitem-container">
                                        <span class="pmmitem-title">Project Manager: </span>
                                        <span class="pmmitem-edit">
                                            <asp:Label ID="lblProjectManager" runat="server"></asp:Label>
                                            <ugit:UserValueBox ID="pmmProjectManager" runat="server" SelectionSet="User" isMulti="true" />
                                            <%--<SharePoint:PeopleEditor PrincipalSource="UserInfoList" AugmentEntitiesFromUserInfo="true" CssClass="peopleeditor-box" ID="pmmProjectManager" runat="server" MultiSelect="true" SelectionSet="User" />--%>
                                        </span>
                                        <asp:HiddenField ID="pmmProjectManagerHidden" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wizardStep3" Title="Project LifeCycle & Schedule" StepType="Step" OnActivate="Step3_Load">
                        <asp:Panel ID="pLifeCycleStageGUI" runat="server" class="pmmitem-container2"></asp:Panel>
                        <div class="pmmitem-container2">
                            <span class="pmmitem-title">Project Lifecycle: </span>
                            <span class="pmmitem-edit">
                              <%--  <asp:DropDownList ID="ddlLifeCycleModel" AutoPostBack="true" Width="175px" OnSelectedIndexChanged="ddlLifeCycleModel_SelectedIndexChanged" runat="server" OnInit="ddlLifeCycleModel_Init"></asp:DropDownList>--%>
                                <dx:ASPxComboBox ID="ddlLifeCycleModel" ClientInstanceName="ddlLifeCycleModel" runat="server" SelectedIndex="0" OnSelectedIndexChanged="ddlLifeCycleModel_SelectedIndexChanged" AutoPostBack="true" OnInit="ddlLifeCycleModel_Init" EnableViewState="false">
                                    <Items>
                                    </Items>
                                </dx:ASPxComboBox>
                            </span>
                        </div>
                        <asp:Panel ID="pImportTaskOptions" runat="server" CssClass="pmmitem-container2">
                            <span class="pmmitem-title">Import Tasks From: </span>
                            <span class="pmmitem-edit">
                                <%--<asp:RadioButton ID="btImportNPRSchedule" CssClass="importoption" Checked="true" OnCheckedChanged="InportSchedule_CheckedChanged" AutoPostBack="true" GroupName="importtask" runat="server" Text="NPR" />--%>
                                <asp:RadioButton ID="btImportTempleSchedule" CssClass="importoption" OnCheckedChanged="InportSchedule_CheckedChanged" AutoPostBack="true" GroupName="importtask" runat="server" Text="Template" />
                            </span>
                        </asp:Panel>
                        <asp:Panel ID="pImportFromTempalte" CssClass="full-width" runat="server" Visible="false">
                            <div class="pmmitem-container2">
                                <span class="pmmitem-title">Task Template: </span>
                                <span class="pmmitem-edit">
<%--                                    <asp:DropDownList ID="ddlTaskTemplates" Width="175px" runat="server" OnInit="ddlTaskTemplates_Init"></asp:DropDownList>--%>
                                </span>
                            </div>
                            <div class="pmmitem-container2">
                                <span class="pmmitem-title">Project Start Date: </span>
                                <span class="pmmitem-edit">
                                  <%--  <d:DateTimeControl ID="dtcStartDate" runat="server" DateOnly="true" />--%>
                                    <dx:ASPxDateEdit ID="dtcStartDate" runat="server"></dx:ASPxDateEdit>
                                </span>
                            </div>
                        </asp:Panel>
                        <div>
                            <span class="pmmitem-title">Enable Scrum</span>
                            <span class="pmmitem-edit">
                                <asp:CheckBox ID="chkIsScrum" runat="server" Checked="false" />
                            </span>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="wizardStep4" Title="Summary" StepType="Finish" OnActivate="Step4_Load">
                        <asp:Panel ID="wizardStep4Panel" CssClass="summarypanel" runat="server"></asp:Panel>
                        <div class="pmmitem-container2">
                            <div class="closenpr">
                                <span class="pmmitem-title">Close NPR: </span>
                                <span class="pmmitem-edit" style="position: relative; top: -2px">
                                    <asp:CheckBox ID="cbCloseNPR" runat="server" Checked="true" />
                                </span>
                            </div>
                            <div style="width: 99%; float: left;">
                                <div class="divPortal" style="border: 1px dotted;">
                                    <div>
                                        <span class="pmmitem-title">Create Document Portal: </span>
                                        <span class="pmmitem-edit" style="position: relative; top: -2px">
                                            <input type="checkbox" id="chkPortal" runat="server" onclick="ChangePortalSelection();" />
                                        </span>
                                    </div>

                                    <div id="divShowFolder" style="display: none;" class="divPortal">
                                        <span class="pmmitem-title">Create Default Folders: </span>
                                        <span class="pmmitem-edit" style="position: relative; top: -2px">
                                            <asp:CheckBox ID="chkFolder" runat="server" Checked="true" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </asp:WizardStep>
                </WizardSteps>
            </asp:Wizard>
            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please wait, this may take a few minutes ..." ClientInstanceName="LoadingPanel" Modal="True">
            </dx:ASPxLoadingPanel>
        </td>
    </tr>
</table>

<asp:Label ID="ddddd" runat="server"></asp:Label>
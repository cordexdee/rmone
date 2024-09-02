<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhaseChanges.ascx.cs" Inherits="uGovernIT.Web.PhaseChanges" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Panel ID="phasePanel" runat="server" Visible="false">
            <table width="100%" style="border-collapse: collapse" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5" UpdateMode="Conditional">
                            <Triggers></Triggers>
                            <ContentTemplate>
                                <asp:HiddenField ID="refreshPhase" runat="server" Value="false" />
                                <div class="ms-viewheadertr row" style="width:100%;float: left; height:auto; padding: 0; box-shadow: 0px 0px 1px #aaaaaa;">
                                    <div class="pmmStatus_projectSatgeTitle col-md-12">Project Stage</div>
                                    <div class="pmmStatus_projectSatgeDropDown col-md-11">
                                        <asp:DropDownList ID="ddlProjectStatus" runat="server" CssClass="aspxDropDownList itsmDropDownList"></asp:DropDownList>
                                    </div>
                                    <asp:Literal ID="liProjectstatus" runat="server" Visible="false"></asp:Literal>
                                    
                                    <div class="fright pmmStatus_btnchange-projectStage col-md-1">
                                        <asp:LinkButton ID="btChangeProjectStage" runat="server" OnClientClick="return changeProjectStage()" OnClick="BtChangeProjectStage_Click"><img style="border:0px;" src="/Content/images/newSaveIcon.png" alt="Save" /></asp:LinkButton>
                                    </div>
                                </div>
                                <dx:ASPxPopupControl ClientInstanceName="confirmCloseTasks" Modal="true"
                                    ID="confirmCloseTasks" Width="450px" Height="100px"
                                    ShowFooter="false" ShowHeader="true" HeaderText="Close Tasks"
                                    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">

                                    <ContentCollection>
                                        <dx:PopupControlContentControl ID="conformToClosePopup6" runat="server">

                                            <div style="width: 100%;">
                                                <div style="display: inline-flex; text-align: center;">
                                                    <dx:ASPxLabel ID="lblinformativeMsg" runat="server" EncodeHtml="false" Text="Do you also want to mark all tasks as Complete?" ClientInstanceName="lblinformativeMsg"></dx:ASPxLabel>
                                                </div>
                                                <div class="fright" style="margin: 30px 0px 0px 0px;">
                                                    <asp:LinkButton ID="btnYes" runat="server" Text="&nbsp;&nbsp;Yes&nbsp;&nbsp;" ToolTip="Move stage and close tasks" CommandName="Yes" OnClick="BtChangeProjectStage_Click" OnClientClick="ConfirmCloseTasks('yes');">
                                        <span class="button-bg">
                                            <b style="float: left; font-weight: normal;">
                                                Yes</b>
                                            <i style="float: left; position: relative; top: -3px;left:2px">
                                                <img src="/Content/ButtonImages/save.png"  style="border:none;" title="" alt=""/>
                                            </i> 
                                        </span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnNo" runat="server" Text="&nbsp;&nbsp;No&nbsp;&nbsp;" ToolTip="Move stage only" CommandName="No" OnClick="BtChangeProjectStage_Click" OnClientClick="ConfirmCloseTasks('no');">
                                        <span class="button-bg">
                                            <b style="float: left; font-weight: normal;">
                                                No</b>
                                            <i style="float: left; position: relative; top: -3px;left:2px">
                                                <img src="/Content/ButtonImages/save.png"  style="border:none;" title="" alt=""/>
                                            </i> 
                                        </span>
                                                    </asp:LinkButton>
                                                    <a href="javascript:Void(0);" onclick="confirmCloseTasks.Hide();"
                                                        title="Cancel">
                                                        <span class="button-bg">
                                                            <b style="float: left; font-weight: normal;">Cancel</b>
                                                            <i style="float: left; position: relative; top: -3px; left: 2px">
                                                                <img src="/Content/ButtonImages/cancelwhite.png" style="border: none;" title="" alt="" />
                                                            </i>
                                                        </span>
                                                    </a>
                                                </div>
                                            </div>

                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
</asp:Panel>

<asp:HiddenField ID="hdnConfirmCloseTasksAction" runat="server" />

<script id="dxss_scriptPhaseChanges" data-v="<%=UGITUtility.AssemblyVersion %>">
    

    function changeProjectStage() {
        var closedStageId = parseInt("<%= closeStageId %>");
        var selectedStageId = parseInt($("#<%= ddlProjectStatus.ClientID %> option:selected").val());
        if (closedStageId == selectedStageId) {
            confirmCloseTasks.Show();
            return false;
        }
        try {
            $(<%= refreshPhase.ClientID %>).val('true');
        } catch (e) {

        }
        return true;
    }
    function ConfirmCloseTasks(action) {

        //loadingPanel.Show();
        confirmCloseTasks.Hide();
        try {
            $("#<%=hdnConfirmCloseTasksAction.ClientID%>").val(action);
        } catch (e) {

        }

    }
</script>

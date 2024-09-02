
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestTypeByLocation.ascx.cs" Inherits="uGovernIT.Web.RequestTypeByLocation" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        border-top: 1px solid #A5A5A5;
        width: 250px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }
    legend {
        font-size:14px !important;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function editField(editPanelClass, viewPanelClass) {
        var hdnCtrl = $("." + viewPanelClass + " input:hidden");
        var state = hdnCtrl.val("");

        $("." + editPanelClass).show();
        $("." + viewPanelClass).hide();
    }
</script>
<div style="float: right; width: 98%; padding-left: 10px;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Location<b style="color: Red;">*</b></h3>
            </td>
            <td class="ms-formbody" style="width: 305px">
                <asp:DropDownList ID="ddlLocation" runat="server">
                </asp:DropDownList>
                <br />
                <asp:CustomValidator ID="cvLocation" CssClass="error" runat="server" Enabled="true"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel"></td>
            <td class="ms-formbody"></td>
        </tr>
    </table>
    <fieldset>
        <legend>SLA Override: Enter values to override module-level SLAs</legend>
        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
            <tr>
                <td class="ms-formlabel" style="width: 158px">
                    <h3 class="ms-standardheader">Requestor Contact SLA<b style="color: Red;">*</b></h3>
                </td>
                <td class="ms-formbody" style="width: 305px">
                    <asp:TextBox ID="txtRequestorContactSLA" runat="server" Width="50px" Text="0" />
                    <asp:DropDownList ID="ddlRequestorContactSLAType" runat="server">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                    <div>
                        <asp:RegularExpressionValidator ID="regextxtRequestorContactSLA" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?|(<Value Varies>)" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtRequestorContactSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvtxtRequestorContactSLA" CssClass="error" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtRequestorContactSLA" ErrorMessage="Enter Requestor Contact SLA" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Assignment SLA<b style="color: Red;">*</b></h3>
                </td>
                <td class="ms-formbody">
                    <asp:TextBox ID="txtAssignmentSLA" runat="server" Width="50px" Text="0" />
                    <asp:DropDownList ID="ddlAssignmentSLAType" runat="server">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                    <div>
                        <asp:RegularExpressionValidator ID="regextxtEscalationMinutes" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?|(<Value Varies>)" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtAssignmentSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvtxtEscalationMinutes" CssClass="error" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtAssignmentSLA" ErrorMessage="Enter Assignment SLA" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Resolution SLA<b style="color: Red;">*</b></h3>
                </td>
                <td class="ms-formbody">
                    <asp:TextBox ID="txtResolutionSLA" runat="server" Width="50px" Text="0" />
                    <asp:DropDownList ID="ddlResolutionSLAType" runat="server">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                    <div>
                        <asp:RegularExpressionValidator ID="regextxtResolutionSLA" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?|(<Value Varies>)" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtResolutionSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvtxtResolutionSLA" CssClass="error" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtResolutionSLA" ErrorMessage="Enter Resolution SLA" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Close SLA<b style="color: Red;">*</b></h3>
                </td>
                <td class="ms-formbody">
                    <asp:TextBox ID="txtCloseSLA" runat="server" Width="50px" Text="0" />
                    <asp:DropDownList ID="ddlCloseSLAType" runat="server">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                    <div>
                        <asp:RegularExpressionValidator ID="regextxtCloseSLA" CssClass="error" ValidationExpression="\d+(\.\d{1,2})?|(<Value Varies>)" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtCloseSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvtxtCloseSLA" CssClass="error" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtCloseSLA" ErrorMessage="Enter Close SLA" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset>
        <legend>Users</legend>
        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">

            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Owner<b style="color: Red;">*</b></h3>
                </td>
                <td class="ms-formbody" style="width: 305px">
                    <asp:Panel ID="pEditPPEOwner" CssClass="pEditPPEOwner" runat="server">
                        <ugit:UserValueBox ID="ppeOwner" runat="server" FieldName="RequestTypeOwner"></ugit:UserValueBox>
                    </asp:Panel>
                    <asp:Panel ID="pViewPPEOwner" CssClass="pViewPPEOwner" runat="server" Style="display: none;">
                        <asp:Label ID="lbOwner" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnOwnerVariesEnable" runat="server" />
                        <img onclick="editField('pEditPPEOwner', 'pViewPPEOwner')" src="/content/images/edit-icon.png" />
                    </asp:Panel>
                    <asp:CustomValidator ID="cvOwner" CssClass="error" runat="server" Enabled="true"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">PRP Group</h3>
                </td>
                <td class="ms-formbody">
                    <asp:Panel ID="pEditPPEPrpGroup" CssClass="pEditPPEPrpGroup" runat="server">
                        <ugit:UserValueBox ID="ppePrpGroup" runat="server"></ugit:UserValueBox>
                    </asp:Panel>
                    <asp:Panel ID="pViewPPEPrpGroup" CssClass="pViewPPEPrpGroup" runat="server" Style="display: none;">
                        <asp:Label ID="lbPRPGroup" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnPRPVariesEnable" runat="server" />
                        <img onclick="editField('pEditPPEPrpGroup', 'pViewPPEPrpGroup')" src="/content/images/uGovernIT/edit-icon.png" />
                    </asp:Panel>
                </td>
            </tr>
             <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">PRP</h3>
                </td>
                <td class="ms-formbody">
                    <asp:Panel ID="pEditPPEPRP" CssClass="pEditPPEORP" runat="server">
                        <ugit:UserValueBox ID="ppePRP" runat="server"></ugit:UserValueBox>
                    </asp:Panel>
                    <asp:Panel ID="pViewPPEPRP" CssClass="pViewPPEORP" runat="server" Style="display: none;">
                        <asp:Label ID="lbPRP" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnPRPUserVariesEnable" runat="server" />
                        <img onclick="editField('pEditPPEPRP', 'pViewPPEPRP')" src="/content/images/edit-icon.png" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">ORP</h3>
                </td>
                <td class="ms-formbody">
                    <asp:Panel ID="pEditPPEORP" CssClass="pEditPPEORP" runat="server">
                        <ugit:UserValueBox ID="ppeORP" runat="server"></ugit:UserValueBox>
                    </asp:Panel>
                    <asp:Panel ID="pViewPPEORP" CssClass="pViewPPEORP" runat="server" Style="display: none;">
                        <asp:Label ID="lbORP" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnORPVariesEnable" runat="server" />
                        <img onclick="editField('pEditPPEORP', 'pViewPPEORP')" src="/content/images/edit-icon.png" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Escalation Manager</h3>
                </td>
                <td class="ms-formbody">
                    <asp:Panel ID="pEditPPEExecManeger" CssClass="pEditPPEExecManeger" runat="server">

                        <ugit:UserValueBox ID="ppeExecManeger" runat="server"></ugit:UserValueBox>
                    </asp:Panel>
                    <asp:Panel ID="pViewPPEExecManeger" CssClass="pViewPPEExecManeger" runat="server" Style="display: none;">
                        <asp:Label ID="lbExecManager" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnExecManagerVariesEnable" runat="server" />
                        <img onclick="editField('pEditPPEExecManeger', 'pViewPPEExecManeger')" src="/content/images/edit-icon.png" />
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Backup Escalation Manager</h3>
                </td>
                <td class="ms-formbody">
                    <asp:Panel ID="pEditPPEBackUpMan" CssClass="pEditPPEBackUpMan" runat="server">

                        <ugit:UserValueBox ID="ppeBackUpMan" runat="server"></ugit:UserValueBox>
                    </asp:Panel>

                    <asp:Panel ID="pViewPPEBackUpMan" CssClass="pViewPPEBackUpMan" runat="server" Style="display: none;">
                        <asp:Label ID="lbBackupMan" runat="server"></asp:Label>
                        <asp:HiddenField ID="hdnBackupManVariesEnable" runat="server" />
                        <img onclick="editField('pEditPPEBackUpMan', 'pViewPPEBackUpMan')" src="/content/images/edit-icon.png" />
                    </asp:Panel>
                </td>
            </tr>

            <tr>
                <td colspan="2" class="ms-formlabel"></td>

            </tr>

        </table>
    </fieldset>
    <table width="100%">
        <tr id="tr2" runat="server">
            <td align="left" style="padding-top: 5px;">
                <div>
                    <asp:LinkButton ID="LnkbtnDelete" runat="server" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" ToolTip="Delete"
                        OnClientClick="return confirm('Are you sure you want to delete?');" OnClick="LnkbtnDelete_Click">
                            <span class="button-bg">
                        <b style="float: left; font-weight: normal;">
                            Delete</b>
                        <i style="float: left; position: relative; top: -3px;left:2px">
                            <img src="/content/images/cancel.png"  style="border:none;" title="" alt=""/>
                        </i> 
                    </span>
                    </asp:LinkButton>
                </div>
            </td>
            <td align="right" style="padding-top: 5px;">
                <div style="float: right;">
                     <dx:ASPxButton ID="btnSave" runat="server" CssClass="primary-blueBtn" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click">
                        <Image Url="/content/images/save.png"></Image>
                    </dx:ASPxButton>
                     <dx:ASPxButton ID="btnCancel" runat="server" CssClass="secondary-cancelBtn" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click">
                        <Image Url="/content/ButtonImages/cancelwhite.png"></Image>
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</div>

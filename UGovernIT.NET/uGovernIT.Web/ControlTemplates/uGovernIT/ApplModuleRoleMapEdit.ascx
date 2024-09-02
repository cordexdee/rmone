
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplModuleRoleMapEdit.ascx.cs" Inherits="uGovernIT.Web.ApplModuleRoleMapEdit" %>
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
    width: 190px;
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

.rdPermission-opt {
}

.rdPermission-opt td {
    padding-left: 15px;
}

.rdPermission-opt td:first-child {
    padding-left: 0px;
}

.rdPermission-opt input {
    float: left;
    position: relative;
    top: -2px;
}

.rdPermission-opt label {
    float: left;
}

.rdPermission-opt label > i {
    float: left;
}

.rdPermission-opt label > b {
    float: left;
    padding: 2px 0 0 2px;
}

.divNoUser {
    /*width: 100%;
    border: solid 1px #c0c0c0;*/
    text-align: center;
    padding-top: 10px;
    padding-bottom: 10px;
    background-color: #f2f3f4;
}

.divNoUser span {
    color: #4b4b4b;
    font-weight: 600;
    font-size: 12px;
}

#txtRoleAssigneeLookupSearchValue {
    width: 100%;
}

#txtRoleAssigneeLookupSearchValue .dxic.dxictb .dxeEditArea_UGITNavyBlueDevEx {
    width: 100% !important;
}

</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function LoadApplications(s,e) {
        
        var value = String( s.GetValue());
        hdnSelectedUser.Set("Id",value);
        btnGetAccess.DoClick();
        <%--$("#<%=btnGetAccess.ClientID%>").get(0).click();--%>
        //AddNotification1('Loading Existing Access for User ...');

    }
    
    function AddNotification1(msg) {
        if (notifyId != "") {
            RemoveNotification1()
        }
        //notifyId = SP.UI.Notify.addNotification(msg, true);
    }
    function RemoveNotification1() {
        //SP.UI.Notify.removeNotification(notifyId);
        notifyId = '';
    }
    $(document).ready(function () {
        $(".divNoUser").height(($(document).height() - ($(".tdRoleAssignee").height() + $(".upperdiv").height() + $(".trButtons").height() + 90)));
    });
</script>

<div>
    <asp:Panel ID="pnlEditMode" runat="server">
        <table style="width: 100%">
            <tr>
                <td colspan="2" class="tdRoleAssignee">
                    <asp:Label ID="lblRoleAssignee" runat="server" Text="Enter user to update access:" CssClass="lblHeader mt-2 d-block"></asp:Label>
                    <ugit:UserValueBox ID="txtRoleAssignee" runat="server" isMulti="false" CssClass="w-100 d-block mt-2 mb-3"></ugit:UserValueBox>
                    <%--<SharePoint:PeopleEditor PrincipalSource="UserInfoList" ID="txtRoleAssignee" MaximumHeight="30" Width="300px" CssClass="peAssignedTo" PlaceButtonsUnderEntityEditor="false" IsValid="false" onblur="LoadApplications"
                        ValidatorEnabled="false" AllowEmpty="false" EnableBrowse="true"  ugselectionset="User,SPGroup" SelectionSet="User,SPGroup" runat="server" MultiSelect="false" AugmentEntitiesFromUserInfo="true" AfterCallbackClientScript="javascript:LoadApplications();" />--%>
                    <dx:ASPxButton ID="btnGetAccess" runat="server" ClientInstanceName="btnGetAccess" OnClick="btnGetAccess_Click" Text="Show List of Applications to update access from"  ClientVisible="false" ValidationGroup="ServiceMatrix">
                        
                    </dx:ASPxButton>
                    <dx:ASPxHiddenField ID="hdnSelectedUser" ClientInstanceName="hdnSelectedUser" runat="server"></dx:ASPxHiddenField>
                    <%--<asp:Button ID="btnGetAccess" runat="server" OnClick="btnGetAccess_Click" Text="Show List of Applications to update access from" CssClass="hide" ValidationGroup="ServiceMatrix" />--%>
                    <asp:HiddenField ID="hdnAssignee" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblErrorMessage" runat="server" Text="Enter User" Style="color: red; font-weight: bold; display: none; padding-left: 7px;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblError" runat="server" Visible="false" Style="color: red; margin-left: 8px; font-weight: bold;"></asp:Label>
                    <div class="divNoUser" runat="server" id="divNoUser">
                        <span>Please specify user to show current access</span>
                    </div>
                    <asp:Panel ID="pnlServiceMatrix" runat="server"></asp:Panel>
                </td>
            </tr>

        </table>
    </asp:Panel>

    <table class="trButtons" style="width: 100%;">
        <tr id="trButtons" runat="server" style="position: absolute; bottom: 15px; right: 2px;">
            <td colspan="2">
                <div style="float: right; padding-left: 15px;">
                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                    <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
</div>

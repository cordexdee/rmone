
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationModulesEdit.ascx.cs" Inherits="uGovernIT.Web.ApplicationModulesEdit" %>
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

    tr.alternet {
        background-color: whitesmoke;
    }
</style>

<table width="100%">
     <tr id="tr2" runat="server">
           <td class="ms-formlabel">
            <h3 class="ms-standardheader">Item Order<b style="color: Red;">*</b>
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:DropDownList ID="ddlItemOrder" runat="server"></asp:DropDownList>
        </td>
     </tr>
   
    <tr id="trTitle" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Title<b style="color: Red;">*</b>
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtTitle" runat="server" Width="386px" />
            <div>
                <asp:CustomValidator id="csvTitle"  ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle" ErrorMessage="Module with same name already exists" ForeColor="Red" Display="Dynamic" OnServerValidate="csvTitle_ServerValidate" ValidationGroup="Save"></asp:CustomValidator>
                <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                    ErrorMessage="Enter Title" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </td>
    </tr>
    <tr id="tr1" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Description
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" /></td>
    </tr>
    <tr  id="trOwner" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Owner
            </h3>
        </td>
        <td class="ms-formbody">
            <ugit:UserValueBox id="ppeOwner" runat="server" isMulti="false" SelectionSet="User"  ValidationGroup="Save" CssClass="functionalArea-userValueBox userValueBox-dropDown"/>
        </td>
    </tr>
    <tr id="trSupportedBy" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Supported By
            </h3>
        </td>
        <td class="ms-formbody">
            <ugit:UserValueBox ID="ppesupportedBy" runat="server" isMulti="false" SelectionSet="User"  ValidationGroup="Save" CssClass="functionalArea-userValueBox userValueBox-dropDown"/>
        </td>
    </tr>
        <tr id="trAppAccessAdmin" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Access Admin
            </h3>
        </td>
        <td class="ms-formbody">
            <ugit:UserValueBox id="ppeAppAccessAdmin" runat="server" isMulti="false" SelectionSet="User"  ValidationGroup="Save" CssClass="functionalArea-userValueBox userValueBox-dropDown"/>
        </td>
    </tr>
    <tr id="trApprovers" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Approver(s)
            </h3>
        </td>
        <td class="ms-formbody">
             <ugit:UserValueBox id="ppeAppApprovers" runat="server" isMulti="false" SelectionSet="User"  ValidationGroup="Save" CssClass="functionalArea-userValueBox userValueBox-dropDown"/>
        </td>
    </tr>
    <tr id="trApprovalNeeded" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Approval Needed
            </h3>
        </td>
        <td class="ms-formbody">
             <ugit:LookUpValueBox ID="ddlApprovalNeeded" runat="server" Width="100%" FieldName="ApprovalTypeChoice" CssClass="lookupValueBox-dropown" FilterExpression="EnableModule=1" IsMandatory="true" ValidationGroup="Save" />
        </td>
    </tr>
    <tr id="tr3" runat="server">
        <td colspan="2">
            <div style="float: right;">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </td>
    </tr>
</table>

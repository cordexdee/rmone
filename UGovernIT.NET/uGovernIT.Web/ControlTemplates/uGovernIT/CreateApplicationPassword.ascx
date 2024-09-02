<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateApplicationPassword.ascx.cs" Inherits="uGovernIT.Web.CreateApplicationPassword" %>
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
        background-color:whitesmoke;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function PasswordExists()
    {
        alert('Password already exists for the user " + userName + "');
        return false;
    }
</script>
<table width="100%">
     <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Title
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtTitle" runat="server" Width="350px" /></td>
    </tr>
   
    <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Username
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtUserName" runat="server" Width="350px"></asp:TextBox>
            <br />
            <asp:Label ID="lblErrorMesage" runat="server" style="color:red;display:none;"></asp:Label>
             <%-- <SharePoint:PeopleEditor PrincipalSource="UserInfoList"  ID="userPeoplePicker" ValidatorEnabled="true" AllowEmpty="true" Width="350px"
                PlaceButtonsUnderEntityEditor="false" AllowTypeIn="true"  runat="server"
                MultiSelect="false"  ugselectionset="User" SelectionSet="User" EnableBrowse="True"  />--%>

        </td>
    </tr>
   <%-- <tr id="trPassword" runat="server" style="display:none;">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Decrypted Password
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:Label ID="lblPassword" runat="server" Width="386px" TextMode="Password" ></asp:Label>
         </td>
    </tr>--%>
    <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Password
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtPassword" runat="server" Width="200px" TextMode="Password" ></asp:TextBox>
            <asp:CheckBox ID="chkShowPassword" AutoPostBack="true" Visible="false" runat="server" ImageUrl="/_layouts/15/Images/uGovernIT/edit-icon.png" OnCheckedChanged="ChkShowPassword_Click" Text="Show Password" />
             
         </td>
    </tr>
     <tr>
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Description
            </h3>
        </td>
        <td class="ms-formbody">
            <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="3" cols="20" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="float: right;">
                 <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </td>
    </tr>
</table>
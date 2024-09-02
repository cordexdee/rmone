<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddEmployeeTypes.ascx.cs" Inherits="uGovernIT.Web.AddEmployeeTypes" %>


<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="tr12" runat="server">
            <div class="ms-formlabel">
                <p class="mb-1">Title</p>
            </div>
            <div>
                <asp:TextBox ID="txtEmployeeType" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtEmployeeType" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtEmployeeType"
                        ErrorMessage="Enter Employee Type" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row mt-2" id="tr13" runat="server">
            <div class="ms-formlabel">
                <p class="mb-1">Description</p>
            </div>
            <div>
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="4" />
            </div>
        </div>

        <div class="row mt-2" id="tr4" runat="server">
            <div class="ms-formlabel">
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="d-flex justify-content-between align-items-center">
            <dx:ASPxButton ID="lnkDelete" CssClass="btn-danger1" Text="Delete" ToolTip="Delete" runat="server"
                OnClick="lnkDelete_Click">
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn"
                    OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
  <%--  <table width="100%">
        <tr id="tr2" runat="server">
            <td align="left" style="padding-top: 5px;">
                <div style="float: left;">
                    

                </div>
            </td>
            <td align="right" style="padding-top: 5px;">
                <div style="float: right;">
                    <div style="float: right;">
                       
                    </div>
                </div>
            </td>
        </tr>
    </table>--%>
</div>

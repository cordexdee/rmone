<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentJobTitleMapping.ascx.cs" Inherits="uGovernIT.Web.DepartmentJobTitleMapping" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .contentPane {
        float: left;
        width: 100%;
        height: unset !important;
    }

    .rmmLookup-valueBoxEdit table.department tr td.dxic input[type="text"] {
        height: 34px !important;
        background: #fff;
    }

    .crm-checkWrap input:checked + label::after {
        top:3px;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function getroleid() {
        var ddlrole = $('#<%=ddlRole.ClientID%>');
        $("#txtRole").val(ddlrole.val());
    };
    function getcheckedvalue() {

        var ApprovalRequired = $('#<%= chkDeleted.ClientID %>').is(':checked');
        $("#txtdeleted").val(ApprovalRequired);
    }

</script>
 

    <div class="ms-formtable accomp-popup">
        <div class="col-md-12 col-sm-12 col-xs-12">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr10" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Department</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookupValueBoxEdit ID="departmentCtr" runat="server" CssClass="rmmLookup-valueBoxEdit"
                        FieldName="DepartmentLookup"></ugit:LookupValueBoxEdit>
                    <asp:Label ID="lbldept" runat="server" Text="(Required)" Visible="false" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="Div3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Employee Cost Rate</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxTextBox ID="txtECR" runat="server" DisplayFormatString="{0:C}" AutoCompleteType="Disabled" Width="100%"></dx:ASPxTextBox>
                    <asp:Label ID="lblecr" runat="server" Text="(Required)" Visible="false" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr7" runat="server" style="display: none">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Role</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlRole" runat="server" onchange="getroleid()" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <asp:Label ID="lblrole" runat="server" Text="(Required)" Visible="false" ForeColor="Red"></asp:Label>

                </div>
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDeleted" onclick="getcheckedvalue()" runat="server" ClientIDMode="Static" TextAlign="Right" Text="(Prevent use for new Job Title)" />
                </div>
                <asp:TextBox ID="txtdeleted" runat="server" Style="display: none" ClientIDMode="Static" ForeColor="Red"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" style="text-align: end;">
                <dx:ASPxButton ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" CssClass="secondary-cancelBtn">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" Visible="true" runat="server" Text="Save"
                    ToolTip="Save" CssClass="primary-blueBtn" AutoPostBack="false" ClientInstanceName="btnSave"
                    OnClick="btnSave_Click">
                </dx:ASPxButton>
            </div>
            <div class="row" style="text-align: center">
                <span style="text-align: center">
                    <asp:Label ID="lblErrorMessage" ViewStateMode="Enabled" EnableViewState="true" runat="server" ForeColor="Red"></asp:Label></span>
                <asp:TextBox ID="txtRole" ClientIDMode="Static" Style="display: none" runat="server"></asp:TextBox>
            </div>
        </div>

    </div>
</div>
 

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddJobTitle.ascx.cs" Inherits="uGovernIT.Web.AddJobTitle" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function DeleteCongigVariable() {
        if (confirm('Are you sure want to delete?')) {
            <%=Page.ClientScript.GetPostBackEventReference(lnkDelete1,string.Empty)%>
        }
    }
    function updateShortName(jobTitle) {
        // Get the short name by extracting the letters of the job title
        document.getElementById('<%= txtshortName.ClientID %>').value = jobTitle;
     }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .contentPane {
    float: left;
    width: 100%;
     height: unset !important; 
}
    .rmmLookup-valueBoxEdit table.department tr td.dxic input[type="text"] {
        height: 34px !important;
    }

    .dxeTextBox_UGITNavyBlueDevEx {
        display: block;
        height: 36px;
    }
</style>

<div class="ms-formtable accomp-popup py-2">
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr12" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Job Title</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:TextBox ID="txtJobTitle" runat="server" OnKeyUp="updateShortName(this.value)" Width="100%" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtJobTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtJobTitle"
                        ErrorMessage="Enter Job Title" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr10" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Short Name</h3>
            </div>
            <div class="ms-formbody pb-2">
                 <asp:TextBox ID="txtshortName" runat="server" Width="100%" ClientIDMode="Static" />
                 <asp:RequiredFieldValidator ID="rfvtxtshortName" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtshortName"
                        ErrorMessage="Enter Short Name" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </div>
        <%--<div class="col-md-4 col-sm-4 col-xs-4" id="tr10" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Department</h3>
            </div>
            <div class="ms-formbody pb-2">
                <ugit:LookupValueBoxEdit ID="departmentCtr" runat="server" CssClass="rmmLookup-valueBoxEdit"
                    FieldName="DepartmentLookup"></ugit:LookupValueBoxEdit>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr7" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Role</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
            </div>
        </div>--%>
        <div class="col-md-4 col-sm-4 col-xs-4">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Job Type</h3>
            </div>
            <div class="ms-formbody pb-2">
                <dx:ASPxComboBox ID="cmbJobType" runat="server" ValueType="System.String" Width="100%"></dx:ASPxComboBox>
            </div>
        </div>

       
    </div>
    <div class="row" style="display:none" >
        
       <%-- <div class="col-md-4 col-sm-4 col-xs-4" id="Div3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Employee Cost Rate</h3>
            </div>
            <div class="ms-formbody pb-2">
                <dx:ASPxTextBox ID="txtECR" runat="server" DisplayFormatString="{0:C}" Width="100%"></dx:ASPxTextBox>
            </div>
        </div>--%>
        <%--   <div class="col-md-4 col-sm-4 col-xs-4" id="Div2" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Billing Labor Rate</h3>
            </div>
            <div class="ms-formbody pb-2">
                <dx:ASPxTextBox ID="txtBLR" runat="server" DisplayFormatString="{0:C}" Width="100%"></dx:ASPxTextBox>
            </div>
        </div> --%>
    </div>
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-4" id="Div1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Role</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
            </div>
            
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Low Revenue Capacity</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:TextBox ID="txtLowRevenueCapacity" CssClass="ms-long" runat="server" Width="350px" />
                <asp:RegularExpressionValidator ID="revLowRevenueCapacity" CssClass="error" runat="server" ControlToValidate="txtLowRevenueCapacity"
                    Display="Dynamic" ErrorMessage="Please enter low revenue capacity in correct formate!"
                    ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr5" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">High Revenue Capacity</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:TextBox ID="txtHighRevenueCapacity" CssClass="ms-long" runat="server" />
                <asp:RegularExpressionValidator ID="revHighRevenueCapacity" CssClass="error" runat="server" ControlToValidate="txtHighRevenueCapacity"
                    Display="Dynamic" ErrorMessage="Please enter high revenue capacity in correct formate!"
                    ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Low Project Capacity</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:TextBox ID="txtLowProjectCapacity" CssClass="ms-long" runat="server" />
                <asp:RegularExpressionValidator ID="revLowProjectCapacity" CssClass="error" runat="server" ControlToValidate="txtLowProjectCapacity"
                    Display="Dynamic" ErrorMessage="Please enter project capacity in correct format"
                    ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">High Project Capacity</h3>
            </div>
            <div class="ms-formbody pb-2">
                <asp:TextBox ID="txtHighProjectCapacity" CssClass="ms-long" runat="server" />
                <asp:RegularExpressionValidator ID="revHighProjectCapacity" CssClass="error" runat="server" ControlToValidate="txtHighProjectCapacity"
                    Display="Dynamic" ErrorMessage="Please enter project capacity in correct format"
                    ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4" id="tr4" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Resource Level Tolerance</h3>
            </div>
            <div class="ms-formbody pb-2">
                <dx:ASPxTextBox ID="txtRLT" runat="server" Width="100%"></dx:ASPxTextBox>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-8">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div class="ms-formbody pb-2 crm-checkWrap">
                <asp:CheckBox ID="chkDeleted" runat="server" TextAlign="Right" Text="(Prevent use for new Job Title)" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-1 col-sm-1 col-xs-1 ">
            <dx:ASPxButton ID="lnkDelete" runat="server" Visible="false" Text="Delete" ToolTip="Delete" AutoPostBack="false"
                CssClass="btn-danger1">
                <ClientSideEvents Click="function(s,e){DeleteCongigVariable();}" />
            </dx:ASPxButton>
            <asp:LinkButton ID="lnkDelete1" runat="server" OnClick="lnkDelete_Click"></asp:LinkButton>
        </div>
         <div class="col-md-8 col-sm-8 col-xs-8">
             <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
             </div>
        <div  class="col-md-3 col-sm-3 col-xs-3">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click">
            </dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Save" ID="btnSave" Visible="true" runat="server" Text="Save"
                ToolTip="Save" CssClass="primary-blueBtn" OnClick="btnSave_Click">
            </dx:ASPxButton>
        </div>
    </div>
    <%--<div class="d-flex justify-content-between align-items-center" style="padding-left: 6px; padding-right: 6px;">

        
    </div>--%>
</div>

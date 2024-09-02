<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddDashboardFactTable.ascx.cs" Inherits="uGovernIT.Web.AddDashboardFactTable" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    function IsNumeric(e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup row">
        <div class="col-md-8 col-sm-8 col-xs-8 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="txtTitleRequired" runat="server" ControlToValidate="txtTitle" 
                    ValidationGroup="sg" Display="Dynamic" Text="*" ToolTip="Required"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4 noPadding" style="margin-top:15px;">
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chkCacheTable" AutoPostBack="true" runat="server" Text="Cache Table" TextAlign="Right" OnCheckedChanged="chkCacheTable_CheckedChanged" />
            </div>
        </div>

    </div>
    <div id="divCacheParam" runat="server" class="ms-formtable accomp-popup row">

        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Cache After</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:TextBox ID="txtCacheAfter" onkeypress="return IsNumeric(event);" ondrop="return false;" 
                     onpaste="return false;" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorAfter" runat="server" ControlToValidate="txtCacheAfter" 
                    ValidationGroup="sg" Display="Dynamic" Text="*" ToolTip="Required"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCacheAfter"
                    ValidationExpression="[0-9]*$" ErrorMessage="*" Display="Dynamic" />
             </div>
        </div>
        <%--<div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Cache Thresold</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtCacheThresold"  onkeypress="return IsNumeric(event);" ondrop="return false;"
                    onpaste="return false;" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCacheThresold"
                    ValidationExpression="[0-9]*$" ErrorMessage="*" Display="Dynamic" />
            </div>
        </div>--%>
        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Cache Mode</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlCacheMode" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                    <asp:ListItem Value="On-Demand">On-Demand</asp:ListItem>
                    <asp:ListItem Value="Scheduled">Scheduled</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Refresh Mode</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlRefreshMode" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                    <asp:ListItem Value="All">All</asp:ListItem>
                    <asp:ListItem Value="ChangesOnly">ChangesOnly</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

    </div>
    <div class="ms-formtable accomp-popup row">

        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
             <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Expiry Date</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxDateEdit ID="dtcExpiryDate" runat="server" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" 
                    CssClass="CRMDueDate_inputField" EditFormat="Date" DropDownButton-Image-Width="16px">
                </dx:ASPxDateEdit>
                <div>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ValidationGroup="sg"
                        ControlToValidate="dtcExpiryDate" Display="Dynamic" CssClass="text-error">
                        <span>Please enter Start Date.</span>
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Status</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                    <asp:ListItem Value="Not Started">Not Started</asp:ListItem>
                    <asp:ListItem Value="Completed">Completed</asp:ListItem>
                    <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

    <div class="ms-formtable accomp-popup row">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="5" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel"  
                    OnClick="btnCancel_Click">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnSubmit" runat="server"  Text="Save" ToolTip="Save" CssClass="primary-blueBtn"
                    ValidationGroup="sg" OnClick="btnSubmit_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>
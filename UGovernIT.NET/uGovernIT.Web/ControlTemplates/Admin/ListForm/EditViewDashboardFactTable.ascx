<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditViewDashboardFactTable.ascx.cs" Inherits="uGovernIT.Web.EditViewDashboardFactTable" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-standardheader {
        text-align: right;
    }

    .text-error {
        color: red;
        font-weight: 500;
        margin-top: 5px;
    }

    .dxeButtonEdit.full-width {
        width: 94%;
    }

    .full-width {
        width: 90%;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/_layouts/15/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }

    .required-item:after {
        content: '* ';
        color: red;
        font-weight: bold;
    }

    .ms-dlgFrameContainer {
        width: 100%;
    }

    .button-bg {
        color: white;
        float: left;
        cursor: pointer;
        background: url(/_layouts/15/images/uGovernIT/firstnavbg.gif) 0px 0px repeat-x scroll transparent;
        margin: 1px;
        padding: 5px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    function IsNumeric(e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        return ret;
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding" style="margin-top:10px;">
    <div id="divTitle" class="ms-formtable accomp-popup row">
        <div class="col-md-8 col-sm-8 col-xs-8 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title <b style="color: Red">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="txtTitleRequired" runat="server" ControlToValidate="txtTitle"
                    ValidationGroup="sg" Display="Dynamic" Text="*" ToolTip="Required"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="col-md-2 col-sm-2 col-xs-2 noPadding" style="margin-top: 15px;">
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chkCacheTable" runat="server" Text="Cache Table" AutoPostBack="true" TextAlign="Right" OnCheckedChanged="chkCacheTable_CheckedChanged" />
            </div>
        </div>

    </div>
    <div id="divCacheParam" runat="server" class="ms-formtable accomp-popup row">

        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Cache After<b style="color: Red">*</b></h3>
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
<%--        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Threshold Limit<b style="color: Red">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtThreshold" runat="server" onkeypress="return IsNumeric(event);" ondrop="return false;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rflvCacheThreshold" runat="server" ControlToValidate="txtThreshold"
                    ValidationGroup="sg" Display="Dynamic" Text="*" ToolTip="Required"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="regexCacheThreshold" runat="server" ControlToValidate="txtThreshold"
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
                <asp:DropDownList ID="ddlRefreshMode" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                    <asp:ListItem Value="All">All</asp:ListItem>
                    <asp:ListItem Value="ChangesOnly">ChangesOnly</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

    </div>
    <div id="divStatus" class="ms-formtable accomp-popup row">
        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Status</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlStatus" Enabled="false" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                    <asp:ListItem Value="Not Started">Not Started</asp:ListItem>
                    <asp:ListItem Value="Completed">Completed</asp:ListItem>
                    <asp:ListItem Value="In Progress">In Progress</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Expiry Date</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxDateEdit ID="dtcExpiryDate" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" runat="server"
                    EditFormat="Date" Enabled="false" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="16px">
                </dx:ASPxDateEdit>
                <div>
                    <asp:RequiredFieldValidator ID="rfvDate" runat="server" ValidationGroup="sg"
                        ControlToValidate="dtcExpiryDate" Display="Dynamic" CssClass="text-error">
                        <span>Please enter Start Date.</span>
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
         <div class="col-md-4 col-sm-4 col-xs-4 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Last Updated</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <%--<dx:ASPxDateEdit ID="dtclastupdated" runat="server" Enabled="false" EditFormat="Date" Width="180px"></dx:ASPxDateEdit>--%>
                <dx:ASPxLabel ID="lblLastUpdated" runat="server" ></dx:ASPxLabel>
                <dx:ASPxTextBox ID="txtLastupdated" runat="server" Visible="false" Enabled="false"></dx:ASPxTextBox>
                <%--<div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="sg"
                        ControlToValidate="dtclastupdated" Display="Dynamic" CssClass="text-error">
                            <span>Please enter last updated date</span>
                    </asp:RequiredFieldValidator>
                </div>--%>
            </div>
        </div>

    </div>
    <div id="divDescription" class="ms-formtable accomp-popup row">
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
        <div class="d-flex justify-content-between align-items-center px-1">
            <dx:ASPxButton ID="lnkDelete" runat="server" CssClass="btn-danger1" Text="Delete" ToolTip="Delete"
                OnClick="lnkDelete_Click">
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn"
                    ToolTip="Cancel" OnClick="btnCancel_Click">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btnSubmit" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn"
                    ValidationGroup="sg" OnClick="btnSubmit_Click">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>


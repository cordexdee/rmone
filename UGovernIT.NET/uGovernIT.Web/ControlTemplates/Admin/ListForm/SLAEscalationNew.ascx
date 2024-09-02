<%@ Register Src="~/CONTROLTEMPLATES/uGovernIT/HtmlEditorControl.ascx" TagPrefix="ugit" TagName="HtmlEditorControl" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SLAEscalationNew.ascx.cs" Inherits="uGovernIT.Web.SLAEscalationNew" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="tr12" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" 
                        OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" ></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlModule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                            ErrorMessage="Select Module "  Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">SLA Rule</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlSLARule" runat="server" CssClass="itsmDropDownList aspxDropDownList" OnSelectedIndexChanged="ddlSLARule_SelectedIndexChanged"
                        AutoPostBack="true"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlSLARule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlSLARule"
                            ErrorMessage="Choose SLA Rule " Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr11" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                       <asp:CheckBox ID="cbUseDesiredComDate" runat="server" Text="Use Desired completion date if later" />
                </div>
            </div>
        </div>
        
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel" style="padding-left:15px">
                <h3 class="ms-standardheader budget_fieldLabel">Escalation After</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-6 ">
                    <asp:TextBox ID="txtEscalationMinutes" runat="server" />
                </div>
                 <div class="col-md-6 col-sm-6 col-xs-6 ">
                    <asp:DropDownList ID="ddlSLAUnitType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                 </div>
                <div>
                    <asp:RegularExpressionValidator ID="regextxtEscalationMinutes" ValidationExpression="\d+(\.\d{1,2})?" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtEscalationMinutes" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvtxtEscalationMinutes" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtEscalationMinutes" ErrorMessage="Enter Escalation Minutes" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
         
        <div class="row" id="tr7" runat="server">
            <div class="ms-formlabel" style="padding-left:15px">
                <h3 class="ms-standardheader budget_fieldLabel">Escalation To</h3>
            </div>
            <div class="ms-formbody accomp_inputField" style="padding-left:15px;">
                <asp:CheckBoxList ID="chklstEscalationRules" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="chklstEscalationRules_SelectedIndexChanged" Height="80px" Width="100%" RepeatColumns="2">
                </asp:CheckBoxList>
                <asp:HiddenField ID="hdnEscalationRules" runat="server" />
            </div>
        </div>
        <div class="row" id="tr9" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                 <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="cbIncludeActionUser" runat="server" Checked="true" Text="CC to Action User" />
                </div>
            </div>
        </div>
        <div class="row" id="tr5" runat="server">
            <div class="ms-formlabel" style="padding-left:15px;">
                <h3 class="ms-standardheader budget_fieldLabel">Also Send To (Emails)</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <div class="col-md-6 col-sm-6 col-xs-6">
                     <asp:TextBox ID="txtEscalationEmail" runat="server"/>
                </div>
               <div class="col-md-6 col-sm-6 col-xs-6 crm-checkWrap">
                   <asp:CheckBox ID="chkbxNotifyInPlainText" runat="server" Text="Notify In Plain Text" />
               </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel" style="padding-left:15px;">
                <h3 class="ms-standardheader budget_fieldLabel">Escalation Frequency </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <asp:TextBox ID="txtEscalationFrequency" runat="server" />
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <asp:DropDownList ID="ddlSLAFreqUnitType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div>
                    <asp:RegularExpressionValidator ID="regextxtEscalationFrequency" ValidationExpression="\d+(\.\d{1,2})?" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtEscalationFrequency" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save"  ForeColor="Red"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvtxtEscalationFrequency" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtEscalationFrequency" ErrorMessage="Enter Escalation Frequency" Display="Dynamic" ValidationGroup="Save"  ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr8" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Mail Subject</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtMailSubject" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtMailSubject" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtMailSubject" ErrorMessage="Enter Mail Subject" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr6" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email Body</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:HtmlEditorControl runat="server" id="HtmlEditorControl"></ugit:HtmlEditorControl>
                </div>
            </div>
        </div>
        
        
        <div class="row" id="tr10" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
           
        </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12">
                 <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" ToolTip="Save" ValidationGroup="Save"  OnClick="btnSave_Click" 
                    AutoPostBack="true" ></dx:ASPxButton>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass=" secondary-cancelBtn" ToolTip="Cancel"  OnClick="btnCancel_Click" AutoPostBack="true">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

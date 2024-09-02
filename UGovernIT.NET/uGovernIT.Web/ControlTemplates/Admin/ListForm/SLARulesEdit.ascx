<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SLARulesEdit.ascx.cs" Inherits="uGovernIT.Web.SLARulesEdit" %>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trTitle" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="true" Enabled="false" CssClass="itsmDropDownList aspxDropDownList" 
                        OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlModule" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlModule"
                            ErrorMessage="Select Module " InitialValue="0" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                            ErrorMessage="Enter Title " Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">SLA Category</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlSLACategory" runat="server" CssClass="itsmDropDownList aspxDropDownList" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlSLACategory" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlSLACategory"
                            ErrorMessage="Choose SLA Category " InitialValue="0" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
       
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr7" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Priority</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlPriority" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlPriority"
                            ErrorMessage="Select Priority " InitialValue="0" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">SLA Days Round UpDown</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlSLADaysRoundUpDown" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel" style="padding-left:15px;">
                <h3 class="ms-standardheader budget_fieldLabel">SLA </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <asp:TextBox ID="txtSLAHours" runat="server" />
                </div>
                 <div class="col-md-6 col-sm-6 col-xs-6">
                     <asp:DropDownList ID="ddlSLAUnitType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                        <asp:ListItem Value="Days">Days</asp:ListItem>
                        <asp:ListItem Value="Hours">Hours</asp:ListItem>
                        <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                    </asp:DropDownList>
                 </div>
                <div>
                    <asp:RegularExpressionValidator ID="regexSLAHours" ValidationExpression="^([0-9]+)$" ValidateEmptyText="true" 
                        Enabled="true" runat="server" ControlToValidate="txtSLAHours" ErrorMessage="Invalid Format" Display="Dynamic" 
                        ValidationGroup="Save" ForeColor="Red"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="revtxtSLAHours" ValidateEmptyText="true" Enabled="true" runat="server" 
                        ControlToValidate="txtSLAHours" ErrorMessage="Enter SLA (Hours)" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
       <div class="row">
           <div class="col-md-6 col-sm-6 col-xs-6" id="tr8" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">SLA Target</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtSLATarget" runat="server" />
                    <div>
                        <asp:RegularExpressionValidator ID="regexSLATarget" ValidationExpression="^([0-9.]+)$" ValidateEmptyText="true" Enabled="true" 
                            runat="server" ControlToValidate="txtSLATarget" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="Save" 
                            ForeColor="Red"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvtxtSLATarget" ValidateEmptyText="true" Enabled="true" runat="server" 
                            ControlToValidate="txtSLATarget" ErrorMessage="Enter SLA Target" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
           <div class="col-md-6 col-sm-6 col-xs-6" id="tr6" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Start Stage</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlStartStage" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlStartStage" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlStartStage"
                            ErrorMessage="Select Start Stage " InitialValue="0" Display="Dynamic" ValidationGroup="Save" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
       </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr9" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">End Stage</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlEndStage" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlEndStage" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlEndStage"
                            ErrorMessage="Select End Stage " InitialValue="None" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr10" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" ></asp:TextBox>
                </div>
            </div>
        </div>
        
        <div class="row">
             <div class="col-md-6 col-sm-6 col-xs-6" id="tr11" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
            </div>
        </div>
       
       <div class="row addEditPopup-btnWrap" id="tblactiontable" runat="server">
           <div class="row">
               <dx:ASPxButton ID="DeletButton" runat="server" Text="Delete" ToolTip="Delete" Visible="false" CssClass="secondary-cancelBtn" 
               OnClick="DeletButton_Click" AutoPostBack="true" ></dx:ASPxButton>
               <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" 
                   OnClick="btnCancel_Click" AutoPostBack="true"></dx:ASPxButton>
               <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" CssClass="primary-blueBtn" 
                   OnClick="btnSave_Click" AutoPostBack="true" ></dx:ASPxButton>
           </div>
       </div>
    </div>
</div>

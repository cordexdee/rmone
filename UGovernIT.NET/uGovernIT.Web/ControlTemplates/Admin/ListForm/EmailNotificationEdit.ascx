
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailNotificationEdit.ascx.cs" Inherits="uGovernIT.Web.EmailNotificationEdit" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/HtmlEditorControl.ascx" TagPrefix="ugit" TagName="HtmlEditorControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function LnkbtnDelete_Click(s, e) {
        if (confirm('Are you sure you want to delete?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }

    //function btnSaveClick(s, e) {
    //    
    //    e.processOnServer = false;
    //    if (Page_IsValid)
    //        e.processOnServer = true;
    //}
    
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding my-2">
    <div class="ms-formtable accomp-popup">
        <div class="row bs" id="trTitle" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:DropDownList runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" 
                        ID="ddlModule"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-12 col-sm-12 col-xs-12" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:TextBox ID="txtStatus" runat="server" ValidationGroup="EmailNotification"></asp:TextBox>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtStatus" ForeColor="Red"  runat="server" ControlToValidate="txtStatus" ErrorMessage="Enter Title"
                            Display="Dynamic" ValidationGroup="EmailNotification"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email Title</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:TextBox ID="txtEmailTitle" runat="server"></asp:TextBox>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtEmailTitle" ForeColor="Red" ValidateEmptyText="true" runat="server"
                            ControlToValidate="txtEmailTitle" ErrorMessage="Enter Email Title " Display="Dynamic" ValidationGroup="EmailNotification">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12" id="tr5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email Body<b style="color: Red;">*</b></h3>
                    <div>
                        <asp:Label ID="lblEmailBodyErrorMessae" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="ms-formbody pb-2">
                    <ugit:HtmlEditorControl runat="server" id="htmlEditor" />
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6" id="Div1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Event Type</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:DropDownList runat="server" ID="ddlEmailEventType" OnSelectedIndexChanged="ddlEmailEventType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <div>
                         <asp:RequiredFieldValidator ID="rfvEmailEventType" ValidateEmptyText="true" InitialValue="None" Enabled="true" runat="server" ControlToValidate="ddlEmailEventType" ErrorMessage="Event Type Should not be none" ForeColor="Red" Display="Dynamic" ValidationGroup="EmailNotification"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="trModuleStep" runat="server" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module Step</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:DropDownList runat="server" ID="ddlModuleStep" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row bs">
           

            <div class="col-md-6 col-sm-6 col-xs-6" id="trPriority" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Priority</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:DropDownList runat="server" ID="ddlPriority" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                </div>
            </div>
       </div>
        <div class="row bs" id="tr4" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email User Types</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:CheckBoxList ID="chklstEmailUserTypes" CssClass="crm-checkWrap"  runat="server" Width="100%" RepeatColumns="2" />
                    <asp:HiddenField ID="hdnEmailUserTypes" runat="server" />
                    <div class="crm-checkWrap" style="width:100%;">
                        <asp:CheckBox ID="chkUserSPGroup" runat="server" Text="User Group" AutoPostBack="true" OnCheckedChanged="chkUserSPGroup_CheckedChanged" />
                        <div style="margin-top:10px; width:100%;">
                            <ugit:UserValueBox ID="ppeSPGroup" CssClass="userValueBox-dropDown" runat="server" isMulti="true"  SelectionSet="Group" Width="100%" MaximumHeight="30"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr7" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">EmailTo CC</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:TextBox ID="txtEmailToCC" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="col-md-6 col-sm-6 col-xs-6" id="tr6" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Custom Properties</h3>
                </div>
                <div class="ms-formbody pb-2">
                    <asp:TextBox ID="txtCustomProperties" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6" id="tr8" runat="server">
                <div class="ms-formbody pb-2 crm-checkWrap">
                    <asp:CheckBox ID="chkSendEvenIfStageSkipped" Text="Send Even If Stage Skipped" runat="server"></asp:CheckBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody pb-2 crm-checkWrap">
                    <asp:CheckBox ID="chkShowTicketFooter" runat="server" Text="Enable Ticket Details Footer"></asp:CheckBox>
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formbody pb-2 crm-checkWrap">
                    <asp:CheckBox ID="chkAllowPlainTxt" runat="server" Text="Notify in Plain Text"></asp:CheckBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formline">
                        <img width="1" height="1" alt="" src="/Content/images/blank.gif">
                </div>
            </div>
        </div>

        <div class="d-flex justify-content-between align-items-center px-1">
            <dx:ASPxButton ID="LnkbtnDelete" runat="server" Text="Delete" ToolTip="Delete" CssClass="btn-danger1" OnClick="LnkbtnDelete_Click" AutoPostBack="false" >
                <ClientSideEvents Click="LnkbtnDelete_Click" />
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton  ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="EmailNotification" OnClick="btSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

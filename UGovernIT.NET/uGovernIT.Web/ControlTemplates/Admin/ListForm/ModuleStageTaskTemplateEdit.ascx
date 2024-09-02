<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleStageTaskTemplateEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleStageTaskTemplateEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>"> 
    function LnkbtnDelete_Click(s, e) {
        var msg ='Are you sure?';
        if (hdnUserEnabledStatus.Get("isEnabled") == "1")
            msg = 'Are you sure you want to disable?';
        else
            msg = 'Are you sure you want to enable?';
        if (confirm(msg)) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row bs">
            <div class="col-md-6 col-sm-6 col-xs-6" id="trTitle" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp-popup">
                    <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="Task"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                        Display="Dynamic" ErrorMessage="Please enter title." ForeColor="Red"></asp:RequiredFieldValidator>

                    <asp:Label ID="lbTitle" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="trModules" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp-popup">
                    <asp:DropDownList ID="ddlModule" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" CssClass="itsmDropDownLists aspxDropDownList" 
                        runat="server"></asp:DropDownList>
                    <asp:Label ID="lbModule" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row bs mt-2">
            <div class="col-md-6 col-sm-6 col-xs-6" id="trStage" runat="server">
                <div class="ms-formlabel">
                     <h3 class="ms-standardheader budget_fieldLabel">Stage<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp-popup">
                     <asp:DropDownList ID="ddlModuleStep" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="requiredModule" runat="server" ControlToValidate="ddlModuleStep" ErrorMessage="Select Stage" SetFocusOnError="true" Display="Dynamic"  ForeColor="Red" ValidationGroup="Task"></asp:RequiredFieldValidator>
                    <asp:Label ID="lbStage" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="trItemOrder" runat="server">
                <div class="ms-formlabel">
                     <h3 class="ms-standardheader budget_fieldLabel">Item Order</h3>
                </div>
                <div class="ms-formbody accomp-popup">
                     <asp:TextBox ID="txtItemOrder" runat="server"></asp:TextBox>
                    <asp:Label ID="lblItemOrder" runat="server" Visible="false"></asp:Label>
                    <asp:RegularExpressionValidator ID="regextxtItemOrder" ErrorMessage="Only numeric allow." ControlToValidate="txtItemOrder" runat="server" 
                        ValidationExpression="^([0-9]+)$" ValidateEmptyText="false" ValidationGroup="Task" ForeColor="Red" />
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-6 col-sm-6 xol-xs-6" id="trAssignedTo" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Assigned To</h3>
                </div>
                 <div class="ms-formbody accomp-popup">
                       <ugit:UserValueBox ID="peAssignedTo" CssClass="assignto-userToken" runat="server" isMulti="true" Width="100%"></ugit:UserValueBox>
                        <asp:Label ID="lbAssignedTo" runat="server" Visible="false"></asp:Label>
                 </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="trUserRoleType" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">User Role Type</h3>
                </div>
                <div class="ms-formbody accomp-popup">
                    <dx:ASPxGridLookup Visible="true" CssClass="stagesctionusers aspxGridLookUp-dropDown" TextFormatString="{2}" SelectionMode="Single" ID="glUserType" 
                        runat="server" KeyFieldName="Name" MultiTextSeparator=";" Width="100%">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="Type" GroupIndex="0" SortOrder="Descending"   Visible="false"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="NameRole" Width="315px" Caption="Choose User Role Type:" SortOrder="Ascending">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="UserType" Visible="false">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Name" Visible="false">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                            <GridViewProperties>
                                <Settings  GroupFormat="{1}" ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            </GridViewProperties>
                        <ClientSideEvents />
                    </dx:ASPxGridLookup>
                    <asp:Label ID="lblUserRoleType" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row bs">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="trDueDate" runat="server">
                <div class="ms-formlabel" style="padding-left:15px;">
                     <h3 class="ms-standardheader budget_fieldLabel">Due Date</h3>
                </div>
                <div class="ms-formbody accomp-popup">
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:DropDownList ID="ddlModuleDates" runat="server" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4 noPadding" style="display:none;">
                        <dx:ASPxDateEdit Visible="false" ID="dtcValue" runat="server" CssClass="CRMDueDate_inputField"
                        DropDownApplyButton-Image-Url="~/Content/Images/calendarNew.png" DropDownApplyButton-Image-Width="18"></dx:ASPxDateEdit>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:DropDownList ID="ddlOperator" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Value="+" Text="+"></asp:ListItem>
                            <asp:ListItem Value="-" Text="-"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4 col-sm-4 col-xs-4">
                        <asp:TextBox ID="txtDays" runat="server" Width="40px"></asp:TextBox>
                        <span><b>days </b></span>
                        <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row bs"> 
            <div class="col-md-6 col-sm-6 col-xs-6" id="trEstimatedHours" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Estimated Hours</h3>
                </div>
                 <div class="d-flex justify-content-between align-items-center">
                     <asp:TextBox ID="txtEstimatedHours" ValidationGroup="Task" CssClass="estimatedhours" runat="server"></asp:TextBox>
                     <div class="ml-2"><asp:Label ID="lbEstimatedHours" runat="server" Visible="false"></asp:Label>hrs</div>
                 </div>
                <asp:RegularExpressionValidator ID="revEstimatedHours" runat="server" ControlToValidate="txtEstimatedHours" ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter estimated hour in correct format" ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="trNote" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Task Description</h3>
                </div>
                <div class="ms-formbody accomp-popup">
                     <asp:TextBox CssClass="full-width" Rows="5" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                    <asp:Label ID="lbDescription" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
        <div class="row bs">
            
        </div>
        <div class="d-flex justify-content-between align-items-center">
            <div class="" id="tdDelete" runat="server" style="display:inline-block;">
                <dx:ASPxButton ID="LnkbtnDelete" runat="server" Text="Disable"  CssClass="btn-danger1" ToolTip="Delete"  OnClick="btDelete_Click">
                    <ClientSideEvents Click="LnkbtnDelete_Click" />
                </dx:ASPxButton>
                <dx:ASPxHiddenField ID="hdnUserEnabledStatus" runat="server" ClientInstanceName="hdnUserEnabledStatus"></dx:ASPxHiddenField>
            </div>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel"  CssClass="secondary-cancelBtn" ToolTip="Cancel"  OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Task" OnClick="btSaveTask_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>

  
   <%-- <table width="100%" cellspacing="0" cellpadding="0">
        <tbody>
            <tr>
                <td class="ms-formline">
                    <img width="1" height="1" alt="" src="/_layouts/15/images/blank.gif"></td>
            </tr>
        </tbody>
    </table>--%>
   
</div>

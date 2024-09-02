<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMRisksEdit.ascx.cs" Inherits="uGovernIT.Web.PMMRisksEdit" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
        $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
    });  
</script>
<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap fetch-popupParent">
    <asp:Panel runat="server" ID="taskForm">
        <div class="accomp-popup ms-formtable">
            <div class="row" id="trTitle" runat="server">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Risk<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Save" />
                            <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                                ErrorMessage="Please enter title." ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12" id="trImpact" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Impact</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:DropDownList ID="ddlImpact" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                <asp:ListItem Text="Project"></asp:ListItem>
                                <asp:ListItem Text="Program"></asp:ListItem>
                                <asp:ListItem Text="Corporate"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbIssueImpact" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            <div class="row" id="trProbability" runat="server">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">% Probability<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtProbability" runat="server"></asp:TextBox>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvtxtProbability" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtProbability"
                                    ErrorMessage="Please enter probability." ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rfvtxtProbability2" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtProbability"
                                    ErrorMessage="Please enter value in percentage." ForeColor="Red" Display="Dynamic" Type="Double" MinimumValue="0" MaximumValue="100" ValidationGroup="Save"></asp:RangeValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12" id="trAssignedTo" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Assigned To</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <ugit:UserValueBox ID="peAssignedTo" runat="server" CssClass="userValueBox-dropDown" />
                            <asp:Label ID="lbAssignedTo" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            <div class="row" id="trMitigationPlan" runat="server">
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Mitigation Plan</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtMitigationPlan" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12" id="trContingencyPlan" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Contingency Plan</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtContingencyPlan" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            <div class="row">
               <div class="col-md-6 col-sm-6 col-xs-6" >
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Attachments</h3>
                    </div>
                   <div class="ms-formbody accomp_inputField">
                        <ugit:FileUploadControl ID="FileUploadControl" runat="server" Width="100%"/>
                    </div>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" ImagePosition="Right" CssClass="secondary-cancelBtn"
                            OnClick="btnCancel_Click">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="LnkbtnDelete" runat="server" Text="Delete" ToolTip="Delete" CssClass="secondary-cancelBtn" 
                            OnClick="LnkbtnDelete_Click">
                            <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to delete forever?');}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="lnkbtnArchive" CssClass="secondary-cancelBtn" runat="server" Text="Archive" ToolTip="Delete" 
                            OnClick="lnkbtnArchive_Click">
                            <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to archive?');}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="LnkBtnUnarchive" CssClass="primary-blueBtn" runat="server" Text="Unarchive" ToolTip="Delete" 
                            OnClick="LnkBtnUnarchive_Click">
                            <ClientSideEvents Click="function(s, e){return confirm('Are you sure you want to unarchive?');}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" ValidationGroup="Save" OnClick="btnSave_Click"
                            ID="btnSave" runat="server" Text="Save">
                        </dx:ASPxButton>
                    </div>
                </div>
        </div>
    </asp:Panel>
</div>






<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectResourceUpdate.ascx.cs" Inherits="uGovernIT.Web.ProjectResourceUpdate" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });
</script>
<dx:ASPxHiddenField ID="hdnInformation" runat="server" ClientInstanceName="hdnInformation"></dx:ASPxHiddenField>
 
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row">
        <div class="fullwidth" style="text-align:left; padding-top: 10px; padding-bottom: 10px;">
            <asp:Label ID="lbInformationMsg" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="ms-formtable accomp-popup  noPadding">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Resource <b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                   <asp:Label ID="lbResource" runat="server" Visible="false" CssClass="resourceVal"></asp:Label>
                   <asp:Panel ID="panelEditResource" runat="server">
                       <ugit:UserValueBox runat="Server" IsMulti="true" Width="100%" ID="peditResource" CssClass="userValueBox-dropDown">
                       </ugit:UserValueBox>
                        <asp:CustomValidator ID="cvPeditResource" ValidateEmptyText="true" ValidationGroup="SaveMe" runat="server" ControlToValidate="txtAllocation" OnServerValidate="cvPeditResource_ServerValidate" ErrorMessage="Please enter resource" Display="Dynamic"></asp:CustomValidator>
                   </asp:Panel>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trAllocation" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Allocation <b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField row">
                    <div class="col-xs-11 noPadding">
                        <dx:ASPxTextBox ID="txtAllocation" ValidationGroup="SaveMe" CssClass="asptextBox-input" Width="100%" runat="server" />
                    </div>
                    <div class="col-xs-1 noPadding">
                         <p style="color:#000; padding-left: 5px;margin-top: 7px;">%</p>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvTxtAllocation" Display="Dynamic" ValidationGroup="SaveMe" runat="server" ControlToValidate="txtAllocation" ErrorMessage="Please enter percentage allocation.">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvTxtAllocation" Display="Dynamic" ValidationGroup="SaveMe" runat="server" Type="Integer" ControlToValidate="txtAllocation" MinimumValue="1" MaximumValue="100" ErrorMessage="Please enter allocation percentage between 1-100."></asp:RangeValidator>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trStartDate" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Start Date</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxDateEdit ID="startDate" runat="server" CssClass="CRMDueDate_inputField"
                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-width="16"></dx:ASPxDateEdit>
                    <asp:Label ID="lbstartDate" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trEndDate" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">End Date</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxDateEdit ID="endDate" runat="server" CssClass="CRMDueDate_inputField"
                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-width="16"></dx:ASPxDateEdit>
                    <asp:Label ID="lbendDate" runat="server" Visible="false"></asp:Label>
                     <br />
                    <asp:CustomValidator ID="cvEndDate" ValidationGroup="SaveMe" ValidateEmptyText="true" runat="server" ControlToValidate="txtAllocation" OnServerValidate="cvEndDate_ServerValidate" ErrorMessage="Start date should be less then due date." Display="Dynamic"></asp:CustomValidator>
                </div>
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12 addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ValidationGroup="SaveMe" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>


 
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMAccomplishmentPlannedItem.ascx.cs" Inherits="uGovernIT.Web.PMMAccomplishmentPlannedItem" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function showMessage(obj) {
        if ($('#<%=btMoveToAccomplishment.ClientID%>').val().trim() == "Move to Planned")
        {
            return confirm('Are you sure you want to move this item to Planned?');
        }
        return true;
 }
    </script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
         <asp:Panel runat="server" ID="taskForm">
             <div class="row" id="trTitle" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader"><asp:Label ID="lblTitle" runat="server"></asp:Label><b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="AccomplishmentPlan"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="error" ID="rfvText" runat="server" ValidationGroup="AccomplishmentPlan" ControlToValidate="txtTitle"
                        Display="Dynamic" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
                </div>
             </div>
             <div class="row" id="trAccomplishmentDate" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Completed On</h3>
                </div>
                 <div class="ms-formbody accomp_inputField">
                     <dx:ASPxDateEdit ID="dtcAccomplishmentDate" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18px"
                         DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                 </div>
             </div>
             <div class="row" id="trDueDate" runat="server" visible="false">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Planned Date</h3>
                 </div>
                  <div class="ms-formbody accomp_inputField">
                     <dx:ASPxDateEdit ID="dtcDueDate" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18px"
                         DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                </div>
             </div>
             <div class="row" id="trNote" runat="server">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Note</h3>
                </div>
                 <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine"></asp:TextBox>
                </div>
             </div>
             <div class="row">
                 <ugit:HomeCardView />
             </div>

                  <div class="row" style="width:50%">
                       <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Attachment</h3>
                        </div>
                     <div class="ms-formbody accomp_inputField">
                         <ugit:FileUploadControl ID="FileUploadControl" runat="server" />
                      </div>
                 </div>
             <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton CssClass="secondary-cancelBtn" AutoPostBack="true" Visible="false"
                    OnClick="btDelete_Click" ID="btDelete" runat="server" Text="Delete Forever">
                    <ClientSideEvents Click="function(s, e) {if(!confirm('Are you sure you want to delete this archived item?')){e.processOnServer = false;}; }" />
                </dx:ASPxButton>
                <dx:ASPxButton CssClass="secondary-cancelBtn" AutoPostBack="true" Visible="false" 
                    OnClick="btMoveFromArchive_Click" ID="btMoveFromArchive" runat="server" Text="Unarchive">
                    <ClientSideEvents Click="function(s, e) { if(!confirm('Are you sure you want to unarchive this item?')){e.processOnServer = false;}; }" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" ToolTip="Cancel" ImagePosition="Right" CssClass="secondary-cancelBtn" 
                     OnClick="btCancel_Click"></dx:ASPxButton>
                 <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" Visible="false" OnClick="btArchiveAccomplishmentplan_Click" 
                     ID="btArchiveAccomplishmentplan" runat="server" Text="Archive">
                        <ClientSideEvents Click="function(s, e) { if(!confirm('Are you sure you want to archive this item?')){ e.processOnServer = false;}}" />
                </dx:ASPxButton>
                 <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" Visible="false" OnClick="btMoveToAccomplishment_Click" 
                    ID="btMoveToAccomplishment" runat="server" Text=""></dx:ASPxButton>
                 
                <dx:ASPxButton ID="ASPxButton1" ValidationGroup="AccomplishmentPlan" ToolTip="Save" runat="server" Text="Save" 
                     OnClick="btSaveAccomplishmentplan_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
             </div>
             <div class="row">
                    <asp:Label ID="lblCreatedBy" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblModifiedBy" runat="server"></asp:Label>
             </div>
         </asp:Panel>
    </div>
</div>
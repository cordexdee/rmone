<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMDecisionLogEdit.ascx.cs" Inherits="uGovernIT.Web.PMMDecisionLogEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
     $(document).ready(function () {
         $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
         $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
         $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
         $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
     });  
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap fetch-popupParent ">
    <asp:Panel runat="server" ID="taskForm">
        <div class="ms-formtable accomp-popup">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Decision Title<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtReleaseTitle" runat="server" ValidationGroup="DecisionLog"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvtxtReleaseTitle" runat="server" ControlToValidate="txtReleaseTitle" 
                            ErrorMessage="Please enter title" ForeColor="Red" ValidationGroup="DecisionLog" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Release Date</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtReleaseDate" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Sequence<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtItemOrder" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvItemOrder" runat="server" ControlToValidate="txtItemOrder" ValidationGroup="DecisionLog" 
                            ErrorMessage="Please enter sequence." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revtxtItemOrder" runat="server" ControlToValidate="txtItemOrder" ValidationExpression="^[0-9]*$"
                            ErrorMessage="Enter only number" ForeColor="Red" Display="Dynamic" ValidationGroup="DecisionLog"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Release ID</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtReleaseID" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Date Identified</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtDateIdentified" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Decision Status<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlDecisionStatus" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Text="Open"></asp:ListItem>
                            <asp:ListItem Text="In Progress"></asp:ListItem>
                            <asp:ListItem Text="Closed"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Source</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtDecisionSource" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Assigned To<b style="color: Red;"></b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:UserValueBox ID="peAssignedTo" runat="server" class="userValueBox-dropDown" IsMandatory="true" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Decision Maker<b style="color: Red;"></b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:UserValueBox ID="peDecisionMaker" runat="server" class="userValueBox-dropDown" IsMandatory="true"/>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Date Assigned</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtDateAssigned" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Decision</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtDecision" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Decision Date </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtDecisionDate" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Additional Comments</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtAdditionalComments" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row" style="padding-top:10px;">
                <div class="col-md-12 col-sm-12 col-xs-12" style="padding-left:20px;">
                    <ugit:UploadAndLinkDocuments runat="server" id="UploadAndLinkDocuments" />
                    <div id="bindMultipleLink" runat="server"></div>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                <div class="col-md-12 col-sm-12 col-xs-12" style="padding-left:20px;">
                    <dx:ASPxButton CssClass="secondary-cancelBtn" AutoPostBack="false" ID="btCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton CssClass="secondary-cancelBtn" AutoPostBack="true"  Visible="false" OnClick="btDelete_Click" ID="btDelete" runat="server"
                        Text="Delete Forever">
                        <ClientSideEvents Click="function(s, e) {if(!confirm('Are you sure you want to delete this archived decision log?')){e.processOnServer = false;}; }" />
                    </dx:ASPxButton>
                    <dx:ASPxButton CssClass="secondary-cancelBtn" AutoPostBack="true" Visible="false" OnClick="btArchiveDecisionLog_Click"
                        ID="btArchiveDecisionLog" runat="server" Text="Archive">
                        <ClientSideEvents Click="function(s, e) { if(!confirm('Are you sure you want to archive this decision log?')){ e.processOnServer = false;}}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" Visible="false" OnClick="btUnArchiveDecisionLog_Click" 
                        ID="btUnArchiveDecisionLog" runat="server" Text="Unarchive">
                        <ClientSideEvents Click="function(s, e) { if(!confirm('Are you sure you want to Unarchive this decision log?')){ e.processOnServer = false;}}" />
                    </dx:ASPxButton>
                    <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" ValidationGroup="DecisionLog" ID="btSaveDecision" runat="server" 
                        OnClick="btSaveDecision_Click" Text="Save" >
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

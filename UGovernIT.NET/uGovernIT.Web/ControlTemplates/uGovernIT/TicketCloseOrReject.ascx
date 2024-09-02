
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketCloseOrReject.ascx.cs" Inherits="uGovernIT.Web.TicketCloseOrReject" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(function () {
        $("#<%= ddlActionType.ClientID%>").change(function () {
            var selectedVal = $(this[this.selectedIndex]).val();
            if (selectedVal == "Close") {

                $("#<%= spanComments.ClientID%>").show();

                if ($("#<%= tr_Type.ClientID%>").attr("showCtrl") == "true")
                    $("#<%= tr_Type.ClientID%>").show();
                else $("#<%= tr_Type.ClientID%>").hide();

                if ($("#<%= tr_ActualHrs.ClientID%>").attr("showCtrl") == "true")
                    $("#<%= tr_ActualHrs.ClientID%>").show();
                else $("#<%= tr_ActualHrs.ClientID%>").hide();
            }
            else {
                $("#<%= tr_ActualHrs.ClientID%>").hide();
                $("#<%= tr_Type.ClientID%>").hide();
                $("#<%= spanComments.ClientID%>").hide();
            }
        });
    });
    $(document).ready(function () {
        $('.popupWrap').parent().addClass("popup-container commentPopup-wrap");
    });
</script>


<div class=" col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="tr_dropdown" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Action Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">            
                <asp:HiddenField ID="hndUserType" runat="server"></asp:HiddenField>
                <asp:DropDownList ID="ddlActionType" runat="server" AutoPostBack="false" CssClass="itsmDropDownList aspxDropDownList" 
                    OnSelectedIndexChanged="ddlActionType_SelectedIndexChanged">
                    <asp:ListItem Value="Close">Close with Resolution</asp:ListItem>
                    <asp:ListItem Value="Reject">Cancel (Reject)</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row" id="tr_ActualHrs" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Actual Hours</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtActualHours" runat="server" Text="0"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtActualHours"
                        ErrorMessage="* Incorrect Actual Hours" CssClass="text-error" Display="Dynamic" ValidationGroup="TicketCancelClose"
                        ValidationExpression="\d+(.\d{1,2})?"></asp:RegularExpressionValidator>
                </div>
            </div>
        </div>
        <div class="row" id="tr_Type" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Resolution Type<span style="color:red;">*</span></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlResolutionType" runat="server" CssClass=" itsmDropDownList aspxDropDownList"></asp:DropDownList>
            </div>
        </div>

        <div class="row">
            <div class="ms-formlabel dropDown-fieldLabel">
                <h3 class="ms-standardheader budget_fieldLabel"><span id="spanComments" runat="server">Resolution</span> Comments</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" TextMode="MultiLine" Height="75px" ></asp:TextBox> 
            </div>
        </div>
    
        <div class="comment_checkboxes">
            <asp:RequiredFieldValidator ID="rfvComments" runat="server" ControlToValidate="txtComment"
                ErrorMessage="* Enter Comments" CssClass="text-error" Display="Dynamic" ValidationGroup="TicketCancelClose">
            </asp:RequiredFieldValidator>
        </div>
      
        <div class="row" id="tr_Checkboxes" runat="server" style="padding-top:15px" visible="false"> 
            <div class="crm-checkWrap">
                <asp:CheckBox ID="chkAddPrivate" runat="server" Text="Private"/>
                <asp:CheckBox ID="chkNotifyRequestor" runat="server" Text="Notify Requestor" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click" ></dx:ASPxButton>
            <dx:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" OnClick="btnUpdate_Click" 
                ValidationGroup="TicketCancelClose"></dx:ASPxButton>
        </div> 
    </div>
</div>
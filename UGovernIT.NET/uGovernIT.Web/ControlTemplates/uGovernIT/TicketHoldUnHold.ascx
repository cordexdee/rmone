

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketHoldUnHold.ascx.cs" Inherits="uGovernIT.Web.TicketHoldUnHold" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {

        var date = new Date();
        // add a day
        date.setDate(date.getDate() + 1);
        aspxdtOnHoldDate.SetDate(date);
    });
    function hideddlOnHoldReason(action) {
        var category = $("#<%=ddlOnHoldReason.ClientID%> option:selected").text();
        $(".divddlOnHoldReason").hide();
        $("#<%=hdnOnHoldReason.ClientID%>").val('1');
        if (action == 1) {
            $("#<%=hdnRequestOnHoldReason.ClientID%>").val(category);
            $("#<%=txtOnHoldReason.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestOnHoldReason.ClientID%>").val("");
            $("#<%=txtOnHoldReason.ClientID%>").val("");
        }
    }
    function showddlOnHoldReason() {
        $("#<%=hdnOnHoldReason.ClientID%>").val('0');
        $(".divddlOnHoldReason").show();
    }
    function validateFeedbackForm(obj) {
        holdUnHoldLoadingPanel.Show();

        if (obj.globalName == "Hold") {
            var isValid = true;

            var errorMsgObj = $("#<%=lblHoldMessage.ClientID%>");
            var errors = new Array();

            var onHoldReason = $("#<%= ddlOnHoldReason.ClientID%>").val();
            if (onHoldReason == "Other" && $('#<%=popedHoldComments.ClientID%>').val() == "") {
                errors.push("Please enter comment");
                isValid = false;
            }

            if (aspxdtOnHoldDate.date == null) {
                errors.push("Please enter the Hold Till date");
                isValid = false;
            }

            //if (isValid) {
            //    commentsHoldPopup.Hide();
            //}
            else {
                if (errors.length == 1) {
                    errorMsgObj.html(errors.join(""));
                }
                else if (errors.length > 1){
                    errorMsgObj.html("1. " + errors[0] + ", 2. " + errors[1]);
                }
                return isValid;
            }
        }
        else {
             //waitTillUpdateComplete();
        }

       
    }
</script>

<dx:ASPxLoadingPanel ID="holdUnHoldLoadingPanel" ClientInstanceName="holdUnHoldLoadingPanel" Modal="True" runat="server"  Text="Please Wait..."></dx:ASPxLoadingPanel>

<asp:HiddenField ID="hdnOnHoldReason" runat="server" Value="0" />
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row mt-2">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel" style="white-space:pre-line;line-height:17px;"><asp:Label ID="lblMsg"  runat="server" Text=""> </asp:Label></h3>
            </div>
        </div>
        <div class="row">
         <div id="trHoldMessage" runat="server">
		    <asp:Label ID="lblHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label>
        </div>
    </div>
        <div class="row mt-2">
        <div class="col-md-6 col-sm-6 col-xs-6 noLeftPadding" id="trHoldTill" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Hold Till<span class="red-star"> *</span></h3>
            </div>
			<div class="ms-formbody accomp_inputField">
                <dx:ASPxDateEdit ID="aspxdtOnHoldDate" CalendarProperties-ShowTodayButton="false" CalendarProperties-ShowClearButton="false" runat="server" DisplayFormatString="d" EditFormatString="d" ClientInstanceName="aspxdtOnHoldDate" CssClass="CRMDueDate_inputField" 
                    DropDownApplyButton-Image-Url="~/Content/Images/calendarNew.png">
                </dx:ASPxDateEdit>
			</div>
		</div>
         <div class="col-md-6 col-sm-6 col-xs-6 noRightPadding" id="trHoldReason" runat="server">
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Reason</h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 <div class="divddlOnHoldReason" id="divddlOnHoldReason" runat="server" style="float: left; width: 100%;">
                                <div class="col-xs-10 noPadding">
                                    <asp:DropDownList ID="ddlOnHoldReason" onchange="hideShowEdit('ddlOnHoldReason')" runat="server" EnableViewState="true" class="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                </div>
                                <div class="col-xs-2 noPadding mt-2">
                                    <img alt="Edit Category" runat="server" class="editicon" id="btCategoryEdit"
                                        src="/content/images/editNewIcon.png" width="16" style="cursor: pointer; position: relative; float: right;"
                                        onclick="javascript:$('.divOnHoldReason').attr('style','display:block');hideddlOnHoldReason(1)" />
                                    <img alt="Add Category" id="Img1" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right; margin-right: 10px;"
                                        onclick="javascript:$('.divOnHoldReason').attr('style','display:block');hideddlOnHoldReason(0);" />
                                </div>

                            </div>
                            <div runat="server" id="divOnHoldReason" class="divOnHoldReason" style="display: none; float: left;">
                                <div class="col-xs-10 noPadding">
                                    <asp:TextBox CssClass="form-control" ID="txtOnHoldReason" runat="server"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnRequestOnHoldReason"></asp:HiddenField>
                                </div>
                                <div class="col-xs-2 noPadding mt-2">
                                    <img alt="Cancel Category" style="float: right" width="16" src="/content/images/close-blue.png" class="cancelModule"
                                        onclick="javascript:$('.divOnHoldReason').attr('style','display:none');showddlOnHoldReason();" />
                                </div>
                            </div>
             </div>
         </div>
    </div>
        <div class="row">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Additional Comments</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <asp:TextBox runat="server" ID="popedHoldComments" TextMode="MultiLine" Height="80" Text="" CssClass="form-control"></asp:TextBox>
            <asp:TextBox runat="server" ID="popedUnHoldComments" TextMode="MultiLine" Height="100" Text="" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="Cancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="Cancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="UnHoldButton" runat="server" CssClass="primary-blueBtn" Text="Remove Hold" ToolTip="Remove Hold" ClientInstanceName="UnHold" OnClick="UnHoldButton_Click">
                <ClientSideEvents Click="function(s, e){return validateFeedbackForm(s);}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="HoldButton" runat="server" Text="Put On Hold" CssClass="primary-blueBtn" ToolTip="Put On Hold" ClientInstanceName="Hold" OnClick="HoldButton_Click" >
                <ClientSideEvents Click="function(s, e){return validateFeedbackForm(s);}" />
            </dx:ASPxButton>
        </div>
    </div>
</div>
    
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhraseAddNew.ascx.cs" Inherits="uGovernIT.Web.PhrasesAddNew" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script Type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function changeRequestType(sender) {
        var moduleid = $('#<%=ddlPhraseTicketType.ClientID %>').val();
        pnlRequestTypeCustom.PerformCallback('ValueChanged~' + moduleid);
    }


     function changeAgentType(sender) {
        var agentType = $('#<%=ddlPhrasesAgentType.ClientID %>').val();
        //alert(agentType);
         if (agentType == 3) {
             //debugger;
            $('#TicketType').show();
            $('#<%=ddlPhraseTicketType.ClientID %>').attr('disabled', true);
            if ($('#<%=ddlPhraseTicketType.ClientID %> option').text() == 'Shared Services (SVC)') {
                $('#<%=ddlPhraseTicketType.ClientID %> option').attr('selected', 'selected');
            }
            $('#RequestType').hide();
            $('#services').show();

        }
        else {
            // alert(agentType);
            $('#TicketType').show();
            $('#<%=ddlPhraseTicketType.ClientID %>').attr('disabled', false);
            $('#RequestType').show();
            $('#services').hide();

        }
     }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup ">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Phrase Title</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtPhrase" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Agent</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                     <asp:DropDownList ID="ddlPhrasesAgentType" runat="server" onchange="javascript:changeAgentType(this);" CssClass="aspxDropDownList itsmDropDownList">
                     </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="TicketType">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Ticket Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlPhraseTicketType" runat="server" autopostback="false" onchange="javascript:changeRequestType(this);" 
                        CssClass="aspxDropDownList itsmDropDownList"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6" id="RequestType">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Request Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div id ="divRequestType" runat="server"></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="services">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Services</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="DDLservices" runat="server" autopostback="false" CssClass="aspxDropDownList itsmDropDownList"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel" ID="lblUserGroups" runat="server">wiki article</h3>
                </div>
                <div class="ms-formbody accomp_inputField" id="userGroupList" runat="server">
                    <dx:ASPxTokenBox ID="wikiTokenBox" runat="server" Width="100%" CssClass="aspxUserTokenBox-control " ></dx:ASPxTokenBox>
                </div>lblUserGroups
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel" ID="Label1" runat="server">Help cards</h3>
                </div>
                <div class="ms-formbody accomp_inputField" id="Div1" runat="server">
                    <dx:ASPxTokenBox ID="helpCardTokenBox" runat="server" Width="100%" CssClass="aspxUserTokenBox-control " ></dx:ASPxTokenBox>
                </div>
            </div>
           
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
        </div>
    </div>
</div>  
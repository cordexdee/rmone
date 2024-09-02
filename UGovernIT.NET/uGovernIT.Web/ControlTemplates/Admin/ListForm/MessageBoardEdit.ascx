<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBoardEdit.ascx.cs" Inherits="uGovernIT.Web.MessageBoardEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;*/
        /*border-top: 1px solid #A5A5A5;*/
        padding: 3px 6px 4px;
        /*vertical-align: top;*/
    }

    /*.ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
        padding-top:7px;
    }

    .ms-standardheader {
        text-align: right;
    }
    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .messagetypes {
    }

    .messagetypes td {
        padding-bottom: 2px;
        padding-right: 6px;
    }
    .messagetypes td > input {
        float:left;
        position:relative;
        top:2px;
    }
   .messagetypes td > label {
          float:left;
    }
    .messagetypes td > label > img {
        float: left;
    }
    .messagetypes td > label > label {
        float: left;
        padding-left: 2px;
        padding-top: 1px;
    }*/
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>"> 
    function LnkbtnDelete_Click(s, e) {
        if (confirm('Are you sure you want to delete?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }

    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding my-2">
    <div class="ms-formtable accomp-popup">
         <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Message Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:RadioButtonList ID="rbMessageTypeList"
                    runat="server"                       
                    RepeatColumns="4"
                    RepeatLayout="Table" CssClass="messagetypes custom-radiobuttonlist">
                    <asp:ListItem Text ="<label class='msgRadio-btnLabel'>Ok</label><img class='msgRadio-btnImg' src='/Content/Images/message_good.png'/>" Value="Ok"></asp:ListItem>
                    <asp:ListItem Text ="<label class='msgRadio-btnLabel'>Information</label><img class='msgRadio-btnImg' src='/Content/Images/message_information.png'/>" Value="Information"></asp:ListItem>
                    <asp:ListItem Text ="<label class='msgRadio-btnLabel'>Warning</label><img class='msgRadio-btnImg' src='/Content/Images/message_warning.png'/>" Value="Warning"></asp:ListItem>
                    <asp:ListItem Text ="<label class='msgRadio-btnLabel'>Critical</label><img class='msgRadio-btnImg' src='/Content/Images/message_critical.png'/>" Value="Critical"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>     
         <div class="row" >
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Message<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtBody" TextMode="MultiLine" runat="server" />
                 <div>                  
                    <asp:CustomValidator runat="server" ID="cusCustom" ValidationGroup="Save" ControlToValidate="rbMessageTypeList" OnServerValidate="cusCustom_ServerValidate" ErrorMessage="Please enter valid message." />
                </div>
            </div>
        </div>
        <div class="row" >
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Authorized To</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <ugit:UserValueBox ID="peAuthorizedTo" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
              <%--  <SharePoint:PeopleEditor PrincipalSource="UserInfoList"  ID="peAuthorizedTo" MaximumHeight="30" 
                                         SelectionSet="User,SPGroup" runat="server" MultiSelect="true" PlaceButtonsUnderEntityEditor="false" AugmentEntitiesFromUserInfo="false" />--%>
                <asp:Label ID="lbAuthorizedTo" runat="server" Visible="false"></asp:Label>           
            </div>
        </div>
        <div class="row" >
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Expires</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                  <dx:ASPxDateEdit ID="dtcExpires" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18" 
                      DropDownButton-Image-Url="~/Content/Images/calendarNew.png"></dx:ASPxDateEdit>
                <%-- <SharePoint:DateTimeControl OnValueChangeClientScript="dateChanged()" DateOnly="true"
                            ID="dtcExpires" runat="server" CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit">
                        </SharePoint:DateTimeControl>--%>
                    <asp:Label ID="lbStartDate" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="d-flex justify-content-between align-items-center px-1">
            <dx:ASPxButton ID="LnkbtnDelete" runat="server" Text="Delete" CssClass="btn-danger1" ToolTip="Delete" OnClick="LnkbtnDelete_Click" AutoPostBack="false">
                <ClientSideEvents Click="LnkbtnDelete_Click" />                        
            </dx:ASPxButton>
            <div>
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" CssClass="primary-blueBtn" OnClick="btnSave_Click"></dx:ASPxButton>
            </div>
        </div>
    </div>
</div>
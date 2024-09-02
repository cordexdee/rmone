

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaveTaskTemplates.ascx.cs" Inherits="uGovernIT.Web.SaveTaskTemplates" %>

<%--<style type="text/css">
      
        .ms-formbody
        {
            background: none repeat scroll 0 0 #E8EDED;
            border-top: 1px solid #A5A5A5;
            padding: 3px 6px 4px;
            vertical-align: top;
        }
        .pctcomplete{ text-align:right;}
        .estimatedhours{text-align:right;}
        .actualhours{text-align:right;}
        .full-width{width:98%;}
        .ms-formlabel{width:100px;}
        .ms-standardheader{text-align:right;}
        .fleft
        {
            float: left;
        }
          .proposeddatelb
     {
         padding-top:5px;padding-right:4px;
         float:left;
     }
    </style>--%>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="ms-formlabel">
                <h3 class=" ms-standardheader budget_fieldLabel ">
                    <asp:Label ID="lbMessage" runat="server" ForeColor="Red"></asp:Label>
                </h3>
            </div>
        </div>
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Template Name<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Task"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                    Display="Dynamic" ErrorMessage="Please enter template name."></asp:RequiredFieldValidator>
                <asp:CustomValidator ValidationGroup="Task" ID="cvTxtTitle" ErrorMessage="Template with this name already exists" runat="server" ControlToValidate="txtTitle" OnServerValidate="cvTxtTitle_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div class="row" id="trNote" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" ClientInstanceName="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(){ window.frameElement.commitPopup(); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="Task" ID="btSaveAsTemplate" ClientInstanceName="btSaveAsTemplate" Visible="true" runat="server"
                Text="Save As Template" ToolTip="Save as Template" OnClick="btSaveAsTemplate_Click" CssClass=" primary-blueBtn">
            </dx:ASPxButton>
        </div>
    </div>
</div>

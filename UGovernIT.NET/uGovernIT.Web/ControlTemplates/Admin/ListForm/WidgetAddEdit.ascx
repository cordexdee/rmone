<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WidgetAddEdit.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.WidgetAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                       <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
                  </div>
                 <div class="ms-formbody accomp_inputField""> 
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">  
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                     <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField"> 
                    <asp:TextBox ID="txtDescription" runat="server" Rows="5"  TextMode="MultiLine"></asp:TextBox>
                </div>       
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6" id="trIconUrl" runat="server" >
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Icon Url</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UGITFileUploadManager ID="iconUploader" width="100%" runat="server" AnchorLabel="Upload Icon" hideWiki="true" />
                </div>
             </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                 <div class="ms-formlabel">
                     <h3 class="ms-standardheader budget_fieldLabel">Parameter</h3>
                </div>
                <div class="ms-formbody accomp_inputField""> 
                    <asp:TextBox ID="txtParameter" runat="server"  ></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Control</h3>
                 </div>
                <div class="ms-formbody accomp_inputField""> 
                    <asp:TextBox ID="txtControl" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6">
                <div class="ms-formlabel">
                   <h3 class="ms-standardheader budget_fieldLabel">Base Url</h3>
                </div>
                <div class="ms-formbody accomp_inputField""> 
                    <asp:TextBox ID="txtBaseUrl" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
         <div class="row">
             <div class="col-md-12 col-sm-12 col-xs-12" id="services">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Services</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="DDLservices" runat="server" autopostback="false" CssClass="aspxDropDownList itsmDropDownList" 
                        OnSelectedIndexChanged="DDLservices_SelectedIndexChanged"></asp:DropDownList>
              </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6">
                 <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">height</h3>
                  </div>
                  <div class="ms-formbody accomp_inputField">
                       <asp:TextBox ID="txtHeight" runat="server"></asp:TextBox>
                  </div>
                </div>
             <div class="col-md-6 col-sm-6 col-xs-6">
                  <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Width</h3>
                  </div>
                  <div class="ms-formbody accomp_inputField">
                       <asp:TextBox ID="txtWidth" runat="server"></asp:TextBox>
                  </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
             <dx:ASPxButton ID="btnDelete" runat="server" Text="Delete" CssClass="secondary-cancelBtn" OnClick="btnDelete_Click" Visible="false">
                    <ClientSideEvents Click="function(s,e){ if(!confirm('Are you sure you want to delete?')){e.processOnServer = false;}; }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
           
        </div>
    </div>
</div>
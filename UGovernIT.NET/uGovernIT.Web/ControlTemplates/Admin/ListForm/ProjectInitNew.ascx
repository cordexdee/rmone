
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectInitNew.ascx.cs" Inherits="uGovernIT.Web.ProjectInitNew" %>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel"><%=BSTitle %></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:LookUpValueBox ID="ddlBusinessStrategy" runat="server" CssClass="lookupValueBox-dropown" Width="100%" FieldName="BusinessStrategyLookup" />
            </div>
        </div>
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;"></b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxTextBox ID="txtTitle" runat="server" Width="100%" CssClass="asptextBox-input" >
                    <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Please Enter Title" ErrorDisplayMode="ImageWithText" ErrorText="Please Enter Title" 
                        ValidationGroup="Save" >
                    </ValidationSettings>
                </dx:ASPxTextBox>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Project Note</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtProjectNote" TextMode="MultiLine" CssClass="asptextbox-asp" runat="server" Width="100%"/>
            </div>
        </div>
        <div class="row" id="tr11" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div class="ms-formbody crm-checkWrap" style="padding-left:5px;">
                <asp:CheckBox ID="chkDeleted" runat="server" TextAlign="Right" Text="(Prevent use for new item)" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" CssClass=" secondary-cancelBtn" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" ></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" CssClass="primary-blueBtn" runat="server" ClientInstanceName="btnSave" Text="Save" ToolTip="Save" ValidationGroup="Save"
                    OnClick ="btnSave_Click">
                <ClientSideEvents Click="function(s, e){
                    if(ASPxClientEdit.ValidateGroup('Save')){
                        btnSave.DoClick();s
                    }
                    }" />
            </dx:ASPxButton>
        </div>
    </div>
</div>

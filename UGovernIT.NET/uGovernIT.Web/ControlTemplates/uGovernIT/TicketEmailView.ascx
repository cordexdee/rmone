<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketEmailView.ascx.cs" Inherits="uGovernIT.Web.TicketEmailView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .fleft {
        float: left;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: central;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: central;
    }

    .ms-formline {
        border-top: 1px solid #A5A5A5;
        padding-left: 8px;
        padding-right: 8px;
    }

    .existing-section-c {
        float: left;
    }

    .new-section-c {
        float: left;
    }

    .existing-section-a {
        float: left;
        padding: 0px 5px;
    }

        .existing-section-a img {
            cursor: pointer;
        }

    .new-section-a {
        float: left;
        padding-left: 5px;
    }

        .new-section-a img {
            cursor: pointer;
        }

    .ms-standardheader {
        text-align: right;
    }

    .auto-style1 {
        border-top: 1px solid rgb(165, 165, 165);
        text-align: right;
        height: 30px;
        padding-right: 8px;
    }


    .auto-style2 {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        height: 30px;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    #tdbutton1 ul li, #tdbutton2 ul li {
        color: #FFFFFF;
        cursor: pointer;
        display: inline;
        float: left;
        list-style: none outside none;
        margin: 0 1px;
        overflow: hidden;
        padding: 0;
        text-align: center;
        width: auto;
    }
</style>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">To:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" Enabled="false" ID="txtEmailTo"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">CC:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtEmailCC" Enabled="false"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Subject:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtSubject" Enabled="false"></asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Body:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxHtmlEditor ID="htmlEditorTicketEmailBody" runat="server" Width="100%" Height="300px"  Enabled="false" 
                    OnHtmlCorrecting="htmlEditorTicketEmailBody_HtmlCorrecting">
                    <Settings AllowHtmlView="false" AllowPreview="false"/>
                    <Toolbars> <dx:HtmlEditorToolbar Visible="false"></dx:HtmlEditorToolbar></Toolbars>
                </dx:ASPxHtmlEditor>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Attachment(s):</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                  <asp:Panel ID="pAttachmentContainer" runat="server" CssClass="attachment-container" Visible="true">
                        <div class="attachmentform">
                            <asp:Panel ID="pAttachment" runat="server" CssClass="oldattachment">
                            </asp:Panel>
                            <asp:Panel ID="pNewAttachment" runat="server" CssClass="newattachment">
                            </asp:Panel>
                        </div>
                    </asp:Panel>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="lnkDelete" runat="server" Text="Delete" ToolTip="Delete" CssClass="secondary-cancelBtn" OnClick="lnkDelete_Click">
                <ClientSideEvents Click="function(s,e){ if(!confirm('Are you sure you want to delete?')){e.processOnServer = false;}; }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
        </div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomEmailNotification.ascx.cs" Inherits="uGovernIT.Web.CustomEmailNotification" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

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

<div style="float: left; width: 98%; padding-left: 10px;">
    <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12">
        <div class="col-md-6 col-sm-6 colForXS" id="tremailto" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">To<b style="color: Red;">*</b>:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtEmailTo" Width="85%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmailTo" ControlToValidate="txtEmailTo" runat="server" ErrorMessage="Field required." Display="Dynamic" ValidationGroup="SendTicketEmail"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmailTo" ControlToValidate="txtEmailTo" runat="server" ErrorMessage="Invalid Email." Display="Dynamic" ValidationGroup="SendTicketEmail" ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,;\s])*)*$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 colForXS" id="tremailcc" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">CC:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtEmailCC" Width="85%"></asp:TextBox>
                <asp:RegularExpressionValidator ID="rfvEmailCC" ControlToValidate="txtEmailCC" runat="server" ErrorMessage="Invalid Email." Display="Dynamic" ValidationGroup="SendTicketEmail" ValidationExpression="^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,;\s])*)*$"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 colForXS">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Subject:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox runat="server" ID="txtSubject" Width="85%"></asp:TextBox></div>
        </div>
        <div class="row emailNotify-content">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Body:</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                
                    <dx:ASPxHtmlEditor ID="htmlEditorTicketEmailBody" runat="server" Width="100%" Height="300px" Theme="DevEx" SettingsAdaptivity-Enabled="true">
                        <Settings AllowHtmlView="false" AllowPreview="false" />
                    </dx:ASPxHtmlEditor>
                </div>
            </div>
        </div>
        
         
        <div class="row externalTeam-popupBtn" id="tractionbuttons" runat="server">
            <div class="saveDelete_btnWrap">
                <div class="popupBtn_Cancel">
                    <asp:LinkButton ID="btnCancel" runat="server" Text="Cancel"
                        ToolTip="Cancel" OnClick="btnCancel_Click" >
                        <span class="cancelBtn">
                            <b>Cancel</b>
                            <%--<i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/Content/images/uGovernIT/ButtonImages/cancelwhite.png"  style="border:none;" title="" alt=""/>
                            </i> --%>
                        </span>
                    </asp:LinkButton>
                </div>
                <div class="activitySave_btnWrap">
                    <asp:LinkButton ID="btnSave" runat="server" Text="Send" ToolTip="Send Email" ValidationGroup="SendTicketEmail" OnClick="btnSave_Click">
                        <span class="activitySave_btn">
                            <b>&nbsp;&nbsp;Send&nbsp;</b>
                            <%--<i style="float: left; position: relative; left:2px">
                                <img src="/Content/images/uGovernIT/MailTo.png"  style="border:none;" title="" alt=""/>
                            </i> --%>
                        </span>
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

</div>
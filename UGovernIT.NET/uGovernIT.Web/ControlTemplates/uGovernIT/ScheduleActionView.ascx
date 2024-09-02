<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScheduleActionView.ascx.cs" Inherits="uGovernIT.Web.ScheduleActionView" %>

<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>


<%--<style type="text/css">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 140px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .error {
        color: red;
    }
</style>--%>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <fieldset>
        <legend>Schedule</legend>
        <div class="ms-formtable accomp-popup">
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Schedule Action Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblScheduleActionType" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row" id="trTitle" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblScheduleTitle" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Ticket ID</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblTicketId" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Start Time</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblStartTime" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email To</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblScheduleEmailTo" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email CC</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblScheduleEmailCC" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email Subject</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="lblScheduleSubject" runat="server" ></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Email Body</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:Label ID="txtScheduleEmailBody" runat="server"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Recurring</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkScheduleRecurring" runat="server" Enabled="false" />
                    <table id="recurrTable" runat="server" visible="false">
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader">Recurring Interval
                                </h3>
                            </td>
                            <td class="ms-formbody">
                                <span>
                                    <asp:Label ID="lblScheduleRecurrInterval" runat="server"></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader">Recurring End Date
                                </h3>
                            </td>
                            <td class="ms-formbody">
                                <asp:Label ID="lblRecurrEndDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </fieldset>

    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding addEditPopup-btnWrap">
            <dx:ASPxButton ID="lnkDelete" runat="server" Text="Delete" ToolTip="Delete" OnClick="lnkDelete_Click" CssClass="btn-danger1">                   
                <ClientSideEvents Click="function(s,e){return confirm('Are you sure want to delete?');}" />
            </dx:ASPxButton>
             <dx:ASPxButton ID="lnkCancel" runat="server" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click" ToolTip="Cancel" Text="Cancel" ></dx:ASPxButton>
        </div>
    </div>
</div>


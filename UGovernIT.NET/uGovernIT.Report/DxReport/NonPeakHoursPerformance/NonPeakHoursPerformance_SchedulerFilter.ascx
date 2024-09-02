<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonPeakHoursPerformance_SchedulerFilter.ascx.cs" Inherits="uGovernIT.Report.DxReport.NonPeakHoursPerformance_SchedulerFilter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<style type="text/css">
    .padding {
        padding-left: 12px;
    }

    .fleft {
        float: left;
    }

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

    .primary-blueBtn {
        background: none;
        border: none;
    }

        .primary-blueBtn .dxb {
            background: #4A6EE2;
            color: #FFF;
            border: none;
            border-radius: 4px;
            padding: 3px 13px 2px 13px !important;
            font-size: 12px;
            font-family: 'Poppins', sans-serif;
            font-weight: 500;
        }

    .secondary-cancelBtn {
        background: none;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        margin-right: 5px;
        margin-top: 1px;
    }

    .dxbButton_UGITNavyBlueDevEx.secondary-cancelBtn .dxb {
        padding: 0px 10px;
        color: #4A6EE2;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .addEditPopup-btnWrap {
        float: right;
        padding: 10px 0px 20px;
        text-align: right;
    }
</style>
<div style="float: left; width: 98%; padding-left: 0px;">
    <fieldset>
        <legend>Non-Peak Hours Performance
        </legend>
        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="100%">
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Date Range</h3>
                </td>
                <td class="ms-formbody">
                    <div style="left: 0px; float: left; margin-top: 6px;">
                        <span id="spanNoOfDays" runat="server" style="float: left; width: 100%; margin: 5px; margin-left: 0px">
                            <span style="float: left">
                                <dx:ASPxSpinEdit id="txtValueFrom" maxvalue="0" minvalue="-100" width="55px" runat="server"></dx:ASPxSpinEdit>
                            </span>
                            <span style="float: left">
                                <dx:ASPxComboBox id="ddlDateUnitsFrom" width="60px" runat="server">
                                            <Items>
                                                <dx:ListEditItem Text="Days" Selected="true" Value="0" />
                                                <dx:ListEditItem Text="Weeks" Value="1" />
                                                <dx:ListEditItem Text="Months" Value="1" />
                                            </Items>
                                        </dx:ASPxComboBox>

                            </span>
                        </span>
                        <span id="spanDateRange" runat="server">
                            <%--<SharePoint:DateTimeControl CssClassTextBox="inputTextBox datetimectr111" DateOnly="true" ID="dtHelpDeskPerformance" runat="server" ToolTip="" />--%>
                        </span>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Non-Peak Hours Window</h3>
               
                </td>
                <td class="ms-formbody">
                    <dx:ASPxSpinEdit width="50px" id="txtNPHWindow" clientinstancename="txtNPHWindow" runat="server"></dx:ASPxSpinEdit>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Working Hours</h3>
                </td>
                <td class="ms-formbody">
                    <div style="width: 100%; left: 0px; float: left; margin-top: 6px;">
                        <div style="float: left">
                            <dx:ASPxTimeEdit id="dtWorkingHoursStart" clientinstancename="dtWorkingHoursStart" runat="server" cssclass="setalign" width="100">
                                        <ClearButton DisplayMode="OnHover"></ClearButton>
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                                        <ClientSideEvents />
                                    </dx:ASPxTimeEdit>
                        </div>

                        <span id="tospan" runat="server" style="margin-left: 3px; margin-right: 3px; float: left; padding-top: 3px;">To</span>
                        <div style="float: left">
                            <dx:ASPxTimeEdit id="dtWorkingHoursEnd" clientinstancename="dtWorkingHoursEnd" runat="server" width="100">
                                        <ClearButton DisplayMode="OnHover"></ClearButton>
                                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />

                                    </dx:ASPxTimeEdit>
                        </div>

                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>&nbsp;
                </td>
                <td style="text-align: right;">
                    <div class="addEditPopup-btnWrap">
                        <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click">
                        </dx:ASPxButton>
                        <dx:ASPxButton id="lnkSubmit" runat="server" text="Schedule Report" CssClass="primary-blueBtn" onclick="lnkSubmit_Click">
                        </dx:ASPxButton>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</div>

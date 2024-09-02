<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonPeakHoursPerformance_SchedulerFilter.ascx.cs" Inherits="uGovernIT.DxReport.NonPeakHoursPerformance_SchedulerFilter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<style type="text/css">
    body {
        background-color: #fff;
    }
    legend {
        width:auto;
        border:none;
        font-size: 12px;
        font-weight: bold !important;
        margin-bottom: 5px !important;
        padding:0px 4px;
    }
    .padding {
        padding-left: 12px;
    }

    .fleft {
        float: left;
    }

    .ms-formbody {
        padding-right: 10px;
        padding-top: 10px;
    }

    .ms-formlabel {
        text-align: right;
        padding-right: 10px;
        padding-top: 10px;
        width: 150px;
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
        padding: 5px 13px;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .dxbButton_UGITGreenDevEx div.dxb {
	    padding: 6px 14px;
        border: none;
    }

    .secondary-cancelBtn {
        background: none;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        margin-right: 5px;
        padding: 0;
    }

    .dxbButton_UGITNavyBlueDevEx.secondary-cancelBtn .dxb {
        padding: 0px 10px;
        color: #4A6EE2;
        font-size: 12px;
        font-family: 'Poppins', sans-serif;
        font-weight: 500;
    }

    .addEditPopup-btnWrap {
        padding-top: 10px;
    }

    .filterBlock {
        padding: 10px;
    }
    .pl10 {
        padding-left: 10px;
    }
</style>
<div class="filterBlock">
    <fieldset>
        <legend>Non-Peak Hours Performance</legend>
        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="100%">
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Date Range</h3>
                </td>
                <td class="ms-formbody">
                    <div>
                        <span id="spanNoOfDays" runat="server" style="float: left; width: 100%; margin: 5px; margin-left: 0px">
                            <span style="float: left">
                                <dx:ASPxSpinEdit id="txtValueFrom" maxvalue="0" minvalue="-100" width="60" runat="server"></dx:ASPxSpinEdit>
                            </span>
                            <span class="pl10" style="float: left">
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
                    <dx:ASPxSpinEdit width="60" id="txtNPHWindow" clientinstancename="txtNPHWindow" runat="server"></dx:ASPxSpinEdit>
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

                        <span id="tospan" class="pl10" runat="server" style="margin-right: 3px; float: left; padding-top: 3px;">To</span>
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

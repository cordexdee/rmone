<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyFeedbackReport_SchedulerFilter.ascx.cs" Inherits="uGovernIT.DxReport.SurveyFeedbackReport_SchedulerFilter" %>
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
    .ms-formbody {
        padding-left: 10px;
        padding-top: 10px;
    }

    .ms-formlabel {
        text-align: right;
        padding-top: 10px;
        width: 80px;
    }
    .fleft {
        float: left;
    }

    .ms-standardheader {
        text-align: right;
    }

    .drp-module {
        margin-left: 7px;
    }

    .chk-shortbymodule {
        margin-top: 5px;
        margin-left: 5px;
    }

    .fleft {
        padding-left: 4px !important;
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
        padding-left: 10px;
        padding-right: 10px;
        text-align: right;
    }

    .filterBlock {
        padding: 10px;
    }
    .pl10 {
        padding-left: 10px;
    }
    .px10 {
        padding-left: 10px;
        padding-right: 10px;
    }
    .pl5 {
        padding-left: 5px;
    }

    .dxeListBoxItem_UGITGreenDevEx {
        padding-left: 4px;
        padding-right: 4px;
    }
</style>
<div class="filterBlock">
    <fieldset>
        <legend>Survey Feedback Options</legend>
        <table style="padding-top: 10px; width: 100%;">
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Select Type</h3>
                </td>
                <td class="ms-formbody">
                    <dx:ASPxComboBox ID="ddlSurveyType" runat="server" Width="230px" AutoPostBack="true" OnSelectedIndexChanged="ddlSurveyType_SelectedIndexChanged">
                        <Items>
                            <dx:ListEditItem Text="ALL" Value="ALL" />
                            <dx:ListEditItem Text="Module" Value="Module" />
                            <dx:ListEditItem Text="Generic" Value="Generic" />
                        </Items>
                    </dx:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Select Survey</h3>
                </td>
                <td class="ms-formbody">
                    <dx:ASPxComboBox ID="ddlSurvey" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSurvey_SelectedIndexChanged"></dx:ASPxComboBox>
                </td>
            </tr>

            <tr>
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Date Created</h3>
                    <td class="ms-formbody">
                        <div class="top_right_nav" style="left: 0px; float: left;">
                            <div class="fleft">
                                <b style="padding-top: 2px; float: left; font-weight: normal;">From:</b>
                            </div>
                            <div class="fleft">
                                <dx:ASPxSpinEdit ID="spneditfrom" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit>
                            </div>
                            <div class="fleft">
                                <b style="padding-top: 2px; float: left; font-weight: normal;">To:</b>
                            </div>
                            <div class="fleft">
                                <dx:ASPxSpinEdit ID="spneditto" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit>
                            </div>
                            <div class="fleft">
                                <dx:ASPxComboBox ID="ddlDateUnitsFrom" Width="60px" runat="server">
                                    <Items>
                                        <dx:ListEditItem Text="Days" Selected="true" Value="0" />
                                        <dx:ListEditItem Text="Weeks" Value="1" />
                                        <dx:ListEditItem Text="Months" Value="1" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </div>
                            <div class="fleft">
                                <dx:ASPxDateEdit ID="dtFromtDatesurvey" ClientVisible="false" Width="90px" ClientInstanceName="dtFromtDatesurvey" runat="server"></dx:ASPxDateEdit>
                                <dx:ASPxDateEdit ID="dtToDatesurvey" ClientVisible="false" ClientInstanceName="dtToDatesurvey" Width="90px" runat="server"></dx:ASPxDateEdit>
                            </div>
                        </div>
                    </td>
            </tr>

        </table>
    </fieldset>

</div>
<div>
    <div class="addEditPopup-btnWrap">
        <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click">
        </dx:ASPxButton>
        <dx:ASPxButton ID="lnkSubmit" runat="server" Text="Schedule Report" CssClass="primary-blueBtn" OnClick="lnkSubmit_Click">
        </dx:ASPxButton>
    </div>
</div>

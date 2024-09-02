
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeeklyTeamReport_SchedulerFilter.ascx.cs" Inherits="uGovernIT.Report.DxReport.WeeklyTeamReport_SchedulerFilter" %>
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

    .drp-module {
        margin-left: 7px;
    }

    .chk-shortbymodule {
        margin-top: 5px;
        margin-left: 5px;
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
<script type="text/javascript" id="WeeklyPrfReportScript">
    function HideWeeklyPrfReport() {
        var popupCtrl = ASPxClientControl.GetControlCollection().GetByName('<%=PopID%>');
        if (popupCtrl != undefined)
            popupCtrl.Hide();
    }

    function GetWeeklyTeamPrfReport(obj) {
        var params = "";
        var selectedModules = '<%=ModuleName%>';

        HideWeeklyPrfReport();
        var selectedCategories = "all";
        if (glCategories.GetValue() != null)
            selectedCategories = glCategories.GetValue().join(',');
        var datecontrolFrom = dtFromtDate.GetFormattedDate();
        var datecontrolTo = dtToDate.GetFormattedDate();

        var url = '<%=delegateControl%>' + "?control=weeklyteamreport&SelectedModule=" + selectedModules + "&DateFrom=" + datecontrolFrom + "&DateTo=" + datecontrolTo + "&Categories=" + selectedCategories;
        var popupHeader = "Weekly Team Performance";
        window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));

     return false;
 }

 function SetWeeklyFiltersForSchedule() {
    // cbpnlMainWeekly.PerformCallback();
     return false;
 }
</script>
<div style="float: left; width: 98%; padding-left: 0px;">
    <fieldset>
         <legend>Weekly Team Performance</legend>
                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="100%" >
                    <tr>
                        <td  class="ms-formlabel">
                              <h3 class="ms-standardheader">Date Range</h3>
                        </td>
                        <td class="ms-formbody">
                            <div class="top_right_nav" style="left: 0px; float: left; margin-top: 6px;">
                              <span id="spanNoOfDays" runat="server"  style="float: left; width: 100%; margin: 5px; margin-left: 0px">
                                    <span style="float: left;padding-top:5px">From:</span>
                                    <span style="float: left"> <dx:ASPxSpinEdit ID="txtValueFrom"  MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit></span>
                                    <span style="float: left;padding-top:5px">To:</span>
                                    <span style="float: left"><dx:ASPxSpinEdit ID="txtValueTo" MaxValue="0" MinValue="-100" Width="55px" runat="server"></dx:ASPxSpinEdit></span>
                                    <span style="float: left">
                                        <dx:ASPxComboBox ID="ddlDateUnitsFrom" Width="60px" runat="server">
                                            <Items>
                                                <dx:ListEditItem Text="Days" Selected="true" Value="0" />
                                                <dx:ListEditItem Text="Weeks" Value="1" />
                                                <dx:ListEditItem Text="Months" Value="1" />
                                            </Items>
                                        </dx:ASPxComboBox>

                                    </span>
                                </span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-formlabel">
                             <h3 class="ms-standardheader">Categories</h3>
                            </td>
                        <td class="ms-formbody">
                            <dx:ASPxGridLookup ClientInstanceName="glCategories" ClientEnabled="true" Visible="true" OnInit="glCategories_Init" SelectionMode="Multiple" ID="glCategories" runat="server" KeyFieldName="Category" MultiTextSeparator="; " Width="270px">
                                <Columns>
                                    <dx:GridViewCommandColumn SelectAllCheckboxMode="AllPages" Caption="" ShowSelectCheckbox="True" Width="40px" />
                                    <dx:GridViewDataTextColumn FieldName="Category" Width="180px">

                                        <SettingsHeaderFilter>
                                            <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                        </SettingsHeaderFilter>
                                        <HeaderTemplate>(Select All)</HeaderTemplate>
                                        <Settings AllowSort="False" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>

                                <GridViewProperties>
                                    <Settings VerticalScrollableHeight="120" ShowFilterRow="false" VerticalScrollBarMode="Auto" />
                                    <SettingsBehavior AllowSort="false" AutoExpandAllGroups="true" AllowClientEventsOnLoad="true" />
                                    <SettingsPager Mode="ShowAllRecords">
                                    </SettingsPager>

                                    <SettingsCommandButton>
                                        <ShowAdaptiveDetailButton ButtonType="Image"></ShowAdaptiveDetailButton>

                                        <HideAdaptiveDetailButton ButtonType="Image"></HideAdaptiveDetailButton>
                                    </SettingsCommandButton>
                                </GridViewProperties>
                                <ClientSideEvents />

                            </dx:ASPxGridLookup>

                        </td>
                    </tr>
                               
                          </table>
         </fieldset>
             <table style="width:100%;">
        <tr>
            <td>&nbsp;
            </td>
            <td style="text-align: right;">
                <div class="addEditPopup-btnWrap">
                    <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="lnkCancel_Click">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="lnkSubmit" runat="server" Text="Schedule Report" CssClass="primary-blueBtn" OnClick="lnkSubmit_Click">
                    </dx:ASPxButton>
                </div>
            </td>
        </tr>
    </table>
    </div>


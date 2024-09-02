
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NonPeakHoursPerformance_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.NonPeakHoursPerformance_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<script type="text/javascript" id="NonPeakHrPrfReportScript">
    function GetHelpDeskPerformance(obj) {
        if (dtHelpDeskPerformance.GetValue() == null || dtWorkingHoursStart.GetValue() == null || dtWorkingHoursEnd.GetValue() == null)
            return false;

        var params = "";
        var DateFrom = "";
        var selectedModules = '<%=ModuleName%>';
        var workingWindowStartTime = dtWorkingHoursStart.GetValueString();
        var workingWindowEndTime = dtWorkingHoursEnd.GetValueString();
        var datecontrolFrom = dtHelpDeskPerformance.GetDate();
        if (datecontrolFrom != undefined && datecontrolFrom != "")
            DateFrom = (datecontrolFrom.getMonth() + 1) + "/" + datecontrolFrom.getDate() + "/" + datecontrolFrom.getFullYear();
        var nonPeakHrWindow = txtNPHWindow.GetValue();
        var url = '<%=delegateControl%>' + "?reportName=NonPeakHoursPerformance&SelectedModule=" + selectedModules + "&Date=" + DateFrom + "&nonPeakHrWindow=" + nonPeakHrWindow + "&workingWindowEndTime=" + workingWindowEndTime + "&workingWindowStartTime=" + workingWindowStartTime;
        var popupHeader = "Non-Peak Hours Performance";
        window.location.href = url;
    return false;
    }

</script>
<div class="col-md-12 col-sm-12 col-xs-12">
    <fieldset>
        <legend class="reportTitle">Non-Peak Hours Performance</legend>
        <div class="ms-formtable accomp-popup row">
            <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                <div class="row">
                     <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Date Range</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtHelpDeskPerformance" ClientInstanceName="dtHelpDeskPerformance" 
                            runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                            DropDownButton-Image-Width="16">
                            <validationsettings CausesValidation="true"  ErrorDisplayMode="ImageWithText" RequiredField-IsRequired="true" RequiredField-ErrorText="Date Required">
                            </validationsettings>
                        </dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="row">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Non-Peak Hours Window</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxSpinEdit width="100%" id="txtNPHWindow" CssClass="aspxSpinEdit-dropDown" clientinstancename="txtNPHWindow" runat="server"></dx:ASPxSpinEdit>
                    </div>
                </div>
                <div class="row">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Working Hours</h3>
                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-6" style="padding-left:7px;">
                        <dx:ASPxTimeEdit id="dtWorkingHoursStart" clientinstancename="dtWorkingHoursStart" runat="server" Cssclass="AspxTimeEdit-ctrl" width="100%">
                            <ClearButton DisplayMode="OnHover"></ClearButton>
                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" RequiredField-ErrorText="Start Working Hours required" />
                        </dx:ASPxTimeEdit>
                    </div>
                     <div class="col-md-6 col-sm-6 col-xs-6" style="padding-right:7px;">
                         <dx:ASPxTimeEdit id="dtWorkingHoursEnd" CssClass="AspxTimeEdit-ctrl" clientinstancename="dtWorkingHoursEnd" runat="server" width="100%">
                            <ClearButton DisplayMode="OnHover"></ClearButton>
                            <ValidationSettings Display="Dynamic" RequiredField-IsRequired="true" RequiredField-ErrorText="End Working Hours required" />
                        </dx:ASPxTimeEdit>
                    </div>
                </div>
                <div class="row addEditPopup-btnWrap">
                    <dx:ASPxButton id="LinkButton7" runat="server" text="Cancel" CssClass="secondary-cancelBtn"  AutoPostBack="false" 
                        OnClick="LinkButton7_Click" CausesValidation="false">
                    </dx:ASPxButton>
                     <dx:ASPxButton ID="LinkButton4" runat="server" Text="Build Report"
                         AutoPostBack="false" CauseValidation="true" CssClass="primary-blueBtn">
                        <ClientSideEvents Click="function(s,e){GetHelpDeskPerformance(s);}" />                                           
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
        <dx:ASPxValidationSummary ID="vsValidationSummary1" RenderMode="BulletedList" runat="server">
            </dx:ASPxValidationSummary>
    </fieldset>
    </div>

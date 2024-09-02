
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeeklyTeamReport_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.WeeklyTeamReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script type="text/javascript" id="WeeklyPrfReportScript">
    function GetWeeklyTeamPrfReport(obj) {

        if (SetDifference() == false)
            return false;

        var params = "";
        var selectedModules = '<%=ModuleName%>';
        var selectedCategories = "all";
        if (glCategories.GetValue() != null)
            selectedCategories = glCategories.GetValue().join(',');
        var datecontrolFrom = dtFromtDate.GetFormattedDate();
        var datecontrolTo = dtToDate.GetFormattedDate();
        var url = '<%=delegateControl%>' + "?reportName=weeklyteamreport&SelectedModule=" + selectedModules +  "&DateFrom=" + datecontrolFrom + "&DateTo=" + datecontrolTo + "&Categories=" + selectedCategories;
        var popupHeader = "Weekly Team Performance";
        window.location.href = url;
     return false;
    }

    function SetDifference() {
        var diff = CheckDifference();
        if (diff < 0) {
            return false;
        }       
        return true;
    }

    function CheckDifference() {
        var startDate = new Date();
        var endDate = new Date();
        var difference = -1;
        startDate = dtFromtDate.GetDate();
        if (startDate != null) {
            endDate = dtToDate.GetDate();
            var startTime = startDate.getTime();
            var endTime = endDate.getTime();
            difference = (endTime - startTime) / 86400000;

        }
        return difference;
    }
</script>

<dx:ASPxCallbackPanel ID="cbpnlMain" ClientInstanceName="cbpnlMainWeekly" runat="server">
    <ClientSideEvents />
    <PanelCollection>
        <dx:PanelContent>
            <fieldset>
                <legend class="reportTitle">Weekly Team Performance</legend>
                <div class="col-md-6 col-sm-6 col-xs-12 ms-formtable accomp-popup">
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">
                                <asp:Label ID="Label10" runat="server" Text="Date Range"></asp:Label>
                            </h3>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">From</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxDateEdit ID="dtFromtDate" Width="100%" ClientInstanceName="dtFromtDate" DropDownButton-Image-Width="16px"
                                    runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                    <validationsettings CausesValidation="True" ErrorText="From date can't be greater than To date" 
                                        ErrorDisplayMode="ImageWithText" RequiredField-IsRequired="true" RequiredField-ErrorText="From Date Required" >
                                    </validationsettings>
                                    <clientsideevents DateChanged="function(s,e){SetDifference();}" 
                                    Validation="function(s,e){e.isValid = (CheckDifference()>=0)}"/>
                                </dx:ASPxDateEdit>
                            </div>
                        </div>
                         <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">To</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxDateEdit ID="dtToDate" ClientInstanceName="dtToDate" Width="100%" DropDownButton-Image-Width="16px" 
                                    runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                                    <validationsettings CausesValidation="True"  ErrorDisplayMode="ImageWithText" RequiredField-IsRequired="true" RequiredField-ErrorText="To Date Required">
                                    </validationsettings>
                                    <clientsideevents DateChanged="function(s,e){SetDifference();}" 
                                    Validation="function(s,e){e.isValid = (CheckDifference()>=0)}"/>
                                </dx:ASPxDateEdit>
                            </div>
                         </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">
                                     <asp:Label ID="Label11" runat="server" Text="Categories"> </asp:Label>
                                </h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <dx:ASPxGridLookup ClientInstanceName="glCategories" ClientEnabled="true"
                                    Visible="true" OnInit="glCategories_Init" SelectionMode="Multiple"
                                    ID="glCategories" runat="server" KeyFieldName="Category" DropDownWindowStyle-CssClass="aspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow" 
                                    GridViewStyles-FilterRow-CssClass="aspxGridLookUp-FilerWrap" GridViewStyles-FilterCell-CssClass="aspxGridLookUp-FilerCell"
                                    MultiTextSeparator="; " Width="100%" CssClass="aspxGridLookUp-dropDown">
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
                            </div>
                        </div>
                    </div>
                    <div class="row addEditPopup-btnWrap">
                        <dx:ASPxButton ID="LinkButton3" runat="server" Text="Cancel" OnClick="LinkButton3_Click" 
                            CausesValidation="false" CssClass="secondary-cancelBtn" >
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="LinkButton6" CssClass="primary-blueBtn" runat="server" Text="Build Report"
                            AutoPostBack="false">
                            <ClientSideEvents Click="function(s,e){GetWeeklyTeamPrfReport(s);}" />
                        </dx:ASPxButton>
                    </div>
                </div>
                <dx:ASPxValidationSummary ID="vsValidationSummary1" RenderMode="BulletedList" runat="server">
                </dx:ASPxValidationSummary>
            </fieldset>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>


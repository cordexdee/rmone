<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessUnitDistribution_Filter.ascx.cs" Inherits="uGovernIT.Report.DxReport.BusinessUnitDistribution_Filter" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css">
    #wrapper_1 { clear: left; float: left; position: relative; left: 50%; }
    #container_1 { display: block; float: left; position: relative; right: 50%; }
</style>

<script type="text/javascript">
    function GetBUDReportPopup(obj) {
        
         var moduleName = "<%=ModuleName%>";
        var datecontrolStart = dtBUDStart.GetDate();  
        var datecontrolEnd = dtBUDEnd.GetDate();

        //datecontrolStart = datecontrolStart.format('MM/dd/yyyy');
        //datecontrolEnd = datecontrolEnd.format('MM/dd/yyyy');

        if (datecontrolStart && Date.parse(datecontrolStart.value) != "NaN") {
            datecontrolStart.value = "";
        }
        if (datecontrolEnd && Date.parse(datecontrolEnd.value) != "NaN") {
            datecontrolEnd.value = "";
        }

        var StartDate =  datecontrolStart != null ? ((datecontrolStart.getMonth() + 1) + '/' + datecontrolStart.getDate() + '/' +  datecontrolStart.getFullYear()) : '';
        var EndDate =  datecontrolEnd != null ? ((datecontrolEnd.getMonth() + 1) + '/' + datecontrolEnd.getDate() + '/' + datecontrolEnd.getFullYear()) : '';
        var url = '<%=BusinessunitdistributionReportURL %>' + "?reportName=BusinessUnitDistribution&Module=" + moduleName + "&dtStart=" + StartDate + "&dtEnd=" + EndDate + "&divisions=" + tbDivision.values.join(',') + "&showReport=true";
        LoadingPanel.Show();
        window.location.href = url;
        return false;
    }
</script>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." CssClass="customeLoader" ClientInstanceName="LoadingPanel" Image-Url="~/Content/IMAGES/ajax-loader.gif" ImagePosition="Top" 
    Modal="True">
</dx:ASPxLoadingPanel>
<div id="dvCLJR" class="col-md-12 col-sm-12 col-xs-12">
    <fieldset class="projectsummary-reportContainer">
        <legend class="summary-reportHeading excutive-sumReport-heading"><span id="spCLJR"><%= ReportTitle %></span></legend>
        <div class="tblReports row">
            <div style="padding-top: 1px;display:none;">
                <asp:Label ID="lblDates" runat="server" Text="Date" CssClass="summary-reportLabel"> </asp:Label>
            </div>
            <div class="col-md-4 col-sm-6 col-xs-12" style="display:none;">
                <div class="reportDate-fieldWrap">
                    <b style="padding-top: 4px; float: left; font-weight: normal; color: #4a90e2;">From:</b>
                    <div>
                        <dx:ASPxDateEdit CssClassTextBox="inputTextBox datetimectr111" ClientInstanceName="dtBUDStart" DateOnly="true"
                            ID="dtBUDStart" runat="server" ToolTip="" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" 
                            DropDownButton-Image-Width="18" CssClass="CRMDueDate_inputField reportDateField"></dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="reportDate-fieldWrap1">
                    <b style="padding-top: 4px; float: left; font-weight: normal; color: #4a90e2;">To:</b>
                    <div>
                        <dx:ASPxDateEdit CssClassTextBox="inputTextBox datetimectr111" ClientInstanceName="dtBUDEnd" DateOnly="true"
                            ID="dtBUDEnd" runat="server" ToolTip="" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" 
                            DropDownButton-Image-Width="18" CssClass="CRMDueDate_inputField reportDateField"></dx:ASPxDateEdit>
                    </div>
                </div>
           </div>
           <div class="col-md-4 col-sm-6 col-xs-12 noPadding">
                <div>
                    <div style="width:100%; float:left" >
                       <b style="padding-top: 4px; float: left; font-weight: normal; color: #4A6EE2; font-size:12px;">Division:</b>
                    </div>
                    
                    <div class="studio-dropDownWrap" style="width:100%">
                        <dx:ASPxTokenBox ID="tbDivision" runat="server" Width="100%" cssClass="aspxUserTokenBox-control" ClientInstanceName="tbDivision" IncrementalFilteringMode="Contains"></dx:ASPxTokenBox>
                    </div>
                </div>
           </div>
        </div>
        <span id="Mesg" style="color:red"></span>
        <div class="row summaryReport-btnWrap">
            <div class="summaryReport-btnContainer col-md-4 col-sm-4 col-xs-12 noPadding">
                <ul class="summaryReport-ul">
                     <li runat="server" id="Li2">
                        <dx:ASPxButton ID="LinkButton2" runat="server" Text="Build Report" AutoPostBack="false" CssClass="buildReport-btn">
                            <ClientSideEvents Click="function(s,e){ GetBUDReportPopup(this); }" />
                        </dx:ASPxButton>
                    </li>
                    <li runat="server" id="Li1">
                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" AutoPostBack="false" OnClick="btnCancel_Click" CssClass="cancelReport-btn">
                        </dx:ASPxButton>
                    </li>

                   
                </ul>
            </div>
        </div>
    </fieldset>
</div>




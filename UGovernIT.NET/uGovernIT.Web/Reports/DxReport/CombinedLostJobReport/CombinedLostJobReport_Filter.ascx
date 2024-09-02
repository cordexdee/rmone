<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CombinedLostJobReport_Filter.ascx.cs" Inherits="uGovernIT.DxReport.CombinedLostJobReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<script>
    //custom reports

    function GetCLJRPopup(obj) {
        
        var moduleName = "<%=ModuleName%>";
       <%-- var path = "<%=CombinedLostJobReportURL%>";--%>
        //var path = '<%=delegateControl %>' + "?reportName=CombinedLostJobReport&SelectedModule=" + moduleName;
        <%--var dtCLJRStart = document.getElementById('<%=dtCLJRStart.ClientID %>').val;
        var dtCLJREnd = document.getElementById('<%=dtCLJREnd.ClientID %>').val;--%>

        var dtCLJStartt = dtCLJRStart.GetDate();
        var dtCLJEndd = dtCLJREnd.GetDate();

        if (dtCLJStartt && Date.parse(dtCLJStartt.value) != "NaN") {
            dtCLJStartt.value = "";
        }

        if (dtCLJEndd && Date.parse(dtCLJEndd.value) != "NaN") {
            dtCLJEndd.value = "";
        }

         var StartDate =  dtCLJStartt != null ? ((dtCLJStartt.getMonth() + 1) + '/' + dtCLJStartt.getDate() + '/' +  dtCLJStartt.getFullYear()) : '';
        var EndDate =  dtCLJEndd != null ? ((dtCLJEndd.getMonth() + 1) + '/' + dtCLJEndd.getDate() + '/' + dtCLJEndd.getFullYear()) : '';

        //var params = "dtStart=" + StartDate + "&dtEnd=" + EndDate;
        //if ($('#spCLJR').html() == "Combined Job Report") {
        //    params += "&JobSummary=1";
        //}
        //window.parent.UgitOpenPopupDialog(path, params, $('#spCLJR').html(), 90, 90, false, escape(window.location.href));
        //document.getElementById("dvCLJR").style.display = "none";
        //return false;
        var path = '<%=delegateControl %>' + "?reportName=CombinedLostJobReport&SelectedModule=" + moduleName +"&dtStart=" + StartDate + "&dtEnd=" + EndDate;

       // var params = "dtStart=" + StartDate + "&dtEnd=" + EndDate;
        if ($('#spCLJR').html() == "Combined Job Report") {
            path += "&JobSummary=1";
        }
       // window.parent.UgitOpenPopupDialog(path, params, $('#spCLJR').html(), 90, 90, false, escape(window.location.href));
        LoadingPanel.Show();

        window.location.href = path;
        document.getElementById("dvCLJR").style.display = "none";
        return false;

    }

    function ShowtCLJRPopup() {

        var datecontrolStart = document.getElementById("<%=dtCLJRStart.ClientID %>_dtCLJRStartDate");
        var datecontrolEnd = document.getElementById("<%=dtCLJREnd.ClientID %>_dtCLJREndDate");
        if (datecontrolStart && Date.parse(datecontrolStart.value) != "NaN") {
            datecontrolStart.value = "";
        }
        if (datecontrolEnd && Date.parse(datecontrolEnd.value) != "NaN") {
            datecontrolEnd.value = "";
        }

        $("#dvCLJR").css({ 'top': 0 + 'px', 'display': 'block', 'left': ($(".imgReport").position().left - $("#dvCLJR").width()) + 'px' });
        return false;
    }

    function HideCLJRPopup() {
        document.getElementById("dvCLJR").style.display = "none";
    }
</script>
<style>
    legend {
        width:auto;
        border:none;
        font-size: 12px;
        font-weight: bold !important;
        margin-bottom: 5px !important;
    }
    .spCLJR {
        font-size: 18px;
    }
</style>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." CssClass="customeLoader" ClientInstanceName="LoadingPanel" Image-Url="~/Content/IMAGES/ajax-loader.gif" ImagePosition="Top"
    Modal="True">
</dx:ASPxLoadingPanel>

<div id="dvCLJR" class="col-md-12 col-sm-12 col-xs-12">
    <fieldset class="px-4">
        <legend id="spCLJR" class="spCLJR"><%= ReportTitle %></legend>
        <div>
            <asp:Label ID="lblDates" runat="server" Text="Date" CssClass="summary-reportLabel"> </asp:Label>
        </div>
        <div class="tblReports row">
            <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                <div class="reportDate-fieldWrap">
                    <b style="padding-top: 4px; float: left; font-weight: normal; color: #4a90e2;">From:</b>
                    <div>
                        <dx:ASPxDateEdit CssClassTextBox="inputTextBox datetimectr111" ClientInstanceName="dtCLJRStart" DateOnly="true"
                            ID="dtCLJRStart" runat="server" ToolTip="" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" 
                            DropDownButton-Image-Width="18" CssClass="CRMDueDate_inputField reportDateField"></dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="reportDate-fieldWrap1 pl-3">
                    <b style="padding-top: 4px; float: left; font-weight: normal; color: #4a90e2;">To:</b>
                    <div>
                        <dx:ASPxDateEdit CssClassTextBox="inputTextBox datetimectr111" ClientInstanceName="dtCLJREnd" DateOnly="true"
                            ID="dtCLJREnd" runat="server" ToolTip="" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" 
                            DropDownButton-Image-Width="18" CssClass="CRMDueDate_inputField reportDateField"></dx:ASPxDateEdit>
                    </div>
                </div>
           </div>
        </div>
        <div class="row summaryReport-btnWrap">
            <div class="pt-3 col-md-6 col-sm-6 col-xs-12 noPadding">
                <ul class="summaryReport-ul">
                     <li runat="server" id="Li2">
                        <dx:ASPxButton ID="LinkButton2" runat="server" Text="Build Report" AutoPostBack="false" CssClass="buildReport-btn">
                            <ClientSideEvents Click="function(s,e){GetCLJRPopup(this);}" />
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

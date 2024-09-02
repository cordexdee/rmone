<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardQueryDisplay.ascx.cs" Inherits="uGovernIT.Web.DashboardQueryDisplay" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .filterpopup {
        float: left;
        width: 300px;
        height: 155px;
        position: absolute;
        z-index: 1000;
        text-align: center;
        background: white;
        border: 2px outset gray;
    }

    .blockView {
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        position: absolute;
        z-index: 1;
    }

    .wherediv {
        width: 100%;
        float: left;
        height: 23px;
        padding: 10px;
        padding-left: 0px;
    }

    .dropdown {
        height: 20px;
        width: 200px;
    }

    #calendarDiv {
        display: none;
    }

    /*.span-title {
        font-weight: bold;
        vertical-align: middle;
    }*/

    /*.table-filter {
        width: 600px;
    }*/
   
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function createFilter(obj) {
        
        _spFormOnSubmitCalled = false;
        $(obj).attr('disabled', 'disabled');
        if ($('.panel-parameter').length > 0) {
            EmptyWhereClause();
        }
        LoadingPanel.Show();
        var rows = $("#<%=countEditableFields.ClientID %>").val();
        var count = parseInt(rows);

        if ($("#<%=isControlLoaded.ClientID %>").val() == "true") {

            var currentPosition = $(obj).position();
            var left = currentPosition.left;
            if (left <= 0) {
                left = 20;
            }

            $("#WherePanelContainer").css({ "display": "block", "height": 60 * count + "px", "top": (currentPosition.top) + "px", "left": (left) + "px" });
        }
        else {
         
            var id = $("#<%=ddlQueryList.ClientID %> option:selected").val();
          
            var sourceurl = "<%=sourceUrl%>";
            sourceurl = updateQueryStringParameter(sourceurl, "queryId", id);
            window.location.href = sourceurl;

        }
    }

    function customCIOReportDialogClose(dialogResult, returnValue) {
        if (reportBlockClone != null) {
            $("#WherePanelContainer").html(reportBlockClone);
        }
    }

    function ClickHidden() {
        $("#WherePanelContainer").css("display", "none");
        $(".hiddenButton").click();
        return false;

    }

    function setFlag() {
        $("#<%=runQueryFlag.ClientID %>").val("false");
        return true;
    }

    function exportReport() {

        if ($("#<%=isTableLoaded.ClientID %>").val() == "true") {
            return true;
        }
        else {
            alert("No Data to export!");
            return false;
        }
    }

    function validate(obj) {
        setFlag();
        if ($(obj).val() == "-1")
            return false;
        else
            return true;
    }

    $(document).ready(function () {

        $('#<%=ddlCategoryList.ClientID%> option').each(function () {
            if (this.value.indexOf("SubCategory--") > -1) {
            }
            else {
                $(this).css('font-weight', 'bold');
            }
        });

        $('.dropdown').change(function () {
            $('#<%= hdnbuttonClick.ClientID%>').val('');
        });

        $('.report-parent').parent().addClass("report-container");
    });

    
</script>

<div id="calendarDiv" class="report-parent">
    <dx:ASPxDateEdit ID="dummyCalendar" runat="server" Visible="true" />
</div>


<asp:HiddenField ID="isTableLoaded" Value="" runat="server" />
<asp:HiddenField ID="sourcePageFactTable" Value="" runat="server" />
<asp:HiddenField ID="isControlLoaded" Value="" runat="server" />
<asp:HiddenField ID="countEditableFields" Value="" runat="server" />
<asp:HiddenField ID="runQueryFlag" Value="" runat="server" />

<div class="col-md-12 col-xs-12 col-sm-12" id="filterTable" runat="server" style="padding-top:15px;">
    <div class="row">
        <div class="reportField-wrap" style="display: none;">
            <div class="reportField-label">Module:</div>
            <div class="reportField-field">
                <asp:DropDownList ID="ddlModules" class="dropdown " AutoPostBack="true" runat="server"
                    OnSelectedIndexChanged="DdlModules_OnSelectedIndexChanged" CssClass="aspxDropDownList"/>
            </div>
        </div>
        <div class="col-md-3 col-sm-3 col-xs-12" style="padding-left:0">
            <div class="reportField-label">Category:</div>
            <div class="reportField-field">
                <asp:DropDownList ID="ddlCategoryList" class="dropdown" AutoPostBack="true" runat="server"
                    OnSelectedIndexChanged="DdlCategoryList_OnSelectedIndexChanged" CssClass="reportDropDown-list aspxDropDownList"/>
            </div>
        </div>
        <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="reportField-label">Report:</div>
            <div class="reportField-field">
                <asp:DropDownList ID="ddlQueryList" runat="server" class="dropdown" CssClass="reportDropDown-list aspxDropDownList" />
            </div>
        </div>
        <div class="col-md-3 col-sm-3 col-xs-12">
            <div class="reportBtn-wrap">
                <input type="button" class="reportBtn-run" id="btRunReport" value="Run" onclick="createFilter(this);" />
                <asp:HiddenField ID="hdnbuttonClick" runat="server" />
            </div>
        </div>
    </div>
</div>

<dx:ASPxCallback ID="Callback" runat="server" ClientInstanceName="Callback">
    <ClientSideEvents CallbackComplete="function(s, e) { LoadingPanel.Hide(); }" />
</dx:ASPxCallback>

<asp:Panel ID="previewInfoPanel" runat="server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="reportPanel" runat="server">
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Panel>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel">
</dx:ASPxLoadingPanel>

<asp:Button ID="btRun" CssClass="hiddenButton" runat="server" OnClick="BtSaveAndRun_OnClick"
    Text="Run" Style="display: none" />

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectResourceReport_Filter.ascx.cs" Inherits="uGovernIT.DxReport.ProjectResourceReport_Filter" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<style type="text/css" runat="server" id="stylegvResource">
    .ModuleBlock {
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        width: 540px;
        margin: 0 auto;
        position:relative;
    }
    legend {
        width: auto;
        border: none;
        font-size: 12px;
        font-weight: 600;
    }

    .filterpopup-table {
        border-collapse: collapse;
    }

        .filterpopup-table > tbody > tr > td {
            border: 1px solid #fff;
            padding: 2px 0px 2px 5px;
        }

        .filterpopup-table .filter-label {
            float: right;
            padding-right: 5px;
            font-weight: bold;
        }

    .hide {
        display: none;
    }



    .headerRowBackground {
        background-color: #C6C6C6;
    }


    .gvResource-table {
        overflow-x: auto;
        width: 1200px; /* i.e. too small for all the columns */
    }

    .subCategoryBackground {
        background-color: #DCE6F1;
    }

    .titleHeaderBackground {
        background-color: #687398;
        color: white;
    }

    .tablerow {
        height: 25px;
    }
    table.gvResourceClass > tbody > tr > th {
        width:200px;
        padding-left:10px;
        padding-right:10px;
       white-space:nowrap;
    }

    table.gvResourceClass > tbody > tr > td {
        width:200px;
    }
    table.gvResourceClass > tbody > tr > td:first-child {
        text-align: left;
        padding-left:3px;
        padding-right:10px;
        width:300px !important;
         white-space:nowrap;
    }
    table.gvResourceClass > tbody > tr > th:first-child {
         text-align: left;
         padding-left:3px;
         padding-right:10px;
         width:300px !important;
          white-space:nowrap;
    }
    
    table.gvResourceClass tr .lasttd {
        border-left: 1px solid black;
        text-align: right;
         padding-right:3px;
         width:70px;
        
    }
</style>
<script type="text/javascript">

    function downloadExcel(obj) {
        debugger;
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function downloadPDF(obj) {
        debugger;
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=pdf";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }


    function startPrint(obj) {
         var exportUrl = '<%=urlBuilder%>';

        exportUrl += "&ExportReport=true&exportType=print";
      
        var winPrint = window.open();
        $("[customType=exportPanel]").hide();

        var allCss = "";
        $("style").each(function (i, item) {
            allCss += "  " + $(item).html();
        });
        
        winPrint.document.write('<html><head><title>Resource Hours Report</title><style type="text/css">');
        winPrint.document.write(allCss);
        winPrint.document.write('</style></head><body >');
        
        winPrint.document.write($("#divResource").html());
        winPrint.document.write('</body></html>');
        winPrint.document.close();
        winPrint.focus();
        winPrint.print();
        winPrint.close();
        $("[customType=exportPanel]").show();
        return true;
    }

    

</script>
<div id="PnlBudgetReportPopup" runat="server" class="ModuleBlock">

    <fieldset>
        <legend>Resource Hours Report</legend>
        <table cellspacing="10px" cellpadding="5px">
            <tr>
                <td class="titleTD" width="100%" align="center" style="text-align: center">
                    <asp:Panel ID="componentsForm" runat="server" Style="float: left; padding-left: 3px; width: 100%; text-align: center;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <table width="500px" class="filterpopup-table" frame="box" cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="width: 50%; text-align: left;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="ms-formlabel labelClass" style="width: 50%; text-align: left;padding: 10px 10px;">
                                                                <asp:Label ID="Label1" CssClass="filter-label" Text="Category:" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="ms-formbody textBoxClass DateControlDateFrom" style="width: 50%; text-align: left;">
                                                                <asp:DropDownList ID="ddlResourceReportType" runat="server">
                                                                    <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                                    <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel labelClass" style="width: 50%; text-align: left;padding: 10px 10px;">
                                                                <asp:Label ID="lblDateFrom" CssClass="filter-label" Text="Date From:" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="ms-formbody textBoxClass DateControlDateFrom" style="width: 50%; text-align: left;">
                                                                <dx:ASPxDateEdit ID="dtDateFrom" runat="server" DateOnly="true" ClientInstanceName="dtDateFrom"></dx:ASPxDateEdit>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="ms-formlabel labelClass" style="width: 50%; text-align: left;padding: 10px 10px;">
                                                                <asp:Label ID="lblDateTo" CssClass="filter-label" Text="Date To:" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="ms-formbody textBoxClass DateControlDateTo" style="width: 50%; text-align: left;">
                                                                <dx:ASPxDateEdit ID="dtDateTo" runat="server" DateOnly="true" ClientInstanceName="dtDateTo"></dx:ASPxDateEdit>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel labelClass" style="text-align: right; padding-top: 5px;">
                                        <div class="fright">
                                            <asp:Button ID="btnRun" runat="server" Text="Run" OnClick="btnRun_Click" CssClass="input-button-bg"  />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="input-button-bg"/>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </asp:Panel>
                    <asp:Label ID="lblMessage" runat="server" Visible="false" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
</div>


<div class="mainblock" runat="server" style="float: left; width: 100%;" >
  
    <asp:HiddenField ID="hdnExportType" runat="server" Value="" />

    <asp:Panel ID="projectPlanPanel" CssClass="worksheetpanel" runat="server" Visible="false">
        <!-- Resource Allocation -->
        <div class="worksheetpanel-m" id="divResource">
            <%--<table style="width:100%;" cellspacing="5px" cellpadding="5px">
                <tr>
                    <td style="text-align: center; font-weight: bold; font-size: medium;">--%>
            <div style="text-align: center; font-weight: bold; font-size: medium; padding-bottom:10px;">
                <asp:Label ID="lblReportTitle" runat="server" Text="Resource Hours Report"></asp:Label>
            </div>
            <%--</td>
                </tr>
                <tr>
                    <td>--%>
            <div>
                <span class="fleft" style="padding-left: 5px;">
                    <asp:Label ID="lblReportDateRange" runat="server" Text="" Visible="false"></asp:Label>
                </span>
                <span class="fright" style="padding-right: 5px; padding-left: 10px;">
                    <asp:Label ID="lblRightHeading" runat="server" Text=""></asp:Label>
                </span>

                <div id="exportPanel" runat="server" style="background: #fff; display: block; float: right; border: 0px inset; text-align: right;" customtype="exportPanel">
                    <span class="fleft">
                        <img src="/Content/IMAGES/excel-icon.png" alt="Excel" title="Export to Excel" style="cursor: pointer;" onclick="downloadExcel(this);" />
                    </span>
                    <span class="fleft" style="padding: 0px 3px;">
                        <img src="/Content/IMAGES/pdf-icon.png" alt="Pdf" title="Export to Pdf" style="cursor: pointer;" onclick="downloadPDF(this);" />
                    </span>
                    <span class="fleft">
                        <img src="/Content/IMAGES/print-icon.png" alt="Print" title="Print" style="cursor: pointer;" onclick="startPrint(this);" />
                    </span>
                </div>
            </div>
            <table style="width: 100%;" cellspacing="5px" cellpadding="5px">
                <tr>
                    <td>
                        <asp:HiddenField runat="server" ID="currentYearHidden" Value="" />

                        <div style="overflow-x: auto; " id="divResourceGrid">
                            <asp:GridView ID="gvResource" runat="server" AutoGenerateColumns="true"  HeaderStyle-CssClass="titleHeaderBackground tablerow" Width="100%" 
                                RowStyle-CssClass="tablerow subCategoryBackground" AlternatingRowStyle-CssClass="tablerow" BorderWidth="0" GridLines="None" CssClass="gvResourceClass">
                                <RowStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="50px" />
                                <AlternatingRowStyle CssClass="ms-alternatingstrong tablerow" />
                                
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float:right;">
            <asp:Button ID="btnCloseResourcePopUp" runat="server" Text="Close" OnClick="btnCloseResourcePopUp_Click" CssClass="input-button-bg"/>
        </div>
        <%--<input type="button" id="btnCloseResourcePopUp" runat="server" onclick="btnCancel_Click" value="Close" style="float: right; margin-top: 10px;"  class="input-button-bg" />--%>
    </asp:Panel>
</div>


<script type="text/javascript">
    $(".gvResourceClass tr> td:last-child").addClass("lasttd");
    $(".gvResourceClass tr> th:last-child").addClass("lasttd");
    $(".gvResourceClass tr> th:gt(1)").css("width", "200px");;
    $(".gvResourceClass tr> td:gt(1)").css("width", "200px");
    $(document).ready(function () { SetWidth(); });
    function SetWidth() {
        var sWidth = $(window).width();
         sWidth = sWidth - 40;
        $("#divResourceGrid").css("width", sWidth);
        return false;
    }
   
</script>
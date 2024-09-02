
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectActualsReport_Filter.ascx.cs"   Inherits="uGovernIT.Report.DxReport.ProjectActualsReport" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx"  %>

<style type="text/css">
    .ModuleBlock
    {
        float: inherit;
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        /*  position: absolute;
    z-index: 1; */
    }

    .dataCellStyle
    {
        text-align: right;
        padding-right: 5px;
    }

    .headerCellStyle
    {
        font-weight: bold;
        text-align: right;
        padding-right: 5px;
    }

    .filterpopup-table
    {
        border-collapse: collapse;
    }

        .filterpopup-table > tbody > tr > td
        {
            border: 1px solid #fff;
            padding: 2px 0px 2px 5px;
        }

        .filterpopup-table .filter-label
        {
            float: right;
            padding-right: 5px;
            font-weight: bold;
        }

    .ugitlightGroupRow
    {
        background-color: Teal;
    }

    .hide
    {
        display: none;
    }

    .categoryBackground
    {
        background-color: #BED0E5;
    }

    .subCategoryBackground
    {
        background-color: #DCE6F1;
    }

    .itemBackground
    {
        background-color: #E9ECF3;
    }

    .treecontent
    {
        border-collapse: collapse;
    }

    .treecontent td
    {
        padding: 4px;
    }
    .titleHeaderBackground
    {
        background-color: #687398;
    }
    .budgetTypeBackground
    {
        background-color: #A4AAC1;
    }
</style>


<script type="text/javascript">
    $(function () {
        var print = '<%=printReport%>';
       if(print == "True")
       {
          window.print();
        }
    });

    function expandCollapse(obj) {
        var currentTr = $(obj).parents("tr");
        var isExp = true;
        if ($(obj).attr("src").indexOf("plus") == -1) {
            isExp = false;
        }

        if (isExp) {
            $(obj).attr("src", "<%= minusURL%>");
        }
        else {
            $(obj).attr("src", "<%= plusURL%>");
        }


        var tableRows = $(".tablerow");
        var currentTrIndex = tableRows.index(currentTr.get(0));
        var currentLevel = parseInt(currentTr.attr("level"));
        for (var i = currentTrIndex + 1; i < tableRows.length; i++) {
            var jqTrObj = $(tableRows[i]);
            var level = parseInt(jqTrObj.attr("level"));

            if (level <= currentLevel) {
                break;
            }
            if (isExp) {
                jqTrObj.removeClass("hide");
            }
            else {
                jqTrObj.addClass("hide");
            }

        }
    }

    function downloadPDF(obj) {
        var exportUrl = '<%=urlBuilder%>';
          exportUrl += "&ExportReport=true&exportType=pdf";
          window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function downloadExcel(obj) {
        var exportUrl = '<%=urlBuilder%>';
          exportUrl += "&ExportReport=true&exportType=excel";
          window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function downloadImage(obj) {
        var exportUrl = '<%=urlBuilder%>';
          exportUrl += "&ExportReport=true&exportType=image";
          window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function startPrint(obj) {
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=print";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
       
    }
    function selectAll(obj) {

        $(".chkBoxCategoryList input[type='checkbox']").each(function () {
            $(this).attr("checked", $("#<%= chkSelectAll.ClientID%>").get(0).checked);
        })
    }

    function closeReport() {
        var sourceURL = "<%= Request["source"] %>";
        window.parent.CloseWindowCallback(sourceURL);
    }
</script>


<div id="PnlBudgetReportPopup" runat="server" style="float: left;" class="ModuleBlock">
    <fieldset>
        <legend style="font-weight:bold;">Project Actuals Report</legend>
        <table cellspacing="10px" cellpadding="5px">
            <tr>
                <td class="titleTD" width="100%" align="center" style="text-align: center">
                    <asp:Panel ID="componentsForm" runat="server" Style="float: left; padding-left: 3px; width: 100%; text-align: center;">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td align="center">
                                        <table width="500px" class="filterpopup-table" frame="box" cellpadding="0" cellspacing="0"
                                            border="0">
                                            <tr>
                                                <td style="width: 50%; text-align: left;">
                                                     <table width="100%">
                                                        <tr>
                                                            <td class="labelClass" style="text-align: right;">
                                                                <asp:Label ID="lblSelectCategory" CssClass="filter-label" Text="Select Category:"
                                                        runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right;">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" Checked="true" Text="All" onclick="selectAll()"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td class="ms-formbody textBoxClass chkBoxCategoryList" style="width: 50%; text-align: left;">
                                                    <asp:CheckBoxList ID="chkBoxCategoryList" runat="server">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ms-formlabel labelClass" style="width: 50%; text-align: left;">
                                                    <asp:Label ID="lblDateFrom" CssClass="filter-label" Text="Date From:" runat="server"></asp:Label>
                                                </td>
                                                <td class="ms-formbody textBoxClass DateControlDateFrom" style="width: 50%; text-align: left;">
                                                    <dx:ASPxDateEdit ID="dtDateFrom" runat="server" ></dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ms-formlabel labelClass" style="width: 50%; text-align: left;">
                                                    <asp:Label ID="lblDateTo" CssClass="filter-label" Text="Date To:" runat="server"></asp:Label>
                                                </td>
                                                <td class="ms-formbody textBoxClass DateControlDateTo" style="width: 50%; text-align: left;">
                                                    <dx:ASPxDateEdit ID="dtDateTo" runat="server"></dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="ms-formlabel labelClass" style="text-align: right; padding-top: 5px;">
                                        <div class="fright">
                                            <dx:ASPxButton ID="btnRun" runat="server" Text="Run" OnClick="btnRun_Click"></dx:ASPxButton>
                                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel">
                                                <ClientSideEvents Click="function(s, e){
                                                    window.frameElement.commitPopup();
                                                    return false;
                                                    }" />
                                            </dx:ASPxButton>
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


<asp:Panel ID="pnlBudgetComponent" runat="server" Style="display: none;">
    <table style="width:100%;">
        <tr>
            <td colspan="2" style="text-align: center; font-size: large;">
                <asp:Label runat="server" ID="lblHeading" Text="Project Budget Report"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <asp:Label runat="server" ID="lblSubHeading" Text="Report From"></asp:Label>
            </td>
            <td style="text-align:right;">
                <div id="exportPanel" runat="server" style="background:#fff;display:block;float: right; border: 0px inset;" >
                    <span  class="fleft" >
                         <img  src="/Content/images/excel-icon.png" alt="Excel" title="Export to Excel"  style="cursor:pointer;" onclick="downloadExcel(this);" />
                    </span>
                    <span class="fleft" style="padding:0px 3px;">
                            <img  src="/Content/images/pdf-icon.png" alt="Pdf" title="Export to Pdf" style="cursor:pointer;" onclick="downloadPDF(this);" />
                    </span>
                     <span  class="fleft" >
                         <img  src="/Content/images/print-icon.png" alt="Print" title="Print"  style="cursor:pointer;" onclick="startPrint(this);"/>
                    </span>
                </div>
            </td>
        </tr>
        <tr style="height: 100%">
            <td colspan="2" style="text-align: left;" >
                <asp:Repeater ID="budgetTypeRepeater" runat="server" OnItemDataBound="BudgetTypeRepeater_ItemDataBound">
                    <HeaderTemplate>
                        <table class="treecontent"  style="width:100%;">
                            <tr class="titleHeaderBackground tablerow">
                                <td class="headerCellStyle" width="35%" style="text-align:left;color:white;">Budget Item</td>
                                <td class="headerCellStyle" width="10%" style="color:white;">Start Date</td>
                                <td class="headerCellStyle" width="10%" style="color:white;">End Date</td>
                                <td class="headerCellStyle" width="15%" style="color:white;">Actual</td>
                                <td class="headerCellStyle" width="30%" style="text-align: left; padding-left: 10px; color:white;">Description</td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <table id="pnlAll"  class="treecontent tablerow" width="100%">
                    </HeaderTemplate>

                    <ItemTemplate>
                        <tr level="0" class="tablerow level0 budgetTypeBackground">
                              <td  width="35%" class="headerCellStyle" style="text-align:left; font-weight:bold" >
                                  <span>
                                      <img src="<%= minusURL%>" onclick="expandCollapse(this)" id="collapseImage" runat="server"/>
                                  </span>&nbsp;<asp:Label ID="lblBudgetType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetType") %>'></asp:Label>

                              </td>
                              <td  width="10%">&nbsp;</td>
                              <td  width="10%">&nbsp;</td>
                              <td  width="15%" class="headerCellStyle"><asp:Label ID="lblBudgetTypeTotal" runat="server" Text=""></asp:Label></td>
                              <td  width="30%">&nbsp;</td>
                        </tr>
                        <asp:Repeater ID="categoryRepeater" runat="server" OnItemDataBound="CategoryRepeater_ItemDataBound">
                           <ItemTemplate>
                                <tr level="1" class="tablerow categoryBackground">
                                    <td style="padding-left: 25px; text-align: left; font-weight: bold;"><span>
                                        <img src="<%= minusURL%>" onclick="expandCollapse(this)" alt="Collapse" /></span>
                                        <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetCategory") %>'></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td class="headerCellStyle">
                                        <asp:Label ID="lblCategoryTotal" runat="server" Text=""></asp:Label></td>
                                    <td>&nbsp;</td>
                                </tr>
                                <asp:Repeater ID="subCategoryRepeater" runat="server" OnItemDataBound="SubCategoryRepeater_ItemDataBound">
                                    <ItemTemplate>
                                        <tr level="2" class="tablerow subCategoryBackground">
                                            <td style="padding-left: 50px; font-weight: bold"><span>
                                                <img src="<%= minusURL%>" onclick="expandCollapse(this)" alt="Expand" /></span>
                                                <asp:Label ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetSubCategory") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td class="headerCellStyle">
                                                <asp:Label ID="lblSubCategoryTotal" runat="server" Text=""></asp:Label></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <asp:Repeater ID="ItemRepeater" runat="server" OnItemDataBound="ItemRepeater_ItemDataBound">
                                            <ItemTemplate>
                                                <tr level="3" class="tablerow">
                                                    <td style="padding-left: 75px; font-weight: bold">
                                                        <span>
                                                            <img src="<%= minusURL%>" onclick="expandCollapse(this)" /></span>&nbsp;
                                                        <asp:Label ID="lblBudgetItem" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetItem") %>'></asp:Label>

                                                    </td>
                                                    <td>&nbsp;<asp:HiddenField id="hdnItemId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ID") %>'/></td>
                                                    <td>&nbsp;</td>
                                                    <td class="headerCellStyle">
                                                        <asp:Label ID="lblActualTotal" runat="server" Text=""></asp:Label></td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <asp:Repeater ID="ItemDataRepeater" runat="server" OnItemDataBound="ItemDataRepeater_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr level="4" style="border: 1,solid;" class="itemBackground tablerow">
                                                            <td style="padding-left: 100px;"><asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' /></td>
                                                            <td class="dataCellStyle">
                                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AllocationStartDate") %>' /></td>
                                                            <td class="dataCellStyle">
                                                                <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AllocationEndDate") %>' /></td>
                                                            <td class="dataCellStyle">
                                                                <asp:Label ID="lblActual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actuals") %>' /></td>
                                                            <td style="padding-left: 10px;">
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetDescription") %>' /></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr level="4" class="tablerow">
                                                            <td style="padding-left: 100px;"><asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Title") %>' /></td>
                                                            <td class="dataCellStyle">
                                                                <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AllocationStartDate") %>' /></td>
                                                            <td class="dataCellStyle">
                                                                <asp:Label ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AllocationEndDate") %>' /></td>
                                                            <td class="dataCellStyle">
                                                                <asp:Label ID="lblActual" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actuals") %>' /></td>
                                                            <td style="padding-left: 10px;">
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetDescription") %>' /></td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                    <FooterTemplate>
                         <tr class="titleHeaderBackground">
                            <td class="leftAlign" style="color:white;font-weight:bold"><asp:Label ID="lblGTotalHeading" runat="server" Text="Grand Total"></asp:Label></td>
                            <td class="leftAlign" colspan="2">&nbsp;</td>
                            <td class="rightAlign headerCellStyle" style="color:white;"><asp:Label ID="lblGTotalActual" runat="server" Text=""></asp:Label></td>
                            <td class="leftAlign">&nbsp;</td>
                         </tr>
                        </table>
                        </td>
                        </tr>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
         <tr>
            <td colspan="2"  style="text-align:right;">
                <dx:ASPxButton runat="server" ID="btnClose" Text="Close">
                    <ClientSideEvents Click="function(){
                        closeReport();
                        }" />
                </dx:ASPxButton>
                <%--<asp:Button runat="server" id="btnClose" Text="Close" CssClass="input-button-bg" OnClientClick='SP.UI.ModalDialog.commonModalDialogClose(0, "Closed")' />--%>
            </td>
        </tr>
    </table>
</asp:Panel>

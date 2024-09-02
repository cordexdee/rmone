<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITGBudgetReport.ascx.cs" Inherits="uGovernIT.Web.ITGBudgetReport" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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

    .headerRowBackground
    {
        background-color: #E1E1E1;
    }

    .budgetTypebackground
    {
        background-color: #F0F0F0;
    }

    .categoryBackground
    {
        background-color: #F0F0F0;
    }

    .subCategoryBackground
    {
        background-color: #FAFAFA;
    }

    .itemBackground
    {
        background-color: #F7F7F7;
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
        background-color: #E1E1E1;
    }
    .budgetTypeBackground
    {
        background-color: #E1E1E1;
    }
</style>

<script type="text/javascript">
    $(function () {
        var print = '<%=printReport%>';
        if (print == "True") {
            window.print();
        }
    });

    function ExpCollapseAll(obj) {
        var isExp = true;
        if ($(obj).attr("src").indexOf("plus") == -1) {
            isExp = false;
        }

        if (isExp) {
            $(obj).attr("src", "/content/images/minus.png");
            $("#pnlAll").removeClass("hide");
        }
        else {
            $(obj).attr("src", "/content/images/plus.png");
            $("#pnlAll").addClass("hide");
        }
    }

    function expandCollapse(obj) {

        var currentTr = $(obj).parents("tr");
        var isExp = true;
        if ($(obj).attr("src").indexOf("plus") == -1) {
            isExp = false;
        }

        if (isExp) {
            $(obj).attr("src", "/Content/images/minus.png");
        }
        else {
            $(obj).attr("src", "/Content/images/plus.png");
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

    function getActualsData(obj) {

        if ($.trim($(obj).text()) == "$0.00") {
            closePopup();
            return false;
        }
        var startDate = '<%=StartDate%>';
        var endDate = '<%=EndDate%>';
        var itemId = $(obj).attr('item');
        var itemName = $(obj).attr('itemName');
       
        var dataVar = "{ 'startDate' : '" + startDate + "', 'itemId':'" + itemId + "', 'endDate':'" + endDate + "' }";

        var subCategory = $(obj).parents("tr [level='3']")

        $.ajax({
           type: "POST",
           url: "<%=ajaxPageURL%>/GetItgActualsData",
           data: dataVar,
           contentType: "application/json; charset=utf-8",
           dataType: "json",
           success: function (message) {
              
             var resultJson = $.parseJSON(message.d);

             $("#tblActual").html("");
             $("#<%=lblSelection.ClientID%>").text("");

               var headerRow = "<tr class='titleHeaderBackground' style='color:white;font-weight:bold'>"
                    + "<td style='width:33%;text-align:left;'> Start Date </td>"
                    + "<td style='width:33%;text-align:left;'> End Date </td>"
                    + "<td style='width:33%; text-align:right;'> Actual </td>"
                    + "</tr>"
                    $(headerRow).appendTo('#tblActual');
                    
                    $.each(resultJson, function (i, item) {
                    
                        var tblActual = $("#tblActual");

                        var tr = "<tr>";

                        if (i % 2 == 0)
                            tr = "<tr  class='tablerow itemBackground'>";

                        var newRow = tr +
                                        "<td  style='width:33%; text-align:left;'>" + item.BudgetStartDate + "</td>" +
                                        "<td  style='width:33%; text-align:left;'>" + item.BudgetEndDate + "</td>" +
                                        "<td  style='width:33%; text-align:right;'>" + "$" + item.Actual + "</td></tr>";

                        $(newRow).appendTo('#tblActual');
                        
                    });
                    positionActualsPopup(obj);


                    // #region: code to fetch category and subcategory name.
                    var subcategory = "";
                    var category = "";
                    var budgetType = "";

                    // Get the row index of clicked object
                    var index = $(obj).parent().parent().index();

                    // Get the total rows from table.
                    var rows = $("#tblReportTree").children("tbody").children("tr");

                    var stop = "false";
                    // start the reverse loop from index to row "0".

                    for (var x = index; x >= 0; x--) {

                        if (stop == "false") {

                            var row = $(rows[x]);
                            if (row.attr("level") == "3") {
                                item = $(row.children("td")[0]).text();
                            }

                            if (row.attr("level") == "2") {
                                subcategory = $(row.children("td")[0]).text();
                            }
                            else if (row.attr("level") == "1") {
                                category = $(row.children("td")[0]).text();
                            }
                            else if (row.attr("level") == "0") {
                                budgetType = $(row.children("td")[0]).text();
                                stop = "true";
                            }
                        }
                    }

               // Combine the headings.
                    if ($.trim(budgetType) == "") {
                        $("#<%=lblSelection.ClientID%>").text($.trim(category) + "/" + $.trim(subcategory) + "/" + $.trim(itemName));
                    }
                    else {
                        $("#<%=lblSelection.ClientID%>").text($.trim(budgetType) + "/" + $.trim(category) + "/" + $.trim(subcategory) + "/" + $.trim(itemName));
                    }

                    // end region
                },
           error: function (xhr, ajaxOptions, thrownError) {
            
           }
       });
        }


        function positionActualsPopup(obj) {
            $("#actualPopup").css({ 'display': 'block' });
            //$("#actualPopup").css({ 'top': '0px',  'display': 'block', 'left': '25%' });
            ////$("#actualPopup").css({ 'top': ($(obj).position().top - $("#actualPopup").height()) + 'px', 'display': 'block', 'left': ($(obj).position().left - $("#actualPopup").width()) + 'px' });
            //return false;
        }

        function closePopup() {
            $("#actualPopup").css({ 'display': 'none' });
        }

        function downloadExcel(obj) {
            $("#<%= hdnExportType.ClientID %>").val('Excel');
            $("#btnSubmitt").click();

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
           $("#<%= hdnExportType.ClientID %>").val('IMAGE');
           $("#btnSubmitt").click();
       }

    function selectAll(obj) {
        $(".chkBoxCategoryList input[type='checkbox']").each(function () {
            $(this).attr("checked", $("#<%= chkSelectAll.ClientID%>").get(0).checked);
        })
    }

    function startPrint(obj) {
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=print";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

</script>


<asp:HiddenField ID="hdnExportType" runat="server" value=""/>
<input type="submit" id="btnSubmitt"  class="hide" />
<div id="PnlBudgetReportPopup" runat="server" style="float:left;" class="ModuleBlock">
 <fieldset><legend style="font-weight:bold;">Non-Project Budget Report</legend>
<table cellspacing="10px" cellpadding="5px" >
    <tr>
        <td class="titleTD" width="100%" align="center" style="text-align:center">
            <asp:Panel ID="componentsForm" runat="server" style="float:left;padding-left:3px;width:100%;text-align:center;">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
               <tbody>
                   <tr>
                    <td align="center">
                    <table width="500px" class="filterpopup-table" frame="box" cellpadding="0" cellspacing="0" border="0">
                    <tr> 
                        <td style="width:50%; text-align:left;">
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
                         <td class="ms-formbody textBoxClass chkBoxCategoryList" style="width:50%; text-align:left;" >
                             <asp:CheckBoxList ID="chkBoxCategoryList" runat="server">
                             </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td  class="ms-formlabel labelClass" style="width:50%; text-align:left;">
                            <asp:Label ID="lblDateFrom" CssClass="filter-label" Text="Date From:" runat="server"></asp:Label>
                        </td>
                         <td class="ms-formbody textBoxClass DateControlDateFrom"  style="width:50%; text-align:left;">
                            <%--<SharePoint:DateTimeControl ID="dtDateFrom" runat="server" DateOnly="true" ></SharePoint:DateTimeControl>--%>
                              <dx:ASPxDateEdit ID="dtDateFrom" runat="server" ></dx:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                       <td  class="ms-formlabel labelClass" style="width:50%; text-align:left;">
                            <asp:Label ID="lblDateTo" CssClass="filter-label" Text="Date To:" runat="server" ></asp:Label>
                       </td>
                       <td class="ms-formbody textBoxClass DateControlDateTo" style="width:50%; text-align:left;">
                            <%--<SharePoint:DateTimeControl ID="dtDateTo" runat="server"  DateOnly="true" ></SharePoint:DateTimeControl>--%>
                           <dx:ASPxDateEdit ID="dtDateTo" runat="server" ></dx:ASPxDateEdit>
                       </td>
                    </tr>
                  </table>
                </td>
                </tr>
                 <tr> 
                    <td  class="ms-formlabel labelClass" style="text-align:right;padding-top:5px;" >
                        <div class="fright">
                            <asp:Button ID="btnRun" runat="server" CssClass="input-button-bg" Text="Run" OnClick="btnRun_Click"/>
                            <asp:Button ID="btnCancel" runat="server" CssClass="input-button-bg" Text="Cancel" OnClientClick="window.frameElement.commitPopup(); return false;" />
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
 <asp:HiddenField ID="exportURL" runat="server" />
<asp:Panel ID="pnlBudgetComponent" runat="server" Style="display: none;">
<table width="100%">
<tr>
<td style="text-align:center;" colspan="2">
    <asp:Label runat="server" id="lblHeading" Text="Report From" Font-Bold="true" Font-Size="Large"></asp:Label>
</td>

</tr>
<tr>
<td style="text-align:left;">
    <asp:Label runat="server" id="lblSubHeading" Text="Report From"></asp:Label>
</td>
<td style="text-align:right;">
        <div id="exportPanel" runat="server" style="background:#fff;display:block;float: right; border: 0px inset;" >
            <span  class="fleft" >
                 <img  src="/Content/images/excel-icon.png" alt="Excel" height="15" width="15" title="Export to Excel"  style="cursor:pointer;" onclick="downloadExcel(this);" />
            </span>
            <span class="fleft" style="padding:0px 3px;">
                    <img  src="/Content/images/pdf-icon.png" alt="Pdf" height="15" width="15" title="Export to Pdf" style="cursor:pointer;" onclick="downloadPDF(this);" />
            </span>
              <span  class="fleft" >
                <img  src="/Content/images/print-icon.png" alt="Print" title="Print" height="15" width="15"  style="cursor:pointer;" onclick="startPrint(this);"/>
              </span>
        </div>
</td>
</tr>

<tr> <td colspan="2">
<table width="100%"> 
<tr style="height:100%">
   <td style="text-align:left;">

<asp:Repeater ID="budgetTypeRepeater" runat="server"  OnItemDataBound="BudgetTypeRepeater_ItemDataBound">
       <HeaderTemplate>
           <table id="tblReport" cellpadding="0" cellspacing="0" class="treecontent tablerow" width="100%">
                <tr class="headerRowBackground">
                    <td class="headerCellStyle" width="25%" style="text-align:left;">Budget Item</td>
                    <td class="headerCellStyle" width="15%" style="text-align:left;">GL Code</td>
                    <td class="headerCellStyle" width="15%" style="text-align:left;">Department</td>
                    <td class="headerCellStyle" width="15%" >Planned</td>
                    <td class="headerCellStyle" width="20%" >Actual</td>
                    <td class="headerCellStyle" width="20%" style="padding-right:6px;">Variance</td>
                </tr>
               <tr>
                   <td colspan="6">
                        <table id="tblReportTree" cellpadding="0" cellspacing="0" class="treecontent tablerow" width="100%">
        </HeaderTemplate>
    <ItemTemplate>
            <tr level="0" class="tablerow level0 budgetTypeBackground">
                <td class="headerCellStyle" style="text-align:left; font-weight:bold"  width="25%"><span><img src="/Content/images/minus.png" onclick="expandCollapse(this)" id="collapseImage" runat="server"/></span>&nbsp;<asp:Label ID="lblBudgetType" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetType") %>'></asp:Label></td>
                <td  width="15%">&nbsp;</td>
                <td  width="15%">&nbsp;</td>
                <td class="headerCellStyle" width="15%"><asp:Label ID="lblPlannedTotalOnBudgetType" runat="server" Text=""></asp:Label></td>
                <td class="headerCellStyle" width="20%"><asp:Label ID="lblActualTotalOnBudgetType" runat="server" Text=""></asp:Label></td>
                <td class="headerCellStyle" width="20%"><asp:Label ID="lblVarianceTotalOnBudgetType" runat="server" Text="" ></asp:Label></td>
            </tr>
      <asp:Repeater ID="categoryRepeater" runat="server" OnItemDataBound="CategoryRepeater_ItemDataBound">
        <ItemTemplate>
           <tr level="1" class="tablerow level1 categoryBackground">
                <td style="text-align:left; padding-left:25px; font-weight:bold;"> <span><img src="/Content/images/minus.png" onclick="expandCollapse(this)" /></span>&nbsp; <asp:Label ID="lblCategory" runat="server"   Text='<%# DataBinder.Eval(Container.DataItem, "BudgetCategory") %>'></asp:Label></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="headerCellStyle"><asp:Label ID="lblPlannedTotalOnCategory" runat="server" Text=""></asp:Label></td>
                <td class="headerCellStyle"><asp:Label ID="lblActualTotalOnCategory" runat="server" Text=""></asp:Label></td>
                <td class="headerCellStyle"><asp:Label ID="lblVarianceTotalOncategory" runat="server" Text="" ></asp:Label></td>
            </tr>
           <asp:Repeater ID="subCategoryRepeater" runat="server" OnItemDataBound = "SubCategoryRepeater_ItemDataBound" >
             <ItemTemplate>
                <tr level="2" class="tablerow level2 subCategoryBackground">
                 <td class="" style="padding-left:50px; font-weight:bold"> <span><img src="/Content/images/minus.png" onclick="expandCollapse(this)" /></span>&nbsp; <asp:Label ID="lblSubCategory" runat="server" Text ='<%# DataBinder.Eval(Container.DataItem, "BudgetSubCategory") %>' /></td> 
                 <td>&nbsp;</td>
                 <td>&nbsp;</td>
                 <td class="headerCellStyle subCategoryBackground"><asp:Label ID="lblPlannedTotalOnSubcategory" runat="server" Text=""></asp:Label></td>
                 <td class="headerCellStyle subCategoryBackground"><asp:Label ID="lblSubCategoryTotal" runat="server" Text=""></asp:Label></td>
                 <td class="headerCellStyle subCategoryBackground"><asp:Label ID="lblVarianceTotalOnSubcategorybel" runat="server" Text=""></asp:Label></td>
                </tr>
                 <asp:Repeater ID="ItemRepeater" runat="server" OnItemDataBound = "ItemRepeater_ItemDataBound">
                 <ItemTemplate>
                    <tr level="3" class="tablerow level3">
                     <td style="padding-left:100px;"><asp:Label ID="lblBudgetItem" runat="server" Text ='<%# DataBinder.Eval(Container.DataItem, "BudgetItem") %>'></asp:Label> </td>
                     <td style="text-align:left;"><asp:Label ID="lblGLCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GLCode") %>'></asp:Label></td>
                     <td style="text-align:left;"><asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DepartmentLookup") %>'></asp:Label></td>
                     <td style="text-align:right;"><asp:Label ID="lblPlanedAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetAmount") %>'></asp:Label></td>
                     <td style="text-align:right;" class="actual"><a href="javascript:void(0);" item='<%# DataBinder.Eval(Container.DataItem, "ID") %>' itemName='<%# DataBinder.Eval(Container.DataItem, "BudgetItem") %>'  onclick="getActualsData(this)"><asp:Label ID="lblActualAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actuals") %>'></asp:Label></a></td>
                     <td style="text-align:right;"><asp:Label ID="lblVarianceAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Variance") %>'></asp:Label></td>
                    </tr>
                 </ItemTemplate>
                  <AlternatingItemTemplate>
                     <tr level="3" class="tablerow level3 itemBackground">
                     <td style="padding-left:100px;"><asp:Label ID="lblBudgetItem" runat="server" Text ='<%# DataBinder.Eval(Container.DataItem, "BudgetItem") %>'></asp:Label> </td>
                     <td style="text-align:left;"><asp:Label ID="lblGLCode" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GLCode") %>'></asp:Label></td>
                     <td style="text-align:left;"><asp:Label ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DepartmentLookup") %>'></asp:Label></td>
                     <td style="text-align:right;"><asp:Label ID="lblPlanedAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "BudgetAmount") %>'></asp:Label></td>
                     <td style="text-align:right;" class="actual"><a href="javascript:void(0);" item='<%# DataBinder.Eval(Container.DataItem, "ID") %>' itemName='<%# DataBinder.Eval(Container.DataItem, "BudgetItem") %>' onclick="getActualsData(this)"><asp:Label ID="lblActualAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actuals") %>'></asp:Label></a></td>
                     <td style="text-align:right;"><asp:Label ID="lblVarianceAmt" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Variance") %>'></asp:Label></td>
                    </tr>
                </AlternatingItemTemplate>
                 </asp:Repeater>
             </ItemTemplate>
            </asp:Repeater>
        </ItemTemplate>
    </asp:Repeater>
        </ItemTemplate>
     <FooterTemplate>
             <tr class="headerRowBackground">
                 <td class="leftAlign" style="font-weight:bold""><asp:Label ID="lblGrandTotalHeading" runat="server" Text="Grand Total"></asp:Label></td>
                 <td class="leftAlign">&nbsp;</td>
                 <td class="leftAlign">&nbsp;</td>
                 <td class="leftAlign headerCellStyle"><asp:Label ID="lblGTotalBudget" runat="server" Text=""></asp:Label></td>
                 <td class="leftAlign headerCellStyle"><asp:Label ID="lblGTotalActual" runat="server" Text=""></asp:Label></td>
                 <td class="rightAlign headerCellStyle"><asp:Label ID="lblGTotalVariance" runat="server" Text=""></asp:Label></td>
             </tr>
           </table>
             </td>
               </tr>
       
   </table>

        </FooterTemplate>
</asp:Repeater>
  
    </td>
    </tr>

    </table>

     </td></tr>
      <tr>
             <td colspan="2"  style="text-align:right;">
                 <div class="fright">
                     <asp:Button runat="server" id="btnClose" Text="Close" CssClass="input-button-bg" OnClientClick='SP.UI.ModalDialog.commonModalDialogClose(0, "Closed")' />
                 </div>
             </td>
         </tr>
    </table>
</asp:Panel>

<div style="text-align: center;">
<div id="actualPopup" style="width: 600px; top: 0px;left:20%;right:20%; display:none;" class="ModuleBlock" >
<table cellspacing = "10px" cellpadding="5px" width="100%">
 <tr>
 <td style="text-align:center;font-weight:bold;"><asp:Label ID="lblActualHeading" runat="server" Text="Actuals"></asp:Label></td> 
 </tr>
  <tr>
      <td style="text-align:left; font-weight:bold;">
          <asp:Label ID="lblSelection" runat="server" Text=""></asp:Label>
      </td>
    </tr>
    <tr >
        <td class="titleTD" width="100%" align="center" style="text-align:center">
            <table width="100%" cellspacing="0px" cellpadding="5px" style="border:1 solid;" id="tblActual">
           </table>
        </td>
    </tr>
    <tr>
      <td style="text-align:right">
          <div class="fright">
            <input type="button" id="btnCloseActual" value="Close" class="input-button-bg" onclick="closePopup()"/>
          </div>
      </td>
    </tr>
  
</table>
</div>
    </div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceSkillReportControl.ascx.cs" Inherits="uGovernIT.Web.ResourceSkillReportControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript">
         function downloadExcel(obj) {
             var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=excel";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    function startPrint(obj) {
        var exportUrl = '<%=urlBuilder%>';
        exportUrl += "&ExportReport=true&exportType=print";
        window.open(exportUrl, "_blank", "height=400,width=600,resizable=0,status=0,toolbar=0,location=0");
    }

    $(function () {
        var print = '<%=printReport%>';
        if (print == "True") {
            $("#s4-titlerow").remove();
            window.print();
        }
    });

    function ClosePopUp() {
        var sourceURL = "<%= Request["source"] %>";
         sourceURL += "**stoprefreshpage";
         window.frameElement.commitPopup(sourceURL);
    }

    function openResourceProfileInfo(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, 0);
    }
</script>

<table width="100%" >
     <tr>
         <td style="text-align:center;font-weight:bold;font-size:medium;">
             <asp:Label ID="lblReportTitle" runat="server" Text="Allocation FTEs by Skill (Resource Demand)"></asp:Label>
         </td>
     </tr>
     <tr>
     <td>
     <span class="fleft" style="padding-left:2px;padding-bottom: 3px;">
            <asp:Label ID="lblReportDateRange" runat="server" Text=""></asp:Label>
     </span>

     <span class="fright" style="padding-right:2px;">
            <asp:Label ID="lblRightHeading" runat="server" Text=""></asp:Label>
     </span>
</td>
</tr>
     <tr>
         <td>
             <div style="float:left">
                 
                                    <dx:ASPxImage runat="server" ID="imgExpand" Text="Expand All Rows" ImageUrl="~/_layouts/15/images/ugovernit/expand-all.png"
                                        AutoPostBack="false">
                                        <ClientSideEvents Click="function() { grid.ExpandAll() }" />
                                    </dx:ASPxImage>
                                
                                    <dx:ASPxImage runat="server" ID="imgCollapse" Text="Collapse All Rows" ImageUrl="~/_layouts/15/images/ugovernit/collapse-all.png"
                                        AutoPostBack="false">
                                        <ClientSideEvents Click="function() { grid.CollapseAll() }" />
                                    </dx:ASPxImage>
                                
             </div>
          <div id="exportPanel" runat="server" style="background:#fff;display:block;float: right; border: 0px inset;padding-bottom:4px;" >

              <span class="fleft">
                                <dx:ASPxButton ID="ASPxButton1" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                    OnClick="btnExcelExport_Click">
                                    <Image Url="/Content/images/excel-icon.png" Height="20px" />
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>
                            <span class="fleft" style="padding-left: 3px;">
                                <dx:ASPxButton ID="ASPxButton2" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                    OnClick="btnPdfExport_Click">
                                    <Image Url="/Content/images/Pdf-icon.png" />
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>

                            <span class="fleft" style="padding-left: 3px;display:none;">
                                <dx:ASPxButton ID="ASPxButton3" runat="server" EnableTheming="false" RenderMode="Link"  UseSubmitBehavior="False"
                                    OnClick="btnPrint_Click">
                                    <Image Url="/Content/images/print-icon.png" />
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>
        </div>
       </td>
  </tr>

 <tr style="height:90%">
    <td>
        <div>
           <dx:ASPxGridView ID="gvResourceReport" runat="server" AutoGenerateColumns="false"
                        Width="100%"  OnDataBinding="gvResourceReport_DataBinding" ClientInstanceName="grid"  OnCustomSummaryCalculate="gvResourceReport_CustomSummaryCalculate"
                        OnHtmlRowPrepared="gvResourceReport_HtmlRowPrepared" OnHeaderFilterFillItems="gvResourceReport_HeaderFilterFillItems">
                        <SettingsBehavior AllowGroup ="true" AutoExpandAllGroups="true" />
                        <Settings ShowGroupedColumns ="false" ShowGroupFooter="VisibleIfExpanded" ShowFooter="false"  />
                    </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvResourceReport"></dx:ASPxGridViewExporter>
        </div>    
    </td>
 </tr>
 <tr >
    <td>
        <table runat="server" id="footerTable">
            
        </table>
    </td>
 </tr>
 <tr style="height:10%">
    <td style="text-align:right";>
        <div style="float:right">
            <asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="ClosePopUp()"/>
        </div>
    </td>
 </tr>
 </table>
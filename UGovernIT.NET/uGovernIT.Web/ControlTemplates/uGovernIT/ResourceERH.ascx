<%--<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"--%>
<%--<%@  Register Assembly="DevExpress.Web.v18.2, Version=18.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx"
 Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceERH.ascx.cs" Inherits="uGovernIT.ControlTemplates.uGovernIT.ResourceERH" %>




<style type="text/css">
       body{overflow:auto !important;}
     #s4-leftpanel {
       display: none;
      }
      .s4-ca {
       margin-left:0px !important;
      }
      #s4-ribbonrow{height:auto !important;min-height:0px !important;}
     #s4-ribboncont{display:none;}
    #s4-titlerow{ display:none;}
    .s4-ba {width:100%; min-height:0px !important;}
    #s4-workspace{float:left;width:100%; overflow:auto  !important;}
    body #MSO_ContentTable{min-height:0px !important;position:inherit;}
    
    .dataCellStyle{text-align:center !important; padding: 0px 1px 0px 0 !important;}
    .footerCell{font-weight:bold; margin-top:5px !important; margin-bottom:5px !important; text-align:center;border-top:2px solid !important;border-bottom:1px solid !important;}
    .totalrowstyle{font-weight:bold;border-color:#000 !important;border-top:2px solid !important;border-bottom:1px solid !important;}
    .headerCellStyle{font-weight:bold; margin-top:5px !important; margin-bottom:5px !important; text-align:center;padding-right:7px !important;}
    .subtotalrowstyle{font-weight:bold;border-color:#000 !important;border-top:2px solid !important;border-bottom:1px solid !important;text-align:center !important;}
    .subtotalrowstyletitle{font-weight:bold;border-color:#000 !important;border-top:2px solid !important;border-bottom:1px solid !important;text-align:right !important;}
    .htmlFooterCell{width:67px; font-weight:bold; margin-top:5px !important; margin-bottom:5px !important; text-align:center;border-bottom:1px solid !important;}
    .htmlHeaderCell{width:140px; font-weight:bold; margin-top:5px !important; margin-bottom:5px !important; text-align:center;border-bottom:1px solid !important;}
    .dxgvIndentCell, .dxgvIndentCell dxgv, .dxgvDataRow td.dxgvIndentCell .Hide
{
   border: none; 
   border-right:none;  
}

    .gerhr-row td.dxgvIndentCell, .gerhr-grouprow td.dxgvIndentCell, .gerhr-groupfooter td.dxgvIndentCell {
    border: none; 
   border-right:none;  
    }
</style>

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
</script>


<table width="100%" >
     <tr>
         <td style="text-align:center;font-weight:bold;font-size:medium;">
             <asp:Label ID="lblReportTitle" runat="server" Text="Estimated Remaining Hours"></asp:Label>
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
          <div id="exportPanel" runat="server" style="background:#fff;display:block;float: right; border: 0px inset;" >
              <span class="fleft">
                                <dx:ASPxButton ID="btnExcelExport" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                    OnClick="btnExcelExport_Click">
                                    <Image Url="/_layouts/15/images/ugovernit/excel-icon.png" />
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>
                            <span class="fleft" style="padding-left: 3px;">
                                <dx:ASPxButton ID="btnPdfExport" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                    OnClick="btnPdfExport_Click">
                                    <Image Url="/_layouts/15/images/ugovernit/Pdf-icon.png" />
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>

                            <span class="fleft" style="padding-left: 3px;">
                                <dx:ASPxButton ID="btnPrint" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                    OnClick="btnPrint_Click">
                                    <Image Url="/_layouts/15/images/ugovernit/print-icon.png" />
                                    <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                </dx:ASPxButton>
                            </span>
        </div>
       </td>
  </tr>
  

 <tr style="height:90%">
    <td>
        <div>

           <dx:ASPxGridView ID="gvEstimatedRemaningHoursReport" runat="server" AutoGenerateColumns="false" OnDataBound="gvEstimatedRemaningHoursReport_DataBound"
                        Width="100%"  OnDataBinding="gvEstimatedRemaningHoursReport_DataBinding" ClientInstanceName="gvEstimatedRemaningHoursReport"
               SettingsText-EmptyDataRow="no record found.">
               <Columns>
                   <dx:GridViewDataTextColumn FieldName="Type" Caption="Type" GroupIndex="0"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="WorkItemLink" Caption="Work Item" PropertiesTextEdit-EncodeHtml="false" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="SubWorkItemLink" Caption="Sub Item" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="ActualHours" Caption="Actual to Date" Width="100px" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="EstimatedHours" Caption="Estimate" Width="80px" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="ActualVariance" Caption="Actual vs Estimate" Width="120px" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="EstimatedRemainingHours" Caption="ERH" Width="50px" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="Projected" Caption="Projected" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="ProjectedEstimate" Caption="Projected vs Estimate" Width="120px" CellStyle-HorizontalAlign="Center"></dx:GridViewDataTextColumn>
                   <dx:GridViewDataTextColumn FieldName="UGITComment" Caption="Comments" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
               </Columns>
                        <SettingsBehavior AllowGroup ="true" AutoExpandAllGroups="true" />
                       <%-- <Settings ShowGroupedColumns ="false" ShowGroupFooter="VisibleIfExpanded" ShowFooter="false"  />--%>
                         <Settings GroupFormat="{1}" />
                         <SettingsPager Mode="ShowAllRecords"></SettingsPager>
               <Styles>
                   <Row CssClass="gerhr-row"></Row>
                   <GroupRow CssClass="gerhr-grouprow"></GroupRow>
                   <GroupFooter CssClass="gerhr-groupfooter"></GroupFooter>
               </Styles>
                    </dx:ASPxGridView>

                    <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvEstimatedRemaningHoursReport"></dx:ASPxGridViewExporter>
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
            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="input-button-bg" />
        </div>
    </td>
 </tr>
 </table>
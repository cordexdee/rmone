<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskSummary.aspx.cs" Inherits="uGovernIT.Report.DxReport.TaskSummary" MasterPageFile="~/Default.master" %>
<%@ Register Assembly="DevExpress.XtraReports.v22.1.Web.WebForms, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>






<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">


    <style type="text/css">
            .first_tier_nav ul {
                float: right;
            }

      

        .maindiv {
            margin-bottom: 10px;
            margin-left: 10px;
            margin-right: 10px;
        }
    </style>

    <script type="text/javascript">
        function CloseChildren(level, id, imageObj) {
            imageObj.setAttribute("src", "/Content/images/plus.gif");
            imageObj.setAttribute("onclick", "OpenChildren('" + level + "', '" + id + "', this)");
            var startingRow = $(imageObj).parents("tr").get(0);
            var nextRow = startingRow.nextSibling;

            var nextRowLevel = nextRow.getAttribute('level');
            while (nextRowLevel > level) {
                nextRow.style.display = "none";
                nextRow = nextRow.nextSibling;
                nextRowLevel = nextRow.getAttribute('level');
            }
        }
        function OpenChildren(level, id, imageObj) {
            imageObj.setAttribute("src", "/Content/images/minus.gif");
            imageObj.setAttribute("onclick", "CloseChildren('" + level + "', '" + id + "', this)");
            var startingRow1 = $(imageObj).parents("tr").get(0);
            var nextRow1 = startingRow1.nextSibling;

            var nextRowLevel1 = nextRow1.getAttribute('level');
            while (nextRowLevel1 > level) {
                nextRow1.style.display = "";
                nextRow1 = nextRow1.nextSibling;

                nextRowLevel1 = nextRow1.getAttribute('level');
            }
        }

        function openForPrintDetail() {
            window.open(window.location.href + "&enablePrint=true");
        }

        $(function () {
        <%if (enablePrint)
          { %>
          //Remove all hidden elements according to jquery rule
          $("div:hidden").remove();
          $("#s4-titlerow").remove();
          window.print();
          <%} %>

          CollapseAll();
      });

      function ExpandAll() {
          var rows = document.getElementsByTagName('tr');
          var numRows = rows.length;
          for (var i = 0; i < numRows; ++i) {
              if (rows[i].getAttribute("isexp") != null && rows[i].getAttribute("isexp").toLowerCase() == "false") {
                  if ($(rows[i].firstChild.firstChild).find('a') != null)
                      $(rows[i].firstChild.firstChild).find("a[title^='Expand/Collapse']").click();
              }
          }
      }

      function CollapseAll() {
          var rows = document.getElementsByTagName('tr');
          var numRows = rows.length;
          for (var i = 0; i < numRows; ++i) {
              if (rows[i].getAttribute("isexp") != null && rows[i].getAttribute("isexp").toLowerCase() == "true") {
                  if ($(rows[i].firstChild.firstChild).find('a') != null)
                      $(rows[i].firstChild.firstChild).find("a[title^='Expand/Collapse']").click();
              }
          }
      }

      function OpenSendEmailWindow(url) {
          //var url = hdnConfiguration.Get("SendEmailUrl");
          var requestUrl = hdnConfiguration.Get("RequestUrl");
          window.parent.UgitOpenPopupDialog(url, '', 'Send Email - Task Summary Report', '800px', '600px', 0, escape(requestUrl))
          return false;
      }


      function SendMailClick() {
          loadingpanel.Show();
          cbMailsend.PerformCallback("SendMail");
      }


      function SaveToDocument() {
          loadingpanel.Show();
          cbMailsend.PerformCallback("SaveToDoc");
      }

      function OpenSaveToDocument(url) {
          //var url = hdnconfiguration.Get("SaveToDocumentUrl");
          var requestUrl = hdnConfiguration.Get("RequestUrl");
          var param = "type=tsksummaryreport&IsSelectFolder=true";
          window.parent.UgitOpenPopupDialog(url, param, 'Save', '400px', '200px', 0, escape(requestUrl))
          return false;
      }

        function OnCallbackComplete(s, e) {
            debugger;
          loadingpanel.Hide();
          var result = e.result
          if (result != null && result.length > 0) {
              if (e.parameter.toString() == "SendMail") {
                  OpenSendEmailWindow(result);
              }
              else if (e.parameter.toString() == "SaveToDoc") {
                  OpenSaveToDocument(result);
              }
          }
          else {
              alert("No record found.");
          }
      }

    </script>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxHiddenField ID="hdnConfiguration" runat="server" ClientInstanceName="hdnConfiguration"></dx:ASPxHiddenField>
    <dx:ASPxLoadingPanel ID="loadingpanel" runat="server" Modal="true" ClientInstanceName="loadingpanel" ></dx:ASPxLoadingPanel>
<dx:ASPxCallback ID="cbMailsend" runat="server"  ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback" >
    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
</dx:ASPxCallback>
    <asp:Panel ID="mainActionPanel" runat="server" CssClass="maindiv">
        <table style="width: 100%;">
            <tr>
                <td>
                    <div style="float: left;">
                        <table>
                            <tr>
                                <td>
                                    <img src="/Content/images/expand-all.png" onclick="gridTaskSummary.ExpandAll();" />
                                </td>
                                <td>
                                    <img src="/Content/images/collapse-all.png" onclick="gridTaskSummary.CollapseAll();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="text-align: right;">
                    <div style="float: right;width:86px;">
                        <table>
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btnPdfExport" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                        OnClick="btnPdfExport_Click">
                                        <Image Url="/Content/images/Pdf-icon.png" />
                                        <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btnExcelExport" runat="server" RenderMode="Link" EnableTheming="false"  UseSubmitBehavior="False"
                                        OnClick="btnExcelExport_Click">
                                        <Image Url="/Content/images/excel-icon.png" />
                                        <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <%--<dx:ASPxButton ID="btnPrint" runat="server" EnableTheming="false"  UseSubmitBehavior="False"
                                         OnClick="btnPrint_Click"  >
                                        <Image Url="/_layouts/15/images/ugovernit/print-icon.png"  />
                                        <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                                    </dx:ASPxButton>--%>
                                    <%--<div class="first_tier_nav">
                                        <ul>
                                            <li runat="server" id="printButtonLI" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                                <a style="color: white" href="javascript:" onclick="openForPrintDetail()" title="Print Summary" class="print">Print</a>
                                            </li>
                                        </ul>
                                    </div>--%>
                                    <dx:ASPxButton ID="SendEmail" runat="server" RenderMode="Link" EnableTheming="false" 
                                         UseSubmitBehavior="False" AutoPostBack="false">
                                        <Image Url="/Content/Images/MailTo.png" />
                                        <ClientSideEvents click="function(s,e) { SendMailClick(); }" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="SaveDocument" runat="server" RenderMode="Link" EnableTheming="false" 
                                         UseSubmitBehavior="False" AutoPostBack="false" >
                                        <Image Url="/Content/images/saveToFolder.png"/>
                                        <ClientSideEvents click="function(s,e) { SaveToDocument(); }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnl1" runat="server">
                        <dx:ASPxGridView ID="gridTaskSummary" runat="server" ClientInstanceName="gridTaskSummary" OnDataBinding="gridTaskSummary_DataBinding"
                            OnHtmlRowPrepared="gridTaskSummary_HtmlRowPrepared" AutoGenerateColumns="false"
                            Width="100%" OnCustomColumnDisplayText="gridTaskSummary_CustomColumnDisplayText"  >
                            <Templates>
                                <GroupRowContent>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblProject" Text='<%# Eval("TicketId") %>' runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblDescription" Text='<%# Eval("TitleWithPctComplete") %>' runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </GroupRowContent>
                            </Templates>
                        </dx:ASPxGridView>
                    <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridTaskSummary" ></dx:ASPxGridViewExporter>
                    </asp:Panel>
                    
                </td>
            </tr>
        </table>
        </asp:Panel>
    <div style="display:none;">
        
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" Visible="false">
   
</asp:Content>

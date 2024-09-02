<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceReport.ascx.cs" Inherits="uGovernIT.Web.ResourceReport" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .dataCellStyle {
            text-align: center !important;
            padding: 0px 1px 0px 0 !important;
        }
        .footerCell {
            font-weight: bold;
            margin-top: 5px !important;
            margin-bottom: 5px !important;
            text-align: center;
            border-top: 2px solid !important;
            border-bottom: 1px solid !important;
        }

        .totalrowstyle {
            font-weight: bold;
            border-color: #000 !important;
            border-top: 2px solid !important;
            border-bottom: 1px solid !important;
        }

        .headerCellStyle {
            font-weight: bold;
            margin-top: 5px !important;
            margin-bottom: 5px !important;
            text-align: center;
            padding-right: 7px !important;
        }

        .subtotalrowstyle {
            font-weight: bold;
            border-color: #000 !important;
            border-top: 2px solid !important;
            border-bottom: 1px solid !important;
            text-align: center !important;
        }

        .subtotalrowstyletitle {
            font-weight: bold;
            border-color: #000 !important;
            border-top: 2px solid !important;
            border-bottom: 1px solid !important;
            text-align: right !important;
        }

        .htmlFooterCell {
            width: 67px;
            font-weight: bold;
            margin-top: 5px !important;
            margin-bottom: 5px !important;
            text-align: center;
            border-bottom: 1px solid !important;
        }

        .htmlHeaderCell {
            width: 140px;
            font-weight: bold;
            margin-top: 5px !important;
            margin-bottom: 5px !important;
            text-align: center;
            border-bottom: 1px solid !important;
        }

        .dxgvIndentCell, .dxgvIndentCell dxgv, .dxgvDataRow td.dxgvIndentCell .Hide {
            border: none;
            border-right: none;
        }

        .resourcereport-row td.dxgvIndentCell, .resourcereport-grouprow td.dxgvIndentCell, .resourcereport-footerrow td.dxgvIndentCell {
            border: none;
            border-right: none;
        }

        .clsseperator {
            border-left: 1px solid #cacbd3 !important;
        }
    </style>


    <script type="text/javascript"  data-v="<%=UGITUtility.AssemblyVersion %>">

        function NextYear() {
            var year = parseInt(document.getElementById("<%=hdnYear.ClientID%>").value) + 1;
            document.getElementById("<%=hdnYear.ClientID%>").value = year;
            document.getElementById("<%=lblProjectYear.ClientID%>").innerHTML = year;

        }

        function PreviousYear() {
            var year = parseInt(document.getElementById("<%=hdnYear.ClientID%>").value) - 1;
            document.getElementById("<%=hdnYear.ClientID%>").value = year;
            document.getElementById("<%=lblProjectYear.ClientID%>").innerHTML = year;
        }

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

        $(function () {
            var tdarr = $('.clsalignright').closest('td');
            $.each(tdarr, function (index) {
                var current = tdarr[index];
                $(current).css({ 'text-align': 'right', 'border-left': '1px solid #cacbd3' });
            });

            var tdleftarr = $('.clsalignleft').closest('td');//
            $.each(tdleftarr, function (index) {
                var current = tdleftarr[index];
                $(current).css('text-align', 'left');
            });
        });
    </script>


<asp:Panel ID="pnlReport" runat="server">

        <table width="100%">
            <tr>
                <td style="text-align: center; font-weight: bold; font-size: medium;padding-bottom:5px;">
                    <asp:Label ID="lblReportTitle" runat="server" Text="Resource Allocation Report"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="fleft" style="padding-left: 2px; padding-bottom: 3px;">
                        <asp:Label ID="lblReportDateRange" runat="server" Text=""></asp:Label>
                    </span>

                    <span class="fright" style="padding-right: 2px;">
                        <asp:Label ID="lblRightHeading" runat="server" Text=""></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="float: left">
                        <dx:ASPxImage runat="server" ID="imgExpand" Text="Expand All Rows" ImageUrl="~/Content/images/expand-all.png"
                            AutoPostBack="false">
                            <ClientSideEvents Click="function() { grid.ExpandAll() }" />
                        </dx:ASPxImage>
                        <dx:ASPxImage runat="server" ID="imgCollapse" Text="Collapse All Rows" ImageUrl="~/Content/images/collapse-all.png"
                            AutoPostBack="false">
                            <ClientSideEvents Click="function() { grid.CollapseAll() }" />
                        </dx:ASPxImage>
                    </div>
                    <div id="exportPanel" runat="server" style="background: #fff; display: block; float: right; border: 0px inset; padding-bottom:3px;">
                        <span class="fleft">
                            <dx:ASPxButton ID="ASPxButton4" runat="server" RenderMode="Link" EnableTheming="false" UseSubmitBehavior="False"
                                OnClick="ASPxButton4_Click">
                                <Image Url="/Content/images/csv-icon16X16.png" />
                                <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                            </dx:ASPxButton>
                        </span>
                        <span class="fleft" style="padding-left: 3px;">
                            <dx:ASPxButton ID="ASPxButton1" runat="server" RenderMode="Link" EnableTheming="false" UseSubmitBehavior="False"
                                OnClick="ASPxButton1_Click">
                                <Image Url="/Content/images/excel-icon.png" Height="20px" />
                                <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                            </dx:ASPxButton>
                        </span>
                        <span class="fleft" style="padding-left: 3px;">
                            <dx:ASPxButton ID="ASPxButton2" runat="server" RenderMode="Link" EnableTheming="false" UseSubmitBehavior="False"
                                OnClick="ASPxButton2_Click">
                                <Image Url="/Content/images/Pdf-icon.png" />
                                <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                            </dx:ASPxButton>
                        </span>

                        <span class="fleft" style="padding-left: 3px;display:none;">
                            <dx:ASPxButton ID="ASPxButton3" runat="server" RenderMode="Link" EnableTheming="false" UseSubmitBehavior="False"
                                OnClick="ASPxButton3_Click">
                                <Image Url="/Content/images/print-icon.png" />
                                <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                            </dx:ASPxButton>
                        </span>
                    </div>
                </td>
            </tr>
            <tr id="trYearSelection" runat="server">
                <td>
                    <span class="fleft" style="padding-right: 5px;">
                        <asp:ImageButton ImageUrl="/Content/images/Previous16x16.png" ID="previousWeek"
                            runat="server" OnClientClick="PreviousYear()" />
                    </span>

                    <span class="fleft calenderyearnum">
                        <asp:Label ID="lblProjectYear" runat="server" Style="font-weight: bold"></asp:Label>
                        <asp:HiddenField ID="hdnYear" runat="server" Value="" />
                    </span>

                    <span class="fleft" style="padding-left: 5px;">
                        <asp:ImageButton ImageUrl="/Content/images/Next16x16.png" ID="nextWeek"
                            runat="server" OnClientClick="NextYear()" />
                    </span>

                    <span>
                        <asp:Button ID="btnActual" runat="server" Text="Show Actuals" OnClick="btnActual_Click" Visible="false" />
                    </span>
                </td>
            </tr>
                        <dx:ASPxGridViewExporter ID="gvResourceReportExporter" GridViewID="gvResourceReport" runat="server"></dx:ASPxGridViewExporter>

            <tr style="height: 90%">
                <td>
                    <div>
                        <ugit:ASPxGridView ID="gvResourceReport" runat="server" AutoGenerateColumns="false"
                            Width="100%" OnDataBinding="gvResourceReport_DataBinding" ClientInstanceName="grid"
                            OnHtmlRowPrepared="gvResourceReport_HtmlRowPrepared" OnHtmlDataCellPrepared="gvResourceReport_HtmlDataCellPrepared" OnHeaderFilterFillItems="gvResourceReport_HeaderFilterFillItems">
<%--                            <Toolbars>
                                    <dx:GridViewToolbar>
                                    <Items>
                                        <dx:GridViewToolbarItem Command="ExportToPdf" />
                                        <dx:GridViewToolbarItem Command="ExportToXlsx" />
                                        <dx:GridViewToolbarItem Command="ExportToCsv" />
                                    </Items>
                                </dx:GridViewToolbar>
                            </Toolbars>--%>
                            <SettingsExport EnableClientSideExportAPI="true" ExcelExportMode="WYSIWYG" SplitDataCellAcrossPages="true" FileName="Resource_Summary_Report"
                                ReportHeader ="Resource Summary Report" PageFooter-Center="Page [Page # of Pages #]" PageHeader-Center="[Date Printed]"/>
                            <SettingsCustomizationDialog Enabled="true" />
                            <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" />
                            <Settings ShowGroupedColumns="false" ShowGroupFooter="VisibleIfExpanded" ShowFooter="true" />
                            <Styles>
                                <Row CssClass="resourcereport-row"></Row>
                                <GroupRow CssClass="resourcereport-grouprow"></GroupRow>
                                <GroupFooter CssClass="resourcereport-footerrow"></GroupFooter>

                            </Styles>
                        </ugit:ASPxGridView>
<%--                        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" Landscape="true" GridViewID="gvResourceReport"></dx:ASPxGridViewExporter>--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <table runat="server" id="footerTable">
                    </table>
                </td>
            </tr>
            <tr style="height: 10%">
                <td style="text-align: right;">
                    <div style="float: right;padding-top:5px;">
                        <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </asp:Panel>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BottleNeckTrendChart.ascx.cs" Inherits="uGovernIT.Web.BottleNeckTrendChart" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register assembly="DevExpress.XtraCharts.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraCharts" tagprefix="cc1" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ValidateDate() {        
        var diff = CheckDifference();
        if (diff < 0) {
            alert("From Date can't be greater than To Date");
            return false;
        }       
        return true;
    }

    function CheckDifference() {
        var startDate = new Date();
        var endDate = new Date();
        var difference = -1;
        startDate = dtcStartDate.GetDate();
        if (startDate != null) {
            endDate = dtcEndDate.GetDate();
            var startTime = startDate.getTime();
            var endTime = endDate.getTime();
            difference = (endTime - startTime) / 86400000;

        }
        return difference;
    }
</script>

<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="90%" id="tblMain" runat="server">        
    <tr id="tr5" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Date From</h3>
        </td>
        <td class="ms-formbody">            
            <dx:ASPxDateEdit ID="dtcStartDate" ClientInstanceName="dtcStartDate" EditFormat="Date" CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit" runat="server" AutoPostBack="false"></dx:ASPxDateEdit>
        </td>
            <td class="ms-formlabel">
            <h3 class="ms-standardheader">Date To</h3>
        </td>
        <td class="ms-formbody">            
            <dx:ASPxDateEdit ID="dtcEndDate" ClientInstanceName="dtcEndDate" EditFormat="Date" CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit" runat="server" AutoPostBack="false"></dx:ASPxDateEdit>
        </td>

            <td class="ms-formlabel">
            <h3 class="ms-standardheader">Type</h3>
        </td>
        <td class="ms-formbody">
            <asp:DropDownList ID="ddlDateType" runat="server">
                <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
                <asp:ListItem Text="Week" Value="Week"></asp:ListItem>
                <asp:ListItem Text="Month" Value="Month"></asp:ListItem>
            </asp:DropDownList>
        </td>

        <td>
            <asp:ImageButton ID="btnRefresh" runat="server" ImageUrl="/Content/images/refresh-icon.png" ToolTip="Refresh" Style="margin-bottom: -4px; margin-left: 5px;" OnClick="btnRefresh_Click" OnClientClick="return ValidateDate()" />  <%-- OnClick="btnRefresh_Click" OnClientClick="ShowLoader()"--%>
        </td>
    </tr>    
</table>


<dxchartsui:WebChartControl ID="WebChartControl1" runat="server"  Height="500px" Width="800px" CrosshairEnabled="True" ClientInstanceName="webChart">

  <DiagramSerializable>
            <cc1:XYDiagram LabelsResolveOverlappingMinIndent="3">
              <%--  <AxisX Title-Text="Months" VisibleInPanesSerializable="-1">
                    <DateTimeScaleOptions MeasureUnit="Month" GridAlignment="Month"/>
                    <Label Staggered="True" textPattern="{A:y}">
                    </Label>
                </AxisX>--%>
                <AxisY Title-Text="Change" VisibleInPanesSerializable="1">

                </AxisY>
            </cc1:XYDiagram>
        </DiagramSerializable>

<FillStyle><OptionsSerializable>
<cc1:SolidFillOptions></cc1:SolidFillOptions>
</OptionsSerializable>
</FillStyle>
<CrosshairOptions><CommonLabelPositionSerializable>
<cc1:CrosshairMousePosition></cc1:CrosshairMousePosition>
</CommonLabelPositionSerializable>
</CrosshairOptions>
<ToolTipOptions showforseries="True"><ToolTipPositionSerializable>
<cc1:ToolTipMousePosition></cc1:ToolTipMousePosition>
</ToolTipPositionSerializable>
</ToolTipOptions>
 </dxchartsui:WebChartControl>
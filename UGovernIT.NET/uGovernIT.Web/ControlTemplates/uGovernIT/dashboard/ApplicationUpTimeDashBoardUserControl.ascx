
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationUpTimeDashBoardUserControl.ascx.cs" Inherits="uGovernIT.Web.ApplicationUpTimeDashBoardUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxp" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<asp:HiddenField ID="hdnCurrentYear" runat="server" />

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $($(".ShowUptimeDtls").parents("div")).css("z-index", 1000);
    });
    function showOutageDetails(value, object) {
        var obj = $("." + object).get(0).id;
        var year = $("#<%=lblSelectedYear.ClientID%>").text();
        value += "_" + year;
        var url = "<%=delegateUrl %>";
        var months = ["\x01", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        params = "control=incuptimedashboard&value=" + value + "";
        var month = months[value.split("_")[0]];
        var applicationName = value.split("_")[1];
        var title = "Incidents for " + applicationName + ": " + month + " " + year;// <application>: May 2017
        window.parent.UgitOpenPopupDialog(url, params, title, '90', '90', 0);

    }
    function previousYrsClick() {
        var year = parseInt($("#<%=lblSelectedYear.ClientID%>").text());
        $("#<%=hdnCurrentYear.ClientID%>").val(year - 1);
        if (aspxCallBackYearFilter != null)
            aspxCallBackYearFilter.PerformCallback();
    }
    function nextYrsClick() {
        var year = parseInt($("#<%=lblSelectedYear.ClientID%>").text());
        $("#<%=hdnCurrentYear.ClientID%>").val(year + 1);
        if (aspxCallBackYearFilter != null)
            aspxCallBackYearFilter.PerformCallback();
    }
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .spanApplicationFilter {
        float: left;
    }

    .spanYearFilter {
        float: left;
        padding: 2px 0px 0px 10px;
    }

    .lblHeading {
        font-size: 12px;
        font-weight: bold;
        padding-right: 10px;
    }

    .ShowUptimeDtls {
        z-index: 100 !important;
    }

    .lblSelectedYear {
        top: 3px !important;
        position: relative;
    }
</style>

<dx:ASPxCallbackPanel ID="aspxCallBackYearFilter" runat="server" ClientInstanceName="aspxCallBackYearFilter" OnCallback="aspxCallBackYearFilter_Callback1">
    <PanelCollection>
        <dx:PanelContent>
            <span id="spanYearFilter" class="spanYearFilter" runat="server">
                <span>
                    <dx:ASPxLabel ID="lblHeadings" CssClass="lblHeading" runat="server" Text="Application/Area Uptime:"></dx:ASPxLabel>
                    <dx:ASPxButton ID="previousYrs" runat="server" AutoPostBack="False" AllowFocus="False" RenderMode="Link" EnableTheming="False">
                        <Image Url="/Content/images/Previous16x16.png">
                        </Image>
                        <ClientSideEvents Click="function(s,e){previousYrsClick();}" />
                    </dx:ASPxButton>
                </span>
                <asp:Label ID="lblSelectedYear" CssClass="lblSelectedYear" runat="server"></asp:Label>
                <span>
                    <dx:ASPxButton ID="nextYrs" runat="server" AutoPostBack="False" AllowFocus="False" RenderMode="Link" EnableTheming="False">
                        <Image Url="/Content/images/Next16x16.png">
                        </Image>
                        <ClientSideEvents Click="function(s,e){nextYrsClick();}" />
                    </dx:ASPxButton>
                </span>
            </span>

            <br />
            <br />

            <span style="width: 90%">
                <dxp:ASPxPivotGrid ID="grid" Width="1200px" runat="server" />
            </span>

        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
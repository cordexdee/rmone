<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketsByStage.ascx.cs" Inherits="uGovernIT.Web.TicketsByStage" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxbButton_BlackGlass {
        background-image: none !important;
        border: none;
    }

    .svcDashboard_TrendChartBtn {
    background: #4A6EE2;
    color: #FFFFFF;
    font-weight: 500;
    font-family: Poppins;
    font-size: 12px;
    text-align: left;
    width: auto;
    /*height: 38px;*/
    /*margin-left: 8px;*/
    padding: 1px 0px 3px;
}
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function OnbtnEmailAlertClick(s, e) {
        grid.GetSelectedFieldValues('TicketId', OnGetSelectedFieldValuesForEmailAlert);
    }

    function OnGetSelectedFieldValuesForEmailAlert(selectedValues) {
        if (selectedValues.length < 1) {
            alert("Please select at least one item.");
            return;
        }
        var param = "ids=" + selectedValues + "&EmailAlert=True&StageTitle=" + '<%= StageTitle %>' + "&ModuleName=" + '<%= ModuleName%>';
        window.parent.UgitOpenPopupDialog('<%= TicketAlertUrl %>', param, 'Ticket Alert', '600px', '500px', 0, escape("<%= Request.Url.AbsolutePath %>"));

        return false;
    }

    function OnbtnChartTrendClick(s, e) {
        var param = "StageTitle=" + '<%= StageTitle %>' + "&ModuleName=" + '<%= ModuleName%>' + "&StageStep=" + '<%= StageStep %>';
        window.parent.UgitOpenPopupDialog('<%= StageTrendChartUrl %>', param, 'Trend Chart', '820px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function CheckHeaderCheckBox(s, e) {
        if (s.checked)
            grid.SelectAllRowsOnPage();
        else
            grid.UnselectAllRowsOnPage();
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        var ticketid = params.split('=')[1];
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }
</script>

<div style="float: left; width: 100%;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
        <tr>
            <td colspan="3" style="text-align: right; padding: 5px 5px 5px 0px;">
                <div>
                    <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
                        OnDataBinding="grid_DataBinding" OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
                        OnHtmlRowPrepared="grid_HtmlRowPrepared" OnHeaderFilterFillItems="grid_HeaderFilterFillItems" Theme="Material"
                        ClientInstanceName="grid" Width="99%" KeyFieldName="TicketId">
                        <Columns>
                        </Columns>
                        <SettingsPopup>
                            <HeaderFilter Height="200" />
                        </SettingsPopup>
                        <SettingsPager Position="TopAndBottom" AlwaysShowPager="true" Mode="ShowPager" PageSize="25">
                            <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100, 200, 500, 1000" />
                        </SettingsPager>
                    </dx:ASPxGridView>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <dx:ASPxButton ID="btnEmailAlert" runat="server" ImagePosition="Right" Visible="true" Text="Alert" Height="25px" AutoPostBack="false" CssClass="btn btn-sm  svcDashboard_TrendChartBtn btnEmailAlert">
                    <Image Url="/layouts/15/images/uGovernIT/MailTo16X16.png"></Image>
                    <ClientSideEvents Click="OnbtnEmailAlertClick" />
                </dx:ASPxButton>

                <dx:ASPxButton ID="btnChartTrend" runat="server" ImagePosition="Right" Visible="true" Text="Trend Chart" Height="25px" AutoPostBack="false" CssClass="btn btn-sm  svcDashboard_TrendChartBtn btnChartTrend">
                    <Image Url="/_layouts/15/images/uGovernIT/chart-icon.png"></Image>
                    <ClientSideEvents Click="OnbtnChartTrendClick" />
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
</div>
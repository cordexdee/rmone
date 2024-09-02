
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DailyTicketCountTrendsControl.ascx.cs" Inherits="uGovernIT.Web.DailyTicketCountTrendsControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .calenderweektxt {
        font-weight: bold;
        padding-left: 5px;
    }

    .calendernextweekbt {
        padding-left: 5px;
    }

    .calendertxtbox {
        /*margin-right: 46px;*/
        visibility: hidden;
    }

    .clsCalendar .ms-dtinput input:first-child {
        width: 0px;
    }
   
    .clsrowResolved:nth-child(6) td {
        border-top: 0.5px solid darkgray !important;
    }

    .clsrowNoResolved:nth-child(5) td {
        border-top: 0.5px solid darkgray !important;
    }

    .dxpcLite_UGITNavyBlueDevEx {
        top: 0 !important;
    }
    .calpopup-header .dxpc-headerContent {
        padding-right: 0 !important;
    }
      /*.clsgrddailyticketcount .dxgvRBB > tbody > tr {
        border-bottom: none;
        border-top: none;
    }*/
</style>

<script id="dxss_TicketCountTrendsUserControl" type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var actiontaken = '';
    function doPreviousClick() {
        actiontaken = 'p';
        pnlactionbutton.PerformCallback("p|nextpreviouscall");
    }

    function doNextClick() {
        actiontaken = 'n';
        pnlactionbutton.PerformCallback("n|nextpreviouscall");
    }

    function OnEndCallBack(s, e) {
        if (actiontaken != '') {
            if (actiontaken == 'p')
                pnlcallCountTrends.PerformCallback("p");
            else
                pnlcallCountTrends.PerformCallback("n");
        }
    }

    function DailyTicketCountDrillDown(module, datecol, view, mode, istotaldrilldown, dateRStart, dateREnd) {
        var paramsvpm = '&module=' + module + '&datecol=' + escape(datecol) + '&view=' + view + '&mode=' + mode + '&istotaldrilldown=' + istotaldrilldown + '&dateRStart=' + dateRStart + '&dateREnd=' + dateREnd;
        window.parent.UgitOpenPopupDialog('<%= dailyTicketCountTrends %>', paramsvpm, mode, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function setFormSubmitToFalse() {
        setTimeout(function () { _spFormOnSubmitCalled = false; }, 3000);
        return true;
    }

    function doaction(mode) {
        dxvcalpanel.PerformCallback(mode);
        return false;
    }

    function onCustomDisabledDate(s, e) {
        var today = new Date();
        if (e.date > today)
            e.isDisabled = true;
    }

    function CalendarAction(s, e) {
        calPopup.Hide();
        dxvcalpanel.PerformCallback('date');
    }

    function resetScroll(s, e) {
        var scrollFieldName = s.name + "_ScrollOffset";
        if (hf.Contains(scrollFieldName)) {
            var offset = hf.Get(scrollFieldName);
            hf.Remove(scrollFieldName);
            setTimeout(function() { s.SetHorzScrollPos(offset); }, 0);
        }
    }

</script>

<dx:ASPxCallbackPanel ID="dxvcalpanel" runat="server" ClientInstanceName="dxvcalpanel" OnCallback="pnlcallCountTrends_Callback">
    <PanelCollection>
        <dx:PanelContent>
              <dx:ASPxHiddenField ID="hf" runat="server" ClientInstanceName="hf">
        </dx:ASPxHiddenField>
            <div style="float: left; width: 100%; padding: 5px 0 20px 10px;">
                <div style="float: left; width: 100%;">
                    <div style="float: left;">
                        <dx:ASPxComboBox ID="ddlModule" runat="server" Caption="Module" Width="230px" AutoPostBack="false" ClientInstanceName="ddlModule">
                            <CaptionStyle Font-Bold="true"></CaptionStyle>
                            <ClientSideEvents ValueChanged="function(s,e){dxvcalpanel.PerformCallback();}" />
                        </dx:ASPxComboBox>
                    </div>
                    <div style="float: left; padding-left: 40px; padding-right: 40px;">
                        <dx:ASPxComboBox ID="ddlview" runat="server" Caption="View" Width="70px" AutoPostBack="false" ClientInstanceName="ddlview">
                            <CaptionStyle Font-Bold="true"></CaptionStyle>
                            <Items>
                                <dx:ListEditItem Text="Daily" Value="daily" Selected="true" />
                                <dx:ListEditItem Text="Weekly" Value="weekly" />
                                <dx:ListEditItem Text="Monthly" Value="monthly" />
                            </Items>
                            <ClientSideEvents ValueChanged="function(s,e){hdnkeepdaterange.Set('range','');dxvcalpanel.PerformCallback();}" />
                        </dx:ASPxComboBox>
                    </div>
                    <div style="width: auto; float: left; padding-top:5px;">
                        <div style="text-align: center; display: inline-flex;">
                            <span class="calenderpreweekbt">
                                <asp:ImageButton ImageUrl="/Content/Images/Previous16x16.png" ID="previousWeek" OnClientClick="return doaction('p')" runat="server" />
                            </span>
                            <span style="padding-top:1px;">
                                <dx:ASPxLabel CssClass="calenderweektxt" ID="lbWeekDuration" ClientInstanceName="lbWeekDuration" runat="server"></dx:ASPxLabel>
                            </span>
                            <span class="calendernextweekbt">
                                <asp:ImageButton OnClientClick="return doaction('n')" ImageUrl="/Content/Images/Next16x16.png" ID="nextWeek" runat="server" />
                            </span>
                            <span style="float: right; padding-left: 30px;">
                                <dx:ASPxPopupControl ID="calPopup" runat="server" ClientInstanceName="calPopup" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="calpopup-header" HeaderText="Select Date" PopupVerticalAlign="TopSides">
                                    <ContentCollection>
                                        <dx:PopupControlContentControl>
                                            <asp:Panel ID="panelCal" runat="server">
                                                <dx:ASPxCalendar ID="dtcStartdate" ToolTip="Calendar" runat="server" AutoPostBack="false">
                                                    <ClientSideEvents SelectionChanged="function(s,e){CalendarAction(s,e);}" CustomDisabledDate="onCustomDisabledDate" />
                                                    <ButtonStyle CssClass="CalendarBtn"></ButtonStyle>
                                                </dx:ASPxCalendar>
                                            </asp:Panel>
                                        </dx:PopupControlContentControl>
                                    </ContentCollection>
                                </dx:ASPxPopupControl>
                                <dx:ASPxImage ID="btnPcKalenderOk" ClientInstanceName="btnPcKalenderOk" Width="16" runat="server" ImageUrl="/Content/Images/calendar.png">
                                    <ClientSideEvents Click="function(s,e){calPopup.Show();}" />
                                </dx:ASPxImage>
                            </span>
                            <asp:HiddenField ID="startWeekDateForEdit" runat="server" />
                        </div>
                    </div>
                    <div style="float: left; padding-left: 20px;">
                        <span class="fleft" style="padding: 1px 3px;">
                            <dx:ASPxButton RenderMode="Link" ID="btnExportExcel" ToolTip="Excel" OnClick="btnExportExcel_Click" runat="server">
                                <ClientSideEvents Click="setFormSubmitToFalse" />
                                <Image Url="/Content/images/excel-icon.png"></Image>
                            </dx:ASPxButton>
                            <dx:ASPxButton RenderMode="Link" ID="btnExportPdf" ToolTip="Pdf" OnClick="Unnamed_Click" runat="server" style="margin-left:2px;">
                                <ClientSideEvents Click="setFormSubmitToFalse" />
                                <Image Url="/Content/images/pdf-icon.png"></Image>
                            </dx:ASPxButton>
                        </span>
                    </div>
                </div>
                <div>
                </div>
                <div style="float: left; width: auto; padding-top: 10px;">

                    <div style="clear: both;" />
                    <div id="grdDiv" runat="server" style ="border-style:solid;border-color:lightgray;border-width: 1px;">
                        <dx:ASPxHiddenField ID="hdnkeepdaterange" runat="server" ClientInstanceName="hdnkeepdaterange"></dx:ASPxHiddenField>
                        <ugit:ASPxGridView ID="grdCountTrends" SettingsDetail-ExportMode="All" runat="server" ClientInstanceName="grdCountTrends" 
                            AutoGenerateColumns="false" OnHtmlDataCellPrepared="grdCountTrends_HtmlDataCellPrepared" 
                            OnDataBinding="grdCountTrends_DataBinding" CssClass="clsgrddailyticketcount">
                            <Styles>
                                <Header Font-Bold="true" HorizontalAlign="Center">
                                    <Paddings Padding="4" />
                                </Header>
                                <Row CssClass="clsrowResolved"></Row>
                            </Styles>
                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            <Settings HorizontalScrollBarMode="Auto" />
                            <Columns>
                            </Columns>
                            <Settings GridLines="None" />
                            <ClientSideEvents Init="function(s,e){resetScroll(s,e);}" />
                        </ugit:ASPxGridView>
                        <dx:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="grdCountTrends" Landscape="true" OnRenderBrick="exporter_RenderBrick">
                            <Styles Header-Wrap="True" Cell-Wrap="True"></Styles>
                        </dx:ASPxGridViewExporter>
                    </div>
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
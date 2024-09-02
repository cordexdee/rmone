<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITSMCtrl.ascx.cs" Inherits="uGovernIT.Web.ITSMCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        var bottleneckUrl = "<%=BottleNeckUrlUrl %>";

        //$('.btn').removeClass('slectedBtn');
        $('#dvBottleNeckChart').attr("src", bottleneckUrl+ '&moduleName=' + $('.slectedBtn').attr('id') + '&moduletitle=""')

        $(".btn").click(function () {        
            $('#dvBottleNeckChart').attr("src", bottleneckUrl+ '&moduleName=' + $(this).attr('id') + '&moduletitle=""')

            ImpactCallbackPanel.PerformCallback($(this).attr('id'))
            SLAPerformanceCallbackPanel.PerformCallback($(this).attr('id'));
            $('.btn').removeClass('slectedBtn');
            $(this).toggleClass("slectedBtn");
        });

        $(".DashboardDesignerSetting").style.height = '<%= Height%>';
        $(".DashboardDesignerSetting").style.height = '<%= Width%>';
    });

     function resizeBottleNeckIframe(obj) {        
         obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';
     }
</script>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.divcell_0 {
        /*background-image: url('/Content/images//pm_gridtopcorner.png');
        height: 32px;
        background-color: #3E4F50;
        color: #F2F2F2;
        width: 107px;
    }

    .cell_0_0 {
        background-color: #3E4F50;
        padding: 0px;
        height: 32px;
        /*width: 107px;*/
    }

    /*.header {
        padding: 0px 9px;
        font-weight: bold;
        text-align: center;
        background-color: #F2F2F2;
        color: #3E4F50;
    }*/

     .tablecss {
        width: 50%;
    }

    .tablecss td,
    .tablecss th {
        border: 1px solid;
        height: 25px;
    }

    .tablecss th,
    .tablecss td {
        padding: 3px;
    }

    .itsmHomeGrid .homeGrid_headerColumn, .tablecss.ITSMLanding-pageGrids .titleclass {
        color: #000;
    }
    .itsmHomeGrid .homeGrid_headerColumn .higt {
        height: 53px;
    }
    .itsmHomeGrid {
        border-radius: 4px;
    }
    .itsmHomeGrid, .itsmHomeGrid > tbody > tr > td, .itsmHomeGrid > tbody > tr > th, .tablecss.ITSMLanding-pageGrids, .tablecss.ITSMLanding-pageGrids > tbody > tr > td, .tablecss.ITSMLanding-pageGrids > tbody > tr > th {
        border: 1px solid #999;
    }

</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function openDrillDownImpacts(mname, impactID, severityID) {
        
        var selectedBtn = $("button.slectedBtn")[0].innerText;
        
        var title = selectedBtn;
        window.parent.UgitOpenPopupDialog("/Layouts/uGovernIT/DelegateControl.aspx?control=ImpactListDrillDown&ModuleName=" + mname + "&ImpactID=" + impactID + "&SeverityID=" + severityID, "", title, 90, 90, 0, '');
    }
</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
<div class="col-md-12 col-sm-12 col-xs-12 DashboardDesignerSetting" style="background:#FFF;">
  <div class="row itsmCtrl-buttonlist">
    <button type="button" class="btn btn-md ITSMButton-secondary slectedBtn" id="TSR">Incidents</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="SVC">Services</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="ACR">Application Changes</button>
    <button type="button" class="btn btn-md ITSMButton-secondary" id="DRQ">Change Management</button>
  </div>


  <div class="row">
    <div class="col-sm-12 noPadding bottleneckChart-containerWrap">   
       <iframe id="dvBottleNeckChart" width="100%" onload="resizeBottleNeckIframe(this)" scrolling="no"></iframe>
    </div>    
  </div>
  <div class="row itsmLanding-gridContainer">
    <div class="col-sm-6 fixed-paddingRight">
        <dx:ASPxCallbackPanel ID="ImpactCallbackPanel" ClientInstanceName="ImpactCallbackPanel" runat="server" OnCallback="Impact_Callback">
            <PanelCollection>
                <dx:PanelContent ID="impactPanel" runat="server">
                    <asp:GridView ID="gridPriority" runat="server" EnableModelValidation="True" Width="100%" GridLines="Both"
                         OnRowDataBound="gridPriority_RowDataBound" CellPadding="4" CssClass="itsmHomeGrid">
                        <HeaderStyle BackColor="#F2F2F2" Font-Bold="True"  HorizontalAlign="Center" CssClass="homeGrid_headerColumn itsmGrid-headerCol" />    
                        <RowStyle BackColor="#FFFFFF" HorizontalAlign="Center" Height="38px" CssClass="itsmGrid_dataRow" />
                    </asp:GridView>
                </dx:PanelContent>
            </PanelCollection>
            <SettingsLoadingPanel Enabled="false" />
            <ClientSideEvents BeginCallback="function(s, e){ LoadingPanel.Show(); }" EndCallback="function(s, e){ LoadingPanel.Hide(); }" />
        </dx:ASPxCallbackPanel>
    </div>
    <div class="col-sm-6 fixed-paddingLeft">
        <dx:ASPxCallbackPanel ID="SLAPerformanceCallbackPanel" ClientInstanceName="SLAPerformanceCallbackPanel" runat="server" OnCallback="SlaPerformance_Callback">
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <table id="tblmain" class="tablecss ITSMLanding-pageGrids" cellspacing="0" cellpadding="0">
                            <asp:Repeater ID="rptSLAParent" runat="server" OnItemDataBound="rptSLAParent_ItemDataBound">
                                <HeaderTemplate>
                                    <tr class="titleclass">
                                        <th scope="col">Activity</th>
                                        <th scope="col">Target (Bus&nbsp;<%=HeaderText %>)</th>
                                        <th>Actual (Bus&nbsp;<%=HeaderText %>) </th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="tbleContent">
                                        <td style="text-align: center;">
                                            <asp:Label ID="lblRuleName" runat="server" Text='<%# Eval("Title") %>' /></td>
                                        <td style="text-align: center; background-color: #80FFD1;">
                                            <asp:Label ID="lblSLATargetX2" runat="server" Text='<%# Eval("SLATargetX2") %>' /></td>
                                        <td style="text-align: center; background-color: #D9F2FF;">
                                            <asp:Label ID="lblSLAActualX2" runat="server" Text='<%# Eval("SLAActualX2") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr class="summary-row">
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblRuleName" runat="server" Text='TOTAL:' /></td>
                                        <td style="text-align: center; background-color: #80FFD1;">
                                            <asp:Label ID="lblSLATargetX2Total" runat="server" Text="-" /></td>
                                        <td style="text-align: center; background-color: #D9F2FF;">
                                            <asp:Label ID="lblSLAActualX2Total" runat="server" Text="-" /></td>
                                    </tr>

                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                </dx:PanelContent>
            </PanelCollection>
            <SettingsLoadingPanel Enabled="false" />
            <ClientSideEvents BeginCallback="function(s, e){ LoadingPanel.Show(); }" EndCallback="function(s, e){ LoadingPanel.Hide(); }" />
        </dx:ASPxCallbackPanel>
    </div>    
  </div>
</div>


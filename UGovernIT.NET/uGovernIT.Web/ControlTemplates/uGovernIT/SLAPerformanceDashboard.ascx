<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SLAPerformanceDashboard.ascx.cs" Inherits="uGovernIT.Web.SLAPerformanceDashboard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1.Web, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"  Namespace="DevExpress.XtraCharts" TagPrefix="dxcharts" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .tablecss {
        width: 100%;
    }

        .tablecss td,
        .tablecss th {
            border: 1px solid;
            height: 25px;
        }

    .dropdown {
        width: 235px;
        margin-bottom: 9px;
    }

    .module {
        float: left;
        padding-right: 3px;
    }

    .tablecss th,
    .tablecss td {
        padding: 3px;
    }

    .ddlperiod {
        height: 24px;
        /*position: absolute;*/
        right: 11px;
        top: 9px;
        width: 100%;
    }

    .summary-row td {
        font-weight: bold;
    }

    .clsrptlengend {
        vertical-align: top;
        padding-left: 11px;
        /*display: none;*/
    }

    .dateSectionCreatedOn {
        float: left;
        padding-top: 2px;
    }

    .dateSectionCompletedOn {
        float: right;
        padding-top: 2px;
    }

    .clsincludeopentickets {
        float: right;
        position: absolute;
        bottom: -32px;
        right: 9px;
    }

     .clsfooter {
    padding-top:30px;
    }
</style>

<script id="dxss_SLAPerformanceDashboard" type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    try {
        var controlLength = customUGITSLADConfig.length;
    }
    catch (ex) {
        customUGITSLADConfig = {}
    }
    customUGITSLADConfig["<%= this.ClientID%>" + '_slaperfomancecallbackpanel_ganttChart'] = { "module": "<%=this.Module%>", "datefilter": escape('<%=filterExp%>'), "IncludeOpen": escape('<%=chkslaperformanceOpen.Checked%>') };


    // Keep client id 

    if (typeof (slaPerformanceArray) == "undefined")
        var slaPerformanceArray = [];
    slaPerformanceArray.push({ clienid: "<%=this.ClientID%>" })

    function slaPerformanceArray_Ctrl(globalName, dxCtrlID) {
        var item = null;
        $.each(slaPerformanceArray, function (s, obj) {
            if (globalName.indexOf(obj.clienid) != -1) {
                item = obj;
                return;
            }
        });

        if (!item)
            return null;
        
        if (dxCtrlID.endsWith("chkslaperformanceOpen"))
            return ASPxClientControl.GetControlCollection().GetByName(item.clienid + "_" + dxCtrlID);   
        else
            return window[item.clienid + "_" + dxCtrlID];
    }

    function GetSummary(s, e) {
        if (e.additionalHitObject != null) {
            var config = customUGITSLADConfig[s.name];
            var title = e.additionalHitObject.argument;
            var createdDropdownInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_ddlCreatedOn');
            var completedDropdownInstamce = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_ddlCompletedOn');
            var chkIncludeInstance = slaPerformanceArray_Ctrl(s.name, "slaperfomancecallbackpanel_chkslaperformanceOpen");
            var createdon = '';
            if (createdDropdownInstance != null && createdDropdownInstance != undefined)
                createdon = createdDropdownInstance.GetValue() == null ? '' : createdDropdownInstance.GetValue();
            var completedon = '';
            if (completedDropdownInstamce != null && completedDropdownInstamce != undefined)
                completedon = completedDropdownInstamce.GetValue() == null ? '' : completedDropdownInstamce.GetValue()

            var customFilterDate = '';
            if (createdon == 'Custom') {
                var customSDate = slaPerformanceArray_Ctrl(s.name, "slaperfomancecallbackpanel_lbldatecStartDate");
                var customEDate = slaPerformanceArray_Ctrl(s.name, "slaperfomancecallbackpanel_lbldatecEndDate");
                var sdate = '';
                var edate = '';
                if (customSDate != null && customSDate != undefined)
                    sdate = customSDate.GetValue() == '' ? '<%=DateTime.Today.ToString("yyyy-MM-dd")%>' : customSDate.GetValue();
                if (customEDate != null && customEDate != undefined) {
                     edate = customEDate.GetValue() == '' ? '<%=DateTime.Today.ToString("yyyy-MM-dd")%>' : customEDate.GetValue();
                }

                customFilterDate = sdate + '!#' + edate;

            }

            if (completedon == 'Custom') {
                var customComSDate = slaPerformanceArray_Ctrl(s.name, "slaperfomancecallbackpanel_lbldatecomStartDate");
                var customComEDate = slaPerformanceArray_Ctrl(s.name, "slaperfomancecallbackpanel_lbldatecomEndDate");
                var sdate = '';
                var edate = '';

                if (customComSDate != null && customComSDate != undefined)
                    sdate = customComSDate.GetValue() == '' ? '<%=DateTime.Today.ToString("yyyy-MM-dd")%>' : customComSDate.GetValue();
                if (customComEDate != null && customComEDate != undefined) {
                    edate = customComEDate.GetValue() == '' ? '<%=DateTime.Today.ToString("yyyy-MM-dd")%>' : customComEDate.GetValue();
                }

                if (customFilterDate != undefined && customFilterDate != '')
                    customFilterDate = customFilterDate + '~#' + sdate + '!#' + edate;
                else
                    customFilterDate = sdate + '!#' + edate;
            }

            config.datefilter = escape(createdon + '~#' + completedon);
            if (title != "") {
                params = "Module=" + config.module + "&Title=" + escape(title) + "&datefilter=" + config.datefilter + "&IncludeOpen=" + chkIncludeInstance.GetChecked();
                if (createdon == 'Custom' || completedon == 'Custom')
                    params = params + '&CustomDateFilter=' +escape(customFilterDate);

                window.parent.UgitOpenPopupDialog('<%= drilDownData %>', params, title, '90', '80', 0, escape("<%= Request.Url.AbsolutePath %>"));
            }
        }
    }
    function CustomFilterChecked(s, e, ctrId) {     
        var val = s.GetValue();
        var callbackInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel');
        var createdPopupInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_customfilterPopup');
        var completedPopupInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_CompletedcustomfilterPopup');
        createdPopupInstance.SetPopupElementID($('#' + callbackInstance.name + "_spnslaperCreatedOn").attr('id'));
        completedPopupInstance.SetPopupElementID($('#' + callbackInstance.name + "_spnslaperCompletedOn").attr('id'));
        createdPopupInstance.Hide();
        completedPopupInstance.Hide();
        e.processOnServer = false;
        var executableBlock = '';
        if (ctrId == 'ddlCreatedOn')
            executableBlock = 'slaperformanceCreatedOne';
        if (ctrId == 'ddlCompletedOn')
            executableBlock = 'slaperformanceCompletedOne';
        if (val == 'Custom' && s.name.endsWith('ddlCreatedOn')) {

            createdPopupInstance.Show();
            completedPopupInstance.Hide();

        }
        else if (val == 'Custom' && s.name.endsWith('ddlCompletedOn')) {
            createdPopupInstance.Hide();
            completedPopupInstance.Show();
        }
        else {
            createdPopupInstance.Hide();
            completedPopupInstance.Hide();
            callbackInstance.PerformCallback(executableBlock);
        }
    }

    function ShowDateSection(s, e, respectiveControlId) {
        ShowPopup(s, e, '');
        var callbackInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel');
        if (callbackInstance != null && callbackInstance != undefined && !callbackInstance.InCallback())
            callbackInstance.PerformCallback(respectiveControlId);
    }

    $(function () {
        debugger;
        var callbackInstance = slaPerformanceArray_Ctrl("<%=this.ClientID%>", 'slaperfomancecallbackpanel');
        var ganttChartInstance = slaPerformanceArray_Ctrl("<%=this.ClientID%>", 'slaperfomancecallbackpanel_ganttChart');
        var ganttChartImgInstance = slaPerformanceArray_Ctrl("<%=this.ClientID%>", 'slaperfomancecallbackpanel_ganttChart_IMG');//_ganttChart_IMG
        var dateSecCreatedOn = $('#' + callbackInstance.name + '_divcreateddateblock');
        var dateSecCompletedOn = $('#' + callbackInstance.name + '_divcompleteddateblock');
        var createdDropdownInstance = slaPerformanceArray_Ctrl("<%=this.ClientID%>", 'slaperfomancecallbackpanel_ddlCreatedOn');
        var completedDropdownInstamce = slaPerformanceArray_Ctrl("<%=this.ClientID%>", 'slaperfomancecallbackpanel_ddlCompletedOn'); 
        var chkIncludeInstance = slaPerformanceArray_Ctrl("<%=this.ClientID%>", "slaperfomancecallbackpanel_chkslaperformanceOpen");
        var completedHideBlock = $('#' + callbackInstance.name + "_divHidecompletedone");
        if (dateSecCreatedOn)
            $(dateSecCreatedOn).hide();
        if (dateSecCompletedOn)
            $(dateSecCompletedOn).hide();

        if (createdDropdownInstance.GetValue() == 'Custom' && dateSecCreatedOn) {
            $(dateSecCreatedOn).show();
        }
        if (completedDropdownInstamce.GetValue() == 'Custom' && dateSecCompletedOn) {
            $(dateSecCompletedOn).show();
        }

        if (chkIncludeInstance.GetChecked()) {
            $(completedHideBlock).hide();
        }

    });
    function ShowPopup(s, e, param) {
        var createdPopupInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_customfilterPopup');
        var completedPopupInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_CompletedcustomfilterPopup');
        createdPopupInstance.Hide();
        completedPopupInstance.Hide();
        if (param == 'con')//Created on
            createdPopupInstance.Show();
        else if (param == 'comon')//Completed on
            completedPopupInstance.Show();
    }

    function ResizeChartHeight(s, e) {
        debugger;
        var callbackInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel');
        var ganttChartInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_ganttChart');
        var createdPopupInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_customfilterPopup');
        var completedPopupInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_CompletedcustomfilterPopup');
        var chkIncludeInstance = slaPerformanceArray_Ctrl(callbackInstance.name, "slaperfomancecallbackpanel_chkslaperformanceOpen");
        var completedHideBlock = $('#' + callbackInstance.name + "_divHidecompletedone");
        if (chkIncludeInstance.GetChecked()) {
            $(completedHideBlock).hide();
        }
        else
            $(completedHideBlock).show();

        createdPopupInstance.Hide();
        completedPopupInstance.Hide();
        var dateSecCreatedOn = $('#' + callbackInstance.name + '_divcreateddateblock');
        var dateSecCompletedOn = $('#' + callbackInstance.name + '_divcompleteddateblock');
        if (dateSecCreatedOn)
            $(dateSecCreatedOn).hide();
        if (dateSecCompletedOn)
            $(dateSecCompletedOn).hide();

        var createdDropdownInstance = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_ddlCreatedOn');
        var completedDropdownInstamce = slaPerformanceArray_Ctrl(s.name, 'slaperfomancecallbackpanel_ddlCompletedOn');

        if (createdDropdownInstance.GetValue() == 'Custom' && dateSecCreatedOn) {
            $(dateSecCreatedOn).show();
        }
        if (completedDropdownInstamce.GetValue() == 'Custom' && dateSecCompletedOn) {
            $(dateSecCompletedOn).show();
        }

    }

    function CheckBoxAction(s, e) {
        var callBackObj = slaPerformanceArray_Ctrl(s.name, "slaperfomancecallbackpanel");
        if (callBackObj != null && callBackObj != undefined && !callBackObj.InCallback())
            callBackObj.PerformCallback();        
    }
</script>

<dx:ASPxCallbackPanel ID="slaperfomancecallbackpanel" runat="server" OnCallback="slaperfomancecallbackpanel_Callback" EnableClientSideAPI="true">
    <ClientSideEvents EndCallback="function(s,e){ResizeChartHeight(s,e);}" />
    <PanelCollection>
        <dx:PanelContent ID="slaperfomancepnl" runat="server">
            <dx:ASPxHiddenField ID="hdnCustomFilterValues" runat="server" ClientInstanceName="hdnCustomFilterValues"></dx:ASPxHiddenField>
            <dx:ASPxPopupControl ID="CompletedcustomfilterPopup" runat="server" Modal="true" PopupHorizontalAlign="LeftSides" HeaderText="Custom Filter" PopupVerticalAlign="Below" PopupElementID="spnslaperCompletedOn" CloseAction="CloseButton" EnableClientSideAPI="true">
                <ClientSideEvents PopUp="function(s,e){ASPxClientEdit.ClearGroup('grpClear');}" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <dx:ASPxPanel ID="ASPxPanel1" runat="server" DefaultButton="btnCOk">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <div style="float: left;">
                                        <div style="float: right;">
                                            <dx:ASPxDateEdit ID="cStartDate" runat="server" ClientInstanceName="cStartDate" Caption="Start Date" ValidationSettings-ValidationGroup="grpClear">
                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                            </dx:ASPxDateEdit>
                                        </div>
                                        <div style="float: right; padding-top: 5px;">
                                            <dx:ASPxDateEdit ID="cEndDate" runat="server" ClientInstanceName="cEndDate" Caption="End Date" Width="170px" ValidationSettings-ValidationGroup="grpClear">
                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div style="float: left; padding-top: 5px; width: 100%">
                                        <div style="float: right;">
                                            <dx:ASPxButton ID="btnCOk" runat="server" Text="Apply" CssClass="primary-blueBtn" AutoPostBack="false" CommandName="slaperformanceCompletedOne">
                                                <ClientSideEvents Click="function(s,e){ShowDateSection(s,e,'slaperformanceCompletedOne');}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="customfilterPopup" runat="server" Modal="true" PopupHorizontalAlign="LeftSides" HeaderText="Custom Filter" PopupVerticalAlign="Below" PopupElementID="spnslaperCreatedOn" CloseAction="CloseButton" EnableClientSideAPI="true">
                <ClientSideEvents PopUp="function(s,e){ASPxClientEdit.ClearGroup('grpClear');}" />
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxPanel ID="pnlfilter" runat="server" DefaultButton="btnOk">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <div style="float: left;">
                                        <div style="float: right;">
                                            <dx:ASPxDateEdit ID="dtStartdate" runat="server" ClientInstanceName="dtStartdate" Caption="Start Date" ValidationSettings-ValidationGroup="grpClear">
                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                            </dx:ASPxDateEdit>
                                        </div>
                                        <div style="float: right; padding-top: 5px;">
                                            <dx:ASPxDateEdit ID="dtEndDate" runat="server" ClientInstanceName="dtEndDate" Caption="End Date" Width="170px" ValidationSettings-ValidationGroup="grpClear">
                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                            </dx:ASPxDateEdit>
                                        </div>
                                    </div>
                                    <div style="float: left; width: 100%; padding-top: 5px;">
                                        <div style="float: right;">
                                            <dx:ASPxButton ID="btnOk" runat="server" Text="Apply" CssClass="primary-blueBtn" AutoPostBack="false" CommandName="slaperformanceCreatedOne">
                                                <ClientSideEvents Click="function(s,e){ShowDateSection(s,e,'slaperformanceCreatedOne');}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <table style="width: 100%; margin-left:6px;">
                <tr>
                    <td id="tdchart" style="vertical-align: top; border: 1px solid;" runat="server">
                        <div style="position: relative; float: left; width: 100%;">
                            <div class="ddlperiod">
                                <div style="float: left; padding: 5px 0px 5px 10px;">
                                    <div style="float: left; word-wrap: break-word; width: 92px;">
                                        <span style="font-weight: normal; vertical-align: middle;"><b>Created On:</b></span>
                                    </div>
                                    <div style="float: left; padding-left: 5px;">
                                        <div style="float: left;">
                                            <dx:ASPxComboBox ID="ddlCreatedOn" runat="server" OnValueChanged="ddlCreatedOn_ValueChanged" EnableClientSideAPI="true" Width="120px" AutoPostBack="true">
                                                <ClientSideEvents ValueChanged="function(s,e){CustomFilterChecked(s,e,'ddlCreatedOn');}" />
                                            </dx:ASPxComboBox>
                                            <span id="spnslaperCreatedOn" runat="server"></span>
                                        </div>
                                        <div id="divcreateddateblock" runat="server" class="dateSectionCreatedOn" style="display: none;">
                                            <div style="float: left; padding-left: 4px;">
                                                <dx:ASPxLabel ID="lbldatecStartDate" EnableClientSideAPI="true" runat="server"></dx:ASPxLabel>
                                            </div>
                                            <div style="float: left; padding-left: 2px; padding-right: 2px;"><span><b>to</b></span></div>
                                            <div style="float: left; padding-left: 2px;">
                                                <dx:ASPxLabel ID="lbldatecEndDate" runat="server" EnableClientSideAPI="true"></dx:ASPxLabel>
                                            </div>
                                            <div style="float: left; padding-left: 4px;">
                                                <dx:ASPxImage ID="imgCreatedon" runat="server" ImageUrl="/Content/images/edit-icon.png">
                                                    <ClientSideEvents Click="function(s,e){ ShowPopup(s,e,'con');}" />
                                                </dx:ASPxImage>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="clear: both;"></div>
                                <div id="divHidecompletedone" runat="server" style="float: left; padding: 5px 0px 5px 10px;">
                                    <div style="float: left; word-wrap: break-word; width: 92px;">
                                        <span style="font-weight: normal; vertical-align: middle;"><b>Completed On:</b></span>
                                    </div>
                                    <div style="float: left; padding-left: 5px;">
                                        <div style="float: left;">
                                            <dx:ASPxComboBox ID="ddlCompletedOn" OnValueChanged="ddlCreatedOn_ValueChanged" runat="server" Width="120px" EnableClientSideAPI="true" AutoPostBack="true">
                                                <ClientSideEvents ValueChanged="function(s,e){ CustomFilterChecked(s,e,'ddlCompletedOn');}" />
                                            </dx:ASPxComboBox>
                                            <span id="spnslaperCompletedOn" runat="server"></span>

                                        </div>
                                        <div id="divcompleteddateblock" runat="server" class="dateSectionCompletedOn" style="display: none;">
                                            <div style="float: left; padding-left: 4px;">
                                                <dx:ASPxLabel ID="lbldatecomStartDate" EnableViewState="true" runat="server" EnableClientSideAPI="true"></dx:ASPxLabel>
                                            </div>
                                            <div style="float: left; padding-left: 2px; padding-right: 2px;"><span><b>to</b></span></div>
                                            <div style="float: left; padding-left: 2px;">
                                                <dx:ASPxLabel ID="lbldatecomEndDate" EnableViewState="true" runat="server" EnableClientSideAPI="true"></dx:ASPxLabel>
                                            </div>
                                            <div style="float: left; padding-left: 4px;">
                                                <dx:ASPxImage ID="imgCompleatedOn" runat="server" ImageUrl="/Content/images/edit-icon.png">
                                                    <ClientSideEvents Click="function(s,e){ ShowPopup(s,e,'comon');}" />
                                                </dx:ASPxImage>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clsincludeopentickets">
                                    <dx:ASPxCheckBox ID="chkslaperformanceOpen" runat="server" AutoPostBack="false" OnValueChanged="chkslaperformanceOpen_ValueChanged" EnableClientSideAPI="true" Text="Include Open Tickets">   <%--OnValueChanged="chkslaperformanceOpen_ValueChanged"--%>
                                        <ClientSideEvents ValueChanged="function(s,e){CheckBoxAction(s,e);}" />                                        
                                    </dx:ASPxCheckBox>
                                </div>
                            </div>
                        </div>
                        <div id="divchartcontainer" runat="server">
                            <dxchartsui:WebChartControl ID="ganttChart" OnCustomDrawSeries="ganttChart_CustomDrawSeries" ClientSideEvents-ObjectSelected="function(s,e){GetSummary(s,e);}" PaletteBaseColorNumber="1" runat="server" CrosshairEnabled="True" EnableClientSideAPI="true" AppearanceNameSerializable="Default" AutoLayout="True">
                                <%--<ClientSideEvents   ObjectHotTracked="function(s,e){s.SetHeight(s.GetHeight() + 100)}" />--%>
                                <BorderOptions Visibility="False" />
                            </dxchartsui:WebChartControl>
                        </div>
                    </td>
                    <td runat="server" id="tdSLAParent" class="clsrptlengend">
                        <table id="tblmain" class="tablecss" cellspacing="0" cellpadding="0">
                            <asp:Repeater ID="rptSLAParent" runat="server" OnItemDataBound="rptSLAParent_ItemDataBound" OnDataBinding="rptSLAParent_DataBinding">
                                <HeaderTemplate>
                                    <tr class="titleclass">
                                        <th scope="col" style="font-weight: bold; text-align: center;">Activity</th>
                                        <th scope="col" style="width: 78px !important; font-weight: bold; text-align: center; background-color: #39CB71;">Target (Bus&nbsp;<%=HeaderText %>)</th>
                                        <th style="width: 78px !important; text-align: center; font-weight: bold; padding-left: 11px; background-color: #43E9E9;">Actual (Bus&nbsp;<%=HeaderText %>) </th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: left; font-weight: normal!important; min-width: 150px;">
                                            <asp:Label ID="lblRuleName" runat="server" Text='<%# Eval("Title") %>' /></td>
                                        <td style="text-align: center; background-color: #39CB71; font-weight: normal!important;">
                                            <asp:Label ID="lblSLATargetX2" runat="server" Text='<%# Eval("SLATargetX2") %>' /></td>
                                        <td style="text-align: center; background-color: #43E9E9; font-weight: normal!important;">
                                            <asp:Label ID="lblSLAActualX2" runat="server" Text='<%# Eval("SLAActualX2") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr class="summary-row">
                                        <td style="text-align: right; min-width: 150px; padding-right: 5px;">
                                            <asp:Label ID="lblRuleName" runat="server" Text='TOTAL:' /></td>
                                        <td style="text-align: center; background-color: #39CB71;">
                                            <asp:Label ID="lblSLATargetX2Total" runat="server" Text="-" /></td>
                                        <td style="text-align: center; background-color: #43E9E9;">
                                            <asp:Label ID="lblSLAActualX2Total" runat="server" Text="-" /></td>
                                    </tr>

                                </FooterTemplate>
                            </asp:Repeater>
                        </table>
                    </td>
                </tr>
            </table>
            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." ClientInstanceName="loadingPanel" Modal="True">
            </dx:ASPxLoadingPanel>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
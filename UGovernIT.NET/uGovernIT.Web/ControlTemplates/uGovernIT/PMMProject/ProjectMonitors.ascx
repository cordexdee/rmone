<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectMonitors.ascx.cs" Inherits="uGovernIT.Web.ProjectMonitors" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
   
    .autoCal label {
        display: inherit !important;
    }

    div[onclick] {
        cursor: pointer;
    }
    .projectScore-label{
        font-family: 'Poppins', sans-serif !important;
        font-size:12px;
        margin-right:3px;
    }
    .projectScore-label span{
        margin-right:5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
     var prevSelectedMonitor = null;
        var prevSelectedMonitorDetails = null;
        function showMonitorDropDown(monitorId) {
            document.getElementById('<%= selectedMonitorId.ClientID %>').value = monitorId;
            if (prevSelectedMonitor != null && prevSelectedMonitorDetails != null) {
                prevSelectedMonitor.className = "";
                prevSelectedMonitorDetails.style.display = "none";
                if (prevSelectedMonitor.id.indexOf(monitorId) > -1) {
                    prevSelectedMonitor = null;
                    return;
                }
            }

            var monitorDropDown = document.getElementById('monitorContainer' + monitorId);
            var monitorDiv = document.getElementById('monitorContainerDiv' + monitorId);

            if (monitorDropDown != null) {
                if (monitorDropDown.style.display == "none") {
                    monitorDropDown.style.display = "";
                    monitorDiv.className = "monitorContainer ugitlight1lighter";
                    prevSelectedMonitor = monitorDiv;
                    prevSelectedMonitorDetails = monitorDropDown;
                }
                else {
                    monitorDropDown.style.display = "none";
                    monitorDiv.className = "";
                }
            }
        }
        function changeMonitor(selectedValue, monitorId) {
            document.getElementById('monitorColor' + monitorId).className = selectedValue;
            document.getElementById('monitorContainer' + monitorId).style.display = "none";
            if (prevSelectedMonitor != null && prevSelectedMonitorDetails != null) {
                prevSelectedMonitor.className = "";
                prevSelectedMonitorDetails.style.display = "none";
            }
        }

        function unselectSelectedMonitor() {
            if (prevSelectedMonitor != null && prevSelectedMonitorDetails != null) {
                prevSelectedMonitor.className = "";
                prevSelectedMonitorDetails.style.display = "none";
            }
        }
</script>
<script id="dxss_saveprojectmonitors" data-v="<%=UGITUtility.AssemblyVersion %>">
    function saveMonitors() {
        document.getElementById('<%=monitorSave.ClientID %>').click();
    }

    function UpdateScrore(selectelement) {
        
        var monitorNotes = "";
        if (selectelement.id.indexOf("monitorNotes") > -1)
            monitorNotes = selectelement.value;
        
        var baseUrl = ugitConfig.apiBaseUrl;
        var monitorId, monitorOptionName, monitorWeight;
        monitorId = $(selectelement).attr("monitorId");
        var selectmonitoroption = $("#monitorContainer" + monitorId).find('select');
        monitorOptionName = $(selectmonitoroption).val();
        var monitorweight = $("#monitorContainer" + monitorId).find('.monitorweight');
        monitorWeight = monitorweight.val();
        var pmmid = parseInt('<%=PMMID%>');
        var qData = '{' + '"monitorId":"' + monitorId + '",  "monitorOptionName":"' + monitorOptionName + '" , "monitorWeight":"' + monitorWeight + '", "ProjectMonitorNotes":"' + monitorNotes + '","pmmid":' + pmmid + '}';
        $.ajax({
            type: "POST",
            url: baseUrl + "/api/module/UpdateScore",
            data: qData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var monitorstate = message;
                $('#<%=monitorColor.ClientID%>').text(monitorstate.Overallscore);
                    $('#<%=score.ClientID%>').val(monitorstate.Overallscore);
                $('#monitorColor' + monitorstate.Id).attr('class', '');
                $('#monitorColor' + monitorstate.Id).addClass(monitorstate.LEDClass);
                $('#monitorscore' + monitorstate.Id).text('  Score: ' + monitorstate.MonitorStateScore);
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function CheckedAutoUpdate(selectelement) {
        var ischecked, monitorId, monitorName, moduleName, pmmid, ticketId;
        var baseUrl = ugitConfig.apiBaseUrl;
        pmmid = parseInt('<%= Request["pmmid"]%>');
        ticketId = '<%=ticketId%>';
        ischecked = $(selectelement).prop('checked');
        selectelement = $(selectelement).parent();
        monitorId = parseInt($(selectelement).attr('MonitorId'));
        monitorName = $(selectelement).attr('MonitorName');
        moduleName = 'PMM';

        var qData = '{' + '"IsChecked":' + ischecked + ',  "MonitorStateId":' + monitorId + ' , "MonitorName":"' + monitorName + '", "ModuleName":"' + moduleName + '","TicketId":"' + ticketId + '", "PmmId":' + pmmid + '}';
        $.ajax({
            type: "POST",
            url: baseUrl + "/api/module/CheckedAutoUpdate",
            data: qData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var monitorstate = message;
                $('#<%=monitorColor.ClientID%>').text(monitorstate.Overallscore);
                $('#<%=score.ClientID%>').val(monitorstate.Overallscore);
                $('#monitorColor' + monitorstate.Id).attr('class', '');
                $('#monitorColor' + monitorstate.Id).addClass(monitorstate.LEDClass);
                $('#monitorscore' + monitorstate.Id).text('  Score: ' + monitorstate.MonitorStateScore);
                if (ischecked == true) {
                    var autospan = $(selectelement).next();
                    var manualdiv = $(selectelement).next().next();
                    $(autospan).find('span').text(monitorstate.SelectedOption);
                    manualdiv.hide();
                    autospan.show();

                }
                else {
                    var autospan = $(selectelement).next();
                    var manualdiv = $(selectelement).next().next();
                    manualdiv.show();
                    autospan.hide();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
</script>
<%--Project Monitors --%>
<div id="monitorsContainer" runat="server" style="padding-bottom: 22px;  box-shadow: 0px 0px 1px #aaaaaa;">
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="monitorSave" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div class="col-md-12 col-sm-12 col-xs-12 projectScore-wrap">
                <div>
                    <asp:Label runat="server" ID="scoreLabel" Visible="false" CssClass="scoreheading fleft" Text='Project Score: '></asp:Label>
                    <em id="monitorContainer0" style="display: none"></em>
                    <em onclick="showMonitorDropDown('0');" id="monitorColor0" class="Good total-score">
                        <asp:Label ID="monitorColor" runat="server">&nbsp;&nbsp;<%= pmmItem != null ?  pmmItem["ProjectScore"] : string.Empty %>&nbsp;&nbsp;
                        </asp:Label>
                    </em>
                    <asp:HiddenField runat="server" ID="score" Value='' />
                </div>
            </div>
            <div style="padding-top:15px; clear:both">
                <table width="100%" style="border-collapse: collapse">
                    <tr>
                        <td valign="top" class="ms-alternatingstrong">
                            <table style="border-collapse: collapse">
                                <tr>
                                    <asp:Repeater ID="monitorsRepeater" runat="server">
                                        <ItemTemplate>
                                            <td style="padding-left: 10px; padding-right: 10px;">
                                                <div id="monitorContainerDiv<%# Eval("ID")%>" class="projectScore-label" onclick="showMonitorDropDown('<%# Eval("ID")%>');">
                                                    <asp:Label runat="server" ID="monitorLabel" CssClass="heading monitorLabel fleft" Text='<%# Eval("ModuleMonitorName") + " : "%>'></asp:Label>
                                                    <em id="monitorColor<%# Eval("ID")%>" title='<%# Eval("ModuleMonitorOptionName") %>' class='<%# Eval("ModuleMonitorOptionLEDClass") %>'>
                                                        <asp:Label ID="monitorColor" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </asp:Label>
                                                    </em>
                                                    <asp:HiddenField runat="server" ID="monitorId" Value='<%# Eval("OptionID")%>' />
                                                </div>
                                            </td>
                                        </ItemTemplate>

                                    </asp:Repeater>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                    <asp:Repeater ID="monitorDetailsRepeater" runat="server" OnItemDataBound="monitorDetailsRepeater_ItemDataBound">
                        <ItemTemplate>
                            <div class="row monitor-content" id="monitorContainer<%# Eval("ID")%>" style="display: none; padding:15px;">
                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                        <div class="monitor-content-label">
                                            <span>Health :</span>
                                        </div>
                                        <div class="monitor-content-feild">
                                            <div id="defaultProcess" runat="server" class="float-left" style="width:100%">
                                                <asp:DropDownList runat="server" ID="monitorValue" MonitorId='<%# Eval("ID")%>' ClassId='<%# Eval("ModuleMonitorOptionLEDClass")%>'
                                                    DataSource='<%# FillMonitorOptions((Convert.ToInt32( Eval("ModuleMonitorOptionIdLookup")))) %>' DataValueField="ID"
                                                    selectedmonitorvalue='<%# Eval("ModuleMonitorOptionName")%>' onchange="UpdateScrore(this);" OptionID='<%# Eval("ModuleMonitorOptionIdLookup") %>'
                                                    DataTextField='ModuleMonitorOptionName' OnDataBound="FillSelectedChoice" CssClass="itsmDropDownList aspxDropDownList" Width="100%">
                                                </asp:DropDownList>
                                                </div>
                                                <div class="float-left autoCal crm-checkWrap" style="padding-left: 3px; margin-top:10px;">
                                                    <asp:CheckBox ID="autocalculate" Text="Auto Calculate" runat="server"
                                                        onclick="CheckedAutoUpdate(this);" MonitorId='<%# Eval("ID")%>' MonitorName='<%# Eval("ModuleMonitorName")%>'
                                                        Checked='<%# (!(Eval("AutoCalculate") is DBNull) &&  UGITUtility.StringToBoolean(Eval("AutoCalculate")))?true:false %>' />
                                                    <div id="autoCalculateActive" runat="server" class="float-left">
                                                        <span><%# Eval("ModuleMonitorOptionName")%></span>
                                                    </div>
                                                </div>
                                                <asp:HiddenField ID="hdnmonitorId" runat="server" Value='<%# Eval("ID")%>' />
                                                <asp:HiddenField ID="hdnmonitorName" runat="server" Value='<%# Eval("ModuleMonitorName")%>' />
                                        </div>
                                    </div>

                                    <div class="col-md-6 col-sm-6 col-xs-12">
                                         <div class="monitor-content-label">
                                                <span>Weight :</span>
                                         </div>
                                        <div class="monitor-content-feild">
                                            <asp:TextBox runat="server" ID="monitorWeight" Text='<%# Eval("ProjectMonitorWeight")%>'
                                                MonitorId='<%# Eval("OptionID")%>' CssClass="monitorweight EditTicket-popupTextarea" onchange="UpdateScrore(this);"></asp:TextBox>
                                            <span id='monitorscore<%# Eval("ID")%>' class="score-label">
                                                <%# (Eval("ModuleMonitorOptionIdLookup") != null && Eval("ProjectMonitorWeight") != null) ?  GetMonitorScore(Eval("ModuleMonitorOptionIdLookup").ToString(), Eval("ProjectMonitorWeight").ToString()) : string.Empty%>
                                            </span>
                                        </div>
                                    </div>
                                <div class="col-md-12 col-sm-12 col-xs-12" style="clear:both;">
                                    <div class="monitor-content-label">
                                        <span>Notes :</span>
                                    </div>
                                    <div class="monitor-content-feild">
                                         <asp:TextBox runat="server" CssClass="EditTicket-popupTextarea" MonitorId='<%# Eval("ID")%>' ID="monitorNotes" Height="87" TextMode="MultiLine" Style="margin-right: 10px;" Text='<%# Eval("ProjectMonitorNotes")%>'></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-6 col-sm-6 col-xs-12" style="display:none">
                                     <img src="/content/images/save-icon.png" onclick="saveMonitors()" alt="Save" title="Save" />
                                    <img src="/Content/images/cancel-icon.png" alt="Cancel" title="Cancel" onclick="unselectSelectedMonitor();" />
                                </div>
                                <asp:HiddenField runat="server" ID="monitorId" Value='<%# Eval("OptionID")%>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </div>
            </div>
            
            <div style="display: none">
                <asp:ImageButton OnClick="monitorSave_Click" ID="monitorSave" ImageUrl="/Content/images/save-icon.png" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div style="display: none;">
    <asp:HiddenField ID="selectedMonitorId" runat="server" />
</div>
<div id="monitorsContainerReadOnly" runat="server" class="readonlyblock">
        <table width="100%" style="border-collapse: collapse">
            <tr>
                <td valign="top" class="ro-monitorshead">
                    <table style="border-collapse: collapse">
                        <tr>
                            <td width="170">
                                    <div>
                                        <asp:Label runat="server" ID="monitorLabel"  CssClass="scoreheading fleft" Text='Project Score: '></asp:Label>
                                        <em  style="display: none"></em>
                                        <em   class="Good total-score">
                                            <asp:Label ID="Label2" runat="server">&nbsp;&nbsp;<%= pmmItem != null ?  pmmItem["ProjectScore"] : string.Empty %>&nbsp;&nbsp;
                                            </asp:Label>
                                        </em>
                                        <asp:HiddenField runat="server" ID="HiddenField1" Value='' />
                                    </div>
                                </td>
                                <asp:Repeater ID="rReadOnlyMonitors" runat="server">
                                    <ItemTemplate>
                                        <td style="padding-left: 10px; padding-right: 10px;">
                                            <div id="monitorContainerDiv<%# Eval("ID")%>" class="projectScore-label" >
                                                <asp:Label runat="server" ID="monitorLabel" CssClass="heading monitorLabel fleft" Text='<%# Eval("ModuleMonitorName") + " : "%>'></asp:Label>
                                                <em id="monitorColor<%# Eval("ID")%>" title='<%# Eval("ModuleMonitorOptionName") %>' class='<%# Eval("ModuleMonitorOptionLEDClass") %>'>
                                                    <asp:Label ID="monitorColor" runat="server">&nbsp;&nbsp;&nbsp;&nbsp;
                                                    </asp:Label>
                                                </em>
                                                <asp:HiddenField runat="server" ID="monitorId" Value='<%# Eval("OptionID")%>' />
                                            </div>
                                        </td>
                                    </ItemTemplate>

                                </asp:Repeater>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>




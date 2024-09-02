<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceProjectComplexity.ascx.cs" Inherits="uGovernIT.Web.ResourceProjectComplexity" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .rmmLookup-valueBoxEdit table.department tr td.dxic input[type="text"] {
        height: 20px !important;
        background: #fff;
    }

    .refreshButtonRight{
        float:right;
    }

    .rightPadding{
        padding-right:12px;
    }
    .dxpgTotalCell_UGITNavyBlueDevEx {
        width: 118px;
    }
    .dxpgRowArea_UGITNavyBlueDevEx {
        width: 203px;
    }
    .tblwidth {
        width: 1036px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">    
    var RefreshData = "Refresh Data";
    var ComplexityRefreshInProcess = "Complexity Refresh In Process";

    $(document).ready(function () {
        setTimeout(CheckComplexityRefreshInProcess, 500);
    });

    function adjustControlSize() {
        setTimeout(function () {
            $("#s4-workspace").width("100%");
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }
     
        function CheckComplexityRefreshInProcess()
        {
            var baseUrl = ugitConfig.apiBaseUrl;
            $.ajax({
                url: baseUrl + "/api/module/CheckComplexityRefreshInProcess",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                type: "POST"
            }).done(function (data) {
                if (data == true) {
                    setTimeout(CheckComplexityRefreshInProcess, 500);
                    btnRefreshProjectComplexity.SetEnabled(false);
                    btnRefreshProjectComplexity.SetText(ComplexityRefreshInProcess);
                }
                else {
                    btnRefreshProjectComplexity.SetEnabled(true);
                    btnRefreshProjectComplexity.SetText(RefreshData);
                    pvtGrdResourceComplexity.PerformCallback();
                }
            });
        }

    function refreshProjectAllocation(s, e) {
        var baseUrl = ugitConfig.apiBaseUrl;
        btnRefreshProjectComplexity.SetEnabled(false);
        btnRefreshProjectComplexity.SetText(ComplexityRefreshInProcess);

        $.ajax({
            url: baseUrl + "/api/module/RefreshProjectSummary",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
        }).done(function (data) {
            if (data == true) {
                setTimeout(CheckComplexityRefreshInProcess, 500);
            }
            else {
                btnRefreshProjectComplexity.SetEnabled(true);
                btnRefreshProjectComplexity.SetText(RefreshData);
                pvtGrdResourceComplexity.PerformCallback();
            }
        });
    }

    function onDepartmentChanged(ccID) {
        var cmbDepartment = $("#" + ccID +" span");
        //var selectedDepartments = cmbDepartment.attr("id");
        //pvtGrdResourceComplexity.PerformCallback(selectedDepartments);
        var selectedDepts = "";
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepts = selectedDepts + cmbDepartment[i].id + ",";

        cbpManagers.PerformCallback(selectedDepts);
        cbpComplexity.PerformCallback("LoadRoles~" + selectedDepts);
    }

    function InitiateGridCallback(s, e) {
        if (!s.InCallback())
            pvtGrdResourceComplexity.PerformCallback();
    }
</script>

<asp:HiddenField ID="hdnaspDepartment" runat="server" />  
<div class="col-md-12 col-sm-12 col-xs-12 reourceUti-container noSidePadding">
    <div class="row">
         <div id="divFilter" runat="server" class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                <div class="row" style="padding-top: 10px; padding-bottom: 10px">
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12">
                        <div class="resourceUti-label">
                            <label>Department:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit" FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />
                            <%--<asp:DropDownList CssClass="txtbox-halfwidth aspxDropDownList rmm-dropDownList" ID="ddlDepartment" runat="server"  OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"
                                AutoPostBack="true"></asp:DropDownList>--%>
                        </div>
                    </div>
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12" style="display:none;">
                        <div class="resourceUti-label">
                            <label>Functional Area:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <asp:DropDownList CssClass="txtbox-halfwidth aspxDropDownList rmm-dropDownList" ID="ddlFunctionalArea" runat="server"  AutoPostBack="true" 
                                OnSelectedIndexChanged="ddlFunctionalArea_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12">
                        <div class="resourceUti-label">
                            <label id="tdManager" runat="server">Manager:</label>
                        </div>
                        <div class="resourceUti-inputField" id="tdManagerDropDown" runat="server">
                            <dx:ASPxCallbackPanel ID="cbpManagers" runat="server"  ClientInstanceName="cbpManagers" OnCallback="cbpManagers_Callback" SettingsLoadingPanel-Enabled="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <asp:DropDownList ID="ddlResourceManager" CssClass="managerdropdown aspxDropDownList rmm-dropDownList"  runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlResourceManager_SelectedIndexChanged"></asp:DropDownList>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>                            
                        </div>
                    </div>
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12">
                        <div class="resourceUti-label">
                            <label  id="td3" runat="server">Roles:</label>
                        </div>
                        <div class="resourceUti-inputField" id="td4" runat="server">
                            <dx:ASPxCallbackPanel ID="cbpComplexity" runat="server" ClientInstanceName="cbpComplexity" OnCallback="cbpComplexity_Callback" SettingsLoadingPanel-Enabled="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <asp:DropDownList ID="ddlUserGroup" CssClass="managerdropdown aspxDropDownList rmm-dropDownList"  runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlResourceManager_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </dx:PanelContent>
                                </PanelCollection>
                                <ClientSideEvents EndCallback="function(s,e){ InitiateGridCallback(s,e);}" />
                            </dx:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12 rightPadding refreshButtonRight next-cancel-but" id="tdType" runat="server">
                        <div class="rmmProject-complexityWrap">
                           <dx:ASPxButton ID="btnRefreshProjectComplexity" ClientInstanceName="btnRefreshProjectComplexity" runat="server" Text="Refresh Data" 
                               AutoPostBack="true" CssClass="RMMBtn-setColor next" >
                                <ClientSideEvents Click="refreshProjectAllocation" />
                            </dx:ASPxButton>
                        </div>
                        <div class="rmmProject-complexityWrap">
                            <dx:ASPxButton ID="btnRefreshComplexityInProcess" ClientInstanceName="btnRefreshComplexityInProcess" Visible="false" runat="server" AutoPostBack="true" 
                            Text="Complexity Refresh In Process" CssClass="RMMBtn-setColor next"></dx:ASPxButton>
                        </div>
                    </div>
                </div>
                <div class="row" style="padding-top:5px;">
                    <div class="col-xl-2 col-md-3 col-sm-3 col-xs-12">
                        <div class="rmmChkBox-container" style="float: none">
                            <asp:CheckBox ID="chkIncludeClosed" runat="server" CssClass="RMM-checkWrap" Text="Include Closed Projects" AutoPostBack="true" OnCheckedChanged="chkIncludeClosed_CheckedChanged" />                                    
                        </div>
                    </div>
                </div>
                <div class="row tblwidth">
                    <div id="tdGridResourceComplexity" runat="server" class="col-md-12 col-sm-12 col-xs-12 respurceUti-gridContainer" style="height:600px;" >
                        <dx:ASPxPivotGrid ID="pvtGrdResourceComplexity" ClientInstanceName="pvtGrdResourceComplexity" runat="server" Width="100%"  
                            OnDataBinding="pvtGrdResourceComplexity_DataBinding" CssClass="homeGrid" OnCustomCallback="pvtGrdResourceComplexity_CustomCallback"
                            OnCustomCellStyle="pvtGrdResourceComplexity_CustomCellStyle"  OnFieldValueDisplayText="pvtGrdResourceComplexity_FieldValueDisplayText">
                            <Styles>
                                <GrandTotalCellStyle Wrap="True" HorizontalAlign="Center" BackColor="#ffffff"></GrandTotalCellStyle>
                                <ColumnAreaStyle  HorizontalAlign="Center"></ColumnAreaStyle>
                                <CellStyle HorizontalAlign="Center" CssClass="prjectComplexity-gridCell"></CellStyle>
                                <FieldValueGrandTotalStyle HorizontalAlign="Center"></FieldValueGrandTotalStyle>
                                <RowAreaStyle CssClass="prjectComplexity-gridRowArea"></RowAreaStyle>
                                <HeaderStyle CssClass="prjectComplexity-gridRowArea" />
                            </Styles>
                            <OptionsFilter  />
                            <OptionsPager RowsPerPage="20" Position="Bottom"></OptionsPager>
                            <OptionsView ShowColumnGrandTotals="False" ShowDataHeaders="False" ShowColumnHeaders="False" ShowColumnGrandTotalHeader="False"  ShowFilterHeaders="False"/>
                        </dx:ASPxPivotGrid>
                    </div>
            </div>
        </div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BenchAnalytics.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Bench.BenchAnalytics" %>


<style>
    .discription-title {
        text-align: center;
        font-size: 24px;
        font-weight: 600;
        margin-bottom: 10px;
        color: black;
        font-family: 'Roboto', sans-serif !important;
        background-color: white;
        /*border-radius: 7px;*/
        padding: 5px;
        /*margin-right: 10px*/
    }
    .dashboard-panel-new {
        border: none !important;
        padding: 15px;
        background-color: #DFDFDF !important;
    }

    .resourceViewContainer {
        overflow: auto;
        background: white;
        padding: 5px;
    }

    body{
        font-family: 'Roboto', sans-serif !important;
    }

    .selected {
        background: none !important;
    }

     .filteralign{
     float:left;
     margin-top:25px;
     margin-left:-22px;
 }  
</style>
<script>
    const baseUrl = ugitConfig.apiBaseUrl;
    var windowFilters = {
        IncludeAllResources: 0,
        IncludeClosedProject: 0,
        DisplayMode: "HRS",
        Department: "",
        Function: "",
        StartDate: "<%=StartDate%>",
        EndDate: "<%=EndDate%>",
        ResourceManager: "",
        AllocationType: "Estimated",
        LevelName: "",
        GlobalRoleId: "",
        Mode: '<%=DefaultDisplayMode%>',
    };

    // Get current date
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();
    var startDate = new Date(currentYear, 0, 1); // Month is 0-based (0 for January)
    var endDate = new Date(currentYear, 11, 31); // Month is 0-based (11 for December)

    $(() => {
        
        //windowFilters.StartDate = startDate.toISOString();
        //windowFilters.EndDate = endDate.toISOString();
        LoadTrendChart();
    });

    function LoadTrendChart() {
        ResourceAvailabilityloadingPanel.Show();
        $.get(baseUrl + `/api/Bench/GetBenchAnalyticstData?` + $.param(windowFilters), function (data, status) {
            ResourceAvailabilityloadingPanel.Hide();
            const chart = $('#divChart').dxChart({
                dataSource: data,
                height: 430,
                commonSeriesSettings: {
                    type: 'stackedBar',
                    argumentField: 'Month',
                },
                commonAxisSettings: {
                    grid: {
                        visible: false,
                    }
                },
                export: {
                    enabled: false,
                    printingEnabled: false
                },
                series: [
                    { valueField: 'AllocatedDemand', name: 'Allocated Demand', color: '#A7BEE1' },
                    { valueField: 'UnfilledDemand', name: 'Unfilled Demand', color: '#D39061' }, 
                    {
                        type: 'spline',
                        valueField: 'TotalCapacity',
                        ignoreEmptyPoints: true,
                        name: 'Total Capacity',
                        color: '#737373',
                        label: {
                            visible:true,
                        }
                    }
                ],
                title: {
                    text: 'Resource Utilization',
                    font: {
                        color: '#000',
                        size: 16,
                        weight: 500,
                        family: 'Roboto, sans-serif !important',
                    }
                },
                argumentAxis: {
                    label: {
                        font: {
                            color: '#000000'
                        },
                    },
                    //constantLines: [{
                    //    value: prevMonday,
                    //    color: '#737373',
                    //    dashStyle: 'dash',
                    //    width: 2,
                    //    label: { text: 'Today' },
                    //}],
                },
                valueAxis: {
                    label: {
                        font: {
                            color: '#000000'
                        }
                    },
                },
                legend: {
                    verticalAlignment: 'bottom',
                    horizontalAlignment: 'center',
                },
            });
        });
    }

    function onDepartmentChanged(ccID) {
        var cmbDepartment = $("#" + ccID + " span");
        var selectedDepartments = cmbDepartment.attr("id");
        var selectedDepts = "";
        for (i = 0; i < cmbDepartment.length; i++) {
            if (i == 0) {
                selectedDepts = cmbDepartment[i].id;
            }
            else {
                selectedDepts = selectedDepts + "," + cmbDepartment[i].id;
            }
        }
        document.getElementById('<%= hdnaspDepartment.ClientID %>').value = selectedDepts;
        cbpResourceAvailability.PerformCallback("LoadRoles~" + selectedDepts.substring(0, selectedDepts.length - 1));
        windowFilters.Department = selectedDepts;

        LoadTrendChart();
    }

    function cmbFunctionRole_Changed(s, e) {
        var functionid = s.GetValue();
        document.getElementById('<%= hdnaspFunction.ClientID %>').value = (functionid != null ? functionid.join(';#') : "");
        cbpResourceAvailability.PerformCallback("FunctionalRole | " + (functionid != null ? functionid.join(';#') : ""));
        windowFilters.Function = functionid != null ? functionid.join(',') : "";
        LoadTrendChart();
    }

    //function cmbRole_onSelectedIndexChanged(s, e) {
    //    var roleid = s.GetValue();
    //    windowFilters.GlobalRoleId = roleid;
    //    LoadTrendChart();
    //}

    function ShowLoader() {
        ResourceAvailabilityloadingPanel.SetText('Loading...');
        var width = window.innerWidth;
        var height = window.outerHeight;
        ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
    }

    function GetRoleSelectedValue(s, e) {
        let roleids = s.GetValue();

        let value = roleids != null ? roleids.join(';') : "";


        $("#hdnRoleId").val(value);
        var path = "";
        $.cookie("roleid", value, { path: "/" });
        windowFilters.GlobalRoleId = roleids != null ? roleids.join(',') : "";
        LoadTrendChart();
    }
    function setFilterMode() {
        debugger;
        var dd = dtFromclient.GetValue().getDate();
        var mm = dtFromclient.GetValue().getMonth() + 1;
        var y = dtFromclient.GetValue().getFullYear();
        windowFilters.StartDate = dd + '-' + mm + '-' + y;

        var dd2 = dtToclient.GetValue().getDate();
        var mm2 = dtToclient.GetValue().getMonth() + 1;
        var y2 = dtToclient.GetValue().getFullYear();
        windowFilters.EndDate = dd2 + '-' + mm2 + '-' + y2;
        ResourceAvailabilityloadingPanel.Show();
        LoadTrendChart();
    }
</script>
<dx:ASPxLoadingPanel ID="ResourceAvailabilityloadingPanel" runat="server" ClientInstanceName="ResourceAvailabilityloadingPanel" Text="Loading..." ImagePosition="Top" CssClass="customeLoader" Modal="True">
    <Image Url="~/Content/Images/ajaxloader.gif"></Image>
</dx:ASPxLoadingPanel>
<div class="row">
    <div class="discription-title">
        <div>Bench Analytics</div>
    </div>
    <div class="resourceViewContainer" style="overflow: auto">
            <div class="row">
                <div id="filtersdiv">
                    <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                        <div class="row resourceUti-filterWarp">
                            <div class="col-xl-2 col-md-4 col-sm-4 col-xs-12 colForTabView">
                                <div class="resourceUti-label">
                                    <label>Department:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit rmmDepartment-drpDown" 
                                        FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />
                                </div>
                            </div>
                            <div class="col-xl-2 col-md-4 col-sm-4 col-xs-12 colForTabView">
                                <div class="resourceUti-label">
                                    <label>Function:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <dx:ASPxGridLookup Visible="true" TextFormatString="{0}" SelectionMode="Multiple" ID="functionGridLookup"
                                        runat="server" MultiTextSeparator="; " CssClass="rmmGridLookup" AllowUserInput="false"
                                        GridViewProperties-Settings-VerticalScrollableHeight="175"
                                        DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow"
                                        Style="width: 100%">
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" SelectAllCheckboxMode="Page" ShowClearFilterButton="true" ToolTip="Select All" />
                                            <dx:GridViewDataTextColumn FieldName="Title" Width="200px" Caption="Choose Function:">
                                            </dx:GridViewDataTextColumn>

                                        </Columns>
                                        <GridViewProperties>
                                            <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                            <SettingsBehavior AllowSort="false" />
                                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                            <SettingsText SelectAllCheckBoxInPageMode="Select All"></SettingsText>
                                        </GridViewProperties>
                                        <ClientSideEvents BeginCallback="function(s,e){cmbFunctionRole_Changed(s,e); }" />
                                    </dx:ASPxGridLookup>
                                    <dx:ASPxComboBox visible="false" ID="cmbFunctionRole" runat="server" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="cmbFunctionRole_Changed" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                    
                            <div class="col-xl-2 col-md-4 col-sm-4 col-xs-12 colForTabView">
                                <div class="resourceUti-label">
                                    <label>Roles:</label>
                                </div>
                                <div class="resourceUti-inputField">
                                    <dx:ASPxCallbackPanel ID="cbpResourceAvailability" ClientInstanceName="cbpResourceAvailability" runat="server" OnCallback="cbpResourceAvailability_Callback" SettingsLoadingPanel-Enabled="false">
                                        <PanelCollection>
                                            <dx:PanelContent>
                                                <dx:ASPxGridLookup SelectInputTextOnClick="false" KeyFieldName="ID" Visible="true" TextFormatString="{0}" SelectionMode="Multiple" ID="userGroupGridLookup"
                                                    runat="server" MultiTextSeparator="; " CssClass="rmmGridLookup" AllowUserInput="false"
                                                    GridViewProperties-Settings-VerticalScrollableHeight="175" AutoPostBack="false"
                                                    DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow"
                                                    Style="width: 100%">
                                                    <Columns>
                                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" SelectAllCheckboxMode="Page" ShowClearFilterButton="true" ToolTip="Select All" />
                                                        <dx:GridViewDataTextColumn FieldName="Name" Width="200px" Caption="Choose Role:">
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                    <GridViewProperties>
                                                        <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                                                        <SettingsBehavior AllowSort="false" />
                                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                        <SettingsText SelectAllCheckBoxInPageMode="Select All"></SettingsText>
                                                    </GridViewProperties>
                                                    <ClientSideEvents BeginCallback="function(s,e){GetRoleSelectedValue(s,e); }" />
                                                </dx:ASPxGridLookup>
                                                <asp:DropDownList Visible="false" ID="ddlUserGroup" runat="server" AutoPostBack="false" onchange="GetRoleSelectedValue();"
                                                     CssClass="resourceUti-dropDownList aspxDropDownList rmm-dropDownList">
                                                </asp:DropDownList>
                                                <%--<dx:ASPxComboBox ID="cmbRole" ClientInstanceName="cmbRole" runat="server" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith"
                                                    EnableSynchronization="True" CssClass="aspxComboBox-dropdown" ListBoxStyle-CssClass="aspxComboBox-listBox" AutoPostBack="false">
                                                    <ClientSideEvents SelectedIndexChanged="cmbRole_onSelectedIndexChanged" />
                                                </dx:ASPxComboBox>--%>
                                                <asp:HiddenField ID="hdnaspDepartment" runat="server" />
                                                <asp:HiddenField ID="hdnaspFunction" runat="server" />
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxCallbackPanel>

                                </div>
                            </div>
                            <div class="col-xl-2 col-md-4 col-sm-4 col-xs-12 colForTabView">
                                <b class="selectFromTo_label">From:</b>
                                <dx:ASPxDateEdit ID="dtFrom" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"
                                    CssClass="CRMDueDate_inputField homeDB-dateInput" ClientInstanceName="dtFromclient" runat="server" Visible="true">
                                    <DropDownButton>
                                    <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                    </DropDownButton>
                                </dx:ASPxDateEdit>
                                </div>
                            <div class="col-xl-2 col-md-4 col-sm-4 col-xs-12 colForTabView">
                                <b class="selectFromTo_label">To:</b>
                                <div class="selectFromTo_field_new">
                                    <dx:ASPxDateEdit ID="dtTo" runat="server" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"
                                        CssClass="CRMDueDate_inputField homeDB-dateInput" ClientInstanceName="dtToclient" Visible="true">
                                        <DropDownButton>
                                        <Image Width="16px" Url="~/Content/Images/calendarNew.png"></Image>
                                        </DropDownButton>
                                    </dx:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="col-xl-2 col-md-4 col-sm-4 col-xs-12 colForTabView">
                                <img id="imgAdvanceMode" class="homeDb_filterImg filteralign" runat="server" onclick="setFilterMode()" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div id="divChart"></div>
            </div>
    </div>

</div>
<asp:HiddenField ID="hdnGenerateColumns" runat="server" Value="0" />

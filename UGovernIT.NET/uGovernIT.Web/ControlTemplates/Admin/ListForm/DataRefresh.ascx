<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataRefresh.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.DataRefresh" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var RefreshData = "Migrate Resource Allocation";
    var UpdateAllocationInProcess = "Update Resource Allocation In Process";
    var summaryText = 'Refresh Summary Data';
    var summaryRefeshMsg = 'Summary Data Refresh In Process';
    $(document).ready(function () {
        //setTimeout(CheckUpdateAlloctionInProcess, 500);
        //setTimeout(CheckUpdateSummaryInProcess, 500);
    });

    function CheckUpdateAlloctionInProcess() {
        var baseUrl = ugitConfig.apiBaseUrl;
        $.ajax({
            url: baseUrl + "/api/rmmapi/CheckUpdateAlloctionInProcess",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST"
        }).done(function (data) {
            if (data == true) {
                setTimeout(CheckUpdateAlloctionInProcess, 500);
                btnUpdateResourceAllocation.SetEnabled(false);
                btnUpdateResourceAllocation.SetText(UpdateAllocationInProcess);
            }
            else {
                btnUpdateResourceAllocation.SetEnabled(true);
                btnUpdateResourceAllocation.SetText(RefreshData);
            }
        });
    }
    function AddLogInfo(message) {
        var baseUrl = ugitConfig.apiBaseUrl;
        $.get(baseUrl + "api/rmmapi/AddLogInfo?message=" + message)
    }
    
    function updateResourceAllocation(s, e) {
        AddLogInfo("User Clicked Update Resource Allocation");
        if (confirm('Warning:  This will copy records from ProjectEstimatedAllocation to ResourceAllocation table.  Copying may take time and should only be run during non-working hours. Are you sure you want to submit ?') == false) {
            AddLogInfo("User Cancelled Update Resource Allocation");
            e.processOnServer = false;
            return false;
        }

        AddLogInfo("User Confirmed Update Resource Allocation");

        var baseUrl = ugitConfig.apiBaseUrl;
        btnUpdateResourceAllocation.SetEnabled(false);
        btnUpdateResourceAllocation.SetText(UpdateAllocationInProcess);

        $.ajax({
            url: baseUrl + "/api/rmmapi/UpdateResourceAllocation",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
        }).done(function (data) {
            if (data == true) {
                setTimeout(CheckUpdateAlloctionInProcess, 500);
            }
            else {
                btnUpdateResourceAllocation.SetEnabled(true);
                btnUpdateResourceAllocation.SetText(RefreshData);
            }

        });

    }

    function UpdateResourceSummaryData(s, e) {
        AddLogInfo("User Clicked Refresh Summary Data");
        if (confirm('Warning:  Refresh Summary Data will replace RMM allocation summary table.  Refresh may run 3 hours or more and should only be run during non-working hours. Are you sure you want to submit ?') == false) {
            AddLogInfo("User Cancelled Refresh Summary Data");
            e.processOnServer = false;
            return false;
        }

        AddLogInfo("User Confirmed Refresh Summary Data");

        var baseUrl = ugitConfig.apiBaseUrl;
        BtnUpdateSummaryDataInProcess.SetEnabled(false);
        BtnUpdateSummaryDataInProcess.SetText(summaryRefeshMsg);

        $.ajax({

            url: baseUrl + "/api/rmmapi/UpdateResourceSummary",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST",
        }).done(function (data) {
            if (data == true) {
                setTimeout(CheckUpdateSummaryInProcess, 500);
            }
            else {
                BtnUpdateSummaryDataInProcess.SetEnabled(true);
                BtnUpdateSummaryDataInProcess.SetText(summaryText);
            }

        });

    }
    function CheckUpdateSummaryInProcess() {
        var baseUrl = ugitConfig.apiBaseUrl;
        $.ajax({
            url: baseUrl + "/api/rmmapi/CheckUpdateSummaryInProcess",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            type: "POST"
        }).done(function (data) {
            if (data == true) {
                setTimeout(CheckUpdateSummaryInProcess, 500);
                BtnUpdateSummaryDataInProcess.SetEnabled(false);
                BtnUpdateSummaryDataInProcess.SetText(UpdateResourceSummaryData);
            }
            else {
                BtnUpdateSummaryDataInProcess.SetEnabled(true);
                BtnUpdateSummaryDataInProcess.SetText(summaryText);
            }
        });
    }
    function showWait(message) {
        aspxConfigCacheLoading.SetText(message);
        aspxConfigCacheLoading.Show(); // Postback hides automatically, so no need to call Hide() later
    }

    function ConfirmRefreshSummary() {
        if (confirm('Warning:  Refresh Summary Data will replace RMM allocation summary table.  Refresh may run 3 hours or more and should only be run during non-working hours. Are you sure you want to submit ?') == true)
            return true;
        else
            return false;
    }


    function ImportProjectAllocations(s, e) {
        var param = '';
        var customDialog = DevExpress.ui.dialog.custom({
            title: "Import Project Allocations Alert",
            message: "Please select one of the options below.",
            buttons: [
                { text: "Delete Existing and Import New Allocations", onClick: function () { return "NewAllocations" } },
                { text: "Update Existing Allocations", onClick: function () { return "UpdateAllocations" } },
                { text: "Cancel", onClick: function () { return "Cancel" } }
            ]
        });
        customDialog.show().done(function (dialogResult) {
            if (dialogResult == "NewAllocations") {
                param = param + "&Action=newallocations";
            }
            else if (dialogResult == "UpdateAllocations") {
                param = param + "&Action=updateallocations";
            }
            else
                return false;

            window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=importexcelfile&listName=ProjectAllocations', param, 'Import Project Allocations', '500px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        });
    }

    function ImportTimeOffAllocations(s, e) {
        var param = '';
        var customDialog = DevExpress.ui.dialog.custom({
            title: "Import TimeOff Allocations",
            message: "Please select one of the options below.",
            buttons: [
                { text: "Import TimeOff Allocations", onClick: function () { return "NewAllocations" } },
                { text: "Cancel", onClick: function () { return "Cancel" } }
            ]
        });
        customDialog.show().done(function (dialogResult) {
            if (dialogResult == "NewAllocations") {
                param = param + "&Action=newallocations";
            }
            else
                return false;

            window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=importexcelfile&listName=TimeOffAllocations', param, 'Import TimeOff Allocations', '500px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        });
    }

    function ImportActualTime(s, e) {
        var param = '';
        var customDialog = DevExpress.ui.dialog.custom({
            title: "Import Actual Time",
            message: "Please select one of the options below.",
            buttons: [
                { text: "Import Actual Time", onClick: function () { return "Actual Time" } },
                { text: "Cancel", onClick: function () { return "Cancel" } }
            ]
        });
        customDialog.show().done(function (dialogResult) {
            if (dialogResult == "Actual Time") {
                param = param + "&Action=newTimesheet";
            }
            else
                return false;

            window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=importexcelfile&listName=ActualTime', param, 'Import Actual Time', '500px', '300px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        });

        function showLoader() {
            var width = window.innerWidth;
            var height = window.outerHeight;
            //ResourceAvailabilityloadingPanel.Show();
            ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
        }

        function hideLoader() {
            ResourceAvailabilityloadingPanel.hide();
        }
    }

    var allowServerClick = false;

    function cnfmUpdateShortname(s, e) {
        var shortNameLength = "<%=ShortNameLength %>";
        e.processOnServer = allowServerClick;
        if (allowServerClick == false) {
            var result = DevExpress.ui.dialog.confirm("<i>Recreating the short name clears all existing short names and creates a short name using the first " + shortNameLength + " characters of the Project Title?</i>", "Warning!");
            //$(".dx-popup-normal").width("50%");
            $(".dx-popup-wrapper > .dx-overlay-content").width("60%");
            result.done(function (dialogResult) {
                if (dialogResult) {
                    allowServerClick = true;
                    btn_updateshortname.DoClick();
                    lPanel.Show();
                } else {
                    allowServerClick = false;
                }
            });
        }
    }

    function cnfmCreateProjectTags(s, e) {
        refreshData.SetText('');
        var params = "ProcessSelectedRecords=False";
        window.parent.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=createprojecttags', params, "Create Project Tags", '700px', '270px', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function cnfmCreateResourceTags(s, e) {
        e.processOnServer = allowServerClick;
        if (allowServerClick == false) {
            var customDialog = DevExpress.ui.dialog.custom({
                title: "Create Resource Experience Tags",
                message: "Creating Resource tags will update OR delete all existing and create new resource experience tags. Please select one of the options below.",
                buttons: [
                    { text: "Delete Existing and Create New Tags", onClick: function () { return "NewTags" } },
                    { text: "Update Existing Tags", onClick: function () { return "UpdateTags" } },
                    { text: "Cancel", onClick: function () { return "Cancel" } }
                ]
            });
            customDialog.show().done(function (dialogResult) {
                if (dialogResult == "NewTags") {
                    var result = DevExpress.ui.dialog.confirm("<i>Are you sure you want to delete existing and create New Tags?</i>", "Warning!");
                    result.done(function (dialogResult) {
                        if (dialogResult) {
                            allowServerClick = true;
                            refreshData.SetText('Please wait until you see the success message.');
                            btn_createNewUserExpTags.DoClick();
                            lPanel.Show();
                        } else {
                            allowServerClick = false;
                            return false;
                        }
                    });
                }
                else if (dialogResult == "UpdateTags") {
                    var result = DevExpress.ui.dialog.confirm("<i>Are you sure you want to update existing Tags?</i>", "Warning!");
                    result.done(function (dialogResult) {
                        if (dialogResult) {
                            allowServerClick = true;
                            refreshData.SetText('Please wait until you see the success message.');
                            btn_updateUserExpTags.DoClick();
                            lPanel.Show();
                        } else {
                            allowServerClick = false;
                            return false;
                        }
                    });
                }
                else {
                    allowServerClick = false;
                    return false;
                }
            });
            $(".dx-popup-wrapper > .dx-overlay-content").width("60%");
        }
    }

    function register(s, e) {
        e.processOnServer = true;
    }
    function cnfmupdatedisabledusersallocation(s, e) {
        e.processOnServer = allowServerClick;
        if (allowServerClick == false) {
            var result = DevExpress.ui.dialog.confirm("Are you sure to update allocations for disabled users?", "Warning!");
            //$(".dx-popup-normal").width("50%");
            $(".dx-popup-wrapper > .dx-overlay-content").width("60%");
            result.done(function (dialogResult) {
                if (dialogResult) {
                    allowServerClick = true;
                    btn_updatedisabledusersallocation.DoClick();
                    lPanel.Show();
                } else {
                    allowServerClick = false;
                }
            });
        }
    }

</script>
<style>
    .dxeBase_UGITNavyBlueDevEx {
        font-family:Roboto sans-serif !important;
        font-size: 15px !important;
        font-weight: 700 !important;
    }
</style>
<dx:ASPxLoadingPanel ID="ASPxLoadingPanel1" runat="server" ClientInstanceName="lPanel" Text="Loading..." ImagePosition="Top" CssClass="customeLoader" Modal="True">
    <Image Url="~/Content/Images/ajaxloader.gif"></Image>
</dx:ASPxLoadingPanel>

<div style="padding-left: 10px; padding-top: 10px;">
    <span>
        <dx:ASPxButton ID="btnUpdateResourceAllocation" ClientInstanceName="btnUpdateResourceAllocation" runat="server" AutoPostBack="false" Text="Migrate Resource Allocation" CssClass="primary-blueBtn">
            <ClientSideEvents Click="updateResourceAllocation" />
        </dx:ASPxButton>
    </span>
    <span id="tdType" runat="server">
        <dx:ASPxButton ID="BtnUpdateSummaryDataInProcess" ClientInstanceName="BtnUpdateSummaryDataInProcess" Visible="true" runat="server" AutoPostBack="false" Text="Refresh Summary Data" CssClass="primary-blueBtn">
            <ClientSideEvents Click="UpdateResourceSummaryData" />
        </dx:ASPxButton>
    </span>
    <span id="Span1" runat="server">
        <dx:ASPxButton ID="btnImportProjAllocations" ClientInstanceName="btnImportProjAllocations" Visible="true" runat="server" AutoPostBack="false" Text="Import Project Allocations" CssClass="primary-blueBtn">
            <ClientSideEvents Click="ImportProjectAllocations" />
        </dx:ASPxButton>
    </span>
    <span id="Span2" runat="server">
        <dx:ASPxButton ID="btnImportTimeOffAllocations" ClientInstanceName="btnImportTimeOffAllocations" Visible="true" runat="server" AutoPostBack="false" Text="Import TimeOff Allocations" CssClass="primary-blueBtn">
            <ClientSideEvents Click="ImportTimeOffAllocations" />
        </dx:ASPxButton>
    </span>

    <span id="Span3" runat="server">
        <dx:ASPxButton ID="btnImportActualTime" ClientInstanceName="btnImportActualTime" Visible="true" runat="server" AutoPostBack="false" Text="Import Actual Time" CssClass="primary-blueBtn">
            <ClientSideEvents Click="ImportActualTime" />
        </dx:ASPxButton>
    </span>
    <span id="Span4" runat="server">
        <dx:ASPxButton ID="btnUpdateOPM_ERPJOBID" ClientInstanceName="btnUpdateOPM_ERPJOBID" Visible="false" runat="server" AutoPostBack="false" Text="Refresh OPM Data" CssClass="primary-blueBtn" OnClick="btnUpdateOPM_ERPJOBID_Click">
            <ClientSideEvents Click="function(s, e) {
                lPanel.Show();
            }" />
        </dx:ASPxButton>
    </span>
    <span id="Span5" runat="server">
        <dx:ASPxButton ID="btnfill_ResourceUtil_SummaryData" Visible="false" ClientInstanceName="btnfill_ResourceUtil_SummaryData" runat="server" AutoPostBack="false" Text="Fill Res. Utilization Data (6 yrs)" CssClass="primary-blueBtn" OnClick="btnfill_ResourceUtil_SummaryData_Click">
            <ClientSideEvents Click="function(s, e) {
                lPanel.Show();
            }" />
        </dx:ASPxButton>
    </span>
    <span id="Span6" runat="server">
        <dx:ASPxButton ID="btn_updateshortname"  Visible="true" ClientInstanceName="btn_updateshortname" runat="server" AutoPostBack="false" Text="Reset Short Name" CssClass="primary-blueBtn" OnClick="btn_updateshortname_Click">
            <ClientSideEvents Click="function(s, e) {
                cnfmUpdateShortname(s,e);
            }" />
        </dx:ASPxButton>
    </span>
     <span id="Span7" runat="server">
        <dx:ASPxButton ID="btn_createProjectTags"  Visible="true" ClientInstanceName="btn_createProjectTags" runat="server" AutoPostBack="false" Text="Create Project Tags" CssClass="primary-blueBtn">
            <ClientSideEvents Click="function(s, e) {
                cnfmCreateProjectTags(s,e);
            }" />
        </dx:ASPxButton>
    </span>
     <span id="Span8" runat="server">
        <dx:ASPxButton ID="btn_createResourceTags"  Visible="true" ClientInstanceName="btn_createResourceTags" runat="server" AutoPostBack="false" Text="Create Resource Tags" CssClass="primary-blueBtn">
            <ClientSideEvents Click="function(s, e) {
                cnfmCreateResourceTags(s,e);
            }" />
        </dx:ASPxButton>
         <dx:ASPxButton ID="btn_createNewUserExpTags"  ClientVisible="False" ClientInstanceName="btn_createNewUserExpTags" runat="server" AutoPostBack="false" OnClick="btn_createNewUserExpTags_Click">
            <ClientSideEvents Click="function(s, e) {
                register(s,e);
            }" />
        </dx:ASPxButton>
         <dx:ASPxButton ID="btn_updateUserExpTags"  ClientVisible="False" ClientInstanceName="btn_updateUserExpTags" runat="server" AutoPostBack="false" OnClick="btn_updateUserExpTags_Click">
            <ClientSideEvents Click="function(s, e) {
                register(s,e);
            }" />
        </dx:ASPxButton>
    </span>
    <span id="Span9" runat="server">
    <dx:ASPxButton ID="btn_updatedisabledusersallocation"  Visible="true" ClientInstanceName="btn_updatedisabledusersallocation" runat="server" AutoPostBack="false" Text="Update disabled users allocation" CssClass="primary-blueBtn" OnClick="btn_updatedisabledusersallocation_Click">
        <ClientSideEvents Click="function(s, e) {
            cnfmupdatedisabledusersallocation(s,e);
        }" />
    </dx:ASPxButton>
</span>
</div>

<div style="text-align:center !important;padding-top:88px !important;font-family:Roboto sans-serif !important;font-size:15px !important;font-weight:bold !important;">
    <dx:ASPxLabel runat="server" ID="refreshData" ClientInstanceName="refreshData"></dx:ASPxLabel>
</div>

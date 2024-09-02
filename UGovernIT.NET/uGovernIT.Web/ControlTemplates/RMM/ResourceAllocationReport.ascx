<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceAllocationReport.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.ResourceAllocationReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var selectedDepts = "";
    $(function () {

        $("#toast").dxToast({
            message: "Download will begin shortly. ",
            type: "info",
            displayTime: 5500,
            position: "{ my: 'center', at: 'center', of: window }"
        });

        $("#limitWarningToast").dxToast({
            message: "Date range cannot exceed 3 years.",
            type: "warning",
            displayTime: 3600,
            position: "{ my: 'center', at: 'center', of: window }"
        });

    });
    function showToastMessage(s, e) {
        // To calculate the time difference of two dates
        var Difference_In_Time = dtToDate.date.getTime() - dtFromDate.date.getTime();

        // To calculate the no. of months between two dates        
        var Difference_In_Months = Difference_In_Time / (1000 * 3600 * 24 * 30);

        if (Difference_In_Months < 37)
            $("#toast").dxToast("show");
        else {
            $("#limitWarningToast").dxToast("show");
            e.processOnServer = false;
        }
    }

    function Grid_EndCallback(s, e) {
        LoadingPanel.Hide();
    }

    function onDepartmentChanged(ccID) {
        selectedDepts = "";
        var cmbDepartment = $("#" + ccID + " span");
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepts = selectedDepts + cmbDepartment[i].id + ",";
        var selectedDepartments = cmbDepartment.attr("id");
        $('#<%= hdnDepartment.ClientID%>').val(selectedDepts);
        $('#<%= lblErrorDepartmentReqrd.ClientID%>').hide();
    }

</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text ="Loading.." ClientInstanceName="LoadingPanel" Modal="true"></dx:ASPxLoadingPanel>
&nbsp;
<div class="">
                    <div id="toast"></div>
    <div id="limitWarningToast"></div>
    <div class="col-lg-4 col-md-4 col-sm-4 col-xs-12" style="height:310px">
        <div class="resourceUti-label">
            <label>Department:</label><dx:ASPxLabel ID="lblDeptAsterisk" runat="server" ForeColor="Red" Visible="true" Text ="*"></dx:ASPxLabel>
        </div>
        <div class="resourceUti-inputField" style="width:230px">
            <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit" 
                FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />
        </div>
        <div>
           <dx:ASPxLabel ID="lblErrorDepartmentReqrd" runat="server" ForeColor="Red" Visible="false" Text ="Required"></dx:ASPxLabel>
        </div>
    </div>
    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12" style="padding-left: 25px;" >
        <div class="resourceUti-label">
            <label>From:</label><dx:ASPxLabel ID="lbldtFromAsterisk" runat="server" ForeColor="Red" Visible="true" Text ="*"></dx:ASPxLabel>
        </div>
        <div class="resourceUti-inputField" style="width:150px">
            <dx:ASPxDateEdit ID="dtFromDate" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"
                CssClass="CRMDueDate_inputField homeDB-dateInput" runat="server" Visible="true" ValidateRequestMode="Enabled" Height="30px" 
                ClientInstanceName ="dtFromDate">
                <ValidationSettings CausesValidation="true" Display="Dynamic"  ValidationGroup="Run"
                    ValidateOnLeave ="true" ErrorDisplayMode="Text" SetFocusOnError="true">
                            <RequiredField IsRequired="true" ErrorText="Required" />
                </ValidationSettings>
            </dx:ASPxDateEdit>
            <dx:ASPxLabel ID="lblErrordtFromReqrd" runat="server" ForeColor="Red" Visible="false" Text ="Required"></dx:ASPxLabel>

        </div>
    </div>
    <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
        <div class="resourceUti-label">
            <label>To:</label><dx:ASPxLabel ID="lbldtToAsterisk" runat="server" ForeColor="Red" Visible="true" Text ="*"></dx:ASPxLabel>
        </div>
        <div class="resourceUti-inputField" style="width:150px">
            <dx:ASPxDateEdit ID="dtToDate" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"
                CssClass="CRMDueDate_inputField homeDB-dateInput" runat="server" Visible="true" Height="30px" ClientInstanceName="dtToDate">
                <ValidationSettings CausesValidation="true" Display="Dynamic"  ValidationGroup="Run"
                    ValidateOnLeave ="true" ErrorDisplayMode="Text" SetFocusOnError="true">
                            <RequiredField IsRequired="true" ErrorText="Required" />
                </ValidationSettings>
            </dx:ASPxDateEdit>
            <dx:ASPxLabel ID="lblErrordtToReqrd" runat="server" ForeColor="Red" Visible="false" Text ="Required"></dx:ASPxLabel>
        </div>
    </div>
        <div class="col-md-1 col-sm-1 col-xs-12" style="vertical-align:bottom; padding-top: 13px">
            <dx:ASPxButton ID ="btnRun" runat="server" Text="Run" OnClick="btnRun_Click" CssClass="buildReport-btn" AutoPostBack="true" ValidationGroup="Run">
                <ClientSideEvents Click="showToastMessage" Init="Grid_EndCallback"/>
            </dx:ASPxButton>
            <asp:HiddenField ID="hdnDepartment" runat="server" />
    </div>

</div>
        <dx:ASPxGridView ID="gvPreview" runat="server" AutoGenerateColumns="false" CssClass="homeGrid" KeyFieldName="WorkItemID"
            Width="100%" OnDataBinding="gvPreview_DataBinding" ClientInstanceName="grid" EnableCallBacks="true" SettingsBehavior-SortMode="Custom"
            OnCustomSummaryCalculate="gvPreview_CustomSummaryCalculate" ClientVisible="false">
            <Columns>
            </Columns>
            <Templates>
                <StatusBar>
                    <div id="editControlBtnContainer" style="display: none;">
                        <asp:HyperLink ID="updateTask" runat="server" Text="Save Task Changes" CssClass="fleft updateTask savepaddingleft" OnClick="grid_BatchUpdate();">
                            <span class="alloTimeSave-gridBtn">
                                <b style="font-weight: normal;">Save Changes</b>
                            </span>
                                            </asp:HyperLink>
                                            <asp:HyperLink ID="cancelTask" runat="server" Style="padding: 10px 5px; float: right;" Text="Cancel Changes" CssClass="fleft" OnClick="grid_CancelBatchUpdate();">
                            <span class="alloTimeCancel-gridBtn">
                                <b style="font-weight: 600;">Cancel Changes</b>
                            </span>
                        </asp:HyperLink>
                    </div>
                </StatusBar>

            </Templates>

            <ClientSideEvents EndCallback="Grid_EndCallback" />
            <Styles>
                <Row CssClass="estReport-dataRow"></Row>
                <Header CssClass="gridHeader RMM-resourceUti-gridHeaderRow" />
                <GroupPanel CssClass="reportGrid-groupPannel"></GroupPanel>
                <GroupRow CssClass="gridGroupRow estReport-gridGroupRow" />
                <GroupFooter CssClass="estReport-groupFooterRow"></GroupFooter>
                <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px"
                    BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow">
                </Footer>
                <Table CssClass="timeline_gridview"></Table>
                <Cell Wrap="True"></Cell>
            </Styles>
            <SettingsDataSecurity AllowDelete="false" />
            <SettingsBehavior AllowGroup="true" AutoExpandAllGroups="true" />
            <SettingsLoadingPanel Mode="Disabled" />
            <SettingsPager PageSizeItemSettings-ShowAllItem="true"></SettingsPager>
            <Settings GroupFormat="{1}" ShowFooter="true" ShowStatusBar="Visible"
                VerticalScrollBarMode="Auto" HorizontalScrollBarMode="Auto" />
        </dx:ASPxGridView>
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvPreview" OnRenderBrick="gridExporter_RenderBrick" PreserveGroupRowStates="true">
        </dx:ASPxGridViewExporter>

    <div id="divAllocationGantt"></div>

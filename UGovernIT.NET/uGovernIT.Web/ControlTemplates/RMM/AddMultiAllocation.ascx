<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddMultiAllocation.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.AddMultiAllocation" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxdvItem_UGITNavyBlueDevEx, .dxdvFlowItem_UGITNavyBlueDevEx {
        border: 0px solid #a9acb5;
        background-color: White;
        padding: 0px;
        height: 0px;
    }

    .dxdvControl_UGITNavyBlueDevEx {
        font: 11px Verdana, Geneva, sans-serif;
        color: #201f35;
        border: 0px solid #9da0aa;
    }

    .floatright{
        float: right;
        padding: 5px;
    }

    .paddingright{
        padding-right: 10%;
    }

    .paddingtop{
        padding-top:10px;
    }

    .rowBackBgColor_0{
        background:#e4e3e3;
    }
    .rowBackBgColor_1{
        background:#F7D7DA;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var selectedItemIDs = [];
    $(document).ready(function () {
        selectedItemIDs = [];
        $(window).keydown(function (event) {
            if (event.keyCode == 13) {
                event.preventDefault();
                return false;
            }
        });

        $("#toastwarning").dxToast({
            message: "Please select atleast one record. ",
            type: "warning",
            displayTime: 1600,
            position: "{ my: 'center', at: 'center', of: window }"
        });
    });

    function btnAddNewItem_Click(s, e) {
        multiAllocationPanel.PerformCallback("Add");
    }

    function ddlLevel1_SelectedIndexChanged(s, e) {
        cbLevel2.PerformCallback();
    }

    function imgDelete_Click(s, e) {
        var input = s.GetMainElement();
        var oldValue = input.attributes["ItemId"];
        multiAllocationPanel.PerformCallback("Delete~" + oldValue.value);
    }

    function cbLevel2_SelectedIndexChanged(s, e) {
        var preconSDate = new Date(Date.parse('<%=preconStart%>'));
        var preconEDate = new Date(Date.parse('<%=preconEnd%>'));
        var constSDate = new Date(Date.parse('<%=constStart%>'));
        var constEDate = new Date(Date.parse('<%=constEnd%>'));
        var closeoutDate = new Date(Date.parse('<%=closeout%>'));
        if (e.isSelected) {
            switch (e.index) {
                case 0:
                    {
                        startDate.SetDate(preconSDate);
                        endDate.SetDate(preconEDate);
                    }
                    break;
                case 1:
                    {
                        startDate.SetDate(constSDate);
                        endDate.SetDate(constEDate);
                    }
                    break;
                case 2:
                    {
                        startDate.SetDate(constEDate);
                        endDate.SetDate(closeoutDate);
                    }
                    break;
            }
        }
    }

    function rbPrecon_Click(s, e) {
        if (selectedItemIDs.length > 0) 
            multiAllocationPanel.PerformCallback("PreconDate~" + selectedItemIDs.toString());
        else
            $("#toastwarning").dxToast("show");
    }

    function rbConstruction_Click(s, e) {
        if (selectedItemIDs.length > 0)
            multiAllocationPanel.PerformCallback("ConstDate~" + selectedItemIDs.toString());
        else
            $("#toastwarning").dxToast("show");
    }

    function rbCloseout_Click(s, e) {
        if (selectedItemIDs.length > 0)
            multiAllocationPanel.PerformCallback("CloseDate~" + selectedItemIDs.toString());
        else
            $("#toastwarning").dxToast("show");
    }

    function chkItemSelect_ValueChanged(s, e) {
     
        var input = s.GetMainElement();
        var itemId = input.attributes["ItemId"];
        if (s.GetValue() == true) {
            selectedItemIDs.push(itemId.value);
            $(input.closest("div.rowContainer")).css("background", "#D3DFFC");
        }
        else {
            $(input.closest("div.rowContainer")).css("background", "");
            var i = 0;
            while (i < selectedItemIDs.length) {
                if (selectedItemIDs[i] === itemId.value) {
                    selectedItemIDs.splice(i, 1);
                } else {
                    ++i;
                }
            }
        }
    }

    function dteStartDate_DateChanged(s, e) {
        var input = s.GetMainElement();
        var oldValue = input.attributes["ItemId"];
        multiAllocationPanel.PerformCallback("StartDate~" + oldValue.value + "~" + s.GetDate().toISOString());
    }
    function dteEndDate_DateChanged(s, e) {
        var input = s.GetMainElement();
        var oldValue = input.attributes["ItemId"];
        multiAllocationPanel.PerformCallback("EndDate~" + oldValue.value + "~" + s.GetDate().toISOString());
    }
    function cmbRoles_SelectedIndexChanged(s, e) {
        var input = s.GetMainElement();
        var oldValue = input.attributes["ItemId"];
        multiAllocationPanel.PerformCallback("Role~" + oldValue.value + "~" + s.GetValue());
    }
    function txtPctAllocation_LostFocus(s, e) {
        var input = s.GetMainElement();
        var oldValue = input.attributes["ItemId"];
        multiAllocationPanel.PerformCallback("PctAllocation~" + oldValue.value + "~" + s.GetText());
    }
    function validate_roles(s, e) {
        if (s.GetText() == "" || s.GetText() == null || typeof (s.GetText()) == "undefined") {
            e.IsValid = false;
            e.ErrorText = 'Role not correct.'
        }
    }
    function validate_pctallocation(s, e) {
     
        if (parseInt(s.GetText()) < 0) {
            e.IsValid = false;
            e.ErrorText = 'Value Too Low.';
        }
        else if (parseInt(s.GetText()) > 100) {
            e.IsValid = false;
            e.ErrorText = 'Value Too High.';
        }
        else {
            e.IsValid = true;
        }
    }
</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
</dx:ASPxLoadingPanel>
<div id="workitemrow" runat="server" class="ms-formtable row accomp-popup paddingtop">
    <div class="col-md-4 col-sm-4 col-xs-6" id="tr1" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Type<b style="color: Red;">*</b></h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxComboBox ID="ddlLevel1" runat="server" OnLoad="ddlLevel1_Load" AutoPostBack="true" OnSelectedIndexChanged="ddlLevel1_SelectedIndexChanged1"
                CssClass="comboBox-dropDown CRMDueDate_inputField">
                <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="save" ErrorText="Required" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
            </dx:ASPxComboBox>

        </div>
    </div>
    <div class="col-md-4 col-sm-7 col-xs-6" id="workitem" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Work Item<b style="color: Red;">*</b>
            </h3>

        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxComboBox ID="cbLevel2" ClientInstanceName="cbLevel2" OnLoad="cbLevel2_Load" runat="server" AutoPostBack="true" 
                DropDownStyle="DropDownList" ValueField="LevelTitle" TextField="LevelTitle" TextFormatString="{0}"
                ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                CallbackPageSize="10" CssClass="comboBox-dropDown CRMDueDate_inputField" PopupHorizontalAlign="RightSides"
                OnCallback="cbLevel2_Callback"
                OnSelectedIndexChanged="cbLevel2_SelectedIndexChanged"
                >
                <Columns></Columns>
                <ClientSideEvents EndCallback="function(s, e) {    cbLevel2.Refresh();   }" />
                <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="save" ErrorText="Required" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
            </dx:ASPxComboBox>
        </div>
    </div>
</div>
<div id="userrow" runat="server" class="ms-formtable row paddingtop">
    <div class="col-md-4 col-sm-5 col-xs-6">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">User<b style="color: Red;">*</b></h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxComboBox ID="cmbUsers" runat="server" OnLoad="cmbUsers_Load" CssClass="comboBox-dropDown CRMDueDate_inputField" Width="100%">
                <ValidationSettings RequiredField-IsRequired="true" ValidationGroup="save" ErrorText="Required" ErrorDisplayMode="ImageWithTooltip">
                </ValidationSettings>
            </dx:ASPxComboBox>
        </div>
    </div>
</div>
<div runat="server" id="DateOptions" class="row">
    <div class="col-md-4 col-sm-4 col-xs-4">
        <div class="ms-formlabel">
            <h4 class="ms-standardheader budget_fieldLabel"><dx:ASPxLabel ID="lblPrecon" runat="server" Font-Size="14px" EncodeHtml="false" Text="&nbsp;"></dx:ASPxLabel></h4>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxButton ID="btnPreconDate" ClientInstanceName="btnPreconDate" runat="server" Text="&nbsp;Select Precon Dates&nbsp;" AutoPostBack="false"
                CssClass="secondary-cancelBtn" >
                <ClientSideEvents Click="rbPrecon_Click" />
            </dx:ASPxButton>
           
        </div>
    </div>
    <div class="col-md-4 col-sm-4 col-xs-4">
        <div class="ms-formlabel">
            <h4 class="ms-standardheader budget_fieldLabel"><dx:ASPxLabel ID="lblConst" runat="server" Font-Size="14px" EncodeHtml="false" Text="&nbsp;"></dx:ASPxLabel></h4>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxButton ID="btnConstruction" ClientInstanceName="btnConstruction" runat="server" Text="&nbsp;Select Const. Dates&nbsp;" AutoPostBack="false"
                CssClass="secondary-cancelBtn" >
                <ClientSideEvents Click="rbConstruction_Click" />
            </dx:ASPxButton>
            
        </div>
    </div>
    <div class="col-md-4 col-sm-4 col-xs-4">
        <div class="ms-formlabel">
            <h4 class="ms-standardheader budget_fieldLabel"><dx:ASPxLabel ID="lblCloseout" runat="server" Font-Size="14px" EncodeHtml="false" Text="&nbsp;"></dx:ASPxLabel></h4>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxButton ID="btnCloseout" runat="server" ClientInstanceName="btnCloseout" Text="&nbsp;Select Closeout Dates&nbsp;" AutoPostBack="false" 
                CssClass="secondary-cancelBtn" >
                <ClientSideEvents Click="rbCloseout_Click" />
            </dx:ASPxButton>
           
                      
        </div>     
    </div>
    
</div>
<div class="row">
    <div class="floatright">
                 <dx:ASPxButton ID="btnAddNewItem" runat="server" ClientInstanceName="btnAddNewItem" Text="Add New" AutoPostBack="false" CssClass="primary-blueBtn">
                        <ClientSideEvents Click="btnAddNewItem_Click" />
                    </dx:ASPxButton>
            </div> 
</div>
<div class="row">
    

    <div>
        <dx:ASPxDataView ID="multiAllocationPanel" ClientInstanceName="multiAllocationPanel" runat="server" Width="100%" PagerAlign="Justify" ItemSpacing="0px"
            Layout="Table" SettingsTableLayout-ColumnCount="1" SettingsBreakpointsLayout-ItemsPerRow="1" SettingsTableLayout-RowsPerPage="100"
            OnCustomCallback="ASPxDataView1_CustomCallback" Height="0px" EnableViewState="false">
            <ItemTemplate>

                <div class="d-flex justify-content-center rowContainer rowBackBgColor_<%# Eval("IsDuplicate") %>">
                    <div class="px-1 py-2" id="Div1" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">&nbsp;</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <dx:ASPxCheckBox ID="chkItemSelect" runat="server" ItemId='<%# Eval("Id") %>' AutoPostBack="false" >
                                <ClientSideEvents ValueChanged="chkItemSelect_ValueChanged" />
                            </dx:ASPxCheckBox>
                        </div>
                    </div>
                    <div class="p-1" id="tr1" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Start Date<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <dx:ASPxDateEdit ID="dteStartDate" runat="server" DateOnly="true" DisplayFormatString="MMM d, yyyy" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                                Date='<%# Eval("StartDate") %>' ItemId='<%# Eval("Id") %>' CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit"
                                 CssClass="CRMDueDate_inputField dateEdit-dropDown" DropDownButton-Image-Width="18px" 
                                CalendarProperties-FooterStyle-CssClass="calenderFooterWrap">
                                <ClientSideEvents  DateChanged="dteStartDate_DateChanged" />
                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Required" ValidationGroup="save" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                            </dx:ASPxDateEdit>
                        </div>
                    </div>
                    <div class="p-1">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">End Date<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <dx:ASPxDateEdit ID="dteEndDate" runat="server" DateOnly="true" DisplayFormatString="MMM d, yyyy" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                                Date='<%# Eval("EndDate") %>' ItemId='<%# Eval("Id") %>' CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit"
                                 CssClass="CRMDueDate_inputField dateEdit-dropDown" DropDownButton-Image-Width="18px" 
                                CalendarProperties-FooterStyle-CssClass="calenderFooterWrap">
                                <ClientSideEvents  DateChanged="dteEndDate_DateChanged" />
                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Required" ValidationGroup="save" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                            </dx:ASPxDateEdit>
                        </div>
                    </div>
                    
                    <div class="p-1" style="width:40%;">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Role<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <dx:ASPxComboBox ID="cmbRoles" runat="server" CssClass="comboBox-dropDown CRMDueDate_inputField" PopupHorizontalAlign="RightSides"
                                 OnLoad="cmbRoles_Load" OnInit="cmbRoles_Init" Width="100%" >
                                <ClientSideEvents SelectedIndexChanged="cmbRoles_SelectedIndexChanged" Validation="validate_roles" />
                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Required" ValidationGroup="save" ErrorDisplayMode="ImageWithTooltip"></ValidationSettings>
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="p-1" style="width:10%;">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">% Allocation<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField" >
                            <dx:ASPxTextBox ID="txtPctAllocation" runat="server" CssClass="CRMDueDate_inputField" ItemId='<%# Eval("Id") %>' 
                                Text='<%# Eval("PctAllocation") %>'>
                                <ClientSideEvents LostFocus="txtPctAllocation_LostFocus" Validation="validate_pctallocation" />
                                <ValidationSettings RequiredField-IsRequired="true" RequiredField-ErrorText="Required" 
                                    RegularExpression-ValidationExpression="^[1-9][0-9]?$|^100$" RegularExpression-ErrorText="Incorrect." ErrorDisplayMode="ImageWithTooltip"
                                    ValidationGroup="save">
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </div>
                    </div>
                    <div class="p-1">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">&nbsp;</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField" style="padding-top:10px;">
                            <dx:ASPxImage ID="imgDelete" runat="server" ClientInstanceName="imgDelete" ImageUrl="/content/images/redNew_delete.png"
                                ItemId='<%# Eval("Id") %>'>
                                <ClientSideEvents Click="imgDelete_Click" />
                            </dx:ASPxImage>
                        </div>
                    </div>
                </div>

            </ItemTemplate>
            <ClientSideEvents EndCallback="function(s, e) { pnlError.SetVisible(false); selectedItemIDs = [];   }" /> 
        </dx:ASPxDataView>
    </div>

</div>
<div class="row">
    <div class="floatright">
        <dx:ASPxButton ID="btnSave" runat="server" Text="Save Allocations" CssClass="primary-blueBtn" OnClick="btnSave_Click"  ValidationGroup="save">
            <ClientSideEvents Click="function(s, e){ if (ASPxClientEdit.ValidateGroup('save')){ LoadingPanel.Show(); } }" />
        </dx:ASPxButton>
    </div>
    <div class="floatright">
        <dx:ASPxButton ID="btnCancel" ClientInstanceName="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" 
             OnClick="btnCancel_Click">            
        </dx:ASPxButton>
    </div>
</div>
<div class="row">
    <dx:ASPxPanel ID="pnlError" ClientInstanceName="pnlError" runat="server" ClientVisible="false">
        <PanelCollection>
            <dx:PanelContent>
                <div class="alert alert-danger" role="alert" >
                    Some Allocations are Overlapping. Please Delete or Update Existing.
                </div>
            </dx:PanelContent>
        </PanelCollection>       
    </dx:ASPxPanel>
    
</div>
<div id="toastwarning"></div>
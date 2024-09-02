<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddWorkItems.ascx.cs" Inherits="uGovernIT.Web.AddWorkItems" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">

    .CRMDueDate_inputField tr td {
        padding: 3px 8px 3px 4px !important;
    }

    .ms-formlabel {
        text-align: right;
        width: 170px;
        vertical-align: top;
        padding-top: 7px;
    }
    /*.ms-standardheader 
    {
        text-align: right;
        padding-left: 9px;
    }*/
    .ms-standardheader .lblsubitem {
        font: inherit;
    }

    .width25 {
        width: 25px;
        text-align: right
    }

    .rightpos {
        /*float:right;*/
    }

    .allocationmargin {
        /*margin-left: 10px;*/
    }

    .dxeEditAreaSys {
        color: inherit;
    }

    .dxeCalendarFooter_UGITNavyBlueDevEx {
        padding-left: 67px;
    }

    body input.dxeEditArea_UGITNavyBlueDevEx {
        color: inherit;
    }
    .dxICheckBox_CustomImage {
        margin: 1px;
    }
    .dxICBFocused_CustomImage {
        margin: 0px;
        border: 1px dotted Orange;
    }
    .budget_fieldLabel {
        color: black;
    }
    .accomp_inputField {
        color:black;
    }
    input[type=password][disabled], input[type=text][disabled], input[type=file][disabled], textarea[disabled], select[disabled], .sp-peoplepicker-topLevelDisabled, .ms-inputBoxDisabled {
    background-color: #f2f2f2 !important;
    }

</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var hidePctRow = "<%=Request["WorkItemType"]%>" == "Time Off" && "<%=useGanttDayFormat%>".toLowerCase() == "true" ? true : false;
    function ShowLoadingPanel() {
        LoadingPanel.Show();
        return true;
    }

    function endDateChange(s, e) {
        var constStartDate = startDate.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }

        var constEndDate = endDate.GetDate();
        if (constEndDate != null && constEndDate != "" && constEndDate != undefined) {
            constEndDate = constEndDate.format('MM/dd/yyyy');
        }

        if (typeof (txtCPRDuration) != "undefined" && constStartDate != null && constStartDate != "" && constEndDate != null && constEndDate != "") {

            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","endDate":"' + constEndDate + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelper %>/GetDurationInWeeks",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        txtCPRDuration.SetText(resultJson.duration);
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);
                }
            });
        }
        //// e.processOnServer = false;

    }

    function startDateChange(s, e) {
        var constStartDate = startDate.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }

        if (typeof (txtCPRDuration) != "undefined") {
            var noOfWeeks = txtCPRDuration.GetText();   // $('.CRMDurationClass').val();

            if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {

                var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelper %>/GetEndDateByWeeks",
                    data: paramsInJson,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (message) {
                        var resultJson = $.parseJSON(message.d);
                        if (resultJson.messagecode == 2) {

                            endDate.SetText(resultJson.enddate);
                        }
                        else {

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }
                });
            }
        }
        //// e.processOnServer = false;

    }

    function changeEndateOnDuration(s, e) {

        var constStartDate = startDate.GetDate();
        constStartDate = constStartDate.format('MM/dd/yyyy');
        var noOfWeeks = txtCPRDuration.GetText();
        noOfWeeks = Math.ceil(noOfWeeks);

        if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {
            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';

            $.ajax({
                type: "POST",
                url: "/Layouts/uGovernIT/ajaxhelper.aspx/GetEndDateByWeeksProject",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        //console.log(message);

                        endDate.SetDate(new Date(resultJson.enddate));
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);

                }
            });
        }
    }

    $(document).ready(function () {
        $('.AddWorkItemOverflow').parent().addClass("popupUpAddWorkItem");
        if (hidePctRow) {
            $(".hidePctRow").hide();
        }
    });

    function updateCheckBoxState(s, e) {
        //debugger;
        var checkState = s.GetCheckState();
        var checked = s.GetChecked();
        var newCheckState = rdbSoftAllocation.GetChecked() ? "Soft" : "Hard";
        lblSoftAllocation.SetText(newCheckState);
    }

    function updateNCOState(s, e) {

    }
</script>
<div style="width: 100%;" class="AddWorkItemOverflow">
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True" Image-Url="~/Content/IMAGES/AjaxLoader.gif" ImagePosition="Top" Text="Loading.." CssClass="customeLoader">
    </dx:ASPxLoadingPanel>
    <div class="ms-formtable row accomp-popup">
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlLevel1" runat="server" OnLoad="FillDropDownLevel1" AutoPostBack="true" CssClass="aspxDropDownList" OnSelectedIndexChanged="FillDropDownLevel2">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="workitem" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Work Item<b style="color: Red;">*</b>
                </h3>

            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="cbLevel2" OnLoad="cbLevel2_Load" runat="server" AutoPostBack="true" OnSelectedIndexChanged="FillDropDownLevel3"
                    DropDownStyle="DropDownList" ValueField="LevelTitle" TextField="LevelTitle" TextFormatString="{0}"
                    ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                    CallbackPageSize="10" CssClass="comboBox-dropDown CRMDueDate_inputField" PopupHorizontalAlign="RightSides">
                    <Columns></Columns>
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="subitem" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">
                    <dx:ASPxLabel ID="lblsubitem" runat="server" Text="Sub Item" CssClass="lblsubitem" Visible="false"></dx:ASPxLabel>
                    <b id="lbl01" runat="server" style="color: Red;" visible="false">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="cbLevel3" runat="server" Width="200"
                    DropDownStyle="DropDownList" TextFormatString="{0}" EncodeHtml="false" Visible="false"
                    ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True" CallbackPageSize="10" CssClass="comboBox-dropDown CRMDueDate_inputField"
                    PopupHorizontalAlign="RightSides" >
                    <Columns></Columns>
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="subSubitem" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">
                    <dx:ASPxLabel ID="lblsubsubitem" runat="server" Text="Sub Sub Item" CssClass="lblsubitem" Visible="false"></dx:ASPxLabel>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="cbLevel4" runat="server" Width="200"
                    DropDownStyle="DropDownList" TextFormatString="{0}" EncodeHtml="false" Visible="false"
                    ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True" CallbackPageSize="10" CssClass="comboBox-dropDown CRMDueDate_inputField"
                    PopupHorizontalAlign="RightSides" >
                    <Columns></Columns>
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trSubItem" runat="server" visible="false">
            <div class="ms-formlabel allocationmargin">
                <h3 class="ms-standardheader budget_fieldLabel">Sub Project </h3>
            </div>
            <div class="ms-formbody accomp_inputField allocationmargin">
                <asp:TextBox ID="txtSubProject" runat="server" Width="265px" />
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding hidePctRow" id="trAllocation" runat="server">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel ">Allocation <span>%</span><b style="color: Red;">*</b>
                    </h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAllocation" runat="server" Text='' />             
                </div>
            </div>
           <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
               <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">&nbsp;&nbsp;</h3>
               </div>
               <div class="ms-formbody accomp_inputField">
                <dx:ASPxCheckBox ID="rdbSoftAllocation" ClientInstanceName="rdbSoftAllocation" runat="server" Checked="false" 
                    ToggleSwitchDisplayMode="Always" Theme="iOS" >
                    <ClientSideEvents ValueChanged="updateCheckBoxState" Init="updateCheckBoxState" />
                </dx:ASPxCheckBox>
                <dx:ASPxLabel ID="lblSoftAllocation" ClientInstanceName="lblSoftAllocation" runat="server" Font-Bold="true" Font-Size="14px" Text="Hard"></dx:ASPxLabel>
                
                <dx:ASPxCheckBox ID="rdbNonChargeable" runat="server" ClientInstanceName="rdbNonChargeable" Checked="false" ToggleSwitchDisplayMode="Always" Theme="iOS">
                    <ClientSideEvents ValueChanged="updateNCOState" Init="updateNCOState" />
                </dx:ASPxCheckBox>
                   <dx:ASPxLabel ID="lblNCO" ClientInstanceName="lblNCO" runat="server" Font-Bold="true" Font-Size="14px" Text="NCO"></dx:ASPxLabel>
            </div>
           </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trStartDate" runat="server" style="clear:both">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Start Date <b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxDateEdit OnValueChangeClientScript="dateChanged()" DateOnly="true" DisplayFormatString="MMM d, yyyy" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                    ID="startDate" ClientInstanceName="startDate" runat="server" CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit"
                    CssClass="CRMDueDate_inputField dateEdit-dropDown" DropDownButton-Image-Width="18px" CalendarProperties-FooterStyle-CssClass="calenderFooterWrap">
                    <ClientSideEvents DateChanged="startDateChange" />
                </dx:ASPxDateEdit>
                <asp:Label ID="lbstartDate" runat="server" Visible="false"></asp:Label>

            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trCPRDuration" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Duration<span>(Weeks)</span></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxTextBox ID="txtCPRDuration" ClientInstanceName="txtCPRDuration" runat="server" CssClass="CRMDueDate_inputField">
                    <ClientSideEvents TextChanged="changeEndateOnDuration" />
                </dx:ASPxTextBox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding " id="trEndDate" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">End Date<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxDateEdit OnValueChangeClientScript="dateChanged()" DateOnly="true" DisplayFormatString="MMM d, yyyy" DropDownButton-Image-Url="~/Content/Images/calendarNew.png"
                    ID="endDate" ClientInstanceName="endDate" runat="server" CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit" 
                    CssClass="CRMDueDate_inputField  dateEdit-dropDown" DropDownButton-Image-Width="18px" CalendarProperties-FooterStyle-CssClass="calenderFooterWrap">
                    <ClientSideEvents DateChanged="endDateChange" />
                </dx:ASPxDateEdit>
                <asp:Label ID="lbendDate" runat="server" Visible="false"></asp:Label>

            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding " id="Div1" runat="server">
            <div id="divSelectDates" runat="server" class="ms-formlabel" visible="false">
                <h3 class="ms-standardheader budget_fieldLabel">Select Any Saved Dates</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxCheckBoxList ID="chkPreconDates" runat="server" ClientVisible="false" Width="100%" Font-Size="16px" >
                    <ClientSideEvents SelectedIndexChanged="chkPreconDates_SelectedIndexChanged" />
                    
                </dx:ASPxCheckBoxList>
            </div>
         </div>
        
        <div class="row fieldWrap" id="tr4" runat="server">
            <div class="ms-formlabel errorMsg-wrap">
                <asp:Label ID="lbMessage" runat="server" Text="" Visible="true" CssClass="error-msg" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn" ></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" CssClass="primary-blueBtn" OnClick="btnSave_Click">
                <ClientSideEvents Click="function(s, e){ShowLoadingPanel()}" />
            </dx:ASPxButton>
        </div>
    </div>
</div>

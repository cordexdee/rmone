<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleResourceAddEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleResourceAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formlabel {
        width: 160px;
        text-align: right;
    }

    .full-width {
        width: 90%;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .text-error {
        color: #a94442;
        font-weight: 500;
        margin-top: 5px;
    }

    div.ms-inputuserfield {
        height: 17px;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/Content/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .required-item:after {
        content: '* ';
        color: red;
        font-weight: bold;
    }

    .hide {
        display: none;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function OnCallbackComplete(s, e) {
        debugger;
        var strFeedID = e.result.toString();
        var mybutton = document.getElementById("<%=lnkTotalSkillFTECount.ClientID %>");
        mybutton.innerHTML = strFeedID;
        return false;
    }
    //Compare the start date and End Date of without recurrance patter
    function validateStartDateAndEndDate(source, args) {

        var StartDate = Date.parse(document.getElementById('<%=dtcStartDate.Controls[0].ClientID%>').value);
        var EndDate = Date.parse(document.getElementById('<%=dtcEndDate.Controls[0].ClientID%>').value);

        if (StartDate <= EndDate) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }
    function FillOnUserFieldChange(obj) {

        $("#<%=btnRefresh.ClientID %>").get(0).click();
    }
    $(document).ready(function () {

        $("#<%=chkbxCreatebudget.ClientID%>").bind("change", function () {

            var chkbcCreateBudget = $("#<%=chkbxCreatebudget.ClientID%>");
            if ($(chkbcCreateBudget).is(":checked")) {
                $('.divDDLBudgetCategories').show();
            }
            else
                $('.divDDLBudgetCategories').hide();
        });
        $("#<%=txtFTEs.ClientID %>").bind("blur", function () {
            var fte = parseFloat($("#<%=txtFTEs.ClientID %>").val());
            var hourlyRate = parseFloat(txtResourceHourlyRate.GetValue());
            var workingHours = 8;
            var endDate = dtcEndDate.GetDate();
            var startDate = dtcStartDate.GetDate();
            var daysDiff = showDays(startDate, endDate);
            NoOfWorkingDays = parseFloat(daysDiff);
            calculateBudget(fte, hourlyRate, workingHours, NoOfWorkingDays);
        });


    });
    var NoOfWorkingDays = 0;
    function calculateBudget(fte, hourlyRate, workingHours, NoOfWorkingDays) {

        if (fte > 0) {
            $("#<%=txtHrs.ClientID%>").val(Math.floor(fte * workingHours * NoOfWorkingDays));
            if (fte != 0 && hourlyRate != 0 && workingHours != 0 && NoOfWorkingDays != 0) {
                lblTotalResourceBudget.SetValue(fte * hourlyRate * workingHours * NoOfWorkingDays);
            }
            else {
                lblTotalResourceBudget.SetValue(0);
            }
        }
    }


    function showDays(firstDate, secondDate) {
        var startDate = new Date(firstDate);
        var endDate = new Date(secondDate);

        //var millisecondsPerDay = 1000 * 60 * 60 * 24;

        //var millisBetween = endDay.getTime() - startDay.getTime();
        //var days = (millisBetween / millisecondsPerDay) + 1;
        var currentDate = startDate;
        //// Round down.
        var count = 0;
        while (currentDate <= endDate) {
            var dayOfWeek = currentDate.getDay();
            if (!((dayOfWeek == 6) || (dayOfWeek == 0)))
                count++;
            currentDate.setDate(currentDate.getDate() + 1);
        }
        return count;
    }
    function onDateChange() {

        var endDate = dtcEndDate.GetDate();
        var startDate = dtcStartDate.GetDate();
        var daysDiff = showDays(startDate, endDate);
        NoOfWorkingDays = parseInt(daysDiff);
        var fte = parseInt($("#<%=txtFTEs.ClientID %>").val());
        var hourlyRate = parseInt(txtResourceHourlyRate.GetValue());
        //var workingHours = parseInt('WorkingHours %>');
        calculateBudget(fte, hourlyRate, 8, NoOfWorkingDays);
    }

    function price_InitAndKeyUp(s, e) {
        var fte = parseInt($("#<%=txtFTEs.ClientID %>").val());
        var hourlyRate = parseInt(s.GetValue());
        // var workingHours = parseInt('WorkingHours %>');
        // var endDate = $('.endDateTimeCtr').get(0).value;
        //var startDate = $('.startDateTimeCtr').get(0).value;
        // var daysDiff = showDays(startDate, endDate);
        // NoOfWorkingDays = parseInt(daysDiff);
        calculateBudget(fte, hourlyRate, 8, 2);
    }
</script>
<!--- js for grid responsive ------>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        gridAllocation.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gridAllocation.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });  
</script>
           

<asp:Button ID="btnRefresh" CssClass="hide" runat="server" OnClick="btnRefresh_Click" ValidationGroup="refresh" />
<dx:ASPxLoadingPanel ID="aspxAssignToLoading" ClientInstanceName="aspxAssignToLoading" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>

<fieldset>
    <legend class="addResource_title">Resources</legend>

    <div class="accomp-popup" width: 100%">
        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Role</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="ddlRoles" runat="server" FieldName="RoleNameChoice" IsMulti="false" CssClass="lookupValueBox-dropown"/>
                </div>
            </div>
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Staff Type<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:LookUpValueBox ID="cbCategory" runat="server" FieldName="BudgetTypeChoice" CssClass="lookupValueBox-dropown"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="AddResource" ControlToValidate="cbCategory"
                        Display="Dynamic" InitialValue="" CssClass="text-error">
                        <span>Please select a Staff Type.</span>
                    </asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Skill<i class="required-item"></i>
                        <asp:ImageButton ID="btnSkill" runat="server" ImageUrl="/Content/Images/Autocalculater.png" ToolTip="Choose Resource" Height="16px"
                            OnClick="btnSkill_Click" />
                    </h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                     <dx:ASPxCallback ID="cbSkillcallback" runat="server" ClientInstanceName="cbSkillcallback" OnCallback="cbSkillcallback_Callback">
                                                        <ClientSideEvents CallbackComplete="OnCallbackComplete" />
                                                    </dx:ASPxCallback>
                   <%-- <ugit:LookUpValueBox ID="cbSkill" CssClass="lookupValueBox-dropown" runat="server"  ClientInstanceName="cbSkill"  FieldName="UserSkillLookup" AllowNull="true"/>--%>
                    <dx:ASPxComboBox ID="cbSkill" ClientInstanceName="cbSkill" CssClass="lookupValueBox-dropown" runat="server" AutoPostBack="false">
                        <Items>
                                    <dx:ListEditItem Text="None" Value="None" Selected="true" />
                                </Items>
                         <ClientSideEvents SelectedIndexChanged="function(s,e){
                             debugger;
                             cbSkillcallback.PerformCallback();
                             }" />
                    </dx:ASPxComboBox>
                    <asp:RequiredFieldValidator ID="rfvSkill" runat="server" ValidationGroup="AddResource" ControlToValidate="cbSkill"
                        Display="Dynamic" InitialValue="" CssClass="text-error">
                        <span>Please select a Skill.</span>
                    </asp:RequiredFieldValidator>
                </div>
             </div>
            <div class="col-sm-6 col-xs-12">
                 <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Responsiblities</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" CssClass="addResouce_noteField aspTextBox" TextMode="MultiLine" Height="45px" runat="server"></asp:TextBox>
                </div>
             </div>
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Start Date<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                     <dx:ASPxDateEdit ID="dtcStartDate" ClientInstanceName="dtcStartDate" runat="server" CssClass="CRMDueDate_inputField"
                         DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"></dx:ASPxDateEdit>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="AddResource"
                        ControlToValidate="dtcStartDate$dtcStartDateDate" Display="Dynamic" CssClass="text-error">
                        <span>Please enter Start Date.</span>
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="AddResource"
                        ControlToValidate="dtcStartDate$dtcStartDateDate" Display="Dynamic" CssClass="text-error"
                        Type="Date" Operator="DataTypeCheck">
                        <span>Please enter a valid Start Date.</span>
                    </asp:CompareValidator>
                </div>
            </div>
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">End Date<i class="required-item"></i></h3>
                </div>
                 <div class="ms-formbody accomp_inputField">
                     <dx:ASPxDateEdit ID="dtcEndDate" ClientInstanceName="dtcEndDate" runat="server" CssClass="CRMDueDate_inputField" 
                         DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16"></dx:ASPxDateEdit>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="AddResource" ControlToValidate="dtcEndDate$dtcEndDateDate"
                        Display="Dynamic" CssClass="text-error">
                        <span>Please enter End Date.</span>
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="AddResource"
                        ControlToValidate="dtcEndDate$dtcEndDateDate" Display="Dynamic" CssClass="text-error"
                        Type="Date" Operator="DataTypeCheck">
                        <span>Please enter a valid End Date.</span>
                    </asp:CompareValidator>
                </div>
            </div>
        </div>
       
       
        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Requested Resource(s)</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                     <ugit:UserValueBox ID="peUsers" runat="server" OnLoad="peUsers_OnLoad" SelectionSet="User" isMulti="true" CssClass="userValueBox-dropDown"/>
                    <dx:ASPxPopupControl ID="grdSkill" runat="server" CloseAction="CloseButton" Modal="true"
                        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ScrollBars="Vertical" 
                        ShowFooter="false" Width="570px" Height="420px" HeaderText="Resource(s) with Skills:" ClientInstanceName="grdSkill" 
                        CssClass="resourceSkill_popup aspxPopup">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                    <div class="row">
                                         <ugit:ASPxGridView ID="gridAllocation" runat="server" AutoGenerateColumns="False"
                                            OnDataBinding="gridAllocation_DataBinding" ClientInstanceName="gridAllocation"
                                            Width="100%" KeyFieldName="ResourceId"  OnCustomColumnDisplayText="gridAllocation_CustomColumnDisplayText"
                                            SettingsText-EmptyDataRow="No Resources Available" CssClass="customgridview homeGrid">
                                             <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0"></dx:GridViewCommandColumn>
                                                <dx:GridViewDataTextColumn PropertiesTextEdit-EncodeHtml="false" Caption="Resource" FieldName="Resource" ReadOnly="True"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Skill" FieldName="UserSkillLookup" ReadOnly="True"></dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn PropertiesTextEdit-EncodeHtml="false" Caption="Total Allocation" FieldName="FullAllocation" ReadOnly="True"></dx:GridViewDataTextColumn>
                                            </Columns>
                                            <settingscommandbutton>
                                                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                            </settingscommandbutton>
                                            <Styles AlternatingRow-CssClass="ugitlight1lightest">
                                                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                                                <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                                                <Row CssClass="homeGrid-groupRow"></Row>
                                            </Styles>
                                            <Settings GroupFormat="{1}" />
                                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                            <SettingsEditing Mode="Inline" />
                                            <SettingsBehavior ConfirmDelete="true" AllowDragDrop="false" AutoExpandAllGroups="true" ColumnResizeMode="Disabled" />
                                            <SettingsPopup HeaderFilter-Height="200"></SettingsPopup>
                                        </ugit:ASPxGridView>
                                    </div>
                                   
                                     <script type="text/javascript">
                                         ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                             UpdateGridHeight();
                                         });
                                         ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                             UpdateGridHeight();
                                         });
                                     </script>
                                    <br />
                                    <asp:Label ID="lblSkillErrorMessage" runat="server" Text="Please select at least one resource." ForeColor="Red" Visible="false"></asp:Label>

                                    <br />
                                    <div class="row addEditPopup-btnWrap">
                                        <div class="fixed-popupBtnWrap">
                                            <dx:ASPxButton ID="btCancelResourceSkill" runat="server" ClientInstanceName="btCancelResourceSkill" Text="Cancel" ToolTip="Cancel"
                                                CausesValidation="false" CssClass="ugitbutton resourceSkill_cancel secondary-cancelBtn">
                                                <ClientSideEvents Click="function(s, e) { grdSkill.Hide(); }" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton ID="dxUpdateSkill" Text="Add" runat="server" ClientInstanceName="dxUpdateSkill" OnClick="dxUpdateSkill_Click"
                                                    CssClass="ugitbutton resourceSkill_add primary-blueBtn" ForeColor="White">
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>

                            </dx:PopupControlContentControl>
                        </ContentCollection>

                    </dx:ASPxPopupControl>
                </div>
            </div>
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">FTEs<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    
                    <span style="width:100%">
                        <asp:TextBox ID="txtFTEs" runat="server" Text="0" CssClass="aspTextBox"></asp:TextBox>&nbsp;
                        
                        <asp:LinkButton ID="lnkTotalSkillFTECount" runat="server"  OnClick="lnkTotalSkillFTECount_Click"></asp:LinkButton>
                        FTEs Available
                           
                        <asp:RequiredFieldValidator ID="rfvtxtFTEs" runat="server" ValidationGroup="AddResource" ControlToValidate="txtFTEs"
                            Display="Dynamic" CssClass="text-error">
                        <span><br />Please enter FTEs.</span>
                                </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revtxtFTEs" runat="server" ValidationGroup="AddResource" ControlToValidate="txtFTEs"
                            Display="Dynamic" CssClass="text-error" ValidationExpression="^(?:\d{1,2})?(?:\.\d{1,2})?$">
                        <span><br />Please enter valid FTEs.</span>
                                </asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Estimated Hours</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtHrs" runat="server" CssClass="aspTextBox"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="AddResource" ControlToValidate="txtHrs"
                        Display="Dynamic" CssClass="text-error" ValidationExpression="\d+(\.\d{1,2})?">
                        <span><br />Please enter valid Estimated Hours.</span>
                    </asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="col-sm-6 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Notes</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtNotes" CssClass="addResouce_noteField" runat="server" TextMode="MultiLine" Height="40px"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row newResouces_budgetItemWrap">
            <div class="col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formbody accomp_inputField crm-checkWrap" style="width:100%;">
                <asp:CheckBox ID="chkbxCreatebudget" runat="server" Text="Create Budget Item" CssClass="" />
                <div class="row">
                    <fieldset id="divDDLBudgetCategories" runat="server" style="margin-top:15px; width: 100%; float: left; display: none" class="divDDLBudgetCategories">
                        <legend class="addResource_title">Budget Item</legend>
                        <div class="accomp-popup">
                            <div class="col-md-6 col-sm-6 col-xs-12 noLeftPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Hourly Rate:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxTextBox ID="txtResourceHourlyRate" ClientInstanceName="txtResourceHourlyRate" width="100%" runat="server" Text="0" CssClass="asptextBox-input">
                                        <MaskSettings Mask="$<0..99999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                        <ClientSideEvents Init="price_InitAndKeyUp" KeyUp="price_InitAndKeyUp" />
                                    </dx:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12 noLeftPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Budget Amount:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                     <dx:ASPxTextBox ID="lblTotalResourceBudget" width="100%" CssClass="asptextBox-input" ClientInstanceName="lblTotalResourceBudget" runat="server" Text="0">
                                        <MaskSettings Mask="$<0..9999999g>.<00..99>" IncludeLiterals="DecimalSymbol" />
                                    </dx:ASPxTextBox>
                                </div>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12 noLeftPadding">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Category: </h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                     <asp:DropDownList ID="ddlBudgetCategories" CssClass="itsmDropDownList aspxDropDownList" ValidationGroup="formABudget"
                                         runat="server"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            </div>
            
        </div>
        <div class="row addEditPopup-btnWrap" id="tr2" runat="server">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
             <dx:ASPxButton ID="addNewResourceItem" runat="server" Text="Save" ToolTip="Save" ValidationGroup="AddResource" OnClick="addNewResourceItem_Click"
                 CssClass="primary-blueBtn"></dx:ASPxButton>
            <dx:ASPxButton ID="updateResourceItem" runat="server" Text="Update" ToolTip="Save" ValidationGroup="AddResource" OnClick="updateResourceItem_Click" 
                CssClass="primary-blueBtn"></dx:ASPxButton>
        </div>
    </div>

</fieldset>
<asp:HiddenField ID="hdnFTESkill" runat="server" />

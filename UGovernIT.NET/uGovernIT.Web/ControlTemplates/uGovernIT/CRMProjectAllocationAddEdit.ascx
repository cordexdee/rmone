<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMProjectAllocationAddEdit.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.CRMProjectAllocationAddEdit" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.ms-formtable {
        border-collapse: collapse;
        width: 100%;
    }*/

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .text-error {
        color: red;
        font-weight: 500;
        margin-top: 5px;
    }

    .dxeButtonEdit.full-width {
        width: 94%;
    }

    .full-width {
        width: 98%;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/_layouts/15/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }

    .required-item:after {
        content: '* ';
        color: red;
        font-weight: bold;
    }

    .ms-dlgFrameContainer {
        width: 100%;
    }

    .auto-icon {
        width: 7%;
        float: left;
        text-align: left;
        vertical-align: middle;
        padding-left: 5px;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">


    function endDateChange(s, e) {
        var constStartDate = startDate.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }

        var constEndDate = endDate.GetDate();
        if (constEndDate != null && constEndDate != "" && constEndDate != undefined) {
            constEndDate = constEndDate.format('MM/dd/yyyy');
        }

        if (constStartDate != null && constStartDate != "" && constEndDate != null && constEndDate != "") {

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

                        $('.CRMDurationClass').val(resultJson.duration);
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
        var noOfWeeks = $('.CRMDurationClass').val();
        noOfWeeks = Math.ceil(noOfWeeks);
        if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {

            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelper %>/GetEndDateByWeeksProject",
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
                    //alert(thrownError);

                }
            });
        }

        //// e.processOnServer = false;

    }





    $(function () {


        $('.CRMDurationClass,.startDateEdit').bind('blur', function (event) {

            console.log("onload");
            var constStartDate = startDate.GetDate();
            constStartDate = constStartDate.format('MM/dd/yyyy');
            var noOfWeeks = $('.CRMDurationClass').val();
            noOfWeeks = Math.ceil(noOfWeeks);
            if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {

                var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelper %>/GetEndDateByWeeksProject",
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
                            //alert(thrownError);

                        }
                    });
                }
            });



            $('#endDate').bind('blur', function (event) {
                console.log("inside");
                ////var constStartDate = $('.startDateEdit').val();
                var constStartDate = startDate.GetDate();
                constStartDate = constStartDate.format('MM/dd/yyyy')
                ////var constEndDate = $('.endDateEdit').val();
                var constEndDate = endDate.GetDate();
                constEndDate = constEndDate.format('MM/dd/yyyy')

                if (constStartDate != null && constStartDate != "" && constEndDate != null && constEndDate != "") {

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


                            $('.CRMDurationClass').val(resultJson.duration);
                        }
                        else {

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        //alert(thrownError);
                    }
                });
            }
        });




        $("[data-tooltip]").each(function (i, e) {
            var tag = $(e);
            if (tag.is("[title]") === false) {
                tag.attr("title", "");
            }
        });

        $(document).tooltip({
            items: "[data-tooltip]",
            content: function () {
                return $(this).attr("data-tooltip");
            }
        });
    });


    function OpenAutoAllocationPopup(obj) {
        if ($(".startDateEdit").val().length <= 0 && $(".endDateEdit").val().length <= 0) {
            var today_date = new Date($.now());
            var end_date = new Date();
            $(".startDateEdit").val(convert(today_date));
            //var today_date = new Date()
            end_date.setDate(today_date.getDate() + 1);
            $(".endDateEdit").val(convert(end_date));
        }
        else if ($(".endDateEdit").val().length <= 0) {
            var start_Date = new Date(Date.parse($(".startDateEdit").val(), "MM/dd/yyyy"));
            var end_date = new Date(start_Date);
            end_date.setDate(start_Date.getDate() + 1);
            $(".endDateEdit").val(convert(end_date));
        }
        var params = "&pStartDate=" + $(".startDateEdit").val() + "&pEndDate=" + $(".endDateEdit").val() + "&pGlobalRoleID=" + ddlUserType.GetText();

        window.parent.UgitOpenPopupDialog('<%= absoluteUrlSearch %>' + params, "", 'Resource Availability', '90', '90', 0);
    }


    function convert(str) {
        var date = new Date(str),
            mnth = ("0" + (date.getMonth() + 1)).slice(-2),
            day = ("0" + date.getDate()).slice(-2);
        return [mnth, day, date.getFullYear()].join("/");
    }


    //Compare the start date and End Date of without recurrance patter

    function validateStartDateAndEndDate(source, args) {
      <%--  var StartDate = Date.parse(document.getElementById('<%=startDate.Controls[0].ClientID%>').value);
        var EndDate = Date.parse(document.getElementById('<%=endDate.Controls[0].ClientID%>').value);--%>

        ////if (StartDate <= EndDate) {
        ////    args.IsValid = true;
        ////}
        ////else {
        ////    args.IsValid = false;
        ////}
    }

    function OpenFindResourceAvailability() {

        if (window.location.href.indexOf("filterMode=CPRTeamAllocation") > -1) {
            var StartDate = '<%=allocationStartDate %>';
            var EndDate = '<%=allocationEndDate %>';

            //string title = projectTitle.Replace("'", string.Empty);
            window.parent.UgitOpenPopupDialog('<%=findResourceUrl %>' + '&pStartDate=' + StartDate + '&pEndDate=' + EndDate + '&ticketId=' + '<%=ticketID%>' + '&pGlobalRoleID=' + glUserGroup.GetValue(), "", "Resources Allocation for " + '<%=projectTitle.Replace("'","&#146;")%>', 90, 90);

            if (StartDate != null && StartDate != "" && EndDate != null && EndDate != "") {

                if (Date.parse(EndDate) > Date.parse(StartDate)) {
                    window.parent.UgitOpenPopupDialog('<%=findResourceUrl %>' + '&pStartDate=' + StartDate + '&pEndDate=' + EndDate + '&ticketId=' + '<%=ticketID%>' + '&pGlobalRoleID=' + glUserGroup.GetValue(), "", "Resources Allocation for " + '<%=projectTitle.Replace("'","&#146;")%>', 90, 90);
                }
                else {
                    $(".ErrorMessage").text("Invaild dates.");
                }

            }
            else {
                $(".ErrorMessage").text("Invaild dates.");
            }
        }
        else {
            var StartDate = document.getElementById('<%=startDate.Controls[0].ClientID%>').value;
            var EndDate = document.getElementById('<%=endDate.Controls[0].ClientID%>').value;

            if (StartDate != null && StartDate != "" && EndDate != null && EndDate != "") {

                if (Date.parse(EndDate) > Date.parse(StartDate)) {
                    if ($('.workitem').text() == "") {
                        window.parent.UgitOpenPopupDialog('<%=findResourceUrl %>' + '&pStartDate=' + StartDate + '&pEndDate=' + EndDate + '&ticketId=' + '<%=ticketID%>' + '&pGlobalRoleID=' + glUserGroup.GetValue(), "", "Resources Allocation for " + '<%=cprprojectId%>' + " " + '<%=projectTitle.Replace("'","&#146;")%>', 90, 90);
                    }
                    else {
                        if (cbLevel2.GetValue() != null || cbLevel2.GetValue() != undefined) {
                            window.parent.UgitOpenPopupDialog('<%=findResourceUrl %>' + '&pStartDate=' + StartDate + '&pEndDate=' + EndDate + '&ticketId=' + '<%=ticketID%>' + '&pGlobalRoleID=' + glUserGroup.GetValue(), "", "Resources Allocation for " + '<%=cprprojectId%>' + " " + '<%=projectTitle.Replace("'","&#146;")%>', 90, 90);
                            $(".ErrorMessage").text("");
                        }
                        else {
                            $(".ErrorMessage").text("Work Item Required.");
                        }
                    }
                }
                else {
                    $(".ErrorMessage").text("Start Date should be less then End Date.");
                }
            }
            else {
                $(".ErrorMessage").text("Dates are required.");
            }
        }

        return false;
    }


</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.usrGrp-dropDown').parents().eq(2).addClass('usrGrp-dropDownWrap');
        $('.projectTeam-popupWrap').parent().addClass("popup-container");
    });
</script>
<div class="projectTeam-popupWrap">
    <fieldset>
        <legend class="activity-title">Allocations </legend>
        <div class="ms-formtable accomp-popup CrmPopup_table col-md-12 col-sm-12 col-xs-12">
            <div class="row" id="trSTPMessage" runat="server" visible="false">
                <div class="ms-formlabel" style="text-align: center;">
                    <asp:Label ID="lblSTPMessage" runat="server" ForeColor="Red" Text="You are not on the project team, please add yourself to the team."> </asp:Label>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS" id="trSubItem" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Sub Project </h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtSubProject" runat="server" Width="265px" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS" id="trAdditionalUserGroup" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">User Groups<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxGridLookup Visible="true" CssClass="CRMDueDate_inputField dropDown-feild findResource-userGroup" TextFormatString="{0}"
                        SelectionMode="Multiple" ID="glUserGroup" Width="100%" ClientInstanceName="glUserGroup" runat="server"
                        KeyFieldName="GroupName" MultiTextSeparator=";" DropDownWindowStyle-CssClass="usrGrp-dropDown"
                        GridViewStyles-FilterRow-CssClass="findRes-filterWrap lookupDropDown-filterWrap" GridViewStyles-Row-CssClass="lookupDropDown-contentRow">
                        <Columns>
                            <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="30px" />
                            <dx:GridViewDataTextColumn FieldName="Name" Width="250px"> </dx:GridViewDataTextColumn>
                        </Columns>
                        <ClientSideEvents Init="function(s, e) {s.GetGridView().SetWidth(s.GetWidth());} "
                            DropDown="function(s, e) {	s.GetGridView().SetWidth(s.GetWidth()-2);} " />

                        <GridViewProperties>
                            <Settings ShowGroupedColumns="false" ShowFilterRow="true" VerticalScrollBarMode="Auto" VerticalScrollableHeight="100" ShowColumnHeaders="false" />
                            <SettingsBehavior AllowSort="false" />
                            <SettingsPager Mode="ShowAllRecords" PageSize="10"></SettingsPager>
                        </GridViewProperties>
                    </dx:ASPxGridLookup>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS" id="trType" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:DropDownList ID="ddlLevel1" Width="242px" runat="server" OnLoad="FillDropDownLevel1"
                        AutoPostBack="true" OnSelectedIndexChanged="FillDropDownLevel2">
                    </asp:DropDownList>
                </div>
            </div>
            <div id="trWorkItem" runat="server" class="workitem col-md-6 col-sm-6 colForXS">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Work Item<b style="color: Red;">*</b>
                    </h3>
                    <%-- TextFormatString="{0}"--%>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="cbLevel2" OnLoad="cbLevel2_Load" runat="server" Width="200" AutoPostBack="true" ClientInstanceName="cbLevel2" OnSelectedIndexChanged="cbLevel2_SelectedIndexChanged"
                        DropDownStyle="DropDownList" ValueField="LevelTitle" TextField="LevelTitle"
                        ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                        CallbackPageSize="10">
                        <Columns>
                        </Columns>
                    </dx:ASPxComboBox>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS" id="trUserGroup" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">User Group<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxComboBox ID="ddlUserType" runat="server" ValueType="System.String" ClientInstanceName="ddlUserType" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged"
                        IncrementalFilteringMode="StartsWith" DropDownStyle="DropDown" AutoPostBack="true" CssClass="CRMDueDate_inputField dropDown-feild">
                        <Items>
                            <dx:ListEditItem Text="APM" Value="APM" />
                            <dx:ListEditItem Text="Quality Control" Value="Quality Control" />
                            <dx:ListEditItem Text="Professional Services" Value="Professional Services" />
                            <dx:ListEditItem Text="Assistant Project Manager" Value="Assistant Project Manager" />
                            <dx:ListEditItem Text="Estimator" Value="Estimator" />
                            <dx:ListEditItem Text="Project Executive" Value="Project Executive" />
                            <dx:ListEditItem Text="Project Manager" Value="Project Manager" />
                            <dx:ListEditItem Text="Superintendent" Value="Superintendent" />
                        </Items>
                    </dx:ASPxComboBox>
                    <div>
                        <%--<asp:RequiredFieldValidator ID="rfvUserType" runat="server" ControlToValidate="ddlUserType" InitialValue=""
                                    ErrorMessage="* Select User Group" CssClass="text-error" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS fieldWrap" id="trUser" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">User<i class="required-item"></i></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div style="float: left; width: 100%;">
                        <dx:ASPxComboBox ID="ASPxComboBoxGroupUser" OnLoad="ASPxComboBoxGroupUser_Load" runat="server" ClientInstanceName="ASPxComboBoxGroupUser"
                            DropDownStyle="DropDownList" ValueField="Id" TextField="Name"
                            ValueType="System.String" IncrementalFilteringMode="Contains" FilterMinLength="0" EnableSynchronization="True"
                            CallbackPageSize="10" CssClass="CRMDueDate_inputField dropDown-feild">
                        </dx:ASPxComboBox>
                    </div>
                    <div class="auto-icon" style="display: none;">
                        <img id="imgAdd" src="/Content/Images/uGovernIT/Autocalculater.png" runat="server" />
                    </div>
                </div>
                <div class="row fieldWrap">
                    <div class="error-msgWrap">
                        <span class="ErrorMessage"></span>
                        <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="text-error"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-md-6 col-sm-6 colForXS" id="trAllocation" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Allocation %<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAlloc" runat="server" Width="25px" />
                    <div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtAlloc"
                            ErrorMessage="Please enter valid value." CssClass="text-error" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtAlloc"
                            ErrorMessage="Please select valid value." CssClass="text-error" Display="Dynamic" ValidationGroup="Save"
                            ValidationExpression="^[1-9]\d*$"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS cpr_Row" id="trStartDate" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Start Date<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <dx:ASPxDateEdit ID="startDate" runat="server" ClientInstanceName="startDate"
                        CssClass="edit-startdate datetimectr datetimectr111 CRMDueDate_inputField dropDown-feild startDateEdit"
                        DisplayFormatString="MM/dd/yyyy" IsRequiredField="true" EnableClientSideAPI="true" EditFormatString="MM/dd/yyyy"
                        DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18">
                        <ClientSideEvents DateChanged="startDateChange" />
                    </dx:ASPxDateEdit>
                    <asp:Label ID="lbstartDate" runat="server" Visible="false"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Save"
                        ControlToValidate="startDate" Display="Dynamic" CssClass="text-error">
                                <span>Please enter Start Date.</span>
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="Save"
                        ControlToValidate="startDate" Display="Dynamic" CssClass="text-error"
                        Type="Date" Operator="DataTypeCheck">
                                <span>Please enter a valid Start Date.</span>
                    </asp:CompareValidator>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS" id="trCPRDuration" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Duration<span>(Weeks)</span></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCPRDuration" runat="server" Width="25px" CssClass="CRMDurationClass" />
                    <div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCPRDuration"
                            ErrorMessage="* Incorrect Duration" CssClass="text-error" Display="Dynamic" ValidationGroup="Save"
                            ValidationExpression="\d*"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS cpr_Row" id="trEndDate" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">End Date<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <%--  <SharePoint:DateTimeControl DateOnly="true"
                        ID="endDate" runat="server" CssClassTextBox="edit-startdate datetimectr datetimectr111 endDateEdit"></SharePoint:DateTimeControl>--%>
                    <dx:ASPxDateEdit ID="endDate" ClientInstanceName="endDate" runat="server" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18"
                        CssClass="edit-startdate datetimectr datetimectr111 CRMDueDate_inputField dropDown-feild endDateEdit"
                        IsRequiredField="true" EnableClientSideAPI="true">
                        <ClientSideEvents DateChanged="endDateChange" />
                    </dx:ASPxDateEdit>
                    <asp:Label ID="lbendDate" runat="server" Visible="false"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Save"
                        ControlToValidate="endDate" Display="Dynamic" CssClass="text-error">
                            <span>Please enter End Date.</span>
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="Save"
                        ControlToValidate="endDate" Display="Dynamic" CssClass="text-error"
                        Type="Date" Operator="DataTypeCheck">
                            <span>Please enter a valid End Date.</span>
                    </asp:CompareValidator>

                    <asp:CustomValidator ID="CustomValidator1" runat="server"
                        ControlToValidate="endDate" Display="Dynamic" CssClass="text-error"
                        ClientValidationFunction="validateStartDateAndEndDate">
                            <span>End Date should be greater than Start Date.</span>
                    </asp:CustomValidator>
                </div>
            </div>
            <%--<div class="row fieldWrap">
                <div>
                    <span class="ErrorMessage" style="color: red;"></span>
                    <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                </div>
            </div>--%>
        </div>
    </fieldset>

    <div class="col-md-12 col-sm-12 col-xs-12 ms-formtable CrmPopup_tableNoPadding  popupAction-btnWrap">
        <div class="row">
            <div class="popupBtnWrap">
                <%--<div class="popupBtn_delete">--%>
                <asp:LinkButton ID="lnkAllocationSave" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save"
                    ValidationGroup="Save" OnClick="lnkAllocationSave_Click" CssClass="popupBtn_save">
                        <span class="btnSave">
                            <b>Save</b>
                          <%--  <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/_layouts/15/images/uGovernIT/ButtonImages/Save.png"  style="border:none;" title="" alt=""/>
                            </i> --%>
                        </span>
                </asp:LinkButton>

                <%--</div>--%>
                <%--<div class="popupBtn_save">--%>
                <asp:LinkButton ID="LnkbtnDelete" runat="server" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" ToolTip="Delete"
                    OnClick="LnkbtnDelete_Click" CssClass="popupBtn_delete">
                                <span class="btnSave">
                            <b>Delete</b>
                           <%-- <i style="float: left; position: relative; top: -3px;left:2px">
                        <img src="/_layouts/15/images/uGovernIT/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/>
                            </i>--%> 
                        </span>
                </asp:LinkButton>
                <%--</div>--%>
                <%--<div class="popupBtn_find">--%>
                <asp:LinkButton ID="btnFind" runat="server" OnClientClick="return OpenFindResourceAvailability();" CssClass="popupBtn_find">
                        <span class="findResource_findBtn">
                            <b style="font-weight: 500;">Find</b>
                           <%-- <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/_layouts/15/images/uGovernIT/search-white.png"  style="border:none;" title="" alt=""/>
                            </i> --%>
                        </span>
                </asp:LinkButton>
                <%--</div>--%>
                <div class="popupBtn_Cancel">
                    <asp:LinkButton ID="btnCancel" runat="server" OnClick="btnCancel_Click">
                        <span class="cancelBtn">
                            <b>Cancel</b>
                            <%--<i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/_layouts/15/images/uGovernIT/ButtonImages/cancelwhite.png"  style="border:none;" title="" alt=""/>
                            </i> --%>
                        </span>
                    </asp:LinkButton>
                </div>
            </div>

            <div style="text-align: center;">
                <asp:Panel ID="pAuditInformataion" runat="server" Visible="false" CssClass="fullwidth">
                    <asp:Label ID="lbCreatedInfo" runat="server" CssClass="fullwidth"></asp:Label>
                    <asp:Label ID="lbModifiedInfo" runat="server" CssClass="fullwidth"></asp:Label>
                </asp:Panel>
            </div>

            <div style="width: 100%; height: auto;">
            </div>
        </div>
    </div>
</div>

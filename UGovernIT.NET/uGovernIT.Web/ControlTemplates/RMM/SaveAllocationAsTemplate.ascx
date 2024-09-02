<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SaveAllocationAsTemplate.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.SaveAllocationAsTemplate" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    
      .findresource-background{
        background:none !important;
    }
    .findresource-background .dxb{
        background:#4fa1d6 !important;
        border-radius:4px;
    }

    .myColor .dx-switch .dx-state-hover .dx-switch-handle:before {
        background-color: yellow !important;
    }

    .myColor .dx-switch-handle:before {
        background-color: red !important;
    }

    .myColor .dx-switch .dx-state-hover .dx-switch-on-value .dx-switch-handle:before {
        background-color: green !important;
    }

    .myColor .dx-switch-on-value .dx-switch-handle:before {
        background-color: orange !important;
    }
</style>
<script type="text/javascript" id="dxss_inlineCtrSaveAsTemplate" data-v="<%=UGITUtility.AssemblyVersion %>">
    var ExistingTemplates = [];
    var IsExisting = false;
    var ExistingTemplateID = 0;
    var saveTemplateControls = {};
    var ProjectTemplates = [];
    $.get("/api/rmmapi/GetProjectTemplates", function (data, status) {
        ProjectTemplates = data;
    });
    $(function () {
        $("#toast_template").dxToast({
            message: "Template Saved Successfully. ",
            type: "success",
            displayTime: 1000,
            position: "{ my: 'center', at: 'center', of: window }"
        });
        $("#switchContainer").dxSwitch({
            value: false,
            width: 60,
            switchedOffText: "NO",
            switchedOnText: "YES",
            onInitialized: function (e) {
                saveTemplateControls.existingCtrl = e.component;
            },
            onValueChanged: function (e) {
                ExistingTemplateID = 0;
                IsExisting = false;
                if (e.value)
                    $("#divSaveTemplates").show();
                else
                    $("#divSaveTemplates").hide();
            }
        });

        $("#divSaveTemplates").dxSelectBox({
            placeholder: "Select Existing Template",
            valueExpr: "ID",
            displayExpr: "Name",
            
            dataSource: "/api/rmmapi/GetProjectTemplates",
            onInitialized: function (e) {
                saveTemplateControls.existingTplCtrl = e.component;
            },  
            onSelectionChanged: function (e) {
                if (typeof e.selectedItem.Name !== "undefined") {
                    txtTemplateName.SetText(e.selectedItem.Name);
                    ExistingTemplateID = e.selectedItem.ID;
                    IsExisting = true;
                }
            },
            onContentReady: function (e) {

            }
        });

    });
    DevExpress.ui.dxOverlay.baseZIndex(99999);

    function startDateChange(s, e) {
        var constStartDate = dteStartDate.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined)
        {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }
        var noOfWeeks = txtDuration.GetText();   

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
                                
                                dteEndDate.SetDate(new Date(resultJson.enddate));
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

    function endDateChange(s, e) {
       
            var constStartDate = dteStartDate.GetDate();
            if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
                constStartDate = constStartDate.format('MM/dd/yyyy');
            }

            var constEndDate = dteEndDate.GetDate();
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


                            txtDuration.SetText(resultJson.duration);
                        }
                        else {

                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        //alert(thrownError);
                    }
                });
            
            //// e.processOnServer = false;

        }
    }

    function changeEndateOnDuration(s, e) {
        var constStartDate = dteStartDate.GetDate();
        constStartDate = constStartDate.format('MM/dd/yyyy');
        var noOfWeeks = txtDuration.GetText();
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
                                
                                dteEndDate.SetDate(new Date(resultJson.enddate));
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

    function saveAsTemplate(s, e) {
        if (!txtTemplateName.isValid)
            return;
        var templateName = txtTemplateName.GetValue().trim().toLowerCase();

        if (ProjectTemplates.filter(x => x.Name.trim().toLowerCase() == templateName).length > 0) {
            var result = DevExpress.ui.dialog.confirm('Selected Project Allocations will override current template allocations. Do you want to proceed?', 'Confirm');
            result.done(function (dialogResult ) {
                if (dialogResult) {
                    IsExisting = true;
                    ExistingTemplateID = ProjectTemplates.filter(x => x.Name.toLowerCase() == templateName)[0].ID;
                    saveAsTemplateInternal();
                }
            });
        }
        else {
            ExistingTemplateID = 0;
            saveAsTemplateInternal();
        }
    }

    function saveAsTemplateInternal() {
        $("#lblMessage").text('');
        loadingPanel.Show();
        var templateName = txtTemplateName.GetValue();
        var aStartDate = '<%=PreconStartDate%>';
        var aEndDate = '<%=CloseOutEndDate%>'
        var aDuration = GetDurationInWeek('<%=ajaxHelper %>', aStartDate, aEndDate);
        //let templateJson = JSON.stringify(globaldata);
       
        var data = {
            Name: templateName, TicketID: "<%= ProjectID %>", ModuleName: "<%= ModuleName %>", StartDate: aStartDate,
            EndDate: aEndDate, Duration: aDuration, SaveOnExiting: IsExisting, ID: ExistingTemplateID,
            PreconStartDate: '<%=PreconStartDate%>', PreconEndDate: '<%=PreconEndDate%>', ConstStartDate: '<%=ConstStartDate%>',
            ConstEndDate: '<%=ConstEndDate%>', CloseOutStartDate: '<%=CloseOutStartDate%>', CloseOutEndDate: '<%=CloseOutEndDate%>'
        };
        $.post(ugitConfig.apiBaseUrl + "/api/RMMAPI/SaveAllocationAsTemplate", data).then(function (response) {
            txtTemplateName.SetText("");
            loadingPanel.Hide();
            $("#toast_template").dxToast("show");
            var url = "/layouts/ugovernit/delegatecontrol.aspx?control=CRMTemplateAllocationView&templateid=" + response;
            window.parent.UgitOpenPopupDialog(url, "", "Preview Template", "95", "95", false, "");
            var popupCtrl = ASPxClientControl.GetControlCollection().GetByName("<%= PopupID%>");
            if (popupCtrl)
                popupCtrl.Hide();
        }, function (error) {
            var msg = error.responseJSON.Message;
            DevExpress.ui.dialog.alert(msg,"alert");
            var popupCtrl = ASPxClientControl.GetControlCollection().GetByName("<%= PopupID%>");
            if (popupCtrl)
                 popupCtrl.Hide();
            loadingPanel.Hide();
            //lblMessage.SetText(error.responseJSON.Message);
        });          
    }


    function st_resetCtrl() {
        ExistingTemplateID = 0;
        IsExisting = false;
        txtTemplateName.SetText("");
        saveTemplateControls.existingCtrl.reset();
        saveTemplateControls.existingTplCtrl.reset();
        saveTemplateControls.existingTplCtrl.getDataSource().reload();
    }
</script>
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" Text="Please wait..." ClientInstanceName="loadingPanel"></dx:ASPxLoadingPanel>
<div id="toast_template"></div>
<div class="row">
   <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
       <dx:ASPxLabel ID="lblMessage" runat="server" ClientInstanceName="lblMessage"  CssClass=""> </dx:ASPxLabel>
    </div>
</div>
<div class="row" style="float:left;display:none">
    <div class="col-md-4 col-sm-6 col-xs-12 noPaddingLeft">
        <div class="ms-formlabel">
            <h3 class="template_fieldLabel ms-standardheader"> Start Date: </h3>
        </div>
        <div class="accomp_inputField">
            <dx:ASPxDateEdit OnValueChangeClientScript="dateChanged()" Width="100%"  ID="dteStartDate" ClientInstanceName="dteStartDate" runat="server" DisplayFormatString="MMM d, yyyy" CssClass="saveAlloct aspTextbox-wrap"  
                DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18"  Height="28px" AutoPostBack="false">
                   <ValidationSettings ValidationGroup="template" ErrorDisplayMode="ImageWithTooltip">
                    <RequiredField IsRequired="false" ErrorText="Please enter start date." />
                </ValidationSettings>
                  <ClientSideEvents DateChanged="startDateChange" />
            </dx:ASPxDateEdit>
        </div>
        </div>
    <div class="col-md-4 col-sm-6 col-xs-12 noPaddingLeft">
        <div class="ms-formlabel">
            <h3 class="template_fieldLabel ms-standardheader"> Duration(Weeks): </h3>
        </div>
        <div class="accomp_inputField">
         
           <dx:ASPxTextBox ID="txtDuration" ClientInstanceName="txtDuration" runat="server"  CssClass="saveAlloct aspTextbox-wrap" Height="30px">
                <ClientSideEvents TextChanged="changeEndateOnDuration" />
           </dx:ASPxTextBox>
        </div>
           </div>
    <div class="col-md-4 col-sm-6 col-xs-12 noPaddingLeft">
        <div class="ms-formlabel">
            <h3 class="template_fieldLabel ms-standardheader"> End Date: </h3>
        </div>
        <div class="accomp_inputField">
           <dx:ASPxDateEdit OnValueChangeClientScript="dateChanged()" ID="dteEndDate" Width="100%" ClientInstanceName="dteEndDate" runat="server"  CssClass="saveAlloct aspTextbox-wrap"
               DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18"  Height="30px" AutoPostBack="false" DisplayFormatString="MMM d, yyyy" >
                  <ValidationSettings ValidationGroup="template" ErrorDisplayMode="ImageWithTooltip">
                    <RequiredField IsRequired="false" ErrorText="Please enter end date." />
                </ValidationSettings>
               <ValidationSettings>
                   <RegularExpression ValidationExpression="" />
               </ValidationSettings>
             <DateRangeSettings CalendarColumnCount="1" StartDateEditID="dteStartDate" />
               <ClientSideEvents DateChanged="endDateChange" />
           </dx:ASPxDateEdit>
        </div>
    </div>
 </div>
<div class="row" style="display:none;">
    <div class="col-md-12 col-sm-12 col-xs-12 noPaddingLeft">
        <div style="float:left; padding-top:10px;">
            <div style="padding-top:3px;" >
                <h3 class="template_fieldLabel ms-standardheader"> Change Existing Template: </h3>
            </div>
        </div>
        <div id="switchContainer" style="float:left; padding-top:10px; padding-left:10px;"></div>
    </div>
</div>

<div class="row">
    <div class="col-md-12 col-sm-12 col-xs-12 devExt-selectBoxWrap">
        <div id="divSaveTemplates" class="devExt-selectBox" style="display:none" ></div>
    </div>
</div>

<div class="row" style="padding-top:10px;">
    
    <div class="col-md-12 col-sm-12 col-xs-12 noPaddingLeft">
        <div class="ms-formlabel">
            <h3 class="template_fieldLabel ms-standardheader">Template Name<b class="error">*</b>:</h3>
        </div>
        <div class="accomp_inputField accomp_inputField">
            <dx:ASPxTextBox ID="txtTemplateName" runat="server" ClientInstanceName="txtTemplateName" Width="100%" Height="35px"  CssClass="aspTextbox-wrap">
                <ValidationSettings ValidationGroup="template" ErrorDisplayMode="ImageWithTooltip">
                    <RequiredField IsRequired="true" ErrorText="Please enter template name." />
                </ValidationSettings>
            </dx:ASPxTextBox>
        </div>
    </div>
</div>



 <div class="row rmmSaveAllo-btnContainer next-cancel-but">
     <dx:ASPxButton ID="btTemplate" runat="server" CssClass="findresource-background next" AutoPostBack="false" ValidationGroup="template" Text="Save Template">
         <ClientSideEvents Click="function(s,e){ saveAsTemplate(s,e); }" />
     </dx:ASPxButton>
 </div>



<%@ Import Namespace="uGovernIT.Manager" %>
<%@ Import Namespace="System.Data" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uGovernITModuleWebpartUserControl.ascx.cs" Inherits="uGovernIT.Web.uGovernITModuleWebpartUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/TicketCommentsView.ascx" TagPrefix="uc1" TagName="TicketCommentsView" %>
<%@ Register Src="~/ControlTemplates/Utility/CustomListDropDown.ascx" TagPrefix="uc2" TagName="CustomListDropDown" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Web.ASPxSpellChecker" Assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<%@ Import Namespace="uGovernIT.Utility" %>
 
<asp:PlaceHolder runat="server">
    <%= UGITUtility.LoadScript("../../Scripts/HelpCardDisplayPopup.js") %>
</asp:PlaceHolder>
<asp:HiddenField ID="eAll" runat="server" Value="0" />
<asp:HiddenField ID="currentTicketIdHidden" runat="server" Value="0" />
<asp:HiddenField ID="hdnTicketCurrentStage" runat="server" Value="" />
<asp:HiddenField ID="hdnSendMailFromStatus" runat="server" Value="false" />
<asp:HiddenField ID="hdnOnHoldReason" runat="server" Value="0" />

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ullist {
        /* grid-template-columns: repeat(8, 200px);*/
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        display: grid;
        margin-top: 5px;
        padding-left: 10px
    }
    .ullist li {
        padding: 6px;
        background-color: #ecf3edf7;
        margin: 2px;
        list-style: none;
    }
    .tdshowallrelatedticket {
        position: absolute;
        top: 6px;
        left: 110px;
    }
    .tdshowallrelatedticket .stagerelatedicon {
        justify-content: start;
    }
    /*.nav {
        text-decoration: none;
        display: inline-block;
        padding: 8px 16px;
    }*/

    /*.nav:hover {
            background-color: #ddd;
            color: black;
        }*/

    /*.previous {
        background-color: #216cf2;
        color: black;
    }

    .next {
        background-color: #216cf2;
        color: white;
    }*/

    /*.compactViewImg{
        cursor:pointer;*/
    /*align-items: center;
          display: flex;
          justify-content: center;*/
    /*}*/

    .flashCard-container-display {
        width: 300px;
        /*margin: 0px auto;
		    border: 1px solid #FFF;*/
        height: 450px;
        /*padding: 10px;
	        box-shadow: 0px 0px 4px #888888;*/
    }

        .flashCard-container-display img {
            max-width: 100%;
        }

    .ugit-stick-bottom {
        left: 0px !important;
        width: 100% !important;
    }

    .scroll-to-fixed-fixed {
        padding: 0px 13px 0px 25px !important;
    }

    .ms-usereditor {
        margin-top: 5px;
        width: 100%;
    }

    .opacity40 {
        opacity: 0.4;
    }

    .whiteborder {
        border: 1px solid white !important;
    }

    .blackborder {
        border: 1px solid black;
    }

    .ShowMe {
        display: block;
    }

    .HideMe {
        display: none;
    }

    linkbutton {
        font-weight: bold;
        font-size: 7pt;
        text-transform: capitalize;
        color: white;
        font-family: Verdana;
    }

    .doubleWidthnHeight {
        height: 40px !important;
        width: 99% !important;
    }

    .extraHeightWithDoubleWidth {
        height: 60px !important;
        width: 99% !important;
    }

    .descExtraHeightWithDoubleWidth {
        height: 150px !important;
        width: 99% !important;
    }

    .doubleWidth {
        width: 99% !important;
    }

    .s4-toplinks .s4-tn a.selected {
        padding-left: 10px;
        padding-right: 10px;
    }

    .s4-ba {
        width: 98%;
    }

    .leftBox {
        width: 1%;
        height: 54px;
        text-align: right;
        background: url(/Content/Images/box_left.gif) no-repeat;
    }

    .rightBox {
        width: 1%;
        height: 54px;
        background: url(/Content/Images/box_right.gif) no-repeat;
    }

    .middleBox {
        width: 100%;
        /*padding-top: 10px;*/
        text-align: left;
        /*float: left;
        margin-top: 1px;
        margin-left: -1px;*/
        background: url(/Content/Images/box_mid.gif) repeat-x;
    }

    .width25 {
        width: 25%;
    }

    .spancontainer {
        float: left;
        width: 100%;
    }

    /*.imp-message-box {
        font-weight: bold;
        font-size: 12px;
        color: Red;
    }*/

    .reportitem {
        border-bottom: 1px solid black;
        float: left;
        padding: 5px;
        padding-top: 5px;
        width: 91%;
        cursor: pointer;
        color: black;
    }

    .steplineimage {
        height: 38px;
        background-repeat: repeat-x;
        float: left;
        margin-top: -2px;
        margin-left: -1px;
    }

    .clsshowhide {
        display: none;
    }

    .tabmaindiv {
        float: left;
        width: 100%;
    }

    .tabmaindivinner {
        float: left;
    }

    .tabspan {
        float: left;
        padding: 6px;
        margin-right: 2px;
    }

    linkbutton {
        FONT-WEIGHT: bold;
        FONT-SIZE: 7pt;
        TEXT-TRANSFORM: capitalize;
        COLOR: white;
        FONT-FAMILY: Verdana;
    }

    .relatedticketbefore {
        position: absolute;
        right: 110px;
        top: 50%;
        transform: translateY(-50%);
    }

    .stagerelatedicon {
        display: flex;
        justify-content: center;
        flex-wrap: wrap;
    }

    .framepanel {
        float: left;
        width: 100%;
    }

    .dropdownicon img {
        background-position: -144px -445px !important;
        width: 10px !important;
        height: 10px !important;
    }

    .resolution-area {
        width: 325px;
        height: 91px;
    }

    .dxtc-sb {
        position: relative;
        top: 0px;
    }

    .tbcDetailTabs_SVA {
        width: 900px;
    }

    .imgReport {
        margin-right: 0px !important;
    }

    .ModuleBlock {
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        position: absolute;
        z-index: 1;
    }

    .dxmodalSys .dxmodalTableSys.dxpc-contentWrapper .dxpc-content {
        display: block;
    }

    .dxmodalSys > .dxpclW{
        width: 100%;
        min-width: 900px !important;
    }

    .node {
        height: 20px;
        width: 20px;
    }

    .pos_rel .node-stageno {        left: 0;
        font-size: 12px;
        top: -5px;
    }

    .textWhite {
        color: #fff;    }

    .textBlack {
        color: #000;
    }
    .timelineopenclose {
        padding-left: 4px;
    }

    .timelineopencloseedit {
        /*margin-top: 10px;*/
        padding-left: 2px;
        /*float: left;
        position: absolute;
        float: right;*/
    }
    #pendingTasks .dx-list-item-content::before {
        content: "";
        width: 5px;
        height: 5px;
        border-radius: 50%;
        margin-right: 9px;
        display: inline-block !important;
        background-color: black;
        vertical-align: middle !important;
        margin-top: 6px;
    }

    #pendingTasks .dx-list:not(.dx-list-select-decorator-enabled) .dx-list-item.dx-state-hover {
        background-color: #fff;
        color: #333;
    }

    .titleText {
        font-size: 16px;
        font-family: 'Roboto', sans-serif !important;
    }

    #pendingTasks .dx-scrollview-content {
        font-family: 'Roboto', sans-serif !important;
        font-size: 14px;
    }
    .project-phase-inner {
        padding: 5px 10px;
        border-radius: 5px;
        color: white;
        background-color: red;
        font-size: 13px;
        font-weight: bold;
    }
     .statusMemo{
        border:none !important;
    }
    .reject-btn {
        user-select: none;
        background: #f65d50 !important;
        border-radius: 4px;
        padding: 6px 15px 7px 10px !important;
        font: 14px 'Roboto', sans-serif !important;
        background-repeat: no-repeat !important;
        background-size: 34px !important;
        background-position: 0px 5px !important;
    }
    #rejectWithCommentsButtonB .dxbButtonHover_UGITNavyBlueDevEx {
        color: #fff !important;
        background: #dde6fe url(/DXR.axd?r=103_1792-t3xkr) repeat-x left top;
        border: 0px solid #a9acb5 !important;
    }

    .btnStyleBottom .dxbButtonSys.dxbTSys {
        margin-top: 0px !important;
    }

    ol, ul {
        margin-top: 0;
        margin-bottom: 0px;
    }
    /*.btnStyleBottom{
        margin-top: 17px !important;
    }*/
</style>
<% if (Request["IsDlg"] != null && Request["IsDlg"].ToString() == "1")
    { %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ModuleBlock {
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        position: absolute;
        z-index: 1;
    }

    .attachmentBox {
        float: left;
        width: 100%;
    }

        .attachmentBox span {
            float: left;
            width: 100%;
        }
        .lblShortNamecss {
            display:none;
            float:right;
        }
    
</style>
<% } %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var currentTicketId = '<%=currentTicketId %>';
    var currentModuleName = '<%=currentModuleName%>';
    var currentTicketPublicID = '<%=currentTicketPublicID%>';
    var AbsolutePath = '<%=Request.Url.AbsolutePath %>';    
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    //Look ahead
    var StatusButtonName = "";
    var drqRelatedRequestControl;
    var ModuleName = "<%= currentModuleName%>";
    var hasAnyPastAllocation = "<%=this.HasAnyPastAllocation%>" == "True" ? true : false;
    $(document).ready(function () {
        $("#rejectWithCommentsButtonB .dxbButton_UGITNavyBlueDevEx img").css("filter", "brightness(0) invert(1)");
        $("#rejectWithCommentsButtonB .dxbButton_UGITNavyBlueDevEx").removeClass("dxbButton_UGITNavyBlueDevEx").addClass("reject-btn");
        //For PMM module, hide svcWorkflow-container. Workflow will be displayed using StageRepeater (stage nos. in plain bubbles).
        if (ModuleName == "PMM") {
            $(".svcWorkflow-container").hide();
        }
        else
            $(".svcWorkflow-container").show();

        //Set the clientID of textbox for Related Request ID
        if ($(".field_RelatedRequestID_edit input") != undefined && $(".field_RelatedRequestID_edit input").length > 0) {
            drqRelatedRequestControl = $(".field_RelatedRequestID_edit input").get(0).id;
        }
        //hide unhide next previous buttons on end indexes
        if ($.cookie("hidenext") === "true" || window.parent.$.cookie("hidenext") === "true")
            $("a.nav.next").hide();
        if ($.cookie("hideprevious") === "true" || window.parent.$.cookie("hideprevious") === "true")
            $("a.nav.previous").hide();
        if ($.cookie("hideprevious") === "true" || window.parent.$.cookie("hideprevious") === "true" || $.cookie("hidenext") === "true" || window.parent.$.cookie("hidenext") === "true")
            $('.edit-ticket-devider').hide();
        $(".hideAndShowTables").slideDown();
        $(".SectionHeader a.imageForToggle").on('click', function () {
            $(this).closest('.SectionHeader').find("table:eq(0)").toggle();
            if ($(this).hasClass('showMore')) {
                $(this).removeClass('showMore');
                $(this).addClass('showLess');
                //$(this).closest('.SectionHeader').find("table:eq(0)").addClass('hideAndShowTables');
            } else {
                $(this).addClass('showMore');
                $(this).removeClass('showLess');
                //if ($(this).closest('.SectionHeader').find("table:eq(0)").hasClass('hideAndShowTables')) {
                //    $(this).closest('.SectionHeader').find("table:eq(0)").removeClass('hideAndShowTables');
                //} else {
                //     $(this).closest('.SectionHeader').find("table:eq(0)").toggle();
                //}

            }
        });

        var demo = $("input[type=text][id*='Title']");
        if (demo.text()) {
            $(".relatedTitleButtonClass").addClass("showrelatedTitleButtonClass");
        }
        var closeoutDate = closeoutdateclientname.GetDate();
        if (closeoutDate != null && closeoutDate != "" && closeoutDate != undefined) {
            closeoutDate = closeoutDate.format('MMM dd, yyyy');
            $(".field_closeoutdate_view").text(closeoutDate);
        }
    });

    function hideddlOnHoldReason(action) {
        var category = $("#<%=ddlOnHoldReason.ClientID%> option:selected").text();
        $(".divddlOnHoldReason").hide();
        $("#<%=hdnOnHoldReason.ClientID%>").val('1');
        if (action == 1) {
            $("#<%=hdnRequestOnHoldReason.ClientID%>").val(category);
            $("#<%=txtOnHoldReason.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestOnHoldReason.ClientID%>").val("");
            $("#<%=txtOnHoldReason.ClientID%>").val("");
        }
    }
    function showddlOnHoldReason() {
        $("#<%=hdnOnHoldReason.ClientID%>").val('0');
        $(".divddlOnHoldReason").show();
    }

    function ShowCount() {
        if (typeof relatedTitleButton == "undefined")
            return;

        var requestTypeID = getValue_RequestType();
        var title = $("input[type=text][id*='Title']").val();
        var requestType = requestTypeID || '';
        if (title != "") {
            if (title)
                $(".relatedTitleButtonClass").addClass("showrelatedTitleButtonClass");

            var pageURL = "<%=ajaxHelperPage %>/GetTitleRelatedTicketCount";

            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: pageURL,
                data: JSON.stringify({ "tableName": ModuleName, "titleName": title, "requestType": requestType }),
                dataType: 'json',
                success: function (result) {
                    //var str = result; //JSON.parse(result.d);
                    if (requestType == "") {
                        $(".result").html("<div class='linkRelatedTicket_msg'><span>Based on Title, total " + result.d + " similar tickets are found.</span><span class='relatedLink_newMsg'> NOTE: Select Request Type from Classification section for more match. </span>" + "<span class='badge badge-light'>" + result.d + "</span> </div>");
                    }
                    else
                        $(".result").html("<div class='linkRelatedTicket_msg'><span>Based on Title & Request type, total " + result.d + " similar tickets are found.  </span> " + "<span class='badge badge-light'>" + result.d + "</span> </div>");
                },
                error: function (xhr, ajaxOptions, thrownError) { //Add these parameters to display the required response                   
                }
            });
        }
    }

    $(document).on("focusout", "input[type=text][id*='Title']", function () {

        ShowCount();
    });

    function ShowPicker(obj) {
        var viewSpan = $($(obj).siblings().get(0));
        var anchorChild = viewSpan.find("a");
        var encryptedControlID = escape(drqRelatedRequestControl + ";~" + anchorChild[0].id);
        var pickerUrl = "<%=pickerListUrl%>" + "&pageTitle=Picker List&isdlg=1&isudlg=1&&Type=RelatedRequestTicket" + "&ControlId=" + encryptedControlID + "&TicketId=" + "<%=currentTicketPublicID%>";
        window.parent.UgitOpenPopupDialog(pickerUrl, '', 'Picker List', '85', '85', false, escape(window.location.href));
    }
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var moduleSetting = {
        ModuleName: currentModuleName,
        OwnerBinding: "<%= TicketRequest.Module.OwnerBindingChoice%>",
        RequestTypeDetail: null,
        PriorityMap: []
    }

    $.ugit.getPriorityMapping(moduleSetting.ModuleName).done(function (response) { try { moduleSetting.PriorityMap = response; } catch (ex) { } });

</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var value = true;
    var dateModel = {
        preconStartDate: "",
        preconEndDate: "",
        constStartDate: "",
        constEndDate: "",
        closeoutStartDate: "",
        closeoutEndDate: "",
    };
    function GetProjectDates() {
        $.get("/api/rmone/GetProjectDates?TicketId=" + "<%=Request["ticketId"].Trim()%>", function (data, status) {
            dateModel.preconStartDate = data.PreconStart == '0001-01-01T00:00:00' ? '' : new Date(data.PreconStart).format('MM/dd/yyyy');
            dateModel.preconEndDate = data.PreconEnd == '0001-01-01T00:00:00' ? '' : new Date(data.PreconEnd).format('MM/dd/yyyy');
            dateModel.constStartDate = data.ConstStart == '0001-01-01T00:00:00' ? '' : new Date(data.ConstStart).format('MM/dd/yyyy');
            dateModel.constEndDate = data.ConstEnd == '0001-01-01T00:00:00' ? '' : new Date(data.ConstEnd).format('MM/dd/yyyy');
            dateModel.closeoutStartDate = data.CloseoutStart == '0001-01-01T00:00:00' ? '' : new Date(data.CloseoutStart).format('MM/dd/yyyy');
            dateModel.closeoutEndDate = data.Closeout == '0001-01-01T00:00:00' ? '' : new Date(data.Closeout).format('MM/dd/yyyy');
        });
    }
    GetProjectDates();
    function OpenAutoUpdateDialog(updateType) {
        $('#<%=hdnAutoUpdateAllocaionDates.ClientID%>').val("false");
        $('#<%=hdnUpdatePastDates.ClientID%>').val("false");
        if (dateModel.preconStartDate != '' && dateModel.preconEndDate != '' && dateModel.constStartDate != '' && dateModel.constEndDate) {
            let preconStartDate = '', preconEndDate = '', constStartDate = '', constEndDate = '', closeoutEndDate = '';

            if (typeof preconstartdateclientname !== 'undefined')
                preconStartDate = preconstartdateclientname.GetDate().format('MM/dd/yyyy');
            else 
                preconStartDate = dateModel.preconStartDate;

            if (typeof preconenddateclientname !== 'undefined')
                preconEndDate = preconenddateclientname.GetDate().format('MM/dd/yyyy');
            else 
                preconEndDate = dateModel.preconEndDate;

            if (typeof estimatedconstructionstartclientname !== 'undefined') 
                constStartDate = estimatedconstructionstartclientname.GetDate().format('MM/dd/yyyy');
            else 
                constStartDate = dateModel.constStartDate;
            
            if (typeof estimatedconstructionendclientname !== 'undefined') 
                constEndDate = estimatedconstructionendclientname.GetDate().format('MM/dd/yyyy');
            else 
                constEndDate = dateModel.constEndDate;

            if (typeof closeoutdateclientname !== 'undefined') 
                closeoutEndDate = closeoutdateclientname.GetDate().format('MM/dd/yyyy');
            else 
                closeoutEndDate = dateModel.closeoutEndDate;

            if (dateModel.preconStartDate != preconStartDate || dateModel.preconEndDate != preconEndDate
                || dateModel.constStartDate != constStartDate || dateModel.constEndDate != constEndDate || dateModel.closeoutEndDate != closeoutEndDate) {
                var confirmDialog = DevExpress.ui.dialog.custom({
                    title: "Alert",
                    message: `Phase Dates Changed.<br/>Do you want RM One to shift allocations also?`,
                    buttons: [
                        { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                        { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                    ]
                });
                confirmDialog.show().done(function (dialogResult) {
                    if (dialogResult == "Ok") {
                        if (hasAnyPastAllocation) {
                            var conflictDialog1 = DevExpress.ui.dialog.custom({
                                title: "Alert",
                                message: `Do you want to shift the past dates?`,
                                buttons: [
                                    { text: "Yes", onClick: function () { return "Ok" }, elementAttr: { "class": "btnBlue" } },
                                    { text: "No", onClick: function () { return "Cancel" }, elementAttr: { "class": "btnNormal" } }
                                ]
                            });
                            conflictDialog1.show().done(function (dialogResult) {
                                if (dialogResult == "Ok") {
                                    $('#<%=hdnAutoUpdateAllocaionDates.ClientID%>').val("true");
                                    $('#<%=hdnUpdatePastDates.ClientID%>').val("true");
                                    waitTillUpdateComplete();
                                    ActionContainer(updateType);
                                }
                                else if (dialogResult == "Cancel") {
                                    $('#<%=hdnAutoUpdateAllocaionDates.ClientID%>').val("true");
                                    waitTillUpdateComplete();
                                    ActionContainer(updateType);
                                }
                            });
                        }
                        else {
                            $('#<%=hdnAutoUpdateAllocaionDates.ClientID%>').val("true");
                            waitTillUpdateComplete();
                            ActionContainer(updateType);
                        }
                    }
                    else if (dialogResult == "Cancel") {
                        waitTillUpdateComplete();
                        ActionContainer(updateType);
                    }
                });
            }
            else {
                waitTillUpdateComplete();
                ActionContainer(updateType);
            }
        }
        else {
            waitTillUpdateComplete();
            ActionContainer(updateType);
        }
    }
    function ActionContainerMain(senderValue) {
        var value = false;
        var currentModuleName = '<%=currentModuleName%>';
        if (currentModuleName == 'CPR') {
            endConstructionDateChanged(senderValue);
        }
        else {
            ActionContainer('btnupdateButton');
        }

    }

    //------------------------------------------ start
    function CRMDurationChanged(s, e) {
        //console.log(s)        
        DataChanged();
        var constStartDate = estimatedconstructionstartclientname.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }

        var noOfWeeks = $('.CRMDuration').val();
        noOfWeeks = Math.ceil(noOfWeeks);
        if (constStartDate != null && constStartDate != "" && noOfWeeks != null && noOfWeeks != "" && noOfWeeks > 0) {

            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","noOfWeeks":"' + parseInt(noOfWeeks) + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperPage %>/GetEndDateByWeeks",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        //console.log(resultJson);
                        var enddate = new Date(resultJson.enddate)
                        $(".field_estimatedconstructionend_view").text(enddate.format('MMM dd, yyyy'));
                        estimatedconstructionendclientname.SetDate(enddate);
                        // Changed closeout start and end date if const end date changed.
                        changeCloseOutEndDate();
                        if (s == "btnupdateButton") {
                            ActionContainer('btnupdateButton');
                        }

                    }
                    else {
                        if (s == "btnupdateButton") {
                            ActionContainer('btnupdateButton');
                        }
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {

                }
            });

        }
        if (s == "btnupdateButton") {
            ActionContainer('btnupdateButton');
        }
        //DataChanged();
    }


    function ApproxContractValueChanged(s, e) {
        var appxContractValue = 0;
        if (!Number.isNaN(s.GetText())) {
            appxContractValue = s.GetText();

            var Data = '{' + '"ContractValue":"' + appxContractValue + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperPage %>/GetProjectComplexity",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        $(".field_crmprojectcomplexity_view").text(resultJson.projectComplexity);
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
    }

    function ShortNameKeyPress(s, e) {
       var ctrlShortName = document.getElementsByClassName("lblShortNamecss");
        ctrlShortName[0].style.display = "none";
        var shortNameLength = "<%=ShortNameLength %>";
        if (shortNameLength) {
            if (s.GetText().length >= shortNameLength) {
                ctrlShortName[0].style.display = "block";
                setTimeout(() => {
                    ctrlShortName[0].style.display = 'none';
                }, 1000);
                return false;
            }
        }

    }

    //---------------------------------------------------------------------------------------end
    $(document).ready(function () {
        $('.field_CRMDuration_edit').find('input').addClass('CRMDuration');
        $('.field_EstimatedConstructionEnd').find('input').addClass('field_estimatedconstructionend_input1');
        $('.field_EstimatedConstructionStart').find('input').addClass('field_estimatedconstructionStart_input1');
        var ctrlShortName = document.getElementsByClassName("lblShortNamecss");
        ctrlShortName[0].style.display = "none";

    });

    //--------------------------------------------------------------------start
    function endConstructionDateChanged(s, e) {

        var constStartDate = estimatedconstructionstartclientname.GetDate();
        if (constStartDate != null && constStartDate != "" && constStartDate != undefined) {
            constStartDate = constStartDate.format('MM/dd/yyyy');
        }
        var constEndDate = estimatedconstructionendclientname.GetDate();
        if (constEndDate != null && constEndDate != "" && constEndDate != undefined) {
            constEndDate = constEndDate.format('MM/dd/yyyy');
        }
        //console.log(constEndDate);
        //console.log(constStartDate);
        DataChanged();

        if (constStartDate != null && constStartDate != "" && constEndDate != null && constEndDate != "") {

            var paramsInJson = '{' + '"startDate":"' + constStartDate + '","endDate":"' + constEndDate + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelperPage %>/GetDurationInWeeks",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    if (resultJson.messagecode == 2) {
                        $('.field_CRMDuration_view').text(resultJson.duration);
                        $('.CRMDuration').val(resultJson.duration);

                        if (s == "btnupdateButton") {
                            CRMDurationChanged("btnupdateButton");
                        }
                    }
                    else {

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    //alert(thrownError);
                }
            });
            // Changed closeout start and end date if const end date changed.
            changeCloseOutEndDate();
        }
        if (s == "btnupdateButton") {
            CRMDurationChanged("btnupdateButton");
        }

        //DataChanged();
    }

    function changeCloseOutEndDate() {
        $.ajax({
            type: "GET",
            url: "<%= ajaxPageURL %>GetNextWorkingDateAndTime?dateString=" + estimatedconstructionendclientname.GetDate().format('MM/dd/yyyy'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                $.cookie("ConstEndDateChanged", 0);
                let closeStartdate = new Date(message);
                let closeEndDate = new Date(GetEndDateByWorkingDays("<%=ajaxHelperPage%>", message, "<%=closeoutperiod%>")); //new Date(new Date(message).valueOf() + 1000 * 3600 * 24 * parseInt('<%=closeoutperiod - 1%>'));
                $('.field_closeoutstartdate_view').text(closeStartdate.format('MMM dd, yyyy'));
                $('.field_closeoutdate_view').text(closeEndDate.format('MMM dd, yyyy'));
                //$('.field_CloseoutDate input').val(closeEndDate.format('MM/dd/yyyy'));
                if (typeof(closeoutdateclientname) != 'undefined') {
                    closeoutdateclientname.SetDate(closeEndDate);
                }
                else {
                    $.cookie("ConstEndDateChanged", 1);
                }
                //$('.field_CloseoutDate input').blur();
                //DataChanged();
            },
            error: function (xhr, ajaxOptions, thrownError) {

            }
        });
    }


    //---------------------------------------------------end 
    function dateChanged(s, e) {
        DataChanged();
    }

    function requestTypeSelectionChanged(s, e) {
        changeRequestOwner();
    }

    function changeRequestOwner() {
        var selectedOption = ASPxClientControl.GetControlCollection().GetByName($(".field_requesttypelookup_edit").attr("id"));
        var selectedId = selectedOption.GetKeyValue();
        var changeAttachment = false;
        var ticketOwnerBinding = moduleSetting.OwnerBinding;
        if (!ticketOwnerBinding)
            ticketOwnerBinding = "Auto";

        var requestTypeID = getValue_RequestType();
        var locationID = getValue_Location();
        if (requestTypeID) {
            //request type mapping is disabled in case of disabled
            if (ticketOwnerBinding != "Disabled") {
                LoadingPanel.Show();
                $.ugit.getRequestTypeDependent(moduleSetting.ModuleName, requestTypeID, locationID, null).done(function (response) {
                    LoadingPanel.Hide();
                    moduleSetting.RequestTypeDetail = response;

                    if (ticketOwnerBinding == "Auto") {
                        setValue_Owner(response.OwnerID, response.Owner);
                        setValue_EstimatedHours(response.EstimatedHours, response.EstimatedHours);
                        setValue_FunctionalArea(response.FunctionalAreaLookup, response.FunctioinalArea);
                        setValue_PRPGroup(response.PRPGroup, response.PRPGroup);
                        setValue_ORP(response.PRPGroup, response.PRPGroup);
                    }
                    else if (ticketOwnerBinding == "ClientSide") {
                        setValue_Owner(response.OwnerID, response.Owner);
                        setValue_EstimatedHours(response.EstimatedHours, response.EstimatedHours);
                        setValue_FunctionalArea(response.FunctionalAreaLookup, response.FunctioinalArea);
                        setValue_PRPGroup(response.PRPGroup, response.PRPGroup);
                        setValue_ORP(response.PRPGroup, response.PRPGroup);
                    }
                });
            }

            if (requestTypeID != "" && requestTypeID > 0) {
                GetIssueType(requestTypeID);
                GetResolutionType(requestTypeID);
            }

            if (changeAttachment) {
                $(".requestTypeHelp").css({ "display": "none" });
                $(".requestTypeAttachment").css({ "display": "none" })
                $(selectedOption).attr("description") == "" ? $(".requestTypeHelp").css({ "display": "none" }) : $(".requestTypeHelp").css({ "display": "block" });
                if ($(selectedOption).attr("isattachment") == "1") {
                    $(".requestTypeAttachment").css({ "display": "block" });
                    $(".requestTypeAttachment").attr("requesttypeid", selectedId);
                }
                else {
                    $(".requestTypeAttachment").css({ "display": "none" });
                    $(".requestTypeAttachment").removeAttr("requesttypeid");
                }
                $(".requestTypeHelp").attr("title", $(selectedOption).attr("description"));
            }
            var incidentOwnerId = "<%=incidentOwnerId %>";
            if (incidentOwnerId != "") {
                //getRequestTypeOwner(selectedId);
                //ExecuteOrDelayUntilScriptLoaded(getRequestTypeOwner, "sp.js");
                //     ExecuteOrDelayUntilScriptLoaded(getRequestTypeOwner, "");
            }
            //Need to ask Amardeep and sayali
            //if (HttpContext.Current.Request.Browser.IsMobileDevice) //mobile
            //{
            //    NewTicketWorkFlowDiv.Visible = false;
            //}
            if (moduleSetting.ModuleName != "CMDB" && ticketOwnerBinding != "Disabled") {
                var curentRequestType = '<%=uGovernIT.Utility.UGITUtility.IsSPItemExist(currentTicket, uGovernIT.Utility.DatabaseObjects.Columns.TicketRequestTypeLookup) ? Convert.ToString(currentTicket[ uGovernIT.Utility.DatabaseObjects.Columns.TicketRequestTypeLookup]): ""%>';
                if ('<%=strStageType%>' == "Assigned" && curentRequestType != requestTypeID) {
                    pcRequestTypeChange.Show();
                }
            }
        }

        // To filter the related request Id drop down in DRQ Module.
        if (selectedOption)
            getModulesOpenTicket(selectedOption.GetValue());
    }

</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(document).ready(function () {
        //var list = document.getElementsByTagName("a");
        $('.uploadedFileContainer').find('a').addClass('hyperLinkIcon');
        var val1 = $('.uploadedFileContainer').find('a').text();
        $('.uploadedFileContainer').find('a').prop('title', val1);
        $('.uploadedFileContainer').find('img').addClass('cancelUploadedFiles');
        // $('#imgCompactView').tooltip();
        //$("#imgCompactView").tooltip({
        //      position: {
        //        my: "left top+15",
        //        at: "right+10"
        //      }
        //});
    });


    var dataChanged = 0;     // global variable flags unsaved changes      
    var ignoreChanges = 0;
    //not required, check in sharepoint
    var mailtoTemplate = "";


    function bindForChange() {
        $.cookie("dataChanged", 0, { path: "/" });

        $('checkbox,radio,select').bind('change', function (event) {
            if (ignoreChanges == 1) {
                ignoreChanges = 0;
                return;
            }

            DataChanged();
        })
        $('input,textarea').bind('keyup', function (event) {
            DataChanged();
        })
        $(".ms-usereditor div").bind('input propertychange', function () {
            DataChanged();
        })
        $(':reset,:submit').bind('click', function (event) {
            dataChanged = 0
            $.cookie("dataChanged", 0, { path: "/" });
        })
    }

    $(function () {
        bindForChange();
        $("input:text").bind("keypress", function (event) {
            return event.keyCode != 13;
        });
        //$(".shortencontainerdesc > div").shorten({ "showChars": 50, "showline": 3 });
        var querystrval = GetParameterValues('UserAction');
        if (querystrval != null) {
            querystrval = querystrval.replace('#done', '');
            if (querystrval.toLowerCase() == 'reject') {
                window.setTimeout(function () { commentsRejectPopup.Show(); }, 1000)
            }
            if (querystrval.toLowerCase() == 'return') {
                window.setTimeout(function () { commentsReturnPopup.Show(); }, 1000)
            }
        }

        <%if (Request["ticketId"] != null)
    {%>
        var ticketid ="<%=Request["ticketId"].Trim()%>";
        var modulename = currentModuleName;
        if (ticketid != "0" && (modulename.toUpperCase() == "CMDB")) {
            BindAssetincidentTicket();
        }
        <%}%>

        $(".dropdownicon").find("img").attr("src", "/Content/Images/arrow_sans_right.png");

        showmandatory();
    });

    var moduleInitialDetail = { TicketID: currentTicketPublicID };
    var collection = null;


    function GetParameterValues(param) {
        var url = window.parent.location.href.slice(window.parent.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] == param) {
                return urlparam[1];
            }
        }
    }

    function showmandatory() {
        try {
            var errorDivID = "errorMsgContainer";
            if (parseInt(currentTicketId) == 0) {
                errorDivID = "Div1";
            }

            $('#' + errorDivID + ' span a').each(function () {
                var errostr = String($.trim($(this).text()) + "*");
                $('.field_box_label').each(function () {
                    var field_name = String($.trim($(this).text()));
                    if (errostr == field_name) {

                        $(this).parent().find('table:first').css("cssText", "border-color: red !important;");
                        $(this).parent().find('textarea').css("cssText", "border-color: red !important; width:100%;");
                        $(this).parent().find('select').css("cssText", "border-color: red !important;");
                    }
                });
            });
        } catch (ex) { }
    }

    function ShowAndHideTimeLineimg() {
        if ($(".TicketRequestor").length > 0) {
            if ($('.timelineopenclose').length > 0)
                $('.timelineopenclose').show();
            else if ($(".timelineopencloseedit").length > 0) {
                $(".timelineopencloseedit").show();
                $(".timelineopencloseedit").css('display', 'inline-flex');
            }
        }
        else {
            if ($('.timelineopenclose').length > 0)
                $('.timelineopenclose').hide();
            else if ($(".timelineopencloseedit").length > 0)
                $(".timelineopencloseedit").hide();
        }
    }

    function ShowRequestorOpenClosedTickets() {
        var peoplePicker = null;
        var peoplepickerValue = null;
        var dxRequestor = ASPxClientControl.GetControlCollection().GetByName($(".field_requestoruser_edit").attr("id"));
        if (dxRequestor)
        //if($("span.Requestor").length>0)
        {
            if (dxRequestor.GetValue() != null && dxRequestor.GetValue() != undefined) {
                peoplepickerValue = dxRequestor.GetValue();
            }
            else {
                peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");

            }
        }
        if ($("span.Owner").length > 0) {
            requestorCtr = $("#" + $("span.Owner").get(0).id + "_upLevelDiv");
            if (requestorCtr.length > 0) {
                // Get new requestor value

                peoplePicker = requestorCtr.get(0);
                var pickerHtml = peoplePicker.innerHTML.toLowerCase();
                if (pickerHtml.indexOf("<span") > -1) {
                    var userSpans = $(peoplePicker).children("span");
                    userSpans.each(function (i, item) {
                        peoplepickerValue = $(item).attr("id");
                        peoplepickerValue = peoplepickerValue.substring(4);
                    });
                }
                else {
                    peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
                }
            }
        }

        if (!peoplepickerValue && $("a.RequestorUser").length > 0) {
            var uObj = $("a.RequestorUser");
            var userID = uObj.attr("usersid");
            var userName = uObj.text();
            if (userID) {
                var url ='<%=openCloseTicketsForRequestorUrl%>' + '&userId=' + userID;
                window.parent.UgitOpenPopupDialog(url, "", userName + " Tickets", 90, 90);
                return;
            }
        }

        if (peoplepickerValue) {
            var url = '<%=openCloseTicketsForRequestorUrl%>' + '&userId=' + peoplepickerValue;
            var name = dxRequestor.GetText() + " Tickets";
            window.parent.UgitOpenPopupDialog(url, "", name, 90, 90);
            return;
        }
    }

    function SendNotification() {
        var cc = document.getElementById('<%=txtNotificationTo.ClientID%>').value;
        var notifyAffectedUsers = $("#<%=chkImpactedUser.ClientID%>").is(":checked");

        if (cc != "" || notifyAffectedUsers) {

            if (verifyEmail()) {
                var incidentId = currentTicketPublicID;
                var expectedResolution = document.getElementById('<%=txtNotificationBody.ClientID%>').value;
                var actionRequiredByUser = "";

                if (document.getElementById('<%=txtActions.ClientID%>')) {
                    var actionRequiredByUser = document.getElementById('<%=txtActions.ClientID%>').value;
                }

                var qData = '{' + '"incidetTicketId":"' + incidentId + '","cc":"' + cc + '","expectedResolution":"' + expectedResolution + '","actionRequiredByUser":"' + actionRequiredByUser + '", "notifyAffectedUsers":"' + notifyAffectedUsers + '"}';
                //          
                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelper %>/SendNotification",
                    data: qData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (message) {

                        var resultJson = $.parseJSON(message.d);

                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }


                });

                document.getElementById('<%=txtNotificationTo.ClientID%>').value = "";
                document.getElementById('<%=txtNotificationBody.ClientID%>').value = "";
                incidentNotificationPopup.Hide();
            }
        }

    }

    function SendNotificationOnResolve(obj, moduleStage, incidentId) {

        //        if (verifyEmail()) {
        var incidentId = currentTicketPublicID;

        var cc = document.getElementById('<%=txtNotificationTo.ClientID%>').value;
        var expectedResolution = document.getElementById('<%=txtNotificationBody.ClientID%>').value;
        var actionRequiredByUser = "";

        if (document.getElementById('<%=txtActions.ClientID%>')) {
            var actionRequiredByUser = document.getElementById('<%=txtActions.ClientID%>').value;
        }
        return true;
        //        }
        //        else {return false;
        //        }
    }

    function verifyEmail() {
        var status = false;
        var valid = true;
        var emailRegEx = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
        if ($.trim(document.getElementById('<%=txtNotificationTo.ClientID%>').value) != "") {
            if ($.trim(document.getElementById('<%=txtNotificationTo.ClientID%>').value).search(emailRegEx) == -1) {
                alert("Please enter a valid email address.");
                valid = false;
            } else {
                valid = true;
            }
        } return valid;

    }

    $(document).ready(function () {
        AutoAdjustActiveTabContainer();
        $(".history_wrap").closest(".field_box").removeClass("field_box");
    });

    try {
        //If stage lable height is less then 20then change top position of label
        $(".alternategraphiclabel").each(function (i, item) {
            var label = $.trim($(item).find("b").html());
            if ($(item).find("b").height() < 20) {
                //If stage label height is less then 20 then change top position of label
                $(item).css("top", "-18px");
            }
        });
    } catch (ex) { }

    function AutoAdjustActiveTabContainer() {

        var tabs = $(".moduleDetailTabsContainer111 li");
        var tabsTxt = $(".moduleDetailTabsContainer111 .menu-item-text");
        var moduleName = "<%= currentModuleName %>";
        for (var j = 0; j <= tabs.length; j++) {
            if ($("#tabPanelContainer_" + (j + 1) + " iframe").length == 1) {
                var totalHeight = GetViewportHeight();
                var calculatedheight = $(".workflowGraphicContainer111").height() + $(".messageConatiner111").height() + $(".moduleDetailTabsContainer111 div").height() + $(".utilityContainer111 div").height();
                var containerHeight = parseFloat(totalHeight) - parseFloat(calculatedheight);
                if ($("#tabPanelContainer_" + (j + 1) + " fieldset").length == 1) {
                    $("#tabPanelContainer_" + (j + 1) + " iframe").height(containerHeight - 50);
                }
                else if ($("#tabPanelContainer_" + (j + 1) + " fieldset").length == 0) {
                    $("#tabPanelContainer_" + (j + 1) + " iframe").height(containerHeight);
                }
                else if ($("#tabPanelContainer_" + (j + 1) + " fieldset").length == 3) {
                    if (moduleName.toLowerCase() == "tsk") {
                        //$("#tabPanelContainer_"+(j+1) +" fieldset").find('iframe').height(containerHeight-300);
                    }

                }
            }
        }
    }


    function CreateIncidentTicket() {

        window.parent.UgitOpenPopupDialog('<%=incidentTicketModulePagePath %>?TicketId=0&SourceTicketId=<%=TicketNoLiteral.Text %>&TargetModuleId=11', "", "Create an Incident from <%=TicketNoLiteral.Text %>", 90, 70);
    }

    function validateFeedbackForm(obj) {
        var isReturn = true;
        if (obj == "Hold") {
            var isValid = true;

            var errorMsgObj = $("#<%=lblHoldMessage.ClientID%>");
            var errors = new Array();

            //var holdCommentAlwaysMandatory = "<%=holdCommentAlwaysMandatory%>";
            var onHoldReason = $("#<%= txtOnHoldReason.ClientID%>").val();

            if (onHoldReason == "Other" && $('#<%=popedHoldComments.ClientID%>').val() == "") {
                errors.push("Please enter comment");
                isValid = false;
            }

            if (aspxdtOnHoldDate.date == null) {
                errors.push("Please enter the Hold Till date");
                isValid = false;
            }

            if (isValid) {
                commentsHoldPopup.Hide();
            }
            else {
                if (errors.length == 1) {
                    errorMsgObj.html(errors.join(""));
                }
                else {
                    errorMsgObj.html("1. " + errors[0] + ", 2. " + errors[1]);
                }
                return isValid;
            }
        }
        else if (obj == "UnHold") {
            if ($('#<%=popedUnHoldComments.ClientID%>').val() == "") {
                $('#<%=lblUnHoldMessage.ClientID%>').html("Please enter comment");
                return false;
            }
            else {
                commentsUnHoldPopup.Hide();
            }
        }
        else if (obj == "Return") {
            if ('<%=TicketRequest.Module.ReturnCommentOptional%>' == 'False') {
                if ($('#<%=popedReturnComments.ClientID%>').val() == "") {
                    $('#<%=lblReturnMessage.ClientID%>').html("Please enter comment");
                    return false;
                }
            }
        }
        else if (obj == "Reject") {
            if (currentModuleName == "CPR" || currentModuleName == "CNS" || currentModuleName == "OPM") {
                if ($('#<%=popedLossRejectComments.ClientID%>').val() == "") {
                    //$('#<=lblRejectMessage.ClientID%>').html("Please enter comment");
                    alert("Please enter comment");
                    return false;
                }
            }
            else {
                if ($('#<%=popedRejectComments.ClientID%>').val() == "") {
                    //$('#<=lblRejectMessage.ClientID%>').html("Please enter comment");
                    alert("Please enter comment");
                    return false;
                }
            }
        }
        else if (obj.getAttribute("Name") == "Award") {
            if ($('#<%=txtCommentsAward.ClientID%>').val() == "") {
                $('#<%=lblAwardMessage.ClientID%>').html("Please enter comment");
                return false;
            }

            if ($("#<%=chkSendAwardNotification.ClientID%>").is(":checked") == true) {
                var toSelected = true;
                var ccSelected = true;
                if ($("#<%=cbIncludeActionUser.ClientID%>").is(":checked") == false) {
                    toSelected = false;
                }


                var vals = ASPxClientControl.GetControlCollection().GetByName("awardGridLookupLookupSearchValue").GetValue()
                if (vals == null || vals.length == 0) {
                    ccSelected = false;
                }

                if (toSelected == false && ccSelected == false) {
                    $('#<%=lblAwardMessage.ClientID%>').html("Check, To (Action User) or Select User(s) or Group(s) from CC");
                    return false;
                }
                LoadingPanel.Hide();
            }
        }

        waitTillUpdateComplete();
        return isReturn;
    }

    function getTicketComment(ticketId, listName, Url) <%--//"It was earlier function parameter" getTicketComment(ticketId,listName,Url)--%> {
    // Url = "/ControlTemplates/uGovernIT/TicketCommentsView.ascx";
       <%-- var editCommentUrl = '<%=editCommentURL%>';--%>
        var editCommentUrl = '<%=editCommentURL%>&ticketId=' + ticketId + "&listName=" + listName + "&ctype=Comment";
        window.UgitOpenPopupDialog(editCommentUrl, '', 'Comment', '70', '60', 0, Url);
        return false;
    }
    function getTicketResolutionComment(ticketId, listName, Url) {

        var editResolutionCommentUrl = '<%=editCommentURL%>&ticketId=' + ticketId + "&listName=" + listName + "&ctype=resolutioncomment";
        window.UgitOpenPopupDialog(editResolutionCommentUrl, '', 'Resolution Comments', '70', '60', 0, Url);
        return false;
    }

    function printSelectedTabs() {
        var arraytab = new Array();
        arraytab = chkSelectTabList.GetSelectedValues();
        if (arraytab.length > 0) {
            var selectedtabs = arraytab.join();
            if (selectedtabs != '') {
                window.open(window.location.href + '&enablePrint=true' + '&selectedTabs=' + selectedtabs + '&isPageBreakup=False');
                aspxPopupPrintOption.Hide();
            }
        }
        else {
            alert("Please select at least one tab.");
            return false;
        }
    }

    function selectAllTab(s, e) {
        if (chkAll.GetValue()) {
            chkSelectTabList.SelectAll();
        }
        else {
            chkSelectTabList.UnselectAll();
        }
    }

    var defaulttab = 1;
    function selectAllTabToPrint(s, e) {
        if (chkSelectTabList.GetItemCount() == chkSelectTabList.GetSelectedValues().length) {
            chkSelectTabList.SelectAll();
            chkAll.SetValue(true);
        }
        else {
            chkAll.SetValue(false);
            if (chkSelectTabList.GetItemCount() > 0 && defaulttab == 1) {
                defaulttab = 0;
                var count = 0;
                for (var i = 0; i < chkSelectTabList.GetItemCount(); i++) {
                    chkSelectTabList.SelectIndices([i]);
                    count++;
                    if (count == 2) {
                        return false;
                    }
                }
            }
        }

    }

    function DataChanged() {
        dataChanged = 1;
        $.cookie("dataChanged", 1, { path: "/" });
    }
</script>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var changedflag = false;

    function autoIframe(frameId, moreSpace) {
        try {

            frame = frameId;
            innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;
            objToResize = (frame.style) ? frame.style : frame;
            objToResize.height = innerDoc.body.scrollHeight + moreSpace;
        } catch (err) {
            window.status = err.message;
        }
    }

    function displayEditblock(currentObj, viewContainerId, EditContainerId) {
        var preFixId = currentObj.id.replace(viewContainerId, "");
        $(currentObj).hide();
        $("#" + preFixId + EditContainerId).show("slow");
    }

    function displayEditblockOnIcon(currentObj, viewContainerId, EditContainerId) {
        var preFixId = $(currentObj).parents("span").get(0).id.replace(viewContainerId, "");
        if ((preFixId + viewContainerId).includes("Attachments")) {
            currentObj.remove();
            $("#" + preFixId + EditContainerId).show("slow");
            $(".updateButtonLI").show();
        }
        else {
            $("#" + preFixId + viewContainerId).hide();
            $("#" + preFixId + EditContainerId).show("slow");
            $(".updateButtonLI").show();
            if (EditContainerId.includes('StudioLookup'))
                callbackStudio();
        }

    }
    function deleteAttachment(TicketID, fileName, anchorElememt, btnDel) {

        var tktID = TicketID;
        var fName = fileName;
        $('#' + anchorElememt).remove();
        $.ajax({
            type: "POST",
            url: "<%= ajaxHelper %>deleteimage",
            data: "{'id':'" + tktID + "','image':'" + fName + "','moduleName': currentModuleName }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });

        $(btnDel).remove();

    }
    function addNotification(msg) {
        if (notifyId == "") {
            //notifyId = SP.UI.Notify.addNotification(msg, true);
        }
    }

    function createBaseline() {
        // $(".createbaselinedesc").val("");
        $("#<%=hiddenCreateBaseline.ClientID%>").val("true");
        addNotification("Please wait, Creating New Baseline");
        HideAllButtons();
        createBaselineContainer.Hide();
        return true;
        //obj.processOnServer = true;
    }

    function CancelcreateBaseline() {
        $(".createbaselinedesc").val("");
        createBaselineContainer.Hide();
    }

    //Show baseline javascript
    var currentBaselineNum = 0;
    function showBaseline() {
        var href = document.location.href;
        if (href.match(/showBaseline=/) != null)
            href = href.replace(/showBaseline=(true|false)/, "");
        if (href.match(/baselineNum=/) != null)
            href = href.replace(/baselineNum=[0-9]+/, "");

        HideAllButtons();
        baselineBoxContainer.Hide();
        document.location.href = href + "&showBaseline=true&baselineNum=" + $(".ddlticketbaselines").val().split("##")[0];
    }

    function removeBaseline() {
        var href = document.location.href;
        if (href.match(/showBaseline=/) != null)
            href = href.replace(/showBaseline=(true|false)/, "");
        if (href.match(/baselineNum=/) != null)
            href = href.replace(/baselineNum=[0-9]+/, "");
        document.location.href = href;
    }

    //Create stageTaskComplete javascript
    var stageTaskCompleteBoxClone = null;
    function markAllTaskComplete() {
        var messageBoard = $("#taskStatusMessage");
        var messageText = $("#<%=taskNames_Pending.ClientID %>").val();
        var messageArray = null;
        var messageContainer = $("#messageBoardContainer");
        var divHeight = 100;
        
        if (messageText != '') {
            var htmlString = new Array();
            messageArray = messageText.split(',:');
            messageArray.sort();
            if (messageArray.length > 0) {
                if (messageArray.length > 4)
                    divHeight = divHeight + 15 * (messageArray.length - 4);
                //messageContainer.height(divHeight);
                if (messageArray.length == 1)
                    htmlString.push("This Task is not complete.");
                else
                    htmlString.push("These Tasks are not complete:");
                htmlString.push("<ul class='ullist'>");

                for (var i = 0; i < messageArray.length; i++) {
                    htmlString.push("<li>");
                    htmlString.push(messageArray[i]);
                    htmlString.push("</li>");
                }
                htmlString.push("</ul>");
                var finalString = htmlString.join(" ");
                messageBoard.html(finalString);
                openStageExitDialog(messageArray);
                //document.getElementById("stageTaskCompleteBox").style.display = "block";
            }
        }
        //messageBoard.text($("#<%=taskNames_Pending.ClientID %>").val());
        stageTaskCompleteBoxClone = $("#stageTaskComplete").parent().html();
        divHeight = divHeight + 70;
        var options = {
            html: document.getElementById("stageTaskComplete"),
            width: 300,
            height: divHeight,
            title: "Task Pending",
            allowMaximize: false,
            showClose: true,
            dialogReturnValueCallback: closeStageTaskCompleteDialog
        };
        //SP.UI.ModalDialog.showModalDialog(options);
        //SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options)
    }
    function openStageExitDialog(titleData) {
        const popupContentTemplate1 = function () {
            let container = $("<div>");
            let upperText = $("<div class='titleText'>These tasks need to be completed before exiting this workflow stage.</div>");
            let loaderDiv = $("<div class='loaderDiv'></div>")

            let datagrid = $("<div id='pendingTasks' class='mt-2'>").dxList({
                dataSource: titleData,
                ID: "grdTemplate",
                height: 250,
                editing: {
                    mode: "cell",
                    allowEditing: true,
                    allowUpdating: true
                },
                sorting: {
                    mode: "multiple" // or "multiple" | "none"
                },
                scrolling: {
                    mode: 'infinite',
                },
                showBorders: false,
                showRowLines: false,
                onCellPrepared: function (e) {
                }
            });
            let confirmBtn = $(`<div class='mt-2 ml-2' style='float:right;font-size: 14px;' />`).dxButton({
                text: "Mark Complete",
                hint: 'Mark Complete',
                icon: 'check',
                visible: true,
                onClick: function (e) {
                    $("#confirmationDialog").dxPopup('instance').hide();
                    closeStageTaskCompleteDialog();
                }
            });

            let cancelBtn = $(`<div class='mt-2 ml-2' style='float:right;font-size: 14px;' />`).dxButton({
                text: "Cancel",
                icon: 'close',
                visible: true,
                onClick: function (e) {
                    $("#confirmationDialog").dxPopup('instance').hide();
                }
            })
            container.append(upperText);
            container.append(loaderDiv);
            container.append(datagrid);
            container.append(confirmBtn);
            container.append(cancelBtn);
            return container;
        };

        const popup = $("#confirmationDialog").dxPopup({
            contentTemplate: popupContentTemplate1,
            width: "650",
            height: "450",
            showTitle: true,
            title: "Pending Stage Exit Criteria",
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            onHiding: function () {
            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => popupContentTemplate1()

        });
        popup.show();
    }
    function closestageTaskCompleteBox() {
        document.getElementById("stageTaskCompleteBox").style.display = "none";
    }
    function closeStageTaskCompleteDialog(s, e) {
        if (stageTaskCompleteBoxClone != null) {
            $("#stageTaskCompleteBox").html(stageTaskCompleteBoxClone);
        }
        addNotification("Please wait..");
        $("#<%=areAllTaskComplete.ClientID %>").val("Completed");
        document.getElementById("<%=approveOnTaskComplete.ClientID %>").click();
        LoadingPanel.SetText("Please Wait ...");
        LoadingPanel.Show();
    }

    function GetComparevalues(s, e) {
        if (s.lastChangedValue != s.lastSuccessValue)
            changedflag = true;

    }
    function UserValueChanged(s, e) {
        var ctrID = document.getElementById(s.name);
        if ($(ctrID).hasClass("field_requestoruser_edit")) {
            FillOnRequestorChange();
        }

        DataChanged();
    }
    function OnCloseUp(s, e) {

    }
    var reqTypeID;
    var reqTypeText;
    function reqTypeChanged() {
        //$('html, body').animate({
        //    scrollTop: $(".btn-count-no").offset().top
        //}, 2000);
    }
    function requestTypeSelectionChanged(s, e) {
        DataChanged();

        changeRequestOwner();
        ShowCount();
        reqTypeChanged();

    }


    function reqGetValues(h) {

        if (h.length == 0)
            return false;
        reqTypeID = h[0][0];
        reqTypeText = h[0][1];

        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName($(".field_requesttypelookup_edit").attr("id"));
        ddEditControl.SetKeyValue(reqTypeID);
        ddEditControl.SetText(reqTypeText)
        ddEditControl.HideDropDown();
        changeRequestOwner();

    }
    function reqGetText(h) {
        reqTypeText = h;
    }

    function rowClick(s, e) {
        var ctrID = document.getElementById(s.name);
        var enableStudioDivisionHierarchy = "<%=enableStudioDivisionHierarchy%>";
        if ($(ctrID).hasClass("field_divisionlookup_edit") && enableStudioDivisionHierarchy == 'True') {
            var dxStudioLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_studiolookup_edit").attr("id"));
            if (dxStudioLookup)
                dxStudioLookup.SetValue('');
            populateStudio();
        }
        if ($(ctrID).hasClass("field_divisionlookup_edit")) {
        }
        if ($(ctrID).hasClass("field_impactlookup_edit") || $(ctrID).hasClass("field_severitylookup_edit")) {
            changePriority();
        }
        if ($(ctrID).hasClass("field_requesttypelookup_edit") || $(ctrID).hasClass("field_locationlookup_edit")) {
            changeRequestOwner();
        }
        if ($(ctrID).hasClass("field_initiatorresolved_edit") || $(ctrID).hasClass("InitiatorResolvedChoice")) {
            enableDisableResolution(s);
        }
        ShowHideRetainageOtherField();
        DataChanged();
    }

    function ShowHideRetainageOtherField() {
        var dxRetainageChoice = ASPxClientControl.GetControlCollection().GetByName($(".field_retainagechoice_edit").attr("id"));
        if (dxRetainageChoice) {
            let value = dxRetainageChoice.GetValue();
            if (value == "Other") {
                $(".field_" + "retainage, .field_" + "Retainage").parent().show();
                if ($(".field_" + "retainage, .field_" + "Retainage").parent().find(".retainageCls").length == 0) {
                    $(".field_" + "retainage, .field_" + "Retainage").parent().find(".field_box_label").append('<b class="retainageCls" style="color:red">* </b>');
                }
                $(".field_retainagechoice_view").text("Other");
            }
            else {
                $(".field_" + "retainage, .field_" + "Retainage").parent().hide();
            }
        }
    }
    function BindAssetincidentTicket() {
        var qData = "{'id': '<%=currentTicketPublicID%>', 'moduleName' : '<%=currentModuleName%>'}";
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelper %>GetAllDetails",
            data: qData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                //var resultvalue = message.d.split(";#");
                var obj = $.parseJSON(message);
                var pcDiv = $("#relatedAssetPopupDiv");
                var pcDivbefore = $("#relatedAssetPopupDivbefore");
                $(".stagerelatedicon").hide();
                var divData = new Array();

                $.each(obj, function (i, s) {
                    $(".stageicon_" + s.key).show();
                    var count = 0;
                    //divData.push("<div class='assetrelatedmapstage' id='assetrelatedmapstage_"+s.key+"'>");
                    var icondiv = new Array();
                    $.each(s.mapt, function (j, sp) {
                        var splitresult = (sp.ticketid).split('^');
                        var ticketid = splitresult[0];
                        //rticket, url, title, ModuleName, rdatetime
                        var url = splitresult[1];
                        var title = unescape(splitresult[2]);
                        var Modulename = splitresult[3];
                        var rdatetime = splitresult[4];
                        var description = unescape(splitresult[5]);
                        var differenceOfDays = splitresult[6];
                        var noOfDays = splitresult[7];
                        icondiv.push("<img id='image_" + ticketid + "' src='/Content/Images/timeline.png' onmouseover='showAssetMapData(this,\"" + ticketid + "\")' style='float:left' />");

                        divData.push("<div class='assetrelatedmapstage' id='assetrelatedmapstage_" + ticketid + "'>");
                        var opendialogurl = "javascript:(window.parent) ? window.parent.UgitOpenPopupDialog(\"" + url + "\",\"TicketId=" + ticketid + "\",\"" + Modulename + " Ticket:" + ticketid + "\",\"auto\",\"auto\") : UgitOpenPopupDialog(\"" + url + "\",\"TicketId=" + ticketid + "\",\"" + Modulename + " Ticket: " + ticketid + "\",\"auto\",\"auto\")";
                        //divData.push("<img id='image_"+ticketid+"' src='/Content/Images/timeline.png' onclick='showAssetMapData(this,"+ticketid+")' style='display:block;' />");

                        divData.push("<div style='cursor:pointer;padding-top:2px;'  onclick='" + opendialogurl + "'><strong>" + rdatetime + ": " + ticketid + " " + "(" + title + ")" + "</strong></br><p>" + description + "</p></div>")
                        divData.push("</div>");

                        count++;
                    });
                    var width = (count * 22) > 120 ? 120 : count * 22;
                    $(".stageicon_" + s.key).width(width + "px");
                    $(".stageicon_" + s.key).html(icondiv.join(""));
                });
                pcDiv.html(divData.join(""));
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // alert(thrownError);
            }

        });

    }


    function checkModuleStageTaskStatus() {
        //var qData = '{"id": currentTicketPublicID ,"currentStep":<%=currentStep%>}';
        var qData = "{'id':'<%=currentTicketPublicID%>', 'image':'','moduleName':'','currentStep':'" + <%=currentStep%> + "'}";
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelper %>StageApproveValue",
            data: qData,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var resultvalue = message.split(";#");
                if (resultvalue[0] === "Pending") {
                    $("#<%=areAllTaskComplete.ClientID %>").val('Pending');
                    $("#<%=taskNames_Pending.ClientID %>").val(resultvalue[1]);
                    markAllTaskComplete();
                    return false;
                }
                else if (resultvalue[0] === "Completed") {
                    $("#<%=areAllTaskComplete.ClientID %>").val('Completed');
                    HideAllButtons();
                    AddNotification("Updating ..");
                    ActionContainer('approvebuttonhidden');
                    //$('#<=approvebuttonhidden.ClientID%>').trigger('click');
                    return true;
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                // alert(thrownError);
                ActionContainer('approvebuttonhidden');
                // $('#<=approvebuttonhidden.ClientID%>').trigger('click');
                return true;
            }

        });


    }

    function approveTicket() {
        if (!performedCloseTicketContraint) {
            //check if need to also close any sub-tickets

            var confirmChildTicketsClose = '<%=this.confirmChildTicketsClose %>';
            if (confirmChildTicketsClose == 'True') {
                $(".txtConfrmToCloseComment").val($(".ResolutionComments").val());

                confirmCloseTicketPopup.Show();

                return false;

            }
        }

        if ($(".InitiatorResolvedChoice").length > 0) {
            var initiatorResolved = ASPxClientControl.GetControlCollection().GetByName(($(".InitiatorResolvedChoice").attr("id")) + "_ListBox");
            if (initiatorResolved) {
                var yesNo = initiatorResolved.GetValue();
                if (yesNo == 'Yes') {
                    resolveInitiator.Show();
                    return false;
                }
            }
        }

        waitTillApproveComplete();
        return false;
    }

    var performedCloseTicketContraint = false;
    function closeCurrentTicketPopupAction(obj, type) {

        $("#<%=hdnCloseTicketType.ClientID%>").val(type)
        performedCloseTicketContraint = true;
        confirmCloseTicketPopup.Hide();
        approveTicket();
    }

    //ORP PRP change Popup handle code :start
    var performedChangeORPOrPRPContraint = false;
    function closeOrpPrpChangePopupAction(obj) {
        performedChangeORPOrPRPContraint = true;
        $("#<%=hdnUpdateButton.ClientID%>").click();
    }

    function ORPorPRPChange() {
        var pRPorORPList = '<%=string.Join(",", this.uPRPorORPList)%>';
        if (pRPorORPList != null) {
            var pRPorORPValues = [];
            var ticketPRPCtr = null;
            var ticketORPCtr = null;
            var peoplePicker = null;
            var peoplepickerValue = null;
            pRPorORPValues.push(pRPorORPList.split(','));
            if ($("span.PRP").length > 0) {
                ticketPRPCtr = $("#" + $("span.PRP").get(0).id + "_upLevelDiv");
                if (ticketPRPCtr.length > 0) {
                    // Get new requestor value

                    peoplePicker = ticketPRPCtr.get(0);
                    var pickerHtml = peoplePicker.innerHTML.toLowerCase();
                    if (pickerHtml.indexOf("<span") > -1) {
                        var userSpans = $(peoplePicker).children("span");
                        userSpans.each(function (i, item) {
                            peoplepickerValue = $(item).attr("id");
                            peoplepickerValue = peoplepickerValue.substring(4);
                            pRPorORPValues.push(peoplepickerValue);
                        });
                    }
                    else {
                        peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
                        pRPorORPValues.push(peoplepickerValue);
                    }
                }
            }

            if ($("span.ORP").length > 0) {
                ticketORPCtr = $("#" + $("span.ORP").get(0).id + "_upLevelDiv");
                if (ticketORPCtr.length > 0) {
                    // Get new requestor value

                    peoplePicker = ticketORPCtr.get(0);
                    var pickerHtml = peoplePicker.innerHTML.toLowerCase();
                    if (pickerHtml.indexOf("<span") > -1) {
                        var userSpans = $(peoplePicker).children("span");
                        userSpans.each(function (i, item) {
                            peoplepickerValue = $(item).attr("id");
                            peoplepickerValue = peoplepickerValue.substring(4);
                            pRPorORPValues.push(peoplepickerValue);
                        });
                    }
                    else {
                        peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
                        pRPorORPValues.push(peoplepickerValue);
                    }
                }
            }

            var check = false;
            //var ORP = ASPxClientControl.GetControlCollection().GetByName('ORPUserLookupSearchValue').GetValue();
            //var PRP = ASPxClientControl.GetControlCollection().GetByName('PRPUserLookupSearchValue').GetValue();
            for (var i = 0; i < pRPorORPValues.length; i++) {

                if (pRPorORPValues[i].indexOf('\\') > -1) {
                    pRPorORPValues[i] = pRPorORPValues[i].replace('\\', '')
                }
                //pRPorORPValues[i] = $.trim(pRPorORPValues[i].replace("&nbsp;", ""));
                //if (pRPorORPList.indexOf(pRPorORPValues[i]) == -1) {
                if (pRPorORPValues[i] != "-1" && pRPorORPValues[i] != "0" && pRPorORPValues[i] != "," && pRPorORPValues[i] != "") {
                    if (changedflag)
                        check = true;
                    break;
                }
            }
            return check;
        }
        return false;
    }

    //ORP PRP change Popup handle code :end


    function waitTillApproveComplete() {
        checkModuleStageTaskStatus();
        return false;
    }
    function waitTillOverrideComplete() {
        HideAllButtons();
        AddNotification("Please wait ..");
        return true;
    }
    function waitTillRejectComplete() {
        HideAllButtons();
        AddNotification("Please wait ..");
        return true;
    }
    function waitTillReturnComplete() {
        HideAllButtons();
        AddNotification("Returning ..");
        return true;
    }
    function waitTillShowReportComplete() {
        HideAllButtons();
        AddNotification("Please wait ..");
        return true;
    }
    function waitTillImportComplete() {
        HideAllButtons();
        AddNotification("Loading ..");
        return true;
    }
    function waitTillUnholdComplete() {
        HideAllButtons();
        AddNotification("Updating ..");
        return true;
    }
    function waitTillUpdateComplete() {
        //ORP PRP change Popup handle :start
        if ("<%= isCurrentStageAssignedStage%>" == "True" && !performedChangeORPOrPRPContraint) {
            if (ORPorPRPChange()) {
                PRPorORPChangePopup.Show();
                return false;
            }
        }
        else {
            PRPorORPChangePopup.Hide();
        }
        //ORP PRP change Popup handle :end
        // autoupdate the tasklist when we click on save button.
        try {
            if ($(".tabcontent11 iframe[src*='TasksList']").length > 0) {
                if ($($(".tabcontent11 iframe[src*='TasksList']").contents()).find('table a[id*=updateTask]').length > 0) {
                    $($(".tabcontent11 iframe[src*='TasksList']").contents()).find('table a[id*=updateTask]').click();
                }
            }
        } catch (ex) { }

        //update close project..
        try {
            if ($(".tabcontent11:visible iframe[src*='CheckListProjectView']").length > 0) {
                if ($($(".tabcontent11:visible iframe[src*='CheckListProjectView']").contents()).find('[id*=btntempsave]').length > 0) {
                    $($(".tabcontent11:visible iframe[src*='CheckListProjectView']").contents()).find('[id*=btntempsave]').click();

                    // commented code to Save changes, when click on Save button, from Check List tab.
                    //return false;
                }
            }

            if ($(".tabcontent11:visible iframe[src*='CRMProjectAllocation']").length > 0) {
                if ($($(".tabcontent11:visible iframe[src*='CRMProjectAllocation']").contents()).find('[id*=btnSaveAllocation]').length > 0) {
                    if (parseInt($.cookie("projTeamAllocSaved")) == 0) {
                        $($(".tabcontent11:visible iframe[src*='CRMProjectAllocation']").contents()).find('[id*=btnSaveAllocation]').click();
                        $.cookie("projTeamAllocSaved", 1, { path: "/" });
                    }
                }
            }

            // TSK module -> Tasks
            if ($(".tabcontent11:visible iframe[src*='NewProjectTask']").length > 0) {
                if ($($(".tabcontent11:visible iframe[src*='NewProjectTask']").contents()).find('[id*=btnSave]').length > 0) {
                    $($(".tabcontent11:visible iframe[src*='NewProjectTask']").contents()).find('[id*=btnSave]').click();
                }
            }

        } catch (ex) { }


        //if( $(".tabcontent11").length >0)
        //{
        //    if($(".tabcontent11 iframe[src*='VendorPMControl']").length>0)
        //    {
        //        var control=$("#"+$(".tabcontent11 iframe[src*='VendorPMControl']"). attr('id')).get(0);
        //        control.contentWindow.saveFormChanges("ParentcallBack", "arguments");
        //        return false;
        //    }
        //}

        AddNotification("Updating ..");
        HideAllButtons();

        return true;
    }

    function ParentcallBack() {
        $("#<%=hdnUpdateButton.ClientID%>").trigger("click");
    }
    function SaveClose() {
        var btn = document.getElementById('<%=hdnUpdateButton.ClientID %>');

        if (btn != null && btn != undefined) {
           <%-- waitTillUpdateComplete();
            // $.globalEval(btn.href);

            $("#<%=hdnUpdateButton.ClientID%>").get(0).click();--%>

            ActionContainer('btnupdateButtonSC');
        }

    }
    function quickCloseTicket(groupName) {
        var isValid = Page_ClientValidate(groupName);
        if (isValid) {
            AddNotification("Updating ..");
            HideAllButtons();
        }
        return isValid;
    }

    function HideAllButtons() {
        $("#btnDiv").css({ 'display': 'none' });
        LoadingPanel.Show();
    }

    function DownloadContractSummaryfile() {
        if ($.cookie("ReportGenerated") == '1') {
            $.cookie("ReportGenerated", 0);
            $("#downloadContractSummaryfile").click(function () {
                $("#downloadContractSummaryfile").attr({
                    target: "_blank",
                    href: "/Content/IMAGES/ugovernit/upload/ContractSummaryReport_" + currentTicketPublicID +".pdf",
                });
            });
            document.getElementById('downloadContractSummaryfile').click();
        }
    }

    //Start
    // Close PMM Ticket and before posting it check whether all task of the project completed or not. 
    // If all tasks have been completed then confirmation msg will be "Do you want to close the project?"
    //If any task is pending to complete then confirmation msg will be "Some project task are in pending stage, do your really want to close project?"
    var taskcollection = null;
    function closePMMTicket(ticketId) {
        closePMMPopUp.Show();
        //try {
        //    var oList = null;
        //    var spContext = new SP.ClientContext.get_current();
        //    var oWebsite = spContext.get_web();
        //    var collList = oWebsite.get_lists();
        //    oList = collList.getByTitle("PMMTasks");

        //    //Create cmal query that fatch all task of ticketid
        //    var camlQuery = new SP.CamlQuery();
        //    camlQuery.set_viewXml('<View><Query><Where><Eq><FieldRef Name=\'TicketPMMIdLookup\' LookupId=\'true\'/><Value Type=\'Lookup\'>' + ticketId + '</Value></Eq></Where></Query><RowLimit>200</RowLimit></View>');
        //    taskcollection = oList.getItems(camlQuery);
        //    spContext.load(taskcollection);
        //    spContext.executeQueryAsync(Function.createDelegate(this, this.onSuccessClosePMMTicket), Function.createDelegate(this, this.onFailClosePMMTicket));
        //} catch (ex) { }
    }

    function onSuccessClosePMMTicket(sender, args) {
        var isTaskCompleted = true;
        if (taskcollection != null) {
            var listItemEnumerator = taskcollection.getEnumerator();
            while (listItemEnumerator.moveNext()) {
                var oListItem = listItemEnumerator.get_current();
                if (oListItem.get_item("Status") != "Completed") {
                    isTaskCompleted = false;
                }
            }
        }

        closePMMPopUp.Show();
    }
    function ClosePMM() {
        LoadingPanel.SetText("Please Wait ...");
        LoadingPanel.Show();
        $('#<%=actionEventID.ClientID%>').val('ClosePMM');
        $('#<%=btnGroupActioner.ClientID%>').click();
        <%--eval($("#<%=btncloseButtonH.ClientID %>").attr("href"));--%>
    }
    function onFailClosePMMTicket(sender, args) {
        alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
    }

    function WaitTillSave() {
        addNotification("Saving");
        var createButtons = $("#createButtons");
        var li2 = $("#<%=createCloseButtonContainer.ClientID %>");
        createButtons.css("display", "none");
        li2.css("display", "none");
        LoadingPanel.SetText("Please Wait ...");
        LoadingPanel.Show();
    }

    function openForPrintDetail() {
        try {
            selectAllTabToPrint();
            //chkSelectTabList.SelectAll();
            aspxPopupPrintOption.Show();

        }
        catch (ex) { }
    }

    //function showhideReportMenu(obj) {
    //    if ($("#report-options").hasClass("selected-export")) {
    //        $("#report-options").hide(300);
    //        $("#report-options").removeClass("selected-export");
    //    }
    //    else {
    //        $("#report-options").show(300);
    //        $("#report-options").addClass("selected-export");
    //    }
    //}

    /* PMM Budget Report */
    function OpenBudgetReportPopup() {
        //showhideReportMenu(obj);
        var moduleName = "<%=currentModuleName%>";
        var path = "";
        var title = "";

        if (moduleName == "PMM") {
            path = "<%=pmmBudgetReportUrl %>";
            title = "Project Budget Summary Report";
        }
        else if (moduleName == "ITG") {
            path = "<%=itgBudgetReportUrl %>";
            title = "Non-Project Budget Summary Report";
        }

        var currentTicketId = "<%= currentTicketId%>";
        window.parent.UgitOpenPopupDialog(path, "PMMId=" + currentTicketId, title, 80, 90, false, escape(window.location.href));
        return false;
    }

    function OpenActualsReportPopup() {
        // showhideReportMenu(obj)
        var moduleName = "<%= currentModuleName%>";
        var path = "";
        var title = "";

        if (moduleName == "PMM") {
            path = "<%=pmmActualsReportUrl %>";
            title = "Project Actuals Report";
        }
        else if (moduleName == "ITG") {
            path = "<%=itgActualsReportUrl%>";
            title = "Non-Project Actuals Report";
        }

        var currentTicketId = "<%= currentTicketId%>";

        window.parent.UgitOpenPopupDialog(path, "PMMId=" + currentTicketId, title, 80, 90, false, escape(window.location.href));
        return false;
    }

    function openProjectReport() {
        //showhideReportMenu(obj);
        var moduleName = '<%=currentModuleName%>';
        if (moduleName == "PMM") {
            var projectReportUrl = '<%=pmmReportUrl%>';
            window.parent.UgitOpenPopupDialog(projectReportUrl, '', 'Project Status Report', '600px', '600px');
        }
        else if (moduleName == "TSK") {
            var projectReportUrl = '<%=tskReportUrl %>';
            window.parent.UgitOpenPopupDialog(projectReportUrl, '', 'Project Status Report', '600px', '400px');
        }
        return false;
    }

    function openResourceReport() {
        //showhideReportMenu(obj);
        var projectReportUrl = '<%=pmmResourceReportUrl%>';
        window.parent.UgitOpenPopupDialog(projectReportUrl, '', 'Resource Hours Report', '90', '90');
        return false;
    }

    function openEstimatedRemainingHoursReport() {
        // showhideReportMenu(obj);
        var projectERHReportUrl = '<%=pmmprojectERHReportUrl%>';
        window.parent.UgitOpenPopupDialog(projectERHReportUrl, '', 'Estimated Remaining Hours Report', '90', '90');
        return false;
    }


    //function reportItemMouseOver(obj) {
    //    $(obj).removeClass("ugitlinkbg");
    //    $(obj).addClass("ugitsellinkbg");
    //}

    //function reportItemMouseOut(obj) {

    //    $(obj).removeClass("ugitsellinkbg");
    //    $(obj).addClass("ugitlinkbg");

    //}
    //Added by mudassir 10 march 2020
    function ShowhideDependent(obj) {
        if (obj.checked)
            $('.clsshowhide').show();

        else
            $('.clsshowhide').hide();
    }
    //
    function ChangeCurrentStage() {
        var lifeCycleStageUrl = '<%=LifeCycleStageURL%>&projectId=<%=currentTicketId%>&publicTicketId=<%=currentTicketPublicID%>';
        window.parent.UgitOpenPopupDialog(lifeCycleStageUrl, '', '<%=currentTicketPublicID%>' + ': Project Lifecycle', '50', '50');
        return false;
    }

    function showeditbuttonOnhover(obj) {
        $($(obj).find('img')).css('display', 'block');
    }
    function hideeditbuttonOnhover(obj) {
        $($(obj).find('img')).css('display', 'none');
    }

    function OpenCreateTemplateForm() {
        window.parent.UgitOpenPopupDialog("<%=SaveAsTemplateURL %>&ModuleName=<%=currentModuleName %>&currentTicketPublicID=<%=currentTicketPublicID %>", "", "New Template", "950px", "600px");
        return false;
    }

    function showEditItem(obj, id) {
        var editContainer = $("[id*=" + id + "]");
        if (obj.checked) {
            editContainer.show();
        }
        else {
            editContainer.hide();
        }

        editContainer.find("input:text").val('');
        if (editContainer.find(".ms-usereditor").length > 0) {
            var userEdit = editContainer.find(".ms-usereditor");
            var userEditSpan = $("#" + userEdit.get(0).id + "_upLevelDiv");
            if (userEditSpan.length > 0) {
                userEditSpan.get(0).innerHTML = "";
            }
        }
        editContainer.find("textarea").val('');
        editContainer.find("select").val('');
    }


    function EditBatchControl(currentObj, viewContainerId, EditContainerId, hiddenID) {

        $("input[id$=" + hiddenID + "]").val("False");
        $($($(currentObj).parents("td:first").children("span")).get(0)).find('input').val("");
        $($($(currentObj).parents("td:first").children("span")).get(0)).find('input').prop("disabled", false);
        $($(currentObj).parents("tr:first").children("td").get(0)).hide();
        $($(currentObj).parents("tr:first").children("td").get(1)).show();
    }

    function DefaultValueforBatchEditing(currentObj) {
        $(currentObj).val("<Value Varies>");
    }

    function showAssetMapData(obj, stageStep) {
        relatedassetticket.Hide();
        $(".assetrelatedmapstage").hide();
        $("#assetrelatedmapstage_" + stageStep).show();
        relatedassetticket.ShowAtElement(obj);
    }


    var isCategoryDropdownChanged = false;
    function requestTypeCategoryChange(obj) {
        requestTypeCallBackPanel.PerformCallback($(obj).val());
    }
    function requestTypeCallBackPanel_OnEndCallback(s, e) {
        isCategoryDropdownChanged = true;
        $(".RequestTypeLookup").trigger("change");

    }
    function setIframeControlChecks(controlID, val) {

        var hiddenCtr = $(".framepanel input:hidden[id $= '" + controlID + "_mandatory']");
        hiddenCtr.val(val);

    }

    $(function () {
        if ($("#errorMsgContainer span").text().trim() == "") {
            if ($.cookie("ShowTicketStage") == '0') {
                HideStages();
            }
            else {
                ShowStages();
            }
        }
        else {
            ShowStages();
        }

        $('#workflowContainer').hover(
            function () {
                $("#imgHideShowStage").show();
            }, function () {
                if ($('#workflowContainer, #ticketStatusInfo').is(':hidden') == false) {
                    $("#imgHideShowStage").hide();

                }
            });

        $('#<%=dvStageHeader.ClientID%>').hover(
            function () {
                $("#imgHideShowStage").show();

            }, function () {
                if ($('#workflowContainer, #ticketStatusInfo').is(':hidden') == false) {
                    $("#imgHideShowStage").hide();
                }
            });

    });

    function HideShowStages(obj) {
        //var ctr = $('#<%=topGraphicPanel.ClientID%>');
        var ctr = $('#workflowContainer, #ticketStatusInfo');
        if (ctr.is(':hidden') == true) {
            ShowStages();
            $.cookie("ShowTicketStage", 1, { expires: 9999 });
        }
        else {
            HideStages()
            $.cookie("ShowTicketStage", 0, { expires: 9999 });
        }
        AutoAdjustActiveTabContainer();
    }


    function ShowStages() {
        //$('#<%=topGraphicPanel.ClientID%>').show();
        $('#workflowContainer, #ticketStatusInfo').show();
        $("#imgHideShowStage").hide();
        $("#imgHideShowStage").attr('src', '/Content/Images/minus-square.png');
        $("#imgHideShowStage").css('position', 'absolute');
        $("#imgHideShowStage").css('top', '-70px');
        $("#imgHideShowStage").css('left', $('#<%=topGraphicPanel.ClientID%>').width() - 200 + 'px');
        $('#<%=dvStageHeader.ClientID%>').css('border-bottom', 'none');
        $('#<%=dvStageHeader.ClientID%>').css('padding-bottom', '0px');
        //$('#<%=dvStageHeader.ClientID%>').css('margin-left', '0px');
        $('#<%=dvStageHeader.ClientID%>').css('margin-bottom', '0');
        $("#spStageTitle").hide();

    }

    function HideStages() {
        //$('#<%=topGraphicPanel.ClientID%>').hide();
        $('#workflowContainer, #ticketStatusInfo').hide();
        $("#imgHideShowStage").show();
        $("#imgHideShowStage").css('position', 'static');
        //$("#imgHideShowStage").css('top', '5px');
        //$("#imgHideShowStage").css('margin-left', '5px');
        //$("#imgHideShowStage").css('left', '0px');
        $("#imgHideShowStage").attr('src', '/Content/Images/plus-square.png');
        $('#<%=dvStageHeader.ClientID%>').css('border-bottom', '1px solid #D8D8D8');
        $('#<%=dvStageHeader.ClientID%>').css('padding-bottom', '10px');
        //$('#<%=dvStageHeader.ClientID%>').css('margin-left', '2px');
        $('#<%=dvStageHeader.ClientID%>').css('margin-bottom', '15px');
        $("#spStageTitle").show();

    }

    function onSetPriorityElevated(chkObj) {
        if (chkObj.checked) {
            $(".priority").html("<%= elevatedPrioirty%>");

        }
        else {
            changePriority();
        }
    }

    function popupMenuActionMenuItemClick(s, e) {

        if (e.item.parent.name == "Macro") {
            $("#<%=hdnMacroTicketTemplate.ClientID%>").val(e.item.name);
            $("#<%=tempbutton.ClientID%>").click();
        }
        else {
            $("#<%=hdnMacroTicketTemplate.ClientID%>").val("");
            if (e.item.name == "Print")
                openForPrintDetail();
            else if (e.item.name == "Comment")
                commentsAddPopup.Show();
            else if (e.item.name == "NewBaseline")
                createBaselineContainer.Show();
            else if (e.item.name == "ShowBaseline")
                baselineBoxContainer.Show();
            else if (e.item.name == "Notify")
                incidentNotificationPopup.Show();
            else if (e.item.name == "Putonhold")
                commentsHoldPopup.Show();
            else if (e.item.name == "PutonUnhold")
                commentsUnHoldPopup.Show();
            else if (e.item.name == "SaveasTemplate")
                OpenCreateTemplateForm();
            else if (e.item.name == "CloseProject")
                closePMMTicket(currentTicketId);
            else if (e.item.name.indexOf("SendAgentLink") != -1)
                OnbtnSendAgentLink(e.item.name);
            else if (e.item.name.indexOf("AgentSubItem") != -1)
                ShowAgents(e.item.name, e.item.GetText());
            else if (e.item.name == "ContractSummary")
                callbackcontrol.PerformCallback();
            <%--else if (e.item.name == "SendToProcore") {
                $("#<%=btnSendToProcore.ClientID%>").click();
            }--%>

            else if (e.item.name == "MoveToPrecon") {
                MoveToPreconTicket();
            }
            else if (e.item.name == "AddAllocation") {
                AddAllocation();
            }
            else if (e.item.name == "NewOpportunity") {
                NewOpportunityFromLead();
            }
            else if (e.item.name == "NewCompany") {
                NewCompany();
            }
            else if (e.item.name == "NewContact") {
                NewContact();
            }
            else if (e.item.name == "NewPermit")
                OnBtnNewPermitClick();
            else if (e.item.name == "Escalation")
                OnbtnBatchEscalationClick();
            else if (e.item.name == "CopyLinktoClipboard")
                copyToClipboard();
            else if (e.item.name == "APPaymentEmail")
                ticketEmail('appaymentcall');

            else if (e.item.name == "AgentProcore") {
                OpenAgentProcoreForm();
            }
            else if (e.item.name == "ProjectExternal") {
                OpenProjectExternal();
            }
            else if (e.item.name == "UpdateProjectStatistics") {
                UpdateProjectStatistics();
            }
<%--            else if (e.item.name == "Metric") {
                $("#<%=btnMetricSync.ClientID%>").click();
            }--%>
            else if (e.item.name == "EscalationEmail") {
                OnbtnBatchEscalationEmailClick();
            }
            else if (e.item.name == "CompactView")
                showCompactView();
        }
    }

    function UpdateProjectStatistics() {
        busyLoader.Show();
        var params = "Action=UpdateStatistics" + "&TicketIDs=" + currentTicketPublicID;
        ASPxCallbackPanel_Actions.PerformCallback(params);
    }

    function OnbtnBatchEscalationEmailClick(s, e) {
        var param = "ids=" + currentTicketPublicID + "&ModuleName=" + currentModuleName;
        window.parent.UgitOpenPopupDialog('<%= TicketManualEscalationUrl %>', param + "&Notification=initialemail", 'Send Email', '80', '90', 0, escape(AbsolutePath).toLowerCase());
    }

    function NewCompany() {
        var params = "hpac=1&TicketId=0";
        window.parent.parent.UgitOpenPopupDialog('<%=NewCompanyUrl%>', params, 'New Company', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }

    function NewContact() {
        var params = "hpac=1&TicketId=0";
        window.parent.parent.UgitOpenPopupDialog('<%=NewContactUrl%>', params, 'New Contact', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
    }



    function OpenProjectExternal() {
        var activeTabIndex = tbcDetailTabs.GetActiveTabIndex();
        var tab = tbcDetailTabs.GetTab(activeTabIndex);

        $.cookie("TicketSelectedTab", tab.name, { expires: 9999 });

        window.parent.parent.UgitOpenPopupDialog("<%=strProjectExternalUrl %>", "", 'Assign External Project Team', '60', '40');
        return false;
    }

    function showCompactView() {
        window.location.href = '<%= compactViewUrl%>';
    }

    function copyToClipboard() {
        $('#lblticketUrl').val('<%=clipboardUrl%>');

        if (navigator.appVersion.indexOf("Mac") != -1)
            aspxPopupCopyToClipboard.SetHeaderText("Press <span style='font-size:16px;position:relative;top:2px;'>&#x2318;</span>-C to copy");

        aspxPopupCopyToClipboard.Show();
    }

    function autoSelect() {
        $('#lblticketUrl').trigger('click');
    }

<%--function OpenAgentProcoreForm()
{
    var param = "TicketId=" + "<%=currentTicketPublicID %>&ProjectName=" +  "<%=ticketTitle %>&ProjectNumber=" + "<%=ticketProjectNumber %>" ;
    var title = "uCOREM Agent - moving project " + '<%=ticketProjectNumber %>' +" " + '<%=ticketTitle %>'  +" to Procore";
    window.parent.parent.UgitOpenPopupDialog("<%=AgentProjectFieldsUrl %>",param, title, '60', '90');
}--%>

    function NewOpportunityFromLead() {
        var param = "hpac=0&LeadTicketId=" + currentTicketPublicID;
        $(".HiddenFieldLead").val(currentTicketPublicID);
        GetOpenTasksCount(currentTicketPublicID);
        if (OpenTaskCount > 0) {
            var customDialog = DevExpress.ui.dialog.custom({
                title: "Open Tasks Alert",
                message: "You have some pending Tasks. Please select one of the options below.",
                buttons: [
                    { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                    { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                    { text: "Cancel", onClick: function () { return "Cancel" } }
                ]
            });
            customDialog.show().done(function (dialogResult) {
                if (dialogResult == "CloseTasks") {
                    param = param + "&CompleteTasks=true";
                }
                else if (dialogResult == "KeepTasksOpen") {
                    param = param + "&CompleteTasks=false";
                }
                else
                    return false;

                window.parent.UgitOpenPopupDialog('<%=NewOpportunityUrl%>', param, 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            });
        }
        else {
            param = param + "&CompleteTasks=false";
            window.parent.UgitOpenPopupDialog('<%=NewOpportunityUrl%>', param, 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
        }

            //window.parent.parent.UgitOpenPopupDialog('<%=NewOpportunityUrl%>', param, 'New Opportunity', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");

        //$(".HiddenFieldLead").val(currentTicketPublicID);
    }

    function OnbtnBatchEscalationClick(s, e) {
        var param = "ids=" + currentTicketPublicID + "&ModuleName=" + currentModuleName;
        if (currentModuleName == "CPR" || currentModuleName == "OPM" || currentModuleName == "CNS" || currentModuleName == "LEM" || currentModuleName == "CPP" || currentModuleName == "CCM") {
            window.parent.UgitOpenPopupDialog('<%= TicketManualEscalationUrl %>', param + "&Notification=award", 'Send Email', '600px', '500px', 0, escape(AbsolutePath).toLowerCase());
        }
        else {
            window.parent.UgitOpenPopupDialog('<%= TicketManualEscalationUrl %>', param, 'Send Email', '600px', '500px', 0, escape(AbsolutePath).toLowerCase());
        }
    }

      <%--  function OnBtnNewPermitClick() {
            var params = "CPRTicketId=<%=currentTicketPublicID%>";
            window.parent.parent.UgitOpenPopupDialog('<%=NewPermitUrl%>', params, 'New Permit', '90', '90', false, "<%=Server.UrlEncode(Request.Url.AbsolutePath) %>");
            return false;
        }--%>


    function SetSelectedTab(SelectedTab) {
        var selectedValue = SelectedTab.value;
        var completeurl = '';

        var url = '<%=clipboardUrl%>';
        var moduleName = currentModuleName;
        if (moduleName != null && moduleName != "undefined" && moduleName != "") {
            if (selectedValue !== '0') {
                completeurl = url + '&showTab=' + selectedValue;
            }
            else {
                completeurl = url;
            }

            $('#lblticketUrl').val(completeurl);
        }
    }


    function autoSelect() {
        $('#lblticketUrl').trigger('click');
    }
    function ShowAgents(agentProp) {
        var agentName = agentProp.split("#")[2];
        var agentId = agentProp.split("#")[1];
        var agentPath = '<%= ServiceURL%>' + agentId;
        var param = "TicketId=" + currentTicketPublicID + "&ModuleName=" + currentModuleName;
        window.parent.UgitOpenPopupDialog(agentPath, param, "Agent: " + agentName + "", 90, 90, false, escape(window.location.href));
        return false;
    }

    function OnbtnSendAgentLink(agentlink) {
        var agentName = agentlink.split("#")[2];
        var agentID = agentlink.split("#")[1];
        var agentSendLink = '<%=TicketManualEscalationUrl%>';
        var params = "sendagentlink=1&ids=" + currentTicketPublicID + "&ModuleName=" + currentModuleName + "&agentID=" + agentID + "";
        window.parent.UgitOpenPopupDialog(agentSendLink, params, "Agent: " + agentName + "", 90, 90, false, escape(window.location.href));
    }

    function AddAllocation() {
        var param = "&ticketId=" + currentTicketPublicID;
        window.parent.UgitOpenPopupDialog("<%=NewAllocationUrl %>", param, "New Allocation", "600px", "430px");
        return false;
    }

    function showhideStageToMove() {
        if ($("#<%=chkboxStageMoveToPrecon.ClientID%>").prop("checked") == true) {
            $("#<%=trddlStageToMove.ClientID%>").css("visibility", "visible");

            if ($("#<%=hndOpsTitle.ClientID%>").val().length > 0)
                $("#<%= txtStatusMailSubject.ClientID %>").val('Assign to PreCon' + ': ' + $("#<%=hndOpsTitle.ClientID%>").val());
            else
                $("#<%= txtStatusMailSubject.ClientID %>").val('Assign to PreCon');
        }
        else {
            $("#<%=trddlStageToMove.ClientID%>").css("visibility", "hidden");

            if ($("#<%=hndOpsTitle.ClientID%>").val().length > 0)
                $("#<%= txtStatusMailSubject.ClientID %>").val('Opportunity ' + $("#<%=ddlOpportunityStatus.ClientID%>").val() + ': ' + $("#<%=hndOpsTitle.ClientID%>").val());
            else
                $("#<%= txtStatusMailSubject.ClientID %>").val('Opportunity ' + $("#<%=ddlOpportunityStatus.ClientID%>").val());
        }
    }

    function MoveToPreconTicket() {

        statusPopup.Show();
        LoadingPanel.Show();
        statusPopup.PerformCallback();
    }

    function ActionContainer(senderValue) {

        if (senderValue != null && senderValue != undefined) {
            if (senderValue == 'btnsuperAdminEditButton') {
                if (waitTillOverrideComplete() == true) {
                    $('#<%=actionEventID.ClientID%>').val('superAdminEditButton');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btnquickCloseTicket') {
                if (quickCloseTicket('ActualHours') == true) {
                    $('#<%=actionEventID.ClientID%>').val('quickCloseTicket');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'addCommentBt') {
                if ($('#<%=txtAddComment.ClientID%>').val().trim() == '') {
                    alert('Comment required.')
                    return false;
                }
                LoadingPanel.Show();
                $('#<%=actionEventID.ClientID%>').val(senderValue);
                $('#<%=btnGroupActioner.ClientID%>').click();
            }
            else if (senderValue == 'btnupdateButton') {


                if (waitTillUpdateComplete()) {
                    $('#<%=actionEventID.ClientID%>').val('updateButton');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btnupdateButtonSC') {


                if (waitTillUpdateComplete()) {
                    $('#<%=actionEventID.ClientID%>').val('updateButtonSC');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btnselfAssignConfirm') {
                if (waitTillUpdateComplete() == true) {
                    $('#<%=actionEventID.ClientID%>').val('selfAssignConfirm');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btncreateBaselineButtonH') {
                if (createBaseline() == true) {
                    $('#<%=actionEventID.ClientID%>').val('createBaselineButtonH');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'rejectWithCommentsButton') {
                commentsRejectPopup.Show();
            }
            else if (senderValue == 'returnWithCommentsButton') {
                commentsReturnPopup.Show();
            }
            else if (senderValue == 'approve') {
                approveTicket();
            }
            else if (senderValue == 'btncloseButtonH') {
                if (waitTillUpdateComplete() == true) {
                    $('#<%=actionEventID.ClientID%>').val('closeButtonH');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'returnButtond') {
                if (validateFeedbackForm('Return') == true) {
                    $('#<%=actionEventID.ClientID%>').val('returnButton');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btnHoldButton') {
                if (validateFeedbackForm('Hold') == true) {
                    $('#<%=actionEventID.ClientID%>').val('HoldButton');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btnUnHoldButton') {
                if (validateFeedbackForm('UnHold') == true) {
                    $('#<%=actionEventID.ClientID%>').val('UnHoldButton');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else if (senderValue == 'btnrejectButton') {
                if (validateFeedbackForm('Reject') == true) {
                    $('#<%=actionEventID.ClientID%>').val('rejectButton');
                    $('#<%=btnGroupActioner.ClientID%>').click();
                }
            }
            else {
                LoadingPanel.Show();
                $('#<%=actionEventID.ClientID%>').val(senderValue);
                $('#<%=btnGroupActioner.ClientID%>').click();
            }
        }
    }
    function StatusChange() {
        if ($("#<%=ddlOpportunityStatus.ClientID%>").val() == "Awarded" || $("#<%=ddlOpportunityStatus.ClientID%>").val() == "Precon") {
            $("#<%=trStageToMove.ClientID%>").css("visibility", "visible");
            $("#<%=trddlStageToMove.ClientID%>").css("visibility", "visible");

            if (!$("#<%=chkboxStageMoveToPrecon.ClientID%>").attr('disabled'))
                $("#<%=chkboxStageMoveToPrecon.ClientID%>").prop('checked', true);

            if ($("#<%=hndOpsTitle.ClientID%>").val().length > 0)
                $("#<%= txtStatusMailSubject.ClientID %>").val('Assign to PreCon' + ': ' + $("#<%=hndOpsTitle.ClientID%>").val());
            else
                $("#<%= txtStatusMailSubject.ClientID %>").val('Assign to PreCon');
        }
        else {
            $("#<%=trddlStageToMove.ClientID%>").css("visibility", "hidden");
            $("#<%=trStageToMove.ClientID%>").css("visibility", "hidden");
            $("#<%=chkboxStageMoveToPrecon.ClientID%>").prop('checked', false);
            if ($("#<%=hndOpsTitle.ClientID%>").val().length > 0)
                $("#<%= txtStatusMailSubject.ClientID %>").val('Opportunity ' + $("#<%=ddlOpportunityStatus.ClientID%>").val() + ': ' + $("#<%=hndOpsTitle.ClientID%>").val());
            else
                $("#<%= txtStatusMailSubject.ClientID %>").val('Opportunity ' + $("#<%=ddlOpportunityStatus.ClientID%>").val());

        }

        if ($("#<%=ddlOpportunityStatus.ClientID%>").val() == "Awarded" || $("#<%=ddlOpportunityStatus.ClientID%>").val() == "Cancelled"
            || $("#<%=ddlOpportunityStatus.ClientID%>").val() == "Declined" || $("#<%=ddlOpportunityStatus.ClientID%>").val() == "Lost") {
            $("#<%=trTicketAwardLossDate.ClientID%>").css("visibility", "visible");
        }
        else {
            $("#<%=trTicketAwardLossDate.ClientID%>").css("visibility", "hidden");
        }

    }

    function OpenStagePopup(buttonName) {
        $('#<%=liStatus.ClientID%>').css('display', '');
        $('#<%=liMoveToPrecon.ClientID%>').css('display', 'none');
        $('#trStatus').css('display', '');
        statusPopup.Show();
        LoadingPanel.Show();
        StatusButtonName = buttonName;
        
        if (StatusButtonName == "statusButton") {
            statusPopup.SetHeaderText("Opportunity Lost");
            $(".classAwardLoss").text("Loss");
        }
        else if (StatusButtonName == "advanceToProject") {
            statusPopup.SetHeaderText("Opportunity Awarded");
            $(".classAwardLoss").text("Award");
        }
        statusPopup.PerformCallback(buttonName);

        if ($("#<%=ddlOpportunityStatus.ClientID%>").val() == "Awarded" || $("#<%=ddlOpportunityStatus.ClientID%>").val() == "Precon") {
            $("#<%=trStageToMove.ClientID%>").css("visibility", "visible");
            $("#<%=chkboxStageMoveToPrecon.ClientID%>").prop('checked', true);
            $('#<%=chkStatusOpenCPR.ClientID%>').prop('checked', false);
            $('#trStatusMailTo').css('display', '');
        }
        else {
            $("#<%=trStageToMove.ClientID%>").css("visibility", "hidden");
            $("#<%=chkboxStageMoveToPrecon.ClientID%>").prop('checked', false);
        }

        return false;
    }


    function AwardMessageChanges() {
        var emailBody = EmailHtmlBody.GetHtml();
        var commenttext = $("#<%=txtCommentsAward.ClientID%>").val();
        emailBody = emailBody.replace("[Your content goes here]", commenttext);


        if ($("#<%=hdnAwardLossComments.ClientID%>").val() != "")
            emailBody = emailBody.replace($("#<%=hdnAwardLossComments.ClientID%>").val(), commenttext);

        $("#<%=hdnAwardLossComments.ClientID%>").val(commenttext);
        EmailHtmlBody.SetHtml(emailBody);

    }

    function StatusMessageChanges() {
        var emailBody = txtStatusMailBody.GetHtml();
        var commenttext = txtStatusMessage.GetText();  // $("#<=txtStatusMessage.ClientID%>").val();

        emailBody = emailBody.replace("[Your content goes here]", commenttext);
        emailBody = emailBody.replace('<span id="mailBodyComment"></span>', "<span id='mailBodyComment'>" + commenttext + "</span>");

        if ($("#<%=hndOpportunityStatus.ClientID%>").val() != "")
            emailBody = emailBody.replace($("#<%=hndOpportunityStatus.ClientID%>").val(), commenttext);

        $("#<%=hndOpportunityStatus.ClientID%>").val(commenttext);
        txtStatusMailBody.SetHtml(emailBody);

    }

    function ReasonMessageChanges() {
        
        var emailBody = txtStatusMailBody.GetHtml();
        var reasontext = txtStatusResson.GetText();

        emailBody = emailBody.replace("[Your content goes here]", reasontext);
        emailBody = emailBody.replace('<span id="mailBodyComment"></span>', "<span id='mailBodyComment'>" + reasontext + "</span>");

        txtStatusMailBody.SetHtml(emailBody);
    }

    function GridLookupLoss_EndCallback(s, e) {
        DisplaySearchImageForLoss();
    }

    function DisplaySearchImageForLoss() {
        $("[id$='_trLossEmailNotification']").each(function (i, item) {
            if ($(item).find(".dxgvCommandColumn_UGITClassicDevEx ").length > 0) {
                $($(item).find(".dxgvCommandColumn_UGITClassicDevEx ").get(0)).append('<img class="magnifier" style="cursor: pointer; " src="/Content/images/uGovernIT/search-black.png" alt="Search">');
            }
        });
    }

    function CloseMoveToPrecon() {

        //window.frameElement.commitPopup();
        //ASPXPopupMovePrecon.Hide();
        statusPopup.Hide();
        return false;
    }
    var OpenTaskCount = 0;
    function StatusButtonClicked(s, e) {
        
        if (ERPJobIDNC.GetText() == '' && '<%=IsCMICMandatory%>' == 'True') {
            alert('Cannot create CPR record from OPM due to ' + $("#<%=hndERPJobID.ClientID%>").val() + ' is missing.');
            e.processOnServer = false;
            return false;
        }

        LoadingPanel.Show();
        let opportunityStatus = $("#<%=ddlOpportunityStatus.ClientID%>").val();
        if (StatusButtonName == "statusButton")
            opportunityStatus = "Lost";
        else if (StatusButtonName == "advanceToProject")
            opportunityStatus = "Awarded";
        $("#<%=hndOpportunityStatus.ClientID%>").val(opportunityStatus);
        $("#<%=hdnStagetoMove.ClientID%>").val($("#<%=ddlStageToMove.ClientID%>").val());

        let resonforChange = txtStatusResson.GetText();

        if (opportunityStatus == "Lost") {
            if (resonforChange == null || resonforChange == '' || typeof (resonforChange) == "undefined") {
                alert('Please Enter Reason');
                LoadingPanel.Hide();
                e.processOnServer = false;
                return false;
            }
        }

        if ($("#<%=chkOPMEmail.ClientID%>").get(0).checked) {
            var vals = ASPxClientControl.GetControlCollection().GetByName("statusMTPUserGroupsLookupSearchValue").GetValue()
            //var vals = statusMTPUserGroups.GetValue();
            if (vals == null || vals.length == 0) {
                alert("select Mail To");
                LoadingPanel.Hide();
                //ASPxClientControl.GetControlCollection().GetByName("statusMTPUserGroupsLookupSearchValue").SetFocus()
                e.processOnServer = false;
                return false;
            }
        }

        statusPopup.Hide();
        LoadingPanel.Show();
        

        if (opportunityStatus == "Awarded" || opportunityStatus == "Cancelled" || opportunityStatus == "Declined" || opportunityStatus == "Lost" || opportunityStatus == "Precon") {
            GetOpenTasksCount('<%=currentTicketPublicID%>');
            if (StatusButtonName == "statusButton" || StatusButtonName == "advanceToProject") {
                if (OpenTaskCount == 0) {
                    e.processOnServer = true;
                    return true;
                }
                else {
                    ActionContainer('lnkStatus');
                }
            }
            else {
                if (OpenTaskCount > 0) {
                    //TasksAction.Show();
                    LoadingPanel.Hide();
                    var customDialog = DevExpress.ui.dialog.custom({
                        title: "Open Tasks Alert",
                        message: "You have some pending Tasks. Please select one of the options below.",
                        buttons: [
                            { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                            { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                            { text: "Cancel", onClick: function () { return "Cancel" } }
                        ]
                    });
                    customDialog.show().done(function (dialogResult) {
                        if (dialogResult == "CloseTasks") {
                            $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(true);
                            ActionContainer('lnkStatus');
                        }
                        else if (dialogResult == "KeepTasksOpen") {
                            $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(false);
                            ActionContainer('lnkStatus');
                        }
                    });
                }
                else {
                    e.processOnServer = true;
                    return true;
                }
            }
        }
        // commented, to add above confirmation popup
        //return true;   
        e.processOnServer = false;
        return false;
    }

    function CheckOpenTasks(buttonName) {
        let opportunityStatus = '';
        if (buttonName == "statusButton")
            opportunityStatus = "Lost";
        else if (buttonName == "advanceToProject")
            opportunityStatus = "Awarded";
        if (opportunityStatus == "Awarded" || opportunityStatus == "Lost") {
            GetOpenTasksCount('<%=currentTicketPublicID%>');

            if (OpenTaskCount > 0) {
                //TasksAction.Show();
                LoadingPanel.Hide();
                var customDialog = DevExpress.ui.dialog.custom({
                    title: "Open Tasks Alert",
                    message: "You have some pending Tasks. Please select one of the options below.",
                    buttons: [
                        { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                        { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                        { text: "Cancel", onClick: function () { return "Cancel" } }
                    ]
                });
                customDialog.show().done(function (dialogResult) {
                    if (dialogResult == "CloseTasks") {
                        $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(true);
                        $("#<%=hdnIgnoreConstraintValidation.ClientID%>").val(true);
                        OpenStagePopup(buttonName);
                    }
                    else if (dialogResult == "KeepTasksOpen") {
                        $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(false);
                        $("#<%=hdnIgnoreConstraintValidation.ClientID%>").val(true);
                        OpenStagePopup(buttonName);
                    }
                });
            }
            else {
                OpenStagePopup(buttonName);
                
            }
        }
        return false;
    }

    function RejectButtonClicked(s, e) {
        ///commentsRejectPopup.Hide();
        GetOpenTasksCount('<%=currentTicketPublicID%>');

        <%--if ($("#<%=chkSendLossEmail.ClientID%>").get(0).checked) {
            var vals = ASPxClientControl.GetControlCollection().GetByName("txtToLookupSearchValue").GetValue()

            if (vals == null || vals.length == 0) {
                alert("select Mail To");
                LoadingPanel.Hide();
                e.processOnServer = false;
                return false;
            }
        }--%>

        if (OpenTaskCount > 0) {
            var customDialog = DevExpress.ui.dialog.custom({
                title: "Open Tasks Alert",
                message: "You have some pending Tasks. Please select one of the options below.",
                buttons: [
                    { text: "Close all open Tasks", onClick: function () { return "CloseTasks" } },
                    { text: "Keep Tasks open", onClick: function () { return "KeepTasksOpen" } },
                    { text: "Cancel", onClick: function () { return "Cancel" } }
                ]
            });
            customDialog.show().done(function (dialogResult) {
                if (dialogResult == "CloseTasks") {
                    $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(true);
                    ActionContainer('btnrejectButton');
                }
                else if (dialogResult == "KeepTasksOpen") {
                    $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(false);
                    ActionContainer('btnrejectButton');
                }
            });
        }
        else {
            $("#<%=hndCompleteTasksOnItemClose.ClientID%>").val(false);
            ActionContainer('btnrejectButton');
        }
    }


    function GetOpenTasksCount(obj) {
        var jsonData = { "ticketID": obj };

        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperPage %>/GetOpenTasksCount",
            data: JSON.stringify(jsonData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                try {
                    OpenTaskCount = $.parseJSON(message.d);
                } catch (ex) {
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function ShowHideAwardNotificationControl() {
        if ($("#<%=chkSendAwardNotification.ClientID%>").is(':checked')) {
            $("#<%=trSendAwardNotification.ClientID%>").css("display", "block");
        }
        else {
            $("#<%=trSendAwardNotification.ClientID%>").css("display", "none");
        }
    }


    function commentsRejectPopupEndcallback() {

        DisplaySearchImageForLoss();
        LoadingPanel.Hide();

        var moduleName = currentModuleName;
        if (moduleName == "LEM") {
            $("#trCPRLossDefaultReturnMessage").css("display", "none");
            $("#trPopLossRejectedComments").css("display", "none");
            $("#trCheckboxSendLossEmail").css("display", "none");
            $("#<%=trLossEmailNotification.ClientID%>").css("display", "none");
            $("#trDefaultReturnMessage").css("display", "none");
            //$("#divContainsRejectPopControl").css("height","275px");
            $("#trLEDDropDownList").css("display", "none");
        }
        else {
            if (moduleName == "CPR" || moduleName == "CNS" || moduleName == "OPM") {
                $("#trDefaultReturnMessage").css("display", "none");
                $("#trPopRejectedComments").css("display", "none");
                //$("#divContainsRejectPopControl").css("height","500px");
                $("#trLEDReturnMessage").css("display", "none");
                $("#trLEDDropDownList").css("display", "none");
            }
            else {
                $("#trCPRLossDefaultReturnMessage").css("display", "none");
                $("#trPopLossRejectedComments").css("display", "none");
                $("#trCheckboxSendLossEmail").css("display", "none");
                $("#<%=trLossEmailNotification.ClientID%>").css("display", "none");
                // $("#trLossEmailNotification")
                $("#trLEDReturnMessage").css("display", "none");
                $("#trLEDDropDownList").css("display", "none");

                //$("#divContainsRejectPopControl").css("height","242px");
            }
        }
    }

    function OpenRejectPopup() {

        var moduleName = currentModuleName;
        if (moduleName == "LEM")
            commentsRejectPopup.SetHeaderText("Close Lead")

        commentsRejectPopup.Show();
        LoadingPanel.Show();
        commentsRejectPopup.PerformCallback();

        if (moduleName == "LEM") {
            $("#trCPRLossDefaultReturnMessage").css("display", "none");
            $("#trPopLossRejectedComments").css("display", "none");
            $("#trCheckboxSendLossEmail").css("display", "none");
            $("#<%=trLossEmailNotification.ClientID%>").css("display", "none");
            // $("#trLossEmailNotification")
            $("#trDefaultReturnMessage").css("display", "none");
            $("#divContainsRejectPopControl").css("height", "260px");
            $("#trLEDDropDownList").css("display", "none");
        }
        else {
            if (moduleName == "CPR" || moduleName == "CNS" || moduleName == "OPM") {
                $("#trDefaultReturnMessage").css("display", "none");
                $("#trPopRejectedComments").css("display", "none");
                $("#divContainsRejectPopControl").css("height", "500px");
                $("#trLEDReturnMessage").css("display", "none");
                $("#trLEDDropDownList").css("display", "none");
            }
            else {
                $("#trCPRLossDefaultReturnMessage").css("display", "none");
                $("#trPopLossRejectedComments").css("display", "none");
                $("#trCheckboxSendLossEmail").css("display", "none");
                $("#<%=trLossEmailNotification.ClientID%>").css("display", "none");
                // $("#trLossEmailNotification")
                $("#trLEDReturnMessage").css("display", "none");
                $("#trLEDDropDownList").css("display", "none");
                $("#divContainsRejectPopControl").css("height", "200px");
            }
        }


        return false;
    }

    function CloseRejectPopup() {
        var emailBody = EmailLossHtmlBody.GetHtml();
        $("#<%=popedLossRejectComments.ClientID%>").val("");
        emailBody = emailBody.replace($("#<%=hdnAwardLossComments.ClientID%>").val(), "[Your content goes here]");
        EmailLossHtmlBody.SetHtml(emailBody);
        commentsRejectPopup.Hide();
    }

    function MoveAwardStage() {
        commentsAwardPopup.Show();
        LoadingPanel.Show();
        commentsAwardPopup.PerformCallback();
        return false;
    }

    function GridLookupAward_EndCallback(s, e) {
        DisplaySearchImageForAward();
    }

    function EndCommentAwardPopup() {

        DisplaySearchImageForAward();
        LoadingPanel.Hide();
    }

    function DisplaySearchImageForAward() {
        $("[id$='_trSendAwardNotification']").each(function (i, item) {
            if ($(item).find(".dxgvCommandColumn_UGITClassicDevEx ").length > 0) {
                $($(item).find(".dxgvCommandColumn_UGITClassicDevEx ").get(0)).append('<img class="magnifier" style="cursor: pointer; " src="/Content/images/uGovernIT/search-black.png" alt="Search">');
            }
        });
    }

    function CloseAwardPopup() {
        var emailBody = EmailHtmlBody.GetHtml();
        $("#<%=txtCommentsAward.ClientID%>").val("");
        emailBody = emailBody.replace($("#<%=hdnAwardLossComments.ClientID%>").val(), "[Your content goes here]");
        EmailHtmlBody.SetHtml(emailBody);
        commentsAwardPopup.Hide();
    }

    function CloseStatusPopup(s, e) {
        statusPopup.Hide();
        e.processOnServer = false;
    }


    function endCallbackStatuspopup() {
        DisplaySearchImageForStatus();
        LoadingPanel.Hide();
    }

    function GridLookupStatus_EndCallback(s, e) {
        DisplaySearchImageForStatus();
    }

    function DisplaySearchImageForStatus() {
        $("[id$='_trOPSMailBody']").each(function (i, item) {
            if ($(item).find(".dxgvCommandColumn_UGITClassicDevEx ").length > 0) {
                $($(item).find(".dxgvCommandColumn_UGITClassicDevEx ").get(0)).append('<img class="magnifier" style="cursor: pointer; " src="/_layouts/15/images/uGovernIT/search-black.png" alt="Search">');
            }
        });
    }

    function ShowHideLossNotificationControl() {
        if ($("#<%=chkSendLossEmail.ClientID%>").is(':checked')) {
            $("#<%=trLossEmailNotification.ClientID%>").css("display", "block");
        }
        else {
            $("#<%=trLossEmailNotification.ClientID%>").css("display", "none");
        }
    }

    function LossMessageChanges() {
        
        var emailBody = EmailLossHtmlBody.GetHtml();
        var commenttext = $("#<%=popedLossRejectComments.ClientID%>").val();
        emailBody = emailBody.replace("[Your content goes here]", commenttext);
        if ($("#<%=hdnAwardLossComments.ClientID%>").val() != "")
            emailBody = emailBody.replace($("#<%=hdnAwardLossComments.ClientID%>").val(), commenttext);

        $("#<%=hdnAwardLossComments.ClientID%>").val(commenttext);
        EmailLossHtmlBody.SetHtml(emailBody);
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }

    //function ShowLeadRanking(obj) {
    //    LoadingPanel.Show();
    //    var path = LeadPriorityUrl + obj;        
    //    //UgitOpenPopupDialog(path,'', 'Lead Priority', 85, 85, false, escape(window.location.href));
    //    setTimeout(function(){ UgitOpenPopupDialog(path,'', 'Lead Priority', 85, 85, false, escape(window.location.href)); LoadingPanel.Hide(); }, 250);
    //}

    function ticketContactEmail(obj) {
        status = 1;
        timer = setTimeout(function () {
            if (status == 1) {
                var contactId = $(obj).attr("contactId");
                window.parent.UgitOpenPopupDialog("<%=TicketEmailURL %>&ModuleName=" + currentModuleName +"&currentTicketPublicID=<%=currentTicketPublicID %>" + "&contactId=" + contactId + "&type=contact", "", "Send Email", 60, 90);
            }
        }, 250);
    }

    function showhideStatusMailOption(obj) {
        if ($(obj).find("input:checkbox").get(0).checked) {
            $('#<%=trOPSMailBody.ClientID%>').css('display', '');
        }
        else {
            $('#<%=trOPSMailBody.ClientID%>').css('display', 'none');
        }
    }

    function sendMailFromStatus(obj) {
        if ($(obj).find("input:checkbox").get(0).checked) {
            $('#<%=hdnSendMailFromStatus.ClientID%>').val('true')
        }
        else {
            $('#<%=hdnSendMailFromStatus.ClientID%>').val('false')
        }
    }

    $(document).ready(function () {

        var isShowResetPopup = '<%=isShowResetPasswordErrorPopup %>';
        var message = '<%=restpasswordagentmessage%>'
        if (isShowResetPopup == 'True') {
            showResetPassWordValidation(message);
        }
        ShowHideRetainageOtherField();
    });

    function showResetPassWordValidation(message) {
        var resultConfirm = DevExpress.ui.dialog.alert(message, "Reset Password: Information");
    }


    function gotoTaskWorkFlow() {
        var serviceworkflowlink = '<%=ServiceTaskWorkFlow%>';
        var params = "TicketId=<%=currentTicketPublicID%>" + "&ModuleName=" + "<%=currentModuleName%>";
        window.parent.UgitOpenPopupDialog(serviceworkflowlink, params, "WorkfLow", 90, 90, false, escape(window.location.href));
    }

    function OpenTicketUpload() {
        PopupUploadTicketIcon.Show();
    }

    function btnUploadTicketIcon_click() {
        PopupUploadTicketIcon.Hide();
    }
</script>
<div id="dialog"></div>
 
<%--this ASPxPopupControl ID="controlsPopup" is for test of edit button--%>

<div id="hiddenContainer" style="visibility: hidden; height:0;">
    <asp:Button ID="btnGroupActioner" runat="server" OnClick="Button_Click" />
    <asp:HiddenField ID="actionEventID" runat="server" />
</div>


<asp:HiddenField ID="hdnMode" runat="server" Value="" />
<%--<asp:Label runat="server" ID="leadidg"></asp:Label>--%>
<dx:ASPxPopupControl ID="controlsPopup" runat="server" CloseAction="CloseButton" CloseOnEscape="true"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="controlsPopup"
    AllowDragging="True" PopupAnimationType="None" EnableViewState="False" AllowResize="true">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <dx:ASPxPanel runat="server" DefaultButton="btnOK" ScrollBars="Vertical">
                <PanelCollection>
                    <dx:PanelContent>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>

        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<asp:Panel runat="server" ID="moduleContainer">
	<asp:HiddenField ID="hdnAwardLossComments" runat="server" />
    <asp:HiddenField ID="hdnMacroTicketTemplate" runat="server" />
    <asp:HiddenField runat="server" ID="deletedFile" Value="" />

    <div class="editpopup-imp-message-box" id="impMessageBox" style="" runat="server">
        <span id="importantMessageBox" runat="server"></span>
    </div>
    <div id="form" class="form">

        <asp:Panel runat="server" ID="panelDetail">
            <%--<div id="divCompactViewImg" runat="server" class="compactViewImg" >
                <img id="imgCompactView" src="/Content/images/Menu/SubMenu/SVC_32x32.svg" height="20px" width="20px" onclick="showCompactView();" title="Compact View" />
            </div>--%>
          
            <div class="row mt-4">
                <asp:Panel runat="server" ID="topGraphicPanel">
                        <div class="row">
                            <%--<div class="com-md-8 col-sm-8 col-xs-12 noPadding">
                                <div class="row">
                                    <div id="divLifecycle" class="lifeCycle-content" runat="server" visible="false">
                                        <asp:Label ID="lblLifecycle" runat="server" Text="Project Lifecycle: "></asp:Label>
                                        <asp:Label ID="lblLifecycleText" runat="server"></asp:Label>
                                        <img id="imgEdit" runat="server" style="cursor: pointer; height: 16px; margin-left:3px;" src="/Content/Images/editNewIcon.png" 
                                            alt="Edit" onclick="return ChangeCurrentStage();" />
                                    </div>
                                    <span class="ticket_header_data">
                                        <asp:Literal ID="currentStageDescriptionLiteral" runat="server"></asp:Literal>
                                    </span>
                                    <div style="text-align: right;">
                                        <asp:Label ID="lblEscalationMessage" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-12 col-xs-12 noPadding">
                                        <span class="ticketTitle_status"><%=moduleTypeTitle %> Status</span> : 
                                        <span class="ticket_status">
                                            <asp:Literal ID="TicketWorkflowStatusLiteral" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                </div>
                            </div>--%>
                            <div class="com-md-1 col-sm-1 col-xs-12 noPadding fleft mb-3 mt-3">
                                <div class="messageConatiner111 ticketTitle_wrap" id="ticketMsgContainer" runat="server">
                                    <div class="changeView-wrap" id="compactViewContainer" runat="server">
                                    <div id="divCompactViewImg" runat="server" class="compactViewImg" >
                                        <img id="imgCompactView" src="/Content/images/changeView.png"  width="16" onclick="showCompactView();" 
                                            title="Compact View" />
                                    </div>
                                </div>
                                    <div class="cioReportbuttonContainer111 ticket-title">
                                        <h6 class="">
                                            <a href="javascript:" class="nav previous edit-ticket-prev" title="Previous" onclick="showNextPreviousTicket('previous')"> 
                                               <%-- <i class="fas fa-angle-double-left" style="font-size: 16px"></i>--%>
                                                <img src="../../Content/Images/pre-arrow.png" style="height:16px;width:16px"/>
                                            </a>
                                            <span class="edit-ticket-devider" style="vertical-align: middle">|</span>
                                            <span class="edit-ticket-ID" style="display:none;"><asp:Literal ID="TicketNoLiteral" runat="server"></asp:Literal></span>
                                            <a href="javascript:" class="nav next edit-ticket-next" title="Next" onclick="showNextPreviousTicket('next')">
                                                <%--<i class="fas fa-angle-double-right" style="font-size: 16px"></i>--%>
                                                <img src="../../Content/Images/next-arrow.png" style="height:16px;width:16px"/>
                                            </a>
                                        </h6>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-10 col-sm-10 col-xs-12">
                                <div id="workflowContainer">
                                    <div class="contract_steps_module workflowGraphicContainer111" runat="server" id="topGraphicDiv">
                                        <div class="contract_steps_container editTicket-workflowContainer">
                                            <div class="contract_steptop_content">
                                                <div class="row SVCWorkflow-wrap">
                                                    <div class="svcWorkflow-container">
                                                            <div class="col-md-12" runat="server" id="divworkFlow">
                                                                <div class="wizard_steps float-left">
                                                                    <nav class="steps">
                                                                        <asp:ListView ID="lvStepSections" runat="server" ItemPlaceholderID="PlaceHolder1" DataKeyNames="ID"   OnItemDataBound="NewTicketWorkFlowListView_ItemDataBound">
                                                                            <LayoutTemplate>
                                                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <div class="step svcWorkFlow-Step">
                                                                                    <div id="activeIconDiv" runat="server" class="step_content employee-info">
                                                                                        <div id="divHelp" runat="server" class="wf-img-wrap divHelp">
                                                                                            <div id="hoverContainer" runat="server" class="svc-actionBtnWrap"  style="margin: 11px;" >
                                                                                                <img id="imgShowHelp" runat="server" class="action-add pmm-actionAdd" src="/content/images/help_22x22.png" alt="Add" title="show help"  />
                                                                                                <img id="imgShowWorkFlow" runat="server"  src="/Content/Images/workflow.png" alt="Add" title="show task workflow" style="float: right;width: 14px;margin-top: 2px;" />
                                                                                            </div>
                                                                                            <div id="tdshowallrelatedticketbefore" class="relatedticketbefore" runat="server">
                                                                                                <div id="tdshowallrelatedticketbeforediv" runat="server"></div>
                                                                                            </div>
                                                                                            <p id="stepIcon" class="step_number" runat="server">
                                                                                                <img id="imgSection" runat="server" visible="true" width="35" />
                                                                                            </p>
                                                                                         </div>

                                                                                        <small>
                                                                                            <asp:Label runat="server" ID="sectionSideBarContainer" ></asp:Label>
                                                                                        </small>

                                                                                        <div class="lineIcons">
                                                                                            <div class="line -background bg-col-blue lineWorkflow" id="lineWorkflow" runat="server"></div>
                                                                                            <div id="tdshowallrelatedticket" class="tdshowallrelatedticket" runat="server">
                                                                                                <div id="tdshowallrelatedticketdiv" runat="server"></div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:ListView>
                                                                    </nav>
                                                                </div>
                                                            </div>
                                                    </div>
                                                </div>

                                                <table style="text-align: center; border-collapse: collapse;" width="98%">
                                                    <tr>
                                                        <td align="center">
                                                            <table style="text-align: center; border-collapse: collapse;">
                                                                <tr>
                                                                    <asp:Repeater ID="StageRepeater" runat="server" OnItemDataBound="StageRepeater_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <td class="agentImg-container">
                                                                                <div class="agentImg-wrap">
                                                                                    <%--<asp:Image src="../../Content/Images/agent-icon.png" width="18px"  runat="server" ID="agentIcon" />--%>
                                                                                    <img id="imgAgent" runat="server" src="../../Content/Images/agent-icon.png" width="20" visible="false" />
                                                                                    <label id="lblAgent" runat="server" class="agentLabel" visible="false"></label>

                                                                                </div>
                                                                            </td>
                                                                            <td id="tdshowallrelatedticketbefore" style="position: relative; background-image: url('/Content/Images/stepline_active.gif');" runat="server">
                                                                                <div id="tdshowallrelatedticketbeforediv" runat="server">
                                                                                    <%--<img id="tdshowallrelatedticketimg" src="/Content/Images/timeline.png" runat="server" style="display:none;" />--%>
                                                                                </div>
                                                                            </td>
                                                                            <td id="tdStage" runat="server" style="height: 38px; width: 36px; background-repeat: no-repeat;" class ="node nodeComplete">
                                                                                <div style="position: relative">
                                                                                    <span class="pos_rel">
                                                                                        <i id="stageNo" runat="server" class="node-stageno" >
                                                                                            <asp:Literal ID="lbStageNumber" runat="server"></asp:Literal>
                                                                                        </i>
                                                                                    </span>
                                                                                    <span class="stage-titlecontainer alternategraphiclabel workflow-lable" id="stageTitleContainer" runat="server">
                                                                                        <b class="pos_rel" >
                                                                                            <asp:Literal ID="stageTitle" runat="server"></asp:Literal>
                                                                                        </b>
                                                                                    </span>

                                                                                    <i id="activeStageArrow" runat="server"></i>
                                                                                </div>
                                                                            </td>
                                                                            <td id="tdshowallrelatedticket" style="position: relative; background-image: url('/Content/Images/stepline_active.gif');" runat="server">
                                                                                <div id="tdshowallrelatedticketdiv" runat="server">
                                                                                    <%--<img id="tdshowallrelatedticketimg" src="/Content/Images/timeline.png" runat="server" style="display:none;" />--%>
                                                                                </div>
                                                                            </td>
                                                                            <td id="tdStepLine" runat="server" style="height: 38px; background-repeat: repeat-x;">
                                                                                <div>
                                                                                    <img id="tdStepLineimg" runat="server" src="/Content/Images/stepline_active.gif" class="steplineimage">
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="com-md-1 col-sm-1 col-xs-12 fright">
                                 <div class="fright" style="display:inline-block;">
                                    <span class="ticket_info_icon" id="helpTextWrap" runat="server">
                                        <asp:Panel ID="helpTextContainer" runat="server" Style="float: right;">
                                        </asp:Panel>
                                    </span>
                                </div>
                                
                            </div>
                        </div>

                        <div class="row">
                            <div id="dvStageHeader" runat="server" style="position: relative; width: 98.9%;">
                                 <span id="spStageTitle" style="display: none;">Workflow</span>
                                <img id="imgHideShowStage" src="/Content/Images/minus-square.png" style="z-index: 9999; position: absolute; display:none;
                                     padding-left: 4px; width: 17px; cursor: pointer;" onclick="HideShowStages(this)" />               
                            </div>
                        </div>
                        
                        <div id="ticketStatusInfo">
                            <div class="row edit-ticket-error-msg-container">
                                <div class="com-md-7 col-sm-7 col-xs-12 noPadding" runat="server" id="stageDescriptionContainer">
                                    <div class="d-flex align-items-center">
                                        <div class="mr-2 imgMx-40 curPont">
                                            <dx:ASPxImage ID="imgTicketIcon" runat="server" Visible="false" CssClass="w-100">
                                                    <ClientSideEvents Click="OpenTicketUpload" />
                                            </dx:ASPxImage>
                                        </div>
                                        <div>
                                            <div class="mb-2">
                                                <div id="divLifecycle" class="lifeCycle-content" runat="server" visible="false">
                                                    <asp:Label ID="lblLifecycle" runat="server" Text="Project Lifecycle: "></asp:Label>
                                                    <asp:Label ID="lblLifecycleText" runat="server"></asp:Label>
                                                    <img id="imgEdit" runat="server" style="cursor: pointer; height: 16px; margin-left:3px;" src="/Content/Images/editNewIcon.png" alt="Edit" onclick="return ChangeCurrentStage();" />
                                                </div>
                                                <span class="ticket_header_data">
                                                    <asp:Literal ID="currentStageDescriptionLiteral" runat="server"></asp:Literal>
                                                </span>
                                                <div style="text-align: right;">
                                                    <asp:Label ID="lblEscalationMessage" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div>
                                                <span class="ticketTitle_status"><%=moduleTypeTitle %> Status</span> : 
                                                <span class="ticket_status">
                                                    <asp:Literal ID="TicketWorkflowStatusLiteral" runat="server"></asp:Literal>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div id="stageDescriptionContainer" class="stageDescriptor" runat="server"></div>--%>
                                <div class="col-md-5 col-sm-5 col-xs-12" style="float:right;">
                                    <div id="errorMsgContainer">
                                        <span class="ticket_error_msg">
                                            <asp:Literal ID="errorMsg" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                    <div class="ticket_error_wrap" style="">
                                        <span class="error_msg2" style="">
                                            <asp:Literal ID="invalidErrorMsg" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                </asp:Panel>
            </div>
            <%--  module detail tabs--%>
            <div class="moduleDetailTabsContainer111 tab_container col-md-12 col-sm-12 col-xs-12 noPaddingLeft" id="moduleDetailTabs">
                <table cellspacing="0" cellpadding="0" style="border-collapse: collapse; width: 100%;">
                    <tr>
                        <td class="moduleDetailTabsSubContainer111">
                            <asp:HiddenField ID="hdnActiveTab" runat="server" />
                            <dx:ASPxTabControl EnableTabScrolling="true" CssClass="tab_wrap" ID="tbcDetailTabs" ClientInstanceName="tbcDetailTabs" Width="100%" runat="server" OnTabDataBound="tbcDetailTabs_TabDataBound">
                                <ClientSideEvents ActiveTabChanged="function(s,e){ actionOnTabChange(s.activeTabIndex);}" TabClick="function(s,e){ actionOnTabClick(e.tab.index, s.activeTabIndex);}" />
                                <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
                            </dx:ASPxTabControl>
                        </td>
                        <td class="svcEdit_actionBtnContainer">
                            <div style="float: right;">
                                <%--  Report Menu --%>
                                <div style="float: right; padding-left: 5px; padding-top: 5px;" runat="server" id="pnlReport">
                                    <span id="exportAction" style="padding-left: 3px" class="fright" runat="server">
                                        <img id="imgExportAction" runat="server" src="/Content/images/Reports.svg" alt="Reports" title="Reports" class="pmmedit_reportIcon imgReport" style="cursor: pointer;" />
                                        <dx:ASPxPopupMenu ID="ASPxPopupMenuReport" GutterWidth="0px" CssClass="report-popupMenu" runat="server" PopupElementID="imgExportAction" ShowPopOutImages="True" CloseAction="MouseOut"
                                            ClientInstanceName="ASPxPopupMenuReport" PopupHorizontalAlign="OutsideRight" PopupVerticalAlign="TopSides" PopupAction="LeftMouseClick">
                                            <Items>
                                                <dx:MenuItem Name="TaskReport" Text="Task Report" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ProjectReport" Text="Project Report" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ProjectCompactReport" Text="1-Pager Report" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ResourceHours" Text="Resource Hours" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ERHReport" Text="ERH Report" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="BudgetReport" Text="Budget Report" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ActualsReport" Text="Actuals Report" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ProjectStageHistory" Text="Project stage history" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                                <dx:MenuItem Name="ProjectActualsReport" Text="Project Actuals Report" ClientVisible="false" Image-Url="/Content/images/Active-Projects.png" Image-Width="16px"></dx:MenuItem>
                                            </Items>
                                            <ClientSideEvents ItemClick="function(s, e) { popupMenuReportItemClick(s,e);}" PopUp="function(s, e) { disableMenuItems(s, e); }" />
                                            <ItemStyle Width="155px"></ItemStyle>
                                        </dx:ASPxPopupMenu>
                                    </span>
                                </div>

                                
                                <div class="cioReportbuttonContainer111 actionBtn_container">
                                    <div class="btn-container">
                                        <div class="dropdown btn-wrap actionSvcBtn_wrap">
                                            <dx:ASPxButton ID="lnkbtnActionMenu" CssClass="btn dropdown-toggle action-btn" data-toggle="dropdown" runat="server" ClientVisible="false" Text="Actions" ImagePosition="Right" AutoPostBack="false">
                                                <Image Url="/Content/Images/arrow-down.png"></Image>
                                            </dx:ASPxButton>

                                            <dx:ASPxPopupMenu ID="ASPxPopupActionMenu" OnLoad="ASPxPopupActionMenu_Load" runat="server" PopupElementID="lnkbtnActionMenu" CloseAction="MouseOut" ItemSpacing="0" SubMenuStyle-ItemSpacing="0"
                                                ClientInstanceName="ASPxPopupActionMenu" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" PopupAction="LeftMouseClick">
                                                <Items>
                                                </Items>
                                                <ClientSideEvents ItemClick="function(s, e) { popupMenuActionMenuItemClick(s,e);}" />
                                               
                                                <ItemStyle CssClass="dxb editTicket-actionMenu" BackColor="#ebedf2" HoverStyle-BackColor="Blue"></ItemStyle>
                                                
                                            </dx:ASPxPopupMenu>
                                        </div>
                                    </div>
                                </div>

                                <div style="float: right;padding-top: 9px;padding-right: 10px;font-size: 12px; font-family: 'Poppins', sans-serif !important;">
                                    <b><%=currentTicketPublicID %></b>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="row">
                <div id ="confirmationDialog"></div>
                <div style="position: relative;" class="utilityContainer111 col-md-12 col-sm-12 col-xs-12 section-btn ">
                    <!--  Show module stage constraints mark complete Popup , start  here-->
                    <asp:HiddenField ID="areAllTaskComplete" runat="server" />
                    <asp:HiddenField ID="taskNames_Pending" runat="server" />
                    <div id="stageTaskCompleteBox" style="display: none;">
                        <div style="padding-left: 20px;" id="stageTaskComplete">
                            <div id="messageBoardContainer" style="margin-bottom: 15px; margin-top: 10px;">
                                <div style="float: inherit;font-weight: bold" id="taskStatusMessage"></div>
                            </div>
                            <div style="text-align: center;">
                                <span>
                                    <a href="javascript:void(0);" onclick="closeStageTaskCompleteDialog(); return false;" style="float: right; padding-left: 10px;">
                                        <span style="background: url('/Content/Images/firstnavbg.gif') repeat-x scroll 0 0 transparent; color: #FFFFFF; cursor: pointer; float: left; margin: 1px; padding: 4px 6px 6px;">
                                            <b style="float: left; font-weight: normal;">Mark Complete</b>
                                            <i
                                                style="float: left; position: relative; top: -1px; left: 2px">
                                                <img src="/Content/Images/tick-iconOld.png" style="border: none;" title="" alt="" /></i>
                                        </span>
                                    </a>
                                    <a href="javascript:void(0);" onclick="closestageTaskCompleteBox(); return false;" style="float: right; padding-left: 10px;">
                                        <span style="background: url('/Content/Images/firstnavbg.gif') repeat-x scroll 0 0 transparent; color: #FFFFFF; cursor: pointer; float: left; margin: 1px; padding: 4px 6px 6px;">
                                            <b style="float: left; font-weight: normal;">Cancel</b>
                                            <i
                                                style="float: left; position: relative; top: -1px; left: 2px">
                                                <img src="/Content/Images/cancel.png" style="border: none;" title="" alt="" /></i>
                                        </span>
                                    </a>
                                </span>
                            </div>
                        </div>
                    </div>

                    <dx:ASPxPopupControl ID="closePMMPopUp" runat="server" Modal="True" Width="390px" PopupElementID="showCloseButtonB"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="closePMMPopUp" EncodeHtml="false"
                        HeaderText="Close Project" AllowDragging="true" PopupAnimationType="None" EnableViewState="False">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl13" runat="server">
                                <div style="float: left;" >
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>Are you sure you want to closeout this project?
                                           
                                            <br />
                                                <br />
                                                This will mark all tasks as completed and move the project to the last stage.
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="float:right; width: 100%; padding: 10px" class="first_tier_nav">
                                    <div style="float:left">
                                        <dx:ASPxButton ID="btnclosePMM" runat="server" ClientInstanceName="btnclosePMM"  Text="Close Project" AutoPostBack="false">
                                            <ClientSideEvents Click="ClosePMM" />
                                        </dx:ASPxButton>
                                    </div>
                                     <div style="float:left;">
                                         <dx:ASPxButton ID="btnpmmcanel" runat="server" ClientInstanceName="btnpmmcanel" Text="Cancel" AutoPostBack="false">
                                             <ClientSideEvents Click="function(){ 
                                                 closePMMPopUp.Hide();
                                                 }" />
                                         </dx:ASPxButton>
                                     </div>   
                                            <%--<a runat="server" id="a3" style="color: white" class="baseline-restore" onclick="return ClosePMM();">Close Project</a>--%>
                                        
                                            <%--<a id="a6" style="color: white" onclick="closePMMPopUp.Hide();" class="cancelwhite" href="javascript:void(0);">Cancel</a>--%>
                                      
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ID="closePopUp" runat="server" Modal="True" Width="390px" PopupElementID="showCloseButtonB"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="closePopUp"
                        HeaderText="Unsaved Changes" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">

                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div>
                                            <asp:Label ID="lblUnsavedData" runat="server" Text="You have some unsaved changes on the form. Please select one of the options below." 
                                                CssClass="headerText"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="unsaved-btnWrap">
                                            <ul>
                                                <li runat="server" id="liSaveClose" class="saveAndClose" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" style="display:inline-block;cursor:pointer;">

                                                    <a runat="server" id="aSaveClose"  class="baseline-restore saveAndClose_btn" onclick="return SaveClose();">Save & Close</a>
                                                </li>
                                                <li runat="server" id="liCloseWithoutSaving" class="saveAndClose" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" 
                                                    style="display:inline-block;cursor:pointer;">
                                                    <a runat="server" id="aCloseWithoutSaving" class="cancelwhite saveAndClose_btn" onclick="CloseWithoutSaving()">Close Without Saving</a>
                                                </li>
                                                <li runat="server" id="liCancel" class="cancelli" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" style="display:block; margin-top:12px;">
                                                    <a id="aCancel" onclick="closePopUp.Hide();" class="cancelwhite unsaved_cancel" href="javascript:void(0);">Cancel</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ID="resolveInitiator" runat="server" Modal="True" Width="500px" PopupElementID="showCloseButtonB"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="resolveInitiator"
                        HeaderText="Resolution" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">

                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
                                <div style="float: left; height: auto; width: auto;" class="first_tier_nav">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width: 100px;">
                                                <asp:Label ID="lblActualHrs" runat="server" Text="Actual Hour(s)<b style='color:red;'>*</b>"> </asp:Label>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtActualHrs" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="errormsg-container" runat="server" ControlToValidate="txtActualHrs" ValidationGroup="ActualHours" ErrorMessage="Please enter actual hours" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" CssClass="errormsg-container" runat="server" ControlToValidate="txtActualHrs"
                                                    ErrorMessage="Only Numbers(0-9) Allowed" Display="Dynamic" ValidationGroup="ActualHours"
                                                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px;">
                                                <asp:Label ID="lblResolutionDesc" runat="server" Text="Resolution Description<b style='color:red;'>*</b>"> </asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtareaResolutionDesc" runat="server" CssClass="resolution-area" TextMode="MultiLine"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvValidator7" CssClass="errormsg-container" runat="server" ControlToValidate="txtareaResolutionDesc" ValidationGroup="ActualHours" ErrorMessage="Please enter resolution" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div style="float: right; width: 100%; margin-top: 10px">
                                                    <ul style="float: right">
                                                        <li runat="server" id="li1" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                            <dx:ASPxButton ID="btnquickCloseTicket" ValidationGroup="ActualHours" runat="server" Text="Save & Close" AutoPostBack="false">
                                                                <ClientSideEvents Click="function(s,e){ActionContainer('btnquickCloseTicket');}" />
                                                            </dx:ASPxButton>
                                                        </li>
                                                        <li runat="server" id="li24" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                            <dx:ASPxButton ID="a5" runat="server" Text="Close" AutoPostBack="false">
                                                                <ClientSideEvents Click="function(s,e){resolveInitiator.Hide();}" />
                                                            </dx:ASPxButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </td>
                                        </tr>

                                    </table>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <!--  Show module stage constraints mark complete Popup , ends here -->

                    <!--  Show PMM baselines Popup Starts here-->
                    <dx:ASPxPopupControl ClientInstanceName="baselineBoxContainer" Modal="true" SettingsAdaptivity-Mode="Always"
                        PopupElementID="showBaselineButtonB" ID="baselineBoxContainer"
                        ShowFooter="false" ShowHeader="true" CssClass="departmentPopup aspxPopup" HeaderText="Baselines"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12" id="baselineBox">
                                    <div class="row ms-formtable accomp-popup ">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Choose Baseline:</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                             <asp:DropDownList CssClass="ddlticketbaselines itsmDropDownList aspxDropDownList" ID="ddlTicketBaselines" 
                                                 OnPreRender="DdlTicketBaselines_PreRender" AutoPostBack="false" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                     <div class="row popupFooter-BtnWrap">
                                         <dx:ASPxButton ID="Li22" runat="server" CssClass="secondary-cancelBtn" Text="Cancel">
                                                <ClientSideEvents Click="function(s,e){baselineBoxContainer.Hide();}" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="LinkButton5" CssClass="primary-blueBtn" runat="server" Text="Show Baseline" ToolTip="Show Baseline" AutoPostBack="false">
                                            <ClientSideEvents Click="function(s,e){return showBaseline();}" />
                                        </dx:ASPxButton>
                                     </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <!--   Show PMM baselines Popup Ends here-->

                    <!--  Create PMM baseline Popup Starts here-->
                    <dx:ASPxPopupControl ClientInstanceName="createBaselineContainer" Modal="true"
                        PopupElementID="createBaselineButtonB" ID="createBaselineContainer"
                        ShowFooter="false" ShowHeader="true" CssClass="departmentPopup  aspxPopup " HeaderText="New Baseline" SettingsAdaptivity-Mode="Always"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div class="row ms-formtable accomp-popup ">
                                        <div class="ms-formlabel">
                                            <h3 class="ms-standardheader budget_fieldLabel">Description:</h3>
                                        </div>
                                        <div class="ms-formbody accomp_inputField">
                                             <asp:TextBox Height="100px"  TextMode="MultiLine" CssClass="createbaselinedesc" runat="server" ID="createBaselineDesc"></asp:TextBox>
                                             <asp:HiddenField ID="hiddenCreateBaseline" runat="server" />
                                        </div>
                                    </div>
                                    <div class="row popupFooter-BtnWrap">
                                         <dx:ASPxButton ID="closeBaseline" CssClass="secondary-cancelBtn" runat="server" Text="Close">
                                            <ClientSideEvents Click="function(s,e){CancelcreateBaseline()}" />
                                        </dx:ASPxButton>
                                        <dx:ASPxButton ID="createBaselineButtonH" runat="server" Text="Create Baseline" ToolTip="Create Baseline" CssClass=" primary-blueBtn" OnClick="Button_Click">
                                            <ClientSideEvents Click="function(s, e){createBaseline(); }" />
                                        </dx:ASPxButton>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>
                    <!--   Create PMM baseline Popup Ends here-->

                    <dx:ASPxPopupControl ClientInstanceName="selfAssignPopup"
                        PopupElementID="selfAssignButtonB" ID="selfAssignPopup"
                        ShowFooter="false" ShowHeader="true" CssClass="departmentPopup" HeaderText="Self-Assign Ticket: Required Input"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" EnableHierarchyRecreation="True">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
                                <div style="height: auto; width: auto;" class="first_tier_nav">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>Estimated Hours<b style='color: red'>* </b></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="popedEstimatedHours" TextMode="SingleLine" Text="0"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ul style="float: right">
                                                    <li runat="server" id="Li3" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                        <dx:ASPxButton runat="server" ID="btnselfAssignConfirm" Style="color: white" Text="Self Assign" CssClass="usertick" AutoPostBack="false">
                                                            <ClientSideEvents Click="function(s,e){ActionContainer('btnselfAssignConfirm')}" />
                                                        </dx:ASPxButton>

                                                    </li>
                                                    <li runat="server" id="Li4" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                        <dx:ASPxButton ID="cancelSelfAssign" runat="server" Visible="true" Text="Cancel">
                                                            <ClientSideEvents Click="function(s,e){selfAssignPopup.Hide();return false;}" />
                                                        </dx:ASPxButton>
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ClientInstanceName="commentsHoldPopup" Modal="true"
                        PopupElementID="holdWithCommentsButton" ID="commentsHoldPopup" SettingsAdaptivity-Mode="Always"
                        ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Put on Hold"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                        EnableHierarchyRecreation="True" CloseAction="CloseButton">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                                    <div class="ms-formtable accomp-popup ">
                                        <div class="row">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">
                                                    <asp:Label ID="lblHoldMessage" runat="server" Text=""> </asp:Label>
                                                </h3>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trHoldTill" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">Hold Till<span style="color:red;"> *</span></h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <dx:ASPxDateEdit ID="aspxdtOnHoldDate" CalendarProperties-ShowTodayButton="false" CalendarProperties-ShowClearButton="false" runat="server" DisplayFormatString="d" EditFormatString="d" ClientInstanceName="aspxdtOnHoldDate" CssClass="CRMDueDate_inputField" 
                                                            DropDownApplyButton-Image-Url="~/Content/Images/calendarNew.png">
                                                    </dx:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="trHoldReason" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">Reason</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <div class="divddlOnHoldReason" id="divddlOnHoldReason" runat="server" style="float: left; width: 100%;">
                                                        <div class="col-xs-9 noPadding">
                                                            <asp:DropDownList ID="ddlOnHoldReason" onchange="hideShowEdit('ddlOnHoldReason')" runat="server" EnableViewState="true" class="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-xs-3 noPadding mt-2">
                                                            <img alt="Edit Category" runat="server" class="editicon" id="btCategoryEdit"
                                                                src="/content/images/editNewIcon.png" width="16" style="cursor: pointer; position: relative; float: right;"
                                                                onclick="javascript:$('.divOnHoldReason').attr('style','display:block');hideddlOnHoldReason(1)" />
                                                            <img alt="Add Category" id="Img1" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right; margin-right: 10px;"
                                                                onclick="javascript:$('.divOnHoldReason').attr('style','display:block');hideddlOnHoldReason(0);" />
                                                        </div>

                                                    </div>
                                                    <div runat="server" id="divOnHoldReason" class="divOnHoldReason" style="display: none; float: left;">
                                                        <div class="col-xs-10 noPadding">
                                                            <asp:TextBox CssClass="form-control" ID="txtOnHoldReason" runat="server"></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hdnRequestOnHoldReason"></asp:HiddenField>
                                                        </div>
                                                        <div class="col-xs-2 noPadding mt-2">
                                                            <img alt="Cancel Category" style="float: right" width="16" src="/content/images/close-blue.png" class="cancelModule"
                                                                onclick="javascript:$('.divOnHoldReason').attr('style','display:none');showddlOnHoldReason();" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Comments</h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox runat="server" ID="popedHoldComments" TextMode="MultiLine" Text=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row" id="trchkEnableCloseOnHoldExpire" runat="server" visible="false">
                                            <div class="ms-formbody accomp_inputField crm-checkWrap">
                                                <asp:CheckBox ID="chkEnableCloseOnHoldExpire" runat="server" Text="Close When Hold Expires" Visible="true" />
                                            </div>
                                        </div>
                                        <div class="row addEditPopup-btnWrap">
                                            <dx:ASPxButton ID="LinkButton2" runat="server" AutoPostBack="false" Text="Cancel" CssClass="secondary-cancelBtn">
                                                <ClientSideEvents Click="function(s,e){commentsHoldPopup.Hide();}" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton runat="server" ID="btnHoldButton" Text="Put on Hold" Name="Hold" AutoPostBack="false" 
                                                CssClass="primary-blueBtn">
                                                <ClientSideEvents Click="function(s,e){ActionContainer('btnHoldButton');}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>

                                <%--<div style="height: auto; width: auto;" class="first_tier_nav">

                                    <table style="width: 100%;">
                                        <tr>
                                            <td colspan="2">
                                                <asp:Label ID="lblHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"> </asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">Hold Till<b style='color: red'>* </b></td>
                                            <td>
                                                <dx:ASPxDateEdit ID="aspxdtOnHoldDate" EditFormat="DateTime" TimeSectionProperties-Visible="true" Width="250px" runat="server" ClientInstanceName="aspxdtOnHoldDate"></dx:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">Reason</td>
                                            <td>
                                                <asp:DropDownList ID="ddlOnHoldReason" runat="server" Width="250px" EnableViewState="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">Comment</td>
                                            <td>
                                                <asp:TextBox runat="server" ID="popedHoldComments" Width="245px" Columns="30" Rows="5" TextMode="MultiLine" Text=""></asp:TextBox></td>
                                        </tr>
                                        <tr id="trchkEnableCloseOnHoldExpire" runat="server" visible="false">
                                            <td style="text-align: left;">Close When Hold Expires</td>
                                            <td style="padding-top: 5px">
                                                <asp:CheckBox ID="chkEnableCloseOnHoldExpire" runat="server" Visible="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <ul style="float: right;">
                                                    <li runat="server" id="Li11">
                                                        <dx:ASPxButton runat="server" ID="btnHoldButton" Text="Hold" Name="Hold" AutoPostBack="false" BackColor="Red" ForeColor="White">
                                                            <ClientSideEvents Click="function(s,e){ActionContainer('btnHoldButton');}" />
                                                        </dx:ASPxButton>

                                                    </li>
                                                    <li runat="server" id="Li12">
                                                        <dx:ASPxButton ID="LinkButton2" runat="server" Text="Cancel">
                                                            <ClientSideEvents Click="function(s,e){commentsHoldPopup.Hide();}" />
                                                        </dx:ASPxButton>

                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>

                                </div>--%>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ClientInstanceName="commentsUnHoldPopup" Modal="true" Width="400px"
                        PopupElementID="unHoldWithCommentsButton" ID="commentsUnHoldPopup" SettingsAdaptivity-Mode="Always"
                        ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Remove Hold Feedback"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                        EnableHierarchyRecreation="True" CloseAction="CloseButton">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                                    <div class="ms-formtable accomp-popup ">
                                        <div class="row">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">
                                                    <asp:Label ID="lblUnHoldMessage" runat="server"></asp:Label>
                                                </h3>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">Comments</h3>
                                            </div>
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox runat="server" ID="popedUnHoldComments" TextMode="MultiLine"
                                                Text="" Width="100%"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row addEditPopup-btnWrap">
                                            <dx:ASPxButton ID="LinkButton3" runat="server" AutoPostBack="false" Text="Cancel" CssClass="secondary-cancelBtn">
                                                <ClientSideEvents Click="function(s,e){commentsUnHoldPopup.Hide();}" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton runat="server" ID="btnUnHoldButton" Name="UnHold" Text="Remove Hold" AutoPostBack="false" CssClass="primary-blueBtn">
                                                <ClientSideEvents Click="function(s,e){ActionContainer('btnUnHoldButton');}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                                <%--<div style="float: left; height: 200px; width: 400px;" class="first_tier_nav">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblUnHoldMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="popedUnHoldComments" Width="400px" Columns="52" Rows="9" TextMode="MultiLine" Text=""></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="buttoncell">
                                                <ul style="float: right;">
                                                    <li runat="server" id="Li13" class="Green" onmouseover="this.className='tabhoverGreen'" onmouseout="this.className='Green'">
                                                        <dx:ASPxButton runat="server" ID="btnUnHoldButton" Name="UnHold" Text="UnHold" AutoPostBack="false">
                                                            <ClientSideEvents Click="function(s,e){ActionContainer('btnUnHoldButton');}" />
                                                        </dx:ASPxButton>
                                                    </li>
                                                    <li runat="server" id="Li14" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                        <dx:ASPxButton ID="LinkButton3" runat="server" Text="Cancel">
                                                            <ClientSideEvents Click="function(s,e){commentsUnHoldPopup.Hide();}" />
                                                        </dx:ASPxButton>

                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                        <!--   Award Notification Popup Starts here-->
                   <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                       $(document).ready(function () {
                           $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
                           $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
                       });
                   </script>
                    <dx:ASPxPopupControl ClientInstanceName="commentsAwardPopup" Modal="true" OnWindowCallback="commentsAwardPopup_WindowCallback" ClientSideEvents-EndCallback="EndCommentAwardPopup"
                        ID="commentsAwardPopup" SettingsLoadingPanel-ShowImage="false" SettingsLoadingPanel-Enabled="false"
                        ShowFooter="false" ShowHeader="true" CssClass="departmentPopup opprtunity-statusPopup" HeaderText="Award Message" 
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Above" EnableHierarchyRecreation="True">
                        <ClientSideEvents EndCallback="EndCommentAwardPopup" />
                        <SettingsAdaptivity Mode="Always" VerticalAlign="WindowTop" MaxWidth="700px"/>
                        <SettingsLoadingPanel Enabled="False" ShowImage="False" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl131" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                    <div style="width: 100%;">
                                        <div class="row">                                        
                                            <div class="col-sm-12 col-md-12 col-xs-12">
                                                <asp:Label ID="lblAwardMessage" runat="server" Font-Size="Small" ForeColor="Red" Text=""></asp:Label>
                                            </div>                                        
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-12 col-md-12 col-xs-12">
                                                <div class="award-commentWrap accomp_inputField">
                                                    <asp:TextBox ID="txtCommentsAward" runat="server" onBlur="AwardMessageChanges()" Rows="5" Text="" TextMode="MultiLine" CssClass="award_comment" placeholder="Comments" ></asp:TextBox>
                                                </div>
                                            </div>
                                   
                                            <div class="col-sm-12 col-md-12 col-xs-12">
                                                <div class="award-chkwrap crm-checkWrap">
                                                    <asp:CheckBox ID="chkSendAwardNotification" runat="server" Checked="true" OnClick="ShowHideAwardNotificationControl()" Text="Send Email " />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="trSendAwardNotification" runat="server">
                                            <div class="col-md-12 col-sm-12 col-xs-12">
                                                <div class="ms-formtable accomp-popup">
                                                    <div class="col-sm-12 col-md-12 col-xs-12 award_emailFields" id="tr9" runat="server">
                                                        <%--<div class="ms-formlabel" style="display:inline-block;">
                                                            <h3 class="ms-standardheader budget_fieldLabel">
                                                                <asp:Label ID="lblEmailToActionUser" runat="server" Text="To (Action User)"></asp:Label>
                                                            </h3>
                                                        </div>--%>
                                                        <div class="crm-checkWrap" >
                                                            <asp:CheckBox ID="cbIncludeActionUser" runat="server" Text="To (Action User)" />
                                                            <asp:Label ID="lbAwardActionUser" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-12 col-md-12 col-xs-12 noPadding" id="tr5" runat="server">
                                                        <div class="ms-formlabel">
                                                            <h3 class="ms-standardheader budget_fieldLabel">CC</h3>
                                                        </div>
                                                        <div class="ms-formbody accomp_inputField">
                                                            <ugit:UserValueBox ID="awardGridLookup" MaximumHeight="30" CssClass="userValueBox-dropDown peAssignedTo awardAssign" runat="server" isMulti="true" />
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12 col-md-12 col-xs-12 award_emailFields" id="tr17" runat="server">
                                                        <%--<div class="ms-formlabel" style="display:inline-block;">
                                                            <h3 class="ms-standardheader budget_fieldLabel">
                                                                <asp:Label ID="lblAwardSendMailFromLoggedInUser" runat="server" Text=""></asp:Label>
                                                            </h3>
                                                        </div>--%>
                                                        <div class="crm-checkWrap" >
                                                            <asp:CheckBox ID="chkSendMailfromLoggedInUserAward" Checked="false" runat="server" Text="Send Mail from Logged In User" />
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-12 col-md-12 col-xs-12 award_emailFields" id="tr8" runat="server">
                                                        <div class="ms-formlabel">
                                                            <h3 class="ms-standardheader budget_fieldLabel">Mail Subject</h3>
                                                        </div>
                                                        <td class="ms-formbody accomp_inputField">
                                                            <%--<asp:TextBox ID="txtMailSubject" runat="server" Text="Ticket [$TicketId$] escalation" Width="350px" />--%>
                                                            <asp:TextBox ID="txtMailSubject" runat="server" Text="[$TicketId$] escalation" Width="350px" />
                                                        </td>
                                                    </div>
                                                    <div class="row award-EmailBody" id="tr6" runat="server">
                                                        <div class="ms-formlabel">
                                                            <h3 class="ms-standardheader budget_fieldLabel">Email Body</h3>
                                                        </div>
                                                        <div class="ms-formbody accomp_inputField">
                                                            <div>
                                                                <dx:ASPxHtmlEditor ID="EmailHtmlBody" runat="server" ClientInstanceName="EmailHtmlBody" Height="320px" Width="100%" >
                                                                    <Settings AllowHtmlView="false" AllowPreview="false" />
                                                                </dx:ASPxHtmlEditor>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="buttoncell">
                                                <ul class="awardBtn_Wrap"> 
                                                     <li id="liACancel" runat="server" class="cancelAward_link" onmouseout="this.className='tabhover'" onmouseover="this.className='tabhover'">
                                                        <div class="cancelAward_btnWrap"">
                                                            <a class="cancelwhite" href="javascript:void(0);" onclick="CloseAwardPopup();">Cancel</a> 
                                                        </div>
                                                    </li>
                                                    <li id="liAward" runat="server" class="okAward_link" onmouseout="this.className='tabhover'" onmouseover="this.className='tabhover'">
                                                        <div class="okAward_btnWrap">
                                                            <asp:LinkButton ID="CommentsAward" runat="server" Name="Award" OnClick="Button_Click" OnClientClick="javascript:return validateFeedbackForm(this)" CssClass="" Text="OK" />
                                                        </div>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <dx:ASPxPopupControl ClientInstanceName="commentsRejectPopup" Modal="true" OnWindowCallback="commentsAwardPopup_WindowCallback" 
                        SettingsLoadingPanel-ShowImage="false" ID="commentsRejectPopup" ClientSideEvents-EndCallback="commentsRejectPopupEndcallback"  
                        ShowFooter="false" ShowHeader="true" SettingsAdaptivity-Mode="Always" 
                        CssClass="aspxPopup" HeaderText="Close/Cancel Workflow" EncodeHtml="false" 
                        SettingsLoadingPanel-Enabled="false" runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" 
                        PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True" Height="1000px" Width="100%">
                    <ClientSideEvents EndCallback="commentsRejectPopupEndcallback" />
                    <SettingsLoadingPanel Enabled="False" ShowImage="False" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <div id="divContainsRejectPopControl" class="deactive-popupContainer col-md-12 col-sm-12 col-xs-12">
                                <div class="row" id="trDefaultReturnMessage">
                                    <div class="col-md-12 col-sm-12 col-xs-12 deAcctive-msgContainer">
                                        <p>This will close/cancel this workflow and move it to the last stage. If configured, a notification will also be sent.</p>
                                        <p>Are you sure you want to proceed? If so please enter the reason below.<span style="color:red">*</span></p>
                                    </div>
                                </div>
                                <div class="row" id="trLEDReturnMessage">
                                    <div class="col-md-12 col-sm-12 col-xs-12 deAcctive-msgContainer">
                                        <p>This will close the lead.</p>
                                        <p>Are you sure you want to proceed? If so please enter the reason below.<span style="color:red">*</span></p>
                                    </div>
                                </div>
                                <div class="row" id="trCPRLossDefaultReturnMessage">
                                    <div class="col-md-12 col-sm-12 col-xs-12 deAcctive-msgContainer">
                                        <p>Are you sure you want to proceed? If so please enter the reason below.<span style="color:red">*</span></p>
                                    </div>
                                </div>
                                <div class="row" id="trLEDDropDownList">
                                    <div class="col-sm-12 col-xs-12 col-md-12 noPadding">
                                        <br />
                                        <asp:DropDownList ID="ddlLEDRejectType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="row">
                                    <div>
                                        <asp:Label ID="lblRejectMessage" runat="server" Font-Size="Smaller" ForeColor="Red" Text=""> </asp:Label>
                                    </div>
                                </div>
                                <div class="row" id="trPopRejectedComments">
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                        <asp:TextBox ID="popedRejectComments" runat="server" CssClass="form-control bg-light-blue deActive-commentBox" 
                                            Text="" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" id="trPopLossRejectedComments">
                                    <div class="accomp_inputField popLossRejectComment-textBox">
                                        <asp:TextBox ID="popedLossRejectComments" runat="server" CssClass="form-control bg-light-blue deActive-commentBox" 
                                             onBlur="LossMessageChanges()" Text="" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" id="trCheckboxSendLossEmail">
                                    <div class="deActive-sendMail">
                                        <asp:CheckBox ID="chkSendLossEmail" runat="server" CssClass="crm-checkWrap" Checked="true" OnClick="ShowHideLossNotificationControl()" Text="Send Email: " />
                                    </div>
                                </div>
                                <div class="row" id="trLossEmailNotification" runat="server">
                                    
                                        <div class="ms-formtable accomp-popup noPadding" width="100%">
                                            
                                            <div  class="row">
                                                <div id="tr3" runat="server" class="col-xs-6 col-md-6 col-sm-6 noPadding">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel closeCancel-fieldLabelWrap">
                                                        </h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField closeCancel-fieldWrap nopadding">
                                                        <asp:CheckBox ID="cbIncludeLossActionUser" runat="server" Text="To (Action User)" Checked="false" CssClass="crm-checkWrap"/>
                                                        <asp:HiddenField ID="hndLossGridData" runat="server" />
                                                        <asp:Label ID="lbLossActionUser" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                                <div id="trToBox" runat="server" class="col-xs-6 col-md-6 col-sm-6 noPadding">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel closeCancel-fieldLabelWrap">TO</h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField closeCancel-fieldWrap">
                                                        <ugit:UserValueBox ID="txtTo" runat="server" MaximumHeight="30" CssClass="peAssignedTo userValueBox-dropDown scrmDropDown_field" isMulti="true" />
                                                    </div>
                                                </div>
                                                <div id="tr2" runat="server" class="col-xs-6 col-md-6 col-sm-6 noPadding">
                                                    <div class="ms-formlabel">
                                                        <h3 class="ms-standardheader budget_fieldLabel closeCancel-fieldLabelWrap">CC</h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField closeCancel-fieldWrap">
                                                        <ugit:UserValueBox ID="lossGridLookup" MaximumHeight="30" CssClass="peAssignedTo userValueBox-dropDown scrmDropDown_field" runat="server" 
                                                            isMulti="true" />
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <div class="row" id="tr4" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel closeCancel-fieldLabelWrap">Mail Subject</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField closeCancel-fieldWrap">
                                                    <asp:TextBox ID="txtMailLossSubject" runat="server" Text="Ticket Email" />
                                                </div>
                                            </div>
                                            <div class="row" id="tr7" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel closeCancel-fieldLabelWrap">Email Body</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField closeCancel-fieldWrap">
                                                    <div style="height: 250px">
                                                        <dx:ASPxHtmlEditor ID="EmailLossHtmlBody" runat="server" ClientInstanceName="EmailLossHtmlBody" Height="250px" Width="100%">
                                                            <Settings AllowHtmlView="false" AllowPreview="false" />
                                                        </dx:ASPxHtmlEditor>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    
                                </div>
                                <div class="row addEditPopup-btnWrap">
                                     <dx:ASPxButton runat="server" ID="ASPxButton1" Name="Reject" AutoPostBack="false" 
                                        Text="No" CssClass="secondary-cancelBtn" >
                                        <ClientSideEvents Click="function (s,e) { commentsRejectPopup.Hide();}" />
                                    </dx:ASPxButton>
                                    <dx:ASPxButton runat="server" ID="btnrejectButton" Name="Reject" AutoPostBack="false" 
                                        Text="Yes, Proceed" CssClass="primary-blueBtn">
                                        <%--<ClientSideEvents Click="function(s,e){ActionContainer('btnrejectButton');}" />--%>
                                        <ClientSideEvents Click="function(s,e){ RejectButtonClicked(s,e);}" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    </dx:ASPxPopupControl>
                   <%-- <script>
                        $(document).ready(function () {
                            $(".addComment_popUp").parent().addClass("addComment_popUp_parent");
                        })
                    </script>--%>
                    <dx:ASPxPopupControl ClientInstanceName="commentsAddPopup" Modal="true"
                        PopupElementID="addCommentButton" ID="commentsAddPopup" SettingsAdaptivity-Mode="Always"
                        ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Add Comment" 
                        Font-Names="'poppins', sans-serif" AllowDragging="true" AllowResize="true"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                        EnableHierarchyRecreation="True">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                                    <div class="ms-formtable accomp-popup ">
                                        <div class="row" style="padding:15px 0px;">
                                            <div class="ms-formbody accomp_inputField">
                                                <asp:TextBox runat="server" ID="txtAddComment" CssClass="aspControl-TextArea"  Style="height:110px !important" TextMode="MultiLine" 
                                                    Text=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="ms-formbody accomp_inputField crm-checkWrap">
                                                 <asp:CheckBox ID="chkAddPrivate" runat="server" Checked="true" onmouseover="this.className='tabhover'" 
                                                     onmouseout="this.className=''" Text="Private" />
                                                 <asp:CheckBox ID="chkNotifyRequestor" runat="server" onmouseover="this.className='tabhover'" 
                                                     onmouseout="this.className=''" Text="Notify Requestor" onclick="ShowhideDependent(this);"  />
                                                 <asp:CheckBox ID="chkPlainText" runat="server" CssClass="clsshowhide" Text="Plain Text" />
                                                 <asp:CheckBox ID="chkDisableTicketLink" runat="server" CssClass="clsshowhide" Text ="Disable Link"  />
                                            </div>
                                        </div>
                                        <div class="row addEditPopup-btnWrap">
                                            <dx:ASPxButton runat="server" ID="LinkButton6" AutoPostBack="false" Text="Cancel" CssClass="secondary-cancelBtn">
                                                <ClientSideEvents Click="function(s,e){commentsAddPopup.Hide();}" />
                                            </dx:ASPxButton>
                                            <dx:ASPxButton runat="server" ID="addCommentBt" Name="Comment" AutoPostBack="false" Text="Add Comment" 
                                                CssClass="primary-blueBtn">
                                                <ClientSideEvents Click="function(s,e){ActionContainer('addCommentBt');}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>


                    <%--To show confirm to Close popop for Sub-Ticket Closing-- :start --%>

                    <asp:HiddenField ID="hdnCloseTicketType" runat="server" />
                    <dx:ASPxPopupControl ClientInstanceName="confirmCloseTicketPopup" Modal="true" PopupElementID="returnWithConformationButtionB" ID="confirmCloseTicketPopup"
                        ShowFooter="false" ShowHeader="true" HeaderText="Close Sub-Ticket(s)" runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Middle" EnableHierarchyRecreation="true">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="conformToClosePopup6" runat="server">
                                <div style="float: left; height: 200px; width: 400px;" class="first_tier_nav">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label Text="This ticket has one or more sub-tickets. Do you want to close the sub-ticket(s) as well? If so, please enter the resolution description below:" runat="server" Style="float: left; margin-bottom: 3px;"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtConfrmToCloseComment" CssClass="txtConfrmToCloseComment" Width="400px" Columns="52" Rows="8" TextMode="MultiLine" Style="resize: none;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="buttoncell">
                                                <ul style="float: right">
                                                    <li runat="server" id="Li78" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" style="color: red">
                                                        <dx:ASPxButton runat="server" ID="ASPxButtoncloseCurrent" Name="Comment" AutoPostBack="false" Text="Close Sub-Ticket(s)">
                                                            <Image Url="/Content/Images/cancel.png" Height="16px"></Image>
                                                            <ClientSideEvents Click="function(s,e){closeCurrentTicketPopupAction(this ,'1');}" />
                                                        </dx:ASPxButton>

                                                        <%--  <a href="javascript:closeCurrentTicketPopupAction(this ,'1');" style="color: white"
                                                        class="close">Close Sub-Ticket(s)</a>--%>
                                                    </li>

                                                    <li runat="server" id="Li77" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" style="color: red">
                                                        <%--   <a href="javascript:closeCurrentTicketPopupAction(this, '0');" style="color: white"
                                                        class="close">Close Parent Only</a>--%>
                                                        <dx:ASPxButton runat="server" ID="ASPxButtonCloseParent" Name="Comment" AutoPostBack="false" Text="Close Parent Only">
                                                            <Image Url="/Content/Images/cancel.png" Height="16px"></Image>
                                                            <ClientSideEvents Click="function(s,e){closeCurrentTicketPopupAction(this, '0');}" />
                                                        </dx:ASPxButton>
                                                    </li>
                                                    <li runat="server" id="Li23" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                        <%--<a style="color: white" class="cancelwhite" onclick="confirmCloseTicketPopup.Hide();" href="javascript:void(0);">Cancel</a>--%>
                                                        <dx:ASPxButton runat="server" ID="ASPxButtonClose" Name="Comment" AutoPostBack="false" Text="Cancel">
                                                            <Image Url="/Content/Images/cancel.png" Height="16px"></Image>
                                                            <ClientSideEvents Click="function(s,e){confirmCloseTicketPopup.Hide();}" />
                                                        </dx:ASPxButton>
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <%--To show confirm to Close popop for Sub-Ticket Closing-- :end --%>



                    <%--To show ORP/PRP Change Popup -- :start --%>

                    <dx:ASPxPopupControl ClientInstanceName="PRPorORPChangePopup" Modal="true"
                        PopupElementID="returnWithPRPorORPChangeButtionB" ID="PRPorORPChangePopup"
                        ShowFooter="false" ShowHeader="true" HeaderText="PRP/ORP Change"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">

                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PRPorORPChangeControlPopup" runat="server">

                                <div style="float: left; height: 200px; width: 400px;" class="first_tier_nav">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" Text="You have added a new PRP or ORP. If you have any comments related to this, please enter them below:" runat="server" Style="float: left; margin-bottom: 3px;"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPRPorORPChangeComment" Width="400px" Columns="52" Rows="8" TextMode="MultiLine" Style="resize: none;"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="buttoncell">
             
                                                <ul style="float: right;display:flex;margin-top:15px;">
                                                    <li runat="server" id="Li178" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" style="margin-right:10px;list-style:none">
                                                        <div class="primary-blueBtn">
                                                            <div class="dxb" style="height:31px">
                                                                <a id="addSavePRPorORPChngeBt" href="javascript:closeOrpPrpChangePopupAction(this);" style="color: #fff;font-weight: 500;padding:0">Save</a>
                                                            </div>
                                                        </div>
                                                    </li>

                                                    <li runat="server" id="Li25" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'" style="list-style:none">
                                                        <div class="primary-blueBtn">
                                                            <div class="dxb" style="height:31px">
                                                                <a class="primary-blueBtn" onclick="PRPorORPChangePopup.Hide();" href="javascript:void(0);" style="color: #fff;font-weight: 500;padding:0">Cancel</a>
                                                            </div>
                                                        </div>
                                                    </li>
                                              </ul>
                                            </td>
                                        </tr>
                                    </table>
                                </div>

                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <%--To show ORP/PRP Change Popup -- :End --%>


                    <dx:ASPxPopupControl ClientInstanceName="commentsReturnPopup" Modal="true" ID="commentsReturnPopup" Width="400px"
                        ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Return Feedback" SettingsAdaptivity-Mode="Always"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                        EnableHierarchyRecreation="True">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                                <div class="configVariable-popupWrap">
                                    <div class=" ms-formtable accomp-popup ">
                                        <div class="row mb-3">
                                            <div class="ms-formlabel">
                                                <h3 class="ms-standardheader budget_fieldLabel">
                                                    <asp:Label ID="lblReturnMessage" runat="server" Text="" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                                                </h3>
                                            </div>
                                            <div class="ms-formbody">
                                                 <asp:TextBox runat="server" ID="popedReturnComments" TextMode="MultiLine" Text=""></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="d-flex justify-content-end">
                                            <dx:ASPxButton Text="Cancel" ID="cancelBtn" CssClass="secondary-cancelBtn" runat="server" AutoPostBack="false">
                                                    <ClientSideEvents Click="function(s,e){commentsReturnPopup.Hide();}" />
                                            </dx:ASPxButton>
                                             <dx:ASPxButton runat="server" CssClass="primary-blueBtn" AutoPostBack="false" ID="returnButtond" Name="Return" 
                                                 Text="OK">
                                                    <ClientSideEvents Click="function(s,e){ActionContainer('returnButtond')}" />
                                            </dx:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                        <!--   Status Popup Starts here-->
                    <dx:ASPxPopupControl ClientInstanceName="statusPopup" Modal="true" ClientSideEvents-EndCallback="endCallbackStatuspopup" Width="100%"
                        ID="statusPopup" SettingsLoadingPanel-ShowImage="false" SettingsLoadingPanel-Enabled="false" AllowResize="true"  
                        ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Status" 
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True"
                        OnWindowCallback="commentsAwardPopup_WindowCallback" SettingsAdaptivity-Mode="Always">
                        <ClientSideEvents EndCallback="endCallbackStatuspopup" />
                        <SettingsLoadingPanel Enabled="False" ShowImage="False" />
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="statuPopupControlContentControl" runat="server">
                                <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

                                    $(document).ready(function () {
                                        $('#lblRmm-showEmail').attr('for', $('#<%=chkOPMEmail.ClientID%>').attr('id'));
                                        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
                                        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
                                    });

                                    $(function () {
                                        if ($("#<%=hndOpsTitle.ClientID%>").val().length > 0)
                                            $("#<%= txtStatusMailSubject.ClientID %>").val('Assign to PreCon' + ': ' + $("#<%=hndOpsTitle.ClientID%>").val());
                                    });

                                    function CloseMTPUserGroupLookupForStatus() {

                                    }

                                    function showhideMoveToPreconMailToFromStatus(obj) {
                                        if ($(obj).find("input:checkbox").get(0).checked) {
                                            $('#trStatusMailTo').css('display', 'none');
                                        }
                                        else {
                                            $('#trStatusMailTo').css('display', '');
                                        }
                                    }

                                    
                                </script>
                                
                                    <div id="trOpportunityStatus" runat="server" visible="false" class="col-sm-3 col-md-3 noPadding">
                                                    <div class="ms-formtable accomp-popup" style="border-collapse: collapse" width="100%">
                                                        <div class="ms-formlabel">
                                                            <h3 class="ms-standardheader budget_fieldLabel">Opportunities Status </h3>
                                                        </div>
                                                        <div class="ms-formbody accomp_inputField">
                                                            <asp:DropDownList ID="ddlOpportunityStatus" runat="server" EnableViewState="true" onchange="StatusChange()" CssClass="aspxDropDownList">
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hndOpsTitle" runat="server" />
                                                        </div>
                                                    </div>
                                                </div>

                                            <div class="row" id="tr10" runat="server">
                                                
                                                <div class="awardDate col-sm-6 col-md-6 noPadding" id="trTicketAwardLossDate" runat="server">
                                                    <div class="ms-formtable accomp-popup" style="border-collapse: collapse" width="100%">
                                                        <div class="ms-formlabel">
                                                            <dx:ASPxLabel ID="lblAwardLoss" runat="server"></dx:ASPxLabel>
                                                            
                                                        </div>
                                                        <div class="ms-formbody accomp_inputField">
                                                            <dx:ASPxDateEdit ID="dtcTicketAwardLossDate" runat="server" CssClass="CRMDueDate_inputField" ClientInstanceName="dtcTicketAwardLossDate" >
                                                                <TimeSectionProperties Visible="false"></TimeSectionProperties>
                                                                <ClientSideEvents Init="function(s,e){ var date = new Date(); s.SetDate(date); }" />
                                                            </dx:ASPxDateEdit>
                                                        </div>
                                                     </div>
                                                </div>
                                                <div id="trddlStageToMove" runat="server" class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                                        <div class="ms-formtable accomp-popup" style="border-collapse: collapse" width="100%">
                                                            <div class="" id="tr12" runat="server">
                                                                <div class="ms-formlabel">
                                                                    <h3 class="ms-standardheader budget_fieldLabel">Select Stage to Move: </h3>
                                                                </div>
                                                                <div class="ms-formbody accomp_inputField">
                                                                    <asp:DropDownList ID="ddlStageToMove" runat="server" CssClass="aspxDropDownList"></asp:DropDownList>
                                                                    <asp:CheckBox ID="chkStatusOpenCPR" runat="server" Visible="false" Font-Bold="true" onchange="showhideMoveToPreconMailToFromStatus(this)" Text="Open CPR record" />
                                                                    <dx:ASPxHiddenField ID="hndMTPUserGroupForStatus" runat="server">
                                                                    </dx:ASPxHiddenField>
                                                                    <asp:HiddenField ID="hdnStagetoMove" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                </div>
                                            </div>
                                            
                                        <div class="row noPadding">
                                             <div class="ms-formtable accomp-popup row">
                                                <div class="col-sm-6 col-md-6 statusComment-field noPadding">
                                                    <div class="ms-formtable accomp-popup" style="border-collapse: collapse" width="100%">
                                                     <div class="ms-formlabel">
                                                         <h3 class="ms-standardheader budget_fieldLabel">
                                                            <asp:Label CssClass="status_commentLabel" ID="lblComment" 
                                                                runat="server" Text="Comment"></asp:Label>
                                                         </h3>
                                                     </div>
                                                     <div class="ms-formbody accomp_inputField">
                                                         <dx:ASPxMemo ID="txtStatusMessage" runat="server" ClientInstanceName="txtStatusMessage" Width="100%" Height="70px" CssClass="statusMemo">
                                                             
                                                         </dx:ASPxMemo>
                                                         <%--<asp:TextBox ID="txtStatusMessage" runat="server" CssClass="asptextbox-asp" Text="" onBlur="StatusMessageChanges()" TextMode="MultiLine"></asp:TextBox>--%>
                                                         <asp:HiddenField ID="hndOpportunityStatus" runat="server" Value="" />
                                                         <asp:HiddenField ID="hndCompleteTasksOnItemClose" runat="server" Value="false" />
                                                         <asp:HiddenField ID="hdnIgnoreConstraintValidation" runat="server" Value="false" />
                                                         <asp:HiddenField ID="hndERPJobID" runat="server" Value="" />
                                                     </div>
                                                        </div>
                                                </div>
                                                <div class="col-sm-6 col-md-6 noPadding">
                                                    <div class="ms-formtable" style="border-collapse: collapse" width="100%">
                                                    <div class="ms-formlabel">
                                                         <h3 class="ms-standardheader budget_fieldLabel">
                                                              <asp:Label CssClass="status_reasonLabel" ID="lblReason" runat="server" Text="Reason"></asp:Label>
                                                             <span id="madatoryLabel" runat="server" style="color: red;" visible="false">*</span>
                                                         </h3>
                                                    </div>
                                                    <div class="ms-formbody accomp_inputField">
                                                        <dx:ASPxMemo ID="txtStatusResson" ClientInstanceName="txtStatusResson" runat="server" Width="100%" Height="70px" CssClass="statusMemo">
                                                            <ClientSideEvents TextChanged="ReasonMessageChanges" LostFocus="ReasonMessageChanges" />
                                                        </dx:ASPxMemo>
                                                        
                                                    </div>
                                                        </div>
                                                </div>
                                            </div>
                                        </div>
                                         
                                        <div class="row fieldWrap" style="padding-bottom:8px; padding-left:7px;">
                                            <div>
                                                <div class="rmm-chkWrap crm-checkWrap">
                                                    <asp:CheckBox ID="chkOPMEmail" runat="server" Font-Bold="true" Checked="true" 
                                                        Text="Email" onchange="showhideStatusMailOption(this)"/>
                                                </div>
                                            </div>
                                            </div>
                                        <div class="row noPadding" id="trStageToMove" runat="server" style="display:none;">
                                            <div class="rmm-chkWrap crm-checkWrap" style="padding-left:7px; padding-bottom:8px;">
                                                <asp:CheckBox ID="chkboxStageMoveToPrecon" runat="server" Checked="true" onchange="showhideStageToMove()" Text="Assign to PreCon" CssClass="preconChk" />
                                                <asp:Label ID="lblPermissionMsg" runat="server" Visible="false" Text="(You are not authorized to Assign to PreCon. Please contact administrator.)" CssClass="preconErrMsg"></asp:Label>

                                            </div>
                                        </div>
                                    
                                    <div class="row" id="trOPSMailBody" runat="server">
                                        <div class="ms-formtable accomp-popup">
                                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr14" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">To</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField mailTo_field">
                                                    <ugit:UserValueBox ID="statusMTPUserGroups" Width="100%" MaximumHeight="30" CssClass="peAssignedTo userValueBox-dropDown" 
                                                        runat="server" isMulti="true"/>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">CC</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField mailTo_field">
                                                    <ugit:UserValueBox ID="ccUsersBox" Width="100%" MaximumHeight="30" CssClass="peAssignedTo userValueBox-dropDown" 
                                                        runat="server" isMulti="true"/>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 colForXS noPadding fieldWrap" id="tr13" runat="server" style="display:none">
                                                <div class="crm-checkWrap" style="padding-left:7px; padding-bottom:8px;">
                                                    <asp:CheckBox ID="chkOPSActionUser" runat="server" Checked="false" Text="CC to Action User" />
                                                    <asp:Label ID="lblStatusActionUser" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="col-md-6 col-sm-6 colForXS noPadding" id="tr11" runat="server" style="display:none">
                                                <div class="crm-checkWrap" style="padding-left:7px;padding-bottom:8px;">
                                                    <asp:CheckBox ID="chkSendMailFromLoggedInUserOPM" Checked="false" Text="Send Mail from Logged In User" runat="server" onchange="sendMailFromStatus(this)" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding mailSub" id="tr15" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Mail Subject</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <asp:TextBox ID="txtStatusMailSubject" runat="server" Text="Assign To Precon" Width="350px" />
                                                </div>
                                            </div>
                                            <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="tr16" runat="server">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Email Body</h3>
                                                </div>
                                                <div class="ms-formbody accomp_inputField">
                                                    <div style="height: 250px; overflow-y:auto">
                                                        <dx:ASPxHtmlEditor ID="txtStatusMailBody" runat="server" ClientInstanceName="txtStatusMailBody" Height="300px" Width="100%">
                                                            <Settings AllowHtmlView="false" AllowPreview="false" />
                                                        </dx:ASPxHtmlEditor>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row addEditPopup-btnWrap" style="clear:both;">
                                        <ul class="statusBtn_wrap">
                                            <li id="liCancelStatusPopup" runat="server" class="" onmouseout="this.className=''" onmouseover="this.className='tabhover'">
                                                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn">
                                                    <ClientSideEvents Click="function(s, e){CloseStatusPopup(s, e);}" />
                                                </dx:ASPxButton>
                                            </li>
                                             <li id="liStatus" runat="server" class="" onmouseout="this.className=''" onmouseover="this.className='tabhover'">
                                                 <dx:ASPxButton ID="lnkStatus" runat="server" Text="OK" ToolTip="OK" OnClick="Button_Click" CssClass="primary-blueBtn">
                                                    <ClientSideEvents Click="function(s, e){return StatusButtonClicked(s,e);}" />
                                                </dx:ASPxButton>
                                             </li>
                                             <li id="liMoveToPrecon" runat="server" class="okStatus_link" onmouseout="this.className=''" onmouseover="this.className='tabhover'" style="color: red; display: none;">
                                                  <dx:ASPxButton ID="MoveToPrecon" runat="server" CssClass="primary-blueBtn" OnClick="Button_Click" Text="Move" ToolTip="Move">
                                                       <ClientSideEvents Click="function(s, e){return TicketMoveToPrecon();}" />
                                                  </dx:ASPxButton>
                                             </li>
                                        </ul>
                                        
                                        
                                    </div>
                                
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <!--   Incident Notification Popup Starts here-->
                    <dx:ASPxPopupControl ClientInstanceName="incidentNotificationPopup" Modal="true"
                        PopupElementID="notificationButtonB" ID="incidentNotificationPopup"
                        ShowFooter="false" ShowHeader="true" CssClass="departmentPopup" HeaderText="Outage Notification"
                        runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                                <div style="height: auto; width: 485px;" class="first_tier_nav">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="text-align: right; padding-right: 5px;">To: </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNotificationTo" Text="" Width="300px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td width="30%">&nbsp;</td>
                                            <td width="70%">
                                                <asp:CheckBox ID="chkImpactedUser" runat="server" Text="Include Impacted Users" /></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; padding-right: 5px;">
                                                <asp:Label ID="lblIncidentBody" runat="server" Text="Expected Resolution"> </asp:Label></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNotificationBody" TextMode="MultiLine" Text="" Rows="5" Width="300px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; padding-right: 5px;">
                                                <asp:Label ID="lblActions" runat="server" Text="Action Required by Users" Visible="false"> </asp:Label></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtActions" TextMode="MultiLine" Text="" Visible="false" Rows="5" Width="300px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" class="buttoncell">
                                                <ul>
                                                    <li runat="server" id="Li9" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                        <dx:ASPxButton runat="server" ID="btnSendNotification"
                                                            Text="Send Notification">
                                                            <ClientSideEvents Click="function(s,e){return SendNotification();return false;}" />
                                                        </dx:ASPxButton>

                                                    </li>
                                                    <li runat="server" id="Li10" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                                        <dx:ASPxButton ID="canNotBtn" runat="server">
                                                            <ClientSideEvents Click="function(s,e){incidentNotificationPopup.Hide();}" />

                                                        </dx:ASPxButton>


                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                    </dx:ASPxPopupControl>

                    <!--  Request Type change confirmation Popup Starts here-->
                    <dx:ASPxPopupControl ID="pcRequestTypeChange" runat="server" CloseAction="CloseButton" Modal="True" Width="370px"
                        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcRequestTypeChange"
                        HeaderText="Request Type Changed" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
                        <ContentCollection>
                            <dx:PopupControlContentControl ID="pcccRequestTypeChange" runat="server">
                                <dx:ASPxPanel ID="ASPxPanel2" runat="server" DefaultButton="btnRequestTypeChangeOk">
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <table style="width: 100%; height: 70px;">
                                                <tr>
                                                    <td class="pcmCellCaption">
                                                        <dx:ASPxLabel ID="lblRequestTypeChangeMessage" runat="server" Text="Do you want to clear the assignees and return the request?">
                                                        </dx:ASPxLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="buttoncell">
                                                            <span style="float: right; padding-right: 1px;">
                                                                <dx:ASPxButton ID="btRequestTypeChangeCancel" runat="server" ClientInstanceName="dxRequestTypeChangeCancel" Text="No, Keep Assigned" Width="50px" ToolTip="Keep Current Assignees" AutoPostBack="false"
                                                                    CausesValidation="false" Style="float: right; margin-right: 1px;">
                                                                    <ClientSideEvents Click="function(s, e) { pcRequestTypeChange.Hide(); }" />
                                                                </dx:ASPxButton>
                                                            </span>

                                                            <span style="float: right; padding-right: 1px;">
                                                                <dx:ASPxButton ID="btnRequestTypeChangeOk" ClientInstanceName="btnRequestTypeChangeOk" runat="server"
                                                                    Text="Yes, Clear Assignment" Width="50px" Style="float: right; margin-right: 5px" ToolTip="Clear Current Assignees"
                                                                    OnClick="btnRequestTypeChangeOk_Click">
                                                                </dx:ASPxButton>
                                                            </span>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </table>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxPanel>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                        <ContentStyle>
                            <Paddings PaddingBottom="5px" />
                        </ContentStyle>
                    </dx:ASPxPopupControl>

                    <%--    <div style="float: left; width: 25%; padding-top: 10px;">
                        <dx:ASPxButton ID="lnkbtnActionMenu" runat="server" ClientVisible="false" Text="Actions" ImagePosition="Right" AutoPostBack="false">
                            <Image Url="/Content/Images/icon-arrow-down-b.png"></Image>
                        </dx:ASPxButton>

                        <dx:ASPxPopupMenu ID="ASPxPopupActionMenu" OnLoad="ASPxPopupActionMenu_Load" runat="server" PopupElementID="lnkbtnActionMenu" CloseAction="MouseOut" ItemSpacing="0" SubMenuStyle-ItemSpacing="0"
                            ClientInstanceName="ASPxPopupActionMenu" PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Above" PopupAction="LeftMouseClick">
                            <Items>
                            </Items>
                            <ClientSideEvents ItemClick="function(s, e) { popupMenuActionMenuItemClick(s,e);}" />
                            <%--<ItemStyle CssClass="dxb" BackgroundImage-ImageUrl="/Content/Images/firstnavbg1X28.gif" BackgroundImage-Repeat="RepeatX" Height="28px" ForeColor="White" VerticalAlign="Middle"
                            HoverStyle-BackgroundImage-ImageUrl="/Content/Images/firstnavbg_hover1X28.gif"
                            HoverStyle-BackgroundImage-Repeat="RepeatX" DropDownButtonStyle-CssClass="dropdownicon"></ItemStyle>
                            <ItemStyle CssClass="dxb" BackColor="#ebedf2" HoverStyle-BackColor="Blue"></ItemStyle>
                            <%-- <SubMenuItemStyle BackgroundImage-ImageUrl="/Content/Images/firstnavbg1X28.gif" BackgroundImage-Repeat="RepeatX" Height="28px" ForeColor="White" VerticalAlign="Middle"
                            HoverStyle-BackgroundImage-ImageUrl="/Content/Images/firstnavbg_hover1X28.gif" HoverStyle-BackgroundImage-Repeat="RepeatX">
                        </SubMenuItemStyle>
                        </dx:ASPxPopupMenu>
                    </div>--%>
                </div>
            </div>
            <%-- Module details --%><%--float: left;--%>
            <div class="moduleitemdetail111 col-md-12 col-sm-12 col-xs-12 noPaddingLeft">
                <asp:Repeater runat="server" ID="tabRepeater" OnItemDataBound="ModuleTabItemCreated">
                    <ItemTemplate>
                        <div id="tabPanelContainer_<%# Eval("TabSequenceOnScreen")%>" style="display: none; float: none; width: 100%;" class="tabcontent11">
                            <asp:Panel runat="server" ID="tabContainer" CssClass="tabContainer field_heading" Width="100%">
                                <asp:Table runat="server" ID="tabTable">
                                </asp:Table>
                            </asp:Panel>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <%--  <div style="float: left; width: 100%">
            </div>--%>

            <%-- Utility UI which includes actions buttons and popup UI --%>
        </asp:Panel>

        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." ClientInstanceName="LoadingPanel" Modal="True" ImagePosition="Top" CssClass="customeLoader">
            <Image Url="~/Content/Images/ajaxloader.gif"></Image>
        </dx:ASPxLoadingPanel>

        <asp:Panel ID="panelNewTicket" runat="server" CssClass="col-md-12 col-sm-12 col-xs-12">
            <asp:Panel runat="server" ID="newTopGraphicPanel">
                <div class="row">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="row">
                                    <div class="middleBox createTicket_heading_wrap">
                                        <asp:Literal ID="initialStageDescriptionLiteral" runat="server"></asp:Literal>
										<asp:Panel ID="HelpTextNewTicket" runat="server" Style="float: right;">
                                		</asp:Panel>
                                    </div>
                                </div>
                            </div>

                            <div class="row nonSvc_createTicketWorkflowWrap">
                                <div class="col-md-12 col-sm-12 col-xs-12">
                                    <div id="NewTicketWorkFlowDiv" class="contract_steps_module" runat="server">
                                        <div class="contract_steps_container">
                                            <div class="contract_steptop_content">

                                                <div class="row SVCWorkflow-wrap">
                                                    <div class="svcWorkflow-container">
                                                     <div class="col-md-12" runat="server" id="div2">
                                                            <div class="wizard_steps float-left">
                                                                <nav class="steps">

                                                                    <asp:ListView ID="NewTicketWorkFlowListView" runat="server" ItemPlaceholderID="PlaceHolder1" DataKeyNames="ID" OnItemDataBound="NewTicketWorkFlowListView_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                                        </LayoutTemplate>
                                                                        <ItemTemplate>
                                                                            <div class="step svcWorkFlow-Step">
                                                                                <div id="activeIconDiv" runat="server" class="step_content employee-info">                                                                                   
                                                                                    <div id="divHelp" runat="server" class="wf-img-wrap" >                                                                                       
                                                                                        <p  id="stepIcon" class="step_number" runat="server">
                                                                                            <img id="imgSection" runat="server" visible="false" width="35"  src="~/Content/Images/add-group.jpg"/>
                                                                                        </p>
                                                                                    </div>
                                                                                    <small>
                                                                                        <asp:HiddenField runat="server" ID="ItemOrder" Value='<%# Eval("ID") %>' />
                                                                                        <asp:Label runat="server" ID="sectionSideBarContainer" ></asp:Label>
                                                                                    </small>
                                                                                    <div class="lines">
                                                                                        <div class="line -background bg-col-blue" id="lineWorkflow" runat="server">
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:ListView>
                                                                </nav>
                                                            </div>
                                                     </div>
                                                </div>
                                                </div>

                                                <table style="text-align: center; border-collapse: collapse;" width="98%">
                                                    <tr>
                                                        <td align="center">
                                                            <table style="text-align: center; border-collapse: collapse;">
                                                                <tr>
                                                                    <asp:Repeater ID="newTicketStageRepeater" runat="server" OnItemDataBound="StageRepeater_ItemDataBound">
                                                                        <ItemTemplate>
                                                                            <td id="tdStage" runat="server" style="height: 100px; width: 38px; background-repeat: no-repeat;">
                                                                                <div style="position: relative">
                                                                                    <span class="pos_rel" style="height: 100px"><i id="stageNo" runat="server" >
                                                                                        <asp:Literal ID="lbStageNumber" runat="server"></asp:Literal>
                                                                                    </i>
                                                                                    </span>
                                                                                    <span class="stage-titlecontainer flowlable" id="stageTitleContainer" runat="server">
                                                                                        <b class="pos_rel "
                                                                                            style="">
                                                                                            <asp:Literal ID="stageTitle" runat="server"></asp:Literal>
                                                                                        </b></span>
                                                                                    <i id="activeStageArrow" runat="server"></i>
                                                                                </div>
                                                                            </td>
                                                                            <td id="tdStepLine" runat="server" style="height: 38px; background-repeat: repeat-x;">&nbsp;
                                                                            </td>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8 col-sm-6 col-xs-12 noPadding">
                                    <div class="btn-count-no">
                                        <dx:ASPxButton ID="relatedTitleButton" CssClass="relatedTitleButtonClass" runat="server" Text="Link Related ticket" ClientInstanceName="relatedTitleButton" OnClick="RelatedTitle_Click" Visible="false"></dx:ASPxButton>
                                        <div class="result"></div>
                                    </div>
                                </div>
                                <div class="col-md-4 col-sm-6 col-xs-12">
                                    <div id="Div1">
                                        <span class="createTicket_errorMsg" style="">
                                            <asp:Literal ID="errorMsgNew" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                    <div class="createTicket_errorMsg" style="float: left; width: 100%;margin-bottom:5px;">
                                        <%--<span style="color: red; float: left; height: 15px; text-align: left; padding-top: 2px;">--%>
                                        <span style="color: red; float: left; text-align: left; padding-top: 2px;">
                                            <asp:Literal ID="invalidErrorMsgNew" runat="server"></asp:Literal>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
            </asp:Panel>
            <asp:Table runat="server" ID="newTicketTable" CssClass="field_heading block-table svcDuplicate_popupTable" Style="width: 100%; float: left;">
            </asp:Table>
            <div class="create_ticket_btnWrap">
                <div class="first_tier_nav createTicket_ulList" id="createButtons">
                    <ul class="createTicket_listWrap">
                        <li runat="server" id="createButtonContainer" class="createButtonContainer" style="display: inline;">
                            <dx:ASPxButton ID="createButton" runat="server" Text="Create" CssClass="primary-blueBtn" ImagePosition="Left" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){ActionContainer('createButton');}" />
                                <%--<ClientSideEvents Click="function(s,e){return WaitTillSave()}" />--%>
                            </dx:ASPxButton>
                        </li>
                        <li runat="server" id="saveAsDraftButtonContainer" style="margin-right:5px;" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave saveASDraftBtn_wrap'">
                            <dx:ASPxButton ID="saveAsDraftButton" runat="server" Text="Save as Draft" AutoPostBack="false" CssClass="primary-blueBtn">
                                <ClientSideEvents Click="function(s,e){ActionContainer('saveAsDraftButton');}" />
                                <%--<ClientSideEvents Click="function(s,e){return WaitTillSave()}" />--%>
                            </dx:ASPxButton>
                        </li>

                        <li runat="server" id="createCloseButtonContainer" style="display: none" class="createCloseButtonContainer" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="createCloseButton" runat="server" Text="Create & Close" AutoPostBack="false">

                                <ClientSideEvents Click="function(s,e){ActionContainer('createCloseButton');}" />
                                <%-- <ClientSideEvents Click="function(s,e){return WaitTillSave()}" />--%>
                            </dx:ASPxButton>
                        </li>
                        <li class="" id="Li2" runat="server">
                            <dx:ASPxButton Text="Cancel" ID="btnC" runat="server" ImagePosition="Left" AutoPostBack="false" CssClass="secondary-cancelBtn">

                                <ClientSideEvents Click="function(s,e){ClosePopUp();}" />
                            </dx:ASPxButton>
                        </li>
                    </ul>
                </div>
            </div>

        </asp:Panel>

        <dx:ASPxPopupControl ID="relatedassetticket" runat="server" Width="370px" HeaderStyle-CssClass="hide" ClientInstanceName="relatedassetticket"
            AllowDragging="false" PopupAnimationType="Slide" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl ID="popcontrolasset" runat="server">
                    <div id="relatedAssetPopupDiv">
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="relatedassetticketbefore" runat="server" Width="370px" HeaderStyle-CssClass="hide" ClientInstanceName="relatedassetticketbefore"
            AllowDragging="false" PopupAnimationType="Slide" EnableViewState="False">
            <ContentCollection>
                <dx:PopupControlContentControl ID="popcontrolassetbefore" runat="server">
                    <div id="relatedAssetPopupDivbefore">
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

        <dx:ASPxPopupControl ID="aspxPopupPrintOption" ClientInstanceName="aspxPopupPrintOption" Width="370px" Modal="true"
            ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Print Options" SettingsAdaptivity-Mode="Always" 
            runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl12" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                        <div class="ms-formtable accomp-popup ">
                            <div class="row">
                                <div class="ms-formlabel">
                                    <h3 class=" ms-standardheader budget_fieldLabel ">Select tabs to print:</h3>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="divheight">
                                <dx:ASPxCheckBox ID="chkAll" ClientInstanceName="chkAll" Text="Select All" runat="server" AutoPostBack="false">
                                    <ClientSideEvents CheckedChanged="function(s,e){selectAllTab(s,e);}" />
                                </dx:ASPxCheckBox>
                                <dx:ASPxCheckBoxList ID="chkSelectTabList" ClientInstanceName="chkSelectTabList" AutoPostBack="false" runat="server" ValueField="ID" Border-BorderWidth="0px" TextField="TabName">
                                    <ClientSideEvents SelectedIndexChanged="function(){selectAllTabToPrint();}" />
                                    <Border BorderWidth="0px" />
                                </dx:ASPxCheckBoxList>
                            </div>
                        </div>
                        <div class="row addEditPopup-btnWrap">
                            <dx:ASPxButton ID="lnkCancel" runat="server" Text="Cancel" ToolTip="Cancel" Visible="true" 
                                CssClass="secondary-cancelBtn" >
                                <ClientSideEvents Click="function(s, e){aspxPopupPrintOption.Hide();}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="lnkPrint" runat="server" Text="Print" ToolTip="Print" Visible="true" 
                                CssClass="primary-blueBtn">
                                <ClientSideEvents Click="function(s, e){printSelectedTabs();}" />
                            </dx:ASPxButton>
                        </div>
                        <%--<div class="row">
                            <ul class="printPopup_listWrap">
                                <li runat="server" id="cancelLI" class="printCancel_btnWrap" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                    <asp:HyperLink ID="lnkCancel1" runat="server" onClick=""
                                        Visible="true" ToolTip="Close" Text="Cancel" CssClass="secondary-linkBtn" />
                                </li>
                                <li runat="server" id="printLI" class="printBtn_wrap" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                    <asp:HyperLink ID="lnkPrint1" runat="server" onClick="printSelectedTabs();" CssClass="primary-linkBtn"
                                        Visible="true" ToolTip="Print" Text="Print" />
                                </li>
                            </ul>
                        </div>--%>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
       <%-- <script>
            $(document).ready(function () {
                $('.svcCopyClipBoardPopup').parent().addClass('svcCopyClipBoardPopup_container');
            });
        </script>--%>
        <dx:ASPxPopupControl ID="aspxPopupCopyToClipboard" ClientInstanceName="aspxPopupCopyToClipboard" Width="500px" Height="200px" Modal="true"
            ShowFooter="false" ShowHeader="true" CssClass="aspxPopup" HeaderText="Press Ctrl-C To Copy" SettingsAdaptivity-Mode="Always"
            runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl14" runat="server">
                    <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                        <div class="ms-formtable accomp-popup ">
                            <div class="row">
                                <div class="ms-formlabel">
                                    <h3 class=" ms-standardheader budget_fieldLabel ">Select Tab</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <asp:DropDownList runat="server" ID="ddlSelectTab" onchange="SetSelectedTab(this)" 
                                        CssClass="aspxDropDownList itsmDropDownList"></asp:DropDownList>
                                </div>
                                <div class="row">
                                     <div class="ms-formbody accomp_inputField">
                                          <textarea id="lblticketUrl" class="ITSM-textarea" style="width: 100%; resize: none;" 
                                              onclick="this.focus();this.select();" readonly></textarea>
                                     </div>
                                </div>
                                <div class="row addEditPopup-btnWrap">
                                    <dx:ASPxButton ID="HyperLink2" runat="server" Visible="true" ToolTip="Close" Text="Close" AutoPostBack="false"
                                        CssClass="secondary-cancelBtn">
                                        <ClientSideEvents Click="function(s,e){aspxPopupCopyToClipboard.Hide();}" />
                                    </dx:ASPxButton>
                                </div>
                            </div>
                        </div>
                                <%--<li runat="server" id="Li29" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'"></li>--%>
                    </div>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <ClientSideEvents PopUp="function(s,e){autoSelect();}" />
        </dx:ASPxPopupControl>

        <div id="information" style="height: auto; width: 200px; float: left; padding: 5px; display: none;" class="first_tier_nav ModuleBlock">

            <table style="width: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <b>Description/Instruction:</b>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px;">
                        <div class="informationBox">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="buttoncell">
                        <ul style="float: right">
                            <li runat="server" id="Li16" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                <dx:ASPxButton ID="LinkButton4" runat="server"
                                    Visible="true"
                                    Text="Close">
                                    <ClientSideEvents Click="function(s,e){HideShowInformation();return false;}" />
                                </dx:ASPxButton>
                            </li>
                        </ul>
                    </td>
                </tr>
            </table>

        </div>

        <div id="attachments" style="height: auto; width: 200px; float: left; padding: 5px; display: none;" class="first_tier_nav ModuleBlock">
            <table style="width: 100%; border-collapse: collapse;" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <b>Attachment(s):</b>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 5px;">
                        <div class="attachmentBox">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="buttoncell" style="text-align: right;">
                        <ul style="float: right">
                            <li runat="server" id="Li15" class="" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                                <dx:ASPxButton ID="LinkButton1" runat="server" Visible="true"
                                    Text="Close">
                                    <ClientSideEvents Click="function(s,e){HideAttachmetns();return false;}" />
                                </dx:ASPxButton>
                            </li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>

        <div id="btnDiv" runat="server" class="first_tier_nav edit_ticket_tabIcon editPopup_btnWrap col-md-12 col-xs-12 col-sm-12">
            <div class="row footerBtn_alignXS">
                <div align="left" class="buttoncell col-md-6 col-sm-5 editTicket_footerBtn-xs" style="min-width: 150px;">

                    <%--this is temp button for fill macro template data--%>
                    <dx:ASPxButton ID="tempbutton" runat="server" EnableClientSideAPI="true" ClientVisible="false" AutoPostBack="true" OnClick="tempbutton_Click">
                        <ClientSideEvents Click="function(s,e){waitTillOverrideComplete(this);}" />
                    </dx:ASPxButton>

                    <ul class="btnStyleBottom">
                        <li runat="server" id="awardButtonLI" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <b style="font-weight: normal;" id="awardButtonB">
                                <asp:LinkButton ID="awardButton" OnClientClick="return MoveAwardStage();" runat="server"
                                    Style="color: white" OnClick="Button_Click" ToolTip="Move to Award Stage."
                                    Text="Award" CssClass="footer_actAwardBtn"  />   
                            </b>
                        </li>

                        <li runat="server" id="statusButtonLI" class="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <b style="font-weight: normal;" id="statusButtonB">
                                <asp:LinkButton ID="statusButton" OnClientClick="return CheckOpenTasks('statusButton');" runat="server"
                                    Style="color: white" OnClick="Button_Click" ToolTip="Update opportunity status with appropiate action."
                                    Text="" CssClass="footer_actStatusBtn" />
                            </b>
                        </li>

                        <li runat="server" id="importPMMLI" class="" onmouseover="this.className='tabhover'"
                            onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="importPMMButton" runat="server" AutoPostBack="false"
                                Visible="true" ToolTip="Convert project request to a project"
                                Text="" CssClass="report">
                                <ClientSideEvents Click="function(s,e){ActionContainer('importPMMButton');}" />
                            </dx:ASPxButton>
                        </li>
                        <li runat="server" id="importNPRLI" visible="false" class="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="importNPRButton" AutoPostBack="false" runat="server" Text="Send to PMM" Visible="true" ToolTip="Export project to Project Management Module">
                            </dx:ASPxButton>
                        </li>                        
                        <li runat="server" id="rejectButtonLI" class="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <b style="font-weight: normal;" id="rejectWithCommentsButtonB">
                                <dx:ASPxButton ID="rejectWithCommentsButton" runat="server" AutoPostBack="false" ClientSideEvents-Click="OpenRejectPopup"
                                    Visible="true" ToolTip="Reject & Close Ticket"
                                    Text="Reject & Close Ticket">
                                    <Image Url="/Content/Images/rejectButton.png" Height="14px"></Image>
                                   
                                </dx:ASPxButton></b></li>
                        <li runat="server" id="returnButtonLI" class="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <b style="font-weight: normal;" id="returnWithCommentsButtonB">
                                <dx:ASPxButton ID="returnWithCommentsButton" runat="server" Visible="true" ToolTip="Return ticket to previous stage" Text="" AutoPostBack="false">
                                    <Image Url="/Content/Images/backward.png" Height="16px"></Image>

                                    <ClientSideEvents Click="function(s,e){ ActionContainer('returnWithCommentsButton');}" />
                                </dx:ASPxButton>
                            </b>
                        </li>
                        <li runat="server" id="selfAssignButtonLI" class="section-icon process_icon" onmouseover="this.className='tabhover'"
                            onmouseout="this.className='mouse_Leave'">
                            <b style="font-weight: normal;" id="selfAssignButtonB">
                                <dx:ASPxButton ID="selfAssignButton" runat="server" CssClass="usertick"
                                    Visible="false" Text="Self-Assign" ToolTip="Assign ticket to self">
                                    <ClientSideEvents Click="function(s,e){return false;}" />
                                </dx:ASPxButton></b></li>
                        <li runat="server" id="approveButtonLI" class="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="approveButton" runat="server" AutoPostBack="false"
                                Visible="true" ToolTip="Approve ticket"
                                Text="">
                                <Image Url="/Content/Images/arrow-64.png" Height="16px"></Image>
                                <ClientSideEvents Click="function(s,e){ActionContainer('approve');}" />
                            </dx:ASPxButton>
                            <%--<dx:ASPxButton ID="approvebuttonhidden" ClientVisible="false" CssClass="HideMe" runat="server" OnClick="Button_Click"></dx:ASPxButton>--%>
                            <dx:ASPxButton ID="approveOnTaskComplete" ClientVisible="false" CssClass="HideMe" runat="server" OnClick="approveOnTaskComplete_Click">
                            </dx:ASPxButton>
                        </li>   
                        <li id="advanceToProjectLI" runat="server" class="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                            <b style="font-weight:normal;" id="advanceToProjectB">
                                <asp:LinkButton ID="advanceToProject" runat="server" OnClientClick="return CheckOpenTasks('advanceToProject');"
                                     OnClick="Button_Click" CssClass="footer_actAdanceToProjectBtn"></asp:LinkButton>
                            </b>
                        </li>
                    </ul>
                </div>
                <div class="buttoncell col-md-2 col-sm-3 editTicket_footerBtn-xs">
                    <ul class="btnStyleBottom">
                        <li runat="server" id="closeButtonLI" create_ticket_btnwrap="section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="closeButton" runat="server" Visible="true" ToolTip="Close ticket"></dx:ASPxButton>
                            <dx:ASPxButton ID="btncloseButtonH" CssClass="HideMe" runat="server" AutoPostBack="false">
                                <ClientSideEvents Click="function(s,e){ActionContainer('btncloseButtonH');}" />
                            </dx:ASPxButton>
                        </li>
                        <%--<asp:Button ID="newsuperAdminEditButton" runat="server" style="display:none" OnClick="Button_Click" OnClientClick="return waitTillOverrideComplete(this);" />--%>
                        <li runat="server" id="superAdminEditButtonLI" class="section-icon process_icon" onmouseover="this.className='tabhover'"
                            onmouseout="this.className='mouse_Leave'" visible="false">
                            <dx:ASPxButton ID="btnsuperAdminEditButton" runat="server" Visible="false" ToolTip="Edit all fields using admin permissions" AutoPostBack="false"
                                Text="Override">
                                <Image Url="../../Content/Images/Rewrite_icon.png" Height="16px"></Image>
                                <ClientSideEvents Click="function(s,e){ActionContainer('btnsuperAdminEditButton');}" />
                            </dx:ASPxButton>
                        </li>
                    </ul>
                </div>
                <div class="buttoncell col-md-4 col-sm-4 editTicket_footerBtn-xs">
                    <ul class="footer_saveCancel_btn btnStyleBottom" style="float: right;">

                        <li runat="server" id="updateButtonLI" class="updateButtonLI section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="updateButton" runat="server" AutoPostBack="false" Visible="false" ToolTip="Save changes" Text="Save" CssClass="test-class-123">
                                <Image Url="/Content/Images/saveFile_icon.png" Height="16px"></Image>
                                <ClientSideEvents Click="function(s,e){OpenAutoUpdateDialog('btnupdateButton');}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="hdnUpdateButton" runat="server" ClientVisible="false" AutoPostBack="false" ToolTip="Save changes" Text="Save">
                                <Image Url="/Content/Images/saveFile_icon.png" Height="16px"></Image>
                                <ClientSideEvents Click="function(s,e){ActionContainer('btnupdateButton');}" />
                            </dx:ASPxButton>
                        </li>

                          <li runat="server" id="Li13" class="updateButtonLI section-icon process_icon" onmouseover="this.className='tabhover'" onmouseout="this.className='mouse_Leave'">
                            <dx:ASPxButton ID="updateButtonSC" runat="server" AutoPostBack="false" Visible="false"  ToolTip="Save changes" Text="Save & Close" CssClass="test-class-123">
                                <Image Url="/Content/Images/saveFile_icon.png" Height="16px"></Image>
                                <ClientSideEvents Click="function(s,e){OpenAutoUpdateDialog('btnupdateButtonSC');}" />
                            </dx:ASPxButton>
                            <dx:ASPxButton ID="hdnUpdateButtonSC" runat="server" ClientVisible="false" AutoPostBack="false" ToolTip="Save changes" Text="Save & Close">
                                <Image Url="/Content/Images/saveFile_icon.png" Height="16px"></Image>
                                <ClientSideEvents Click="function(s,e){ActionContainer('btnupdateButtonSC');}" />
                            </dx:ASPxButton>
                        </li>


                        <li runat="server" id="cancelButtonLI" class="" onmouseover="this.className=''" onmouseout="this.className=''">
                            <dx:ASPxButton ID="cancelButton" runat="server" ImagePosition="Left" Visible="true" ToolTip="Exit" Text="Close" AutoPostBack="false">
                                <Image Url="/Content/Images/closeMenu_icon.png" Height="16px"></Image>

                                <ClientSideEvents Click="function(s,e){ActionContainer('btnupdateButtonSC');return ClosePopUp();}" />
                            </dx:ASPxButton>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div id="showSubTicketDetails">
    </div>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function actionOnTabChange(tabNumber) {
            var activeTab = tbcDetailTabs.GetTab(tabNumber);
            var tab = activeTab.name;
            $.cookie("currentActiveTab", tab);
            $(".tabcontent11").each(function (i, item) {
                $(item).hide();
            });
            var activeTabContainer = $(".tabcontent11[id='" + "tabPanelContainer_" + tab + "']");
            activeTabContainer.show();
            $("#<%=hdnActiveTab.ClientID%>").val(tab);

            if ("<%=preloadAllModuleTabs%>" == "True") {
                var timeOutTime = 100;

                setTimeout(function () {
                    var frames = $(activeTabContainer).find("iframe");
                    if (frames.length > 0) {
                        for (var i = 0; i < frames.length; i++) {
                            var frame = $(frames[i]).get(0);
                            try {
                                frame.contentWindow.clickUpdateSize();
                                setTimeout("adjustIframeControlSize('" + $(frames[i]).attr("id") + "')", timeOutTime);
                            } catch (ex) { }
                        }
                    }
                }, 0);

            }
            else {
                RefreshIframeIfDataNotExist(activeTabContainer.get(0), false);
            }
        }

        function adjustIframeControlSize(iframeID) {
            var frameJQ = $("#" + iframeID);
            if (frameJQ.length > 0) {
                var frame = frameJQ.get(0);
                try {
                    frame.contentWindow.adjustControlSize();
                } catch (ex) { } // Need try-catch in case adjustControlSize() not implemented for control
            }
        }

        function actionOnTabClick(clickTabIndex, activeTabIndex) {
            //debugger;
            //if (clickTabIndex == activeTabIndex) {
                var clickTab = tbcDetailTabs.GetTab(clickTabIndex).name;
                var activeTab = tbcDetailTabs.GetTab(activeTabIndex).name;
                var activeTabContainer = $(".tabcontent11[id='" + "tabPanelContainer_" + clickTab + "']");
                activeTabContainer.show();
                RefreshIframeIfDataNotExist(activeTabContainer.get(0), false);
            //}
        }

        function RefreshAllIframe() {
            if (typeof (tbcDetailTabs) === "undefined")
                return;

            for (var i = 0; i < tbcDetailTabs.tabs.length; i++) {
                var tab = tbcDetailTabs.GetTab(i).name;
                var activeTabContainer = $(".tabcontent11[id='" + "tabPanelContainer_" + tab + "']");
                RefreshIframeIfDataNotExist(activeTabContainer, false);
            }
        }

        function RefreshIframeIfDataNotExist(selectedContainer, ifNotLoaded) {
            //debugger;
            var currentDate = new Date();
            if (selectedContainer != null) {
                var frames = $(selectedContainer).find("iframe");

                if (frames.length > 0) {
                    for (var i = 0; i < frames.length; i++) {
                        var frame = $(frames[i]);

                        if (frame.attr("frameurl") != null) {
                            var url = frame.attr("frameurl");
                            var frameSrcUrl = frame.attr("src");
                            if (frameSrcUrl != undefined && frameSrcUrl != "" && ifNotLoaded) {
                                return;
                            }
                            if (url.indexOf("PMMSprints") > 0) {
                                //_aspxSetCookieInternalDelete("IsPaneExpanded", "", new Date(1970, 1, 1), _spPageContextInfo.webServerRelativeUrl);
                                _aspxSetCookieInternalDelete("IsPaneExpanded", "", new Date(1970, 1, 1), window.location.pathname);
                            }
                            frame.attr("src", url + "&timespan=" + currentDate.getTime());
                            var loadingElt = "<span class='ugit-trcnoti-base loading111' role='alert' style='top: 0px;position:absolute;background:yellow'><div class='ugit-trcnoti-bg'><div class='ugit-trcnoti-toast'>Loading...</div></div></span>";
                            frame.parent().append(loadingElt);
                            frame.parent().css("position", "relative");
                        }
                    }
                }
            }
        }

        function setUGITTabActive(selectedTab) {

            var sTab = parseInt(selectedTab);
            if (isNaN(sTab) || sTab <= 0)
                sTab = 0;


            var activeTabIndex = tbcDetailTabs.GetActiveTabIndex();
            var tab = tbcDetailTabs.GetTabByName(sTab);
            if (tab == null) {
                tab = tbcDetailTabs.GetTab(sTab)
            }

            tbcDetailTabs.SetActiveTab(tab);
            if (activeTabIndex == tab.index) {
                //var activeTabContainer = $(".tabcontent11[id='" + "tabPanelContainer_" + 1 + "']");
                //RefreshIframeIfDataNotExist(activeTabContainer, false);
                actionOnTabChange(tab.index);
            }
        }

        <%if (!printEnable && preloadAllModuleTabs)
        { %>

        window.onload = function () {
            try {

                RefreshAllIframe();
            }
            catch (ex) {
                alert(ex.message);
            }
        };

        // $(document).ready(function () {
        //    try {
        //        RefreshAllIframe();
        //    } catch (e) {
        //        alert(e.message);
        //    }
        //});

        <%}%>

        $(function () {
            // set the page title as module name.
            document.title = '<%=currentModuleName%>';
         <%if (printEnable)
        { %>
            //Show data for printout
            {
                //Set src of all iframe with param enableprint
                var currentDate = new Date();
                var iframes = $(".moduleitemdetail111 iframe")
                for (var i = 0; i < iframes.length; i++) {
                    $("body").data(iframes[i].id, { "isLoaded": false });
                    var frame = $(iframes[i]);
                    if (frame.attr("frameurl")) {
                        frame.bind("load", function () {

                            //get iframe html and put it into parent container
                            if ($(this) != null && $(this).contents() != null) {
                                $(this).parent().html($(this).contents().find(".managementcontrol-main").html());
                            }

                            //set this has been loaded
                            if ($("body").data(this.id) != undefined) {
                                $("body").data(this.id).isLoaded = true;
                            }

                            //if all iframe loaded then call print command
                            var allLoaded = true;
                            for (var j = 0; j < iframes.length; j++) {
                                if ($("body").data(iframes[j].id) != undefined && $("body").data(iframes[j].id).isLoaded == false) {
                                    allLoaded = false;
                                }
                            }

                            if (allLoaded) {
                                window.print();
                            }

                        });

                        frame.attr("frameurl", frame.attr("frameurl") + "&enableprint=true");
                        frame.attr("src", frame.attr("frameurl") + "&timespan=" + currentDate.getTime());
                    }
                }

                //$(".tabcontent11").prev().
                //$(".tabcontent11").css("page-break-before","always");
                //Remove unwanted elements
                $(".cioReportbuttonContainer111").remove();
                $(".utilityContainer111").remove();
                $("#holdButtonContainer").remove();
                $(".messageConatiner111").remove();
                $(".workflowGraphicContainer111 .active").removeClass("active");

                //Change look and feel of tabs and show all tab detail
                //var tabs = $(".moduleDetailTabsContainer111 li");
                //var tabsTxt =  $(".moduleDetailTabsContainer111 .menu-item-text");

                $("body").append('<br/>')
                $("body").append($(".workflowGraphicContainer111").html())

                var pagebreakup = $('#<%=hdnpagebreakup.ClientID%>').val();
                for (var j = 0; j < tbcDetailTabs.tabs.length; j++) {
                    var headercss = "tabcontent111_header";
                    var sectionHtml = '';
                    if (j == 0) {
                        headercss = "";
                        sectionHtml = "<div class='" + headercss + "' style='border-bottom:1px solid;float:left;width:100%;'><span style='float:left;padding:4px;font-weight:bold;font-size:13px;'>" + tbcDetailTabs.tabs[j].GetText() + "</span><span style='float:right;padding:4px;font-weight:bold;font-size:13px;'>" +'<%=currentTicketPublicID%>' + "</span></div>";
                    }
                    <%--else {
                        if (pagebreakup == 'True')
                            sectionHtml = "<div class='" + headercss + "' style='border-bottom:1px solid;float:left;width:100%;'><span style='float:left;padding:4px;font-weight:bold;font-size:13px;'>" + tbcDetailTabs.tabs[j].GetText() + "</span><span style='float:right;padding:4px;font-weight:bold;font-size:13px;'>" +'<%=currentTicketPublicID%>' + "</span></div>";
                        else
                            sectionHtml = "<div class='" + headercss + "' style='border-bottom:1px solid;float:left;width:100%;'><span style='float:left;padding:4px;font-weight:bold;font-size:13px;'>" + tbcDetailTabs.tabs[j].GetText() + "</span></div>";
                    }--%>


                    $("#tabPanelContainer_" + (j + 1)).before(sectionHtml);
                    $("#tabPanelContainer_" + (j + 1)).css("display", "block");

                    $("body").append($("#tabPanelContainer_" + (j + 1)).html());
                }


                $(".moduleDetailTabsContainer111").remove();

                //Change important some properties
                $("body").css("overflow", "Visible");

                $("aspnetForm").remove()

                //Remove attachment button
                var attachButton = $("input[type='file']").next()
                if (attachButton.length > 0 && attachButton.attr("type").toLowerCase() == "submit") {
                    attachButton.remove();
                }
                $("input[type='file']").remove();
                $(".uploadfileinputcontainer").remove();
                $(".defaultattachmentcontainer").remove();
                $(".idAttachmentsTable a[class *='deleteFile']").remove();

                //Remove all hidden elements according to jquery rule
                $("div:hidden").remove();
                $("#s4-titlerow").remove();

                //Change title of page
                var title = "<%=currentTicketId %>";
            <%if (Request["pageTitle"] != null && Request["pageTitle"] != string.Empty)
        {  %>
                title ="<%=Request["pageTitle"] %>";
            <%} %>
                $(".ms-WPTitle nobr").text(title);
                document.title = title;

                //if there is no iframe in page then call print command right away
                if ($("body").data() == null) {
                    window.print();
                }
            }
            <%}
        else
        {%>
            try {
                tbcDetailTabs.SetWidth($('.moduleDetailTabsSubContainer111').width())
                setUGITTabActive("<%=hdnActiveTab.Value%>");
            }
            catch (ex) {
            }
            <%}%>

            <% if (currentModuleName == "CMT")
        {%>
            AutoCalculateReminderDate();
                <%}%>

        });

        function AutoCalculateReminderDate() {

            try {
                var currentdate = new Date();
                currentdate = new Date(currentdate.format("yyyy-MM-dd"));
                $('.ReminderDays').bind("change", function () {
                    var value = $('.ReminderDays').val();
                    if (value != "" && $.isNumeric(value)) {
                        var datevaluetemp = $('.clsExpirationDate').val();

                        if (datevaluetemp != "") {
                            var arr = datevaluetemp.split('/');
                            arr[0] = arr[0].length == 2 ? arr[0] : "0" + arr[0];
                            arr[1] = arr[1].length == 2 ? arr[1] : "0" + arr[1];

                            datevaluetemp = arr[2] + "-" + arr[0] + "-" + arr[1];
                        }
                        var datevalue = new Date(datevaluetemp);
                        //new Date(($('.ContractExpirationDate').find('input').val()));

                        if (datevalue != "" && Date.parse(datevalue)) {
                            datevalue.setDate(datevalue.getDate() - value);

                            if (datevalue >= currentdate) {
                                var reminderdate = $.datepicker.formatDate("mm/dd/yy", datevalue);
                                $('.ReminderDate').find('input').val(reminderdate);
                                var reminderdatetodisplay = $.datepicker.formatDate("M-d-yy", datevalue);
                                $('.clsReminderDate').parents("td.tableCell:eq(0)").find(".labelvalue").html(reminderdatetodisplay);
                            }
                            else {
                                $('.ReminderDate').find('input').val("");
                                $('.clsReminderDate').parents("td.tableCell:eq(0)").find(".labelvalue").html("");
                            }

                        }
                    }
                });

                $('.clsReminderDate').focusout(function () {

                    //var contractExpirationdate=new Date(($('.ContractExpirationDate').find('input').val()));
                    var datevaluetemp = $('.clsExpirationDate').val();

                    if (datevaluetemp != "") {
                        var arr = datevaluetemp.split('/');
                        arr[0] = arr[0].length == 2 ? arr[0] : "0" + arr[0];
                        arr[1] = arr[1].length == 2 ? arr[1] : "0" + arr[1];

                        datevaluetemp = arr[2] + "-" + arr[0] + "-" + arr[1];
                    }
                    var contractExpirationdate = new Date(datevaluetemp);
                    //var reminderdate=new Date($('.ReminderDate').find('input').val());
                    var reminderdatetemp = $('.clsReminderDate').val();
                    if (reminderdatetemp != "") {
                        var arr = reminderdatetemp.split('/');
                        arr[0] = arr[0].length == 2 ? arr[0] : "0" + arr[0];
                        arr[1] = arr[1].length == 2 ? arr[1] : "0" + arr[1];

                        reminderdatetemp = arr[2] + "-" + arr[0] + "-" + arr[1];
                    }
                    var reminderdate = new Date(reminderdatetemp);
                    if (contractExpirationdate != "" && Date.parse(contractExpirationdate) && reminderdate >= currentdate && reminderdate <= contractExpirationdate) {
                        var days = 0;
                        if (contractExpirationdate > reminderdate) {
                            var millisecondsPerDay = 1000 * 60 * 60 * 24;
                            var millisBetween = contractExpirationdate.getTime() - reminderdate.getTime();
                            days = millisBetween / millisecondsPerDay;
                            var daysdifference = Math.floor(days);
                            $('.ReminderDays').val(daysdifference);

                            $('.ReminderDays').parents("td.tableCell:eq(0)").find(".labelvalue").html(daysdifference);
                        }
                        else {
                            $('.ReminderDays').val(days);

                            $('.ReminderDays').parents("td.tableCell:eq(0)").find(".labelvalue").html(days);
                        }
                    }
                    else {
                        $('.clsReminderDate').val("");
                        $('.clsReminderDate').parents("td.tableCell:eq(0)").find(".labelvalue").html("");
                    }
                });

                $('.clsExpirationDate').focusout(function () {
                    var reminderdays = $('.ReminderDays').val();
                    var reminderdate = new Date($('.ReminderDate').find('input').val());
                    if (reminderdays != "" && $.isNumeric(reminderdays)) {
                        var datevaluetemp = $('.clsExpirationDate').val();

                        if (datevaluetemp != "") {
                            var arr = datevaluetemp.split('/');
                            arr[0] = arr[0].length == 2 ? arr[0] : "0" + arr[0];
                            arr[1] = arr[1].length == 2 ? arr[1] : "0" + arr[1];

                            datevaluetemp = arr[2] + "-" + arr[0] + "-" + arr[1];
                        }


                        var datevalue = new Date(datevaluetemp);

                        if (datevalue != "" && Date.parse(datevalue)) {

                            datevalue.setDate(datevalue.getDate() - reminderdays);
                            if (datevalue >= currentdate) {
                                var reminderdate = $.datepicker.formatDate("mm/dd/yy", datevalue);
                                $('.clsReminderDate').val(reminderdate);
                                var reminderdatetodisplay = $.datepicker.formatDate("M-d-yy", datevalue);
                                $('.clsReminderDate').parents("td.tableCell:eq(0)").find(".labelvalue").html(reminderdatetodisplay);
                            }
                            else {
                                $('.clsReminderDate').val("");
                                $('.clsReminderDate').parents("td.tableCell:eq(0)").find(".labelvalue").html("");
                            }
                        }
                        else {
                            $('.clsReminderDate').val("");
                            $('.clsReminderDate').parents("td.tableCell:eq(0)").find(".labelvalue").html("");
                        }
                    }
                });
            }
            catch (ex) { }
        }


        function ShowInformation(obj) {
            $("#attachments").css('display', 'none');
            $("#information .informationBox").html($(obj).attr("title"));
            $("#information").css({ 'top': ($(obj).position().top + 15) + 'px', 'display': 'block', 'left': ($(obj).position().left + 4) + 'px' });
            return false;
        }

        function ShowAttachments(obj) {

            var requestTypeID = $(obj).attr("requesttypeid");
            var attachmentBox = $("#attachments .attachmentBox");

            attachmentBox.html("Loading...");
            var attachmentHtml = new Array();
            var qData = '{' + '"list":"RequestType","id":"' + requestTypeID + '"}';;
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelper %>/GetAttachments",
                data: qData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var resultJson = $.parseJSON(message.d);
                    var attachments = resultJson.attachments.split(";#");
                    var j = 1;
                    $.each(attachments, function (i, item) {
                        var fileInfo = item.split("###");
                        if (fileInfo.length > 1) {
                            attachmentHtml.push("<b style='cursor:pointer;float:left;width:100%;' onclick='window.open(\"" + fileInfo[0] + fileInfo[1] + "\");'>" + j + ". " + fileInfo[1] + "</b>");
                            j = j + 1;
                        }
                    });
                    attachmentBox.html(attachmentHtml.join(""));
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    attachmentBox.html("<b>Error</b>");
                }

            });

            if ($("#attachments").css('display') == "none") {
                $("#information").css('display', 'none');
                $("#attachments").css({ 'top': ($(obj).position().top + 15) + 'px', 'display': 'block', 'left': ($(obj).position().left + 4) + 'px' });
            }
            else {
                $("#attachments").css({ 'display': 'none' });
            }
            return false;
        }

        function HideShowInformation() {
            document.getElementById("information").style.display = "none";
        }
        function HideAttachmetns() {
            document.getElementById("attachments").style.display = "none";
        }
    </script>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">



        function mailToUser(obj) {
            var users = $(obj).attr("UsersID");
            if (users) {
                var usersID = users.split(",");
                var usrInfo = getSelectedUsers(usersID);
                if (usrInfo.length > 0) {
                    var emails = new Array();
                    var names = new Array();
                    for (var i = 0; i < usrInfo.length; i++) {
                        if (usrInfo[i].EMail)
                            emails.push(usrInfo[i].EMail);

                        if (usrInfo[i].Title)
                            names.push(usrInfo[i].Title);
                    }
                    var mtTemplate = mailtoTemplate.replace("(email)", emails.join(";")).replace("(name)", names.join(", "));
                    window.location.href = mtTemplate;
                }
            }
        }

        var openUserDetailClicked = false;
        function ticketEmail(obj) {
            openUserDetailClicked = false;
            timer = setTimeout(function () {
                if (!openUserDetailClicked) {
                    var users = $(obj).attr("UsersID");
                    window.parent.UgitOpenPopupDialog("<%=TicketEmailURL %>&ModuleName=" + currentModuleName + "&currentTicketPublicID=" + currentTicketPublicID + "&users=" + users, "", "Send Email", 60, 90);
                }
                openUserDetailClicked = false;

            }, 250);
        }


        function OpenUserDetails(obj) {

            clearTimeout(timer);
            openUserDetailClicked = true;
            var users = $(obj).attr("UsersID");
            window.parent.UgitOpenPopupDialog("<%=userDetailURL %>uID=" + users, "", "User Detail", "600px", "90");
        }

        function getField(fieldType, fieldTitle) {
            var docTags = document.getElementsByTagName(fieldType);
            for (var i = 0; i < docTags.length; i++) {
                if (docTags[i].title == fieldTitle) {
                    return docTags[i];
                }
            }
            return false;
        }

        // Handled server-side now
        //function ValidateFileName() {
        //    if($(".inputFile")){
        //        $.each($(".inputFile"),function()
        //        {
        //            if(this && this.value != null && this.value.match("[~#%&*{}\<>?/+|\"]") != null){
        //                alert("File Name cannot contain '[~#%&*{}\<>?/+|\"]'");
        //                return false;
        //            }
        //        })
        //    }
        //    return true;
        //}

        // In case of DRQ module select the RelatedRequestID on selection of RequestTYpe module.
        function getModulesOpenTicket(selectedId) {
            var $relatedRequestIdDropDown = $("select[class*='RelatedRequestId']").get(0);
            if ($relatedRequestIdDropDown) {
                var selectedId = selectedId.replace("— ", "");
                var $options = $('option', $relatedRequestIdDropDown);
                var qData = '{' + '"moduleName":"' + selectedId + '"}';

                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelper %>/GetModulesOpenTicket",
                    data: qData,
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (message) {
                        message = message.slice(6, message.length - 2);
                        var data = message.split('####');
                        var selectedval = $("select[class*='RelatedRequestId']").get(0).value;
                        $options.each(function () {
                            $(this).remove();
                        });
                        if (data.length > 1) {
                            for (var x = 0; x < data.length - 1; x++) {
                                var item = data[x].split(";#");
                                var opt = "<option value='" + item[0] + "'  title='" + item[1] + "'>" + item[0] + "</option>";
                                $("select[class*='RelatedRequestId']").append(opt);
                            }
                            if (selectedval != "") {
                                $(".RelatedRequestId option[value='" + selectedval + "']").attr("selected", true);
                            }
                            var selectedValue = $(".RelatedRequestId").attr("selectedvalue");
                            if (selectedValue != "") {
                                $(".RelatedRequestId option[value='" + selectedValue + "']").attr("selected", true);
                            }
                        }
                        else {
                            var opt = "<option value='N/A'>N/A</option>";
                            $("select:[class*='RelatedRequestId']").append(opt);
                        }

                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }


                });
            }
        }



        function SetCategoryByRequestTypeId() {

            var rtJosn = moduleSetting.RequestTypeDetail;
            if (rtJosn) {
                checkCategory(rtJosn.Workflowtype, rtJosn.Category);

                <%if (lblCategoryTypeNew != null)
        { %>
                $('#<%=lblCategoryTypeNew.ClientID %>').html(rtJosn.Category);
                    <%} %>

                <%if (lblCategoryType != null)
        { %>
                $('#<%=lblCategoryType.ClientID %>').html(rtJosn.Category);
                <%} %>
                return;
            }
            else {
                // Did not find request type, so set to default
                checkCategory();
            }
        }

        var requestOwnerCollection = null;
        function getRequestTypeOwner(selectedId) {
            if (selectedId != null || selectedId != "" || selectedId != undefined) {
                var requestTypesDetaill = filterRequestTypeByID(selectedId);<%--//<%=LoadRequestTypes()%>--%>
                var OwnerCtr = ASPxClientControl.GetControlCollection().GetByName($(".field_owner_edit").attr("id"));
                if (OwnerCtr) {
                    OwnerCtr.SetValue(requestTypesDetaill.OwnerID);
                }
                $(".Owner").html(requestTypesDetaill.owners);
            }

        }

        function onQuerySucceeded(sender, args) {
            if (requestOwnerCollection != null) {
                var listItemEnumerator = requestOwnerCollection.getEnumerator();
                while (listItemEnumerator.moveNext()) {
                    var oListItem = listItemEnumerator.get_current();
                    $("#<%= incidentOwnerId%>_ctl00_UserField_upLevelDiv").html(oListItem.get_item("RequestTypeOwner").get_lookupValue());
                }
            }
        }

        function onQueryFailed(sender, args) {
        }

        var docTag1 = document.getElementsByTagName('select');
        for (var i = 0; i < docTag1.length; i++) {
            if (docTag1[i].title == 'Impact' || docTag1[i].title == 'Severity') {
                docTag1[i].onchange = function () { changePriority() };
            }
            ignoreChanges = 1;
        }



        function enableDisableResolution(initiatorResolved) {
            //if ($(".InitiatorResolved").length == 0)
            //    return;
            if (initiatorResolved == null || initiatorResolved == undefined && initiatorResolved == '')
                initiatorResolved = ASPxClientControl.GetControlCollection().GetByName(($(".InitiatorResolvedChoice").attr("id")) + "_ListBox");
            if (initiatorResolved) {
                var yesNo = initiatorResolved.GetValue();
                if (yesNo == 'Yes') {
                    if ($(".ResolutionCommentsRow")) { $(".ResolutionCommentsRow").css({ "visibility": "visible", "position": "" }); }
                    if ($(".createButtonContainer")) { $(".createButtonContainer").css({ "display": "none" }); }
                    if ($(".createCloseButtonContainer")) { $(".createCloseButtonContainer").css({ "display": "" }); }
                }
                else {
                    if ($(".ResolutionCommentsRow")) { $(".ResolutionCommentsRow").css({ "visibility": "hidden", "position": "absolute" }); }
                    if ($(".createButtonContainer")) { $(".createButtonContainer").css({ "display": "" }); }
                    if ($(".createCloseButtonContainer")) { $(".createCloseButtonContainer").css({ "display": "none" }); }
                    //if($(".CreateButtonLI")){ $(".CreateButtonLI").css({"display":""}); }
                    //if($(".CreateCloseButtonLI")){ $(".CreateCloseButtonLI").css({"display":"none"}); }
                }
            }
        }

        function enableDisableReviewers() {
            if ($(".NeedReview").length > 0) {
                var needReview = ASPxClientControl.GetControlCollection().GetByName($(".NeedReview").get(0).id);
                if (needReview) {
                    var yesNo = needReview.GetValue();
                    if (yesNo == 'Yes') {
                        if ($(".LegalRow")) { $(".LegalRow").css({ "visibility": "visible", "position": "" }); }
                        if ($(".FinanceManagerRow")) { $(".FinanceManagerRow").css({ "visibility": "visible", "position": "" }); }
                        if ($(".PurchasingRow")) { $(".PurchasingRow").css({ "visibility": "visible", "position": "" }); }
                    }
                    else {
                        if ($(".LegalRow")) { $(".LegalRow").css({ "visibility": "hidden", "position": "absolute" }); }
                        if ($(".FinanceManagerRow")) { $(".FinanceManagerRow").css({ "visibility": "hidden", "position": "absolute" }); }
                        if ($(".PurchasingRow")) { $(".PurchasingRow").css({ "visibility": "hidden", "position": "absolute" }); }
                    }
                }
            }
        }
        // Populate Studio based on Division/DivisionIdLookup
        function populateStudio() {            
            var dxDivisionLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_divisionlookup_edit").attr("id"));
            var dxStudioLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_studiolookup_edit").attr("id"));
            var division = "";
            if (dxDivisionLookup) {
                division = dxDivisionLookup.GetValueString();
                $.cookie("ticketDivision", division, { path: "/" });

                // obj = ['DummyValue', 'Tells if Param#3 is Div or Dept', 'Param#3. Used to filter studios']
                //var obj = ['Filter', 'DivisionIdLookup', division];
                //if (dxStudioLookup) {
                //    dxStudioLookup.GetGridView().PerformCallback(obj);
                //}
            }
        }

        function callbackStudio() {
            var dxDivisionLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_divisionlookup_edit").attr("id"));
            var dxStudioLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_studiolookup_edit").attr("id"));
            var division = "";
            if (dxDivisionLookup) {
                division = dxDivisionLookup.GetValueString();
                $.cookie("ticketDivision", division, { path: "/" });

                obj = ['DummyValue', 'Tells if Param#3 is Div or Dept', 'Param#3. Used to filter studios']
                var obj = ['Filter', 'DivisionIdLookup', division];
                if (dxStudioLookup) {
                    dxStudioLookup.GetGridView().PerformCallback(obj);
                }
            }
        }

        //Create Priority severity mapping
        function changePriority() {
            if (moduleSetting.PriorityMap && moduleSetting.PriorityMap.length == 0)
                return;
            var priorityMap = moduleSetting.PriorityMap;

            var dxSeverityLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_severitylookup_edit").attr("id"));
            var dxImpactLookup = ASPxClientControl.GetControlCollection().GetByName($(".field_impactlookup_edit").attr("id"));
            var impactID = severityID = "";
            if (dxSeverityLookup)
                severityID = dxSeverityLookup.GetValueString();
            if (dxImpactLookup)
                impactID = dxImpactLookup.GetValueString();


            if (impactID && severityID && priorityMap.length > 0) {
                var mapObj = _.find(priorityMap, function (v) { return v.ImpactLookup == impactID && v.SeverityLookup == severityID; });

                if (mapObj) {
                    if (($(".chkelevatecheck").find("input").is(":checked"))) {
                        $(".field_prioritylookup_view").html("<%= elevatedPrioirty %>");
                    }
                    else if (mapObj.ModulePrioirty && !mapObj.ModulePrioirty.IsDeleted) {
                        $(".field_prioritylookup_view").html(mapObj.ModulePrioirty.Title);
                    }
                    else {
                        $(".field_prioritylookup_view").html("No Priority");
                    }
                }
                else {
                    $(".field_prioritylookup_view").html("No Priority");
                }
            }
        }
        function showHidden(id) {
            document.getElementById(id).style.display = "block";
        }

        function ticketIdChanged(obj) {
            document.getElementById('<%=lblProjectDescription.ClientID %>').innerHTML = obj.value;
        }

        function moduleChanged() {
            var projectIds = document.getElementById('<%=ddlTicketProjectReference.ClientID %>');
            var moduleSelected = document.getElementById('<%=ddlTicketProjectRelated.ClientID %>').options[document.getElementById('<%=ddlTicketProjectRelated.ClientID %>').selectedIndex].value;
            projectIds.innerHTML = "";
            projectIds.length = 0;
            projectIds.add(new Option("", ""));
            if (projectsByModule) {
                for (var i = 0; i < projectsByModule.length; i++) {
                    if (projectsByModule[i].indexOf(moduleSelected + '####') != -1) {
                        projectIds.add(new Option(projectsByModule[i].split('####')[1].split('##')[0], projectsByModule[i].split('####')[1].split('##')[1]));
                    }
                }
            }
        }

        function checkCategory(workflowType, category) {
            if (workflowType == 'Quick') {
                if ($(".ResolutionCommentsRow")) { $(".ResolutionCommentsRow").css({ "visibility": "visible", "position": "" }); }
                if ($(".CreateButtonLI")) { $(".CreateButtonLI").css({ "display": "none" }); }
                if ($(".CreateCloseButtonLI")) { $(".CreateCloseButtonLI").css({ "display": "" }); }
                if ($(".GLCodeRow")) { $(".GLCodeRow").css({ "visibility": "hidden", "position": "absolute" }); }
            }
            else if (workflowType == 'Requisition') {
                if ($(".ResolutionCommentsRow")) { $(".ResolutionCommentsRow").css({ "visibility": "hidden", "position": "absolute" }); }
                if ($(".CreateButtonLI")) { $(".CreateButtonLI").css({ "display": "" }); }
                if ($(".CreateCloseButtonLI")) { $(".CreateCloseButtonLI").css({ "display": "none" }); }
                if ($(".GLCodeRow")) { $(".GLCodeRow").css({ "visibility": "", "position": "" }); }
            }
            else if (workflowType == 'NoTest') {
                if ($(".TesterRow")) { $(".TesterRow").css({ "visibility": "hidden", "position": "absolute" }); }
                if ($(".ResolutionCommentsRow")) { $(".ResolutionCommentsRow").css({ "visibility": "hidden", "position": "absolute" }); }
                if ($(".CreateButtonLI")) { $(".CreateButtonLI").css({ "display": "" }); }
                if ($(".CreateCloseButtonLI")) { $(".CreateCloseButtonLI").css({ "display": "none" }); }
                if ($(".GLCodeRow")) { $(".GLCodeRow").css({ "visibility": "hidden", "position": "absolute" }); }
            }
            else {
                if ($(".ResolutionCommentsRow")) { $(".ResolutionCommentsRow").css({ "visibility": "hidden", "position": "absolute" }); }
                if ($(".CreateButtonLI")) { $(".CreateButtonLI").css({ "display": "" }); }
                if ($(".CreateCloseButtonLI")) { $(".CreateCloseButtonLI").css({ "display": "none" }); }
                // if($(".GLCodeRow")){ $(".GLCodeRow").css({"visibility":"hidden","position":"absolute"});}
            }

            var initiatorResolved = $(".InitiatorResolvedChoice").get(0);
            if (initiatorResolved) {
                enableDisableResolution();
            }

            if ($(".RequestTypeCategory").get(0)) { $(".RequestTypeCategory").get(0).innerHTML = category; }
        }


    </script>

    <script type="text/ecmascript" language="ecmascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        var notifyId = '';
        var statusId = '';

        function openTicket(id, ticketId) {
            var winWidth = 0, winHeight = 0;
            if (parseInt(navigator.appVersion) > 3) {
                if (navigator.appName == "Netscape") {
                    winWidth = window.innerWidth;
                    winHeight = window.innerHeight;
                }
                if (navigator.appName.indexOf("Microsoft") != -1) {
                    winWidth = document.body.offsetWidth;
                    winHeight = document.body.offsetHeight;
                }
            }
            var options = {
                url: '<%=currentModulePagePath %>?TicketId=' + id,
                width: winWidth,
                height: winHeight,
                title: "<%=currentModuleName %> Ticket: " + ticketId,
                allowMaximize: false,
                showClose: true,
                dialogReturnValueCallback: SP.UI.ModalDialog.RefreshPage
            };
            //SP.UI.ModalDialog.showModalDialog(options);
            //SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options)
        }
        function CloseWithoutSaving() {
            delete_cookie('ticketDivision');
            var sourceURL = "<%= Request["source"] %>";
            window.parent.CloseWindowCallback(sourceURL);
        }
        function ClosePopUp() {
            $.cookie("ticketDivision", null, { path: "/" });
            delete_cookie('ticketDivision');
            var changed = parseInt($.cookie("dataChanged"));
            var unsavedData = dataChanged || changed;
            if (unsavedData && currentTicketPublicID != "") {
                closePopUp.Show();
            }
            else {
                //if ($.cookie("_refreshPraent") == '1') {
                var sourceURL = "<%= Request["source"] %>";
                sourceURL += "**refreshDataOnly";
                //window.parent.location.reload(false);
                window.parent.CloseWindowCallback(1, sourceURL);
            <%--}
                else {
                    var sourceURL = "<%= Request["source"] %>";
                    window.parent.CloseWindowCallback(sourceURL);
                }--%>
            }

            return false;
        }
        function CreateAsset() {
            window.parent.UgitOpenPopupDialog('<%=currentCreateTSR %>?TSRTicketId=<%=TicketNoLiteral.Text %>', "", "Create an Asset from <%=TicketNoLiteral.Text %>", 90, 70);
        }

        function GetSubTickets() {
            var nprId = document.getElementById('<%=nprId %>');
            var selectedValue = nprId.options[nprId.selectedIndex].value;
            var url = "http://localhost/_vti_bin/listdata.svc/NPRTicket?$filter=Id eq " + selectedValue;
            var request = new Sys.Net.WebRequest();
            request.set_httpVerb("get");
            request.set_url(url);
            request.get_headers()["Accept"] = "application/json";

            request.add_completed(handleRequestComplete);
            request.invoke();
        }
        function handleRequestComplete(response, userContext) {
            var ddddd = response.get_object().d.results;
            if (ddddd.length) {
                var subTicketDiv = document.getElementById("showSubTicketDetails");
                var table = document.createElement("Table");
                table.style.border = "1px solid black";
                var tr = document.createElement("tr");

                var td1 = document.createElement("td");
                td1.innerHTML = ddddd[0].Title;
                var td2 = document.createElement("td");
                td2.innerHTML = ddddd[0].Description;
                var td3 = document.createElement("td");
                td3.innerHTML = ddddd[0].TicketProjectScope;
                var td4 = document.createElement("td");
                td3.innerHTML = ddddd[0].TicketProjectManager;
                var td5 = document.createElement("td");
                td3.innerHTML = ddddd[0].TicketStakeHolders;
                var td6 = document.createElement("td");
                td3.innerHTML = ddddd[0].TicketProjectDirector;

                tr.appendChild(td1);
                tr.appendChild(td2);
                tr.appendChild(td3);
                tr.appendChild(td4);
                tr.appendChild(td5);
                tr.appendChild(td6);
                table.appendChild(tr);

                subTicketDiv.appendChild(table);
                subTicketDiv.style.position = "absolute";
                subTicketDiv.style.top = "300px";
                subTicketDiv.style.left = "200px";
                subTicketDiv.style.display = "block";
            }
        }
    </script>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        var isEditOrRequesterChange = false;
        var isduplicate = false;

        var isduplicateValue ='<%=IsDuplicate%>';
        if (isduplicateValue.toLowerCase() == 'true')
            isduplicate = true;


        if (<%=currentTicketId%> > 0) {
            isEditOrRequesterChange = false;
        }
        else {
            isEditOrRequesterChange = true;
        }
        var requestorRelatedData = {};
        function UgitOpenPopupDialogLocal(path, params, titleVal, width, height, returnVaul) {

            var winWidth = 0, winHeight = 0;
            if (parseInt(navigator.appVersion) > 3) {
                if (navigator.appName == "Netscape") {
                    winWidth = window.innerWidth;
                    winHeight = window.innerHeight;
                }
                if (navigator.appName.indexOf("Microsoft") != -1) {
                    winWidth = document.body.offsetWidth;
                    winHeight = document.body.offsetHeight;
                }
            }

            var fWidth = winWidth;
            var fHeight = winHeight;


            if (width != "auto") {
                fWidth = (fWidth * width) / 100
                //fWidth =  fWidth - width;
            }
            if (height != "auto") {
                //fHeight = fHeight - height;
                fHeight = (fHeight * height) / 100
            }
            var startpram = "?";
            if (path.split("?").length > 1) {
                startpram = "&";
            }
            var options = {
                url: path + startpram + params + "&Width=" + fWidth + "&Height=" + fHeight,
                width: fWidth,
                height: fHeight,
                title: titleVal,
                allowMaximize: false,
                showClose: true,
                dialogReturnValueCallback: doNothing
            };

            //SP.UI.ModalDialog.showModalDialog(options);
            SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options)
        }

        function doNothing() {
        }

        function getValue_RequestType() {
            var dxRequestType = ASPxClientControl.GetControlCollection().GetByName($(".field_requesttypelookup_edit").attr("id"));
            var requestTypeID = 0;
            if (dxRequestType)
                requestTypeID = dxRequestType.GetKeyValue();

            return requestTypeID;
        }

        function getValue_Location() {
            var dxlocation = ASPxClientControl.GetControlCollection().GetByName($(".field_locationlookup_edit").attr("id"));
            var locationID = 0;
            if (requestorRelatedData != null && requestorRelatedData.length > 0) {
                locationID = requestorRelatedData.userlocationid;
            }
            if (dxlocation) {
                locationID = dxlocation.GetValue();
            }
            if (!locationID) {
                locationID = '<%=userLocation%>';
            }
            return locationID;
        }

        function setValue_Owner(val, text) {
            $(".OwnerUser").html(text);
            var OwnerCtr = ASPxClientControl.GetControlCollection().GetByName($(".field_owneruser_view ").attr("id"));
            if (OwnerCtr)
                OwnerCtr.SetValue(val);
        }

        function setValue_PRPGroup(val, text) {
            var ticketPRPObj = $(".PRPGroupUser");
            ticketPRPObj.html(text);
        }

        function setValue_ORP(val, text) {

        }

        function setValue_FunctionalArea(val, text) {

        }

        function setValue_EstimatedHours(val, text) {

        }


        function GetResolutionType(requestTypeID) {
            var resolutionTypeObj = ASPxClientControl.GetControlCollection().GetByName($(".field_resolutiontypechoice_edit").attr("id"));
            if (resolutionTypeObj) {
                var obj = [requestTypeID, 'ResolutionTypeChoice','<%=resolutionType%>'];
                resolutionTypeObj.GetGridView().PerformCallback(obj);
            }
        }

        function request_OnInit() {
            var requestTypeObj = ASPxClientControl.GetControlCollection().GetByName($(".field_requesttypelookup_edit").attr("id"));
            if (requestTypeObj != null && requestTypeObj != undefined) {

                GetIssueType(requestTypeObj.GetKeyValue());
                GetResolutionType(requestTypeObj.GetKeyValue());
            }
        }

        function GetIssueType(requestTypeID) {
            var issueTypeObj = ASPxClientControl.GetControlCollection().GetByName($(".field_category_edit").attr("id"));
            if (issueTypeObj) {
                var obj = [requestTypeID, 'Category','<%=issueType%>'];
                issueTypeObj.GetGridView().PerformCallback(obj);
            }
        }


        var businessManagerDependentField;
        function PreLoadDefaultSelections() {
            request_OnInit();
            var devRequestor = ASPxClientControl.GetControlCollection().GetByName($(".field_requesttypelookup_edit").attr("id"));
            if (devRequestor) {
                var selectedId = devRequestor.GetKeyValue();
                if (selectedId)
                    SetCategoryByRequestTypeId(selectedId);
                else
                    checkCategory();
            }
            else
                checkCategory();
            var hiddenFiles = $('#idAttachmentsTable');
            if (hiddenFiles.length > 0) {
                $(".defaultattachmentcontainer").css("display", "none");
                $(hiddenFiles.get(0)).css("displaychangeRequestOwner", "none");
            }

            if (moduleInitialDetail.TicketID == "") {
                changePriority();
            }



            //Setup auto-update of location (and business manager) when requestor modified
            if ($(".TicketRequestor").length > 0) {
                $("#" + $(".TicketRequestor")[0].id + "_upLevelDiv").bind("blur", FillOnRequestorChange);
                $("span.TicketRequestor").attr('eeaftercallbackclientscript', 'FillOnRequestorChange');
                //FillOnRequestorChange();
            }
            //ShowAndHideTimeLineimg();
            if ($(".Owner").length > 0) {
                $("#" + $(".Owner")[0].id + "_upLevelDiv").bind("blur", FillOnRequestorChange);
                $("span.Owner").attr('eeaftercallbackclientscript', 'FillOnRequestorChange');

            }
            $(".locationctr").bind("change", function () {
                $(".TicketRequestTypeLookup").trigger("change");

            });

            // If form has Business Manager field AND we are NOT filling it based on Requestor
            // setup auto update of business manager based on dependent field
            if ($(".TicketBusinessManager").length > 0) {
                // if($(".TicketBusinessManager").attr("class") != undefined && $(".TicketBusinessManager").attr("class").split("TicketBusinessManager ")[1] != undefined)
                if ($(".TicketBusinessManager").hasClass("TicketBusinessManager")) {
                    var allClasses = $(".TicketBusinessManager").attr("class").split(" ");
                    var dependentClasses = $.grep(allClasses, function (v, i) {
                        return v.indexOf('dependentField') != -1;
                    })

                    var dependentClass = "";
                    if (dependentClasses.length > 0) {
                        dependentClass = dependentClasses[0];
                    }
                    businessManagerDependentField = $.trim(dependentClass.replace("dependentField", ""));
                }
            }
        }

        var oldrequester = "";
        var newrequester = "";
        function FillOnRequestorChange() {
            var requestorCtr = null;
            var peoplePicker = null;
            var peoplepickerValue = null;
            var dxRequestor = ASPxClientControl.GetControlCollection().GetByName($(".field_requestoruser_edit").attr("id"));
            if (dxRequestor)
            //if($("span.Requestor").length>0)
            {
                if (dxRequestor.GetValue() != null && dxRequestor.GetValue() != undefined) {
                    peoplepickerValue = dxRequestor.GetValue();
                }
                else {
                    peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");

                }
                //requestorCtr = $("#" + $("span.Requestor").get(0).id + "_upLevelDiv");
                //if(requestorCtr.length >0)
                //{
                //    // Get new requestor value

                //    peoplePicker = requestorCtr.get(0);
                //    var pickerHtml = peoplePicker.innerHTML.toLowerCase();  
                //    if (pickerHtml.indexOf("<span") > -1) {
                //        var userSpans = $(peoplePicker).children("span");
                //        userSpans.each(function (i, item) {
                //            peoplepickerValue = $(item).attr("id");
                //            peoplepickerValue = peoplepickerValue.substring(4);
                //        });
                //    }
                //    else {
                //        peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
                //    }
                //}
            }

            if ($("span.Owner").length > 0) {
                requestorCtr = $("#" + $("span.Owner").get(0).id + "_upLevelDiv");
                if (requestorCtr.length > 0) {
                    // Get new requestor value

                    peoplePicker = requestorCtr.get(0);
                    var pickerHtml = peoplePicker.innerHTML.toLowerCase();
                    if (pickerHtml.indexOf("<span") > -1) {
                        var userSpans = $(peoplePicker).children("span");
                        userSpans.each(function (i, item) {
                            peoplepickerValue = $(item).attr("id");
                            peoplepickerValue = peoplepickerValue.substring(4);
                        });
                    }
                    else {
                        peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
                    }
                }
            }


            if (typeof (cbAssets) != 'undefined' && !cbAssets.InCallback()) {
                //if(peoplepickerValue!="")
                var userVal = "";
                try {
                    userVal = cbAssets.cpDependentFieldValue;
                } catch (ex) { }

                if (peoplepickerValue != null && peoplepickerValue != "") {
                    userVal = escape(peoplepickerValue);
                }

                cbAssets.PerformCallback(userVal);
            }

            if (peoplepickerValue != null && peoplepickerValue != "") {
                // Get requestor location
                peoplepickerValue = escape(peoplepickerValue);
                newrequester = peoplepickerValue;

                if (oldrequester == "") {
                    oldrequester = newrequester;
                }
                else if (oldrequester != "" && oldrequester != null && oldrequester != newrequester) {
                    isEditOrRequesterChange = true;
                }
                GetUserLocation(peoplepickerValue);
                var modulename = currentModuleName;
                FillDepartmentLocation(peoplepickerValue);
                // Get requestor manager if dependent on requestor
                if (businessManagerDependentField == "Requestor") {
                    GetBusinessManager(peoplepickerValue);
                }
            }
        }

        function FillDepartmentLocation(peoplepickerValue) {

            try {
                var deparmentlocation = $.parseJSON(getDepatmentLocationByUser(peoplepickerValue), true);

                var department = deparmentlocation[0].DepartmentLookup;
                $(".selecteddepartmentdetail").each(function (i, item) {
                    var dID = item.id.replace("_pSelectedDepartments", "");
                    setUGITDepartment(dID, department);
                });

                var location = deparmentlocation[1].LocationLookup;
                var locationCtr = $(".locationctr");
                if (location != "0") {
                    if (isduplicate && !filldepartmentLocationwithduplicate) {
                        locationCtr.val(locationCtr.val());
                        filldepartmentLocationwithduplicate = true;
                    }

                    else
                        locationCtr.val(location);
                    //if(isduplicate && locationCtr.val()=="" && locationCtr.val()==location)
                    //    locationCtr.val(location);
                    //else if(isduplicate && locationCtr.val()!="")
                    //    locationCtr.val(locationCtr.val());
                }
                else {
                    locationCtr.val("");
                }
            }
            catch (ex) {
            }

        }


        function FillManagerFor() {
            // Get new value of dependent user field
            var peoplePicker = $("#" + $("." + businessManagerDependentField)[0].id + "_upLevelDiv")[0];
            var peoplepickerValue = null;
            var pickerHtml = peoplePicker.innerHTML.toLowerCase();
            if (pickerHtml.indexOf("<span") > -1) {
                var userSpans = $(peoplePicker).children("span");
                userSpans.each(function (i, item) {
                    peoplepickerValue = $(item).attr("title");
                });
            }
            else {
                peoplepickerValue = peoplePicker.innerHTML.replace("<br>", "");
            }
            peoplepickerValue = escape(peoplepickerValue);

            // Get manager
            GetBusinessManager(peoplepickerValue);
        }

        var changePriorityOnLoad = false;
        function GetUserLocation(userName) {

            var qData = '{' + '"UserID":"' + userName + '"}';
            $.ajax({
                type: "POST",
                url: "<%= ajaxHelper %>Requestordata",
                data: qData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    var locationid = data.UserLocationID;
                    var location = data.Location;
                    var deskloction = data.UserDeskLoaction;
                    var department = data.Department;
                    var managerid = data.ManagerID;
                    var managername = data.Manager;
                    var name = data.Name;
                    var locationCtr = ASPxClientControl.GetControlCollection().GetByName($(".field_locationlookup_edit").attr("id"));
                    var managerCtr = ASPxClientControl.GetControlCollection().GetByName($(".field_businessmanageruser_edit").attr("id"));
                    if (managerCtr) {
                        managerCtr.SetValue(managerid);
                        $(".field_businessmanageruser_view").text(managername)
                    }
                    if (locationCtr) {
                        if (isEditOrRequesterChange) {
                            if (locationid != undefined && locationid != null && location == '') {
                                $(".field_locationlookup_view").text(location);
                                locationCtr.gridView.SelectRowsByKey(locationid);
                                changeRequestOwner();
                            }
                            else {
                                $(".field_locationlookup_view").text(location);
                                locationCtr.gridView.SelectRowsByKey(locationid);
                                changeRequestOwner();
                            }
                        }
                    }
                    if (changePriorityOnLoad) {
                        var vipCheckbox = $(".chkelevatecheck input");
                        if (data.isUserVIP == "True") {
                            vipCheckbox.attr("checked", "checked");
                            $(".field_prioritylookup_view").html("<%= elevatedPrioirty %>");
                            changePriority();
                        }
                        else {
                            vipCheckbox.removeAttr("checked");
                            changePriority();
                        }
                    }

                    changePriorityOnLoad = true;

                    if ("<%= Convert.ToString(Request.QueryString["BatchEditing"])%>" != "true") {
                        $(".RequestTypeLookup").trigger("change");

                    }


                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }

        function GetBusinessManager(userName) {
            var qData = '{' + '"userName":"' + userName + '"}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelper %>GetBusinessManager",
                data: qData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    if (message.d != "")
                        $("#" + $(".BusinessManager")[0].id + "_upLevelDiv")[0].innerHTML = message.d.split(";#")[1];
                    else
                        $("#" + $(".BusinessManager")[0].id + "_upLevelDiv")[0].innerHTML = "";
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }

        function FindAndAttachEvent() {
            $(".defaultattachmentcontainer").css("display", "none");
            $('.TestingTotalHours').bind("blur", function () { CalculateTotalHour(); });
            $('.BATotalHours').bind("blur", function () { CalculateTotalHour(); });
            $('.DeveloperTotalHours').bind("blur", function () { CalculateTotalHour(); });
        }

        function CalculateTotalHour() {
            var testingHours = 0;
            var baTotalHours = 0;
            var ticketDevTotalHours = 0;
            var testingHoursBox = $('.TestingTotalHours');
            var firstBox = $('.BATotalHours');
            var secondBox = $('.DeveloperTotalHours');

            if (Number(testingHoursBox.val()) && Number(testingHoursBox.val()) != "NaN") {
                testingHours = Number($(testingHoursBox).val());
            }

            if (Number(firstBox.val()) && Number(firstBox.val()) != "NaN") {
                baTotalHours = Number($(firstBox).val());
            }

            if (Number(secondBox.val()) && Number(secondBox.val()) != "NaN") {
                ticketDevTotalHours = Number($(secondBox).val());
            }

            var actualHour = eval(testingHours + baTotalHours + ticketDevTotalHours);
            actualHour = Math.round(actualHour * 100) / 100
            $('.ActualHours span').html(actualHour);
            $('.ActualHours input').val(actualHour);
            $('.BAtotalHours').val(baTotalHours);
            $('.DeveloperTotalHours').val(ticketDevTotalHours);
        }

        function isNumeric(value) {
            if (value != null && !value.toString().match(/^[-]?\d*\.?\d*$/)) return false;
            return true;
        }


        function getUserTooltip() {
            return;
        }
        $(function () {
            var timer;
            var status = 1;

            try {
                $(".jqtooltip").tooltip({
                    hide: { effect: "fadeOut", easing: "easeInExpo" },
                    content: function () {
                        var users = $(this).attr("UsersID");
                        if (users) {
                            var usersID = users.split(",");
                            var tooltipHtml = getUserTooltip(usersID);
                            $(this).attr("title", tooltipHtml);
                        }
                        var title = $(this).attr("title");
                        if (title)
                            return title.replace(/\n/g, "<br/>");
                    }
                });
            }
            catch (ex) {
            }

            try {
                //If stage lable height is less then 20then change top position of label
                $(".alternategraphiclabel").each(function (i, item) {
                    var label = $.trim($(item).find("b").html());
                    if ($(item).find("b").height() < 20) {
                        //If stage lable height is less then 20then change top position of label
                        $(item).css("top", "-18px");
                    }

                });
            } catch (ex) {
            }
            FindAndAttachEvent();

            PreLoadDefaultSelections();

            //$(".labeltooltip").tooltip( {hide: { effect: "fadeOut", easing: "easeInExpo" },
            //    content:function(){
            //        var title = $(this).attr("title");
            //        if(title)
            //            return title.replace(/\n/g, "<br/>");
            //    }
            //});
        });





        function getLabelTooltip(moduleName, fieldDisplayName) {
            var htmltooltip;
            var isnew = true;
            if (parseInt(currentTicketId) > 0) {
                isnew = false;
            }
            var paramsInJson = '{' + '"moduleName":"' + moduleName + '","fieldDisplayName":"' + fieldDisplayName + '","isNew":' + isnew + '}';
            $.ajax({
                type: "POST",
                url: "<%=ajaxHelper %>/GetLabelTooltipHTML",
                data: paramsInJson,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    htmltooltip = message;
                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }


        function deleteFileFromUpload(obj, fileIndex, maxFiles) {
            var blockFiles = $('.fileuploaderContainer').find('tr:visible');
            $('#fileContainer' + fileIndex + " input").val("");
            $('#fileContainer' + fileIndex).css("display", "none");

            blockFiles = $('.fileuploaderContainer').find('tr:visible');

            if (blockFiles.length < maxFiles) {
                $(blockFiles.get(blockFiles.length - 1)).find(".addmorefiles").parent().css("display", "block");
            }
        }

        function addMoreFilesForUpload(obj, fileIndex, maxFiles) {
            var hiddenFiles = $('.fileuploaderContainer').find('tr:hidden');
            if (hiddenFiles.length > 0) {
                var currnetInput = $(hiddenFiles.get(0)).find("input:file");
                var fileInputControl = "<input type='file' id='" + currnetInput.attr("id") + "' name='" + currnetInput.attr("name") + "'/>";
                var containerTd = currnetInput.parent();
                currnetInput.remove();
                containerTd.append(fileInputControl);
                $(hiddenFiles.get(0)).css("display", "block");
            }

            var blockFiles = $('.fileuploaderContainer').find("tr:visible");
            if (maxFiles == blockFiles.length) {
                $(blockFiles.get(blockFiles.length - 1)).find(".addmorefiles").parent().css("display", "none");
            }
        }

        function deleteFileCopyFile(obj, fileIndex, maxFiles) {
            $('#CopyFileContainer' + fileIndex).css("display", "none");
            $('#CopyFileContainer' + fileIndex + " :hidden").val("");
        }


        function DeleteFile(obj, fileIndex) {
            var hiddenFiles = $('.file' + fileIndex);
            if (hiddenFiles.length > 0) {
                var answer = confirm("Are you sure you want to delete this attachment?")
                if (answer) {
                    $('#<%=deletedFile.ClientID%>').val($(hiddenFiles.get(0)).html());
                }
                else {
                    return false;
                }
            }
        }

        $(document).ready(function () {
            AutoAdjustActiveTabContainer();
        });
        function GetViewportHeight() {
            return;
        }
        function AutoAdjustActiveTabContainer() {
            var tabs = $(".moduleDetailTabsContainer111 li");
            var tabsTxt = $(".moduleDetailTabsContainer111 .menu-item-text");
            var moduleName = currentModuleName;
            for (var j = 0; j <= tabs.length; j++) {
                if ($("#tabPanelContainer_" + (j + 1) + " iframe").length == 1) {
                    var totalHeight = GetViewportHeight();
                    var calculatedheight = $(".workflowGraphicContainer111").height() + $(".messageConatiner111").height() + $(".moduleDetailTabsContainer111 div").height() + $(".utilityContainer111 div").height();
                    var containerHeight = parseFloat(totalHeight) - parseFloat(calculatedheight);
                    if ($("#tabPanelContainer_" + (j + 1) + " fieldset").length == 1) {
                        $("#tabPanelContainer_" + (j + 1) + " iframe").height(containerHeight - 40);
                    }
                    else if ($("#tabPanelContainer_" + (j + 1) + " fieldset").length == 0) {
                        $("#tabPanelContainer_" + (j + 1) + " iframe").height(containerHeight);
                    }
                    else if ($("#tabPanelContainer_" + (j + 1) + " fieldset").length == 3) {
                        if (moduleName.toLowerCase() == "tsk") {
                            //$("#tabPanelContainer_"+(j+1) +" fieldset").find('iframe').height(containerHeight-300);
                        }

                    }
                }
            }
        }



        function disableMenuItems() {

            var moduleName = currentModuleName;
            var trackprojectstagehistory = "<%=TrackProjectStageHistory%>"
            // show/hide the item in popupmenu control.
            if (moduleName == "PMM") {
                ASPxPopupMenuReport.GetItemByName("ProjectReport").SetVisible(true);
                ASPxPopupMenuReport.GetItemByName("ResourceHours").SetVisible(true);
                ASPxPopupMenuReport.GetItemByName("ERHReport").SetVisible(true);
                ASPxPopupMenuReport.GetItemByName("ProjectStageHistory").SetVisible(true);
                ASPxPopupMenuReport.GetItemByName("ProjectCompactReport").SetVisible(true);
                ASPxPopupMenuReport.GetItemByName("TaskReport").SetVisible(false);
                if (trackprojectstagehistory.toLowerCase() == "true")
                    ASPxPopupMenuReport.GetItemByName("ProjectStageHistory").SetVisible(true);
                else
                    ASPxPopupMenuReport.GetItemByName("ProjectStageHistory").SetVisible(false);
                ASPxPopupMenuReport.GetItemByName("ProjectActualsReport").SetVisible(true);
            }
            if (moduleName == "TSK") {
                ASPxPopupMenuReport.GetItemByName("TaskReport").SetVisible(false);//true

                ASPxPopupMenuReport.GetItemByName("ProjectReport").SetVisible(false);
                ASPxPopupMenuReport.GetItemByName("ResourceHours").SetVisible(false);
                ASPxPopupMenuReport.GetItemByName("ERHReport").SetVisible(false);
                ASPxPopupMenuReport.GetItemByName("ProjectStageHistory").SetVisible(false);

                ASPxPopupMenuReport.GetItemByName("BudgetReport").SetVisible(false);
                ASPxPopupMenuReport.GetItemByName("ActualsReport").SetVisible(false);
            }

            if (moduleName != "TSK") {
                ASPxPopupMenuReport.GetItemByName("BudgetReport").SetVisible(true);
                ASPxPopupMenuReport.GetItemByName("ActualsReport").SetVisible(false);
            }
        }
        var reportType = '';
        function popupMenuReportItemClick(s, e) {
            if (e.item.name == "TaskReport")
                openProjectReport();
            if (e.item.name == "ProjectReport") {
                reportType = 'ProjectStatusReport';
                GetSelectedProjects();
            }
            if (e.item.name == "ResourceHours") {
                reportType = 'ProjectResourceReport';
                GetSelectedProjects();
            }
            if (e.item.name == "ERHReport") {
                reportType = 'EstimatedRemainingHoursReport';
                GetSelectedProjects();
            }
            if (e.item.name == "ProjectStageHistory")
                OpenTrackprojectStageReport();
            if (e.item.name == "BudgetReport") {
                reportType = 'ProjectBudgetReport';
                GetSelectedProjects();
            }
            if (e.item.name == "ProjectCompactReport") {
                reportType = 'OnePagerReport';
                GetSelectedProjects();
            }
            if (e.item.name == "ActualsReport")
                OpenActualsReportPopup();
            if (e.item.name == "ProjectActualsReport") {
                reportType = 'ProjectActualsReport';
                GetSelectedProjects();
            }
        }

        function GetSelectedFieldValues(field, functionName) {
            if (typeof gridClientInstance !== "undefined")
                return gridClientInstance.GetSelectedFieldValues(field, functionName);
            else if (typeof cardClientInstance !== "undefined")
                return cardClientInstance.GetSelectedFieldValues(field, functionName);
            else
                return null;
        }
        function GetSelectedProjects() {
            values = '<%=currentTicketPublicID%>';
            var params = "";
            if (values.length > 0)
                params = "alltickets=" + values;
            var moduleName = "<%= currentModuleName %>";
            var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName;
            var title = '';
            switch (reportType) {
                case "ProjectStatusReport":
                    var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%=userId %>' + "&" + params;
                    var popupHeader = "Project Status Report";
                    params = params + "&individual=&Filter=";
                    window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                    break;
                case "OnePagerReport":
                    var url = '<%= reportUrl %>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userId %>' + "&" + params;
                    var popupHeader = "1-Pager Project Report";
                    params = params + "&Viewer=&PMMIds=" +'<%=currentTicketId%>';
                    window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                    break;
                case "ProjectResourceReport":

                    var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userId %>' + "&" + params;
                    var popupHeader = "Resource Hours Report";
                    params = params + "&Filter=";
                    window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                    break;
                case "EstimatedRemainingHoursReport":
                    var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userId %>' + "&" + params;
                    var popupHeader = "Estimated Remaining Hours Report";
                    params = params + "&Filter=";
                    window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                    break;
                case "ProjectBudgetReport":
                    var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%= userId %>' + "&" + params;
                    var popupHeader = "Project Budget Summary Report";
                    if (moduleName == 'ITG')
                        popupHeader = 'Non-Project Budget Summary Report';
                    params = params + "&TicketId=" + '<%=currentTicketPublicID%>' + "&Filter=";
                    window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                    break;
                case "ProjectActualsReport":
                    var url = '<%=reportUrl%>' + "?reportName=" + reportType + "&Module=" + moduleName + '&userId=<%=userId%>' + "&" + params;
                    var popupHeader = "Project Actuals Report";
                    params = params + "&TicketId=" + '<%=currentTicketPublicID%>' + "&Filter=";
                    window.parent.UgitOpenPopupDialog(url, params, popupHeader, '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
                default:
                    break;
            }

            return false;
        }
    </script>

    <script data-v="<%=UGITUtility.AssemblyVersion %>">
        function MndtryFldTbShow(obj) {
            if (obj > 0)
                tbcDetailTabs.SetActiveTab(tbcDetailTabs.GetTab(obj - 1));
        }

        function OpenTrackprojectStageReport() {
            var trackProjectStageUrl = '<%=TrackProjectStageUrl%>&publicTicketId=<%=currentTicketPublicID%>';
            window.parent.UgitOpenPopupDialog(trackProjectStageUrl, '', 'Project Stage History', '800px', '500px');

        }

        function item_mousehover(obj, itemId, currentList) {
            if (currentList.toLowerCase() == 'assets')
                showAssetDetails(obj, itemId, currentList);
            else if (currentList.toLowerCase() == 'assetvendors')
                showAssetVendorDetails(obj, itemId, currentList);
            else if (currentList.toLowerCase() == 'assetmodels')
                showAssetModelDetails(obj, itemId, currentList);
        }
        function item_mouseout(obj, itemId, currentList) {
            if (currentList.toLowerCase() == 'assets')
                aspxPopupToolTip.Hide();
            else if (currentList.toLowerCase() == 'assetvendors')
                aspxPopupAssetVendorToolTip.Hide();
            else if (currentList.toLowerCase() == 'assetmodels')
                aspxPopupAssetModelToolTip.Hide();
        }

        var assetDataList = {};
        var tooltipColMapping = {
            TicketId: 'Asset ID', AssetTagNum: 'Asset Tag', AssetName: 'Asset Name', OwnerUser: 'Owner', RequestTypeLookup: 'Asset Type', VendorLookup: 'Vendor'
            , AssetModelLookup: 'Model', Status: 'Status', AssetDispositionChoice: 'Disposition', HostName: 'Host Name'
            , IPAddress: 'IP Address', DepartmentLookup: 'Department', LocationLookup: 'Location'
        };
        function showAssetDetails(trObj, cItemId, cCurrentList) {
            var assetObj = $(trObj);
            if (assetObj.length > 0) {
                var currentAssetData = assetDataList[cItemId];
                if (currentAssetData) {
                    $(".aspxPopupToolTipContent").html(currentAssetData.message);
                }
                else {
                    $(".aspxPopupToolTipContent").html("Loading..");
                }

                aspxPopupToolTip.ShowAtElement(assetObj.get(0));
                var jsonData = { "cAssetId": cItemId, "listName": cCurrentList };

                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelperPage %>/GetToolTip",
                    data: JSON.stringify(jsonData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (message) {
                        try {
                            var json = $.parseJSON(message.d);
                            if (json) {
                                var htmlData = [];
                                $.each(json, function (key, val) {
                                    var cols = key.split(";~");
                                    var finalStr = '';
                                    if (cols.length > 1) {
                                        var firstCol = tooltipColMapping[cols[0]];
                                        var secondCol = tooltipColMapping[cols[1]];
                                        finalStr = firstCol + ' & ' + secondCol;
                                    }
                                    else
                                        finalStr = tooltipColMapping[cols[0]];

                                    htmlData.push("<div><b>" + finalStr + ": </b><span>" + val + "</span></div>");
                                });

                                assetDataList[cItemId] = { message: htmlData.join(""), json: json };
                                $(".aspxPopupToolTipContent").html(htmlData.join(""));
                            }
                        } catch (ex) {
                            $(".aspxPopupToolTipContent").html("No Data Available");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }

        var assetVendorDataList = {};
        var assetVendortooltipColMapping = {
            VendorName: 'Vendor', ContactName: 'Contact Name', VendorLocation: 'Location', VendorPhone: 'Phone', VendorEmail: 'Email', VendorAddress: 'Address'
        };
        function showAssetVendorDetails(trObj, cItemId, cCurrentList) {
            var assetVendorObj = $(trObj);
            if (assetVendorObj.length > 0) {
                var currentAssetVendorData = assetVendorDataList[cItemId];
                if (currentAssetVendorData) {
                    $(".aspxPopupAssetVendorToolTipContent").html(currentAssetVendorData.message);
                }
                else {
                    $(".aspxPopupAssetVendorToolTipContent").html("Loading..");
                }

                aspxPopupAssetVendorToolTip.ShowAtElement(assetVendorObj.get(0));
                var jsonData = { "cAssetId": cItemId, "listName": cCurrentList };

                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelperPage %>/GetToolTip",
                    data: JSON.stringify(jsonData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (message) {
                        try {
                            var json = $.parseJSON(message.d);
                            if (json) {
                                var htmlData = [];
                                $.each(json, function (key, val) {
                                    var cols = key.split(";~");
                                    var finalStr = '';
                                    if (cols.length > 1) {
                                        var firstCol = assetVendortooltipColMapping[cols[0]];
                                        var secondCol = assetVendortooltipColMapping[cols[1]];
                                        finalStr = firstCol + ' & ' + secondCol;
                                    }
                                    else
                                        finalStr = assetVendortooltipColMapping[cols[0]];

                                    htmlData.push("<div><b>" + finalStr + ": </b><span>" + val + "</span></div>");
                                });

                                assetVendorDataList[cItemId] = { message: htmlData.join(""), json: json };
                                $(".aspxPopupAssetVendorToolTipContent").html(htmlData.join(""));
                            }
                        } catch (ex) {
                            $(".aspxPopupAssetVendorToolTipContent").html("No Data Available");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }

        var assetModelDataList = {};
        var assetModeltooltipColMapping = {
            VendorLookup: 'Vendor', ModelName: 'Model Name', ExternalType: 'Type', ModelDescription: 'Description', BudgetLookup: 'Budget Item'
        };

        function showAssetModelDetails(trObj, cItemId, cCurrentList) {
            var assetModelObj = $(trObj);
            if (assetModelObj.length > 0) {
                var currentAssetModelData = assetModelDataList[cItemId];
                if (currentAssetModelData) {
                    $(".aspxPopupAssetModelToolTipContent").html(currentAssetModelData.message);
                }
                else {
                    $(".aspxPopupAssetModelToolTipContent").html("Loading..");
                }

                aspxPopupAssetModelToolTip.ShowAtElement(assetModelObj.get(0));
                var jsonData = { "cAssetId": cItemId, "listName": cCurrentList };

                $.ajax({
                    type: "POST",
                    url: "<%=ajaxHelperPage %>/GetToolTip",
                    data: JSON.stringify(jsonData),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (message) {
                        try {
                            var json = $.parseJSON(message.d);
                            if (json) {
                                var htmlData = [];
                                $.each(json, function (key, val) {
                                    var cols = key.split(";~");
                                    var finalStr = '';
                                    if (cols.length > 1) {
                                        var firstCol = assetModeltooltipColMapping[cols[0]];
                                        var secondCol = assetModeltooltipColMapping[cols[1]];
                                        finalStr = firstCol + ' & ' + secondCol;
                                    }
                                    else
                                        finalStr = assetModeltooltipColMapping[cols[0]];

                                    htmlData.push("<div><b>" + finalStr + ": </b><span>" + val + "</span></div>");
                                });

                                assetModelDataList[cItemId] = { message: htmlData.join(""), json: json };
                                $(".aspxPopupAssetModelToolTipContent").html(htmlData.join(""));
                            }
                        } catch (ex) {
                            $(".aspxPopupAssetModelToolTipContent").html("No Data Available");
                        }
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                    }
                });
            }
        }
    </script>   

   </asp:Panel>
<dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="callbackcontrol"
        OnCallback="contractSummary_Callback">
    <ClientSideEvents EndCallback="DownloadContractSummaryfile"/>
</dx:ASPxCallback>

<%--When Attachment field control is not rendered on page, then its show some weird value in field control value.
 Add temporary attachment control will solve this issue.--%>
<asp:Panel ID="tempAttachment" runat="server" Style="display: none;"></asp:Panel>
<asp:HiddenField ID="hdnpagebreakup" runat="server" />
<asp:HiddenField ID="hdnAutoUpdateAllocaionDates" runat="server" Value="false" />
<asp:HiddenField ID="hdnUpdatePastDates" runat="server" Value="false" />
<asp:Panel runat="server" ID="unauthorizedPanel" GroupingText="Access Denied">
    <asp:Label runat="server" ID="unauthorizedLabel"> Unauthorized, please contact the administrator to request access.    
    </asp:Label>
</asp:Panel>
<dx:ASPxPopupControl ID="aspxPopupToolTip" runat="server" CloseAction="CloseButton"
    PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
    ShowFooter="false" ShowHeader="false" Width="300px" Height="50px" HeaderText="Hold Details" ClientInstanceName="aspxPopupToolTip">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl16" runat="server">
            <div style="vertical-align: middle" class="aspxPopupToolTipContent">
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="aspxPopupAssetVendorToolTip" runat="server" CloseAction="CloseButton"
    PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
    ShowFooter="false" ShowHeader="false" Width="300px" Height="50px" HeaderText="Hold Details" ClientInstanceName="aspxPopupAssetVendorToolTip">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl17" runat="server">
            <div style="vertical-align: middle" class="aspxPopupAssetVendorToolTipContent">
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="aspxPopupAssetModelToolTip" runat="server" CloseAction="CloseButton"
    PopupVerticalAlign="Above" PopupHorizontalAlign="RightSides"
    ShowFooter="false" ShowHeader="false" Width="300px" Height="50px" HeaderText="Hold Details" ClientInstanceName="aspxPopupAssetModelToolTip">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl18" runat="server">
            <div style="vertical-align: middle" class="aspxPopupAssetModelToolTipContent">
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="PopupUploadTicketIcon" ClientInstanceName="PopupUploadTicketIcon" runat="server"
    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" CloseAction="CloseButton" HeaderText="Upload Icon"
    ShowFooter="false" ShowHeader="true" Width="300px" Height="100px">
    <ContentCollection>
        <dx:PopupControlContentControl>
            <dx:ASPxUploadControl ID="uploadTicketIcon" ClientInstanceName="uploadTicketIcon" runat="server"></dx:ASPxUploadControl>
            <dx:ASPxButton ID="btnUploadTicketIcon" runat="server" CssClass="primary-blueBtn" Text="Save Icon" OnClick="btnUploadTicketIcon_Click">
                <ClientSideEvents Click="btnUploadTicketIcon_click" />
            </dx:ASPxButton>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<a id="downloadContractSummaryfile" hidden href="#"></a>

<%--Callback to run Statistics--%>
 <dx:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel_Actions" Height="100%" ClientInstanceName="ASPxCallbackPanel_Actions" Enabled="true" OnCallback="ASPxCallbackPanel_Actions_Callback">
    <SettingsLoadingPanel Enabled="false" Text="Loading..." />
    <PanelCollection>
        <dx:PanelContent runat="server">
            <dx:ASPxLoadingPanel ID="busyLoader" CssClass="customeLoader" ClientInstanceName="busyLoader" Modal="True" runat="server" Image-Url="~/Content/IMAGES/AjaxLoader.gif" 
                ImagePosition="Top" Text="Loading...">
                <Image Url="~/Content/IMAGES/AjaxLoader.gif"></Image>
            </dx:ASPxLoadingPanel>
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxCallbackPanel>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CapacityReport.ascx.cs" Inherits="uGovernIT.Web.CapacityReport" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">    
    body input.dxeEditArea_UGITNavyBlueDevEx /*Bootstrap correction*/ {
        color: inherit;
    }

    .ModuleBlock {
        float: left;
        background: none repeat scroll 0 0 #ECE8D3;
        border: 4px double #FCCE92;
        position: absolute;
        z-index: 1;
    }

    .radiobutton label {
        /*margin-left: 5px;*/
        margin-right: 1px;
    }

    .checkboxbutton label {
        padding: 3px 0 0 3px;
    }
    /*.valueViewMode {
        float: left; border: solid; border-width: 1px; border-color: #D9DAE0; height: 25px; padding-top: 3px; padding-left: 5px; padding-right:5px;
    }*/
    /*.valueViewMode label {
        position:relative;
        top:2px;
    }*/
    .valueTypeMode {
        float: left; padding-left: 30px; 
    }
    /*.valueTypeMode label {
        position: relative;
        top: 2px;
    }*/
    /*.viewProjectMode {
        float: left; padding-top: 5px;padding-left:5px;
    }*/
    .viewProjectMode label {
        position: relative;
        top: 2px;
        padding-right:2px;
    }

    .valueComplexityMode{
        float:left;
        padding-left:30px;
    }

    .rmmLookup-valueBoxEdit table.department tr td.dxic input[type="text"] {
        height: 20px !important;
        background: #fff;
    }

    .reportPanelImage {
        float: right;
    }
    .reportPanelImage {
        display: inline-block;
        padding-left: 5px;
    }
    .displayHide {
        display: none;
    }
    .valueViewMode, .viewProjectMode, .allocationView {
        border: 1px solid #D9DAE0;
        padding: 8px 10px 11px 5px;
        border-radius: 4px;
        display: inline-flex;
        justify-content: space-evenly;
    }
    .RMM-checkWrap label::before {
        content: '';
        -webkit-appearance: none;
        background-color: transparent;
        border: 2px solid black;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05), inset 0px -15px 10px -12px rgba(0, 0, 0, 0.05);
        padding: 6px;
        display: inline-block;
        position: relative;
        height: 17px;
        margin-top: 7px;
        vertical-align: middle;
        cursor: pointer;
        margin-right: 5px;
    }
    .RMM-checkWrap label {
        display:flex;
    }
     .resource-img {
        width: 18px;
        filter: brightness(0) invert(0);
    }
     .RMM-checkWrap input:checked + label::after {
        content: '';
        display: block;
        position: absolute;
        top: 10px;
        left: 9px;
        width: 6px;
        height: 9px;
        border: solid black;
        border-width: 0 2px 2px 0;
        transform: rotate(45deg);
    }
     .dxichTextCellSys {
        padding: 2px 0px 1px;
        font-size: 13px;
        font-weight: 400;
    }
     .preconDateStyle {
    background-color: #52BED9;
    border: 2px solid #52BED9;
    color: #fff;
}
.constDateStyle {
    background-color: #005C9B;
    border: 2px solid #005C9B;
    color: #fff;
}
.closeoutDateStyle {
    background-color: #351B82;
    border: 2px solid #351B82;
    color: #fff;
}
.otherDateStyle {
    background-color: #fff;
    border: 2px solid #aca9a9;
    color: #000000de;
}
.dateConstClass {
    display: grid;
    align-content: center;
    text-align: center;
    padding: 8px;
    font-size: 13px;
}
.title-opm {
    padding: 4px;
    border-radius: 11px;
    border: 2px solid #4fa1d6;
    font-weight: 500;
    font-size: 13px;
    background: #fff;
    color: #000;
    text-align: center;
    width: 100%;
    min-height: 38px;        
}
.devExtDataGrid-DataRow {
    border-bottom: 2px solid #f6f7fb !important;
}
.title-cpr {
    border: none;
    padding: 5px;
    border-radius: 11px;
    font-weight: 500;
    font-size: 13px;
    background: #4fa1d6;
    color: white !important;
    text-align: center;
    width: 100%;
    min-height: 38px;
}
.grdCell {
    padding: 10px;
    font-weight: 400;
    font-size: 13px;
    background: #ededed;
    color: #4b4b4b;
    text-align: center;
}
    .dataIdcell {
        height: 38px;
        background-color: white;
        border: 2px solid #ddd;
        padding: 7px;
        color: #4b4b4b;
        text-align: center;
        font-weight: 400;
        font-size: 13px;
    }
    .dx-checkbox-text {
    margin-top: 2px;
}

.custom-icon .icon {
    font-size: 17px;
    color: #f05b41;
    margin-right: 2px;
}

.dx-field {
    margin-bottom: 50px;
}

.availibility-1 {
    width: 200px;
    margin-top: 9px;
}

.availibility-2 {
    text-align: center;
    width: 180px;
}
.conflict-circle {
    width: 30px;
    height: 30px;
    border-radius: 20px;
    position: absolute;
    right: -10px;
    bottom: -2px;
    color: black;
    padding-top: 7px;
    background-color: orange;
    font-weight: 600;
}

.date-box {
    width: 50%;
    border: 1px solid #ddd !important;
    padding: 3px;
}

.date-box-1 {
    border: 2px solid #ddd;
    padding: 1px;
    border-top: 0px;
}

.overflow {
    overflow-x: hidden !important;
}

#divProjectView .dx-scrollable-content {
    top: 11px !important;
}

a:visited {
    color: #4A6EE2;
    text-decoration: none;
}

.paneldiv .panelImgDiv1 {
    background: gray;
    width: 50px;
    height: 50px;
    margin-left: auto;
    margin-right: auto;
    border-radius: 50%;
    margin-bottom: 10px;
    overflow: hidden;
}

.paneldiv p {
    font-size: 12px;
}

#divProjectView .dx-tile {
    left: auto !important;
    margin-top: -21px;
}

#divProjectView .dx-scrollview-content.dx-tileview-wrapper .dx-item.dx-tile, #divLeftPanel .dx-list-item .detailblock .dx-scrollview-content.dx-tileview-wrapper .dx-item.dx-tile {
    position: relative;
    height: 110px !important;
    width: 200px !important;
    margin-right: 10px;
}

#divProjectView .dx-scrollview-content.dx-tileview-wrapper, #divLeftPanel .dx-list-item .detailblock .dx-scrollview-content.dx-tileview-wrapper {
    display: flex;
    height: auto;
    width: auto !important;
    justify-content: flex-start;
}

.outer-border-date-box {
    border: 2px solid #ddd;
    height: 69px;
}
#ConflictWeekDataGridDialog .dx-popup-content, #ConflictWeekDataGridDialogSummary .dx-popup-content {
    margin-top:-10px;
}
.dx-overlay-wrapper.dx-overlay-shader.dx-loadpanel-wrapper{
  z-index:200000001 !important
}

    .switch {
        position: relative;
        display: inline-block;
        width: 44px;
        height: 18px;
    }

        .switch input {
            opacity: 0;
            width: 0;
            height: 0;
        }

    .slider {
        position: absolute;
        cursor: pointer;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background-color: #ccc;
        -webkit-transition: .4s;
        transition: .4s;
    }

        .slider:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 0px;
            bottom: 0px;
            background-color: #4fa1d6;
            -webkit-transition: .4s;
            transition: .4s;
        }

    input:checked + .slider {
        background-color: #ccc;
    }

    input:focus + .slider {
        box-shadow: 0 0 1px #ccc;
    }

    input:checked + .slider:before {
        -webkit-transform: translateX(26px);
        -ms-transform: translateX(26px);
        transform: translateX(26px);
    }

    /* Rounded sliders */
    .slider.round {
        border-radius: 34px;
    }

        .slider.round:before {
            border-radius: 34px;
        }


    .tag-container {
        display: flex;
        align-items: center;
        margin-bottom: 4px;
        justify-content: space-between;
    }

    .projectExperiencePopup .dx-tag-content-1, #projectExpTags .dx-tag-content-1 {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px dotted gray !important;
        margin: 4px 0 0 4px;
        padding: 3px 21px 4px 9px !important;
        min-width: 40px;
        background-color: #fff !important;
        border-radius: 28px;
        color: #333 !important;
        font-weight: 500;
    }

    .projectExperiencePopup .dx-tag-content, #projectExpTags .dx-tag-content {
        position: relative;
        display: inline-block;
        text-align: center;
        border: 2px #ddd;
        border-radius: 28px;
        cursor: auto;
        margin: 4px 3px 4px 4px;
        padding: 5px 21px 6px 12px;
        min-width: 40px;
        background-color: #ededed;
        /* border-radius: 2px; */
        color: #333;
        font-weight: 500;
    }

    .projectExperiencePopup .dx-overlay-wrapper {
        font-family: 'Roboto', sans-serif !important;
        color: black !important;
        /* color: black; */
    }

    .projectExperiencePopup .font-size-class {
        font-size: 15px;
    }

    .boxProp {
        box-shadow: 0 6px 14px #ccc;
        padding: 15px;
        height: 100%;
        width: 100%;
        /* display: flex; */
        /* align-items: center; */
    }

    #projectExpTags.dx-texteditor.dx-editor-outlined {
        background: #fff;
        border: 0px solid #ddd;
        border-radius: 4px;
    }

    #addExpTags.dx-button-has-icon .dx-icon {
        width: 25px;
        height: 25px;
    }

    #clearExpTags.dx-button-has-icon .dx-icon {
        width: 40px;
        height: 25px;
    }

    .th-header {
        display: flex;
        flex-direction: column;
        align-items: center;
        width: 90px;
    }

    .count-box {
        padding: 10px 0px;
        background: aquamarine;
        margin: 5px;
        font-weight: 500;
        font-size: 14px;
        width: 60px;
        text-align: center;
    }

    .tag-name {
        margin-top: 13px;
        font-weight: 500;
        font-size: 14px;
    }

    .userExperiencePopup {
        font-family: 'Roboto', sans-serif !important;
    }

    .tr-border {
        border-bottom: 2px solid gray;
    }

    .tag-container-1 {
        display: flex;
        align-items: center;
        margin-bottom: 10px;
        justify-content: center;
    }

    .availibility-4 {
        text-align: center;
        width: 134px;
    }

    .availibility-3 {
        width: 146px;
        margin-top: 9px;
    }

    #compareTags .dx-icon {
        filter: invert(1);
        border: 2px solid white;
    }

    .tag-container-2 {
        display: flex;
        justify-content: flex-start;
        align-items: center;
    }

    .dx-tag-1 {
        /*margin-top: -4px;*/
    }

    .dx-popup-content-popover {
        padding: 10px 15px !important;
    }

    .userImageStyle {
        width: 35px;
        border-radius: 50%;
    }

    .rounded-circle {
        background: #ededed;
        border-radius: 15px;
        padding: 5px 10px;
        margin-left: 2px;
        margin-right: 2px;
        text-align: center;
    }

    .rounded-circle-dotted {
        background: white;
        border: 1px dotted black;
        border-radius: 15px;
        padding: 5px 10px;
        margin-left: 2px;
        margin-right: 2px;
        text-align: center;
    }

    #form .dx-layout-manager .dx-label-h-align.dx-flex-layout {
        display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
        flex-direction: column;
    }

    #form .dx-form-group-with-caption > .dx-form-group-content {
        padding-top: 0px !important;
        margin-top: 0px !important;
        border-top: 0px solid #ddd !important;
        padding-bottom: 15px !important;
    }

    #divMixedSwitch .dx-button-content {
        padding: 2px 5px 2px !important;
        background-color:#f3eded;
    }

    .dx-item.dx-toolbar-item.dx-toolbar-label{
        max-width:100% !important;
    }
    #ConflictWeekDataGridDialog .dx-popup-bottom.dx-toolbar, #ConflictWeekDataGridDialogSummary .dx-popup-bottom.dx-toolbar {
        padding-top: 0px !important;
    }

    #ConflictWeekDataGridDialog .dx-popup-title.dx-has-close-button, #ConflictWeekDataGridDialogSummary .dx-popup-title.dx-has-close-button {
        padding-top: 9px;
        padding-bottom: 9px;
        font-size: 16px;
        font-weight: 500;
        background-color: #f0f0f0;
    }
    #ConflictWeekDataGridDialog .close-btn, #ConflictWeekDataGridDialogSummary .close-btn {
        position: absolute;
        right: 15px;
        margin-top: -3px;
        cursor:pointer;
    }
    .color-style {
        color: #4A6EE2;
        text-decoration: underline;
        font-weight: 500;
        text-align: center;
    }
    .profileUserImg{
        height:35px;
        width:35px;
        border-radius:35px;
    }
    #commonUserProject .dx-datagrid-headers.dx-datagrid-nowrap {
        padding-right: 0px;
        background-color: #ddd;
        color: black;
        font-size: 13px;
        font-weight: 500;
    }
    #commonUserProject .dx-datagrid .dx-column-lines > td {
        border-left: 0px solid #ddd;
        border-right: 0px solid #ddd;
        cursor:pointer;
    }
    #commonUserProject .dx-row.dx-data-row.dx-column-lines {
        border-bottom: 2px solid #ddd;
    }
    #commonUserProject .exptags-style {
        text-align: center;
        width: 40px;
        border: 2px solid #ddd;
        border-radius: 5px;
        padding: 5px 0px;
        padding-top: 4px;
        font-size: 14px;
        font-weight: 500;
    }
    /*.dx-popup-bottom .dx-button {
        min-width: 100px;
        background-color: #789CCE;
        color: white;
    }*/
    .btnBlue {
        background-color: #789CCE !important;
        color: white;
    }
    .btnNormal {
        background-color: white !important;
        color: black;
    }
    .btnCancelAlert {
        background-color: #f65d50 !important;
        color: white;
    }
    .btnSaveAllocation {
        background-color: #789CCE !important;
        color: white;
        background-image: url(/content/Images/SaveWhite.png) !important;
        background-repeat: no-repeat !important;
        background-size: 15px !important;
        top: 10px;
        background-position: 4px 7px !important;
    }

    #btnSaveClose .dx-button-text {
        margin-left: 20px;     
    }

    .dx-button-text {
           font-family: 'Roboto', sans-serif !important;
           font-size:12px;
    }

    #updatePhaseDates .dx-checkbox-container {
    display: flex;
    flex-direction: row-reverse;
    font-weight:500;

  }
    #updatePhaseDates .dx-checkbox-icon {
    width: 22px;
    height: 22px;
    margin-top: 2px;
    border-radius: 2px;
    border: 1px solid #ddd;
    background-color: #fff;
    margin-left: 5px;
}
    .btnShowGanttView .dx-button-content {
    padding: 0;
}

.filterlb-jobtitle {
    float: left;
    padding-left: 15px;
    float: left;
    padding-top: 7px;
    margin-top: 5px;
    width: 80px;
}

.filterctrl-jobtitle {
    width: 25%;
    display: inline-block;
    margin-top: 6px;
}

.filterlb-jobDepartment {
    float: left;
    clear: both;
    padding-left: 15px;
    float: left;
    padding-top: 7px;
    padding-right: 7px;
    margin-top: 5px;
    width: 100px;
}

.filterctrl-jobDepartment, .filterctrl-userpicker {
    width: 25%;
    display: inline-block;
    margin-top: 6px;
}

.filterctrl-experiencedTag {
    /*clear: both;
    float: left;*/
    width: 25.3%;
    display: inline-block;
    margin-top: -6px;
    margin-left: 12px;
}

.filterctrl-addexperiencedTag {
    /*clear: both;
    float: left;*/
    width: 25.3%;
    display: inline-block;
    margin-top: -6px;
    margin-left: 12px;
    border-style: none !important;
}

.filterctrl-userpicker .dx-dropdowneditor-icon::before {
    content: "\f027";
}

.cls .dx-datagrid-revert-tooltip {
    display: none;
}

.display-flex {
    display: flex;
    padding-right: 18px;
}
 .tableTileView .dx-tile, .tileViewContainer .dx-tile {
     text-align: center;
 }
    .tableTileView .dx-empty-message, #tileViewContainer .dx-empty-message {
        text-align: center;
        padding-top: 62px;
    }

    .tableTileView .capacityblock, #tileViewContainer .capacityblock {
        float: left;
        width: 74px;
        text-align: center;
        /*height: 20px;*/
        padding-top: 2px;
        padding-bottom: 2px;
    }

        .tableTileView .capacityblock:first-child, #tileViewContainer .capacityblock:first-child {
            border-right: 1px solid #c3c3c3;
        }

    .tableTileView .capacityblock-1, #tileViewContainer .capacityblock-1 {
        float: left;
        width: 148px;
        text-align: center;
        /*height: 20px;*/
        padding-top: 2px;
        padding-bottom: 2px;
    }

        .tableTileView .capacityblock-1:first-child, #tileViewContainer .capacityblock-1:first-child {
            border-right: 1px solid #c3c3c3;
        }

    .tableTileView .allocation-v0, #tileViewContainer .allocation-v0 {
        background: #ffffff;
        color: #000;
    }

    .tableTileView .allocation-v1, #tileViewContainer .allocation-v1 {
        background: #fcf7b5;
        color: #000;
    }

    .tableTileView .allocation-v2, #tileViewContainer .allocation-v2 {
        background: #f8ac4a;
        color: #000;
    }

    .tableTileView .allocation-r0, #tileViewContainer .allocation-r0 {
        /*background: #57A71D;*/
        background:#6BA538;
        color: #fff;
    }

    .tableTileView .allocation-r1, #tileViewContainer .allocation-r1 {
        /*background: #A9C23F;*/
        background: #6BA538;
        color: #fff;
    }

    .tableTileView .allocation-r2, #tileViewContainer .allocation-r2 {
        background: #FF3535;
        color: #ffffff;
    }

    /*    #tileViewContainer .allocation-r3 {
        background: #f8ac4a;
    }
*/
    .tableTileView .allocation-r3, #tileViewContainer .allocation-r3 {
        background: #FF3535;
        color: #ffffff;
    }

    .tableTileView .allocation-c0, #tileViewContainer .allocation-c0 {
        background: #ffffff;
        color: #000;
    }

    .tableTileView .allocation-c1, #tileViewContainer .allocation-c1 {
        background: #fcf7b5;
        color: #fff;
    }

    .tableTileView .allocation-c2, #tileViewContainer .allocation-c2 {
        background: #f8ac4a;
        color: #000;
    }

    .highlightedBox {
        background: #cbc500 !important;
        border: 2px solid #375268;
    }

    .tableTileView .allocation-block, #tileViewContainer .allocation-block {
        height: 59px;
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 7px;
    }

        .tableTileView .allocation-block .timesheet, #tileViewContainer .allocation-block .timesheet {
            position: absolute;
            top: 4px;
            left: 86%;
            cursor: pointer;
        }

    .tableTileView .dx-tile, #tileViewContainer .dx-tile {
        border: none;
    }

    .tableTileView .capacitymain, #tileViewContainer .capacitymain {
        border-top: 1px solid #c3c3c3;
        overflow: hidden;
        border-bottom-left-radius: 5px;
        border-bottom-right-radius: 5px;
        margin-top: 3px;
    }
    .innerCheckbox {
        position: absolute;
        right: 0px;
        height: 100%;
        bottom: 0px;
        height: 14px;
        width: 14px;
        padding: 1em 1.2em !important;
        padding-bottom: 5em !important;
    }
    .clsSmartFilter {
        float: left;
        padding-top: 2px;
        font-weight: 500;
        padding-left: 10px;
    }
    .shadow-effect {
        box-shadow: 0 6px 14px #ddd;
        border-radius: 6px;
        padding-top: 10px;
        padding-bottom: 15px;
    }
    .dx-checkbox-container {
        align-items: flex-start;
    }
    .dx-datagrid .dx-column-lines > td {
        border-left: 2px solid #ddd;
        border-right: 2px solid #ddd;
    }
    .profileUserImg{
    height:35px;
    width:35px;
    border-radius:35px;
}
     .chkFilterCheck {
     /*padding-top:5px;*/
     padding-left: 2px;
     padding-right: 2px;
     width: unset;
 }
      #compareResume .dx-icon, #openGanttView .dx-icon, #compareTags .dx-icon{
     width: 25px !important;
     height: 25px !important;
 }
 #openCommonProject .dx-icon {
     width: 32px !important;
     height: 25px !important;
 }
 #compareResume .dx-button-mode-contained.dx-state-hover, #openGanttView .dx-button-mode-contained.dx-state-hover, #compareTags .dx-button-mode-contained.dx-state-hover
 , #openCommonProject .dx-button-mode-contained.dx-state-hover{
     background-color: white !important;
 }

 .btnAddNew .dx-icon {
     filter: brightness(10);
 }

 #btnAddNew .dx-icon {
     margin-right: 4px;
     transform: scale(1.4);
     filter: none;
 }

 .btnAddNew .dx-button-content {
     margin: 0px 5px;
 }

</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        $(function () {

            var rows = gvResourceAvailablity.GetVisibleRowsOnPage()
            if (rows > 16)
                gvResourceAvailablity.SetHeight(1000);
            else
                gvResourceAvailablity.SetHeight(rows * 22 + 100);

            $(".reportPanelImage").click(function () {

                if ($(".ExportOption-btns").is(":visible") == true)
                    $(".ExportOption-btns").hide();
                else
                    $(".ExportOption-btns").show();

            });

            $(".checkboxbutton").click(function () {
                //ResourceAvailabilityloadingPanel.Show();
                ShowLoader();
            });
        });

        function adjustControlSize() {
            setTimeout(function () {
                $("#s4-workspace").width("100%");
                var height = $(window).height();
                $("#s4-workspace").height(height);
            }, 10);
        }

        function OnSelectionChanged(s, e) {
            if ('<%= string.IsNullOrEmpty(Convert.ToString(Request["pGlobalRoleID"]))%>' == "False") {
                //$("[id$='btnClose']").get(0).click();
            }
        }


    function ClickOnDrillDown(obj, fieldname, caption) {
        $('#<%= hdnSelectedDate.ClientID%>').val(fieldname);
        if (typeof $("[id$='btnDrilDown']").get(0) != 'undefined') {
            showLoader();
            $("[id$='btnDrilDown']").get(0).click();
        }
        else {
            return;
        }
    }

    function ClickOnDrillUP(obj, caption) {
        $('#<%= hdnSelectedDate.ClientID%>').val(caption);
        if (typeof $("[id$='btnDrilUp']").get(0) != 'undefined') {
            showLoader();
            $("[id$='btnDrilUp']").get(0).click();
        }
        else {
            return;
        }
    }
    function refreshGridDataUsingCallback() {
        showLoader();
        gvResourceAvailablity.PerformCallback();
    }
    function rbtnCount_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnPercentage_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnHrs_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnFTE_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function rbtnAvailability_CheckedChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function chkAll_ValueChanged(s, e) {
        refreshGridDataUsingCallback();
    }

    function chkIncludeClosed_ValueChanged(s, e) {
        refreshGridDataUsingCallback();
    }
    function OnEndCallback(s, e) {
        ResourceAvailabilityloadingPanel.Hide();
    }
        function ClickOnPrevious() {
            $('#<%= previousYear.ClientID%>').click();

        }

        function ClickOnNext() {
            $('#<%= nextYear.ClientID%>').click();

        }
        function OpenAddAllocationPopup(obj, userName) {
            window.parent.UgitOpenPopupDialog('<%= absoluteUrlView %>' + '&SelectedUsersList=' + obj + '&groupId=' + $('#<%= hdnSelectedGroup.ClientID%>').val(), "", 'Add Allocation for ' + userName, '600px', '410px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }


        function openResourceAllocationDialog(path, titleVal, returnUrl) {
            window.parent.parent.UgitOpenPopupDialog(path, '', titleVal, 85, 90, false, returnUrl);
        }

        function showTaskActions(ID) {
            $("[id$='actionButtons" + ID + "']").css("display", "block");
        }

        function hideTaskActions(ID) {
            //show description icon
            //$("#actionButtons" + taskID).css("display", "none");
            $("[id$='actionButtons" + ID + "']").css("display", "none");
        }

        function ShowLoader() {
            //ResourceAvailabilityloadingPanel.Show();
            var width = window.innerWidth;
            var height = window.outerHeight;
            ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
        }

        function TypeCloseUp(s, e) {
            if ($('#<%= hdnTypeLoader.ClientID%>').val() != s.GetText()) {
                ShowLoader();
            }
        }


    function LoaderOnExport(text) {
        ResourceAvailabilityloadingPanel.SetText(text + '...');
        //ResourceAvailabilityloadingPanel.Show();
        var width = window.innerWidth;
        var height = window.outerHeight;
        ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
        setTimeout(function () { ResourceAvailabilityloadingPanel.Hide(); }, 3000);
    }

    function showLoader() {
        //ResourceAvailabilityloadingPanel.Show();
        var width = window.innerWidth;
        var height = window.outerHeight;
        ResourceAvailabilityloadingPanel.ShowAtPos((width / 2), (height / 4));
    }

    function ShowEditImage(objthis) {
        $(objthis).find('img').css('visibility', 'visible');
    }
    function HideEditImage(objthis) {
        $(objthis).find('img').css('visibility', 'hidden');
    }

    function onDepartmentChanged(ccID) {
        /*
        var cmbDepartment = $("#" + ccID + " span");
        //var selectedDepartments = cmbDepartment.attr("id");
        var selectedDepts = "";
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepts = selectedDepts + cmbDepartment[i].id + ",";

        cbpResourceAvailability.PerformCallback("LoadRoles~" + selectedDepts);
        cbpManagers.PerformCallback(selectedDepts);
        //gvResourceAvailablity.PerformCallback(selectedDepartments);
        showLoader();
        */

        showLoader();
        var cmbDepartment = $("#" + ccID + " span");
        var selectedDepartments = cmbDepartment.attr("id");

        var selectedDepts = "";
        for (i = 0; i < cmbDepartment.length; i++)
            selectedDepts = selectedDepts + cmbDepartment[i].id + ",";
        document.getElementById('<%= hdnaspDepartment.ClientID %>').value = selectedDepts;
        cbpResourceAvailability.PerformCallback("LoadRoles~" + selectedDepts.substring(0, selectedDepts.length - 1));
    }

    function InitiateGridCallback(s, e) {
        if (!s.InCallback())
            gvResourceAvailablity.PerformCallback();

        ResourceAvailabilityloadingPanel.Hide();
    }

	 $(document).ready(function () {
        $('.reourceUti-container').parent().addClass('popup-container');
    });

    function OpenSendEmailWindow(url) {
        //var url = hdnConfiguration.Get("SendEmailUrl");
        //var requestUrl = hdnConfiguration.Get("RequestUrl");
        UgitOpenPopupDialog(url, '', 'Send Email - Query Report', '800px', '600px', 0, escape("<%=Request.Url.AbsolutePath%>"))
        return false;
    }

    function SendMailClick() {
        ResourceAvailabilityloadingPanel.Show();
        cbMailsend.PerformCallback("SendMail");
    }

    function OnCallbackComplete(s, e) {
        ResourceAvailabilityloadingPanel.Hide();
        var result = e.result
        if (result != null && result.length > 0) {
            if (e.parameter.toString() == "SendMail") {
                OpenSendEmailWindow(result);
            }           
        }
        else {
            alert("No record found.");
        }
    }
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        gvResourceAvailablity.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gvResourceAvailablity.SetHeight(containerHeight);
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
    var allocModel = {};
    var UsersData = [];
    var UnfilledAllocations = [];
    var arrDivisions = [];
    var arrRoles = [];
    var SelectedRoles = [];
    var SelectedDivn = [];
    var isSoftAllocation = false;
    var CallOnChange = true;
    var popupFilters = {};
    var allowGroupFilterOnExpTags = "<%=this.AllowGroupFilterOnExpTags%>" == "True" ? true : false;
    var UserConflictData = [];
    var ResourceTagCount = [];
    var experienceTags = [];
    var certifications = [];
    var existingProjectTags = [];
    var projectExperiencTags = [];
    var projectExperiencModel = {};
    projectExperiencModel.ProjectId = "";
    projectExperiencModel.ProjectTags = [];
    var showTimeSheet = false;
    var selectBoxItems = [
        { text: "All Resources", value: 2 },
        { text: "Fully Available", value: 0 },
        { text: "Partially Available", value: 1 }
    ];
    function FindUnfilledAllocationsByRole(roleId, startDate, endDate, includeSoftAllocations) {
        allocModel.UserId = ""; 
        allocModel.UserName = ""; 
        allocModel.RoleId = roleId; 
        allocModel.StartDate = startDate;
        allocModel.EndDate = endDate;
        allocModel.IncludeSoftAllocation = includeSoftAllocations == "0" ? false : true;
        allocModel.Allocations = [];
        $.post("/api/Bench/FindPotentialAllocationsByRole/", allocModel).then(function (response) {
            UnfilledAllocations = response.UnfilledAllocations;
            UsersData = response.UserProfiles;
            arrDivisions = [...new Set(UnfilledAllocations.map(item => item.Division))];
            arrRoles = [...new Set(UnfilledAllocations.map(item => item.TypeName))];
            const popupContentTemplate1 = function () {
                let container = $("<div>");
                let upperSection = $('<div class="row mb-2 mt-2">').append(
                    $('<div class="col-sm-2 col-md-2">').append(
                        $('<div class="searchLabel">Search Week</div>'),
                        $('<div id="lblSearchStart" class="searchTextbox" />').dxDateBox({
                            type: 'date',
                            value: allocModel.StartDate != null ? new Date(allocModel.StartDate).toDateString().slice(4) : '',
                            displayFormat: "MMM dd, yyyy",
                            inputAttr: { 'aria-label': 'Date Time' },
                            onValueChanged: function (e) {
                                allocModel.StartDate = $("#lblSearchStart").dxDateBox('instance').option('value');
                                if ($("#<%=hdndisplayMode.ClientID%>").val() == "Weekly") {
                                    allocModel.EndDate = new Date(allocModel.StartDate).addDays(7).toDateString().slice(4);
                                } else {
                                    allocModel.EndDate = new Date(allocModel.StartDate).addDays(30).toDateString().slice(4);
                                }
                                BindGrid();
                            }
                        })
                    ),
                    $('<div class="col-sm-2 col-md-2">').append(
                        $('<div class="searchLabel">Division</div>'),
                        $('<div id="ddlDivision" class="searchRoundControl" />').dxTagBox({
                            showSelectionControls: true,
                            dataSource: arrDivisions.filter(x => x != null && x != ''),
                            placeholder: 'Division',
                            showClearButton: true,
                            searchEnabled: true,
                            maxDisplayedTags: 1,
                            onValueChanged: function (data) {
                                if (CallOnChange) {
                                    SelectedDivn = data.value;
                                    commonGridFilter();
                                }
                            }
                        })
                    ),
                    $('<div class="col-sm-3 col-md-3">').append(
                        $('<div class="searchLabel">Functional Roles</div>'),
                        $('<div id="tboxAssignedRoles" class="searchRoundControl" />').dxTagBox({
                            showSelectionControls: true,
                            dataSource: arrRoles,
                            applyValueMode: 'instantly',
                            searchEnabled: true,
                            maxDisplayedTags: 1,
                            onValueChanged: function (data) {
                                if (CallOnChange) {
                                    SelectedRoles = data.value;
                                    commonGridFilter();
                                }
                            }
                        })
                    ),
                    $('<div class="col-sm-3 col-md-offset-3 col-md-2">').append(
                        $('<div id="chkIncludeSoft" class="mt-4" style="float:right;" />').dxCheckBox({
                            value: allocModel.IncludeSoftAllocation,
                            text: "Include Soft Allocations",
                            onValueChanged: function (e) {
                                allocModel.IncludeSoftAllocation = e.value;
                                BindGrid();
                            }
                        })
                    )
                );
                let prevTitle = '';
                let prevCmic = '';
                let prevRole = '';
                let gridHeight = parseInt(65 * $(window).height() / 100);
                let datagrid = $('<div class="row mt-2">').append(
                    $('<div class="col-sm-12 col-md-12">').append(
                        $(`<div id='UnfilledAllocationsGrid' style='max-height:${gridHeight}px;'>`).dxDataGrid({
                            dataSource: UnfilledAllocations,
                            ID: "grdTemplate",
                            showBorders: false,
                            showRowLines: false,
                            showColumnLines: false,
                            editing: {
                                mode: "cell",
                                allowEditing: true,
                                allowUpdating: true
                            },
                            sorting: {
                                mode: "multiple" // or "multiple" | "none"
                            },
                            scrolling: {
                                mode: 'Standard',
                            },
                            paging: { enabled: false },
                            columns: [
                                {
                                    dataField: 'ModuleName',
                                    sortIndex: 0,
                                    sortOrder: 'asc',
                                    visible: false
                                },
                                {
                                    dataField: 'RoleId',
                                    visible: false
                                },
                                {
                                    dataField: 'AssignedTo',
                                    visible: false
                                },
                                {
                                    dataField: 'Title',
                                    sortIndex: 1,
                                    sortOrder: 'asc',
                                    alignment: 'center',
                                    allowEditing: false,
                                    cellTemplate: function (container, options) {
                                        if (options.data.Title != "") {
                                            let className = "title-opm";
                                            if (options.data.ModuleName == "CPR")
                                                className = "title-cpr";
                                            if (options.data.HasDuplicateRecord == "0") {
                                                let ticketUrl = `openTicketDialog('${options.data.StaticModulePagePath}','TicketId=${options.data.ProjectId}','${options.data.ERPJobID}: ${options.data.Title}','95','95', 0, '')`
                                                $(`<button class='${className}' title='${options.data.Title}' onclick = \"${ticketUrl}\" >${truncateString(options.data.Title, 45)}</button>`).appendTo(container);
                                            }
                                            else {
                                                $(`<button class='${className}' style='display:none' onclick = \"${options.data.ProjectLink}\" >${options.data.Title}</button>`).appendTo(container);
                                            }
                                        }


                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    caption: 'CMIC#',
                                    dataField: "ERPJobID",
                                    alignment: 'center',
                                    width: "8%",
                                    allowEditing: false,
                                    cellTemplate: function (container, options) {
                                            if (options.data.ERPJobID != null && options.data.ERPJobID != ""
                                                && options.data.HasDuplicateRecord == "0") {
                                            $("<div class='grdCell'>" + options.data.ERPJobID + "</div>").appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    caption: 'Role',
                                    dataField: 'TypeName',
                                    width: "18%",
                                    sortIndex: 1,
                                    sortOrder: 'asc',
                                    alignment: 'center',
                                    allowEditing: false,
                                    cellTemplate: function (container, options) {
                                        if (options.data.TypeName != "" && options.data.HasDuplicateRecord == "0") {
                                            $("<div class='grdCell'>" + options.data.TypeName + "</div>").appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    dataField: "AssignedToName",
                                    dataType: "text",
                                    width: "22%",
                                    caption: "Assigned To",
                                    cellTemplate: function (container, options) {
                                        $('.dx-header-row').addClass('devExtDataGrid-headerRow');
                                        $('.dx-data-row').addClass('devExtDataGrid-DataRow');
                                        if (options.key.ID > 0) {
                                            var str = options.data.AssignedTo + "','" + options.data.AssignedToName.replace("'", "`");
                                            var strwithspace = str.replace(/ /g, "&nbsp;")
                                            $("<div id='dataId' class='dataIdcell' >")
                                                .append("<span style='float: left;overflow: auto;'>" + (options.data.UserImageUrl != null && options.data.UserImageUrl != "" ? "<img src=" + options.data.UserImageUrl + " class='profileUserImg' />" + '  ' : '') + "<a href='javascript:void(0);' onclick=openResourceTimeSheet('" + strwithspace + "');>"
                                                    + (options.data.IsResourceDisabled ? "<span style='color:red;'>" + options.value + "</span>" : options.value) + "</a></span>")
                                                .append($("<img>", { "src": "/content/images/moreoptions_blue.png", "ID": options.data.ID, "group": options.data.Type, "startDate": options.data.AllocationStartDate, "endDate": options.data.AllocationEndDate, "assignedTo": options.data.AssignedToName, "style": "float: right;overflow: auto;cursor: pointer;", "class": "assigneeToImg" }))
                                                .appendTo(container);
                                        }
                                    }
                                },
                                {
                                    caption: 'Start Date',
                                    dataField: 'AllocationStartDate',
                                    width: "9%",
                                    alignment: 'center',
                                    allowEditing:false,
                                    cellTemplate: function (container, options) {
                                        if (options.data.AllocationStartDate != "") {
                                            let bkColorcss = getDateBackgroundColor(new Date(options.data.AllocationStartDate), options.data.PreconStartDate, options.data.PreconEndDate,
                                                options.data.EstimatedConstructionStart, options.data.EstimatedConstructionEnd, options.data.CloseoutStartDate, options.data.CloseoutDate);

                                            $("<div class='dateConstClass " + bkColorcss + "'>" + new Date(options.data.AllocationStartDate).format('MMM dd, yyyy') + "</div>").appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    caption: 'End Date',
                                    dataField: 'AllocationEndDate',
                                    width: "9%",
                                    alignment: 'center',
                                    allowEditing: false,
                                    cellTemplate: function (container, options) {
                                        if (options.data.AllocationEndDate != "") {
                                            let bkColorCSS = getDateBackgroundColor(new Date(options.data.AllocationEndDate), options.data.PreconStartDate, options.data.PreconEndDate,
                                                options.data.EstimatedConstructionStart, options.data.EstimatedConstructionEnd, options.data.CloseoutStartDate, options.data.CloseoutDate);

                                            $("<div class='dateConstClass " + bkColorCSS + "'>" + new Date(options.data.AllocationEndDate).format('MMM dd, yyyy') + "</div>").appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    caption: 'Len(Wks)',
                                    dataField: 'Length',
                                    alignment: 'center',
                                    width: "5%",
                                    cellTemplate: function (container, options) {
                                        if (options.data.Length == "0") {
                                            $("<div class='grdCell'>1</div>").appendTo(container);
                                        }
                                        else if (options.data.Length != "") {
                                            $("<div class='grdCell'>" + options.data.Length + "</div>").appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                                {
                                    caption: 'Alloc%',
                                    dataField: "PctAllocation",
                                    alignment: 'center',
                                    width: "5%",
                                    cellTemplate: function (container, options) {
                                        if (options.data.PctAllocation != "") {
                                            $("<div class='grdCell'>" + options.data.PctAllocation + "</div>").appendTo(container);
                                        }
                                    },
                                    headerCellTemplate: function (header, info) {
                                        $(`<div class="grdHeader">${info.column.caption}</div>`).appendTo(header);
                                    }
                                },
                            ],
                            showBorders: true,
                            showRowLines: true,
                            onEditorPreparing: function (e) {
                               
                                if (e.parentType === 'dataRow' && e.dataField === 'AssignedToName') {
                                    var dataGrid = $("#UnfilledAllocationsGrid").dxDataGrid("instance");
                                    let rType = dataGrid.getDataSource()._items[e.row.rowIndex]?.RoleId;
                                    let uData = rType != '' && rType != null && !rType.startsWith("TYPE-") ? UsersData.filter(x => x.GlobalRoleId == rType && x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1)
                                        : UsersData.filter(x => x.Enabled == true).sort((a, b) => (a.Name > b.Name) ? 1 : -1);

                                    e.editorElement.dxSelectBox({
                                        dataSource: uData,
                                        valueExpr: "Id",
                                        displayExpr: "Name",
                                        value: e.row.data.AssignedTo,
                                        searchEnabled: true,
                                        onValueChanged: function (ea) {
                                            $.each(ea.component._dataSource._items, function (i, v) {
                                                if (v.Id === ea.value) {
                                                    e.setValue(v.Name);
                                                    dataGrid.getDataSource()._items[e.row.rowIndex].AssignedTo = v.Id;
                                                }
                                            });
                                        }
                                    });
                                    e.cancel = true;
                                } 
                            },
                            onCellPrepared: function (e) {
                                if (e.rowType === 'data') {
                                    var preconstartDate = new Date(e.PreconStartDate);
                                    var preconEndDate = new Date(e.PreconEndDate);

                                    var conststartDate = new Date(e.EstimatedConstructionStart);
                                    var constEndDate = new Date(e.EstimatedConstructionEnd);

                                    var closeoutstartDate = new Date(e.CloseoutStartDate);
                                    var closeoutEndDate = new Date(e.CloseoutDate);

                                    if (e.column.dataField == 'AllocationStartDate') {
                                        let cellValue = new Date(e.data.AllocationStartDate)
                                        let className = getDateBackgroundColor(cellValue, preconstartDate, preconEndDate, conststartDate, constEndDate, closeoutstartDate, closeoutEndDate);
                                        e.cellElement.addClass(className);
                                    }
                                    if (e.column.dataField == 'AllocationEndDate') {
                                        let cellValue = new Date(e.data.AllocationEndDate)
                                        let className = getDateBackgroundColor(cellValue, preconstartDate, preconEndDate, conststartDate, constEndDate, closeoutstartDate, closeoutEndDate);
                                        e.cellElement.addClass(className);
                                    }
                                }
                            }
                        })
                    )
                );
                let bottomSection = $('<div class="row mt-2">').append(
                    $('<div class="col-sm-12 col-md-12">').append(
                        $(`<div class="btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                            text: "Save",
                            hint: 'Save Allocations',
                            visible: true,
                            onClick: function (e) {
                                var dataGrid = $('#UnfilledAllocationsGrid').dxDataGrid("instance");
                                var Selectedrows = dataGrid.getDataSource()._items.filter(x => x.AssignedTo != null && x.AssignedTo != '');
                                if (Selectedrows.length <= 0) {
                                    DevExpress.ui.dialog.alert(`No Data to Save`, 'Error');
                                    return false;
                                }
                                let ChangedAllocations = [];
                                var curDate = new Date();
                                var isStartDateGreater = false;
                                Selectedrows.forEach(function (row, index) {
                                    let newRow = JSON.parse(JSON.stringify(row));
                                    if (new Date(row.AllocationStartDate) < curDate && new Date(row.AllocationEndDate) >= curDate) {
                                        isStartDateGreater = true;
                                        newRow.AllocationStartDate = new Date(curDate).format('yyyy-MM-dd') + "T00:00:00";
                                    }
                                    ChangedAllocations.push(newRow);
                                });
                                if (isStartDateGreater) {
                                    var result = DevExpress.ui.dialog.custom({
                                        title: "Warning!",
                                        message: "RM One will adjust the start date of this allocation to Current Date",
                                        buttons: [
                                            { text: "Retain as Start Date", onClick: function () { return "Yes" } },
                                            { text: "Change Date to " + new Date(curDate).toLocaleDateString("en-US"), onClick: function () { return "No" } }
                                        ]
                                    });
                                    result.show().done(function (dialogResult) {
                                        if (dialogResult == "Yes") {
                                            SaveUnfilledAllocations(Selectedrows, false);
                                        }
                                        if (dialogResult == "No") {
                                            SaveUnfilledAllocations(ChangedAllocations, true);
                                        }
                                    });
                                } else {
                                    SaveUnfilledAllocations(Selectedrows, false);
                                }
                                //popup.hide();
                            }
                        }),
                        $(`<div class="btnAddNew" style='float:right;padding: 0px 10px;font-size: 14px;' />`).dxButton({
                            text: "Cancel",
                            visible: true,
                            onClick: function (e) {
                                popup.hide();
                            }
                        })
                    )
                );
                
                container.append(upperSection);
                container.append(datagrid);
                container.append(bottomSection);
                return container;
            };

            const popup = $("#UnfilledAllocationsGridDialog").dxPopup({
                contentTemplate: popupContentTemplate1,
                width: "85%",
                height: "auto",
                showTitle: true,
                title: "Unfilled Roles",
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                },
                onHiding: function () {
                    refreshGridDataUsingCallback();
                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupContentTemplate1()

            });
            popup.show();
        });
    }
    function BindGrid() {
        $.post("/api/Bench/FindPotentialAllocationsByRole/", allocModel).then(function (response) {
            UnfilledAllocations = response.UnfilledAllocations;
            arrDivisions = [...new Set(UnfilledAllocations.map(item => item.Division))];
            arrRoles = [...new Set(UnfilledAllocations.map(item => item.TypeName))];
            CallOnChange = false;
            $('#ddlDivision').dxTagBox('option', 'dataSource', arrDivisions);
            if (SelectedDivn?.length > 0)
                $('#ddlDivision').dxTagBox('option', 'value', SelectedDivn.filter(x => arrDivisions.includes(x)));
            $('#tboxAssignedRoles').dxTagBox('option', 'dataSource', arrRoles);
            if (SelectedRoles?.length > 0)
                $('#tboxAssignedRoles').dxTagBox('option', 'value', SelectedRoles.filter(x => arrRoles.includes(x)));
            CallOnChange = true;
            commonGridFilter();
        });
    }
    function commonGridFilter()
    {
        var grid = $("#UnfilledAllocationsGrid").dxDataGrid('instance');
        var data = JSON.parse(JSON.stringify(UnfilledAllocations));
        
        if (data != undefined && SelectedRoles != null && SelectedRoles.length > 0) {
            data = data.filter(alloc => SelectedRoles.includes(alloc.TypeName));
        }
        if (data != undefined && SelectedDivn != null && SelectedDivn.length > 0) {
            data = data.filter(alloc => SelectedDivn.includes(alloc.Division));
        }

        grid.option("dataSource", data);
    }

    function getDateBackgroundColor(cellValue, preconstartDate, preconEndDate, conststartDate, constEndDate, closeoutstartDate, closeoutEndDate) {
        preconstartDate = new Date(preconstartDate);
        preconEndDate = new Date(preconEndDate);
        conststartDate = new Date(conststartDate);
        constEndDate = new Date(constEndDate);
        closeoutstartDate = new Date(closeoutstartDate);
        closeoutEndDate = new Date(closeoutEndDate);
        if (isDateValid(closeoutstartDate) && isDateValid(closeoutEndDate)
            && cellValue >= closeoutstartDate && cellValue <= closeoutEndDate) {
            return 'closeoutDateStyle';
        }
        else if (isDateValid(conststartDate) && isDateValid(constEndDate) &&
            cellValue <= constEndDate && cellValue >= conststartDate) {
            return 'constDateStyle';
        }
        else if (isDateValid(preconstartDate) && isDateValid(preconEndDate)
            && cellValue >= preconstartDate && cellValue <= preconEndDate) {
            return 'preconDateStyle';
        }
        else
            return 'otherDateStyle';
    }
    function SaveUnfilledAllocations(Selectedrows, dialogResult) {
        var dataGrid = $("#UnfilledAllocationsGrid").dxDataGrid('instance');
        ResourceAvailabilityloadingPanel.Show();
        Selectedrows.forEach(function (row, index) {
            var list = {};
            list.Allocations = [];
            list.PreConStart = row.PreconStartDate;
            list.PreConEnd = row.PreconEndDate;
            list.ConstStart = row.EstimatedConstructionStart;
            list.ConstEnd = row.EstimatedConstructionEnd;
            list.ProjectID = row.ProjectId;
            list.CalledFrom = "Capacity Report > Potential Allocation > Save";
            var objAllocations = {};
            objAllocations.AllocationStartDate = row.AllocationStartDate;
            objAllocations.AllocationEndDate = row.AllocationEndDate;
            objAllocations.AssignedTo = row.AssignedTo;
            objAllocations.Type = row.RoleId;
            objAllocations.PctAllocation = row.PctAllocation;
            objAllocations.ProjectID = row.ProjectId;
            objAllocations.Title = row.Title;
            objAllocations.ID = row.ProjectEstimatedAllocationId;
            objAllocations.TypeName = row.TypeName;
            objAllocations.SoftAllocation = row.SoftAllocation;
            objAllocations.NonChargeable = row.NonChargeable;
            objAllocations.IsLocked = row.IsLocked;
            objAllocations.isChangeStartDate = dialogResult
            list.Allocations[0] = objAllocations;
            let SavedAllocations = [];
            $.post("/api/rmmapi/UpdateCRMAllocation/", list).then(function (response) {
                if (response.includes("OverlappingAllocation")) {
                    var resultvalues = response.split(":");
                    var datakey = _.findWhere(UnfilledAllocations, { ProjectEstimatedAllocationId: resultvalues[1] });
                    var rowIndex = dataGrid.getRowIndexByKey(datakey);
                    DevExpress.ui.dialog.alert("Overlapping allocations are not permitted. Save unsuccessful.", "Error");
                    dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                    ResourceAvailabilityloadingPanel.Hide();
                    return false;
                }
                else if (response.includes("AllocationOutofbounds")) {
                    var resultvalues = response.split("~");
                    var datakey = _.findWhere(UnfilledAllocations, { ProjectEstimatedAllocationId: resultvalues[1] });
                    var rowIndex = dataGrid.getRowIndexByKey(datakey);
                    DevExpress.ui.dialog.alert("Allocation date entered is either prior to start date or after the end date of the resource. <br/>Name: " + resultvalues[4] + " <br/>Start Date: " + resultvalues[2] + " End Date: " + resultvalues[3] + ". <br/>Please enter valid dates.", "Error");
                    dataGrid.getRowElement(rowIndex).css("background-color", "#FFCCCB");
                    ResourceAvailabilityloadingPanel.Hide();
                    return false;
                }
                else {
                    SavedAllocations.push(row.ID);
                    if (index == Selectedrows.length - 1) {
                        ResourceAvailabilityloadingPanel.Hide();
                        DevExpress.ui.notify('Allocation Saved Successfully.', "success");
                    }
                }
                UnfilledAllocations = UnfilledAllocations.filter(x => !SavedAllocations.includes(x.ID));
                dataGrid.option('dataSource', UnfilledAllocations);
            },
            function (error) {
                ResourceAvailabilityloadingPanel.Hide();
            });
        });
    }
    function OpenResumeCompare(Type, ModuleName) {
        let checkedUser = [];
        let additionalData = [];
        let typeName;
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
                typeName = $(this).attr("name");
                additionalData.push({ Id: $(this).attr("id"), pctAllcation: $(this).attr("pctallocation"), allocaionView: $(this).attr("allocaionview") });
            }
        });

        $.cookie("additionalData", '');
        if (checkedUser.length == 0) {
            DevExpress.ui.dialog.alert("Select at least one user.", "Warning!");
            return false;
        }
        $.cookie("additionalData", JSON.stringify(additionalData));
        let url = '<%=UGITUtility.GetAbsoluteURL("/Layouts/uGovernIT/DelegateControl.aspx?control=compareuserprofile")%>' + '&SelectedUsers=' + checkedUser.join(';');

            if (popupFilters.Customer) {
                url += "&companyname=" + popupFilters.Customer;
        }
        if (popupFilters.RequestTypes) {
            url += "&requesttype=" + popupFilters.RequestTypes;
        }
        if (popupFilters.complexity) {
            url += "&projectcomplexity=" + popupFilters.complexity;
        }
        url += "&modulename=" + ModuleName;
        window.parent.UgitOpenPopupDialog(url, '', 'Resume Comparison: ' + Type, '95', '95', "", false);
   
    }
    function OpenUsersGanttView(ganttProjSD, ganttProjED, classNameSt, classNameEd, ganttProjReqAloc) {
        let checkedUser = [];
        let typeName;
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
                typeName = $(this).attr("name");
            }
        });
        if (checkedUser.length == 0) {
            DevExpress.ui.dialog.alert("Select at least one user.", "Warning!");
            return false;
        }
        if (ganttProjSD != undefined && ganttProjED != undefined && classNameSt != undefined && classNameEd != undefined && ganttProjReqAloc != undefined && ganttProjSD != null && ganttProjED != null && classNameSt != null && classNameEd != null && ganttProjReqAloc != null) {
            var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&RequestFromProjectAllocation=true&SelectedUsers=" + checkedUser.join(',') + "&classNameSt=" + classNameSt + "&classNameEd=" + classNameEd + "&ganttProjSD=" + ganttProjSD + "&ganttProjED=" + ganttProjED + "&ganttProjReqAloc=" + ganttProjReqAloc;
            window.parent.UgitOpenPopupDialog(url, "", "Timeline for User", "95", "95", "", false);
        }
        else {
            var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&RequestFromProjectAllocation=true&SelectedUsers=" + checkedUser.join(',') + "&ShowDateInfo=false";
            window.parent.UgitOpenPopupDialog(url, "", "Timeline for User", "95", "95", "", false);
        }
    }
    $(document).on("click", "img.assigneeToImg", function (e) {
        let id = $(this).attr("ID");
        let data = UnfilledAllocations.filter(x => x.ID == id)[0];
        var groupid = data.RoleId;
        var dataid = data.ID;
        popupFilters.projectID = data.ProjectId;
        popupFilters.resourceAvailability = 2;
        popupFilters.complexity = false;
        popupFilters.projectVolume = false;
        popupFilters.projectCount = false;
        popupFilters.RequestTypes = false;
        popupFilters.Customer = false;
        popupFilters.Sector = false;
        popupFilters.groupID = groupid;
        popupFilters.allocationStartDate = new Date(data.AllocationStartDate);
        popupFilters.allocationEndDate = new Date(data.AllocationEndDate);
        popupFilters.pctAllocation = data.PctAllocation;
        popupFilters.isAllocationView = 1;
        popupFilters.ModuleIncludes = false;
        popupFilters.JobTitles = "";
        popupFilters.departments = "";
        popupFilters.DivisionId = "";
        popupFilters.SelectedUserID = "";
        popupFilters.ID = dataid;
        popupFilters.Allocations = [];
        popupFilters.Allocations.push({ StartDate: popupFilters.allocationStartDate.format('yyyy-MM-dd') + "T00:00:00", EndDate: popupFilters.allocationEndDate.format('yyyy-MM-dd') + "T00:00:00", RequiredPctAllocation: parseFloat(popupFilters.pctAllocation), ID: dataid });
        GetProjectExperienceTagList(data.TagMultiLookup, data.ProjectId);
        var popupTitle = "Available Resource";
        if (data)
            //popupTitle = "Allocated " + data.TypeName + "s";
            popupTitle = "Allocate " + data.TypeName;

        $("#popupContainer").dxPopup({
            title: popupTitle,
            width: "98%",
            height: "100%",
            visible: true,
            scrolling: true,
            dragEnabled: true,
            resizeEnabled: true,
            position: { my: "left top", at: "left top", of: window },
            contentTemplate: function (contentElement) {
                let preconstartDate = new Date(data.PreconStartDate);
                let preconendDate = new Date(data.PreconEndDate);
                let conststartDate = new Date(data.EstimatedConstructionStart);
                let constEndDate = new Date(data.EstimatedConstructionEnd);

                let closeoutstartDate = new Date(data.CloseoutStartDate);
                let closeoutendDate = new Date(data.CloseoutDate);
                let windowHeight = parseInt(60 * $(window).height() / 100);
                let classNameSt = null;
                let classNameEd = null;
                let ganttProjSD = null;
                let ganttProjED = null;
                let ganttProjReqAloc = null;
                if ($(window).height() < 700) {
                    windowHeight = parseInt(45 * $(window).height() / 100);
                }
                contentElement.append(
                    $("<div id='popuploader' />").dxLoadPanel({
                        message: "Loading...",
                        visible: true
                    }),
                    $("<div class='d-flex row px-3 shadow-effect mb-3' />").append(
                        $("<div class='col-md-7 col-sm-6 pl-1 pr-0 d-flex outer-border-date-box justify-content-between align-items-center'>").append(
                            $(`<a class="pr-1" id="myProjectLeftIcon" style="visibility:hidden;" href="#" onclick="moveLeft()"><i class="glyphicon glyphicon-chevron-left" style="color:black;"></i></a>`),
                            $(`<div id="divProjectView" style='width:400px' class="overflow-hidden overflow"></div>`).dxTileView({
                                items: popupFilters.Allocations,
                                height: '65px',
                                width: function () {
                                    return window.innerWidth / 2.1;
                                },
                                showScrollbar: true,
                                itemTemplate: function (itemData, itemIndex, itemElement) {
                                    classNameSt = getDateBackgroundColor(new Date(itemData.StartDate), preconstartDate, preconendDate, conststartDate, constEndDate, closeoutstartDate, closeoutendDate);
                                    classNameEd = getDateBackgroundColor(new Date(itemData.EndDate), preconstartDate, preconendDate, conststartDate, constEndDate, closeoutstartDate, closeoutendDate);
                                    ganttProjSD = new Date(itemData.StartDate).format('MMM d, yyyy');
                                    ganttProjED = new Date(itemData.EndDate).format('MMM d, yyyy');
                                    ganttProjReqAloc = itemData.RequiredPctAllocation;

                                    itemElement.append(
                                        $(`<div class='paneldiv tooltipp'>`).append(
                                            $(`<div class='d-flex'>`).append(
                                                $(`<div class='date-box ${classNameSt}'>${new Date(itemData.StartDate).format('MMM d, yyyy')}</div>`),
                                                $(`<div class='date-box ${classNameEd}'>${new Date(itemData.EndDate).format('MMM d, yyyy')}</div>`),
                                            ),
                                            $(`<div class='date-box-1'>${itemData.RequiredPctAllocation}%</div>`),
                                            $(`<span class='tooltiptext' style="display:none !important"></span> `)
                                        )
                                    )
                                },
                                //onItemRendered: function (e) {
                                //    setTimeout(checkScrollEnd, 1500);
                                //},
                            }),
                            $(`<a id="myProjectRightIcon" style="visibility:hidden;" class="pl-1 pr-1" href="#" onclick="moveRight()"><i class="glyphicon glyphicon-chevron-right" style="color:black;"></i></a>`)
                        ),
                        $("<div class='col-md-5 col-sm-6 pl-1 pr-0 d-flex justify-content-between outer-border-date-box'>").append(
                            $("<div class='col-md-9 noPadding d-flex align-items-end justify-content-around'>").append(
                                $("<div>").append(
                                    $("<label class='availibility-2'>% View</label>"),
                                    $(`<div class="tag-container-1 availibility-1 font-size-class-1"><div class="mr-2">Availability</div><label class="switch"><input id='AvailabilityChk' type="checkbox" onclick='ChangeAvailability(this, "${data.TypeName}");' checked><span class="slider round"></span></label>
                    <div class="ml-2">Allocation</div></div>`)
                                ),
                                $("<div>").append(
                                    $("<label class='availibility-4'>Project Experience</label>"),
                                    $(`<div class="tag-container-1 availibility-3 font-size-class-1"><div class="mr-2">Show</div><label class="switch"><input id='ProjectExperienceChk' onclick='ChangeProjectExperience();' type="checkbox" checked><span class="slider round"></span></label>
                    <div class="ml-2">Hide</div></div>`)
                                )
                            ),
                            $("<div class='col-md-3 noPadding mr-1 d-flex justify-content-end'>").append(
                                $("<div id='compareTags' class='pt-3 mt-1' />").append(
                                    $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                        icon: '/content/Images/RMONE/comparetags.png',
                                        hint: 'Open Tag Matrix',
                                        onClick() {
                                            OpenExperienceTagPopup();
                                        },
                                    })
                                ),
                                $("<div id='compareResume' class='pt-3 mt-1' />").append(
                                    $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                        icon: '/content/Images/RMONE/compareresume.png',
                                        hint: 'Compare Resume',
                                        onClick() {
                                            OpenResumeCompare(data.TypeName, data.ModuleName);
                                        },
                                    })
                                ),
                                $("<div id='openGanttView' class='pt-3 mt-1' />").append(
                                    $("<div style='border:none;-webkit-box-shadow: none'>").dxButton({
                                        icon: '/content/Images/ganttBlackNew.png',
                                        hint: 'Open Gantt View',
                                        onClick() {
                                            OpenUsersGanttView(ganttProjSD, ganttProjED, classNameSt, classNameEd, ganttProjReqAloc);
                                        },
                                    })
                                )
                            )
                        )
                    ),
                    $("<div class='shadow-effect' style='padding-bottom: 10px;' />").append(
                        $("<div id='filterChecks' class='clearfix pb-2 pt-2' />").append(
                            $("<Label class='clsSmartFilter' >Suggested Filters:</Label>"),
                            $("<div id='chkComplexity' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: "Complexity " + data.CRMProjectComplexityChoice + "+",
                                visible: (data.ModuleName == "OPM" || data.ModuleName == "CPR" || data.ModuleName == "CNS") == true && data.CRMProjectComplexityChoice != null && data.CRMProjectComplexityChoice != '' ? true : false,
                                value: popupFilters.complexity,
                                onValueChanged: function (e) {
                                    popupFilters.complexity = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),

                            $("<div id='chkRequestType' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                /*text: "Project Type",*/
                                text: data.RequestType,
                                hint: 'Project Type',
                                visible: (data.RequestType != '' && data.RequestType != null) == true ? true : false,
                                onValueChanged: function (e) {
                                    popupFilters.RequestTypes = e.value;
                                    bindDatapopup(popupFilters);
                                }
                            }),

                            $("<div id='chkCustomer' title='Customer' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: data.CompanyTitle,
                                visible: (data.CompanyTitle != '' && data.CompanyTitle != null) == true ? true : false,
                                value: popupFilters.Customer,
                                hint: 'Customer',
                                onValueChanged: function (e) {
                                    popupFilters.CompanyLookup = data.CRMCompanyLookup;
                                    popupFilters.Customer = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),

                            $("<div id='chkSector' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: data.SectorChoice,
                                visible: (data.SectorChoice != '' && data.SectorChoice != null) == true ? true : false,
                                hint: 'Sector',
                                value: popupFilters.Sector,
                                onValueChanged: function (e) {
                                    popupFilters.SectorName = data.SectorChoice;
                                    popupFilters.Sector = e.value;
                                    bindDatapopup(popupFilters);
                                },
                            }),

                            $("<div id='chkCertification' class='chkFilterCheck pl-3' style='float:left;' />").dxCheckBox({
                                text: "Certification",
                                visible: true,
                                hint: 'Certification',
                                onValueChanged: function (e) {
                                    if (e.value) {
                                        e.component.option("text", "");
                                        $("#certificationTxt").dxTagBox("option", "visible", true);
                                    }
                                    else {
                                        e.component.option("text", "Certification");
                                        $("#certificationTxt").dxTagBox("option", "visible", false);
                                        $("#certificationTxt").dxTagBox("option", "value", []);
                                    }
                                },
                            }),
                            $("<div id='certificationTxt' class='filterctrl-jobDepartment' style='margin-top:-6px;padding-left:5px;' />").dxTagBox({  //dxSelectBox
                                showSelectionControls: true,
                                placeholder: "Certification",
                                valueExpr: "ID",
                                grouped: true,
                                visible: false,
                                displayExpr: "Title",
                                dataSource: new DevExpress.data.DataSource({
                                    store: certifications,
                                    key: 'ID',
                                    group: 'CategoryName',
                                }),
                                maxDisplayedTags: 1,
                                searchEnabled: true,
                                onValueChanged: function (selectedItems) {
                                    popupFilters.SelectedCertifications = selectedItems.value.join();
                                    bindDatapopup(popupFilters);
                                },
                            }),
                        ),

                        $("<div class='clearfix pt-1' />").append(
                            $("<Label class='clsSmartFilter pr-4' style='margin-top:10px;' >Tags:</Label>"),
                            $('<div class="tag-container mr-3" style="border: 2px solid #ddd;min-height:45px;">').append(
                                $('<div class="tag-container-2 mr-3">').append(
                                    $("<div id='addExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                        icon: "/Content/Images/plus-blue-new.png",
                                        hint: "Reset Experience Tags",
                                        visible: allowGroupFilterOnExpTags,
                                        focusStateEnabled: false,
                                        onClick() {
                                            popupFilters.SelectedTags = JSON.parse(JSON.stringify(projectExperiencTags));
                                            RenderProjectTagsOnFrame();
                                        }
                                    }),
                                    $('<div id="projectExpTags" style="display:flex;flex-wrap:wrap;" />'),
                                ),
                                $("<div id='clearExpTags' style='border: none; -webkit-box-shadow: none' />").dxButton({
                                    icon: "/Content/Images/RMONE/clear-icon.png",
                                    hint: "Clear Experience Tags",
                                    visible: allowGroupFilterOnExpTags,
                                    onClick: function () {
                                        popupFilters.SelectedTags = [];
                                        RenderProjectTagsOnFrame();
                                    }
                                }),
                            ),

                        ),
                        $("<div class='clearfix pb-1' />").append(
                            $("<Label class='clsSmartFilter' style='margin-top:10px;' >Filters:</Label>"),
                            $("<div id='dropdownFilters' class='d-flex justify-content-between' />").append(
                                $("<div class='flex-grow-1 display-flex' />").append(

                                    $("<div class='filterctrl-jobDepartment' />").dxTagBox({  //dxSelectBox
                                        showSelectionControls: true,
                                        placeholder: "Division",
                                        valueExpr: "ID",
                                        displayExpr: "Title",
                                        dataSource: "/api/CoreRMM/GetDivisions?OnlyEnabled=1",
                                        maxDisplayedTags: 1,
                                        searchEnabled: true,
                                        onValueChanged: function (selectedItems) {
                                            //var items = selectedItems.addedItems[0].ID;
                                            let divisionId = '0';
                                            var items = selectedItems.component._selectedItems;
                                            if (items.length > 0) {
                                                var lstItems = items.map(function (i) {
                                                    return i.ID;
                                                });
                                                divisionId = lstItems.join(',');
                                            }
                                            popupFilters.DivisionId = divisionId;
                                            //popupFilters.departments = items;
                                            $.get("/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=" + divisionId, function (data, status) {

                                                JobTitleData = data;
                                                var tagBox = $("#tagboxTitles").dxTagBox("instance");
                                                tagBox.option("dataSource", JobTitleData);
                                                tagBox.reset();
                                            });
                                            bindDatapopup(popupFilters);
                                        },
                                    }),
                                    $("<div id='tagboxTitles' class='filterctrl-jobtitle' />").dxTagBox({
                                        showSelectionControls: true,
                                        text: "Job Titles",
                                        placeholder: "Title",
                                        searchEnabled: true,
                                        dataSource: "/api/rmmapi/GetGroupTitles?GroupID=" + popupFilters.groupID + "&DivisionID=0",
                                        maxDisplayedTags: 1,
                                        onValueChanged: function (selectedItems) {
                                            var items = selectedItems.component._selectedItems;
                                            if (items.length > 0) {
                                                var lstItems = items.map(function (i) {
                                                    return i;
                                                });
                                                popupFilters.JobTitles = lstItems.join(';#');
                                            }
                                            else {
                                                popupFilters.JobTitles = '';
                                            }
                                            bindDatapopup(popupFilters);
                                        }
                                    }),

                                    $("<div class='filterctrl-jobDepartment' />").dxSelectBox({
                                        dataSource: selectBoxItems,
                                        displayExpr: "text",
                                        value: _.findWhere(selectBoxItems, { value: popupFilters.resourceAvailability }),
                                        layout: "horizontal",
                                        onValueChanged: function (e) {
                                            popupFilters.resourceAvailability = e.value.value;
                                            bindDatapopup(popupFilters);
                                        }
                                    }),

                                    $("<div class='filterctrl-userpicker' />").dxSelectBox({
                                        placeholder: "Search team member",
                                        valueExpr: "Id",
                                        displayExpr: "Name",
                                        searchEnabled: true,
                                        showClearButton: true,
                                        dataSource: "/api/rmmapi/GetUserList",
                                        onSelectionChanged: function (selectedItems) {
                                            if (selectedItems.selectedItem == null) {
                                                popupFilters.SelectedUserID = "";
                                                popupFilters.complexity = false;
                                                //var checkcomplexity = $("#chkComplexity").dxCheckBox("instance");
                                                //checkcomplexity.option("value", true);
                                                bindDatapopup(popupFilters);
                                            } else {
                                                var items = selectedItems.selectedItem.Id;
                                                popupFilters.SelectedUserID = items;
                                                popupFilters.projectID = data.ProjectId;
                                                popupFilters.complexity = false;
                                                bindDatapopup(popupFilters);
                                            }
                                        }
                                    }),
                                ),
                            )
                        )
                    ),
                    $(`<div id='tileViewContainer' style='clear:both;padding-bottom:10px;max-height:${windowHeight}px;' />`).dxTileView({
                        width: window.innerWidth - 100, //150
                        showScrollbar: true,
                        baseItemHeight: 65,
                        baseItemWidth: 150,
                        itemMargin: 15,
                        direction: "vertical",
                        elementAttr: { "class": "tileViewContainer" },
                        noDataText: "No resource available",
                        itemTemplate: function (itemData, itemIndex, itemElement) {
                            if (UserConflictData.filter(x => x.AssignedTo == itemData.AssignedTo).length == 0)
                                UserConflictData.push(itemData);
                            itemData.PctAllocation = Math.round(itemData.PctAllocation);
                            itemData.SoftPctAllocation = Math.round(itemData.SoftPctAllocation);
                            itemData.TotalPctAllocation = Math.round(itemData.TotalPctAllocation);
                            var html = new Array();
                            var str = itemData.AssignedTo + "','" + itemData.AssignedToName;
                            if (itemData.ResourceTags != null) {
                                itemData.ResourceTags.forEach(function (value, index) {
                                    if (CheckTagExist(value.Type, value.TagId)) {
                                        let expTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                                        if (ResourceTagCount.length == 0 || ResourceTagCount.filter(x => x.TagId == value.TagId && x.Type == value.Type && x.UserId == itemData.AssignedTo).length == 0) {
                                            ResourceTagCount.push({
                                                TagId: value.TagId,
                                                Title: expTag.Title,
                                                TagCount: value.TagCount,
                                                Type: value.Type,
                                                UserId: itemData.AssignedTo,
                                                UserName: itemData.AssignedToName,
                                                Allocation: itemData.PctAllocation,
                                                UserPicture: itemData.UserImagePath,
                                                RoleName: itemData.RoleName,
                                                Availability: (100 - Number(itemData.PctAllocation)),
                                            });
                                        }
                                    }
                                });
                            }
                            var strwithspace = str.replace(/\s/g, '&nbsp;'); //str.replace(" ", "&nbsp;")
                            //html.push("<div class='timesheet'><img src='/content/images/icon_three_black_dots.png' height='5px' title='Allocation Timeline' onclick=openResourceTimeSheet('" + strwithspace + "'); />");
                            html.push("<label class='innerCheckbox' onclick=storeCheckedUser()>");
                            html.push("<input type='checkbox' allocaionview='" + popupFilters.isAllocationView + "' pctallocation='" + itemData.PctAllocation + "' id='" + itemData.AssignedTo + "' name='" + itemData.JobTitle + "' onclick=storeCheckedUser()>");
                            html.push("</label>");
                            html.push("<div class='UserDetails'>");
                            html.push("<div id='" + itemData.AssignedTo + "'>");
                            //   html.push("<div id='" + itemData.AssignedTo + "'>");
                            html.push("<div class='AssignedToName'>");
                            html.push(itemData.AssignedToName);
                            html.push("</div>");

                            if (itemData.PctAllocation >= 100 && popupFilters.isAllocationView) {
                                html.push("<div>");
                                html.push("(" + itemData.PctAllocation + "%)");
                                html.push("</div>");
                            }
                            else if (!popupFilters.isAllocationView) {
                                html.push("<div>");
                                html.push("(" + (100 - Number(itemData.PctAllocation)) + "%)");
                                //html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                                html.push("</div>");
                            }
                            if (popupFilters.isAllocationView && itemData.PctAllocation < 100) {
                                html.push("<div>");
                                html.push("(" + (Number(itemData.PctAllocation)) + "%)");
                                html.push("</div>");
                            }
                            if (popupFilters.projectCount || popupFilters.projectVolume) {
                                if (itemData.PctAllocation > 0) {
                                    html.push("<div class='capacitymain'>");
                                    html.push("<div class='capacityblock allocation-v" + itemData.TotalVolumeRange + "'>");
                                    html.push(itemData.TotalVolume == null ? "$0" : itemData.TotalVolume);
                                    html.push("</div>");
                                    html.push("<div class='capacityblock allocation-c" + itemData.projectCountRange + "''>");
                                    html.push("#" + itemData.ProjectCount);
                                    html.push("</div>");
                                    html.push("</div>");
                                }
                            }
                            else {

                            }
                            if (itemData.WeekWiseAllocations?.length > 0 && itemData.WeekWiseAllocations?.filter(x => !x.IsAvailable).length > 0) {
                                html.push(`<div style='cursor:pointer;' onclick='storeCheckedUser();OpenConflictWeekData(${JSON.stringify(itemData.WeekWiseAllocations).replaceAll("'", "`")}, "${itemData.AssignedToName.replaceAll("'", "`")}", "${itemData.AssignedTo}", "${itemData.UserImagePath}", ${dataid});'>`);
                                html.push("<div class='conflict-circle'>");
                                html.push(itemData.WeekWiseAllocations.filter(x => !x.IsAvailable).length);
                                html.push("</div>");
                                html.push("</div>");
                            }
                            html.push("</div>");
                            html.push("</div>");
                            itemElement.attr("class", "allocation-block allocation-r" + itemData.AllocationRange);
                            //itemElement.attr("title", itemData.JobTitle);
                            itemElement.attr("id", "allocation-block" + itemIndex);
                            itemElement.attr("onmouseover", "showTooltip('allocation-block" + itemIndex + "','" + itemData.AssignedTo + "')");
                            itemElement.attr("onmouseout", "hideTooltip()");
                            itemElement.append(html.join(""));

                        },
                        onItemClick: function (e) {
                            let checkedUser = 0;
                            var data = e.itemData;
                            $(".innerCheckbox input").each(function () {
                                if ($(this).is(":checked")) {
                                    checkedUser++;
                                }
                            });
                            if (checkedUser > 0) {
                                if (showTimeSheet == false)
                                    $("#" + data.AssignedTo).click();
                                if (showTimeSheet == true)
                                    showTimeSheet = false;
                            }
                            else {
                                if (showTimeSheet == true) {
                                    showTimeSheet = false;
                                    return;
                                }
                                let uData = UnfilledAllocations.filter(x => x.ID == dataid)[0];
                                uData.AssignedTo = data.AssignedTo;
                                uData.AssignedToName = data.AssignedToName;
                                var grid = $("#UnfilledAllocationsGrid").dxDataGrid('instance');
                                grid.option("dataSource", UnfilledAllocations);
                                $('#popupContainer').dxPopup('instance').hide();
                            }
                        },
                        noDataText: function (e) {
                            $('.dx-empty-message').html('No resource available');
                            $("#popuploader").dxLoadPanel({
                                message: "Loading...",
                                visible: false
                            });
                        }
                    })

                )
            },
            onContentReady: function () {
                bindDatapopup(popupFilters);
            },
            itemClick: function (e) {
            },
            onHiding: function (e) {
                popupFilters.SelectedTags = [];
                popupFilters.SelectedCertifications = "";
            }
        });
        RenderProjectTagsOnFrame(false);
        var popupInstance = $('#popupContainer').dxPopup('instance');
        popupInstance.option('title', popupTitle);

    });

    function GetExperienceTagsData() {
        $.get("/api/rmmapi/GetExperiencedTagList?tagMultiLookup=All", function (data) {
            experienceTags = data;
            GetCertificationsData();
        });
    }

    function GetCertificationsData() {
        $.get("/api/rmmapi/GetCertificationsList", function (data) {
            certifications = data;
        });
    }

    function GetProjectExperienceTagList(data, projectId) {
        if (data != "EmptyProjectTags" && data != "null" && data != null) {
            existingProjectTags = JSON.parse(data).filter(x => x.Type == 2);
            projectExperiencModel.ProjectTags = JSON.parse(JSON.stringify(existingProjectTags));
            projectExperiencModel.ProjectId = projectId;
            GenerateData();
        }
        else {
            projectExperiencTags = [];
        }
    }

    GetExperienceTagsData();

    function GenerateData() {
        projectExperiencTags = [];
        existingProjectTags.forEach(function (value, index) {
            let tag = {};
            if (CheckTagExist(value.Type, value.TagId)) {
                let expTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                tag.TagId = value.TagId;
                tag.ID = String(value.Type) + String(value.TagId);
                tag.Type = value.Type;
                tag.Title = expTag.Title;
                tag.MinValue = parseInt(value.MinValue) > 1 ? 1 : value.MinValue;
                tag.IsMandatory = value.IsMandatory;
                projectExperiencTags.push(tag);
            }
        });
    }
    function RenderProjectTags() {
        $(".experience-tags-row").html("");
        if (projectExperiencModel.ProjectTags != null && projectExperiencModel.ProjectTags.length > 0) {
            projectExperiencModel.ProjectTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == value.TagId)[0] : certifications.filter(x => x.ID == value.TagId)[0];
                    let cssClass = value.IsMandatory ? "dx-tag-content" : "dx-tag-content-1";
                    let title = value.MinValue > 0 ? experiencTag.Title + " &ge; " + value.MinValue : experiencTag.Title
                    let element = $(`<div class="dx-tag"><div class="${cssClass}"><span>${title}</span><div onclick="RemoveProjectTags(${value.TagId}, ${value.Type})" class="dx-tag-remove-button"></div></div></div>`);
                    $(".experience-tags-row").append(element);
                }
            });
        }
    }
    function RenderProjectTagsOnFrame(bindData) {
        if (bindData == undefined)
            bindData = true;
        $("#projectExpTags").html("");
        if (popupFilters.SelectedTags != null && popupFilters.SelectedTags.length > 0) {
            popupFilters.SelectedTags.forEach(function (value, index) {
                if (CheckTagExist(value.Type, value.TagId)) {
                    let experiencTag = value.Type == 2 ? experienceTags.filter(x => x.ID == parseInt(value.TagId))[0] : certifications.filter(x => x.ID == parseInt(value.TagId))[0];
                    let cssClass = value.MinValue > 0 ? "dx-tag-content" : "dx-tag-content-1";
                    let title = value.MinValue > 1 ? experiencTag.Title + " > " + String(parseInt(value.MinValue) - 1) : experiencTag.Title;
                    let style = value.MinValue > 0 ? 'style="cursor:pointer;"' : "";

                    let element = allowGroupFilterOnExpTags
                        ? $(`<div class="dx-tag-1"><div class="${cssClass}"><span id=tag${value.TagId}${value.Type} ${style} onclick="OpenProjectTag(${value.TagId}, ${value.Type})">${title}</span><div onclick="DeleteProjectTag(${value.TagId}, ${value.Type})" style="cursor:pointer;" class="dx-tag-remove-button"></div></div></div>`)
                        : $(`<div class="dx-tag-1"><div class="${cssClass}"><span id=tag${value.TagId}${value.Type}>${title}</span></div></div>`);
                    $("#projectExpTags").append(element);
                }
            });
        }
        
        if (bindData)
            bindDatapopup(popupFilters);
    }

    function DeleteProjectTag(tagId, type) {
        popupFilters.SelectedTags = popupFilters.SelectedTags.removeByValue(popupFilters.SelectedTags.filter(x => x.TagId == tagId && x.Type == type)[0]);
        RenderProjectTagsOnFrame();
    }

    function OpenProjectTag(tagId, type) {
        let expTag = popupFilters.SelectedTags.filter(x => x.TagId == tagId && x.Type == type)[0];
        if (expTag.MinValue != 0) {
            const popupUsersExperienceTags = function () {
                let container = $("<div class='divPopover'>");
                container.append(
                    $("<Label>Tag Name:</Label>"),
                    $("<div id='tagName'>").dxTextBox({
                        placeholder: "Min # of Project Experience",
                        value: expTag.Title,
                        disabled: true,
                    }),
                    $("<Label class='mt-1'>Min # of Project Experience:</Label>"),
                    $("<div id='tagMinValue'>").dxNumberBox({
                        placeholder: "Min # of Project Experience",
                        value: expTag.MinValue,
                        width: 200,
                        min: 1,
                        max: 1000,
                        hint: "Min # of Project Experience",
                        inputAttr: { 'aria-label': 'Min And Max' },
                    }),
                    $(`<div />`).append(
                        $(`<div id="addBtn" class="mt-2 mb-2 btnAddNew" style="float:right;padding: 0px 10px;font-size: 14px;" />`).dxButton({
                            text: "Apply",
                            width: "100%",
                            hint: 'Add Experience Tags',
                            onClick: function (e) {
                                let minValue = $("#tagMinValue").dxNumberBox("instance").option("value") != "" ? parseInt($("#tagMinValue").dxNumberBox("instance").option("value")) : 1;
                                expTag.MinValue = parseInt(minValue);
                                RenderProjectTagsOnFrame();
                                popup.hide();
                            }
                        })
                    ),
                )
                return container;
            };

            const popup = $("#usersExperienceTagsPopover").dxPopup({
                contentTemplate: popupUsersExperienceTags,
                width: "auto",
                height: "auto",
                showTitle: false,
                title: "",
                visible: false,
                dragEnabled: true,
                hideOnOutsideClick: true,
                showCloseButton: true,
                position: {
                    at: 'center',
                    my: 'center',
                    offset: '0 110',
                },
                onHiding: function () {

                },
                onContentReady: function () {
                    $(".divPopover").parent().addClass("dx-popup-content-popover");
                }
            }).dxPopup('instance');

            popup.option({
                contentTemplate: () => popupUsersExperienceTags(),
                'position.of': `#tag${tagId}${type}`,
            });
            popup.show();
        }
    }

    function RemoveProjectTags(tagId, type) {
        if (projectExperiencModel.ProjectTags != null && projectExperiencModel.ProjectTags.length > 0) {
            projectExperiencModel.ProjectTags = projectExperiencModel.ProjectTags.removeByValue(projectExperiencModel.ProjectTags.filter(x => x.TagId == tagId && x.Type == type)[0]);
            RenderProjectTags();
            let doneBtn = $("#doneBtn").dxButton("instance");
            doneBtn.option("visible", true);
        }
    }

    Array.prototype.removeByValue = function (val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i].TagId === val.TagId && this[i].Type === val.Type) {
                this.splice(i, 1);
                i--;
            }
        }
        return this;
    }

    function CheckTagExist(tagType, tagId) {
        if (tagType == 2) {
            return experienceTags.filter(x => x.ID == tagId).length > 0 ? true : false;
        }
        if (tagType == 1) {
            return certifications.filter(x => x.ID == tagId).length > 0 ? true : false;
        }
    }
    function showTooltip(element, assignedTo) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        let userTags = ResourceTagCount.filter(x => x.UserId == assignedTo);
        if (userTags != null && userTags.length > 0) {
            tooltip.option({
                target: "#" + element,
                contentTemplate: function (contentElement) {
                    userTags.forEach(function (value) {
                        contentElement.append(
                            $("<div />").addClass("date-info-tooltip").append(
                                $("<span style='font-weight:600;' />").text(value.Title + "- " + value.TagCount)
                            )
                        );
                    })
                }
            });
            tooltip.show();
        }
    }
    function hideTooltip(element) {
        var tooltip = $("#tooltip").dxTooltip("instance");
        tooltip.hide();
    }

    function OpenExperienceTagPopup() {
        let checkedUser = [];
        $(".innerCheckbox input").each(function () {
            if ($(this).is(":checked")) {
                checkedUser.push($(this).attr("id"));
            }
        });

        if (checkedUser.length <= 1) {
            DevExpress.ui.dialog.alert("Select at least two user.", "Warning!");
            return false;
        }
        let tags = popupFilters.SelectedTags.filter(x => x.IsMandatory);
        if (tags == null || tags.length == 0) {
            DevExpress.ui.dialog.alert("Please select at least one tag from the tags filter or add a new tag.", "Warning!");
            return false;
        }
        const popupUsersExperienceTags = function () {
            let container = $("<div class='userExperiencePopup'>");
            let html = [];
            let resourcesTag = JSON.parse(JSON.stringify(ResourceTagCount.filter(x => checkedUser.includes(x.UserId))));
            let userIds = [...new Set(resourcesTag.map(x => x.UserId))];

            html.push("<table>");
            html.push(`<tr><th></th>`);
            html.push(`<th><div class="th-header mb-1">${popupFilters.isAllocationView ? "Allocated" : "Availability"}</div></th>`);
            tags.forEach(function (value, index) {
                let cssClass = value.MinValue > 0 ? "rounded-circle" : "rounded-circle-dotted";
                html.push(`<th><div class="th-header ${cssClass} mb-1">${value.MinValue > 1 ? value.Title + " > " + (value.MinValue - 1) : value.Title}</div ></th >`)
            });
            html.push("</tr>");

            userIds.forEach(function (value, index) {
                let color = "";
                let userData = resourcesTag.filter(x => x.UserId == value)[0];
                html.push("<tr class='tr-border'>");
                html.push(`<td style="display:flex;align-items:flex-start;justify-content:flex-start;"><img id="userDisplayImage" class="userImageStyle mt-2" alt="User Photo" src="${userData.UserPicture}">
            <div class="mt-2 ml-3 mr-3" style="text-align:center;width:70%"><div style="font-weight:500;">${userData.UserName}</div><div>${userData.RoleName}</div></div></td>`);
                color = parseInt(userData.Availability) > 71 ? "#81B622" : parseInt(userData.Availability) > 0 && parseInt(userData.Availability) <= 71 ? "#A4DE02" : "#D4E8C1";
                html.push(`<td><div class="th-header"><div class="count-box" style="background-color:${color}">${popupFilters.isAllocationView ? userData.Allocation : userData.Availability}%</div ></div ></td >`);
                tags.forEach(function (tag) {
                    let data = resourcesTag.filter(x => x.TagId == tag.TagId && x.Type == tag.Type && x.UserId == value)[0];
                    let maxCount = Math.max.apply(Math, resourcesTag.filter(x => x.TagId == tag.TagId && x.Type == tag.Type).map(x => x.TagCount));
                    let count = parseInt(data.TagCount);
                    if (data.Type == 1 || maxCount == count) {
                        color = "#81B622";
                    }
                    else if (count > (maxCount / 2)) {
                        color = "#A4DE02";
                    }
                    else {
                        color = "#D4E8C1";
                    }
                    html.push(`<td><div class="th-header"><div onclick="OpenExperienceTagProjects(${tag.TagId},'${value}');" class="count-box" style="background-color:${color}">${data.TagCount}</div></div></td>`);
                });
                html.push("</tr>");
            });
            html.push("</table>");
            container.append(html.join(""));
            return container;
        };

        const popup = $("#usersExperienceTagsDialog").dxPopup({
            contentTemplate: popupUsersExperienceTags,
            width: "auto",
            height: "auto",
            showTitle: true,
            title: "",
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
            contentTemplate: () => popupUsersExperienceTags()

        });
        popup.show();
    }

    function bindDatapopup(popupFilters) {
        if (typeof (popupFilters.allocationStartDate) == "object") {
            if (popupFilters.allocationStartDate != null)
                popupFilters.allocationStartDate = new Date(popupFilters.allocationStartDate).format('MM/dd/yyyy');
        }

        if (typeof (popupFilters.allocationEndDate) == "object") {
            if (popupFilters.allocationEndDate != null)
                popupFilters.allocationEndDate = new Date(popupFilters.allocationEndDate).format('MM/dd/yyyy');
        }
        UserConflictData = [];
        $.post("/api/rmmapi/FindResourceBasedOnGroupNew/", popupFilters).then(function (response) {
            if ($("#tileViewContainer").length > 0) {
                var titleViewObj = $('#tileViewContainer').dxTileView('instance');
                if (titleViewObj) {
                    titleViewObj.option("dataSource", response);
                    titleViewObj._refresh();
                }
                $("#popuploader").dxLoadPanel({
                    message: "Loading...",
                    visible: false
                });
            }
        });
    };
    function ChangeAvailability(elem, type) {
        var popup = $('#popupContainer').dxPopup('instance');
        if (popupFilters.isAllocationView == 1) {
            popupFilters.isAllocationView = 0;
            var popupTitle = "Available Resource";
            if (type != undefined || type == "")
                popupTitle = "Available " + type + "s";
            popup.option('title', popupTitle);
        }
        else {
            popupFilters.isAllocationView = 1;

            var popupTitle = "Allocated Resource";
            if (type != undefined || type == "")
                //popupTitle = "Allocated " + type + "s";
                popupTitle = "Allocate " + type;
            popup.option('title', popupTitle)
        }
        bindDatapopup(popupFilters);
    }

    function ChangeProjectExperience() {
        if (popupFilters.projectVolume) {
            popupFilters.projectVolume = false;
            popupFilters.projectCount = false;
            bindDatapopup(popupFilters);
        } else {
            popupFilters.projectVolume = true;
            popupFilters.projectCount = true;
            bindDatapopup(popupFilters);
        }
    }
    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }
    function openResourceTimeSheet(assignedTo, assignedToName, selectedYear) {
        showTimeSheet = true;
        //param isRedirectFromCardView is used to hide card view and show allocation grid
        //param ShowUserResume is used to show user resume page.
        if (selectedYear == undefined) {
            selectedYear = new Date().getFullYear();
        }
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGridNew&ViewName=FindAvailability&isRedirectFromCardView=true&showuserresume=true&selectedYear=" + selectedYear + "&selecteddepartment=-1&SelectedResource=" + assignedTo;
        window.parent.UgitOpenPopupDialog(url, "", "Gantt: " + assignedToName, "95", "95", "", false);
    }
    function OpenConflictWeekData(data, name, userId, UserImageUrl, allocationID = null) {
        let conflictData = data.filter(x => !x.IsAvailable);
        var selectedYear = new Date(Math.min.apply(null, conflictData.map(x => new Date(x.WeekStartDate)))).getFullYear();
        let filteredUsers = [];
        
        filteredUsers = UserConflictData.filter(x => x.WeekWiseAllocations.every(y => y.IsAvailable));
        let allocationData = UnfilledAllocations.filter(x => x.ID == allocationID)[0];
        let ticketUrl = `openTicketDialog('${allocationData.StaticModulePagePath}','TicketId=${allocationData.ProjectId}','${allocationData.ERPJobID}: ${allocationData.Title}','95','95', 0, '')`
        const ConflictWeekDataGridTemplate = function () {
            let container = $("<div>");
            container.append(
                $(`<div style='margin-bottom:10px;'>Weekly 'Alloc %' shown includes prospective <span onclick="${ticketUrl}" style='color:#4fa1d6;text-decoration:underline;cursor:pointer;'>${allocationData.ERPJobID}</span> allocation</div>`)
            );
            let windowHeight = parseInt(70 * $(window).height() / 100);
            let datagrid = $(`<div id='ConflictWeekDataGrid' style='max-height:${windowHeight}px'>`).dxDataGrid({
                dataSource: conflictData,
                ID: "grdConflictWeekData",
                editing: {
                    mode: "cell",
                    allowEditing: true,
                    allowUpdating: true
                },
                sorting: {
                    mode: "multiple"
                },
                paging: {
                    enabled: false,
                },
                scrolling: {
                    mode: 'Standard',
                },
                columns: [
                    {
                        dataField: "WeekStartDate",
                        dataType: "date",
                        caption: "Start Date",
                        allowEditing: false,
                        sortIndex: "0",
                        sortOrder: "asc",
                        format: 'MMM d, yyyy',
                    },
                    {
                        caption: "End Date",
                        dataType: "date",
                        format: 'MMM d, yyyy',
                        calculateCellValue: function (rowData) {
                            return new Date(rowData.WeekStartDate).addDays(parseInt(rowData.NoOfDays) - 1);
                        }
                    },
                    {
                        dataField: "PctAllocation",
                        caption: "% Alloc",
                        dataType: "text",
                        width: "20%",
                        allowEditing: false,
                    },
                    {
                        dataField: "PostPctAllocation",
                        caption: "% Post Alloc",
                        dataType: "text",
                        width: "20%",
                        allowEditing: false,
                    }
                ],
                onCellClick: function (e) {
                    OpenConflictWeekDetailSummary(e.data.WeekdetailedSummaries);
                },
                onRowPrepared: function (info) {
                    if (info.rowType === 'data') {
                        info.rowElement.css("cursor", 'pointer');
                    }
                },
                onCellPrepared: function (e) {

                    if (e.rowType === 'data') {
                        var preconstartDate = new Date(allocationData.PreconStartDate);
                        var preconEndDate = new Date(allocationData.PreconEndDate);

                        var conststartDate = new Date(allocationData.EstimatedConstructionStart);
                        var constEndDate = new Date(allocationData.EstimatedConstructionEnd);

                        var closeoutstartDate = new Date(allocationData.CloseoutStartDate);
                        var closeoutEndDate = new Date(allocationData.CloseoutDate);

                        if (e.column.dataField == 'WeekStartDate') {
                            let cellValue = new Date(e.data.WeekStartDate)
                            let className = getDateBackgroundColor(cellValue, preconstartDate, preconEndDate, conststartDate, constEndDate, closeoutstartDate, closeoutEndDate);
                            e.cellElement.addClass(className);
                        }
                        if (typeof e.column.dataField == "undefined") {
                            let cellValue = new Date(e.data.WeekStartDate).addDays(parseInt(e.data.NoOfDays) - 1);
                            let className = getDateBackgroundColor(cellValue, preconstartDate, preconEndDate, conststartDate, constEndDate, closeoutstartDate, closeoutEndDate);
                            e.cellElement.addClass(className);
                        }
                    }

                    if (e.row == undefined) return;
                    if (e.row.key.WeekdetailedSummaries?.length > 1 && e.column.name == "PostPctAllocation") {
                        e.cellElement.addClass('color-style');
                    }
                },
                showBorders: true,
                showRowLines: true,
            });
            container.append(datagrid);
            return container;
        };

        const popup = $("#ConflictWeekDataGridDialog").dxPopup({
            contentTemplate: ConflictWeekDataGridTemplate,
            width: "600",
            height: "auto",
            showTitle: true,
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            titleTemplate: function () {
                let headerData = $(`<span style="float: left;overflow: auto;">Conflict Weeks: <a href="javascript:void(0);" onclick="openResourceTimeSheet('${userId}','${name}','${selectedYear}');">${name}</a></span>`);
                headerData.append(
                    $(`<span title="Close" class="dx-button-content close-btn" onclick="$('#ConflictWeekDataGridDialog').dxPopup('instance').hide()"><i class="dx-icon dx-icon-close" style='font-size:20px;'></i></span>`)
                );
                return headerData;
            },
            wrapperAttr: {
                id: "ConflictWeekDataGridDialog",
                class: "class-name"
            },
            onHiding: function () {
            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => ConflictWeekDataGridTemplate()

        });
        popup.show();
    }

    function OpenConflictWeekDetailSummary(data) {

        const ConflictWeekDataGridTemplateSummary = function () {
            let container = $("<div>");
            let windowHeight = parseInt(75 * $(window).height() / 100);
            let datagrid = $(`<div id='ConflictWeekDataSummaryGrid' style='max-height:${windowHeight}px'>`).dxDataGrid({
                dataSource: data,
                ID: "grdConflictWeekDataSummary",
                editing: {
                    mode: "cell",
                    allowEditing: true,
                    allowUpdating: true
                },
                sorting: {
                    mode: "multiple"
                },
                paging: {
                    enabled: false,
                },
                scrolling: {
                    mode: 'Standard',
                },
                columns: [
                    {
                        dataField: "Title",
                        caption: "Title",
                        dataType: "text",
                        width: "60%",
                        sortIndex: "0",
                        sortOrder: "asc",
                    },
                    {
                        dataField: "Role",
                        caption: "Role",
                        dataType: "text",
                    }
                ],
                showBorders: true,
                showRowLines: true,
            });
            container.append(datagrid);
            return container;
        };

        const popup = $("#ConflictWeekDataGridDialogSummary").dxPopup({
            contentTemplate: ConflictWeekDataGridTemplateSummary,
            width: "500",
            height: "auto",
            showTitle: true,
            visible: false,
            dragEnabled: true,
            hideOnOutsideClick: true,
            showCloseButton: true,
            position: {
                at: 'center',
                my: 'center',
            },
            titleTemplate: function () {
                let headerData = $(`<span style="float: left;overflow: auto;">Conflict Week Summary</span>`);
                headerData.append(
                    $(`<span title="Close" class="dx-button-content close-btn" onclick="$('#ConflictWeekDataGridDialogSummary').dxPopup('instance').hide()"><i class="dx-icon dx-icon-close" style='font-size:20px;'></i></span>`)
                );
                return headerData;
            },
            wrapperAttr: {
                id: "ConflictWeekDataGridDialogSummary",
                class: "class-name"
            },
            onHiding: function () {

            }
        }).dxPopup('instance');

        popup.option({
            contentTemplate: () => ConflictWeekDataGridTemplateSummary()

        });
        popup.show();
    }

    function storeCheckedUser() {
        showTimeSheet = true;
    }
</script>
<div id="UnfilledAllocationsGridDialog"></div>
<dx:ASPxLoadingPanel ID="ResourceAvailabilityloadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="ResourceAvailabilityloadingPanel"
    Modal="True">
</dx:ASPxLoadingPanel>

<asp:HiddenField ID="hdnTypeLoader" runat="server" />

<dx:ASPxCallback ID="cbMailsend" runat="server" ClientInstanceName="cbMailsend" OnCallback="cbMailsend_Callback">
    <ClientSideEvents CallbackComplete="OnCallbackComplete" />
</dx:ASPxCallback>

<div class="col-md-12 col-sm-12 col-xs-12 noSidePadding reourceUti-container">
    <div class="row">
        <div id="divFilter" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12 noSidePadding">
                <div class="row resourceUti-filterWarp">
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Department:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <ugit:LookupValueBoxEdit ID="ddlDepartment" runat="server" IsMulti="true" CssClass="rmmLookup-valueBoxEdit rmmDepartment-drpDown" FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged" />
                            <%--<asp:DropDownList CssClass="txtbox-halfwidth aspxDropDownList rmm-dropDownList" ID="ddlDepartment" runat="server" onchange="ShowLoader()" 
                                OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>--%>
                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView" style="display: none;">
                        <div class="resourceUti-label">
                            <label>Functional Area:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <asp:DropDownList CssClass="txtbox-halfwidth aspxDropDownList rmm-dropDownList" ID="ddlFunctionalArea" runat="server" onchange="ShowLoader()" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlFunctionalArea_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView">
                        <div class="resourceUti-label">
                            <label>Category:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <dx:ASPxComboBox ClientInstanceName="ddlCategory" runat="server" ID="ddlCategory"
                                DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" EnableSynchronization="true" CssClass="aspxComboBox-dropdown"
                                ListBoxStyle-CssClass="aspxComboBox-listBox" AutoPostBack="false">
                                <ClientSideEvents SelectedIndexChanged="refreshGridDataUsingCallback" />
                                <Items>
                                    <dx:ListEditItem Text="Utilization by Job Title" Value="JobTitle" />
                                    <dx:ListEditItem Text="Utilization by Role" Value="Role" Selected="true" />
                                    <dx:ListEditItem Text="Unfilled Project Roles" Value="UnfilledRoles" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView" style="display:none">
                        <div class="resourceUti-label">
                            <label>Roles:</label>
                        </div>
                        <div class="resourceUti-inputField">
                            <dx:ASPxCallbackPanel ID="cbpResourceAvailability" ClientInstanceName="cbpResourceAvailability" runat="server"  OnCallback="cbpResourceAvailability_Callback" SettingsLoadingPanel-Enabled="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <asp:DropDownList ID="ddlUserGroup" runat="server" AutoPostBack="true" onchange="ShowLoader()"
                                            OnSelectedIndexChanged="ddlUserGroup_SelectedIndexChanged" CssClass="resourceUti-dropDownList aspxDropDownList rmm-dropDownList">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnaspDepartment" runat="server" />
                                    </dx:PanelContent>
                                </PanelCollection>
                                <%--<SettingsLoadingPanel ShowImage="false" Enabled="false" />--%>
                                <ClientSideEvents EndCallback="function(s,e){ InitiateGridCallback(s,e);}" />
                            </dx:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-3 col-xs-12 colForTabView clearBoth-forSM" style="display:none">
                        <div class="resourceUti-label" id="tdManager" runat="server">
                            <label>Manager:</label>
                        </div>
                        <div class="resourceUti-inputField" id="tdManagerDropDown" runat="server">
                            <dx:ASPxCallbackPanel ID="cbpManagers" ClientInstanceName="cbpManagers" runat="server"  OnCallback="cbpManagers_Callback" SettingsLoadingPanel-Enabled="false">
                                <PanelCollection>
                                    <dx:PanelContent>
                                        <asp:DropDownList ID="ddlResourceManager" CssClass="managerdropdown aspxDropDownList rmm-dropDownList" onchange="ShowLoader()" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlResourceManager_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxCallbackPanel>                            
                        </div>
                    </div>
                    <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12 colForTabView">
                        <div class="resourceUti-label" id="tdType" runat="server">
                            <label>Type:</label>
                        </div>
                         <div id="tdTypeGridLookup" runat="server" class="resourceUti-inputField">
                              <dx:ASPxGridLookup  Visible="true" TextFormatString="{0}" SelectionMode="Multiple" ID="glType" 
                                    runat="server" KeyFieldName="LevelTitle" MultiTextSeparator="; " AutoPostBack="false" CssClass="rmmGridLookup widthFull" 
                                  DropDownWindowStyle-CssClass="RMMaspxGridLookup-dropDown" GridViewStyles-Row-CssClass="aspxGridloookUp-drpDownRow" 
                                  GridViewStyles-FilterRow-CssClass="aspxGridLookUp-FilerWrap" 
                                  GridViewStyles-FilterCell-CssClass="aspxGridLookUp-FilerCell" GridViewProperties-Settings-VerticalScrollableHeight="150">
                                    <Columns>
                                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" Width="28px" />
                                        <dx:GridViewDataTextColumn FieldName="LevelTitle" Width="200px" Caption="Choose Type:">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="LevelName" Visible="false"></dx:GridViewDataTextColumn>
                                    </Columns>
                                    <GridViewProperties>
                                        <Settings ShowGroupedColumns="false" ShowFilterRow="true" VerticalScrollBarMode="Auto" />
                                        <SettingsBehavior AllowSort="false" />
                                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                    </GridViewProperties>
                                    <ClientSideEvents CloseUp="TypeCloseUp"  EndCallback="function(s,e){ InitiateGridCallback(s,e);}" />
                                </dx:ASPxGridLookup>
                         </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="float: right; margin-left: 2px; padding-left: 5px; padding-right: 10px; display: none">
            <span style="padding-right: 5px;">
                <asp:ImageButton ImageUrl="/Content/images/Previous16x16.png" ID="previousYear" ToolTip="Prevoius" runat="server" OnClientClick="ShowLoader()" 
                    OnClick="previousYear_Click" CssClass="resource-img" />
            </span>
            <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
            <span style="padding-left: 5px;">
                <asp:ImageButton ImageUrl="/Content/images/next-arrowBlue.png" ID="nextYear" ToolTip="Next" runat="server" OnClientClick="ShowLoader()" OnClick="nextYear_Click" 
                    CssClass="resource-img" />
            </span>
        </div>
        <asp:HiddenField ID="hdnSelectedGroup" runat="server" />
    </div>

    <div class="row" style="padding-top:5px; padding-bottom:10px;">
        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12">
            <div class="rmmChkBox-container AllCheckBox" id="divCheckbox" runat="server">
                    <dx:ASPxCheckBox ID="chkAll" runat="server" AutoPostBack="false" Text="All Resources" CssClass="Checkbox">
                        <ClientSideEvents ValueChanged="chkAll_ValueChanged" />
                    </dx:ASPxCheckBox>
                    <dx:ASPxCheckBox ID="chkIncludeClosed" runat="server" AutoPostBack="false" Text="Closed Projects" CssClass="Checkbox">
                        <ClientSideEvents ValueChanged="chkIncludeClosed_ValueChanged" />
                    </dx:ASPxCheckBox>
                    <dx:ASPxCheckBox ID="chkIncludeSoftAllocation" runat="server" AutoPostBack="false" Text="Include Soft Allocations" CssClass="Checkbox">
                        <ClientSideEvents ValueChanged="chkIncludeClosed_ValueChanged" />
                    </dx:ASPxCheckBox>
                    <dx:ASPxCheckBox ID="chkOnlyNCOs" runat="server" AutoPostBack="false" Text="Only NCO" CssClass="Checkbox">
                        <ClientSideEvents ValueChanged="chkIncludeClosed_ValueChanged" />
                    </dx:ASPxCheckBox>
                </div>
        </div>
         <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12">
            <div class="valueViewMode bC-radioBtnWrap" style="width:100%">
                    <dx:ASPxRadioButton ID="rbtnFTE" runat="server" AutoPostBack="false" Text="FTE" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                        <ClientSideEvents CheckedChanged="rbtnFTE_CheckedChanged" />
                    </dx:ASPxRadioButton>
                    <dx:ASPxRadioButton ID="rbtnHrs" runat="server" AutoPostBack="false" Text="Hrs" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                        <ClientSideEvents CheckedChanged="rbtnHrs_CheckedChanged" />
                    </dx:ASPxRadioButton>
                    <dx:ASPxRadioButton ID="rbtnPercentage" runat="server" AutoPostBack="false" Text="%" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                        <ClientSideEvents CheckedChanged="rbtnPercentage_CheckedChanged" />
                    </dx:ASPxRadioButton>
                    <dx:ASPxRadioButton ID="rbtnItemCount" runat="server" AutoPostBack="false" Text="#" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                        <ClientSideEvents CheckedChanged="rbtnCount_CheckedChanged" />
                    </dx:ASPxRadioButton>
                    <dx:ASPxRadioButton ID="rbtnAvailability" runat="server" AutoPostBack="false" Text="Availability" CssClass="radiobutton importChk-radioBtn" GroupName="filtermode">
                        <ClientSideEvents CheckedChanged="rbtnAvailability_CheckedChanged" />
                    </dx:ASPxRadioButton>
                </div>
        </div>
        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12">
            <div id="divProject" runat="server" class="viewProjectMode bC-radioBtnWrap" visible="true">
                <asp:RadioButton ID="rbtnProject" runat="server" AutoPostBack="true" Text="Project Allocation"  onchange="showLoader()" Checked="true" 
                    GroupName="ProjectAllocation" OnCheckedChanged="rbtnProject_CheckedChanged" CssClass="importChk-radioBtn" />
                <asp:RadioButton ID="rbtnAll" runat="server" AutoPostBack="true" Text="All Allocation"  onchange="showLoader()" 
                    GroupName="ProjectAllocation" OnCheckedChanged="rbtnAll_CheckedChanged" CssClass="importChk-radioBtn" />
            </div>
        </div>
        <div class="col-xl-3 col-md-3 col-sm-6 col-xs-12">
            <div id="divAllocation" runat="server" class="valueTypeMode">
                <div class="allocationView bC-radioBtnWrap">
                    <asp:RadioButton ID="rbtnEstimate" runat="server" AutoPostBack="true" Text="Allocation" CssClass="radiobutton importChk-radioBtn" 
                        onchange="showLoader()" Checked="true" GroupName="Allocation" OnCheckedChanged="rbtnEstimate_CheckedChanged" />
                    <asp:RadioButton ID="rbtnAssigned" runat="server" AutoPostBack="true" Text="Assignment" CssClass="radiobutton importChk-radioBtn"
                        onchange="showLoader()" GroupName="Allocation" OnCheckedChanged="rbtnAssigned_CheckedChanged" />
                </div>
            </div>
        
        <div class="rmmExport-optionBtnWrap">
            <%--<div class="d-flex justify-content-end align-items-center">
                <asp:Button ID="btnExportToExcel" runat="server" Style="float: left; padding-right: 5px; margin-right: 5px;" Text="Excel" OnClick="btnExportToExcel_Click" />
                <asp:Button ID="btnExportToPdf" runat="server" Style="float: left; padding-right: 5px; margin-right: 5px;" Text="PDF" OnClick="btnExportToPdf_Click" />
                <div class="ExportOption-btns">
                    <a title="Export to Excel" class="dxbButton dxbButtonSys" id="ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_btnExcelExport" href="javascript:;" style="font-size: 0pt;"><img class="excelicon dx-vam" src="/DXR.axd?r=1_89-9fJdn" alt="" id="ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_btnExcelExportImg"></a>
                </div>
                <div class="ExportOption-btns">
                    <a title="Export to Pdf" class="dxbButton export-buttons dxbButtonSys" id="ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_btnPdfExport" href="javascript:;" style="font-size: 0pt;"><img class="pdf-icon dx-vam" src="/DXR.axd?r=1_89-9fJdn" alt="" id="ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_btnPdfExportImg"></a>
                </div>
            </div>--%>

            <div class="ExportOption-btns displayHide" style="padding-right: 5px;">
                <%--<dx:ASPxButton ID="btnExcelExport" ClientInstanceName="btnExcelExport" runat="server" EnableTheming="false" UseSubmitBehavior="False"
                    OnClick="btnExportToExcel_Click" RenderMode="Link" ToolTip="Export to Excel">
                    <Image>
                        <SpriteProperties CssClass="excelicon" />
                    </Image>
                    <ClientSideEvents Click="function(s, e) { LoaderOnExport('Exporting'); _spFormOnSubmitCalled=false; }" />
                </dx:ASPxButton>--%>
                    <dx:ASPxMenu ID="mnuExportOptions" runat="server" AllowSelectItem="false"
                    OnItemClick="mnuExportOptions_ItemClick"
                        ShowPopOutImages="True" BackColor="White" Border-BorderColor="White" SubMenuItemStyle-DropDownButtonStyle-Paddings-PaddingTop="60px" BorderLeft-BorderStyle="None" HorizontalPopOutImage-IconID="none" style="width: 35px;">
                    <Items>
                        <dx:MenuItem ItemStyle-BorderRight-BorderStyle="None" Text="" ItemStyle-DropDownButtonStyle-Paddings-PaddingTop="40px" ItemStyle-Height="0px" ItemStyle-BackColor="White" ItemStyle-Width="39px" ItemStyle-Border-BorderColor="White">
                            <Items>
                                <dx:MenuItem Text="Excel" ToolTip="Export to Excel">
                                    <SubMenuItemStyle Width="242px" Paddings-PaddingTop="20px"></SubMenuItemStyle>
                                    <Image>
                                        <SpriteProperties CssClass="excelicon"/>
                                    </Image>
                                </dx:MenuItem>
                                <dx:MenuItem Text="CSV" ToolTip="Export to CSV">
                                    <Image>
                                        <SpriteProperties CssClass="csvicon"/>
                                    </Image>
                                </dx:MenuItem>
                            </Items>
                            <Image>
                                <SpriteProperties CssClass="excelicon"/>
                            </Image>
                        </dx:MenuItem>
                    </Items>
                    <ClientSideEvents ItemClick="function(s, e) { LoaderOnExport(); _spFormOnSubmitCalled=false; }" />
                </dx:ASPxMenu>
            </div>
            <div class="ExportOption-btns displayHide">
                <dx:ASPxButton ID="btnPdfExport" ClientInstanceName="btnPdfExport" runat="server" CssClass="export-buttons" EnableTheming="false" UseSubmitBehavior="False"
                    RenderMode="Link" OnClick="btnExportToPdf_Click" ToolTip="Export to Pdf" style="padding-top: 4px;">
                    <Image>
                        <SpriteProperties CssClass="pdf-icon" />
                    </Image>
                    <ClientSideEvents Click="function(s, e) { LoaderOnExport('Exporting'); _spFormOnSubmitCalled=false; }" />
                </dx:ASPxButton>
            </div>
            <div class="ExportOption-btns displayHide">
                <dx:ASPxButton ID="SendEmail" runat="server" CssClass="export-buttons" AutoPostBack="false" Visible="false"
                    EnableTheming="false" UseSubmitBehavior="False" RenderMode="Link">
                    <Image>
                        <SpriteProperties CssClass="sendmail" />
                    </Image>
                    <ClientSideEvents Click="function(s,e) { SendMailClick(); }" />
                </dx:ASPxButton>
            </div>
            <div class="reportPanelImage">
                <dx:ASPxButton ID="imgReport" runat="server" RenderMode="Link" AutoPostBack="false" Image-Width="25" Image-Height="25"
                    Image-Url="~/Content/Images/reports-Black.png">
                </dx:ASPxButton>
            </div>
        </div>
            </div>
    </div>
    <div class="row">
        <div class="respurceUti-gridContainer" style="margin-bottom:250px">
           <%-- OnHtmlRowPrepared="gvCapacityReport_HtmlRowPrepared" 
               OnHtmlDataCellPrepared="gvCapacityReport_HtmlDataCellPrepared" --%>
            <ugit:ASPxGridView ID="gvCapacityReport" runat="server" AutoGenerateColumns="false" KeyFieldName="ItemOrder"
                 CssClass="homeGrid" 
                OnDataBinding="gvCapacityReport_DataBinding" ClientInstanceName="gvResourceAvailablity"  
                OnCustomSummaryCalculate="gvCapacityReport_CustomSummaryCalculate" OnCustomCallback="gvResourceAvailablity_CustomCallback"
                 SettingsText-EmptyDataRow="No Resource allocated." Width="100%">
                <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                <Columns>
                </Columns>
                
                <SettingsBehavior AllowGroup="false" AutoExpandAllGroups="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                <Settings ShowGroupedColumns="false" ShowGroupFooter="VisibleIfExpanded" ShowFooter="true" VerticalScrollBarMode="Hidden" />
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <SettingsLoadingPanel mode="Disabled"/>
                <ClientSideEvents SelectionChanged="OnSelectionChanged" EndCallback="OnEndCallback" />
                <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <Styles>
                    <SelectedRow BackColor="#DBEAF9"></SelectedRow>
                    <Row CssClass="RMM-resourceUti-gridDataRow"></Row>
                    <Header CssClass="RMM-resourceUti-gridHeaderRow"></Header>
                    <AlternatingRow BackColor="#f6f7fc"></AlternatingRow>
                    <Footer Font-Bold="true" HorizontalAlign="Center" Border-BorderColor="#D9DAE0" Border-BorderStyle="Solid" Border-BorderWidth="1px" 
                        BorderRight-BorderWidth=".5px" CssClass="resourceUti-gridFooterRow"></Footer>
                </Styles>
            </ugit:ASPxGridView>

            <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvCapacityReport" OnRenderBrick="gridExporter_RenderBrick"></dx:ASPxGridViewExporter>
            <%--<dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gvCapacityReport"></dx:ASPxGridViewExporter>--%>

             <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
        </div>
    </div>
</div>

<div style="padding-left: 15px;">
    <div style="display: none;">
<%--        <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" OnClientClick="ShowLoader()" />--%>
        <asp:Button ID="btnDrilDown" runat="server" OnClick="btnDrilDown_Click" OnClientClick="ShowLoader()" />
        <asp:Button ID="btnDrilUp" runat="server" OnClick="btnDrilUp_Click" OnClientClick="ShowLoader()" />
    </div>
</div>

<asp:HiddenField ID="hdndtfrom" runat="server" />
<asp:HiddenField ID="hdndtto" runat="server" />
<asp:HiddenField ID="hdndisplayMode" runat="server" />
<asp:HiddenField ID="hdnSelectedDate" runat="server" />
<div id="popupContainer"></div>
<div id="usersExperienceTagsPopover"></div>
<div id="ConflictWeekDataGridDialog"></div>
<div id="ConflictWeekDataGridDialogSummary"></div>
<div id="usersExperienceTagsDialog"></div>
<div id="tooltip"></div>
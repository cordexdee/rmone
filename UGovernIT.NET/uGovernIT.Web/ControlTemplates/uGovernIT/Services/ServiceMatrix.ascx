<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceMatrix.ascx.cs" EnableViewState="true" Inherits="uGovernIT.Web.ServiceMatrix" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" src="/Scripts/jquery.verticalCarousel.min.js"></script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .hidetabdiv {
        display: none;
    }

    .vertical-carousel-mobile {
        margin: 5px 0px 2px 11px;
    }

    .vertical-carousel {
        padding: 2px 0px 0px 10px;
        height: auto;
    }

    /*.divgrd-mobile,
    .divgrd {
        float: left;
        width: 60%;
        overflow: auto;
    }*/

    /*.divgrd-mobile {
        float: inherit;
        width: inherit;
    }*/

    .scrollup {
        height: 12px;
        background: url('/content/images/scrollup.png') no-repeat 94px 1px;
    }

    .scrolldown {
        height: 8px;
        background: url('/content/images/scrolldown.png') no-repeat 95px 0px;
    }

    .vertical-carousel-container {
        /*padding: 15px 0px 0px 11px;*/
    }

    .vertical-carousel-mobile a,
    .vertical-carousel a {
        display: block;
        padding: 10px;
        text-align: center;
        color: #333;
        text-decoration: none;
    }

    .vertical-carousel-mobile ul.vertical-carousel-list,
    .vertical-carousel ul.vertical-carousel-list {
        position: relative;
        margin: 0 auto;
        top: 0px;
        padding: 0px;
    }

        .vertical-carousel ul.vertical-carousel-list li {
            list-style: none;
            margin: 0px 10px 5px 0px;
            color: #333;
            display: block;
            /*display: inline;*/
            padding: 5px;
            text-align: center;
            background-color: #ddd;
            cursor: pointer;
            cursor: hand;
            border: 1px solid #a8a8a8;
        }

        .vertical-carousel-mobile ul.vertical-carousel-list li {
            list-style: none;
            margin: 10px 10px 0px 0px;
            color: #333;
            display: inline-block;
            padding: 5px;
            text-align: center;
            background-color: #ddd;
            cursor: pointer;
            cursor: hand;
            border: 1px solid #a8a8a8;
        }

        .vertical-carousel ul.vertical-carousel-list li.active {
            list-style: none;
            margin: 0px 10px 5px 0px;
            color: #fff;
            display: block;
            /*display: inline;*/
            padding: 5px;
            margin-bottom: 5px;
            text-align: center;
            background-color: #394850;
            cursor: pointer;
            cursor: hand;
            border: 1px solid #a8a8a8;
        }

        .vertical-carousel-mobile ul.vertical-carousel-list li.active {
            list-style: none;
            margin: 10px 10px 0px 0px;
            color: #fff;
            display: inline-block;
            padding: 5px;
            text-align: center;
            background-color: #394850;
            cursor: pointer;
            cursor: hand;
            border: 1px solid #a8a8a8;
        }

    .clear {
        clear: both;
    }

    .subCategoryBackground {
        background-color: WhiteSmoke;
    }

    .titleHeaderBackground {
        text-align: center;
        /*background: lightgray !important;*/
    }

    .alternateBackground {
        background-color: white;
    }

    .tablerow {
        /*height: 25px;*/
        border: 1px solid black;
    }

    table.gvResourceClass {
        margin-bottom: 10px;
        background-color:#fff;
    }

        table.gvResourceClass > tbody > tr > td {
            /*width: 100px;*/
            height: 20px;
            text-align: center;
        }

        table.gvResourceClass td.locked, th.locked {
            position: relative;
            left: expression((this.parentElement.parentElement.parentElement.parentElement.scrollLeft-2)+'px');
        }

    .lblHeader {
        font-weight: bold;
        margin-bottom: 10px;
    }

    .lblNotes {
        font-weight: bold;
        margin-bottom: 10px;
        display: inline;
    }

    .rptApplications {
        margin-top: 10px;
    }

    .header {
        padding: 0px 9px;
        font-weight: bold;
        text-align: left;
        background-color: LightGray;
        color: #3E4F50;
        width: 80px;
        text-align: center;
    }

    .divcell_0 {
        background-image: url('/content/images/pm_gridtopcorner.png');
        height: 32px;
        background-color: #3E4F50;
        color: #F2F2F2;
        width: 107px;
    }

    .cell_0_0 {
        background-color: #3E4F50;
        padding: 0px;
        height: 24px;
        width: 107px;
    }

    .lightText {
        color: #9B9B9B;
    }

    .hide {
        display: none;
    }

   

    /*.divAppTab {
        float: left;
        width: 160px;
        padding-top: 9px;
    }*/

    .lblRoleName {
        float: left;
        margin-left: 7px;
        margin-top: 3px;
    }

    .selectallCheckbox {
        float: left;
    }

        .selectallCheckbox input {
            position: absolute;
            left: 0px;
        }

        .selectallCheckbox label {
            float: left;
            padding-top: 3px;
        }

    .selectallCheckboxCell {
        Position: relative;
        text-align: left;
        padding-left: 20px !important;
    }
</style>

<script type="text/javascript"  data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        devexGrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        devexGrid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function selectAllModules(obj) {
        var chkCheckBoxAccess = obj;
        var checkboxes = $($(chkCheckBoxAccess).closest('td')).parent('tr').find('input:checkbox');
        $.each(checkboxes, function () {
            var item = $(this);
            if (item.get(0).id != chkCheckBoxAccess.id) {
                if ($(chkCheckBoxAccess).prop("checked") == true) {
                    if (item.prop('checked') == false && item.prop('disabled')==false) {
                        item.prop('checked', true);
                        $(item).parents('td:first').css("background-color", "#ffff9e");
                    }
                }
                else {
                    item.prop('checked', false);
                    $(item).parents('td:first').css("background-color", "inherit");
                }

            }
        });
    }
    function toggleAccess(obj) {
        var chkCheckBoxAccess = obj;
        var isChecked = $(chkCheckBoxAccess).parent().attr('ischecked');
        var ischeckedbckgrnd = $(chkCheckBoxAccess).parent().attr('ischeckedbckgrnd');
        var module = $(chkCheckBoxAccess).parent().attr('ItemText');
        var role = $(chkCheckBoxAccess).parent().attr('ItemValue');
        if ($(chkCheckBoxAccess).is(':checked')) {
            if (isChecked == "false")
                $(chkCheckBoxAccess).parents('td:first').css("background-color", "#ffff9e");
            else if (ischeckedbckgrnd != undefined)
                $(chkCheckBoxAccess).parents('td:first').css("background-color", ischeckedbckgrnd);
            else
                $(chkCheckBoxAccess).parents('td:first').css("background-color", "inherit");

        }
        else {
            if (isChecked == "true")
                $(chkCheckBoxAccess).parents('td:first').css("background-color", "#FFE7E4");
            else if (ischeckedbckgrnd != undefined)
                $(chkCheckBoxAccess).parents('td:first').css("background-color", ischeckedbckgrnd);
            else
                $(chkCheckBoxAccess).parents('td:first').css("background-color", "inherit");

        }
        var selectall = false;
        var selectedrow = $(chkCheckBoxAccess).closest('tr.subCategoryBackground');
        if ($(selectedrow.find('.moduleCheckbox input:checkbox')).not(':checked').length == 0) {
            selectall = true
        }


        if ($(selectedrow.find('.selectallCheckbox input:checkbox'))) {
            $(selectedrow.find('.selectallCheckbox input:checkbox')).prop('checked', selectall);
        }
    }

    $(document).ready(function () {
        $($('.selectallCheckbox').closest('td')).addClass('selectallCheckboxCell');

        $.each($('.gvResourceClass'), function () {
            var application = $(this);
            $.each(application.find('.subCategoryBackground'), function () {
                var row = $(this);
                if ($(row.find('.moduleCheckbox input:checkbox')).not(':checked').length == 0) {
                    if ($(row.find('.selectallCheckbox input:checkbox')))
                        $(row.find('.selectallCheckbox input:checkbox')).prop('checked', true);
                }
            });
        });

        if ($(".txtNotes").val() == "" || $.trim($(".txtNotes").val()) == '') {
            $(".txtNotes").addClass("lightText").val("Any Special Instructions")
        }
        $(".txtNotes").focus(function () {
            if ($(this).val() == "Any Special Instructions") {
                $(this).removeClass("lightText").val("");
            }
        }).blur(function () {
            if ($(this).val() == "") {
                $(this).val("Any Special Instructions").addClass("lightText");
            }
        });
        $('.vertical-carousel-list').on('click', 'li', function () {

            $(this).addClass('active').siblings().removeClass('active');
        });
        setApplicationScroll();
    });
    function setApplicationScroll()
    {
         var selectedApplicationCount = parseInt("<%=SelectedApplicationCount %>");
        var showApplications = parseInt("<%= ShowApplications %>");

        if ('<%=IsMobile%>' == 'True') {
            $("#<%=divAppCarousal.ClientID%>").addClass("vertical-carousel-mobile");
            $("#<%=divAppCarousal.ClientID%>").removeClass("vertical-carousel");
            $("#<%=divAppTab.ClientID%>").removeClass("divAppTab");
            $("#vertical-carousel-container").addClass("vertical-carousel-container");
        }
        else {
            $("#<%=divAppCarousal.ClientID%>").addClass("vertical-carousel");
            $("#<%=divAppCarousal.ClientID%>").removeClass("vertical-carousel-mobile");
            $("#<%=divAppTab.ClientID%>").addClass("divAppTab");
        }
        if (selectedApplicationCount > showApplications && showApplications != 0 && '<%=IsMobile%>' == 'False') {

            $("#<%=divAppCarousal.ClientID%>").verticalCarousel({ nSlots: showApplications, speed: 400 });
            $('#<%=divAppCarousal.ClientID%>').find(".scru").show();
            $('#<%=divAppCarousal.ClientID%>').find('.scrd').show();
        }
        else {
            $('#<%=divAppCarousal.ClientID%>').find(".scru").hide();
            $('#<%=divAppCarousal.ClientID%>').find('.scrd').hide();
        }
    }
    function SetWidth(parentControl) {
        setTimeout(function () {
            var gridID = $($(parentControl).find('.gvResourceClass')).get(0).id;
            var gridObject = ASPxClientControl.GetControlCollection().GetByName(gridID);
            gridObject.AdjustControl();
            gridObject.RaiseInit();
            setApplicationScroll();
        }, 10);
        
    }
    function SetApplicationGridWidth(s, e) {
        if (s.IsVisible() == false)
            s.SetVisible(true);
        if (('<%=ParentControl%>').toLowerCase() == "service") {
            var selectedApplicationCount = parseInt("<%=SelectedApplicationCount %>");
            if (selectedApplicationCount == 1) {
                if (('<%=IsReadOnly%>').toLowerCase() == "true")
                    s.SetWidth($("#" + devexGrid.GetMainElement().id).width() + 190);
                else
                    s.SetWidth($("#" + devexGrid.GetMainElement().id).width() + 140);
            }
            else {
                if (('<%=IsReadOnly%>').toLowerCase() == "true")
                    s.SetWidth($("#" + devexGrid.GetMainElement().id).width() + 28);

                var gridPaneHeight = $(".divgrd").height();
                var tabHeightHeight = $(".divAppTab").height();
                if (gridPaneHeight < tabHeightHeight) {
                    s.SetHeight(tabHeightHeight - 65);
                }
            }
        }
        else if (('<%=ParentControl%>').toLowerCase() == "application") {
            s.SetWidth($(document).width() - 40);
            s.SetHeight($(document).height() - ($(".tdRoleAssignee").height() + $(".upperdiv").height() + $(".trButtons").height() + 90));
        }
        else if (('<%=ParentControl%>').toLowerCase() == "task") {
            s.SetWidth($(document).width() - ($(".ms-formlabel").width() + 85));
        }
}

function setWidthGridDiv() {
    var grdWidth = $("#grdApplModuleRoleMapAppId1").width();
    $("#divMain1").css("width", "824px");
}

function CheckProjectManager(source, arguments) {
    if (aspnetForm.ctl00_PlaceHolderMain_ProjectManager_downlevelTextBox.value == "")
        arguments.IsValid = false;
    else
        arguments.IsValid = true;
}

var notifyId = "";
function AddNotification1(msg) {
    if (notifyId != "") {
        RemoveNotification1()
    }
    notifyId = SP.UI.Notify.addNotification(msg, true);
}

function RemoveNotification1() {
    SP.UI.Notify.removeNotification(notifyId);
    notifyId = '';
}

var activeIndex = 0;
var activeAppTabPageID = "<%= appTabPage.ClientID%>";
    function tabclick(index, appTabPageID) {
        activeIndex = index;
        activeAppTabPageID = appTabPageID;
        var appTabPage = ASPxClientControl.GetControlCollection().GetByName(appTabPageID)
        if (appTabPage != null && appTabPage != '' && appTabPage != undefined) {
            appTabPage.SetActiveTabIndex(index);
            var element = appTabPage.GetContentElement(index);
            SetWidth(element);
        }
    }

    function initialiseMatrix() {
        tabclick(activeIndex, activeAppTabPageID);
    }
                var initializeCtr = true;
    $(function () {
        if (initializeCtr) {
            tabclick(activeIndex, activeAppTabPageID);
            initializeCtr = false;
        }
    });
</script>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
   

    .divColorCodeNew {
        margin: 5px;
        padding: 8px;
        float: left;
        background-color: #ffff9e;
        border: 1px solid #a8a8a8;
    }

    .divColorCodeRemove {
        margin: 5px;
        padding: 8px;
        float: left;
        background-color: #FFE7E4;
        border: 1px solid #a8a8a8;
    }

    .divColorCodeNo {
        margin: 5px;
        padding: 8px;
        float: left;
        background-color: #ffffff;
        border: 1px solid #a8a8a8;
    }

    .divColorCodeContent {
        float: left;
        margin: 5px 10px 0px 0px;
    }

    .divScrolling {
        height: 378px;
        overflow-y: scroll;
        overflow-x: hidden;
    }

    .appMatrixTitle {
        width: auto;
        margin: 0px;
        padding: 0px;
        text-align: left;
        background: #3e4f50 ;
        font-weight: bold;
    }
</style>

<div>
    <div class="upperdiv">
        <div id="divRoleAssignee" class="svcCreate_accessUsrTable">
            <asp:Label ID="lblAssignee" runat="server" Style="display: none; margin-bottom: 10px;">
            </asp:Label>
            <asp:Label ID="lblAccessRequestType" runat="server" Visible="false"></asp:Label>
        </div>
        <div class="clear"></div>
        <div style="float: left; margin: -3px 0px 0px 8px" id="divAccessChanges" runat="server" visible="false">
        </div>
        <div class="clear"></div>
        <div style="float: left;" id="divAccessRequestModes" runat="server" visible="false">
            <asp:RadioButtonList ID="rdblstAccessReqMode" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdblstAccessReqMode_SelectedIndexChanged" CssClass="rdblstAccessReqMode" ValidationGroup="accessmodes" CausesValidation="false">
            </asp:RadioButtonList>
        </div>
        <div class="clear"></div>
        <span id="spanApplicationName" runat="server" visible="false" style="margin: 14px 20px 0px 4px; float: left">
            <b>Application: </b>
            <asp:Label ID="lblApplicationName" runat="server" Style="margin-bottom: 10px;">
            </asp:Label>
        </span>
        <div style="margin: 11px 4px 0px 5px; float: left">
            <asp:CheckBox runat="server" ID="chkbxExistingAccess" OnCheckedChanged="chkbxExistingAccess_CheckedChanged" AutoPostBack="true" Text="Show Only Existing Access" />
        </div>

        <div class="applLegend row">

            <div class="divColorKeyMain col-xs-12 col-md-3">
                <div class="divColorCodeNew"></div>
                <div class="divColorCodeContent">New Access</div>
            </div>
            <div class="divColorKeyMain col-xs-12 col-md-3">
                <div class="divColorCodeRemove"></div>
                <div class="divColorCodeContent">Remove Existing Access</div>
            </div>
            <div class="divColorKeyMain col-xs-12 col-md-3">
                <div class="divColorCodeNo"></div>
                <div class="divColorCodeContent">No Change in Existing Access</div>
            </div>
            <div class="clear"></div>
        </div>
        <div class="clear"></div>
    </div>

    <div class="clear"></div>
    <div class="row">
    <div class="divMain divMainAccess removeVerticalScroll col-xs-12 col-sm-9 col-md-12">
        <div id="divAppTab" runat="server" class="divAppTab col-xs-12 col-sm-3 col-md-3">
            <div id="divAppCarousal" class="vertical-carousel" runat="server">
                <a href="javascript:" class="scru scrollup"></a>
                <div class="vertical-carousel-container" id="vertical-carousel-container">
                    <ul class="vertical-carousel-list" runat="server" id="ulAppTab">
                    </ul>
                </div>
                <a href="javascript:" class="scrd scrolldown"></a>
            </div>
        </div>
        <div id="divGrd" class="divgrd divgrd-mobile col-xs-12 col-sm-9 col-md-9" runat="server">
            <dx:ASPxPageControl ID="appTabPage" Width="100%" EnableViewState="false" TabPosition="Top" 
                runat="server" ShowTabs="false" ContentStyle-BackColor="Transparent" 
                ContentStyle-Border-BorderStyle="None" EnableTabScrolling="true" TabAlign="Justify"
                ActiveTabIndex="2" CssClass="applAccess_appTabPage">
                <TabStyle CssClass="hellouser" Paddings-PaddingLeft="40px" Paddings-PaddingRight="20px"  />
            </dx:ASPxPageControl>
            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                function UpdateGridHeight() {
                    devexGrid.SetHeight(0);
                    var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
                    if (document.body.scrollHeight > containerHeight)
                        containerHeight = document.body.scrollHeight;
                    devexGrid.SetHeight(containerHeight);
                }
                window.addEventListener('resize', function (evt) {
                    if (!ASPxClientUtils.androidPlatform)
                        return;
                    var activeElement = document.activeElement;
                    if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                        window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
                });

               
        </script>
        </div>
        <div style="clear: both; width: 0%"></div>

        <div style="margin: 5px !important">
            <asp:Label ID="lblNoAccess" runat="server" Visible="false"></asp:Label>
        </div>
    </div>
   </div>
    <asp:TextBox ID="txtHidden" runat="server" CssClass="hide hiddenCtr" ValidationGroup="ServiceMatrix" Style="display: none"></asp:TextBox>
    <asp:CustomValidator ID="csChkChangedData" runat="server" ControlToValidate="txtHidden"
        ErrorMessage="No changes made" Display="Dynamic" ValidateEmptyText="true" CssClass="errormsg-container" OnServerValidate="cvMandatoryData_ServerValidate">
    </asp:CustomValidator>
</div>

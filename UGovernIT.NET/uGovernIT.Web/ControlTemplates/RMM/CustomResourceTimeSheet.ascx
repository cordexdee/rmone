<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomResourceTimeSheet.ascx.cs" Inherits="uGovernIT.Web.CustomResourceTimeSheet" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
.action-buttons {
 width:100%;
 height:23px;
    }
    .eachdaytotal, .Total{
        font-weight:600;
        color:#000;
        font-size:11px;
    }
    .daytotal .lbvtotal {
        padding-left: 7px !important;
    }
    .Total  {
        border-top:solid 1px #bbb;
    }
    .hideRow {
        display:none;
    }
    .unHideRow {
        display: table-row!important;
}
.action-buttons ul {
 width:auto;
 float:left;
 height:25px;
 padding:0;
 overflow:hidden;
 background:#fff;
 border-top:1px solid #fff;
 margin-left:0px;
}

.action-buttons ul li,
.action-buttons ul li.tabactive,
.action-buttons ul li.tabhover {
  width:auto;
  float:left;
 display:inline;
  list-style:none;
  margin:0 1px;
  text-align:center;
  overflow:hidden;
  color:#fff;
  background:url(/content/images/firstnavbg.gif) repeat-x;
  cursor:pointer; padding: 0 5px;
}

.action-buttons ul li.tabactive {
  background:url(/content/images/firstnavbg_active.gif) repeat-x;
}
.action-buttons ul li.tabhover {
  background:url(/content/images/firstnavbg_hover.gif) repeat-x;
}

.action-buttons ul li.Red,
.action-buttons ul li.tabactiveRed,
.action-buttons ul li.tabhoverRed {
  width:auto;
  float:left;
  display:inline;
  list-style:none;
  margin:0 1px;
  text-align:center;
  overflow:hidden;
  color:#fff;
  background:url(/content/images/firstnavbgRed.png) repeat-x;
  cursor:pointer; padding: 0 5px;
}
.action-buttons ul li.tabactiveRed {
  background:url(/content/images/firstnavbg_activeRed.png) repeat-x;
}
.action-buttons ul li.tabhoverRed {
  background:url(/content/images/firstnavbg_activeRed.png) repeat-x;
}
.action-buttons ul li.Green,
.action-buttons ul li.tabactiveGreen,
.action-buttons ul li.tabhoverGreen {
  width:auto;
  float:left;
  display:inline;
  list-style:none;
  margin:0 1px;
  text-align:center;
  overflow:hidden;
  color:#fff;
  background:url(/content/images/firstnavbgGreen.png) repeat-x;
  cursor:pointer; padding: 0 5px;
}
.action-buttons ul li.tabactiveGreen {
  background:url(/content/images/firstnavbg_activeGreen.png) repeat-x;
}
.action-buttons ul li.tabhoverGreen {
  background:url(/content/images/firstnavbg_activeGreen.png) repeat-x;
}
.action-buttons ul li a,
.action-buttons ul li.tabactive a, 
.action-buttons ul li.tabhover a 
.action-buttons ul li.tabactiveRed a, 
.action-buttons ul li.tabhoverRed a 
.action-buttons ul li.tabactiveGreen a, 
.action-buttons ul li.tabhoverGreen a 
{
   float:left;
   padding:4px 21px 5px 0px !important;
   display:block;
   background-repeat: no-repeat;
   background-position: right;
   color:White;
}

.action-buttons ul li a:hover,
.action-buttons ul li.tabactive a:hover,
.action-buttons ul li.tabhover a:hover
{
    text-decoration:none;
}
    .detailviewmain {
        float: left;
        width: 100%;
        min-height: 550px;
    }

    .worksheetmessage-m {
        text-align: center;
        position: relative;
        top: -10px;
        display: none;
    }

    .worksheetmessage-m1 {
        float: left;
        padding-left: 7px;
        width: 99%;
        margin-top: 6px;
    }

    .worksheetheading-m {
    }

    .worksheetheading {
    }

    .worksheetpanel {
    }

    .worksheetpanel-m {
        float: left;
        padding: 7px;
        width: 99%;
    }

    .worksheettable {
    }

    .worksheetheader {
    }

    .paddingfirstcell {
    }

    .alncenter {
        text-align: center !important;
    }

    .editpanel {
        float: left;
    }

    .editinputwidth {
        width: 35px;
        height: 12px;
    }

    .editinputheaderwidth {
        width: 55px;
    }

    .editinputheadertotalwidth {
        width: 45px;
    }

    .editdropdownwidth {
        width: 168px;
    }

    .alnright {
        text-align: right;
    }

    .totalborderhorisontal {
        border-bottom: 1px solid #6c6e70 !important;
        font-weight: bold;
    }

    .totalbordervartical {
        border-left: 1px solid #6c6e70 !important;
        border-right: 1px solid #6c6e70 !important;
        font-weight: bold;
    }

    .detailviewheading-m {
        float: left;
        width: 98%;
        padding: 0px 12px;
    }

    .filteredcalender-m {
        float: left;
        position: relative;
        display: flex;
        justify-content: center;
    }

    .calendertxtbox {
        margin-right: 46px;
        visibility: hidden;
    }
    

    .calenderpreweekbt {
        padding-right: 4px;
        padding-top: 0px;
    }

    .calenderweektxt {
        
        color:black;
        font-family:'Roboto', sans-serif !important;
        font-size:14px;
        font-weight:500;
    }

    .action-container {
        float: left;
        width: 20px;
    }

    .message-container {
        background: yellow;
        float: left;
        padding: 4px;
    }

    .hide, .hideRow  {
        display: none;
    }
      
    .rmmaction-container {
        float: right;
    }
    .dxeEditArea_UGITBlackDevEx {
     display: none;
        }

     .disabled .dxeButtonEditButton,
        .disabled .dxeButtonEditButton_BlackGlass,
        .disabled .dxeButtonEditButton_Youthful, 
        .disabled .dxeButtonEditButton_Metropolis {
            display: none;
        }

    #ms-belltown-table {
        padding-left: 6px!important;
    }

        .clsSaveNotifyVisibility {
        display: none;
    }

    .color-rust {
        color: #d96125;
    }

    .context-popup-grid .dxpc-content {
        padding: 0 0 0 0 !important;
    }

    .stop-linking {
        text-decoration: none !important;
    }

    .ms-listviewtable .ms-vh2 {
        border: 1px solid black;
        border-left: none;
        border-right: none;
        font-size:11px;
        color:#000;
        padding-right:9px;
    }
    .ms-vb, .ms-vb2, .ms-vb-user, .ms-vb-tall, .ms-pb, .ms-pb-selected {vertical-align:middle; }
    .typegroupstart-row {
        border-top: solid 1px #bbb;
    }

    .clsweekendhighlight {
        background-color:#f1f1f1;
    }
   .paddingfirstcell {
        padding-right:20px !important;
        width:200px;
    }
    .comment {
        position: relative;
        top: -5px;
        float: right;
    }
    .dxEditors_edtDropDown_UGITNavyBlueDevEx {
    background: url(/Content/Images/icons/down-arrow-triangle.png);
    /* padding: 0px 10px; */
    background-repeat: no-repeat;
    background-color: transparent;
    margin-top: 4px;
    margin-right: 8px;
    margin-bottom: 10px;
    transform: rotate(135deg);
}
    .cmbUser {
    color:black;
    font-size:13px;
    }
    .dxeButtonEditButtonHover_UGITNavyBlueDevEx, .dxeSpinIncButtonHover_UGITNavyBlueDevEx, .dxeSpinDecButtonHover_UGITNavyBlueDevEx, .dxeSpinLargeIncButtonHover_UGITNavyBlueDevEx, .dxeSpinLargeDecButtonHover_UGITNavyBlueDevEx {
    background:none;
    }
    .prevStyle {
    transform: rotate(90deg);
    width: 11px;
    }
    .nextStyle {
    transform: rotate(270deg);
    width: 11px;
    }
    .columnStyle {
    display: flex; 
    flex-direction: column;
    padding-top:10px;
    justify-content: space-evenly;
    height:70px;
    }
    .selectedresourcelbheading {
    color: black;
    font-weight: 500 !important;
    font-family: 'Roboto', sans-serif !important;
    font-size:14px;
}
    .text-align-center {
    text-align:center;
    }
    .btnStyle {
    background: #4fa1d6;
    color: white;
    font-weight: 500;
    font-size: 14px;
    border-radius:10px;
    }
    .primary-btn-link {
    float: right;
    background: #4fa1d6;
    color: #fff;
    padding: 6px 12px;
    border-radius: 9px;
    margin-left: 8px;
    font-weight: 500;
}
    a.primary-linkBtn {
    color: #fff;
    background-color: #4fa1d6;
    padding: 7px 12px;
    font-size: 14px;
    border-radius: 9px;
    font-family: 'Roboto', sans-serif;
    font-weight: 500;
}
    .phrasesAdd-label {
    color: #4A6EE2;
    font-size: 14px;
    font-family: 'Roboto', sans-serif;
}
    .primary-btn-link img {
    filter: brightness(9);
    width: 16px;
    margin-right: 8px;
    float: left;
    margin-top: 3px;
}
    .primary-btn-link span {
    vertical-align: text-top;
    }
    .btnSave {
        background: #4fa1d6;
    }
    .btnCancel {
    border: 1px solid #4fa1d6;
    color: #4fa1d6;
}
    .calenderStyle {
    margin-top:-3px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

   var currentSelectedUser = "";
    $(document).ready(function () {
        
        var objAgent = navigator.userAgent;
        var objOffsetName;

        // If browser is Microsoft Internet Explorer 
        if ((objOffsetVersion = objAgent.indexOf("MSIE")) != -1) {
            $('#spnTimeSheetStatus').addClass('ro-table');
            $('#tblTimeSheetStatus').addClass('ro-table');
        }

        $('#statusPickerIcon').click(function () {
            ShowHideStatusGrid();
        });

        $('#pendingStatusPickerIcon').click(function () {
            ShowHidePendingStatusGrid();
        });

        //highlight current date in header 
        var header=$('.clshighlightheadercell');
        if (header) {
            $(header.parent()).css('border', '2px solid red');
        }

        var weekendheader = $('.clshighlightweekendheader');
        if (weekendheader) {
            $(weekendheader.parent()).css({ "background-color": "#f1f1f1"});
        }
    });



    function adjustControlSize() {
        setTimeout(function () {
            $("#s4-workspace").width("100%");
            var height = $(window).height();
            $("#s4-workspace").height(height);
        }, 10);
    }

    var stopEditing = false;
    var isInEditMode = false;
    var actionValue = "";
    var readyForApproval = false;
    var isActionPerform = false;

    function CalculateTotalOnFly(workHour) {
        //debugger;
        var listViewId = "<%= rTimeSheet.ClientID%>";
        var totalControl = document.getElementById(listViewId + "_lbWeekDay" + textIndex + "VTotal");
        var weekTotalControl = document.getElementById(listViewId + "_lbVTotal");
        var weekHTotalControl = document.getElementById("lbHTotalAction");
        //lbHTotalAction 
        var weekHTotal = Number(weekHTotalControl.innerHTML); calenderweektxt
        var weekTotal = Number(weekTotalControl.innerHTML);
        var oldTotal = Number(totalControl.innerHTML);
        var oldTxtVal = Number(control.getAttribute("oldVal"));
        var nexTxtVal = Number(control.value);
        var newTotal = 0;
        var workHourNumber = Number(workHour);

        if (oldTotal != NaN && nexTxtVal != NaN && oldTxtVal != NaN) {

            var newMin = (nexTxtVal * 60) % 60;
            var wholeHour = Number(control.value.split(".")[0]);

            if (newMin > 45) {
                newMin = 60;
            }
            else if (newMin > 30) {
                newMin = 45;
            }
            else if (newMin > 15) {
                newMin = 30;
            }
            else if (newMin > 0) {
                newMin = 15;
            }

            newTotal = ((wholeHour * 60) + newMin) / 60;
            //set value into current textbox
            control.value = newTotal;
            $(control).attr("oldVal", newTotal);

            //set total of current day
            totalControl.innerHTML = (oldTotal - oldTxtVal) + newTotal;

            //set total of current workitem for whole week
            weekHTotalControl.innerHTML = (weekHTotal - oldTxtVal) + newTotal;

            //set total of whole week
            weekTotalControl.innerHTML = (weekTotal - oldTxtVal) + newTotal;

        }
    }

    function checkWeekValidation() {
        var isValid = true;
        var listViewId = "<%= rTimeSheet.ClientID%>";
        for (var i = 1; i <= 7; i++) {
            var totalControl = document.getElementById(listViewId + "_lbWeekDay" + i + "VTotal");
            if (Number($(totalControl).html()) > 24) {
                var control = document.getElementById("<%=lbMessage.ClientID %>");
                control.innerHTML = "You cannot add more then 24 hours in a day";
                $(control).css("color", "red");
                setTimeout("HideMessage()", 10000);
                isValid = false;
            }
        }
        return isValid;
    }



    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_initializeRequest(InitializeRequest);
    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequest);
    var notifiedID = null;
    function InitializeRequest(sender, args) {
        //var editBoxs = $(".worksheettable input:text");
        var editBoxs = $(".weekedit input:text");

        if (editBoxs.length > 0 && $("#stopsave").attr("value") == "false") {
            if (confirm("Do you want to save time sheet?")) {
                saveWorkSheet();
            }
        }
    }


    function BeginRequestHandler(sender, args) {

        notifiedID = AddNotification("Processing ..");
    }

    function EndRequest(sender, args) {
        //do your stuff
        var s = sender;
        var a = args;
        var msg = null;
        if (a._error != null) {
            switch (args._error.name) {
                case "Sys.WebForms.PageRequestManagerServerErrorException":
                    msg = "PageRequestManagerServerErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerParserErrorException":
                    msg = "PageRequestManagerParserErrorException";
                    break;
                case "Sys.WebForms.PageRequestManagerTimeoutException":
                    msg = "PageRequestManagerTimeoutException";
                    break;
            }
            args._error.message = "My Custom Error Message " + msg;
            args.set_errorHandled(true);

        }
        else {
            RemoveNotification(notifiedID);
            $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
                addHeightToCalculateFrameHeight(this, 220);
            });

            //Put &nbsp; in action container if empty
            $.each($(".action-container"), function () {
                if ($.trim($(this).html()) == "") {
                    $(this).html("&nbsp;");
                }
            });

            if ($(".insertaction").length > 0) {
                stopEditing = true;
                $(".rmmaction-container").css("visibility", "hidden");

            }
            else {
                stopEditing = false;
                $(".rmmaction-container").css("visibility", "visible");
            }
        }
        setTimeout("HideMessage()", 10000);
       
    }

    $(function () {
        $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
            addHeightToCalculateFrameHeight(this, 220);
        });

        //Put &nbsp; in action container if empty
        $.each($(".action-container"), function () {
            if ($.trim($(this).html()) == "") {
                $(this).html("&nbsp;");
            }
        });

      


    });

    function HideMessage() {
        var control = document.getElementById("<%=lbMessage.ClientID %>");
        control.innerHTML = "";
    }


    function childClick(imgObj) {
        var jsonObj = [];
        var rTimeSheetState = $.cookie("rTimeSheetState");
        try {
            if (rTimeSheetState != undefined && rTimeSheetState != '' && rTimeSheetState!=null)
                jsonObj = JSON.parse($.cookie("rTimeSheetState"));
        } catch (e) { jsonObj = []; }

        var user = cmbCurrentUser.GetValue();

        var trRow = $(imgObj).parents("tr:eq(0)");

        var arrayClass = trRow.attr("class").split(' ');
        if (trRow.hasClass("Total")) {
            $.each(arrayClass, function (i, item) {

                if (item.indexOf("Parent") >= 0) {
                    var childcss = item.replace('Parent_', 'Child_');

                    $('.' + childcss).toggleClass("hideRow");
                    var temp = { "user": user, "workItem": childcss };

                    var index = checkExist(jsonObj, temp);
                    if ($('.' + childcss + ".hideRow").length > 0) {
                        if (index === -1)
                            jsonObj.push(temp);
                        $(imgObj).attr('src', "/Content/Images/plus_16x16.png");
                    }
                    else {
                        if (index >= 0)
                            jsonObj.splice(index, 1);

                        $(imgObj).attr('src', "/Content/Images/minus_16x16.png");
                    }
                } 
            });
        }

       //change expand and collapse button
       
        $.cookie("rTimeSheetState", JSON.stringify(jsonObj), { path: $(location).attr('href')});
    }

    function checkExist(arrayJson, item)
    {
        var index = -1;
        if (arrayJson.length>0)
        {
            $.each(arrayJson, function (key, items) {
                if (items.user == item.user && items.workItem == item.workItem)
                    index= key;
            });
           
        }
        return index;
    }

    function AddTicketHoursComment(objID, objComment, objCommentListName) {
        $("#<%=hdnCommentId.ClientID %>").val(objID);
        $("#<%=txtComment.ClientID %>").val(objComment.replaceAll('<br/>', '\n'));
        $("#<%=hdnCommentListName.ClientID %>").val(objCommentListName);
        pcAddComment.Show();
    }

    function ShowCommentIcon(objthis) {
        $(objthis).find("div").css("visibility", "visible");
    }

    function HideCommentIcon(objthis) {
        $(objthis).find("div").css("visibility", "hidden");
    }

    function OpenCommentDialog() {
            pcAddComment.PerformCallback($("#<%=hdnCommentId.ClientID %>").val());
            pcAddComment.Hide();
    }

    $(document).ready(function () {
        $('tr.Total td:contains("Total")').css('color', 'transparent');
    });
</script>

<asp:HiddenField ID="hdnCommentListName" runat="server" />
<asp:HiddenField ID="hdnCommentId" runat="server" />
<dx:ASPxLoadingPanel ID="loadingPanel" runat="server" CssClass="customeLoader" Text="Loading..." ClientInstanceName="loadingPanel" Image-Url="~/Content/IMAGES/AjaxLoader.gif" ImagePosition="Top"
    Modal="True">
</dx:ASPxLoadingPanel>
<table cellpadding="0" cellspacing="0" style="border-collapse: collapse;" width="98%">
    <tr>
        <td>
            <div style="display: none;">
                <dx:ASPxDateEdit ID="DateTimeControl1" runat="server"></dx:ASPxDateEdit>
            </div>
            <div class="detailviewmain">
              <div class="row">
                  <div class="col-md-3 col-sm-4 col-xs-6 columnStyle">
                      <asp:Label ID="lbSearchViewDrop" CssClass="fleft selectedresourcelbheading mb-2" runat="server"
                          Text="Resource:" ></asp:Label>
                      <dx:ASPxComboBox ID="cmbCurrentUser" DropDownHeight="220px" OnLoad="cmbCurrentUser_Load" OnSelectedIndexChanged="cmbCurrentUser_SelectedIndexChanged" EnableClientSideAPI="true" AutoPostBack="true" ClientInstanceName="cmbCurrentUser" runat="server" Width="270px" CssClass="cmbUser"></dx:ASPxComboBox>
                  </div>
                  <div class="col-md-4 col-sm-4 col-xs-6 columnStyle">
                      <asp:Label ID="lbStartDate" CssClass="fleft selectedresourcelbheading mb-2 text-align-center" Font-Bold="true" runat="server"
                          Text="Week:"></asp:Label>
                      <div class="filteredcalender-m">
                          <span class="calenderpreweekbt mr-2">
                              <asp:ImageButton ImageUrl="/content/images/icons/down-arrow-triangle.png" ID="previousWeek" runat="server" OnClientClick="javascript:loadingPanel.Show();" OnClick="PreviousWeek_Click" CssClass="prevStyle" />
                          </span>
                          <asp:Label CssClass="calenderweektxt mr-2" ID="lbWeekDuration" runat="server"></asp:Label>
                          
                          <span class="calendernextweekbt mr-2">
                              <asp:ImageButton ImageUrl="/content/images/icons/down-arrow-triangle.png" ID="nextWeek" runat="server" OnClientClick="javascript:loadingPanel.Show();" OnClick="NextWeek_Click" CssClass="nextStyle" />
                          </span>
                          <span style="float: right; padding-left: 5px;">
                              <dx:ASPxPopupControl ID="calPopup" runat="server" ClientInstanceName="calPopup" PopupHorizontalAlign="WindowCenter" HeaderText="Select Date" PopupVerticalAlign="TopSides">
                                  <ContentCollection>
                                      <dx:PopupControlContentControl>
                                          <asp:Panel ID="panelCal" runat="server">
                                              <dx:ASPxCalendar ID="dtcStartdate" runat="server" AutoPostBack="true">
                                                  <ClientSideEvents SelectionChanged="function(s,e){calPopup.Hide();}" />
                                              </dx:ASPxCalendar>
                                              <%--    <dx:ASPxDateEdit AutoPostBack="true" ID="dtcStartdate" PopupHorizontalAlign="Center" PopupVerticalAlign="WindowCenter"  CalendarProperties-FastNavProperties-DisplayMode="Popup"   ClientInstanceName="dtcStartdate" CssClassTextBox="calendertxtbox datetimectr111" runat="server" DateOnly="true" >
                                                                           <DisabledStyle CssClass="disabled" Border-BorderStyle="None"></DisabledStyle> 
                                                                          <ClientSideEvents Init="function (s, e) {  s.ShowDropDown(); }" />
                                                                      </dx:ASPxDateEdit>--%>
                                          </asp:Panel>
                                      </dx:PopupControlContentControl>
                                  </ContentCollection>
                              </dx:ASPxPopupControl>
                          </span>
                          <span>
                              <dx:ASPxImage ID="btnPcKalenderOk" ClientInstanceName="btnPcKalenderOk" Width="19" CssClass="calenderStyle" runat="server" ImageUrl="/Content/Images/icons/calender-new.png">
                                  <ClientSideEvents Click="function(s,e){calPopup.Show();}" />
                              </dx:ASPxImage>
                          </span>
                          <asp:HiddenField ID="startWeekDateForEdit" runat="server" />
                      </div>
                  </div>
                  <div class="col-md-4" style="margin-top:34px;">
                      <a id="statusPicker" runat="server" class="btn btn-default btnStyle" visible="true">
                          <span src="/Content/Images/add-group.jpg" id="statusPickerIcon" title="Timesheet Status For My Team" style="height: 16px; width: 16px; cursor: pointer;">Team Time Sheet</span></a>
                      <a id="pendingStatusPicker" runat="server" class="btn btn-default btnStyle" visible="false">
                          <span src="/Content/ButtonImages/schedule.png" id="pendingStatusPickerIcon" title="Timesheets Pending Approval" style="cursor: pointer;" >Pending Time Sheets</span></a>
                  </div>
              </div>

                        <div class="worksheetmessage-m1">
                            <div>
                                <table cellpadding="2" cellspacing="0" class="bordercolps w-100">
                                    <tr>
                                     
                                       <td>
                                            <div style="float: right; padding-right: 7px;">
                                                <div style="float: left; margin-right: 20px;">
                                                    <asp:Label ID="lblStatus" runat="server" Font-Bold="true" Text="Status: "></asp:Label>
                                                    <asp:HyperLink ID="timeSheetStatus" runat="server" CssClass="" Font-Bold="true" Text="" NavigateUrl="javascript:ShowSignOffHistory();"></asp:HyperLink>
                                                </div>
                                                <img style="cursor: pointer; border: none; width: 16px;" src="/Content/Images/print-icon.png" alt="PDF" title="Print" onclick="PrintPanel();" />
                                              </div>
                                        </td>

                                    </tr>
                                </table>
                            </div>
                            <div>
                                <asp:Label ID="lbMessage" runat="server" Text="" ForeColor="Blue"></asp:Label>
                            </div>
                        </div>
                        <div class="worksheetheading-m">
                            <asp:Label ID="lbDetailViewHeading" CssClass="worksheetheading" runat="server"></asp:Label>
                        </div>
                   
                        <asp:Panel ID="workSheetPanel" CssClass="worksheetpanel" runat="server" OnLoad="DetailViewPanel_Load">

                                    <div class="worksheetpanel-m">
                                        <asp:HiddenField ID="sortingExp" runat="server" />
                                        <asp:HiddenField ID="StartWeekDate" runat="server" />

                                        <asp:ListView ID="rTimeSheet" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1"
                                            DataKeyNames="WorkItemID"
                                            OnItemEditing="RTimeSheet_ItemEditing" OnItemCanceling="RTimeSheet_ItemCanceling"
                                            OnItemUpdating="RTimeSheet_ItemUpdating" OnItemDataBound="rTimeSheet_ItemDataBound"
                                            OnItemCommand="RTimeSheet_ItemCommand" OnItemDeleting="RTimeSheet_ItemDeleting"
                                            OnSorting="RTimeSheet_Sorting">
                                            <LayoutTemplate>
                                            <table class="ms-listviewtable worksheettable" style="border-collapse: collapse" width="100%" cellpadding="0" cellspacing="0">
                                                <tr class="worksheetheader ms-viewheadertr" >
                                                   <th class="ms-vh2 paddingfirstcell" style="font-weight: bold;">
                                                       <asp:LinkButton ID="lbType" runat="server" Text="Type" Enabled="false" CssClass="stop-linking" />
                                                        
                                                    </th>
                                                    <th class="ms-vh2 paddingfirstcell" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWorkItem" runat="server" Text="Type" Enabled="false" CssClass="stop-linking" />
                                                    </th>
                                                    
                                                    <th class="ms-vh2 paddingfirstcell" id="subWorkItemHead" visible="false" runat="server" style="font-weight: bold;">
                                                         <asp:LinkButton ID="lbSubWorkItem" runat="server" Text="Work Item" Enabled="false" CssClass="stop-linking" />
                                                    </th>

                                                    <th class="ms-vh2 paddingfirstcell" id="subSubWorkItemHead" visible="false" runat="server" style="font-weight: bold;">
                                                         <asp:LinkButton ID="lbSubSubWorkItem" runat="server" Text="Work Item" Enabled="false" CssClass="stop-linking" />
                                                    </th>

                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay1" CssClass="weekheader day1 stop-linking" runat="server" Text="Mon" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay2" CssClass="weekheader day2 stop-linking" runat="server" Text="Tue" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay3" CssClass="weekheader day3 stop-linking" runat="server" Text="Wed" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay4" CssClass="weekheader day4 stop-linking" runat="server" Text="Thu" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay5" CssClass="weekheader day5 stop-linking" runat="server" Text="Fri" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay6" CssClass="weekheader day6 stop-linking" runat="server" Text="Sat" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter editinputheaderwidth" style="font-weight: bold;">
                                                        <asp:LinkButton ID="lbWeekDay7" CssClass="weekheader day7 stop-linking" runat="server" Text="Sun" Enabled="false" />
                                                    </th>
                                                    <th class="ms-vh2 alncenter totalbordervartical editinputheadertotalwidth">
                                                        <asp:LinkButton ID="lbHorizontalTotal" runat="server" Text="Total" Enabled="false" CssClass="stop-linking" />
                                                    </th>

                                                    <th class="ms-vh2 alncenter totalbordervartical editinputheadertotalwidth">
                                                        <asp:Label ID="lblERH" runat="server" Text="ERH" ForeColor="#777777"></asp:Label>
                                                    </th>
                                                    <th class="ms-vh2 alncenter" width="50">
                                                        <a href="javascript:Void(0)">&nbsp;</a>
                                                    </th>

                                                </tr>
                                                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                                                <tr id="trSubWorkItemTotal1" runat="server" style="display: none;">
                                                    <td class="ms-vb2 paddingfirstcell" colspan="2"></td>
                                                    <td class="ms-vb2 paddingfirstcell" id="subWorkItemTotal1" runat="server" visible="false"></td>
                                                    <td class="ms-vb2" colspan="7">&nbsp;</td>
                                                    <td class="totalbordervartical"></td>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr id="trSubWorkItemTotal2" runat="server" style="display: none;">
                                                    <td class="ms-vb2 totalborderhorisontal" colspan="2"></td>
                                                    <td class="ms-vb2 totalborderhorisontal" id="subWorkItemTotal2" runat="server" visible="false"></td>
                                                    <td class="ms-vb2 totalborderhorisontal" colspan="7"></td>
                                                    <td class="alncenter totalborderhorisontal totalbordervartical">&nbsp;</td>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr class="eachdaytotal">
                                                    <td class="ms-vh2 paddingfirstcell totalborderhorisontal" style="font-weight: bold;" colspan="2"><%--Total--%></td>
                                                    <%--<td class="ms-vh2 totalborderhorisontal" style="font-weight: bold;" visible="false" id="subWorkItemTotal3" runat="server">Total</td>--%>
                                                    <td class="ms-vh2 totalborderhorisontal" style="font-weight: bold;text-align:right" visible="false" id="subWorkItemTotal3" runat="server"></td>
                                                    <td class="ms-vh2 totalborderhorisontal" style="font-weight: bold;text-align:right" visible="false" id="subSubWorkItemTotal" runat="server"></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay1VTotal" CssClass="day1total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay2VTotal" CssClass="day2total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay3VTotal" CssClass="day3total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay4VTotal" CssClass="day4total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay5VTotal" CssClass="day5total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay6VTotal" CssClass="day6total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbWeekDay7VTotal" CssClass="day7total daytotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal totalbordervartical">
                                                        <asp:Label CssClass="lbvtotal" ID="lbVTotal" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">
                                                        <asp:Label ID="lbVTotalAction" runat="server"></asp:Label></td>
                                                    <td class="ms-vh2 alncenter totalborderhorisontal">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="workSheetRow" runat="server" class='worksheetrow' ondblclick="editItemOnbdClick(this,event);">
                                                <td class="ms-vb2 paddingfirstcell ">
                                                    <div style="display: none;" class="workitemkey"><%#  Eval("WorkItemID")%></div>
                                                    <div style="display: none;" class="workitemtype"><%#  Eval("TypeName")%></div>
                                                    <div style="display: none;" class="workitemname"><%#  Eval("WorkItem")%></div>
                                                    <div style="display: none;" class="subworkitem"><%#  Eval("OriginalSubWorkItem")%></div>
                                                    <div style="display: none;" class="subsubworkitem"><%#  Eval("SubSubWorkItem")%></div>

                                                    <asp:HiddenField ID="hdnworkitemtype" runat="server" Value='<%#  Eval("TypeName")%>' />
                                                    <asp:HiddenField ID="hdnworkitemname" runat="server" Value='<%#  Eval("WorkItem")%>' />
                                                    <asp:HiddenField ID="hdnsubworkitem" runat="server" Value='<%#  Eval("OriginalSubWorkItem")%>' />
                                                    <asp:HiddenField ID="hdnsubSubworkitem" runat="server" Value='<%#  Eval("SubSubWorkItem")%>' />

                                                    <asp:HiddenField ID="hdnSubworkItemTotal" runat="server" Value='<%#  Eval("SubWorkItem")%>' />
                                                    <asp:HiddenField ID="hdnListName" runat="server" Value='<%#  Eval("ListName")%>'  />
                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>

                                                </td>
                                                <td class="ms-vb2 paddingfirstcell" style="position: relative; width:200px;">      
                                                      <asp:Label ID="lblWorkItem" runat="server"><%#SetExpandCollapseIcon()%>&nbsp;<%# Eval("WorkItemLink")%>
                                                                    <div style="font-size: 11px;float:left;width:100%;"><%# UGITUtility.TruncateWithEllipsis(Convert.ToString(Eval(DatabaseObjects.Columns.Title)), 100)%></div>
                                                    </asp:Label>

                                         
                                                </td>
                                                <td class="ms-vb2 paddingfirstcell" visible="false" id="subWorkItemItem" runat="server"><%# Eval("SubWorkItemLink")%></td>
                                                <td class="ms-vb2 paddingfirstcell" visible="false" id="subSubWorkItemItem" runat="server"><%# Eval("SubSubWorkItemLink")%> &nbsp; <%#SetWorkItemIcon()%></td>
                                                <td id="tdday1" runat="server" class='ms-vb2 alncenter ' daynum="day1"><%# Eval("WeekDay1")%>
                                                     <asp:HiddenField ID="hdnWeekDays1" runat="server" Value='<%#  Eval("WeekDay1")%>' />
                                                     <asp:HiddenField ID="hdbID1" runat="server" Value='<%#  Eval("ID1")%>' />
                                                     <asp:HiddenField ID="hdnComment1" runat="server" Value='<%#  Eval("Comment1")%>' />
                                                    <div id="divCommentDay1" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment1")).Replace("\"", "\\\""))%>" alt="Help" />
                                                        </div>
                                                </td>
                                                <td id="tdday2" runat="server" class='ms-vb2 alncenter ' daynum="day2"><%# Eval("WeekDay2")%>
                                                     <asp:HiddenField ID="hdnWeekDays2" runat="server" Value='<%#  Eval("WeekDay2")%>' />
                                                     <asp:HiddenField ID="hdbID2" runat="server" Value='<%#  Eval("ID2")%>' />
                                                     <asp:HiddenField ID="hdnComment2" runat="server" Value='<%#  Eval("Comment2")%>' />
                                                     <div id="divCommentDay2" runat="server" style="margin-top:-12px;" >
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment2")).Replace("\"", "\\\""))%>" alt="Help" />
                                                        </div>
                                                </td>
                                                <td id="tdday3" runat="server" class='ms-vb2 alncenter ' daynum="day3"><%# Eval("WeekDay3")%>
                                                     <asp:HiddenField ID="hdnWeekDays3" runat="server" Value='<%#  Eval("WeekDay3")%>' />
                                                     <asp:HiddenField ID="hdbID3" runat="server" Value='<%#  Eval("ID3")%>' />
                                                      <asp:HiddenField ID="hdnComment3" runat="server" Value='<%#  Eval("Comment3")%>' />
                                                     <div id="divCommentDay3" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment3")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday4" runat="server" class='ms-vb2 alncenter ' daynum="day4"><%# Eval("WeekDay4")%>
                                                     <asp:HiddenField ID="hdnWeekDays4" runat="server" Value='<%#  Eval("WeekDay4")%>' />
                                                     <asp:HiddenField ID="hdbID4" runat="server" Value='<%#  Eval("ID4")%>' />
                                                      <asp:HiddenField ID="hdnComment4" runat="server" Value='<%#  Eval("Comment4")%>' />
                                                     <div id="divCommentDay4" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment4")).Replace("\"", "\\\""))%>" alt="Help" />
                                                        </div>
                                                </td>
                                                <td id="tdday5" runat="server" class='ms-vb2 alncenter ' daynum="day5"><%# Eval("WeekDay5")%>
                                                     <asp:HiddenField ID="hdnWeekDays5" runat="server" Value='<%#  Eval("WeekDay5")%>' />
                                                     <asp:HiddenField ID="hdbID5" runat="server" Value='<%#  Eval("ID5")%>' />
                                                      <asp:HiddenField ID="hdnComment5" runat="server" Value='<%#  Eval("Comment5")%>' />
                                                     <div id="divCommentDay5" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment5")).Replace("\"", "\\\""))%>" alt="Help" />
                                                        </div>
                                                </td>
                                                <td id="tdday6" runat="server" class='ms-vb2 alncenter ' daynum="day6"><%# Eval("WeekDay6")%>
                                                     <asp:HiddenField ID="hdnWeekDays6" runat="server" Value='<%#  Eval("WeekDay6")%>' />
                                                     <asp:HiddenField ID="hdbID6" runat="server" Value='<%#  Eval("ID6")%>' />
                                                      <asp:HiddenField ID="hdnComment6" runat="server" Value='<%#  Eval("Comment6")%>' />
                                                     <div id="divCommentDay6" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment6")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday7" runat="server" class='ms-vb2 alncenter ' daynum="day7"><%# Eval("WeekDay7")%>
                                                     <asp:HiddenField ID="hdnWeekDays7" runat="server" Value='<%#  Eval("WeekDay7")%>' />
                                                     <asp:HiddenField ID="hdbID7" runat="server" Value='<%#  Eval("ID7")%>' />
                                                      <asp:HiddenField ID="hdnComment7" runat="server" Value='<%#  Eval("Comment7")%>' />
                                                     <div id="divCommentDay7" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment7")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td class="ms-vb2 alncenter totalbordervartical lbhtotal"><%# (double)Eval("WeekDay1") + (double)Eval("WeekDay2") + (double)Eval("WeekDay3") + (double)Eval("WeekDay4") + (double)Eval("WeekDay5") + (double)Eval("WeekDay6") + (double)Eval("WeekDay7")%></td>
                                                <td class="ms-vb2 alncenter totalbordervartical lbhremaining" id="Td1" runat="server"><%# UGITUtility.StringToDouble(Eval("EstimatedRemainingHours")) == 0 ? "-" : Convert.ToString(Eval("EstimatedRemainingHours")) %></td>
                                                <td class="ms-vb2 alncenter">
                                                    <span class="alncenter" onclick="return confirm('Are you sure you want to delete these hours?')">
                                                        <asp:ImageButton runat="server" ID="lnkDelete" Visible='<%# (bool)Eval("ShowDeleteButton") %>' CommandName="Delete" ImageUrl="/Content/images/delete-icon-new.png" BorderWidth="0" ToolTip="Delete" />
                                                    </span>
                                                </td>

                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                             <tr id="workSheetRow" runat="server" class='worksheetrow ms-alternatingstrong' ondblclick="editItemOnbdClick(this,event);">
                                                <td class="ms-vb2 paddingfirstcell ">
                                                    <div style="display: none;" class="workitemkey"><%#  Eval("WorkItemID")%></div>
                                                    <div style="display: none;" class="workitemtype"><%#  Eval("TypeName")%></div>
                                                    <div style="display: none;" class="workitemname"><%#  Eval("WorkItem")%></div>
                                                    <div style="display: none;" class="subworkitem"><%#  Eval("OriginalSubWorkItem")%></div>
                                                    <div style="display: none;" class="subsubworkitem"><%#  Eval("SubSubWorkItem")%></div>
                                                    <asp:HiddenField ID="hdnworkitemtype" runat="server" Value='<%#  Eval("TypeName")%>' />
                                                    <asp:HiddenField ID="hdnworkitemname" runat="server" Value='<%#  Eval("WorkItem")%>' />
                                                    <asp:HiddenField ID="hdnsubworkitem" runat="server" Value='<%#  Eval("OriginalSubWorkItem")%>' />
                                                    <asp:HiddenField ID="hdnsubSubworkitem" runat="server" Value='<%#  Eval("SubSubWorkItem")%>' />
                                                    <asp:HiddenField ID="hdnSubworkItemTotal" runat="server" Value='<%#  Eval("SubWorkItem")%>' />
                                                    <asp:HiddenField ID="hdnListName" runat="server" Value='<%#  Eval("ListName")%>' />
                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>' Font-Bold="true"></asp:Label>

                                                      
                                                   
                                                </td>
                                                <td class="ms-vb2 paddingfirstcell" style="position: relative; width:200px;">    
                                                    <asp:Label ID="lblWorkItem" runat="server"><%# SetExpandCollapseIcon()%>&nbsp;<%# Eval("WorkItemLink")%>
                                                                    <div style="font-size:11px;float:left;width:100%;"><%# UGITUtility.TruncateWithEllipsis(Convert.ToString(Eval(DatabaseObjects.Columns.Title)), 100)%></div>
                                                    </asp:Label>
                                                  
                                                </td>
                                                <td class="ms-vb2 paddingfirstcell" visible="false" id="subWorkItemItem" runat="server"><%# Eval("SubWorkItemLink")%></td>
                                                <td class="ms-vb2 paddingfirstcell" visible="false" id="subSubWorkItemItem" runat="server"><%# Eval("SubSubWorkItemLink")%> &nbsp; <%#SetWorkItemIcon()%></td>

                                                <td id="tdday1" runat="server" class='ms-vb2 alncenter  ' daynum="day1"><%# Eval("WeekDay1")%>
                                                     <asp:HiddenField ID="hdnWeekDays1" runat="server" Value='<%#  Eval("WeekDay1")%>' />
                                                     <asp:HiddenField ID="hdbID1" runat="server" Value='<%#  Eval("ID1")%>' />
                                                      <asp:HiddenField ID="hdnComment1" runat="server" Value='<%#  Eval("Comment1")%>' />
                                                    <div id="divCommentDay1" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment1")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday2" runat="server" class='ms-vb2 alncenter  ' daynum="day2"><%# Eval("WeekDay2")%>
                                                     <asp:HiddenField ID="hdnWeekDays2" runat="server" Value='<%#  Eval("WeekDay2")%>' />
                                                     <asp:HiddenField ID="hdbID2" runat="server" Value='<%#  Eval("ID2")%>' />
                                                      <asp:HiddenField ID="hdnComment2" runat="server" Value='<%#  Eval("Comment2")%>' />
                                                    <div id="divCommentDay2" runat="server" style="margin-top:-12px;">

                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment2")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday3" runat="server" class='ms-vb2 alncenter  ' daynum="day3"><%# Eval("WeekDay3")%>
                                                     <asp:HiddenField ID="hdnWeekDays3" runat="server" Value='<%#  Eval("WeekDay3")%>' />
                                                     <asp:HiddenField ID="hdbID3" runat="server" Value='<%#  Eval("ID3")%>' />
                                                      <asp:HiddenField ID="hdnComment3" runat="server" Value='<%#  Eval("Comment3")%>' />
                                                    <div id="divCommentDay3" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment3")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday4" runat="server" class='ms-vb2 alncenter  ' daynum="day4"><%# Eval("WeekDay4")%>
                                                     <asp:HiddenField ID="hdnWeekDays4" runat="server" Value='<%#  Eval("WeekDay4")%>' />
                                                     <asp:HiddenField ID="hdbID4" runat="server" Value='<%#  Eval("ID4")%>' />
                                                      <asp:HiddenField ID="hdnComment4" runat="server" Value='<%#  Eval("Comment4")%>' />
                                                    <div id="divCommentDay4" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment4")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday5" runat="server" class='ms-vb2 alncenter  ' daynum="day5"><%# Eval("WeekDay5")%>
                                                     <asp:HiddenField ID="hdnWeekDays5" runat="server" Value='<%#  Eval("WeekDay5")%>' />
                                                     <asp:HiddenField ID="hdbID5" runat="server" Value='<%#  Eval("ID5")%>' />
                                                      <asp:HiddenField ID="hdnComment5" runat="server" Value='<%#  Eval("Comment5")%>' />
                                                    <div id="divCommentDay5" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment5")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday6" runat="server" class='ms-vb2 alncenter  ' daynum="day6"><%# Eval("WeekDay6")%>
                                                     <asp:HiddenField ID="hdnWeekDays6" runat="server" Value='<%#  Eval("WeekDay6")%>' />
                                                     <asp:HiddenField ID="hdbID6" runat="server" Value='<%#  Eval("ID6")%>' />
                                                      <asp:HiddenField ID="hdnComment6" runat="server" Value='<%#  Eval("Comment6")%>' />
                                                    <div id="divCommentDay6" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment6")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td id="tdday7" runat="server" class='ms-vb2 alncenter  ' daynum="day7"><%# Eval("WeekDay7")%>
                                                     <asp:HiddenField ID="hdnWeekDays7" runat="server" Value='<%#  Eval("WeekDay7")%>' />
                                                     <asp:HiddenField ID="hdbID7" runat="server" Value='<%#  Eval("ID7")%>' />
                                                      <asp:HiddenField ID="hdnComment7" runat="server" Value='<%#  Eval("Comment7")%>' />
                                                    <div id="divCommentDay7" runat="server" style="margin-top:-12px;">
                                                     <img class="action-description comment" src='/Content/buttonimages/comments.png'
                                                                title="<%# Server.HtmlEncode(Convert.ToString(Eval("Comment7")).Replace("\"", "\\\""))%>" alt="Help"  />
                                                        </div>
                                                </td>
                                                <td class="ms-vb2 alncenter totalbordervartical lbhtotal"><%# (double)Eval("WeekDay1") + (double)Eval("WeekDay2") + (double)Eval("WeekDay3") + (double)Eval("WeekDay4") + (double)Eval("WeekDay5") + (double)Eval("WeekDay6") + (double)Eval("WeekDay7")%></td>
                                                <td class="ms-vb2 alncenter totalbordervartical lbhremaining" id="Td1" runat="server"><%# UGITUtility.StringToDouble(Eval("EstimatedRemainingHours")) == 0 ? "-" : Convert.ToString(Eval("EstimatedRemainingHours"))%></td>
                                                <td class="ms-vb2 alncenter">
                                                    <span class="alncenter" onclick="return confirm('Are you sure you want to delete these hours?')">
                                                        <asp:ImageButton runat="server" ID="lnkDelete" Visible='<%# (bool)Eval("ShowDeleteButton") %>' CommandName="Delete" ImageUrl="/Content/images/delete-icon-new.png" BorderWidth="0" ToolTip="Delete" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EditItemTemplate>
                                            <tr>
                                                <td class="ms-vb2 paddingfirstcell "><%# Eval("Type")%></td>
                                                <td class="ms-vb2 paddingfirstcell ">
                                                    <%# Eval("WorkItem")%>
                                                    <div style="font-size: smaller;"><%# Eval("Title")%></div>
                                                </td>
                                                <td class="ms-vb2 paddingfirstcell" visible="false" id="subWorkItemEdit" runat="server"><%# Eval("SubWorkItemLink")%></td>
                                                <td class="ms-vb2 alncenter">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(1,this)" oldVal='<%# Eval("WeekDay1")%>' ID="txtWeekDay1" runat="server" Text='<%# Eval("WeekDay1")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(2,this)" oldVal='<%# Eval("WeekDay2")%>' ID="txtWeekDay2" runat="server" Text='<%# Eval("WeekDay2")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(3,this)" oldVal='<%# Eval("WeekDay3")%>' ID="txtWeekDay3" runat="server" Text='<%# Eval("WeekDay3")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(4,this)" oldVal='<%# Eval("WeekDay4")%>' ID="txtWeekDay4" runat="server" Text='<%# Eval("WeekDay4")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(5,this)" oldVal='<%# Eval("WeekDay5")%>' ID="txtWeekDay5" runat="server" Text='<%# Eval("WeekDay5")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(6,this)" oldVal='<%# Eval("WeekDay6")%>' ID="txtWeekDay6" runat="server" Text='<%# Eval("WeekDay6")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter ">
                                                    <asp:TextBox CssClass="editinputwidth alncenter" onChange="javascript:CalculateTotalOnFly(7,this)" oldVal='<%# Eval("WeekDay7")%>' ID="txtWeekDay7" runat="server" Text='<%# Eval("WeekDay7")%>'></asp:TextBox></td>
                                                <td class="ms-vb2 alncenter totalbordervartical">
                                                    <span id="lbHTotalAction"><%# (double)Eval("WeekDay1") + (double)Eval("WeekDay2") + (double)Eval("WeekDay3") + (double)Eval("WeekDay4") + (double)Eval("WeekDay5") + (double)Eval("WeekDay6") + (double)Eval("WeekDay7")%></span>
                                                </td>

                                                <td class="ms-vb2 alncenter">
                                                    <span>
                                                        <asp:ImageButton ID="btnUpdate" runat="server" OnClientClick="return checkWeekValidation()" ImageUrl="/Content/images/save-icon.png" BorderWidth="0" CommandName="Update" />
                                                    </span>
                                                    <asp:LinkButton ID="btnCancel" runat="server" Text="<img style='border:0px;' src='/Content/images/cancel-icon.png' alt='Cancel'/>" CommandName="Cancel" />
                                                </td>

                                            </tr>
                                        </EditItemTemplate>

                                        </asp:ListView>

                                        <div class="fleft pt-2">
                                            <a id="aAddItem" class="primary-btn-link pull-left ml-0" runat="server" href="">
                                                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" style="border: none;" />
                                                <asp:Label ID="LblAddItem" CssClass="phrasesAdd-label" runat="server" Text="New Work Item"></asp:Label>
                                            </a>
                                             <dx:ASPxButton ID="btCopyTimeSheet" Text="Copy Previous" runat="server" CssClass="primary-linkBtn ml-2" RenderMode="Link" AutoPostBack="false" ToolTip="Copy work item(s) from the previous week's timesheet" >
                                                <Image Url="/Content/Images/duplicate.png" Width="18"></Image>
                                                 <ClientSideEvents Click="function(s,e){copyPreviousWeekWorkSheet();}" />
                                            </dx:ASPxButton>
                                        </div>
                                         <asp:Panel ID="timeSheetActions" runat="server" CssClass="rmmaction-container">
                                            <div id="createButtons">
                                                <ul class="d-flex mt-2 list-unstyled">
                                                    <li runat="server" id="btEditTimesheetLI">
                                                        <asp:HyperLink ID="btEditTimesheet" CssClass="btnSave" runat="server" Text="Edit" NavigateUrl="javascript:editWorkSheet();"></asp:HyperLink>
                                                    </li>
                                                    <li runat="server" id="btSaveTimesheetLI" style="display: none;">
                                                        <asp:HyperLink ID="btSaveTimesheet" CssClass="btnSave" runat="server" Text="Save" NavigateUrl="javascript:loadingPanel.Show();saveWorkSheet();"></asp:HyperLink>
                                                    </li>
                                                    <li runat="server" id="btCancelEditingLI" style="display: none;">
                                                        <asp:HyperLink ID="btCancelEditing" CssClass="btnCancel" runat="server" Text="Cancel" NavigateUrl="javascript:cancelWorkSheet();"></asp:HyperLink>
                                                    </li>

                                                      <li runat="server" id="btnReturnTimeSheetLI" style="display:Block;">
                                                        <asp:HyperLink ID="btnReturnTimeSheet" CssClass="btnCancel" runat="server" Text="Return" NavigateUrl="javascript:UnLockWorkSheet();" ></asp:HyperLink>
                                                    </li>

                                                   <li runat="server" id="btnSendApprovalLI" style="display: Block;">
                                                        <asp:HyperLink ID="btnSendApproval" CssClass="btnCancel" runat="server" Text="Send For Approval" NavigateUrl="javascript:SendForApproval();"></asp:HyperLink>
                                                   </li>

                                                   <li runat="server" id="btnApporvedLI" style="display: Block;">
                                                        <asp:HyperLink ID="btnApporved" CssClass="btnCancel" runat="server" Text="Approve " NavigateUrl="javascript:Approved();"  ></asp:HyperLink>
                                                   </li>
                                                    <li runat="server" id="btnSignOffLI" style="display: Block;">
                                                        <asp:HyperLink ID="btSignOff" CssClass="btnCancel" runat="server" Text="Sign Off " NavigateUrl="javascript:SignOff();" ></asp:HyperLink>
                                                   </li>

                                                    <asp:Button ID="btRefresh" runat="server" CssClass="hide btrefresh" />
                                                    <div id="stopsave" value="false" style="display: none;"></div>
                                                </ul>
                                            </div>
                                        </asp:Panel>

                                        <dx:ASPxPopupControl ID="popAllocation" HeaderText="Add Allocation" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ResizingMode="Live" AllowResize="true" RenderIFrameForPopupElements="True" Modal="true" ClientInstanceName="popAllocation" runat="server">
                                            <SettingsLoadingPanel Enabled="true" />                                             
                                            <ContentCollection>
                                                <dx:PopupControlContentControl runat="server">
                                                    <dx:ContentControl runat="server" ID="popContainer"></dx:ContentControl>
                                                </dx:PopupControlContentControl>  
                                            </ContentCollection>                                  
                                        </dx:ASPxPopupControl>

                                    </div>
                                </asp:Panel>
                
            </div>
        </td>
    </tr>
</table>

<asp:Panel ID="printworkSheetPanel" runat="server" Style="display: none;">

    <table style="width: 100%;">
        <tr>
            <td colspan="3">
                <asp:Image ID="imgCompanyLogo" runat="server" />
            </td>
        </tr>
        <tr style="height: 10px;">
            <td colspan="3"></td>
        </tr>
        <tr>
            <td width="400">
                <asp:Label ID="lblResourceName" Font-Bold="true" runat="server" Text="Resource:"></asp:Label>
                <asp:Label ID="lblRescourceManagerName" runat="server" Style="padding-left: 20px;"></asp:Label>
            </td>
            <td width="300">
                <div style="padding-left: 10px;">
                    <asp:Label ID="Label1" Font-Bold="true" runat="server" Style="float: left; padding-bottom: 5px;" Text="Week:"></asp:Label>
                    <div style="float: left; padding-left: 10px;">
                        <asp:Label ID="LblWeekDurationPrint" runat="server"></asp:Label>
                    </div>
                </div>
            </td>
            <td style="float: right; padding-right: 20px;">
                <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Status: "></asp:Label>
                <asp:Label ID="lblPrintStatus" runat="server" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>


    <asp:ListView ID="ListViewPrint" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1" OnItemDataBound="ListViewPrint_ItemDataBound"
        DataKeyNames="WorkItemID">
        <LayoutTemplate>
            <table style="border-collapse: collapse; width: 100%;" cellpadding="0" cellspacing="0">
                <tr>
                    <th style="font-weight: bold; border: 1px solid black; font-size: 0.85em; color: #777; text-align: left; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px;">
                        <asp:Label ID="lbType" runat="server" Text="Type" Enabled="false"  />

                    </th>
                    <th style="font-weight: bold; border: 1px solid black; font-size: 0.85em; color: #777; text-align: left; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px;">
                        <asp:Label ID="lbWorkItem" runat="server" Text="Type" Enabled="false"  />
                    </th>
                    
                    <th id="subWorkItemHead" visible="false" runat="server" style="font-weight: bold; border: 1px solid #bbb; font-size: 0.85em; color: #777; text-align: left; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px;">
                        <asp:Label ID="lbSubWorkItem" runat="server" Text="Work Item" Enabled="false" />
                    </th>
                    <th id="subSubWorkItemHead" visible="false" runat="server" style="font-weight: bold; border: 1px solid #bbb; font-size: 0.85em; color: #777; text-align: left; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px;">
                        <asp:Label ID="lbSubSubWorkItem" runat="server" Text="Work Item" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 55px;">
                        <asp:Label ID="lbWeekDay1"  runat="server" Text="Mon" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbWeekDay2"  runat="server" Text="Tue" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbWeekDay3"  runat="server" Text="Wed" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbWeekDay4"  runat="server" Text="Thu" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbWeekDay5"  runat="server" Text="Fri" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbWeekDay6"  runat="server" Text="Sat" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; border-left: none; border-right: none; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbWeekDay7"  runat="server" Text="Sun" Enabled="false" />
                    </th>
                    <th style="font-weight: bold; border: 1px solid black; text-align: center; font-size: 0.85em; color: #777; text-decoration: none; vertical-align: middle; padding: 5px 17px 5px 5px; width: 60px;">
                        <asp:Label ID="lbHorizontalTotal" runat="server" Text="Total" Enabled="false"  />
                    </th>
                </tr>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                <tr id="trSubWorkItemTotal1" runat="server" style="display: none;">
                    <td style="font-weight: bold; text-align: left; border: none;" colspan="2"></td>
                    <td style="font-weight: bold; text-align: left; border: none;" id="subWorkItemTotal1" runat="server" visible="false"></td>
                    <td colspan="7">&nbsp;</td>
                    <td style="border: 1px solid black;border-bottom:none;border-top:none;"></td>                  
                </tr>
                <tr id="trSubWorkItemTotal2" runat="server" style="display: none;">
                    <td style="font-weight: bold; text-align: left; border: none;" colspan="2"></td>
                    <td style="font-weight: bold; text-align: left; border: none;" id="subWorkItemTotal2" runat="server" visible="false"></td>
                    <td style="font-weight: bold; text-align: left; border: none;" colspan="7"></td>
                    <td style="font-weight: bold; border: 1px solid black;text-align: left; border-bottom:none;border-top:none; ">&nbsp;</td>
                </tr>
                <tr>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: left; border-left: none; border-right: none; padding: 5px 17px 5px 5px" colspan="2"><%--Total--%></td>
                    <%--<td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: left; border-left: none; border-right: none; padding: 5px 17px 5px 5px" visible="false" id="subWorkItemTotal3" runat="server">Total</td>--%>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: right; border-left: none; border-right: none; padding: 5px 17px 5px 5px" visible="false" id="subWorkItemTotal3" runat="server"></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: right; border-left: none; border-right: none; padding: 5px 17px 5px 5px" visible="false" id="subSubWorkItemTotal" runat="server"></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay1VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay2VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold; font-size: 0.85em; color: #777; border: 1px solid black;text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay3VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777; border: 1px solid black; text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay4VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay5VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay6VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  font-size: 0.85em; color: #777; border: 1px solid black;text-align: center; border-left: none; border-right: none; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbWeekDay7VTotal" runat="server"></asp:Label></td>
                    <td style="font-weight: bold;font-size: 0.85em; color: #777;  border: 1px solid black;text-align: center; padding: 5px 17px 5px 5px">
                        <asp:Label ID="lbVTotal" runat="server"></asp:Label></td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td style="font-weight: bold; text-align: left;vertical-align: top; border: none; padding: 4px 8px 4px 4px;">
                    <div style="display: none;" class="workitemkey"><%#  Eval("WorkItemID")%></div>
                    <div style="display: none;" class="workitemtype"><%#  Eval("TypeName")%></div>
                    <div style="display: none;" class="workitemname"><%#  Eval("WorkItem")%></div>
                    <div style="display: none;" class="subworkitem"><%#  Eval("OriginalSubWorkItem")%></div>
                    <asp:HiddenField ID="hdnworkitemtype" runat="server" Value='<%#  Eval("TypeName")%>' />
                    <asp:HiddenField ID="hdnworkitemname" runat="server" Value='<%#  Eval("WorkItem")%>' />
                    <asp:HiddenField ID="hdnsubworkitem" runat="server" Value='<%#  Eval("OriginalSubWorkItem")%>' />
                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>

                </td>
                <td style="padding: 4px 8px 4px 4px;  text-align: left;vertical-align: top; border-left: none; border-right: none;">
                     <asp:Label ID="lblWorkItem" runat="server"> <%# SetExpandCollapseIcon()%> <%# Eval("WorkItem")%>
                    <div style="font-size: smaller;"><%# UGITUtility.TruncateWithEllipsis(Convert.ToString(Eval(DatabaseObjects.Columns.Title)), 100)%></div>
                    </asp:Label>
                </td>
                <td style="padding: 4px 8px 4px 4px;  text-align: left;vertical-align: top; border: none;" visible="false" id="subWorkItemItem" runat="server"><%# Eval("SubWorkItemLink")%></td>
                <td style="padding: 4px 8px 4px 4px;  text-align: left;vertical-align: top; border: none;" visible="false" id="subSubWorkItemItem" runat="server"><%# Eval("SubSubWorkItemLink")%></td>

                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day1"><%# Eval("WeekDay1")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day2"><%# Eval("WeekDay2")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day3"><%# Eval("WeekDay3")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day4"><%# Eval("WeekDay4")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day5"><%# Eval("WeekDay5")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day6"><%# Eval("WeekDay6")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class='<%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day7"><%# Eval("WeekDay7")%></td>
                <td style="padding: 4px 8px 4px 4px; border: 1px solid #bbb;text-align: center;vertical-align: top; border-bottom: none; border-top: none;"><%# (double)Eval("WeekDay1") + (double)Eval("WeekDay2") + (double)Eval("WeekDay3") + (double)Eval("WeekDay4") + (double)Eval("WeekDay5") + (double)Eval("WeekDay6") + (double)Eval("WeekDay7")%></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color: #EFEFEF;">
                <td style="font-weight: bold; padding: 4px 8px 4px 4px; text-align: left;vertical-align: top; border: none;">
                    <div style="display: none;" class="workitemkey"><%#  Eval("WorkItemID")%></div>
                    <div style="display: none;" class="workitemtype"><%#  Eval("TypeName")%></div>
                    <div style="display: none;" class="workitemname"><%#  Eval("WorkItem")%></div>
                    <div style="display: none;" class="subworkitem"><%#  Eval("OriginalSubWorkItem")%></div>
                    <asp:HiddenField ID="hdnworkitemtype" runat="server" Value='<%#  Eval("TypeName")%>' />
                    <asp:HiddenField ID="hdnworkitemname" runat="server" Value='<%#  Eval("WorkItem")%>' />
                    <asp:HiddenField ID="hdnsubworkitem" runat="server" Value='<%#  Eval("OriginalSubWorkItem")%>' />
                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("Type")%>'></asp:Label>

                    
                </td>
                <td style="padding: 4px 8px 4px 4px; text-align: left; vertical-align: top;border: none;">
                     <asp:Label ID="lblWorkItem" runat="server"><%# Eval("WorkItem")%>
                    <div style="font-size: smaller;"><%# UGITUtility.TruncateWithEllipsis(Convert.ToString(Eval(DatabaseObjects.Columns.Title)), 100)%></div>
                    </asp:Label>

                </td>
                <td style="padding: 4px 8px 4px 4px; text-align: left;vertical-align: top; border: none;" visible="false" id="subWorkItemItem" runat="server"><%# Eval("SubWorkItemLink")%></td>
                <td style="padding: 4px 8px 4px 4px;  text-align: left;vertical-align: top; border: none;" visible="false" id="subSubWorkItemItem" runat="server"><%# Eval("SubSubWorkItemLink")%></td>

                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day1"><%# Eval("WeekDay1")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day2"><%# Eval("WeekDay2")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center; vertical-align: top;border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day3"><%# Eval("WeekDay3")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day4"><%# Eval("WeekDay4")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day5"><%# Eval("WeekDay5")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center; vertical-align: top;border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day6"><%# Eval("WeekDay6")%></td>
                <td style='padding: 4px 8px 4px 4px; text-align: center;vertical-align: top; border: none;' class=' <%# UGITUtility.StringToBoolean(Eval("ShowEditButtons")) ? "week": string.Empty %>' daynum="day7"><%# Eval("WeekDay7")%></td>
                <td style="padding: 4px 8px 4px 4px; border: 1px solid #bbb;text-align: center;vertical-align: top; border-bottom: none; border-top: none;"><%# (double)Eval("WeekDay1") + (double)Eval("WeekDay2") + (double)Eval("WeekDay3") + (double)Eval("WeekDay4") + (double)Eval("WeekDay5") + (double)Eval("WeekDay6") + (double)Eval("WeekDay7")%></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:ListView>
</asp:Panel>


<asp:HiddenField ID="hdnActionValue" runat="server" />
<dx:ASPxPopupControl ID="signOffCommentsPopup" runat="server" Modal="true" CssClass="departmentPopup" Height="175px"
    ClientInstanceName="signOffCommentsPopup" PopupElementID="addCommentButton" ShowFooter="false" ShowHeader="true"
    EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Above" EnableHierarchyRecreation="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <div class="timesheet-addcomment-popup first_tier_nav">
                <table class="ro-table">
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="txtAddComment" CssClass="txtaddcomment" Width="400px" Columns="52" Rows="9"
                                TextMode="MultiLine" Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 4px;"></td>
                    </tr>
                    <tr>
                        <td class="buttoncell">
                            <ul style="float: right">
                                <li runat="server" id="btnAddCommentLI" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                    <asp:LinkButton runat="server" CssClass="comment ugit-btntext-color" ID="btnSignOffTimeSheet"
                                        OnClientClick="SignOffTimeSheet();" OnClick="btnSignOffTimeSheet_Click" Text="Ok" />
                                </li>
                                <li runat="server" id="btnCancelCommentLI" class="ugit-btntext-color" onmouseover="this.className='tabhover'" onmouseout="this.className=''">
                                    <a id="btnCancelComment" onclick="signOffCommentsPopup.Hide();"
                                        class="cancelwhite" href="javascript:void(0);">Cancel</a>
                                </li>
                            </ul>
                        </td>
                    </tr>
                </table>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<asp:Button ID="btnNotifySave" runat="server" CssClass="clsSaveNotifyVisibility" OnClick="btnNotifySave_Click" />
<asp:Button ID="btRefreshPage" runat="server" CssClass="hide" OnClick="btRefreshPage_Click"/>
<asp:Button ID="btnLoadTimeSheet" runat="server" CssClass="clsSaveNotifyVisibility" OnClick="btnLoadTimeSheet_Click" />

<%--Popup to show all subordinates with their TimeSheet Status for a selected week--%>
<dx:ASPxPopupControl ID="statusGridContainer" runat="server" AllowDragging="false" ClientInstanceName="statusGridContainer" PopupElementID="statusPicker" CloseAction="OuterMouseClick" ShowCloseButton="true"
    ShowFooter="false" ShowHeader="true" HeaderText="Timesheet Status For My Team" PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" EnableViewState="false" EnableHierarchyRecreation="True"
    OnWindowCallback="statusGridContainer_WindowCallback" CssClass="context-popup-grid" Width="750" Height="250">
    <HeaderStyle Font-Bold="true" ForeColor="Black" />
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <dx:ASPxGridView ID="statusGrid" runat="server" OnDataBinding="statusGrid_DataBinding" KeyFieldName="ResourceId" AutoGenerateColumns="false"
                Width="100%" ClientInstanceName="statusGrid" EnableViewState="false">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ResourceId" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="ResourceUser" Caption="Resource" Width="40%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center"
                        HeaderStyle-HorizontalAlign="Center" SortOrder="Ascending">
                        <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="Status" Caption="Timesheet Status" Width="20%" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="WorkDate" Caption="Latest Time Entry" Width="20%" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataDateColumn FieldName="Modified" Caption="Last Modified" Width="20%" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
                    </dx:GridViewDataDateColumn>
                </Columns>
                <Settings ShowHeaderFilterButton="true" EnableFilterControlPopupMenuScrolling="true" VerticalScrollBarMode="Visible"
                    VerticalScrollableHeight="200" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsBehavior AllowSelectByRowClick="true" AllowSort="true" EnableRowHotTrack="true" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
                <Styles>
                    <AlternatingRow Enabled="True"></AlternatingRow>
                </Styles>
                <FormatConditions>
                    <dx:GridViewFormatConditionHighlight FieldName="Status" Expression="[Status] = 'Time Entry'" Format="Custom" CellStyle-ForeColor="DarkBlue"></dx:GridViewFormatConditionHighlight>
                    <dx:GridViewFormatConditionHighlight FieldName="Status" Expression="[Status] = 'Pending Approval'" Format="Custom" CellStyle-CssClass="color-rust"></dx:GridViewFormatConditionHighlight>
                    <dx:GridViewFormatConditionHighlight FieldName="Status" Expression="[Status] = 'Approved'" Format="Custom" CellStyle-ForeColor="Green"></dx:GridViewFormatConditionHighlight>
                    <dx:GridViewFormatConditionHighlight FieldName="Status" Expression="[Status] = 'Returned'" Format="Custom" CellStyle-ForeColor="Red"></dx:GridViewFormatConditionHighlight>
                    <dx:GridViewFormatConditionHighlight FieldName="Status" Expression="[Status] = 'Sign Off'" Format="Custom" CellStyle-ForeColor="Green"></dx:GridViewFormatConditionHighlight>
                </FormatConditions>
                <ClientSideEvents RowClick="function(s,e){ statusGrid_RowClickEvent(s,e);}" />
            </dx:ASPxGridView>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>

<%--Popup to show all the subordinates with their TimeSheet Status WeekStartDate whose TimeSheet Status is 'Pending Approval'--%>
<dx:ASPxPopupControl ID="pendingStatusGridContainer" runat="server" AllowDragging="false" ClientInstanceName="pendingStatusGridContainer" PopupElementID="pendingStatusPicker" ShowCloseButton="true"
    ShowFooter="false" ShowHeader="true" HeaderText="Timesheets Pending Approval" PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides" EnableViewState="false" EnableHierarchyRecreation="True"
    OnWindowCallback="pendingStatusGridContainer_WindowCallback" CssClass="context-popup-grid" Width="400" Height="250">
    <HeaderStyle Font-Bold="true" ForeColor="Black" />
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <dx:ASPxHiddenField ID="hdnAllowCallBack" runat="server" ClientInstanceName="hdnAllowCallBack"></dx:ASPxHiddenField>
            <dx:ASPxGridView ID="pendingStatusGrid" runat="server" OnDataBinding="pendingStatusGrid_DataBinding" KeyFieldName="ResourceId" AutoGenerateColumns="false"
                Width="100%" ClientInstanceName="pendingStatusGrid" EnableViewState="false">
                <Columns>
                    <dx:GridViewDataTextColumn FieldName="ResourceId" Visible="false">
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="WeekStartDate" Caption="Date" Width="40%" HeaderStyle-Font-Bold="true"
                        CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortOrder="Ascending">
                        <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
                    </dx:GridViewDataDateColumn>
                    <dx:GridViewDataTextColumn FieldName="ResourceUser" Caption="Resource" Width="60%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings AllowHeaderFilter="True" AllowAutoFilter="True" AllowSort="True" />
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowHeaderFilterButton="true" EnableFilterControlPopupMenuScrolling="true" VerticalScrollBarMode="Visible"
                    VerticalScrollableHeight="200" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsBehavior AllowSelectByRowClick="true" AllowSort="true" />
                <SettingsPager Mode="ShowAllRecords" />
                <SettingsDataSecurity AllowInsert="false" AllowEdit="false" AllowDelete="false" />
                <Styles>
                    <AlternatingRow Enabled="True"></AlternatingRow>
                </Styles>
                <ClientSideEvents RowClick="function(s,e){ pendingStatusGrid_RowClickEvent(s,e);}" />
            </dx:ASPxGridView>
        </dx:PopupControlContentControl>
    </ContentCollection>

</dx:ASPxPopupControl>


<dx:ASPxPopupControl ID="pcAddComment" runat="server" CloseAction="CloseButton" Width="370px" OnWindowCallback="pcAddComment_WindowCallback"
    PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ClientInstanceName="pcAddComment"
    HeaderText="Add/Edit Comment" AllowDragging="false" ShowFooter="false">
    <ContentCollection>
        <dx:PopupControlContentControl ID="pcccAddComment" runat="server">
            <dx:ASPxPanel ID="panelComment" runat="server" DefaultButton="btnCommentSave">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <table style="width: 100%; height: 70px;">                           
                            <tr>
                                <td colspan="2">
                                    <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" CssClass="txtaddcomment" Width="400px" Columns="52" Rows="9" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-top:5px;">
                                    <div class="buttoncell">
                                        <span style="float: right; padding-right: 1px;">
                                            <dx:ASPxButton ID="btCommentCancel" runat="server" ClientInstanceName="btCommentCancel" Text="Cancel" Width="50px" ToolTip="Cancel" AutoPostBack="false"
                                                CausesValidation="false" Style="float: right; margin-right: 1px;">
                                                <ClientSideEvents Click="function(s, e) { pcAddComment.Hide(); }" />
                                            </dx:ASPxButton>
                                        </span>

                                        <span style="float: right; padding-right: 1px;">
                                            <dx:ASPxButton ID="btnCommentSave" ClientInstanceName="btnCommentSave" runat="server" AutoPostBack="false"
                                                Text="Save" Width="70px" Style="float: right; margin-right: 5px" ToolTip="Save">
                                                <ClientSideEvents Click="function(s, e) { OpenCommentDialog();}" />
                                                
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
</dx:ASPxPopupControl>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function resolveFractionHour(workHour) {
      
        var workHourNumber = Number(workHour);
      
        var newTotal = 0;
        if (Number(workHour)) {
            var hours = Number(workHourNumber.toString().split(".")[0]);
            var mins = (workHourNumber * 60) % 60;

            if (mins > 45) {
                mins = 60;
            }
            else if (mins > 30) {
                mins = 45;
            }
            else if (mins > 15) {
                mins = 30;
            }
            else if (mins > 0) {
                mins = 15;
            }

            newTotal = ((hours * 60) + mins) / 60;
        }

        return newTotal;
    }

    function editItemOnbdClick(rowIndex) {
        if ($(".worksheettable .weekedit").length <= 0) {
            editWorkSheet();
        }
    }

    //$(function () {
    //    $(document).bind("keypress", function (e) {
    //        var code = e.which;
    //        if (code == 13 && !$(e.target).is(":submit") && !$(e.target).is(":image")) { //Enter keycode
    //            return false;
    //        }
    //        return true;
    //    });


    //});

    function editWorkSheet() {
        //debugger;
        if (stopEditing) {
            return;
        }

        removeMessage();
        if ($(".worksheettable .week").length > 0 && $(".worksheettable .weekedit").length <= 0) {
            $.each($(".worksheettable .week"), function (i, item) {
                //$(item).html("<input  class='weekedit' tabindex='" + (i + 1) + "' onChange='fillTimeSheetHours(this)' type='text' style='width:35px;text-align:center;' orgVal='" + $.trim($(item).html()) + "' oldVal='" + $.trim($(item).html()) + "' value='" + $.trim($(item).html()) + "'/>");
                $(item).html("<input  class='weekedit' tabindex='" + (i + 1) + "' onChange='fillTimeSheetHours(this)' type='text' style='width:35px;text-align:center;' orgVal='" + $.trim($(item).text()) + "' oldVal='" + $.trim($(item).text()) + "' value='" + $.trim($(item).text()) + "'/>");
            });
        }

        $("#<%= btEditTimesheetLI.ClientID %>").hide();
        $("#<%= btSaveTimesheetLI.ClientID %>").show();
        $("#<%= btCancelEditingLI.ClientID %>").show();

    }
    function cancelWorkSheet() {

        $("#stopsave").attr("value", "true");
        $(".btrefresh").get(0).click();


    }

    function fillTimeSheetHours(obj) {
        //debugger;
        removeMessage();
       
        var number = Number($.trim($(obj).val()));
        if ($.trim($(obj).val()) == "" || !Number($.trim($(obj).val()))) {
            if (number != 0) {
                $(obj).val($(obj).attr("oldVal") != "" ? $(obj).attr("oldVal") : "0");
                $(obj).get(0).focus();
                setMessage("Only numbers are allowed", "red", 0);
                return;
            }
        }

        if (Number($.trim($(obj).val())) && Number($.trim($(obj).val()) < 0)) {
            $(obj).val($(obj).attr("oldVal") != "" ? $(obj).attr("oldVal") : "0");
            $(obj).get(0).focus();
            setMessage("Only positvie numbers are allowed", "red", 0);
            return;
        }

        var hTotal = 0;
        var currentTr = $(obj).parent().parent();
        $.each(currentTr.find(".weekedit"), function (i, item) {
            if (Number($.trim($(item).val()))) {
                hTotal += Number($.trim($(item).val()));
            }
        });


        var vTotal = 0;
        var dayWorkSheet = $(".worksheettable tbody").find("td[daynum='" + $(obj).parent().attr("daynum") + "']");
        $.each(dayWorkSheet, function (i, item) {
            var inputVal = $(item).find("input").val();
            var attr = $(item).find("input").attr('orgval'); //skip row with SubTotal.
            if (attr === undefined || attr === false) {
                return;
            }
            if (Number($.trim(inputVal))) {
                vTotal += Number($.trim(inputVal));
            }
        });

        var dayTotal = $(".worksheettable .eachdaytotal ." + $(obj).parent().attr("daynum") + "total");
        if (vTotal > 24) {

            vTotal = vTotal - Number($.trim($(obj).val()));
            hTotal = hTotal - Number($.trim($(obj).val()));
            $(obj).val($(obj).attr("oldVal") != "" ? $(obj).attr("oldVal") : "0");
            setMessage("You cannot add more then 24 hours in a day", null, 0);
        }
        dayTotal.html(resolveFractionHour(vTotal));

        var horizantalTotal = currentTr.find(".lbhtotal")
        horizantalTotal.html(resolveFractionHour(hTotal));

        var allDayTotal = $(".lbvtotal")
        var daysTotal = $(".eachdaytotal .daytotal")
        var aTotal = 0;
        $.each(daysTotal, function (i, item) {
            if (Number($.trim($(item).html()))) {
                aTotal += Number($.trim($(item).html()));
            }
        });
        allDayTotal.html(resolveFractionHour(aTotal));

        var newValue = resolveFractionHour($.trim($(obj).val()));
        $(obj).attr("oldVal", newValue);
        $(obj).val(newValue);
        updateSubTotal(obj);
    }


    function updateSubTotal(ctrl)
    {       
        var attribute = ctrl.parentElement.getAttribute('daynum')
        var trRow = $(ctrl).parents("tr:eq(0)");
        var arrayClass = trRow.attr("class").split(' ');
        var childClass = "";
        $.each(arrayClass, function (key, item) {
            if (item.indexOf("Child")> -1) {
                childClass = item;
            }
        });
        var parentClass = childClass.replace("Child", "Parent");
        var trList = $('.' + childClass);
        var total = 0;
        $.each(trList, function (key, tritem) {
            var tditem = $(tritem).find("td[daynum='" + attribute + "']");
            var inputItem=tditem.find("input[class='weekedit']");
            total +=parseInt(inputItem.val());
        });
        var parentTR = $('.' + parentClass);
        var subTotalTd = parentTR.find("td[daynum='" + attribute + "']");
        if (subTotalTd && subTotalTd.length>0) {
            subTotalTd[0].innerText = total;
        }
        var totalAll = 0;
        var tdall=$('.' + parentClass).find('td[daynum]');
        $.each(tdall, function (key, tdtotal) {
            totalAll += parseInt(tdtotal.innerText);
        });
        var tdTotalAll=$('.' + parentClass).find('td[class="ms-vb2 alncenter totalbordervartical lbhtotal"]');
        if (tdTotalAll && tdTotalAll.length>0)
            tdTotalAll[0].innerText = totalAll;
    }


    function saveWorkSheet() {
        saveSheetDone = false;
        setMessage("Validating and saving hours ...", null, 0);
        startWaiting("detailviewmain");

        var worksheetRows = $(".worksheetrow");

        var timeSheetArray = "<ArrayOfWorkItemHours>";
        //debugger;
        $.each(worksheetRows, function (i, item) {
            var weekHeader = $(".worksheettable .weekheader");
            var editableColumns = $(item).find(".week");
            var workItemID = $.trim($(item).find(".workitemkey").html());
            var workItemType = $.trim($(item).find(".workitemtype").html());
            var workItemName = $.trim($(item).find(".workitemname").html());
            var subWorkItem = $.trim($(item).find(".subworkitem").html());
            var subSubWorkItem = $.trim($(item).find(".subsubworkitem").html());

            if (editableColumns.length > 0) {
                var innerXml = new Array();
                var weekdata = new Array();

                if (!workItemID || workItemID == "0") {
                    
                    innerXml.push("<WorkItemType>" + workItemType + "</WorkItemType>");
                    innerXml.push( "<WorkItem>" + workItemName + "</WorkItem>");
                    innerXml.push("<SubWorkItem>" + subWorkItem + "</SubWorkItem>");
                    innerXml.push("<SubSubWorkItem>" + subSubWorkItem + "</SubSubWorkItem>");
                }
                else {
                    innerXml.push("<WorkItemID>" + workItemID + "</WorkItemID>");
                }

                for (var i = 0; i < editableColumns.length; i++) {
                    var head = weekHeader.filter(function (index) {
                        return $(this).hasClass($(editableColumns[i]).attr("daynum"));
                    });

                    var day = head.html();
                    var orgVal = $(editableColumns.get(i)).find("input").attr("orgVal");
                    var value = $(editableColumns.get(i)).find("input").val();
                   
                    day = day.split(" ")[0];
                    weekdata.push("<" + day + ">" + value + "</" + day + ">");
                    

                    //if (orgVal !== value) {
                    //    day = day.split(" ")[0];
                    //    weekdata.push("<" + day + ">" + value + "</" + day + ">");
                    //}
                }

               if (weekdata.length > 0) {
                    innerXml.push(weekdata.join(""));
                    var workItemJson = "<WorkItemHours>" + innerXml.join("") + "</WorkItemHours>";
                    timeSheetArray += workItemJson;
                }
                
            }
        });

        timeSheetArray += "</ArrayOfWorkItemHours>";

        saveWorkSheetData(timeSheetArray);
    }

    function saveWorkSheetData(timeSheetArray) {
        saveSheetDone = false;
        var startDate = $("#<%=startWeekDateForEdit.ClientID %>").val();

        var currentID = cmbCurrentUser.GetValue(); 
        var dataVar = "{ 'timeSheetData' : \"" + timeSheetArray + "\", 'startDate':'" + startDate + "', 'userID': '" + currentID + "' , 'currentUserID': '<%=currentUserID%>' }";
        
        var token = sessionStorage.getItem('Token');
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }
        $.ajax({
            type: "POST",
            url: "<%= ajaxPageURL %>SaveTimeSheet",
            data: dataVar,
            header: {
                'Authorization': 'Bearer ' + token
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (message) {
                
                var resultJson = $.parseJSON(message);
                if (resultJson.messagecode = 0) {
                    setMessage(resultJson.message, "red", 0);
                }
                else if (resultJson.messagecode = 1) {
                    setMessage(resultJson.message, "blue", 2);
                    if ($(".worksheettable .weekedit").length > 0) {
                        $.each($(".worksheettable .weekedit"), function (i, item) {
                            $(item).parent().html($(item).val());
                        });
                    }

                    $("#<%= btEditTimesheetLI.ClientID %>").show();
                    $("#<%= btSaveTimesheetLI.ClientID %>").hide();
                    $("#<%= btCancelEditingLI.ClientID %>").hide();
                    //debugger;
                    if (!isActionPerform) {
                        //send notification to manager if in sheet is on pending approval mode
                        var btnNotifySave = document.getElementById('<%=btnNotifySave.ClientID%>');
                        btnNotifySave.click();
                    }
                    else {
                        var btrefreshpage = $('#<%=btRefreshPage.ClientID %>');
                        btrefreshpage.trigger("click");
                    }
                }

                stopWaiting("detailviewmain");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                
                //   alert(ajaxOptions);
            }
        });
}



function setMessage(message, color, removeAfter) {
    var lbMessage = $("#<%=lbMessage.ClientID %>");
        lbMessage.addClass("message-container");
        lbMessage.html(message);
        if (color == undefined || color == null || color == "") {
            color = "blue";
        }
        lbMessage.css("color", color);
        if (removeAfter != undefined && removeAfter != null && removeAfter > 0) {
            setTimeout("removeMessage()", removeAfter * 1000);
        }
    }

    function removeMessage() {
        var lbMessage = $("#<%=lbMessage.ClientID %>");
        lbMessage.removeClass("message-container");
        lbMessage.html("");
    }



    //Function to submit the timesheet in case of Sending for approval, returning, approving and Signing Off the timesheet
function SignOffTimeSheet() {
    isActionPerform = true;
    if (isInEditMode) {
        $.when(saveWorkSheet()).then(isInEditMode = false);
    }
    var total = $(".lbvtotal");
    var totalHours = Number(total.text());
    isActionPerform = false;

    //hidden field hdnActionValue is used to perform specific task on the basis of its value
    var actionField = document.getElementById('<%= hdnActionValue.ClientID%>');
    if (actionValue == "Return") {
        actionField.value = actionValue;
    }
    else if (actionValue == "SendForApproval" || actionValue == "Approved" || actionValue == "Sign Off") {
        if (!(totalHours > 0)) {
            readyForApproval = false;

            if (actionValue == "SendForApproval")
                alert('Please fill the Time Sheet before sending it for Approval.');
            else if (actionValue == "Approved")
                alert('Please fill the Time Sheet before Approving it.');
            else if (actionValue == "Sign Off")
                alert('Please fill the Time Sheet before Sign Off.');

            return false;
        }
        readyForApproval = true;
        actionField.value = actionValue;
    }

    signOffCommentsPopup.Hide();
}


    //function to Return the Time Sheet
function UnLockWorkSheet() {
    stopEditing = false;
    actionValue = "Return";
    signOffCommentsPopup.SetHeaderText(actionValue);
    signOffCommentsPopup.Show();
}

    //function to sending the Time Sheet for Approval
    function SendForApproval() {
        //debugger;
        notShowUnloadConfirmation = true;
    actionValue = "SendForApproval";
    if (!readyForApproval)
        SignOffTimeSheet();

    if (readyForApproval) {
        var btnSubmitTimeSheet = document.getElementById('<%=btnSignOffTimeSheet.ClientID%>');
        btnSubmitTimeSheet.click();
    }
    else
        return;
    }
    //function to Approve the Time Sheet
    function Approved() {
        notShowUnloadConfirmation = true;
    actionValue = "Approved";

    if (!readyForApproval)
        SignOffTimeSheet();

    if (readyForApproval) {
        var btnSubmitTimeSheet = document.getElementById('<%=btnSignOffTimeSheet.ClientID%>');
        btnSubmitTimeSheet.click();
    }
    return false;
}

    //function to Sign Off the Time Sheet
    function SignOff() {
        if (confirm('Are you sure that you want to Sign Off the Timesheet?')) {
            notShowUnloadConfirmation = true;
            actionValue = "Sign Off";

            if (!readyForApproval)
                SignOffTimeSheet();

            if (readyForApproval) {
                var btnSubmitTimeSheet = document.getElementById('<%=btnSignOffTimeSheet.ClientID%>');
                btnSubmitTimeSheet.click();
            }
            else
                return;
        }
        else
            return;
    }

    //function to Lock the Time Sheet for editing
    function LockEditing() {
        stopEditing = true;
    }

    //function to Open SignOff History Popup
    function ShowSignOffHistory() {
        var signOffItemId = "<%=signOffItemId%>";
        if (signOffItemId > 0) {
            var signOffHistoryUrl = "<%=signOffHistoryUrl%>" + "&pageTitle=History&isdlg=1&isudlg=1" + "&signOffItemId=" + signOffItemId;
            window.parent.UgitOpenPopupDialog(signOffHistoryUrl, '', 'History', '75', '50', false, escape(window.location.href));
        }
        else
            alert("History is not available for this week.");
    }


    //Function to Show/hide and Bind Status Grid
    function ShowHideStatusGrid() {
        pendingStatusGridContainer.Hide();
        statusGridContainer.PerformCallback();
    }

    //Function to bind timesheet when user select any row from Status Grid
    function statusGrid_RowClickEvent(s, e) {
    if (!statusGridContainer.InCallback()) {
        var resourceId = s.GetRowKey(e.visibleIndex);
        if (resourceId == cmbCurrentUser.GetSelectedItem().value)
            return;

        if (!cmbCurrentUser.InCallback()) {
            cmbCurrentUser.PerformCallback('ReloadTimeSheet:' + resourceId);
            statusGridContainer.Hide();
        }
    }
}

    //Method to Show/Hide pendingStatus Grid
function ShowHidePendingStatusGrid() {
    statusGridContainer.Hide();
    pendingStatusGridContainer.PerformCallback();
}

    //Function to bind timesheet when user click on any row from pendingStatus Grid
    function pendingStatusGrid_RowClickEvent(s, e) {
        if (!pendingStatusGridContainer.InCallback()) {
            s.GetRowValues(e.visibleIndex, 'ResourceId;WeekStartDate', OnSelectedRowValues);
        }
    }

    function OnSelectedRowValues(selectedValues) {
        if (!cmbCurrentUser.InCallback()) {
            hdnAllowCallBack.Set("Result", selectedValues[0] + ";" + selectedValues[1]);
            var btnLoadTimeSheet = document.getElementById('<%=btnLoadTimeSheet.ClientID%>');
            btnLoadTimeSheet.click();
            pendingStatusGridContainer.Hide();
        }
    }

<%--    function onselectedrowvalues(selectedvalues) {
    if (!cmbcurrentuser.incallback()) {
        hdnallowcallback.set("result", selectedvalues[0] + ";" + selectedvalues[1]);
        var btnloadtimesheet = document.getelementbyid('<%=btnloadtimesheet.clientid%>');
        btnloadtimesheet.click();
        pendingstatusgridcontainer.hide();
    }
}--%>

    function PrintPanel() {
        var panel = document.getElementById("<%=printworkSheetPanel.ClientID %>");
        document.getElementById("<%=LblWeekDurationPrint.ClientID %>").innerText = document.getElementById("<%=lbWeekDuration.ClientID %>").innerText;
        var printWindow = window.open('', '', 'top=200,left=200,height=500,width=800');
        printWindow.document.write('<html><head> <style> @page { size: landscape; } </style> <title>Resource Weekly TimeSheet</title>');
        printWindow.document.write('</head><body style="-webkit-print-color-adjust:exact;"  >');
        printWindow.document.write(panel.innerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
  
    function copyPreviousWeekWorkSheet() {
        //debugger;
        var startDate = $("#<%=startWeekDateForEdit.ClientID %>").val();
        var currentID = cmbCurrentUser.GetValue();
      
        var dataVar = "{  'startDate':'" + startDate + "', 'userID': '" + currentID + "' }";

        loadingPanel.Show();

        $.ajax({
            type: "POST",
            url: "<%= ajaxPageURL %>/CopyPreviousWeekTimeSheet",
            data: dataVar,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                loadingPanel.Hide();
                var resultJson = $.parseJSON(message);
                if (resultJson.status === 'done') {
                    var btRefreshPage = $('#<%=btRefreshPage.ClientID%>');
                    btRefreshPage.trigger("click");
                }
                else if (resultJson.status === 'nochange') {
                     setMessage(resultJson.message, "blue", 2);
                }
                else {
                     setMessage(resultJson.message, "blue", 2);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //   alert(ajaxOptions);
                loadingPanel.Hide();
            }
        });
    }
</script>

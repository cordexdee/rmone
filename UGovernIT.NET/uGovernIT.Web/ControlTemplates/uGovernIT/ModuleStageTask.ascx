
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleStageTask.ascx.cs" Inherits="uGovernIT.Web.ModuleStageTask" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .fleft {
        float: left;
    }

    .proposeddatelb {
        padding-top: 5px;
        padding-right: 4px;
        float: left;
    }

    .colForXS {
        width: 49%;
        padding: 0px;
        display: inline-block;
        /*float: left;*/
    }
</style>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">

    <div class="ms-formtable accomp-popup ">
        <div class="row">
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trTitle" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="Task"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>

                        <asp:Label ID="lbTitle" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trStage" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Stage
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlModuleStep" runat="server">
                        </asp:DropDownList>
                        <asp:Label ID="lbStage" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trItemOrder" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Item Order
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtItemOrder" runat="server"></asp:TextBox>
                        <asp:Label ID="lblItemOrder" runat="server" Visible="false"></asp:Label>
                        <asp:RegularExpressionValidator ID="regextxtItemOrder" ErrorMessage="Only numeric allow." ControlToValidate="txtItemOrder" runat="server" ValidationExpression="^([0-9]+)$" ValidateEmptyText="false" ValidationGroup="Task" />
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trStatus" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Status
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <div>
                            <asp:DropDownList ID="ddlStatus" runat="server">
                                <asp:ListItem Text="Not Started"></asp:ListItem>
                                <asp:ListItem Text="In Progress"></asp:ListItem>
                                <asp:ListItem Text="Completed"></asp:ListItem>
                                <asp:ListItem Text="Deferred"></asp:ListItem>
                                <asp:ListItem Text="Waiting on someone else"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbStatus" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div style="float: right; padding-right: 20px;" id="tdCompletedOn" runat="server" visible="false">
                            <table style="float: right">
                                <tr>
                                    <td class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Completed On:
                                        </h3>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCompletedOn" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 colForXS" id="txtComplete">
                <div id="trPctComplete" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">% Complete
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtPctComplete" Width="50" ValidationGroup="Task" CssClass="pctcomplete"
                            runat="server">
                        </asp:TextBox>
                        <asp:Label ID="lbPctComplete" runat="server" Visible="false"></asp:Label>
                        <asp:RegularExpressionValidator ID="eevPctComplete" runat="server" ControlToValidate="txtPctComplete"
                            ValidationGroup="Task" Display="Dynamic" ErrorMessage="range 0 to 100." ForeColor="Red"
                            ValidationExpression="(100)|[0-9]\d?"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trAssignedTo" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Assigned To
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:UserValueBox runat="Server" isMulti="true" CssClass="userValueBox-dropDown" ID="peAssignedTo" Width="100%" />
                        <asp:Label ID="lbAssignedTo" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trAutoApprove" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Auto Approve
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlAutoApprove" runat="server">
                            <asp:ListItem Text="Yes"></asp:ListItem>
                            <asp:ListItem Text="No"></asp:ListItem>

                        </asp:DropDownList>
                        <asp:Label ID="lblAutoApprove" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trStartDate" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Start Date
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcStartDate" runat="server"></dx:ASPxDateEdit>
                        <%--<SharePoint:DateTimeControl OnValueChangeClientScript="dateChanged()" DateOnly="true"
                ID="dtcStartDate" runat="server" CssClassTextBox="edit-startdate datetimectr datetimectr111 startDateEdit"></SharePoint:DateTimeControl>--%>
                        <asp:Label ID="lbStartDate" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6 colForXS col-sm-6">
                <div id="trDueDate" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Due Date<%--<b style="color: Red;">*</b>--%></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcDueDate" runat="server" CssClass="CRMDueDate_inputField"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="16px">
                        </dx:ASPxDateEdit>
                        <%--                <SharePoint:DateTimeControl DateOnly="true"
                    ID="dtcDueDate" runat="server" CssClassTextBox="edit-duedate datetimectr datetimectr111 endDateEdit"></SharePoint:DateTimeControl>--%>
                        <%--<asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ValidationGroup="Task" ControlToValidate="dtcDueDate"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Please Enter Due Date."></asp:RequiredFieldValidator>--%>
                        <asp:Label ID="dtcDateError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lbDueDate" CssClass="proposeddatelb" runat="server" Visible="false"></asp:Label>
                        <asp:HyperLink ID="btProposeNewDate" Visible="false" NavigateUrl="javascript:void(0);" runat="server" Style="float: left;">
                            <span class="button-bg">
                             <b style="float: left; font-weight: normal;">
                             Propose New Date</b>
                             <i style="float: left; position: relative; top: -1px;left:3px">
                            <img src="/_layouts/15/images/calendar.gif"  style="border:none;" title="" alt=""/></i> 
                             </span>
                        </asp:HyperLink>
                    </div>
                </div>
            </div>
            <div class="col-md-6 colForXS col-sm-6 moreInfo">
                <div id="trProposedDueDate" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Proposed Date</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:Label ID="lbProposedDate" runat="server" CssClass="proposeddatelb" Visible="false"></asp:Label>
                        <asp:HyperLink ID="btApprove" Visible="false" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Accept&nbsp;&nbsp;"
                            ToolTip="Accept" Style="float: left;">
                        <span class="button-bg">
                         <b style="float: left; font-weight: normal;">
                         Accept</b>
                         <i style="float: left; position: relative; top: -2px;left:2px">
                        <img src="/_layouts/15/images/uGovernIT/ButtonImages/approve.png"  style="border:none;" title="" alt=""/></i> 
                           </span>
                        </asp:HyperLink>
                        <asp:HyperLink ID="btReject" Visible="false" NavigateUrl="javascript:void(0);" runat="server" Text="&nbsp;&nbsp;Reject&nbsp;&nbsp;"
                            ToolTip="Accept" Style="float: left;">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                            Reject</b>
                            <i style="float: left; position: relative; top: -2px;left:2px">
                        <img src="/_layouts/15/images/uGovernIT/ButtonImages/reject.png"  style="border:none;" title="" alt=""/></i> 
                            </span>
                        </asp:HyperLink>
                    </div>
                </div>
            </div>


        </div>


        <%--<div class="row"> 
            
        </div>--%>
        <%-- <tr id="trPredecessors" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Predecessors
                    </h3>
                </td>
                <td class="ms-formbody">
                    <div style="height: 150px; overflow-y: scroll; width: 300px; background: white;" id="ltPredecessors" runat="server">
                        <asp:CheckBoxList ID="listPredecessors" SelectionMode="Multiple" CssClass="edit-predecessor predecessorEdit"
                            runat="server">
                        </asp:CheckBoxList>
                    </div>
                    <asp:Label ID="lbPredecessors" runat="server" Visible="false"></asp:Label>
                </td>
            </tr>--%>
        <%--<div class="row">
            
        </div>--%>
        <div class="row col-md-12 col-sm-12 col-xs-12 showMoreLink" runat="server" style="display: none;">
            <a id="infoShowHide" onclick=" return ShowMoreFunction()" class="infoShowHideLabel">Show More >>></a>
        </div>
        <div class="row moreInfo">
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trEstimatedHours" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Estimated Hours
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtEstimatedHours" Width="50" ValidationGroup="Task" CssClass="estimatedhours"
                            runat="server">
                        </asp:TextBox>
                        <asp:Label ID="lbEstimatedHours" runat="server" Visible="false"></asp:Label>
                        hrs
                            <asp:RegularExpressionValidator ID="revEstimatedHours" runat="server" ControlToValidate="txtEstimatedHours"
                                ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter estimated hour in correct format" ForeColor="Red"
                                ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div class="row" id="trActualHours" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Actual Hours
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtActualHours" Width="50" ValidationGroup="Task" CssClass="actualhours"
                            runat="server">
                        </asp:TextBox>
                        <asp:Label ID="lbActualHours" runat="server" Visible="false"></asp:Label>
                        hrs
                            <asp:RegularExpressionValidator ID="revActualHours" runat="server" ControlToValidate="txtActualHours" ForeColor="Red"
                                ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter actual hours in correct format"
                                ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row moreInfo">
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trNote" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Task Description
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox CssClass="full-width" Rows="5" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                        <asp:Label ID="lbDescription" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 colForXS">
                <div id="trComment" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Comment
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <div style="float: left; width: 100%;">
                            <div style="float: left; width: 100%;">
                                <asp:TextBox CssClass="full-width" Rows="5" ID="txtComment" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div style="float: left; width: 100%; display: block; max-height: 150px; overflow-x: auto;">
                                <asp:Repeater ID="rComments" runat="server" OnItemDataBound="RComments_ItemDataBound">
                                    <ItemTemplate>
                                        <div style="float: left; width: 100%;">
                                            <span style="font-weight: bold;"><a href="javascript:void(0);">
                                                <asp:Literal ID="lCommentOwner" runat="server"></asp:Literal></a>
                                                (<a href="javascript:void(0);"><asp:Literal ID="lCommentDate" runat="server"></asp:Literal></a>):</span>
                                            <span>
                                                <asp:Literal ID="lCommentDesc" runat="server"></asp:Literal>
                                            </span>
                                        </di>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>




        <%--  <tr id="trWeight" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Weight
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:TextBox Width="100" ID="txtWeight" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtWeight"
                        ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter weight in digit"
                        ValidationExpression="^[0-9]+"></asp:RegularExpressionValidator>

                    <asp:Label ID="lbWeight" runat="server" Visible="false"></asp:Label>
                </td>

            </tr>--%>
        <%--<tr id="trSvcConfigSelectModules" runat="server">
                <td colspan="2">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader">Module Name
                                </h3>
                            </td>
                            <td class="ms-formbody">
                                <asp:DropDownList Height="22px" ID="ddlModuleDetail" AutoPostBack="true" 
                                    runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                       
                    </table>
                </td>
            </tr>--%>
        <div class="row" id="trSvcSelectModules" runat="server">
            <div>

                <table width="100%" cellpadding="0" cellspacing="0">
                    <%-- <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader">Module Name
                                </h3>
                            </td>
                            <td class="ms-formbody">
                                <asp:DropDownList Height="22px" ID="ddlModules" AutoPostBack="true" OnSelectedIndexChanged="DdlModules_SelectedIndexChanged"
                                    runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                    <%--  <tr>
                            <td class="ms-formlabel">
                                <h3 class="ms-standardheader">Map Ticket<b style="color: Red">*</b>
                                </h3>
                            </td>
                            <td class="ms-formbody">
                                <asp:DropDownList Height="22px" Width="100%" ID="ddlTickets" runat="server">
                                </asp:DropDownList>
                                <asp:Label ID="lbTicketMessage" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>--%>
                </table>

            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btDelete" runat="server" Text="Delete" OnClick="BtDelete_Click" Visible="false" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s, e){return confirmBeforeDelete(this);}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="delete" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn">
                <ClientSideEvents Click="function(){window.parent.CloseWindowCallback(0, '');}" />
            </dx:ASPxButton>
            <dx:ASPxButton Visible="false" ID="btSaveAndNotify" ValidationGroup="Task" OnClick="BtSaveTask_Click" CssClass="primary-blueBtn"
                runat="server" Text="Save & Notify">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btSaveTask" ValidationGroup="Task" OnClick="BtSaveTask_Click" CssClass="primary-blueBtn"
                runat="server" Text="Save">
            </dx:ASPxButton>
            <dx:ASPxButton ID="btSaveMyTask" runat="server" Text="Save" Visible="false" OnClick="BtSaveMyTask_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            <%-- <div style="padding-top: 5px;">
                <asp:Button  OnClientClick="" />

                <asp:Button  />
                <asp:Button  />
                <asp:Button />
                <input type="button" value=""  />
            </div>--%>
        </div>
    </div>

</div>
<asp:HiddenField ID="hdnActionType" runat="server" />
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function checkBeforSave(obj) {

        var status = $.trim($("#<%=ddlStatus.ClientID %>").val());
        var pctComplete = Number($.trim($("#<%=txtPctComplete.ClientID %>").val()));
        var returnBit = true;
        if (status == "Completed" || pctComplete >= 100) {

            var baselinePrompt = false;
            var confirmChildTaskCompletePromt = false;
            if (baselineTaskThreshold && baselineTaskThreshold > 0 && baselineTaskThreshold <= childTaskCount) {
                baselinePrompt = true;
            }

            if (childTaskCount > 0) {
                confirmChildTaskComplete = true;
            }

            if (confirmChildTaskComplete) {
                confirmChildTaskCompletePromt = true;
            }

            if (confirmChildTaskCompletePromt) {
                if (!confirm("This action will also mark all child tasks as Completed. Would you like to proceed?")) {
                    returnBit = false;
                }
            }

            if (baselinePrompt && returnBit) {
                if (confirm("You are changing the status of " + childTaskCount + " tasks to Completed. Would you like to create a baseline first?")) {
                    $("#<%= hdnActionType.ClientID %>").val("BaselineAndSave");
                }
            }
        }

        return returnBit;
    }

    function setStartDateAndDuration() {
        var originalStartDateRaw = $(".startDateEdit").val();
        var originalEndDateRaw = $(".endDateEdit").val()



        var newStartDate = null;
        var selected = $(".predecessorEdit span input:checked").parent();
        for (var i = 0; i < selected.length; i++) {
            var selectedStartDateRaw = selected.get(i).getAttribute("startdate").split('/');
            var selectedStartDate = new Date(selectedStartDateRaw[2].split(" ")[0], selectedStartDateRaw[0] - 1, selectedStartDateRaw[1]);

            if (newStartDate == null || newStartDate < selectedStartDate) {
                newStartDate = selectedStartDate;
            }
        }
        newStartDateRaw = (newStartDate.getMonth() + 1) + "/" + newStartDate.getDate() + "/" + newStartDate.getFullYear();
        var paramsInJson = '{' + '"startDateRaw":"' + originalStartDateRaw + '","endDateRaw":"' + originalEndDateRaw + '","newStartDateRaw":"' + newStartDateRaw + '","addOneMoreDay":"true"}';
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperURL %>/GetNextWorkingDate",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    if ($(".endDateEdit")) { $(".endDateEdit").val(resultJson.enddate); }
                    if ($(".startDateEdit")) { $(".startDateEdit").val(resultJson.startdate); }
                    $(".estimatedhours").val(resultJson.workinghours);
                    lastStartDate = $(".startDateEdit").val();
                }
                else {

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }
    var prevId;
    var lastStartDate = "";
    $(function () {
        //$(".predecessorEdit input").bind("click", function () { setStartDateAndDuration(); })

        //lastStartDate = $(".startDateEdit").val();
        //$(".startDateEdit").bind("change", function (e) {
        //    dateChanged();
        //});
        //$(".endDateEdit").bind("change", function (e) {
        //    dateChanged();
        //});
    });

    function dateChanged() {
        var startDate = $(".startDateEdit").val();
        var endDate = $(".endDateEdit").val();
        if (startDate == "" || endDate == "" || lastStartDate == "") {
            lastStartDate = startDate;
            return;
        }

        //alert('' + '"startDateRaw":"' + lastStartDate + '","endDateRaw":"' + endDate + '","newStartDateRaw":"' + startDate + '"');
        var paramsInJson = '{' + '"startDateRaw":"' + lastStartDate + '","endDateRaw":"' + endDate + '","newStartDateRaw":"' + startDate + '","addOneMoreDay":"false"}';
        $.ajax({
            type: "POST",
            url: "<%=ajaxHelperURL %>/GetNextWorkingDate",
            data: paramsInJson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var resultJson = $.parseJSON(message.d);
                if (resultJson.messagecode == 2) {
                    if ($(".startDateEdit").length > 0) { $(".startDateEdit").val(resultJson.startdate); }
                    if ($(".endDateEdit").length > 0) { $(".endDateEdit").val(resultJson.enddate); }
                    $(".estimatedhours").val(resultJson.workinghours);
                    lastStartDate = $(".startDateEdit").val();
                }
                else {

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
    }

    function modifyStatusFromPctComplete() {
        var statusObj = $("#<%= ddlStatus.ClientID %>");
        var pctCompleteObj = $("#<%= txtPctComplete.ClientID%>");
        var pctComplete = Number($.trim(pctCompleteObj.val()));

        if (pctComplete <= 0) {
            statusObj.val("Not Started");
            pctCompleteObj.val("0");
        }
        else if (pctComplete >= 100) {
            statusObj.val("Completed");
            pctCompleteObj.val("100");
        }
        else {
            statusObj.val("In Progress");
        }
    }

    function modifyPctCompleteFromStatus() {
        var statusObj = $("#<%= ddlStatus.ClientID %>");
        var pctCompleteObj = $("#<%= txtPctComplete.ClientID%>");
        var pctComplete = Number($.trim(pctCompleteObj.val()));
        if (statusObj.val() == "Not Started") {
            pctCompleteObj.val("0");
        }
        else if (statusObj.val() == "Completed") {
            pctCompleteObj.val("100");
        }
        else if (statusObj.val() != "Completed" && pctComplete >= 100) {
            pctCompleteObj.val("90");
        }
    }

    function confirmBeforeDelete(obj) {

        var message = "";

        message = "Are you sure you want to delete this task?";


        if (confirm(message)) {
            return true;
        }

        return false;
    }

    $(document).ready(function () {
        if ("<%=isModuleConstraint%>" == "1") {
            $(".showMoreLink").show();
            $('.moreInfo').hide();
        }
        else {
            $(".showMoreLink").hide();
            $('.moreInfo').show();
        }
    });

    function ShowMoreFunction() {
        $('.moreInfo').toggle();
        $('#infoShowHide').text($('#infoShowHide').text() == 'Show Less >>>' ? 'Show More >>>' : 'Show Less >>>');
    }
</script>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="uGovernIT.Web._UserInfo" MasterPageFile="~/master/Root.master" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<asp:Content ID="MainBody" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%= ResolveUrl(@"~/Content/uGITCommon.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />
    <link href="<%= ResolveUrl(@"~/Content/token-input.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />
    <link href="<%= ResolveUrl(@"~/Content/token-input-facebook.css") + "?v=" + UGITUtility.AssemblyVersion %>" rel="stylesheet" />

    <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        body {
            overflow-y: auto !important;
        }

        #s4-leftpanel {
            display: none;
        }

        .s4-ca {
            margin-left: 0px !important;
        }

        #s4-ribbonrow {
            height: auto !important;
            min-height: 0px !important;
        }

        #s4-ribboncont {
            display: none;
        }

        #s4-titlerow {
            display: none;
        }

        .s4-ba {
            width: 100%;
            min-height: 0px !important;
        }

        #s4-workspace {
            float: left;
            width: 100%;
            overflow: auto !important;
        }

        body #MSO_ContentTable {
            min-height: 0px !important;
            position: inherit;
        }

        .ms-formbody accomp_inputField {
            background: none repeat scroll 0 0 #E8EDED;
            border-top: 1px solid #A5A5A5;
            padding: 3px 6px 4px 0px;
            vertical-align: top;
        }

        .ms-formlabel {
            width: 190px;
        }

        .pctcomplete {
            text-align: right;
        }

        .full-width {
            width: 98%;
        }

        .viewpanel {
            float: left;
            width: 98%;
        }

        .editpanel {
            float: left;
            width: 98%;
            padding-left: 10px;
        }

        .txtbox-width {
            width: 167px;
        }

        .txtbox-halfwidth {
            width: 167px;
        }

        .ms-standardheader budget_fieldLabel {
            text-align: right !important;
        }

        .button-red {
            color: white;
            background: url("/content/images/firstnavbgRed.png") repeat-x scroll 0 0 transparent;
            float: left;
            margin: 1px;
            padding: 4px 6px 6px;
            cursor: pointer;
        }

        .chkbxShowPassword input {
            margin: 6px 5px 5px 0px;
        }

        .hideOutofOfficePanel {
            display: none;
        }

        .enableoutofoffice .on {
            color: white;
            background: green;
            font-weight: bold;
            margin-left: 3px;
            padding: 2px;
        }

        .enableoutofoffice .off {
            color: white;
            background: red;
            margin-left: 3px;
            padding: 2px;
        }

        .enableoutofoffice .read {
            width: 100%;
            background: rgb(243, 188, 188) none repeat scroll 0% 0%;
            font-weight: bold;
            margin-top: 5px;
            margin-left: 3px;
        }

        .outofofficedate {
            width: 70px;
        }

        .enableoutofoffice .readdegatefor {
            width: 100%;
            background: rgb(243, 188, 188) none repeat scroll 0% 0%;
            font-weight: bold;
        }

        .setalign {
            float: left;
        }

        /*.cssassettimeline {
            float: right;
            display: inline-flex;
        }*/

        .hide {
            display: none;
        }

        .profileUserImg {
            height: 35px;
            width: 35px;
            border-radius: 35px;
        }

        .ProfileLabel{
            display:none;
            float:right;
            color: #4b4b4b;
            font-size: 12px;
            font-weight: 800;
        }
    </style>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            function OnComboBoxSelectedIndexChanged(s, e) {
                var selectedValue = s.GetValue(); // Get selected value
                PnlCallbackJobTitle_Role.PerformCallback(selectedValue);
            }
        var editGroupUrl = '<%= editGroupUrl %>';

        function onChange(s, e) {
            s.ClearTokenCollection();
            s.AddItem(s.GetSelectedIndex());

        }

        function DeleteUser(s, e) {
            if (confirm("Are you sure you want to delete this user from the site?(Choose Cancel button and Reassign Open/ In-progress tickets to another user(s), before deleting User.)")) {
                e.processOnServer = true;
                return true;
            }
            else {
                e.processOnServer = false;
                return false;
            }
        }
        var doenable ='<%=IsEnable%>';
        doenable = doenable.toLowerCase().trim();
        var invokepopup = false;
        $(function () {

            ResetGridTr();
            $('#<%=chkbxShowPassword.ClientID%>').change(function () {

                showPassword();
            });


            $('.chkkeeptrack :checkbox').click(function () {
                if (!this.checked)
                    invokepopup = true;
                else
                    invokepopup = false;
            });
        });



        function ResetGridTr() {
            $(".gridlookup-view tr").each(function () {

                if ($(this).css('display') == 'none') {
                    $(this).css('display', '');
                }
            });
        }

        function btnResetPassword_Click() {
            window.parent.UgitOpenPopupDialog("<%= absoluteURL("/Layouts/ugovernit/DelegateControl.aspx")%>?control=changepassword&resetUserPwd=1&userCode=<%=userID%>", "", 'Reset Password', "500px", "540px");
        }

        function GotoParentWindow(mailParameter) {
            if (mailParameter == "ismail") {
                window.top.location.href = "<%= userHomePage %>";
            }
            else if (mailParameter == "istrailuser") {
                history.pushState({}, null, "/Pages/RMM");
            }
        }


        function OnSaveClientCick() {

            if ($('#<%=chkOutOfOffice.ClientID%>').is(':checked')) { return enableOutofOfficeErrorhandel(); }


            var skillData = [];
            var strSkillId = $("#demo-input").val().split(',');
            var tempcount = 0;
            $(".token-input-token-facebook p").each(function (index) {
                skillData.push({ id: strSkillId[tempcount], name: $(this).text() });
                tempcount++;
            });

            $("#<%= skillJson.ClientID%>").val(JSON.stringify(skillData));
            if (invokepopup) {
                confirmDisablePopup.Show();
                return false;
            }
            else
                return true;
        }
        function generateRandomPassword() {
            $.ajax({
                type: "POST",
                url: "<%= ajaxHelperURL%>/GenerateRandomPassword",
                data: "",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {
                    var randomPassword = message;

                    $('#<%=txtPassword.ClientID%>').val(randomPassword);
                    $('#<%=txtReenterPassword.ClientID%>').val(randomPassword);

                },
                error: function (xhr, ajaxOptions, thrownError) {
                }
            });
        }
        function showPassword() {

            if ($('#<%=chkbxShowPassword.ClientID%>').is(":checked")) {
                $('#<%=txtPassword.ClientID%>').attr("type", "text");
                $('#<%=txtReenterPassword.ClientID%>').attr("type", "text");

            }
            else {
                $('#<%=txtPassword.ClientID%>').attr("type", "password");
                $('#<%=txtReenterPassword.ClientID%>').attr("type", "password");
            }
        }
        function EnableOutofOfficehandel() {
            if ($('#<%=chkOutOfOffice.ClientID%>').is(':checked')) {
                $('#<%=outOfOfficePanelEdit.ClientID%>').css("display", "block");
            }
            else {
                $('#<%=outOfOfficePanelEdit.ClientID%>').css("display", "none");
            }
        }

        function enableOutofOfficeErrorhandel() {
            // var from=  document.getElementById('LeaveFromDate.Controls[0].ClientID%>').value;
            // var to= document.getElementById('LeavetoDate.Controls[0].ClientID%>').value;

            var from = LeaveFromDate.GetDate();
            var to = LeavetoDate.GetDate();


            if ($('#<%=chkOutOfOffice.ClientID%>').is(':checked') && (from == "" || to == "")) {
                $('#<%=this.tr660.ClientID%>').css("display", "block ");
                $('#<%=this.errorMsg.ClientID%>').text('Please enter Dates !!');

                return false;
            }
            else {
                $('#<%=this.tr660.ClientID%>').css("display", "none");
                return true;
            }
        }

        function ValidateWorkingHours(s, e) {
            var startdate = new Date();
            startdate.setHours(dtWorkingHoursStart.GetDate().getHours(), dtWorkingHoursStart.GetDate().getMinutes(), dtWorkingHoursStart.GetDate().getSeconds());
            var enddate = new Date();
            enddate.setHours(dtWorkingHoursEnd.GetDate().getHours(), dtWorkingHoursEnd.GetDate().getMinutes(), dtWorkingHoursEnd.GetDate().getSeconds());
            if (startdate >= enddate) {
                var newenddate = dtWorkingHoursStart.GetDate();
                dtWorkingHoursEnd.SetValue(newenddate);
            }
        }

        function ShowTimeLineTickets(s, e) {
            e.processOnServer = false;
           <% if (userInfo != null)
        {%>
            var user ='<%=userInfo.Name.Replace("'",string.Empty)%>';
            if (user != "" && user != undefined && user != null) {
                var url ='<%=openCloseTicketsForRequestorUrl%>&userId=<%=userInfo.Id%>';
                window.parent.UgitOpenPopupDialog(url, "", user + "&#39;s Tickets", 90, 90);
            }
            <%}%>
        }



        function ShowAssignedAssets(s, e) {
            e.processOnServer = false;
            <%if (userInfo != null)
        {%>
            var userid ='<%=userInfo.Id%>';
            if (userid != "" && userid != undefined && userid != null) {
                //var url = '<%=assetUrl%>';
                var url = "/layouts/ugovernit/delegateControl.aspx?control=userprofilerelatedassets&listName=Assets&Module=CMDB&UserId=" + userid;
                window.parent.UgitOpenPopupDialog(url, "", "Assets Details", 90, 90);
            }
            <%}%>
        }

        function DisableUser(obj) {
            var checked = obj.checked;
            if (!checked) {
                confirmDisablePopup.Show();
                return true;
            }
            else {
                confirmDisablePopup.Hide();
                return false;
            }
        }

        function DeleteFromGroupandUpdate(obj) {
            hdnkeepAction.Set('action', '');

            hdnkeepAction.Set('action', obj);
            $('#<%=btnDisableIndividualUser.ClientID%>').trigger('click');
        }
        function openResourceTimesheet(s, e) {

         <% if (userInfo != null)
        {%>

            var user = '<%=userInfo.Name.Replace("'",string.Empty)%>';
            if (user != "" && user != undefined && user != null) {
                var url = "/layouts/ugovernit/delegatecontrol.aspx?control=ResourceAllocationGrid&SelectedResource=<%=  userInfo.Id%>";
                window.parent.UgitOpenPopupDialog(url, "", "Timeline for User : " + user, "95", "95", false, "");
            }
                <%}%>
        }
        function openAssociatedGroups(s, e) {
               <% if (userInfo != null)
        {%>

            var user = '<%=userInfo.Name.Replace("'",string.Empty)%>';
            if (user != "" && user != undefined && user != null) {
                var url = "/layouts/ugovernit/delegatecontrol.aspx?control=AssociatedGroups&id=<%=  userInfo.Id%>";
                window.parent.UgitOpenPopupDialog(url, "", "Associated Groups : " + user, "25", "50", false, "");
            }
                <%}%>
        }

        function ShowMoreFunction(s, e) {
            $('#moreInfo').toggle();
            $('#infoShowHide').text($('#infoShowHide').text() == 'Show Less >>>' ? 'Show More >>>' : 'Show Less >>>');
        }

        function addNewGroup() {
            window.UgitOpenPopupDialog(editGroupUrl, '', 'Add New Group', "550px", "300px", false, escape("<%= Request.Url.AbsolutePath %>"));
        }

        function DisplayDialog() {
            var encodedMessage = DevExpress.utils.string.encodeHtml("You have exceeded number of users in the Trial plan.Please press 'Okay' to Purchase");
            var myDialog = DevExpress.ui.dialog.custom({
                title: "Limitation for Users",
                messageHtml: encodedMessage,
                toolbarItems: [
                    { text: "Okay", onClick: function () { window.top.location.href = "/purchase.aspx" } },
                    { text: "Cancel", onClick: function () { window.parent.CloseWindowCallback(1, document.location.href); } }
                ]

            });
            myDialog.show().done(function (dialogResult) {
                /*console.log(dialogResult.buttonText);*/
            });
        }

        $(document).ready(function () {
            $('#dialog').dialog({
                autoOpen: false,
                width: 550,
                hieght: 200,
                modal: true,
                title: "User Creation Details",
                buttons: [{
                    text: "Ok",
                    "class": 'okayButton',
                    click: function () {
                        window.top.location.href = "/purchase.aspx";
                    }
                },
                {
                    text: "Cancel",
                    click: function () {
                        $(this).dialog("close");
                    }
                }]
            });
        });

        function ValidateUser() {
            var checkLimit = '<%=limitExceed%>';
            if (checkLimit.toLowerCase() == 'true') {
                window.top.location.href = "/purchase.aspx";
                // $("#dialog").html("<div class='bulkuser-SucsMsg'> Users Created Sucessfully.  You have exceeded number of users in the Trial plan And List of the user(s) are not added given below <br/></div>");

                // var theDialog = $("#dialog").dialog();

                //theDialog.dialog("open");


                //DisplayDialog();
            }

        }
        function deleteResume() {
            var confrm = confirm("Are you sure you want to delete?");
            if (confrm) {
                $('#<%=btnDeleteResume.ClientID%>').click();
            }
        }

        function onDepartmentChanged(ccID) {
            debugger;
            var cmbDepartment = $("#" + ccID + " span");

            //This code is used for multi select depts
            //var selectedDepts = "";
            //for (i = 0; i < cmbDepartment.length; i++)
            //    selectedDepts = selectedDepts + cmbDepartment[i].id + ",";
            var selectedDepartments = cmbDepartment.attr("id"); //this code is used in case of single select dept
            var dxStudioLookup = ASPxClientControl.GetControlCollection().GetByName("ddlStudio"); 
            var dxjobPropDown = ASPxClientControl.GetControlCollection().GetByName("cmbJobTitle"); 
            if (selectedDepartments > 1) {
                // obj = ['DummyValue', 'Tells if Param#3 is Div or Dept', 'Param#3. Used to filter studios']
                var obj = ['Filter', 'DepartmentID', selectedDepartments];
                if (dxStudioLookup) {
                    dxStudioLookup.GetGridView().PerformCallback(obj);
                }
                dxjobPropDown.PerformCallback(selectedDepartments);
            }
        }

    </script>
    <script data-v="<%=UGITUtility.AssemblyVersion %>">
        $(document).ready(function () {
            $('#<%=lblRmmenable.ClientID%>').attr('for', $('#<%=chkEnable.ClientID%>').attr('id'));
            $('#<%=lblRmmpwdExp.ClientID%>').attr('for', $('#<%=chkEnablePwdExpiration.ClientID%>').attr('id'));
            $('#<%=lblRmmworkFlowDis.ClientID%>').attr('for', $('#<%=chkDisableWorkflowNotifications.ClientID%>').attr('id'));
            $('#lblRmm-outOff').attr('for', $('#<%=chkOutOfOffice.ClientID%>').attr('id'));
            $("#<%=lblRmmIT.ClientID%>").attr('for', $('#<%=cbIT.ClientID%>').attr('id'));
            $('#<%=lblRmmConsultant.ClientID%>').attr('for', $('#<%=cbIsConsultant.ClientID%>').attr('id'));
            $('#<%=lblRmmManager.ClientID%>').attr('for', $('#<%=cbIsManager.ClientID%>').attr('id'));
            $('#lblRmm-showPwd').attr('for', $('#<%=chkbxShowPassword.ClientID%>').attr('id'));
            $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
            $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        });

        function validateUserName(s, e) {
            if ($("#<%=txtUserName.ClientID%>").val().trim() == '') {
                $('#lblUserNameMsg').html('Please enter UserName')
                e.processOnServer = false;
            }
        }
        //Code to disable autoComplete for Password fields & User Groups, in Chrome.
        $(document).ready(function () {
            $(".lookUpValueBox-dropDown").attr("autocomplete", "new-group");
            $("#<%=txtPassword.ClientID%>").attr("autocomplete", "new-password");
            $("#<%=txtReenterPassword.ClientID%>").attr("autocomplete", "new-password");
            $(".childCerti").hide();
            $(".childSkill").hide();
            $('#btnAddCretificate').dxButton({
                icon: 'plus',
                onClick() {
                    let selection = tbCertificateValue.GetValue();
                    let drpValue = tbCertificate.GetValue();
                    if (drpValue == "") {
                        tbCertificate.SetValue(tbCertificateValue.GetValue());
                    }
                    else {
                        if (selection != "") {
                            tbCertificate.SetValue(drpValue + "," + selection);
                        }
                    }
                    ShowHideCertificatePanel();
                },
            });
            $('#btnAddSkill').dxButton({
                icon: 'plus',
                onClick() {
                    let selection = tbSkillValue.GetValue();
                    let drpValue = tbSkills.GetValue();
                    if (drpValue == "") {
                        tbSkills.SetValue(tbSkillValue.GetValue());
                    }
                    else {
                        if (selection != "") {
                            tbSkills.SetValue(drpValue + "," + selection);
                        }
                    }
                    ShowHideSkillPanel();
                },
            });
        });
        function OnCategoryChanged() {
            tbCertificateValue.PerformCallback(CmbCertificateCat.GetValue().toString());
        }
        function ShowHideCertificatePanel() {
            if ($(".childCerti").is(":visible")) {
                $(".childCerti").hide();
                $(".parentCerti").show();
            }
            else {
                $(".childCerti").show();
                $(".parentCerti").hide();
            }
        }
        function OnSkillCategoryChanged() {
            tbSkillValue.PerformCallback(CmbSkillCat.GetValue().toString());
        }
        function ShowHideSkillPanel() {
            if ($(".childSkill").is(":visible")) {
                $(".childSkill").hide();
                $(".parentSkill").show();
            }
            else {
                $(".childSkill").show();
                $(".parentSkill").hide();
            }
        }
    </script>
    <div id="dialog"></div>
    <dx:ASPxHiddenField ID="hdnkeepAction" runat="server" ClientInstanceName="hdnkeepAction"></dx:ASPxHiddenField>
    <asp:Panel ID="viewPanel" CssClass="viewpanel" runat="server">

        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback">
            <ClientSideEvents CallbackComplete="function(s, e) { saveUserLoading.Hide(); }" />
        </dx:ASPxCallback>

        <div class="ms-formtable accomp-popup">
            <div id="tr0" runat="server" class="row col-md-12 col-sm-12 col-xs-12 pt-2">
                <asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red" />
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Login Name <b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField">
                    <asp:Image runat="server" ID="ProfileImg" CssClass="profileUserImg"></asp:Image>&nbsp;&nbsp;<asp:Label ID="lblUserName" runat="server"></asp:Label>

                    &nbsp;&nbsp;<img alt="Change User Name" id="ImgUserName" src="/content/images/editNewIcon.png" runat="server" visible="false" title="Change User Name" style="cursor: pointer; width: 16px;" onclick="javascript:$('.trUserName').attr('style','display:block');" />

                    <div id="divassetsTickets" runat="server" class="cssassettimeline">
                        <div style="display: inline-block;">
                            <dx:ASPxButton ID="lnkassets" runat="server" Visible="true" Text="Assets" CssClass="secondary-cancelBtn">
                                <ClientSideEvents Click="ShowAssignedAssets" />
                            </dx:ASPxButton>

                        </div>
                        <div style="display: inline-block;">
                            <dx:ASPxButton ID="lnktimeline" runat="server" Visible="true" Text="Tickets" CssClass="secondary-cancelBtn">
                                <ClientSideEvents Click="ShowTimeLineTickets" />
                            </dx:ASPxButton>

                        </div>
                        <div style="display: inline-block;">
                            <dx:ASPxButton ID="btntimeline" runat="server" Visible="true" AutoPostBack="false" Text="Timeline" CssClass="secondary-cancelBtn">
                                <ClientSideEvents Click="function(s,e){openResourceTimesheet(s,e);}" />
                            </dx:ASPxButton>

                        </div>
                        <div style="display: inline-block;">
                            <dx:ASPxButton ID="btnGroupAssociated" runat="server" Visible="true" AutoPostBack="false" Text="Groups Associated" CssClass="secondary-cancelBtn">
                                <ClientSideEvents Click="function(s,e){openAssociatedGroups(s,e);}" />
                            </dx:ASPxButton>

                        </div>
                    </div>

                </div>
            </div>

            <div id="trUserName" class="row col-md-12 col-sm-12 col-xs-12 trUserName" runat="server" style="display: none;">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">New Login Name<b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField">
                    <asp:TextBox ID="txtUserName" CssClass="txtbox-width" runat="server"></asp:TextBox>
                    <img alt="Cancel Editing User Name" width="12" height="10" src="/content/images/cancel-icon.png" class="cancelModule" onclick="javascript:$('.trUserName').attr('style','display:none'); $('#<%=txtUserName.ClientID%>').val('');$('#lblUserNameMsg').html('')" />
                    <label id="lblUserNameMsg" style="color: red"></label>
                </div>
            </div>

            <div id="tr1" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Name<b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField">
                    <asp:Label ID="lbName" runat="server"></asp:Label>
                    <asp:TextBox ID="txtName" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtNameRequired" CssClass="error" runat="server" ControlToValidate="txtName" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter name">
                    </asp:RequiredFieldValidator>

                </div>
            </div>

            <div id="trLoginName" runat="server" visible="false" class="row col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Login Name<b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField">
                    <asp:TextBox ID="txtLoginName" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoginName" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter valid Login name">
                    </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvUserName" runat="server" ControlToValidate="txtLoginName" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="User is already exits" OnServerValidate="cvUserName_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                </div>
            </div>

            <div id="tr2" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">E-Mail<b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField">
                    <asp:Label ID="lbEmail" runat="server"></asp:Label>
                    <asp:TextBox ID="txtEmail" CssClass="txtbox-width" runat="server" Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtEmailRequired" CssClass="error" runat="server" ControlToValidate="txtEmail" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter email">
                    </asp:RequiredFieldValidator>

                    <asp:RegularExpressionValidator ID="txtEmailRegularExpress" CssClass="error" ControlToValidate="txtEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" Display="Dynamic" ErrorMessage="Enter valid email"
                        ValidationGroup="savechanges" runat="server" />
                    <asp:CustomValidator ID="cvEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Email is already exits" OnServerValidate="cvEmail_ServerValidate" ForeColor="red"></asp:CustomValidator>


                </div>
            </div>

            <div class="row col-md-12 col-sm-12 col-xs-12" id="tr17" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel  userinfo-grouplabel">User Groups
                    </h3>
                    <asp:Label ID="lbCreateGroup" runat="server" CssClass="addgroupicon">
                    <img class="newUserGroupIMG" src="/content/Images/add-groupBlue.png" alt="Add" title="Add Group" onclick="addNewGroup()" style="width:18px;" />
                    </asp:Label>
                </div>


                <div class="ms-formbody rmm-inputField">
                    <asp:Label ID="lblUserGroups" runat="server" Visible="false"></asp:Label>
                    <div id="userGroupList" runat="server">
                        <ugit:LookUpValueBox ID="rolesListBox" CssClass="lookUpValueBox-dropDown" IsSearch="true" FieldName="MultiRoles" runat="server" ShowSelectAllCheckbox="false" />
                    </div>
                </div>
            </div>
            <div class="row col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel" runat="server">
                    <h3 class="ms-standardheader budget_fieldLabel">Landing Page
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField">
                    <asp:Label ID="lblUserRole" runat="server"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlUserRole" CssClass="aspxDropDownList">
                    </asp:DropDownList>
                </div>
            </div>

            <div id="trTypeofUser" runat="server" visible="false" class="row col-md-12 col-sm-12 col-xs-12">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Type of User<b style="color: Red">*</b>
                    </h3>

                </div>
                <div class="ms-formbody rmm-inputField">
                    <dx:ASPxComboBox runat="server" ID="ddlUserType">
                        <Items>
                            <dx:ListEditItem Text="AD User" Value="0" />
                            <dx:ListEditItem Text="FBA User" Value="1" />
                        </Items>
                    </dx:ASPxComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlUserType" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please select user type">
                    </asp:RequiredFieldValidator>

                </div>
            </div>

            <div id="trPasword" runat="server" class="row col-md-12 col-sm-12 col-xs-12" visible="false">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Password<b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField row">
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                        <asp:Label ID="Label2" runat="server"></asp:Label>
                        <asp:TextBox ID="txtPassword" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter password">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-2 col-sm-2 col-xs-2 noPadding">
                        <div class="rmmGrnPwd-btnWrap">
                            <input type="button" id="btnRandomPassword" class="rmmGrnPwd-btn" tabindex="-1" onclick="generateRandomPassword()" name="GeneratePassword" value="Generate" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="trReenterPassword" class="row col-md-12 col-sm-12 col-xs-12" runat="server" visible="false">
                <div class="ms-formlabel" style="vertical-align: top;">
                    <h3 class="ms-standardheader budget_fieldLabel">Re-enter password<b style="color: Red">*</b>
                    </h3>
                </div>
                <div class="ms-formbody rmm-inputField row">
                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                        <asp:Label ID="Label3" runat="server"></asp:Label>
                        <asp:TextBox ID="txtReenterPassword" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="error" runat="server" ControlToValidate="txtReenterPassword" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please reenter password">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" CssClass="error" runat="server" ErrorMessage="Re-entered password must be the same" ControlToCompare="txtPassword" ControlToValidate="txtReenterPassword" Display="Dynamic" SetFocusOnError="True"></asp:CompareValidator>
                        <br />
                        <%-- <label for="chkbxShowPassword"><input type="checkbox" id="chkbxShowPassword" /> <span>Show Password</span></label>--%>
                        <div class="rmm-chkWrap crm-checkWrap showPwd-chkWrap" style="margin-left: -5px;">
                            <input type="checkbox" id="chkbxShowPassword" runat="server" class="chkbxShowPassword" />
                            <label class="rmm-Label" id="lblRmm-showPwd">&nbsp; Show Password  &nbsp;</label>
                            <%--<asp:checkbox id="chkbxshowpassword" runat="server" text="show password" cssclass="chkbxshowpassword" />--%>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12 col-sm-12 col-xs-12" style="padding-top: 7px; padding-bottom: 5px;">
                <div class="ms-formbody rmm-inputField crm-checkWrap" style="margin-left: -5px;">
                    <asp:CheckBox ID="chkimidiate" runat="server" Text="Create Immediately" AutoPostBack="true" OnCheckedChanged="chkimidiate_change"
                        ToolTip="Create Immediately" TextAlign="Right" />
                </div>
            </div>
            <div class="row col-md-12 col-sm-12 col-xs-12 showMoreLink" runat="server">
                <a id="infoShowHide" onclick=" return ShowMoreFunction()" class="infoShowHideLabel">Show More >>></a>
            </div>

            <div id="messageTR" runat="server" visible="false" class="row col-md-12 col-sm-12 col-xs-12">
                <div style="text-align: center; font-size: small; color: red;">
                    <asp:Literal ID="lblMessage" runat="server"></asp:Literal>
                </div>
            </div>

            <div id="moreInfo" style="display: none;">



                <div id="tr51" class="row col-md-12 col-sm-12 col-xs-12" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Upload Profile Pic<b style="color: Red"></b>
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblProfilePic" runat="server" CssClass="ProfileLabel"></asp:Label>
                        <asp:FileUpload ID="FileUploadUserPics" runat="server" accept="image/*" multiple="false" />
                        <asp:RegularExpressionValidator ID="rvFileUploadUserPics"
                            runat="server" ControlToValidate="FileUploadUserPics"
                            ErrorMessage="Only .jpg, .jpeg, .png image formats are allowed." ForeColor="Red"
                            ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.png|.JPG|.JPEG|.PNG)$" Display="Dynamic"
                            ValidationGroup="savechanges" SetFocusOnError="true"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div id="tr24" runat="server" class="row col-md-12 col-sm-12 col-xs-12" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Notification E-Mail
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbNotificationEmail" runat="server"></asp:Label>
                        <asp:TextBox ID="txtNotificationEmail" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regexNotificationEmail" CssClass="error" ControlToValidate="txtNotificationEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" Display="Dynamic" ErrorMessage="Enter valid email"
                            ValidationGroup="savechanges" runat="server" />
                    </div>
                </div>
                <div id="tr3" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Phone Number
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbMobileNumber" runat="server"></asp:Label>
                        <asp:TextBox ID="txtMobileNumber" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="txtMobileRegularExpression" runat="server" ControlToValidate="txtMobileNumber" ValidationGroup="savechanges" Display="Dynamic" ValidationExpression="^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$" ErrorMessage="Enter valid phone number" CssClass="error" />
                    </div>
                </div>

                <div id="tr26" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Employee Id
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbEmployeeId" runat="server"></asp:Label>
                        <asp:TextBox ID="txtEmployeeId" CssClass="txtbox-width" runat="server" Visible="false" MaxLength="100"></asp:TextBox>
                    </div>
                </div>
                <div id="Div1" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Employee Type
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="Label1" runat="server"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlEmpType" CssClass="aspxDropDownList">
                        </asp:DropDownList>
                    </div>
                </div>
                <div id="tr4" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel"><%=departmentLabel %>
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbDepartmentCtr" runat="server"></asp:Label>
                        <ugit:LookupValueBoxEdit ID="departmentCtr" CssClass="rmmLookup-valueBoxEdit" runat="server" FieldName="DepartmentLookup" JsCallbackEvent="onDepartmentChanged"></ugit:LookupValueBoxEdit>

                    </div>
                </div>
                <div id="tr99" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Studio
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbStudio" runat="server"></asp:Label>
                        <ugit:LookUpValueBox ID="ddlStudio" runat="server" ClientInstanceName="ddlStudio" CssClass="lookupValueBox-dropown field_studiolookup_edit" Width="100%" FieldName="StudioLookup" />
                    </div>
                </div>
                <dx:ASPxCallbackPanel ID="PnlCallbackJobTitle_Role" ClientInstanceName="PnlCallbackJobTitle_Role" runat="server" OnCallback="PnlCallbackJobTitle_Role_Callback">
                    <PanelCollection>
                        <dx:PanelContent>
                            <div class="row col-md-6 col-sm-6 col-xs-6" id="tr5" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Job Title
                                    </h3>
                                </div>
                                <div class="ms-formbody rmm-inputField">
                                    <asp:Label ID="lbJobTitle" runat="server"></asp:Label>

                                    <dx:ASPxComboBox ID="cmbJobTitle" Visible="false" DropDownHeight="200px" AutoPostBack="false" AllowMouseWheel="false" ClientInstanceName="cmbJobTitle" runat="server" CssClass="CRMDueDate_inputField comboBox-dropDown" Width="250px" OnCallback="CmbJobTitles_Callback">
                                        <ClientSideEvents SelectedIndexChanged="OnComboBoxSelectedIndexChanged" />
                                    </dx:ASPxComboBox>
                                </div>
                            </div>

                            <div class="row col-md-6 col-sm-6 col-xs-6" id="divRole" runat="server">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Role
                                    </h3>
                                </div>
                                <div class="ms-formbody rmm-inputField">
                                    <asp:Label ID="lblUsersRole" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtUsersRole" runat="server" Enabled="false" Style="background: lightgray;"></asp:TextBox>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr15" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Functional Area
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblFunctionalArea" runat="server"></asp:Label>
                        <ugit:LookUpValueBox ID="ddlFunctionalArea" Width="100%" CssClass="lookupValueBox-dropown" FieldName="FunctionalAreaLookup" runat="server" />
                    </div>
                </div>



                <div id="tr6" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Hourly Rate($)
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbHourlyRate" runat="server"></asp:Label>
                        <asp:TextBox ID="txtHourlyRate" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ForeColor="Red" runat="server" ControlToValidate="txtHourlyRate" ErrorMessage="Only Numbers Allowed" Display="Dynamic" ValidationGroup="savechanges" ValidationExpression="^(^(\$)?\d+(\.\d+)?$|^(-)?\d+(\.\d+)?$)$">

                        </asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr25" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Approve Level Amount($)
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbApproveLevelAmount" runat="server"></asp:Label>
                        <asp:TextBox ID="txtApproveLevelAmount" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revApprovalLevelAmount" runat="server" ForeColor="Red" ControlToValidate="txtApproveLevelAmount" EnableClientScript="true" ErrorMessage="Only Numbers Allowed" Display="Dynamic" ValidationGroup="savechanges" ValidationExpression="^(^(\$)?\d+(\.\d+)?$|^(-)?\d+(\.\d+)?$)$">

                        </asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr7" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Manager
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbManager" runat="server"></asp:Label>
                        <ugit:UserValueBox ID="managerUser" SelectionSet="User" runat="server" CssClass="crmDropDown_field userValueBox-dropDown" />

                    </div>
                </div>

                <div class="col-md-12 col-sm-12 col-xs-12" id="tr8" runat="server">
                    <div class="ms-formlabel">
                        <h3 id="h3It" class="ms-standardheader budget_fieldLabel" runat="server" visible="false">IT
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lbIT" runat="server"></asp:Label>
                        <asp:CheckBox ID="cbIT" runat="server" Visible="false" />
                        <label class="rmm-Label" id="lblRmmIT" runat="server">&nbsp; IT  &nbsp;</label>
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr9" runat="server">
                    <div class="ms-formlabel">
                        <h3 id="h3Consultant" class="ms-standardheader budget_fieldLabel" runat="server" visible="false">Consultant</h3>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lbIsConsultant" runat="server"></asp:Label>
                        <asp:CheckBox ID="cbIsConsultant" runat="server" Visible="false" />
                        <label class="rmm-Label" id="lblRmmConsultant" runat="server">&nbsp; Consultant  &nbsp;</label>
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr10" runat="server">
                    <div class="ms-formlabel">
                        <h3 id="h3Manager" class="ms-standardheader budget_fieldLabel" runat="server" visible="false">Manager </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lbIsManager" runat="server"></asp:Label>
                        <asp:CheckBox ID="cbIsManager" runat="server" Visible="false" />
                        <label class="rmm-Label" id="lblRmmManager" runat="server">&nbsp; Manager  &nbsp;</label>
                    </div>
                </div>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr101" runat="server">
                    <div class="ms-formlabel">
                        <h3 id="h3ServiceAccount" class="ms-standardheader budget_fieldLabel" runat="server" visible="false">Service Account </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lblIsServiceAccount" runat="server"></asp:Label>
                        <asp:CheckBox ID="cbIsServiceAccount" runat="server" Visible="false" />
                        <label class="rmm-Label" id="lblServiceAccount" runat="server">&nbsp; Service Account  &nbsp;</label>
                    </div>
                </div>

                <div id="tr12" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Budget Category
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbBudgetCategory" runat="server"></asp:Label>
                        <asp:UpdatePanel ID="budgetPanel" runat="server">
                            <ContentTemplate>
                                <div class=" col-md-6 col-sm-6 col-xs-6 noPadding">
                                    <asp:DropDownList ID="ddlBudgetCategory" AutoPostBack="true" runat="server" Width="167px" Visible="false" CssClass="aspxDropDownList"
                                        OnSelectedIndexChanged="DDLBudgetCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class=" col-md-6 col-sm-6 col-xs-6 rightNoPadding">
                                    <asp:DropDownList ID="ddlSubBudgetCategory" runat="server" Width="167px" Visible="false" CssClass="aspxDropDownList"></asp:DropDownList>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr11" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Location
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lbLocation" runat="server"></asp:Label>
                        <dx:ASPxGridLookup TextFormatString="{3}" Width="100%" CssClass="gridlookup-view rmmaspGrid-lookup aspxGridLookUp-dropDown" ClientEnabled="true" Visible="false" SelectionMode="Single"
                            ID="glLocation" runat="server" KeyFieldName="ID" GridViewStyles-FilterRow-CssClass="lookupDropDown-filterWrap" GridViewStyles-Row-CssClass="lookupDropDown-contentRow">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="Country" Visible="false">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="State" Visible="false">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Region" Visible="false">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="Title" Caption="Choose Location:" Width="90%">
                                </dx:GridViewDataTextColumn>
                            </Columns>

                            <GridViewProperties>
                                <%--<Settings ShowFilterRow="True" GroupFormat="{0}" VerticalScrollBarMode="Auto" />--%>
                                <SettingsBehavior AllowSort="true" AllowClientEventsOnLoad="true" />
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" VerticalScrollBarMode="Visible" VerticalScrollableHeight="150" />
                            </GridViewProperties>
                            <ClientSideEvents EndCallback="function(s,e){ ResetGridTr();}" />
                        </dx:ASPxGridLookup>
                    </div>
                </div>
                <div id="tr18" class="row col-md-12 col-sm-12 col-xs-12" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Desk Location
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblDeskLocation" runat="server"></asp:Label>
                        <asp:TextBox ID="txtDeskLocation" CssClass="txtbox-width" runat="server" Visible="false"></asp:TextBox>
                    </div>
                </div>

                <div class="row col-md-6 col-sm-6 col-xs-6 rightNoPadding" id="tr19" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Start Date
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                        <dx:ASPxDateEdit ID="dtcStartDate" runat="server" CssClass="datetimectr datetimectr111 CRMDueDate_inputField" EditFormat="Custom"
                            EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" DropDownButton-Image-Url="/Content/Images/calendarNew.png" DropDownButton-Image-Width="18px">
                        </dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="row col-md-6 col-sm-6 col-xs-6" id="tr20" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">End Date
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                        <dx:ASPxDateEdit ID="dtcEndDate" runat="server" CssClass="datetimectr datetimectr111 CRMDueDate_inputField"
                            EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" DropDownButton-Image-Url="/Content/Images/calendarNew.png"
                            DropDownButton-Image-Width="18px">
                        </dx:ASPxDateEdit>
                        <%-- <asp:CompareValidator ID="cmpDates" ControlToCompare="dtcStartDate" ControlToValidate="dtcEndDate" Type="Date" Operator="GreaterThanEqual" ValidationGroup="savechanges" ForeColor="Red"
                            ErrorMessage="Start Date should be less than End Date" runat="server"></asp:CompareValidator>--%>
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr13" runat="server" style="display: none">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Role
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblRole" runat="server"></asp:Label>
                        <ugit:LookUpValueBox ID="ddlRole" FieldName="MultiRoles" runat="server" IsMulti="false" CssClass="lookUpValueBox-dropDown" />
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr16" runat="server">
                    <div class="ms-formlabel" style="display: flex;">
                        <h3 class="ms-standardheader budget_fieldLabel">Skills
                        </h3>
                        <img style="width: 14px; height: 14px; margin-left: 5px;" src="/content/images/plus-blue-new.png" title="New Allocation" onclick="ShowHideSkillPanel()" />
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="lblSkills" runat="server"></asp:Label>
                        <div id="skilldiv" runat="server" class="rmmWrap-aspxTokenBox parentSkill">
                            <dx:ASPxTokenBox runat="server" ID="tbSkills" ClientInstanceName="tbSkills" CssClass="rmm-aspxTokenBox" ItemValueType="System.String" AllowCustomTokens="false"></dx:ASPxTokenBox>
                        </div>
                        <div class="childSkill" style="display: flex; justify-content: space-between;">
                            <div>
                                <dx:ASPxComboBox runat="server" ID="CmbSkillCat" ClientInstanceName="CmbSkillCat" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith"
                                    Width="100%"
                                    EnableSynchronization="False">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnSkillCategoryChanged(s); }" />
                                </dx:ASPxComboBox>
                            </div>
                            <div class="rmmWrap-aspxTokenBox" style="width: 58%">
                                <dx:ASPxTokenBox runat="server" EnableCallbackMode="true" ID="tbSkillValue" ClientInstanceName="tbSkillValue" OnCallback="tbSkillValue_Callback" CssClass="rmm-aspxTokenBox" ItemValueType="System.String" AllowCustomTokens="false"></dx:ASPxTokenBox>
                            </div>
                            <div>
                                <div id="btnAddSkill"></div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="Div2" runat="server">
                    <div class="ms-formlabel" style="display: flex;">
                        <h3 class="ms-standardheader budget_fieldLabel">Certificates
                        </h3>
                        <img style="width: 14px; height: 14px; margin-left: 5px;" src="/content/images/plus-blue-new.png" title="New Allocation" onclick="ShowHideCertificatePanel()" />
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <div id="certificateDiv" runat="server" class="rmmWrap-aspxTokenBox parentCerti">
                            <dx:ASPxTokenBox runat="server" ID="tbCertificate" ClientInstanceName="tbCertificate" CssClass="rmm-aspxTokenBox" ItemValueType="System.String" AllowCustomTokens="false"></dx:ASPxTokenBox>
                        </div>
                        <div class="childCerti" style="display: flex; justify-content: space-between;">
                            <div>
                                <dx:ASPxComboBox runat="server" ID="CmbCertificateCat" ClientInstanceName="CmbCertificateCat" DropDownStyle="DropDownList" IncrementalFilteringMode="StartsWith"
                                    Width="100%"
                                    EnableSynchronization="False">
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCategoryChanged(s); }" />
                                </dx:ASPxComboBox>
                            </div>
                            <div class="rmmWrap-aspxTokenBox" style="width: 58%">
                                <dx:ASPxTokenBox runat="server" EnableCallbackMode="true" ID="tbCertificateValue" ClientInstanceName="tbCertificateValue" OnCallback="tbCertificateValue_Callback" CssClass="rmm-aspxTokenBox" ItemValueType="System.String" AllowCustomTokens="false"></dx:ASPxTokenBox>
                            </div>
                            <div>
                                <div id="btnAddCretificate"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr14" runat="server">
                    <div class="ms-formlabel">
                        <h3 id="h3Enable" class="ms-standardheader budget_fieldLabel" runat="server" visible="false">Enabled
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:CheckBox ID="chkEnable" runat="server" class="chkkeeptrack rmm-chkBox" Visible="false" Checked="true" OnCheckedChanged="chkEnable_CheckedChanged" AutoPostBack="true" />
                        <%--<asp:CheckBox  ID="CheckBox1" runat="server" CClass="chkkeeptrack rmm-chkBox" Checked="true" Visible="false"/>--%>
                        <label class="rmm-Label" id="lblRmmenable" runat="server">&nbsp; Enabled  &nbsp;</label>
                        <asp:Label ID="lblEnable" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr21" runat="server">
                    <div class="ms-formlabel">
                        <h3 id="h3EPE" class="ms-standardheader budget_fieldLabel" runat="server" visible="false">Enable Password Expiration</h3>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lbEnablePwdExpiration" runat="server"></asp:Label>
                        <asp:CheckBox ID="chkEnablePwdExpiration" AutoPostBack="true" OnCheckedChanged="chkEnablePwdExpiration_CheckedChanged" runat="server" Visible="false"></asp:CheckBox>
                        <label class="rmm-Label" id="lblRmmpwdExp" runat="server">&nbsp; Enable Password Expiration  &nbsp;</label>
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="tr23" runat="server">
                    <div class="ms-formlabel">
                        <%--<h3 class="ms-standardheader budget_fieldLabel">Disable Workflow Notifications
                    </h3>--%>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lblDisableWorkflowNotifications" runat="server"></asp:Label>
                        <asp:CheckBox ID="chkDisableWorkflowNotifications" runat="server" Visible="false"></asp:CheckBox>
                        <label class="rmm-Label" id="lblRmmworkFlowDis" runat="server">&nbsp; Disable Workflow Notifications  &nbsp;</label>
                    </div>
                </div>

                <div id="tr22" class="row col-md-12 col-sm-12 col-xs-12" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel budget_fieldLabel">Password Expires on
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <span>
                            <asp:Label ID="lbPwdExpiryDate" runat="server"></asp:Label>
                            <dx:ASPxDateEdit ID="dtcPwdExpiryDate" CssClass="CRMDueDate_inputField dateEdit-dropDown" runat="server" EditFormat="Custom"
                                EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" DropDownButton-Image-Url="/Content/Images/calendarNew.png"
                                DropDownButton-Image-Width="18px">
                            </dx:ASPxDateEdit>
                        </span>
                        <asp:Label ID="lbExpirationPeriod" runat="server"></asp:Label>
                    </div>
                </div>



                <%--new tr for add out of office calender start--%>

                <div id="tr66" runat="server" visible="false" class="enableoutofoffice row col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel budget_fieldLabel">Delegated task for
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Panel ID="delgateUserForPanelRead" runat="server" Visible="true">
                            <table style="width: 100%;">

                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblDelegatedTaskFor" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-md-12 col-sm-12 col-xs-12" id="tr43" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Working Hours
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField col-md-12 col-sm-12 col-xs-12" style="padding-top: 6px;">
                        <asp:Label ID="lblWorwkingHoursStart" runat="server"></asp:Label>
                        <asp:Label ID="lblWorkingHoursEnd" runat="server"></asp:Label>
                        <div class="col-md-5 col-sm-5 col-xs-5 noPadding">
                            <dx:ASPxTimeEdit ID="dtWorkingHoursStart" ClientInstanceName="dtWorkingHoursStart" runat="server" CssClass="setalign rmmWorkHrs-inputField">
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                                <ClientSideEvents />
                            </dx:ASPxTimeEdit>
                        </div>
                        <div class=" col-md-2 col-sm-2 col-xs-2 rmmWorkHrsTOWrap noPadding">
                            <span id="tospan" runat="server" class="rmmWorkHrsTO">To</span>
                        </div>
                        <div class="col-md-5 col-sm-5 col-xs-5 noPadding">
                            <dx:ASPxTimeEdit ID="dtWorkingHoursEnd" ClientInstanceName="dtWorkingHoursEnd" runat="server" CssClass="rmmWorkHrs-inputField">
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                                <ClientSideEvents />
                            </dx:ASPxTimeEdit>
                        </div>
                    </div>
                </div>

                <div id="tr60" runat="server" visible="false" class="enableoutofoffice col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <%-- <h3 class="ms-standardheader">Out Of Office
                    </h3>--%>
                    </div>
                    <div class="ms-formbody rmm-inputField rmm-chkWrap">
                        <asp:Label ID="lblEnableOutOfOffice" runat="server" CssClass="off" Visible="false"></asp:Label>
                        <asp:CheckBox ID="chkOutOfOffice" onclick="EnableOutofOfficehandel();" runat="server" />
                        <label class="rmm-Label" id="lblRmm-outOff">&nbsp; Out Of Office  &nbsp;</label>
                        <asp:Panel ID="outOfOfficePanelEdit" runat="server" Style="display: none;">
                            <div class="accomp-popup">
                                <div class="row col-md-6 col-sm-6 col-xs-6 rightNoPadding" id="tr61">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">From<b style="color: Red">*</b>:&nbsp;</h3>
                                    </div>
                                    <div class="ms-formbody rmm-inputField">
                                        <dx:ASPxDateEdit ID="LeaveFromDate" ClientInstanceName="LeaveFromDate" runat="server" EditFormat="Custom"
                                            EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy" CssClass="CRMDueDate_inputField dateEdit-dropDown"
                                            DropDownButton-Image-Url="/Content/Images/calendarNew.png" DropDownButton-Image-Width="18px">
                                        </dx:ASPxDateEdit>
                                    </div>
                                </div>
                                <div class="row col-md-6 col-sm-6 col-xs-6 rightNoPadding">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">To<b style="color: Red">*</b>:&nbsp;</h3>
                                    </div>
                                    <div class="ms-formbody rmm-inputField">
                                        <dx:ASPxDateEdit ID="LeavetoDate" ClientInstanceName="LeavetoDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy"
                                            NullText="MM/dd/yyyy" CssClass="CRMDueDate_inputField dateEdit-dropDown"
                                            DropDownButton-Image-Url="/Content/Images/calendarNew.png" DropDownButton-Image-Width="18px">
                                        </dx:ASPxDateEdit>
                                    </div>
                                    <div>
                                        <asp:CompareValidator ID="cmpFromTodate" ControlToCompare="LeaveFromDate" ControlToValidate="LeavetoDate" Type="Date" Operator="GreaterThanEqual" ValidationGroup="savechanges" ForeColor="Red"
                                            ErrorMessage="from Date should be less than To Date" runat="server"></asp:CompareValidator>
                                    </div>
                                </div>
                                <div class="row" id="tr660" runat="server" style="display: none">
                                    <div>
                                        <span style="float: left; position: relative; top: 3px;">
                                            <asp:Label ID="errorMsg" runat="server" ForeColor="Red"> </asp:Label>
                                        </span>
                                    </div>
                                </div>

                                <div class="row col-md-6 col-sm-6 col-xs-6 rightNoPadding" id="tr62">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Delegate task to: </h3>
                                    </div>
                                    <div class="ms-formbody rmm-inputField">
                                        <ugit:UserValueBox ID="DelegateUserOnLeave" runat="server" SelectionSet="User" HideCurrentUser="true" CssClass="userValueBox-dropDown userDetails-dropDown" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="outOfOfficePanelRead" runat="server" Visible="false" CssClass="read">
                            <table style="width: 100%;">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblLeaveDate" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblDelegateUserOnLeave" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>

                <div id="trResume" class="row col-md-12 col-sm-12 col-xs-12" runat="server" visible="true">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Upload Resume<b style="color: Red"></b>
                        </h3>
                    </div>
                    <div class="ms-formbody rmm-inputField" style="padding-top: 6px">
                        <asp:FileUpload ID="FileUploadResume" runat="server" accept="application/.pdf" multiple="false" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorResume"
                            runat="server" ControlToValidate="FileUploadResume"
                            ErrorMessage="Only .pdf files are allowed." ForeColor="Red"
                            ValidationExpression="^.*\.(pdf|PDF)$" Display="Dynamic"
                            ValidationGroup="savechanges" SetFocusOnError="true"></asp:RegularExpressionValidator>
                        <asp:HiddenField ID="hdnResume" runat="server" />
                    </div>
                </div>
                <div class="row" id="dvResume" runat="server" visible="false">
                    <div class="ExportOption-btns padding-left:10px" style="padding-left: 20px; margin-top: 5px;">
                        <dx:ASPxButton ID="btnResumePdfExport" ClientInstanceName="btnResumePdfExport" runat="server" CssClass="export-buttons" EnableTheming="false" UseSubmitBehavior="False"
                            RenderMode="Link" ToolTip="Export to Pdf" OnClick="btnResumePdfExport_Click" Text="Resume">
                            <ClientSideEvents Click="function(s, e) { _spFormOnSubmitCalled=false; }" />
                        </dx:ASPxButton>
                    </div>
                    <div>
                        <image src="/content/images/close-red.png" onclick="deleteResume()" style="margin-left: 2px; margin-top: 4px;">
                            <dx:ASPxButton ID="btnDeleteResume" runat="server" OnClick="btnDeleteResume_Click" Style="visibility: hidden; display: none;"></dx:ASPxButton>
                    </div>
                </div>

                <div class="row col-md-12 col-sm-12 col-xs-12" id="Div3" runat="server">
                    <div class="ms-formlabel" style="display: flex;">
                        <h3 class="ms-standardheader budget_fieldLabel">Experience Tag</h3>
                    </div>
                    <div class="ms-formbody rmm-inputField">
                        <asp:Label ID="Label4" runat="server"></asp:Label>
                        <div id="experiencedTagdiv" runat="server" class="rmmWrap-aspxTokenBox parentSkill">
                            <dx:ASPxTokenBox runat="server" ID="tbExperiencedTag" ClientInstanceName="tbExperiencedTag" CssClass="rmm-aspxTokenBox" ItemValueType="System.String" AllowCustomTokens="false"></dx:ASPxTokenBox>
                        </div>
                    </div>
                </div>
            </div>
            <%--new tr for add out of office calender end--%>
            <div class="row fieldWrap cancelInvite">
                <div class="rmmNewUserbtn">
                    <div class="RMMBtnWrap">
                        <dx:ASPxButton ID="btnDelete" runat="server" OnClick="BtnDelete_Click" Visible="true" Text="Delete From Site" ImagePosition="Right"
                            CssClass="btn-danger1">
                            <ClientSideEvents Click="DeleteUser" />
                        </dx:ASPxButton>
                        <%--<div class="secondaryCancelBtn-wrap">--%>
                        <dx:ASPxButton ID="btCancel" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;" OnClick="BtCancel_Click" ImagePosition="Right"
                            CssClass="secondary-cancelBtn">
                            <%-- <ClientSideEvents Click="function(s, e) {btCancel_Click() }" />--%>
                        </dx:ASPxButton>
                        <%--</div>--%>
                        <dx:ASPxButton ID="btnResetPassword" runat="server" Visible="true" Text="Reset Password" ImagePosition="Right" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s, e) {btnResetPassword_Click() }" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btSaveOwnProfile" runat="server" Visible="false" Text="Save" CausesValidation="true" ValidationGroup="savemychanges"
                            OnClick="btSaveOwnProfile_Click" CssClass="primary-blueBtn">
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btSave" ValidationGroup="savechanges" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" OnClick="BtSave_Click"
                            ImagePosition="Right" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){
                                if(Page_ClientValidate('savechanges'))
                                {
                                    if($('.trUserName').is(':visible') == true)
                                    {
                                        validateUserName(s, e);                                        
                                    }
                                    else
                                        saveUserLoading.Show();
                                }
                                }" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnViewHistory" runat="server" Visible="false" AutoPostBack="false" Text="History" ImagePosition="Right" CssClass="primary-blueBtn">
                            <ClientSideEvents Click="function(s,e){ historyPopup.Show(); e.processOnServer = false; }" />
                        </dx:ASPxButton>
                        <dx:ASPxLoadingPanel ID="saveUserLoading" runat="server" ClientInstanceName="saveUserLoading" Modal="True" Image-Url="~/Content/IMAGES/AjaxLoader.gif" ImagePosition="Top" Text="Loading.." CssClass="customeLoader">
                        </dx:ASPxLoadingPanel>
                        <%-- <asp:Button ID="btnInvite" runat="server" Text="Invite" onClick="btnInvite_Click" />--%>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="skillJson" runat="server" />
        <asp:HiddenField ID="hdnEditSkill" runat="server" />
        <asp:Button ID="btnDisableIndividualUser" OnClick="BtSave_Click" runat="server" CssClass="hide"></asp:Button>
    </asp:Panel>

    <%--<asp:Panel ID="pnlMyAssets" runat="server"></asp:Panel>--%>
    <dx:ASPxPopupControl ID="confirmDisablePopup" runat="server" ClientInstanceName="confirmDisablePopup"
        Modal="True" Width="450px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Message" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxPanel ID="pnldisable" ClientInstanceName="pnldisable" runat="server">
                    <PanelCollection>
                        <dx:PanelContent>
                            <div style="width: 100%;">
                                <div style="display: inline-flex; text-align: center;">
                                    <dx:ASPxLabel ID="lblinformativeMsg" runat="server" EncodeHtml="false" ClientInstanceName="lblinformativeMsg"></dx:ASPxLabel>
                                </div>
                                <div class="fright" style="margin: 12px 0px 0px 0px;">


                                    <a href="javascript:Void(0);" onclick="confirmDisablePopup.Hide();"
                                        title="Cancel">
                                        <%--<span class="button-bg">
                                            <b style="float: left; font-weight: normal;">Cancel</b>
                                            <i style="float: left; position: relative; top: -3px; left: 2px">
                                                <img src="/Content/images/cancelwhite.png" style="border: none;" title="" alt="" />
                                            </i>
                                        </span>--%>
                                    </a>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>

    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ID="historyPopup" runat="server" ClientInstanceName="historyPopup"
        Modal="True" Width="550px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="User Profile Change History" AllowDragging="true" AllowResize="true" PopupAnimationType="None" EnableViewState="False">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <dx:ASPxGridView ID="gvHistory" runat="server" AutoGenerateColumns="false" Width="100%" SettingsBehavior-AllowSort="false">
                    <Columns>
                        <dx:GridViewDataTextColumn FieldName="createdBy" Caption="Updated By" Width="20%"></dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="entry" Caption="Description" Width="60%">
                            <PropertiesTextEdit EncodeHtml="false"></PropertiesTextEdit>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="created" Caption="Date" Width="20%"></dx:GridViewDataTextColumn>
                    </Columns>
                    <Settings VerticalScrollBarMode="Auto"></Settings>
                    <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                </dx:ASPxGridView>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>




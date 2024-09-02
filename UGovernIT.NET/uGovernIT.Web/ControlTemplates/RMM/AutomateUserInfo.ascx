<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutomateUserInfo.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.RMM.AutomateUserInfo" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function createEmail(textInput) {
        var sFullname = $('#sfullname').val();
        var sEmail = $('#semail').val();
        var dFullname = $(textInput).val();
        var retValue = '';

        if (sFullname.length > 0 && sEmail.length > 0 && dFullname.length > 0) {

            // Split sname to get first name and last name
            var sNames = sFullname.split(" ");
            if (sNames.length > 1) {

                if (sNames.length == 2) {
                    var sFirstname = sNames[0].toLowerCase();
                    var sLastname = sNames[1].toLowerCase();
                }
                else if (sNames.length == 3) {
                    var sFirstname = sNames[0].toLowerCase();
                    var sLastname = sNames[2].toLowerCase();
                }
                sEmail = sEmail.toLowerCase();
                var emailDomain = sEmail.substr(sEmail.indexOf("@"));

                //checkmailContainsDot=sEmail.split("@")
                //var dotindex=(checkmailContainsDot.includes(".")) ? -1 : 1
                //var dotindex= checkmailContainsDot.indexOf(".");
                //// specail char 
                //var index=sEmail.indexOf(".")
                //if (index != -1) {
                //   var array = sEmail.split(index);

                //}
                var dNames = dFullname.split(" ");
                if (dNames.length == 2) {
                    var dFirstname = dNames[0].toLowerCase();
                    var dLastname = dNames[1].toLowerCase();
                }
                if (dNames.length == 3) {
                    var dFirstname = dNames[0].toLowerCase();
                    var dLastname = dNames[2].toLowerCase();
                }

                //if (dNames.length == 1) {

                // var dFirstname = dNames.toLowerCase();
                //}
                // var dFirstname = dNames[0].toLowerCase();
                //var dLastname = dNames[1].toLowerCase();
                // var dLastname = dNames.Last().toLowerCase();
                //var dLastname =dNames.substring(dNames.lastIndexOf(" "));

                var dotindex = 10;
                if (dotindex == -1) {
                    //first name + last name combination
                    if (sEmail.indexOf(sFirstname + "." + sLastname) === 0) {
                        retValue = dFirstname + "." + dLastname + emailDomain;
                    }

                    //last name + first name combination
                    else if (sEmail.indexOf(sLastname + "." + sFirstname) === 0) {
                        retValue = dLastname + "." + dFirstname + emailDomain;

                    }

                    //first name + last name initial combination
                    else if (sEmail.startsWith(sFirstname + ".") === true) {
                        retValue = dFirstname + "." + dLastname.slice(0, 1) + emailDomain;
                    }

                    //last name + first name initial combination
                    else if (sEmail.startsWith(sLastname + ".") === true) {
                        retValue = dLastname + "." + dFirstname.slice(0, 1) + emailDomain;
                    }

                    //first name initial + last name combination
                    else if (sEmail.endsWith(+"." + sLastname + emailDomain) === true) {
                        retValue = dFirstname.slice(0, 1) + "." + dLastname + emailDomain;
                    }

                    //last name initial + first name combination
                    else if (sEmail.endsWith(+"." + sFirstname + emailDomain) === true) {
                        retValue = dLastname.slice(0, 1) + "." + dFirstname + emailDomain;
                    }


                }

                else if (dNames.length == 1) {

                    sEmail = sEmail.toLowerCase();
                    var emailDomain = sEmail.substr(sEmail.indexOf("@"));

                   // var dNames = dFullname.split(" ");

                    //first name + EmailDomain  combination
                    if (sEmail == (sFirstname + emailDomain)) {
                        retValue = dNames + emailDomain;
                    }
                     if (sEmail == (sLastname + emailDomain)) {
                        retValue = dNames + emailDomain;
                    }
                    retValue = dNames + emailDomain;

                //if (retValue.length > 0) {
                //    $(textInput).parent().next().find('input').val(retValue);
                //}



                }
                // cases without specail char{
                else {

                    //first name + last name combination

                    if (sEmail.indexOf(sFirstname + sLastname) === 0 && !(sEmail == (sFirstname + emailDomain))) {
                        retValue = dFirstname + dLastname + emailDomain;
                    }

                    //last name + first name combination
                    else if (sEmail.indexOf(sLastname + sFirstname) === 0) {
                        retValue = dLastname + dFirstname + emailDomain;

                    }

                    //else if (sEmail==(sFirstname+emailDomain)) {
                    //    retValue = dFirstname + emailDomain;
                    //}

                    //first name + last name initial combination
                    else if (sEmail.startsWith(sFirstname) === true && !(sEmail == (sFirstname + emailDomain))) {
                        retValue = dFirstname + dLastname.slice(0, 1) + emailDomain;
                    }

                    //first name + EmailDomain  combination
                    else if (sEmail == (sFirstname + emailDomain)) {
                        retValue = dFirstname + emailDomain;
                    }

                    //last name + first name initial combination
                    else if (sEmail.startsWith(sLastname) === true && !(sEmail == (sLastname + emailDomain))) {
                        retValue = dLastname + dFirstname.slice(0, 1) + emailDomain;
                    }

                    //last name + EmailDomain  combination
                    else if (sEmail == (sLastname + emailDomain)) {
                        retValue = dLastname + emailDomain;
                    }

                    //first name initial + last name combination
                    else if (sEmail.endsWith(sLastname + emailDomain) === true) {
                        retValue = dFirstname.slice(0, 1) + dLastname + emailDomain;
                    }

                    //last name initial + first name combination
                    else if (sEmail.endsWith(sFirstname + emailDomain) === true) {
                        retValue = dLastname.slice(0, 1) + dFirstname + emailDomain;
                    }
                }

                //if (retValue.length > 0) {
                //    $(textInput).parent().next().find('input').val(retValue);
                //}
            }
          
            // entered name is only one word 
            else if (sNames.length == 1) {
            sEmail = sEmail.toLowerCase();
            var emailDomain = sEmail.substr(sEmail.indexOf("@"));

                var dNames = dFullname.split(" ");

                //first name + EmailDomain  combination
                if (sEmail == (sNames + emailDomain)) {
                    retValue = dNames[0] + emailDomain;
                }


        }

            if (retValue.length > 0) {
                $(textInput).parent().next().find('input').val(retValue);
            }

        }


    }

    function UserValidation() {
        var encodedMessage = DevExpress.utils.string.encodeHtml("User is not created");
        var myDialog = DevExpress.ui.dialog.custom({
            title: "Limitation for Users",
            messageHtml: encodedMessage + "<div class='bulkuser-SucsMsg'></div>",
            toolbarItems: [
                { text: "Details", onClick: function () { window.top.location.href = "/purchase.aspx" } },
                { text: "Cancel", onClick: function () { window.parent.CloseWindowCallback(1, document.location.href); } }
            ]

        });
        myDialog.show().done(function (dialogResult) {
            /*console.log(dialogResult.buttonText);*/
        });
    }

    function SaveUserInfo() {

        var isUserLimitExceed = '<%=IsUserLimitExceed%>';

        if (isUserLimitExceed.toLocaleLowerCase() == 'true') {

            UserValidation();
        }
        else {
            var userData = [];
            var userInfos = $(".userInfoRow");

            $.each(userInfos, function (i, item) {

                var _userName = $(item).find("input[name^=fullname]").val();
                var _email = $(item).find("input[name^=email]").val();
                var _role = $(item).find("input[name^=role]").val();

                if (_userName.length > 0 && _email.length > 0) {
                    userData.push({ username: _userName, email: _email, role: _role });
                }
            });

            saveUserData(userData);
            return false;
        }
    }

    function saveUserData(userData) {

        // $(".okayButton").prop("disabled",true);

        var userInfoRequestModel = "{ 'UserData' : " + JSON.stringify(userData) + " }";

        $.ajax({
            type: "POST",
            url: "<%= ajaxPageURL %>CreateUsers",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: userInfoRequestModel,
            success: function (data) {

                if (data.IsUsersCreated == true) {

                    if (data.IsUserLimitExceed) {
                        if (data.userName != null) {
                            var usersArray = data.userName.split(',');
                            $("#dialogForUservalidation").html("<div class='excced-SucsMsg'> Below list of the users are not added in the system. Please click on 'Details' button for more details. <br/></div>");
                            $.each(usersArray, function (index, value) {
                                $(".excced-SucsMsg").append(value + "<br/>");
                                // $("#dialog").html("<div class='bulkuser-SucsMsg'> Users Created Sucessfully.  You have exceeded number of users in the Trial plan And List of the user(s) are not added" + value + "<br/></div>");
                            });
                            // $(".bulkuser-SucsMsg").append("Press 'Okay' for purchase");

                            //$(".okayButton").prop("disabled", false);


                        }
                        $("#dialogForUservalidation").dialog('open');

                    }
                    else {
                        $("#dialog").html("<div class='bulkuser-SucsMsg'> Users created sucessfully. An invite email was sent to a new user.</div>");
                        $("#dialog").dialog('open');
                    }
                }
                else {
                    var userList = data.userName;
                    var list = userList.split(",")
                    var html = new Array();
                    if (data.IsUserLimitExceed) {
                        html.push("<div class='exceed-SucsMsg'>  Users are not added !!<br/></div>");

                        $("#dialogForUservalidation").html(html);

                        $("#dialogForUservalidation").dialog('open');

                    }
                    else {
                        html.push("<div class='bulkuser-crtMsg'>Below users are not created in the system. Please check details and try again</div>");

                        for (var i = 0; i < list.length; i++) {
                            var user = list[i];
                            html.push("<div class='bulkuser-existUser'>" + user + "</div>");
                        }
                        $("#dialog").html(html);
                        $("#dialog").dialog('open');
                    }
                }

                $('#sfullname').val("");
                $('#semail').val("");
                $('#srole').val("");

                $('.userName').val("");
                $('.email').val("");
                $('.role').val("");
            }
        });
    }


    function addUserRow() {
        var markup = "<tr class='userInfoRow'><td class='bulkUser-addtd'><input type='text' class='userName' name='fullname' placeholder='Full Name' onfocusout=\'createEmail(this);\'></td><td class='bulkUser-addtd'><input type='text' class='email' name='email' placeholder='Email Address'></td><td class='bulkUser-addtd'><input class='role'name='role' placeholder='Role' ></td></tr>";
        $("table tbody").append(markup);
    }

    $(document).ready(function () {
        for (i = 1; i < 4; i++) {
            addUserRow();
        }

        $('#insertRow').click(function (e) {
            addUserRow();
            e.preventDefault();

        });
    });

    //Okay: function () {
    //                window.top.location.href = "/purchase.aspx";
    //            },
    //            Close: function () {
    //                $(this).dialog('close');
    //            },

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
                    var sourceURL = "<%= Request["source"] %>";
                    window.parent.CloseWindowCallback(0, sourceURL);

                    // window.top.location.href = "/purchase.aspx";
                }
            }]
        });
        $('#dialogForUservalidation').dialog({
            autoOpen: false,
            width: 550,
            hieght: 200,
            modal: true,
            title: "User Creation Details",
            buttons: [{
                text: "Details",
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

    $(document).ready(function () {
       
        $.ajax({
            url: "<%= ajaxPageURL %>GetRoles",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

                $(".role").autocomplete({
                    source: data
                });
            }
        });
    });
     


</script>
<div id="dialog"></div>
<div id="dialogForUservalidation"></div>
<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row">
        <table class="bulkUser-addtable">
            <tbody>
                <tr class="userInfoRow">
                    <td class="bulkUser-addtd">
                        <input type="text" id="sfullname" name="fullname" placeholder="Full Name"></td>
                    <td class="bulkUser-addtd">
                        <input type="text" id="semail" name="email" placeholder="Email Address"></td>
                    <td class="bulkUser-addtd">
<%--                        <input type="text" id="srole" name="role" placeholder="Role"></td>--%>
                      <input  class="role" name="role" placeholder="Role"> </td>

                </tr>


                <tr class="userInfoRow">
                    <td class="bulkUser-addtd">
                        <input type="text" class="userName" name="fullname" placeholder="Full Name" onfocusout="createEmail(this);"></td>
                    <td class="bulkUser-addtd">
                        <input type="text" class="email" name="email" placeholder="Email Address"></td>
                    <td class="bulkUser-addtd">
<%--                        <input type="text" id="tags" class="role" name="role" placeholder="Role"></td>--%>
                      <input  class="role" name="role" placeholder="Role"> </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="row bulkUser-btnWrap">
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
<%--        <button id="insertRow" class="insertBtn-bulkUSer">New Row</button>--%>
        <dx:ASPxButton ID="save_btn" runat="server" AutoPostBack="false" Text="Invite" CssClass="primary-blueBtn">
            <ClientSideEvents Click="SaveUserInfo" />
        </dx:ASPxButton>
    </div>
</div>

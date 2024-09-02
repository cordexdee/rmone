<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfoControl.ascx.cs" Inherits="uGovernIT.Web.UserInfoControl" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

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

        .ms-formbody {
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

        .ms-standardheader {
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

        .cssassettimeline {
            float: right;
            display: inline-flex;
            /*padding-left: 10px;*/
        }

        .hide {
            display: none;
        }
    </style>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

        function onChange(s, e) {
            s.ClearTokenCollection();
            s.AddItem(s.GetSelectedIndex());

        }

        function DeleteUser(obj) {
            if (confirm("Are you sure you want to delete this user from the site?")) {

                return true;
            }
            else {
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
            window.parent.UgitOpenPopupDialog("<%= absoluteURL("/Layout/ugovernit/DelegateControl.aspx")%>?control=changepassword&resetUserPwd=1&userCode=<%=userID%>", "", 'Reset Password', "600px", "300px");
        }

        function OnSaveClientCick() {

            if ($('#<%=chkOutOfOffice.ClientID%>').is(':checked'))
            { return enableOutofOfficeErrorhandel(); }


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

        function ShowTimeLineTickets() {

           <% if (userInfo != null)
        {%>
            var user ='<%=userInfo.Name.Replace("'",string.Empty)%>';
            if (user != "" && user != undefined && user != null) {
                var url ='<%=openCloseTicketsForRequestorUrl%>&Id=<%=userInfo.Id%>';
                window.parent.UgitOpenPopupDialog(url, "", user + "'s Tickets", 90, 90);
            }
            <%}%>
        }



        function ShowAssignedAssets() {

            <%if (userInfo != null)
        {%>
            var userid ='<%=userInfo.Id%>';
            if (userid != "" && userid != undefined && userid != null) {
                var url ='<%=assetUrl%>';
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
    </script>

    <dx:ASPxHiddenField ID="hdnkeepAction" runat="server" ClientInstanceName="hdnkeepAction"></dx:ASPxHiddenField>
    <asp:Panel ID="viewPanel" CssClass="viewpanel" runat="server">
        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
            width="100%">
            <tr id="messageTR" runat="server" visible="false">
                <td colspan="2" style="text-align: center; font-size: small; color: red;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="tr0" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Account <b style="color: Red">*</b>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblUserName" runat="server"></asp:Label>

                    <div id="divassetsTickets" runat="server" class="cssassettimeline">
                        <div class="button-bg">
                            <dx:ASPxButton ID="lnkassets" runat="server" Visible="false" Text="Assets">
                                <ClientSideEvents Click="function(s, e) {ShowAssignedAssets()}" />
                            </dx:ASPxButton>

                        </div>
                        <div class="button-bg" style="padding-left: 10px;">
                            <dx:ASPxButton ID="lnktimeline" runat="server" Visible="false" Text="Tickets">
                                <ClientSideEvents Click="function(s, e) {ShowTimeLineTickets()}" />
                            </dx:ASPxButton>

                        </div>
                    </div>

                </td>
            </tr>
            <tr id="trTypeofUser" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Type of User<b style="color: Red">*</b>
                    </h3>

                </td>
                <td class="ms-formbody">
                    <dx:ASPxComboBox runat="server" ID="ddlUserType">
                        <Items>
                            <dx:ListEditItem Text="AD User" Value="0" />
                            <dx:ListEditItem Text="FBA User" Value="1" />
                        </Items>
                    </dx:ASPxComboBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlUserType" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please select user type">
                    </asp:RequiredFieldValidator>

                </td>
            </tr>
            <tr id="trLoginName" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Enter Login Name<b style="color: Red">*</b>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:TextBox ID="txtLoginName" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLoginName" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter valid Login name">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr id="trPasword" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Password<b style="color: Red">*</b>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="Label2" runat="server"></asp:Label>
                    <asp:TextBox ID="txtPassword" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="error" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter password">
                    </asp:RequiredFieldValidator>
                    <input type="button" id="btnRandomPassword" tabindex="-1" onclick="generateRandomPassword()" name="GeneratePassword" value="Generate" />
                </td>
            </tr>
            <tr id="trReenterPassword" runat="server" visible="false">
                <td class="ms-formlabel" style="vertical-align: top;">
                    <h3 class="ms-standardheader">Re-enter password<b style="color: Red">*</b>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="Label3" runat="server"></asp:Label>
                    <asp:TextBox ID="txtReenterPassword" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="error" runat="server" ControlToValidate="txtReenterPassword" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please reenter password">
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" CssClass="error" runat="server" ErrorMessage="Re-entered password must be the same" ControlToCompare="txtPassword" ControlToValidate="txtReenterPassword" Display="Dynamic" SetFocusOnError="True"></asp:CompareValidator>
                    <br />
                    <%-- <label for="chkbxShowPassword"><input type="checkbox" id="chkbxShowPassword" /> <span>Show Password</span></label>--%>
                    <asp:CheckBox ID="chkbxShowPassword" runat="server" Text="Show Password" CssClass="chkbxShowPassword" />
                </td>
            </tr>

            <tr id="tr1" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Name<b style="color: Red">*</b>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbName" runat="server"></asp:Label>
                    <asp:TextBox ID="txtName" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="txtNameRequired" CssClass="error" runat="server" ControlToValidate="txtName" ValidationGroup="savechanges" Display="Dynamic" ErrorMessage="Please enter name">
                    </asp:RequiredFieldValidator>


                    <%--<dx:ASPxButton ID="btnUpload" runat="server" Text="Upload Profile Picture" OnClick="btnUpload_Click"></dx:ASPxButton>--%>
                    
                </td>
            </tr>
            <tr id="tr51" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Upload Profile Pic<b style="color: Red"></b>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:FileUpload ID="FileUploadUserPics" runat="server" /></td>
            </tr>
            <tr id="tr2" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">E-Mail
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbEmail" runat="server"></asp:Label>
                    <asp:TextBox ID="txtEmail" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr id="tr24" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Notification E-Mail
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbNotificationEmail" runat="server"></asp:Label>
                    <asp:TextBox ID="txtNotificationEmail" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr id="tr3" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Phone Number
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbMobileNumber" runat="server"></asp:Label>
                    <asp:TextBox ID="txtMobileNumber" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>

            <tr id="tr4" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader"><%=departmentLabel %>
                    </h3>
                </td>
                <td class="ms-formbody">
                    <%--                  <ugit:Department id="ctDepartment" runat="server" EnablePostBack="false" ControlMode="Edit"></ugit:Department>--%>
                    <ugit:LookupValueBoxEdit ID="departmentCtr" runat="server" FieldName="DepartmentLookup"></ugit:LookupValueBoxEdit>
                </td>
            </tr>
            <tr id="tr15" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Functional Area
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblFunctionalArea" runat="server"></asp:Label>
                    <ugit:LookUpValueBox ID="ddlFunctionalArea" FieldName="FunctionalAreaLookup" runat="server" />
                </td>
            </tr>

            <tr id="tr5" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Job Title
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbJobTitle" runat="server"></asp:Label>
                    <asp:TextBox ID="txtJobTitle" CssClass="txtbox-width" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>


            <tr id="tr6" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Hourly Rate($)
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbHourlyRate" runat="server"></asp:Label>
                    <asp:TextBox ID="txtHourlyRate" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server" Visible="false"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ForeColor="Red" runat="server" ControlToValidate="txtHourlyRate" ErrorMessage="Only Numbers Allowed" Display="Dynamic" ValidationGroup="savechanges" ValidationExpression="^(^(\$)?\d+(\.\d+)?$|^(-)?\d+(\.\d+)?$)$">

                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="tr25" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Approve Level Amount($)
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbApproveLevelAmount" runat="server"></asp:Label>
                    <asp:TextBox ID="txtApproveLevelAmount" CssClass="txtbox-width" ValidationGroup="savechanges" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revApprovalLevelAmount" runat="server" ForeColor="Red" ControlToValidate="txtApproveLevelAmount" EnableClientScript="true" ErrorMessage="Only Numbers Allowed" Display="Dynamic" ValidationGroup="savechanges" ValidationExpression="^(^(\$)?\d+(\.\d+)?$|^(-)?\d+(\.\d+)?$)$">

                    </asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr id="tr7" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Manager
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbManager" runat="server"></asp:Label>
                    <ugit:UserValueBox ID="managerUser" SelectionSet="User" runat="server" />

                </td>
            </tr>

            <tr id="tr8" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">IT
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbIT" runat="server"></asp:Label>
                    <asp:CheckBox ID="cbIT" runat="server" Visible="false" />
                </td>
            </tr>

            <tr id="tr9" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Consultant
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbIsConsultant" runat="server"></asp:Label>
                    <asp:CheckBox ID="cbIsConsultant" runat="server" Visible="false" />
                </td>
            </tr>

            <tr id="tr10" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Manager
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbIsManager" runat="server"></asp:Label>
                    <asp:CheckBox ID="cbIsManager" runat="server" Visible="false" />
                </td>
            </tr>
            <tr id="tr12" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Budget Category
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbBudgetCategory" runat="server"></asp:Label>
                    <asp:UpdatePanel ID="budgetPanel" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlBudgetCategory" AutoPostBack="true" runat="server" Width="167px" Visible="false" OnSelectedIndexChanged="DDLBudgetCategory_SelectedIndexChanged"></asp:DropDownList>
                            <asp:DropDownList ID="ddlSubBudgetCategory" runat="server" Width="167px" Visible="false"></asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="tr11" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Location
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbLocation" runat="server"></asp:Label>
                    <dx:ASPxGridLookup TextFormatString="{3}" CssClass="gridlookup-view" ClientEnabled="true" Visible="false" SelectionMode="Single" ID="glLocation" runat="server" KeyFieldName="ID" Width="167px">
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
                            <Settings ShowFilterRow="True" ShowStatusBar="Visible" />
                        </GridViewProperties>
                        <ClientSideEvents EndCallback="function(s,e){ ResetGridTr();}" />
                    </dx:ASPxGridLookup>
                </td>
            </tr>
            <tr id="tr18" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Desk Location
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblDeskLocation" runat="server"></asp:Label>
                    <asp:TextBox ID="txtDeskLocation" CssClass="txtbox-width" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>

            <tr id="tr19" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Start Date
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                    <dx:ASPxDateEdit ID="dtcStartDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy"></dx:ASPxDateEdit>
                </td>
            </tr>
            <tr id="tr20" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">End Date
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                    <dx:ASPxDateEdit ID="dtcEndDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy"></dx:ASPxDateEdit>
                </td>
            </tr>

            <tr id="tr13" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Role
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblRole" runat="server"></asp:Label>
                    <ugit:LookUpValueBox ID="ddlRole" FieldName="MultiRoles" runat="server" CssClass="lookUpValueBox-dropDown" />
                </td>
            </tr>

            <tr id="tr16" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Skills
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblSkills" runat="server"></asp:Label>
                    <div id="skilldiv" runat="server">
                        <input type="text" id="demo-input" name="demoinput" />
                        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">


                            var skilluserData = [];
                          <%--  $(document).ready(function () {
                                if (skilluserData.length > 0)
                                    return;
                                var query = "<OrderBy><FieldRef Name='Title' Ascending='TRUE'/></OrderBy>";
                                wsBaseUrl = L_Menu_BaseUrl + '/_vti_bin/';
                                var res = queryItems('UserSkills', query, ['ID', 'Title'], undefined, undefined, function (e) {
                                    uInfoList = e.items;
                                    $.each(e.items, function (idx, item) {
                                        skilluserData.push({ id: item['ID'], name: item['Title'] });
                                    });
                                    $("#demo-input").tokenInput(skilluserData , {
                                        preventDuplicates: true,
                                        theme: "facebook",
                                        prePopulate: <%= jsonSkillData %>
                                        });
                                });

                            });--%>
</script>
                    </div>

                </td>
            </tr>


            <tr id="tr14" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Enabled
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblEnable" runat="server"></asp:Label>
                    <asp:CheckBox ID="chkEnable" runat="server" CssClass="chkkeeptrack" Checked="true" Visible="false" />
                </td>
            </tr>
            <tr id="tr21" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Enable Password Expiration
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lbEnablePwdExpiration" runat="server"></asp:Label>
                    <asp:CheckBox ID="chkEnablePwdExpiration" AutoPostBack="true" OnCheckedChanged="chkEnablePwdExpiration_CheckedChanged" runat="server" Visible="false"></asp:CheckBox>
                </td>
            </tr>

            <tr id="tr23" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Disable Workflow Notifications
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblDisableWorkflowNotifications" runat="server"></asp:Label>

                    <asp:CheckBox ID="chkDisableWorkflowNotifications" runat="server" Visible="false"></asp:CheckBox>

                </td>
            </tr>

            <tr id="tr22" runat="server" visible="false">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Password Expires on
                    </h3>
                </td>
                <td class="ms-formbody">
                    <span>
                        <asp:Label ID="lbPwdExpiryDate" runat="server"></asp:Label>
                        <dx:ASPxDateEdit ID="dtcPwdExpiryDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy"></dx:ASPxDateEdit>
                    </span>
                    <asp:Label ID="lbExpirationPeriod" runat="server"></asp:Label>
                </td>
            </tr>



            <%--new tr for add out of office calender start--%>

            <tr id="tr66" runat="server" visible="false" class="enableoutofoffice">
                <td class="ms-formlabel" valign="top">
                    <h3 class="ms-standardheader">Delegated task for
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Panel ID="delgateUserForPanelRead" runat="server" Visible="true">
                        <table style="width: 100%;">

                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblDelegatedTaskFor" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr id="tr43" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">Working Hours
                    </h3>
                </td>
                <td class="ms-formbody" style="padding-top: 6px;">
                    <asp:Label ID="lblWorwkingHoursStart" runat="server"></asp:Label>
                    <asp:Label ID="lblWorkingHoursEnd" runat="server"></asp:Label>
                    <dx:ASPxTimeEdit ID="dtWorkingHoursStart" ClientInstanceName="dtWorkingHoursStart" runat="server" CssClass="setalign" Width="80">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                        <ClientSideEvents />
                    </dx:ASPxTimeEdit>
                    <span id="tospan" runat="server" style="margin-left: 3px; margin-right: 3px; float: left; padding-top: 3px;">To</span>
                    <dx:ASPxTimeEdit ID="dtWorkingHoursEnd" ClientInstanceName="dtWorkingHoursEnd" runat="server" Width="80">
                        <ClearButton DisplayMode="OnHover"></ClearButton>
                        <ValidationSettings ErrorDisplayMode="ImageWithTooltip" Display="Dynamic" />
                        <ClientSideEvents />
                    </dx:ASPxTimeEdit>
                </td>
            </tr>
            <tr id="tr60" runat="server" visible="false" class="enableoutofoffice">
                <td class="ms-formlabel" valign="top">
                    <h3 class="ms-standardheader">Out Of Office
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblEnableOutOfOffice" runat="server" CssClass="off" Visible="false"></asp:Label>
                    <asp:CheckBox ID="chkOutOfOffice" onclick="EnableOutofOfficehandel();" runat="server" />
                    <asp:Panel ID="outOfOfficePanelEdit" runat="server" Style="display: none;">
                        <table>
                            <tr id="tr61">
                                <td>
                                    <span style="float: left; position: relative; top: 5px;">From<b style="color: Red">*</b>:&nbsp;</span>
                                    <span style="float: left; margin-top: 5px;">
                                        <dx:ASPxDateEdit ID="LeaveFromDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy"></dx:ASPxDateEdit>

                                    </span>
                                </td>
                                <td style="align-content: flex-end">
                                    <span style="float: left; position: relative; top: 5px;">To<b style="color: Red">*</b>:&nbsp;</span>
                                    <span style="float: left; margin-top: 5px;">
                                        <dx:ASPxDateEdit ID="LeavetoDate" runat="server" EditFormat="Custom" EditFormatString="MM/dd/yyyy" NullText="MM/dd/yyyy"></dx:ASPxDateEdit>

                                    </span>
                                </td>
                            </tr>
                            <tr id="tr660" runat="server" style="display: none">
                                <td colspan="2">
                                    <span style="float: left; position: relative; top: 3px;">
                                        <asp:Label ID="errorMsg" runat="server" ForeColor="Red"> </asp:Label>
                                    </span>
                                </td>
                            </tr>

                            <tr id="tr62">
                                <td colspan="2">
                                    <span style="float: left; position: relative; top: 5px;">Delegate task to: </span>
                                    <br />
                                    <span style="float: left; margin-top: 5px;">
                                        <ugit:UserValueBox ID="DelegateUserOnLeave" runat="server" />
                                        <%-- <SharePoint:PeopleEditor PrincipalSource="UserInfoList" ID="DelegateUserOnLeave" MaximumHeight="18" Width="216px" CssClass="peAssignedTo"
                                            SelectionSet="User" runat="server" MultiSelect="false" PlaceButtonsUnderEntityEditor="false" AugmentEntitiesFromUserInfo="true" />--%>
                                    </span>
                                </td>
                            </tr>
                        </table>
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
                </td>
            </tr>

            <tr id="tr17" runat="server">
                <td class="ms-formlabel">
                    <h3 class="ms-standardheader">User Groups
                    </h3>
                </td>
                <td class="ms-formbody">
                    <asp:Label ID="lblUserGroups" runat="server"></asp:Label>
                </td>
            </tr>
            <%--new tr for add out of office calender end--%>

            <tr>
                <td align="left" style="padding-top: 10px" colspan="2">
                    <div style="float: left; padding-top: 5px; padding-left: 5px;">
                        <dx:ASPxButton ID="btnDelete" runat="server" OnClick="BtnDelete_Click" Visible="false" Text="Delete From Site" ImagePosition="Right">
                            <Image Url="/Content/Images/delete-icon.png"></Image>
                            <%-- <ClientSideEvents Click="function(s, e) {return DeleteUser(this) }" />--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btnResetPassword" runat="server" Visible="false" Text="Reset Password" ImagePosition="Right">
                            <Image Url="/Content/images/uGovernIT/ButtonImages/remove-user.png"></Image>
                            <%--<ClientSideEvents Click="function(s, e) {btnResetPassword_Click() }" />--%>
                        </dx:ASPxButton>


                    </div>
                    <%-- </td>--%>
                    <%--  <td style="padding-top: 10px">--%>
                    <div style="float: right;">
                        <dx:ASPxButton ID="btSaveOwnProfile" runat="server" Visible="false" Text="Save" CausesValidation="true" ValidationGroup="savemychanges" OnClick="btSaveOwnProfile_Click" ImagePosition="Right">
                            <Image Url="/Content/Images/save-icon.png"></Image>
                            <ClientSideEvents Click="function(s, e) {return OnSaveClientCick();}" />
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btSave" ValidationGroup="savechanges" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" OnClick="BtSave_Click" ImagePosition="Right">
                            <Image Url="/Content/Images/save-icon.png"></Image>
                            <%--  <ClientSideEvents Click="function(s, e) {return OnSaveClientCick();}" />--%>
                        </dx:ASPxButton>
                        <dx:ASPxButton ID="btCancel" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;" OnClick="BtCancel_Click" ImagePosition="Right">
                            <Image Url="/Content/Images/cancel-icon.png"></Image>

                        </dx:ASPxButton>



                    </div>

                </td>
            </tr>
            <%-- <tr>
                                    <td colspan="2"></td>
                                </tr>--%>
        </table>

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
                                        <span class="button-bg">
                                            <b style="float: left; font-weight: normal;">Cancel</b>
                                            <i style="float: left; position: relative; top: -3px; left: 2px">
                                                <img src="/Content/images/cancelwhite.png" style="border: none;" title="" alt="" />
                                            </i>
                                        </span>
                                    </a>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>

    </dx:ASPxPopupControl>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceQuestionEditor.ascx.cs" Inherits="uGovernIT.Web.ServiceQuestionEditor" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Src="~/ControlTemplates/uGovernIT/Services/RequestTypeDropDownList.ascx" TagPrefix="ugit" TagName="RequestTypeDropDownList" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function ddlTargetType_SelectedIndexChanged(ddl) {
        var e = document.getElementById("ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_ddlTargetTypes");
        var strval = e.options[e.selectedIndex].value;

        switch (strval) {
            case "File":
                document.getElementById("trFileUploads").style.display = "block"
                document.getElementById("trLinks").style.display = 'none';
                document.getElementById("trWikis").style.display = 'none';
                document.getElementById("trHelpCards").style.display = "none";
                break;
            case "Link":
                document.getElementById("trLinks").style.display = "block";
                document.getElementById("trFileUploads").style.display = 'none';
                document.getElementById("trWikis").style.display = 'none';
                document.getElementById("trHelpCards").style.display = "none";
                break;
            case "Wiki":
                document.getElementById("trLink").style.display = "none";
                document.getElementById("trFileUpload").style.display = 'none';
                document.getElementById("trWiki").style.display = 'block';
                document.getElementById("trHelpCards").style.display = "none";
                break;
            case "HelpCard":
                document.getElementById("trLinks").style.display = "none";
                document.getElementById("trFileUploads").style.display = 'none';
                document.getElementById("trWikis").style.display = 'none';
                document.getElementById("trHelpCards").style.display = "block";
                break;
            default:
                break;

        }
    }
    function closePopup(obj) {
        if (window.frameElement) {
            // in frame
            var sourceURL = "";
            if ($(obj).find("b").text().toLowerCase() == 'close') {
                sourceURL = "<%= Request["source"] %>";
            }
            window.parent.CloseWindowCallback(1, sourceURL);
        }
    }
</script>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .tbDateProperties {
        margin-top: 10px;
    }
    /*.tdDateProperties 
    {
        padding-bottom:5px;
    }*/
    .fleft {
        float: left;
    }

    /*.ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }*/

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    /*.full-width {
        width: 98%;
    }*/

    .ms-formlabel {
        width: 160px;
    }

    /*.existing-section-c {
        float: left;
    }*/

    /*.new-section-c {
        float: left;
    }*/

    /*.existing-section-a {
        float: left;
        padding: 0px 5px;
    }*/

    /*.existing-section-a img {
            cursor: pointer;
        }*/

    /*.new-section-a {
        float: left;
        padding-left: 5px;
    }

        .new-section-a img {
            cursor: pointer;
        }*/

    .auto-style2 {
        width: 321px;
    }

    .cssAssetpeople {
        display: inline !important;
        height: 15px;
    }

    div.ms-inputuserfield {
        height: 16px !important;
    }

    .rdbdates {
        position: relative;
        left: 1px;
        top: -1px;
    }

    .setChkbxAttrib {
        float: right;
        display: inline;
    }

    .setlength {
        position: relative;
        float: left;
        top: -4px;
    }

    .pAppAccessReqProperties > tbody > tr > td {
        padding-top: 5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
   <%--  function showUploadControl() {
        $("#<%=txtWiki.ClientID %>").hide();
         $("#<%=fileupload.ClientID %>").show()
    }--%>

    function addNewQuestionSection(obj) {
        $("#newSectionC").show("medium");
        $("#<%= hfEditSectionID.ClientID %>").val("-1");
        $("#<%= txtEditSection.ClientID %>").val("");
    }

    function editQuestionSection(obj) {
        if (selectedQuestion.attr("value") != "0") {
            var selectedQuestion = $("#<%= questionServiceSectionsDD.ClientID%> option:selected");
            if (selectedQuestion != null && selectedQuestion.length > 0) {
                $("#newSectionC").show("medium");
                $("#<%= hfEditSectionID.ClientID %>").val(selectedQuestion.attr("value"));
                $("#<%= txtEditSection.ClientID %>").val(selectedQuestion.text());
            }
        }
    }

    function cancelSectionEditor(obj) {
        $("#<%= hfEditSectionID.ClientID %>").val("");
        $("#<%= txtEditSection.ClientID %>").val("");
        $("#newSectionC").hide("medium");
    }

    function HideUploadLabel() {
        $("#<%=lblUploadedFiles.ClientID %>").hide();
        $("#<%=fileUploadControls.ClientID %>").show();
        $("#<%=ImgEditFiles.ClientID%>").hide();
        return false;
    }
    $(function () {

        //  $(".ddlPickUser").bind("change", function () { });
        $(".skipitem :checkbox").bind("click", function () {
            var isCategory = $(this).parent().attr("source").indexOf("applicationid") == -1 ? true : false;
            var isChecked = $(this).is(":checked");
            var val = $(this).parent().attr("source");
            if (isCategory) {
                var applications = $(".skipitem[source^='" + val + "']");

                $.each(applications, function (i, item) {
                    $(item).find(":checkbox").attr("checked", false);
                    if (isChecked) {
                        $(item).find(":checkbox").attr("checked", true);
                    }
                });
            }
            else {
                //var CategoryVal = val.split("-")[0] + "-" + val.split("-")[1];
                var CategoryVal = val.split("~")[0];
                var applications = $(".skipitem[source^='" + CategoryVal + "']");
                var selectedApplications = $(".skipitem[source^='" + CategoryVal + "']").find(":checked");
                var category = $(".skipitem[source='" + CategoryVal + "']");
                if (category.length > 0) {
                    if (!isChecked) {
                        category.find(":checkbox").removeAttr("checked");
                    }
                    else if (applications.length - 1 == selectedApplications.length) {
                        category.find(":checkbox").attr("checked", true);
                    }
                }
            }
            var items = $(".skipitem[source^='category']");
            var isAllChecked = true;
            $.each(items, function (i, item) {
                if ($(item).find(":checkbox").is(":checked") == false) {
                    isAllChecked = false;
                }
            });

            if (isAllChecked == true) {
                $(".rtallcategory :checkbox").attr("checked", true)
            }
            else {
                $(".rtallcategory :checkbox").attr("checked", false)
            }
        });

        $(".rtallcategory :checkbox").bind("click", function () {
            var isChecked = $(this).is(":checked");

            var items = $(".skipitem[source^='category']");
            $.each(items, function (i, item) {
                $(item).find(":checkbox").attr("checked", false);
                if (isChecked) {
                    $(item).find(":checkbox").attr("checked", true);
                }

            });

        });
        changeDate();
        EnableDateValidation();
    });


    function ChkSections(sender, args) {
        var newSection = document.getElementById("newSectionC");
        var e = document.getElementById("<%=questionServiceSectionsDD.ClientID%>");
        var tst = "ss";
        var existingSection = e.options[e.selectedIndex].value;
        var txtEditSection = document.getElementById("<%=txtEditSection.ClientID%>").value;
        if ((existingSection > 0) || (newSection.style.display != "none" && txtEditSection.trim() != "")) {
            $("#errormsg").css({ 'display': "none" });
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }

    function EnableDateValidation() {
        
        if ($("#<%=chkDateValidations.ClientID%>").is(':checked')) {
            $("#<%=conditionalDateHandler.ClientID%>").css('display', 'inline-block');
            $(".tblDateValidations").css('border', "1px solid #a5a5a5");
        }
        else {
            $("#<%=conditionalDateHandler.ClientID%>").css('display', 'none');
            $(".tblDateValidations").css('border', "none");
        }
    }
    function DateValidation() {
        var val = $("#<%=rdbDateValidationAgainst.ClientID%>").find(":checked").val();
        if (val == "questions") {
            $("#<%=ddlDateQuestions.ClientID%>").css('display', 'inline-block');
        }
        else {
            $("#<%=ddlDateQuestions.ClientID%>").val("0");
            $("#<%=ddlDateQuestions.ClientID%>").css('display', 'none');
        }
        changeDate();
    }
    function changeDate() {

        var val = $("#<%=ddlDateQuestions.ClientID%> :selected").text();
        if ($("#<%=ddlDateQuestions.ClientID%> :selected").val() == "0")
            val = $("#<%=rdbDateValidationAgainst.ClientID%> :checked").next('label:first').html();
        $("#<%=pastDates.ClientID%>").next('label:first').html("Allow Dates before " + val + "");
        $("#<%=futureDates.ClientID%>").next('label:first').html("Allow Dates after " + val + "");
        $("#<%=presentDates.ClientID%>").next('label:first').html("Allow Dates equal to " + val + "");
    }

    function handelTxtFunctionality() {
        var val = $('#<%=ddlTxtBoxType.ClientID%> :selected').text();
        chkSetLength.SetChecked(false);
        if (val == "Text") {
            $('#<%=chkSetLength.ClientID%>').css("display", "inline");
        }
        else {
            $('#<%=chkSetLength.ClientID%>').css("display", "none");
            $('#<%=lengthLabelTr.ClientID%>').addClass("hide");
            $('#<%=lengthChkbxTr.ClientID%>').addClass("hide");
        }
    }

    function hideShowTextLengthSetter() {
        if (chkSetLength.GetChecked()) {
            $('#<%=lengthLabelTr.ClientID%>').removeClass("hide");
            $('#<%=lengthChkbxTr.ClientID%>').removeClass("hide");
        }
        else {
            $('#<%=lengthLabelTr.ClientID%>').addClass("hide");
            $('#<%=lengthChkbxTr.ClientID%>').addClass("hide");
        }
    }
    function onAssetTypeChange(e) {
        if (e.checked)
            $('#<%=divAssetTypeDropdown.ClientID%>').removeClass("hide");
        else
            $('#<%=divAssetTypeDropdown.ClientID%>').addClass("hide");
    }
    function LnkbtnDelete_Click(s, e) {
        if (confirm('Are you sure you want to delete this and related question mapping?')) {
            e.processOnServer = true;
        }
        else {
            e.processOnServer = false;
        }
    }
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });
</script>
<div style="display: none">
    <ugit:UserValueBox ID="tttt" runat="server" AugmentEntitiesFromUserInfo="true" />
    <dx:ASPxDateEdit ID="ddd" runat="server"></dx:ASPxDateEdit>
</div>

<asp:HiddenField ID="hfQuestionID" runat="server" />
<asp:HiddenField ID="hfServiceID" runat="server" />
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div style="padding-bottom: 2px;">
                <asp:Label ID="lblMeesageNoParentService" runat="server" Visible="false" ForeColor="Red" 
                    Text="Service will not create any task if &ldquo;Create Parent Service Request&rdquo; option is unchecked."></asp:Label>
            </div>
        </div>
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Question<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtQuestion" CssClass="asptextbox-asp" runat="server" ValidationGroup="questionGroup"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtQuestion"
                    Display="Dynamic" ErrorMessage="Please enter title." CssClass="error"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row" id="tr3" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Token(short name)<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <span style="margin-left:3px; margin-right:10px;"> [$ </span>
                 <asp:TextBox ID="txtToken" runat="server" ValidationGroup="questionGroup" CssClass="asptextbox-asp tokenTextBox"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtToken"
                    Display="Dynamic" ErrorMessage="Please enter token." CssClass="error"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="cvFieldValidator1" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtToken"
                    Display="Dynamic" ErrorMessage="Token Must be unique in service." OnServerValidate="CVFieldValidator1_ServerValidate" CssClass="error"></asp:CustomValidator>
                <span style="margin-left:10px;"> $]</span>
            </div>
        </div>
        <div class="row" id="tr4" runat="server">
            <div class="ms-formbody accomp_inputField crm-checkWrap-right" >
                <asp:CheckBox ID="chkIsContinue" runat="server" Text="Continue Prev Line" />
            </div>
        </div>
        <div class="row" id="trMandatory" runat="server">
            <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                <asp:CheckBox ID="cbMandatory" runat="server" Text="Mandatory" ValidationGroup="questionGroup" />
            </div>
        </div>

        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Question Type</h3>
            </div>
            <div class="">
                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                    <div class="row">
                        <div class="ms-formbody accomp_inputField">
                            <asp:DropDownList runat="server" ID="questionBasedOnNew" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                                OnSelectedIndexChanged="QuestionBasedOnNew_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row" id="pDropdownProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row" id="pDropdownControlType" visible="false" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Option Type:</h3>
                                    </div>
                                </div>
                                <div class="row" id="pDropdownControlTypeButtion" visible="false" runat="server">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlOptionType" runat="server" CssClass="ddltxtboxtype itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Text="DropDown" Value="dropdown"></asp:ListItem>
                                            <asp:ListItem Text="RadioButtons" Value="radiobuttons"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                        <asp:CheckBox ID="PickfromField" CssClass="PickfromField" runat="server"
                                            Text="Pick choices field" Visible="false" ValidationGroup="questionGroup" AutoPostBack="true"
                                            OnCheckedChanged="PickfromField_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="row" id="trDropdownDefaultOptions" runat="server" visible="false">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Enter each choice on a separate line:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox TextMode="MultiLine" Width="100%" ID="txtDropdownOptions" runat="server"
                                            CssClass="txtdropdownoptions asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" id="ChoseFromDropdown" runat="server" visible="false">
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <asp:DropDownList ID="ChooseFromList" runat="server" AutoPostBack="true" Visible="true" CssClass="itsmDropDownList aspxDropDownList"
                                            OnSelectedIndexChanged="DDLChooseFromList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <asp:DropDownList ID="ChooseFromFields" AutoPostBack="true" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                        </asp:DropDownList>
                                    </div>                                    
                                </div>
                                <div class="row" id="singleChoiceDefaulttr" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Default Value:</h3>
                                    </div>
                                </div>
                                <div class="row" id="singleChoiceTextboxtr" runat="server">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox ID="txtDropDownDefaultVal" runat="server" CssClass="txtdropdowndefaultval asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pMultiSelectProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row" id="trpDropdownControlType" visible="false" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Option Type:</h3>
                                    </div>
                                </div>
                                <div class="row" id="trpDropdownControlTypeButtion" visible="false" runat="server">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlMultiSelectOptionType" runat="server" CssClass="ddltxtboxtype itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Text="DropDown" Value="dropdowngrid"></asp:ListItem>
                                            <asp:ListItem Text="CheckBox" Value="radiobuttons"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row" id="trPickFromFieldMC" runat="server">
                                    <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                        <asp:CheckBox ID="chkbxPickFromFieldMC" CssClass="PickfromField" runat="server" Text="Pick choice field"
                                            ValidationGroup="questionGroup" AutoPostBack="true" OnCheckedChanged="chkbxPickFromFieldMC_CheckedChanged" />
                                    </div>
                                </div>
                                <div id="trChooseFromListMC" runat="server" visible="false">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlChoosefromListMC" Visible="false" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                                            OnSelectedIndexChanged="ddlChoosefromListMC_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlChoosefromFieldMC" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                        </asp:DropDownList>
                                    </div>

                                </div>
                                <div class="row" id="trMultiChoiceValuesCaption" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Enter each choice on a separate line:</h3>
                                    </div>
                                </div>
                                <div class="row" id="trMultiChoiceValues" runat="server">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox TextMode="MultiLine" Width="100%" Rows="4" ID="txtMSOptions" runat="server" CssClass="txtdropdownoptions asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Default Value:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox ID="txtMSDefault" runat="server" CssClass="txtdropdowndefaultval asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pCheckboxProperties" runat="server" visible="false">
                        <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                            <asp:CheckBox ID="cbDefaultValue" runat="server" Text="Default Value" />
                        </div>
                    </div>
                    <div class="row" id="pTxtBoxProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Data Type:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlTxtBoxType" onchange="handelTxtFunctionality();" runat="server" CssClass="ddltxtboxtype fleft">
                                            <asp:ListItem Text="Text" Value="Text"></asp:ListItem>
                                            <asp:ListItem Text="Multiline" Value="Multiline"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="ms-formbody accomp_inputField setlength">
                                        <dx:ASPxCheckBox ID="chkSetLength" EnableClientSideAPI="true" CssClass="setChkbxAttrib" ClientInstanceName="chkSetLength" Text="Restrict Length" TextAlign="Right" runat="server" Checked="false">
                                            <ClientSideEvents CheckedChanged="function(s,e){   hideShowTextLengthSetter();  }" />
                                        </dx:ASPxCheckBox>
                                    </div>
                                </div>

                                <div class="row hide" id="lengthLabelTr" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Max Length:</h3>
                                    </div>
                                </div>
                                <div class="row hide" id="lengthChkbxTr" runat="server">
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxSpinEdit ID="spnbtnTextMaxLength" Width="100%" MinValue="1" MaxValue="255" runat="server" ShowIncrementButtons="False"
                                            SpinButtons-ShowLargeIncrementButtons="false" CssClass="aspxSpinEdit-controller">
                                        </dx:ASPxSpinEdit>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Default Value:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox ID="txtTBDefault" runat="server" CssClass="txtdropdowndefaultval asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" id="trUserDeskLocation" runat="server" visible="false">
                                    <div class="ms-formbody accomp_inputField  crm-checkWrap-right">
                                        <asp:CheckBox ID="chkUserDeskLocation" runat="server" AutoPostBack="true" Text="Prefill with desk location of user" OnCheckedChanged="chkUserDeskLocation_CheckedChanged" />
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlUserFieldDeskLocation" runat="server" Visible="false" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pUserProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">User Type:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged"
                                            CssClass=" itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Value="0" Text="Any"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Users Only"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Groups Only"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="Logged-In User"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="Manager Only"></asp:ListItem>
                                            <asp:ListItem Value="5" Text="Specific User/Group"></asp:ListItem>
                                        </asp:DropDownList>
                                        <div class="crm-checkWrap-right" style="margin-top:10px;">
                                            <asp:CheckBox ID="chkSingleEntryOnly" runat="server" Text="Single Entry Only" />
                                        </div>
                                    </div>
                                    <div style="clear: both"></div>
                                </div>
                                <div class="row" id="trGrpPeoplePicker" runat="server" visible="false">
                                    <div class="ms-formbody accomp_inputField">
                                        <ugit:UserValueBox ID="pEditorGrp" runat="server" CssClass="userValueBox-dropDown" />
                                    </div>
                                </div>
                                <div class="row" id="trUserDefaultValue" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Default Value:</h3>
                                    </div>
                                    <div id="divDefaultUser" runat="server" class="ms-formbody accomp_inputField">
                                        <%--<ugit:UserValueBox  PrincipalSource="UserInfoList" ID="pEditorUser"  IsNotPostBack="true" runat="server" Height="30" 
                                            AugmentEntitiesFromUserInfo="true" CssClass="userValueBox-dropDown" />--%>
                                    </div>
                                </div>
                                <div class="row" id="trUserManager" runat="server" visible="false" style="padding-top: 5px;">
                                    <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                        <asp:CheckBox ID="chkUserManager" runat="server" AutoPostBack="true" Text="Prefill with manager of user" OnCheckedChanged="chkUserManager_CheckedChanged" />
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlUserManager" runat="server" Visible="false" CssClass="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pUserFieldProperties" runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row" id="trassetowner" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Asset Owner:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                        <asp:CheckBox ID="chkincludeasset" Text="Include Department Assets" runat="server" />
                                    </div>
                                </div>
                                <div class="row" id="trradiooption" runat="server">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:RadioButtonList ID="rdAssetowner" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" CssClass="custom-radiobuttonlist" 
                                            OnSelectedIndexChanged="rdAssetowner_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="1">Current User</asp:ListItem>
                                            <asp:ListItem Value="2">Specific User</asp:ListItem>
                                            <asp:ListItem Value="3">User from Question</asp:ListItem>
                                            <asp:ListItem Value="4">All</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                              
                                <div class="row" id="trcurrentuseruserfield1" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Select User: </h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlUserField" runat="server" CssClass=" itsmDropDownList aspxDropDownList"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row trcurrentpeoplecls" id="trcurrentuserpeoplepicker" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Select User:</h3>
                                    </div>
                                </div>
                                <div class="row" id="trcurrentuserpeoplepicker1" runat="server" style="height: 15px">
                                    <div class="ms-formbody accomp_inputField">
                                        <ugit:UserValueBox ID="pAssetpeopleeditor" runat="server" AugmentEntitiesFromUserInfo="true" CssClass="userValueBox-dropDown" />
                                    </div>
                                </div>
                                <div class="row" id="trAssetType" runat="server">
                                    <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                        <div id="divAssetType" class="ms-formbody accomp_inputField crm-checkWrap-right" runat="server">
                                            <asp:CheckBox ID="chkbxAssetType" runat="server" Checked="true" Text="Selected Asset Type(s):" onClick="onAssetTypeChange(this)" />
                                           <%-- <dx:ASPxCheckBox ID="chkbxAssetType1" runat="server" Text="Selected Asset Type(s): ">
                                                <ClientSideEvents CheckedChanged="function(s,e){onAssetTypeChange(s,e)}" />
                                            </dx:ASPxCheckBox>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                        <div id="divAssetTypeDropdown" runat="server" class="ms-formbody accomp_inputField assetType-list">
                                            <ugit:RequestTypeDropDownList ModuleName="CMDB" HeaderCaption="Asset Type" runat="server" ID="rqDropdown" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pDateProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="tbDateProperties">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Date Format:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlDateFormat" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Text="DateTime" Value="DateTime"></asp:ListItem>
                                            <asp:ListItem Text="DateOnly" Value="DateOnly"></asp:ListItem>
                                            <asp:ListItem Text="TimeOnly" Value="TimeOnly"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                        <div class="ms-formtable accomp-popup tblDateValidations">
                                            <div class="row">
                                                <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                                    <asp:CheckBox ID="chkDateValidations" CssClass="chkDateValidation" Text="Enable Validations" runat="server" OnClick="EnableDateValidation()" />
                                                </div>
                                            </div>
                                            <div class="row" id="conditionalDateHandler" runat="server" style="display: none;">
                                                <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                                    <div class="">
                                                        <div class="row">
                                                            <div class="ms-formlabel">
                                                                <h3 class="ms-standardheader budget_fieldLabel">Validate Against:</h3>
                                                            </div>
                                                            <div class="ms-formbody accomp_inputField">
                                                                <asp:RadioButtonList ID="rdbDateValidationAgainst" CssClass="rdbdates" runat="server" RepeatDirection="Horizontal" RepeatLayout="flow" OnClick="DateValidation()">
                                                                    <asp:ListItem Value="currentdate" Text="Current Date" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="questions" Text="From Other Question"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="ms-formbody accomp_inputField">
                                                                <asp:DropDownList ID="ddlDateQuestions" onchange="changeDate()" Style="display: none"
                                                                    runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                                                <asp:CheckBox ID="pastDates" Text="Allow Dates before Current Date" Checked="true" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                                                <asp:CheckBox ID="futureDates" Text="Allow Dates after Current Date" Checked="true" runat="server" />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                                                <asp:CheckBox ID="presentDates" Text="Allow Dates equal to Current Date" Checked="true" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Default Date:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                            <asp:DropDownList ID="ddlPlusMinusDefaultDate" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                                                <asp:ListItem Value="0">+</asp:ListItem>
                                                <asp:ListItem Value="1">-</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="co-md-6 col-sm-6 col-xs-12 noPadding">
                                            <dx:ASPxSpinEdit ID="speDefaultDateNoofDays" HelpTextSettings-VerticalAlign="Middle" HelpText="Day(s)"
                                                HelpTextSettings-Position="Right" Width="75px" MinValue="0" MaxValue="365" runat="server" ShowIncrementButtons="False"
                                                SpinButtons-ShowLargeIncrementButtons="false" Paddings-Padding="0px">
                                            </dx:ASPxSpinEdit>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pLookupProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Lookup List:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlLookupList" AutoPostBack="true" runat="server" CssClass="itsmDropDownList aspxDropDownList"
                                            OnSelectedIndexChanged="DDLLookupList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row" id="lpModuletr" runat="server" visible="false">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Module Name:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList CssClass="itsmDropDownList aspxDropDownList" ID="ddlLPModule" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <%--no need to show lookup field as not necessary--%>
                                <div class="row" style="display:none"> 
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Lookup Field:</h3>
                                    </div>
                                </div>
                                <div class="row" style="display:none">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList CssClass="itsmDropDownList aspxDropDownList" ID="ddlLookupFields" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                        <asp:CheckBox ID="chbAllowMultiLookup" Visible="false" runat="server" Text="Allow Multiple" TextAlign="Right" />
                                    </div>
                                </div>
                                <div class="row" id="tr1radiooption" runat="server" style="display: none;">
                                    <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                        <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                            <asp:CheckBox ID="chkDepartmentowner" runat="server" TextAlign="Right" AutoPostBack="true"
                                                OnCheckedChanged="chkDepartmentowner_CheckedChanged" Text="Pre-fill department of user" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                        <div class="ms-formbody accomp_inputField">
                                            <%--<ugit:UserValueBox ID="ddlUserField1" runat="server" Height="20px" Visible="false"></ugit:UserValueBox>--%>
                                            <asp:DropDownList CssClass="itsmDropDownList aspxDropDownList" ID="ddlUserField1" runat="server" Visible="false"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="trSubLocationConfig" runat="server" style="display: none;">
                                    <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                        <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                            <asp:CheckBox ID="chkDepndLocation" runat="server" TextAlign="Right" AutoPostBack="true"
                                                OnCheckedChanged="chkDepndLocation_CheckedChanged" Text="Tie to Location Question" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                        <div class="ms-formbody accomp_inputField">
                                            <asp:DropDownList ID="ddlLocationQuestions" runat="server" CssClass="itsmDropDownList aspxDropDownList" Visible="false"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pAttachmentProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:RadioButton ID="cbAttSingle" GroupName="attm" runat="server" Text="Single" AutoPostBack="true" OnCheckedChanged="Attm_CheckedChanged" />
                                        <asp:RadioButton ID="cbAttMultiple" GroupName="attm" runat="server" Text="Multiple" AutoPostBack="true" OnCheckedChanged="Attm_CheckedChanged" />
                                    </div>
                                </div>
                                <div class="row" id="attSubContainer" runat="server" visible="false">
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                        <div class="ms-formtable accomp-popup">
                                            <div class="row">
                                                <div class="ms-formlabel">
                                                    <h3 class="ms-standardheader budget_fieldLabel">No. of Attachment</h3>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="ms-formbody accomp_inputField">
                                                    <asp:DropDownList ID="ddlAttCount" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Size Limit(MB)</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox ID="txtAttSizeLimit" runat="server" Text="10" CssClass="asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pRequestTypeProperties" runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Module</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlRTModule" AutoPostBack="true" runat="server" CssClass="itsmDropDownList aspxDropDownList"
                                            OnSelectedIndexChanged="DDLRTModules_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                        <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                            <asp:CheckBox ID="chkEnableRCategoryDropDown" runat="server" Text="Enable Category Dropdown" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                        <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                            <asp:CheckBox ID="chkbxEnableIssueType" runat="server" Text="Enable Issue Type Dropdown" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="trrtcategory ms-formbody accomp_inputField">
                                        <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                            <div class="crm-checkWrap-right">
                                                <asp:CheckBox ID="cbRTAllCategory" CssClass="rtallcategory" runat="server" Text="All Categories" />
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-12 noPadding">
                                            <div class="rtcategorylist requestType-table">
                                                <asp:CheckBoxList Width="200" Height="100px" ID="ddlRTCategory" runat="server" Visible="false"></asp:CheckBoxList>
                                                <asp:HiddenField ID="hdnRequestTypeModule" runat="server" />
                                                <dx:ASPxTreeList ID="requestTypeTreeList" runat="server" Width="100%" ClientInstanceName="requestTypeTreeList" AutoGenerateColumns="false"
                                                    AutoGenerateServiceColumns="true" OnDataBound="requestTypeTreeList_DataBound" KeyFieldName="ID" ParentFieldName="ParentID" Border-BorderStyle="Solid">
                                                    <Columns>
                                                        <dx:TreeListDataColumn VisibleIndex="0" FieldName="RequestType" Caption="Request Type">
                                                        </dx:TreeListDataColumn>
                                                    </Columns>
                                                    <Styles>
                                                        <AlternatingNode Enabled="True">
                                                        </AlternatingNode>
                                                    </Styles>
                                                    <SettingsSelection Enabled="True" AllowSelectAll="true" Recursive="true" />
                                                </dx:ASPxTreeList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="trRTViewTypeLB" runat="server" visible="false">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">View Type</h3>
                                    </div>
                                </div>
                                <div class="row" id="trRTViewTypeddl" runat="server" visible="false">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlRTDropdownType" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                                            <asp:ListItem Text="Single Dropdown" Value="Single"></asp:ListItem>
                                            <asp:ListItem Text="Dependent Dropdown" Value="Dependent"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pNumberProperties" runat="server" visible="false">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Data Type:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlNumberBoxType" runat="server" CssClass="itsmDropDownList aspxDropDownListddltxtboxtype">
                                            <asp:ListItem Text="Integer" Value="Integer"></asp:ListItem>
                                            <asp:ListItem Text="Double" Value="Double"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Default Value:</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:TextBox ID="txtNumberDefaultValue" runat="server" CssClass="txtdropdowndefaultval asptextbox-asp"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="pAppAccessReqProperties" runat="server">
                        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                            <div class="pAppAccessReqProperties">
                                <div class="row" id="trDueDate" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Due Date:</h3>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-3 noPadding">
                                        <div class="ms-formbody accomp_inputField">
                                            <asp:DropDownList ID="ddlDueDateFrom" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                                                <asp:ListItem Value="today">Today</asp:ListItem>
                                                <asp:ListItem Value="ques">Questions</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-sm-4 col-xs-3 noPadding">
                                        <div class="ms-formbody accomp_inputField">
                                            <asp:DropDownList ID="ddlPlusMinus" CssClass="itsmDropDownList aspxDropDownList" runat="server">
                                                <asp:ListItem Value="0">+</asp:ListItem>
                                                <asp:ListItem Value="1">-</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-4 co-sm-4 col-xs-3 noPadding">
                                        <dx:ASPxSpinEdit ID="dxNoOfDays" HelpTextSettings-VerticalAlign="Middle" HelpText="Day(s)" HelpTextSettings-Position="Right"
                                             MinValue="0" MaxValue="365" CssClass="aspxSpinEdit-dropDown queDueDate" runat="server" ShowIncrementButtons="False" 
                                            SpinButtons-ShowLargeIncrementButtons="false">
                                        </dx:ASPxSpinEdit>
                                    </div>
                                </div>
                                <div class="row" id="trpredecessorslable" runat="server" visible="false">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabe">Predecessors:</h3>
                                    </div>
                                </div>
                                <div class="row" id="trpredecessorsControl" runat="server" visible="false">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:PlaceHolder runat="server" ID="pheditcontrol" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabe">Pick User from</h3>
                                    </div>
                                </div>
                                <div class="row trrtApplications">
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:RadioButtonList ID="rdbPickUserFrom" RepeatDirection="Horizontal" runat="server" AutoPostBack="true" 
                                            OnSelectedIndexChanged="rdbPickUserFrom_SelectedIndexChanged" CssClass="custom-radiobuttonlist">
                                            <asp:ListItem Value="1" Selected="True">New User</asp:ListItem>
                                            <asp:ListItem Value="2">Existing User</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="row" id="trAutoCreateAccountTask" visible="false" runat="server">
                                    <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                        <asp:CheckBox ID="chkAutoCreateAccountTask" runat="server" Text="Auto Create Account" />
                                    </div>
                                </div>
                                <div class="row" id="trUserFieldAppRequest" visible="false" runat="server">
                                    <div id="lbUserFieldAppRequestQuestions" class="ms-formlabel" runat="server" visible="false">
                                        <h3 class="ms-standardheader budget_fieldLabel">User Name:</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <%--//<ugit:UserValueBox ID="ddlUserFieldAppRequestQuestions"  runat="server" SelectionSet="User"/>
                                        --%>
                                        <dx:ASPxGridLookup AllowUserInput="false" NullText="Please Select" KeyFieldName="ID" AutoPostBack="false" GridViewProperties-Settings-VerticalScrollBarMode="Auto" TextFormatString="{0}" SelectionMode="Multiple"
                                            ID="ddlUserFieldAppRequestQuestions" runat="server" Width="100%" CssClass="aspxGridLookUp-dropDown">
                                            <Columns>
                                                <dx:GridViewCommandColumn ShowSelectCheckbox="true" SelectAllCheckboxMode="Page" Width="30"></dx:GridViewCommandColumn>
                                                <dx:GridViewDataTextColumn FieldName="QuestionTitle" Caption="Question"></dx:GridViewDataTextColumn>
                                            </Columns>
                                            <GridViewProperties>
                                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                                <Settings ShowColumnHeaders="false" />
                                            </GridViewProperties>
                                            <ValidationSettings ValidationGroup="questionGroup" RequiredField-IsRequired="true" ErrorDisplayMode="Text" RequiredField-ErrorText="Please Select"></ValidationSettings>
                                        </dx:ASPxGridLookup>
                                    </div>
                                </div>
                                <div class="row" id="trMirrorAccessFrom" visible="false" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Mirror Access From: </h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <asp:DropDownList ID="ddlAccessMirrorFrom" CssClass="itsmDropDownList aspxDropDownList" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row" id="trAccessRequestMode" runat="server" visible="false">
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                                        <div class="ms-formtable accomp-popup">
                                            <div class="row">
                                                <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                                                    <asp:CheckBox ID="chbxEnableAccessMode" AutoPostBack="true" OnCheckedChanged="chbxEnableAccessMode_CheckedChanged"
                                                        runat="server" Text="Enable Access Request Mode" />
                                                </div>
                                            </div>
                                            <div class="row" id="trAccessModeOptions" runat="server" visible="false">
                                                <div class="ms-formbody accomp_inputField">
                                                    <asp:CheckBoxList ID="chkbxlstAccessReqMode" runat="server" CssClass="custom-chkList">
                                                        <asp:ListItem Text="Add/Change" Value="Add"></asp:ListItem>
                                                        <asp:ListItem Text="Remove from Specific Application(s)" Value="Remove"></asp:ListItem>
                                                        <asp:ListItem Text="Remove from All Application(s)" Value="RemoveAll"></asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Application(s)</h3>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="ms-formbody accomp_inputField trrtApplications">
                                        <div class="crm-checkWrap-right" style="margin-bottom:5px; display:inline-block; margin-right:15px;">
                                            <asp:CheckBox ID="cbRTAllApplications" CssClass="rtallcategory" runat="server" Text="All Applications" />
                                        </div>
                                        <div class="crm-checkWrap-right" style="margin-bottom:5px; display:inline-block; margin-left:15px;"">
                                            <asp:CheckBox ID="cbDisableAllCheckBox" runat="server" Text="Disable Select All Checkbox" />
                                        </div>
                                        <div style="height: 150px; overflow-y: scroll; float: left; border: 1px solid #ccc; border-radius:4px; width: 99%; padding:5px;" class="rtcategorylist">
                                            <asp:CheckBoxList CssClass="crm-checkWrap" Height="150px" ID="chkRTApplications" runat="server" BorderStyle="None"></asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                   
                </div>
            </div>
        </div>
        <div class="row" id="tr2" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Section<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-12 noPadding existing-section-c">
                    <div class="queEditor-sectionDropDown">
                        <asp:DropDownList runat="server" ID="questionServiceSectionsDD" CssClass="itsmDropDownList aspxDropDownList" ValidationGroup="questionGroup"></asp:DropDownList>
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2"  runat="server" ValidationGroup="questionGroup" ControlToValidate="questionServiceSectionsDD"
                    Display="Dynamic" ErrorMessage="Please select section" InitialValue="0" ></asp:RequiredFieldValidator>--%>


                        <asp:CustomValidator ID="customvalidatorSection" runat="server" EnableClientScript="true"
                            ErrorMessage="Please select section"
                            ClientValidationFunction="ChkSections"
                            ControlToValidate="questionServiceSectionsDD" ValidationGroup="questionGroup"
                            Display="Dynamic" CssClass="error">
                        </asp:CustomValidator>
                    </div>
                    <div class="queEditor-sectionIcons existing-section-a">
                        <img alt="New Section" src="/Content/images/plus-blue.png" style="cursor: pointer; width: 16px;" onclick="addNewQuestionSection(this)" />
                        <img alt="Edit Section" src="/Content/images/editNewIcon.png" style="cursor: pointer; width: 16px;" onclick="editQuestionSection(this)" />
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12 noPadding new-section-c" id="newSectionC" style="display: none">
                    <div class="queEditor-txtEditSection">
                        <asp:TextBox ID="txtEditSection" runat="server" CssClass="asptextbox-asp"></asp:TextBox>
                        <asp:HiddenField ID="hfEditSectionID" runat="server" />
                    </div>
                    <div class="queEditor-txtEditSectionImg">
                        <img alt="Cancel" src="/Content/images/close-blue.png" style="cursor: pointer; width: 16px;" class="canceladdcategory"
                            onclick="cancelSectionEditor(this);" />
                    </div>
                </div>
                <div id="errormsg"></div>
            </div>
        </div>
        <div class="row" id="tr5" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Help Text</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="questionHelpTextNew" Width="100%" runat="server" TextMode="MultiLine" CssClass="asptextbox-asp"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="trZoom" runat="server" visible="false">
            <div class="ms-formbody accomp_inputField crm-checkWrap-right">
                <asp:CheckBox ID="chkEnableZoomIn" runat="server" Text="Enable Zoom In" ValidationGroup="questionGroup" />
            </div>
        </div>
        <%--<div class="row" id="trServiceHelp" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Question Help</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtWiki" runat="server" />

                    <asp:FileUpload ID="fileupload" class="fileupload" Style="display: none" runat="server" /><br />
                    <a id="aAddItem" runat="server" onclick="showWiki()" style="cursor: pointer;">Add Wiki</a> |
                                    <a onclick="showUploadControl()" style="cursor: pointer;">Upload Document</a>
             </div>
        </div>--%>

        <div id="trNavigationType" runat="server" class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Link Type</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlTargetTypes" CssClass="target_section itsmDropDownList aspxDropDownList"
                    runat="server" onchange="ddlTargetType_SelectedIndexChanged(this)">
                </asp:DropDownList>
            </div>

        </div>

        <div id="trFileUploads" class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">File</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:Label ID="lblUploadedFiles" runat="server"></asp:Label>
                <asp:FileUpload ID="fileUploadControls" CssClass="fileUploader" Width="200px" ToolTip="Browse and upload file" runat="server" Style="display: none;" />
                <img alt="Edit File" title="Edit File" runat="server" id="ImgEditFiles" src="/content/Images/editNewIcon.png" style="cursor: pointer;" 
                    onclick="HideUploadLabel();" width="16" />
                <div>
                    <asp:RequiredFieldValidator ID="rfvFileUpload" CssClass="rfvdFileUploader" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="fileUploadControls" ErrorMessage="Upload a file." Display="Dynamic" ValidationGroup="fileSave"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>
        <div id="trLinks" class="row" style="display: none">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Link URL</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtFileLinks" CssClass="fileUploaderLink asptextbox-asp" runat="server" />
            </div>

        </div>
        <div id="trWikis" style="display: none" class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Wiki
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtWikis" runat="server" CssClass="asptextbox-asp" />
                <a id="aAddItems" runat="server" style="cursor: pointer;">
                    <img alt="Add Wiki" title="Add Wiki" runat="server" id="imgWikis" width="16" src="/content/Images/editNewIcon.png" style="cursor: pointer;" />
                </a>
            </div>
        </div>
        <div id="trHelpCards" style="display: none" class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Help Card</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtHelpCards" runat="server" CssClass=" asptextbox-asp" />
                <a id="aAddHelpCards" runat="server" style="cursor: pointer;">
                    <img alt="Add Help Card" title="Add Help Card" runat="server" width="16" id="img" src="/content/Images/editNewIcon.png" style="cursor: pointer;" />
                </a>
            </div>
        </div>

        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxButton ID="btDelete" ValidationGroup="questionGroup" CssClass="secondary-cancelBtn" runat="server" Text="Delete" Visible="false"
                    OnClick="BtDelete_Click">
                    <ClientSideEvents Click="LnkbtnDelete_Click" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="btSaveQuestion1" ValidationGroup="questionGroup" runat="server" CssClass="primary-blueBtn" Text="Save & New Question"
                    OnClick="BtSaveAndNewQuestionClick">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btSaveQuestion" ValidationGroup="questionGroup" CssClass="primary-blueBtn" runat="server" Text="Save & Close"
                    OnClick="BtSaveQuestionClick">
                </dx:ASPxButton>
                <dx:ASPxButton ID="btClosePopup" runat="server" CssClass="secondary-cancelBtn" Text="Close" AutoPostBack="false">
                    <ClientSideEvents Click="closePopup" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    $(function () {


        $(".trrtcategory .rtcategorylist :checkbox").bind("click", function () {
            var allCategory = $(".trrtcategory .rtcategorylist :checkbox");
            var selectedCategory = $(".trrtcategory .rtcategorylist :checkbox:checked");
            if (allCategory.length == selectedCategory.length) {
                $(".trrtcategory .rtallcategory :checkbox").attr("checked", true);
            }
            else {
                $(".trrtcategory .rtallcategory :checkbox").attr("checked", false);
            }
        });

        $(".trrtcategory .rtallcategory :checkbox").bind("click", function () {
            if (this.checked) {
                $(".trrtcategory .rtcategorylist :checkbox").attr("checked", true);
            }
            else {
                $(".trrtcategory .rtcategorylist :checkbox").attr("checked", false);
            }
        });

    });


</script>

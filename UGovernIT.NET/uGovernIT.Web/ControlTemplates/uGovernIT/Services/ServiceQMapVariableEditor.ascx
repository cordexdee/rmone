<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceQMapVariableEditor.ascx.cs" Inherits="uGovernIT.Web.ServiceQMapVariableEditor" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .fleft {
        float: left;
    }

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

    .existing-section-c {
        float: left;
    }

    .new-section-c {
        float: left;
    }

    .existing-section-a {
        float: left;
        padding: 0px 5px;
    }

        .existing-section-a img {
            cursor: pointer;
        }

    .new-section-a {
        float: left;
        padding-left: 5px;
    }

        .new-section-a img {
            cursor: pointer;
        }

    .poperatorvalue {
        float: left;
        width: 100%;
        max-height: 100px;
        overflow-y: auto;
        overflow-x: hidden;
    }

    .pickervaluec {
        position: relative;
    }

    .pickervaluevontainer {
        background: #fff;
        float: left;
        width: 300px;
        height: 300px;
        display: none;
        z-index: 10000;
        position: absolute;
        overflow-y: auto;
        overflow-x: auto;
        border: 1px solid;
        top: 29px;
        left: 1px;
    }

    .lbpickedvalue {
        float: left;
        margin-top: 4px;
        min-height: 12px;
        max-width: 350px;
        margin-left: 3px;
    }

    .efformsg {
        float: left;
        width: 95%;
    }

    .btaddcondition {
        float: right !important;
    }

    .span-label-header {
        float: left;
        font-weight: bold;
    }

    .panel-question {
        float: left;
        background-color: #d0d1d7;
        border: 1px solid #9698a4;
    }

    .panel-align {
        padding-bottom: 21px;
    }

        .panel-align .poperatorvalue {
            float: left;
            width: auto;
            max-height: 100px;
            overflow-y: auto;
            overflow-x: hidden;
        }

    .bottompickerContainer {
        background: #fff;
        float: left;
        width: 267px;
        height: 300px;
        display: none;
        z-index: 10000;
        position: absolute;
        overflow-y: auto;
        overflow-x: auto;
        border: 1px solid;
        top: 29px;
        left: 1px;
    }

    .pickedvalue {
        float: left;
        margin-top: 2px;
        min-height: 12px;
        max-width: 350px;
        margin-left: 3px;
    }

    td, th {
        padding: 0;
    }

    .default-selector {
        cursor: pointer;
    }

    .multi-addbtn {
        margin-left: 5px;
    }

    .hide-control {
        visibility: hidden;
    }

    .remove-group {
        vertical-align: middle;
        right: -14px;
        width: 14px;
        height: 14px;
        cursor: pointer;
        margin-top: -1px;
    }

    .ms-inputuserfield {
        height: 15px !important;
    }

    .questionitem, .bottompickerContainer input[type="checkbox"] {
        margin-left: 5px !important;
    }
    
    .bottompickerContainer label {
        margin-left: 3px;
    }

    .span-label-header {
        float: left;
        font-weight: bold;
    }
    .aspNetDisabled label {
        font-weight: bold;
        color: inherit;
    }

    .pickervaluec .input-button-bg {
        background: #4A6EE2;
        border-radius: 4px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function addCondition() {
        var txtCondition = $(".cDetail");
        var ddlquestion = $(".cquestions");
        var ddloperator = $(".coperators");
        var txtValue = $(".cvalue");

        var txtConditionText = $.trim(txtCondition.val());

        txtCondition.val(ddlquestion.val() + " " + ddloperator.val() + " [" + txtValue.val() + "]");

    }

    $(function () {
        $(".skipitem :checkbox").bind("click", function () {
            var isSection = $(this).parent().attr("source").indexOf("question") == -1 ? true : false;
            var isChecked = $(this).is(":checked");
            var val = $(this).parent().attr("source");
            if (isSection) {
                var questions = $(".skipitem[source^='" + val + "']");

                $.each(questions, function (i, item) {
                    $(item).find(":checkbox").attr("checked", false);
                    if (isChecked) {
                        $(item).find(":checkbox").attr("checked", true);
                    }
                });
            }
            else {
                var sectionVal = val.split("-")[0] + "-" + val.split("-")[1];
                var questions = $(".skipitem[source^='" + sectionVal + "']");
                var selectedQuestions = $(".skipitem[source^='" + sectionVal + "']").find(":checked");
                var section = $(".skipitem[source='" + sectionVal + "']");
                if (section.length > 0) {
                    if (!isChecked) {
                        section.find(":checkbox").removeAttr("checked");
                    }
                    else if (questions.length - 1 == selectedQuestions.length) {
                        section.find(":checkbox").attr("checked", true);
                    }
                }
            }
        });
    });

    function confirmDelete() {
        var conditionTitle = $(".txttitlediv :text").val();
        if (confirm("Are you sure! you want to delete variable: \"" + conditionTitle + "\"?")) {
            return true;
        }
        else {
            return false;
        }
    }

    function confirmDeleteCondition() {
        if (confirm("Are you sure! you want to delete condition")) {
            return true;
        }
        else {
            return false;
        }
    }

    var multiChoice = true;
    $(function () {

        // Multiple quesiton control
        var multiQuestionContainer = $(".masterquestioncontainer");

        if (multiQuestionContainer.length > 0) {
            var multiQuestionPanels = $(".questioncontainer");

            $.each(multiQuestionPanels, function (index, ctr) {

                var ddlOperator = $(ctr).find(".multioperators");

                var checkboxes = $(ctr).find(".bottompickerContainer input:checkbox");
                if (checkboxes.length > 1) {
                    checkboxes.bind("click", function () {
                        if (ddlOperator.val() == "=" || ddlOperator.val() == "!=") {
                            var selectedCheckboxes = checkboxes.filter(":checked");
                            selectedCheckboxes.removeAttr("checked");
                            $(this).attr("checked", true);
                        }
                        var selectedCheckboxes = checkboxes.filter(":checked");
                        var values = new Array();

                        $.each(selectedCheckboxes, function (i, item) {
                            values.push($(item).parent().find("label").text());
                        });
                        $(ctr).find(".pickedvalue").html(values.join(", "));

                    });
                }

                $(ctr).find(".multioperators").bind("change", function () {
                    if ($(this).val() == "=" || $(this).val() == "!=") {
                        var checkboxes1 = $(ctr).find(".bottompickerContainer input:checkbox");   // pickervaluevontainer bottompickerContainer
                        if (checkboxes1.length > 1) {
                            var selectedCheckboxes = checkboxes1.filter(":checked");
                            if (selectedCheckboxes.length > 1) {
                                selectedCheckboxes.removeAttr("checked");
                                $(selectedCheckboxes[0]).attr("checked", true);
                            }

                            var selectedCheckboxes = checkboxes.filter(":checked");
                            var values = new Array();
                            $.each(selectedCheckboxes, function (i, item) {
                                values.push($(item).parent().find("label").text());
                            });
                            $(ctr).find(".pickedvalue").html(values.join(", "));
                        }
                    }
                });
            });
        }
    });

    function showValuePicker(obj, selectorClass) {
        if ($(obj).val() == "Done") {
            $("." + selectorClass).hide();
            $(obj).val("Pick");
        }
        else {
            $(obj).val("Done");
            $("." + selectorClass).show();
        }
    }

    function SkipCondition_Onchange(obj) {
        if (obj.selectedIndex != 0)
            hdnSelectedSkipLogic.Set("ctrName", "ddlExistingConditions");
        else
            hdnSelectedSkipLogic.Clear();
    }

    function EnableGroup() {
        var items = getSelectedQuestionItems();
        var enableCreate = true;

        if (items.length == 0)
            enableCreate = false;

        //enable create group
        for (var i = 0; i < items.length; i++) {
            if (items[i].parentid > 0 || items.length <= 1) {
                enableCreate = false;
                break;
            }
        }
        btCreateGroup.SetEnabled(enableCreate);
    }

    function getSelectedQuestionItems() {
        var questionItems = $(".questionitem:checked");

        if (questionItems.length == 0) {
            return [];
        }

        var items = [];
        $.each(questionItems, function (i, s) {
            if ($(s).attr("itemid") == undefined || $(s).attr("parentid") == undefined) {
                items = [];
                return false;
            }

            items.push({ itemid: $(s).attr("itemid"), parentid: $(s).attr("parentid") });
        });
        return items;
    }

    function CreateGroup_click(s, e) {
        var items = getSelectedQuestionItems();
        if (items.length <= 1) {
            e.processOnServer = false;
            return;
        }

        var itemIds = [];
        for (var i = 0; i < items.length; i++) {
            itemIds.push(items[i].itemid);
        }

        hdnSelectedSkipLogic.Set("action", 'create');
        hdnSelectedSkipLogic.Set("items", itemIds.join(','));
    }

    function btRemoveGroup_click(id) {
        hdnSelectedSkipLogic.Set("items", id);
        btRemoveGroup.DoClick();
    }

    function btAddCondition_click() {
        btSaveConditionPost.DoClick();
    }

</script>

<dx:ASPxHiddenField ID="hdnSelectedSLogic" runat="server" ClientInstanceName="hdnSelectedSkipLogic"></dx:ASPxHiddenField>
<dx:ASPxButton ID="btRemoveGroup" ClientVisible="false" ForeColor="Red" Paddings-PaddingLeft="5px" CssClass="padding-left5" RenderMode="Link" OnClick="btRemoveGroup_Click" Image-Url="/_layouts/15/Images/uGovernIT/delete-icon.png" ClientInstanceName="btRemoveGroup" runat="server" Text="Remove Group" AutoPostBack="true">
</dx:ASPxButton>

<div style="display: none;">
    <dx:ASPxDateEdit ID="tempDate" runat="server"></dx:ASPxDateEdit>
</div>

<div style="float: left; width: 98%; padding-left: 10px;">
    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
        width="100%">
        <tr id="trExistingConditions" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Variables<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:DropDownList ID="ddlExistingVariables" runat="server" AutoPostBack="true" CssClass="fleft" OnSelectedIndexChanged="DDlExistingVariables_SelectedIndexChanged"></asp:DropDownList>

                <span onclick="return confirmDelete();" style="float: left;">
                    <asp:ImageButton ID="btDeleteButton" runat="server" Text="Delete" OnClick="BtDelete_Click"
                        ImageUrl="/Content/images/delete-icon-new.png" BorderWidth="0" />
                </span>
            </td>
        </tr>

        <tr id="trTitle" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Title:<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody txttitlediv">
                <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="questionGroup"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtTitle"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr id="trShortName" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Token(short name):<b style="color: Red;">*</b>
                </h3>
            </td>
            <td class="ms-formbody txttitlediv">[$<asp:TextBox ID="txtShortName" ValidationGroup="questionGroup" Width="150px" runat="server"></asp:TextBox>$]
                 <asp:RequiredFieldValidator CssClass="efformsg" ID="RequiredFieldValidator2" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtShortName"
                     Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter short name."></asp:RequiredFieldValidator>
                <asp:CustomValidator CssClass="efformsg" ID="cvShortname" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtShortName"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Name already exists" OnServerValidate="cvShortname_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Type:
                </h3>
            </td>
            <td class="ms-formbody txttitlediv">
                <asp:DropDownList ID="ddlVariableType" AutoPostBack="true" OnSelectedIndexChanged="ddlVariableType_SelectedIndexChanged" runat="server">
                    <asp:ListItem Text="TextBox" Value="textbox"></asp:ListItem>
                    <asp:ListItem Text="Numeric" Value="numeric"></asp:ListItem>
                    <asp:ListItem Text="UserField" Value="userfield"></asp:ListItem>
                    <asp:ListItem Text="DateTime" Value="datetime"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trDefault" runat="server">
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Default Value:
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:UpdatePanel ID="upDefaultVal" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="ddlDefaultValQuestion" AutoPostBack="true" runat="server" ValidationGroup="questionGroup"></asp:DropDownList>
                        <asp:Panel ID="pElseConstantVal" runat="server">
                            <asp:TextBox ID="pElseConstantValText" TextMode="MultiLine" CssClass="full-width" runat="server" Visible="false"></asp:TextBox>
                            <dx:ASPxDateEdit ID="pElseConstantValDate" runat="server" ClientVisible="false"></dx:ASPxDateEdit>
                            <ugit:UserValueBox ID="pElseConstantValUser" runat="server" isMulti="false" Visible="false" />

                        </asp:Panel>
                        <asp:CustomValidator ID="cvCompatibilityCheck" OnServerValidate="CVCompatibilityCheck_ServerValidate" ValidationGroup="questionGroup"
                            runat="server" Display="Dynamic" ForeColor="Red" ControlToValidate="ddlDefaultValQuestion" ErrorMessage="Values must be compatible."></asp:CustomValidator>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>

        <tr id="trCondtion" runat="server">
            <td class="ms-formlabel" style="background: #E8EDED;">
                <h3 class="ms-standardheader">New Condition:
                </h3>
            </td>
            <td class="ms-formbody">
                <table width="760px" cellpadding="0" cellspacing="2">
                    <tr>
                        <td colspan="2">
                            <span id="spnQuestion" runat="server" class="hide" style="color: red;">Please select question for condition.</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel runat="server" ID="questionContainerPanel" CssClass="panel-question" Width="750px">
                                <table width="748px">
                                    <tr>
                                        <td width="45px">
                                            <span class="span-label-header">Group</span>
                                        </td>
                                        <td width="117px">
                                            <span class="span-label-header">AND/OR</span>
                                        </td>
                                        <td width="223px">
                                            <span class="span-label-header">Question</span>
                                        </td>
                                        <td width="115px">
                                            <span class="span-label-header">Operator</span>
                                        </td>
                                        <td width="250px">
                                            <span class="span-label-header" style="padding-top: 3px;">Value</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Panel ID="pnlQuestionContainer" runat="server" CssClass="masterquestioncontainer">
                                                <asp:Panel ID="pnlQuestion1" runat="server" CssClass="panel-align questioncontainer">
                                                    <div style="float: left; padding-right: 4px;">
                                                        <div style="width: 40px; float: left;">
                                                            <input type="checkbox" runat="server" id="chkBox1" class="questionitem" disabled="disabled" onclick="EnableGroup();" />
                                                        </div>
                                                        <div style="width: 117px; float: left;">
                                                            <asp:DropDownList ID="ddlCondition1" runat="server" Style="width: 50px !important;" Enabled="false" CssClass="formulafields hide-control">
                                                                <asp:ListItem Text="NONE" Value="NONE" />
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup1" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>
                                                        <asp:DropDownList ID="ddlCQuestions" AutoPostBack="true" CssClass="formulafields" runat="server" Style="width: 220px !important;"
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperators" CssClass="multioperators" runat="server" Style="width: 116px !important;">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div style="width: 240px; float: left;">
                                                        <asp:Panel ID="pOperatorValue" runat="server" CssClass="poperatorvalue">
                                                        </asp:Panel>

                                                        <asp:Panel ID="pPickerValueContainer" runat="server" CssClass="pickervaluec">
                                                            <input type="button" class="input-button-bg" style="height: 17px; padding-top: 1.5px; margin-top: 2px;" value="Pick"
                                                                onclick="showValuePicker(this, 'selector1');" />
                                                            <asp:Label ID="lbPickedValue" runat="server" CssClass="pickedvalue"></asp:Label>

                                                            <div class="bottompickerContainer selector1">
                                                                <asp:Panel ID="pPickerValuePopup" runat="server">
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:ImageButton ID="ibtnAdd1" runat="server" ImageUrl="/Content/images/add_icon.png" OnClick="ibtnAdd_Click"
                                                            CssClass="default-selector multi-addbtn" ToolTip="Add Next Question" AlternateText="Add" />
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="pnlQuestion2" runat="server" CssClass="panel-align questioncontainer" Visible="false">
                                                    <div style="float: left; padding-right: 4px;">
                                                        <div style="width: 40px; float: left;">
                                                            <input type="checkbox" runat="server" id="chkBox2" class="questionitem" onclick="EnableGroup();" />
                                                        </div>

                                                        <div style="width: 117px; float: left;">
                                                            <asp:DropDownList ID="ddlCondition2" runat="server" Style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup2" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>

                                                        <asp:DropDownList ID="ddlQuestion2" AutoPostBack="true" CssClass="formulafields" Style="width: 220px !important;" runat="server"
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator2" CssClass="multioperators" runat="server" Style="width: 116px !important;">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div style="width: 240px; float: left;">
                                                        <asp:Panel ID="pnlOperatorValue2" runat="server" CssClass="poperatorvalue" Width="">
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlPickerValueContainer2" runat="server" CssClass="pickervaluec">
                                                            <input type="button" class="input-button-bg" style="height: 17px; padding-top: 1.5px; margin-top: 2px;"
                                                                title="Pick value for this question" value="Pick" onclick="showValuePicker(this, 'selector2');" />
                                                            <asp:Label ID="lblPickedValue2" runat="server" CssClass="pickedvalue"></asp:Label>

                                                            <div class="bottompickerContainer selector2">
                                                                <asp:Panel ID="pnlPickerValuePopup2" runat="server">
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:ImageButton ID="ibtnRemoveQues2" OnClick="btnRemoveQuestion_Click" ImageUrl="/Content/images/Cancel.png" runat="server"
                                                            ToolTip="Remove this question" Style="cursor: pointer; margin-top: -1px;" AlternateText="Remove" />

                                                        <asp:ImageButton ID="ibtnAdd2" runat="server" ImageUrl="/Content/images/add_icon.png" OnClick="ibtnAdd_Click"
                                                            CssClass="default-selector" Style="margin-top: -1px;" ToolTip="Add Next Question" AlternateText="Add" />
                                                    </div>

                                                </asp:Panel>

                                                <asp:Panel ID="pnlQuestion3" runat="server" CssClass="panel-align questioncontainer" Visible="false">
                                                    <div style="float: left; padding-right: 4px;">
                                                        <div style="width: 40px; float: left;">
                                                            <input type="checkbox" runat="server" id="chkBox3" class="questionitem" onclick="EnableGroup();" />
                                                        </div>

                                                        <div style="width: 117px; float: left;">
                                                            <asp:DropDownList ID="ddlCondition3" runat="server" Style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup3" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>

                                                        <asp:DropDownList ID="ddlQuestion3" AutoPostBack="true" CssClass="formulafields" Style="width: 220px !important;" runat="server"
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator3" CssClass="multioperators" runat="server" Style="width: 116px !important;">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div style="width: 240px; float: left;">
                                                        <asp:Panel ID="pnlOperatorValue3" runat="server" CssClass="poperatorvalue">
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlPickerValueContainer3" runat="server" CssClass="pickervaluec">
                                                            <input type="button" class="input-button-bg" style="height: 17px; padding-top: 1.5px; margin-top: 2px;"
                                                                title="Pick value for this question" value="Pick" onclick="showValuePicker(this, 'selector3');" />
                                                            <asp:Label ID="lblPickedValue3" runat="server" CssClass="pickedvalue"></asp:Label>

                                                            <div class="bottompickerContainer selector3">
                                                                <asp:Panel ID="pnlPickerValuePopup3" runat="server">
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:ImageButton ID="ibtnRemoveQues3" OnClick="btnRemoveQuestion_Click" ImageUrl="/Content/images/Cancel.png" runat="server"
                                                            ToolTip="Remove this question" Style="cursor: pointer; margin-top: -1px;" AlternateText="Remove" />

                                                        <asp:ImageButton ID="ibtnAdd3" runat="server" ImageUrl="/Content/images/add_icon.png" OnClick="ibtnAdd_Click"
                                                            CssClass="default-selector" Style="margin-top: -1px;" ToolTip="Add Next Question" AlternateText="Add" />
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="pnlQuestion4" runat="server" CssClass="panel-align questioncontainer" Visible="false">
                                                    <div style="float: left; padding-right: 4px;">
                                                        <div style="width: 40px; float: left;">
                                                            <input type="checkbox" runat="server" id="chkBox4" class="questionitem" onclick="EnableGroup();" />
                                                        </div>

                                                        <div style="width: 117px; float: left;">
                                                            <asp:DropDownList ID="ddlCondition4" runat="server" Style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup4" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>

                                                        <asp:DropDownList ID="ddlQuestion4" AutoPostBack="true" CssClass="formulafields" Style="width: 220px !important;" runat="server"
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator4" CssClass="multioperators" runat="server" Style="width: 116px !important;">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div style="width: 240px; float: left;">
                                                        <asp:Panel ID="pnlOperatorValue4" runat="server" CssClass="poperatorvalue">
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlPickerValueContainer4" runat="server" CssClass="pickervaluec">
                                                            <input type="button" class="input-button-bg" style="height: 17px; padding-top: 1.5px; margin-top: 2px;"
                                                                title="Pick value for this question" value="Pick" onclick="showValuePicker(this, 'selector4');" />
                                                            <asp:Label ID="lblPickedValue4" runat="server" CssClass="pickedvalue"></asp:Label>

                                                            <div class="bottompickerContainer selector4">
                                                                <asp:Panel ID="pnlPickerValuePopup4" runat="server">
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:ImageButton ID="ibtnRemoveQues4" OnClick="btnRemoveQuestion_Click" ImageUrl="/Content/images/Cancel.png" runat="server"
                                                            ToolTip="Remove this question" Style="cursor: pointer; margin-top: -1px;" AlternateText="Remove" />

                                                        <asp:ImageButton ID="ibtnAdd4" runat="server" ImageUrl="/Content/images/add_icon.png" OnClick="ibtnAdd_Click"
                                                            CssClass="default-selector" Style="margin-top: -1px;" ToolTip="Add Next Question" AlternateText="Add" />
                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="pnlQuestion5" runat="server" CssClass="panel-align questioncontainer" Visible="false">
                                                    <div style="float: left; padding-right: 4px;">
                                                        <div style="width: 40px; float: left;">
                                                            <input type="checkbox" runat="server" id="chkBox5" class="questionitem" onclick="EnableGroup();" />
                                                        </div>

                                                        <div style="width: 117px; float: left;">
                                                            <asp:DropDownList ID="ddlCondition5" runat="server" Style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup5" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>


                                                        <asp:DropDownList ID="ddlQuestion5" AutoPostBack="true" CssClass="formulafields" Style="width: 220px !important;" runat="server"
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator5" CssClass="multioperators" runat="server" Style="width: 116px !important;">
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div style="width: 240px; float: left;">
                                                        <asp:Panel ID="pnlOperatorValue5" runat="server" CssClass="poperatorvalue">
                                                        </asp:Panel>

                                                        <asp:Panel ID="pnlPickerValueContainer5" runat="server" CssClass="pickervaluec">
                                                            <input type="button" class="input-button-bg" style="height: 17px; padding-top: 1.5px; margin-top: 2px;"
                                                                title="Pick value for this question" value="Pick" onclick="showValuePicker(this, 'selector5');" />
                                                            <asp:Label ID="lblPickedValue5" runat="server" CssClass="pickedvalue"></asp:Label>

                                                            <div class="bottompickerContainer selector5">
                                                                <asp:Panel ID="pnlPickerValuePopup5" runat="server">
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>

                                                        <asp:ImageButton ID="ibtnRemoveQues5" OnClick="btnRemoveQuestion_Click" ImageUrl="/Content/images/Cancel.png" runat="server"
                                                            ToolTip="Remove this question" Style="cursor: pointer; margin-top: -1px;" AlternateText="Remove" />
                                                    </div>
                                                </asp:Panel>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="border: 1px solid #A5A5A5;">
                                            <div style="float: left;">
                                                <dx:ASPxButton ID="btCreateGroup" ClientEnabled="false" Paddings-PaddingLeft="5px" RenderMode="Link" OnClick="btCreateGroup_Click"
                                                    Image-Url="/Content/images/add_icon.png" ClientInstanceName="btCreateGroup" runat="server" Text="Create Group" AutoPostBack="true">
                                                    <ClientSideEvents Click="CreateGroup_click" />
                                                </dx:ASPxButton>
                                                <asp:Button ID="btnBuildExpression" runat="server" Text="Apply" CssClass="default-selector" OnClick="btnBuildExpression_Click" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7">
                                            <asp:TextBox ID="skipLogicExpression" Enabled="false" Width="98%" Height="45px" TextMode="MultiLine" ForeColor="Black"
                                                runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>

                <table width="100%" cellpadding="0" cellspacing="3">
                    <tr>
                        <td colspan="2">
                            <asp:CustomValidator ID="cvcConditionValidator" ValidationGroup="questionGroup"
                                runat="server" ForeColor="Red" Display="Dynamic" ControlToValidate="ddlDefaultValQuestion" ErrorMessage="Please select question for condition."></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="trThen" runat="server">
                        <td colspan="2" style="border-top: 1px solid #A5A5A5;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <%--<Triggers>
                                        <asp:PostBackTrigger ControlID="btAddCondition" />
                                    </Triggers>--%>
                                <ContentTemplate>
                                    <div style="float: left; padding-top: 5px; width: 100%;">
                                        <div>
                                            <span>Then:</span>
                                            <asp:DropDownList ID="ddlIfValQuestion" AutoPostBack="true"
                                                runat="server">
                                            </asp:DropDownList>
                                            <span style="float: right;">
                                                <asp:HiddenField ID="hdnConditionIndex" runat="server" />
                                                <dx:ASPxButton ID="btAddCondition" ClientInstanceName="btAddCondition" CssClass="primary-blueBtn" runat="server" Text="Add Condition" AutoPostBack="false" ValidationGroup="questionGroup">
                                                    <ClientSideEvents Click="btAddCondition_click" />
                                                </dx:ASPxButton>
                                            </span>
                                        </div>
                                        <asp:Panel ID="pIfConstantVal" runat="server">
                                            <asp:TextBox ID="pIfConstantValText" TextMode="MultiLine" CssClass="full-width" runat="server" Visible="false"></asp:TextBox>
                                            <dx:ASPxDateEdit ID="pIfConstantValDate" runat="server" ClientVisible="false"></dx:ASPxDateEdit>
                                            <ugit:UserValueBox ID="pIfConstantValUser" runat="server" isMulti="false" Visible="false" />
                                        </asp:Panel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="ms-formlabel">
                <h3 class="ms-standardheader">Conditions:
                </h3>
            </td>
            <td class="ms-formbody">
                <asp:Repeater ID="rVariableConditions" runat="server" OnItemDataBound="rVariableConditions_ItemDataBound">
                    <ItemTemplate>
                        <div style="border-bottom: 1px solid gray; float: left; width: 100%;">
                            <div class="fleft full-width">
                                <b>IF&nbsp;&nbsp;</b><asp:Label ID="lbCondition" runat="server"></asp:Label><b>&nbsp;&nbsp;Then</b>
                            </div>
                            <div class="fleft full-width">
                                <b>Value:&nbsp;</b><asp:Label ID="lbConditionValue" runat="server"></asp:Label>
                                <span style="float: right;">
                                    <asp:ImageButton ImageUrl="/Content/Images/edit-icon.png" OnClick="btEditVarCondition_Click" ID="btEditVarCondition" runat="server" />
                                </span>
                                <span style="float: right;" onclick="return confirmDeleteCondition();">
                                    <asp:ImageButton ID="btDeleteVarCondition" runat="server" ImageUrl="/Content/images/delete-icon-new.png"
                                        OnClick="btDeleteVarCondition_Click" />
                                </span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>

        <tr>
            <td colspan="2" align="right" style="padding-top: 5px;">
                <span style="float: right;">

                    <dx:ASPxButton ID="btSaveAndNewVariable" ClientInstanceName="btSaveAndNewVariable" runat="server" Text="Save" ValidationGroup="questionGroup" OnClick="btSaveAndNewVariable_Click" CssClass="primary-blueBtn">
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btClose" ClientInstanceName="btClose" runat="server" Text="Close" OnClick="BtClose_Click" CssClass="primary-blueBtn"></dx:ASPxButton>

                    <dx:ASPxButton ID="btSaveConditionPost" ClientInstanceName="btSaveConditionPost" runat="server" ClientVisible="false" OnClick="btAddCondition_Click" ValidationGroup="questionGroup"></dx:ASPxButton>

                </span>
            </td>
        </tr>
    </table>
</div>

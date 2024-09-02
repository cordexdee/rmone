

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceQuestionBranch.ascx.cs" Inherits="uGovernIT.Web.ServiceQuestionBranch" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
 .fleft{float:left;}
.ms-formbody
{
    /*background: none repeat scroll 0 0 #E8EDED;
    border-top: 1px solid #A5A5A5;*/
    padding: 3px 6px 4px;
    vertical-align: top;
}
/*.pctcomplete{ text-align:right;}
.estimatedhours{text-align:right;}
.actualhours{text-align:right;}
.full-width{width:98%;}
.ms-formlabel{width:160px;}
.existing-section-c{float:left;}
.new-section-c{float:left;}
.existing-section-a{float:left;padding:0px 5px;}
.existing-section-a img{cursor:pointer;}
.new-section-a{float:left;padding-left:5px;}
.new-section-a img{cursor:pointer;}
.poperatorvalue
{
    float:left;
    width:100%;
    max-height:100px;
    overflow-y:auto;
    overflow-x:hidden;
}
    .pickervaluec
    {
        position:relative;
    }
    .pickervaluevontainer
    {
        background:#fff;
        float:left;
        width:300px;height:300px;
        display:none;z-index:10000;
        position:absolute;
        overflow-y:auto;
        overflow-x:auto;
        border:1px solid;
        top:29px;
        left:1px;
    }
    .lbpickedvalue
    {
        float:left;
        margin-top:4px;
        min-height:12px;
        max-width:350px;
        margin-left:3px;

    }*/

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

    .poperatorvalue {
        float: left;
        width: 100%;
        max-height: 100px;
        overflow-y: auto;
        overflow-x: hidden;
    }
    
    .panel-align .poperatorvalue {
        float: left;
        width: auto;
        max-height: 100px;
        overflow-y: auto;
        overflow-x: hidden;
    }
    .pickervaluec {
        position: relative;
    }

    .pickedvalue {
        float: left;
        margin-top: 2px;
        min-height: 12px;
        max-width: 350px;
        margin-left: 3px;
    }

    .bottompickerContainer {
        background: #fff;
        float: left;
        width: 275px;
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

    .default-selector {
        cursor: pointer;
    }

    .multi-addbtn {
        margin-left: 5px;
    }

    .remove-group {
        vertical-align: middle;
        right: -14px;
        width: 14px;
        height: 14px;
        cursor: pointer;
        margin-top: -1px;
    }

    .hide-control {
        visibility: hidden;
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
                    else if(questions.length - 1 == selectedQuestions.length){
                            section.find(":checkbox").attr("checked", true);
                    }
                }
            }
        });
    });

    function confirmDelete() {
        var conditionTitle = $(".txttitlediv :text").val();
        if (confirm("Are you sure you want to delete condition: \"" + conditionTitle + "\"?")) {
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
</script>

<dx:ASPxHiddenField ID="hdnSelectedSLogic" runat="server" ClientInstanceName="hdnSelectedSkipLogic"></dx:ASPxHiddenField>
<dx:ASPxButton ID="btRemoveGroup" ClientVisible="false" ForeColor="Red" Paddings-PaddingLeft="5px" CssClass="padding-left5" RenderMode="Link" OnClick="btRemoveGroup_Click" Image-Url="/_layouts/15/Images/uGovernIT/delete-icon.png" ClientInstanceName="btRemoveGroup" runat="server" Text="Remove Group" AutoPostBack="true">
</dx:ASPxButton>

<div style="display:none;">
    <dx:ASPxDateEdit id="tempDate" runat="server"></dx:ASPxDateEdit>
</div>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
     <div class="ms-formtable accomp-popup">
          <div class="row" id="trExistingConditions" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">
                   Skip Conditions<b style="color: Red;">*</b>
                 </h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 <div class="dropDwonList-div">
                     <asp:DropDownList ID="ddlExistingConditions" runat="server" AutoPostBack="true"  CssClass="itsmDropDownList aspxDropDownList fleft" 
                     OnSelectedIndexChanged="DDlExistingConditions_SelectedIndexChanged"></asp:DropDownList>
                 </div>
                 <div class="deleteIcon-div" onclick="return confirmDelete();">
                   <asp:ImageButton ID="btDeleteButton" runat="server" Text="Delete" OnClick="BtDelete_Click" 
                       ImageUrl="/Content/images/redNew_delete.png" BorderWidth="0" Width="16" />
                </div>
             </div>
         </div>
         
         <div class="row" id="trTitle" runat="server" >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">
                     Title<b style="color: Red;">*</b>
                 </h3>
             </div>
             <div class="ms-formbody accomp_inputField txttitlediv">
                 <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="questionGroup"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtTitle"
                     Display="Dynamic" CssClass="error" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
             </div>
         </div>
         <div class="row" id="trCondtion" runat="server" >
             <div class="ms-formlabel section-divide">
                 <h3 class="ms-standardheader budget_fieldLabel">
                     Condition
                 </h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                     <div class="row">
                         <div>
                             <span id="spnQuestion" runat="server" class="hide" style="color:red;">Please select question for condition.</span>
                         </div>
                     </div>
                     <div class="row">
                         <div>
                             <asp:Panel runat="server" ID="questionContainerPanel" CssClass="panel-question" Width="750px">
                                 <table width="750px">
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
                                                            <asp:DropDownList ID="ddlCondition1" runat="server" style="width: 50px !important;" Enabled="false" CssClass="formulafields hide-control">
                                                                <asp:ListItem Text="NONE" Value="NONE" />
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup1" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>
                                                        <asp:DropDownList ID="ddlCQuestions" AutoPostBack="true" CssClass="formulafields" runat="server" style="width: 220px !important;" 
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperators" CssClass="multioperators" runat="server" style="width: 116px !important;">
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
                                                            <asp:DropDownList ID="ddlCondition2" runat="server" style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup2" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>

                                                        <asp:DropDownList ID="ddlQuestion2" AutoPostBack="true" CssClass="formulafields" style="width: 220px !important;" runat="server" 
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator2" CssClass="multioperators" runat="server" style="width: 116px !important;">
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
                                                            <asp:DropDownList ID="ddlCondition3" runat="server" style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup3" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>

                                                        <asp:DropDownList ID="ddlQuestion3" AutoPostBack="true" CssClass="formulafields" style="width: 220px !important;" runat="server" 
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator3" CssClass="multioperators" runat="server" style="width: 116px !important;">
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
                                                            <asp:DropDownList ID="ddlCondition4" runat="server" style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup4" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>

                                                        <asp:DropDownList ID="ddlQuestion4" AutoPostBack="true" CssClass="formulafields" style="width: 220px !important;"  runat="server" 
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator4" CssClass="multioperators" runat="server" style="width: 116px !important;" >
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
                                                            <asp:DropDownList ID="ddlCondition5" runat="server" style="width: 50px !important;" CssClass="formulafields">
                                                                <asp:ListItem Text="AND" Value="AND" />
                                                                <asp:ListItem Text="OR" Value="OR" />
                                                            </asp:DropDownList>
                                                            <asp:ImageButton ID="ibtnRemoveGroup5" runat="server" ImageUrl="/Content/images/indent-dec.png"
                                                                CssClass="hide" ToolTip="Remove Group" AlternateText="" />
                                                        </div>


                                                        <asp:DropDownList ID="ddlQuestion5" AutoPostBack="true" CssClass="formulafields" style="width: 220px !important;" runat="server" 
                                                            OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                                                        </asp:DropDownList>

                                                        <asp:DropDownList ID="ddlOperator5" CssClass="multioperators" runat="server" style="width: 116px !important;">
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
                                        <td colspan="5" style="border:1px solid #A5A5A5;">
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
                         </div>
                     </div>
                     

                 </div>
             </div>
         </div>
         <div class="row" id="trSkipSections" runat="server"  >
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Skip Section/Question</h3>
             </div>
             <div class="ms-formbody accomp_inputField">
                 <span id="spnSections" runat="server" class="hide" style="color:red;">You cannot skip questions which belong to same section of condition's question.</span>
                <asp:CheckBoxList ID="ddlSectionList" runat="server" ValidationGroup="questionGroup"></asp:CheckBoxList>
             </div>
         </div>
          <div class="row  addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxButton id="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btSaveCondition" ValidationGroup="questionGroup" runat="server" Text="Save" CssClass="primary-blueBtn"
                    OnClick="BtSaveCondition_Click"></dx:ASPxButton>
            </div>
         </div>
    </div>
</div>
         
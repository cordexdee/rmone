<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleStagesRule.ascx.cs" Inherits="uGovernIT.Web.ModuleStagesRule" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .saveButton
    {
        display:none;
    }
    .AddBtn {
        background: #4FA1D6;
        border: 1px solid #4FA1D6;
        color: #FFF !important;
        border-radius: 4px;
        padding: 5px 15px;
        margin-top: 25px;
        font-weight:500;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var expressionJsonObject = null;

    $(function () {
        loadExpression();
        onControlLoad();
        $('.drop-column').bind('change', function () {
            if (this != null) {

                var txt = '#constraintValue';
                var chk = '#chkValue';
                var dtc = '#span_datetime';
                var user = '#span_user';
                change_column(this, chk, dtc, txt, user);
            }
        });

        $('#ddlConditionalOperator').bind('change', function () {
            if (this != null) {

                var txt = '#constraintValue';
                var chk = '#chkValue';
                var dtc = '#span_datetime';
                var user = '#span_user';
                hideValueBox(this, chk, dtc, txt, user);
            }
        });

        $(".show-constraint").bind("blur", function () {
            $('#<%=expressionJson_hidden.ClientID%>').val($.trim($(".show-constraint").val()));
        });

    });

    function loadExpression() {
        var expression = $('#<%=expressionJson_hidden.ClientID%>').val();
        var constraint = '';
        $(".show-constraint").html('');
        if (expression != null && expression.length > 0) {
            $("#constraintStartsWith").removeAttr("disabled");
            $(".show-constraint").val(expression);
        }

        else
            $("#constraintStartsWith").attr('disabled', 'disabled');
    }


    function funcAddConstraint(option) {
        if (validate()) {
            var expressionText = $(".show-constraint");
            var dtype = $(".drop-column").find(":selected").attr("datatype");
            var valueObj = $("#constraintValue");
            var valueConstraint = valueObj.val().trim();
            var chk = $('#chkValue');
            var peoplePickerId = '<%=ppeValue.UserTokenBoxAdd.ClientID%>';
            var peoplePicker = ASPxClientControl.GetControlCollection().GetByName(peoplePickerId);//$("#" + peoplePickerId);
            var userVal = peoplePicker.GetText();
            var operatorDropDown = $('#ddlConditionalOperator');
            var customDate = $("#customDate").val();
            var expressionJson = '';
            var leftOperand = $("#<%=ddlField.ClientID%>").val();
            var appendWith = $("#constraintStartsWith").val();
            var constraint = '';
            var booleanValue = 'false';

            if (chk.is(':checked'))
                booleanValue = 'true';

            if ($.trim($(".show-constraint").val()) != '') {
                $("#constraintStartsWith").removeAttr('disabled');
            }
            else {
                $("#constraintStartsWith").attr('disabled', 'disabled');
            }

            if (operatorDropDown.val() != 'Null' && operatorDropDown.val() != 'NotNull') {
                if (dtype == "bit" || dtype == "boolean") {
                    if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                        constraint = "[" + leftOperand + "]" + " " + operatorDropDown.val() + " '" + booleanValue + "' ";
                        expressionJson += constraint;
                    }
                    else {
                        constraint = appendWith + " " + "[" + leftOperand + "]" + " " + operatorDropDown.val() + " '" + booleanValue + "' ";
                        expressionJson += constraint;
                    }
                }

                if (dtype == "nvarchar" || dtype == "varchar" || dtype == "string" ) {
                    if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                        constraint = "[" + leftOperand + "]" + " " + operatorDropDown.val() + " " + " '" + valueConstraint + "'" + " ";
                        expressionJson += constraint;
                    }
                    else {
                        constraint = appendWith + " " + "[" + leftOperand + "]" + " " + operatorDropDown.val() + " '" + valueConstraint + "'" + " ";
                        expressionJson += constraint;
                    }
                }

                if (dtype == "Double" || dtype == "int" || dtype == "bigint" || dtype == "float" || dtype == "decimal" || dtype == "Currency") {
                    if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                        constraint = "[" + leftOperand + "]" + " " + operatorDropDown.val() + " " + valueConstraint + " ";
                        expressionJson += constraint;
                    }
                    else {
                        constraint = appendWith + " " + "[" + leftOperand + "]" + " " + operatorDropDown.val() + " "+ valueConstraint  + " ";
                        expressionJson += constraint;
                    }
                }
                if (dtype == "datetime" || dtype == "date") {
                    if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                        constraint = "[" + leftOperand + "]" + " " + operatorDropDown.val() + " " + customDate + " ";
                        expressionJson += constraint;
                    }
                    else {
                        constraint = appendWith + " " + "[" + leftOperand + "]" + " " + operatorDropDown.val() + " " + customDate + " ";
                        expressionJson += constraint;
                    }
                }

                if (dtype == "user") {
                    if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                        constraint = "[" + leftOperand + "]" + " " + operatorDropDown.val() + " '" + userVal + "'" + " ";
                        expressionJson += constraint;
                    }
                    else {
                        constraint = appendWith + " " + "[" + leftOperand + "]" + " " + operatorDropDown.val() + " '" + userVal + "'" + " ";
                        expressionJson += constraint;
                    }
                }
            }
            else if (operatorDropDown.val() == 'Null') {
                if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                    constraint = "[" + leftOperand + "]" + " = 'null' ";
                    expressionJson += constraint;
                }
                else {
                    constraint = appendWith + " " + "[" + leftOperand + "]" + " = 'null' ";
                    expressionJson += constraint;
                }
            }
            else if (operatorDropDown.val() == 'NotNull') {
                if ($("#constraintStartsWith").attr('disabled') == 'disabled' || $("#constraintStartsWith").attr('disabled') == true) {
                    constraint = "[" + leftOperand + "]" + " <> 'null' ";
                    expressionJson += constraint;
                }
                else {
                    constraint = appendWith + " " + "[" + leftOperand + "]" + " != 'null' ";
                    expressionJson += constraint;
                }
            }
            expressionText.val(expressionText.val() + " " + expressionJson);
            $('#<%=expressionJson_hidden.ClientID%>').val(expressionText.val());
            $("#constraintStartsWith").removeAttr('disabled');
        }
    }

    function setMessage(message, color, removeAfter) {
        var valueObj = $("#constraintValue");
        $("#lbl_Value").addClass("message-container");
        $("#lbl_Value").css("visibility", "visible")
        $("#lbl_Value").html(message);
        if (color == undefined || color == null || color == "") {
            color = "blue";
        }
        $("#lbl_Value").css("color", color);
        if (removeAfter != undefined && removeAfter != null && removeAfter > 0 && removeAfter < 10) {
            setTimeout("removeMessage()", removeAfter * 1000);
        }
        valueObj.val('');
        valueObj.focus();
    }

    function removeMessage() {
        $("#lbl_Value").css("visibility", "hidden")
        $("#lbl_Value").removeClass("message-container");
        $("#lbl_Value").html("");
    }

    function showError(message) {
        var valueObj = $("#constraintValue");
        $("#lbl_Value").css("visibility", "visible")
        $("#lbl_Value").text(message);
        setTimeout("javascript:document.getElementById('lbl_Value').style.visibility='hidden';", 2000);
    }

    function validate() {
       
        var peoplePickerId = '<%=ppeValue.UserTokenBoxAdd.ClientID%>';
        var peoplePicker =ASPxClientControl.GetControlCollection().GetByName(peoplePickerId);
        var peoplepickerValue = peoplePicker.GetText();
        var operatorDropDown = $('#ddlConditionalOperator');
        var dtype = $(".drop-column").find(":selected").attr("datatype");
        var valueObj = $("#constraintValue");
        var valueConstraint = valueObj.val().trim();
        var customDate = $("#customDate").val();
       
        if (operatorDropDown.val() != 'Null' && operatorDropDown.val() != 'NotNull') {
            if (dtype != "datetime" && dtype != "String" && dtype != "user" && dtype != "boolean" && dtype != "bit") {
                if (valueConstraint == '') {
                    setMessage("Please enter a value", 'red', 2);
                    return false;
                }
                if ((dtype == "decimal" || dtype == "Currency" || dtype == "int" || dtype == "bigint" || dtype == "double" ||  dtype == "float") && isNaN(valueConstraint)) {
                    setMessage("Entered Value is not a number", 'red', 2);
                    return false;
                }
            }
            if (dtype == "datetime" || dtype == "date") {
                if ($.trim(customDate) == '') {
                    setMessage("Please enter a value.", 'red', 2);
                    return false;
                }
            }

            if (dtype == "user") {

                if (peoplepickerValue == null || peoplepickerValue == "") {
                    setMessage("Please enter a user.", 'red', 2);
                    return false;
                }
            }
        }
        return true;
    }


    function onControlLoad() {
        var txt = '#constraintValue';
        var chk = '#chkValue';
        var dtc = '#span_datetime';
        var user = '#span_user';
        var operatorDropDown = $('#ddlConditionalOperator').get(0);
        var sourceDropDown = $(".drop-column").get(0);
        change_column(sourceDropDown, chk, dtc, txt, user);
        hideValueBox(operatorDropDown, chk, dtc, txt, user);
    }

    function hideValueBox(drpcolumn, chk, dtc, txt, user) {
        var helprow = $("#dateHelp");
        var data = $(drpcolumn).val();
        if (data == 'Null' || data == 'NotNull') {
            $(txt).hide();
            $(user).hide();
            $(chk).hide();
            $(dtc).hide();
            $(".constraintValue-section").css('display', 'none');
            helprow.hide();
        }
        else {
            $(".constraintValue-section").removeAttr('display');
            change_column($(".drop-column").get(0), chk, dtc, txt, user);
        }
    }

    function change_column(drpcolumn, chk, dtc, txt, user) {
      
        $(txt).hide();
        $(user).hide();
        $(chk).hide();
        $(dtc).hide();
        var operatorDropDown = $('#ddlConditionalOperator');
        var helprow = $("#dateHelp");
        helprow.hide();
        var selectedcolumn = $(drpcolumn).val();
        var datatype = $($(drpcolumn).find(":selected")).attr('datatype');
        if (operatorDropDown.val() != 'NotNull' && operatorDropDown.val() != 'Null') {
            switch (datatype) {
                case 'datetime':
                    $(dtc).show();
                    helprow.show();
                    break;
                case 'bit':
                    $(chk).show();
                    break;
                case 'boolean':
                    $(chk).show();
                    break;
                case 'user':
                    $(user).show();
                    break;
                default:
                    $(txt).show();
                    break;
            }
        }

    }

    function validateExpression(flag) {
       
        var dataType = '';
        var expression = $('#<%=expressionJson_hidden.ClientID%>').val();
        if ($.trim(expression) == '') {
          
            if (flag)
                $(".saveButton").trigger('click');
            else
                setMessage("Expression is empty!", 'blue', 2);

            return true;
        }
        var fieldData = $.parseJSON($('#<%=fieldDataJson.ClientID%>').val());
        var patt = new RegExp('\\[.+?]', "g");
        var matches = expression.match(patt);
        if (matches != null && matches.length > 0) {
            for (var index = 0; index < matches.length ; index++) {
                for (var fieldIndex = 0 ; fieldIndex < fieldData.length ; fieldIndex++) {
                    if ("[" + fieldData[fieldIndex].FieldDisplayName + "]" == matches[index]) {
                        dataType = fieldData[fieldIndex].DataType;
                        break;
                    }
                    else if (("[" + fieldData[fieldIndex].FieldName + "]" == matches[index]))
                    {
                        dataType = fieldData[fieldIndex].DataType;
                        break;
                    }
                }

                if (dataType != null && dataType != '') {
                   
                    switch (dataType.toLowerCase()) {
                        case 'datetime':
                            if (matches[index] != '[Today]')
                                expression = expression.replace(matches[index], "12-03-2013");
                            break;
                        case 'boolean':
                            expression = expression.replace(matches[index], "'true'");
                            break;
                              case 'bit':
                            expression = expression.replace(matches[index], "'true'");
                            break;
                        case 'string':
                            expression = expression.replace(matches[index], "'Hello'");
                            break;
                        case 'int':
                        case 'bigint':
                        case 'smallint':
                        case 'double':
                        case 'float':
                         case 'decimal':
                        case 'currency':
                            expression = expression.replace(matches[index], "1");
                            break;
                        case 'user':
                            expression = expression.replace(matches[index], "'system\\user'");
                            break;
                        default:
                            break;
                    }

                }
            }
        }
       
            var dataVar = "{ 'name' : \"" + escape(expression) + "\"}";
            var ajaxUrl = "<%= ajaxPageURL %>/ValidateRuleExpression";
            $.ajax({
                type: "POST",
                url: ajaxUrl,
                data: dataVar,
                async:false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (message) {                 
                    var resultJson = $.parseJSON(message);
                    if (resultJson.messagecode == 0) {                     
                        setMessage(resultJson.message, 'red',12);                       
                    }
                    else if (resultJson.messagecode == 1) {
                        setMessage(resultJson.message, 'blue', 2);
                        if (flag)                            
                        $(".saveButton").trigger('click');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {                   
                    setMessage("Error Occurred", 'red', 12); 
                }
            });
            return false;
    }
</script>

<asp:HiddenField runat="server" ID="expressionJson_hidden" />
<asp:HiddenField runat="server" ID="fieldDataJson" />

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup">
        <div class="row" id="trModules" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" AutoPostBack="true" OnSelectedIndexChanged="ddlModule_SelectedIndexChanged"
                    runat="server" CssClass="itsmDropDownList aspxDropDownList">
                </asp:DropDownList>
                <asp:Label ID="lbModule" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row" id="trStage" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Stage<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModuleStep" runat="server" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                    OnSelectedIndexChanged="ddlModuleStep_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="lbStage" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <div class="row" id="trExpression" runat="server">
            <div class="ms-formlabel mt-2 mb-2 ml-1">
                <h3 class="ms-standardheader budget_fieldLabel" style="font-weight:500;">Create Expression</h3>
            </div>
            <div class="expressionTool ms-formbody accomp_inputField">
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="col-md-3 col-sm-3 col-xs-3 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Operator</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <select id="constraintStartsWith" style="width: 90px;">
                            <option value="&&">AND</option>
                            <option value="||">OR</option>
                        </select>
                    </div>
                </div>
                    <div class="col-md-9 col-sm-9 col-xs-9 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Source</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlField" CssClass="drop-column itsmDropDownList aspxDropDownList" runat="server"
                            CausesValidation="false" AutoPostBack="false">
                        </asp:DropDownList>
                    </div>
                </div>
                </div>
                <div class="col-md-2 col-sm-2 col-xs-2 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Operator</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <select id="ddlConditionalOperator" style="width: 90px;">
                            <option value="=">Equal to</option>
                            <option value="<>">Not equal To</option>
                            <option value="<">Less than</option>
                            <option value="<=">Less than or equal to</option>
                            <option value=">">Greater than</option>
                            <option value=">=">Greater than or equal to</option>
                            <option value="Null">Is Null</option>
                            <option value="NotNull">Is Not Null</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-3 col-sm-3 col-xs-3 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Value</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <input id="constraintValue" type="text" />
                        <input id="chkValue" type="checkbox" style="display: none; width: 200px;" checked="checked" />
                        <span id="span_user" style="display: none; width: 200px;">
                            <ugit:UserValueBox ID="ppeValue" Width="200px" CssClass="peAssignedTo userPicker" SelectionSet="User" runat="server" isMulti="false" />
                        </span>
                        <span id="span_datetime" style="display: none; width: 200px;">
                            <input type="text" id="customDate" style="width: 200px;">
                        </span>
                    </div>
                </div>
                <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                    <input id="Button1" type="button" class="AddBtn" value="Add" onclick="funcAddConstraint()" />
                </div>
            </div>
            <div style="display: none; margin-right:6px;" id="dateHelp">
                <div colspan="6">
                    <div style="float: right"><b style="color: Gray; float: left; padding-top: 1px;">Add Days using f:adddays([Today],2)</b></div>
                </div>
            </div>
            <div colspan="4">
                <div style="float: left">
                    <label id="lbl_Value" style="float: right; visibility: hidden; color: red" class="error"></label>
                </div>
            </div>
                    </div>

            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Expression</h3>
                </div>

                <div class="showConstraintSection">
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtStageRule" runat="server" TextMode="MultiLine" Height="30" CssClass="show-constraint"></asp:TextBox>
                    </div>
                </div>
            </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="row addEditPopup-btnWrap">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
                <dx:ASPxButton ID="saveRule" runat="server" CssClass="primary-blueBtn saveButton" Text="Save" ToolTip="Save" ValidationGroup="Rule" OnClick="btSaveRule_Click">
                    <ClientSideEvents Click="function(s, e){return validateExpression(true);}" />
                </dx:ASPxButton>
                <%--<dx:ASPxButton ID="Button2" ValidationGroup="Rule" CssClass="primary-blueBtn" OnClick="btSaveRule_Click" runat="server" Text="Save"></dx:ASPxButton>--%>
                <dx:ASPxButton ID="validateButton" runat="server" CssClass="primary-blueBtn" ValidationGroup="Rule" Text="Validate" ToolTip="Validate Expression">
                    <ClientSideEvents Click="function(s, e){return validateExpression(false);}" />
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>




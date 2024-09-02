<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportExcel.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.ImportExcel" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<style type="text/css">
    .style1
    {
        width: 120px;
    }
   
    .labelStyle
    {
        font-weight: bold;
    }
    .style3
    {
        width: 112px;
    }
    .style4
    {
        width: 116px;
    }
    .style6
    {
        width: 493px;
    }
</style>


<script language="javascript" type="text/javascript">

    var action;
    var operation;
    var pageName;
    var newList;
    var fileName;
    var listName;
    var listType;
    var listOperation;
    $(document).ready(function () {
      
        <%--var flag = '<%= flag %>';
        pageName = '<%= pageName %>';

        if (pageName == "ITGBudgetManagement" || pageName == "PMMBudget") {
            $("#dialog").css("display", 'none');
            $("#uploadExcelDiv").css("display", 'block');
            $("#<%= UploadButton.ClientID %>").css("display", 'block');
            $("#maindiv").css("display", 'none');
            $("#workFlow").css("display", 'none');
            action = 'oldList';
            operation = 'rePopulate';


            document.getElementById('<%= operationToBePerformed.ClientID %>').value = operation;--%>
        //}
        
            
        
        //if (flag == "") {
        //}
        //else {
        //    alert('Action status:' + flag);
        //    flag = "";
        //}
    });

    function nextFrom_FirstHalt() {
        action = $("#action input:radio:checked").val();
        document.getElementById('<%= operationToBePerformed.ClientID %>').value = action;
        if (action == "newList") {
            $("#newListNameDiv").css("display", 'block');
            listType = "New List"

        }

        else {
            listType = "Old List"
            $("#dropDownDiv").css("display", 'block');
            $("#operationsDialog").css("display", 'block');
        }
        $("#dialog").css("display", 'none');
        $("#btBack").attr("disabled", false);
        $("#step1Div").removeClass("arrow_active").addClass("arrow");
        $("#step2Div").removeClass("arrow").addClass("arrow_active");
    }

    function nextFrom_SecondHalt() {
        operation = $("#operation input:radio:checked").val();
        document.getElementById('<%= operationToBePerformed.ClientID %>').value = operation;
        if (operation == "rePopulate")
            listOperation = "Repopulate";
        if (operation == "append")
            listOperation = "Append To List";
        if (operation == "delete")
            listOperation = "Delete and Create";
        listName = document.getElementById('<%= operationsDropDown.ClientID%>').value
        $("#dropDownDiv").css("display", 'none');
        $("#operationsDialog").css("display", 'none');
        $("#uploadExcelDiv").css("display", 'block');
        $("#step2Div").removeClass("arrow_active").addClass("arrow");
        $("#step3Div").removeClass("arrow").addClass("arrow_active");
    }

    function nextFrom_ThirdHalt() {
        if (Validate()) {
          
            var uploadedFile = document.getElementById('<%=FileUploadControl.ClientID%>').value;
            var indexFrom = uploadedFile.lastIndexOf("\\");
            var to = uploadedFile.length;
            fileName = uploadedFile.substring(indexFrom, to);
            $("#uploadExcelDiv").css("display", 'none');

            $("#summaryDiv").css("display", 'block');
            $("#btConfirm").css("display", 'block');
             $("#btNext").css("display", false);
            $("#btNext").attr("disabled", true);
            $("#step3Div").removeClass("arrow_active").addClass("arrow");
            $("#step4Div").removeClass("arrow").addClass("arrow_active");
            var htmlString = "<tr><td><b>Are you sure to import file?</b></td></tr>";
            htmlString+= "<tr> <td><b> List type:</b> " + listType + " </td> </tr>";
            htmlString += "<tr> <td><b> List Name:</b> " + listName + " </td> </tr>";
            htmlString += "<tr> <td> <b>List Operation:</b> " + listOperation + " </td> </tr>";
            htmlString += "<tr> <td><b> File Name:</b> " + fileName + " </td> </tr>";
           
         
            $("#summaryTable").html(htmlString);
        }

    }

    function nextFrom_NewHalt() {
        if (checkName()) {
            newList = $("#newListName").val();
            listName = newList;
            document.getElementById('<%= newListTitle.ClientID %>').value = newList;
            $("#uploadExcelDiv").css("display", 'block');
            $("#newListNameDiv").css("display", 'none');
            listOperation ="Create New"
            $("#step2Div").removeClass("arrow_active").addClass("arrow");
            $("#step3Div").removeClass("arrow").addClass("arrow_active");
            
            
        }
    }

    function checkName() {
        if ($("#newListName").val().length == 0) {
            alert("Please enter list Name");
            return false;
        }
        re = /^[A-Za-z]+$/
        if (!re.test($("#newListName").val())) {

            alert('Enter alphabets only');

            return false;
        }
        return true;
    }
    function backFrom_newHalt() {
        $("#dialog").css("display", 'block');
        $("#newListNameDiv").css("display", 'none');
        $("#btBack").attr("disabled", true);
        $("#step2Div").removeClass("arrow_active").addClass("arrow");
        $("#step1Div").removeClass("arrow").addClass("arrow_active");
    }

    function backFrom_FourthHalt() {
        action = $("#action input:radio:checked").val();

        $("#btConfirm").css("display", 'none');
        $("#summaryDiv").css("display", 'none');
        $("#btNext").attr("disabled", false);
        if (pageName == "ITGBudgetManagement") {
            $("#step2Div").removeClass("arrow_active").addClass("arrow");
            $("#step1Div").removeClass("arrow").addClass("arrow_active");
            $("#operationsDialog").css("display", 'block');
            $("#btBack").css("display", 'none');

        }
        else {
            $("#step4Div").removeClass("arrow_active").addClass("arrow");
            $("#step3Div").removeClass("arrow").addClass("arrow_active");
            $("#uploadExcelDiv").css("display", 'block');
        }
    }

    function backFrom_ThirdhHalt() {
        $("#uploadExcelDiv").css("display", 'none');
        document.getElementById('<%=FileUploadControl.ClientID%>').value = "";
        if (action == "newList") {
            $("#newListNameDiv").css("display", 'block');
        }
        else if (action == "oldList") {
            $("#dropDownDiv").css("display", 'block');
            $("#operationsDialog").css("display", 'block');
        }
        $("#step3Div").removeClass("arrow_active").addClass("arrow");
        $("#step2Div").removeClass("arrow").addClass("arrow_active");

    }

    function backFrom_SecondhHalt() {

        if (action == "newList") {
            $("#newListNameDiv").css("display", 'none');
        }
        else if (action == "oldList") {
            $("#dropDownDiv").css("display", 'none');
            $("#operationsDialog").css("display", 'none');
        }
        $("#dialog").css("display", 'block');
        $("#step2Div").removeClass("arrow_active").addClass("arrow");
        $("#step1Div").removeClass("arrow").addClass("arrow_active");
        $("#btBack").attr("disabled", true);
        $("#step2Div").removeClass("arrow_active").addClass("arrow");
        $("#step1Div").removeClass("arrow").addClass("arrow_active");
    }

    function validateExtension(value) {
        var allowedExtensions = new Array("xls", "xlsx");
        for (var ct = 0; ct < allowedExtensions.length; ct++) {
            ext = value.lastIndexOf(allowedExtensions[ct]);
            if (ext != -1) { return true; }
        }
        return false;
    }

    function Validate() {
     
        var uploadControl = document.getElementById('<%=FileUploadControl.ClientID%>').value;
        if (uploadControl.length > 0)
            flag = true;
        else {
            alert("Please select a file.");
            return false;
        }
        if (validateExtension(uploadControl)) {
            return true;
        } else {
            alert("Upload status: Only Excel files are accepted!");
            flag = false;
        }

        return flag;
    }

    function wizardAction(value) {

        $("#btBack").css("display", 'block');
        var halt = "first";
        if (document.getElementById("dialog").style.display == "block")
            halt = "first";
        else if ((document.getElementById("dropDownDiv").style.display == "block") || (document.getElementById("operationsDialog").style.display == "block"))
            halt = "second";

        else if (document.getElementById("uploadExcelDiv").style.display == "block")
            halt = "third";
        else if (document.getElementById("newListNameDiv").style.display == "block")
            halt = "new";
        else if (document.getElementById("summaryDiv").style.display == "block")
            halt = "fourth";

        if (value == "next") {
            switch (halt) {
                case "first":
                    nextFrom_FirstHalt();

                    break;
                case "second":
                    nextFrom_SecondHalt();

                    break;

                case "third":
                    nextFrom_ThirdHalt();
                    break;

                case "fourth":
                    break;

                case "new":
                    nextFrom_NewHalt();
                    break;

                default:
                    showDropdown();
            }
        }
        else if (value == "back") {

            switch (halt) {
                case "first":

                    break;
                case "second":
                    backFrom_SecondhHalt();

                    break;
                case "third":
                    backFrom_ThirdhHalt();

                    break;
                case "fourth":
                    backFrom_FourthHalt();


                    break;
                case "new":
                    backFrom_newHalt()

                    break;
                default:

            }
        }
    }
   
</script>
<div id='workFlow'>
    <table width="100%" style="border-collapse: collapse; padding: 0px; display:none" cellspacing="0"
        cellpadding="0">
        <tr>
            <td class="arrow_active" id="step1Div">
                <div>
                    <asp:Label runat="server" ID="Step1Header">1</asp:Label>
                    <strong></strong>
                </div>
            </td>
            <td class="arrow" id="step2Div">
                <div>
                    <em></em>
                    <asp:Label runat="server" ID="Step2Header">2</asp:Label>
                    <strong></strong>
                </div>
            </td>
            <td class="arrow" id="step3Div">
                <div>
                    <em></em>
                    <asp:Label runat="server" ID="Step3Header">3</asp:Label>
                    <strong></strong>
                </div>
            </td>
            <td class="arrow" id="step4Div">
                <div>
                    <em></em>
                    <asp:Label runat="server" ID="Step4Header">4</asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr valign="middle" class="selected_background" style="text-align: center;">
            <td colspan="4" style="height: 20px">
                <asp:Label runat="server" ID="stepHeading" Font-Bold="true"></asp:Label>
            </td>
        </tr>
    </table>
</div>
<div id="dialog" class="web_dialog" style="display: none; position: relative; top: 18px;
    left: 3px; width: 793px;">
    <table style="border-style: none; border-color: inherit; border-width: 0px; width: 49%;"
        cellpadding="3" cellspacing="0">
        <tr>
            <td style="font-weight: bold" class="style1">
                Choose list type:
            </td>
            <td style="padding-left: 15px;">
                <div id="action">
                    <input id="action1" name="userAction" type="radio" value="newList" checked="checked" />
                    Create a new List
                    <input id="action2" name="userAction" type="radio" value="oldList" />
                    Choose a List
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="newListNameDiv" style="display: none; position: relative; top: 18px; left: 0px;
    width: 551px; height: 28px;">
    <table>
        <tr>
            <td style="font-weight: bold ;padding-right: 15px">
                Enter List Name:
            </td>
            <td>
                <input type='text' id='newListName' />
            </td>
        </tr>
    </table>
</div>
<div id="dropDownDiv" style="display: none; position: relative; top: 18px; left: 2px;
    width: 548px; height: 27px;">
    <table style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
        <tr>
            <td style="font-weight: bold" class="style3">
                Choose list:
            </td>
            <td>
                <asp:DropDownList ID="operationsDropDown" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
</div>
<div id="operationsDialog" class="web_dialog" style="display: none; position: relative;
    top: 31px; left: 0px; width: 552px; height: 30px;">
    <table style="width: 100%; border: 0px;" cellpadding="3" cellspacing="0">
        <tr>
            <td style="font-weight: bold" class="style4">
                Choose Action:
            </td>
            <td style="padding-left: 0px;">
                <div id="operation">
                    <input id="operation1" name="userOperation" type="radio" value="append" checked="checked" />
                    Append to List
                    <input id="operation2" name="userOperation" type="radio" value="rePopulate" />
                    Repopulate
                    <input id="operation3" name="userOperation" type="radio" value="delete" />
                    Delete And Create Again
                </div>
            </td>
        </tr>
    </table>
</div>
<div id="uploadExcelDiv" class="" style="display: block; position: relative; top: 16px;
    left: 0px; width: 794px; height: 50px;">
    
     <asp:Label runat="server" ID="Label3" Text="Please choose a .xls or .xlsx file to import " />
   <br />
   <br />
     <table style="border-collapse: collapse; padding: 0px;" cellspacing="0"
        cellpadding="0">
    <tr>
    <td>
    <asp:Label runat="server" ID="Label1" Text="Upload file: " CssClass="labelStyle" /></td>
    <td>

    <asp:FileUpload ID="FileUploadControl" runat="server" />
    </td>
    <td>
    <asp:Button runat="server" ID="UploadButton" Style="display: block" Text="Import File"
        OnClientClick="return Validate()" OnClick="UploadButton_Click" />
    </td>
    </tr>
    </table>
   
</div>
<div id="maindiv" style="display: none; position: absolute; top: 175px; left: 25px;
    width: 557px; height: 45px;">
    <table style="height: 63px; width: 558px" cellspacing="5">
        <tr>
            <td align="right" class="style6" >
                <input type="button" id="btBack" value="Back" style="display:none;z-index:2"  onclick="wizardAction('back')" />
                </td>
                <td align="right" >
                <input type="button" id="btNext" value="Next"   onclick="wizardAction('next')" />
            </td>
        </tr>
    </table>
</div>
<div id="summaryDiv" style="display: none; position: relative; top: 20px; left: 8px;
    width: 550px; height: 35px;">
    <table id="summaryTable" width="250px" style="border-collapse: collapse; padding: 0px;" cellspacing="5"
        cellpadding="5">
       <tr>
       <td></td>
       </tr>
    </table>
    <br />

     <table width ="558px" style="border-collapse: collapse; padding: 0px;" cellspacing="5"
        cellpadding="5";>
        <tr>
        <td align="right">
         <asp:Button ID='btConfirm' text='Import' runat='server' OnClick='btConfirm_Click' style="position:relative; top:-31px; left:12px"/>
        </td>
        </tr>
        </table>
</div>
<asp:HiddenField ID="operationToBePerformed" runat="server" Value="" />
<asp:HiddenField ID="newListTitle" runat="server" Value="" />
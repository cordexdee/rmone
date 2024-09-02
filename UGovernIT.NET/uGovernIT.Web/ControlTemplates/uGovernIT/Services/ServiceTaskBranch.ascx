

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceTaskBranch.ascx.cs" Inherits="uGovernIT.Web.ServiceTaskBranch" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
 .fleft{float:left;}
.ms-formbody
{
    background: none repeat scroll 0 0 #E8EDED;
    border-top: 1px solid #A5A5A5;
    padding: 3px 6px 4px;
    vertical-align: top;
}
.pctcomplete{ text-align:right;}
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
        width:350px;
        margin-left:3px;

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
        ddlLogicalOpr.removeAttr("disabled");
    }

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
        var operatorCtr = $(".coperators");

        var checkboxes = $(".pickervaluevontainer input:checkbox");
        if (checkboxes.length > 1) {
            checkboxes.bind("click", function () {
                if (operatorCtr.val() == "=" || operatorCtr.val() == "!=") {
                    var selectedCheckboxes = checkboxes.filter(":checked");
                    selectedCheckboxes.removeAttr("checked");
                    $(this).attr("checked", true);
                }
                var selectedCheckboxes = checkboxes.filter(":checked");
                var values = new Array();

                $.each(selectedCheckboxes, function (i, item) {
                    values.push($(item).parent().find("label").text());
                });
                $(".lbpickedvalue").html(values.join(", "));

            });
        }

        $(".coperators").bind("change", function () {
            if ($(this).val() == "=" || $(this).val() == "!=") {
                var checkboxes1 = $(".pickervaluevontainer input:checkbox");
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
                    $(".lbpickedvalue").html(values.join(", "));
                }
            }
        });
    });

    function showValuePicker(obj) {

        if ($(obj).val() == "Done") {
            $(".pickervaluevontainer").hide();
            $(obj).val("Pick");
        }
        else {
            $(obj).val("Done");
            $(".pickervaluevontainer").show();
        }

    }

</script>

<div style="float: left; width: 98%; padding-left: 10px;">
     <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
         width="100%">
          <tr id="trExistingConditions" runat="server" >
             <td class="ms-formlabel">
                 <h3 class="ms-standardheader">
                   Skip Conditions<b style="color: Red;">*</b>
                 </h3>
             </td>
             <td class="ms-formbody">
                 <asp:DropDownList ID="ddlExistingConditions" runat="server" CssClass="fleft" AutoPostBack="true" OnSelectedIndexChanged="DDlExistingConditions_SelectedIndexChanged"></asp:DropDownList>
                  <span onclick="return confirmDelete();" style="float:left;">
                      <asp:ImageButton ID="btDeleteButton" runat="server" OnClick="BtDelete_Click" 
                       ImageUrl="/content/images/delete-icon.png" BorderWidth="0" />
                </span>
             </td>
         </tr>
         
         <tr id="trTitle" runat="server" >
             <td class="ms-formlabel">
                 <h3 class="ms-standardheader">
                     Title<b style="color: Red;">*</b>
                 </h3>
             </td>
             <td class="ms-formbody txttitlediv">
                 <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="questionGroup"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtTitle"
                     Display="Dynamic" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>
             </td>
         </tr>
          <tr id="trCondtion" runat="server" >
             <td class="ms-formlabel">
                 <h3 class="ms-standardheader">
                     Condition
                 </h3>
             </td>
             <td class="ms-formbody">
                 <table width="100%" cellpadding="0" cellspacing="2">
                     <tr>
                         <td colspan="2">
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="questionGroup" ControlToValidate="ddlCQuestions"
                             Display="Dynamic" ErrorMessage="Please select question for condition."></asp:RequiredFieldValidator>
                         </td>
                     </tr>
                  <tr>
                  <td>Question:</td>
                  <td width="100%">
                      <asp:DropDownList ID="ddlCQuestions" AutoPostBack="true" CssClass="cquestions" Width="100%" runat="server" OnSelectedIndexChanged="DDLCQuestions_SelectedIndexChanged" ></asp:DropDownList></td>
                  </tr>
                    <tr>
                        <td>Operator:</td>
                        <td>
                            <asp:DropDownList ID="ddlOperators" CssClass="coperators" runat="server" Width="100">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Value(s):</td>
                        <td>
                        <asp:Panel id="pOperatorValue" runat="server" CssClass="poperatorvalue">
                              
                        </asp:Panel>
                            
                            <asp:Panel id="pPickerValueContainer" runat="server" CssClass="pickervaluec">
                              <input type="button" class="input-button-bg" style="margin-top:6px;" value="Pick" onclick="showValuePicker(this);" />
                              <asp:Label ID="lbPickedValue" runat="server" CssClass="lbpickedvalue"></asp:Label>
                             
                              <div class="pickervaluevontainer">
                                    <asp:Panel ID="pPickerValuePopup" runat="server">
                                    </asp:Panel>
                              </div>
                            </asp:Panel>
                        </td>
                     </tr>
                 </table>
             </td>
         </tr>

         <tr id="trSkipSections" runat="server" visible="false" >
             <td class="ms-formlabel">
                 <h3 class="ms-standardheader">
                    Skip Section
                 </h3>
             </td>
             <td class="ms-formbody">
                <asp:DropDownList ID="ddlSections" runat="server" Width="200" ></asp:DropDownList>
             </td>
         </tr>

         <tr id="trSkipTasks" runat="server"  visible="false">
             <td class="ms-formlabel">
                 <h3 class="ms-standardheader">
                    Skip tasks/tickets
                 </h3>
             </td>
             <td class="ms-formbody">
                <asp:CheckBoxList ID="cbltaskList" runat="server"></asp:CheckBoxList>
             </td>
         </tr>

         <tr>
            <td colspan="2" align="right" style="padding-top: 5px;">
               
                    <dx:ASPxButton ID="btSaveCondition" runat="server" ValidationGroup="questionGroup" Text="Save" OnClick="BtSaveCondition_Click">
                    </dx:ASPxButton>
 
                    <dx:ASPxButton ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" >
                       
                    </dx:ASPxButton>

            </td>
         </tr>
</table>
</div>
       
﻿
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMIssuesNew.ascx.cs" Inherits="uGovernIT.Web.PMMIssuesNew" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
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
          else if (statusObj.val() != "Completed" && pctComplete >= 100) {
              pctCompleteObj.val("90");
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
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
        $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
    }); 
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding fetch-popupParent">
    <asp:Panel runat="server" ID="taskForm">
        <div class="ms-formtable accomp-popup">
            <div class="row" id="trTitle" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Issue<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="Task"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="error" ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                            Display="Dynamic" ErrorMessage="Please enter title."></asp:RequiredFieldValidator>

                        <asp:Label ID="lbTitle" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>            
                <div class="col-md-6 col-sm-6 col-xs-12" id="trStatus" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Status</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Text="Not Started" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="In Progress"></asp:ListItem>
                            <asp:ListItem Text="Completed"></asp:ListItem>
                            <asp:ListItem Text="Deferred"></asp:ListItem>
                            <asp:ListItem Text="Waiting on someone else"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbStatus" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row" id="trPctComplete" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">% Complete</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtPctComplete" Text="0" ValidationGroup="Task" CssClass="pctcomplete" runat="server"></asp:TextBox>
                        <asp:Label ID="lbPctComplete" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12" id="trAssignedTo" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Assigned To</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:UserValueBox ID="peAssignedTo" runat="server" CssClass="userValueBox-dropDown"/>
                        <asp:Label  ID="lbAssignedTo" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row" id="trPriority" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Priority</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Text="High"></asp:ListItem>
                            <asp:ListItem Text="Normal"></asp:ListItem>
                            <asp:ListItem Text="Low"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbPripority" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12" id="trIssueImpact" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Impact</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlIssueImpact" runat="server" CssClass="itsmDropDownList aspxDropDownList">
                            <asp:ListItem Text="Project"></asp:ListItem>
                            <asp:ListItem Text="Program"></asp:ListItem>
                            <asp:ListItem Text="Corporate"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lbIssueImpact" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row" id="trStartDate" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Date Identified</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit CssClass="CRMDueDate_inputField" DropDownButton-Image-Url="~/Content/Images/calendarNew.png" ID="dtcStartDate"
                            runat="server" DropDownButton-Image-Width="18"></dx:ASPxDateEdit>
                        <asp:Label ID="lbStartDate" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12" id="trDueDate" runat="server">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Due Date</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit CssClass="CRMDueDate_inputField" ID="dtcDueDate" runat="server"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png" DropDownButton-Image-Width="18">
                        </dx:ASPxDateEdit>
                        <asp:Label ID="lbDueDate" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="row" id="trNote" runat="server">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                        <asp:Label ID="lbDescription" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
            </div>
            
            <div class="row" id="trResolutionDate" runat="server" visible="false">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Resolution Date</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcResolutionDate" runat="server" CssClass="CRMDueDate_inputField" DropDownButton-Image-Width="18"
                            DropDownButton-Image-Url="~/Content/Images/calendarNew.png">
                        </dx:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12" id="tr1" runat="server" visible="false">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Resolution</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtResolution" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row" id="trComment" runat="server" visible="false">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Comment</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                
            </div>
            <div class="row"> 
                <div class="col-md-6 col-sm-6 col-xs-6">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Attachment</h3>
                    </div>
                    <div style="padding-left:6px;">
                        <ugit:FileUploadControl ID="FileUploadControl" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row addEditPopup-btnWrap">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" 
                        OnClick="btnCancel_Click"></dx:ASPxButton>
                    <dx:ASPxButton ID="btSaveAndNotify" ValidationGroup="formABudget" ToolTip="Save" runat="server" Text="Save & Notify" 
                        OnClick="btSaveAndNotify_Click" CssClass="primary-blueBtn">            
                    </dx:ASPxButton>
                    <dx:ASPxButton CssClass="primary-blueBtn" AutoPostBack="true" ValidationGroup="Task" OnClick="BtSaveTask_Click" ID="btSaveTask" 
                        runat="server" Text="Save">
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </asp:Panel>
</div>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .attachment-container {
            float: left;
            width: 100%;
            padding-top: 7px;
        }

            .attachment-container .label {
                float: left;
                width: 24%;
                padding-left: 23px;
            }

            .attachment-container .attachmentform {
                float: left;
                width: 70%;
            }

        .attachmentform .oldattachment, .attachmentform .newattachment {
            float: left;
            width: 100%;
        }

        .attachmentform .fileupload {
            float: left;
            width: 100%;
        }

        .fileupload span, .fileread span {
            float: left;
        }

        .fileupload label, .fileread label {
            float: left;
            padding-left: 5px;
            font-weight: bold;
            padding-top: 3px;
            cursor: pointer;
        }

        .attachmentform .fileread {
            float: left;
            width: 100%;
        }

        .attachment-container .addattachment {
            float: left;
        }

            .attachment-container .addattachment img {
                border: 1px outset;
                padding: 3px;
            }
    </style>

 <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
     function addAttachment() {
         var uploadFiles = $(".attachment-container .fileitem");
         var uploadContainer = $(".attachment-container .newattachment");
         var addIcon = $(".attachment-container .addattachment");

         if (uploadFiles.length <= 5) {
             if (uploadFiles.length == 4) {
                 addIcon.css("visibility", "hidden");
             }
             uploadContainer.append('<div class="fileitem fileupload"><span><input type="file" name="pAttachment1" /></span><label onclick="removeAttachment(this)"><img src="/Content/Images/redNew_delete.png" alt="Delete"/></label></div>');
         }
     }

     function removeAttachment(obj) {

         var fileName = $(obj).find("span").text()
         var deleteCtr = $(".attachment-container .label :text");
         deleteCtr.val(deleteCtr.val() + "~" + fileName);

         var addIcon = $(".attachment-container .addattachment");
         $(obj).parent().remove();
         addIcon.css("visibility", "visible");
     }

    </script>
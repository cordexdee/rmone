<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMMSprintAdd.ascx.cs" Inherits="uGovernIT.Web.PMMSprintAdd" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        width: 160px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .btnDeleteTask {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/Content/images/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }
</style>
<script data-v="<%=UGITUtility.AssemblyVersion %>">

    function setSelectedDocumentDetails(documentData, documentId) {

       
        //Link multiple files
       <%-- for (var index = 0; index < documentData.length; index++) {

            $("div#<%=bindMultipleLink.ClientID%>").append('<a id=file_' + documentId[index] + ' onclick="return downloadSelectedFile(' + documentId[index] + ')" style="cursor: pointer;">' + documentData[index] +
                '</a>' + "<img src='/content/images/close-red.png' id= img_" + documentId[index] + " class='' onclick='deleteLinkedDocument(\"" + documentId[index] + "\")'></img><br/>");

        }
        //close Document folder page
        window.parent.CloseWindowCallback(document.location.href);

        $("#<%=fileAttchmentId.ClientID%>").val(documentId.join(","));--%>
    }

    function lnkDeleteTask() {
        if (confirm("Are you sure you want to delete the task?")) {
            return true;
        }
        else {
            return false;
        }
    }
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

    function addAttachment() {
        var uploadFiles = $(".attachment-container .fileitem");
        var uploadContainer = $(".attachment-container .newattachment");
        var addIcon = $(".attachment-container .addattachment");

        if (uploadFiles.length <= 5) {
            if (uploadFiles.length == 4) {
                addIcon.css("visibility", "hidden");
            }

            uploadContainer.append('<div class="fileitem fileupload"><span><input type="file" name="pAttachment1" /></span><label onclick="removeAttachment(this)"><img src="/_layouts/15/images/ugovernit/delete-icon.png" alt="Delete"/></label></div>');
        }
    }

    function removeAttachment(obj) {
        var fileName = $(obj).parent().find("a b").html();
        var deleteCtr = $(".attachment-container .label :text");
        deleteCtr.val(deleteCtr.val() + "~" + fileName);
        var addIcon = $(".attachment-container .addattachment");
        $(obj).parent().remove();
        addIcon.css("visibility", "visible");
    }
</script>
<div id="divAddTask" class="pickervaluevontainer" style="margin-left:10px">
    <asp:Panel ID="pnlAddTasks" runat="server"  class="accomp-popup">
        <div style="width: 100%">
            <div class="row">
                <div class="colXs">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader">Sprint</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlSprints" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="colXs">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader">Release</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlReleases" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="colXs">
                     <div class="ms-formlabel">
                        <h3 class="ms-standardheader">Title<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtTitle" CssClass="full-width" runat="server" ValidationGroup="Task"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="Task" ControlToValidate="txtTitle"
                            Display="Dynamic" ErrorMessage="Please enter title." CssClass="input_errorMsg"></asp:RequiredFieldValidator>

                        <asp:Label ID="lbTitle" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div class="colXs">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader">Estimated Hours(hrs)</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtEstimatedHours" Width="50" ValidationGroup="Task" runat="server">
                        </asp:TextBox>
                        <asp:Label ID="lbEstimatedHours" runat="server" Visible="false"></asp:Label>
                        <asp:RegularExpressionValidator ID="revEstimatedHours" runat="server" ControlToValidate="txtEstimatedHours"
                            ValidationGroup="Task" Display="Dynamic" ErrorMessage="Please enter estimated hour in correct format"
                            ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$">
                        </asp:RegularExpressionValidator>
                    </div>
                </div>
            </div>
             <div class="row">
                 <div class="colXs">
                      <div id="trAssignedTo" runat="server" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader">Assigned To
                            </h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <ugit:UserValueBox runat="Server" isMulti="true" ID="peAssignedTo" UserType="UserOnly" CssClass="pmmAssignTo_dropDown"></ugit:UserValueBox>
                            <asp:Label ID="lbAssignedTo" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                 </div>
                 <div class="colXs">
                     <div id="trStatus" runat="server" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader">Status
                            </h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:DropDownList ID="ddlStatus" runat="server" onchange="modifyPctCompleteFromStatus();">
                                <asp:ListItem Text="Not Started"></asp:ListItem>
                                <asp:ListItem Text="In Progress"></asp:ListItem>
                                <asp:ListItem Text="Completed"></asp:ListItem>
                                <asp:ListItem Text="Deferred"></asp:ListItem>
                                <asp:ListItem Text="Waiting on someone else"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbStatus" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                 </div>
             </div>
           
            <div class="row">
                <div class="colXs">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader">Description</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox CssClass="pmmtextarea_inputField" Rows="5" ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>
                        <asp:Label ID="lbDescription" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>
                <div class="colXs">
                    <div id="trPctComplete" runat="server" visible="false">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader">% Complete
                            </h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtPctComplete" Width="50" ValidationGroup="Task" Style="text-align: right;"
                                runat="server" onblur="modifyStatusFromPctComplete()">
                            </asp:TextBox>
                            <asp:Label ID="lbPctComplete" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
                
            </div>
            <div class="row">
                   <div id="trComment" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader">Comment</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div style="float: left; width: 98%;">
                                    <asp:TextBox CssClass="pmmtextarea_inputField" Rows="5" ID="txtComment" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <div style="float: left; width: 100%; display: block; max-height: 150px; overflow-x: auto;">
                                    <asp:Repeater ID="rComments" runat="server" OnItemDataBound="RComments_ItemDataBound">
                                        <ItemTemplate>
                                            <div style="float: left; width: 100%;">
                                                <span style="font-weight: bold;"><a href="javascript:void(0);">
                                                    <asp:Literal ID="lCommentOwner" runat="server"></asp:Literal></a>
                                                    (<a href="javascript:void(0);"><asp:Literal ID="lCommentDate" runat="server"></asp:Literal></a>):</span>
                                                <span>
                                                    <asp:Literal ID="lCommentDesc" runat="server"></asp:Literal>
                                                </span>
                                            </di>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
            </div>
            
                
                
                    
            </div>
            <div id="trAttachment" runat="server" visible="false" class="row">

                <ugit:UploadAndLinkDocuments ID="UploadAndLinkDocuments" runat="server" />
                
                <%--<div class="ms-formlabel pmmScrum_attachmentWrap">
                    <ugit:FileUploadControl ID="ugitupAttachments" runat="server" CssClass="fileUploadIcon" />
                    <asp:Panel ID="pAttachmentContainer" runat="server" CssClass="attachment-container" Visible="true">
                        <div class="label">
                            <span style="display: none;">
                                <asp:TextBox ID="txtDeleteFiles" runat="server"></asp:TextBox>
                            </span>
                        </div>
                        <div class="attachmentform">
                            <asp:Panel ID="pAttachment" runat="server" CssClass="oldattachment">
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                    <asp:Label ID="lblerror" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                </div>--%>
            </div>
         <div id="bindMultipleLink" runat="server">
                            </div>
        </div>
        <div class="budgetBtn_wrap row fieldWrap cancelInvite">
                        <div class="rmmNewUserbtn">
                            <div class="RMMBtnWrap">                       
                                <div class="secondaryCancelBtn-wrap">
                                    <div class="row">
                                    <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn" ToolTip="Cancel" >
                                    </dx:ASPxButton>
                                    <dx:ASPxButton ID="lnkDeleteTask" Visible="false" runat="server" Text="Delete Tasks" OnClick="lnkDeleteTask_Click" CssClass="secondary-cancelBtn" ToolTip="Delete" >
                                        <ClientSideEvents Click="lnkDeleteTask" />
                                    </dx:ASPxButton>
                                  
                                <dx:ASPxButton ID="btAddTask" CssClass="primary-blueBtn" runat="server" Text="Save" ToolTip="Save" OnClick="btAddTask_Click" >
                                </dx:ASPxButton>
                                </div>
                                       </div> 
                            </div>
                        </div>
             </div>
    </asp:Panel>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketCommentsView.ascx.cs" Inherits="uGovernIT.Web.TicketCommentsView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function Validation(sender,e) {

        var message = "";
        if (aspxGridClientInstance.GetSelectedRowCount() > 0) {
            if (aspxGridClientInstance.GetSelectedRowCount() > 1) {

                message = "Are you sure you want to delete the selected entries?";
            }
            else {
                var index = Number(aspxGridClientInstance.GetSelectedKeysOnPage()[0]);
                var comment = $($(aspxGridClientInstance.GetDataRow(index)).find("td")[3]).html();
                message = "Are you sure you want to delete this entry: \n\n" + comment;                
            }
        }
        else {
            alert("Please select at least one entry.");
            return false;
        }

        if (confirm(message)) {
            return true;

        }
        else {
            e.processOnServer = false;
            return false;
        }
    }

</script>
<style type="text/css">
    .editcomment-label {
        width:100px;
    }
    .commentcommandview img {
       padding:2px;
    }

    .profileImage {
    margin: 0px 0px 0px 5px;
    font-size: 18px;
    border: 2px solid lightgray;
    border-radius: 26px;
    height: 20px;
    width: 20px;
}
.roboto-font-family {
    font-family: 'Roboto', sans-serif !important;
}

  .addCommentButton{
    border:none;
    background:none;
    float:right;
    /*margin-top:7px;*/
}  
  .labelTitle {
    color: black;
    font-size: 13px;
    font-weight: 500;
    text-align: center !important;
}

    .btnDelete {
        background: none;
        border: 1px solid #4A6EE2;
        border-radius: 4px;
        margin-right: 5px;
        margin-top: 10px;
        color: #4FA1D6 !important;
        padding: 4px 18px;
        font-size: 12px;
        font-weight: 500;
    }

    .dx-overlay-content.dx-popup-normal.dx-popup-draggable.dx-resizable.dx-popup-flex-height {
        transform: translate(476px, 30px) scale(1) !important;
    }
 
</style>

     <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
         var CommentDetails = null;
         var data = null;
         var alertMessage = "";
         $(function () {

             $.get("/api/RMOne/GetModuleComments?projectID=" + '<%=TicketID%>', function (ConstrainDetails, status) {
                 data = ConstrainDetails != "Unable to Fetch" ? ConstrainDetails.lstModuleStageConstraints : null;
                 CommentDetails = ConstrainDetails.ListComments;
                 BindCommentDetails();
             });
         });


         function BindCommentDetails() {
             //debugger;
             $('#CommentsGridDiv').html('');
             let container = $('#CommentsGridDiv');
             container.append(
                 $("<div class='row col-md-12' style='padding:0px;'>").append(
                     $('<div id="commentsGrid">').dxDataGrid({
                         dataSource: CommentDetails,
                         remoteOperations: false,
                         searchPanel: {
                             visible: false,
                         },
                         height: 455,
                         rowAlternationEnabled: true,
                         wordWrapEnabled: true,
                         showBorders: true,
                         showColumnHeaders: true,
                         showColumnLines: false,
                         showRowLines: true,
                         noDataText: "No Data Found",
                         selection: {
                             mode: 'multiple',
                             showCheckBoxesMode: 'always',
                         },
                         columns: [
                             {
                                 dataField: "createdBy",
                                 dataType: "text",
                                 width: "20%",
                                 caption: 'Created By',
                                 cellTemplate: function (container, options) {
                                     if (options.data != null) {
                                         $(`<div class="roboto-font-family"><img src="${options.data.Picture}" class="profileImage"><span style='margin-left:5px;font-size:13px;'>${options.data.createdBy}</span></div>`).appendTo(container);
                                     }
                                 },
                                 headerCellTemplate: function (header, info) {
                                     $(`<div class='labelTitle'>${info.column.caption}</div>`).appendTo(header);
                                 }
                             },
                             {
                                 dataField: "created",
                                 dataType: "text",
                                 width: "15%",
                                 caption: 'Created',
                                 cellTemplate: function (container, options) {
                                     if (options.data != null) {
                                         $(`<div class="roboto-font-family"><span style='margin-left:5px;font-size:13px;'>${options.data.created}</span></div>`).appendTo(container);
                                     }
                                 },
                                 headerCellTemplate: function (header, info) {
                                     $(`<div class='labelTitle'>${info.column.caption}</div>`).appendTo(header);
                                 }
                             },
                             {
                                 dataField: "entry",
                                 dataType: "text",
                                 width: "65%",
                                 caption: 'Comment',
                                 cellTemplate: function (container, options) {
                                     if (options.data != null) {
                                         if (options.data.PrivateCommentImage != "") {
                                             $(`<div class="col-md-12 roboto-font-family" style="padding-top: 5px;">${options.data.PrivateCommentImage}<span style='margin-left:5px;font-size:13px;'>${options.data.entry}</span></div>`).appendTo(container);
                                         }
                                         else {
                                             $(`<div class="col-md-12 roboto-font-family" style="padding-top: 5px;"><span style='margin-left:5px;font-size:13px;'>${options.data.entry}</span></div>`).appendTo(container);
                                         }
                                     }
                                 },
                                 headerCellTemplate: function (header, info) {
                                     $(`<div class='labelTitle'>${info.column.caption}</div>`).appendTo(header);
                                 }
                             }
                         ],
                         onRowClick(e) {
                             editComment(e.data.entry, e.data.Index);
                         }
                     })
                 )
                 //)
             );
             return container;
         }
                 

         function OpenAddCommentPopup() {
             const popupAddComment = function () {
                 let container = $('<div class="roboto-font-family">');
                 container.append(
                     $("<div id='commentDescription' />").dxTextArea({
                         height: 120,
                         inputAttr: { 'aria-label': 'Comment' },
                     }).dxValidator({
                         validationGroup: "addCommentValidate",
                         validationRules: [{
                             type: "required",
                             message: "Enter Comment"
                         }]
                     })
                 );
                 let confirmBtn = $(`<div class="mt-2 mb-3 ml-2 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                     text: "Save",
                     hint: 'Save',
                     visible: true,
                     onClick: AddComments
                 })
                 let cancelBtn = $(`<div class="mt-2 mb-3 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                     text: "Cancel",
                     visible: true,
                     onClick: function (e) {
                         const popup = $("#addCommentPopup").dxPopup("instance");
                         popup.hide();
                     }
                 })
                 container.append(confirmBtn);
                 container.append(cancelBtn);
                 return container;
             };

             const popup = $('#addCommentPopup').dxPopup({
                 contentTemplate: popupAddComment,
                 visible: false,
                 hideOnOutsideClick: false,
                 showTitle: true,
                 showCloseButton: true,
                 title: "Add Comment",
                 width: 500,
                 height: "auto",
                 resizeEnabled: true,
                 dragEnabled: true,
             }).dxPopup('instance');

             popup.option({
                 contentTemplate: () => popupAddComment()
             });
             popup.show();
         }

         function editComment(OldComments, Indexs) {
             const popupUpdateComment = function () {
                 let container = $('<div class="roboto-font-family">');
                 container.append(
                     $("<div id='updateCommentDescription' />").dxTextArea({
                         height: 120,
                         value: OldComments,
                         inputAttr: { 'aria-label': 'Comment' },
                     }).dxValidator({
                         validationGroup: "updateCommentValidate",
                         validationRules: [{
                             type: "required",
                             message: "Enter Comment"
                         }]
                     })
                 );
                 let confirmBtn = $(`<div class="mt-2 mb-3 ml-2 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                     text: "Save",
                     hint: 'Save',
                     visible: true,
                     onClick: function (e) {
                         UpdateComments(OldComments, Indexs)
                     }
                 })
                 let cancelBtn = $(`<div class="mt-2 mb-3 btnAddNew" style='float:right;font-size: 14px;' />`).dxButton({
                     text: "Cancel",
                     visible: true,
                     onClick: function (e) {
                         const popup = $("#updateCommentPopup").dxPopup("instance");
                         popup.hide();
                     }
                 })
                 container.append(confirmBtn);
                 container.append(cancelBtn);
                 return container;
             };

             const popup = $('#updateCommentPopup').dxPopup({
                 contentTemplate: popupUpdateComment,
                 visible: false,
                 hideOnOutsideClick: false,
                 showTitle: true,
                 showCloseButton: true,
                 title: "Update Comment",
                 width: 500,
                 height: "auto",
                 resizeEnabled: true,
                 dragEnabled: true,
             }).dxPopup('instance');

             popup.option({
                 contentTemplate: () => popupUpdateComment()
             });
             popup.show();
         }

         function UpdateComments(OldComments, Indexs) {
             var result = DevExpress.validationEngine.validateGroup("updateCommentValidate").isValid;
             if (result) {
                 var Comments = $("#updateCommentDescription").dxTextArea('instance').option('value');
                 var TicketID = '<%=TicketID%>';
                 $.post("/api/RMOne/UpdateCommentDetails?Comment=" + encodeURIComponent(Comments) + "&Index=" + Indexs + "&TicketID=" + TicketID).then(function (response) {
                     const popup = $("#updateCommentPopup").dxPopup("instance");
                     popup.hide();
                     $.cookie("fromCommentTask", "0", { path: "/" });
                     //debugger;
                     $.get("/api/RMOne/GetModuleComments?projectID=" + '<%=TicketID%>', function (ConstrainDetails, status) {
                         CommentDetails = ConstrainDetails.ListComments;
                         BindCommentDetails();
                     });
                 });
             }
         }

         function AddComments() {
             //debugger;
             var result = DevExpress.validationEngine.validateGroup("addCommentValidate").isValid;
             if (result) {
                 var Comments = $("#commentDescription").dxTextArea('instance').option('value');
                 var TicketID = '<%=TicketID%>';
                 $.post("/api/RMOne/AddCommentDetails?Comment=" + encodeURIComponent(Comments) + "&TicketID=" + TicketID).then(function (response) {
                     //debugger;
                     const popup = $("#addCommentPopup").dxPopup("instance");
                     popup.hide();
                     //debugger;
                     $.cookie("fromCommentTask", "0", { path: "/" });
                     $.get("/api/RMOne/GetModuleComments?projectID=" + '<%=TicketID%>', function (ConstrainDetails, status) {
                         CommentDetails = ConstrainDetails.ListComments;
                         BindCommentDetails();
                     });
                 });
             }
         }

         function DeleteComments() {
             var grid = $('#commentsGrid').dxDataGrid("instance");
             var Selectedrows = grid.getSelectedRowsData();
             if (Selectedrows.length > 0) {
                 if (Selectedrows.length > 1) {
                     alertMessage = "Are you sure you want to delete the selected entries?";
                 }
                 else {
                     var entry = Selectedrows[0].entry;
                     alertMessage = "Are you sure you want to delete this entry: \n\n" + entry;                
                 }
                 if (confirm(alertMessage)) {
                     var list = {};                     
                     var gridmodels = new Array();
                     Selectedrows.forEach(function (row, index) {
                         var objComments = {};
                         objComments.Created = row.created;
                         objComments.CreatedBy = row.createdBy;
                         objComments.Entry = row.entry;
                         objComments.TicketID = '<%=TicketID%>';
                         gridmodels.push(objComments);
                     });
                     list.lstComments = gridmodels;
                     $.post("/api/RMOne/DeleteComments/", list).then(function (response) {
                         $.get("/api/RMOne/GetModuleComments?projectID=" + '<%=TicketID%>', function (ConstrainDetails, status) {
                             CommentDetails = ConstrainDetails.ListComments;
                             BindCommentDetails();                             
                         });
                     });
                 }
             }
             else {
                 alert("Please select at least one entry.");
                 return false;
             }
         }

        function UpdateGridHeight() {
            aspxGridClientInstance.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            aspxGridClientInstance.SetHeight(containerHeight);
        }
        window.addEventListener('resize', function (evt) {
            if (!ASPxClientUtils.androidPlatform)
                return;
            var activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
        });
     </script>
<div style="height:500px;">    
    <div style="float: right;margin-bottom:10px;">
        <input type="button" class="btnDelete" id="btnDelete" value="Delete" onclick="DeleteComments()" />
    </div>
    <div id="CommentsGridDiv"></div>
    <%--<div id="addCommentPopup"></div>--%>
    <div id="updateCommentPopup"></div>
    
    <%--<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
        <div class="row">
             <ugit:ASPxGridView ID="aspxGrid"  runat="server" AutoGenerateColumns="False" Images-HeaderActiveFilter-Url="../../Content/Images/Filter_Red_16.png"
                ClientInstanceName="aspxGridClientInstance" 
                 Width="100%"  OnRowUpdating="aspxGrid_RowUpdating"  OnCommandButtonInitialize="aspxGrid_CommandButtonInitialize" SettingsText-EmptyHeaders="&nbsp;" KeyFieldName="IndexID" CssClass="">
               <settingsadaptivity adaptivitymode="Off" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="true" Name="checkboxAction" Width="20px">
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataTextColumn Caption="Created" FieldName="Created" Width="150px" HeaderStyle-Font-Bold="true"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
                        <EditFormSettings Visible="False"  />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Created By" FieldName="createdByUser" Width="150px" HeaderStyle-Font-Bold="true"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                           <EditFormSettings Visible="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataMemoColumn PropertiesMemoEdit-Rows="4"   Width="20px"  Caption="Comment" FieldName="entry" HeaderStyle-Font-Bold="true" CellStyle-Wrap="True" >
                        <EditFormCaptionStyle CssClass="editcomment-label" ></EditFormCaptionStyle>
                    </dx:GridViewDataMemoColumn>
                    <dx:GridViewCommandColumn Width="20px" Name="editAction" Caption="" ShowEditButton="true"  ButtonType="Image">
                    </dx:GridViewCommandColumn>
                </Columns>
                <SettingsCommandButton >
                      <EditButton Image-Url="/Content/Images/editNewIcon.png" Image-Width="20px"></EditButton>
                        <UpdateButton Text="Update" Image-Url="/Content/Images/update-button.png" ></UpdateButton>
                        <CancelButton Image-Url="/Content/Images/close-red-big.png"></CancelButton>
                      <ShowAdaptiveDetailButton ButtonType="Button"></ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Button"></HideAdaptiveDetailButton>
                </SettingsCommandButton>

                <SettingsBehavior AllowSort="false" />
                <SettingsEditing Mode="EditFormAndDisplayRow" EditFormColumnCount="1"/>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <ClientSideEvents />
              <Styles>
                  <CommandColumn CssClass="commentcommandview"></CommandColumn>
                  <Row CssClass="gridData_row"></Row>
              </Styles>
            </ugit:ASPxGridView>
             <script type="text/javascript">
                     ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                         UpdateGridHeight();
                     });
                     ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                         UpdateGridHeight();
                     });
               </script>
        </div>
        <div class="row editBtnWrap addEditPopup-btnWrap">
             <dx:ASPxButton ID="lnkbtnDelete" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" runat="server" ToolTip="Delete" OnClick="lnkbtnDelete_Click" cssclass="secondary-cancelBtn">
                <ClientSideEvents Click="function(s,e){return Validation(this);}" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="lnkClose" runat="server" Text="&nbsp;&nbsp;Close&nbsp;&nbsp;" ToolTip="Close" OnClick="lnkClose_Click" CssClass="primary-blueBtn">
            </dx:ASPxButton>
        </div>
    </div>--%>
    </div>
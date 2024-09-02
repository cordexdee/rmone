<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpCardAdd.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.HelpCardAdd" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<div id="loadPanel"></div>
<br />
<div class="col-md-12 col-xs-12 col-md-12 wikiNew-popupWrap">
      <div class="row"> 
       <div class="col-md-2">
             <div class="wikiTitle-container">
             <div class="mandatoryField"> <span style="color:red"> *</span></div>
               <div id="HelpCardTitle" class="helpCardTitle-textBox"></div>
              <div id ="validateTitle"></div>
             </div>        
        </div>
          <div class="col-md-2">
                <div class="wikiTitle-container">
                 <div class="mandatoryField"> <span style="color:red"> *</span></div>
                   <div id="HelpCardCategory" class="HelpCard-Category"></div>
                  <div id ="validateCategory"></div>
               </div>
          </div>
          <div class="col-md-3">
                <div class="wikiTitle-container">
                 <div class="mandatoryField"> <span style="color:red"> *</span></div>
                   <div id="HelpCardDescription" class="HelpCard-Category"></div>
                  <div id ="validateDescription"></div>
               </div>
          </div>

           <div class="col-md-2">
                <div class="wikiTitle-container">                 
                   <div id="divAgent" class="HelpCard-Category"></div>                 
               </div>
          </div>
          <div class="col-md-2">
            <div id="btncreate" class="dxtPrimary-btn"></div>
            <div id="btnSave" class="dxtPrimary-btn"></div>
         </div>
    </div> 
        <br />
        <br />
        <div class="row">
            <div class="col-md-9">
                <div class="htmleditor"></div>
                 <div id="upl-popup"></div>
            </div>
            <div class="col-md-3">
                <div class="row">
                  
		          <div class="helpCardPreview flashCard-container">
                  </div>
                 </div>                
            </div>
        </div>
        <br />
 
 </div>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
/*.flashCard-container{
		    width: 341px;
		    margin: 0px auto;
		    border: 1px solid #FFF;
		    height: 495px;
		    padding: 10px;
	        box-shadow: 0px 0px 4px #888888;
		}
.flashCard-container img{
			max-width: 100%;
		}*/

.requiredField{
        float: left;
        margin-right: 3px;
    }
.requiredField, .wikiNew-title{
        display:inline-block;
    }
.requiredField, .HelpCard-Category{
        display:inline-block;
    }
.agentBtnImg{
    width: 30px;
}
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var flashCard = {};
    var AddHelpCardModel = {};
    var HelpCardResponse = {};
    var listCategory = [];
    var TicketId = '<%= TicketId%>';
    var strAction = '<%= strAction%>';
    var HelpCardId = <%= HelpCardId%>;
    var editor = "";
    var HelpHtmlContent = "";
    var htmlEditorContent = '';
    var agentBtnHtml = "";
    var AgentDropdown = "";
    var selectedAgent = [];
    $(function () {
        DrawFlashCardHtmlEditor(); 
        DrawAgent([]);
    })

    function DrawFlashCardHtmlEditor() {

            var uploadPopup = $("#upl-popup")
                .dxPopup({
                    width: "40%",
                    height: "40%",
                  contentTemplate: function(contentElement) {
                    $("<div>")
                      .appendTo(contentElement)
                      .dxFileUploader({
                          height: "100%",                          
                          labelText:"",
                          uploadMode: "useButtons",
                          maxFileSize:1000000,
                          readyToUploadMessage:"Ready to insert",
                          uploadUrl: "/api/HelpCard/AsyncUpload",
                        allowedFileExtensions: [".pdf", ".docx"],
                        onUploaded: function (e) {                            
                          var range = editor.getSelection();
                          var index = range && range.index || 0;  
                          //editor.insertEmbed(index, "extendedImage", {
                          //    src: "/Content/Images/helpcardImage/" + e.file.name
                          //});

                            editor.insertEmbed(index, "link", {
                                href: "/Content/Images/helpcardImage/" + e.file.name,
                                text:e.file.name
                          });


                          uploadPopup.hide();
                          editor.focus();
                        }
                      });
                  }
                })
                .dxPopup("instance");



         editor = $(".htmleditor").dxHtmlEditor({
            height: 550,
            valueType: "html",
            value: HelpHtmlContent,
            toolbar: {
                items: [
                    "undo", "redo", "separator",
                    {
                        name: "size",
                        acceptedValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"]
                    },
                    {
                        name: "font",
                        acceptedValues: ["Arial", "Courier New", "Georgia", "Impact", "Lucida Console", "Tahoma", "Times New Roman", "Verdana"]
                    },
                    "separator", "bold", "italic", "strike", "underline", "separator",
                    "alignLeft", "alignCenter", "alignRight", "alignJustify", "separator",
                    "orderedList", "bulletList", "separator",
                    {
                        name: "header",
                        acceptedValues: [false, 1, 2, 3, 4, 5]
                    },
                    //{
                        
                    //    widget: "dxButton",
                    //    options: {
                    //        icon: "image",
                    //        hint: "Insert Image",                            
                    //        onClick: function () {
                    //            uploadPopup.show();
                    //        }
                    //    }
                    //},
                    {
                        
                        widget: "dxButton",
                        options: {
                            icon: "doc",
                            hint: "Upload doc",                            
                            onClick: function () {
                                uploadPopup.show();
                            }
                        }
                    },

                    "separator",
                    "color", "background", "separator",
                    "link", "image", "separator",
                    "clear", "codeBlock", "blockquote"
                ],
                multiline: true
            },
            mediaResizing:
            {
                enabled: true
            },
             onValueChanged: function (e) {
                 //$(".helpCardPreview").html(e.component.option("value"));

                 htmlEditorContent = e.component.option("value");

                 $(htmlEditorContent).find('a').each(function () {
                     var href = $(this).attr('href');
                     if (href.toLowerCase().indexOf("http:") != -1) {
                         DevExpress.ui.dialog.alert("Not accepting URL having http request.", "Error");
                         var btnCreate = $("#btncreate").dxButton("instance");
                         btnCreate.option("visible", false);
                         var btnSave = $("#btnSave").dxButton("instance");
                         btnSave.option("visible", false);
                         return false;
                     }
                     else {
                         if (TicketId != null && strAction == "edit") {
                             var btnSave = $("#btnSave").dxButton("instance");
                             btnSave.option("visible", true);
                         }
                         else {
                             var btnCreate = $("#btncreate").dxButton("instance");
                             btnCreate.option("visible", true);
                         }
                     }
                 });
                // e.component.option("value",htmlEditorContent + agentBtnHtml);  
                 $(".helpCardPreview").html(htmlEditorContent + agentBtnHtml);
             },            

        }).dxHtmlEditor("instance");

    }
    function DrawAgent(listAgent) {
       AgentDropdown=  $("#divAgent").dxTagBox({
             items: listAgent,
             displayExpr: "Title",
             placeholder:"Select Agents",
             showSelectionControls: true,
             applyValueMode: "useButtons",
             searchEnabled: true, 
             value: selectedAgent,
             text:'widgets',
             onValueChanged: function (e) {
                const previousValues = e.previousValue;
                 const newValues = e.value;
                 /*console.log(newValues);*/ 
                 var agentButton = '';
                 if (newValues) {
                     newValues.forEach(function (x, i) {                         
                         agentButton = `<div class='col-md-6' title='${x.Title}' onclick='handleWidget(${x.Id})'><a id=${x.Id} ><img src='${x.Icon ? x.Icon : "/Content/Images/agent-icon.png"}' class='agentBtnImg'/> </a></div>` + agentButton                        
                     });
                     agentBtnHtml =`<div class='row'> ${agentButton} </div>`;
                 }
                 
                 $(".helpCardPreview").html(htmlEditorContent + agentBtnHtml);
           }
       }).dxTagBox('instance');
    }
    $(document).ready(function () {
        $.get("/api/HelpCard/GetAgents", function (data, status) {

            /*console.log(data);*/
            if (data) {
                DrawAgent(data);
            }
            else {
                data = [];
               // DrawAgent(data);
                AgentDropdown.option('value', HelpCardResponse.listAgents);                      
            }
        });        
            if (TicketId != null && strAction == "edit") {

                   var btnCreate = $("#btncreate").dxButton("instance");
                    btnCreate.option("visible", false);

                   $.get("/api/HelpCard/GetHelpCardByTicketId?id=" + TicketId, function (data, status) {
                       HelpCardResponse = data;
                       AgentDropdown.option('value',HelpCardResponse.listAgents)                       
                       var textbox = $('#HelpCardTitle').dxTextBox("instance");
                       textbox.option("value", HelpCardResponse.HelpCardTitle);                       
                       var ddlCategory = $('#HelpCardCategory').dxAutocomplete("instance");
                       ddlCategory.option("value", HelpCardResponse.HelpCardCategory);

                       var txtDescription = $("#HelpCardDescription").dxTextArea("instance");
                       txtDescription.option("value", HelpCardResponse.Description);
                        
                       HelpHtmlContent = HelpCardResponse.HelpCardContent;
                       var editorhtml = $(".htmleditor").dxHtmlEditor('instance');
                       editorhtml.repaint();
                       DrawFlashCardHtmlEditor();
                });

            }
            else
            {
                    var btnSave = $("#btnSave").dxButton("instance");
                    btnSave.option("visible", false);

            }

             $(document).ready(function(){
                 $('.htmleditor').on("paste", function (e) {                     
                     var type = e.originalEvent.clipboardData.types[0];
                     if (type != "text/html" && type != 'text/plain')
                     {
                        e.preventDefault();
                     }
               });
            });


    });
    

        $("#HelpCardTitle").dxTextBox({

            placeholder: "Enter Help Card  Title",
            onValueChanged: function (e) {                
                AddHelpCardModel.HelpCardTitle = e.value;
                if (AddHelpCardModel.HelpCardTitle != undefined) {
                  $("#validateTitle").html("");

                }

            },
        });

        $("#HelpCardDescription").dxTextArea({

            placeholder: "Enter Description",
            height: 50,
                onValueChanged: function (e) {                
                    AddHelpCardModel.Description = e.value;
                    if (AddHelpCardModel.Description != undefined) {
                      $("#validateDescription").html("");

                    }

                },
    });

        $.get("/api/HelpCard/GetCategories", function (data,status) {
                        listCategory = data;
                        $("#HelpCardCategory").dxAutocomplete({
                            dataSource: listCategory,
                            placeholder: "Type Category...",
                            onValueChanged: function (e) {                                
                                AddHelpCardModel.HelpCardCategory = e.value; 
                               AddHelpCardModel.HelpCardCategory = AddHelpCardModel.HelpCardCategory ? AddHelpCardModel.HelpCardCategory.trim() : AddHelpCardModel.HelpCardCategory;
                                if (AddHelpCardModel.HelpCardCategory != undefined)
                                {
                                        $("#validateCategory").html("");
                                }
                            },
                            onChange: function (e) {                                
                                var value = e.component.option('value');
                                value = value ? value.trim() : value;
                                AddHelpCardModel.HelpCardCategory = value;
                                if (listCategory != null && listCategory.length != 0 && value) {
                                    var newValue = listCategory.filter(x => x.toUpperCase() == value.toUpperCase());
                                    if ((newValue[0])) {
                                        e.component.option('value', newValue[0]);
                                        AddHelpCardModel.HelpCardCategory = newValue[0];                                    
                                    }                                                                         
                                }                                
                                if (AddHelpCardModel.HelpCardCategory != undefined)
                                {
                                        $("#validateCategory").html("");
                                }
                            },
                          
                        });
        });         
        
        $("#btncreate").dxButton({
            text: "Create",
            onClick: function (e) {                
                var allowCreate = true;                
                var myText =editor.option("value");                
                //var myText =htmlEditorContent + agentBtnHtml;                
                 AddHelpCardModel.HelpCardContent = myText;
                if (AddHelpCardModel.HelpCardTitle == undefined || AddHelpCardModel.HelpCardTitle == '') {
                    allowCreate = false;
                    $("#validateTitle").html("<span style='color:red'>Enter title</span>");
                }
                if (AddHelpCardModel.HelpCardCategory == undefined || AddHelpCardModel.HelpCardCategory == '') {
                    allowCreate = false;
                    $("#validateCategory").html("<span style='color:red'>Enter Category</span>");
                }
                if (AddHelpCardModel.Description == undefined || AddHelpCardModel.Description == '') {
                    allowCreate = false;
                    $("#validateDescription").html("<span style='color:red'>Enter Description</span>");
                }              
                AddHelpCardModel.listAgents = AgentDropdown.option("value");
                AddHelpCardModel.AgentContent = agentBtnHtml;
                if (allowCreate == true) {
                        $.ajax({
                            type: "POST",
                            data :JSON.stringify(AddHelpCardModel),
                            url: "/api/HelpCard/CreateHelpCard",
                            contentType: "application/json",
                            success: function () {
                                window.parent.CloseWindowCallback(1, document.location.href);
                            }
                        });                                            
                }
            }
        });

        $("#btnSave").dxButton({
            text: "Save",
            onClick: function (e) {                
                $(this).parent(".ui-dialog.ui-corner-all").dialog('close');
                var allowSave = true;
                var myText = editor.option("value"); 
               // var myText =htmlEditorContent + agentBtnHtml;                
                AddHelpCardModel.HelpCardContent = myText;
                AddHelpCardModel.HelpCardTicketId = TicketId;
                AddHelpCardModel.listAgents = AgentDropdown.option("value");
                AddHelpCardModel.AgentContent = agentBtnHtml;

                var textbox = $('#HelpCardTitle').dxTextBox("instance");
                AddHelpCardModel.HelpCardTitle = textbox._getOptionValue('value');

                var txtDescription = $("#HelpCardDescription").dxTextArea("instance");
                AddHelpCardModel.Description = txtDescription._getOptionValue('value');

                var ddlCategory = $('#HelpCardCategory').dxAutocomplete("instance");
                AddHelpCardModel.HelpCardCategory = ddlCategory._getOptionValue('value');

                if (AddHelpCardModel.HelpCardTitle == undefined || AddHelpCardModel.HelpCardTitle == '') {
                    allowSave = false;
                    $("#validateTitle").html("<span style='color:red'>Enter title</span>");
                }

                if (AddHelpCardModel.HelpCardCategory == undefined || AddHelpCardModel.HelpCardCategory == '') {
                    allowSave = false;
                    $("#validateCategory").html("<span style='color:red'>Enter Category</span>");
                }  

                if (AddHelpCardModel.Description == undefined || AddHelpCardModel.Description == '') {
                    allowCreate = false;
                    $("#validateDescription").html("<span style='color:red'>Enter Description</span>");
                }   

                if (allowSave == true) {
                  $.ajax({
                            type: "POST",
                            data :JSON.stringify(AddHelpCardModel),
                            url: "/api/HelpCard/UpdateHelpCard",
                            contentType: "application/json",
                      success: function () {
                                 window.parent.CloseWindowCallback(1, document.location.href);

                      }
               });

                    //window.parent.CloseWindowCallbackForWiki(0, document.location.href);
                    //var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails" + "&ticketId=" + TicketId;
                    //window.parent.UgitOpenPopupDialog(url, "", "View Wiki Article", "98", "100", false, "");
                    
                }

            }
        });


</script>
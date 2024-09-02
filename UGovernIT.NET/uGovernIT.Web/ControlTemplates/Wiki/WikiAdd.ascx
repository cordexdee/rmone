<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiAdd.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Wiki.WikiAdd" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .html-editor-wrapper {
        height: 900px;
        width: 600px;
        background-color: !important floralwhite
    }

    .wikiNew-popupWrap {
        padding-top: 10px;
    }

    .wikiNew-title {
        /*border: none !important;*/
        width: 95%;
    }

    .wikiNew-dropDownWrap {
        padding: 10px 0px 10px 0px;
    }

    .noSidePadding {
        padding: 0px;
    }

    .wikiNew-moduleDropDown {
        padding-left: 0px;
        border: none !important;
        background: none !important;
        display: inline-block;
        width: 95%;
    }

        .wikiNew-moduleDropDown div.dx-dropdowneditor-input-wrapper {
            background: #fff;
            border: 1px solid #ababab;
            border-radius: 4px;
            display: inline-block;
            width: 91%;
        }

        .wikiNew-moduleDropDown .dx-dropdowneditor-input-wrapper div.dx-texteditor-container div.dx-texteditor-input-container input[type="text"] {
            border: none;
        }

    .wikiNew-requestType-dropDown {
        background: none !important;
        border: none !important;
        padding-right: 0px;
        display: inline-block;
        width: 91%;
    }

        .wikiNew-requestType-dropDown .dx-dropdowneditor-input-wrapper {
            background: #fff;
            border: 1px solid #ababab;
            border-radius: 4px;
        }

            .wikiNew-requestType-dropDown .dx-dropdowneditor-input-wrapper div.dx-texteditor-container div.dx-texteditor-input-container input[type="text"] {
                border: none;
            }

    /*.wikiNew-htmlEditor {
        margin-top: 10px;
        border-color: #ababab;
        border-radius: 4px;
        padding: 1px;
    }

        .wikiNew-htmlEditor .dx-htmleditor-toolbar-wrapper {
            background: #eaeaef;
            border-bottom: 1px solid #ababab;
        }

        .wikiNew-htmlEditor .dx-quill-container.ql-container {
            background: #fff;
        }*/

    .wikiNew-btnWrap {
        margin-top: 10px;
        float: right;
    }

        .wikiNew-btnWrap div.dx-button-mode-contained {
            background-color: #4A6EE2;
            color: #fff;
            font: 14px 'Poppins', sans-serif !important;
            border-color: #4A6EE2;
        }

    .newWiki-htmlEditor {
        width: 100%;
    }

        .newWiki-htmlEditor tr td table {
            width: 100% !important;
            background: #e3e3e5;
        }

    .requiredField {
        float: left;
        margin-right: 3px;
    }

    .requiredField, .wikiNew-title {
        display: inline-block;
    }

    .requiredField, .ddlModule {
        display: inline-block;
    }

    .requiredField, .ddlRequestType {
        display: inline-block;
    }

    #validateTitle, #validateModule, #validateRequestType {
        margin-left: 13px;
        font: 15px 'Poppins', sans-serif !important;
    }

    .wikiTitle-container {
        padding-left: 12px;
    }

    .wikiTitle-wrap .wikiTitleAnchor {
        cursor: pointer;
    }
</style>

<div id="loadPanel"></div>

<div class="col-md-12 col-xs-12 col-md-12 wikiNew-popupWrap">
       <div class="wikiTitle-container">
         <div class="requiredField"> <span style="color:red"> *</span></div>
          <div id="wikiTitle" class="wikiNew-title"></div>
          <div id ="validateTitle"></div>
        </div>

      <div class="row wikiNew-dropDownWrap">
            <div class="col-md-6 col-sm-12 col-xs-12"">
               <div class="requiredField"> <span style="color:red"> *</span></div>
                 <div id="ddlModule" class="wikiNew-moduleDropDown"></div>
                  <div id ="validateModule"></div>
               </div>
            <div class="col-md-6 col-sm-12 col-xs-12"">
                 <div class="requiredField"> <span style="color:red"> *</span></div>
                  <div id="ddlRequestType" class="wikiNew-requestType-dropDown"></div>
                  <div id ="validateRequestType"></div>
                </div>
    </div>



    <div class="htmleditor">
          <dx:ASPxHtmlEditor ID="ASPxHtmlEditor" runat="server" ClientInstanceName="htmlEditor" CssClass="newWiki-htmlEditor">
                         <Toolbars>
                                <dx:HtmlEditorToolbar Name="myCustomToolbar">
                                    <Items>
                                        <dx:ToolbarCutButton></dx:ToolbarCutButton>
                                        <dx:ToolbarCopyButton></dx:ToolbarCopyButton>
                                        <dx:ToolbarPasteButton></dx:ToolbarPasteButton>
                                        <dx:ToolbarPasteFromWordButton></dx:ToolbarPasteFromWordButton>
                                        <dx:ToolbarUndoButton BeginGroup="True"></dx:ToolbarUndoButton>
                                        <dx:ToolbarRedoButton></dx:ToolbarRedoButton>
                                        <dx:ToolbarRemoveFormatButton></dx:ToolbarRemoveFormatButton>
                                        <dx:ToolbarSuperscriptButton></dx:ToolbarSuperscriptButton>
                                        <dx:ToolbarSubscriptButton></dx:ToolbarSubscriptButton>
                                        <dx:ToolbarInsertOrderedListButton></dx:ToolbarInsertOrderedListButton>
                                        <dx:ToolbarIndentButton></dx:ToolbarIndentButton>
                                        <dx:ToolbarOutdentButton></dx:ToolbarOutdentButton>
                                        <dx:ToolbarInsertLinkDialogButton></dx:ToolbarInsertLinkDialogButton>
                                        <dx:ToolbarUnlinkButton></dx:ToolbarUnlinkButton>
                                        <dx:CustomToolbarButton CommandName="addWikiLink" ToolTip="Add wiki link" Text="Wiki Link"></dx:CustomToolbarButton>
                                        <dx:ToolbarInsertImageDialogButton></dx:ToolbarInsertImageDialogButton>
                                        <dx:ToolbarInsertTableDialogButton BeginGroup="True" ViewStyle="ImageAndText"></dx:ToolbarInsertTableDialogButton>
                                        <dx:ToolbarFullscreenButton></dx:ToolbarFullscreenButton>
                                    </Items>
                                </dx:HtmlEditorToolbar>
                            </Toolbars>
                      
                         <Toolbars>
                                <dx:HtmlEditorToolbar Name="myCustomToolbar2">
                                    <Items>
                                        <dx:ToolbarParagraphFormattingEdit>
                                            <Items>
                                                <dx:ToolbarListEditItem Text="Normal" Value="p" />
                                                <dx:ToolbarListEditItem Text="Heading  1" Value="h1" />
                                                <dx:ToolbarListEditItem Text="Heading  2" Value="h2" />
                                                <dx:ToolbarListEditItem Text="Heading  3" Value="h3" />
                                                <dx:ToolbarListEditItem Text="Heading  4" Value="h4" />
                                                <dx:ToolbarListEditItem Text="Heading  5" Value="h5" />
                                                <dx:ToolbarListEditItem Text="Heading  6" Value="h6" />
                                                <dx:ToolbarListEditItem Text="Address" Value="address" />
                                                <dx:ToolbarListEditItem Text="Normal (DIV)" Value="div" />
                                            </Items>
                                        </dx:ToolbarParagraphFormattingEdit>
                                        <dx:ToolbarFontNameEdit>
                                            <Items>
                                                <dx:ToolbarListEditItem Text="Times New Roman" Value="Times New Roman" />
                                                <dx:ToolbarListEditItem Text="Tahoma" Value="Tahoma" />
                                                <dx:ToolbarListEditItem Text="Verdana" Value="Verdana" />
                                                <dx:ToolbarListEditItem Text="Arial" Value="Arial" />
                                                <dx:ToolbarListEditItem Text="MS Sans Serif" Value="MS Sans Serif" />
                                                <dx:ToolbarListEditItem Text="Courier" Value="Courier" />
                                            </Items>
                                        </dx:ToolbarFontNameEdit>
                                        <dx:ToolbarFontSizeEdit>
                                            <Items>
                                                <dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
                                                <dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
                                                <dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
                                                <dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
                                                <dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
                                                <dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
                                                <dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
                                            </Items>
                                        </dx:ToolbarFontSizeEdit>
                                        <dx:ToolbarBoldButton></dx:ToolbarBoldButton>
                                        <dx:ToolbarItalicButton></dx:ToolbarItalicButton>
                                        <dx:ToolbarUnderlineButton></dx:ToolbarUnderlineButton>
                                        <dx:ToolbarStrikethroughButton></dx:ToolbarStrikethroughButton>
                                        <dx:ToolbarBackColorButton></dx:ToolbarBackColorButton>
                                    </Items>
                                </dx:HtmlEditorToolbar>
                            </Toolbars>
        </dx:ASPxHtmlEditor>


        </div>
      <div class="row wikiNew-btnWrap">
        <div id="btncreate"></div>
          <div id="btnSave"></div>
    </div>
</div>



<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var GroupsData = [];
    var AddWikiRequestModel = {};
    var wikiarticle = {};
    var wikiPermission = {};

    var TicketId = '<%= TicketId%>';
    var strAction = '<%= strAction%>';

    //while edit wikiarticle

    $(document).ready(function () {
        if (TicketId != null && strAction == "edit") {

            var btnCreate = $("#btncreate").dxButton("instance");
            btnCreate.option("visible", false);

            $.get("/api/WikiArticles/GetWikiArticleByTicketId?id= " + TicketId, function (data, status) {
                wikiarticle = data;


                var textbox = $('#wikiTitle').dxTextBox("instance");
                textbox.option("value", wikiarticle.Title);

                var moduleSelectBox = $("#ddlModule").dxSelectBox("instance");
                moduleSelectBox.option("value", wikiarticle.ModuleNameLookup);



                var requestTypeSelectBox = $("#ddlRequestType").dxSelectBox("instance");
                requestTypeSelectBox.option("value", wikiarticle.RequestTypeLookup);

                //var grid = $("#ddlRequestType").dxSelectBox("instance");
                //grid.option("dataSource", GroupsData);

            });

        }
        else {
            var btnSave = $("#btnSave").dxButton("instance");
            btnSave.option("visible", false);

        }
    });


    $("#wikiTitle").dxTextBox({

        placeholder: "Enter Wiki Title",
        onValueChanged: function (e) {

            AddWikiRequestModel.WikiTitle = e.value;
            if (AddWikiRequestModel.WikiTitle != undefined) {
                $("#validateTitle").html("");

            }

        },
    });


    //    $(function() {

    //        $("#selectBox").dxSelectBox({
    //        dataSource: ["item1","item2","item3"] ,
    //        placeholder: "Enter Module",
    //        searchEnabled: true
    //    });

    //});

    $.get("/api/WikiArticles/GetDdlModules", function (modules, status) {



        // modulesData = modules 

        $("#ddlModule").dxSelectBox({
            placeholder: "Select Module",
            dataSource: modules,
            valueExpr: "ModuleName",
            displayExpr: "Title",
            onValueChanged: function (e) {
                AddWikiRequestModel.Module = e.value;
                if (AddWikiRequestModel.Module != undefined) {
                    $("#validateModule").html(" ");
                }

                $.get("/api/WikiArticles/GetDdlRequestType?moduleName= " + e.value, function (data, status) {
                    GroupsData = data;
                    var grid = $("#ddlRequestType").dxSelectBox("instance");
                    grid.option("dataSource", GroupsData);

                });
            }
        });

    });



    $("#ddlRequestType").dxSelectBox({
        placeholder: "Select  Request Type ",
        dataSource: GroupsData,
        valueExpr: "ID",
        displayExpr: "RequestType",
        onValueChanged: function (e) {
            // alert('onchangevent');
            AddWikiRequestModel.RequestType = e.value;
            if (AddWikiRequestModel.RequestType != undefined) {
                $("#validateRequestType").html(" ");
            }

        }
    });

    $("#btncreate").dxButton({
        text: "Create",
        onClick: function (e) {
            var allowCreate = true;
            var myText = htmlEditor.GetHtml();
            AddWikiRequestModel.HtmlBody = myText;
            if (AddWikiRequestModel.WikiTitle == undefined) {
                allowCreate = false;
                $("#validateTitle").html("<span style='color:red'>Enter title</span>");
            }
            else if (AddWikiRequestModel.Module == undefined) {
                allowCreate = false;
                $("#validateModule").html("<span style='color:red'>Enter module</span>");
            }
            else if (AddWikiRequestModel.RequestType == undefined) {
                allowCreate = false;
                $("#validateRequestType").html("<span style='color:red'> Enter request type</span>")
            }
            if (allowCreate == true) {
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(AddWikiRequestModel),
                    url: "/api/WikiArticles/CreateWiki",
                    contentType: "application/json",
                    success: function () {
                       window.parent.CloseWindowCallback(1, document.location.href);
                    }
                });

                //$.post("/api/WikiArticles/CreateWiki?" + $.param(AddWikiRequestModel));
                //window.parent.CloseWindowCallback(1, document.location.href);
                
            }
        }
    });

    $("#btnSave").dxButton({
        text: "Save",
        onClick: function (e) {
            $(this).parent(".ui-dialog.ui-corner-all").dialog('close');
            var allowSave = true;
            var myText = htmlEditor.GetHtml();
            AddWikiRequestModel.HtmlBody = myText;
            AddWikiRequestModel.TicketId = TicketId;

            if (AddWikiRequestModel.WikiTitle == undefined) {
                allowSave = false;
                $("#validateTitle").html("<span style='color:red'>Enter title</span>");
            }
            else if (AddWikiRequestModel.Module == undefined) {
                allowSave = false;
                $("#validateModule").html("<span style='color:red'>Enter module</span>");
            }
            else if (AddWikiRequestModel.RequestType == undefined) {
                allowSave = false;
                $("#validateRequestType").html("<span style='color:red'> Enter request type</span>")
            }

            if (allowSave == true) {
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(AddWikiRequestModel),
                    url: "/api/WikiArticles/UpdateWiki",
                    contentType: "application/json",
                    success: function () {
                       window.parent.CloseWindowCallback(1, document.location.href);
                        //alert(url);
                    }
                });
                //alert('1');
                //window.parent.CloseWindowCallbackForWiki(0, document.location.href);
                //var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails" + "&ticketId=" + TicketId;
                //window.parent.UgitOpenPopupDialog(url, "", "View Wiki Article", "98", "100", false, "");
                //window.parent.CloseWindowCallbackForWiki(0, document.location.href);
            }


            //$.post("/api/WikiArticles/UpdateWiki?" + $.param(AddWikiRequestModel));
            // $.post("/api/WikiArticles/UpdateWiki/AddWikiRequestModel");
            //location.reload();
            //window.parent.CloseWindowCallback(1, document.location.href);
        }
    });





</script>
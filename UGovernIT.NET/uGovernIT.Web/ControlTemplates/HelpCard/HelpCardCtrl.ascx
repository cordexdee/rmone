<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpCardCtrl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.HelpCardCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .showArchive-Helpcard {
        float: right;
        font-family: 'Poppins', sans-serif;
    }
    .HelpCardTiles .dx-scrollable-wrapper .editHelpCardInline {
        position: absolute;
        bottom: 10px;
        right: 10px;
    }
</style>
<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="helpCard-listWrap fleft">
        <div id="btnnew" class="dxtPrimary-btn"></div>
        <div id="duplicateHelpCard" class="dxtPrimary-btn"></div>
        <div id="btnDelete" class="dxtSecondary-btn"></div>
        <div id="btnArchive" class="dxtSecondary-btn"></div>
        <div id="btnPermission" class="dxtSecondary-btn"></div>                
        <div id="btnUnArchive" class="dxtSecondary-btn" ></div>
        <div id="showArchive" class="showArchive-Helpcard"></div>
        <div class="wiki-topDiv row">
            <div id="wikileftNavigation" class="col-md-3 col-sm-4 wikiNav-menuWrap flashCard-catList"></div>
            <div class="HelpCardTiles col-md-9 col-sm-9"></div>            
        </div>
        <br />
        <div class="wiki-btnDiv">
            <div id="btnloadMore" class="wiki-loadMoreBtn"></div>
            
        </div>
    </div>
    <div class="helpCard-templateWrap fright helpCard-display-container col-md-3 col-sm-3">   
         <div id="editHelpCard" class="edit-helpCard dxtPrimary-btn"></div>
		   <div class="helpCardPreview flashCard-container">
         </div>
   </div>
    
</div>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var Category = "All Categories";
    var ticketIdForEdit = '';
    var listCategory = {};
    var HelpCardTilesConfig = {};
    var gridDataSource = {};
    var arrOfCardsToBeDeleted = [];
    var arrOfSelectedTitles = [];
    var isShowArchive = false;
    var list;
    function openEditDialog(obj) {
        var WikiDetailsResponse = {};        
        var ticketId = obj.id;
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=helpcard" + "&action=add&ticketId=" + ticketId;
        window.parent.UgitOpenPopupDialog(url, "", "View Help Card", "95", "95", false, "");
    }


    $(function () {
        $(".helpCard-display-container").css("display", "none");
        $(".helpCard-listWrap").addClass("col-md-12 col-sm-12");
        drawHelpCardTiles();
        $(".helpCard-display-container").hide();
        drawCategoryMenu(); 
        var btnUnArchive = $("#btnUnArchive").dxButton("instance");
        btnUnArchive.option("visible", false);
    });

    
    function drawCategoryMenu() {        
        $.get("/api/HelpCard/GetCategories?isShowArchive=" + isShowArchive, function (data, status) {
            
            listCategory = data;            
            listCategory != null ? listCategory.unshift("All Categories") : {};
            Category = (listCategory != null && jQuery.inArray(Category, listCategory) != -1) ? Category : "All Categories";                        
                  list =  $("#wikileftNavigation").dxList({
                        dataSource: listCategory,
                        noDataText: "",
                        selectionMode: "single",
                        selectedItem: Category,
                        itemTemplate: function (itemData, itemIndex, itemElement) {            
                            var html = new Array();
                            html.push("<div class='wikiMenu-itemWrap'>")
                            //html.push("<div class='wikiMenu-imgWrap'><img class='wikiMenu-img'></img></div>");
                            html.push("<div class='wikiMenu-infoWrap'>");
                            html.push("<div class='wikiMenu-title'>" + itemData + "</div>");
                            html.push("<div class='wikiMenu-count'></div>");
                            html.push("</div>");
                            html.push("</div>")
                            itemElement.append(html.join(""));
                        },
                        onItemClick: function (e) {                     
                                     arrOfCardsToBeDeleted = [];
                                     Category = e.itemData;
                                     gridDataSource.reload();                     
                                    $(".helpCard-display-container").css("display", "none");
                                    $(".helpCard-listWrap").removeClass("col-md-9 col-sm-9");
                                     $(".helpCard-listWrap").addClass("col-md-12 col-sm-12");

                                     drawHelpCardTiles();
                        }

                    }).dxList('instance');
                         
        });

    }
   
    
    function drawHelpCardTiles()
    {
        
         gridDataSource = new DevExpress.data.DataSource({
                key: "ID",
             load: function (loadOptions) {                 
                 return $.getJSON("/api/HelpCard/GetHelpCards?Category=" + Category + "&isShowArchive=" + isShowArchive, function (data) {                                          
                     if ( (data.length == 0 || data == null)) {
                         Category != "All Categories";                         
                         //$.getJSON("/api/HelpCard/GetHelpCards?Category=" + Category + "&isShowArchive=" + isShowArchive, function (data1) {
                         //    return data1;
                         //});
                     }
                     return data;
                    });
                },
                insert: function (values) {

                },
                remove: function (key) {

                },
                update: function (key, values) {
                    
                }
            });

        HelpCardTilesConfig =
        {          
            noDataText: "No cards available",
            dataSource: gridDataSource,
            height: 600,
            //width: 600,
            paginate: true,
            direction: "vertical",
              allowItemDeleting: true,
            showScrollbar:true,
            baseItemHeight: 185,
            baseItemWidth: 185,
                    itemTemplate: function (itemData, itemIndex, itemElement) {
                        
                        itemElement.append(
                            $(`<div class='chkFilterCheckFlashCard' data-helpcard-id='${itemData.HelpCardTicketId}' data-helpcard-title='${itemData.HelpCardTitle}' />`).dxCheckBox({
                                value: false,
                                onValueChanged: function (objcheckbox) {
                                    
                                    var helpCardTicketId = objcheckbox.element.attr('data-helpcard-id');
                                    
                                    var helpCardTitle = objcheckbox.element.attr('data-helpcard-title');
                                    if (objcheckbox.value) {
                                        arrOfCardsToBeDeleted.push(helpCardTicketId);
                                        arrOfSelectedTitles.push(helpCardTitle);
                                    }
                                    else {
                                        arrOfCardsToBeDeleted = arrOfCardsToBeDeleted.filter(e => e !== helpCardTicketId);
                                        arrOfSelectedTitles = arrOfSelectedTitles.filter(e => e !== helpCardTitle);
                                    }
                                    objcheckbox.event.stopPropagation();
                                }

                            }),

                            $("<div class='row wikiTitle-container'></div>"),
                            $("<div class='favorite-imgWrap' />"),
                            $(`<div class='helpCard-titleWrap' >${itemData.HelpCardTitle}</div>`),


                            $(`<div class='wilkiInfo-container'>Created on ${itemData.Created}</div>`),

                            $(`<div class='wilkiInfo-container'>By ${itemData.CreatedBy}</div>`),

                            $(`<div class='editHelpCardInline edit-help-btn-inline' data-helpcard-id=${itemData.HelpCardTicketId}></div>`).dxButton({
                                icon: '/Content/images/editNewIcon.png',
                                hoverStateEnabled: false,
                                focusStateEnabled: false,
                                onClick: function (e) {
                                    var ticketId = e.element.attr('data-helpcard-id');
                                    var url = "/layouts/ugovernit/delegatecontrol.aspx?control=helpcard" + "&action=edit&ticketId=" + ticketId;
                                    window.parent.UgitOpenPopupDialog(url, "", "Edit Help Card", "95", "95", false, "");
                                    e.event.stopPropagation();
                                }
                            }),
                        );

            },
            onContentReady: function (obj) {
                 
                                       
            },
            onItemClick: function (obj) {
                
                if (obj != null && obj.itemData != null) {
                    obj.itemElement.addClass("activeHelpCard");
                    $(".helpCardPreview").html(obj.itemData.HelpCardContent + (obj.itemData.AgentContent ? obj.itemData.AgentContent : ''));
                    $(".helpCard-display-container").css("display", "block");
                    $(".helpCard-listWrap").addClass("col-md-9 col-sm-9");
                    $(".helpCard-listWrap").removeClass("col-md-12 col-sm-12");                    
                    obj.component.repaint();
                    ticketIdForEdit = obj.itemData.HelpCardTicketId;

                }
            }

        };

        $(".HelpCardTiles").dxTileView(HelpCardTilesConfig);

       
    }
    
    $("#showArchive").dxCheckBox({
        value: false,
        width: 80,
        text: "Show Archive",
        onValueChanged: function (obj) {
            var btnArchive = $("#btnArchive").dxButton("instance");
            var btnUnArchive = $("#btnUnArchive").dxButton("instance");            
            if (obj != null && obj.value == true) {               
                btnArchive.option("visible", false);
                btnUnArchive.option("visible", true);
                isShowArchive = true;                                 
                refreshPage();
            }
            else {                
                btnArchive.option("visible", true);               
                btnUnArchive.option("visible", false);
                isShowArchive = false;                
                refreshPage();
            }
            arrOfCardsToBeDeleted = [];            
        }
    });

    $("#btnArchive").dxButton(
        {
            text : "Archive",            
            onClick: function (e) {
                if (arrOfCardsToBeDeleted.length == 0) {
                    var result = DevExpress.ui.dialog.alert('No selected help cards?', 'Archive');
                    result.done(function () { return });
                }
                else {
                    var result = DevExpress.ui.dialog.confirm('Are you sure you want to archive selected help cards ? ', 'Confirm');
                    result.done(function (confirmation) {
                        if (confirmation) {
                            if (confirmation) {
                                arrOfCardsToBeDeleted.forEach((function (ticketId, index, arr) {
                                    $.post("/api/HelpCard/ArchiveHelpCard?ticketId=" + ticketId, function (data, status) {
                                        if (index == arrOfCardsToBeDeleted.length - 1) {
                                            refreshPage();
                                        }
                                    });
                                }));
                            }
                        }
                    });
                }
            }

        });
           
    $("#btnUnArchive").dxButton(
        {
           text :"UnArchive", 
            onClick: function (e) {
                if (arrOfCardsToBeDeleted.length == 0) {
                    var result = DevExpress.ui.dialog.alert('No selected help cards?', 'UnArchive');
                    result.done(function () { return });
                }
                else
                {
                     var result = DevExpress.ui.dialog.confirm('Are you sure you want to unArchive this help cards?', 'Confirm');
                    result.done(function (confirmation) {
                        if (confirmation) {
                            arrOfCardsToBeDeleted.forEach((function (ticketId, index, arr) {
                                $.post("/api/HelpCard/UnArchiveHelpCard?ticketId=" + ticketId, function (data, status) {
                                    if (index == arrOfCardsToBeDeleted.length -1)
                                    {
                                        refreshPage();
                                    }
                                });                            
                            }));                 
                                    
                         }
                    });
                }

                  


            }
        });
    
    $("#btnDelete").dxButton({
        text: "Delete",
        onClick: function (e)
        {
            if (arrOfCardsToBeDeleted.length == 0) {
                var result = DevExpress.ui.dialog.alert('No selected help cards?', 'Delete');
                result.done(function () { return });
            }
            else {
                var ttiles = arrOfSelectedTitles.join("<BR>");
                var result = DevExpress.ui.dialog.confirm('Are you sure you want to delete the selected help cards ? <br>' + ttiles, 'Confirm');
                result.done(function (confirmation) {
                    if (confirmation) {
                        arrOfCardsToBeDeleted.forEach((function (ticketId, index, arr) {

                            $.post("/api/HelpCard/DeleteHelpCard?ticketId=" + ticketId, function (data, status) {
                                if (index == arrOfCardsToBeDeleted.length - 1) {
                                    refreshPage();
                                }
                            });
                        }));
                    }
                });
            }
        }

    });

    $("#duplicateHelpCard").dxButton({
        text: "Duplicate",
        onClick: function (e) {
            if (arrOfCardsToBeDeleted.length == 0) {
                var result = DevExpress.ui.dialog.alert('No selected help cards?', 'Duplicate');
                result.done(function () { return });
            }
            else {
                  var result = DevExpress.ui.dialog.confirm('Are you sure you want to duplicate selected help cards ? ', 'Confirm');
                  result.done(function (confirmation) {
                      if (confirmation) {
                          if (confirmation) {                              
                              arrOfCardsToBeDeleted.forEach((function (ticketId, index, arr) {
                                 
                                  $.post("/api/HelpCard/DuplicateHelpCard?duplicateTicketId=" + ticketId, function (data, status) {
                                      if (index == arrOfCardsToBeDeleted.length -1)
                                      {
                                        refreshPage();
                                      }
                                  });
                              }));
                                                             
                          }
                      }
                  });
            }

        }
    })

    $(function () {
        $("#btnnew").dxButton({
        text: "New",
        onClick: function (e) { 
           // DevExpress.ui.notify("The OK button was clicked");
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=helpcard&action=add";        
        window.parent.UgitOpenPopupDialog(url, "", "New Help Card", "95", "95", false, "");

        }
    });
    });
    
    $("#editHelpCard").dxButton({
        text: "Edit ",
        onClick: function (e) {            
           var url = "/layouts/ugovernit/delegatecontrol.aspx?control=helpcard" + "&action=edit&ticketId=" + ticketIdForEdit;
            window.parent.UgitOpenPopupDialog(url, "", "Edit Help Card", "95", "95", false, "");            
        }
    });
   
    function refreshPage() {
        
        Category = list.option("selectedItem");       
        drawCategoryMenu();
        gridDataSource.reload();        
        var tileview = $(".HelpCardTiles").dxTileView(HelpCardTilesConfig).dxTileView('instance');
        tileview.repaint();
        arrOfCardsToBeDeleted = [];
        
        $(".helpCard-display-container").css("display", "none");
        $(".helpCard-listWrap").removeClass("col-md-9 col-sm-9");
        $(".helpCard-listWrap").addClass("col-md-12 col-sm-12");
    }
</script>

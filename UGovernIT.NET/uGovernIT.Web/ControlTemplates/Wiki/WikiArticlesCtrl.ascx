<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiArticlesCtrl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Wiki.WikiArticlesCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .favorite-imgWrap{
        /*display:inline-block;*/
        margin-right:3px;
        padding-left: 5px;
    }
    .wikiTitle-wrap{
        /*display:inline-block;*/
        font-family: 'Poppins', sans-serif !important;
        font-size: 13px;
        font-weight: 600;
        padding-left: 10px;
        padding-right: 0px;
    }
        .wikiTitle-wrap .wikiTitleAnchor {
            cursor: pointer;
        }

    .wiki-likes{
        display:inline-block;
        margin-right:10px;
    }
    .wiki-likes img{
        margin-right: 3px;
    }
    .wiki-disLikes img{
         margin-right: 3px;
    }
    .wiki-disLikes{
        display:inline-block;
        margin-right:5px;
    }
    .wikiTitle-container{
        margin-bottom:10px;
        margin-top: 6px;
    }
    .wilkiInfo-container{
        margin-left:36px;
        margin-bottom:5px;
    }
    .likeDislike-container{
        position: absolute;
        bottom: 7px;
        right: 5px;
    }
    .wiki-loadMoreBtn{
        float:right;
        background: none;
        /*border: 1px solid #4A6EE2;*/
        border:none;
        color: #4A6EE2;
        font: 14px 'Poppins', sans-serif;
        font-weight: 600;
    }
    .wiki-loadMoreBtn.dx-state-hover{
        background:none;
        border-bottom:2px solid #4A6EE2;    
        border-radius:0px;
    }
    .wiki-newBtn{
        float:right;
        background: #4A6EE2;
        border: 1px solid #4A6EE2;
        color: #fff;
        font: 14px 'Poppins', sans-serif;
        font-weight: 600;
        margin-right: 10px;
    }
    .wiki-newBtn.dx-state-hover{
        color:#fff;
        background-color:#4A6EE2;
    }
    .wiki-loadMoreBtn.dx-button-content{
            padding: 3px 13px 0px;
    }
    .noleftPadding{
        padding-left:0px;
    }
    .demoTree-container{
        margin-top:20px;
    }
    .wikiHome-search-wrap{
        padding-left:20px;
    }
    .treeView.dx-treeview-with-search > .dx-scrollable{
        background: #FFF;
        border: 1px solid #c3c2c2aa;
        border-radius: 4px;
        height: 619px;
        /*overflow-y: scroll;*/
        position: relative;
    }
    /*.dx-treeview-item{
        padding: 5px 6px;
        min-height: 32px;
        color: #4A6EE2;
        font: 14px 'Poppins', sans-serif;
        font-weight: 600;
    }*/

    .wikiMenu-itemWrap{
        text-align:center;  
    }
    .wikiNav-menuWrap{
        width: 120px;
        display: inline-block;
        vertical-align: top;
        border-right: 1px solid #f5f5f5;
    }
    .wikiMenu-imgWrap{
        width:100%;
        text-align: center;
    }
    .ugitWikiArticles{
        display:inline-block;
        width:auto;
    }
    .wiki-topDiv, .wiki-btnDiv{
        display:block;
    }
    .wikiMenu-infoWrap{
      text-align:center;
    }
    .ugitWikiArticles .dx-scrollable-container .dx-item.dx-tile {
        box-shadow: 3px 5px 5px rgba(0, 0, 0, 0.2);
    }
    .wikiMenu-title, .wikiMenu-count{
        display:inline-block;
        margin-right: 5px;
        font-family: 'Poppins', sans-serif;
        font-size: 12px;
        font-weight: 500
    }
    .wikiMenu-imgWrap img.wikiMenu-img{
        width:50px;
    }
</style>

<div class="row">
<%--    <div class="col-md-4 col-sm-6 col-xs-12 wikiHome-search-wrap">
        <div id="selectBoxContainer"></div>
    </div>--%>
</div>

<div class="row">
    <div class="col-md-9 col-sm-8 col-xs-12 noleftPadding">
        <div class="wiki-topDiv">
            <div id="wikileftNavigation" class="wikiNav-menuWrap"></div>
            <div class="ugitWikiArticles"></div>
        </div>
        <div class="wiki-btnDiv">
            <div id="btnloadMore" class="wiki-loadMoreBtn"></div>
            <div id="btnnew" class="wiki-newBtn"></div>
        </div>
    </div>
    <div class="col-md-3 col-sm-4 col-xs-12">
        <div class="demoTree-container">
            <div id="treeview" class="treeView"></div>
        </div>
    </div>
</div>

<div id="selectBox"></div>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    var obj = {};
    $(document).ready(function () {

                  obj.Action == "add";
                  // wikiPermission.TicketId=TicketId
        
             $.post(ugitConfig.apiBaseUrl + "/api/WikiArticles/GetPermissions?Action=add&ticketId= ", function (data, status) {
               
                    if (data.AuthorizedToView == false) {

                        var btnnew = $("#btnnew").dxButton("instance");
                        btnnew.option("visible", false);
                    }
                    else
                    {
                    var btnSave = $("#btnnew").dxButton("instance");
                    btnSave.option("visible", true);

                    }

              });


    });

    $.get("/api/WikiArticles/GetWikiMenuLeftNavigation", function (data, status) {
        if (data.length == 0) {
                //var navigation = $("#wikileftNavigation").dxList("instance");
                //navigation.option("visible", false);
                $('#wikileftNavigation').attr("style", "display:none");
                    //navigation.option("Disabled", true);

        }
        if (data.length > 0) {
        $("#wikileftNavigation").dxList({
            dataSource: data,
            noDataText: " ",
            // allowItemDeleting: true,
            // itemDeleteMode: "static" ,

            itemTemplate: function (itemData, itemIndex, itemElement) {
                var html = new Array();
                html.push("<div class='wikiMenu-itemWrap'>")
                html.push("<div class='wikiMenu-imgWrap'><img class='wikiMenu-img' src=" + itemData.ImageUrl + "></img></div>");
                html.push("<div class='wikiMenu-infoWrap'>");
                html.push("<div class='wikiMenu-title'>" + itemData.Title + "</div>");
                html.push("<div class='wikiMenu-count'>(" + itemData.ItemOrder + ")</div>");
                html.push("</div>");
                html.push("</div>")
                itemElement.append(html.join(""));
            },
            onItemClick: function (e) {
                var itemData = e.itemData;
                
                  $.get("/api/WikiArticles/GetWikiNavigationList?navigationType=" +itemData.ColumnType+ "&navigationId="+itemData.ID , function (data, status) {

                       gridDataSource = data;
                    var wikiArticlesConfig = {
                        noDataText: "No articles available",
                        dataSource: data,
                        direction: "vertical",
                        showScrollbar: true,
                        height: 900,
                        //width: 100%,
                        baseItemHeight: 140,
                        baseItemWidth: 250,
                        searchPanel: {
                                visible: true
                            },

                        itemTemplate: function (itemData, itemIndex, itemElement) {
                            var html = new Array();
                            html.push("<div class='row wikiTitle-container'>");
                            html.push("<div class='favorite-imgWrap col-xs-1'>");
                            if (itemData.WikiFavorites == true) {
                                html.push("<img src='/content/ButtonImages/Favorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            else if (itemData.WikiFavorites == false) {
                                html.push("<img src='/content/ButtonImages/UnFavorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            html.push("</div>");
                            html.push("<div class='wikiTitle-wrap col-xs-10'>");
                            html.push("<a class='wikiTitleAnchor' id="+itemData.WikiTicketId+" onclick ='openEditDialog(this);'>"+itemData.Title+"</a>" );

                           // html.push("<a onclick ="openEditDialog(itemData.Title);">"+itemData.Title "</a>" );
                            html.push("</div>");
                            html.push("</div>");

                            //html.push("<div>")
                            //html.push("Views:"+itemData.WikiViews);
                            //html.push("</div>")

                            //html.push("<div class='dv-inline'>")

                            //html.push("<div>")
                            //html.push("Service Requests"+itemData.WikiServiceRequestCount);
                            //html.push("</div>")
                            //html.push("<div>")
                            //html.push("Discussion Notes:"+itemData.WikiDiscussionCount);
                            //html.push("</div>")

                            //html.push("<div>")
                            //html.push("Knowledge Links:"+itemData.WikiLinksCount);
                            //html.push("</div>")
                            //html.push("</div>")
                            html.push("<div class='wilkiInfo-container'>");
                            html.push("Created on " + itemData.Created);
                            html.push("</div>");

                            html.push("<div class='wilkiInfo-container'>");
                            html.push("By " + itemData.CreatedByUser);
                            html.push("</div>");


                            html.push("<div class='row likeDislike-container'>");
                            html.push("<div class='wiki-likes'>");
                            html.push("<img src='/content/Images/approve.png' style='width:16px;cursor:pointer;'></img>" + itemData.WikiLikesCount);
                            html.push("</div>");

                            html.push("<div class='wiki-disLikes'>");
                            html.push("<img src='/content/Images/reject.png' style='width:16px;cursor:pointer;'></img>" + itemData.WikiDislikesCount);
                            html.push("</div>");
                            html.push("</div>");


                            itemElement.append(html.join(""));

                        },

                    };
                    $(".ugitWikiArticles").dxTileView(wikiArticlesConfig);



              });

            }

        });

        }

    });

    //var gridDataSource = new DevExpress.data.DataSource({
    //        key: "ID",
    //        //paginate: true,
    //        //pageSize: 10,
    //        load: function(loadOptions) {
    //            var d = $.Deferred(),
    //                    params = {};
    //            [
    //                "skip",     
    //                "take", 
    //                "requireTotalCount", 
    //                "requireGroupCount", 
    //                "sort", 
    //                "filter", 
    //                "totalSummary", 
    //                "group", 
    //                "groupSummary"
    //            ].forEach(function(i) {
    //                if(i in loadOptions && isNotEmpty(loadOptions[i])) 
    //                    params[i] = JSON.stringify(loadOptions[i]);
    //            });
    //            $.getJSON("/api/WikiArticles/Getwikies", params)
    //                .done(function(result) {
    //                    d.resolve(result, { 
    //                        //totalCount: result.totalCount,
    //                        //summary: result.summary,
    //                        //groupCount: result.groupCount
    //                    });
    //                });
    //            return d.promise();
    //        }
    //    });




     var gridDataSource = [];
     var pageIndex =1;

     $.get(ugitConfig.apiBaseUrl + "/api/WikiArticles/Getwikies?pageIndex= " + pageIndex, function (wikies, status) {

                 gridDataSource = wikies;
                    var wikiArticlesConfig = {
                        noDataText: "No articles available",
                        dataSource: wikies,
                        //direction: "vertical",
                        showScrollbar: true,
                        height: 900,
                       // width: 100,
                        baseItemHeight: 150,
                        baseItemWidth: 300,
                        searchPanel: {
                                visible: true
                            },

                        itemTemplate: function (itemData, itemIndex, itemElement) {
                            var html = new Array();
                            html.push("<div class='row wikiTitle-container'>");
                            html.push("<div class='favorite-imgWrap col-xs-1'>");
                            if (itemData.WikiFavorites == true) {
                                html.push("<img src='/content/ButtonImages/Favorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            else if (itemData.WikiFavorites == false) {
                                html.push("<img src='/content/ButtonImages/UnFavorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            html.push("</div>");
                            html.push("<div class='wikiTitle-wrap col-xs-10'>");
                            html.push("<a class='wikiTitleAnchor' id="+itemData.WikiTicketId+" onclick ='openEditDialog(this);'>"+itemData.Title+"</a>" );

                           // html.push("<a onclick ="openEditDialog(itemData.Title);">"+itemData.Title "</a>" );
                            html.push("</div>");
                            html.push("</div>");

                            //html.push("<div>")
                            //html.push("Views:"+itemData.WikiViews);
                            //html.push("</div>")

                            //html.push("<div class='dv-inline'>")

                            //html.push("<div>")
                            //html.push("Service Requests"+itemData.WikiServiceRequestCount);
                            //html.push("</div>")
                            //html.push("<div>")
                            //html.push("Discussion Notes:"+itemData.WikiDiscussionCount);
                            //html.push("</div>")

                            //html.push("<div>")
                            //html.push("Knowledge Links:"+itemData.WikiLinksCount);
                            //html.push("</div>")
                            //html.push("</div>")
                            html.push("<div class='wilkiInfo-container'>");
                            html.push("Created on " + itemData.Created);
                            html.push("</div>");

                            html.push("<div class='wilkiInfo-container'>");
                            html.push("By " + itemData.CreatedByUser);
                            html.push("</div>");


                            html.push("<div class='row likeDislike-container'>");
                            html.push("<div class='wiki-likes'>");
                            html.push("<img src='/content/Images/approve.png' style='width:16px;cursor:pointer;'></img>" + itemData.WikiLikesCount);
                            html.push("</div>");

                            html.push("<div class='wiki-disLikes'>");
                            html.push("<img src='/content/Images/reject.png' style='width:16px;cursor:pointer;'></img>" + itemData.WikiDislikesCount);
                            html.push("</div>");
                            html.push("</div>");


                            itemElement.append(html.join(""));

                        },

                    };
                    $(".ugitWikiArticles").dxTileView(wikiArticlesConfig);


                });


    function isNotEmpty(value) {
        return value !== undefined && value !== null && value !== "";
    }
    //gridDataSource.pageIndex(1);
    //gridDataSource.load();
 
       var wikiArticlesConfig = {
          
        noDataText: "No articles available",
        dataSource: gridDataSource,
        height: 900,
        //width: 810,
        paginate: true,
        showScrollbar:true,
        baseItemHeight: 140,
        baseItemWidth: 180,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var html = new Array();
                        html.push("<div class='row wikiTitle-container'>");
                            html.push("<div class='favorite-imgWrap'>");
                            if (itemData.WikiFavorites == true)
                            {
                                html.push("<img src='/content/ButtonImages/Favorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            else if(itemData.WikiFavorites == false)
                            {
                                html.push("<img src='/content/ButtonImages/UnFavorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            html.push("</div>");
                            html.push("<div class='wikiTitle-wrap'>");
                            html.push(itemData.Title );
                            html.push("</div>");
                        html.push("</div>");
                       

                        html.push("<div class='wilkiInfo-container'>");
                        html.push("Created on " +itemData.Created);
                        html.push("</div>");

                        html.push("<div class='wilkiInfo-container'>");
                        html.push("By " +itemData.CreatedByUser);
                        html.push("</div>");


                        html.push("<div class='row likeDislike-container'>");
                            html.push("<div class='wiki-likes'>");
                                html.push("<img src='/content/ButtonImages/thumbs_up_20x20.png' style='width:18px;cursor:pointer;'></img>"+itemData.WikiLikesCount);
                            html.push("</div>");

                            html.push("<div class='wiki-disLikes'>");
                                html.push("<img src='/content/ButtonImages/thumbs_down_20x20.png' style='width:18px;cursor:pointer;'></img>"+itemData.WikiDislikesCount);
                            html.push("</div>");
                    html.push("</div>");

              
                itemElement.append(html.join(""));

            },

    };


       $(".ugitWikiArticles").dxTileView(wikiArticlesConfig);


       var modulesData = [];
       var requestModel = { };


         $.get(ugitConfig.apiBaseUrl + "/api/WikiArticles/GetModules", function (dataOfTree, status) {
         
                //modulesData = data;
                //var jsonData = JSON.stringify(ModulesData);
                  
            $(function(){
                var treeView = $("#treeview").dxTreeView({
                    items: dataOfTree,
                    width: 300,
                   searchEnabled: true,
                   selectionMode:"single",
                   //showCheckBoxesMode: true,
                   //selectNodesRecursive:false,
                   onItemClick : function(e) 
                      {
                      // alert(e.itemData.Id);
                       requestModel.Id = parseInt(e.itemData.Id);
                       //alert("demo= "+ $.param(requestModel));
                       //alert("test="+$.param(requestModel.Id));
                       $.get(ugitConfig.apiBaseUrl + "/api/WikiArticles/GetwikiesByRequestType?id=" +e.itemData.Id  , function (wikies, status) {

                              var wikiArticlesConfig = {
                                noDataText: "No articles available",
                                dataSource: wikies,
                                showScrollbar: true,
                                height: 900,
                                //width: 810,
                                baseItemHeight: 150,
                                baseItemWidth: 280,
                         itemTemplate: function (itemData, itemIndex, itemElement) {
                            var html = new Array();
                            html.push("<div class='row wikiTitle-container'>");
                            html.push("<div class='favorite-imgWrap col-xs-1'>");
                            if (itemData.WikiFavorites == true) {
                                html.push("<img src='/content/ButtonImages/Favorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            else if (itemData.WikiFavorites == false) {
                                html.push("<img src='/content/ButtonImages/UnFavorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            html.push("</div>");
                            html.push("<div class='wikiTitle-wrap col-xs-10'>");
                            html.push("<a class='wikiTitleAnchor' id="+itemData.WikiTicketId+" onclick ='openEditDialog(this);'>"+itemData.Title+"</a>" );

                           // html.push("<a onclick ="openEditDialog(itemData.Title);">"+itemData.Title "</a>" );
                            html.push("</div>");
                            html.push("</div>");


                            html.push("<div class='wilkiInfo-container'>");
                            html.push("Created on " + itemData.Created);
                            html.push("</div>");

                            html.push("<div class='wilkiInfo-container'>");
                            html.push("By " + itemData.CreatedByUser);
                            html.push("</div>");


                            html.push("<div class='row likeDislike-container'>");
                            html.push("<div class='wiki-likes'>");
                            html.push("<img src='/content/ButtonImages/thumbs_up_20x20.png' style='width:18px;cursor:pointer;'></img>" + itemData.WikiLikesCount);
                            html.push("</div>");

                            html.push("<div class='wiki-disLikes'>");
                            html.push("<img src='/content/ButtonImages/thumbs_down_20x20.png' style='width:18px;cursor:pointer;'></img>" + itemData.WikiDislikesCount);
                            html.push("</div>");
                            html.push("</div>");


                            itemElement.append(html.join(""));

                        },

    };
                              $(".ugitWikiArticles").dxTileView(wikiArticlesConfig);


                       });

                  




                       // $.get("/api/WikiArticles/Getwikies", function (gridDataSource, status)
                            //        var item = e.node;
    
                            //        if(isProduct(item)) {
                            //            processProduct($.extend({
                            //                category: item.parent.text
                            //            }, item));
                            //        } 
                            //    else {
                            //            $.each(item.items, function(index, dataOfTree) {
                            //                processProduct($.extend({
                            //                    category: item.text
                            //                }, dataOfTree));
                            //            });
                            //        }
                            //checkedItemsList.option("items", checkedItems);
                    },

            }).dxTreeView("instance");

            $("#searchMode").dxSelectBox({
                items: ["contains", "startswith"],
                value: "contains",
                onValueChanged: function(data) {
                    treeView.option("searchMode", data.value);
                }
            });
                });


        });

    
 
        $(function () {
            $("#btnloadMore").dxButton({
                text: "Load More...",
                onClick: function (e) { 
                     pageIndex++;
                   // DevExpress.ui.notify("The OK button was clicked");
                    $.get(ugitConfig.apiBaseUrl + "/api/WikiArticles/Getwikies?pageIndex= " + pageIndex, function (wikies, status) {
                        gridDataSource.push(wikies);
                    var wikiArticlesConfig = {
                        noDataText: "No articles available",
                        dataSource: wikies,
                        showScrollbar: true,
                        height: 900,
                        width: 810,
                        baseItemHeight: 150,
                        baseItemWidth: 280,
                                                itemTemplate: function (itemData, itemIndex, itemElement) {
                            var html = new Array();
                            html.push("<div class='row wikiTitle-container'>");
                            html.push("<div class='favorite-imgWrap col-xs-1'>");
                            if (itemData.WikiFavorites == true) {
                                html.push("<img src='/content/ButtonImages/Favorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            else if (itemData.WikiFavorites == false) {
                                html.push("<img src='/content/ButtonImages/UnFavorite.png' style='width:18px;cursor:pointer;'></img>");

                            }
                            html.push("</div>");
                            html.push("<div class='wikiTitle-wrap col-xs-10'>");
                            html.push("<a class='wikiTitleAnchor' id="+itemData.WikiTicketId+" onclick ='openEditDialog(this);'>"+itemData.Title+"</a>" );

                           // html.push("<a onclick ="openEditDialog(itemData.Title);">"+itemData.Title "</a>" );
                            html.push("</div>");
                            html.push("</div>");


                            html.push("<div class='wilkiInfo-container'>");
                            html.push("Created on " + itemData.Created);
                            html.push("</div>");

                            html.push("<div class='wilkiInfo-container'>");
                            html.push("By " + itemData.CreatedByUser);
                            html.push("</div>");


                            html.push("<div class='row likeDislike-container'>");
                            html.push("<div class='wiki-likes'>");
                            html.push("<img src='/content/ButtonImages/thumbs_up_20x20.png' style='width:18px;cursor:pointer;'></img>" + itemData.WikiLikesCount);
                            html.push("</div>");

                            html.push("<div class='wiki-disLikes'>");
                            html.push("<img src='/content/ButtonImages/thumbs_down_20x20.png' style='width:18px;cursor:pointer;'></img>" + itemData.WikiDislikesCount);
                            html.push("</div>");
                            html.push("</div>");


                            itemElement.append(html.join(""));

                        },

                    };
                    $(".ugitWikiArticles").dxTileView(wikiArticlesConfig);


                });

                }
            });
        });
   
    $(function () {
    $("#btnnew").dxButton({
        text: "New",
        onClick: function (e) { 
           // DevExpress.ui.notify("The OK button was clicked");
        var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiList&action=add";
        window.parent.UgitOpenPopupDialog(url, "", "New Wiki Article", "98", "98", false, "");

        }
    });
});

        $(function() {
            $("#selectBoxContainer").dxSelectBox({
              
       // placeholder: "Select a product..."
    });
});
      


    function openEditDialog(obj) {        
             var WikiDetailsResponse = {};
            //alert(obj.text);
            //alert($('.wikiTitleAnchor').attr('id'));
            //var ticketId = $('.wikiTitleAnchor').attr('id');
            var ticketId = obj.id;
           var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails" + "&ticketId=" + ticketId;
                   //  alert(url);

           window.parent.UgitOpenPopupDialog(url, "", "View Wiki Article", "98", "100", false, "");


            //$.get("/api/WikiArticles/GetwikiDetails?ticketId= " +ticketId, function (data, status) {
            //WikiDetailsResponse = data;
            //    $('#listLinks').text('test data');
        //});

            //var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails" + "&data=" + JSON.stringify(WikiDetailsResponse);
            // alert(url);
            //window.parent.UgitOpenPopupDialog(url, "", "View Wiki Article", "85", "100", false, "");

            //var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiDetails" + "&ticketId=" + ticketId;
            //alert(url);
            //window.parent.UgitOpenPopupDialog(url, "", "View Wiki Article", "85", "100", false, "");

<%--        var ticketId = $(obj).attr('ArticleId');
        var url = '<%=detailsUrl %>' + "&ticketId=" + ticketId;
        window.parent.UgitOpenPopupDialog(url, "", 'View Wiki Article', '90', '90', 0, escape("<%= Request.Url.AbsolutePath %>"));
        return false;--%>
    }


</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectKanBanView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.PMM.ProjectKanBanView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .demo-container {
        box-sizing: border-box;
        width: 100%;
        height: auto !important;
    }



#kanban {
    white-space: nowrap;
    height:100%;
}

.list {
    border-radius: 1px;
    margin: 1px;
    background-color: rgba(192, 192, 192, 0.4);
    display: inline-block;
    vertical-align: top;
    white-space: normal;
}


#kanban {
    white-space: nowrap;
}

.list {
    border-radius: 1px;
    margin: 10px;
    background-color: rgba(192, 192, 192, 0.4);
    display: inline-block;
    vertical-align: top;
    white-space: normal;
    width:18%;
    height:100%;
}

.list-title {
    font-size: 14px;
    padding: 3px;
    padding-left: 3px;
    /*margin-bottom: -10px;*/
    font-weight: bold;
    cursor: pointer;
    width:100%;
    background: fixed;
    color: #FFF !important;
}

.scrollable-list {
    height: 400px;
    width: 260px;
}

.sortable-cards {
    min-height: 380px
}

.card {
    position: relative;
    
    box-sizing: border-box;
    /*width: 230px;*/
    padding: 10px 20px;
    margin: 5px;
    cursor: pointer;
    border: none;
}

.card-subject {
    padding-bottom: 10px;
}

.card-assignee {
    opacity: 0.6;
}

.card-priority {
    position: absolute;
    top: 10px;
    bottom: 10px;
    left: 5px;
    width: 5px;
    border-radius: 2px;
    background: #86C285;
}



.priority-notstarted {
    background: #ADADAD;
}

.priority-inprogress{
    background: #EDC578;
}

.priority-completed {
    background: #86C285;
}

.priority-waiting {
    background: #EDC578;
}

.owner {
    display: table-cell;
}

.dx-treelist-headers{
    display: none;
}

.predecesser{
     background-image: url("~/Content/Images/redNew_delete.png");
}

.resource{

    max-width: 20px;
}


</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var Tasks;
    var categories = []; 
    var sExpand = "ParentTaskID";
    var listTasks = [];
    var bAllowAdding = true;
    var bActUser = false;
    var bAddNewTaskDisabled = true;
    var bDeletingAllow = true;
    var EditingAllow = true ;
 


  $(function () {
        RepaintTaskWorkflow();
    });
    
    function RepaintTaskWorkflow() {
        
        $.ajax({
            url: "/api/kanbanapi/GetTask?TicketID=PMM-19-000012",
            type: "GET",
            contentType: "application/json",
            success: function (data) {
                if (data != null) {

                    Tasks = data;
                    categories = Tasks.filter(function(Task) { return Task.Level == 0  });
                    DrawKanBan(Tasks);
                }
                else
                    DrawKanBan();
            }

        });

    }

//loadig panel start
var loadPanel = $("#loadpanel").dxLoadPanel({
            
            shadingColor: "rgba(0,0,0,0.4)",
            position: { of: "#kanban" },
            visible: false,
            showIndicator: true,
            showPane: true,
            shading: true,
            hideOnOutsideClick: false
        }).dxLoadPanel("instance");


/*console.log(loadPanel);*/
//Start DataSource    

 TreeListDataSource = new DevExpress.data.DataSource({
            key: "ID",
            paginate: false,
            load: function (loadOptions) {
                //debugger;
                //    console.log("for the tiem");
                return $.getJSON(apiUrl, function (data) {
                    var treeL = $("#tree-list").dxTreeList("instance");
                    if (data.length > 0) {
                        // If StartDate and DueDate are Min Date then set Null
                        $.each(data, function (index, value) {

                            if (value.StartDate == minDate)
                                value.StartDate = null;
                            if (value.DueDate == minDate)
                                value.DueDate = null;
                        });

                        if (treeL.hasEditData())
                            treeL.cancelEditData();
                    }
                    else {
                        if (showBaseline.toLowerCase()=="false" && bActUser.toLowerCase()=="true" && TaskParameter != 'My Tasks')
                        {
                                treeL.addRow();
                                treeL.addRow();
                                treeL.addRow();
                                treeL.addRow();
                                treeL.addRow();
                        }
                    }
                    
                    if (Y > 0) {
                        window.scrollTo(0, Y);
                        Y = 0;
                    }
                    
                    return data;
                });
            },
            insert: function (values) {
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "/api/module/InsertTask",
                    type: "POST",
                    data: { values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                    async: false,
                    success: function (data) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        treeL.refresh(true);
                        if (IsCompactView.toLowerCase()=="true")
                            RepaintTaskWorkflow();
                    }
                })
            },
            remove: function (key) {
                    /*console.log("remove")*/
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "/api/module/DeleteTask",
                    type: "DELETE",
                    data: { key: key, TicketId: '<%= TicketID%>' },
                    async: false,
                    success: function (data) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        treeL.refresh(true);
                        if (IsCompactView.toLowerCase()=="true")
                           RepaintTaskWorkflow();
                    }
                })
            },
            update: function (key, values) {
                // Set Min Date if StartDate or DueDate is null
                            debugger; 

                if (values.hasOwnProperty('StartDate') && values.StartDate == null)
                    values.StartDate = minDate;
                if (values.hasOwnProperty('DueDate') && values.DueDate == null)
                    values.DueDate = minDate;

                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "/api/module/UpdateTask",
                    type: "PUT",
                    data: { key: JSON.stringify(key), values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                    async: false,
                    success: function (data) {
                        var treeL = $("#tree-list").dxTreeList("instance");
                        treeL.refresh(true);
                        if (IsCompactView.toLowerCase()=="true")
                          RepaintTaskWorkflow();
                    }
                })
            }
        });



//end DataSource



         
   function DrawKanBan(data) {

            renderKanban($("#kanban"), categories);
            function renderKanban($container, categories) {

                      //  debugger;
                    //loadPanel.show();
                  //  loadPanel.hide();
                    $container.empty();
                   
                    categories.forEach(function (category) {
                        renderList($container, category)
                    });

                    $container.addClass("scrollable-board").dxScrollView({
                        direction: "horizontal",
                        showScrollbar: "always"
                    });
                
                    //$container.addClass("sortable-lists").dxSortable({
                       // filter: ".list",
                       // itemOrientation: "horizontal",
                       // handle: ".list-title",
                       // moveItemOnDrop: true,
                        //onDragChange: function (e) { console.log("Uppr"); console.log(e); },
                        //onDragEnd: function (e) { console.log("starM"); console.log(e); },
                       // onDragMove: function (e) { console.log("starMk"); console.log(e); }
                   // }) 
                
            }

            function renderList($container, category) {

                    var TreeListDataSource = Tasks.filter(function(Task21) { return Task21.ParentTaskID == category.ID || Task21.Level >= 2   });
                    var $list = $("<div>").addClass("list").appendTo($container);
                   // var $scroll = $("<div>").appendTo($list);
                     //   $scroll.addClass("scrollable-list").dxScrollView({
                      //      direction: "vertical",
                      //      showScrollbar: "always"
                      //   });

                  // debugger;
                    renderListTitle($list, category);
                    var listInner = $("<div>").attr("id", category.ID).appendTo($list).dxTreeList({
                        dataSource: TreeListDataSource,
                        keyExpr: "ID",
                        parentIdExpr: "ParentTaskIDummy",
                        hasItemsExpr: "Has_Items",
                        expandedRowKeys: [1, 2],
                        scrolling: {
                                 mode: "virtual",
                                 rowRenderingMode: "virtual"
                        },
                        row:{rowType: "detailAdaptive"},
                        buttons: [{
                            name: "add",
                            cssClass: "my-class"
                         }],
                            onToolbarPreparing: function(e) { e.toolbarOptions.visible = false;},
                        rowDragging: 
                        {
                            showDragIcons: false,
                            allowDropInsideItem: true,
                            allowReordering: true,
                            group: "tasksGroup",
                            onDragChange: function (e) {
                                if (e.itemData.ID != 0) {
                                        
                                    //var treeL = $("#tree-list").dxTreeList("instance");
                                   // var visibleRows = treeL.getVisibleRows();
                                   // debugger;
                                    var visibleRows = e.component.getVisibleRows();
                                    if(visibleRows != null && visibleRows.length != 0){
                                        //sourceNode = e.component.getNodeByKey(e.itemData.ID);
                                        //targetNode = visibleRows[e.toIndex].node;

                                       // while (targetNode && targetNode.data) {
                                      //  if (targetNode.data.ID === sourceNode.data.ID) {
                                       //    e.cancel = true;
                                        //    break;
                                        //}
                                       // targetNode = targetNode.parent;
                                       // }
                                    }                                                                                                            
                                }
                            },
                            onAdd: function (e) 
                            {
                                     // loadPanel.show();                                        
                                      if (e.itemData.ID != 0) 
                                        {
                                            debugger;
                                            sourceData = e.itemData;
                                            var visibleRows =  e.toComponent.getVisibleRows();
                                            var targetDataID = 0;
                                            if(visibleRows != null && visibleRows.length != 0)
                                            {
                                                 var targetDataID = visibleRows[e.toIndex].data.ID;  
                                            }
                                                                                         
                                             $.ajax({
                                                       url: ugitConfig.apiBaseUrl + "/api/module/DragAndDrop?TicketPublicId=PMM-19-000012" + "&toKey=" + sourceData.ID + "&fromKey=" + targetDataID,
                                                       method: "POST",
                                                       success: function (data) { 
                                                                            RepaintTaskWorkflow();
                                                                    },
                                                                    error: function (error) { }
                                                    })
    
                                        }   
                                 },
                       
                            onReorder: function (e) {
                            if (e.itemData.ID != 0) {
                                var visibleRows = e.component.getVisibleRows();
                                sourceData = e.itemData;
                                targetData = visibleRows[e.toIndex].data;
                               // loadPanel.show();
                                $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DragAndDrop?TicketPublicId=PMM-19-000012" + "&toKey=" + sourceData.ID + "&fromKey=" + targetData.ID,
                                    method: "POST",
                                    success: function (data) {
                                        //var treeL = $("#tree-list").dxTreeList("instance");
                                                    //debugger;
                                        /*console.log("klklkl");*/
                                        var treeL = e.component;
                                        treeL.refresh();
                                         var tree =  $("#1").dxTreeList("instance");
                                                       
                                        //loadPanel.hide();

                                        //if (IsCompactView.toLowerCase() == "true")
                                            RepaintTaskWorkflow();
                                    },
                                    error: function (error) { }
                                })
                                }
                                }
                         },
                        columns: [{
                                    dataField: "Title",
                                    allowAdding: true,
                                    allowEditing: true,
                                    minWidth:100,
                                    maxWidth:50,
                                    validationRules: [{ type: "required" }],
                                    cellTemplate: function (container1, options) {   
                    
                                    if (typeof options.data.Title != "undefined" && options.data.Title != null) 
                                    {
                                              var $item = $("<div>")
                                                    .addClass("card")
                                                    .addClass("dx-card")
                                                    .addClass("dx-theme-text-color")
                                                    .addClass("dx-theme-background-color")
                                                    
                                                    .appendTo(container1);
                                             $("<div>").addClass("card-priority").addClass("priority-" +  options.data.Status.replace(' ','').toLowerCase()).appendTo($item);
                                            
                                             $("<div id='dataTitle'>")
                                            .append("<span style='float: left;overflow: auto;'>" +  options.data.ItemOrder +  ". " + options.data.Title + "</span>")
                                            .appendTo($item);
                                            if(typeof options.data.AssignedToName != "undefined" && options.data.AssignedToName != null && options.data.AssignedToName != ''){
                                                         $("</br>").appendTo($item);               
                                                        $("<div>").addClass("owner").text("Owner :" + options.data.AssignedToName).appendTo($item);
                                                   
                                                     }  
                                                $("</br>").appendTo($item);    
                                               // $("<div>").addClass("owner").text("Itemorder : " + options.data.ItemOrder).appendTo($item);     
                                           // var appendImg = $("<img>", { "src": "/content/images/moreoptions_blue.png"}).appendTo($item);
                                            
                                             $("<div>").addClass("predecesser").appendTo($item); 
                                              $("<div>").addClass("predecesser").text(options.data.StartDate.split('T')[0]   + " - "  + options.data.DueDate.split('T')[0] ).appendTo($item); 
                                            $("<img>", { "src": "/Assets/recource-management.png" }).addClass("resource").appendTo($item);

                                       
                                    }
                        
                                }
                               }, {
                                    dataField: "EstimatedRemainingHours",
                                    caption: "ERH",
                                    visible:false,
                                    allowEditing: EditingAllow,
                                    allowFiltering: false,
                                    dataType: "number",
                                    width: 50,
                                      },
                                     {
                                    dataField: "StartDate",
                                    caption: "Start Date",
                                    mode: "batch",
                                    visible:false,
                                    allowEditing: EditingAllow,
                                    dataType: "date",
                                    allowFiltering: false,
                                    width: 100,
                                    //validationRules: [{ type: "required" }],
                                    format: 'MMM d, yyyy',
                                    editorOptions: {
                                        showClearButton: true
                                    }
                                }, {
                                    allowAdding: true,
                                    dataField: "DueDate",
                                    caption: "Due Date",
                                    allowEditing: EditingAllow,
                                    allowFiltering: false,
                                    dataType: "date",
                                    width: 100,
                                    visible:false,
                                    //validationRules: [{ type: "required" }],
                                    format: 'MMM d, yyyy',
                                    editorOptions: {
                                        showClearButton: true
                                    }
                               },{
                dataField: "Status",
                caption: "Status",
                visible: false,
                allowEditing: EditingAllow,
                width: 90,
                lookup: {
                    dataSource: [
                        "Not Started",
                        "In Progress",
                        "Completed",
                        "Waiting",
                                ]
                        }
                    }],
                        showRowLines: true,
                        showBorders: true,
                        columnAutoWidth: true,
                        editing: {
                            mode: "form",

                            allowAdding:true,
                              
                            allowUpdating: true,
                            allowDeleting: bDeletingAllow,
                            useIcons: true,
                            refreshMode:"reshape",
                          },
                          onRowUpdating: function(e){
                                
                                var key = e.key;
                                var values = e.newData;
                                return $.ajax({
                                        url: ugitConfig.apiBaseUrl + "/api/module/UpdateTask",
                                        type: "PUT",
                                        data: { key: JSON.stringify(key), values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                                        async: false,
                                        success: function (data) {                                              
                                                RepaintTaskWorkflow();
                                                        }
                                                    })

                          },//end onRowUpdating
                        
                     onRowRemoving: function(e){
                                debugger;
                                var key = e.key; 
                                /*console.log("remove");*/
                                return $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DeleteTask",
                                    type: "DELETE",
                                    data: { key: key, TicketId: '<%= TicketID%>' },
                                    async: false,
                                    success: function (data) {                                        
                                           RepaintTaskWorkflow();
                                    }
                                })

                    },//end onRowRemoving
                    onRowInserting: function(e){
                                debugger;
                                    e.data.ParentTaskID = e.data.ParentTaskIDummy;
                                    var values = e.data;
                                    
                                    return $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/InsertTask",
                                    type: "POST",
                                    data: { values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                                    async: false,
                                    success: function (data) {                                      
                                            RepaintTaskWorkflow();
                                    }
                                })

                    }// end onRowInserting
            
          })

       }

            function renderListTitle($container, category)
            {
                 var bindCategory = [];
                 bindCategory[0] = category;
                
                 $("<div>")
                    .addClass("list-title")
                    .addClass("dx-theme-text-color")
                    .text(category.Title).attr("id", category.ID + "_main").appendTo($container).dxTreeList({
                        dataSource: bindCategory,
                         keyExpr: "ID",
                        parentIdExpr: "ParentTaskID",
                         onToolbarPreparing: function(e) { e.toolbarOptions.visible = false;},
                         rowDragging: 
                         {
                            showDragIcons: false,
                            allowDropInsideItem: true,
                            allowReordering: true,
                            group: "tasksGroup",
                            showColumnHeaders: false,
                            onDragChange: function (e) {
                               
                            },
                            onAdd: function (e) 
                            {
                                   var targetData = e.toComponent.getVisibleRows()[e.toIndex].data;
                                  sourceData = e.itemData;
                                  $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/DragAndDrop?TicketPublicId=PMM-19-000012" + "&toKey=" + sourceData.ID + "&fromKey=" + targetData.ID,
                                    method: "POST",
                                    success: function (data) {
                                        //var treeL = $("#tree-list").dxTreeList("instance");
                                                    //debugger;
                                        /*console.log("klklkl");*/
                                        var treeL = e.component;
                                        treeL.refresh();
                                         var tree =  $("#1").dxTreeList("instance");
                                                       
                                        //loadPanel.hide();

                                        //if (IsCompactView.toLowerCase() == "true")
                                            RepaintTaskWorkflow();
                                    },
                                    error: function (error) { }
                                })
                            },
                         },
                            columns: [{
                                  
                                    dataField: "Title",
                                    allowEditing: true,
                                    minWidth:120,
                                    validationRules: [{ type: "required" }],
                                    cellTemplate: function (container1, options) { 
                                     var $item = $("<div>")
                                                    .addClass("card")
                                                    .addClass("dx-card")
                                                    .addClass("dx-theme-text-color")
                                                    .addClass("dx-theme-background-color")
                                                    
                                                    .appendTo(container1);
                                          $("<div>").addClass("card-priority").addClass("priority-" +  options.data.Status).appendTo($item);

                                            $("<div id='dataTitle'>")
                                            .append("<span style='float: left;overflow: auto;'>" + options.data.Title + "</span>")
                                            .appendTo($item);
                                               if(typeof options.data.AssignedToName != "undefined" && options.data.AssignedToName != null){
                                                                      //  $("<div>").addClass("owner").text(+ options.data.AssignedToName).appendTo($item);
                                                   
                                               }}    
                                                

                                } ,{
                                    dataField: "EstimatedRemainingHours",
                                    caption: "ERH",
                                    visible:false,
                                    allowEditing: EditingAllow,
                                    allowFiltering: false,
                                    dataType: "number",
                                    width: 50,
                                      },
                                     {
                                    dataField: "StartDate",
                                    caption: "Start Date",
                                    mode: "batch",
                                    visible:false,
                                    allowEditing: EditingAllow,
                                    dataType: "date",
                                    allowFiltering: false,
                                    width: 100,
                                    //validationRules: [{ type: "required" }],
                                    format: 'MMM d, yyyy',
                                    editorOptions: {
                                        showClearButton: true
                                    }
                                }, {
                                    allowAdding: true,
                                    dataField: "DueDate",
                                    caption: "Due Date",
                                    allowEditing: EditingAllow,
                                    allowFiltering: false,
                                    dataType: "date",
                                    width: 100,
                                    visible:false,
                                    //validationRules: [{ type: "required" }],
                                    format: 'MMM d, yyyy',
                                    editorOptions: {
                                        showClearButton: true
                                    }
                               },{
                dataField: "Status",
                caption: "Status",
                visible: false,
                allowEditing: EditingAllow,
                width: 90,
                lookup: {
                    dataSource: [
                        "Not Started",
                        "In Progress",
                        "Completed",
                        "Waiting",
                                ]
                        }
                    }],
                        showRowLines: true,
                        showBorders: true,
                        columnAutoWidth: true,
                        editing: {
                            mode: "form",

                            allowAdding:true,
                              
                            //allowUpdating: true,
                            //allowDeleting: bDeletingAllow,
                            useIcons: true,
                            refreshMode:"reshape",
                          },
                            onRowInserting: function(e){
                                debugger;
                                   // e.data.ParentTaskID = e.data.ParentTaskIDummy;
                                    var values = e.data;
                                    
                                    return $.ajax({
                                    url: ugitConfig.apiBaseUrl + "/api/module/InsertTask",
                                    type: "POST",
                                    data: { values: JSON.stringify(values), TicketId: '<%= TicketID%>' },
                                    async: false,
                                    success: function (data) {                                      
                                            RepaintTaskWorkflow();
                                    }
                                })

                    }// end onRowInserting


                    }).dxTreeList("instance");

                   // .appendTo($container);
  
            }

      }

       
</script>


<div class="demo-container">
        <div id="kanban"></div>
    <div id="loadpanel"></div>
</div>



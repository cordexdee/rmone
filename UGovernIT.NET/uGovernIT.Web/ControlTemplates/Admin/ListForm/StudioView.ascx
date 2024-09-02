<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StudioView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.StudioView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
   .primary-blueBtn .dxb {
        height:33px;
        font-weight: 400;
    }
        .dx-button-has-text .dx-icon{
            filter: brightness(0) invert(1);
        }
        .imgEditIcon{width: 18px;}
   .dx-button-has-text .dx-button-content {
          /*font-weight: bold;*/
          color: #fff;
          padding: 10px;
          background: #4FA1D6;
    }
</style>


<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        var showArchived = false;
        
         var studiosource  = new DevExpress.data.DataSource({
            key: "ID",
            load: function (loadOptions) {
                
                var apiUrl = "/api/module/GetStudioData?showArchived=" + showArchived;
                //return $.getJSON(ugitConfig.apiBaseUrl + apiUrl);
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + apiUrl,
                    type: "GET",                   
                    async: false,
                    success: function (data) {                        
                        return(data); 
                    }
                });

            },

            insert: function (values) {
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "api/module/AddUpdateStudio",
                    type: "POST",
                    data: values,
                    async: false,
                    success: function (data) {
                        
                        if (data == "Duplicate")
                            DevExpress.ui.notify({ message: "Studio Already Exists.", width: 300, shading: true }, "error", 3000);
                        else if (data == "EmptyTitle")
                            DevExpress.ui.notify({ message: "Please Enter Title For Studio.", width: 300, shading: true }, "error", 3000);
                        else if (data == "DivisionRequired")
                            DevExpress.ui.notify({ message: "Please select a Division for Studio.", width: 300, shading: true }, "error", 3000);
                        else
                            studiosource.load();
                    }
                });
            },
            update: function (key, values) {
                values.ID = String(key);
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "api/module/AddUpdateStudio",
                    type: "POST",
                    data: values,
                    async: false,
                    success: function (data) {
                        if (data == "Duplicate")
                            DevExpress.ui.notify({ message: "Studio Already Exists.", width: 300, shading: true }, "error", 3000);
                        else if (data == "EmptyTitle")
                            DevExpress.ui.notify({ message: "Please Enter Title For Studio.", width: 300, shading: true }, "error", 3000);
                        else if (data == "DivisionRequired")
                            DevExpress.ui.notify({ message: "Please select a Division for Studio.", width: 300, shading: true }, "error", 3000);
                        else
                            studiosource.load();
                    }
                });
            },
            remove: function (key) {
                //alert(key);
                return $.ajax({
                    url: ugitConfig.apiBaseUrl + "api/module/DeleteStudio",
                    type: "DELETE",
                    data: { key: key },
                    async: false,
                    success: function (data) {
                            studiosource.load();
                    }
                });
            }
        });

        var divisionsource = new DevExpress.data.DataSource({
            key: "ID",
            load: function (loadOptions) {
                return $.getJSON(ugitConfig.apiBaseUrl + "/api/module/GetDivsionData");
            }
        });

        var lookupDataSource = {
            store: new DevExpress.data.CustomStore({
                key: "ID",
                loadMode: "raw",
                load: function () {
                    // Returns an array of objects that have the following structure:
                    return $.getJSON(ugitConfig.apiBaseUrl + "/api/module/GetDivsionData");
                }
                
            }),
            sort: "Title"
        }
        const actionAdd = $('#action-add').dxButton({
            text: 'Add Studio',
            icon: "/content/Images/plus-blue.png",
            index: 1,
            onClick: function (e) {
                grid.addRow();
            },
        }).dxButton('instance');


        $("#chkShowArchived").dxCheckBox({
            text: "Show Deleted",
            visible: true,
            onValueChanged: function (e) {
                showArchived = e.value;
                var studioGrid = $("#DivStudioGrid").dxDataGrid("instance");
                studioGrid.refresh();
            }
        });
        const grid =  $("#DivStudioGrid").dxDataGrid({
            dataSource: studiosource,
            keyExpr: "ID",
            showBorders: true,
            showRowLines: true,
            paging: {
                enabled: false
            },
            editing: {
                mode: "batch",
                allowUpdating: true,
                allowAdding: false,
                allowDeleting: true,
                useIcons: true,
                selectTextOnEditStart: true,
                startEditAction: 'click',
                texts: { confirmDeleteMessage: 'Are you sure you want to proceed?' }
            },
            onToolbarPreparing: function (e) {
                $.each(e.toolbarOptions.items,
                    function (i, v) {
                        if (v.name == "saveButton") {
                            //v.options.icon = 'runner';
                            v.options.icon = "";
                            v.options.text = "Save";
                            v.showText = "always";
                        }
                        else if (v.name == "revertButton") {
                            v.options.icon = "";
                            v.options.text = "Undo";
                            v.showText = "always";
                        }
                    });
            },
            columns: [
                {
                    dataField: "DivisionLookup",
                    caption: "Division",
                    width: 200,
                    visible: "<%=EnableStudioDivisionHierarchy%>".toLowerCase() === "true",
                    allowEditing: "<%=EnableStudioDivisionHierarchy%>".toLowerCase() === "true",
                    lookup: {
                        dataSource: lookupDataSource,
                        displayExpr: "Title",
                        valueExpr: "ID",
                        allowEditing: true
                    },
                    headerCellTemplate: function (header, info) {
                        $(`<div style="color: black;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                    }//,validationRules: [{ type: "<%=EnableStudioDivisionHierarchy%>".toLowerCase() === "true" ? 'required' : 'null'}]
                },                {
                    dataField: "Title",
                    caption: "Studio",
                    width: 200,
                    validationRules: [{ type: "required" }],
                    headerCellTemplate: function (header, info) {
                        $(`<div style="color: black;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    dataField: "Description",
                    caption: "Description",
                    headerCellTemplate: function (header, info) {
                        $(`<div style="color: black;font-weight:600;">${info.column.caption}</div>`).appendTo(header);
                    }
                },
                {
                    type: 'buttons',
                    width: 80,
                    buttons: [
                        {
                            name: "edit",
                            hint: "Edit",
                            icon: '/content/images/editNewIcon.png',
                            cssClass: 'imgEditIcon'
                        }
                        ,
                        {
                            name: "save",
                            hint: "Save",
                            icon: '/content/images/saveAsTemplate-blue.png',
                            cssClass: 'imgEditIcon'
                        }
                        ,
                        {
                            name: "undelete",
                            hint: "Revert",
                        }
                        ,
                        {
                            name: 'delete',
                            hint: 'Delete',
                            visible(e) {
                                return !showArchived;
                            },
                            icon: '/content/images/redNew_delete.png',                            
                        }
                        ,
                        {
                            name: 'delete',
                            hint: 'Un-Delete',
                            visible(e) {
                                return showArchived;
                            },
                            icon: '/content/images/approve.png',
                        }
                    ],
                },

            ]
        }).dxDataGrid('instance');
    });


    
</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
</dx:ASPxLoadingPanel>

<div>
 <div class="col-sm-6 col-md-6 pt-1">
        <div id="action-add" class="btnAddNew mr-1" ></div>
     <div id="chkShowArchived"></div>
 </div>
<div class="col-sm-6 col-md-6">
        <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn rightBtnSection pt-1" Text="Apply Changes" 
            ToolTip="Apply Changes" OnClick="btnApplyChanges_Click">
                   <ClientSideEvents Click="function(s, e){LoadingPanel.Show();}" />

        </dx:ASPxButton>
 </div>

    <div id="DivStudioGrid" class="col-sm-12 col-md-12 pt-1" >

    </div>
</div>
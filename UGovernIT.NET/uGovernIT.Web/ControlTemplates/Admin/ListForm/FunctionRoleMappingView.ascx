<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionRoleMappingView.ascx.cs" Inherits="uGovernIT.Web.FunctionRoleMappingView" %>

<script>
    var FunctionRoleData = [];
    var Functions = [];
    var GlobalRoles = [];
    $(()=>{

        $.get(ugitConfig.apiBaseUrl + "/api/bench/GetFunctonRoles", function(data){
            Functions = data;
            });

        $.get(ugitConfig.apiBaseUrl + "/api/NewWizard/GetRoles", function(data){
            GlobalRoles = data;
            });
        $(document).on("click", "img.imgDeleteNew", function (e) {
            var delId = $(this).attr("ID");
            var delItem = FunctionRoleData.filter(x => String(x.ID) == delId);
            $.get(ugitConfig.apiBaseUrl + "/api/Bench/DeleteFunctionRoleMapping?functionRoleId=" + delId + "&FunctionId="+ delItem[0].FunctionId + "&RoleId=" + delItem[0].RoleId, function (data) {
                DevExpress.ui.notify('Mapping deleted successfully.', "success");
                FunctionRoleData = FunctionRoleData.filter(x => String(x.ID) != delId);
                let dataGrid = $("#divFunctionRoleGrid").dxDataGrid("instance");
                dataGrid.option("dataSource", FunctionRoleData);
            });
        });

        $.get(ugitConfig.apiBaseUrl + "/api/Bench/GetFunctionRoleMapping", function (data) {
            if (data == "No Data Found")
                FunctionRoleData = [];
            else
                FunctionRoleData = data;

            var functionrolegrid = $("#divFunctionRoleGrid").dxDataGrid({
                        dataSource:FunctionRoleData,
                        keyExpr: 'ID',
                        showBorders: true,
                        repaintChangesOnly: true,
                        paging: {
                          enabled: false,
                        },
                        editing: {
                            mode: 'cell',
                            newRowPosition:'last',
                          allowEditing:true,
                          allowUpdating: true,
                          allowAdding: false,
                          allowDeleting: false,
                        },
                        columns:[
                            {
                                dataField:"FunctionName",
                                caption:"Function",
                                lookup: {
                                    dataSource: Functions,
                                    valueExpr: 'ID',
                                    displayExpr: 'Title',
                                  },
                                  validationRules: [{ type: 'required' }],
                                  cellTemplate: function (container, options){
                                      $(`<div><a onclick='return OpenFunctionEditPopup(${options.data.FunctionId})'>${options.data.FunctionName}</a></div>`).appendTo(container);
                                },
                                editCellTemplate: comboBoxFunctionEditorTemplate
                            },
                            {
                                dataField:"RoleName",
                                caption:"Role",
                                lookup: {
                                    dataSource: GlobalRoles,
                                    valueExpr: 'Id',
                                    displayExpr: 'Name',
                                  },
                                  validationRules: [{ type: 'required' }],
                                  cellTemplate: function (container, options){
                                      $(`<div>${options.data.RoleName}</div>`).appendTo(container);
                                },
                                editCellTemplate: comboBoxRoleEditorTemplate
                            },
                            {
                                width: "5%",
                                cellTemplate: function (container, options){
                                    $("<div id='rowDelete' style='text-align:center;'>")
                                            .append($("<img>", {
                                                "src": "/content/images/deleteIcon-new.png", "ID": options.data.ID, 
                                                "style": "overflow: auto;cursor: pointer;", "class": "imgDeleteNew", "title": "Delete", "width": "23px"
                                            }))
                                            .appendTo(container);
                                }
                            }
                ],
                onInitNewRow: function (e) {
                    e.data.ID = 0;
                    e.data.RoleId = null;
                    e.data.FunctionName = null;
                    e.data.RoleName = null;
                        }
                    });
        });

        

        var btnSave = $("#btnAddNew").dxButton({
            text:"Add New Mapping",
            onClick: function (e) {
                //var grid = $("#divFunctionRoleGrid").dxDataGrid("instance");
                //grid.addRow();
                UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=FunctionRoleMappingAddEdit','','Function Role','60','90', false, '', false);
            }
        });

        var btnAddFunction = $("#btnAddFunction").dxButton({
            text:"Add Function",
            onClick:function(e){
                     UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=functionroleaddedit&mode=Add','','Function Role','80','80', false, '', false);
            }
        });
    });

    function OpenFunctionEditPopup(functionid) {
        UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=functionroleaddedit&mode=Edit&Id=' + functionid, '', 'Function Role', '80', '80', false, '', false);
    }

    function comboBoxFunctionEditorTemplate(cellElement, cellInfo){
        return $(`<div>`).dxSelectBox({
            dataSource:ugitConfig.apiBaseUrl + "/api/bench/GetFunctonRoles",
            value: cellInfo.value,
            valueExpr: 'ID',
            displayExpr: 'Title',
            searchEnabled: true,
            onValueChanged(e) {
            cellInfo.setValue(e.value);
            },
            onSelectionChanged() {
            cellInfo.component.updateDimensions();
            },
        });
    }

    function comboBoxRoleEditorTemplate(cellElement, cellInfo){
        return $(`<div>`).dxSelectBox({
              dataSource: ugitConfig.apiBaseUrl + "/api/NewWizard/GetRoles",
              value: cellInfo.value,
              valueExpr: 'Id',
              displayExpr: 'Name',
              searchEnabled: true,
              onValueChanged(e) {
                cellInfo.setValue(e.value);
              },
              onSelectionChanged() {
                cellInfo.component.updateDimensions();
              },
        });
    }
</script>

<div class="container">
    <div class="row pt-2">
        <div id="divFunctionRoleGrid"></div>
    </div>
    <div class="row d-flex flex-row-reverse pt-2">
        <div id="btnAddNew" class="btnAddNew"></div>
        <div id="btnAddFunction" class="btnAddNew"></div>
    </div>
</div>
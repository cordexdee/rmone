<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalRolesView.ascx.cs" Inherits="uGovernIT.Web.GlobalRolesView" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpreadsheet.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpreadsheet" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-datagrid-headers .dx-datagrid-table .dx-row {
        border-bottom: 1px solid #ddd;
        max-width: 100%;
        font-weight: bold;
        color: #4b4b4b;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    <%--function openGlobalRoleDialog(path, params, titleVal, width, height, stopRefresh) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }

    function NewAddRoleDialog() {
        window.parent.UgitOpenPopupDialog('<%= EditUrl%>&JobTitleID=0', "", "Add Roles", '600px', '400px', 0);
    }--%>
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var baseUrl = ugitConfig.apiBaseUrl;
    var DS = [];

    var RoleId = "";
    $(() => {
        $('#checked').dxCheckBox({
            value: false,
            text: 'Show Deleted',
            onValueChanged(e) {
                var grd = $('#gridContainer').dxDataGrid("instance");
                if (e.value) {
                    grd.option("dataSource", DS.Table);
                }
                else {
                    grd.option("dataSource", DS.Table.filter(item => item.Deleted == false));
                }
            },
        });
        $('#btnImport').dxButton({
            elementAttr: {
                id: "btnImport",
                class: "primary-btn-link phrasesAdd-label"
            },
            stylingMode: 'contained',
            text: 'Import',
            type: 'default',
            width: 110,
            onClick() {
                Import();
                //DevExpress.ui.notify('The Contained button was clicked');
            },
        });

        $('#btnAdd').dxButton({
            elementAttr: {
                id: "btnAdd",
                class: "primary-btn-link phrasesAdd-label"
            },
            stylingMode: 'contained',
            text: 'Add New Item',
            type: 'default',
            width: 130,
            onClick() {
                AddRole();
                //DevExpress.ui.notify('The Contained button was clicked');
            },
        });

        $.get(baseUrl + "api/common/GetRoleDetails", function (data) {
            DS = data;
            var dt = data.Table.filter(item => item.Deleted == false);
            $('#gridContainer').dxDataGrid({
                dataSource: dt,
                keyExpr: 'Id',
                showBorders: true,
                paging: false,
                remoteOperations: false,
                cellHintEnabled: true,
                rowAlternationEnabled: true,
                columns: [
                    {
                        caption: 'Role Name',
                        dataField: 'Name',
                        width: 300,
                        cellTemplate: function (container, options) {
                            //debugger;
                            $(`<span><a style ='cursor:pointer'onclick="openJobTitleDialog('<%= editUrl%>','RoleID=${options.data.Id}','Add Roles','680px','380px',false)">${options.data.Name}</a></span >`)
                                .appendTo(container);
                        },
                    },
                    {
                        caption: 'Field Name',
                        dataField: 'FieldName',
                        minWidth: 50,
                        alignment: 'center'
                    },
                    {
                        caption: 'Description',
                        dataField: 'Description',
                        minWidth: 5,
                        alignment: 'center',
                    },
                ],

                onRowPrepared: function (e) {
                    if (e.rowType == "data") {
                        if (e.data.Deleted != false) {
                            e.rowElement.addClass('clssdeletedrow');
                        }
                    }
                },

                masterDetail: {
                    enabled: true,
                    template(container, options) {
                       
                        data = DS.Table1.filter(item => item.RoleLookup == options.data.Id && item.Deleted == false);
                        var title = 'Roles Using Title: ' + options.data.Name;
                        RoleId = options.data.Id;
                        var url = '<%= departmentJobtitleMappingUrl%>';
                        $('<div>')
                            .addClass('master-detail-caption')
                            .text(`Departments Using Role: ` + options.data.Name)
                            .append($(`<img type='button' style='cursor:pointer;margin-left:5px' class='orgGridBtn' height='18' width='18' src='/content/images/plus-blue.png' onclick="openJobTitleDialog('${url}','RoleId=${options.data.Id}','${title}','600px','400px',false)" />`))
                            .append($(`<input type="checkbox" id='checkedchild_${options.data.Id}' onclick="showdeletedchilddata(this,'${options.data.Id}')" ><label for='checkedchild_${options.data.Id}' > Show Deleted</label>`))
                            .appendTo(container);
                        $(`<div class="child1" id='div_${options.data.Id}'>`)
                            .dxDataGrid({
                                columnAutoWidth: true,
                                showBorders: true,
                                paging: false,
                                rowAlternationEnabled: true,
                                remoteOperations: false,
                                keyExpr: 'ID',
                                columns: [{
                                    caption: 'Department',
                                    dataField: 'DepartmentName',
                                    width: 300,
                                    cellTemplate: function (container, options) {
                                       // debugger;
                                        $(`<span><a style ='cursor:pointer'onclick="openJobTitleDialog('<%= departmentJobtitleMappingUrl%>','deptid=${options.data.DepartmentLookup}&roleid=${options.data.RoleLookup}&blr=${options.data.BillingRate}&deleted=${options.data.Deleted}&mode=${'E'}&Id=${options.data.ID}','Edit ${options.data.DepartmentName}','680px','380px',false)">${options.data.DepartmentName}</a></span >`)
                                            .appendTo(container);
                                    },
                                },

                                    {
                                        caption: 'Billing Rate ($)',
                                        dataField: 'BillingRate',
                                        minWidth: 50,
                                        alignment: 'center'
                                    },
                                    
                                ],
                                dataSource: new DevExpress.data.DataSource({
                                    store: new DevExpress.data.ArrayStore({
                                        key: 'ID',
                                        data: data,
                                    }),

                                    filter: ['RoleLookup', '=', options.key],
                                }),
                                onRowPrepared: function (e) {
                                    if (e.rowType == "data") {
                                        if (e.data.Deleted != false) {
                                            e.rowElement.addClass('clssdeletedrow');
                                        }
                                    }
                                },
                                 
                            }).appendTo(container);
                    },

                },
            });
        });

    });
    function Import() {
        var param = '&listName=Roles';
        window.parent.UgitOpenPopupDialog('<%= ImportUserRolesUrl%>', param, 'Import User Roles', '550px', '260px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
    function AddRole() {
        window.parent.UgitOpenPopupDialog('<%= EditUrl%>&JobTitleID=0', "", "Add Roles", '600px', '400px', 0);
    }
    function openJobTitleDialog(path, params, titleVal, width, height, stopRefresh) {

        window.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh);
    }
    function showdeletedchilddata(e,Id) {
      
        var grd = $('#div_' + Id).dxDataGrid("instance");
        if (e.checked) {
            grd.option("dataSource", DS.Table1.filter(item => item.RoleLookup == Id));
        }
        else {
            grd.option("dataSource", DS.Table1.filter(item => item.RoleLookup == Id && item.Deleted == false));

        }
    }
</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .clssdeletedrow {
        color: red;
    }

    .dx-button .dx-button-content {
        padding: unset !important;
    }

    .child1 {
        border: 1px solid #000000;
    }

    .child2 {
        border: 1px solid #1b13dc;
    }

    .contentPane {
        float: left;
        width: 100%;
        height: auto !important;
    }

    .dx-datagrid-headers {
        background-color: #f7f7f7 !important;
        color: black;
    }

    #gridContainer {
        max-height: 500px;
    }

    .master-detail-caption {
        padding: 0 0 5px 10px;
        font-size: 14px;
        font-weight: bold;
    }
    .div_actions {
        margin: 7px 15px 7px 15px;
    }
    .primary-blueBtn_Export {
        background: none;
        border: none;
        padding:0px !important;
        width:110px !important; 
        background-color:#337AB7 !important;
    }
    .primary-blueBtn_Export .dxb {
        background: #337AB7;
        color: #FFF;
        border: 1px solid #4FA1D6 !important;
        border-radius: 4px;
        padding: 5px 13px 5px 13px !important;
        font-size: 12px;
        font-family: 'Roboto', sans-serif;
        font-weight: 500;
    }
</style>
<div class="row div_actions">
        <div id="checked"></div>
        <div id="btnAdd"></div>
    <div id="btnImport"></div>
    <dx:ASPxButton runat="server" Text="Export" OnClick="btnExport_Click" CssClass="primary-blueBtn_Export primary-btn-link" ID="btnExports"></dx:ASPxButton>
</div>
<div class="row">
    <div class="demo-container col-md-12 col-sm-12 col-xs-12">
        <div id="gridContainer"></div>
    </div>
</div>
<%--<div class="col-md-12 col-sm-12 col-xs-12" style="padding-bottom:25px;">
    <div class="row">
        <div style="float: right; padding-top:5px">
            <a id="btnAddNewRole1" runat="server" href="" style="padding-left:15px" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
    </div>
    <div class="row" style="padding-top:15px;">
        <dx:ASPxGridView ID="grdRoles" AutoGenerateColumns="False" runat="server" CssClass="customgridview homeGrid"  SettingsText-EmptyDataRow="No record found."
            KeyFieldName="ID"  ClientInstanceName="grdRoles" OnHtmlRowPrepared="aspxGridJobTitles_HtmlRowPrepared" Width="100%" >
            <Columns>
                <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0" Width="10px" Settings-ShowFilterRowMenu="False">
                    <DataItemTemplate>
                        <img style="width:16px" src="/Content/Images/editNewIcon.png" alt="Edit" />
                    </DataItemTemplate>
                    <Settings AllowAutoFilter="False" ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataColumn FieldName="Name" Caption="Role Name" SortIndex="0" SortOrder="Ascending" Settings-SortMode="DisplayText" Settings-ShowInFilterControl="True" Settings-AllowFilterBySearchPanel="True"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="FieldName" Caption="Field Name"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Description" Caption="Description"></dx:GridViewDataColumn>
            </Columns>
            <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowFilterRow="true" ShowFilterBar="Auto" ShowFooter="true" ShowGroupPanel="false" 
                ShowHeaderFilterBlankItems="false" />
            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
        </dx:ASPxGridView>
    </div>
    <div class="row">
        <div style="float: right; padding-top:5px">
            <a id="btnAddNewRole" runat="server" href="" style="padding-left:15px" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
    </div>
</div>--%>


 <dx:ASPxSpreadsheet ID="ASPxSpreadsheet1" runat="server" Visible="false"></dx:ASPxSpreadsheet>

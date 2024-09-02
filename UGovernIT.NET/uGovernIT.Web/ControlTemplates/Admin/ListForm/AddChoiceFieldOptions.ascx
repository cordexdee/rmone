<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddChoiceFieldOptions.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.AddChoiceFieldOptions" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    var selectedTable;
    var selectedField;
    var allOptions;
    var choiceFields = [];
    var txtOptions;
    var Tablename = [];

    $(function () {
        choiceFields = new DevExpress.data.DataSource({
            key: "FieldName",
            load: function (e) {
                
                return $.getJSON("/api/module/GetChoiceTypeFields?SearchValue=" + e.searchValue, function (data) {
                    return data;
                });
            },
            searchExpr: "FieldName"
        });
        Tablename = new DevExpress.data.DataSource({
            key: "Tablename",
            load: function (e) {
                return $.getJSON("/api/module/GetChoiceTypeTable?SearchValue=" + e.searchValue, function (data) {
                    return data;
                });
            },
            searchExpr: "Tablename"
        });

        $("#divTable").dxSelectBox({
            dataSource: Tablename,
            displayExpr: "TableName",
            valueExpr: "TableName",
            placeholder: "Select a Table Name...",
            searchEnabled: true,
            searchExpr: "TableName",
            searchMode: 'Contains',
            searchTimeout: 500,
            showClearButton: true,
            activeStateEnabled: true,
            onValueChanged: function (e) {
                selectedTable = e.value;
                selectedField = e.value;
                //debugger;
                $.get("/api/module/GetChoiceTypeFields?SearchValue=" + selectedField, function (data) {
                    if (typeof data != "undefined") {
                        var ChoiceFieldDS = $('#divChoice').dxSelectBox('instance');
                        ChoiceFieldDS.option("dataSource", data);
                    }
                });

            }
        }).dxSelectBox("instance");
        $("#divChoice").dxSelectBox({
            dataSource: choiceFields,
            displayExpr: "FieldName",
            valueExpr: "FieldName",
            placeholder: "Select a Choice Field...",
            searchEnabled: true,
            searchExpr: "FieldName",
            searchMode: 'Contains',
            searchTimeout: 500,
            showClearButton: true,
            activeStateEnabled: true,
            onValueChanged: function (e) {
               // debugger;
                selectedField = e.value;
                $.get("/api/module/GetChoiceFieldOptions?FieldName=" + selectedField + "&TableName=" + selectedTable, function (data) {

                    if (typeof data != "undefined") {
                        allOptions = data.split(';#').join('\n');
                        txtOptions.option('value', allOptions);
                    }
                });
            }
        }).dxSelectBox("instance");


        txtOptions = $("#divMemo").dxTextArea({
            placeholder: "Enter each choice in seperate line...",
            height: "150px",
            onValueChanged: function (e) {
                allOptions = e.value;
            }
        }).dxTextArea("instance");

    });

    function AddEditChoices(s, e) {
        var choicevalues = allOptions.split("\n").join("~");
        $.get("/api/module/UpdateChoiceFieldOptions?TableName=" + selectedTable + "&FieldName=" + selectedField + "&values=" + choicevalues, function (data) {
            if (typeof data != "undefined") {
                allOptions = data.split(';#').join('\n');
                DevExpress.ui.notify({ message: "Successfully saved changes", width: 300, shading: false }, "success", 700);
                txtOptions.option('value', allOptions);
            }
        });
    }

    function closePopup(s, e) {
        window.parent.CloseWindowCallback(0, document.location.href);
    }
</script>



<div class="col-md-12 col-sm-12 col-xs-12 popupUp-mainContainer" style="margin-top:10px;">
    <div class="ms-formtable">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Select Table Name</h3>
                    </div>
                <div class="ms-formbody accomp_inputField">
                    <div id="divTable" class="dxselect-div"></div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Select Choice Field</h3>
                    </div>
                <div class="ms-formbody accomp_inputField">
                    <div id="divChoice" class="dxselect-div"></div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="ms-formbody accomp_inputField">
                <div id="divMemo"></div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap" style="padding-right: 6px !important;">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" AutoPostBack="false">
                <ClientSideEvents Click="function(s,e){ closePopup(s,e); }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" AutoPostBack="false">
                <ClientSideEvents Click="function(s,e){ AddEditChoices(s,e); }" />
            </dx:ASPxButton>
        </div>
    </div>
</div>

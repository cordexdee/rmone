
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITGBudgetEditor.ascx.cs" Inherits="uGovernIT.Web.ITGBudgetEditor" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Panel ID="projectStatusEditMode" runat="server">
    <div id="cssDiv">
        <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
            .fleft {
                float: left;
            }

            .mainblock {
            }

            .fullwidth {
                width: 99%;
            }

            .paddingfirstcell {
                padding-left: 6px !important;
            }

            .ms-listviewtable {
                background: #F8F8F8 !important;
            }

            .ms-viewheadertr .ms-vh2-gridview {
                height: 25px;
            }

            .detailviewitem td {
                text-align: left;
            }

            .widhtfirstcell {
                width: 99px;
            }

            .editviewtable td, .editviewtable th {
                border: 1px solid #DCDCDC;
                text-align: center;
                padding: 2px;
            }

                .editviewtable td td {
                    border: none;
                }

            .datetimectr {
                height: 20px;
                width: 70px;
            }

            .fleft {
                float: left;
            }

            .padding-button {
                padding-left: 2px;
            }

            .calenderyearnum {
                font-weight: bold;
                padding-top: 1px;
                padding-left: 3px;
                padding-right: 3px;
            }

            .alncenter {
                text-align: center;
            }

            .worksheetpanel {
                position: relative;
            }

            .errormessage-block {
                text-align: center;
                display: block;
            }

                .errormessage-block ol, .errormessage-block ol {
                    list-style-type: none;
                    color: Red;
                    margin: 0px;
                }

            .totalbudget-container td {
                border-top: 1px solid gray !important;
                border-bottom: 1px solid gray !important;
            }

            .fullwidth96 {
                width: 96%;
            }

            .fullwidth94 {
                width: 94%;
            }

            .hide {
                display: none;
            }

            .itgbudgetlist-container {
            }

            .itgbudgetActuallist-container {
            }

            .itgbudgetlist-container .ms-viewheadertr {
                width: 98%;
            }

            .itgbudgetActuallist-container .ms-viewheadertr {
                width: 98%;
            }

            .ui-multiselect {
                padding: 2px 0 2px 4px;
                text-align: left;
            }

            .ui-icon {
                background: url("/Content/images/drop-down-arrow.png") no-repeat scroll 0 0 transparent;
                float: right;
                height: 10px;
                position: relative;
                top: 5px;
                width: 13px;
            }

            .amount {
                text-align: right;
                border: 1px solid lightgray;
                border-bottom: none;
                border-top: none;
            }

            .categorymenu-container {
                z-index: 10000;
                float: left;
                width: 300px;
                border: 1px solid;
                display: none;
                position: absolute;
                background: #ffffff;
            }

            .category-itemspan {
                float: left;
                width: 195px;
                color: Red;
                font-weight: bold;
            }

            .subcategory-itemspan {
                float: left;
                width: 275px;
            }

            .subcategorylist-panel {
                float: left;
                width: 298px;
                height: 150px;
                overflow-x: hidden;
                overflow-y: scroll;
            }

            .categorylist-panel {
                float: left;
                width: 298px;
                height: 150px;
                overflow-x: hidden;
                overflow-y: scroll;
            }

            .categorytypelist-panel {
                float: left;
                width: 298px;
                height: 150px;
                overflow-x: hidden;
                overflow-y: scroll;
            }

            .company-itemspan {
                float: left;
                width: 195px;
                color: black;
                font-weight: bold;
            }

            .department-itemspan {
                float: left;
                width: 195px;
            }

            .departmentlist-panel {
                float: left;
                width: 248px;
                height: 150px;
                overflow-x: hidden;
                overflow-y: scroll;
            }

            a.clear-link {
                text-decoration: underline;
            }

                a.clear-link:hover {
                    text-decoration: none;
                }

            .first_tier_nav {
                padding-top: 0px;
            }

                .first_tier_nav ul {
                    margin: 0px;
                    float: right;
                }

            .amountcontainer {
                float: right;
                padding-right: 15px;
            }

            .ms-viewheadertr th[align="right"] td.ms-vb {
                text-align: right;
            }

            .hide {
                display: none;
            }

            .ms-formbody {
                background: none repeat scroll 0 0 #E8EDED;
                padding: 3px 6px 4px;
                vertical-align: top;
                border: 1px solid #A5A5A5;
                border-left: 0px;
                border-right: 0px;
            }

            .ms-formlabel {
                padding-left: 6px;
            }

            .lbbudgetglcode {
                font-weight: bold;
            }

            .selectedbudgetitems {
                float: left;
                font-weight: bold;
                padding: 2px 2px 3px 8px;
            }


            .bitemtable {
                border-collapse: collapse;
                width: 100%;
            }


                .bitemtable tr > th {
                    text-align: left;
                }

                .bitemtable > tbody > tr > th:nth-child(2) {
                    width: 140px;
                }

                .bitemtable > tbody > tr > th:nth-child(3) {
                    width: 150px;
                }

                .bitemtable > tbody > tr > th:nth-child(4) {
                    width: 80px;
                }

                .bitemtable > tbody > tr > th:nth-child(5) {
                    width: 70px;
                }

                .bitemtable > tbody > tr > th:nth-child(6) {
                    width: 75px;
                }

                .bitemtable > tbody > tr > th:nth-child(7) {
                    width: 75px;
                }

                .bitemtable .datetime {
                    width: 75px;
                    text-align: center;
                }

                .bitemtable td, .bitemtable th {
                    height: 20px;
                }

            .actual-head {
                background: #E1E1E1;
            }

                .actual-head td {
                    border-top: 1px solid black;
                    font-weight: bold;
                }

                    .actual-head td:first-child {
                        border-right: 1px solid black;
                        border-top: none;
                        width: 20px;
                        background: #fff;
                    }


            .actual-item {
            }

                .actual-item td:first-child {
                    border-right: 1px solid black;
                }

            .actual-item-last td {
                border-bottom: 1px solid black;
                height: 0px;
            }

                .actual-item-last td:first-child {
                    border-bottom: none;
                    border-right: 1px solid black;
                    height: 0px;
                    background: #fff;
                }

            .acutal-last-column {
                border-right: 1px solid black;
                height: 0px;
            }

            .actual-head .acutal-last-column {
                border-right: 1px solid black;
                width: 20px;
            }

            .actual-item-last .acutal-last-column {
                border-right: 1px solid black;
                height: 0px;
            }

            .subcategory-item td:first-child {
                padding-left: 10px;
            }

            .budget-item td:first-child {
                padding-left: 20px;
            }

            .actual-head td:first-child {
                width: 30px;
                background: #fff;
            }

            .actual-item td:first-child {
                width: 30px;
                background: #fff;
            }

            .bitemtable td, bitemtable th {
                padding-right: 5px;
            }

            .bitemtable .amount {
                text-align: right;
                padding-right: 10px;
            }

            .category-item td {
                font-weight: bold;
            }

            .subcategory-item td {
                font-weight: bold;
            }

            .categorytype-item td {
                font-weight: bold;
            }

            .bitemtable .grandtotal td {
                font-weight: bold;
            }

            .bitemtable .action-container {
                background: none repeat scroll 0 0 #FFFFAA;
                border: 1px solid #FFD47F;
                float: left;
                padding: 1px 5px 0;
                position: absolute;
                z-index: 1000;
                margin-top: -4px;
                margin-left: 3px;
                right: 0px;
            }

            .bitemtable-header {
                background: #E1E1E1; /*#687398;
            color:#fff;*/
            }

            .bitemtable .grandtotal {
                background: #E1E1E1;
            }

            .bitemtable .categorytype-item {
                background: #F0F0F0; /*#A4AAC1;*/
            }

            .bitemtable .category-item {
                background: #F0F0F0; /*#BED0E5;*/
            }

            .bitemtable .subcategory-item {
                background: #FAFAFA;
            }

            .bitemtable .bugdet-itemalt {
                background: #F7F7F7; /*#E9ECF3;*/
            }

            .itemtext-div {
                position: relative;
            }
        </style>
        <asp:Panel ID="pEnableCategoryType" runat="server" CssClass="penablecategorytype" Visible="false">
            <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
                .category-item td:first-child {
                    padding-left: 10px;
                }

                .subcategory-item td:first-child {
                    padding-left: 20px;
                }

                .budget-item td:first-child {
                    padding-left: 30px;
                }

                .actual-head td:first-child {
                    width: 40px;
                    background: #fff;
                }

                .actual-item td:first-child {
                    width: 40px;
                    background: #fff;
                }
            </style>


        </asp:Panel>
    </div>

    <style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
        .ugit-dropdown {
            background: none repeat scroll 0 0 #FFFFFF;
            border: 1px solid silver;
            cursor: pointer;
            margin: 0 auto;
            outline: medium none;
            padding: 3px;
            position: relative;
            width: 230px;
            color: black;
        }

            .ugit-dropdown:after {
                border-color: #000 transparent;
                border-style: solid;
                border-width: 4px 3px 0;
                content: "";
                height: 0;
                margin-top: -3px;
                position: absolute;
                right: 10px;
                top: 50%;
                width: 0;
            }

            .ugit-dropdown .dropdown {
                -moz-border-bottom-colors: inherit;
                -moz-border-left-colors: inherit;
                -moz-border-right-colors: inherit;
                -moz-border-top-colors: inherit;
                background: none repeat scroll 0 0 white;
                border-bottom: inherit;
                border-image: inherit;
                border-left: inherit;
                border-right: inherit;
                border-top: medium none;
                left: -1px;
                list-style: none outside none;
                margin-top: 1px;
                position: absolute;
                right: -1px;
                top: 100%;
                display: none;
                float: left;
                height: 100px;
                overflow: auto;
                color: black;
                margin: 0;
                padding: 0;
            }

                .ugit-dropdown .dropdown:before, .ugit-dropdown:before {
                }

                .ugit-dropdown .dropdown li {
                    position: relative;
                    color: black;
                    display: block;
                    padding: 3px;
                    list-type: none;
                    margin: 0;
                }

                    .ugit-dropdown .dropdown li:last-of-type label {
                        border: medium none;
                    }

                    .ugit-dropdown .dropdown li:hover {
                        background: none repeat scroll 0 0 #3399ff;
                    }

            .ugit-dropdown.active:after {
            }

            .ugit-dropdown.active .dropdown {
                display: block;
                color: white;
            }

        .no-opacity ugit-dropdown .dropdown, .no-pointerevents .ugit-dropdown .dropdown {
            display: block;
        }

        .no-opacity .ugit-dropdown.active .dropdown, .no-pointerevents .ugit-dropdown.active .dropdown {
            display: block;
        }

        .reportitem {
            border-bottom: 1px solid black;
            float: left;
            padding: 5px;
            padding-top: 5px;
            width: 91%;
            cursor: pointer;
            color: black;
        }
    </style>
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function DropDown(el) {
            this.dd = el;
            this.opts = this.dd.find('ul.dropdown > li');
            this.val = [];
            this.index = [];
            this.initEvents();
        }
        DropDown.prototype = {
            initEvents: function () {
                var obj = this;
                obj.dd.bind('click', function (event) {
                    $(this).toggleClass('active');
                    event.stopPropagation();
                });
                obj.opts.children('label').bind('click', function (event) {
                    var opt = $(this).parent();
                    ($.inArray(val, obj.val) !== -1) ? obj.val.splice($.inArray(val, obj.val), 1) : obj.val.push(val);
                    ($.inArray(idx, obj.index) !== -1) ? obj.index.splice($.inArray(idx, obj.index), 1) : obj.index.push(idx);
                });
            },
            getValue: function () {
                return this.val;
            },
            getIndex: function () {
                return this.index;
            }
        }

    </script>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        window.itgBudgetFilter = window.itgBudgetFilter || {};
        window.itgBudget = window.itgBudget || {};
        window.itgBudgetActual = window.itgBudgetActual || {};
        window.itgGrid = window.itgGrid || {};


        var budgetScrollPositionTop = 0;
        var budgetActualPositionTop = 0;

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequest);
        var btnId;

        function InitializeRequest(sender, args) {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
        }

        var notifyId = "";

        function BeginRequestHandler(sender, args) {
            budgetScrollPositionTop = $(".itgbudgetlist-container").scrollTop();
            budgetActualPositionTop = $(".itgbudgetActuallist-container").scrollTop();

            notifyId = AddNotification("Processing ..");
        }

        function EndRequest(sender, args) {

            var s = sender;
            var a = args;
            var msg = null;

            if (a._error != null) {

                switch (args._error.name) {
                    case "Sys.WebForms.PageRequestManagerServerErrorException":
                        msg = "PageRequestManagerServerErrorException";
                        break;
                    case "Sys.WebForms.PageRequestManagerParserErrorException":
                        msg = "PageRequestManagerParserErrorException";
                        break;
                    case "Sys.WebForms.PageRequestManagerTimeoutException":
                        msg = "PageRequestManagerTimeoutException";
                        break;
                }
                args._error.message = "My Custom Error Message " + msg;
                args.set_errorHandled(true);

            }
            else {


                $(".datetimectr111").parents("table").find("img").bind("click", function (e) {
                    try {
                        addHeightToCalculateFrameHeight(this, 170);
                    }
                    catch (ex) {
                    }
                });


            }
            
            initialize();

            $(".itgbudgetlist-container").scrollTop(budgetScrollPositionTop);
            $(".itgbudgetActuallist-container").scrollTop(budgetActualPositionTop);

            try {
                clickUpdateSize()
            }
            catch (ex) {
            }
        }


        window.itgBudgetFilter = {
            sCategoryType: "",
            sCategories: new Array(),
            sSubCategories: new Array(),
            sDepartments: new Array(),

            showHideCategoryTypeMenu: function () {
                if ($("#categoryTypeItems").is(":hidden")) {

                    $("#categoryTypeItems").show();
                    $("#categoryItems").hide();
                    $("#subCategoryItems").hide();
                    $("#departmentItems").hide();

                }
                else {
                    $("#categoryTypeItems").hide();
                    this.executeFilter();
                }

                return false;
            },


            showHideCategoriesMenu: function () {
                if ($("#categoryItems").is(":hidden")) {

                    $("#categoryItems").show();

                    $("#categoryTypeItems").hide();
                    $("#subCategoryItems").hide();
                    $("#departmentItems").hide();

                }
                else {
                    $("#categoryItems").hide();
                    this.executeFilter();
                }

                return false;
            },

            showHideSubCategoriesMenu: function () {
                if ($("#subCategoryItems").is(":hidden")) {

                    $("#subCategoryItems").show();
                    $("#categoryTypeItems").hide();
                    $("#categoryItems").hide();
                    $("#departmentItems").hide();

                }
                else {
                    $("#subCategoryItems").hide();
                    this.executeFilter();
                }

                return false;
            },

            showHideDepartmentMenu: function () {
                if ($("#departmentItems").is(":hidden")) {
                    $("#departmentItems").show();
                    $("#subCategoryItems").hide();
                    $("#categoryItems").hide();
                    $("#categoryTypeItems").hide();

                }
                else {
                    $("#departmentItems").hide();
                    this.executeFilter();
                }

                return false;
            },

            selectAllSubCategoryCheck: function (obj) {
                //<%-- Create attr clause using selected categories--%>
                var selectedCategories = $(".subcategorylist-panel :checkbox");
                if ($(obj).is(":checked")) {
                    for (var i = 0; i < selectedCategories.length; i++) {
                        if ($(selectedCategories[i]).parent("span").css('display') == 'block') {
                            $(selectedCategories[i]).attr("checked", "checked");
                        }
                        else
                            $(selectedCategories[i]).removeAttr("checked");
                    }
                }
                else
                    selectedCategories.removeAttr("checked");

                var attrClause = "";
                for (var i = 0; i < this.sCategories.length; i++) {
                    if (i != 0) {
                        attrClause = attrClause + ",";
                    }
                    attrClause = attrClause + " :checkbox[pid='" + selectedCategories[i] + "'] ";
                }


                this.sSubCategories = new Array();
                var allSubCChildren = $(".subcategorylist-panel " + attrClause);
                if ($(obj).is(":checked")) {
                    allSubCChildren.attr("checked", "checked");
                    for (var i = 0; i < allSubCChildren.length; i++) {

                        this.sSubCategories.push($(allSubCChildren[i]).attr("sid"));
                    }
                }
                else {
                    allSubCChildren.removeAttr("checked");
                }

                this.showSelectedSubCategoriesInMenu();
            },

            selectAllDepartmentCheck: function (obj) {
                this.sDepartments = new Array();
                var allDepartments = $(".departmentlist-panel :checkbox");
                if ($(obj).is(":checked")) {
                    allDepartments.attr("checked", "checked");

                    for (var i = 0; i < allDepartments.length; i++) {
                        this.sDepartments.push($(allDepartments[i]).val());
                    }

                }
                else {
                    allDepartments.removeAttr("checked");
                }

                this.showSelectedDepartmentsInMenu()
            },

            showSelectedBudgetTypesInMenu: function () {
                var selectedBudgetTypes = $(".categorytypelist-panel :checked");
                if (selectedBudgetTypes.length == 1) {
                    $("#categorytypeselectorTxt").html($(selectedBudgetTypes[0]).val());
                    $("#categorytypeselectorTxt").attr("selectedItem", selectedBudgetTypes.length);
                    $("#categorytypeselectorTxt").css("font-weight", "bold");
                }
                else {
                    $("#categorytypeselectorTxt").html("Budget Type");
                    $("#categorytypeselectorTxt").attr("selectedItem", selectedBudgetTypes.length);
                    $("#categorytypeselectorTxt").css("font-weight", "normal");
                }
            },

            showSelectedCategoriesInMenu: function () {
               
                var sselectedCategories = $(".categorylist-panel :checked");
                if (sselectedCategories.length == 1) {
                    $("#categoryselectorTxt").html(sselectedCategories.length + " Category Selected");
                    $("#categoryselectorTxt").attr("selectedItem", sselectedCategories.length);
                    $("#categoryselectorTxt").css("font-weight", "bold");
                }
                else if (sselectedCategories.length > 1) {
                    $("#categoryselectorTxt").html(sselectedCategories.length + " Categories Selected");
                    $("#categoryselectorTxt").attr("selectedItem", sselectedCategories.length);
                    $("#categoryselectorTxt").css("font-weight", "bold");
                }
                else {
                    $("#categoryselectorTxt").html("Select Categories");
                    $("#categoryselectorTxt").attr("selectedItem", sselectedCategories.length);
                    $("#categoryselectorTxt").css("font-weight", "normal");
                }
            },

            showSelectedSubCategoriesInMenu: function () {
                var sselectedSubCategories = $(".subcategorylist-panel :checked");
                if (sselectedSubCategories.length == 1) {
                    $("#subCategoryselectorTxt").html(sselectedSubCategories.length + " Sub Category Selected");
                    $("#subCategoryselectorTxt").attr("selectedItem", sselectedSubCategories.length);
                    $("#subCategoryselectorTxt").css("font-weight", "bold");
                }
                else if (sselectedSubCategories.length > 1) {
                    $("#subCategoryselectorTxt").html(sselectedSubCategories.length + " Sub Categories Selected");
                    $("#subCategoryselectorTxt").attr("selectedItem", sselectedSubCategories.length);
                    $("#subCategoryselectorTxt").css("font-weight", "bold");
                }
                else {
                    $("#subCategoryselectorTxt").html("Select Sub Categories");
                    $("#subCategoryselectorTxt").attr("selectedItem", sselectedSubCategories.length);
                    $("#subCategoryselectorTxt").css("font-weight", "normal");
                }
            },

            showSelectedDepartmentsInMenu: function () {
                var sselectedDepartments = $(".departmentlist-panel :checked");
                if (sselectedDepartments.length == 1) {
                    $("#departmentselectorTxt").html(sselectedDepartments.length + " Department Selected");
                    $("#departmentselectorTxt").attr("selectedItem", sselectedDepartments.length);
                    $("#departmentselectorTxt").css("font-weight", "bold");
                }
                else if (sselectedDepartments.length > 1) {
                    $("#departmentselectorTxt").html(sselectedDepartments.length + " Departments Selected");
                    $("#departmentselectorTxt").attr("selectedItem", sselectedDepartments.length);
                    $("#departmentselectorTxt").css("font-weight", "bold");
                }
                else {
                    $("#departmentselectorTxt").html("Select Departments");
                    $("#departmentselectorTxt").attr("selectedItem", sselectedDepartments.length);
                    $("#departmentselectorTxt").css("font-weight", "normal");
                }

            },

            budgetTypeCheckBox: function (currentCheckbox) {
                var sCategoryTypes = $(".categorytypelist-panel :checkbox");
                $.each(sCategoryTypes, function (i, item) {
                    if ($(currentCheckbox).val() != $(item).val()) {
                        $(item).removeAttr("checked");
                    }
                });

                this.sCategoryType = "";
                this.sCategories = new Array();
                this.sSubCategories = new Array();

                var sCategories = this.sCategories;
                var sSubCategories = this.sSubCategories;

                if ($(currentCheckbox).is(":checked")) {
                    this.sCategoryType = $(currentCheckbox).val();

                    //hide all categories and sub categories
                    var allCategories = $(".categorylist-panel :checkbox");
                    var allSubCategories = $(".subcategorylist-panel :checkbox");
                    $.each(allCategories, function (i, item) {
                        $(item).removeAttr("checked");
                        $(item).parent().css("display", "none");

                    });
                    $.each(allSubCategories, function (i, item) {
                        $(item).removeAttr("checked");
                        $(item).parent().css("display", "none");

                    });



                    //Get all specified categorytype's categories and sub categories
                    var categories = $(".categorylist-panel :checkbox[btype='" + this.sCategoryType + "']")
                    var subCategories = $(".subcategorylist-panel :checkbox[btype='" + this.sCategoryType + "']");
                    $.each(categories, function (i, item) {
                        $(item).attr("checked", "checked");
                        $(item).parent().css("display", "block");
                        sCategories.push($(item).val());
                    });

                    $.each(subCategories, function (i, item) {
                        $(item).attr("checked", "checked");
                        $(item).parent().css("display", "block");
                        sSubCategories.push($(item).attr("sid"));
                    });



                    this.sCategories = sCategories;
                    this.sSubCategories = sSubCategories;
                }
                else {

                    var allCategories = $(".categorylist-panel :checkbox");
                    var allSubCategories = $(".subcategorylist-panel :checkbox");
                    $.each(allCategories, function (i, item) {
                        $(item).removeAttr("checked");
                        $(item).parent().css("display", "block");
                    });

                    $.each(allSubCategories, function (i, item) {
                        $(item).removeAttr("checked");
                        $(item).parent().css("display", "none");
                    });
                }

                this.showSelectedBudgetTypesInMenu();
                this.showSelectedCategoriesInMenu();
                this.showSelectedSubCategoriesInMenu();
            },

            categoryCheckBox: function (currentCheckbox) {
             
                $("#selectAllSubCategoryAction").attr("checked", "checked");
                var selectCategoriesTemp = $(".categorylist-panel :checked");

                this.sCategories = new Array();
                this.sSubCategories = new Array();
                var sCategories = this.sCategories;
                var sSubCategories = this.sSubCategories;

                $.each(selectCategoriesTemp, function (i, item) {
                    sCategories.push($(item).val());
                });


                var allSubCChildren = $(".subcategorylist-panel :checkbox");
                $.each(allSubCChildren, function (i, item) {
                    $(item).removeAttr("checked");
                    $(item).parent().css("display", "none");
                });

                var attrClause = "";
                for (var i = 0; i < this.sCategories.length; i++) {
                    if (i != 0) {
                        attrClause = attrClause + ",";
                    }
                    attrClause = attrClause + " :checkbox[pid='" + this.sCategories[i] + "'] ";
                }

                if (attrClause && attrClause != "") {
                    var children = $(".subcategorylist-panel " + attrClause);
                    var subCategories = "";
                    $.each(children, function (i, item) {
                        $(item).parent().css("display", "block");
                        $(item).attr("checked", "checked");
                        sSubCategories.push($(item).attr("sid"));
                    });
                }


                this.showSelectedCategoriesInMenu();
                this.showSelectedSubCategoriesInMenu();
            },

            subCategoryCheckBox: function (currentCheckbox) {

                if (!$(currentCheckbox).is(":checked")) {
                    $("#selectAllSubCategoryAction").attr("checked", false);
                }


                this.sSubCategories = new Array();
                var sselectedSubCategories = $(".subcategorylist-panel :checked")
                for (var i = 0; i < sselectedSubCategories.length; i++) {
                    this.sSubCategories.push($(sselectedSubCategories[i]).attr("sid"));
                }

                this.showSelectedSubCategoriesInMenu();

            },

            departmentCheckBox: function (currentCheckbox) {
                if (!$(currentCheckbox).is(":checked")) {
                    $("#selectAllDepartmentAction").attr("checked", false);
                }

                var companyId = $(currentCheckbox).attr("cId");
                if (companyId && companyId != "") {
                    var children = $(".departmentlist-panel :checkbox[pid='" + companyId + "']")
                    if ($(currentCheckbox).is(":checked")) {
                        $.each(children, function (i, item) {
                            $(item).attr("checked", "checked");
                        });
                    }
                    else {
                        $.each(children, function (i, item) {
                            $(item).removeAttr("checked");
                        });
                    }
                }
                else {
                    var parentID = $(currentCheckbox).attr("pId");
                    var parent = $(".departmentlist-panel :checkbox[cid='" + parentID + "']")
                    var children = $(".departmentlist-panel :checkbox[pid='" + parentID + "']").filter(":not(:checked)");
                    if (parent.length > 0) {
                        if (children.length <= 0) {
                            parent.attr("checked", "checked");
                        }
                        else {
                            parent.removeAttr("checked");
                        }
                    }
                }

                this.sDepartments = new Array();

                var ssDepartments = $(".departmentlist-panel :checked");
                for (var j = 0; j < ssDepartments.length; j++) {
                    this.sDepartments.push($(ssDepartments[j]).attr("did"));
                }

                this.showSelectedDepartmentsInMenu();
            },

            executeFilter: function () {
            

                var filterString = "";
                if (this.sCategories.length > 0) {

                    for (var i = 0; i < this.sCategories.length; i++) {
                        if (i != 0) {
                            filterString += ", ";
                        }
                        filterString += this.sCategories[i];
                        var selectedCheckBoxes = $(".subcategorylist-panel :checkbox[pid='" + this.sCategories[i] + "']").filter(":checked");
                        var children = selectedCheckBoxes;
                        if (children.length > 0) {
                            var subCategories = "[";
                            $.each(children, function (j, jItem) {
                                if (j != 0) {
                                    subCategories += ", ";
                                }
                                subCategories += $(jItem).val();
                            });
                            subCategories += "]";
                            filterString += subCategories;
                        }
                    }
                    debugger;
                    var selectedCategoryIDs = "";
                    for (var j = 0; j < this.sSubCategories.length; j++) {
                        if (j != 0) {
                            selectedCategoryIDs += ",";
                        }
                        selectedCategoryIDs += this.sSubCategories[j];
                    }





                }
                var dFilterString = "";
                if (this.sDepartments.length > 0) {

                    var children = $(".departmentlist-panel :checked");
                    if (children.length > 0) {
                        $.each(children, function (j, jItem) {
                            if (j != 0) {
                                dFilterString += ", ";
                            }
                            dFilterString += $(jItem).val();
                        });
                    }

                    var selectedDepartmentIDs = "";
                    for (var j = 0; j < this.sDepartments.length; j++) {
                        if (j != 0) {
                            selectedDepartmentIDs += ",";
                        }
                        selectedDepartmentIDs += this.sDepartments[j];
                    }


                }

                $("#selectedCriteria").html("");
                $("#<%= hfSelectedCategories.ClientID %>").val("");
                if (filterString != "") {
                    $("#selectedCriteria").html("<b> Categories: </b>" + filterString);
                    $("#<%= hfSelectedCategories.ClientID %>").val(selectedCategoryIDs);
                    $(".selectedcriterial-div").show("medium");
                }




                $("#selectedDepartmentCriteria").html("");
                $("#<%= hfSelectedDepartments.ClientID %>").val("");
                if (dFilterString != "") {
                    $("#selectedDepartmentCriteria").html("<b> Departments: </b>" + dFilterString);
                    $("#<%= hfSelectedDepartments.ClientID %>").val(selectedDepartmentIDs);
                    $(".selectedcriterial-div").show("medium");
                }

                if ($("#selectedCriteria").html() == "" && $("#selectedDepartmentCriteria").html() == "") {
                    $(".selectedcriterial-div").hide("medium");
                }

                $(".btrefresh").get(0).click();
            }

        };






    </script>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

        $(function () {

            initialize();

            $(".categorytypelist-panel :checkbox").bind("click", function () {
                window.itgBudgetFilter.budgetTypeCheckBox(this);
            });


            $(".categorylist-panel :checkbox").bind("click", function () {
                window.itgBudgetFilter.categoryCheckBox(this);
            });


            $(".subcategorylist-panel :checkbox").bind("click", function () {
                window.itgBudgetFilter.subCategoryCheckBox(this);
            });


            $(".departmentlist-panel :checkbox").bind("click", function () {
                window.itgBudgetFilter.departmentCheckBox(this);
            });

        });

        function initialize() {
            window.itgGrid.expandCollapseDefault();

            $("#budgetPopup .trfbudgetcategory select").bind("change", function () {
                window.itgBudget.changeGLCode();
            });



            $("#budgetActualPopup .trfactualcategory select").bind("change", function () {
                window.itgBudgetActual.changeGLCode();
            });



            $("#budgetActualPopup .trfactualbudget .ugit-dropdown li").bind("click", function () {
                window.itgBudgetActual.selectBugdetItemForActual(this);
            });

            $(".budget-item").mouseover(function () {

                window.itgGrid.showBudgetAction(this);
            });
            $(".budget-item").mouseout(function () {
                window.itgGrid.hideBudgetAction(this);

            });

            $(".actual-item").mouseover(function () {
                window.itgGrid.showBudgetActualAction(this);
            });
            $(".actual-item").mouseout(function () {
                window.itgGrid.hideBudgetActualAction(this);
            });


            var dd = new DropDown($('.ugit-dropdown'));
            $(document).click(function () {
                // all dropdowns
                $('.ugit-dropdown').removeClass('active');
            });



        }

        function startPrint(obj) {
            //  showAll();

            $("#printTD").html($("#tdBudgetDetail").get(0).innerHTML);

            var cssDiv = document.createElement("div");
            $(cssDiv).append($("#cssDiv").get(0).innerHTML);
            var css = new Array();
            css.push('<style type="text/css">');
            css.push('.bitemtable-header {background:#E1E1E1 !important; }');
            css.push('.bitemtable .grandtotal{background:#E1E1E1 !important;}');
            css.push('.bitemtable .categorytype-item {background:#F0F0F0 !important;}');
            css.push('.bitemtable .category-item{background:#F0F0F0 !important;}');
            css.push('.bitemtable .subcategory-item{background:#FAFAFA !important;}');
            css.push('.bitemtable .bugdet-itemalt{background:#F7F7F7 !important;}');
            css.push('.actual-head td:first-child{width:40px;background:#fff !important;}');
            css.push('.ms-alternatingstrong{width:40px;background:#F2F2F2 !important;}');
            css.push('.ugitlight1lighter {background-color:#E1E1E1 !important;}');
            css.push('</style>');

            css.push('<script type="text/javascript">');

            css.push('$(function () {');

            css.push('$("#printTD :hidden").show();');
            css.push('$("#printTD #btnAddBudgetActual").remove();');
            css.push('$("#printTD #btnAddBudgetItem").remove();');

            css.push('$("#printTD tr").each(function () {');
            css.push('$(this).find("td").each(function () {');
            css.push('$(this).find(".hide").remove();');
            css.push('$(this).find("img").remove();');
            css.push('});');
            css.push('});');
            css.push('});');

            $(cssDiv).append(css.join(" "));
            Popup($("#printTD").get(0).innerHTML, cssDiv.innerHTML);
        }

        function Popup(data, headerData) {
            var mywindow = window.open();
            mywindow.document.write('<html><head><title>ITG Budget</title>');
            mywindow.document.write(headerData);
            mywindow.document.write('</head><body >');
            mywindow.document.write(data);
            mywindow.document.write('</body></html>');
            mywindow.document.close();
            mywindow.focus();
            mywindow.print();
            mywindow.close();
            return true;
        }


    </script>

    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

        window.itgBudget = {
            currentBudgetItemID: 0,
            addNewBudget: function (obj) {
                this.clearBudgetForm();
                budgetPopupBox.SetHeaderText("New Budget Item");
                budgetPopupBox.Show();

            },
            clearBudgetForm: function () {
                $("#budgetPopup .trfbudgetcategory select").get(0).selectedIndex = 0;
                $("#budgetPopup .trfbudgetcategory input:hidden").val("");
                $("#budgetPopup .selecteddepartmentdetail").html("");
                $("#budgetPopup .trfglcode .lbbudgetglcode").html("");
                $("#budgetPopup .trfbudgetitem :text").val("");
                $("#budgetPopup .trfbudgetamount :text").val("");

            },
            closePopup: function (clearForm) {
                if (clearForm) {
                    this.clearBudgetForm();
                }

                budgetPopupBox.Hide();
            },
            changeGLCode: function () {
                var categoryBox = $("#budgetPopup .trfbudgetcategory select option:selected");
                var departmentBox = $("#budgetPopup .selecteddepartmentdetail");
                var glcodeLabel = $("#budgetPopup .trfglcode .lbbudgetglcode");

                var categoryGlCode = $.trim(categoryBox.attr("glcode"));
                var departmentGlCode = $.trim(departmentBox.find("span").attr("code"));
                var glCode = departmentGlCode;

                if (glCode != "" && categoryGlCode != "") {
                    glCode += "-";
                }
                if (categoryGlCode != "") {
                    glCode += categoryGlCode;
                }
                glcodeLabel.html(glCode);
            },
            editBudget: function (obj) {
                
                this.clearBudgetForm();

                var parentTr = $($(obj).parents("tr")[0]);
                var itemJson = $.parseJSON($.trim(parentTr.find(".budgetiteminfo").html()));
                $("#budgetPopup .trfbudgetcategory .ms-formlabel input:hidden").val(itemJson.itemid);
                $("#budgetPopup .trfbudgetcategory select option[value='" + itemJson.cid + "']").attr("selected", "selected");
                //$("#budgetPopup .trfbudgetdepartment select option[value='" + itemJson.did + "']").attr("selected", "selected");
                setUGITDepartment("<%= ctDepartment_Budget.ClientID %>", itemJson.did);
                $("#budgetPopup .trfglcode .lbbudgetglcode").html(itemJson.glcode);
                $("#budgetPopup .trfbudgetitem :text").val(itemJson.title);
                $("#budgetPopup .trfbudgetamount :text").val(itemJson.amount);
                $("#budgetPopup .trfbudgetstartdate :text").val(itemJson.startdate);
                $("#budgetPopup .trfbudgetenddate :text").val(itemJson.enddate);

                var dateStart = new Date(itemJson.startdate);
                var dStartDate = new Date(dateStart.getFullYear(), dateStart.getMonth(), dateStart.getDate());
                var dateEnd = new Date(itemJson.enddate);
                var ddateEnd = new Date(dateEnd.getFullYear(), dateEnd.getMonth(), dateEnd.getDate());
                dtcBudgetStartDate.SetDate(dStartDate);
                dtcBudgetEndDate.SetDate(ddateEnd);

                budgetPopupBox.SetHeaderText("Update Budget Item");
                budgetPopupBox.Show();
            },
            deleteBudget: function (obj) {
                
                var parentTr = $($(obj).parents("tr")[0]);
                var itemJson = $.parseJSON($.trim(parentTr.find(".budgetiteminfo").html()));

                if (confirm("Are you sure you want to delete this budget item? This will also delete any actuals for this item:\n\"" + itemJson.title + "\"")) {
                    $("#budgetPopup .trfbudgetcategory .ms-formlabel input:hidden").val(itemJson.itemid);
                    $(".btdeletebudget").get(0).click();
                }
            },
            changeDepartment: function (deptID) {
                //debugger;
                this.changeGLCode();
            }
        }


        window.itgBudgetActual = {
            currentBudgetItemID: 0,
            changeGLCode: function () {

                var categoryBox = $("#budgetActualPopup .trfactualcategory select option:selected");
                var departmentBox = $("#budgetActualPopup .selecteddepartmentdetail");

                var glcodeLabel = $("#budgetActualPopup .trfactualglcode .lbactualglcode");
                var categoryGlCode = $.trim(categoryBox.attr("glcode"));
                var departmentGlCode = $.trim(departmentBox.find("span").attr("code"));
                var glCode = departmentGlCode;
                if (glCode != "" && categoryGlCode != "") {
                    glCode += "-";
                }
                if (categoryGlCode != "") {
                    glCode += categoryGlCode;
                }
                glcodeLabel.html(glCode);
                //debugger;

                var budgetItemBox = $("#budgetActualPopup .trfactualbudget .ugit-dropdown");

                var budgetItemOptions = budgetItemBox.find("li");
                if (categoryBox.val() == "" && departmentBox.val() == "") {
                    budgetItemOptions.show();
                }
                else if (categoryBox.val() == "") {
                    for (var i = 1; i < budgetItemOptions.length; i++) {
                        $(budgetItemOptions[i]).show();
                        if (!$(budgetItemOptions[i]).is("[glcode^='" + glCode + "']")) {
                            $(budgetItemOptions[i]).hide();
                        }
                    }
                }
                else if (departmentBox.val() == "") {
                    for (var i = 1; i < budgetItemOptions.length; i++) {
                        $(budgetItemOptions[i]).show();
                        if (!$(budgetItemOptions[i]).is("[glcode$='" + glCode + "']")) {
                            $(budgetItemOptions[i]).hide();
                        }
                    }

                }
                else {
                    for (var i = 1; i < budgetItemOptions.length; i++) {
                        $(budgetItemOptions[i]).show();
                        if (!$(budgetItemOptions[i]).is("[glcode='" + glCode + "']")) {
                            $(budgetItemOptions[i]).hide();
                        }
                    }
                }

                var previosuVal = budgetItemBox.find("input:hidden").val();
                if (previosuVal) {
                    var selectedBudgetItem = budgetItemBox.find("li[value='" + previosuVal + "']");
                    if (selectedBudgetItem.length > 0) {
                        budgetItemBox.find("label").html(selectedBudgetItem.html());
                        budgetItemBox.find("input:hidden").val(selectedBudgetItem.attr("value"));
                    }
                    else {
                        budgetItemBox.find("label").html("--Select--");
                        budgetItemBox.find("input:hidden").val("");
                    }
                }
                else {
                    budgetItemBox.find("label").html("--Select--");
                    budgetItemBox.find("input:hidden").val("");
                }


            },
            addNewActual: function (obj) {
                this.clearActualForm();
                budgetActualPopupBox.SetHeaderText("Add New Actual");
                budgetActualPopupBox.Show();
            },
            clearActualForm: function () {
                $("#budgetActualPopup .trfactualcategory select").get(0).selectedIndex = 0;
                $("#budgetPopup .selecteddepartmentdetail").html("");
                $("#budgetActualPopup .trfactualglcode .lbactualglcode").html("");

                $("#budgetActualPopup .trfactualbudget .ugit-dropdown li").show();
                $("#budgetActualPopup .trfactualbudget input:hidden").val("");
                $("#budgetActualPopup .trfactualbudget .ugit-dropdown label").html("--Select--");
                $("#budgetActualPopup .trfactualvender select").get(0).selectedIndex = 0;
                $("#budgetActualPopup .trfactualinvoice :text").val("");
                $("#budgetActualPopup .trfactualdescription :text").val("");
                $("#budgetActualPopup .trfactualamount :text").val("");
                $("#budgetActualPopup .trfactualdescription input:hidden").val("");

                $("#budgetActualPopup .trfactualstartdate :text").val("");
                $("#budgetActualPopup .trfactualenddate :text").val("");
            },
            closePopup: function (clearForm) {
                if (clearForm) {
                    this.clearActualForm();
                }
                budgetActualPopupBox.Hide();
            },
            submitPopup: function () {
                var budgetVal = $("#<%=hfEditBudgetActualID.ClientID%>").val();
                if (budgetVal == "" || budgetVal == "0") {
                    return false;
                }
                else {
                    // $("#budgetActualPopup").hide();
                }
            },
            changeDepartment: function (deptID) {
                this.changeGLCode();

            },
            addNewActualFromBudget: function (obj) {
                //debugger;
                this.clearActualForm();
                var parentTr = $($(obj).parents("tr")[0]);
                var itemJson = $.parseJSON($.trim(parentTr.find(".budgetiteminfo").html()));


                //debugger;
                //$("#budgetActualPopup .trfactualstartdate :text").val(itemJson.startdate);
                //$("#budgetActualPopup .trfactualenddate :text").val(itemJson.enddate);
                setUGITDepartment("<%= ctDepartment_Actual.ClientID %>", itemJson.did);
                var budgetItemBox = $("#budgetActualPopup .trfactualbudget .ugit-dropdown");
                var selectedBudgetItem = budgetItemBox.find("li[value='" + itemJson.itemid + "']");

                if (selectedBudgetItem.length > 0) {
                    budgetItemBox.find("label").html(selectedBudgetItem.html());
                    budgetItemBox.find("input:hidden").val(selectedBudgetItem.attr("value"));
                }
                else {
                    budgetItemBox.find("label").html("--Select--");
                    budgetItemBox.find("input:hidden").val("");
                }

                this.selectCategoryDepart();

                if (selectedBudgetItem.length > 0) {
                    budgetItemBox.find("label").html(selectedBudgetItem.html());
                    budgetItemBox.find("input:hidden").val(selectedBudgetItem.attr("value"));
                }
                else {
                    budgetItemBox.find("label").html("--Select--");
                    budgetItemBox.find("input:hidden").val("");
                }

                budgetActualPopupBox.SetHeaderText("New Actual For Budget");
                budgetActualPopupBox.Show();
            },
            editActual: function (obj) {
                
                this.clearActualForm();
                var parentTr = $($(obj).parents("tr")[0]);
                var itemJson = $.parseJSON($.trim(parentTr.find(".budgetactualinfo").html()));

                $("#budgetActualPopup .trfactualdescription input:hidden").val(itemJson.itemid);

                setUGITDepartment("<%= ctDepartment_Actual.ClientID %>", itemJson.did);
                var budgetItemBox = $("#budgetActualPopup .trfactualbudget .ugit-dropdown");
                var selectedBudgetItem = budgetItemBox.find("li[value='" + itemJson.bid + "']");

                if (selectedBudgetItem.length > 0) {
                    budgetItemBox.find("label").html(selectedBudgetItem.html());
                    budgetItemBox.find("input:hidden").val(selectedBudgetItem.attr("value"));
                }
                else {
                    budgetItemBox.find("label").html("&nbsp;");
                    budgetItemBox.find("input:hidden").val("");
                }

                $("#budgetActualPopup .trfactualvender select option[value='" + itemJson.vid + "']").attr("selected", "selected");
                $("#budgetActualPopup .trfactualinvoice :text").val(itemJson.invoiceno);
                $("#budgetActualPopup .trfactualdescription :text").val(itemJson.title);
                $("#budgetActualPopup .trfactualamount :text").val(itemJson.amount);
                $("#budgetActualPopup .trfactualstartdate :text").val(itemJson.startdate);
                $("#budgetActualPopup .trfactualenddate :text").val(itemJson.enddate);
                var dateStart = new Date(itemJson.startdate);
                var dStartDate = new Date(dateStart.getFullYear(), dateStart.getMonth(), dateStart.getDate());
                var dateEnd = new Date(itemJson.enddate);
                var ddateEnd = new Date(dateEnd.getFullYear(), dateEnd.getMonth(), dateEnd.getDate());
                dtcActualStartDate.SetDate(dStartDate);
                dtcActualEndDate.SetDate(ddateEnd);

                this.selectCategoryDepart();

                var budgetItemBox = $("#budgetActualPopup .trfactualbudget .ugit-dropdown");

                if (selectedBudgetItem.length > 0) {
                    budgetItemBox.find("label").html(selectedBudgetItem.html());
                    budgetItemBox.find("input:hidden").val(selectedBudgetItem.attr("value"));
                }
                else {
                    budgetItemBox.find("label").html("&nbsp;");
                    budgetItemBox.find("input:hidden").val("");
                }




                budgetActualPopupBox.SetHeaderText("Update Budget Actual");
                budgetActualPopupBox.Show();


            },
            selectBugdetItemForActual: function (obj) {
                $("#budgetActualPopup .trfactualbudget .ugit-dropdown input:hidden").val($(obj).attr("value"));
                $("#budgetActualPopup .trfactualbudget .ugit-dropdown label").html($(obj).html());

            },
            selectCategoryDepart: function () {
                //debugger;
                var budgetID = $("#budgetActualPopup .trfactualbudget .ugit-dropdown input:hidden").val();
                var budgetItemObj = $("#budgetActualPopup .trfactualbudget .ugit-dropdown li[value='" + budgetID + "']");

                var glCode = budgetItemObj.attr("glcode");
                var budgetID = budgetItemObj.attr("value");

                if (budgetID != "-1" && glCode) {
                    var categoryBoxs = $("#budgetActualPopup .trfactualcategory select option");
                    $.each(categoryBoxs, function (i, item) {
                        var splitAr = glCode.split($(item).attr("glcode"));
                        if (splitAr.length == 2 && splitAr[1] == "") {
                            $(item).attr("selected", "selected");
                            return;
                        }
                    });

                    var departmentBoxs = $("#budgetActualPopup .trfactualdepartment select option");
                    $.each(departmentBoxs, function (i, item) {
                        var splitAr = glCode.split($(item).attr("glcode"));
                        if (splitAr.length == 2 && splitAr[0] == "") {
                            $(item).attr("selected", "selected");
                            return;
                        }
                    });

                    this.changeGLCode();
                }
            },
            deleteActual: function (obj) {
                
                var parentTr = $($(obj).parents("tr")[0]);
                var itemJson = $.parseJSON($.trim(parentTr.find(".budgetactualinfo").html()));
                if (confirm("Are you sure you want to delete this actual?:\n \"" + itemJson.title + "\"")) {
                    $("#budgetActualPopup .trfactualbudget input:hidden").val(itemJson.itemid.toString());
                    $(".btdeletebudgetactual").get(0).click();
                }
            }
        }


        window.itgGrid = {

            expandByTr: function (trObj) {
                var startLevel = 1;
                if ($(".penablecategorytype").length > 0) {
                    startLevel = 0;
                }

                var currentTr = $(trObj);
                var currentLevel = parseInt(currentTr.attr("level"));
                var trList = $(".bitemtable .bitem");
                var currentIndex = $.inArray(currentTr.get(0), trList)

                currentTr.attr("isExpand", "true")
                currentTr.find(".expandcollapse-icon").attr("src", "/Content/images/minus.png");

                for (var i = currentIndex + 1; i < trList.length; i++) {
                    var level = parseInt($(trList[i]).attr("level"));

                    if (level > currentLevel) {
                        if ($(trList[i]).attr("isExpand") && $(trList[i]).attr("isExpand") == "true") {
                            $(trList[i]).removeAttr("isExpand");
                            $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                        }
                        if (level == currentLevel + 1) {
                            $(trList[i]).removeClass("hide");
                        }
                        else {
                            $(trList[i]).addClass("hide");
                        }
                    }
                    else {
                        break;
                    }
                }

            },
            expandCollpaseBudgetCategory: function (obj) {

                var startLevel = 1;
                if ($(".penablecategorytype").length > 0) {
                    startLevel = 0;
                }

                var currentTr = $($(obj).parents("tr").get(0));
                var currentLevel = parseInt(currentTr.attr("level"));
                var trList = $(".bitemtable .bitem");
                var currentIndex = $.inArray(currentTr.get(0), trList)
                var doExpand = false;


                if (currentTr.attr("isExpand") == "true") {
                    currentTr.removeAttr("isExpand")

                }
                else {
                    doExpand = true;
                    currentTr.attr("isExpand", "true")

                }


                if (doExpand) {
                    currentTr.find(".expandcollapse-icon").attr("src", "/Content/images/minus.png");

                    for (var i = currentIndex + 1; i < trList.length; i++) {
                        var level = parseInt($(trList[i]).attr("level"));

                        if (level > currentLevel) {

                            if ($(trList[i]).attr("isExpand") && $(trList[i]).attr("isExpand") == "true") {
                                $(trList[i]).removeAttr("isExpand");
                                $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                            }

                            if (level == currentLevel + 1) {
                                $(trList[i]).removeClass("hide");
                            }
                            else {
                                $(trList[i]).addClass("hide");
                            }
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    currentTr.find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");

                    for (var i = currentIndex + 1; i < trList.length; i++) {
                        var level = parseInt($(trList[i]).attr("level"));

                        if (level > currentLevel) {

                            if ($(trList[i]).attr("isExpand") && $(trList[i]).attr("isExpand") == "true") {
                                $(trList[i]).removeAttr("isExpand");
                                $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                            }
                            $(trList[i]).addClass("hide");
                        }
                        else {
                            break;
                        }
                    }
                }

                window.itgGrid.rememberCollapsedRowsRows();
            },
            expandCollapseDefault: function () {
                var startLevel = 1;
                if ($(".penablecategorytype").length > 0) {
                    startLevel = 0;
                }

                var trList = $(".bitemtable .bitem");
                var startLevelItems = $(".bitemtable .bitem[level='" + startLevel + "']");

                if (startLevelItems.length > 3) {
                    for (var i = 0; i < trList.length; i++) {
                        var level = parseInt($(trList[i]).attr("level"));
                        var parentNode = $(trList[i]).find(".expandcollapse-icon");
                        if (parentNode.length > 0) {
                            $(trList[i]).removeAttr("isExpand");
                            $(trList[i]).find(".expandcollapse-icon").attr("src", "/Content/images/plus.png");
                        }
                        if (level == startLevel) {
                            $(trList[i]).removeClass("hide");
                        }
                        else {
                            $(trList[i]).addClass("hide");
                        }
                    }
                }
                else {

                    for (var i = 0; i < trList.length; i++) {
                        var level = parseInt($(trList[i]).attr("level"));

                        if (level == startLevel) {
                            $(trList[i]).removeClass("hide");

                            var parentNode = $(trList[i]).find(".expandcollapse-icon");

                            if (parentNode.length > 0) {
                                $(trList[i]).attr("isExpand", "true");
                                parentNode.attr("src", "/Content/images/minus.png");
                            }
                        }
                        else if (level == startLevel + 1) {
                            $(trList[i]).removeClass("hide");

                            var parentNode = $(trList[i]).find(".expandcollapse-icon");
                            if (parentNode.length > 0) {
                                $(trList[i]).removeAttr("isExpand");
                                parentNode.attr("src", "/Content/images/plus.png");
                            }
                        }
                        else {
                            $(trList[i]).addClass("hide");

                            var parentNode = $(trList[i]).find(".expandcollapse-icon");
                            if (parentNode.length > 0) {
                                $(trList[i]).removeAttr("isExpand");
                                parentNode.attr("src", "/Content/images/plus.png");
                            }
                        }
                    }

                }


                var trIndexs = $(".treeStateSpan :hidden").val().split(",");
                if (trIndexs.length > 0) {
                    var trList = $(".bitemtable .bitem");
                    for (var j = 0; j < trIndexs.length; j++) {

                        var expectedTr = $(".bitemtable .bitem[current='" + trIndexs[j] + "']");
                        if (expectedTr.length > 0) {
                            this.expandByTr(expectedTr.get(0));
                        }
                    }
                }
            },
            expandAll: function () {
                var startLevel = 1;
                if ($(".penablecategorytype").length > 0) {
                    startLevel = 0;
                }

                var trList = $(".bitemtable .bitem");
                for (var i = 0; i < trList.length; i++) {
                    $(trList[i]).removeClass("hide");
                    var parentNode = $(trList[i]).find(".expandcollapse-icon");
                    if (parentNode.length > 0) {
                        $(trList[i]).attr("isExpand", "true");
                        parentNode.attr("src", "/Content/images/minus.png");
                    }
                }
            },
            showBudgetAction: function (obj) {
                var actionCtr = $(obj).find(".action-container");
                actionCtr.removeClass("hide");
            },
            hideBudgetAction: function (obj) {
                var actionCtr = $(obj).find(".action-container");
                actionCtr.addClass("hide");
            },
            showBudgetActualAction: function (obj) {
                var actionCtr = $(obj).find(".action-container");
                actionCtr.removeClass("hide");
            },
            hideBudgetActualAction: function (obj) {
                var actionCtr = $(obj).find(".action-container");
                actionCtr.addClass("hide");
            },
            rememberCollapsedRowsRows: function () {
                var expandedTrList = $(".bitemtable .bitem[isExpand='true']");
                var visibleIndexes = "";
                for (var i = 0; i < expandedTrList.length; i++) {
                    if (i != 0) {
                        visibleIndexes += ",";
                    }
                    visibleIndexes += $(expandedTrList[i]).attr("current");
                }
               
                $(".treeStateSpan :hidden").val(visibleIndexes);
                $.cookie("expandedState", unescape($(".treeStateSpan :hidden").val()));
            },
            forgetVisibleRows: function () {
                $(".treeStateSpan :hidden").val("");
            }
        }

        function reportItemMouseOver(obj) {
            $(obj).removeClass("ugitlinkbg");
            $(obj).addClass("ugitsellinkbg");
        }

        function reportItemMouseOut(obj) {

            $(obj).removeClass("ugitsellinkbg");
            $(obj).addClass("ugitlinkbg");

        }

        /* PMM Budget Report */
        function OpenBudgetReportPopup(obj) {
            showhideReportMenu(obj);

            var path = "";
            var title = "";

            path = "<%=budgetReportUrl%>";
            title = "Non-Project Budget Summary Report";

            window.parent.UgitOpenPopupDialog(path, "", title, 80, 90, false, escape(window.location.href));
            return false;
        }

        function OpenActualsReportPopup(obj) {
            showhideReportMenu(obj)

            var path = "";
            var title = "";

            path = "<%=actualsReportUrl%>";
            title = "Non-Project Actuals Report";


            window.parent.UgitOpenPopupDialog(path, "", title, 80, 90, false, escape(window.location.href));
            return false;
        }

        function showhideReportMenu(obj) {
            if ($("#report-options").hasClass("selected-export")) {
                $("#report-options").hide(300);
                $("#report-options").removeClass("selected-export");
            }
            else {
                $("#report-options").show(300);
                $("#report-options").addClass("selected-export");
            }
        }
    </script>
    <asp:HiddenField ID="hdnCurrentYear" runat="server" />
    <table style="border-collapse: collapse; width: 99%;" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left" valign="top" style="padding-bottom: 7px;">
                <%--Select Type menu button--%>
                <div style="float: left; width: 240px;" id="divBudgetType" runat="server" visible="false">
                    <button class="ui-multiselect categorytypeselector" onclick="window.itgBudgetFilter.showHideCategoryTypeMenu();return false;" style="width: 190px;">
                        <span class="ui-icon"></span>
                        <span id='categorytypeselectorTxt'>Select Type</span>
                    </button>
                    <div class="categorymenu-container" id="categoryTypeItems">
                        <asp:Panel ID="categoryTypeListPanel" CssClass="categorytypelist-panel" runat="server">
                        </asp:Panel>
                        <div style="float: left; width: 100%;" class="ms-alternatingstrong">
                            <span style="float: right;">
                                <input type="button" value="Done" style="cursor: pointer" id="hideCategoryTypeOptionButton" onclick="window.itgBudgetFilter.showHideCategoryTypeMenu()" /></span>
                        </div>
                    </div>
                </div>

                <%--Categories menu button--%>
                <div style="float: left; width: 240px;" id="divCategoryFilter" runat="server">
                    <button class="ui-multiselect categoryselector" style="width: 190px;" onclick="window.itgBudgetFilter.showHideCategoriesMenu();return false;">
                        <span class="ui-icon"></span>
                        <span id='categoryselectorTxt'>Select Categories</span>
                    </button>
                    <div class="categorymenu-container" id="categoryItems">
                        <asp:Panel ID="categoryListPanel" CssClass="categorylist-panel" runat="server">
                        </asp:Panel>
                        <div style="float: left; width: 100%;" class="ms-alternatingstrong">
                            <span style="float: right;">
                                <input type="button" value="Done" class="input-button-bg" style="cursor: pointer" id="hideCategoryOptionButton" onclick="window.itgBudgetFilter.showHideCategoriesMenu()" /></span>
                        </div>
                    </div>
                </div>

                <%-- Sub Categories menu button--%>
                <div style="float: left; width: 280px;" id="divSubCategoryFilter" runat="server">
                    <button class="ui-multiselect subcategoryselector" onclick="window.itgBudgetFilter.showHideSubCategoriesMenu();return false;" style="width: 225px;">
                        <span class="ui-icon"></span>
                        <span id='subCategoryselectorTxt'>Select Sub Categories</span>
                    </button>
                    <div class="categorymenu-container" id="subCategoryItems">
                        <asp:Panel ID="subCategoryListPanel" CssClass="subcategorylist-panel" runat="server" OnLoad="CategoryListPanel_Load">
                        </asp:Panel>
                        <div style="float: left; width: 100%;" class="ms-alternatingstrong">
                            <span style="float: left;">
                                <input type="checkbox" value="selectAll" id="selectAllSubCategoryAction" onclick="window.itgBudgetFilter.selectAllSubCategoryCheck(this);" />
                            </span>
                            <span style="float: left; padding-top: 2px">Select All</span>
                            <span style="float: right;">
                                <input type="button" value="Done" id="hideSubCategoryOptionButton" class="input-button-bg" style="cursor: pointer" onclick="window.itgBudgetFilter.showHideSubCategoriesMenu()" /></span>
                        </div>
                    </div>
                </div>

                <%--Department Menu button--%>
                <div style="float: left; width: 280px;" id="divDepartmentFilter" runat="server">
                    <button class="ui-multiselect departmentselector" onclick="window.itgBudgetFilter.showHideDepartmentMenu();return false;" style="width: 225px;">
                        <span class="ui-icon"></span>
                        <span id='departmentselectorTxt'>Select <%= departmentLabel %>(s)</span>
                    </button>
                    <div style="z-index: 10000; float: left; width: 250px; border: 1px solid; display: none; position: absolute; background: #ffffff" id="departmentItems">
                        <asp:Panel ID="departmentListPanel" CssClass="departmentlist-panel" runat="server" OnLoad="DepartmentListPanel_Load">
                        </asp:Panel>
                        <div style="float: left; width: 100%;" class="ms-alternatingstrong">
                            <span style="float: left;">
                                <input type="checkbox" value="selectAll" id="selectAllDepartmentAction" onclick="window.itgBudgetFilter.selectAllDepartmentCheck(this);" /></span><span style="float: left; padding-top: 2px">Select All</span>
                            <span style="float: right;">
                                <input type="button" value="Done" id="hideDepartOptionButton" class="input-button-bg" style="cursor: pointer" onclick="window.itgBudgetFilter.showHideDepartmentMenu()" /></span>
                        </div>
                    </div>
                </div>

                <asp:TextBox ID="hfSelectedCategories" runat="server" CssClass="hide" />
                <asp:TextBox ID="hfSelectedDepartments" runat="server" CssClass="hide" />
            </td>
        </tr>
        <tr>
            <td>
                <div style="float: left; width: 98%; padding: 5px; display: none" class='ugitaccent1lightest selectedcriterial-div'><i id="selectedCriteria"></i><i id="selectedDepartmentCriteria"></i></div>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" style="padding-bottom: 7px; height: 400px;">
                <div style="display: none">
                    <dx:ASPxDateEdit ID="dummyCalendar" runat="server" Visible="true"></dx:ASPxDateEdit>
                    <%--<SharePoint:DateTimeControl runat="server" ID="dummyCalendar" Visible="true" />--%>
                </div>
                <asp:UpdatePanel runat="server" ID="updatepanelAcutal" UpdateMode="Conditional">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>
                        <span style="display: none;" class="treeStateSpan">
                            <asp:HiddenField ID="hfTreeState" runat="server" />
                        </span>

                        <asp:Button ID="btDeleteBudget" runat="server" ValidationGroup="refreshgrid" CssClass="hide btdeletebudget" OnClick="BtDeleteBudget_Click" />
                        <asp:Button ID="btDeleteActual" runat="server" ValidationGroup="refreshgrid" CssClass="hide btdeletebudgetactual" OnClick="BtDeleteActual_Click" />
                        <asp:Button ID="btRefresh" CssClass="hide btrefresh" runat="server" ValidationGroup="refreshgrid" />

                        <table style="width: 100%;">
                            <tr>
                                <td style="text-align: center; padding-top: 15px; background: none;">
                                    <asp:Label ID="budgetMessage" runat="server" EnableViewState="false" CssClass="errormessage-block ms-alternatingstrong"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="left">
                                    <div style="float: left; width: 100%;" id="divExpCollapse" runat="server">
                                        <b style="float: left; padding-right: 3px; font-weight: normal;">
                                            <img src="/Content/images/itgbudget-icon.png" title="Actual" /></b><b style="float: left; padding-top: 2px;">Budget Items:</b>

                                        <span style="float: left;">
                                            <img src="/Content/images/expand-all.png" title="Expand All" onclick="window.itgGrid.expandAll()" style="cursor: pointer;" />
                                        </span>
                                        <span style="float: left">
                                            <img onclick="window.itgGrid.expandCollapseDefault()" style="cursor: pointer;" src="/Content/images/collapse-all.png" title="Collapse All" />
                                        </span>



                                        <span style="float: right; right: 75px; padding-bottom:2px;"><span>
                                            <asp:DropDownList runat="server" ID="ddlYear" ValidationGroup="refreshgrid" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Calendar Year</asp:ListItem>
                                                <asp:ListItem Value="2">Fiscal Year</asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                            <span id="spancalenderNonbudget" runat="server" visible="false">
                                                <span>
                                                    <asp:ImageButton runat="server" ValidationGroup="refreshgrid" ID="previousYrs" OnClick="previousYrs_Click" ImageUrl="/Content/images/Previous16x16.png" />

                                                </span>
                                                <asp:Label ID="lblSelectedYear" runat="server" Style=""></asp:Label>
                                                <span style="padding-left: 5px;">
                                                    <asp:ImageButton ValidationGroup="refreshgrid" ImageUrl="/Content/images/Next16x16.png" ID="nextYrs" OnClick="nextYrs_Click" ToolTip="Next Year" runat="server" />
                                                </span></span></span>
                                        <span style="float: right; width: 40px;">
                                            <span id="exportAction" style="padding-left: 3px; float: left; padding-right: 2px;" runat="server">
                                                <img id="imgExportAction" runat="server" src="/Content/images/Reports_16x16.png" onclick="showhideReportMenu(this)" alt="Reports" title="Reports" style="cursor: pointer;" />
                                                <div style="position: relative;">
                                                    <div id="report-options" style="background: none repeat scroll 0% 0% rgb(255, 255, 255); float: left; position: absolute; border: 1px inset; padding: 1px 1px 0px; top: 0px; width: 110px; left: -98px; display: none; z-index: 1;">
                                                        <asp:Panel ID="pnlReportList" runat="server">
                                                        </asp:Panel>
                                                    </div>
                                                </div>
                                            </span>
                                            <span>
                                                <img src="/Content/images/print-icon.png" alt="Print" height="15" width="15" title="Print" style="cursor: pointer;" onclick="startPrint(this);" />
                                            </span>
                                        </span>

                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdBudgetDetail">
                                    <%-- Project budget detail --%>

                                    <asp:Repeater ID="rBudgetInfo" runat="server" OnItemDataBound="RBudgetInfo_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="bitemtable">
                                                <tr class="bitemtable-header">
                                                    <th colspan="2">Budget</th>
                                                    <th><%= departmentLabel %></th>
                                                    <th>GL Code</th>
                                                    <th class="amount">Planned</th>
                                                    <th class="amount">Actual</th>
                                                    <th class="datetime">From</th>
                                                    <th class="datetime">To</th>
                                                </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr id="trCategoryType" runat="server" visible="false" level="0" class="bitem categorytype-item ">
                                                <td colspan="4">
                                                    <img src="/Content/images/minus.png" alt="+" title="Expand/Collpase Category" class='expandcollapse-icon' onclick="window.itgGrid.expandCollpaseBudgetCategory(this);" />
                                                    <asp:Label ID="lbCategoryType" runat="server"></asp:Label>
                                                </td>
                                                <td class="amount">
                                                    <asp:Label ID="lbCategoryTypeAmount" runat="server"></asp:Label></td>
                                                <td class="amount">
                                                    <asp:Label ID="lbCategoryTypeActualAmount" runat="server"></asp:Label></td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr id="trCategory" runat="server" visible="false" level="1" class="bitem category-item">
                                                <td colspan="4" id="tdCategory" runat="server">
                                                    <img src="/Content/images/minus.png" alt="+" title="Expand/Collpase Sub Category" class='expandcollapse-icon' onclick="window.itgGrid.expandCollpaseBudgetCategory(this);" />
                                                    <asp:Label ID="lbCategory" runat="server"></asp:Label>
                                                </td>
                                                <td class="amount">
                                                    <asp:Label ID="lbSubTotalAmount" runat="server"></asp:Label></td>
                                                <td class="amount">
                                                    <asp:Label ID="lbSubTotalActualAmount" runat="server"></asp:Label></td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr id="trSubCategory" runat="server" visible="false" level="2" class="bitem subcategory-item">
                                                <td colspan="4" id="td1" runat="server">
                                                    <img src="/Content/images/minus.png" alt="+" title="Expand/Collpase Budget" class='expandcollapse-icon' onclick="window.itgGrid.expandCollpaseBudgetCategory(this);" />
                                                    <asp:Label ID="lbSubCategory" runat="server"></asp:Label>
                                                </td>
                                                <td class="amount">
                                                    <asp:Label ID="lbSubCategoryAmount" runat="server"></asp:Label></td>
                                                <td class="amount">
                                                    <asp:Label ID="lbSubCategoryActualAmount" runat="server"></asp:Label></td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr id="trBudgetItem" runat="server" visible="false" level="3" class="bitem budget-item">
                                                <td colspan="2" id="tdSubCategory" runat="server">
                                                    <div class="itemtext-div">
                                                        <img id="expcollImg3" runat="server" src="/Content/images/minus.png" alt="+" title="Expand/Collpase Actual" class='expandcollapse-icon' onclick="window.itgGrid.expandCollpaseBudgetCategory(this);" />
                                                        <asp:Label ID="lbItemInfo" runat="server" CssClass="hide budgetiteminfo"></asp:Label>
                                                        <asp:HiddenField ID="hfSubCategoryID" runat="server" />
                                                        <asp:Label ID="lbBudgetItem" runat="server"></asp:Label>
                                                        <span class="action-container hide">
                                                            <img style="cursor: pointer;" src="/Content/images/edit-icon.png" alt="Edit"
                                                                title="Edit" onclick="window.itgBudget.editBudget(this)" />
                                                            <img style="cursor: pointer;" src="/Content/images/delete-icon.png" title="Delete" onclick="window.itgBudget.deleteBudget(this)" />
                                                            <img style="cursor: pointer;" src="/Content/images/invoice-icon.png" alt='Actual' title="Add Actual" onclick="window.itgBudgetActual.addNewActualFromBudget(this);" />
                                                        </span>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbDepartment" runat="server"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lbBudgetCOA" runat="server"></asp:Label></td>
                                                <td class="amount">
                                                    <asp:Label ID="lbAmount" runat="server"></asp:Label></td>
                                                <td class="amount">
                                                    <asp:Label ID="lbActualAmount" runat="server"></asp:Label></td>
                                                <td class="datetime">
                                                    <asp:Label ID="lbStartDate" runat="server"></asp:Label></td>
                                                <td class="datetime">
                                                    <asp:Label ID="lbEndDate" runat="server"></asp:Label></td>
                                            </tr>
                                            <asp:Repeater ID="rBudgetActuals" runat="server" OnItemDataBound="RBudgetActuals_ItemDataBound">
                                                <HeaderTemplate>
                                                    <tr class="bitem actual-head" level="4" id="trBudgetActualItemHead" current="A-header" runat="server">
                                                        <td>&nbsp;</td>
                                                        <td>Actual</td>
                                                        <td>Vendor</td>
                                                        <td colspan="2">PO/Invoice #</td>
                                                        <td></td>
                                                        <td class="datetime"></td>
                                                        <td class="datetime acutal-last-column"></td>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="bitem actual-item" level="4" id="trBudgetActualItem" runat="server">
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <div class="itemtext-div">
                                                                <asp:Label ID="lbBudgetActualInfo" runat="server" CssClass="hide budgetactualinfo"></asp:Label>
                                                                <asp:Label ID="lbBATitle" runat="server"></asp:Label>
                                                                <span class="action-container hide">
                                                                    <img style="cursor: pointer;" alt="Edit" title="Edit" src="/Content/images/edit-icon.png" onclick="window.itgBudgetActual.editActual(this);" />
                                                                    <img style="cursor: pointer;" alt="Delete" title="Delete" src="/Content/images/delete-icon.png" onclick="window.itgBudgetActual.deleteActual(this);" />
                                                                </span>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbBAVendorLookup" runat="server"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbBAInvoiceNumber" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="amount">
                                                            <asp:Label ID="lbBAActualCost" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="datetime">
                                                            <asp:Label ID="lbBABudgetStartDate" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="datetime acutal-last-column">
                                                            <asp:Label ID="lbBABudgetEndDate" runat="server"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <tr class="bitem actual-item actual-item-last" level="4" current="A-footer" id="trBudgetActualItemFooter" runat="server">
                                                        <td></td>
                                                        <td colspan="6"></td>
                                                        <td class="acutal-last-column"></td>
                                                    </tr>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </ItemTemplate>

                                        <FooterTemplate>
                                            <tr id="trTotal" runat="server" visible="true" class="grandtotal">
                                                <td></td>
                                                <td></td>
                                                <td></td>
                                                <td>Grand Total:</td>
                                                <td class="amount">
                                                    <asp:Label ID="lbTotalAmount" runat="server"></asp:Label></td>
                                                <td class="amount">
                                                    <asp:Label ID="lbTotalActualAmount" runat="server"></asp:Label></td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>

                                    <div style="float: left; width: 100%; padding-top: 10px;" id="divActionButton" runat="server">
                                        <span style="float: right;">
                                            <input type="button" class="input-button-bg" value="Add Actual" onclick="window.itgBudgetActual.addNewActual(this);" id="btnAddBudgetActual" />
                                        </span>
                                        <span style="float: right;">
                                            <input type="button" value="Add Budget Item" class="input-button-bg" onclick="window.itgBudget.addNewBudget(this);" id="btnAddBudgetItem" />
                                        </span>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <%-- Project budget popup --%>
                <dx:ASPxPopupControl
                    MaxWidth="350px" MinWidth="150px" MaxHeight="250px" MinHeight="150px" ID="budgetPopupBox" ClientInstanceName="budgetPopupBox"
                    ShowFooter="false" ShowHeader="true" HeaderText="New Budget Item"
                    runat="server" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <div id="budgetPopup">
                                <div>
                                    <div style="float: left; width: 100%;">
                                        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                            width="350px">
                                            <tr class="trfbudgetcategory">
                                                <td class="ms-formlabel">
                                                    <asp:HiddenField ID="hfEditBudgetID" runat="server" />
                                                    <h3 class="ms-standardheader">Category<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:DropDownList ID="ddlBudgetCategories" Width="200" ValidationGroup="formBudget" runat="server"></asp:DropDownList>
                                                    <span style="float: left; width: 100%;">
                                                        <asp:RequiredFieldValidator CssClass="error" Display="Dynamic" ValidationGroup="formBudget" ID="rfvDDLBudgetCategories" runat="server" ControlToValidate="ddlBudgetCategories" ErrorMessage="Please select category"></asp:RequiredFieldValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="trfbudgetdepartment">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader"><%= departmentLabel %><b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <%--<ugit:LookUpValueBox  ID="ctDepartment_Budget" runat="server" FieldName="DepartmentLookup" IsMulti="false" />--%>
                                                    <ugit:LookupValueBoxEdit ID="ctDepartment_Budget" runat="server" FieldName="DepartmentLookup" IsMulti="false" JsCallbackEvent="window.itgBudget.changeDepartment" />
                                                    <%--<UGIT:Department id="ctDepartment_Budget" CallBackJSEvent="window.itgBudget.changeDepartment" MandatoryCheck="true" ShowGLCode="true" ValidationGroup="formBudget" runat="server"></UGIT:Department>--%>
                                                </td>
                                            </tr>
                                            <tr class="trfglcode">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">GL Code
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:Label ID="lbBudgetGLCode" CssClass="lbbudgetglcode" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="trfbudgetitem">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Budget Item<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:TextBox ID="txtBudgetItemVal" ValidationGroup="formBudget" runat="server" CssClass="fullwidth"></asp:TextBox>
                                                    <span style="float: left; width: 100%;">
                                                        <asp:RequiredFieldValidator Display="Dynamic" CssClass="error" ValidationGroup="formBudget" ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBudgetItemVal" ErrorMessage="Please enter title"></asp:RequiredFieldValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="trfbudgetamount">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Amount<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:TextBox ID="txtBudgetAmountf" runat="server"></asp:TextBox>
                                                    <span style="float: left; width: 100%;">
                                                        <asp:RequiredFieldValidator Display="Dynamic" CssClass="error" ValidationGroup="formBudget" ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBudgetAmountf" ErrorMessage="Please enter amount"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator CssClass="error" Display="Dynamic" ValidationGroup="formBudget" ID="RequiredFieldValidator4" runat="server"
                                                            ValidationExpression="^-?[0-9]\d*(\.\d+)?$" ControlToValidate="txtBudgetAmountf" ErrorMessage="Please use format '12345.99'"></asp:RegularExpressionValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="trfbudgetstartdate">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Start Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxDateEdit ID="dtcBudgetStartDate" runat="server" ClientInstanceName="dtcBudgetStartDate" ></dx:ASPxDateEdit>
                                                    <%--<SharePoint:DateTimeControl CssClassTextBox="datetimectr datetimectr111" ID="dtcBudgetStartDate"
                                                        runat="server" DateOnly="true" />--%>
                                                </td>
                                            </tr>
                                            <tr class="trfbudgetenddate">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">End Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxDateEdit ID="dtcBudgetEndDate" runat="server" ClientInstanceName="dtcBudgetEndDate"></dx:ASPxDateEdit>
                                                    <%--<SharePoint:DateTimeControl CssClassTextBox="datetimectr datetimectr111" ID="dtcBudgetEndDate"
                                                        runat="server" DateOnly="true" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right; padding: 5px 5px 5px 0px;">
                                                    <div style="float: right;">
                                                        <asp:Button ValidationGroup="formBudget" CssClass="input-button-bg" ID="btBudgetSave" runat="server" Text="Save" OnClick="BtBudgetSave_Click" />
                                                        <input type="button" value="Cancel" class="input-button-bg" onclick="window.itgBudget.closePopup(true);" />
                                                    </div>
                                                </td>

                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>


                <%-- Project Actuals Popup--%>
                <dx:ASPxPopupControl ShowPageScrollbarWhenModal="false" AllowDragging="true" AutoUpdatePosition="true" 
                    MaxWidth="350px" MinWidth="150px" MaxHeight="250px" MinHeight="150px" ID="budgetActualPopupBox" ClientInstanceName="budgetActualPopupBox"
                    ShowFooter="false" ShowHeader="true" HeaderText="New Budget Item"
                    runat="server" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <div id="budgetActualPopup">
                                <div>
                                    <div style="float: left; width: 100%;">
                                        <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse"
                                            width="350px">
                                            <tr class="trfactualcategory">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Category
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:DropDownList ID="ddlActualCategory" Width="200" ValidationGroup="formABudget" runat="server"></asp:DropDownList>
                                                    <%--<span style="float: left; width: 100%;">
                                                        <asp:RequiredFieldValidator Display="Dynamic" CssClass="error" ValidationGroup="formABudget" ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlActualCategory" ErrorMessage="Please select category"></asp:RequiredFieldValidator>
                                                    </span>--%>
                                                </td>
                                            </tr>
                                            <tr class="trfactualdepartment">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader"><%= departmentLabel %>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <%--<ugit:LookUpValueBox  ID="ctDepartment_Actual" runat="server" FieldName="DepartmentLookup" IsMulti="false"  />--%>
                                                    <ugit:LookupValueBoxEdit ID="ctDepartment_Actual" runat="server" FieldName="DepartmentLookup" IsMulti="false" JsCallbackEvent="window.itgBudgetActual.changeDepartment" />
                                                    <%--<UGIT:Department id="ctDepartment_Actual" MandatoryCheck="false" ValidationGroup="formABudget" ShowGLCode="true" runat="server" CallBackJSEvent="window.itgBudgetActual.changeDepartment"></UGIT:Department>--%>
                                                </td>
                                            </tr>
                                            <tr class="trfactualglcode">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">GL Code
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:Label ID="lbActualGlcode" CssClass="lbactualglcode" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="trfactualbudget">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Budget Item<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <div class="wrapper-demo">
                                                        <div class="ugit-dropdown" id="ugit-actualbudget-dropdown">
                                                            <asp:HiddenField ID="hfEditBudgetActualID" runat="server" />
                                                            <label>--Select--</label>
                                                            <asp:BulletedList CssClass="dropdown" ID="ddlActualBudget" runat="server">
                                                            </asp:BulletedList>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="trfactualvender">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Vendor
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:DropDownList ID="ddlActualVender" CssClass="fullwidth" runat="server"
                                                        OnLoad="DDLVendorActual_Load">
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr class="trfactualinvoice">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">PO/Invoice #
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:TextBox ID="txtActualInvoice" CssClass="fullwidth" ValidationGroup="formABudget" runat="server" MaxLength="200"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="trfactualdescription">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Description<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:HiddenField ID="hfEditActualID" runat="server" />
                                                    <asp:TextBox ID="txtActualTitle" CssClass="fullwidth" ValidationGroup="formABudget" runat="server" MaxLength="240"></asp:TextBox>
                                                    <span style="float: left; width: 100%;">
                                                        <asp:RequiredFieldValidator Display="Dynamic" CssClass="error" ValidationGroup="formABudget" ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtActualTitle" ErrorMessage="Please enter description"></asp:RequiredFieldValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="trfactualamount">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Amount<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <asp:TextBox ID="txtActualAmount" ValidationGroup="formABudget" runat="server" MaxLength="100"></asp:TextBox>
                                                    <span style="float: left; width: 100%;">
                                                        <asp:RequiredFieldValidator CssClass="error" Display="Dynamic" ValidationGroup="formABudget" ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtActualAmount" ErrorMessage="Please enter amount"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator Display="Dynamic" CssClass="error" ValidationGroup="formABudget" ID="RegularExpressionValidator1" runat="server"
                                                            ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$" ControlToValidate="txtActualAmount" ErrorMessage="Please use format '12345.99'"></asp:RegularExpressionValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr class="trfactualstartdate">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">Start Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxDateEdit ID="dtcActualStartDate" ClientInstanceName="dtcActualStartDate" runat="server"></dx:ASPxDateEdit>
                                                    <%--<SharePoint:DateTimeControl CssClassTextBox="datetimectr datetimectr111" ID="dtcActualStartDate"
                                                        runat="server" DateOnly="true" />--%>
                                                </td>
                                            </tr>
                                            <tr class="trfactualenddate">
                                                <td class="ms-formlabel">
                                                    <h3 class="ms-standardheader">End Date<b style="color: Red;">*</b>
                                                    </h3>
                                                </td>
                                                <td class="ms-formbody">
                                                    <dx:ASPxDateEdit ID="dtcActualEndDate" runat="server" ClientInstanceName="dtcActualEndDate" ></dx:ASPxDateEdit>
                                                    <%--<SharePoint:DateTimeControl CssClassTextBox="datetimectr datetimectr111" ID="dtcActualEndDate"
                                                        runat="server" DateOnly="true" />--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right; padding: 5px 5px 5px 0px;">
                                                    <div style="float: right;">
                                                        <asp:Button ValidationGroup="formABudget" CssClass="input-button-bg" ID="btBudgetActualSave" CommandName="addActualsaveclick" runat="server" Text="Save" OnClick="BtBudgetActualSave_Click" />
                                                        <input type="button" value="Cancel" class="input-button-bg" onclick="window.itgBudgetActual.closePopup(true);" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>

            </td>
        </tr>
    </table>


    <table id="printTable" style="display: none;">
        <tr>
            <td id="printTD"></td>
        </tr>
    </table>
</asp:Panel>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        try {
            clickUpdateSize()
        }
        catch (ex) {
        }
    });
</script>


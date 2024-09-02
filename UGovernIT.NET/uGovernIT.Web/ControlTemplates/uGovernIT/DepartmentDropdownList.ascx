<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentDropdownList.ascx.cs" Inherits="uGovernIT.Web.DepartmentDropdownList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
        display: none;
    }

    .overrideFontSize {
        font-size: 11px !important;
    }

    label {
        margin-left: 0;
    }

    .dxlpLoadingPanel_UGITNavyBlueDevEx td.dx, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx td.dx {
        white-space: nowrap;
        text-align: center;
        padding: 0px 10px;
    }

    .dxlpLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxcaLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx .dxlp-loadingImage, .dxeImage_UGITNavyBlueDevEx.dxe-loadingImage {
        background-image: url(/Content/Images/ajaxloader.gif);
        height: 32px;
        width: 32px;
    }

    .dxlpLoadingPanel_UGITNavyBlueDevEx, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx {
        font: 11px roboto, Geneva, sans-serif;
        color: #201f35;
        background-color: White;
        border: none !important;
    }

    .dxbButtonSys.dxbTSys {
        -webkit-box-sizing: border-box;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
        display: inline-table;
        border-spacing: 0;
        border-collapse: separate;
        margin-top: 17px;
    }

    body{
        font-family: 'Roboto', sans-serif !important;
    }

    .dxeDropDownWindow_UGITNavyBlueDevEx{
        font:11px Roboto, sans-serif !important;
    }
</style>




<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    if (typeof (departmentdropdownlist) == 'undefined')
        var departmentdropdownlist = [];

    departmentdropdownlist.push({ clienid: "<%=this.ClientID%>", dropdownid:<%= this.DropDownEdit.ClientID%>})

    function getDepartmentDropDownList_Ctrl(globalName, dxCtrlID) {
        var item = _.find(departmentdropdownlist, function (s) {
            return globalName.indexOf(s.clienid) != -1;
        });

        if (!item)
            return null;

        return window[item.clienid + "_" + dxCtrlID];
    }

    function getDepartmentDropDownList_dropdown(globalName) {
        var cliendID = _.find(departmentdropdownlist, function (s) {
            return globalName.indexOf(s.clienid) != -1;
        });

        if (!cliendID)
            return null;

        var item = _.findWhere(departmentdropdownlist, { clienid: cliendID.clienid });
        return window[item.dropdownid.id];
    }

    try {
        var controlLength = departmentControlConfig.length;
    }
    catch (ex) {
        departmentControlConfig = {}
    }

    departmentControlConfig["<%= this.ClientID%>"] = { "CallBackJSEvent":"<%= CallBackJSEvent%>", dropdownid:"<%= this.DropDownEdit.ClientID%>" };

    $(function () {
        $(".departmentedit").dialog({
            autoOpen: false,
            width: 300,
            height: 300
        });
        //var dementmentviewtd = $("#" + ccID + "_dementmentViewTD");
        //if (dementmentviewtd)
        //    dementmentviewtd.tooltip();
        $(".dementmentviewtd").tooltip();
    });

    //$(function () {
    //    $('#ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_ddlDepartment_BoxEditCallBack_customdropdownedit_DDD_DDTC_department_cmbDepartment_TL').hide();
    //});


    function getPreSelectedDpt(ccID) {
        var jqDepartments = $("#" + ccID + "_pSelectedDepartments");
        return jqDepartments;
    }

    function addDepartment(ccID) {
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments");
        var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1");
        var hdnSetParams = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnSetParams");
        var ismulti = hdnDepartments.Get("ismulti");
        var division = hdnDivision.Get("Division");

        var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
        var cmbCompany = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbCompany")
        var cmbDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDivision")
        var selectedDepartments = cmbDepartment.GetSelectedItems();
        var ddEditControl = getDepartmentDropDownList_dropdown(cmbDivision.name);
        if (hdnSetParams.Get('AddAllDeptsOfDivn') == 'YES') {
            cmbDivision.SetValue(hdnDivision.Get('Division'));
            cmbDivision.SetEnabled(false);
            if (selectedDepartments.length > 1)
                ddEditControl.SetText("<Various>");
        }
        for (var i = 0; i < selectedDepartments.length; i++) {
            var selectedDept = selectedDepartments[i];

            var departmentID = "";
            var department = "";
            var departmentGLCode = "";
            if (selectedDepartments.length > 0) {
                departmentID = selectedDept.value;
                department = selectedDept.texts[0];
                if (selectedDept.texts.length > 1) {
                    department = selectedDept.texts[1];
                    departmentGLCode = selectedDept.texts[0];
                }
            }
            else {
                return;
            }

            if (departmentID == "")
                return;


            var company = "";
            var companyGLCode = "";
            try {
                if (cmbCompany) {
                    var selCompany = cmbCompany.GetSelectedItem();
                    if (selCompany != null) {
                        company = selCompany.texts[0];
                        if (selCompany.texts.length > 1) {
                            company = selCompany.texts[1];
                            companyGLCode = selCompany.texts[0];
                        }
                    }
                }
            } catch (ex) { }
            var division = "";
            var divisionGLCode = "";
            //var divisionText = ""
            try {
                if (cmbDivision) {
                    var selDivision = cmbDivision.GetSelectedItem() ? cmbDivision.GetSelectedItem() : hdnDivision.Get('Division');
                    if (selDivision != null) {
                        if (ismulti) {
                            division = selDivision.texts[0];
                            if (selDivision.texts.length > 1) {
                                division = selDivision.texts[1];
                                divisionGLCode = selDivision.texts[0];
                            }
                        }
                        else {
                            division = selDivision.value;
                            if (selDivision.texts.length > 1) {
                                divisionGLCode = selDivision.texts[0];
                            }
                        }
                    }
                }

            } catch (ex) { }

            var fullGLCode = "";
            if (companyGLCode && companyGLCode != "") {
                fullGLCode = companyGLCode;
            }
            if (divisionGLCode && divisionGLCode != "") {
                if (fullGLCode != "")
                    fullGLCode += "-";
                fullGLCode += divisionGLCode;
            }
            if (departmentGLCode && departmentGLCode != "") {
                if (fullGLCode != "")
                    fullGLCode += "-";
                fullGLCode += departmentGLCode;
            }

            var jqDepartments = getPreSelectedDpt(ccID);

            var dptExist = jqDepartments.find("span[id='" + departmentID + "']");
            if (dptExist.length == 0) {
                var cDpt = new Array();
                cDpt.push("<span id='" + departmentID + "' code='" + fullGLCode + "'>");
                cDpt.push("<i class='name' code='" + departmentGLCode + "'>" + department + "</i>");
                cDpt.push("<i class='company' code='" + companyGLCode + "'>" + company + "</i>");
                cDpt.push("<i class='division' code='" + divisionGLCode + "'>" + division + "</i>");
                cDpt.push("</span>");
                jqDepartments.append(cDpt.join(" "));
            }
        }

        setMultiEditViewSelectedDepartment(ccID);
        try {
            //cmbJobTitle.PerformCallback(selectedDepartments[0].value);
            if (!ismulti) {
                if (departmentID != '') {
                    cmbJobTitle.PerformCallback(departmentID);
                }
            }
        } catch (e) {
            console.log(e);
        }

        if (!ismulti) {
            debugger;
            var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
            var division = hdnDivision.Get("Division");
            var divisionText = '';
            if (division != '') {
                divisionText = hdnDivision.Get("DivisionText");
            }
            var selectedDepartments = cmbDepartment.GetSelectedItems();
            if (selectedDepartments.length > 0) {
                var ddEditControl = getDepartmentDropDownList_dropdown(ccID);
                if (divisionText != null) {
                    ddEditControl.SetText(divisionText + ' > ' + selectedDepartments[0].text);
                    ddEditControl.SetKeyValue(selectedDepartments[0].value);
                    ddEditControl.HideDropDown();
                }
                else {
                    ddEditControl.SetText(selectedDepartments[0].text);
                    ddEditControl.SetKeyValue(selectedDepartments[0].value);
                    ddEditControl.HideDropDown();
                }
            }

        }
    }

    function setMultiEditViewSelectedDepartment(ccID) {
        var jqDepartments = getPreSelectedDpt(ccID);
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
        var ismulti = hdnDepartments.Get("ismulti");
        var showDepartmentDetail = hdnDepartments.Get("showDepartmentDetail");

        var multiViewDptsDiv = $("#" + ccID + "_multipleDepartmentViewDiv");
        var dementmentviewtd = $("#" + ccID + "_dementmentViewTD");
        var sperator = "";
        if (ismulti)
            sperator = ";";


        multiViewDptsDiv.html("");
        dementmentviewtd.html("");
        var departments = jqDepartments.find("span");



        departments.each(function (i, abct) {
            var aa = abct;
            var toolTip = getDepartmentToolTipData(ccID, aa);
            var enabledivision = hdnDepartments.Get("enabledivision");
            if (enabledivision && $(aa).find(".division").text().length > 0) {
                var element = "<span title='" + toolTip + "' id='" + $(aa).prop("id") + "'>" + $(aa).find(".division").text() + " > " + $(aa).find(".name").text() + "</span>";
                multiViewDptsDiv.append(element);
            }
            else {
                var element = "<span title='" + toolTip + "' id='" + $(aa).prop("id") + "'>" + $(aa).find(".name").text() + "</span>";
                multiViewDptsDiv.append(element);
            }

            var element1 = "";
            if (departments.length == 1 || showDepartmentDetail) {
                element1 = "<span title='" + toolTip + "'>" + toolTip.replace(/\s/g, "&nbsp;") + sperator + "</span>";
            }
            else {
                element1 = "<span title='" + toolTip + "'>" + $(aa).find(".name").text() + sperator + "</span>";
            }
            // dementmentviewtd.append(element1);
            if (!ismulti) {
                clnt = "<%= this.Parent.ClientID%>";
                var ddEditControl = ASPxClientControl.GetControlCollection().GetByName(clnt.replace("_DDD_DDTC", "")); //getDepartmentDropDownList_dropdown(clnt.replace("_DDD_DDTC", "")); s.globalName
                ddEditControl.SetText($(element).attr("title"));
                ddEditControl.SetKeyValue($(element).attr("id"));
                ddEditControl.HideDropDown();
                //return;
            }
        });

        dementmentviewtd.tooltip();
        setMultiSelectedValue(ccID);
    }

    function getDepartmentToolTipData(ccID, item) {
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
        var tooltip = "";
        var enabledivision = hdnDepartments.Get("enabledivision");
        var showDepartmentDetail = hdnDepartments.Get("showDepartmentDetail");
        var companyRequired = hdnDepartments.Get("companyRequired");

        // Add company
        var company = $(item).find(".company").text();
        if (companyRequired) {
            tooltip += company;
            if (hdnDepartments.Get('ismulti') == false) {
                company12.SetValue(company);

            }
        }
        // Add division
        var division = $(item).find(".division").text();
        //var division = cmbDivision.GetValue().toString();
        if (enabledivision && division != "N/A") {
            if (division != '' && division != company) {
                if (tooltip != "")
                    tooltip += " > ";

                tooltip += division;
                if (hdnDepartments.Get('ismulti') == false) {
                    division12.PerformCallback(company + "|" + division + "|" + $(item).find(".name").text());
                }
            }
            else if (division == company) {
                if (hdnDepartments.Get('ismulti') == false) {
                    division12.PerformCallback(company + "|" + company + "|" + $(item).find(".name").text());
                }
            }
        }
        else {
            if (hdnDepartments.Get('ismulti') == false) {
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment");
                if (cmbDepartment !== null) {
                    cmbDepartment.PerformCallback($(item).find(".name").text());
                }
            }
        }

        // Add department
        if (tooltip != "")
            tooltip += " > ";
        tooltip += $(item).find(".name").text();
        tooltip = tooltip.replace(/'/g, "\'").replace(/\"/g, "\\\"");
        return tooltip;
    }

    function callBeforeDepartmentPopupOpen(ccID, s) {
        setMultiEditViewSelectedDepartment(ccID);
    }

    function setMultiSelectedValue(ccID) {
        var jqDepartments = getPreSelectedDpt(ccID);
        var selectedDpts = new Array();
        jqDepartments.find("span").each(function (i, item) {
            var jqItem = $(item);
            selectedDpts.push(jqItem.attr("id") + ";#" + jqItem.find(".name").text() + ";#" + jqItem.find(".division").text());
        });
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
        hdnDepartments.Set("selectedDepartments", selectedDpts.join(";#"));
        $("#" + ccID + "_txtDepartmentCtr").val(selectedDpts.join(";#"));
    }


    function resetDepartment(s, e, ccID) {
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
        var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1")
        var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
        var cmbDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDivision")
        hdnDivision.Set("Division", "");
        hdnDivision.Set("DivisionText", "");
        var jqDepartments = getPreSelectedDpt(ccID);
        jqDepartments.html("");
        hdnDepartments.Set("selectedDepartments", "");
        var multiViewDptsDiv = $("#" + ccID + "_multipleDepartmentViewDiv");
        var dementmentviewtd = $("#" + ccID + "_dementmentViewTD");
        multiViewDptsDiv.html("");
        dementmentviewtd.html("");
        $("#" + ccID + "_txtDepartmentCtr").val("");
        clnt = "<%= this.Parent.ClientID%>";
        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName(clnt.replace("_DDD_DDTC", ""));
        

        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName(departmentControlConfig[ccID].dropdownid);
        if (ddEditControl) {
            ddEditControl.SetText("All");
            ddEditControl.SetKeyValue("");
            cmbDivision.SetValue("");
            var deptCount = cmbDepartment.itemsValue.length;
            for (var i = 0; i < deptCount; i++) {
                cmbDepartment.RemoveItem(0);
            }
        }
        ddEditControl.HideDropDown();


        if (departmentControlConfig[ccID].CallBackJSEvent != "") {
            $.globalEval(departmentControlConfig[ccID].CallBackJSEvent + "('" + ccID + "_pSelectedDepartments')");

            //$.globalEval(departmentControlConfig[ccID].CallBackJSEvent + "('" + ccID + "_pSelectedDivision')");
        }
    }

    function editDepartmentDone(s, e, ccID) {
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
        var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1")
        var ismulti = hdnDepartments.Get("ismulti");
        if (!ismulti) {

            var jqDepartments = getPreSelectedDpt(ccID);
            jqDepartments.html("");
            addDepartment(ccID);
        }
        else {
            DoneValueMultiple(s, e, ccID);
        }
        // var popup = ASPxClientControl.GetControlCollection().GetByName(ccID + "_pcMain");
        //  popup.Hide();

        if (departmentControlConfig[ccID].CallBackJSEvent != "") {
            $.globalEval(departmentControlConfig[ccID].CallBackJSEvent + "('" + ccID + "_pSelectedDepartments')");

            //$.globalEval(departmentControlConfig[ccID].CallBackJSEvent + "('" + ccID + "_pSelectedDivision')");
        }
        <%if (EnablePostBack)
    {%>
        $("#" + ccID + "_departmentPostBack").click();
        <%}%>

        if (!ismulti) {
            debugger;
            var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
            var division = hdnDivision.Get("Division");
            var divisionText = '';
            if (division != '') {
                divisionText = hdnDivision.Get("DivisionText");
            }
            var selectedDepartments = cmbDepartment.GetSelectedItems();
            if (selectedDepartments.length > 0) {
                var ddEditControl = getDepartmentDropDownList_dropdown(s.globalName);
                if (divisionText != null) {
                    ddEditControl.SetText(divisionText + ' > ' + selectedDepartments[0].text);
                    ddEditControl.SetKeyValue(selectedDepartments[0].value);
                    ddEditControl.HideDropDown();
                }
                else {
                    ddEditControl.SetText(selectedDepartments[0].text);
                    ddEditControl.SetKeyValue(selectedDepartments[0].value);
                    ddEditControl.HideDropDown();
                }
            }

        }
    }

    function removeDepartment_MultiSelect(ccID) {
        var jqDepartments = getPreSelectedDpt(ccID);
        var selectedDpt = $("#" + ccID + "_multipleDepartmentViewDiv span.selected");

        /* Added */
        if (selectedDpt.length == 0) {
            var selectedDeptIDlength = jqDepartments.find("span");
            var selectedDeptID = 0;
            if (selectedDeptIDlength.length > 0) {
                selectedDeptID = selectedDeptIDlength.prop("id");
            }
            var dptExist = jqDepartments.find("span[id='" + selectedDeptID + "']");
            dptExist.remove();
        }
        /* End */

        selectedDpt.each(function (i, item) {
            var selectedDptID = $(item).prop("id")
            var dptExist = jqDepartments.find("span[id='" + selectedDptID + "']");
            dptExist.remove();
        });

        var selectedDeptIDlengthNew = jqDepartments.find("span");
        if (selectedDeptIDlengthNew.length > 0) {
            var removeIdComma = selectedDeptIDlengthNew.prop("id");
            if (removeIdComma.includes(",")) {
                var dptExist = jqDepartments.find("span[id='" + removeIdComma + "']");
                dptExist.remove();
            }
        }

        setMultiEditViewSelectedDepartment(ccID);
    }

    function OnDepartmentOnlyBoxChanged(ccID, dptOnlyCtr) {
        var dpt = "";
        var dptID = "";
        var dptCode = "";
        var selectedItem = dptOnlyCtr.GetSelectedItem();
        if (selectedItem != null) {
            dpt = selectedItem.texts[0];
            dptID = selectedItem.value;
            if (selectedItem.texts.length > 1) {
                dpt = selectedItem.texts[1];
                dptCode = selectedItem.texts[0];
            }
        }

        var jqDepartments = getPreSelectedDpt(ccID);
        jqDepartments.html("");
        var cDpt = new Array();
        cDpt.push("<span id='" + dptID + "' code='" + dptCode + "'>");
        cDpt.push("<i class='name' code='" + dptCode + "'>" + dpt + "</i>");
        cDpt.push("<i class='company' code=''></i>");
        cDpt.push("<i class='division' id='" + divisionText + "' code=''></i>");
        cDpt.push("</span>");
        jqDepartments.append(cDpt.join(" "));


        $("#" + ccID + "_txtDepartmentCtr").val(dpt);
        if (departmentControlConfig[ccID].CallBackJSEvent != "") {
            $.globalEval(departmentControlConfig[ccID].CallBackJSEvent + "('" + ccID + "_pSelectedDepartments')");
            //$.globalEval(departmentControlConfig[ccID].CallBackJSEvent + "('" + ccID + "_pSelectedDivision')");
        }
    }

    function openDepartPopupToEdit(ccID, obj) {
        var popup = ASPxClientControl.GetControlCollection().GetByName(ccID + "_pcMain");
        popup.ShowAtElement(obj);
    }

    function openDepartPopupToClose(ccID, obj) {
        clnt = "<%= this.Parent.ClientID%>";
        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName(clnt.replace("_DDD_DDTC", ""));
        ddEditControl.HideDropDown();
    }

    function setUGITDepartment(ccID, departmentID) {
        var ASPxCallback1 = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxCallback1")
        if (typeof ASPxCallback1 !== "undefined" && ASPxCallback1 != null)
            ASPxCallback1.PerformCallback(departmentID);
    }



    function onDepartmentCallbackComplete(ccID, s, e) {
        var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
        if (hdnDepartments != null) {
            hdnDepartments.Set("selectedDepartments", "");
            var dementmentviewtd = $("#" + ccID + "_dementmentViewTD");
            var multiViewDptsDiv = $("#" + ccID + "_multipleDepartmentViewDiv");
            multiViewDptsDiv.html("");
            dementmentviewtd.html("");
            var jqDepartment = getPreSelectedDpt(ccID);
            jqDepartment.html(e.result);
            setMultiEditViewSelectedDepartment(ccID);
        }
        else {
            var jqDepartment = getPreSelectedDpt(ccID);
            jqDepartment.html(e.result);
            var departmentOnly = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepamentOnly")
            var dID = jqDepartment.find("span").attr("id");
            departmentOnly.SetValue(dID);
            $("#" + ccID + "_txtDepartmentCtr").val(dID);
        }
    }


    function showdepartmenteditbuttonOnhover(obj) {

        $(obj).find('.editicon').show();
    }
    function hidedepartmenteditbuttonOnhover(obj) {

        $(obj).find('.editicon').hide();
    }
    function showDepartmentEditForm(ccID, obj) {

        $("#" + ccID + "_pDepartmentView").hide();
        $("#" + ccID + "_pDepartmentOnly").show("slow");

    }


    var clientControlID = "";
    function onGridSelectionChanged_department(s, e, clientID) {

        clientControlID = clientID;
        var parentCtrl = ASPxClientControl.GetControlCollection().GetByName(clientControlID.replace("_DDD_DDTC", ""));
        gdDepartmentOnly.GetSelectedFieldValues("ID;Title", onGridSelectionChanged_department_Callback);
        loadingPanelDepartment.ShowInElementByID(s.GetMainElement().id);

    }

    function onGridSelectionChanged_department_Callback(h) {

        if (h.length == 0)
            return false;
        var ddEditControl = ASPxClientControl.GetControlCollection().GetByName(clientControlID.replace("_DDD_DDTC", ""));
        reqTypeID = h[0][0];
        reqTypeText = h[0][1];

        var ismulti = "<%= IsMulti%>";
        if (ismulti == "True") {
            var lstid = [];
            var lstname = [];
            for (var i = 0; i < h.length; i++) {
                depTypeID = h[i][0];
                lstid.push(depTypeID);
            }
            for (var j = 0; j < h.length; j++) {
                depTypeText = h[j][1];
                lstname.push(depTypeText);
            }
            ddEditControl.SetKeyValue(lstid.join(","));
            ddEditControl.SetText(lstname.join(", "));
            ddEditControl.HideDropDown();
        }
        ddEditControl.SetKeyValue(reqTypeID);
        ddEditControl.SetText(reqTypeText)
        loadingPanelDepartment.Hide();
        ddEditControl.HideDropDown();
        if (ismulti == "False") {
            $("#cmbDepamentOnly").empty();
            //for (var i = 0; i < response.length; i++) {
            //    var item = response[i].split("-|-");
            var Option = "<option value='" + reqTypeID + "'>" + reqTypeText + "</option>";
            Option.text = reqTypeText;
            Option.value = reqTypeID;
            $("#cmbDepamentOnly").append(Option);
            cmbJobTitle.PerformCallback(reqTypeID);
            /*}*/

        }
    }

</script>
<dx:ASPxLoadingPanel ID="loadingPanelDepartment" ClientInstanceName="loadingPanelDepartment" runat="server" Modal="true" Text="Please Wait..."></dx:ASPxLoadingPanel>

<dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="Callback1"
    OnCallback="ASPxCallback1_Callback">
</dx:ASPxCallback>
<asp:Panel ID="pDepartmentView" runat="server" CssClass="fullwidth" Visible="false">
    <dx:ASPxHiddenField ID="hdnDepartments" runat="server" ClientInstanceName="hdnDepartments"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField ID="hdnSetParams" runat="server" ClientInstanceName="hdnSetParams"></dx:ASPxHiddenField>
     <div style="display: none;">
        <asp:Button ID="departmentPostBack" runat="server" />
    </div>
    <div id="dementmentViewTD" runat="server" class="dementmentviewtd fleft">
    </div>
</asp:Panel>
<div id="pSelectedDepartments" runat="server" style="display: none;" class="selecteddepartmentdetail"></div>
<div id="pSelectedDivision" runat="server" style="display: none;" class="selecteddivisiondetail"></div>
<asp:Panel ID="pDepartmentOnly" runat="server" Visible="false">
    <dx:ASPxComboBox runat="server" ID="cmbDepamentOnly" DropDownStyle="DropDown" AutoPostBack="false" IncrementalFilteringMode="StartsWith" Visible="false"></dx:ASPxComboBox>
    <ugit:ASPxGridView runat="server" ID="gdDepartmentOnly" ClientInstanceName="gdDepartmentOnly" Width="100%" KeyFieldName="ID" OnDataBinding="gdDepartmentOnly_DataBinding"
        EnableCallBacks="true">
        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
        <Settings  ShowColumnHeaders="false"  VerticalScrollableHeight="100" VerticalScrollBarMode="Visible" />
        
        <Styles>
            <Row CssClass="lookupValueBox-editRow"></Row>
            <FocusedRow>
            </FocusedRow>
        </Styles>
        <Columns>
            <dx:GridViewCommandColumn Name="CheckWithMulti" ShowSelectCheckbox="true" Visible="false" />
            <dx:GridViewDataTextColumn SortOrder="Ascending" SortIndex="0" FieldName="Title" Caption=""></dx:GridViewDataTextColumn>
        </Columns>
        <SettingsPager Mode="ShowAllRecords"/>

    </ugit:ASPxGridView>
</asp:Panel>
<asp:TextBox ID="txtDepartmentCtr" runat="server" CssClass="hide txtDepartmentctr"></asp:TextBox>
<asp:RequiredFieldValidator ID="txtDepartmentValidator" Display="Dynamic" Style="display: block;" Visible="false" ControlToValidate="txtDepartmentCtr" runat="server" CssClass="errormsg-container" ErrorMessage="Please specify"></asp:RequiredFieldValidator>

<asp:Panel ID="pcMain" CssClass="departmentPopup" runat="server" Width="525px">
    <div id="pDepartmentPopup" runat="server">

        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            // <![CDATA[

            var lastCompany = null;
            var lastDivision = null;
            var clnt = "";
            function OnCompanyChanged(ccID, cmbCompany) {
                var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                var cmbDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDivision")

                lastCompany = cmbCompany.GetValue().toString();
                var cmbCtr = cmbDepartment;
                var enabledivision = hdnDepartments.Get("enabledivision");
                if (enabledivision)
                    cmbCtr = cmbDivision;

                if (!cmbCtr.InCallback()) {
                    cmbCtr.PerformCallback(cmbCompany.GetValue().toString());
                }
            }



            function OnDivisionChanged(ccID, cmbDivision, divisionID = -1) {
                debugger;
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                var cmbCompany = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbCompany")

                if (cmbDivision == '' && divisionID > -1)
                {
                    cmbDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                    cmbDivision.SetValue(divisionID);
                    var hdnSetParams = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnSetParams")
                    hdnSetParams.Set("AddAllDeptsOfDivn", 'YES');
                }
                var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1")
                hdnDivision.Set("DivisionChanged", "true");
                if (cmbDivision.GetValue() == null) {
                    hdnDivision.Set("Division", divisionID);
                    hdnDivision.Set("DivisionText", '');
                    cmbDepartment.PerformCallback(divisionID);
                    return;
                }
                else {
                    hdnDivision.Set("Division", cmbDivision.GetValue().toString());
                    hdnDivision.Set("DivisionText", cmbDivision.GetText());
                }
                if (cmbDepartment.InCallback()) {
                    lastDivision = cmbDivision.GetValue().toString();
                }
                else {
                    if (cmbCompany != null)
                        cmbDepartment.PerformCallback(cmbDivision.GetValue().toString() + "|" + cmbCompany.GetValue().toString());
                    else
                        cmbDepartment.PerformCallback(cmbDivision.GetValue().toString());
                }
            }

            $(function () {
                $("#<%= this.ClientID%>_multipleDepartmentViewDiv").click(function (e) {
                    if (e.target) {
                        $("#<%= this.ClientID%>_multipleDepartmentViewDiv span").removeClass("selected");
                        $(e.target).addClass("selected");
                    }
                });

                $("#<%= this.ClientID%>_multipleDepartmentViewDiv").dblclick(function (e) {
                    if (e.target) {
                        removeDepartment_MultiSelect("<%= this.ClientID%>");
                    }
                });
            });

            function OnDepartmentEndCallback(ccID, s, e) {
                
                var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1")
                hdnDivision.Set("DivisionChanged", "false");

                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
                if (hdnDepartments.Get('ismulti') == false) {
                    if (typeof (s.cpDepartmentSelectedIndex) != 'undefined') {
                        if (s.cpDivisionSelectedIndex != -1) {
                            cmbDepartment.SelectIndex(s.cpDepartmentSelectedIndex);

                        }
                    }
                }
                var hdnSetParams = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnSetParams")
                if (hdnSetParams.Get('AddAllDeptsOfDivn') == 'YES') {
                    addAllDepartment(ccID);
                }
            }

            function OnDivisionEndCallback(ccID, s, e) {
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                var cmbDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDivision")
                var hdnDepartments = ASPxClientControl.GetControlCollection().GetByName(ccID + "_hdnDepartments")
                if (hdnDepartments.Get('ismulti') == false) {
                    if (typeof (s.cpDivisionSelectedIndex) != 'undefined') {
                        if (s.cpDivisionSelectedIndex != -1) {
                            cmbDivision.SelectIndex(s.cpDivisionSelectedIndex);
                            var divVal = cmbDivision.GetValue();
                            if (!cmbDepartment.InCallback()) {
                                if (typeof (s.cpDepartment) != 'undefined') {
                                    cmbDepartment.PerformCallback(divVal + "|" + s.cpDepartment);
                                }
                            }
                        }
                    }
                }
                if (lastCompany) {
                    cmbDivision.SelectIndex(0);
                    //lastCompany = null;

                    var divVal = cmbDivision.GetValue();
                    if (!cmbDepartment.InCallback())
                        cmbDepartment.PerformCallback(divVal + "|" + lastCompany);
                }
                //hideLoader();
            }

            function addAllDepartment(ccID) {
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment");
                cmbDepartment.SelectAll();
                cmbDepartment.GetSelectedValues();
                addDepartment(ccID);
                cmbDepartment.UnselectAll();
            }

            function removeAllDepartment(ccID) {
                var selectedDiv = ccID + "_multipleDepartmentViewDiv";
                $("#" + selectedDiv + " span").addClass("selected")
                removeDepartment_MultiSelect(ccID);
            }
            function DoneValueMultiple(s, e, ccID) {
                var jqDepartments = getPreSelectedDpt(ccID);
                var selectedDpt = $("#" + ccID + "_multipleDepartmentViewDiv").get(0).getElementsByTagName('span');

                for (selectDepartmentssss of selectedDpt) {
                    if (selectDepartmentssss.innerHTML == '') {
                        selectDepartmentssss.remove();
                    }
                }

                var selectedDiv = $("#" + ccID + "_multipleDepartmentViewDiv").get(0).getElementsByTagName('span');
                var cmbDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDivision");
                var divValue = '';
                if (cmbDivision != null || cmbDivision != undefined)
                    divValue = cmbDivision.GetValue().toString();
                var listOfDivision = [];
                var listOfid = [];
                var listOftext = [];
                for (var i = 0; i < selectedDpt.length; i++) {
                    var span = selectedDpt[i];
                    listOfid.push($(span).attr("id"));
                    //listOftext.push($($(span).attr("title").split('>')).last().get(0).trim());
                    listOftext.push($(span).attr("title"));
                    if (divValue != '') {
                        listOfDivision.push($($(span).attr("title").split('>')).first().get(0).trim());
                    }
                }
                var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1")
                if (listOfDivision.length > 0) {
                    hdnDivision.Set("Division", listOfDivision);
                }
                /* Added */
                //if (divValue != '') {
                //    hdnDivision.Set("Division", divValue);
                //}
                /* End */
                /*ctl00_ctl00_MainContent_ContentPlaceHolderContainer_ctl00_ddlDepartment_BoxEditCallBack_customdropdownedit_DDD_DDTC_department_cmbDivision_I*/
                clnt = "<%= this.Parent.ClientID%>";
                var ddEditControl = getDepartmentDropDownList_dropdown(s.globalName);

                /* Added  */
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                var selectedDepartments = cmbDepartment.GetSelectedItems();
                if (selectedDepartments.length == 0) {
                    if (listOfid.length > 0) {
                        var cDpt = new Array();
                        cDpt.push("<span id='" + listOfid.join(", ") + "'>");
                        cDpt.push("</span>");
                        jqDepartments.append(cDpt.join(" "));
                    }
                }
                //var departments = jqDepartments.find("span");
                //var countOfDept = departments.length;
                //addDepartment(ccID);
                //if (listOftext.length > 1) {
                //    if (listOftext[0] == '') {
                //        selectedDpt.slice(0, 1);
                //        var dptExist = jqDepartments.find("span[id='" + listOfid.join(", ") + "']");
                //        if (dptExist.length == 0) {
                //            var cDpt = new Array();
                //            cDpt.push("<span id='" + listOfid.join(", ") + "'>");
                //            cDpt.push("</span>");
                //            jqDepartments.append(cDpt.join(" "));
                //        }
                //    }
                //}
                /* End */

                if (selectedDpt.length > 1)
                    ddEditControl.SetText("<Various>");
                else
                    if (listOftext.length > 0) {
                        ddEditControl.SetText(listOftext.join(","));
                    }
                    else {
                        ddEditControl.SetText("All");
                    }
                    

                ddEditControl.SetKeyValue(listOfid.join(", "));
                ddEditControl.HideDropDown();

            }

            function SelectDivisionDepartments() {
                var hdnDivision = ASPxClientControl.GetControlCollection().GetByName(ccID + "_ASPxHiddenField1")
                var cmbDepartment = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbDepartment")
                var cmbCompany = ASPxClientControl.GetControlCollection().GetByName(ccID + "_cmbCompany")

                hdnDivision.Set("DivisionChanged", "true");
                hdnDivision.Set("Division", cmbDivision.GetValue().toString());
                hdnDivision.Set("DivisionText", cmbDivision.GetText());


            }


    // ]]>
        </script>
        <dx:ASPxHiddenField ID="ASPxHiddenField1" runat="server"></dx:ASPxHiddenField>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding mt-3">
            <div class="row">
                <div id="cmpControlDiv" runat="server" class="col-md-5 col-sm-5 col-xs-5 text-left" style="padding: 0px 0px 0px 14px !important">
                    <dx:ASPxLabel runat="server" Text="Company:" ID="lbCompany" AssociatedControlID="cmbCompany" CssClass="userProfile-lable" />
                    <dx:ASPxComboBox ClientInstanceName="company12" runat="server" ID="cmbCompany" DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith"
                        EnableSynchronization="True" CssClass="aspxComboBox-dropdown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                    </dx:ASPxComboBox>
                </div>
                <div id="divisionControlDiv" runat="server" class="col-md-5 col-sm-5 col-xs-5 text-left mb-2">
                    <dx:ASPxLabel runat="server" ID="lbDivision" AssociatedControlID="cmbDivision" CssClass="userProfile-lable" />
                    <dx:ASPxComboBox ClientInstanceName="division12" runat="server" ID="cmbDivision" OnCallback="cmbDivision_Callback" 
                        DropDownStyle="DropDown" IncrementalFilteringMode="StartsWith" EnableSynchronization="true" CssClass="aspxComboBox-dropdown"
                        ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%">
                    </dx:ASPxComboBox>
                </div>
                <div>
                    <asp:Label ID="rightBottomAction" runat="server" CssClass="">
                        <span>
                            <dx:ASPxButton ID="btResetPopup" runat="server" AutoPostBack="false" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                        </span>
                    </asp:Label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-5 col-sm-5 col-xs-5 text-left">
                    <dx:ASPxLabel runat="server" Text="Department:" ID="lbDepartment" AssociatedControlID="cmbDepartment" CssClass="chooseDepartment_lable" />
                    <dx:ASPxListBox ClientInstanceName="department12" runat="server" ID="cmbDepartment" OnCallback="cmbDepartment_Callback" CssClass="chooseDepartment_list"
                        SelectionMode="Single" AutoPostBack="false" Width="100%" IncrementalFilteringMode="StartsWith" EnableSynchronization="True">
                    </dx:ASPxListBox>
                </div>
                <div class="col-md-2 col-sm-2 col-xs-2 pl-0">
                    <div id="multipleDepartmentActionTD" runat="server">
                        <div class="optionBtn_wrap">
                            <div class="optionBtn_addLogo" visible="false">
                                <input type="button" value=">>" id="btAddAllDepartment" runat="server" class="input-button-bg" style="margin-bottom: 2px !important;" />
                                <input type="button" value="Add >" id="btAddDepartment" runat="server" class="input-button-bg" style="margin-bottom: 2px !important;" />
                                <input type="button" id="btRemoveFromList" runat="server" value="< Remove" class="input-button-bg" style="margin-bottom: 2px !important;" />
                                <input type="button" value="<<" id="btRemoveAllDepartment" runat="server" class="input-button-bg" style="margin-bottom: 2px !important;" />
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-5 col-sm-5 col-xs-5 text-left" style="padding: 0px 14px 0px 0px !important">
                    <div id="multipleDepartmentTD" runat="server" class="">
                        <div class="chooseDepartment_lable">
                            Chosen <%= departmentLevel %>s:
                        </div>
                        <div id="multipleDepartmentViewDiv" style="width: 100% !important; height: 105px;" runat="server" class="multipledepartmentViewdiv selected">
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12"">
                    <div id="bottomActionTD" runat="server" style="padding: 10px 0">
                        <div class="nprDropDown_actionBtnWrap" style="float:right;">                           


                            <dx:ASPxButton ID="btCancel" runat="server" Text="Cancel" AutoPostBack="false" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                            <dx:ASPxButton ID="btDonePopup" runat="server" Text="Done" AutoPostBack="false"
                                CssClass-="primary-blueBtn">
                            </dx:ASPxButton>

                        </div>

                    </div>
                </div>
            </div>
        </div>


    </div>


</asp:Panel>


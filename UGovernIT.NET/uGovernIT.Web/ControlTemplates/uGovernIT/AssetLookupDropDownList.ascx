<%--<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>--%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetLookupDropDownList.ascx.cs" Inherits="uGovernIT.Web.AssetLookupDropDownList" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<script type="text/javascript" id="dxss_assetdropdownjs" >
    var expire = new Date();
    expire.setDate(expire.getDate() + (-1));
    var hdnDependentCIN;
    function onAssetDropDownInit(s, e) {
        BindRelatedAssetsOnLoad(s, e);
    }
    function onEndCallBack(s, e) {
        var view = "";
        try {
            view = $.cookie("View");
        }
        catch (ex) {
            view = getCookie("View");
        }

        if (view != null) {
            if (view != undefined && view == "allassets") {
                rdbAssetsFilterCIN.SetSelectedIndex(1);
            }
            else {
                try {
                    $.cookie("View", null, { path: '/' });
                }
                catch (ex) {

                    document.cookie = "View=;expires=" + expire + ";path=/;";
                }
                rdbAssetsFilterCIN.SetSelectedIndex(0);
            }

        }
    }
    function FillRelatedAssetOnReqChange() {
        var requestorCtr = null;
        var peoplePicker = [];
        var peoplepickerValue = null;
        if ($(".RequestorUser").length > 0) {
            requestorCtr=ASPxClientControl.GetControlCollection().GetByName($(".field_requestoruser_edit").attr("id"));
            //In Case of mobile agent
            if (document.cookie.indexOf("mobile=1") != -1) {
                var userBoxObj = $(".field_requestoruser_edit input[type=hidden]");
                if (userBoxObj != null && userBoxObj.length > 0) {
                    var itemm = [];
                    itemm.push(userBoxObj.val());
                    $.each(itemm, function (index, userVal) {
                        peoplePicker.push(userVal);
                    });
                    peoplepickerValue = peoplePicker.join(';');
                    peoplePicker = [];
                }
            }
            else if (typeof (requestorCtr) !== 'undefined' && requestorCtr!=null) {
                var item = requestorCtr.GetValue();;
                if (typeof (item) !== 'undefined' && item!='') {
                    $.each(item.split(','), function (index, userVal) {
                        peoplePicker.push(userVal);
                    });
                    peoplepickerValue = peoplePicker.join(';');
                    peoplePicker = [];
                }

            }
        }

        if ($(".OwnerUser").length > 0) {
            requestorCtr=ASPxClientControl.GetControlCollection().GetByName($(".field_owneruser_edit").attr("id"));
            if (typeof (requestorCtr) !== 'undefined' && requestorCtr!=null) {
                var item = requestorCtr.GetValue();
                if (typeof (item) != undefined && item!='') {
                    $.each(item.split(','), function (index, userVal) {
                        peoplePicker.push(userVal);
                    });
                    peoplepickerValue = peoplePicker.join(';');
                    peoplePicker = [];
                }
            }
        }

        if (typeof (cbAssetList) != 'undefined') {

            var moduleName = relatedAssetDropDown.cpCurrentModuleName;
            // Show only assets owned by Requestor for PRS & TSR, ALL assets for other modules
            if (moduleName == "PRS" || moduleName == "TSR") {
                var userVal = "";
                try {
                    userVal = relatedAssetDropDown.cpDependentFieldValue;
                } catch (ex) { }

                if (peoplepickerValue != null && peoplepickerValue != "") {
                    userVal = escape(peoplepickerValue);
                }
            }
            else
                userVal = 'all';

            var selectedValue = "";
            if (relatedAssetDropDown != null) {
                selectedValue = relatedAssetDropDown.cpSelectedValue;
                if (userVal == undefined && relatedAssetDropDown.cpDependentFieldValue) {
                    userVal = relatedAssetDropDown.cpDependentFieldValue;
                }
            }

            if (userVal != undefined && userVal != null && userVal != "") {

                if (selectedValue != "")
                    userVal = userVal + "~" + selectedValue;
            }
            relatedAssetDropDown.cpDependentFieldValue = userVal;
            cbAssetList.PerformCallback(userVal);
        }
    }
    function BindRelatedAssetsOnLoad(s, e) {
        try {
            $.cookie("View", null, { path: '/' });
        }
        catch (ex) {

            document.cookie = "View=;expires=" + expire + ";path=/;";
        }
        var requestorCtr = null;
        var peoplePicker = [];
        var peoplepickerValue = null;
        if ($(".RequestorUser").length > 0) {
            requestorCtr=ASPxClientControl.GetControlCollection().GetByName($(".field_requestoruser_edit").attr("id"));
           if (typeof (requestorCtr) !== 'undefined' && requestorCtr!=null) {
               var item = requestorCtr.GetValue();
                if (typeof (item) !== 'undefined' && item!='')
                {
                    $.each(item.split(','), function (index, userVal) {
                        peoplePicker.push(userVal);
                    });
                    peoplepickerValue = peoplePicker.join(';');
                    peoplePicker = [];
                }
                
            }
        }

        if ($(".OwnerUser").length > 0) {
            requestorCtr=ASPxClientControl.GetControlCollection().GetByName($(".field_owneruser_edit").attr("id"));
            if (typeof (requestorCtr) !== 'undefined' && requestorCtr!=null) {
                var item = requestorCtr.GetValue();
                if (typeof (item) != undefined && item!='') {
                    $.each(item.split(','), function (index, userVal) {
                        peoplePicker.push(userVal);
                    });
                    peoplepickerValue = peoplePicker.join(';');
                    peoplePicker = [];
                }
            }
        }
        if (typeof (cbAssetList) != 'undefined' && !cbAssetList.InCallback()) {
            var moduleName = relatedAssetDropDown.cpCurrentModuleName;
            // Show only assets owned by Requestor for PRS & TSR, ALL assets for other modules
            if (moduleName == "PRS" || moduleName == "TSR") {
                var userVal = "";
                try {
                    userVal = relatedAssetDropDown.cpDependentFieldValue;
                } catch (ex) { }

                if (peoplepickerValue != null && peoplepickerValue != "") {
                    userVal = escape(peoplepickerValue);
                }
            }
            else
                userVal = 'all';
            var selectedValue = "";
            if (relatedAssetDropDown != null) {
                selectedValue = relatedAssetDropDown.cpSelectedValue;
                if (userVal == undefined && relatedAssetDropDown.cpDependentFieldValue) {
                    userVal = relatedAssetDropDown.cpDependentFieldValue;
                }
            }

            if (userVal != undefined && userVal != null && userVal != "") {

                if (selectedValue != "")
                    userVal = userVal + "~" + selectedValue;
            }

            showAllAssets(s, e);
        }
    }

    function onAssetSelectionChanged(s, e) {
        s.GetSelectedFieldValues("AssetTagNum;TicketId;ID", onSelected);
    }
    function onSelected(selectedValues) {
        var selectedText = "";
        var selectedValue = "";
        if (selectedValues == undefined)
            return;
        for (var i = 0; i < selectedValues.length; i++) {
            if (selectedText != "")
                selectedText += ",";
            if (selectedValue != "")
                selectedValue += ",";
            if (selectedValues[i][0] == null) {
                selectedText += selectedValues[i][1];
                selectedValue += selectedValue[i][2]; //+ ";#" + selectedValues[i][1];
            }

            else {
                selectedText += selectedValues[i][0];
                selectedValue += selectedValues[i][2];// + ";#" + selectedValues[i][0];
            }
        }

        if (relatedAssetDropDown.GetText() != selectedText) {
            relatedAssetDropDown.SetTextBase(selectedText);
            relatedAssetDropDown.SetKeyValue(selectedValue)
        }
    }

    function showAllAssets(s, e) {
        
        if (rdbAssetsFilterCIN.GetSelectedIndex() == 1) {
            if (typeof (cbAssetList) != 'undefined' && !cbAssetList.InCallback()) {
                var selectedValue = "";
                if (relatedAssetDropDown != null) {
                    selectedValue = relatedAssetDropDown.cpSelectedValue;
                }
                if (selectedValue != "")
                    selectedValue = "all" + "~" + selectedValue;

                cbAssetList.PerformCallback(selectedValue);
            }
        }
        else
            FillRelatedAssetOnReqChange();
    }
    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
</script>



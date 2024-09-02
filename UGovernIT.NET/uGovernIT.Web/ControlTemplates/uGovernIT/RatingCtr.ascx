<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RatingCtr.ascx.cs" Inherits="uGovernIT.Web.RatingCtr" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .ratingctrcontainer td {
        width: 20px;
        height: 20px;
        border: 1px solid;
        background: #EAEAEA;
    }

    .ratingctrcontainer .ratingopt {
        cursor: pointer;
    }

    .ratingctrcontainer .ratingyellow {
        background: #FFD400;
    }

    .ratingmain-container .selectedoption {
        font-weight: bold;
        padding-left: 5px;
        float: left;
    }

    .setpadding {
        padding-left: 4px !important;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ratingOptionClick(s, e) {
        
        var obj = s.GetMainElement().parentNode;
        var parentContainers = $(obj).parents(".ratingmain-container");
        var parentC = null;
        if (parentContainers.length > 0) {
            parentC = $(parentContainers[0])
        }

        var allOptions = s.titles;
        var isCurrentOptionS = false;
        var selectedRating = s.GetValue();
        var selectedText = s.titles[selectedRating - 1];
        var totalSelectedOption = 0;

        var index = selectedRating;

        var optionTextCtr = parentC.find(".ratingselectedoption .selectedoption");
        var selectedOptionValCtr = parentC.find(".ratingselectedoption .selectedvalue");
        optionTextCtr.html("");
        selectedOptionValCtr.val("0");
        if (index == 0) {
            if (totalSelectedOption > 1) {
                parentC.find(".ratingselectedoption .selectedoption").html("Value: " + selectedText);
                //rRatingCtr.SetValue(selectedRating);
                selectedOptionValCtr.val(selectedRating);
            }
            else if (!isCurrentOptionS) {
                parentC.find(".ratingselectedoption .selectedoption").html("Value: " + selectedText);
               // rRatingCtr.SetValue(selectedRating);
                selectedOptionValCtr.val(selectedRating);
            }
        }
        else {
            for (var i = 0; i <= allOptions.length; i++) {
                if (i == index) {
                    parentC.find(".ratingselectedoption .selectedoption").html("Value: " + selectedText);
                    //rRatingCtr.SetValue(selectedRating);
                    selectedOptionValCtr.val(selectedRating);
                    break;
                }
            }
        }
        
        var select_product_fleft = $(".select_product fleft");
        questionBrachLogic(<%= JSOnMouseOutParam%>);
    }

    function DropdownRatingClick(s, e) {
        var obj = s.GetMainElement().parentNode;
        var parentContainers = $(obj).parents(".ratingmain-container");
        var parentC = null;
        if (parentContainers.length > 0) {
            parentC = $(parentContainers[0])
        }

        var allOptions = new Array();
        for (var i = 0; i < s.GetItemCount(); i++) {
            var value = s.GetItem(i);
            allOptions.push(value);
        }

        var isCurrentOptionS = false;
        var selectedRating = s.GetValue();
        var totalSelectedOption = 0;

        var index = s.GetSelectedIndex();

        var optionTextCtr = parentC.find(".ratingselectedoption .selectedoption");
        var selectedOptionValCtr = parentC.find(".ratingselectedoption .selectedvalue");
        optionTextCtr.html("");
        selectedOptionValCtr.val("0");
        if (index == 0) {
            if (totalSelectedOption > 1) {
                selectedOptionValCtr.val(selectedRating);
            }
            else if (!isCurrentOptionS) {
                selectedOptionValCtr.val(selectedRating);
            }
        }
        else {
            for (var i = 0; i < allOptions.length; i++) {
                if (i == index) {
                    selectedOptionValCtr.val(selectedRating);
                    break;
                }
            }
        }

        selectedOptionValCtr.keyup();
    }

    function RadioRatingClick(s, e) {
        var obj = s.GetMainElement().parentNode;
        var parentContainers = $(obj).parents(".ratingmain-container");
        var parentC = null;
        if (parentContainers.length > 0) {
            parentC = $(parentContainers[0])
        }

        var allOptions = s.items;
        var isCurrentOptionS = false;
        var selectedRating = s.GetValue();
        var totalSelectedOption = 0;

        var index = s.GetSelectedIndex();

        var optionTextCtr = parentC.find(".ratingselectedoption .selectedoption");
        var selectedOptionValCtr = parentC.find(".ratingselectedoption .selectedvalue");
        optionTextCtr.html("");
        selectedOptionValCtr.val("0");
        if (index == 0) {
            if (totalSelectedOption > 1) {
                selectedOptionValCtr.val(selectedRating);
            }
            else if (!isCurrentOptionS) {
                selectedOptionValCtr.val(selectedRating);
            }
        }
        else {
            for (var i = 0; i < allOptions.length; i++) {
                if (i == index) {
                    selectedOptionValCtr.val(selectedRating);
                    break;
                }
            }
        }
        
        selectedOptionValCtr.keyup();
    }

</script>
<div class="ratingmain-container">
    <div style="float: left;">
        <table class="ratingctrcontainer" style="border-collapse: collapse; border: 0px;">
            <tr>
                <dx:ASPxRatingControl ID="rRatingCtr" ClientInstanceName="rRatingCtr" runat="server" AutoPostBack="false">
                    <ClientSideEvents ItemClick="ratingOptionClick" />
                </dx:ASPxRatingControl>
            </tr>
        </table>
        <div id="divCmbRating" runat="server">
            <dx:ASPxComboBox ID="ddlRating" runat="server" ValueField="Key" TextField="Value" DropDownStyle="DropDown">
                <ClientSideEvents SelectedIndexChanged="function(s,e){ DropdownRatingClick(s,e);}" />
            </dx:ASPxComboBox>
        </div>
        <div id="divrdnButton" runat="server">
            <dx:ASPxRadioButtonList ID="rdnRadiobuttonH" ValueField="Key" TextField="Value" runat="server" onclick="RadioRatingClick(this)">
                <ClientSideEvents SelectedIndexChanged="function(s,e){RadioRatingClick(s,e);}" />
            </dx:ASPxRadioButtonList>
        </div>
    </div>
    <div style="float: left;" class="ratingselectedoption">
        <span style="display: none;">
            <asp:TextBox ID="txtRatingVal" CssClass="selectedvalue" runat="server"></asp:TextBox>
        </span>
        <asp:Label ID="lbSelectedOption" runat="server" CssClass="selectedoption"></asp:Label>
        <asp:RequiredFieldValidator ID="rqdrating" InitialValue="0" CssClass="errormsg-container setpadding" runat="server" ControlToValidate="txtRatingVal"
            Display="Dynamic" ErrorMessage="Please specify" Visible="false"></asp:RequiredFieldValidator>
    </div>
</div>

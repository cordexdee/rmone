<%@ Control Language="C#" AutoEventWireup="true" ValidateRequestMode="Disabled" CodeBehind="RequestTypeListEdit.ascx.cs" Inherits="uGovernIT.Web.RequestTypeListEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    aspxUserTokenBox-control {
    height: 0px !important;
}
    .fleft {
        float: left;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .pctcomplete {
        text-align: right;
    }

    .estimatedhours {
        text-align: right;
    }

    .actualhours {
        text-align: right;
    }

    .full-width {
        width: 98%;
    }

    .ms-formlabel {
        text-align: right;
    }

    .existing-section-c {
        float: left;
    }

    .new-section-c {
        float: left;
    }

    .existing-section-a {
        float: left;
        padding: 0px 5px;
    }

        .existing-section-a img {
            cursor: pointer;
        }

    .new-section-a {
        float: left;
        padding-left: 5px;
    }

        .new-section-a img {
            cursor: pointer;
        }

    .ms-standardheader {
        text-align: right;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }

    a:hover {
        text-decoration: underline;
    }

    .editablediv {
        border: 1px solid #bababa;
        margin: 0;
        overflow-y: auto;
        overflow-x: hidden;
        background: #fff;
    }

    a, img {
        border: 0px;
    }

    /*#inputkeyword {
        border: 0 none;
        height: 20px;
        padding: 0;
        float: left;
    }*/

    /*#txtticketsender {
        border: 0 none;
        height: 20px;
        padding: 0;
        float: left;
    }*/
    .keyspan {
        background-color: #E8EDED;
        border: 1px solid #A5A5A5;
        float: left;
        top: 4px;
        margin: 1px;
        padding: 3px;
    }

        .keyspan strong {
            float: left;
            margin-right: 3px;
        }

    .ticketsenderspan {
        background-color: #E8EDED;
        border: 1px solid #A5A5A5;
        float: left;
        top: 4px;
        margin: 1px;
        padding: 3px;
    }

        .ticketsenderspan strong {
            float: left;
            margin-right: 3px;
        }

    .spanimg {
        margin-top: 2px;
        float: left;
    }

        .spanimg:hover {
            background-color: #CED8D9;
        }

    .ticketsenderspanimg {
        margin-top: 2px;
        float: left;
    }

        .ticketsenderspanimg:hover {
            background-color: #CED8D9;
        }

    /*#spaninputkeyword {
        top: 5px;
        float: left;
    }*/

    /*#spanticketsender {
        top: 5px;
        float: left;
    }*/
    /*.showKeywords {
        border: solid 1px #A5A5A5;
        height: 90px;
        width: 300px;
        background-color: #fff;
        overflow-y: auto;
    }*/

    /*.showticketsender {
        border: solid 1px #A5A5A5;
        height: 90px;
        width: 300px;
        background-color: #fff;
        overflow-y: auto;
    }*/
    legend {
        font-size: 14px !important;
        margin-bottom: 1px;
    }

    input[type="checkbox"] {
        margin: -2px 0 0 !important;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function hideddlCategory(action) {

        var category = $("#<%=ddlCategory.ClientID%> option:selected").text();
        $(".ddlCategory").hide();

        $("#<%=hdnCategory.ClientID%>").val('1');
        if (action == 1) {
            $("#<%=hdnRequestCategory.ClientID%>").val("1");
            $("#<%=txtCategory.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestCategory.ClientID%>").val("");
            $("#<%=txtCategory.ClientID%>").val("");
        }
    }
    function hideddlSubCategory(action) {
        var category = $("#<%=ddlSubCategory.ClientID%> option:selected").text();
        $(".ddlSubCategory").hide();
        $("#<%=hdnSubCategory.ClientID%>").val('1');
        if (action == 1) {
            $("#<%=hdnRequestSubCategory.ClientID%>").val("1");
            $("#<%=txtSubCategory.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestSubCategory.ClientID%>").val("");
            $("#<%=txtSubCategory.ClientID%>").val("");
        }
    }
    function hideddlSubCategoryaspercategory(action) {
        var subcategory = $("#<%=ddlsubcategoryaspercategory.ClientID%> option:selected").text();
        $(".ddlsubcategoryaspercategory").hide();
        $("#<%=hdnsubcategoryaspercategory.ClientID%>").val('1');
        if (action == 1) {
            $("#<%=hdnRequestSubcategoryaspercategory.ClientID%>").val("1");
            $("#<%=txtSubcategoryaspercategory.ClientID%>").val(subcategory);
        }
        else {
            $("#<%=hdnRequestSubcategoryaspercategory.ClientID%>").val("");
            $("#<%=txtSubcategoryaspercategory.ClientID%>").val("");
        }
    }
    function hideddlWorkflow() {
        debugger;
       // $("#ctl00_PlaceHolderMain_ctl00_ddlWorkflow").get(0).selectedIndex = 0;
        $(".ddlWorkflow").hide();
        $("#ctl00_PlaceHolderMain_ctl00_hdnWorkflow").val('1');
    }
    function showddlCategory() {
        $(".ddlCategory").show();
        $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('0');
    }
    function showddlSubCategory() {
        $(".ddlSubCategory").show();
        $("#ctl00_PlaceHolderMain_ctl00_hdnCategory").val('0');
    }
    function showddlSubCategoryaspercategory() {
        $(".ddlsubcategoryaspercategory").show();
        $("#ctl00_PlaceHolderMain_ctl00_hdnsubcategoryaspercategory").val('0');
    }
    function showddlWorkflow() {
        debugger;
        $(".ddlWorkflow").show();
        $("#ctl00_PlaceHolderMain_ctl00_hdnWorkflow").val('0');
    }

    function ShowMoreFunction(s, e) {
        $('#showMoreInfo').toggle();
        $('#infoShowHide').text($('#infoShowHide').text() == 'Show Less >>>' ? 'Show More >>>' : 'Show Less >>>');
    }

    $(function () {
        $(".showKeywords").bind("click", function (e) {
            $("#inputkeyword").focus();
        });

        $('#inputkeyword').blur(function () {
            funcAddKeywords();
        });

        $("#inputkeyword").bind("keypress", function (e) {
            //alert('0');
            if (e.keyCode == 13) {
                // Enter pressed... do anything here...
                funcAddKeywords();
            }
        });

        $(".showticketsender").bind("click", function (e) {
            $("#txtticketsender").focus();
        });

        $('#txtticketsender').blur(function () {
            funcAddticketSender();
        });

        $("#txtticketsender").bind("keypress", function (e) {
            //alert('0');
            if (e.keyCode == 13) {
                // Enter pressed... do anything here...
                funcAddKeywords();
            }
        });
    });


    function funcAddticketSender() {
        var spaninput = $("#spanticketsender");
        var newword = $("#txtticketsender").val();
        if (newword == '') {
            return;
        }
        ///validation for duplicate keywords.
        var keywords = $(".hiddenticketsender").val();
        var wordarray = keywords.split(';');
        var exists = false;
        $.each(wordarray, function (i) {

            if (newword == wordarray[i]) {
                $("#txtticketsender").val('');
                alert('sender already exists');
                exists = true;
            }
        });
        if (exists) {
            return;
        }

        $("#txtticketsender").val("");
        var maindiv = $(".showticketsender");
        var spanhtml = "<span class='ticketsenderspan' data=" + newword + "><strong>" + newword + "</strong><img class='ticketsenderspanimg' src='/content/images/cross_icn.png' alt='delete' > </span>";
        ///adding value to hidden field.

        keywords = keywords + newword + ";";
        $(".hiddenticketsender").val(keywords);

        ///adding span before input control.
        spaninput.before(spanhtml);

        $(".showticketsender").delegate("img", "click", function () {
            removeTicketSenderTag(this);
        });
    }
    //Add bubble is KeyWords fields
    function funcAddKeywords() {

        var spaninput = $("#spaninputkeyword");
        var newword = $("#inputkeyword").val();
        if (newword == '') {
            return;
        }
        ///validation for duplicate keywords.
        var keywords = $(".hiddenkeyword").val();
        var wordarray = keywords.split(';');
        var exists = false;
        $.each(wordarray, function (i) {

            if (newword == wordarray[i]) {
                $("#inputkeyword").val('');
                alert('Keyword already exists');
                exists = true;
            }
        });
        if (exists) {
            return;
        }

        $("#inputkeyword").val("");
        var maindiv = $(".showKeywords");
        var spanhtml = "<span class='keyspan' data=" + newword + "><strong>" + newword + "</strong><img class='spanimg' src='/content/images/cross_icn.png' alt='delete' > </span>";
        ///adding value to hidden field.

        keywords = keywords + newword + ";";
        $(".hiddenkeyword").val(keywords);

        ///adding span before input control.
        spaninput.before(spanhtml);

        $(".showKeywords").delegate("img", "click", function () {
            removeTag(this);
        });
    }

    function removeTag(image) {
        ///remove tags value remove from hidden fields.
        var myval = $.trim($($(image).parent()).text()) + ';';
        var hdnfieldvalue = $(".hiddenkeyword").val();
        //alert(myval);

        hdnfieldvalue = hdnfieldvalue.replace(myval, '');
        $(".hiddenkeyword").val(hdnfieldvalue);

        ///remove span tag to reference of img.
        $($(image).parent()).remove();
        $("#inputkeyword").focus();
    }

    function removeTicketSenderTag(image) {
        ///remove tags value remove from hidden fields.
        var myval = $.trim($($(image).parent()).text()) + ';';
        var hdnfieldvalue = $(".hiddenticketsender").val();
        //alert(myval);

        hdnfieldvalue = hdnfieldvalue.replace(myval, '');
        $(".hiddenticketsender").val(hdnfieldvalue);

        ///remove span tag to reference of img.
        $($(image).parent()).remove();
        $("#txtticketsender").focus();
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        //set_cookie('UseManageStateCookies', 'true', null, "<SPContext.Current.Web.ServerRelativeUrl %>");
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);

    }


    function editOwner() {
        $("[id$='hndOwner']").val('');
        $("[id$='dvOwner']").css('display', 'none');
        $("[id$='ppeOwner']").css('display', '')
    }

    function editPRPGroup() {
        $("[id$='hndPRPGroup']").val('');
        $("[id$='dvPRPGroup']").css('display', 'none');
        $("[id$='ppePrpGroup']").css('display', '')
    }
    function editORP() {
        $("[id$='hndORP']").val('');
        $("[id$='dvORP']").css('display', 'none');
        $("[id$='ppeORP']").css('display', '')
    }
    function editExecManeger() {
        $("[id$='hndExecManeger']").val('');
        $("[id$='dvExecManeger']").css('display', 'none');
        $("[id$='ppeExecManeger']").css('display', '')
    }
    function editBackUpMan() {
        $("[id$='hndBackUpMan']").val('');
        $("[id$='dvBackUpMan']").css('display', 'none');
        $("[id$='ppeBackUpMan']").css('display', '')
    }

    function hideShowEdit(mainClass) {
        var jsMain = $("." + mainClass);
        var dropdown = jsMain.find("select");
        var editIcon = jsMain.find(".editicon");
        editIcon.hide();
        if (dropdown.val() != "-1" && dropdown.val() != "0") {
            editIcon.show();
        }
    }
    function editDisableSLA() {
        $("[id$='hdnDisableSLA']").val('');
        $("[id$='dvDisableSLA']").css('display', 'none');
        $("[id$='dvChkDisableSLA']").css('display', '');
    }
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    }); 
</script>


<%--new--%>

<div class="col-md-12 col-sm-12 col-xs-12 popupUp-mainContainer">
    <fieldset>
        <legend>General</legend>
        <div class="ms-formtable accomp-popup">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Module<b style="color: Red;">*</b></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlModule" runat="server" CssClass="aspxDropDownList" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvddlmodule" runat="server" ValidationGroup="RequestTypeGroup" ForeColor="Red"
                            Display="Dynamic" ControlToValidate="ddlModule" InitialValue="0" ErrorMessage="Select Module Name"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="row" id="trCategory" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Category<b style="color: Red;">*</b></h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlSubCategory" id="divddlSubCategory" runat="server" style="float: left; width: 100%;">
                                <div class="col-xs-10 noPadding">
                                    <asp:DropDownList ID="ddlSubCategory" onchange="hideShowEdit('ddlSubCategory')" runat="server" CssClass="aspxDropDownList" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-2 noPadding">
                                    <img alt="Edit Category" runat="server" class="editicon" id="btCategoryEdit"
                                        src="/content/images/editNewIcon.png" width="16" style="cursor: pointer; position: relative; float: right;"
                                        onclick="javascript:$('.divSubCategory').attr('style','display:block');hideddlSubCategory(1)" />
                                    <img alt="Add Category" id="Img1" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right; margin-right: 10px;"
                                        onclick="javascript:$('.divSubCategory').attr('style','display:block');hideddlSubCategory(0);" />
                                </div>

                            </div>
                            <div runat="server" id="divSubCategory" class="divSubCategory" style="display: none; float: left;">
                                <div class="col-xs-10 noPadding">
                                    <asp:TextBox runat="server" ID="txtSubCategory" CssClass="txtCategory"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnRequestSubCategory"></asp:HiddenField>
                                </div>
                                <div class="col-xs-2 noPadding">
                                    <img alt="Cancel Category" style="float: right" width="16" src="/content/images/close-blue.png" class="cancelModule"
                                        onclick="javascript:$('.divSubCategory').attr('style','display:none');showddlSubCategory();" />
                                </div>
                            </div>
                            <div style="width: auto; padding: 4px 4px 0px; display: inline-block;">
                                <asp:CustomValidator ID="csvdivSubCategory" ForeColor="Red" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtSubCategory" ErrorMessage="Select Category" Display="Dynamic" OnServerValidate="csvdivSubCategory_ServerValidate" ValidationGroup="RequestTypeGroup"></asp:CustomValidator>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">
                            <asp:Label ID="lblRequestType" runat="server" Text="Request Type"></asp:Label><b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox runat="server" ID="txtRequestType" CssClass="txtCategory"></asp:TextBox>
                        <asp:LinkButton ID="lblRequestTypeLink" runat="server" Visible="false"></asp:LinkButton>
                        <asp:LinkButton ID="lblApplicationId" runat="server" Visible="false"></asp:LinkButton>
                        <asp:RequiredFieldValidator ID="csvRequestType" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtRequestType"
                            ErrorMessage="Enter Request Type" Display="Dynamic" ValidationGroup="RequestTypeGroup" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Owner</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:UserValueBox ID="ppeOwner" runat="server" ValidationGroup="RequestTypeGroup" IsMandatory="false" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                        <div id="dvOwner" runat="server" style="display: none;">
                            <asp:Label ID="lblOwner" name="lblOwner" runat="server"></asp:Label>
                            <asp:HiddenField ID="hndOwner" runat="server" />
                            <img id="imgEditItem" onclick="editOwner()" runat="server" src="/content/images/edit-icon.png" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">WorkFlow Type<b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div class="ddlWorkflow" id="divddlWorkflow" runat="server" style="float: left; width: 100%;">
                        <div class="col-xs-11 noPadding">
                            <asp:DropDownList ID="ddlWorkflow" runat="server" AppendDataBoundItems="true" CssClass="aspxDropDownList">
                                <asp:ListItem Text="Full"></asp:ListItem>
                                <asp:ListItem Text="NoTest"></asp:ListItem>
                                <%--                        <asp:ListItem Text="Quick"></asp:ListItem>--%>
                                <asp:ListItem Text="Requisition"></asp:ListItem>
                                <asp:ListItem Text="SkipApprovals"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-xs-1 noPadding">
                            <img alt="Add WorkFlow Type" id="imgWorkflow" src="/content/images/plus-blue.png" style="cursor: pointer; float: right;"
                                onclick="javascript:$('.dvWorkflow').attr('style','display:block');hideddlWorkflow();" width="16" />
                        </div>
                    </div>
                    <div runat="server" id="divWorkflow" class="dvWorkflow" style="display: none; float: left;">
                        <div class="col-xs-11 noPadding">
                            <asp:TextBox runat="server" ID="txtWorkflow" CssClass="txtFuncArea"></asp:TextBox>
                        </div>
                        <div class="col-xs-1 noPadding">
                            <img alt="Cancel" width="16" style="float: right;" src="/content/images/close-blue.png" class="cancelModule"
                                onclick="javascript:$('.dvWorkflow').attr('style','display:none');showddlWorkflow();" />
                        </div>
                    </div>
                    <div style="width: auto; padding: 4px 4px 0px">
                        <asp:CustomValidator ID="csvdvWorkflow" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtWorkflow" ErrorMessage="Enter work flow type"
                            Display="Dynamic" OnServerValidate="csvdvWorkflow_ServerValidate" ValidationGroup="RequestTypeGroup" ForeColor="Red"></asp:CustomValidator>
                    </div>
                </div>
            </div>

            <div class="row showMoreLink">
                <a id="infoShowHide" onclick=" return ShowMoreFunction()" class="infoShowHideLabel">Show More >>></a>
            </div>
        </div>
    </fieldset>
    <div id="showMoreInfo" class="showMoreInfo row" style="display: none;">
        <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">RMM Category</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <div class="ddlCategory row" id="divddlCategory" runat="server" style="float: left; width: 100%;">
                            <div class="col-xs-11 noPadding">
                                <asp:DropDownList ID="ddlCategory" onchange="hideShowEdit('ddlCategory')" runat="server" CssClass="aspxDropDownList"></asp:DropDownList>
                            </div>
                            <div class="col-xs-1 noPadding">
                                <img alt="Edit RMM Category" runat="server" class="editicon" id="btRMMCategoryEdit" src="/content/images/editNewIcon.png"
                                    style="cursor: pointer; position: relative; float: right" onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory(1)" />
                                <img alt="Add RMM Category" id="imcategory" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right;"
                                    onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory(0)" />
                            </div>
                        </div>
                        <div runat="server" id="divCategory" class="divCategory" style="display: none; float: left;">
                            <div class="col-xs-11 noPadding">
                                <asp:TextBox runat="server" ID="txtCategory" CssClass="txtCategory"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdnRequestCategory"></asp:HiddenField>
                            </div>
                            <div class="col-xs-1 noPadding">
                                <img alt="Cancel RMM Category" width="16" src="/content/images/close-blue.png" class="cancelModule" style="float: right;"
                                    onclick="javascript:$('.divCategory').attr('style','display:none');showddlCategory();" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="row" id="trsubcategoryaspercateegory" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Sub Category</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlsubcategoryaspercategory" id="divddlSubcategoryaspercategory" runat="server" style="float: left; width: 100%">
                                <div class="col-xs-10 noPadding">
                                    <asp:DropDownList ID="ddlsubcategoryaspercategory" onchange="hideShowEdit('ddlsubcategoryaspercategory')"
                                        CssClass="aspxDropDownList" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-xs-2 noPadding">
                                    <img alt="Edit Category" runat="server" class="editicon" id="btSubCategoryEdit" width="16"
                                        src="/content/images/editNewIcon.png" style="cursor: pointer; position: relative; float: right"
                                        onclick="javascript:$('.divsubcategoryaspercategory').attr('style','display:block');hideddlSubCategoryaspercategory(1);" />
                                    <img alt="Add Category" id="Img3" src="/content/images/plus-blue.png" width="16" style="cursor: pointer; float: right; margin-right: 10px;"
                                        onclick="javascript:$('.divsubcategoryaspercategory').attr('style','display:block');hideddlSubCategoryaspercategory(0);" />
                                </div>
                            </div>
                            <div runat="server" id="divsubcategoryaspercategory" class="divsubcategoryaspercategory" style="display: none; float: left;">
                                <div class="col-xs-10 noPadding">
                                    <asp:TextBox runat="server" ID="txtSubcategoryaspercategory" CssClass="txtCategory"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="hdnRequestSubcategoryaspercategory"></asp:HiddenField>
                                </div>
                                <div class="col-xs-2 noPadding">
                                    <img alt="Cancel Category" width="16" src="/content/images/close-blue.png" class="cancelModule" style="float: right;"
                                        onclick="javascript:$('.divsubcategoryaspercategory').attr('style','display:none');showddlSubCategoryaspercategory();" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Budget Item</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <ugit:LookUpValueBox ID="ddlBudgetItem" CssClass="lookupValueBox-dropown" runat="server" FieldName="BudgetIdLookup"></ugit:LookUpValueBox>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">
                            <asp:Label ID="Label1" runat="server" Text="Item Code"></asp:Label></h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:Label ID="lblItemCode" runat="server"></asp:Label>
                        <asp:TextBox runat="server" ID="txtItemCode" CssClass="txtItemCode"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Estimated Hours</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox runat="server" ID="txtEstimatedHours"></asp:TextBox>
                    <div style="float: left; width: 305px">
                        <asp:RegularExpressionValidator ID="regextxtEstimatedHours" ValidationExpression="^[0-9]*(\.[0-9]+)?$" ValidateEmptyText="true" Enabled="true"
                            ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="RequestTypeGroup" ControlToValidate="txtEstimatedHours" runat="server"
                            CssClass="errormsg-container" ForeColor="Red" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <fieldset>
                <legend>SLA Override: Enter values to override module-level SLAs</legend>
                <dx:ASPxCallbackPanel ClientInstanceName="overrideSLAPanel" ID="overrideSLAPanel" runat="server" OnCallback="overrideSLAPanel_Callback">
                    <SettingsLoadingPanel Enabled="true" />
                    <PanelCollection>
                        <dx:PanelContent>
                            <div class="ms-formtable accomp-popup">
                                <div class="row">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Disable SLA</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <div id="dvChkDisableSLA" runat="server">
                                            <dx:ASPxCheckBox ID="chkDisableSLA" runat="server">
                                                <ClientSideEvents CheckedChanged="function(s,e){ overrideSLAPanel.PerformCallback(); }" />
                                            </dx:ASPxCheckBox>
                                        </div>
                                        <div id="dvDisableSLA" runat="server" style="display: none;">
                                            <asp:Label ID="lblDisableSLA" runat="server" EnableViewState="false"></asp:Label>
                                            <asp:HiddenField ID="hdnDisableSLA" runat="server" />
                                            <img id="imgDisableSLA" onclick="editDisableSLA()" runat="server" src="/content/images/edit-icon.png" />&nbsp;
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="trUse24x7Calendar" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Use 24x7 Calendar</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <dx:ASPxCheckBox ID="chkUse24x7Calendar" runat="server"></dx:ASPxCheckBox>
                                        <asp:HiddenField ID="hdnUse24x7Calendar" runat="server" />
                                    </div>
                                </div>
                                <div class="row" id="trSLARequestor" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Requestor Contact SLA</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                                            <asp:TextBox ID="txtRequestorContactSLA" runat="server" Text="0" />
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                            <asp:DropDownList ID="ddlRequestorContactSLAType" runat="server" CssClass="aspxDropDownList">
                                                <asp:ListItem Value="Days">Days</asp:ListItem>
                                                <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:RegularExpressionValidator ID="regextxtRequestorContactSLA" ValidationExpression="(\d+(\.\d{1,2})?)|(<Value Varies>)" ValidateEmptyText="true" ForeColor="Red"
                                                Enabled="true" runat="server" ControlToValidate="txtRequestorContactSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="RequestTypeGroup"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtRequestorContactSLA" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtRequestorContactSLA"
                                                ErrorMessage="Enter Requestor Contact SLA" Display="Dynamic" ValidationGroup="RequestTypeGroup" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="trSLAAssignment" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Assignment SLA</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                                            <asp:TextBox ID="txtAssignmentSLA" runat="server" Text="0" />
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                            <asp:DropDownList ID="ddlAssignmentSLAType" runat="server" CssClass="aspxDropDownList">
                                                <asp:ListItem Value="Days">Days</asp:ListItem>
                                                <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>


                                        <div>
                                            <asp:RegularExpressionValidator ID="regextxtEscalationMinutes" ValidationExpression="(\d+(\.\d{1,2})?)|(<Value Varies>)" ValidateEmptyText="true" ForeColor="Red"
                                                Enabled="true" runat="server" ControlToValidate="txtAssignmentSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="RequestTypeGroup"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtEscalationMinutes" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtAssignmentSLA"
                                                ErrorMessage="Enter Assignment SLA" Display="Dynamic" ValidationGroup="RequestTypeGroup" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="trSLAResolution" runat="server">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Resolution SLA</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                                            <asp:TextBox ID="txtResolutionSLA" runat="server" Text="0" />
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                            <asp:DropDownList ID="ddlResolutionSLAType" runat="server" CssClass="aspxDropDownList">
                                                <asp:ListItem Value="Days">Days</asp:ListItem>
                                                <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:RegularExpressionValidator ID="regextxtResolutionSLA" ValidationExpression="(\d+(\.\d{1,2})?)|(<Value Varies>)" ValidateEmptyText="true" Enabled="true" ForeColor="Red"
                                                runat="server" ControlToValidate="txtResolutionSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="RequestTypeGroup"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtResolutionSLA" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtResolutionSLA" ErrorMessage="Enter Resolution SLA"
                                                Display="Dynamic" ValidationGroup="RequestTypeGroup" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="trSLAClose" runat="server" style="margin-bottom: 25px;">
                                    <div class="ms-formlabel">
                                        <h3 class="ms-standardheader budget_fieldLabel">Close SLA</h3>
                                    </div>
                                    <div class="ms-formbody accomp_inputField">
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                                            <asp:TextBox ID="txtCloseSLA" runat="server" Text="0" />
                                        </div>
                                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                                            <asp:DropDownList ID="ddlCloseSLAType" runat="server" CssClass="aspxDropDownList">
                                                <asp:ListItem Value="Days">Days</asp:ListItem>
                                                <asp:ListItem Value="Hours">Hours</asp:ListItem>
                                                <asp:ListItem Value="Minutes">Minutes</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:RegularExpressionValidator ID="regextxtCloseSLA" ValidationExpression="(\d+(\.\d{1,2})?)|(<Value Varies>)" ValidateEmptyText="true" Enabled="true" ForeColor="Red"
                                                runat="server" ControlToValidate="txtCloseSLA" ErrorMessage="Invalid Format" Display="Dynamic" ValidationGroup="RequestTypeGroup"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtCloseSLA" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtCloseSLA" ErrorMessage="Enter Close SLA"
                                                Display="Dynamic" ValidationGroup="RequestTypeGroup" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxCallbackPanel>
            </fieldset>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <fieldset>
                <legend>Users</legend>
                <div class="ms-formtable accomp-popup">
                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Auto-Assign PRP</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:CheckBox ID="chkAutoAssignPRP" runat="server" AutoPostBack="false"></asp:CheckBox>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">PRP Group</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <ugit:UserValueBox ID="ppePrpGroup" runat="server" CssClass=" userValueBox-dropDown"></ugit:UserValueBox>
                                <div id="dvPRPGroup" runat="server" style="display: none;">
                                    <asp:Label ID="lblPRPGroup" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:HiddenField ID="hndPRPGroup" runat="server" />
                                    <img id="imgPRPGroup" onclick="editPRPGroup()" runat="server" src="/content/images/edit-icon.png" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">PRP</h3>
                            </div>
                            <ugit:UserValueBox ID="ppePRP" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">ORP</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <ugit:UserValueBox ID="ppeORP" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                                <div id="dvORP" runat="server" style="display: none;">
                                    <asp:Label ID="lblORP" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:HiddenField ID="hndORP" runat="server" />
                                    <img id="imgORP" onclick="editORP()" runat="server" src="/content/images/edit-icon.png" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row" style="margin-bottom: 25px;">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Escalation Manager</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <ugit:UserValueBox ID="ppeExecManeger" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                                <div id="dvExecManeger" runat="server" style="display: none;">
                                    <asp:Label ID="lblExecManeger" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:HiddenField ID="hndExecManeger" runat="server" />
                                    <img id="imgExecManeger" onclick="editExecManeger()" runat="server" src="/content/images/edit-icon.png" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Backup Escalation Manager</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <ugit:UserValueBox ID="ppeBackUpMan" runat="server" CssClass="userValueBox-dropDown"></ugit:UserValueBox>
                                <div id="dvBackUpMan" runat="server" style="display: none;">
                                    <asp:Label ID="lblBackUpMan" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:HiddenField ID="hndBackUpMan" runat="server" />
                                    <img id="imgBackUpMan" onclick="editBackUpMan()" runat="server" src="/content/images/edit-icon.png" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" id="locationWiseDiv" runat="server">
            <fieldset>
                <legend>Location</legend>
                <div class="ms-formtable accomp-popup">
                    <div class="row">
                        <a id="aLocAddItem" runat="server" href="">
                            <img id="Img2" runat="server" src="/content/images/plus-blue.png" width="16" />
                            <asp:Label ID="LblAddItem" runat="server" Text="Add Location"></asp:Label>
                        </a>
                    </div>
                    <div class="row">
                        <div id="divLocation" style="width: 100%;" runat="server">
                            <ugit:ASPxGridView ID="gvLocation" Width="100%" runat="server" KeyFieldName="ID" AutoGenerateColumns="false"
                                OnHtmlDataCellPrepared="gvLocation_HtmlDataCellPrepared" EnableCallBacks="true">
                                <Columns>
                                    <dx:GridViewDataTextColumn Name="aEdit" SortOrder="Ascending">
                                        <DataItemTemplate>
                                            <a id="aEdit" runat="server" href="">
                                                <img id="Imgedit" runat="server" src="~/Content/Images/edit-icon.png" />
                                            </a>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Name="Location" FieldName="LocationLookup" Caption="Location" SortOrder="Ascending">
                                    </dx:GridViewDataTextColumn>
                                    <%--<asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <a id="aLocation" runat="server" href=""></a>
                                            <asp:HiddenField runat="server" ID="hdnLocation" Value='<Bind("LocationLookup") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <%-- <asp:BoundField HeaderText="Owner" DataField="RequestTypeOwner" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="PRP Group" DataField="PRPGroup" HeaderStyle-HorizontalAlign="Left" />--%>
                                    <dx:GridViewDataTextColumn FieldName="OwnerUser" Caption="Owner"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="PRPGroupUser" Caption="PRP Group"></dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="PRPUser" Caption="PRP"></dx:GridViewDataTextColumn>
                                </Columns>

                            </ugit:ASPxGridView>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding" style="margin-top: 25px;">
            <fieldset>
                <legend style="margin-top: 25px;">Other</legend>
                <div class="ms-formtable accomp-popup">
                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Functional Area</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <ugit:LookUpValueBox ID="ddlFuncArea" runat="server" CssClass="lookupValueBox-dropown" FieldName="FunctionalAreaLookup"></ugit:LookUpValueBox>
                                <asp:Label ID="lblfunctionalareaText" runat="server" Visible="false"></asp:Label>
                                <div>
                                    <asp:Label ID="lblFuncArea" ForeColor="Red" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Request Type Description</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Issue Types (enter one per line)</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox runat="server" ID="txtIssueTypes" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                <asp:Label runat="server" ID="lblissuetypeoption" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Resolution Types (enter one per line)</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <asp:TextBox runat="server" ID="txtResolutionType" TextMode="MultiLine" Rows="5"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 col-sm-6 col-xs-6 noPaddingLeft">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Keywords</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <div runat="server" id="divShowKeywords" class="showKeywords">
                                    <span id="spaninputkeyword">
                                        <input id="inputkeyword" type="text" />
                                    </span>
                                </div>
                                <input class="hiddenkeyword" type="hidden" id="hdnkw" runat="server" value="" />
                            </div>
                        </div>
                        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
                            <div class="ms-formlabel">
                                <h3 class="ms-standardheader budget_fieldLabel">Email-to-Item Sender</h3>
                            </div>
                            <div class="ms-formbody accomp_inputField">
                                <div runat="server" id="dvticketsender" class="showticketsender">
                                    <span id="spanticketsender">
                                        <input id="txtticketsender" type="text" />
                                    </span>
                                </div>
                                <input class="hiddenticketsender" type="hidden" id="hdnticketsender" runat="server" value="" />
                            </div>
                        </div>
                    </div>
                    <div class="row" id="trSortToBottom" runat="server">
                        <%--<div class="ms-formlabel">
                             <h3 class="ms-standardheader budget_fieldLabel"></h3>
                        </div>--%>
                        <div class="ms-formbody accomp_inputField crm-checkWrap">
                            <asp:CheckBox ID="chkSortToBottom" runat="server" Text="Sort To Bottom" />
                        </div>
                    </div>
                    <div class="row" id="trOutOfOffice" runat="server">
                        <%--<div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel"></h3>
                        </div>--%>
                        <div class="ms-formbody accomp_inputField crm-checkWrap">
                            <asp:CheckBox ID="chkOutOfOffice" runat="server" Text="Out Of Office" />
                        </div>
                    </div>
                    <div class="row" id="trDeleted" runat="server">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Deleted</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField crm-checkWrap">
                            <%--<label style="font-weight:normal !important; float:right;padding-right:165px;"></label>--%>
                            <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">Task Template</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <div class="ddlTaskTemplate" id="dvTaskTemplate" runat="server" style="float: left; width: 100%;">
                                <asp:DropDownList ID="ddlTaskTemplate" runat="server" AppendDataBoundItems="true" CssClass="aspxDropDownList">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </fieldset>
        </div>

    </div>


    <div class="row addEditPopup-btnWrap">
        <%--<div class="col-md-12 col-sm-12 col-xs-12">--%>
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnClose_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
        <dx:ASPxButton ID="btnSave" runat="server" Text="Save" CssClass="primary-blueBtn" ToolTip="Save" ValidationGroup="RequestTypeGroup" OnClick="btSave_Click"></dx:ASPxButton>
        <%-- </div>--%>
    </div>
    <asp:HiddenField ID="hdnCategory" runat="server" />
    <asp:HiddenField ID="hdnWorkflow" runat="server" />
    <asp:HiddenField ID="hdnSubCategory" runat="server" />
    <asp:HiddenField ID="hdnApplicationId" runat="server" />
    <asp:HiddenField ID="hdnAppModuleId" runat="server" />
    <asp:HiddenField ID="hdnsubcategoryaspercategory" runat="server" />
</div>



<%--end new--%>







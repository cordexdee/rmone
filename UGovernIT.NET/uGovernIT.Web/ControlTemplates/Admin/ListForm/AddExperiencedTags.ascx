<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddExperiencedTags.ascx.cs" Inherits="uGovernIT.Web.AddExperiencedTags" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .auto-style1 {
        text-align: right;
        width: 190px;
        vertical-align: top;
        height: 20px;
    }

    .auto-style2 {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
        height: 20px;
    }

    .btnDelete {
        float: left;
        margin: 1px;
        color: #fff !important;
        background: url(/_layouts/15/images/uGovernIT/firstnavbgRed.png) repeat-x;
        cursor: pointer;
        padding: 6px;
    }
</style>
<script>
    function hideShowEdit(mainClass) {
        var jsMain = $("." + mainClass);
        var dropdown = jsMain.find("select");
        var editIcon = jsMain.find(".editicon");
        editIcon.hide();
        if (dropdown.val() != "-1" && dropdown.val() != "0") {
            editIcon.show();
        }
    }

    function hideddlCategory(action) {
        var category = $("#<%=ddlCategory.ClientID%> option:selected").text();
        $(".divddlCategory").hide();
        $("#<%=hdnCategory.ClientID%>").val('1');
        if (action == 1) {
            $("#<%=hdnRequestCategory.ClientID%>").val(category);
            $("#<%=txtCategory.ClientID%>").val(category);
        }
        else {
            $("#<%=hdnRequestCategory.ClientID%>").val("");
            $("#<%=txtCategory.ClientID%>").val("");
        }
    }
    function showddlCategory() {
        $("#<%=hdnCategory.ClientID%>").val('0');
        $(".divddlCategory").show();
    }
</script>
<asp:HiddenField ID="hdnCategory" runat="server" Value="0" />
<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="accomp-popup ms-formtable">

        <%--<div class="row" id="trl1" runat="server">
            <div class="ms-formlabel">
                <%--<h3 class="ms-standardheader budget_fieldLabel">Project Type</h3>--%>
        <%-- <h3 class="ms-standardheader budget_fieldLabel">Projects</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <%--<asp:TextBox ID="txtProjectType" runat="server" />--%>
        <%--<ugit:LookUpValueBox ID="ddlProjectType" runat="server" FieldName="ProjectType"
                    CssClass="lookupValueBox-dropown"></ugit:LookUpValueBox>--%>
        <%--<asp:DropDownList ID="ddlProjectType" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                        OnSelectedIndexChanged="ddlProjectType_SelectedIndexChanged">
                    </asp:DropDownList>--%>
        <%--<asp:DropDownList ID="ddlProjects" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                        OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged">
                    </asp:DropDownList>
                <%--<ugit:LookUpValueBox ID="ddlProjects" runat="server" FieldName="Projects"
                    CssClass="lookupValueBox-dropown"></ugit:LookUpValueBox>--%>
        <%-- <div>
                    <asp:RequiredFieldValidator ID="rfvddlProjects" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlProjects"
                        ErrorMessage="Select Projects" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>--%>


        <div class="row mt-2">
            <div class="col-md-12 col-sm-12 col-xs-12" id="trCategory" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Category</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <div class="divddlCategory" id="divddlCategory" runat="server" style="float: left; width: 100%;">
                        <div class="col-xs-10 noPadding">
                            <asp:DropDownList ID="ddlCategory" onchange="hideShowEdit('ddlCategory')" runat="server" EnableViewState="true" class="itsmDropDownList aspxDropDownList"></asp:DropDownList>
                        </div>
                        <div class="col-xs-2 noPadding mt-2">
                            <img alt="Edit Category" runat="server" class="editicon" id="btCategoryEdit"
                                src="/content/images/editNewIcon.png" width="16" style="cursor: pointer; position: relative; float: right;"
                                onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory(1)" />
                            <img alt="Add Category" id="Img1" width="16" src="/content/images/plus-blue.png" style="cursor: pointer; float: right; margin-right: 10px;"
                                onclick="javascript:$('.divCategory').attr('style','display:block');hideddlCategory(0);" />
                        </div>
                    </div>
                    <div runat="server" id="divCategory" class="divCategory" style="display: none; float: left;">
                        <div class="col-xs-10 noPadding">
                            <asp:TextBox CssClass="form-control" ID="txtCategory" runat="server"></asp:TextBox>
                            <asp:HiddenField runat="server" ID="hdnRequestCategory"></asp:HiddenField>
                        </div>
                        <div class="col-xs-2 noPadding mt-2">
                            <img alt="Cancel Category" style="float: right" width="16" src="/content/images/close-blue.png" class="cancelModule"
                                onclick="javascript:$('.divCategory').attr('style','display:none');showddlCategory();" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-sm-12 col-xs-12" id="Div1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Experience Tag</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtExperiencedTag" runat="server" autocomplete="off" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtExperiencedTag" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtExperiencedTag"
                            ErrorMessage="Enter Experience Tag" Display="Dynamic" ForeColor="Red" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="row" id="tr4" runat="server">
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <div class="d-flex justify-content-between align-items-center px-1" id="tr2" runat="server">
        <dx:ASPxButton ID="lnkDelete" CssClass="btn-danger1 ml-3" Text="Delete" ToolTip="Delete" runat="server" OnClick="lnkDelete_Click"></dx:ASPxButton>
        <div class="mr-3">
            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click"></dx:ASPxButton>
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
        </div>
    </div>
</div>

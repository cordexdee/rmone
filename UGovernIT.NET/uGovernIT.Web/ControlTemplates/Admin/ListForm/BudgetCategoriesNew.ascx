<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetCategoriesNew.ascx.cs" Inherits="uGovernIT.Web.BudgetCategoriesNew" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function showddlBudgetType() {
        $("#divTextBudgetType").hide();
        $("#divDDLBudgetType").show();
        $(".hiddenfield").val("false");
    }
    function showddlBudgetCategory() {
        
        $("#divTextBudgetCategory").hide();
        $("#divDDLBudgetCategory").show();
        $(".hiddenfield").val("false");
        <%--ValidatorEnable($("#<%=rfvtxtBudgetCategory.ClientID %>")[0], false);
        ValidatorEnable($("#<%=rfvddlBudgetCategory.ClientID %>")[0], true);--%>

    }
    function showddlBudgetSubCategory() {
        $("#divTextBudgetSubCategory").hide();
        $("#divDDLBudgetSubCategory").show();
        $(".hiddenfield").val("false");
        <%--ValidatorEnable($("#<%=rfvtxtBudgetSubCategory.ClientID %>")[0], false);
        ValidatorEnable($("#<%=rfvddlBudgetSubCategory.ClientID %>")[0], true);--%>
    }
    function hideddlBudgetType() {
        $("#divTextBudgetType").show();
        $("#divDDLBudgetType").hide();
        $(".hiddenfield").val("true");

    }
    function hideddlBudgetSubCategory() {
        $("#divTextBudgetSubCategory").show();
        $("#divDDLBudgetSubCategory").hide();
        $(".hiddenfield").val("true");
        <%--ValidatorEnable($("#<%=rfvtxtBudgetSubCategory.ClientID %>")[0], true);
        ValidatorEnable($("#<%=rfvddlBudgetSubCategory.ClientID %>")[0], false);--%>
    }
    function hideddlBudgetCategory() {
        
        $("#divTextBudgetCategory").show();
        $("#divDDLBudgetCategory").hide();
        $(".hiddenfield").val("true");
     <%--   ValidatorEnable($("#<%=rfvtxtBudgetCategory.ClientID %>")[0], true);
        ValidatorEnable($("#<%=rfvddlBudgetCategory.ClientID %>")[0], false);--%>

    }
    $(document).ready(function () {
        $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
        $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
        $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });

    function duplicate() {
        
        var ddlcat = null; var ddlsubcat = null;
        var txtcat = $("#<%=txtBudgetCategory.ClientID %>").val();
        var txtsubcat = $("#<%=txtBudgetSubCategory.ClientID %>").val();
        if (ddlBudgetCategory.GetSelectedItem() != null) {
            ddlcat = ddlBudgetCategory.GetSelectedItem().value;
        }
        if (ddlBudgetSubCategory.GetSelectedItem() != null) {
            ddlsubcat = ddlBudgetSubCategory.GetSelectedItem().value;
        }
        if ((txtcat != "" || txtsubcat != "") || (ddlcat != "None" || ddlsubcat != "None")) {
            return true;
        }
        //else if (ddlcat != "None" || ddlsubcat != "None")
        //{
        //    return true;
        //}
        //else if (txtcat != "" || txtsubcat != "")
        //{
        //    return true;
        //}
        else {
            //alert('Please select at least one dashboard to duplicate');
            $("#<%=lblMsgCategory.ClientID %>").html('Enter Budget Category');
            $("#<%=lblMsgSubCategory.ClientID %>").html('Enter Budget Sub-Category');
            return false;
        }



    }
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Category <b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <input type="hidden" class="hiddenfield" id="Hidden1" runat="server" />
                    <div id="divTextBudgetCategory" style="display: none;">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                            <asp:TextBox ID="txtBudgetCategory" runat="server" />
                            <%--<div>
                                <asp:RequiredFieldValidator ID="rfvtxtBudgetCategory" ValidateEmptyText="true" Enabled="false" runat="server" ControlToValidate="txtBudgetCategory"
                                    ErrorMessage="Enter Budget Category " ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </div>--%>
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Cancel Category" width="16" src="/Content/images/close-blue.png" class="cancelModule" onclick="showddlBudgetCategory();" />
                        </div>
                    </div>
                    <div id="divDDLBudgetCategory">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                            <dx:ASPxComboBox ID="ddlBudgetCategory" runat="server" Width="100%" ClientInstanceName="ddlBudgetCategory" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                <Items>
                                    <dx:ListEditItem Text="None" Value="None" Selected="true" />
                                </Items>
                            </dx:ASPxComboBox>
                            <%--<asp:RequiredFieldValidator ID="rfvddlBudgetCategory" runat="server" ControlToValidate="ddlBudgetCategory" InitialValue="None" ValidationGroup="Save" Enabled="true" ErrorMessage="Enter Budget Category" ForeColor="Red" Display="Dynamic" />--%>
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Add Category" id="imBudgetCategory" width="16" src="/Content/images/plus-blue.png" style="cursor: pointer;" onclick="hideddlBudgetCategory();" />
                        </div>
                    </div>
                    <dx:ASPxLabel ID="lblMsgCategory" runat="server" ForeColor="Red"></dx:ASPxLabel>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Sub-Category <b style="color: Red;">*</b></h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <input type="hidden" class="hiddenfield" id="Hidden2" runat="server" />
                    <div id="divTextBudgetSubCategory" style="display: none;">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                            <asp:TextBox ID="txtBudgetSubCategory" runat="server" />
                            <%--<div>
                                <asp:RequiredFieldValidator ID="rfvtxtBudgetSubCategory" ValidateEmptyText="true" Enabled="false" runat="server" ControlToValidate="txtBudgetSubCategory"
                                    ErrorMessage="Enter Budget Sub-Category " ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </div>--%>
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Cancel Category" width="16" src="/Content/images/close-blue.png" class="cancelModule" onclick="showddlBudgetSubCategory();" />
                        </div>
                    </div>
                    <div id="divDDLBudgetSubCategory">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                            <dx:ASPxComboBox ID="ddlBudgetSubCategory" ClientInstanceName="ddlBudgetSubCategory" runat="server" Width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                <Items>
                                    <dx:ListEditItem Text="None" Value="None" Selected="true" />
                                </Items>
                            </dx:ASPxComboBox>
                            <%--<asp:RequiredFieldValidator ID="rfvddlBudgetSubCategory" runat="server" ControlToValidate="ddlBudgetSubCategory" InitialValue="None" ValidationGroup="Save" Enabled="true" ErrorMessage="Enter Budget Sub-Category" ForeColor="Red" Display="Dynamic"/>--%>
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Add Category" id="imBudgetSubCategory" width="16" src="/Content/images/plus-blue.png" style="cursor: pointer;" onclick="hideddlBudgetSubCategory();" />
                        </div>
                    </div>
                    <dx:ASPxLabel ID="lblMsgSubCategory" runat="server" ForeColor="Red"></dx:ASPxLabel>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr13" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" runat="server" />

                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr1" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Category GL Code</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtAcronym" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr5" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Sub-Category GL Code</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtCOA" runat="server" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr6" runat="server">
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkIncludesStaffing" runat="server" Text="Includes Staffing" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr7" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Authorized To View</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UserValueBox ID="ppeAuthorizedToView" runat="server" CssClass="userValueBox-dropDown" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr8" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Authorized To Edit</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <ugit:UserValueBox ID="ppeAuthorizedToEdit" runat="server" CssClass="userValueBox-dropDown" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr9" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <input type="hidden" class="hiddenfield" id="hdnTextFieldVisible" runat="server" />
                    <div id="divTextBudgetType" style="display: none;">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                            <asp:TextBox ID="txtBudgetType" runat="server" />
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Cancel Category" width="16" src="/Content/images/close-blue.png" class="cancelModule" onclick="showddlBudgetType();" />
                        </div>
                    </div>
                    <div id="divDDLBudgetType">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                            <dx:ASPxComboBox ID="ddlBudgetType" runat="server" Width="100%" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                                <Items>
                                    <dx:ListEditItem Text="None" Value="None" Selected="true" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Add Category" id="imcategory" width="16" src="/Content/images/plus-blue.png" style="cursor: pointer;" onclick="hideddlBudgetType();" />
                        </div>
                        <%--<asp:DropDownList ID="ddlBudgetType" runat="server" Width="306px" >
                        </asp:DropDownList>--%>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr10" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Type GL Code</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtBudgetTypeCOA" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr14" runat="server">
                <%--<div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">CapEx</h3>
                </div>--%>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkCapEx" runat="server" Text="CapEx" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr11" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" AutoPostBack="false" CssClass="primary-blueBtn">
                    <ClientSideEvents Click="function(s,e){if(duplicate()){hdnBtn.DoClick();}}" />
                </dx:ASPxButton>
                <dx:ASPxButton ID="hdnBtn" ClientInstanceName="hdnBtn" EnableClientSideAPI="true" ClientVisible="false"
                    runat="server" Text="hdnBtn" OnClick="btnSave_Click" CssClass="primary-blueBtn">
                </dx:ASPxButton>
            </div>
        </div>
    </div>
</div>

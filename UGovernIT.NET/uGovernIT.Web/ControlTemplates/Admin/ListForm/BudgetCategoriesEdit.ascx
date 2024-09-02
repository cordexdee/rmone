<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetCategoriesEdit.ascx.cs" Inherits="uGovernIT.Web.BudgetCategoriesEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function showddlBudgetType()
    {
        $("#divTextBudgetType").hide();
        $("#divDDLBudgetType").show();
        $(".hiddenfield").val("false");
    }

    function hideddlBudgetType()
    {
        $("#divTextBudgetType").show();
        $("#divDDLBudgetType").hide();
        $(".hiddenfield").val("true");

    }
     $(document).ready(function () {
         $('.userValueBox-Table').parent().addClass("userValueBox-searchFilterWrap");
         $('.userValueBox-searchFilterWrap').parent().addClass("userValueBox-searchFilterContainer");
         $('.userValueBox-searchFilterContainer').parents().eq(3).addClass('userValueBox-dropDownWrap');
    });  
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap noPadding">
    <div class="ms-formtable accomp-popup">
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr12" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Category</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtBudgetCategory" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtBudgetCategory" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtBudgetCategory"
                            ErrorMessage="Enter Budget Category" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr3" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Sub-Category</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtBudgetSubCategory" runat="server" />
                    <div>
                        <asp:RequiredFieldValidator ID="rfvtxtBudgetSubCategory" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtBudgetSubCategory"
                            ErrorMessage="Enter Budget Sub-Category" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
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
                    <asp:TextBox ID="txtCOA" runat="server" Width="386px" />
                </div>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr6" runat="server">
                <%--<div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Includes Staffing</h3>
                </div>--%>
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
                    <ugit:UserValueBox ID="ppeAuthorizedToEdit" CssClass="userValueBox-dropDown" runat="server"/>
               
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-6 noPadding" id="tr9" runat="server">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Budget Type</h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <input type="hidden" class="hiddenfield" ID="hdnTextFieldVisible" runat="server"  />
                    <div id="divTextBudgetType" style="display:none;">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                             <asp:TextBox ID="txtBudgetType" runat="server"/>
                        </div>
                        <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Cancel Category" width="16" src="/Content/images/close-blue.png"  class="cancelModule" onclick="showddlBudgetType();" />
                        </div>
                    </div>
                    <div id="divDDLBudgetType">
                        <div class="col-md-11 col-sm-11 col-xs-11 noLeftPadding">
                             <asp:DropDownList ID="ddlBudgetType" runat="server" CssClass="itsmDropDownList aspxDropDownList" ></asp:DropDownList>
                        </div>
                       <div class="col-md-1 col-sm-1 col-xs-1 noPadding">
                            <img alt="Add Category" id="imcategory" width="16" src="/Content/images/plus-blue.png" style=" cursor:pointer;" onclick="hideddlBudgetType();" />
                       </div>
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
               <%-- <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">CapEx</h3>
                </div>--%>
                <div class="ms-formbody accomp_inputField crm-checkWrap">
                    <asp:CheckBox ID="chkCapEx" runat="server" Text="CapEx" />
                </div>
            </div>
            <div id="tr11" runat="server" class="col-md-6 col-sm-6 col-xs-6 noPadding">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
                </div>
                <div class="ms-formbody crm-checkWrap accomp_inputField">
                    <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                </div>
            </div>
        </div>
       <div class="row addEditPopup-btnWrap">
           <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" CssClass="primary-blueBtn" ValidationGroup="Save" OnClick="btnSave_Click"></dx:ASPxButton>
           </div>
       </div>
    </div>
</div>

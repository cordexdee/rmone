<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleBudgetAddEdit.ascx.cs" Inherits="uGovernIT.Web.ModuleBudgetAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" Namespace="uGovernIT.Web" Assembly="uGovernIT.Web" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody
    {
        background: none repeat scroll 0 0 #E8EDED;
        padding: 3px 6px 4px;
        vertical-align: top;
    }
    .floatleft{
        float:left;
    }
    .floatright{
        float:right;
    }
    .ms-formlabel
    {
        padding-left: 6px;
    }
    .ms-standardheader {
        float:right;
    }
    .width300 {
        width:300px;
    }
    .actionbutton {
        float:right;
        margin-top:10px;
        margin-right:2px;
    }
    .text-error {
        color: #a94442;
        font-weight: 500;
        margin-top: 5px;
    }
    .dxeCalendar_UGITNavyBlueDevEx {
        width: 134% !important;
    }

</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function validateStartDateAndEndDate(source, args) {

        var StartDate = Date.parse(document.getElementById('<%=dtcBudgetStartDate.Controls[0].ClientID%>').value);
        var EndDate = Date.parse(document.getElementById('<%=dtcBudgetEndDate.Controls[0].ClientID%>').value);

        if (StartDate <= EndDate) {
            args.IsValid = true;
        }
        else {
            args.IsValid = false;
        }
    }

    $(document).ready(function () {
         $('.popupUp-mainContainer').parent().addClass('popupForm-container');
    });
</script>

<asp:Panel runat="server" ID="pnlBudget" CssClass="accomp-popup popupUp-mainContainer">
    <fieldset>
        <div class="ms-formtable">
            <div class="trfbudgetcategory row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Category<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:DropDownList ID="ddlBudgetCategories" Width="200" ValidationGroup="formABudget" runat="server"></asp:DropDownList>
                        <span style="float: left;">

                            <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="formABudget" 
                                ID="rfvDDLBudgetCategories" runat="server" ControlToValidate="ddlBudgetCategories" 
                                ErrorMessage="Please select category" CssClass="text-error" ></asp:RequiredFieldValidator>
                        </span>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Budget Item<b style="color: Red;">*</b>
                        </h3>
                    </div>
                     <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtBudgetItemVal" ValidationGroup="formABudget" runat="server" CssClass="width300"></asp:TextBox>
                        <span style="/*float: left; width: 100%;*/">
                            <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="formABudget" 
                                ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBudgetItemVal" 
                                ErrorMessage="Please enter title" CssClass="text-error" ></asp:RequiredFieldValidator>
                        </span>
                    </div>
                </div>
            </div>
            <div class="trfbudgetamount row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                     <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Amount<b style="color: Red;">*</b>
                        </h3>
                    </div>
                     <div class="ms-formbody accomp_inputField">
                       <%-- <asp:TextBox ID="txtBudgetAmountf" runat="server"></asp:TextBox>--%>
                       <ugit:NumberValueBox ID="txtBudgetAmountf" runat="server" FieldName="BudgetAmount" CssClass="budget_amountFeild"></ugit:NumberValueBox>
                      
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                     <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Start Date
                        </h3>
                    </div>
                     <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcBudgetStartDate" EditFormat="Date" runat="server" CssClass="statusIsuue_dateFeild">
                            <CalendarProperties ></CalendarProperties>
                        </dx:ASPxDateEdit>
                       
                    </div>
                </div>
            </div>
            <div class="trfbudgetenddate row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">End Date
                        </h3>
                    </div>
                     <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcBudgetEndDate" runat="server" CssClass="statusIsuue_dateFeild"></dx:ASPxDateEdit>
                        
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Description
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtBudgetDescription" CssClass="budget_textareaField" ValidationGroup="formABudget" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row trfactualcomment" style="display:none;">
                <div class="ms-formlabel">
                    <h3 class="ms-standardheader budget_fieldLabel">Comment
                    </h3>
                </div>
                <div class="ms-formbody accomp_inputField">
                    <asp:TextBox ID="txtBudgetComment" CssClass="width300" ValidationGroup="rejectgroup" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    <span class="fullwidth errorspan">
                        <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="rejectgroup" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBudgetComment" ErrorMessage="Please enter comment"></asp:RequiredFieldValidator>
                    </span>
                </div>
            </div>
        </div>

    </fieldset>

            <div runat="server" id="divAttachments" class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Attachment
                        </h3>
                    </div>
                <div>
                    <ugit:FileUploadControl ID="FileUploadControl" runat="server" />
                </div>
                </div>
            </div>
    <div class="row">
        <%--<ugit:UploadAndLinkDocuments runat="server" id="UploadAndLinkDocuments" FolderName="Budgets" IsTabActive="true" />--%>
    </div>
        <div id="bindMultipleLink" class="row" runat="server">
       </div>
    <div id="div_budget" class="row">
        <div class="floatright">
                        <div class="floatleft">
                            <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" ImagePosition="Right"
                                CssClass="secondary-cancelBtn">
                            </dx:ASPxButton>
                        </div>
                        <div class="floatleft">
                        <dx:ASPxButton ID="btnBudgetSave" ValidationGroup="formABudget" ToolTip="Save" runat="server" Text="Save" OnClick="btnBudgetSave_Click"
                            ImagePosition="Right" CssClass="primary-blueBtn">            
                        </dx:ASPxButton>
                        </div>
        </div>
     </div>
</asp:Panel>
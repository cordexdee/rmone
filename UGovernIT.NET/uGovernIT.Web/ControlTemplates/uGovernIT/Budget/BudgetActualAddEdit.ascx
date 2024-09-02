<%--  --%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetActualAddEdit.ascx.cs" Inherits="uGovernIT.Web.BudgetActualAddEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        padding-left: 6px;
    }

    .ms-standardheader {
        float: right;
    }

    .width300 {
        width: 300px;
    }

    .actionbutton {
        float: right;
        margin-top: 10px;
        margin-right: 2px;
    }

    .text-error {
        color:red;
        font-weight: 500;
        margin-top: 5px;
    }
    .dxeCalendar_UGITNavyBlueDevEx {
        width: 134% !important;
    }
    .budgetBtn_wrap{
        float:right;
    }
    .secondaryCancelBtn-wrap {
        display: inline-block;
    }
    .floatleft{
        float:left;
    }
</style>
<asp:Panel runat="server" ID="pnlActual" CssClass="accomp-popup">
    <fieldset>
        <div class="ms-formtable">    
            <div class="trfactualbudget">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <asp:HiddenField ID="hfEditBudgetActualID" runat="server" />
                        <h3 class="ms-standardheader budget_fieldLabel">Budget Item<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField budget_inputField">
                        <dx:ASPxGridLookup ID="glActualBudget1" IncrementalFilteringMode="Contains" Width="342px" ViewStateMode="Disabled" OnDataBinding="glActualBudget1_DataBinding" runat="server" SelectionMode="Single" KeyFieldName="Id" TextFormatString="{2}" CssClass="statusIsuue_dateFeild">
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="BudgetCategory" Caption="" GroupIndex="1"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="BudgetSubCategory" GroupIndex="2" Caption="" Width="90%"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="BudgetItem" Caption="Budget Item" Width="60%"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn FieldName="BudgetAmount" PropertiesTextEdit-DisplayFormatString="${0:n2}"  Caption="Budget Amount" Width="40%" HeaderStyle-HorizontalAlign="Right"></dx:GridViewDataTextColumn>
                            </Columns>
                            <GridViewProperties>
                                <Settings GroupFormat="{1}"  VerticalScrollBarMode="Auto" VerticalScrollableHeight="500" />
                                <SettingsBehavior AllowDragDrop="false" AutoExpandAllGroups="true" />
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            </GridViewProperties>
                          <ValidationSettings ValidationGroup="formABudgetActual" ErrorDisplayMode="Text" Display="Dynamic" ErrorTextPosition="Top">
                              <RequiredField IsRequired="true" ErrorText="Please select a budget item" />
                          </ValidationSettings>
                        </dx:ASPxGridLookup>
                    </div>
                </div>
            </div>
            <div class="trfactualnote" id="trVendor" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Vendor</h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxComboBox ID="cbVendorList" runat="server" CssClass="statusIsuue_dateFeild" OnLoad="cbVendorList_Load" DropDownStyle="DropDown">     
                        </dx:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div class="trfactualnote" id="trInvoiceNumber" runat="server">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">PO/Invoice #
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtInvoiceNumber" CssClass="width300" ValidationGroup="formABudgetActual" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="trfactualdescription">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Description<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtActualTitle" CssClass="width300" ValidationGroup="formABudgetActual" runat="server" MaxLength="100"></asp:TextBox>
                        <span style="float: left; width: 100%;">
                            <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="formABudgetActual"
                                ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtActualTitle"
                                ErrorMessage="Please enter description" CssClass="text-error"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                </div>
            </div>
            <div class="trfactualamount">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Amount<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtActualAmount" ValidationGroup="formABudgetActual" runat="server" MaxLength="100"></asp:TextBox>
                        <span style="float: left; width: 100%;">
                            <asp:RequiredFieldValidator Display="Dynamic" ValidationGroup="formABudgetActual" ID="RequiredFieldValidator8" runat="server"
                                ControlToValidate="txtActualAmount" ErrorMessage="Please enter amount" CssClass="text-error"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" ValidationGroup="formABudgetActual" ID="RegularExpressionValidator1" runat="server"
                                ValidationExpression="^[0-9]+(\.[0-9]{1,2})?$" ControlToValidate="txtActualAmount" CssClass="text-error" ErrorMessage="Please use format '12345.99'"></asp:RegularExpressionValidator>
                        </span>
                    </div>
                </div>
            </div>
            <div class="trfactualstartdate">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Start Date<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcActualStartDate" runat="server" CssClass="statusIsuue_dateFeild"></dx:ASPxDateEdit>

                        <asp:RequiredFieldValidator ID="rvactualstartdate" runat="server" ValidationGroup="formABudgetActual"
                            ControlToValidate="dtcActualStartDate" Display="Dynamic" CssClass="text-error">
                        <span>Please enter Start Date.</span>
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvactualstartdate" runat="server" ValidationGroup="formABudgetActual"
                            ControlToValidate="dtcActualStartDate" Display="Dynamic" CssClass="text-error"
                            Type="Date" Operator="DataTypeCheck">
                        <span>Please enter a valid Start Date.</span>
                        </asp:CompareValidator>
                    </div>
                </div>
            </div>
            <div class="trfactualenddate">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">End Date<b style="color: Red;">*</b>
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <dx:ASPxDateEdit ID="dtcActualEndDate" runat="server" CssClass="statusIsuue_dateFeild"></dx:ASPxDateEdit>

                        <asp:RequiredFieldValidator ID="rvactualenddate" runat="server" ValidationGroup="formABudgetActual"
                            ControlToValidate="dtcActualEndDate" Display="Dynamic" CssClass="text-error">
                        <span>Please enter End Date.</span>
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cpactualenddate" runat="server" ValidationGroup="formABudgetActual"
                            ControlToValidate="dtcActualEndDate" Display="Dynamic" CssClass="text-error"
                            Type="Date" Operator="DataTypeCheck">
                        <span>Please enter a valid End Date.</span>
                        </asp:CompareValidator>
<%--                        <asp:CustomValidator ID="cusvactualenddate" runat="server" ValidationGroup="formABudgetActual"
                            ControlToValidate="dtcActualEndDate$dtcActualEndDateDate" Display="Dynamic" CssClass="text-error"
                            ClientValidationFunction="validateActualStartDateAndEndDate">
                        <span>End Date should be greater than Start Date.</span>
                        </asp:CustomValidator>--%>
                        <asp:CompareValidator ID="cvactualenddate" runat="server" ValidationGroup="formABudgetActual"
                            ControlToValidate="dtcActualEndDate" Display="Dynamic" CssClass="text-error"
                            Type="Date" Operator="GreaterThanEqual" ControlToCompare="dtcActualStartDate">
                        <span>End Date should be greater than Start Date.</span>
                        </asp:CompareValidator>
                    </div>
                </div>
            </div>
            <div class="trfactualnote">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="ms-formlabel">
                        <h3 class="ms-standardheader budget_fieldLabel">Notes
                        </h3>
                    </div>
                    <div class="ms-formbody accomp_inputField">
                        <asp:TextBox ID="txtActualNotes" CssClass="statusIsuue_dateFeild" ValidationGroup="formABudgetActual" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                </div>
            </div>
            
        </div>
    </fieldset>

            <div id="divAttachments" class="row">
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
     <div id="bindMultipleLink" runat="server">
                            </div>
    <div id="div_actuals" class="budgetBtn_wrap row fieldWrap cancelInvite">
                                      
                    <div class="secondaryCancelBtn-wrap">
                        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click" ImagePosition="Right"
                            CssClass="secondary-cancelBtn">
                        </dx:ASPxButton>
                    </div>
                    <div class="floatleft">
                        <dx:ASPxButton ID="btnBudgetSave" ValidationGroup="formABudgetActual" ToolTip="Save" runat="server" Text="Save" OnClick="btBudgetActualSave_Click"
                            ImagePosition="Right" CssClass="primary-blueBtn">            
                        </dx:ASPxButton>
                    </div>
                   
            </div>
</asp:Panel>
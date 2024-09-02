
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurveyRatingQuestionEditor.ascx.cs" Inherits="uGovernIT.Web.SurveyRatingQuestionEditor" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .radiobutton label {
        font-size: 12px;
        padding-left: 5px;
        padding-right: 25px;
        margin-top: 4px;    
    }
</style>

<asp:HiddenField ID="hfQuestionID" runat="server" />
<asp:HiddenField ID="hfServiceID" runat="server" />

<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="ms-formtable accomp-popup ">
        <div class="row" id="tr4" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">
                    Rating<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlRatingFields" CssClass="itsmDropDownList aspxDropDownList" runat="server" 
                    ValidationGroup="questionGroup" Width="100%">
                    <asp:ListItem Text="Rating 1" Value="Rating1"></asp:ListItem>
                    <asp:ListItem Text="Rating 2" Value="Rating2"></asp:ListItem>
                    <asp:ListItem Text="Rating 3" Value="Rating3"></asp:ListItem>
                    <asp:ListItem Text="Rating 4" Value="Rating4"></asp:ListItem>
                    <asp:ListItem Text="Rating 5" Value="Rating5"></asp:ListItem>
                    <asp:ListItem Text="Rating 6" Value="Rating6"></asp:ListItem>
                    <asp:ListItem Text="Rating 7" Value="Rating7"></asp:ListItem>
                    <asp:ListItem Text="Rating 8" Value="Rating8"></asp:ListItem>
                </asp:DropDownList>
                <asp:CustomValidator ID="rfValidator" runat="server" ValidationGroup="questionGroup" ControlToValidate="ddlRatingFields"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Only one question mapped to one rating." OnServerValidate="RFValidator_ServerValidate"></asp:CustomValidator>
            </div>
        </div>
        <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Display Mode</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="ddlRatingdisplaymode" runat="server" CssClass="aspxComBox-dropDown" 
                     DropDownStyle="DropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">
                    <Items>
                        <dx:ListEditItem Text="Rating Bar" Value="RatingBar" />
                        <dx:ListEditItem Text="Drop-down" Value="Dropdown" />
                        <dx:ListEditItem Text="Radio Buttons (left to right)" Value="RadioButtonsH" />
                        <dx:ListEditItem Text="Radio Buttons (one per line)" Value="RadioButtonsV" />
                    </Items>
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="row">
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID="chkMandatory" runat="server" Text="Mandatory" TextAlign="Right" AutoPostBack="false" />
            </div>
        </div>
        <div class="row" id="trTitle" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Title<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField"><div>
                <asp:TextBox ID="txtQuestion" Width="100%" runat="server" ValidationGroup="questionGroup"></asp:TextBox>
                </div><div>
                <asp:RequiredFieldValidator ID="rfvText" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtQuestion"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter title."></asp:RequiredFieldValidator></div>
            </div>
        </div>
        <div class="row" id="tr1" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Weight</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtWeight" runat="server" ValidationGroup="questionGroup" Text="1"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RequiredFieldValidator2" ValidationExpression="[0-9]+" runat="server" ValidationGroup="questionGroup" ControlToValidate="txtWeight"
                    Display="Dynamic" ForeColor="Red" ErrorMessage="Please enter Number."></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row" id="trRatingMax" runat="server">
             <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Rating Max</h3>
            </div>
            <div class="ms-formbody accomp_inputField  bC-radioBtnWrap">
                <asp:RadioButton ID="rbtn3" runat="server" AutoPostBack="true" Text="3" OnCheckedChanged="rbtn3_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn4" runat="server" Checked="true" AutoPostBack="true" Text="4" OnCheckedChanged="rbtn4_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn5" runat="server" AutoPostBack="true" Text="5" OnCheckedChanged="rbtn5_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn6" runat="server" AutoPostBack="true" Text="6" OnCheckedChanged="rbtn6_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn7" runat="server" AutoPostBack="true" Text="7" OnCheckedChanged="rbtn7_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn8" runat="server" AutoPostBack="true" Text="8" OnCheckedChanged="rbtn8_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn9" runat="server" AutoPostBack="true" Text="9" OnCheckedChanged="rbtn9_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
                <asp:RadioButton ID="rbtn10" runat="server" AutoPostBack="true" Text="10" OnCheckedChanged="rbtn10_CheckedChanged" GroupName="maxrating" CssClass="radiobutton" />
            </div>
        </div>
        <div class="row" id="tr2" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 1</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR1" runat="server" Width="100%" Text="Low"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 2</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR2" runat="server" Text="2"></asp:TextBox>
            </div>
        </div>
        <div  class="row" id="tr7" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 3</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR3" runat="server" Text="3"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr8" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 4</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR4" runat="server" Text="High"></asp:TextBox>
            </div>
        </div>

        <div class="row" id="tr9" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 5</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR5" runat="server" Width="100%" Text="High"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr3" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 6</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR6" runat="server" Width="100%" Text="High"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr10" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 7</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR7" Width="100%" runat="server" Text="High"></asp:TextBox>
            </div>
        </div>

        <div class="row" id="tr8a" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 8</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR8" runat="server" Width="100%" Text="High"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr9a" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 9</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR9" runat="server" Text="High"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr10a" runat="server" visible="false">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Value 10</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtR10" Width="100%" runat="server" Text="High"></asp:TextBox>
            </div>
        </div>
        <div class="row" id="tr11" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Default</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtDefaultValue" runat="server"
                    Width="100%"></asp:TextBox>
            </div>
        </div>

        <div class="row" id="tr5" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Help Text</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="questionHelpTextNew" runat="server" Columns="5"
                    TextMode="MultiLine" Width="100%"></asp:TextBox>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btDelete" runat="server" ClientInstanceName="btDelete" ValidationGroup="questionGroup"
                Text="Delete" CssClass="secondary-cancelBtn"
                    OnClick="BtDelete_Click" ClientVisible="false">
                <ClientSideEvents Click="function(){
                    return confirm('Are you sure you want to delete this entry?');
                    }" />
            </dx:ASPxButton>
            <dx:ASPxButton ID="btSaveQuestion1" runat="server" ClientInstanceName="btSaveQuestion1" 
                ValidationGroup="questionGroup" CssClass="primary-blueBtn"
                    Text="Save & New Rating" OnClick="BtSaveAndNewQuestionClick">                    
            </dx:ASPxButton>
            <dx:ASPxButton ID="btSaveQuestion" runat="server" ClientInstanceName="btSaveQuestion" ValidationGroup="questionGroup" 
                Text="Save & Close" CssClass="primary-blueBtn" OnClick="BtSaveQuestionClick"></dx:ASPxButton>
            <dx:ASPxButton ID="btClosePopup" runat="server" Text="Close" ClientInstanceName="btClosePopup" 
                OnClick="BtClosePopup_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
        </div>
    </div>
</div>



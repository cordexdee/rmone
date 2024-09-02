<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RankingCriteriaAdd.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.RankingCriteriaAdd" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function ddlRanking()
    {
        //var weigthedscore = Math.round(txtWeight.GetValue() * ddlRanking.GetValue(), 10) / 10;
        var weigthedscore = (txtWeight.GetValue() * ddlRanking.GetValue()).toFixed(1);
        
        txtWeightScore.SetValue(weigthedscore);

    }

    function txtweightchange()
    {
        //var weigthedscore = Math.round(txtWeight.GetValue() * ddlRanking.GetValue(), 10) / 10;
        var weigthedscore = (txtWeight.GetValue() * ddlRanking.GetValue()).toFixed(1);
       
        txtWeightScore.SetValue(weigthedscore);
    }

    //function btnDelete_Click(s, e) {
    //    if (confirm('Are you sure you want to delete?')) {
    //        e.processOnServer = true;
    //    }
    //    else {
    //        e.processOnServer = false;
    //    }
    //}
</script>
<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="ms-formtable accomp-popup">
        <div class="row">
                <dx:ASPxLabel ID="ASPxLabel1" runat="server" ForeColor="Red" Visible="false"></dx:ASPxLabel>
        </div>
        <div class="row">
                <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Visible="false"></dx:ASPxLabel>
        </div>
        <div id="trRankingCriteria" runat="server" class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Ranking Criteria<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtRankingCriteria" runat="server"  Width="100%"/>
                <div>
                    <asp:RequiredFieldValidator ID="rfvtxtImpact" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtRankingCriteria" 
                        ErrorMessage="Ranking Criteria" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </div>
            </div>
        </div>

        <div class="row" id="trDescription" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server"  
                     Width="100%"/>
            </div>
        </div>

        <div class="row" id="trStatus" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Ranking</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="ddlRanking" runat="server" ValueType="System.Int32" ClientInstanceName="ddlRanking"
                    IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList" CssClass="CRMDueDate_inputField comboBox-dropDown">
                    <ClientSideEvents SelectedIndexChanged="ddlRanking" />
                </dx:ASPxComboBox>
            </div>
        </div>
         <div class="row" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Weight<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
               <dx:ASPxSpinEdit ID="txtWeight" runat="server" Number="0" Width="100%" ClientInstanceName="txtWeight" 
                   MaxLength="5" CssClass="aspxSpinEdit-dropDown">
                <SpinButtons ShowIncrementButtons="False" ShowLargeIncrementButtons="False" />
                   <ClientSideEvents ValueChanged="txtweightchange"/>
               </dx:ASPxSpinEdit>
                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtWeight" 
                        ErrorMessage="Enter Weight" ForeColor="Red" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
            </div>
        </div>
         <div class="row" id="tr7" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Weight Score<b style="color: Red;">*</b></h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <dx:ASPxTextBox ID="txtWeightScore" ClientInstanceName="txtWeightScore" Width="100%" runat="server" ReadOnly="true"></dx:ASPxTextBox>
            </div>
        </div>
       <div class="row">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Delete</h3>
            </div>
            <div class="ms-formbody accomp_inputField crm-checkWrap">
                <asp:CheckBox ID ="chkDeleted" runat="server" Text="(Prevent use for new item)" TextAlign="Left" />
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton  ID="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" ToolTip="Cancel"  
                OnClick="btnCancel_Click"></dx:ASPxButton>  
            <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" 
                OnClick="btnSaveRankingCriteria_Click" CssClass="primary-blueBtn">
            </dx:ASPxButton>
        </div>
</div>
</div>
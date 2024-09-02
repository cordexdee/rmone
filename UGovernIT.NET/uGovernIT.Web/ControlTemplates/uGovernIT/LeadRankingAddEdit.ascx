<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeadRankingAddEdit.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.LeadRankingAddEdit" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function ddlRanking() {
       // var weigthedscore = Math.round(txtWeight.GetValue() * ddlRanking.GetValue(), 10) / 10;

        var weigthedscore = (txtWeight.GetValue() * ddlRanking.GetValue()).toFixed(1);
        
        txtWeightScore.SetValue(weigthedscore);

    }
</script>

<div class="row">
   <div class="ms-formtable accomp-popup">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div>
                <dx:ASPxLabel ID="lblError" runat="server" ForeColor="Red" Visible="false" > </dx:ASPxLabel>
            </div>
        </div>

        <div class="col-md-6 col-sm-6 colForXS leadPopup-padding"  id="trRankingCriteria" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Ranking Criteria<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:TextBox ID="txtRankingCriteria" runat="server"  ReadOnly="true"/>
            </div>
        </div>

        <div class="col-md-6 col-sm-6 colForXS leadPopup-padding" id="trDescription" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Description<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <asp:TextBox ID="txtDescription" TextMode="MultiLine" CssClass="ms-long" runat="server" ReadOnly="true"/>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 colForXS leadPopup-padding"  id="trStatus" runat="server" >
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Ranking</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="ddlRanking" runat="server" ValueType="System.Int32"
                    IncrementalFilteringMode="StartsWith" DropDownStyle="DropDownList" CssClass="CRMDueDate_inputField comboBox-dropDown" ClientInstanceName="ddlRanking">
                    <ClientSideEvents SelectedIndexChanged="ddlRanking" />
                </dx:ASPxComboBox>
            </div>
        </div>

        
         <div class="col-md-6 col-sm-6 colForXS leadPopup-padding" id="tr6" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Weight<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
               <%--<dx:ASPxSpinEdit ID="txtWeight" runat="server" Number="0" Width="100%">
                <SpinButtons ShowIncrementButtons="False" ShowLargeIncrementButtons="False"  Enabled="false"  />
               </dx:ASPxSpinEdit>--%>

                <dx:ASPxTextBox ID="txtWeight"  runat="server" ReadOnly="true" ClientInstanceName="txtWeight"></dx:ASPxTextBox>


                   <%-- <MaskSettings Mask="9999" />
                </dx:ASPxTextBox>--%>
            </div>
        </div>

         <div class="col-md-6 col-sm-6 colForXS leadPopup-padding" id="tr7" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Weight Score<b style="color: Red;">*</b>
                </h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                 <dx:ASPxTextBox ID="txtWeightScore" ClientInstanceName="txtWeightScore" runat="server" ReadOnly="true"></dx:ASPxTextBox>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12" id="tr2" runat="server">
            <%--<div align="left" style="padding-top: 5px;" >--%>
                <%--<div>
                    <asp:LinkButton ID="LnkbtnDelete" runat="server" Text="&nbsp;&nbsp;Delete&nbsp;&nbsp;" ToolTip="Delete"
                        OnClientClick="return confirm('Are you sure you want to delete?');" OnClick="LnkbtnDelete_Click">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">
                                Delete</b>
                            <i style="float: left; position: relative; top: -3px;left:2px">
                                <img src="/_layouts/15/images/uGovernIT/ButtonImages/cancel.png"  style="border:none;" title="" alt=""/>
                            </i> 
                        </span>
                    </asp:LinkButton>
                </div>--%>
            <%--</div>--%>
            <div class="leadRanking-editBtnContainer">
                <div class="leadRanking-editSaveBtn">
                    <dx:ASPxButton ID="btnSave" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" ValidationGroup="Save" OnClick="btnSaveRankingCriteria_Click"
                        CssClass="lead-saveBtn">
                    </dx:ASPxButton>
                </div>
                <div class="leadRanking-editCancelBtn">
                    <dx:ASPxButton  ID="btnCancel" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;"
                        ToolTip="Cancel" OnClick="btnCancel_Click" CssClass="lead-cancelBtn"></dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
</div>
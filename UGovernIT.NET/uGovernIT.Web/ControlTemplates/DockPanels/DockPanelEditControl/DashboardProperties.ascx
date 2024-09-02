<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardProperties.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.DockPanels.DashboardProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function pickFilter(filter)
    {
        filter = unescape(filter).replace(/@/g, "\'");
        $($(".filterexpression").get(0)).text(filter);
        hdnDataFilterExpression.Set("FilterExpression", filter);
    }
    function ClearConditionPicker()
    {
        $($(".filterexpression").get(0)).text('');
        hdnDataFilterExpression.Set("FilterExpression", '');
    }
   
</script>
<div class="col-md-12 col-sm-12 col-xs-12 ms-formtable accomp-popup">
    <div class="row" id="tr12" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <div>
                <dx:ASPxCheckBox ID="chkTitle" runat="server">
                    <ClientSideEvents
                        Init="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
            </div>
            <div>
                <dx:ASPxTextBox ID="txtTitle" CssClass="asptextBox-input" ClientInstanceName="txtTitle" runat="server"></dx:ASPxTextBox>
            </div>
        </div>
    </div>
    <div class="row" id="tr1" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Dashboard Group</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
             <dx:ASPxComboBox ID="ddlDashboardGroup" ClientInstanceName="ddlDashboardGroup" CssClass="aspxComBox-dropDown"
                 NullText="Select Dashboard Group" runat="server" ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%">
                <Items>
                    <dx:ListEditItem Text="Indivisible Dashboards" Value="Indivisible Dashboards"/>
                    <dx:ListEditItem Text="Super Dashboards" Value="Super Dashboards"/>
                    <dx:ListEditItem Text="Common Dashboards" Value="Common Dashboards"/>
                </Items>
                <ClientSideEvents SelectedIndexChanged="function(s,e){ddlDashboardView.PerformCallback(ddlDashboardGroup.GetValue());}" />
            </dx:ASPxComboBox>
        </div>
    </div>
    <div class="row" id="tr2" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Dashboard</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxComboBox  ID="ddlDashboardView" SettingsLoadingPanel-Enabled="true" CssClass="aspxComBox-dropDown" 
                ClientInstanceName="ddlDashboardView" runat="server" Width="100%" EnableCallbackMode="true" 
                OnCallback="ddlDashboardView_Callback" ListBoxStyle-CssClass="aspxComboBox-listBox"  >
            </dx:ASPxComboBox>
        </div>
    </div>
</div>


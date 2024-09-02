<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RMMWebpartProperties.ascx.cs" Inherits="uGovernIT.Web.RMMWebpartProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
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
        width: 100px;
        vertical-align: middle;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }
    .padding-left5 {
        padding-left:5px;
    }
</style>

<table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
     <tr id="tr12" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Title</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkTitle" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                <dx:ASPxTextBox ID="txtTitle" ClientInstanceName="txtTitle" runat="server"></dx:ASPxTextBox>
                </div>
            </div>
        </td>
    </tr>
     <tr id="tr1" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Allocation Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkAllocationTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlAllocationTabOrder.SetVisible(true)}else{ddlAllocationTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlAllocationTabOrder.SetVisible(true)}else{ddlAllocationTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlAllocationTabOrder" ClientInstanceName="ddlAllocationTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
     <tr id="tr2" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Actual Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkActualTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlActualTabOrder.SetVisible(true)}else{ddlActualTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlActualTabOrder.SetVisible(true)}else{ddlActualTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlActualTabOrder" ClientInstanceName="ddlActualTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
     <tr id="tr3" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Resource Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkResourceTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlResourceTabOrder.SetVisible(true)}else{ddlResourceTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlResourceTabOrder.SetVisible(true)}else{ddlResourceTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlResourceTabOrder" ClientInstanceName="ddlResourceTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
     <tr id="tr4" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Resource Planning Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkResourcePlanningTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlResourcePlanningTabOrder.SetVisible(true)}else{ddlResourcePlanningTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlResourcePlanningTabOrder.SetVisible(true)}else{ddlResourcePlanningTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlResourcePlanningTabOrder" ClientInstanceName="ddlResourcePlanningTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
    <tr id="tr5" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Resource Utilization Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkResourceAvailabilityTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlResourceAvailabilityTabOrder.SetVisible(true)}else{ddlResourceAvailabilityTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlResourceAvailabilityTabOrder.SetVisible(true)}else{ddlResourceAvailabilityTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlResourceAvailabilityTabOrder" ClientInstanceName="ddlResourceAvailabilityTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
    <tr id="tr6" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Allocation Timeline Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkAllocationTimelineTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlAllocationTimelineTabOrder.SetVisible(true)}else{ddlAllocationTimelineTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlAllocationTimelineTabOrder.SetVisible(true)}else{ddlAllocationTimelineTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlAllocationTimeline" ClientInstanceName="ddlAllocationTimelineTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
    <tr id="tr7" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Project Complexity Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkProjectComplexityTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlProjectComplexityTabOrder.SetVisible(true)}else{ddlProjectComplexityTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlProjectComplexityTabOrder.SetVisible(true)}else{ddlProjectComplexityTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlProjectComplexity" ClientInstanceName="ddlProjectComplexityTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
        <tr id="tr8" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Capacity Planning Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkCapacityReportTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlCapacityReportTabOrder.SetVisible(true)}else{ddlCapacityReportTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlCapacityReportTabOrder.SetVisible(true)}else{ddlCapacityReportTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlCapacityReport" ClientInstanceName="ddlCapacityReportTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
    <tr id="tr9" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Billing And Margins Report Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkBillingAndMarginTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlBillingAndMarginsTabOrder.SetVisible(true)}else{ddlBillingAndMarginsTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlBillingAndMarginsTabOrder.SetVisible(true)}else{ddlBillingAndMarginsTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlBillingAndMargins" ClientInstanceName="ddlBillingAndMarginsTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
    <tr id="tr10" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Executive KPI Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkExecutiveKPITab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlExecutiveKPITabOrder.SetVisible(true)}else{ddlExecutiveKPITabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlExecutiveKPITabOrder.SetVisible(true)}else{ddlExecutiveKPITabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlExecutiveKPI" ClientInstanceName="ddlExecutiveKPITabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
        <tr id="tr11" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Resource Utilization Index Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkResourceUtilizationIndexTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlResourceUtilizationIndexTabOrder.SetVisible(true)}else{ddlResourceUtilizationIndexTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlResourceUtilizationIndexTabOrder.SetVisible(true)}else{ddlResourceUtilizationIndexTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlResourceUtilizationIndex" ClientInstanceName="ddlResourceUtilizationIndexTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
        <tr id="tr13" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Manage Allocation Templates Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkManageAllocationTemplatesTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlManageAllocationTemplatesTabOrder.SetVisible(true)}else{ddlManageAllocationTemplatesTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlManageAllocationTemplatesTabOrder.SetVisible(true)}else{ddlManageAllocationTemplatesTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlManageAllocationTemplatesTabOrder" ClientInstanceName="ddlManageAllocationTemplatesTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>
        <tr id="tr14" runat="server">
    <td class="ms-formlabel">
        <h3 class="ms-standardheader">Manage Bench Tab</h3>
    </td>
    <td class="ms-formbody">
        <div>
            <div class="fleft">
            <dx:ASPxCheckBox ID="chkBenchab" runat="server">
                <ClientSideEvents 
                    Init="function(s,e){ if(s.GetChecked()){ddlBenchTabOrder.SetVisible(true)}else{ddlBenchTabOrder.SetVisible(false)} }"
                    CheckedChanged="function(s,e){ if(s.GetChecked()){ddlBenchTabOrder.SetVisible(true)}else{ddlBenchTabOrder.SetVisible(false)} }" />
            </dx:ASPxCheckBox>
            </div>
            <div class="fleft padding-left5">
              <dx:ASPxComboBox ID="ddlBenchTabOrder" ClientInstanceName="ddlBenchTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                  <Items>
                      <dx:ListEditItem Value="1" Text="1" />
                      <dx:ListEditItem Value="2" Text="2" />
                      <dx:ListEditItem Value="3" Text="3" />
                      <dx:ListEditItem Value="4" Text="4" />
                      <dx:ListEditItem Value="5" Text="5" />
                      <dx:ListEditItem Value="6" Text="6" />
                      <dx:ListEditItem Value="7" Text="7" />
                      <dx:ListEditItem Value="8" Text="8" />
                      <dx:ListEditItem Value="9" Text="9" />
                      <dx:ListEditItem Value="10" Text="10" />
                      <dx:ListEditItem Value="11" Text="11" />
                  </Items>
              </dx:ASPxComboBox>
            </div>
        </div>
    </td>
</tr>
   <%-- <tr id="tr13" runat="server">
        <td class="ms-formlabel">
            <h3 class="ms-standardheader">Financial View Tab</h3>
        </td>
        <td class="ms-formbody">
            <div>
                <div class="fleft">
                <dx:ASPxCheckBox ID="chkFinancialViewTab" runat="server">
                    <ClientSideEvents 
                        Init="function(s,e){ if(s.GetChecked()){ddlFinancialViewTabOrder.SetVisible(true)}else{ddlFinancialViewTabOrder.SetVisible(false)} }"
                        CheckedChanged="function(s,e){ if(s.GetChecked()){ddlFinancialViewTabOrder.SetVisible(true)}else{ddlFinancialViewTabOrder.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
                </div>
                <div class="fleft padding-left5">
                  <dx:ASPxComboBox ID="ddlFinancialView" ClientInstanceName="ddlFinancialViewTabOrder" Caption="Order" CaptionSettings-Position="Right" CaptionSettings-ShowColon="false" Width="40" runat="server">
                      <Items>
                          <dx:ListEditItem Value="1" Text="1" />
                          <dx:ListEditItem Value="2" Text="2" />
                          <dx:ListEditItem Value="3" Text="3" />
                          <dx:ListEditItem Value="4" Text="4" />
                          <dx:ListEditItem Value="5" Text="5" />
                          <dx:ListEditItem Value="6" Text="6" />
                          <dx:ListEditItem Value="7" Text="7" />
                          <dx:ListEditItem Value="8" Text="8" />
                          <dx:ListEditItem Value="9" Text="9" />
                          <dx:ListEditItem Value="10" Text="10" />
                          <dx:ListEditItem Value="11" Text="11" />
                          <dx:ListEditItem Value="12" Text="12" />
                      </Items>
                  </dx:ASPxComboBox>
                </div>
            </div>
        </td>
    </tr>--%>
</table>
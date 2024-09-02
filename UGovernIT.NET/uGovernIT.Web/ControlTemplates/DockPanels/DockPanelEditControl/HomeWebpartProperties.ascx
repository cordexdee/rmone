<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeWebpartProperties.ascx.cs" Inherits="uGovernIT.Web.HomeWebpartProperties" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<div class="col-md-12 col-sm-12 col-xs-12 ms-formtable accomp-popup noPadding">
    <div class="row" id="tr12" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Title</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <div class="fleft">
                <dx:ASPxCheckBox ID="chkTitle" runat="server" TextAlign="Right">
                    <ClientSideEvents
                    Init="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }"
                    CheckedChanged="function(s,e){ if(s.GetChecked()){txtTitle.SetVisible(true)}else{txtTitle.SetVisible(false)} }" />
                </dx:ASPxCheckBox>
            </div>
            <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
                <dx:ASPxTextBox ID="txtTitle" CssClass="asptextBox-input" Width="100%" ClientInstanceName="txtTitle" runat="server"></dx:ASPxTextBox>
            </div>
        </div>
    </div>
    <div class="row" id="tr13" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">No. of Preview Tickets</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxTextBox ID="txtNoOfTickets" CssClass="asptextBox-input" Width="100%" runat="server" NullText="No. Of tickets"></dx:ASPxTextBox>
        </div>
    </div>
    <div class="row">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Enable New Button</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxCheckBox ID="chkEnableNewButton" runat="server" ></dx:ASPxCheckBox>
        </div>
    </div>
    <div class="row" id="tr8" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Options</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
           <%-- <dx:ASPxCheckBox ID="chkSMSModules" runat="server" Text="SMS Module" TextAlign="Right"></dx:ASPxCheckBox>
            <dx:ASPxCheckBox ID="chkGovernanceModules" runat="server" Text="Governance Modules" TextAlign="Right"></dx:ASPxCheckBox>--%>
            <dx:ASPxCheckBox ID="chkShowBandedRows" runat="server" Text="Banding" TextAlign="Right"></dx:ASPxCheckBox>
            <dx:ASPxCheckBox ID="chkShowCompactRows" runat="server" Text="Compact" TextAlign="Right"></dx:ASPxCheckBox>
            <dx:ASPxCheckBox ID="chkServiceCatalog" runat="server" Text="Service Catalog" TextAlign="Right">
                <ClientSideEvents
                Init="function(s,e){ if(s.GetChecked()){chkShowServiceIcons.SetVisible(true)}else{chkShowServiceIcons.SetVisible(false)} }"
                CheckedChanged="function(s,e){ if(s.GetChecked()){chkShowServiceIcons.SetVisible(true)}else{chkShowServiceIcons.SetVisible(false)} }" />
            </dx:ASPxCheckBox>
            <div>
                <dx:ASPxCheckBox ID="chkShowServiceIcons" ClientInstanceName="chkShowServiceIcons" runat="server" 
                    Text="Show icons" ></dx:ASPxCheckBox>
               <%-- <dx:ASPxCheckBox ID="chkCRMModules" runat="server" Text="CRM Modules" TextAlign="Right"></dx:ASPxCheckBox>--%>
            </div>
            <div style="margin-top:15px;">
                <span>
                    <asp:RadioButtonList runat="server" ID="rblServiceViewType" AutoPostBack="false" 
                        RepeatDirection="Horizontal" CssClass="custom-radiobuttonlist">
                        <asp:ListItem Value="ListView" Text="List View"></asp:ListItem>
                        <asp:ListItem Value="ButtonView" Text="Button View"></asp:ListItem>
                        <asp:ListItem Value="DropdownView" Text="Dropdown View"></asp:ListItem>
                    </asp:RadioButtonList>
                </span>
            </div>
        </div>
    </div>
    <div class="row" id="tr9" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Display Ordering</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxComboBox ID="ddlMyTicketPanelOrder" runat="server" Width="20%" ListBoxStyle-CssClass="aspxComboBox-listBox"
                CaptionSettings-ShowColon="false" CaptionSettings-Position="Right" CssClass="aspxComBox-dropDown"
                Caption="My Ticket Panel">
                <Items>
                    <dx:ListEditItem Text="1" Value="1" />
                    <dx:ListEditItem Text="2" Value="2" />
                    <dx:ListEditItem Text="3" Value="3" />
                </Items>
            </dx:ASPxComboBox>
            <dx:ASPxComboBox ID="ddlServiceCatalogOrder" runat="server" Width="20%" ListBoxStyle-CssClass="aspxComboBox-listBox" 
                CaptionSettings-ShowColon="false"  CaptionSettings-Position="Right" CssClass="aspxComBox-dropDown"
                Caption="Service Catalog">
                <Items>
                    <dx:ListEditItem Text="1" Value="1" />
                    <dx:ListEditItem Text="2" Value="2" />
                    <dx:ListEditItem Text="3" Value="3" />
                </Items>
            </dx:ASPxComboBox>
            <dx:ASPxComboBox ID="ddlModulePanelOrder" runat="server" Width="20%" ListBoxStyle-CssClass="aspxComboBox-listBox"
                CaptionSettings-ShowColon="false"  CaptionSettings-Position="Right" CssClass="aspxComBox-dropDown"
                Caption="Modules Panel">
                <Items>
                    <dx:ListEditItem Text="1" Value="1" />
                    <dx:ListEditItem Text="2" Value="2" />
                    <dx:ListEditItem Text="3" Value="3" />
                </Items>
            </dx:ASPxComboBox>
        </div>
    </div>
    <div class="row" id="tr10" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Welcome Title</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxMemo ID="txtWelcomeTitle" CssClass="aspxMemo-linkBox" Width="100%" runat="server"></dx:ASPxMemo>
        </div>
    </div>
    <div class="row" id="tr11" runat="server">
        <div class="ms-formlabel">
            <h3 class="ms-standardheader budget_fieldLabel">Welcome Description</h3>
        </div>
        <div class="ms-formbody accomp_inputField">
            <dx:ASPxMemo ID="txtWelcomeDesc" CssClass="aspxMemo-linkBox" Width="100%" runat="server"></dx:ASPxMemo>
        </div>
    </div>
</div>



<%--Tab related properties configured from Config_TabView table, Add/Edit and view will be based on this table--%>
<dx:ASPxCallbackPanel ID="control" SettingsLoadingPanel-Enabled="true"  HideContentOnCallback="true" runat="server" ClientInstanceName="repeaterTabViewControlCallBackPanel" OnCallback="control_Callback">
    <PanelCollection>
        <dx:PanelContent>
            <asp:Repeater ID="repeaterTabView" runat="server">
            <HeaderTemplate>
                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse;overflow-x:auto" width="100%">
            </HeaderTemplate>
            <ItemTemplate>
                <tr id="tr12" runat="server">
                <td class="ms-formlabel" style="width:130px!important;">
                    <h3 class="ms-standardheader"><%# Eval("TablabelName") %></h3>
                    <asp:HiddenField ID="idField" runat="server"  Value=<%# Eval("ID") %> />
                    <asp:HiddenField ID="hdnTabOrder" runat="server"  Value=<%# Eval("TabOrder") %> />
                    <asp:HiddenField ID="hdnTablabelName" runat="server"  Value=<%# Eval("TablabelName") %> />
                </td>
                <td class="ms-formbody">
                    <div>
                        <div class="fleft">
                            <dx:ASPxCheckBox ID="chkTitle" runat="server" Checked=<%# Eval("ShowTab") %> ClientSideEvents-CheckedChanged='<%# "function(s,e) { if(s.GetChecked()){"+ Eval("TabName") +".SetVisible(true)}else{"+ Eval("TabName") +".SetVisible(false)} }" %>' 
                                ClientSideEvents-Init='<%# "function(s,e) { if(s.GetChecked()){"+ Eval("TabName") +".SetVisible(true)}else{"+ Eval("TabName") +".SetVisible(false)} }" %>' >
                       
                            </dx:ASPxCheckBox>
                        </div>
                        <div class="fleft padding-left5">
                            <dx:ASPxTextBox ID="txtDisplayName" ClientInstanceName=<%# Eval("TabName") %> Text=<%# Eval("TabDisplayName") %> runat="server" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-RequiredField-ErrorText="Required"></dx:ASPxTextBox>
                        </div>
                    </div>
                </td>
            </tr>
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
        </asp:Repeater>

                </dx:PanelContent>
            </PanelCollection>
</dx:ASPxCallbackPanel>

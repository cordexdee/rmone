<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpCardListPicker.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.HelpCardListPicker" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" id="helpCardScript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function RowSelectionChangeHelpCard(s, e)
    {
        debugger;
        if (e.isSelected)
        {
            //grid.GetSelectedFieldValues('TicketId', OnGetRowSelectedFieldValues);
            var key = s.GetRowKey(e.visibleIndex);
            var callback = eval("<%=this.CallBackWiki%>");
            if (typeof callback === "function") {
                callback(s,e,key);

            }
        }
    }  
    function helpCardListPickerPanel_CallBack(s, e)
    {
    }
</script>

<div class="col-md-12 col-sm-12 col-xs-12 noPadding">
    <div class="ms-formtable accomp-popup row">
        <div class="col-md-4 col-sm-4 col-xs-12 noPadding" id="dvModule" runat="server">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Category:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="ddlHelpCardCategory" EnableClientSideAPI="true" runat="server" CssClass=" aspxComBox-dropDown"
                    ClientInstanceName="ddlHelpCardCategory" AutoPostBack="true"  Visible="true" ListBoxStyle-CssClass="aspxComboBox-listBox" 
                    OnSelectedIndexChanged="ddlHelpCardCategory_SelectedIndexChanged">                            
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
                OnCustomUnboundColumnData="grid_CustomUnboundColumnData" OnDataBinding="grid_DataBinding" 
                ClientInstanceName="grid" CssClass="customgridview homeGrid"
                Width="100%" KeyFieldName="TicketId">
                <Columns>
                </Columns>
                <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                <SettingsPopup>
                    <HeaderFilter Height="100" />
                </SettingsPopup>
                <SettingsPager Position="TopAndBottom" PageSize="10">
                    <PageSizeItemSettings Items="10, 20, 30, 40, 50, 60, 70, 80, 90, 100" />
                </SettingsPager>
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass="homeGrid_headerColumn"></Header>
                </Styles>
                <ClientSideEvents SelectionChanged ="function(s,e){RowSelectionChangeHelpCard(s,e);}" />
            </ugit:ASPxGridView>                       
        </div>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiListPicker.ascx.cs" Inherits="uGovernIT.Web.WikiListPicker" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" id="wikiScript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function RowSelectionChange(s, e)
    {

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
    function wikiListPickerPanel_CallBack(s, e)
    {
    }
</script>
    <div class="ms-formtable accomp-popup col-md-12 col-sm-12 col-xs-12 noPadding">
        <div class="row ">
                    <div id="dvModule" runat="server" class="wiki-list-picker" style="float:left; margin-top: 6px">
                        <div class="ms-formlabel">
                            <h3 class="popup-fieldLabel">Select Module:</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <dx:ASPxComboBox ID="ddlModuleService" EnableClientSideAPI="true" runat="server" ClientInstanceName="ddlModuleService" 
                                OnSelectedIndexChanged="ddlModuleService_SelectedIndexChanged" Width="100%"
                                AutoPostBack="true"  Visible="true" CssClass="aspxComBox-dropDown" ListBoxStyle-CssClass="aspxComboBox-listBox">                                
                            </dx:ASPxComboBox>
                        </div>
                    </div>
                    <div id="dvCategory" class="ms-formbody accomp_inputField wiki-list-picker" runat="server" style="margin-top:23px;">
                        <dx:ASPxComboBox ID="ddlRequestType" runat="server" EnableClientSideAPI="true" ClientInstanceName="ddlRequestType" AutoPostBack="true" 
                            OnCallback="ddlRequestType_Callback" Visible="true" EnableCallbackMode="true" CssClass="aspxComBox-dropDown" 
                            ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%">
                        </dx:ASPxComboBox>
                    </div>
                        <%--<td style="float:right;">
                            <asp:Label ID="lblNotificationText" runat="server" Text="click on a row to select it" Font-Size="small" Font-Bold="false" ForeColor="#000066"></asp:Label>
                        </td>--%>
                    </div>
        <div class="row">
            <div class="wikiGrid-wrap col-md-12 col-sm-12 col-xs-12 noPadding">
                <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssClass="customgridview homeGrid"
                    OnCustomUnboundColumnData="grid_CustomUnboundColumnData" OnDataBinding="grid_DataBinding" ClientInstanceName="grid"
                    Width="100%" KeyFieldName="TicketId" StylesPager-PageSizeItem-ComboBoxStyle-CssClass="gridPageSize-dropDown">
                    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                    <Columns>
                    </Columns>
                    <settingscommandbutton>
                        <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                    </settingscommandbutton>
                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                    <SettingsPopup>
                        <HeaderFilter Height="100" />
                    </SettingsPopup>
                    <Styles>
                        <Row CssClass="homeGrid_dataRow"></Row>
                        <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                        <SelectedRow CssClass="homeGrid-selectedRow"></SelectedRow>
                    </Styles>
                    <SettingsPager Position="TopAndBottom" PageSize="10" PageSizeItemSettings-DropDownImage-Url="../../../Content/Images/DropdownArrow.png">
                        <PageSizeItemSettings Items="10, 20, 30, 40, 50, 60, 70, 80, 90, 100" />
                    </SettingsPager>
                    <ClientSideEvents SelectionChanged ="function(s,e){RowSelectionChange(s,e);}" />
                </ugit:ASPxGridView>
            </div>
        </div>
    </div>
        




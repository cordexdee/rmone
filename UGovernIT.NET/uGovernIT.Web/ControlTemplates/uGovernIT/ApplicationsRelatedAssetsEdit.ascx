<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationsRelatedAssetsEdit.ascx.cs" Inherits="uGovernIT.Web.ApplicationsRelatedAssetsEdit" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<table class="fullwidth">
    <tr>
        <td>
            <div>
                <span style="float: left; padding-right: 3px; padding-top: 1px;"><b>Environment:</b><b style="color: Red;">*</b></span>
                <span style="float: left;">
                    <asp:DropDownList ID="ddlEnvironment" CssClass="enviroment" runat="server"></asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvddlEnvironment" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="ddlEnvironment"
                            ErrorMessage="Select Environment" ForeColor="Red" InitialValue="0" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                    </div>
                </span>
                <img alt="Add Enviroment" id="imgEnviroment" runat="server" src="/Content/images/uGovernIT/add_icon.png" style="cursor: pointer; float: left; padding-top: 3px; padding-left: 5px;" />
            </div>
            <span style="padding-left: 10px; float: left; padding-right: 5px; padding-top: 1px;"><b>Server Functions:</b></span>
            <div id="divServerFunctions" style="float: left" runat="server">
                <dx:ASPxGridLookup Visible="true" SelectionMode="Multiple" ID="glServerFunctions" runat="server" KeyFieldName="ID" MultiTextSeparator="; ">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                        <dx:GridViewDataTextColumn FieldName="Title" Width="170px">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <GridViewProperties>
                        <Settings ShowGroupedColumns="false" VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                    </GridViewProperties>
                    <ClientSideEvents />
                </dx:ASPxGridLookup>
            </div>
        </td>
        <td style="float: right; padding-right: 5px">
            <asp:Label ID="lblNotificationText" runat="server" Text="Click on a row to select asset" Font-Bold="true" ForeColor="#000066"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <div style="padding-top: 5px;">
                <dx:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" OnCustomColumnDisplayText="grid_CustomColumnDisplayText"
                    OnDataBinding="grid_DataBinding" ClientInstanceName="grid"
                    Width="100%" KeyFieldName="ID">
                    <Columns></Columns>
                    <SettingsBehavior AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <SettingsPager Position="TopAndBottom">
                        <PageSizeItemSettings Items="10, 20, 30, 40, 50, 60, 70, 80, 90, 100" />
                    </SettingsPager>
                    <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" AutoFilterCondition="Contains" ShowFilterRow="true" ShowFilterBar="Auto" ShowFooter="true" ShowGroupPanel="false" />
                </dx:ASPxGridView>
            </div>
        </td>
    </tr>
    <tr id="tr3" runat="server">
        <td colspan="2">
            <div style="float: right; padding-top: 10px">
                <asp:LinkButton ID="btnSave" runat="server" Text="&nbsp;&nbsp;Update&nbsp;&nbsp;" ToolTip="Update" ValidationGroup="Save" OnClick="btnSave_Click">
                    <span class="button-bg">
                        <b style="float: left; font-weight: normal;">
                            Save</b>
                        <i style="float: left; position: relative; top: -3px;left:2px">
                            <img src="/Content/images/uGovernIT/ButtonImages/save.png"  style="border:none;" title="" alt=""/>
                        </i> 
                    </span>
                </asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" Text="&nbsp;&nbsp;Cancel&nbsp;&nbsp;" ToolTip="Cancel" >
                    <span class="button-bg">
                        <b style="float: left; font-weight: normal;">
                            Cancel</b>
                        <i style="float: left; position: relative; top: -3px;left:2px">
                            <img src="/Content/images/uGovernIT/ButtonImages/cancelwhite.png"  style="border:none;" title="" alt=""/>
                        </i> 
                    </span>
                </asp:LinkButton>
            </div>
        </td>
    </tr>
</table>
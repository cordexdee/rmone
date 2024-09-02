
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationServerEdit.ascx.cs" Inherits="uGovernIT.Web.ApplicationServerEdit" %>
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
        width: 300px;
        vertical-align: top;
    }

    .ms-standardheader {
        text-align: right;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .enviroment {
        margin-left: 0px;
    }
</style>

<table width="100%">
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
                <dx:ASPxButton ID="imgEnviroment" runat="server" RenderMode="Link" AutoPostBack="false" Image-Url="/Content/images/add_icon.png"
                     style="cursor: pointer; float: left; padding-top: 3px; padding-left: 5px;"></dx:ASPxButton>
                <%--<img alt="Add Enviroment" id="imgEnviroment" runat="server" src="/Content/images/add_icon.png" style="cursor: pointer; float: left; padding-top: 3px; padding-left: 5px;" />--%>
            </div>
            <span style="padding-left: 10px; float: left; padding-right: 5px; padding-top: 1px;"><b>Server Functions:</b></span>
            <div id="divServerFunctions" style="float:left" runat="server">
                <dx:ASPxGridLookup Visible="true"   SelectionMode="Multiple" ID="glServerFunctions" runat="server" KeyFieldName="ServerFunctionsChoice" MultiTextSeparator="; ">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" />
                        <dx:GridViewDataTextColumn FieldName="ServerFunctionsChoice"  Width="170px" >
                        </dx:GridViewDataTextColumn>
                    </Columns>

                    <GridViewProperties>
                        <Settings ShowGroupedColumns="false"  VerticalScrollBarMode="Auto" />
                        <SettingsBehavior AllowSort="false" AllowGroup="true" AutoExpandAllGroups="true" />
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                    </GridViewProperties>
                    <ClientSideEvents  />
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
                <ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False"
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

                </ugit:ASPxGridView>
            </div>
        </td>
    </tr>

    <tr id="tr3" runat="server">
        <td colspan="2">
            <div style="float: right; padding-top: 10px">
                <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel"  OnClick="btnCancel_Click" CssClass="secondary-cancelBtn"></dx:ASPxButton>
                <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            </div>
        </td>
    </tr>
</table>

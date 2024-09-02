<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApplicationsRelatedAssetsView.ascx.cs" Inherits="uGovernIT.Web.ApplicationsRelatedAssetsView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<div class="col-md-12 col-sm-12 col-xs-12">
    <table class="fullwidth">
        <tr>
            <td>
                <dx:ASPxGridView ID="appServerGrid" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
                    ClientInstanceName="appServerGrid" Width="100%" KeyFieldName="ID" OnRowDeleting="appServerGrid_RowDeleting" SettingsBehavior-ConfirmDelete="true">
                    <Columns>
                        <dx:GridViewDataTextColumn Caption="Id" FieldName="ID" Visible="false">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Application" FieldName="AppTitle" HeaderStyle-Font-Bold="true"
                            CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Width="40%">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Application Environment" FieldName="EnvTitle" Width="30%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Server Function(s)" FieldName="ServerFunctionsChoice" Width="30%" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewCommandColumn Name="CommandColumn" ShowDeleteButton="true">
                        </dx:GridViewCommandColumn>
                    </Columns>

                    <Styles>
                        <AlternatingRow CssClass="ugitlight1lightest"></AlternatingRow>
                    </Styles>

                    <SettingsBehavior AllowSelectByRowClick="false" AutoExpandAllGroups="true" />
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <SettingsPager Position="TopAndBottom">
                        <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                    </SettingsPager>
                    <Settings ShowHeaderFilterButton="false" GridLines="None" />
                    <SettingsCommandButton>
                        <DeleteButton RenderMode="Image" Image-Url="/Content/images/delete-icon.png"></DeleteButton>
                    </SettingsCommandButton>
                    <SettingsText ConfirmDelete="Are you sure you want to delete the relationship to this item?" EmptyDataRow="There are no items to show in this view." />
                </dx:ASPxGridView>
            </td>
        </tr>
        <tr>
            <td>
                <span style="float: right; margin-top: 5px;">
                    <a href="javascript:" title="New" id="aAddNew" runat="server">
                        <span class="button-bg">
                            <b style="float: left; font-weight: normal;">New</b>
                            <i style="float: left; position: relative; top: 1px; left: 2px">
                                <img alt="" title="" style="border: none;" src="/Content/images/uGovernIT/add_icon.png">
                            </i>
                        </span>
                    </a>
                </span>
            </td>
        </tr>
    </table>
</div>

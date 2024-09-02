<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CurrentTenant.ascx.cs" Inherits="uGovernIT.Web.CurrentTenant" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<ContentTemplate>
        <div class="col-md-12 col-sm-12 col-xs-12">
            <dx:ASPxGridView ID="superAdminGrid" runat="server" CssClass="customgridview  homeGrid" Width="100%">
            <Columns>
                <%--<dx:GridViewCommandColumn ShowSelectCheckbox="true"></dx:GridViewCommandColumn>--%>
                <dx:GridViewDataTextColumn FieldName="TenantName" Caption="Current Users" SettingsHeaderFilter-DateRangeCalendarSettings-EnableMultiSelect="true"/>

                 <dx:GridViewDataDateColumn FieldName="Created" Caption="Since" >
                    <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>

                   <dx:GridViewDataTextColumn FieldName="Trial" Caption="Trial?" SettingsHeaderFilter-DateRangeCalendarSettings-EnableMultiSelect="true"/>
                
                <dx:GridViewDataTextColumn FieldName="UsageStatistics" Caption="Usage Statistics" />
                              
            </Columns>

            <Settings ShowFooter="false" ShowHeaderFilterButton="false" />
            <SettingsBehavior AllowSort="false" AllowDragDrop="false" AutoExpandAllGroups="true" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Row HorizontalAlign="Center" CssClass="homeGrid_dataRow"></Row>
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="homeGrid_headerColumn"></Header>
                <%--<AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>--%>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            </Styles>

        </dx:ASPxGridView>
        </div>
    </ContentTemplate>
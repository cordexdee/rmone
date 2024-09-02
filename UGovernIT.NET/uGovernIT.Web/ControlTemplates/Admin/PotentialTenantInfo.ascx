<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="PotentialTenantInfo.ascx.cs" Inherits="uGovernIT.Web.PotentialTenantInfo" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .homeGrid_headerColumn {
        padding-left: 6px;
        padding-right: 6px;
    }
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        gvPotentialTenants.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gvPotentialTenants.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<dx:ASPxLoadingPanel ID="adUserLoading" ClientInstanceName="adUserLoading" Modal="True" runat="server" Text="Please Wait..." CssClass="customeLoader"></dx:ASPxLoadingPanel>
    <ContentTemplate>
        <div class="col-md-12 col-sm-12 col-xs-12">
            <dx:ASPxGridView ID="gvPotentialTenants" runat="server" ClientInstanceName="gvPotentialTenants" CssClass="customgridview  homeGrid" Width="100%" KeyFieldName="Id"  SettingsPager-PageSize="20">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="Company" Caption="Company">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Name" Caption="Name">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </dx:GridViewDataTextColumn>
<%--                <dx:GridViewDataTextColumn FieldName="Trials" Caption="Admin" SettingsHeaderFilter-DateRangeCalendarSettings-EnableMultiSelect="true"/>
                <dx:GridViewDataTextColumn FieldName="AccountId" Caption="Tenant Name" SettingsHeaderFilter-DateRangeCalendarSettings-EnableMultiSelect="true"/>--%>
                <dx:GridViewDataTextColumn FieldName="Email" Caption ="Email">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption ="Title">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Contact" Caption ="Phone">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Url" Caption ="Url">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataDateColumn FieldName="Created" CellStyle-HorizontalAlign="Center">
                    <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>               
                <dx:GridViewDataTextColumn>
                    <DataItemTemplate>
                        <asp:ImageButton CssClass="superAdmin-gridDelIcon" style ="margin-left:0px !important" Width="15" ID="deleteTenant" runat="server"  OnClick="DeleteTenant_Click" OnClientClick='<%# "javascript:return confirm(\"Do you want to Delete this Tenant information? \");"  %>' 
                             src="/Content/Images/grayDelete.png" ToolTip="Delete Tenant Information"
                             CommandArgument='<%# string.Format("{0}", Eval("Id"))%>'  />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>                
            </Columns>
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Settings ShowFooter="false" ShowHeaderFilterButton="true"  />
            <SettingsBehavior AllowSort="false" AllowDragDrop="false" AutoExpandAllGroups="true" />
            <SettingsPopup>
                <HeaderFilter Height="350px" />
            </SettingsPopup>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Row HorizontalAlign="Left" CssClass="homeGrid_dataRow" ></Row>
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="homeGrid_headerColumn"></Header>
                <%--<AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>--%>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            </Styles>

        </dx:ASPxGridView>
             <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                 ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                     UpdateGridHeight();
                 });
            </script>
        </div>
    </ContentTemplate>


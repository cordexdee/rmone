
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelatedCompanies.ascx.cs" Inherits="uGovernIT.Web.RelatedCompanies" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
        display: none;
    }

    .clickedTab {
        background-color: #0072c6;
    }

        .clickedTab a {
            color: #fff !important;
        }

    .pagerBox td table tr td span {
        /* font-size : larger; */
        border: 1px solid black;
        padding: 0px 3px;
    }

    table.ms-listviewtable > tbody > tr > td {
        border: none;
    }

    .ms-viewheadertr .ms-vh2-gridview {
        background: transparent !important;
        height: 22px;
    }

    .ms-vh2 .ms-selectedtitle .ms-vb, .ms-vh2 .ms-unselectedtitle .ms-vb {
        text-align: left;
    }

    .ms-listviewtable .ms-vb2, .ms-summarystandardbody .ms-vb2 {
        text-align: left;
    }

    .pctcompletecolumn {
        padding-right: 10px;
        text-align: center;
    }

    .fleft {
        float: left;
    }


    .action-container {
        background: none repeat scroll 0 0 #FFFFAA;
        border: 1px solid #FFD47F;
        float: left;
        padding: 1px 5px 0;
        position: absolute;
        z-index: 1000;
        margin-top: -4px;
        margin-left: 3px;
        right: 0px;
        top: 0px;
    }

    .ucontentdiv {
        float: left;
        height: 18px;
        float: left;
        padding: 1px 6px 0;
        margin: 2px 4px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    AlertToSaveChanges();

    function openDialog(path, params, titleVal) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '600px', '350px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function changeMyActivityView(viewType) {
        $("#myActivityViewType").val(viewType);
        set_cookie("myActivityViewType", viewType);
        return true;
    }

    function OpneEditActivity(obj, activityId) {

        var param = "&ID=" + activityId + "&ticketID=0";
        window.parent.UgitOpenPopupDialog('<%=absoluteUrlEdit%>', param, "Activities - Edit Item", '600px', '310px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function showContactActivityActions(trObj, activityId) {
        $("#ContactActionButtons" + activityId).css("display", "block");
    }

    function hideContactActivityActions(trObj, activityId) {
        //show description icon
        $("#ContactActionButtons" + activityId).css("display", "none");
    }

   

   

</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grdRelatedCompanies.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grdRelatedCompanies.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" Modal="True"></dx:ASPxLoadingPanel>

<div class="col-md-12 col-sm-12 col-xs-12">
    <div class="row">
        <div>
            <asp:HiddenField ID="hndContactId" runat="server" Value="0" />

            <dx:ASPxCallbackPanel ID="cbPanel" OnCallback="cbPanel_Callback" width="100%"  runat="server" ClientInstanceName="cbPanel" RenderMode="Table">
                <%--<ClientSideEvents EndCallback="cbPanel_EndCallback" />--%>
                <PanelCollection>
                    <dx:PanelContent>
                        <dx:ASPxGridView ID="grdRelatedCompanies" AutoGenerateColumns="False" runat="server" ClientInstanceName="grdRelatedCompanies" OnHtmlRowPrepared="grdRelatedCompanies_HtmlRowPrepared"
                            SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="100%" OnHtmlDataCellPrepared="grdRelatedCompanies_HtmlDataCellPrepared"
                            CssClass="customgridview homeGrid">
                            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true"></settingsadaptivity>
                            <SettingsText EmptyDataRow="No record found."></SettingsText>
                            <Columns>
                               <%-- <dx:GridViewDataColumn FieldName="ItemOrder" VisibleIndex="1"  Caption="#"  SortIndex="0" SortOrder="Ascending" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Width="10px">
                                    <Settings HeaderFilterMode="CheckedList"  />
                                </dx:GridViewDataColumn>--%>

                                <dx:GridViewDataColumn FieldName="CompanyName" VisibleIndex="2"  Caption="Company" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" AllowTextTruncationInAdaptiveMode="false" >
                                    <Settings HeaderFilterMode="CheckedList" />
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <DataItemTemplate>
                                        <div style="float: left; width: 150px; position: relative;">
                                            <div style="float: left;">
                                                <a id="aTitle" runat="server" style="cursor: pointer" href="" onload="aTitle_Load"></a>
                                            </div>
                                        </div>
                                    </DataItemTemplate>
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="Address" VisibleIndex="4"  Caption="Address" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList"  />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="ContactLookup" VisibleIndex="4" Caption="Contact" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList"  />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="RelationshipType" VisibleIndex="4" Caption="Type" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList"  />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="CostCodeLookup" VisibleIndex="4" Caption="Cost Codes" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList"  />
                                </dx:GridViewDataColumn>

                            </Columns>
                            <settingscommandbutton>
                              <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                 <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                            </settingscommandbutton>
                            <Templates>
                                <GroupRowContent>
                                    <table>
                                        <tr>
                                            <td>
                                                <a id="aContact" runat="server" style="cursor: pointer" href=""></a>

                                            </td>
                                        </tr>
                                    </table>
                                </GroupRowContent>
                            </Templates>
                            <Settings ShowFooter="false" ShowHeaderFilterButton="false" />
                            <SettingsBehavior AllowSort="false"  AllowDragDrop="false" AutoExpandAllGroups="true"  />
                            <SettingsPopup>
                                <HeaderFilter Height="200" />
                            </SettingsPopup>
                           
                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                                <Row HorizontalAlign="Center" CssClass="CRMstatusGrid_row"></Row>
                                <GroupRow Font-Bold="true"></GroupRow>
                                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
                                <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                            </Styles>
                        </dx:ASPxGridView>
                        <script type="text/javascript">
                            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                UpdateGridHeight();
                            });
                            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                UpdateGridHeight();
                            });
                    </script>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>
        </div>
    </div>

    <div class="row" style="margin-top:15px;">
        <div class="addCompany-link">
            <a id="aAddItem" runat="server" href="#" class="addComLink">
                <img src="/Content/images/plus-blue-new.png" />
                <span>Add Company</span>
            </a>

        <%--    <div style="display: none;">
                <asp:LinkButton ID="lnkSyncProjectDirectory" runat="server" Text="Sync Project Directory" OnClick="lnkSyncProjectDirectory_Click">
                   <img src="/_layouts/15/Images/uGovernIT/add_icon.png"  style="padding-left:5px;"/>
                   Sync Project Directory
                </asp:LinkButton>
            </div>--%>


         <%--   <asp:LinkButton ID="lnkSyncFromCallback" runat="server" Text="Sync Project Directory" OnClientClick="return OnSyncProjectDirectoryClick();">
               <img src="/_layouts/15/Images/uGovernIT/add_icon.png"  style="padding-left:5px;"/>
               Sync Project Directory
            </asp:LinkButton>--%>

        </div>
    </div>

</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMProjectAllocationView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.CRMProjectAllocationView" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="ugit" TagName="SaveAllocationAsTemplate" Src="~/ControlTemplates/RMM/SaveAllocationAsTemplate.ascx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #header {
        text-align: left;
        /*height: 30px;*/
        float: left;
        padding: 0px 2px;
    }

    #content {
        width: 100%;
    }

    a, img {
        border: 0px;
    }

        a:hover {
            text-decoration: underline;
        }

    .aAddItem_Top {
        padding-left: 10px;
    }

    .dxgvHeader_CustomMaterial {
    }
</style>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openDialog(path, params, titleVal) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '600px', '480px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function openReAllocationDialog(path, params, titleVal) {
        window.parent.UgitOpenPopupDialog(path, params + "ModeType=ReAllocation", titleVal, '600px', '480px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function OpenFindingResource(url) {
        var StartDate = '<%=projectStartDate %>';
        var EndDate = '<%=projectEndDate %>';
        var ticketid = '<%=ticketID%>'
        if (StartDate != null && StartDate != "" && EndDate != null && EndDate != "") {

            if (Date.parse(EndDate) > Date.parse(StartDate)) {
                window.parent.UgitOpenPopupDialog(url, "", "Project Allocation for " + ticketid, 25, 60);
            }
            else
                alert("Invaild Dates");
        }
        else
            alert("Project Start and End dates are required.");

        return false;
    }

    function OpenFindProjectRole(url) {
        window.parent.UgitOpenPopupDialog(url, "", "Project Role", 55, 70);
    }

    //function OpenProjectExternalTeam(url) {
    //    window.parent.UgitOpenPopupDialog(url, "", "Project External Team", 90, 60);
    //}
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grdAllocation.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grdAllocation.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<asp:Panel ID="pnlContainer" runat="server" ScrollBars="Auto">
    <div id="content">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="row">
                <div>
                    <div style="width: 100%; float: right;">

                        <dx:ASPxGridView ID="grdAllocation" ClientInstanceName="grdAllocation" AutoGenerateColumns="False" runat="server" OnHtmlRowPrepared="grdAllocation_HtmlRowPrepared" OnHtmlDataCellPrepared="grdAllocation_HtmlDataCellPrepared"
                            SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="100%" CssClass="customgridview homeGrid" Theme="CustomMaterial" UseFixedTableLayout="false" ShowHorizontalScrollBar="true" EnableRowsCache="true">
                            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                            <Columns>

                                <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0">
                                    <DataItemTemplate>
                                        <img id="reAllocationLink" runat="server" src="/Content/Images/Re Assign.png" alt="ReAllocation" style="padding-right: 0px; width: 12px; height: 12px;" />
                                    </DataItemTemplate>
                                    <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0">
                                    <DataItemTemplate>
                                        <img id="editLink" runat="server" src="~/Content/Images/editNewIcon.png" alt="Edit" style="padding-right: 0px; width: 12px; height: 12px;" height="30" />
                                    </DataItemTemplate>
                                    <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn FieldName="ItemOrder" Visible="False" Caption="&nbsp;&nbsp;#" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataTextColumn FieldName="Title" Caption="Sub Project" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewDataColumn FieldName="AssignedTo" Caption="User" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataColumn FieldName="Type" Caption="Role" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataTextColumn Caption="Start Date" FieldName="AllocationStartDate" PropertiesTextEdit-DisplayFormatString="MMM-d-yyyy"></dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="End Date" FieldName="AllocationEndDate" PropertiesTextEdit-DisplayFormatString="MMM-d-yyyy"></dx:GridViewDataTextColumn>

                                <dx:GridViewDataColumn FieldName="PctAllocation" Caption="Alloc %" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <Settings HeaderFilterMode="CheckedList" />
                                </dx:GridViewDataColumn>

                            </Columns>
                            <SettingsCommandButton>
                                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                            </SettingsCommandButton>
                            <Settings ShowFooter="false" ShowHeaderFilterButton="false" />
                            <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
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
                                <%--<Footer Font-Bold="true" HorizontalAlign="Center"></Footer>--%>
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
                </div>
            </div>
            <div class="row">&nbsp;</div>
            <div class="row">
                <div class="projectTeam_linkWrap">
                    <a id="aAddItem" runat="server" href="#">
                        <img style="width: 16px;" src="/Content/images/plus-blue.png" />
                        <span style="font: 13px 'Poppins', sans-serif !important; color: #4A90E2;">Assign Internal Project Team</span>
                    </a>

                    <a id="aFindResource" runat="server" href="#" style="padding-left: 10px;">
                        <img style="width: 16px;" src="/Content/images/searchNew.png" />
                        <span style="font: 13px 'Poppins', sans-serif !important; color: #4A90E2;">Find Resource</span>
                    </a>

                    <a id="aProjectRole" runat="server" href="#" style="padding-left: 10px;">
                        <img style="width: 16px;" src="/Content/images/searchNew.png" />
                        <span style="font: 13px 'Poppins', sans-serif !important; color: #4A90E2;">Procore Project Role</span>
                    </a>

                    <a id="aSaveAsTemplate" runat="server" style="padding-left: 10px;" href="javascript:pcSaveAsTemplate.Show();">
                        <img style="width: 16px;" src="/Content/images/saveastemplate.png" />
                        <span style="font: 13px 'Poppins', sans-serif !important; color: #4A90E2">Save as Template</span>
                    </a>

                    <a id="aImportTemplate" runat="server" style="padding-left: 10px;">
                        <img style="width: 16px;" src="/Content/images/importtasks.png" />
                        <span style="font: 13px 'Poppins', sans-serif !important; color: #4A90E2">Import Template</span>
                    </a>
                    <%--<a id="aProjectExternal" runat="server" href="#" style="padding-left:10px;">
                    <img src="/_layouts/15/Images/uGovernIT/search-black.png" />
                    Project External
                </a>--%>
                </div>
            </div>
            <div style="min-height: 40px;">
                &nbsp;
       
            </div>
        </div>
    </div>
</asp:Panel>

<dx:ASPxPopupControl ID="pcSaveAsTemplate"  runat="server" Modal="True" Width="470px"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pcSaveAsTemplate" AllowDragging="true" 
    HeaderText="Save Allocation as Template" CloseOnEscape="false" PopupAnimationType="None" EnableViewState="False" CssClass="unsaved_popUp">
    <ContentCollection>
        <dx:PopupControlContentControl ID="pcccRequestTypeChange" runat="server">
            <ugit:SaveAllocationAsTemplate runat="server" id="saveAsTemplateCtrl"></ugit:SaveAllocationAsTemplate>
        </dx:PopupControlContentControl>
    </ContentCollection>
    <ContentStyle>
        <Paddings PaddingBottom="5px" />
    </ContentStyle>
</dx:ASPxPopupControl>

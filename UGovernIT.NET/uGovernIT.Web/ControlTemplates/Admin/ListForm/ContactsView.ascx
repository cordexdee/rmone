<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactsView.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Admin.ListForm.ContactsView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>


<style data-v="<%=UGITUtility.AssemblyVersion %>">
    #header {
        text-align: left;
        height: 30px;
        /*float: left;*/
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
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openContactDialog(path, params, titleVal) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '600px', '510px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function Contact_SelectionChanged(s, e) {
        if ("<%= ShowSearchOption %>" == "True") {
            $("[id$='btnClose']").get(0).click();
        }
        else {
            CallbackPanel.PerformCallback();
        }
    }

    function ContactRowClick(path, params) {
        params = "ticketID=" + params;
        window.parent.UgitOpenPopupDialog(path, params, 'Contact Activity', '600px', '510px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function UpdateGridHeight() {
            grdContacts.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            grdContacts.SetHeight(containerHeight);
        }
        window.addEventListener('resize', function (evt) {
            if (!ASPxClientUtils.androidPlatform)
                return;
            var activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
        });
</script>

<dx:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="CallbackPanel" OnCallback="CallbackPanel_Callback"
    Width="100%">
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <div id="content" class="col-md-12 col-xs-12 col-sm-12 noPadding">
                    <div id="trSearch" runat="server" class="row">
                        <div>
                            <asp:TextBox ID="txtSearch" runat="server" Visible="false"></asp:TextBox>
                            <div style="display: none;">
                                <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <dx:ASPxGridView ID="grdContacts" ClientInstanceName="grdContacts" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found."
                            KeyFieldName="ID" Width="100%" OnHtmlRowPrepared="grdContacts_HtmlRowPrepared" 
                            CssClass="CRMstatus_gridContainer homeGrid" Settings-VerticalScrollBarMode="Hidden" 
                            OnHtmlDataCellPrepared="grdContacts_HtmlDataCellPrepared">
                            <SettingsText EmptyDataRow="No record found."></SettingsText>
                            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                            <Columns>
                                <dx:GridViewDataTextColumn FieldName="Edit" Caption="&nbsp;">

                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString="" ></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                    <DataItemTemplate>
                                        <img id="editLink" runat="server" src="/Content/Images/editNewIcon.png" alt="Edit" style="float: left; padding-left: 5px; width:20px; margin-top:4px;" />
                                        <%--  <img id="imgDeactive" runat="server" src="/_layouts/15/images/uGovernIT/ButtonImages/reject.png" alt="Actived" style="float:right;padding-right:5px" />
                                        <img id="imgActive" runat="server" src="/_layouts/15/images/uGovernIT/ButtonImages/approve.png" alt="Deactived" style="float:right;padding-right:5px" />
                                    --%> 

                                    </DataItemTemplate>
                                    <Settings AllowAutoFilter="False" AllowSort="False"   AllowHeaderFilter="False" />
                                </dx:GridViewDataTextColumn>

                                <dx:GridViewCommandColumn ShowNewButtonInHeader="false" ShowEditButton="false" VisibleIndex="0" Visible="false" />

                                <dx:GridViewDataColumn FieldName="Title" Caption="Contact Name" CellStyle-HorizontalAlign="Left" 
                                    HeaderStyle-HorizontalAlign="Left">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>

                                    <EditFormSettings  />
                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                </dx:GridViewDataColumn>

                                <%--<dx:GridViewDataColumn FieldName="UGITMiddleName" Visible="false" Caption="Middle Name​">
                                    <EditFormSettings VisibleIndex="1" Visible="True" />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="LastName" Visible="false" Caption="Last Name​">
                                    <EditFormSettings VisibleIndex="2" />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="AddressedAs" Visible="false" Caption="Addressed As">
                                    <EditFormSettings VisibleIndex="4" Visible="True" />
                                </dx:GridViewDataColumn>--%>

                                <dx:GridViewDataColumn FieldName="CRMCompanyLookup" Caption="Company" CellStyle-HorizontalAlign="Left" 
                                    HeaderStyle-HorizontalAlign="Left">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>

                                    <EditFormSettings />

                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="NameTitle" Caption="Title" CellStyle-HorizontalAlign="Left" 
                                    HeaderStyle-HorizontalAlign="Left">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>

                                    <EditFormSettings  Visible="True" />

                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                </dx:GridViewDataColumn>


                                <dx:GridViewDataColumn FieldName="Telephone" Caption="Direct Line">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                    <EditFormSettings  Visible="True" />
                                </dx:GridViewDataColumn>

                                <dx:GridViewDataColumn FieldName="EmailAddress"  Caption="Email Address">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                    <%--<EditFormSettings VisibleIndex="15" Visible="True" />--%>
                                </dx:GridViewDataColumn>



                                <dx:GridViewDataColumn FieldName="TicketStatus" Caption="Status">
                                    <SettingsHeaderFilter>
                                        <DateRangePickerSettings EditFormatString=""></DateRangePickerSettings>
                                    </SettingsHeaderFilter>
                                    <EditFormSettings  Visible="True" />
                                </dx:GridViewDataColumn>

                                    <%--<dx:GridViewDataColumn FieldName="Fax" Visible="false" Caption="Fax">
                                    <EditFormSettings VisibleIndex="17" Visible="True" />
                                </dx:GridViewDataColumn>--%>

                            </Columns>
                            <settingscommandbutton >
                                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn" ></ShowAdaptiveDetailButton>
                                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></HideAdaptiveDetailButton>
                            </settingscommandbutton>
                            <SettingsEditing EditFormColumnCount="3" />
                            <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                            <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" AllowSelectByRowClick="true" 
                                AllowSelectSingleRowOnly="true"  />
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
                            <%-- <ClientSideEvents SelectionChanged="Contact_SelectionChanged" />--%>
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

                    <div class="row">
                        <div style="margin-top:10px;">
                            <a id="aAddItem_Top" style="padding-left: 0px;" runat="server" href="#" class="aAddItem_Top">
                                <img src="/Content/Images/plus-blue.png" style="width:20px;" />
                                <span class="CrmLink_lable">Add New Contact</span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </dx:PanelContent>
    </PanelCollection>

</dx:ASPxCallbackPanel>
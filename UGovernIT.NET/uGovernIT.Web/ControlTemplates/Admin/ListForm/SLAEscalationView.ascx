<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SLAEscalationView.ascx.cs" Inherits="uGovernIT.Web.SLAEscalationView" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function MoveToProduction(obj) {
        var url = '<%=moveToProductionUrl%>';
        window.parent.UgitOpenPopupDialog(url, '', 'Migrate Change(s)', '300px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        _SPGrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        _SPGrid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="margin-top:10px;">
    <div class="row" id="header">
        <div class="formLayout-dropDownWrap col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxComboBox ID="ddlModule" Width="100%" runat="server" AutoPostBack="true" CssClass="aspxComBox-dropDown"
                    OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" ListBoxStyle-CssClass="aspxComboBox-listBox">
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="headerContent-right" style="padding-top: 20px;">
                <div class="headerItem-btnWrap">
                    <asp:Button ID="btnApplyChanges" CssClass="aspBtn-primaryBlue" CausesValidation="false" OnClientClick="LoadingPanel.Show();"
                        OnClick="btnRefreshCache_Click" Text="Apply Changes" runat="server" />
                    <asp:Button ID="btnMigrateSLAEscalation" runat="server" CssClass="aspBtn-primaryBlue" Text="Migrate" OnClientClick="MoveToProduction(this)" Visible="false" />
                </div>
                <div class="headerItem-addItemBtn fright">
                    <a id="aAddItem_Top" runat="server" href="" style="padding-left: 5px" class="primary-btn-link">
                        <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
                        <asp:Label ID="Label1" runat="server" Text="Add New Item"></asp:Label>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="crm-checkWrap" style="margin-bottom:10px;">
            <asp:CheckBox ID="cbEnableEscalation" Text="Enable Escalations" AutoPostBack="true" OnCheckedChanged="cbEnableEscalation_CheckedChanged" runat="server" />
        </div>
    </div>
    <div class="row" id="content">
        <ugit:ASPxGridView ID="_SPGrid" runat="server" CssClass="customgridview homeGrid" Width="100%" OnHtmlRowPrepared="_SPGrid_RowDataBound" AllowGrouping="true"
            KeyFieldName="ID" AutoGenerateColumns="false" ClientInstanceName="_SPGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" SortOrder="Ascending">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="SLA Rule" FieldName="Title">
                    <DataItemTemplate>
                        <a id="aSLARule" runat="server" href=""></a>
                        <asp:HiddenField runat="server" ID="hiddenSLARule" Value='<%#Bind("SLARuleIdLookup") %>' />
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="SLA" PropertiesTextEdit-DisplayFormatString="{0:0.##}" Caption="Escalation After" />
                <dx:GridViewDataTextColumn FieldName="EscalationToRoles" Caption="Escalation To" />
                <dx:GridViewDataTextColumn FieldName="SLAFrequency" Caption="Escalation Frequency" />
                <%--    <asp:BoundField DataField="EscalationToEmails" HeaderText="Escalation To Emails" />--%>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
        </ugit:ASPxGridView>
        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
            ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
            ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                UpdateGridHeight();
            });
        </script>
    </div>
    <div class="row bottom-addBtn">
        <div class="bottom-leftSideBtn">
            <dx:ASPxButton ID="btReGenerateEscalation" Text="Refresh Escalations" runat="server" CssClass="primary-blueBtn" OnClick="btReGenerateEscalation_Click"></dx:ASPxButton>
           <dx:ASPxButton ID="btRefreshingEscalation" runat="server" Text="Refresh Escalations (In Process)" Visible="false" Enabled="false"
                CssClass="primary-blueBtn">
            </dx:ASPxButton>
        </div>
        <div class="headerItem-addItemBtn">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img1" runat="server" src="/Content/images/plus-symbol.png" />
                <asp:Label ID="LblAddItem" runat="server" Text="Add New Item"></asp:Label>
            </a>
        </div>
    </div>
</div>


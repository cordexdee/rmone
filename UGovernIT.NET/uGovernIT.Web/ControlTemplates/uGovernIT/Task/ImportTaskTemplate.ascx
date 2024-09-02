<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportTaskTemplate.ascx.cs" Inherits="uGovernIT.Web.ImportTaskTemplate" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        grid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        grid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function getConfirm(s, e) {

        if ($(".overrideTask input").is(":checked")) {
            if (grid.GetSelectedRowCount() == 0) {
                alert("Please select template to import tasks from");
                e.processOnServer = false;
                return false;
            }
            if (confirm('All existing tasks will be replaced from the imported file.\nThis will also delete ALL ASSIGNED allocations for this project.\n\nAre you sure you want to proceed?')) {
                LoadingPanel.Show();
                //document.getElementById('ImportBox').style.display = 'none'; Commented 28 jan 2020

                return true;
            }
            else {
                e.processOnServer = false;
                return false;
            }
        }
        else {
            if (grid.GetSelectedRowCount() == 0) {
                alert("Please select template to import tasks from");
                e.processOnServer = false;
                return false;
            }
            if (typeof tvTasks != "undefined" && tvTasks.GetSelectedNode() == null) {
                alert("Please select task to import under");
                e.processOnServer = false;
                return false;
            }
            LoadingPanel.Show();
        }

    }
</script>
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait..." ClientInstanceName="LoadingPanel" Modal="True" />
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="ms-formtable accomp-popup ">
        <div class="row">
            <div class="ms-formbody accomp_inputField">
                <div class="bC-radioBtnWrap">
                    <asp:RadioButton Text="Import under existing task" runat="server" ID="importtask" Checked="true" GroupName="importTask"
                        AutoPostBack="true" OnCheckedChanged="overrideTask_CheckedChanged" CssClass="radiobutton importtask" />
                    <asp:RadioButton Text="Replace all existing tasks" runat="server" ID="overrideTask" GroupName="importTask" AutoPostBack="true"
                        OnCheckedChanged="overrideTask_CheckedChanged" CssClass="radiobutton overrideTask" />
                </div>
                <div class="crm-checkWrap">
                    <asp:CheckBox Text="Keep Template Dates" runat="server" ID="chkboxSaveTaskDate" AutoPostBack="true"
                        OnCheckedChanged="SaveTaskDate_CheckedChanged" />
                </div>
                <%--<asp:Label CssClass="warning-message" ForeColor="Red" ID="lbMessage" runat="server"></asp:Label>--%>
            </div>
        </div>

        <div class="row">
            <div class="ms-formbody accomp_inputField">
                <dx:ASPxSplitter ID="splitterImporttask" runat="server" Height="400px">
                    <Panes>
                        <dx:SplitterPane ScrollBars="Auto" Size="40%" Name="Tasks">
                            <ContentCollection>
                                <dx:SplitterContentControl>
                                    <div style="width: 98%; height: 20px; padding: 5px; background-color: lightblue; font-weight: bold;">
                                        Select parent task
                               
                                    </div>
                                    <dx:ASPxTreeView ID="tvTasks" ClientInstanceName="tvTasks" EnableClientSideAPI="true" runat="server" TextField="Title" ShowExpandButtons="true"
                                        ShowTreeLines="true" AllowSelectNode="true">
                                    </dx:ASPxTreeView>
                                </dx:SplitterContentControl>
                            </ContentCollection>
                        </dx:SplitterPane>
                        <dx:SplitterPane Name="Template" ScrollBars="Vertical">
                            <ContentCollection>
                                <dx:SplitterContentControl>
                                    <ugit:ASPxGridView ID="grid" runat="server" EnableClientSideAPI="true" AutoGenerateColumns="False"
                                        ClientInstanceName="grid" CssClass="customgridview homeGrid" Width="100%" KeyFieldName="ID" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared">
                                        <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="Title" Caption="Title" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Name="aEdit">
                                                <DataItemTemplate>
                                                    <a id="editlink" runat="server" href="">
                                                        <img id="Imgedit" style="width: 16px" runat="server" src="~/Content/Images/eyeview.png" />
                                                    </a>
                                                </DataItemTemplate>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsBehavior AutoExpandAllGroups="true" AllowSelectByRowClick="true" AllowSelectSingleRowOnly="true" />
                                        <SettingsPopup>
                                            <HeaderFilter Height="200" />
                                        </SettingsPopup>
                                        <SettingsCommandButton>
                                            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                            <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                        </SettingsCommandButton>
                                        <Styles>
                                            <Row CssClass="homeGrid_dataRow importTask-Grid-DataRow"></Row>
                                            <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                                            <GroupRow CssClass="homeGrid-groupRow" Font-Bold="true"></GroupRow>
                                            <SelectedRow BackColor="#AAD4FF"></SelectedRow>
                                        </Styles>
                                        <SettingsPager Position="Bottom" PageSize="15">
                                            <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                                        </SettingsPager>
                                    </ugit:ASPxGridView>
                                    <script type="text/javascript">
                                        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                            UpdateGridHeight();
                                        });
                                        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                            UpdateGridHeight();
                                        });
                                    </script>
                                </dx:SplitterContentControl>
                            </ContentCollection>
                        </dx:SplitterPane>
                    </Panes>
                </dx:ASPxSplitter>
            </div>
        </div>
        <div class="row addEditPopup-btnWrap">
            <dx:ASPxButton ID="btnCancel" ClientInstanceName="btnCancel" runat="server" Text="Cancel" CssClass="secondary-cancelBtn" OnClick="btnCancel_Click">
                
            </dx:ASPxButton>
            <dx:ASPxButton ValidationGroup="task" ID="btLoadFromTemplate" ClientInstanceName="btLoadFromTemplate" Visible="true" CssClass="primary-blueBtn"
                runat="server" Text="Import" ToolTip="Load From Tempate" OnClick="btLoadFromTemplate_Click">
                <ClientSideEvents Click="getConfirm" />
            </dx:ASPxButton>
        </div>
    </div>
</div>


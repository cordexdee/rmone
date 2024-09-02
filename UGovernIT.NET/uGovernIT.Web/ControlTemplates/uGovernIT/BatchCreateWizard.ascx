<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BatchCreateWizard.ascx.cs" Inherits="uGovernIT.Web.BatchCreateWizard" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function OnAllCheckedChanged(s, e) {
        if (s.GetChecked()) {
            aspxGridviewUser.SelectRows();
        }
        else {
            aspxGridviewUser.UnselectRows();
        }
    }

    $(document).ready(function () {
        $('.fetch-popupParent').parent().addClass('popupUp-mainContainer');
    });
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        aspxGridviewUser.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        aspxGridviewUser.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        aspxGridViewGroup.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        aspxGridViewGroup.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<div class="fetch-popupParent">
    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Always">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImportUserSubmit" />
        </Triggers>
        <ContentTemplate>
            <div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
                <div class="ms-formtable accomp-popup">
                    <div class="row">
                        <div class="ms-formlabel">
                            <h3 class="ms-standardheader budget_fieldLabel">User Selection Source:</h3>
                        </div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:RadioButton ID="rbGroupList" runat="server" CssClass="bC-radioBtnWrap" Text="Groups" AutoPostBack="true" 
                                GroupName="SelectUserSoruce" Checked="true" />
                            <asp:RadioButton ID="rbUserList" runat="server" CssClass="bC-radioBtnWrap" Text="Users" AutoPostBack="true" 
                                GroupName="SelectUserSoruce" />
                            <asp:RadioButton ID="rbUserImport" runat="server"  CssClass="bC-radioBtnWrap" Text="Import User File" 
                                AutoPostBack="true" GroupName="SelectUserSoruce" />
                        </div>
                    </div>
                    <div class="row" id="trUserImport" runat="server" visible="false">
                        <div class="ms-formbody accomp_inputField">
                            <div style="float: left">
                                <asp:Label ID="lblImportUser" runat="server" Text="Import Users:" Style="padding-right: 30px;"></asp:Label>
                            </div>
                            <div style="float: left">
                                <asp:FileUpload ID="flpImportUser" runat="server" />
                            </div>
                             <div style="float: left">
                                 <dx:ASPxButton ID="btnImportUserSubmit" runat="server" Text="Submit" ToolTip="Submit" ValidationGroup="upload" 
                                     OnClick="btnImportUserSubmit_Click" CssClass="primary-blueBtn">
                                 </dx:ASPxButton>
                                    <%--<asp:Button ID="btnImportUserSubmit1" Text="Submit" ValidationGroup="upload" runat="server" Style="padding-left: 10px;"
                                        OnClick="btnImportUserSubmit_Click" />--%>
                             </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="ms-formbody accomp_inputField">
                             <asp:Label ID="lblFileUpload" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <asp:Label ID="lblMessage" runat="server" Visible="false" Style="text-align: center" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblSelectUser" runat="server" Visible="false"  ForeColor="Red"></asp:Label>
                    </div>
                    <div class="row">
                        <ugit:ASPxGridView ID="aspxGridviewUser" runat="server"  ClientInstanceName="aspxGridviewUser" KeyFieldName="Id" Width="100%" 
                            Images-HeaderActiveFilter-Url="/Content/images/Filter_Red_16.png" CssClass="customgridview homeGrid"
                            OnCustomColumnDisplayText="aspxGridviewUser_CustomColumnDisplayText" OnDataBinding="aspxGridviewUser_DataBinding" 
                            Settings-VerticalScrollBarMode="Auto">
                            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                                <Columns>
                                    <dx:GridViewCommandColumn Caption="" ShowSelectCheckbox="true">
                                        <HeaderTemplate>
                                            <dx:ASPxCheckBox ID="cbAll" runat="server" ClientInstanceName="cbAll" OnInit="cbAll_Init" ToolTip="Select all rows">
                                                <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                                            </dx:ASPxCheckBox>
                                        </HeaderTemplate>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataColumn FieldName="Id" Caption="Id" Visible="false" />
                                    <dx:GridViewDataTextColumn FieldName="Title" Caption="Name" PropertiesTextEdit-EncodeHtml="false" 
                                        Settings-FilterMode="Value" Settings-HeaderFilterMode="CheckedList">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="Job" Caption="Title" Settings-HeaderFilterMode="CheckedList" />
                                    <dx:GridViewDataTextColumn FieldName="Manager" Caption="Manager Name" PropertiesTextEdit-EncodeHtml="false" 
                                        Settings-FilterMode="Value" Settings-HeaderFilterMode="CheckedList" />
                                    <dx:GridViewDataTextColumn FieldName="IT" Caption="IT" Settings-HeaderFilterMode="CheckedList" />
                                    <dx:GridViewDataTextColumn FieldName="Consultant" Caption="Cons" Settings-HeaderFilterMode="CheckedList" />
                                    <dx:GridViewDataTextColumn FieldName="IsManager" Caption="Mgr" Settings-HeaderFilterMode="CheckedList" />
                                    <dx:GridViewDataTextColumn FieldName="Department" Caption="Department" Settings-HeaderFilterMode="CheckedList" />
                                    <dx:GridViewDataTextColumn FieldName="Location" Caption="Location" Settings-HeaderFilterMode="CheckedList" />
                                </Columns>
                                <settingscommandbutton>
                                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                                </settingscommandbutton>
                                <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowFilterRow="false" ShowFilterBar="Auto" ShowFooter="false" 
                                    ShowGroupPanel="false" />
                                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                                <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Auto"></Settings>
                                <SettingsText EmptyDataRow="There are no items to show in this view." />
                                <SettingsPopup>
                                    <HeaderFilter Height="200" />
                                </SettingsPopup>
                                <Styles>
                                    <AlternatingRow Enabled="True" />
                                    <Header Font-Bold="true" CssClass="homeGrid_headerColumn" />
                                    <Row CssClass="homeGrid_dataRo"></Row>
                                </Styles>
                        </ugit:ASPxGridView>
                        <ugit:ASPxGridView ID="aspxGridViewGroup" Visible="false" Paddings-Padding="0" runat="server" KeyFieldName="Group" 
                            AutoGenerateColumns="false" ClientInstanceName="aspxGridViewGroup" Width="100%" CssClass="customgridview homeGrid">
                            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                            <Columns>
                                <dx:GridViewCommandColumn ShowSelectCheckbox="true"  Caption="">
                                        
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="Group" Caption="Group Name" SortIndex="1" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                            </Columns>
                            <settingscommandbutton>
                                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                            </settingscommandbutton>
                            <Settings ShowFilterRowMenu="true" ShowHeaderFilterButton="true" ShowFilterRow="false" ShowFilterBar="Auto" ShowFooter="false" ShowGroupPanel="false" />
                            <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                            <Settings VerticalScrollableHeight="300" VerticalScrollBarMode="Auto"></Settings>
                            <SettingsBehavior AllowSelectSingleRowOnly="true" />
                            <SettingsText EmptyDataRow="There are no items to show in this view." />
                            <SettingsPopup>
                                <HeaderFilter Height="200" />
                            </SettingsPopup>
                            <Styles>
                                <AlternatingRow Enabled="True" />
                                <Row CssClass="homeGrid_dataRow"></Row>
                                <Header Font-Bold="true" CssClass="homeGrid_headerColumn" />
                            </Styles>
                        </ugit:ASPxGridView>
                        <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                            try {
                                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                                    UpdateGridHeight();
                                });
                                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                                    UpdateGridHeight();
                                });
                            } catch (ex) {}
                        </script>
                    </div>
                    <div class="row addEditPopup-btnWrap" id="divbatchcreate" runat="server">
                        <dx:ASPxButton ID="btnBatchCreateFinish" ClientInstanceName="btnBatchCreateFinish" runat="server" Text="Create"
                            OnClick="btnBatchCreateFinish_Click" CssClass="primary-blueBtn">
                        </dx:ASPxButton>
                    </div>
                    <asp:HiddenField ID="hdnFileName" runat="server" />
                    <asp:HiddenField ID="hdnSellectAll" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

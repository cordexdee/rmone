<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailNotificationView.ascx.cs" Inherits="uGovernIT.Web.EmailNotificationView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    $(function () {
        var hdnAduserProperty = $('[id$="hdnAdUserProperty"]').val();
        if (hdnAduserProperty == "Yes") {
            adUserLoading.Show();
        }
        else {
            adUserLoading.Hide();
        }
    });

    function UpdateGridHeight() {
        emaildxgrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        emaildxgrid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<dx:ASPxLoadingPanel ID="adUserLoading" ClientInstanceName="adUserLoading" Modal="True" runat="server" Text="Please Wait..."></dx:ASPxLoadingPanel>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" style="margin-top:10px;">
    <div class="row">
        <div class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-5 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField" style="padding-top:7px;">
                <dx:ASPxComboBox ID="dxddlModule" runat="server" AutoPostBack="true" Width="100%" CssClass="aspxComBox-dropDown"
                    OnSelectedIndexChanged="dxddlmodule_SelectedIndexChanged" ListBoxStyle-CssClass="aspxComboBox-listBox">
                </dx:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-7 noPadding">
            <div class="headerItem-showChkBox">
                <div class="crm-checkWrap">
                    <asp:CheckBox ID="dxShowDeleted" Text="Enable Notifications (Global)" runat="server" AutoPostBack="true" OnCheckedChanged="dxShowDeleted_CheckedChanged" />
                </div>
            </div>
            <div class="headerContent-right">
                <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApplyChanges_Click"></dx:ASPxButton>
                <div class="headerItem-addItemBtn">
                    <a id="aAddItem_Top" runat="server" href=""  class="primary-btn-link">
                        <img id="Img3" runat="server" src="/content/images/plus-symbol.png" width="16" style="margin-bottom: 2px" />
                        <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        
        <ugit:ASPxGridView ID="dxgrid" runat="server" Width="100%" CssClass="customgridview homeGrid" OnHtmlDataCellPrepared="dxgrid_HtmlDataCellPrepared" KeyFieldName="ID"
            AutoGenerateColumns="false" ClientInstanceName="emaildxgrid"  >
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit1" runat="server" src="/Content/Images/editNewIcon.png" width="16" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Status" Caption="Title">
                    <DataItemTemplate>
                        <a id="editLink" href="" runat="server"></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EmailTitle" Caption="Email Title" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Stage" Caption="Module Step" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EmailEventType" Caption="Email Event Type" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Priority" Caption="Priority" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EmailUserTypes" Caption="User Type" CellStyle-HorizontalAlign="Left"></dx:GridViewDataTextColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Header CssClass="homeGrid_headerColumn"></Header>
                <Row CssClass="homeGrid_dataRow"></Row>
            </Styles>
            <SettingsPager PageSize="20"></SettingsPager>
            <Settings ShowHeaderFilterButton="true"  />
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
        <div class="headerItem-addItemBtn">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img4" runat="server" src="/content/images/plus-symbol.png" />
                <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>


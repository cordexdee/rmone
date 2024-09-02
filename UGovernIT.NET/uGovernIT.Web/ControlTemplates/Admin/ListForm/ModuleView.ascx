<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleView.ascx.cs" Inherits="uGovernIT.Web.ModuleView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    //loadmodule()
    //function Popup() {

    //    popup.SetContentUrl('http://win-mxdel006:2014/sites/ugovernit/Layouts/uGovernIT/DelegateControl.aspx?control=moduleedit&ID=16&module=False');
    //    popup.Show();
    //}
    function openDialog(path, params, titleVal) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '600px', '510px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }


</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        dxModuleView.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight) {
            containerHeight = document.body.scrollHeight;
            dxModuleView.SetHeight(containerHeight);
        }
    }

    try {
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
    } catch (e) {
    }    
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row modules-linkWrap">
        <%--<div class="chkBoxWrap-dis">
            <dx:ASPxCheckBox ID="dxShowDisabled" Text="Show Disabled" TextAlign="Left" runat="server" OnCheckedChanged="dxShowDisabled_CheckedChanged"  
                AutoPostBack="true"></dx:ASPxCheckBox>
        </div>--%>
        <div class="crm-checkWrap2 crm-checkWrap">
            <asp:CheckBox ID="dxShowDisabled" Text="Show Disabled" runat="server" AutoPostBack="true" OnCheckedChanged="dxShowDisabled_CheckedChanged" />
        </div>
        <div class="moduleAdd-itemWrap">
            <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView runat="server" ID="dxModuleView" ClientInstanceName="dxModuleView" CssClass="customgridview homeGrid" OnHtmlDataCellPrepared="dxModuleView_HtmlDataCellPrepared"
            Width="100%" KeyFieldName="ID" AutoGenerateColumns="false">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit1" runat="server" src="~/Content/Images/editNewIcon.png" style="width: 16px;" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="#" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Title" FieldName="Title">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ModuleName" Caption="Name" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ModuleType" Caption="Module Type" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ShortName" Caption="ShortName" SortOrder="ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="CategoryName" Caption="Category Name" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="EnableEventReceivers" PropertiesTextEdit-DisplayFormatString="Yes;No" CellStyle-HorizontalAlign="Center"
                    HeaderStyle-HorizontalAlign="Center" Caption="Enable User Statistics">
                </dx:GridViewDataTextColumn>
                <%--<dx:GridViewDataTextColumn FieldName="ModuleTicketTable" Caption="Ticket Table" SortOrder="Ascending"></dx:GridViewDataTextColumn>--%>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Settings ShowHeaderFilterButton="true" />
            <SettingsPager Mode="ShowAllRecords">
                <PageSizeItemSettings Items="15" />
            </SettingsPager>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[EnableModule]= 0" Format="Custom" RowStyle-CssClass="formatcolor" ApplyToRow="true" />
            </FormatConditions>
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div style="float: right; padding: 15px 0 20px;">
            <a id="aAddItem" runat="server" href="" class="primary-btn-link">
                <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png" />
                <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
    </div>
</div>

 
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EnvironmentView.ascx.cs" Inherits="uGovernIT.Web.EnvironmentView" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        _gridView.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        _gridView.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>

<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap" id="content" style="padding-top:10px;">
    <div class="row">
        <div class="col-md-4 col-sm-4 col-xs-6 crm-checkWrap noPadding">
            <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted" runat="server" TextAlign="Left" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged"/>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-6 noPadding">
            <div style="float: right;">
                <a id="addItemTop" runat="server" href="" class="primary-btn-link">
                    <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label1" runat="server" Text="Add New Item"></asp:Label>
                </a>
            </div>
        </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="_gridView" runat="server" Width="100%" EnableViewState="false" CssClass="customgridview homeGrid"  AutoGenerateColumns="false" AllowFiltering="true"
            KeyFieldName="ID" OnHtmlRowCreated="_gridView_HtmlRowCreated" GridLines="None" EnableCallBacks="false" ClientInstanceName="_gridView" >
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Columns>
                <dx:GridViewDataColumn FieldName="ID" Visible="false"></dx:GridViewDataColumn>
                <dx:GridViewDataColumn>
                    <DataItemTemplate>
                        <dx:ASPxButton ID="aEdit" runat="server" RenderMode="Link" Image-Width="16" Image-Url="~/Content/Images/editNewIcon.png">
                        </dx:ASPxButton>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Title" Caption="Title">
                    <DataItemTemplate>
                        <dx:ASPxButton ID="lblTitle" runat="server" RenderMode="Link"></dx:ASPxButton>
                    </DataItemTemplate>
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="Description" Caption="Description"></dx:GridViewDataColumn>
            </Columns>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Expression="[Deleted]=True" ApplyToRow="true" Format="Custom" RowStyle-CssClass="formatcolor"></dx:GridViewFormatConditionHighlight> 
            </FormatConditions>
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
    <div class="row" style="padding-top:10px;">
         <a id="addItem" runat="server" href="" class="primary-btn-link">
            <img id="Img11" runat="server" src="/Content/Images/plus-symbol.png" />
            <asp:Label ID="LblAddItem1" runat="server" Text="Add New Item"></asp:Label>
         </a>
    </div>
</div>

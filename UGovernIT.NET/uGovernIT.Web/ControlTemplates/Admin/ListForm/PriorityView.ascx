<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriorityView.ascx.cs" Inherits="uGovernIT.Web.PriorityView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function changeRequestType() {
        var moduleid = dxddlModule.GetValue();
        dxgridPriority.PerformCallback('ValueChanged~' + moduleid);
    }
    function UpdateGridHeight() {
        dxgridPriority.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        dxgridPriority.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row" id="header" style="margin-top:10px;">
        <div class="formLayout-dropDownWrap col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:LookUpValueBox ID="dxddlModule" ClientInstanceName="dxddlModule" runat="server" CssClass="lookupValueBox-dropown" FieldName="ModuleNameLookup" FilterExpression="EnableModule=1"  />
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-6 noPadding">
            <div class="headerContent-right">
                <div class="headerItem-addItemBtn" style="padding-top: 25px;">
                    <dx:ASPxButton ID="aAddItem_Top" runat="server" CssClass="primary-blueBtn" Text="Add New Item" AutoPostBack="false" 
                        Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png"></dx:ASPxButton>
                </div>
                
                <div class="menuNav-applyChngBtn">
                    <dx:ASPxButton ID="btnApplyChanges" CausesValidation="false" Text="Apply Changes" runat="server" OnClick="btnApplyChanges_Click"
                        CssClass="primary-blueBtn">
                    </dx:ASPxButton>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="headerItem-showChkBox" style="margin-bottom:10px;">
            <div class="crm-checkWrap">
                <asp:CheckBox ID="dxshowdeleted" Text="Show Deleted" runat="server" AutoPostBack="true" OnCheckedChanged="dxShowDeleted_CheckedChanged" />
            </div>
        </div>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="dxgridPriority" ClientInstanceName="dxgridPriority" runat="server" OnHtmlDataCellPrepared="dxgridPriority_HtmlDataCellPrepared" KeyFieldName="ID"
            Width="100%" AutoGenerateColumns="false" OnCustomCallback="dxgridPriority_CustomCallback" CssClass="customgridview homeGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" width="16" src="~/Content/Images/editNewIcon.png" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Color">
                    <DataItemTemplate>
                        <a id="aaa" runat="server" href="#" style="height: 20px; text-decoration: none; width: 30px;"><%#Eval("Color")%></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="Item Order" CellStyle-HorizontalAlign="Center" Width="100px" SortOrder="Ascending"></dx:GridViewDataTextColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
            <Settings ShowHeaderFilterButton="true" />
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
    <div class="row bottom-addBtn">
        <div class="headerItem-addItemBtn">
            <dx:ASPxButton ID="aAddItem" runat="server" CssClass="primary-blueBtn" Text="Add New Item" AutoPostBack="false" 
                Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png"></dx:ASPxButton>
        </div>
    </div>
</div>


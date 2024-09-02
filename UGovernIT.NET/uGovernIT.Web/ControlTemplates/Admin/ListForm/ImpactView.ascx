<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpactView.ascx.cs" Inherits="uGovernIT.Web.ImpactView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.StaticMenuStyle a {
        border-width: 4px;
        font: menu 16px arial;
        height: 0;
        padding: 2px 40px;
        text-align: center;
        width: auto;
    }

    #header {
        text-align: center;
        /*height: 30px;
        float: left;
        padding: 0px 2px;
    }

    /*#content {
        width: 100%;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }

    a:hover {
        text-decoration: underline;
    }

    a, img {
        border: 0px;
    }

    .formatcolor {
        background-color: #f85752;
        color: white;
    }

        .formatcolor a {
            color: white;
        }*/
</style>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function changeRequestType() {
        var moduleid = ddlModule.GetValue();
        _gridImpact.PerformCallback('ValueChanged~' + moduleid);
    }

    function UpdateGridHeight() {
        _gridImpact.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        _gridImpact.SetHeight(containerHeight);
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
        <div class="formLayout-dropDownWrap col-md-4 col-sm-4 col-xs-5 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <ugit:LookUpValueBox ID="ddlModule" runat="server" CssClass="lookupValueBox-dropown" FieldName="ModuleNameLookup" ClientInstanceName="ddlModule" Width="243px"
                    FilterExpression="EnableModule=1" />
            </div>
        </div>
        <div class="col-md-8 col-sm-8 col-xs-7 noPadding">
            <div class="headerContent-right">
                <div class="headerItem-addItemBtn" style="padding-top: 25px;">
                    <dx:ASPxButton ID="aAddItem_Top" runat="server" CssClass="primary-blueBtn" Text="Add New Item" AutoPostBack="false"
                        Image-Width="12" Image-Url="~/Content/Images/plus-symbol.png">
                    </dx:ASPxButton>
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
                <asp:CheckBox ID="chkShowDeleted" Text="Show Deleted" runat="server" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged" />
            </div>
        </div>
    </div>
    <div class="row" id="content">
        <asp:HiddenField ID="hdnMode" runat="server" Value="Impact" />
        <ugit:ASPxGridView ID="_gridImpact" ClientInstanceName="_gridImpact" runat="server" Width="100%" KeyFieldName="ID" EnableViewState="false"
            AutoGenerateColumns="false" AllowFiltering="true" CssClass="customgridview homeGrid"
            DataKeyNames="ID" OnHtmlRowPrepared="_gridImpact_HtmlRowPrepared" OnHtmlDataCellPrepared="_gridImpact_HtmlDataCellPrepared"
            GridLines="None" OnCustomCallback="_gridImpact_CustomCallback">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Name="aEdit" Width="10px">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href="">
                            <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                        </a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn FieldName="Title" Caption="Title">
                    <DataItemTemplate>
                        <a id="editLink" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataColumn Visible="false">
                    <DataItemTemplate>
                        <a id="aImpact" runat="server" href=""></a>
                        <asp:HiddenField runat="server" ID="hiddenImpact" Value='<%#Bind("MyTitle") %>' />
                    </DataItemTemplate>
                </dx:GridViewDataColumn>

                <dx:GridViewDataColumn FieldName="ItemOrder" Caption="Item Order"></dx:GridViewDataColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
            </Styles>
            <FormatConditions>
                <dx:GridViewFormatConditionHighlight FieldName="Title" Format="Custom" ApplyToRow="true" RowStyle-CssClass="formatcolor" Expression="[Deleted] = True"></dx:GridViewFormatConditionHighlight>
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

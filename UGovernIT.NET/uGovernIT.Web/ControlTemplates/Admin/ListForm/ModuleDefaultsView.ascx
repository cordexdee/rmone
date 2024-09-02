
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleDefaultsView.ascx.cs" Inherits="uGovernIT.Web.ModuleDefaultsView" %>
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
        text-align: left;
        /*height: 30px;
        float: left;
        padding: 0px 2px;
    }

    #content {
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
    }*/
</style>


 <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
     function openDialog(path, params, titleVal, width, height, stopRefresh) {
         window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, 0);
     }
</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        ASPxGridViewAdminDefault.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        ASPxGridViewAdminDefault.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
    function expandAllTask() {
        ASPxGridViewAdminDefault.ExpandAll();
    }
    function collapseAllTask() {
        ASPxGridViewAdminDefault.CollapseAll();
    }

</script>

<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row" id="header">
        <div class="col-md-6 col-sm-6 col-xs-12 noPadding ms-formtable accomp-popup">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
            </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                    OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-12 PopupaddItem-linkWrap">
            <a id="aAddItem_Top" runat="server"   href="#" class="primary-btn-link">
                <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" width="16"/>
                <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
            <dx:ASPxButton ID="btnApply" runat="server" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApply_Click" CssClass="primary-blueBtn"></dx:ASPxButton>
            <%--<asp:Button ID="" runat="server" Text="Apply Changes" OnClick="btnApply_Click" CssClass="AspPrimary-blueBtn" />--%>
        </div>
    </div>
    <div class="row" id="content">
       <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
           <div style="margin-top: 7px">
            <img src="/content/images/expand-all-new.png" title="Expand All" onclick="expandAllTask()" width="16" />
            <img onclick="collapseAllTask()" src="/content/images/collapse-all-new.png" title="Collapse All" width="16" />
        </div>
           <ugit:ASPxGridView ID="ASPxGridViewAdminDefault" AutoGenerateColumns="False" runat="server" SettingsText-EmptyDataRow="No record found." CssClass="customgridview homeGrid"
                KeyFieldName="ID" Width="100%" ClientInstanceName="ASPxGridViewAdminDefault" OnHtmlRowPrepared="ASPxGridViewAdminDefault_HtmlRowPrepared">
               <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0">
                        <DataItemTemplate>
                            <img  alt="Edit" style="float: right;" runat="server" src="~/Content/Images/editNewIcon.png" width="16" />
                        </DataItemTemplate>
                        <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                    </dx:GridViewDataTextColumn>

                <%--     <dx:GridViewDataDateColumn FieldName="Title" VisibleIndex="1" Caption="Title" CellStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataDateColumn>--%>

                    <dx:GridViewDataDateColumn FieldName="ModuleNameLookup" VisibleIndex="2" Caption="Module" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataColumn FieldName="KeyName" VisibleIndex="3" Caption="Field" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="KeyValue" VisibleIndex="4" Caption="Key Value" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="ModuleStageName" VisibleIndex="5" Caption="Stage"  CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" GroupIndex="0">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="CustomProperties" VisibleIndex="6" Caption="Custom Properties" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                </Columns>
               <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <Settings ShowFooter="false" ShowHeaderFilterButton="true" GroupFormat="{0} {1}" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords" ></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <GroupRow CssClass="homeGrid-groupRow" Font-Bold="true"></GroupRow>
                    <Header Font-Bold="true" CssClass="homeGrid_headerColumn"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
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
    </div>
    <div class="row popupBottom-addLinkWrap">
        <a id="aAddItem" runat="server" href="#" class="primary-btn-link">
            <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" width="16" />
            <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"> </asp:Label>
        </a>
    </div>
</div>
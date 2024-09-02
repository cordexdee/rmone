<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleColumnsView.ascx.cs" Inherits="uGovernIT.Web.ModuleColumnsView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function InitalizejQuery() {
        $(".sortable").find("tbody").sortable({
            start: function (event, ui) {
                sourceKey = $(ui.item[0]).find("input[type='hidden']").val();
                sourceIndex = ui.item[0].rowIndex;
            }
        });

        $(".sortable").find("tbody").sortable({
            stop: function (event, ui) {
                targetKey = $(ui.item[0]).next().find("input[type='hidden']").val();
                if (sourceIndex != 0) {
                    grid.PerformCallback("DRAGROW|" + sourceKey + '|' + targetKey);
                }
            }
        });
    }
</script>
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
<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
</dx:ASPxLoadingPanel>
 
<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer" style="margin-top:10px;">

    <div class="row" id="header">
        <div class="col-md-3 col-sm-3 col-xs-12 noPadding ms-formtable accomp-popup">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel">Type:</h3>
            </div>
             <div class="ms-formbody accomp_inputField">
                 <dx:ASPxRadioButtonList ID="rdnModuleClassification" AutoPostBack="true" CaptionSettings-VerticalAlign="Middle" 
                    OnSelectedIndexChanged="rdnModuleClassification_SelectedIndexChanged" runat="server" CssClass="requestList-radioListWrap" 
                    CaptionStyle-Font-Bold="true" Border-BorderWidth="0" ClientInstanceName="rdnModuleClassification" 
                    RepeatDirection="Horizontal">
                    <Items>
                        <dx:ListEditItem Text="Module" Value="0" Selected="true" />
                        <dx:ListEditItem Text="Non-Module" Value="1" />
                    </Items>
                </dx:ASPxRadioButtonList>
             </div>
        </div>
        <div class="col-md-3 col-sm-3 col-xs-12 noPadding ms-formtable accomp-popup">
             <div class="ms-formlabel">
                 <h3 class="ms-standardheader budget_fieldLabel">Select Module:</h3>
             </div>
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="itsmDropDownList aspxDropDownList"
                    OnSelectedIndexChanged="ddlModule_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-6 col-sm-6 col-xs-12 noRightPadding AspPrimary-blueBtnWrap">
            <a id="aAddItem_Top" runat="server" class="primary-btn-link" href="">
                <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" width="16" />
                <asp:Label ID="Label1" runat="server" Text="Add New Item"></asp:Label>
            </a>
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn fright" Text="Apply Changes" ToolTip="Apply Changes"  OnClick="btnApplyChanges_Click">
                <ClientSideEvents Click="function(s, e){LoadingPanel.Show();}" />
            </dx:ASPxButton>
          
        </div>
        <%--<div class="col-md-3 col-sm-3 col-xs-12">
            <div class="">
                <asp:Button ID="" runat="server" CssClass="AspPrimary-blueBtn"  Text="Apply Changes" OnClick=""/>
            </div>
        </div>--%>
        <div class="col-md-3 col-sm-3 col-xs-12 PopupaddItem-linkWrap">
            
        </div>
    </div>
    <div class="row" id="content">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
              <ugit:ASPxGridView ID="grid" CssClass="sortable customgridview homeGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                  EnableCallBacks="true" Width="100%" KeyFieldName="ID" OnHtmlRowPrepared="grid_HtmlRowPrepared">
                  <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                    <Columns >
                        <dx:GridViewDataTextColumn Caption=" " FieldName="Change">
                            <DataItemTemplate>
                                <div class="draggable12">
                                    <a id="aEdit" runat="server" href="" onload="aEdit_Load">
                                        <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16"/>
                                    </a>
                                    <input type="hidden" value='<%# Container.KeyValue %>' />
                                </div>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="#" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="FieldSequence" />

                        <dx:GridViewDataTextColumn Caption="Display Name" FieldName="FieldDisplayName">
                            <DataItemTemplate>
                                <a id="aDisplayName" runat="server" href="" onload="aDisplayName_Load" ></a>
                            </DataItemTemplate>
                        </dx:GridViewDataTextColumn>

                        <dx:GridViewDataTextColumn Caption="Field" FieldName="FieldName" />
                        <dx:GridViewDataTextColumn Caption="Type" FieldName="ColumnType" />
                        <dx:GridViewDataTextColumn Caption="Sort" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="SortOrder" />
                        <dx:GridViewDataTextColumn Caption="&#x25B2;" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="IsAscending" />
                        <dx:GridViewDataTextColumn Caption="Display" PropertiesTextEdit-DisplayFormatString="Yes;No" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="IsDisplay" />
                        <dx:GridViewDataTextColumn Caption="Display For Closed " PropertiesTextEdit-DisplayFormatString="Yes;No" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="DisplayForClosed" />
                        <dx:GridViewDataTextColumn Caption="Show In Tabs" FieldName="SelectedTabs" />
                        <dx:GridViewDataTextColumn Caption="Use In Search" FieldName="IsUseInWildCard" PropertiesTextEdit-DisplayFormatString="Yes;No" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                        <dx:GridViewDataTextColumn Caption="Text Alignment" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="TextAlignment" />
                    </Columns>
                    <settingscommandbutton>
                        <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                        <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                    </settingscommandbutton>
                    <Styles>
                        <row CssClass="homeGrid_dataRow"></row>
                        <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                    </Styles>
                    <SettingsBehavior AllowSort="false" AllowGroup="false" />
                    <Settings GridLines="Horizontal" />
                    <SettingsPopup>
                        <HeaderFilter Height="200" />
                    </SettingsPopup>
                    <SettingsPager Mode="ShowAllRecords" Position="TopAndBottom">
                        <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
                    </SettingsPager>
            </ugit:ASPxGridView>
            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
            <dx:ASPxGlobalEvents ID="ge" runat="server">
                <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
            </dx:ASPxGlobalEvents>
        </div>
    </div>
    <div class="row addEditPopup-btnWrap" style="padding-right:0;">
        <a id="aAddItem" runat="server" href="" class="primary-btn-link">
            <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" width="16"/>
            <asp:Label ID="LblAddItem" runat="server" Text="Add New Item"></asp:Label>
        </a>
    </div>
</div>


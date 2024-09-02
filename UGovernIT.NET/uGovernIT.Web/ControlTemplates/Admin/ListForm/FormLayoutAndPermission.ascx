<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormLayoutAndPermission.ascx.cs" Inherits="uGovernIT.Web.FormLayoutAndPermission" %>
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
    }*/

    /*#header {
        text-align: left;
        float: left;
        padding: 0px 2px;
        width: 100%;
    }*/

    /*#content {
        width: 100%;
    }

    .IndentCell {
        padding-left: 20px!important;
    }*/

    .dxmodalSys .dxmodalTableSys.dxpc-contentWrapper .dxpc-content {
        display: block;
    }

    .gridheader {
        height: 20px;
        background-color: #CED8D9;
        text-align: left;
        font-weight: normal;
    }

    /*th {
        font-weight: normal;
    }

    a:hover {
        text-decoration: underline;
    }

    a, img {
        border: 0px;
    }

    .fleft {
        float: left;
    }

    .fright {
        float: right;
    }

    .drag-over {
        background-color: blue;
    }
    legend {
        font-weight:bold;
    }*/

    /*[+][25-10-2023][SANKET][Added for loader]*/
.dxlpLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxcaLoadingPanel_UGITNavyBlueDevEx .dxlp-loadingImage, .dxlpLoadingPanelWithContent_UGITNavyBlueDevEx .dxlp-loadingImage, .dxeImage_UGITNavyBlueDevEx.dxe-loadingImage {
    background-image: url(/Content/Images/ajaxloader.gif);
    height: 32px;
    width: 32px;
}
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var lastEditClicked;
    var lastTabSaved;
    var keydownCode;
    var keyupCode;
    $(document).bind('click', function (e) {
        if ($(e.target).closest('#' + $('.dvText').find('input').attr('id')).length == 0) {
            ResetTab();
        }
    });
    $(document).bind('keypress', function (e) {
        if (e.keyCode == 13) {
            if ($(e.target).closest('#' + $('.dvText').find('input').attr('id')).length != 0) {
                ResetTab();
            }
        }
    });
    $(document).ready(function () {
        InitalizejQuery();
    });


    function ResetTab() {
        for (var ctr = 0; $('.dvText').find('input').length > ctr; ctr++) {
            if ($($('.dvText')[ctr]).css('display') != 'none') {
                if ($($('.dvText')[ctr]).find('input').val().trim() != '') {
                    var title = $($('.dvText')[ctr]).find('input').val().trim();
                    $($('.dvText')[ctr]).parent()[0].innerHTML = title;
                   
                    if (GetTabNameByText(title) == '')
                        
                        CallbackPanel.PerformCallback("ADDTAB|" + title);

                    else
                        CallbackPanel.PerformCallback("RENAMETAB|" + GetTabNameByText(title) + '|' + title);
                    lastTabSaved = title;
                }
                else {
                    if ($($('.dvText')[ctr]).parent().text().trim() == '') {

                        $($('.dvText')[ctr]).parent()[0].innerHTML = $(".dxtc-tab:last a")[0].innerHTML = '<div style="float:left; width:30px;height:15px;cursor:pointer;" onclick="javascript:event.cancelBubble=true; OnAddTab(this)"> <img style="margin-bottom: -1px;" onclick="javascript:event.cancelBubble=true; OnAddTab(this)" src="/content/Images/add_icon.png" id="btn"/></div>'
                                   + '<div id="dvText" class="dvText" style="float:left;display:none;"> <input type="text" onclick="javascript:event.cancelBubble=true;" id="txt_New" /></div>';

                    }
                    else
                        $($('.dvText')[ctr]).parent()[0].innerHTML = $($('.dvText')[ctr]).parent().text().trim();
                }
            }
        }

    }

    function OnEditTab(obj) {
        //$(obj).parent().parent().hide().next().show().find('input').focus();
        //lastEditClicked = $(obj).parent().parent().next().find('input').val();
        //flTabControl.AdjustSize();
        debugger;
        aspxPopupFormTab.Show();
        $('#<%=hndCurrentEditTab.ClientID%>').val(obj);
        aspxPopupFormTab.PerformCallback(obj);
        flTabControl.AdjustSize();
    }

    function OnDeleteTab(obj) {
        if (confirm("Do you really want to delete this tab?")) {
            CallbackPanel.PerformCallback("DELETETAB|" + obj);
        }
        else
            return false;
    }

    function OnAddTab(obj) {
        //$('.dvText').prev().hide().next().show().find('input');
        //flTabControl.AdjustSize();

        aspxPopupFormTab.Show();
        $('#<%=hndCurrentEditTab.ClientID%>').val("");

        $('#<%=txtTabName.ClientID%>').val("");
        $('#<%=txtOrder.ClientID%>').val("")
        $('#<%=chkShowinMobile.ClientID%>').prop('checked', false);
        flTabControl.AdjustSize();
    }

    $(function () {
        CreateNewItemUrl();
        BindActionButtons();
    });

    function BindActionButtons() {
        $(".dxtc-tab, .dxtc-activeTab").hover(
           function () {
               flTabControl.AdjustSize();

               if ($(this).find('input').length == 0 || $(this).find('input').parent().css('display') == 'none') {
                   if ($(this).find('a').css('display') != 'none' && $(this).find('a').find('div').length == 0) {
                       var tbTitle = $(this).find('a').text();
                       if (tbTitle == '') {
                           $(this).find('a')[0].innerHTML = '<div style="float:left; width:30px;height:15px;cursor:pointer;" onclick="javascript:event.cancelBubble=true; OnAddTab(this)"> <img style="margin-bottom: -1px;" onclick="javascript:event.cancelBubble=true; OnAddTab(this)" src="/content/Images/add_icon.png" id="btn"/></div>'
                               + '<div id="dvText" class="dvText" style="float:left;display:none;"> <input type="text" onclick="javascript:event.cancelBubble=true;" id="txt_New" /></div>';
                       }
                       else {

                           var tabId = GetTabNameByText(tbTitle).trim();
                           if (tabId > 0) {
                               $(this).find('a')[0].innerHTML = '<div style="float:left;"><span style="padding-right:5px;">' + tbTitle + '</span>' +
                                                                '<span><img style="margin-bottom: -5px;cursor:pointer;height:16px;" onclick="javascript:event.cancelBubble=true; OnEditTab(' + tabId + ')" src="/Content/Images/editNewIcon.png" id="btn"/><img style="margin-bottom: -6px; margin-left:5px;" onclick="javascript:event.cancelBubble=true; OnDeleteTab(' + tabId + ');" src="/content/Images/close-red.png" id="btnDelete"/>' +
                                                                '</span></div><div id="dvText" class="dvText" style="float:left;display:none;"><input type="text" onclick="javascript:event.cancelBubble=true;" value="' + tbTitle + '" id="txt_' + tabId + '" /></div>';

                           }
                       }
                   }
               }
           }, function () {
               if ($(this).find('input').parent().css('display') == 'none') {
                   if ($(this).find('a').find('span').length > 0)
                       $(this).find('a')[0].innerHTML = $(this).text().trim();
               }
           });


        $($(".dxtc-tab")[$(".dxtc-tab").length - 1]).find('a')[0].innerHTML = '<div style="float:left; width:30px;height:15px;cursor:pointer;" onclick="javascript:event.cancelBubble=true; OnAddTab(this)"><img style="margin-bottom: -1px; width:16px;"   onclick="javascript:event.cancelBubble=true; OnAddTab(this)"  src="/content/Images/plus-blue.png" id="btn"/></div>'
                              + '<div id="dvText" class="dvText" style="float:left;display:none;"> <input type="text" onclick="javascript:event.cancelBubble=true;" id="txt_New" /></div>';
        flTabControl.AdjustSize();
    }

    //Apply sorting to grid row
    function InitalizejQuery() {

        var sourceKey;
        var targetKey;
        var sourceIndex;
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



        // to handle drage and drop on tabs

        $('.dxtc-tab, .dxtc-activeTab').droppable({
            activeClass: "hover",
            tolerance: "pointer",
            drop: function (event, ui) {

                if (ui.draggable.find("input[type='hidden']").length > 0) {
                    if ($(this).text().trim() != '') {
                        var draggingRowKey = ui.draggable.find("input[type='hidden']").val();
                        grid.PerformCallback("DROPONTAB|" + draggingRowKey + '|' + GetTabNameByText(flTabControl.tabs[flTabControl.activeTabIndex].GetText()));
                        this.click();
                    }
                }
                else {

                    if (ui.draggable.text().trim() != '' && GetTabNameByText(ui.draggable.text().trim()) != '0' && $(this).text().trim() != '' && GetTabNameByText($(this).text()) != '0')
                        CallbackPanel.PerformCallback("DRAGTAB|" + GetTabNameByText(ui.draggable.text()) + '|' + GetTabNameByText($(this).text()));
                }
            }
        });

        $('.dxtc-tab, .dxtc-activeTab').draggable({
            helper: 'clone',
            opacity: 1,
            cursor: "move",
            cursorAt: { top: 7, left: 50 },

            drag: function (event, ui) {

                if (ui.helper.css('height') != '25px') {
                    ui.helper.css('height', '25px');
                }
            }
        });

    }

    function GetTabNameByText(val) {
        var name;
        for (var i = 0; flTabControl.tabs.length > i ; i++) {
            if (flTabControl.tabs[i].GetText() == val.trim()) {
                name = flTabControl.tabs[i].name;
                break;
            }
        }
        return name;
    }


    function OnTabClick(s, e) {
        if (e.tab.name != '') {
            $('#<%=hndCurrentTab.ClientID%>').val(GetTabNameByText(e.tab.GetText()));
               CreateNewItemUrl();
               grid.PerformCallback();
           }
       }

       function CreateNewItemUrl() {
           var url = '<%=addNewItemUrl%>';
           url += "&currentTabIndex=" + $('#<%=hndCurrentTab.ClientID%>').val();
           $('#<%=aAddItem.ClientID%>').attr("href", "javascript:UgitOpenPopupDialog('" + url + "','','New Item','800','830',0,'" + encodeURIComponent(window.location.pathname) + "', 'true')");
           $('#<%=aAddItem_Top.ClientID%>').attr("href", "javascript:UgitOpenPopupDialog('" + url + "','','New Item','800','830',0,'" + encodeURIComponent(window.location.pathname) + "','true')");
       }

       function OnEndCallback(s, e) {
           BindActionButtons();
       }


       $(function () {
           var hdn = hdnvalsltedrow.value;

       });

       function MoveToProduction(obj)
       {
           var url = '<%=moveToProductionUrl%>';
           window.parent.UgitOpenPopupDialog(url, '', 'Migrate Change(s)', '300px', '150px', 0, escape("<%= Request.Url.AbsolutePath %>"));
       }
	   
	   function SaveSelectedTabs() {
            var title = $('#<%=txtTabName.ClientID%>').val().trim();
            if (title == "" || title == null) {
                alert("Tab name required!");
                return;
            }
        var tabId = $('#<%=hndCurrentEditTab.ClientID%>').val();
           $('#<%=hndAuthorizedToView.ClientID%>').val(ASPxClientControl.GetControlCollection().GetByName("authorizedToViewLookupSearchValue").GetValue())
            aspxPopupFormTab.Hide();
            if (tabId == '')
                CallbackPanel.PerformCallback("ADDTAB|" + title);
            else
                CallbackPanel.PerformCallback("RENAMETAB|" + tabId + '|' + title);
        }

        function ValidateOrderNo() {
            if ((event.keyCode > 47 && event.keyCode < 58))
                return event.returnValue;
            return event.returnValue = '';
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
<div class="col-md-12 col-sm-12 col-xs-12 configVariable-popupWrap">
    <div class="row formLayout-headerWrap" id="header">
        <div class="col-md-1 col-sm-1 noPadding">
            <div class="ms-formlabel">
                <h3 class="ms-standardheader budget_fieldLabel labelfloating">Select Module:</h3>
            </div>
        </div>
        <div class="formLayout-dropDownWrap col-md-4 col-sm-4" style="float:left!important">
            <div class="ms-formbody accomp_inputField">
                <asp:DropDownList ID="ddlModule" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlModule_SelectedIndexChanged" CssClass="itsmDropDownList aspxDropDownList">
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-md-7 col-sm-7 col-xs-12 noPadding">
            <div class="modules-linkWrap moduleAdd-itemWrap ">
                <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                    <img id="Img2" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="Label1" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
                <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn margin-right10" Text="Apply Changes" ToolTip="Apply Changes" 
                    OnClick="btnApplyChanges_Click"></dx:ASPxButton>
            </div>
            <div class="formLayout-rightHederContent">
                <span>
                    <asp:Button ID="btnMoveStageToProduction" runat="server" Text="Migrate" OnClientClick="MoveToProduction(this)" Visible="false" />
                </span>
            </div>
        </div>
    </div>

    <div class="row formLayout-tabContainer">
        <dx:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="CallbackPanel" Width="100%" CssClass="formLayout-tabControlPannel">
            <PanelCollection>
                <dx:PanelContent runat="server">
                    <dx:ASPxTabControl ID="flTabControl" CssClass="fltabcontrol11 formLayout-tabControl" runat="server" Width="100%" Height="25px"
                        TabStyle-Height="25px" ClientInstanceName="flTabControl" OnActiveTabChanged="flTabControl_ActiveTabChanged">
                        <Tabs>
                        </Tabs>
                        <TabStyle Paddings-PaddingLeft="13px" Paddings-PaddingRight="13px"></TabStyle>
                        <ClientSideEvents TabClick="OnTabClick" />
                    </dx:ASPxTabControl>

                </dx:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="OnEndCallback"></ClientSideEvents>
        </dx:ASPxCallbackPanel>
        <asp:HiddenField ID="hndCurrentTab" Value="1" runat="server" />
		<asp:HiddenField ID="hndCurrentEditTab" Value="1" runat="server" />
		<asp:HiddenField ID="hndAuthorizedToView" Value="" runat="server" />
    </div>

<dx:ASPxPopupControl ID="aspxPopupFormTab" ClientInstanceName="aspxPopupFormTab" Width="500px" Height="150px" Modal="true" OnWindowCallback="aspxPopupFormTab_WindowCallback"  LoadContentViaCallback="OnFirstShow"
    ShowFooter="false" ShowHeader="true" HeaderText="Tab Form" CloseButtonImage-Url="~/Content/Images/close-red-big.png" CssClass="aspxPopup" SettingsAdaptivity-Mode="Always"
    runat="server" EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True"  HeaderStyle-CssClass="modal-header">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <div class="col-md-12 col-sm-12 col-xs-12 accomp-popup noPadding">
                <div class="row">
                    <div class="ms-formlabel">
                        <span class="ms-standardheader budget_fieldLabel">Tab Name:<b style="color: Red;">*</b></span>
                    </div>
                    <div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtTabName" runat="server" ValidationGroup="FormTab"></asp:TextBox>                            
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="ms-formlabel">
                        <span class="ms-standardheader budget_fieldLabel">Order:</span>
                    </div>
                    <div>
                        <div class="ms-formbody accomp_inputField">
                            <asp:TextBox ID="txtOrder" runat="server" onkeypress="return ValidateOrderNo()"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="ms-formlabel">
                        <span class="ms-standardheader budget_fieldLabel">Authorized to View:</span>
                    </div>
                    <div>
                        <div class="ms-formbody accomp_inputField">
                            <%--<ugit:UserValueBox ID="ppeAuthorizedToView" runat="server" isMulti="true"></ugit:UserValueBox>--%>
                            <ugit:UserValueBox ID="authorizedToView" runat="server" CssClass="userValueBox-dropDown" isMulti="true"></ugit:UserValueBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="crm-checkWrap">
                        <asp:CheckBox ID="chkShowinMobile" runat="server" Text="Show in mobile" />
                    </div>
                </div>
            </div>

            <div class="row tabForm-btnWrap">
                <ul>
                    <li runat="server" id="cancelLI" class="secondary-BtnLinkWrap">
                        <asp:HyperLink ID="lnkCancel" runat="server" onClick="aspxPopupFormTab.Hide();"
                            Visible="true" ToolTip="Cancel" Text="Cancel" CssClass="secondary-BtnLink" />
                    </li>
                    <li runat="server" id="saveasTabLI" class="Primary-BtnLinkWrap">
                        <asp:HyperLink ID="lnkSaveAsTab" runat="server" onClick="SaveSelectedTabs(this);" CssClass="Primary-BtnLink" ValidationGroup="FormTab"
                            Visible="true" ToolTip="Save" Text="Save" />
                    </li>

                    
                </ul>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>    
</dx:ASPxPopupControl>

    <div class="row formLayout-gridWrap" id="content">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <ugit:ASPxGridView ID="grid" runat="server"  CssClass="sortable customgridview homeGrid" AutoGenerateColumns="False" OnHtmlRowPrepared="grid_HtmlRowPrepared"
                OnCustomButtonCallback="grid_CustomButtonCallback" ClientInstanceName="grid" EnableCallBacks="true" EnableViewState="false" Width="100%" KeyFieldName="ID">
                <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn Caption=" " FieldName="Change" Width="60px">
                        <DataItemTemplate>
                            <div>
                                <a id="aEdit" runat="server" href="" onload="aEdit_Load" style="float:left;">
                                    <img id="Imgedit" runat="server" src="/Content/Images/editNewIcon.png" width="16"/>
                                </a>
                                <a id="aPermission" runat="server" style="float:left;" href="" onload="aPermission_Load" class="ms-cui-img-16by16 ms-cui-img-cont-float">
                                    <img id="ImgPermission" runat="server" src="/content/Images/permission.png" />
                                </a>
                                <input type="hidden" id="hdnvalsltedrow" value='<%# Container.KeyValue %>' />
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="#" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="FieldSequence" />
                                    
                    <dx:GridViewDataTextColumn Caption="Display Name" FieldName="FieldDisplayName">
                        <PropertiesTextEdit EncodeHtml="false"></PropertiesTextEdit>
                        <DataItemTemplate>
                            <a id="aDisplayName" runat="server" href="" onload="aDisplayName_Load"></a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>

                    <dx:GridViewDataTextColumn Caption="Field" FieldName="FieldName" />
                    <dx:GridViewDataTextColumn Caption="Type" FieldName="ColumnType" />
                    <dx:GridViewDataTextColumn Caption="Display Width" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="FieldDisplayWidth" />
                </Columns>
                 <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <SettingsBehavior AllowSort="false" AutoExpandAllGroups="true" AllowSelectByRowClick="false" />
                <Settings GridLines="Horizontal" />
                <Styles>
                    <Row CssClass="homeGrid_dataRow"></Row>
                    <Header CssClass="homeGrid_headerColumn" Font-Bold="true"></Header>
                </Styles>
                <SettingsCookies Enabled="false" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords" Position="TopAndBottom">
                </SettingsPager>
            </ugit:ASPxGridView>
            <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
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
            <dx:ASPxGlobalEvents ID="ge" runat="server">
                <ClientSideEvents ControlsInitialized="InitalizejQuery" EndCallback="InitalizejQuery" />
            </dx:ASPxGlobalEvents>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="modules-linkWrap moduleAdd-itemWrap">
                <a id="aAddItem" runat="server" href="" class="primary-btn-link" style="float:right">
                    <img id="Img1" runat="server" src="/Content/Images/plus-symbol.png" />
                    <asp:Label ID="LblAddItem" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
                </a>
            </div>
        </div>
    </div>
</div>




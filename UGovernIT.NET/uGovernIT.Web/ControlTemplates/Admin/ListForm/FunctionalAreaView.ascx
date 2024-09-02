<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FunctionalAreaView.ascx.cs" Inherits="uGovernIT.Web.FunctionalAreaView" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        dxgridview.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        dxgridview.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>


<div class="col-md-12 col-sm-12 col-xs-12 formLayout-addPopupContainer">
    <div class="row PopupaddItem-linkWrap">
        <div class="functionalArea-linkWrap">
            <dx:ASPxButton ID="btnApplyChanges" runat="server" CssClass="primary-blueBtn" Text="Apply Changes" ToolTip="Apply Changes" OnClick="btnApplyChanges_Click"></dx:ASPxButton>
            <a id="aAddItem_Top" runat="server" href="" class="primary-btn-link">
                <img id="Img3" runat="server" src="/Content/Images/plus-symbol.png" width="16" />
                <asp:Label ID="Label2" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
            </a>
        </div>
         
    </div>
    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <div class="crm-checkWrap" style="float:right;margin:10px;">
                <asp:CheckBox ID="chkShowDeleted10" Text="Show Deleted" runat="server" TextAlign="Right" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged1" />
              <%--<dx:ASPxCheckBox ID="chkShowDeleted10" Text="Show Deleted" runat="server" TextAlign="right" AutoPostBack="true" OnCheckedChanged="chkShowDeleted_CheckedChanged1">
              </dx:ASPxCheckBox>--%>
            </div>
        </div>
        <div class="col-md-12 col-sm-12 col-xs-12 noPadding">
            <ugit:ASPxGridView ID="dxgridview" Width="100%" runat="server" ClientInstanceName="dxgridview"  KeyFieldName="ID" AutoGenerateColumns="false" CssClass="customgridview homeGrid" 
                OnHtmlDataCellPrepared="dxgridview_HtmlDataCellPrepared" EnableCallBacks="true" OnHeaderFilterFillItems="dxgridview_HeaderFilterFillItems"
                OnCustomColumnDisplayText="dxgridview_CustomColumnDisplayText">
                <SettingsBehavior SortMode="DisplayText" />
                <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn Name="aEdit" SortOrder="Ascending">
                        <DataItemTemplate>
                           <a id="editLink" runat="server" href="">
                               <img id="Imgedit" runat="server" src="~/Content/Images/editNewIcon.png" width="16"/>
                           </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Functional Area" FieldName="Title" >
                       <DataItemTemplate>
                           <a id="editLink" runat="server" href=""> </a>
                        </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="DepartmentLookup" Caption="Department" SortOrder="Ascending" >
                        <Settings FilterMode="DisplayText" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="OwnerUser" Caption="Owner" SortOrder="Ascending">
                        <Settings FilterMode="DisplayText" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="FunctionalAreaDescription" Caption="Functional Area Description" SortOrder="Ascending"></dx:GridViewDataTextColumn>
                  </Columns>
                 <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <Settings ShowHeaderFilterButton="true"/>
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
    </div>
    <div class="row popupBottom-addLinkWrap">
         <a id="aAddItem" runat="server" href="" class="primary-btn-link">
            <img id="Img4" runat="server" src="/Content/Images/plus-symbol.png" width="16" />
            <asp:Label ID="Label3" runat="server" Text="Add New Item" CssClass="phrasesAdd-label"></asp:Label>
        </a>
    </div>
</div>
<div style="float:right;">
    <div style="float:right; width:114px;padding-top:2px">
     
</div>  
   
</div>
<div>
    
</div>
<div style="float:right; padding-top:5px">
   
</div>  

<%--<div id="content">
    <table width="100%" align="left">
        <tr>
            <td align="left">
                <div style="width: 100%; float: right;">
                    <div style="border: 2px solid #CED8D9">--%>
                      <%--<asp:GridView ID="_gridView" runat="server" Width="100%" EnableViewState="false" AlternatingRowStyle-BackColor="WhiteSmoke"
                            HeaderStyle-Height="20px" HeaderStyle-CssClass="gridheader" HeaderStyle-Font-Bold="false"  AutoGenerateColumns="false" AllowFiltering="true"
                            DataKeyNames="ID" OnRowDataBound="_gridView_RowDataBound" GridLines="None">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Height="2">
                                    <ItemTemplate>
                                        <a id="aEdit" runat="server" href="">
                                            <img id="Imgedit" runat="server" src="/Content/images/edit.gif"/>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Functional Area" HeaderStyle-Width="400px">
                                    <ItemTemplate>
                                        <a id="aTitle" runat="server" href=""></a>
                                        <asp:HiddenField runat="server" ID="hiddenTitle" Value='<%#Bind("Title") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DepartmentLookup"  />
                                <asp:BoundField DataField="UGITOwner" HeaderText="Owner" />
                                <asp:BoundField DataField="FunctionalAreaDescription" HeaderText="Functional Area Description"/>
                            </Columns>
                        </asp:GridView>--%>
                  <%--  </div>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                <a id="aAddItem1" runat="server" href="">
                    <img id="Img1" runat="server" src="../Content/images/uGovernIT/add_icon.png" />
                    <asp:Label ID="LblAddItem" runat="server" Text="Add New Item"></asp:Label>
                </a>
            </td>
        </tr>
    </table>

</div>--%>
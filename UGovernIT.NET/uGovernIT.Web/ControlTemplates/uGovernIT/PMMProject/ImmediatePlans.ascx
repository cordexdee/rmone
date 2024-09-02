<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImmediatePlans.ascx.cs" Inherits="uGovernIT.Web.ImmediatePlans" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--Immediate Plans --%>

<script type="text/javascript" id="dxss_ImmediatePlan" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        gridImmediatePlans.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gridImmediatePlans.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });
</script>
<div class="mainblock col-md-12 noPadding">
    <div class="row">
         <asp:Label CssClass="errormessage-block ugitlight1lightest" runat="server" ID="immediatePlansMessage"></asp:Label>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="gridImmediatePlans" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
        OnDataBinding="gridImmediatePlans_DataBinding" OnCustomCallback="gridImmediatePlans_CustomCallback"
        OnHtmlRowPrepared="gridImmediatePlans_HtmlRowPrepared"
        ClientInstanceName="gridImmediatePlans" Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Width="20px" Caption=" " Settings-AllowHeaderFilter="False" 
                    Settings-AllowSort="False" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="Created">
                    <HeaderCaptionTemplate>                        
                         <div onmousedown="return CancelEvent(event)" onmouseup="return CancelEvent(event)">
                             <asp:ImageButton ID="issueAddbtn" OnClientClick=" return newImmediatePlansItem();" runat="server" ImageUrl="~/Content/Images/plus-blue.png" Width="20px" CssClass="statusHeaderButton"/>                       
                        </div>                       
                    </HeaderCaptionTemplate>
                    <DataItemTemplate >
                        <%#  Container.ItemIndex + 1%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="" FieldName="Planned Items" Width="15%">
                     
                    <DataItemTemplate>
                        <a id="aTitle" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
               <%-- <dx:GridViewDataTextColumn Caption="Created On" FieldName="Created" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn Caption="Planned Date" FieldName="EndDate" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Description" PropertiesTextEdit-EncodeHtml="false" FieldName="ProjectNote" Width="10%"/>

                <dx:GridViewDataTextColumn Caption=" " FieldName="ID" Settings-AllowHeaderFilter="False" Settings-AllowSort="False">
                    <HeaderTemplate>                        
                      <div class="crm-checkWrap" style="float:right; width:auto; display:inline-block;">                        
                        <asp:CheckBox ID="cbHeaderShowArchivedImmediate" Text="Show Archived"   runat="server" onClick="plannedCheckArchived(this.id);"  OnInit="cbHeaderShowArchivedImmediate_Init" />
                      </div>
                    </HeaderTemplate>                   
                    <DataItemTemplate>
                        <div>
                             <a id="aArchive" title="Archive" runat="server" href="javascript:" style="float: right">
                                <img id="Img10" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aDelete" title="Delete" visible="false" runat="server" href="javascript:" style="float: right">
                                <img id="Img1" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aUnArchive" title="Unarchive" runat="server" visible="false" href="javascript:" style="float: right">
                                <img id="Img3" runat="server" src="/Content/Images/unarchive.png" />
                            </a>
                            <a id="aMoveToAccomp" title="Move to Accomplishment" runat="server" href="javascript:" style="float: right">
                                <img id="ImgMoveToAccomp" runat="server" src="/Content/Images/newMoveTo.png" class="status_moveToIcon" />
                            </a>
                            <a id="aPlannedItemsEdit" title="Edit" runat="server" href="javascript:" style="float: right; padding-left: 4px; padding-right: 2px;">
                                <img id="Img2" runat="server" width="16" src="/Content/Images/editNewIcon.png" />
                            </a>
                        
                            <input type="hidden" value='<%# Container.KeyValue %>' />
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsCommandButton>
                <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </SettingsCommandButton>
            <Styles>
                <Row CssClass="homeGrid_dataRow"></Row>
                <Header CssClass="homeGrid_headerColumn"></Header>
            </Styles>

            <SettingsBehavior AllowSelectByRowClick="false" AutoExpandAllGroups="true" />
            <SettingsPopup>
                <HeaderFilter Height="200" />
            </SettingsPopup>
            <SettingsPager Position="TopAndBottom">
                <PageSizeItemSettings Items="10, 15, 20, 25, 50, 75, 100" />
            </SettingsPager>
            <Settings ShowHeaderFilterButton="true" GridLines="None" />
            <ClientSideEvents EndCallback="function(s,e){refreshPageAfterCallback();}" />
        </ugit:ASPxGridView>
    </div>
    
   <div class="row">
      <div class="col-md-1 noPadding" style="padding-right:0; margin-top:10px; float:right; display:inline-block;">
          <div style="display:none">
            <asp:LinkButton OnClientClick="return newImmediatePlansItem()" ID="LinkButton1" runat="server" Text="Add New"
            CssClass="aspLinkButton"></asp:LinkButton>
              </div>
        </div>
       <div class="crm-checkWrap" style="float:right; width:auto; display:none; margin-top:15px;">
            <asp:CheckBox ID="cbShowArchivedImmediate" Text="Show Archived" OnLoad="cbShowArchivedImmediate_Load" AutoPostBack="true" runat="server" />
        </div>
   </div>

    
    <script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
    </script>
    
</div>

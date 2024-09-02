<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DecisionLogList.ascx.cs" Inherits="uGovernIT.Web.DecisionLogList" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%-- Decision Log --%>
<script type="text/javascript" id="dxss_DecisionLog" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        try {
            gridDecisionLog.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            gridDecisionLog.SetHeight(containerHeight);
        } catch (e) {

        }
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
         <asp:Label CssClass="errormessage-block ugitlight1lightest" runat="server" ID="issuesMessageDecisionLog"></asp:Label>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="gridDecisionLog" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter="" 
        OnCustomCallback="gridDecisionLog_CustomCallback" ClientInstanceName="gridDecisionLog" Width="100%" KeyFieldName="ID" 
        OnHtmlRowPrepared="gridDecisionLog_HtmlRowPrepared" 
            CssClass="customgridview homeGrid">
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Width="20px" Caption=" " Settings-AllowHeaderFilter="False" 
                    Settings-AllowSort="False" FieldName="" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <HeaderCaptionTemplate>                        
                         <div onmousedown="return CancelEvent(event)" onmouseup="return CancelEvent(event)">
                             <asp:ImageButton ID="issueAddbtn" OnClientClick=" return newDecisionLog();" runat="server" ImageUrl="~/Content/Images/plus-blue.png" Width="20px" CssClass="statusHeaderButton"/>                       
                        </div>                       
                    </HeaderCaptionTemplate>
                    <DataItemTemplate>
                        <%#  Container.ItemIndex + 1%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Decisions" FieldName="Title" Width="15%">
                     
                    <DataItemTemplate>
                        <a id="aTitle" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Release Date" FieldName="ReleaseDate" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Status" FieldName="DecisionStatus" Width="10%"/>

                <dx:GridViewDataTextColumn Caption="Assigned To" FieldName="AssignedToUser" Width="10%">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Decision Maker" FieldName="DecisionMakerUser" Width="10%"/>


                <dx:GridViewDataTextColumn Caption=" " FieldName="ID" Settings-AllowHeaderFilter="False" Settings-AllowSort="False">
                   <HeaderTemplate>                        
                      <div class="crm-checkWrap" style="float:right; width:auto; display:inline-block;">
                        <%--<asp:CheckBox ID="cbHeaderShowArchivedIssues" Text="Show Archived" OnLoad="cbHeaderShowArchivedIssues_Load" OnInit="cbHeaderShowArchivedIssues_Init" AutoPostBack="true" runat="server" OnCheckedChanged="cbHeaderShowArchivedIssues_CheckedChanged"/>--%>
                        <asp:CheckBox ID="chHeaderShowDecisonLogArchive" Text="Show Archived"   runat="server" onClick="decesionCheckArchived(this.id);"  OnInit="chHeaderShowDecisonLogArchive_Init" />
                      </div>
                    </HeaderTemplate>                   

                    <DataItemTemplate>
                        <div>
                            <a id="aDelete" visible="false" title="Delete" runat="server" href="javascript:" style="float: right">
                                <img id="Img7" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aArchive" runat="server" title="Archive" href="javascript:" style="float: right">
                                <img id="Img12" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aUnArchive" runat="server" title="Unarchive" visible="false" href="javascript:" style="float: right">
                                <img id="Img9" runat="server" src="/Content/Images/unarchive.png" />
                            </a>
                            <a id="aEdit" runat="server" title="Edit" href="javascript:" style="float: right;">
                                <img id="Img8" runat="server" width="16" src="/Content/images/editNewIcon.png" class="pmmStatusAcc_editIcon" />
                            </a>
                            
                        </div>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
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
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div class="col-md-1 noPadding" style="padding-right:0; margin-top:10px; float:right; display:inline-block;">
            <div style="display:none">
            <asp:LinkButton OnClientClick="return newDecisionLog()" ID="btDecisionLog" runat="server" Text="Add New"
                CssClass="aspLinkButton"></asp:LinkButton>
                </div>
        </div>
        <div class="crm-checkWrap" style="float:right; width:auto; display:none; margin-top:15px;">
            <asp:CheckBox ID="chShowDecisonLogArchive" AutoPostBack="true" runat="server" Text="Show Archived" />
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

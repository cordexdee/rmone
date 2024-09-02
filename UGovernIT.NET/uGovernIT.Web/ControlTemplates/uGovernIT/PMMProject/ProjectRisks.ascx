<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectRisks.ascx.cs" Inherits="uGovernIT.Web.ProjectRisks" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%--Project Risks --%>

<script type="text/javascript" id="dxss_ProjectRisks" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        try {
            gridRisks.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            gridRisks.SetHeight(containerHeight);
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
        <asp:Label CssClass="errormessage-block ugitlight1lightest" runat="server" ID="Label3"></asp:Label>
    </div>
    
    <div class="row">
         <ugit:ASPxGridView ID="gridRisks" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
        OnDataBinding="gridRisks_DataBinding" OnCustomCallback="gridRisks_CustomCallback" OnHtmlDataCellPrepared="gridRisks_HtmlDataCellPrepared"
        OnHtmlRowPrepared="gridRisks_HtmlRowPrepared" OnHeaderFilterFillItems="gridRisks_HeaderFilterFillItems"
        ClientInstanceName="gridRisks" Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Width="20px" Caption=" " Settings-AllowHeaderFilter="False"
                    Settings-AllowSort="False" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="Created">
                    <HeaderCaptionTemplate>                        
                         <div onmousedown="return CancelEvent(event)" onmouseup="return CancelEvent(event)">
                             <asp:ImageButton ID="issueAddbtn" OnClientClick=" return newRiskItem();" runat="server" ImageUrl="~/Content/Images/plus-blue.png" Width="20px" CssClass="statusHeaderButton"/>                       
                        </div>                       
                    </HeaderCaptionTemplate>   
                    <DataItemTemplate>
                        <%# Container.ItemIndex + 1%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Risks" FieldName="Title" Width="15%">
                          
                    <DataItemTemplate>
                        <a id="aTitle" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Probability" FieldName="RiskProbability" Width="10%">
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Caption="Impact" FieldName="IssueImpact" Width="10%">
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Assigned To" FieldName="AssignedToUser" Width="10%"/>

                <dx:GridViewDataTextColumn Caption="Mitigation Plan" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="False" Settings-AllowSort="False" FieldName="MitigationPlan" Width="10%"/>

                <dx:GridViewDataTextColumn Caption="Contingency Plan" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="False" Settings-AllowSort="False" FieldName="ContingencyPlan" Width="10%"/>

                <dx:GridViewDataTextColumn Caption=" " FieldName="ID" Settings-AllowHeaderFilter="False" Settings-AllowSort="False">
                   <HeaderTemplate>                        
                      <div class="crm-checkWrap" style="float:right; width:auto; display:inline-block;">                       
                        <asp:CheckBox ID="cbHeaderShowArchivedRisks" Text="Show Archived"   runat="server" onClick="riskCheckArchived(this.id);"  OnInit="cbHeaderShowArchivedRisks_Init" />
                      </div>
                    </HeaderTemplate>                   

                    <DataItemTemplate>
                        <div>
                            <a id="aDelete" title="Delete" visible="false" runat="server" href="javascript:" style="float: right">
                                <img id="Img4" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aArchive" title="Archive" runat="server" href="javascript:" style="float: right">
                                <img id="Img11" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                            <a id="aUnArchive" title="Unarchive" visible="false" runat="server" href="javascript:" style="float: right">
                                <img id="Img6" runat="server" src="/Content/Images/unarchive.png" />
                            </a>
                            <a id="aEdit" title="Edit" runat="server" href="javascript:" style="float: right; padding-left: 4px; padding-right: 4px;">
                                <img id="Img5" runat="server" width="16" src="/Content/Images/editNewIcon.png" />
                            </a>
                           
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
            <ClientSideEvents />
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div class="col-md-1 noPadding" style="padding-right:0; margin-top:10px; float:right; display:inline-block;">
            <div style="display:none">
            <asp:LinkButton OnClientClick="return newRiskItem()" ID="LinkButton2" runat="server" Text="Add New"
                CommandName="NewPMMTask" CommandArgument="0#0" CssClass="aspLinkButton"></asp:LinkButton>
                </div>
        </div>
         <div class="crm-checkWrap" style="float:right; width:auto; display:none; margin-top:15px;">
            <asp:CheckBox ID="cbShowArchivedRisks" OnLoad="cbShowArchivedRisks_Load" AutoPostBack="true" runat="server" Text="Show Archived" />
        </div>
    </div>
   
    <script type="text/javascript">
        ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
        ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
            UpdateGridHeight();
        });
    </script>
    
</div>


<div class="readonlyblock">
        <asp:ListView ID="lvReadOnlyRisks" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1">
            <LayoutTemplate>
                <br />
                <table class="ro-table" frame="box" rules="all" width="100%" cellpadding="0" cellspacing="0">
                    <tr class="ro-header" style="border-bottom: 2px solid grey">
                        <th class="ro-padding" width="20px">
                            <b>&nbsp;</b>
                        </th>
                        <th class="ro-padding" style="text-align: left;" width="300px">
                            <b>Risks</b>
                        </th>
                        <th class="ro-padding" style="text-align: center;" width="100px">
                            <b>Probability</b>
                        </th>
                        <th class="ro-padding" style="text-align: center;" width="120px">
                            <b>Impact</b>
                        </th>
                        <th class="ro-padding" style="text-align: center;" width="200px"><b>Assigned To</b></th>
                        <th class="ro-padding" style="text-align: left;">
                            <b>Mitigation Plan</b>
                        </th>
                        <th class="ro-padding" style="text-align: left;">
                            <b>Contingency Plan</b>
                        </th>
                    </tr>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="ro-item">
                    <td class="ro-padding" style="text-align: right;">
                        <%# Container.DataItemIndex +1 %>
                    </td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.Title) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0}",  Eval(Convert.ToString(DatabaseObjects.Columns.RiskProbability))) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0}",  Eval(DatabaseObjects.Columns.IssueImpact)) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;"><%# Eval(DatabaseObjects.Columns.AssignedTo)%></td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.MitigationPlan)%>
                    </td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.ContingencyPlan)%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="ro-alternateitem">
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.Title) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0}",  Eval(Convert.ToString( DatabaseObjects.Columns.RiskProbability))) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0}",  Eval(DatabaseObjects.Columns.IssueImpact)) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;"><%# Eval(DatabaseObjects.Columns.AssignedTo)%></td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.MitigationPlan)%>
                    </td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.ContingencyPlan)%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:ListView>
    </div>
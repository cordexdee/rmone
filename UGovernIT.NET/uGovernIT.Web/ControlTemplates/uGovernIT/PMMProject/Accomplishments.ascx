<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Accomplishments.ascx.cs" Inherits="uGovernIT.Web.Accomplishments" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<%--Accomplishments --%>

<script type="text/javascript" id="dxss_Accomplishments" data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        gridAccomplishment.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        gridAccomplishment.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });

    $(document).ready(function () {
        $('#aArchive').tooltip();
        $('#aEdit').tooltip();
    });
</script>

<div id="mainblock" class="mainblock col-md-12 noPadding" runat="server">
    <div class="row">
        <asp:Label CssClass="errormessage-block ugitlight1lightest" runat="server" ID="accomplishmentMessage"></asp:Label>
    </div>
    <div class="row">
        <ugit:ASPxGridView ID="gridAccomplishment" runat="server" AutoGenerateColumns="False" SettingsText-CommandClearFilter=""
        OnDataBinding="gridAccomplishment_DataBinding" OnCustomCallback="gridAccomplishment_CustomCallback"
        OnHtmlRowPrepared="gridAccomplishment_HtmlRowPrepared"
        ClientInstanceName="gridAccomplishment" Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid">
            <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
            <Columns>
                <dx:GridViewDataTextColumn Caption=" " HeaderStyle-Font-Bold="true" Settings-AllowHeaderFilter="False" 
                    Settings-AllowSort="False" Width="20px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FieldName="Created">
                    <HeaderCaptionTemplate>                        
                         <div onmousedown="return CancelEvent(event)" onmouseup="return CancelEvent(event)">
                             <asp:ImageButton ID="issueAddbtn" OnClientClick=" return newAccomplishmentItem();" runat="server" ImageUrl="~/Content/Images/plus-blue.png" Width="20px" CssClass="statusHeaderButton"/>                       
                        </div>                       
                    </HeaderCaptionTemplate>
                    <DataItemTemplate>
                        <%#  Container.ItemIndex + 1%>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Accomplishments" FieldName="Title" Width="15%">
                     
                    <DataItemTemplate>
                        <a id="aTitle" runat="server" href=""></a>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
           <%--     <dx:GridViewDataTextColumn Caption="Created On" FieldName="Created" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                </dx:GridViewDataTextColumn>--%>
                <dx:GridViewDataTextColumn Caption="Completed On" FieldName="AccomplishmentDate" Width="10%">
                    <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Caption="Description" PropertiesTextEdit-EncodeHtml="false" Settings-AllowHeaderFilter="False"
                    Settings-AllowSort="False" FieldName="ProjectNote" Width="10%"/>
                <dx:GridViewDataTextColumn Caption="" FieldName="ID" Settings-AllowHeaderFilter="False" Settings-AllowSort="False">
                     <HeaderTemplate>                        
                      <div class="crm-checkWrap" style="float:right; width:auto; display:inline-block;">                       
                        <asp:CheckBox ID="cbHeaderShowArchivedAccomplishment" Text="Show Archived"   runat="server" onClick="accomplishmentCheckArchived(this.id);"  OnInit="cbHeaderShowArchivedAccomplishment_Init" />
                      </div>
                    </HeaderTemplate> 
                    <DataItemTemplate>
                        <div>
                            <a id="aDelete" title="Delete" style="float: right;" visible="false" runat="server" href="javascript:">
                                <img id="ImgPermission" width="16" runat="server" src="/Content/Images/grayDelete.png" />
                            </a>

                            <a id="aArchive" style="float: right;" title="Archive" runat="server" href="javascript:">
                                <img id="Img10" runat="server" width="16" src="/Content/Images/grayDelete.png" />
                            </a>
                             <a id="aUnArchive" title="Unarchive" visible="false" runat="server" href="javascript:" style="float: right" class="ms-cui-img-16by16 ms-cui-img-cont-float">
                                <img id="ImgUnArchive" runat="server" src="/Content/Images/unarchive.png" style="float:left; margin-top:3px; width:15px;" />
                            </a>

                            <a id="aEdit" runat="server" href="javascript:" title="Edit" style="float: right; padding-left: 4px; padding-right: 4px;">
                                <img id="Imgedit" runat="server" width="16" src="/Content/Images/editNewIcon.png" />
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
            <ClientSideEvents />
        </ugit:ASPxGridView>
    </div>
    <div class="row">
        <div class="col-md-1 noPadding" style="padding-right:0; margin-top:10px; float:right; display:inline-block;">
            <div style="display:none">
            <asp:LinkButton OnClientClick="return newAccomplishmentItem()" ID="newTask" runat="server" Text="Add New"
                CssClass="aspLinkButton"></asp:LinkButton>
                </div>
        </div>
         
        <div class="crm-checkWrap" style="float:right; width:auto; display:none; margin-top:15px;">
            <asp:CheckBox ID="cbShowArchivedAccomplishment" Text="Show Archived" OnLoad="cbShowArchivedAccomplishment_Load" AutoPostBack="true"
            runat="server" />
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



<%-- Accomplishments --%>
    <div id="readonlyblock" class="readonlyblock" runat="server">
        <asp:ListView ID="lvReadOnlyAccomplishments" Visible="true" runat="server" ItemPlaceholderID="PlaceHolder1">
            <LayoutTemplate>
                <br />
                <table class="ro-table" frame="box" rules="all" width="100%" cellpadding="0" cellspacing="0">
                    <tr class="ro-header" style="border-bottom: 2px solid grey">
                        <th class="ro-padding" width="20px">
                            <b>&nbsp;</b>
                        </th>
                        <th class="ro-padding" style="text-align: left;" width="300px">
                            <b>Accomplishments</b>
                        </th>
                        <th class="ro-padding" style="text-align: center;" width="100px">
                            <b>Created On</b>
                        </th>
                        <th class="ro-padding" style="text-align: center;" width="120px">
                            <b>Completed On</b>
                        </th>
                        <th class="ro-padding" style="text-align: left;">
                            <b>Description</b>
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
                        <%# string.Format("{0:MMM-dd-yyyy}",  Eval(DatabaseObjects.Columns.Created)) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0:MMM-dd-yyyy}",  Eval(DatabaseObjects.Columns.AccomplishmentDate)) %>
                    </td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.ProjectNote)%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="ro-alternateitem">
                    <td class="ro-padding" style="text-align: right;">
                        <%# Container.DataItemIndex +1 %>
                    </td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.Title) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0:MMM-dd-yyyy}",  Eval(DatabaseObjects.Columns.Created)) %>
                    </td>
                    <td class="ro-padding" style="text-align: center;">
                        <%# string.Format("{0:MMM-dd-yyyy}",  Eval(DatabaseObjects.Columns.AccomplishmentDate)) %>
                    </td>
                    <td class="ro-padding" style="text-align: left;">
                        <%# Eval(DatabaseObjects.Columns.ProjectNote)%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        </asp:ListView>
    </div>



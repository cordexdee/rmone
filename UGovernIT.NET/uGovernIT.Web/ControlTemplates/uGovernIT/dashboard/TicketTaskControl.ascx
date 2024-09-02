<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketTaskControl.ascx.cs" Inherits="uGovernIT.Web.TicketTaskControl" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {

        
        var ticketid = params.split('=')[1];
        

       <%--//set_cookie('UseManageStateCookies', 'true', null, "<%= SPContext.Current.Web.ServerRelativeUrl %>");--%>
        window.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
    }


</script>

<div id="dvViewChangeCardPending" runat="server" class="pendingTask-heading" visible="false">
     <h5 class="cardView-label" runat ="server">Tickets</h5>
    <%-- <asp:ImageButton runat="server"  ImageUrl="/Content/Images/card-viewNew.png" ID="ViewChangeCardPending" AlternateText="" OnClientClick="BringCard();" class="newGridIcon"/>--%>
     <asp:ImageButton runat="server"  ImageUrl="/Content/Images/card-viewBlue.png" ID="ViewChangeCardPending" AlternateText="" OnClick="ViewChangeCardPending_Click"  class="newGridIcon"/>
</div>

<asp:Panel ID="gridPanel" runat="server"></asp:Panel>

<ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="false" SettingsPager-PageSize="10"  ClientInstanceName="grid"  visible="false"
    UseFixedTableLayout="false"
    ShowHorizontalScrollBar="true"
    Width="100%" KeyFieldName="TicketId" CssClass="customgridview homeGrid" EnableRowsCache="true" OnHtmlRowCreated="grid_HtmlRowPrepared" >
    <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
    <columns>
        <%--<dx:GridViewDataTextColumn  FieldName="ModuleName" Caption="Module" >    
            <DataItemTemplate>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn  FieldName="TicketId" Caption="ID" >    
            
        </dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="Item Order"></dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Title"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PercentComplete" Caption="% Complete"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Status" Caption="Status"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="AssignedTo" Caption="Assigned To"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="EstimatedHours" Caption="Est. Hrs"></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="StartDate" Caption="Start Date"><PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" /></dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="DueDate" Caption="Due Date"><PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" /></dx:GridViewDataTextColumn>
    </columns>
    <settingscommandbutton>
            <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
    </settingscommandbutton>
    <settingspopup>
        <HeaderFilter Height="200" />
    </settingspopup>
    <settingspager>
        <PageSizeItemSettings Position="Right" Visible="true" Items="5,10,15,20,25,50,100"></PageSizeItemSettings>                                                            
    </settingspager>
    <styles>
        <Row CssClass="customrowheight newUIGrid-dataRow homeGrid_dataRow" HorizontalAlign="Center"></Row>  
        <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
    </styles>
    <settingsbehavior allowsort="true" enablerowhottrack="false" />
</ugit:ASPxGridView>

<div class="cardView-wrap" >
    <div id="headCardView" runat="server" class="pendingTask-heading">
        <h5 class="cardView-label" runat ="server" onclick="showgrid();">Pending Tickets &nbsp;&nbsp;&nbsp; <asp:ImageButton runat="server" ImageUrl="/Content/Images/gridBlue.png" ID="ImageButton1" AlternateText=""  OnClick="ImageButton1_Click" class="newGridIcon"/></h5>
    </div>
    <dx:ASPxCardView ID="CardView" CssClass="cardView-container" ClientInstanceName="CardView"  runat ="server" KeyFieldName="KeyId" EnableCardsCache="true" Width="100%" 
        OnHtmlCardPrepared="CardView_HtmlCardPrepared" OnCardLayoutCreated="CardView_CardLayoutCreated" OnClientLayout="CardView_ClientLayout" 
        Settings-LayoutMode="Flow"  CardLayoutProperties-SettingsItems-HorizontalAlign="Right" 
        Styles-Card-HorizontalAlign="Left" SettingsCommandButton-EndlessPagingShowMoreCardsButton-Styles-Style-CssClass="showMore-linkContainer" 
        SettingsPager-SettingsFlowLayout-ItemsPerPage="100"
        SettingsBehavior-AllowFocusedCard="false" >
         <Columns >
            
                <dx:CardViewColumn FieldName="AgeText"  />
                <dx:CardViewColumn FieldName="Title"   />
                <dx:CardViewColumn FieldName="Age"   />
                <dx:CardViewColumn FieldName="Color" VisibleIndex="3"  />
                <dx:CardViewColumn FieldName="TicketId" VisibleIndex="4"  />
                <dx:CardViewColumn FieldName="ID" VisibleIndex="5"  />
                <dx:CardViewColumn FieldName="ItemOrder" VisibleIndex="6"  />
            </Columns>
            <CardLayoutProperties ColCount="1" >
                <Items>
                    <dx:CardViewColumnLayoutItem ColumnName="AgeText" ShowCaption="False" RowSpan="10" VerticalAlign="Top"  
                        CaptionSettings-HorizontalAlign="Center" HorizontalAlign="Center" CaptionStyle-Font-Underline="true" CssClass="cardHeading-row"
                        BackgroundImage-VerticalPosition="center"  BackgroundImage-Repeat="NoRepeat" />
                    <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False"  HorizontalAlign="Center" CssClass="cardTask-row" />
                </Items>
            </CardLayoutProperties>
             <FormatConditions >
               <%--<dx:CardViewFormatConditionColorScale FieldName="Age"  MaximumColor="Yellow"  MinimumColor="Black" ShowInColumn="AgeText" MiddleColor="Turquoise"/>--%>
                 <%--<dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] <= 3 && [Age] >= 0" Format="GreenFillWithDarkGreenText" />
                 <dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] > 3" Format="YellowFillWithDarkYellowText" />
                  <dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] < 0" Format="LightRedFill" />
                 <dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] == 0" Format="GreenFillWithDarkGreenText" />--%>
             </FormatConditions>
            <Settings  VerticalScrollableHeight="150"   />
            <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
            <SettingsPager Mode="EndlessPaging"  EndlessPagingMode="OnClick"  AlwaysShowPager="true"  SettingsFlowLayout-ItemsPerPage="6"  />
            <Styles>
                <Card Width="10%" Height="90%"  ></Card>
            
            </Styles>
        </dx:ASPxCardView>
</div>





<div class="cardView-wrap" >
    <div class="completedTask-heading" id="headCardViewRecentTask" runat="server">
       <%-- <h5 class="cardView-label" runat ="server">Recently Completed Tasks <a href="<%=landingPageUrl %>?Viewmode=gridview&type=CompletedTask"><img src="/Content/Images/gridNew.png" id="gridView1"  class="newGridIcon"/></a></h5>--%>
        <h5 class="cardView-label" runat ="server">Recently Completed Tickets &nbsp;&nbsp;&nbsp; <asp:ImageButton runat="server" ImageUrl="/Content/Images/gridBlue.png" ID="viewChange" AlternateText="" OnClick="viewChange_Click" class="newGridIcon"/></h5>
        <%--<asp:ImageButton runat="server" ImageUrl="/Content/Images/gridNew.png" ID="viewChange" AlternateText="" OnClientClick="showgridrecent();" class="newGridIcon"/>--%>
        <%--<asp:ImageButton runat="server" ImageUrl="/Content/Images/gridNew.png" ID="viewChange" AlternateText="" OnClick="viewChange_Click" class="newGridIcon"/>--%>
    </div>
    <dx:ASPxCardView ID="CardViewRecentTask" CssClass="cardView-container" runat="server" KeyFieldName="ID" EnableCardsCache="True" Width="100%" OnHtmlCardPrepared="CardViewRecentTask_HtmlCardPrepared"  ClientInstanceName="CardViewRecentTask"
        Settings-LayoutMode="Flow"  CardLayoutProperties-SettingsItems-HorizontalAlign="Right" Styles-Card-HorizontalAlign="Left" 
        SettingsCommandButton-EndlessPagingShowMoreCardsButton-Styles-Style-CssClass="showMore-linkContainer">
            <Columns>
                <dx:CardViewColumn FieldName="AgeText" HeaderStyle-Font-Bold="true"  />
                <dx:CardViewColumn FieldName="Title"   />
                <dx:CardViewColumn FieldName="Age"   />
                <dx:CardViewColumn FieldName="Color" VisibleIndex="3"  />
                <dx:CardViewColumn FieldName="TicketId" VisibleIndex="4"  />
                <dx:CardViewColumn FieldName="ID" VisibleIndex="5"  />
                <dx:CardViewColumn FieldName="ItemOrder" VisibleIndex="6"  />
            </Columns>
            <CardLayoutProperties ColCount="1" >
                <Items>
                    <dx:CardViewColumnLayoutItem ColumnName="AgeText" ShowCaption="False" RowSpan="10" VerticalAlign="Top"   
                        CaptionSettings-HorizontalAlign="Center" HorizontalAlign="Center" CssClass="cardHeading-row"
                        BackgroundImage-ImageUrl="/Content/Images/paper-clip.png" BackgroundImage-VerticalPosition="center"  BackgroundImage-Repeat="NoRepeat"/>
                    <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False"  HorizontalAlign="Center" CssClass="cardTask-row"/>
                </Items>
            </CardLayoutProperties>
        
            <Settings VerticalScrollableHeight="150" />
            <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
            <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="3" EndlessPagingMode="OnClick"  AlwaysShowPager="true"  SettingsFlowLayout-ItemsPerPage="6"   />
            <Styles>
                <Card Width="10%" Height="90%"/>
            </Styles>
        </dx:ASPxCardView>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskUserControl.ascx.cs" Inherits="uGovernIT.Web.TaskUserControl" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dxcvFLECW {
        text-align: center !important;
    }

    .pendingTask-heading .cardView-label, .completedTask-heading .cardView-label {
        margin-bottom: 10px;
    }

    .cardView-wrap .dxcvEmptyCard_UGITNavyBlueDevEx {
        width: auto;
        height: 98px;
    }

    .dxtcLite_UGITNavyBlueDevEx.dxtcLite-row .dxtc-leftIndent, .dxtcLite_UGITNavyBlueDevEx.dxtcLite-row .dxtc-rightIndent {
        display: none !important;
    }

    .CRMstatusGrid_headerRow {
        padding-left: 6px;
        padding-right: 6px;
    }

</style>

<div id="dvViewChangeCardPending" runat="server" class="pendingTask-heading" visible="false">
    <h5 class="cardView-label" runat="server">Tasks</h5>
    <%-- <asp:ImageButton runat="server"  ImageUrl="/Content/Images/card-viewNew.png" ID="ViewChangeCardPending" AlternateText="" OnClientClick="BringCard();" class="newGridIcon"/>--%>
    <asp:ImageButton runat="server" ImageUrl="/Content/Images/card-viewBlue.png" ID="ViewChangeCardPending" AlternateText="" OnClick="ViewChangeCardPending_Click" class="newGridIcon" />
</div>

<dx:ASPxTabControl ID="tabs" runat="server" AutoPostBack="true" OnTabClick="tab_TabClick" Visible="false" CssClass="dxtcLite-row">
    <Tabs>
        <dx:Tab Text="Pending" Name="PendingTask" TabStyle-CssClass="py-1"></dx:Tab>
        <dx:Tab Text="Recently Completed" Name="CompletedTask" TabStyle-CssClass="py-1"></dx:Tab>
    </Tabs>
</dx:ASPxTabControl>
<ugit:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="false" SettingsPager-PageSize="10" ClientInstanceName="grid" Visible="false"
    UseFixedTableLayout="false"
    ShowHorizontalScrollBar="true"
    Width="100%" KeyFieldName="ID" CssClass="customgridview homeGrid mt-0" EnableRowsCache="true" OnHtmlRowCreated="grid_HtmlRowPrepared" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared">
    <SettingsAdaptivity AdaptivityMode="HideDataCells" AllowOnlyOneAdaptiveDetailExpanded="true"></SettingsAdaptivity>
    <Columns>
        <%--<dx:GridViewDataTextColumn  FieldName="ModuleName" Caption="Module" >    
            <DataItemTemplate>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="TicketId" Caption="Project">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </dx:GridViewDataTextColumn>
        <%--<dx:GridViewDataTextColumn FieldName="ItemOrder" Caption="Item Order"></dx:GridViewDataTextColumn>--%>
        <dx:GridViewDataTextColumn FieldName="Title" Caption="Task Title">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="PercentComplete" Caption="%" Width="50">
            <CellStyle HorizontalAlign="Center"></CellStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="Status" Caption="Status">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="AssignedToUser" Caption="Assigned To">
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="EstimatedHours" Caption="Est. Hrs" Width="50">
            <CellStyle HorizontalAlign="Center"></CellStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="StartDate" Caption="Start Date">
            <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            <CellStyle HorizontalAlign="Center"></CellStyle>
        </dx:GridViewDataTextColumn>
        <dx:GridViewDataTextColumn FieldName="DueDate" Caption="Due Date">
            <PropertiesTextEdit DisplayFormatString="MMM-dd-yyyy" />
            <CellStyle HorizontalAlign="Center"></CellStyle>
        </dx:GridViewDataTextColumn>
    </Columns>
    <SettingsCommandButton>
        <ShowAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
        <HideAdaptiveDetailButton ButtonType="Button" Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
    </SettingsCommandButton>
    <SettingsPopup>
        <HeaderFilter Height="200" />
    </SettingsPopup>
    <SettingsPager AlwaysShowPager="false">
        <PageSizeItemSettings Position="Right" Visible="false" Items="5,10,15,20,25,50,100"></PageSizeItemSettings>
    </SettingsPager>
    <Styles>
        <Row CssClass="customrowheight homeGrid_dataRow" HorizontalAlign="Left" Cursor="pointer"></Row>
        <Header Font-Bold="true" HorizontalAlign="Center" CssClass="CRMstatusGrid_headerRow"></Header>
    </Styles>
    <SettingsBehavior AllowSort="true" EnableRowHotTrack="false" />
</ugit:ASPxGridView>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    //function BringCard(s, e) {
    //    console.log("sssss");
    //    //grid.PerformCallback();
    //    CardView.SetVisible(true);
    //    grid.SetVisible(false);
    //}
    //function showgridrecent(s, e) {
    //    console.log("sssss");
    //    //CardView.PerformCallback();
    //    grid.SetVisible(true);

    //}
    //function showgrid(s, e) {
    //    console.log("sssss");
    //   grid.SetVisible(true);
    //    CardView.SetVisible(false);
    //    //CardView.PerformCallback();
    //}
    function isEmpty(el) {
        return !$.trim(el.html())
    }
    // disabling Enter Key press event, when search word entered in Global Search & pressing Enter key is causing entire Page Reload.
    $(document).ready(function () {
        document.onkeypress = function (evt) {
            return ((evt) ? evt : ((event) ? event : null)).keyCode != 13;
        };
        //if (isEmpty($('.cardView-wrap:eq(0)'))) {
        //    var $container = $('.cardView-wrap:eq(0)');
        //    $container.append('<div style="margin-top:100px; margin-left:auto;margin-right:auto;width:200px;font-size:20px" id="emptymsg ' + $container.children().length + 1 + '">No Task Found</div>');
        //    //alert('empty');
        //}
    });

    var selectedTask = {};

    function doTaskComplete(s, e) {
        selectedTask = {};

        var obj = s.GetMainElement();
        var ticketid = obj.attributes["TicketId"];
        var taskid = obj.attributes["TaskId"];
        var tasktype = obj.attributes["TableName"];

        if (confirm("Are you sure you want to mark task as completed?")) {
            markTaskAsComplete(ticketid, taskid, tasktype);
        }

    }

    function markTaskAsComplete(ticketid, taskid, tasktype) {


        $.ajax({
            url: ugitConfig.apiBaseUrl + "/api/module/MarkTaskAsComplete",
            method: "POST",
            data: { TaskKeys: taskid.value, TicketPublicId: ticketid.value, TaskType: tasktype.value },
            success: function (data) {
                debugger;
                CardView.Refresh();
                CardViewRecentTask.Refresh();
            },
            error: function (error) { }
        });
    }


    function editTask(s, e) {

        var obj = s.GetMainElement();
        var ticketID = obj.attributes["TicketId"];
        var taskID = obj.attributes["TaskId"];
        var title = obj.attributes["CardToolTip"];
        var type = obj.attributes["TableName"];
        var moduleName = ""; // obj.attributes["ModuleName"];
        var viewType = type.value;
        var moduleStage = "";
        var taskUrl = '<%= ConstraintTaskUrl %>';
        var ruleUrl = '<%= ConstraintRuleUrl %>';

        var taskParams = "";
        var ruleParams = "";
        taskParams = "module=" + moduleName + "&ticketId=" + ticketID.value + "&conditionType='ModuleTaskCT'&moduleStage=" + moduleStage + "&taskID=" + taskID.value + "&type='ExistingConstraint'" + "&viewType=" + viewType + "";
        ruleParams = "module=" + moduleName + "&ticketId=" + ticketID.value + "&conditionType='ModuleRuleCT'&moduleStage=" + moduleStage + "&taskID=" + taskID.value + "&type='ExistingConstraint'";
        if (type.value != null && type.value == "ModuleStageConstraints") {

            window.parent.UgitOpenPopupDialog(taskUrl, taskParams, title, '800px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }

        else if (type.value != null && type.value == "ModuleStageConstraints") {

            window.parent.UgitOpenPopupDialog(ruleUrl, ruleParams, title, '800px', '600px', 0, escape("<%= Request.Url.AbsolutePath %>"));
        }
    }

    function deleteTask(s, e) {
        var postData = {};
        var obj = s.GetMainElement();
        var ticketID = obj.attributes["TicketId"];
        var taskID = obj.attributes["TaskId"];
        var title = obj.attributes["CardToolTip"];
        var type = obj.attributes["TableName"];
        postData.TicketId = ticketID.value;
        postData.TaskId = taskID.value;
        postData.mode = type.value;

        if (confirm("Are you sure you want to delete task?")) {
            $.ajax({
                url: ugitConfig.apiBaseUrl + "/api/module/DeleteHomePageTask",
                method: "DELETE",
                data: { TaskKeys: postData, TicketPublicId: ticketID.value },
                success: function (data) {
                    CardView.Refresh();
                    CardViewRecentTask.Refresh();
                },
                error: function (error) { }
            });
        }
    }
</script>


<div class="cardView-wrap">
    <div id="headCardView" runat="server" class="pendingTask-heading">
        <%-- <h5 class="cardView-label">Pending Tasks <a href="<%=landingPageUrl %>?Viewmode=gridview&type=PendingTask"><img src="/Content/Images/gridNew.png" id="gridView"  class="newGridIcon"/></a></h5>--%>
        <h5 class="cardView-label" runat="server">Pending Tasks &nbsp;&nbsp;&nbsp;
            <asp:ImageButton runat="server" ImageUrl="/Content/Images/gridBlue.png" ID="ImageButton1" AlternateText="" OnClick="ImageButton1_Click" class="newGridIcon" /></h5>
        <%--<asp:ImageButton runat="server" ImageUrl="/Content/Images/gridNew.png" ID="ImageButton1" AlternateText=""  OnClientClick="showgrid();" class="newGridIcon"/>--%>
        <%--<asp:ImageButton runat="server" ImageUrl="/Content/Images/gridNew.png" ID="ImageButton1" AlternateText=""  OnClick="ImageButton1_Click" class="newGridIcon"/>--%>
    </div>
    <dx:ASPxCardView ID="CardView" CssClass="cardView-container" ClientInstanceName="CardView" runat="server" KeyFieldName="ID" EnableCardsCache="True" Width="100%"
        OnHtmlCardPrepared="CardView_HtmlCardPrepared" OnCardLayoutCreated="CardView_CardLayoutCreated" OnClientLayout="CardView_ClientLayout"
        SettingsPager-SettingsFlowLayout-ItemsPerPage="2" Settings-LayoutMode="Flow" CardLayoutProperties-SettingsItems-HorizontalAlign="Right"
        Styles-Card-HorizontalAlign="Left" SettingsCommandButton-EndlessPagingShowMoreCardsButton-Styles-Style-CssClass="showMore-linkContainer">
        <Columns>

            <dx:CardViewColumn FieldName="AgeText">
                <DataItemTemplate>
                    <div id="divAgeText">
                        <span><%# Eval("AgeText") %></span>
                        <div style="float: right; padding-right: 5px;">
                            <dx:ASPxImage ID="imgMarkascomplete" ClientInstanceName="imgMarkascomplete" runat="server" ImageUrl="/Content/images/tick-icon.png" ToolTip="Mark As Complete"
                                TicketId='<%# Eval("TicketId") %>' TaskId='<%# Eval("ID") %>' TableName='<%# Eval("TableName") %>' CardToolTip='<%# Eval("CardToolTip") %>'>
                                <ClientSideEvents Click="function(s, e){ 
                                event.cancelBubble=true;
                                doTaskComplete(s, e); 
                                }" />
                            </dx:ASPxImage>
                            <dx:ASPxImage ID="imgEditTask" ClientInstanceName="imgEditTask" runat="server" ImageUrl="/Content/images/edit-icon.png" Visible="false" ToolTip="Edit Task"
                                TicketId='<%# Eval("TicketId") %>' TaskId='<%# Eval("ID") %>' TableName='<%# Eval("TableName") %>' CardToolTip='<%# Eval("CardToolTip") %>'>
                                <ClientSideEvents Click="function(s, e){ 
                                    event.cancelBubble=true;
                                    editTask(s, e);
                                    }" />
                            </dx:ASPxImage>
                            <dx:ASPxImage ID="imgDeleteTask" ClientInstanceName="imgDeleteTask" runat="server" ImageUrl="/Content/images/delete-icon.png" ToolTip="Delete Task"
                                TicketId='<%# Eval("TicketId") %>' TaskId='<%# Eval("ID") %>' TableName='<%# Eval("TableName") %>' CardToolTip='<%# Eval("CardToolTip") %>'>
                                <ClientSideEvents Click="function(s, e){
                                    event.cancelBubble = true;
                                    deleteTask(s, e);
                                    }" />
                            </dx:ASPxImage>
                        </div>
                    </div>
                </DataItemTemplate>
            </dx:CardViewColumn>
            <dx:CardViewColumn FieldName="Title" />
            <dx:CardViewColumn FieldName="Age" />
            <dx:CardViewColumn FieldName="Color" VisibleIndex="3" />
            <dx:CardViewColumn FieldName="TicketId" VisibleIndex="4" />
            <dx:CardViewColumn FieldName="ID" VisibleIndex="5" />
            <dx:CardViewColumn FieldName="ItemOrder" VisibleIndex="6" />
        </Columns>
        <CardLayoutProperties ColCount="1">
            <Items>
                <dx:CardViewColumnLayoutItem ColumnName="AgeText" ShowCaption="False" RowSpan="10" VerticalAlign="Top"
                    CaptionSettings-HorizontalAlign="Center" HorizontalAlign="Center" CaptionStyle-Font-Underline="true" CssClass="cardHeading-row"
                    BackgroundImage-ImageUrl="/Content/Images/paper-clip.png" BackgroundImage-VerticalPosition="center" BackgroundImage-Repeat="NoRepeat" />
                <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False" HorizontalAlign="Center" CssClass="cardTask-row" />
            </Items>
        </CardLayoutProperties>
        <FormatConditions>
            <%--<dx:CardViewFormatConditionColorScale FieldName="Age"  MaximumColor="Yellow"  MinimumColor="Black" ShowInColumn="AgeText" MiddleColor="Turquoise"/>--%>
            <%--<dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] <= 3 && [Age] >= 0" Format="GreenFillWithDarkGreenText" />
             <dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] > 3" Format="YellowFillWithDarkYellowText" />
              <dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] < 0" Format="LightRedFill" />
             <dx:CardViewFormatConditionHighlight FieldName="AgeText" Expression="[Age] == 0" Format="GreenFillWithDarkGreenText" />--%>
        </FormatConditions>
        <Settings VerticalScrollableHeight="150" />
        <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
        <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="3" EndlessPagingMode="OnClick" AlwaysShowPager="true" SettingsFlowLayout-ItemsPerPage="6" />
        <Styles>
            <Card Width="10%" Height="90%"></Card>

        </Styles>
    </dx:ASPxCardView>
</div>





<div class="cardView-wrap">
    <div class="completedTask-heading" id="headCardViewRecentTask" runat="server">
        <%-- <h5 class="cardView-label" runat ="server">Recently Completed Tasks <a href="<%=landingPageUrl %>?Viewmode=gridview&type=CompletedTask"><img src="/Content/Images/gridNew.png" id="gridView1"  class="newGridIcon"/></a></h5>--%>
        <h5 class="cardView-label" runat="server">Recently Completed Tasks &nbsp;&nbsp;&nbsp;
            <asp:ImageButton runat="server" ImageUrl="/Content/Images/gridBlue.png" ID="viewChange" AlternateText="" OnClick="viewChange_Click" class="newGridIcon" /></h5>
        <%--<asp:ImageButton runat="server" ImageUrl="/Content/Images/gridNew.png" ID="viewChange" AlternateText="" OnClientClick="showgridrecent();" class="newGridIcon"/>--%>
        <%--<asp:ImageButton runat="server" ImageUrl="/Content/Images/gridNew.png" ID="viewChange" AlternateText="" OnClick="viewChange_Click" class="newGridIcon"/>--%>
    </div>
    <dx:ASPxCardView ID="CardViewRecentTask" CssClass="cardView-container" runat="server" KeyFieldName="ID" EnableCardsCache="True" Width="100%" OnHtmlCardPrepared="CardViewRecentTask_HtmlCardPrepared" ClientInstanceName="CardViewRecentTask"
        Settings-LayoutMode="Flow" CardLayoutProperties-SettingsItems-HorizontalAlign="Right" Styles-Card-HorizontalAlign="Left"
        SettingsCommandButton-EndlessPagingShowMoreCardsButton-Styles-Style-CssClass="showMore-linkContainer">
        <Columns>
            <dx:CardViewColumn FieldName="AgeText" HeaderStyle-Font-Bold="true">
                <DataItemTemplate>
                    <div id="divRecentTaskAgeText">
                        <span><%# Eval("AgeText") %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                        <div style="float: right; padding-right: 5px;">
                            <dx:ASPxImage ID="imgDeleteTaskRecent" ClientInstanceName="imgDeleteTaskRecent" runat="server" ImageUrl="/Content/images/delete-icon.png" ToolTip="Delete Task"
                                TicketId='<%# Eval("TicketId") %>' TaskId='<%# Eval("ID") %>' TableName='<%# Eval("TableName") %>' CardToolTip='<%# Eval("CardToolTip") %>'>
                                <ClientSideEvents Click="function(s, e){
                                    event.cancelBubble = true;
                                    deleteTask(s, e);
                                    }" />
                            </dx:ASPxImage>
                        </div>
                    </div>
                </DataItemTemplate>
            </dx:CardViewColumn>
            <dx:CardViewColumn FieldName="Title" />
            <dx:CardViewColumn FieldName="Age" />
            <dx:CardViewColumn FieldName="Color" VisibleIndex="3" />
            <dx:CardViewColumn FieldName="TicketId" VisibleIndex="4" />
            <dx:CardViewColumn FieldName="ID" VisibleIndex="5" />
            <dx:CardViewColumn FieldName="ItemOrder" VisibleIndex="6" />
        </Columns>
        <CardLayoutProperties ColCount="1">
            <Items>
                <dx:CardViewColumnLayoutItem ColumnName="AgeText" ShowCaption="False" RowSpan="10" VerticalAlign="Top"
                    CaptionSettings-HorizontalAlign="Center" HorizontalAlign="Center" CssClass="cardHeading-row"
                    BackgroundImage-ImageUrl="/Content/Images/paper-clip.png" BackgroundImage-VerticalPosition="center" BackgroundImage-Repeat="NoRepeat" />
                <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False" HorizontalAlign="Center" CssClass="cardTask-row" />
            </Items>
        </CardLayoutProperties>

        <Settings VerticalScrollableHeight="150" />
        <SettingsCommandButton EndlessPagingShowMoreCardsButton-Text="Show More..."></SettingsCommandButton>
        <SettingsPager Mode="EndlessPaging" SettingsTableLayout-ColumnCount="3" EndlessPagingMode="OnClick" AlwaysShowPager="true" SettingsFlowLayout-ItemsPerPage="6" />
        <Styles>
            <Card Width="10%" Height="90%" />
        </Styles>
    </dx:ASPxCardView>
</div>

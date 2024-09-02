<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactActivity.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.uGovernIT.ContactActivity" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    .hide {
        display: none;
    }

    .clickedTab {
        background-color: #0072c6;
    }

        .clickedTab a {
            color: #fff !important;
        }

    .pagerBox td table tr td span {
        /* font-size : larger; */
        border: 1px solid black;
        padding: 0px 3px;
    }

    table.ms-listviewtable > tbody > tr > td {
        border: none;
    }

    .ms-viewheadertr .ms-vh2-gridview {
        background: transparent !important;
        height: 22px;
    }

    .ms-vh2 .ms-selectedtitle .ms-vb, .ms-vh2 .ms-unselectedtitle .ms-vb {
        text-align: left;
    }

    .ms-listviewtable .ms-vb2, .ms-summarystandardbody .ms-vb2 {
        text-align: left;
    }

    .pctcompletecolumn {
        padding-right: 10px;
        text-align: center;
    }

    .fleft {
        float: left;
    }


    .action-container {
        background: none repeat scroll 0 0 #FFFFAA;
        border: 1px solid #FFD47F;
        float: left;
        padding: 1px 5px 0;
        position: absolute;
        z-index: 1000;
        margin-top: -4px;
        margin-left: 3px;
        right: 0px;
        top: 0px;
    }

    .ucontentdiv {
        float: left;
        height: 18px;
        float: left;
        padding: 1px 6px 0;
        margin: 2px 4px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function openActivityDialog(path, params, titleVal) {
        window.parent.UgitOpenPopupDialog(path, params, titleVal, '600px', '410px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function changeMyActivityView(viewType) {
        $("#myActivityViewType").val(viewType);
        set_cookie("myActivityViewType", viewType);
        return true;
    }

    function OpneEditActivity(obj, activityId) {

        var param = "&ID=" + activityId + "&contactID=0 &ticketID=<%= ticketID%>";
        window.parent.UgitOpenPopupDialog('<%=absoluteUrlEdit%>', param, "Activities - Edit Item", '600px', '410px', 0, escape("<%= Request.Url.AbsolutePath %>"));
    }

    function showContactActivityActions(trObj, activityId) {
        $("#ContactActionButtons" + activityId).css("display", "block");
        

    }

    function hideContactActivityActions(trObj, activityId) {
        //show description icon
        $("#ContactActionButtons" + activityId).css("display", "none");
    }

    function openTicketDialog(path, params, titleVal, width, height, stopRefresh, returnUrl) {
        titleVal = 
        window.parent.parent.UgitOpenPopupDialog(path, params, titleVal, width, height, stopRefresh, returnUrl);
        console.log(titleVal)
    }

</script>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function UpdateGridHeight() {
            grdActivities.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            grdActivities.SetHeight(containerHeight);
        }
        window.addEventListener('resize', function (evt) {
            if (!ASPxClientUtils.androidPlatform)
                return;
            var activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
        });
</script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
        function UpdateGridHeight() {
            grdEmailActivity.SetHeight(0);
            var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
            if (document.body.scrollHeight > containerHeight)
                containerHeight = document.body.scrollHeight;
            grdEmailActivity.SetHeight(containerHeight);
        }
        window.addEventListener('resize', function (evt) {
            if (!ASPxClientUtils.androidPlatform)
                return;
            var activeElement = document.activeElement;
            if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
                window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
        });
</script>
<table id="tbTabs" runat="server" cellpadding="0" cellspacing="0" border="0" style="border-collapse: collapse;" width="99%">
    <tr>
        <td class="ugit-contentcontainer" style="padding-top: 3px">
            <input type="hidden" id="myActivityViewType" name="myActivityViewType" />
            <div class="uborderdiv">
                <div class="ucontentdiv ugitlinkbg" id="myActivityByContact" runat="server">
                    <span>
                        <asp:LinkButton ID="btActivityByContact" OnClientClick="javascript:return changeMyActivityView('bycontact')"
                            runat="server" Text="By Contact"></asp:LinkButton>
                    </span>
                </div>
            </div>
            <div class="uborderdiv">
                <div class="ucontentdiv ugitlinkbg" id="myActivityByDueDate" runat="server">
                    <span>
                        <asp:LinkButton ID="btMyActivityByDueDate" runat="server" Text="By Due Date"
                            OnClientClick="javascript:return changeMyActivityView('byduedate')"></asp:LinkButton>
                    </span>
                </div>
            </div>

        </td>
    </tr>

</table>


<div class="col-md-12 col-sm-12 col-xs-12" style="width: 100%">
    <div class="row">
        <div style="padding-top: 10px">
            <asp:HiddenField ID="hndContactId" runat="server" Value="0" />
            <dx:ASPxGridView ID="grdActivities" AutoGenerateColumns="False" runat="server" ClientInstanceName="grdActivities" 
                OnHtmlRowPrepared="grdActivities_HtmlRowPrepared" OnHtmlDataCellPrepared="grdActivities_HtmlDataCellPrepared"
                SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" Width="100%" OnRowCommand="grdActivities_RowCommand"
                CssClass="customgridview homeGrid" >
                <SettingsText EmptyDataRow="No record found."></SettingsText>
                <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>

                    <%-- <dx:GridViewDataTextColumn FieldName=" " VisibleIndex="0" Width="30px">
                        <DataItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Delete" CommandName="DeleteActivity" OnClientClick="return confirm('Are you sure you want to delete?');">
                                                      <img src="/_Layouts/15/Images/uGovernIT/delete-icon.png" />
                            </asp:LinkButton>
                        </DataItemTemplate>

                        <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                    </dx:GridViewDataTextColumn>--%>



                    <dx:GridViewDataColumn FieldName="Title" Caption="Title" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left"></CellStyle>
                        <DataItemTemplate>
                            <div style="float: left; width: 100%; position: relative;">
                                <div style="float: left;">
                                    <a id="aTitle" runat="server" style="cursor: pointer" href="" onload="aTitle_Load"></a>
                                </div>
                                <div id='ContactActionButtons<%# Eval("ID") %>' style="display: none;  float:left; top: -3px; right: 5px; padding-left: 5px;">
                                    <asp:LinkButton ID="lnkMarkAsComplete" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Mark as Complete" CommandName="MarkAsComplete" CssClass="crmActivity_markAsComplete">
                                                      <img src="/Content/images/accept-symbol.png" />
                                    </asp:LinkButton>

                                    <asp:ImageButton OnClientClick='<%# string.Format("javascript:return OpneEditActivity(this, {0})", Eval("ID")) %>'
                                        ToolTip="Edit" ID="imgButtonEdit" runat="server" ImageUrl="/Content/images/editNewIcon.png" Style="padding-bottom: 8px;" CssClass="crmActivity_editBtn"/>

                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Delete" CommandName="DeleteActivity" OnClientClick="return confirm('Are you sure you want to delete?');" CssClass="CRMlead_deleteIcon">
                                                      <img src="/content/images/grayDelete.png" width="16" />
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>

                      <dx:GridViewDataColumn FieldName="ContactLookup" Caption="Contact" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Wrap="True" >
                        <Settings HeaderFilterMode="CheckedList" />
                        <HeaderStyle HorizontalAlign="Left" ></HeaderStyle>
                        <CellStyle HorizontalAlign="Left"></CellStyle>
                       <DataItemTemplate>
                            <div style="width:100%; position: relative;">
                                <div style="float: left;">
                                    <a id="aContact" runat="server" style="cursor: pointer" href=""></a>
                                </div>

                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>

                       <dx:GridViewDataColumn FieldName="TicketId" Caption="Lead" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left"></CellStyle>
                        <DataItemTemplate>
                            <div style="float: left; width: 100%; position: relative;">
                                <div style="float: left;">
                                    <a id="aLead" runat="server" style="cursor: pointer" href=""></a>
                                </div>

                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="AssignedToUser"  Caption="Assignee​" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <DataItemTemplate>
                            <div style="width: 100%;min-height:25px;">
                                <div style="padding-top:5px;">
                                    <asp:Label ID="lblAssignTo" runat="server" Text='<%# Eval("AssignedToUser") %>'></asp:Label>
                                </div>
                                <%--<div id='ContactActionButtons<%# Eval("ID") %>' style="display: none; width: 58px; float:left; top: -3px; right: 5px; padding-left: 5px;">
                                    <asp:LinkButton ID="lnkMarkAsComplete" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Mark as Complete" CommandName="MarkAsComplete" CssClass="crmActivity_markAsComplete">
                                                      <img src="/Content/images/accept-symbol.png" />
                                    </asp:LinkButton>

                                    <asp:ImageButton OnClientClick='<%# string.Format("javascript:return OpneEditActivity(this, {0})", Eval("ID")) %>'
                                        ToolTip="Edit" ID="imgButtonEdit" runat="server" ImageUrl="/Content/images/edit-newIcon.png" Style="padding-bottom: 8px;" CssClass="crmActivity_editBtn"/>

                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Delete" CommandName="DeleteActivity" OnClientClick="return confirm('Are you sure you want to delete?');" CssClass="CRMlead_deleteIcon">
                                                      <img src="/content/images/redNew_delete.png" />
                                    </asp:LinkButton>
                                </div>--%>
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="Description"  Caption="Activity" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="DueDate"  Caption="When Due" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <dx:GridViewDataColumn FieldName="ActivityStatus"  Caption="Status" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                </Columns>
                <Templates>
                    <GroupRowContent>
                        <table>
                            <tr>
                                <td>
                                    <a id="aGroupContact" runat="server" style="cursor: pointer" href=""></a>

                                </td>
                            </tr>
                        </table>
                    </GroupRowContent>
                </Templates>
                 <settingscommandbutton>
                    <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                    <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
                </settingscommandbutton>
                <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <Row HorizontalAlign="Center" CssClass="homeGrid_dataRow"></Row>
                    <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                    <Header Font-Bold="true" HorizontalAlign="Center" CssClass="homeGrid_headerColumn"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                </Styles>
            </dx:ASPxGridView>
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

    <div class="row">
        <div style="margin-top:10px">
            <a id="aAddItem" runat="server" href="#">
                <img src="/Content/Images/plus-blue.png" style="width:20px;"/>
                <span class="CrmLink_lable">Add New Activity</span>
            </a>
        </div>
    </div>

    <div class="row">
        <div style="padding-top: 10px">
            <dx:ASPxGridView ID="grdEmailActivity" AutoGenerateColumns="False" runat="server" ClientInstanceName="grdEmailActivity" 
                OnHtmlRowPrepared="grdActivities_HtmlRowPrepared" SettingsText-EmptyDataRow="No record found." KeyFieldName="ID" 
                Width="100%" OnRowCommand="grdEmailActivity_RowCommand" CssClass="homeGrid CRMstatus_gridContainer">
                <SettingsText EmptyDataRow="No record found."></SettingsText>
                <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
                <Columns>
                    <dx:GridViewDataTextColumn FieldName=" ">
                        <DataItemTemplate>
                            <%-- <img id="editLink" runat="server" src="/_layouts/15/images/uGovernIT/edit-icon.png" alt="Edit" style="float: right; padding-right: 10px;" />--%>
                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Delete" CommandName="DeleteActivity" OnClientClick="return confirm('Are you sure you want to delete?');">
                                                      <img src="/_Layouts/15/Images/uGovernIT/delete-icon.png" />
                            </asp:LinkButton>
                        </DataItemTemplate>

                        <Settings AllowAutoFilter="False" AllowSort="False" AllowHeaderFilter="False" />
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn FieldName="Title" Caption="Mail Subject" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        <CellStyle HorizontalAlign="Left"></CellStyle>
                        <DataItemTemplate>
                            <div style="float: left; width: 100%; position: relative;">
                                <div style="float: left;">
                                    <a id="a1" runat="server" style="cursor: pointer" href="" onload="aTitle_Load"></a>
                                </div>
                                <div id='ContactActionButtons<%# Eval("ID") %>' style="display: none; width: 38px; float: right; top: -3px; right: 10px; padding-left: 5px; position: absolute;">

                                    <%-- <asp:ImageButton CommandArgument='<%# Eval("ID") %>' 
                                                    CssClass="markascomplete-action" ToolTip="Mark as Complete" CommandName="MarkAsComplete" ID="btMarkComplete" runat="server"
                                                    ImageUrl="/_layouts/15/images/ugovernit/tick-icon.png" Style="padding-bottom: 5px; float: right;" />--%>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("ID") %>' ToolTip="Mark as Complete" CommandName="MarkAsComplete">
                                                      <img src="/_layouts/15/images/ugovernit/tick-icon.png" />
                                    </asp:LinkButton>

                                    <asp:ImageButton OnClientClick='<%# string.Format("javascript:return OpneEditActivity(this, {0})", Eval("ID")) %>'
                                        ToolTip="Edit" ID="ImageButton1" runat="server" ImageUrl="/_layouts/15/images/ugovernit/edit-icon.png" Style="padding-bottom: 5px; float: right;" />
                                </div>
                            </div>
                        </DataItemTemplate>
                    </dx:GridViewDataDateColumn>

                    <dx:GridViewDataColumn FieldName="ContactLookup" Caption="Contact Person" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>

                    <%-- <dx:GridViewDataColumn FieldName="Company" VisibleIndex="4" Caption="Company" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" Width="150px">
                                        <Settings HeaderFilterMode="CheckedList" />
                                    </dx:GridViewDataColumn>--%>

                    <dx:GridViewDataColumn FieldName="UGITAssignedTo" Caption="Sent by​" CellStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="UGITDueDate" Caption="Sent Date" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                        <Settings HeaderFilterMode="CheckedList" />
                    </dx:GridViewDataColumn>
                </Columns>
                <Templates>
                    <GroupRowContent>
                        <table>
                            <tr>
                                <td>
                                    <a id="a2" runat="server" style="cursor: pointer" href=""></a>
                                </td>
                            </tr>
                        </table>
                    </GroupRowContent>
                </Templates>
                <Settings ShowFooter="false" ShowHeaderFilterButton="true" />
                <SettingsBehavior AllowSort="true" AllowDragDrop="false" AutoExpandAllGroups="true" />
                <SettingsPopup>
                    <HeaderFilter Height="200" />
                </SettingsPopup>
                <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                    <Row HorizontalAlign="Center" CssClass="homeGrid_dataRow"></Row>
                    <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                    <Header Font-Bold="true" HorizontalAlign="Center" CssClass=" homeGrid_headerColumn"></Header>
                    <AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>
                    <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
                </Styles>
            </dx:ASPxGridView>
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

</div>
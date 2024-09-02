<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemResource.ascx.cs" Inherits="uGovernIT.Web.SystemResource" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .OptionsTable {
        margin-bottom: 5px;
    }

    /* ASPxImagreSlider styles */
    .dxisControl .dxis-nbItem {
        width: 130px;
        height: 150px;
        background-color: transparent;
    }

        .dxisControl .dxis-nbSelectedItem,
        .dxisControl .dxis-nbSelectedItem > div,
        .dxisControl .dxis-nbItem .dxis-nbHoverItem {
            display: none !important; /* Hide Selection Frame */
        }

    .url:visited {
        /*color: #660066;*/
    }

    .url:hover {
        color: #dd0000;
    }


    .CustomBottomMargin {
        margin-bottom: 5px;
    }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var absPath = "";
    var title = "";
    var width = "";
    var height = "";

    function cardclick(s, e) {

        var workflowValue = e.item.index;

        if (workflowValue == 0) {
            absPath = '<%=linkImpact  %>';
            title = 'Impact';
            width = '700';
            height = '500';
        }

        else if (workflowValue == 1) {
            absPath = '<%= linkSeverity %>';
            title = 'Severity';
            width = '700';
            height = '500';
        }
        else if (workflowValue == 2) {
            absPath = '<%= linkPriority %>';
            title = 'Priority';
            width = '700';
            height = '500';
        }

        else if (workflowValue == 3) {
            absPath = '<%= linkPriortyMapping %>';
            title = 'Priort Mapping';
            width = '700';
            height = '500';
        }
        else if (workflowValue == 4) {
            absPath = '<%= linkSLARule %>';
            title = 'SLA Rule';
            width = '1000';
            height = '700';
        }
        else if (workflowValue == 5) {
            absPath = '<%= linkEscalationRule %>';
            title = 'SLA Escalation';
            width = '800';
            height = '600';
        }

        UgitOpenPopupDialog(absPath, '', title, width, height, '', '', true);


    }

    function SystemResource(s, e) {

        var workflowValue = e.item.index;

        if (workflowValue == 0) {

            absPath = '<%=linkSMTPMailSetting %>';
            title = 'SMTP Credentials';
            width = '50';
            height = '60';
        }

        else if (workflowValue == 1) {
            absPath = '<%= linkUGITLog %>';
            title = 'Logs';
            width = '75';
            height = '66';
        }
        else if (workflowValue == 2) {
            absPath = '<%= linkDeleteTickets %>';
            title = 'Delete Tickets';
            width = '90';
            height = '90';
        }
        else if (workflowValue == 3) {
            absPath = '<%= cacheurl %>';
            title = e.item.text;
            width = '90';
            height = '90';
        }
        else if (workflowValue == 4) {
            absPath = '<%= licenseUrl %>';
            title = e.item.text;
            width = '90';
            height = '90';
        }

        else if (workflowValue == 5) {
            absPath = '<%= scheduleaction %>';
            title = e.item.text;
            width = '90';
            height = '90';
        }

        UgitOpenPopupDialog(absPath, '', title, width, height, false, '', false);
    }

    function UIConfiguration(s, e) {
        var workflowValue = e.item.index;

        if (workflowValue == 0) {
            absPath = '<%=linkMenuNavigationView  %>';
            title = 'Menu Navigation';
            width = '90';
            height = '90';
        }

        else if (workflowValue == 1) {
            absPath = '<%= linkPageEditorView %>';
            title = 'Page Editor';
            width = '90';
            height = '90';
        }
        else if (workflowValue == 2) {
            absPath = '<%= linkCustomControl %>';/*need to show msg*/
            title = 'Custom control';
            width = '90';
            height = '90';
        }
        UgitOpenPopupDialog(absPath, '', title, width, height, false, '', false);
    }

    function Resources(s, e) {

        var workflowValue = e.item.index;

        if (workflowValue == 0) {
            absPath = '<%=linkAssetModelView  %>';
            title = 'Asset Models';
            width = '90';
            height = '90';
        }

        else if (workflowValue == 1) {
            absPath = '<%= linkBgetCatView %>';
            title = 'Budget Categories';
            width = '90';
            height = '90';
        }
        else if (workflowValue == 2) {
            absPath = '<%= linkContract %>';/*need to show msg*/
            title = 'Contract';
            width = '90';
            height = '90';
        }
        else if (workflowValue == 3) {
            absPath = '<%= linkAssetVendorView %>';/*need to show msg*/
            title = 'Vendors';
            width = '90';
            height = '90';
        }
        UgitOpenPopupDialog(absPath, '', title, width, height, false, '', false);
    }




</script>

<div class="row">
   <div class="col-md-2 col-sm-2 col-xs-12 workflowTitle-wrap">
        <div class="workflowTitle-container">
            <%-- <h3 class="heading">System</h3>--%>
        </div>
    </div>
    <div class="col-md-10 col-sm-10 col-xs-12 workflowList-container">
        <ul class="workFlow-listWrap">
            <li class="workflow-list">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/NewAdmin/priorities-and-sales.png" />
                        <p class="workFlow-heading">Priorities and SLAs</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ImageSlider" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="4"
                            Theme="Default" NavigateUrlField="Link" NameField="ModuleName" CssClass="workflow-imageSlider">
                            <ItemThumbnailTemplate>
                                <a class="url">
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true"
                                        CssClass="aspxImage-sliderImg" Width="30px" />
                                    <div>
                                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top: 2px solid #B59B70;" size="8" class='<%# Container.EvalDataItem("HrClass") %>'/>
                                        <div class="workflowDot"></div>
                                    </div>
                                    <p class="module-name"><%# Container.EvalDataItem("Text") %></p>
                                </a>

                            </ItemThumbnailTemplate>

                            <SettingsBehavior ExtremeItemClickMode="Select" />
                            <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page"
                                ItemSpacing="0" />
                            <Styles Item-BackColor="White" Item-Border-BorderStyle="None">
                                <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                <Thumbnail CssClass="item"></Thumbnail>
                            </Styles>
                            <ClientSideEvents ThumbnailItemClick="cardclick" />
                        </dx:ASPxImageSlider>
                    </div>
                </div>
            </li>

            <li class="workflow-list listNo2">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/NewAdmin/system-resources.png" />
                        <p class="workFlow-heading">System Resources</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ASPxImageSlider1" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="4" Theme="Default" 
                            CssClass="workflow-imageSlider" NavigateUrlField="Link" NameField="ModuleName">
                            <ItemThumbnailTemplate>
                                <a class="url">
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true"
                                        CssClass="aspxImage-sliderImg" Width="30px" />
                                    <div>
                                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top: 2px solid #B59B70;" size="8" class='<%# Container.EvalDataItem("HrClass") %>' />
                                        <div class="workflowDot"></div>
                                    </div>
                                    <p class="module-name"><%# Container.EvalDataItem("Text") %></p>
                                </a>

                            </ItemThumbnailTemplate>

                            <SettingsBehavior ExtremeItemClickMode="Select" />
                            <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page"
                                ItemSpacing="0" />
                            <Styles Item-BackColor="White" Item-Border-BorderStyle="None">
                                <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                <Thumbnail CssClass="item"></Thumbnail>
                            </Styles>
                            <ClientSideEvents ThumbnailItemClick="SystemResource" />
                        </dx:ASPxImageSlider>

                    </div>
                </div>
            </li>

            <li class="workflow-list listNo2">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/NewAdmin/ui-configuration.png" />
                        <p class="workFlow-heading">Configuration</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ASPxImageSlider2" CssClass="workflow-imageSlider smallImage-slider" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false"
                            SettingsNavigationBar-VisibleItemsCount="4" Theme="Default" NavigateUrlField="Link" NameField="ModuleName">
                            <ItemThumbnailTemplate>
                                <a class="url">
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true"
                                        CssClass="aspxImage-sliderImg" Width="30px" />
                                    <div>
                                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top:2px solid #B59B70;" size="8" class='<%# Container.EvalDataItem("HrClass") %>'/>
                                        <div class="workflowDot"></div>
                                    </div>
                                    <p class="module-name"><%# Container.EvalDataItem("Text") %></p>
                                </a>

                            </ItemThumbnailTemplate>

                            <SettingsBehavior ExtremeItemClickMode="Select" />
                            <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page"
                                ItemSpacing="0" />
                            <Styles Item-BackColor="White" Item-Border-BorderStyle="None">
                                <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                <Thumbnail CssClass="item"></Thumbnail>
                            </Styles>
                            <ClientSideEvents ThumbnailItemClick="UIConfiguration" />
                        </dx:ASPxImageSlider>

                    </div>
                </div>
            </li>

            <li class="workflow-list listNo2">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/NewAdmin/resources.png" />
                        <p class="workFlow-heading">Resources</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ASPxImageSlider3" CssClass="workflow-imageSlider smallImage-slider" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false" 
                            SettingsNavigationBar-VisibleItemsCount="4" Theme="Default" NavigateUrlField="Link" NameField="ModuleName">
                            <ItemThumbnailTemplate>
                                <a class="url">
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true"
                                        CssClass="aspxImage-sliderImg" Width="30px" />
                                    <div>
                                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top: 2px solid #B59B70;" size="8" class='<%# Container.EvalDataItem("HrClass") %>'/>
                                        <div class="workflowDot"></div>
                                    </div>
                                    <p class="module-name"><%# Container.EvalDataItem("Text") %></p>
                                </a>

                            </ItemThumbnailTemplate>

                            <SettingsBehavior ExtremeItemClickMode="Select" />
                            <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page"
                                ItemSpacing="0" />
                            <Styles Item-BackColor="White" Item-Border-BorderStyle="None">
                                <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                <Thumbnail CssClass="item"></Thumbnail>
                            </Styles>
                            <ClientSideEvents ThumbnailItemClick="Resources" />
                        </dx:ASPxImageSlider>
                    </div>
                </div>
            </li>

        </ul>
    </div>
</div>




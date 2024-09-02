<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Governance.ascx.cs" Inherits="uGovernIT.Web.Governance" %>
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

    function DashBoardConfigLink(s, e) {

        var workflowValue = e.item.index;

        if (workflowValue == 0) {
            absPath = '<%=linkDashboardButtons %>';
            title = 'Dashboard Buttons';
            width = '80';
            height = '80';
        }

        else if (workflowValue == 1) {
            absPath = '<%= linkLinkViews %>';
            title = 'Link Views';
            width = '80';
            height = '80';
        }
        else if (workflowValue == 2) {
            absPath = '<%= linkADUserSync %>';
            title = 'AD User Sync';
            width = '80';
            height = '80';
        }

        UgitOpenPopupDialog(absPath, '', title, width, height, false, '', false);

    }

    function UserSurveyFeedback(s, e) {

        var workflowValue = e.item.index;

        if (workflowValue == 0) {
            absPath = '<%=configurance  %>';
            title = e.item.text;
            width = '90';
            height = '90';
        }

        else if (workflowValue == 1) {
            absPath = '<%= linkSurvey %>';
            title = "Survey";
            width = '90';
            height = '90';
        }
        else if (workflowValue == 2) {
            absPath = '<%= linkSurveyFeedback %>';
            title = "Survey Feedback";
            width = '90';
            height = '90';
        }

        UgitOpenPopupDialog(absPath, '', title, width, height, false, '', false);
    }


</script>

<div class="row">
    <div class="col-md-2 col-sm-2 col-xs-12 workflowTitle-wrap">
        <div class="workflowTitle-container">
          <%--  <h3 class="heading">Governance</h3>--%>
        </div>
    </div>
    <div class="col-md-9 col-sm-10 col-xs-12 workflowList-container">
        <ul class="workFlow-listWrap">
            <li class="workflow-list">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/workflow.png" />
                        <p class="workFlow-heading">IT Governance</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ImageSlider" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="4"
                            Theme="Default" NavigateUrlField="Link" NameField="ModuleName" CssClass="workflow-imageSlider smallImage-slider">
                            <ItemThumbnailTemplate>
                                <a class="url">
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true"
                                        CssClass="aspxImage-sliderImg" Width="30px" />
                                    <div>
                                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top: 2px solid #B59B70;" class='<%# Container.EvalDataItem("HrClass") %>' size="8" />
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
                            <ClientSideEvents ThumbnailItemClick="DashBoardConfigLink" />
                        </dx:ASPxImageSlider>
                    </div>
                </div>
            </li>
            <li class="workflow-list listNo2">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/workflow-support.png" />
                        <p class="workFlow-heading">Survey</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ASPxImageSlider1" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="4" Theme="Default" NavigateUrlField="Link" NameField="ModuleName" CssClass="workflow-imageSlider smallImage-slider">
                            <ItemThumbnailTemplate>
                                <a class="url">
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true"
                                        CssClass="aspxImage-sliderImg" Width="30px" />
                                    <div>
                                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top: 2px solid #B59B70;" class='<%# Container.EvalDataItem("HrClass") %>' size="8" />
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
                            <ClientSideEvents ThumbnailItemClick="UserSurveyFeedback" />
                        </dx:ASPxImageSlider>
                    </div>
                </div>
            </li>
        </ul>
    </div>
</div>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DashboardAndReports.ascx.cs" Inherits="uGovernIT.Web.DashboardAndReports" %>
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
    /* Template styles */
    .url {
        display: block;
        /*color: #0068bb;*/
        line-height: 1.2;
        /*//border: 11px solid #ffffff;*/
        /*padding: 20px 25px 25px 25px;*/
        padding: 48px 1px 1px 1px;
        text-align: center;
        -moz-box-sizing: border-box;
        box-sizing: border-box;
        width: 129px;
        height: 150px;
    }

        .url:visited {
            /*color: #660066;*/
        }

        .url:hover {
            color: #dd0000;
            /*border: 1px solid #dddddd;*/
            /*background-color: #f8f8f8;*/
        }

    .name {
        font-size: 13px;
        white-space: normal;
        font-family: helvetica, arial, sans-serif;
        font-family: 'Segoe UI', Helvetica, Tahoma, Geneva, sans-serif;
    }

    .CustomBottomMargin {
        margin-bottom: 5px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function cardclick(s, e) {
        var absPath = "";
        var title = "";
        var width = "";
        var height = "";

        var workflowValue = e.item.index;

        if (workflowValue == 0) {

            absPath = '<%=linkFactTable%>';
            title = 'Fact Table';
            width = '90';
            height = '70';
        }
        else if (workflowValue == 1) {
            absPath = '<%=linkDashboardAndQueries%>';
            title = 'Dashboard And Queries';
            width = '90';
            height = '90';
        }
        UgitOpenPopupDialog(absPath, '', title, width, height);
    }


</script>
<div class="row">
    <%--<h3 class="heading">Dashboard and queries</h3>--%>
    <div class="col-md-2 col-sm-1 hidden-xs"></div>
    <div class="col-md-10 col-sm-11 col-xs-12">

        <dx:ASPxImageSlider ID="ImageSlider" runat="server" EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="7" Theme="Default" NavigateUrlField="Link" NameField="ModuleName">
            <ItemThumbnailTemplate>
                <a class="url">
                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true" />
                    <b>
                        <hr style="margin-bottom: 3px; margin-top: 3px; border-top: 1px solid;" size="8" />
                    </b>
                    <span class="name"><%# Container.EvalDataItem("Text") %></span>

                </a>

            </ItemThumbnailTemplate>

            <SettingsBehavior ExtremeItemClickMode="Select" />
            <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page" ItemSpacing="0" />
            <Styles Item-BackColor="White" Item-Border-BorderStyle="None">
            </Styles>
            <ClientSideEvents ThumbnailItemClick="cardclick" />
        </dx:ASPxImageSlider>

    </div>
</div>

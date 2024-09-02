<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfigureUserAdmin.ascx.cs" Inherits="uGovernIT.Web.ConfigureUserAdmin" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">

.OptionsTable
{
    margin-bottom: 5px;
}
/* ASPxImagreSlider styles */
.dxisControl .dxis-nbItem
{
    width: 130px;
    height: 150px;
    background-color: transparent;
}
.dxisControl .dxis-nbSelectedItem,
.dxisControl .dxis-nbSelectedItem > div,
.dxisControl .dxis-nbItem .dxis-nbHoverItem
{
    display: none !important; /* Hide Selection Frame */
}
/* Template styles */
/*.url
{
    display: block;
    /*color: #0068bb;
    line-height: 1.2;
    /border: 11px solid #ffffff;
    /*padding: 20px 25px 25px 25px;
    padding:33px 1px 1px 1px;
    
    text-align: center;
    -moz-box-sizing: border-box; 
    box-sizing: border-box;
    width: 130px;
    height: 150px;
}*/
.url:visited
{
    /*color: #660066;*/
}
.url:hover
{
    color: #dd0000;
    /*border: 1px solid #dddddd;*/
    /*background-color: #f8f8f8;*/
}
/*.name
{
    font-size: 13px;
    white-space: normal;
    font-family: helvetica, arial, sans-serif;
    font-family: 'Segoe UI', Helvetica, Tahoma, Geneva, sans-serif;
}*/

.CustomBottomMargin 
        {
            margin-bottom: 5px;
        }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function cardclick(s,e) {
        


       var index = e.item.index;
         console.log(index);
         var absPath = "";
         var title = "";
         if (index == 0) {
             absPath = '<%= userskills %>';
             title = e.item.text;
             UgitOpenPopupDialog(absPath,'',title,'60','75','','');
         }
         else if (index == 1) {
              absPath = '<%= userroles %>';
             title = e.item.text;
             UgitOpenPopupDialog(absPath,'',title,'60','75','','');
         }
         else if (index == 2)
         {
            var message = "This " + e.item.text + " is not configured for you";
              alert(message);
         }
         else if (index == 3)
         {
             absPath = '<%= usermanagement %>';
             title = e.item.text;
             UgitOpenPopupDialog(absPath,'',title,'90','90','','');
         }
         else if (index == 4) {
             absPath = '<%= rolesview %>';
             title = e.item.text;
             UgitOpenPopupDialog(absPath, '', title, '90', '90', '', '');
         }
         else if (index == 5)
         {
              var message = "This " + e.item.text + "  is not configured for you";
              alert(message);
         }
          
    }

    function ListUserGroup(s,e) {


       var index = e.item.index;
         console.log(index);
         var absPath = "";
         var title = "";
         if (index == 0) {
              absPath = '<%= usermanagement %>';
             title = e.item.text;
             UgitOpenPopupDialog(absPath, '', title, '90', '90', '', '');
         }
         else if (index == 1)
         {
              var editGroupUrl = '<%= editGroupUrl %>';
              window.UgitOpenPopupDialog(editGroupUrl, '', 'Add New Group', "600px", "300px", false, escape("<%= Request.Url.AbsolutePath %>"));
         }
         else if (index == 2)
         {
              var message = "This " + e.item.text + "  option is not configured for you";
              alert(message);
              
         }
                 
    }


</script>

   <div class="row">
    <div class="col-md-2 col-sm-2 col-xs-12 workflowTitle-wrap">
        <div class="workflowTitle-container">
            <%-- <h3 class="heading">Resources</h3>--%>
        </div>
    </div>
    <div class="col-md-9 col-sm-10 col-xs-12 workflowList-container">
        <ul class="workFlow-listWrap">
            <li class="workflow-list">
                <div>
                    <div class="workflow-mainImg">
                        <img src="../../Content/Images/NewAdmin/manage-user.png" />
                        <p class="workFlow-heading">Manage Users</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                    <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ImageSliderManageUser" runat="server"  EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="4" 
                            Theme="Default" NavigateUrlField="Link" NameField="ModuleName" CssClass="workflow-imageSlider">
                                    <ItemThumbnailTemplate  >
                                    <a class="url" >
                                        <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true" 
                                            CssClass="aspxImage-sliderImg" Width="30px" />
                                        <div>
                                            <hr style="margin-bottom:3px;margin-top:3px; border-top: 2px solid #B59B70;" class='<%# Container.EvalDataItem("HrClass") %>' size="8"/>
                                            <div class="workflowDot"></div>
                                        </div>
                                        <p class="module-name"><%# Container.EvalDataItem("Text") %></p>
                                    </a>
                                </ItemThumbnailTemplate>
                                <SettingsBehavior ExtremeItemClickMode="Select"/>
                                <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page" ItemSpacing="0"/>
                                <Styles  Item-BackColor="White" Item-Border-BorderStyle="None" >
                                    <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                    <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                    <Thumbnail CssClass="item"></Thumbnail>
                                </Styles>
                            <ClientSideEvents ThumbnailItemClick="cardclick"/>
                        </dx:ASPxImageSlider>
                    </div>
                </div>
            </li>
            <li class="workflow-list listNo2"">
                <div>
                     <div class="workflow-mainImg">
                        <img src="../../Content/Images/NewAdmin/list-user-group.png" />
                         <p class="workFlow-heading">List User Group</p>
                    </div>
                    <div class="workflow-vrLine">
                        <div class="verticalLine-dot"></div>
                    </div>
                     <div class="module-workflow">
                        <dx:ASPxImageSlider ID="ImageSliderListUserGroup" runat="server"  EnableViewState="False" EnableTheming="False" ShowImageArea="false" SettingsNavigationBar-VisibleItemsCount="4" Theme="Default" NavigateUrlField="Link" NameField="ModuleName" CssClass="workflow-imageSlider smallImage-slider">
                            <ItemThumbnailTemplate  >
                                <a class="url" >
                                    <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' 
                                        ShowLoadingImage="true" CssClass="aspxImage-sliderImg" Width="30px" />
                                        <div>
                                            <hr style="margin-bottom:3px;margin-top:3px; border-top: 2px solid #B59B70;" class='<%# Container.EvalDataItem("HrClass") %>' size="8"/>
                                            <div class="workflowDot"></div>
                                        </div>
                                    <p class="module-name"><%# Container.EvalDataItem("Text") %></p>
                                </a>
                            </ItemThumbnailTemplate>
                            <SettingsBehavior ExtremeItemClickMode="Select" />
                            <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page" 
                                ItemSpacing="0"/>
                            <Styles  Item-BackColor="White" Item-Border-BorderStyle="None">
                                    <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                    <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                    <Thumbnail CssClass="item"></Thumbnail>
                            </Styles>
                            <ClientSideEvents ThumbnailItemClick="ListUserGroup"/>
                        </dx:ASPxImageSlider>
                    </div>
                </div>
            </li>
        </ul>
    </div>
 </div>  


      




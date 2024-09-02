<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InitialSetUp.ascx.cs" Inherits="uGovernIT.Web.InitialSetUp" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx"%>
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
    border: 11px solid #ffffff
    /*padding: 20px 25px 25px 25px;
    padding:48px 1px 1px 1px;
    
    text-align: center;
    -moz-box-sizing: border-box; 
    box-sizing: border-box;
    width: 129px;
    height: 150px;
}
.url:visited
{
    /*color: #660066;
}*/
/*.url:hover
{
    color: #dd0000;
    /*border: 1px solid #dddddd;*/
    /*background-color: #f8f8f8;
}*/
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
    function cardclick(s, e){
         var index = e.item.index;
         console.log(index);
         var absPath = "";
         var title = "";

        if (index == 0)
        {
            UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configvar', '', 'Configuration Variables', '90', '90', false, '', false);

        }
        else if (index == 1)
        {
            UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduleview','','Modules','80','80', false, '', false)
             
         }
         else if (index == 2)
         {
           UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission','','Form Layout','90','92','','')
         }
        if (index == 3) {
           UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsview','','Request Lists','70','90','','')
         }
        if (index == 4) {
            UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=requesttype', '', 'Request Types', '90', '90', false, '', false);
         }
        if (index == 5)
        {
            UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deptview', '', 'Departments', '90', '80', false, '', false);
         }
        if (index == 6)
        {
           UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=funcareaview','','Functional Areas','90','90','','')
        }
        if (index == 7)
        {
          UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=locationview','','Locations','80','90','','')
        }
        if (index == 8)
        {
            UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduledefaultvalues', '', 'Module Defaults', '90', '90', false, '', false);
        }
        if (index == 9)
        {
          UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=phrasesView','','Phrases','90','90', false, '', false)

        }

    }

    </script>

<div id="newDiv" >
    <%--<h3 class="service-title">SERVICE PRIME SET-UP</h3>--%>
    <div class="col-md-12 col-sm-12 col-xs-12 worflow-wrap">
         <dx:ASPxImageSlider ID="ImageSlider" runat="server"  EnableViewState="False" EnableTheming="False" ShowImageArea="false" 
                SettingsNavigationBar-VisibleItemsCount="6" Theme="Default" NavigateUrlField="Link" Width="100%"
                NameField="ModuleName" CssClass="workflow-imageSlider servicePrime-workFlow" >
                <SettingsImageArea ImageSizeMode="ActualSizeOrFit" NavigationButtonVisibility="Always" EnableLoopNavigation="true" />
                <ItemThumbnailTemplate >
                        <a class="url servicePrime-url">
                            <div class="service-image">
                                <dx:ASPxImage runat="server" ImageUrl='<%# Container.EvalDataItem("Image") %>' AlternateText='<%# Container.EvalDataItem("Image") %>' ShowLoadingImage="true" 
                                    CssClass="aspxImage-sliderImg" Width="40px" />
                                <%--<img src="/Content/Images/configurationVariable.png">--%>
                            </div>
                            <div class="vr-line">
                                <span class="dot"></span>
                            </div>
                            <div class="ServiceModule-content">
                                <p class="Service-title"><%# Container.EvalDataItem("Text") %></p>
                                <p class="service-discription"><%# Container.EvalDataItem("Desc") %></p>
                            </div>
                        </a>
             
                    </ItemThumbnailTemplate>

                        <SettingsBehavior ExtremeItemClickMode="Select"/>
                        <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="OnMouseOver" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Page" 
                            ItemSpacing="50"/>
                        <Styles  Item-BackColor="White" Item-Border-BorderStyle="None" >
                            <NextPageButtonHorizontalOutside CssClass="servicePrime-nextArrow"></NextPageButtonHorizontalOutside>
                            <PrevPageButtonHorizontalOutside CssClass="servicePrime-prevArrow"></PrevPageButtonHorizontalOutside>
                        </Styles>
                    <ClientSideEvents ThumbnailItemClick="cardclick"/>
            </dx:ASPxImageSlider>
    </div>
    <%--<div class="col-md-4 col-sm-7 col-xs-12 main-container">
        <ul class="module-list">
            <li class="list">
                <a onclick="javascrpt:UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=configvar','','Configuration Variables','80','80', false, '', false)">
                    <div class="image">
                        <img src="/Content/Images/configurationVariable.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <p class="module-title">Configuration Variable</p>
                        <p class="module-discription">Thease control the operation of Core</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduleview','','Modules','80','80', false, '', false)">
                    <div class="image">
                        <img src="/Content/Images/modules.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Modules</h5>
                        <p class="module-discription">Set up modules and configure them for the specific tenant.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=formlayoutandpermission','','Form Layout','90','90','','')">
                    <div class="image">
                        <img src="/Content/Images/formLayout.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Form Layout</h5>
                        <p class="module-discription">Design the forms for the tenant from the pre-bulid out of the box forms.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=modulecolumnsview','','Request Lists','70','90','','')">
                    <div class="image">
                        <img src="/Content/Images/requestList.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Request List</h5>
                        <p class="module-discription">Define the specific request list which display summary forms on the landing pages.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=requesttype','','Request Types','90','90', false, '', false)">
                    <div class="image">
                        <img src="/Content/Images/requesttype.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Request Types</h5>
                        <p class="module-discription">Configure applicable request types for the modules using pre-build request types.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=deptview','','Departments','1000px','90', false, '', false)">
                    <div class="image">
                        <img src="/Content/Images/organization.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Orginzation</h5>
                        <p class="module-discription">Set up the tenant Departments.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=funcareaview','','Functional Areas','70','70','','')">
                    <div class="image">
                        <img src="/Content/Images/functionalArea.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Functional Areas</h5>
                        <p class="module-discription">Set up the functional areas for the tenant.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=locationview','','Locations','90','90','','')">
                    <div class="image">
                        <img src="/Content/Images/locations.png">
                    </div>
                    <div class="vr-line">
                        <span class="dot"></span>
                    </div>
                    <div class="module-content">
                        <h5 class="module-title">Locations</h5>
                        <p class="module-discription">Set up the geographic locations.</p>
                    </div>
                </a>
            </li>
            <li class="list">
                <a onclick="window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/uGovernITConfiguration.aspx?control=moduledefaultvalues','','Module Defaults','80','80', false, '', false)">
                    <div class="image">
                        <img src="/Content/Images/moduleDefault.png">
                    </div>
                    <div class="lastDot"></div>
                    <div class="module-content lastContent">
                        <h5 class="module-title">Module Defaults</h5>
                        <p class="module-discription">Type your text hear.</p>
                    </div>
                </a>
            </li>

        </ul>
    </div>--%>
</div>
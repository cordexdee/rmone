<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserService.ascx.cs" Inherits="uGovernIT.Web.UserService" %>
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


.dxgvTable_UGITNavyBlueDevEx {
        background-color: #f6f7fb !important;
}

</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function documents(s,e) {
        

        var Servicesid =  e.item.name;
        var absPath = '<%= newServiceURL %>' + Servicesid;

        UgitOpenPopupDialog(absPath, '', e.item.text, '95', '95', 0, "<%=Server.UrlEncode(Request.Url.AbsolutePath)%>");

        //UgitOpenPopupDialog(absPath, '', title, '90', '90', '', '');
    }

    </script>

<div class="row">
    <div class="col-md-2 col-sm-2 col-xs-12 workflowTitle-wrap">
        <div class="workflowTitle-container">
<%--             <h3 class="heading">Technologies Enabled Services</h3>--%>
        </div>
    </div>
    <div class="col-md-9 col-sm-10 col-xs-12 workflowList-container">

                <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="ID"
         Width="100%" OnHtmlRowCreated="grid_HtmlRowCreated" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared" 
    Border-BorderStyle="None" Settings-VerticalScrollBarMode="Hidden" 
    
    
    BorderBottom-BorderStyle="None" >
        <Columns>
            <dx:GridViewDataColumn CellStyle-BorderRight-BorderColor="#B59B70" CellStyle-BorderRight-BorderWidth="2px" CellStyle-BorderBottom-BorderStyle="None" 
                CellStyle-HorizontalAlign="Right" CellStyle-VerticalAlign="Top">
                <DataItemTemplate >
                     <div class="workflow-mainImg">
                        <img src="/Content/Images/workflow.png" />
                         <div>
                              <dx:ASPxLabel runat="server" Text='<%# Eval("CategoryName") %>' Font-Bold="true" CssClass="workFlow-heading" Font-Size="14px"/>
                         </div>
                      
                    </div>
                </DataItemTemplate>
            </dx:GridViewDataColumn>
                
            <dx:GridViewDataColumn  CellStyle-BorderBottom-BorderStyle="None" CellStyle-VerticalAlign="Bottom">
                <DataItemTemplate >
                   
                     <dx:ASPxImageSlider ID="ASPxImageSliderGrid" runat="server"  EnableViewState="False" EnableTheming="False" ShowImageArea="false" 
                                        SettingsNavigationBar-VisibleItemsCount="3" Theme="Default" NavigateUrlField="Link" 
                                        NameField="ID" CssClass="workflow-imageSlider smallImage-slider">
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
                                    <SettingsNavigationBar ThumbnailsModeNavigationButtonVisibility="Always" ThumbnailsNavigationButtonPosition="Outside" PagingMode="Single" 
                                        ItemSpacing="0"/>
                                    <Styles  Item-BackColor="White" Item-Border-BorderStyle="None" >
                                        <NextPageButtonHorizontalOutside CssClass="workflow-nextArrow"></NextPageButtonHorizontalOutside>
                                        <PrevPageButtonHorizontalOutside CssClass="workflow-prevArrow"></PrevPageButtonHorizontalOutside>
                                        <Thumbnail CssClass="item"></Thumbnail>
                                    </Styles>
                                    <ClientSideEvents ThumbnailItemClick="documents"/>
                                    </dx:ASPxImageSlider>

                </DataItemTemplate>
            </dx:GridViewDataColumn>

        </Columns>
        <Templates>
            <DetailRow>

            </DetailRow>
        </Templates>
    <Styles >
        <Header BackColor="#f6f7fb" Border-BorderStyle="None" ></Header>
        
    </Styles>
</dx:ASPxGridView>
     </div>
    </div>
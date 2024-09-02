
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceCatalog.ascx.cs" Inherits="uGovernIT.Web.ServiceCatalog" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style type="text/css" data-v="<%=UGITUtility.AssemblyVersion %>">
    /*.categorylist-container{
        width:200px !important;
    }*/

    /*.categorylist-container .servicecategory{
        border: 1px solid #bbb !important;
        vertical-align: middle;
        height: 180px;
        cursor:pointer;
        background: linear-gradient(#fefefe, #fff, #fff, #fff, #fff, #fefefe);
        box-shadow: inset 0 0 42px 0 rgba(0, 0, 0, 0.196);
    }*/
    .panelfloatleft{
        float:left;
        margin:10px;
        background:white;
        min-height:128px;
    }
    .largeiconserviceitem-header{
        border-bottom:1px solid lightgray;
        margin-left:10px;
        color:darkgray;
    }
    .servicecategoryLargeIcon{
        border: 1px solid #bbb !important;
        vertical-align: middle;
        height: 180px;
        cursor:pointer;
        background: linear-gradient(#fefefe, #fff, #fff, #fff, #fff, #fefefe);
        box-shadow: inset 0 0 42px 0 rgba(0, 0, 0, 0.196);
    }
    .categorylist-container-IconView{
        vertical-align: middle;
        height: 128px;
        cursor:pointer;
        color:blue;
        background:white;
        width:236px !important;
    }
    .serviceitemlist-container .dxpc-content{
        padding: 0 0 0 0 !important;
    }

    .serviceitemlist-container .serviceitem-noicon {
        padding-left: 7px !important;
    }
    /*.serviceitemlist-container .serviceitem{
        padding-left: 5px !important;
        padding-right: 5px !important;
    }
    .serviceitemlist-container .serviceitem:hover{
        background:#eee;
    }*/

    /*.buttonview-container .ugit-contentcontainer {
        background-color: #fff !important;
    }*/

    .servciecatalog-main input[type="button"]:hover {
        color: #fff;
        background-image: -webkit-linear-gradient(top, rgba(63, 79, 87, 0.56) 0%, #3f4f57 100%);
        background-image:      -o-linear-gradient(top, rgba(63, 79, 87, 0.56) 0%, #3f4f57 100%);
        background-image: -webkit-gradient(linear, left top, left bottom, from(rgba(63, 79, 87, 0.56)), to(#3f4f57));
        background-image:         linear-gradient(to bottom, rgba(63, 79, 87, 0.56) 0%, #3f4f57 100%);
        filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='rgba(63, 79, 87, 0.56)', endColorstr='#3f4f57', GradientType=0);
        filter: progid:DXImageTransform.Microsoft.gradient(enabled = false);
        background-repeat: repeat-x;
        border:none;
        cursor: pointer;
    }
    /*.serviceitem-header{
        background: #eee !important;
        box-shadow: inset 0 0 5px 0 rgba(0, 0, 0, 0.196);
    }*/

    /*.servciecatalog-main .categorylist{
        width: 300px !important;
    }*/

   .serviceitemlist {
       max-height: 250px;
       overflow-y: auto;
   }
</style>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    function goService() {
        var selectedServiceID = cmbServices.GetValue();

        if (cmbServices.GetSelectedItem().text.length > 0 && $.trim(selectedServiceID) != '') {
            var selectedServiceTitle = cmbServices.GetSelectedItem().text;
            var url = "<%= newServiceURL %>" + selectedServiceID;
            UgitOpenPopupDialog(url, '', selectedServiceTitle, 90, 90, false, '');
        }
    }
    //Hide and expand the services on UI
    function showMoreServices(obj) {
        if ($.trim($(obj).text()) == "More >>") {
            $(obj).parent().parent().find(".expandableservice").show();
            $(obj).html("<< Less");
        }
        else {

            $(obj).parent().parent().find(".expandableservice").hide();
            $(obj).html("More >>");
        }
    }

    function showMoreServicePanel() {
        if ($(".populate-services").is(":hidden")) {
            $(".populate-services").slideDown("medium");
            var parentDiv = $("#moreServiceLink").parent();
            parentDiv.html("<span style='font-weight:bold;'>All Services:</span>");
            parentDiv.css("border-bottom", "1px solid black");
        }
    }

    $(function () {
        var serviceBlocks = $(".service-catalog .pcategorycontainer");
        var groupCount = serviceBlocks.length / 3;
        for (var i = 0; i < groupCount; i++) {
            var startIndex = 3 * i;
            var endIndex = startIndex + 2;
            if (endIndex > serviceBlocks.length) {
                endIndex = serviceBlocks.length - 1;
            }

            var maxHeight = 0;
            var tempIndex = startIndex;
            while (tempIndex <= endIndex) {
                var height = $(serviceBlocks[tempIndex]).height();
                if (maxHeight < height)
                    maxHeight = height;

                tempIndex += 1;
            }

            tempIndex = startIndex;
            maxHeight += 15;
            /*while (tempIndex <= endIndex) {
                $(serviceBlocks[tempIndex]).css("min-height", maxHeight + "px");
                tempIndex += 1;
            }*/
        }

        try {
            $(".jqtooltip").tooltip({
                hide: { effect: "fadeOut", easing: "easeInExpo" },
                content: function () {
                    var title = unescape($(this).attr("title"));
                    if (title)
                        return title.replace(/\n/g, "<br/>");
                },
                open: function (e, ui) {
                    ui.tooltip.css("max-width", "500px");
                }
            });
        }
        catch (ex) {
        }
    });

    function showLoadingServerCatalog() {
        if ($("#populate_services").length > 0) {
            loadingPanelServiceCatalog.ShowInElement($("#populate_services").get(0));
            var top, left;
            top = $(loadingPanelServiceCatalog.GetLoadingDiv()).position().top;
            left = $(loadingPanelServiceCatalog.GetLoadingDiv()).position().left;
            $(loadingPanelServiceCatalog.GetLoadingDiv()).css({ "top": top - 60 + "px", "left": 0 + "px" });

            top = $(loadingPanelServiceCatalog.GetMainElement()).position().top;
            left = $(loadingPanelServiceCatalog.GetMainElement()).position().left;
            $(loadingPanelServiceCatalog.GetMainElement()).css({ "top": top - 60 + "px", "left": left - 150 + "px" });
        }
    }

    function ShowHideListMenu(obj, event) {
        var currentCategoryControlId = categoryMenuListContainer.GetPopupElementIDList()[0];

        if (currentCategoryControlId == obj.id && !categoryMenuListContainer.IsVisible()) {
            categoryMenuListContainer.Hide();
            return false;
        }

        var parentDiv = $('#serviceItemList');
        parentDiv.html("");

        var serviceItemListContainer = $(obj).parent().parent().find('div.category' + obj.getAttribute("categoryid") + '_Service');
        parentDiv.html($(serviceItemListContainer[0]).html());

        categoryMenuListContainer.SetHeaderText(obj.innerText.trim());
        categoryMenuListContainer.ShowAtPos(event.clientX, event.clientY);
    }

    function HideServiceMenu() {
        categoryMenuListContainer.Hide();
    }
    $(document).ready(function () {
        $(".setTopauto").parent().addClass("serviceCatParent SetTopAutoMainDiv");
    });
</script>

<div class="servciecatalog-main servcieCatalog_listView" id="pServiceCatelogMain" runat="server">
    <div class="ugit-contentcontainer">
            <div style="font-weight: bold; float: left; width: 99%; padding: 15px 0 5px 3px; position: relative; top: -13px; display:none;">
                <p class="service-toptitle-sub"><b>Welcome <%= Convert.ToString(((uGovernIT.Utility.Entities.UserProfile)HttpContext.Current.Items["CurrentUser"]).Name) %> !</b> Please click on a <%= ServiceCatalogViewMode == "ButtonView" ? "service category" : "service" %> from one of the categories below:</p>
            </div>
                <div id="ServiceCatalog1" runat="server">
                    <div id="service_catalog" class="service-catalog row dashboardService_catalogeContainer <%= ServiceCatalogViewMode == "DropdownView" ? "hide" : "" %>">
                        <asp:Repeater ID="rptSubCategory" runat="server" OnItemDataBound="RptSubCategory_ItemDataBound">
                        <ItemTemplate>
                            <asp:Panel ID="pCategoryContainer" runat="server" CssClass="pcategorycontainer col-xs-12 col-md-4 col-sm-4">
                                <div class="row">
                                    <div id="tdCategoryContainer" runat="server" categoryid='<%#Eval("ID")%>' class="servicecategory serviceBoard_btnView" 
                                        valign="top" style="text-align: center;">
                                        <div class="serviceBoard_imgesWrap">
                                            <img id="imgCategory" runat="server" width="50" src='<%#Eval("ImageUrl")%>' alt="" />
                                        </div>
                                         <div class="serviceBtnView-labelWrap">
                                            <asp:Label ID="lblSubCategroy" runat="server" Text='<%#Eval("CategoryName")%>' CssClass="serviceBoard_lblView"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-12 col-sm-12 col-xs-12 noPadding service_listView servicetype <%# ServiceCatalogViewMode == "ButtonView" || ServiceCatalogViewMode == "DropdownView" ? "hide" : "" %>">
                                        <div id="serviceListContainer" runat="server" class='<%# "category" + Eval("ID") + "_Service"%>'>
                                            <asp:Repeater ID="rptItemList" runat="server" OnItemDataBound="RptItemList_ItemDataBound">
                                            <HeaderTemplate>
                                                <table style="width: 100%;">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div id="trServiceItem" runat="server" class="serviceItem-row">
                                                    <%--<td class="serviceitem" style='border-bottom: 1px solid black; padding:0px;'>--%>
                                                    <div class='serviceitem' style='<%# !ShowServiceIcons ? "padding:2px 0px;": ""%>'>
                                                        <asp:LinkButton ID="lnkListName" runat="server">
                                                            <img src='<%# uGovernIT.Utility.UGITUtility.GetAbsoluteURL(Convert.ToString(Eval("ImageUrl")))%>' style='float: left; margin-right: 5px; max-height:20px; max-width:20px; display:<%# !ShowServiceIcons || string.IsNullOrWhiteSpace(Convert.ToString(Eval("ImageUrl"))) ? "none" : "block"%>' /> 
                                                            <span style="vertical-align:middle; display:block; <%# ServiceCatalogViewMode == "ButtonView" && (!ShowServiceIcons) ? "padding-left:2px;" : "" %>"> <%#Eval("Title")%></span> 
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>                                          
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        </div>
                                        <div class="sc-excp" id="hideAndExpand" runat="server">
                                            <a id="moreServiceDisplayLink" href="javascript:void(0);" onclick="showMoreServices(this);" runat="server" style="cursor: pointer;">More >>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                </asp:Repeater>

            </div>
            </div>
                <div id="divLargeServiceIcon" runat="server" style="font-weight:bold; float:left;" visible="false">
                    <asp:Repeater ID="rptlargeserviceicon" runat="server" OnItemDataBound="rptlargeserviceicon_ItemDataBound">
                        <ItemTemplate>
                            <asp:Panel ID="pLargeServiceContainer" runat="server" Direction="LeftToRight">
                                <div class="row">
                                    <div id="tdLargeCategoryContainer" runat="server" categoryid='<%#Eval("ID")%>'>
                                        <div class="largeiconserviceitem-header" valign="top" style="font-size:medium; width:300px;text-align: initial;">
                                            <asp:Label ID="lblLargeSubCategroy" runat="server" Text='<%#Eval("CategoryName")%>'></asp:Label>
                                        </div>
                                        <div class="servciecatalog-main buttonview-container" valign="top" style="text-align: center;overflow:hidden;">
                                            <asp:Repeater ID="rptServiceIcon" runat="server" OnItemDataBound="rptServiceIcon_ItemDataBound" >
                                                <HeaderTemplate>
                                                    <table style="width: 100%;">
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Panel ID="pServiceIconContainer" runat="server" CssClass="categorylist-container-IconView panelfloatleft">
                                                        <div style="margin:20px 10px 10px 10px;">
                                                        <asp:LinkButton ID="lnkLargeIconListName" runat="server"> 
                                                            <span style="vertical-align:middle; display:block;color:blue; "><b> <%#Eval("Title")%> </b></span>
                                                            <br />
                                                            <img id="imgServiceIcon" runat="server" onerror="this.src='/Content/ButtonImages/brokenimage.png'" />
                                                        </asp:LinkButton>
                                                        </div>
                                                     </asp:Panel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </table>                                          
                                                </FooterTemplate>
                                            </asp:Repeater>
                                         </div>
                                    </div>
                                 </div>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            <div style="float: left; padding: 5px 0px 1px 5px; width: 97%; margin: 0px 3px; display:none;" id="moreServiceLinkContainer">
                <a id="moreServiceLink" href="javascript:void(0);" onclick="showMoreServicePanel()" style="cursor: pointer;">
                    <h3>More Services ...</h3>
                </a>
            </div>

            <div class="populate-services row" id="populate_services" runat="server" >
                <asp:UpdatePanel ID="categoryUpdatePanel" runat="server" UpdateMode="Conditional">
                    <Triggers></Triggers>
                    <ContentTemplate>
                        <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Loading..." ClientInstanceName="loadingPanelServiceCatalog"
                            Modal="True">
                        </dx:ASPxLoadingPanel>
                        <asp:Panel ID="populatePanel" runat="server" CssClass="service-moreservices dashboardServices_container">
                            <div class="ms-formtable accomp-popup col-md-5 col-sm-6 col-xs-12">
                                <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Category:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxComboBox ID="cmbCategories" runat="server" OnSelectedIndexChanged="DDLCategories_SelectedIndexChanged" AutoPostBack="true" 
                                        CssClass="aspxComBox-dropDown" ValueField="ID" TextField="CategoryName" NullText="--Select Category--"
                                        DropDownStyle="DropDown" ValueType="System.String" ListBoxStyle-CssClass="aspxComboBox-listBox" Width="100%">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ showLoadingServerCatalog();}" />
                                        <%--<ItemStyle>
                                            <BorderTop BorderColor="LightGray" BorderWidth="1px" BorderStyle="Solid" />
                                        </ItemStyle>
                                        <ItemImage Height="20px" Width="20px">                                            
                                        </ItemImage>--%>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="ms-formtable accomp-popup col-md-5 col-sm-6 col-xs-12">
                               <div class="ms-formlabel">
                                    <h3 class="ms-standardheader budget_fieldLabel">Service:</h3>
                                </div>
                                <div class="ms-formbody accomp_inputField">
                                    <dx:ASPxComboBox ID="cmbServices" runat="server" ClientInstanceName="cmbServices" CssClass="aspxComBox-dropDown" Width="100%"
                                        ValueField="ID" TextField="Title" DropDownStyle="DropDown" ListBoxStyle-CssClass="aspxComboBox-listBox" ValueType="System.String">
                                       <%-- <ItemStyle>
                                            <BorderTop BorderColor="LightGray" BorderWidth="1px" BorderStyle="Solid" />
                                        </ItemStyle>--%>
                                    </dx:ASPxComboBox>
                                </div>
                            </div>
                            <div class="dashboard_goBtn_wrap col-md-2 col-sm-6 col-xs-12">
                                <input type="button" class="ugit-button dashboard_goBtn" value="Go" onclick="goService()"/>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
   

    <dx:ASPxPopupControl ID="categoryMenuListContainer" runat="server" CloseAction="OuterMouseClick" ShowCloseButton="false" AllowDragging="false"
        ClientInstanceName="categoryMenuListContainer" ShowFooter="false" ShowHeader="true" PopupVerticalAlign="Below" PopupHorizontalAlign="LeftSides"
        Height="55px" Width="250px" CssClass="serviceitemlist-container setTopauto">
        <HeaderStyle Font-Bold="true" ForeColor="Black" CssClass="serviceitem-header" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div id="serviceItemList" class="serviceitemlist">
                    
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</div>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GuideMe.aspx.cs" Inherits="uGovernIT.Web.SitePages.GuideMe" MasterPageFile="~/master/Main.master" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<asp:Content runat="server" ID="contentGuideMe" ContentPlaceHolderID="ContentPlaceHolderContainer">
<asp:PlaceHolder runat="server">
   <%= UGITUtility.LoadScript("../../Scripts/HelpCardDisplayPopup.js") %>
</asp:PlaceHolder>   
<script type="text/javascript">
    var helpCardTiles = '';
    var HelpCards = [];
    var HelpCardWidgets = []
    var dxCategoryList = []
    var filteredSearchCard = [];
    var alllistCategory = [];
    var defaultCategory = [];
    var widgetCategoryJquery = '<%=widgetCategoy %>';
    var isNoOfCardsToShowIsGreaterThenThree = false;
    var switchViewLimit = 3;
    var switchBaseItemHeight = 495;
    var switchBaseItemWidth = 341;

    function forceDocumentLoadCodeForFirefox() {

        DrawCategory();
        drawHelpCardTiles();

        var helpSearchBox = $(".helpCardContainer").dxTextBox({
            placeholder: "Search For Help",
            stylingMode: "filled",
            buttons: [{
                name: "password",
                location: "after",
                options: {
                    icon: "/Content/images/searchNew.png",
                    type: "default",
                    onClick: function (e) {
                        $(".divClearSearch").show();
                        const searchString = helpSearchBox.option("value");
                        filteredSearchCard = HelpCards.filter(x => x.HelpCardCategory.toLowerCase().includes(searchString.toLowerCase()) || x.HelpCardTitle.toLowerCase().includes(searchString.toLowerCase()));
                        var CategoryAndGroupBy = _.groupBy(filteredSearchCard, 'HelpCardCategory');
                        var arrFilteredCategory = Object.keys(CategoryAndGroupBy);
                        arrFilteredCategory != null ? arrFilteredCategory.unshift("All Categories") : {};
                        dxCategoryList.option('dataSource', arrFilteredCategory);
                        dxCategoryList.option('selectedItems', ["All Categories"]);
                        //SetbaseWidthHeightAndView(filteredSearchCard);
                        helpCardTiles.option('dataSource', filteredSearchCard);
                    }
                }
            }]
        }).dxTextBox("instance");

        $(".divClearSearch").hide();
    }

    $(document).ready(function () {
        
        

        var helpSearchBox = $(".helpCardContainer").dxTextBox({
            placeholder: "Search For Help",
            stylingMode: "filled",
            buttons: [{
                name: "password",
                location: "after",
                options: {
                    icon: "/Content/images/searchNew.png",
                    type: "default",
                    onClick: function (e) {
                        $(".divClearSearch").show();
                        const searchString = helpSearchBox.option("value");
                        filteredSearchCard = HelpCards.filter(x => x.HelpCardCategory.toLowerCase().includes(searchString.toLowerCase()) || x.HelpCardTitle.toLowerCase().includes(searchString.toLowerCase()));
                        var CategoryAndGroupBy = _.groupBy(filteredSearchCard, 'HelpCardCategory');
                        var arrFilteredCategory = Object.keys(CategoryAndGroupBy);
                        arrFilteredCategory != null ? arrFilteredCategory.unshift("All Categories") : {};
                        dxCategoryList.option('dataSource', arrFilteredCategory);
                        dxCategoryList.option('selectedItems', ["All Categories"]);
                        SetbaseWidthHeightAndView(filteredSearchCard);
                        helpCardTiles.option('dataSource', filteredSearchCard);
                    }
                }
            }]
        }).dxTextBox("instance");

        $(".divClearSearch").hide();

        $.get("/api/HelpCard/GetCategories?isShowArchive=false", function (data, status) {
            alllistCategory = data;
            alllistCategory != null ? alllistCategory.unshift("All Categories") : {};
            defaultCategory = alllistCategory.filter(x => x.toLowerCase() == widgetCategoryJquery.toLowerCase());

            DrawCategory();
            //help card
            $.getJSON("/api/HelpCard/GetHelpCards?Category=All Categories&isShowArchive=false", function (data) {
                if ((data)) {
                    debugger;
                    drawHelpCardTiles();
                    HelpCards = data;
                    HelpCardWidgets = HelpCards.filter(x => x.HelpCardCategory.toLowerCase() == widgetCategoryJquery.toLowerCase());


                    defaultCategory = alllistCategory.filter(x => x.toLowerCase() == widgetCategoryJquery.toLowerCase());
                    if (defaultCategory == null || defaultCategory.length == 0) {
                        SetbaseWidthHeightAndView(HelpCards);
                        helpCardTiles.option('dataSource', HelpCards);
                    }
                    else {
                        SetbaseWidthHeightAndView(HelpCardWidgets);
                        helpCardTiles.option('dataSource', HelpCardWidgets);
                    }
                    dxCategoryList.option('dataSource', alllistCategory);
                    if (defaultCategory != null && defaultCategory.length != 0) {
                        //dxCategoryList.option('selectedItems', defaultCategory);
                    }
                    else {
                        //dxCategoryList.option('selectedItems', ["All Categories"]);
                    }

                }
            });

            //help card end
        });
    });

    function drawHelpCardTiles() {
        helpCardTiles = $('.HelpCardTiles').dxTileView({
            noDataText: "",
            height: '100%',
            paginate: true,
            direction: "vertical",
            allowItemDeleting: true,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                
                var html = new Array();
                if (isNoOfCardsToShowIsGreaterThenThree) {
                    html = new Array();
                    html.push("<div class='row wikiTitle-container'>");
                    html.push("<div class='helpCard-titleWrap'>");
                    html.push(itemData.HelpCardTitle);
                    html.push("</div>");
                    html.push("</div>");
                    html.push("<div class='wilkiInfo-container'>");
                    html.push(itemData.Description);
                    html.push("</div>");
                    var agentDiv = $(`<div class='cardFooter-Icon' id=agentsDropDown_${itemData.HelpCardTicketId} ></div>`);
                    var attachmentDiv = $(`<div class='cardFooter-Icon' id=attachmentDropDown_${itemData.HelpCardTicketId} class="attachmentdiv" ></div>`);
                    var openContent = $(`<div class="showcarddiv" title='View' onclick="openHelpCard('${itemData.HelpCardTicketId}', '');"><img id=agentsDropDown_${itemData.HelpCardTicketId} src="/Content/Images/focusView.png" width='18px'/></div>`);
                    var actionItemsDiv = $(`<div class='actionItemsDiv'><hr class='hrDivider' /></div>`);
                    //agents
                    itemElement.append(html.join(""));
                    if (itemData.AgentLookUp)
                        actionItemsDiv.append(agentDiv);
                    var agentHtmlContent = jQuery.parseHTML(itemData.AgentContent);
                    var agentArray = new Array();
                    $(agentHtmlContent).find('div').each(function () {
                        var imgAndOnclick = {};
                        imgAndOnclick.text = $(this).attr('title');
                        imgAndOnclick.Onclick = $(this).attr('onclick');
                        agentArray.push(imgAndOnclick);
                    });
                    //attachments
                    var content = jQuery.parseHTML(itemData.HelpCardContent);
                    if ($(content).find('a').length)
                        actionItemsDiv.append(attachmentDiv);
                    var attachmentArray = new Array();
                    $(content).find('a').each(function () {
                        var hrefAndTitle = {};
                        const href = $(this).attr('href');
                        hrefAndTitle.text = $(this).text();
                        hrefAndTitle.Url = $(this).attr('href');
                        attachmentArray.push(hrefAndTitle);
                    });
                    actionItemsDiv.append(openContent);
                    itemElement.append(actionItemsDiv);

                    if (itemData.AgentLookUp) {
                        $("#agentsDropDown_" + itemData.HelpCardTicketId).dxDropDownButton({
                            icon: "user",
                            hint: "Agent",
                            showArrowIcon: false,
                            dropDownOptions: {
                                width: 230
                            },
                            onItemClick: function (e) {
                                event.stopPropagation();
                            },
                            itemTemplate: function (data, index, element) {
                                var anchor = $(`<div onclick='${data.Onclick};event.stopPropagation();'><a>${data.text}</a><div/>`);
                                element.append(anchor);
                            },
                            items: agentArray
                        });
                    }
                    if ($(content).find('a').length) {
                        $("#attachmentDropDown_" + itemData.HelpCardTicketId).dxDropDownButton({
                            icon: "link",
                            hint: "Attachment and links",
                            showArrowIcon: false,
                            dropDownOptions: {
                                width: 230
                            },
                            onItemClick: function (e) {
                                //event.stopPropagation();
                            },
                            itemTemplate: function (data, index, element) {

                                var anchor = $(`<div><a href='${data.Url}' target="blank">${data.text}</a><div/>`);
                                element.append(anchor);
                            },
                            items: attachmentArray
                        });
                    }

                }
                else {
                    html = new Array();
                    html.push("<div class='row wikiTitle-container' style='padding-top:10px;'>");
                    html.push("<div class='helpCard-titleWrap'>");
                    html.push(itemData.HelpCardTitle);
                    html.push("<hr>");
                    html.push("</div>");
                    
                    html.push("</div>");
                    
                    const scrollView = $('<div />');
                    scrollView.append("<div class='helpCardPreview'>");
                    scrollView.append(itemData.HelpCardContent + (itemData.AgentContent ? itemData.AgentContent : ''));
                    scrollView.append("</div>")
                    scrollView.dxScrollView(
                        {
                            height: 390
                        });
                    itemElement.append(html.join(""));
                    itemElement.append(scrollView);
                    GenerateEmbedContent();
                }

            },
            onItemClick: function (obj) {
            },

        }).dxTileView('instance');

    }

    function clearSearch(obj) {
        debugger;
        filteredSearchCard = [];
        dxCategoryList.option('dataSource', alllistCategory);
        dxCategoryList.option('selectedItems', defaultCategory);
        SetbaseWidthHeightAndView(HelpCardWidgets);
        helpCardTiles.option('dataSource', HelpCardWidgets);
        $(".divClearSearch").hide();
    }
    function SetbaseWidthHeightAndView(datasource) {
        var isFirefox = navigator.userAgent.toLowerCase().indexOf('firefox') > -1;
        if (isFirefox)
            forceDocumentLoadCodeForFirefox();
        helpCardTiles._options.option('baseItemHeight', 495);
        helpCardTiles._options.option('baseItemWidth', 341);
        isNoOfCardsToShowIsGreaterThenThree = false;
        if (datasource && datasource.length > switchViewLimit) {
            isNoOfCardsToShowIsGreaterThenThree = true;
            switchBaseItemHeight = 200;
            switchBaseItemWidth = 250;
            helpCardTiles.option('baseItemHeight', 200);
            helpCardTiles.option('baseItemWidth', 250);
        }
    }

    function openLink(href) {



    }


    function DrawCategory() {
        dxCategoryList = $("#wikileftNavigation").dxList({
            noDataText: "",
            selectionMode: "single",
            activeStateEnabled: true,
            //selectedItem: widgetCategoryJquery,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                var html = new Array();
                html.push("<div class='wikiMenu-itemWrap'>")
                html.push("<div class='wikiMenu-infoWrap'>");
                html.push("<div class='wikiMenu-title'>" + itemData + "</div>");
                html.push("<div class='wikiMenu-count'></div>");
                html.push("</div>");
                html.push("</div>")
                itemElement.append(html.join(""));
            },
            onSelectionChanged: function (e) {
                debugger;
                const category = e.addedItems[0];
                selectedCategoryHelpCards = HelpCards.filter(x => x.HelpCardCategory.toLowerCase() == category.toLowerCase());
                if (e.itemData == 'All Categories')
                    selectedCategoryHelpCards = HelpCards;
                if (filteredSearchCard.length != 0) {
                    selectedCategoryHelpCards = filteredSearchCard.filter(x => x.HelpCardCategory.toLowerCase() == category.toLowerCase());
                    if (e.itemData == 'All Categories')
                        selectedCategoryHelpCards = filteredSearchCard;
                }

                SetbaseWidthHeightAndView(selectedCategoryHelpCards);
                helpCardTiles.option('dataSource', selectedCategoryHelpCards);
                //e.component.option('selectedItemKeys', [category]);
                //e.component.selectItem(category);
                widgetCategoryJquery = category;
            }

        }).dxList('instance');
    }


</script>

 <style>

.helpCardPreview p img {
    max-width: 100%;
    height: auto;
}

 .flashCard-container-display {
    width: 300px;
    height: 450px;
 }

.flashCard-container-display img {
    max-width: 100%;
}

.moveleft{
    margin-left: 112px;
}

/*.nav-moveleft{
     margin-left: 12px;
}*/

.att-icon{
    width: 11px;    
    margin-left: 10px;
}
.attachmentdiv{
    /*margin-top:20px;
    margin-left:2px;*/
}

.showcarddiv{
    border: 1px solid #CCC;
    padding: 5px;
    border-radius: 4px;
    display: inline-block;
    box-shadow: 3px 1px 5px rgba(0, 0, 0, 0.2);
    cursor: pointer;
}

.dx-buttongroup-wrapper {
    display: block;
}

.actionItemsDiv{
    position: absolute;
    bottom: 0px;
    width: 100%;
    text-align:right;
    left: 0;
    padding: 10px;
}
.hrDivider{
    border-top: 1px solid #ccc;
    margin: 12px;
}

.dx-widget.dx-dropdownbutton{
    
    border-radius: 0px;
    border: none;
}

.iframe-container{
   min-height: 100px;
   background:#fff url("../Content/Images/ajax-loader.gif") no-repeat 50% 50% !important;
}

</style>

<h4 style="margin-left: 36px;"></h4>  
<div class="row">  
    <div class="col-md-2 col-sm-2 col-xs-2"></div>
    <div class="col-md-6 col-sm-6 col-xs-6 moveleft"> <div class="helpCardContainer "></div></div>    
    <div class="col-md-3 col-sm-3 col-xs-3">
     <div class="btn btn-link divClearSearch" onclick="clearSearch(this);"> Clear Search</div>
    </div>
</div>
        <div class="wiki-topDiv row" style="margin-top: 47px;">              
            <div id="wikileftNavigation" class="col-md-2 col-sm-2 wikiNav-menuWrap nav-moveleft"></div>           
            <div class="HelpCardTiles col-md-10 col-sm-10"></div>            
  
            </div>

    <div id="helpcardpopup"></div>
     <div id="linkpopup"></div>
<div class="row" id="content">
    
</div>


</asp:Content>


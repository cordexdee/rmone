<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpCardsPanel.ascx.cs" Inherits="uGovernIT.Web.HelpCardsPanel" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
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

.showcarddiv {
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

.actionItemsDiv {
    position: absolute;
    bottom: 0px;
    width: 100%;
    text-align: right;
    left: 0;
    padding: 10px;
}
.hrDivider{
    border-top: 1px solid #ccc;
    margin: 12px;
}

    .dx-widget.dx-dropdownbutton {
        border-radius: 0px;
        border: none;
    }
</style>

<script src="../../Scripts/HelpCardDisplayPopup.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var globalheight = '<%=Height%>';
    var width = '<%=Width%>';

    $.ajax({
        type: "GET",
        url: "/api/CoreRMM/GetUserHelpCards?ViewID=" + '<%=ViewID%>',
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (message) {
            $("#divHelpCardView").dxTileView({
                items: message,
                itemMargin: 10,
                baseItemHeight: 200,
                baseItemWidth: 245,
                direction: "vertical",
                height: '<%=Height%>',
                width: '<%=Width%>',
                showScrollbar: true,
                hint: message.Name,
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var html = new Array();
                    if (true) {
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
                        var openContent = $(`<div class="showcarddiv" title='View' onclick="openHelpCard('${itemData.HelpCardTicketId}', '${itemData.HelpCardTitle}');"><img id=agentsDropDown_${itemData.HelpCardTicketId} src="/Content/Images/focusView.png" width='18px'/></div>`);
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
                        html.push("<div class='helpCardPreview flashCard-container'>");
                        html.push(itemData.HelpCardContent + (itemData.AgentContent ? itemData.AgentContent : ''))
                        html.push("</div>");
                        itemElement.append(html.join(""));
                    }
                },
                onItemClick: function (e) {
                }
            });

        },
        beforeSend: function () {
            $("#loadpanel").dxLoadPanel({
                message: "Loading...",
                visible: true
            });
        },
        complete: function (data) {
            $("#loadpanel").dxLoadPanel({ visible: false });
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log('error: ' + xhr);
        }
    });   
</script>

<div class="p-3">
    <div id="loadpanel"></div>
    <div id="divHelpCardView" class="divHelpCardView"></div>
</div>



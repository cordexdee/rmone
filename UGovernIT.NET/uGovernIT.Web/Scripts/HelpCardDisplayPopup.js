var Parentdiv = "";
var HelpCardContent = "";
var AgentContent = "";
var popup = "";
var popupOptions;
var title;
function DrawPopup() {
    popupOptions = $("#helpcardpopup").dxPopup({
        width: 400,
        height: 495,
        contentTemplate() {
            const scrollView = $('<div />');
            scrollView.append("<div class='row'>");
            scrollView.append("<div class='helpCardPreview'>");
            if (AgentContent)
                scrollView.append(HelpCardContent + AgentContent);
            else
                scrollView.append(HelpCardContent);
            scrollView.append("</div>");
            scrollView.append("</div>")
            scrollView.dxScrollView();
            return scrollView;
        },
        showTitle: true,
        title: title,
        visible: false,
        dragEnabled: false,
        hideOnOutsideClick: true
    }).dxPopup("instance");
}

function openHelpCard(ticketId, fieldname) {
    if (ticketId != null && ticketId != '') {
        $.get("/api/HelpCard/GetHelpCardByTicketId?id=" + ticketId, function (data) {
            HelpCardContent = data.HelpCardContent;
            AgentContent = data.AgentContent;
            title = data.HelpCardTitle;
            DrawPopup();
            popupOptions.show();
            GenerateEmbedContent();
        });
    }
}

function GenerateEmbedContent() {
    $(".helpCardPreview").siblings().find('a').each(function () {
        $(this).removeAttr('rel');
        $(this).removeAttr('target');
        $(this).click(function () {
            var href = $(this).attr('href');
            if (href.length > 1) {
                $(this).attr('href', '#');
                $(this).attr('url', href);
            }
            else {
                href = $(this).attr('url');
            }
            href = href.replace("watch?v=", "embed/");
            DrawLinkPopup(href);
        });
    });
}


function DrawLinkPopup(href) {
    var linkPopupOptions = $("#linkpopup").dxPopup({
        width: 1160,
        height: 730,
        contentTemplate() {
            const scrollView = $('<div />');
            scrollView.append('<div class="iframe-container"><iframe width="1120" height="680" src="' + href + '"></iframe ></div>');
            scrollView.dxScrollView();
            return scrollView;
        },
        showTitle: false,
        visible: false,
        dragEnabled: false,
        hideOnOutsideClick: true
    }).dxPopup("instance");
    linkPopupOptions.show();
}
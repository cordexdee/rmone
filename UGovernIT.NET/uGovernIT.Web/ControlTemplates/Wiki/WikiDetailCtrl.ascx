<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiDetailCtrl.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Wiki.WikiDetailCtrl" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .dx-textarea .dx-texteditor-input {
        resize: auto;
        font-family: inherit;
        display: block;
        overflow: auto;
        white-space: pre-wrap;
        margin: 0;
        height: 10px;
    }
    .listComments-wrap, .boxTwo {
        height: 150px;
    }
    .dxsplS {
        background-color: #b4c6ff;
    }
    .wikiArtical-popupWrap .dxsplS {
        width: auto !important;
    }
    .dxsplHSeparatorButton_UGITNavyBlueDevEx, .dxsplVSeparatorButton_UGITNavyBlueDevEx {
        padding: 3px;
    }
    .listComments-wrap .dx-button-has-icon .dx-icon-remove, .related-request .dx-button-has-icon .dx-icon-remove {
        width: 10px;
        height: 10px;
        color: red !important;
        font-size: 10px;
        line-height: 12px;
    }
    .Comment_Wrapup {
        display: flex;
    }

    .wikiRight-btnwrap {
        float: right;
    }

    .rightBtn-container {
        float: right;
        right: 10px;
        position: absolute;
    }

    .wikifooter-actionBtn {
        background: #4A6EE2;
        color: #fff;
        font: 12px 'Poppins', sans-serif !important;
    }

        .wikifooter-actionBtn:hover {
            background: #4A6EE2;
        }

    .wikiPermissionBTn {
        margin-left: 10px;
    }

    .wiki-btn-container {
        position: absolute;
        bottom: 0;
        width: 100%;
        right: 0;
    }

    .wikiRight-btnwrap {
        float: right;
    }

    .hasPlaceholder {
        color: #777;
    }

    .scoreTable {
        background-color: Black;
        color: White;
        border: 1px solid #454545;
        padding-left: 2px;
        padding-right: 2px;
        display: inline;
    }

    .descHeading {
        position: absolute;
        padding-left: 0px;
        padding-top: 5px;
        font-weight: bold;
    }
</style>


<asp:Panel runat="server" ID="tabServiceDetails" Style="width: 100%;">
    <dx:ASPxSplitter ID="ASPxSplitterWiki" runat="server" FullscreenMode="true" Width="100%" ClientInstanceName="wikiSplitter">
        <Styles>
            <Pane>
                <Paddings Padding="0px" />
            </Pane>
        </Styles>
        <Panes>
            <dx:SplitterPane Size="30%" MinSize="110px" Name="wikiLinksContainer" ShowCollapseBackwardButton="True">
                <PaneStyle CssClass="clsleftpane"></PaneStyle>
                <ContentCollection>
                    <dx:SplitterContentControl ID="SplitterContentControl1" runat="server">
                        <div class="row wikiDetail-linkSec">
                            <asp:Panel ID="pnlWikiLinks" runat="server" Style="display: inline-block; vertical-align: top;" />
                            <%--<div class="wiki-label">Links  <div id="btnlinkAdd" class="wikiAdd-btn wikiLink-addBTn"></div></div>
                            <div id="listLinks"></div>
                            <div class="col-md-12 col-sm-12 col-xs-12 wikiLinkTitle">
                                <div id="description" class="wikiNew-title"></div>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12 wikiLinkUrl">
                                <div id="url" class="wikiNew-title"></div>
                            </div>--%>
                           
                        </div>

                    </dx:SplitterContentControl>
                </ContentCollection>
            </dx:SplitterPane>

            <dx:SplitterPane MinSize="350px" Name="wikiDetailContainer">
                <Panes>
                    <dx:SplitterPane Name="wikiDetailsContainer" Size="72%" MinSize="100px">
                        <ContentCollection>
                            <dx:SplitterContentControl ID="SplitterContentControl2" runat="server" CssClass="wikiDetail">
                                <div id="divWikiInfo" runat="server" class="divWikiInfo">
                                    <div id="divWikiDetails" class="px-3 py-2" style="display: block; width: 100%;">
                                        <div style="display: block; padding-bottom: 2px;">
                                            <div style="width: 3%; display: inline-block; vertical-align: middle; margin-left: 5px">
                                                <asp:ImageButton ID="imgFavorite" ImageUrl="~/Content/ButtonImages/UnFavorite.png"
                                                    runat="server" isfavorite="false" OnClick="imgFavorite_Click" Style="margin-right: 5px;" />
                                            </div>
                                            <div style="width: 80%; display: inline-block; vertical-align: middle;">
                                                <asp:Label ID="lblWikiTitle" ClientIDMode="Static" runat="server" Style="margin-bottom: 10px; padding: 0px; font-weight: bold;"></asp:Label>
                                            </div>
                                            <div style="width: 15%; display: inline-block; vertical-align: top;">
                                                <asp:Label ID="lblTicketId" ClientIDMode="Static" runat="server" Style="margin-bottom: 5px; font-weight: bold; float: right;"></asp:Label>
                                                <div style="float: right;">
                                                    <asp:Label ID="Label1" runat="server" Style="margin-bottom: 10px; font-weight: bold; display: inline;">Score:</asp:Label>
                                                    <div id="divScore" style="text-align: center; display: inline;">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div>
                                            <div class="pt-3">
                                                <asp:Label ID="lblHeading" CssClass="descHeading" runat="server"></asp:Label>
                                            </div>
                                            <div id="taWikiDescriptionSection" class="taWikiDescriptionSection" style="width: 100%; overflow-y: auto; overflow-x: auto;"></div>
                                        </div>

                                        <div class="d-flex align-items-center justify-content-between pt-2">
                                            <div>
                                                Author:
                                            <asp:Label ID="lblCreatedBy" ClientIDMode="Static" runat="server" Style="font-weight: bold;"></asp:Label>&nbsp;&nbsp;&nbsp;Modified by:
                                            <asp:Label ID="lblUpdatedBy" ClientIDMode="Static" runat="server" Style="margin-bottom: 10px; font-weight: bold;"></asp:Label>
                                                <asp:Label ID="lblUpadtedOn" ClientIDMode="Static" runat="server"></asp:Label>
                                            </div>

                                            <div class="first_tier_nav d-flex align-items-center">
                                                <div class="d-flex">
                                                    <asp:ImageButton ID="imgLikebtn" ImageUrl="~/Content/Images/approve.png"
                                                        runat="server" isliked="false" OnClick="imgLikebtn_Click" ToolTip="Like" />
                                                    <div class="likeDisCount ml-1"><%=wikiLikesCount%></div>
                                                    <asp:ImageButton CssClass="ml-1" ID="imgDislikebtn" ImageUrl="~/Content/Images/reject.png"
                                                        runat="server" isdisliked="false" OnClick="imgDislikebtn_Click" ToolTip="Dislike" />
                                                    <div class="likeDisCount ml-1"><%=wikiDislikesCount%> </div>
                                                </div>

                                                <div class="ml-2">
                                                    <div id="editWikiBtn" class="wikiEditBtn"></div>
                                                    <%--<ul style="margin: 0px; padding-inline-start: 0;">
                                                        <li runat="server" id="lnkImgEditDetails" class="" onmouseover="this.className='tabhover'" onmouseout="this.className=''" style="margin-top: -5px;">
                                                            <asp:LinkButton ID="imgEditDetails" Visible="true" runat="server" Text="Edit" CssClass="edit" ToolTip="Edit"
                                                                Style="float: right; padding-left: 10px;"></asp:LinkButton>
                                                        </li>
                                                    </ul>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </dx:SplitterContentControl>
                        </ContentCollection>
                    </dx:SplitterPane>
                    <dx:SplitterPane Name="bottomContainer" MinSize="200px" ShowCollapseForwardButton="True">
                        <ContentCollection>
                            <dx:SplitterContentControl ID="SplitterContentControl3" runat="server">
                                <div class="row bottomContainer" style="display: block; margin-top: 10px; width: 100%;">
                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                        <div class="discussion-thread">
                                            <div class="wiki-label d-flex justify-content-between align-items-center" style="color: #4A6EE2">
                                                <span>Discussion Thread</span>
                                                <div class="flex-grow-1 px-2">
                                                    <div id="comments"></div>
                                                </div>
                                                <div id="btnDiscussionAdd" class="wikiAdd-btn"></div>
                                            </div>
                                            <div id="listComments" class="listComments-wrap pt-2"></div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-sm-6 col-xs-6">

                                        <div class="row wikiRequest-sec">
                                            <div class="related-request">
                                                <div class="wiki-label d-flex align-items-center" style="color: #4A6EE2">
                                                    <span>Related Request</span>
                                                    <%--<img src="/Content/Images/related_to.png" />--%>
                                                    <div id="other-ticket" class="otherTicket-link ml-1"></div>
                                                </div>
                                                <div id="listLinkedOtherTickets" class="boxTwo py-2"></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </dx:SplitterContentControl>
                        </ContentCollection>
                    </dx:SplitterPane>
                </Panes>
            </dx:SplitterPane>
        </Panes>
        <ClientSideEvents Init="function(s,e){ASPxSplitterWiki_Init(s,e);}" PaneExpanded="function(s,e){ASPxSplitterWiki_Adjust(s,e);}" PaneCollapsed="function(s,e){ASPxSplitterWiki_Adjust(s,e);}" />
    </dx:ASPxSplitter>

</asp:Panel>
<div class="row wiki-btn-container" style="display: inline-block; width: 100%; padding-top: 2px">
    <div class="col-md-6 col-sm-6 col-xs-12 wikiRight-btnwrap">
        <div class="rightBtn-container">
            <div id="btnEmailLink" class="wikifooter-actionBtn"></div>
        </div>
    </div>
    <div class="col-md-6 col-sm-6 col-xs-12">
        <div class="leftBtn-container">
            <div id="btnArchive" class="wikifooter-actionBtn"></div>
            <div id="btnPermission" class="wikifooter-actionBtn wikiPermissionBTn"></div>
            <div id="btnDelete" class="wikifooter-actionBtn"></div>
            <div id="btnUnArchive" class="wikifooter-actionBtn"></div>
        </div>
    </div>
</div>


<script data-v="<%=UGITUtility.AssemblyVersion %>">

    var wikiDiscussion = {};
    var wikiLinks = {};
    var commentsList = [];
    var globaldata = [];
    var TicketId = '<%=TicketId%>';
    //alert(id);
    $(document).ready(function () {
        $('.wikiArtical-popupWrap').parents().eq(1).addClass('wikiArtical-popupContainer');
        //$('#wikiHtmlcontent').child().addClass("wikiHtmlcontent-wrap");

        LoadData();
    });


    function LoadData() {
        $.get("/api/WikiArticles/GetwikiDetails?ticketId=" + '<%=TicketId%>', function (data, status) {
            globaldata = data;
            var wikiTitle = globaldata.wikiContents.Title;
            var htmlText = globaldata.wikiContents.Content;
            var modifiedBy = globaldata.wikiContents.ModifiedByUser;
            var modifiedDate = globaldata.WikiContentsModified;
             
            $('#lblWikiTitle').text(globaldata.wikiContents.Title);
            $('#lblTicketId').text(globaldata.TicketID);
            if (globaldata.WikiArticles.WikiAverageScore != null) {
                document.getElementById("divScore").innerHTML = "<div class='scoreTable'>" + globaldata.WikiArticles.WikiAverageScore; +"</div>"; 
            }
            else {
                document.getElementById("divScore").innerHTML = "<div class='scoreTable'>N/A</div>"
            }
             
            document.getElementById("taWikiDescriptionSection").innerHTML = globaldata.wikiContents.Content;
            $('#lblCreatedBy').text(globaldata.WikiArticles.CreatedBy);
            $('#lblUpdatedBy').text(globaldata.WikiArticles.ModifiedBy);
            $('#lblUpadtedOn').text("on "+modifiedDate);

            $('#wikiTitle').append(wikiTitle);
            $('#wikiHtmlcontent').append(htmlText);
            $('#modifiedBy').append("<span class='modify-name'>" + modifiedBy + "</span>");
            $('#modifiedDate').append("<span class='modify-date'>" + modifiedDate + "</span>");


            $("#listLinks").dxList({
                dataSource: globaldata.WikiLinks,
                noDataText: " ",
                allowItemDeleting: true,
                itemDeleteMode: "static",

                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var html = new Array();
                    // html.push(itemData.Title);
                    //   html.push("<a class='wikiTitleAnchor' id="+itemData.Title+" onclick ='openEditDialog(this);'>"+itemData.Title+"</a>" );
                    // html.push("<a href="https://www.w3schools.com">itemData.Title</a>" );
                    // html.push("<a  href = "+itemData.URL+">" +itemData.Title+ "</a>" );
                    html.push("<a  href = " + itemData.URL + " target='_blank' >" + itemData.Title + "</a>");

                    //html.push("By " + itemData.CreatedByUser);
                    itemElement.append(html.join(""));
                    itemElement.addClass('listLinkItem');
                }

            });

            if (globaldata.IsAuthorizedToView == true) {

                if (globaldata.WikiArticles.IsDeleted == false) {
                    var btnArchive = $("#btnArchive").dxButton("instance");
                    btnArchive.option("visible", true);

                    var btnPermission = $("#btnPermission").dxButton("instance");
                    btnPermission.option("visible", true);

                    var btnEmailLink = $("#btnEmailLink").dxButton("instance");
                    btnEmailLink.option("visible", true);

                    var btnDelete = $("#btnDelete").dxButton("instance");
                    btnDelete.option("visible", false);

                    var btnUnArchive = $("#btnUnArchive").dxButton("instance");
                    btnUnArchive.option("visible", false);

                }
                else {
                    var btnArchive = $("#btnArchive").dxButton("instance");
                    btnArchive.option("visible", true);

                    var btnPermission = $("#btnPermission").dxButton("instance");
                    btnPermission.option("visible", true);

                    var btnEmailLink = $("#btnEmailLink").dxButton("instance");
                    btnEmailLink.option("visible", true);

                    var btnDelete = $("#btnDelete").dxButton("instance");
                    btnDelete.option("visible", true);

                    var btnUnArchive = $("#btnUnArchive").dxButton("instance");
                    btnUnArchive.option("visible", true);

                }


            }

            else {

                var btnArchive = $("#btnArchive").dxButton("instance");
                btnArchive.option("visible", false);

                var btnPermission = $("#btnPermission").dxButton("instance");
                btnPermission.option("visible", false);

                var btnEmailLink = $("#btnEmailLink").dxButton("instance");
                btnEmailLink.option("visible", false);

                var btnDelete = $("#btnDelete").dxButton("instance");
                btnDelete.option("visible", false);

                var btnUnArchive = $("#btnUnArchive").dxButton("instance");
                btnUnArchive.option("visible", false);

                //link add & delete

                var description = $("#description").dxTextBox("instance");
                description.option("visible", false);
                var url = $("#url").dxTextBox("instance");
                url.option("visible", false);

                var btnlinkAdd = $("#btnlinkAdd").dxButton("instance");
                btnlinkAdd.option("visible", false);
                var listLinks = $("#listLinks").dxList("instance");
                listLinks.option("allowItemDeleting", false);


                //Discussion add
                var comments = $("#comments").dxTextArea("instance");
                comments.option("visible", false);
                comments.option("autoResizeEnabled", true);
                var btnDiscussionAdd = $("#btnDiscussionAdd").dxButton("instance");
                btnDiscussionAdd.option("visible", false);
                var listComments = $("#listComments").dxList("instance");
                listComments.option("allowItemDeleting", false);

                //edit wiki btn

                var editWikiBtn = $("#editWikiBtn").dxButton("instance");
                editWikiBtn.option("visible", false);

            }

            //   $('#listLinks').text('test data');
        });

    }


    $.get("/api/WikiArticles/GetLinksToOtherTickets?ticketId=" +'<%=TicketId%>', function (data, status) {

        $("#listLinkedOtherTickets").dxList({
            dataSource: data,
            allowItemDeleting: true,
            noDataText: " ",
            itemTemplate: function (itemData, itemIndex, itemElement) {
                var html = new Array();
                //html.push("<div class='TicketDetails' onclick=\"" + itemData.Link + "\" >");
                html.push("<div class='TicketDetails' onclick=\"" + itemData.Link + "\" >");
                html.push("<div class='childTicketId'>")
                html.push(itemData.ChildTicketID + " : " +"<b>"+ itemData.ChildTicketTitle+"<b>");
                html.push("</div>")
                html.push("</div>")
                itemElement.append(html.join(""));

            },

            onItemDeleted: function (e) {
                var itemData = e.itemData;


                $.post("/api/WikiArticles/DeleteLinksToOtherTickets?ID= " + itemData.ID + "&ticketId=" + TicketId, function (data, status) {

                    var discussions = $("#listLinkedOtherTickets").dxList("instance");
                    discussions.option("dataSource", data);
                });



                // Handler of the "itemDeleted" event
            }


        });

    });

    $.get("/api/WikiArticles/GetWikiDiscussions?ticketId=" + '<%=TicketId%>', function (data, status) {


        $("#listComments").dxList({
            dataSource: data,
            noDataText: " ",
            allowItemDeleting: true,
            itemTemplate: function (itemData, itemIndex, itemElement) {
                var html = new Array();
                html.push("<div class='Comment_Wrapup'>");
                html.push(itemData.CreatedByUser);
                html.push("<div class='CommentDate ml-1'>")
                html.push(itemData.CreatedDate);
                html.push("</div>")
                html.push("<div class='Comment'>")
                html.push(": " + "<b>" + itemData.Comment +"</b>");
                html.push("</div>");
                html.push("</div>");
                itemElement.append(html.join(""));
            },


            onItemDeleted: function (e) {
                var itemData = e.itemData;
                // alert(itemData.ID);

                $.post("/api/WikiArticles/DeleteWikiDiscussion?ID= " + itemData.ID, function (data, status) {

                    var discussions = $("#listComments").dxList("instance");
                    discussions.option("dataSource", data);
                    discussions.option("noDataText", " ");
                });



                // Handler of the "itemDeleted" event
            }

        });



    });


    $("#description").dxTextBox({
        placeholder: "Link Title",
        onValueChanged: function (e) {

            wikiLinks.Title = e.value;
        }

    });

    $("#url").dxTextBox({

        placeholder: "Link Url",
        onValueChanged: function (e) {

            wikiLinks.URL = e.value;

        }

    });





    $(function () {
        $("#other-ticket").dxButton({
            /*text: "Link Requests",*/
            allowItemDeleting: true,
            itemDeleteMode: "static",
            icon: '/Content/Images/add_icon.png',

            onClick: function (e) {
                //var myText = htmlEditor.GetHtml();
                // AddWikiRequestModel.HtmlBody = myText;
                // $.post("/api/WikiArticles/CreateWiki?" + $.param(AddWikiRequestModel));
                var url = "/Layouts/uGovernIT/DelegateControl.aspx?control=listpicker&isdlg=1&isudlg=1&Type=TicketRelation&TicketId=" + TicketId;
                //url = UGITUtility.GetAbsoluteURL(string.Format(listpickerUrl, "listpicker", "Picker List", currentModuleName, title, ticketRequestTypeLookup, currentModuleListPagePath, "", ""));

                // var url = "/layouts/ugovernit/delegatecontrol.aspx?control=listpicker&TicketId= " +TicketId+ "&Type=TicketWiki&ParentModule=WIKI"  ;
                window.parent.UgitOpenPopupDialog(url, "", "Picker List", "75", "95", false, "");

                // window.parent.CloseWindowCallback(1, document.location.href);



            },

            onItemDeleted: function (e) {
                var itemData = e.itemData;
                // alert(itemData.ID);

                $.post("/api/WikiArticles/DeleteLinksToOtherTickets?ID=" + itemData.ID + "&ticketId=" + TicketId, function (data, status) {

                    var discussions = $("#listLinkedOtherTickets").dxList("instance");
                    discussions.option("dataSource", data);
                });



                // Handler of the "itemDeleted" event
            }

        });


        $.get("/api/WikiArticles/GetLinksToOtherTickets?ticketId=" +'<%=TicketId%>', function (data, status) {

            $("#listLinkedOtherTickets").dxList({
                dataSource: data,
                allowItemDeleting: true,
                noDataText: " ",
                itemTemplate: function (itemData, itemIndex, itemElement) {
                    var html = new Array();
                    html.push("<div class='TicketDetails' onclick=\"" + itemData.Link + "\" >");
                    html.push("<div class='childTicketId'>")
                    html.push(itemData.ChildTicketID + " : " +"<b>" +itemData.ChildTicketTitle+"<b>");
                    html.push("</div>")
                    //html.push("<div class='childTicketTitle'>")
                    //html.push(itemData.ChildTicketTitle);
                    //html.push("</div>");
                    html.push("</div>")
                    itemElement.append(html.join(""));

                },

                onItemDeleted: function (e) {
                    var itemData = e.itemData;

                    //alert(e.itemData.ID);
                    $.post("/api/WikiArticles/DeleteLinksToOtherTickets?ID=" + itemData.ID + "&ticketId=" + TicketId, function (data, status) {

                        var discussions = $("#listLinkedOtherTickets").dxList("instance");
                        discussions.option("dataSource", data);
                        discussions.option("noDataText", " ");
                    });



                    // Handler of the "itemDeleted" event
                }


            });

        });


    });

    //    $(document).keyup(function() {
    //    if ($("#comments") && event.key == "Enter") {
    //        alert('Enter key pressed');
    //        alert(this.val);

    //    }
    //});




    $('#comments').dxTextArea({

        placeholder: "Comments",

        onValueChanged: function (e) {

            wikiDiscussion.comment = e.value

        }

    });


    $("#btnDiscussionAdd").dxButton({
        icon: '/Content/Images/add_icon.png',

        onClick: function (e) {

            wikiDiscussion.TicketId = TicketId;
            $.post("/api/WikiArticles/CreateWikiDiscussion?" + $.param(wikiDiscussion), function (data, status) {
                commentsList = data;
                $('#comments textarea.dx-texteditor-input').val('');// clear the text box 
                $('div [data-dx_placeholder=Comments]').removeClass('dx-state-invisible'); //to remove dx invisible class from dx div

                $("#listComments").dxList({
                    dataSource: commentsList,
                    noDataText: " ",
                    allowItemDeleting: true,
                    itemDeleteMode: "static",
                    itemTemplate: function (itemData, itemIndex, itemElement) {
                        var html = new Array();
                        html.push("<div class='Comment_Wrapup'>");
                        html.push(itemData.CreatedByUser);
                        html.push("<div class='CommentDate'>")
                        html.push(itemData.CreatedDate);
                        html.push("</div>")
                        html.push("<div class='Comment'>")
                        html.push(": " + itemData.Comment);
                        html.push("</div>");
                        html.push("</div>");
                        itemElement.append(html.join(""));

                    },

                    onItemDeleted: function (e) {
                        var itemData = e.itemData;
                        // alert(itemData.ID);

                        $.post("/api/WikiArticles/DeleteWikiDiscussion?ID= " + itemData.ID, function (data, status) {

                            var discussions = $("#listComments").dxList("instance");
                            discussions.option("dataSource", data);
                            discussions.option("noDataText", " ");
                        });



                        // Handler of the "itemDeleted" event
                    }

                });


            });
        }
    });


    $("#btnlinkAdd").dxButton({
        icon: '/Content/Images/add_icon.png',
        onClick: function (e) {
            wikiLinks.TicketId = TicketId;
            $.post("/api/WikiArticles/CreateWikiLinks?" + $.param(wikiLinks), function (data, status) {

                $('#description .dx-texteditor-input').val('');// clear the text box 
                $("div [data-dx_placeholder='Link Title']").removeClass('dx-state-invisible'); //to remove dx invisible class from dx div

                $('#url .dx-texteditor-input').val('');// clear the text box 

                $("div [data-dx_placeholder='Link Url']").removeClass('dx-state-invisible');   //to remove dx invisible class from dx div

                // commentsList = data;
                $("#listLinks").dxList({
                    dataSource: data,
                    allowItemDeleting: true,
                    itemDeleteMode: "static",

                    itemTemplate: function (itemData, itemIndex, itemElement) {
                        var html = new Array();
                        // html.push(itemData.Title);
                        // html.push("<a  href = "+itemData.URL+">" +itemData.Title+ "</a>" );
                        html.push("<a  href = " + itemData.URL + " target='_blank' >" + itemData.Title + "</a>");

                        itemElement.append(html.join(""));
                    },

                    onItemDeleted: function (e) {
                        var itemData = e.itemData;
                        //alert(itemData.ID);

                        $.post("/api/WikiArticles/DeleteWikiLinks?ID= " + itemData.ID, function (data, status) {

                            var links = $("#listLinks").dxList("instance");
                            links.option("dataSource", data);
                        });

                    }

                });

            });

        }
    });

    $(function () {
        $("#editWikiBtn").dxButton({

            text: "Edit",
            onClick: function (e) {

                //DevExpress.ui.notify("The Edit button was clicked");
                var url = "/layouts/ugovernit/delegatecontrol.aspx?control=wikiList&action=edit&TicketId=" + TicketId;
                window.parent.UgitOpenPopupDialog(url, "", "Edit Wiki Article", "75", "95", false, "");

            }


        });
<%--            $.get("/api/WikiArticles/GetwikiDetails?ticketId=" + '<%=TicketId%>', function (data, status) {
           // globaldata = data;

            var wikiTitle = data.wikiContents.Title;
            var htmlText =  data.wikiContents.Content;
            var modifiedBy = data.wikiContents.ModifiedByUser;
            var modifiedDate = data.WikiContentsModified;
            $('#wikiTitle').remove();
            $('#wikiHtmlcontent').remove();
            $('#modifiedBy').remove();
            $('#modifiedDate').remove();

            $('#wikiTitle').append(wikiTitle);
            $('#wikiHtmlcontent').append(htmlText);
            $('#modifiedBy').append("<span class='modify-name'>"+modifiedBy+"</span>");
            $('#modifiedDate').append("<span class='modify-date'>" + modifiedDate+ "</span>");


        });--%>
    });

    $("#btnArchive").dxButton(
        {
            text: "Archive",

            onClick: function (e) {
                //wikiLinks.TicketId = TicketId;

                var result = DevExpress.ui.dialog.confirm('Are you sure you want to archive this wiki?', 'Confirm');
                result.done(function (confirmation) {
                    if (confirmation) {
                        $.post("/api/WikiArticles/ArchiveWikiArticle?ticketId=" + '<%=TicketId%>', function (data, status) {

                            window.parent.CloseWindowCallback(1, document.location.href);

                        });

                    }
                });


            }

        });

    $("#btnPermission").dxButton(
        {
            text: "Permission",

            onClick: function (e) {
                var url = "/Layouts/uGovernIT/DelegateControl.aspx?control=wikipermission&ModuleName=WIKI&Type=wik&currentTicketPublicID=" + TicketId;

                window.parent.UgitOpenPopupDialog(url, "", "Wiki Permission", "50", "50", false, "");


            }

        });

    $("#btnDelete").dxButton(
        {
            text: "Delete",
            onClick: function (e) {


                var result = DevExpress.ui.dialog.confirm('Are you sure you want to delete this wiki?', 'Confirm');
                result.done(function (confirmation) {
                    if (confirmation) {
                        $.post("/api/WikiArticles/DeleteWikiArticle?ticketId=" +'<%=TicketId%>', function (data, status) {

                            window.parent.CloseWindowCallback(1, document.location.href);

                        });

                    }
                });

                //wikiLinks.TicketId = TicketId;
            }

        });

    $("#btnUnArchive").dxButton(
        {
            text: "UnArchive",
            onClick: function (e) {
                //wikiLinks.TicketId = TicketId;
                var result = DevExpress.ui.dialog.confirm('Are you sure you want to unArchive this wiki?', 'Confirm');
                result.done(function (confirmation) {
                    if (confirmation) {
                        $.post("/api/WikiArticles/UnArchiveWikiArticle?ticketId=" +'<%=TicketId%>', function (data, status) {


                        });
                        window.parent.CloseWindowCallback(1, document.location.href);

                    }
                });


            }
        });


    $("#btnEmailLink").dxButton(
        {
            text: "EmailLink",
            visible: true,
            onClick: function (e) {
                //wikiLinks.TicketId = TicketId;

                //var url = "/Layouts/uGovernIT/DelegateControl.aspx?control=listpicker&isdlg=1&isudlg=1&Type=TicketRelation&TicketId= " + TicketId;
                var url = "/Layouts/uGovernIT/DelegateControl.aspx?control=ticketemail&ModuleName=WIKI&Type=wik&currentTicketPublicID=" + TicketId;

                window.parent.UgitOpenPopupDialog(url, "", "Wiki Link", "75", "95", false, "");
                //window.parent.CloseWindowCallback(1, document.location.href);

            }
        });



    $("#likeWiki").click(function () {
        // alert("hi from like");
        $.post("/api/WikiArticles/LikeWikiArticle?ticketId=" + TicketId, function (data, status) {

        });

    });


    $("#disLikeWiki").click(function () {
        //alert("hi from Dislikelike");
        $.post("/api/WikiArticles/DisLikeWikiArticle?ticketId=" + TicketId, function (data, status) {

        });

    });
    function ASPxSplitterWiki_Adjust(s, e) {
        $(".taWikiDescriptionSection").css("height", ($(".wikiDetail").height() - 110) + "px");
    }

    function ASPxSplitterWiki_Init(s, e) {
        $(".taWikiDescriptionSection").css("height", ($(".wikiDetail").height() - 110) + "px");
        var panelbottom = wikiSplitter.GetPaneByName("bottomContainer");
        var panelleft = wikiSplitter.GetPaneByName("wikiLinksContainer")
        if (panelbottom != null) {
            if ('<%=IsExpandBottomContainer%>' == "True")
                panelbottom.Expand();
        }

        if (panelleft != null) {
            if ('<%=IsExpandLeftContainer%>' == "True")
                panelleft.Expand();
        }

    }
</script>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WikiLink.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Wiki.WikiLink" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script data-v="<%=UGITUtility.AssemblyVersion %>">

    jQuery(function () {
        jQuery.support.placeholder = false;
        test = document.createElement('input');
        if ('placeholder' in test) jQuery.support.placeholder = true;
    });
    // This adds placeholder support to browsers that wouldn't otherwise support it.
    $(function () {
        if (!$.support.placeholder) {
            var active = document.activeElement;
            $(':text').focus(function () {
                if ($(this).attr('placeholder') != '' && $(this).val() == $(this).attr('placeholder')) {
                    $(this).val('').removeClass('hasPlaceholder');
                }
            }).blur(function () {
                if ($(this).attr('placeholder') != '' && ($(this).val() == '' || $(this).val() == $(this).attr('placeholder'))) {
                    $(this).val($(this).attr('placeholder')).addClass('hasPlaceholder');
                }
            });
            $(':text').blur();
            $(active).focus();
            $('form:eq(0)').submit(function () {
                $(':text.hasPlaceholder').val('');
            });
        }
    });

    function ConfirmDelete() {
        if (confirm("Are you sure you want to delete this link?")) {
            return true;
        }
        else {
            return false;
        }
    }
    function ShowDeleteButton(PrmThis) {
        $("span[linkid][isLink]", $(PrmThis)).css("display", "inline");
    }
    function HideDeleteButton(PrmThis) {
        $("span[linkid][isLink]", $(PrmThis)).css("display", "none");
    }
    //function ShowDeleteImage(PrmThis) {
    //     $(PrmThis).css("display", "inline");
    //}
    //function HideDeleteImage(PrmThis) {
    //    $(PrmThis).css("display", "none");
    //}
    function OnEditLink() {

        if ($(".deleteLink").is(":visible") == true) {
            $(".deleteLink").hide();
        }
        else {
            $(".deleteLink").show();
        }
    }


    function showUploadControl() {
        $("#<%=fileupload.ClientID %>").show();
    }

    function showWiki() {

        $("#<%=fileupload.ClientID %>").hide();
    }

</script>


<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">

    window.WikiLinks = {

        currentBudgetItemID: 0,
        addLinks: function (obj) {
            this.clearAddLinkForm();
            $(".divAddLink .ugit-ms-dlgTitleText").html("Add Link");

            this.setPopupPosition(obj, true);
            $(".divAddLink").show();
            $("#<%=fileupload.ClientID %>").hide();
        },
        clearAddLinkForm: function () {

            $(".divAddLink .trTitle :text").val("");
            $(".divAddLink .trUrl :text").val("");

        },
        closePopup: function (clearForm) {
            if (clearForm) {
                this.clearAddLinkForm();
            }
            $(".divAddLink").hide();
        },

        setPopupPosition: function (obj, isNew) {

            var currentLeft = $(obj).position().left;
            var currentTop = $(obj).position().top;
            if (isNew) {
                currentLeft = $(obj).position().left;
                currentTop = $(obj).position().top;
            }
            var windowWidth = $(window).width();
            var windowHeight = $(window).height();

            var newLeft = 0;
            var newTop = 0;

            newLeft = currentLeft;
            if ((windowWidth - currentLeft) < 350) {
                newLeft = currentLeft - 340;
            }

            newTop = currentTop;
            if ((windowHeight - currentTop) < 275) {
                newTop = currentTop - 265;
            }

            if ((windowHeight - currentTop) > 240 && (windowHeight - currentTop) < 275) {
                newTop = currentTop - 140;
            }

            if ((windowHeight - currentTop) > 200 && (windowHeight - currentTop) < 240) {
                newTop = currentTop - (345 - (windowHeight - currentTop));
            }

            if ((windowHeight - currentTop) > 100 && (windowHeight - currentTop) < 200) {
                newTop = currentTop - (345 - (windowHeight - currentTop));
            }

            $(".divAddLink").css({ "left": newLeft + "px", "top": newTop + "px" });
        }
    }


    function ValidateUrl() {
        var fUpload = $('#<%=fileupload.ClientID%>');
        var txtLink = $('#<%=txtLinkUrl.ClientID%>');

        if ((fUpload.val() == '' && txtLink.val().trim() == '')) {
            //$('#dvLinkError').show();
            return false;
        }

        //else if (fUpload.val() == '' && !txtLink.val().match('^http://') && !txtLink.val().match('^https://')) {        
        //    $('#dvLinkError').show();
        //    return false;
        //}

        //$('#dvLinkError').hide();
        return true;
    }

</script>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .fUpload {
        margin-left: 0px;
        padding-top: 2px;
    }

    .columnPosition {
        position: relative;
        vertical-align: top;
    }

    .wrapText {
        word-wrap: break-word; /* IE 5+ */
        word-break: break-all;
    }

    .gvLinks td {
        vertical-align: top;
        padding-bottom: 5px;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        padding: 3px 6px 4px;
        vertical-align: top;
        border-top: 2px solid #FFF;
    }

    .ms-formlabel {
        padding-left: 32px;
        border-top: none;
        padding-top: 8px;
    }
</style>
<div id="divLinks" runat="server" style="height: 100%;">
    <div id="divLinkList">
        <div id="divWikiLinkHeader" runat="server">
            <label style="position: relative; top: 5px; left: 5px; color: Black; font-weight: bold; vertical-align: top;">Links</label>
            <img id="imgnew" runat="server" style="position: relative; padding-bottom: 3px; padding-left: 8px;" src="/Content/images/add_icon.png" onclick="window.WikiLinks.addLinks(this);" />
            <img id="imgEdit" runat="server" style="position: relative; padding-top: 3px; padding-left: 4px;" src="/Content/Images/edit-icon.png" onclick="OnEditLink();" />
        </div>
        <asp:GridView ID="gvLinkList" runat="server" CssClass="gvLinks" AutoGenerateColumns="false" Style="border: 0px; width: 100%; margin-left: 5px; margin-bottom: 10px;" OnRowDataBound="gvLinkList_RowDataBound"
            GridLines="None">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink NavigateUrl='<%#Eval("URL") %>' Target="_blank" ID="lnkUrl" runat="server" Text='<%#Eval("Comments") %>'
                            Style="cursor: pointer; vertical-align: top;"></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle CssClass="columnPosition wrapText" />

                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <span class="deleteLink" style="width: 17px; display: none; padding-right: 6px; float: right;" linkid='<%#Eval("ID") %>' islink="true"><%--background-color:white;position:absolute;--%>
                            <asp:ImageButton ID="imgDeleteLink" ImageUrl="~/Content/images/delete-Icon.png" LinkId='<%#Eval("ID") %>' runat="server" OnClientClick="return ConfirmDelete();" OnClick="DeleteLink_Click" />
                        </span>
                    </ItemTemplate>
                    <ItemStyle CssClass="columnPosition" VerticalAlign="Top" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>



    </div>

    <div style="background: #ffffff; float: left; border: 1px solid #7C684D; position: absolute; z-index: 1000; display: none;" runat="server" class="divAddLink" id="divAddLink">
        <div class="ugit-ms-dlgTitle" style="cursor: default; background: #191919; height: 32px; overflow: hidden; white-space: nowrap;">
            <span title="Edit Note" class="ugit-ms-dlgTitleText" style="width: 286px; color: white; float: left; padding-left: 5px; padding-top: 6px;">New Budget Item</span><span class="ms-dlgTitleBtns"><a id="DlgClose7c120c0d-e246-48f6-829c-a86377df0513"
                class="ms-dlgCloseBtn" title="Close" href="javascript:;" accesskey="C"><span class="s4-clust"
                    style="height: 18px; width: 18px; position: relative; display: inline-block; overflow: hidden;">
                    <img class="ms-dlgCloseBtnImg" style="left: -0px !important; top: -658px !important; position: absolute;"
                        src="/Content/images/fgimg.png" onclick="window.WikiLinks.closePopup(true);">
                </span></a></span>
        </div>

        <div>
            <div style="float: left; width: 100%;">
                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="400px">



                    <tr class="trTitle">
                        <td class="ms-formlabel" style="float: right; padding-right: 0px;">
                            <h3 class="ms-standardheader">Description<b style="color: Red;">*</b>
                            </h3>
                        </td>
                        <td class="ms-formbody">
                            <asp:TextBox ID="txtLinkTitle" ValidationGroup="AddLink" Width="270px" runat="server" CssClass="fullwidth"></asp:TextBox>
                            <span style="float: left; width: 100%;">
                                <asp:RequiredFieldValidator ID="ReqValtxtLinkTitle" runat="server" ValidationGroup="AddLink" SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtLinkTitle" ErrorMessage="Link description is empty." ForeColor="Red"></asp:RequiredFieldValidator>
                            </span>
                        </td>
                    </tr>



                    <tr class="trUrl">
                        <td class="ms-formlabel" style="float: right; padding-right: 0px;">
                            <h3 class="ms-standardheader">Url<b style="color: Red;">*</b>
                            </h3>
                        </td>
                        <td class="ms-formbody">
                            <asp:TextBox runat="server" ID="txtLinkUrl" ValidationGroup="AddLink" Width="270px"></asp:TextBox>
                            <asp:FileUpload ID="fileupload" class="fUpload" Width="270px" runat="server" />

                            <%--Add Add wiki and document link--%>
                            <asp:TextBox ID="txtHelp" runat="server" Visible="false" Width="386px" />

                            <a id="aShowWiki" runat="server" onclick="showWiki()" style="cursor: pointer;">Add Wiki</a> |
                                <a onclick="showUploadControl()" style="cursor: pointer;">Upload Document</a>
                            <%--<div id="dvLinkError" style="float: left; width: 100%;display:none;color:red">Link url is not valid.</div>--%>
                                   
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" style="text-align: right; padding: 5px 5px 5px 0px;">

                            <div style="float: right; padding-top: 10px;">

                                <asp:LinkButton ID="btNewLink" Visible="true" runat="server" Text="&nbsp;&nbsp;New&nbsp;&nbsp;" ValidationGroup="AddLink"
                                    ToolTip="New Link" Style="padding-top: 10px;" OnClientClick="return ValidateUrl();" OnClick="btnAddLink_Click">
                    <span class="button-bg">
                     <b style="float: left; font-weight: normal;">
                     Add</b>
                     <i
                    style="float: left; position: relative; top: 1px;left:2px">
                    <img src="/Content/images/uGovernIT/add_icon.png"  style="border:none;" title="" alt=""/></i> 
                     </span>
                                </asp:LinkButton>

                                <span class="button-bg" style="height: 25px;" onclick="window.WikiLinks.closePopup(true);">
                                    <b style="float: left; font-weight: normal;">Cancel</b>
                                    <i
                                        style="float: left; position: relative; top: -2px; left: 2px">
                                        <img src="/Content/images/cancel.png" style="border: none;" title="" alt="" /></i>
                                </span>


                            </div>
                        </td>

                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>

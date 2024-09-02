<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadAndLinkDocuments.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.UploadAndLinkDocuments" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<script src="../../Scripts/DMS/Repository.js?v=<%=UGITUtility.AssemblyVersion %>"></script>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    var cnt = 0;
    function UpLoadDocument(isUpload) {
       // var parentFrameId = window.parent.iframeObj.attr('id');
        var parentFrameID = window.parent.iframeObj.attr('id');
        localStorage.setItem('parentID', parentFrameID);
        if (localStorage.getItem("callbackcnt") === null) {
            parentFramID = localStorage.setItem('OldparentID', parentFrameID);
        }
        url = '<%=DocumentManagementUrl%>';
        $.ajax({
            type: "POST",
            url: "/documentmanagement/Index?ticketid=<%=projectPublicID%>&typeOfFolder=<%=FolderName%>&isUpload=" + isUpload +"&isTabSelected=<%=IsTabActive%>",
            success: function () {
                
            }

        });

        window.parent.UgitOpenPopupDialog(url, '', 'Link To Document', 90, 90, false, "<%=sourceURL %>");
    }
    

    function setSelectedDocumentDetails(documentData, documentId) {

        console.log("from setSelectedDocumentDetails");
        //Link multiple files
        for (var index = 0; index < documentData.length; index++) {

            $("div#<%=bindMultipleLink.ClientID%>").append('<a id=file_' + documentId[index] + ' onclick="return downloadSelectedFile(' + documentId[index] + ')" style="cursor: pointer;">' + documentData[index] +
                '</a>' + "<img src='/content/images/close-red.png' id= img_" + documentId[index] + " class='linkDelete-icon' onclick='deleteLinkedDocument(\"" + documentId[index] + "\")'></img><br/>");

        }
        //close Document folder page
        window.parent.CloseWindowCallback(document.location.href);

        $("#<%=fileAttchmentId.ClientID%>").val(documentId.join(","));
    }

    function deleteLinkedDocument(documentid) {

        console.log("from Upload and link Document");

        if (confirm("Are you sure you want to delete attachment?")) {

            if (documentid != '' && documentid != null && documentid != undefined) {

                var dataVar = "{'id':'" + documentid + "', 'name':'" + documentid + "'}";

                $.ajax({
                    type: "POST",
                    url: "/api/Account/DeleteDocument",
                    data: dataVar,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response == 'deleted') {
                            $('#file_' + documentid).remove();
                            $('#img_' + documentid).remove();
                        }
                    },
                    error: function () {
                        return false;
                    }
                });

            }
        }
    }
    var $idown;

    //Download files
    function downloadSelectedFile(fileId) {

        var path = "/DocumentManagement/DownloadFile?FileId=" + fileId;

        if ($idown) {

            $idown.attr('src', path);

        }

        else {

            $idown = $('<iframe>', { id: 'idown', src: path }).hide().appendTo('body');
        }
        // var path = "DocumentManagement/DownloadFile?FileId=" + fileId;
        //$.get("/DocumentManagement/DownloadFile", { fileId: fileId }, function (data) {
        //    alert("file is downloaded");
        //});
        // }); 

    }

    function InitiateCreateDocumentPortal() {
        //debugger;
        DMurl = '<%=DocumentManagementUrl%>';

            var createDefaultFolders = true;
            if ($('#chkDefaultFolders') != undefined && $('#chkDefaultFolders').length > 0)
                createDefaultFolders = $('#chkDefaultFolders')[0].checked;
            $.ajax({
                type: "POST",
                url: "/documentmanagement/Index?chkDefaultFolder=" + createDefaultFolders + "&ticketid=<%=projectPublicID%>&typeOfFolder=&isUpload=false&isTabSelected=<%=IsTabActive%>",
            success: function () {

            }

        });
        var url = hdnConfiguration.Get("RequestUrl");
        window.parent.UgitOpenPopupDialog(DMurl, '', 'Link To Document', 90, 90, false, hdnConfiguration.Get("RequestUrl"));
    }

</script>


<asp:HiddenField ID="fileAttchmentId" runat="server" />
<div runat="server" id="divLinkToDoc">
<div class="documentUpload-btnWrap">
    <dx:ASPxButton ID="btnLinkDocument" runat="server" Text="Link and Upload Document" CssClass="primary-blueBtn">
        <ClientSideEvents Click="function(s, e) { UpLoadDocument(true); }" />
    </dx:ASPxButton>
</div>

<div class="documentUpload-btnWrap linkExist">
    <dx:ASPxButton ID="btnLinkExistingDocument" runat="server" Text="Link Existing Document" CssClass="link_ext_document primary-blueBtn">
        <ClientSideEvents Click="function(s, e) { UpLoadDocument(false); }" />
    </dx:ASPxButton>
</div>
</div>
<asp:Panel ID="pnlCreateDocPortal" runat="server" Style="display: none;">
    <label runat="server" id="lblPortalError1" style="display: none; cursor: pointer;"></label>
    <div style="padding-top: 10px;" id="div1" runat="server">
        <a id="btnCreateDocPortal" onclick="InitiateCreateDocumentPortal()" style="padding-top: 10px;" text="&nbsp;&nbsp;Create Document Portal&nbsp;&nbsp;" tooltip="Create Document Portal">
            <span class="button-bg">
                <b style="float: left; font-weight: normal;">Create Document Portal</b>
                <i style="float: left; position: relative; left: 2px">
                    <img src="/Content/images/add_icon.png" style="border: none;" title="" alt="" />
                </i>
            </span>                                                    
        </a>
        <span style="margin-left: 10px;">
            <input type="checkbox" id="chkDefaultFolders" checked="checked" />Create Default Folders
        </span>
    </div>
</asp:Panel>

<div id="bindMultipleLink" runat="server" class="document-linkWrap">
</div>

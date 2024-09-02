var toastrOpts = function () { };
toastrOpts.positionClass = 'toast-top-center';
toastrOpts.timeOut = 1000;
toastrOpts.extendedTimeOut = 0;
/*var baseUrl = ugitConfig.apiBaseUrl + "DocumentManagement/"; */
var baseUrl = window.location.origin + "/DocumentManagement/";
$(function () {
    function GetParameterValues(param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < url.length; i++) {
            var urlparam = url[i].split('=');
            if (urlparam[0] === param) {
                return urlparam[1];
            }
        }
        return null;
    }

    function loadTree() {
        $.ajax({
            url: baseUrl + "BuildTree/",
            beforeSend: function () {
                $('#loading-image').show();
            },
            success: function (result) {
                if (result !== "" && result.IsSuccess !== undefined && (result.IsSuccess === false || result.IsSuccess === true)) {
                    if (result.ErrorCode === -999) {
                        //window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                        return;
                    }
                }
                $('#tree').dynatree({
                    minExpandLevel: 2,
                    selectMode: 1,
                    generateIds: true,
                    autoFocus: true, // Set focus to first child, when expanding or lazy-loading.
                    keyboard: true, // Support keyboard navigation.
                    children: JSON.parse(result),
                    activeVisble: true,
                    onPostInit: function (isReloading, isError) {

                        // check if id is passed then select it
                        var fileId = GetParameterValues('fid');
                        if (fileId !== null) {
                            fileId = "F" + fileId;
                            var tree = $("#tree").dynatree("getTree");
                            var activeNodeDetails = tree.activateKey("D0"); // ALL node
                            if (activeNodeDetails !== null) {
                                activeNodeDetails.visit(function (node) {
                                    if (node.data.key === fileId) {
                                        node.activate();
                                        return false; // stop traversal (if we are only interested in first match)
                                    }
                                });
                            }
                        }

                        var directoryId = GetParameterValues('did');

                        if (directoryId !== null) {
                            directoryId = "D" + directoryId;
                            var tree = $("#tree").dynatree("getTree");
                            var activeNodeDetails = tree.activateKey("D0"); // ALL node

                            if (activeNodeDetails !== null) {
                                activeNodeDetails.visit(function (node) {
                                    if (node.data.key === directoryId) {
                                        node.activate();
                                        return false; // stop traversal (if we are only interested in first match)
                                    }
                                });
                            }
                        }
                        //test 
                        //var parseRoot = JSON.parse(result);

                        //var jsonData = JSON.parse(result).children;

                        //$(jsonData).each(function (index, item) {

                        //    var titleName = item.title;

                        //    if (titleName == parseRoot.folderName) {//Remove Hardcode value

                        //        if (parseRoot.isUpload) {

                        //            $("#FileUploadDialog").dialog('open');
                        //        }

                        //        $('li:contains(' + titleName + '):last').click();
                        //    }
                        //});

                    },
                    onActivate: function (node) {
                        var node1 = node;
                        var title = '';
                        while (node1 === node1.getParent()) {
                            if (node1.data.title !== null) {
                                title = node1.data.title + "/" + title;
                            }
                        }

                        //$("#SelectedItemId").html(title + node.data.title);
                        //$("#pathId").html(title + node.data.title);

                        var fileFolderId = node.data.ID;
                        var isFolder = node.data.isFolder;

                        //st attribute to upload file click
                        //$('#uploadfileForm').data('directoryId', directoryid);

                        // for document tab, do not list all directories for ALL
                        if (fileFolderId > 0)
                            GetTreeData(fileFolderId, isFolder);
                    }
                });

                /*********code to select default folder in dynatee*****/
                //$('.dynatree-container ').find('ul').find('li').map(function () {
                //    var folderTitle = $(this).find('.dynatree-title').text();
                //    if (folderTitle == 'ProjectTask') {
                //        $(this).find('span.dynatree-node').addClass('dynatree-active');
                //    }
                //});

            },
            complete: function () {
                $('#loading-image').hide();

                loadDirectoryDetail();

            },

        });
    }

    $("#FileUploadDialog").dialog({
        bgiframe: true,
        title: 'Upload File',
        //height: 300,
        modal: true,
        autoOpen: false,
        resizable: true,
        buttons: {
            "Cancel": function () {
                $(this).dialog("close");
            }
        }
    });

    OpenUploadDialog = function () {
        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var id = activeNodeDetails.data.ID;
        var toastrOpts = function () { };

        toastrOpts.positionClass = 'toast-top-center';
        toastrOpts.timeOut = 1000;
        toastrOpts.extendedTimeOut = 0;

        if (!id || !activeNodeDetails.data.isFolder) {
            toastr.error("Please select folder", " ", toastrOpts);
            return;
        }

        try {
            $("#FileUploadDialog").dialog('option', 'width', 350);
            $("#FileUploadDialog").dialog('open');
        } catch (e) {
            console.log(e);
        }
        // $('#uploadfileForm').attr('action', MyAppUrlSettings.MyUsefulUrl + 'Repository/Upload?directoryId=' + id);
        $('#uploadfileForm').attr('action', 'Upload?directoryId=' + id);
    };

    var $idown;
    function fileDownload() {
        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var fileId = activeNodeDetails.data.ID;
        //var path = MyAppUrlSettings.MyUsefulUrl + "DocumentManagement/DownloadFile?FileId=" + fileId;
        var path = "DownloadFile?FileId=" + fileId;
        //window.location.href = path;
        //window.open(path);        

        if ($idown) {
            $idown.attr('src', path);
        } else {
            $idown = $('<iframe>', { id: 'idown', src: path }).hide().appendTo('body');
        }
    }

    CreateFolder = function () {
        var win = $('<div><p>Enter your new folder name</p></div>');
        var userInput = $('<input type="text" class="form-control required" style="width:100%"></input>');
        userInput.appendTo(win);
        var userValue = '';

        $(win).dialog({
            'modal': true,
            'title': 'Create New Folder',
            'buttons': {
                'Ok': function () {
                    if ($(userInput).val().length > 0) {
                        CreateNewFolderCallback($(userInput).val());
                        $(this).dialog('close');
                    }
                    else {
                        toastr.error("Please provide folder name.", "", toastrOpts);
                        return;
                    }
                },
                'Cancel': function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    function CreateNewFolderCallback(folderName) {

        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var directoryid = activeNodeDetails.data.ID;

        if (!directoryid) {
            directoryid = 0;
        }

        $.ajax({
            type: "POST",
            //url: MyAppUrlSettings.MyUsefulUrl + "Repository/CreateNewFolder",
            url: "CreateNewFolder",
            data: { foldername: folderName, DirectoryId: directoryid },
            cache: false,
            dataType: "json",
            success: function (data) {
                if (data !== "" && data.IsSuccess !== undefined && (data.IsSuccess === false || data.IsSuccess === true)) {
                    if (data.ErrorCode === -999) {
                        window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                        return;
                    }
                }
                if (data !== "" && data.IsSuccess !== undefined && data.IsSuccess === false) {
                    toastr.error(data.Message);
                } else {
                    activeNodeDetails.addChild(JSON.parse(data));
                    activeNodeDetails.deactivate();
                    activeNodeDetails.activate();

                    if (!activeNodeDetails.isExpanded()) {
                        activeNodeDetails.toggleExpand();
                    }
                }
            },
            Error: function (data) {
                toastr.error("Failed to create folder " + folderName);
            }
        });
    }

    $("#btnUpload").click(function () {

        if ($("#files").val() === "") {
            toastr.error("Please select file", "", toastrOpts);
            return false;
        }

        $("form#uploadfileForm").submit(function (e) {

            e.preventDefault();
            e.stopImmediatePropagation();

            $("#btnUpload").attr("disabled", "disabled");
            $("#btnUpload").val("Uploading...");
            var formData = new FormData(this);

            $.ajax({
                url: $('#uploadfileForm').attr('action'),
                type: 'POST',
                data: formData,
                success: function (data) {
                    if (data !== "" && data.IsSuccess !== undefined && (data.IsSuccess === false || data.IsSuccess === true)) {
                        if (data.ErrorCode === -999) {
                            window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                            return;
                        }
                    }

                    $("#btnUpload").attr("disabled", null);
                    $("#btnUpload").val("Upload");

                    if (data !== "" && data.IsSuccess !== undefined && data.IsSuccess === false) {
                        if (data.ErrorCode === -111) {
                            $("#FileUploadDialog").dialog('close');
                        }
                        toastr.error(data.Message);
                    }
                    else if (data !== "" && data.IsSuccess !== undefined && data.IsSuccess === true) {
                        toastr.success("File Uploaded successfully.");
                        $("#FileUploadDialog").dialog('close');

                        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
                        activeNodeDetails.deactivate();
                        activeNodeDetails.activate();
                    }
                    else {

                        //// do not add file as node to tree
                        //var activeNodeDetails = $("#tree").dynatree("getActiveNode");
                        //activeNodeDetails.addChild(JSON.parse(data));
                        //activeNodeDetails.deactivate();
                        //activeNodeDetails.activate();

                        //if (!activeNodeDetails.isExpanded()) {
                        //    activeNodeDetails.toggleExpand();
                        //}

                        toastr.success("File Uploaded successfully.");
                        $("#FileUploadDialog").dialog('close');
                    }
                },
                Error: function (data) {
                    $("#btnUpload").attr("disabled", null);
                    $("#btnUpload").val("Upload");
                    toastr.error("Failed to upload, please try after sometime.");
                },
                cache: false,
                contentType: false,
                processData: false
            });
            return false;
        });

        return true;
    });

    //$("#UploadFileLink").click(function () {
    //    OpenUploadDialog();
    //});

    //$("#CreateNewFolderLink").click(function () {
    //    CreateFolder();
    //});

    $('#tree').contextMenu({
        selector: '.dynatree-node',
        build: function ($triggerElement, e) {

            var role = $("#userRole").val();
            if ($triggerElement.hasClass("dynatree-folder") && (role === "Admin" || role === "SuperAdmin")) {
                return {
                    callback: function (key, options) {
                        if (key === "fileUpload") {
                            OpenUploadDialog();
                        }
                        else if (key === "createFolder") {
                            CreateFolder();
                        }
                    },
                    items: {
                        "createFolder": {
                            name: "Create Folder",
                            icon: ".context-menu-item.icon-add"
                        },
                        "fileUpload": {
                            name: "Upload File",
                            icon: ".context-menu-item.icon-add"
                        }
                    }
                };
            }
            if (!($triggerElement.hasClass("dynatree-folder"))) {
                return {
                    callback: function (key, options) {
                        if (key === "fileDownload") {
                            fileDownload();
                        }
                        //else if (key === "CheckedOut") {
                        //    checkOut();
                        //}
                        //else if (key === "Undo CheckedOut") {
                        //    undoCheckOut();
                        //}
                        //else if (key === "CheckedIn") {
                        //    CheckedIn();
                        //}
                    },
                    items: {
                        "fileDownload": {
                            name: "Download File",
                            icon: ".context-menu-item.icon-add"
                        }//,
                        //"Undo CheckedOut": {
                        //    name: "Undo CheckedOut",
                        //    icon: ".context-menu-item.icon-add"
                        //},
                        //"CheckedOut": {
                        //    name: "Check Out & Download",
                        //    icon: ".context-menu-item.icon-add"
                        //},
                        //"CheckedIn": {
                        //    name: "Check In",
                        //    icon: ".context-menu-item.icon-add"
                        //}
                    }
                };
            }
        }
    });

    $("#tree").mouseup(function (e) {
        if (e.button === 2) { //RIGHT MOUSE UP
            e.target.click();
        }
    });

    function CheckedIn() {
        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var fileId = activeNodeDetails.data.ID;

        $.ajax({
            type: "POST",
            url: "IsCheckOutByMe",
            data: { fileId: fileId },
            cache: false,
            dataType: "json",
            success: function (data) {

                if (data !== "" && data.IsSuccess !== undefined && (data.IsSuccess === false || data.IsSuccess === true)) {
                    if (data.ErrorCode === -999) {
                        window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                        return;
                    }
                }

                if (data.Data.IsSuccess) {
                    //toastr.success(data.Message);
                    $("#versiondiv").html("Versions: <br>" + data.version);

                    //$('#uploadfileForm').attr('action', MyAppUrlSettings.MyUsefulUrl + 'Repository/UploadCheckIn?id=' + fileId);
                    $('#uploadfileForm').attr('action', 'UploadCheckIn?id=' + fileId);
                    $("#FileUploadDialog").dialog('open');
                    //checkInFileDialog(FileId);
                }
                else {
                    toastr.error(data.Data.Message);
                }
            },
            Error: function (data) {
                toastr.error("Failed to checkin, please try after sometime.");
            }
        });
    }

    function undoCheckOut() {
        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var fileId = activeNodeDetails.data.ID;

        $.ajax({
            type: "POST",
            //url: MyAppUrlSettings.MyUsefulUrl + "Repository/UndoCheckout",
            url: "UndoCheckout",
            data: { fileId: fileId },
            cache: false,
            dataType: "json",
            success: function (data) {

                if (data !== "" && data.IsSuccess !== undefined && data.IsSuccess === false || data.IsSuccess === true) {
                    if (data.ErrorCode === -999) {
                        window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                        return;
                    }
                    if (data.IsSuccess === false) {
                        toastr.error(data.Message);
                    }
                    else if (data.IsSuccess === true) {
                        $('#tblFileList').find('input:checkbox:first').prop('checked', false);
                        toastr.success(data.Message);
                    }
                }
            },
            Error: function (data) {
                toastr.error(data.Message);
            }
        });
    }

    function checkOut() {

        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var fileId = activeNodeDetails.data.ID;

        $.ajax({
            type: "POST",
            //  url: MyAppUrlSettings.MyUsefulUrl + "Repository/Checkout", 
            url: "Checkout",

            data: { fileId: fileId },
            cache: false,
            dataType: "json",
            success: function (data) {

                if (data !== "" && data.IsSuccess !== undefined && (data.IsSuccess === false || data.IsSuccess === true)) {
                    if (data.ErrorCode === -999) {
                        window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                        return;
                    }
                }
                if (data.IsSuccess) {
                    //toastr.success(data.Message);
                    //var path = MyAppUrlSettings.MyUsefulUrl + "Repository/DownloadLatestFile?FileId=" + fileId;
                    var path = "DownloadLatestFile?FileId=" + fileId;

                    $('#tblFileList').find('input:checkbox:first').trigger('click');
                    $('#tblFileList').find('input:checkbox:first').prop('checked', true);
                    window.location.href = path;
                }
                else {
                    toastr.error(data.Message);
                }
            },
            Error: function (data) {

            }
        });
    }

    deleteFileConfirmDailog = function () {

        if ($('.checkbox-file:checked').length === 0) {
            toastr.error("Please select file(s) to delete");
            return;
        }

        var selectedFiles = '<p>';
        $('.checkbox-file:checked').each(function () {
            selectedFiles += $(this).attr('filename') + '</br>';
        });
        selectedFiles += '</p>';

        var win = $('<div class="ui-dialog-confirm-dialog"><p>Delete will delete documents. Are you sure you want to delete?</p>' + selectedFiles + '</div>');

        $(win).dialog({
            'modal': true,
            'width': 'auto',
            'dialogClass': 'ui-dialog-confirm-dialog',
            'title': 'Please Confirm',
            'buttons': {
                'Ok': function () {
                    deleteFiles();
                    $(this).dialog('close');
                },
                'Cancel': function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    LinkFile = function () {
        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var directoryid = activeNodeDetails.data.ID;

        if (!directoryid) {
            directoryid = 0;
        }

        var selectedFiles = [];
        var selectedfilesId = [];

        $('.checkbox-file:checked').each(function () {

            selectedfilesId.push($(this).attr('fileid'));
            selectedFiles.push($(this).attr('filename'));

        });

        if (selectedFiles.length === 0) {
            toastr.error("Please select the file(s) to be linked.");
            return;
        }

        var parentFramID = null;
       
        if (localStorage.getItem('callbackcnt') === null) {
            parentFramID = localStorage.getItem('parentID');
            var cnt = localStorage.getItem('callbackcnt');
            localStorage.setItem('callbackcnt', parseInt(cnt++));
        }
        else {
            parentFramID = localStorage.getItem('OldparentID');
        }
        //Remove local storage
        //Add flag
        if (parentFramID != null && window.parent.parent.frames[parentFramID] != undefined &&
            window.parent.parent.frames[parentFramID].contentWindow.setSelectedDocumentDetails != undefined) {
            
            window.parent.parent.frames[parentFramID].contentWindow.setSelectedDocumentDetails(selectedFiles, selectedfilesId);
            
            localStorage.removeItem("parentID");
        }

        var allframes = window.parent.parent.frames;

        for (var index = 0; index < allframes.length; index++) {

            if (allframes[index].window.setSelectedDocumentDetails != undefined) {
                //SelectedFiles[] 
                allframes[index].window.setSelectedDocumentDetails(selectedFiles, selectedfilesId);
            }

            //  console.log(allframes[index].location);
        }


        // console.log(document.location.href);
    };

    deleteFiles = function () {
        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var directoryid = activeNodeDetails.data.ID;

        if (!directoryid) {
            directoryid = 0;
        }

        var selectedFiles = [];
        $('.checkbox-file:checked').each(function () {
            selectedFiles.push($(this).attr('fileid'));
        });

        if (selectedFiles.length === 0) {
            toastr.error("Please select file(s) to delete");
            return;
        }

        $.ajax({
            url: "DeleteFiles",
            type: "POST",
            data: { DirectoryId: directoryid, FileIds: selectedFiles },
            cache: false,
            success: function (data) {
                $('#fileList').empty();
                $('#fileList').append(data);
                toastr.success("File(s) deleted successfully");
            },
            error: function (data) {
                toastr.error("Failed to delete file(s)");
            }
        });
    };

    deleteFolderConfirmDailog = function () {

        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var directoryid = activeNodeDetails.data.ID;
        var directoryName = activeNodeDetails.data.title;

        if (!directoryid) {
            directoryid = 0;
        }

        if (directoryid === 0) {
            toastr.error("Please select folder to delete");
            return;
        }

        var win = $('<div class="ui-dialog-confirm-dialog"><p>Delete will delete folder and all documents in the folder.</br>Are you sure you want to delete folder ' + directoryName + '?</p></div>');

        $(win).dialog({
            'modal': true,
            'width': 'auto',
            'dialogClass': 'ui-dialog-confirm-dialog',
            'title': 'Please Confirm',
            'buttons': {
                'Ok': function () {
                    deleteFolder();
                    $(this).dialog('close');
                },
                'Cancel': function () {
                    $(this).dialog('close');
                }
            }
        });
    };

    deleteFolder = function () {

        var activeNodeDetails = $("#tree").dynatree("getActiveNode");
        var directoryid = activeNodeDetails.data.ID;

        if (!directoryid) {
            directoryid = 0;
        }

        if (directoryid === 0) {
            toastr.error("Please select folder to delete");
            return;
        }

        $.ajax({
            url: "DeleteFolder",
            type: "POST",
            data: { DirectoryId: directoryid },
            cache: false,
            success: function (data) {
                $('#fileList').empty();
                $('#fileList').append(data);

                // remove selected node
                $("#tree").dynatree("getActiveNode").remove();

                toastr.success("Folder deleted successfully");
            },
            error: function (data) {
                toastr.error("Failed to delete folder");
            }
        });
    };

    if (window.location.href.indexOf('Login') < 0) {
        loadTree();
    }

});

//$(document).ready(function () {
//    GetTreeData(0, true);
//});

function loadDirectoryDetail() {
    try {
        var isActiveChild = false;
        if ($("#tree").dynatree("getRoot").childList == undefined)
            return;
        var rootNode = $("#tree").dynatree("getRoot").childList[0].data;
        var childFolder = rootNode.children;
        var childFolderId = "";
        if (childFolder == undefined || childFolder.length == 0) {
            if (rootNode != undefined )
                GetTreeData(rootNode.ID, true);
            return;
        }

        for (var index = 0; index < childFolder.length; index++) {

            if (childFolder[index].title == "Tasks" && childFolder[index].isTabSelected)// check Which tab is selected.
            {
                GetTabWiseDocument(childFolder[index].ID, rootNode.isUpload);
                //GetTreeData(childFolder[index].ID, true);

                //if (rootNode.isUpload) {
                //    $('#uploadfileForm').attr('action', 'Upload?directoryId=' + childFolder[index].ID);
                //    $("#FileUploadDialog").dialog('open');

                //}
                isActiveChild = true;
                break;
            }

            else if (childFolder[index].title == "Sprints" && childFolder[index].isTabSelected) {
                //GetTreeData(childFolder[index].ID, true);
                //if (rootNode.isUpload) {
                //    $('#uploadfileForm').attr('action', 'Upload?directoryId=' + childFolder[index].ID);
                //    $("#FileUploadDialog").dialog('open');
                //}
                GetTabWiseDocument(childFolder[index].ID, rootNode.isUpload);
                isActiveChild = true;
                break;
            }

            else if (childFolder[index].title == "Budgets" && childFolder[index].isTabSelected) {
                //GetTreeData(childFolder[index].ID, true);
                //if (rootNode.isUpload) {
                //    $('#uploadfileForm').attr('action', 'Upload?directoryId=' + childFolder[index].ID);
                //    $("#FileUploadDialog").dialog('open');
                //}
                GetTabWiseDocument(childFolder[index].ID, rootNode.isUpload);
                isActiveChild = true;
                break;
            }

            else if (childFolder[index].title == "Status" && childFolder[index].isTabSelected) {
                GetTabWiseDocument(childFolder[index].ID, rootNode.isUpload);
                isActiveChild = true;
                break;
            }

        }

        if (!isActiveChild) {
            GetTreeData(rootNode.ID, true);
        }

    } catch (e) {
        console.log(e);
    }
}

function GetTabWiseDocument(childFolderId, isUpload) {
    GetTreeData(childFolderId, true);

    if (isUpload) {
        $('#uploadfileForm').attr('action', 'Upload?directoryId=' + childFolderId);

        $("#FileUploadDialog").dialog('open');

    }


}

function GetTreeData(fileFolderId, isFolder) {

    $.ajax({
        // url: MyAppUrlSettings.MyUsefulUrl +"Repository/SelectFile",
        url: "SelectFile",
        type: "POST",
        data: { id: fileFolderId, isFolder: isFolder },
        success: function (data) {
            $('#fileList').empty();
            $('#fileList').append(data);
        },
        error: function (data) {
        }
    });
}

function callAfterFrameLoad(control) {
    $(control).parent().find(".loading111").remove();
    try {
        control.contentWindow.clickUpdateSize();
        control.contentWindow.adjustControlSize();
    } catch (ex) {
        console.log(ex);
    }
}

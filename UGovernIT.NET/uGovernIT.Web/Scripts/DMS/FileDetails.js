function SubmitDirectory(_directoryId) {
    var directoryId = _directoryId;
    var activeNodeDetails = $("#tree").dynatree("getActiveNode");
    for (var i = 0; i < activeNodeDetails.childList.length; i++) {
        if (activeNodeDetails.childList[i].data.ID == directoryId) {
            activeNodeDetails.childList[i].activate();
        }
    }
}

var $idown;
function SubmitFile(fileId) {
    //var path = MyAppUrlSettings.MyUsefulUrl + "Repository/DownloadFile?FileId=" + fileId;
    var path = "DownloadFile?FileId=" + fileId;
    //window.location.href = path;
    //window.open(path);

    if ($idown) {
        $idown.attr('src', path);
    } else {
        $idown = $('<iframe>', { id: 'idown', src: path }).hide().appendTo('body');
    }

    //$.ajax({
    //    url: MyAppUrlSettings.MyUsefulUrl +"Repository/FileVersions",
    //    type: "Post",
    //    data: { FileId: fileId },
    //    success: function (data) {
    //        if (data != "" && data.IsSuccess != undefined && data.IsSuccess === false) {
    //            if (data.ErrorCode == -999) {
    //                window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
    //                return;
    //            }
    //        } else if (data != "" && data.IsSuccess != undefined && data.IsSuccess === true) {
    //            if (data.ErrorCode == -999) {
    //                window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
    //                return;
    //            }
    //        } 
    //        $('#fileList').html(data);
    //    },
    //    error: function (data) {
    //    }
    //});
}

function FileDetails(fileId)
{
    
    window.parent.UgitOpenPopupDialog('/Layouts/uGovernIT/DelegateControl.aspx?control=filedetails&fileID=' + fileId, '', 'Document Details', 50, 90, 0, '%2flayouts%2fugovernit%2fdelegatecontrol.aspx');
  
}


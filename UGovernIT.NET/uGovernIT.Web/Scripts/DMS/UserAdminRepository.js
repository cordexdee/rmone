
var UserAdminRepository = function () {

    var init = function () {

    },
        
        userData = function () {
            var userId = $("#UserId").val();
            var UserName = $("#UserName").val();
            if (UserName == undefined || UserName == "") {
                toastr.error("Select user");
                return false;
            }
            $.ajax({
                url: "LoadUserAccessedData",
                type: "POST",
                data: { userId: userId },
                beforeSend: function () {
                    $('#loading-image').show();
                },
                success: function (data) {
                    $('#loading-image').show();
                    if (data != "" && data.IsSuccess != undefined && (data.IsSuccess === false || data.IsSuccess === true)) {
                        if (data.ErrorCode == -999) {
                            window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                            return;
                        }
                    } 
                    $('#fileDirectoryList').html(data);
                    var c = 0;                                // create a counter
                    $('.fullpath').each(function () {
                        var text = $(this).html().split('/'), // split your text strings
                            len = text.length,                // get lenght of splitted parts
                            result = [];                      // create array
                        for (i = 0; i < len; i++) {              // looping 'len' times....
                            result[i] = '<span class="foldername">' + text[i] + '</span>';  // populate array
                        }
                        $(this).html(result.join(' / '));       // apply edits
                    });
                    $("#SavePermissionDetailId").show();
                },
                complete: function () {
                    $('#loading-image').hide();
            },
            error: function (data) {
            }
        });
    };
    SaveAccessDetails = function () {
        var UserId = $("#UserId").val();
        var readSelected = [];
        var writeSelected = [];
        $('#fileDirectoryList tr').each(function () {
            var optsRead = {};
            var optsWrite = {};
            var length = $(this).find('input:checked').length;
            if (length > 0) {
                var File = $(this).find('input[name=FileId]').val();
                var Directory = $(this).find('input[name=DirectoryId]').val();
                var ParentID = $(this).find('input[name=ParentDirectoryId]').val();
                var checkedData = $(this).find('input:checked');
                checkedData.each(function () {
                    var accessType = $(this).attr('value');
                    if (accessType == 1) {
                        var readID = $(this).attr('name');
                        optsRead.ID = readID;
                        optsRead.IsFile = File;
                        optsRead.ParentID = ParentID;
                        optsRead.IsFolder = Directory;
                        readSelected.push(optsRead);
                    }
                    else {
                        var writeID = $(this).attr('name');
                        optsWrite.ID = writeID;
                        optsWrite.IsFile = File;
                        optsWrite.ParentID = ParentID;
                        optsWrite.IsFolder = Directory;
                        writeSelected.push(optsWrite);
                    }
                })
            }
        });
        accessData(UserId, readSelected, writeSelected);
    };
    accessData = function (userId, ReadAccess, WriteAccess) {
        var readData = JSON.stringify(ReadAccess);
        var writeData = JSON.stringify(WriteAccess);
        $.ajax({
            url: "UpdateUserAccessToFilesAndDirectories",
            type: "Post",
            data: { userId: userId, ReadAccess: readData, WriteAccess: writeData },
            success: function (data) {
                if (data != "" && data.IsSuccess != undefined && data.IsSuccess === false) {
                    if (data.ErrorCode == -999) {
                        window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                    }
                    toastr.error(data.Message);
                } else if (data != "" && data.IsSuccess != undefined && data.IsSuccess === true) {
                    if (data.ErrorCode == -999) {
                        window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                    }
                    toastr.success(data.Message);
                    userData();
                }
            },
            error: function (data) {
            }
        });

    };
    return {
        init: init,
        userData: userData,
        SaveAccessDetails: SaveAccessDetails
    }
}();

$(document).ready(function () {
    $("#UserId").val("");
    $("#SearchUserPermissionId").click(function () {
        var userId = $("#UserId").val();
        if (userId == undefined || userId == "") {
            toastr.error("Please enter valid user");
            return false;
        }
        UserAdminRepository.userData();
    });
    $('form input').keydown(function (e) {
        if (e.keyCode == 13) {
            UserAdminRepository.userData();
            e.preventDefault();
            return false;
        }
    });
    $("#SavePermissionDetailId").click(function () {
        UserAdminRepository.SaveAccessDetails();
    });
    $("#UserName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "UserAdminRepository",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    if (data != "" && data.IsSuccess != undefined && (data.IsSuccess === false || data.IsSuccess === true)) {
                        if (data.ErrorCode == -999) {
                            window.location.href = MyAppUrlSettings.MyUsefulUrl + "Account/LogIn";
                        }
                    }
                    else {
                        if (data.length > 0 && data != "") {
                            response($.map(data, function (item) {
                                $("#UserId").val(item.Id);
                                //return { label: item.UserName, value: item.UserName, id: item.Id };
                            }));
                        }
                        else {
                            $("#UserId").val("");
                        }
                    }
                }
            })
        },
        select: function (event, ui) {
            //$("#UserId").val(ui.item.label); // display the selected text
            $("#UserId").val(ui.item.Id); // save selected id to hidden input
        },
        //messages: {
        //    noResults: "", results: ""
        //}
    });
    //UserAdminRepository.userData();
});

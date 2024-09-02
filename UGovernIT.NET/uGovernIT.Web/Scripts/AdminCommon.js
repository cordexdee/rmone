var AdminCommon = function () {
    showMessage = function (message, thenFade, action, title, toastrOpts) {
        if (!toastrOpts) {
            toastr.clear(null, { force: true });
            toastrOpts = {
                timeOut: 1000
            };
        }
        var msg = '<span>' + message + '</span>';
        if (thenFade) {
            // don't add spinner
        } else {
            msg = msg + '<span> ' + smallWait + '</span>';
            toastrOpts.positionClass = 'toast-top-center';
            toastrOpts.timeOut = 0;
            toastrOpts.extendedTimeOut = 0;
        }
        var $toast;
        switch (action) {
            case 'success':
                $toast = toastr.success(msg, title, toastrOpts);
                break;
            case 'error':
                $toast = toastr.error(msg, title, toastrOpts);
                break;
            case 'warning':
                $toast = toastr.warning(msg, title, toastrOpts);
                break;
            case 'info':
            default:
                $toast = toastr.info(msg, title, toastrOpts);
                break;
        }
        return $toast;
    },
        refresh = function () {
            var $div = $('#tenantDiv');//.html(spinner);
            var data = {};
            $.get('CreateTenant', data, function (html) {
                $div.html(html);
            }).fail(function (x) {
            });
        };
    return {
        showMessage: showMessage,
        refresh: refresh
    };
}();
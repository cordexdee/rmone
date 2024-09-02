/// <reference path="jquery.form.js" />
var Admin = function () {
    var init = function () {

    },

        loadModule = function (action, url, title, width, hight, requestUrl, setFlag) {
            actionName = action.toLowerCase();
            window.parent.UgitOpenPopupDialog(url, '', title, width, hight, requestUrl, setFlag);
        };

    getTab = function (action) {
        if (!action) {
            return;
        }
        var url = '/Admin/' + action;
        window.location = url;
    },

    redirectModule = function (href) {
        window.location.href = href;
    };

    return {
        init: init,
        loadModule: loadModule,
        getTab: getTab,
        redirectModule: redirectModule
    }
}();
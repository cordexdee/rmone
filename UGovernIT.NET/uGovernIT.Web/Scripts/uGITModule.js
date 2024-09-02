(function ($) {
    $.ugit = {};
    var baseUrl = ugitConfig.apiBaseUrl;

    $.ugit.getPriorityMapping = function (moduleName) {
        return $.get({ url: $.ugit.getBaseUrl("api/module/GetPriorityMappings"), data: { moduleName: moduleName } });
    };

    $.ugit.getRequestTypeDependent = function (moduleName, requestTypeID, locationID, requestor) {
        return $.get({ url: $.ugit.getBaseUrl("api/module/GetRequestTypeDependent"), data: { moduleName: moduleName, requestTypeID: requestTypeID, locationID: locationID, requestor: requestor } });
    };

    $.ugit.getBaseUrl = function (requestPath) {
        if (baseUrl.endsWith("/")) {
            return baseUrl + requestPath;
        }
        return baseUrl + '/' + requestPath;
    };

})(jQuery);


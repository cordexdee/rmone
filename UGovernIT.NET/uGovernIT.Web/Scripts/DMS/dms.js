/// <reference path="jquery.form.js" />
var Dms = function () {
    //var init = function () {
    //    var $tabs = $('#DmsTabs li');
    //    $tabs.find('a').on('click', function () {
    //        var $this = $(this);
    //        $tabs.removeClass('active');
    //        $(this).closest('li').addClass('active');

    //        getTab($this.attr('href'), $this);
    //    });
    //    var $a = $tabs.filter('.active').find('a');
    //    $a.css('cursor', 'pointer');
    //    var href = $a.attr('href');
    //    getTab(href, $a);
    //},

    var $tabs = $('#DmsTabs li');
    $('#DmsTabs li').find('a').on('click', function () {
        var $tabs = $('#DmsTabs li');
        var $this = $(this);
        $tabs.removeClass('active');
        $(this).closest('li').addClass('active');

       // getTab($this.attr('href'), $this);
    });
    getTab = function (href, $a) {
        var $this = $(this);
        jQuery(this).parent('li').addClass('active').removeClass('active');
    if (!href) {
        return;
    }
    var url = href;
    //window.location = url;
    };
    return {
        //init: init,
        getTab: getTab
    }
}();
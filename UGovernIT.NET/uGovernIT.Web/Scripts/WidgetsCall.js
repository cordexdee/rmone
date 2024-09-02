

function handleWidget(widgetId) {
    
    console.log(widgetId);
    
    $.ajax({
        url: '/api/widget/GetWidgetsResponse?widgetId=' + widgetId,
        type: 'Get',
        contentType:"application/json;charset=utf-8",
        success: function (data) {
            if (data) {
                debugger;
                window.parent.UgitOpenPopupDialog(data.Url, "", data.Title, data.Width + 'px', data.Height + 'px', 0, '%2fLayouts%2fuGovernIT%2fDelegateControl.aspx**stoprefreshpage');
            }            
        },
        error: function (data) {
            
        }


    })
}
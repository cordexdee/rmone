
window.onload = function () {
    //var chart = new CanvasJS.Chart("chartContainer",
    //    {
    //        title: {
    //            text: "Projects",
    //            fontFamily: "Impact",
    //            fontWeight: "normal"
    //        },

    //        legend: {
    //            verticalAlign: "bottom",
    //            horizontalAlign: "center"
    //        },
    //        data: [
    //            {
    //                //startAngle: 45,
    //                indexLabelFontSize: 20,
    //                indexLabelFontFamily: "Garamond",
    //                indexLabelFontColor: "darkgrey",
    //                indexLabelLineColor: "darkgrey",
    //                indexLabelPlacement: "outside",
    //                type: "doughnut",
    //                showInLegend: true,
    //                dataPoints: [
    //                    { y: 53.37 },
    //                    { y: 35.0 },
    //                    { y: 7 },
    //                    { y: 2 },
    //                    { y: 5, legendText: "", indexLabel: "" }
    //                ]
    //            }
    //        ]
    //    });

    //chart.render();
}
$(document).ready(function () {
    var flag = 0;
    $('.sidebar-icon').bind("click", function () {
        if (flag === 0) {
            flag++;
            $('.logoText').css("display", "none");
            $('.logoText1').css("display", "block");
        }
        else {
            flag--;
            $('.logoText1').css("display", "none");
            $('.logoText').css("display", "block");
        }
    });
});

/*
$(function () {
    var current_progress = 0;
    var current_progress1 = 0;
    var current_progress2 = 0;
    var current_progress3 = 0;
    var openTicket = (($('.openTicket').text()) * 100) / 200;
    var Unassigned = (($('.highPriority').text()) * 100) / 200;
    var waitingOnMe = (($('.waitingOnMe').text()) * 100) / 200;
    var highPriority = (($('.Unassigned').text()) * 100) / 200;

    var interval = setInterval(function () {
        current_progress += 10;

        $("#dynamic")
            .css("width", current_progress + "%")
            .attr("aria-valuenow", current_progress)

        if (current_progress >= openTicket)
            clearInterval(interval);

    }, 1000);
    var interval1 = setInterval(function () {

        current_progress1 += 10;
        $("#dynamic1")
            .css("width", current_progress1 + "%")
            .attr("aria-valuenow1", current_progress1)

        if (current_progress1 >= Unassigned)
            clearInterval(interval1);
    }, 1000);

    var interval2 = setInterval(function () {

        current_progress2 += 10;
        $("#dynamic2")
            .css("width", current_progress2 + "%")
            .attr("aria-valuenow2", current_progress2)

        if (current_progress2 >= waitingOnMe)
            clearInterval(interval2);
    }, 1000);

    var interval3 = setInterval(function () {

        current_progress3 += 10;
        $("#dynamic3")
            .css("width", current_progress3 + "%")
            .attr("aria-valuenow3", current_progress3)

        if (current_progress3 >= highPriority)
            clearInterval(interval3);
    }, 1000);

});
*/

//google.charts.load('current', { 'packages': ['corechart'] });
//google.charts.setOnLoadCallback(drawVisualization);

function drawVisualization() {
    // Some raw data (not necessarily accurate)
    var data = google.visualization.arrayToDataTable([
        ['', 'Days to resolve', 'Days to respond', 'None'],
        ['Mar18', 57, 24, 43],
        ['Apr18', 0, 60, 44],
        ['May18', 12, 43, 25],
        ['June18', 0, 25, 15]
    ]);

    var options = {
        title: 'TSR Responsive Trends',
        // vAxis: {title: 'Cups'},
        // hAxis: {title: 'Month'},
        seriesType: 'bars',
        series: { 3: { type: 'line' } }
    };

    var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
    chart.draw(data, options);
}


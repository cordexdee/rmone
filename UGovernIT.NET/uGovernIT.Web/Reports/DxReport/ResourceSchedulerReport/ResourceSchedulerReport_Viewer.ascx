<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResourceSchedulerReport_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.ResourceSchedule.ResourceScheduler_Viewer" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<head>
    
    <link href="https://cdn.dhtmlx.com/gantt/edge/dhtmlxgantt.css" rel="stylesheet" type="text/css" />
  <%--  <script src="https://cdn.dhtmlx.com/gantt/edge/dhtmlxgantt.js"></script>--%>
<script src="/Report/DxReport/ResourceSchedulerReport/dhtmlxgantt.js"></script>
    <script src="//export.dhtmlx.com/gantt/api.js?v=6.2.3"></script>
	<script src="/Report/DxReport/ResourceSchedulerReport/dhtmlxgantt_grouping.js"></script>
    <script src="/Report/DxReport/ResourceSchedulerReport/dhtmlxgantt_marker.js"></script>
    <style>
        .ToolButtons{
            text-align:right !important;
        }
        .group_task {
            border:2px solid #BFC518;
            color:#181818;
            background: #181818;
        }
        .tooltip {
  position: relative;
  display: inline-block;
  border-bottom: 1px dotted black;
}

    </style>
</head>
<body>
    
    <div class="gantt_control ToolButtons">

           <a href="#"> <img src="/Content/images/pdf_icon.png" onclick="exportpdf()" class="imgcursor" /></a>

         <a href="#"><img src="/Content/images/excel_icon.png" onclick="gantt.exportToExcel()" /></a>
          
  </div>

<div id="gantt_here" style='width:100%; height:90vh;'></div>

<script>

   

    var globaltasks = [];

    $(function () {
        loadingPanel.Show();
         
        gantt.init("gantt_here");
        gantt.config.start_on_monday = true;
        gantt.config.scale_height = 90;

        var weekScaleTemplate = function (date) {
            var dateToStr = gantt.date.date_to_str("%d/%m");
            var endDate = gantt.date.add(gantt.date.add(date, 1, "week"), -1, "day");
            return dateToStr(date); // + " - " + dateToStr(endDate);
        };

        //var daysStyle = function (date) {
        //    var dateToStr = gantt.date.date_to_str("%D");
        //    if (dateToStr(date) == "Sun" || dateToStr(date) == "Sat") return "weekend";
        //    return "";
        //};


        gantt.config.scales = [
            { unit: "month", step: 1, format: "%F, %Y" },
            { unit: "week", step: 1, format: weekScaleTemplate }
            //{ unit: "day", step: 1, format: "%D", css: daysStyle }
        ];
            gantt.config.readonly = true;
        gantt.config.fit_tasks = true;
        gantt.config.autosize = "xy";
        
            gantt.config.columns = [
                {
                    name: "Id", label: "Task Name", tree: true, width: "200", template: function (task) {
                        return task.text;
                    }
                },
                { name: "text", label: "Title", align: "center", hide: true },
                //{ name: "id", label: "Ticket ID", align: "center" },
                //{ name: "parent", label:"parent", align:"center"},
                { name: "start_date", label: "Start Date", width:"60", align: "center", format:"%D %m/%d/%y" },
                { name: "end_date", label: "End Date", width:"60", format:"%D %m/%d/%y" },
                { name: "duration", label: "Duration", width: "40" },
                { name:"ProjectExecutive", label:"PX", width:"40" },
                { name: "ProjectManager", label: "PM", width:"40"  },
                { name: "APM", label: "APM", width:"40"  },
                { name: "Estimator", label: "Estimator", width:"40"  },
                { name: "Supritendent", label: "Super", width:"40"  },
            ];
            gantt.groupBy({
                relation_property: "Id",
                groups:<%=jsonGroupsData%>,
                group_id: "key",
                group_text: "label"
            });
           gantt.config.grid_resize = true;
            gantt.config.grid_width = 700;

        gantt.templates.task_class = function (start, end, task) {
                if (task.type == gantt.config.types.project) {
                    task.color = "black";
                }
                return "";
            };
            gantt.templates.task_text = function (s, e, task) {
                return "";
            }
        gantt.templates.leftside_text = function (start, end, task) {
            return task.text;
            };
            gantt.templates.rightside_text = function (s, e, task) {
                return task.end_date.getMonth() + "/" + task.end_date.getFullYear();
            };
          
            var date_to_str = gantt.date.date_to_str(gantt.config.task_date);
 
            var id = gantt.addMarker({
                start_date: new Date(),
                css: "today",
                title: date_to_str(new Date())
            });
            //setInterval(function () {
            //    var today = gantt.getMarker(id);
            //    today.start_date = new Date();
            //    today.title = date_to_str(today.start_date);
            //    gantt.updateMarker(id);
            //}, 1000 * 60);

            gantt.parse(<%=jsonReportData%>);
        gantt.eachTask(function (task) {
            task.$open = true;
        });
        
        gantt.render();
        loadingPanel.Hide();
       // gantt.parse(tasks);
    });


    function toggleMode(toggle) {
        toggle.enabled = !toggle.enabled;
        if (toggle.enabled) {
            toggle.innerHTML = "Set default Scale";
            //Saving previous scale state for future restore
            saveConfig();
            zoomToFit();
        } else {

            toggle.innerHTML = "Zoom to Fit";
            //Restore previous scale state
            restoreConfig();
            gantt.render();
        }
    }

    var cachedSettings = {};

	function saveConfig() {
		var config = gantt.config;
		cachedSettings = {};
		cachedSettings.scales = config.scales;
		cachedSettings.template = gantt.templates.date_scale;
		cachedSettings.start_date = config.start_date;
		cachedSettings.end_date = config.end_date;
	}

	function restoreConfig() {
		applyConfig(cachedSettings);
    }

    function applyConfig(config, dates) {
		if (config.scales[0].date) {
			gantt.templates.date_scale = null;
		}
		else {
			gantt.templates.date_scale = config.scales[0].template;
		}

		gantt.config.scales = config.scales;

		if (dates && dates.start_date && dates.start_date) {
			gantt.config.start_date = gantt.date.add(dates.start_date, -1, config.scales[0].subscale_unit);
			gantt.config.end_date = gantt.date.add(gantt.date[config.scales[0].subscale_unit + "_start"](dates.end_date), 2, config.scales[0].subscale_unit);
		} else {
			gantt.config.start_date = gantt.config.end_date = null;
		}
    }

    function zoomToFit() {
		var project = gantt.getSubtaskDates(),
			areaWidth = gantt.$task.offsetWidth;

		for (var i = 0; i < scaleConfigs.length; i++) {
			var columnCount = getUnitsBetween(project.start_date, project.end_date, scaleConfigs[i].scales[0].subscale_unit, scaleConfigs[i].scales[0].step);
			if ((columnCount + 2) * gantt.config.min_column_width <= areaWidth) {
				break;
			}
		}

		if (i == scaleConfigs.length) {
			i--;
		}

		applyConfig(scaleConfigs[i], project);
		gantt.render();
    }

    // get number of columns in timeline
	function getUnitsBetween(from, to, unit, step) {
		var start = new Date(from),
			end = new Date(to);
		var units = 0;
		while (start.valueOf() < end.valueOf()) {
			units++;
			start = gantt.date.add(start, step, unit);
		}
		return units;
    }

    //Setting available scales
	var scaleConfigs = [
		// minutes
		{
			scales: [
				{subscale_unit: "minute", unit: "hour", step: 1, format: "%H"},
				{unit: "minute", step: 1, format: "%H:%i"}
			]
		},
		// hours
		{
			scales: [
				{subscale_unit: "hour", unit: "day", step: 1, format: "%j %M"},
				{unit: "hour", step: 1, format: "%H:%i"}

			]
		},
		// days
		{
			scales: [
				{subscale_unit: "day", unit: "month", step: 1, format: "%F"},
				{unit: "day", step: 1, format: "%j"}
			]
		},
		// weeks
		{
			scales: [
				{subscale_unit: "week", unit: "month", step: 1, date: "%F"},
				{
					unit: "week", step: 1, template: function (date) {
						var dateToStr = gantt.date.date_to_str("%d %M");
						var endDate = gantt.date.add(gantt.date.add(date, 1, "week"), -1, "day");
						return dateToStr(date) + " - " + dateToStr(endDate);
					}
				}
			]
		},
		// months
		{
			scales: [
				{subscale_unit: "month", unit: "year", step: 1, format: "%Y"},
				{unit: "month", step: 1, format: "%M"}
			]
		},
		// quarters
		{
			scales: [
				{subscale_unit: "month", unit: "year", step: 3, format: "%Y"},
				{
					unit: "month", step: 3, template: function (date) {
						var dateToStr = gantt.date.date_to_str("%M");
						var endDate = gantt.date.add(gantt.date.add(date, 3, "month"), -1, "day");
						return dateToStr(date) + " - " + dateToStr(endDate);
					}
				}
			]
		},
		// years
		{
			scales: [
				{subscale_unit: "year", unit: "year", step: 1, date: "%Y"},
				{
					unit: "year", step: 5, template: function (date) {
						var dateToStr = gantt.date.date_to_str("%Y");
						var endDate = gantt.date.add(gantt.date.add(date, 5, "year"), -1, "day");
						return dateToStr(date) + " - " + dateToStr(endDate);
					}
				}
			]
		},
		// decades
		{
			scales: [
				{
					subscale_unit: "year", unit: "year", step: 10, template: function (date) {
						var dateToStr = gantt.date.date_to_str("%Y");
						var endDate = gantt.date.add(gantt.date.add(date, 10, "year"), -1, "day");
						return dateToStr(date) + " - " + dateToStr(endDate);
					}
				},
				{
					unit: "year", step: 100, template: function (date) {
						var dateToStr = gantt.date.date_to_str("%Y");
						var endDate = gantt.date.add(gantt.date.add(date, 100, "year"), -1, "day");
						return dateToStr(date) + " - " + dateToStr(endDate);
					}
				}
			]
		}
	];


    function exportpdf() {
        var date = new Date().toDateString();
        gantt.exportToPDF({
            raw: true,
            header: "<h2>South Bay Resource Schedule</h2>",
            footer: "<div style='padding-top:5px;'><div style='float:left;'>South Bay PM Resource Schedule " + date + "&nbsp&nbsp&nbsp&nbspsummary&nbsp </div><div style='width:60px;background-color:black;float:left;'>&nbsp</div> <div style='float:left;'>&nbsp RFP&nbsp&nbsp&nbsp&nbspProject&nbsp </div><div style='width:60px;background-color:blue;float:left;'>&nbsp</div><div style='float:left;'>&nbsp Pre Const&nbsp&nbsp&nbsp&nbspProject&nbsp</div> <div style='width:60px;background-color:orange;float:left;'>&nbsp</div><div style='float:left;'>&nbsp In Const&nbsp&nbsp&nbsp&nbspOpportunity&nbsp </div><div style='width:60px;background-color:red;float:left;'>&nbsp</div></div></br>"
        });
    }
</script>
</body>

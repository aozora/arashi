/// <summary>
/// admin.analytics
/// </summary>


$(function () {
   // Setup the datepicker
   $.datepicker.setDefaults($.datepicker.regional[""]);
   $("input.datepicker").datepicker($.datepicker.regional[__cultureInfo["name"].substr(0, 2)]);

   $("#refresh-link").click(function () {
      showGoogleAnalytics();
   });

   showGoogleAnalytics();

});




function showGoogleAnalytics() {
   pageLoading(false, __loading_data);

   $.getJSON(urlGA,
      {
         dateFrom: $("#StartDate").val(),
         dateTo: $("#EndDate").val()
      },
      function (data) {

         $('#ga-visitspageviews-chart').css({
            height: '250px',
            width: '900px'
         });

         // visits & page views
         // --------------------------------------------------
         $.plot($('#ga-visitspageviews-chart'),
         [{ label: __ga["visits"], data: data.visits }, { label: __ga["pageviews"], data: data.pageviews}],
         {
            xaxis: {
               mode: "time",
               timeformat: "%d %b",
               monthNames: __cultureInfo["dateTimeFormat"].AbbreviatedMonthNames,
               tickSize: [7, "day"]
            },
            yaxis: {
               min: data.min,
               max: data.max,
               ticks: data.ticks
            },
            lines: {
               show: true,
               fill: 0.2,
               lineWidth: 2
            },
            points: {
               show: true,
               radius: 3,
               fillColor: "#ffffff"
            },
            grid: {
               hoverable: true,
               clickable: true,
               labelMargin: 10,
               borderWidth: 1,
               borderColor: "#cccccc"
            },
            colors: ["#0077cc", "#50B432"]
         });

         var previousPoint = null;
         $('#ga-visitspageviews-chart').bind("plothover", function (event, pos, item) {
            if (item) {
               if (previousPoint != item.dataIndex) {
                  previousPoint = item.dataIndex;
                  var date = new Date(item.datapoint[0]);
                  var contents = '<span>' + dateformat(date, "l, F j, Y") + '</span><p>' + item.series.label + ': <strong>' + item.datapoint[1] + '</strong></p>';
                  showTooltip(item.pageX, item.pageY, contents);
               }
            } else {
               $("#tooltip").remove();
               previousPoint = null;
            }
         });

         // traffic source pie
         // --------------------------------------------------
         $('#ga-piesource-chart').css({
            height: '200px',
            width: '250px'
         });

         var plot_piesource = [];
         for (var i in data.piesource) {
            i = parseInt(i);
            plot_piesource[i] = { label: data.piesource[i][0], data: parseInt(data.piesource[i][1]) };
         }

         $.plot($("#ga-piesource-chart"), plot_piesource,
         {
            series: {
               pie: {
                  show: true,
                  offset: {
                     top: 0,
                     left: 0
                  },
               }
            },
            legend: {
               show: true,
               labelFormatter: function(label, series) {
                  return label + ' ' + Math.round(series.percent) + '%'
               },
               backgroundColor: "#FFF",
               backgroundOpacity: 0.8
            },
            grid: {
               borderWidth: 1,
               borderColor: "#cccccc",
         		backgroundColor: "#cccccc"
         	},
            colors: ["#058DC7", "#50B432", "#ED561B"]
         });

         // browsers pie
         // --------------------------------------------------
         $('#ga-browsers-chart').css({
            height: '200px',
            width: '250px'
         });

         var plot_data_b = [];
         for (var i in data.browsers) {
            i = parseInt(i);
            plot_data_b[i] = {label: data.browsers[i][0], data: parseInt(data.browsers[i][1])};
         }

         $.plot($("#ga-browsers-chart"), plot_data_b,
         {
            series: {
               pie: {
                  show: true,
                  offset: {
                     top: 0,
                     left: -48
                  }
               }
            },
            legend: {
               show: true,
               labelFormatter: function(label, series) {
                  return label + ' ' + Math.round(series.percent) + '%'
               },
               backgroundColor: "#FFF",
               backgroundOpacity: 0.8
            },
            grid: {
               borderWidth: 1,
               borderColor: "#cccccc",
         		backgroundColor: "#cccccc"
         	},
            colors: ["#058DC7", "#50B432", "#ED561B"]
         });

         // new vs returning visitors pie
         // --------------------------------------------------
         $('#ga-pievisitortypes-chart').css({
            height: '200px',
            width: '250px'
         });

         var plot_visitortypes = [];
         for (var i in data.visitortypes) {
            i = parseInt(i);
            plot_visitortypes[i] = {label: data.visitortypes[i][0], data: parseInt(data.visitortypes[i][1])};
         }

         $.plot($("#ga-pievisitortypes-chart"), plot_visitortypes,
         {
            series: {
               pie: {
                  show: true,
                  offset: {
                     top: 0,
                     left: -48
                  }
               }
            },
            legend: {
               show: true,
               labelFormatter: function(label, series) {
                  return label + ' ' + Math.round(series.percent) + '%'
               },
               backgroundColor: "#FFF",
               backgroundOpacity: 0.8
            },
            grid: {
               borderWidth: 1,
               borderColor: "#cccccc",
         		backgroundColor: "#cccccc"
         	},
            colors: ["#058DC7", "#50B432", "#ED561B"]
         });

         // traffic source
         // --------------------------------------------------
         $.each(data.source, function (index, value) {
            var $tbody = $('#source-table tbody');

            var $tr = $("<tr/>");
            $.each(value, function (j, cellData) {
               $tr.append('<td>' + cellData + '</td>');
            })
            $tbody.append($tr);
         });

         // keywords
         // --------------------------------------------------
         $.each(data.keywords, function (index, value) {
            var $tbody = $('#keywords-table tbody');

            var $tr = $("<tr/>");
            $.each(value, function (j, cellData) {
               $tr.append('<td>' + cellData + '</td>');
            })
            $tbody.append($tr);
         });


         $("#google-analytics-container").show();
         pageLoading(true);
      });

}



function showTooltip(x, y, contents) {
   $('<div id="tooltip" class="gatooltip ui-widget ui-widget-header ui-corner-all"><div class="tcont-1"><div class="tcont-2"><div class="tcont-3">' + contents + '</div></div></div></div>').appendTo("body").css({
      top: y - 20,
      left: x - $("#tooltip").width() - 10
   });

   if ((x - $("#tooltip").width()) < $('#ga-visitspageviews-chart').offset().left) 
      $("#tooltip").css({
         left: x + 10
      }).fadeIn(200);
   else 
      $("#tooltip").fadeIn(200);
}



function dateformat(date, format) {
   return format.replace(/(Y|y|M|F|m|j|D|l|d)/g, function ($1) {
      switch ($1) {
         case 'Y':
            return date.getFullYear();
         case 'y':
            var f = date.getFullYear() + "";
            return f.substr(2, 4);
         case 'F':
            return __cultureInfo["dateTimeFormat"].MonthNames[date.getMonth()];
         case 'M':
            return __cultureInfo["dateTimeFormat"].AbbreviatedMonthNames[date.getMonth()];
         case 'm':
            return (date.getMonth() < 9 ? '0' : '') + (date.getMonth() + 1);
         case 'j':
            return date.getDate();
         case 'D':
            return __cultureInfo["dateTimeFormat"].AbbreviatedDayNames[date.getDay()];
         case 'l':
            return __cultureInfo["dateTimeFormat"].DayNames[date.getDay()];
         case 'd':
            return (date.getDate() < 10 ? '0' : '') + date.getDate();
      }
   });
}









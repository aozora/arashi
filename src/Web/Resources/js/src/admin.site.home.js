/// <summary>
/// admin.site.home
/// </summary>


$(function () {
   // setup pseudo-buttons

   $("#showStatistics").click(function () {
      $(this).hide();
      showGoogleAnalytics();
   })




      $("#feature-category-container li .feature-category").hover(
         function () {
            //$(this).addClass("ui-state-hover ui-state-default ui-button");
            $(this).animate({ 
               margin: '10px',
               backgroundColor: "#F4F9FD",
               border: '1px',
               borderColor: "#74b2e2",
               borderRadius: "6px"
            }, { queue: false, duration: 500 });
         },
         function () {
            //$(this).removeClass("ui-state-hover ui-state-default ui-button");
            $(this).animate({
               margin: '0px',
               backgroundColor: "transparent",
               border: '0px',
               borderColor: "transparent" ,
               borderRadius: "0px"
            }, { queue: false, duration: 500 });
         }
      );

//   $("#feature-category-container").lavaLamp({
//      //target: 'div.feature-category',
//      autoResize: true
//   });

   // Quicksand
   $("#feature-category-container li .feature-category").click(function () {
      var $this = $(this);
      var destination = "#destination-" + $this.parent("li").attr("data-id");

      $("#feature-category-container").quicksand($(destination + ' li'), {
         useScaling: true,
         easing: 'easeInOutQuad'
      }, function () {
         $("#category-heading").text($("h3", $this).text());
      });

      //      $(this).parent("li").addClass("active");
      //      $("#feature-category-container li:not(.active)").effect('fade', {}, 500, null);
      //$(this).next(".feature-category-features").show();
   });




   $(".controlpanelgroup li.controlpanelicon a.ui-iconplustext").not(".nohover").hover(
      function () {
         $(this).addClass("ui-state-hover ui-state-default ui-button ui-shadow ui-corner-all");
      },
      function () {
         $(this).removeClass("ui-state-hover ui-state-default ui-button ui-shadow ui-corner-all");
      }
   );

      $("a.expander")
         .hover(
            function () {
               $(this).addClass("ui-state-hover");

               if ($(this).parents(".controlpanelgroup").find("ul:visible").length > 0)
                  $(this).attr("title", resCollapseThisGroup)
               else
                  $(this).attr("title", resExpandThisGroup)
            },
            function () {
               $(this).removeClass("ui-state-hover");
            }
         )
         .click(
            function (event) {
               event.preventDefault();

               var $this = $(this);

               $(this).parents(".controlpanelgroup").find("ul").slideToggle(function () {
                  //console.log($("span.ui-icon", $this).attr("class"));
                  if ($this.parents(".controlpanelgroup").find("ul:hidden").length > 0) {
                     $("span.ui-icon", $this)
                        .removeClass("ui-icon-carat-1-n")
                        .addClass("ui-icon-carat-1-s");
                  } else {
                     $("span.ui-icon", $this)
                        .removeClass("ui-icon-carat-1-s")
                        .addClass("ui-icon-carat-1-n");
                  }
               });
            });



   var $tutorialLink = $('<a id="showTutorial" href="#"></a>').text(__tutorial);
   $("#help-container").append($tutorialLink);

   $("#showTutorial").click(function () {
      return startTutorial();
   });

   if (showWelcome) {
      // Setup Welcome dialog
      $("#closeWelcomeDialog").live("click", function () {
         $.unblockUI();
      });

      $("#startTutorial").live("click", function () {
         return startTutorial();
      });

      $.blockUI({
         title: $("#welcome-dialog").attr("title"),
         message: $("#welcome-dialog").html(),
         theme: true,
         themedCSS: {
            width: '40%',
            top: '30%',
            left: '35%'
         },
         draggable: false,
         timeout: 0
      });
   }


});




function showGoogleAnalytics() {
   pageLoading(false, "Loading data...");

   $.getJSON(urlGA, function (data) {

      $('#ga-visitspageviews-chart').css({
         height: '150px',
         width: '800px'
      });

      // visits & page views
      // --------------------------------------------------
      var plot_data_visits = new Array();
      var plot_data_pv = new Array();

      for (var i in data.visits) {
         i = parseInt(i);
         plot_data_visits.push([data.visits[i].date, data.visits[i].visits]);
         plot_data_pv.push([data.pageviews[i].date, data.pageviews[i].pageviews]);
      }

      $.plot($('#ga-visitspageviews-chart'),
         [{ label: __ga["visits"], data: plot_data_visits }, { label: __ga["pageviews"], data: plot_data_pv}], 
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


      // browsers pie
      // --------------------------------------------------
      $('#ga-browsers-chart').css({
         height: '150px',
         width: '250px'
      });

      var plot_data_b = [];
      for (var i in data.browsers) {
         i = parseInt(i);
         plot_data_b[i] = {label: data.browsers[i].browser, data: parseInt(data.browsers[i].visits)};
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
            show: true
         },
   		grid: {
            borderWidth: 1,
            borderColor: "#cccccc",
			   backgroundColor: "#cccccc"
			},
			colors: ["#0077cc", "#50B432", "#AFD8F8"]
      });



      $("#google-analytics-container").show();
      pageLoading(true);
   });

}



function showTooltip(x, y, contents) {
   $('<div id="tooltip" class="tooltip ui-widget ui-widget-header ui-corner-all"><div class="tcont-1"><div class="tcont-2"><div class="tcont-3">' + contents + '</div></div></div></div>').appendTo("body").css({
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


//   $.get(urlGA, function (data) {
//      $("#google-analytics-container").html(data);

//      // generate the chart
//      $("#ga-visitors-data").visualize({
//         type: 'line',
//         width: $("#google-analytics-container").width() - 200
//      });

//      // hide the source table
//      //$("#ga-visitors-data").addClass("accessHide");

//      pageLoading(true);
//   });








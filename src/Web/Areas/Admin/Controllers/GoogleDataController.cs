
namespace Arashi.Web.Areas.Admin.Controllers
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Globalization;
   using System.Linq;
   using System.Threading;
   using System.Web.Mvc;
   using Arashi.Core.Extensions;
   using Arashi.Services.Localization;
   using Arashi.Services.Membership;
   using Arashi.Services.SiteStructure;
   using Arashi.Web.Areas.Admin.Models;
   using Arashi.Web.Mvc.Controllers;
   using Arashi.Web.Mvc.Filters;
   using Common.Logging;
   using Google.GData.Analytics;

   // For help, use the Google Data Feed Query Explorer: http://code.google.com/apis/analytics/docs/gdata/gdataExplorer.html


   public class GoogleDataController : SecureControllerBase
   {
      private readonly ILog log;

      #region Constructor

      public GoogleDataController(ILog log, ILocalizationService localizationService, IUserService userService, ISiteService siteService)
         : base(log, localizationService, userService, siteService)
      {
         this.log = log;
      }

      #endregion

      #region Index

      /// <summary>
      /// Show the analytics dashboard 
      /// </summary>
      /// <returns></returns>
      [OutputCache(Duration = 60)]
      [AcceptVerbs(HttpVerbs.Get)]
      [PermissionFilter(RequiredRights = Rights.DashboardAccess)]
      public ActionResult Index()
      {
         GoogleAnalyticsModel model = new GoogleAnalyticsModel();

         // TODO: add validation

         // take the last 30 days
         model.EndDate = DateTime.Now;
         model.StartDate = model.EndDate.Subtract(new TimeSpan(30, 0, 0, 0));
         
         return View("Index", model);
      }



      //[AcceptVerbs(HttpVerbs.Post)]
      //[PermissionFilter(RequiredRights = Rights.DashboardAccess)]
      //public ActionResult Index(GoogleAnalyticsModel model)
      //{
      //   //model.StartDate = DateTime.Now;
      //   //model.EndDate = model.StartDate.Subtract(new TimeSpan(30, 0, 0, 0));
         
      //   return View("Index", model);
      //}

      #endregion

      /// <summary>
      /// Get the json for all stats
      /// </summary>
      /// <returns></returns>
      [OutputCache(Duration = 60)]
      [PermissionFilter(RequiredRights = Rights.DashboardAccess)]
      public JsonResult FullStatistics(string dateFrom, string dateTo)
      {
         DateTime startDate = DateTime.ParseExact(dateFrom, "d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);
         DateTime endDate = DateTime.ParseExact(dateTo, "d", Thread.CurrentThread.CurrentCulture.DateTimeFormat);

         string userName = Context.CurrentSite.GoogleDataUserName;
         string passWord = Context.CurrentSite.GoogleDataPassword;

         AnalyticsService service = new AnalyticsService("AnalyticsSampleApp");
         if (!string.IsNullOrEmpty(userName))
            service.setUserCredentials(userName, passWord);

         // Visits & Page Views
         DataQuery visitsAndPageViewsQuery = GetGoogleDataQuery(startDate, endDate, "ga:date", "ga:visits,ga:pageviews", "ga:date");
         DataFeed data1 = service.Query(visitsAndPageViewsQuery);
         IList<string[]> arrayVisits = new List<string[]>();
         IList<string[]> arrayPageViews = new List<string[]>();

         foreach (DataEntry entry in data1.Entries)
         {
            string date = DateTime.ParseExact(entry.Dimensions[0].Value, "yyyyMMdd", null).ToJavascriptTimestamp().ToString();
            arrayVisits.Add(new string[] { date, entry.Metrics[0].Value });
            arrayPageViews.Add(new string[] { date, entry.Metrics[1].Value });
         }

         // Browsers
         DataQuery browsersQuery = GetGoogleDataQuery(startDate, endDate, "ga:browser", "ga:visits", string.Empty);
         DataFeed data5 = service.Query(browsersQuery);
         IList<string[]> arrayBrowsersSource = new List<string[]>();

         foreach (DataEntry entry in data5.Entries)
         {
            arrayBrowsersSource.Add(new string[] { entry.Dimensions[0].Value, entry.Metrics[0].Value });
         }

         // New visitors / Returning
         DataQuery pieVisitorTypesQuery = GetGoogleDataQuery(startDate, endDate, "ga:visitorType", "ga:visits", string.Empty);
         DataFeed data6 = service.Query(pieVisitorTypesQuery);
         IList<string[]> arrayVisitorTypes = new List<string[]>();

         foreach (DataEntry entry in data6.Entries)
         {
            arrayVisitorTypes.Add(new string[] { entry.Dimensions[0].Value, entry.Metrics[0].Value });
         }


         // All Traffic Source (PIE)
         DataQuery pieTrafficSourceQuery = GetGoogleDataQuery(startDate, endDate, "ga:medium", "ga:visits", string.Empty);
         DataFeed data4 = service.Query(pieTrafficSourceQuery);
         IList<string[]> arrayPieTrafficSource = new List<string[]>();

         foreach (DataEntry entry in data4.Entries)
         {
            string source = entry.Dimensions[0].Value;

            if (source == "(none)")
               source = "Direct";
            else if (source == "organic")
               source = "Search";

            arrayPieTrafficSource.Add(new string[] { source, entry.Metrics[0].Value });
         }


         // Traffic Soruce
         DataQuery trafficSourceQuery = GetGoogleDataQuery(startDate, endDate, "ga:source,ga:referralPath", "ga:visits,ga:visitBounceRate,ga:avgTimeOnSite,ga:percentNewVisits", "-ga:visits");
         DataFeed data2 = service.Query(trafficSourceQuery);
         IList<string[]> arrayTrafficSource = new List<string[]>();
         NumberFormatInfo numberFormat = new System.Globalization.CultureInfo("en-US").NumberFormat;

         foreach (DataEntry entry in data2.Entries)
         {
            // ga:avgTimeOnSite is in secs
            // ga:percentNewVisits is in %
            arrayTrafficSource.Add(new string[] { entry.Dimensions[0].Value, 
                                                   entry.Dimensions[1].Value, 
                                                   entry.Metrics[0].Value, 
                                                   entry.Metrics[1].Value, 
                                                   TimeSpan.FromSeconds(Convert.ToDouble(entry.Metrics[2].Value, numberFormat)).ToString("g"), 
                                                   string.Format("{0}%", Math.Round(Convert.ToDouble(entry.Metrics[3].Value, numberFormat), 2) ) });
         }


         // Keywords
         DataQuery keywordsQuery = GetGoogleDataQuery(startDate, endDate, "ga:keyword", "ga:visits", string.Empty);
         DataFeed data3 = service.Query(keywordsQuery);
         IList<string[]> arrayKeywords = new List<string[]>();

         foreach (DataEntry entry in data3.Entries)
         {
            arrayKeywords.Add(new string[] { entry.Dimensions[0].Value, entry.Metrics[0].Value });
         }



         return Json(new
         {
            visits = arrayVisits,
            pageviews = arrayPageViews,
            piesource = arrayPieTrafficSource,
            source = arrayTrafficSource,
            keywords = arrayKeywords,
            browsers = arrayBrowsersSource,
            visitortypes = arrayVisitorTypes
         },
         JsonRequestBehavior.AllowGet);
      }



      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      [OutputCache(Duration = 60)]
      [PermissionFilter(RequiredRights = Rights.DashboardAccess)]
      public JsonResult QuickStatistics()
      {
         // user login
         //const string dataFeedUrl = "https://www.google.com/analytics/feeds/data";
         string userName = Context.CurrentSite.GoogleDataUserName;
         string passWord = Context.CurrentSite.GoogleDataPassword;
         string profileId = Context.CurrentSite.GoogleDataProfileID;

         AnalyticsService service = new AnalyticsService("AnalyticsSampleApp");
         if (!string.IsNullOrEmpty(userName))
         {
            service.setUserCredentials(userName, passWord);
         }

         //string gaStartDate = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
         //string gaEndDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd");

         #region Visits & Page Views

         DataQuery visitorsQuery = GetGoogleDataQuery(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-2), "ga:date", "ga:visits,ga:pageviews", "ga:date");
         DataFeed data = service.Query(visitorsQuery);

         int count = data.Entries.Count;
         IList<string[]> arrayVisits = new List<string[]>();
         IList<string[]> arrayPageViews = new List<string[]>();

         foreach (DataEntry entry in data.Entries)
         {
            string date = DateTime.ParseExact(entry.Dimensions[0].Value, "yyyyMMdd", null).ToJavascriptTimestamp().ToString();
            //log.DebugFormat("DateTime: {0}, GoogleData: {1}, Js: {2}", DateTime.ParseExact(entry.Dimensions[0].Value, "yyyyMMdd", null), entry.Dimensions[0].Value, date);
            
            arrayVisits.Add(new string[] {date, entry.Metrics[0].Value});
            arrayPageViews.Add(new string[] {date, entry.Metrics[1].Value});
         }

         var visitsList = from x in arrayVisits
                      select new
                      {
                         date = x[0],
                         visits = x[1]
                      };

         var pageviewsList = from x in arrayPageViews
                         select new
                         {
                            date = x[0],
                            pageviews = x[1]
                         };

         #endregion

         #region Browsers

         DataQuery browsersQuery = GetGoogleDataQuery(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(-2), "ga:browser", "ga:visits", string.Empty);
         DataFeed data2 = service.Query(browsersQuery);
         IList<string[]> arrayBrowsers = new List<string[]>();

         foreach (DataEntry entry in data2.Entries)
         {
            // browser, visits
            arrayBrowsers.Add(new string[] { entry.Dimensions[0].Value, entry.Metrics[0].Value });
            //log.DebugFormat("Browser: {0}, Visits: {1}", entry.Dimensions[0].Value, entry.Metrics[0].Value);
         }

         var browsersList = from x in arrayBrowsers
                      select new
                      {
                         browser = x[0],
                         visits = x[1]
                      };

         #endregion


         return Json(new
         {
            visits = visitsList,
            pageviews = pageviewsList,
            browsers = browsersList
         },
         JsonRequestBehavior.AllowGet);



         //// #2 visits by type
         //// x = date, y = visits, type
         ////            New| 1 2  1  0  4
         ////      Returning| 2 3  4  1  9
         ////               |-----------------------------
         ////               | t t1 t2 t3 t4
         //DataQuery visitsByTypeQuery = new DataQuery(dataFeedUrl)
         //{
         //   Ids = profileId,
         //   GAStartDate = gaStartDate,
         //   GAEndDate = gaEndDate,
         //   Dimensions = "ga:date,ga:visitorType",
         //   Metrics = "ga:visits",
         //   Sort = "ga:date"
         //};

         //DataFeed visitsByTypeData = service.Query(visitsByTypeQuery);


         //GoogleDataQuickModel model = new GoogleDataQuickModel();
         ////model.VisitorsData = visitorsData;
         ////model.VisitsByTypeData = GetVisitorChartData(visitsByTypeData);

         //model.VisitorsArray = visitors.ToString();

         //return model;
      }



      

      #region Helpers

      /// <summary>
      /// Create a Google Data Query API
      /// </summary>
      /// <param name="startDate"></param>
      /// <param name="endDate"></param>
      /// <param name="dimensions"></param>
      /// <param name="metrics"></param>
      /// <param name="sort"></param>
      /// <returns></returns>
      private DataQuery GetGoogleDataQuery(DateTime startDate, DateTime endDate, string dimensions, string metrics, string sort)
      {
         const string dataFeedUrl = "https://www.google.com/analytics/feeds/data";
         //string userName = Context.CurrentSite.GoogleDataUserName;
         //string passWord = Context.CurrentSite.GoogleDataPassword;
         string profileId = Context.CurrentSite.GoogleDataProfileID;

         //AnalyticsService service = new AnalyticsService("AnalyticsSampleApp");
         //if (!string.IsNullOrEmpty(userName))
         //   service.setUserCredentials(userName, passWord);

         DataQuery dataQuery = new DataQuery(dataFeedUrl)
         {
            Ids = profileId,
            GAStartDate = startDate.ToString("yyyy-MM-dd"),
            GAEndDate = endDate.ToString("yyyy-MM-dd"),
            Dimensions = dimensions,
            Metrics = metrics,
            Sort = sort
         };

         return dataQuery;
      }


      ///// <summary>
      ///// GetVisitorChartData
      ///// the exported data show more record for the same date, 1 for vistype1 & antoher for vistype2
      ///// </summary>
      ///// <param name="result"></param>
      //private Dictionary<string, VisitorTypeVisits> GetVisitorChartData(DataFeed result)
      //{
      //   AtomEntryCollection entries = result.Entries;
      //   IList<string> returningVisitors = new List<string>();
      //   IList<string> newVisitors = new List<string>();
      //   IList<string> dates = new List<string>();

      //   //          DIMENSIONS		         |  METRICS
      //   //   ga:date   |	ga:visitorType	   |  ga:visits
      //   //   -------------------------------------------
      //   //   20101118	| New Visitor	      |     1
      //   //   20101118	| Returning Visitor	|     2
      //   //   20101119	| New Visitor	      |     2
      //   //   20101120	| New Visitor	      |     1

      //   Dictionary<string, VisitorTypeVisits> rows = new Dictionary<string, VisitorTypeVisits>();

      //   foreach (DataEntry entry in entries)
      //   {
      //      string date = entry.Dimensions.Where(d => d.Name == "ga:date").First().Value;
      //      string visType = entry.Dimensions.Where(d => d.Name == "ga:visitorType").First().Value;
      //      int visits = entry.Metrics.Where(m => m.Name == "ga:visits").First().IntegerValue;

      //      if (!rows.ContainsKey(date))
      //         rows.Add(date, new VisitorTypeVisits(){NewVisitors = 0, ReturningVisitors = 0});

      //      if (visType == "New Visitor")
      //      {
      //         rows[date].NewVisitors = visits;
      //      }
      //      else
      //      {
      //         rows[date].ReturningVisitors = visits;
      //      }

      //   }

      //   return rows;
      //}

      #endregion

   }
}
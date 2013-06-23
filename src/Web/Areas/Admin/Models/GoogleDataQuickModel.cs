namespace Arashi.Web.Areas.Admin.Models
{
   using System;
   using System.Collections.Generic;

   using Google.GData.Analytics;


   public class GoogleDataQuickModel
   {

      public string VisitorsArray {get; set;}

      public DataFeed VisitorsData { get; set; }
      public Dictionary<string, VisitorTypeVisits> VisitsByTypeData { get; set; }
   }



   public class VisitorTypeVisits
   {
      public int NewVisitors { get; set; }
      public int ReturningVisitors { get; set; }
   }

}
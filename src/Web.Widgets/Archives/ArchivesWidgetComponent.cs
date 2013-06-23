namespace Arashi.Web.Widgets.Archives
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;
   using Arashi.Core.Domain;
   using Arashi.Core.Extensions;
   using Arashi.Services.Content;
   using Arashi.Core.Domain.Dto;
   using Arashi.Core.Util;
   using System.Threading;
   using Common.Logging;



   /// <summary>
   /// Display links for archived posts
   /// </summary>
   public class ArchivesWidgetComponent : WidgetComponentBase
   {
      #region Private Fields

      private ILog log;
      private readonly IContentItemService<Post> contentItemService;
      private IList<ContentItemCalendarDTO> calendar;
      private enum ArchiveList
      {
         yearly,
         monthly,
         daily,
         weekly,
         postbypost
      }

      // Settings
      private ArchiveList type;
      private string format;
      private bool showPostCount;
      private string before;
      private string after;

      #endregion

      #region Constructor

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="contentItemService"></param>
      public ArchivesWidgetComponent(ILog log, IContentItemService<Post> contentItemService)
      {
         this.log = log;
         this.contentItemService = contentItemService;
      }

      #endregion

      public override void Init()
      {
         log.Debug("ArchivesWidgetComponent.Init: start");

         // Get the Calendar data
         calendar = contentItemService.GetPostCalendarForPublishedBySite(context.CurrentSite);

         log.Debug("ArchivesWidgetComponent.Init: end");

         base.Init();
      }



      public override string Render()
      {
         // TODO: how to check if a settings exists?

         type = ArchiveList.monthly; // default
         format = "html";
         showPostCount = true;

         if (widget.Settings.Count > 0)
         {
            type = (ArchiveList) Enum.Parse(typeof (ArchiveList), widget.Settings["type"]);
            format = widget.Settings["format"];
            showPostCount = Convert.ToBoolean(widget.Settings["show_post_count"].ToLower());
            //int? limit;
            before = widget.Settings["before"];
            after = widget.Settings["after"];
         }

         // TODO: support others list types
         switch (type)
         {
            case ArchiveList.yearly:
               break;
            case ArchiveList.daily:
               break;
            case ArchiveList.weekly:
               break;
            case ArchiveList.postbypost:
               break;
            case ArchiveList.monthly:
            default:
               return GetMonthly();
         }

         return string.Empty;
      }



      private string GetMonthly()
      {
         StringBuilder html = new StringBuilder();
         
         var monthlyCalendar = (from dto in calendar
                                group dto by new
                                {
                                   dto.Year,
                                   dto.Month
                                } into g
                                select new
                                {
                                   Year = g.Key.Year,
                                   Month = g.Key.Month,
                                   Count = g.Sum(dto => dto.Count)
                                });

         foreach (var item in monthlyCalendar)
         {
            string text = DateUtil.MonthNames(Thread.CurrentThread.CurrentUICulture, false)[item.Month].Capitalize() + "&nbsp;" + item.Year.ToString();
            string href = string.Concat(GetCurrentSiteUrlRoot(),
                                        "/",
                                        item.Year.ToString(),
                                        "/",
                                        item.Month.ToString().PadLeft(2, '0'),
                                        "/");
            string postCount = showPostCount ? string.Format("&nbsp;({0})", item.Count.ToString()) : string.Empty;
            string template;

            // {0} text
            // {1} href
            // {2} before
            // {3} after
            // {4} count
            switch (format)
            {
               case "option":
                  template = "<option value=\"{1}\"><a title=\"{0}\" href=\"{1}\">{2}{0}{3}{4}</a><option>";
                  break;
               case "link":
                  template = "<a title=\"{0}\" href=\"{1}\">{2}{0}{3}{4}</a><br />";
                  break;
               case "custom":
                  template = "{1}{0}{3}{4}";
                  break;
               case "html":
               default:
                  template = "<li><a title=\"{0}\" href=\"{1}\">{2}{0}{3}</a>{4}</li>";
                  break;
            }

            html.AppendFormat(template, text, href, before, after, postCount);
         }

         if (format == "html")
         {
            html.Insert(0, "<ul>");
            html.Append("</ul>");
         }

         return html.ToString();
      }


   }
}

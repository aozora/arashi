using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Services.Localization;
using Arashi.Core.Extensions;
using Arashi.Services.Content;
using Arashi.Core.Domain.Dto;
using Arashi.Core.Util;
using System.Threading;
using log4net;

namespace Arashi.Web.Widgets.Tags
{
   public class TagCloudWidgetComponent : WidgetComponentBase
   {
      #region Private Fields

      private static readonly ILog log = LogManager.GetLogger(typeof(TagCloudWidgetComponent));
      private readonly ILocalizationService localizationService;
      private readonly ITagService tagService;
      private IList<TagDTO> tagCloud;
      private IList<Tag> siteTags;
      #endregion

      #region Constructor

      public TagCloudWidgetComponent(ILocalizationService localizationService, ITagService tagService)
      {
         this.localizationService = localizationService;
         this.tagService = tagService;
      }

      #endregion

      private string Resource(string token)
      {
         return localizationService.ThemeResource(token, Thread.CurrentThread.CurrentUICulture);
      }


      public override void Init()
      {
         log.Debug("TagCloudWidgetComponent.Init: start");

         // Get the list of categories data
         tagCloud = tagService.GetTagCloudBySite(context.CurrentSite);
         siteTags = tagService.GetAllTagsBySite(context.CurrentSite);

         log.Debug("TagCloudWidgetComponent.Init: end");

         base.Init();
      }


      // Pay attention: is slightly different from wp_list_categories !!!!!!!!!!!!!!
      public override string Render()
      {
         int smallest = 8;
         int largest = 22;
         string unit = "pt";
         int number = 45;
         string format = "flat";
         string orderby = "name";
         string order = "ASC";
         string taxonomy = "post_tag";

         smallest = Convert.ToInt32(widget.Settings["smallest"]);
         largest = Convert.ToInt32(widget.Settings["largest"]);
         unit = widget.Settings["unit"];
         number = Convert.ToInt32(widget.Settings["number"]);
         format = widget.Settings["format"];
         orderby = widget.Settings["orderby"];
         order = widget.Settings["order"];
         taxonomy = widget.Settings["taxonomy"];
         //'exclude'  => , 
         //'include'  => , 
         //string link = "view";
         //'echo'     => true 

         #region  Check valid settings (if a setting is invalid, log it and set the default)

         const string allowedUnit = "pt,px,em,%";
         if (allowedUnit.IndexOf(unit) == -1)
         {
            unit = this.widget.Type.DefaultSettings["unit"];
            log.WarnFormat("TagCloudWidgetComponent.Render: widgetid = {0}, invalid value for setting[\"unit\"] = {1}", this.widget.WidgetId.ToString(), unit);
         }

         const string allowedFormat = "flat,list,array";
         if (allowedFormat.IndexOf(format) == -1)
         {
            format = this.widget.Type.DefaultSettings["format"];
            log.WarnFormat("TagCloudWidgetComponent.Render: widgetid = {0}, invalid value for setting[\"format\"] = {1}", this.widget.WidgetId.ToString(), format);
         }

         const string allowedOrderBy = "name,count";
         if (allowedOrderBy.IndexOf(orderby) == -1)
         {
            orderby = this.widget.Type.DefaultSettings["orderby"];
            log.WarnFormat("TagCloudWidgetComponent.Render: widgetid = {0}, invalid value for setting[\"orderby\"] = {1}", this.widget.WidgetId.ToString(), orderby);
         }

         const string allowedOrder = "ASC,DESC,RAND";
         if (allowedOrder.IndexOf(order) == -1)
         {
            order = this.widget.Type.DefaultSettings["order"];
            log.WarnFormat("TagCloudWidgetComponent.Render: widgetid = {0}, invalid value for setting[\"order\"] = {1}", this.widget.WidgetId.ToString(), order);
         }

         const string allowedTaxonomy = "post_tag,category,link_category";
         if (allowedTaxonomy.IndexOf(allowedTaxonomy) == -1)
         {
            taxonomy = this.widget.Type.DefaultSettings["taxonomy"];
            log.WarnFormat("TagCloudWidgetComponent.Render: widgetid = {0}, invalid value for setting[\"taxonomy\"] = {1}", this.widget.WidgetId.ToString(), taxonomy);
         }

         #endregion


         IEnumerable<TagDTO> tagsToRender = tagCloud;
         IEnumerable<TagDTO> tags;

         // eventually restrict the tags to the given number at max
         if (number > 0)
         {
            tagsToRender = (from t in tagCloud
                            where t.Count > 0
                            orderby t.Count descending
                            select t).Take(number);
         }


         // sorry, but the DynamicLinq library doesn't work...
         if (orderby == "count" && order == "ASC")
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Count ascending
                   select t;
         else if (orderby == "count" && order == "DESC")
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Count descending
                   select t;
         else if (orderby == "name" && order == "DESC")
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Name descending
                   select t;
         else // (orderby == "name" && order == "ASC")
            tags = from t in tagsToRender
                   where t.Count > 0
                   orderby t.Name ascending
                   select t;

         // if there are no tags (i.e. fresh setup) then exit
         if (tags == null || tags.Count() == 0)
            return string.Empty;

         // determine the font size
         long min_count = tags.Min(t => t.Count);
         long spread = tags.Max(t => t.Count) - min_count;
         int font_spread = largest - smallest;
         if (font_spread < 0)
            font_spread = 1;
         double font_step = (double)font_spread / spread;


         StringBuilder html = new StringBuilder();

         foreach (TagDTO tag in tags)
         {
            string tag_link = GetAbsoluteUrl(siteTags.Single(t => t.TagId == tag.TagId).GetTagUrl());
            string link = string.Format("<a href='{0}' class='tag-link-{1}' title='{2} {6}' rel='tag' style='font-size: {3}{4}'>{5}</a>&nbsp;",
                                        tag_link,
                                        tag.TagId.ToString(),
                                        tag.Count.ToString(),
                                        (smallest + ((tag.Count - min_count) * font_step)).ToString(),
                                        unit,
                                        tag.Name,
                                        Resource("TagCloud_TitleTopics"));

            if (format == "list")
               html.AppendFormat("<li>{0}</li>", link);
            else
               html.Append(link);
         }

         if (format == "list")
         {
            html.Insert(0, "<ul class='wp-tag-cloud'>");
            html.Append("</ul>");
         }
         else
         {
            html.Insert(0, "<div>");
            html.Append("</div>");
         }

         return html.ToString();
      }


   }
}

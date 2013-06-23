using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Arashi.Core.Extensions;

namespace Arashi.Web.Mvc
{
   /// <summary>
   /// Render a syndication feed
   /// See http://www.58bits.com/blog/2009/07/26/ASPNET-MVC-304-Not-Modified-Filter-For-Syndication-Content.aspx
   /// </summary>
   public class SyndicationActionResult : ActionResult
   {
      public SyndicationFeed Feed {get; set;}
      public SyndicationFeedFormatter Formatter {get; set;}


      public override void ExecuteResult(ControllerContext context)
      {
            var response = context.HttpContext.Response;
            SyndicationFeed data = Feed;
            
            if (data != null)
            {
               response.ContentType = "application/rss+xml";
               response.AppendHeader("Cache-Control", "private");

               if (data.Items.Count() > 0)
               {
                  string lastUpdatedTime = data.Items.Max(item => item.LastUpdatedTime).ToString("r");
                  response.AppendHeader("Last-Modified", lastUpdatedTime);
                  response.AppendHeader("ETag", String.Format("\"{0}\"", lastUpdatedTime.EncryptToMD5()));
               }
               //response.Cache.SetLastModified(data.LastModifiedDate);
               
               //response.Output.WriteLine(data.Content);
               using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
               {
                  Formatter.WriteTo(writer);
               }

               response.StatusCode = 200;
               response.StatusDescription = "OK";
            }
      }
   }
}

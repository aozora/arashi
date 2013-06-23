using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Arashi.Core;
using Arashi.Core.Domain;
using Arashi.Services.Content;
using Arashi.Services.Membership;
using Arashi.Services.Search;
using Arashi.Core.Exceptions;
using Arashi.Core.Extensions;
using Arashi.Web.Mvc.Filters;
using Arashi.Web.Mvc.Models;
using log4net;
using uNhAddIns.Pagination;
using Arashi.Core.NHibernate;

namespace Arashi.Web.Areas.Admin.Controllers
{
   [PermissionFilter(RequiredRights = Rights.AdminAccess)]
   public class TestController : Arashi.Web.Mvc.Controllers.ControllerBase
   {
      private static readonly ILog log = LogManager.GetLogger(typeof(TestController));
      private static object lockObject = "";

      public ActionResult Index()
      {
         //RegisterFileManager();

         ViewData["RouteData"] = GetRouteData();

         return View();
      }



      public ActionResult UIElements()
      {
         return View();
      }




      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult TestSHA2Encryption(string plainText)
      {
         if (!string.IsNullOrEmpty(plainText))
         {
            ViewData["encryptedText"] = plainText.EncryptToSHA2();
         }

         return View("Index");
      }



      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult TestException(string statuscode)
      {

         switch (statuscode)
         {
            case "NOHTTP":
               throw new Exception("Non Http Exception!");
            case "404":
               throw new HttpException(500, "ERROR!", new PageNullException("Page not found!"));
            case "503":
               throw new HttpException(500, "ERROR!", new SiteNullException("Site not found or not exists!"));
            case "403":
               throw new HttpException(500, "ERROR!", new SecurityException("Access Forbidden"));
            case "500":
               throw new HttpException(500, "Generig server error!");
         }

         //Exception ex = null;

         //switch (statuscode)
         //{
         //   case "NOHTTP":
         //      ex = new Exception("Non Http Exception!");
         //      break;
         //   case "404":
         //      ex = new HttpException(500, "ERROR!", new PageNullException("Page not found!"));
         //      break;
         //   case "503":
         //      ex = new HttpException(500, "ERROR!", new SiteNullException("Site not found or not exists!"));
         //      break;
         //   case "403":
         //      ex = new HttpException(500, "ERROR!", new AccessForbiddenException("Access Forbidden"));
         //      break;
         //   case "500":
         //      ex = new HttpException(500, "Generig server error!");
         //      break;
         //}

         //HandleErrorInfo model = new HandleErrorInfo(ex, "Test", "TestException");

         //ViewData["ErrorTitle"] = "ERROR!!";
         //ViewData["ErrorMessage"] = ex.Message;
         //ViewData["DebugInfo"] = "";
         //ViewData["ErrorCode"] = statuscode;
         //return View("Error", model);

         return View("Error");
      }



      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult TestRouting(string actionName, string controllerName, string areaName, string idValue)
      {
         string generatedUrl = Url.Action(actionName, controllerName, new {id = idValue, area = areaName});

         ViewData["GeneratedUrl"] = generatedUrl;

         ViewData["RouteData"] = GetRouteData();
         return View("Index");
      }


      #region Test Pings

      public ActionResult TestTrackback()
      {
         IContentItemService<Post> contentItemService = IoC.Resolve<IContentItemService<Post>>();

         IList<Post> posts = contentItemService.FindAllBySite(Context.ManagedSite);

         Post post = (from p in posts
                      where p.PublishedDate <= DateTime.Now.ToUniversalTime() &&
                            p.WorkflowStatus == WorkflowStatus.Published &&
                            p.AllowPings == true
                      select p).FirstOrDefault();

         StringBuilder sb = new StringBuilder();


         if (post != null)
         {
            string postId = post.Id.ToString();
            string title = post.Title;
            string excerpt = "test";
            string blogName = Context.ManagedSite.Name;
            string partialUrl = post.GetContentUrl();
            string url = string.Concat(Request.Url.GetLeftPart(UriPartial.Authority),
                                       "/",
                                       partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
                                          ? partialUrl.Substring(1)
                                          : partialUrl);

            string targetUrl = Request.Url.GetLeftPart(UriPartial.Authority) +
                              string.Format("/trackback.axd?id={0}&url={1}", postId, url.UrlEncode());

            // prepare POST parameters
            string postString = String.Format("title={0}&excerpt={1}&blog_name={2}", title, excerpt, blogName);
            byte[] formPostData = Encoding.UTF8.GetBytes(postString);

            log.Debug("TestTrackback: targetUrl = " + targetUrl);
            log.Debug("TestTrackback: postString = " + postString);

            // Send request with post parameters
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = formPostData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
               requestStream.Write(formPostData, 0, formPostData.Length);
               requestStream.Close();
            }

            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
               sb.Append("Headers.AllKeys: ");
               foreach (string key in response.Headers.AllKeys)
                  sb.AppendFormat("{0} = {1}, ", key, response.Headers[key]);

               sb.AppendLine("");
               sb.AppendLine("ContentType: " + response.ContentType);

               using (StreamReader reader = new StreamReader(response.GetResponseStream()))
               {
                  sb.AppendLine(reader.ReadToEnd());
               }
            }

         }

         MessageModel model = new MessageModel
                                 {
                                    Text = "Trackback sended!",
                                    Icon = MessageModel.MessageIcon.Info,
                                    CssClass = "margin-topbottom",
                                    IsClosable = true
                                 };
         RegisterMessage(model, true);

         ViewData["TestTrackback"] = sb.ToString();

         return View("Index");
      }



      //public ActionResult TestPingback()
      //{
      //   IContentItemService<Post> contentItemService = IoC.Resolve<IContentItemService<Post>>();

      //   IList<Post> posts = contentItemService.FindAllBySite(Context.ManagedSite);

      //   Post post = (from p in posts
      //                where p.PublishedDate <= DateTime.Now.ToUniversalTime() &&
      //                      p.WorkflowStatus == WorkflowStatus.Published &&
      //                      p.AllowPings == true
      //                select p).FirstOrDefault();

      //   StringBuilder sb = new StringBuilder();


      //   if (post != null)
      //   {
      //      string postId = post.Id.ToString();
      //      string title = post.Title;
      //      string excerpt = "test";
      //      string blogName = Context.ManagedSite.Name;
      //      string partialUrl = post.GetContentUrl();
      //      string url = string.Concat(Request.Url.GetLeftPart(UriPartial.Authority),
      //                                 "/",
      //                                 partialUrl.StartsWith("~") || partialUrl.StartsWith("/")
      //                                    ? partialUrl.Substring(1)
      //                                    : partialUrl);

      //      string targetUrl = Request.Url.GetLeftPart(UriPartial.Authority) +
      //                        string.Format("/pingback.axd?id={0}&url={1}", postId, url.UrlEncode());

      //      // prepare POST parameters
      //      string postString = String.Format("title={0}&excerpt={1}&blog_name={2}", title, excerpt, blogName);
      //      byte[] formPostData = Encoding.UTF8.GetBytes(postString);

      //      log.Debug("TestTrackback: targetUrl = " + targetUrl);
      //      log.Debug("TestTrackback: postString = " + postString);

      //      // Send request with post parameters
      //      HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUrl);
      //      request.Credentials = CredentialCache.DefaultNetworkCredentials;
      //      request.Method = "POST";
      //      request.ContentType = "application/x-www-form-urlencoded";
      //      request.ContentLength = formPostData.Length;

      //      using (Stream requestStream = request.GetRequestStream())
      //      {
      //         requestStream.Write(formPostData, 0, formPostData.Length);
      //         requestStream.Close();
      //      }

      //      // Get the response
      //      using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
      //      {
      //         sb.Append("Headers.AllKeys: ");
      //         foreach (string key in response.Headers.AllKeys)
      //            sb.AppendFormat("{0} = {1}, ", key, response.Headers[key]);

      //         sb.AppendLine("");
      //         sb.AppendLine("ContentType: " + response.ContentType);

      //         using (StreamReader reader = new StreamReader(response.GetResponseStream()))
      //         {
      //            sb.AppendLine(reader.ReadToEnd());
      //         }
      //      }

      //   }

      //   MessageModel model = new MessageModel
      //   {
      //      Text = "Trackback sended!",
      //      Icon = MessageModel.MessageIcon.Info,
      //      CssClass = "margin-topbottom",
      //      IsClosable = true
      //   };
      //   RegisterMessage(model, true);

      //   ViewData["TestTrackback"] = sb.ToString();

      //   return View("Index");
      //}

      #endregion


      #region Rebuild Search Index

      public ActionResult RebuildIndex ()
      {
         BuildIndexBySites(); 

         return View("Index");
      }



      private void BuildIndexBySites()
      {
         ISearchService searchService = IoC.Resolve<ISearchService>();
         IContentItemService<IContentItem> contentItemService = IoC.Resolve<IContentItemService<IContentItem>>();

         IList<Site> sites = siteService.GetAllSites();
         bool isOk = false;

         foreach (Site site in sites)
         {
            IEnumerable<IContentItem> contentItemsToIndex = from c in contentItemService.FindAllBySite(site)
                                                            where c.PublishedDate.HasValue == true
                                                               && c.PublishedDate.Value <= DateTime.Now
                                                            select c;

            //only one thread at a time
            System.Threading.Monitor.Enter(lockObject);

            try
            {
               searchService.RebuildIndex(site, contentItemsToIndex);
               isOk = true;
            }
            catch (Exception ex)
            {
               log.Error(ex.ToString());

               // Show the confirmation message
               MessageModel model = new MessageModel
               {
                  Text = ex.Message,
                  Icon = MessageModel.MessageIcon.Alert,
                  CssClass = "margin-topbottom"
               };
               RegisterMessage(model, true);
            }
            finally
            {
               System.Threading.Monitor.Exit(lockObject);
            }



            if (isOk)
            {
               MessageModel model = new MessageModel
                                       {
                                          Text = string.Format("Site {0}: Rebuild completed!", site.Name),
                                          Icon = MessageModel.MessageIcon.Info,
                                          CssClass = "margin-topbottom",
                                          IsClosable = true
                                       };
               RegisterMessage(model, true);
            }
         }
      }



      //private void BuildIndexByNode(Node node)
      //{
      //   foreach (Section section in node.Sections)
      //   {
      //      //handle ContentItems
      //      IList<ContentItem> contentItems = contentItemService.FindContentItemsBySection(section);
      //      try
      //      {
      //         foreach (ContentItem contentItem in contentItems)
      //         {
      //            if (contentItem is ISearchableContent)
      //            {
      //               this._searchService.AddContent(contentItem);
      //            }
      //         }
      //      }
      //      catch (Exception ex)
      //      {
      //         log.Error(String.Format("Indexing ContentItems of Section {0} - {1} failed.", section.Id, section.Title), ex);
      //      }


      //      ////handle SearchContents
      //      //ModuleBase module = null;
      //      //try
      //      //{
      //      //   module = base.ModuleLoader.GetModuleFromSection(section);
      //      //}
      //      //catch (Exception ex)
      //      //{
      //      //   log.Error(String.Format("Unable to create Module for Section {0} - {1}.", section.Id, section.Title), ex);
      //      //}

      //      //if (module is ISearchable)
      //      //{
      //      //   ISearchable searchableModule = (ISearchable)module;
      //      //   try
      //      //   {
      //      //      List<SearchContent> searchContents = new List<SearchContent>(searchableModule.GetAllSearchableContent());
      //      //      this._searchService.AddContent(searchContents);
      //      //   }
      //      //   catch (Exception ex)
      //      //   {
      //      //      log.Error(String.Format("Indexing SearchContents of Section {0} - {1} failed.", section.Id, section.Title), ex);
      //      //   }
      //      //}
      //   }

      //   //foreach (Node childNode in node.ChildNodes)
      //   //{
      //   //   BuildIndexByNode(childNode);
      //   //}
      //}


      #endregion


      /// <summary>
      /// Display a list of the current Route Table
      /// </summary>
      /// <returns></returns>
      private string GetRouteData()
      {
         string path = Request.AppRelativeCurrentExecutionFilePath;
         StringBuilder html = new StringBuilder("<h3>Request.AppRelativeCurrentExecutionFilePath: " + path + "</h3>\r\n");

         RouteData routeData = base.ControllerContext.RequestContext.RouteData;
         RouteValueDictionary values = routeData.Values;
         RouteBase base2 = routeData.Route;

         html.Append(@"
<table class=""grid ui-widget ui-state-default ui-corner-all"">
   <thead>
      <tr>
         <th>Route.Url</th>
         <th>Route.Defaults</th>
         <th>Route.Constraints</th>
         <th>Route.DataTokens</th>
      </tr>
   <thead>");

         html.Append("<tbody class=\"ui-widget-content\">");

         foreach (RouteBase routeBase in RouteTable.Routes)
         {
            Route route = routeBase as Route;

            string url = "-";
            string defaults = "-";
            string constraints = "-";
            string dataTokens = "-";

            if (route != null)
            {
               url = route.Url;
               defaults = FormatRouteValueDictionary(route.Defaults);
               constraints = FormatRouteValueDictionary(route.Constraints);
               dataTokens = FormatRouteValueDictionary(route.DataTokens);
            }

            html.Append("<tr>");
            html.AppendFormat("<td>{0}</td>", url);
            html.AppendFormat("<td>{0}</td>", defaults);
            html.AppendFormat("<td>{0}</td>", constraints);
            html.AppendFormat("<td>{0}</td>", dataTokens);
            html.Append("</tr>");
         }

         html.Append("</tbody></table>");

         return html.ToString();
      }


      private static string FormatRouteValueDictionary(RouteValueDictionary values)
      {
         if (values == null)
         {
            return "(null)";
         }
         string str = string.Empty;

         foreach (string str2 in values.Keys)
         {
            if (str2 == "namespaces")
            {
               string[] str3 = (string[])values[str2];
               string str4 = string.Join(",", str3);
               str = str + string.Format("{0} = {1}, ", str2, str4);
            }
            else
            {
               str = str + string.Format("{0} = {1}, ", str2, values[str2]);
            }
         }

         if (str.EndsWith(", "))
            str = str.Substring(0, str.Length - 2);

         return str;
      }


      #region Post Bulk Insert

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult DoPostBulkInsert(int numberOfPost)
      {
         string[] dummy = new string[6];
         dummy[0] = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
         dummy[1] = "Sed ut perspiciatis unde omnis iste natus error sit <u>voluptatem</u> accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";
         dummy[2] = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat";
         dummy[3] = "<i>Lorem ipsum dolor sit amet</i>, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";
         dummy[4] = "Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.";
         dummy[5] = "<b>At vero eos et accusamus</b> et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. ";

         Random rndPost = new Random(DateTime.Now.Millisecond);
         Random rndDay = new Random(DateTime.Now.Millisecond);
         Random rndMonth = new Random(DateTime.Now.Millisecond);

         ICategoryService categoryService = IoC.Resolve<ICategoryService>();
         ITagService tagService = IoC.Resolve<ITagService>();
         IContentItemService<Post> contentItemService = IoC.Resolve<IContentItemService<Post>>();

         IEnumerable<Category> categories = categoryService.GetAllCategoriesBySite(Context.ManagedSite);
         IList<Tag> tags = tagService.GetAllTagsBySite(Context.ManagedSite);

         int categoryCount;
         int tagCount;

         Random rndCat = new Random(DateTime.Now.Millisecond);
         Random rndTag = new Random(DateTime.Now.Millisecond);


         try
         {
            using (NHTransactionScope tx = new NHTransactionScope())
            {
               // Bulk Insert
               for (int postIndex = 0; postIndex < numberOfPost; postIndex++)
               {
                  int indexPost = rndPost.Next(6);
                  if (indexPost > 5)
                     indexPost = 0;

                  int day = rndDay.Next(1, 29); // get a number of the day between 1 and 29
                  int month = rndMonth.Next(1, 12); // get a number of the day between 1 and 29

                  categoryCount = rndCat.Next(categories.Count());
                  //tagCount = rndCat.Next(tags.Count);
                  tagCount = rndCat.Next(1, 2);

                  DateTime publishedDate = new DateTime(DateTime.Now.Year, month, day);

                  if (publishedDate > DateTime.Now)
                     publishedDate = DateTime.Now.Subtract(new TimeSpan(numberOfPost, 0, 0, 0));
                     

                  Post post = new Post()
                  {
                     Title = dummy[indexPost].StripHtml().GetFirstWords(5) + " @ " + publishedDate.ToShortDateString(),
                     Summary = null,
                     Content = dummy[indexPost],
                     WorkflowStatus = WorkflowStatus.Published,
                     AllowComments = Context.ManagedSite.AllowComments,
                     AllowPings = Context.ManagedSite.AllowPings,
                     Site = Context.ManagedSite,
                     Culture = Context.ManagedSite.DefaultCulture,
                     PublishedBy = Context.CurrentUser,
                     PublishedDate = publishedDate.ToUniversalTime()
                  };


                  if (categoryCount > 0)
                  {
                     foreach (Category item in categories)
                     {
                        if (categoryCount >= 0)
                           post.Categories.Add(item);

                        categoryCount--;
                     }
                  }

                  if (tagCount > 0)
                  {
                     foreach (Tag item in tags)
                     {
                        if (tagCount >= 0)
                           post.Tags.Add(item);

                        tagCount--;
                     }
                  }

                  // Save
                  contentItemService.Save(post);
               }

               // Commit all
               tx.VoteCommit();
            }
            MessageModel model = new MessageModel
            {
               Text = "Bulk Insert Done!!!",
               Icon = MessageModel.MessageIcon.Info,
               CssClass = "margin-topbottom",
               IsClosable = true
            };
            RegisterMessage(model, true);


         }
         catch (Exception ex)
         {
            log.Error(ex.ToString());

            MessageModel model = new MessageModel
            {
               Text = ex.Message,
               Icon = MessageModel.MessageIcon.Alert,
               CssClass = "margin-topbottom",
               IsClosable = true
            };
            RegisterMessage(model, true);
         }

         return View("Index");
      }

      #endregion



      //private string GenerateUrl(string routeName, 
      //                           string actionName, 
      //                           string controllerName, 
      //                           RouteValueDictionary routeValues, 
      //                           RouteCollection routeCollection, 
      //                           RequestContext requestContext, 
      //                           bool includeImplicitMvcValues)
      //{
      //   RouteValueDictionary values = MergeRouteValues(actionName, controllerName, requestContext.RouteData.Values, routeValues, includeImplicitMvcValues);
      //   VirtualPathData data = routeCollection.GetVirtualPath(requestContext, routeName, values);
         
      //   if (data == null)
      //   {
      //      return null;
      //   }

      //   return GenerateClientUrl(requestContext.HttpContext, data.VirtualPath);
      //}



      //public RouteValueDictionary MergeRouteValues(string actionName, string controllerName, RouteValueDictionary implicitRouteValues, RouteValueDictionary routeValues, bool includeImplicitMvcValues)
      //{
      //   RouteValueDictionary dictionary = new RouteValueDictionary();
         
      //   if (includeImplicitMvcValues)
      //   {
      //      object obj2;
      //      if ((implicitRouteValues != null) && implicitRouteValues.TryGetValue("action", out obj2))
      //      {
      //         dictionary["action"] = obj2;
      //      }
      //      if ((implicitRouteValues != null) && implicitRouteValues.TryGetValue("controller", out obj2))
      //      {
      //         dictionary["controller"] = obj2;
      //      }
      //   }

      //   if (routeValues != null)
      //   {
      //      foreach (KeyValuePair<string, object> pair in GetRouteValues(routeValues))
      //      {
      //         dictionary[pair.Key] = pair.Value;
      //      }
      //   }

      //   if (actionName != null)
      //      dictionary["action"] = actionName;

      //   if (controllerName != null)
      //      dictionary["controller"] = controllerName;

      //   return dictionary;
      //}



      //public RouteValueDictionary GetRouteValues(RouteValueDictionary routeValues)
      //{
      //   if (routeValues == null)
      //   {
      //      return new RouteValueDictionary();
      //   }

      //   return new RouteValueDictionary(routeValues);
      //}




      //public string GenerateClientUrl(HttpContextBase httpContext, string contentPath)
      //{
      //   string str;
      //   if (string.IsNullOrEmpty(contentPath))
      //   {
      //      return contentPath;
      //   }
      //   contentPath = StripQuery(contentPath, out str);
      //   return (GenerateClientUrlInternal(httpContext, contentPath) + str);
      //}

   }
}
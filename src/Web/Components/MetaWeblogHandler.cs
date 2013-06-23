using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using Arashi.Services.Content;
using Arashi.Web.Components.MetaWeblogApi;
using CookComputing.XmlRpc;

namespace Arashi.Web.Components
{
   //public class MetaWeblogHandler : XmlRpcService, IMetaWeblog
   //{
   //   private readonly CategoryService categoryService = new CategoryService();
   //   private readonly PostService postService = new PostService();

   //   #region IMetaWeblog Members

   //   /// <summary>
   //   /// Adds the post.
   //   /// </summary>
   //   /// <param name="blogid">The blogid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <param name="post">The post.</param>
   //   /// <param name="publish">if set to <c>true</c> [publish].</param>
   //   /// <returns></returns>
   //   public string AddPost(string blogid, string username, string password, Post post, bool publish)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         ValidateUser(username, password);

   //         try
   //         {

   //            if (post.categories == null)
   //               throw new XmlRpcFaultException(0, "Invalid Category!");

   //            if (post.categories.Length < 1)
   //               throw new XmlRpcFaultException(0, "Invalid Category!");

   //            string[] tags = Utils.RetrieveTagFromBody(post.description);
   //            string body = Utils.RemoveWlwHtmlTags(post.description);

   //            PostBackOfficeDTO nwPost = new PostBackOfficeDTO
   //            {
   //               Title = post.title.DecodeHtml(),
   //               Slug = post.title.SanitizeText(),
   //               Username = username,
   //               FormattedBody = body,
   //               Abstract = post.description.CleanHTMLText().Trim().Replace("&nbsp;", string.Empty).Cut(250),
   //               PublishDate = (post.dateCreated == DateTime.MinValue || post.dateCreated == DateTime.MaxValue) ? DateTime.Now : post.dateCreated,
   //               BreakOnAggregate = post.description.Contains("[more]")
   //            };

   //            IList<string> categories = post.categories.ToList();

   //            nwPost.Publish = true;
   //            nwPost.BreakOnAggregate = body.Contains("[more]");

   //            CategoryDTO cat = new CategoryService().GetByName(categories[0]);

   //            if (cat == null)
   //               throw new XmlRpcFaultException(0, "Invalid Category! Try to refresh the category list");

   //            nwPost.CategoryName = cat.Name;
   //            nwPost.CategoryID = cat.ID;

   //            nwPost.Tags = tags;

   //            postService.SaveOrUpdate(nwPost);

   //            return nwPost.ID.ToString();
   //         }
   //         catch (SecurityException)
   //         {
   //            throw new XmlRpcFaultException(0, "Access Denied please contact the Administrator.");
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// Updates the post.
   //   /// </summary>
   //   /// <param name="postid">The postid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <param name="post">The post.</param>
   //   /// <param name="publish">if set to <c>true</c> [publish].</param>
   //   /// <returns></returns>
   //   public bool UpdatePost(string postid, string username, string password, Post post, bool publish)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         try
   //         {
   //            ValidateUser(username, password);

   //            if (post.categories == null)
   //               throw new XmlRpcFaultException(0, "Invalid Category!");

   //            if (post.categories.Length < 1)
   //               throw new XmlRpcFaultException(0, "Invalid Category!");

   //            string[] tags = Utils.RetrieveTagFromBody(post.description);
   //            string body = Utils.RemoveWlwHtmlTags(post.description);

   //            PostBackOfficeDTO p = postService.GetByIDForBackoffice(postid.ToInt32());

   //            if (p == null)
   //               throw new XmlRpcFaultException(0, "Post not Found!");

   //            p.Title = post.title;
   //            p.Slug = p.Title.SanitizeText();
   //            p.Username = username;
   //            p.FormattedBody = body;
   //            p.PublishDate = (post.dateCreated == DateTime.MinValue || post.dateCreated == DateTime.MaxValue) ? DateTime.Now : post.dateCreated;

   //            IList<string> categories = post.categories.ToList();

   //            p.Publish = true;
   //            p.BreakOnAggregate = body.Contains("[more]");

   //            p.CategoryName = categories[0];

   //            p.Tags = tags;

   //            postService.SaveOrUpdate(p);

   //            return true;

   //         }
   //         catch (SecurityException)
   //         {
   //            throw new XmlRpcFaultException(0, "Access Denied please contact the Administrator.");
   //         }
   //         catch (Exception e)
   //         {
   //            e.Data.Add("Username", username);
   //            e.Data.Add("Title", post.title);
   //            e.Data.Add("Description", post.description);

   //            throw;
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// Gets the post.
   //   /// </summary>
   //   /// <param name="postid">The postid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <returns></returns>
   //   public Post GetPost(string postid, string username, string password)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         ValidateUser(username, password);

   //         try
   //         {
   //            PostDTO item = postService.GetByID(postid.ToInt32());

   //            Post p = new Post
   //            {
   //               wp_slug = item.Slug,
   //               userid = item.Username,
   //               title = item.Title,
   //               permalink = item.Url,
   //               dateCreated = item.PublishDate,
   //               description = item.FormattedBody,
   //               postid = item.ID,
   //               categories = new[] { item.CategoryName }
   //            };

   //            return p;

   //         }
   //         catch (Exception)
   //         {
   //            throw new XmlRpcFaultException(0, "Post Not Found!");
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// Gets the categories.
   //   /// </summary>
   //   /// <param name="blogid">The blogid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <returns></returns>
   //   public CategoryInfo[] GetCategories(string blogid, string username, string password)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         ValidateUser(username, password);

   //         IList<FlatCategoryDTO> categories = categoryService.GetFlatCategories();

   //         return categories.Select(c => new CategoryInfo
   //         {
   //            categoryid = c.ID.ToString(),
   //            title = c.Name,
   //            description = c.Name,
   //            htmlurl = c.Url,
   //            rssUrl = c.FeedUrl

   //         }).ToArray();
   //      }
   //   }

   //   /// <summary>
   //   /// Gets the recent posts.
   //   /// </summary>
   //   /// <param name="blogid">The blogid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <param name="numberOfPosts">The number of posts.</param>
   //   /// <returns></returns>
   //   public Post[] GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         ValidateUser(username, password);
   //         try
   //         {
   //            IList<PostDTO> posts = postService.GetRecent(numberOfPosts);

   //            Post[] items = new Post[posts.Count];

   //            for (int i = 0; i < posts.Count; i++)
   //            {
   //               items[i] = new Post
   //               {
   //                  title = posts[i].Title,
   //                  dateCreated = posts[i].PublishDate,
   //                  description = posts[i].Abstract,
   //                  permalink = posts[i].Url,
   //                  postid = posts[i].ID.ToString(),
   //                  wp_slug = posts[i].Slug,
   //                  categories = new[] { posts[i].CategoryName }
   //               };
   //            }

   //            return items;

   //         }
   //         catch (SecurityException)
   //         {
   //            throw new XmlRpcFaultException(0, "Access Denied please contact the Administrator.");
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// News the media object.
   //   /// </summary>
   //   /// <param name="blogid">The blogid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <param name="mediaObject">The media object.</param>
   //   /// <returns></returns>
   //   public MediaObjectInfo NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         try
   //         {
   //            ValidateUser(username, password);

   //            string newFileName = string.Concat(Guid.NewGuid(), Path.GetExtension(mediaObject.name));
   //            string userFolderName = username.MakeValidFileName();
   //            string targetFolder = Shared.DexterEnvironment.Context.MapPath(Path.Combine(UrlBuilder.ImagePath, userFolderName));

   //            if (!Directory.Exists(targetFolder))
   //               Directory.CreateDirectory(targetFolder);

   //            string filePath = Path.Combine(targetFolder, newFileName);

   //            if (mediaObject.bits != null)
   //            {

   //               using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
   //               {
   //                  using (BinaryWriter bw = new BinaryWriter(fs))
   //                  {
   //                     bw.Write(mediaObject.bits);
   //                  }
   //               }

   //               MediaObjectInfo objectInfo = new MediaObjectInfo
   //               {
   //                  url = string.Concat(UrlBuilder.SiteWithHttp, UrlBuilder.ImagePath, "/", userFolderName, "/", newFileName)
   //               };

   //               return objectInfo;
   //            }

   //            throw new XmlRpcFaultException(0, "Invalid Media");

   //         }
   //         catch (Exception)
   //         {
   //            throw new XmlRpcFaultException(0, "Generic Error");
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// Deletes the post.
   //   /// </summary>
   //   /// <param name="key">The key.</param>
   //   /// <param name="postid">The postid.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <param name="publish">if set to <c>true</c> [publish].</param>
   //   /// <returns></returns>
   //   public bool DeletePost(string key, string postid, string username, string password, bool publish)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         try
   //         {
   //            ValidateUser(username, password);
   //            postService.Delete(postid.ToInt32(0));

   //            return true;

   //         }
   //         catch (SecurityException)
   //         {
   //            throw new XmlRpcFaultException(0, "Access Denied please contact the Administrator.");
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// Gets the users blogs.
   //   /// </summary>
   //   /// <param name="key">The key.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <returns></returns>
   //   public BlogInfo[] GetUsersBlogs(string key, string username, string password)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         try
   //         {

   //            ValidateUser(username, password);
   //            IList<BlogInfo> infoList = new Collection<BlogInfo>();
   //            BlogInfo blogInfo = new BlogInfo
   //            {
   //               blogid = "nothing",
   //               blogName = DexterConfiguration.SiteConfiguration.BlogName,
   //               url = UrlBuilder.SiteWithHttp.ToString()
   //            };

   //            infoList.Add(blogInfo);

   //            return infoList.ToArray();
   //         }
   //         catch (SecurityException)
   //         {
   //            throw new XmlRpcFaultException(0, "Access Denied please contact the Administrator.");
   //         }
   //      }
   //   }

   //   /// <summary>
   //   /// Gets the user info.
   //   /// </summary>
   //   /// <param name="key">The key.</param>
   //   /// <param name="username">The username.</param>
   //   /// <param name="password">The password.</param>
   //   /// <returns></returns>
   //   public UserInfo GetUserInfo(string key, string username, string password)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         ValidateUser(username, password);
   //         UserInfo info = new UserInfo();
   //         MembershipUser usr = Membership.GetUser(username);

   //         info.email = usr.Email;
   //         info.nickname = username;
   //         info.url = UrlBuilder.SiteWithHttp.ToString();

   //         return info;
   //      }
   //   }

   //   public int newCategory(string blogid, string username, string password, WordpressCategory category)
   //   {
   //      using (UnitOfWorkThreadScope.StartUnitOfWork())
   //      {
   //         new CategoryService().SaveOrUpdate(category.name, null, null);

   //         CategoryDTO cat = new CategoryService().GetByName(category.name);

   //         if (cat != null)
   //            return cat.ID;

   //         return 0;
   //      }

   //   }

   //   public int newPage(string blog_id, string username, string password, Post content, bool publish)
   //   {
   //      throw new NotImplementedException();
   //   }

   //   public int editPage(string blog_id, string page_id, string username, string password, Post content, bool publish)
   //   {
   //      throw new NotImplementedException();
   //   }

   //   public Post[] getPages(string blog_id, string username, string password, int numberOfPosts)
   //   {
   //      throw new NotImplementedException();
   //   }

   //   public Post getPage(string blog_id, string page_id, string username, string password)
   //   {
   //      throw new NotImplementedException();
   //   }

   //   public bool deletePage(string blog_id, string username, string password, string page_id)
   //   {
   //      throw new NotImplementedException();
   //   }

   //   #endregion

   //   #region Private Methods

   //   private static void ValidateUser(string username, string password)
   //   {
   //      bool isValid = Membership.ValidateUser(username, password);

   //      if (!isValid)
   //         throw new XmlRpcFaultException(0, "User is not valid!");

   //      string[] roles = Roles.GetRolesForUser(username);

   //      ImpersonateThread(username, roles);
   //   }

   //   private static void ImpersonateThread(string username, string[] roles)
   //   {
   //      IIdentity identity = new GenericIdentity(username);
   //      IPrincipal principal = new GenericPrincipal(identity, roles);

   //      Thread.CurrentPrincipal = principal;
   //   }

   //   #endregion
   //}
}
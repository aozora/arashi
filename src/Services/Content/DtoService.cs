namespace Arashi.Services.Content
{
   using System;
   using System.Collections;
   using System.Collections.Generic;
   using System.Linq;
   using System.Text;

   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Dto;
   using Arashi.Core.Repositories;

   using NHibernate;
   using NHibernate.Criterion;

   using uNhAddIns.Transform;

   public class DtoService : IDtoService
   {
      public DtoService()
      {
      }



      public TemplateContentDTO GetTemplateContentDTO(Site site)
      {
         ISession session = RepositoryHelper.GetSession();
         var transformer = new PositionalToBeanResultTransformer(typeof(TagDTO), new[] { "TagId", "Name", "Count" });
         var transformer2 = new PositionalToBeanResultTransformer(typeof(ContentItemCalendarDTO), new[] { "Year", "Month", "Day", "Count" });

         IMultiQuery multiQuery = session.CreateMultiQuery()
                                          .AddNamedQuery("GetAllCategoriesBySite")
                                          .AddNamedQuery("GetAllTagsBySite")
                                          .Add(
                                             session.GetNamedQuery("GetTagCloud")
                                                .SetResultTransformer(transformer)
                                          )
                                          .Add(
                                             session.GetNamedQuery("GetPostCalendarForPublishedBySite")
                                                .SetDateTime("now", DateTime.Now.ToUniversalTime())
                                                .SetResultTransformer(transformer2)
                                          )
                                          .AddNamedQuery("FindAllPagesBySite")
                                          .Add( 
                                             session.GetNamedQuery("GetRecentComments")
                                                .SetMaxResults(15)
                                          )
                                          .Add( // TODO: move to namedquery
                                             session.GetNamedQuery("GetRecentPosts")
                                                .SetMaxResults(15)
                                          )
                                          .SetEntity("site", site)
                                          .SetEnum("status", WorkflowStatus.Published)
                                          .SetDateTime("date", DateTime.Now.ToUniversalTime())
                                          .SetEnum("type", CommentType.Comment)
                                          .SetEnum("commentstatus", CommentStatus.Approved)
                                          .SetCacheable(true);
         
         // get the multi query result
         IList results = multiQuery.List();

         TemplateContentDTO dto = new TemplateContentDTO
         {
            Categories = ((IList)results[0]).Cast<Category>(),
            Tags = ((IList)results[1]).Cast<Tag>().ToList<Tag>(),
            TagCloud = ((IList)results[2]).Cast<TagDTO>().ToList<TagDTO>(),
            Calendar = ((IList)results[3]).Cast<ContentItemCalendarDTO>().ToList<ContentItemCalendarDTO>(),
            Pages = ((IList)results[4]).Cast<Page>().ToList<Page>(),
            RecentComments = ((IList)results[5]).Cast<Comment>().ToList<Comment>(),
            RecentPosts = ((IList)results[6]).Cast<Post>().ToList<Post>()
         };

         return dto;
      }


   }
}

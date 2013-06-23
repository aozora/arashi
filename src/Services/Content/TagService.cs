using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{
   using System.Collections.Generic;
   using Arashi.Core.Domain;
   using Arashi.Core.Domain.Dto;
   using Arashi.Core.Repositories;
   using Common.Logging;
   using NHibernate;
   using NHibernate.Criterion;
   using uNhAddIns.Transform;
   using System;



   public class TagService : ServiceBase, ITagService
   {
      public TagService(Arashi.Core.NHibernate.ISessionFactory sessionFactory, ILog log)
         : base(sessionFactory, log)
      {
      }

      #region Implementation of ITagService

      public Tag GetById(int tagId)
      {
         return Repository<Tag>.FindById(tagId);
      }



      public Paginator<Tag> GetPaginatorBySite(Site site, int pageSize)
      {
         if (site == null)
            throw new ArgumentNullException("site");

         DetachedCriteria criteria = DetachedCriteria.For<Tag>()
                                       .AddOrder(new Order("Name", false))
                                       .Add(Restrictions.Eq("Site", site));

         return Repository<Tag>.GetPaginator(criteria, pageSize);
      }



      public Tag GetBySiteAndFriendlyName(Site site, string friendlyName)
      {
         //string hql = "from Arashi.Core.Domain.Tag c where c.Site = :site and c.FriendlyName = :name";
         return Session.GetNamedQuery("GetTagBySiteAndFriendlyName")
                           .SetEntity("site", site)
                           .SetString("name", friendlyName)
                           .UniqueResult<Tag>();
      }



      public IList<TagDTO> GetTagCloudBySite(Site site)
      {
//         string hql = @"select t.TagId, t.Name, count(t)
//                        from Tag t 
//                        join t.ContentItems tc
//                        where t.Site = :site
//                        group by t.TagId, t.Name";
         var transformer = new PositionalToBeanResultTransformer(typeof(TagDTO), new[] { "TagId", "Name", "Count" });

         IList<TagDTO> list = Session.GetNamedQuery("GetTagCloud")
                       .SetEntity("site", site)
                       .SetResultTransformer(transformer)
                       .List<TagDTO>();
         return list;
      }



      /// <summary>
      /// Get a list of all the tags of a given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      public IList<Tag> GetAllTagsBySite(Site site)
      {
         //string hql = "from Arashi.Core.Domain.Tag t where t.Site = :site order by t.Name asc";
         return Session.GetNamedQuery("GetAllTagsBySite")
                           .SetEntity("site", site)
                           .List<Tag>();
      }



      /// <summary>
      /// Save or Update a tag
      /// </summary>
      /// <param name="tag"></param>
      public void Save(Tag tag)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
            // Check if the FriendlyName is unique
            long similarCount = GetCountForSimilarFriendlyNameBySite(tag.Site, tag.FriendlyName);

            if (similarCount > 0)
               tag.FriendlyName = tag.FriendlyName + "-" + (similarCount + 1).ToString();

            Repository<Tag>.Save(tag);
         //   tx.VoteCommit();
         //}
      }



      /// <summary>
      /// Delete an existing tag
      /// </summary>
      /// <param name="tag"></param>
      public void Delete(Tag tag)
      {
         //using (NHTransactionScope tx = new NHTransactionScope())
         //{
         Repository<Tag>.Delete(tag);
         //   tx.VoteCommit();
         //}
      }



      public long GetCountForSimilarFriendlyNameBySite(Site site, string friendlyName)
      {
         DetachedCriteria criteria = DetachedCriteria.For<Tag>()
                                       .Add(Restrictions.Eq("Site", site))
                                       .Add(Restrictions.Like("FriendlyName", friendlyName, MatchMode.Start))
                                       .SetProjection(Projections.Count("TagId"));

         ICriteria c = criteria.GetExecutableCriteria(Session);

         return Convert.ToInt64(c.UniqueResult());
      }


      #endregion
   }
}

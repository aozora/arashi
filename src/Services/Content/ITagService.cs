using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;
using Arashi.Core.Domain.Dto;
using uNhAddIns.Pagination;

namespace Arashi.Services.Content
{
   public interface ITagService
   {
      Tag GetById(int tagId);

      /// <summary>
      /// Returns a tag given its friendly name and site
      /// </summary>
      /// <param name="site"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
      Tag GetBySiteAndFriendlyName(Site site, string friendlyName);


      /// <summary>
      /// Get a list of Tag DTO usefull to build a tag cloud
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<TagDTO> GetTagCloudBySite(Site site);



      /// <summary>
      /// Get a list of all the tags of a given site
      /// </summary>
      /// <param name="site"></param>
      /// <returns></returns>
      IList<Tag> GetAllTagsBySite(Site site);


      Paginator<Tag> GetPaginatorBySite(Site site, int pageSize);


      /// <summary>
      /// Save or Update a tag
      /// </summary>
      /// <param name="tag"></param>
      void Save(Tag tag);


      /// <summary>
      /// Delete an existing tag
      /// </summary>
      /// <param name="tag"></param>
      void Delete(Tag tag);



      /// <summary>
      /// Returns the number of posts with a similar friendlyname.
      /// This is used by the Save method to ensure that each post have a unique friendlyname
      /// </summary>
      /// <param name="site"></param>
      /// <param name="friendlyName"></param>
      /// <returns></returns>
      long GetCountForSimilarFriendlyNameBySite(Site site, string friendlyName);

   }

}

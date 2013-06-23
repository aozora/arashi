using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Arashi.Core.Domain;

namespace Arashi.Services.Versioning
{
    public class VersioningService<T> : IVersioningService<T> where T : IContentItem
    {

        #region IVersioningService<T> Members

        public T CreateNewVersion(T entity)
        {
            //1 transform entity to VersionEntry list according to versioningInfo

            //2 load the last version

            //3 compare versions

            //4 save diff
            return entity;
    
        }

        public T SetToVersion(T entity, string version)
        {

            return entity;
        }

        public void DeleteVersion(T entity, string version)
        {
            
        }

        #endregion
     
    }
}

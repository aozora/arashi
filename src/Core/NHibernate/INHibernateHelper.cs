using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// The contract of NHibernate configuration.
   /// </summary>
   public interface INHibernateHelper
   {
      /// <summary>
      /// Gets or sets the session factory.
      /// </summary>
      /// <value>The session factory.</value>
      global::NHibernate.ISessionFactory SessionFactory
      {
         get;
         set;
      }

      /// <summary>
      /// Return the NHibernate configuration.
      /// </summary>
      /// <value>The configuration.</value>
      global::NHibernate.Cfg.Configuration Configuration
      {
         get;
      }

      /// <summary>
      /// Configures NHibernate
      /// </summary>
      void Configure();

      /// <summary>
      /// Gets the session.
      /// </summary>
      /// <returns></returns>
      ISession GetSession();

      ///// <summary>
      ///// Gets a value indicating whether [use cache].
      ///// </summary>
      ///// <value><c>true</c> if NHibernate should cache the queries; otherwise, <c>false</c>.</value>
      //bool UseCache
      //{
      //   get;
      //}

      ///// <summary>
      ///// Checks if the database schema should be updated.
      ///// </summary>
      ///// <returns><c>True</c> if the schema requires the update;  otherwise, <c>false</c>.</returns>
      //bool ShouldUpdateSchema();

      ///// <summary>
      ///// Updates the database schema.
      ///// </summary>
      //void UpdateSchema();

      ///// <summary>
      ///// Initializes the schema.
      ///// </summary>
      //void InitializeSchema();

      ///// <summary>
      ///// Drops the schema.
      ///// </summary>
      //void DropSchema();

      /// <summary>
      ///// Gets the get connection string.
      ///// </summary>
      ///// <value>The get connection string.</value>
      //string GetConnectionString
      //{
      //   get;
      //}
   }
}

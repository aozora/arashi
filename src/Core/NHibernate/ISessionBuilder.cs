using NHibernate;
using NHibernate.Cfg;

namespace Arashi.Core.NHibernate
{
   /// <summary>
   /// Credits: original code from Tarantino.Infrastructure.Commons.DataAccess.ORMapper
   /// </summary>
   public interface ISessionBuilder
   {
      ISessionFactory GetSessionFactory();
      ISessionFactory GetSessionFactory(string configurationFile);
      ISession GetSession();
      Configuration GetConfiguration();
      ISession GetSession(string configurationFile);
      IStatelessSession GetStatelessSession();
      IStatelessSession GetStatelessSession(string configurationFile);
      Configuration GetConfiguration(string configurationFile);
      ISession GetExistingWebSession(string configurationFile);
      ISession GetExistingWebSession();
   }
}
namespace Arashi.Core.NHibernate.Wcf
{
   using global::NHibernate;
   using global::NHibernate.Cfg;

   public static class NHibernateFactory
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory Initialize()
        {
            return Initialize(new Configuration().Configure().BuildSessionFactory());
        }

        public static ISessionFactory Initialize(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
           return _sessionFactory;
        }

        public static ISession OpenSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}
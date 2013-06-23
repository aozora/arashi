namespace Arashi.Core.NHibernate.Wcf.Testing
{
   using System;

   using global::NHibernate;
   using global::NHibernate.Cfg;

   using Arashi.Core.NHibernate.Wcf;

   public class NHibernateTestingContextExtension:INHibernateContextExtension
    {
        private readonly string _hbmXmlFilePath;

        public NHibernateTestingContextExtension(string hbmXmlFilePath)
        {
            _hbmXmlFilePath = hbmXmlFilePath;
        }

        public NHibernateTestingContextExtension()
        {
        }

        private ISession CreateSession()
        {
            var sessionFactory = !string.IsNullOrEmpty(_hbmXmlFilePath) ? 
                new Configuration().Configure(_hbmXmlFilePath).BuildSessionFactory() : 
                new Configuration().Configure().BuildSessionFactory();

            return sessionFactory.OpenSession();
        }

        public ISession Session
        {
            get { return CreateSession(); }
        }

        public void InstanceContextFaulted(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
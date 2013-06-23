namespace Arashi.Core.NHibernate.Wcf
{
   using global::NHibernate;

   public class WcfSessionStorage:ISessionStorage
    {
        public ISession GetSession()
        {
            return NHibernateContext.Current().Session;
        }
    }
}
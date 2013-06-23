namespace Arashi.Core.NHibernate.Wcf.Testing
{
   using global::NHibernate;

   using Arashi.Core.NHibernate.Wcf;

   public class InMemorySessionStorage:ISessionStorage
    {
        private readonly ISession _session;

        public InMemorySessionStorage(ISession session)
        {
            _session = session;
        }

        public ISession GetSession()
        {
            return _session;
        }
    }
}
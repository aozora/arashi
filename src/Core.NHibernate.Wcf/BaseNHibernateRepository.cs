namespace Arashi.Core.NHibernate.Wcf
{
   using System.Collections.Generic;

   using global::NHibernate;

   public class BaseNHibernateRepository<T> where T : IDomain
    {
        private readonly ISessionStorage _sessionStorage;

        public BaseNHibernateRepository(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public ISession Session
        {
            get { return _sessionStorage.GetSession(); }
        }

        public T FetchById(long idToSearchFor)
        {
            return Session.Get<T>(idToSearchFor);
        }

        public IEnumerable<T> FetchAll()
        {
            return Session.CreateCriteria(typeof (T)).List<T>();
        }

        public void SaveOrUpdate(T classToUpdate)
        {
            Session.SaveOrUpdate(classToUpdate);
//            Session.Flush();
        }

        public void Delete(T classToDelete)
        {
            Session.Delete(classToDelete);
        }
    }
}
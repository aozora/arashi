namespace Arashi.Core.NHibernate.Wcf
{
   using global::NHibernate;

   using NHibernate;

   public interface ISessionStorage
    {
        ISession GetSession();
    }
}
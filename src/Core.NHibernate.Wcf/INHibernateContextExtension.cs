namespace Arashi.Core.NHibernate.Wcf
{
   using global::NHibernate;

   public interface INHibernateContextExtension
    {
        ISession Session { get; }
//        void InstanceContextFaulted(object sender, EventArgs e);
    }
}
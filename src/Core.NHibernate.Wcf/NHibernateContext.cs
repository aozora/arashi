namespace Arashi.Core.NHibernate.Wcf
{
   using System.ServiceModel;

   public class NHibernateContext
    {
        public static NHibernateContextExtension Current()
        {
            
            return OperationContext.Current.
                    InstanceContext.Extensions.
                    Find<NHibernateContextExtension>();
        }
    }
}
namespace Arashi.Core.NHibernate.Wcf
{
   using System;
   using System.Diagnostics;
   using System.ServiceModel.Channels;
   using System.ServiceModel.Dispatcher;

   public class NHibernateAutoRollbackErrorHandler:IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //Log.For(this).Error("Error caught in service, NHibernate Session rolled back", error);
           Trace.WriteLine("Error caught in service, NHibernate Session rolled back" + error.ToString());
            NHibernateContext.Current().Rollback();
        }

        public bool HandleError(Exception error)
        {
            return false;
        }
    }
}
namespace Arashi.Core.NHibernate.Wcf
{
   using System.Collections.Generic;
   using System.Linq;
   using System.ServiceModel;
   using System.ServiceModel.Channels;
   using System.ServiceModel.Dispatcher;

   public class NHibernateContextInitializer : IDispatchMessageInspector
    {
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            instanceContext.Extensions.Add(new NHibernateContextExtension(NHibernateFactory.OpenSession()));
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var extensions = OperationContext.Current.InstanceContext.Extensions.FindAll<NHibernateContextExtension>();

            foreach (var extension in extensions)
            {
                OperationContext.Current.InstanceContext.Extensions.Remove(extension);
            }

            var errorHandlers =
                new List<IErrorHandler>(OperationContext.Current.EndpointDispatcher.ChannelDispatcher.ErrorHandlers.
                                            Where(
                                            h => h.GetType() == typeof (NHibernateAutoRollbackErrorHandler)));

            foreach (var errorHandler in errorHandlers)
            {
                OperationContext.Current.EndpointDispatcher.ChannelDispatcher.ErrorHandlers.Remove(errorHandler);
            }
        }
    }
}
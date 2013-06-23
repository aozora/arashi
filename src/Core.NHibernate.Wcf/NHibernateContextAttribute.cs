namespace Arashi.Core.NHibernate.Wcf
{
   using System;
   using System.Collections.ObjectModel;
   using System.ServiceModel;
   using System.ServiceModel.Channels;
   using System.ServiceModel.Description;
   using System.ServiceModel.Dispatcher;

   using global::NHibernate;

   public enum Rollback
    {
        Automatically,
        Manually
    }

    public class NHibernateContextAttribute : Attribute, IServiceBehavior
    {
        private readonly Rollback _rollback;
        

        public NHibernateContextAttribute() : this(Rollback.Manually)
        {
        }

        public NHibernateContextAttribute(Rollback rollback)
        {
            _rollback = rollback;
        }

        public ISessionFactory SessionFactory { private get; set; }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
                                         Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (var endpoint in channelDispatcher.Endpoints)
                {
                    endpoint.DispatchRuntime.MessageInspectors.Add(new NHibernateContextInitializer());
                }

                if (_rollback == Rollback.Automatically)
                {
                    channelDispatcher.ErrorHandlers.Add(new NHibernateAutoRollbackErrorHandler());
                }
            }
        }
    }
}
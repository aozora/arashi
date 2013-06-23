namespace Arashi.Core.NHibernate.Wcf
{
   using System.ServiceModel;

   using global::NHibernate;

   public class NHibernateContextExtension : IExtension<InstanceContext>, INHibernateContextExtension
    {
        private ITransaction _transaction;

        public NHibernateContextExtension(ISession session)
        {
            Session = session;
        }

        public void Rollback()
        {
            if (_transaction != null && _transaction.IsActive && !_transaction.WasRolledBack &&
                !_transaction.WasCommitted)
            {
                _transaction.Rollback();
            }
        }

        public ISession Session { get; private set; }

        public void Attach(InstanceContext owner)
        {
            _transaction = Session.BeginTransaction();
        }

        public void Detach(InstanceContext owner)
        {
            if (_transaction != null && !_transaction.WasRolledBack && !_transaction.WasCommitted &&
                _transaction.IsActive)
            {
                _transaction.Commit();
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }
    }
}
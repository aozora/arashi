namespace Arashi.Core.NHibernate.Wcf
{
   public class BaseDomain : IDomain
    {
        public long Id { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == this.GetType() && base.Equals(obj);
        }
    }
}
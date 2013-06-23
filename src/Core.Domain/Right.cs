namespace Arashi.Core.Domain
{
   /// <summary>
   /// Represents a right that a related user is allowed to perform 
   /// (for example 'CanAccessSiteManager')
   /// </summary>
   public class Right
   {
      private int id;

      /// <summary>
      /// ID
      /// </summary>
      public virtual int Id
      {
         get { return id; }
         set { id = value; }
      }

      /// <summary>
      /// Name of the right.
      /// </summary>
      public virtual string Name { get; set; }

      /// <summary>
      /// Description of the right.
      /// </summary>
      public virtual string Description { get; set; }

      /// <summary>
      /// A simple group name. Used only by the admin UI to group different sets of rights
      /// </summary>
      public virtual string RightGroup { get; set; }



      #region Constructor

      /// <summary>
      /// Constructor.
      /// </summary>
      public Right()
      {
         this.id = -1;
      }

      #endregion

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Right right = other as Right;
         if (right == null)
            return false;
         if (Id != right.Id)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = Id.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}
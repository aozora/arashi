using System;

namespace Arashi.Core.Domain
{
	/// <summary>
	/// Base class for permission related association objects.
	/// </summary>
	public interface IPermission
   {

      int Id
      {
         get;
         set;
      }

      bool ViewAllowed
      {
         get;
         set;
      }

      bool EditAllowed
      {
         get;
         set;
      }

      bool DeleteAllowed
      {
         get;
         set;
      }

      Role Role
      {
         get;
         set;
      }
   
   }
}

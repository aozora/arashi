using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Arashi.Core.Domain
{
	/// <summary>
	/// Summary description for Role.
	/// </summary>
	public class Role
	{
		private int roleId;
	   private Site site;
		private string name;
      //private int permissionLevel;
      //private AccessLevel[] permissions;
      private IList<Right> rights;

      #region Public Properties

      /// <summary>
		/// Property Id (int)
		/// </summary>
		public virtual int RoleId
		{
			get { return roleId; }
         set
         {
            roleId = value;
         }
		}

      public virtual Site Site
      {
         get
         {
            return site;
         }
         set
         {
            site = value;
         }
      }

		/// <summary>
		/// Property Name (string)
		/// </summary>
		public virtual string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

      ///// <summary>
      ///// Property PermissionLevel (int). 
      ///// When set, the integer value is translated to a list of AccessLevel enums (Permissions).
      ///// (Is the sum of one or more AccessLevel)
      ///// </summary>
      //[Obsolete("PermissionLevel is deprecated and replaced by the Rights collection.")]
      //public virtual int PermissionLevel
      //{
      //   get { return this.permissionLevel; }
      //   set 
      //   { 
      //      this.permissionLevel = value; 
      //      TranslatePermissionLevelToAccessLevels();
      //   }
      //}

      ///// <summary>
      ///// Gets a list of translated AccessLevel enums of the Role.
      ///// </summary>
      //public virtual AccessLevel[] Permissions
      //{
      //   get { return this.permissions; }
      //}

      //public virtual string PermissionsString
      //{
      //   get { return GetPermissionsAsString(); }
      //}

      /// <summary>
      /// Gets or sets a list of access rights.
      /// </summary>
      public virtual IList<Right> Rights
      {
         get { return rights; }
         set { rights = value; }
      }


      /// <summary>
      /// 
      /// </summary>
      public virtual string RightsString
      {
         get { return GetRightsAsString(); }
      }

      #endregion


      /// <summary>
		/// Default constructor.
		/// </summary>
		public Role()
		{
			roleId = -1;
			name = null;
         rights = new List<Right>();
		}



      private string GetRightsAsString()
      {
         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < rights.Count; i++)
         {
            Right right = rights[i];
            sb.Append(right.Name);
            if (i < rights.Count - 1)
            {
               sb.Append(", ");
            }
         }

         return sb.ToString();
      }



      //private void TranslatePermissionLevelToAccessLevels()
      //{
      //   ArrayList permissions = new ArrayList();
      //   AccessLevel[] accessLevels = (AccessLevel[])Enum.GetValues(typeof(AccessLevel));

      //   foreach (AccessLevel accesLevel in accessLevels)
      //   {
      //      if ((this.PermissionLevel & (int)accesLevel) == (int)accesLevel)
      //      {
      //         permissions.Add(accesLevel);
      //      }
      //   }
      //   this.permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
      //}



      //private string GetPermissionsAsString()
      //{
      //   StringBuilder sb = new StringBuilder();

      //   for (int i = 0; i < this.permissions.Length; i++)
      //   {
      //      AccessLevel accessLevel = this.permissions[i];
      //      sb.Append(accessLevel.ToString());
      //      if (i < this.permissions.Length - 1)
      //      {
      //         sb.Append(", ");
      //      }
      //   }

      //   return sb.ToString();
      //}

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         Role role = other as Role;
         if (role == null)
            return false;
         if (RoleId != role.RoleId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = RoleId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

	}
}

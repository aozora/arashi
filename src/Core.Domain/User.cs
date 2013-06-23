using System;
using System.Collections;
using System.Security.Principal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Arashi.Core.Domain.Validation;


namespace Arashi.Core.Domain
{
   /// <summary>
   /// Summary description for User.
   /// </summary>
   public class User : IPrincipal, IIdentity
   {
      #region Private Fields

      private int userId;
      private string displayName;
      private bool isAuthenticated; // IIdentity field
      private IList<Right> rights;

      #endregion

      #region Public Properties

      /// <summary>
      /// Property Id (int)
      /// </summary>
      public virtual int UserId
      {
         get
         {
            return userId;
         }
         set
         {
            userId = value;
         }
      }

      public virtual Site Site { get; set; }

      /// <summary>
      /// DisplayName
      /// If by chance is null, the left part of the email is displayed
      /// </summary>
      public virtual string DisplayName
      {
         get
         {
            if (string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(this.Email))
               return this.Email.Substring(0, this.Email.IndexOf('@') - 1);
            else
               return displayName;
         }
         set
         {
            displayName = value;
         }
      }


      
      [Required(ErrorMessage = "You must specify the password.")]
      [StringRange(8, 100, ErrorMessage = "The password length must be between 8 and 100 chars.")]
      public virtual string Password
      {
         get;
         set;
      }

      /// <summary>
      /// Password confirmation.
      /// </summary>
      [Required(ErrorMessage = "You must confirm the password.")]
      [StringRange(8, 100, ErrorMessage = "The password confirmation length must be between 8 and 100 chars.")]
      public virtual string PasswordConfirmation { get; set; }



      /// <summary>
      /// Property FirstName (string)
      /// </summary>
      [StringRange(1, 100, ErrorMessage = "The first name length must be between 1 and 100 chars.")]
      public virtual string FirstName
      {
         get;
         set;
      }


      /// <summary>
      /// Property LastName (string)
      /// </summary>
      [StringRange(1, 100, ErrorMessage = "The last name length must be between 1 and 100 chars.")]
      public virtual string LastName
      {
         get;
         set;
      }

      /// <summary>
      /// The full name of the user. This can be used for display purposes. 
      /// If there is no firstname and lastname, the username will be returned.
      /// </summary>
      public virtual string FullName
      {
         get
         {
            return (DisplayName + " " + string.Join(" ", new string[] {LastName, FirstName})).Trim();
         }
      }


      public virtual string Description { get; set; }


      [Required(ErrorMessage = "Email address is missing.")]
      [Email(ErrorMessage = "Invalid e-mail address.")]
      public virtual string Email
      {
         get;
         set;
      }

      public virtual int TimeZone { get; set; }
      public virtual string WebSite { get; set; }
      public virtual bool IsActive { get; set; }
      public virtual DateTime? LastLogin { get; set; }
      public virtual string LastIp { get; set; }
      public virtual string PasswordQuestion { get; set; }
      public virtual string PasswordAnswer { get; set; }
      public virtual int? FailedPasswordAttemptCount { get; set; }
      public virtual string FailedPasswordAttemptWindowStart { get; set; }
      public virtual int? FailedPasswordAnswerAttemptCount { get; set; }
      public virtual string FailedPasswordAnswerAttemptWindowStart { get; set; }

      /// <summary>
      /// The theme applied to the control panel pages
      /// </summary>
      public virtual string AdminTheme { get; set; }

      /// <summary>
      /// Default culture for the control panel pages
      /// </summary>
      public virtual string AdminCulture { get; set; }

      /// <summary>
      /// Property Roles (IList)
      /// </summary>
      //[ValidateCollectionNotEmpty("RolesValidatorNotEmpty")]
      public virtual IList<Role> Roles { get; set; }
      public virtual DateTime CreatedDate { get; set; }
      public virtual DateTime? UpdatedDate { get; set; }
      

      /// <summary>
      /// Returns a list of all Rights in the user roles
      /// </summary>
      public virtual IList<Right> Rights
      {
         get
         {
            if (rights == null)
            {
               rights = new List<Right>();

               foreach (Role role in Roles)
               {
                  foreach (Right right in role.Rights)
                  {
                     if (!rights.Contains(right))
                     {
                        rights.Add(right);
                     }
                  }
               }
            }
            return rights;
         }
      }
      public virtual bool IsLogicallyDeleted { get; set; }

      protected virtual int Version {get; set;}



      public virtual string GetAuthorUrl()
      {
         const string defaultUrlFormat = "author/{0}/";

         return String.Format(defaultUrlFormat, this.DisplayName);
      }

      #endregion

      #region IIdentity Implementation

      /// <summary>
      /// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
      /// </summary>
      public virtual bool IsAuthenticated
      {
         get
         {
            return isAuthenticated;
         }
         set
         {
            isAuthenticated = value;
         }
      }



      /// <summary>
      /// IIdentity property. 
      /// <remark>Returns a string with the Id of the user and not the username</remark>
      /// </summary>
      public virtual string Name
      {
         get
         {
            if (isAuthenticated)
               return userId.ToString();
            else
               return "";
         }
      }



      /// <summary>
      /// IIdentity property <see cref="System.Security.Principal.IIdentity" />.
      /// </summary>
      public virtual string AuthenticationType
      {
         get
         {
            return "ArashiAuthentication";
         }
      }

      #endregion

      #region Implementation of IPrincipal

      /// <summary>
      /// Determines whether the current principal belongs to the specified role.
      /// </summary>
      /// <returns>
      /// true if the current principal is a member of the specified role; otherwise, false.
      /// </returns>
      /// <param name="roleName">
      /// The name of the role for which to check membership. 
      /// </param>
      public virtual bool IsInRole(string roleName)
      {
         foreach (Role role in this.Roles)
         {
            if (role.Name == roleName)
            {
               return true;
            }
         }
         return false;
      }



      /// <summary>
      /// Gets the identity of the current principal.
      /// </summary>
      /// <returns>
      /// The <see cref="T:System.Security.Principal.IIdentity" /> object associated with the current principal.
      /// </returns>
      public virtual IIdentity Identity
      {
         get
         {
            return this;
         }
      }

      #endregion

      #region Constructor

      /// <summary>
      /// Default constructor.
      /// </summary>
      public User()
      {
         userId = -1;
         isAuthenticated = false;
         //permissions = new AccessLevel[0];
         Roles = new List<Role>(); 
         // Default to now, otherwise NHibernate tries to insert a NULL.
         CreatedDate = DateTime.Now;
         AdminTheme = "arashi";
      }

      #endregion

      #region Equality
      
      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         User user = other as User;
         if (user == null)
            return false;
         if (UserId != user.UserId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result;
            result = UserId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}

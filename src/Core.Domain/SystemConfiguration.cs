using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arashi.Core.Domain
{
   public class SystemConfiguration
   {
      private int systemConfigurationId;

      #region Public Properties

      public virtual int SystemConfigurationId
      {
         get
         {
            return systemConfigurationId;
         }
         set
         {
            systemConfigurationId = value;
         }
      }

      public virtual string SmtpHost {get; set;}
      public virtual int SmtpHostPort {get; set;}
      public virtual bool SmtpRequireSSL {get; set;}
      public virtual string SmtpUserName {get; set;}
      public virtual string SmtpUserPassword {get; set;}
      public virtual string SmtpDomain {get; set;}

      #endregion

      public SystemConfiguration()
      {
         systemConfigurationId = -1;
         SmtpHostPort = 25;
         SmtpRequireSSL = false;
      }

      #region Equality

      public override bool Equals(object other)
      {
         if (this == other)
            return true;
         SystemConfiguration c = other as SystemConfiguration;
         if (c == null)
            return false;
         if (SystemConfigurationId != c.SystemConfigurationId)
            return false;
         return true;
      }



      public override int GetHashCode()
      {
         unchecked
         {
            int result = SystemConfigurationId.GetHashCode();
            result = 29 * result;
            return result;
         }
      }

      #endregion

   }
}

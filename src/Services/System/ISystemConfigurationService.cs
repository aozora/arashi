using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arashi.Core.Domain;

namespace Arashi.Services.SystemService
{
   /// <summary>
   /// ISystemConfigurationService
   /// </summary>
   public interface ISystemConfigurationService
   {
      /// <summary>
      /// Get the instance of current system configuration
      /// </summary>
      /// <returns></returns>
      SystemConfiguration Get();


      /// <summary>
      /// Save a system configuration
      /// </summary>
      /// <param name="systemConfiguration"></param>
      void Save(SystemConfiguration systemConfiguration);

   }
}

#region Disclaimer/Info

/////////////////////////////////////////////////////////////////////////////////////////////////
//
//   File:		AutoMockingFacility.cs
//   Website:		http://dexterblogengine.com/
//   Authors:		http://dexterblogengine.com/About.ashx
//   Rev:		1
//   Created:		22/11/2010
//   Last edit:		19/01/2011
//   License:		GNU Library General Public License (LGPL)
//   File:            AutoMockingFacility.cs
//   For updated news and information please visit http://dexterblogengine.com/
//   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
//   For any question contact info@dexterblogengine.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using Castle.Core.Configuration;
using Castle.MicroKernel;

namespace Arashi.UnitTests.AutoMockingContainer
{
   public class AutoMockingFacility : IFacility
   {
      readonly IAutoMockingRepository relatedRepository;
      AutoMockingDependencyResolver autoMockingDependencyResolver;

      public AutoMockingFacility(IAutoMockingRepository relatedRepository)
      {
         this.relatedRepository = relatedRepository;
      }

      #region IFacility Members

      public void Init(IKernel kernel, IConfiguration facilityConfig)
      {
         autoMockingDependencyResolver = new AutoMockingDependencyResolver(relatedRepository);
         kernel.Resolver.AddSubResolver(autoMockingDependencyResolver);
      }

      public void Terminate()
      {
      }

      #endregion
   }
}

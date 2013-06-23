#region Disclaimer/Info

/////////////////////////////////////////////////////////////////////////////////////////////////
//
//   File:		IAutoMockingRepository.cs
//   Website:		http://dexterblogengine.com/
//   Authors:		http://dexterblogengine.com/About.ashx
//   Rev:		1
//   Created:		22/11/2010
//   Last edit:		19/01/2011
//   License:		GNU Library General Public License (LGPL)
//   File:            IAutoMockingRepository.cs
//   For updated news and information please visit http://dexterblogengine.com/
//   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
//   For any question contact info@dexterblogengine.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using Castle.Core;
using Castle.Windsor;

namespace Arashi.UnitTests.AutoMockingContainer
{
   public interface IAutoMockingRepository : IWindsorContainer
   {
      MockingStrategy GetStrategyFor(DependencyModel model);
      void AddStrategy(Type serviceType, MockingStrategy strategy);
      void OnMockCreated<T>(Object mock, String dependencyName);

      /// <summary>
      /// </summary>
      /// <param name = "type"></param>
      /// <param name = "mock">The mock.</param>
      /// <param name = "dependencyName">Name of the dependency.</param>
      void OnMockCreated(Type type, Object mock, String dependencyName);

      /// <summary>
      /// If false the container will not populate properties with mock.
      /// </summary>
      /// <value><c>true</c> if [resolve properties]; otherwise, <c>false</c>.</value>
      Boolean ResolveProperties
      {
         get;
         set;
      }

      /// <summary>
      /// Determines whether this instance [can satisfy dependency key] the specified dependency key.
      /// </summary>
      /// <param name="dependencyKey">The dependency key.</param>
      /// <returns>
      /// 	<c>true</c> if this instance [can satisfy dependency key] the specified dependency key; otherwise, <c>false</c>.
      /// </returns>
      Boolean CanSatisfyDependencyKey(String dependencyKey);
   }
}

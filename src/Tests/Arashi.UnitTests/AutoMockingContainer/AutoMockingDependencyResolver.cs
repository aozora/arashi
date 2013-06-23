#region Disclaimer/Info

/////////////////////////////////////////////////////////////////////////////////////////////////
//
//   File:		AutoMockingDependencyResolver.cs
//   Website:		http://dexterblogengine.com/
//   Authors:		http://dexterblogengine.com/About.ashx
//   Rev:		1
//   Created:		22/11/2010
//   Last edit:		19/01/2011
//   License:		GNU Library General Public License (LGPL)
//   File:            AutoMockingDependencyResolver.cs
//   For updated news and information please visit http://dexterblogengine.com/
//   Dexter is hosted to Codeplex at http://dexterblogengine.codeplex.com
//   For any question contact info@dexterblogengine.com
//
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Rhino.Mocks;

namespace Arashi.UnitTests.AutoMockingContainer
{

   public class AutoMockingDependencyResolver : ISubDependencyResolver
   {
      readonly IAutoMockingRepository relatedRepository;

      public AutoMockingDependencyResolver(IAutoMockingRepository relatedRepository)
      {
         this.relatedRepository = relatedRepository;
      }

      #region ISubDependencyResolver Members

      public bool CanResolve(
       CreationContext context,
       ISubDependencyResolver parentResolver,
       ComponentModel model,
       DependencyModel dependency)
      {
         bool shouldResolveDependencyKey =
             dependency.DependencyType == DependencyType.Service &&
             relatedRepository.CanSatisfyDependencyKey(dependency.DependencyKey);

         Boolean resolveIfProperty =
             !dependency.IsOptional ||
             relatedRepository.ResolveProperties;

         return shouldResolveDependencyKey && resolveIfProperty;
      }

      public object Resolve(
         CreationContext context,
         ISubDependencyResolver parentResolver,
         ComponentModel model,
         DependencyModel dependency)
      {
         MockingStrategy strategy = relatedRepository.GetStrategyFor(dependency);

         if (strategy.Instance != null)
         {
            return strategy.Instance;
         }
         if (strategy.Mock == MockingStrategyType.Mock)
         {
            //if a dependencywas already registered in the main controller, go and use it
            IHandler registration = relatedRepository.Kernel.GetHandler(dependency.TargetType);
            object resolvedDependencyObject = registration == null
                                                ? MockRepository.GenerateStub(dependency.TargetType)
                                                : relatedRepository.Resolve(dependency.TargetType);

            relatedRepository.OnMockCreated(dependency.TargetType, resolvedDependencyObject, dependency.DependencyKey);
            return resolvedDependencyObject;
         }
         if (strategy.Mock == MockingStrategyType.Resolve)
         {
            return relatedRepository.Resolve(dependency.TargetType);
         }

         return null;
      }

      #endregion
   }
}

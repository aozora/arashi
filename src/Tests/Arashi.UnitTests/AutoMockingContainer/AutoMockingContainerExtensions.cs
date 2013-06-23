using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Rhino.Mocks;

namespace Arashi.UnitTests.AutoMockingContainer
{
   public static class AutoMockingContainerExtensions
   {
      public static AutoMockingContainer AutoMockingContainer(this TestBase fixture)
      {
         return fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.CONTAINER_KEY);
      }

      public static T GetFirstCreatedMock<T>(this TestBase fixture)
      {
         return fixture.AutoMockingContainer().GetFirstCreatedMock<T>();
      }

      public static void AssertWasCalledOnAutoMock<T>(this TestBase fixture, Action<T> action)
      {
         fixture.AutoMockingContainer().GetFirstCreatedMock<T>()
            .AssertWasCalled(action);
      }

      public static T ResolveWithAutomock<T>(this TestBase fixture)
      {
         var container = fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.CONTAINER_KEY);
         return container.Resolve<T>();
      }

      public static T ResolveWithAutomock<T>(this TestBase fixture, IDictionary arguments)
      {
         var container = fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.CONTAINER_KEY);
         return container.Resolve<T>(arguments);
      }


      public static T GetMock<T>(this TestBase fixture)
      {
         var container = fixture.GetFromTestContext<AutoMockingContainer>(AutoMockingContainerHelper.CONTAINER_KEY);
         return container.GetFirstCreatedMock<T>();
      }
   }


   public class AutoMockingContainerHelper //: ITestHelper
   {
      internal const String CONTAINER_KEY = "AutoMockingContainerHelper_container";

      public Type[] Types
      {
         get;
         set;
      }

      public string[] IgnoreDependencies
      {
         get;
         set;
      }

      public bool ResolveProperties
      {
         get;
         set;
      }

      #region ITestHelper Members

      public void FixtureSetUp(TestBase fixture)
      {
      }

      public void SetUp(TestBase fixture)
      {
         var container = new AutoMockingContainer();
         fixture.DisposeAtTheEndOfTest(container);
         fixture.SetIntoTestContext(CONTAINER_KEY, container);
         //fixture.DisposeAtTheEndOfTest(DexterContainer.OverrideEngine(new CastleDexterContainer(container)));
         fixture.DisposeAtTheEndOfTest(container);

         foreach (Type type in Types)
         {
            container.Register(Component
                                    .For(type)
                                    .ImplementedBy(type)
                                    .LifeStyle.Transient);
         }

         container.ResolveProperties = ResolveProperties;

         if (IgnoreDependencies != null)
         {
            foreach (var ignoreDependency in IgnoreDependencies)
            {
               container.DependencyToIgnore.Add(ignoreDependency);
            }
         }
      }

      public void TearDown(TestBase fixture)
      {
      }

      public void FixtureTearDown(TestBase fixture)
      {
      }

      #endregion
   }

   //public class UseAutoMockingContainerAttribute : HelperAttributeBase, ITestHelperAttribute
   //{

   //   private Type[] Types
   //   {
   //      get;
   //      set;
   //   }

   //   public UseAutoMockingContainerAttribute(Type[] types)
   //   {
   //      Types = types;
   //      ResolveProperties = true;
   //   }

   //   public UseAutoMockingContainerAttribute()
   //      : this(Type.EmptyTypes)
   //   {

   //   }

   //   #region ITestHelperAttribute Members

   //   public ITestHelper Create()
   //   {
   //      AutoMockingContainerHelper autoMockingContainerHelper = new AutoMockingContainerHelper()
   //      {
   //         Types = this.Types
   //      };
   //      autoMockingContainerHelper.IgnoreDependencies = IgnoreDependencies;
   //      autoMockingContainerHelper.ResolveProperties = ResolveProperties;
   //      return autoMockingContainerHelper;
   //   }

   //   public String[] IgnoreDependencies
   //   {
   //      get;
   //      set;
   //   }

   //   public Boolean ResolveProperties
   //   {
   //      get;
   //      set;
   //   }
   //   #endregion
   //}

}

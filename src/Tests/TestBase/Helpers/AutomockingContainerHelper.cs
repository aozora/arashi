
namespace TestBase.Helpers
{
   #region Usings

   using System;
   using Castle.MicroKernel.Registration;
   using AutoMockingContainer;
   using Arashi.Core;

   //using Dexter.Castle;

   #endregion

   public class AutomockingContainerHelper : ITestHelper
   {
      public const String AutomockContainer = "AutomockContainer";

      public Type SutType
      {
         get;
         set;
      }

      public AutomockingContainerHelper()
      {
      }

      public AutomockingContainerHelper(Type sutType)
      {
         SutType = sutType;
      }

      #region ITestHelper Members

      public void FixtureSetUp()
      {
      }

      public void SetUp(BaseTestFixture fixture)
      {
         AutoMockingContainer container = new AutoMockingContainer();
         fixture.SetIntoTestContext(AutomockContainer, container);
         //fixture.DisposeAtTheEndOfTest(IoC.OverrideEngine(new CastleIoC(container)));
         fixture.DisposeAtTheEndOfTest( IoC.Container );
         
         if (SutType != null)
            container.Register(Component.For(SutType).LifeStyle.Transient);
      }

      public void TearDown(BaseTestFixture fixture)
      {
         fixture.RemoveFromTestContext(AutomockContainer);
      }

      public void FixtureTearDown()
      {
      }

      #endregion
   }



   [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
   public class UseAutomockingContainerAttribute : Attribute, ITestHelperAttribute
   {
      #region ITestHelperAttribute Members

      public Type SutType
      {
         get;
         set;
      }

      public ITestHelper Create()
      {
         return new AutomockingContainerHelper(SutType);
      }

      #endregion
   }



   public static class HelperMethods
   {
      public static AutoMockingContainer GetAutoMockingContainer(this BaseTestFixture fixture)
      {
         return fixture.GetFromTestContext<AutoMockingContainer>(AutomockingContainerHelper.AutomockContainer);
      }
   }
}